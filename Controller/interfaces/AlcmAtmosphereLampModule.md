# AlcmAtmosphereLampModule 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | AlcmAtmosphereLampModule |
| 描述 | LIN-Product,ALCM音乐氛围灯 |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| Lin19200 | LinBus | LIN总线19200 |
| LedLin | LinBus | LED LIN总线 |
| AmbtLgtZn1AnmIdxCmd | int | R/W,模式控制-音乐模式=5-非音乐模式=1 |
| AmbtLgtZn1ColrIdxCmd | int | R/W,颜色控制-颜色控制0~121 |
| AmmbtLgtZn1ColrIntstyCmd | int | R/W,亮度控制-亮度调节0~100 |
| CheckBcmLinMsgResult | string | R,BCM LIN消息检测结果 |
| AmbtLgtSoftwareVersionCmd | string | R,氛围灯软件版本命令 |
| CheckLedLinMsgResult | string | R,LED LIN消息检测结果 |
| AppVersion | string | R,App软件信息 |
| CalInternalVersion | string | R,CAL内部版本号 |
| PartNo | string | R,零件号 |
| InternalVersion | string | R,内部版本号 |
| VerF1F1 | string | R,版本号F1F1 |
| CalF1F7 | string | R,标定F1F7 |
| InternalFblVerFc03 | string | R,引导程序软件内部版本号FC03 |
| AppDownloadResult | string | R,App下载结果 |
| AppDownloadCostTime | float | R,APP下载耗时-秒 |
| CalDownloadResult | string | R,Cal下载结果 |
| CalDownloadCostTime | float | R,APP下载耗时-秒 |
| FlashDriverFilePath | string | R/W,FlashDrv文件路径 |
| AppFielPath | string | R/W,APP文件路径 |
| CalibrationFilePath | string | R/W,CAL文件路径 |
| RepeatCalDownloadResult | string | R,重复CAL下载测试结果 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| CheckBcmLinMsg | 无 | void | 检测BCM LIN消息 |
| CheckAmbtLgtSoftwareVersionCmd | 无 | void | 检测氛围灯软件版本命令 |
| CheckLedLinMsg | 无 | void | 检测LED LIN消息 |
| ModuleAwake | 无 | void | 模块唤醒 |
| ModuleSleep | 无 | void | 模块休眠 |
| ReadAppVersion | 无 | void | 读App软件信息 |
| ReadCalInternalVersion | 无 | void | 读cal内部版本号 |
| ReadPartNo | 无 | void | 读零件号 |
| ReadInternalVersion | 无 | void | 读内部版本号 |
| ReadAllVersionInfo | 无 | void | 读所有版本信息 |
| AppDownload | 无 | void | 刷新APP文件 |
| CaiDownload | 无 | void | 刷新Cal文件 |
| RepeatCalDownload | repeatCount: int | void | 重复CAL下载测试 |

## 方法详细说明

### CheckBcmLinMsg

**描述**: 检测BCM LIN消息

**示例代码**:
```csharp
controller.CheckBcmLinMsg();
string result = controller.CheckBcmLinMsgResult;
```

---

### CheckAmbtLgtSoftwareVersionCmd

**描述**: 检测氛围灯软件版本命令

**示例代码**:
```csharp
controller.CheckAmbtLgtSoftwareVersionCmd();
string ver = controller.AmbtLgtSoftwareVersionCmd;
```

---

### CheckLedLinMsg

**描述**: 检测LED LIN消息

**示例代码**:
```csharp
controller.CheckLedLinMsg();
string result = controller.CheckLedLinMsgResult;
```

---

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

### ReadAppVersion

**描述**: 读App软件信息

**示例代码**:
```csharp
controller.ReadAppVersion();
string ver = controller.AppVersion;
```

---

### ReadCalInternalVersion

**描述**: 读cal内部版本号

**示例代码**:
```csharp
controller.ReadCalInternalVersion();
string ver = controller.CalInternalVersion;
```

---

### ReadPartNo

**描述**: 读零件号

**示例代码**:
```csharp
controller.ReadPartNo();
string pn = controller.PartNo;
```

---

### ReadInternalVersion

**描述**: 读内部版本号

**示例代码**:
```csharp
controller.ReadInternalVersion();
string ver = controller.InternalVersion;
```

---

### ReadAllVersionInfo

**描述**: 读所有版本信息

**示例代码**:
```csharp
controller.ReadAllVersionInfo();
```

---

### AppDownload

**描述**: 刷新APP文件

**示例代码**:
```csharp
controller.AppDownload();
string result = controller.AppDownloadResult;
float time = controller.AppDownloadCostTime;
```

---

### CaiDownload

**描述**: 刷新Cal文件

**示例代码**:
```csharp
controller.CaiDownload();
string result = controller.CalDownloadResult;
float time = controller.CalDownloadCostTime;
```

---

### RepeatCalDownload

**描述**: 重复CAL下载测试

**参数**:
- `repeatCount` (int): 重复次数

**示例代码**:
```csharp
controller.RepeatCalDownload(10);
string result = controller.RepeatCalDownloadResult;
```
