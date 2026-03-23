# Ep33IscRearLamp 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | Ep33IscRearLamp |
| 描述 | CAN-Product,EP33-ISC-后灯 |
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
| ChangeToRclb | 无 | void | 切换成R_B |
| ChangeToRclaLeft | 无 | void | 切换成R_Left_A+C |
| ChangeToRclaRight | 无 | void | 切换成R_Right_A+C |
| RclbLeftTurnOn | grayValue: string | void | B灯左转向开 |
| RclbRightTurnOn | grayValue: string | void | B灯右转向开 |
| RedLedOn | grayValue: string | void | 点亮所有红光LED |
| RedLedOddOn | grayValue: string | void | 点亮所有单数红光LED |
| RedLedEvenOn | grayValue: string | void | 点亮所有双数红光LED |
| YelowLedOn | grayValue: string | void | 点亮所有黄光LED |
| YellowLedOddOn | grayValue: string | void | 点亮所有单数黄光LED |
| YellowLedEvenOn | grayValue: string | void | 点亮所有双数黄光LED |
| AllLedOff | 无 | void | 关闭所有LED |
| ReadSoftwareVersion | 无 | void | 读软件版本号 |
| ReadHardwareVersion | 无 | void | 读硬件版本号 |
| FaultDetect | 无 | void | 读取故障信息 |
| HardwareControl | isOn: bool | void | 硬件控制 |

## 方法详细说明

### ChangeToRclb

**描述**: 切换成R_B

**示例代码**:
```csharp
controller.ChangeToRclb();
```

---

### ChangeToRclaLeft

**描述**: 切换成R_Left_A+C

**示例代码**:
```csharp
controller.ChangeToRclaLeft();
```

---

### RclbLeftTurnOn

**描述**: B灯左转向开

**参数**:
- `grayValue` (string): 灰度值

**示例代码**:
```csharp
controller.RclbLeftTurnOn("128");
```

---

### RedLedOn

**描述**: 点亮所有红光LED

**参数**:
- `grayValue` (string): 灰度值

**示例代码**:
```csharp
controller.RedLedOn("255");
```

---

### YelowLedOn

**描述**: 点亮所有黄光LED

**参数**:
- `grayValue` (string): 灰度值

**示例代码**:
```csharp
controller.YelowLedOn("200");
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

### FaultDetect

**描述**: 读取故障信息

**示例代码**:
```csharp
controller.FaultDetect();
string faults = controller.FaultRead;
```

---

### HardwareControl

**描述**: 硬件控制

**参数**:
- `isOn` (bool): 是否开启

**示例代码**:
```csharp
controller.HardwareControl(true);
```
