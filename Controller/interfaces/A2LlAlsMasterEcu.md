# A2LlAlsMasterEcu

## Controller Description

LIN-Product,A2LL ALS Master

## Public Fields

| 名称 | 描述 | 类型 |
|------|------|------|
| FblVersion | R,引导程序版本号 | string |
| HardwareVersion | R,硬件版本号 | string |
| PartNo | R,ECU供货零件号 | string |
| Date | R,生产日期 | string |
| SerialNo | R,生产序列号 | string |
| AppVersion | R,应用程序版本号 | string |
| AppPartNo | R,应用程序零件号 | string |
| ServerIp | R/W,服务器IP地址 | string |
| ServerDataBase | R/W,服务器数据库名称 | string |
| ServerUid | R/W,服务器用户名 | string |
| ServerPwd | R/W,服务器用密码 | string |
| ModuleSupplyVolt | R,模块电源电压 | double |
| FrontSensorVolt | R,前传感器信号 | double |
| RearSensorVolt | R,后传感器信号 | double |
| SensorSupplyVolt | R,传感器电源电压 | double |
| MotorDirectionConfigResult | R,马达运动方向配置 | string |
| MotorPositionConfigResult | R,DHL马达位置信息配置 | string |
| MotorInitResult | R,控制DHL马达初始化 | string |
| MotorRunResult | R,控制DHL马达运动 | string |
| MotorPositionReadResult | R,读取DHL马达当前实际位置 | string |
| MotorOpenError | R,读取DHL马达开路故障 | string |
| MotorShortError | R,读取DHL马达短路故障 | string |
| ErrorClearResult | R,故障清除结果 | string |

## Public Methods

| 方法名称 | 描述 | 参数 |
|----------|------|------|
| ModuleSleep | ECU休眠 |  |
| ModuleAwake | ECU唤醒 |  |
| InitMasterLinId | 设置命令帧LIN-ID | masterLinId |
| InitSlavaLinId | 设置响应帧LIN-ID | slaveLinId |
| ReadFblVersion | Read引导程序版本号 |  |
| ReadAppVersion | Read应用程序版本号 |  |
| ReadAppPartNo | Read应用程序零件号 |  |
| WritrTraceInfo | 写追溯 | partNo, hardwareNo |
| ReadPartNo | Read供货零件号 |  |
| ReadHardwareVersion | Read硬件版本号 |  |
| ReadProductionDate | Read生产日期 |  |
| ReadSerialNo | Read生产序列号 |  |
| ReadSensorSupplyVolt | 读传感器电源电压 |  |
| ReadFrontRearSensorSignal | 读前后传感器信号 |  |
| ReadModuleSupplyVolt | 读模块电源电压 |  |
| MotorDirectionConfig | 马达运动方向配置 |  |
| MotorPositionConfig | DHL马达位置信息配置 |  |
| MotorInit | 控制DHL马达初始化 |  |
| MotorRun | 控制DHL马达运动 |  |
| ReadMotorPosition | 读取DHL马达当前实际位置 |  |
| ReadMotorOpenShortError | 读取DHL马达开路短路故障 |  |
| MotorMoveToDownLimit | DHL马达运动到下极限确认调光角度 |  |
| MotorResetToOpticalZero | DHL马达回到复位位置-光学零位 |  |
| ClearError | 故障清除 |  |
| GenerateQrCode | 生成二维码 |  |
| PrintBarcode | Print Barcode |  |
