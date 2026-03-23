# A58RLCMA 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | A58RLCMA |
| 描述 | CAN-Product,A58_RLCMA |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| Can | CanBus | CAN总线 |
| PlPwm | string | R/W,PL_PWM |
| TlPwm | string | R/W,TL_PWM |
| IsLeft | bool | R,左右识别 |
| AppVer | string | R,应用程序版本号 |
| FblVer | string | R,引导程序版本号 |
| CfgVer | string | R,配置程序版本号 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| SetLeft | 无 | void | 设置为左节点 |
| SetRight | 无 | void | 设置为右节点 |
| SetCanId | reqCanId: string, recvCanId: string | void | 设置CANID |
| ReadAppVer | 无 | void | Read应用程序版本号 |
| ReadFblVer | 无 | void | Read引导程序版本号 |
| ReadCfgVer | 无 | void | Read配置程序版本号 |

## 方法详细说明

### SetLeft

**描述**: 设置为左节点

**示例代码**:
```csharp
controller.SetLeft();
```

---

### SetRight

**描述**: 设置为右节点

**示例代码**:
```csharp
controller.SetRight();
```

---

### SetCanId

**描述**: 设置CANID

**参数**:
- `reqCanId` (string): 请求CAN ID (十六进制)
- `recvCanId` (string): 接收CAN ID (十六进制)

**示例代码**:
```csharp
controller.SetCanId("75B", "7DB");
```

---

### ReadAppVer

**描述**: Read应用程序版本号

**示例代码**:
```csharp
controller.ReadAppVer();
string ver = controller.AppVer;
```

---

### ReadFblVer

**描述**: Read引导程序版本号

**示例代码**:
```csharp
controller.ReadFblVer();
string ver = controller.FblVer;
```

---

### ReadCfgVer

**描述**: Read配置程序版本号

**示例代码**:
```csharp
controller.ReadCfgVer();
string ver = controller.CfgVer;
```