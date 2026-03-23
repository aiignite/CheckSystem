# F03RearLamp 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | F03RearLamp |
| 描述 | CAN-Product,F03尾灯 |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| CanFD | CanBus | CAN FD总线 |
| SecurityAccess0102Result | string | R,安全访问解锁0102结果 |
| ECUName | string | R,ECU名称读取[F197] |
| SeeYaoECUHwVer | string | R,信耀定义ECU硬件版本号[F193] |
| SeeYaoECUSwVer | string | R,信耀定义ECU软件版本号[F195] |
| BootLoaderVer | string | R,BootLoader软件版本号[F180] |
| FactoryECUSwVer | string | R,整车厂ECU软件版本号[F189] |
| BackupSwVer | string | R,备份软件版本号[F090] |
| FactoryECUHwVer | string | R,整车厂ECU硬件版本号[F089] |
| FactoryPartNo | string | R,整车厂定义零件号[F187] |
| SupplierCode | string | R,供应商代码[F18A] |
| Thumbprint | string | R,读取指纹信息[F184] |
| RunningAreaInfo | string | R,当前运行分区信息[F0F0] |
| UpdateTriedCounter | string | R,刷写尝试计数器[F0F1] |
| DependecyCheckSuccessCounter | string | R,编程依赖检查成功计数器[F0F3] |
| TraceCodeRes | string | R,写精确追溯码结果 |
| FactorySwPartNo | string | R,整车厂定义软件零件号[F013] |
| ECUProduceDataRes | string | R,写ECU生产日期结果 |
| ClearDtcResult | string | R,清除DTC结果 |
| ReadDtcResult | string | R,读取DTC结果 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| StartCAN | 无 | void | 打开CAN |
| StopCAN | 无 | void | 关闭CAN |
| LeftTiOnOff | value: int | void | 左后转向灯点亮关闭命令 |
| RightTiOnOff | value: int | void | 右后转向灯点亮关闭命令 |
| LeftTiFlow | value: int | void | 左后转向灯流水使能 |
| RightTiFlow | value: int | void | 右后转向灯流水使能 |
| LeftTiBrightSet | value: int | void | 左后转向灯亮度设置 |
| RightTiBrightSet | value: int | void | 右后转向灯亮度设置 |
| LeftStopOnOff | value: int | void | 左制动灯点亮关闭命令 |
| RightStopOnOff | value: int | void | 右制动灯点亮关闭命令 |
| MiddleLeftStopOnOff | value: int | void | 中左制动灯点亮关闭命令 |
| MiddleRightStopOnOff | value: int | void | 中右制动灯点亮关闭命令 |
| MiddleLeftBulOnOff | value: int | void | 中左倒车灯点亮关闭命令 |
| MiddleRightBulOnOff | value: int | void | 中右倒车灯点亮关闭命令 |
| RearADSOnOff | value: int | void | 后ADS灯点亮关闭命令 |
| RearADSBritSet | value: int | void | 后ADS灯亮度设置 |
| RearADSEffectCtrl | value: int | void | 后ADS灯灯语控制命令 |
| RearADSEffectSelect | value: int | void | 后ADS灯灯语效果选择 |
| RearADSEffectModeSet | value: int | void | 后ADS灯灯语效果模式设置 |
| RearRibbonEffectModeSet | value: int | void | 后贯穿灯灯语效果模式设置 |
| RearRibbonEffectCtrl | value: int | void | 后贯穿灯灯语效果控制命令 |
| RearRibbonEffectSelect | value: int | void | 后贯穿灯灯语效果选择 |
| LeftRearPosBritSet | value: int | void | 左后灯亮度设置 |
| RightRearPosBritSet | value: int | void | 右后灯亮度设置 |
| LeftRearPosFlow | value: int | void | 左后灯流水使能 |
| RightRearPosFlow | value: int | void | 右后灯流水使能 |
| LeftRearPosCtrl | value: int | void | 左后灯点亮关闭命令 |
| RightRearPosCtrl | value: int | void | 右后灯点亮关闭命令 |
| MidLeftRearPosFlow | value: int | void | 中左后灯流水使能 |
| MidRightRearPosFlow | value: int | void | 中右后灯流水使能 |
| MidLeftRearPosBritSet | value: int | void | 中左后灯亮度设置 |
| MidRightRearPosBritSet | value: int | void | 中右后灯亮度设置 |
| MidLeftRearPosCtrl | value: int | void | 中左后灯点亮关闭命令 |
| MidRightRearPosCtrl | value: int | void | 中右后灯点亮关闭命令 |
| TailGateCtrl | value: int | void | 尾门信号 |
| ADS_L3 | value: int | void | ADS_L3_使能 |
| RearLogoCtrl | value: int | void | LOGO使能 |
| VIU_UsageMode | value: int | void | VIU_UsageMode |
| DebugAnimationBegin | value: int | void | 测试动画begin |
| DebugAnimationEnd | 无 | void | 测试动画end |
| EnterExtendMode | 无 | void | 进入拓展模式 |
| EnterNormalMode | 无 | void | 进入正常模式 |
| SecurityAccess0102 | 无 | void | 安全访问解锁0102 |
| ReadEcuName | 无 | void | ECU名称读取[F197] |
| ReadSeeYaoECUHwVer | 无 | void | 信耀定义ECU硬件版本号[F193] |
| ReadSeeYaoECUSwVer | 无 | void | 信耀定义ECU软件版本号[F195] |
| ReadBootLoaderVer | 无 | void | BootLoader软件版本号[F180] |
| ReadFactoryECUSwVer | 无 | void | 整车厂ECU软件版本号[F189] |
| ReadFactorySwPartNo | 无 | void | 整车厂定义软件零件号[F013] |
| ReadBackupSwVer | 无 | void | 备份软件版本号[F090] |
| ReadFactoryECUHwVer | 无 | void | 整车厂ECU硬件版本号[F089] |
| ReadFactoryPartNo | 无 | void | 整车厂定义零件号[F187] |
| ReadSupplierCode | 无 | void | 供应商代码[F18A] |
| ReadThumbprint | 无 | void | 读取指纹信息[F184] |
| ReadRunningAreaInfo | 无 | void | 当前运行分区信息[F0F0] |
| ReadUpdateTriedCounter | 无 | void | 刷写尝试计数器[F0F1] |
| ReadDependecyCheckSuccessCounter | 无 | void | 刷写尝试计数器[F0F3] |
| WriteCode | prNo: string, partNo: string, supplierCode: string, lineNo: int, factorySwPn: string | void | 写追溯信息 |
| WriteCodeByBarcodeCode | partNo: string, supplyerCode: string, codeLen: int, gse: string, factorySwPn: string, simplePartNo: string | void | 通过二维码写追溯信息 |
| VersionCheck | prNo: string, partNo: string, supplierCode: string, lineNo: int, factorySwPn: string | void | 信息验证 |
| ClearDtc | 无 | void | 清除DTC |
| ReadDtc | 无 | void | 读取DTC |

