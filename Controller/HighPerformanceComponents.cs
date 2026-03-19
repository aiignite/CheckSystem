using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CheckSystem.Controller
{
    /// <summary>
    /// 高性能缓冲区管理器，使用ArrayPool减少内存分配
    /// </summary>
    public class HighPerformanceBufferManager
    {
        private readonly ArrayPool<byte> _arrayPool = ArrayPool<byte>.Shared;
        private readonly StringBuilder _stringBuilder = new StringBuilder(256);
        
        // 使用Span<T>和Memory<T>减少内存分配
        public void ProcessData(ReadOnlySpan<byte> data)
        {
            // 使用Span<T>处理数据，避免数组复制
            if (data.Length == 0) return;
            
            // 从ArrayPool租用数组，避免频繁分配
            byte[] buffer = _arrayPool.Rent(data.Length);
            try
            {
                data.CopyTo(buffer);
                
                // 处理数据...
                ProcessBuffer(buffer, data.Length);
            }
            finally
            {
                // 归还数组到池中
                _arrayPool.Return(buffer);
            }
        }
        
        private void ProcessBuffer(byte[] buffer, int length)
        {
            // 实际的数据处理逻辑
        }
        
        /// <summary>
        /// 高效的字符串拼接
        /// </summary>
        public string BuildHexString(ReadOnlySpan<byte> data)
        {
            _stringBuilder.Clear();
            _stringBuilder.EnsureCapacity(data.Length * 3); // 预分配容量
            
            for (int i = 0; i < data.Length; i++)
            {
                if (i > 0) _stringBuilder.Append(' ');
                _stringBuilder.Append(data[i].ToString("X2"));
            }
            
            return _stringBuilder.ToString();
        }
        
        /// <summary>
        /// 高效的字符串分割
        /// </summary>
        public string[] SplitString(string input, char separator)
        {
            if (string.IsNullOrEmpty(input)) return Array.Empty<string>();
            
            // 预估分割后的数量，避免数组多次扩容
            int estimatedCount = input.Count(c => c == separator) + 1;
            var result = new string[estimatedCount];
            
            int start = 0;
            int index = 0;
            int pos;
            
            while ((pos = input.IndexOf(separator, start)) != -1 && index < estimatedCount - 1)
            {
                result[index++] = input.Substring(start, pos - start);
                start = pos + 1;
            }
            
            result[index] = input.Substring(start);
            
            // 如果预估过多，创建正确大小的数组
            if (index + 1 < estimatedCount)
            {
                Array.Resize(ref result, index + 1);
            }
            
            return result;
        }
    }
    
    /// <summary>
    /// 对象池实现，减少对象创建开销
    /// </summary>
    public class ObjectPool<T> where T : class, new()
    {
        private readonly ConcurrentQueue<T> _objects = new ConcurrentQueue<T>();
        private readonly Func<T> _objectGenerator;
        private readonly Action<T> _resetAction;
        
        public ObjectPool(Func<T> objectGenerator = null, Action<T> resetAction = null)
        {
            _objectGenerator = objectGenerator ?? (() => new T());
            _resetAction = resetAction;
        }
        
        public T Get()
        {
            if (_objects.TryDequeue(out T item))
            {
                return item;
            }
            return _objectGenerator();
        }
        
        public void Return(T item)
        {
            if (item == null) return;
            
            _resetAction?.Invoke(item);
            _objects.Enqueue(item);
        }
    }
    
    /// <summary>
    /// 高性能CAN消息处理器
    /// </summary>
    public class HighPerformanceCanMessageHandler
    {
        private readonly HighPerformanceBufferManager _bufferManager;
        private readonly ObjectPool<CanMsg> _canMsgPool;
        private readonly ObjectPool<List<byte>> _listPool;
        
        public HighPerformanceCanMessageHandler(HighPerformanceBufferManager bufferManager)
        {
            _bufferManager = bufferManager ?? throw new ArgumentNullException(nameof(bufferManager));
            _canMsgPool = new ObjectPool<CanMsg>(() => new CanMsg(), msg => ResetCanMsg(msg));
            _listPool = new ObjectPool<List<byte>>(() => new List<byte>(), list => list.Clear());
        }
        
        public void ProcessCanMessage(uint canId, ReadOnlySpan<byte> data)
        {
            // 从对象池获取CanMsg对象，避免创建新对象
            CanMsg msg = _canMsgPool.Get();
            try
            {
                msg.CanId = canId;
                msg.Dlc = (byte)data.Length;
                
                // 使用ArrayPool租用数组，避免内存分配
                byte[] buffer = ArrayPool<byte>.Shared.Rent(data.Length);
                try
                {
                    data.CopyTo(buffer);
                    msg.Data = buffer;
                    
                    // 处理消息...
                    ProcessMessage(msg);
                }
                finally
                {
                    // 归还数组到池中
                    ArrayPool<byte>.Shared.Return(buffer);
                }
            }
            finally
            {
                // 归还CanMsg对象到池中
                _canMsgPool.Return(msg);
            }
        }
        
        private void ResetCanMsg(CanMsg msg)
        {
            msg.CanId = 0;
            msg.Dlc = 0;
            msg.Data = null;
        }
        
        private void ProcessMessage(CanMsg msg)
        {
            // 实际的消息处理逻辑
        }
        
        /// <summary>
        /// 高效的UDS消息构建
        /// </summary>
        public byte[] BuildUdsMessage(byte serviceId, ReadOnlySpan<byte> data)
        {
            // 使用ArrayPool租用数组，避免内存分配
            byte[] buffer = ArrayPool<byte>.Shared.Rent(data.Length + 1);
            try
            {
                buffer[0] = serviceId;
                data.CopyTo(buffer.AsSpan(1));
                
                // 创建正确大小的结果数组
                byte[] result = new byte[data.Length + 1];
                Array.Copy(buffer, result, data.Length + 1);
                
                return result;
            }
            finally
            {
                // 归还数组到池中
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }
    }
    
    /// <summary>
    /// 高性能数据库操作
    /// </summary>
    public class HighPerformanceDbOperations
    {
        private readonly string _connectionString;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(Environment.ProcessorCount, Environment.ProcessorCount);
        
        public HighPerformanceDbOperations(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        /// <summary>
        /// 批量插入数据，减少数据库往返次数
        /// </summary>
        public async Task<int> BatchInsertAsync<T>(IEnumerable<T> items, int batchSize = 100)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            
            int totalInserted = 0;
            var batch = new List<T>(batchSize);
            
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
                    foreach (var item in items)
                    {
                        batch.Add(item);
                        
                        if (batch.Count >= batchSize)
                        {
                            totalInserted += await db.Insertable(batch).ExecuteCommandAsync();
                            batch.Clear();
                        }
                    }
                    
                    // 插入剩余项目
                    if (batch.Count > 0)
                    {
                        totalInserted += await db.Insertable(batch).ExecuteCommandAsync();
                    }
                }
            }
            finally
            {
                _semaphore.Release();
            }
            
            return totalInserted;
        }
        
        /// <summary>
        /// 异步查询，避免阻塞线程
        /// </summary>
        public async Task<List<T>> QueryAsync<T>(Expression<Func<T, bool>> whereExpression)
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
                    return await db.Queryable<T>().WhereIF(whereExpression != null, whereExpression).ToListAsync();
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}