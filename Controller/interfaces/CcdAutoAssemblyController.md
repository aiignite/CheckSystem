# CcdAutoAssemblyController 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: CCD自动装配控制器

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| CcdAutoAssemblyController | CCD自动装配控制器 | CurrentSelectProductNo | string | 字段 | - | 当前选择的产品编号 | - |
| CcdAutoAssemblyController | CCD自动装配控制器 | CurrentSelectProductPartNo | string | 字段 | - | 当前选择的产品零件号 | - |
| CcdAutoAssemblyController | CCD自动装配控制器 | IsProductSelected | bool | 字段 | - | 是否已选择产品 | - |
| CcdAutoAssemblyController | CCD自动装配控制器 | IsTrackOk | bool | 字段 | - | 追溯是否正常 | - |
| CcdAutoAssemblyController | CCD自动装配控制器 | RemoteSqlIp | string | 字段 | - | 远程SQL服务器IP | 127.0.0.1 |
| CcdAutoAssemblyController | CCD自动装配控制器 | LoadPlateBarcode | string | 字段 | - | 载盘条码 | - |
| CcdAutoAssemblyController | CCD自动装配控制器 | AddProduct | void | 方法 | string productNoProductName | 添加产品到列表 | - |
| CcdAutoAssemblyController | CCD自动装配控制器 | SelectProduct | void | 方法 | - | 选择产品 | - |
| CcdAutoAssemblyController | CCD自动装配控制器 | SetCurrentProcessName | void | 方法 | string processName | 设置当前工序名称 | - |
| CcdAutoAssemblyController | CCD自动装配控制器 | SetLastProcessName | void | 方法 | string processName | 设置上一工序名称 | - |
| CcdAutoAssemblyController | CCD自动装配控制器 | SaveLoadPlateState | void | 方法 | string plateState | 保存载盘状态 | - |
| CcdAutoAssemblyController | CCD自动装配控制器 | ReadLoadPlateState | void | 方法 | - | 读取载盘状态 | - |
| CcdAutoAssemblyController | CCD自动装配控制器 | DebugReadLoadPlateState | void | 方法 | - | 调试读取载盘状态 | - |
| CcdAutoAssemblyController | CCD自动装配控制器 | InitClient | void | 方法 | string ipPort | 初始化Modbus客户端 | - |
| CcdAutoAssemblyController | CCD自动装配控制器 | ConnectPlc | void | 方法 | string ipPort | 连接PLC | - |
| CcdAutoAssemblyController | CCD自动装配控制器 | InitServer | void | 方法 | string port | 初始化Modbus服务器 | - |
| CcdAutoAssemblyController | CCD自动装配控制器 | ConnectBarcode | void | 方法 | string ipPort | 连接条码枪 | - |
| CcdAutoAssemblyController | CCD自动装配控制器 | ReadBarcode | void | 方法 | string format | 读取条码 | - |
