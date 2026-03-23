# S12LLowLevelHeadLamp 控制器接口文档

## 控制器信息

| 控制器名称 | 控制器描述 |
|-----------|-----------|
| S12LLowLevelHeadLamp | LIN-Product,S12L低配前灯 |

## 公共字段

| 名称 | 类型 | 类别 | 描述 |
|------|------|------|------|
| TurnFlowEnable | bool | 读/写 | R/W,转向流水使能 |
| AppVer | string | 读 | R,软件版本号 |
| HardwareVer | string | 读 | R,硬件版本号 |
| AppDownloadResult | string | 读 | R,App下载结果 |
| AppDownloadCostTime | float | 读 | R,APP下载耗时-秒 |
| FlashDriverFilePath | string | 读/写 | R/W,FlashDrv文件路径 |
| AppFielPath | string | 读/写 | R/W,APP文件路径 |

## 公共方法

| 名称 | 参数 | 类型 | 说明 | 示例 |
|------|------|------|------|------|
| LampAwake | 无 | 方法 | 唤醒 | |
| LampSleep | 无 | 方法 | 休眠 | |
| HighBeamReq | 无 | 方法 | HighBeamReq | |
| LowBeamReq | 无 | 方法 | LowBeamReq | |
| DrlReq | 无 | 方法 | DrlReq | |
| PlReq | 无 | 方法 | PlReq | |
| Turn | 无 | 方法 | 开关Turn | |
| WlcReq | 无 | 方法 | Welcome Light Request迎宾请求 | |
| WlcMode1 | 无 | 方法 | 迎宾模式关 | |
| WlcMode2 | lvl: string | 方法 | 迎宾模式开 | WlcMode2("1") |
| Fwr | 无 | 方法 | Farewell Light Request欢送请求 | |
| HdlmpLvlngReq | value: string | 方法 | HdlmpLvlngReq | HdlmpLvlngReq("5") |
| ReadVer | 无 | 方法 | Read版本号 | |
| AppDownload | 无 | 方法 | 刷新APP文件 | |
