# SyRenesasMcuControllerMaster 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`, `ICcdIoController`
**控制器描述**: CAN-Device

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| SyRenesasMcuControllerMaster | CAN-Device | GatewayCan1 | CanBus | 字段 | - | 网关CAN1 | - |
| SyRenesasMcuControllerMaster | CAN-Device | GatewayCan2 | CanBus | 字段 | - | 网关CAN2 | - |
| SyRenesasMcuControllerMaster | CAN-Device | GatewayCan3 | CanBus | 字段 | - | 网关CAN3 | - |
| SyRenesasMcuControllerMaster | CAN-Device | GatewayCan4 | CanBus | 字段 | - | 网关CAN4 | - |
| SyRenesasMcuControllerMaster | CAN-Device | GatewayLin1 | LinBus | 字段 | - | 网关LIN1 | - |
| SyRenesasMcuControllerMaster | CAN-Device | GatewayLin2 | LinBus | 字段 | - | 网关LIN2 | - |
| SyRenesasMcuControllerMaster | CAN-Device | UartCan | MySerialPort | 字段 | - | UART接口 | - |
| SyRenesasMcuControllerMaster | CAN-Device | Di1 | string | 字段 | - | R,输入1 | - |
| SyRenesasMcuControllerMaster | CAN-Device | Di2 | string | 字段 | - | R,输入2 | - |
| SyRenesasMcuControllerMaster | CAN-Device | Di3 | string | 字段 | - | R,输入3 | - |
| SyRenesasMcuControllerMaster | CAN-Device | Di4 | string | 字段 | - | R,输入4 | - |
| SyRenesasMcuControllerMaster | CAN-Device | Di5 | string | 字段 | - | R,输入5 | - |
| SyRenesasMcuControllerMaster | CAN-Device | Di6 | string | 字段 | - | R,输入6 | - |
| SyRenesasMcuControllerMaster | CAN-Device | Do1 | bool | 字段 | - | R/W,输出1 | - |
| SyRenesasMcuControllerMaster | CAN-Device | Do2 | bool | 字段 | - | R/W,输出2 | - |
| SyRenesasMcuControllerMaster | CAN-Device | Do3 | bool | 字段 | - | R/W,输出3 | - |
| SyRenesasMcuControllerMaster | CAN-Device | Do4 | bool | 字段 | - | R/W,输出4 | - |
| SyRenesasMcuControllerMaster | CAN-Device | Freq1 | double | 字段 | - | R,频率1 | - |
| SyRenesasMcuControllerMaster | CAN-Device | Duty1 | double | 字段 | - | R,占空比1 | - |
| SyRenesasMcuControllerMaster | CAN-Device | Freq2 | double | 字段 | - | R,频率2 | - |
| SyRenesasMcuControllerMaster | CAN-Device | Duty2 | double | 字段 | - | R,占空比2 | - |
| SyRenesasMcuControllerMaster | CAN-Device | Freq3 | double | 字段 | - | R,频率3 | - |
| SyRenesasMcuControllerMaster | CAN-Device | Duty3 | double | 字段 | - | R,占空比3 | - |
| SyRenesasMcuControllerMaster | CAN-Device | Freq4 | double | 字段 | - | R,频率4 | - |
| SyRenesasMcuControllerMaster | CAN-Device | Duty4 | double | 字段 | - | R,占空比4 | - |
| SyRenesasMcuControllerMaster | CAN-Device | Freq5 | double | 字段 | - | R,频率5 | - |
| SyRenesasMcuControllerMaster | CAN-Device | Duty5 | double | 字段 | - | R,占空比5 | - |
| SyRenesasMcuControllerMaster | CAN-Device | Freq6 | double | 字段 | - | R,频率6 | - |
| SyRenesasMcuControllerMaster | CAN-Device | Duty6 | double | 字段 | - | R,占空比6 | - |
| SyRenesasMcuControllerMaster | CAN-Device | HSDCurrent1 | float | 字段 | - | R,HSD输出电流1 | - |
| SyRenesasMcuControllerMaster | CAN-Device | HSDCurrent2 | float | 字段 | - | R,HSD输出电流2 | - |
| SyRenesasMcuControllerMaster | CAN-Device | HSDCurrent3 | float | 字段 | - | R,HSD输出电流3 | - |
| SyRenesasMcuControllerMaster | CAN-Device | HSDCurrent4 | float | 字段 | - | R,HSD输出电流4 | - |
| SyRenesasMcuControllerMaster | CAN-Device | IsConnected | bool | 字段 | - | 连接状态 | - |
| SyRenesasMcuControllerMaster | CAN-Device | RemoteIpPort | string | 字段 | - | 远程IP和端口 | - |
| SyRenesasMcuControllerMaster | CAN-Device | InitRemoteIpAddress | void | 方法 | string ipPort | 初始化远程IP地址 | InitRemoteIpAddress("192.168.1.100:5000") |
| SyRenesasMcuControllerMaster | CAN-Device | SetOutputs | bool | 方法 | - | 设置输出 | SetOutputs() |
| SyRenesasMcuControllerMaster | CAN-Device | GetInputs | void | 方法 | - | 获取输入 | GetInputs() |
| SyRenesasMcuControllerMaster | CAN-Device | ResetOutPuts | void | 方法 | - | 重置输出 | ResetOutPuts() |
| SyRenesasMcuControllerMaster | CAN-Device | SetSlavesRLs | void | 方法 | - | 操作从站继电器 | SetSlavesRLs() |
| SyRenesasMcuControllerMaster | CAN-Device | ReadSlavesADs | void | 方法 | - | 读取从站AD值 | ReadSlavesADs() |
| SyRenesasMcuControllerMaster | CAN-Device | ReadSlavesSensorValues | void | 方法 | - | 读取从站光敏传感器采样值 | ReadSlavesSensorValues() |
| SyRenesasMcuControllerMaster | CAN-Device | MasterReadDIs | void | 方法 | - | 主站读取DI | MasterReadDIs() |
| SyRenesasMcuControllerMaster | CAN-Device | MasterReadDIsAndDuty | void | 方法 | - | 主站读取DI和Duty | MasterReadDIsAndDuty() |
| SyRenesasMcuControllerMaster | CAN-Device | MasterReadHSDCurr | void | 方法 | - | 主站读取电流 | MasterReadHSDCurr() |
| SyRenesasMcuControllerMaster | CAN-Device | MasetSetDOs | void | 方法 | - | 主站设置DO | MasetSetDOs() |
| SyRenesasMcuControllerMaster | CAN-Device | SlaveTryConnectMaster | bool | 方法 | string masterIp, string slaveId, object slaveInstance | 从站连接主站 | SlaveTryConnectMaster("192.168.1.100", "0x01", slave) |
| SyRenesasMcuControllerMaster | CAN-Device | SlaveSetRLs | void | 方法 | string masterIp, uint slaveId | RL从站单独控制继电器 | SlaveSetRLs("192.168.1.100", 0x01) |
| SyRenesasMcuControllerMaster | CAN-Device | SlaveReadADs | void | 方法 | string masterIp, uint slaveId | AD从站单独刷新AD值 | SlaveReadADs("192.168.1.100", 0x01) |
| SyRenesasMcuControllerMaster | CAN-Device | SlaveReadSensorValues | void | 方法 | string masterIp, uint slaveId | 从站单独刷新光敏电阻采样值 | SlaveReadSensorValues("192.168.1.100", 0x01) |
| SyRenesasMcuControllerMaster | CAN-Device | SetSlaveId | void | 方法 | string slaveId | 设置从站ID | SetSlaveId("0x01") |
| SyRenesasMcuControllerMaster | CAN-Device | SetSlaveCanFunc | void | 方法 | string type, uint bauteRate | 设置从站CAN功能 | SetSlaveCanFunc("0", 500000) |
| SyRenesasMcuControllerMaster | CAN-Device | SetLinBauteRate | void | 方法 | int type, uint bauteRate | 设置LIN波特率 | SetLinBauteRate(0, 19200) |
