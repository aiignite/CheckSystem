# SyRenesasMcuControllerSlaveWith12ADs 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`, `ICcdIoController`
**控制器描述**: AD从站 0x101 - 0x10F

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| SyRenesasMcuControllerSlaveWith12ADs | AD从站 0x101 - 0x10F | Current1 | double | 字段 | - | R,电流1 | - |
| SyRenesasMcuControllerSlaveWith12ADs | AD从站 0x101 - 0x10F | Current2 | double | 字段 | - | R,电流2 | - |
| SyRenesasMcuControllerSlaveWith12ADs | AD从站 0x101 - 0x10F | Current3 | double | 字段 | - | R,电流3 | - |
| SyRenesasMcuControllerSlaveWith12ADs | AD从站 0x101 - 0x10F | Current4 | double | 字段 | - | R,电流4 | - |
| SyRenesasMcuControllerSlaveWith12ADs | AD从站 0x101 - 0x10F | Current5 | double | 字段 | - | R,电流5 | - |
| SyRenesasMcuControllerSlaveWith12ADs | AD从站 0x101 - 0x10F | Current6 | double | 字段 | - | R,电流6 | - |
| SyRenesasMcuControllerSlaveWith12ADs | AD从站 0x101 - 0x10F | Resistance1 | double | 字段 | - | R,分压测电阻1 | - |
| SyRenesasMcuControllerSlaveWith12ADs | AD从站 0x101 - 0x10F | Voltage2 | double | 字段 | - | R,电压2 | - |
| SyRenesasMcuControllerSlaveWith12ADs | AD从站 0x101 - 0x10F | Voltage3 | double | 字段 | - | R,电压3 | - |
| SyRenesasMcuControllerSlaveWith12ADs | AD从站 0x101 - 0x10F | Voltage4 | double | 字段 | - | R,电压4 | - |
| SyRenesasMcuControllerSlaveWith12ADs | AD从站 0x101 - 0x10F | Voltage5 | double | 字段 | - | R,电压5 | - |
| SyRenesasMcuControllerSlaveWith12ADs | AD从站 0x101 - 0x10F | Voltage6 | double | 字段 | - | R,电压6 | - |
| SyRenesasMcuControllerSlaveWith12ADs | AD从站 0x101 - 0x10F | Voltage7 | double | 字段 | - | R,电压7 | - |
| SyRenesasMcuControllerSlaveWith12ADs | AD从站 0x101 - 0x10F | Currents1 | List<double> | 字段 | - | 循环刷新从站AD值 | - |
| SyRenesasMcuControllerSlaveWith12ADs | AD从站 0x101 - 0x10F | Currents2 | List<double> | 字段 | - | 循环刷新从站AD值 | - |
| SyRenesasMcuControllerSlaveWith12ADs | AD从站 0x101 - 0x10F | Currents3 | List<double> | 字段 | - | 循环刷新从站AD值 | - |
| SyRenesasMcuControllerSlaveWith12ADs | AD从站 0x101 - 0x10F | Currents4 | List<double> | 字段 | - | 循环刷新从站AD值 | - |
| SyRenesasMcuControllerSlaveWith12ADs | AD从站 0x101 - 0x10F | Currents5 | List<double> | 字段 | - | 循环刷新从站AD值 | - |
| SyRenesasMcuControllerSlaveWith12ADs | AD从站 0x101 - 0x10F | Currents6 | List<double> | 字段 | - | 循环刷新从站AD值 | - |
| SyRenesasMcuControllerSlaveWith12ADs | AD从站 0x101 - 0x10F | Voltages2 | List<double> | 字段 | - | 循环刷新从站AD值 | - |
| SyRenesasMcuControllerSlaveWith12ADs | AD从站 0x101 - 0x10F | Voltages3 | List<double> | 字段 | - | 循环刷新从站AD值 | - |
| SyRenesasMcuControllerSlaveWith12ADs | AD从站 0x101 - 0x10F | Voltages4 | List<double> | 字段 | - | 循环刷新从站AD值 | - |
| SyRenesasMcuControllerSlaveWith12ADs | AD从站 0x101 - 0x10F | Voltages5 | List<double> | 字段 | - | 循环刷新从站AD值 | - |
| SyRenesasMcuControllerSlaveWith12ADs | AD从站 0x101 - 0x10F | Voltages6 | List<double> | 字段 | - | 循环刷新从站AD值 | - |
| SyRenesasMcuControllerSlaveWith12ADs | AD从站 0x101 - 0x10F | Voltages7 | List<double> | 字段 | - | 循环刷新从站AD值 | - |
| SyRenesasMcuControllerSlaveWith12ADs | AD从站 0x101 - 0x10F | ConnectMaster | bool | 方法 | string masterIp, string slaveId | 连接主站 | - |
| SyRenesasMcuControllerSlaveWith12ADs | AD从站 0x101 - 0x10F | SlaveReadADs | void | 方法 | - | 刷新从站AD值 | - |
| SyRenesasMcuControllerSlaveWith12ADs | AD从站 0x101 - 0x10F | SetOutputs | bool | 方法 | - | 设置输出 | - |
| SyRenesasMcuControllerSlaveWith12ADs | AD从站 0x101 - 0x10F | GetInputs | void | 方法 | - | 获取输入 | - |
| SyRenesasMcuControllerSlaveWith12ADs | AD从站 0x101 - 0x10F | ResetOutPuts | void | 方法 | - | 重置输出 | - |
| SyRenesasMcuControllerSlaveWith12ADs | AD从站 0x101 - 0x10F | ReadCurrentsVoltages | void | 方法 | int n | 循环刷新从站AD值 | - |
| SyRenesasMcuControllerSlaveWith12ADs | AD从站 0x101 - 0x10F | RefreshCurrent | void | 方法 | int idx, int min, int max | 读取CurrentList中满足范围值 | - |
| SyRenesasMcuControllerSlaveWith12ADs | AD从站 0x101 - 0x10F | RefreshVoltage | void | 方法 | int idx, int min, int max | 读取VoltageList中满足范围值 | - |
