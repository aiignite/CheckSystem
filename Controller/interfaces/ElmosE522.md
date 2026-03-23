# ElmosE522 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | ElmosE522 |
| 描述 | (无描述) |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| MySerialPort | MySerialPort | 串口通信 |
| ReadProgStatusResult | string | R,写入后读取ProgStatus结果 |
| ReadIdResult | string | R,读取ID结果 |
| ReadRegResult | string | R,读寄存器结果 |
| OtpProgramResult | string | R,OTP编程结果 |
| OtpRegToWriteFromConfig | string | R,配置文件中准备写入OTP的寄存器内容 |
| OtpRegRead | string | R,OTP读取内容 |
| OtpReadAndComprareWithConfigResult | string | R,OTP读取内容与配置文件比较结果 |
| OtpConfigFilePath | string | R/W,配置文件路径 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| Read_BUS_PULSE | devAddressDec: string | void | 读总线模式下的LED的占空比 |
| Read_BUS_CURRENT | devAddressDec: string | void | 读总线模式下的LED的电流 |
| Read_LED_ENABLE | devAddressDec: string | void | 读LED通道使能 |
| Read_RESULT_VLED | devAddressDec: string | void | 读通道的LED电压值 |
| Read_RESULT_VDIF | devAddressDec: string | void | 读供电电压与LED电压差值 |
| Read_RESULT_ILED | devAddressDec: string | void | 读LED电流值 |
| Read_LED_OPEN_SHORT | devAddressDec: string | void | 读LED开路短路状态 |
| ClearAllReadResult | 无 | void | 清空所有读取结果 |
| ReadReg | devAddressDec: string, memoryAddrAddressHex: string, readCount: string | void | 读寄存器 |
| Write_COM_DEV_ADDR | devIdToWriteDec: string | void | 写入ID |

## 方法详细说明

### Read_BUS_PULSE

**描述**: 读总线模式下的LED的占空比

**参数**:
- `devAddressDec` (string): 设备地址(十进制)

**示例代码**:
```csharp
controller.Read_BUS_PULSE("1");
```

---

### Read_BUS_CURRENT

**描述**: 读总线模式下的LED的电流

**参数**:
- `devAddressDec` (string): 设备地址(十进制)

**示例代码**:
```csharp
controller.Read_BUS_CURRENT("1");
```

---

### Read_LED_ENABLE

**描述**: 读LED通道使能

**参数**:
- `devAddressDec` (string): 设备地址(十进制)

**示例代码**:
```csharp
controller.Read_LED_ENABLE("1");
```

---

### Read_RESULT_VLED

**描述**: 读通道的LED电压值

**参数**:
- `devAddressDec` (string): 设备地址(十进制)

**示例代码**:
```csharp
controller.Read_RESULT_VLED("1");
```

---

### Read_RESULT_VDIF

**描述**: 读供电电压与LED电压差值

**参数**:
- `devAddressDec` (string): 设备地址(十进制)

**示例代码**:
```csharp
controller.Read_RESULT_VDIF("1");
```

---

### Read_RESULT_ILED

**描述**: 读LED电流值

**参数**:
- `devAddressDec` (string): 设备地址(十进制)

**示例代码**:
```csharp
controller.Read_RESULT_ILED("1");
```

---

### Read_LED_OPEN_SHORT

**描述**: 读LED开路短路状态

**参数**:
- `devAddressDec` (string): 设备地址(十进制)

**示例代码**:
```csharp
controller.Read_LED_OPEN_SHORT("1");
```

---

### ClearAllReadResult

**描述**: 清空所有读取结果

**示例代码**:
```csharp
controller.ClearAllReadResult();
```

---

### ReadReg

**描述**: 读寄存器

**参数**:
- `devAddressDec` (string): 设备地址(十进制)
- `memoryAddrAddressHex` (string): 内存地址(十六进制)
- `readCount` (string): 读取数量

**示例代码**:
```csharp
controller.ReadReg("1", "B8", "2");
```

---

### Write_COM_DEV_ADDR

**描述**: 写入ID

**参数**:
- `devIdToWriteDec` (string): 设备ID(十进制)

**示例代码**:
```csharp
controller.Write_COM_DEV_ADDR("5");
```