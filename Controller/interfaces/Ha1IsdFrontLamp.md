# Ha1IsdFrontLamp 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | Ha1IsdFrontLamp |
| 描述 | CAN-Product,HA1-ISD-前灯 |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| Can | CanBus | CAN总线 |
| Gray | string | R/W,点亮灰度值0~255 |
| EvenOddSwitch | string | R/W,单双切换基数 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| LampAwake | 无 | void | 唤醒 |
| LampSleep | 无 | void | 休眠 |
| TurnOn | 无 | void | 转向灯开 |
| TurnOff | 无 | void | 转向灯关 |
| DrlOn | 无 | void | 日行灯开 |
| DrlOff | 无 | void | 日行灯关 |
| PlOn | 无 | void | 位置灯开 |
| PlOff | 无 | void | 位置灯关 |
| AllLedOn | 无 | void | 所有LED全亮 |
| AllLedOff | 无 | void | 所有LED全关 |
| AllWhiteOn | 无 | void | 所有白光LED全亮 |
| AllWhiteEvenOn | 无 | void | 所有白光LED偶数点亮 |
| AllWhiteOddOn | 无 | void | 所有白光LED奇数点亮 |
| AllYellowOn | 无 | void | 所有黄光LED全亮 |
| AllYellowEvenOn | 无 | void | 所有黄光LED偶数点亮 |
| AllYellowOddOn | 无 | void | 所有黄光LED奇数点亮 |
| ChangeToFrontALeft | 无 | void | 切换成前灯A左 |
| ChangeToFrontARight | 无 | void | 切换成前灯A右 |
| ChangeToFrontC | 无 | void | 切换成前灯C |
| ChangeToFrontBLeft | 无 | void | 切换成前灯B左 |
| ChangeToFrontBRight | 无 | void | 切换成前灯B右 |
| ChangeToFrontBLeftUp | 无 | void | 切换成高配前灯B左 |
| ChangeToFrontBRightUp | 无 | void | 切换成高配前灯B右 |
| ReadFblPartNo | reqCanId: string, recvCanId: string, did: string | void | 读取FBL零件号 |
| ReadFblVersion | reqCanId: string, recvCanId: string, did: string | void | 读取FBL版本号 |
| ReadInternalFblVersion | reqCanId: string, recvCanId: string, did: string | void | 读取FBL内部版本号 |
| ReadAppPartNo | reqCanId: string, recvCanId: string, did: string | void | 读取APP零件号 |
| ReadAppVersion | reqCanId: string, recvCanId: string, did: string | void | 读取APP版本号 |
| ReadInternalAppVersion | reqCanId: string, recvCanId: string, did: string | void | 读取APP内部版本号 |

## 方法详细说明

### LampAwake

**描述**: 唤醒

**示例代码**:
```csharp
controller.LampAwake();
```

---

### LampSleep

**描述**: 休眠

**示例代码**:
```csharp
controller.LampSleep();
```

---

### TurnOn

**描述**: 转向灯开

**示例代码**:
```csharp
controller.TurnOn();
```

---

### TurnOff

**描述**: 转向灯关

**示例代码**:
```csharp
controller.TurnOff();
```

---

### DrlOn

**描述**: 日行灯开

**示例代码**:
```csharp
controller.DrlOn();
```

---

### DrlOff

**描述**: 日行灯关

**示例代码**:
```csharp
controller.DrlOff();
```

---

### PlOn

**描述**: 位置灯开

**示例代码**:
```csharp
controller.PlOn();
```

---

### PlOff

**描述**: 位置灯关

**示例代码**:
```csharp
controller.PlOff();
```

---

### AllLedOn

**描述**: 所有LED全亮

**示例代码**:
```csharp
controller.AllLedOn();
```

---

### AllLedOff

**描述**: 所有LED全关

**示例代码**:
```csharp
controller.AllLedOff();
```

---

### AllWhiteOn

**描述**: 所有白光LED全亮

**示例代码**:
```csharp
controller.AllWhiteOn();
```

