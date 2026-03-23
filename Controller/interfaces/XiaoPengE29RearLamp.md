# XiaoPengE29RearLamp 控制器接口文档

## 控制器基本信息

| 控制器名称 | 控制器描述 |
|------------|------------|
| XiaoPengE29RearLamp | CAN-Product,小鹏E29尾灯 |

## 公共属性

| 名称 | 类型 | 类别 | 说明 | 示例 |
|------|------|------|------|------|
| Can | CanBus | 属性 | CAN总线 | - |
| MRL_RLParkinglightOutputSt | string | 属性 | 左后位置灯输出状态信号 | - |
| MRL_RLParkinglightFailureSt | string | 属性 | 左后位置灯故障信号 | - |
| MRL_RLTurnLampFailureSt | string | 属性 | 左后转向灯故障信号 | - |
| MRL_RLTurnLampOutputSt | string | 属性 | 左后转向灯输出显示状态信号 | - |
| MRL_RRParkinglightOutputSt | string | 属性 | 右后位置灯输出状态信号 | - |
| MRL_RRParkinglightFailureSt | string | 属性 | 右后位置灯故障信号 | - |
| MRL_RRTurnLampOutputSt | string | 属性 | 右后转向灯输出显示状态信号 | - |
| MRL_RRTurnLampFailureSt | string | 属性 | 右后转向灯故障信号 | - |
| FaultRead | string | 属性 | R,故障信息读取 | - |
| SoftwareVer | string | 属性 | R,SoftwareVer | - |

## 公共方法

| 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|------|------|------|------|------|------|
| StartCan | void | 方法 | 无 | 打开CAN消息 | StartCan() |
| StopCan | void | 方法 | 无 | 关闭CAN消息 | StopCan() |
| TailOn | void | 方法 | 无 | TAIL ON | TailOn() |
| TailHdOn | void | 方法 | 无 | TAIL HD ON | TailHdOn() |
| TailOff | void | 方法 | 无 | TAIL OFF | TailOff() |
| TurnLeftOn | void | 方法 | 无 | TURN LEFT ON | TurnLeftOn() |
| TurnLeftLineOn | void | 方法 | 无 | TURN LEFT LINE ON | TurnLeftLineOn() |
| TurnLeftOff | void | 方法 | 无 | TURN LEFT OFF | TurnLeftOff() |
| TurnRightOn | void | 方法 | 无 | TURN RIGHT ON | TurnRightOn() |
| TurnRightLineOn | void | 方法 | 无 | TURN RIGHT LINE ON | TurnRightLineOn() |
| TurnRightOff | void | 方法 | 无 | TURN RIGHT OFF | TurnRightOff() |
| StopOn | void | 方法 | 无 | STOP ON | StopOn() |
| StopOff | void | 方法 | 无 | STOP OFF | StopOff() |
| OpenLampSignalMode | void | 方法 | 无 | 打开灯语模式 | OpenLampSignalMode() |
| CloseLampSignalMode | void | 方法 | 无 | 关闭灯语模式 | CloseLampSignalMode() |
| OpenSingleLed | void | 方法 | index: int, value: byte | 打开单颗LED | OpenSingleLed(1, 0x0A) |
| CloseSingleLed | void | 方法 | index: int | 关闭单颗LED | CloseSingleLed(1) |
| OpenAllLed | void | 方法 | value: byte | 打开所有LED | OpenAllLed(0x0A) |
| OpenOddLed | void | 方法 | value: byte | 打开奇数LED | OpenOddLed(0x0A) |
| OpenEvenLed | void | 方法 | value: byte | 打开偶数LED | OpenEvenLed(0x0A) |
| CloseAllLed | void | 方法 | 无 | 关闭所有LED | CloseAllLed() |
| StartRunning | void | 方法 | value: byte | 打开跑马效果 | StartRunning(0x05) |
| StopRunning | void | 方法 | 无 | 关闭跑马效果 | StopRunning() |
| FaultDetect | void | 方法 | 无 | 读取故障信息 | FaultDetect() |
| SoftwareVerRead | void | 方法 | 无 | 读取软件版本信息 | SoftwareVerRead() |