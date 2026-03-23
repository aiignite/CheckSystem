# OsramEviyosPowerNgi3412E 控制器接口文档

## 控制器信息

| 控制器名称 | 控制器描述 |
|-----------|-----------|
| OsramEviyosPowerNgi3412E |  |

## 公共字段

| 名称 | 类型 | 类别 | 描述 |
|------|------|------|------|
| CurrentV | float | 读 | R,电压读取 |
| CurrentC | float | 读 | R,电流读取 |
| CurrentC2 | float | 读 | R,电流读取 |
| readCh1Curr | float | 读 | 通道1电流读取 |
| readCh2Curr | float | 读 | 通道2电流读取 |

## 公共方法

| 名称 | 参数 | 类型 | 说明 | 示例 |
|------|------|------|------|------|
| ReadOutputC3 | cl: byte | 方法 | 读取选定通道的输出电流 | ReadOutputC3(0) |
| ReadOutputC | 无 | 方法 | 读取选定通道的输出电流 |  |
| ReadOutputC2 | 无 | 方法 | 读取选定通道的输出电流 |  |
| InitPower | ip: string | 方法 | 初始化电源 | InitPower("192.168.1.1") |
| PowerOn | cl: byte | 方法 | 打开电源 | PowerOn(0) |
| PowerOff | cl: byte | 方法 | 关闭电源 | PowerOff(0) |
| SelectChannel | cl: byte | 方法 | 选择通道 | SelectChannel(0) |
| ChannelOn | 无 | 方法 | 打开通道 |  |
| ChannelOff | 无 | 方法 | 关闭通道 |  |
| SetV | v: float | 方法 | 设置电压 | SetV(12.5) |
| SetC | c: float | 方法 | 设置电流 | SetC(0.5) |
| ConnectPower | protocolValue: string | 方法 | 连接电源 | ConnectPower("192.168.1.1:7000") |
