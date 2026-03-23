# MatrixTps92662With3Q1 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: MatrixTps92662With3Q1

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| MatrixTps92662With3Q1 | R,ICID | Icid | string | 字段 | - | ICID | - |
| MatrixTps92662With3Q1 | R/W,是否打印log | IsPrintTrace | bool | 字段 | - | 是否打印log | - |
| MatrixTps92662With3Q1 | R/W,是否打开PhaseShift | IsPSON | bool | 字段 | - | 是否打开PhaseShift | - |
| MatrixTps92662With3Q1 | R,DSTR | Dstr | string | 字段 | - | DSTR | - |
| MatrixTps92662With3Q1 | R,读ADC1_8Bits | Adc18Bits | double | 字段 | - | 读ADC1_8Bits | - |
| MatrixTps92662With3Q1 | R,读ADC2_8Bits | Adc28Bits | double | 字段 | - | 读ADC2_8Bits | - |
| MatrixTps92662With3Q1 | R,读ADC1_8Bits转电阻值 | Adc18BitsToResValue | double | 字段 | - | 读ADC1_8Bits转电阻值 | - |
| MatrixTps92662With3Q1 | R,读ADC2_8Bits转电阻值 | Adc28BitsToResValue | double | 字段 | - | 读ADC2_8Bits转电阻值 | - |
| MatrixTps92662With3Q1 | R,REG_0xB0 | REG_0xB0 | string | 字段 | - | REG_0xB0 | - |
| MatrixTps92662With3Q1 | R,REG_0xB1 | REG_0xB1 | string | 字段 | - | REG_0xB1 | - |
| MatrixTps92662With3Q1 | R,REG_0xB2 | REG_0xB2 | string | 字段 | - | REG_0xB2 | - |
| MatrixTps92662With3Q1 | 广播发送写ADCID命令 | BroadcastWriteAdcid | void | 方法 | int value | 广播发送写ADCID命令 | BroadcastWriteAdcid(1) |
| MatrixTps92662With3Q1 | 发送初始化命令 | SysConfig | void | 方法 | - | 发送初始化命令 | SysConfig() |
| MatrixTps92662With3Q1 | 停止周期帧 | SysNotActive | void | 方法 | - | 停止周期帧 | SysNotActive() |
| MatrixTps92662With3Q1 | 打开所有LED | Allon | void | 方法 | - | 打开所有LED | Allon() |
| MatrixTps92662With3Q1 | 关闭所有LED | Alloff | void | 方法 | - | 关闭所有LED | Alloff() |
| MatrixTps92662With3Q1 | 打开LED | LedOn | void | 方法 | string index | 打开LED | LedOn("1") |
| MatrixTps92662With3Q1 | 关闭LED | LedOff | void | 方法 | string index | 关闭LED | LedOff("1") |
| MatrixTps92662With3Q1 | 设置广播模式占空比 | SetBroadcastWidth | void | 方法 | ushort widthVal | 设置广播模式占空比 | SetBroadcastWidth(1023) |
| MatrixTps92662With3Q1 | Read ICID | ReadIcIdentificationRegister | void | 方法 | string devAddr | 读取ICID | ReadIcIdentificationRegister("000") |
| MatrixTps92662With3Q1 | Read DSTR | ReadDstrRegister | void | 方法 | string devAddr | 读取DSTR | ReadDstrRegister("000") |
| MatrixTps92662With3Q1 | 读ADC_8Bits | ReadAdc_8bits | void | 方法 | string devAddr | 读ADC_8Bits | ReadAdc_8bits("000") |
| MatrixTps92662With3Q1 | 读取所有寄存器并保存 | ReadAllRegValue | void | 方法 | string devAddr | 读取所有寄存器并保存 | ReadAllRegValue("000") |
| MatrixTps92662With3Q1 | ReadReg0XB0_0XB1_0XB2 | ReadReg0XB0_0XB1_0XB2 | void | 方法 | int addr | 读取寄存器0xB0/0xB1/0xB2 | ReadReg0XB0_0XB1_0XB2(0) |
