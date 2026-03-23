# Cd764RearLamp

## Controller Description

LIN-Product,CD764组合后灯-lin波特率10417

## Public Fields

| 名称 | 描述 | 类型 |
|------|------|------|
| Lin | LinBus | LIN总线 |
| HascoAppVersion | R,读取HASCO软件版本号 | string |
| HascoAppPartNo | R,读取HASCO软件零件号 | string |

## Public Methods

| 方法名称 | 描述 | 参数 |
|----------|------|------|
| LampAwake | 唤醒 |  |
| LampSleep | 休眠 |  |
| Reset | Reset |  |
| LogoOn | LogoOn |  |
| LogoOff | LogoOff |  |
| Welcome | 迎宾动画 | modeIndex |
| Farewell | 伴我回家动画 | modeIndex |
| SetLeftRcla | 设置为Left RCLA |  |
| SetRightRcla | 设置为Right RCLA |  |
| SetLeftRclb | 设置为Left RCLB |  |
| SetRightRclb | 设置为Right RCLB |  |
| ReadHascoSoftwareVersion | 读取HASCO从节点软件版本号 |  |
| ReadHascoAppPartNo | 读取HASCO应用程序软件零件号 |  |
