# BarcodeScanReader 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | BarcodeScanReader |
| 描述 | 条码扫描阅读器 |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| IsInUsing | bool | 是否正在使用 |
| GetBarcodeStr | string | R/W,扫码枪读取字符串 |
| LockBarcode | bool | 锁定条码 |
| GetBarcodeLength | int | R,当前读取字符串长度 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| ConnectBarcodeScanner | protocolValue: string | void | 连接扫码枪 |
| GetBarcode | keyAndKeyindexAndLen: string | void | 获取条码 |
| ReadBarcode | length: int | void | 读取条码 |
| StopReadBarcode | 无 | void | 停止读取条码 |
| LockBarcodeScanReader | 无 | void | 锁定条码扫描器 |
| UnlockBarcodeScanReader | 无 | void | 解锁条码扫描器 |

## 方法详细说明

### ConnectBarcodeScanner

**描述**: 连接扫码枪

**参数**:
- `protocolValue` (string): 连接协议 (格式: "COM1:115200" 或 "192.168.1.100:8080")

**示例代码**:
```csharp
controller.ConnectBarcodeScanner("COM1:115200");
```

---

### GetBarcode

**描述**: 获取条码

**参数**:
- `keyAndKeyindexAndLen` (string): 解析格式 (格式: "Key:KeyIndex:Len", 例如 "LM:0:17")

**示例代码**:
```csharp
controller.GetBarcode("LM:0:17");
string barcode = controller.GetBarcodeStr;
```

---

### ReadBarcode

**描述**: 读取条码

**参数**:
- `length` (int): 读取长度

**示例代码**:
```csharp
controller.ReadBarcode(10);
string barcode = controller.GetBarcodeStr;
int len = controller.GetBarcodeLength;
```

---

### StopReadBarcode

**描述**: 停止读取条码

**示例代码**:
```csharp
controller.StopReadBarcode();
```

---

### LockBarcodeScanReader

**描述**: 锁定条码扫描器

**示例代码**:
```csharp
controller.LockBarcodeScanReader();
```

---

### UnlockBarcodeScanReader

**描述**: 解锁条码扫描器

**示例代码**:
```csharp
controller.UnlockBarcodeScanReader();
```
