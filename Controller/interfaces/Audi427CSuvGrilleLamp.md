# Audi427CSuvGrilleLamp

## Controller Description

CAN-Product,427C-SUV格栅灯

## Public Fields

| 名称 | 描述 | 类型 |
|------|------|------|
| SwVer | R,软件版本号F18A | string |
| AppVer | R,APP软件版本号F189 | string |
| AppPartNo | R,APP软件零件本号F187 | string |
| BootVer | R,Boot软件版本号F180 | string |
| BootPartNo | R,Boot软件零件本号F181 | string |

## Public Methods

| 方法名称 | 描述 | 参数 |
|----------|------|------|
| StartCan | 打开CAN |  |
| StopCan | 关闭CAN |  |
| FroSigLampOn | 静态点亮 |  |
| FroSigLampOff | 静态关闭 |  |
| FroSigLampPixel | 动态点亮 |  |
| AllPixelOpen | 打开所有像素 | pwm |
| AllPixelClose | 关闭所有灯像素 |  |
| OddPixelOpen | 打开奇数像素 | pwm |
| OddPixelClose | 关闭奇数像素 |  |
| EvenPixelOpen | 打开偶数像素 | pwm |
| EvenPixelClose | 关闭偶数像素 |  |
| OpenRunMode | 打开跑马灯 | pwm |
| CloseRunMode | 关闭跑马灯 |  |
| ReadSwVer | Read软件版本号F18A |  |
| ReadAppVer | ReadAPP软件版本号F189 |  |
| ReadAppPartNo | ReadAPP软件零件本号F187 |  |
| ReadBootVer | ReadBoot软件版本号F180 |  |
| ReadBootPartNo | ReadBoot软件零件本号F181 |  |
