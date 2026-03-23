# S11LIscRearLamp 控制器接口文档

## 控制器信息

| 控制器名称 | 控制器描述 |
|-----------|-----------|
| S11LIscRearLamp | CAN-Product,S11L-ISC-后灯 |

## 公共字段

| 名称 | 类型 | 类别 | 描述 |
|------|------|------|------|
| FblVer | string | 读 | R,FblVer |
| AppVer | string | 读 | R,AppVer |
| RcmmLedDriverWorkSts | string | 读 | R,RCMM-LedDriver工作状态 |
| RcmmTurnSts | string | 读 | R,RCMM-转向灯信号状态 |
| RcmmBreakSts | string | 读 | R,RCMM-刹车灯信号状态 |
| RcmmLedDriverInternalFault | string | 读 | R,RCMM-LedDriver内部故障 |
| RcmlLedDriverWorkSts | string | 读 | R,RCML-LedDriver工作状态 |
| RcmlTurnSts | string | 读 | R,RCML-转向灯信号状态 |
| RcmlBreakSts | string | 读 | R,RCML-刹车灯信号状态 |
| RcmlLedDriverInternalFault | string | 读 | R,RCML-LedDriver内部故障 |
| RcmrLedDriverWorkSts | string | 读 | R,RCMR-LedDriver工作状态 |
| RcmrTurnSts | string | 读 | R,RCMR-转向灯信号状态 |
| RcmrBreakSts | string | 读 | R,RCMR-刹车灯信号状态 |
| RcmrLedDriverInternalFault | string | 读 | R,RCMR-LedDriver内部故障 |
| FaultRead | string | 读 | R,故障信息读取 |
| AppFlag | string | 读 | R,AppFlag |
| WriteFlagResult | string | 读 | R,写APP标准位结果 |

## 公共方法

| 名称 | 参数 | 类型 | 说明 | 示例 |
|------|------|------|------|------|
| SetRcmm | 无 | 方法 | 设置为B灯 | |
| SetRcml | 无 | 方法 | 设置为A灯左 | |
| SetRcmr | 无 | 方法 | 设置为A灯右 | |
| StartCanMsg | 无 | 方法 | 开启CAN消息 | |
| StopCanMsg | 无 | 方法 | 关闭CAN消息 | |
| IscHardwareOn | 无 | 方法 | 开启ISC硬线控制模式 | |
| IscHardwareOff | 无 | 方法 | 关闭ISC硬线控制模式 | |
| ReadAppVer | 无 | 方法 | 读AppVer | |
| ReadFblVer | 无 | 方法 | 读FblVer | |
| AllLedOn | gray: string | 方法 | 全部点亮 | AllLedOn("100") |
| AllOddOn | gray: string | 方法 | 全部奇数点亮 | AllOddOn("100") |
| AllEvenOn | gray: string | 方法 | 全部偶数点亮 | AllEvenOn("100") |
| AllLedOff | 无 | 方法 | 全部关闭 | |
| SingleOn | 无 | 方法 | 单颗扫描开 | |
| SingleOff | 无 | 方法 | 单颗扫描关 | |
| SingleAllOn | 无 | 方法 | 逐颗点亮开 | |
| SingleAllOff | 无 | 方法 | 逐颗点亮关 | |
| TailNormalOn | gray: string | 方法 | 位置灯亮 | TailNormalOn("100") |
| TailHdOn | 无 | 方法 | 位置灯高亮-仅A灯 | |
| TailLdOn | 无 | 方法 | 位置灯低亮-仅A灯 | |
| TailLeftOn | gray: string | 方法 | 左位置灯亮-仅B灯 | TailLeftOn("100") |
| TailRightOn | gray: string | 方法 | 右位置灯亮-仅B灯 | TailRightOn("100") |
| TailOff | 无 | 方法 | 位置灯关闭 | |
| TurnNormalOn | gray: string | 方法 | 转向灯亮 | TurnNormalOn("100") |
| TurnHdOn | 无 | 方法 | 转向灯高亮-仅A灯 | |
| TurnLdOn | 无 | 方法 | 转向灯低亮-仅A灯 | |
| TurnLeftOn | gray: string | 方法 | 左转向灯亮-仅B灯 | TurnLeftOn("100") |
| TurnRightOn | gray: string | 方法 | 右转向灯亮-仅B灯 | TurnRightOn("100") |
| TurnOff | 无 | 方法 | 转向灯关闭 | |
| StopOn | gray: string | 方法 | 制动灯亮 | StopOn("100") |
| StopLeftOn | gray: string | 方法 | 左制动灯亮-仅B灯 | StopLeftOn("100") |
| StopRightOn | gray: string | 方法 | 右制动灯亮-仅B灯 | StopRightOn("100") |
| StopOff | 无 | 方法 | 制动灯灭 | |
| IscOn | gray: string | 方法 | ISC打开-仅B灯 | IscOn("100") |
| IscOff | 无 | 方法 | ISC关闭-仅B灯 | |
| FaultDetect | 无 | 方法 | 读取故障信息 | |
| WriteAppFlagRcml | 无 | 方法 | 写A灯左APP标志位 | |
| ReadAppFlagRcml | 无 | 方法 | 读A灯左APP标志位 | |
| WriteAppFlagRcmr | 无 | 方法 | 写A灯右APP标志位 | |
| WriteAppFlagRcmm | 无 | 方法 | 写B灯APP标志位 | |