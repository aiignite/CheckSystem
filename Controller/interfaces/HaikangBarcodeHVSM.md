# HaikangBarcodeHVSM 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: 海康条码HVSM控制器

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| HaikangBarcodeHVSM | 海康条码HVSM控制器 | BarcodeCount | int | 字段 | - | 需要条码数量 | - |
| HaikangBarcodeHVSM | 海康条码HVSM控制器 | ChangeResult | string | 字段 | - | 切换参数结果 | - |
| HaikangBarcodeHVSM | 海康条码HVSM控制器 | IsGetBar | bool | 字段 | - | 是否获取到条码 | - |
| HaikangBarcodeHVSM | 海康条码HVSM控制器 | Barcode1 | string | 字段 | - | 条码1 | - |
| HaikangBarcodeHVSM | 海康条码HVSM控制器 | Barcode2 | string | 字段 | - | 条码2 | - |
| HaikangBarcodeHVSM | 海康条码HVSM控制器 | Barcode3 | string | 字段 | - | 条码3 | - |
| HaikangBarcodeHVSM | 海康条码HVSM控制器 | Barcode4 | string | 字段 | - | 条码4 | - |
| HaikangBarcodeHVSM | 海康条码HVSM控制器 | Barcode5 | string | 字段 | - | 条码5 | - |
| HaikangBarcodeHVSM | 海康条码HVSM控制器 | Barcode6 | string | 字段 | - | 条码6 | - |
| HaikangBarcodeHVSM | 海康条码HVSM控制器 | Barcode7 | string | 字段 | - | 条码7 | - |
| HaikangBarcodeHVSM | 海康条码HVSM控制器 | Barcode8 | string | 字段 | - | 条码8 | - |
| HaikangBarcodeHVSM | 海康条码HVSM控制器 | Barcode9 | string | 字段 | - | 条码9 | - |
| HaikangBarcodeHVSM | 海康条码HVSM控制器 | Barcode10 | string | 字段 | - | 条码10 | - |
| HaikangBarcodeHVSM | 海康条码HVSM控制器 | Barcode11 | string | 字段 | - | 条码11 | - |
| HaikangBarcodeHVSM | 海康条码HVSM控制器 | Barcode12 | string | 字段 | - | 条码12 | - |
| HaikangBarcodeHVSM | 海康条码HVSM控制器 | Barcode13 | string | 字段 | - | 条码13 | - |
| HaikangBarcodeHVSM | 海康条码HVSM控制器 | Barcode14 | string | 字段 | - | 条码14 | - |
| HaikangBarcodeHVSM | 海康条码HVSM控制器 | Barcode15 | string | 字段 | - | 条码15 | - |
| HaikangBarcodeHVSM | 海康条码HVSM控制器 | Barcode16 | string | 字段 | - | 条码16 | - |
| HaikangBarcodeHVSM | 海康条码HVSM控制器 | InitTcpClient | void | 方法 | string ipstress | 连接 | - |
| HaikangBarcodeHVSM | 海康条码HVSM控制器 | InitTcpClientChangeByDate0109 | void | 方法 | string ipPort | 初始化客户端 | - |
| HaikangBarcodeHVSM | 海康条码HVSM控制器 | TcpClientClose | void | 方法 | - | 关闭TCP客户端 | - |
| HaikangBarcodeHVSM | 海康条码HVSM控制器 | ClientWrite | void | 方法 | string msg | 客户端发送 | - |
| HaikangBarcodeHVSM | 海康条码HVSM控制器 | ChangeParameter | void | 方法 | int idx | 切换参数 | - |
| HaikangBarcodeHVSM | 海康条码HVSM控制器 | ClearBarcode | void | 方法 | - | 清除条码 | - |
