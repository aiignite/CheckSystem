# 控制器文档

## LedAppDownload

### 控制器描述
LIN-Product,LedAppDownload

### 字段

| 名称 | 类型 | 类别 | 说明 |
|------|------|------|------|
| DownloadResult | string | R | 下载结果 |
| DownloadCostTime | float | R | 下载耗时-秒 |
| AppFilePath | string | R/W | APP文件路径 |

### 方法

| 名称 | 参数 | 说明 | 示例 |
|------|------|------|------|
| ChangeToLa |  | 切换成后灯A左 |  |
| ChangeToLb |  | 切换成后灯B左 |  |
| ChangeToRa |  | 切换成后灯A右 |  |
| ChangeToRb |  | 切换成后灯B右 |  |
| GetAppBlocks |  | 读取APP文件 |  |
| StartAppDownload |  | 开始APP下载 |  |

