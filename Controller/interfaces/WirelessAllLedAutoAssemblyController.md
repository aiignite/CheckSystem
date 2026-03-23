# WirelessAllLedAutoAssemblyController 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: WirelessAllLedAutoAssemblyController

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| WirelessAllLedAutoAssemblyController | 产品选择 | AddProduct | 方法 | 方法 | string productNoProductName | 添加产品 | - |
| WirelessAllLedAutoAssemblyController | 产品选择 | SelectProduct | 方法 | 方法 | - | 选择产品 | - |
| WirelessAllLedAutoAssemblyController | 追溯载盘 | SetCurrentProcessName | 方法 | 方法 | string processName | 设置当前工序名称 | - |
| WirelessAllLedAutoAssemblyController | 追溯载盘 | SetLastProcessName | 方法 | 方法 | string processName | 设置上一工序名称 | - |
| WirelessAllLedAutoAssemblyController | 追溯载盘 | SaveLoadPlateState | 方法 | 方法 | string plateState | 保存载盘状态 | - |
| WirelessAllLedAutoAssemblyController | 追溯载盘 | ReadLoadPlateState | 方法 | 方法 | - | 读取载盘状态 | - |
| WirelessAllLedAutoAssemblyController | 追溯载盘 | ReadLoadPlateStateCopyToComplete | 方法 | 方法 | - | 读取载盘状态并复制到完成 | - |
| WirelessAllLedAutoAssemblyController | PCBA二维码 | SavePcbaBarcode | 方法 | 方法 | - | 保存PCBA条码 | - |
| WirelessAllLedAutoAssemblyController | PCBA二维码 | ReadPcbaBarcode | 方法 | 方法 | - | 读取PCBA条码 | - |
| WirelessAllLedAutoAssemblyController | 扫码 | ConnectBarcode | 方法 | 方法 | string ipPort | 连接条码扫描器 | - |
| WirelessAllLedAutoAssemblyController | 扫码 | ReadBarcode | 方法 | 方法 | string format | 读取条码 | - |
| WirelessAllLedAutoAssemblyController | 激光打标 | PrintOkNg1 | 方法 | 方法 | string filePath | 打印产品1结果 | - |
| WirelessAllLedAutoAssemblyController | 激光打标 | PrintOkNg2 | 方法 | 方法 | string filePath | 打印产品2结果 | - |
| WirelessAllLedAutoAssemblyController | 激光打标 | PrintOkNg3 | 方法 | 方法 | string filePath | 打印产品3结果 | - |
| WirelessAllLedAutoAssemblyController | 激光打标 | PrintOkNg4 | 方法 | 方法 | string filePath | 打印产品4结果 | - |
| WirelessAllLedAutoAssemblyController | 激光打标 | PrintBarcode1 | 方法 | 方法 | string filePath | 打印条码1 | - |
| WirelessAllLedAutoAssemblyController | 激光打标 | PrintBarcode2 | 方法 | 方法 | string filePath | 打印条码2 | - |
| WirelessAllLedAutoAssemblyController | 激光打标 | PrintBarcode3 | 方法 | 方法 | string filePath | 打印条码3 | - |
| WirelessAllLedAutoAssemblyController | 激光打标 | PrintBarcode4 | 方法 | 方法 | string filePath | 打印条码4 | - |
| WirelessAllLedAutoAssemblyController | Modbus客户端 | InitClient | 方法 | 方法 | string ipPort | 初始化Modbus客户端 | - |
| WirelessAllLedAutoAssemblyController | Modbus客户端 | ConnectPlc | 方法 | 方法 | string ipPort | 连接PLC | - |
