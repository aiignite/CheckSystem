# PowerNgi 控制器接口文档

## 控制器信息

| 控制器名称 | 控制器描述 |
|-----------|-----------|
| PowerNgi | NGI电源控制器 |

## 公共字段

| 名称 | 类型 | 类别 | 描述 |
|------|------|------|------|
| Volt1 | float | 读 | R,电压1 |
| Volt2 | float | 读 | R,电压2 |
| Curr1 | float | 读 | R,电流1 |
| Curr2 | float | 读 | R,电流2 |

## 公共方法

| 名称 | 参数 | 类型 | 说明 | 示例 |
|------|------|------|------|------|
| ConnectPower | ipPort: string | 方法 | 连接电源 | ConnectPower("192.168.1.1:5000") |
| PowerOnCh1 | 无 | 方法 | 打开通道1输出 |  |
| PowerOffCh1 | 无 | 方法 | 关闭通道1输出 |  |
| PowerOnCh2 | 无 | 方法 | 打开通道2输出 |  |
| PowerOffCh2 | 无 | 方法 | 关闭通道2输出 |  |
| SetVoltCh1 | volt: string | 方法 | 设置通道1电压 | SetVoltCh1("12.0") |
| SetCurrCh1 | curr: string | 方法 | 设置通道1电流 | SetCurrCh1("5.0") |
| SetVoltCh2 | volt: string | 方法 | 设置通道2电压 | SetVoltCh2("12.0") |
| SetCurrCh2 | curr: string | 方法 | 设置通道2电流 | SetCurrCh2("5.0") |
| ReadCurrVolt | 无 | 方法 | 读取电流电压 |  |
