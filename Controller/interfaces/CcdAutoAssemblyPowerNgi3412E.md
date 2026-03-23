# CcdAutoAssemblyPowerNgi3412E 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | CcdAutoAssemblyPowerNgi3412E |
| 描述 | (无描述) |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| CurrentV | float | R,电压读取 |
| CurrentC | float | R,电流读取 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| InitPower | ip: string | void | 初始化电源 |
| PowerOn | cl: byte | void | 打开电源 |
| PowerOff | cl: byte | void | 关闭电源 |
| SelectChannel | cl: byte | void | 选择通道 |
| ChannelOn | 无 | void | 打开通道 |
| ChannelOff | 无 | void | 关闭通道 |
| SetV | v: float | void | 设置电压 |
| SetC | c: float | void | 设置电流 |
| ConnectPower | protocolValue: string | void | 连接电源 |
| PowerOn | 无 | void | 打开电源(多通道) |
| PowerOff | 无 | void | 关闭电源(多通道) |
| SetCombSerOn | 无 | void | 设置串联 |
| SetCombParaOn | 无 | void | 设置并联 |
| SetCombOff | 无 | void | 关闭组合 |
| SetVoltage1 | voltage: float | void | 设置电压通道1 |
| SetVoltage2 | voltage: float | void | 设置电压通道2 |
| SetVoltage3 | voltage: float | void | 设置电压通道3 |
| SetCurrent1 | current: float | void | 设置电流通道1 |
| SetCurrent2 | current: float | void | 设置电流通道2 |
| SetCurrent3 | current: float | void | 设置电流通道3 |
| ReadCurrAndVolt | 无 | void | 读取输入 |

## 方法详细说明

### InitPower

**描述**: 初始化电源

**参数**:
- `ip` (string): IP地址

**示例代码**:
```csharp
controller.InitPower("192.168.1.100");
```

---

### PowerOn

**描述**: 打开电源

**参数**:
- `cl` (byte): 通道号

**示例代码**:
```csharp
controller.PowerOn(0);
```

---

### PowerOff

**描述**: 关闭电源

**参数**:
- `cl` (byte): 通道号

**示例代码**:
```csharp
controller.PowerOff(0);
```

---

### SelectChannel

**描述**: 选择通道

**参数**:
- `cl` (byte): 通道号

**示例代码**:
```csharp
controller.SelectChannel(1);
```

---

### ChannelOn

**描述**: 打开通道

**示例代码**:
```csharp
controller.ChannelOn();
```

---

### ChannelOff

**描述**: 关闭通道

**示例代码**:
```csharp
controller.ChannelOff();
```

---

### SetV

**描述**: 设置电压

**参数**:
- `v` (float): 电压值

**示例代码**:
```csharp
controller.SetV(12.5f);
```

---

### SetC

**描述**: 设置电流

**参数**:
- `c` (float): 电流值

**示例代码**:
```csharp
controller.SetC(2.5f);
```

---

### ConnectPower

**描述**: 连接电源

**参数**:
- `protocolValue` (string): 协议值(IP地址)

**示例代码**:
```csharp
controller.ConnectPower("192.168.1.100");
```

---

### PowerOn

**描述**: 打开电源(多通道)

**示例代码**:
```csharp
controller.PowerOn();
```

---

### PowerOff

**描述**: 关闭电源(多通道)

**示例代码**:
```csharp
controller.PowerOff();
```

---

### SetCombSerOn

**描述**: 设置串联

**示例代码**:
```csharp
controller.SetCombSerOn();
```

---

### SetCombParaOn

**描述**: 设置并联

**示例代码**:
```csharp
controller.SetCombParaOn();
```

---

### SetCombOff

**描述**: 关闭组合

**示例代码**:
```csharp
controller.SetCombOff();
```

---

### SetVoltage1

**描述**: 设置电压通道1

**参数**:
- `voltage` (float): 电压值

**示例代码**:
```csharp
controller.SetVoltage1(12.0f);
```

---

### SetVoltage2

**描述**: 设置电压通道2

**参数**:
- `voltage` (float): 电压值

**示例代码**:
```csharp
controller.SetVoltage2(12.0f);
```

---

### SetVoltage3

**描述**: 设置电压通道3

**参数**:
- `voltage` (float): 电压值

**示例代码**:
```csharp
controller.SetVoltage3(12.0f);
```

---

### SetCurrent1

**描述**: 设置电流通道1

**参数**:
- `current` (float): 电流值

**示例代码**:
```csharp
controller.SetCurrent1(1.0f);
```

---

### SetCurrent2

**描述**: 设置电流通道2

**参数**:
- `current` (float): 电流值

**示例代码**:
```csharp
controller.SetCurrent2(1.0f);
```

---

### SetCurrent3

**描述**: 设置电流通道3

**参数**:
- `current` (float): 电流值

**示例代码**:
```csharp
controller.SetCurrent3(1.0f);
```

---

### ReadCurrAndVolt

**描述**: 读取输入

**示例代码**:
```csharp
controller.ReadCurrAndVolt();
float voltage = controller.CurrentV;
```