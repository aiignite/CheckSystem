# PowerNgi3412E 控制器接口文档

## 控制器信息

| 控制器名称 | 控制器描述 |
|-----------|-----------|
| PowerNgi3412E | NGI3412E多通道电源控制器 |

## 公共字段

| 名称 | 类型 | 类别 | 描述 |
|------|------|------|------|
| MsgDelayMs | int | 读/写 | R/W,每次发送消息后延时一段时间 |
| Volt1 | float | 读 | R,CH1电压读取 |
| Volt2 | float | 读 | R,CH2电压读取 |
| Volt3 | float | 读 | R,CH3电压读取 |
| Curr1 | float | 读 | R,CH1电流读取 |
| Curr2 | float | 读 | R,CH2电流读取 |
| Curr3 | float | 读 | R,CH3电流读取 |

## 公共方法

| 名称 | 参数 | 类型 | 说明 | 示例 |
|------|------|------|------|------|
| InitPower | ip: string | 方法 | 初始化电源 | InitPower("192.168.1.1") |
| ConnectPower | protocolValue: string | 方法 | 连接电源 | ConnectPower("192.168.1.1") |
| PowerOnCH1 | 无 | 方法 | 打开电源通道1 |  |
| PowerOnCH2 | 无 | 方法 | 打开电源通道2 |  |
| PowerOnCH3 | 无 | 方法 | 打开电源通道3 |  |
| PowerOffCH1 | 无 | 方法 | 关闭电源通道1 |  |
| PowerOffCH2 | 无 | 方法 | 关闭电源通道2 |  |
| PowerOffCH3 | 无 | 方法 | 关闭电源通道3 |  |
| PowerOn | 无 | 方法 | 打开电源所有通道 |  |
| PowerOff | 无 | 方法 | 关闭电源所有通道 |  |
| SetCombSerOn | 无 | 方法 | 打开串联模式 |  |
| SetCombParaOn | 无 | 方法 | 打开并联模式 |  |
| SetCombOff | 无 | 方法 | 关闭并联模式 |  |
| SetVoltage1 | voltage: float | 方法 | 设置通道1电压 | SetVoltage1(12.0) |
| SetVoltage2 | voltage: float | 方法 | 设置通道2电压 | SetVoltage2(12.0) |
| SetVoltage3 | voltage: float | 方法 | 设置通道3电压 | SetVoltage3(12.0) |
| SetCurrent1 | current: float | 方法 | 设置通道1电流 | SetCurrent1(1.0) |
| SetCurrent2 | current: float | 方法 | 设置通道2电流 | SetCurrent2(1.0) |
| SetCurrent3 | current: float | 方法 | 设置通道3电流 | SetCurrent3(1.0) |
| SetVoltAll | ch1Volt: float, ch2Volt: float, ch3Volt: float | 方法 | 设置所有通道电压 | SetVoltAll(12.0, 12.0, 12.0) |
| SetCurrAll | ch1Curr: float, ch2Curr: float, ch3Curr: float | 方法 | 设置所有通道电流 | SetCurrAll(1.0, 1.0, 1.0) |
| ReadCurrAndVolt | 无 | 方法 | 读取输入 |  |
