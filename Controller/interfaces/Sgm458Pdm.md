# Sgm458Pdm 控制器接口文档

## 基本信息

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: CAN-Product,SGM458PDM

## 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| Sgm458Pdm | CAN-Product,SGM458PDM | MosBreak | 方法 | 方法 | - | MOS管断开 | MosBreak() |
| Sgm458Pdm | CAN-Product,SGM458PDM | MosFulldutyFactorOpen | 方法 | 方法 | - | MOS管全占空比开启 | MosFulldutyFactorOpen() |
| Sgm458Pdm | CAN-Product,SGM458PDM | ExitCurrentCalibration | 方法 | 方法 | - | 退出电流校准 | ExitCurrentCalibration() |
| Sgm458Pdm | CAN-Product,SGM458PDM | ReadCurrent | 方法 | 方法 | - | 读取电流值 | ReadCurrent() |
| Sgm458Pdm | CAN-Product,SGM458PDM | ReadStatus | 方法 | 方法 | - | 读取状态 | ReadStatus() |
| Sgm458Pdm | CAN-Product,SGM458PDM | UnlockMotor | 方法 | 方法 | - | 解锁电机 | UnlockMotor() |
| Sgm458Pdm | CAN-Product,SGM458PDM | ClearFault | 方法 | 方法 | - | 清除故障 | ClearFault() |
| Sgm458Pdm | CAN-Product,SGM458PDM | Awake | 方法 | 方法 | - | 唤醒 | Awake() |
| Sgm458Pdm | CAN-Product,SGM458PDM | Sleep | 方法 | 方法 | - | 休眠 | Sleep() |
| Sgm458Pdm | CAN-Product,SGM458PDM | EnterDefaultSession | 方法 | 方法 | - | 进入默认会话 | EnterDefaultSession() |
| Sgm458Pdm | CAN-Product,SGM458PDM | EnterExtendedSession | 方法 | 方法 | - | 进入扩展会话 | EnterExtendedSession() |
| Sgm458Pdm | CAN-Product,SGM458PDM | EnterProgramSession | 方法 | 方法 | - | 进入编程会话 | EnterProgramSession() |

## UDS会话说明
- **EnterDefaultSession**: 进入默认会话模式
- **EnterExtendedSession**: 进入扩展会话模式,用于执行诊断服务
- **EnterProgramSession**: 进入编程会话模式,用于固件更新

## MOS控制说明
- **MosBreak**: MOS管断开,停止输出
- **MosFulldutyFactorOpen**: MOS管全占空比开启,满输出
- **ExitCurrentCalibration**: 退出电流校准模式

- **ReadCurrent**: 读取当前电流值
- **ReadStatus**: 读取设备状态

## 电源管理说明
- **Awake**: 唤醒设备
- **Sleep**: 设备进入休眠模式
- **UnlockMotor**: 解锁电机
- **ClearFault**: 清除设备故障

## 通信方式
- **CAN总线**: 使用CAN总线进行通信
- **UDS协议**: 支持UDS诊断服务
