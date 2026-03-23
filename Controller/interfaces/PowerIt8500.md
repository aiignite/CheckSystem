# PowerIt8500 控制器接口文档

## 控制器信息

| 控制器名称 | 控制器描述 |
|-----------|-----------|
| PowerIt8500 | TCP电源控制器 |

## 公共方法

| 名称 | 参数 | 类型 | 说明 | 示例 |
|------|------|------|------|------|
| InitRemoteEndpoint | host: string | 方法 | 初始化远程连接 | InitRemoteEndpoint("192.168.1.1:5000") |
| SetRemoteControl | 无 | 方法 | 设置远程控制 |  |
| SetLocalControl | 无 | 方法 | 设置本地控制 |  |
| SetPowerModel | model: int | 方法 | 设置负载模式,0:CC,1:CV,2:CW,3:CR | SetPowerModel(1) |
| SetVoltage | voltage: float | 方法 | 设置电压 | SetVoltage(12.0) |
| SetIo | io: float | 方法 | 设置电流 | SetIo(5.0) |
| SetMaxA | i: float | 方法 | 设置最大电流 | SetMaxA(10) |
| SetOutPutState | state: int | 方法 | 设置输出状态,0:关闭,1:打开 | SetOutPutState(1) |
