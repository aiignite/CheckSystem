# CcdAutoAssemblyLaserPrinter 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | CcdAutoAssemblyLaserPrinter |
| 描述 | (无描述) |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| PrintBarcode1 | filePath: string | void | 写入文档 |
| PrintBarcodeNG | filePath: string | void | 写入NG文档 |

## 方法详细说明

### PrintBarcode1

**描述**: 写入文档

**参数**:
- `filePath` (string): 文件路径

**示例代码**:
```csharp
controller.PrintBarcode1("C:\\temp\\barcode.txt");
```

---

### PrintBarcodeNG

**描述**: 写入NG文档

**参数**:
- `filePath` (string): 文件路径

**示例代码**:
```csharp
controller.PrintBarcodeNG("C:\\temp\\barcode_ng.txt");
```