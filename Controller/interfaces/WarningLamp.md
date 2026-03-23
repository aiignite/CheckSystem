# WarningLamp 控制器接口文档

## 控制器基本信息

| 控制器名称 | 控制器描述 |
|------------|------------|
| WarningLamp | 警告灯控制器 |

## 公共属性

| 名称 | 类型 | 类别 | 说明 | 示例 |
|------|------|------|------|------|
| IsInUsing | bool | 属性 | 使用中 | - |
| GetBarcodeStr | string | 属性 | 条码字符串 | - |
| LockBarcode | bool | 属性 | 锁定条码 | - |

## 公共方法

| 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|------|------|------|------|------|------|
| ConnectWarningLamp | void | 方法 | portBaudRate: string | 连接警告灯 | ConnectWarningLamp("COM1:115200") |
| OnRedLampLong | void | 方法 | 无 | 红灯常亮 | OnRedLampLong() |
| OnYellowLampLong | void | 方法 | 无 | 黄灯常亮 | OnYellowLampLong() |
| OnGreenLampLong | void | 方法 | 无 | 绿灯常亮 | OnGreenLampLong() |
| OnBuzzerLong | void | 方法 | 无 | 蜂鸣器常响 | OnBuzzerLong() |
| OnRedLampShort | void | 方法 | 无 | 红灯短亮 | OnRedLampShort() |
| OnYellowLampShort | void | 方法 | 无 | 黄灯短亮 | OnYellowLampShort() |
| OnGreenLampShort | void | 方法 | 无 | 绿灯短亮 | OnGreenLampShort() |
| OnBuzzerShort | void | 方法 | 无 | 蜂鸣器短响 | OnBuzzerShort() |
| OffRedLamp | void | 方法 | 无 | 红灯灭 | OffRedLamp() |
| OffYellowLamp | void | 方法 | 无 | 黄灯灭 | OffYellowLamp() |
| OffGreenLamp | void | 方法 | 无 | 绿灯灭 | OffGreenLamp() |
| OffAllLamps | void | 方法 | 无 | 所有灯灭 | OffAllLamps() |
| OffBuzzer | void | 方法 | 无 | 蜂鸣器关闭 | OffBuzzer() |
| GetBarcode | void | 方法 | keyAndKeyindexAndLen: string | 获取条码 | GetBarcode("KEY:7:45") |
| ReadBarcode | void | 方法 | length: int | 读取条码 | ReadBarcode(10) |
| LockBarcodeScanReader | void | 方法 | 无 | 锁定条码扫描器 | LockBarcodeScanReader() |
| UnlockBarcodeScanReader | void | 方法 | 无 | 解锁条码扫描器 | UnlockBarcodeScanReader() |