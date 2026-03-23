# OptimizedWirelessChargingModule 控制器接口文档

## 控制器信息

| 控制器名称 | 控制器描述 |
|-----------|-----------|
| OptimizedWirelessChargingModule | 优化后的无线充电模块控制器 |

## 公共字段

| 名称 | 类型 | 类别 | 描述 |
|------|------|------|------|

## 公共方法

| 名称 | 参数 | 类型 | 说明 | 示例 |
|------|------|------|------|------|
| StartAsync | 无 | 异步方法 | 启动模块 |  |
| StopAsync | 无 | 异步方法 | 停止模块 |  |
| ReadEcuInfoAsync | infoType: EcuInfoType, cancellationToken: CancellationToken | 异步方法 | 读取ECU信息（优化版本） |  |
| WriteAndVerifyDdiAsync | ddi: byte, data: byte[], cancellationToken: CancellationToken | 异步方法 | 写入并验证DDI（优化版本） |  |
| GetCommunicationStatistics | 无 | 方法 | 获取通讯统计信息 |  |
| Dispose | 无 | 方法 | 释放资源 |  |
