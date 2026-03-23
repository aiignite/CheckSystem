# OsramEviyos 控制器接口文档

## 控制器信息

| 控制器名称 | 控制器描述 |
|-----------|-----------|
| OsramEviyos |  |

## 公共字段

| 名称 | 类型 | 类别 | 描述 |
|------|------|------|------|
| MySerialPort | MySerialPort | 总线 | 串口 |
| OsramBarcode | string | 读/写 | R/W,欧司朗二维码内容 |
| OsramBinFilePath | string | 读 | BIN文件路径 |
| ProductBarcode1 | string | 写 | W,工位1产品二维码 |
| ProductBarcode2 | string | 读 | R,获取工位2产品二维码 |
| BinStrFromOsramFile | string | 读 | R,BIN读取全部内容 |
| DmcFromOsramBinFile | string | 读 | R,BIN读取DMC |
| DeviceIdFromOsramBinFile | string | 读 | R,BIN读取DeviceId |
| OsramBarcodes | string | 读 | R,根据这个二维码找对应的BIN文件 |
| OsramFile | string | 读 | R,找到的BIN文件 |
| BinStrFromProgramRead | string | 读 | R,BIN读取全部内容 |
| DmcFromProgramRead | string | 读 | R,BIN读取DMC |
| DeviceIdFromProgramRead | string | 读 | R,BIN读取DeviceId |
| CompareDeviceIdAndDmcResult | string | 读 | R,反读flash与源文件-比对device和dmc |
| ReadBinFilePath | string | 读 | 反读BIN文件路径 |
| RemoteBinFilePath | string | 读/写 | R/W,bin文件的远程目录 |
| RemoteBinFileUserName | string | 读/写 | R/W,bin文件的远程目录用户名 |
| RemoteBinFilePassword | string | 读/写 | R/W,bin文件的远程目录密码 |
| IsRepeat | string | 读 | 重复标记 |
| isGetBar | bool | 读 | 是否获取条码 |
| RegRead70To7F | string | 读 | 寄存器读取70-7F |
| RegRead80To8F | string | 读 | 寄存器读取80-8F |
| RegRead90To9F | string | 读 | 寄存器读取90-9F |
| RegReadA0ToAf | string | 读 | 寄存器读取A0-AF |
| RegReadB0ToBf | string | 读 | 寄存器读取B0-BF |
| RegReadC0ToCf | string | 读 | 寄存器读取C0-CF |
| RegReadD0ToDf | string | 读 | 寄存器读取D0-DF |
| RegReadE0ToEf | string | 读 | 寄存器读取E0-EF |
| CR | string | 读 | R,CR |
| DTR | string | 读 | R,DTR |
| TSTIM | string | 读 | R,TSTIM |
| RATO | string | 读 | R,RATO |
| MB | string | 读 | R,MB |
| DLSET | string | 读 | R,DLSET |
| NVMCR | string | 读 | R,NVMCR |
| CURR | string | 读 | R,CURR |
| HWSET | string | 读 | R,HWSET |
| FWCT | string | 读 | R,FWCT |
| FRT | string | 读 | R,FRT |
| EGSET | string | 读 | R,EGSET |
| TSTDR | string | 读 | R,TSTDR |
| PWMR | string | 读 | R,PWMR |
| EVDDPT | string | 读 | R,EVDDPT |
| TSPDR | string | 读 | R,TSPDR |
| FSTXR | string | 读 | R,FSTXR |
| ETEMPT | string | 读 | R,ETEMPT |
| MBDR | string | 读 | R,MBDR |
| FSTYR | string | 读 | R,FSTYR |
| NTB | string | 读 | R,NTB |
| ErrorPositionAnalysisResult | string | 读 | 错误位置分析结果 |
| Led1ErrorType-Led64ErrorType | string | 读 | R,LED1-64错误类型 |
| Led1Position-Led64Position | string | 读 | R,LED1-64位置 |

## 公共方法

| 名称 | 参数 | 类型 | 说明 | 示例 |
|------|------|------|------|------|
| StartCan | 无 | 方法 | 开启CAN |  |
| StopCan | 无 | 方法 | 关闭CAN |  |
| ReadLedError | 无 | 方法 | 读LED ERROR |  |
| ConnectHikBarcodeScaner | ipPort: string | 方法 | 连接海康扫码枪 | ConnectHikBarcodeScaner("192.168.1.1:5000") |
| ReadBarcode | cmd: string | 方法 | 向海康扫码枪发送读码指令 | ReadBarcode("T") |
| CheckRepeatByLocaDataBase | 无 | 方法 | 本地数据库重复检查 |  |
| GetProductBarcode2ByPlms | 无 | 方法 | 根据产品1二维码获取产品2二维码 |  |
| ReadBinByBarcode | 无 | 方法 | 根据二维码读取对应BIN文件 |  |
| ReadBinFromProgramReadBinFile | 无 | 方法 | 从反读的BIN文件读取内容 |  |
| ReadBinFromProgramReadBinFileChange0103 | 无 | 方法 | 从反读的BIN文件读取内容-偏移地址从0000开始 |  |
| ReadRemoteBinByBarcode | 无 | 方法 | 根据二维码读取对应BIN文件 |  |
| ReadConfigRegisters | 无 | 方法 | 读配置寄存器 |  |
| ClearBarcode | 无 | 方法 | 清除条码 |  |
| ClearReadErrorHistory | 无 | 方法 | 清除读取错误历史 |  |
| ReadErrorAndAnalysis | maxCount: int | 方法 | 读取错误并分析 |  |
