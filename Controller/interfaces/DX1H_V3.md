# DX1H_V3 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | DX1H_V3 |
| 描述 | Can-Product,DX1H座椅模块 |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| FanPower | float | R,风扇供电 |
| FanCurrent1 | float | R,FanCurrent1 |
| FanCurrent2 | float | R,FanCurrent2 |
| KL30_1Voltage | float | R,KL30_1Voltage |
| KL30_2Voltage | float | R,KL30_2Voltage |
| KL30_3Voltage | float | R,KL30_3Voltage |
| NTC1Temperature | float | R,NTC1温度值 |
| NTC2Temperature | float | R,NTC2温度值 |
| SecRIHotCurrent | float | R,坐垫加热电流值 |
| BackrestSeatHotCurrent | float | R,靠背加热电流值 |
| Mot2Current | float | R,电机2电流值 |
| Mot4Current | float | R,电机4电流值 |
| Mot6Current | float | R,电机6电流值 |
| Mot8Current | float | R,电机8电流值 |
| BackrestSeatHotPWM | float | R,靠背加热PWM |
| SecRIHotPWM | float | R,坐垫加热PWM |
| Mot2Hall | float | R,电机2霍尔值 |
| Mot4Hall | float | R,电机4霍尔值 |
| Mot6Hall | float | R,电机6霍尔值 |
| Mot8Hall | float | R,电机8霍尔值 |
| Mot2P | float | R,电机2频率 |
| Mot4P | float | R,电机4频率 |
| Mot6P | float | R,电机6频率 |
| Mot8P | float | R,电机8频率 |
| DPCState | float | R,DPCState |
| OBSSwitchState | string | R,obs开关状态 |
| CanControlleName | string | R/W,通讯板子ID |
| Can | CanBus | CAN总线 |
| KL30TestState | bool | R/W,KL30读取 |
| LinTestState | bool | R/W,Lin通讯 |
| SwitchTestState | bool | R/W,开关电阻 |
| SeatNTCTestState | bool | R/W,座椅靠背开关 |
| FanTestState | bool | R/W,风扇开关 |
| APPVarState | bool | R/W,软件版本 |
| MotorTestState | bool | R/W,马达开关 |
| LumbarSupportState | bool | R/W,腰托开关 |
| BarcodeContent | string | 二维码内容 |
| IsReciveCanMeaage | string | 接收CAN消息状态 |
| AI_BackRest_CushionTilt_LegRestVert_Switch1 | float | R,AI_BackRest_CushionTilt_LegRestVert_Switch1 |
| AI_Slide_LegRestHori_Switch2 | float | R,AI_Slide_LegRestHori_Switch2 |
| AI_Lumbar_4W_Switch3 | float | R,AI_Lumbar_4W_Switch3 |
| AI_Micro_SWITCH11 | float | R,AI_Micro_SWITCH11 |
| AI_ODS_SWITCH12 | float | R,AI_ODS_SWITCH12 |
| AI_EasyEntry_SWITCH13 | float | R,AI_EasyEntry_SWITCH13 |
| AI_M1_M2_SWITCH14 | float | R,AI_M1_M2_SWITCH14 |
| AI_ZeroGravity_RESET_SWITCH15 | float | R,AI_ZeroGravity_RESET_SWITCH15 |
| AI_Pedal_SWITCH16 | float | R,AI_Pedal_SWITCH16 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| SetLeft | isLeft: bool | void | 设置左右 |
| GetCurrentMessage | 无 | void | 刷新信息 |
| LinStartScheduler | 无 | void | 打开Can发送 |
| LinStopScheduler | 无 | void | 关闭Can发送 |
| Group1Motro2Start_BackrestMotOnL | 无 | void | 左电机组1电机2启动 |
| Group1Motro2_BackrestMotOffL | 无 | void | 左电机组1电机2停 |
| Group2Motro4Start_LenMotOnL | 无 | void | 左电机组2电机4动 |
| Group2Motro4_LenMotOffL | 无 | void | 左电机组2电机4停 |
| Group3Motro6Start_BackrestMotOnL | 无 | void | 左电机组3电机6启动 |
| Group3Motro6_BackrestMotOffL | 无 | void | 左电机组3电机6停 |
| Group4Motro8Start_LenMotOnL | 无 | void | 左电机组4电机8动 |
| Group4Motro8_LenMotOffL | 无 | void | 左电机组4电机8停 |
| Group1Motro2Start_BackrestMotOnR | 无 | void | 右电机组1电机2启动 |
| Group1Motro2_BackrestMotOffR | 无 | void | 右电机组1电机2停 |
| Group2Motro4Start_LenMotOnR | 无 | void | 右电机组2电机4动 |
| Group2Motro4_LenMotOffR | 无 | void | 右电机组2电机4停 |
| Group3Motro6Start_BackrestMotOnR | 无 | void | 右电机组3电机6启动 |
| Group3Motro6_BackrestMotOffR | 无 | void | 右电机组3电机6停 |
| Group4Motro8Start_LenMotOnR | 无 | void | 右电机组4电机8动 |
| Group4Motro8_LenMotOffR | 无 | void | 右电机组4电机8停 |

