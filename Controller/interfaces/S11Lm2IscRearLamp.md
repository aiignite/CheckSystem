# S11Lm2IscRearLamp 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: CAN-Product,S11L-M2后组合灯

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| S11Lm2IscRearLamp | 是否接RCLB | IsHaveRclb | bool | 字段 | - | R/W,是否接RCLB | - |
| S11Lm2IscRearLamp | 是否接RCLA | IsHaveRcla | bool | 字段 | - | R/W,是否接RCLA | - |
| S11Lm2IscRearLamp | RclbAppVer | RclbAppVer | string | 字段 | - | R,RclbAppVer | - |
| S11Lm2IscRearLamp | RclbSoftwareVer | RclbSoftwareVer | string | 字段 | - | R,RclbSoftwareVer | - |
| S11Lm2IscRearLamp | RclaLeftAppVer | RclaLeftAppVer | string | 字段 | - | R,RclaLeftAppVer | - |
| S11Lm2IscRearLamp | RclaLeftSoftwareVer | RclaLeftSoftwareVer | string | 字段 | - | R,RclaLeftSoftwareVer | - |
| S11Lm2IscRearLamp | RclaRightAppVer | RclaRightAppVer | string | 字段 | - | R,RclaRightAppVer | - |
| S11Lm2IscRearLamp | RclaRightSoftwareVer | RclaRightSoftwareVer | string | 字段 | - | R,RclaRightSoftwareVer | - |
| S11Lm2IscRearLamp | 开启CAN | StartCan | void | 方法 | - | 开启CAN | - |
| S11Lm2IscRearLamp | 关闭CAN | StopCan | void | 方法 | - | 关闭CAN | - |
| S11Lm2IscRearLamp | 开启硬线控制 | StartHardwareControl | void | 方法 | - | 开启硬线控制 | - |
| S11Lm2IscRearLamp | 关闭硬线控制 | StopHardwareControl | void | 方法 | - | 关闭硬线控制 | - |
| S11Lm2IscRearLamp | LeftTailOn | LeftTailOn | void | 方法 | - | LeftTailOn | - |
| S11Lm2IscRearLamp | LeftTailHdOn | LeftTailHdOn | void | 方法 | - | LeftTailHdOn | - |
| S11Lm2IscRearLamp | LeftTailOff | LeftTailOff | void | 方法 | - | LeftTailOff | - |
| S11Lm2IscRearLamp | LeftStopOn | LeftStopOn | void | 方法 | - | LeftStopOn | - |
| S11Lm2IscRearLamp | LeftStopOff | LeftStopOff | void | 方法 | - | LeftStopOff | - |
| S11Lm2IscRearLamp | LeftTurnOn | LeftTurnOn | void | 方法 | - | LeftTurnOn | - |
| S11Lm2IscRearLamp | LeftTurnHdOn | LeftTurnHdOn | void | 方法 | - | LeftTurnHdOn | - |
| S11Lm2IscRearLamp | LeftTurnOff | LeftTurnOff | void | 方法 | - | LeftTurnOff | - |
| S11Lm2IscRearLamp | RightTailOn | RightTailOn | void | 方法 | - | RightTailOn | - |
| S11Lm2IscRearLamp | RightTailHdOn | RightTailHdOn | void | 方法 | - | RightTailHdOn | - |
| S11Lm2IscRearLamp | RightTailOff | RightTailOff | void | 方法 | - | RightTailOff | - |
| S11Lm2IscRearLamp | RightStopOn | RightStopOn | void | 方法 | - | RightStopOn | - |
| S11Lm2IscRearLamp | RightStopOff | RightStopOff | void | 方法 | - | RightStopOff | - |
| S11Lm2IscRearLamp | RightTurnOn | RightTurnOn | void | 方法 | - | RightTurnOn | - |
| S11Lm2IscRearLamp | RightTurnHdOn | RightTurnHdOn | void | 方法 | - | RightTurnHdOn | - |
| S11Lm2IscRearLamp | RightTurnOff | RightTurnOff | void | 方法 | - | RightTurnOff | - |
| S11Lm2IscRearLamp | 红色奇数打开 | RedOddOn | void | 方法 | - | 红色奇数打开 | - |
| S11Lm2IscRearLamp | 红色双数打开 | RedEvenOn | void | 方法 | - | 红色双数打开 | - |
| S11Lm2IscRearLamp | 红色所有打开 | RedAllOn | void | 方法 | - | 红色所有打开 | - |
| S11Lm2IscRearLamp | 红色所有关闭 | RedAllOff | void | 方法 | - | 红色所有关闭 | - |
| S11Lm2IscRearLamp | 黄色奇数打开 | YellowOddOn | void | 方法 | - | 黄色奇数打开 | - |
| S11Lm2IscRearLamp | 黄色偶数打开 | YellowEvenOff | void | 方法 | - | 黄色偶数打开 | - |
| S11Lm2IscRearLamp | 黄色所有打开 | YellowAllOn | void | 方法 | - | 黄色所有打开 | - |
| S11Lm2IscRearLamp | 黄色所有关闭 | YellowAllOff | void | 方法 | - | 黄色所有关闭 | - |
| S11Lm2IscRearLamp | ReadRclbVer | ReadRclbVer | void | 方法 | - | ReadRclbVer | - |
| S11Lm2IscRearLamp | ReadRclaLeftVer | ReadRclaLeftVer | void | 方法 | - | ReadRclaLeftVer | - |
| S11Lm2IscRearLamp | ReadRclaRightVer | ReadRclaRightVer | void | 方法 | - | ReadRclaRightVer | - |
