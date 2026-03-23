# S12LHeatPumpController 控制器接口文档

## 控制器信息

| 控制器名称 | 控制器描述 |
|-----------|-----------|
| S12LHeatPumpController | CAN-Product,S12L热泵控制器 |

## 公共字段

| 名称 | 类型 | 类别 | 描述 |
|------|------|------|------|
| IsSaveCanLog | bool | 读/写 | R/W,是否存储CAN日志 |
| SendSleepCmdResult | string | 读 | R,发送休眠指令结果 |
| SecurityAccessResult | string | 读 | R,安全访问结果 |
| ChangeZoneResult | string | 读 | R,切换分区结果 |
| EcuResetResult | string | 读 | R,ECU复位结果 |
| Mode | string | 读 | R,模式读取 |
| EcuSerialNumber | string | 读 | R,EcuSerialNumber-F18C |
| EcuBootloaderSoftwareReferenceNumber | string | 读 | R,EcuBootloaderSoftwareReferenceNumber-F183 |
| EcuPartNumber | string | 读 | R,ECUPartNumber-F187 |
| EcuHardwareNumber | string | 读 | R,ECUHardwareNumber-F191 |
| EcuSoftwareNumber | string | 读 | R,ECUSoftwareNumber-F1A0 |
| EcuCalibrationSoftwareNumber | string | 读 | R,EcuCalibrationSoftwareNumber-F1A1 |
| SsbHardwareNumber | string | 读 | R,SsbHardwareNumber-F192 |
| SsbSoftwareNumber | string | 读 | R,SsbSoftwareNumber-F194 |
| EcuNcfNumber | string | 读 | R,ECUNCFNumber-F1A2 |
| EcuConfigurationFileNumber | string | 读 | R,EcuConfigurationFileNumber-F1A9 |
| EcuProgrammingProcessFileNumber | string | 读 | R,EcuProgrammingProcessFileNumber-F1AA |
| SsbPartNumber | string | 读 | R,SsbPartNumber-FD01 |
| 初始功能配置码 | string | 读 | R,初始功能配置码-C000 |
| VbatVol | double | 读 | R,VBAT_Vol-0112 |
| Sensor5V | double | 读 | R,Sensor5V-B020 |
| ProgramFlag | string | 读 | R,烧写标志位-AFF0 |
| SoftwareValidFlag | string | 读 | R,软件有效性标志位-AFFF |
| SoftwareCompatibilityStatus | string | 读 | R,SoftwareCompatibilityStatus-AFFE |
| SoftwareIntegrityStatus | string | 读 | R,SoftwareIntegrityStatus-AFFD |
| ProgrammingCounter | string | 读 | R,刷新计数ProgrammingCounter-AFFC |
| FrontModeMotorCaliResult | string | 读 | R,左区模式风门电机标定结果 |
| RearModeMotorCaliResult | string | 读 | R,后区模式风门电机标定结果 |
| DefrostModeMotorCaliResult | string | 读 | R,除霜风门电机标定结果 |
| IntakeModeMotorCaliResult | string | 读 | R,内外循环风门电机标定结果 |
| ServoHepaMotorCaliResult | string | 读 | R,HEPA风门电机标定结果 |
| Aqs信号输入pwm2 | string | 读 | R,AQS信号输入PWM2-0xE019 |
| 冷却风扇pwm | string | 读 | R,冷却风扇PWM-0xE060 |
| 内冷阀HsdReadValue | string | 读 | R,内冷阀Hsd-0xE04F |
| 制热阀HsdReadValue | string | 读 | R,制热阀Hsd-0xE050 |
| 制冷阀HsdReadValue | string | 读 | R,制冷阀Hsd-0xE051 |
| 前风窗加热丝ReadValue | string | 读 | R,前风窗加热丝-0xE072 |
| 后风窗加热驱动ReadValue | string | 读 | R,后风窗加热驱动-0xE03C |
| Chiller电子膨胀阀ReadValue | string | 读 | R,Chiller电子膨胀阀-0xE041 |
| 制热电子膨胀阀ReadValue | string | 读 | R,制热电子膨胀阀-0xE056 |
| 前蒸发器电子膨胀阀ReadValue | string | 读 | R,前蒸发器电子膨胀阀-0xE058 |
| 左区模式风门电机位置反馈 | string | 读 | R,左区模式风门电机位置反馈-0xE001 |
| 后区模式风门电机位置反馈 | string | 读 | R,后区模式风门电机位置反馈-0xE003 |
| 内外循环风门电机位置反馈 | string | 读 | R,内外循环风门电机位置反馈-0xE002 |
| 除霜风门电机位置反馈 | string | 读 | R,除霜风门电机位置反馈-0xE006 |
| Hepa风门电机位置反馈 | string | 读 | R,HEPA风门电机位置反馈-0xE061 |
| Lin1MsgTest-Lin6MsgTest | string | 读 | R,LIN1-LIN6测试 |
| Can1MsgTest | string | 读 | R,CAN1测试 |
| Can2MsgTest | string | 读 | R,CAN2测试 |
| DownloadResult | string | 读 | R,下载结果 |
| AppFilePath | string | 读/写 | R/W,APP文件路径 |
| CaliFailPath | string | 读/写 | R/W,CAL文件路径 |
| ToWriteBarcode | string | 读/写 | R/W,写入条码 |
| WtireMtcResult | string | 读 | R,写入MTC结果 |