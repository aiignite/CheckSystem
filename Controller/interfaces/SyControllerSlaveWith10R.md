# SyControllerSlaveWith10R 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: SyControllerSlaveWith10R

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| SyControllerSlaveWith10R | SyControllerSlaveWith10R | CanId | uint | 字段 | - | R,CANID，默认0x201 | - |
| SyControllerSlaveWith10R | SyControllerSlaveWith10R | Relay1 | bool | 字段 | - | R/W,继电器1 | - |
| SyControllerSlaveWith10R | SyControllerSlaveWith10R | Relay2 | bool | 字段 | - | R/W,继电器2 | - |
| SyControllerSlaveWith10R | SyControllerSlaveWith10R | Relay3 | bool | 字段 | - | R/W,继电器3 | - |
| SyControllerSlaveWith10R | SyControllerSlaveWith10R | Relay4 | bool | 字段 | - | R/W,继电器4 | - |
| SyControllerSlaveWith10R | SyControllerSlaveWith10R | Relay5 | bool | 字段 | - | R/W,继电器5 | - |
| SyControllerSlaveWith10R | SyControllerSlaveWith10R | Relay6 | bool | 字段 | - | R/W,继电器6 | - |
| SyControllerSlaveWith10R | SyControllerSlaveWith10R | Relay7 | bool | 字段 | - | R/W,继电器7 | - |
| SyControllerSlaveWith10R | SyControllerSlaveWith10R | Relay8 | bool | 字段 | - | R/W,继电器8 | - |
| SyControllerSlaveWith10R | SyControllerSlaveWith10R | Relay9 | bool | 字段 | - | R/W,继电器9 | - |
| SyControllerSlaveWith10R | SyControllerSlaveWith10R | Relay10 | bool | 字段 | - | R/W,继电器10 | - |
| SyControllerSlaveWith10R | SyControllerSlaveWith10R | SetOutputs | bool | 方法 | 无 | 设置输出 | SetOutputs() |
| SyControllerSlaveWith10R | SyControllerSlaveWith10R | GetInputs | void | 方法 | 无 | 获取输入 | GetInputs() |
| SyControllerSlaveWith10R | SyControllerSlaveWith10R | ResetOutPuts | void | 方法 | 无 | 重置输出 | ResetOutPuts() |
| SyControllerSlaveWith10R | SyControllerSlaveWith10R | ConnectToMaster | void | 方法 | string masterName | 连接到主站 | ConnectToMaster("Master1") |
| SyControllerSlaveWith10R | SyControllerSlaveWith10R | ChangeCanId | void | 方法 | string canId | 更改CANID | ChangeCanId("0x201") |
| SyControllerSlaveWith10R | SyControllerSlaveWith10R | ReceiveMsg | void | 方法 | EndPoint ipEndPointFrom, object dataPackage | 接收消息 | ReceiveMsg(endpoint, data) |
| SyControllerSlaveWith10R | SyControllerSlaveWith10R | GetSlaveDis | bool | 方法 | 无 | 获取从站断开状态 | GetSlaveDis() |
| SyControllerSlaveWith10R | SyControllerSlaveWith10R | SetSlaveRelays | void | 方法 | 无 | 操作所有继电器 | SetSlaveRelays() |
| SyControllerSlaveWith10R | SyControllerSlaveWith10R | GetSlaveRelays | bool | 方法 | 无 | 读取继电器状态 | GetSlaveRelays() |
| SyControllerSlaveWith10R | SyControllerSlaveWith10R | AllRelaysOff | void | 方法 | 无 | 关闭所有继电器 | AllRelaysOff() |
