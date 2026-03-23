# ElmosAppDownload

## Controller Description

LIN-Product,ELMOSAppDownload

## Public Fields

| 名称 | 描述 | 类型 |
|------|------|------|
| DatDownloadResult | R,刷新结果 | string |
| ReadProductData | R,读取生产日期 | string |
| ReadProductSerialNo | R,读取生产序列号 | string |
| ReadHardwareNo | R,读取硬件版本号 | string |
| ReadProductNo | R,读取供货零件号 | string |
| ReadSlaveConfig | R,读取节点配置 | string |
| BaseSerialNoIndex | R/W,基础序列号 | int |
| DatFilePath | R/W,Dat文件路径 | string |
| BmmLinCmdId | R/W,BmmLinCmdId | string |
| BmmLinAnsId | R/W,BmmLinAnsId | string |
| BmmLinInitialNad | R/W,BmmLinInitialNad | string |
| HardWareVersion | R/W,硬件版本号 | string |
| ProductNo | R/W,供货零件号 | string |
| NodeInfoHex | R/W,节点配置Hex | string |
| SoftPart | R,软件零件号 | string |
| SoftVersion | R,软件版本号 | string |
| ServerIp | R/W,服务器IP地址 | string |
| ServerDataBase | R/W,服务器数据库名称 | string |
| ServerUid | R/W,服务器用户名 | string |
| ServerPwd | R/W,服务器用密码 | string |

## Public Methods

| 方法名称 | 描述 | 参数 |
|----------|------|------|
| DatFileDownload | ELMOSAppDownload |  |
| ReadEpprom | 读EPPROM |  |
| ReadSoftInfo | 读取版本信息 |  |
