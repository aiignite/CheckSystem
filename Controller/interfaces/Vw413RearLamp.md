# Vw413RearLamp 控制器接口文档

## 控制器基本信息

| 控制器名称 | 控制器描述 |
|------------|------------|
| Vw413RearLamp | LIN-Product,VW413组合后灯-lin波特率19200 |

## 公共属性

| 名称 | 类型 | 类别 | 说明 | 示例 |
|------|------|------|------|------|
| Lin19200 | LinBus | 属性 | LIN总线 | - |

## 公共方法

| 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|------|------|------|------|------|------|
| LampAwake | void | 方法 | 无 | 唤醒 | LampAwake() |
| LampSleep | void | 方法 | 无 | 休眠 | LampSleep() |
| Kl15On | void | 方法 | 无 | KL15开关ON | Kl15On() |
| Kl15Off | void | 方法 | 无 | KL15开关OFF | Kl15Off() |
| StopOn | void | 方法 | 无 | 制动灯亮 | StopOn() |
| StopOff | void | 方法 | 无 | 制动灯灭 | StopOff() |
| Tail3DOn | void | 方法 | 无 | TAIL 3D亮 | Tail3DOn() |
| Tail3DOff | void | 方法 | 无 | TAIL 3D灭 | Tail3DOff() |
| TailGateOn | void | 方法 | 无 | 尾门信号开 | TailGateOn() |
| TailGateOff | void | 方法 | 无 | 尾门信号关 | TailGateOff() |
| RearLampOn | void | 方法 | 无 | 位置灯低亮 | RearLampOn() |
| RearLampHdOn | void | 方法 | 无 | 位置灯高亮开 | RearLampHdOn() |
| RearLampOff | void | 方法 | 无 | 位置灯灭 | RearLampOff() |
| BulLampOn | void | 方法 | 无 | 倒车灯亮 | BulLampOn() |
| BulLampOff | void | 方法 | 无 | 倒车灯灭 | BulLampOff() |
| FogLampOn | void | 方法 | 无 | 雾灯亮 | FogLampOn() |
| FogLampOff | void | 方法 | 无 | 雾灯灭 | FogLampOff() |
| LeftRearTurnRunningOn | void | 方法 | 无 | 左转向灯顺序亮 | LeftRearTurnRunningOn() |
| LeftRearTurnFlickerOn | void | 方法 | 无 | 左转向灯闪烁亮 | LeftRearTurnFlickerOn() |
| LeftRearTurnOff | void | 方法 | 无 | 左转向灯灭 | LeftRearTurnOff() |
| RightRearTurnRunningOn | void | 方法 | 无 | 右转向灯顺序亮 | RightRearTurnRunningOn() |
| RightRearTurnFlickerOn | void | 方法 | 无 | 右转向灯闪烁亮 | RightRearTurnFlickerOn() |
| RightRearTurnOff | void | 方法 | 无 | 右转向灯灭 | RightRearTurnOff() |
| LevingHome | void | 方法 | 无 | 离家动画 | LevingHome() |
| ComingHome | void | 方法 | 无 | 回家动画 | ComingHome() |
| LeavingHomeTriggerBySchlusslichtSignatur | void | 方法 | type: int | 离家动画-由SchlusslichtSignatur信号触发 | LeavingHomeTriggerBySchlusslichtSignatur(1) |
| ComingHomeTriggerBySchlusslichtSignatur | void | 方法 | type: int | 回家动画-由SchlusslichtSignatur信号触发 | ComingHomeTriggerBySchlusslichtSignatur(1) |