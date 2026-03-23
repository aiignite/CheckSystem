# CcdAutoAssemblyJXTRM 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | CcdAutoAssemblyJXTRM |
| 描述 | (无描述) |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| MySerialPort | MySerialPort | 串口通信 |
| VoltageRead | float | R,输出电压值 |
| CurrenttRead | float | R,输出电流值 |
| R1-R40 | double | 电阻值1-40 |
| RR1-RR40 | double | 电阻值1-40 (第二组) |
| enableReady | bool | 上使能状态 |
| enableReady2 | bool | 上使能状态2 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| ConnectJX | protocolValue: string | void | 连接JX设备 |
| Enable | 无 | void | 上使能 |
| Enable2 | 无 | void | 上使能2 |
| CaiJi | 无 | void | 采集 |
| GetReciveRValue | count: int | void | 读电阻 |
| GetReciveRValue2 | count: int | void | 读电阻2 |

## 方法详细说明

### ConnectJX

**描述**: 连接JX设备

**参数**:
- `protocolValue` (string): 协议值，格式为"COM端口:波特率"或"IP地址:端口"

**示例代码**:
```csharp
controller.ConnectJX("COM1:9600");
// 或
controller.ConnectJX("192.168.1.100:8080");
```

---

### Enable

**描述**: 上使能

**示例代码**:
```csharp
controller.Enable();
bool ready = controller.enableReady;
```

---

### Enable2

**描述**: 上使能2

**示例代码**:
```csharp
controller.Enable2();
bool ready = controller.enableReady2;
```

---

### CaiJi

**描述**: 采集

**示例代码**:
```csharp
controller.CaiJi();
```

---

### GetReciveRValue

**描述**: 读电阻

**参数**:
- `count` (int): 数量

**示例代码**:
```csharp
controller.GetReciveRValue(40);
double r1 = controller.R1;
```

---

### GetReciveRValue2

**描述**: 读电阻2

**参数**:
- `count` (int): 数量

**示例代码**:
```csharp
controller.GetReciveRValue2(40);
double rr1 = controller.RR1;
```