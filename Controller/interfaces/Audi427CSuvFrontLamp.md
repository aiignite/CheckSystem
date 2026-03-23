# Audi427CSuvFrontLamp

## Controller Description

CAN-Product,427C-SUV前组合灯

## Public Fields

| 名称 | 描述 | 类型 |
|------|------|------|
| NowMode | R,当前模式 | Mode |
| LeftCL | R/W,CL_L_PWM(0~63) | byte |
| RightCL | R/W,CL_R_PWM(0~63) | byte |

## Public Methods

| 方法名称 | 描述 | 参数 |
|----------|------|------|
| StartCan | 打开CAN |  |
| StopCan | 关闭CAN |  |
| NormalLampMode | 前灯模式 |  |
| DlpOn | DLP打开 |  |
| DlpOff | DLP关闭 |  |
| LbOn | 近光打开 |  |
| LbOff | 近光关闭 |  |
| HbOnL | 远光打开L |  |
