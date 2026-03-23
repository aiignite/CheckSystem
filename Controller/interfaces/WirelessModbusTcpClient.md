# WirelessModbusTcpClient 控制器接口文档

## 控制器基本信息

| 控制器名称 | 控制器描述 |
|------------|------------|
| WirelessModbusTcpClient | Modbus TCP客户端 |

## 公共属性

| 名称 | 类型 | 类别 | 说明 | 示例 |
|------|------|------|------|------|
| ReadMaxLenth | ushort | 属性 | 最大读取长度 | - |
| Hr40001-Hr40120 | ushort | 属性 | 保持寄存器40001-40120 | - |
| Ir40001-Ir40120 | ushort | 属性 | 输入寄存器40001-40120 | - |

## 公共方法

| 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|------|------|------|------|------|------|
| InitRemoteIpAddress | void | 方法 | ipPort: string | 初始化远程IP地址 | InitRemoteIpAddress("192.168.1.100:502") |
| TestWrite1 | void | 方法 | 无 | 测试写入1 | TestWrite1() |
| TestWrite2 | void | 方法 | 无 | 测试写入2 | TestWrite2() |