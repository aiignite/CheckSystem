using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using CommonUtility.FileOperator;

namespace StateMachine
{
    public sealed class State
    {
        public delegate void PushEndResultEventHandle(string processNo, List<CheckData> value);
        public event PushEndResultEventHandle PushEndResult;

        public delegate void PushDisplayEventHandle(List<CheckData> value);
        public event PushDisplayEventHandle PushDisplay;

        public delegate void PushEnterEventHandle(string wsName, string enterStaus, string[] enterAction);
        public event PushEnterEventHandle PushEnter;

        public delegate void PushDuringEventHandle(string wsName, string enterStaus, string[] enterAction);
        public event PushDuringEventHandle PushDuring;

        public bool IsPaused { get; set; }

        public string XmlFilePath { get; set; }

        public DeviceConfig DeviceConfig { get; private set; }

        /// <summary>
        /// Controllers List.
        /// </summary>
        public List<object> LstControllers { get; private set; }

        /// <summary>
        /// Workstations List.
        /// </summary>
        public List<Workstation> LstWorkstations { get; private set; }

        /// <summary>
        /// 设备号
        /// </summary>
        public string StrDeviceNo { get; private set; }

        /// <summary>
        /// 设备版本
        /// </summary>
        public string StrDeviceVersion { get; private set; }

        /// <summary>
        /// 工序号
        /// </summary>
        public string StrProcessNo { get; private set; }

        /// <summary>
        /// 工序版本
        /// </summary>
        public string StrProcessVersion { get; private set; }

        public void Init<T>(string filePath, string assemblyFile)
        {
            InitSysConfig(filePath);
            ControllerConfig<T>(assemblyFile);
            ProcessparametersConfig();
            StateMachineConfig(filePath);
        }

        private void InitSysConfig(string filePath)
        {
            XmlFilePath = filePath;
            DeviceConfig = XmlHelper.Deserialize<DeviceConfig>(filePath);
            DeviceConfig.Paras = DeviceConfig.Paras == null ? new DeviceConfigPara[0] : DeviceConfig.Paras;
            DeviceConfig.Gears = DeviceConfig.Gears == null ? new DeviceConfigGear[0] : DeviceConfig.Gears;
            DeviceConfig.Rois = DeviceConfig.Rois == null ? new DeviceConfigRoi[0] : DeviceConfig.Rois;
        }

        private void ControllerConfig<T>(string assemblyFile)
        {
            LstControllers = new List<object>();
            var controllers = DeviceConfig.Controllers;
            var asmb = Assembly.LoadFrom(assemblyFile);

            foreach (var controller in from item in controllers
                                       let typeName = asmb.GetType("Controller." + item.Type)
                                       where typeName != null
                                       select (T)Activator.CreateInstance(typeName, item.Name))
                LstControllers.Add(controller);
        }

        public readonly Dictionary<string, List<string>> BarcodeGroupList =
            new Dictionary<string, List<string>>();
        public readonly Dictionary<string, List<string>> LedGroupList =
            new Dictionary<string, List<string>>();

