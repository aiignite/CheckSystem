# SySerialPortController 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: SySerialPortController

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| SySerialPortController | SySerialPortController | currentData | string | 字段 | - | - | - |
| SySerialPortController | SySerialPortController | data1Value | double | 字段 | - | R,缓存位置1 | - |
| SySerialPortController | SySerialPortController | data2Value | double | 字段 | - | R,缓存位置2 | - |
| SySerialPortController | SySerialPortController | data12Value | double | 字段 | - | R,缓存位位置1与缓存位置2的差值 | - |
| SySerialPortController | SySerialPortController | Init | void | 方法 | string portName_BaudRate | 初始化串口 | - |
| SySerialPortController | SySerialPortController | ReceivedBytesThreshold | void | 方法 | int i | 设置接收字节阈值 | - |
| SySerialPortController | SySerialPortController | SetEncoding | void | 方法 | string str | 设置编码格式 | - |
| SySerialPortController | SySerialPortController | Send | bool | 方法 | string str | 发送字符串 | - |
| SySerialPortController | SySerialPortController | AddSendStr | void | 方法 | string str | 添加周期发送命令值 | - |
| SySerialPortController | SySerialPortController | StartOnPeriodSend | void | 方法 | - | 开始周期发送命令 | - |
| SySerialPortController | SySerialPortController | StopOnperiodSend | void | 方法 | - | 停止周期发送命令 | - |
| SySerialPortController | SySerialPortController | AddReadSlice | void | 方法 | string startIndex_Count | 添加读取切片 | - |
| SySerialPortController | SySerialPortController | GetMaxValue | float | 方法 | - | 获取最大值 | - |
| SySerialPortController | SySerialPortController | GetMinValue | float | 方法 | - | 获取最小值 | - |
| SySerialPortController | SySerialPortController | ClearPerviousData | void | 方法 | - | 清除缓存 | - |
| SySerialPortController | SySerialPortController | Get1Value | void | 方法 | - | 读取值存入缓存位置1 | - |
| SySerialPortController | SySerialPortController | Get2Value | void | 方法 | - | 读取值存入缓存位2 | - |
| SySerialPortController | SySerialPortController | GetValue12 | void | 方法 | - | 读取缓存位位置1与缓存位置2的差值 | - |
