# ToomossUsb2XxxSlaveLin 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: ToomossUsb2XxxSlaveLin

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| ToomossUsb2XxxSlaveLin | ToomossUsb2XxxSlaveLin | SlaveLin1 | ToomossUsbSlaveLinChannel | 字段 | - | LIN1从机通道 | - |
| ToomossUsb2XxxSlaveLin | ToomossUsb2XxxSlaveLin | SlaveLin2 | ToomossUsbSlaveLinChannel | 字段 | - | LIN2从机通道 | - |
| ToomossUsb2XxxSlaveLin | ToomossUsb2XxxSlaveLin | State | bool | 字段 | - | 设备状态 | - |
| ToomossUsb2XxxSlaveLin | ToomossUsb2XxxSlaveLin | SetBaudRate | void | 方法 | string linIndex, string baudRate | 设置波特率 | - |
| ToomossUsb2XxxSlaveLin | ToomossUsb2XxxSlaveLin | ConfigLinMsgCount | void | 方法 | int linIndex, int count | 配置LIN消息数量 | - |
| ToomossUsb2XxxSlaveLin | ToomossUsb2XxxSlaveLin | UpdateLinMsg | void | 方法 | int linIndex, string linId, string value | 更新LIN消息 | - |

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| ToomossUsbSlaveLinChannel | ToomossUsb从机LIN通道 | ConfigLinMsgCount | void | 方法 | int count | 配置LIN消息数量 | - |
| ToomossUsbSlaveLinChannel | ToomossUsb从机LIN通道 | UpdateLinExMsg | void | 方法 | string linId, string value | 更新LIN消息 | - |
