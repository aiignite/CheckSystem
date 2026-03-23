# BarcodeScanReaderDlpEol

## Controller Description

DLP EOL条码扫描器控制器

## Public Fields

| 名称 | 描述 | 类型 |
|------|------|------|
| IsInUsing | 使用中标志 | bool |
| GetBarcodeStr | 获取条码字符串 | string |
| GetBarcodeStr1 | 获取条码字符串1 | string |
| LockBarcode | 锁定条码 | bool |
| GetBarcodeLength | 获取条码长度 | int |

## Public Methods

| 方法名称 | 描述 | 参数 |
|----------|------|------|
| ConnectBarcodeScanner | 连接条码扫描器 | protocolValue |
| GetBarcode | 获取条码 | keyAndKeyindexAndLen |
| ReadBarcode | 读取条码 | length |
| StopReadBarcode | 停止读取条码 |  |
| LockBarcodeScanReader | 锁定条码扫描器 |  |
| UnlockBarcodeScanReader | 解锁条码扫描器 |  |
