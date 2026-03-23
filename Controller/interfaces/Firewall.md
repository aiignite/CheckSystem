# Firewall 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | Firewall |
| 描述 | CAN-Product,防火墙 |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| DiagnosisCan | CanBus | CAN总线 |
| FblPartNo | string | R,引导程序零件号-01C0 |
| AppPartNo | string | R,应用程序零件号-01C1 |
| Cali1PartNo | string | R,配置文件1零件号-01C2 |
| FblVer | string | R,引导程序版本号-01D0 |
| AppVer | string | R,应用程序版本号-01D1 |
| Cali1Ver | string | R,配置文件1版本号-01D2 |
| EndModulePartNumber | string | R,当前零件号-EndModulePartNumber-01CB |
| EndModuleAlphaCode | string | R,当前版本号-当前版本号-01DB |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| StopCanMsg | 无 | void | 休眠 |
| StartCanMsg | 无 | void | 唤醒 |
| EnterDefaultSession | 无 | void | 进入正常模式 |
| EnterExtendedSession | 无 | void | 进入拓展模式 |
| EnterProgrammingSession | 无 | void | 进入编程模式 |
| SendBbin | 无 | void | 在BBIN网络上发送二次0x620帧 |
| WriteMtcAndEcuIdByBarcode | 无 | void | 根据二维码写入MTC和ECUID-F0B4和F0F3 |
| ReadFblPartNo | 无 | void | ReadF1C0-引导程序零件号 |
| ReadAppPartNo | 无 | void | Read01C1-应用程序零件号 |
| ReadCali1PartNo | 无 | void | Read01C2-配置文件1零件号 |
| ReadFblVer | 无 | void | Read01D0-引导程序版本号 |
| ReadAppVer | 无 | void | Read01D1-应用程序版本号 |
| ReadCali1Ver | 无 | void | Read01D2-配置文件1版本号 |
| ReadEndModulePartNumber | 无 | void | Read01CB-当前零件号-EndModulePartNumber |
| ReadEndModuleAlphaCode | 无 | void | Read01DB-当前版本号-EndModulePartNumber |

## 方法详细说明

### StopCanMsg

**描述**: 休眠

**示例代码**:
```csharp
controller.StopCanMsg();
```

---

### StartCanMsg

**描述**: 唤醒

**示例代码**:
```csharp
controller.StartCanMsg();
```

---

### EnterDefaultSession

**描述**: 进入正常模式

**示例代码**:
```csharp
controller.EnterDefaultSession();
```

---

### EnterExtendedSession

**描述**: 进入拓展模式

**示例代码**:
```csharp
controller.EnterExtendedSession();
```

---

### EnterProgrammingSession

**描述**: 进入编程模式

**示例代码**:
```csharp
controller.EnterProgrammingSession();
```

---

### SendBbin

**描述**: 在BBIN网络上发送二次0x620帧

**示例代码**:
```csharp
controller.SendBbin();
```

---

### WriteMtcAndEcuIdByBarcode

**描述**: 根据二维码写入MTC和ECUID-F0B4和F0F3

**示例代码**:
```csharp
controller.WriteMtcAndEcuIdByBarcode();
```

---

### ReadFblPartNo

**描述**: ReadF1C0-引导程序零件号

**示例代码**:
```csharp
controller.ReadFblPartNo();
string partNo = controller.FblPartNo;
```

---

### ReadAppPartNo

**描述**: Read01C1-应用程序零件号

**示例代码**:
```csharp
controller.ReadAppPartNo();
string partNo = controller.AppPartNo;
```

---

### ReadCali1PartNo

**描述**: Read01C2-配置文件1零件号

**示例代码**:
```csharp
controller.ReadCali1PartNo();
string partNo = controller.Cali1PartNo;
```

---

### ReadFblVer

**描述**: Read01D0-引导程序版本号

**示例代码**:
```csharp
controller.ReadFblVer();
string ver = controller.FblVer;
```

---

### ReadAppVer

**描述**: Read01D1-应用程序版本号

**示例代码**:
```csharp
controller.ReadAppVer();
string ver = controller.AppVer;
```

---

### ReadCali1Ver

**描述**: Read01D2-配置文件1版本号

**示例代码**:
```csharp
controller.ReadCali1Ver();
string ver = controller.Cali1Ver;
```

---

### ReadEndModulePartNumber

**描述**: Read01CB-当前零件号-EndModulePartNumber

**示例代码**:
```csharp
controller.ReadEndModulePartNumber();
string pn = controller.EndModulePartNumber;
```

---

### ReadEndModuleAlphaCode

**描述**: Read01DB-当前版本号-EndModulePartNumber

**示例代码**:
```csharp
controller.ReadEndModuleAlphaCode();
string code = controller.EndModuleAlphaCode;
```
