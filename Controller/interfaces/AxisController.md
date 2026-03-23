# AxisController 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | AxisController |
| 描述 | 轴控制器 |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| RtexController | RtexControllerBase | RTEX控制器 |
| AxisCurrentPos | float | 当前轴位置 |
| IsAxisServoOn | bool | 轴伺服是否打开 |
| IsHoming | bool | 是否正在回原点 |
| IsAxisReady | bool | 轴是否就绪 |
| IsAxisBusy | bool | 轴是否忙碌 |
| IsMoveDone | bool | 运动是否完成 |
| SpeedPercent | float | 速度百分比 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| InitAxisIndex | axisNo: string | void | 初始化轴索引 |
| SetSpeed | speed: string | void | 设置当前速度 |
| AxisMoveTo | pos: string | void | 运动到目标位置 |
| AxisMoveRelativeTo | pos: string | void | 根据当前位置运动相对距离 |
| ServoOn | 无 | void | 使能打开 |
| ServoOff | 无 | void | 使能关闭 |
| AxisHome | 无 | void | 回到原点 |
| AddPrePos | value: string | void | 添加一个预存点位 |
| RemovePrePos | name: string | void | 移除一个预存点位 |
| ClearPrePos | 无 | void | 清除所有预存点位 |
| RunToPrePos | name: string | void | 运动到预存点位 |

## 方法详细说明

### InitAxisIndex

**描述**: 初始化轴索引

**参数**:
- `axisNo` (string): 轴号

**示例代码**:
```csharp
controller.InitAxisIndex("0");
```

---

### SetSpeed

**描述**: 设置当前速度

**参数**:
- `speed` (string): 速度值

**示例代码**:
```csharp
controller.SetSpeed("100.5");
```

---

### AxisMoveTo

**描述**: 运动到目标位置

**参数**:
- `pos` (string): 目标位置

**示例代码**:
```csharp
controller.AxisMoveTo("150.5");
bool done = controller.IsMoveDone;
```

---

### AxisMoveRelativeTo

**描述**: 根据当前位置运动相对距离

**参数**:
- `pos` (string): 相对距离

**示例代码**:
```csharp
controller.AxisMoveRelativeTo("10.5");
```

---

### ServoOn

**描述**: 使能打开

**示例代码**:
```csharp
controller.ServoOn();
```

---

### ServoOff

**描述**: 使能关闭

**示例代码**:
```csharp
controller.ServoOff();
```

---

### AxisHome

**描述**: 回到原点

**示例代码**:
```csharp
controller.AxisHome();
```

---

### AddPrePos

**描述**: 添加一个预存点位

**参数**:
- `value` (string): 点位信息 (格式: "Name:Speed=102.32/Pos=213.5")

**示例代码**:
```csharp
controller.AddPrePos("Pos1:Speed=50/Pos=100.5");
```

---

### RemovePrePos

**描述**: 移除一个预存点位

**参数**:
- `name` (string): 点位名称

**示例代码**:
```csharp
controller.RemovePrePos("Pos1");
```

---

### ClearPrePos

**描述**: 清除所有预存点位

**示例代码**:
```csharp
controller.ClearPrePos();
```

---

### RunToPrePos

**描述**: 运动到预存点位

**参数**:
- `name` (string): 点位名称

**示例代码**:
```csharp
controller.RunToPrePos("Pos1");
```
