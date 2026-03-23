# FlashRunner 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | FlashRunner |
| 描述 | Flash编程器控制器 |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| ProjName | string | 项目名称 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| InitIpPort | ipPort: string | void | 初始化IP和端口 |
| Run | index: string | void | 运行项目 |

## 方法详细说明

### InitIpPort

**描述**: 初始化IP和端口

**参数**:
- `ipPort` (string): IP和端口，格式如"192.168.1.100:8080"

**示例代码**:
```csharp
controller.InitIpPort("192.168.1.100:8080");
```

---

### Run

**描述**: 运行项目

**参数**:
- `index` (string): 项目索引

**示例代码**:
```csharp
controller.Run("1");
```
