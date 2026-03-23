# SyRenesasMcuControllerSlaveWith8RLs 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`, `ICcdIoController`
**控制器描述**: 继电器从站 0x201 - 0x20F

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| SyRenesasMcuControllerSlaveWith8RLs | 继电器从站 0x201 - 0x20F | Relay1 | bool | 字段 | - | R/W,继电器1 | - |
| SyRenesasMcuControllerSlaveWith8RLs | 继电器从站 0x201 - 0x20F | Relay2 | bool | 字段 | - | R/W,继电器2 | - |
| SyRenesasMcuControllerSlaveWith8RLs | 继电器从站 0x201 - 0x20F | Relay3 | bool | 字段 | - | R/W,继电器3 | - |
| SyRenesasMcuControllerSlaveWith8RLs | 继电器从站 0x201 - 0x20F | Relay4 | bool | 字段 | - | R/W,继电器4 | - |
| SyRenesasMcuControllerSlaveWith8RLs | 继电器从站 0x201 - 0x20F | Relay5 | bool | 字段 | - | R/W,继电器5 | - |
| SyRenesasMcuControllerSlaveWith8RLs | 继电器从站 0x201 - 0x20F | Relay6 | bool | 字段 | - | R/W,继电器6 | - |
| SyRenesasMcuControllerSlaveWith8RLs | 继电器从站 0x201 - 0x20F | Relay7 | bool | 字段 | - | R/W,继电器7 | - |
| SyRenesasMcuControllerSlaveWith8RLs | 继电器从站 0x201 - 0x20F | Relay8 | bool | 字段 | - | R/W,继电器8 | - |
| SyRenesasMcuControllerSlaveWith8RLs | 继电器从站 0x201 - 0x20F | ConnectMaster | bool | 方法 | string masterIp, string slaveId | 连接主站 | - |
| SyRenesasMcuControllerSlaveWith8RLs | 继电器从站 0x201 - 0x20F | SlaveSetRLs | void | 方法 | - | 设置从站继电器状态 | - |
| SyRenesasMcuControllerSlaveWith8RLs | 继电器从站 0x201 - 0x20F | SlaveRefreshRLs | void | 方法 | - | 刷新从站继电器状态 | - |
| SyRenesasMcuControllerSlaveWith8RLs | 继电器从站 0x201 - 0x20F | SetOutputs | bool | 方法 | - | 设置输出 | - |
| SyRenesasMcuControllerSlaveWith8RLs | 继电器从站 0x201 - 0x20F | GetInputs | void | 方法 | - | 获取输入 | - |
| SyRenesasMcuControllerSlaveWith8RLs | 继电器从站 0x201 - 0x20F | ResetOutPuts | void | 方法 | - | 重置输出 | - |
