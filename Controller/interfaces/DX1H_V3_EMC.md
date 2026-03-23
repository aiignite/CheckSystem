# DX1H_V3_EMC 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: Can-Product,DX1H_V3

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| DX1H_V3_EMC | Can-Product,DX1H_V3 | KL30_1Voltage | float | 字段 | - | KL30_1电压 | - |
| DX1H_V3_EMC | Can-Product,DX1H_V3 | KL30_2Voltage | float | 字段 | - | KL30_2电压 | - |
| DX1H_V3_EMC | Can-Product,DX1H_V3 | KL30_3Voltage | float | 字段 | - | KL30_3电压 | - |
| DX1H_V3_EMC | Can-Product,DX1H_V3 | NTC1Temperature | float | 字段 | - | NTC1温度值 | - |
| DX1H_V3_EMC | Can-Product,DX1H_V3 | NTC2Temperature | float | 字段 | - | NTC2温度值 | - |
| DX1H_V3_EMC | Can-Product,DX1H_V3 | SecRIHotCurrent | float | 字段 | - | 坐垫加热电流值 | - |
| DX1H_V3_EMC | Can-Product,DX1H_V3 | BackrestSeatHotCurrent | float | 字段 | - | 靠背加热电流值 | - |
| DX1H_V3_EMC | Can-Product,DX1H_V3 | Mot2Current | float | 字段 | - | 电机2电流值 | - |
| DX1H_V3_EMC | Can-Product,DX1H_V3 | Mot4Current | float | 字段 | - | 电机4电流值 | - |
| DX1H_V3_EMC | Can-Product,DX1H_V3 | Mot6Current | float | 字段 | - | 电机6电流值 | - |
| DX1H_V3_EMC | Can-Product,DX1H_V3 | Mot8Current | float | 字段 | - | 电机8电流值 | - |
| DX1H_V3_EMC | Can-Product,DX1H_V3 | BackrestSeatHotPWM | float | 字段 | - | 靠背加热PWM | - |
| DX1H_V3_EMC | Can-Product,DX1H_V3 | SecRIHotPWM | float | 字段 | - | 坐垫加热PWM | - |
| DX1H_V3_EMC | Can-Product,DX1H_V3 | Mot2Hall | float | 字段 | - | 电机2霍尔值 | - |
| DX1H_V3_EMC | Can-Product,DX1H_V3 | Mot4Hall | float | 字段 | - | 电机4霍尔值 | - |
| DX1H_V3_EMC | Can-Product,DX1H_V3 | Mot6Hall | float | 字段 | - | 电机6霍尔值 | - |
| DX1H_V3_EMC | Can-Product,DX1H_V3 | Mot8Hall | float | 字段 | - | 电机8霍尔值 | - |
| DX1H_V3_EMC | Can-Product,DX1H_V3 | DPCState | float | 字段 | - | DPC状态 | - |
| DX1H_V3_EMC | Can-Product,DX1H_V3 | KL30TestState | bool | 字段 | - | 开关 | - |
| DX1H_V3_EMC | Can-Product,DX1H_V3 | Msg171 | string | 字段 | - | 0x171报文 | - |
| DX1H_V3_EMC | Can-Product,DX1H_V3 | Msg149 | string | 字段 | - | 0x149报文 | - |
| DX1H_V3_EMC | Can-Product,DX1H_V3 | Msg120 | string | 字段 | - | 0x120报文 | - |
| DX1H_V3_EMC | Can-Product,DX1H_V3 | IsCanLoss | bool | 字段 | - | CAN丢失 | - |
| DX1H_V3_EMC | Can-Product,DX1H_V3 | StartScheduler | void | 方法 | - | 打开Can发送 | - |
| DX1H_V3_EMC | Can-Product,DX1H_V3 | StopScheduler | void | 方法 | - | 关闭Can发送 | - |
| DX1H_V3_EMC | Can-Product,DX1H_V3 | Heat100OnMotorOn | void | 方法 | - | 100%加热电机开 | - |
| DX1H_V3_EMC | Can-Product,DX1H_V3 | Heat100OnMotorOff | void | 方法 | - | 100%加热电机关 | - |
| DX1H_V3_EMC | Can-Product,DX1H_V3 | Heat50OnMotorOn | void | 方法 | - | 50%加热电机开 | - |
| DX1H_V3_EMC | Can-Product,DX1H_V3 | Heat50OnMotorOff | void | 方法 | - | 50%加热电机关 | - |
| DX1H_V3_EMC | Can-Product,DX1H_V3 | AllOff | void | 方法 | - | 全部关闭 | - |
