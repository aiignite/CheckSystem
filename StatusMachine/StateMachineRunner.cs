using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CommonUtility;
using CommonUtility.FileOperator;
using Controller;

namespace StateMachine
{
    public sealed class StateMachineRunner
    {
        public DeviceConfig DeviceConfig { get; set; }
        public string ConfigFilePath { get; set; }
        public Dictionary<string, ControllerBase> ControllerList =
            new Dictionary<string, ControllerBase>();

        public Dictionary<string, StateMachineClass.Workstation> WorkstationList =
            new Dictionary<string, StateMachineClass.Workstation>();
        public Dictionary<string, Dictionary<string, StateMachineClass.StateUnit>> StateUnitList =
            new Dictionary<string, Dictionary<string, StateMachineClass.StateUnit>>();
        public Dictionary<string, Dictionary<string, StateMachineClass.Condition>> ConditionList =
            new Dictionary<string, Dictionary<string, StateMachineClass.Condition>>();

        //public Dictionary<string, StateMachine<StateMachineClass.StateUnit, StateMachineClass.Condition>> StateMachines =
        //    new Dictionary<string, StateMachine<StateMachineClass.StateUnit, StateMachineClass.Condition>>();

        public Dictionary<string, Func<object>> PartGetValueList = new Dictionary<string, Func<object>>();
        public Dictionary<string, Action<string, object>> PartSetValueList = new Dictionary<string, Action<string, object>>();
        public Dictionary<string, Func<object>> ParaList = new Dictionary<string, Func<object>>();

        public StateMachineRunner(string configFilePath)
        {
            ConfigFilePath = configFilePath;
        }

        public void InitStateMachine()
        {
            InitDeviceConfig();
            InitControllers();
            InitPartsGetValue();
            InitParasGetValue();
            InitWorkstation();

            //var w = WorkstationList[WorkstationList.Keys.ToList()[0]];
            //w.MachineStart();

            //var listAction = new List<Action>();
            //ChangeStringToAction("检测程序.Field.Bint0++;\r\nSY-G-TEST.Part.测试1=1;", ref listAction);
            //foreach (var ac in listAction.Where(ac => ac != null))
            //    ac.Invoke();

            //ChangeStringToAction("SY-G-TEST.Part.测试1=1;", ref listAction);
            //ChangeStringToAction("检测程序.Field.Bbool1=SY-G-TEST.Part.测试1;", ref listAction);
            //ChangeStringToAction("检测程序.Method.Sleep(检测程序.Field.Bint3,3)");
        }

        private void InitDeviceConfig()
        {
            try
            {
                DeviceConfig = XmlHelper.Deserialize<DeviceConfig>(ConfigFilePath);
            }
            catch (Exception)
            {
                DeviceConfig = null;
            }
        }

        private void InitControllers()
        {
            if (DeviceConfig == null || DeviceConfig.Controllers == null)
                return;

            var asmb = Assembly.LoadFrom("Controller.dll");

            foreach (var c in DeviceConfig.Controllers)
            {
                var controller = asmb.GetType("Controller." + c.Type);
                if (controller != null && !ControllerList.ContainsKey(c.Name))
                    ControllerList.Add(c.Name, Activator.CreateInstance(controller, c.Name) as ControllerBase);
            }
        }

        private void InitPartsGetValue()
        {
            if (DeviceConfig == null || DeviceConfig.Parts == null)
                return;

            foreach (var partCode in DeviceConfig.Parts.Where(
                p => !string.IsNullOrEmpty(p.Name) &&
                     !string.IsNullOrEmpty(p.ControllerName) &&
                     !string.IsNullOrEmpty(p.ControllerField)).Select(p => string.Format("{0}.Part.{1}", p.ProcessNo, p.Name)))
            {
                if (!PartGetValueList.ContainsKey(partCode))
                    PartGetValueList.Add(partCode, GetPartValue(partCode));

                if (PartSetValueList.ContainsKey(partCode))
                    PartSetValueList.Add(partCode, SetPartValue());
            }
        }

