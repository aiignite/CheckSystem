# MotorCheckCustomPlc 控制器接口文档

## 控制器信息

| 控制器名称 | 控制器描述 |
|-----------|-----------|
| MotorCheckCustomPlc |  |

## 公共字段

| 名称 | 类型 | 类别 | 描述 |
|------|------|------|------|
| O00001-O00150 | bool | 读/写 | 输出端口 |
| I10001-I10150 | bool | 读 | 输入端口 |
| Gt1Read | float | 读 | Gt1读取值 |
| Gt2Read | float | 读 | Gt2读取值 |
| Gt3Read | float | 读 | Gt3读取值 |
| Gt4Read | float | 读 | Gt4读取值 |

## 公共方法

| 名称 | 参数 | 类型 | 说明 | 示例 |
|------|------|------|------|------|
| InitRemoteAddress | ipPort: string | 方法 | 初始化远程地址 | InitRemoteAddress("192.168.1.1:502") |
| ReadIo | 无 | 方法 | 读取IO |  |
| StartReadWriteTh | 无 | 方法 | 启动读写线程 |  |
| ReadGt | 无 | 方法 | 读取Gt值 |  |
