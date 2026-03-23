# MatrixTps92663With3Q1 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: MatrixTps92663With3Q1

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| MatrixTps92663With3Q1 | R,ICID | Icid | string | 字段 | - | ICID | - |
| MatrixTps92663With3Q1 | R,DSTR | Dstr | string | 字段 | - | DSTR | - |
| MatrixTps92663With3Q1 | R,读ADC1_8Bits | Adc18Bits | double | 字段 | - | 读ADC1_8Bits | - |
| MatrixTps92663With3Q1 | R,读ADC2_8Bits | Adc28Bits | double | 字段 | - | 读ADC2_8Bits | - |
| MatrixTps92663With3Q1 | R,读ADC1_8Bits转电阻值 | Adc18BitsToResValue | double | 字段 | - | 读ADC1_8Bits转电阻值 | - |
| MatrixTps92663With3Q1 | R,读ADC2_8Bits转电阻值 | Adc28BitsToResValue | double | 字段 | - | 读ADC2_8Bits转电阻值 | - |
| MatrixTps92663With3Q1 | 广播发送写ADCID命令 | BroadcastWriteAdcid | void | 方法 | int value | 广播发送写ADCID命令 | BroadcastWriteAdcid(1) |
| MatrixTps92663With3Q1 | 发送初始化命令 | SysConfig | void | 方法 | - | 发送初始化命令 | SysConfig() |
| MatrixTps92663With3Q1 | 停止周期帧 | SysNotActive | void | 方法 | - | 停止周期帧 | SysNotActive() |
| MatrixTps92663With3Q1 | 打开所有LED | Allon | void | 方法 | - | 打开所有LED | Allon() |
| MatrixTps92663With3Q1 | 关闭所有LED | Alloff | void | 方法 | - | 关闭所有LED | Alloff() |
| MatrixTps92663With3Q1 | 打开LED | LedOn | void | 方法 | string index | 打开LED | LedOn("1") |
| MatrixTps92663With3Q1 | 关闭LED | LedOff | void | 方法 | string index | 关闭LED | LedOff("1") |
| MatrixTps92663With3Q1 | Read ICID | ReadIcIdentificationRegister | void | 方法 | string devAddr | 读取ICID | ReadIcIdentificationRegister("000") |
| MatrixTps92663With3Q1 | Read DSTR | ReadDstrRegister | void | 方法 | string devAddr | 读取DSTR | ReadDstrRegister("000") |
| MatrixTps92663With3Q1 | 读ADC_8Bits | ReadAdc_8bits | void | 方法 | string devAddr | 读ADC_8Bits | ReadAdc_8bits("000") |
