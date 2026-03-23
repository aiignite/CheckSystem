# Ha1LightingMaster 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | Ha1LightingMaster |
| 描述 | CAN-Product,HA1灯光主控制器 |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| BodyCan | CanBus | Body CAN总线 |
| SubCan | CanBus | Sub CAN总线 |
| IsdRlCanFd1 | CanBus | ISD右灯CAN FD1 |
| IsdRrCanFd2 | CanBus | ISD右尾灯CAN FD2 |
| IsdFdCanFd3 | CanBus | ISD前灯CAN FD3 |
| IsdRmCanFd4 | CanBus | ISD右中灯CAN FD4 |
| IsdF1CanFd5 | CanBus | ISD F1 CAN FD5 |
| BodyCanIsHaveMsgCheckResult | string | R,BodyCan消息检测 |
| SubCanIsHaveMsgCheckResult | string | R,SubCan消息检测 |
| DlpCamrFitSts | string | R,反馈消息读取0x21d |
| DiagnosticInfomation | string | R,故障DTC读取 |
| ClearDiagnosticInfomationResult | string | R,故障DTC清除 |
| IscLightShowTest | string | R,ISC灯光秀测试结果 |
| IscErrorInfo | string | R,故障信息 |
| CanFd1FrameTest | string | R,RL_CAN消息读取 |
| CanFd2FrameTest | string | R,RR_CAN消息读取 |
| CanFd3FrameTest | string | R,FD_CAN消息读取 |
| CanFd4FrameTest | string | R,RM_CAN消息读取 |
| CanFd5FrameTest | string | R,F1_CAN消息读取 |
| EthernetCheckResult | string | R,以太网连接测试结果 |
| RrCan0X35Byte5Bit6 | string | R,接收测试CANRR通道第6个字节的Bit6 |
| RrCan0X35Byte5Bit7 | string | R,接收测试CANRR通道第6个字节的Bit7 |
| RmCan0X35Byte5Bit3 | string | R,接收测试CANRM通道第6个字节的Bit3 |
| RmCan0X35Byte5Bit4 | string | R,接收测试CANRM通道第6个字节的Bit4 |
| RmCan0X35Byte5Bit5 | string | R,接收测试CANRM通道第6个字节的Bit5 |
| RlCan0X35Byte5Bit1 | string | R,接收测试CANRL通道第6个字节的Bit1 |
| RlCan0X35Byte5Bit2 | string | R,接收测试CANRL通道第6个字节的Bit2 |
| FdCan0X35Byte4Bit4 | string | R,接收测试CANFD通道第5个字节的Bit4 |
| FdCan0X35Byte5Bit0 | string | R,接收测试CANFD通道第6个字节的Bit0 |
| F1Can0X35Byte4Bit2 | string | R,接收测试CANF1通道第5个字节的Bit2 |
| F1Can0X35Byte4Bit3 | string | R,接收测试CANF1通道第5个字节的Bit3 |
| F1Can0X35Byte4Bit5 | string | R,接收测试CANF1通道第5个字节的Bit5 |
| F1Can0X35Byte4Bit6 | string | R,接收测试CANF1通道第5个字节的Bit6 |
| F1Can0X35Byte4Bit7 | string | R,接收测试CANF1通道第5个字节的Bit7 |
| MpuRunFlag | string | R,MPU启动标志位 |
| ProductionDate | string | R/W,生产日期 |
| SerialNo | string | R/W,序列号 |
| Barcode | string | R/W,条码 |
| EcuProgrammingProcessFileNumber | string | R,读取物流配置 |
| LightingMasterPartNo | string | R,LightingMaster的零件号 |
| LightingMasterHardwarePartNo | string | R,LightingMaster的硬件零件号 |
| LightingMasterSoftwarePartNo | string | R,LightingMaster的软件零件号 |
| LightingMasterHardwareVersion | string | R,LightingMaster的硬件版本号 |
| LightingMasterSoftwareVersion | string | R,LightingMaster的软件版本号 |
| MpuSoftwarePartNo | string | R,MPU的软件零件号 |
| MpuSoftwareVersion | string | R,MPU的软件版本号 |
| SysVolt | double | R,系统电压 |
| IscSoftwarePartNo | string | R,ISC模块的软件零件号 |
| IscSoftwareVersion | string | R,ISC模块的软件版本号 |
| IscConfigPartNo | string | R,ISC模块的配置文件零件号 |
| IscConfigVersion | string | R,ISC模块的配置文件版本号 |
| CameraMpuSoftwareVersion | string | R,Camera模块的MPU软件版本号 |
| CameraMcuSoftwareVersion | string | R,Camera模块的MCU软件版本号 |
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
| DlpCamrFit78Sts | string | R,DLP Camera Fit 78状态 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| Awake | 无 | void | 唤醒 |
| Sleep | 无 | void | 休眠 |
| EnterDefaultSession | 无 | void | 进入正常模式 |
| EnterExtendedSession | 无 | void | 进入拓展模式 |
| EnterProgramSession | 无 | void | 进入编程模式 |
| SecurityAccess | subfunc: string | void | 解锁SeedKey |
| HA1ExecuteBodyCanIsHaveMsgCheck | 无 | void | HA1BodyCan消息检测 |
| ExecuteSubCanIsHaveMsgCheck | 无 | void | SubCan消息检测 |
| ReadDtc | 无 | void | 读取DTC |
| ClearDtc | 无 | void | 清除DTC |
| DisableDtc | 无 | void | 禁用DTC |
| DisableCommunication | 无 | void | 关闭正常通信 |
| StartIscLightShowTest | 无 | void | 启动ISC灯光秀测试 |
| ReadrIscErrorInfo | 无 | void | 读取故障信息 |
| ReadCanFdFrames | timeOutMs: string | void | 读取CANFD消息帧 |
| SendRecvTest | timeOutMs: string | void | 接收测试 |
| StartCanFdSend | 无 | void | 启动CANFD发送 |
| ExecuteEthernetCheck | 无 | void | 以太网连接测试 |
| HsdOn | delayMs: string | void | 打开HSD |
| WriteProductionDate | 无 | void | 写生产日期 |
| WriteSerialNo | 无 | void | 写生产序列号 |
| ReadProductionDate | 无 | void | 读生产日期 |
| ReadSerialNo | 无 | void | 读生产序列号 |
| WriteEcuProgrammingProcessFileNumber | value: string | void | 写物流配置 |
| ReadEcuProgrammingProcessFileNumber | 无 | void | 读取物流配置 |
| ReadLightingMasterPartNo | 无 | void | 读LightingMaster的零件号 |
| ReadLightingMasterHardwarePartNo | 无 | void | 读LightingMaster的硬件零件号 |
| ReadLightingMasterSoftwarePartNo | 无 | void | 读LightingMaster的软件零件号 |
| ReadLightingMasterHardwareVersion | 无 | void | 读LightingMaster的硬件版本号 |
| ReadLightingMasterSoftwareVersion | 无 | void | 读LightingMaster的软件版本号 |
| ReadMpuSoftwarePartNo | 无 | void | 读MPU模块的软件零件号 |
| ReadMpuSoftwareVersion | 无 | void | 读MPU模块的软件版本号 |
| ReadSysVolt | 无 | void | 读系统电压 |
| ReadIscVersionInfo | 无 | void | 检查ISC模块的软件版本号 |
| ReadCameraSoftwareVersion | 无 | void | 读Camera模块的软件版本号 |
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

