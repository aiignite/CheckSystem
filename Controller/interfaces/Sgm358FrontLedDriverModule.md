# Sgm358FrontLedDriverModule 控制器接口文档

## 基本信息

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: CAN-Product,SGM358前灯

## 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| Sgm358FrontLedDriverModule | CAN-Product,SGM358前灯 | Download | 方法 | 方法 | string dataFilePath | 下载数据到模块 | Download("firmware.hex") |
| Sgm358FrontLedDriverModule | CAN-Product,SGM358前灯 | EnterSleepMode | 方法 | 方法 | - | 进入休眠模式 | EnterSleepMode() |
| Sgm358FrontLedDriverModule | CAN-Product,SGM358前灯 | ExitSleepMode | 方法 | 方法 | - | 退出休眠模式 | ExitSleepMode() |
| Sgm358FrontLedDriverModule | CAN-Product,SGM358前灯 | EnterExtendedSession | 方法 | 方法 | - | 进入扩展会话 | EnterExtendedSession() |
| Sgm358FrontLedDriverModule | CAN-Product,SGM358前灯 | SetupTestMode | 方法 | 方法 | - | 设置测试模式 | SetupTestMode() |
| Sgm358FrontLedDriverModule | CAN-Product,SGM358前灯 | SetupNormorMode | 方法 | 方法 | - | 设置正常模式 | SetupNormorMode() |
| Sgm358FrontLedDriverModule | CAN-Product,SGM358前灯 | ReadMode | 方法 | 方法 | - | 读取当前模式 | ReadMode() |
| Sgm358FrontLedDriverModule | CAN-Product,SGM358前灯 | ReadNtc | 方法 | 方法 | - | 读取NTC温度传感器 | ReadNtc() |
| Sgm358FrontLedDriverModule | CAN-Product,SGM358前灯 | WritePartNo | 方法 | 方法 | string partNo | 写入零件号 | WritePartNo("12345678") |
| Sgm358FrontLedDriverModule | CAN-Product,SGM358前灯 | ReadPartNo | 方法 | 方法 | - | 读取零件号 | ReadPartNo() |
| Sgm358FrontLedDriverModule | CAN-Product,SGM358前灯 | LeftParkLmpOn | 方法 | 方法 | - | 左驻车灯开 | LeftParkLmpOn() |
| Sgm358FrontLedDriverModule | CAN-Product,SGM358前灯 | LeftParkLmpOff | 方法 | 方法 | - | 左驻车灯关 | LeftParkLmpOff() |
| Sgm358FrontLedDriverModule | CAN-Product,SGM358前灯 | RightParkLmpOn | 方法 | 方法 | - | 右驻车灯开 | RightParkLmpOn() |
| Sgm358FrontLedDriverModule | CAN-Product,SGM358前灯 | RightParkLmpOff | 方法 | 方法 | - | 右驻车灯关 | RightParkLmpOff() |
| Sgm358FrontLedDriverModule | CAN-Product,SGM358前灯 | LeftDrlOn | 方法 | 方法 | - | 左日行灯开 | LeftDrlOn() |
| Sgm358FrontLedDriverModule | CAN-Product,SGM358前灯 | LeftDrlOff | 方法 | 方法 | - | 左日行灯关 | LeftDrlOff() |
| Sgm358FrontLedDriverModule | CAN-Product,SGM358前灯 | RightDrlOn | 方法 | 方法 | - | 右日行灯开 | RightDrlOn() |
| Sgm358FrontLedDriverModule | CAN-Product,SGM358前灯 | RightDrlOff | 方法 | 方法 | - | 右日行灯关 | RightDrlOff() |
| Sgm358FrontLedDriverModule | CAN-Product,SGM358前灯 | LeftTurnHoldOn | 方法 | 方法 | - | 左转向灯常亮 | LeftTurnHoldOn() |
| Sgm358FrontLedDriverModule | CAN-Product,SGM358前灯 | LeftTurnOff | 方法 | 方法 | - | 左转向灯关 | LeftTurnOff() |
| Sgm358FrontLedDriverModule | CAN-Product,SGM358前灯 | RightTurnHoldOn | 方法 | 方法 | - | 右转向灯常亮 | RightTurnHoldOn() |
| Sgm358FrontLedDriverModule | CAN-Product,SGM358前灯 | RightTurnOff | 方法 | 方法 | - | 右转向灯关 | RightTurnOff() |

## UDS诊断服务说明

- **EnterExtendedSession**: 进入扩展会话模式,用于执行诊断服务
- **SetupTestMode**: 设置测试模式
- **SetupNormorMode**: 设置正常工作模式

- **ReadMode**: 读取当前工作模式

## 灯光控制说明
- **驻车灯(Park Lamp)**: LeftParkLmp/RightParkLmp
- **日行灯(DRL)**: LeftDrl/RightDrl
- **转向灯(Turn Lamp)**: LeftTurn/RightTurn

## 通信方式
- **CAN总线**: 使用CAN总线进行通信
- **UDS协议**: 支持UDS诊断服务
