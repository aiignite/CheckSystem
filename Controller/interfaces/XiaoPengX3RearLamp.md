# XiaoPengX3RearLamp 控制器接口文档

## 控制器基本信息

| 控制器名称 | 控制器描述 |
|------------|------------|
| XiaoPengX3RearLamp | CAN-Product,小鹏X3尾灯 |

## 公共属性

| 名称 | 类型 | 类别 | 说明 | 示例 |
|------|------|------|------|------|
| Can | CanBus | 属性 | CAN总线 | - |
| IsLConnect | bool | 属性 | R/W,是否连接L | - |
| IsRConnect | bool | 属性 | R/W,是否连接R | - |

## 公共方法

| 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|------|------|------|------|------|------|
| StartCanMsg | void | 方法 | 无 | 打开CAN消息 | StartCanMsg() |
| StopCanMsg | void | 方法 | 无 | 关闭CAN消息 | StopCanMsg() |
| PosOn | void | 方法 | 无 | 位置灯亮 | PosOn() |
| PosOff | void | 方法 | 无 | 位置灯灭 | PosOff() |
| TurnLOn | void | 方法 | value: int | 左转向灯亮 | TurnLOn(1) |
| TurnROn | void | 方法 | value: int | 右转向灯亮 | TurnROn(1) |
| TurnOff | void | 方法 | 无 | 转向灯灭 | TurnOff() |
| StopOn | void | 方法 | 无 | 制动灯亮 | StopOn() |
| StopOff | void | 方法 | 无 | 制动灯灭 | StopOff() |
| RedOn | void | 方法 | value: int | 红灯亮 | RedOn(1) |
| RedOff | void | 方法 | 无 | 红灯灭 | RedOff() |
| YellowOn | void | 方法 | value: int | 黄灯亮 | YellowOn(1) |
| YellowOff | void | 方法 | 无 | 黄灯灭 | YellowOff() |
| PosTurnLOn | void | 方法 | value: int | 位置左转 | PosTurnLOn(1) |
| PosTurnROn | void | 方法 | value: int | 位置右转 | PosTurnROn(1) |