using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CheckSystem.Controller
{
    /// <summary>
    /// 线程安全的缓冲区管理器
    /// </summary>
    public class ThreadSafeBufferManager
    {
        private readonly ConcurrentQueue<byte> _uds22Buffer = new ConcurrentQueue<byte>();
        private readonly ConcurrentQueue<byte> _udsRid272Buffer = new ConcurrentQueue<byte>();
        private readonly ConcurrentQueue<KeyLogData> _injectKeyBuffer = new ConcurrentQueue<KeyLogData>();
        
        // 使用volatile确保多线程可见性
        private volatile bool _bReadDid;
        private volatile bool _bRid272;
        private volatile bool _isLogInjectKeyBuffer;
        
        // 使用ManualResetEventSlim提高性能
        private readonly ManualResetEventSlim _bReadDidWaitHandle = new ManualResetEventSlim(false);
        private readonly ManualResetEventSlim _bRid272WaitHandle = new ManualResetEventSlim(false);
        
        // 使用读写锁保护复杂操作
        private readonly ReaderWriterLockSlim _bufferLock = new ReaderWriterLockSlim();
        
        public void AddToUds22Buffer(byte[] data)
        {
            if (data == null) return;
            
            foreach (var b in data)
            {
                _uds22Buffer.Enqueue(b);
            }
        }
        
        public bool TryGetUds22Buffer(out List<byte> buffer)
        {
            buffer = new List<byte>();
            _bufferLock.EnterReadLock();
            try
            {
                while (_uds22Buffer.TryDequeue(out byte b))
                {
                    buffer.Add(b);
                }
                return buffer.Count > 0;
            }
            finally
            {
                _bufferLock.ExitReadLock();
            }
        }
        
        public void ClearUds22Buffer()
        {
            _bufferLock.EnterWriteLock();
            try
            {
                while (_uds22Buffer.TryDequeue(out _)) { }
            }
            finally
            {
                _bufferLock.ExitWriteLock();
            }
        }
        
        // 类似的方法用于_udsRid272Buffer和_injectKeyBuffer...
        
        public bool BReadDid
        {
            get => _bReadDid;
            set => _bReadDid = value;
        }
        
        public bool BReadDidWaitHandleWait(int timeoutMs)
        {
            return _bReadDidWaitHandle.Wait(timeoutMs);
        }
        
        public void SetBReadDidWaitHandle()
        {
            _bReadDidWaitHandle.Set();
        }
        
        public void ResetBReadDidWaitHandle()
        {
            _bReadDidWaitHandle.Reset();
        }
        
        // 类似的方法用于其他EventWaitHandle...
        
        public void Dispose()
        {
            _bReadDidWaitHandle?.Dispose();
            _bRid272WaitHandle?.Dispose();
            _bufferLock?.Dispose();
        }
    }
    
    /// <summary>
    /// 线程安全的数据库操作包装器
    /// </summary>
    public class ThreadSafeDbWrapper
    {
        private readonly string _connectionString;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1); // 限制并发访问
        
        public ThreadSafeDbWrapper(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public async Task<T> ExecuteAsync<T>(Func<IDbConnection, T> operation)
        {
            await _semaphore.WaitAsync();
            try
            {
                using (var db = new SqlSugarClient(
                    new ConnectionConfig
                    {
                        ConnectionString = _connectionString,
                        DbType = DbType.SqlServer,
                        IsAutoCloseConnection = true,
                        InitKeyType = InitKeyType.SystemTable
                    }))
                {
                    return operation(db);
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }
        
        public void Dispose()
        {
            _semaphore?.Dispose();
        }
    }
    
    /// <summary>
    /// 线程安全的CAN消息处理器
    /// </summary>
    public class ThreadSafeCanMessageHandler
    {
        private readonly ThreadSafeBufferManager _bufferManager;
        private readonly ConcurrentDictionary<string, Action<CanMsg>> _messageHandlers;
        
        public ThreadSafeCanMessageHandler(ThreadSafeBufferManager bufferManager)
        {
            _bufferManager = bufferManager ?? throw new ArgumentNullException(nameof(bufferManager));
            _messageHandlers = new ConcurrentDictionary<string, Action<CanMsg>>();
        }
        
        public void RegisterHandler(string messageId, Action<CanMsg> handler)
        {
            _messageHandlers.AddOrUpdate(messageId, handler, (key, oldValue) => handler);
        }
        
        public void UnregisterHandler(string messageId)
        {
            _messageHandlers.TryRemove(messageId, out _);
        }
        
        public void OnCanMsgReceived(CanMsg msg)
        {
            if (msg == null) return;
            
            // 处理特定消息ID的消息
            if (_messageHandlers.TryGetValue(msg.CanId.ToString(), out var handler))
            {
                try
                {
                    handler(msg);
                }
                catch (Exception ex)
                {
                    // 记录错误但不抛出，避免影响其他消息处理
                    Console.WriteLine($"Error processing CAN message {msg.CanId}: {ex.Message}");
                }
            }
            
            // 处理UDS22响应
            if (msg.CanId == 0x7E8 && _bufferManager.BReadDid)
            {
                _bufferManager.AddToUds22Buffer(msg.Data);
                _bufferManager.SetBReadDidWaitHandle();
            }
            
            // 处理RID272响应
            if (msg.CanId == 0x7E8 && _bufferManager.BRid272)
            {
                _bufferManager.AddToUdsRid272Buffer(msg.Data);
                _bufferManager.SetBRid272WaitHandle();
            }
        }
    }
}