## 方法详细说明

### StartCAN

**描述**: 打开CAN

**示例代码**:
```csharp
controller.StartCAN();
```

---

### StopCAN

**描述**: 关闭CAN

**示例代码**:
```csharp
controller.StopCAN();
```

---

### LeftTiOnOff

**描述**: 左后转向灯点亮关闭命令

**参数**:
- `value` (int): 命令值 (0-3)

**示例代码**:
```csharp
controller.LeftTiOnOff(2);
```

---

### RightTiOnOff

**描述**: 右后转向灯点亮关闭命令

**参数**:
- `value` (int): 命令值 (0-3)

**示例代码**:
```csharp
controller.RightTiOnOff(2);
```

---

### LeftTiFlow

**描述**: 左后转向灯流水使能

**参数**:
- `value` (int): 使能值 (0-7)

**示例代码**:
```csharp
controller.LeftTiFlow(1);
```

---

### RightTiFlow

**描述**: 右后转向灯流水使能

**参数**:
- `value` (int): 使能值 (0-7)

**示例代码**:
```csharp
controller.RightTiFlow(1);
```

---

### LeftTiBrightSet

**描述**: 左后转向灯亮度设置

**参数**:
- `value` (int): 亮度值 (0-127)

**示例代码**:
```csharp
controller.LeftTiBrightSet(100);
```

---

### LeftStopOnOff

**描述**: 左制动灯点亮关闭命令

**参数**:
- `value` (int): 命令值 (0-3)

**示例代码**:
```csharp
controller.LeftStopOnOff(2);
```

