# Cd569AllLedDriver

## Controller Description

LIN-Product,CD569全LED驱动模块

## Public Fields

| 名称 | 描述 | 类型 |
|------|------|------|
| HwPn | R,总成零件号 | string |
| SerialNum | R,生产序列号 | string |
| ManufactureDate | R,生产日期 | string |
| FblSwPn | R,FBL零件号 | string |
| AppSwPn | R,应用程序零件号 | string |
| CfgPn | R,配置文件零件号 | string |
| FblVer | R,FBL版本号 | string |
| AppSwVer | R,应用程序版本号 | string |
| CfgVer | R,配置文件版本号 | string |
| MecValue | R,MEC值读取 | string |
| MecClearResult | R,MEC清零 | string |
| SeedKeyResult | R,解锁SeedKey | string |
| ChipOniCfgFilePath | R/W,芯旺微单片机配置文件路径 | string |
| ChipOniCfgDownloadResult | R,芯旺微单片机配置下载结果 | string |

## Public Methods

| 方法名称 | 描述 | 参数 |
|----------|------|------|
| InitMasterLinId | 初始化主LIN ID | masterLinId |
| InitSlavaLinId | 初始化从LIN ID | slaveLinId |
| MecClear | MEC清零 |  |
| MecReset | MEC复位至可点亮 |  |
| ReadMec | MEC值读取 |  |
| WriteHwPn | 测试写总成零件号 |  |
| WriteSerialNum | 测试写固定值生产序列号 |  |
| WriteManufactureDate | 测试写固定值生产日期 |  |
| ReadHwPn | 读总成零件号 |  |
| ReadSerialNum | 读生产序列号 |  |
| ReadManufactureData | 读生产日期 |  |
| ReadFblSwPn | 读FBL零件号 |  |
| ReadAppSwPn | 读应用程序零件号 |  |
| ReadCfgPn | 读配置文件零件号 |  |
| ReadFblVer | 读FBL版本号 |  |
| ReadAppSwVer | 读应用程序版本号 |  |
| ReadCfgVer | 配置文件版本号 |  |
| WriteCustomCmd | 写自定义命令 | sendByteVal |
| ReadCustom | 读自定义 | sendByteVal |
| WriteCustomMasterCmd | 写自定义主命令 | sendByteVal |
| UnlockSeedKey | 解锁SeedKey |  |
| ClearResult | 清除结果 |  |
| ChipOniCfgDownload | 芯旺微单片机配置文件下载 |  |
