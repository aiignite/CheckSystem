# PowerIt39120 控制器接口文档

## 控制器信息

| 控制器名称 | 控制器描述 |
|-----------|-----------|
| PowerIt39120 | UDP电源控制器 |

## 公共字段

| 名称 | 类型 | 类别 | 描述 |
|------|------|------|------|
| CurrentV | float | 读 | R,读取电压值 |
| CurrentC | float | 读 | R,读取电流值 |

## 公共方法

| 名称 | 参数 | 类型 | 说明 | 示例 |
|------|------|------|------|------|
| InitPower | ipPort: string | 方法 | 初始化电源 | InitPower("192.168.1.1:5000") |
| PowerOn1 | 无 | 方法 | 电源通道打开 |  |
| PowerOff | 无 | 方法 | 电源通道关闭 |  |
| PowerSetV | v: float | 方法 | 设置电压值 | PowerSetV(12.0) |
| ReadV | 无 | 方法 | 读取电压值 |  |
| PowerSetC | c: float | 方法 | 设置电流值 | PowerSetC(5.0) |
| ReadC | 无 | 方法 | 读取电流值 |  |
