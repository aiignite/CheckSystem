# A88FrontLamp

## Controller Description

CAN-Product,A88前灯

## Public Fields

| 名称 | 描述 | 类型 |
|------|------|------|
| DrlPlPwm | R/W,DrlPL_PWM | string |
| TlPwm | R/W,TL_PWM | string |
| HbPwm | R/W,Hb_PWM | string |
| LbPwm | R/W,Lb_PWM | string |
| AppVer | R,应用程序版本号 | string |
| FblVer | R,引导程序版本号 | string |
| CfgVer | R,配置程序版本号 | string |

## Public Methods

| 方法名称 | 描述 | 参数 |
|----------|------|------|
| SetCanId | 设置CANID | reqCanId, recvCanId |
| ReadAppVer | Read应用程序版本号 |  |
| ReadFblVer | Read引导程序版本号 |  |
| ReadCfgVer | Read配置程序版本号 |  |
| LbOn | 近光开 |  |
| LbOff | 近光关 |  |
| HbOn | 远光开 |  |
| HbOff | 远光关 |  |
| DrlOn | DRL开 |  |
| DrlOff | DRL关 |  |
| DrlSingleOn | DRL单颗开 |  |
| DrlSingleOff | DRL单颗关 |  |
| PlOn | PL开 |  |
| PlOff | PL关 |  |
| TurnOn | 转向开 |  |
| TurnOff | 转向关 |  |
| MotorFindMechanicalZero | 马达-找机械零位 |  |
| MotorToOpticalZero | 马达-到光学零位 |  |
| MotorToDownLimit | 马达-到下极限 |  |
| MotorToUpLimit | 马达-到上极限 |  |
