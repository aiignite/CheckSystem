# RobotController 控制器接口文档

## 控制器信息

| 控制器名称 | 控制器描述 |
|-----------|-----------|
| RobotController | 多机器人UDP控制器 |

## 公共字段

| 名称 | 类型 | 类别 | 描述 |
|------|------|------|------|
| Robot0StateIdle | string | 读 | R,机器人状态_是否IDLE |
| Robot0StateStandStill | string | 读 | R,机器人状态_是否StandStill |
| Robot0StateJogging | string | 读 | R,机器人状态_是否Jogging |
| Robot0StateRunBlock | string | 读 | R,机器人状态_是否RunBlock |
| Robot0StateRunProgram | string | 读 | R,机器人状态_是否RunProgram |
| Robot0StateServoReady | string | 读 | R,机器人状态_是否ServoReady |
| Robot0StateError | string | 读 | R,机器人状态_是否Error |
| Robot0StateAll | string | 读 | R,机器人状态_全状态 |
| Robot0ErrorAll | string | 读 | R,机器人状态_Error状态 |
| Robot1StateIdle | string | 读 | R,机器人状态_是否IDLE |
| Robot1StateStandStill | string | 读 | R,机器人状态_是否StandStill |
| Robot1StateJogging | string | 读 | R,机器人状态_是否Jogging |
| Robot1StateRunBlock | string | 读 | R,机器人状态_是否RunBlock |
| Robot1StateRunProgram | string | 读 | R,机器人状态_是否RunProgram |
| Robot1StateServoReady | string | 读 | R,机器人状态_是否ServoReady |
| Robot1StateError | string | 读 | R,机器人状态_是否Error |
| Robot1StateAll | string | 读 | R,机器人状态_全状态 |
| Robot1ErrorAll | string | 读 | R,机器人状态_Error状态 |
| Robot2StateIdle | string | 读 | R,机器人状态_是否IDLE |
| Robot2StateStandStill | string | 读 | R,机器人状态_是否StandStill |
| Robot2StateJogging | string | 读 | R,机器人状态_是否Jogging |
| Robot2StateRunBlock | string | 读 | R,机器人状态_是否RunBlock |
| Robot2StateRunProgram | string | 读 | R,机器人状态_是否RunProgram |
| Robot2StateServoReady | string | 读 | R,机器人状态_是否ServoReady |
| Robot2StateError | string | 读 | R,机器人状态_是否Error |
| Robot2StateAll | string | 读 | R,机器人状态_全状态 |
| Robot2ErrorAll | string | 读 | R,机器人状态_Error状态 |
| Robot3StateIdle | string | 读 | R,机器人状态_是否IDLE |
| Robot3StateStandStill | string | 读 | R,机器人状态_是否StandStill |
| Robot3StateJogging | string | 读 | R,机器人状态_是否Jogging |
| Robot3StateRunBlock | string | 读 | R,机器人状态_是否RunBlock |
| Robot3StateRunProgram | string | 读 | R,机器人状态_是否RunProgram |
| Robot3StateServoReady | string | 读 | R,机器人状态_是否ServoReady |
| Robot3StateError | string | 读 | R,机器人状态_是否Error |
| Robot3StateAll | string | 读 | R,机器人状态_全状态 |
| Robot3ErrorAll | string | 读 | R,机器人状态_Error状态 |
| R0M0-R0M99 | string | 读 | R0,M0-M99 |
| R1M0-R1M99 | string | 读 | R1,M0-M99 |
| R2M0-R2M99 | string | 读 | R2,M0-M99 |
| R3M0-R3M99 | string | 读 | R3,M0-M99 |
| R3M70Finished | string | 读 | R3,M70抓取完成 |
| IoInputDi0-IoInputDi11 | string | 读 | R,输入_DI0-DI11 |
| IoOutputDo0-IoOutputDo7 | string | 读/写 | R/W,继电器_DO0-DO7 |

## 公共方法

| 名称 | 参数 | 类型 | 说明 | 示例 |
|------|------|------|------|------|
| InitRemoteIpAddress | ipPort: string | 方法 | 初始化远程IP地址 | InitRemoteIpAddress("192.168.1.1:5000") |
| RunProgram | robotID: string, programName: string | 方法 | 运行程序 | RunProgram("0", "Program1") |
| ServoOn | robotID: string | 方法 | 使能ON | ServoOn("0") |
| ServoOff | robotID: string | 方法 | 使能OFF | ServoOff("0") |
| RobotReset | 无 | 方法 | 机器人重置 | |
| ResetMCode | robotID: string, mCode: string | 方法 | M码重置 | ResetMCode("0", "R0M0") |
| AddMatrix | code: string | 方法 | 添加一个矩阵 | AddMatrix("matrix1:prog1\|prog2") |
| RemoveMaxtrix | matrixName: string | 方法 | 移除一个矩阵 | RemoveMaxtrix("matrix1") |
| ClearMatrix | 无 | 方法 | 移除所有矩阵 | |
| RunMatrix | robotID: string, matrixName: string | 方法 | 运行矩阵 | RunMatrix("0", "matrix1") |
| ResetMatrix | matrixName: string | 方法 | 重置矩阵 | ResetMatrix("matrix1") |