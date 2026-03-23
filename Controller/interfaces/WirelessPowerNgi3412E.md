# WirelessPowerNgi3412E 控制器接口文档

## 控制器基本信息

| 控制器名称 | 控制器描述 |
|------------|------------|
| WirelessPowerNgi3412E | 无线电源控制器 |

## 公共属性

| 名称 | 类型 | 类别 | 说明 | 示例 |
|------|------|------|------|------|
| currentV | float | 属性 | 当前电压 | - |
| currentC | float | 属性 | 当前电流 | - |

## 公共方法

| 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|------|------|------|------|------|------|
| InitPower | void | 方法 | ip: string | 初始化电源 | InitPower("192.168.1.100") |
| SetCombParaOn | void | 方法 | 无 | 打开并联模式 | SetCombParaOn() |
| SetCombSerOn | void | 方法 | 无 | 打开串联模式 | SetCombSerOn() |
| PowerOn | void | 方法 | cl: byte = 0 | 电源开启 | PowerOn() |
| PowerOff | void | 方法 | cl: byte = 0 | 电源关闭 | PowerOff() |
| SelectChannel | void | 方法 | cl: byte = 0 | 选择通道 | SelectChannel(0) |
| ChannelOn | void | 方法 | cl: byte = 0 | 通道开启 | ChannelOn() |
| ChannelOff | void | 方法 | cl: byte = 0 | 通道关闭 | ChannelOff() |
| SetV | void | 方法 | v: float | 设置电压 | SetV(13.5f) |
| SetC | void | 方法 | c: float | 设置电流 | SetC(1.5f) |