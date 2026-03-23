# Al22FrontLamp 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | Al22FrontLamp |
| 描述 | CAN-Product,A2LL前灯 |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| Can | CanBus | CAN总线 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| StartCanMsg | 无 | void | 开启CAN消息 |
| StopCanMsg | 无 | void | 关闭CAN消息 |
| CornerOn | per: string | void | 按百分比打开Corner |
| CornerOff | 无 | void | 关闭Corner |

## 方法详细说明

### StartCanMsg

**描述**: 开启CAN消息

**示例代码**:
```csharp
controller.StartCanMsg();
```

---

### StopCanMsg

**描述**: 关闭CAN消息

**示例代码**:
```csharp
controller.StopCanMsg();
```

---

### CornerOn

**描述**: 按百分比打开Corner

**参数**:
- `per` (string): 百分比值 (1-100)

**示例代码**:
```csharp
controller.CornerOn("50"); // 50%亮度
```

---

### CornerOff

**描述**: 关闭Corner

**示例代码**:
```csharp
controller.CornerOff();
```
