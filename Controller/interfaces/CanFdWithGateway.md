# CanFdWithGateway

## Controller Description

CAN FD网关控制器

## Public Fields

| 名称 | 描述 | 类型 |
|------|------|------|
| MySocketViaUdp | UDP网关 | GatewaySocketViaUdp |
| GatewayCanFd0 | CAN FD通道0 | CanBus |
| GatewayCanFd1 | CAN FD通道1 | CanBus |
| GatewayCanFd2 | CAN FD通道2 | CanBus |
| GatewayCanFd3 | CAN FD通道3 | CanBus |
| GatewayCanFd4 | CAN FD通道4 | CanBus |
| GatewayCanFd5 | CAN FD通道5 | CanBus |
| GatewayCanFd6 | CAN FD通道6 | CanBus |
| GatewayCanFd7 | CAN FD通道7 | CanBus |
| GatewayCanFd8 | CAN FD通道8 | CanBus |
| GatewayCanFd9 | CAN FD通道9 | CanBus |

## Public Methods

| 方法名称 | 描述 | 参数 |
|----------|------|------|
| InitLocalIpAddress | 初始化本地IP地址 | localIpPort |
| InitRemoteIpAddress | 初始化远程IP地址 | remoteIpPort |
| ConnectCanFdWithGateway | 连接CAN FD网关 |  |
| SelectCanChannel | 选择CAN通道 | canChannelId |
