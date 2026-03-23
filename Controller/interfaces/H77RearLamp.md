# H77RearLamp 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | H77RearLamp |
| 描述 | LIN-Product,H77尾灯 |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| LinWithBaudRate10417 | LinBus | LIN总线 |
| LeftBoot | string | R,LeftBoot |
| RightBoot | string | R,RightBoot |
| LeftApp | string | R,LeftApp |
| RightApp | string | R,RightApp |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| LinStartScheduler | 无 | void | 打开LIN发送 |
| LinStopScheduler | 无 | void | 关闭LIN发送 |
| EmcMode | modeIndex: string | void | EMC_MODE |
| EmcSleep | 无 | void | EMC_MODE5_SLEEP |
| LeftParkLampOff | 无 | void | L_位置灯OFF |
| LeftParkLampOn | value: int | void | L_位置灯根据信号打开 |
| LeftParkLamp1 | 无 | void | L_位置灯信号1 |
| LeftParkLamp7 | 无 | void | L_位置灯信号7 |
| RightParkLampOff | 无 | void | R_位置灯OFF |
| RightParkLampOn | value: int | void | R_位置灯根据信号打开 |
| RightParkLamp1 | 无 | void | R_位置灯信号1 |
| RightParkLamp7 | 无 | void | R_位置灯信号7 |
| LeftTurnOff | 无 | void | L_转向灯OFF |
| LeftTurnOn | value: int | void | L_转向灯根据信号打开 |
| LeftTurnLamp1 | 无 | void | L_转向灯信号1 |
| LeftTurnLamp2 | 无 | void | L_转向灯信号2 |
| LeftTurnLamp3 | 无 | void | L_转向灯信号3 |
| LeftTurnLamp4 | 无 | void | L_转向灯信号4 |
| RightTurnOff | 无 | void | R_转向灯OFF |
| RightTurnOn | value: int | void | R_转向灯根据信号打开 |
| RightTurnLamp1 | 无 | void | R_转向灯信号1 |
| RightTurnLamp2 | 无 | void | R_转向灯信号2 |
| RightTurnLamp3 | 无 | void | R_转向灯信号3 |
| RightTurnLamp4 | 无 | void | R_转向灯信号4 |
| RrLmpShwMod_64_5 | mod: string | void | RrLmpShwMod_64_5 |
| RrLmpShwTyp_64_5 | typ: string | void | RrLmpShwTyp_64_5 |
| LeftStopOn | value: int | void | L_制动灯按信号打开 |
| RightStopOn | value: int | void | R_制动灯按信号打开 |
| LogoOn | value: int | void | LOGO灯按信号打开 |
| SendCorrectSleepCmd | 无 | void | 发送正确休眠命令 |
| SendIncorrectSleepCmd | 无 | void | 发送错误休眠命令 |
| LeftReset | 无 | void | 左复位 |
| RightReset | 无 | void | 右复位 |
| ReadLeftBoot | 无 | void | 读左BOOT |
| ReadLeftApp | 无 | void | 读左APP |
| ReadRightBoot | 无 | void | 读右BOOT |
| ReadRightApp | 无 | void | 读右APP |

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

### EmcMode

**描述**: EMC_MODE

**参数**:
- `modeIndex` (string): 模式索引 (1-8, 不包括5)

**示例代码**:
```csharp
controller.EmcMode("3");
```

---

### EmcSleep

**描述**: EMC_MODE5_SLEEP

**示例代码**:
```csharp
controller.EmcSleep();
```

---

### LeftParkLampOff

**描述**: L_位置灯OFF

**示例代码**:
```csharp
controller.LeftParkLampOff();
```

---

### LeftParkLampOn

**描述**: L_位置灯根据信号打开

**参数**:
- `value` (int): 信号值 (0-8)

**示例代码**:
```csharp
controller.LeftParkLampOn(1);
```

---

### LeftParkLamp1

**描述**: L_位置灯信号1

**示例代码**:
```csharp
controller.LeftParkLamp1();
```

---

### LeftParkLamp7

**描述**: L_位置灯信号7

**示例代码**:
```csharp
controller.LeftParkLamp7();
```

---

### RightParkLampOff

**描述**: R_位置灯OFF

**示例代码**:
```csharp
controller.RightParkLampOff();
```

---

### RightParkLampOn

**描述**: R_位置灯根据信号打开

**参数**:
- `value` (int): 信号值 (0-8)

**示例代码**:
```csharp
controller.RightParkLampOn(1);
```

