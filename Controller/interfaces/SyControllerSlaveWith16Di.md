# SyControllerSlaveWith16Di 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: SyControllerSlaveWith16Di

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| SyControllerSlaveWith16Di | SyControllerSlaveWith16Di | CanId | uint | 字段 | - | R,CANID，默认0x301 | - |
| SyControllerSlaveWith16Di | SyControllerSlaveWith16Di | Di1 | bool | 字段 | - | R,DI1 | - |
| SyControllerSlaveWith16Di | SyControllerSlaveWith16Di | Di2 | bool | 字段 | - | R,DI2 | - |
| SyControllerSlaveWith16Di | SyControllerSlaveWith16Di | Di3 | bool | 字段 | - | R,DI3 | - |
| SyControllerSlaveWith16Di | SyControllerSlaveWith16Di | Di4 | bool | 字段 | - | R,DI4 | - |
| SyControllerSlaveWith16Di | SyControllerSlaveWith16Di | Di5 | bool | 字段 | - | R,DI5 | - |
| SyControllerSlaveWith16Di | SyControllerSlaveWith16Di | Di6 | bool | 字段 | - | R,DI6 | - |
| SyControllerSlaveWith16Di | SyControllerSlaveWith16Di | Di7 | bool | 字段 | - | R,DI7 | - |
| SyControllerSlaveWith16Di | SyControllerSlaveWith16Di | Di8 | bool | 字段 | - | R,DI8 | - |
| SyControllerSlaveWith16Di | SyControllerSlaveWith16Di | Di9 | bool | 字段 | - | R,DI9 | - |
| SyControllerSlaveWith16Di | SyControllerSlaveWith16Di | Di10 | bool | 字段 | - | R,DI10 | - |
| SyControllerSlaveWith16Di | SyControllerSlaveWith16Di | Di11 | bool | 字段 | - | R,DI11 | - |
| SyControllerSlaveWith16Di | SyControllerSlaveWith16Di | Di12 | bool | 字段 | - | R,DI12 | - |
| SyControllerSlaveWith16Di | SyControllerSlaveWith16Di | Di13 | bool | 字段 | - | R,DI13 | - |
| SyControllerSlaveWith16Di | SyControllerSlaveWith16Di | Di14 | bool | 字段 | - | R,DI14 | - |
| SyControllerSlaveWith16Di | SyControllerSlaveWith16Di | Di15 | bool | 字段 | - | R,DI15 | - |
| SyControllerSlaveWith16Di | SyControllerSlaveWith16Di | Di16 | bool | 字段 | - | R,DI16 | - |
| SyControllerSlaveWith16Di | SyControllerSlaveWith16Di | ConnectToMaster | void | 方法 | string masterName | 连接到主站 | ConnectToMaster("Master1") |
| SyControllerSlaveWith16Di | SyControllerSlaveWith16Di | ChangeCanId | void | 方法 | string canId | 更改CANID | ChangeCanId("0x301") |
| SyControllerSlaveWith16Di | SyControllerSlaveWith16Di | ReceiveMsg | void | 方法 | EndPoint ipEndPointFrom, object dataPackage | 接收消息 | ReceiveMsg(endpoint, data) |
| SyControllerSlaveWith16Di | SyControllerSlaveWith16Di | GetSlaveDis | bool | 方法 | 无 | 获取从站断开状态 | GetSlaveDis() |
