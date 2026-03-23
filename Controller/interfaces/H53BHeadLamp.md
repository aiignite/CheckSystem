# H53BHeadLamp 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | H53BHeadLamp |
| 描述 | LIN-Product,H53B点灯 |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| LinWithBaudRate10417 | LinBus | LIN总线 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| LinStartScheduler | 无 | void | 打开LIN发送 |
| LinStopScheduler | 无 | void | 关闭LIN发送 |
| DrlOn | 无 | void | 日行灯亮 |
| PlOn | 无 | void | 位置灯亮 |
| TurnRunningOn | 无 | void | 转向流水亮 |
| TurnHoldingOn | 无 | void | 转向常亮 |
| LedOff | 无 | void | 灯熄灭 |

## 方法详细说明

### LinStartScheduler

**描述**: 打开LIN发送

**示例代码**:
```csharp
controller.LinStartScheduler();
```

---

### LinStopScheduler

**描述**: 关闭LIN发送

**示例代码**:
```csharp
controller.LinStopScheduler();
```

---

### DrlOn

**描述**: 日行灯亮

**示例代码**:
```csharp
controller.DrlOn();
```

---

### PlOn

**描述**: 位置灯亮

**示例代码**:
```csharp
controller.PlOn();
```

---

### TurnRunningOn

**描述**: 转向流水亮

**示例代码**:
```csharp
controller.TurnRunningOn();
```

---

### TurnHoldingOn

**描述**: 转向常亮

**示例代码**:
```csharp
controller.TurnHoldingOn();
```

---

### LedOff

**描述**: 灯熄灭

**示例代码**:
```csharp
controller.LedOff();
```
