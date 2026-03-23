# WriteNow 控制器接口文档

## 控制器基本信息

| 控制器名称 | 控制器描述 |
|------------|------------|
| WriteNow | 多通道烧写器 |

## 公共属性

| 名称 | 类型 | 类别 | 说明 | 示例 |
|------|------|------|------|------|
| Site1DownloadResult | string | 属性 | R,通道1烧写结果 | - |
| Site2DownloadResult | string | 属性 | R,通道2烧写结果 | - |
| Site3DownloadResult | string | 属性 | R,通道3烧写结果 | - |
| Site4DownloadResult | string | 属性 | R,通道4烧写结果 | - |
| Site5DownloadResult | string | 属性 | R,通道5烧写结果 | - |
| Site6DownloadResult | string | 属性 | R,通道6烧写结果 | - |
| Site7DownloadResult | string | 属性 | R,通道7烧写结果 | - |
| Site8DownloadResult | string | 属性 | R,通道8烧写结果 | - |

## 公共方法

| 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|------|------|------|------|------|------|
| InitComBaute | void | 方法 | comtBaudTate: string | 初始化串口 | InitComBaute("COM1:115200") |
| InitLan | void | 方法 | ipPort: string | 初始化网口 | InitLan("192.168.1.100:8080") |
| CloseLan | void | 方法 | 无 | 关闭网口 | CloseLan() |
| SelectProjectName | void | 方法 | projectName: string | 选项目名称 | SelectProjectName("project_name") |
| RunSingleSite | void | 方法 | siteIndex: string | RUN单通道 | RunSingleSite("1") |
| RunRangeSite | void | 方法 | startEnd: string | RUN多通道 | RunRangeSite("1~4") |