# P12LM1HeadLamp 控制器接口文档

## 控制器信息

| 控制器名称 | 控制器描述 |
|-----------|-----------|
| P12LM1HeadLamp | CAN-Product,P12LM1前灯-融合方案 |

## 公共字段

| 名称 | 类型 | 类别 | 描述 |
|------|------|------|------|
| Can | CanBus | 总线 | CAN总线 |
| IsLeftLamp | bool | 读/写 | R/W,是否为左灯 |
| RightHbLedGray | string | 读/写 | R/W,右远光 |
| LeftHbLedGray | string | 读/写 | R/W,左远光 |
| RightLbLedGray | string | 读/写 | R/W,右近光 |
| LeftLbLedGray | string | 读/写 | R/W,左近光 |
| HdLampRespsCmd | bool | 读/写 | R/W,是否影响线控制器控制 |
| SwVer1 | string | 读 | R,软件版本号1 |
| SwVer2 | string | 读 | R,软件版本号2 |
| SwVer3 | string | 读 | R,软件版本号3 |
| EcuResetResult | string | 读 | R,ECU复位结果 |
| DownloadResult | string | 读 | R,下载结果 |
| DownloadCostTime | string | 读 | R,下载耗时 |
| AppFilePath | string | 读/写 | R/W,APP文件路径 |

## 公共方法

| 名称 | 参数 | 类型 | 说明 | 示例 |
|------|------|------|------|------|
| StartCanLinMsg | 无 | 方法 | 开启CAN消息 |  |
| StopCanLinMsg | 无 | 方法 | 关闭CAN消息 |  |
| DrlPlOn | grayValue: string | 方法 | DRL/PL打开 | DrlPlOn("100") |
| DrlPlOff | 无 | 方法 | DRL/PL关闭 |  |
| TiOn | grayValue: string | 方法 | TI打开 | TiOn("100") |
| TiOff | 无 | 方法 | TI关闭 |  |
| YLedSingleOn | ledIndex: string, grayValue: string | 方法 | YLed单独打开 | YLedSingleOn("1", "100") |
| YLedSingleOff | ledIndex: string | 方法 | YLed单独关闭 | YLedSingleOff("1") |
| LedAllOff | 无 | 方法 | Led全部关闭 |  |
| SideTurnOn | 无 | 方法 | 打开侧转 |  |
| SideTurnOff | 无 | 方法 | 关闭侧转 |  |
| HdlmpLvlngReq | value: string | 方法 | HdlmpLvlngReq | HdlmpLvlngReq("1") |
| CanIsHaveMsgCheck | idx: int | 方法 | BodyCan消息检测 |  |
| ReadSwVer | 无 | 方法 | Read软件版本号 |  |
| EcuReset | 无 | 方法 | ECU复位 |  |
| DownLoadFile | 无 | 方法 | 下载 |  |
