# Sgm358Ecu 控制器接口文档

## 控制器信息

| 控制器名称 | 控制器描述 |
|-----------|-----------|
| Sgm358Ecu | CAN-Product,SGM358ECU |

## 公共字段

| 名称 | 类型 | 类别 | 描述 |
|------|------|------|------|
| ServerIp | string | 读/写 | R/W,服务器IP地址 |
| ServerDataBase | string | 读/写 | R/W,服务器数据库名称 |
| ServerUid | string | 读/写 | R/W,服务器用户名 |
| ServerPwd | string | 读/写 | R/W,服务器用密码 |
| EcuId | string | 读 | R,读ECU_ID-01F3 |
| BootLoaderPartNumber | string | 读 | R,读引导程序零件号-01C0 |
| ApplicationSoftwarePartNumber | string | 读 | R,读应用程序零件号-01C1 |
| CalibrationSoftwarePartNumber | string | 读 | R,读配置文件零件号-01C2 |
| EndModulePartNumber | string | 读 | R,读当前零件号-01CB |
| BaseModulePartNumber | string | 读 | R,读初始零件号-F191 |
| BootLoaderAlphaCode | string | 读 | R,读引导程序版本号-01D0 |
| ApplicationSoftwareAlphaCode | string | 读 | R,读应用程序版本号-01D1 |
| CalibrationSoftwareAlphaCode | string | 读 | R,读配置文件版本号-01D2 |
| EndModuleAlphaCode | string | 读 | R,读当前版本号-01DB |
| BaseModuleAlphaCode | string | 读 | R,读初始版本号-01DC |
| Mec | double | 读 | R,读MEC值-01A0 |
| Ddi | string | 读 | R,读DDI值-019A |
| EcuVolt | double | 读 | R,读ECU供电电压-F088 |
| SensorPowerCircuit | double | 读 | R,读传感器电源回路-8240 |
| FrontSensorSignalLoop | double | 读 | R,读前传感器信号回路-832D |
| RearSensorSignalLoop | double | 读 | R,读后传感器信号回路-832E |
| ClearFaultMemoryResult | string | 读 | R,清除错误结果 |
| ReadrFaultMemoryResult | string | 读 | R,读取错误结果-190209 |
| Vpps | string | 读 | R,读取VPPS-01AB |
| Duns | string | 读 | R,读取DUNS-01B3 |

## 公共方法

| 名称 | 参数 | 类型 | 说明 | 示例 |
|------|------|------|------|------|
| AhlActivate | 无 | 方法 | 唤醒 | |
| AhlNotActivate | 无 | 方法 | 休眠 | |
| EnterDefaultSession | 无 | 方法 | 进入正常模式 | |
| EnterExtendedSession | 无 | 方法 | 进入拓展模式 | |
| EnterProgramSession | 无 | 方法 | 进入编程模式 | |
| SecurityAccess | subFunc: string | 方法 | 解锁SeedKey | SecurityAccess("01") |
| ReadEcuId | 无 | 方法 | 读ECU_ID-01F3 | |
| WriteEcuId | partNo: string, baseModulePartNumber: string, fna: string, upc1: string, upc2: string, baseModuleAlphaCode: string, endModulePartNumber: string, endModuleAlphaCode: string | 方法 | 写ECU_ID-D004 | WriteEcuId("358", "26381122", "9770A", "1", "2C", "AA", "26381122", "AA") |
| ReadBootLoaderPartNumber | 无 | 方法 | 读引导程序零件号-01C0 | |
| ReadBootLoaderAlphaCode | 无 | 方法 | 读引导程序版本号-01D0 | |
| ReadBaseModulePartNumber | 无 | 方法 | 读初始零件号-F191 | |
| ReadBaseModuleAlphaCode | 无 | 方法 | 读初始版本号-01DC | |
| ReadEndModulePartNumber | 无 | 方法 | 读当前零件号-01CB | |
| ReadEndModuleAlphaCode | 无 | 方法 | 读当前版本号-01DB | |
| ReadApplicationSoftwarePartNumber | 无 | 方法 | 读应用程序零件号-01C1 | |
| ReadApplicationSoftwareAlphaCode | 无 | 方法 | 读应用程序版本号-01D1 | |
| ReadCalibrationSoftwarePartNumber | 无 | 方法 | 读配置文件零件号-01C2 | |
| ReadCalibrationSoftwareAlphaCode | 无 | 方法 | 读配置文件版本号-01D2 | |
| ReadMec | 无 | 方法 | 读MEC值-01A0 | |
| ReadDdi | 无 | 方法 | 读DDI值-019A | |
| ReadEcuVolt | 无 | 方法 | 读ECU供电电压-F088 | |
| ReadSensorPowerCircuit | 无 | 方法 | 读传感器电源回路-8240 | |
| ReadFrontSensorSignalLoop | 无 | 方法 | 读前传感器信号回路-832D | |
| ReadRearSensorSignalLoop | 无 | 方法 | 读后传感器信号回路-832E | |
| ReadVpps | 无 | 方法 | 读取VPPS-01AB | |
| ReadDuns | 无 | 方法 | 读取DUNS-01B3 | |
| ReadFaultMemory | 无 | 方法 | 读取故障-190209 | |
| ClearFaultMemory | 无 | 方法 | 清除故障 | |
