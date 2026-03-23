# Psi5TransferWithElmosE52140 控制器接口文档

## 控制器信息

| 控制器名称 | 控制器描述 |
|-----------|-----------|
| Psi5TransferWithElmosE52140 | PSI5传输控制器 |

## 公共字段

| 名称 | 类型 | 类别 | 描述 |
|------|------|------|------|
| IsDemoBoardConfingOk | string | 读 | R,控制器初始化是否成功 |
| Channel1Psi5OutPut14Bits | double | 读 | R,通道1-PSI5帧输出(14bit)16进制数 |
| Channel1Psi5OutPutData | double | 读 | R,通道1-PSI5帧输出 |
| Channel1Psi5RollingCounter | double | 读 | R,通道1-PSI5帧RollingCounter |
| Channel1Psi5StatusBit | string | 读 | R,通道1-PSI5帧StatusBit |
| Channel1OutPutPercent | double | 读 | R,通道1-输出占比 |
| Channel2Psi5OutPut14Bits | double | 读 | R,通道2-PSI5帧输出(14bit)16进制数 |
| Channel2Psi5OutPutData | double | 读 | R,通道2-PSI5帧输出 |
| Channel2Psi5RollingCounter | double | 读 | R,通道2-PSI5帧RollingCounter |
| Channel2Psi5StatusBit | string | 读 | R,通道2-PSI5帧StatusBit |
| Channel2OutPutPercent | double | 读 | R,通道2-输出占比 |
| Channel3Psi5OutPut14Bits | double | 读 | R,通道3-PSI5帧输出(14bit)16进制数 |
| Channel3Psi5OutPutData | double | 读 | R,通道3-PSI5帧输出 |
| Channel3Psi5RollingCounter | double | 读 | R,通道3-PSI5帧RollingCounter |
| Channel3Psi5StatusBit | string | 读 | R,通道3-PSI5帧StatusBit |
| Channel3OutPutPercent | double | 读 | R,通道3-输出占比 |
| Channel4Psi5OutPut14Bits | double | 读 | R,通道4-PSI5帧输出(14bit)16进制数 |
| Channel4Psi5OutPutData | double | 读 | R,通道4-PSI5帧输出 |
| Channel4Psi5RollingCounter | double | 读 | R,通道4-PSI5帧RollingCounter |
| Channel4Psi5StatusBit | string | 读 | R,通道4-PSI5帧StatusBit |
| Channel4OutPutPercent | double | 读 | R,通道4-输出占比 |

## 公共方法

| 名称 | 参数 | 类型 | 说明 | 示例 |
|------|------|------|------|------|
| ConnectController | protocolValue: string | 方法 | 连接控制器 | ConnectController("192.168.1.1:5000") |
| PowerOn | 无 | 方法 | 通道上电 |  |
| PowerOff | 无 | 方法 | 通道断电 |  |
| DemoBoardConfig | 无 | 方法 | Demo板寄存器配置 |  |
| ReadPsi5Data | channel: string | 方法 | 读PSI5帧输出 | ReadPsi5Data("1") |
