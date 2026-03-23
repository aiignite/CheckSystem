# OsramEviyosSiemensPlcAddHoldingRegs 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: ModbusTCP-Product,OsramEviyosSiemensPlcAddHoldingRegs

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| OsramEviyosSiemensPlcAddHoldingRegs | ModbusTCP-Product,OsramEviyosSiemensPlcAddHoldingRegs | InitRemoteIpAddress | 方法 | 方法 | string ip | 初始化远程IP地址 | InitRemoteIpAddress("192.168.0.1") |
| OsramEviyosSiemensPlcAddHoldingRegs | ModbusTCP-Product,OsramEviyosSiemensPlcAddHoldingRegs | CycleUpdate | 方法 | 方法 | - | 循环更新PLC数据 | CycleUpdate() |
| OsramEviyosSiemensPlcAddHoldingRegs | ModbusTCP-Product,OsramEviyosSiemensPlcAddHoldingRegs | ReadCurrentStatus | 方法 | 方法 | - | 读取当前状态 | ReadCurrentStatus() |
| OsramEviyosSiemensPlcAddHoldingRegs | ModbusTCP-Product,OsramEviyosSiemensPlcAddHoldingRegs | RefreshOutputs | 方法 | 方法 | - | 刷新输出 | RefreshOutputs() |
| OsramEviyosSiemensPlcAddHoldingRegs | ModbusTCP-Product,OsramEviyosSiemensPlcAddHoldingRegs | ResetOutputs | 方法 | 方法 | - | 重置输出 | ResetOutputs() |
| OsramEviyosSiemensPlcAddHoldingRegs | ModbusTCP-Product,OsramEviyosSiemensPlcAddHoldingRegs | ReadRegs | 方法 | 方法 | ushort startAddr, ushort length | 读取寄存器 | ReadRegs(40001, 10) |
| OsramEviyosSiemensPlcAddHoldingRegs | ModbusTCP-Product,OsramEviyosSiemensPlcAddHoldingRegs | GetAsciiFromUshort | 方法 | 方法 | ushort[] values | 从Ushort获取ASCII | GetAsciiFromUshort(values) |
| OsramEviyosSiemensPlcAddHoldingRegs | ModbusTCP-Product,OsramEviyosSiemensPlcAddHoldingRegs | GetLocalBarcode | 方法 | 方法 | - | 获取本地条码 | GetLocalBarcode() |
| OsramEviyosSiemensPlcAddHoldingRegs | ModbusTCP-Product,OsramEviyosSiemensPlcAddHoldingRegs | GetLocalBarcode2 | 方法 | 方法 | - | 获取本地条码2 | GetLocalBarcode2() |
| OsramEviyosSiemensPlcAddHoldingRegs | ModbusTCP-Product,OsramEviyosSiemensPlcAddHoldingRegs | I10001 ~ I10184 | 字段 | 字段 | - | 输入信号(10001-10184) | R,输入信号状态 |
| OsramEviyosSiemensPlcAddHoldingRegs | ModbusTCP-Product,OsramEviyosSiemensPlcAddHoldingRegs | O00001 ~ O00184 | 字段 | 字段 | - | 输出信号(00001-00184) | R/W,输出信号状态 |
| OsramEviyosSiemensPlcAddHoldingRegs | ModbusTCP-Product,OsramEviyosSiemensPlcAddHoldingRegs | Hr40001 ~ Hr40300 | 字段 | 字段 | - | 保持寄存器(40001-40300) | R/W,保持寄存器值 |
| OsramEviyosSiemensPlcAddHoldingRegs | ModbusTCP-Product,OsramEviyosSiemensPlcAddHoldingRegs | Ir40001 ~ Ir40120 | 字段 | 字段 | - | 输入寄存器(40001-40120) | R,输入寄存器值 |

### 特殊功能说明

本控制器在OsramEviyosSiemensPlc基础上增加了条码读取功能:
- **GetLocalBarcode**: 获取本地条码数据
- **GetLocalBarcode2**: 获取第二个本地条码数据

### 输入信号说明

输入信号I10001-I10184,共184个输入点,用于接收外部设备状态信号。

### 输出信号说明

输出信号O00001-O00184,共184个输出点,用于控制外部设备。

### 寄存器说明

- **保持寄存器(Hr)**: Hr40001-Hr40300,共300个保持寄存器,可读写
- **输入寄存器(Ir)**: Ir40001-Ir40120,共120个输入寄存器,只读

### 通信方式

- **Modbus TCP**: 通过TCP/IP网络与西门子PLC进行Modbus通信
- **默认端口**: 502
