# RtexControllerBase 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: RtexControllerBase

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| RtexControllerBase | RtexControllerBase | RtexController | RtexControllerBase | 字段 | - | Rtex控制器实例 | obj.RtexController; |
| RtexControllerBase | RtexControllerBase | DataBytes | byte[] | 字段 | - | 数据字节数组 | obj.DataBytes; |
| RtexControllerBase | RtexControllerBase | InitLocalLocalIpAddress | void | 方法 | string ipPort | 初始化本地IP地址 | obj.InitLocalLocalIpAddress("192.168.1.50:8088"); |
| RtexControllerBase | RtexControllerBase | InitRemoteIpAddress | void | 方法 | string ipPort | 初始化远程IP地址并启动通信 | obj.InitRemoteIpAddress("192.168.1.30:8088"); |
| RtexControllerBase | RtexControllerBase | RunEvent | void | 方法 | int asixIndex, EventType eventType | 运行事件 | obj.RunEvent(0, EventType.ServoEnable); |
| RtexControllerBase | RtexControllerBase | RunJog | void | 方法 | int asixIndex, float pos, float speed | 运行Jog运动 | obj.RunJog(0, 100.0f, 50.0f); |
