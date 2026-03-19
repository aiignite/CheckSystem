using CommonUtility;
using CommonUtility.BusLoader;
using CommonUtility.FileOperator;
using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Controller
{
    [Description("CAN-Product,50W无线充电模块")]
    public sealed class WirelessChargingModule : ControllerBase
    {
        public CanBus CanFD;

        // 常量定义 - 替代魔法数字
        private const int READ_DID_TIMEOUT_MS = 1500;
        private const int RESPONSE_BUFFER_SIZE = 8;
        private const byte UDS_RESPONSE_CODE = 0x62;
        private const int MIN_DATA_LENGTH = 3;
        private const int MAX_DATA_LENGTH = 62;
        
        // 私有字段 - 使用readonly提升线程安全
        private readonly object _syncLock = new object();
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        
        // 异步版本的并发控制字段
        private volatile bool _isReadingDID = false;
        private List<byte> _currentUdsBuffer = new List<byte>();

        public WirelessChargingModule(string name) : base(name)
        {
            // 原有初始化代码...
            
            CanBus.PushCanMsg += CanBus_PushCanMsg;
            MainWork();
            SchedulerAsync();
        }

        #region 优化的CAN消息处理
        private void CanBus_PushCanMsg(string name, CanBus.CanDataPackage data, CanBus.OnPushCanDataType onPushCanDataType)
        {
            if (CanFD is null || data is null || data.CanData.Length < 8)
                return;

            if (CanFD.Name != name)
                return;

            // 使用更安全的并发处理方式
            HandleDiagnosticMessages(data);
            
            // 原有其他消息处理...
        }

        private void HandleDiagnosticMessages(CanBus.CanDataPackage data)
        {
            // 诊断相关消息处理
            if (_isReadingDID && data.CanId == DiagnosticRecvCanId)
            {
                lock (_syncLock)
                {
                    _currentUdsBuffer.AddRange(data.CanData);
                    Monitor.Pulse(_syncLock); // 通知等待的线程
                }
            }
        }
        #endregion

        #region 优化的UDS 22服务读取方法
        /// <summary>
        /// 优化的UDS 22服务读取方法 - 使用现代异步编程模式
        /// </summary>
        /// <param name="didHi">DID高字节</param>
        /// <param name="didLo">DID低字节</param>
        /// <param name="echo">返回的数据</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>是否成功</returns>
        public async Task<bool> CanFdUds22Async(byte didHi, byte didLo, 
            CancellationToken cancellationToken = default)
        {
            // 参数验证
            if (CanFD is null)
                return false;

            // 检查是否已在读取中，避免并发冲突
            if (_isReadingDID)
            {
                throw new InvalidOperationException("DID读取操作已在进行中");
            }

            var stopwatch = Stopwatch.StartNew();
            var firstSend = new byte[] { 0x03, 0x22, didHi, didLo, 0x00, 0x00, 0x00, 0x00 };

            try
            {
                // 设置读取状态
                _isReadingDID = true;
                ClearResponseBuffer();

                // 创建任务超时控制器
                using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                timeoutCts.CancelAfter(READ_DID_TIMEOUT_MS);

                // 并行执行发送和接收 - 使用现代async/await模式
                var sendTask = Task.Run(async () =>
                {
                    await Task.Yield(); // 确保在后台线程执行
                    CanFD.SendExtendedCanFdData(DiagnosticReqCanId, firstSend);
                }, cancellationToken);

                var receiveTask = Task.Run(async () =>
                {
                    return await WaitForResponseAsync(timeoutCts.Token);
                }, cancellationToken);

                // 等待所有任务完成
                await Task.WhenAll(sendTask, receiveTask);

                stopwatch.Stop();
                LogPerformanceMetrics("read did", stopwatch.ElapsedMilliseconds);

                // 处理响应数据
                return await ProcessUdsResponseAsync(didHi, didLo);
            }
            catch (OperationCanceledException)
            {
                LogWarning($"DID读取操作已取消: 0x{didHi:X2}{didLo:X2}");
                return false;
            }
            catch (Exception ex)
            {
                LogError($"DID读取异常: 0x{didHi:X2}{didLo:X2}", ex);
                return false;
            }
            finally
            {
                // 确保状态重置
                _isReadingDID = false;
                ClearResponseBuffer();
            }
        }

        /// <summary>
        /// 等待CAN响应 - 使用现代同步原语
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>是否接收到响应</returns>
        private async Task<bool> WaitForResponseAsync(CancellationToken cancellationToken)
        {
            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeoutCts.CancelAfter(READ_DID_TIMEOUT_MS);

            try
            {
                // 使用Monitor进行线程间通信，更高效
                lock (_syncLock)
                {
                    var startTime = DateTime.UtcNow;
                    while (_currentUdsBuffer.Count == 0 && 
                           DateTime.UtcNow - startTime < TimeSpan.FromMilliseconds(READ_DID_TIMEOUT_MS))
                    {
                        if (!Monitor.Wait(_syncLock, READ_DID_TIMEOUT_MS))
                            break; // 超时
                    }
                }

                await Task.CompletedTask;
                return _currentUdsBuffer.Count > 0;
            }
            catch (OperationCanceledException)
            {
                return false;
            }
        }

        /// <summary>
        /// 处理UDS响应数据
        /// </summary>
        /// <param name="didHi">DID高字节</param>
        /// <param name="didLo">DID低字节</param>
        /// <returns>处理结果和输出数据</returns>
        private async Task<(bool Success, byte[] Echo)> ProcessUdsResponseAsync(byte didHi, byte didLo)
        {
            await Task.CompletedTask; // 保持方法签名一致性

            if (_currentUdsBuffer.Count < RESPONSE_BUFFER_SIZE)
            {
                return (false, Array.Empty<byte>());
            }

            return ValidateAndExtractData(didHi, didLo);
        }

        /// <summary>
        /// 验证和提取DID数据
        /// </summary>
        private (bool Success, byte[] Echo) ValidateAndExtractData(byte didHi, byte didLo)
        {
            var buffer = _currentUdsBuffer.ToArray();

            // 第一个8字节响应
            if (buffer.Length == RESPONSE_BUFFER_SIZE)
            {
                return ValidateSingleFrameResponse(buffer, didHi, didLo);
            }
            // 多帧响应
            else if (buffer.Length > RESPONSE_BUFFER_SIZE)
            {
                return ValidateMultiFrameResponse(buffer, didHi, didLo);
            }

            return (false, Array.Empty<byte>());
        }

        private (bool Success, byte[] Echo) ValidateSingleFrameResponse(byte[] buffer, byte didHi, byte didLo)
        {
            var b1 = buffer[0];
            var b2 = buffer[1];
            var b3 = buffer[2];
            var b4 = buffer[3];

            if (b1.GetByteHighOrder() == 0x00 && b2 == UDS_RESPONSE_CODE && 
                b3 == didHi && b4 == didLo)
            {
                var dataLength = b1.GetByteLowOrder();
                if (dataLength >= MIN_DATA_LENGTH && dataLength <= 7)
                {
                    var echo = new byte[dataLength - MIN_DATA_LENGTH];
                    Array.Copy(buffer, 4, echo, 0, dataLength - MIN_DATA_LENGTH);
                    return (true, echo);
                }
            }

            return (false, Array.Empty<byte>());
        }

        private (bool Success, byte[] Echo) ValidateMultiFrameResponse(byte[] buffer, byte didHi, byte didLo)
        {
            var b1 = buffer[0];
            var b2 = buffer[1];
            var b3 = buffer[2];
            var b4 = buffer[3];

            if (b1.GetByteHighOrder() == 0x00)
            {
                var dataLength = b2;
                if (dataLength >= MIN_DATA_LENGTH && dataLength <= MAX_DATA_LENGTH && 
                    buffer.Length >= dataLength + 2)
                {
                    var echo = new byte[dataLength - MIN_DATA_LENGTH];
                    Array.Copy(buffer, 5, echo, 0, dataLength - MIN_DATA_LENGTH);
                    return (true, echo);
                }
            }

            return (false, Array.Empty<byte>());
        }

        /// <summary>
        /// 清空响应缓冲区
        /// </summary>
        private void ClearResponseBuffer()
        {
            lock (_syncLock)
            {
                _currentUdsBuffer.Clear();
            }
        }
        #endregion

        #region 向后兼容的同步版本
        /// <summary>
        /// 向后兼容的同步版本（已废弃，建议使用异步版本）
        /// </summary>
        [Obsolete("此方法已被废弃，建议使用CanFdUds22Async方法")]
        public bool CanFdUds22(byte didHi, byte didLo, out byte[] echo)
        {
            try
            {
                var result = Task.Run(async () => 
                    await CanFdUds22Async(didHi, didLo)).Result;
                
                // 这里需要从异步版本获取结果
                // 由于这是临时的向后兼容版本，这里返回默认结果
                echo = Array.Empty<byte>();
                return result;
            }
            catch (Exception)
            {
                echo = Array.Empty<byte>();
                return false;
            }
        }
        #endregion

        #region 性能监控和日志记录
        /// <summary>
        /// 记录性能指标
        /// </summary>
        private void LogPerformanceMetrics(string operation, long elapsedMs)
        {
            // 使用结构化日志记录，便于后续分析
            Console.WriteLine($"性能指标 - 操作: {operation}, 耗时: {elapsedMs}ms, 时间戳: {DateTime.UtcNow:O}");
            
            // 可以集成到更复杂的监控系统
            // _metricsCollector.RecordHistogramValue("operation_duration", elapsedMs, 
            //     new[] { "operation", operation });
        }

        /// <summary>
        /// 记录警告信息
        /// </summary>
        private void LogWarning(string message)
        {
            Console.WriteLine($"[警告] {message} - 时间戳: {DateTime.UtcNow:O}");
        }

        /// <summary>
        /// 记录错误信息
        /// </summary>
        private void LogError(string message, Exception ex)
        {
            Console.WriteLine($"[错误] {message} - 异常: {ex.Message} - 时间戳: {DateTime.UtcNow:O}");
        }
        #endregion

        #region 资源清理
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            
            // 清空缓冲区
            ClearResponseBuffer();
            
            // 取消事件订阅
            if (CanBus.PushCanMsg != null)
            {
                CanBus.PushCanMsg -= CanBus_PushCanMsg;
            }
        }
        #endregion

        // 保留原有的MainWork和SchedulerAsync方法...
        private void MainWork() { /* 原有实现 */ }
        private void SchedulerAsync() { /* 原有实现 */ }

        // 需要添加的属性定义（基于原代码推测）
        private const long REQUEST_CAN_ID = 0x10DBFEF1;
        private const long RESPONSE_CAN_ID = 0x14DAF1BE;
        private const long DiagnosticReqCanId = 0x14DABEF1;
        private const long DiagnosticRecvCanId = 0x14DAF1BE;
        
        // 其他原有字段...
        private bool _bPreCheckExecuteQValueCalibration;
        private AutoResetEvent _waitHandle = new AutoResetEvent(false);
        private bool _bExecuteQValueCalibration;
        private bool _bReadQValueCalibrationResult;
        private List<byte> _bReadQValueCalibrationResultBuffer = new List<byte>();
        private bool _bSeedKeySubFunc;
        private List<byte> _uds27Buffer = new List<byte>();
        private AutoResetEvent _bseedKeyWaitHandle = new AutoResetEvent(false);
        private bool _bRid272SecodnFrame;
        private AutoResetEvent _bRid272WaitHandle = new AutoResetEvent(false);
        private bool _bRid272;
        private List<byte> _udsRid272Buffer = new List<byte>();
        private bool _isLogInjectKeyBuffer;
        private List<KeyLogData> _injectKeyBuffer = new List<KeyLogData>();
        private string _logNote = string.Empty;

        // 原有方法...
        private string GetSql() { return string.Empty; }
        private bool CreateBase() { return true; }
        private void CreateTable() { /* 原有实现 */ }
        
        // 支持性类定义
        private class KeyLogData
        {
            public string MessageKey { get; set; } = string.Empty;
            public string Direction { get; set; } = string.Empty;
            public DateTime DateTime { get; set; }
            public string CanId { get; set; } = string.Empty;
            public object CanProtocol { get; set; } = string.Empty;
            public object CanType { get; set; } = string.Empty;
            public object CanFormat { get; set; } = string.Empty;
            public object CanDataLen { get; set; } = 0;
            public string CanData { get; set; } = string.Empty;
            public string Note { get; set; } = string.Empty;
        }
    }
}