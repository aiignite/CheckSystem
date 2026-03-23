# SmartGlassDisplay 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: CAN-Product,三立LCD屏

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| SmartGlassDisplay | CAN-Product,三立LCD屏 | CanFd | CanBus | 字段 | - | CAN FD总线实例 | - |
| SmartGlassDisplay | CAN-Product,三立LCD屏 | BCM_Crc1Val | byte | 字段 | - | BCM CRC1值 | - |
| SmartGlassDisplay | CAN-Product,三立LCD屏 | Lamp_ExtrnlTailLmpOnReq | byte | 字段 | - | 外部尾灯灯请求 | - |
| SmartGlassDisplay | CAN-Product,三立LCD屏 | Lamp_HdLmpWlcmCmd | byte | 字段 | - | 近光欢迎命令 | - |
| SmartGlassDisplay | CAN-Product,三立LCD屏 | Lamp_HdLmpEscortShortCmd | byte | 字段 | - | 近光护航短命令 | - |
| SmartGlassDisplay | CAN-Product,三立LCD屏 | Lamp_AutoLtSnsrNightSta | byte | 字段 | - | 自动灯光夜间状态 | - |
| SmartGlassDisplay | CAN-Product,三立LCD屏 | CF_AVN_IFSActVehSpd_Set | byte | 字段 | - | AVN IFS实际车速设置 | - |
| SmartGlassDisplay | CAN-Product,三立LCD屏 | CF_AVN_DWL_SelectNvalueSet | byte | 字段 | - | AVN DWL选择N值设置 | - |
| SmartGlassDisplay | CAN-Product,三立LCD屏 | Warn_DrvDrSwSta | byte | 字段 | - | 驾驶员门开关状态 | - |
| SmartGlassDisplay | CAN-Product,三立LCD屏 | Warn_AsstDrSwSta | byte | 字段 | - | 副驾驶员门开关状态 | - |
| SmartGlassDisplay | CAN-Product,三立LCD屏 | Warn_RrLftDrSwSta | byte | 字段 | - | 左后门开关状态 | - |
| SmartGlassDisplay | CAN-Product,三立LCD屏 | Warn_RrRtDrSwSta | byte | 字段 | - | 右后门开关状态 | - |
| SmartGlassDisplay | CAN-Product,三立LCD屏 | WHL_SpdFLVal | uint | 字段 | - | 左前轮速度值 | - |
| SmartGlassDisplay | CAN-Product,三立LCD屏 | WHL_SpdFRVal | uint | 字段 | - | 右前轮速度值 | - |
| SmartGlassDisplay | CAN-Product,三立LCD屏 | WHL_SpdRLVal | uint | 字段 | - | 左后轮速度值 | - |
| SmartGlassDisplay | CAN-Product,三立LCD屏 | WHL_SpdRRVal | uint | 字段 | - | 右后轮速度值 | - |
| SmartGlassDisplay | CAN-Product,三立LCD屏 | StartCan | void | 方法 | 无 | 打开CAN | StartCan() |
| SmartGlassDisplay | CAN-Product,三立LCD屏 | StopCan | void | 方法 | 无 | 关闭CAN | StopCan() |
| SmartGlassDisplay | CAN-Product,三立LCD屏 | Set_Lamp_ExtrnlTailLmpOnReq | void | 方法 | byte value | 设置外部尾灯灯请求 | Set_Lamp_ExtrnlTailLmpOnReq(1) |
| SmartGlassDisplay | CAN-Product,三立LCD屏 | Set_Lamp_HdLmpWlcmCmd | void | 方法 | byte value | 设置近光欢迎命令 | Set_Lamp_HdLmpWlcmCmd(1) |
| SmartGlassDisplay | CAN-Product,三立LCD屏 | Set_Lamp_HdLmpEscortShortCmd | void | 方法 | byte value | 设置近光护航短命令 | Set_Lamp_HdLmpEscortShortCmd(1) |
| SmartGlassDisplay | CAN-Product,三立LCD屏 | Set_Lamp_AutoLtSnsrNightSta | void | 方法 | byte value | 设置自动灯光夜间状态 | Set_Lamp_AutoLtSnsrNightSta(1) |
| SmartGlassDisplay | CAN-Product,三立LCD屏 | Set_CF_AVN_IFSActVehSpd_Set | void | 方法 | byte value | 设置AVN IFS车速 | Set_CF_AVN_IFSActVehSpd_Set(5) |
| SmartGlassDisplay | CAN-Product,三立LCD屏 | Set_CF_AVN_DWL_SelectNvalueSet | void | 方法 | byte value | 设置AVN DWL N值 | Set_CF_AVN_DWL_SelectNvalueSet(5) |
| SmartGlassDisplay | CAN-Product,三立LCD屏 | Set_Warn_DrvDrSwSta | void | 方法 | byte value | 设置驾驶员门状态 | Set_Warn_DrvDrSwSta(1) |
| SmartGlassDisplay | CAN-Product,三立LCD屏 | Set_Warn_AsstDrSwSta | void | 方法 | byte value | 设置副驾驶员门状态 | Set_Warn_AsstDrSwSta(1) |
| SmartGlassDisplay | CAN-Product,三立LCD屏 | Set_Warn_RrLftDrSwSta | void | 方法 | byte value | 设置左后门状态 | Set_Warn_RrLftDrSwSta(1) |
| SmartGlassDisplay | CAN-Product,三立LCD屏 | Set_Warn_RrRtDrSwSta | void | 方法 | byte value | 设置右后门状态 | Set_Warn_RrRtDrSwSta(1) |
| SmartGlassDisplay | CAN-Product,三立LCD屏 | Set_WHL_SpdFLVal | void | 方法 | double value | 设置左前轮速度(0~512) | Set_WHL_SpdFLVal(100) |
| SmartGlassDisplay | CAN-Product,三立LCD屏 | Set_WHL_SpdFRVal | void | 方法 | double value | 设置右前轮速度(0~512) | Set_WHL_SpdFRVal(100) |
| SmartGlassDisplay | CAN-Product,三立LCD屏 | Set_WHL_SpdRLVal | void | 方法 | double value | 设置左后轮速度(0~512) | Set_WHL_SpdRLVal(100) |
| SmartGlassDisplay | CAN-Product,三立LCD屏 | Set_WHL_SpdRRVal | void | 方法 | double value | 设置右后轮速度(0~512) | Set_WHL_SpdRRVal(100) |
