# Ep33DpRearLamp 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | Ep33DpRearLamp |
| 描述 | LIN-Product,EP33低配组合后灯 |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| Lin | LinBus | LIN总线 |
| HascoSoftwareVersion | string | R,读取HASCO总软件版本号 |
| HascoAppVersion | string | R,读取HASCO应用程序软件版本号 |
| HascoAppVersionFlag | string | R,读取HASCO应用程序小软件版本号 |
| HascoAppPartNo | string | R,读取HASCO应用程序软件零件号 |
| HascoFblPartNo | string | R,读取HASCO引导程序软件零件号 |
| HascoCaliPartNo | string | R,读取HASCO标定程序软件零件号 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| LampAwake | 无 | void | 唤醒 |
| LampSleep | 无 | void | 休眠 |
| TailOn | 无 | void | 位置灯低亮ON |
| TailHsdOn | 无 | void | 位置灯高亮ON |
| TailOff | 无 | void | 位置灯OFF |
| WeimenOn | 无 | void | 尾门ON |
| WeimenOff | 无 | void | 尾门OFF |
| TurnOn | 无 | void | 转向灯正常点亮 |
| TurnRunningOn | 无 | void | 转向灯流水点亮 |
| Welcome | 无 | void | 欢迎动画 |
| Farewell | 无 | void | 欢送动画 |
| AbortWelcomeFarewell | 无 | void | 关闭动画 |
| setLeftRcla | 无 | void | 设置为Left RCLA |
| setRightRcla | 无 | void | 设置为RCLB |
| setRclb | 无 | void | 设置为Right RCLA |
| ReadHascoSoftwareVersion | nad: string | void | 读取HASCO总软件版本号 |
| ReadHascoAppVersion | 无 | void | 读取HASCO应用程序软件版本号 |
| ReadHascoAppVersionFlag | 无 | void | 读取HASCO应用程序小软件版本号 |
| ReadHascoAppPartNo | 无 | void | 读取HASCO应用程序软件零件号 |
| ReadHascoFblPartNo | 无 | void | 读取HASCO引导程序软件零件号 |
| ReadHascoCaliPartNo | 无 | void | 读取HASCO标定程序软件零件号 |
| WriteAppFlag | 无 | void | S11L解锁APP |

## 方法详细说明

### LampAwake

**描述**: 唤醒

**示例代码**:
```csharp
controller.LampAwake();
```

---

### LampSleep

**描述**: 休眠

**示例代码**:
```csharp
controller.LampSleep();
```

---

### TailOn

**描述**: 位置灯低亮ON

**示例代码**:
```csharp
controller.TailOn();
```

---

### TailHsdOn

**描述**: 位置灯高亮ON

**示例代码**:
```csharp
controller.TailHsdOn();
```

---

### TurnOn

**描述**: 转向灯正常点亮

**示例代码**:
```csharp
controller.TurnOn();
```

---

### TurnRunningOn

**描述**: 转向灯流水点亮

**示例代码**:
```csharp
controller.TurnRunningOn();
```

---

### Welcome

**描述**: 欢迎动画

**示例代码**:
```csharp
controller.Welcome();
```

---

### Farewell

**描述**: 欢送动画

**示例代码**:
```csharp
controller.Farewell();
```

---

### ReadHascoSoftwareVersion

**描述**: 读取HASCO总软件版本号

**参数**:
- `nad` (string): NAD值（十六进制）

**示例代码**:
```csharp
controller.ReadHascoSoftwareVersion("01");
string ver = controller.HascoSoftwareVersion;
```

---

### WriteAppFlag

**描述**: S11L解锁APP

**示例代码**:
```csharp
controller.WriteAppFlag();
```
