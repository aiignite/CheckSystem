# Sgm458Plg 控制器接口文档

## 基本信息

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: CAN-Product,SGM458PLG

## 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| Sgm458Plg | CAN-Product,SGM458PLG | MosBreak | 方法 | 方法 | - | MOS管断开 | MosBreak() |
| Sgm458Plg | CAN-Product,SGM458PLG | MosFulldutyFactorOpen | 方法 | 方法 | - | MOS管全占空比开启 | MosFulldutyFactorOpen() |
| Sgm458Plg | CAN-Product,SGM458PLG | ExitCurrentCalibration | 方法 | 方法 | - | 退出电流校准 | ExitCurrentCalibration() |
| Sgm458Plg | CAN-Product,SGM458PLG | ReadCurrent | 方法 | 方法 | - | 读取电流值 | ReadCurrent() |
| Sgm458Plg | CAN-Product,SGM458PLG | ReadStatus | 方法 | 方法 | - | 读取状态 | ReadStatus() |
| Sgm458Plg | CAN-Product,SGM458PLG | UnlockMotor | 方法 | 方法 | - | 解锁电机 | UnlockMotor() |
| Sgm458Plg | CAN-Product,SGM458PLG | ClearFault | 方法 | 方法 | - | 清除故障 | ClearFault() |
| Sgm458Plg | CAN-Product,SGM458PLG | Awake | 方法 | 方法 | - | 唤醒 | Awake() |
| Sgm458Plg | CAN-Product,SGM458PLG | Sleep | 方法 | 方法 | - | 休眠 | Sleep() |
| Sgm458Plg | CAN-Product,SGM458PLG | EnterDefaultSession | 方法 | 方法 | - | 进入默认会话 | EnterDefaultSession() |
| Sgm458Plg | CAN-Product,SGM458PLG | EnterExtendedSession | 方法 | 方法 | - | 进入扩展会话 | EnterExtendedSession() |
| Sgm458Plg | CAN-Product,SGM458PLG | EnterProgramSession | 方法 | 方法 | - | 进入编程会话 | EnterProgramSession() |
| Sgm458Plg | CAN-Product,SGM458PLG | SecurityAccess0D0E | 方法 | 方法 | - | 安全访问0D0E | SecurityAccess0D0E() |
| Sgm458Plg | CAN-Product,SGM458PLG | Step1MasterKey | 方法 | 方法 | - | 步骤1-主密钥注入 | Step1MasterKey() |
| Sgm458Plg | CAN-Product,SGM458PLG | Step2UnlockKey | 方法 | 方法 | - | 步骤2-解锁密钥注入 | Step2UnlockKey() |
| Sgm458Plg | CAN-Product,SGM458PLG | Step3MacKey | 方法 | 方法 | - | 步骤3-MAC密钥注入 | Step3MacKey() |
| Sgm458Plg | CAN-Product,SGM458PLG | GenerateEcuIdAndIdCode | 方法 | 方法 | - | 生成ECU ID和ID代码 | GenerateEcuIdAndIdCode() |
| Sgm458Plg | CAN-Product,SGM458PLG | WriteEcuId | 方法 | 方法 | - | 写入ECU ID | WriteEcuId() |
| Sgm458Plg | CAN-Product,SGM458PLG | WriteIdCode | 方法 | 方法 | - | 写入ID代码 | WriteIdCode() |
| Sgm458Plg | CAN-Product,SGM458PLG | ReadEcuId | 方法 | 方法 | - | 读取ECU ID | ReadEcuId() |
| Sgm458Plg | CAN-Product,SGM458PLG | ReadIdCode | 方法 | 方法 | - | 读取ID代码 | ReadIdCode() |
| Sgm458Plg | CAN-Product,SGM458PLG | SaveData | 方法 | 方法 | - | 保存数据到数据库 | SaveData() |
| Sgm458Plg | CAN-Product,SGM458PLG | UnlockEcuId | 方法 | 方法 | - | 释放当前使用的ECU-ID | UnlockEcuId() |
| Sgm458Plg | CAN-Product,SGM458PLG | GenerateBarcode | 方法 | 方法 | string generalPartNo, string generalVpps, string seeyaoDuns | 生成二维码内容 | GenerateBarcode("26467265", "8210700000000X", "547656863") |
| Sgm458Plg | CAN-Product,SGM458PLG | PrintToXml | 方法 | 方法 | string printXmlFilePath, string sql, out int count | 导出XML文件 | PrintToXml("output.xml", "SELECT...", out count) |
| Sgm458Plg | CAN-Product,SGM458PLG | OverAgainReadEcuId | 方法 | 方法 | - | 返工-读取ECU ID | OverAgainReadEcuId() |
| Sgm458Plg | CAN-Product,SGM458PLG | OverAgainGenerateEcuIdAndIdCode | 方法 | 方法 | - | 返工-生成ECU ID和ID代码 | OverAgainGenerateEcuIdAndIdCode() |
| Sgm458Plg | CAN-Product,SGM458PLG | OverAgainSaveData | 方法 | 方法 | - | 返工-保存数据 | OverAgainSaveData() |
| Sgm458Plg | CAN-Product,SGM458PLG | DisableRxAndTxCommunication | 方法 | 方法 | - | 禁用Rx和Tx通信 | DisableRxAndTxCommunication() |
| Sgm458Plg | CAN-Product,SGM458PLG | EnableRxAndTxCommunication | 方法 | 方法 | - | 启用Rx和Tx通信 | EnableRxAndTxCommunication() |

