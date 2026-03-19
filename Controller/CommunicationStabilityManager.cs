using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace CheckSystem.Controller
{
    /// <summary>
    /// 通讯稳定性优化组件
    /// </summary>
    public class CommunicationStabilityManager
    {
        private readonly CanBus _canBus;
        private readonly ILogger _logger;
        
        // 通讯状态监控
        private volatile bool _isConnected;
        private volatile int _consecutiveFailures;
        private volatile DateTime _lastSuccessfulCommunication = DateTime.Now;
        
        // 重试配置
        private readonly int _maxRetryAttempts = 3;
        private readonly TimeSpan _baseDelay = TimeSpan.FromMilliseconds(100);
        private readonly TimeSpan _maxDelay = TimeSpan.FromSeconds(5);
        
        // 通讯质量统计
        private readonly ConcurrentQueue<CommunicationRecord> _communicationHistory = new ConcurrentQueue<CommunicationRecord>();
        private readonly TimeSpan _statisticsWindow = TimeSpan.FromMinutes(5);
        
        public CommunicationStabilityManager(CanBus canBus, ILogger logger)
        {
            _canBus = canBus ?? throw new ArgumentNullException(nameof(canBus));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _isConnected = true;
        }
        
        /// <summary>
        /// 带重试机制的CAN消息发送
        /// </summary>
        public async Task<CanMsg> SendAndReceiveWithRetryAsync(
            CanMsg sendMsg, 
            uint expectedId, 
            TimeSpan timeout, 
            CancellationToken cancellationToken = default)
        {
            int attempt = 0;
            TimeSpan delay = _baseDelay;
            
            while (attempt < _maxRetryAttempts)
            {
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    
                    // 检查连接状态
                    if (!_isConnected)
                    {
                        await TryReconnectAsync(cancellationToken);
                    }
                    
                    // 动态调整超时时间
                    TimeSpan adjustedTimeout = AdjustTimeoutBasedOnQuality(timeout);
                    
                    // 发送和接收消息
                    var result = await SendAndReceiveAsync(sendMsg, expectedId, adjustedTimeout, cancellationToken);
                    
                    // 记录成功通讯
                    RecordCommunication(true, adjustedTimeout);
                    
                    return result;
                }
                catch (Exception ex)
                {
                    attempt++;
                    RecordCommunication(false, timeout);
                    
                    _logger.LogWarning($"Communication attempt {attempt} failed: {ex.Message}");
                    
                    if (attempt >= _maxRetryAttempts)
                    {
                        _consecutiveFailures++;
                        throw new CommunicationException($"Failed after {_maxRetryAttempts} attempts", ex);
                    }
                    
                    // 指数退避策略
                    await Task.Delay(delay, cancellationToken);
                    delay = TimeSpan.FromMilliseconds(Math.Min(delay.TotalMilliseconds * 2, _maxDelay.TotalMilliseconds));
                }
            }
            
            throw new CommunicationException("Unexpected error in retry logic");
        }
        
        /// <summary>
        /// 自适应超时调整
        /// </summary>
        private TimeSpan AdjustTimeoutBasedOnQuality(TimeSpan baseTimeout)
        {
            // 计算最近通讯成功率
            var recentRecords = GetRecentCommunicationRecords();
            if (recentRecords.Count == 0) return baseTimeout;
            
            int successCount = 0;
            foreach (var record in recentRecords)
            {
                if (record.IsSuccess) successCount++;
            }
            
            double successRate = (double)successCount / recentRecords.Count;
            
            // 根据成功率调整超时时间
            if (successRate > 0.9) return TimeSpan.FromMilliseconds(baseTimeout.TotalMilliseconds * 0.8);
            if (successRate < 0.5) return TimeSpan.FromMilliseconds(baseTimeout.TotalMilliseconds * 1.5);
            
            return baseTimeout;
        }
        
        /// <summary>
        /// 异步发送和接收CAN消息
        /// </summary>
        private async Task<CanMsg> SendAndReceiveAsync(
            CanMsg sendMsg, 
            uint expectedId, 
            TimeSpan timeout, 
            CancellationToken cancellationToken)
        {
            using (var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
            {
                cts.CancelAfter(timeout);
                
                // 发送消息
                if (!_canBus.SendCanDatas(sendMsg))
                {
                    throw new CommunicationException("Failed to send CAN message");
                }
                
                // 等待响应
                var tcs = new TaskCompletionSource<CanMsg>();
                CanMsg receivedMsg = null;
                
                // 注册消息接收处理
                Action<CanMsg> messageHandler = null;
                messageHandler = msg =>
                {
                    if (msg.CanId == expectedId)
                    {
                        receivedMsg = msg;
                        tcs.TrySetResult(msg);
                    }
                };
                
                _canBus.OnCanMsgReceived += messageHandler;
                
                try
                {
                    // 等待响应或超时
                    using (cancellationToken.Register(() => tcs.TrySetCanceled()))
                    {
                        receivedMsg = await tcs.Task;
                        return receivedMsg;
                    }
                }
                finally
                {
                    // 取消注册消息处理
                    _canBus.OnCanMsgReceived -= messageHandler;
                }
            }
        }
        
        /// <summary>
        /// 尝试重新连接CAN总线
        /// </summary>
        private async Task TryReconnectAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to reconnect CAN bus...");
            
            int attempts = 0;
            const int maxReconnectAttempts = 5;
            
            while (attempts < maxReconnectAttempts && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    attempts++;
                    
                    // 关闭当前连接
                    _canBus.StopCan();
                    
                    // 等待一段时间再重连
                    await Task.Delay(TimeSpan.FromMilliseconds(500 * attempts), cancellationToken);
                    
                    // 尝试重新打开连接
                    if (_canBus.StartCan())
                    {
                        _isConnected = true;
                        _consecutiveFailures = 0;
                        _logger.LogInformation($"CAN bus reconnected successfully after {attempts} attempts");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"Reconnect attempt {attempts} failed: {ex.Message}");
                }
            }
            
            throw new CommunicationException($"Failed to reconnect after {maxReconnectAttempts} attempts");
        }
        
        /// <summary>
        /// 记录通讯记录
        /// </summary>
        private void RecordCommunication(bool isSuccess, TimeSpan duration)
        {
            var record = new CommunicationRecord
            {
                Timestamp = DateTime.Now,
                IsSuccess = isSuccess,
                Duration = duration
            };
            
            _communicationHistory.Enqueue(record);
            
            // 更新状态
            if (isSuccess)
            {
                _lastSuccessfulCommunication = DateTime.Now;
                _consecutiveFailures = 0;
            }
            else
            {
                _consecutiveFailures++;
                
                // 如果连续失败次数过多，标记为断开连接
                if (_consecutiveFailures >= 5)
                {
                    _isConnected = false;
                }
            }
            
            // 清理过期的通讯记录
            CleanupOldRecords();
        }
        
        /// <summary>
        /// 获取最近的通讯记录
        /// </summary>
        private List<CommunicationRecord> GetRecentCommunicationRecords()
        {
            var result = new List<CommunicationRecord>();
            var cutoffTime = DateTime.Now - _statisticsWindow;
            
            foreach (var record in _communicationHistory)
            {
                if (record.Timestamp >= cutoffTime)
                {
                    result.Add(record);
                }
            }
            
            return result;
        }
        
        /// <summary>
        /// 清理过期的通讯记录
        /// </summary>
        private void CleanupOldRecords()
        {
            var cutoffTime = DateTime.Now - _statisticsWindow;
            
            while (_communicationHistory.TryPeek(out var record) && record.Timestamp < cutoffTime)
            {
                _communicationHistory.TryDequeue(out _);
            }
        }
        
        /// <summary>
        /// 获取通讯质量统计信息
        /// </summary>
        public CommunicationStatistics GetStatistics()
        {
            var recentRecords = GetRecentCommunicationRecords();
            
            if (recentRecords.Count == 0)
            {
                return new CommunicationStatistics
                {
                    SuccessRate = 0,
                    AverageResponseTime = TimeSpan.Zero,
                    TotalCommunications = 0,
                    IsConnected = _isConnected
                };
            }
            
            int successCount = 0;
            long totalTicks = 0;
            
            foreach (var record in recentRecords)
            {
                if (record.IsSuccess) successCount++;
                totalTicks += record.Duration.Ticks;
            }
            
            return new CommunicationStatistics
            {
                SuccessRate = (double)successCount / recentRecords.Count,
                AverageResponseTime = TimeSpan.FromTicks(totalTicks / recentRecords.Count),
                TotalCommunications = recentRecords.Count,
                IsConnected = _isConnected,
                LastSuccessfulCommunication = _lastSuccessfulCommunication,
                ConsecutiveFailures = _consecutiveFailures
            };
        }
    }
    
    /// <summary>
    /// 通讯记录
    /// </summary>
    internal class CommunicationRecord
    {
        public DateTime Timestamp { get; set; }
        public bool IsSuccess { get; set; }
        public TimeSpan Duration { get; set; }
    }
    
    /// <summary>
    /// 通讯统计信息
    /// </summary>
    public class CommunicationStatistics
    {
        public double SuccessRate { get; set; }
        public TimeSpan AverageResponseTime { get; set; }
        public int TotalCommunications { get; set; }
        public bool IsConnected { get; set; }
        public DateTime LastSuccessfulCommunication { get; set; }
        public int ConsecutiveFailures { get; set; }
    }
    
    /// <summary>
    /// 通讯异常
    /// </summary>
    public class CommunicationException : Exception
    {
        public CommunicationException(string message) : base(message) { }
        public CommunicationException(string message, Exception innerException) : base(message, innerException) { }
    }
    
    /// <summary>
    /// 优先级消息队列
    /// </summary>
    public class PriorityMessageQueue
    {
        private readonly ConcurrentQueue<PriorityMessage> _highPriorityQueue = new ConcurrentQueue<PriorityMessage>();
        private readonly ConcurrentQueue<PriorityMessage> _normalPriorityQueue = new ConcurrentQueue<PriorityMessage>();
        private readonly ConcurrentQueue<PriorityMessage> _lowPriorityQueue = new ConcurrentQueue<PriorityMessage>();
        
        public void Enqueue(PriorityMessage message)
        {
            switch (message.Priority)
            {
                case MessagePriority.High:
                    _highPriorityQueue.Enqueue(message);
                    break;
                case MessagePriority.Normal:
                    _normalPriorityQueue.Enqueue(message);
                    break;
                case MessagePriority.Low:
                    _lowPriorityQueue.Enqueue(message);
                    break;
            }
        }
        
        public bool TryDequeue(out PriorityMessage message)
        {
            // 按优先级顺序尝试出队
            if (_highPriorityQueue.TryDequeue(out message))
                return true;
            
            if (_normalPriorityQueue.TryDequeue(out message))
                return true;
            
            return _lowPriorityQueue.TryDequeue(out message);
        }
        
        public int Count => _highPriorityQueue.Count + _normalPriorityQueue.Count + _lowPriorityQueue.Count;
    }
    
    /// <summary>
    /// 优先级消息
    /// </summary>
    public class PriorityMessage
    {
        public CanMsg Message { get; set; }
        public MessagePriority Priority { get; set; }
        public DateTime Timestamp { get; set; }
        public CancellationToken CancellationToken { get; set; }
    }
    
    /// <summary>
    /// 消息优先级
    /// </summary>
    public enum MessagePriority
    {
        Low,
        Normal,
        High
    }
}