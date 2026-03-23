# WriteNowApi 控制器接口文档

## 控制器基本信息

| 控制器名称 | 控制器描述 |
|------------|------------|
| WriteNowApi | 多通道烧写器API |

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
| GenerateWniSaveFilePath | string | 属性 | R/W,生成Wni文件的保存路径 | - |
| ToGenerateBinFilePath | string | 属性 | R/W,需要生成的BIN文件路径 | - |
| CheckFileExit | string | 属性 | R,判断生成的文件是否存在 | - |
| DownLoadFileFolder | string | 属性 | R/W,需要下载的文件目录 | - |
| RemoteBinFilePath | string | 属性 | R/W,bin文件的远程目录 | - |
| RemoteBinFileUserName | string | 属性 | R/W,bin文件的远程目录用户名 | - |
| RemoteBinFilePassword | string | 属性 | R/W,bin文件的远程目录密码 | - |
| DownloadFilePath | string | 属性 | 下载文件路径 | - |
| UploadFilePath | string | 属性 | 上传文件路径 | - |

## 公共方法

| 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|------|------|------|------|------|------|
| ConnectLan | void | 方法 | ipPort: string | 连接烧写器网口 | ConnectLan("192.168.1.100:8080") |
| DisConnectLan | void | 方法 | 无 | 断开烧写器网口 | DisConnectLan() |
| SelectProjectName | void | 方法 | projectName: string | 选项目名称 | SelectProjectName("project_name") |
| RunSingleSite | void | 方法 | siteIndex: string | RUN单通道 | RunSingleSite("1") |
| RunRangeSite | void | 方法 | startEnd: string | RUN多通道 | RunRangeSite("1~4") |
| GenerateBin2WniFile | void | 方法 | 无 | 生成Wni文件 | GenerateBin2WniFile() |
| DeleteWniFile | void | 方法 | 无 | 删除Wni文件 | DeleteWniFile() |
| DownloadFile | void | 方法 | fileName: string | 下载文件 | DownloadFile("filename.bin") |
| DeleteFile | void | 方法 | fileName: string | 删除文件 | DeleteFile("filename.bin") |
| UploadFile | void | 方法 | 无 | 上传文件 | UploadFile() |
| GenerateRemoteBin2WniFile | void | 方法 | 无 | 生成Wni文件(远程) | GenerateRemoteBin2WniFile() |