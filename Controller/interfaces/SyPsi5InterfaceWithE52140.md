# SyPsi5InterfaceWithE52140 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: CAN-Product,PSI5读取

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | CicCan | CanBus | 字段 | - | CIC模块CAN总线 | - |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | Channel1Psi5RollingCounter | double | 字段 | - | R,CH1-PSI5帧RollingCounter | - |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | Channel1Psi5StatusBit | string | 字段 | - | R,CH1-PSI5帧StatusBit | - |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | Channel1Psi5Data14Bit | string | 字段 | - | R,CH1-PSI5帧14Bit | - |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | Channel1Psi5OutputData | double | 字段 | - | R,CH1-PSI5帧输出 | - |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | Channel1OutPutPercent | double | 字段 | - | R,CH1-输出占比 | - |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | Channel1Psi5RollingCounterBits | string | 字段 | - | R,CH1-PSI5帧RoiiingCounterBits | - |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | Channel1Psi5CrcBits | string | 字段 | - | R,CH1-PSI5帧CrcBits | - |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | Channel2Psi5RollingCounter | double | 字段 | - | R,CH2-PSI5帧RollingCounter | - |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | Channel2Psi5StatusBit | string | 字段 | - | R,CH2-PSI5帧StatusBit | - |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | Channel2Psi5Data14Bit | string | 字段 | - | R,CH2-PSI5帧24Bit | - |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | Channel2Psi5OutputData | double | 字段 | - | R,CH2-PSI5帧输出 | - |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | Channel2OutPutPercent | double | 字段 | - | R,CH2-输出占比 | - |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | Channel2Psi5RollingCounterBits | string | 字段 | - | R,CH2-PSI5帧RoiiingCounterBits | - |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | Channel2Psi5CrcBits | string | 字段 | - | R,CH2-PSI5帧CrcBits | - |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | Channel3Psi5RollingCounter | double | 字段 | - | R,CH3-PSI5帧RollingCounter | - |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | Channel3Psi5StatusBit | string | 字段 | - | R,CH3-PSI5帧StatusBit | - |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | Channel3Psi5Data14Bit | string | 字段 | - | R,CH3-PSI5帧24Bit | - |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | Channel3Psi5OutputData | double | 字段 | - | R,CH3-PSI5帧输出 | - |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | Channel3OutPutPercent | double | 字段 | - | R,CH3-输出占比 | - |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | Channel3Psi5RollingCounterBits | string | 字段 | - | R,CH3-PSI5帧RoiiingCounterBits | - |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | Channel3Psi5CrcBits | string | 字段 | - | R,CH3-PSI5帧CrcBits | - |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | Channel4Psi5RollingCounter | double | 字段 | - | R,CH4-PSI5帧RollingCounter | - |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | Channel4Psi5StatusBit | string | 字段 | - | R,CH4-PSI5帧StatusBit | - |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | Channel4Psi5Data14Bit | string | 字段 | - | R,CH4-PSI5帧24Bit | - |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | Channel4Psi5OutputData | double | 字段 | - | R,CH4-PSI5帧输出 | - |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | Channel4OutPutPercent | double | 字段 | - | R,CH4-输出占比 | - |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | Channel4Psi5RollingCounterBits | string | 字段 | - | R,CH4-PSI5帧RoiiingCounterBits | - |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | Channel4Psi5CrcBits | string | 字段 | - | R,CH4-PSI5帧CrcBits | - |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | ConnectInterface | void | 方法 | string protocolValue | 连接TCP接口 | ConnectInterface("192.168.1.100:5000") |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | ConnectSerialPort | void | 方法 | string comPort | 连接串口 | ConnectSerialPort("COM1:19200") |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | SetVBus | void | 方法 | - | 设置VBUS=5.15V | SetVBus() |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | SetAllCh189Kps | void | 方法 | - | 设置CH1~4=189kbps | SetAllCh189Kps() |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | EnableSyncPulseChargePumpAndAllCh | void | 方法 | - | 打开CH1~4 | EnableSyncPulseChargePumpAndAllCh() |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | ConfigChTimeslot | void | 方法 | int ch, uint frameLen | 设置每个通道的数据长度 | ConfigChTimeslot(1, 20) |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | ConfigSpiBuffer | void | 方法 | int ch, uint bitLen | 设置每个通道的buffer长度 | ConfigSpiBuffer(1, 48) |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | ReadSensorData24Bit | void | 方法 | int ch, int shortSyncPulseCount | 读取24位传感器数据 | ReadSensorData24Bit(1, 3) |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | ReadSensorData32Bit | void | 方法 | int ch, int shortSyncPulseCount | 读取32位传感器数据 | ReadSensorData32Bit(1, 3) |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | ReadSensorData32BitByCicCan | void | 方法 | - | CIC模块读取PSI5输出 | ReadSensorData32BitByCicCan() |
| SyPsi5InterfaceWithE52140 | CAN-Product,PSI5读取 | CicReset | void | 方法 | - | CIC-RESET | CicReset() |
