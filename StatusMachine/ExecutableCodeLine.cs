using System;
using System.Collections.Generic;
using System.Linq;

namespace StateMachine
{
    public sealed class ExecutableCodeLine : CodeLine
    {
        private Action Executation { get; set; }

        public ExecutableCodeLine(
            string codeLineValue,
            List<object> controllersList,
            DeviceConfig deviceConfig)
            : base(controllersList, deviceConfig)
        {
            CodeStr = codeLineValue;
            Executation = Analyze();
        }

        public override Action Analyze()
        {
            var left = CodeStr.GetStrFromLeftSingleEqualitySign();
            var right = CodeStr.GetStrFromRightSingleEqualitySign();

            if (CodeStr.Contains(".Method."))
            {
                //var leftSplitByDot = CodeStr.Substring(0, CodeStr.IndexOf('(')).GetStrsSplitByDot();
                //var leftController = CodeStr.Substring(0, CodeStr.IndexOf('.')).GetControllerByName(ControllersList);

                var leftSplitByDot =
                    CodeStr.GetStrsSplitByValue(".Method.");
                var leftController =
                    CodeStr.GetStrsSplitByValue(".Method.")[0].GetControllerByName(ControllersList);

                var methodName = leftSplitByDot[1].Substring(0, leftSplitByDot[1].IndexOf('('));
                var strParas = leftSplitByDot[1].Substring(leftSplitByDot[1].IndexOf('(') + 1,
                    leftSplitByDot[1].IndexOf(')') - leftSplitByDot[1].IndexOf('(') - 1);

                var lstParaObjs = new List<object>();

                // 把参数列表分开]
                if (leftController == null)
                    return () => { };

                var getAllMethods = leftController.GetType().GetMethods();
                var lstParas = strParas.Split(',').ToList();
                var paraCount = lstParas.Count;

                //var funcString = new Func<string, string>(valPara =>
                //{
                //    if (valPara.Contains(".Field."))
                //    {
                //        var valParaSplitByDot = valPara.Split('.');
                //        var controller = valParaSplitByDot[0].GetControllerByName(ControllersList);
                //        var rightField = controller.GetType().GetField(valParaSplitByDot[2]);
                //    }

                //    return string.Empty;
                //});

                if (paraCount == 1 && lstParas[0].Equals(""))
                    paraCount = 0;
                else
                {
                    foreach (
                        var methodPara in
                            from m in getAllMethods
                            where m.Name == methodName && m.GetParameters().Length == paraCount
                            select m.GetParameters())
                        lstParaObjs.AddRange(
                            methodPara.Select((t, i) => Convert.ChangeType(lstParas[i], t.ParameterType)));
                }

                return
                    getAllMethods.Where(m => m.Name == methodName && m.GetParameters().Length == paraCount)
                        .Select(m1 => (Action)(() => m1.Invoke(leftController, lstParaObjs.ToArray())))
                        .FirstOrDefault();
            }
            if (string.IsNullOrEmpty(left))
                return null;

            var bIsLeftTypeStringSetEmpty = false;
            if (string.IsNullOrEmpty(right))
            {
                if (left.Contains(".Part."))
                {
                    var part = DeviceConfig.Parts.ToList().Find(f => f.Name == left.GetStrsSplitByValue(".Part.")[1]);

                    if (part == null)
                        return () => { };

                    if (!string.IsNullOrEmpty(part.ControllerField))
                    {
                        var tleftController = part.ControllerField.Split('.')[0].GetControllerByName(ControllersList);
                        var tleftFieldName = part.ControllerField.Split('.')[2];

                        try
                        {
                            if (tleftController.GetType().GetField(tleftFieldName).GetType() != typeof(string))
                            {
                                return null;
                            }
                            else
                            {
                                bIsLeftTypeStringSetEmpty = true;
                            }
                        }
                        catch (Exception)
                        {
                            return null;
                        }
                    }
                }
                else if (left.Contains(".Field."))
                {
                    var tleftController = left.GetStrsSplitByValue(".Field.")[0].GetControllerByName(ControllersList);
                    //leftField = leftController.GetType().GetField(leftSplitByDot[2]);
                    var tleftFieldName = left.GetStrsSplitByValue(".Field.")[1];

                    try
                    {
                        if (tleftController.GetType().GetField(tleftFieldName).FieldType != typeof(string))
                        {
                            return null;
                        }
                        else
                        {
                            bIsLeftTypeStringSetEmpty = true;
                        }
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            }

            {
                //var leftSplitByDot =left.GetStrsSplitByDot();
                var leftController = new object();
                //FieldInfo leftField;
                var leftFieldName = string.Empty;

                if (left.Contains(".Part."))
                {
                    var part = DeviceConfig.Parts.ToList().Find(f => f.Name == left.GetStrsSplitByValue(".Part.")[1]);

                    if (part == null)
                        return () => { };

                    if (!string.IsNullOrEmpty(part.ControllerField))
                    {
                        leftController = part.ControllerField.Split('.')[0].GetControllerByName(ControllersList);
                        leftFieldName = part.ControllerField.Split('.')[2];
                    }
                }
                else if (left.Contains(".Field."))
                {
                    leftController = left.GetStrsSplitByValue(".Field.")[0].GetControllerByName(ControllersList);
                    //leftField = leftController.GetType().GetField(leftSplitByDot[2]);
                    leftFieldName = left.GetStrsSplitByValue(".Field.")[1];
                }

                //var leftController = leftSplitByDot[0].GetControllerByName(ControllersList);
                //var leftField = leftController.GetType().GetField(leftSplitByDot[2]);
                //var rightSplitByDot = right.GetStrsSplitByDot();

                if ((left.Contains(".Field.") || left.Contains(".Part.")) && right.Contains(".Field.") && (CodeStr.Contains("=") && !CodeStr.Contains("+=") && !CodeStr.Contains("-=") && !CodeStr.Contains("*=") && !CodeStr.Contains("/=")))
                {
                    var rightController = right.GetStrsSplitByValue(".Field.")[0].GetControllerByName(ControllersList);

                    if (rightController == null)
                        return () => { };

                    return () =>
                    {
                        var rightField = rightController.GetType().GetField(right.GetStrsSplitByValue(".Field.")[1]);

                        leftController.GetType()
                            .GetField(leftFieldName)
                            .SetValue(leftController, rightField.GetValue(rightController));
                    };

                    //leftField.SetValue(leftController, rightField.GetValue(rightController));
                }
                else
                {
                    if (bIsLeftTypeStringSetEmpty)
                    {
                        return () =>
                        {
                            var leftFieldType = leftController.GetType().GetField(leftFieldName).FieldType;
                            var rightValue = string.Empty;
                            Func<object> leftValue = null;

                            if (CodeStr.Contains("=") && !CodeStr.Contains("+=") && !CodeStr.Contains("-=") && !CodeStr.Contains("*=") && !CodeStr.Contains("/="))
                            {
                                leftValue = () => rightValue;
                            }
                            else if (CodeStr.Contains("+="))
                            {
                                leftValue = () => string.Format("{0}{1}",
                                    leftController.GetType().GetField(leftFieldName).GetValue(leftController) ?? string.Empty,
                                    rightValue);
                            }

                            if (leftValue != null)
                                leftController.GetType()
                                    .GetField(leftFieldName)
                                    .SetValue(leftController, Convert.ChangeType(leftValue.Invoke(), leftFieldType));
                        };
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(leftFieldName) && !string.IsNullOrEmpty(right) && leftController != null)
                        {
                            if (right.Contains(".Field.") || right.Contains(".Part."))
                            {
                                return () =>
                                {
                                    var leftFieldType = leftController.GetType().GetField(leftFieldName).FieldType;
                                    var rightValue = string.Empty;

                                    {
                                        if (right.Contains(".Part."))
                                        {
                                            var part = DeviceConfig.Parts.ToList().Find(f => f.Name == right.GetStrsSplitByValue(".Part.")[1]);

                                            if (part == null)
                                                return;

                                            if (!string.IsNullOrEmpty(part.ControllerField))
                                            {
                                                var rController = part.ControllerField.Split('.')[0].GetControllerByName(ControllersList);
                                                var rFieldName = part.ControllerField.Split('.')[2];

                                                try
                                                {
                                                    var rf = rController.GetType().GetField(rFieldName);
                                                    if (rf.FieldType == typeof(bool))
                                                    {
                                                        var rv = (bool)rf.GetValue(rController);
                                                        if (rv)
                                                        {
                                                            rightValue = "1";
                                                        }
                                                        else
                                                        {
                                                            rightValue = "0";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        rightValue = rf.GetValue(rController) is null ? string.Empty : rf.GetValue(rController).ToString();
                                                    }
                                                }
                                                catch (Exception)
                                                {
                                                    return;
                                                }
                                            }
                                            else
                                            {
                                                return;
                                            }
                                        }
                                        else if (right.Contains(".Field."))
                                        {
                                            var rController = right.GetStrsSplitByValue(".Field.")[0].GetControllerByName(ControllersList);
                                            var rFieldName = right.GetStrsSplitByValue(".Field.")[1];

                                            try
                                            {
                                                var rf = rController.GetType().GetField(rFieldName);
                                                if (rf.FieldType == typeof(bool))
                                                {
                                                    var rv = (bool)rf.GetValue(rController);
                                                    if (rv)
                                                    {
                                                        rightValue = "1";
                                                    }
                                                    else
                                                    {
                                                        rightValue = "0";
                                                    }
                                                }
                                                else
                                                {
                                                    rightValue = rf.GetValue(rController) is null ? string.Empty : rf.GetValue(rController).ToString();
                                                }
                                            }
                                            catch (Exception)
                                            {
                                                return;
                                            }
                                        }
                                    }

                                    Func<object> leftValue = null;

                                    if (leftFieldType == typeof(bool))
                                        leftValue = () => rightValue == "1";
                                    else
                                    {
                                        if (leftFieldType == typeof(ushort))
                                        {
                                            if (CodeStr.Contains("=") && !CodeStr.Contains("+=") && !CodeStr.Contains("-=") && !CodeStr.Contains("*=") && !CodeStr.Contains("/="))
                                            {
                                                leftValue = () => Convert.ToUInt16(rightValue);
                                            }
                                            else if (CodeStr.Contains("+="))
                                            {
                                                leftValue = () => (ushort)(Convert.ToUInt16(rightValue) +
                                                                      Convert.ToUInt16(
                                                                          leftController.GetType().GetField(leftFieldName).GetValue(leftController)));
                                            }
                                            else if (CodeStr.Contains("-="))
                                            {
                                                leftValue = () => (ushort)(Convert.ToUInt16(rightValue) -
                                                                      Convert.ToUInt16(
                                                                          leftController.GetType().GetField(leftFieldName).GetValue(leftController)));
                                            }
                                            else if (CodeStr.Contains("*="))
                                            {
                                                leftValue = () => (ushort)(Convert.ToUInt16(rightValue) *
                                                                     Convert.ToUInt16(
                                                                         leftController.GetType().GetField(leftFieldName).GetValue(leftController)));
                                            }
                                            else if (CodeStr.Contains("+="))
                                            {
                                                leftValue = () => (ushort)(Convert.ToUInt16(rightValue) /
                                                                     Convert.ToUInt16(
                                                                         leftController.GetType().GetField(leftFieldName).GetValue(leftController)));
                                            }

                                        }
                                        else if (leftFieldType == typeof(float))
                                        {
                                            //leftValue = Convert.ToSingle(rightValue);

                                            if (CodeStr.Contains("=") && !CodeStr.Contains("+=") && !CodeStr.Contains("-=") && !CodeStr.Contains("*=") && !CodeStr.Contains("/="))
                                            {
                                                leftValue = () => Convert.ToSingle(rightValue);
                                            }
                                            else if (CodeStr.Contains("+="))
                                            {
                                                leftValue = () => Convert.ToSingle(rightValue) +
                                                            Convert.ToSingle(
                                                                leftController.GetType().GetField(leftFieldName).GetValue(leftController));
                                            }
                                            else if (CodeStr.Contains("-="))
                                            {
                                                leftValue = () => Convert.ToSingle(rightValue) -
                                                            Convert.ToSingle(
                                                                leftController.GetType().GetField(leftFieldName).GetValue(leftController));
                                            }
                                            else if (CodeStr.Contains("*="))
                                            {
                                                leftValue = () => Convert.ToSingle(rightValue) *
                                                            Convert.ToSingle(
                                                                leftController.GetType().GetField(leftFieldName).GetValue(leftController));
                                            }
                                            else if (CodeStr.Contains("/="))
                                            {
                                                leftValue = () => Convert.ToSingle(rightValue) /
                                                            Convert.ToSingle(
                                                                leftController.GetType().GetField(leftFieldName).GetValue(leftController));
                                            }
                                        }
                                        else if (leftFieldType == typeof(double))
                                        {
                                            //leftValue = Convert.ToDouble(rightValue);

                                            if (CodeStr.Contains("=") && !CodeStr.Contains("+=") && !CodeStr.Contains("-=") && !CodeStr.Contains("*=") && !CodeStr.Contains("/="))
                                            {
                                                leftValue = () => Convert.ToDouble(rightValue);
                                            }
                                            else if (CodeStr.Contains("+="))
                                            {
                                                leftValue = () => Convert.ToDouble(rightValue) +
                                                            Convert.ToDouble(
                                                                leftController.GetType().GetField(leftFieldName).GetValue(leftController));
                                            }
                                            else if (CodeStr.Contains("-="))
                                            {
                                                leftValue = () => Convert.ToDouble(rightValue) -
                                                            Convert.ToDouble(
                                                                leftController.GetType().GetField(leftFieldName).GetValue(leftController));
                                            }
                                            else if (CodeStr.Contains("*="))
                                            {
                                                leftValue = () => Convert.ToDouble(rightValue) *
                                                            Convert.ToDouble(
                                                                leftController.GetType().GetField(leftFieldName).GetValue(leftController));
                                            }
                                            else if (CodeStr.Contains("/="))
                                            {
                                                leftValue = () => Convert.ToDouble(rightValue) /
                                                            Convert.ToDouble(
                                                                leftController.GetType().GetField(leftFieldName).GetValue(leftController));
                                            }
                                        }
                                        else if (leftFieldType == typeof(int))
                                        {
                                            //leftValue = Convert.ToInt32(rightValue);

                                            if (CodeStr.Contains("=") && !CodeStr.Contains("+=") && !CodeStr.Contains("-=") && !CodeStr.Contains("*=") && !CodeStr.Contains("/="))
                                            {
                                                leftValue = () => Convert.ToInt32(rightValue);
                                            }
                                            else if (CodeStr.Contains("+="))
                                            {
                                                leftValue = () => Convert.ToInt32(rightValue) +
                                                            Convert.ToInt32(
                                                                leftController.GetType().GetField(leftFieldName).GetValue(leftController));
                                            }
                                            else if (CodeStr.Contains("-="))
                                            {
                                                leftValue = () => Convert.ToInt32(rightValue) -
                                                            Convert.ToInt32(
                                                                leftController.GetType().GetField(leftFieldName).GetValue(leftController));
                                            }
                                            else if (CodeStr.Contains("*="))
                                            {
                                                leftValue = () => Convert.ToInt32(rightValue) *
                                                            Convert.ToInt32(
                                                                leftController.GetType().GetField(leftFieldName).GetValue(leftController));
                                            }
                                            else if (CodeStr.Contains("/="))
                                            {
                                                leftValue = () => Convert.ToInt32(rightValue) /
                                                            Convert.ToInt32(
                                                                leftController.GetType().GetField(leftFieldName).GetValue(leftController));
                                            }
                                        }
                                        else if (leftFieldType == typeof(byte))
                                        {
                                            //leftValue = Convert.ToByte(rightValue);

                                            if (CodeStr.Contains("=") && !CodeStr.Contains("+=") && !CodeStr.Contains("-=") && !CodeStr.Contains("*=") && !CodeStr.Contains("/="))
                                            {
                                                leftValue = () => Convert.ToByte(rightValue);
                                            }
                                            else if (CodeStr.Contains("+="))
                                            {
                                                leftValue = () => (byte)(Convert.ToByte(rightValue) +
                                                                    Convert.ToByte(
                                                                        leftController.GetType().GetField(leftFieldName).GetValue(leftController)));
                                            }
                                            else if (CodeStr.Contains("-="))
                                            {
                                                leftValue = () => (byte)(Convert.ToByte(rightValue) -
                                                                    Convert.ToByte(
                                                                        leftController.GetType().GetField(leftFieldName).GetValue(leftController)));
                                            }
                                            else if (CodeStr.Contains("*="))
                                            {
                                                leftValue = () => (byte)(Convert.ToByte(rightValue) *
                                                                    Convert.ToByte(
                                                                        leftController.GetType().GetField(leftFieldName).GetValue(leftController)));
                                            }
                                            else if (CodeStr.Contains("/="))
                                            {
                                                leftValue = () => (byte)(Convert.ToByte(rightValue) /
                                                                    Convert.ToByte(
                                                                        leftController.GetType().GetField(leftFieldName).GetValue(leftController)));
                                            }
                                        }
                                        else if (leftFieldType == typeof(string))
                                        {
                                            //leftValue = rightValue;

                                            if (CodeStr.Contains("=") && !CodeStr.Contains("+=") && !CodeStr.Contains("-=") && !CodeStr.Contains("*=") && !CodeStr.Contains("/="))
                                            {
                                                leftValue = () => rightValue;
                                            }
                                            else if (CodeStr.Contains("+="))
                                            {
                                                leftValue = () => string.Format("{0}{1}",
                                                    leftController.GetType().GetField(leftFieldName).GetValue(leftController) ?? string.Empty,
                                                    rightValue);
                                            }
                                        }
                                    }

                                    if (leftValue != null)
                                        leftController.GetType()
                                            .GetField(leftFieldName)
                                            .SetValue(leftController, Convert.ChangeType(leftValue.Invoke(), leftFieldType));
                                };
                            }
                            else
                            {
                                return () =>
                                {
                                    var leftFieldType = leftController.GetType().GetField(leftFieldName).FieldType;
                                    var rightValue = right;
                                    Func<object> leftValue = null;

                                    if (leftFieldType == typeof(bool))
                                        leftValue = () => rightValue == "1";
                                    else
                                    {
                                        if (leftFieldType == typeof(ushort))
                                        {
                                            if (CodeStr.Contains("=") && !CodeStr.Contains("+=") && !CodeStr.Contains("-=") && !CodeStr.Contains("*=") && !CodeStr.Contains("/="))
                                            {
                                                leftValue = () => Convert.ToUInt16(rightValue);
                                            }
                                            else if (CodeStr.Contains("+="))
                                            {
                                                leftValue = () => (ushort)(Convert.ToUInt16(rightValue) +
                                                                      Convert.ToUInt16(
                                                                          leftController.GetType().GetField(leftFieldName).GetValue(leftController)));
                                            }
                                            else if (CodeStr.Contains("-="))
                                            {
                                                leftValue = () => (ushort)(Convert.ToUInt16(rightValue) -
                                                                      Convert.ToUInt16(
                                                                          leftController.GetType().GetField(leftFieldName).GetValue(leftController)));
                                            }
                                            else if (CodeStr.Contains("*="))
                                            {
                                                leftValue = () => (ushort)(Convert.ToUInt16(rightValue) *
                                                                     Convert.ToUInt16(
                                                                         leftController.GetType().GetField(leftFieldName).GetValue(leftController)));
                                            }
                                            else if (CodeStr.Contains("+="))
                                            {
                                                leftValue = () => (ushort)(Convert.ToUInt16(rightValue) /
                                                                     Convert.ToUInt16(
                                                                         leftController.GetType().GetField(leftFieldName).GetValue(leftController)));
                                            }

                                        }
                                        else if (leftFieldType == typeof(float))
                                        {
                                            //leftValue = Convert.ToSingle(rightValue);

                                            if (CodeStr.Contains("=") && !CodeStr.Contains("+=") && !CodeStr.Contains("-=") && !CodeStr.Contains("*=") && !CodeStr.Contains("/="))
                                            {
                                                leftValue = () => Convert.ToSingle(rightValue);
                                            }
                                            else if (CodeStr.Contains("+="))
                                            {
                                                leftValue = () => Convert.ToSingle(rightValue) +
                                                            Convert.ToSingle(
                                                                leftController.GetType().GetField(leftFieldName).GetValue(leftController));
                                            }
                                            else if (CodeStr.Contains("-="))
                                            {
                                                leftValue = () => Convert.ToSingle(rightValue) -
                                                            Convert.ToSingle(
                                                                leftController.GetType().GetField(leftFieldName).GetValue(leftController));
                                            }
                                            else if (CodeStr.Contains("*="))
                                            {
                                                leftValue = () => Convert.ToSingle(rightValue) *
                                                            Convert.ToSingle(
                                                                leftController.GetType().GetField(leftFieldName).GetValue(leftController));
                                            }
                                            else if (CodeStr.Contains("/="))
                                            {
                                                leftValue = () => Convert.ToSingle(rightValue) /
                                                            Convert.ToSingle(
                                                                leftController.GetType().GetField(leftFieldName).GetValue(leftController));
                                            }
                                        }
                                        else if (leftFieldType == typeof(double))
                                        {
                                            //leftValue = Convert.ToDouble(rightValue);

                                            if (CodeStr.Contains("=") && !CodeStr.Contains("+=") && !CodeStr.Contains("-=") && !CodeStr.Contains("*=") && !CodeStr.Contains("/="))
                                            {
                                                leftValue = () => Convert.ToDouble(rightValue);
                                            }
                                            else if (CodeStr.Contains("+="))
                                            {
                                                leftValue = () => Convert.ToDouble(rightValue) +
                                                            Convert.ToDouble(
                                                                leftController.GetType().GetField(leftFieldName).GetValue(leftController));
                                            }
                                            else if (CodeStr.Contains("-="))
                                            {
                                                leftValue = () => Convert.ToDouble(rightValue) -
                                                            Convert.ToDouble(
                                                                leftController.GetType().GetField(leftFieldName).GetValue(leftController));
                                            }
                                            else if (CodeStr.Contains("*="))
                                            {
                                                leftValue = () => Convert.ToDouble(rightValue) *
                                                            Convert.ToDouble(
                                                                leftController.GetType().GetField(leftFieldName).GetValue(leftController));
                                            }
                                            else if (CodeStr.Contains("/="))
                                            {
                                                leftValue = () => Convert.ToDouble(rightValue) /
                                                            Convert.ToDouble(
                                                                leftController.GetType().GetField(leftFieldName).GetValue(leftController));
                                            }
                                        }
                                        else if (leftFieldType == typeof(int))
                                        {
                                            //leftValue = Convert.ToInt32(rightValue);

                                            if (CodeStr.Contains("=") && !CodeStr.Contains("+=") && !CodeStr.Contains("-=") && !CodeStr.Contains("*=") && !CodeStr.Contains("/="))
                                            {
                                                leftValue = () => Convert.ToInt32(rightValue);
                                            }
                                            else if (CodeStr.Contains("+="))
                                            {
                                                leftValue = () => Convert.ToInt32(rightValue) +
                                                            Convert.ToInt32(
                                                                leftController.GetType().GetField(leftFieldName).GetValue(leftController));
                                            }
                                            else if (CodeStr.Contains("-="))
                                            {
                                                leftValue = () => Convert.ToInt32(rightValue) -
                                                            Convert.ToInt32(
                                                                leftController.GetType().GetField(leftFieldName).GetValue(leftController));
                                            }
                                            else if (CodeStr.Contains("*="))
                                            {
                                                leftValue = () => Convert.ToInt32(rightValue) *
                                                            Convert.ToInt32(
                                                                leftController.GetType().GetField(leftFieldName).GetValue(leftController));
                                            }
                                            else if (CodeStr.Contains("/="))
                                            {
                                                leftValue = () => Convert.ToInt32(rightValue) /
                                                            Convert.ToInt32(
                                                                leftController.GetType().GetField(leftFieldName).GetValue(leftController));
                                            }
                                        }
                                        else if (leftFieldType == typeof(byte))
                                        {
                                            //leftValue = Convert.ToByte(rightValue);

                                            if (CodeStr.Contains("=") && !CodeStr.Contains("+=") && !CodeStr.Contains("-=") && !CodeStr.Contains("*=") && !CodeStr.Contains("/="))
                                            {
                                                leftValue = () => Convert.ToByte(rightValue);
                                            }
                                            else if (CodeStr.Contains("+="))
                                            {
                                                leftValue = () => (byte)(Convert.ToByte(rightValue) +
                                                                    Convert.ToByte(
                                                                        leftController.GetType().GetField(leftFieldName).GetValue(leftController)));
                                            }
                                            else if (CodeStr.Contains("-="))
                                            {
                                                leftValue = () => (byte)(Convert.ToByte(rightValue) -
                                                                    Convert.ToByte(
                                                                        leftController.GetType().GetField(leftFieldName).GetValue(leftController)));
                                            }
                                            else if (CodeStr.Contains("*="))
                                            {
                                                leftValue = () => (byte)(Convert.ToByte(rightValue) *
                                                                    Convert.ToByte(
                                                                        leftController.GetType().GetField(leftFieldName).GetValue(leftController)));
                                            }
                                            else if (CodeStr.Contains("/="))
                                            {
                                                leftValue = () => (byte)(Convert.ToByte(rightValue) /
                                                                    Convert.ToByte(
                                                                        leftController.GetType().GetField(leftFieldName).GetValue(leftController)));
                                            }
                                        }
                                        else if (leftFieldType == typeof(string))
                                        {
                                            //leftValue = rightValue;

                                            if (CodeStr.Contains("=") && !CodeStr.Contains("+=") && !CodeStr.Contains("-=") && !CodeStr.Contains("*=") && !CodeStr.Contains("/="))
                                            {
                                                leftValue = () => rightValue;
                                            }
                                            else if (CodeStr.Contains("+="))
                                            {
                                                leftValue = () => string.Format("{0}{1}",
                                                    leftController.GetType().GetField(leftFieldName).GetValue(leftController) ?? string.Empty,
                                                    rightValue);
                                            }
                                        }
                                    }

                                    if (leftValue != null)
                                        leftController.GetType()
                                            .GetField(leftFieldName)
                                            .SetValue(leftController, Convert.ChangeType(leftValue.Invoke(), leftFieldType));
                                };
                            }
                        }
                    }

                    return () => { };
                    //leftField.SetValue(leftController, Convert.ChangeType(leftValue, leftFieldType));
                }
            }
        }

        public override Action Analyze(object obj)
        {
            throw new NotImplementedException();
        }

        public override void Invoke()
        {
            if (Executation != null)
                Executation.Invoke();
        }
    }
}
