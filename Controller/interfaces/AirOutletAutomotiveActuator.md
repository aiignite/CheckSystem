# AirOutletAutomotiveActuator 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | AirOutletAutomotiveActuator |
| 描述 | LIN-Product,出风口执行器 |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| Lin | LinBus | LIN总线 |
| ChangeNadResult | string | R,更改NAD结果 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| StartLin | 无 | void | START_LIN |
| StopLin | 无 | void | STOP_LIN |
| Stall_detection_off | nad: byte | void | 堵转使能-Stall_detection_off |
| Stall_detection_on | nad: byte | void | 堵转使能-Stall_detection_on |
| Do_not_reset_event_flags | nad: byte | void | 清除事件标志-Do_not_reset_event_flags |
| Clear_Emergency_run_occurred | nad: byte | void | 清除事件标志-Clear_Emergency_run_occurred |
| Clear_Stall_occurred | nad: byte | void | 清除事件标志-Clear_Stall_occurred |
| Clear_Reset | nad: byte | void | 清除事件标志-Clear_Reset |
| Set_position_valid | nad: byte | void | 有效位置选择-Set_position_valid |
| Start_position_valid | nad: byte | void | 有效位置选择-Start_position_valid |
| No_position_specification_valid | nad: byte | void | 有效位置选择-No_position_specification_valid |
| Set_Pos_65535_Signal | nad: byte, value: ushort | void | 设置目标位置 |
| Sta_Pos_65535_Signal | nad: byte, value: ushort | void | 设置起始位置 |
| Emergency_run_close | nad: byte | void | 紧急使能操作-Emergency_run_close |
| Emergency_run_release | nad: byte | void | 紧急使能操作-Emergency_run_release |
| Lower_stop | nad: byte | void | 紧急位置选择-Lower_stop |
| Upper_stop | nad: byte | void | 紧急位置选择-Upper_stop |
| Rot_Dir_CW | nad: byte | void | 旋转方向选择-Rot_Dir_CW |
| Rot_Dir_CCW | nad: byte | void | 旋转方向选择-Rot_Dir_CCW |
| Normal_Mode | nad: byte | void | 启停选择-Normal_Mode |
| Stop_Mode | nad: byte | void | 启停选择-Stop_Mode |
| AddSlave | linIdHex: string, nadHex: string | void | 添加一个子节点 |
| RemoveSlave | linIdHex: string, nadHex: string | void | 移除一个子节点 |
| ReadAppBoot | nad: byte | void | 读APP/BOOT |
| ChangeNad | srcNadHex: string, tgrtNadHex: string | void | 改NAD |
| AutomaticAddressing | ms: int | void | 自动寻址 |

## 方法详细说明

### StartLin

**描述**: START_LIN

**示例代码**:
```csharp
controller.StartLin();
```

---

### StopLin

**描述**: STOP_LIN

**示例代码**:
```csharp
controller.StopLin();
```

---

### Stall_detection_off

**描述**: 堵转使能-Stall_detection_off

**参数**:
- `nad` (byte): NAD地址

**示例代码**:
```csharp
controller.Stall_detection_off(0x01);
```

---

### Stall_detection_on

**描述**: 堵转使能-Stall_detection_on

**参数**:
- `nad` (byte): NAD地址

**示例代码**:
```csharp
controller.Stall_detection_on(0x01);
```

---

### Do_not_reset_event_flags

**描述**: 清除事件标志-Do_not_reset_event_flags

**参数**:
- `nad` (byte): NAD地址

**示例代码**:
```csharp
controller.Do_not_reset_event_flags(0x01);
```

---

### Clear_Emergency_run_occurred

**描述**: 清除事件标志-Clear_Emergency_run_occurred

**参数**:
- `nad` (byte): NAD地址

**示例代码**:
```csharp
controller.Clear_Emergency_run_occurred(0x01);
```

---

### Clear_Stall_occurred

**描述**: 清除事件标志-Clear_Stall_occurred

**参数**:
- `nad` (byte): NAD地址

**示例代码**:
```csharp
controller.Clear_Stall_occurred(0x01);
```

---

### Clear_Reset

**描述**: 清除事件标志-Clear_Reset

**参数**:
- `nad` (byte): NAD地址

**示例代码**:
```csharp
controller.Clear_Reset(0x01);
```