---

### RightParkLamp1

**描述**: R_位置灯信号1

**示例代码**:
```csharp
controller.RightParkLamp1();
```

---

### RightParkLamp7

**描述**: R_位置灯信号7

**示例代码**:
```csharp
controller.RightParkLamp7();
```

---

### LeftTurnOff

**描述**: L_转向灯OFF

**示例代码**:
```csharp
controller.LeftTurnOff();
```

---

### LeftTurnOn

**描述**: L_转向灯根据信号打开

**参数**:
- `value` (int): 信号值 (0-8)

**示例代码**:
```csharp
controller.LeftTurnOn(1);
```

---

### LeftTurnLamp1

**描述**: L_转向灯信号1

**示例代码**:
```csharp
controller.LeftTurnLamp1();
```

---

### LeftTurnLamp2

**描述**: L_转向灯信号2

**示例代码**:
```csharp
controller.LeftTurnLamp2();
```

---

### LeftTurnLamp3

**描述**: L_转向灯信号3

**示例代码**:
```csharp
controller.LeftTurnLamp3();
```

---

### LeftTurnLamp4

**描述**: L_转向灯信号4

**示例代码**:
```csharp
controller.LeftTurnLamp4();
```

---

### RightTurnOff

**描述**: R_转向灯OFF

**示例代码**:
```csharp
controller.RightTurnOff();
```

---

### RightTurnOn

**描述**: R_转向灯根据信号打开

**参数**:
- `value` (int): 信号值 (0-8)

**示例代码**:
```csharp
controller.RightTurnOn(1);
```

---

### RightTurnLamp1

**描述**: R_转向灯信号1

**示例代码**:
```csharp
controller.RightTurnLamp1();
```

---

### RightTurnLamp2

**描述**: R_转向灯信号2

**示例代码**:
```csharp
controller.RightTurnLamp2();
```

---

### RightTurnLamp3

**描述**: R_转向灯信号3

**示例代码**:
```csharp
controller.RightTurnLamp3();
```

---

### RightTurnLamp4

**描述**: R_转向灯信号4

**示例代码**:
```csharp
controller.RightTurnLamp4();
```

---

### RrLmpShwMod_64_5

**描述**: RrLmpShwMod_64_5

**参数**:
- `mod` (string): 模式值 (0-31)

**示例代码**:
```csharp
controller.RrLmpShwMod_64_5("10");
```

---

### RrLmpShwTyp_64_5

**描述**: RrLmpShwTyp_64_5

**参数**:
- `typ` (string): 类型值 (0-7)

**示例代码**:
```csharp
controller.RrLmpShwTyp_64_5("3");
```

---

### LeftStopOn

**描述**: L_制动灯按信号打开

**参数**:
- `value` (int): 信号值 (0-1)

**示例代码**:
```csharp
controller.LeftStopOn(1);
```

---

### RightStopOn

**描述**: R_制动灯按信号打开

**参数**:
- `value` (int): 信号值 (0-1)

**示例代码**:
```csharp
controller.RightStopOn(1);
```

---

### LogoOn

**描述**: LOGO灯按信号打开

**参数**:
- `value` (int): 信号值 (0-2)

**示例代码**:
```csharp
controller.LogoOn(1);
```

---

### SendCorrectSleepCmd

**描述**: 发送正确休眠命令

**示例代码**:
```csharp
controller.SendCorrectSleepCmd();
```

---

### SendIncorrectSleepCmd

**描述**: 发送错误休眠命令

**示例代码**:
```csharp
controller.SendIncorrectSleepCmd();
```

---

### LeftReset

**描述**: 左复位

**示例代码**:
```csharp
controller.LeftReset();
```

---

### RightReset

**描述**: 右复位

**示例代码**:
```csharp
controller.RightReset();
```

---

### ReadLeftBoot

**描述**: 读左BOOT

**示例代码**:
```csharp
controller.ReadLeftBoot();
string ver = controller.LeftBoot;
```

---

### ReadLeftApp

**描述**: 读左APP

**示例代码**:
```csharp
controller.ReadLeftApp();
string ver = controller.LeftApp;
```

---

### ReadRightBoot

**描述**: 读右BOOT

**示例代码**:
```csharp
controller.ReadRightBoot();
string ver = controller.RightBoot;
```

---

### ReadRightApp

**描述**: 读右APP

**示例代码**:
```csharp
controller.ReadRightApp();
string ver = controller.RightApp;
```
