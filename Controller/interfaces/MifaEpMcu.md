# MifaEpMcu 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: CAN-Product,MIFA-EPMCU

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| MifaEpMcu | R,生产序列号-F18C | ProductSerialNo | string | 字段 | - | 生产序列号 | - |
| MifaEpMcu | R,总成零件号-F187 | PartNo | string | 字段 | - | 总成零件号 | - |
| MifaEpMcu | R,引导程序零件号-F183 | FblPartNo | string | 字段 | - | 引导程序零件号 | - |
| MifaEpMcu | R,应用程序零件号-F1A0 | AppPartNo | string | 字段 | - | 应用程序零件号 | - |
| MifaEpMcu | R,供应商软件参考号-F194 | SupplySwNo | string | 字段 | - | 供应商软件参考号 | - |
| MifaEpMcu | R,标定程序零件号-F1A1 | CaliPartNo | string | 字段 | - | 标定程序零件号 | - |
| MifaEpMcu | R,下载结果 | DownloadResult | string | 字段 | - | 下载结果 | - |
| MifaEpMcu | R/W,APP文件路径 | AppFilePath | string | 字段 | - | APP文件路径 | - |
| MifaEpMcu | 唤醒 | Awake | void | 方法 | - | 唤醒控制器 | Awake() |
| MifaEpMcu | 休眠 | Sleep | void | 方法 | - | 休眠控制器 | Sleep() |
| MifaEpMcu | 进入正常模式 | EnterDefaultSession | void | 方法 | - | 进入正常模式 | EnterDefaultSession() |
| MifaEpMcu | 进入拓展模式 | EnterExtendedSession | void | 方法 | - | 进入拓展模式 | EnterExtendedSession() |
| MifaEpMcu | 进入编程模式 | EnterProgramSession | void | 方法 | - | 进入编程模式 | EnterProgramSession() |
| MifaEpMcu | 解锁SeedKey | SecurityAccess | bool | 方法 | string subFunc | 解锁SeedKey | SecurityAccess("1") |
| MifaEpMcu | ReadF18C-生产序列号 | ReadProductSerialNo | void | 方法 | - | 读取生产序列号 | ReadProductSerialNo() |
| MifaEpMcu | ReadF187-总成零件号 | ReadPartNo | void | 方法 | - | 读取总成零件号 | ReadPartNo() |
| MifaEpMcu | ReadF183-引导程序零件号 | ReadFblPartNo | void | 方法 | - | 读取引导程序零件号 | ReadFblPartNo() |
| MifaEpMcu | ReadF1A0-应用程序零件号 | ReadAppPartNo | void | 方法 | - | 读取应用程序零件号 | ReadAppPartNo() |
| MifaEpMcu | ReadF194-供应商软件参考号 | ReadSupplySwNo | void | 方法 | - | 读取供应商软件参考号 | ReadSupplySwNo() |
| MifaEpMcu | ReadF1A1-标定程序零件号 | ReadCaliPartNo | void | 方法 | - | 读取标定程序零件号 | ReadCaliPartNo() |
| MifaEpMcu | WriteF187-总成零件号 | WritePartNo | void | 方法 | - | 写入总成零件号 | WritePartNo() |
| MifaEpMcu | WriteF18C-生产序列号 | WriteSerialNo | void | 方法 | - | 写入生产序列号 | WriteSerialNo() |
| MifaEpMcu | 下载 | DownloadFile | void | 方法 | - | 下载APP文件 | DownloadFile() |
