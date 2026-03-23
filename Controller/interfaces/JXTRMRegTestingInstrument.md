# 控制器文档

## JXTRMRegTestingInstrument

### 控制器描述
无

### 字段

| 名称 | 类型 | 类别 | 说明 |
|------|------|------|------|
| BusyWaitingMs | int | R/W | State为busy时的等待时间 |
| CommunicationTimeoutMs | int | R/W | 通讯超时时间 |
| CurrentMaxReadRegCount | int | R/W | 当前最大读取电阻数量 |
| R1 | double | R | DEV1-R1 |
| R2 | double | R | DEV1-R2 |
| R3 | double | R | DEV1-R3 |
| R4 | double | R | DEV1-R4 |
| R5 | double | R | DEV1-R5 |
| R6 | double | R | DEV1-R6 |
| R7 | double | R | DEV1-R7 |
| R8 | double | R | DEV1-R8 |
| R9 | double | R | DEV1-R9 |
| R10 | double | R | DEV1-R10 |
| R11 | double | R | DEV1-R11 |
| R12 | double | R | DEV1-R12 |
| R13 | double | R | DEV1-R13 |
| R14 | double | R | DEV1-R14 |
| R15 | double | R | DEV1-R15 |
| R16 | double | R | DEV1-R16 |
| R17 | double | R | DEV1-R17 |
| R18 | double | R | DEV1-R18 |
| R19 | double | R | DEV1-R19 |
| R20 | double | R | DEV1-R20 |
| R21 | double | R | DEV1-R21 |
| R22 | double | R | DEV1-R22 |
| R23 | double | R | DEV1-R23 |
| R24 | double | R | DEV1-R24 |
| R25 | double | R | DEV1-R25 |
| R26 | double | R | DEV1-R26 |
| R27 | double | R | DEV1-R27 |
| R28 | double | R | DEV1-R28 |
| R29 | double | R | DEV1-R29 |
| R30 | double | R | DEV1-R30 |
| RR1 | double | R | DEV2-RR1 |
| RR2 | double | R | DEV2-RR2 |
| RR3 | double | R | DEV2-RR3 |
| RR4 | double | R | DEV2-RR4 |
| RR5 | double | R | DEV2-RR5 |
| RR6 | double | R | DEV2-RR6 |
| RR7 | double | R | DEV2-RR7 |
| RR8 | double | R | DEV2-RR8 |
| RR9 | double | R | DEV2-RR9 |
| RR10 | double | R | DEV2-RR10 |
| RR11 | double | R | DEV2-RR11 |
| RR12 | double | R | DEV2-RR12 |
| RR13 | double | R | DEV2-RR13 |
| RR14 | double | R | DEV2-RR14 |
| RR15 | double | R | DEV2-RR15 |
| RR16 | double | R | DEV2-RR16 |
| RR17 | double | R | DEV2-RR17 |
| RR18 | double | R | DEV2-RR18 |
| RR19 | double | R | DEV2-RR19 |
| RR20 | double | R | DEV2-RR20 |
| RR21 | double | R | DEV2-RR21 |
| RR22 | double | R | DEV2-RR22 |
| RR23 | double | R | DEV2-RR23 |
| RR24 | double | R | DEV2-RR24 |
| RR25 | double | R | DEV2-RR25 |
| RR26 | double | R | DEV2-RR26 |
| RR27 | double | R | DEV2-RR27 |
| RR28 | double | R | DEV2-RR28 |
| RR29 | double | R | DEV2-RR29 |
| RR30 | double | R | DEV2-RR30 |

### 方法

| 名称 | 参数 | 说明 | 示例 |
|------|------|------|------|
| ConnectJXTRM | ipPort |  |  |
| ReadRegFromR1ToR30 |  | 读DEV1的R1到R30 |  |
| ReadRegFromRR1ToRR30 |  | 读DEV2的RR1到RR30 |  |

