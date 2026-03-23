# Ep33DpFrontLamp 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | Ep33DpFrontLamp |
| 描述 | LIN-Product,EP33-DP前灯 |
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
| WlcReq | 无 | void | Welcome Light Request迎宾请求 |
| WlcMode1 | 无 | void | 迎宾模式关 |
| WlcMode2 | 无 | void | 迎宾模式开 |
| Fwr | 无 | void | Farewell Light Request欢送请求 |
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

### WlcReq

**描述**: Welcome Light Request迎宾请求

**示例代码**:
```csharp
controller.WlcReq();
```

---

### Fwr

**描述**: Farewell Light Request欢送请求

**示例代码**:
```csharp
controller.Fwr();
```

---

### SetSingal

**描述**: 改信号

**参数**:
- `name` (string): 信号名称
- `value` (string): 信号值（十六进制）

**示例代码**:
```csharp
controller.SetSingal("MainBeamLghtOnReq_lL5_BCM_LIN5", "01");
```
