# DongFengH77RearLamp

## Controller Description

CAN-Product,东风岚图H77

## Public Fields

| 名称 | 描述 | 类型 |
|------|------|------|
| CurrentNode | R,当前诊断灯具 | string |
| EcuName | R,ECU名称[F197] | string |
| InternalAppVersion | R,SeeYao内部版本号[F195] | string |
| InternalBootVersion | R,SeeYao内部Boot版本号[F180] | string |
| DongFengAppVersion | R,东风定义APP版本号[F189] | string |
| DongFengHwVersion | R,东风定义HW版本号[F089] | string |
| DongFengPartNo | R,东风定义零件号[F187] | string |
| EcuSerialNo | R,ECU流水编号[F18C] | string |
| CustomHwVersion | R,供应商HW版本号[F193] | string |
| CustomName | R,供应商名称[F18A] | string |
| InternalCalVersion | R,SeeYao内部CAL版本号[1500] | string |
| IsCheckLocalHistory | R/W,是否检查本地历史[F18C] | bool |
| BindingBarcode | R,绑定二维码 | string |
| ClearDtcResult | R,诊断-清除DTC结果 | string |
| ReadDtcResult | R,诊断-读取DTC结果 | string |
| CheckWelcomeShowUartCan1DataResult | R,检测动画时UartCan1的数据结果 | string |
| CheckWelcomeShowUartCan2DataResult | R,检测动画时UartCan2的数据结果 | string |
| CheckParkingShowUartCan1DataResult | R,检测驻车动画时UartCan1的数据结果 | string |
| CheckParkingShowUartCan2DataResult | R,检测驻车动画时UartCan2的数据结果 | string |

## Public Methods

| 方法名称 | 描述 | 参数 |
|----------|------|------|
| StartCanMsg | 开启CAN消息 |  |
| StopCanMsg | 关闭CAN消息 |  |
| SetDiagnosticLeftRCLA | 切换成A左-默认 |  |
| SetDiagnosticRightRCLA | 切换成A右 |  |
| SetDiagnosticRCLB | 切换成B |  |
| ReadEcuName | ReadECU名称[F197] |  |
| ReadInternalAppVersion | ReadSeeYao内部版本号[F195] |  |
| ReadInternalBootVersion | ReadSeeYao内部Boot版本号[F180] |  |
| ReadDongFengAppVersion | Read东风定义APP版本号[F189] |  |
| ReadDongFengHwVersion | Read东风定义HW版本号[F089] |  |
| ReadDongFengPartNo | Read东风定义零件号[F187] |  |
| ReadCustomHwVersion | Read供应商HW版本号[F193] |  |
| ReadCustomName | Read供应商名称[F18A] |  |
| ReadInternalCalVersion | ReadSeeYao内部CAL版本号[1500] |  |
| WriteCustomSerialNo | 写供应商代码 | prNo, hwNo |
| ClearDtc | 清除DTC |  |
| ReadDtc | 读取DTC |  |
| AddDtcCodeIntoWhiteList | 将DTC添加进白名单 | code |
| ReadLeftTurnLampFault | 读取左转向灯故障 | ms |
| ReadLeftPositionLampFault | 读取左位置灯故障 | ms |
| ReadLeftStopLampFault | 读取左制动灯故障 | ms |
| ReadLeftFogLampFault | 读取左雾灯故障 | ms |
| ReadRightTurnLampFault | 读取右转向灯故障 | ms |
| ReadRightPositionLampFault | 读取右位置灯故障 | ms |
| ReadRightStopLampFault | 读取右制动灯故障 | ms |
| ReadRightFogLampFault | 读取右雾灯故障 | ms |
| ReadFbBackUp | 读取倒车灯故障 | ms |
| ReadRLM_MidApositionLampFault | 读取中位置灯故障 | ms |
| ReadRLM_MidLeftTurnLampFault | 读取中左转向灯故障 | ms |
| ReadRLM_MidLeftStopLampFault | 读取中左制动灯故障 | ms |
| ReadRLM_LicenseLampFault | 读取牌照灯故障 | ms |
| ReadRLM_HighStopLampFault | 读取高位制动灯故障 | ms |
| ReadRLM_MidRightTurnLampFault | 读取中右转向灯故障 | ms |
| ReadRLM_AdasBlueLampFault | 读取ADAS蓝灯故障 | ms |
| RMRearLampOn | B灯位置灯开 |  |
| RMRearLampOff | B灯位置灯关 |  |
| LeftRearLampOn | 左尾灯开 |  |
| LeftRearLampOff | 左尾灯关 |  |
| RightRearLampOn | 右尾灯开 |  |
| RightRearLampOff | 右尾灯关 |  |
| StopOn | 制动灯开 |  |
| StopOff | 制动灯关 |  |
| FogOn | 雾灯开 |  |
| FogOff | 雾灯关 |  |
| ReversingOn | 倒车灯开 |  |
| ReversingOff | 倒车灯关 |  |
| LicenseLampOn | 牌照灯开 |  |
| LicenseLampOff | 牌照灯关 |  |
| LeftTurnOn | 左转开 |  |
| LeftTurnOff | 左转关 |  |
| RightTurnOn | 右转开 |  |
| RightTurnOff | 右转关 |  |
| TurnLampSerialOn | 转向流水使能 |  |
| TurnLampSerialOff | 转向流水失能 |  |
| LogoDynamicOn | logoDynamic开 |  |
| LogoStaticOn | logoStatic开 |  |
| LogoOff | logo关 |  |
| ParkingAnimationOn | 驻车动画开 |  |
| ParkingAnimationOff | 驻车动画关 |  |
| BlueLampOn | 小蓝灯开 |  |
| BlueLampOff | 小蓝灯关 |  |
| WelcomeOn | welcome打开 | mode |
| WelcomeOff | welcome关闭 |  |
| Turn400MsOn | 转向400毫秒使能 |  |
| Turn400MsOff | 转向400毫秒失能 |  |
