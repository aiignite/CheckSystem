# ChargePortDoorLight

## Controller Description

LIN-Product,充电门指示灯

## Public Fields

| 名称 | 描述 | 类型 |
|------|------|------|
| VerByte2 | R,读取软件版本号Byte2的高位 | string |

## Public Methods

| 方法名称 | 描述 | 参数 |
|----------|------|------|
| StartLin | 打开LIN |  |
| StopLin | 关闭LIN |  |
| ReadVerByte2Hi | 读取软件版本号Byte2的高位 |  |
| SetColorGreen | 绿色G |  |
| SetColorBlue | 蓝色B |  |
| SetColorRed | 红色R |  |
| SetColorNone | 无色NONE |  |
| SetDutyCycle | 设置闪烁周期 | duty |
| SetFlashRate | 设置闪烁频率 | flashRate |
| SetStyleOff | 设置闪烁样式-不亮 |  |
| SetStyleSolid | 设置闪烁样式-常亮 |  |
| SetStyleBlink | 设置闪烁样式-闪烁 |  |
| SetStylePulse | 设置闪烁样式-呼吸 |  |
| AmbientLightOn | 小照明灯ON |  |
| AmbientLightOff | 小照明灯OFF |  |
