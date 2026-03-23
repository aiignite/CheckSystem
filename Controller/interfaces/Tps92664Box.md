# Tps92664Box 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: Tps92664Box

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| Tps92664Box | Tps92664Box | MySerialPort | MySerialPort | 字段 | - | 串口对象 | - |
| Tps92664Box | Tps92664Box | Adc18Bits | double | 字段 | - | R,读ADC1_8Bits | - |
| Tps92664Box | Tps92664Box | Adc28Bits | double | 字段 | - | R,读ADC2_8Bits | - |
| Tps92664Box | Tps92664Box | Adc110Bits | double | 字段 | - | R,读ADC1_10Bits | - |
| Tps92664Box | Tps92664Box | Adc210Bits | double | 字段 | - | R,读ADC2_10Bits | - |
| Tps92664Box | Tps92664Box | Adc18BitsToResValue | double | 字段 | - | R,读ADC1_8Bits转电阻值 | - |
| Tps92664Box | Tps92664Box | Adc28BitsToResValue | double | 字段 | - | R,读ADC2_8Bits转电阻值 | - |
| Tps92664Box | Tps92664Box | Adc110BitsToResValue | double | 字段 | - | R,读ADC1_10Bits转电阻值 | - |
| Tps92664Box | Tps92664Box | Adc210BitsToResValue | double | 字段 | - | R,读ADC2_10Bits转电阻值 | - |
| Tps92664Box | Tps92664Box | Icid | string | 字段 | - | R,ICID | - |
| Tps92664Box | Tps92664Box | SendCmdWidth | void | 方法 | - | 发送宽度命令 | - |
| Tps92664Box | Tps92664Box | SendCmdPhase | void | 方法 | - | 发送相位命令 | - |
| Tps92664Box | Tps92664Box | SendCmdReadWidth | void | 方法 | - | 发送读取宽度命令 | - |
| Tps92664Box | Tps92664Box | AddDev | void | 方法 | string addr | 添加一个地址的芯片 | - |
| Tps92664Box | Tps92664Box | Remove | void | 方法 | string addr | 移除一个地址的芯片 | - |
| Tps92664Box | Tps92664Box | SysConfig | void | 方法 | - | 发送初始化命令 | - |
| Tps92664Box | Tps92664Box | SysNotActive | void | 方法 | - | 停止报文 | - |
| Tps92664Box | Tps92664Box | Allon | void | 方法 | string addr | 打开目标地址的所有LED | - |
| Tps92664Box | Tps92664Box | Alloff | void | 方法 | string addr | 关闭目标地址的所有LED | - |
| Tps92664Box | Tps92664Box | TargetDevSingleChOn | void | 方法 | string addr, string ledIndex | 打开目标芯片的第X个通道 | - |
| Tps92664Box | Tps92664Box | TargetDevSingleChOff | void | 方法 | string addr, string ledIndex | 关闭目标芯片的第X个通道 | - |
| Tps92664Box | Tps92664Box | TargetDevOddChOn | void | 方法 | string addr | 打开目标芯片的单数通道 | - |
| Tps92664Box | Tps92664Box | TargetDevOddChOff | void | 方法 | string addr | 关闭目标芯片的单数通道 | - |
| Tps92664Box | Tps92664Box | TargetDevEvenChOn | void | 方法 | string addr | 打开目标芯片的双数通道 | - |
| Tps92664Box | Tps92664Box | TargetDevEvenChOff | void | 方法 | string addr | 关闭目标芯片的双数通道 | - |
| Tps92664Box | Tps92664Box | TargetDevAllChOn | void | 方法 | string addr | 打开目标芯片的所有通道 | - |
| Tps92664Box | Tps92664Box | TargetDevAllChOff | void | 方法 | string addr | 关闭目标芯片的所有通道 | - |
| Tps92664Box | Tps92664Box | TargetDevSingleChWidth | void | 方法 | string addr, string ledIndex, int width | 设置目标芯片的目标通道的width | - |
| Tps92664Box | Tps92664Box | TargetDevSingleChPhase | void | 方法 | string addr, string ledIndex, int phase | 设置目标芯片的目标通道的phase | - |
| Tps92664Box | Tps92664Box | ReadAdc_8bits | void | 方法 | string addr | 读ADC_8Bits | - |
| Tps92664Box | Tps92664Box | ReadAdc_10Bits | void | 方法 | string addr | 读ADC_10Bits | - |
| Tps92664Box | Tps92664Box | ReadIcid | void | 方法 | string addr | 读ICID | - |
