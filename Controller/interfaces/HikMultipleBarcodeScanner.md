# 控制器文档

## HikMultipleBarcodeScanner

### 控制器描述
无

### 字段

| 名称 | 类型 | 类别 | 说明 |
|------|------|------|------|
| IsGetBar | bool | R | 是否取得条码 |
| MaxScanCodeCount | int | R | 最大扫描数量 |
| CurrentReadBarcode1 | string | R | 当前扫描的条码1 |
| CurrentReadBarcode2 | string | R | 当前扫描的条码2 |
| CurrentReadBarcode3 | string | R | 当前扫描的条码3 |
| CurrentReadBarcode4 | string | R | 当前扫描的条码4 |
| CurrentReadBarcode5 | string | R | 当前扫描的条码5 |
| CurrentReadBarcode6 | string | R | 当前扫描的条码6 |
| CurrentReadBarcode7 | string | R | 当前扫描的条码7 |
| CurrentReadBarcode8 | string | R | 当前扫描的条码8 |
| CurrentReadBarcode9 | string | R | 当前扫描的条码9 |
| CurrentReadBarcode10 | string | R | 当前扫描的条码10 |
| CurrentReadBarcode11 | string | R | 当前扫描的条码11 |
| CurrentReadBarcode12 | string | R | 当前扫描的条码12 |
| CurrentReadBarcode13 | string | R | 当前扫描的条码13 |
| CurrentReadBarcode14 | string | R | 当前扫描的条码14 |
| CurrentReadBarcode15 | string | R | 当前扫描的条码15 |
| CurrentReadBarcode16 | string | R | 当前扫描的条码16 |
| CurrentReadBarcode17 | string | R | 当前扫描的条码17 |
| CurrentReadBarcode18 | string | R | 当前扫描的条码18 |
| CurrentReadBarcode19 | string | R | 当前扫描的条码19 |
| CurrentReadBarcode20 | string | R | 当前扫描的条码20 |
| DequeueBuffBarcode1 | string | R | 缓存中取得的条码1 |
| DequeueBuffBarcode2 | string | R | 缓存中取得的条码2 |
| DequeueBuffBarcode3 | string | R | 缓存中取得的条码3 |
| DequeueBuffBarcode4 | string | R | 缓存中取得的条码4 |
| DequeueBuffBarcode5 | string | R | 缓存中取得的条码5 |
| DequeueBuffBarcode6 | string | R | 缓存中取得的条码6 |
| DequeueBuffBarcode7 | string | R | 缓存中取得的条码7 |
| DequeueBuffBarcode8 | string | R | 缓存中取得的条码8 |
| DequeueBuffBarcode9 | string | R | 缓存中取得的条码9 |
| DequeueBuffBarcode10 | string | R | 缓存中取得的条码10 |
| DequeueBuffBarcode11 | string | R | 缓存中取得的条码11 |
| DequeueBuffBarcode12 | string | R | 缓存中取得的条码12 |
| DequeueBuffBarcode13 | string | R | 缓存中取得的条码13 |
| DequeueBuffBarcode14 | string | R | 缓存中取得的条码14 |
| DequeueBuffBarcode15 | string | R | 缓存中取得的条码15 |
| DequeueBuffBarcode16 | string | R | 缓存中取得的条码16 |
| DequeueBuffBarcode17 | string | R | 缓存中取得的条码17 |
| DequeueBuffBarcode18 | string | R | 缓存中取得的条码18 |
| DequeueBuffBarcode19 | string | R | 缓存中取得的条码19 |
| DequeueBuffBarcode20 | string | R | 缓存中取得的条码20 |

### 方法

| 名称 | 参数 | 说明 | 示例 |
|------|------|------|------|
| InitTcpClient | ipstress |  |  |
| ClientWriteHex | msg |  |  |
| ClientWriteString | msg |  |  |
| ExecuteBarcodeScan | start, end | 执行读码 |  |
| DequeueBarcode |  | 取队列缓存 |  |
| SetBarcodePosition | index, xPos, yPos, xRange, yRange | 设置二维码坐标 |  |

