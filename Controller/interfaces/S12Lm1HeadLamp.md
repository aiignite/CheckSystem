# S12Lm1HeadLamp 控制器接口文档

## 控制器信息

| 控制器名称 | 控制器描述 |
|-----------|-----------|
| S12Lm1HeadLamp | CAN-Product,S12L-M1前灯 |

## 公共字段

| 名称 | 类型 | 类别 | 描述 |
|------|------|------|------|
| IsLeftLamp | bool | 读/写 | R/W,是否为左灯 |
| LeftLbLedGray | string | 读/写 | R/W,LeftLBLedGray |
| RightLbLedGray | string | 读/写 | R/W,RightLBLedGray |
| LeftHbLedGray | string | 读/写 | R/W,LeftHBLedGray |
| RightHbLedGray | string | 读/写 | R/W,RightHBLedGray |
| SwVer | string | 读 | R,Software Version |

## 公共方法

| 名称 | 参数 | 类型 | 说明 | 示例 |
|------|------|------|------|------|
| StartCanMsg | 无 | 方法 | 开启CAN消息 | |
| StopCanMsg | 无 | 方法 | 关闭CAN消息 | |
| DrlOn | 无 | 方法 | DRL打开 | |
| PlOn | 无 | 方法 | PL打开 | |
| DrlOddOn | 无 | 方法 | DRL单数打开 | |
| DrlEvenOn | 无 | 方法 | DRL双数打开 | |
| DrlPlOff | 无 | 方法 | DRL/PL关闭 | |
| TiOn | 无 | 方法 | TI打开 | |
| TiOff | 无 | 方法 | TI关闭 | |
| HdlmpLvlngReq | value: string | 方法 | HdlmpLvlngReq | HdlmpLvlngReq("5") |
| ReadSwVer | 无 | 方法 | READ SwVer | |
