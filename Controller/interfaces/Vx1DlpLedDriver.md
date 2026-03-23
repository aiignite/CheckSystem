# Vx1DlpLedDriver 控制器接口文档

## 控制器基本信息

| 控制器名称 | 控制器描述 |
|------------|------------|
| Vx1DlpLedDriver | CAN-Product,VX1-DLP-LED-Driver |

## 公共属性

| 名称 | 类型 | 类别 | 说明 | 示例 |
|------|------|------|------|------|
| Can | CanBus | 属性 | CAN总线 | - |
| ProductData | string | 属性 | 产品数据 | - |
| ProductSerialNumber | string | 属性 | 产品序列号 | - |
| HardwareVersion | string | 属性 | R,HardwareVersion | - |
| PartNo | string | 属性 | R,PartNo | - |
| FblVersion | string | 属性 | R,FblVersion | - |
| AppVersion | string | 属性 | R,AppVersion | - |
| AppPartNo | string | 属性 | R,AppPartNo | - |
| BarcodeBuff | string | 属性 | 条码缓存 | - |
| DownloadResult | string | 属性 | R,APP下载结果 | - |
| DownloadTimeS | string | 属性 | R,APP下载时间 | - |
| AppFilePath | string | 属性 | R/W,APP文件路径 | - |

## 公共方法

| 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|------|------|------|------|------|------|
| LedOn | void | 方法 | pwm: string | 打开LED | LedOn("255") |
| LedOff | void | 方法 | 无 | 关闭LED并停止周期帧 | LedOff() |
| GetBarcode | void | 方法 | keyAndKeyindexAndLen: string | 获取条码 | GetBarcode("P00120613:7:45") |
| ClearBarcode | void | 方法 | 无 | 清空条码 | ClearBarcode() |
| SecurityAccess | void | 方法 | key: string | SecurityAccess | SecurityAccess("021122334401") |
| EnterExtendedMode | void | 方法 | 无 | 进入扩展模式 | EnterExtendedMode() |
| EcuReset | void | 方法 | 无 | ECU重置 | EcuReset() |
| WriteHardwareVersion | void | 方法 | hardwareVersion: string | 写硬件版本号 | WriteHardwareVersion("01") |
| WriteProductData | void | 方法 | 无 | 写产品数据 | WriteProductData() |
| WriteProductSerialNumber | void | 方法 | 无 | 写产品序列号 | WriteProductSerialNumber() |
| ReadHardwareVersion | void | 方法 | 无 | 读硬件版本号 | ReadHardwareVersion() |
| ReadProductData | void | 方法 | 无 | 读产品数据 | ReadProductData() |
| ReadProductSerialNumber | void | 方法 | 无 | 读产品序列号 | ReadProductSerialNumber() |
| ReadPartNo | void | 方法 | 无 | 读总成零件号 | ReadPartNo() |
| ReadFblVersion | void | 方法 | 无 | 读FBL版本号 | ReadFblVersion() |
| ReadAppVersion | void | 方法 | 无 | 读APP版本号 | ReadAppVersion() |
| ReadAppPartNo | void | 方法 | 无 | 读APP零件号 | ReadAppPartNo() |