## 方法详细说明

### SetLeft

**描述**: 设置左右

**参数**:
- `isLeft` (bool): 是否左侧

**示例代码**:
```csharp
controller.SetLeft(true);  // 左侧
controller.SetLeft(false); // 右侧
```

---

### GetCurrentMessage

**描述**: 刷新信息

**示例代码**:
```csharp
controller.GetCurrentMessage();
float voltage = controller.KL30_1Voltage;
```

---

### LinStartScheduler

**描述**: 打开Can发送

**示例代码**:
```csharp
controller.LinStartScheduler();
```

---

### LinStopScheduler

**描述**: 关闭Can发送

**示例代码**:
```csharp
controller.LinStopScheduler();
```

---

### Group1Motro2Start_BackrestMotOnL

**描述**: 左电机组1电机2启动

**示例代码**:
```csharp
controller.Group1Motro2Start_BackrestMotOnL();
```

---

### Group1Motro2_BackrestMotOffL

**描述**: 左电机组1电机2停

**示例代码**:
```csharp
controller.Group1Motro2_BackrestMotOffL();
```

---

### Group2Motro4Start_LenMotOnL

**描述**: 左电机组2电机4动

**示例代码**:
```csharp
controller.Group2Motro4Start_LenMotOnL();
```

---

### Group2Motro4_LenMotOffL

**描述**: 左电机组2电机4停

**示例代码**:
```csharp
controller.Group2Motro4_LenMotOffL();
```

---

### Group3Motro6Start_BackrestMotOnL

**描述**: 左电机组3电机6启动

**示例代码**:
```csharp
controller.Group3Motro6Start_BackrestMotOnL();
```

---

### Group3Motro6_BackrestMotOffL

**描述**: 左电机组3电机6停

**示例代码**:
```csharp
controller.Group3Motro6_BackrestMotOffL();
```

---

### Group4Motro8Start_LenMotOnL

**描述**: 左电机组4电机8动

**示例代码**:
```csharp
controller.Group4Motro8Start_LenMotOnL();
```

---

### Group4Motro8_LenMotOffL

**描述**: 左电机组4电机8停

**示例代码**:
```csharp
controller.Group4Motro8_LenMotOffL();
```

---

### Group1Motro2Start_BackrestMotOnR

**描述**: 右电机组1电机2启动

**示例代码**:
```csharp
controller.Group1Motro2Start_BackrestMotOnR();
```

---

### Group1Motro2_BackrestMotOffR

**描述**: 右电机组1电机2停

**示例代码**:
```csharp
controller.Group1Motro2_BackrestMotOffR();
```

---

### Group2Motro4Start_LenMotOnR

**描述**: 右电机组2电机4动

**示例代码**:
```csharp
controller.Group2Motro4Start_LenMotOnR();
```

---

### Group2Motro4_LenMotOffR

**描述**: 右电机组2电机4停

**示例代码**:
```csharp
controller.Group2Motro4_LenMotOffR();
```

---

### Group3Motro6Start_BackrestMotOnR

**描述**: 右电机组3电机6启动

**示例代码**:
```csharp
controller.Group3Motro6Start_BackrestMotOnR();
```

---

### Group3Motro6_BackrestMotOffR

**描述**: 右电机组3电机6停

**示例代码**:
```csharp
controller.Group3Motro6_BackrestMotOffR();
```

---

### Group4Motro8Start_LenMotOnR

**描述**: 右电机组4电机8动

**示例代码**:
```csharp
controller.Group4Motro8Start_LenMotOnR();
```

---

### Group4Motro8_LenMotOffR

**描述**: 右电机组4电机8停

**示例代码**:
```csharp
controller.Group4Motro8_LenMotOffR();
```