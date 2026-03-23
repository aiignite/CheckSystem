# AnalogMax25608 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | AnalogMax25608 |
| 描述 | (无描述) |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| MySerialPort | MySerialPort | 串口通信 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| AddDev | deviceAddressHex: string | void | 添加设备 |
| Remove | deviceAddressHex: string | void | 移除设备 |
| TargetDevSingleChOn | deviceAddressHex: string, channelDecimal: string | void | 单通道打开 |
| TargetDevSingleChOff | deviceAddressHex: string, channelDecimal: string | void | 单通道关闭 |
| TargetDevOddChOn | deviceAddressHex: string | void | 奇数通道打开 |
| TargetDevOddChOff | deviceAddressHex: string | void | 奇数通道关闭 |
| TargetDevEvenChOn | deviceAddressHex: string | void | 偶数通道打开 |
| TargetDevEvenChOff | deviceAddressHex: string | void | 偶数通道关闭 |
| TargetDevAllChOn | deviceAddressHex: string | void | 所有通道打开 |
| TargetDevAllChOff | deviceAddressHex: string | void | 所有通道关闭 |

## 方法详细说明

### AddDev

**描述**: 添加设备

**参数**:
- `deviceAddressHex` (string): 设备地址(十六进制)

**示例代码**:
```csharp
controller.AddDev("00");
```

---

### Remove

**描述**: 移除设备

**参数**:
- `deviceAddressHex` (string): 设备地址(十六进制)

**示例代码**:
```csharp
controller.Remove("00");
```

---

### TargetDevSingleChOn

**描述**: 单通道打开

**参数**:
- `deviceAddressHex` (string): 设备地址(十六进制)
- `channelDecimal` (string): 通道号(十进制)

**示例代码**:
```csharp
controller.TargetDevSingleChOn("00", "1");
```

---

### TargetDevSingleChOff

**描述**: 单通道关闭

**参数**:
- `deviceAddressHex` (string): 设备地址(十六进制)
- `channelDecimal` (string): 通道号(十进制)

**示例代码**:
```csharp
controller.TargetDevSingleChOff("00", "1");
```

---

### TargetDevOddChOn

**描述**: 奇数通道打开

**参数**:
- `deviceAddressHex` (string): 设备地址(十六进制)

**示例代码**:
```csharp
controller.TargetDevOddChOn("00");
```

---

### TargetDevOddChOff

**描述**: 奇数通道关闭

**参数**:
- `deviceAddressHex` (string): 设备地址(十六进制)

**示例代码**:
```csharp
controller.TargetDevOddChOff("00");
```

---

### TargetDevEvenChOn

**描述**: 偶数通道打开

**参数**:
- `deviceAddressHex` (string): 设备地址(十六进制)

**示例代码**:
```csharp
controller.TargetDevEvenChOn("00");
```

---

### TargetDevEvenChOff

**描述**: 偶数通道关闭

**参数**:
- `deviceAddressHex` (string): 设备地址(十六进制)

**示例代码**:
```csharp
controller.TargetDevEvenChOff("00");
```

---

### TargetDevAllChOn

**描述**: 所有通道打开

**参数**:
- `deviceAddressHex` (string): 设备地址(十六进制)

**示例代码**:
```csharp
controller.TargetDevAllChOn("00");
```

---

### TargetDevAllChOff

**描述**: 所有通道关闭

**参数**:
- `deviceAddressHex` (string): 设备地址(十六进制)

**示例代码**:
```csharp
controller.TargetDevAllChOff("00");
```