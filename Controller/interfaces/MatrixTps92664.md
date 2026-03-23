# MatrixTps92664 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: MatrixTps92664

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| MatrixTps92664 | R,读ADC1_8Bits | Adc18Bits | double | 字段 | - | 读ADC1_8Bits | - |
| MatrixTps92664 | R,读ADC2_8Bits | Adc28Bits | double | 字段 | - | 读ADC2_8Bits | - |
| MatrixTps92664 | R,读ADC1_10Bits | Adc110Bits | double | 字段 | - | 读ADC1_10Bits | - |
| MatrixTps92664 | R,读ADC2_10Bits | Adc210Bits | double | 字段 | - | 读ADC2_10Bits | - |
| MatrixTps92664 | R,读ADC1_8Bits转电阻值 | Adc18BitsToResValue | double | 字段 | - | 读ADC1_8Bits转电阻值 | - |
| MatrixTps92664 | R,读ADC2_8Bits转电阻值 | Adc28BitsToResValue | double | 字段 | - | 读ADC2_8Bits转电阻值 | - |
| MatrixTps92664 | R,读ADC1_10Bits转电阻值 | Adc110BitsToResValue | double | 字段 | - | 读ADC1_10Bits转电阻值 | - |
| MatrixTps92664 | R,读ADC2_10Bits转电阻值 | Adc210BitsToResValue | double | 字段 | - | 读ADC2_10Bits转电阻值 | - |
| MatrixTps92664 | R,ICID | Icid | string | 字段 | - | ICID | - |
| MatrixTps92664 | 添加一个地址的芯片 | AddDev | void | 方法 | string addr | 添加一个地址的芯片 | AddDev("0") |
| MatrixTps92664 | 移除一个地址的芯片 | Remove | void | 方法 | string addr | 移除一个地址的芯片 | Remove("0") |
| MatrixTps92664 | 发送初始化命令 | SysConfig | void | 方法 | - | 发送初始化命令 | SysConfig() |
| MatrixTps92664 | 停止报文 | SysNotActive | void | 方法 | - | 停止报文 | SysNotActive() |
| MatrixTps92664 | 打开目标地址的所有LED | Allon | void | 方法 | string addr | 打开目标地址的所有LED | Allon("0") |
| MatrixTps92664 | 关闭目标地址的所有LED | Alloff | void | 方法 | string addr | 关闭目标地址的所有LED | Alloff("0") |
| MatrixTps92664 | 打开目标芯片的第X个通道 | TargetDevSingleChOn | void | 方法 | string addr, string ledIndex | 打开目标芯片的第X个通道 | TargetDevSingleChOn("0", "1") |
| MatrixTps92664 | 关闭目标芯片的第X个通道 | TargetDevSingleChOff | void | 方法 | string addr, string ledIndex | 关闭目标芯片的第X个通道 | TargetDevSingleChOff("0", "1") |
| MatrixTps92664 | 打开目标芯片的单数通道 | TargetDevOddChOn | void | 方法 | string addr | 打开目标芯片的单数通道 | TargetDevOddChOn("0") |
| MatrixTps92664 | 关闭目标芯片的单数通道 | TargetDevOddChOff | void | 方法 | string addr | 关闭目标芯片的单数通道 | TargetDevOddChOff("0") |
| MatrixTps92664 | 打开目标芯片的双数通道 | TargetDevEvenChOn | void | 方法 | string addr | 打开目标芯片的双数通道 | TargetDevEvenChOn("0") |
| MatrixTps92664 | 关闭目标芯片的双数通道 | TargetDevEvenChOff | void | 方法 | string addr | 关闭目标芯片的双数通道 | TargetDevEvenChOff("0") |
| MatrixTps92664 | 打开目标芯片的所有通道 | TargetDevAllChOn | void | 方法 | string addr | 打开目标芯片的所有通道 | TargetDevAllChOn("0") |
| MatrixTps92664 | 关闭目标芯片的所有通道 | TargetDevAllChOff | void | 方法 | string addr | 关闭目标芯片的所有通道 | TargetDevAllChOff("0") |
| MatrixTps92664 | 设置目标芯片的目标通道的width | TargetDevSingleChWidth | void | 方法 | string addr, string ledIndex, int width | 设置目标芯片的目标通道的width | TargetDevSingleChWidth("0", "1", 1023) |
| MatrixTps92664 | 设置目标芯片的目标通道的phase | TargetDevSingleChPhase | void | 方法 | string addr, string ledIndex, int phase | 设置目标芯片的目标通道的phase | TargetDevSingleChPhase("0", "1", 50) |
| MatrixTps92664 | 读ADC_8Bits | ReadAdc_8bits | void | 方法 | string addr | 读ADC_8Bits | ReadAdc_8bits("0") |
| MatrixTps92664 | 读ADC_10Bits | ReadAdc_10Bits | void | 方法 | string addr | 读ADC_10Bits | ReadAdc_10Bits("0") |
| MatrixTps92664 | 读ICID | ReadIcid | void | 方法 | string addr | 读ICID | ReadIcid("0") |
