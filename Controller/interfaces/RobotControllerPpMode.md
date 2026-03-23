# RobotControllerPpMode 控制器接口文档

## 控制器信息

| 控制器名称 | 控制器描述 |
|-----------|-----------|
| RobotControllerPpMode | 机器人托盘编程模式控制器 |

## 公共字段

| 名称 | 类型 | 类别 | 描述 |
|------|------|------|------|
| AllAxisesIdle | bool | 读 | R,RobotSate-AllAxisesIdle |
| AllAxisesServoOn | bool | 读 | R,RobotSate-AllAxisesServoOn |
| AllAxisesReady | bool | 读 | R,RobotSate-AllAxisesReady |
| AllAxisesRunning | bool | 读 | R,RobotSate-AllAxisesRunning |
| AxisError | bool | 读 | R,RobotSate-AxisError |
| EmergencyStop | bool | 读 | R,RobotSate-EmergencyStop |
| ErrorAxisId | string | 读 | R,RobotSate-ErrorAxisId |
| Si0-Si40 | bool | 读/写 | R/W,通用输入SI0-SI40 |
| So0-So40 | bool | 读/写 | R/W,通用输出SO0-SO40 |
| Di0-Di11 | string | 读 | R,输入_DI0-DI11 |
| Do0-Do7 | string | 读/写 | R/W,继电器_DO0-DO7,低边输出_DO4-DO7 |
| Pattet0RunIndex-Pattet9RunIndex | int | 读 | R,Pallet0-Pallet9当前点位索引 |

## 公共方法

| 名称 | 参数 | 类型 | 说明 | 示例 |
|------|------|------|------|------|
| InitLocalLocalIpAddress | ipPort: string | 方法 | 初始化本地IP地址 | InitLocalLocalIpAddress("192.168.1.50:5000") |
| InitRemoteIpAddress | ipPort: string | 方法 | 初始化远程IP地址 | InitRemoteIpAddress("192.168.1.100:5001") |
| RobotRun | paras: Dictionary | 方法 | 多轴运行 | RobotRun(paras) |
| RunEventNull | index: string | 方法 | 运行事件-空 | RunEventNull("0") |
| RunEventServoOn | index: string | 方法 | 运行事件-伺服ON | RunEventServoOn("0") |
| RunEventServoOff | index: string | 方法 | 运行事件-伺服OFF | RunEventServoOff("0") |
| RunEventEmergencyStop | index: string | 方法 | 运行事件-急停 | RunEventEmergencyStop("0") |
| RunEventReset | index: string | 方法 | 运行事件-复位 | RunEventReset("0") |
| RunEventHome | index: string | 方法 | 运行事件-回原点 | RunEventHome("0") |
| RunEventSetHomeOffset | index: string | 方法 | 设置原点偏移 | RunEventSetHomeOffset("0") |
| RunEventStop | index: string | 方法 | 运行事件-停止 | RunEventStop("0") |
| RunEventHalt | index: string | 方法 | 运行事件-暂停 | RunEventHalt("0") |
| RunEventContinue | index: string | 方法 | 运行事件-继续 | RunEventContinue("0") |
| RunEventNullAll | 无 | 方法 | 运行事件-全部空 | |
| RunEventServoOnAll | 无 | 方法 | 运行事件-全部伺服ON | |
| RunEventServoOffAll | 无 | 方法 | 运行事件-全部伺服OFF | |
| RunEventEmergencyStopAll | 无 | 方法 | 运行事件-全部急停 | |
| RunEventResetAll | 无 | 方法 | 运行事件-全部复位 | |
| RunEventHomeAll | 无 | 方法 | 运行事件-全部回原点 | |
| RunEventStopAll | 无 | 方法 | 运行事件-全部停止 | |
| RunEventHaltAll | 无 | 方法 | 运行事件-全部暂停 | |
| RunEventContinueAll | 无 | 方法 | 运行事件-全部继续 | |
| RunEventNullMultiple | axitList: List<int> | 方法 | 运行事件-多轴空 | RunEventNullMultiple([0,1,2]) |
| RunEventServoOnMultiple | axitList: List<int> | 方法 | 运行事件-多轴伺服ON | RunEventServoOnMultiple([0,1,2]) |
| RunEventServoOffMultiple | axitList: List<int> | 方法 | 运行事件-多轴伺服OFF | RunEventServoOffMultiple([0,1,2]) |
| RunEventEmergencyStopMultiple | axitList: List<int> | 方法 | 运行事件-多轴急停 | RunEventEmergencyStopMultiple([0,1,2]) |
| RunEventResetMultiple | axitList: List<int> | 方法 | 运行事件-多轴复位 | RunEventResetMultiple([0,1,2]) |
| RunEventHomeMultiple | axitList: List<int> | 方法 | 运行事件-多轴回原点 | RunEventHomeMultiple([0,1,2]) |
| RunEventStopMultiple | axitList: List<int> | 方法 | 运行事件-多轴停止 | RunEventStopMultiple([0,1,2]) |
| RunEventHaltMultiple | axitList: List<int> | 方法 | 运行事件-多轴暂停 | RunEventHaltMultiple([0,1,2]) |
| RunEventContinueMultiple | axitList: List<int> | 方法 | 运行事件-多轴继续 | RunEventContinueMultiple([0,1,2]) |
| StartProgram | name: string | 方法 | 启动程序 | StartProgram("Program1") |
| RobotRunProgram | name: string, blockIndex: int, endIndex: int | 方法 | 运行机器人程序 | RobotRunProgram("Program1", 0, -1) |
| PattetReset | palletIndex: string | 方法 | 托盘复位 | PattetReset("0") |
| PattetBlockSkip | palletIndex: string, blockSkipTo: string | 方法 | 托盘块跳转 | PattetBlockSkip("0", "5") |
| ReLoadPallet | 无 | 方法 | 重新加载托盘配置 | |
| ReadFlash | 无 | 方法 | 读取Flash | |
| WriteFlash | sendValues: byte[] | 方法 | 写入Flash | WriteFlash(bytes) |