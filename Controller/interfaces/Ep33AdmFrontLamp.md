# Ep33AdmFrontLamp 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | Ep33AdmFrontLamp |
| 描述 | CAN-Product,P00141531_EP33辅助远近光大灯 |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| Can | CanBus | CAN总线 |
| ModeStr | string | R,读取模式配置 |
| LrdfStr | string | R,读取节点配置 |
| HbHwCmdSts | string | R,读取远光备份线状态 |
| LbHwCmdSts | string | R,读取近光备份线状态 |
| HwPn | string | R,总成零件号 |
| BootAppSwPn | string | R,引导程序零件号 |
| BootAppSwVer | string | R,引导程序版本号 |
| AppSwPn | string | R,应用程序零件号 |
| CfgPn | string | R,配置文件零件号 |
| AppSwVer | string | R,应用程序版本号 |
| CfgVer | string | R,配置文件版本号 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| LampAwake | 无 | void | 唤醒 |
| LampSleep | 无 | void | 休眠 |
| LbOn | 无 | void | 打开辅近光 |
| LbOff | 无 | void | 关闭辅近光 |
| HbOn | index: string | void | 打开单颗辅远光 |
| HbOff | index: string | void | 关闭单颗辅远光 |
| WriteNormalMode | 无 | void | 配置为普通模式 |
| WriteTestMode | 无 | void | 配置为测试模式 |
| EnterExtendedSession | 无 | void | 进入拓展 |
| ExitExtendedSession | 无 | void | 退出拓展 |
| ReadCurrMode | 无 | string | 读取模式配置 |
| ReadLrdf | 无 | void | 读取节点配置 |
| ReadHwCmdSts | 无 | void | 备份线状态检测 |
| ReadHwPn | 无 | void | 读总成零件号 |
| ReadBootAppSwPn | 无 | void | 读引导程序零件号 |
| ReadBootAppSwVer | 无 | void | 读引导程序版本号 |
| ReadAppSwPn | 无 | void | 读应用程序零件号 |
| ReadCfgPn | 无 | void | 读配置文件零件号 |
| ReadAppSwVer | 无 | void | 读应用程序版本号 |
| ReadCfgVer | 无 | void | 配置文件版本号 |

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

### LbOn

**描述**: 打开辅近光

**示例代码**:
```csharp
controller.LbOn();
```

---

### LbOff

**描述**: 关闭辅近光

**示例代码**:
```csharp
controller.LbOff();
```

---

### HbOn

**描述**: 打开单颗辅远光

**参数**:
- `index` (string): 远光索引（"1"-"5"）

**示例代码**:
```csharp
controller.HbOn("1");
```

---

### HbOff

**描述**: 关闭单颗辅远光

**参数**:
- `index` (string): 远光索引（"1"-"5"）

**示例代码**:
```csharp
controller.HbOff("1");
```

---

### WriteNormalMode

**描述**: 配置为普通模式

**示例代码**:
```csharp
controller.WriteNormalMode();
```

---

### WriteTestMode

**描述**: 配置为测试模式

**示例代码**:
```csharp
controller.WriteTestMode();
```

---

### EnterExtendedSession

**描述**: 进入拓展

**示例代码**:
```csharp
controller.EnterExtendedSession();
```

---

### ExitExtendedSession

**描述**: 退出拓展

**示例代码**:
```csharp
controller.ExitExtendedSession();
```

---

### ReadCurrMode

**描述**: 读取模式配置

**返回类型**: string

**示例代码**:
```csharp
string mode = controller.ReadCurrMode();
```

---

### ReadLrdf

**描述**: 读取节点配置

**示例代码**:
```csharp
controller.ReadLrdf();
string config = controller.LrdfStr;
```

---

### ReadHwPn

**描述**: 读总成零件号

**示例代码**:
```csharp
controller.ReadHwPn();
string pn = controller.HwPn;
```

---

### ReadAppSwVer

**描述**: 读应用程序版本号

**示例代码**:
```csharp
controller.ReadAppSwVer();
string ver = controller.AppSwVer;
```