        private void ProcessparametersConfig()
        {
            try
            {
                var processparas = DeviceConfig.Paras;
                if (processparas == null)
                    return;

                foreach (var para in processparas)
                {
                    if (para.DataType.ToLower().Equals("BarcodeGroup".ToLower()))
                    {
                        if (DeviceConfig.Gears == null)
                            continue;

                        var findGear = DeviceConfig.Gears.ToList().FindAll(f => f.Name == para.Name);

                        if (!findGear.Any())
                            continue;

                        BarcodeGroupList.Add(para.Name, new List<string>());

                        foreach (var t in findGear)
                        {
                            var tempCharList = para.Value.Select(c => c.ToString().ToUpper()).ToList();

                            if (!string.IsNullOrEmpty(t.MatchingCodeContent))
                                for (var i = 0; i < t.MatchingCodeContent.Length; i++)
                                    tempCharList[int.Parse(t.MatchingCodeIndex) - 1 + i] = t.MatchingCodeContent[i].ToString();

                            if (!string.IsNullOrEmpty(t.Gear1Content))
                                for (var i = 0; i < t.Gear1Content.Length; i++)
                                    tempCharList[int.Parse(t.Gear1Index) - 1 + i] = t.Gear1Content[i].ToString();

                            if (!string.IsNullOrEmpty(t.Gear2Content))
                                for (var i = 0; i < t.Gear2Content.Length; i++)
                                    tempCharList[int.Parse(t.Gear2Index) - 1 + i] = t.Gear2Content[i].ToString();

                            if (!string.IsNullOrEmpty(t.Gear3Content))
                                for (var i = 0; i < t.Gear3Content.Length; i++)
                                    tempCharList[int.Parse(t.Gear3Index) - 1 + i] = t.Gear3Content[i].ToString();

                            if (!string.IsNullOrEmpty(t.Gear4Content))
                                for (var i = 0; i < t.Gear4Content.Length; i++)
                                    tempCharList[int.Parse(t.Gear4Index) - 1 + i] = t.Gear4Content[i].ToString();

                            var str = tempCharList.Aggregate(string.Empty, (current, c) => current + c.ToUpper());
                            BarcodeGroupList[para.Name].Add(str);
                        }
                    }
                    else if (para.DataType.ToLower().Equals("LedGroup".ToLower()))
                    {
                        if (string.IsNullOrEmpty(para.Value))
                            continue;

                        var paraValueSplit = para.Value.Split(new[] { "[", "]" }, StringSplitOptions.RemoveEmptyEntries);

                        if (!paraValueSplit.Any())
                            continue;

                        LedGroupList.Add(para.Name, new List<string>());

                        foreach (var group in paraValueSplit[0].Split(','))
                            LedGroupList[para.Name].Add(@group);
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
            //StrProcessNo = processparas.processno.ToString();
            //StrProcessVersion = processparas.parasversion;
        }

        private void StateMachineConfig(string filePath)
        {
            LstWorkstations = new List<Workstation>();

            var workstations = DeviceConfig.WorkStations;

            foreach (var w in workstations)
            {
                var wName = w.Name;

                var ws = new Workstation
                {
                    Name = wName,
                    CurrentStatusUint = StateMachineHelper.EDefaultStateUnits.Idle.ToString().ToUpper(),
                    InitStatusUnit = StateMachineHelper.EDefaultStateUnits.Idle.ToString().ToUpper()
                };

                var stateUnits = DeviceConfig.StatusUnits.ToList().FindAll(f => f.WorkStationName.Equals(w.Name));

                var indexSu = 3;
                var w1 = w;
                //foreach (var newState in from object item in Enum.GetValues(typeof(StateMachineHelper.EDefaultStateUnits))
                //                         let state = stateUnits.Find(t => string.Equals(t.Name.ToUpper(), item.ToString().ToUpper(), StringComparison.CurrentCultureIgnoreCase))
                //                         where state == null
                //                         select new DeviceConfigStatusUnit
                //                             {
                //                                 WorkStationName = w1.Name,
                //                                 Name = item.ToString().ToUpper(),
                //                                 DuringFunction = string.Empty,
                //                                 EnterFunction = string.Empty,
                //                                 PositionSize = 300 * indexSu++ + ",340,180,90"
                //                             })
                //    stateUnits.Add(newState);

                var tempList = DeviceConfig.StatusUnits.ToList();
                tempList.RemoveAll(f => f.WorkStationName.Equals(w.Name));
                tempList.AddRange(stateUnits);

                DeviceConfig.StatusUnits = tempList.ToArray();

                foreach (
                    var temp in
                        from su in DeviceConfig.StatusUnits
                        where su.WorkStationName == wName
                        select DesignStatusUnit(su, ws))
                    ws.LstClassStatusUnits.Add(temp);

                LstWorkstations.Add(ws);

                foreach (
                    var temp in
                        DeviceConfig.Conditions.Where(con => con.WorkStationName == wName)
                            .Select(con => DesignCondition(con, ws)))
                    ws.LstClassConditions.Add(temp);
            }

            DeviceConfig.StatusUnits = DeviceConfig.StatusUnits.ToList().OrderBy(f => f.WorkStationName).ToArray();
            DeviceConfig.Conditions = DeviceConfig.Conditions.ToList().OrderBy(f => f.WorkStationName).ToArray();
            //XmlHelper.SerializeToFile(DeviceConfig, filePath, Encoding.UTF8);
        }

        private ClassStatusUnit DesignStatusUnit(
           DeviceConfigStatusUnit status, Workstation ws)
        {
            //例子：PowerIn.Field.Voltage1 OR PowerIn.Method.SetVoltage(System.String voltage)
            var actEnter = new Func<Action>(() => ChangeStringToAction(status.EnterFunction));
            var actDuring = new Func<Action>(() => ChangeStringToAction(status.DuringFunction));

            return new ClassStatusUnit(ws.Name, ws.LstClassStatusUnits.Count, status.Name, actEnter(), status.EnterFunction, actDuring(),
                status.DuringFunction);
        }

        private ClassCondition DesignCondition(
            DeviceConfigCondition condition, Workstation ws)
        {
            var actGetConditionResult =
                new Func<Func<object>>(() => ChangeStringToConditionFunc(condition.ConditionFunction, ws));

            var actExit = new Func<Action>(() => ChangeStringToAction(condition.ExitFunction));

            return new ClassCondition(condition.SourceSuName, condition.TargetSuName, actGetConditionResult(),
                actExit(), condition.ConditionFunction.StrTrim());
        }

        private Action ChangeStringToAction(string strAction)
        {
            if (string.IsNullOrEmpty(strAction))
                return null;

            var strTemp = strAction.StrTrim();

            if (strTemp.Length <= 0)
                return null;

            var lstCodeLine = new List<string>(strTemp.TrimEnd(';').Split(';'));

            var lstExecutableCodeLine =
                lstCodeLine.Select(codeLine => new ExecutableCodeLine(codeLine, LstControllers, DeviceConfig)).ToList();

            return () =>
            {
                foreach (var t in lstExecutableCodeLine)
                    t.Invoke();
            };
        }

        private Func<ConditionalCodeLine> ChangeStringToConditionFunc(
            string strCondition, Workstation ws)
        {
            if (string.IsNullOrEmpty(strCondition))
            //throw new Exception(ws.Name + " 条件为空，请检查");
            {
                return () => null;
            }

            var strTemp = strCondition.StrTrim();

            if (strTemp.Length <= 0)
                throw new Exception("条件为空，请检查");

            var actionWithPara = new Action<CheckData, bool>((para, isPush) =>
            {
                var addDataFunc = new Func<bool>(() =>
                {
                    foreach (var t in ws.LstCurrentCheckData.Where(t => t.ParaName.Equals(para.ParaName) &&
                                                                        t.ProcessNo.Equals(para.ProcessNo) &&
                                                                        t.Range.Equals(para.Range)))
                    {
                        t.Value = para.Value;
                        t.Result = para.Result;
                        return true;
                    }

                    return false;
                });

                if (!addDataFunc())
                    ws.LstCurrentCheckData.Add(para);

                //ws.GetCheckDataStrList();
                OnPushDisplay(ws.LstCurrentCheckData, isPush);
            });

            var conditionalCodeLine =
                new ConditionalCodeLine(strTemp, LstControllers, DeviceConfig.Paras, actionWithPara, DeviceConfig);

            return () =>
            {
                conditionalCodeLine.Invoke();
                return conditionalCodeLine;
            };
        }

        [DllImport("Kernel32.dll")]
        public static extern IntPtr GetCurrentThread();

        [DllImport("Kernel32.dll")]
        public static extern int SetThreadIdealProcessor(IntPtr hThread, uint dwIdealProcessor);

        public void TaskCheckStatus()
        {
            // 设置CPU线程优先级
            SetThreadIdealProcessor(GetCurrentThread(), 0u);

            foreach (var t in LstWorkstations)
            {
                try
                {
                    t.StatusRun = GetWorkstationAction(t);
                    t.StatusRun.BeginInvoke(StatusRunEnd, t);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private static void StatusRunEnd(IAsyncResult ir)
        {
            try
            {
                if (ir == null || !ir.IsCompleted)
                    return;

                var temp = ir.AsyncState as Workstation;
                if (temp == null) return;
                temp.StatusRun.EndInvoke(ir);
                temp.StatusRun.BeginInvoke(StatusRunEnd, temp);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private Action GetWorkstationAction(Workstation workstation)
        {
            return () =>
            {
                Thread.Sleep(25);

                if (IsPaused)
                    return;

                var statusunit =
                    workstation.LstClassStatusUnits.Find(
                        t => t.StatusEnName.ToUpper() == workstation.CurrentStatusUint.ToUpper());

                if (statusunit != null)
                {
                    var isSampleCheck = false;
                    foreach (var comparedValue in LstControllers
                        .Where(item => item.GetType().Name == "CheckApp").Select(item => item.GetType()
                        .GetField("IsByPass")
                        .GetValue(item).ToString()).Where(comparedValue => comparedValue == true.ToString()))
                        isSampleCheck = bool.Parse(comparedValue);

                    var tempConditions =
                        workstation.LstClassConditions.Where(
                            condition => condition.StrSourceStatus.ToUpper() == workstation.CurrentStatusUint.ToUpper())
                            .ToList();

                    if (!statusunit.EnterOnce)
                    {
                        //Thread.Sleep(5000);

                        string[] enterStrings = null;
                        if (!string.IsNullOrEmpty(statusunit.FunctionEnter.StrTrim()))
                        {
                            var s = new List<string>(statusunit.FunctionEnter.StrTrim().TrimEnd(';').Split(';'));
                            enterStrings = new string[s.Count];
                            Array.Copy(s.ToArray(), enterStrings, s.Count);
                        }
                        OnPushEnter(workstation.Name, workstation.CurrentStatusUint, enterStrings);

                        if (string.Equals(statusunit.StatusEnName,
                            StateMachineHelper.EDefaultStateUnits.CheckFail.ToString(),
                            StringComparison.CurrentCultureIgnoreCase))
                        {
                            var listPara =
                                workstation.LstCurrentCheckData.GroupBy(g => g.ProcessNo).Select(f => f.Key).ToList();

                            foreach (var t in listPara)
                            {
                                workstation.LstCurrentCheckData.Add(new CheckData
                                {
                                    ProcessNo = t,
                                    ParaName = "检测结果NG",
                                    Range = "OK",
                                    Result = false.ToString(),
                                    Unit = string.Empty,
                                    Value = "NG"
                                });
                                OnPushDisplay(workstation.LstCurrentCheckData, true);
                                OnPushEnd(t, workstation.LstCurrentCheckData);
                                workstation.LstCurrentCheckData.Clear();
                            }
                        }
                        else if (string.Equals(statusunit.StatusEnName,
                            StateMachineHelper.EDefaultStateUnits.CheckEnd.ToString(),
                            StringComparison.CurrentCultureIgnoreCase))
                        {
                            var listPara =
                                workstation.LstCurrentCheckData.GroupBy(g => g.ProcessNo)
                                    .Select(f => f.Key)
                                    .ToList();

                            if (isSampleCheck)
                            {
                                foreach (var t in listPara)
                                {
                                    workstation.LstCurrentCheckData.Add(new CheckData
                                    {
                                        ProcessNo = t,
                                        ParaName = "样件检测结果",
                                        Range = "结束",
                                        Result = true.ToString(),
                                        Unit = string.Empty,
                                        Value = "结束"
                                    });
                                    OnPushDisplay(workstation.LstCurrentCheckData, true);
                                    OnPushEnd(t, workstation.LstCurrentCheckData);
                                    workstation.LstCurrentCheckData.Clear();
                                }
                            }
                            else
                            {
                                foreach (var t in listPara)
                                {
                                    workstation.LstCurrentCheckData.Add(new CheckData
                                    {
                                        ProcessNo = t,
                                        ParaName = "检测结果OK",
                                        Range = "OK",
                                        Result = true.ToString(),
                                        Unit = string.Empty,
                                        Value = "OK"
                                    });
                                    OnPushDisplay(workstation.LstCurrentCheckData, true);
                                    OnPushEnd(t, workstation.LstCurrentCheckData);
                                    workstation.LstCurrentCheckData.Clear();
                                }
                            }
                        }

                        //当开始检测前进入idle状态时清空工作站检测数据
                        if (workstation.CurrentStatusUint.Equals(workstation.InitStatusUnit))
                            workstation.LstCurrentCheckData.Clear();

                        statusunit.EnterOnce = true;
                        if (isSampleCheck && string.Equals(statusunit.StatusEnName,
                            StateMachineHelper.EDefaultStateUnits.CheckEnd.ToString(),
                            StringComparison.CurrentCultureIgnoreCase))
                        {

                        }
                        else
                        {
                            if (statusunit.ActionEnter != null)
                                statusunit.ActionEnter();
                        }
                    }

                    // during执行函数
                    if (statusunit.ActionDuring != null)
                        statusunit.ActionDuring();
                    string[] durningStrings = null;
                    if (!string.IsNullOrEmpty(statusunit.FunctionDuring.StrTrim()))
                    {
                        var s = new List<string>(statusunit.FunctionDuring.StrTrim().TrimEnd(';').Split(';'));
                        durningStrings = new string[s.Count];
                        Array.Copy(s.ToArray(), durningStrings, s.Count);
                    }
                    OnPushDuring(workstation.Name, workstation.CurrentStatusUint, durningStrings);

                    // 在原状态单元是当前状态单元的条件列表中查找满足的条件
                    for (var i = 0; i < tempConditions.Count; i++)
                    {
                        var result = tempConditions[i].FuncCondition() as ConditionalCodeLine;
                        var isTrue = result != null && Convert.ToBoolean(result.ComparativeResult);

                        if (!isTrue)
                        {
                            if (tempConditions[i].StrCondition.Contains(".Para."))
                            {
                                if (isSampleCheck)
                                {
                                    if (tempConditions[i].StrTargerStatus.Equals(
                                        StateMachineHelper.EDefaultStateUnits.CheckEnd.ToString()))
                                    {
                                        tempConditions[i].PreviousResult = string.Empty;
                                        if (tempConditions[i].ActionExit != null)
                                            tempConditions[i].ActionExit();
                                        statusunit.EnterOnce = false;
                                        workstation.CurrentStatusUint =
                                            StateMachineHelper.EDefaultStateUnits.CheckFail.ToString();
                                        break;
                                    }

                                    tempConditions[i].PreviousResult = string.Empty;
                                    if (tempConditions[i].ActionExit != null)
                                        tempConditions[i].ActionExit();
                                    statusunit.EnterOnce = false;
                                    workstation.CurrentStatusUint = tempConditions[i].StrTargerStatus;
                                    break;
                                }

                                if (statusunit.StatusEnName !=
                                    StateMachineHelper.EDefaultStateUnits.CheckFail.ToString())
                                {
                                    tempConditions[i].PreviousResult = string.Empty;
                                    if (tempConditions[i].ActionExit != null)
                                        tempConditions[i].ActionExit();
                                    statusunit.EnterOnce = false;
                                    workstation.CurrentStatusUint =
                                        StateMachineHelper.EDefaultStateUnits.CheckFail.ToString();
                                    break;
                                }
                            }

                            continue;
                        }
                        tempConditions[i].PreviousResult = string.Empty;

                        //显示检测结果
                        //DisplayCheckData(workstation.LstCurrentCheckData);
                        if (tempConditions[i].ActionExit != null)
                            tempConditions[i].ActionExit();
                        statusunit.EnterOnce = false;

                        if (tempConditions[i].StrCondition.Contains(".Para.") && isSampleCheck &&
                            tempConditions[i].StrTargerStatus.Equals(
                                StateMachineHelper.EDefaultStateUnits.CheckEnd.ToString().ToUpper()))
                            workstation.CurrentStatusUint =
                                StateMachineHelper.EDefaultStateUnits.CheckEnd.ToString();
                        else
                            workstation.CurrentStatusUint =
                                tempConditions[i].StrTargerStatus;
                        break;
                    }
                }
            };
        }

        private void OnPushDisplay(List<CheckData> value, bool isPushDisplay)
        {
            if (PushDisplay != null && isPushDisplay)
                PushDisplay.Invoke(value);
        }

        private void OnPushEnd(string processNo, List<CheckData> value)
        {
            if (PushEndResult != null)
                PushEndResult(processNo, value);
        }

        private void OnPushEnter(string wsName, string enterstaus, string[] enteraction)
        {
            if (PushEnter != null)
                PushEnter.Invoke(
                    wsName, enterstaus, enteraction);
        }

        private void OnPushDuring(string wsname, string enterstaus, string[] enteraction)
        {
            if (PushDuring != null)
                PushDuring.Invoke(wsname, enterstaus, enteraction);
        }
    }
}
