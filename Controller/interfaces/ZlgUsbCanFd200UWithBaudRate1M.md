# ZlgUsbCanFd200UWithBaudRate1M 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: CAN-Device

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| ZlgUsbCanFd200UWithBaudRate1M | ZLG CAN通道0 | ZlgCanChannel0 | CanBus | 字段 | - | ZLG CAN通道0 | - |
| ZlgUsbCanFd200UWithBaudRate1M | ZLG CAN通道1 | ZlgCanChannel1 | CanBus | 字段 | - | ZLG CAN通道1 | - |
| ZlgUsbCanFd200UWithBaudRate1M | 判断是否为扩展帧 | IsEff | 方法 | 方法 | uint id | 判断是否为扩展帧 | - |
| ZlgUsbCanFd200UWithBaudRate1M | 判断是否为远程帧 | IsRtr | 方法 | 方法 | uint id | 判断是否为远程帧 | - |
| ZlgUsbCanFd200UWithBaudRate1M | 判断是否为错误帧 | IsErr | 方法 | 方法 | uint id | 判断是否为错误帧 | - |
| ZlgUsbCanFd200UWithBaudRate1M | 获取ID | GetId | 方法 | 方法 | uint id | 获取CAN ID | - |
