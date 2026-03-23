# Cd764DpFrontLamp 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | Cd764DpFrontLamp |
| 描述 | LIN-Product,CD764低配前灯-波特率10.417 |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| Lin | LinBus | LIN总线 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| ModuleAwake | 无 | void | 模块唤醒 |
| ModuleSleep | 无 | void | 模块休眠 |
| HightBeamOn | 无 | void | 远光打开 |
| HighBeamOff | 无 | void | 远光关闭 |
| AnimationOn | 无 | void | 动画开 |
| AnimationOff | 无 | void | 动画关 |

## 方法详细说明

### ModuleAwake

**描述**: 模块唤醒

**示例代码**:
```csharp
controller.ModuleAwake();
```

---

### ModuleSleep

**描述**: 模块休眠

**示例代码**:
```csharp
controller.ModuleSleep();
```

---

### HightBeamOn

**描述**: 远光打开

**示例代码**:
```csharp
controller.HightBeamOn();
```

---

### HighBeamOff

**描述**: 远光关闭

**示例代码**:
```csharp
controller.HighBeamOff();
```

---

### AnimationOn

**描述**: 动画开

**示例代码**:
```csharp
controller.AnimationOn();
```

---

### AnimationOff

**描述**: 动画关

**示例代码**:
```csharp
controller.AnimationOff();
```
