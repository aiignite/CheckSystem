# CycloneController 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | CycloneController |
| 描述 | 烧写控制器 - Cyclone Flash Programmer |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| ProgramResult | string | R,烧写结果 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| ConnectDevice | name: string | void | 连接设备 |
| StartProgram | imageIndex: string | void | 按索引启动烧写 |
| StartProgramByName | imageName: string | void | 按名称启动烧写 |

## 方法详细说明

### ConnectDevice

**描述**: 连接设备

**参数**:
- `name` (string): 设备名称

**示例代码**:
```csharp
var controller = new CycloneController("Cyclone1");
controller.ConnectDevice("COM1");
```

---

### StartProgram

**描述**: 按索引启动烧写

**参数**:
- `imageIndex` (string): 镜像索引

**示例代码**:
```csharp
controller.StartProgram("1");
```

---

### StartProgramByName

**描述**: 按名称启动烧写

**参数**:
- `imageName` (string): 镜像名称

**示例代码**:
```csharp
controller.StartProgramByName("Application");
```