---

### RearADSOnOff

**描述**: 后ADS灯点亮关闭命令

**参数**:
- `value` (int): 命令值 (0-3)

**示例代码**:
```csharp
controller.RearADSOnOff(2);
```

---

### RearADSBritSet

**描述**: 后ADS灯亮度设置

**参数**:
- `value` (int): 亮度值 (0-127)

**示例代码**:
```csharp
controller.RearADSBritSet(100);
```

---

### LeftRearPosBritSet

**描述**: 左后灯亮度设置

**参数**:
- `value` (int): 亮度值 (0-127)

**示例代码**:
```csharp
controller.LeftRearPosBritSet(100);
```

---

### TailGateCtrl

**描述**: 尾门信号

**参数**:
- `value` (int): 信号值 (0-15)

**示例代码**:
```csharp
controller.TailGateCtrl(1);
```

---

### RearLogoCtrl

**描述**: LOGO使能

**参数**:
- `value` (int): 使能值 (0-3)

**示例代码**:
```csharp
controller.RearLogoCtrl(1);
```

---

### DebugAnimationBegin

**描述**: 测试动画begin

**参数**:
- `value` (int): 动画值 (0x01, 0x08, 0x07, 0x14)

**示例代码**:
```csharp
controller.DebugAnimationBegin(0x01);
```

---

### DebugAnimationEnd

**描述**: 测试动画end

**示例代码**:
```csharp
controller.DebugAnimationEnd();
```

---

### EnterExtendMode

**描述**: 进入拓展模式

**示例代码**:
```csharp
controller.EnterExtendMode();
```

---

### EnterNormalMode

**描述**: 进入正常模式

**示例代码**:
```csharp
controller.EnterNormalMode();
```

---

### SecurityAccess0102

**描述**: 安全访问解锁0102

**示例代码**:
```csharp
controller.SecurityAccess0102();
string result = controller.SecurityAccess0102Result;
```

---

### ReadEcuName

**描述**: ECU名称读取[F197]

**示例代码**:
```csharp
controller.ReadEcuName();
string name = controller.ECUName;
```

---

### ReadBootLoaderVer

**描述**: BootLoader软件版本号[F180]

**示例代码**:
```csharp
controller.ReadBootLoaderVer();
string ver = controller.BootLoaderVer;
```

---

### ReadFactoryPartNo

**描述**: 整车厂定义零件号[F187]

**示例代码**:
```csharp
controller.ReadFactoryPartNo();
string pn = controller.FactoryPartNo;
```

---

### WriteCode

**描述**: 写追溯信息

**参数**:
- `prNo` (string): PR编号
- `partNo` (string): 零件号
- `supplierCode` (string): 供应商代码
- `lineNo` (int): 生产线号
- `factorySwPn` (string): 整车厂软件零件号

**示例代码**:
```csharp
controller.WriteCode("PR4101001062", "0000", "VG350", 1, "8744AFK010");
string traceResult = controller.TraceCodeRes;
string ecuResult = controller.ECUProduceDataRes;
```

---

### WriteCodeByBarcodeCode

**描述**: 通过二维码写追溯信息

**参数**:
- `partNo` (string): 零件号
- `supplyerCode` (string): 供应商代码
- `codeLen` (int): 二维码长度
- `gse` (string): GSE
- `factorySwPn` (string): 整车厂软件零件号
- `simplePartNo` (string): 零件简号

**示例代码**:
```csharp
controller.WriteCodeByBarcodeCode("0000", "VG350", 34, "GSE", "8744AFK010", "0000");
```

---

### VersionCheck

**描述**: 信息验证

**参数**:
- `prNo` (string): PR编号
- `partNo` (string): 零件号
- `supplierCode` (string): 供应商代码
- `lineNo` (int): 生产线号
- `factorySwPn` (string): 整车厂软件零件号

**示例代码**:
```csharp
controller.VersionCheck("PR4101001062", "0000", "VG350", 1, "8744AFK010");
```

---

### ClearDtc

**描述**: 清除DTC

**示例代码**:
```csharp
controller.ClearDtc();
string result = controller.ClearDtcResult;
```

---

### ReadDtc

**描述**: 读取DTC

**示例代码**:
```csharp
controller.ReadDtc();
string result = controller.ReadDtcResult;
```
