# Vx1LedDriver 控制器接口文档

## 控制器基本信息

| 控制器名称 | 控制器描述 |
|------------|------------|
| Vx1LedDriver | CAN-Product,VX1前灯高配 |

## 公共属性

| 名称 | 类型 | 类别 | 说明 | 示例 |
|------|------|------|------|------|
| MainCan | CanBus | 属性 | 主CAN总线 | - |
| SubCan | GatewayCan | 属性 | 子CAN总线(网关) | - |
| DownloadResultStr | string | 属性 | 下载结果 | - |
| PartNumberStr | string | 属性 | 零件号 | - |
| SerialNumerStr | string | 属性 | 序列号 | - |
| FblPartNoStr | string | 属性 | FBL零件号 | - |
| FblVersionStr | string | 属性 | FBL版本号 | - |
| AppPartNoStr | string | 属性 | APP零件号 | - |
| AppVersionStr | string | 属性 | APP版本号 | - |
| ConfigPartNoStr | string | 属性 | 配置零件号 | - |
| ConfigVersionStr | string | 属性 | 配置版本号 | - |
| Ntc1ReadDouble | double | 属性 | NTC1读取值 | - |
| Ntc2ReadDouble | double | 属性 | NTC2读取值 | - |
| Ntc3ReadDouble | double | 属性 | NTC3读取值 | - |
| Ntc4ReadDouble | double | 属性 | NTC4读取值 | - |
| DrvFlashFilePath | string | 属性 | 驱动文件路径 | - |
| AppFilePath | string | 属性 | APP文件路径 | - |
| CaliFailPath | string | 属性 | 标定失败路径 | - |
| Led1Control | bool | 属性 | LED1控制 | - |
| Led2Control | bool | 属性 | LED2控制 | - |
| Slave1Control | bool | 属性 | 从机1控制 | - |

## 公共方法

| 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|------|------|------|------|------|------|
| Awake | void | 方法 | 无 | 模块唤醒 | Awake() |
| Sleep | void | 方法 | 无 | 模块休眠 | Sleep() |
| AHbOn | void | 方法 | 无 | 打开AHB | AHbOn() |
| AHbOff | void | 方法 | 无 | 关闭AHB | AHbOff() |
| ALb1On | void | 方法 | 无 | 打开ALB1 | ALb1On() |
| ALb1Off | void | 方法 | 无 | 关闭ALB1 | ALb1Off() |
| ALb2On | void | 方法 | 无 | 打开ALB2 | ALb2On() |
| ALb2Off | void | 方法 | 无 | 关闭ALB2 | ALb2Off() |
| ALb3On | void | 方法 | 无 | 打开ALB3 | ALb3On() |
| ALb3Off | void | 方法 | 无 | 关闭ALB3 | ALb3Off() |
| ALb4On | void | 方法 | 无 | 打开ALB4 | ALb4On() |
| ALb4Off | void | 方法 | 无 | 关闭ALB4 | ALb4Off() |
| LeftAHbOn | void | 方法 | 无 | 打开左灯AHB | LeftAHbOn() |
| LeftALb1On | void | 方法 | 无 | 打开左灯ALB1 | LeftALb1On() |
| LeftALb2On | void | 方法 | 无 | 打开左灯ALB2 | LeftALb2On() |
| LeftALb3On | void | 方法 | 无 | 打开左灯ALB3 | LeftALb3On() |
| LeftALb4On | void | 方法 | 无 | 打开左灯ALB4 | LeftALb4On() |
| LeftAHbOff | void | 方法 | 无 | 关闭左灯AHB | LeftAHbOff() |
| LeftALb1Off | void | 方法 | 无 | 关闭左灯ALB1 | LeftALb1Off() |
| LeftALb2Off | void | 方法 | 无 | 关闭左灯ALB2 | LeftALb2Off() |
| LeftALb3Off | void | 方法 | 无 | 关闭左灯ALB3 | LeftALb3Off() |
| LeftALb4Off | void | 方法 | 无 | 关闭左灯ALB4 | LeftALb4Off() |
| RightAHbOn | void | 方法 | 无 | 打开右灯AHB | RightAHbOn() |
| RightALb1On | void | 方法 | 无 | 打开右灯ALB1 | RightALb1On() |
| RightALb2On | void | 方法 | 无 | 打开右灯ALB2 | RightALb2On() |
| RightALb3On | void | 方法 | 无 | 打开右灯ALB3 | RightALb3On() |
| RightALb4On | void | 方法 | 无 | 打开右灯ALB4 | RightALb4On() |
| RightAHbOff | void | 方法 | 无 | 关闭右灯AHB | RightAHbOff() |
| RightALb1Off | void | 方法 | 无 | 关闭右灯ALB1 | RightALb1Off() |
| RightALb2Off | void | 方法 | 无 | 关闭右灯ALB2 | RightALb2Off() |
| RightALb3Off | void | 方法 | 无 | 关闭右灯ALB3 | RightALb3Off() |
| RightALb4Off | void | 方法 | 无 | 关闭右灯ALB4 | RightALb4Off() |
| ChangeToLeftNode | void | 方法 | 无 | 切换到左节点 | ChangeToLeftNode() |
| ChangeToRightNode | void | 方法 | 无 | 切换到右节点 | ChangeToRightNode() |
| Download | void | 方法 | 无 | 下载 | Download() |
| SetupTestMode | void | 方法 | 无 | 设置测试模式 | SetupTestMode() |
| InputOutPutControl | void | 方法 | 无 | 输入输出控制 | InputOutPutControl() |
| ReadNtc | void | 方法 | index: string | 读NTC | ReadNtc("1") |