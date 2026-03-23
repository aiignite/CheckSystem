# HikBarcodeScaner 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | HikBarcodeScaner |
| 描述 | 海康条码扫描器 |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| ReadBarcodeResults | string | R,条码扫描结果JSON |
| ReadBarcodes | string | R,条码值(换行分隔) |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| OpenScanner | 无 | bool | 打开设备 |
| CloseScanner | 无 | void | 关闭设备 |
| StartGrabbing | 无 | bool | 打开采集 |
| StopGrabbing | 无 | void | 关闭采集 |
| SetContinuesMode | deviceSn: string | void | 设置为连续模式 |
| SetSoftTriggerMode | deviceSn: string | void | 设置为软触发模式 |
| SoftTrigger | deviceSn: string, delayMs: string | void | 软触发一次 |

## 方法详细说明

### OpenScanner

**描述**: 打开设备

**示例代码**:
```csharp
bool result = controller.OpenScanner();
```

---

### CloseScanner

**描述**: 关闭设备

**示例代码**:
```csharp
controller.CloseScanner();
```

---

### StartGrabbing

**描述**: 打开采集

**示例代码**:
```csharp
bool result = controller.StartGrabbing();
```

---

### StopGrabbing

**描述**: 关闭采集

**示例代码**:
```csharp
controller.StopGrabbing();
```

---

### SetContinuesMode

**描述**: 设置为连续模式

**参数**:
- `deviceSn` (string): 设备序列号

**示例代码**:
```csharp
controller.SetContinuesMode("deviceSN123");
```

---

### SetSoftTriggerMode

**描述**: 设置为软触发模式

**参数**:
- `deviceSn` (string): 设备序列号

**示例代码**:
```csharp
controller.SetSoftTriggerMode("deviceSN123");
```

---

### SoftTrigger

**描述**: 软触发一次

**参数**:
- `deviceSn` (string): 设备序列号
- `delayMs` (string): 延迟时间(毫秒),有效范围1-10000,默认2500

**示例代码**:
```csharp
controller.SoftTrigger("deviceSN123", "2500");
string barcodes = controller.ReadBarcodes;
```
