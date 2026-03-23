# As33LedAppDownload 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | As33LedAppDownload |
| 描述 | LIN-Product,AS33-LedAppDownload |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| Lin | LinBus | LIN总线 |
| DownloadResult | string | 下载结果 |
| DownloadCostTime | string | 下载耗时 |
| AppFilePath | string | 应用程序文件路径 |
| EraseAppResult | string | 擦除应用程序结果 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| SetCommandLinId | linId: string | void | 设置命令LIN ID |
| SetResponseLinId | linId: string | void | 设置响应LIN ID |
| EraseApp | 无 | void | 擦除应用程序 |
| StartAppDownload | 无 | void | 开始应用程序下载 |

## 方法详细说明

### SetCommandLinId

**描述**: 设置命令LIN ID

**参数**:
- `linId` (string): LIN ID (十六进制)

**示例代码**:
```csharp
controller.SetCommandLinId("01");
```

---

### SetResponseLinId

**描述**: 设置响应LIN ID

**参数**:
- `linId` (string): LIN ID (十六进制)

**示例代码**:
```csharp
controller.SetResponseLinId("02");
```

---

### EraseApp

**描述**: 擦除应用程序

**示例代码**:
```csharp
controller.EraseApp();
string result = controller.EraseAppResult;
```

---

### StartAppDownload

**描述**: 开始应用程序下载

**示例代码**:
```csharp
controller.StartAppDownload();
string result = controller.DownloadResult;
string costTime = controller.DownloadCostTime;
```