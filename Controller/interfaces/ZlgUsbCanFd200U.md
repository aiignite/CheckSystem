# ZlgUsbCanFd200U 控制器接口文档

## 控制器基本信息

| 控制器名称 | 控制器描述 |
|------------|------------|
| ZlgUsbCanFd200U | CAN-Device |

## 公共属性

| 名称 | 类型 | 类别 | 说明 | 示例 |
|------|------|------|------|------|
| CustomMaxLen | int | 属性 | 自定义最大长度 | - |
| ZlgCanChannel0 | CanBus | 属性 | CAN通道0 | - |
| ZlgCanChannel1 | CanBus | 属性 | CAN通道1 | - |

## 公共方法

| 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|------|------|------|------|------|------|
| IsEff | bool | 方法 | id: uint | 1:extend frame 0:standard frame | IsEff(0x100) |
| IsRtr | bool | 方法 | id: uint | 1:remote frame 0:data frame | IsRtr(0x100) |
| IsErr | bool | 方法 | id: uint | 1:error frame 0:normal frame | IsErr(0x100) |