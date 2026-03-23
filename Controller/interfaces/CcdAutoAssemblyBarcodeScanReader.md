# CcdAutoAssemblyBarcodeScanReader 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | CcdAutoAssemblyBarcodeScanReader |
| 描述 | (无描述) |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| IsInUsing | bool | 是否正在使用 |
| GetBarcodeStr | string | R,当前读码数据 |
| LockBarcode | bool | 锁定条码 |
| GetBarcodeLength | int | R,当前读码数据缓存 |
| DequeueBuffBarcode | string | R,缓存中取得的条码 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| ConnectBarcodeScanner | protocolValue: string | void | 连接读码器 |
| ReadBarcode | 无 | void | 读码 |
| DequeueBarcode | 无 | void | 取队列缓存 |

## 方法详细说明

### ConnectBarcodeScanner

**描述**: 连接读码器

**参数**:
- `protocolValue` (string): 协议值 (格式: "COM1:115200" 或 "192.168.1.100:8080")

**示例代码**:
```csharp
controller.ConnectBarcodeScanner("COM1:115200");
// 或
controller.ConnectBarcodeScanner("192.168.1.100:8080");
```

---

### ReadBarcode

**描述**: 读码

**示例代码**:
```csharp
controller.ReadBarcode();
string barcode = controller.GetBarcodeStr;
```

---

### DequeueBarcode

**描述**: 取队列缓存

**示例代码**:
```csharp
controller.DequeueBarcode();
string barcode = controller.DequeueBuffBarcode;
```