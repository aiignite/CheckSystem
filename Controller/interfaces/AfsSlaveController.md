# AfsSlaveController

## Controller Description

LIN-Product,AFS_Slave_Controller

## Public Fields

| 名称 | 描述 | 类型 |
|------|------|------|
| RecvData0X34 | R,左侧DHL状态反馈 | string |
| RecvData0X35 | R,右侧DHL状态反馈 | string |
| RecvData0X36 | R,左侧AFL状态反馈 | string |
| RecvData0X37 | R,右侧AFL状态反馈 | string |
| NodePartNo | R,供货零件号 | string |
| NodeProductionDate | R,生产日期 | string |
| NodeSerialNo | R,生产序列号 | string |
| NodeHardwareVersion | R,硬件版本 | string |
| NodeSoftwareVersion | R,软件版本 | string |
| NodeSoftwarePartNo | R,软件零件号 | string |
| NodeCurrentNode | R,当前节点 | string |
| DhlInitCompleteFlag | R,DHL马达初始化完成标志位 | string |
| DhlStatus | R,DHL马达状态 | string |
| DhlUnderVoltageFaultEnable | R,DHL从节点低压故障检测开启 | string |
| DhlUnderVoltageFault | R,DHL从节点低压故障 | string |
| DhlOverVoltageFaultEnable | R,DHL从节点过压故障检测开启 | string |
| DhlOverVoltageFault | R,DHL从节点过压故障 | string |
| DhlOpenCircuitFaultEnable | R,DHL开路故障检测开启 | string |
| DhlOpenCircuitFault | R,DHL开路故障 | string |
| DhlGroundShortFaultEnable | R,DHL对地短路故障检测开启 | string |
| DhlGroundShortFault | R,DHL对地短路故障 | string |
| DhlTempAlarmEnable | R,DHL马达芯片温度报警检测开启 | string |
| DhlTempAlarm | R,DHL马达芯片温度报警 | string |
| DhlLogicPosition | R,DHL马达逻辑位置 | string |
| AflHallLearnFlag | R,AFL霍尔学习标志 | string |
| AflInitCompleteFlag | R,AFL马达初始化完成标志位 | string |
| AflStatus | R,AFL马达状态 | string |
| AflUnderVoltageFaultEnable | R,AFL从节点低压故障检测开启 | string |
| AflUnderVoltageFault | R,AFL从节点低压故障 | string |
| AflOverVoltageFaultEnable | R,AFL从节点过压故障检测开启 | string |
| AflOverVoltageFault | R,AFL从节点过压故障 | string |
| AflGroundOpenFaultEnable | R,AFL马达对地开路检测开启 | string |
| AflGroundOpenFault | R,AFL马达对地开路 | string |
| AflShortFaultEnable | R,AFL马达短路故障检测开启 | string |
| AflShortFault | R,AFL马达短路 | string |
| AflHallFaultEnable | R,AFL-Hall故障检测开启 | string |
| AflHallFault | R,AFL-Hall故障 | string |
| AflSpeedConfigId | R,AFL速度配置表ID | string |
| AflTempAlarmEnable | R,AFL温度报警检测开启 | string |
| AflTempAlarm | R,AFL马达温度报警 | string |
| AflHidFaultEnable | R,AFL-Hid故障检测开启 | string |
| AflHidFault | R,AFL-Hid故障 | string |
| AflLogicPosition | R,AFL马达逻辑位置 | string |
| TargetGear | R/W,运动到目标档位 | string |
| LeftDhlPosition | R/W,左DHL位置 | string |
| DhlMotorDirectionConfig | R/W,DHL马达运动方向配置 | string |
| LeftDhlInitCmd | R/W,左DHL马达初始化命令位 | string |
| LeftAflPosition | R/W,左AFL位置 | string |
| MotorDriveLeftRightRatio | R/W,马达驱动左右速度比 | string |
| AflSpeedConfig | R/W,AFL速度配置表 | string |
| LeftAflInitCmd | R/W,左AFL马达初始化命令位 | string |
| RightAflPosition | R/W,右AFL位置 | string |
| RightAflInitCmd | R/W,右AFL马达初始化命令位 | string |
| RightDhlPosition | R/W,右DHL位置 | string |
| RightDhlInitCmd | R/W,右DHL马达初始化命令位 | string |

## Public Methods

| 方法名称 | 描述 | 参数 |
|----------|------|------|
| SlaveAwake | AFS唤醒 |  |
| SlaveSleep | AFS休眠 |  |
| StartAutoScanStatus | 开启自动读取AFS状态 |  |
| StopAutoScanStatus | 停止自动读取AFS状态 |  |
| CurrentDhlLeft | 当前为DHL左节点 |  |
| CurrentDhlRight | 当前为DHL右节点 |  |
| CurrentAflLeft | 当前为AFL左节点 |  |
| CurrentAflRight | 当前为AFL右节点 |  |
| ReadNodeConfigInfo | 读节点配置信息 |  |
| MotorMove | 电机移动 |  |
| ReadStatusInfo | 读取状态信息 |  |
| WriteConfigLeftDhl | 写入配置为左DHL |  |
| WriteConfigRightDhl | 写入配置为右DHL |  |
| WriteConfigLeftAfl | 写入配置为左AFL |  |
| WriteConfigRightAfl | 写入配置为右AFL |  |
| MotorInit1 | 马达初始化1 |  |
| MotorInit2 | 马达初始化2 |  |
| MoveToOpticalZero | 运动到光学零位 |  |
| MoveToUpLimit | 运动到上极限 |  |
| MoveToDownLimit | 运动到下极限 |  |
| LeftMotorMoveToTarget | 左马达运动到目标位置 | target |
| RightMotorMoveToTarget | 右马达运动到目标位置 | target |
