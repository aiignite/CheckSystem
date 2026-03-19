using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CommonUtility.BusLoader;
using CommonUtility.SqlSugar;
using CommonUtility.Tools;
using SqlSugar;

namespace CheckSystem.Controller
{
    /// <summary>
    /// 优化后的无线充电模块控制器
    /// 实现线程安全、低CPU内存开销和稳定通讯
    /// </summary>
    public class OptimizedWirelessChargingModule : ControllerBase
    {
        #region 私有字段
        
        // 线程安全的组件
        private readonly ThreadSafeBufferManager _bufferManager;
        private readonly ThreadSafeCanMessageHandler _canMessageHandler;
        private readonly CommunicationStabilityManager _communicationManager;
        private readonly HighPerformanceDbOperations _dbOperations;
        
        // 对象池
        private readonly ObjectPool<CanMsg> _canMsgPool;
        private readonly ObjectPool<List<byte>> _listPool;
        
        // 同步原语
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        
        // CAN总线
        private readonly CanBus _canBus;
        
        // 数据库
        private readonly string _sqliteConnectionString;
        private readonly string _sqlServerConnectionString;
        
        // 配置参数
        private readonly ModuleConfiguration _config;
        
        #endregion
        
        #region 构造函数
        
        public OptimizedWirelessChargingModule(
            CanBus canBus,
            string sqliteConnectionString,
            string sqlServerConnectionString,
            ModuleConfiguration config = null)
        {
            _canBus = canBus ?? throw new ArgumentNullException(nameof(canBus));
            _sqliteConnectionString = sqliteConnectionString ?? throw new ArgumentNullException(nameof(sqliteConnectionString));
            _sqlServerConnectionString = sqlServerConnectionString ?? throw new ArgumentNullException(nameof(sqlServerConnectionString));
            _config = config ?? new ModuleConfiguration();
            
            // 初始化线程安全组件
            _bufferManager = new ThreadSafeBufferManager();
            _canMessageHandler = new ThreadSafeCanMessageHandler(_bufferManager);
            _communicationManager = new CommunicationStabilityManager(_canBus, new ConsoleLogger());
            _dbOperations = new HighPerformanceDbOperations(_sqlServerConnectionString);
            
            // 初始化对象池
            _canMsgPool = new ObjectPool<CanMsg>(() => new CanMsg(), msg => ResetCanMsg(msg));
            _listPool = new ObjectPool<List<byte>>(() => new List<byte>(), list => list.Clear());
            
            // 注册CAN消息处理器
            _canBus.OnCanMsgReceived += _canMessageHandler.OnCanMsgReceived;
            
            // 初始化数据库
            InitializeDatabase();
        }
        
        #endregion
        
        #region 公共方法
        
