# CcdAutoAssemblySiemensPlc 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | CcdAutoAssemblySiemensPlc |
| 描述 | (无描述) |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 事件

| 名称 | 参数 | 描述 |
|------|------|------|
| PushValue | string controllerName, string fieldName, string value | 值推送事件 |

## 公共字段

### 输入信号 (I)

| 名称 | 类型 | 描述 |
|------|------|------|
| I10001-I10160 | bool | R,输入%I0.0-%I21.7 |

### 输出信号 (O)

| 名称 | 类型 | 描述 |
|------|------|------|
| O00001-O00184 | bool | R/W,输出%Q0.0-%Q22.7 |

### 保持寄存器 (Hr)

| 名称 | 类型 | 描述 |
|------|------|------|
| Hr40001-Hr40250 | ushort | R/W,保持寄存器 |

### 输入寄存器 (Ir)

| 名称 | 类型 | 描述 |
|------|------|------|
| Ir40001-Ir40120 | ushort | R,输入寄存器 |

### 其他

| 名称 | 类型 | 描述 |
|------|------|------|
| ReadMaxLenth | ushort | 最大读取长度 |
| ReadStartAddr | ushort | 读取起始地址 |
| WriteStartAddr | ushort | 写入起始地址 |
| IsReadO | bool | 是否读取输出 |
| IsWriteO | bool | 是否写入输出 |
| GetAsciiStr | string | ASCII字符串 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| InitRemoteIpAddress | ipport: string | void | 初始化远程IP地址 |
| CycleUpdate | 无 | void | 循环更新 |
| ReadCurrentStatus | 无 | void | 读取当前状态 |
| SetSpeed | speed: int | void | 设置速度 |
| SetWidth | width: int | void | 设置宽度 |
| RefreshOutputs | 无 | void | 刷新输出 |
| ResetOutputs | 无 | void | 重置输出 |
| ReadRegs | 无 | void | 读取寄存器 |
| GetAsciiFromUshort | start: string, len: string, format: string | void | 从ushort获取ASCII |

## 方法详细说明

### InitRemoteIpAddress

**描述**: 初始化远程IP地址

**参数**:
- `ipport` (string): IP地址和端口，格式为"IP:端口"

**示例代码**:
```csharp
controller.InitRemoteIpAddress("192.168.1.100:502");
```

---

### CycleUpdate

**描述**: 循环更新

**示例代码**:
```csharp
controller.CycleUpdate();
```

---

### ReadCurrentStatus

**描述**: 读取当前状态

**示例代码**:
```csharp
controller.ReadCurrentStatus();
bool value = controller.I10001;
```

---

### SetSpeed

**描述**: 设置速度

**参数**:
- `speed` (int): 速度值

**示例代码**:
```csharp
controller.SetSpeed(1000);
```

---

### SetWidth

**描述**: 设置宽度

**参数**:
- `width` (int): 宽度值

**示例代码**:
```csharp
controller.SetWidth(500);
```

---

### RefreshOutputs

**描述**: 刷新输出

**示例代码**:
```csharp
controller.RefreshOutputs();
```

---

### ResetOutputs

**描述**: 重置输出

**示例代码**:
```csharp
controller.ResetOutputs();
```

---

### ReadRegs

**描述**: 读取寄存器

**示例代码**:
```csharp
controller.ReadRegs();
ushort value = controller.Hr40001;
```

---

### GetAsciiFromUshort

**描述**: 从ushort获取ASCII

**参数**:
- `start` (string): 起始位置
- `len` (string): 长度
- `format` (string): 格式

**示例代码**:
```csharp
controller.GetAsciiFromUshort("40001", "10", "A:0:12");
string ascii = controller.GetAsciiStr;
```