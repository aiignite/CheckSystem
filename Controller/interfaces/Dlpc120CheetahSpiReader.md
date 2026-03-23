# Dlpc120CheetahSpiReader

## Controller Description



## Public Fields

| 名称 | 描述 | 类型 |
|------|------|------|
| CalFormatPoccolo | R,Cal Format (Piccolo) | string |
| CfgFormatPoccolo | R,Cfg Format (Piccolo) | string |
| PiccoloSwVersion | R,Piccolo SW Version | string |
| Dlpc120RtlVersion | R,DLPC120 RTL Version | string |
| CalDataIdentifier | R,Cal Data Identifier | string |
| CfgDataIdentifier | R,Cfg Data Identifier | string |
| AsicAppFlashData | R,ASIC App Flash Data | string |

## Public Methods

| 方法名称 | 描述 | 参数 |
|----------|------|------|
| GetCalFileFormatVersion | 获取校准文件格式版本 |  |
| GetCfgFileFormatVersion | 获取配置文件格式版本 |  |
| GetDlpc120RtlVersion | 获取DLPC120 RTL版本 |  |
| GetPiccoloVersion | 获取Piccolo版本 |  |
| GetCalibrationVersion | 获取校准版本 |  |
| GetAsicAppFlashIdentifier | 获取ASIC应用Flash标识 |  |
| SendCommand | 发送命令 | packet, packetLen, rdData, rdLen, timeout |
