# 控制器文档

## LinCommunication

### 控制器描述



### 字段

| 名称 | 类型 | 类别 | 说明 |
|------|------|------|------|
| BarcodeContent | string |  | 条码内容 |
| DownLoadResult | string |  | 下载结果 |
| HwPn | string |  | 总成零件号 |
| SerialNum | string |  | 生产序列号 |
| ManufactureDate | string |  | 生产日期 |
| FblSwPn | string |  | FBL零件号 |
| AppSwPn | string |  | 应用程序零件号 |
| CfgPn | string |  | 配置文件零件号 |
| FblVer | string |  | FBL版本号 |
| AppSwVer | string |  | 应用程序版本号 |
| CfgVer | string |  | 配置文件版本号 |
| CustomRead | string |  | 自定义读取内容 |

### 方法

| 名称 | 参数 | 说明 | 示例 |
|------|------|------|------|
| InitMasterLinId | masterLinId |  |  |
| InitSlavaLinId | slaveLinId |  |  |
| ReadDownLoadFile | filePath |  |  |
| DownloadByVersion1 | fileName |  |  |
| DownloadByVersion2 | fileName |  |  |
| ConnectStatusCheck | tryCount, masterLinId, slaveLinId |  |  |
| WriteHwPn | sendByteVal | 写总成零件号 |  |
| WriteSerialNum | sendByteVal | 写生产序列号 |  |
| WriteManufactureDate | sendByteVal | 写生产日期 |  |
| ReadHwPn | sendByteVal | 读总成零件号 |  |
| ReadSerialNum | sendByteVal | 读生产序列号 |  |
| ReadManufactureData | sendByteVal | 读生产日期 |  |
| ReadFblSwPn | sendByteVal | 读FBL零件号 |  |
| ReadAppSwPn | sendByteVal | 读应用程序零件号 |  |
| ReadCfgPn | sendByteVal | 读配置文件零件号 |  |
| ReadFblVer | sendByteVal | 读FBL版本号 |  |
| ReadAppSwVer | sendByteVal | 读应用程序版本号 |  |
| ReadCfgVer | sendByteVal | 配置文件版本号 |  |
| WriteCustomCmd | sendByteVal |  |  |
| ReadCustom | sendByteVal |  |  |
| WriteCustomMasterCmd | sendByteVal |  |  |
| GenerateBarcode | value |  |  |

