# H53B_HCM_HeadLamp

## Controller Description

CAN-Product,H53B

## Public Fields

| 名称 | 描述 | 类型 |
|------|------|------|
| BCM_RightTurnLampReq | R/W, 右转向灯请求信号 | byte |
| BCM_FRdrlReq | R/W, 右前昼间行驶灯请求信号 | byte |
| BCM_FLdrlReq | R/W, 左前昼间行驶灯请求信号 | byte |
| BCM_EXLHeightAdjtReq | R/W, 大灯高度手动调节请求 | byte |
| BCM_ADBModeReq | R/W, ADB模式请求 | byte |
| BCM_FLPosLampReq | R/W, 左前位置灯请求信号 | byte |
| BCM_FRPosLampReq | R/W, 右前位置灯请求信号 | byte |
| BCM_LowBeamReq | R/W, 近光灯请求信号 | byte |
| BCM_HighBeamReq | R/W, 远光灯请求信号 | byte |
| BCM_LeftTurnLampReq | R/W, 左转向灯请求信号 | byte |
| BCM_turnLampFlickerReq | R/W, 转向灯闪烁频率请求 | byte |
| BCM_turnLampSerialReq | R/W, 转向灯流水请求 | byte |
| RollingCounter2A1 | R/W, every message increments the counter | byte |
| Checksum2A1 | R/W, CRC8, checksum | byte |

## Public Methods

| 方法名称 | 描述 | 参数 |
|----------|------|------|
| LinStartScheduler | 打开Can发送 |  |
| LinStopScheduler | 关闭Can发送 |  |
| TurnLOn | TurnLOn |  |
| TurnLOff | TurnLOff |  |
| TurnROn | TurnROn |  |
| TurnROff | TurnROff |  |
| DrlROn | DrlROn |  |
| DrlROff | DrlROff |  |
| DrlLOn | DrlLOn |  |
| DrlLOff | DrlLOff |  |
| TailLOn | TailLOn |  |
| TailLOff | TailLOff |  |
| TailROn | TailROn |  |
| TailROff | TailROff |  |
| BCM_LowBeamReqOn | BCM_LowBeamReqOn |  |
| BCM_LowBeamReqOff | BCM_LowBeamReqOff |  |
| BCM_HighBeamReqOn | BCM_HighBeamReqOn |  |
| BCM_HighBeamReqOff | BCM_HighBeamReqOff |  |
| BCM_LeftTurnLampReqOn | BCM_LeftTurnLampReqOn |  |
| BCM_LeftTurnLampReqOff | BCM_LeftTurnLampReqOff |  |
| BCM_turnLampFlickerReqOn | BCM_turnLampFlickerReqOn | value |
| BCM_turnLampSerialReqOn | BCM_turnLampSerialReqOn |  |
| BCM_turnLampSerialReqOff | BCM_turnLampSerialReqOff |  |
| LeftTurnLampFlickerOn | 左转向灯闪烁开 |  |
