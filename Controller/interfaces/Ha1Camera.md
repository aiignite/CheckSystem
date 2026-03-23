# Ha1Camera 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | Ha1Camera |
| 描述 | CAN-Product,HA1-Camera |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| Can | CanBus | CAN总线 |
| CameraPartNumber | string | R,摄像头控制线路板总成零件号F187 |
| SystemSupplierIdentificationNo | string | R,系统供应商标识号F18A |
| ElectricalControllerSerialNo | string | R,电控单元序列号F18C |
| SystemSupplierHardwareVersion | string | R,系统供应商硬件版本号F193 |
| SystemSupplierApplicationVersion | string | R,系统供应商软件版本号F195 |
| ElectricalControllerCanMatrixVersion | string | R,电控单元CAN矩阵版本F1A2 |
| Cv22PmuPowerGood | string | R,CV22配套PMUpowergood_FA03 |
| FirstGrade5VdcdcPowerGood | string | R,一级5VDCDCPowergood_FA03 |
| Cv22CriticalVoltagePwrCv1V579 | string | R,CV22关键电源PWR_CV_1V579_FA03 |
| DebounceGpio | string | R,DEBOUNCE_GPIO_FA03 |
| Kl30Voltage | double | R,KL30电压_FA04 |
| PlateCardHighTemperatureState | string | R,板卡温度_高温状态_FA05 |
| PlateCardLowTemperatureState | string | R,板卡温度_极低温状态_FA05 |
| PwrCvsys1V8 | string | R,PWR_CVSYS_1V8电压状态_FA06 |
| PwrDdr1V1 | string | R,PWR_DDR_1V1电压状态_FA06 |
| PwrCvVdd0V75 | string | R,PWR_CV_VDD_0V75电压状态_FA06 |
| PwrCvsys3V3 | string | R,PWR_CVSYS_3V3电压状态_FA06 |
| PwrCvVdda0V8 | string | R,PWR_CV_VDDA_0V8电压状态_FA06 |
| PmicVddio3V | string | R,PMIC_VDDIO_3V电压状态_FA06 |
| PwrDdr1V8 | string | R,PWR_DDR_1V8电压状态_FA06 |
| PwrEmmc1V8 | string | R,PWR_eMMC_1V8电压状态_FA06 |
| PwrCv1V579 | string | R,PWR_CV_1V579电压状态_FA06 |
| EthVddo1V0 | string | R,ETH_VDDO_1V0电压状态_FA06 |
| FrontCameraHighSideVolt | double | R,前视摄像头高边开关输出电压_FA0E |
| First5VDcdcVolt | double | R,一级5VDCDC输出电压_FA0F |
| AdbCameraBuckBoostVolt | double | R,ADB-camerabuck-boost输出电压检测_FA01 |
| SignalVolt | double | R,休眠唤醒信号线电压_FA02 |
| FrontCameraVideo | string | R,前视摄像头视频_FA00 |
| FrontCameraBuckBoostPowerGood | string | R,前视摄像头buck-boostpowerGood_FA00 |
| FrontCameraHighSideState | string | R,前视摄像头高边开关状态_FA00 |
| DmsVideo | string | R,DMS视频_FA00 |
| EthernetState | string | R,以太网状态_FA00 |
| Cvv2ZoneState | string | R,CV22分区检测_FA00 |
| BoardId | string | R,BoardID_FA00 |
| Cv22ProgramState | string | R,CV22烧写信息_FB01 |
| McuInfo | string | R,MCU信息_FB01 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| EnterDefaultSession | 无 | void | 进入正常模式 |
| EnterExtendedSession | 无 | void | 进入拓展模式 |
| ReadCameraPartNumber | 无 | void | 读取摄像头控制线路板总成零件号 |
| ReadSystemSupplierIdentificationNo | 无 | void | 读系统供应商标识号 |
| ReadElectricalControlSerialNo | 无 | void | 读电控单元序列号 |
| ReadSystemSupplierHardwareVersion | 无 | void | 读系统供应商硬件版本号 |
| ReadSystemSupplierApplicationVersion | 无 | void | 读系统供应商软件版本号 |
| ReadElectricalControllerCanMatrixVersion | 无 | void | 读电控单元CAN矩阵版本 |
| ReadMpuInfo | delayTime: string | void | 读MPU信息 |
| ReadAdbCameraBuckBoostVolt | 无 | void | 读ADB-camera-buck-boost-输出电压检测 |
| ReadSignalVolt | 无 | void | 读休眠唤醒信号线电压 |
| ReadVoltState | 无 | void | 读电压状态 |
| ReadKl30Voltage | 无 | void | 读KL30电压 |
| ReadPlateCardTemplateState | 无 | void | 读板卡温度状态 |
| ReadSystemInternalVolt | 无 | void | 读取系统关键电压状态和电压 |
| ReadZoneProgramInfo | 无 | void | 读取固件烧写信息 |
| ReadFrontCameraHighSideVolt | 无 | void | 读前视摄像头高边开关输出电压 |
| ReadFirst5VDcdcVolt | 无 | void | 一级5VDCDC输出电压 |

