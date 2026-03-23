# CheckApp 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | CheckApp |
| 描述 | 系统变量控制器 |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| IsByPass | bool | R/W,系统变量IsByPass |
| IsRun | bool | R/W,系统变量IsRun |
| YesOrNo | string | R,YesOrNo |
| EqualTrue | bool | 系统变量EqualTrue |
| Bbool0-Bbool255 | bool | R/W,系统变量Bool0-Bool255 |

## 使用示例

```csharp
CheckApp checkApp = new CheckApp("CheckApp");
// 设置系统变量
checkApp.IsByPass = true;
checkApp.IsRun = true;
// 读取系统变量
bool isBypass = checkApp.IsByPass;
```
