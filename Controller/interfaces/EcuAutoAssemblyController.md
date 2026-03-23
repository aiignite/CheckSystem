# EcuAutoAssemblyController 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: ECU自动装配控制器

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| EcuAutoAssemblyController | ECU自动装配控制器 | CurrentSelectProductNo | string | 字段 | - | 当前选择的产品编号 | - |
| EcuAutoAssemblyController | ECU自动装配控制器 | CurrentSelectProductPartNo | string | 字段 | - | 当前选择的产品零件号 | - |
| EcuAutoAssemblyController | ECU自动装配控制器 | IsProductSelected | bool | 字段 | - | 是否已选择产品 | - |
| EcuAutoAssemblyController | ECU自动装配控制器 | IsTrackOk | bool | 字段 | - | 追溯是否正常 | - |
| EcuAutoAssemblyController | ECU自动装配控制器 | RemoteSqlIp | string | 字段 | - | 远程SQL服务器IP | 127.0.0.1 |
| EcuAutoAssemblyController | ECU自动装配控制器 | LoadPlateBarcode | string | 字段 | - | 载盘条码 | - |
| EcuAutoAssemblyController | ECU自动装配控制器 | BarcodeTriggerCmd | string | 字段 | - | 条码触发命令 | T |
| EcuAutoAssemblyController | ECU自动装配控制器 | IsReadingBarcode | bool | 字段 | - | 是否正在读取条码 | - |
| EcuAutoAssemblyController | ECU自动装配控制器 | WtireBarcode1Ok | ushort | 字段 | - | 写入条码1完成 | - |
| EcuAutoAssemblyController | ECU自动装配控制器 | WtireBarcode2Ok | ushort | 字段 | - | 写入条码2完成 | - |
| EcuAutoAssemblyController | ECU自动装配控制器 | WtireBarcode3Ok | ushort | 字段 | - | 写入条码3完成 | - |
| EcuAutoAssemblyController | ECU自动装配控制器 | WtireBarcode4Ok | ushort | 字段 | - | 写入条码4完成 | - |
| EcuAutoAssemblyController | ECU自动装配控制器 | ReadySignal | ushort | 字段 | - | 就绪信号 | - |
| EcuAutoAssemblyController | ECU自动装配控制器 | CompleteSignal | ushort | 字段 | - | 完成信号 | - |
| EcuAutoAssemblyController | ECU自动装配控制器 | LoadPlateProduct1State | ushort | 字段 | - | 载盘产品1状态 | - |
| EcuAutoAssemblyController | ECU自动装配控制器 | LoadPlateProduct2State | ushort | 字段 | - | 载盘产品2状态 | - |
| EcuAutoAssemblyController | ECU自动装配控制器 | LoadPlateProduct3State | ushort | 字段 | - | 载盘产品3状态 | - |
| EcuAutoAssemblyController | ECU自动装配控制器 | LoadPlateProduct4State | ushort | 字段 | - | 载盘产品4状态 | - |
| EcuAutoAssemblyController | ECU自动装配控制器 | AddProduct | void | 方法 | string productNoProductName | 添加产品到列表 | - |
| EcuAutoAssemblyController | ECU自动装配控制器 | SelectProduct | void | 方法 | - | 选择产品 | - |
| EcuAutoAssemblyController | ECU自动装配控制器 | SetCurrentProcessName | void | 方法 | string processName | 设置当前工序名称 | - |
| EcuAutoAssemblyController | ECU自动装配控制器 | SetLastProcessName | void | 方法 | string processName | 设置上一工序名称 | - |
| EcuAutoAssemblyController | ECU自动装配控制器 | SaveLoadPlateState | void | 方法 | string plateState | 保存载盘状态 | - |
| EcuAutoAssemblyController | ECU自动装配控制器 | ReadLoadPlateState | void | 方法 | - | 读取载盘状态 | - |
| EcuAutoAssemblyController | ECU自动装配控制器 | ConnectBarcode | void | 方法 | string ipPort | 连接条码枪 | - |
| EcuAutoAssemblyController | ECU自动装配控制器 | ReadBarcode | void | 方法 | string format | 读取条码 | - |
| EcuAutoAssemblyController | ECU自动装配控制器 | WritePlgBarcodeData | void | 方法 | string filePath, string index | 写入PLG条码数据 | - |
| EcuAutoAssemblyController | ECU自动装配控制器 | InitClient | void | 方法 | string ipPort | 初始化Modbus客户端 | - |
| EcuAutoAssemblyController | ECU自动装配控制器 | ConnectPlc | void | 方法 | string ipPort | 连接PLC | - |
| EcuAutoAssemblyController | ECU自动装配控制器 | InitServer | void | 方法 | string port | 初始化Modbus服务器 | - |