### Awake

**描述**: 唤醒

**示例代码**:
```csharp
controller.Awake();
```

---

### Sleep

**描述**: 休眠

**示例代码**:
```csharp
controller.Sleep();
```

---

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

### EnterProgramSession

**描述**: 进入编程模式

**示例代码**:
```csharp
controller.EnterProgramSession();
```

---

### SecurityAccess

**描述**: 解锁SeedKey

**参数**:
- `subfunc` (string): 子功能 (如 "0102" 表示 seedSubFunc=01, keySubFunc=02)

**示例代码**:
```csharp
controller.SecurityAccess("0102");
```

---

### HA1ExecuteBodyCanIsHaveMsgCheck

**描述**: HA1BodyCan消息检测

**示例代码**:
```csharp
controller.HA1ExecuteBodyCanIsHaveMsgCheck();
string result = controller.BodyCanIsHaveMsgCheckResult;
```

---

### ExecuteSubCanIsHaveMsgCheck

**描述**: SubCan消息检测

**示例代码**:
```csharp
controller.ExecuteSubCanIsHaveMsgCheck();
string result = controller.SubCanIsHaveMsgCheckResult;
```

---

### ReadDtc

**描述**: 读取DTC

**示例代码**:
```csharp
controller.ReadDtc();
string dtc = controller.DiagnosticInfomation;
```

---

### ClearDtc

**描述**: 清除DTC

**示例代码**:
```csharp
controller.ClearDtc();
string result = controller.ClearDiagnosticInfomationResult;
```

---

### DisableDtc

**描述**: 禁用DTC

**示例代码**:
```csharp
controller.DisableDtc();
```

---

### DisableCommunication

**描述**: 关闭正常通信

**示例代码**:
```csharp
controller.DisableCommunication();
```

---

### StartIscLightShowTest

**描述**: 启动ISC灯光秀测试

**示例代码**:
```csharp
controller.StartIscLightShowTest();
string result = controller.IscLightShowTest;
```

---

### ReadrIscErrorInfo

**描述**: 读取故障信息

**示例代码**:
```csharp
controller.ReadrIscErrorInfo();
string errorInfo = controller.IscErrorInfo;
```

---

### ReadCanFdFrames

**描述**: 读取CANFD消息帧

**参数**:
- `timeOutMs` (string): 超时时间(毫秒),有效范围1-10000,默认10000

