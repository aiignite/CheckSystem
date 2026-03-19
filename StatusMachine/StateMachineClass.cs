using System;

namespace StateMachine
{
    public class StateMachineClass
    {
        public sealed class Workstation
        {
            public string Name { get; set; }
            public string InitStateUnitName { get; set; }
            public string CurrentStateUnitName { get; set; }

            //public readonly Dictionary<string, StateUnit> StateUnitList = new Dictionary<string, StateUnit>();
            //public readonly Dictionary<string, Condition> ConditionList = new Dictionary<string, Condition>();

            //public StateMachine<StateUnit, Condition> StateMachine { get; set; }
            //public StateUnit InternStateUintBeforeIdle { get; set; }
            //public Condition InternCondintionBeforeIdle { get; set; }

            /// <summary>
            /// 工作站
            /// </summary>
            /// <param name="name">工作站名称</param>
            /// <param name="initStateUnitName">初始状态名称</param>
            public Workstation(string name, string initStateUnitName)
            {
                Name = name;
                InitStateUnitName = CurrentStateUnitName = initStateUnitName;

                //InternStateUintBeforeIdle =
                //    new StateUnit(name, Guid.NewGuid().ToString());
                //InternCondintionBeforeIdle =
                //    new Condition(
                //        name, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            }

            ///// <summary>
            ///// 添加一个状态单元
            ///// </summary>
            ///// <param name="stateUnit"></param>
            //public void AddStateUnit(StateUnit stateUnit)
            //{
            //    if (!StateUnitList.ContainsKey(stateUnit.Name))
            //        StateUnitList.Add(stateUnit.Name, stateUnit);
            //}

            ///// <summary>
            ///// 添加一个条件
            ///// </summary>
            ///// <param name="condition"></param>
            //public void AddCondition(Condition condition)
            //{
            //    if (!ConditionList.ContainsKey(condition.Name))
            //        ConditionList.Add(condition.Name, condition);
            //}

            ///// <summary>
            ///// 配置状态机
            ///// </summary>
            //public void MachineConfig()
            //{
            //    if (StateUnitList.ContainsKey(InitStateUnitName))
            //    {
            //        StateMachine = new StateMachine<StateUnit, Condition>(
            //            () => InternStateUintBeforeIdle,
            //            s => InternStateUintBeforeIdle = s);

            //        StateMachine.Configure(InternStateUintBeforeIdle)
            //            .Permit(InternCondintionBeforeIdle, StateUnitList[InitStateUnitName]);
            //    }

            //    foreach (var key in StateUnitList.Keys)
            //    {
            //        var stateUint = StateUnitList[key];

            //        var st = StateMachine.Configure(stateUint);
            //        st.OnEntry(() => RunStateUnitEnterAcion(stateUint));
            //    }

            //    foreach (var key in ConditionList.Keys)
            //    {
            //        var condition = ConditionList[key];
            //        var st = StateMachine.Configure(StateUnitList[condition.SourceStateName]);
            //        st.Permit(condition, StateUnitList[condition.TargetStateName]);
            //    }
            //}

            ///// <summary>
            ///// 开始状态机
            ///// </summary>
            //public void MachineStart()
            //{
            //    StateMachine.FireAsync(InternCondintionBeforeIdle);
            //}

            ///// <summary>
            ///// 状态单元执行进入函数
            ///// </summary>
            ///// <param name="su"></param>
            //private void RunStateUnitEnterAcion(StateUnit su)
            //{
            //    su.EnterTime = DateTime.Now;
            //    su.EntryAction.BeginInvoke(StateUnitEnterAcionEnd, su);
            //}

            ///// <summary>
            ///// 进入函数执行结束
            ///// </summary>
            ///// <param name="ir"></param>
            //private void StateUnitEnterAcionEnd(IAsyncResult ir)
            //{
            //    if (ir == null)
            //        return;

            //    var temp = ir.AsyncState as StateUnit;
            //    if (temp == null)
            //        return;
            //    temp.DurningAcion.BeginInvoke(StateUnitDuringAcionEnd, temp);
            //}

            ///// <summary>
            ///// 执行函数执行结束
            ///// </summary>
            ///// <param name="ir"></param>
            //private void StateUnitDuringAcionEnd(IAsyncResult ir)
            //{
            //    if (ir == null)
            //        return;

            //    var temp = ir.AsyncState as StateUnit;
            //    if (temp == null)
            //        return;

            //    if (temp.NExcTime > 0)
            //    {
            //        temp.NExcTime--;
            //        RunStateUnitEnterAcion(temp);
            //    }
            //    else
            //    {
            //        var trueConditionName = string.Empty;
            //        var trueConditionPriority = int.MaxValue;

            //        foreach (var key in ConditionList.Keys)
            //        {
            //            var c = ConditionList[key];

            //            if (c.SourceStateName == temp.Name)
            //            {
            //                c.CompareResultValue = string.Empty;
            //                var compareStruct = c.ConditionFunc();

            //                if (c.IsHaveCheckPara)
            //                {

            //                }
            //                else
            //                {
            //                    c.CompareResultValue = compareStruct.CompareNote;
            //                    if (compareStruct.CompareResult && c.Priority <= trueConditionPriority)
            //                    {
            //                        trueConditionName = c.Name;
            //                        trueConditionPriority = c.Priority;
            //                    }
            //                }
            //            }
            //        }

            //        foreach (
            //            var c in
            //                ConditionList.Keys.Select(key => ConditionList[key])
            //                    .Where(c => c.SourceStateName == temp.Name && c.ConditionFunc.Invoke().CompareResult))
            //        {
            //            temp.NExcTime = temp.ExcTimes;
            //            c.ExitAction.BeginInvoke(ConditionExitActionEnd, c);
            //            return;
            //        }

            //        temp.DurningAcion.BeginInvoke(StateUnitDuringAcionEnd, temp);
            //    }
            //}

            ///// <summary>
            ///// 条件退出函数执行结束
            ///// </summary>
            ///// <param name="ir"></param>
            //private void ConditionExitActionEnd(IAsyncResult ir)
            //{
            //    if (ir == null)
            //        return;

            //    var temp = ir.AsyncState as Condition;
            //    if (temp == null)
            //        return;

            //    StateMachine.FireAsync(temp);
            //}
        }

