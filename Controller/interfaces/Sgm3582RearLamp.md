# Sgm3582RearLamp 控制器接口文档

## 控制器信息

| 控制器名称 | 控制器描述 |
|-----------|-----------|
| Sgm3582RearLamp | LIN-Product,SGM358-2后灯 |

## 公共字段

| 名称 | 类型 | 类别 | 描述 |
|------|------|------|------|
| IsAllControl | bool | 读/写 | R/W,L&R左右同时控制 |
| HASCO总软件版本号 | string | 读 | R,HASCO总软件版本号 |
| HASCO应用程序软件版本号 | string | 读 | R,HASCO应用程序软件版本号 |
| HASCO应用程序软件小版本号 | string | 读 | R,HASCO应用程序软件小版本号 |
| HASCO引导程序软件版本号 | string | 读 | R,HASCO引导程序软件版本号 |
| HASCO引导程序软件小版本号 | string | 读 | R,HASCO引导程序软件小版本号 |
| HASCO标定版本号 | string | 读 | R,HASCO标定版本号 |
| HASCO标定小版本号 | string | 读 | R,HASCO标定小版本号 |
| HASCO应用程序软件零件号 | string | 读 | R,HASCO应用程序软件零件号 |
| HASCO引导程序软件零件号 | string | 读 | R,HASCO引导程序软件零件号 |
| HASCO标定零件号 | string | 读 | R,HASCO标定零件号 |
| 信耀引导程序软件内部版本号 | string | 读 | R,信耀引导程序软件内部版本号 |
| 信耀应用程序内部版本号 | string | 读 | R,信耀应用程序内部版本号 |
| 泛亚标定零件号 | string | 读 | R,泛亚标定零件号 |
| 左侧A灯转向灯 | string | 读 | R,左侧A灯转向灯状态 |
| 左侧A灯制动灯 | string | 读 | R,左侧A灯制动灯状态 |
| 左侧A灯位置灯 | string | 读 | R,左侧A灯位置灯状态 |
| 右侧A灯转向灯 | string | 读 | R,右侧A灯转向灯状态 |
| 右侧A灯制动灯 | string | 读 | R,右侧A灯制动灯状态 |
| 右侧A灯位置灯 | string | 读 | R,右侧A灯位置灯状态 |
| 左侧B灯转向灯 | string | 读 | R,左侧B灯转向灯状态 |
| 左侧B灯位置灯 | string | 读 | R,左侧B灯位置灯状态 |
| 右侧B灯转向灯 | string | 读 | R,右侧B灯转向灯状态 |
| 右侧B灯位置灯 | string | 读 | R,右侧B灯位置灯状态 |

## 公共方法

| 名称 | 参数 | 类型 | 说明 | 示例 |
|------|------|------|------|------|
| SetLeft | 无 | 方法 | 设置为左灯 | |
| SetRight | 无 | 方法 | 设置为右灯 | |
| LampAwake | 无 | 方法 | LIN唤醒 | |
| LampSleep | 无 | 方法 | LIN休眠 | |
| TailNormalOn | 无 | 方法 | 位置灯高亮 | |
| TailHighLightOn | 无 | 方法 | 位置灯高亮,复用位置灯低亮 | |
| TailNoAction | 无 | 方法 | 位置灯灭 | |
| TurnSwipeTurn | 无 | 方法 | 转向灯A灯低亮B灯流水 | |
| TurnHighLightOn | 无 | 方法 | 转向灯A灯高亮B灯流水 | |
| TurnHoldOnLight | 无 | 方法 | 转向灯A灯低亮B灯高亮 | |
| TurnNotActive | 无 | 方法 | 转向灯灭 | |
| StartSwitchTurn | 无 | 方法 | 开启转向灯400ms亮灭 | |
| StopSwitchTurn | 无 | 方法 | 关闭转向灯400ms亮灭 | |
| SimpleLeavingHome | 无 | 方法 | 简单解锁 | |
| SimpleComingHome | 无 | 方法 | 简单闭锁 | |
| ComplexLeavingHome | 无 | 方法 | 复杂解锁 | |
| ComplexComingHome | 无 | 方法 | 复杂闭锁 | |
| LampShowAbort | 无 | 方法 | 打断动画 | |
| ReadVer | 无 | 方法 | ReadVer | |