**示例代码**:
```csharp
controller.ReadCanFdFrames("5000");
string canFd1 = controller.CanFd1FrameTest;
string canFd2 = controller.CanFd2FrameTest;
```

---

### SendRecvTest

**描述**: 接收测试

**参数**:
- `timeOutMs` (string): 超时时间(毫秒),有效范围1-5000,默认5000

**示例代码**:
```csharp
controller.SendRecvTest("3000");
```

---

### StartCanFdSend

**描述**: 启动CANFD发送

**示例代码**:
```csharp
controller.StartCanFdSend();
```

---

### ExecuteEthernetCheck

**描述**: 以太网连接测试

**示例代码**:
```csharp
controller.ExecuteEthernetCheck();
string result = controller.EthernetCheckResult;
```

---

### HsdOn

**描述**: 打开HSD

**参数**:
- `delayMs` (string): 延迟时间(毫秒)

**示例代码**:
```csharp
controller.HsdOn("1000");
```

---

### WriteProductionDate

**描述**: 写生产日期

**示例代码**:
```csharp
controller.Barcode = "LM:000ABCP001EFG000";
controller.WriteProductionDate();
```

---

### WriteSerialNo

**描述**: 写生产序列号

**示例代码**:
```csharp
controller.Barcode = "LM:000ABCP001EFG000";
controller.WriteSerialNo();
```

---

### ReadProductionDate

**描述**: 读生产日期

**示例代码**:
```csharp
controller.ReadProductionDate();
string date = controller.ProductionDate;
```

---

### ReadSerialNo

**描述**: 读生产序列号

**示例代码**:
```csharp
controller.ReadSerialNo();
string serial = controller.SerialNo;
```

---

### WriteEcuProgrammingProcessFileNumber

**描述**: 写物流配置

**参数**:
- `value` (string): 物流配置值(十六进制字符串)

**示例代码**:
```csharp
controller.WriteEcuProgrammingProcessFileNumber("01A2");
```

---

### ReadEcuProgrammingProcessFileNumber

**描述**: 读取物流配置

**示例代码**:
```csharp
controller.ReadEcuProgrammingProcessFileNumber();
string config = controller.EcuProgrammingProcessFileNumber;
```

---

### ReadLightingMasterPartNo

**描述**: 读LightingMaster的零件号

**示例代码**:
```csharp
controller.ReadLightingMasterPartNo();
string partNo = controller.LightingMasterPartNo;
```

---

### ReadLightingMasterHardwarePartNo

**描述**: 读LightingMaster的硬件零件号

**示例代码**:
```csharp
controller.ReadLightingMasterHardwarePartNo();
string hwPartNo = controller.LightingMasterHardwarePartNo;
```

---

### ReadLightingMasterSoftwarePartNo

**描述**: 读LightingMaster的软件零件号

**示例代码**:
```csharp
controller.ReadLightingMasterSoftwarePartNo();
string swPartNo = controller.LightingMasterSoftwarePartNo;
```

---

### ReadLightingMasterHardwareVersion

**描述**: 读LightingMaster的硬件版本号

**示例代码**:
```csharp
controller.ReadLightingMasterHardwareVersion();
string ver = controller.LightingMasterHardwareVersion;
```

---

### ReadLightingMasterSoftwareVersion

**描述**: 读LightingMaster的软件版本号

**示例代码**:
```csharp
controller.ReadLightingMasterSoftwareVersion();
string ver = controller.LightingMasterSoftwareVersion;
```

---

### ReadMpuSoftwarePartNo

**描述**: 读MPU模块的软件零件号

**示例代码**:
```csharp
controller.ReadMpuSoftwarePartNo();
string partNo = controller.MpuSoftwarePartNo;
```

---

### ReadMpuSoftwareVersion

**描述**: 读MPU模块的软件版本号

**示例代码**:
```csharp
controller.ReadMpuSoftwareVersion();
string ver = controller.MpuSoftwareVersion;
```

---

### ReadSysVolt

**描述**: 读系统电压

**示例代码**:
```csharp
controller.ReadSysVolt();
double volt = controller.SysVolt;
```

---

### ReadIscVersionInfo

**描述**: 检查ISC模块的软件版本号

**示例代码**:
```csharp
controller.ReadIscVersionInfo();
string swPartNo = controller.IscSoftwarePartNo;
string swVer = controller.IscSoftwareVersion;
string configPartNo = controller.IscConfigPartNo;
string configVer = controller.IscConfigVersion;
```

---

### ReadCameraSoftwareVersion

**描述**: 读Camera模块的软件版本号

**示例代码**:
```csharp
controller.ReadCameraSoftwareVersion();
string mpuVer = controller.CameraMpuSoftwareVersion;
string mcuVer = controller.CameraMcuSoftwareVersion;
```

---

### ReadCameraPartNumber

**描述**: 读取摄像头控制线路板总成零件号

**示例代码**:
```csharp
controller.ReadCameraPartNumber();
string partNo = controller.CameraPartNumber;
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
