# SyControllerMaster 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: CAN-Device

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| SyControllerMaster | CAN-Device | DoHighSide1 | bool | 字段 | - | R/W,高边输出1 | - |
| SyControllerMaster | CAN-Device | DoHighSide2 | bool | 字段 | - | R/W,高边输出2 | - |
| SyControllerMaster | CAN-Device | DoLowSide1 | bool | 字段 | - | R/W,低边输出1 | - |
| SyControllerMaster | CAN-Device | DoLowSide2 | bool | 字段 | - | R/W,低边输出2 | - |
| SyControllerMaster | CAN-Device | Di1 | bool | 字段 | - | R,DI输入1 | - |
| SyControllerMaster | CAN-Device | Di2 | bool | 字段 | - | R,DI输入2 | - |
| SyControllerMaster | CAN-Device | AdCurrent1 | float | 字段 | - | R,电流1 | - |
| SyControllerMaster | CAN-Device | AdCurrent2 | float | 字段 | - | R,电流2 | - |
| SyControllerMaster | CAN-Device | AdVoltage1 | float | 字段 | - | R,电压1 | - |
| SyControllerMaster | CAN-Device | AdVoltage2 | float | 字段 | - | R,电压2 | - |
| SyControllerMaster | CAN-Device | AdVoltage3 | float | 字段 | - | R,电压3 | - |
| SyControllerMaster | CAN-Device | AdVoltage4 | float | 字段 | - | R,电压4 | - |
| SyControllerMaster | CAN-Device | MasterGatewayCan | ControllerMasterGatewayCan | 字段 | - | 主站CAN网关 | - |
| SyControllerMaster | CAN-Device | MasterGatewayLin | ControllerMasterGatewayLin | 字段 | - | 主站LIN网关 | - |
| SyControllerMaster | CAN-Device | MasterGatewaySci | ControllerMasterGatewaySci | 字段 | - | 主站串口网关 | - |
| SyControllerMaster | CAN-Device | GetTimeOutput | SyControllerSlaveWith10R | 字段 | - | 需要操作寄存器的从站 | - |
| SyControllerMaster | CAN-Device | GetTimeInput | SyControllerSlaveWith14Ad | 字段 | - | 需要监控电压或电流的从站 | - |
| SyControllerMaster | CAN-Device | Time | float | 字段 | - | 时间 | - |
| SyControllerMaster | CAN-Device | In1Frequency | float | 字段 | - | PWM输入1频率 | - |
| SyControllerMaster | CAN-Device | In1Duty | float | 字段 | - | PWM输入1占空比 | - |
| SyControllerMaster | CAN-Device | InitRemoteIpAddr | void | 方法 | string ipPort | 初始化远程IP地址 | InitRemoteIpAddr("192.168.1.150:8088") |
| SyControllerMaster | CAN-Device | GetMasterAd | bool | 方法 | 无 | 获取主站电流电压值 | GetMasterAd() |
| SyControllerMaster | CAN-Device | GetMasterDi | bool | 方法 | 无 | 获取主站DI值 | GetMasterDi() |
| SyControllerMaster | CAN-Device | GetMasterDoHighSide | bool | 方法 | 无 | 获取主站高边输出状态 | GetMasterDoHighSide() |
| SyControllerMaster | CAN-Device | GetMasterDoLowSide | bool | 方法 | 无 | 获取主站低边输出状态 | GetMasterDoLowSide() |
| SyControllerMaster | CAN-Device | GetPwm | bool | 方法 | 无 | 获取PWM值 | GetPwm() |
| SyControllerMaster | CAN-Device | SetMasterDoHighSide | void | 方法 | 无 | 设置主站高边输出 | SetMasterDoHighSide() |
| SyControllerMaster | CAN-Device | SetMasterDoLowSide | void | 方法 | 无 | 设置主站低边输出 | SetMasterDoLowSide() |
| SyControllerMaster | CAN-Device | OpenMasterDoHs1Pwm | void | 方法 | ushort doHs1PwmFrequency, ushort doHs1PwmDuty | 打开主站PWM1 | OpenMasterDoHs1Pwm(1000, 50) |
| SyControllerMaster | CAN-Device | CloseMasterDoHs1Pwm | void | 方法 | 无 | 关闭主站PWM1 | CloseMasterDoHs1Pwm() |
| SyControllerMaster | CAN-Device | OpenMasterDoHs2Pwm | void | 方法 | ushort doHs2PwmFrequency, ushort doHs2PwmDuty | 打开主站PWM2 | OpenMasterDoHs2Pwm(1000, 50) |
| SyControllerMaster | CAN-Device | CloseMasterDoHs2Pwm | void | 方法 | 无 | 关闭主站PWM2 | CloseMasterDoHs2Pwm() |
| SyControllerMaster | CAN-Device | GetMasterAdCurrent1RisingTime | void | 方法 | string threshold | 获取主站电流1上升时间 | GetMasterAdCurrent1RisingTime("2.5") |
| SyControllerMaster | CAN-Device | GetMasterAdCurrent2RisingTime | void | 方法 | string threshold | 获取主站电流2上升时间 | GetMasterAdCurrent2RisingTime("2.5") |
| SyControllerMaster | CAN-Device | GetMasterAdVoltage1RisingTime | void | 方法 | string threshold | 获取主站电压1上升时间 | GetMasterAdVoltage1RisingTime("2.5") |
| SyControllerMaster | CAN-Device | GetMasterAdVoltage2RisingTime | void | 方法 | string threshold | 获取主站电压2上升时间 | GetMasterAdVoltage2RisingTime("2.5") |
| SyControllerMaster | CAN-Device | GetMasterAdVoltage3RisingTime | void | 方法 | string threshold | 获取主站电压3上升时间 | GetMasterAdVoltage3RisingTime("2.5") |
| SyControllerMaster | CAN-Device | GetMasterAdVoltage4RisingTime | void | 方法 | string threshold | 获取主站电压4上升时间 | GetMasterAdVoltage4RisingTime("2.5") |
| SyControllerMaster | CAN-Device | GetMasterAdCurrent1FailingTime | void | 方法 | string threshold | 获取主站电流1下降时间 | GetMasterAdCurrent1FailingTime("2.5") |
| SyControllerMaster | CAN-Device | GetMasterAdCurrent2FailingTime | void | 方法 | string threshold | 获取主站电流2下降时间 | GetMasterAdCurrent2FailingTime("2.5") |
| SyControllerMaster | CAN-Device | GetMasterAdVoltage1FailingTime | void | 方法 | string threshold | 获取主站电压1下降时间 | GetMasterAdVoltage1FailingTime("2.5") |
| SyControllerMaster | CAN-Device | GetMasterAdVoltage2FailingTime | void | 方法 | string threshold | 获取主站电压2下降时间 | GetMasterAdVoltage2FailingTime("2.5") |
| SyControllerMaster | CAN-Device | GetMasterAdVoltage3FailingTime | void | 方法 | string threshold | 获取主站电压3下降时间 | GetMasterAdVoltage3FailingTime("2.5") |
| SyControllerMaster | CAN-Device | GetMasterAdVoltage4FailingTime | void | 方法 | string threshold | 获取主站电压4下降时间 | GetMasterAdVoltage4FailingTime("2.5") |
| SyControllerMaster | CAN-Device | GetSlaveAdCurrent1RisingTime | void | 方法 | string threshold | 获取从站电流1上升时间 | GetSlaveAdCurrent1RisingTime("2.5") |
| SyControllerMaster | CAN-Device | GetSlaveAdCurrent2RisingTime | void | 方法 | string threshold | 获取从站电流2上升时间 | GetSlaveAdCurrent2RisingTime("2.5") |
| SyControllerMaster | CAN-Device | GetSlaveAdCurrent3RisingTime | void | 方法 | string threshold | 获取从站电流3上升时间 | GetSlaveAdCurrent3RisingTime("2.5") |
| SyControllerMaster | CAN-Device | GetSlaveAdCurrent4RisingTime | void | 方法 | string threshold | 获取从站电流4上升时间 | GetSlaveAdCurrent4RisingTime("2.5") |
| SyControllerMaster | CAN-Device | GetSlaveAdCurrent5RisingTime | void | 方法 | string threshold | 获取从站电流5上升时间 | GetSlaveAdCurrent5RisingTime("2.5") |
| SyControllerMaster | CAN-Device | GetSlaveAdCurrent6RisingTime | void | 方法 | string threshold | 获取从站电流6上升时间 | GetSlaveAdCurrent6RisingTime("2.5") |
| SyControllerMaster | CAN-Device | GetSlaveAdVoltage1RisingTime | void | 方法 | string threshold | 获取从站电压1上升时间 | GetSlaveAdVoltage1RisingTime("2.5") |
| SyControllerMaster | CAN-Device | GetSlaveAdVoltage2RisingTime | void | 方法 | string threshold | 获取从站电压2上升时间 | GetSlaveAdVoltage2RisingTime("2.5") |
| SyControllerMaster | CAN-Device | GetSlaveAdVoltage3RisingTime | void | 方法 | string threshold | 获取从站电压3上升时间 | GetSlaveAdVoltage3RisingTime("2.5") |
| SyControllerMaster | CAN-Device | GetSlaveAdVoltage4RisingTime | void | 方法 | string threshold | 获取从站电压4上升时间 | GetSlaveAdVoltage4RisingTime("2.5") |
| SyControllerMaster | CAN-Device | GetSlaveAdVoltage5RisingTime | void | 方法 | string threshold | 获取从站电压5上升时间 | GetSlaveAdVoltage5RisingTime("2.5") |
| SyControllerMaster | CAN-Device | GetSlaveAdVoltage6RisingTime | void | 方法 | string threshold | 获取从站电压6上升时间 | GetSlaveAdVoltage6RisingTime("2.5") |
| SyControllerMaster | CAN-Device | GetSlaveAdVoltage7RisingTime | void | 方法 | string threshold | 获取从站电压7上升时间 | GetSlaveAdVoltage7RisingTime("2.5") |
| SyControllerMaster | CAN-Device | GetSlaveAdVoltage8RisingTime | void | 方法 | string threshold | 获取从站电压8上升时间 | GetSlaveAdVoltage8RisingTime("2.5") |
| SyControllerMaster | CAN-Device | GetSlaveAdCurrent1FailingTime | void | 方法 | string threshold | 获取从站电流1下降时间 | GetSlaveAdCurrent1FailingTime("2.5") |
| SyControllerMaster | CAN-Device | GetSlaveAdCurrent2FailingTime | void | 方法 | string threshold | 获取从站电流2下降时间 | GetSlaveAdCurrent2FailingTime("2.5") |
| SyControllerMaster | CAN-Device | GetSlaveAdCurrent3FailingTime | void | 方法 | string threshold | 获取从站电流3下降时间 | GetSlaveAdCurrent3FailingTime("2.5") |
| SyControllerMaster | CAN-Device | GetSlaveAdCurrent4FailingTime | void | 方法 | string threshold | 获取从站电流4下降时间 | GetSlaveAdCurrent4FailingTime("2.5") |
| SyControllerMaster | CAN-Device | GetSlaveAdCurrent5FailingTime | void | 方法 | string threshold | 获取从站电流5下降时间 | GetSlaveAdCurrent5FailingTime("2.5") |
| SyControllerMaster | CAN-Device | GetSlaveAdCurrent6FailingTime | void | 方法 | string threshold | 获取从站电流6下降时间 | GetSlaveAdCurrent6FailingTime("2.5") |
| SyControllerMaster | CAN-Device | GetSlaveAdVoltage1FailingTime | void | 方法 | string threshold | 获取从站电压1下降时间 | GetSlaveAdVoltage1FailingTime("2.5") |
| SyControllerMaster | CAN-Device | GetSlaveAdVoltage2FailingTime | void | 方法 | string threshold | 获取从站电压2下降时间 | GetSlaveAdVoltage2FailingTime("2.5") |
| SyControllerMaster | CAN-Device | GetSlaveAdVoltage3FailingTime | void | 方法 | string threshold | 获取从站电压3下降时间 | GetSlaveAdVoltage3FailingTime("2.5") |
| SyControllerMaster | CAN-Device | GetSlaveAdVoltage4FailingTime | void | 方法 | string threshold | 获取从站电压4下降时间 | GetSlaveAdVoltage4FailingTime("2.5") |
| SyControllerMaster | CAN-Device | GetSlaveAdVoltage5FailingTime | void | 方法 | string threshold | 获取从站电压5下降时间 | GetSlaveAdVoltage5FailingTime("2.5") |
| SyControllerMaster | CAN-Device | GetSlaveAdVoltage6FailingTime | void | 方法 | string threshold | 获取从站电压6下降时间 | GetSlaveAdVoltage6FailingTime("2.5") |
| SyControllerMaster | CAN-Device | GetSlaveAdVoltage7FailingTime | void | 方法 | string threshold | 获取从站电压7下降时间 | GetSlaveAdVoltage7FailingTime("2.5") |
| SyControllerMaster | CAN-Device | GetSlaveAdVoltage8FailingTime | void | 方法 | string threshold | 获取从站电压8下降时间 | GetSlaveAdVoltage8FailingTime("2.5") |
