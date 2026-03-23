# SyControllerSlaveWith20Do 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`, `ControllerMasterSocketByUdp.IControllerMasterSocketSubscriber`
**控制器描述**: SyControllerSlaveWith20Do

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| SyControllerSlaveWith20Do | - | CanId | uint | 字段 | - | CAN ID，默认为0x401 | CanId = 0x401 |
| SyControllerSlaveWith20Do | - | Do1 | bool | 字段 | - | 数字输出1 | Do1 = true |
| SyControllerSlaveWith20Do | - | Do2 | bool | 字段 | - | 数字输出2 | Do2 = false |
| SyControllerSlaveWith20Do | - | Do3 | bool | 字段 | - | 数字输出3 | Do3 = true |
| SyControllerSlaveWith20Do | - | Do4 | bool | 字段 | - | 数字输出4 | Do4 = false |
| SyControllerSlaveWith20Do | - | Do5 | bool | 字段 | - | 数字输出5 | Do5 = true |
| SyControllerSlaveWith20Do | - | Do6 | bool | 字段 | - | 数字输出6 | Do6 = false |
| SyControllerSlaveWith20Do | - | Do7 | bool | 字段 | - | 数字输出7 | Do7 = true |
| SyControllerSlaveWith20Do | - | Do8 | bool | 字段 | - | 数字输出8 | Do8 = false |
| SyControllerSlaveWith20Do | - | Do9 | bool | 字段 | - | 数字输出9 | Do9 = true |
| SyControllerSlaveWith20Do | - | Do10 | bool | 字段 | - | 数字输出10 | Do10 = false |
| SyControllerSlaveWith20Do | - | Do11 | bool | 字段 | - | 数字输出11 | Do11 = true |
| SyControllerSlaveWith20Do | - | Do12 | bool | 字段 | - | 数字输出12 | Do12 = false |
| SyControllerSlaveWith20Do | - | Do13 | bool | 字段 | - | 数字输出13 | Do13 = true |
| SyControllerSlaveWith20Do | - | Do14 | bool | 字段 | - | 数字输出14 | Do14 = false |
| SyControllerSlaveWith20Do | - | Do15 | bool | 字段 | - | 数字输出15 | Do15 = true |
| SyControllerSlaveWith20Do | - | Do16 | bool | 字段 | - | 数字输出16 | Do16 = false |
| SyControllerSlaveWith20Do | - | Do17 | bool | 字段 | - | 数字输出17 | Do17 = true |
| SyControllerSlaveWith20Do | - | Do18 | bool | 字段 | - | 数字输出18 | Do18 = false |
| SyControllerSlaveWith20Do | - | Do19 | bool | 字段 | - | 数字输出19 | Do19 = true |
| SyControllerSlaveWith20Do | - | Do20 | bool | 字段 | - | 数字输出20 | Do20 = false |
| SyControllerSlaveWith20Do | - | ConnectToMaster | void | 方法 | string masterName | 连接主站 | ConnectToMaster("Master1") |
| SyControllerSlaveWith20Do | - | ChangeCanId | void | 方法 | string canId | 修改CAN ID | ChangeCanId("0x401") |
| SyControllerSlaveWith20Do | - | ReceiveMsg | void | 方法 | EndPoint ipEndPointFrom, object dataPackage | 接收CAN消息 | - |
| SyControllerSlaveWith20Do | - | SetSlaveDos | bool | 方法 | - | 设置从站DO输出 | SetSlaveDos() |
| SyControllerSlaveWith20Do | - | GetSlaveDos | bool | 方法 | - | 获取从站DO状态 | GetSlaveDos() |
