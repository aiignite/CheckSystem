# S11LLightingMaster 控制器接口文档

## 控制器信息

| 控制器名称 | 控制器描述 |
|-----------|-----------|
| S11LLightingMaster | S11L灯光主控制器 |

## 公共字段

| 名称 | 类型 | 类别 | 描述 |
|------|------|------|------|
| BodyCanIsHaveMsgCheckResult | string | 读 | R,BodyCan消息检测 |
| SubCanIsHaveMsgCheckResult | string | 读 | R,SubCan消息检测 |
| DlpCamrFitSts | string | 读 | R,反馈消息读取0x21d |
| DiagnosticInfomation | string | 读 | R,故障DTC读取 |
| ClearDiagnosticInfomationResult | string | 读 | R,故障DTC清除 |
| IscLightShowTest | string | 读 | R,ISC灯光秀测试结果 |
| IscErrorInfo | string | 读 | R,故障信息 |
| CanFd1FrameTest | string | 读 | R,RL_CAN消息读取 |
| CanFd2FrameTest | string | 读 | R,RR_CAN消息读取 |
| CanFd3FrameTest | string | 读 | R,FD_CAN消息读取 |
| CanFd4FrameTest | string | 读 | R,RM_CAN消息读取 |
| CanFd5FrameTest | string | 读 | R,F1_CAN消息读取 |
| EthernetCheckResult | string | 读 | R,以太网连接测试结果 |
| RrCan0X35Byte5Bit1 | string | 读 | R,接收测试CANRR通道第6个字节的Bit1 |
| RmCan0X35Byte5Bit0 | string | 读 | R,接收测试CANRM通道第6个字节的Bit0 |
| RlCan0X35Byte4Bit7 | string | 读 | R,接收测试CANRL通道第5个字节的Bit7 |
| FdCan0X35Byte4Bit0 | string | 读 | R,接收测试CANFD通道第5个字节的Bit0 |
| FdCan0X35Byte4Bit2 | string | 读 | R,接收测试CANFD通道第5个字节的Bit2 |
| FdCan0X35Byte4Bit4 | string | 读 | R,接收测试CANFD通道第5个字节的Bit4 |
| FdCan0X35Byte4Bit6 | string | 读 | R,接收测试CANFD通道第5个字节的Bit6 |
| F1Can0X35Byte3Bit7 | string | 读 | R,接收测试CANF1通道第4个字节的Bit7 |
| F1Can0X35Byte4Bit1 | string | 读 | R,接收测试CANF1通道第5个字节的Bit1 |
| F1Can0X35Byte4Bit3 | string | 读 | R,接收测试CANF1通道第5个字节的Bit3 |
| F1Can0X35Byte4Bit5 | string | 读 | R,接收测试CANF1通道第5个字节的Bit5 |
| MpuRunFlag | string | 读 | R,MPU启动标志位 |
| LightingMasterPartNo | string | 读 | R,LightingMaster的零件号 |
| LightingMasterHardwarePartNo | string | 读 | R,LightingMaster的硬件零件号 |
| LightingMasterSoftwarePartNo | string | 读 | R,LightingMaster的软件零件号 |
| LightingMasterHardwareVersion | string | 读 | R,LightingMaster的硬件版本号 |
| MpuSoftwarePartNo | string | 读 | R,MPU的软件零件号 |
| MpuSoftwareVersion | string | 读 | R,MPU的软件版本号 |
| SysVolt | double | 读 | R,系统电压 |
| IscSoftwarePartNo | string | 读 | R,ISC模块的软件零件号 |
| IscSoftwareVersion | string | 读 | R,ISC模块的软件版本号 |
| IscConfigPartNo | string | 读 | R,ISC模块的配置文件零件号 |
| IscConfigVersion | string | 读 | R,ISC模块的配置文件版本号 |
| CameraMpuSoftwareVersion | string | 读 | R,Camera模块的MPU软件版本号 |
| CameraMcuSoftwareVersion | string | 读 | R,Camera模块的MCU软件版本号 |
| CameraPartNumber | string | 读 | R,摄像头控制线路板总成零件号F187 |
| SystemSupplierIdentificationNo | string | 读 | R,系统供应商标识号F18A |
| ElectricalControllerSerialNo | string | 读 | R,电控单元序列号F18C |
| SystemSupplierHardwareVersion | string | 读 | R,系统供应商硬件版本号F193 |
| SystemSupplierApplicationVersion | string | 读 | R,系统供应商软件版本号F195 |
| ElectricalControllerCanMatrixVersion | string | 读 | R,电控单元CAN矩阵版本F1A2 |
| Cv22PmuPowerGood | string | 读 | R,CV22配套PMUpowergood_FA03 |
| FirstGrade5VdcdcPowerGood | string | 读 | R,一级5VDCDCPowergood_FA03 |
| Cv22CriticalVoltagePwrCv1V579 | string | 读 | R,CV22关键电源PWR_CV_1V579_FA03 |
| DebounceGpio | string | 读 | R,DEBOUNCE_GPIO_FA03 |
| Kl30Voltage | double | 读 | R,KL30电压_FA04 |
| PlateCardHighTemperatureState | string | 读 | R,板卡温度_高温状态_FA05 |
| PlateCardLowTemperatureState | string | 读 | R,板卡温度_极低温状态_FA05 |
| PwrCvsys1V8 | string | 读 | R,PWR_CVSYS_1V8电压状态_FA06 |
| PwrDdr1V1 | string | 读 | R,PWR_DDR_1V1电压状态_FA06 |
| PwrCvVdd0V75 | string | 读 | R,PWR_CV_VDD_0V75电压状态_FA06 |
| PwrCvsys3V3 | string | 读 | R,PWR_CVSYS_3V3电压状态_FA06 |
| PwrCvVdda0V8 | string | 读 | R,PWR_CV_VDDA_0V8电压状态_FA06 |
| PmicVddio3V | string | 读 | R,PMIC_VDDIO_3V电压状态_FA06 |
| PwrDdr1V8 | string | 读 | R,PWR_DDR_1V8电压状态_FA06 |
| PwrEmmc1V8 | string | 读 | R,PWR_eMMC_1V8电压状态_FA06 |
| PwrCv1V579 | string | 读 | R,PWR_CV_1V579电压状态_FA06 |
| EthVddo1V0 | string | 读 | R,ETH_VDDO_1V0电压状态_FA06 |
| FrontCameraHighSideVolt | double | 读 | R,前视摄像头高边开关输出电压_FA0E |
| First5VDcdcVolt | double | 读 | R,一级5VDCDC输出电压_FA0F |
| AdbCameraBuckBoostVolt | double | 读 | R,ADB-camerabuck-boost输出电压检测_FA01 |
| SignalVolt | double | 读 | R,休眠唤醒信号线电压_FA02 |
| FrontCameraVideo | string | 读 | R,前视摄像头视频_FA00 |
| FrontCameraBuckBoostPowerGood | string | 读 | R,前视摄像头buck-boostpowerGood_FA00 |
| FrontCameraHighSideState | string | 读 | R,前视摄像头高边开关状态_FA00 |
| DmsVideo | string | 读 | R,DMS视频_FA00 |
| EthernetState | string | 读 | R,以太网状态_FA00 |
| Cvv2ZoneState | string | 读 | R,CV22分区检测_FA00 |
| BoardId | string | 读 | R,BoardID_FA00 |
| Cv22ProgramState | string | 读 | R,CV22烧写信息_FB01 |
| McuInfo | string | 读 | R,MCU信息_FB01 |
| EcuProgrammingProcessFileNumber | string | 读 | R,读取物流配置 |
| ProductionDate | string | 读 | R,生产日期 |
| SerialNo | string | 读 | R,序列号 |
| Barcode | string | 读/写 | R/W,条码 |
| LeftDlpcHsdError | string | 读 | R,左DLPC HSD错误 |
| RightDlpcHsdError | string | 读 | R,右DLPC HSD错误 |
| FpdLinkExecute | string | 读 | R,FPD链接执行 |