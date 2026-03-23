# WirelessBarcodeScanReader 控制器接口文档

## 控制器基本信息

| 控制器名称 | 控制器描述 |
|------------|------------|
| WirelessBarcodeScanReader | 无线条码扫描器 |

## 公共属性

| 名称 | 类型 | 类别 | 说明 | 示例 |
|------|------|------|------|------|
| IsInUsing | bool | 属性 | 使用中 | - |
| GetBarcodeStr | string | 属性 | 条码字符串 | - |
| LockBarcode | bool | 属性 | 锁定条码 | - |
| GetBarcodeLength | int | 属性 | 条码长度 | - |
| ReadBarcodeTimeoutMs | int | 属性 | 读取条码超时(毫秒) | - |

## 公共方法

| 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|------|------|------|------|------|------|
| ConnectBarcodeScanner | void | 方法 | protocolValue: string | 连接条码扫描器 | ConnectBarcodeScanner("COM1:115200") |
| GetBarcode | void | 方法 | keyAndKeyindexAndLen: string | 获取条码 | GetBarcode("KEY:7:45") |
| ReadBarcode | void | 方法 | length: int | 读取条码 | ReadBarcode(10) |
| StopReadBarcode | void | 方法 | 无 | 停止读取条码 | StopReadBarcode() |
| LockBarcodeScanReader | void | 方法 | 无 | 锁定条码扫描器 | LockBarcodeScanReader() |
| UnlockBarcodeScanReader | void | 方法 | 无 | 解锁条码扫描器 | UnlockBarcodeScanReader() |