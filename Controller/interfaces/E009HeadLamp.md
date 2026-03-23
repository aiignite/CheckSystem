# E009HeadLamp 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | E009HeadLamp |
| 描述 | CAN-Product,红旗E009头灯 |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| Can | CanBus | CAN总线 |
| EcuResetResult | string | R,ECU复位结果 |
| ProgrammingDate | string | R,Programming Date-F199 |
| TesterSerialNumber | string | R,Tester Serial Number-F198 |
| SupplySwVersion | string | R,SupplySwVersion-F195 |
| SupplyHwVersion | string | R,SupplyHwVersion-F193 |
| SoftwareVersion | string | R,SoftwareVersion-F1A0 |
| LampBoardSoftwareVersion | string | R,LampBoardSoftwareVersion-F189 |
| LampBoardHardwareVersion | string | R,LampBoardHardwareVersion-F191 |
| LampBoardPartNumber | string | R,LampBoardPartNumber-F187 |
| ReadNodeValue | int | 读取节点值 |
| DownloadResult | string | R,下载结果 |
| DownloadCostTime | string | R,下载耗时 |
| DrvFlashFilePath | string | 驱动Flash文件路径 |
| AppFilePath | string | APP文件路径 |
| DatasetFilePathFl | string | 左节点Dataset文件路径 |
| DatasetFilePathFr | string | 右节点Dataset文件路径 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| ControllerSleep | 无 | void | 休眠 |
| ControllerAwake | 无 | void | 唤醒 |
| ReadNode | 无 | void | 读取节点信息 |
| ChangeToLeftNode | 无 | void | 切换成左节点 |
| ChangeToRightNode | 无 | void | 切换成右节点 |
| EcuReset | 无 | string | ECU复位 |
| EnterDefaultSession | 无 | void | 进入正常模式 |
| EnterExtendedSession | 无 | void | 进入拓展模式 |
| EnterProgramSession | 无 | void | 进入编程模式 |
| ReadProgrammingDate | isNodeReq: bool | void | ReadProgrammingDate |
| ReadTesterSerialNumber | isNodeReq: bool | void | ReadTesterSerialNumber |
| ReadSupplySwVersion | isNodeReq: bool | void | ReadSupplySwVersion |
| ReadSupplyHwVersion | isNodeReq: bool | void | ReadSupplyHwVersion |
| ReadSoftwareVersion | isNodeReq: bool | void | ReadSoftwareVersion |
| ReadLampBoardSoftwareVersion | isNodeReq: bool | void | ReadLampBoardSoftwareVersion |
| ReadLampBoardHardwareVersion | isNodeReq: bool | void | ReadLampBoardHardwareVersion |
| ReadLampBoardPartNumber | isNodeReq: bool | void | ReadLampBoardPartNumber |
| ReadLampControlVersion | 无 | void | 读取灯控信息 |
| ReadLampVersion | 无 | void | 读取灯具信息 |
| DownLoadFile | 无 | void | 下载 |
| RepeatDownload | repeatCount: int | void | 重复下载测试 |

## 方法详细说明

### ControllerSleep

**描述**: 休眠

**示例代码**:
```csharp
controller.ControllerSleep();
```

---

### ControllerAwake

**描述**: 唤醒

**示例代码**:
```csharp
controller.ControllerAwake();
```

---

### ReadNode

**描述**: 读取节点信息

**示例代码**:
```csharp
controller.ReadNode();
```

---

### ChangeToLeftNode

**描述**: 切换成左节点

**示例代码**:
```csharp
controller.ChangeToLeftNode();
```

---

### ChangeToRightNode

**描述**: 切换成右节点

**示例代码**:
```csharp
controller.ChangeToRightNode();
```

---

### EcuReset

**描述**: ECU复位

**返回类型**: string

**示例代码**:
```csharp
string result = controller.EcuReset();
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

### EnterProgramSession

**描述**: 进入编程模式

**示例代码**:
```csharp
controller.EnterProgramSession();
```

---

### ReadProgrammingDate

**描述**: ReadProgrammingDate

**参数**:
- `isNodeReq` (bool): 是否为节点请求

**示例代码**:
```csharp
controller.ReadProgrammingDate(false);
string date = controller.ProgrammingDate;
```

---

### ReadTesterSerialNumber

**描述**: ReadTesterSerialNumber

**参数**:
- `isNodeReq` (bool): 是否为节点请求

**示例代码**:
```csharp
controller.ReadTesterSerialNumber(false);
string serial = controller.TesterSerialNumber;
```

---

### ReadSupplySwVersion

**描述**: ReadSupplySwVersion

**参数**:
- `isNodeReq` (bool): 是否为节点请求

**示例代码**:
```csharp
controller.ReadSupplySwVersion(false);
string ver = controller.SupplySwVersion;
```

---

### ReadSupplyHwVersion

**描述**: ReadSupplyHwVersion

**参数**:
- `isNodeReq` (bool): 是否为节点请求

**示例代码**:
```csharp
controller.ReadSupplyHwVersion(false);
string ver = controller.SupplyHwVersion;
```

---

### ReadSoftwareVersion

**描述**: ReadSoftwareVersion

**参数**:
- `isNodeReq` (bool): 是否为节点请求

**示例代码**:
```csharp
controller.ReadSoftwareVersion(false);
string ver = controller.SoftwareVersion;
```

---

### ReadLampBoardSoftwareVersion

**描述**: ReadLampBoardSoftwareVersion

**参数**:
- `isNodeReq` (bool): 是否为节点请求

**示例代码**:
```csharp
controller.ReadLampBoardSoftwareVersion(false);
string ver = controller.LampBoardSoftwareVersion;
```

---

### ReadLampBoardHardwareVersion

**描述**: ReadLampBoardHardwareVersion

**参数**:
- `isNodeReq` (bool): 是否为节点请求

**示例代码**:
```csharp
controller.ReadLampBoardHardwareVersion(false);
string ver = controller.LampBoardHardwareVersion;
```

---

### ReadLampBoardPartNumber

**描述**: ReadLampBoardPartNumber

**参数**:
- `isNodeReq` (bool): 是否为节点请求

**示例代码**:
```csharp
controller.ReadLampBoardPartNumber(false);
string pn = controller.LampBoardPartNumber;
```

---

### ReadLampControlVersion

**描述**: 读取灯控信息

**示例代码**:
```csharp
controller.ReadLampControlVersion();
// 读取: ProgrammingDate, TesterSerialNumber, SupplySwVersion, SupplyHwVersion, SoftwareVersion, LampBoardHardwareVersion, LampBoardSoftwareVersion, LampBoardPartNumber
```

---

### ReadLampVersion

**描述**: 读取灯具信息

**示例代码**:
```csharp
controller.ReadLampVersion();
// 读取节点信息: ProgrammingDate, TesterSerialNumber, SupplySwVersion, SupplyHwVersion, SoftwareVersion, LampBoardHardwareVersion, LampBoardSoftwareVersion, LampBoardPartNumber
```

---

### DownLoadFile

**描述**: 下载

**示例代码**:
```csharp
controller.DownLoadFile();
string result = controller.DownloadResult;
string costTime = controller.DownloadCostTime;
```

---

### RepeatDownload

**描述**: 重复下载测试

**参数**:
- `repeatCount` (int): 重复次数

**示例代码**:
```csharp
controller.RepeatDownload(10);
```
