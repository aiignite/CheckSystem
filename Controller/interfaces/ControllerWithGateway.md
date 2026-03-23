# ControllerWithGateway

## Controller Description

网关控制器

## Public Fields

| 名称 | 描述 | 类型 |
|------|------|------|
| Current1 | R,电流1 | float |
| Current2 | R,电流2 | float |
| Current3 | R,电流3 | float |
| Current4 | R,电流4 | float |
| Voltage1 | R,电压1 | float |
| Voltage2 | R,电压2 | float |
| Voltage3 | R,电压3 | float |
| Voltage4 | R,电压4 | float |
| Voltage5 | R,电压5 | float |
| Relay1 | R/W,继电器1 | bool |
| Relay2 | R/W,继电器2 | bool |
| Relay3 | R/W,继电器3 | bool |
| Relay4 | R/W,继电器4 | bool |
| Relay5 | R/W,继电器5 | bool |
| Relay6 | R/W,继电器6 | bool |
| Relay7 | R/W,继电器7 | bool |
| Relay8 | R/W,继电器8 | bool |
| Relay9 | R/W,继电器9 | bool |
| Input1 | R,DI | bool? |

## Public Methods

| 方法名称 | 描述 | 参数 |
|----------|------|------|
| InitRemoteIpAddress | 初始化远程IP地址 | ipPort |
| InitSerialPort | 初始化下位机的串口 | strPort |
| SetRelays | 设置继电器 |  |
| ResetAllRelays | 重置所有继电器 |  |
| UpdateCurrsAndVolts | 更新电流和电压 |  |
| UpdateDelayTime | 更新延时时间 |  |
| ReadInput | 读取DI状态 |  |
| OpenOut1Pwm | 打开通道1的PWM输出 | hzPercentage |
| CloseOut1Pwm | 关闭通道1的PWM输出 |  |
| OpenOut2Pwm | 打开通道2的PWM输出 | hzPercentage |
| CloseOut2Pwm | 关闭通道2的PWM输出 |  |
