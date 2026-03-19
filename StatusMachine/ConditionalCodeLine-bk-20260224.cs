using CommonUtility;
using CommonUtility.HikSdk;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Size = OpenCvSharp.Size;

namespace StateMachine
{
    public sealed class ConditionalCodeLine : CodeLine
    {
        public delegate void PushCheckDataEventHandle(CheckData checkData);
        public static event PushCheckDataEventHandle PushCheckData;

        public bool ComparativeResult { get; private set; }
        public List<string> ComparativeResultStr { get; private set; }
        private Action ComparativeAction { get; set; }
        private List<DeviceConfigPara> LstParas { get; set; }

        public ConditionalCodeLine(
            string codeLineValue,
            List<object> controllersList,
            IEnumerable<DeviceConfigPara> paras,
            object obj,
            DeviceConfig deviceConfig)
            : base(controllersList, deviceConfig)
        {
            CodeStr = codeLineValue;
            ComparativeResultStr = new List<string>();
            var deviceConfigParas = paras as DeviceConfigPara[] ?? paras.ToArray();
            LstParas = deviceConfigParas.ToList();
            ComparativeAction = Analyze(obj);
        }

        private class TempR
        {
            public string R { get; set; }

            public List<Tuple<List<CheckData>, bool>> L = new List<Tuple<List<CheckData>, bool>>();
        }

        public override Action Analyze(object obj)
        {
            return () =>
            {
                ComparativeResultStr.Clear();

                var compareTrueFalseList = new StringBuilder();

                // &&优先级高于||
                var lstCodeLineOr =
                    new List<string>(CodeStr.Split(new[] { "||" },
                        StringSplitOptions.RemoveEmptyEntries));

                if (lstCodeLineOr.Any(f => f.ToLower().Contains(".para.") && !f.ToLower().Contains(".field.") && !f.ToLower().Contains(".part.")))
                {
                    foreach (var t in lstCodeLineOr)
                    {
                        var lstCodeLineAnd =
                            new List<string>(t.Split(new[] { "&&" }, StringSplitOptions.RemoveEmptyEntries));

                        var temp = new StringBuilder();

                        if (lstCodeLineAnd.Count == 0)
                            temp.Append(false + "&&");

                        //var task = new Task<TempR>[lstCodeLineAnd.Count];

                        var listTempR = new List<TempR>();

                        for (var i = 0; i < lstCodeLineAnd.Count; i++)
                        {
                            var cts1 = HighPrecisionTimer.GetTimestamp();

                            var codeLineAnd = lstCodeLineAnd[i];

                            var returnR = new TempR();

                            var left = codeLineAnd.GetStrFromLeftSign();
                            var right = codeLineAnd.GetStrFromRightSign();
                            //var leftSplitByDot = left.GetStrsSplitByDot();

                            var returnTempStr = string.Empty;

                            if (left.Contains(".Para.") &&
                                (right.ToUpper().Equals("TRUE") ||
                                right.ToUpper().Equals("FALSE") ||
                                right.Equals("1") ||
                                right.Equals("0"))) // 1. 和参数作比较
                            {
                                var paraName = left.GetStrsSplitByValue(".Para.")[1];
                                var processNo = left.GetStrsSplitByValue(".Para")[0];
                                bool isAssertTrue;
                                if (right.ToUpper().Equals("TRUE") ||
                                    right.ToUpper().Equals("FALSE"))
                                    isAssertTrue = bool.Parse(right.ToUpper());
                                else
                                    isAssertTrue = right.Equals("1");

                                List<CheckData> c1;
                                bool isP1;
                                var compareResult =
                                    ComparedWithPara(processNo, paraName, isAssertTrue, obj as Action<CheckData, bool>, out c1, out isP1).ToString();

                                returnR.L.Add(new Tuple<List<CheckData>, bool>(c1, isP1));

                                //temp.Append(compareResult + "&&");
                                returnTempStr = compareResult + "&&";

                                if (compareResult != true.ToString())
                                {
                                    List<CheckData> c2;
                                    bool isP2;
                                    ComparedWithPara(processNo, paraName, !isAssertTrue, obj as Action<CheckData, bool>, out c2, out isP2);
                                    returnR.L.Add(new Tuple<List<CheckData>, bool>(c2, isP2));
                                }
                            }
                            else if (left.Contains(".Field.")) // 2. 和变量作比较
                            {
                                var compareResult = ComparedWithField(left, right,
                                    codeLineAnd.Replace(left, string.Empty).Replace(right, string.Empty));
                                //temp.Append(compareResult + "&&");
                                returnTempStr = compareResult + "&&";
                            }
                            else if (left.Contains(".Part."))  // 3. 和part作比较
                            {
                                var part = DeviceConfig.Parts.ToList().Find(f => f.Name == left.GetStrsSplitByValue(".Part.")[1]);
                                var leftField = part.ControllerField;

                                var compareResult = ComparedWithField(leftField, right,
                                   codeLineAnd.Replace(left, string.Empty).Replace(right, string.Empty));
                                //temp.Append(compareResult + "&&");
                                returnTempStr = compareResult + "&&";
                            }

                            returnR.R = returnTempStr;
                            listTempR.Add(returnR);

                            var cts2 = HighPrecisionTimer.GetTimestamp();
                            //Console.WriteLine("处理一个codeline para耗时: {0}ms", HighPrecisionTimer.GetTimestampIntervalMs(cts1, cts2));

                            //task[i] = Task.Run(() =>
                            //{
                            //    var returnR = new TempR();

                            //    var left = codeLineAnd.GetStrFromLeftSign();
                            //    var right = codeLineAnd.GetStrFromRightSign();
                            //    //var leftSplitByDot = left.GetStrsSplitByDot();

                            //    var returnTempStr = string.Empty;

                            //    if (left.Contains(".Para.") &&
                            //        (right.ToUpper().Equals("TRUE") ||
                            //        right.ToUpper().Equals("FALSE") ||
                            //        right.Equals("1") ||
                            //        right.Equals("0"))) // 1. 和参数作比较
                            //    {
                            //        var paraName = left.GetStrsSplitByValue(".Para.")[1];
                            //        var processNo = left.GetStrsSplitByValue(".Para")[0];
                            //        bool isAssertTrue;
                            //        if (right.ToUpper().Equals("TRUE") ||
                            //            right.ToUpper().Equals("FALSE"))
                            //            isAssertTrue = bool.Parse(right.ToUpper());
                            //        else
                            //            isAssertTrue = right.Equals("1");

                            //        List<CheckData> c1;
                            //        bool isP1;
                            //        var compareResult =
                            //            ComparedWithPara(processNo, paraName, isAssertTrue, obj as Action<CheckData, bool>, out c1, out isP1).ToString();

                            //        returnR.L.Add(new Tuple<List<CheckData>, bool>(c1, isP1));

                            //        //temp.Append(compareResult + "&&");
                            //        returnTempStr = compareResult + "&&";

                            //        if (compareResult != true.ToString())
                            //        {
                            //            List<CheckData> c2;
                            //            bool isP2;
                            //            ComparedWithPara(processNo, paraName, !isAssertTrue, obj as Action<CheckData, bool>, out c2, out isP2);
                            //            returnR.L.Add(new Tuple<List<CheckData>, bool>(c2, isP2));
                            //        }
                            //    }
                            //    else if (left.Contains(".Field.")) // 2. 和变量作比较
                            //    {
                            //        var compareResult = ComparedWithField(left, right,
                            //            codeLineAnd.Replace(left, string.Empty).Replace(right, string.Empty));
                            //        //temp.Append(compareResult + "&&");
                            //        returnTempStr = compareResult + "&&";
                            //    }
                            //    else if (left.Contains(".Part."))  // 3. 和part作比较
                            //    {
                            //        var part = DeviceConfig.Parts.ToList().Find(f => f.Name == left.GetStrsSplitByValue(".Part.")[1]);
                            //        var leftField = part.ControllerField;

                            //        var compareResult = ComparedWithField(leftField, right,
                            //           codeLineAnd.Replace(left, string.Empty).Replace(right, string.Empty));
                            //        //temp.Append(compareResult + "&&");
                            //        returnTempStr = compareResult + "&&";
                            //    }

                            //    returnR.R = returnTempStr;

                            //    return returnR;
                            //});
                        }

                        //Task.WaitAll(task);

                        //foreach (var t1 in task)
                        //{
                        //    foreach (var item in t1.Result.L)
                        //    {
                        //        var ckd = item.Item1;
                        //        var ispush = item.Item2;
                        //        if (ckd != null && ckd.Any())
                        //        {
                        //            foreach (var tc in ckd)
                        //                (obj as Action<CheckData, bool>).Invoke(tc, ispush);

                        //        }
                        //    }

                        //    temp.Append(t1.Result.R);
                        //}

                        var tcts1 = HighPrecisionTimer.GetTimestamp();

                        foreach (var t1 in listTempR)
                        {
                            foreach (var item in t1.L)
                            {
                                var ckd = item.Item1;
                                var ispush = item.Item2;
                                if (ckd != null && ckd.Any())
                                {
                                    foreach (var tc in ckd)
                                        (obj as Action<CheckData, bool>).Invoke(tc, ispush);

                                }
                            }

                            temp.Append(t1.R);
                        }

                        compareTrueFalseList.Append(temp.ToString().Contains(false.ToString())
                            ? false.ToString()
                            : true.ToString());
                        compareTrueFalseList.Append("||");

                        var tcts2 = HighPrecisionTimer.GetTimestamp();
                        //Console.WriteLine("push一个codeline para耗时: {0}ms", HighPrecisionTimer.GetTimestampIntervalMs(tcts1, tcts2));
                    }
                }
                else
                {
                    foreach (var t in lstCodeLineOr)
                    {
                        var lstCodeLineAnd =
                            new List<string>(t.Split(new[] { "&&" }, StringSplitOptions.RemoveEmptyEntries));

                        var temp = new StringBuilder();

                        if (lstCodeLineAnd.Count == 0)
                            temp.Append(false + "&&");

                        foreach (var codeLineAnd in lstCodeLineAnd)
                        {
                            var left = codeLineAnd.GetStrFromLeftSign();
                            var right = codeLineAnd.GetStrFromRightSign();
                            //var leftSplitByDot = left.GetStrsSplitByDot();

                            if (left.Contains(".Para.") &&
                                (right.ToUpper().Equals("TRUE") ||
                                right.ToUpper().Equals("FALSE") ||
                                right.Equals("1") ||
                                right.Equals("0"))) // 1. 和参数作比较
                            {
                                var paraName = left.GetStrsSplitByValue(".Para.")[1];
                                var processNo = left.GetStrsSplitByValue(".Para")[0];
                                bool isAssertTrue;
                                if (right.ToUpper().Equals("TRUE") ||
                                    right.ToUpper().Equals("FALSE"))
                                    isAssertTrue = bool.Parse(right.ToUpper());
                                else
                                    isAssertTrue = right.Equals("1");

                                List<CheckData> c1;
                                bool isP1;
                                var compareResult =
                                    ComparedWithPara(processNo, paraName, isAssertTrue, obj as Action<CheckData, bool>, out c1, out isP1).ToString();

                                if (c1 != null && c1.Any())
                                {
                                    foreach (var tc1 in c1)
                                    {
                                        (obj as Action<CheckData, bool>).Invoke(tc1, isP1);
                                    }
                                }

                                temp.Append(compareResult + "&&");

                                if (compareResult != true.ToString())
                                {
                                    List<CheckData> c2;
                                    bool isP2;
                                    ComparedWithPara(processNo, paraName, !isAssertTrue, obj as Action<CheckData, bool>, out c2, out isP2);
                                    if (c2 != null && c2.Any())
                                    {
                                        foreach (var tc2 in c2)
                                        {
                                            (obj as Action<CheckData, bool>).Invoke(tc2, isP2);
                                        }
                                    }
                                }
                            }
                            else if (left.Contains(".Field.")) // 2. 和变量作比较
                            {
                                var compareResult = ComparedWithField(left, right,
                                    codeLineAnd.Replace(left, string.Empty).Replace(right, string.Empty));
                                temp.Append(compareResult + "&&");
                            }
                            else if (left.Contains(".Part."))  // 3. 和part作比较
                            {
                                var part = DeviceConfig.Parts.ToList().Find(f => f.Name == left.GetStrsSplitByValue(".Part.")[1]);
                                var leftField = part.ControllerField;

                                var compareResult = ComparedWithField(leftField, right,
                                   codeLineAnd.Replace(left, string.Empty).Replace(right, string.Empty));
                                temp.Append(compareResult + "&&");
                            }
                        }

                        compareTrueFalseList.Append(temp.ToString().Contains(false.ToString())
                            ? false.ToString()
                            : true.ToString());
                        compareTrueFalseList.Append("||");
                    }
                }

                ComparativeResult = compareTrueFalseList.ToString().Contains(true.ToString());
            };
        }

