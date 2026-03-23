# As33FrontLamp 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | As33FrontLamp |
| 描述 | LIN-Product,AS33前灯 |
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
| DrlReq | 无 | void | DrlReq |
| PlReq | 无 | void | PlReq |
| DrlMode1 | 无 | void | 日间行车灯的点亮模式0 |
| DrlMode2 | 无 | void | 日间行车灯的点亮模式1 |
| DrlMode3 | 无 | void | 日间行车灯的点亮模式2 |
| DrlMode4 | 无 | void | 日间行车灯的点亮模式3 |
| DrlMode5 | 无 | void | 日间行车灯的点亮模式4 |
| DrlMode6 | 无 | void | 日间行车灯的点亮模式5 |
| Dm7 | 无 | void | 日间行车灯的点亮模式6 |
| WlcReq | 无 | void | Welcome Light Request迎宾请求 |
| WlcMode1 | 无 | void | 迎宾模式关 |
| WlcMode2 | 无 | void | 迎宾模式开 |
| Chg | 无 | void | 充电灯 |
| Fwr | 无 | void | Farewell Light Request欢送请求 |
| Turn | 无 | void | 开关Turn |
| SetSingal | name: string, value: string | void | 改信号 |

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

### DrlMode1

**描述**: 日间行车灯的点亮模式0

**示例代码**:
```csharp
controller.DrlMode1();
```

---

### DrlMode2

**描述**: 日间行车灯的点亮模式1

**示例代码**:
```csharp
controller.DrlMode2();
```

---

### DrlMode3

**描述**: 日间行车灯的点亮模式2

**示例代码**:
```csharp
controller.DrlMode3();
```

---

### DrlMode4

**描述**: 日间行车灯的点亮模式3

**示例代码**:
```csharp
controller.DrlMode4();
```

---

### DrlMode5

**描述**: 日间行车灯的点亮模式4

**示例代码**:
```csharp
controller.DrlMode5();
```

---

### DrlMode6

**描述**: 日间行车灯的点亮模式5

**示例代码**:
```csharp
controller.DrlMode6();
```

---

### Dm7

**描述**: 日间行车灯的点亮模式6

**示例代码**:
```csharp
controller.Dm7();
```

---

### WlcReq

**描述**: Welcome Light Request迎宾请求

**示例代码**:
```csharp
controller.WlcReq();
```

---

### WlcMode1

**描述**: 迎宾模式关

**示例代码**:
```csharp
controller.WlcMode1();
```

---

### WlcMode2

**描述**: 迎宾模式开

**示例代码**:
```csharp
controller.WlcMode2();
```

---

### Chg

**描述**: 充电灯

**示例代码**:
```csharp
controller.Chg();
```

---

### Fwr

**描述**: Farewell Light Request欢送请求

**示例代码**:
```csharp
controller.Fwr();
```

---

### Turn

**描述**: 开关Turn

**示例代码**:
```csharp
controller.Turn();
```

---

### SetSingal

**描述**: 改信号

**参数**:
- `name` (string): 信号名称
- `value` (string): 信号值(十六进制)

**示例代码**:
```csharp
controller.SetSingal("MainBeamLghtOnReq_lL5_BCM_LIN5", "01");
```
