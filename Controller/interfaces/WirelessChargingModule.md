# WirelessChargingModule 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: CAN-Product,50W无线充电模块

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| WirelessChargingModule | R,存储数据结果 | SaveDataResult | string | 字段 | - | 存储数据结果 | - |
| WirelessChargingModule | R/W,服务器IP地址 | ServerIp | string | 字段 | - | 服务器IP地址 | - |
| WirelessChargingModule | R/W,服务器数据库名称 | ServerDataBase | string | 字段 | - | 服务器数据库名称 | - |
| WirelessChargingModule | R/W,服务器用户名 | ServerUid | string | 字段 | - | 服务器用户名 | - |
| WirelessChargingModule | R/W,服务器用密码 | ServerPwd | string | 字段 | - | 服务器用密码 | - |
| WirelessChargingModule | R/W,追溯条码 | TrackBarcode | string | 字段 | - | 追溯条码 | - |
| WirelessChargingModule | R,追溯结果 | TrackResult | string | 字段 | - | 追溯结果 | - |
| WirelessChargingModule | 读取追溯 | LoadTrack | 方法 | 方法 | int mtcIndex, int mtcLen | 读取追溯 | - |
| WirelessChargingModule | 执行写入追溯信息 | ExecuteWriteTrackInfo | 方法 | 方法 | int bootPartNo, string bootVer, string ddi, int mec | 执行写入追溯信息 | - |
| WirelessChargingModule | 清除追溯 | ClearTrack | 方法 | 方法 | - | 清除追溯 | - |
| WirelessChargingModule | 上传追溯到服务器 | UploadTrackToServer | 方法 | 方法 | - | 上传追溯到服务器 | - |
| WirelessChargingModule | R/W,校准等待时间MS | CALIBRATION_WAIT_TIME | int | 字段 | - | 校准等待时间(毫秒) | - |
| WirelessChargingModule | R,线圈1-Q值 | Q1Value | float | 字段 | - | 线圈1的Q值 | - |
| WirelessChargingModule | R,线圈1-频率 | Q1Frequency | float | 字段 | - | 线圈1的频率 | - |
| WirelessChargingModule | R,线圈1-阻抗 | Q1Impedance | float | 字段 | - | 线圈1的阻抗 | - |
| WirelessChargingModule | R,线圈2-Q值 | Q2Value | float | 字段 | - | 线圈2的Q值 | - |
| WirelessChargingModule | R,线圈2-频率 | Q2Frequency | float | 字段 | - | 线圈2的频率 | - |
| WirelessChargingModule | R,线圈2-阻抗 | Q2Impedance | float | 字段 | - | 线圈2的阻抗 | - |
| WirelessChargingModule | R,线圈3-Q值 | Q3Value | float | 字段 | - | 线圈3的Q值 | - |
| WirelessChargingModule | R,线圈3-频率 | Q3Frequency | float | 字段 | - | 线圈3的频率 | - |
| WirelessChargingModule | R,线圈3-阻抗 | Q3Impedance | float | 字段 | - | 线圈3的阻抗 | - |
| WirelessChargingModule | 执行完整的Q值校准流程 | PerformQValueCalibration | 方法 | 方法 | - | 执行完整的Q值校准流程 | - |
| WirelessChargingModule | 打开CAN | StartCan | 方法 | 方法 | - | 打开CAN | - |
| WirelessChargingModule | 关闭CAN | StopCan | 方法 | 方法 | - | 关闭CAN | - |
| WirelessChargingModule | 打开PowerMode | PowerModeOn | 方法 | 方法 | - | 打开PowerMode | - |
| WirelessChargingModule | 关闭PowerMode | PowerModeOff | 方法 | 方法 | - | 关闭PowerMode | - |
