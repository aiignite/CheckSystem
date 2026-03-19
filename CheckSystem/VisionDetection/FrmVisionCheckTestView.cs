using CheckSystem.VisionDetection.Control;
using CheckSystem.VisionDetection.Vision;
using CommonUtility;
using CommonUtility.FileOperator;
using Controller;
using Newtonsoft.Json;
using StateMachine;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckSystem.VisionDetection
{
    public sealed partial class FrmVisionCheckTestView : UIPage
    {
        private readonly CheckApp _checkApp = new CheckApp("CheckApp");
        private readonly Dictionary<string, DynamicImageViewer> _dynamicImageViewers = new Dictionary<string, DynamicImageViewer>();
        private readonly Dictionary<string, StaticImageViewer> _staticImageViewers = new Dictionary<string, StaticImageViewer>();

        /// <summary>
        /// 当前窗体的宽度
        /// </summary>
        private float _x;

        /// <summary>
        /// 当前窗体的高度
        /// </summary>
        private float _y;

        public FrmVisionCheckTestView(bool isLoad)
        {
            InitializeComponent();

            if (!isLoad)
                return;
            ToRunVisionConfig = XmlHelper.Deserialize<VisionConfig>(VisionCommon.FilePath);

            if (ToRunVisionConfig == null)
                return;
            GetCount();
            InitUi();
            InitMainDgv();

            Load += FrmVisionCheckTestView_Load;
            Closed += FrmVisionCheckTestView_Closed;
        }

        private Thread BackgroundWorker { get; set; }
        private Thread DeviceBackgroundWorker { get; set; }

        /// <summary>
        /// 0或1为界面手动启动，0为ByPass，1为NoByPass，2为正常启动
        /// </summary>
        private int IsByPass { get; set; }

        private bool IsChecking { get; set; }

        public VisionConfig ToRunVisionConfig { get; set; }

        public bool CheckLogicAd(VisionConfigPara t)
        {
            var binding = t.ParaBinding;
            var min = double.Parse(t.ParaGroups[0].ParaGroupMin);
            var max = double.Parse(t.ParaGroups[0].ParaGroupMax);
            var k = double.Parse(t.ParaGroups[0].ParaGroupK);
            var b = double.Parse(t.ParaGroups[0].ParaGroupB);
            if (VisionCommon.SelectGroups.Any())
            {
                if (VisionCommon.SelectGroups.ContainsKey(t.ParaName))
                {
                    var g = VisionCommon.SelectGroups.Keys.First(f => f == t.ParaName);
                    {
                        var gp =
                            t.ParaGroups.ToList().Find(f => f.ParaGroupName == VisionCommon.SelectGroups[g]);
                        min = double.Parse(gp.ParaGroupMin);
                        max = double.Parse(gp.ParaGroupMax);
                        k = double.Parse(gp.ParaGroupK);
                        b = double.Parse(gp.ParaGroupB);
                    }
                }
                else
                {
                    if (t.ParaGroups.ToList().Any())
                    {
                        var gp = t.ParaGroups.ToList()[0];
                        min = double.Parse(gp.ParaGroupMin);
                        max = double.Parse(gp.ParaGroupMax);
                        k = double.Parse(gp.ParaGroupK);
                        b = double.Parse(gp.ParaGroupB);
                    }
                }
            }
            else
            {
                if (t.ParaGroups.ToList().Any())
                {
                    var gp = t.ParaGroups.ToList()[0];
                    min = double.Parse(gp.ParaGroupMin);
                    max = double.Parse(gp.ParaGroupMax);
                    k = double.Parse(gp.ParaGroupK);
                    b = double.Parse(gp.ParaGroupB);
                }
            }

            double ad = -9999;

            if (binding.StartsWith("电压"))
            {
                if (VisionCommon.Volts.ContainsKey(binding))
                    ad = VisionCommon.Volts[binding].Invoke();
            }
            else if (binding.StartsWith("电流"))
            {
                if (VisionCommon.Currs.ContainsKey(binding))
                    ad = VisionCommon.Currs[binding].Invoke();
            }
            else if (binding.StartsWith("电源电流"))
            {
                try
                {
                    var adIndex = int.Parse(binding.Replace("电源电流", ""));
                    var power = VisionCommon.ListCcdPowers.ToList()[0].Value;
                    power.ReadCurrAndVolt();
                    if (power is PowerIt6932As)
                        ad = ((PowerIt6932As)power).CurrenttRead;
                    else if (power is PowerIt6302)
                    {
                        var field = power.GetType().GetField("CurreatRead" + adIndex);
                        if (field != null)
                            ad = double.Parse(field.GetValue(power).ToString());
                    }
                    else if (power is PowerNgi3412E)
                    {
                        var field = power.GetType().GetField("Curr" + adIndex);
                        if (field != null)
                            ad = double.Parse(field.GetValue(power).ToString());
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            if (t.ParaType.Contains("电阻"))
                ad = k * ad / (b - ad);
            else
                ad = ad * k + b;

            ad = Math.Round(ad, 2, MidpointRounding.AwayFromZero);
            var isOk = ad >= min && ad <= max;

            if (t.ParaType.Contains("电阻"))
                UpdateMainDgv(t.ParaName, "电阻", ad.ToString(CultureInfo.InvariantCulture), t.ParaUnit,
                    isOk ? "OK" : "NG");
            else
                UpdateMainDgv(t.ParaName, "电性能", ad.ToString(CultureInfo.InvariantCulture), t.ParaUnit,
                    isOk ? "OK" : "NG");

            return isOk;
        }

        public bool CheckLogicDynamicImgs(VisionConfigPara t)
        {
            var visionImgViewer = _dynamicImageViewers[t.ParaName];

            for (var i = 0; i < imgTabControl.TabPages.Count; i++)
            {
                var page = imgTabControl.TabPages[i];
                if (page == null || page.Text != t.ParaName)
                    continue;
                Invoke(new Action(() =>
                {
                    imgTabControl.SelectedIndex = i;
                }));
                break;
            }

            var func = ToRunVisionConfig.VisionInfo.ToList().Find(f => f.VisionFuncName == t.ParaName);
            var isOk = visionImgViewer.DoWork(func, t, VisionCommon.IsLeft);
            var lrVisionFunc = VisionCommon.IsLeft ? func.VisionFuncDetailL[0] : func.VisionFuncDetailR[0];
            var device =
                  VisionCommon.NiImaqd.CameraList.Find(
                      f => f.GigeInfo.chSerialNumber == lrVisionFunc.UserId);

            if (!isOk)
            {
                Invoke(new Action(() =>
                {
                    visionImgViewer.Reset();
                }));
                isOk = visionImgViewer.DoWork(func, t, VisionCommon.IsLeft);
            }

            if (device != null)
            {
                device.ClearBuffer();
                device.CloseCamera();
            }

            UpdateMainDgv(t.ParaName, "动态图像", "见右表", "", isOk ? "OK" : "NG");
            visionImgViewer.ReleaseImg();
            return isOk;
        }

        public bool CheckLogicStaticImg(VisionConfigPara t)
        {
            var visionImgViewer = _staticImageViewers[t.ParaName];

            for (var i = 0; i < imgTabControl.TabPages.Count; i++)
            {
                var page = imgTabControl.TabPages[i];
                if (page == null || page.Text != t.ParaName)
                    continue;
                Invoke(new Action(() =>
                {
                    imgTabControl.SelectedIndex = i;
                }));
                break;
            }
            var func = ToRunVisionConfig.VisionInfo.ToList().Find(f => f.VisionFuncName == t.ParaName);

            var isOk = visionImgViewer.DoWork(func, VisionCommon.IsLeft);

            if (!isOk)
            {
                Invoke(new Action(() =>
                {
                    visionImgViewer.Reset();
                }));
                isOk = visionImgViewer.DoWork(func, VisionCommon.IsLeft);
            }

            UpdateMainDgv(t.ParaName, "静态图像", "见右表", "", isOk ? "OK" : "NG");
            return isOk;
        }

        public bool CheckLogicVersion(VisionConfigPara vcp, bool isLike)
        {
            var binding = vcp.ParaBinding;

            var controllerName = binding.Split('.')[0];
            var fieldName = binding.Split('.')[1];

            var controllersList = VisionCommon.CommunicationControllers.Select(t => t.Value).Cast<object>().ToList();
            controllersList.AddRange(VisionCommon.IoControllers.Select(t => t.Value));
            controllersList.AddRange(VisionCommon.CustomControllers.Select(t => t.Value));

            var fieldValue = string.Empty;

            var controller = controllerName.GetControllerByName(controllersList);
            if (controller != null)
            {
                var getField = controller.GetType().GetField(fieldName);
                if (getField != null)
                {
                    var getFieldValue = getField.GetValue(controller);
                    if (getFieldValue != null)
                        fieldValue = getField.GetValue(controller).ToString();
                }
            }

            var expectedValue = vcp.ParaGroups[0].ParaGroupValue;
            if (VisionCommon.SelectGroups.Any())
            {
                if (VisionCommon.SelectGroups.ContainsKey(vcp.ParaName))
                {
                    var g = VisionCommon.SelectGroups.Keys.First(f => f == vcp.ParaName);
                    {
                        var gp =
                            vcp.ParaGroups.ToList().Find(f => f.ParaGroupName == VisionCommon.SelectGroups[g]);
                        expectedValue = gp.ParaGroupValue;
                    }
                }
                else
                {
                    if (vcp.ParaGroups.ToList().Any())
                    {
                        var gp = vcp.ParaGroups.ToList()[0];
                        expectedValue = gp.ParaGroupValue;
                    }
                }
            }
            else
            {
                if (vcp.ParaGroups.ToList().Any())
                {
                    var gp = vcp.ParaGroups.ToList()[0];
                    expectedValue = gp.ParaGroupValue;
                }
            }

            if (vcp.ParaGroups != null &&
                vcp.ParaGroups.Any() &&
                !string.IsNullOrEmpty(expectedValue))
            {
                if (isLike)
                {
                    if (fieldValue.Contains(expectedValue))
                    {
                        UpdateMainDgv(vcp.ParaName, "信息读取（like）", fieldValue, "", "OK");
                        return true;
                    }

                    UpdateMainDgv(vcp.ParaName, "信息读取（like）", fieldValue, "", "NG");
                    return false;
                }
                else
                {
                    if (fieldValue.Equals(expectedValue))
                    {
                        UpdateMainDgv(vcp.ParaName, "信息读取（==）", fieldValue, "", "OK");
                        return true;
                    }

                    UpdateMainDgv(vcp.ParaName, "信息读取（==）", fieldValue, "", "NG");
                    return false;
                }
            }

            UpdateMainDgv(vcp.ParaName, isLike ? "信息读取（like）" : "信息读取（==）", fieldValue, "", "NG");
            return false;
        }

        public void InitUi()
        {
            UIStyles.InitColorful(Color.FromArgb(80, 126, 164), Color.White);
            uiTitlePanel1.Text = GetTotalCountShowString();//@" 测试汇总：【日期：" + DateTime.Now.ToString("yyyy/MM/dd") + @"，OK：" + VisionCommon.OkCount + @"/总：" + VisionCommon.TotalCount + @"】";
            uiMarkLabel1.Text = string.Format("测试时间：等待开始\r\n测试结果：等待开始\r\n测试耗时：等待开始");
            uiLedBulb1.Color = Color.DarkGoldenrod;
            //InitMainDgv();

            Text = @"测试页面 ";
            Text = string.Empty;
            if (VisionCommon.SelectGroups.Any())
            {
                var dicBin = new Dictionary<string, List<string>>();

                foreach (var t in VisionCommon.SelectGroups.Keys)
                {
                    var bin = VisionCommon.SelectGroups[t].ToUpper();
                    if (dicBin.ContainsKey(bin))
                    {
                        dicBin[bin].Add(t);
                    }
                    else
                    {
                        dicBin.Add(bin, new List<string>());
                        dicBin[bin].Add(t);
                    }

                    //Text += string.Format("[{0}:{1}]", t, VisionCommon.SelectGroups[t]);
                    //Text += @"：" + string.Format("{0}：{1}", t, VisionCommon.SelectGroups[t]);
                }


                var lineCount = 0;
                foreach (var key in dicBin.Keys)
                {
                    var bin = key;
                    var names = dicBin[key].Aggregate(string.Empty, (current, name) => current + name + "/").TrimEnd('/');
                    Text += string.Format(@"【{0}：{1}档】  ", names, bin);

                    if (Text.Length > 175 && lineCount == 0)
                    {
                        Text = Text.TrimEnd(' ');
                        Text += Environment.NewLine;
                        lineCount++;
                    }
                }

                //foreach (var t in listShowBin)
                //    Text += string.Format(@"[{0}],", t);

                Text = Text.TrimEnd(' ');
            }
        }

        public bool InvokeParaAction(VisionConfigPara t)
        {
            var ngCount = 0;
            var stEnter = HighPrecisionTimer.GetTimestamp();
            var checkCount = 0;

            if (t.ParaType == "静态图像" || t.ParaType == "电性能，静态图像" || t.ParaType == "电性能" || t.ParaType == "电阻")
            {
                var listTasks = new List<Task>();

                var ledTask = Task.Factory.StartNew(() =>
                {
                    VisionCommon.InvokeRelays(t.ParaReleysList);
                    if (t.ParaMethods != null)
                        VisionCommon.InvokeCustomCmd(t.ParaMethods.ParaMethodsBefore);

                    if (!string.IsNullOrEmpty(t.ParaDelayMs) || t.ParaDelayMs != "/" ||
                        int.Parse(t.ParaDelayMs) > 0)
                        Thread.Sleep(int.Parse(t.ParaDelayMs));
                });
                listTasks.Add(ledTask);

                if (t.ParaType.Contains("静态图像"))
                {
                    var cameraTask = Task.Factory.StartNew(() =>
                    {
                        var func = ToRunVisionConfig.VisionInfo.ToList().Find(f => f.VisionFuncName == t.ParaName);
                        var temp = VisionCommon.IsLeft ? func.VisionFuncDetailL[0] : func.VisionFuncDetailR[0];

                        var device =
                            VisionCommon.NiImaqd.CameraList.Find(
                                f => f.GigeInfo.chSerialNumber == temp.UserId);
                        if (device == null)
                            return;

                        device.OpenCamera();
                        var allOther =
                            VisionCommon.NiImaqd.CameraList.FindAll(
                                f => f.GigeInfo.chSerialNumber != device.GigeInfo.chSerialNumber);
                    });
                    listTasks.Add(cameraTask);
                }

                Task.WaitAll(listTasks.ToArray());

                if (t.ParaType.EndsWith("静态图像"))
                {
                    checkCount++;
                    if (!CheckLogicStaticImg(t))
                        ngCount++;
                }

                if (t.ParaType.StartsWith("电性能") || t.ParaType.StartsWith("电阻"))
                {
                    checkCount++;
                    if (!CheckLogicAd(t))
                    {
                        Thread.Sleep(50);
                        if (!CheckLogicAd(t))
                            ngCount++;
                    }
                }

                VisionCommon.InvokeRelays(t.ParaReleysList, true);

                if (t.ParaMethods != null)
                    VisionCommon.InvokeCustomCmd(t.ParaMethods.ParaMethodsAfter);
            }
            else if (t.ParaType == "动态图像")
            {
                checkCount++;
                if (!CheckLogicDynamicImgs(t))
                    ngCount++;
            }
            else if (t.ParaType.StartsWith("信息读取"))
            {
                checkCount++;
                VisionCommon.InvokeRelays(t.ParaReleysList);

                if (t.ParaMethods != null)
                    VisionCommon.InvokeCustomCmd(t.ParaMethods.ParaMethodsBefore);

                if (!string.IsNullOrEmpty(t.ParaDelayMs) || t.ParaDelayMs != "/" ||
                    int.Parse(t.ParaDelayMs) > 0)
                    Thread.Sleep(int.Parse(t.ParaDelayMs));

                if (t.ParaType.EndsWith("（==）"))
                {
                    if (!CheckLogicVersion(t, false))
                        ngCount++;
                }
                else if (t.ParaType.EndsWith("（like）"))
                {
                    if (!CheckLogicVersion(t, true))
                        ngCount++;
                }

                VisionCommon.InvokeRelays(t.ParaReleysList, true);

                if (t.ParaMethods != null)
                    VisionCommon.InvokeCustomCmd(t.ParaMethods.ParaMethodsAfter);
            }

            var stEnd = HighPrecisionTimer.GetTimestamp();
            var ms = HighPrecisionTimer.GetTimestampIntervalMs(stEnter, stEnd) / checkCount;

            var spType = t.ParaType.Split('，');
            foreach (var item in spType)
            {
                var itemNew = item.Trim(' ');
                UpdateMainDgv(t.ParaName, itemNew, ms);
            }

            return ngCount <= 0;
        }

        public void ReInitMainDgv()
        {
            mainDgv.Style = UIStyle.Gray;
            mainDgv.ReadOnly = true;
            mainDgv.RowHeadersVisible = false;
            mainDgv.AllowUserToAddRows = false;
            mainDgv.AllowUserToResizeRows = false;
            mainDgv.MultiSelect = true;
            mainDgv.RowHeadersVisible = false;
            mainDgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            mainDgv.ClearAll();
            mainDgv.AddColumn("名称", "名称");
            mainDgv.AddColumn("类型", "类型");
            mainDgv.AddColumn("标准", "标准");
            mainDgv.AddColumn("测试值", "测试值");
            mainDgv.AddColumn("结果", "结果");
            //mainDgv.AddColumn("耗时", "耗时");

            foreach (var t in ToRunVisionConfig.ParaInfo)
            {
                if (VisionCommon.VisionConfig.DeviceInfo.DeviceLeftOrRight != null &&
                    string.Equals(VisionCommon.VisionConfig.DeviceInfo.DeviceLeftOrRight, @"不区分",
                        StringComparison.CurrentCultureIgnoreCase))
                {
                    GenerateDgvRows(t);
                }
                else
                {
                    if (VisionCommon.IsLeft && (t.ParaLeftOrRight == "L&R" || t.ParaLeftOrRight == "仅L"))
                        GenerateDgvRows(t);
                    else if (!VisionCommon.IsLeft && (t.ParaLeftOrRight == "L&R" || t.ParaLeftOrRight == "仅R"))
                        GenerateDgvRows(t);
                }
                //if (t.ParaType == "信息读取（==）" || t.ParaType == "信息读取（like）")
                //{
                //    mainDgv.AddRow(t.ParaName, t.ParaType, "/", "/", "/");
                //}
                //else if (t.ParaType == "电阻")
                //{
                //    mainDgv.AddRow(t.ParaName, "电阻", string.Format("{0}{2}~{1}{2}", t.ParaGroups[0].ParaGroupMin,
                //        t.ParaGroups[0].ParaGroupMax, t.ParaUnit), "/", "/");
                //}
                //else if (t.ParaType == "电性能" || t.ParaType == "电性能，静态图像")
                //{
                //    mainDgv.AddRow(t.ParaName, "电性能", string.Format("{0}{2}~{1}{2}", t.ParaGroups[0].ParaGroupMin,
                //                    t.ParaGroups[0].ParaGroupMax, t.ParaUnit), "/", "/");

                //    if (VisionCommon.SelectGroups.Any())
                //    {
                //        if (VisionCommon.SelectGroups.ContainsKey(t.ParaName))
                //        {
                //            var g = VisionCommon.SelectGroups.Keys.First(f => f == t.ParaName);
                //            if (g != null)
                //            {
                //                var gp =
                //                    t.ParaGroups.ToList().Find(f => f.ParaGroupName == VisionCommon.SelectGroups[g]);

                //                mainDgv.Rows[mainDgv.RowCount - 1].Cells[2].Value = string.Format("{0}{2}~{1}{2}", gp.ParaGroupMin,
                //                       gp.ParaGroupMax, t.ParaUnit);
                //            }
                //        }
                //    }

                //    if (t.ParaType.EndsWith("静态图像"))
                //    {
                //        mainDgv.AddRow(t.ParaName, "静态图像", "/", "/", "/");
                //    }
                //}
                //else if (t.ParaType == "静态图像" || t.ParaType == "动态图像")
                //{
                //    mainDgv.AddRow(t.ParaName, t.ParaType, "/", "/", "/");
                //}
            }

            mainDgv.AutoResizeColumns();
        }

        /// <summary>
        /// 0或1为界面手动启动，0为ByPass，1为NoByPass，2为正常启动
        /// </summary>
        /// <param name="isByPass"></param>
        public void Start(int isByPass)
        {
            if (BackgroundWorker != null && !IsChecking && VisionCommon.NiImaqdLoadOk && !_bStart)
            {
                IsByPass = isByPass;
                IsChecking = true;

                Invoke(new Action(() =>
                {
                    uiMarkLabel1.Text = string.Format("测试时间：正在检测\r\n测试结果：正在检测\r\n测试耗时：正在检测");
                    uiLedBulb1.Color = Color.DarkGoldenrod;
                    InitMainDgv();

                    foreach (var key in _staticImageViewers.Keys)
                        _staticImageViewers[key].Reset();
                    foreach (var key in _dynamicImageViewers.Keys)
                        _dynamicImageViewers[key].Reset();
                }));

                //_backgroundWorker.RunWorkerAsync();
                _bStart = true;
            }
        }

        public void UpdateMainDgv(string name, string type, string value, string unit, string result)
        {
            Invoke(
                new Action(
                    () =>
                    {
                        uiTabControl1.SelectedIndex = 1;

                        for (var j = 0; j < mainDgv.RowCount; j++)
                        {
                            var row = mainDgv.Rows[j];
                            if (row.Cells[0].Value.ToString() == name && row.Cells[1].Value.ToString() == type)
                            {
                                mainDgv.FirstDisplayedScrollingRowIndex = j;
                                row.Cells[3].Value = value.ToString(CultureInfo.InvariantCulture) + unit;
                                row.Cells[4].Value = result;
                                //row.Cells[5].Value = string.Empty;
                                if (result == "NG")
                                    row.DefaultCellStyle.BackColor = Color.Red;
                                else
                                    row.DefaultCellStyle.BackColor = Color.AntiqueWhite;
                                break;
                            }
                        }

                        mainDgv.ClearSelection();
                    }));
        }

        public void UpdateMainDgv(string name, string type, double ms)
        {
            Invoke(
                new Action(
                    () =>
                    {
                        uiTabControl1.SelectedIndex = 1;

                        for (var j = 0; j < mainDgv.RowCount; j++)
                        {
                            var row = mainDgv.Rows[j];
                            if (row.Cells[0].Value.ToString() == name && row.Cells[1].Value.ToString() == type)
                            {
                                mainDgv.FirstDisplayedScrollingRowIndex = j;
                                row.Cells[4].Value = row.Cells[4].Value + "/" + Math.Round(ms / 1000, 2, MidpointRounding.AwayFromZero) + "S";
                            }
                        }

                        mainDgv.ClearSelection();
                    }));
        }

        public void UpdateVisionPara(VisionConfig config)
        {
            if (ToRunVisionConfig != null && config != null && config.VisionInfo != null)
                ToRunVisionConfig.VisionInfo = config.VisionInfo;
        }

        private static void GetCount()
        {
            {
                try
                {
                    var sql =
                        string.Format(
                            "select taskNo from manufactureCheckData where creater = '{0}'  and (checkResult = '0001' or checkResult = '0002') and createTime between datetime('now','start of day','+1 seconds') and  datetime('now','start of day','+1 days','-1 seconds') group by taskNo",
                            VisionCommon.VisionDeviceConfig.DeviceInfo.DeviceNo + ":" + VisionCommon.Title);

                    var getData = LocalDbHelper.QueryBySqlString(sql);
                    VisionCommon.TotalCount = getData.Count;
                }
                catch (Exception)
                {
                    VisionCommon.TotalCount = 0;
                }
            }

            {


                try
                {
                    var sqlOk = string.Format("select taskNo from manufactureCheckData where creater = '{0}' and createTime between datetime('now','start of day','+1 seconds') and  datetime('now','start of day','+1 days','-1 seconds') and checkResult = '0001' group by taskNo", VisionCommon.VisionDeviceConfig.DeviceInfo.DeviceNo + ":" + VisionCommon.Title);
                    var getData = LocalDbHelper.QueryBySqlString(sqlOk);
                    VisionCommon.OkCount = getData.Count;
                }
                catch (Exception)
                {
                    VisionCommon.OkCount = 0;
                }
            }
        }

        /// <summary>
        /// 根据窗体大小调整控件大小
        /// </summary>
        /// <param name="newX">窗体宽度缩放比例</param>
        /// <param name="newY">窗体高度缩放比例</param>
        /// <param name="cons">随窗体改变控件大小</param>
        private static void SetControls(float newX, float newY, System.Windows.Forms.Control cons)
        {
            try
            {
                //遍历窗体中的控件，重新设置控件的值
                foreach (System.Windows.Forms.Control con in cons.Controls)
                {
                    if (con.GetType() == typeof(DynamicImageViewer) || con.GetType() == typeof(StaticImageViewer))
                        continue;

                    var myTag = con.Tag.ToString().Split(':'); //获取控件的Tag属性值，并分割后存储字符串数组
                    var a = Convert.ToSingle(myTag[0]) * newX; //根据窗体缩放比例确定控件的值，宽度
                    con.Width = (int)a; //宽度
                    a = Convert.ToSingle(myTag[1]) * newY; //高度
                    con.Height = (int)(a);
                    a = Convert.ToSingle(myTag[2]) * newX; //左边距离
                    con.Left = (int)(a);
                    a = Convert.ToSingle(myTag[3]) * newY; //上边缘距离
                    con.Top = (int)(a);
                    var currentSize = Convert.ToSingle(myTag[4]) * newY; //字体大小
                    con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                    if (con.Controls.Count > 0)
                        SetControls(newX, newY, con);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// 将控件的宽，高，左边距，顶边距和字体大小暂存到tag属性中
        /// </summary>
        /// <param name="cons">递归控件中的控件</param>
        private static void SetTag(System.Windows.Forms.Control cons)
        {
            foreach (System.Windows.Forms.Control con in cons.Controls)//循环窗体中的控件
            {
                con.Tag = con.Width + ":" + con.Height + ":" + con.Left + ":" + con.Top + ":" + con.Font.Size;
                if (con.Controls.Count > 0)
                    SetTag(con);
            }
        }

        private bool _bStart;

        private void backgroundWorker_DoWork()
        {
            //var worker = (BackgroundWorker)sender;

            while (BackgroundWorker.IsAlive)
            {
                if (!BackgroundWorker.IsAlive)
                    return;

                if (!_bStart)
                {
                    Thread.Sleep(5);
                    continue;
                }

                var ngCount = 0;
                var stopWatch = new Stopwatch();
                stopWatch.Start();

                foreach (var t in VisionCommon.IoControllers)
                    t.Value.ResetOutPuts();

                var otherDelayTime = 0;

                try
                {
                    if (ToRunVisionConfig != null && ToRunVisionConfig.ParaInfo != null)
                    {
                        if (ToRunVisionConfig.DeviceInfo != null && ToRunVisionConfig.DeviceInfo.Actions != null &&
                            ToRunVisionConfig.DeviceInfo.Actions.PropulsionCylinder != null)
                        {
                            try
                            {
                                if (string.Equals(
                                        ToRunVisionConfig.DeviceInfo.Actions.PropulsionCylinder.IsPropulsionCylinder,
                                        "true", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    VisionCommon.InvokeRelays(new VisionConfigParaParaReleysList
                                    {
                                        ParaReleysOffList = string.Empty,
                                        ParaReleysOnList = ToRunVisionConfig.DeviceInfo.Actions.PropulsionCylinder.Binding
                                    });
                                    Thread.Sleep(int.Parse(ToRunVisionConfig.DeviceInfo.Actions.PropulsionCylinder.Delay));
                                    otherDelayTime +=
                                        int.Parse(ToRunVisionConfig.DeviceInfo.Actions.PropulsionCylinder.Delay);
                                }
                            }
                            catch (Exception a)
                            {
                                Console.WriteLine(a);
                            }
                        }

                        if (ToRunVisionConfig.TestFlowInfo == null ||
                            ToRunVisionConfig.TestFlowInfo.Length == 0) // 没有特殊，用默认方法
                        {
                            for (var i = 0; i < ToRunVisionConfig.ParaInfo.Length; i++)
                            {
                                var t = ToRunVisionConfig.ParaInfo[i];

                                if (VisionCommon.IsLeft)
                                {
                                    if (t.ParaLeftOrRight == "仅R")
                                        continue;
                                }
                                else
                                {
                                    if (t.ParaLeftOrRight == "仅L")
                                        continue;
                                }

                                var thisStepPowerPara = string.IsNullOrEmpty(t.PowerPara)
                                    ? string.Format("电压1={0}V，电流1={1}A，电压2={2}V，电流2={3}A，电压3={4}V，电流3={5}A，串并联模式={6}", 13.5,
                                        10, 0, 0, 5, 1, "无")
                                    : t.PowerPara;

                                VisionCommon.PowerSet(thisStepPowerPara, true);

                                if (!InvokeParaAction(t))
                                {
                                    ngCount++;
                                    if (IsByPass == 2 || IsByPass == 1)
                                        break;
                                }
                            }

                            foreach (var power in VisionCommon.ListCcdPowers)
                            {
                                VisionCommon.IsPowerOn = false;
                                power.Value.PowerOff();
                            }
                        }
                        else
                        {
                            var checkCount = 0;

                            for (var i = 0; i < ToRunVisionConfig.TestFlowInfo.Length; i++)
                            {
                                var level1 = VisionCommon.VisionConfig.TestFlowInfo[i];

                                if (string.IsNullOrEmpty(level1.TestFlowValue))
                                    continue;

                                if (ngCount > 0)
                                {
                                    if (IsByPass == 2 || IsByPass == 1)
                                        break;
                                }

                                VisionCommon.PowerSet(level1.TestFlowValue.Split('：')[1], true);

                                if (level1.TestFlow != null) // level 2
                                {
                                    foreach (var level2 in level1.TestFlow)
                                    {
                                        if (string.IsNullOrEmpty(level2.TestFlowValue))
                                            continue;

                                        if (level2.TestFlowValue.StartsWith("检测："))
                                        {
                                            var checkList = level2.TestFlowValue.Split("：")[1].Split("，");

                                            if (checkList.Any())
                                            {
                                                foreach (var ct in checkList)
                                                {
                                                    var t = ToRunVisionConfig.ParaInfo.ToList().Find(f => f.ParaName == ct);

                                                    if (t != null)
                                                    {
                                                        var checkResult = InvokeParaAction(t);
                                                        checkCount++;

                                                        if (!checkResult)
                                                        {
                                                            ngCount++;
                                                            if (IsByPass == 2 || IsByPass == 1)
                                                                break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (level2.TestFlowValue.StartsWith("继电器"))
                                            {
                                                var sp = level2.TestFlowValue.Split("：");
                                                var key = sp[0];
                                                var value = string.Equals(sp[1], "ON",
                                                    StringComparison.CurrentCultureIgnoreCase);
                                                if (!VisionCommon.Relays.ContainsKey(key))
                                                    continue;
                                                VisionCommon.Relays[key].Invoke(value);

                                                //foreach (var t in VisionCommon.IoControllers)
                                                //    t.Value.SetOutputs();
                                            }
                                            else if (level2.TestFlowValue.StartsWith("延时："))
                                            {
                                                var sp = level2.TestFlowValue.Split("：");

                                                Thread.Sleep(int.Parse(sp[1]));
                                            }
                                            else if (level2.TestFlowValue.StartsWith("函数："))
                                            {
                                                var sp = level2.TestFlowValue.Split("：");

                                                VisionCommon.InvokeCustomCmd(sp[1]);
                                            }
                                        }
                                    }
                                }

                                foreach (var power in VisionCommon.ListCcdPowers)
                                {
                                    VisionCommon.IsPowerOn = false;
                                    power.Value.PowerOff();
                                }
                            }

                            if (checkCount != ToRunVisionConfig.ParaInfo.Length)
                                ngCount++;
                        }

                        stopWatch.Stop();

                        if (InvokeRequired)
                        {
                            Invoke(new Action(() =>
                            {
                                if (ngCount > 0)
                                {
                                    SaveData(false, IsByPass);
                                    uiLedBulb1.Color = Color.Red;
                                    uiMarkLabel1.Text =
                                        string.Format("测试时间：" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") +
                                                      "\r\n测试结果：NG\r\n测试耗时：" +
                                                      (stopWatch.ElapsedMilliseconds - otherDelayTime) / 1000f + "秒");
                                    _checkApp.Alarm();

                                    // 20241219,+NG后清空二维码重新扫描
                                    btnBarcodeScanReset_Click(null, null);
                                }
                                else
                                {
                                    SaveData(true, IsByPass);
                                    uiLedBulb1.Color = Color.Green;
                                    uiMarkLabel1.Text =
                                        string.Format("测试时间：" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") +
                                                      "\r\n测试结果：OK\r\n测试耗时：" +
                                                      (stopWatch.ElapsedMilliseconds - otherDelayTime) / 1000f + "秒");

                                    if (VisionCommon.VisionDeviceConfig.DeviceInfo.Actions.Bang != null &&
                                        IsByPass == 2)
                                    {
                                        if (VisionCommon.VisionConfig.DeviceInfo.Actions != null &&
                                            VisionCommon.VisionConfig.DeviceInfo.Actions.Bang != null &&
                                            VisionCommon.VisionConfig.DeviceInfo.Actions.Bang.IsBang != null &&
                                            string.Equals(VisionCommon.VisionConfig.DeviceInfo.Actions.Bang.IsBang,
                                                bool.TrueString, StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            if (VisionCommon.VisionDeviceConfig.DeviceInfo.Actions.Bang.Controller !=
                                                null &&
                                                VisionCommon.VisionDeviceConfig.DeviceInfo.Actions.Bang.Field != null)
                                            {
                                                if (VisionCommon.IoControllers.ContainsKey(VisionCommon
                                                        .VisionDeviceConfig
                                                        .DeviceInfo.Actions.Bang.Controller))
                                                {
                                                    var bangController =
                                                        VisionCommon.IoControllers[
                                                            VisionCommon.VisionDeviceConfig.DeviceInfo.Actions.Bang
                                                                .Controller];

                                                    var field = bangController.GetType()
                                                        .GetField(VisionCommon.VisionDeviceConfig.DeviceInfo.Actions
                                                            .Bang
                                                            .Field);
                                                    if (field != null)
                                                    {
                                                        field.SetValue(bangController, true);
                                                        bangController.SetOutputs();

                                                        if (int.Parse(VisionCommon.VisionConfig.DeviceInfo.Actions.Bang
                                                                .Delay) > 0)
                                                            Thread.Sleep(int.Parse(VisionCommon.VisionConfig.DeviceInfo
                                                                .Actions.Bang.Delay));

                                                        field.SetValue(bangController, false);
                                                        bangController.SetOutputs();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }));
                        }

                        if (ToRunVisionConfig.DeviceInfo != null &&
                            ToRunVisionConfig.DeviceInfo.Actions != null &&
                            ToRunVisionConfig.DeviceInfo.Actions.PropulsionCylinder != null)
                        {
                            try
                            {
                                if (string.Equals(
                                        ToRunVisionConfig.DeviceInfo.Actions.PropulsionCylinder.IsPropulsionCylinder,
                                        "true",
                                        StringComparison.CurrentCultureIgnoreCase))
                                {
                                    VisionCommon.InvokeRelays(new VisionConfigParaParaReleysList
                                    {
                                        ParaReleysOffList = string.Empty,
                                        ParaReleysOnList = ToRunVisionConfig.DeviceInfo.Actions.PropulsionCylinder
                                            .Binding
                                    }, true);
                                    Thread.Sleep(450);
                                }
                            }
                            catch (Exception a)
                            {
                                Console.WriteLine(a);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
                finally
                {
                    foreach (var item in VisionCommon.NiImaqd.CameraList)
                        item.OpenCamera();

                    foreach (var t in VisionCommon.IoControllers)
                        t.Value.ResetOutPuts();

                    _bStart = false;
                    IsChecking = false;
                }
            }
        }

        private void FrmVisionCheckTestView_Load(object sender, EventArgs e)
        {
            _x = this.Width;//获取窗体的宽度
            _y = this.Height;//获取窗体的高度
            SetTag(uiTitlePanel1);//调用方法

            Resize += FrmVisionCheckTestView_Resize;

            VisionCommon.LoadDevice();

            //_backgroundWorker = new BackgroundWorker();
            //_backgroundWorker.DoWork += backgroundWorker_DoWork;
            //_backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            //_backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
            //_backgroundWorker.WorkerReportsProgress = true;
            //_backgroundWorker.WorkerSupportsCancellation = true;

            //DeviceBackgroundWorker = new BackgroundWorker();
            //DeviceBackgroundWorker.DoWork += DeviceBackgroundWorker_DoWork;
            //DeviceBackgroundWorker.RunWorkerCompleted += DeviceBackgroundWorker_RunWorkerCompleted;
            //DeviceBackgroundWorker.ProgressChanged += DeviceBackgroundWorker_ProgressChanged;
            //DeviceBackgroundWorker.WorkerReportsProgress = true;
            //DeviceBackgroundWorker.WorkerSupportsCancellation = true;
            //DeviceBackgroundWorker.RunWorkerAsync();

            if (BackgroundWorker != null)
            {
                BackgroundWorker.Abort();
                BackgroundWorker.Join();
            }

            BackgroundWorker = new Thread(backgroundWorker_DoWork) { IsBackground = true };
            BackgroundWorker.Start();

            if (DeviceBackgroundWorker != null)
            {
                DeviceBackgroundWorker.Abort();
                DeviceBackgroundWorker.Join();
            }

            DeviceBackgroundWorker = new Thread(DeviceBackgroundWorker_DoWork) { IsBackground = true };
            DeviceBackgroundWorker.Start();
        }

        private void FrmVisionCheckTestView_Closed(object sender, EventArgs e)
        {
            if (BackgroundWorker != null)
            {
                BackgroundWorker.Abort();
                BackgroundWorker.Join();
            }

            if (DeviceBackgroundWorker != null)
            {
                DeviceBackgroundWorker.Abort();
                DeviceBackgroundWorker.Join();
            }
        }

        private void FrmVisionCheckTestView_Resize(object sender, EventArgs e)
        {
            var newX = (this.Width) / _x; //窗体宽度缩放比例
            var newY = ((this.Height) / _y) / 1;//窗体高度缩放比例
            SetControls(newX, newY, uiTitlePanel1);//随窗体改变控件大小
        }

        private void GenerateDgvRows(VisionConfigPara t)
        {
            if (t.ParaType == "信息读取（==）" || t.ParaType == "信息读取（like）")
            {
                if (t.ParaGroups != null && t.ParaGroups.Any() &&
                    !string.IsNullOrEmpty(t.ParaGroups[0].ParaGroupValue))
                {
                    mainDgv.AddRow(t.ParaName, t.ParaType, t.ParaGroups[0].ParaGroupValue, "/", "/");
                }
                else
                {
                    mainDgv.AddRow(t.ParaName, t.ParaType, "", "/", "/");
                }

                if (VisionCommon.SelectGroups.Any())
                {
                    if (VisionCommon.SelectGroups.ContainsKey(t.ParaName))
                    {
                        var g = VisionCommon.SelectGroups.Keys.First(f => f == t.ParaName);
                        if (g != null)
                        {
                            var gp =
                                t.ParaGroups.ToList().Find(f => f.ParaGroupName == VisionCommon.SelectGroups[g]);

                            mainDgv.Rows[mainDgv.RowCount - 1].Cells[2].Value = string.Format("{0}", gp.ParaGroupValue);
                        }
                    }
                }
            }
            else if (t.ParaType == "电阻")
            {
                mainDgv.AddRow(t.ParaName, "电阻", string.Format("{0}{2}~{1}{2}", t.ParaGroups[0].ParaGroupMin,
                    t.ParaGroups[0].ParaGroupMax, t.ParaUnit), "/", "/");

                if (VisionCommon.SelectGroups.Any())
                {
                    if (VisionCommon.SelectGroups.ContainsKey(t.ParaName))
                    {
                        var g = VisionCommon.SelectGroups.Keys.First(f => f == t.ParaName);
                        if (g != null)
                        {
                            var gp =
                                t.ParaGroups.ToList().Find(f => f.ParaGroupName == VisionCommon.SelectGroups[g]);

                            mainDgv.Rows[mainDgv.RowCount - 1].Cells[2].Value = string.Format("{0}{2}~{1}{2}", gp.ParaGroupMin,
                                   gp.ParaGroupMax, t.ParaUnit);
                        }
                    }
                }
            }
            else if (t.ParaType == "电性能" || t.ParaType == "电性能，静态图像")
            {
                mainDgv.AddRow(t.ParaName, "电性能", string.Format("{0}{2}~{1}{2}", t.ParaGroups[0].ParaGroupMin,
                                t.ParaGroups[0].ParaGroupMax, t.ParaUnit), "/", "/");

                if (VisionCommon.SelectGroups.Any())
                {
                    if (VisionCommon.SelectGroups.ContainsKey(t.ParaName))
                    {
                        var g = VisionCommon.SelectGroups.Keys.First(f => f == t.ParaName);
                        if (g != null)
                        {
                            var gp =
                                t.ParaGroups.ToList().Find(f => f.ParaGroupName == VisionCommon.SelectGroups[g]);

                            mainDgv.Rows[mainDgv.RowCount - 1].Cells[2].Value = string.Format("{0}{2}~{1}{2}", gp.ParaGroupMin,
                                   gp.ParaGroupMax, t.ParaUnit);
                        }
                    }
                }

                if (t.ParaType.EndsWith("静态图像"))
                {
                    mainDgv.AddRow(t.ParaName, "静态图像", "/", "/", "/");

                    if (!_staticImageViewers.ContainsKey(t.ParaName))
                    {
                        var imgViewer = new StaticImageViewer { Dock = DockStyle.Fill };
                        _staticImageViewers.Add(t.ParaName, imgViewer);
                        var tabPage = new TabPage(t.ParaName);
                        tabPage.Controls.Add(imgViewer);
                        imgTabControl.Controls.Add(tabPage);
                    }
                }
            }
            else if (t.ParaType == "静态图像" || t.ParaType == "动态图像")
            {
                mainDgv.AddRow(t.ParaName, t.ParaType, "/", "/", "/");

                if (!_staticImageViewers.ContainsKey(t.ParaName) && t.ParaType == "静态图像")
                {
                    var imgViewer = new StaticImageViewer { Dock = DockStyle.Fill };
                    _staticImageViewers.Add(t.ParaName, imgViewer);
                    var tabPage = new TabPage(t.ParaName);
                    tabPage.Controls.Add(imgViewer);
                    imgTabControl.Controls.Add(tabPage);
                }
                else if (!_dynamicImageViewers.ContainsKey(t.ParaName) && t.ParaType == "动态图像")
                {
                    var imgViewer = new DynamicImageViewer { Dock = DockStyle.Fill };
                    _dynamicImageViewers.Add(t.ParaName, imgViewer);
                    var tabPage = new TabPage(t.ParaName);
                    tabPage.Controls.Add(imgViewer);
                    imgTabControl.Controls.Add(tabPage);
                }
            }
        }

        private static string GetTotalCountShowString()
        {
            //VisionCommon.OkCount = 15215;
            //VisionCommon.TotalCount = 24581;

            return @"测试汇总：【" + DateTime.Now.ToString("MM/dd").Replace('-', '/') + "：" + @"OK=" + VisionCommon.OkCount +
                   @"，NG=" + (VisionCommon.TotalCount - VisionCommon.OkCount) + @"，总=" + VisionCommon.TotalCount + @"】";
        }

        private void InitMainDgv()
        {
            #region test barcode

            //barcodeDgv.Style = UIStyle.Gray;
            //barcodeDgv.ReadOnly = true;
            //barcodeDgv.RowHeadersVisible = false;
            //barcodeDgv.AllowUserToAddRows = false;
            //barcodeDgv.AllowUserToResizeRows = false;
            //barcodeDgv.MultiSelect = true;
            //barcodeDgv.RowHeadersVisible = false;
            //barcodeDgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            //barcodeDgv.ClearAll();
            //barcodeDgv.AddColumn("名称", "名称");
            //barcodeDgv.AddColumn("结果", "结果");

            //for (var i = 0; i < 15; i++)
            //{
            //    barcodeDgv.AddRow(new object[] { i, i });
            //    richTextBox1.AppendText(string.Format("{0}_{1}\r\n", DateTime.Now.ToString("O"), Guid.NewGuid().ToString()));
            //}

            //richTextBox1.SelectionStart = richTextBox1.Text.Length;
            //richTextBox1.ScrollToCaret();

            #endregion

            mainDgv.Style = UIStyle.Gray;
            mainDgv.ReadOnly = true;
            mainDgv.RowHeadersVisible = false;
            mainDgv.AllowUserToAddRows = false;
            mainDgv.AllowUserToResizeRows = false;
            mainDgv.MultiSelect = false;
            mainDgv.RowHeadersVisible = false;
            mainDgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            mainDgv.ClearRows();
            mainDgv.ClearColumns();
            mainDgv.AddColumn("名称", "名称");
            mainDgv.AddColumn("类型", "类型");
            mainDgv.AddColumn("标准", "标准");
            mainDgv.AddColumn("测试值", "测试值");
            mainDgv.AddColumn("结果", "结果");
            //mainDgv.AddColumn("耗时", "耗时");

            foreach (var t in ToRunVisionConfig.ParaInfo)
            {
                if (VisionCommon.VisionConfig.DeviceInfo.DeviceLeftOrRight != null &&
                    string.Equals(VisionCommon.VisionConfig.DeviceInfo.DeviceLeftOrRight, @"不区分",
                        StringComparison.CurrentCultureIgnoreCase))
                {
                    GenerateDgvRows(t);
                }
                else
                {
                    if (VisionCommon.IsLeft && (t.ParaLeftOrRight == "L&R" || t.ParaLeftOrRight == "仅L"))
                        GenerateDgvRows(t);
                    else if (!VisionCommon.IsLeft && (t.ParaLeftOrRight == "L&R" || t.ParaLeftOrRight == "仅R"))
                        GenerateDgvRows(t);
                }
            }

            mainDgv.AutoResizeColumns();
        }

        private void SaveData(bool isOk, int isByPass)
        {
            if (isByPass == 2)
            {
                VisionCommon.TotalCount++;
                if (isOk)
                    VisionCommon.OkCount++;
            }
            uiTitlePanel1.Text = GetTotalCountShowString(); //@"测试汇总：【日期：" + DateTime.Now.ToString("yyyy/MM/dd") + @"，OK：" + VisionCommon.OkCount + @"/总：" + VisionCommon.TotalCount + @"】";

            //var checkDatas = new List<FormCheck.CheckNameAndData>();
            var checkDatas2 = new List<SyProductionSaveCheckData.CheckDataDetail>();
            for (var i = 0; i < mainDgv.RowCount; i++)
            {
                if (mainDgv.Rows[i].Cells[4].Value != null && !string.IsNullOrEmpty(mainDgv.Rows[i].Cells[4].Value.ToString()) && mainDgv.Rows[i].Cells[4].Value.ToString() != "/")
                {
                    var spResult = mainDgv.Rows[i].Cells[4].Value.ToString().Split("/");

                    var name = string.Format("{0}_{1}/{2}", mainDgv.Rows[i].Cells[0].Value, mainDgv.Rows[i].Cells[1].Value, spResult[1]);
                    //var data = mainDgv.Rows[i].Cells[3].Value.ToString();
                    var range = mainDgv.Rows[i].Cells[2].Value.ToString();

                    //var result = string.Empty;
                    //result = mainDgv.Rows[i].Cells[4].Value.ToString().ToLower().StartsWith("OK".ToLower()) ? "True" : "False";

                    var result = string.Equals(spResult[0], "OK", StringComparison.CurrentCultureIgnoreCase)
                        ? "True"
                        : "False";
                    var value = mainDgv.Rows[i].Cells[3].Value.ToString();
                    var type = mainDgv.Rows[i].Cells[1].Value.ToString();

                    if (value == "见右表" && (type == "静态图像" || type == "动态图像"))
                        value = result == "True" ? "OK" : "NG";
                    //checkDatas.Add(new FormCheck.CheckNameAndData { Name = name, Data = data });

                    checkDatas2.Add(new SyProductionSaveCheckData.CheckDataDetail { ParaName = name, Range = range, Result = result, Type = type, Value = value });
                }
            }

            string prNo;
            string materialName;
            if (VisionCommon.VisionConfig.DeviceInfo.DeviceLeftOrRight != null &&
                string.Equals(VisionCommon.VisionConfig.DeviceInfo.DeviceLeftOrRight, @"不区分", StringComparison.CurrentCultureIgnoreCase))
            {
                prNo = VisionCommon.VisionConfig.DeviceInfo.DeviceNo;
                materialName = VisionCommon.VisionConfig.DeviceInfo.DeviceName;
            }
            else
            {
                if (VisionCommon.IsLeft)
                {
                    prNo = VisionCommon.VisionConfig.DeviceInfo.DeviceNo;
                    materialName = VisionCommon.VisionConfig.DeviceInfo.DeviceName + "-L";
                }
                else
                {
                    prNo = !string.IsNullOrEmpty(VisionCommon.VisionConfig.DeviceInfo.DeviceNo2)
                       ? VisionCommon.VisionConfig.DeviceInfo.DeviceNo2
                       : VisionCommon.VisionConfig.DeviceInfo.DeviceNo;
                    materialName = VisionCommon.VisionConfig.DeviceInfo.DeviceName + "-R";
                }
            }

            var barcodeList = new List<string>();
            //if (barcodeDgv.RowCount > 0)
            //{
            //    for (var i = 0; i < barcodeDgv.RowCount; i++)
            //        barcodeList.Add(barcodeDgv.Rows[i].Cells[1].Value.ToString());
            //}

            barcodeList.AddRange(_toSaveBarcodes);

            var saveLocalAction = new Action<string, string, string, List<SyProductionSaveCheckData.CheckDataDetail>>((taskid, uid, productNo, checkData) =>
                        {
                            try
                            {
                                var saveLocalData = new LocalDbHelper.manufactureCheckData
                                {
                                    taskNo = taskid,
                                    productNo = productNo,
                                    productUid = uid,
                                    pcbaNo = string.Empty,
                                    pcbaBarcode = uid,
                                    productBarcode = uid,
                                    packageBarcode = string.Empty,
                                    processNo = productNo + "_001",
                                    checkData = JsonConvert.SerializeObject(checkData),
                                    checkDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                    checkStaffNo = VisionCommon.LoginUser,
                                    checkResult = isOk ? "0001" : "0002",
                                    creater = VisionCommon.VisionDeviceConfig.DeviceInfo.DeviceNo + ":" + VisionCommon.Title,
                                    createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                    checkDataText = string.Empty
                                };

                                LocalDbHelper.InsertData(new LocalDbHelper.manufactureCheckData[] { saveLocalData });

                                //var insert =
                                //    string.Format(
                                //        "insert into manufactureCheckData (taskNo,productNo,productUid,pcbaNo,pcbaBarcode,productBarcode,packageBarcode,processNo,checkData,checkDate,checkStaffNo,checkResult,creater,createTime,checkDataText) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}')",
                                //        taskid, productNo, uid, string.Empty, uid, uid, string.Empty, productNo + "_001",
                                //        JsonConvert.SerializeObject(checkData),
                                //        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), VisionCommon.LoginUser,
                                //        isOk ? "0001" : "0002",
                                //        VisionCommon.VisionDeviceConfig.DeviceInfo.DeviceNo + ":" + VisionCommon.Title,
                                //        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), JsonConvert.SerializeObject(checkData));

                                //LocalDbHelper.LocalSqlite.Query(insert);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                        });

            var saveRemoteAction =
                new Action<string, string, List<string>, List<SyProductionSaveCheckData.CheckDataDetail>>(
                    (taskId, productNo, barcode, checkData) =>
                    {
                        SyProductionSaveCheckData.SaveData(
                            isOk, isByPass != 2,
                            VisionCommon.VisionDeviceConfig.DeviceInfo.DeviceNo, materialName, productNo, barcode,
                            checkData, VisionCommon.LoginUser, VisionCommon.TotalCount);
                    });

            var guid = Guid.NewGuid().ToString();
            if (isByPass == 2)
            {
                if (barcodeList.Any())
                    foreach (var item in barcodeList)
                        saveLocalAction.Invoke(guid, item, prNo, checkDatas2);
                else
                    saveLocalAction.Invoke(guid, string.Empty, prNo, checkDatas2);

                //if (barcodeDgv.RowCount > 0)
                //    for (var i = 0; i < barcodeDgv.RowCount; i++)
                //        savelocalAction.BeginInvoke(guid, barcodeDgv.Rows[i].Cells[1].Value.ToString(), prNo, checkDatas2, null, null);
                //else
                //    savelocalAction.BeginInvoke(guid, string.Empty, prNo, checkDatas2, null, null);
            }

            saveRemoteAction.Invoke(guid, prNo, barcodeList, checkDatas2);
            //saveRemoteAction.BeginInvoke(guid, prNo, barcodeList, checkDatas2, null, null);
        }

        #region device
        private readonly List<bool> _barcodeState = new List<bool>();

        private bool IsBarcodeScanExecute { get; set; }

        private bool IsBarcodeScanReset { get; set; }

        public bool IsRepeatTest { get; set; }

        private readonly List<string> _toSaveBarcodes = new List<string>();
        private readonly Dictionary<string, string> _preScanBarcodes = new Dictionary<string, string>();

        public bool WaitDi()
        {
            //Thread.Sleep(2500);
            //return true;

            var startController = VisionCommon.VisionDeviceConfig.DeviceInfo.Actions.Start.Controller;
            var c = VisionCommon.IoControllers[startController];
            c.GetInputs();

            var field = c.GetType().GetField(VisionCommon.VisionDeviceConfig.DeviceInfo.Actions.Start.Field);
            if (field != null)
            {
                if (field.FieldType == typeof(string))
                {
                    if (field.GetValue(c) == null)
                    {
                        return false;
                    }
                    if (field.GetValue(c).ToString() == "1")
                    {
                        if (c.GetType() == typeof(ControllerWithGateway))
                            return false;

                        return true;
                    }
                    if (field.GetValue(c).ToString() == "0")
                    {
                        if (c.GetType() == typeof(ControllerWithGateway))
                            return true;

                        return false;
                    }
                }
                else if (field.FieldType == typeof(bool) || field.FieldType == typeof(bool?))
                {
                    if (field.GetValue(c) == null)
                    {
                        return false;
                    }
                    if (string.Equals(field.GetValue(c).ToString(), bool.TrueString, StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (c.GetType() == typeof(ControllerWithGateway))
                            return false;

                        return true;
                    }
                    if (string.Equals(field.GetValue(c).ToString(), bool.FalseString, StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (c.GetType() == typeof(ControllerWithGateway))
                            return true;

                        return false;
                    }
                }
            }

            return false;
        }

        private void btnBarcodeScanReset_Click(object sender, EventArgs e)
        {
            BarcodeScanReset(!(sender == null && e == null));
        }

        private void BarcodeScanReset(bool isShowAlert)
        {
            if (IsBarcodeScanExecute && !IsBarcodeScanReset)
            {
                if (isShowAlert)
                {
                    if (this.ShowAskDialog("是否重新开始扫码"))
                    {

                        IsBarcodeScanReset = true;
                        this.ShowSuccessTip("扫码已经重置");
                    }
                }
                else
                {
                    IsBarcodeScanReset = true;
                    this.ShowSuccessTip("扫码已经重置");
                }
            }
        }

        private void DeviceBackgroundWorker_DoWork()
        {
            var step = 0;

            while (DeviceBackgroundWorker.IsAlive)
            {
                if (!DeviceBackgroundWorker.IsAlive)
                    return;

                Thread.Sleep(25);

                if (ToRunVisionConfig != null && VisionCommon.NiImaqdLoadOk)
                {
                    var startType = ToRunVisionConfig.DeviceInfo.Actions.Start.Type;

                    if (!IsChecking)
                    {
                        if (IsRepeatTest)
                        {
                            if (step == 0)
                            {
                                step = 1;
                            }
                            else if (step == 1)
                            {
                                _toSaveBarcodes.Clear();
                                _preScanBarcodes.Clear();

                                Invoke(new Action(() =>
                                {
                                    lblBarcodeScanNow.Text = @"重复测试中...";

                                    barcodeDgv.ClearAll();
                                    barcodeDgv.AddColumn("名称", "名称");
                                    barcodeDgv.AddColumn("结果", "结果");

                                    richTextBox1.Clear();
                                }));

                                Thread.Sleep(500);
                                Start(2);
                                Thread.Sleep(100);
                                step = 0;
                            }
                        }
                        else
                        {
                            //扫码启动
                            //按键启动
                            //扫码后按键启动
                            //按键后扫码启动
                            if (startType == "扫码启动")
                            {
                                if (step == 0)
                                {
                                    if (WaitBarcode())
                                    {
                                        step = 1;
                                    }
                                }
                                else if (step == 1)
                                {
                                    IsBarcodeScanExecute = false;
                                    Start(2);
                                    Thread.Sleep(100);
                                    step = 0;
                                }
                            }
                            else if (startType == "按键启动")
                            {
                                if (step == 0)
                                {
                                    if (WaitDi())
                                        step = 1;
                                }
                                else if (step == 1)
                                {
                                    Start(2);
                                    Thread.Sleep(100);
                                    step = 0;
                                }
                            }
                            else if (startType == "扫码后按键启动")
                            {
                                if (step == 0)
                                {
                                    if (WaitBarcode())
                                    {
                                        step = 1;
                                    }
                                }
                                else if (step == 1)
                                {
                                    if (WaitDi())
                                    {
                                        IsBarcodeScanExecute = false;
                                        CopyProScanBarcodeToGgv();
                                        step = 2;
                                    }
                                    else
                                    {
                                        if (IsBarcodeScanReset)
                                        {
                                            step = 0;
                                        }
                                    }
                                }
                                else if (step == 2)
                                {
                                    // f03debug;
                                    {
                                        var code = _toSaveBarcodes.Find(f => f.StartsWith("+"));
                                        if (code != null)
                                        {
                                            var keys = VisionCommon.CustomControllers.Keys.ToList();
                                            foreach (var key in keys)
                                            {
                                                var item = VisionCommon.CustomControllers[key];
                                                if (item.GetType() == typeof(F03RearLamp))
                                                    ((F03RearLamp)item).ToWriteBarcode = code;
                                            }
                                        }
                                    }
                                    Start(2);
                                    Thread.Sleep(100);
                                    step = 0;
                                }
                            }
                            else if (startType == "按键后扫码启动")
                            {
                                if (step == 0)
                                {
                                    if (WaitDi())
                                        step = 1;
                                }
                                else if (step == 1)
                                {
                                    if (WaitBarcode())
                                        step = 2;
                                }
                                else if (step == 2)
                                {
                                    IsBarcodeScanExecute = false;
                                    Start(2);
                                    Thread.Sleep(100);
                                    step = 0;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!IsRepeatTest && startType == "扫码后按键启动")
                        {
                            if (step == 0)
                            {
                                _preScanBarcodes.Clear();
                                if (WaitBarcode(true))
                                    step = 1;
                            }
                            else if (step == 1)
                            {
                                if (IsBarcodeScanReset)
                                    step = 0;
                            }
                        }
                    }
                }
            }
        }

        private bool WaitBarcode(bool isPreScan = false)
        {
            _barcodeState.Clear();

            Invoke(new Action(() =>
            {
                if (VisionCommon.VisionConfig.BarcodeInfo.Any())
                    lblBarcodeScanNow.Text = string.Format(@"请扫‘{0}’标签！第{1}个/共{2}个二维码", VisionCommon.VisionConfig.BarcodeInfo[0].Name, 1, VisionCommon.VisionConfig.BarcodeInfo.Length);
            }));

            if (VisionCommon.VisionConfig.BarcodeInfo == null ||
                !VisionCommon.VisionConfig.BarcodeInfo.Any())
                return false;

            for (var i = 0; i < VisionCommon.VisionConfig.BarcodeInfo.Length; i++)
                _barcodeState.Add(false);

            while (true)
            {
                Thread.Sleep(5);

                if (IsRepeatTest)
                {
                    IsBarcodeScanReset = false;
                    IsBarcodeScanExecute = false;

                    Invoke(new Action(() =>
                    {
                        barcodeDgv.Style = UIStyle.Gray;
                        barcodeDgv.ReadOnly = true;
                        barcodeDgv.RowHeadersVisible = false;
                        barcodeDgv.AllowUserToAddRows = false;
                        barcodeDgv.AllowUserToResizeRows = false;
                        barcodeDgv.MultiSelect = true;
                        barcodeDgv.RowHeadersVisible = false;
                        barcodeDgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                        barcodeDgv.ClearAll();
                        barcodeDgv.AddColumn("名称", "名称");
                        barcodeDgv.AddColumn("结果", "结果");

                        lblBarcodeScanNow.Text = string.Empty;

                        richTextBox1.Clear();
                    }));

                    return true;
                }

                IsBarcodeScanExecute = true;

                VisionCommon.BarcodeScanReader.ReadBarcodeTimeoutMs = 2000;
                VisionCommon.BarcodeScanReader.ReadBarcode(1);
                if (string.IsNullOrEmpty(VisionCommon.BarcodeScanReader.GetBarcodeStr))
                {
                    if (IsBarcodeScanReset)
                    {
                        IsBarcodeScanReset = false;
                        IsBarcodeScanExecute = false;

                        Invoke(new Action(() =>
                        {
                            _preScanBarcodes.Clear();

                            if (!isPreScan)
                            {
                                barcodeDgv.ClearAll();
                                barcodeDgv.AddColumn("名称", "名称");
                                barcodeDgv.AddColumn("结果", "结果");
                            }

                            richTextBox1.Clear();
                        }));

                        return false;
                    }

                    continue;
                }

                var barcode = VisionCommon.BarcodeScanReader.GetBarcodeStr;

                for (var e = 0; e < VisionCommon.VisionConfig.BarcodeInfo.Length; e++)
                {
                    if (_barcodeState[e])
                        continue;

                    if (e == 0)
                    {
                        Invoke(new Action(() =>
                        {
                            if (!isPreScan)
                            {
                                barcodeDgv.Style = UIStyle.Gray;
                                barcodeDgv.ReadOnly = true;
                                barcodeDgv.RowHeadersVisible = false;
                                barcodeDgv.AllowUserToAddRows = false;
                                barcodeDgv.AllowUserToResizeRows = false;
                                barcodeDgv.MultiSelect = true;
                                barcodeDgv.RowHeadersVisible = false;
                                barcodeDgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                                barcodeDgv.ClearAll();
                                barcodeDgv.AddColumn("名称", "名称");
                                barcodeDgv.AddColumn("结果", "结果");
                            }

                            richTextBox1.Clear();
                        }));
                    }

                    var isRepeatInGgv = false;

                    //if (isPreScan)
                    {
                        for (var i = 0; i < barcodeDgv.RowCount; i++)
                        {
                            var row = barcodeDgv.Rows[i];
                            if (row.Cells.Count >= 2 && row.Cells[1].Value != null && string.Equals(row.Cells[1].Value.ToString(), barcode, StringComparison.CurrentCultureIgnoreCase))
                            {
                                isRepeatInGgv = true;
                                break;
                            }
                        }
                    }

                    if (!isRepeatInGgv)
                    {
                        var code = VisionCommon.VisionConfig.BarcodeInfo[e];
                        var name = code.Name;
                        var standardStrs = new List<string>();

                        for (var i = 0; i < int.Parse(code.Length); i++)
                            standardStrs.Add("?");

                        var keyValue = VisionCommon.IsLeft ? code.KeyWord.Value.L : code.KeyWord.Value.R;

                        var keyIndex = int.Parse(code.KeyWord.Index);
                        foreach (var t1 in keyValue)
                        {
                            standardStrs[keyIndex] = t1.ToString();
                            keyIndex++;
                        }

                        if (VisionCommon.SelectBarcodes.ContainsKey(name))
                        {
                            var v = VisionCommon.SelectBarcodes[name];
                            var g = code.Groups[v];

                            foreach (var gg in g)
                            {
                                var ggSp = gg.Split("，");
                                var ggIndex = int.Parse(ggSp[0]);
                                var ggValue = ggSp[1];

                                foreach (var t1 in ggValue)
                                {
                                    standardStrs[ggIndex] = t1.ToString();
                                    ggIndex++;
                                }
                            }
                        }

                        var standard = standardStrs.Aggregate(string.Empty, (current, ss) => current + ss);
                        var expecedStr = standard;

                        var e1 = e;
                        Invoke(new Action(() =>
                        {
                            if (!isPreScan)
                                uiTabControl1.SelectedIndex = 0;

                        }));

                        string analysisStr;
                        if (ConditionalCodeLine.GetStr(expecedStr, barcode, out analysisStr))
                        {
                            if (!LocalDbHelper.IsRepeat(analysisStr))
                            {
                                _barcodeState[e] = true;

                                Invoke(new Action(() =>
                                {
                                    if (!isPreScan)
                                    {
                                        barcodeDgv.AddRow(code.Name, analysisStr);
                                        barcodeDgv.AutoResizeColumns();
                                    }
                                    else
                                    {
                                        if (_preScanBarcodes.ContainsKey(code.Name))
                                            _preScanBarcodes[code.Name] = analysisStr;
                                        else
                                            _preScanBarcodes.Add(code.Name, analysisStr);
                                    }

                                    richTextBox1.SelectionStart = richTextBox1.TextLength;
                                    richTextBox1.SelectionColor = Color.Green;
                                    richTextBox1.AppendText(string.Format(
                                        "第{0}个/共{1}个二维码, 名称: {2}, 扫描标准: {3}, 长度: {4}{5}\r\n", e1 + 1,
                                        VisionCommon.VisionConfig.BarcodeInfo.Length, code.Name, expecedStr,
                                        int.Parse(code.Length) >= 100 ? ">" : "=", code.Length));

                                    richTextBox1.SelectionStart = richTextBox1.TextLength;
                                    richTextBox1.SelectionColor = Color.Green;
                                    richTextBox1.AppendText(string.Format("扫描数据: {0}, 长度: {1}\r\n", barcode,
                                        barcode.Length));

                                    richTextBox1.SelectionStart = richTextBox1.Text.Length;
                                    richTextBox1.ScrollToCaret();

                                    lblBarcodeScanNow.Text = e != VisionCommon.VisionConfig.BarcodeInfo.Length - 1
                                        ? string.Format("请扫‘{0}’标签！第{1}个/共{2}个二维码", VisionCommon.VisionConfig.BarcodeInfo[e + 1].Name, e1 + 1 + 1, VisionCommon.VisionConfig.BarcodeInfo.Length)
                                        : string.Empty;
                                }));
                            }
                            else
                            {
                                Invoke(new Action(() =>
                                {
                                    richTextBox1.SelectionStart = richTextBox1.TextLength;
                                    richTextBox1.SelectionColor = Color.Red;
                                    richTextBox1.AppendText(string.Format("扫描数据: {0}, 已有检测OK记录\r\n", analysisStr));

                                    richTextBox1.SelectionStart = richTextBox1.Text.Length;
                                    richTextBox1.ScrollToCaret();
                                }));
                            }
                        }
                        else
                        {
                            Invoke(new Action(() =>
                            {
                                richTextBox1.SelectionStart = richTextBox1.TextLength;
                                richTextBox1.SelectionColor = Color.Red;
                                richTextBox1.AppendText(string.Format(
                                    "第{0}个/共{1}个二维码, 名称: {2}, 扫描标准: {3}, 长度: {4}{5}\r\n", e1 + 1,
                                    VisionCommon.VisionConfig.BarcodeInfo.Length, code.Name, expecedStr,
                                    int.Parse(code.Length) >= 100 ? ">" : "=", code.Length));

                                richTextBox1.SelectionStart = richTextBox1.TextLength;
                                richTextBox1.SelectionColor = Color.Red;
                                richTextBox1.AppendText(
                                    string.Format("扫描数据: {0}, 长度: {1}\r\n", barcode, barcode.Length));

                                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                                richTextBox1.ScrollToCaret();
                            }));
                        }
                    }
                    else
                    {
                        Invoke(new Action(() =>
                        {
                            richTextBox1.SelectionStart = richTextBox1.TextLength;
                            richTextBox1.SelectionColor = Color.Red;
                            richTextBox1.AppendText(string.Format("扫描数据: {0}, 请勿重复扫描\r\n", barcode));

                            richTextBox1.SelectionStart = richTextBox1.Text.Length;
                            richTextBox1.ScrollToCaret();
                        }));
                    }

                    break;
                }

                if (_barcodeState.FindAll(f => f == false).Count == 0)
                {
                    Invoke(new Action(() =>
                    {
                        if (barcodeDgv.RowCount > 0 && !isPreScan)
                        {
                            _toSaveBarcodes.Clear();
                            for (var i = 0; i < barcodeDgv.RowCount; i++)
                                _toSaveBarcodes.Add(barcodeDgv.Rows[i].Cells[1].Value.ToString());
                        }

                        if (VisionCommon.VisionConfig.BarcodeInfo.Any())
                        {
                            if (!isPreScan)
                            {
                                lblBarcodeScanNow.Text = string.Format(@"扫码结束，等待启动{0}启动后可以开始扫新标签：请扫‘{1}’标签！第{2}个/共{3}个二维码",
                                    Environment.NewLine, VisionCommon.VisionConfig.BarcodeInfo[0].Name, 1,
                                    VisionCommon.VisionConfig.BarcodeInfo.Length);
                            }
                            else
                            {
                                lblBarcodeScanNow.Text = string.Format(@"扫码结束，等待当前结束后按下启动");
                            }
                        }
                    }));

                    return true;
                }

                if (IsBarcodeScanReset)
                {
                    IsBarcodeScanReset = false;
                    IsBarcodeScanExecute = false;

                    Invoke(new Action(() =>
                    {
                        _preScanBarcodes.Clear();

                        if (!isPreScan)
                        {
                            barcodeDgv.ClearAll();
                            barcodeDgv.AddColumn("名称", "名称");
                            barcodeDgv.AddColumn("结果", "结果");
                        }

                        richTextBox1.Clear();
                    }));

                    return false;
                }
            }
        }

        private void CopyProScanBarcodeToGgv()
        {
            if (_preScanBarcodes.Any())
            {
                _toSaveBarcodes.Clear();

                var keys = _preScanBarcodes.Keys.ToList();
                foreach (var key in keys)
                    _toSaveBarcodes.Add(_preScanBarcodes[key]);

                //_toSaveBarcodes.AddRange(_preScanBarcodes);

                Invoke(new Action(() =>
                {
                    for (var i = 0; i < barcodeDgv.RowCount; i++)
                    {
                        var row = barcodeDgv.Rows[i];
                        row.Cells[1].Value = string.Empty;

                        if (i < _preScanBarcodes.Count)
                            row.Cells[1].Value = _preScanBarcodes[keys[i]];
                    }

                    barcodeDgv.AutoResizeColumns();

                    _preScanBarcodes.Clear();
                    uiTabControl1.SelectedIndex = 0;
                    Thread.Sleep(150);
                }));
            }
        }

        #endregion
    }
}