        public override Action Analyze()
        {
            throw new NotImplementedException();
        }

        private readonly List<RoiBuffer> _roiBuffers = new List<RoiBuffer>();

        /// <summary>
        /// 和工序参数作比较
        /// </summary>
        /// <returns></returns>
        private bool ComparedWithPara(
            string processNo, string paraName, bool isAssertTrue, Action<CheckData, bool> actionWithPara, out List<CheckData> ckd, out bool isPush)
        {
            ckd = new List<CheckData>();
            isPush = false;

            var expectedPara = LstParas.Find(f => f.Name.Equals(paraName) && f.ProcessNo.Equals(processNo));
            if (expectedPara == null)
                throw new Exception();
            else if (expectedPara.DataType.ToLower() != "roi" && expectedPara.DataType.ToLower() != "roi[]" && expectedPara.DataType.ToLower() != "platebarcode")
            {
                if (string.IsNullOrEmpty(expectedPara.Value))
                    expectedPara.Value = string.Empty;
                var expectedUnit = string.Empty;
                if (!string.IsNullOrEmpty(expectedPara.Unit))
                    expectedUnit = expectedPara.Unit;

                var comparedFieldValue = GetFieldValueStr(expectedPara.ControllerField);
                var expectedType = expectedPara.DataType.ToLower();
                var min = string.IsNullOrEmpty(expectedPara.Min)
                                    ? string.Empty
                                    : expectedPara.Min;
                var max = string.IsNullOrEmpty(expectedPara.Max)
                    ? expectedPara.Value
                    : expectedPara.Max;

                var msg = isAssertTrue
                               ? string.Format("判断参数【{0}】应在范围【{1}~{2}】内。", paraName, min, max)
                               : string.Format("判断参数【{0}】应不在范围【{1}~{2}】内。", paraName, min, max);

                //var onInvokeAddMsg =
                //    new Action<bool, string, string>((result, resultstr, unit) =>
                //    {
                //        ComparativeResultStr.Add(msg + "实际值为【" + resultstr + "】，判断结果为【" + result + "】。");

                //        var checkData = new CheckData
                //        {
                //            ProcessNo = processNo,
                //            ParaName = paraName,
                //            Type = expectedType,
                //            Range = string.Format("({0}~{1})", min, max).TrimStart('~'),
                //            Value = resultstr,
                //            Unit = unit,
                //            Format = expectedPara.OkFormat
                //        };
                //        if (isAssertTrue)
                //            checkData.Result = result ? bool.TrueString : bool.FalseString;
                //        else
                //            checkData.Result = result ? bool.FalseString : bool.TrueString;

                //        actionWithPara.Invoke(checkData, true);
                //    });

                var onInvokeAddMsg =
                    new Func<bool, string, string, Tuple<CheckData, bool>>((result, resultstr, unit) =>
                    {
                        ComparativeResultStr.Add(msg + "实际值为【" + resultstr + "】，判断结果为【" + result + "】。");
                        var checkData = new CheckData
                        {
                            ProcessNo = processNo,
                            ParaName = paraName,
                            Type = expectedType,
                            Range = string.Format("({0}~{1})", min, max).TrimStart('~'),
                            Value = resultstr,
                            Unit = unit,
                            Format = expectedPara.OkFormat
                        };
                        if (isAssertTrue)
                            checkData.Result = result ? bool.TrueString : bool.FalseString;
                        else
                            checkData.Result = result ? bool.FalseString : bool.TrueString;

                        //actionWithPara.Invoke(checkData, true);
                        return Tuple.Create<CheckData, bool>(checkData, true);
                    });

                switch (expectedType.ToLower())
                {
                    #region double
                    case "double":
                        var actualValueDouble =
                            CalculatedWithOffset(expectedPara.ProcessNo, comparedFieldValue,
                                string.IsNullOrEmpty(expectedPara.ControllerFieldOffset)
                                    ? "1*X+0"
                                    : expectedPara.ControllerFieldOffset).Invoke();

                        var isInternalOffset = false;

                        //if (actualValueDouble > double.Parse(max) && actualValueDouble < double.Parse(max) * 1.1)
                        //{
                        //    actualValueDouble = actualValueDouble * 0.9;
                        //    isInternalOffset = true;
                        //}
                        //else if (actualValueDouble < double.Parse(min) && actualValueDouble > double.Parse(min) * 0.9)
                        //{
                        //    actualValueDouble = actualValueDouble * 1.1;
                        //    isInternalOffset = true;
                        //}

                        actualValueDouble = Math.Round(actualValueDouble, 4, MidpointRounding.AwayFromZero);

                        if (!double.IsNaN(actualValueDouble))
                        {
                            if (isAssertTrue)
                            {
                                if (actualValueDouble >= double.Parse(min) && actualValueDouble <= double.Parse(max))
                                {
                                    var r1 = onInvokeAddMsg.Invoke(true,
                                         isInternalOffset
                                             ? actualValueDouble.ToString(CultureInfo.InvariantCulture) + "*"
                                             : actualValueDouble.ToString(CultureInfo.InvariantCulture), expectedUnit);
                                    ckd.Add(r1.Item1);
                                    isPush = r1.Item2;
                                    return true;
                                }
                                else
                                {
                                    var r1 = onInvokeAddMsg.Invoke(false, isInternalOffset
                                            ? actualValueDouble.ToString(CultureInfo.InvariantCulture) + "*"
                                            : actualValueDouble.ToString(CultureInfo.InvariantCulture), expectedUnit);

                                    ckd.Add(r1.Item1);
                                    isPush = r1.Item2;
                                }
                            }
                            else
                            {
                                if (actualValueDouble < double.Parse(min) || actualValueDouble > double.Parse(max))
                                {
                                    var r1 = onInvokeAddMsg.Invoke(true, isInternalOffset
                                            ? actualValueDouble.ToString(CultureInfo.InvariantCulture) + "*"
                                            : actualValueDouble.ToString(CultureInfo.InvariantCulture), expectedUnit);
                                    ckd.Add(r1.Item1);
                                    isPush = r1.Item2;
                                    return true;
                                }
                                else
                                {
                                    var r1 = onInvokeAddMsg.Invoke(false, isInternalOffset
                                           ? actualValueDouble.ToString(CultureInfo.InvariantCulture) + "*"
                                           : actualValueDouble.ToString(CultureInfo.InvariantCulture), expectedUnit);

                                    ckd.Add(r1.Item1);
                                    isPush = r1.Item2;
                                }
                            }
                        }
                        break;
                    #endregion

                    #region string
                    case "string":
                        var actualStr = comparedFieldValue;

                        if (expectedPara.OkFormat == "=")
                        {
                            if (isAssertTrue)
                            {
                                if (actualStr.Equals(expectedPara.Value))
                                {
                                    var r1 = onInvokeAddMsg.Invoke(true, actualStr, expectedUnit);
                                    ckd.Add(r1.Item1);
                                    isPush = r1.Item2;
                                    return true;
                                }
                                else
                                {
                                    var r1 = onInvokeAddMsg.Invoke(false, actualStr, expectedUnit);
                                    ckd.Add(r1.Item1);
                                    isPush = r1.Item2;
                                }
                            }
                            else
                            {
                                if (!actualStr.Equals(expectedPara.Value))
                                {
                                    var r1 = onInvokeAddMsg.Invoke(true, actualStr, expectedUnit);
                                    ckd.Add(r1.Item1);
                                    isPush = r1.Item2;
                                    return true;
                                }
                                else
                                {
                                    var r1 = onInvokeAddMsg.Invoke(false, actualStr, expectedUnit);
                                    ckd.Add(r1.Item1);
                                    isPush = r1.Item2;

                                }
                            }
                        }
                        else if (expectedPara.OkFormat == "like")
                        {
                            if (isAssertTrue)
                            {
                                if (actualStr.Contains(expectedPara.Value))
                                {
                                    var r1 = onInvokeAddMsg.Invoke(true, actualStr, expectedUnit);
                                    ckd.Add(r1.Item1);
                                    isPush = r1.Item2;
                                    return true;
                                }
                                else
                                {
                                    var r1 = onInvokeAddMsg.Invoke(false, actualStr, expectedUnit);
                                    ckd.Add(r1.Item1);
                                    isPush = r1.Item2;
                                }
                            }
                            else
                            {
                                if (!actualStr.Contains(expectedPara.Value))
                                {
                                    var r1 = onInvokeAddMsg.Invoke(true, actualStr, expectedUnit);
                                    ckd.Add(r1.Item1);
                                    isPush = r1.Item2;
                                    return true;
                                }
                                else
                                {
                                    var r1 = onInvokeAddMsg.Invoke(false, actualStr, expectedUnit);
                                    ckd.Add(r1.Item1);
                                    isPush = r1.Item2;
                                }
                            }
                        }
                        break;
                    #endregion

                    #region bool
                    case "bool":
                        var actualBool = comparedFieldValue == true.ToString() ? 1.ToString() : 0.ToString();
                        string exopectedBool;

                        if (expectedPara.Value.ToLower() == true.ToString())
                            exopectedBool = 1.ToString();
                        else if (expectedPara.Value.ToLower() == false.ToString())
                            exopectedBool = 0.ToString();
                        else
                            exopectedBool = expectedPara.Value;

                        if (isAssertTrue)
                        {
                            if (actualBool == exopectedBool)
                            {
                                var r1 = onInvokeAddMsg.Invoke(true, actualBool, expectedUnit);
                                ckd.Add(r1.Item1);
                                isPush = r1.Item2;
                                return true;
                            }
                            else
                            {
                                var r1 = onInvokeAddMsg.Invoke(false, actualBool, expectedUnit);
                                ckd.Add(r1.Item1);
                                isPush = r1.Item2;
                            }
                        }
                        else
                        {
                            if (actualBool != exopectedBool)
                            {
                                var r1 = onInvokeAddMsg.Invoke(true, actualBool, expectedUnit);
                                ckd.Add(r1.Item1);
                                isPush = r1.Item2;
                                return true;
                            }
                            else
                            {
                                var r1 = onInvokeAddMsg.Invoke(false, actualBool, expectedUnit);
                                ckd.Add(r1.Item1);
                                isPush = r1.Item2;
                            }
                        }
                        break;
                    #endregion

                    #region vision
                    case "vision":
                        var str = comparedFieldValue;
                        var regCount = new Regex(@"(?<=<Vision>)([\,\.\*\#\$\@\^\w]+)(?=</Vision>)+");
                        var countMatches = regCount.Matches(str);

                        var isGrayInRange = true;
                        if (countMatches.Count == 0)
                        {
                            isGrayInRange = false;
                        }
                        else
                        {
                            foreach (var c in countMatches)
                            {
                                var sp = c.ToString().Split(',');
                                var type = sp[1];

                                double minGray = 0;
                                double maxGray = 0;
                                double actualGray = -9999;

                                if (type.Equals("PolygonContour"))
                                {
                                    minGray = double.Parse(sp[10]);
                                    maxGray = double.Parse(sp[11]);
                                    actualGray = double.Parse(sp[12]);
                                }
                                else if (type.Equals("RectangleContour"))
                                {
                                    minGray = double.Parse(sp[6]);
                                    maxGray = double.Parse(sp[7]);
                                    actualGray = double.Parse(sp[8]);
                                }

                                if (actualGray < minGray || actualGray > maxGray)
                                {
                                    isGrayInRange = false;
                                    break;
                                }
                            }
                        }

                        if (isAssertTrue)
                        {
                            if (isGrayInRange)
                            {
                                var r1 = onInvokeAddMsg.Invoke(true, str, expectedUnit);
                                ckd.Add(r1.Item1);
                                isPush = r1.Item2;
                                return true;
                            }
                            else
                            {
                                var r1 = onInvokeAddMsg.Invoke(true, str, expectedUnit);
                                ckd.Add(r1.Item1);
                                isPush = r1.Item2;
                                return false;
                            }
                        }
                        else
                        {
                            if (!isGrayInRange)
                            {
                                var r1 = onInvokeAddMsg.Invoke(true, str, expectedUnit);
                                ckd.Add(r1.Item1);
                                isPush = r1.Item2;
                                return true;
                            }
                            else
                            {
                                var r1 = onInvokeAddMsg.Invoke(false, str, expectedUnit);
                                ckd.Add(r1.Item1);
                                isPush = r1.Item2;
                            }
                        }
                        break;
                    #endregion

                    #region barcode[]
                    case "barcodegroup":
                        try
                        {
                            //var onInvokeBarcodeGroupAddMsg =
                            //    new Action<bool, string>((result, resultstr) =>
                            //    {
                            //        ComparativeResultStr.Add(msg + "实际值为【" + resultstr + "】，判断结果为【" + result + "】。");

                            //        var checkData = new CheckData
                            //        {
                            //            ProcessNo = processNo,
                            //            ParaName = paraName,
                            //            Type = expectedType,
                            //            Range = expectedPara.Value,
                            //            Value = resultstr,
                            //            Unit = string.Empty,
                            //            Format = expectedPara.OkFormat
                            //        };

                            //        if (isAssertTrue)
                            //        {
                            //            checkData.Result = result ? bool.TrueString : bool.FalseString;
                            //        }
                            //        else
                            //        {
                            //            checkData.Result = result ? bool.FalseString : bool.TrueString;
                            //        }

                            //        actionWithPara.Invoke(checkData, true);
                            //    });

                            var onInvokeBarcodeGroupAddMsg =
                                new Func<bool, string, Tuple<CheckData, bool>>((result, resultstr) =>
                                {
                                    ComparativeResultStr.Add(msg + "实际值为【" + resultstr + "】，判断结果为【" + result + "】。");

                                    var checkData = new CheckData
                                    {
                                        ProcessNo = processNo,
                                        ParaName = paraName,
                                        Type = expectedType,
                                        Range = expectedPara.Value,
                                        Value = resultstr,
                                        Unit = string.Empty,
                                        Format = expectedPara.OkFormat
                                    };

                                    if (isAssertTrue)
                                    {
                                        checkData.Result = result ? bool.TrueString : bool.FalseString;
                                    }
                                    else
                                    {
                                        checkData.Result = result ? bool.FalseString : bool.TrueString;
                                    }

                                    //actionWithPara.Invoke(checkData, true);

                                    return Tuple.Create<CheckData, bool>(checkData, true);
                                });

                            string barcode;
                            var isOk = GetStr(expectedPara.Value, comparedFieldValue, out barcode);

                            var okHistory = 0;

                            if (!string.IsNullOrEmpty(barcode))
                            {
                                if (LocalDbHelper.IsRepeat(barcode))
                                {
                                    okHistory++;
                                }
                            }

                            okHistory = 0;

                            if (isOk && okHistory > 0)
                            {
                                isOk = false;
                                barcode = "二维码重复：" + barcode;
                            }

                            var resultCode = barcode;
                            if (isAssertTrue)
                            {
                                if (isOk)
                                {
                                    var r1 = onInvokeBarcodeGroupAddMsg.Invoke(true, resultCode);
                                    ckd.Add(r1.Item1);
                                    isPush = r1.Item2;
                                    return true;
                                }
                                else
                                {
                                    var r1 = onInvokeBarcodeGroupAddMsg.Invoke(false, resultCode);
                                    ckd.Add(r1.Item1);
                                    isPush = r1.Item2;
                                }
                            }
                            else
                            {
                                if (!isOk)
                                {
                                    var r1 = onInvokeBarcodeGroupAddMsg.Invoke(true, resultCode);
                                    ckd.Add(r1.Item1);
                                    isPush = r1.Item2;
                                    return true;
                                }
                                else
                                {
                                    var r1 = onInvokeBarcodeGroupAddMsg.Invoke(false, resultCode);
                                    ckd.Add(r1.Item1);
                                    isPush = r1.Item2;
                                }
                            }
                        }
                        catch (Exception)
                        {
                            return false;
                        }
                        break;
                    #endregion

                    #region barcode
                    case "barcode":
                        var expectedBarcode = expectedPara.Value;

                        //var onInvokeBarcodeAddMsg =
                        //    new Action<bool, string>((result, resultstr) =>
                        //    {
                        //        ComparativeResultStr.Add(
                        //            msg + "实际值为【" + resultstr + "】，判断结果为【" + result + "】。");

                        //        var checkData = new CheckData
                        //        {
                        //            ProcessNo = processNo,
                        //            ParaName = paraName,
                        //            Type = expectedType,
                        //            Range = expectedBarcode,
                        //            Value = resultstr,
                        //            Unit = string.Empty,
                        //            Format = expectedPara.OkFormat
                        //        };

                        //        //var okHistory = 0;

                        //        //if (!string.IsNullOrEmpty(resultstr))
                        //        //{
                        //        //    var bll = new BLL.manufactureCheckData();
                        //        //    okHistory =
                        //        //        bll.GetModelList(
                        //        //            string.Format("productBarcode = '{0}' and checkResult = '0001'",
                        //        //                resultstr)).Count;
                        //        //}

                        //        if (isAssertTrue)
                        //        {
                        //            //if (result && okHistory > 0)
                        //            //{
                        //            //    checkData.Value = "二维码重复：" + checkData.Value;
                        //            //    result = false;
                        //            //}

                        //            checkData.Result = result ? bool.TrueString : bool.FalseString;
                        //        }
                        //        else
                        //        {
                        //            //if (!result && okHistory > 0)
                        //            //{
                        //            //    checkData.Value = "二维码重复：" + checkData.Value;
                        //            //    result = true;
                        //            //}

                        //            checkData.Result = result ? bool.FalseString : bool.TrueString;
                        //        }

                        //        actionWithPara.Invoke(checkData, true);
                        //    });

                        var onInvokeBarcodeAddMsg =
                            new Func<bool, string, Tuple<CheckData, bool>>((result, resultstr) =>
                            {
                                ComparativeResultStr.Add(
                                    msg + "实际值为【" + resultstr + "】，判断结果为【" + result + "】。");

                                var checkData = new CheckData
                                {
                                    ProcessNo = processNo,
                                    ParaName = paraName,
                                    Type = expectedType,
                                    Range = expectedBarcode,
                                    Value = resultstr,
                                    Unit = string.Empty,
                                    Format = expectedPara.OkFormat
                                };

                                //var okHistory = 0;

                                //if (!string.IsNullOrEmpty(resultstr))
                                //{
                                //    var bll = new BLL.manufactureCheckData();
                                //    okHistory =
                                //        bll.GetModelList(
                                //            string.Format("productBarcode = '{0}' and checkResult = '0001'",
                                //                resultstr)).Count;
                                //}

                                if (isAssertTrue)
                                {
                                    //if (result && okHistory > 0)
                                    //{
                                    //    checkData.Value = "二维码重复：" + checkData.Value;
                                    //    result = false;
                                    //}

                                    checkData.Result = result ? bool.TrueString : bool.FalseString;
                                }
                                else
                                {
                                    //if (!result && okHistory > 0)
                                    //{
                                    //    checkData.Value = "二维码重复：" + checkData.Value;
                                    //    result = true;
                                    //}

                                    checkData.Result = result ? bool.FalseString : bool.TrueString;
                                }

                                //actionWithPara.Invoke(checkData, true);
                                return new Tuple<CheckData, bool>(checkData, true);
                            });

                        string barcode1;
                        var okHistory1 = 0;
                        var isOk1 = GetStr(expectedPara.Value, comparedFieldValue, out barcode1);

                        if (!string.IsNullOrEmpty(barcode1))
                        {
                            if (LocalDbHelper.IsRepeat(barcode1))
                            {
                                okHistory1++;
                            }
                        }

                        okHistory1 = 0;

                        if (isOk1 && okHistory1 > 0)
                        {
                            isOk1 = false;
                            barcode1 = "二维码重复：" + barcode1;
                        }

                        string resultCode1 = barcode1;
                        if (isAssertTrue)
                        {
                            if (isOk1)
                            {
                                var r1 = onInvokeBarcodeAddMsg.Invoke(true, resultCode1);
                                ckd.Add(r1.Item1);
                                isPush = r1.Item2;
                                return true;
                            }
                            else
                            {
                                var r1 = onInvokeBarcodeAddMsg.Invoke(false, resultCode1);
                                ckd.Add(r1.Item1);
                                isPush = r1.Item2;
                            }
                        }
                        else
                        {
                            if (!isOk1)
                            {
                                var r1 = onInvokeBarcodeAddMsg.Invoke(true, resultCode1);
                                ckd.Add(r1.Item1);
                                isPush = r1.Item2;
                                return true;
                            }
                            else
                            {
                                var r1 = onInvokeBarcodeAddMsg.Invoke(false, resultCode1);
                                ckd.Add(r1.Item1);
                                isPush = r1.Item2;
                            }
                        }
                        break;
                    #endregion

                    #region ledgroup
                    case "ledgroup":
                        var ledGroupIndex =
                            expectedPara.Value.Trim('[', ']').Split(',').ToList().FindIndex(f => f.Equals(expectedPara.OkFormat.Trim('[', ']')));
                        var thisLedMin = expectedPara.Min.Trim('[', ']').Split(',')[ledGroupIndex];
                        var thisLedMax = expectedPara.Max.Trim('[', ']').Split(',')[ledGroupIndex];

                        //var onInvokeLedGroupAddMsg =
                        //    new Action<bool, string>((result, resultstr) =>
                        //    {
                        //        ComparativeResultStr.Add(
                        //            msg + "实际值为【" + resultstr + "】，判断结果为【" + result + "】。");

                        //        var checkData = new CheckData
                        //        {
                        //            ProcessNo = processNo,
                        //            ParaName = paraName,
                        //            Type = expectedType,
                        //            Range = string.Format("({0}~{1})", thisLedMin, thisLedMax).TrimStart('~'),
                        //            Value = resultstr,
                        //            Unit = expectedPara.Unit,
                        //            Format = expectedPara.OkFormat
                        //        };
                        //        if (isAssertTrue)
                        //            checkData.Result = result ? bool.TrueString : bool.FalseString;
                        //        else
                        //            checkData.Result = result ? bool.FalseString : bool.TrueString;

                        //        actionWithPara.Invoke(checkData, true);
                        //    });

                        var onInvokeLedGroupAddMsg =
                            new Func<bool, string, Tuple<CheckData, bool>>((result, resultstr) =>
                            {
                                ComparativeResultStr.Add(
                                    msg + "实际值为【" + resultstr + "】，判断结果为【" + result + "】。");

                                var checkData = new CheckData
                                {
                                    ProcessNo = processNo,
                                    ParaName = paraName,
                                    Type = expectedType,
                                    Range = string.Format("({0}~{1})", thisLedMin, thisLedMax).TrimStart('~'),
                                    Value = resultstr,
                                    Unit = expectedPara.Unit,
                                    Format = expectedPara.OkFormat
                                };
                                if (isAssertTrue)
                                    checkData.Result = result ? bool.TrueString : bool.FalseString;
                                else
                                    checkData.Result = result ? bool.FalseString : bool.TrueString;

                                //actionWithPara.Invoke(checkData, true);
                                return new Tuple<CheckData, bool>(checkData, true);
                            });

                        var thisLedActual =
                            CalculatedWithOffset(expectedPara.ProcessNo, comparedFieldValue,
                                string.IsNullOrEmpty(expectedPara.ControllerFieldOffset)
                                    ? "1*X+0"
                                    : expectedPara.ControllerFieldOffset).Invoke();

                        var isNeedInternalOffset = false;

                        //if (thisLedActual > double.Parse(max) && thisLedActual < double.Parse(max) * 1.1)
                        //{
                        //    actualValueDouble = thisLedActual * 0.9;
                        //    isInternOffset = true;
                        //}
                        //else if (thisLedActual < double.Parse(min) && thisLedActual > double.Parse(min) * 0.9)
                        //{
                        //    actualValueDouble = thisLedActual * 1.1;
                        //    isInternOffset = true;
                        //}

                        actualValueDouble = Math.Round(thisLedActual, 4, MidpointRounding.AwayFromZero);

                        if (!double.IsNaN(actualValueDouble))
                        {
                            if (isAssertTrue)
                            {
                                if (actualValueDouble >= double.Parse(thisLedMin) && actualValueDouble <= double.Parse(thisLedMax))
                                {
                                    var r1 = onInvokeLedGroupAddMsg.Invoke(true,
                                           isNeedInternalOffset
                                               ? actualValueDouble.ToString(CultureInfo.InvariantCulture) + "*"
                                               : actualValueDouble.ToString(CultureInfo.InvariantCulture));
                                    ckd.Add(r1.Item1);
                                    isPush = r1.Item2;
                                    return true;
                                }
                                else
                                {
                                    var r1 = onInvokeLedGroupAddMsg.Invoke(false, isNeedInternalOffset
                                            ? actualValueDouble.ToString(CultureInfo.InvariantCulture) + "*"
                                            : actualValueDouble.ToString(CultureInfo.InvariantCulture));
                                    ckd.Add(r1.Item1);
                                    isPush = r1.Item2;
                                }
                            }
                            else
                            {
                                if (actualValueDouble < double.Parse(thisLedMin) || actualValueDouble > double.Parse(thisLedMax))
                                {
                                    var r1 = onInvokeLedGroupAddMsg.Invoke(true, isNeedInternalOffset
                                            ? actualValueDouble.ToString(CultureInfo.InvariantCulture) + "*"
                                            : actualValueDouble.ToString(CultureInfo.InvariantCulture));
                                    ckd.Add(r1.Item1);
                                    isPush = r1.Item2;
                                    return true;
                                }
                                else
                                {
                                    var r1 = onInvokeLedGroupAddMsg.Invoke(false, isNeedInternalOffset
                                            ? actualValueDouble.ToString(CultureInfo.InvariantCulture) + "*"
                                            : actualValueDouble.ToString(CultureInfo.InvariantCulture));
                                    ckd.Add(r1.Item1);
                                    isPush = r1.Item2;
                                }
                            }
                        }
                        break;
                    #endregion

                    #region chart
                    case "chart":
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

                        var groupCount = expectedPara.ControllerFieldOffset.TrimStart('[').TrimEnd(']').Split(',').Length;
                        var totalCount =
                            expectedPara.Value.Split(';')
                                .Select(t => t.TrimStart('[').TrimEnd(']').Split(','))
                                .Aggregate(0, (current, tt) => current + tt.Length);

                        var ngCount = 0;
                        var strChartVaue = string.Empty;

                        var listGroupRange = new List<int[]>();
                        var startIndex = 0;
                        for (var i = 0; i < groupCount; i++)
                        {
                            var endIndex = startIndex + expectedPara.Value.Split(';')[i].TrimStart('[').TrimEnd(']').Split(',').Length -
                                        1;
                            listGroupRange.Add(new[] { startIndex, endIndex });
                            startIndex = endIndex + 1;
                        }

                        var tempExpectedParr =
                            expectedPara.Value.Split(';')
                                .Select(t => t.TrimStart('[').TrimEnd(']'))
                                .SelectMany(tt => tt.Split(','))
                                .Aggregate(string.Empty, (current, ttt) => current + ttt + ",");

                        tempExpectedParr = tempExpectedParr.TrimEnd(',');
                        //expectedPara.Value.Replace("[", "").Replace("]", "").Replace(";", "");

                        for (var i = 0; i < comparedFieldValue.Split(',').Length; i++)
                        {
                            var groupIndex = listGroupRange.FindIndex(f => i >= f[0] && i <= f[1]);

                            //var groupIndex = i / (totalCount / groupCount);
                            var offsetMin =
                                 expectedPara.ControllerFieldOffset.TrimStart('[').TrimEnd(']').Split(',')[groupIndex];
                            var offsetMax =
                                expectedPara.ControllerFieldOffset.TrimStart('[').TrimEnd(']').Split(',')[groupIndex];

                            var toCompareValue = double.Parse(comparedFieldValue.Split(',')[i]);
                            double actualMin;
                            double acutaMax;

                            {
                                var x =
                                    tempExpectedParr.Split(',')[i];
                                offsetMin = offsetMin.Replace("X", x);
                                actualMin = calculateFunc.Invoke(offsetMin) + double.Parse(min);
                            }
                            {
                                var y =
                                 tempExpectedParr.Split(',')[i];
                                offsetMax = offsetMax.Replace("X", y);
                                acutaMax = calculateFunc.Invoke(offsetMax) + double.Parse(max);
                            }

                            if (!double.IsNaN(actualMin) && !double.IsNaN(acutaMax))
                            {
                                if (toCompareValue >= actualMin &&
                                    toCompareValue <= acutaMax)
                                {

                                }
                                else
                                {
                                    ngCount++;
                                }

                                if (string.IsNullOrEmpty(strChartVaue))
                                {
                                    strChartVaue += string.Format("[{0},{1},{2},{3}]", tempExpectedParr.Split(',')[i], actualMin, acutaMax, toCompareValue);
                                }
                                else
                                {
                                    strChartVaue += ";" + string.Format("[{0},{1},{2},{3}]",
                                       tempExpectedParr.Split(',')[i], actualMin, acutaMax, toCompareValue);
                                }
                            }
                        }

                        if (isAssertTrue)
                        {
                            if (ngCount == 0)
                            {
                                var r1 = onInvokeAddMsg.Invoke(true, strChartVaue, expectedUnit);
                                ckd.Add(r1.Item1);
                                isPush = r1.Item2;
                                return true;
                            }
                            else
                            {
                                var r1 = onInvokeAddMsg.Invoke(true, strChartVaue, expectedUnit);
                                ckd.Add(r1.Item1);
                                isPush = r1.Item2;
                                return false;
                            }
                        }
                        else
                        {
                            if (ngCount > 0)
                            {
                                var r1 = onInvokeAddMsg.Invoke(true, strChartVaue, expectedUnit);
                                ckd.Add(r1.Item1);
                                isPush = r1.Item2;
                                return true;
                            }
                            else
                            {
                                var r1 = onInvokeAddMsg.Invoke(false, strChartVaue, expectedUnit);
                                ckd.Add(r1.Item1);
                                isPush = r1.Item2;
                            }
                        }
                        break;
                    #endregion

                    #region bitmap

                    case "bitmap":
                        var rb1 = onInvokeAddMsg.Invoke(true, comparedFieldValue, string.Empty);
                        ckd.Add(rb1.Item1);
                        isPush = rb1.Item2;
                        return true;

                        #endregion
                }
            }
            else if (expectedPara.DataType.ToLower() is "platebarcode")
            {
                // platebarcode
                if (string.IsNullOrEmpty(expectedPara.Value))
                    expectedPara.Value = string.Empty;
                var expectedUnit = string.Empty;
                if (!string.IsNullOrEmpty(expectedPara.Unit))
                    expectedUnit = expectedPara.Unit;

                var comparedFieldValue = GetFieldValueStr(expectedPara.ControllerField);
                var expectedType = expectedPara.DataType.ToLower();
                var min = string.IsNullOrEmpty(expectedPara.Min)
                                    ? string.Empty
                                    : expectedPara.Min;
                var max = string.IsNullOrEmpty(expectedPara.Max)
                    ? expectedPara.Value
                    : expectedPara.Max;

                var msg = isAssertTrue
                               ? string.Format("判断参数【{0}】应在范围【{1}~{2}】内。", paraName, min, max)
                               : string.Format("判断参数【{0}】应不在范围【{1}~{2}】内。", paraName, min, max);

                //var onInvokeAddMsg =
                //    new Action<bool, string, string>((result, resultstr, unit) =>
                //    {
                //        ComparativeResultStr.Add(msg + "实际值为【" + resultstr + "】，判断结果为【" + result + "】。");

                //        var checkData = new CheckData
                //        {
                //            ProcessNo = processNo,
                //            ParaName = paraName,
                //            Type = expectedType,
                //            Range = string.Format("({0}~{1})", min, max).TrimStart('~'),
                //            Value = resultstr,
                //            Unit = unit,
                //            Format = expectedPara.OkFormat
                //        };
                //        if (isAssertTrue)
                //            checkData.Result = result ? bool.TrueString : bool.FalseString;
                //        else
                //            checkData.Result = result ? bool.FalseString : bool.TrueString;

                //        actionWithPara.Invoke(checkData, true);
                //    });

                var onInvokeAddMsg =
                    new Func<bool, string, string, Tuple<CheckData, bool>>((result, resultstr, unit) =>
                    {
                        ComparativeResultStr.Add(msg + "实际值为【" + resultstr + "】，判断结果为【" + result + "】。");
                        var checkData = new CheckData
                        {
                            ProcessNo = processNo,
                            ParaName = paraName,
                            Type = expectedType,
                            Range = string.Format("({0}~{1})", min, max).TrimStart('~'),
                            Value = resultstr,
                            Unit = unit,
                            Format = expectedPara.OkFormat
                        };
                        if (isAssertTrue)
                            checkData.Result = result ? bool.TrueString : bool.FalseString;
                        else
                            checkData.Result = result ? bool.FalseString : bool.TrueString;

                        //actionWithPara.Invoke(checkData, true);
                        return Tuple.Create<CheckData, bool>(checkData, true);
                    });

                var actualStr = comparedFieldValue;

                if (expectedPara.OkFormat == "=")
                {
                    if (isAssertTrue)
                    {
                        if (actualStr.Equals(expectedPara.Value))
                        {
                            var r1 = onInvokeAddMsg.Invoke(true, actualStr, expectedUnit);
                            ckd.Add(r1.Item1);
                            isPush = r1.Item2;
                            return true;
                        }
                        else
                        {
                            var r1 = onInvokeAddMsg.Invoke(false, actualStr, expectedUnit);
                            ckd.Add(r1.Item1);
                            isPush = r1.Item2;
                        }
                    }
                    else
                    {
                        if (!actualStr.Equals(expectedPara.Value))
                        {
                            var r1 = onInvokeAddMsg.Invoke(true, actualStr, expectedUnit);
                            ckd.Add(r1.Item1);
                            isPush = r1.Item2;
                            return true;
                        }
                        else
                        {
                            var r1 = onInvokeAddMsg.Invoke(false, actualStr, expectedUnit);
                            ckd.Add(r1.Item1);
                            isPush = r1.Item2;

                        }
                    }
                }
                else if (expectedPara.OkFormat == "like")
                {
                    if (isAssertTrue)
                    {
                        if (actualStr.Contains(expectedPara.Value))
                        {
                            var r1 = onInvokeAddMsg.Invoke(true, actualStr, expectedUnit);
                            ckd.Add(r1.Item1);
                            isPush = r1.Item2;
                            return true;
                        }
                        else
                        {
                            var r1 = onInvokeAddMsg.Invoke(false, actualStr, expectedUnit);
                            ckd.Add(r1.Item1);
                            isPush = r1.Item2;
                        }
                    }
                    else
                    {
                        if (!actualStr.Contains(expectedPara.Value))
                        {
                            var r1 = onInvokeAddMsg.Invoke(true, actualStr, expectedUnit);
                            ckd.Add(r1.Item1);
                            isPush = r1.Item2;
                            return true;
                        }
                        else
                        {
                            var r1 = onInvokeAddMsg.Invoke(false, actualStr, expectedUnit);
                            ckd.Add(r1.Item1);
                            isPush = r1.Item2;
                        }
                    }
                }
            }
            else if (expectedPara.DataType.ToLower() is "roi") // 单帧
            {
                var nowTs = HighPrecisionTimer.GetTimestamp();
                //_roiBuffers.RemoveAll(f => HighPrecisionTimer.GetTimestampIntervalMs(f.ImgTs, nowTs) > 60 * 1000);
                _roiBuffers.RemoveAll(f => HighPrecisionTimer.GetTimestampIntervalMs(f.ImgTs, nowTs) > 60 * 1000 * 3);

                var ctrlField = expectedPara.ControllerField;
                var okFormatAsRoiName = expectedPara.OkFormat.ToLower();
                var offsetAsSrcDicBuffKey = expectedPara.ControllerFieldOffset;

                var strSpliByDot = ctrlField.GetStrsSplitByValue(".Field.");
                var comparedControllerName = strSpliByDot[0];
                var comparedFieldName = strSpliByDot[1];
                var comparedController = comparedControllerName.GetControllerByName(ControllersList);
                var comparedValue = comparedController.GetType().GetField(comparedFieldName).GetValue(comparedController);
                if (comparedValue.GetType() == typeof(Dictionary<string, Tuple<long, string>[]>))
                {
                    // 显示roi框结果
                    var onInvokeRoiMsg =
                        new Func<bool, Rect, string, string, string, int, bool, Tuple<CheckData, bool>>((result, rect, min, max, resultstr, index, isPushf) =>
                        {
                            var checkData = new CheckData
                            {
                                ProcessNo = processNo,
                                ParaName = paraName + "[" + index + "]" + string.Format("_[{0:F0},{1:F0},{2:F0},{3:F0}]", rect.X, rect.Y, rect.Width, rect.Height),
                                Type = "Roi",
                                Range = string.Format("{0}~{1}", min, max),
                                Value = resultstr,
                                Unit = paraName,
                                Format = okFormatAsRoiName,
                                Result = isAssertTrue ? (result ? bool.TrueString : bool.FalseString) : (result ? bool.FalseString : bool.TrueString)
                            };

                            //actionWithPara.Invoke(checkData, isPush);
                            return new Tuple<CheckData, bool>(checkData, isPushf);
                        });

                    // 显示测试结果
                    var onInvokeVisionResultMsg =
                        new Func<bool, string, Tuple<CheckData, bool>>((result, resultstr) =>
                        {
                            var checkData = new CheckData
                            {
                                ProcessNo = processNo,
                                ParaName = paraName,
                                Type = "静态图像",
                                Range = "OK",
                                Value = resultstr,
                                Unit = string.Empty,
                                Result = isAssertTrue ? (result ? bool.TrueString : bool.FalseString) : (result ? bool.FalseString : bool.TrueString)
                            };
                            //actionWithPara.Invoke(checkData, true);
                            return new Tuple<CheckData, bool>(checkData, true);
                        });

                    var key = offsetAsSrcDicBuffKey;
                    var dic = comparedValue as Dictionary<string, Tuple<long, string>[]>;
                    var rois = DeviceConfig.Rois.ToList().FindAll(f => string.Equals(f.Name, okFormatAsRoiName, StringComparison.CurrentCultureIgnoreCase));
                    if (dic != null && dic.ContainsKey(key) && dic[key] != null && dic[key].Any() && rois.Any())
                    {
                        var retryCount = dic[key].Length;

                        var listRect = new List<Rect>();
                        var listGrays = new List<double>();
                        var listMin = new List<double>();
                        var listMax = new List<double>();
                        var listIsInRange = new List<bool>();
                        try
                        {
                            foreach (var item in rois)
                            {
                                var x = double.Parse(item.RectX);
                                var y = double.Parse(item.RectY);
                                var w = double.Parse(item.RectWidth);
                                var h = double.Parse(item.RectHeight);
                                listRect.Add(new Rect((int)x, (int)y, (int)w, (int)h));
                                listMin.Add(double.Parse(item.Min));
                                listMax.Add(double.Parse(item.Max));
                                listGrays.Add(double.MinValue);
                                listIsInRange.Add(false);
                            }
                        }
                        catch (Exception)
                        {
                            var re1 = onInvokeVisionResultMsg(!isAssertTrue, dic[key][0].Item2);
                            ckd.Add(re1.Item1);
                            isPush = re1.Item2;
                            return !isAssertTrue;
                        }

                        var isVisionNg = false;
                        var breakIndex = -1;
                        var enterMs = HighPrecisionTimer.GetTimestamp();
                        Console.WriteLine("开始处理：" + expectedPara.Name);                      

                        for (var tryingIndex = 0; tryingIndex < retryCount; tryingIndex++)
                        {
                            breakIndex++;
                            var imgBuff = dic[key][tryingIndex].Item2;
                            var imgTs = dic[key][tryingIndex].Item1;

                            for (var i = 0; i < rois.Count; i++)
                            {
                                listGrays[i] = double.MinValue;
                                listIsInRange[i] = false;
                            }

                            var findRoiBuffer = _roiBuffers.Find(f => f.ImgTs == imgTs);
                            if (findRoiBuffer is null)
                            {
                                var allThisBuffPara = DeviceConfig.Paras.ToList().FindAll(f => f.DataType.ToLower() is "roi" && f.ControllerFieldOffset.ToLower() == key.ToLower());

                                var allRois = DeviceConfig.Rois.ToList();
                                var roiBuffer = new RoiBuffer { ImgName = key, ImgTs = imgTs };
                                foreach (var eachRoi in allRois)
                                {
                                    if (eachRoi != null && !string.IsNullOrEmpty(eachRoi.Name) && !string.IsNullOrEmpty(eachRoi.RectX) & allThisBuffPara.Any(f => f.OkFormat.ToLower() == eachRoi.Name.ToLower()))
                                    {
                                        var x = double.Parse(eachRoi.RectX);
                                        var y = double.Parse(eachRoi.RectY);
                                        var w = double.Parse(eachRoi.RectWidth);
                                        var h = double.Parse(eachRoi.RectHeight);
                                        if (!roiBuffer.Rects.Any(f => IsSameRect(x, y, w, h, f.Rect)))
                                            roiBuffer.Rects.Add(new RectGray { Gray = double.MinValue, Rect = new Rect((int)x, (int)y, (int)w, (int)h) });
                                    }
                                }

                                var base64String = dic[key];
                                //var bitMap = MyCamera.Base64StringToBitmap(imgBuff);
                                var bitMap = RunFuncTask(() => { return MyCamera.Base64StringToBitmap(imgBuff); }).Result;
                                Console.WriteLine("取缓存的base64后转换成bitmap耗时：{0}ms", HighPrecisionTimer.GetTimestampIntervalMs(enterMs, HighPrecisionTimer.GetTimestamp()));

                                if (bitMap == null)
                                {
                                    var rbb1 = onInvokeVisionResultMsg(!isAssertTrue, string.Empty);
                                    ckd.Add(rbb1.Item1);
                                    isPush = rbb1.Item2;
                                    return !isAssertTrue;
                                }
                                else
                                {
                                    //var mat = BitmapConverter.ToMat(bitMap);
                                    var mat = RunFuncTask(() => { return BitmapConverter.ToMat(bitMap); }).Result;
                                    Console.WriteLine("取缓存的base64后转换成bitmap后再转换成mat耗时：{0}ms", HighPrecisionTimer.GetTimestampIntervalMs(enterMs, HighPrecisionTimer.GetTimestamp()));

                                    if (mat.Channels() != 1)
                                    {
                                        Cv2.CvtColor(mat, mat, ColorConversionCodes.BGR2GRAY);
                                        Console.WriteLine("mat转gray耗时：{0}ms", HighPrecisionTimer.GetTimestampIntervalMs(enterMs, HighPrecisionTimer.GetTimestamp()));
                                    }

                                    // 解析ROI并读取对应的gray
                                    try
                                    {
                                        Parallel.ForEach(roiBuffer.Rects, (tshape, state, index) =>
                                        {
                                            var shape = tshape.Rect;
                                            var newX = (shape.TopLeft.X + shape.Width / 2);
                                            newX = newX < 0 ? 0 : newX;
                                            newX = newX > mat.Width ? mat.Width : newX;

                                            var newY = (shape.TopLeft.Y + shape.Height / 2);
                                            newY = newY < 0 ? 0 : newY;
                                            newY = newY > mat.Height ? mat.Height : newY;

                                            var newWidth = shape.Width;
                                            newWidth = newX + newWidth > mat.Width ? mat.Width - newX : newWidth;

                                            var newHeight = shape.Height;
                                            newHeight = newY + newHeight > mat.Height ? mat.Height - newY : newHeight;

                                            var rectOnImage = new Rect(newX - shape.Width / 2, newY - shape.Height / 2, newWidth, newHeight);
                                            var roiMat = new Mat(mat, MyCamera.GetRectInMat(mat.Width, mat.Height, rectOnImage));
                                            try
                                            {
                                                var gray = Cv2.Mean(roiMat);
                                                var grayPerMat = Math.Round(gray.Val0, 2, MidpointRounding.AwayFromZero);
                                                roiBuffer.Rects[(int)index].Gray = Math.Round(grayPerMat, 2, MidpointRounding.AwayFromZero);
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);
                                            }
                                            finally
                                            {
                                                roiMat?.Dispose();
                                                roiMat = null;
                                            }
                                        });
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                    finally
                                    {
                                        bitMap?.Dispose();
                                        bitMap = null;
                                        mat?.Dispose();
                                        mat = null;

                                        GC.Collect();
                                    }

                                    _roiBuffers.Add(roiBuffer);
                                }
                            }

                            findRoiBuffer = _roiBuffers.Find(f => f.ImgTs == imgTs);
                            if (findRoiBuffer != null)
                            {
                                for (var i = 0; i < listRect.Count; i++)
                                {
                                    var x = listRect[i].X;
                                    var y = listRect[i].Y;
                                    var w = listRect[i].Width;
                                    var h = listRect[i].Height;

                                    var findRoiIndex = -1;
                                    for (int fi = 0; fi < findRoiBuffer.Rects.Count; fi++)
                                    {
                                        var fiR = findRoiBuffer.Rects[fi];
                                        if (IsSameRect(x, y, w, h, fiR.Rect))
                                        {
                                            findRoiIndex = fi;
                                            break;
                                        }
                                    }

                                    if (findRoiIndex != -1)
                                    {
                                        var fiR = findRoiBuffer.Rects[findRoiIndex];
                                        listGrays[i] = fiR.Gray;
                                        listIsInRange[i] = fiR.Gray >= listMin[i] && fiR.Gray <= listMax[i];
                                    }
                                }

                                if (listGrays.Any(f => f is double.MinValue))
                                {
                                    var rg1 = onInvokeVisionResultMsg(!isAssertTrue, imgBuff);
                                    ckd.Add(rg1.Item1);
                                    isPush = rg1.Item2;
                                    return !isAssertTrue;
                                }
                            }
                            else
                            {
                                var rg1 = onInvokeVisionResultMsg(!isAssertTrue, imgBuff);
                                ckd.Add(rg1.Item1);
                                isPush = rg1.Item2;
                                return !isAssertTrue;
                            }

                            isVisionNg = listIsInRange.Any(f => f is false);

                            if (!isVisionNg)
                                break;
                        }

                        //var bitmapResult = MyCamera.Base64StringToBitmap(dic[key][breakIndex].Item2);
                        var bitmapResult = RunFuncTask(() => { return MyCamera.Base64StringToBitmap(dic[key][breakIndex].Item2); }).Result;
                        var matShow = BitmapConverter.ToMat(bitmapResult);
                        if (matShow.Channels() == 1)
                            Cv2.CvtColor(matShow, matShow, ColorConversionCodes.GRAY2BGR);

                        Console.WriteLine("缓存转显示图像耗时：{0}ms", HighPrecisionTimer.GetTimestampIntervalMs(enterMs, HighPrecisionTimer.GetTimestamp()));

                        for (var i = 0; i < rois.Count; i++)
                        {
                            var rect = listRect[i];
                            var isInRange = listIsInRange[i];
                            var min = listMin[i].ToString();
                            var max = listMax[i].ToString();
                            var actualValueDouble = listGrays[i];

                            if (!isInRange)
                            {
                                var rroi1 = onInvokeRoiMsg.Invoke(isAssertTrue ? isInRange : !isInRange, rect, min, max, actualValueDouble.ToString(CultureInfo.InvariantCulture), i, i == rois.Count - 1);
                                ckd.Add(rroi1.Item1);
                                isPush = rroi1.Item2;
                            }

                            Cv2.Rectangle(matShow, rect, isInRange ? Scalar.Green : Scalar.Red, 3);
                            Cv2.PutText(matShow, i.ToString(), new OpenCvSharp.Point(rect.X + rect.Width + 1, rect.Y), HersheyFonts.HersheySimplex, 0.4d, isInRange ? Scalar.Green : Scalar.Red);
                        }

                        Console.WriteLine("在显示图像上画roi耗时：{0}ms", HighPrecisionTimer.GetTimestampIntervalMs(enterMs, HighPrecisionTimer.GetTimestamp()));

                        Cv2.Resize(matShow, matShow, new Size(1024, 768));

                        Console.WriteLine(expectedPara.Name + "显示的图像缩放耗时：{0}ms", HighPrecisionTimer.GetTimestampIntervalMs(enterMs, HighPrecisionTimer.GetTimestamp()));

                        bitmapResult?.Dispose();
                        bitmapResult = null;
                        //bitmapResult = BitmapConverter.ToBitmap(matShow);
                        bitmapResult = RunFuncTask(() => { return BitmapConverter.ToBitmap(matShow); }).Result;
                        matShow?.Dispose();
                        matShow = null;

                        var base64VisisonResult = MyCamera.BitmapToBase64String(bitmapResult);
                        bitmapResult?.Dispose();
                        bitmapResult = null;
                        var r1 = onInvokeVisionResultMsg.Invoke(isAssertTrue ? !isVisionNg : isVisionNg, base64VisisonResult);
                        ckd.Add(r1.Item1);
                        isPush = r1.Item2;
                        return isAssertTrue ? !isVisionNg : isVisionNg;
                    }
                    else
                    {
                        var r1 = onInvokeVisionResultMsg(!isAssertTrue, string.Empty);
                        ckd.Add(r1.Item1);
                        isPush = r1.Item2;
                    }
                }

                return !isAssertTrue;
            }
            //else if (expectedPara.DataType.ToLower() is "roi[]") // 多帧
            //{
            //    var ctrlField = expectedPara.ControllerField;
            //    var okFormatAsRoiName = expectedPara.OkFormat.ToLower();
            //    var offsetAsSrcDicBuffKey = expectedPara.ControllerFieldOffset;

            //    var strSpliByDot = ctrlField.GetStrsSplitByValue(".Field.");
            //    var comparedControllerName = strSpliByDot[0];
            //    var comparedFieldName = strSpliByDot[1];
            //    var comparedController = comparedControllerName.GetControllerByName(ControllersList);
            //    var comparedValue = comparedController.GetType().GetField(comparedFieldName).GetValue(comparedController);
            //    if (comparedValue.GetType() == typeof(Dictionary<string, string[]>))
            //    {
            //        // 显示roi框结果
            //        var onInvokeRoiMsg =
            //            new Action<bool, Rect, string, string, string, int>((result, rect, min, max, resultstr, index) =>
            //            {
            //                var checkData = new CheckData
            //                {
            //                    ProcessNo = processNo,
            //                    ParaName = paraName + "_框" + index + string.Format("_位置[{0:F0},{1:F0},{2:F0},{3:F0}]", rect.X, rect.Y, rect.Width, rect.Height),
            //                    Type = "Roi",
            //                    Range = string.Format("{0}~{1}", min, max),
            //                    Value = resultstr,
            //                    Unit = paraName,
            //                    Format = okFormatAsRoiName,
            //                    Result = isAssertTrue ? (result ? bool.TrueString : bool.FalseString) : (result ? bool.FalseString : bool.TrueString)
            //                };
            //                actionWithPara.Invoke(checkData, true);
            //            });

            //        // 显示测试结果
            //        var onInvokeVisionResultMsg =
            //            new Action<bool, string>((result, resultstr) =>
            //            {
            //                var checkData = new CheckData
            //                {
            //                    ProcessNo = processNo,
            //                    ParaName = paraName,
            //                    Type = "动态图像",
            //                    Range = "OK",
            //                    Value = resultstr,
            //                    Unit = string.Empty,
            //                    Result = isAssertTrue ? (result ? bool.TrueString : bool.FalseString) : (result ? bool.FalseString : bool.TrueString)
            //                };
            //                actionWithPara.Invoke(checkData, true);
            //            });

            //        var key = offsetAsSrcDicBuffKey;
            //        var dic = comparedValue as Dictionary<string, string[]>;
            //        var matchedRois = DeviceConfig.Rois.ToList().FindAll(f => string.Equals(f.Name, okFormatAsRoiName, StringComparison.CurrentCultureIgnoreCase));
            //        if (dic != null && dic.ContainsKey(key) && dic[key] != null && dic[key].Any() && dic[key][0] != null && !string.IsNullOrEmpty(dic[key][0]) && matchedRois.Any())
            //        {
            //            var imgCount = dic[key].Length;

            //            // 按Group分组
            //            //var groupedRois = matchedRois.GroupBy(g => g.Group).ToList().OrderByDescending(grp => grp).ToArray();

            //            var groupedRois = new List<DeviceConfigRoi[]>();

            //            var maxRoiCount = int.MinValue;
            //            var maxRoiAtGroupIndex = 0;

            //            for (var g = 0; g < 100; g++)
            //            {
            //                var tpGroupedRois = matchedRois.FindAll(f => string.Equals(f.Group, g.ToString(), StringComparison.CurrentCultureIgnoreCase)).ToArray();
            //                if (g == 0 && !tpGroupedRois.Any())
            //                {
            //                    var showMat = MyCamera.FormatMat(dic[key]);
            //                    var showBitMap = BitmapConverter.ToBitmap(showMat);
            //                    showMat?.Dispose();
            //                    var showStr = MyCamera.BitmapToBase64String(showBitMap);
            //                    showBitMap?.Dispose();
            //                    onInvokeVisionResultMsg(!isAssertTrue, showStr);
            //                    return !isAssertTrue;
            //                }

            //                if (!tpGroupedRois.Any())
            //                    break;

            //                groupedRois.Add(tpGroupedRois);

            //                if (tpGroupedRois.Length > maxRoiCount)
            //                {
            //                    maxRoiCount = tpGroupedRois.Length;
            //                    maxRoiAtGroupIndex = g;
            //                }
            //            }

            //            if (!groupedRois.Any())
            //            {
            //                var showMat = MyCamera.FormatMat(dic[key]);
            //                var showBitMap = BitmapConverter.ToBitmap(showMat);
            //                showMat?.Dispose();
            //                var showStr = MyCamera.BitmapToBase64String(showBitMap);
            //                showBitMap?.Dispose();
            //                onInvokeVisionResultMsg(!isAssertTrue, showStr);
            //            }
            //            else
            //            {
            //                if (groupedRois.Count > imgCount)
            //                {
            //                    var showMat = MyCamera.FormatMat(dic[key]);
            //                    var showBitMap = BitmapConverter.ToBitmap(showMat);
            //                    showMat?.Dispose();
            //                    var showStr = MyCamera.BitmapToBase64String(showBitMap);
            //                    showBitMap?.Dispose();
            //                    onInvokeVisionResultMsg(!isAssertTrue, showStr);
            //                }
            //                else
            //                {
            //                    var dicg = new Dictionary<int, List<ImgLedState>>();
            //                    for (var i = 0; i < groupedRois.Count; i++)
            //                    {
            //                        dicg.Add(i, new List<ImgLedState>());
            //                        for (var k = 0; k < imgCount; k++)
            //                        {
            //                            dicg[i].Add(new ImgLedState { ImgIndex = k });

            //                            for (var j = 0; j < groupedRois[i].Length; j++)
            //                            {
            //                                var roi = groupedRois[i][j];
            //                                var rect = new Rect(int.Parse(roi.RectX), int.Parse(roi.RectY), int.Parse(roi.RectWidth), int.Parse(roi.RectHeight));
            //                                var min = double.Parse(roi.Min);
            //                                var max = double.Parse(roi.Max);

            //                                dicg[i][k].LedState.Add(new LedState { Roi = rect, Min = min, Max = max, Gray = double.MinValue });
            //                            }
            //                        }
            //                    }

            //                    var keyGs = dicg.Keys.ToList();
            //                    var st = new Stopwatch();
            //                    st.Start();

            //                    Parallel.For(0, imgCount, tryingIndex =>
            //                    {
            //                        var imgBuff = dic[key][tryingIndex];
            //                        var tpbitmap = MyCamera.Base64StringToBitmap(imgBuff);
            //                        var tpmat = BitmapConverter.ToMat(tpbitmap);
            //                        var matWidth = tpmat.Width;
            //                        var matHeight = tpmat.Height;
            //                        tpbitmap.Dispose();


            //                        for (var i = 0; i < keyGs.Count; i++)
            //                        {
            //                            var keyG = keyGs[i];
            //                            var imgLedState = dicg[keyG][tryingIndex];

            //                            Parallel.For(0, imgLedState.LedState.Count, ledIndex =>
            //                            {
            //                                var originRect = imgLedState.LedState[ledIndex].Roi;
            //                                var roiMat = new Mat(tpmat, MyCamera.GetRectInMat(matWidth, matHeight, originRect));
            //                                var meanVal = Cv2.Mean(roiMat);
            //                                var grayPer = Math.Round(meanVal.Val0, 1, MidpointRounding.AwayFromZero);
            //                                grayPer = MyCamera.GetLxByRgb(meanVal);
            //                                imgLedState.LedState[ledIndex].Gray = grayPer;
            //                                roiMat.Dispose();
            //                            });
            //                        }
            //                    });

            //                    st.Stop();
            //                    Console.WriteLine("calc cost {0}ms", st.ElapsedMilliseconds);

            //                    var listRect = new List<Rect>();
            //                    var listGrays = new List<double>();
            //                    var listMin = new List<double>();
            //                    var listMax = new List<double>();
            //                    var listIsInRange = new List<bool>();
            //                    try
            //                    {
            //                        foreach (var item in matchedRois)
            //                        {
            //                            var x = double.Parse(item.RectX);
            //                            var y = double.Parse(item.RectY);
            //                            var w = double.Parse(item.RectWidth);
            //                            var h = double.Parse(item.RectHeight);
            //                            listRect.Add(new Rect((int)x, (int)y, (int)w, (int)h));
            //                            listMin.Add(double.Parse(item.Min));
            //                            listMax.Add(double.Parse(item.Max));
            //                            listGrays.Add(double.MinValue);
            //                            listIsInRange.Add(false);
            //                        }
            //                    }
            //                    catch (Exception)
            //                    {
            //                        onInvokeVisionResultMsg(!isAssertTrue, string.Empty);
            //                        return !isAssertTrue;
            //                    }

            //                    var isVisionNg = false;
            //                    var breakIndex = -1;

            //                    for (var tryingIndex = 0; tryingIndex < imgCount; tryingIndex++)
            //                    {
            //                        breakIndex++;
            //                        var imgBuff = dic[key][tryingIndex];

            //                        for (var i = 0; i < matchedRois.Count; i++)
            //                        {
            //                            listGrays[i] = double.MinValue;
            //                            listIsInRange[i] = false;
            //                        }

            //                        var base64String = dic[key];
            //                        var bitMap = MyCamera.Base64StringToBitmap(imgBuff);
            //                        if (bitMap == null)
            //                        {
            //                            onInvokeVisionResultMsg(!isAssertTrue, string.Empty);
            //                            return !isAssertTrue;
            //                        }
            //                        else
            //                        {
            //                            var mat = BitmapConverter.ToMat(bitMap);

            //                            // 解析ROI并读取对应的gray
            //                            try
            //                            {
            //                                Parallel.ForEach(listRect, (shape, state, index) =>
            //                                {
            //                                    var newX = (shape.TopLeft.X + shape.Width / 2);
            //                                    newX = newX < 0 ? 0 : newX;
            //                                    newX = newX > mat.Width ? mat.Width : newX;

            //                                    var newY = (shape.TopLeft.Y + shape.Height / 2);
            //                                    newY = newY < 0 ? 0 : newY;
            //                                    newY = newY > mat.Height ? mat.Height : newY;

            //                                    var newWidth = shape.Width;
            //                                    newWidth = newX + newWidth > mat.Width ? mat.Width - newX : newWidth;

            //                                    var newHeight = shape.Height;
            //                                    newHeight = newY + newHeight > mat.Height ? mat.Height - newY : newHeight;

            //                                    var rectOnImage = new Rect(newX - shape.Width / 2, newY - shape.Height / 2, newWidth, newHeight);
            //                                    var roiMat = new Mat(mat, MyCamera.GetRectInMat(mat.Width, mat.Height, rectOnImage));
            //                                    try
            //                                    {
            //                                        var gray = Cv2.Mean(roiMat);
            //                                        var grayPerMat = Math.Round(gray.Val0, 2, MidpointRounding.AwayFromZero);
            //                                        listGrays[(int)index] = Math.Round(grayPerMat, 2, MidpointRounding.AwayFromZero);
            //                                        listIsInRange[(int)index] = grayPerMat >= listMin[(int)index] && grayPerMat <= listMax[(int)index];
            //                                    }
            //                                    catch (Exception ex)
            //                                    {
            //                                        Console.WriteLine(ex.Message);
            //                                    }
            //                                    finally
            //                                    {
            //                                        roiMat.Dispose();
            //                                    }
            //                                });
            //                            }
            //                            catch (Exception ex)
            //                            {
            //                                Console.WriteLine(ex.Message);
            //                            }
            //                            finally
            //                            {
            //                                bitMap?.Dispose();
            //                                mat.Dispose();
            //                            }

            //                            if (listGrays.Any(f => f is double.MinValue))
            //                            {
            //                                onInvokeVisionResultMsg(!isAssertTrue, string.Empty);
            //                                return !isAssertTrue;
            //                            }
            //                        }

            //                        isVisionNg = listIsInRange.Any(f => f is false);

            //                        if (!isVisionNg)
            //                            break;
            //                    }

            //                    var bitmapResult = MyCamera.Base64StringToBitmap(dic[key][breakIndex]);
            //                    var matShow = BitmapConverter.ToMat(bitmapResult);
            //                    if (matShow.Channels() == 1)
            //                        Cv2.CvtColor(matShow, matShow, ColorConversionCodes.GRAY2BGR);

            //                    for (var i = 0; i < matchedRois.Count; i++)
            //                    {
            //                        var rect = listRect[i];
            //                        var isInRange = listIsInRange[i];
            //                        var min = listMin[i].ToString();
            //                        var max = listMax[i].ToString();
            //                        var actualValueDouble = listGrays[i];

            //                        onInvokeRoiMsg.Invoke(isAssertTrue ? isInRange : !isInRange, rect, min, max, actualValueDouble.ToString(CultureInfo.InvariantCulture), i);
            //                        Cv2.Rectangle(matShow, rect, isInRange ? Scalar.Green : Scalar.Red, 3);
            //                        Cv2.PutText(matShow, i.ToString(), new OpenCvSharp.Point(rect.X + rect.Width + 1, rect.Y), HersheyFonts.HersheySimplex, 0.4d, isInRange ? Scalar.Green : Scalar.Red);
            //                    }

            //                    bitmapResult?.Dispose();
            //                    bitmapResult = null;
            //                    bitmapResult = BitmapConverter.ToBitmap(matShow);
            //                    matShow?.Dispose();
            //                    matShow = null;

            //                    var base64VisisonResult = MyCamera.BitmapToBase64String(bitmapResult);
            //                    bitmapResult?.Dispose();
            //                    bitmapResult = null;
            //                    onInvokeVisionResultMsg.Invoke(isAssertTrue ? !isVisionNg : isVisionNg, base64VisisonResult);
            //                    return isAssertTrue ? !isVisionNg : isVisionNg;
            //                }
            //            }
            //        }
            //        else
            //        {
            //            onInvokeVisionResultMsg(!isAssertTrue, string.Empty);
            //        }
            //    }

            //    return !isAssertTrue;
            //}

            return !isAssertTrue;
        }

