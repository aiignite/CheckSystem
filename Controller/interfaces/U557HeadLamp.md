# U557HeadLamp 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: LIN-Product,U557前灯

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| U557HeadLamp | LIN-Product,U557前灯 | LinWithBaudRate10417 | LinBus | 字段 | - | LIN总线 | - |
| U557HeadLamp | LIN-Product,U557前灯 | LeftBoot | string | 字段 | - | R,LeftBoot | - |
| U557HeadLamp | LIN-Product,U557前灯 | RightBoot | string | 字段 | - | R,RightBoot | - |
| U557HeadLamp | LIN-Product,U557前灯 | LeftApp | string | 字段 | - | R,LeftApp | - |
| U557HeadLamp | LIN-Product,U557前灯 | RightApp | string | 字段 | - | R,RightApp | - |
| U557HeadLamp | LIN-Product,U557前灯 | LinStartScheduler | void | 方法 | - | 打开LIN发送 | - |
| U557HeadLamp | LIN-Product,U557前灯 | LinStopScheduler | void | 方法 | - | 关闭LIN发送 | - |
| U557HeadLamp | LIN-Product,U557前灯 | EmcMode | void | 方法 | string modeIndex | EMC_MODE | - |
| U557HeadLamp | LIN-Product,U557前灯 | EmcSleep | void | 方法 | - | EMC_MODE5_SLEEP | - |
| U557HeadLamp | LIN-Product,U557前灯 | LeftParkLampOff | void | 方法 | - | L_位置灯OFF | - |
| U557HeadLamp | LIN-Product,U557前灯 | LeftParkLampOn | void | 方法 | int value | L_位置灯根据信号打开 | - |
| U557HeadLamp | LIN-Product,U557前灯 | LeftParkLamp1 | void | 方法 | - | L_位置灯信号1 | - |
| U557HeadLamp | LIN-Product,U557前灯 | LeftParkLamp7 | void | 方法 | - | L_位置灯信号7 | - |
| U557HeadLamp | LIN-Product,U557前灯 | RightParkLampOff | void | 方法 | - | R_位置灯OFF | - |
| U557HeadLamp | LIN-Product,U557前灯 | RightParkLampOn | void | 方法 | int value | R_位置灯根据信号打开 | - |
| U557HeadLamp | LIN-Product,U557前灯 | RightParkLamp1 | void | 方法 | - | R_位置灯信号1 | - |
| U557HeadLamp | LIN-Product,U557前灯 | RightParkLamp7 | void | 方法 | - | R_位置灯信号7 | - |
| U557HeadLamp | LIN-Product,U557前灯 | LeftTurnOff | void | 方法 | - | L_转向灯OFF | - |
| U557HeadLamp | LIN-Product,U557前灯 | LeftTurnOn | void | 方法 | int value | L_转向灯根据信号打开 | - |
| U557HeadLamp | LIN-Product,U557前灯 | LeftTurnLamp1 | void | 方法 | - | L_转向灯信号1 | - |
| U557HeadLamp | LIN-Product,U557前灯 | LeftTurnLamp2 | void | 方法 | - | L_转向灯信号2 | - |
| U557HeadLamp | LIN-Product,U557前灯 | LeftTurnLamp3 | void | 方法 | - | L_转向灯信号3 | - |
| U557HeadLamp | LIN-Product,U557前灯 | LeftTurnLamp4 | void | 方法 | - | L_转向灯信号4 | - |
| U557HeadLamp | LIN-Product,U557前灯 | RightTurnOff | void | 方法 | - | R_转向灯OFF | - |
| U557HeadLamp | LIN-Product,U557前灯 | RightTurnOn | void | 方法 | int value | R_转向灯根据信号打开 | - |
| U557HeadLamp | LIN-Product,U557前灯 | RightTurnLamp1 | void | 方法 | - | R_转向灯信号1 | - |
| U557HeadLamp | LIN-Product,U557前灯 | RightTurnLamp2 | void | 方法 | - | R_转向灯信号2 | - |
| U557HeadLamp | LIN-Product,U557前灯 | RightTurnLamp3 | void | 方法 | - | R_转向灯信号3 | - |
| U557HeadLamp | LIN-Product,U557前灯 | RightTurnLamp4 | void | 方法 | - | R_转向灯信号4 | - |
| U557HeadLamp | LIN-Product,U557前灯 | RrLmpShwMod_64_5 | void | 方法 | string mod | RrLmpShwMod_64_5 | - |
| U557HeadLamp | LIN-Product,U557前灯 | RrLmpShwTyp_64_5 | void | 方法 | string typ | RrLmpShwTyp_64_5 | - |
| U557HeadLamp | LIN-Product,U557前灯 | SendCorrectSleepCmd | void | 方法 | - | 发送正确休眠命令 | - |
| U557HeadLamp | LIN-Product,U557前灯 | SendIncorrectSleepCmd | void | 方法 | - | 发送错误休眠命令 | - |
| U557HeadLamp | LIN-Product,U557前灯 | LeftReset | void | 方法 | - | 左复位 | - |
| U557HeadLamp | LIN-Product,U557前灯 | RightReset | void | 方法 | - | 右复位 | - |
| U557HeadLamp | LIN-Product,U557前灯 | ReadLeftBoot | void | 方法 | - | 读左BOOT | - |
| U557HeadLamp | LIN-Product,U557前灯 | ReadLeftApp | void | 方法 | - | 读左APP | - |
| U557HeadLamp | LIN-Product,U557前灯 | ReadRightBoot | void | 方法 | - | 读右BOOT | - |
| U557HeadLamp | LIN-Product,U557前灯 | ReadRightApp | void | 方法 | - | 读右APP | - |
