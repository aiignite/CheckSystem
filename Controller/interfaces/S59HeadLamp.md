# S59HeadLamp 控制器接口文档

## 控制器信息

| 控制器名称 | 控制器描述 |
|-----------|-----------|
| S59HeadLamp | LIN-Product,S59前灯 |

## 公共字段

| 名称 | 类型 | 类别 | 描述 |
|------|------|------|------|
| IsLbOn | bool | 读 | R,近光是否打开 |
| IsHbOn | bool | 读 | R,远光是否打开 |
| IsTurnOn | bool | 读 | R,转向是否打开 |
| IsTurnFlash | bool | 读 | R,转向闪烁是否打开 |
| IsDrlOn | bool | 读 | R,DRL是否打开 |
| IsPlOn | bool | 读 | R,PL是否打开 |
| CurrentLightMode | string | 读 | R,当前模式 |
| CurrentAnimation | string | 读 | R,当前动画 |
| CurrentAnimationDuration | string | 读 | R,当前动画剩余/S |
| TurnFlashMaxCount | int | 读/写 | R/W,TurnFlashMaxCount |

## 公共方法

| 名称 | 参数 | 类型 | 说明 | 示例 |
|------|------|------|------|------|
| Sleep | 无 | 方法 | 休眠 | |
| Awake | 无 | 方法 | 唤醒 | |
| LbOn | 无 | 方法 | 近光开 | |
| LbOff | 无 | 方法 | 近光关 | |
| HbOn | 无 | 方法 | 远光开 | |
| HbOff | 无 | 方法 | 远光关 | |
| DrlOn | 无 | 方法 | 日行灯开 | |
| DrlOff | 无 | 方法 | 日行灯关 | |
| PlOn | 无 | 方法 | 位置灯开 | |
| PlOff | 无 | 方法 | 位置灯关 | |
| TurnOn | 无 | 方法 | 转向灯开 | |
| TurnFlashOn | 无 | 方法 | 转向灯闪烁开 | |
| TurnOff | 无 | 方法 | 转向灯关 | |
| UnLockWelcomeLampReq1 | 无 | 方法 | 解锁动态迎宾请求1 | |
| UnLockWelcomeLampReq2 | 无 | 方法 | 解锁动态迎宾请求2 | |
| UnLockWelcomeLampReq3 | 无 | 方法 | 解锁动态迎宾请求3 | |
| UnLockWelcomeLampReq4 | 无 | 方法 | 解锁动态迎宾请求4 | |
| UnLockWelcomeLampReq5 | 无 | 方法 | 解锁动态迎宾请求5 | |
| LockWelcomeLampReq1 | 无 | 方法 | 闭锁动态迎宾请求1 | |
| LockWelcomeLampReq2 | 无 | 方法 | 闭锁动态迎宾请求2 | |
| ResetWelcomLampReq | 无 | 方法 | 关闭迎宾请求 | |
| StartSpecialMode | index: string | 方法 | 打开特定模式 | StartSpecialMode("1") |
| StopSpecialMode | 无 | 方法 | 关闭特定模式 | |
