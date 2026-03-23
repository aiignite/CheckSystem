# PowerIt6932AS 控制器接口文档

## 控制器信息

| 控制器名称 | 控制器描述 |
|-----------|-----------|
| PowerIt6932As | 6932AS电源控制器 |

## 公共字段

| 名称 | 类型 | 类别 | 描述 |
|------|------|------|------|
| MySerialPort | MySerialPort | 总线 | 串口 |
| VoltageRead | float | 读 | R,输出电压值 |
| CurrenttRead | float | 读 | R,输出电流值 |

## 公共方法

| 名称 | 参数 | 类型 | 说明 | 示例 |
|------|------|------|------|------|
| ConnectPower | protocolValue: string | 方法 | 连接电源 | ConnectPower("COM1:9600") |
| PowerOn | 无 | 方法 | 打开通道输出 |  |
| PowerOff | 无 | 方法 | 关闭通道输出 |  |
| SetCombSerOn | 无 | 方法 | 串联模式开启 |  |
| SetCombParaOn | 无 | 方法 | 并联模式开启 |  |
| SetCombOff | 无 | 方法 | 关闭组合模式 |  |
| SetVoltage1 | voltage: float | 方法 | 设置通道1的电压值 | SetVoltage1(12.0) |
| SetVoltage2 | voltage: float | 方法 | 设置通道2的电压值 | SetVoltage2(12.0) |
| SetVoltage3 | voltage: float | 方法 | 设置通道3的电压值 | SetVoltage3(12.0) |
| SetCurrent1 | current: float | 方法 | 设置通道1的电流值 | SetCurrent1(1.0) |
| SetCurrent2 | current: float | 方法 | 设置通道2的电流值 | SetCurrent2(1.0) |
| SetCurrent3 | current: float | 方法 | 设置通道3的电流值 | SetCurrent3(1.0) |
| SetVoltage | voltage: float | 方法 | 设置通道的电压值 | SetVoltage(12.0) |
| SetCurrent | current: float | 方法 | 设置通道的电流值 | SetCurrent(1.0) |
| ReadVoltage | 无 | 方法 | 读取通道的输出电压值 |  |
| ReadCurrent | 无 | 方法 | 读取通道的输出电流值 |  |
| ReadCurrentAndVoltage | 无 | 方法 | 读取通道的输出电流和电压值 |  |
| LocalControll | 无 | 方法 | 本地连接 |  |
