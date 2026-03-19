using CommonUtility.FileOperator;
using CommonUtility.HikSdk;
using Controller;
using Controller.HardwareController;
using StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CheckSystem.VisionDetection.Vision
{
    public static class VisionCommon
    {
        public static VisionDeviceConfig VisionDeviceConfig =
            XmlHelper.Deserialize<VisionDeviceConfig>(Program.SysDir + @"\图像检测配置文件\CCD_DeviceConfig.CcdDeviceConfig");
        public static CameraControl NiImaqd { get; set; }
        public static bool NiImaqdLoadOk { get; set; }
        public static string FilePath = Program.SysDir + @"\图像检测配置文件\";
        public static VisionConfig VisionConfig { get; set; }
        public static string LoginUser { get; set; }
        public static int OkCount { get; set; }
        public static int TotalCount { get; set; }

        public static bool IsLeft = true;
        public static string Title { get; set; }
        public static Dictionary<string, string> SelectGroups = new Dictionary<string, string>();
        public static Dictionary<string, int> SelectBarcodes = new Dictionary<string, int>();

        //public static Bd18331Euv IcustomController = new Bd18331Euv("BD18331");

        public static Dictionary<string, ICcdPower> ListCcdPowers = new Dictionary<string, ICcdPower>();
        public static BarcodeScanReader BarcodeScanReader = new BarcodeScanReader("扫码枪");
        public static Dictionary<string, ICcdIoController> IoControllers = new Dictionary<string, ICcdIoController>();

        public static Dictionary<string, object> CustomControllers = new Dictionary<string, object>();
        public static Dictionary<string, ControllerBase> CommunicationControllers = new Dictionary<string, ControllerBase>();

        public static Dictionary<string, Func<double>> Volts = new Dictionary<string, Func<double>>();
        public static Dictionary<string, Func<double>> Currs = new Dictionary<string, Func<double>>();
        public static Dictionary<string, Action<bool>> Relays = new Dictionary<string, Action<bool>>();

        public static float PowerV1 = 13.5f;
        public static float PowerV2 = 0f;
        public static float PowerV3 = 5f;
        public static float PowerC1 = 10f;
        public static float PowerC2 = 0f;
        public static float PowerC3 = 1f;
        public static string PowerMode = "无";
        public static bool IsPowerOn;

        public static void LoadDevice()
        {
            if (string.IsNullOrEmpty(LoginUser) &&
                VisionDeviceConfig != null &&
                VisionDeviceConfig.DeviceInfo != null &&
                VisionDeviceConfig.DeviceInfo.DeviceNo != null)
                LoginUser = VisionDeviceConfig.DeviceInfo.DeviceNo;

            if (VisionDeviceConfig != null && VisionDeviceConfig.DeviceInfo != null && VisionDeviceConfig != null && VisionDeviceConfig.DeviceInfo.Controllers.Power != null)
            {
                for (var i = 0; i < VisionDeviceConfig.DeviceInfo.Controllers.Power.Length; i++)
                {
                    var p = VisionDeviceConfig.DeviceInfo.Controllers.Power[i];

                    if (p.Type == "IP6932AS")
                    {
                        var power = new PowerIt6932As(p.Name);
                        power.ConnectPower(p.Address);
                        ListCcdPowers.Add(power.Name, power);
                    }
                    else if (p.Type == "PowerIt6302")
                    {
                        var power = new PowerIt6302(p.Name);
                        power.ConnectPower(p.Address);
                        ListCcdPowers.Add(power.Name, power);
                    }
                    else if (p.Type == "PowerNgi3412E")
                    {
                        var power = new PowerNgi3412E(p.Name);
                        power.ConnectPower(p.Address);
                        ListCcdPowers.Add(power.Name, power);
                    }
                }
            }

            if (VisionConfig.TestFlowInfo != null && VisionConfig.TestFlowInfo.Any())
            {
                PowerSet(VisionConfig.TestFlowInfo[0].TestFlowValue.Split('：')[1], false, true);
            }
            else
            {
                if (VisionConfig.ParaInfo != null && VisionConfig.ParaInfo.Any())
                {
                    PowerSet(VisionConfig.ParaInfo[0].PowerPara, false, true);
                }
            }

            if (VisionDeviceConfig.DeviceInfo.Controllers.BarcodeScaner != null &&
                VisionDeviceConfig.DeviceInfo.Controllers.BarcodeScaner.Any())
            {
                var b = VisionDeviceConfig.DeviceInfo.Controllers.BarcodeScaner[0];
                BarcodeScanReader.ConnectBarcodeScanner(b.Address);
            }

            if (VisionDeviceConfig.DeviceInfo.Controllers.IoController != null)
            {
                foreach (var c in VisionDeviceConfig.DeviceInfo.Controllers.IoController)
                {
                    if (c.Type == "SyControllerWith56Pin")
                    {
                        var controller = new SyControllerWith56Pin(c.Name);
                        controller.InitRemoteIpAddress(c.Address);
                        IoControllers.Add(controller.Name, controller);
                    }
                    else if (c.Type == "SyControllerMaster")
                    {
                        var controller = new SyControllerMaster(c.Name);
                        controller.InitRemoteIpAddr(c.Address);
                        IoControllers.Add(controller.Name, controller);
                    }
                    else if (c.Type == "SyControllerSlaveWith10R")
                    {
                        var controller = new SyControllerSlaveWith10R(c.Name);
                        controller.ConnectToMaster(c.Address.Split(':')[0]);
                        controller.ChangeCanId(c.Address.Split(':')[1]);
                        IoControllers.Add(controller.Name, controller);
                    }
                    else if (c.Type == "SyControllerSlaveWith14Ad")
                    {
                        var controller = new SyControllerSlaveWith14Ad(c.Name);
                        controller.ConnectToMaster(c.Address.Split(':')[0]);
                        controller.ChangeCanId(c.Address.Split(':')[1]);
                        IoControllers.Add(controller.Name, controller);
                    }
                    else if (c.Type == "SyRenesasMcuControllerMaster")
                    {
                        var controller = new SyRenesasMcuControllerMaster(c.Name);
                        controller.InitRemoteIpAddress(c.Address);
                        IoControllers.Add(controller.Name, controller);
                    }
                    else if (c.Type == "SyRenesasMcuControllerSlaveWith8RLs")
                    {
                        var controller = new SyRenesasMcuControllerSlaveWith8RLs(c.Name);
                        controller.ConnectMaster(c.Address.Split(':')[0], c.Address.Split(':')[1]);
                        IoControllers.Add(controller.Name, controller);
                    }
                    else if (c.Type == "SyRenesasMcuControllerSlaveWith12ADs")
                    {
                        var controller = new SyRenesasMcuControllerSlaveWith12ADs(c.Name);
                        controller.ConnectMaster(c.Address.Split(':')[0], c.Address.Split(':')[1]);
                        IoControllers.Add(controller.Name, controller);
                    }
                    else if (c.Type == "ControllerWithGateway")
                    {
                        var controller = new ControllerWithGateway(c.Name);
                        //controller.ConnectMaster(c.Address.Split(':')[0], c.Address.Split(':')[1]);
                        controller.InitRemoteIpAddress(c.Address);
                        IoControllers.Add(controller.Name, controller);
                    }
                }
            }

            foreach (var t in IoControllers)
                t.Value.ResetOutPuts();

            if (VisionDeviceConfig.DeviceInfo.Bidings.Volt != null)
            {
                for (var i = 0; i < VisionDeviceConfig.DeviceInfo.Bidings.Volt.Length; i++)
                {
                    var v = VisionDeviceConfig.DeviceInfo.Bidings.Volt[i];

                    if (IoControllers.ContainsKey(v.Controller))
                    {
                        var v1 = v;
                        var func = new Func<double>(() =>
                        {
                            var c = IoControllers[v1.Controller];
                            c.GetInputs();

                            var field = c.GetType().GetField(v1.Field);
                            if (field != null)
                                return Math.Round(double.Parse(field.GetValue(c).ToString()), 4,
                                    MidpointRounding.AwayFromZero);

                            return -9999;
                        });

                        Volts.Add("电压" + (i + 1), func);
                    }
                }
            }

            if (VisionDeviceConfig.DeviceInfo.Bidings.Curr != null)
            {
                for (int i = 0; i < VisionDeviceConfig.DeviceInfo.Bidings.Curr.Length; i++)
                {
                    var curr = VisionDeviceConfig.DeviceInfo.Bidings.Curr[i];

                    if (IoControllers.ContainsKey(curr.Controller))
                    {
                        var curr1 = curr;
                        var func = new Func<double>(() =>
                        {
                            var c = IoControllers[curr1.Controller];
                            c.GetInputs();

                            var field = c.GetType().GetField(curr1.Field);
                            if (field != null)
                                return Math.Round(double.Parse(field.GetValue(c).ToString()), 4,
                                    MidpointRounding.AwayFromZero);

                            return -9999;
                        });

                        Currs.Add("电流" + (i + 1), func);
                    }
                }
            }

            if (VisionDeviceConfig.DeviceInfo.Bidings.Relay != null)
            {
                for (var i = 0; i < VisionDeviceConfig.DeviceInfo.Bidings.Relay.Length; i++)
                {
                    var r = VisionDeviceConfig.DeviceInfo.Bidings.Relay[i];
                    if (IoControllers.ContainsKey(r.Controller))
                    {
                        var r1 = r;
                        var aciton = new Action<bool>(p =>
                        {
                            var c = IoControllers[r1.Controller];

                            var field = c.GetType().GetField(r1.Field);
                            if (field != null)
                            {
                                field.SetValue(c, p);
                                c.SetOutputs();
                            }
                        });

                        Relays.Add("继电器" + (i + 1), aciton);
                    }
                }
            }

            CommunicationControllers.Add("ZLG", new ZlgUsbCanFd200U("ZLG"));
            CommunicationControllers.Add("Toomoss", new ToomossUsb2XxxCanLin("Toomoss"));

            CustomControllers.Add("CheckApp", new CheckApp("CheckApp"));
            if (VisionConfig != null && VisionConfig.DeviceInfo != null &&
                VisionConfig.DeviceInfo.CustomContrller != null &&
                VisionConfig.DeviceInfo.CustomContrller.Any())
            {
                var dirPath = Program.SysDir;
                var controllerPath = dirPath + @"\Controller.dll";
                var asmb = Assembly.LoadFrom(controllerPath);

                foreach (var custom in VisionConfig.DeviceInfo.CustomContrller)
                {
                    if (!string.IsNullOrEmpty(custom.Type) && !string.IsNullOrEmpty(custom.Name))
                    {
                        var typeName = asmb.GetType("Controller." + custom.Type);
                        if (typeName != null)
                        {
                            var newC = Activator.CreateInstance(typeName, custom.Name);
                            CustomControllers.Add(custom.Name, newC);
                        }
                    }

                    if (!string.IsNullOrEmpty(custom.InitPara))
                    {
                        InvokeCustomCmd(custom.InitPara);
                    }
                }
            }
        }

        public static void PowerSet(string powerValue, bool isOn, bool isInit = false)
        {
            var power = ListCcdPowers.ToList()[0].Value;
            var v1 = PowerV1;
            var v2 = PowerV2;
            var v3 = PowerV3;
            var c1 = PowerC1;
            var c2 = PowerC2;
            var c3 = PowerC3;
            var mode = PowerMode;
            var isPowerOn = IsPowerOn;

            var thisStepPowerPara = string.IsNullOrEmpty(powerValue)
                ? string.Format("电压1={0}V，电流1={1}A，电压2={2}V，电流2={3}A，电压3={4}V，电流3={5}A，串并联模式={6}", v1,
                    c1, v2, c2, v3, v3, mode)
                : powerValue;
            var thisMode =
                thisStepPowerPara.Split('，').ToList().Find(f => f.StartsWith("串并联模式=")).Split('=')[1];
            var thisV1 =
                float.Parse(thisStepPowerPara.Split('，')
                    .ToList()
                    .Find(f => f.StartsWith("电压1="))
                    .Split('=')[1]
                    .Replace("V", ""));
            var thisV2 =
                float.Parse(thisStepPowerPara.Split('，')
                    .ToList()
                    .Find(f => f.StartsWith("电压2="))
                    .Split('=')[1]
                    .Replace("V", ""));
            var thisV3 =
                float.Parse(thisStepPowerPara.Split('，')
                    .ToList()
                    .Find(f => f.StartsWith("电压3="))
                    .Split('=')[1]
                    .Replace("V", ""));
            var thisC1 =
                float.Parse(thisStepPowerPara.Split('，')
                    .ToList()
                    .Find(f => f.StartsWith("电流1="))
                    .Split('=')[1]
                    .Replace("A", ""));
            var thisC2 =
                float.Parse(thisStepPowerPara.Split('，')
                    .ToList()
                    .Find(f => f.StartsWith("电流2="))
                    .Split('=')[1]
                    .Replace("A", ""));
            var thisC3 =
                float.Parse(thisStepPowerPara.Split('，')
                    .ToList()
                    .Find(f => f.StartsWith("电流3="))
                    .Split('=')[1]
                    .Replace("A", ""));

            if (isInit)
            {
                power.PowerOff();
                isPowerOn = false;

                if (thisMode == "无")
                    power.SetCombOff();
                else if (thisMode == "串联")
                    power.SetCombSerOn();
                else if (thisMode == "并联")
                    power.SetCombParaOn();

                power.SetVoltage1(thisV1);
                power.SetVoltage2(thisV2);
                power.SetVoltage3(thisV3);
                power.SetCurrent1(thisC1);
                power.SetCurrent2(thisC2);
                power.SetCurrent3(thisC3);
            }
            else
            {
                if (thisMode != mode)
                {
                    if (isPowerOn)
                    {
                        power.PowerOff();
                        isPowerOn = false;
                    }

                    if (thisMode == "无")
                        power.SetCombOff();
                    else if (thisMode == "串联")
                        power.SetCombSerOn();
                    else if (thisMode == "并联")
                        power.SetCombParaOn();
                }

                if (Math.Abs(thisV1 - v1) > 0.005)
                    power.SetVoltage1(thisV1);
                if (Math.Abs(thisV2 - v2) > 0.005)
                    power.SetVoltage2(thisV2);
                if (Math.Abs(thisV3 - v3) > 0.005)
                    power.SetVoltage3(thisV3);
                if (Math.Abs(thisC1 - c1) > 0.005)
                    power.SetCurrent1(thisC1);
                if (Math.Abs(thisC2 - c2) > 0.005)
                    power.SetCurrent2(thisC2);
                if (Math.Abs(thisC3 - c3) > 0.005)
                    power.SetCurrent3(thisC3);
            }

            if (isOn)
            {
                if (!isPowerOn)
                {
                    power.PowerOn();
                    isPowerOn = true;
                }
            }
            else
            {
                if (isPowerOn)
                {
                    power.PowerOff();
                    isPowerOn = false;
                }
            }

            v1 = thisV1;
            v2 = thisV2;
            v3 = thisV3;
            c1 = thisC1;
            c2 = thisC2;
            c3 = thisC3;
            mode = thisMode;


            PowerV1 = v1;
            PowerV2 = v2;
            PowerV3 = v3;
            PowerC1 = c1;
            PowerC2 = c2;
            PowerC3 = c3;
            PowerMode = mode;
            IsPowerOn = isPowerOn;
        }

        public static void InvokeRelays(VisionConfigParaParaReleysList t,
            bool isReversal = false)
        {
            if (t != null)
            {
                if (t.ParaReleysOnList != null)
                {
                    foreach (var r in t.ParaReleysOnList.Trim().Split(';'))
                    {
                        if (!string.IsNullOrEmpty(r) && r.Trim().StartsWith("继电器"))
                        {
                            var strR = r.Trim();// string.Format("继电器" + r);
                            if (Relays.ContainsKey(strR))
                                Relays[strR].Invoke(!isReversal);
                        }
                    }
                }

                if (t.ParaReleysOffList != null)
                {
                    foreach (var r in t.ParaReleysOffList.Split(','))
                    {
                        if (!string.IsNullOrEmpty(r) && r.Trim().StartsWith("继电器"))
                        {
                            var strR = r.Trim();// string.Format("继电器" + r);
                            if (Relays.ContainsKey(strR))
                                Relays[strR].Invoke(isReversal);
                        }
                    }
                }

                //foreach (var tc in IoControllers)
                //    tc.Value.SetOutputs();

                if (isReversal)
                {
                    //Thread.Sleep(20);
                }
            }
        }

        public static void InvokeCustomCmd(string cmd)
        {
            var strTemp = cmd.StrTrim();

            if (strTemp.Length <= 0)
                return;

            var lstCodeLine = new List<string>(strTemp.TrimEnd(';').Split(';'));

            foreach (var item in lstCodeLine)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    if (item.Contains(".Method."))
                    {
                        InvokeMethods(item);
                    }
                    else
                    {
                        InvokeSetField(item);
                    }
                }
            }
        }

        public static void InvokeMethods(string methods)
        {
            var controllersList = CommunicationControllers.Select(t => t.Value).Cast<object>().ToList();
            controllersList.AddRange(IoControllers.Select(t => t.Value));
            controllersList.AddRange(CustomControllers.Select(t => t.Value));

            foreach (var t in methods.Trim().Split(';'))
            {
                if (!string.IsNullOrEmpty(t))
                {
                    var t1 = t.Trim();

                    var sp = t1.Split(new[] { ".Method." }, StringSplitOptions.RemoveEmptyEntries);

                    var leftController =
                        methods.GetStrsSplitByValue(".Method.")[0].GetControllerByName(controllersList);
                    if (leftController != null)
                    {
                        var methodName = sp[1].Substring(0, sp[1].IndexOf('('));
                        var strParas = sp[1].Substring(sp[1].IndexOf('(') + 1,
                            sp[1].IndexOf(')') - sp[1].IndexOf('(') - 1);
                        var lstParaObjs = new List<object>();
                        var getAllMethods = leftController.GetType().GetMethods();
                        var lstParas = strParas.Split(',').ToList();
                        var paraCount = lstParas.Count;

                        if (paraCount == 1 && lstParas[0].Equals(""))
                            paraCount = 0;
                        else
                        {
                            foreach (
                                var methodPara in
                                    from m in getAllMethods
                                    where m.Name == methodName && m.GetParameters().Length == paraCount
                                    select m.GetParameters())
                                lstParaObjs.AddRange(methodPara.Select((tt, i) => Convert.ChangeType(lstParas[i], tt.ParameterType)));
                        }

                        var firstOrDefault =
                            getAllMethods.Where(m => m.Name == methodName && m.GetParameters().Length == paraCount)
                                .Select(m1 => (Action)(() => m1.Invoke(leftController, lstParaObjs.ToArray())))
                                .FirstOrDefault();
                        if (firstOrDefault != null)
                            firstOrDefault.Invoke();
                    }
                }
            }
        }

        public static void InvokeSetField(string codeStr)
        {
            var controllersList = CommunicationControllers.Select(t => t.Value).Cast<object>().ToList();
            controllersList.AddRange(IoControllers.Select(t => t.Value));
            controllersList.AddRange(CustomControllers.Select(t => t.Value));

            var left = codeStr.GetStrFromLeftSingleEqualitySign();
            var right = codeStr.GetStrFromRightSingleEqualitySign();

            if (string.IsNullOrEmpty(left) || string.IsNullOrEmpty(right))
                return;
            {
                //var leftSplitByDot =left.GetStrsSplitByDot();
                var leftController = new object();
                //FieldInfo leftField;
                var leftFieldName = string.Empty;

                if (left.Contains(".Part."))
                {

                }
                else if (left.Contains(".Field."))
                {
                    leftController = left.GetStrsSplitByValue(".Field.")[0].GetControllerByName(controllersList);
                    //leftField = leftController.GetType().GetField(leftSplitByDot[2]);
                    leftFieldName = left.GetStrsSplitByValue(".Field.")[1];
                }

                //var leftController = leftSplitByDot[0].GetControllerByName(ControllersList);
                //var leftField = leftController.GetType().GetField(leftSplitByDot[2]);
                //var rightSplitByDot = right.GetStrsSplitByDot();

                if ((left.Contains(".Field.") || left.Contains(".Part.")) && right.Contains(".Field."))
                {
                    var rightController = right.GetStrsSplitByValue(".Field.")[0].GetControllerByName(controllersList);

                    if (rightController == null)
                        return;

                    var rightField = rightController.GetType().GetField(right.GetStrsSplitByValue(".Field.")[1]);
                    if (rightField != null)
                    {
                        var rightValue = rightField.GetValue(rightController);

                        try
                        {
                            var leftField = leftController.GetType().GetField(leftFieldName);
                            if (leftField != null)
                            {
                                leftField.SetValue(leftController, rightValue);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                    //leftController.GetType()
                    //    .GetField(leftFieldName)
                    //    .SetValue(leftController, rightField.GetValue(rightController));
                }
                else
                {
                    if (!string.IsNullOrEmpty(leftFieldName) && !string.IsNullOrEmpty(right) && leftController != null)
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
                                if (codeStr.Contains("=") && !codeStr.Contains("+=") && !codeStr.Contains("-=") &&
                                    !codeStr.Contains("*=") && !codeStr.Contains("/="))
                                {
                                    leftValue = () => Convert.ToUInt16(rightValue);
                                }
                                else if (codeStr.Contains("+="))
                                {
                                    leftValue = () => (ushort)(Convert.ToUInt16(rightValue) +
                                                                Convert.ToUInt16(
                                                                    leftController.GetType()
                                                                        .GetField(leftFieldName)
                                                                        .GetValue(leftController)));
                                }
                                else if (codeStr.Contains("-="))
                                {
                                    leftValue = () => (ushort)(Convert.ToUInt16(rightValue) -
                                                                Convert.ToUInt16(
                                                                    leftController.GetType()
                                                                        .GetField(leftFieldName)
                                                                        .GetValue(leftController)));
                                }
                                else if (codeStr.Contains("*="))
                                {
                                    leftValue = () => (ushort)(Convert.ToUInt16(rightValue) *
                                                                Convert.ToUInt16(
                                                                    leftController.GetType()
                                                                        .GetField(leftFieldName)
                                                                        .GetValue(leftController)));
                                }
                                else if (codeStr.Contains("+="))
                                {
                                    leftValue = () => (ushort)(Convert.ToUInt16(rightValue) /
                                                                Convert.ToUInt16(
                                                                    leftController.GetType()
                                                                        .GetField(leftFieldName)
                                                                        .GetValue(leftController)));
                                }

                            }
                            else if (leftFieldType == typeof(float))
                            {
                                //leftValue = Convert.ToSingle(rightValue);

                                if (codeStr.Contains("=") && !codeStr.Contains("+=") && !codeStr.Contains("-=") &&
                                    !codeStr.Contains("*=") && !codeStr.Contains("/="))
                                {
                                    leftValue = () => Convert.ToSingle(rightValue);
                                }
                                else if (codeStr.Contains("+="))
                                {
                                    leftValue = () => Convert.ToSingle(rightValue) +
                                                      Convert.ToSingle(
                                                          leftController.GetType()
                                                              .GetField(leftFieldName)
                                                              .GetValue(leftController));
                                }
                                else if (codeStr.Contains("-="))
                                {
                                    leftValue = () => Convert.ToSingle(rightValue) -
                                                      Convert.ToSingle(
                                                          leftController.GetType()
                                                              .GetField(leftFieldName)
                                                              .GetValue(leftController));
                                }
                                else if (codeStr.Contains("*="))
                                {
                                    leftValue = () => Convert.ToSingle(rightValue) *
                                                      Convert.ToSingle(
                                                          leftController.GetType()
                                                              .GetField(leftFieldName)
                                                              .GetValue(leftController));
                                }
                                else if (codeStr.Contains("/="))
                                {
                                    leftValue = () => Convert.ToSingle(rightValue) /
                                                      Convert.ToSingle(
                                                          leftController.GetType()
                                                              .GetField(leftFieldName)
                                                              .GetValue(leftController));
                                }
                            }
                            else if (leftFieldType == typeof(double))
                            {
                                //leftValue = Convert.ToDouble(rightValue);

                                if (codeStr.Contains("=") && !codeStr.Contains("+=") && !codeStr.Contains("-=") &&
                                    !codeStr.Contains("*=") && !codeStr.Contains("/="))
                                {
                                    leftValue = () => Convert.ToDouble(rightValue);
                                }
                                else if (codeStr.Contains("+="))
                                {
                                    leftValue = () => Convert.ToDouble(rightValue) +
                                                      Convert.ToDouble(
                                                          leftController.GetType()
                                                              .GetField(leftFieldName)
                                                              .GetValue(leftController));
                                }
                                else if (codeStr.Contains("-="))
                                {
                                    leftValue = () => Convert.ToDouble(rightValue) -
                                                      Convert.ToDouble(
                                                          leftController.GetType()
                                                              .GetField(leftFieldName)
                                                              .GetValue(leftController));
                                }
                                else if (codeStr.Contains("*="))
                                {
                                    leftValue = () => Convert.ToDouble(rightValue) *
                                                      Convert.ToDouble(
                                                          leftController.GetType()
                                                              .GetField(leftFieldName)
                                                              .GetValue(leftController));
                                }
                                else if (codeStr.Contains("/="))
                                {
                                    leftValue = () => Convert.ToDouble(rightValue) /
                                                      Convert.ToDouble(
                                                          leftController.GetType()
                                                              .GetField(leftFieldName)
                                                              .GetValue(leftController));
                                }
                            }
                            else if (leftFieldType == typeof(int))
                            {
                                //leftValue = Convert.ToInt32(rightValue);

                                if (codeStr.Contains("=") && !codeStr.Contains("+=") && !codeStr.Contains("-=") &&
                                    !codeStr.Contains("*=") && !codeStr.Contains("/="))
                                {
                                    leftValue = () => Convert.ToInt32(rightValue);
                                }
                                else if (codeStr.Contains("+="))
                                {
                                    leftValue = () => Convert.ToInt32(rightValue) +
                                                      Convert.ToInt32(
                                                          leftController.GetType()
                                                              .GetField(leftFieldName)
                                                              .GetValue(leftController));
                                }
                                else if (codeStr.Contains("-="))
                                {
                                    leftValue = () => Convert.ToInt32(rightValue) -
                                                      Convert.ToInt32(
                                                          leftController.GetType()
                                                              .GetField(leftFieldName)
                                                              .GetValue(leftController));
                                }
                                else if (codeStr.Contains("*="))
                                {
                                    leftValue = () => Convert.ToInt32(rightValue) *
                                                      Convert.ToInt32(
                                                          leftController.GetType()
                                                              .GetField(leftFieldName)
                                                              .GetValue(leftController));
                                }
                                else if (codeStr.Contains("/="))
                                {
                                    leftValue = () => Convert.ToInt32(rightValue) /
                                                      Convert.ToInt32(
                                                          leftController.GetType()
                                                              .GetField(leftFieldName)
                                                              .GetValue(leftController));
                                }
                            }
                            else if (leftFieldType == typeof(byte))
                            {
                                //leftValue = Convert.ToByte(rightValue);

                                if (codeStr.Contains("=") && !codeStr.Contains("+=") && !codeStr.Contains("-=") &&
                                    !codeStr.Contains("*=") && !codeStr.Contains("/="))
                                {
                                    leftValue = () => Convert.ToByte(rightValue);
                                }
                                else if (codeStr.Contains("+="))
                                {
                                    leftValue = () => (byte)(Convert.ToByte(rightValue) +
                                                              Convert.ToByte(
                                                                  leftController.GetType()
                                                                      .GetField(leftFieldName)
                                                                      .GetValue(leftController)));
                                }
                                else if (codeStr.Contains("-="))
                                {
                                    leftValue = () => (byte)(Convert.ToByte(rightValue) -
                                                              Convert.ToByte(
                                                                  leftController.GetType()
                                                                      .GetField(leftFieldName)
                                                                      .GetValue(leftController)));
                                }
                                else if (codeStr.Contains("*="))
                                {
                                    leftValue = () => (byte)(Convert.ToByte(rightValue) *
                                                              Convert.ToByte(
                                                                  leftController.GetType()
                                                                      .GetField(leftFieldName)
                                                                      .GetValue(leftController)));
                                }
                                else if (codeStr.Contains("/="))
                                {
                                    leftValue = () => (byte)(Convert.ToByte(rightValue) /
                                                              Convert.ToByte(
                                                                  leftController.GetType()
                                                                      .GetField(leftFieldName)
                                                                      .GetValue(leftController)));
                                }
                            }
                            else if (leftFieldType == typeof(string))
                            {
                                //leftValue = rightValue;

                                if (codeStr.Contains("=") && !codeStr.Contains("+=") && !codeStr.Contains("-=") &&
                                    !codeStr.Contains("*=") && !codeStr.Contains("/="))
                                {
                                    leftValue = () => rightValue;
                                }
                                else if (codeStr.Contains("+="))
                                {
                                    leftValue = () => string.Format("{0}{1}",
                                        leftController.GetType().GetField(leftFieldName).GetValue(leftController) ??
                                        string.Empty,
                                        rightValue);
                                }
                            }
                        }

                        if (leftValue != null)
                            leftController.GetType()
                                .GetField(leftFieldName)
                                .SetValue(leftController, Convert.ChangeType(leftValue.Invoke(), leftFieldType));
                    }
                }
            }
        }

        public static VisionConfig CreateNewVisionConfig(string prName, string prNo, string prNo2, string leftOrRight)
        {
            var visionConfig = new VisionConfig
            {
                DeviceInfo = new VisionConfigDeviceInfo
                {
                    DeviceGuid = Guid.NewGuid().ToString(),
                    DeviceVersion = "V01",
                    DeviceUpdateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                    DeviceNo = prNo,
                    DeviceNo2 = prNo2,
                    DeviceName = prName,
                    DeviceLeftOrRight = leftOrRight,
                    Actions = new VisionConfigDeviceInfoActions
                    {
                        Start = new VisionConfigDeviceInfoActionsStart { Type = "按键启动" },
                        Bang = new VisionConfigDeviceInfoActionsBang
                        {
                            Delay = "850",
                            IsBang = "True"
                        },
                        PropulsionCylinder = new VisionConfigDeviceInfoActionsPropulsionCylinder
                        {
                            IsPropulsionCylinder = "False",
                            Binding = "继电器12;",
                            Delay = "1000"
                        }
                    },
                    CustomContrller = new VisionConfigDeviceInfoCustomContrller[0]
                },
                ParaInfo = new VisionConfigPara[0],
                TestFlowInfo = new VisionConfigTestFlow[0],
                VisionInfo = new VisionConfigVisionFunc[0],
                BarcodeInfo = new VisionConfigBarcode[0]
            };

            return visionConfig;
        }
    }
}
