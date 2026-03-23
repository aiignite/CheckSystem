# E2EpMcu 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | E2EpMcu |
| 描述 | CAN-Product,E2-EPMCU |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| Can | CanBus | CAN总线 |
| ProductSerialNo | string | R,生产序列号-F18C |
| PartNo | string | R,总成零件号-F187 |
| FblPartNo | string | R,引导程序零件号-F183 |
| AppPartNo | string | R,应用程序零件号-F1A0 |
| SupplySwNo | string | R,供应商软件参考号-F194 |
| CaliPartNo | string | R,标定程序零件号-F1A1 |
| DownloadResult | string | R,下载结果 |
| AppFilePath | string | R/W,APP文件路径 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| Awake | 无 | void | 唤醒 |
| Sleep | 无 | void | 休眠 |
| EnterDefaultSession | 无 | void | 进入正常模式 |
| EnterExtendedSession | 无 | void | 进入拓展模式 |
| EnterProgramSession | 无 | void | 进入编程模式 |
| SecurityAccess | subFunc: string | bool | 解锁SeedKey |
| ReadProductSerialNo | 无 | void | ReadF18C-生产序列号 |
| ReadPartNo | 无 | void | ReadF187-总成零件号 |
| ReadFblPartNo | 无 | void | ReadF183-引导程序零件号 |
| ReadAppPartNo | 无 | void | ReadF1A0-应用程序零件号 |
| ReadSupplySwNo | 无 | void | ReadF194-供应商软件参考号 |
| ReadCaliPartNo | 无 | void | ReadF1A1-标定程序零件号 |
| WritePartNo | 无 | void | WriteF187-总成零件号 |
| WriteSerialNo | 无 | void | WriteF18C-生产序列号 |
| DownloadFile | 无 | void | 下载 |

## 方法详细说明

### Awake

**描述**: 唤醒

**示例代码**:
```csharp
controller.Awake();
```

---

### Sleep

**描述**: 休眠

**示例代码**:
```csharp
controller.Sleep();
```

---

### EnterDefaultSession

**描述**: 进入正常模式

**示例代码**:
```csharp
controller.EnterDefaultSession();
```

---

### EnterExtendedSession

**描述**: 进入拓展模式

**示例代码**:
```csharp
controller.EnterExtendedSession();
```

---

### EnterProgramSession

**描述**: 进入编程模式

**示例代码**:
```csharp
controller.EnterProgramSession();
```

---

### SecurityAccess

**描述**: 解锁SeedKey

**参数**:
- `subFunc` (string): 子功能（"1"或"5"）

**返回类型**: bool

**示例代码**:
```csharp
bool success = controller.SecurityAccess("1");
```

---

### ReadProductSerialNo

**描述**: ReadF18C-生产序列号

**示例代码**:
```csharp
controller.ReadProductSerialNo();
string serialNo = controller.ProductSerialNo;
```

---

### ReadPartNo

**描述**: ReadF187-总成零件号

**示例代码**:
```csharp
controller.ReadPartNo();
string partNo = controller.PartNo;
```

---

### ReadAppPartNo

**描述**: ReadF1A0-应用程序零件号

**示例代码**:
```csharp
controller.ReadAppPartNo();
string appPartNo = controller.AppPartNo;
```

---

### DownloadFile

**描述**: 下载

**示例代码**:
```csharp
controller.DownloadFile();
string result = controller.DownloadResult;
```
