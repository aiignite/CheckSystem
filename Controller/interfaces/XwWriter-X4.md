# XwWriter-X4 控制器接口文档

## 控制器基本信息

| 控制器名称 | 控制器描述 |
|------------|------------|
| XwWriter_X4 | 稀微1拖4烧写器 |

## 公共属性

| 名称 | 类型 | 类别 | 说明 | 示例 |
|------|------|------|------|------|
| SelectProjectName | string | 属性 | 烧写文件名称 | - |
| CurrentRcvMsg | string | 属性 | 当前接收到的信息 | - |
| CurrentBurnMsg | string | 属性 | 当前烧录的信息 | - |
| BurnResult1 | string | 属性 | R,通道1烧写结果 | - |
| BurnResult2 | string | 属性 | R,通道2烧写结果 | - |
| BurnResult3 | string | 属性 | R,通道3烧写结果 | - |
| BurnResult4 | string | 属性 | R,通道4烧写结果 | - |
| ReadProjectName | string | 属性 | R/W,当前配置文件名称 | - |

## 公共方法

| 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|------|------|------|------|------|------|
| ConnectXwWriter | void | 方法 | portBaudRate: string | 连接烧写器 | ConnectXwWriter("COM1:115200") |
| SelectProject | void | 方法 | projectName: string | 选择配置文件名称 | SelectProject("project_name") |
| ReadCurrentProject | void | 方法 | 无 | 读取当前配置文件名称 | ReadCurrentProject() |
| ReadCurrentProjectWaitResult | void | 方法 | timeOutSecond: int = 3 | 读取当前配置文件名称(等待结果) | ReadCurrentProjectWaitResult(3) |
| StartBurn | void | 方法 | portNums: string | 开始烧写 | StartBurn("1:2:3:4") |
| StartBurnWaitResult | void | 方法 | portNums: string, timeOutSecond: int = 15 | 开始烧写 | StartBurnWaitResult("1:2:3:4", 15) |