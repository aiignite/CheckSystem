# S59HeadLampBox 控制器接口文档

## 控制器信息

| 控制器名称 | 控制器描述 |
|-----------|-----------|
| S59HeadLampBox | S59前灯箱 |

## 公共字段

| 名称 | 类型 | 类别 | 描述 |
|------|------|------|------|
| IsLbOn | bool | 读 | R,近光是否打开 |
| IsHbOn | bool | 读 | R,远光是否打开 |
| IsTurnOn | bool | 读 | R,转向是否打开 |
| IsDrlOn | bool | 读 | R,DRL是否打开 |
| IsPlOn | bool | 读 | R,PL是否打开 |
| CurrentLightMode | string | 读 | R,当前模式 |
| CurrentAnimation | string | 读 | R,当前动画 |
| CurrentAnimationDuration | string | 读 | R,当前动画剩余/S |
| Lin10X13Show | string | 读 | R,LIN1_0x13数据 |
| Lin1LShow | string | 读 | R,LIN1左灯数据 |
| Lin1RShow | string | 读 | R,LIN1右灯数据 |
| Lin2LShow | string | 读 | R,LIN2左灯数据 |
| Lin2RShow | string | 读 | R,LIN2右灯数据 |
| Lin20X13Show | string | 读 | R,LIN2_0x13数据 |
| Lin1LTurnState | string | 读 | R,LIN1左转向状态 |
| Lin1RTurnState | string | 读 | R,LIN1右转向状态 |
| Lin2LTurnState | string | 读 | R,LIN2左转向状态 |
| Lin2RTurnState | string | 读 | R,LIN2右转向状态 |

## 公共方法

| 名称 | 参数 | 类型 | 说明 | 示例 |
|------|------|------|------|------|
| LbOn | 无 | 方法 | 近光开 | |
| LbOff | 无 | 方法 | 近光关 | |
| HbOn | 无 | 方法 | 远光开 | |
| HbOff | 无 | 方法 | 远光关 | |
| DrlOn | 无 | 方法 | 日行灯开 | |
| DrlOff | 无 | 方法 | 日行灯关 | |
| PlOn | 无 | 方法 | 位置灯开 | |
| PlOff | 无 | 方法 | 位置灯关 | |
| TurnOn | 无 | 方法 | 转向灯开 | |
| TurnLOn | 无 | 方法 | 转向灯L开 | |
| TurnROn | 无 | 方法 | 转向灯R开 | |
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