        private void InitParasGetValue()
        {
            if (DeviceConfig == null || DeviceConfig.Paras == null)
                return;

            foreach (
                var paraName in
                    DeviceConfig.Paras.Where(p => !string.IsNullOrEmpty(p.ProcessNo) && !string.IsNullOrEmpty(p.Name))
                        .Select(p => string.Format("{0}.para.{1}", p.ProcessNo, p.Name))
                        .Where(paraName => !ParaList.ContainsKey(paraName)))
                ParaList.Add(paraName, GetParaValue(paraName));
        }

        private void InitWorkstation()
        {
            #region 初始化每个工作站

            if (DeviceConfig == null || DeviceConfig.WorkStations == null)
                return;

            foreach (
                var ws in DeviceConfig.WorkStations.Where(ws => !WorkstationList.ContainsKey(ws.Name)))
            {
                WorkstationList.Add(ws.Name, new StateMachineClass.Workstation(ws.Name, "IDLE"));
                var ws1 = ws;

                #region 添加状态
                if (DeviceConfig.StatusUnits != null)
                {
                    if (!StateUnitList.ContainsKey(ws1.Name))
                    {
                        StateUnitList.Add(ws1.Name, new Dictionary<string, StateMachineClass.StateUnit>());

                        foreach (var su in DeviceConfig.StatusUnits.Where(su => su.WorkStationName == ws1.Name))
                        {
                            if (!StateUnitList[ws1.Name].ContainsKey(su.Name))
                            {
                                var newSu = new StateMachineClass.StateUnit(ws1.Name, su.Name);
                                newSu.NExcTime = newSu.ExcTimes;

                                var entryAcions = new List<Action>();
                                ChangeStringToAction(su.EnterFunction, ref entryAcions);
                                newSu.EntryAction = new StateMachineAction(
                                    ws1.Name, newSu.Name,
                                    () =>
                                    {
                                        foreach (var ac in entryAcions.Where(ac => ac != null))
                                            ac.Invoke();
                                    },
                                    StateMachineActionType.StateEntry);

                                var duringActions = new List<Action>();
                                ChangeStringToAction(su.DuringFunction, ref duringActions);
                                newSu.DurningAcion = new StateMachineAction(
                                    ws1.Name, newSu.Name,
                                    () =>
                                    {
                                        foreach (var ac in duringActions.Where(ac => ac != null))
                                            ac.Invoke();
                                    },
                                    StateMachineActionType.StateDuring);

                                StateUnitList[ws1.Name].Add(newSu.Name, newSu);
                            }
                        }
                    }
                }
                #endregion

                #region 添加条件
                if (DeviceConfig.Conditions != null)
                {
                    if (!ConditionList.ContainsKey(ws1.Name))
                    {
                        ConditionList.Add(ws1.Name, new Dictionary<string, StateMachineClass.Condition>());

                        foreach (var c in DeviceConfig.Conditions.Where(c => c.WorkStationName == ws1.Name))
                        {
                            var newC = new StateMachineClass.Condition(
                            ws1.Name, c.Name, c.SourceSuName, c.TargetSuName);

                            Func<StateMachineClass.Condition.ConditionFuncStruct> conditionFunc;
                            ChangeStringToFunc(WorkstationList[ws1.Name], c.ConditionFunction, out conditionFunc);
                            newC.ConditionFunc =
                                () => conditionFunc.Invoke();

                            var exitActions = new List<Action>();
                            ChangeStringToAction(c.ExitFunction, ref exitActions);
                            newC.ExitAction = new StateMachineAction(
                                ws1.Name, newC.Name,
                                () =>
                                {
                                    foreach (var ac in exitActions.Where(ac => ac != null))
                                        ac.Invoke();
                                },
                                StateMachineActionType.ConditionExit);

                            ConditionList[ws1.Name].Add(newC.Name, newC);
                        }
                    }
                }
                #endregion

                #region 配置状态机
                //var w = WorkstationList[ws.Name];
                //if (w.StateUnitList.ContainsKey(w.InitStateUnitName))
                //{
                //    var newStateMachine =
                //        new StateMachine<StateMachineClass.StateUnit, StateMachineClass.Condition>(
                //            () => w.InternStateUintBeforeIdle,
                //            s => w.InternStateUintBeforeIdle = s);

                //    newStateMachine.Configure(w.InternStateUintBeforeIdle)
                //        .Permit(w.InternCondintionBeforeIdle, w.StateUnitList[w.InitStateUnitName]);

                //    foreach (var stateUint in w.StateUnitList.Keys.Select(key => w.StateUnitList[key]))
                //    {
                //        var u = stateUint;
                //        newStateMachine.Configure(stateUint)
                //            .OnEntry(() => RunStateUnitEnterAcion(u));
                //    }

                //    foreach (var key in w.ConditionList.Keys)
                //    {
                //        var condition = w.ConditionList[key];
                //        var st = newStateMachine.Configure(w.StateUnitList[condition.SourceStateName]);
                //        st.Permit(condition, w.StateUnitList[condition.TargetStateName]);
                //    }
                //}
                #endregion
            }

            #endregion
        }

