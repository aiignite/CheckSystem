# GlobalHvsmSeatHeatingModule 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | GlobalHvsmSeatHeatingModule |
| 描述 | LIN-Product,GlobalHVSM座椅加热模块 |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| LinWithBaudRate10417 | LinBus | LIN总线 |
| QuiescentCurrent | double | R,静态电流 |
| Barcode | string | R,外壳二维码 |
| ModbusBarcode | string | R,外壳二维码-Modbus通讯格式 |
| PcbaBarcode | string | R,PCBA二维码 |
| VBAT1 | float | R,VBAT1采样 |
| VBAT2 | float | R,VBAT2采样 |
| DriverBackSignalAdc | float | R,NTC信号状态-DriverBackSignal信号检测 |
| CoDriverBackSignalAdc | float | R,NTC信号状态-Co-DriverBackSignal信号检测 |
| DriverCushionSignalAdc | float | R,NTC信号状态-DriverCushionSignal信号检测 |
| CoDriverCushionSignalAdc | float | R,NTC信号状态-Co-DriverCushionSignal信号检测 |
| DiscreteInput | string | R,DiscreteInput信号检测 |
| DriverBackCurr | float | R,DriverBack电流检测 |
| CoDriverBackCurr | float | R,Co-DriverBack电流检测 |
| DriverCushionCurr | float | R,DriverCushion电流检测 |
| CoDriverCushionCurr | float | R,Co-DriverCushion电流检测 |
| LowCurrentCircuitLeft | float | R,小电流电路检测Left |
| LowCurrentCircuitRight | float | R,小电流电路检测Right |
| AppVer | string | R,软件APP版本号 |
| BootVer | string | R,软件BOOT版本号 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| StartLin | 无 | void | 打开LIN |
| StopLin | 无 | void | 关闭LIN |
| NormalMode | 无 | void | 正常模式 |
| TempMode | 无 | void | 温度模式 |
| PwmMode | 无 | void | PWM模式 |
| DriverFanPwm | pwm: double | void | DriverFanPwm |
| CoDriverFanPwm | pwm: double | void | CoDriverFanPwm |
| DriverBackHsdPwm | pwm: double | void | DriverBackHsdPwm |
| CoDriverBackHsdPwm | pwm: double | void | CoDriverBackHsdPwm |
| DriverCushionHsdPwm | pwm: double | void | DriverCushionHsdPwm |
| CoDriverCushionHsdPwm | pwm: double | void | CoDriverCushionHsdPwm |
| SetFrontRow | 无 | void | 设置成前排-DiscreteInput需悬空 |
| SetBakRow | 无 | void | 设置成后排-DiscreteInput需接GND |
| ReadAppVer | 无 | void | 读软件APP版本号 |
| ReadBootVer | 无 | void | 读软件Boot版本号 |
| ReadVBAT1 | 无 | void | 读VBAT1 |
| ReadVBAT2 | 无 | void | 读VBAT2 |
| ReadDriverBackSignalAdc | 无 | void | 读DriverBackSignal信号检测 |
| ReadCoDriverBackSignalAdc | 无 | void | 读Co-DriverBackSignal信号检测 |
| ReadDriverCushionSignalAdc | 无 | void | 读DriverCushionSignal信号检测 |
| ReadCoDriverCushionSignalAdc | 无 | void | 读Co-DriverCushionSignal信号检测 |
| ReadDiscreteInput | 无 | void | 读DiscreteInput信号检测 |
| ReadLowCurrentCircuitLeft | 无 | void | 读小电流电路检测-Left |
| ReadLowCurrentCircuitRight | 无 | void | 读小电流电路检测-Right |
| ReadDriverBackCurr | 无 | void | 读DriverBack电流检测 |
| ReadCoDriverBackCurr | 无 | void | 读Co-DriverBack电流检测 |
| ReadDriverCushionCurr | 无 | void | 读DriverCushion电流检测 |
| ReadCoDriverCushionCurr | 无 | void | 读Co-DriverCushion电流检测 |
| GenerateBarcode | generalPartNo: string, generalVpps: string, seeyaoDuns: string, track1: string, track2: string, track3: string | void | 生成二维码 |

## 方法详细说明

### StartLin

**描述**: 打开LIN

**示例代码**:
```csharp
controller.StartLin();
```

---

### StopLin

**描述**: 关闭LIN

**示例代码**:
```csharp
controller.StopLin();
```

---

### NormalMode

**描述**: 正常模式

**示例代码**:
```csharp
controller.NormalMode();
```

---

### TempMode

**描述**: 温度模式

**示例代码**:
```csharp
controller.TempMode();
```

---

### PwmMode

**描述**: PWM模式

**示例代码**:
```csharp
controller.PwmMode();
```

---

### DriverFanPwm

**描述**: DriverFanPwm

**参数**:
- `pwm` (double): PWM值 (0-100)

**示例代码**:
```csharp
controller.DriverFanPwm(50);
```

---

### CoDriverFanPwm

**描述**: CoDriverFanPwm

**参数**:
- `pwm` (double): PWM值 (0-100)

