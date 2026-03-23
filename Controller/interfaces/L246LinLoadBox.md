# 控制器文档

## L246LinLoadBox

### 控制器描述
LIN-Product,L246LinLoadBox

### 字段

| 名称 | 类型 | 类别 | 说明 |
|------|------|------|------|
| MasterLin1Msg | string | R | Master LIN1读取消息内容 |
| ReadMasterLin1MsgResult | string | R | Master LIN1读取消息结果 |
| MasterLin2Msg | string | R | Master LIN2读取消息内容 |
| ReadMasterLin2MsgResult | string | R | Master LIN2读取消息结果 |
| MasterLin3Msg | string | R | Master LIN3读取消息内容 |
| ReadMasterLin3MsgResult | string | R | Master LIN3读取消息结果 |
| SlaveLinMsg | string | R | Slave LIN1读取消息结果 |
| ReadSlaveLinMsgResult | string | R | Slave LIN1读取消息内容 |

### 方法

| 名称 | 参数 | 说明 | 示例 |
|------|------|------|------|
| ReadMasterLin1Message | delayMs | Read Master LIN1 Message |  |
| ReadMasterLin2Message | delayMs | Read Master LIN2 Message |  |
| ReadMasterLin3Message | delayMs | Read Master LIN3 Message |  |
| ReadSlaveMessage | delayMs | Read Slave LIN Message |  |

