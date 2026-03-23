# FogControlPlc 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | FogControlPlc |
| 描述 | 雾灯控制PLC |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| PcWriteCheckEnd | bool | R/W,40001检测完成 |
| PcWriteCheckOk | bool | R/W,40002检测结果OK |
| PcWriteCheckNg | bool | R/W,40003检测结果NG |
| PcWriteServoRun | bool | R/W,40004检测模组同步 |
| PcWriteBarcodeScanEnd | bool | R/W,40005扫码完成 |
| PcWriteProductType | int | R/W,40006当前产品编号 |
| PcWriteServoXPos | float | R/W,40007~40008检测模组X位置 |
| PcWriteServoZPos | float | R/W,40009~40010检测模组Z位置 |
| PcReadCheckStart | bool | R,400121检测启动 |
| PcReadServoRunEnd | bool | R,400122检测模组到位 |
| PcReadBarcodeScanStart | bool | R,400123扫码启动 |
| PcReadIlluminometer | float | R,400125~400126照度计寄存器 |
| PreStart | bool | R,预启动状态 |
| BarcodeResult | string | 扫码结果 |
| BarcodeTriggerCmd | string | 扫码触发命令 |
| IsReadingBarcode | bool | 是否正在读取条码 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| InitRemoteIpAddress | ipport: string | void | 初始化远程IP地址 |
| CycleUpdate | 无 | void | 循环更新 |
| ConnectBarcode | ipPort: string | void | 连接条码扫描器 |
| ReadBarcode | format: string | void | 读取条码 |
| DequeueBarocde | 无 | void | 取出条码结果 |

## 方法详细说明

### ConnectBarcode

**描述**: 连接条码扫描器

**参数**:
- `ipPort` (string): 条码扫描器IP地址和端口，格式如"192.168.1.100:8080"

**示例代码**:
```csharp
FogControlPlc fogPlc = new FogControlPlc("FogPlc");
fogPlc.ConnectBarcode("192.168.1.100:8080");
```

---

### ReadBarcode

**描述**: 读取条码

**参数**:
- `format` (string): 格式字符串，格式为"key:keyIndex:len"，例如"T:0:12"表示以T开头，偏移0，长度12

**示例代码**:
```csharp
fogPlc.ReadBarcode("T:0:12");
Thread.Sleep(1000);
fogPlc.DequeueBarocde();
string barcode = fogPlc.BarcodeResult;
```

---

### DequeueBarocde

**描述**: 取出条码结果

**示例代码**:
```csharp
fogPlc.DequeueBarocde();
string result = fogPlc.BarcodeResult;
```
