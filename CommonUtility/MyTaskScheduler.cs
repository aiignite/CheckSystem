using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CommonUtility
{
    public class MyTaskScheduler
    {
        private readonly ConcurrentDictionary<int, TaskInfo> _tasks = new ConcurrentDictionary<int, TaskInfo>();
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private int _taskIdCounter;

        /// <summary>
        /// 任务线程
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task SchedulerAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var elapsed = HighPrecisionTimer.GetTimestamp();
                    var sendTasks = new List<Task>();

                    foreach (var taskPair in _tasks)
                    {
                        var task = taskPair.Value;

                        //if (elapsed - task.LastExecution < task.Interval)
                        //    continue;

                        var interval = HighPrecisionTimer.GetTimestampIntervalMs(task.LastExecution, elapsed);
                        if (interval < task.Interval)
                            continue;

                        task.LastExecution = elapsed;
                        sendTasks.Add(RunTaskAction(task.Action));
                    }

                    if (sendTasks.Count > 0)
                        await Task.WhenAll(sendTasks);

                    await Task.Delay(1, token);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private static async Task RunTaskAction(Action action)
        {
            await Task.Run(() =>
            {
                if (action == null)
                    return;

                try
                {
                    action.Invoke();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
        }

        /// <summary>
        /// 初始化任务
        /// </summary>
        /// <param name="taskInfo"></param>
        public void SetTimer(TaskInfo taskInfo)
        {
            var taskId = Interlocked.Increment(ref _taskIdCounter);
            taskInfo.TaskId = taskId;
            taskInfo.LastExecution = HighPrecisionTimer.GetTimestamp();
            _tasks[taskId] = taskInfo;
        }

        /// <summary>
        /// 启动任务
        /// </summary>
        public async void SchedulerAsync()
        {
            await SchedulerAsync(_cts.Token);
        }

        public void SchedulerCancel()
        {
            // 取消任务并清理资源
            _cts.Cancel();
        }

        public class TaskInfo
        {
            /// <summary>
            /// 任务ID
            /// </summary>
            public int TaskId { get; set; }

            /// <summary>
            /// Action
            /// </summary>
            public Action Action { get; set; }

            /// <summary>
            /// 间隔时间（毫秒）
            /// </summary>
            public int Interval { get; set; }

            /// <summary>
            /// 上次执行时间
            /// </summary>
            public long LastExecution { get; set; }
        }
    }
}
