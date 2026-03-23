# PowerIt6302 控制器接口文档

## 控制器信息

| 控制器名称 | 控制器描述 |
|-----------|-----------|
| PowerIt6302 | 多通道电源控制器 |

## 公共字段

| 名称 | 类型 | 类别 | 描述 |
|------|------|------|------|
| MySerialPort | MySerialPort | 总线 | 串口 |
| VoltageRead1 | float | 读 | R,通道1输出电压值 |
| VoltageRead2 | float | 读 | R,通道2输出电压值 |
| VoltageRead3 | float | 读 | R,通道3输出电压值 |
| CurreatRead1 | float | 读 | R,通道1输出电流值 |
| CurreatRead2 | float | 读 | R,通道1输出电流值 |
| CurreatRead3 | float | 读 | R,通道1输出电流值 |

## 公共方法

| 名称 | 参数 | 类型 | 说明 | 示例 |
|------|------|------|------|------|
| ConnectPower | protocolValue: string | 方法 | 连接电源 | ConnectPower("COM1:9600") |
| PowerOn | 无 | 方法 | 打开所有通道输出 |  |
| PowerOff | 无 | 方法 | 关闭所有通道输出 |  |
| OpenAllChannels | 无 | 方法 | 打开所有通道输出 |  |
| CloseAllChannels | 无 | 方法 | 关闭所有通道输出 |  |
| SetCombSerOn | 无 | 方法 | 将电源的通道 1 和通道 2 设置为串联状态 |  |
| SetCombParaOn | 无 | 方法 | 将电源的通道 1 和通道 2 设置为并联状态 |  |
| SetCombOff | 无 | 方法 | 解除电源的通道 1 和通道 2 的串并联状态 |  |
| OpenSingleChannel | channelIndex: string | 方法 | 打开单个通道 | OpenSingleChannel("1") |
| CloseSingleChannel | channelIndex: string | 方法 | 关闭单个通道 | CloseSingleChannel("1") |
| SetVoltageAll | voltage: float | 方法 | 设置通道1-3的电压值 | SetVoltageAll(12.0) |
| SetVoltage1 | voltage: float | 方法 | 设置通道1的电压值 | SetVoltage1(12.0) |
| SetVoltage2 | voltage: float | 方法 | 设置通道2的电压值 | SetVoltage2(12.0) |
| SetVoltage3 | voltage: float | 方法 | 设置通道3的电压值 | SetVoltage3(12.0) |
| SetCurrentAll | current: float | 方法 | 设置通道1-3的电流值 | SetCurrentAll(1.0) |
| SetCurrent1 | current: float | 方法 | 设置通道1的电流值 | SetCurrent1(1.0) |
| SetCurrent2 | current: float | 方法 | 设置通道2的电流值 | SetCurrent2(1.0) |
| SetCurrent3 | current: float | 方法 | 设置通道3的电流值 | SetCurrent3(1.0) |
| UpdateCurrentsAndVoltages | 无 | 方法 | 更新通道1-3当前的输出电压值和电流值 |  |
| UpdateCh1CurrentAndVoltage | 无 | 方法 | 仅更新通道1当前的输出电压值和电流值 |  |
