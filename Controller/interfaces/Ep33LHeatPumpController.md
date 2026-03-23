# Ep33LHeatPumpController 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | Ep33LHeatPumpController |
| 描述 | LIN-Product,EP33L热泵控制器 |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| Lin | LinBus | LIN总线 |
| Can | CanBus | CAN总线 |
| CheckCanMessage | string | R,检测CAN消息结果 |
| PartNumber | string | R,PartNumber |
| HardwareNumber | string | R,HardwareNumber |
| SoftwareNumber | string | R,SoftwareNumber |
| NcfNumber | string | R,NCFNumber |
| Kl30Volt1 | float | R,电源电压KL30_1 |
| Kl30Volt2 | float | R,电源电压KL30_2 |
| Sys5VSupply | float | R,内部系统5V |
| Sensor5VSupply | float | R,传感器5V |
| PtcTemSnsr1Volt | float | R,传感器-1-PTCTemSnsr-FR-SIG |
| PtcTemSnsr2Volt | float | R,传感器-2-PTCTemSnsr-FL-SIG |
| IndrHeatExcgrInltTemSnsr3Volt | float | R,传感器-3-IndrHeatExcgrInltTemSnsr-SIG |
| InltHeatExcgrOtltTemSnsr4Volt | float | R,传感器-4-InltHeatExcgrOtltTemSnsr-SIG |
| OtdrHeatExcgrOtltTemSnsr5Volt | float | R,传感器-5-OtdrHeatExcgrOtltTemSnsr-SIG |
| IntkComprTemSnsr6Volt | float | R,传感器-6-IntkComprTemSnsr-SIG |
| IndrHeatEhgOtltPrsSnsr7Volt | float | R,传感器-7-IndrHeatEhgOtltPrsSnsr-SIG |
| IntkComprPrsSnsr8Volt | float | R,传感器-8-IntkComprPrsSnsr-SIG |
| OutsideSnsr9Volt | float | R,传感器-9-OutsideCondensor outside pressure sensor |
| Con2Snsr10Volt | float | R,传感器-10-CON2-PT |
| HtngVlvHsd1State | string | R,高边输出-1-CoolngVlv-HSD—状态查询 |
| CoolngVlvHsd2State | string | R,高边输出-2-HtngVlv-HSD—状态查询 |
| BypsVlvHsd3State | string | R,高边输出-3-BypsVlv-HSD—状态查询 |
| DhmdyVlvHsd4State | string | R,高边输出-4-DhmdyVlv-HSD—状态查询 |
| CabinTxvHsd5State | string | R,高边输出-5-CabinTxv-HSD—状态查询 |
| MultisenseAdVolt1 | float | R,Multisense模拟量输入检测-输出反馈电压1 |
| MultisenseAdVolt2 | float | R,Multisense模拟量输入检测-输出反馈电压2 |
| MultisenseAdVolt3 | float | R,Multisense模拟量输入检测-输出反馈电压3 |
| MultisenseAdVolt4 | float | R,Multisense模拟量输入检测-输出反馈电压4 |
| MultisenseAdVolt5 | float | R,Multisense模拟量输入检测-输出反馈电压5 |
| Eccv1ControlStep | float | R,ECCV1 control当前步数 |
| Eccv2ControlStep | float | R,ECCV2 control当前步数 |
| Eccv3ControlStep | float | R,ECCV3 control当前步数 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| CheckCanMsg | delayMs: string | void | 检测CAN消息 |
| ReadPartNumber | 无 | void | Read PartNumber |
| ReadHardwareNumber | 无 | void | Read HardwareNumber |
| ReadSoftwareNumber | 无 | void | Read SoftwareNumber |
| ReadNcfNumber | 无 | void | Read NCFNumber |
| EnterExtendedSession | 无 | void | 进入拓展模式 |
| EnterNomalSeesion | 无 | void | 进入正常模式 |
| SecurityAccess | 无 | void | 安全解锁 |
| SendSleepCmd | 无 | void | 模块休眠 |
| SendAwakeCmd | 无 | void | 模块唤醒 |
| ReadKl30Volt1 | 无 | void | 读取电源电压KL30_1 |
| ReadKl30Volt2 | 无 | void | 读取电源电压KL30_2 |
| ReadSys5VSupply | 无 | void | 读取内部系统5V |
| ReadSensor5VSupply | 无 | void | 读取传感器5V |
| ReadPtcTemSnsr1Volt | 无 | void | 读传感器-1-PTCTemSnsr-FR-SIG |
| ReadPtcTemSnsr2Volt | 无 | void | 读传感器-2-PTCTemSnsr-FL-SIG |
| ReadIndrHeatExcgrInltTemSnsr3Volt | 无 | void | 读传感器-3-IndrHeatExcgrInltTemSnsr-SIG |
| ReadInltHeatExcgrOtltTemSnsr4Volt | 无 | void | 读传感器-4-InltHeatExcgrOtltTemSnsr-SIG |
| ReadOtdrHeatExcgrOtltTemSnsr5Volt | 无 | void | 读传感器-5-OtdrHeatExcgrOtltTemSnsr-SIG |
| ReadIntkComprTemSnsr6Volt | 无 | void | 读传感器-6-IntkComprTemSnsr-SIG |
| ReadIndrHeatEhgOtltPrsSnsr7Volt | 无 | void | 读传感器-7-IndrHeatEhgOtltPrsSnsr-SIG |
| ReadIntkComprPrsSnsr8Volt | 无 | void | 读传感器-8-IntkComprPrsSnsr-SIG |
| ReadOutsideSnsr9Volt | 无 | void | 读传感器-9-OutsideCondensor-outside-pressure-sensor |
| ReadCon2Snsr10Volt | 无 | void | 读传感器-10-CON2-PT |
| OpenHtngVlvHsd1 | 无 | void | 打开VN7050高边输出-1-HtngVlv-HSD |
| CloseHtngVlvHsd1 | 无 | void | 关闭VN7050高边输出-1-HtngVlv-HSD |
| OpenCoolngVlvHsd2 | 无 | void | 打开VN7050高边输出-2-CoolngVlv-HSD |
| CloseCoolngVlvHsd2 | 无 | void | 关闭VN7050高边输出-2-CoolngVlv-HSD |
| OpenBypsVlvHsd3 | 无 | void | 打开VN7050高边输出-3-BypsVlv-HSD |
| CloseBypsVlvHsd3 | 无 | void | 关闭VN7050高边输出-3-BypsVlv-HSD |
| OpenDhmdyVlvHsd4 | 无 | void | 打开VN7050高边输出-4-DhmdyVlv-HSD |
| CloseDhmdyVlvHsd4 | 无 | void | 关闭VN7050高边输出-4-DhmdyVlv-HSD |
| OpenCabinTxvHsd5 | 无 | void | 打开VN7050高边输出-5-CabinTXV-HSD |
| CloseCabinTxvHsd5 | 无 | void | 关闭VN7050高边输出-5-CabinTXV-HSD |
| ReadHtngVlvHsd1State | 无 | void | 高边输出-1-CoolngVlv-HSD—状态查询 |
| ReadCoolngVlvHsd2State | 无 | void | 高边输出-2-HtngVlv-HSD—状态查询 |
| ReadBypsVlvHsd3State | 无 | void | 高边输出-3-BypsVlv-HSD—状态查询 |
| ReadDhmdyVlvHsd4State | 无 | void | 高边输出-4-DhmdyVlv-HSD—状态查询 |
| ReadCabinTxvHsd5State | 无 | void | 高边输出-5-CabinTxv-HSD—状态查询 |
| ExitHtngVlvHsd1Control | 无 | void | 高边输出-1-CoolngVlv-HSD—退出控制 |
| ExitCoolngVlvHsd2Control | 无 | void | 高边输出-2-HtngVlv-HSD—退出控制 |
| ExitVlvHsd3Control | 无 | void | 高边输出-3-BypsVlv-HSD—退出控制 |
| ExitDhmdyVlvHsd4Control | 无 | void | 高边输出-4-DhmdyVlv-HSD—退出控制 |
| ExitCabinTxvHsd5Control | 无 | void | 高边输出-5-CabinTxv-HSD—退出控制 |
| ReadMultisenseAdVolt1 | 无 | void | 读取Multisense模拟量输入检测-输出反馈电压1 |
| ReadMultisenseAdVolt2 | 无 | void | 读取Multisense模拟量输入检测-输出反馈电压2 |
| ReadMultisenseAdVolt3 | 无 | void | 读取Multisense模拟量输入检测-输出反馈电压3 |
| ReadMultisenseAdVolt4 | 无 | void | 读取Multisense模拟量输入检测-输出反馈电压4 |
| ReadMultisenseAdVolt5 | 无 | void | 读取Multisense模拟量输入检测-输出反馈电压5 |
| WriteEccv1ControlStep | step: string | void | 写入ECCV1 control目标步数 |
| ReadEccv1ControlStep | 无 | void | 读取ECCV1 control当前步数 |
| WriteEccv2ControlStep | step: string | void | 写入ECCV2 Clockwise目标步数 |
| ReadEccv2ControlStep | 无 | void | 读取ECCV2 Clockwise当前步数 |
| WriteEccv3ControlStep | step: string | void | 写入ECCV3 Clockwise目标步数 |
| ReadEccv3ControlStep | 无 | void | 读取ECCV3 Clockwise当前步数 |
| ExitEccv1Control | 无 | void | 退出ECCV1 control |
| ExitEccv2Control | 无 | void | 退出ECCV2 control |
| ExitEccv3Control | 无 | void | 退出ECCV3 control |

