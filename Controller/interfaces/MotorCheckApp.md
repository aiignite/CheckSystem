# MotorCheckApp 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: MotorCheckApp

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| MotorCheckApp | 下降前位置 | PreDownPosition | float | 字段 | - | 下降前位置 | - |
| MotorCheckApp | 下降位置 | DownPosition | float | 字段 | - | 下降位置 | - |
| MotorCheckApp | 上升前位置 | PreUpPosition | float | 字段 | - | 上升前位置 | - |
| MotorCheckApp | 上升位置 | UpPosition | float | 字段 | - | 上升位置 | - |
| MotorCheckApp | 时间间隔 | Interval | float | 字段 | - | 时间间隔 | - |
| MotorCheckApp | 下降开始时间 | MoveDownStartTime | DateTime | 字段 | - | 下降开始时间 | - |
| MotorCheckApp | 下降结束时间 | MoveDownEndTime | DateTime | 字段 | - | 下降结束时间 | - |
| MotorCheckApp | 上升开始时间 | MoveUpStartTime | DateTime | 字段 | - | 上升开始时间 | - |
| MotorCheckApp | 上升结束时间 | MoveUpEndTime | DateTime | 字段 | - | 上升结束时间 | - |
| MotorCheckApp | 上升速度 | MoveUpSpeed | float | 字段 | - | 上升速度 | - |
| MotorCheckApp | 下降速度 | MoveDownSpeed | float | 字段 | - | 下降速度 | - |
| MotorCheckApp | 移动电流 | MoveCurrent | float | 字段 | - | 移动电流 | - |
| MotorCheckApp | 起始位置 | StartPos | float | 字段 | - | 起始位置 | - |
| MotorCheckApp | 结束位置 | EndPos | float | 字段 | - | 结束位置 | - |
| MotorCheckApp | 临时浮点1 | TempFloat1 | float | 字段 | - | 临时浮点1 | - |
| MotorCheckApp | 临时浮点2 | TempFloat2 | float | 字段 | - | 临时浮点2 | - |
| MotorCheckApp | 临时浮点3 | TempFloat3 | float | 字段 | - | 临时浮点3 | - |
| MotorCheckApp | 设置下降开始时间 | SetMoveDownStartTime | void | 方法 | - | 设置下降开始时间 | SetMoveDownStartTime() |
| MotorCheckApp | 设置下降结束时间 | SetMoveDownEndTime | void | 方法 | - | 设置下降结束时间 | SetMoveDownEndTime() |
| MotorCheckApp | 设置上升开始时间 | SetMoveUpStartTime | void | 方法 | - | 设置上升开始时间 | SetMoveUpStartTime() |
| MotorCheckApp | 设置上升结束时间 | SetMoveUpEndTime | void | 方法 | - | 设置上升结束时间 | SetMoveUpEndTime() |
| MotorCheckApp | 计算时间间隔 | ComputeInterval | void | 方法 | - | 计算时间间隔 | ComputeInterval() |
| MotorCheckApp | 计算下降速度 | ComputeMovwDownSpeed | void | 方法 | - | 计算下降速度 | ComputeMovwDownSpeed() |
| MotorCheckApp | 计算上升速度 | ComputeMovwUpSpeed | void | 方法 | - | 计算上升速度 | ComputeMovwUpSpeed() |
| MotorCheckApp | 临时设置OK | TempSetOk | void | 方法 | - | 临时设置OK | TempSetOk() |
