# PesControlPlc 控制器接口文档

## 控制器信息

| 控制器名称 | 控制器描述 |
|-----------|-----------|
| PesControlPlc | Modbus TCP PLC控制器 |

## 公共字段

| 名称 | 类型 | 类别 | 描述 |
|------|------|------|------|
| PcWriteCheckEnd | bool | 读/写 | R/W,40001检测完成 |
| PcWriteCheckOk | bool | 读/写 | R/W,40002检测结果OK |
| PcWriteCheckNg | bool | 读/写 | R/W,40003检测结果NG |
| PcWriteServoRun | bool | 读/写 | R/W,40004检测模组同步 |
| PcWriteBarcodeScanEnd | bool | 读/写 | R/W,40005扫码完成 |
| PcWriteProductType | int | 读/写 | R/W,40006当前产品编号 |
| PcWriteServoXPos | float | 读/写 | R/W,40007~40008检测模组X位置 |
| PcWriteServoZPos | float | 读/写 | R/W,40009~40010检测模组Z位置 |
| PcReadCheckStart | bool | 读 | R,400121检测启动 |
| PcReadServoRunEnd | bool | 读 | R,400122检测模组到位 |
| PcReadBarcodeScanStart | bool | 读 | R,400123扫码启动 |
| PcReadIlluminometer | float | 读 | R,400125~400126照度计寄存器 |
| PreStart | bool | 读 | R,预启动信号 |
| BarcodeResult | string | 读 | 扫码结果 |
| BarcodeTriggerCmd | string | 读 | 扫码触发命令 |
| IsReadingBarcode | bool | 读 | 是否正在读取条码 |

## 公共方法

| 名称 | 参数 | 类型 | 说明 | 示例 |
|------|------|------|------|------|
| InitRemoteIpAddress | ipport: string | 方法 | 连接到siemens PLC的socket server上 | InitRemoteIpAddress("192.168.1.1:502") |
| CycleUpdate | 无 | 方法 | 循环更新 |  |
| ConnectBarcode | ipPort: string | 方法 | 连接条码扫码器 | ConnectBarcode("192.168.1.1:5000") |
| DequeueBarocde | 无 | 方法 | 取出条码 |  |
| ClearBarcodeBuff | 无 | 方法 | 清除条码缓冲 |  |
| ReadBarcode | format: string | 方法 | 读取条码 | ReadBarcode("T") |
