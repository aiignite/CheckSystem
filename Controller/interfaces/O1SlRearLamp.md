# O1SlRearLamp 控制器接口文档

## 控制器信息

| 控制器名称 | 控制器描述 |
|-----------|-----------|
| O1SlRearLamp | LIN-Product,O1SL组合后灯 |

## 公共字段

| 名称 | 类型 | 类别 | 描述 |
|------|------|------|------|
| Lin | LinBus | 总线 | LIN总线 |
| SlaveSparePartNumber | string | 读 | R,SlaveSparePartNumber |
| SlaveAppSwVersionNumber | string | 读 | R,SlaveAppSwVersionNumber |

## 公共方法

| 名称 | 参数 | 类型 | 说明 | 示例 |
|------|------|------|------|------|
| SetLeftRcla | 无 | 方法 | 设置为Left RCLA |  |
| SetRightRcla | 无 | 方法 | 设置为Right RCLA |  |
| SetRclb | 无 | 方法 | 设置为RCLB |  |
| ReadSlaveSparePartNumber | 无 | 方法 | 读SlaveSparePartNumber |  |
| ReadSlaveAppSwVersionNumber | 无 | 方法 | 读SlaveAppSwVersionNumber |  |
