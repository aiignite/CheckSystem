# Bx1EEcu

## Controller Description

CAN-Product,BX1E-ECU

## Public Fields

| 名称 | 描述 | 类型 |
|------|------|------|
| PartNo | R,零件号 | string |
| GeelyNo | R,零件号(Geely) | string |
| HardwareVer | R,硬件版本号 | string |
| SoftwareVer | R,软件版本号 | string |
| SerialNo | R,序列号 | string |
| WriteAndReadSerialNoResult | R,写入并读取生产序列号 | string |
| Boot1Ver | R,读取软件版本号(Boot 1) | string |
| Boot2Ver | R,读取软件版本号(Boot 2) | string |
| DbcVer | R,读取软件版本号(DBC Version) | string |
| ServerIp | R/W,服务器IP地址 | string |
| ServerDataBase | R/W,服务器数据库名称 | string |
| ServerUid | R/W,服务器用户名 | string |
| ServerPwd | R/W,服务器用密码 | string |
| SecurityAccess0506Result | R,安全访问0506结果 | string |

## Public Methods

| 方法名称 | 描述 | 参数 |
|----------|------|------|
| CanAwake | CAN唤醒 |  |
| CanSleep | CAN休眠 |  |
| ReadPartNo | 读零件号 |  |
| ReadGeelyNo | 零件号(Geely) |  |
| ReadHardwareVer | 硬件版本号 |  |
| ReadSoftwareVer | 读取软件版本号(APP) |  |
| ReadBootVer | 读取软件版本号Boot |  |
| ReadDbcVer | 读取软件版本号(DBC Version) |  |
| WriteAndReadSerialNo | 写入并读取生产序列号 |  |
| EnterDefaultSession | 进入正常模式 |  |
| EnterExtendedSession | 进入拓展模式 |  |
| EnterProgramSession | 进入编程模式 |  |
| DisableRxAndTxCommunication | 关闭正常通信 |  |
| SecurityAccess0506 | 安全访问0D0E |  |
| UsbIn | USB挂载 |  |
| UsbOut | USB卸载 |  |
| LcdOn | LCD显示ON |  |
| LcdOff | LCD显示OFF |  |
| Shadow1On | 功能测试1On |  |
| Shadow2On | 功能测试2On |  |
| Shadow3On | 功能测试3On |  |
| ShadowOff | 功能测试Off |  |
| ClearDtc | 清除错误 |  |