        #region 状态机运行逻辑

        public void RunStateMachine()
        {
            foreach (var su in from wsKey in WorkstationList.Keys
                               where StateUnitList.ContainsKey(wsKey) &&
                                     StateUnitList[wsKey].ContainsKey(WorkstationList[wsKey].InitStateUnitName)
                               select StateUnitList[wsKey][WorkstationList[wsKey].InitStateUnitName])
                su.EntryAction.Action.BeginInvoke(StateUnitAcionRunEnd, su);
        }

        /// <summary>
        /// 函数执行结束
        /// </summary>
        /// <param name="ir"></param>
        private void StateUnitAcionRunEnd(IAsyncResult ir)
        {
            if (ir == null)
                return;

            var tempSu = ir.AsyncState as StateMachineClass.StateUnit;
            if (tempSu == null)
                return;

            tempSu.DurningAcion.Action.Invoke();

            if (ConditionList.ContainsKey(tempSu.WorkStationName))
            {
                foreach (
                    var con in
                        ConditionList[tempSu.WorkStationName].Values.ToList()
                            .OrderBy(f => f.Priority)
                            .Where(con => con.SourceStateName == tempSu.Name))
                {
                    if (con.IsHaveCheckPara)
                    {
                        var checkApp = ControllerList.Values.ToList()
                            .Find(f => f.GetType() == typeof(CheckApp));

                        if (checkApp != null)
                        {
                            if (((CheckApp)checkApp).IsByPass)
                            {
                                con.ExitAction.Action.Invoke();
                                if (StateUnitList.ContainsKey(con.WorkStationName) &&
                                    StateUnitList[con.WorkStationName].ContainsKey(con.TargetStateName))
                                {
                                    var targetSu = StateUnitList[con.WorkStationName][con.TargetStateName];
                                    targetSu.EntryAction.Action.BeginInvoke(StateUnitAcionRunEnd, targetSu);
                                    return;
                                }
                            }
                            else
                            {
                                if (con.ConditionFunc.Invoke().CompareResult)
                                {
                                    con.ExitAction.Action.Invoke();
                                    if (StateUnitList.ContainsKey(con.WorkStationName) &&
                                        StateUnitList[con.WorkStationName].ContainsKey(con.TargetStateName))
                                    {
                                        var targetSu = StateUnitList[con.WorkStationName][con.TargetStateName];
                                        targetSu.EntryAction.Action.BeginInvoke(StateUnitAcionRunEnd, targetSu);
                                        return;
                                    }
                                }
                                else
                                {
                                    var checkFailedSuName =
                                        StateMachineHelper.EDefaultStateUnits.CheckFail.ToString().ToUpper();

                                    con.ExitAction.Action.Invoke();
                                    if (StateUnitList.ContainsKey(con.WorkStationName) &&
                                        StateUnitList[con.WorkStationName].ContainsKey(checkFailedSuName))
                                    {
                                        var su = StateUnitList[con.WorkStationName][checkFailedSuName];
                                        su.EntryAction.Action.BeginInvoke(StateUnitAcionRunEnd, su.EntryAction);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (con.ConditionFunc.Invoke().CompareResult)
                        {
                            con.ExitAction.Action.Invoke();
                            if (StateUnitList.ContainsKey(con.WorkStationName) &&
                                StateUnitList[con.WorkStationName].ContainsKey(con.TargetStateName))
                            {
                                var targetSu = StateUnitList[con.WorkStationName][con.TargetStateName];
                                targetSu.EntryAction.Action.BeginInvoke(StateUnitAcionRunEnd, targetSu);
                                return;
                            }
                        }
                    }
                }
            }

            new Action(() => { }).BeginInvoke(StateUnitAcionRunEnd, tempSu);
        }

        #endregion

        /// <summary>
        /// 获取字段的当前值
        /// </summary>
        /// <param name="field">.Field.</param>
        /// <returns></returns>
        public Func<object> GetFieldValue(string field)
        {
            return () =>
            {
                if (!string.IsNullOrEmpty(field) && field.Contains(".Field."))
                {
                    var sp = field.GetStrsSplitByValue(".Field.", true);
                    if (sp.Length == 2)
                    {
                        var controllerName = sp[0];
                        var fieldName = sp[1];

                        if (ControllerList.ContainsKey(controllerName))
                        {
                            var controller = ControllerList[controllerName];
                            if (controller != null)
                            {
                                var findField = ControllerList[controllerName].GetType().GetField(fieldName);
                                if (findField != null)
                                    return findField.GetValue(controller);
                            }
                        }
                    }
                }

                return null;
            };
        }

        public Action<string, object> SetFieldValue()
        {
            return (field, fieldValue) =>
            {
                if (string.IsNullOrEmpty(field) || !field.Contains(".Field."))
                    return;
                var sp = field.GetStrsSplitByValue(".Field.", true);

                if (sp.Length != 2)
                    return;

                var controllerName = sp[0];
                var fieldName = sp[1];

                if (!ControllerList.ContainsKey(controllerName))
                    return;

                var controller = ControllerList[controllerName];
                if (controller == null)
                    return;
                var findField =
                    ControllerList[controllerName].GetType().GetField(fieldName);
                if (findField == null)
                    return;
                try
                {
                    var obj = Convert.ChangeType(fieldValue, findField.FieldType);
                    findField.SetValue(controller, obj);
                }
                catch (Exception)
                {
                    // ignored
                }
            };
        }

        public Type GetFieldType(string field)
        {
            if (string.IsNullOrEmpty(field) || !field.Contains(".Field."))
                return null;
            var sp = field.GetStrsSplitByValue(".Field.", true);
            if (sp.Length != 2)
                return null;
            var controllerName = sp[0];
            var fieldName = sp[1];

            if (!ControllerList.ContainsKey(controllerName))
                return null;
            var controller = ControllerList[controllerName];
            if (controller == null)
                return null;
            var findField =
                ControllerList[controllerName].GetType().GetField(fieldName);
            return findField != null ? findField.FieldType : null;
        }

        /// <summary>
        /// 获取部件映射的字段的当前值
        /// </summary>
        /// <param name="part">.part.</param>
        /// <returns></returns>
        public Func<object> GetPartValue(string part)
        {
            return () =>
            {
                if (!string.IsNullOrEmpty(part) && part.Contains(".Part."))
                {
                    var sp = part.GetStrsSplitByValue(".Part.", true);
                    if (sp.Length == 2)
                    {
                        var no = sp[0];
                        var partName = sp[1];

                        if (DeviceConfig != null && DeviceConfig.Parts != null)
                        {
                            var findPartIndex =
                                DeviceConfig.Parts.ToList().FindIndex(f => f.ProcessNo == no && f.Name == partName);
                            if (findPartIndex != -1)
                            {
                                var findPart = DeviceConfig.Parts.ToList()[findPartIndex];

                                if (!string.IsNullOrEmpty(findPart.ControllerName) &&
                                    !string.IsNullOrEmpty(findPart.ControllerField))
                                    return GetFieldValue(findPart.ControllerField).Invoke();
                            }
                        }
                    }
                }

                return null;
            };
        }

        public Action<string, object> SetPartValue()
        {
            return (part, value) =>
            {
                if (string.IsNullOrEmpty(part) || !part.Contains(".Part."))
                    return;

                var sp = part.GetStrsSplitByValue(".Part.", true);
                if (sp.Length != 2)
                    return;

                var no = sp[0];
                var partName = sp[1];

                if (DeviceConfig == null || DeviceConfig.Parts == null)
                    return;

                var findPartIndex =
                    DeviceConfig.Parts.ToList().FindIndex(f => f.ProcessNo == no && f.Name == partName);
                if (findPartIndex == -1)
                    return;

                var findPart = DeviceConfig.Parts.ToList()[findPartIndex];

                if (!string.IsNullOrEmpty(findPart.ControllerName) &&
                    !string.IsNullOrEmpty(findPart.ControllerField))
                    SetFieldValue().Invoke(findPart.ControllerField, value);
            };
        }

        /// <summary>
        /// 获取参数对应的字段的当前值
        /// </summary>
        /// <param name="para">.para.</param>
        /// <returns></returns>
        public Func<object> GetParaValue(string para)
        {
            return () =>
            {
                if (!string.IsNullOrEmpty(para) && para.Contains(".Para."))
                {
                    var sp = para.GetStrsSplitByValue(".Para.", true);

                    if (sp.Length == 1)
                    {
                        var no = sp[0];
                        var paraName = sp[1];

                        if (!string.IsNullOrEmpty(no) &&
                            !string.IsNullOrEmpty(paraName) && DeviceConfig.Paras != null)
                        {
                            var findPara =
                                DeviceConfig.Paras.ToList().Find(f => f.ProcessNo == no && f.Name == paraName);

                            if (findPara != null && !string.IsNullOrEmpty(findPara.ControllerField))
                            {
                                if (!string.IsNullOrEmpty(findPara.DataType) &&
                                    findPara.DataType.ToLower() == "double")
                                {

                                }
                                else
                                {
                                    return GetFieldValue(findPara.ControllerField);
                                }
                            }
                        }
                    }
                }

                return null;
            };
        }

        public Type GetPartType(string part)
        {
            if (string.IsNullOrEmpty(part) || !part.Contains(".Part."))
                return null;
            var sp = part.GetStrsSplitByValue(".Part.", true);
            if (sp.Length != 2)
                return null;
            var no = sp[0];
            var partName = sp[1];

            if (DeviceConfig == null || DeviceConfig.Parts == null)
                return null;
            var findPartIndex =
                DeviceConfig.Parts.ToList().FindIndex(f => f.ProcessNo == no && f.Name == partName);
            if (findPartIndex == -1)
                return null;
            var findPart = DeviceConfig.Parts.ToList()[findPartIndex];

            if (!string.IsNullOrEmpty(findPart.ControllerName) &&
                !string.IsNullOrEmpty(findPart.ControllerField))
                return GetFieldType(findPart.ControllerField);

            return null;
        }

        public void ChangeStringToAction(
            string codeStr, ref List<Action> actions)
        {
            var strSplit = codeStr.StrTrim().GetStrsSplitByValue(";", true);

            foreach (var code in strSplit)
            {
                if (code.Contains(".Method.")) // 执行方法
                {
                    var sp = code.GetStrsSplitByValue(".Method.", true);

                    if (sp != null && sp.Length == 2)
                    {
                        var controllerName = sp[0];

                        if (!string.IsNullOrEmpty(controllerName) &&
                            ControllerList.ContainsKey(controllerName))
                        {
                            var controller = ControllerList[controllerName];

                            if (controller != null)
                            {
                                var methodName = sp[1].Substring(0, sp[1].IndexOf('('));
                                var strParas = sp[1].Substring(sp[1].IndexOf('(') + 1,
                                    sp[1].IndexOf(')') - sp[1].IndexOf('(') - 1);

                                if (!string.IsNullOrEmpty(strParas))
                                {
                                    var paraSp = strParas.GetStrsSplitByValue(",", true);

                                    var methodInfos =
                                        controller.GetType()
                                            .GetMethods()
                                            .ToList()
                                            .FindAll(
                                                f => f.GetParameters().Length == paraSp.Length && f.Name == methodName);

                                    if (!methodInfos.Any())
                                        continue;

                                    var method = methodInfos[0];
                                    var needParaMeters = method.GetParameters().ToList();
                                    var lstParaObjs = new List<object>();

                                    for (var i = 0; i < needParaMeters.Count; i++)
                                    {
                                        var p = paraSp[i];
                                        var paramterType = needParaMeters[i].ParameterType;

                                        object value;
                                        if (p.Contains(".Field."))
                                            value = GetFieldValue(p).Invoke();
                                        else if (p.Contains(".Part.") && PartGetValueList.ContainsKey(p))
                                            value = PartGetValueList[p].Invoke();
                                        else
                                            value = p;

                                        if (value == null)
                                            continue;
                                        try
                                        {
                                            lstParaObjs.Add(Convert.ChangeType(value, paramterType));
                                        }
                                        catch (Exception)
                                        {
                                            // ignored
                                        }
                                    }

                                    if (lstParaObjs.Count == needParaMeters.Count)
                                        actions.Add(() => { method.Invoke(controller, lstParaObjs.ToArray()); });
                                }
                                else
                                {
                                    var methodInfos =
                                        controller.GetType()
                                            .GetMethods()
                                            .ToList()
                                            .FindAll(f => f.GetParameters().Length == 0 && f.Name == methodName);

                                    if (methodInfos.Any())
                                        actions.Add(() => { methodInfos[0].Invoke(controller, null); });
                                }
                            }
                        }
                    }
                }
                else if (code.Contains("="))
                {
                    var sp = code.GetStrsSplitByValue("=", true);

                    if (sp != null && sp.Length == 2)
                    {
                        var left = sp[0];
                        var right = sp[1];

                        object rightValue;

                        if (right.Contains(".Field."))
                            rightValue = GetFieldValue(right).Invoke();
                        else if (right.Contains(".Part."))
                            rightValue = GetPartValue(right).Invoke();
                        else
                            rightValue = right;

                        if (left.Contains(".Field."))
                        {
                            var leftType = GetFieldType(left);

                            if (leftType != null && leftType == typeof(bool))
                            {
                                if (rightValue != null && rightValue.ToString() == 1.ToString())
                                    rightValue = bool.TrueString;
                                else if (rightValue != null && rightValue.ToString() == 1.ToString())
                                    rightValue = bool.FalseString;
                            }

                            actions.Add(() => { SetFieldValue().Invoke(left, rightValue); });
                        }
                        else if (left.Contains(".Part."))
                        {
                            var leftType = GetPartType(left);

                            if (leftType != null && leftType == typeof(bool))
                            {
                                if (rightValue != null && rightValue.ToString() == 1.ToString())
                                    rightValue = bool.TrueString;
                                else if (rightValue != null && rightValue.ToString() == 1.ToString())
                                    rightValue = bool.FalseString;
                            }

                            actions.Add(() => { SetPartValue().Invoke(left, rightValue); });
                        }
                    }
                }
                else if (code.Contains("++")) // 检测程序.Field.Bint1++
                {
                    var sp = code.GetStrsSplitByValue("++", true);

                    if (sp != null && sp.Length == 1)
                    {
                        var left = sp[0];

                        if (left.Contains(".Field."))
                        {
                            actions.Add(() =>
                            {
                                var leftValue = GetFieldValue(left).Invoke();
                                if (leftValue == null)
                                    return;

                                var lt = leftValue.GetType();

                                if (lt == typeof(int))
                                    SetFieldValue().Invoke(left, Convert.ToInt32(leftValue) + 1);
                                else if (lt == typeof(double))
                                    SetFieldValue().Invoke(left, Convert.ToDouble(leftValue) + 1);
                                else if (lt == typeof(float))
                                    SetFieldValue().Invoke(left, Convert.ToSingle(leftValue) + 1);
                            });
                        }
                        else if (left.Contains(".Part."))
                        {
                            actions.Add(() =>
                            {
                                var leftValue = GetParaValue(left).Invoke();
                                if (leftValue == null)
                                    return;

                                var lt = leftValue.GetType();

                                if (lt == typeof(int))
                                    SetPartValue().Invoke(left, Convert.ToInt32(leftValue) + 1);
                                else if (lt == typeof(double))
                                    SetPartValue().Invoke(left, Convert.ToDouble(leftValue) + 1);
                                else if (lt == typeof(float))
                                    SetPartValue().Invoke(left, Convert.ToSingle(leftValue) + 1);
                            });
                        }
                    }
                }
                else if (code.Contains("--")) // 检测程序.Field.Bint1--
                {
                    var sp = code.GetStrsSplitByValue("--", true);
                    if (sp != null && sp.Length == 1)
                    {
                        var left = sp[0];

                        if (left.Contains(".Field."))
                        {
                            actions.Add(() =>
                            {
                                var leftValue = GetFieldValue(left).Invoke();

                                if (leftValue != null)
                                {
                                    var lt = leftValue.GetType();

                                    if (lt == typeof(int))
                                        SetFieldValue().Invoke(left, Convert.ToInt32(leftValue) - 1);
                                    else if (lt == typeof(double))
                                        SetFieldValue().Invoke(left, Convert.ToDouble(leftValue) - 1);
                                    else if (lt == typeof(float))
                                        SetFieldValue().Invoke(left, Convert.ToSingle(leftValue) - 1);
                                }
                            });
                        }
                        else if (left.Contains(".Part."))
                        {
                            actions.Add(() =>
                            {
                                var leftValue = GetParaValue(left).Invoke();

                                if (leftValue != null)
                                {
                                    var lt = leftValue.GetType();

                                    if (lt == typeof(int))
                                        SetPartValue().Invoke(left, Convert.ToInt32(leftValue) - 1);
                                    else if (lt == typeof(double))
                                        SetPartValue().Invoke(left, Convert.ToDouble(leftValue) - 1);
                                    else if (lt == typeof(float))
                                        SetPartValue().Invoke(left, Convert.ToSingle(leftValue) - 1);
                                }
                            });
                        }
                    }
                }
                else if (code.Contains("+="))
                {

                }
                else if (code.Contains("-="))
                {

                }
                else if (code.Contains("*="))
                {

                }
                else if (code.Contains("/="))
                {

                }
            }
        }

        public void ChangeStringToFunc(
            StateMachineClass.Workstation ws,
            string codeStr,
            out Func<StateMachineClass.Condition.ConditionFuncStruct> funcs)
        {
            var strSplit =
                codeStr.StrTrim().GetStrsSplitByValues(new[] { "&&", "||" }, true).ToList();

            var codeList = new List<string>();

            for (var i = 0; i < strSplit.Count; i++)
            {
                var c = strSplit[i];

                if (!string.IsNullOrEmpty(c) && (c.EndsWith("&&") || c.EndsWith("||")))
                {
                    var connectValue = c.Substring(c.Length - 2, 2);
                    var code = c.Substring(0, c.Length - 2);

                    if (code.Contains("="))
                    {
                        var connectLeft = string.Empty;
                        var connectRight = string.Empty;
                    }
                }
            }

            funcs = () =>
                new StateMachineClass.Condition.ConditionFuncStruct
                {
                    CompareResult = true,
                    CompareNote = string.Empty
                };
        }

        public class StateMachineAction
        {
            public string WorkstationName { get; private set; }
            public string ParentName { get; private set; }
            public Action Action { get; set; }
            public StateMachineActionType ActiontType { get; private set; }

            public StateMachineAction(
                string workstationName, string parentName,
                Action action, StateMachineActionType actionType)
            {
                WorkstationName = workstationName;
                ParentName = parentName;
                Action = action;
                ActiontType = actionType;
            }
        }

        public enum StateMachineActionType
        {
            StateEntry,
            StateDuring,
            ConditionExit
        }
    }
}