## 方法详细说明

### CheckCanMsg

**描述**: 检测CAN消息

**参数**:
- `delayMs` (string): 延迟毫秒

**示例代码**:
```csharp
controller.CheckCanMsg("1000");
string result = controller.CheckCanMessage;
```

---

### ReadPartNumber

**描述**: Read PartNumber

**示例代码**:
```csharp
controller.ReadPartNumber();
string pn = controller.PartNumber;
```

---

### ReadHardwareNumber

**描述**: Read HardwareNumber

**示例代码**:
```csharp
controller.ReadHardwareNumber();
string hw = controller.HardwareNumber;
```

---

### ReadSoftwareNumber

**描述**: Read SoftwareNumber

**示例代码**:
```csharp
controller.ReadSoftwareNumber();
string sw = controller.SoftwareNumber;
```

---

### ReadNcfNumber

**描述**: Read NCFNumber

**示例代码**:
```csharp
controller.ReadNcfNumber();
string ncf = controller.NcfNumber;
```

---

### EnterExtendedSession

**描述**: 进入拓展模式

**示例代码**:
```csharp
controller.EnterExtendedSession();
```

---

### EnterNomalSeesion

**描述**: 进入正常模式

**示例代码**:
```csharp
controller.EnterNomalSeesion();
```

---

