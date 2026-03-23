# P12LLowLevelHeadLamp 控制器接口文档

## 控制器信息

| 控制器名称 | 控制器描述 |
|-----------|-----------|
| P12LLowLevelHeadLamp | LIN-Product,P12L低配前灯 |

## 公共字段

| 名称 | 类型 | 类别 | 描述 |
|------|------|------|------|
| BcmLin5 | LinBus | 总线 | BCM LIN5 |
| IccLin6 | LinBus | 总线 | ICC LIN6 |
| DiSucsLghtEnb | bool | 读 | R,转向灯流水使能 |

## 公共方法

| 名称 | 参数 | 类型 | 说明 | 示例 |
|------|------|------|------|------|
| LampAwake | 无 | 方法 | 唤醒 |  |
| LampSleep | 无 | 方法 | 休眠 |  |
| HighBeamReq | 无 | 方法 | HighBeamReq |  |
| LowBeamReq | 无 | 方法 | LowBeamReq |  |
| DrlReq | 无 | 方法 | DrlReq |  |
| PlReq | value: string | 方法 | PL请求 | PlReq("100") |
| Turn | 无 | 方法 | 开关Turn |  |
| WlcReq | 无 | 方法 | Welcome Light Request迎宾请求 |  |
| WlcMode1 | 无 | 方法 | 迎宾模式关 |  |
| WlcMode2 | 无 | 方法 | 迎宾模式开 |  |
| Fwr | 无 | 方法 | Farewell Light Request欢送请求 |  |
| HdlmpLvlngReq | value: string | 方法 | HdlmpLvlngReq | HdlmpLvlngReq("1") |
