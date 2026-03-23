# WirelessSiemensPlc 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: WirelessSiemensPlc

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| WirelessSiemensPlc | R/W,输入%I0.0 | I10001 | bool | 字段 | - | 输入信号 | - |
| WirelessSiemensPlc | R/W,输入%I0.1 | I10002 | bool | 字段 | - | 输入信号 | - |
| WirelessSiemensPlc | ... | ... | ... | ... | - | 输入信号(更多) | - |
| WirelessSiemensPlc | R/W,输出%Q0.0 | O00001 | bool | 字段 | - | 输出信号 | - |
| WirelessSiemensPlc | R/W,输出%Q0.1 | O00002 | bool | 字段 | - | 输出信号 | - |
| WirelessSiemensPlc | ... | ... | ... | ... | - | 输出信号(更多) | - |
| WirelessSiemensPlc | R/W,Hr40001 | Hr40001 | ushort | 字段 | - | 保持寄存器 | - |
| WirelessSiemensPlc | R/W,Hr40002 | Hr40002 | ushort | 字段 | - | 保持寄存器 | - |
| WirelessSiemensPlc | ... | ... | ... | ... | - | 保持寄存器(更多) | - |
| WirelessSiemensPlc | 初始化远程IP地址 | InitRemoteIpAddress | 方法 | 方法 | string ipport | 初始化远程IP地址 | - |
| WirelessSiemensPlc | 周期更新 | CycleUpdate | 方法 | 方法 | - | 周期更新 | - |
| WirelessSiemensPlc | 读取当前状态 | ReadCurrentStatus | 方法 | 方法 | - | 读取当前状态 | - |
| WirelessSiemensPlc | 刷新输出 | RefreshOutputs | 方法 | 方法 | - | 刷新输出 | - |
| WirelessSiemensPlc | 重置输出 | ResetOutputs | 方法 | 方法 | - | 重置输出 | - |
| WirelessSiemensPlc | 读取寄存器 | ReadRegs | 方法 | 方法 | - | 读取寄存器 | - |
| WirelessSiemensPlc | 从ushort获取ASCII | GetAsciiFromUshort | 方法 | 方法 | string start, string len, string format | 从ushort获取ASCII字符串 | - |
