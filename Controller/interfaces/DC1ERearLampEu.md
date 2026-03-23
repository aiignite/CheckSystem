# DC1ERearLampEu 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | DC1ERearLampEu |
| 描述 | CAN-Product,DC1E后灯-海外版 |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| Can | CanBus | CAN总线 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| LampAwake | 无 | void | 控制版唤醒 |
| LampSleep | 无 | void | 控制板休眠 |
| StopOn | 无 | void | 制动灯ON |
| StopOff | 无 | void | 制动灯OFF |
| TailOn | 无 | void | 位置灯ON |
| TailOff | 无 | void | 位置灯OFF |
| TailHdOn | 无 | void | 位置灯高亮ON |
| TailHdOff | 无 | void | 位置灯高亮OFF |
| BulOn | 无 | void | 倒车灯ON |
| BulOff | 无 | void | 倒车灯OFF |
| FogOn | 无 | void | 雾灯ON |
| FogOff | 无 | void | 雾灯OFF |
| LogoOn | 无 | void | LOGO灯ON |
| LogoOff | 无 | void | LOGO灯OFF |
| WelcomeAnimationOn | 无 | void | 回家动画ON |
| WelcomeAnimationOff | 无 | void | 回家动画OFF |
| GoodByeAnimationOn | 无 | void | 离家动画ON |
| GoodByeAnimationOff | 无 | void | 离家动画OFF |

## 方法详细说明

### LampAwake

**描述**: 控制版唤醒

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

### StopOn

**描述**: 制动灯ON

**示例代码**:
```csharp
controller.StopOn();
```

---

### StopOff

**描述**: 制动灯OFF

**示例代码**:
```csharp
controller.StopOff();
```

---

### TailOn

**描述**: 位置灯ON

**示例代码**:
```csharp
controller.TailOn();
```

---

### TailOff

**描述**: 位置灯OFF

**示例代码**:
```csharp
controller.TailOff();
```

---

### TailHdOn

**描述**: 位置灯高亮ON

**示例代码**:
```csharp
controller.TailHdOn();
```

---

### TailHdOff

**描述**: 位置灯高亮OFF

**示例代码**:
```csharp
controller.TailHdOff();
```

---

### BulOn

**描述**: 倒车灯ON

**示例代码**:
```csharp
controller.BulOn();
```

---

### BulOff

**描述**: 倒车灯OFF

**示例代码**:
```csharp
controller.BulOff();
```

---

### FogOn

**描述**: 雾灯ON

**示例代码**:
```csharp
controller.FogOn();
```

---

### FogOff

**描述**: 雾灯OFF

**示例代码**:
```csharp
controller.FogOff();
```

---

### LogoOn

**描述**: LOGO灯ON

**示例代码**:
```csharp
controller.LogoOn();
```

---

### LogoOff

**描述**: LOGO灯OFF

**示例代码**:
```csharp
controller.LogoOff();
```

---

### WelcomeAnimationOn

**描述**: 回家动画ON

**示例代码**:
```csharp
controller.WelcomeAnimationOn();
```

---

### WelcomeAnimationOff

**描述**: 回家动画OFF

**示例代码**:
```csharp
controller.WelcomeAnimationOff();
```

---

### GoodByeAnimationOn

**描述**: 离家动画ON

**示例代码**:
```csharp
controller.GoodByeAnimationOn();
```

---

### GoodByeAnimationOff

**描述**: 离家动画OFF

**示例代码**:
```csharp
controller.GoodByeAnimationOff();
```