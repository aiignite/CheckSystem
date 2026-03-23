# OsramEviyosSiemensPlc 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: ModbusTCP-Product,OsramEviyosSiemensPlc

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| OsramEviyosSiemensPlc | ModbusTCP-Product,OsramEviyosSiemensPlc | InitRemoteIpAddress | 方法 | 方法 | string ip | 初始化远程IP地址 | InitRemoteIpAddress("192.168.0.1") |
| OsramEviyosSiemensPlc | ModbusTCP-Product,OsramEviyosSiemensPlc | CycleUpdate | 方法 | 方法 | - | 循环更新PLC数据 | CycleUpdate() |
| OsramEviyosSiemensPlc | ModbusTCP-Product,OsramEviyosSiemensPlc | ReadCurrentStatus | 方法 | 方法 | - | 读取当前状态 | ReadCurrentStatus() |
| OsramEviyosSiemensPlc | ModbusTCP-Product,OsramEviyosSiemensPlc | RefreshOutputs | 方法 | 方法 | - | 刷新输出 | RefreshOutputs() |
| OsramEviyosSiemensPlc | ModbusTCP-Product,OsramEviyosSiemensPlc | ResetOutputs | 方法 | 方法 | - | 重置输出 | ResetOutputs() |
| OsramEviyosSiemensPlc | ModbusTCP-Product,OsramEviyosSiemensPlc | ReadRegs | 方法 | 方法 | ushort startAddr, ushort length | 读取寄存器 | ReadRegs(40001, 10) |
| OsramEviyosSiemensPlc | ModbusTCP-Product,OsramEviyosSiemensPlc | GetAsciiFromUshort | 方法 | 方法 | ushort[] values | 从Ushort获取ASCII | GetAsciiFromUshort(values) |
| OsramEviyosSiemensPlc | ModbusTCP-Product,OsramEviyosSiemensPlc | I10001 ~ I10184 | 字段 | 字段 | - | 输入信号(10001-10184) | R,输入信号状态 |
| OsramEviyosSiemensPlc | ModbusTCP-Product,OsramEviyosSiemensPlc | O00001 ~ O00184 | 字段 | 字段 | - | 输出信号(00001-00184) | R/W,输出信号状态 |
| OsramEviyosSiemensPlc | ModbusTCP-Product,OsramEviyosSiemensPlc | Hr40001 ~ Hr40300 | 字段 | 字段 | - | 保持寄存器(40001-40300) | R/W,保持寄存器值 |
| OsramEviyosSiemensPlc | ModbusTCP-Product,OsramEviyosSiemensPlc | Ir40001 ~ Ir40120 | 字段 | 字段 | - | 输入寄存器(40001-40120) | R,输入寄存器值 |

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
