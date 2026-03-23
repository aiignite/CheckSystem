# HopooColorOhs350XUsb 控制器接口文档

## 控制器信息

| 控制器名称 | 控制器描述 |
|-----------|-----------|
| HopooColorOhs350XUsb |  |

## 公共字段

| 名称 | 类型 | 类别 | 描述 |
|------|------|------|------|
| Lux | float | 读 | R,照度/lx |
| Color | float | 读 | R,色温/K |
| CoordinateX | float | 读 | R,色坐标/X |
| CoordinateY | float | 读 | R,色坐标/Y |

## 公共方法

| 名称 | 参数 | 类型 | 说明 | 示例 |
|------|------|------|------|------|
| ConnectSerialPort | portName: string | 方法 | 连接串口 | ConnectSerialPort("COM1") |
| StartTestByAutoIntegralTime | 无 | 方法 | 通过自动积分时间启动测试 |  |