---

### Set_position_valid

**描述**: 有效位置选择-Set_position_valid

**参数**:
- `nad` (byte): NAD地址

**示例代码**:
```csharp
controller.Set_position_valid(0x01);
```

---

### Start_position_valid

**描述**: 有效位置选择-Start_position_valid

**参数**:
- `nad` (byte): NAD地址

**示例代码**:
```csharp
controller.Start_position_valid(0x01);
```

---

### No_position_specification_valid

**描述**: 有效位置选择-No_position_specification_valid

**参数**:
- `nad` (byte): NAD地址

**示例代码**:
```csharp
controller.No_position_specification_valid(0x01);
```

---

### Set_Pos_65535_Signal

**描述**: 设置目标位置

**参数**:
- `nad` (byte): NAD地址
- `value` (ushort): 目标位置值 (0-65535)

**示例代码**:
```csharp
controller.Set_Pos_65535_Signal(0x01, 32000);
```

---

### Sta_Pos_65535_Signal

**描述**: 设置起始位置

**参数**:
- `nad` (byte): NAD地址
- `value` (ushort): 起始位置值 (0-65535)

**示例代码**:
```csharp
controller.Sta_Pos_65535_Signal(0x01, 1000);
```

---

### Emergency_run_close

**描述**: 紧急使能操作-Emergency_run_close

**参数**:
- `nad` (byte): NAD地址

**示例代码**:
```csharp
controller.Emergency_run_close(0x01);
```

---

### Emergency_run_release

**描述**: 紧急使能操作-Emergency_run_release

**参数**:
- `nad` (byte): NAD地址

**示例代码**:
```csharp
controller.Emergency_run_release(0x01);
```

---

### Lower_stop

**描述**: 紧急位置选择-Lower_stop

**参数**:
- `nad` (byte): NAD地址

**示例代码**:
```csharp
controller.Lower_stop(0x01);
```

---

### Upper_stop

**描述**: 紧急位置选择-Upper_stop

**参数**:
- `nad` (byte): NAD地址

**示例代码**:
```csharp
controller.Upper_stop(0x01);
```

---

### Rot_Dir_CW

**描述**: 旋转方向选择-Rot_Dir_CW

**参数**:
- `nad` (byte): NAD地址

**示例代码**:
```csharp
controller.Rot_Dir_CW(0x01);
```

---

### Rot_Dir_CCW

**描述**: 旋转方向选择-Rot_Dir_CCW

**参数**:
- `nad` (byte): NAD地址

**示例代码**:
```csharp
controller.Rot_Dir_CCW(0x01);
```

---

### Normal_Mode

**描述**: 启停选择-Normal_Mode

**参数**:
- `nad` (byte): NAD地址

**示例代码**:
```csharp
controller.Normal_Mode(0x01);
```

---

### Stop_Mode

**描述**: 启停选择-Stop_Mode

**参数**:
- `nad` (byte): NAD地址

**示例代码**:
```csharp
controller.Stop_Mode(0x01);
```

---

### AddSlave

**描述**: 添加一个子节点

**参数**:
- `linIdHex` (string): LIN ID (十六进制字符串)
- `nadHex` (string): NAD (十六进制字符串)

**示例代码**:
```csharp
controller.AddSlave("0x02", "0x21");
```

---

### RemoveSlave

**描述**: 移除一个子节点

**参数**:
- `linIdHex` (string): LIN ID (十六进制字符串)
- `nadHex` (string): NAD (十六进制字符串)

**示例代码**:
```csharp
controller.RemoveSlave("0x02", "0x21");
```

---

### ReadAppBoot

**描述**: 读APP/BOOT

**参数**:
- `nad` (byte): NAD地址

**示例代码**:
```csharp
controller.ReadAppBoot(0x01);
```

---

### ChangeNad

**描述**: 改NAD

**参数**:
- `srcNadHex` (string): 源NAD (十六进制字符串)
- `tgrtNadHex` (string): 目标NAD (十六进制字符串)

**示例代码**:
```csharp
controller.ChangeNad("0x01", "0x02");
```

---

### AutomaticAddressing

**描述**: 自动寻址

**参数**:
- `ms` (int): 延迟时间(毫秒)

**示例代码**:
```csharp
controller.AutomaticAddressing(100);
```