## 字段列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| Sgm458Plg | CAN-Product,SGM458PLG | EcuId | 字段 | 字段 | - | R,ECU ID | 16进制字符串 |
| Sgm458Plg | CAN-Product,SGM458PLG | TrackInfo | 字段 | 字段 | - | R,追溯信息 | 追溯码 |
| Sgm458Plg | CAN-Product,SGM458PLG | SecurityAccessResult | 字段 | 字段 | - | R,安全访问0D0E结果 | OK/NG |
| Sgm458Plg | CAN-Product,SGM458PLG | MasterKey | 字段 | 字段 | - | R,Step1-MasterKey注入结果 | OK/NG |
| Sgm458Plg | CAN-Product,SGM458PLG | UnlockKey | 字段 | 字段 | - | R,Step2-UnlockKey注入结果 | OK/NG |
| Sgm458Plg | CAN-Product,SGM458PLG | MacKey | 字段 | 字段 | - | R,Step3-MacKey注入结果 | OK/NG |
| Sgm458Plg | CAN-Product,SGM458PLG | GeneratedEcuIdResult | 字段 | 字段 | - | R,生成的ECU ID结果 | 结果字符串 |
| Sgm458Plg | CAN-Product,SGM458PLG | GeneratedWriteIdResult | 字段 | 字段 | - | R,生成的写入ID结果 | 结果字符串 |
| Sgm458Plg | CAN-Product,SGM458PLG | WriteEcuIdResult | 字段 | 字段 | - | R,写入ECU ID结果 | OK/NG |
| Sgm458Plg | CAN-Product,SGM458PLG | WriteIdCodeResult | 字段 | 字段 | - | R,写入ID Code结果 | OK/NG |
| Sgm458Plg | CAN-Product,SGM458PLG | SaveDataResult | 字段 | 字段 | - | R,存储数据结果 | OK/NG |
| Sgm458Plg | CAN-Product,SGM458PLG | GeneratedBarcode | 字段 | 字段 | - | R,生成二维码内容 | 二维码字符串 |
| Sgm458Plg | CAN-Product,SGM458PLG | ServerIp | 字段 | 字段 | - | R/W,服务器IP地址 | 192.168.0.138 |
| Sgm458Plg | CAN-Product,SGM458PLG | ServerDataBase | 字段 | 字段 | - | R/W,服务器数据库名称 | IPMS |
| Sgm458Plg | CAN-Product,SGM458PLG | ServerUid | 字段 | 字段 | - | R/W,服务器用户名 | ipms |
| Sgm458Plg | CAN-Product,SGM458PLG | ServerPwd | 字段 | 字段 | - | R/W,服务器密码 | 密码 |

## 安全密钥注入流程
1. **SecurityAccess0D0E**: 安全访问
2. **Step1MasterKey**: 主密钥注入
3. **Step2UnlockKey**: 解锁密钥注入
4. **Step3MacKey**: MAC密钥注入

## ECU ID和ID代码操作
- **GenerateEcuIdAndIdCode**: 生成新的ECU ID和ID代码
- **WriteEcuId**: 写入ECU ID到设备
- **WriteIdCode**: 写入ID代码到设备
- **ReadEcuId**: 从设备读取ECU ID
- **ReadIdCode**: 从设备读取ID代码

## 数据库操作
- **SaveData**: 保存数据到SQL Server数据库
- **UnlockEcuId**: 释放当前使用的ECU-ID
- **PrintToXml**: 导出数据到XML文件

## 返工操作
- **OverAgainReadEcuId**: 返工时读取ECU ID
- **OverAgainGenerateEcuIdAndIdCode**: 返工时生成ECU ID和ID代码
- **OverAgainSaveData**: 返工时保存数据

## 通信方式
- **CAN总线**: 使用CAN总线进行通信
- **UDS协议**: 支持UDS诊断服务
- **SQL Server**: 数据存储到SQL Server数据库
