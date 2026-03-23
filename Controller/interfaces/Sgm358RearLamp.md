# Sgm358RearLamp 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: LIN-Product,SGM358后灯

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| Sgm358RearLamp | LIN-Product,SGM358后灯 | LinWithBaudRate10417 | LinBus | 字段 | - | LIN总线 | - |
| Sgm358RearLamp | LIN-Product,SGM358后灯 | LampAwake | void | 方法 | - | LIN唤醒 | LampAwake() |
| Sgm358RearLamp | LIN-Product,SGM358后灯 | LampSleep | void | 方法 | - | LIN休眠 | LampSleep() |
| Sgm358RearLamp | LIN-Product,SGM358后灯 | LeftRearTurnLampHoldOn | void | 方法 | - | 左后转向灯常亮 | LeftRearTurnLampHoldOn() |
| Sgm358RearLamp | LIN-Product,SGM358后灯 | LeftRearTurnLampOff | void | 方法 | - | 左后转向灯熄灭 | LeftRearTurnLampOff() |
| Sgm358RearLamp | LIN-Product,SGM358后灯 | LeftRearTurnLampSwipeOn | void | 方法 | - | 左后转向灯时序点亮 | LeftRearTurnLampSwipeOn() |
| Sgm358RearLamp | LIN-Product,SGM358后灯 | LeftRearTurnLampNormalFlashOn | void | 方法 | - | 左后转向灯1.33hz正常频率闪烁点亮 | LeftRearTurnLampNormalFlashOn() |
| Sgm358RearLamp | LIN-Product,SGM358后灯 | LeftRearTurnLampNormalFlash1HzOn | void | 方法 | - | 左后转向灯1hz频率闪烁点亮 | LeftRearTurnLampNormalFlash1HzOn() |
| Sgm358RearLamp | LIN-Product,SGM358后灯 | LeftRearTurnLampNormalFlashMode3On | void | 方法 | - | 左后转向灯3hz频率闪烁点亮 | LeftRearTurnLampNormalFlashMode3On() |
| Sgm358RearLamp | LIN-Product,SGM358后灯 | LeftRearTurnLampNormalFlashMode3Off | void | 方法 | - | 左后转向灯3hz频率闪烁熄灭 | LeftRearTurnLampNormalFlashMode3Off() |
| Sgm358RearLamp | LIN-Product,SGM358后灯 | LeftRearTurnLampFaultFlashOn | void | 方法 | - | 左后转向灯故障闪烁点亮 | LeftRearTurnLampFaultFlashOn() |
| Sgm358RearLamp | LIN-Product,SGM358后灯 | RightRearTurnLampHoldOn | void | 方法 | - | 右后转向灯常亮 | RightRearTurnLampHoldOn() |
| Sgm358RearLamp | LIN-Product,SGM358后灯 | RightRearTurnLampOff | void | 方法 | - | 右后转向灯熄灭 | RightRearTurnLampOff() |
| Sgm358RearLamp | LIN-Product,SGM358后灯 | RightRearTurnLampSwipeOn | void | 方法 | - | 右后转向灯时序点亮 | RightRearTurnLampSwipeOn() |
| Sgm358RearLamp | LIN-Product,SGM358后灯 | RightRearTurnLampNormalFlashOn | void | 方法 | - | 右后转向灯1.33hz正常频率闪烁点亮 | RightRearTurnLampNormalFlashOn() |
| Sgm358RearLamp | LIN-Product,SGM358后灯 | RightRearTurnLampNormalFlash1HzOn | void | 方法 | - | 右后转向灯1hz频率闪烁点亮 | RightRearTurnLampNormalFlash1HzOn() |
| Sgm358RearLamp | LIN-Product,SGM358后灯 | RightRearTurnLampNormalFlashMode3On | void | 方法 | - | 右后转向灯3hz频率闪烁点亮 | RightRearTurnLampNormalFlashMode3On() |
| Sgm358RearLamp | LIN-Product,SGM358后灯 | RightRearTurnLampFaultFlashOn | void | 方法 | - | 右后转向灯故障闪烁点亮 | RightRearTurnLampFaultFlashOn() |
| Sgm358RearLamp | LIN-Product,SGM358后灯 | RightRearParkLampOn | void | 方法 | - | 右尾灯打开 | RightRearParkLampOn() |
| Sgm358RearLamp | LIN-Product,SGM358后灯 | RightRearParkLampOff | void | 方法 | - | 右尾灯关闭 | RightRearParkLampOff() |
| Sgm358RearLamp | LIN-Product,SGM358后灯 | LeftRearParkLampOn | void | 方法 | - | 左尾灯打开 | LeftRearParkLampOn() |
| Sgm358RearLamp | LIN-Product,SGM358后灯 | LeftRearParkLampOff | void | 方法 | - | 左尾灯关闭 | LeftRearParkLampOff() |
| Sgm358RearLamp | LIN-Product,SGM358后灯 | RrLmpShwNotActive | void | 方法 | - | 默认值等待或开始动画模式 | RrLmpShwNotActive() |
| Sgm358RearLamp | LIN-Product,SGM358后灯 | RrLmpShwFactoryMode | void | 方法 | - | 工厂测试模式 | RrLmpShwFactoryMode() |
| Sgm358RearLamp | LIN-Product,SGM358后灯 | RrLampShowTypNotActive | void | 方法 | - | 默认值，等待灯光秀开始或结束 | RrLampShowTypNotActive() |
| Sgm358RearLamp | LIN-Product,SGM358后灯 | RrLampShowTypAbortShow | void | 方法 | - | 停止或打断灯光秀 | RrLampShowTypAbortShow() |
| Sgm358RearLamp | LIN-Product,SGM358后灯 | RrLampShowTypEngineOffDay | void | 方法 | - | 白天引擎关 | RrLampShowTypEngineOffDay() |
| Sgm358RearLamp | LIN-Product,SGM358后灯 | RrLmpShwTypEngineOffNight | void | 方法 | - | 晚上引擎关 | RrLmpShwTypEngineOffNight() |
| Sgm358RearLamp | LIN-Product,SGM358后灯 | RrLmpShwTypEngineOn | void | 方法 | - | 引擎开 | RrLmpShwTypEngineOn() |
| Sgm358RearLamp | LIN-Product,SGM358后灯 | RrLmpShwTypUnlockDay | void | 方法 | - | 白天解锁 | RrLmpShwTypUnlockDay() |
| Sgm358RearLamp | LIN-Product,SGM358后灯 | RrLmpShwTypUnlockNight | void | 方法 | - | 晚上解锁 | RrLmpShwTypUnlockNight() |
| Sgm358RearLamp | LIN-Product,SGM358后灯 | RrLmpShwTypUnlockNightOnlyLmc | void | 方法 | - | LMC下晚上解锁 | RrLmpShwTypUnlockNightOnlyLmc() |
| Sgm358RearLamp | LIN-Product,SGM358后灯 | RrLmpShwTypLockEngineOffDay | void | 方法 | - | 白天锁车且引擎关 | RrLmpShwTypLockEngineOffDay() |
