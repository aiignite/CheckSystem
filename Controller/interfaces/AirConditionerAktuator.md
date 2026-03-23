# AirConditionerAktuator 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | AirConditionerAktuator |
| 描述 | LIN-Product,空调出风口执行器 |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| Lin | LinBus | LIN总线 |
| LinOut | LinBus | LIN输出总线 |
| ReadStr1 | string | R,Lin0x18读取结果 |
| ReadStr2 | string | R,Lin0x10读取结果 |
| ReadStr3 | string | R,读取结果3 |
| _Responseerror | string | R,Responseerror |
| _Over_temperature | string | R,Over_temperature |
| _Electr_defect | string | R,Electr_defect |
| _Supply_voltage | string | R,Supply_voltage |
| _Emergency_operation_occurred | string | R,Emergency_operation_occurred |
| _Release_blockage_detection | string | R,Release_blockage_detection |
| _Blockage_occurred | string | R,Blockage_occurred |
| _Reset | string | R,Reset |
| _Coil_hold_current_flow | string | R,Coil_hold_current_flow |
| _Position_specification_status | string | R,Position_specification_status |
| _Speed_status | string | R,Speed_status |
| _Actual_position_actuator | string | R,Actual_position_actuator |
| _Direction_of_travel | string | R,Direction_of_travel |
| _Holding_torque | string | R,Holding_torque |
| _Special_functions | string | R,Special_functions |
| _Address_actuator | string | R,Address_actuator |
| _Emergency_operation_release | string | R,Emergency_operation_release |
| _Emergency_operation_position | string | R,Emergency_operation_position |
| _Direction_of_rotation | string | R,Direction_of_rotation |
| _Stop_mode | string | R,Stop_mode |
| _SMP_Volt | string | R,母线电压(mV) |
| _SMP_IS_RUN | string | R,运行母线电流(mA) |
| _SMP_IS_STALL | string | R,堵转母线电流(mA) |
| _Sts_Dir | string | R,运行方向 |
| _Sts_TSD | string | R,芯片温度 |
| _MaxCurr | string | R,峰值电流 |
| SupplyVolt | string | R,供电电压 |
| WorkCurr | string | R,工作电流 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| ChangeType | idx: int | void | 切换型号 |
| ModuleSleep | 无 | void | ECU休眠 |
| ModuleAwake | 无 | void | ECU唤醒 |
| Stop | 无 | void | 停止读写 |
| MotorStop | 无 | void | 电机停止 |
| MotorStart | 无 | void | 电机启动 |
| MotorSpeed | value: byte | void | 电机档位选择 |
| MotorForward | speed: byte | void | 正转 |
| MotorReverse | speed: byte | void | 反转 |
| ClearEventStart | 无 | void | 报警清除开启 |
| ClearEventStop | 无 | void | 报警清除关闭 |
| ClearMaxCurr | 无 | void | 峰值电流清空 |
| ReadStateMessage | 无 | void | 读取状态信息 |

## 方法详细说明

### ChangeType

**描述**: 切换型号 (0=大众, 1=奥迪)

**参数**:
- `idx` (int): 型号索引

**示例代码**:
```csharp
controller.ChangeType(0); // 大众
controller.ChangeType(1); // 奥迪
```

---

### ModuleSleep

**描述**: ECU休眠

**示例代码**:
```csharp
controller.ModuleSleep();
```

---

### ModuleAwake

**描述**: ECU唤醒

**示例代码**:
```csharp
controller.ModuleAwake();
```

---

### Stop

**描述**: 停止读写

**示例代码**:
```csharp
controller.Stop();
```

---

### MotorStop

**描述**: 电机停止

**示例代码**:
```csharp
controller.MotorStop();
```

---

### MotorStart

**描述**: 电机启动

**示例代码**:
```csharp
controller.MotorStart();
```

---

### MotorSpeed

**描述**: 电机档位选择

**参数**:
- `value` (byte): 档位值

**示例代码**:
```csharp
controller.MotorSpeed(5);
```

---

### MotorForward

**描述**: 正转

**参数**:
- `speed` (byte): 速度

**示例代码**:
```csharp
controller.MotorForward(10);
```

---

### MotorReverse

**描述**: 反转

**参数**:
- `speed` (byte): 速度

**示例代码**:
```csharp
controller.MotorReverse(10);
```

---

### ClearEventStart

**描述**: 报警清除开启

**示例代码**:
```csharp
controller.ClearEventStart();
```

---

### ClearEventStop

**描述**: 报警清除关闭

**示例代码**:
```csharp
controller.ClearEventStop();
```

---

### ClearMaxCurr

**描述**: 峰值电流清空

**示例代码**:
```csharp
controller.ClearMaxCurr();
```

---

### ReadStateMessage

**描述**: 读取状态信息

**示例代码**:
```csharp
controller.ReadStateMessage();
string status = controller._Actual_position_actuator;
```
