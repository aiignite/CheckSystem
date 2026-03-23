# A12FrontLampControlModule 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | A12FrontLampControlModule |
| 描述 | CAN-Product,A12前灯控制模块 |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| Can | CanBus | CAN总线 |
| ModeStr | string | R,读取模式配置 |
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
| SwThumbPrint | string | R,软件指纹 |
| ProgramDate | string | R,编程日期 |
| ProductionLineNumber | string | R/W,生产追溯信息-生产线编号 |
| ProductNo | string | R/W,ProductNo |
| GacBootAppSwVer | string | R,广汽引导程序版本号 |
| GacAppSwVer | string | R,应用程序版本号 |
| GacCfgVer | string | R,配置文件版本号 |
| RBin1Volt | double | R,RBIN_1采样电压 |
| RBin2Volt | double | R,RBIN_2采样电压 |
| RBin3Volt | double | R,RBIN_3采样电压 |
| RBin4Volt | double | R,RBIN_4采样电压 |
| RBin5Volt | double | R,RBIN_5采样电压 |
| RBin6Volt | double | R,RBIN_6采样电压 |
| Ntc1Volt | double | R,NTC_1采样电压 |
| Ntc3Volt | double | R,NTC_3采样电压 |
| Ntc4Volt | double | R,NTC_4采样电压 |
| Ntc5Volt | double | R,NTC_5采样电压 |
| Inductance1 | double | R,电感1读取值 |
| Inductance2 | double | R,电感2读取值 |
| Inductance3 | double | R,电感3读取值 |
| Inductance4 | double | R,电感4读取值 |
| Inductance5 | double | R,电感5读取值 |
| Inductance6 | double | R,电感6读取值 |
| DownloadResult | string | R,下载结果 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| SwitchRight | 无 | void | 切换成右节点 |
| SwitchLeft | 无 | void | 切换成左节点 |
| EnterSleepMode | 无 | void | 模块休眠 |
| ExitSleepMode | 无 | void | 模块唤醒 |
| WriteNormalMode | 无 | void | 配置为普通模式 |
| WriteTestMode | 无 | void | 配置为测试模式 |
| EnterExtendedSession | 无 | void | 进入拓展 |
| ExitExtendedSession | 无 | void | 退出拓展 |
| WriteFactoryState | 无 | void | 配置成工厂状态 |
| AllControlOn | 无 | void | 测试模式下打开所有LED |
| ReadCurrMode | 无 | string | 读取模式配置 |
| ReadLrdf | 无 | string | 读取节点配置 |
| HighBmOn | 无 | void | 打开远光HB |
| HighBmOff | 无 | void | 关闭远光HB |
| PlOn | 无 | void | 打开PL |
| PlOff | 无 | void | 关闭PL |
| DrlOn | 无 | void | 打开DRL |
| DrlOff | 无 | void | 关闭DRL |
| LftLowBmOn | 无 | void | 打开左近光L_LB |
| LftLowBmOff | 无 | void | 关闭左近光L_LB |
| LftTrnOn | 无 | void | 打开左转向L_TURN |
| LftTrnOff | 无 | void | 关闭左转向L_TURN |
| RightTrnOn | 无 | void | 打开右转向R_TURN |
| RightTrnOff | 无 | void | 关闭右转向R_TURN |
| ReadBootAppSwPn | 无 | void | 读引导程序零件号 |
| ReadBootAppSwVer | 无 | void | 读引导程序版本号 |
| ReadHwPn | 无 | void | 读总成零件号 |
| ReadAppSwPn | 无 | void | 读应用程序零件号 |
| ReadCfgPn | 无 | void | 读配置文件零件号 |
| ReadAppSwVer | 无 | void | 读应用程序版本号 |
| ReadCfgVer | 无 | void | 配置文件版本号 |
| ReadProductData | 无 | void | 读生产追溯信息 |
| ReadProgramDate | 无 | void | 读编程日期 |
| ReadSwThumbPrint | 无 | void | 读软件指纹 |
| ReadGacBootAppSwVer | 无 | void | 读广汽版本引导程序版本号 |
| ReadGacAppSwVer | 无 | void | 读广汽版本应用程序版本号 |
| ReadGacCfgVer | 无 | void | 读广汽版本配置文件版本号 |
| ReadRBinVolt | index: int | void | 读RBIN采样电压 |
| ReadNtcVolt | index: int | void | 读NTC采样电压 |
| SetInductance | 无 | void | 设置电感 |
| ReadInductance | 无 | void | 读取电感 |
| DownLoadFile | 无 | void | 下载 |

## 方法详细说明

### SwitchRight

**描述**: 切换成右节点

**示例代码**:
```csharp
controller.SwitchRight();
```

---

### SwitchLeft

**描述**: 切换成左节点

**示例代码**:
```csharp
controller.SwitchLeft();
```

---

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

### WriteFactoryState

**描述**: 配置成工厂状态

**示例代码**:
```csharp
controller.WriteFactoryState();
```