        private async void RunActionTask(Action act)
        {
            await Task.Run(() => act());
        }

        private async Task<T> RunFuncTask<T>(Func<T> func)
        {
            var result = await Task.Run(() => { return func.Invoke(); });
            return result;
        }

        private class ImgLedState
        {
            public int ImgIndex;

            public readonly List<LedState> LedState = new List<LedState>();
        }

        private class LedState
        {
            public Rect Roi;
            public double Gray;
            public double Min;
            public double Max;
        }

        /// <summary>
        /// 和变量做比较
        /// </summary>
        /// <returns></returns>
        private bool ComparedWithField(string left, string right, string compareMethod)
        {
            // 2.1 变量是常量
            // 2.2 变量是参数
            var leftFieldValueStr = GetFieldValueStr(left);

            //var part = DeviceConfig.Parts.ToList().Find(f => f.Name == leftSplitByDot[2]);
            //var leftField = part.ControllerField;

            string rightFieldValueStr;
            if (right.Contains(".Field."))
            {
                rightFieldValueStr = GetFieldValueStr(right);
            }
            else if (right.Contains(".Part."))
            {
                var part = DeviceConfig.Parts.ToList().Find(f => f.Name == right.GetStrsSplitByValue(".Part.")[1]);
                var rightField = part.ControllerField;
                rightFieldValueStr = GetFieldValueStr(rightField);
            }
            else
            {
                rightFieldValueStr = right;
            }

            //string[] rightFieldValueStr = { right.Contains(".Field.") ? GetFieldValueStr(right) : right };

            var msg = string.Format("判断【变量：{0}】应【{1}】【{2}】，实际值为【{3}】。", left, compareMethod, right,
                leftFieldValueStr);
            var r =
                new Action<bool>(
                    result => ComparativeResultStr.Add(msg + "判断结果为【" + result + "】。"));

            switch (compareMethod)
            {
                case "==":
                    // bool型需要特殊处理
                    if (
                        left.GetStrsSplitByValue(".Field.")[0].GetControllerByName(ControllersList)
                            .GetType()
                            .GetField(left.GetStrsSplitByValue(".Field.")[1])
                            .FieldType == typeof(bool))
                    {
                        rightFieldValueStr = rightFieldValueStr == "1" ? bool.TrueString.ToUpper() : bool.FalseString.ToUpper();
                        leftFieldValueStr = leftFieldValueStr.ToUpper();
                    }
                    else if (
                        left.GetStrsSplitByValue(".Field.")[0].GetControllerByName(ControllersList)
                            .GetType()
                            .GetField(left.GetStrsSplitByValue(".Field.")[1])
                            .FieldType == typeof(bool?))
                    {
                        rightFieldValueStr = rightFieldValueStr == "1" ? bool.TrueString.ToUpper() : bool.FalseString.ToUpper();
                        leftFieldValueStr = leftFieldValueStr.ToUpper();
                    }

                    if (
                        string.Equals(leftFieldValueStr, rightFieldValueStr))
                    {
                        r.Invoke(true);
                        return true;
                    }
                    break;

                case "!=":
                    if (!string.Equals(leftFieldValueStr, rightFieldValueStr))
                    {
                        r.Invoke(true);
                        return true;
                    }
                    break;

                case ">=":
                    if (double.Parse(leftFieldValueStr) >= double.Parse(rightFieldValueStr))
                    {
                        r.Invoke(true);
                        return true;
                    }
                    break;

                case "<=":
                    if (double.Parse(leftFieldValueStr) <= double.Parse(rightFieldValueStr))
                    {
                        r.Invoke(true);
                        return true;
                    }
                    break;

                case ">":
                    if (double.Parse(leftFieldValueStr) > double.Parse(rightFieldValueStr))
                    {
                        r.Invoke(true);
                        return true;
                    }
                    break;

                case "<":
                    if (double.Parse(leftFieldValueStr) < double.Parse(rightFieldValueStr))
                    {
                        r.Invoke(true);
                        return true;
                    }
                    break;
            }

            r.Invoke(false);
            return false;
        }