---

### AllWhiteEvenOn

**描述**: 所有白光LED偶数点亮

**示例代码**:
```csharp
controller.AllWhiteEvenOn();
```

---

### AllWhiteOddOn

**描述**: 所有白光LED奇数点亮

**示例代码**:
```csharp
controller.AllWhiteOddOn();
```

---

### AllYellowOn

**描述**: 所有黄光LED全亮

**示例代码**:
```csharp
controller.AllYellowOn();
```

---

### AllYellowEvenOn

**描述**: 所有黄光LED偶数点亮

**示例代码**:
```csharp
controller.AllYellowEvenOn();
```

---

### AllYellowOddOn

**描述**: 所有黄光LED奇数点亮

**示例代码**:
```csharp
controller.AllYellowOddOn();
```

---

### ChangeToFrontALeft

**描述**: 切换成前灯A左

**示例代码**:
```csharp
controller.ChangeToFrontALeft();
```

---

### ChangeToFrontARight

**描述**: 切换成前灯A右

**示例代码**:
```csharp
controller.ChangeToFrontARight();
```

---

### ChangeToFrontC

**描述**: 切换成前灯C

**示例代码**:
```csharp
controller.ChangeToFrontC();
```

---

### ChangeToFrontBLeft

**描述**: 切换成前灯B左

**示例代码**:
```csharp
controller.ChangeToFrontBLeft();
```

---

### ChangeToFrontBRight

**描述**: 切换成前灯B右

**示例代码**:
```csharp
controller.ChangeToFrontBRight();
```

---

### ChangeToFrontBLeftUp

**描述**: 切换成高配前灯B左

**示例代码**:
```csharp
controller.ChangeToFrontBLeftUp();
```

---

### ChangeToFrontBRightUp

**描述**: 切换成高配前灯B右

**示例代码**:
```csharp
controller.ChangeToFrontBRightUp();
```

---

### ReadFblPartNo

**描述**: 读取FBL零件号

**参数**:
- `reqCanId` (string): 请求CAN ID
- `recvCanId` (string): 接收CAN ID
- `did` (string): DID

**示例代码**:
```csharp
controller.ReadFblPartNo("775", "77D", "F194");
string partNo = controller.FblPartNo;
```

---

### ReadFblVersion

**描述**: 读取FBL版本号

**参数**:
- `reqCanId` (string): 请求CAN ID
- `recvCanId` (string): 接收CAN ID
- `did` (string): DID

**示例代码**:
```csharp
controller.ReadFblVersion("775", "77D", "F195");
string ver = controller.FblVersion;
```

---

### ReadInternalFblVersion

**描述**: 读取FBL内部版本号

**参数**:
- `reqCanId` (string): 请求CAN ID
- `recvCanId` (string): 接收CAN ID
- `did` (string): DID

**示例代码**:
```csharp
controller.ReadInternalFblVersion("775", "77D", "F196");
string ver = controller.InternalFblVersion;
```

---

### ReadAppPartNo

**描述**: 读取APP零件号

**参数**:
- `reqCanId` (string): 请求CAN ID
- `recvCanId` (string): 接收CAN ID
- `did` (string): DID

**示例代码**:
```csharp
controller.ReadAppPartNo("775", "77D", "F194");
string partNo = controller.AppPartNo;
```

---

### ReadAppVersion

**描述**: 读取APP版本号

**参数**:
- `reqCanId` (string): 请求CAN ID
- `recvCanId` (string): 接收CAN ID
- `did` (string): DID

**示例代码**:
```csharp
controller.ReadAppVersion("775", "77D", "F195");
string ver = controller.AppVersion;
```

---

### ReadInternalAppVersion

**描述**: 读取APP内部版本号

**参数**:
- `reqCanId` (string): 请求CAN ID
- `recvCanId` (string): 接收CAN ID
- `did` (string): DID

**示例代码**:
```csharp
controller.ReadInternalAppVersion("775", "77D", "F196");
string ver = controller.InternalAppVersion;
```