### SecurityAccess

**描述**: 安全解锁

**示例代码**:
```csharp
controller.SecurityAccess();
```

---

### SendSleepCmd

**描述**: 模块休眠

**示例代码**:
```csharp
controller.SendSleepCmd();
```

---

### SendAwakeCmd

**描述**: 模块唤醒

**示例代码**:
```csharp
controller.SendAwakeCmd();
```

---

### ReadKl30Volt1

**描述**: 读取电源电压KL30_1

**示例代码**:
```csharp
controller.ReadKl30Volt1();
float volt = controller.Kl30Volt1;
```

---

### ReadKl30Volt2

**描述**: 读取电源电压KL30_2

**示例代码**:
```csharp
controller.ReadKl30Volt2();
float volt = controller.Kl30Volt2;
```

---

### ReadSys5VSupply

**描述**: 读取内部系统5V

**示例代码**:
```csharp
controller.ReadSys5VSupply();
float volt = controller.Sys5VSupply;
```

---

### ReadSensor5VSupply

**描述**: 读取传感器5V

**示例代码**:
```csharp
controller.ReadSensor5VSupply();
float volt = controller.Sensor5VSupply;
```

---

### OpenHtngVlvHsd1

**描述**: 打开VN7050高边输出-1-HtngVlv-HSD

**示例代码**:
```csharp
controller.OpenHtngVlvHsd1();
```

---

### CloseHtngVlvHsd1

**描述**: 关闭VN7050高边输出-1-HtngVlv-HSD

**示例代码**:
```csharp
controller.CloseHtngVlvHsd1();
```

---

### OpenCoolngVlvHsd2

**描述**: 打开VN7050高边输出-2-CoolngVlv-HSD

**示例代码**:
```csharp
controller.OpenCoolngVlvHsd2();
```

---

### CloseCoolngVlvHsd2

**描述**: 关闭VN7050高边输出-2-CoolngVlv-HSD

**示例代码**:
```csharp
controller.CloseCoolngVlvHsd2();
```

---

### OpenBypsVlvHsd3

**描述**: 打开VN7050高边输出-3-BypsVlv-HSD

**示例代码**:
```csharp
controller.OpenBypsVlvHsd3();
```

---

### CloseBypsVlvHsd3

**描述**: 关闭VN7050高边输出-3-BypsVlv-HSD

**示例代码**:
```csharp
controller.CloseBypsVlvHsd3();
```

---

### OpenDhmdyVlvHsd4

**描述**: 打开VN7050高边输出-4-DhmdyVlv-HSD

**示例代码**:
```csharp
controller.OpenDhmdyVlvHsd4();
```

---

### CloseDhmdyVlvHsd4

**描述**: 关闭VN7050高边输出-4-DhmdyVlv-HSD

**示例代码**:
```csharp
controller.CloseDhmdyVlvHsd4();
```

---

### OpenCabinTxvHsd5

**描述**: 打开VN7050高边输出-5-CabinTXV-HSD

**示例代码**:
```csharp
controller.OpenCabinTxvHsd5();
```

---

### CloseCabinTxvHsd5

**描述**: 关闭VN7050高边输出-5-CabinTXV-HSD

**示例代码**:
```csharp
controller.CloseCabinTxvHsd5();
```

---

### ReadMultisenseAdVolt1

**描述**: 读取Multisense模拟量输入检测-输出反馈电压1

**示例代码**:
```csharp
controller.ReadMultisenseAdVolt1();
float volt = controller.MultisenseAdVolt1;
```

---

### WriteEccv1ControlStep

**描述**: 写入ECCV1 control目标步数

**参数**:
- `step` (string): 步数值

**示例代码**:
```csharp
controller.WriteEccv1ControlStep("100");
```

---

### ReadEccv1ControlStep

**描述**: 读取ECCV1 control当前步数

**示例代码**:
```csharp
controller.ReadEccv1ControlStep();
float step = controller.Eccv1ControlStep;
```

---

### ExitEccv1Control

**描述**: 退出ECCV1 control

**示例代码**:
```csharp
controller.ExitEccv1Control();
```
