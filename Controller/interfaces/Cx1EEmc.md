# Cx1EEmc 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | Cx1EEmc |
| 描述 | (无描述) |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| AppCan | CanBus | CAN总线 |
| Can2 | CanBus | CAN2总线 |
| Lin | LinBus | LIN总线 |
| LastAppCanRecvDate | DateTime | 最后AppCan接收时间 |
| LastCan2RecvDate | DateTime | 最后Can2接收时间 |
| LastLinRecvDate | DateTime | 最后Lin接收时间 |
| BossIncSts | string | R,老板键1 |
| BossSldSts | string | R,老板键2 |
| CurrDoor | double | R,电动门电机 |
| CurrElectricRelese | double | R,儿童锁/电释放电机 |
| CurrHandle | double | R,门把手电机 |
| CurrLock2 | double | R,吸合锁电机 |
| CurrLockActvnLock | double | R,中控锁/电释放复位电机 |
| CurrMirrorX | double | R,后视镜X电机 |
| CurrMirrorY | double | R,后视镜Y电机 |
| CurrPedal | double | R,电动脚踏电机 |
| CurrWindow | double | R,车窗电机 |
| DoorDrvrHndlSts | string | R,外门把手位置显示 |
| DoorDrvrLatPosn | string | R,门Ajar开关状态 |
| DoorDrvrLockSts | string | R,中控锁状态反馈 |
| DoorDrvrOpenLowReqOutdSwt1 | string | R,外门把手1 |
| DoorDrvrOpenReqInsdSwt1 | string | R,内门把手2 |
| DoorDrvrOpenReqInsdSwtCan2 | string | R,内门把手1 |
| DoorDrvrSwtIntrLockgReq | string | R,中控锁开关状态 |
| FronHandleLight | byte | 前把手灯 |
| IsHavemsg | bool | 是否有消息 |
| LockResetSts | string | R,吸合锁复位开关 |
| Machine | StateMachine | 状态机 |
| MirrPosnStsAtDrvrMirrPosnAdjCldLeRi | double | R,后视镜位置状态 |
| MirrPosnStsAtDrvrMirrPosnAdjCldUpDwn | double | R,后视镜上下位置状态 |
| MotorCmd | byte | 电机命令 |
| MotorDoorSpeed | double | R,门电机速度 |
| MotorPedalSpeed | double | R,脚踏电机速度 |
| OpenDrvrSwt | string | R,门全开开关状态 |
| PawlDrvrSwt | string | R,棘爪(半开)开关状态 |
| SwBsd | bool | BSD开关 |
| SwPocketLight | bool | 口袋灯开关 |
| SwPowSupplyCmdPos | bool | 电源命令位置 |
| SwPowSupplyConDdSswitch | bool | 电源连接DD开关 |
| SwPowSupplyConDoorHall | bool | 电源连接门霍尔 |
| SwPowSupplyConHandleHall | bool | 电源连接把手霍尔 |
| SwPowSupplyConPendalHall | bool | 电源连接脚踏霍尔 |
| SwStepLight | bool | 踏板灯开关 |
| Temperature | double | R,温度 |
| Vba1 | double | R,VBA1 |
| Vba2 | double | R,VBA2 |
| WindowLight | byte | 窗灯 |
| WinSwtReqFrntLe | string | R,车窗本地开关状态 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| StartCan | 无 | void | CAN启用 |
| StopCan | 无 | void | CAN关闭 |
| StartDiagnosis | 无 | void | 开启诊断 |
| MirrTintgCmd | perValue: byte | void | 防眩目调光 |
| MirrDefrstAtDrvrCmd | isOn: bool | void | 后视镜加热 |
| ActvnOfIndcrIndcrOut | isOn: bool | void | 转向灯 |
| ActvnOfPudLi | isOn: bool | void | 照地灯 |
| ActvnOfHndlDoorLi1HndlDoorLiDrvr | value: byte | void | 门把手灯 |
| DrvrMirrCmd | value: byte | void | 后视镜折叠展开 |
| DrvrAdjCmd | value: byte | void | 驾驶员调节命令 |
| ShortDropWin | value: byte | void | 车窗短降 |
| ChdLockReLeCtrlHmiReq | value: byte | void | 儿童锁 |
| Door | value: byte | void | 门控制 |
| Pendal | value: byte | void | 脚踏控制 |
| DoorDrvrHndlCmd | value: byte | void | 门把手命令 |

## 方法详细说明

### StartCan

**描述**: CAN启用

**示例代码**:
```csharp
controller.StartCan();
```

---

### StopCan

**描述**: CAN关闭

**示例代码**:
```csharp
controller.StopCan();
```

---

### StartDiagnosis

**描述**: 开启诊断

**示例代码**:
```csharp
controller.StartDiagnosis();
```

---

### MirrTintgCmd

**描述**: 防眩目调光

**参数**:
- `perValue` (byte): 调光百分比 (0-100)

**示例代码**:
```csharp
controller.MirrTintgCmd(50);
```

---

### MirrDefrstAtDrvrCmd

**描述**: 后视镜加热

**参数**:
- `isOn` (bool): 是否开启

**示例代码**:
```csharp
controller.MirrDefrstAtDrvrCmd(true);
```

---

### ActvnOfIndcrIndcrOut

**描述**: 转向灯

**参数**:
- `isOn` (bool): 是否开启

**示例代码**:
```csharp
controller.ActvnOfIndcrIndcrOut(true);
```

---

### ActvnOfPudLi

**描述**: 照地灯

**参数**:
- `isOn` (bool): 是否开启

**示例代码**:
```csharp
controller.ActvnOfPudLi(true);
```

---

### ActvnOfHndlDoorLi1HndlDoorLiDrvr

**描述**: 门把手灯

**参数**:
- `value` (byte): 灯值

**示例代码**:
```csharp
controller.ActvnOfHndlDoorLi1HndlDoorLiDrvr(1);
```

---

### DrvrMirrCmd

**描述**: 后视镜折叠展开

**参数**:
- `value` (byte): 命令值

**示例代码**:
```csharp
controller.DrvrMirrCmd(1);
```

---

### DrvrAdjCmd

**描述**: 驾驶员调节命令

**参数**:
- `value` (byte): 调节值

**示例代码**:
```csharp
controller.DrvrAdjCmd(1);
```

---

### ShortDropWin

**描述**: 车窗短降

**参数**:
- `value` (byte): 车窗值

**示例代码**:
```csharp
controller.ShortDropWin(1);
```

---

### ChdLockReLeCtrlHmiReq

**描述**: 儿童锁

**参数**:
- `value` (byte): 儿童锁值

**示例代码**:
```csharp
controller.ChdLockReLeCtrlHmiReq(1);
```

---

### Door

**描述**: 门控制

**参数**:
- `value` (byte): 门值

**示例代码**:
```csharp
controller.Door(1);
```

---

### Pendal

**描述**: 脚踏控制

**参数**:
- `value` (byte): 脚踏值

**示例代码**:
```csharp
controller.Pendal(1);
```

---

### DoorDrvrHndlCmd

**描述**: 门把手命令

**参数**:
- `value` (byte): 把手值

**示例代码**:
```csharp
controller.DoorDrvrHndlCmd(1);
```