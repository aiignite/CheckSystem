# Sgm458Msm 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: CAN-Product,SGM458座椅模块

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| Sgm458Msm | 当前选择的产品类别 | SelectModeName | string | 字段 | - | R,当前选择的产品类别 | - |
| Sgm458Msm | 诊断-当前模式 | CurrentSession | string | 字段 | - | R,诊断-当前模式 | - |
| Sgm458Msm | 诊断-安全访问0D0E结果 | SecurityAccess0D0EResult | string | 字段 | - | R,诊断-安全访问0D0E结果 | - |
| Sgm458Msm | 诊断-当前零件号-EndModulePartNumber-F1CB | EndModulePartNumber | string | 字段 | - | R,诊断-当前零件号-EndModulePartNumber-F1CB | - |
| Sgm458Msm | 诊断-当前版本号-EndModuleAlphaCode-F1DB | EndModuleAlphaCode | string | 字段 | - | R,诊断-当前版本号-当前版本号-F1DB | - |
| Sgm458Msm | 初始零件号-BaseModulePartNumber-F1CC | BaseModulePartNumber | string | 字段 | - | R,初始零件号-BaseModulePartNumber-F1CC | - |
| Sgm458Msm | 初始版本号-BaseModuleAlphaCode-F1DC | BaseModuleAlphaCode | string | 字段 | - | R,初始版本号-BaseModuleAlphaCode-F1DC | - |
| Sgm458Msm | 诊断-ApplicationSoftware-F181 | ApplicationSoftware | string | 字段 | - | R,诊断-ApplicationSoftware-F181 | - |
| Sgm458Msm | 诊断-MEC-F1A0 | ManufacturersEnableCounter | int | 字段 | - | R,诊断-MEC-F1A0 | - |
| Sgm458Msm | 诊断-追溯信息-F0B4 | TrackInfo | string | 字段 | - | R,诊断-追溯信息-F0B4 | - |
| Sgm458Msm | 诊断-ECU_ID-F0F3 | EcuId | string | 字段 | - | R,诊断-ECU_ID-F0F3 | - |
| Sgm458Msm | 诊断-当前座椅位置-FD16 | SeatPosConfig | string | 字段 | - | R,诊断-当前座椅位置-FD16 | - |
| Sgm458Msm | 诊断-当前座椅车型配置-FD18 | SeatNumConfig | string | 字段 | - | R,诊断-当前座椅车型配置-FD18 | - |
| Sgm458Msm | 2排L | Set2L | void | 方法 | - | 2排L | - |
| Sgm458Msm | 2排R | Set2R | void | 方法 | - | 2排R | - |
| Sgm458Msm | 3排L | Set3L | void | 方法 | - | 3排L | - |
| Sgm458Msm | 3排R | Set3R | void | 方法 | - | 3排R | - |
| Sgm458Msm | 唤醒 | Awake | void | 方法 | - | 唤醒 | - |
| Sgm458Msm | 休眠 | Sleep | void | 方法 | - | 休眠 | - |
| Sgm458Msm | 诊断-进入正常模式 | EnterDefaultSession | void | 方法 | - | 诊断-进入正常模式 | - |
| Sgm458Msm | 诊断-进入拓展模式 | EnterExtendedSession | void | 方法 | - | 诊断-进入拓展模式 | - |
| Sgm458Msm | 诊断-进入编程模式 | EnterProgramSession | void | 方法 | - | 诊断-进入编程模式 | - |
| Sgm458Msm | 诊断-关闭正常通信 | DisableRxAndTxCommunication | void | 方法 | - | 诊断-关闭正常通信 | - |
| Sgm458Msm | 诊断-软件复位 | EcuReset | void | 方法 | - | 诊断-软件复位 | - |
| Sgm458Msm | 诊断-安全访问0D0E | SecurityAccess0D0E | void | 方法 | - | 诊断-安全访问0D0E | - |
| Sgm458Msm | 诊断-ReadF1CB-当前零件号-EndModulePartNumber | ReadEndModulePartNumber | void | 方法 | - | 诊断-ReadF1CB-当前零件号-EndModulePartNumber | - |
| Sgm458Msm | 诊断-ReadF1DB-当前版本号-EndModuleAlphaCode | ReadEndModuleAlphaCode | void | 方法 | - | 诊断-ReadF1DB-当前版本号-EndModuleAlphaCode | - |
| Sgm458Msm | ReadF1CC-初始零件号-BaseModulePartNumber | ReadBaseModulePartNumber | void | 方法 | - | ReadF1CC-初始零件号-BaseModulePartNumber | - |
| Sgm458Msm | ReadF1DC-初始版本号-BaseModuleAlphaCode | ReadBaseModuleAlphaCode | void | 方法 | - | ReadF1DC-初始版本号-BaseModuleAlphaCode | - |
| Sgm458Msm | 诊断-ReadF0F3和F0B4-ECU_ID和追溯信息 | ReadEcuIdAndTrackInfo | void | 方法 | - | 诊断-ReadF0F3和F0B4-ECU_ID和追溯信息 | - |
| Sgm458Msm | 诊断-ReadF181-ApplicationSoftware | ReadApplicationSoftware | void | 方法 | - | 诊断-ReadF181-ApplicationSoftware | - |
