# 控制器文档

## L234LoadBox

### 控制器描述
LIN-Product,L234-LoadBox

### 字段

| 名称 | 类型 | 类别 | 说明 |
|------|------|------|------|
| Read0X01Str | string | R | 读0x01结果 |
| Read0X02Str | string | R | 读0x02结果 |
| 电源电压 | string | R | 电源电压 |
| 右侧电机霍尔频率 | string | R | 右侧电机霍尔频率 |
| 左侧电机霍尔频率 | string | R | 左侧电机霍尔频率 |
| 右侧开关电压 | string | R | 右侧开关电压 |
| 左侧开关电压 | string | R | 左侧开关电压 |
| 右侧电机电流 | string | R | 右侧电机电流 |
| 左侧电机电流 | string | R | 左侧电机电流 |
| 左侧开关状态 | string | R | 左侧开关状态 |
| 右侧开关状态 | string | R | 右侧开关状态 |
| Vcc1_5V电压 | string | R | VCC1_5V电压 |

### 方法

| 名称 | 参数 | 说明 | 示例 |
|------|------|------|------|
| Motor100PerRun |  | 电机100%运行 |  |
| MotorStandby |  | 电机待机模式 |  |
| MotorSleep |  | 电机休眠模式 |  |
| StartAutoScanStatus |  | 开启自动读取状态 |  |
| StopAutoScanStatus |  | 停止自动读取状态 |  |
| ReadStateMessage |  | 读取状态信息 |  |

