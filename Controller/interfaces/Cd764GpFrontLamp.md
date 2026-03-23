# Cd764GpFrontLamp 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | Cd764GpFrontLamp |
| 描述 | CAN-Product,CD764高配前灯 |
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
| ModuleAwake | 无 | void | 模块唤醒 |
| ModuleSleep | 无 | void | 模块休眠 |
| SpotEnOn | 无 | void | SpotEnOn |
| SpotEnOff | 无 | void | SpotEnOff |
| LedOn | index: string | void | 打开单颗LED |
| LedOff | 无 | void | 关闭单颗LED |
| SliceOn | index: string | void | 打开SLICE |
| SliceOff | 无 | void | 关闭SLICE |
| HighBeamFlashToPass | 无 | void | 打开远光-Flash to pass |
| HighBeamOff | 无 | void | 关闭远光 |
| LeftDrlOn | 无 | void | 打开左DRL |
| LeftPlOn | 无 | void | 打开左PL |
| LeftDrlPlOff | 无 | void | 关闭左DRL/PL |
| RightDrlOn | 无 | void | 打开右DRL |
| RightPlOn | 无 | void | 打开右PL |
| RightDrlPlOff | 无 | void | 关闭右DRL/PL |
| TurnHoldingOn | 无 | void | 转向灯常亮 |
| TurnRunningOn | keepMs: string | void | 转向灯闪烁点亮 |
| TurnFlickerOn | keepMs: string | void | 转向灯时序点亮 |
| TurnOff | 无 | void | 关闭转向灯 |
| SblOn | percent: string | void | 按百分比亮度打开SBL |
| SblOff | 无 | void | 关闭SBL |
| Welcome | 无 | void | Welcome |
| Firewell | 无 | void | Firewell |
| ResetWelcomeFirewell | 无 | void | ResetWelcomeFirewell |
| ReadAppVer | reqCanId: string, recvCanId: string | void | 读取HASCO从节点软件版本号 |
| ReadAppPartNo | reqCanId: string, recvCanId: string | void | 读取HASCO从节点软件零件号 |

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

### SpotEnOn

**描述**: SpotEnOn

**示例代码**:
```csharp
controller.SpotEnOn();
```

---

### SpotEnOff

**描述**: SpotEnOff

**示例代码**:
```csharp
controller.SpotEnOff();
```

---

### LedOn

**描述**: 打开单颗LED

**参数**:
- `index` (string): LED索引

**示例代码**:
```csharp
controller.LedOn("1");
```

---

### LedOff

**描述**: 关闭单颗LED

**示例代码**:
```csharp
controller.LedOff();
```

---

### SliceOn

**描述**: 打开SLICE

**参数**:
- `index` (string): SLICE索引

**示例代码**:
```csharp
controller.SliceOn("1");
```

---

### SliceOff

**描述**: 关闭SLICE

**示例代码**:
```csharp
controller.SliceOff();
```

---

### HighBeamFlashToPass

**描述**: 打开远光-Flash to pass

**示例代码**:
```csharp
controller.HighBeamFlashToPass();
```

---

### HighBeamOff

**描述**: 关闭远光

**示例代码**:
```csharp
controller.HighBeamOff();
```

---

### LeftDrlOn

**描述**: 打开左DRL

**示例代码**:
```csharp
controller.LeftDrlOn();
```

---

### LeftPlOn

**描述**: 打开左PL

**示例代码**:
```csharp
controller.LeftPlOn();
```

---

### LeftDrlPlOff

**描述**: 关闭左DRL/PL

**示例代码**:
```csharp
controller.LeftDrlPlOff();
```

---

### RightDrlOn

**描述**: 打开右DRL

**示例代码**:
```csharp
controller.RightDrlOn();
```

---

### RightPlOn

**描述**: 打开右PL

**示例代码**:
```csharp
controller.RightPlOn();
```

---

### RightDrlPlOff

**描述**: 关闭右DRL/PL

**示例代码**:
```csharp
controller.RightDrlPlOff();
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
- `keepMs` (string): 保持毫秒

**示例代码**:
```csharp
controller.TurnRunningOn("500");
```

---

### TurnFlickerOn

**描述**: 转向灯时序点亮

**参数**:
- `keepMs` (string): 保持毫秒

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
- `percent` (string): 百分比值 (1-100)

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

---

### ReadAppVer

**描述**: 读取HASCO从节点软件版本号

**参数**:
- `reqCanId` (string): 请求CAN ID (十六进制)
- `recvCanId` (string): 接收CAN ID (十六进制)

**示例代码**:
```csharp
controller.ReadAppVer("0x7b5", "0x6b5");
string ver = controller.AppVer;
```

---

### ReadAppPartNo

**描述**: 读取HASCO从节点软件零件号

**参数**:
- `reqCanId` (string): 请求CAN ID (十六进制)
- `recvCanId` (string): 接收CAN ID (十六进制)

**示例代码**:
```csharp
controller.ReadAppPartNo("0x7b5", "0x6b5");
string partNo = controller.AppPartNo;
```
