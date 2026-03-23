# Es33DpFrontLamp 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | Es33DpFrontLamp |
| 描述 | LIN-Product,ES33-DP前灯 |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| Lin | LinBus | LIN总线 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| LampAwake | 无 | void | 唤醒 |
| LampSleep | 无 | void | 休眠 |
| HighBeamReq | 无 | void | HighBeamReq |
| AuxHighBeamReq | 无 | void | AuxHighBeamReq |
| DrlReq | 无 | void | DrlReq |
| PlReq | 无 | void | PlReq |
| DrlMode0 | 无 | void | 日间行车灯的点亮模式0 |
| DrlMode1 | 无 | void | 日间行车灯的点亮模式1 |
| DrlMode2 | 无 | void | 日间行车灯的点亮模式2 |
| DrlMode3 | 无 | void | 日间行车灯的点亮模式3 |
| DrlMode4 | 无 | void | 日间行车灯的点亮模式4 |
| DrlMode5 | 无 | void | 日间行车灯的点亮模式5 |
| DrlMode6 | 无 | void | 日间行车灯的点亮模式6 |
| DipdBeamReq | 无 | void | DipdBeamLghtOnReq |
| WlcmLightReq | 无 | void | WlcmLight |
| FrwlLightReq | 无 | void | FrwlLghtReq |
| ChrgLght | 无 | void | 充电灯-3块单白光有动画 |
| DebugSingal | 无 | void | 测试 |

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

### HighBeamReq

**描述**: HighBeamReq

**示例代码**:
```csharp
controller.HighBeamReq();
```

---

### AuxHighBeamReq

**描述**: AuxHighBeamReq

**示例代码**:
```csharp
controller.AuxHighBeamReq();
```

---

### DrlReq

**描述**: DrlReq

**示例代码**:
```csharp
controller.DrlReq();
```

---

### PlReq

**描述**: PlReq

**示例代码**:
```csharp
controller.PlReq();
```

---

### DrlMode0

**描述**: 日间行车灯的点亮模式0

**示例代码**:
```csharp
controller.DrlMode0();
```

---

### DrlMode1

**描述**: 日间行车灯的点亮模式1

**示例代码**:
```csharp
controller.DrlMode1();
```

---

### DrlMode2

**描述**: 日间行车灯的点亮模式2

**示例代码**:
```csharp
controller.DrlMode2();
```

---

### DrlMode3

**描述**: 日间行车灯的点亮模式3

**示例代码**:
```csharp
controller.DrlMode3();
```

---

### DrlMode4

**描述**: 日间行车灯的点亮模式4

**示例代码**:
```csharp
controller.DrlMode4();
```

---

### DrlMode5

**描述**: 日间行车灯的点亮模式5

**示例代码**:
```csharp
controller.DrlMode5();
```

---

### DrlMode6

**描述**: 日间行车灯的点亮模式6

**示例代码**:
```csharp
controller.DrlMode6();
```

---

### DipdBeamReq

**描述**: DipdBeamLghtOnReq

**示例代码**:
```csharp
controller.DipdBeamReq();
```

---

### WlcmLightReq

**描述**: WlcmLight

**示例代码**:
```csharp
controller.WlcmLightReq();
```

---

### FrwlLightReq

**描述**: FrwlLghtReq

**示例代码**:
```csharp
controller.FrwlLightReq();
```

---

### ChrgLght

**描述**: 充电灯-3块单白光有动画

**示例代码**:
```csharp
controller.ChrgLght();
```

---

### DebugSingal

**描述**: 测试

**示例代码**:
```csharp
controller.DebugSingal();
```
