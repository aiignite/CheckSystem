# S12LHighLevelHeadLamp 控制器接口文档

## 控制器信息

| 控制器名称 | 控制器描述 |
|-----------|-----------|
| S12LHighLevelHeadLamp | CAN-Product,S12L高配前灯 |

## 公共字段

| 名称 | 类型 | 类别 | 描述 |
|------|------|------|------|
| IsLeftLamp | bool | 读/写 | R/W,是否为左灯 |
| LeftLbLedGray | string | 读/写 | R/W,LeftLBLedGray |
| RightLbLedGray | string | 读/写 | R/W,RightLBLedGray |
| LeftHbLedGray | string | 读/写 | R/W,LeftHBLedGray |
| RightHbLedGray | string | 读/写 | R/W,RightHBLedGray |

## 公共方法

| 名称 | 参数 | 类型 | 说明 | 示例 |
|------|------|------|------|------|
| StartCanLinMsg | 无 | 方法 | 开启CAN/LIN消息 | |
| StopCanLinMsg | 无 | 方法 | 关闭CAN/LIN消息 | |
| DrlPlOn | grayValue: string | 方法 | DRL/PL打开 | DrlPlOn("100") |
| DrlPlOff | 无 | 方法 | DRL/PL关闭 | |
| TiOn | grayValue: string | 方法 | TI打开 | TiOn("100") |
| TiOff | 无 | 方法 | TI关闭 | |
| LedSingleOn | ledIndex: string, grayValue: string | 方法 | Led单独打开 | LedSingleOn("1", "100") |
| LedSingleOff | ledIndex: string | 方法 | Led单独关闭 | LedSingleOff("1") |
| LedAllOff | 无 | 方法 | Led全部关闭 | |
| SideTurnOn | 无 | 方法 | 打开侧转 | |
| SideTurnOff | 无 | 方法 | 关闭侧转 | |
| HdlmpLvlngReq | value: string | 方法 | HdlmpLvlngReq | HdlmpLvlngReq("5") |