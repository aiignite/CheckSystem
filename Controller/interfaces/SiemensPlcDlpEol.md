# SiemensPlcDlpEol 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: SiemensPlcDlpEol

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| SiemensPlcDlpEol | SiemensPlcDlpEol | I10001-I10184 | bool | 字段 | - | R,输入%I0.0-%I22.7 | - |
| SiemensPlcDlpEol | SiemensPlcDlpEol | O00001-O00184 | bool | 字段 | - | R/W,输出%Q0.0-%Q22.7 | - |
| SiemensPlcDlpEol | SiemensPlcDlpEol | Hr40001-Hr40121 | ushort | 字段 | - | R/W,Hr40001-Hr40121 | - |
| SiemensPlcDlpEol | SiemensPlcDlpEol | Hr40122-Hr40130 | ushort | 字段 | - | R/W,Hr400122-Hr400130 | - |
| SiemensPlcDlpEol | SiemensPlcDlpEol | Hr40135-Hr40138 | ushort | 字段 | - | R/W,Hr400135-Hr400138 | - |
| SiemensPlcDlpEol | SiemensPlcDlpEol | Ir40001-Ir40120 | ushort | 字段 | - | Input Regs | - |
| SiemensPlcDlpEol | SiemensPlcDlpEol | ReadMaxLenth | ushort | 字段 | - | 读取最大长度 | - |
| SiemensPlcDlpEol | SiemensPlcDlpEol | ReadStartAddr | ushort | 字段 | - | 读取起始地址，默认120 | - |
| SiemensPlcDlpEol | SiemensPlcDlpEol | WriteStartAddr | ushort | 字段 | - | 写入起始地址，默认0 | - |
| SiemensPlcDlpEol | SiemensPlcDlpEol | IsReadO | bool | 字段 | - | 是否读取输出信号 | - |
| SiemensPlcDlpEol | SiemensPlcDlpEol | IsWriteO | bool | 字段 | - | 是否写入输出信号 | - |
| SiemensPlcDlpEol | SiemensPlcDlpEol | GetAsciiStr | string | 字段 | - | 获取ASCII字符串 | - |
| SiemensPlcDlpEol | SiemensPlcDlpEol | InitRemoteIpAddress | void | 方法 | string ipport | 初始化远程IP地址 | InitRemoteIpAddress("192.168.1.1:502") |
| SiemensPlcDlpEol | SiemensPlcDlpEol | CycleUpdate | void | 方法 | 无 | 循环更新 | CycleUpdate() |
| SiemensPlcDlpEol | SiemensPlcDlpEol | ReadCurrentStatus | void | 方法 | 无 | 读取当前状态 | ReadCurrentStatus() |
| SiemensPlcDlpEol | SiemensPlcDlpEol | RefreshOutputs | void | 方法 | 无 | 刷新输出 | RefreshOutputs() |
| SiemensPlcDlpEol | SiemensPlcDlpEol | ResetOutputs | void | 方法 | 无 | 重置输出 | ResetOutputs() |
| SiemensPlcDlpEol | SiemensPlcDlpEol | ReadRegs | void | 方法 | 无 | 读取寄存器 | ReadRegs() |
| SiemensPlcDlpEol | SiemensPlcDlpEol | GetAsciiFromUshort | void | 方法 | string start, string len, string format | 从ushort获取ASCII | GetAsciiFromUhort("0", "10", "KEY:0:8") |
