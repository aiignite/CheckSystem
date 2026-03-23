# CommunicationStabilityManager 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | CommunicationStabilityManager |
| 描述 | 通讯稳定性优化组件 |
| 命名空间 | CheckSystem.Controller |
| 基类 | 无 |

## 公共字段

无

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| SendAndReceiveWithRetryAsync | sendMsg: CanMsg, expectedId: uint, timeout: TimeSpan, cancellationToken: CancellationToken | Task<CanMsg> | 带重试机制的CAN消息发送 |
| GetStatistics | 无 | CommunicationStatistics | 获取通讯质量统计信息 |

## 方法详细说明

### SendAndReceiveWithRetryAsync

**描述**: 带重试机制的CAN消息发送

**参数**:
- `sendMsg` (CanMsg): 发送的CAN消息
- `expectedId` (uint): 期望的响应CAN ID
- `timeout` (TimeSpan): 超时时间
- `cancellationToken` (CancellationToken): 取消令牌

**返回类型**: Task<CanMsg>

**示例代码**:
```csharp
var result = await stabilityManager.SendAndReceiveWithRetryAsync(msg, 0x100, TimeSpan.FromSeconds(5));
```

---

### GetStatistics

**描述**: 获取通讯质量统计信息

**参数**: 无

**返回类型**: CommunicationStatistics

**示例代码**:
```csharp
var stats = stabilityManager.GetStatistics();
Console.WriteLine($"Success Rate: {stats.SuccessRate}");
```

---

## 内部类

### CommunicationRecord

**描述**: 通讯记录

**属性**:
- `Timestamp` (DateTime): 时间戳
- `IsSuccess` (bool): 是否成功
- `Duration` (TimeSpan): 通讯持续时间

---

### CommunicationStatistics

**描述**: 通讯统计信息

**属性**:
- `SuccessRate` (double): 成功率
- `AverageResponseTime` (TimeSpan): 平均响应时间
- `TotalCommunications` (int): 总通讯次数
- `IsConnected` (bool): 是否已连接
- `LastSuccessfulCommunication` (DateTime): 最后成功通讯时间
- `ConsecutiveFailures` (int): 连续失败次数

---

### CommunicationException

**描述**: 通讯异常

**构造参数**:
- `message` (string): 异常消息
- `innerException` (Exception): 内部异常

---

### PriorityMessageQueue

**描述**: 优先级消息队列

**方法**:
- `Enqueue(message: PriorityMessage)`: 入队
- `TryDequeue(out message: PriorityMessage)`: 出队

**属性**:
- `Count` (int): 队列中的消息数量

---

### PriorityMessage

**描述**: 优先级消息

**属性**:
- `Message` (CanMsg): CAN消息
- `Priority` (MessagePriority): 优先级
- `Timestamp` (DateTime): 时间戳
- `CancellationToken` (CancellationToken): 取消令牌

---

### MessagePriority

**描述**: 消息优先级枚举

**值**:
- `Low`: 低优先级
- `Normal`: 普通优先级
- `High`: 高优先级
