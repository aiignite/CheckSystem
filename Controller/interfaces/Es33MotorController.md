# Es33MotorController 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | Es33MotorController |
| 描述 | LIN-Product,ES33调光执行器 |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| Lin | LinBus | LIN总线 |
| RecvData | string | R,状态反馈 |
| 霍尔学习标志 | string | R,霍尔学习标志 |
| 马达初始化完成标志位 | string | R,马达初始化完成标志位 |
| 马达状态 | string | R,马达状态 |
| 从节点低压故障检测开启 | string | R,从节点低压故障检测开启 |
| 从节点低压故障 | string | R,从节点低压故障 |
| 从节点过压故障检测开启 | string | R,从节点过压故障检测开启 |
| 从节点过压故障 | string | R,从节点过压故障 |
| 马达对地开路检测开启 | string | R,马达对地开路检测开启 |
| 马达对地开路 | string | R,马达对地开路 |
| 马达短路故障检测开启 | string | R,马达短路故障检测开启 |
| 马达短路 | string | R,马达短路 |
| Hall故障检测开启 | string | R,Hall故障检测开启 |
| Hall故障 | string | R,Hall故障 |
| 速度配置表id | string | R,速度配置表ID |
| 温度报警检测开启 | string | R,温度报警检测开启 |
| 马达温度报警 | string | R,马达温度报警 |
| Hid故障检测开启 | string | R,Hid故障检测开启 |
| Hid故障 | string | R,Hid故障 |
| 马达逻辑位置 | string | R,马达逻辑位置 |
| TraceInfo01 | string | R,节点配置信息-01-供货零件号 |
| TraceInfo02 | string | R,节点配置信息-02-生产日期 |
| TraceInfo03 | string | R,节点配置信息-03-生产序列号 |
| TraceInfo04 | string | R,节点配置信息-04-硬件版本 |
| TraceInfo05 | string | R,节点配置信息-05-软件零件号 |
| TraceInfo06 | string | R,节点配置信息-06-应用程序软件版本 |
| TraceInfo07 | string | R,节点配置信息-07-保留 |
| TraceInfo08 | string | R,节点配置信息-08-节点配置 |
| TraceInfo09 | string | R,节点配置信息-09-保留 |
| TraceInfo0A | string | R,节点配置信息-0A-DHL马达运动方向配置 |
| TraceInfo0B | string | R,节点配置信息-0B-马达复位位置(低8位) |
| TraceInfo0C | string | R,节点配置信息-0C-马达复位位置(高8位) |
| PartNum | string | R,供货零件号 |
| ProductDate | int | R,生产日期 |
| ProductNum | double | R,生产序列号 |
| HardVersion | string | R,硬件版本 |
| SoftVersion | string | R,软件版本 |
| SoftPart | string | R,软件零件号 |
| CurrentNode | string | R,当前节点 |
| AppVersion | string | R,应用程序版本号 |
| PrintProductDateFilePath | string | R/W,打印生产日期文本文件 |
| PrintMatrixFilePath | string | R/W,打印二维码文本文件 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| SlaveAwake | 无 | void | Slave唤醒 |
| SlaveSleep | 无 | void | Slave休眠 |
| StartAutoScanStatus | 无 | void | 开启自动读取状态 |
| StopAutoScanStatus | 无 | void | 停止自动读取状态 |
| MotorMove | pos: int | void | 电机移动到档位 |
| ReadStateMessage | 无 | void | 读取状态信息 |
| ReadTraceInfo | 无 | void | 读节点配置信息 |
| Print | version: string | void | 打印 |

## 方法详细说明

### SlaveAwake

**描述**: Slave唤醒

**示例代码**:
```csharp
controller.SlaveAwake();
```

---

### SlaveSleep

**描述**: Slave休眠

**示例代码**:
```csharp
controller.SlaveSleep();
```

---

### StartAutoScanStatus

**描述**: 开启自动读取状态

**示例代码**:
```csharp
controller.StartAutoScanStatus();
```

---

### StopAutoScanStatus

**描述**: 停止自动读取状态

**示例代码**:
```csharp
controller.StopAutoScanStatus();
```

---

### MotorMove

**描述**: 电机移动到档位

**参数**:
- `pos` (int): 档位

**示例代码**:
```csharp
controller.MotorMove(5);
```

---

### ReadStateMessage

**描述**: 读取状态信息

**示例代码**:
```csharp
controller.ReadStateMessage();
// 读取: 霍尔学习标志, 马达初始化完成标志位, 马达状态, 从节点低压故障检测开启, 从节点低压故障, 从节点过压故障检测开启, 从节点过压故障, 马达对地开路检测开启, 马达对地开路, 马达短路故障检测开启, 马达短路, Hall故障检测开启, Hall故障, 速度配置表id, 温度报警检测开启, 马达温度报警, Hid故障检测开启, Hid故障, 马达逻辑位置
```

---

### ReadTraceInfo

**描述**: 读节点配置信息

**示例代码**:
```csharp
controller.ReadTraceInfo();
// 读取: TraceInfo01-TraceInfo0C, PartNum, ProductDate, ProductNum, HardVersion, SoftVersion, SoftPart, CurrentNode, AppVersion
```

---

### Print

**描述**: 打印

**参数**:
- `version` (string): 版本

**示例代码**:
```csharp
controller.Print("V1.0");
```
