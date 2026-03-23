# Sgm458RearLamp 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: LIN-Product,SGM458后灯

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| Sgm458RearLamp | LIN唤醒 | LampAwake | void | 方法 | - | LIN唤醒 | LampAwake() |
| Sgm458RearLamp | LIN休眠 | LampSleep | void | 方法 | - | LIN休眠 | LampSleep() |
| Sgm458RearLamp | Tail-NoAction-灭 | TailNoAction | void | 方法 | - | Tail-NoAction-灭 | TailNoAction() |
| Sgm458RearLamp | Tail-NormalOn-低亮 | TailNormalOn | void | 方法 | - | Tail-NormalOn-低亮 | TailNormalOn() |
| Sgm458RearLamp | Tail-HighlightOn-高亮 | TailHighlightOn | void | 方法 | - | Tail-HighlightOn-高亮 | TailHighlightOn() |
| Sgm458RearLamp | Turn-NotActive-灭 | TurnNotActive | void | 方法 | - | Turn-NotActive-灭 | TurnNotActive() |
| Sgm458RearLamp | Turn-NormalFlash | TurnNormalFlash | void | 方法 | - | Turn-NormalFlash | TurnNormalFlash() |
| Sgm458RearLamp | Turn-SwipeTurn-低亮 | TurnSwipeTurn | void | 方法 | - | Turn-SwipeTurn-低亮 | TurnSwipeTurn() |
| Sgm458RearLamp | Turn-Active1-高亮 | TurnActive1 | void | 方法 | - | Turn-Active1-高亮 | TurnActive1() |
| Sgm458RearLamp | 简单解锁 | SimpleLeavingHome | void | 方法 | - | 简单解锁 | SimpleLeavingHome() |
| Sgm458RearLamp | 简单闭锁 | SimpleComingHome | void | 方法 | - | 简单闭锁 | SimpleComingHome() |
| Sgm458RearLamp | 复杂解锁 | ComplexLeavingHome | void | 方法 | - | 复杂解锁 | ComplexLeavingHome() |
| Sgm458RearLamp | 复杂闭锁 | ComplexComingHome | void | 方法 | - | 复杂闭锁 | ComplexComingHome() |
| Sgm458RearLamp | Follow Me Home | FollowMeHome | void | 方法 | - | Follow Me Home | FollowMeHome() |
| Sgm458RearLamp | 打断动画 | LampShowAbort | void | 方法 | - | 打断动画 | LampShowAbort() |
| Sgm458RearLamp | R,HASCO从节点软件版本号 | HASCO从节点软件版本号 | string | 字段 | - | HASCO从节点软件版本号 | - |
| Sgm458RearLamp | 读HASCO从节点软件版本号 | ReadHASCO从节点软件版本号 | void | 方法 | string nad | 读HASCO从节点软件版本号 | ReadHASCO从节点软件版本号("01") |
