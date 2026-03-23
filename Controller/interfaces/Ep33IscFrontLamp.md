# Ep33IscFrontLamp 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | Ep33IscFrontLamp |
| 描述 | CAN-Product,EP33-ISC-前灯 |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| CanFd | CanBus | CAN FD总线 |
| SoftwareVersion | string | R,软件版本号读取 |
| HardwareVersion | string | R,硬件版本号读取 |
| FaultRead | string | R,故障信息读取 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| ChangeToFontBLeft | 无 | void | 切换成L_Front_B |
| ChangeToFontBRight | 无 | void | 切换成R_Front_B |
| ChangeToFrontALeft | 无 | void | 切换成L_Front_A |
| ChangeToFrontARight | 无 | void | 切换成R_Front_A |
| YellowLedOn | grayValue: string | void | 点亮所有黄光LED |
| WhiteLedOn | grayValue: string | void | 点亮所有白光LED |
| WhiteLedOddOn | grayValue: string | void | 点亮所有单数白光LED |
| WhiteLedEvenOn | grayValue: string | void | 点亮所有双数白光LED |
| YellowLedOddOn | grayValue: string | void | 点亮所有单数黄光LED |
| YellowLedEvenOn | grayValue: string | void | 点亮所有双数黄光LED |
| AllLedOn | grayValue: string | void | 点亮所有LED |
| AllLedOff | 无 | void | 关闭所有LED |
| ReadSoftwareVersion | 无 | void | 读软件版本号 |
| ReadHardwareVersion | 无 | void | 读硬件版本号 |
| FaultDetect | 无 | void | 读取故障信息 |

## 方法详细说明

### ChangeToFontBLeft

**描述**: 切换成L_Front_B

**示例代码**:
```csharp
controller.ChangeToFontBLeft();
```

---

### ChangeToFrontALeft

**描述**: 切换成L_Front_A

**示例代码**:
```csharp
controller.ChangeToFrontALeft();
```

---

### YellowLedOn

**描述**: 点亮所有黄光LED

**参数**:
- `grayValue` (string): 灰度值

**示例代码**:
```csharp
controller.YellowLedOn("128");
```

---

### WhiteLedOn

**描述**: 点亮所有白光LED

**参数**:
- `grayValue` (string): 灰度值

**示例代码**:
```csharp
controller.WhiteLedOn("255");
```

---

### AllLedOn

**描述**: 点亮所有LED

**参数**:
- `grayValue` (string): 灰度值

**示例代码**:
```csharp
controller.AllLedOn("200");
```

---

### AllLedOff

**描述**: 关闭所有LED

**示例代码**:
```csharp
controller.AllLedOff();
```

---

### ReadSoftwareVersion

**描述**: 读软件版本号

**示例代码**:
```csharp
controller.ReadSoftwareVersion();
string ver = controller.SoftwareVersion;
```

---

### ReadHardwareVersion

**描述**: 读硬件版本号

**示例代码**:
```csharp
controller.ReadHardwareVersion();
string ver = controller.HardwareVersion;
```

---

### FaultDetect

**描述**: 读取故障信息

**示例代码**:
```csharp
controller.FaultDetect();
string faults = controller.FaultRead;
```
