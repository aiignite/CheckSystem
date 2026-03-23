# SolenoidValueAppDownload 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: LIN-Product,SolenoidValueAppDownload

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| SolenoidValueAppDownload | LIN-Product,SolenoidValueAppDownload | Lin | LinBus | 字段 | - | LIN总线实例 | - |
| SolenoidValueAppDownload | LIN-Product,SolenoidValueAppDownload | DownloadResult | string | 字段 | - | R,下载结果 | - |
| SolenoidValueAppDownload | LIN-Product,SolenoidValueAppDownload | DownloadCostTime | float | 字段 | - | R,下载耗时-秒 | - |
| SolenoidValueAppDownload | LIN-Product,SolenoidValueAppDownload | AppFilePath | string | 字段 | - | R/W,APP文件路径 | - |
| SolenoidValueAppDownload | LIN-Product,SolenoidValueAppDownload | CalFilePath | string | 字段 | - | R/W,Cal文件路径 | - |
| SolenoidValueAppDownload | LIN-Product,SolenoidValueAppDownload | FirstPointWriteFlag | string | 字段 | - | R,读取第1个点写入标志 | - |
| SolenoidValueAppDownload | LIN-Product,SolenoidValueAppDownload | FirstPointOutputDuty | string | 字段 | - | R,读取第1个点输出占空比 | - |
| SolenoidValueAppDownload | LIN-Product,SolenoidValueAppDownload | SecondPointWriteFlag | string | 字段 | - | R,读取第2个点写入标志 | - |
| SolenoidValueAppDownload | LIN-Product,SolenoidValueAppDownload | SecondPointOutputDuty | string | 字段 | - | R,读取第2个点输出占空比 | - |
| SolenoidValueAppDownload | LIN-Product,SolenoidValueAppDownload | SetCommandLinId | void | 方法 | string lindId | 设置命令帧LinID | SetCommandLinId("1B") |
| SolenoidValueAppDownload | LIN-Product,SolenoidValueAppDownload | SetResponseLinId | void | 方法 | string lindId | 设置响应帧LinID | SetResponseLinId("1C") |
| SolenoidValueAppDownload | LIN-Product,SolenoidValueAppDownload | StartAppDownload | void | 方法 | 无 | 开始APP下载 | StartAppDownload() |
| SolenoidValueAppDownload | LIN-Product,SolenoidValueAppDownload | WriteFirstPointUp | void | 方法 | 无 | 写入第1个点-基于当前输出占空比+1 | WriteFirstPointUp() |
| SolenoidValueAppDownload | LIN-Product,SolenoidValueAppDownload | WriteFirstPointDown | void | 方法 | 无 | 写入第1个点-基于当前输出占空比-1 | WriteFirstPointDown() |
| SolenoidValueAppDownload | LIN-Product,SolenoidValueAppDownload | WriteSecondPointUp | void | 方法 | 无 | 写入第2个点-基于当前输出占空比+1 | WriteSecondPointUp() |
| SolenoidValueAppDownload | LIN-Product,SolenoidValueAppDownload | WriteSecondPointDown | void | 方法 | 无 | 写入第2个点-基于当前输出占空比-1 | WriteSecondPointDown() |
| SolenoidValueAppDownload | LIN-Product,SolenoidValueAppDownload | ReadFirstPoint | void | 方法 | 无 | 读取第1个点 | ReadFirstPoint() |
| SolenoidValueAppDownload | LIN-Product,SolenoidValueAppDownload | ReadSecondPoint | void | 方法 | 无 | 读取第2个点 | ReadSecondPoint() |
