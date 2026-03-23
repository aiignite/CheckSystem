# SiemensPlcHVSM 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: SiemensPlcHVSM

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| SiemensPlcHVSM | SiemensPlcHVSM | I10001-I10184 | bool | 字段 | - | R,输入%I0.0-%I22.7 | - |
| SiemensPlcHVSM | SiemensPlcHVSM | O00001-O00184 | bool | 字段 | - | R/W,输出%Q0.0-%Q22.7 | - |
| SiemensPlcHVSM | SiemensPlcHVSM | Hr40001-Hr40120 | ushort | 字段 | - | R/W,Hr40001-Hr40120 | - |
| SiemensPlcHVSM | SiemensPlcHVSM | Hr40121-Hr40130 | ushort | 字段 | - | R/W,Hr400121-Hr400130 | - |
| SiemensPlcHVSM | SiemensPlcHVSM | Hr40131-Hr40250 | ushort | 字段 | - | Holding Registers | - |
| SiemensPlcHVSM | SiemensPlcHVSM | Ir40001-Ir40120 | ushort | 字段 | - | Input Regs | - |
| SiemensPlcHVSM | SiemensPlcHVSM | ReadMaxLenth | ushort | 字段 | - | 读取最大长度 | - |
| SiemensPlcHVSM | SiemensPlcHVSM | ReadStartAddr | ushort | 字段 | - | 读取起始地址，默认120 | - |
| SiemensPlcHVSM | SiemensPlcHVSM | WriteStartAddr | ushort | 字段 | - | 写入起始地址，默认0 | - |
| SiemensPlcHVSM | SiemensPlcHVSM | IsReadO | bool | 字段 | - | 是否读取输出信号 | - |
| SiemensPlcHVSM | SiemensPlcHVSM | IsWriteO | bool | 字段 | - | 是否写入输出信号 | - |
| SiemensPlcHVSM | SiemensPlcHVSM | GetAsciiStr | string | 字段 | - | 获取ASCII字符串 | - |
| SiemensPlcHVSM | SiemensPlcHVSM | InitRemoteIpAddress | void | 方法 | string ipport | 初始化远程IP地址 | InitRemoteIpAddress("192.168.1.1:502") |
| SiemensPlcHVSM | SiemensPlcHVSM | CycleUpdate | void | 方法 | 无 | 循环更新 | CycleUpdate() |
| SiemensPlcHVSM | SiemensPlcHVSM | ReadCurrentStatus | void | 方法 | 无 | 读取当前状态 | ReadCurrentStatus() |
| SiemensPlcHVSM | SiemensPlcHVSM | RefreshOutputs | void | 方法 | 无 | 刷新输出 | RefreshOutputs() |
| SiemensPlcHVSM | SiemensPlcHVSM | ResetOutputs | void | 方法 | 无 | 重置输出 | ResetOutputs() |
| SiemensPlcHVSM | SiemensPlcHVSM | ReadRegs | void | 方法 | 无 | 读取寄存器 | ReadRegs() |
| SiemensPlcHVSM | SiemensPlcHVSM | GetAsciiFromUshort | void | 方法 | string start, string len, string format | 从ushort获取ASCII | GetAsciiFromUhort("0", "10", "KEY:0:8") |
