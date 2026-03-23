# XiaoPengX3FrontLamp 控制器接口文档

## 控制器基本信息

| 控制器名称 | 控制器描述 |
|------------|------------|
| XiaoPengX3FrontLamp | CAN-Product,小鹏X3前灯 |

## 公共属性

| 名称 | 类型 | 类别 | 说明 | 示例 |
|------|------|------|------|------|
| Can | CanBus | 属性 | CAN总线 | - |
| IsLConnect | bool | 属性 | R/W,是否连接L | - |
| IsRConnect | bool | 属性 | R/W,是否连接R | - |
| IsMConnect | bool | 属性 | R/W,是否连接M | - |

## 公共方法

| 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|------|------|------|------|------|------|
| StartCanMsg | void | 方法 | 无 | 打开CAN消息 | StartCanMsg() |
| StopCanMsg | void | 方法 | 无 | 关闭CAN消息 | StopCanMsg() |
| LbOn | void | 方法 | 无 | 近光灯亮 | LbOn() |
| LbOff | void | 方法 | 无 | 近光灯灭 | LbOff() |
| HbOn | void | 方法 | 无 | 远光灯亮 | HbOn() |
| HbOff | void | 方法 | 无 | 远光灯灭 | HbOff() |
| DrlOn | void | 方法 | value: int | 日行灯亮 | DrlOn(255) |
| DrlOff | void | 方法 | 无 | 日行灯灭 | DrlOff() |
| TurnLOn | void | 方法 | value: int | 左转向灯亮 | TurnLOn(1) |
| TurnROn | void | 方法 | value: int | 右转向灯亮 | TurnROn(1) |
| TurnOff | void | 方法 | 无 | 转向灯灭 | TurnOff() |
| PosOn | void | 方法 | 无 | 位置灯亮 | PosOn() |
| PosOff | void | 方法 | 无 | 位置灯灭 | PosOff() |