**示例代码**:
```csharp
controller.CoDriverFanPwm(50);
```

---

### DriverBackHsdPwm

**描述**: DriverBackHsdPwm

**参数**:
- `pwm` (double): PWM值 (0-100)

**示例代码**:
```csharp
controller.DriverBackHsdPwm(50);
```

---

### CoDriverBackHsdPwm

**描述**: CoDriverBackHsdPwm

**参数**:
- `pwm` (double): PWM值 (0-100)

**示例代码**:
```csharp
controller.CoDriverBackHsdPwm(50);
```

---

### DriverCushionHsdPwm

**描述**: DriverCushionHsdPwm

**参数**:
- `pwm` (double): PWM值 (0-100)

**示例代码**:
```csharp
controller.DriverCushionHsdPwm(50);
```

---

### CoDriverCushionHsdPwm

**描述**: CoDriverCushionHsdPwm

**参数**:
- `pwm` (double): PWM值 (0-100)

**示例代码**:
```csharp
controller.CoDriverCushionHsdPwm(50);
```

---

### SetFrontRow

**描述**: 设置成前排-DiscreteInput需悬空

**示例代码**:
```csharp
controller.SetFrontRow();
```

---

### SetBakRow

**描述**: 设置成后排-DiscreteInput需接GND

**示例代码**:
```csharp
controller.SetBakRow();
```

---

### ReadAppVer

**描述**: 读软件APP版本号

**示例代码**:
```csharp
controller.ReadAppVer();
string ver = controller.AppVer;
```

---

### ReadBootVer

**描述**: 读软件Boot版本号

**示例代码**:
```csharp
controller.ReadBootVer();
string ver = controller.BootVer;
```

---

### ReadVBAT1

**描述**: 读VBAT1

**示例代码**:
```csharp
controller.ReadVBAT1();
float vbat = controller.VBAT1;
```

---

### ReadVBAT2

**描述**: 读VBAT2

**示例代码**:
```csharp
controller.ReadVBAT2();
float vbat = controller.VBAT2;
```

---

### ReadDriverBackSignalAdc

**描述**: 读DriverBackSignal信号检测

**示例代码**:
```csharp
controller.ReadDriverBackSignalAdc();
float val = controller.DriverBackSignalAdc;
```

---

### ReadCoDriverBackSignalAdc

**描述**: 读Co-DriverBackSignal信号检测

**示例代码**:
```csharp
controller.ReadCoDriverBackSignalAdc();
float val = controller.CoDriverBackSignalAdc;
```

---

### ReadDriverCushionSignalAdc

**描述**: 读DriverCushionSignal信号检测

**示例代码**:
```csharp
controller.ReadDriverCushionSignalAdc();
float val = controller.DriverCushionSignalAdc;
```

---

### ReadCoDriverCushionSignalAdc

**描述**: 读Co-DriverCushionSignal信号检测

**示例代码**:
```csharp
controller.ReadCoDriverCushionSignalAdc();
float val = controller.CoDriverCushionSignalAdc;
```

---

### ReadDiscreteInput

**描述**: 读DiscreteInput信号检测

**示例代码**:
```csharp
controller.ReadDiscreteInput();
string val = controller.DiscreteInput;
```

---

### ReadLowCurrentCircuitLeft

**描述**: 读小电流电路检测-Left

**示例代码**:
```csharp
controller.ReadLowCurrentCircuitLeft();
float val = controller.LowCurrentCircuitLeft;
```

---

### ReadLowCurrentCircuitRight

**描述**: 读小电流电路检测-Right

**示例代码**:
```csharp
controller.ReadLowCurrentCircuitRight();
float val = controller.LowCurrentCircuitRight;
```

---

### ReadDriverBackCurr

**描述**: 读DriverBack电流检测

**示例代码**:
```csharp
controller.ReadDriverBackCurr();
float curr = controller.DriverBackCurr;
```

---

### ReadCoDriverBackCurr

**描述**: 读Co-DriverBack电流检测

**示例代码**:
```csharp
controller.ReadCoDriverBackCurr();
float curr = controller.CoDriverBackCurr;
```

---

### ReadDriverCushionCurr

**描述**: 读DriverCushion电流检测

**示例代码**:
```csharp
controller.ReadDriverCushionCurr();
float curr = controller.DriverCushionCurr;
```

---

### ReadCoDriverCushionCurr

**描述**: 读Co-DriverCushion电流检测

**示例代码**:
```csharp
controller.ReadCoDriverCushionCurr();
float curr = controller.CoDriverCushionCurr;
```

---

### GenerateBarcode

**描述**: 生成二维码

**参数**:
- `generalPartNo` (string): 通用零件号
- `generalVpps` (string): 通用VPPS号
- `seeyaoDuns` (string): DUNS码
- `track1` (string): 追溯信息1
- `track2` (string): 追溯信息2
- `track3` (string): 追溯信息3

**示例代码**:
```csharp
controller.GenerateBarcode("12345678", "VPPS001", "123456789", "A", "01", "01");
string barcode = controller.Barcode;
```
