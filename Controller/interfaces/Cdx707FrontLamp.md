# Cdx707FrontLamp 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | Cdx707FrontLamp |
| 描述 | CAN-Product,CDX707前灯 |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| Can | CanBus | CAN总线 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| ModuleAwake | 无 | void | 模块唤醒 |
| ModuleSleep | 无 | void | 模块休眠 |
| ClassMode | modeIndex: string | void | CLASS选择 |
| LbOn | 无 | void | LowBeamOn |
| LbOff | 无 | void | LowBeamOff |
| HighBeamFlashToPass | 无 | void | 打开远光全亮-FlashToPass |
| HighBeamOnAuto | 无 | void | 打开远光单颗或ADB控制-OnAuto |
| LeftHbASingleOn | ledIndex: string | void | 左模块线路板A单颗 |
| LeftHbBSingleOn | ledIndex: string | void | 左模块线路板B单颗 |
| LeftHbAdbOn | adbIndex: string | void | 左模块线路板ADB |
| RightHbASingleOn | ledIndex: string | void | 右模块线路板A单颗 |
| RightHbBSingleOn | ledIndex: string | void | 右模块线路板B单颗 |
| RightHbAdbOn | adbIndex: string | void | 右模块线路板ADB |
| HighBeamOff | 无 | void | 关闭远光 |
| DrlOn | 无 | void | 打开DRL |
| PlOn | 无 | void | 打开PL |
| DrlPlOff | 无 | void | 关闭DRL/PL |
| TurnHoldingOn | 无 | void | 转向灯常亮 |
| TurnRunningOn | keepMs: string | void | 转向灯闪烁点亮 |
| TurnFlickerOn | keepMs: string | void | 转向灯时序点亮 |
| TurnOff | 无 | void | 关闭转向灯 |
| SblOn | percent: string | void | 按百分比亮度打开SBL |
| SblOff | 无 | void | 关闭SBL |
| Welcome | 无 | void | Welcome |
| Firewell | 无 | void | Firewell |
| ResetWelcomeFirewell | 无 | void | ResetWelcomeFirewell |

## 方法详细说明

### ModuleAwake

**描述**: 模块唤醒

**示例代码**:
```csharp
controller.ModuleAwake();
```

---

### ModuleSleep

**描述**: 模块休眠

**示例代码**:
```csharp
controller.ModuleSleep();
```

---

### ClassMode

**描述**: CLASS选择

**参数**:
- `modeIndex` (string): CLASS索引 (0-13)

**示例代码**:
```csharp
controller.ClassMode("5");
```

---

### LbOn

**描述**: LowBeamOn

**示例代码**:
```csharp
controller.LbOn();
```

---

### LbOff

**描述**: LowBeamOff

**示例代码**:
```csharp
controller.LbOff();
```

---

### HighBeamFlashToPass

**描述**: 打开远光全亮-FlashToPass

**示例代码**:
```csharp
controller.HighBeamFlashToPass();
```

---

### HighBeamOnAuto

**描述**: 打开远光单颗或ADB控制-OnAuto

**示例代码**:
```csharp
controller.HighBeamOnAuto();
```

---

### LeftHbASingleOn

**描述**: 左模块线路板A单颗

**参数**:
- `ledIndex` (string): LED索引 (1-30)

**示例代码**:
```csharp
controller.LeftHbASingleOn("5");
```

---

### LeftHbBSingleOn

**描述**: 左模块线路板B单颗

**参数**:
- `ledIndex` (string): LED索引 (1-30)

**示例代码**:
```csharp
controller.LeftHbBSingleOn("5");
```

---

### LeftHbAdbOn

**描述**: 左模块线路板ADB

**参数**:
- `adbIndex` (string): ADB索引 (1-6)

**示例代码**:
```csharp
controller.LeftHbAdbOn("3");
```

---

### RightHbASingleOn

**描述**: 右模块线路板A单颗

**参数**:
- `ledIndex` (string): LED索引 (1-30)

**示例代码**:
```csharp
controller.RightHbASingleOn("5");
```

---

### RightHbBSingleOn

**描述**: 右模块线路板B单颗

**参数**:
- `ledIndex` (string): LED索引 (1-30)

**示例代码**:
```csharp
controller.RightHbBSingleOn("5");
```

---

### RightHbAdbOn

**描述**: 右模块线路板ADB

**参数**:
- `adbIndex` (string): ADB索引 (1-6)

**示例代码**:
```csharp
controller.RightHbAdbOn("3");
```

---

### HighBeamOff

**描述**: 关闭远光

**示例代码**:
```csharp
controller.HighBeamOff();
```

---

### DrlOn

**描述**: 打开DRL

**示例代码**:
```csharp
controller.DrlOn();
```

---

### PlOn

**描述**: 打开PL

**示例代码**:
```csharp
controller.PlOn();
```

---

### DrlPlOff

**描述**: 关闭DRL/PL

**示例代码**:
```csharp
controller.DrlPlOff();
```

---

### TurnHoldingOn

**描述**: 转向灯常亮

**示例代码**:
```csharp
controller.TurnHoldingOn();
```

---

### TurnRunningOn

**描述**: 转向灯闪烁点亮

**参数**:
- `keepMs` (string): 闪烁间隔毫秒

**示例代码**:
```csharp
controller.TurnRunningOn("500");
```

---

### TurnFlickerOn

**描述**: 转向灯时序点亮

**参数**:
- `keepMs` (string): 时序间隔毫秒

**示例代码**:
```csharp
controller.TurnFlickerOn("500");
```

---

### TurnOff

**描述**: 关闭转向灯

**示例代码**:
```csharp
controller.TurnOff();
```

---

### SblOn

**描述**: 按百分比亮度打开SBL

**参数**:
- `percent` (string): 亮度百分比 (1-100)

**示例代码**:
```csharp
controller.SblOn("50");
```

---

### SblOff

**描述**: 关闭SBL

**示例代码**:
```csharp
controller.SblOff();
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