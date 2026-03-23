# MyTaskScheduler 控制器接口文档

**命名空间**: `CommonUtility`
**父类**: `object`
**控制器描述**: 任务调度器

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| MyTaskScheduler | 任务调度器 | TaskInfo | 嵌套类 | 类 | - | 任务信息类 | - |
| MyTaskScheduler | 任务调度器 | TaskId | int | 字段 | - | 任务ID | obj.TaskId; |
| MyTaskScheduler | 任务调度器 | Action | Action | 字段 | - | Action | obj.Action; |
| MyTaskScheduler | 任务调度器 | Interval | int | 字段 | - | 间隔时间（毫秒） | obj.Interval; |
| MyTaskScheduler | 任务调度器 | LastExecution | long | 字段 | - | 上次执行时间 | obj.LastExecution; |
| MyTaskScheduler | 任务调度器 | SetTimer | void | 方法 | TaskInfo taskInfo | 初始化任务 | obj.SetTimer(taskInfo); |
| MyTaskScheduler | 任务调度器 | SchedulerAsync | void | 方法 | - | 启动任务 | obj.SchedulerAsync(); |
| MyTaskScheduler | 任务调度器 | SchedulerCancel | void | 方法 | - | 取消任务并清理资源 | obj.SchedulerCancel(); |
