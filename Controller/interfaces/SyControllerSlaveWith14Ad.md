# SyControllerSlaveWith14Ad 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: SyControllerSlaveWith14Ad

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| SyControllerSlaveWith14Ad | SyControllerSlaveWith14Ad | CanId | uint | 字段 | - | R,CANID，默认0x101 | - |
| SyControllerSlaveWith14Ad | SyControllerSlaveWith14Ad | AdCurrent1 | float | 字段 | - | R,电流1 | - |
| SyControllerSlaveWith14Ad | SyControllerSlaveWith14Ad | AdCurrent2 | float | 字段 | - | R,电流2 | - |
| SyControllerSlaveWith14Ad | SyControllerSlaveWith14Ad | AdCurrent3 | float | 字段 | - | R,电流3 | - |
| SyControllerSlaveWith14Ad | SyControllerSlaveWith14Ad | AdCurrent4 | float | 字段 | - | R,电流4 | - |
| SyControllerSlaveWith14Ad | SyControllerSlaveWith14Ad | AdCurrent5 | float | 字段 | - | R,电流5 | - |
| SyControllerSlaveWith14Ad | SyControllerSlaveWith14Ad | AdCurrent6 | float | 字段 | - | R,电流6 | - |
| SyControllerSlaveWith14Ad | SyControllerSlaveWith14Ad | AdVoltage1 | float | 字段 | - | R,电压1 | - |
| SyControllerSlaveWith14Ad | SyControllerSlaveWith14Ad | AdVoltage2 | float | 字段 | - | R,电压2 | - |
| SyControllerSlaveWith14Ad | SyControllerSlaveWith14Ad | AdVoltage3 | float | 字段 | - | R,电压3 | - |
| SyControllerSlaveWith14Ad | SyControllerSlaveWith14Ad | AdVoltage4 | float | 字段 | - | R,电压4 | - |
| SyControllerSlaveWith14Ad | SyControllerSlaveWith14Ad | AdVoltage5 | float | 字段 | - | R,电压5 | - |
| SyControllerSlaveWith14Ad | SyControllerSlaveWith14Ad | AdVoltage6 | float | 字段 | - | R,电压6 | - |
| SyControllerSlaveWith14Ad | SyControllerSlaveWith14Ad | AdVoltage7 | float | 字段 | - | R,电压7 | - |
| SyControllerSlaveWith14Ad | SyControllerSlaveWith14Ad | AdVoltage8 | float | 字段 | - | R,电压8 | - |
| SyControllerSlaveWith14Ad | SyControllerSlaveWith14Ad | SetOutputs | bool | 方法 | 无 | 设置输出 | SetOutputs() |
| SyControllerSlaveWith14Ad | SyControllerSlaveWith14Ad | GetInputs | void | 方法 | 无 | 获取输入 | GetInputs() |
| SyControllerSlaveWith14Ad | SyControllerSlaveWith14Ad | ResetOutPuts | void | 方法 | 无 | 重置输出 | ResetOutPuts() |
| SyControllerSlaveWith14Ad | SyControllerSlaveWith14Ad | ConnectToMaster | void | 方法 | string masterName | 连接到主站 | ConnectToMaster("Master1") |
| SyControllerSlaveWith14Ad | SyControllerSlaveWith14Ad | ChangeCanId | void | 方法 | string canId | 更改CANID | ChangeCanId("0x101") |
| SyControllerSlaveWith14Ad | SyControllerSlaveWith14Ad | ReceiveMsg | void | 方法 | EndPoint ipEndPointFrom, object dataPackage | 接收消息 | ReceiveMsg(endpoint, data) |
| SyControllerSlaveWith14Ad | SyControllerSlaveWith14Ad | GetSlaveAds | bool | 方法 | 无 | 获取所有通道的电流电压值 | GetSlaveAds() |
