# T9M

## Controller Description

CAN-Product,T9M照地灯

## Public Fields

| 名称 | 描述 | 类型 |
|------|------|------|
| BootVer | R,读取Boot版本号 | string |
| PartNo | R,读取零件号 | string |
| HwVer | R,读取硬件版本号 | string |
| SwVer | R,读取软件版本号 | string |
| AppVer | R,读取App版本号 | string |
| MaterialVersionNumber | R,读取素材版本号 | string |
| FactoryMode | R,读取工厂模式 | string |
| SwChildVer | R,读取软件子版本号 | string |
| ReadMsg269Result | R,读取0x269报文测试结果 | string |
| HighVoltError | R,高压故障 | string |
| LowVoltError | R,低压故障 | string |
| NormalVoltNoError | R,正常电压无故障 | string |

## Public Methods

| 方法名称 | 描述 | 参数 |
|----------|------|------|
| StartCan | 打开CAN |  |
| StopCan | 关闭CAN |  |
| ClearReadResult | 清除所有读取信息结果 |  |
| ReadBootVer | 读取Boot版本号 |  |
| ReadAppVer | 读取App版本号 |  |
| ReadHwVer | 读取硬件版本号 |  |
| ReadSwVer | 读取软件版本号 |  |
| ReadPartNo | 读取零件号 |  |
| ReadMaterialVersionNumber | 读取素材版本号 |  |
| ReadFactoryMode | 读取工厂模式 |  |
| ReadSwChildVer | 读取软件子版本号 |  |
