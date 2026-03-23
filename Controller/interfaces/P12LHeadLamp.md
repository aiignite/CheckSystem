# P12LHeadLamp 控制器接口文档

## 控制器信息

| 控制器名称 | 控制器描述 |
|-----------|-----------|
| P12LHeadLamp | CAN-Product,P12L前灯-融合方案 |

## 公共字段

| 名称 | 类型 | 类别 | 描述 |
|------|------|------|------|
| Can | CanBus | 总线 | CAN总线 |
| IsLeftLamp | bool | 读/写 | R/W,是否为左灯 |
| RightHbLedGray | string | 读/写 | R/W,右远光 |
| LeftHbLedGray | string | 读/写 | R/W,左远光 |
| RightLbLedGray | string | 读/写 | R/W,右近光 |
| LeftLbLedGray | string | 读/写 | R/W,左近光 |
| HdlampRespsCmd | bool | 读/写 | R/W,是否影线控制控制 |

## 公共方法

| 名称 | 参数 | 类型 | 说明 | 示例 |
|------|------|------|------|------|
| StartCanLinMsg | 无 | 方法 | 开启CAN消息 |  |
| StopCanLinMsg | 无 | 方法 | 关闭CAN消息 |  |
| DrlPlOn | grayValue: string | 方法 | DRL/PL打开 | DrlPlOn("100") |
| DrlPlOff | 无 | 方法 | DRL/PL关闭 |  |
| TiOn | grayValue: string | 方法 | TI打开 | TiOn("100") |
| TiOff | 无 | 方法 | TI关闭 |  |
| WLedSingleOn | ledIndex: string, grayValue: string | 方法 | WLed单独打开 | WLedSingleOn("1", "100") |
| WLedSingleOff | ledIndex: string | 方法 | WLed单独关闭 | WLedSingleOff("1") |
| YLedSingleOn | ledIndex: string, grayValue: string | 方法 | YLed单独打开 | YLedSingleOn("1", "100") |
| YLedSingleOff | ledIndex: string | 方法 | YLed单独关闭 | YLedSingleOff("1") |
| HdlmpLvlngReq | value: string | 方法 | HdlmpLvlngReq | HdlmpLvlngReq("1") |
