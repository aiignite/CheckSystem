# SiemensPlc 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: SiemensPlc

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| SiemensPlc | SiemensPlc | PushValue | event | 字段 | - | 推送值事件 | - |
| SiemensPlc | SiemensPlc | IsReadO | bool | 字段 | - | 是否读取输出 | obj.IsReadO = true; |
| SiemensPlc | SiemensPlc | IsWriteO | bool | 字段 | - | 是否写入输出 | obj.IsWriteO = true; |
| SiemensPlc | SiemensPlc | ReadMaxLenth | ushort | 字段 | - | 最大读取长度 | obj.ReadMaxLenth = 120; |
| SiemensPlc | SiemensPlc | ReadStartAddr | ushort | 字段 | - | 读取起始地址 | obj.ReadStartAddr = 120; |
| SiemensPlc | SiemensPlc | WriteStartAddr | ushort | 字段 | - | 写入起始地址 | obj.WriteStartAddr = 0; |
| SiemensPlc | SiemensPlc | I10001-I10184 | bool | 字段 | - | 输入%I0.0-%I22.7 | obj.I10001; |
| SiemensPlc | SiemensPlc | O00001-O00184 | bool | 字段 | - | 输出%Q0.0-%Q22.7 | obj.O00001; |
| SiemensPlc | SiemensPlc | Hr40001-Hr40120 | ushort | 字段 | - | Holding Registers | obj.Hr40001; |
| SiemensPlc | SiemensPlc | Hr40121-Hr40130 | ushort | 字段 | - | Holding Registers | obj.Hr40121; |
| SiemensPlc | SiemensPlc | Hr40131-Hr40250 | ushort | 字段 | - | Holding Registers | obj.Hr40131; |
| SiemensPlc | SiemensPlc | Ir40001-Ir40120 | ushort | 字段 | - | Input Registers | obj.Ir40001; |
| SiemensPlc | SiemensPlc | InitRemoteIpAddress | void | 方法 | string ipport | 初始化远程IP地址 | obj.InitRemoteIpAddress("192.168.1.10:502"); |
| SiemensPlc | SiemensPlc | CycleUpdate | void | 方法 | - | 循环更新 | obj.CycleUpdate(); |
| SiemensPlc | SiemensPlc | ReadCurrentStatus | void | 方法 | - | 读取当前状态 | obj.ReadCurrentStatus(); |
| SiemensPlc | SiemensPlc | RefreshOutputs | void | 方法 | - | 刷新输出 | obj.RefreshOutputs(); |
| SiemensPlc | SiemensPlc | ResetOutputs | void | 方法 | - | 重置输出 | obj.ResetOutputs(); |
| SiemensPlc | SiemensPlc | ReadRegs | void | 方法 | - | 读取寄存器 | obj.ReadRegs(); |
| SiemensPlc | SiemensPlc | GetAsciiFromUshort | void | 方法 | string start, string len, string format | 从ushort获取ASCII字符串 | obj.GetAsciiFromUshort("0", "10", "KEY:0:8"); |
| SiemensPlc | SiemensPlc | GetAsciiStr | string | 字段 | - | ASCII字符串结果 | obj.GetAsciiStr; |
