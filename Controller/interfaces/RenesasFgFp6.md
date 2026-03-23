# RenesasFgFp6 控制器接口文档

## 控制器信息

| 控制器名称 | 控制器描述 |
|-----------|-----------|
| RenesasFgFp6 | 瑞萨RENESAS烧写器 |

## 公共字段

| 名称 | 类型 | 类别 | 描述 |
|------|------|------|------|
| ProgResult | string | 读 | R,烧写结果 |
| FlashMemory | string | 读 | R,FlashMemory |
| OptionBytes | string | 读 | R,OptionBytes |
| IcuSStatus | string | 读 | R,ICU-S |
| SetIdCodeResult | string | 读 | R,写ID-Code结果 |
| IdCodeToSet | string | 读/写 | R/W,需要写入的ID-Code |
| IdCodeFromGet | string | 读 | R,读取的ID-Code |
| IdCodeCompare | string | 读 | R,读取与写入的ID-Code对比 |
| AuthId | string | 读/写 | R/W,Authentication ID |
| UniqueCodeStartAddr | string | 读/写 | R/W,UniqueCode起始地址 |
| UniqueCodeHexPattern | string | 读/写 | R/W,UniqueCode16进制参数 |
| PrmFile | string | 读 | R,参数文件路径,需全英文路径 |
| SetFile | string | 读 | R,设置文件路径,需全英文路径 |
| LodFile | string | 读 | R,烧写文件路径,需全英文路径 |
| LoadFileResult | string | 读 | R,烧写结果 |

## 公共方法

| 名称 | 参数 | 类型 | 说明 | 示例 |
|------|------|------|------|------|
| ConnectFp6 | portBaudTate: string | 方法 | 连接FP6 | ConnectFp6("COM1:9600") |
| ConnectDevice | 无 | 方法 | 切换烧写区域 |  |
| ChangeProgArea | index: string | 方法 | 切换烧写区域 | ChangeProgArea("0") |
| SetUniqueCode | 无 | 方法 | SetUniqueCode |  |
| EraseAndProgram | timeOutMs: string | 方法 | 擦除程序后烧写 | EraseAndProgram("60000") |
| SetFlashOption | 无 | 方法 | SetFlashOption |  |
| GetOptionBytes | 无 | 方法 | GetOptionBytes |  |
| ReadFlashMemoryViaUniqueCode | 无 | 方法 | 根据UniqueCode读FlashMemory |  |
| ReadFlashMemory | startAddress: string, endAddress: string | 方法 | ReadFlashMemory | ReadFlashMemory("0x0000", "0x1000") |
| SetIdCode | 无 | 方法 | 写ID-Code |  |
| ChangeAuthId | 无 | 方法 | ChangeAuthenticationIdCode |  |
| ReadIcuS | 无 | 方法 | Read ICU-S |  |
| InitTeraTermComPort | com: string | 方法 | 初始化TeraTerm串口 | InitTeraTermComPort("COM1") |
| UploadFile | index: string | 方法 | 上传文件 | UploadFile("0") |
