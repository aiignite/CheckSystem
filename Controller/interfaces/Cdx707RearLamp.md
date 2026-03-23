# Cdx707RearLamp 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | Cdx707RearLamp |
| 描述 | CAN-Product,CDX707后灯 |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| Can | CanBus | CAN总线 |
| AppVer | string | R,读取HASCO从节点软件版本号 |
| AppPartNo | string | R,读取HASCO从节点软件零件号 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| ModuleAwake | 无 | void | 控制板唤醒 |
| ModuleSleep | 无 | void | 控制板休眠 |
| TailOn | 无 | void | TailOn |
| TailOff | 无 | void | TailOff |
| LeftTailOn | 无 | void | LeftTailOn |
| LeftTailOff | 无 | void | LeftTailOff |
| RightTailOn | 无 | void | RightTailOn |
| RightTailOff | 无 | void | RightTailOff |
| TailGateOn | 无 | void | 尾门控制ON |
| TailGateOff | 无 | void | 尾门控制OFF |
| LogoSignalOn | 无 | void | Logo信号开 |
| LogoSignalOff | 无 | void | Logo信号关 |
| Welcome | 无 | void | Welcome |
| Firewell | 无 | void | Firewell |
| ResetWelcomeFirewell | 无 | void | ResetWelcomeFirewell |
| ReadAppVer | 无 | void | 读取HASCO从节点软件版本号 |
| ReadAppPartNo | 无 | void | 读取HASCO从节点软件零件号 |
| ChangeToRcml | 无 | void | 切换为A灯(L) |
| ChangeToRcmR | 无 | void | 切换为A灯(R) |

## 方法详细说明

### ModuleAwake

**描述**: 控制板唤醒

**示例代码**:
```csharp
controller.ModuleAwake();
```

---

### ModuleSleep

**描述**: 控制板休眠

**示例代码**:
```csharp
controller.ModuleSleep();
```

---

### TailOn

**描述**: TailOn

**示例代码**:
```csharp
controller.TailOn();
```

---

### TailOff

**描述**: TailOff

**示例代码**:
```csharp
controller.TailOff();
```

---

### LeftTailOn

**描述**: LeftTailOn

**示例代码**:
```csharp
controller.LeftTailOn();
```

---

### LeftTailOff

**描述**: LeftTailOff

**示例代码**:
```csharp
controller.LeftTailOff();
```

---

### RightTailOn

**描述**: RightTailOn

**示例代码**:
```csharp
controller.RightTailOn();
```

---

### RightTailOff

**描述**: RightTailOff

**示例代码**:
```csharp
controller.RightTailOff();
```

---

### TailGateOn

**描述**: 尾门控制ON

**示例代码**:
```csharp
controller.TailGateOn();
```

---

### TailGateOff

**描述**: 尾门控制OFF

**示例代码**:
```csharp
controller.TailGateOff();
```

---

### LogoSignalOn

**描述**: Logo信号开

**示例代码**:
```csharp
controller.LogoSignalOn();
```

---

### LogoSignalOff

**描述**: Logo信号关

**示例代码**:
```csharp
controller.LogoSignalOff();
```

---

### Welcome

**描述**: Welcome

**示例代码**:
```csharp
controller.Welcome();
```

---

### Firewell

**描述**: Firewell

**示例代码**:
```csharp
controller.Firewell();
```

---

### ResetWelcomeFirewell

**描述**: ResetWelcomeFirewell

**示例代码**:
```csharp
controller.ResetWelcomeFirewell();
```

---

### ReadAppVer

**描述**: 读取HASCO从节点软件版本号

**示例代码**:
```csharp
controller.ReadAppVer();
string ver = controller.AppVer;
```

---

### ReadAppPartNo

**描述**: 读取HASCO从节点软件零件号

**示例代码**:
```csharp
controller.ReadAppPartNo();
string partNo = controller.AppPartNo;
```

---

### ChangeToRcml

**描述**: 切换为A灯(L)

**示例代码**:
```csharp
controller.ChangeToRcml();
```

---

### ChangeToRcmR

**描述**: 切换为A灯(R)

**示例代码**:
```csharp
controller.ChangeToRcmR();
```
