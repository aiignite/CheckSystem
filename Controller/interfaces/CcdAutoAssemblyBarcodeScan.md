# CcdAutoAssemblyBarcodeScan

## Controller Description

CCD自动装配条码扫描控制器

## Public Fields

| 名称 | 描述 | 类型 |
|------|------|------|
| LoadPlateBarcode | 载具条码 | string |
| BarcodeTriggerCmd | 条码触发命令 | string |
| IsReadingBarcode | 是否正在读取条码 | bool |
| ReadingBarcodeNG | 条码读取NG | bool |

## Public Methods

| 方法名称 | 描述 | 参数 |
|----------|------|------|
| ConnectBarcode | 连接条码扫描器 | ipPort |
| ReadBarcode | 读取条码 | format |