        public sealed class StateUnit
        {
            public string WorkStationName { get; set; }
            public string Name { get; set; }
            public StateMachineRunner.StateMachineAction EntryAction { get; set; }
            public StateMachineRunner.StateMachineAction DurningAcion { get; set; }
            public DateTime EnterTime { get; set; }
            public int OutTime10Ms { get; set; }
            public int DelayTime10Ms { get; set; }
            public int ExcTimes { get; set; }
            public int NExcTime { get; set; }

            public StateUnit(string workstationName, string name)
            {
                WorkStationName = workstationName;
                Name = name;
                //ExcTimes = 2;
            }
        }

        public sealed class Condition
        {
            public string WorkStationName { get; set; }
            public string Name { get; set; }
            public int Priority { get; set; }
            public string SourceStateName { get; set; }
            public string TargetStateName { get; set; }
            public bool IsHaveCheckPara { get; set; }
            public StateMachineRunner.StateMachineAction ExitAction { get; set; }
            public Func<ConditionFuncStruct> ConditionFunc { get; set; }
            public string CompareResultValue { get; set; }

            public Condition(string workstationName, string name, string srcState, string desState)
            {
                WorkStationName = workstationName;
                Name = name;
                SourceStateName = srcState;
                TargetStateName = desState;
            }

            public struct ConditionFuncStruct
            {
                public bool CompareResult;
                public string CompareNote;
            }
        }
    }
}
