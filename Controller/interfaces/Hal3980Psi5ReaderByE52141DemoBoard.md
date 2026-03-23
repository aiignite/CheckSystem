# Hal3980Psi5ReaderByE52141DemoBoard 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: Hal3980Psi5ReaderByE52141DemoBoard

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| Hal3980Psi5ReaderByE52141DemoBoard | R,控制器初始化是否成功 | IsDemoBoardConfingOk | string | 字段 | - | 控制器初始化是否成功 | - |
| Hal3980Psi5ReaderByE52141DemoBoard | R,通道1-PSI5帧输出(14bit)16进制数 | Channel1Psi5OutPut14Bits | double | 字段 | - | 通道1-PSI5帧输出(14bit)16进制数 | - |
| Hal3980Psi5ReaderByE52141DemoBoard | R,通道1-PSI5帧输出 | Channel1Psi5OutPutData | double | 字段 | - | 通道1-PSI5帧输出 | - |
| Hal3980Psi5ReaderByE52141DemoBoard | R,通道1-PSI5帧RollingCounter | Channel1Psi5RollingCounter | double | 字段 | - | 通道1-PSI5帧RollingCounter | - |
| Hal3980Psi5ReaderByE52141DemoBoard | R,通道1-PSI5帧StatusBit | Channel1Psi5StatusBit | string | 字段 | - | 通道1-PSI5帧StatusBit | - |
| Hal3980Psi5ReaderByE52141DemoBoard | R,通道1-输出占比 | Channel1OutPutPercent | double | 字段 | - | 通道1-输出占比 | - |
| Hal3980Psi5ReaderByE52141DemoBoard | R,通道2-PSI5帧输出(14bit)16进制数 | Channel2Psi5OutPut14Bits | double | 字段 | - | 通道2-PSI5帧输出(14bit)16进制数 | - |
| Hal3980Psi5ReaderByE52141DemoBoard | R,通道2-PSI5帧输出 | Channel2Psi5OutPutData | double | 字段 | - | 通道2-PSI5帧输出 | - |
| Hal3980Psi5ReaderByE52141DemoBoard | R,通道2-PSI5帧RollingCounter | Channel2Psi5RollingCounter | double | 字段 | - | 通道2-PSI5帧RollingCounter | - |
| Hal3980Psi5ReaderByE52141DemoBoard | R,通道2-PSI5帧StatusBit | Channel2Psi5StatusBit | string | 字段 | - | 通道2-PSI5帧StatusBit | - |
| Hal3980Psi5ReaderByE52141DemoBoard | R,通道2-输出占比 | Channel2OutPutPercent | double | 字段 | - | 通道2-输出占比 | - |
| Hal3980Psi5ReaderByE52141DemoBoard | R,通道3-PSI5帧输出(14bit)16进制数 | Channel3Psi5OutPut14Bits | double | 字段 | - | 通道3-PSI5帧输出(14bit)16进制数 | - |
| Hal3980Psi5ReaderByE52141DemoBoard | R,通道3-PSI5帧输出 | Channel3Psi5OutPutData | double | 字段 | - | 通道3-PSI5帧输出 | - |
| Hal3980Psi5ReaderByE52141DemoBoard | R,通道3-PSI5帧RollingCounter | Channel3Psi5RollingCounter | double | 字段 | - | 通道3-PSI5帧RollingCounter | - |
| Hal3980Psi5ReaderByE52141DemoBoard | R,通道3-PSI5帧StatusBit | Channel3Psi5StatusBit | string | 字段 | - | 通道3-PSI5帧StatusBit | - |
| Hal3980Psi5ReaderByE52141DemoBoard | R,通道3-输出占比 | Channel3OutPutPercent | double | 字段 | - | 通道3-输出占比 | - |
| Hal3980Psi5ReaderByE52141DemoBoard | R,通道4-PSI5帧输出(14bit)16进制数 | Channel4Psi5OutPut14Bits | double | 字段 | - | 通道4-PSI5帧输出(14bit)16进制数 | - |
| Hal3980Psi5ReaderByE52141DemoBoard | R,通道4-PSI5帧输出 | Channel4Psi5OutPutData | double | 字段 | - | 通道4-PSI5帧输出 | - |
| Hal3980Psi5ReaderByE52141DemoBoard | R,通道4-PSI5帧RollingCounter | Channel4Psi5RollingCounter | double | 字段 | - | 通道4-PSI5帧RollingCounter | - |
| Hal3980Psi5ReaderByE52141DemoBoard | R,通道4-PSI5帧StatusBit | Channel4Psi5StatusBit | string | 字段 | - | 通道4-PSI5帧StatusBit | - |
| Hal3980Psi5ReaderByE52141DemoBoard | R,通道4-输出占比 | Channel4OutPutPercent | double | 字段 | - | 通道4-输出占比 | - |
| Hal3980Psi5ReaderByE52141DemoBoard | 通道上电 | ConnectBarcodeScanner | void | 方法 | string protocolValue | 连接条码扫描器 | ConnectBarcodeScanner("COM1:9600") |
| Hal3980Psi5ReaderByE52141DemoBoard | 通道上电 | PowerOn | void | 方法 | - | 通道上电 | PowerOn() |
| Hal3980Psi5ReaderByE52141DemoBoard | Demo板寄存器配置 | DemoBoardConfig | void | 方法 | - | Demo板寄存器配置 | DemoBoardConfig() |
| Hal3980Psi5ReaderByE52141DemoBoard | 通道断电 | PowerOff | void | 方法 | - | 通道断电 | PowerOff() |
| Hal3980Psi5ReaderByE52141DemoBoard | 读PSI5帧输出 | ReadPsi5Data | void | 方法 | string channel | 读PSI5帧输出 | ReadPsi5Data("1") |
