# 控制器文档

## HvsmEmcModule

### 控制器描述
LIN-Product,HVSM-EMC版本

### 字段

| 名称 | 类型 | 类别 | 说明 |
|------|------|------|------|
| IsLinLoss | bool | R | LIN丢失 |
| HSD1_Current | double | R | HSD1_Current |
| HSD2_Current | double | R | HSD2_Current |
| HSD3_Current | double | R | HSD3_Current |
| HSD4_Current | double | R | HSD4_Current |
| Vbat1 | double | R | VBAT1 |
| Vbat2 | double | R | VBAT2 |
| Ntc1 | double | R | NTC1 |
| Ntc2 | double | R | NTC2 |
| Ntc3 | double | R | NTC3 |
| Ntc4 | double | R | NTC4 |
| Input | string | R | INPUT |
| Msg0x10 | string | R | VbatNTC12_Status_0x10 |
| Msg0x11 | string | R | HSD1234_Current_Status_0x11 |
| Msg0x12 | string | R | HSD1234_Fault_NTC1234_Fault_Status_0x12 |
| Msg0x13 | string | R | FAN12_Fault_NTC34_Status_0x13 |

### 方法

| 名称 | 参数 | 说明 | 示例 |
|------|------|------|------|
| StartLin |  | 打开LIN |  |
| StopLin |  | 关闭LIN |  |
| SendSleepCmd |  | 发送休眠指令 |  |
| HsdDuty | index, duty | 设置高边占空比 |  |
| HsdFreq | index, freq | 设置高边频率 |  |
| FanDuty | index, duty | 设置风扇占空比 |  |