## 方法详细说明

### EnterDefaultSession

**描述**: 进入正常模式

**示例代码**:
```csharp
controller.EnterDefaultSession();
```

---

### EnterExtendedSession

**描述**: 进入拓展模式

**示例代码**:
```csharp
controller.EnterExtendedSession();
```

---

### ReadCameraPartNumber

**描述**: 读取摄像头控制线路板总成零件号

**示例代码**:
```csharp
controller.ReadCameraPartNumber();
string pn = controller.CameraPartNumber;
```

---

### ReadSystemSupplierIdentificationNo

**描述**: 读系统供应商标识号

**示例代码**:
```csharp
controller.ReadSystemSupplierIdentificationNo();
string id = controller.SystemSupplierIdentificationNo;
```

---

### ReadElectricalControlSerialNo

**描述**: 读电控单元序列号

**示例代码**:
```csharp
controller.ReadElectricalControlSerialNo();
string sn = controller.ElectricalControllerSerialNo;
```

---

### ReadSystemSupplierHardwareVersion

**描述**: 读系统供应商硬件版本号

**示例代码**:
```csharp
controller.ReadSystemSupplierHardwareVersion();
string ver = controller.SystemSupplierHardwareVersion;
```

---

### ReadSystemSupplierApplicationVersion

**描述**: 读系统供应商软件版本号

**示例代码**:
```csharp
controller.ReadSystemSupplierApplicationVersion();
string ver = controller.SystemSupplierApplicationVersion;
```

---

### ReadElectricalControllerCanMatrixVersion

**描述**: 读电控单元CAN矩阵版本

**示例代码**:
```csharp
controller.ReadElectricalControllerCanMatrixVersion();
string ver = controller.ElectricalControllerCanMatrixVersion;
```

---

### ReadMpuInfo

**描述**: 读MPU信息

**参数**:
- `delayTime` (string): 延迟时间(毫秒)

**示例代码**:
```csharp
controller.ReadMpuInfo("5000");
// 读取: FrontCameraVideo, FrontCameraBuckBoostPowerGood, FrontCameraHighSideState, DmsVideo, EthernetState, Cvv2ZoneState, BoardId
```

---

### ReadAdbCameraBuckBoostVolt

**描述**: 读ADB-camera-buck-boost-输出电压检测

**示例代码**:
```csharp
controller.ReadAdbCameraBuckBoostVolt();
double volt = controller.AdbCameraBuckBoostVolt;
```

---

### ReadSignalVolt

**描述**: 读休眠唤醒信号线电压

**示例代码**:
```csharp
controller.ReadSignalVolt();
double volt = controller.SignalVolt;
```

---

### ReadVoltState

**描述**: 读电压状态

**示例代码**:
```csharp
controller.ReadVoltState();
// 读取: Cv22PmuPowerGood, FirstGrade5VdcdcPowerGood, Cv22CriticalVoltagePwrCv1V579, DebounceGpio
```

---

### ReadKl30Voltage

**描述**: 读KL30电压

**示例代码**:
```csharp
controller.ReadKl30Voltage();
double volt = controller.Kl30Voltage;
```

---

### ReadPlateCardTemplateState

**描述**: 读板卡温度状态

**示例代码**:
```csharp
controller.ReadPlateCardTemplateState();
// 读取: PlateCardHighTemperatureState, PlateCardLowTemperatureState
```

---

### ReadSystemInternalVolt

**描述**: 读取系统关键电压状态和电压

**示例代码**:
```csharp
controller.ReadSystemInternalVolt();
// 读取: PwrCvsys1V8, PwrDdr1V1, PwrCvVdd0V75, PwrCvsys3V3, PwrCvVdda0V8, PmicVddio3V, PwrDdr1V8, PwrEmmc1V8, PwrCv1V579, EthVddo1V0
```

---

### ReadZoneProgramInfo

**描述**: 读取固件烧写信息

**示例代码**:
```csharp
controller.ReadZoneProgramInfo();
// 读取: Cv22ProgramState, McuInfo
```

---

### ReadFrontCameraHighSideVolt

**描述**: 读前视摄像头高边开关输出电压

**示例代码**:
```csharp
controller.ReadFrontCameraHighSideVolt();
double volt = controller.FrontCameraHighSideVolt;
```

---

### ReadFirst5VDcdcVolt

**描述**: 一级5VDCDC输出电压

**示例代码**:
```csharp
controller.ReadFirst5VDcdcVolt();
double volt = controller.First5VDcdcVolt;
```
