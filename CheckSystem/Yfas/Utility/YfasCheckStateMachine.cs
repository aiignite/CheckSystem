using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace CheckSystem.Yfas.Utility
{
    public class YfasCheckStateMachine
    {
        public delegate void CheckDataViewEventHandle(CheckDataView value);
        public static event CheckDataViewEventHandle PushCheckDataView;

        public delegate void PushEndEventHandle();
        public static event PushEndEventHandle PushEnd;

        public delegate void PushWaitInfoEventHandle(bool isShow, string value = "");
        public static event PushWaitInfoEventHandle PushWaitInfo;

        public bool IsByPass = true;

        public YfasCheckStateMachine(TreeNode node)
        {
            YfasTreeNodes.Clear();

            for (var i = 0; i < node.Nodes.Count; i++)
            {
                var tn = node.Nodes[i];
                TreeviewToYfafTreeNodes((i + 1).ToString(), tn);
            }
        }

        private void TreeviewToYfafTreeNodes(string i, TreeNode treeNode)
        {
            YfasTreeNodes.Add(i, new YfasTreeNode(treeNode.Tag.ToString(), treeNode.ImageIndex, i));

            for (var j = 0; j < treeNode.Nodes.Count; j++)
            {
                var tnSub = treeNode.Nodes[j];
                TreeviewToYfafTreeNodes(i + "." + (j + 1), tnSub);
            }
        }

        private int _productId;
        private int _currentBaseLine;
        //private string _currentLine;
        //private string _currentLineStartWith;
        private int _paraNgCount;
        private static readonly Dictionary<string, YfasTreeNode> YfasTreeNodes = new Dictionary<string, YfasTreeNode>();
        private static readonly Stopwatch Stopwatch = new Stopwatch();

        public void OnBegin(int productId)
        {
            _productId = productId;

            var model = new _3TierBll.YfasProductInfo().GetModel(productId);
            YfasDeviceBase.SetCarModel(model.RowIndex, model.Pos);

            _currentBaseLine = 1;
            //_currentLine = "1";
            //_currentLineStartWith = string.Empty;

            if (!YfasTreeNodes.ContainsKey(_currentBaseLine.ToString()))
            {
                OnEnd();
                return;
            }
            Stopwatch.Start();
            BeginInvoke(_currentBaseLine.ToString());
        }

        public void OnEnd()
        {
            Stopwatch.Stop();
            Console.WriteLine(@"OnEnd");
            if (PushEnd != null)
                PushEnd();
        }

        private static void OnPushDisplay(CheckDataView value)
        {
            if (PushCheckDataView != null)
                PushCheckDataView.Invoke(value);
        }

        private void BeginInvoke(string key)
        {
            var line = YfasTreeNodes.ToList().Find(f => f.Key == key);
            var lineAction = new Action(() => { line.Value.JsonObjInvoke(); });
            lineAction.BeginInvoke(BeginInvokeAsyncCallBack, line.Value);
        }

        private void BeginInvokeAsyncCallBack(IAsyncResult ir)
        {
            if (ir == null)
                return;

            var yfasTreeNode = ir.AsyncState as YfasTreeNode;
            if (yfasTreeNode == null)
                return;

            if (yfasTreeNode.TreeNodeType == YfasTreeNodeType.Para)
            {
                if (yfasTreeNode.IsDetectionHaveNg)
                {
                    _paraNgCount++;
                    RollBackInvoke(yfasTreeNode.NodeIndex);
                }
                else
                    GoToChild(yfasTreeNode.NodeIndex);
            }
            else if (yfasTreeNode.TreeNodeType == YfasTreeNodeType.Func ||
                yfasTreeNode.TreeNodeType == YfasTreeNodeType.Action)
            {
                GoToChild(yfasTreeNode.NodeIndex);
            }
        }

        private void RollBackInvoke(string key)
        {
            //if (YfasTreeNodes[key].TreeNodeType == YfasTreeNodeType.Action)
            //    YfasTreeNodes[key].JsonObjInvoke();

            if (key == _currentBaseLine.ToString())
            {
                _currentBaseLine++;

                if (_paraNgCount > 0)
                {
                    if (IsByPass)
                    {
                        if (YfasTreeNodes.ContainsKey(_currentBaseLine.ToString()))
                            BeginInvoke(key);
                        else
                            OnEnd();
                    }
                    else
                    {
                        OnEnd();
                    }
                }
                else
                {
                    if (YfasTreeNodes.ContainsKey(_currentBaseLine.ToString()))
                        BeginInvoke(key);
                    else
                        OnEnd();
                }
            }
            else
            {
                // 先找平级
                if (_paraNgCount > 0)
                {
                    if (IsByPass)
                    {
                        if (IsFindNext(key))
                        {
                            GoToNext(key);
                        }
                        else
                        {
                            var keySp = key.Split('.');

                            var rollBackKey = string.Empty;
                            for (var i = 0; i < keySp.Length - 1; i++)
                                rollBackKey += keySp[i] + ".";
                            rollBackKey = rollBackKey.TrimEnd('.');

                            var rollBackAction = new Action(() =>
                            {
                                if (YfasTreeNodes[rollBackKey].TreeNodeType == YfasTreeNodeType.Action)
                                    YfasTreeNodes[rollBackKey].JsonObjInvoke();
                            });
                            rollBackAction.BeginInvoke(RoolBackInvokeAsyncCallBack, YfasTreeNodes[rollBackKey]);
                        }
                    }
                    else
                    {
                        var keySp = key.Split('.');

                        var rollBackKey = string.Empty;
                        for (var i = 0; i < keySp.Length - 1; i++)
                            rollBackKey += keySp[i] + ".";
                        rollBackKey = rollBackKey.TrimEnd('.');

                        var rollBackAction = new Action(() =>
                        {
                            if (YfasTreeNodes[rollBackKey].TreeNodeType == YfasTreeNodeType.Action)
                                YfasTreeNodes[rollBackKey].JsonObjInvoke();
                        });
                        rollBackAction.BeginInvoke(RoolBackInvokeAsyncCallBack, YfasTreeNodes[rollBackKey]);
                    }
                }
                else
                {
                    if (IsFindNext(key))
                    {
                        GoToNext(key);
                    }
                    else
                    {
                        var keySp = key.Split('.');

                        var rollBackKey = string.Empty;
                        for (var i = 0; i < keySp.Length - 1; i++)
                            rollBackKey += keySp[i] + ".";
                        rollBackKey = rollBackKey.TrimEnd('.');

                        var rollBackAction = new Action(() =>
                        {
                            if (YfasTreeNodes[rollBackKey].TreeNodeType == YfasTreeNodeType.Action)
                                YfasTreeNodes[rollBackKey].JsonObjInvoke();
                        });
                        rollBackAction.BeginInvoke(RoolBackInvokeAsyncCallBack, YfasTreeNodes[rollBackKey]);
                    }
                }
            }
        }

        private void RoolBackInvokeAsyncCallBack(IAsyncResult ir)
        {
            if (ir == null)
                return;

            var yfasTreeNode = ir.AsyncState as YfasTreeNode;
            if (yfasTreeNode == null)
                return;

            if (yfasTreeNode.NodeIndex == _currentBaseLine.ToString())
            {
                _currentBaseLine++;

                if (_paraNgCount > 0)
                {
                    if (IsByPass)
                    {
                        if (YfasTreeNodes.ContainsKey(_currentBaseLine.ToString()))
                            BeginInvoke(_currentBaseLine.ToString());
                        else
                            OnEnd();
                    }
                    else
                    {
                        OnEnd();
                    }
                }
                else
                {
                    if (YfasTreeNodes.ContainsKey(_currentBaseLine.ToString()))
                        BeginInvoke(_currentBaseLine.ToString());
                    else
                        OnEnd();
                }
            }
            else
            {
                RollBackInvoke(yfasTreeNode.NodeIndex);
            }
        }

        private void GoToNext(string nodeIndex)
        {
            var nodeIndexSp = nodeIndex.Split('.');

            var nextIndex = string.Empty;
            nodeIndexSp[nodeIndexSp.Length - 1] = (int.Parse(nodeIndexSp[nodeIndexSp.Length - 1]) + 1).ToString();
            nextIndex = nodeIndexSp.Aggregate(nextIndex, (current, t) => current + (t + "."));
            nextIndex = nextIndex.TrimEnd('.');
            BeginInvoke(nextIndex);
        }

        public bool IsFindNext(string nodeIndex)
        {
            var nodeIndexSp = nodeIndex.Split('.');

            var nextIndex = string.Empty;
            nodeIndexSp[nodeIndexSp.Length - 1] = (int.Parse(nodeIndexSp[nodeIndexSp.Length - 1]) + 1).ToString();
            nextIndex = nodeIndexSp.Aggregate(nextIndex, (current, t) => current + (t + "."));
            nextIndex = nextIndex.TrimEnd('.');

            return YfasTreeNodes.Keys.ToList().FindAll(f => f == nextIndex).Any();
        }

        private void GoToChild(string key)
        {
            if (IsFindChild(key + "."))
                BeginInvoke(key + ".1");
            else
            {
                RollBackInvoke(key);
            }
        }

        private bool IsFindChild(string startWith)
        {
            var lvl = startWith.ToCharArray().ToList().FindAll(c => c.ToString() == ".").Count + 1;
            return YfasTreeNodes.Keys.ToList().FindAll(f => f.StartsWith(startWith) && f.Split('.').Length == lvl).Any();
        }

        public class YfasTreeNode
        {
            public int BaseIndex { get; set; }
            public int Level { get; set; }
            public string TimeTickets { get; set; }
            public string NodeIndex { get; set; }
            public YfasTreeNodeType TreeNodeType { get; set; }
            public object JsonObj { get; set; }
            public bool IsDetectionHaveNg { get; set; }

            public YfasTreeNode(string jsonValue, int type, string nodeIndex)
            {
                NodeIndex = nodeIndex;

                Level = nodeIndex.Split('.').Length;
                BaseIndex = int.Parse(nodeIndex.Split('.')[0]);

                switch (type)
                {
                    case (int)YfasTreeNodeType.Action:
                        TreeNodeType = YfasTreeNodeType.Action;
                        JsonObj = JsonConvert.DeserializeObject<ExcActions>(jsonValue);
                        break;

                    case (int)YfasTreeNodeType.Para:
                        TreeNodeType = YfasTreeNodeType.Para;
                        JsonObj = JsonConvert.DeserializeObject<ExcDetect>(jsonValue);
                        break;

                    case (int)YfasTreeNodeType.Func:
                        TreeNodeType = YfasTreeNodeType.Func;
                        JsonObj = JsonConvert.DeserializeObject<ExcFunc>(jsonValue);
                        break;
                }
            }

            public void JsonObjInvoke()
            {
                if (string.IsNullOrEmpty(TimeTickets))
                {
                    TimeTickets = Stopwatch.ElapsedMilliseconds.ToString();
                }

                switch (TreeNodeType)
                {
                    case YfasTreeNodeType.Action:
                        ((ExcActions)JsonObj).Invoke();
                        return;

                    case YfasTreeNodeType.Para:
                        var lvl1 = YfasTreeNodes[NodeIndex.Split('.')[0]];
                        IsDetectionHaveNg = !((ExcDetect)JsonObj).Invoke((long.Parse(TimeTickets) - long.Parse(lvl1.TimeTickets)).ToString());

                        //IsDetectionHaveNg = !((ExcDetect)JsonObj).Invoke(GetLastTimeTickets(Level).ToString());
                        return;

                    case YfasTreeNodeType.Func:
                        ((ExcFunc)JsonObj).Invoke();
                        return;

                    default:
                        return;
                }
            }

            private long GetLastTimeTickets(int level)
            {
                if (level == 1) // 1、2、3...
                {
                    var lvl1 = YfasTreeNodes[NodeIndex.Split('.')[0]];
                    return long.Parse(TimeTickets) - long.Parse(lvl1.TimeTickets);
                }

                // parent have brother

                var sp = NodeIndex.Split('.').ToList();
                sp.RemoveAt(sp.Count - 1);

                if (sp.Count == 1)
                {
                    var lvl1 = YfasTreeNodes[NodeIndex.Split('.')[0]];
                    return long.Parse(TimeTickets) - long.Parse(lvl1.TimeTickets);
                }

                var pStartWith = string.Empty;
                for (var i = 0; i < level - 2; i++)
                {
                    pStartWith += sp[i] + '.';
                }

                if (YfasTreeNodes.Keys.ToList().FindAll(f => f.Split('.').Length == level - 1 && f.StartsWith(pStartWith)).Count > 1)
                {
                    pStartWith = string.Empty;
                    for (var i = 0; i < level - 1; i++)
                        pStartWith += sp[i] + '.';
                    pStartWith = pStartWith.TrimEnd('.');

                    var p = YfasTreeNodes[pStartWith];
                    if (p.TreeNodeType == YfasTreeNodeType.Action)
                    {
                        return long.Parse(TimeTickets) - long.Parse(p.TimeTickets);
                    }

                    pStartWith = string.Empty;
                    for (var i = 0; i < level; i++)
                        pStartWith += sp[i] + '.';
                    pStartWith = pStartWith.TrimEnd('.');
                    p = YfasTreeNodes[pStartWith];
                    return long.Parse(TimeTickets) - long.Parse(p.TimeTickets);
                }

                return GetLastTimeTickets(level - 1);


                var nodeIndexSp = NodeIndex.Split('.');
                var parent = string.Empty;
                for (int i = 0; i < nodeIndexSp.Length - 1; i++)
                {

                }

                var nextIndex = string.Empty;
                nodeIndexSp[nodeIndexSp.Length - 1] = (int.Parse(nodeIndexSp[nodeIndexSp.Length - 1]) + 1).ToString();
                nextIndex = nodeIndexSp.Aggregate(nextIndex, (current, t) => current + t + ".");
                nextIndex = nextIndex.TrimEnd('.');

                if (YfasTreeNodes.Keys.ToList().FindAll(f => f == nextIndex).Any())
                {

                }

                return 0;
            }
        }

        public class ExcActions
        {
            public PreProgramAction[] Enter { get; set; }
            public PreProgramAction[] Exit { get; set; }

            private bool _isEnter = true;

            public ExcActions(PreProgramAction[] enterActions, PreProgramAction[] exitActions)
            {
                Enter = enterActions;
                Exit = exitActions;
            }

            public void Invoke()
            {
                if (_isEnter)
                {
                    InvokeEnter();
                    _isEnter = false;
                }
                else
                {
                    InvokeExit();
                    _isEnter = true;
                }
            }

            private void InvokeEnter()
            {
                ExecuteAction(Enter);
            }

            private void InvokeExit()
            {
                ExecuteAction(Exit);
            }

            private static void ExecuteAction(PreProgramAction[] actions)
            {
                if (actions == null)
                    return;

                //foreach (var act in actions)
                //{
                //    Console.WriteLine(act.ActionName);
                //}
                //return;

                var tasks = new List<Task>();

                foreach (var t in actions)
                {
                    var acts =
                        new _3TierBll.YfasPreProgramActCmd().GetModelList(string.Format("ActionId = '{0}'", t.Id));

                    foreach (var a in acts)
                    {
                        var controller = YfasDeviceBase.GetController(a.ControllerId);
                        //if (a.ControllerId == 0) // 458 controller
                        //    controller = YfasDeviceBase.ProductController;
                        //else if (a.ControllerId >= 1 && a.ControllerId <= 5) // 56pin
                        //    controller = YfasDeviceBase.YfasIoControllers.Find(f => f.Id == a.ControllerId).SyController;

                        if (controller != null)
                        {
                            switch (a.Type)
                            {
                                #region SetField

                                case (int)ActionCommandType.SetField:
                                    var fieldName = a.TargetName;
                                    var fieldValue = a.TargetParas;

                                    var leftFieldType = controller.GetType().GetField(fieldName).FieldType;
                                    var rightValue = fieldValue;
                                    object leftValue = null;

                                    if (leftFieldType == typeof(bool))
                                        leftValue = rightValue == "1";
                                    else
                                    {
                                        if (leftFieldType == typeof(ushort))
                                            leftValue = Convert.ToUInt16(rightValue);
                                        else if (leftFieldType == typeof(float))
                                            leftValue = Convert.ToSingle(rightValue);
                                        else if (leftFieldType == typeof(double))
                                            leftValue = Convert.ToDouble(rightValue);
                                        else if (leftFieldType == typeof(int))
                                            leftValue = Convert.ToInt32(rightValue);
                                        else if (leftFieldType == typeof(byte))
                                            leftValue = Convert.ToByte(rightValue);
                                        else if (leftFieldType == typeof(string))
                                            leftValue = rightValue;
                                    }

                                    if (leftValue != null)
                                    {
                                        var setFieldTask = new Task(() => { controller.GetType().GetField(fieldName).SetValue(controller, Convert.ChangeType(leftValue, leftFieldType)); });
                                        setFieldTask.ContinueWith(f => { GC.Collect(); });
                                        setFieldTask.Start();
                                        tasks.Add(setFieldTask);
                                    }
                                    break;

                                #endregion

                                #region InvokeMethod

                                case (int)ActionCommandType.InvokeMethod:
                                    var methodName = a.TargetName;
                                    var methodParas = a.TargetParas;
                                    var lstParaObjs = new List<object>();
                                    var getAllMethods = controller.GetType().GetMethods();
                                    var lstParas = methodParas.Split(',').ToList();
                                    var paraCount = lstParas.Count;
                                    if (paraCount == 1 && lstParas[0].Equals(""))
                                        paraCount = 0;
                                    else
                                    {
                                        foreach (var m in getAllMethods)
                                        {
                                            if (m.Name == methodName && m.GetParameters().Length == paraCount)
                                            {
                                                var methodPara = m.GetParameters();
                                                lstParaObjs.AddRange(methodPara.Select((tt, i) => Convert.ChangeType(lstParas[i], tt.ParameterType)));
                                            }
                                        }
                                    }

                                    var firstOrDefault = getAllMethods.Where(m => m.Name == methodName && m.GetParameters().Length == paraCount).Select(m1 => (Action)(() => m1.Invoke(controller, lstParaObjs.ToArray()))).FirstOrDefault();

                                    var methodTask = new Task(() =>
                                    {
                                        if (firstOrDefault != null)
                                            firstOrDefault.Invoke();
                                    });
                                    methodTask.ContinueWith(f => { GC.Collect(); });
                                    methodTask.Start();
                                    tasks.Add(methodTask);
                                    break;

                                #endregion

                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }
                    }
                }

                Task.WaitAll(tasks.ToArray());
            }
        }

        public class ExcFunc
        {
            public PreProgramFunc Func { get; set; }

            public ExcFunc(PreProgramFunc func)
            {
                Func = func;
            }

            private bool _isTimeOut;

            public bool Invoke()
            {
                Console.WriteLine(Func.FuncName);

                if (Func.TimeOut > 0)
                {
                    var timer = new System.Timers.Timer { Interval = Func.TimeOut };
                    timer.Elapsed += timer_Elapsed;
                    timer.Start();
                }

                if (PushWaitInfo != null)
                    PushWaitInfo.Invoke(true, "等待：" + Func.FuncName);

                while (true)
                {
                    if (Func.TimeOut > 0)
                    {
                        if (_isTimeOut || Judge())
                        {
                            if (PushWaitInfo != null)
                                PushWaitInfo.Invoke(false);
                            return true;
                        }
                    }
                    else
                    {
                        if (Judge())
                        {
                            if (PushWaitInfo != null)
                                PushWaitInfo.Invoke(false);
                            return true;
                        }
                    }
                }
            }

            private bool Judge()
            {
                var bll = new _3TierBll.YfasPreProgramFuncCmd();
                var models = bll.GetModelList("FuncId= '" + Func.Id + "'");

                var matchCount = 0;
                foreach (var m in models)
                {
                    Debug.Assert(m.ControllerId != null, "m.ControllerId != null");
                    var controller = YfasDeviceBase.GetController((int)m.ControllerId);
                    var field = controller.GetType().GetField(m.ToCompareName);
                    var leftValue = field.GetValue(controller) == null ? string.Empty : field.GetValue(controller).ToString();
                    var rightValue = m.ToCompareValue;

                    switch (m.Type)
                    {
                        case (int)FuncCommandType.Equal: // ==
                            // bool特殊处理
                            if (field.FieldType == typeof(bool))
                            {
                                rightValue = rightValue == "1" ? bool.TrueString.ToUpper() : bool.FalseString.ToUpper();
                                leftValue = leftValue.ToUpper();
                            }
                            else if (field.FieldType == typeof(bool?))
                            {
                                rightValue = rightValue == "1" ? bool.TrueString.ToUpper() : bool.FalseString.ToUpper();
                                leftValue = leftValue.ToUpper();
                            }

                            if (
                                string.Equals(leftValue, rightValue))
                            {
                                matchCount++;
                            }
                            break;

                        case (int)FuncCommandType.GreatthanOr: // >
                            if (double.Parse(leftValue) > double.Parse(rightValue))
                            {
                                matchCount++;
                            }
                            break;

                        case (int)FuncCommandType.GreatthanOrEqual: // >=
                            if (double.Parse(leftValue) >= double.Parse(rightValue))
                            {
                                matchCount++;
                            }
                            break;

                        case (int)FuncCommandType.Lessthan: // <
                            if (double.Parse(leftValue) < double.Parse(rightValue))
                            {
                                matchCount++;
                            }
                            break;

                        case (int)FuncCommandType.LessthanOrEqual: // <=
                            if (double.Parse(leftValue) <= double.Parse(rightValue))
                            {
                                matchCount++;
                            }
                            break;

                        case (int)FuncCommandType.Unequal: // !=
                            if (!string.Equals(leftValue, rightValue))
                            {
                                matchCount++;
                            }
                            break;

                        default:
                            return false;
                    }
                }

                return matchCount == models.Count;
            }

            private void timer_Elapsed(
                object sender, System.Timers.ElapsedEventArgs e)
            {
                var timer = sender as System.Timers.Timer;
                if (timer == null)
                    return;
                _isTimeOut = true;
                timer.Stop();
            }
        }

        public class ExcDetect
        {
            public int ProductId { get; set; }
            public _3TierModel.YfasDetectionBase[] ProductDetections { get; set; }

            public bool Invoke(string timeTickets)
            {
                //Thread.Sleep(5000);
                var ng = 0;
                foreach (var p in ProductDetections)
                {
                    if (p.DetectionName.Contains("ECU"))
                    {

                    }
                    var bll = new _3TierBll.YfasProductParas();
                    var model = bll.GetModelList(string.Format("ProductId = '{0}' and DetectionId = '{1}'", ProductId, p.id))[0];

                    Debug.Assert(model.BindControllerId != null, "model.BindControllerId != null");
                    Debug.Assert(model != null, "model != null");
                    var controller = YfasDeviceBase.GetController((int)model.BindControllerId);

                    try
                    {
                        Debug.Assert(controller != null, "controller != null");
                        var fieldValue = controller.GetType()
                            .GetField(model.BindControllerFieldName)
                            .GetValue(controller) == null
                            ? string.Empty
                            : controller.GetType()
                                .GetField(model.BindControllerFieldName)
                                .GetValue(controller).ToString();


                        var newData = new CheckDataView(p.DetectionName) { Data = fieldValue, CostTime = timeTickets + "ms" };

                        if (string.Equals(model.DateType, "double", StringComparison.CurrentCultureIgnoreCase))
                        {

                            if (string.IsNullOrEmpty(fieldValue))
                            {
                                ng++;
                                newData.Result = "NG";
                            }
                            else
                            {
                                var doubleOffset = CalculatedWithOffset(fieldValue, model.Offset)();
                                fieldValue = doubleOffset.ToString(CultureInfo.InvariantCulture);
                                newData.Data = fieldValue;

                                if (double.Parse(fieldValue) < double.Parse(model.Min) ||
                                    double.Parse(fieldValue) > double.Parse(model.Max))
                                {
                                    ng++;
                                    newData.Result = "NG";
                                }
                                else
                                {
                                    newData.Result = "OK";
                                }
                            }
                        }
                        else if (string.Equals(model.DateType, "string", StringComparison.CurrentCultureIgnoreCase))
                        {
                            if (model.OkForamt == "=")
                            {
                                if (fieldValue != model.Value)
                                {
                                    ng++;
                                    newData.Result = "NG";
                                }
                                else
                                {
                                    newData.Result = "OK";
                                }
                            }
                            else if (string.Equals(model.OkForamt, "Like", StringComparison.CurrentCultureIgnoreCase))
                            {
                                string analysisValue;
                                var isOk = StateMachine.ConditionalCodeLine.GetStr(model.Value, fieldValue, out analysisValue);
                                if (!isOk)
                                {
                                    ng++;
                                    newData.Result = "NG";
                                }
                                else
                                {
                                    newData.Result = "OK";
                                }
                            }
                            else if (string.Equals(model.OkForamt, "Contain", StringComparison.CurrentCultureIgnoreCase))
                            {
                                if (!fieldValue.Contains(model.Value))
                                {
                                    ng++;
                                    newData.Result = "NG";
                                }
                                else
                                {
                                    newData.Result = "OK";
                                }
                            }
                        }
                        else if (string.Equals(model.DateType, "uid", StringComparison.CurrentCultureIgnoreCase))
                        {
                            newData.IsUid = true;
                            string analysisValue;
                            var isOk = StateMachine.ConditionalCodeLine.GetStr(model.Value, fieldValue, out analysisValue);
                            if (!isOk)
                            {
                                ng++;
                                newData.Result = "NG";
                            }
                            else
                            {
                                newData.Result = "OK";
                            }
                        }

                        OnPushDisplay(newData);
                        Console.WriteLine(p.DetectionName);
                    }
                    catch (Exception ex)
                    {
                        var newData = new CheckDataView(p.DetectionName)
                        {
                            Data = ex.Message,
                            CostTime = timeTickets + "ms",
                            Result = "NG"
                        };
                        OnPushDisplay(newData);
                    }
                }

                return ng == 0;
                //return true;
            }

            private Func<double> CalculatedWithOffset(string value, string offset)
            {
                return () =>
                {
                    var calculated = new DataTable();
                    var calculateFunc =
                        new Func<string, double>(formula =>
                        {
                            try
                            {
                                return Convert.ToDouble(calculated.Compute(formula, ""));
                            }
                            catch (Exception)
                            {
                                return -9999;
                            }
                        });

                    if (string.IsNullOrEmpty(offset))
                        offset = @"1*X+0";

                    var lstson = offset.Split('+', '-', '*', '/', '(', ')');
                    if (lstson[1].ToUpper() == "X" || lstson.Contains("X"))
                    {
                        offset = offset.Replace("X", value);
                        return calculateFunc.Invoke(offset);
                    }
                    else
                    {
                        foreach (var son in lstson.Where(son => !string.IsNullOrEmpty(son)))
                        {
                            int result;
                            if (int.TryParse(son, out result))
                                continue;

                            var son1 = son;
                            return calculateFunc.Invoke(son1);
                            //offset = (from p in LstParas.Where(p => p.Name == son1 && p.ProcessNo == processNo)
                            //          let pValue = GetFieldValueStr(p.ControllerField)
                            //          let pOffset = p.ControllerFieldOffset
                            //          select CalculatedWithOffset(processNo, pValue, pOffset).Invoke().ToString(CultureInfo.InvariantCulture))
                            //    .Aggregate(offset, (current, xxx) => current.Replace(son1, xxx));
                        }

                        return calculateFunc.Invoke(offset);
                    }
                };
            }
        }

        public enum YfasTreeNodeType
        {
            /// <summary>
            /// 基节点
            /// </summary>
            BaseNode = -1,

            /// <summary>
            /// 动作
            /// </summary>
            Action = 0,

            /// <summary>
            /// 检测项目
            /// </summary>
            Para = 1,

            /// <summary>
            /// 判断
            /// </summary>
            Func = 2
        }

        public enum ActionCommandType
        {
            /// <summary>
            /// 设置字段值
            /// </summary>
            [Description("设置字段值")]
            SetField,

            /// <summary>
            /// 执行方法
            /// </summary>
            [Description("执行方法")]
            InvokeMethod
        }

        public enum FuncCommandType
        {
            /// <summary>
            /// ==
            /// </summary>
            [Description("==")]
            Equal,

            /// <summary>
            /// !=
            /// </summary>
            [Description("!=")]
            Unequal,

            /// <summary>
            /// >=
            /// </summary>
            [Description(">=")]
            GreatthanOrEqual,

            /// <summary>
            /// >
            /// </summary>
            [Description(">")]
            GreatthanOr,

            /// <summary>
            /// <=
            /// </summary>
            [Description("<=")]
            LessthanOrEqual,

            /// <summary>
            /// >=
            /// </summary>
            [Description("<")]
            Lessthan
        }

        public class PreProgramActionCmd
        {
            public int Id { get; set; }
            public int ActionId { get; set; }
            public ActionCommandType Type { get; set; }
            public int ControllerId { get; set; }
            public string TargetName { get; set; }
            public string TargetParas { get; set; }
            //public int IsDelete { get; set; }
        }

        public class PreProgramAction
        {
            public int Id { get; set; }
            public string ActionName { get; set; }
            //public int IsDelete { get; set; }
        }

        public class PrePrograFuncCmd
        {
            public int Id { get; set; }
            public int FuncId { get; set; }
            public FuncCommandType Type { get; set; }
            public int ControllerId { get; set; }
            public string ToCompareName { get; set; }
            public string ToCompareValue { get; set; }
            //public int IsDelete { get; set; }
        }

        public class PreProgramFunc
        {
            public int Id { get; set; }
            public string FuncName { get; set; }
            public int TimeOut { get; set; }
            //public int IsDelete { get; set; }
        }

        public class CheckDataView
        {
            public string Name { get; set; }
            public string IsNotNa { get; set; }
            public string Range { get; set; }
            public string Unit { get; set; }
            public string Data { get; set; }
            public string Result { get; set; }
            public string CostTime { get; set; }

            public bool IsUid;

            public CheckDataView(string name)
            {
                Name = name;
                IsNotNa = "NA";
                //Range = "NA";
                //Unit = "NA";
                //Data = "NA";
                //Result = "NA";
                //CostTime = "NA";
                Range = "/";
                Unit = "/";
                Data = "/";
                Result = "/";
                CostTime = "/";
            }
        }
    }
}
