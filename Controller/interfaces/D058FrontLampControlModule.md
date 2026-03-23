# D058FrontLampControlModule 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | D058FrontLampControlModule |
| 描述 | CAN-Product,D058高配前灯控制模块 |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| NodeStr | string | R,读取节点配置 |
| LrdfStr | string | R,读取节点配置 |
| BarcodeStr | string | R,条码 |
| HwPn | string | R,总成零件号 |
| BootAppSwPn | string | R,引导程序零件号 |
| BootAppSwVer | string | R,引导程序版本号 |
| AppSwPn | string | R,应用程序零件号 |
| CfgPn | string | R,配置文件零件号 |
| AppSwVer | string | R,应用程序版本号 |
| CfgVer | string | R,配置文件版本号 |
| ProductData | string | R,生产追溯信息 |
| Inductance1 | double | R,电感1读取值 |
| Inductance2 | double | R,电感2读取值 |
| Inductance3 | double | R,电感3读取值 |
| Inductance4 | double | R,电感4读取值 |
| Led1Current | double | 电感1电流 |
| Led2Current | double | 电感2电流 |
| Led3Current | double | 电感3电流 |
| Led4Current | double | 电感4电流 |
| Led1K | double | 电感1系数K |
| Led1B | double | 电感1系数B |
| Led2K | double | 电感2系数K |
| Led2B | double | 电感2系数B |
| Led3K | double | 电感3系数K |
| Led3B | double | 电感3系数B |
| Led4K | double | 电感4系数K |
| Led4B | double | 电感4系数B |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| EnterSleepMode | 无 | void | 模块休眠 |
| ExitSleepMode | 无 | void | 模块唤醒 |
| ChangeLeftNode | 无 | void | 配置为左节点 |
| ChangeRightNode | 无 | void | 配置为右节点 |
| LbOn | 无 | void | 近光开 |
| LbOff | 无 | void | 近光关 |
| HbOn | 无 | void | 远光开 |
| HbOff | 无 | void | 远光关 |
| TurnNormalOn | 无 | void | 转向普通开 |
| TurnWaterFlash | 无 | void | 转向跑马开 |
| TurnOff | 无 | void | 转向关 |
| DrlOn | 无 | void | 日行灯开 |
| DrlOff | 无 | void | 日行灯关 |
| PlOn | 无 | void | 位置灯开 |
| PlOff | 无 | void | 位置灯关 |
| CornerOn | 无 | void | 角灯开 |
| CornerOff | 无 | void | 角灯关 |
| AllLedOff | 无 | void | 关闭所有 |
| EnterExtendedSession | 无 | void | 进入拓展 |
| ExitExtendedSession | 无 | void | 退出拓展 |
| DisableDtc | 无 | void | 禁止故障诊断 |
| WriteNode | node: string | void | 节点配置 |
| ReadNode | 无 | void | 节点读取 |
| ReadBootAppSwPn | 无 | void | 读引导程序零件号 |
| ReadBootAppSwVer | 无 | void | 读引导程序版本号 |
| ReadHwPn | 无 | void | 读总成零件号 |
| ReadAppSwPn | 无 | void | 读应用程序零件号 |
| ReadCfgPn | 无 | void | 读配置文件零件号 |
| ReadAppSwVer | 无 | void | 读应用程序版本号 |
| ReadCfgVer | 无 | void | 配置文件版本号 |
| ReadProductData | 无 | void | 读生产追溯信息 |
| SetInductance | 无 | void | 设置电感参数 |
| ReadInductance | 无 | void | 读取电感 |

## 方法详细说明

### EnterSleepMode

**描述**: 模块休眠

**示例代码**:
```csharp
controller.EnterSleepMode();
```

---

### ExitSleepMode

**描述**: 模块唤醒

**示例代码**:
```csharp
controller.ExitSleepMode();
```

---

### LbOn

**描述**: 近光开

**示例代码**:
```csharp
controller.LbOn();
```

---

### LbOff

**描述**: 近光关

**示例代码**:
```csharp
controller.LbOff();
```

---

### HbOn

**描述**: 远光开

**示例代码**:
```csharp
controller.HbOn();
```

---

### HbOff

**描述**: 远光关

**示例代码**:
```csharp
controller.HbOff();
```

---

### TurnNormalOn

**描述**: 转向普通开

**示例代码**:
```csharp
controller.TurnNormalOn();
```

---

### TurnWaterFlash

**描述**: 转向跑马开

**示例代码**:
```csharp
controller.TurnWaterFlash();
```

---

### DrlOn

**描述**: 日行灯开

**示例代码**:
```csharp
controller.DrlOn();
```

---

### CornerOn

**描述**: 角灯开

**示例代码**:
```csharp
controller.CornerOn();
```

---

### AllLedOff

**描述**: 关闭所有

**示例代码**:
```csharp
controller.AllLedOff();
```

---

### EnterExtendedSession

**描述**: 进入拓展

**示例代码**:
```csharp
controller.EnterExtendedSession();
```

---

### ReadNode

**描述**: 节点读取

**示例代码**:
```csharp
controller.ReadNode();
string node = controller.NodeStr;
```

---

### WriteNode

**描述**: 节点配置

**参数**:
- `node` (string): 节点值（十六进制）

**示例代码**:
```csharp
controller.WriteNode("0A");
```

---

### ReadInductance

**描述**: 读取电感

**示例代码**:
```csharp
controller.ReadInductance();
double val = controller.Inductance1;
```
