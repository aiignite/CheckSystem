# E2LbGpFrontLamp 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | E2LbGpFrontLamp |
| 描述 | CAN-Product,E2LB前灯高配-P00133228 |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| Can | CanBus | CAN总线 |
| ModeStr | string | R,读取模式配置 |
| LdmConfig | string | R,LDMConfig |
| LrdfStr | string | R,读取节点配置 |
| HwPn | string | R,总成零件号 |
| BootAppSwPn | string | R,引导程序零件号 |
| BootAppSwVer | string | R,引导程序版本号 |
| AppSwPn | string | R,应用程序零件号 |
| CfgPn | string | R,配置文件零件号 |
| AppSwVer | string | R,应用程序版本号 |
| CfgVer | string | R,配置文件版本号 |
| PlatformPartsInformation | string | R,平台零件信息 |
| ProductiontInfo | string | R,生产追溯信息 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| WriteNormalMode | 无 | void | 配置为普通模式 |
| WriteTestMode | 无 | void | 配置为测试模式 |
| EnterExtendedSession | 无 | void | 进入拓展 |
| ExitExtendedSession | 无 | void | 退出拓展 |
| ReadCurrMode | 无 | string | 读取模式配置 |
| ReadLdmConfig | 无 | void | 读取LDMConfig |
| ReadLrdf | 无 | void | 读取节点配置 |
| SetLeftNode | 无 | void | 配置为左节点 |
| SetRightNode | 无 | void | 配置为右节点 |
| ReadHwPn | 无 | void | 读总成零件号 |
| ReadBootAppSwPn | 无 | void | 读引导程序零件号 |
| ReadBootAppSwVer | 无 | void | 读引导程序版本号 |
| ReadAppSwPn | 无 | void | 读应用程序零件号 |
| ReadCfgPn | 无 | void | 读配置文件零件号 |
| ReadAppSwVer | 无 | void | 读应用程序版本号 |
| ReadCfgVer | 无 | void | 配置文件版本号 |
| ReadPlatformPartsInformation | 无 | void | 读取平台零件信息 |
| ReadProductiontInfo | 无 | void | 读取生产追溯信息 |
| SecurityAccess | 无 | void | 安全访问 |
| LampAwake | 无 | void | 唤醒 |
| LampSleep | 无 | void | 休眠 |
| LftBndOn | 无 | void | 打开左角灯 |
| LefBndOff | 无 | void | 关闭左角灯 |
| RghtBndCmdOn | 无 | void | 打开右角灯 |
| RghtBndCmdOff | 无 | void | 关闭右角灯 |
| DebugIoControl | 无 | void | 测试单颗LB |

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

### SetLeftNode

**描述**: 配置为左节点

**示例代码**:
```csharp
controller.SetLeftNode();
```

---

### SetRightNode

**描述**: 配置为右节点

**示例代码**:
```csharp
controller.SetRightNode();
```

---

### LftBndOn

**描述**: 打开左角灯

**示例代码**:
```csharp
controller.LftBndOn();
```

---

### LefBndOff

**描述**: 关闭左角灯

**示例代码**:
```csharp
controller.LefBndOff();
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

---

### SecurityAccess

**描述**: 安全访问

**示例代码**:
```csharp
controller.SecurityAccess();
```

---

### DebugIoControl

**描述**: 测试单颗LB

**示例代码**:
```csharp
controller.DebugIoControl();
```
