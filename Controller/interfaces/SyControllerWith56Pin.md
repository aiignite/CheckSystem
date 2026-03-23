# SyControllerWith56Pin 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`, `ICcdIoController`
**控制器描述**: CAN-Device

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| SyControllerWith56Pin | CAN-Device | GatwayCan1 | CanBus | 字段 | - | 网关CAN1 | - |
| SyControllerWith56Pin | CAN-Device | GatwayCan2 | CanBus | 字段 | - | 网关CAN2 | - |
| SyControllerWith56Pin | CAN-Device | GatewayLin | LinBus | 字段 | - | 网关LIN | - |
| SyControllerWith56Pin | CAN-Device | GatewaySci2 | MySerialPort | 字段 | - | 网关SCI2 | - |
| SyControllerWith56Pin | CAN-Device | GatewaySci3 | MySerialPort | 字段 | - | 网关SCI3 | - |
| SyControllerWith56Pin | CAN-Device | GatewaySci6 | MySerialPort | 字段 | - | 网关SCI6 | - |
| SyControllerWith56Pin | CAN-Device | GatheringFrequency | int | 字段 | - | R/W,单次采集次数 | GatheringFrequency = 10 |
| SyControllerWith56Pin | CAN-Device | GatheringTime | ushort | 字段 | - | R/W,单次采样时间 | GatheringTime = 250 |
| SyControllerWith56Pin | CAN-Device | Current1 | float | 字段 | - | R,电流1 | - |
| SyControllerWith56Pin | CAN-Device | Current2 | float | 字段 | - | R,电流2 | - |
| SyControllerWith56Pin | CAN-Device | Current3 | float | 字段 | - | R,电流3 | - |
| SyControllerWith56Pin | CAN-Device | Current4 | float | 字段 | - | R,电流4 | - |
| SyControllerWith56Pin | CAN-Device | Voltage1 | float | 字段 | - | R,电压1 | - |
| SyControllerWith56Pin | CAN-Device | Voltage2 | float | 字段 | - | R,电压2 | - |
| SyControllerWith56Pin | CAN-Device | Voltage3 | float | 字段 | - | R,电压3 | - |
| SyControllerWith56Pin | CAN-Device | Voltage4 | float | 字段 | - | R,电压4 | - |
| SyControllerWith56Pin | CAN-Device | Voltage5 | float | 字段 | - | R,电压5 | - |
| SyControllerWith56Pin | CAN-Device | Voltage6 | float | 字段 | - | R,电压6 | - |
| SyControllerWith56Pin | CAN-Device | Di1 | string | 字段 | - | R,输入1 | - |
| SyControllerWith56Pin | CAN-Device | Di2 | string | 字段 | - | R,输入2 | - |
| SyControllerWith56Pin | CAN-Device | HighSide1Frequency | float | 字段 | - | R/W,输出频率占空比_high_side1_frequency | - |
| SyControllerWith56Pin | CAN-Device | HighSide1Duty | float | 字段 | - | R/W,输出频率占空比_high_side1_duty | - |
| SyControllerWith56Pin | CAN-Device | HighSide2Frequency | float | 字段 | - | R/W,输出频率占空比_high_side2_frequency | - |
| SyControllerWith56Pin | CAN-Device | HighSide2Duty | float | 字段 | - | R/W,输出频率占空比_high_side2_duty | - |
| SyControllerWith56Pin | CAN-Device | DoHighSide1 | bool | 字段 | - | R/W,高边输出1 | - |
| SyControllerWith56Pin | CAN-Device | DoHighSide2 | bool | 字段 | - | R/W,高边输出2 | - |
| SyControllerWith56Pin | CAN-Device | DoLowSide1 | bool | 字段 | - | R/W,低边输出1 | - |
| SyControllerWith56Pin | CAN-Device | DoLowSide2 | bool | 字段 | - | R/W,低边输出2 | - |
| SyControllerWith56Pin | CAN-Device | DoLowSide3 | bool | 字段 | - | R/W,低边输出3 | - |
| SyControllerWith56Pin | CAN-Device | DoLowSide4 | bool | 字段 | - | R/W,低边输出4 | - |
| SyControllerWith56Pin | CAN-Device | Relay1 | bool | 字段 | - | R/W,继电器1 | - |
| SyControllerWith56Pin | CAN-Device | Relay2 | bool | 字段 | - | R/W,继电器2 | - |
| SyControllerWith56Pin | CAN-Device | Relay3 | bool | 字段 | - | R/W,继电器3 | - |
| SyControllerWith56Pin | CAN-Device | Relay4 | bool | 字段 | - | R/W,继电器4 | - |
| SyControllerWith56Pin | CAN-Device | Relay5 | bool | 字段 | - | R/W,继电器5 | - |
| SyControllerWith56Pin | CAN-Device | Relay6 | bool | 字段 | - | R/W,继电器6 | - |
| SyControllerWith56Pin | CAN-Device | Relay7 | bool | 字段 | - | R/W,继电器7 | - |
| SyControllerWith56Pin | CAN-Device | Relay8 | bool | 字段 | - | R/W,继电器8 | - |
| SyControllerWith56Pin | CAN-Device | Relay9 | bool | 字段 | - | R/W,继电器9 | - |
| SyControllerWith56Pin | CAN-Device | Relay10 | bool | 字段 | - | R/W,继电器10 | - |
| SyControllerWith56Pin | CAN-Device | Relay11 | bool | 字段 | - | R/W,继电器11 | - |
| SyControllerWith56Pin | CAN-Device | Relay12 | bool | 字段 | - | R/W,继电器12 | - |
| SyControllerWith56Pin | CAN-Device | Pwm1CaptureFrequency | float | 字段 | - | R,输入捕获频率占空比_PWM1_Capture_Freq | - |
| SyControllerWith56Pin | CAN-Device | Pwm1CaptureDuty | float | 字段 | - | R,输入捕获频率占空比_PWM1_Capture_Duty | - |
| SyControllerWith56Pin | CAN-Device | Pwm2CaptureFrequency | float | 字段 | - | R,输入捕获频率占空比_PWM2_Capture_Freq | - |
| SyControllerWith56Pin | CAN-Device | Pwm2CaptureDuty | float | 字段 | - | R,输入捕获频率占空比_PWM2_Capture_Duty | - |
| SyControllerWith56Pin | CAN-Device | Current1Ad | float | 字段 | - | R,电流1-AD | - |
| SyControllerWith56Pin | CAN-Device | Current2Ad | float | 字段 | - | R,电流2-AD | - |
| SyControllerWith56Pin | CAN-Device | Current3Ad | float | 字段 | - | R,电流3-AD | - |
| SyControllerWith56Pin | CAN-Device | Current4Ad | float | 字段 | - | R,电流4-AD | - |
| SyControllerWith56Pin | CAN-Device | Voltage1Ad | float | 字段 | - | R,电压1-AD | - |
| SyControllerWith56Pin | CAN-Device | Voltage2Ad | float | 字段 | - | R,电压2-AD | - |
| SyControllerWith56Pin | CAN-Device | Voltage3Ad | float | 字段 | - | R,电压3-AD | - |
| SyControllerWith56Pin | CAN-Device | Voltage4Ad | float | 字段 | - | R,电压4-AD | - |
| SyControllerWith56Pin | CAN-Device | Voltage5Ad | float | 字段 | - | R,电压5-AD | - |
| SyControllerWith56Pin | CAN-Device | Voltage6Ad | float | 字段 | - | R,电压6-AD | - |
| SyControllerWith56Pin | CAN-Device | Can1ToCan2TransferTime | float | 字段 | - | R,CAN1到CAN2转发时间 | - |
| SyControllerWith56Pin | CAN-Device | Can2ToCan1TransferTime | float | 字段 | - | R,CAN2到CAN1转发时间 | - |
| SyControllerWith56Pin | CAN-Device | Mcrotime | float | 字段 | - | R,启动时间 | - |
| SyControllerWith56Pin | CAN-Device | FilterType | int | 字段 | - | R/W,FilterType | FilterType = 0 |
| SyControllerWith56Pin | CAN-Device | InitRemoteIpAddress | void | 方法 | string ipPort | 初始化远程IP地址 | InitRemoteIpAddress("192.168.1.100:5000") |
| SyControllerWith56Pin | CAN-Device | SetOutputs | bool | 方法 | - | 设置高低边以及继电器输出 | SetOutputs() |
| SyControllerWith56Pin | CAN-Device | GetInputs | void | 方法 | - | 获取输入 | GetInputs() |
| SyControllerWith56Pin | CAN-Device | ResetOutPuts | void | 方法 | - | 重置输出 | ResetOutPuts() |
| SyControllerWith56Pin | CAN-Device | GetOutputs | void | 方法 | - | 获取高低边以及继电器输出状态 | GetOutputs() |
| SyControllerWith56Pin | CAN-Device | StartAutoRefresh | void | 方法 | - | 开启自动获取电流电压及DI | StartAutoRefresh() |
| SyControllerWith56Pin | CAN-Device | CanelAutoRefresh | void | 方法 | - | 取消自动获取电流电压及DI | CanelAutoRefresh() |
| SyControllerWith56Pin | CAN-Device | GetCurrentVoltageDi | void | 方法 | - | 获取电流电压及DI | GetCurrentVoltageDi() |
| SyControllerWith56Pin | CAN-Device | GetAds | void | 方法 | - | 获取AD值 | GetAds() |
| SyControllerWith56Pin | CAN-Device | GetFrequencyCurr | void | 方法 | - | 按频率采集电流 | GetFrequencyCurr() |
| SyControllerWith56Pin | CAN-Device | GetSingleCurr | void | 方法 | string currIndex | 采集单通道电流 | GetSingleCurr("1") |
| SyControllerWith56Pin | CAN-Device | GetFrequencyVolt | void | 方法 | - | 按频率采集电压 | GetFrequencyVolt() |
| SyControllerWith56Pin | CAN-Device | GetSingleVolt | void | 方法 | string voltIndex | 采集单通道电压 | GetSingleVolt("1") |
| SyControllerWith56Pin | CAN-Device | ReadHighsideFrequencyDuty | void | 方法 | - | 读高边输出频率占空比 | ReadHighsideFrequencyDuty() |
| SyControllerWith56Pin | CAN-Device | ReadPwm | void | 方法 | - | 读输入捕获频率占空比 | ReadPwm() |
| SyControllerWith56Pin | CAN-Device | ReadFlash | bool | 方法 | - | 读FLASH | ReadFlash() |
| SyControllerWith56Pin | CAN-Device | GetCan1ToCan2TransferTime | bool | 方法 | string msg | 获取CAN1到CAN2转发时间 | GetCan1ToCan2TransferTime("0x123,01020304") |
| SyControllerWith56Pin | CAN-Device | GetCan2ToCan1TransferTime | bool | 方法 | string msg | 获取CAN2到CAN1转发时间 | GetCan2ToCan1TransferTime("0x123,01020304") |
| SyControllerWith56Pin | CAN-Device | GetCurrentRisingTime | void | 方法 | string adIndex, ushort maxValue | 读电流上升时间 | GetCurrentRisingTime("1", 1000) |
| SyControllerWith56Pin | CAN-Device | LinSetBaudRate | void | 方法 | int value | LIN设置波特率 | LinSetBaudRate(192) |
