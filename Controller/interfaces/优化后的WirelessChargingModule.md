# 优化后的WirelessChargingModule 控制器接口文档

## 控制器基本信息

| 控制器名称 | 控制器描述 |
|------------|------------|
| 优化后的WirelessChargingModule | CAN-Product,50W无线充电模块 |

## 公共属性

| 名称 | 类型 | 类别 | 说明 | 示例 |
|------|------|------|------|------|
| CanFD | CanBus | 属性 | CAN FD总线 | - |

## 公共方法

| 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|------|------|------|------|------|------|
| CanFdUds22Async | bool | 方法 | didHi: byte, didLo: byte, cancellationToken: CancellationToken = default | 优化的UDS 22服务读取方法 - 使用现代异步编程模式 | CanFdUds22Async(0xF1, 0x90) |
| CanFdUds22 | bool | 方法 | didHi: byte, didLo: byte, out byte[] echo | 向后兼容的同步版本（已废弃，建议使用异步版本） | CanFdUds22(0xF1, 0x90, out echo) |
| Dispose | void | 方法 | 无 | 释放资源 | Dispose() |