        private static string GetFieldValueStr(string str)
        {
            try
            {
                var strSpliByDot = str.GetStrsSplitByValue(".Field.");
                var comparedControllerName = strSpliByDot[0];
                var comparedFieldName = strSpliByDot[1];
                var comparedController = comparedControllerName.GetControllerByName(ControllersList);
                var comparedValue = comparedController.GetType()
                    .GetField(comparedFieldName)
                    .GetValue(comparedController).ToString();

                return comparedValue;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        //private Func<double> CalculatedWithOffset(
        //     string value, string offset)
        //{
        //    return () =>
        //    {
        //        var calculated = new DataTable();
        //        var calculateFunc =
        //            new Func<string, double>(formula =>
        //            {
        //                try
        //                {
        //                    return Convert.ToDouble(calculated.Compute(formula, ""));
        //                }
        //                catch (Exception)
        //                {
        //                    return -9999;
        //                }
        //            });

        //        if (string.IsNullOrEmpty(offset))
        //            offset = @"1*X+0";

        //        var lstson = offset.Split('+', '-', '*', '/', '(', ')');
        //        if (lstson[1].ToUpper() == "X")
        //        {
        //            offset = offset.Replace("X", value);
        //            return calculateFunc.Invoke(offset);
        //        }
        //        else
        //        {
        //            foreach (var son in lstson.Where(son => !string.IsNullOrEmpty(son)))
        //            {
        //                int result;
        //                if (int.TryParse(son, out result))
        //                    continue;
        //            }

        //            return calculateFunc.Invoke(offset);
        //        }
        //    };
        //}

        private Func<double> CalculatedWithOffset(
            string processNo, string value, string offset)
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
                        offset = (from p in LstParas.Where(p => p.Name == son1 && p.ProcessNo == processNo)
                                  let pValue = GetFieldValueStr(p.ControllerField)
                                  let pOffset = p.ControllerFieldOffset
                                  select CalculatedWithOffset(processNo, pValue, pOffset).Invoke().ToString(CultureInfo.InvariantCulture))
                            .Aggregate(offset, (current, xxx) => current.Replace(son1, xxx));
                    }

                    return calculateFunc.Invoke(offset);
                }
            };
        }

        public static bool GetStr(string expecedStr, string actualStr, out string resultStr)
        {
            resultStr = string.Empty;

            //var isOk = false;
            //var getStr = actualStr;

            var compareLen = expecedStr.Length;
            if (actualStr.Length < compareLen)
            {
                resultStr = actualStr;
                return false;
            }

            var currentIndex = actualStr.Length - expecedStr.Length;

            while (true)
            {
                if (currentIndex < 0)
                    break;

                var tempStr = actualStr.Substring(currentIndex, compareLen);
                var isCompareOk = true;

                for (var i = 0; i < compareLen; i++)
                {
                    if (expecedStr[i].ToString() == @"?")
                        continue;

                    if (expecedStr[i].ToString() == tempStr[i].ToString())
                        continue;

                    isCompareOk = false;
                    break;
                }

                if (isCompareOk)
                {
                    resultStr = tempStr;
                    return true;
                }

                currentIndex--;
            }

            resultStr = actualStr;
            return false; ;
        }

        public override void Invoke()
        {
            if (ComparativeAction != null)
                ComparativeAction.Invoke();
        }

        private void OnPushCheckData(CheckData checkdata)
        {
            var handler = PushCheckData;
            if (handler != null) handler(checkdata);
        }

        public class RoiResult
        {
            public bool IsInRange { get; set; }
            public Rect Rect { get; set; }
            public string Min { get; set; }
            public string Max { get; set; }
            public string ActualValue { get; set; }
            public int Index { get; set; }
        }

        public class RoiBuffer
        {
            public string ImgName { get; set; }
            public long ImgTs { get; set; }
            public List<RectGray> Rects = new List<RectGray>();
        }

        public class RectGray
        {
            public Rect Rect { get; set; }
            public double Gray { get; set; }
        }

        private bool IsSameRect(double x, double y, double w, double h, Rect fiR)
        {
            if (Math.Abs(x - fiR.X) <= 1 && Math.Abs(y - fiR.Y) <= 1 && Math.Abs(w - fiR.Width) <= 1 && Math.Abs(h - fiR.Height) <= 1)
            {
                return true;
            }

            return false;
        }
    }
}