---

### AllControlOn

**描述**: 测试模式下打开所有LED

**示例代码**:
```csharp
controller.AllControlOn();
```

---

### ReadCurrMode

**描述**: 读取模式配置

**示例代码**:
```csharp
string mode = controller.ReadCurrMode();
```

---

### ReadLrdf

**描述**: 读取节点配置

**示例代码**:
```csharp
string config = controller.ReadLrdf();
```

---

### HighBmOn

**描述**: 打开远光HB

**示例代码**:
```csharp
controller.HighBmOn();
```

---

### HighBmOff

**描述**: 关闭远光HB

**示例代码**:
```csharp
controller.HighBmOff();
```

---

### PlOn

**描述**: 打开PL

**示例代码**:
```csharp
controller.PlOn();
```

---

### PlOff

**描述**: 关闭PL

**示例代码**:
```csharp
controller.PlOff();
```

---

### DrlOn

**描述**: 打开DRL

**示例代码**:
```csharp
controller.DrlOn();
```

---

### DrlOff

**描述**: 关闭DRL

**示例代码**:
```csharp
controller.DrlOff();
```

---

### LftLowBmOn

**描述**: 打开左近光L_LB

**示例代码**:
```csharp
controller.LftLowBmOn();
```

---

### LftLowBmOff

**描述**: 关闭左近光L_LB

**示例代码**:
```csharp
controller.LftLowBmOff();
```

---

### LftTrnOn

**描述**: 打开左转向L_TURN

**示例代码**:
```csharp
controller.LftTrnOn();
```

---

### LftTrnOff

**描述**: 关闭左转向L_TURN

**示例代码**:
```csharp
controller.LftTrnOff();
```

---

### RightTrnOn

**描述**: 打开右转向R_TURN

**示例代码**:
```csharp
controller.RightTrnOn();
```

---

### RightTrnOff

**描述**: 关闭右转向R_TURN

**示例代码**:
```csharp
controller.RightTrnOff();
```

---

### ReadBootAppSwPn

**描述**: 读引导程序零件号

**示例代码**:
```csharp
controller.ReadBootAppSwPn();
string pn = controller.BootAppSwPn;
```

---

### ReadBootAppSwVer

**描述**: 读引导程序版本号

**示例代码**:
```csharp
controller.ReadBootAppSwVer();
string ver = controller.BootAppSwVer;
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

### ReadAppSwPn

**描述**: 读应用程序零件号

**示例代码**:
```csharp
controller.ReadAppSwPn();
string pn = controller.AppSwPn;
```

---

### ReadCfgPn

**描述**: 读配置文件零件号

**示例代码**:
```csharp
controller.ReadCfgPn();
string pn = controller.CfgPn;
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

### ReadCfgVer

**描述**: 配置文件版本号

**示例代码**:
```csharp
controller.ReadCfgVer();
string ver = controller.CfgVer;
```

---

### ReadProductData

**描述**: 读生产追溯信息

**示例代码**:
```csharp
controller.ReadProductData();
string data = controller.ProductData;
```

---

### ReadProgramDate

**描述**: 读编程日期

**示例代码**:
```csharp
controller.ReadProgramDate();
string date = controller.ProgramDate;
```

---

### ReadSwThumbPrint

**描述**: 读软件指纹

**示例代码**:
```csharp
controller.ReadSwThumbPrint();
string print = controller.SwThumbPrint;
```

---

### ReadGacBootAppSwVer

**描述**: 读广汽版本引导程序版本号

**示例代码**:
```csharp
controller.ReadGacBootAppSwVer();
string ver = controller.GacBootAppSwVer;
```

---

### ReadGacAppSwVer

**描述**: 读广汽版本应用程序版本号

**示例代码**:
```csharp
controller.ReadGacAppSwVer();
string ver = controller.GacAppSwVer;
```

---

### ReadGacCfgVer

**描述**: 读广汽版本配置文件版本号

**示例代码**:
```csharp
controller.ReadGacCfgVer();
string ver = controller.GacCfgVer;
```

---

### ReadRBinVolt

**描述**: 读RBIN采样电压

**参数**:
- `index` (int): 索引 (1-6)

**示例代码**:
```csharp
controller.ReadRBinVolt(1);
double volt = controller.RBin1Volt;
```

---

### ReadNtcVolt

**描述**: 读NTC采样电压

**参数**:
- `index` (int): 索引 (1-4)

**示例代码**:
```csharp
controller.ReadNtcVolt(1);
double volt = controller.Ntc1Volt;
```

---

### SetInductance

**描述**: 设置电感

**示例代码**:
```csharp
controller.SetInductance();
```

---

### ReadInductance

**描述**: 读取电感

**示例代码**:
```csharp
controller.ReadInductance();
double ind1 = controller.Inductance1;
```

---

### DownLoadFile

**描述**: 下载

**示例代码**:
```csharp
controller.DownLoadFile();
string result = controller.DownloadResult;
```
