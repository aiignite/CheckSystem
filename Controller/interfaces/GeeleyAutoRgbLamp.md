# GeeleyAutoRgbLamp 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | GeeleyAutoRgbLamp |
| 描述 | LIN-Product,GEELY_仪表盘氛围灯 |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| Lin | LinBus | LIN总线 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| LampAwake | 无 | void | 控制板唤醒 |
| LampSleep | 无 | void | 控制板休眠 |
| SwitchRgd | red: string, green: string, blue: string | void | 切换RBG |
| BreathModeOn | 无 | void | 呼吸模式打开 |
| BreathModeOff | 无 | void | 呼吸模式关闭 |
| SwitchRed | 无 | void | Red |
| SwitchGreen | 无 | void | Green |
| SwitchBlue | 无 | void | Blue |
| AutoColorOn | 无 | void | AutoColorOn |

## 方法详细说明

### LampAwake

**描述**: 控制板唤醒

**示例代码**:
```csharp
controller.LampAwake();
```

---

### LampSleep

**描述**: 控制板休眠

**示例代码**:
```csharp
controller.LampSleep();
```

---

### SwitchRgd

**描述**: 切换RBG

**参数**:
- `red` (string): 红色值 (0-255)
- `green` (string): 绿色值 (0-255)
- `blue` (string): 蓝色值 (0-255)

**示例代码**:
```csharp
controller.SwitchRgd("255", "128", "0");
```

---

### BreathModeOn

**描述**: 呼吸模式打开

**示例代码**:
```csharp
controller.BreathModeOn();
```

---

### BreathModeOff

**描述**: 呼吸模式关闭

**示例代码**:
```csharp
controller.BreathModeOff();
```

---

### SwitchRed

**描述**: Red

**示例代码**:
```csharp
controller.SwitchRed();
```

---

### SwitchGreen

**描述**: Green

**示例代码**:
```csharp
controller.SwitchGreen();
```

---

### SwitchBlue

**描述**: Blue

**示例代码**:
```csharp
controller.SwitchBlue();
```

---

### AutoColorOn

**描述**: AutoColorOn

**示例代码**:
```csharp
controller.AutoColorOn();
```
