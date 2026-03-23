# SHB_HUD 控制器接口文档

## 控制器信息

| 控制器名称 | 控制器描述 |
|-----------|-----------|
| SHB_HUD | CAN-Product,SH-B_HUD |

## 公共字段

| 名称 | 类型 | 类别 | 描述 |
|------|------|------|------|
| SoftwareVer | string | 读 | R,SoftwareVer |
| PartNo | string | 读 | R,PartNo |
| BootVer | string | 读 | R,BootVer |
| SoftIDASCII | string | 读 | R,SoftID-ASCII |
| SoftIDHex | string | 读 | R,SoftID-hex |
| EcuHardwareNo | string | 读 | R,EcuHardwareNo |
| Voltage | double | 读 | R,volt |
| State | string | 读 | R,State |
| SecurityAccess0102Result | string | 读 | R,安全访问解锁0102结果 |

## 公共方法

| 名称 | 参数 | 类型 | 说明 | 示例 |
|------|------|------|------|------|
| StartCan | 无 | 方法 | StartCan | |
| StopCan | 无 | 方法 | StopCan | |
| SoftwareVerRead | 无 | 方法 | 读取软件版本信息 | |
| PartNoRead | 无 | 方法 | 读取硬件号 | |
| BootVerRead | 无 | 方法 | 读取Boot版本信息 | |
| SoftIDWrite | str: string | 方法 | 寫入软件识别码softID | SoftIDWrite("CDC051001") |
| SoftIDRead | 无 | 方法 | 读取软件识别码softID | |
| EcuHardwareNoWrite | str: string | 方法 | 寫入供应商定义ECU硬件版本号 | EcuHardwareNoWrite("00000001") |
| EcuHardwareNoRead | 无 | 方法 | 读取供应商定义ECU硬件版本号 | |
| VoltageRead | 无 | 方法 | 讀取供電電壓 | |
| StateRead | 无 | 方法 | 讀取狀態 | |
| EnterExtendMode | 无 | 方法 | 进入拓展模式 | |
| SecurityAccess0102 | 无 | 方法 | 安全访问解锁0102 | |
| AllReadTest | x: int | 方法 | 测试-全部读取 | AllReadTest(1) |
