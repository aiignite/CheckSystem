# E001RearLamp 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | E001RearLamp |
| 描述 | CAN-Product,红旗E001尾灯 |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| Can | CanBus | CAN总线 |
| ReadDtcLResult | string | R,诊断-读取左灯DTC结果 |
| ClearFaultLResult | string | R,清除左灯错误结果 |
| ReadDtcRResult | string | R,诊断-读取右灯DTC结果 |
| ClearFaultRResult | string | R,清除右灯错误结果 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| ModuleAwake | 无 | void | 唤醒 |
| ModuleSleep | 无 | void | 休眠 |
| StopOn | 无 | void | 制动打开 |
| StopOff | 无 | void | 制动关闭 |
| BullOn | 无 | void | 倒车灯开 |
| BullOff | 无 | void | 倒车灯关 |
| FogOn | 无 | void | 雾灯打开 |
| FogOff | 无 | void | 雾灯关闭 |
| LeftTailOn | 无 | void | 左尾灯打开 |
| LeftTailOff | 无 | void | 左尾灯关闭 |
| LeftTurnOn | 无 | void | 左转向打开 |
| LeftTurnOff | 无 | void | 左转向关闭 |
| RightTailOn | 无 | void | 右尾灯打开 |
| RightTailOff | 无 | void | 右尾灯关闭 |
| RightTurnOn | 无 | void | 右转向打开 |
| RightTurnOff | 无 | void | 右转向关闭 |
| ReadDtcL | 无 | void | 读左灯DTC |
| ClearLFault | 无 | void | 清除左灯错误 |
| ReadDtcR | 无 | void | 读右灯DTC |
| ClearRFault | 无 | void | 清除右灯错误 |

## 方法详细说明

### ModuleAwake

**描述**: 唤醒

**示例代码**:
```csharp
controller.ModuleAwake();
```

---

### ModuleSleep

**描述**: 休眠

**示例代码**:
```csharp
controller.ModuleSleep();
```

---

### StopOn

**描述**: 制动打开

**示例代码**:
```csharp
controller.StopOn();
```

---

### StopOff

**描述**: 制动关闭

**示例代码**:
```csharp
controller.StopOff();
```

---

### BullOn

**描述**: 倒车灯开

**示例代码**:
```csharp
controller.BullOn();
```

---

### BullOff

**描述**: 倒车灯关

**示例代码**:
```csharp
controller.BullOff();
```

---

### FogOn

**描述**: 雾灯打开

**示例代码**:
```csharp
controller.FogOn();
```

---

### FogOff

**描述**: 雾灯关闭

**示例代码**:
```csharp
controller.FogOff();
```

---

### LeftTailOn

**描述**: 左尾灯打开

**示例代码**:
```csharp
controller.LeftTailOn();
```

---

### LeftTailOff

**描述**: 左尾灯关闭

**示例代码**:
```csharp
controller.LeftTailOff();
```

---

### LeftTurnOn

**描述**: 左转向打开

**示例代码**:
```csharp
controller.LeftTurnOn();
```

---

### LeftTurnOff

**描述**: 左转向关闭

**示例代码**:
```csharp
controller.LeftTurnOff();
```

---

### RightTailOn

**描述**: 右尾灯打开

**示例代码**:
```csharp
controller.RightTailOn();
```

---

### RightTailOff

**描述**: 右尾灯关闭

**示例代码**:
```csharp
controller.RightTailOff();
```

---

### RightTurnOn

**描述**: 右转向打开

**示例代码**:
```csharp
controller.RightTurnOn();
```

---

### RightTurnOff

**描述**: 右转向关闭

**示例代码**:
```csharp
controller.RightTurnOff();
```

---

### ReadDtcL

**描述**: 读左灯DTC

**示例代码**:
```csharp
controller.ReadDtcL();
string dtc = controller.ReadDtcLResult;
```

---

### ClearLFault

**描述**: 清除左灯错误

**示例代码**:
```csharp
controller.ClearLFault();
string result = controller.ClearFaultLResult;
```

---

### ReadDtcR

**描述**: 读右灯DTC

**示例代码**:
```csharp
controller.ReadDtcR();
string dtc = controller.ReadDtcRResult;
```

---

### ClearRFault

**描述**: 清除右灯错误

**示例代码**:
```csharp
controller.ClearRFault();
string result = controller.ClearFaultRResult;
```