        /// <summary>
        /// 启动模块
        /// </summary>
        public async Task<bool> StartAsync()
        {
            try
            {
                // 启动CAN总线
                if (!_canBus.StartCan())
                {
                    throw new CommunicationException("Failed to start CAN bus");
                }
                
                // 启动主工作循环
                await StartMainWorkLoopAsync(_cancellationTokenSource.Token);
                
                return true;
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError($"Failed to start module: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// 停止模块
        /// </summary>
        public async Task StopAsync()
        {
            try
            {
                // 取消所有操作
                _cancellationTokenSource.Cancel();
                
                // 停止CAN总线
                _canBus.StopCan();
                
                // 等待所有操作完成
                await Task.Delay(1000);
                
                // 释放资源
                Dispose();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError($"Error stopping module: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 读取ECU信息（优化版本）
        /// </summary>
        public async Task<byte[]> ReadEcuInfoAsync(EcuInfoType infoType, CancellationToken cancellationToken = default)
        {
            await _semaphore.WaitAsync(cancellationToken);
            try
            {
                byte[] did = GetDidForEcuInfoType(infoType);
                
                // 使用通讯稳定性管理器发送和接收消息
                var sendMsg = _canMsgPool.Get();
                try
                {
                    sendMsg.CanId = 0x7E0;
                    sendMsg.Dlc = 2;
                    sendMsg.Data = new byte[] { 0x22, did };
                    
                    var response = await _communicationManager.SendAndReceiveWithRetryAsync(
                        sendMsg, 
                        0x7E8, 
                        TimeSpan.FromMilliseconds(_config.DefaultTimeoutMs), 
                        cancellationToken);
                    
                    // 处理响应数据
                    return ProcessEcuInfoResponse(response.Data, infoType);
                }
                finally
                {
                    _canMsgPool.Return(sendMsg);
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }
        
        /// <summary>
        /// 写入并验证DDI（优化版本）
        /// </summary>
        public async Task<bool> WriteAndVerifyDdiAsync(byte ddi, byte[] data, CancellationToken cancellationToken = default)
        {
            await _semaphore.WaitAsync(cancellationToken);
            try
            {
                // 使用ArrayPool租用数组，避免内存分配
                byte[] buffer = ArrayPool<byte>.Shared.Rent(data.Length + 2);
                try
                {
                    buffer[0] = 0x2E; // 写入DID服务ID
                    buffer[1] = ddi;
                    Array.Copy(data, 0, buffer, 2, data.Length);
                    
                    var sendMsg = _canMsgPool.Get();
                    try
                    {
                        sendMsg.CanId = 0x7E0;
                        sendMsg.Dlc = (byte)(data.Length + 2);
                        sendMsg.Data = buffer;
                        
                        // 发送写入请求
                        var writeResponse = await _communicationManager.SendAndReceiveWithRetryAsync(
                            sendMsg, 
                            0x7E8, 
                            TimeSpan.FromMilliseconds(_config.DefaultTimeoutMs), 
                            cancellationToken);
                        
                        // 验证写入响应
                        if (!IsValidWriteResponse(writeResponse.Data))
                        {
                            return false;
                        }
                        
                        // 读取验证
                        var readResponse = await ReadDdiAsync(ddi, cancellationToken);
                        if (readResponse == null || readResponse.Length != data.Length)
                        {
                            return false;
                        }
                        
                        // 比较数据
                        for (int i = 0; i < data.Length; i++)
                        {
                            if (readResponse[i] != data[i])
                            {
                                return false;
                            }
                        }
                        
                        return true;
                    }
                    finally
                    {
                        _canMsgPool.Return(sendMsg);
                    }
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(buffer);
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }
        
        /// <summary>
        /// 获取通讯统计信息
        /// </summary>
        public CommunicationStatistics GetCommunicationStatistics()
        {
            return _communicationManager.GetStatistics();
        }
        
        #endregion
        
        #region 私有方法
        
        /// <summary>
        /// 初始化数据库
        /// </summary>
        private void InitializeDatabase()
        {
            // 使用异步方式初始化数据库，避免阻塞
            Task.Run(async () =>
            {
                try
                {
                    // 初始化SQLite数据库
                    using (var sqliteDb = new SqlSugarClient(
                        new ConnectionConfig
                        {
                            ConnectionString = _sqliteConnectionString,
                            DbType = DbType.Sqlite,
                            IsAutoCloseConnection = true
                        }))
                    {
                        await sqliteDb.CodeFirst.SetTablesToCreateByEntityTypes(typeof(ECUData)).CreateTablesByEntityTypesAsync();
                    }
                    
                    // 初始化SQL Server数据库
                    using (var sqlServerDb = new SqlSugarClient(
                        new ConnectionConfig
                        {
                            ConnectionString = _sqlServerConnectionString,
                            DbType = DbType.SqlServer,
                            IsAutoCloseConnection = true
                        }))
                    {
                        await sqlServerDb.CodeFirst.SetTablesToCreateByEntityTypes(typeof(ECUData)).CreateTablesByEntityTypesAsync();
                    }
                }
                catch (Exception ex)
                {
                    ConsoleLogger.LogError($"Database initialization error: {ex.Message}");
                }
            });
        }
        
        /// <summary>
        /// 启动主工作循环
        /// </summary>
        private async Task StartMainWorkLoopAsync(CancellationToken cancellationToken)
        {
            await Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        // 执行周期性任务
                        await PerformPeriodicTasksAsync(cancellationToken);
                        
                        // 等待下一次执行
                        await Task.Delay(TimeSpan.FromMilliseconds(_config.MainWorkIntervalMs), cancellationToken);
                    }
                    catch (OperationCanceledException)
                    {
                        // 正常取消，退出循环
                        break;
                    }
                    catch (Exception ex)
                    {
                        ConsoleLogger.LogError($"Error in main work loop: {ex.Message}");
                        
                        // 短暂等待后继续
                        await Task.Delay(TimeSpan.FromMilliseconds(1000), cancellationToken);
                    }
                }
            }, cancellationToken);
        }
        
        /// <summary>
        /// 执行周期性任务
        /// </summary>
        private async Task PerformPeriodicTasksAsync(CancellationToken cancellationToken)
        {
            // 处理缓冲区中的数据
            ProcessBufferData();
            
            // 更新通讯状态
            UpdateCommunicationStatus();
            
            // 其他周期性任务...
        }
        
        /// <summary>
        /// 处理缓冲区数据
        /// </summary>
        private void ProcessBufferData()
        {
            // 处理UDS22缓冲区
            if (_bufferManager.TryGetUds22Buffer(out List<byte> uds22Buffer))
            {
                try
                {
                    ProcessUds22Data(uds22Buffer);
                }
                finally
                {
                    _listPool.Return(uds22Buffer);
                }
            }
            
            // 处理RID272缓冲区
            if (_bufferManager.TryGetUdsRid272Buffer(out List<byte> rid272Buffer))
            {
                try
                {
                    ProcessRid272Data(rid272Buffer);
                }
                finally
                {
                    _listPool.Return(rid272Buffer);
                }
            }
        }
        
        /// <summary>
        /// 更新通讯状态
        /// </summary>
        private void UpdateCommunicationStatus()
        {
            var stats = _communicationManager.GetStatistics();
            
            // 如果连续失败次数过多，尝试重新连接
            if (stats.ConsecutiveFailures >= 5 && !stats.IsConnected)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        await _communicationManager.TryReconnectAsync(_cancellationTokenSource.Token);
                    }
                    catch (Exception ex)
                    {
                        ConsoleLogger.LogError($"Reconnection failed: {ex.Message}");
                    }
                });
            }
        }
        
        /// <summary>
        /// 读取DDI（优化版本）
        /// </summary>
        private async Task<byte[]> ReadDdiAsync(byte ddi, CancellationToken cancellationToken = default)
        {
            var sendMsg = _canMsgPool.Get();
            try
            {
                sendMsg.CanId = 0x7E0;
                sendMsg.Dlc = 2;
                sendMsg.Data = new byte[] { 0x22, ddi };
                
                var response = await _communicationManager.SendAndReceiveWithRetryAsync(
                    sendMsg, 
                    0x7E8, 
                    TimeSpan.FromMilliseconds(_config.DefaultTimeoutMs), 
                    cancellationToken);
                
                return ExtractDataFromResponse(response.Data);
            }
            finally
            {
                _canMsgPool.Return(sendMsg);
            }
        }
        
        /// <summary>
        /// 处理UDS22数据
        /// </summary>
        private void ProcessUds22Data(List<byte> data)
        {
            // 实现UDS22数据处理逻辑
        }
        
        /// <summary>
        /// 处理RID272数据
        /// </summary>
        private void ProcessRid272Data(List<byte> data)
        {
            // 实现RID272数据处理逻辑
        }
        
        /// <summary>
        /// 从响应中提取数据
        /// </summary>
        private byte[] ExtractDataFromResponse(byte[] response)
        {
            if (response == null || response.Length < 3) return Array.Empty<byte>();
            
            // 检查响应格式
            if (response[0] == 0x62) // 正确响应
            {
                byte[] data = new byte[response.Length - 3];
                Array.Copy(response, 3, data, 0, data.Length);
                return data;
            }
            
            return Array.Empty<byte>();
        }
        
        /// <summary>
        /// 验证写入响应
        /// </summary>
        private bool IsValidWriteResponse(byte[] response)
        {
            return response != null && response.Length >= 1 && response[0] == 0x6E;
        }
        
        /// <summary>
        /// 处理ECU信息响应
        /// </summary>
        private byte[] ProcessEcuInfoResponse(byte[] response, EcuInfoType infoType)
        {
            // 实现ECU信息响应处理逻辑
            return ExtractDataFromResponse(response);
        }
        
        /// <summary>
        /// 获取ECU信息类型对应的DID
        /// </summary>
        private byte GetDidForEcuInfoType(EcuInfoType infoType)
        {
            return infoType switch
            {
                EcuInfoType.Boot => 0xF180,
                EcuInfoType.App => 0xF181,
                EcuInfoType.Cal => 0xF182,
                EcuInfoType.SerialNumber => 0xF188,
                _ => 0xF180
            };
        }
        
        /// <summary>
        /// 重置CAN消息对象
        /// </summary>
        private void ResetCanMsg(CanMsg msg)
        {
            msg.CanId = 0;
            msg.Dlc = 0;
            msg.Data = null;
        }
        
        #endregion
        
        #region 资源释放
        
        public void Dispose()
        {
            try
            {
                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource?.Dispose();
                
                _semaphore?.Dispose();
                
                _bufferManager?.Dispose();
                
                if (_canBus != null)
                {
                    _canBus.OnCanMsgReceived -= _canMessageHandler.OnCanMsgReceived;
                }
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError($"Error disposing resources: {ex.Message}");
            }
        }
        
        #endregion
    }
    
    #region 辅助类和枚举
    
    /// <summary>
    /// 模块配置
    /// </summary>
    public class ModuleConfiguration
    {
        public int DefaultTimeoutMs { get; set; } = 1000;
        public int MainWorkIntervalMs { get; set; } = 100;
        public int MaxRetryAttempts { get; set; } = 3;
        public int MaxConsecutiveFailures { get; set; } = 5;
    }
    
    /// <summary>
    /// ECU信息类型
    /// </summary>
    public enum EcuInfoType
    {
        Boot,
        App,
        Cal,
        SerialNumber
    }
    
    /// <summary>
    /// 简单的控制台日志记录器
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        public void LogInformation(string message)
        {
            Console.WriteLine($"[INFO] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
        }
        
        public void LogWarning(string message)
        {
            Console.WriteLine($"[WARN] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
        }
        
        public void LogError(string message)
        {
            Console.WriteLine($"[ERROR] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
        }
    }
    
    /// <summary>
    /// 日志记录器接口
    /// </summary>
    public interface ILogger
    {
        void LogInformation(string message);
        void LogWarning(string message);
        void LogError(string message);
    }
    
    #endregion
}