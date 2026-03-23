# Vx1IsdLamp 控制器接口文档

## 控制器基本信息

| 控制器名称 | 控制器描述 |
|------------|------------|
| Vx1IsdLamp | CAN-Product,VX1-ISD前后灯 |

## 公共属性

| 名称 | 类型 | 类别 | 说明 | 示例 |
|------|------|------|------|------|
| CanFd | CanBus | 属性 | CAN FD总线 | - |
| LedGray | string | 属性 | R/W,LED点亮灰度值 | - |
| SoftwareVersion | string | 属性 | R,软件版本号 | - |
| HardwareVersion | string | 属性 | R,硬件版本号 | - |
| FaultDetectResult | string | 属性 | R,故障检测读取结果 | - |
| ReadCurrent | string | 属性 | R,标定电流读取 | - |

## 公共方法

| 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|------|------|------|------|------|------|
| ChangeToVx1IsdFontBLeft | void | 方法 | 无 | 切换成ISD前灯B左 | ChangeToVx1IsdFontBLeft() |
| ChangeToVx1IsdFontBRight | void | 方法 | 无 | 切换成ISD前灯B右 | ChangeToVx1IsdFontBRight() |
| ChangeToVx1IsdRclALeft | void | 方法 | 无 | 切换成ISD后灯A左 | ChangeToVx1IsdRclALeft() |
| ChangeToVx1IsdRclARight | void | 方法 | 无 | 切换成ISD后灯A右 | ChangeToVx1IsdRclARight() |
| FaultDetect | void | 方法 | 无 | 读取故障信息 | FaultDetect() |
| LedOnRedOrWhite | void | 方法 | ledIndex: string | 以红光或白光单独点亮 | LedOnRedOrWhite("1") |
| LedOnYellow | void | 方法 | ledIndex: string | 以黄光单独点亮 | LedOnYellow("1") |
| LedOff | void | 方法 | ledIndex: string | 单独熄灭 | LedOff("1") |
| LedRangeOnRedOrWhite | void | 方法 | startEnd: string | 以红光或白光范围点亮 | LedRangeOnRedOrWhite("1:10") |
| LedRangeOddOnRedOrWhite | void | 方法 | maxIndex: string | 以红光或白光奇数范围点亮 | LedRangeOddOnRedOrWhite("10") |
| LedRangeDualOnRedOrWhite | void | 方法 | maxIndex: string | 以红光或白光偶数范围点亮 | LedRangeDualOnRedOrWhite("10") |
| LedRangeOnYellow | void | 方法 | startEnd: string | 以黄光范围点亮 | LedRangeOnYellow("1:10") |
| LedRangeOddOnYellow | void | 方法 | maxIndex: string | 以黄光奇数范围点亮 | LedRangeOddOnYellow("10") |
| LedRangeDualOnYellow | void | 方法 | maxIndex: string | 以黄光偶数范围点亮 | LedRangeDualOnYellow("10") |
| LedRangeOff | void | 方法 | startEnd: string | 范围熄灭 | LedRangeOff("1:10") |
| ReadSoftwareVersion | void | 方法 | 无 | 读软件版本号 | ReadSoftwareVersion() |
| ReadHardwareVersion | void | 方法 | 无 | 读硬件版本号 | ReadHardwareVersion() |
| WriteAndReadCurrent | void | 方法 | writeValue: string | 读写电流标定 | WriteAndReadCurrent("FF00") |