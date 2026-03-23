# E001HeadLamp 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | E001HeadLamp |
| 描述 | CAN-Product,红旗E001头灯 |
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
| LeftLowBeamOn | 无 | void | 左近光打开 |
| LeftLowBeamOff | 无 | void | 左近光关闭 |
| RightLowBeamOn | 无 | void | 右近光打开 |
| RightLowBeamOff | 无 | void | 右近光关闭 |
| LeftHighBeamOn | 无 | void | 左远光打开 |
| LeftHighBeamOff | 无 | void | 左远光关闭 |
| RightHighBeamOn | 无 | void | 右远光打开 |
| RightHighBeamOff | 无 | void | 右远光关闭 |
| RightTurningLightOn | 无 | void | 右转向打开 |
| RightFlowTurningLightOn | 无 | void | 右转向时序打开 |
| RightTurnLightOff | 无 | void | 右转向关闭 |
| RightDayRunningLightOn | 无 | void | 右日行灯打开 |
| RightDayRunningLightOff | 无 | void | 右日行灯关闭 |
| RightPositionLightOn | 无 | void | 右位置灯打开 |
| RightPositionLightOff | 无 | void | 右位置灯关闭 |
| LeftTurningLightOn | 无 | void | 左转向打开 |
| LeftFlowTurningLightOn | 无 | void | 左转向时序打开 |
| LeftTurnLightOff | 无 | void | 左转向关闭 |
| LeftDayRunningLightOn | 无 | void | 左日行灯打开 |
| LeftDayRunningLightOff | 无 | void | 左日行灯关闭 |
| LeftPositionLightOn | 无 | void | 左位置灯打开 |
| LeftPositionLightOff | 无 | void | 左位置灯关闭 |
| LockCar | mode: int | void | 闭锁-LockCar-mode1~5 |
| UnlockCar | mode: int | void | 解锁-UnlockCar-mode1~5 |
| LightingShow | mode: int | void | LightingShow-mode1~5 |
| AbortAnimotion | 无 | void | 停止动画 |
| AddDtcCodeIntoBlackList | code: string | void | 将DTC添加进黑名单 |
| ClearBlackList | 无 | void | 清空黑名单 |
| ReadDtcL | 无 | void | 读左灯DTC |
| ClearLFault | 无 | void | 清除左灯错误 |
| ReadDtcR | 无 | void | 读右灯DTC |
| ClearRFault | 无 | void | 清除右灯错误 |
| AddDrlFtslDtcCodeIntoBlackList | 无 | void | 将DRL/FTSL的DTC添加进黑名单 |

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

### LeftLowBeamOn

**描述**: 左近光打开

**示例代码**:
```csharp
controller.LeftLowBeamOn();
```

---

### LeftHighBeamOn

**描述**: 左远光打开

**示例代码**:
```csharp
controller.LeftHighBeamOn();
```

---

### RightTurningLightOn

**描述**: 右转向打开

**示例代码**:
```csharp
controller.RightTurningLightOn();
```

---

### RightFlowTurningLightOn

**描述**: 右转向时序打开

**示例代码**:
```csharp
controller.RightFlowTurningLightOn();
```

---

### LeftDayRunningLightOn

**描述**: 左日行灯打开

**示例代码**:
```csharp
controller.LeftDayRunningLightOn();
```

---

### LockCar

**描述**: 闭锁-LockCar-mode1~5

**参数**:
- `mode` (int): 模式（1-5）

**示例代码**:
```csharp
controller.LockCar(3);
```

---

### UnlockCar

**描述**: 解锁-UnlockCar-mode1~5

**参数**:
- `mode` (int): 模式（1-5）

**示例代码**:
```csharp
controller.UnlockCar(2);
```

---

### LightingShow

**描述**: LightingShow-mode1~5

**参数**:
- `mode` (int): 模式（1-5）

**示例代码**:
```csharp
controller.LightingShow(1);
```

---

### AbortAnimotion

**描述**: 停止动画

**示例代码**:
```csharp
controller.AbortAnimotion();
```

---

### AddDtcCodeIntoBlackList

**描述**: 将DTC添加进黑名单

**参数**:
- `code` (string): DTC代码

**示例代码**:
```csharp
controller.AddDtcCodeIntoBlackList("B151814");
```

---

### ClearBlackList

**描述**: 清空黑名单

**示例代码**:
```csharp
controller.ClearBlackList();
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

### ReadDtcR

**描述**: 读右灯DTC

**示例代码**:
```csharp
controller.ReadDtcR();
string dtc = controller.ReadDtcRResult;
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

### ClearRFault

**描述**: 清除右灯错误

**示例代码**:
```csharp
controller.ClearRFault();
string result = controller.ClearFaultRResult;
```

---

### AddDrlFtslDtcCodeIntoBlackList

**描述**: 将DRL/FTSL的DTC添加进黑名单

**示例代码**:
```csharp
controller.AddDrlFtslDtcCodeIntoBlackList();
```
