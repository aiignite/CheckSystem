# S11LIscRearLampVave 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: CAN-Product,S11L-ISC-后灯VAVE

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| S11LIscRearLampVave | FblVer | FblVer | string | 字段 | - | R,FblVer | - |
| S11LIscRearLampVave | AppVer | AppVer | string | 字段 | - | R,AppVer | - |
| S11LIscRearLampVave | InternalAppVer | InternalAppVer | string | 字段 | - | R,InternalAppVer | - |
| S11LIscRearLampVave | HardwareVer | HardwareVer | string | 字段 | - | R,HardwareVer | - |
| S11LIscRearLampVave | RCMM-LedDriver工作状态 | RcmmLedDriverWorkSts | string | 字段 | - | R,RCMM-LedDriver工作状态 | - |
| S11LIscRearLampVave | RCMM-转向灯信号状态 | RcmmTurnSts | string | 字段 | - | R,RCMM-转向灯信号状态 | - |
| S11LIscRearLampVave | RCMM-刹车灯信号状态 | RcmmBreakSts | string | 字段 | - | R,RCMM-刹车灯信号状态 | - |
| S11LIscRearLampVave | RCMM-LedDriver内部故障 | RcmmLedDriverInternalFault | string | 字段 | - | R,RCMM-LedDriver内部故障 | - |
| S11LIscRearLampVave | RCML-LedDriver工作状态 | RcmlLedDriverWorkSts | string | 字段 | - | R,RCML-LedDriver工作状态 | - |
| S11LIscRearLampVave | RCML-转向灯信号状态 | RcmlTurnSts | string | 字段 | - | R,RCML-转向灯信号状态 | - |
| S11LIscRearLampVave | RCML-刹车灯信号状态 | RcmlBreakSts | string | 字段 | - | R,RCML-刹车灯信号状态 | - |
| S11LIscRearLampVave | RCML-LedDriver内部故障 | RcmlLedDriverInternalFault | string | 字段 | - | R,RCML-LedDriver内部故障 | - |
| S11LIscRearLampVave | RCMR-LedDriver工作状态 | RcmrLedDriverWorkSts | string | 字段 | - | R,RCMR-LedDriver工作状态 | - |
| S11LIscRearLampVave | RCMR-转向灯信号状态 | RcmrTurnSts | string | 字段 | - | R,RCMR-转向灯信号状态 | - |
| S11LIscRearLampVave | RCMR-刹车灯信号状态 | RcmrBreakSts | string | 字段 | - | R,RCMR-刹车灯信号状态 | - |
| S11LIscRearLampVave | RCMR-LedDriver内部故障 | RcmrLedDriverInternalFault | string | 字段 | - | R,RCMR-LedDriver内部故障 | - |
| S11LIscRearLampVave | 故障信息读取 | FaultRead | string | 字段 | - | R,故障信息读取 | - |
| S11LIscRearLampVave | 写APP标准位结果 | WriteFlagResult | string | 字段 | - | R,写APP标准位结果 | - |
| S11LIscRearLampVave | 写项目号标志位结果 | WriteProjFlagRcmmResult | string | 字段 | - | R,写项目号标志位结果 | - |
| S11LIscRearLampVave | 读项目号标志位结果 | ReadProjFlagRcmmResult | string | 字段 | - | R,读项目号标志位结果 | - |
| S11LIscRearLampVave | 写电流信息结果 | WriteCurrRcmmResult | string | 字段 | - | R,写电流信息结果 | - |
| S11LIscRearLampVave | 读电流信息结果 | ReadCurrRcmmResult | string | 字段 | - | R,读电流信息结果 | - |
| S11LIscRearLampVave | 设置为B灯 | SetRcmm | void | 方法 | - | 设置为B灯 | - |
| S11LIscRearLampVave | 开启CAN消息 | StartCanMsg | void | 方法 | - | 开启CAN消息 | - |
| S11LIscRearLampVave | 关闭CAN消息 | StopCanMsg | void | 方法 | - | 关闭CAN消息 | - |
| S11LIscRearLampVave | 开启ISC硬线控制模式 | IscHardwareOn | void | 方法 | - | 开启ISC硬线控制模式 | - |
| S11LIscRearLampVave | 关闭ISC硬线控制模式 | IscHardwareOff | void | 方法 | - | 关闭ISC硬线控制模式 | - |
| S11LIscRearLampVave | 读取故障信息 | FaultDetect | void | 方法 | - | 读取故障信息 | - |
| S11LIscRearLampVave | 写B灯APP标志位0x0101 | WriteAppFlagRcmm | void | 方法 | - | 写B灯APP标志位0x0101 | - |
| S11LIscRearLampVave | 读写B灯项目号标志位0x0103 | WriteProjFlagRcmm | void | 方法 | - | 读写B灯项目号标志位0x0103 | - |
| S11LIscRearLampVave | 读写B灯电流信息0x0104 | WriteCurrInfo | void | 方法 | - | 读写B灯电流信息0x0104 | - |
| S11LIscRearLampVave | 全部点亮 | AllLedOn | void | 方法 | string gray | 全部点亮 | - |
| S11LIscRearLampVave | 全部奇数点亮 | AllOddOn | void | 方法 | string gray | 全部奇数点亮 | - |
| S11LIscRearLampVave | 全部偶数点亮 | AllEvenOn | void | 方法 | string gray | 全部偶数点亮 | - |
| S11LIscRearLampVave | 全部关闭 | AllLedOff | void | 方法 | - | 全部关闭 | - |
| S11LIscRearLampVave | 单颗扫描开 | SingleOn | void | 方法 | - | 单颗扫描开 | - |
| S11LIscRearLampVave | 单颗扫描关 | SingleOff | void | 方法 | - | 单颗扫描关 | - |
| S11LIscRearLampVave | 逐颗点亮开 | SingleAllOn | void | 方法 | - | 逐颗点亮开 | - |
| S11LIscRearLampVave | 逐颗点亮关 | SingleAllOff | void | 方法 | - | 逐颗点亮关 | - |
| S11LIscRearLampVave | 位置灯亮 | TailNormalOn | void | 方法 | string gray | 位置灯亮 | - |
| S11LIscRearLampVave | 位置灯高亮-仅A灯 | TailHdOn | void | 方法 | - | 位置灯高亮-仅A灯 | - |
| S11LIscRearLampVave | 位置灯低亮-仅A灯 | TailLdOn | void | 方法 | - | 位置灯低亮-仅A灯 | - |
| S11LIscRearLampVave | 左位置灯亮-仅B灯 | TailLeftOn | void | 方法 | string gray | 左位置灯亮-仅B灯 | - |
| S11LIscRearLampVave | 右位置灯亮-仅B灯 | TailRightOn | void | 方法 | string gray | 右位置灯亮-仅B灯 | - |
| S11LIscRearLampVave | 位置灯关闭 | TailOff | void | 方法 | - | 位置灯关闭 | - |
| S11LIscRearLampVave | 转向灯亮 | TurnNormalOn | void | 方法 | string gray | 转向灯亮 | - |
| S11LIscRearLampVave | 转向灯高亮-仅A灯 | TurnHdOn | void | 方法 | - | 转向灯高亮-仅A灯 | - |
| S11LIscRearLampVave | 转向灯低亮-仅A灯 | TurnLdOn | void | 方法 | - | 转向灯低亮-仅A灯 | - |
| S11LIscRearLampVave | 左转向灯亮-仅B灯 | TurnLeftOn | void | 方法 | string gray | 左转向灯亮-仅B灯 | - |
| S11LIscRearLampVave | 右转向灯亮-仅B灯 | TurnRightOn | void | 方法 | string gray | 右转向灯亮-仅B灯 | - |
| S11LIscRearLampVave | 转向灯关闭 | TurnOff | void | 方法 | - | 转向灯关闭 | - |
| S11LIscRearLampVave | 制动灯亮 | StopOn | void | 方法 | string gray | 制动灯亮 | - |
| S11LIscRearLampVave | 左制动灯亮-仅B灯 | StopLeftOn | void | 方法 | string gray | 左制动灯亮-仅B灯 | - |
| S11LIscRearLampVave | 右制动灯亮-仅B灯 | StopRightOn | void | 方法 | string gray | 右制动灯亮-仅B灯 | - |
| S11LIscRearLampVave | 制动灯灭 | StopOff | void | 方法 | - | 制动灯灭 | - |
| S11LIscRearLampVave | ISC打开-仅B灯 | IscOn | void | 方法 | string gray | ISC打开-仅B灯 | - |
| S11LIscRearLampVave | ISC关闭-仅B灯 | IscOff | void | 方法 | - | ISC关闭-仅B灯 | - |
