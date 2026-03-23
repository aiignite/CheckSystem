# ThreadSafeComponents 控制器接口文档

**命名空间**: `CheckSystem.Controller`
**父类**: -
**控制器描述**: 线程安全的缓冲区管理器

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| ThreadSafeBufferManager | 线程安全的缓冲区管理器 | AddToUds22Buffer | void | 方法 | byte[] data | 添加数据到UDS22缓冲区 | - |
| ThreadSafeBufferManager | 线程安全的缓冲区管理器 | TryGetUds22Buffer | bool | 方法 | out List<byte> buffer | 获取UDS22缓冲区 | - |
| ThreadSafeBufferManager | 线程安全的缓冲区管理器 | ClearUds22Buffer | void | 方法 | - | 清空UDS22缓冲区 | - |
| ThreadSafeBufferManager | 线程安全的缓冲区管理器 | BReadDid | bool | 属性 | - | 读写标志 | - |
| ThreadSafeBufferManager | 线程安全的缓冲区管理器 | BReadDidWaitHandleWait | bool | 方法 | int timeoutMs | 等待读取完成 | - |
| ThreadSafeBufferManager | 线程安全的缓冲区管理器 | SetBReadDidWaitHandle | void | 方法 | - | 设置读取完成事件 | - |
| ThreadSafeBufferManager | 线程安全的缓冲区管理器 | ResetBReadDidWaitHandle | void | 方法 | - | 重置读取完成事件 | - |
| ThreadSafeBufferManager | 线程安全的缓冲区管理器 | Dispose | void | 方法 | - | 释放资源 | - |

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| ThreadSafeDbWrapper | 线程安全的数据库操作包装器 | ExecuteAsync | Task<T> | 方法 | Func<IDbConnection, T> operation | 异步执行数据库操作 | - |
| ThreadSafeDbWrapper | 线程安全的数据库操作包装器 | Dispose | void | 方法 | - | 释放资源 | - |

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| ThreadSafeCanMessageHandler | 线程安全的CAN消息处理器 | RegisterHandler | void | 方法 | string messageId, Action<CanMsg> handler | 注册消息处理器 | - |
| ThreadSafeCanMessageHandler | 线程安全的CAN消息处理器 | UnregisterHandler | void | 方法 | string messageId | 注销消息处理器 | - |
| ThreadSafeCanMessageHandler | 线程安全的CAN消息处理器 | OnCanMsgReceived | void | 方法 | CanMsg msg | 处理CAN消息 | - |
