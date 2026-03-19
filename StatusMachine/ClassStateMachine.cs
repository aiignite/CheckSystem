using System;
using System.Collections.Generic;

namespace StateMachine
{
    public class ClassCondition
    {
        public int SourceStatusIndex;
        public int TargetStatusIndex;
        public string StrWorkStationName;
        public string StrConditionName;
        public string StrSourceStatus;
        public string StrTargerStatus;
        public string StrCondition;
        public string StrActExit;
        public string StrPosition;
        public Func<object> FuncCondition;
        public Action ActionExit;
        public string PreviousResult;

        public ClassCondition()
        {

        }

        public ClassCondition(string sourceStatus, string targetStatus, Func<object> funcCondition, Action actExit)
        {
            StrConditionName = sourceStatus + "To" + targetStatus;
            StrSourceStatus = sourceStatus;
            StrTargerStatus = targetStatus;
            FuncCondition = funcCondition;
            ActionExit = actExit;
        }

        public ClassCondition(string sourceStatus, string targetStatus, Func<object> funcCondition, Action actExit, string conditionfunction)
        {
            StrConditionName = sourceStatus + "To" + targetStatus;
            StrSourceStatus = sourceStatus;
            StrTargerStatus = targetStatus;
            StrCondition = conditionfunction;
            FuncCondition = funcCondition;
            ActionExit = actExit;
        }

        public ClassCondition(string workStationName, string sourceStatus, string targetStatus, Func<object[]> funcCondition,
            Action actExit)
        {
            StrWorkStationName = workStationName;
            StrConditionName = sourceStatus + "To" + targetStatus;
            StrSourceStatus = sourceStatus;
            StrTargerStatus = targetStatus;
            FuncCondition = funcCondition;
            ActionExit = actExit;
        }
    }

    public class ClassStatusUnit
    {
        public int StatusIndex;
        public string StrWorkStationName;
        public string StatusEnName;
        public string StatusCnName;
        public string FunctionEnter;
        public string FunctionDuring;
        public bool EnterOnce = false;
        public Action ActionEnter;
        public Action ActionDuring;
        public int OutTime10mS;//在此状态中超过时间参数，未达到退出条件时，调到超时状态
        public int DelayTime10mS; //在此状态中延时退出状态时间参数
        public int ExcTimes; //此状态执行次数参数
        public int nDelayTime10mS;//实时记录延时时间
        public int nOutTime10mS;
        public int nExcTimes;
        public string StatusState;

        public ClassStatusUnit()
        {

        }

        public ClassStatusUnit(string workStationName, int statusIndex, string statusEnName, Action actEnter, Action actDuring, int outTime10mS = 500, int delayTime10mS = 0, int excTimes = 1)
        {
            StrWorkStationName = workStationName;
            StatusIndex = statusIndex;
            StatusEnName = statusEnName;
            ActionEnter = actEnter;
            ActionDuring = actDuring;
            OutTime10mS = outTime10mS;
            DelayTime10mS = delayTime10mS;
            ExcTimes = excTimes;
            //超时退出状态
            // var condition0 = new StatusCondition((int)ClassVariants.eStatusUnits.S_StatusOutTime,
            //     () => OutTime10mS == 0, null);
            // LstCondition.Add(condition0);
            // LstTimers.Add(NDelayTime10mS);
            //LstTimers.Add(nOutTime10mS);
        }

        public ClassStatusUnit(string workStationName, int statusIndex, string statusEnName, Action actEnter,
            string actEnterStr, Action actDuring, string actDuringStr, int outTime10mS = 500, int delayTime10mS = 0,
            int excTimes = 1)
        {
            StrWorkStationName = workStationName;
            StatusIndex = statusIndex;
            StatusEnName = statusEnName;
            ActionEnter = actEnter;
            ActionDuring = actDuring;
            OutTime10mS = outTime10mS;
            DelayTime10mS = delayTime10mS;
            ExcTimes = excTimes;
            FunctionEnter = actEnterStr;
            FunctionDuring = actDuringStr;
            //超时退出状态
            // var condition0 = new StatusCondition((int)ClassVariants.eStatusUnits.S_StatusOutTime,
            //     () => OutTime10mS == 0, null);
            // LstCondition.Add(condition0);
            // LstTimers.Add(NDelayTime10mS);
            //LstTimers.Add(nOutTime10mS);

        }

        public ClassStatusUnit(int statusIndex, string statusEnName, Action actEnter, Action actDuring, int outTime10mS = 500, int delayTime10mS = 0, int excTimes = 1)
        {
            //StrWorkStationName = workStationName;
            StatusIndex = statusIndex;
            StatusEnName = statusEnName;
            ActionEnter = actEnter;
            ActionDuring = actDuring;
            OutTime10mS = outTime10mS;
            DelayTime10mS = delayTime10mS;
            ExcTimes = excTimes;
            //超时退出状态
            // var condition0 = new StatusCondition((int)ClassVariants.eStatusUnits.S_StatusOutTime,
            //     () => OutTime10mS == 0, null);
            // LstCondition.Add(condition0);
            // LstTimers.Add(NDelayTime10mS);
            //LstTimers.Add(nOutTime10mS);
        }

        public ClassStatusUnit(string statusEnName, string satusCnName, string functionEnter, string functionDuring)
        {
            // StatusIndex = statusIndex;
            StatusEnName = statusEnName;
            StatusCnName = satusCnName;
            FunctionEnter = functionEnter;
            FunctionDuring = functionDuring;
        }
    }

    public class Workstation
    {
        public Action StatusRun;
        public string Name;
        public string CurrentStatusUint;
        public string InitStatusUnit;
        //public readonly List<string> LstCurrentCheckDataStr = new List<string>();
        public readonly List<CheckData> LstCurrentCheckData = new List<CheckData>();
        public string StatusType;
        public int NStatusIndex = 0;    //当前状态
        public int NDelayTime10mS = 0;  //当前状态的参数
        public int NOutTIme10mS = 0;  //当前状态的参数
        public readonly List<ClassStatusUnit> LstClassStatusUnits = new List<ClassStatusUnit>();
        public readonly List<ClassCondition> LstClassConditions = new List<ClassCondition>();
        public string StrCheckData;
        public string WorkStationState;

        ///// <summary>
        ///// 获得检测数据
        ///// 名称,范围,标准,结果,单位
        ///// </summary>
        //public void GetCheckDataStrList()
        //{
        //    LstCurrentCheckDataStr.Clear();
        //    foreach (var t in LstCurrentCheckData)
        //    {
        //        LstCurrentCheckDataStr.Add(
        //            string.Format("{0},{1},{2},{3},{4}", t.ParaName, t.Range, t.Value, t.Result, t.Unit));
        //    }
        //}
    }

    public class CheckData
    {
        public string ProcessNo;
        public string Type;
        public string ParaName;
        public string Range;
        public string Value;
        public string Result;
        public string Unit;
        public string Format;
    }
}
