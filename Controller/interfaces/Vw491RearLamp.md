# Vw491RearLamp 控制器接口文档

## 控制器基本信息

| 控制器名称 | 控制器描述 |
|------------|------------|
| Vw491RearLamp | LIN-Product,VW491组合后灯-lin波特率19200 |

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
| Tail3DOn | void | 方法 | 无 | TAIL 3D亮 | Tail3DOn() |
| Tail3DOff | void | 方法 | 无 | TAIL 3D灭 | Tail3DOff() |
| StopOn | void | 方法 | 无 | 制动灯亮 | StopOn() |
| StopOff | void | 方法 | 无 | 制动灯灭 | StopOff() |
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
| AllLedOff | void | 方法 | 无 | 关闭所有灯光并打断动画 | AllLedOff() |
| SetRightRcla | void | 方法 | 无 | 设置为RIGHT RCLA | SetRightRcla() |
| SetLeftRcla | void | 方法 | 无 | 设置为LEFT RCLA | SetLeftRcla() |
| SetRightRclb | void | 方法 | 无 | 设置为RIGHT RCLB | SetRightRclb() |
| SetRigthRclb | void | 方法 | 无 | 设置为LEFT RCLB | SetRigthRclb() |
| ReadVer | void | 方法 | 无 | ReadVer | ReadVer() |