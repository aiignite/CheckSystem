# SgmModuleDebug 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: CAN-Product,SGM模块调试类

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| SgmModuleDebug | CAN-Product,SGM模块调试类 | Can | CanBus | 字段 | - | CAN总线实例 | - |
| SgmModuleDebug | CAN-Product,SGM模块调试类 | Awake | void | 方法 | 无 | 唤醒 | Awake() |
| SgmModuleDebug | CAN-Product,SGM模块调试类 | Sleep | void | 方法 | 无 | 休眠 | Sleep() |
| SgmModuleDebug | CAN-Product,SGM模块调试类 | EnterDefaultSession | void | 方法 | 无 | 进入正常模式 | EnterDefaultSession() |
| SgmModuleDebug | CAN-Product,SGM模块调试类 | EnterExtendedSession | void | 方法 | 无 | 进入拓展模式 | EnterExtendedSession() |
| SgmModuleDebug | CAN-Product,SGM模块调试类 | DisableRxAndTxCommunication | void | 方法 | 无 | 关闭正常通信 | DisableRxAndTxCommunication() |
| SgmModuleDebug | CAN-Product,SGM模块调试类 | EcuId | string | 字段 | - | R,ECU_ID-F0F3 | - |
| SgmModuleDebug | CAN-Product,SGM模块调试类 | TrackInfo | string | 字段 | - | R,追溯信息MTC-F0B4 | - |
| SgmModuleDebug | CAN-Product,SGM模块调试类 | ReadEcuIdAndTrackInfoThenTrackSql | void | 方法 | 无 | ReadF0F3和F0B4-ECU_ID和追溯信息 | ReadEcuIdAndTrackInfoThenTrackSql() |
| SgmModuleDebug | CAN-Product,SGM模块调试类 | SecurityAccessResult | string | 字段 | - | R,安全访问0D0E结果 | - |
| SgmModuleDebug | CAN-Product,SGM模块调试类 | MasterKey | string | 字段 | - | R,Step1-MasterKey注入结果 | - |
| SgmModuleDebug | CAN-Product,SGM模块调试类 | UnlockKey | string | 字段 | - | R,Step2-UnlockKey注入结果 | - |
| SgmModuleDebug | CAN-Product,SGM模块调试类 | MacKey | string | 字段 | - | R,Step3-MacKey注入结果 | - |
| SgmModuleDebug | CAN-Product,SGM模块调试类 | SecurityAccess0D0E | void | 方法 | 无 | 安全访问0D0E | SecurityAccess0D0E() |
| SgmModuleDebug | CAN-Product,SGM模块调试类 | PrivateTestKey | void | 方法 | 无 | 调试解锁Key | PrivateTestKey() |
