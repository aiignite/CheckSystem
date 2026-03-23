# WuLingController 控制器接口文档

## 控制器基本信息

| 控制器名称 | 控制器描述 |
|------------|------------|
| WuLingController | CAN-Product,五菱控制器 |

## 公共属性

| 名称 | 类型 | 类别 | 说明 | 示例 |
|------|------|------|------|------|
| TesterCan | CanBus | 属性 | CAN总线 | - |
| IsCanOffline | bool | 属性 | R,CAN接收异常 | - |
| FrEvprTemSens_Value | int | 属性 | R,前蒸温度传感器 | - |

## 公共方法

| 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|------|------|------|------|------|------|
| StartCan | void | 方法 | 无 | 打开CAN消息发送 | StartCan() |
| StopCan | void | 方法 | 无 | 关闭CAN消息发送 | StopCan() |
| WriteEXV1Step | void | 方法 | value: string | EXV1步数请求 | WriteEXV1Step("500") |
| WriteEXV2Step | void | 方法 | value: string | EXV2步数请求 | WriteEXV2Step("500") |