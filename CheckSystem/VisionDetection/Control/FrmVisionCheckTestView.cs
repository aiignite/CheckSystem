using CheckSystem.VisionDetection.Control;
using CheckSystem.VisionDetection.Vision;
using CommonUtility;
using CommonUtility.FileOperater;
using Controller;
using Newtonsoft.Json;
using StateMachine;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CheckSystem.VisionDetection
{
    public sealed partial class FrmVisionCheckTestView : UIPage
    {
        private readonly CheckApp _checkApp = new CheckApp("CheckApp");
        private readonly Dictionary<string, DynamicImageViewer> _dynamicImageViewers = new Dictionary<string, DynamicImageViewer>();
        private readonly Dictionary<string, StaticImageViewer> _staticImageViewers = new Dictionary<string, StaticImageViewer>();
        private BackgroundWorker _backgroundWorker;
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
        }

        private BackgroundWorker DeviceBackgroundWorker { get; set; }
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
                    if (g != null)
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

            if (t.ParaType.Contains("电阻"))
                ad = k * ad / (b - ad);
            else
                ad = ad * k + b;

            var isOk = ad >= min && ad <= max;

            if (t.ParaType.Contains("电阻"))
                UpdataMainDgv(t.ParaName, "电阻", ad.ToString(CultureInfo.InvariantCulture), t.ParaUnit,
                    isOk ? "OK" : "NG");
            else
                UpdataMainDgv(t.ParaName, "电性能", ad.ToString(CultureInfo.InvariantCulture), t.ParaUnit,
                    isOk ? "OK" : "NG");

            return isOk;
        }

        public bool CheckLogicDynamicImgs(VisionConfigPara t)
        {
            var visionImgViewer = _dynamicImageViewers[t.ParaName];

            for (var i = 0; i < imgTabControl.TabPages.Count; i++)
            {
                var page = imgTabControl.TabPages[i];
                if (page != null && page.Text == t.ParaName)
                {
                    Invoke(new Action(() =>
                    {
                        imgTabControl.SelectedIndex = i;
                    }));
                    break;
                }
            }
            var func = ToRunVisionConfig.VisionInfo.ToList().Find(f => f.VisionFuncName == t.ParaName);

            var isOk = visionImgViewer.DoWork(func, t, VisionCommon.IsLeft);

            if (!isOk)
            {
                Invoke(new Action(() =>
                {
                    visionImgViewer.Reset();
                }));
                isOk = visionImgViewer.DoWork(func, t, VisionCommon.IsLeft);
            }

            UpdataMainDgv(t.ParaName, "动态图像", "见右表", "", isOk ? "OK" : "NG");
            visionImgViewer.ReleaseImg();
            return isOk;
        }

        public bool CheckLogicStaticImg(VisionConfigPara t)
        {
            var visionImgViewer = _staticImageViewers[t.ParaName];

            for (var i = 0; i < imgTabControl.TabPages.Count; i++)
            {
                var page = imgTabControl.TabPages[i];
                if (page != null && page.Text == t.ParaName)
                {
                    Invoke(new Action(() =>
                    {
                        imgTabControl.SelectedIndex = i;
                    }));
                    break;
                }
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

            UpdataMainDgv(t.ParaName, "静态图像", "见右表", "", isOk ? "OK" : "NG");
            return isOk;
        }

        public bool CheckLogicVersion(VisionConfigPara vcp, bool isLike)
        {
            var binding = vcp.ParaBinding;

            var controllerName = binding.Split('.')[0];
            var fieldName = binding.Split('.')[1];

            var controllersList = VisionCommon.Communicationontollers.Select(t => t.Value).Cast<object>().ToList();
            controllersList.AddRange(VisionCommon.IoControllers.Select(t => t.Value));
            controllersList.AddRange(VisionCommon.CustomContollers.Select(t => t.Value));

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

            if (vcp.ParaGroups != null &&
                vcp.ParaGroups.Any() &&
                !string.IsNullOrEmpty(vcp.ParaGroups[0].ParaGroupValue))
            {
                if (isLike)
                {
                    if (fieldValue.Contains(vcp.ParaGroups[0].ParaGroupValue))
                    {
                        UpdataMainDgv(vcp.ParaName, "信息读取（like）", fieldValue, "", "OK");
                        return true;
                    }

                    UpdataMainDgv(vcp.ParaName, "信息读取（like）", fieldValue, "", "NG");
                    return false;
                }
                else
                {
                    if (fieldValue.Equals(vcp.ParaGroups[0].ParaGroupValue))
                    {
                        UpdataMainDgv(vcp.ParaName, "信息读取（==）", fieldValue, "", "OK");
                        return true;
                    }

                    UpdataMainDgv(vcp.ParaName, "信息读取（==）", fieldValue, "", "NG");
                    return false;
                }
            }

            UpdataMainDgv(vcp.ParaName, isLike ? "信息读取（like）" : "信息读取（==）", fieldValue, "", "NG");
            return false;
        }

        public void InitUi()
        {
            UIStyles.InitColorful(Color.FromArgb(80, 126, 164), Color.White);
            uiTitlePanel1.Text = GetTotalCountShowString();//@" 测试汇总：【日期：" + DateTime.Now.ToString("yyyy/MM/dd") + @"，OK：" + VisionCommon.OkCount + @"/总：" + VisionCommon.TotalCount + @"】";
            uiMarkLabel1.Text = string.Format("测试时间：等待开始\r\n测试结果：等待开始\r\n测试耗时：等待开始");
            uiLedBulb1.Color = Color.DarkGoldenrod;
            //InitMainDgv();

            Text = @"测试页面";
            if (VisionCommon.SelectGroups.Any())
                foreach (var t in VisionCommon.SelectGroups.Keys)
                    Text += @"：" + string.Format("{0}：{1}", t, VisionCommon.SelectGroups[t]);
        }

        public bool InvokeParaAction(VisionConfigPara t)
        {
            var ngCount = 0;

            if (t.ParaType == "静态图像" || t.ParaType == "电性能，静态图像" || t.ParaType == "电性能" || t.ParaType == "电阻")
            {
                VisionCommon.InvokeRelays(t.ParaReleysList);

                if (t.ParaMethods != null)
                    VisionCommon.InvokeCustomCmd(t.ParaMethods.ParaMethodsBefore);

                if (!string.IsNullOrEmpty(t.ParaDelayMs) || t.ParaDelayMs != "/" ||
                    int.Parse(t.ParaDelayMs) > 0)
                    Thread.Sleep(int.Parse(t.ParaDelayMs));

                if (t.ParaType.EndsWith("静态图像"))
                    if (!CheckLogicStaticImg(t))
                        ngCount++;

                if (t.ParaType.StartsWith("电性能") || t.ParaType.StartsWith("电阻"))
                {
                    if (!CheckLogicAd(t))
                    {
                        Thread.Sleep(50);
                        if (!CheckLogicAd(t))
                        {
                            ngCount++;
                        }
                    }
                }

                VisionCommon.InvokeRelays(t.ParaReleysList, true);

                if (t.ParaMethods != null)
                    VisionCommon.InvokeCustomCmd(t.ParaMethods.ParaMethodsAfter);
            }
            else if (t.ParaType == "动态图像")
            {
                if (!CheckLogicDynamicImgs(t))
                    ngCount++;
            }
            else if (t.ParaType.StartsWith("信息读取"))
            {
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
            if (_backgroundWorker != null && !IsChecking && VisionCommon.NiImaqdLoadOk)
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

                _backgroundWorker.RunWorkerAsync();
            }
        }

        public void UpdataMainDgv(string name, string type, string value, string unit, string result)
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

        public void UpdateVisionPara(VisionConfig config)
        {
            if (ToRunVisionConfig != null && config != null && config.VisionInfo != null)
            {
                ToRunVisionConfig.VisionInfo = config.VisionInfo;
            }
        }

        private static void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private static void GetCount()
        {
            {
                //var sqlAll =
                //    string.Format(
                //        "select taskNo from manufactureCheckData where creater = '{0}' and DATEDIFF(DD,createTime,GETDATE()) = 0 group by taskNo",
                //        VisionCommon.VisionDeviceConfig.DeviceInfo.DeviceNo + ":" + VisionCommon.Title);


                //using (var connection = new SqlConnection("server=.;database=IPMS;uid=sa;pwd=123456"))
                //{
                //    var ds = new DataSet();
                //    try
                //    {
                //        connection.Open();
                //        var command = new SqlDataAdapter(sqlAll, connection);
                //        command.Fill(ds, "ds");

                //        var dt = ds.Tables[0];
                //        var rowsCount = dt.Rows.Count;
                //        VisionCommon.TotalCount = rowsCount;
                //    }
                //    catch (SqlException)
                //    {
                //        VisionCommon.TotalCount = 0;
                //    }
                //}

                if (Program.LocalSqlite != null)
                {
                    try
                    {
                        var sql =
                            string.Format(
                                "select taskNo from manufactureCheckData where creater = '{0}' and createTime between datetime('now','start of day','+1 seconds') and  datetime('now','start of day','+1 days','-1 seconds') group by taskNo",
                                VisionCommon.VisionDeviceConfig.DeviceInfo.DeviceNo + ":" + VisionCommon.Title);

                        if (Program.LocalSqlite != null)
                        {
                            var getData = Program.LocalSqlite.GetRows(sql);
                            VisionCommon.TotalCount = getData.Length;
                        }
                    }
                    catch (Exception e)
                    {
                        VisionCommon.TotalCount = 0;
                    }
                }
                else
                {
                    VisionCommon.TotalCount = 0;
                }
            }

            {
                //var sqlOk =
                //    string.Format(
                //        "select taskNo from manufactureCheckData where creater = '{0}' and DATEDIFF(DD,createTime,GETDATE()) = 0 and CheckResult = '0001' group by taskNo",
                //        VisionCommon.VisionDeviceConfig.DeviceInfo.DeviceNo + ":" + VisionCommon.Title);

                //using (var connection = new SqlConnection("server=.;database=IPMS;uid=sa;pwd=123456"))
                //{
                //    var ds = new DataSet();
                //    try
                //    {
                //        connection.Open();
                //        var command = new SqlDataAdapter(sqlOk, connection);
                //        command.Fill(ds, "ds");

                //        var dt = ds.Tables[0];
                //        var rowsCount = dt.Rows.Count;
                //        VisionCommon.OkCount = rowsCount;
                //    }
                //    catch (SqlException)
                //    {
                //        VisionCommon.OkCount = 0;
                //    }
                //}

                var sqlOk = string.Format(
                    "select taskNo from manufactureCheckData where creater = '{0}' and createTime between datetime('now','start of day','+1 seconds') and  datetime('now','start of day','+1 days','-1 seconds') and checkResult = '0001' group by taskNo",
                    VisionCommon.VisionDeviceConfig.DeviceInfo.DeviceNo + ":" + VisionCommon.Title);

                if (Program.LocalSqlite != null)
                {
                    try
                    {
                        var getData = Program.LocalSqlite.GetRows(sqlOk);
                        VisionCommon.OkCount = getData.Length;
                    }
                    catch (Exception e)
                    {
                        VisionCommon.OkCount = 0;
                    }
                }
                else
                {
                    VisionCommon.OkCount = 0;
                }
            }
        }

        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {


                foreach (SqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
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

                    var myTag = con.Tag.ToString().Split(new char[] { ':' }); //获取控件的Tag属性值，并分割后存储字符串数组
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

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;

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
                            if (string.Equals(ToRunVisionConfig.DeviceInfo.Actions.PropulsionCylinder.IsPropulsionCylinder, "true", StringComparison.CurrentCultureIgnoreCase))
                            {
                                VisionCommon.InvokeRelays(new VisionConfigParaParaReleysList { ParaReleysOffList = string.Empty, ParaReleysOnList = ToRunVisionConfig.DeviceInfo.Actions.PropulsionCylinder.Binding });
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

                    if (ToRunVisionConfig.TestFlowInfo == null || ToRunVisionConfig.TestFlowInfo.Length == 0) // 没有特殊，用默认方法
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
                                            var value = string.Equals(sp[1], "ON", StringComparison.CurrentCultureIgnoreCase);
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
                        {
                            ngCount++;
                        }
                    }

                    stopWatch.Stop();

                    if (InvokeRequired)
                    {
                        BeginInvoke(new Action(() =>
                        {
                            if (ngCount > 0)
                            {
                                SaveData(false);
                                uiLedBulb1.Color = Color.Red;
                                uiMarkLabel1.Text =
                                    string.Format("测试时间：" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") +
                                                  "\r\n测试结果：NG\r\n测试耗时：" +
                                                  (stopWatch.ElapsedMilliseconds - otherDelayTime) / 1000f + "秒");
                                _checkApp.Alarm();
                            }
                            else
                            {
                                SaveData(true);
                                uiLedBulb1.Color = Color.Green;
                                uiMarkLabel1.Text =
                                    string.Format("测试时间：" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") +
                                                  "\r\n测试结果：OK\r\n测试耗时：" +
                                                  (stopWatch.ElapsedMilliseconds - otherDelayTime) / 1000f + "秒");

                                if (VisionCommon.VisionDeviceConfig.DeviceInfo.Actions.Bang != null && IsByPass == 2)
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
                                            if (VisionCommon.IoControllers.ContainsKey(VisionCommon.VisionDeviceConfig
                                                    .DeviceInfo.Actions.Bang.Controller))
                                            {
                                                var bangController =
                                                    VisionCommon.IoControllers[
                                                        VisionCommon.VisionDeviceConfig.DeviceInfo.Actions.Bang
                                                            .Controller];

                                                var field = bangController.GetType()
                                                    .GetField(VisionCommon.VisionDeviceConfig.DeviceInfo.Actions.Bang
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
                            if (string.Equals(ToRunVisionConfig.DeviceInfo.Actions.PropulsionCylinder.IsPropulsionCylinder, "true",
                                    StringComparison.CurrentCultureIgnoreCase))
                            {
                                VisionCommon.InvokeRelays(new VisionConfigParaParaReleysList
                                {
                                    ParaReleysOffList = string.Empty,
                                    ParaReleysOnList = ToRunVisionConfig.DeviceInfo.Actions.PropulsionCylinder.Binding
                                }, true);
                                Thread.Sleep(450);
                            }
                        }
                        catch (Exception a)
                        {
                            Console.WriteLine(a);
                        }
                    }

                    _backgroundWorker.CancelAsync();
                    IsChecking = false;
                }
            }
            catch (Exception exception)
            {
                // ignored
            }
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void FrmVisionCheckTestView_Load(object sender, EventArgs e)
        {
            _x = this.Width;//获取窗体的宽度
            _y = this.Height;//获取窗体的高度
            SetTag(uiTitlePanel1);//调用方法

            Resize += FrmVisionCheckTestView_Resize;

            VisionCommon.LoadDevice();

            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.DoWork += backgroundWorker_DoWork;
            _backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            _backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
            _backgroundWorker.WorkerReportsProgress = true;
            _backgroundWorker.WorkerSupportsCancellation = true;

            DeviceBackgroundWorker = new BackgroundWorker();
            DeviceBackgroundWorker.DoWork += DeviceBackgroundWorker_DoWork;
            DeviceBackgroundWorker.RunWorkerCompleted += DeviceBackgroundWorker_RunWorkerCompleted;
            DeviceBackgroundWorker.ProgressChanged += DeviceBackgroundWorker_ProgressChanged;
            DeviceBackgroundWorker.WorkerReportsProgress = true;
            DeviceBackgroundWorker.WorkerSupportsCancellation = true;
            DeviceBackgroundWorker.RunWorkerAsync();
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

        private string GetTotalCountShowString()
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

        private void SaveData(bool isOk)
        {
            if (IsByPass == 2)
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
                    var name = string.Format("{0}_{1}", mainDgv.Rows[i].Cells[0].Value, mainDgv.Rows[i].Cells[1].Value);
                    var data = mainDgv.Rows[i].Cells[3].Value.ToString();
                    var range = mainDgv.Rows[i].Cells[2].Value.ToString();
                    var result = string.Equals(mainDgv.Rows[i].Cells[4].Value.ToString(), "OK", StringComparison.CurrentCultureIgnoreCase)
                        ? "True"
                        : "False";
                    var value = mainDgv.Rows[i].Cells[3].Value.ToString();
                    var type = mainDgv.Rows[i].Cells[1].Value.ToString();

                    if (data == "见右表")
                    {
                        data = mainDgv.Rows[i].Cells[4].Value.ToString();
                    }
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
            if (barcodeDgv.RowCount > 0)
            {
                for (var i = 0; i < barcodeDgv.RowCount; i++)
                    barcodeList.Add(barcodeDgv.Rows[i].Cells[1].Value.ToString());
            }

            var savelocalAction = new Action<string, string, string, List<SyProductionSaveCheckData.CheckDataDetail>>((taskid, uid, productNo, checkData) =>
            {
                if (Program.LocalSqlite != null)
                {
                    try
                    {
                        var insert =
                            string.Format(
                                "insert into manufactureCheckData (taskNo,productNo,productUid,pcbaNo,pcbaBarcode,productBarcode,packageBarcode,processNo,checkData,checkDate,checkStaffNo,checkResult,creater,createTime,checkDataText) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}')",
                                taskid, productNo, uid, string.Empty, uid, uid, string.Empty, productNo + "_001", JsonConvert.SerializeObject(checkData),
                                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), VisionCommon.LoginUser, isOk ? "0001" : "0002", VisionCommon.VisionDeviceConfig.DeviceInfo.DeviceNo + ":" + VisionCommon.Title,
                                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), JsonConvert.SerializeObject(checkData));

                        Program.LocalSqlite.Query(insert);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            });

            var saveRemoteAction =
                new Action<string, string, List<string>, List<SyProductionSaveCheckData.CheckDataDetail>>(
                    (taskid, productNo, barcodes, checkData) =>
                    {
                        var strSql = new StringBuilder();
                        strSql.Append("insert into SyProductionCheckData(");
                        strSql.Append(
                            "ItemId,MaterialNo,MaterialLotNo,MaterialUid,MaterialBarcode,MaterialCustomerBarcode,SubMaterialsInfo,DeviceNo,DeviceName,Result,CheckData,MaterialName,CreateTime)");
                        strSql.Append(" values (");
                        strSql.Append(
                            "@ItemId,@MaterialNo,@MaterialLotNo,@MaterialUid,@MaterialBarcode,@MaterialCustomerBarcode,@SubMaterialsInfo,@DeviceNo,@DeviceName,@Result,@CheckData,@MaterialName,GETDATE())");
                        strSql.Append(";select @@IDENTITY");

                        var getLotNoSql =
                            string.Format(
                                "SELECT COUNT(MaterialNo) FROM SyProductionCheckData WHERE DATEDIFF(DD,createTime,GETDATE()) = 0 AND MaterialNo = '{0}'",
                                productNo);

                        var lotNo = -1;
                        using (
                            var connection = new SqlConnection("server=192.168.0.136;database=PLMS;uid=sa;pwd=123456"))
                        {
                            var ds = new DataSet();
                            try
                            {
                                connection.Open();
                                var command = new SqlDataAdapter(getLotNoSql, connection);
                                command.Fill(ds, "ds");

                                var dt = ds.Tables[0];
                                if (dt.Rows.Count > 0)
                                {
                                    lotNo = int.Parse(dt.DefaultView[0].Row[0].ToString()) + 1;
                                }
                            }
                            catch (SqlException)
                            {
                            }
                        }

                        if (lotNo > -1)
                        {
                            var itemId = taskid;
                            var materialNo = productNo;
                            var materialLotNo = string.Format("{0}{1}", DateTime.Now.ToString("yyyMMdd"),
                                lotNo.ToString().PadLeft(4, '0'));

                            string materialUid;
                            string materialBarcode;
                            string materialCustomerBarcode;
                            var subMaterialsInfo = string.Empty;

                            var device = VisionCommon.VisionDeviceConfig.DeviceInfo.DeviceNo;
                            var checkDataJsonStr = JsonConvert.SerializeObject(checkData);
                            var createTime = DateTime.Now;

                            string result;
                            if (IsByPass == 2)
                                result = isOk ? "0001" : "0002";
                            else
                                result = isOk ? "9001" : "9002";

                            if (barcodes.Any())
                            {
                                materialUid = !string.IsNullOrEmpty(barcodes[0]) ? barcodes[0] : materialLotNo;
                                materialBarcode = !string.IsNullOrEmpty(barcodes[0]) ? barcodes[0] : materialLotNo;
                                materialCustomerBarcode = !string.IsNullOrEmpty(barcodes[0])
                                    ? barcodes[0]
                                    : materialLotNo;

                                subMaterialsInfo =
                                    barcodes.Where(b => !string.IsNullOrEmpty(b))
                                        .Aggregate(subMaterialsInfo, (current, b) => current + b + "|");

                                subMaterialsInfo = subMaterialsInfo.TrimEnd('|');
                            }
                            else
                            {
                                materialUid = materialLotNo;
                                materialBarcode = materialLotNo;
                                materialCustomerBarcode = materialLotNo;
                            }

                            SqlParameter[] parameters =
                            {
                                new SqlParameter("@ItemId", SqlDbType.NVarChar), //0
                                new SqlParameter("@MaterialNo", SqlDbType.NVarChar), //1
                                new SqlParameter("@MaterialLotNo", SqlDbType.NVarChar), //2
                                new SqlParameter("@MaterialUid", SqlDbType.NVarChar), //3
                                new SqlParameter("@MaterialBarcode", SqlDbType.NVarChar), //4
                                new SqlParameter("@MaterialCustomerBarcode", SqlDbType.NVarChar), //5
                                new SqlParameter("@SubMaterialsInfo", SqlDbType.NVarChar), //6
                                new SqlParameter("@DeviceNo", SqlDbType.NVarChar), //7
                                new SqlParameter("@DeviceName", SqlDbType.NVarChar), //8
                                new SqlParameter("@Result", SqlDbType.NVarChar), //9
                                new SqlParameter("@CheckData", SqlDbType.Text), //10
                                //new SqlParameter("@CreateTime", SqlDbType.DateTime), //11
                                new SqlParameter("@MaterialName", SqlDbType.NVarChar), //11
                            };
                            parameters[0].Value = itemId;
                            parameters[1].Value = materialNo;
                            parameters[2].Value = materialLotNo;
                            parameters[3].Value = materialUid;
                            parameters[4].Value = materialBarcode;
                            parameters[5].Value = materialCustomerBarcode;
                            parameters[6].Value = subMaterialsInfo;
                            parameters[7].Value = device;
                            parameters[8].Value = device;
                            parameters[9].Value = result;
                            parameters[10].Value = checkDataJsonStr;
                            //parameters[11].Value = createTime;
                            parameters[11].Value = materialName;

                            using (
                                var connection =
                                    new SqlConnection("server=192.168.0.136;database=PLMS;uid=sa;pwd=123456"))
                            {
                                using (var cmd = new SqlCommand())
                                {
                                    try
                                    {
                                        PrepareCommand(cmd, connection, null, strSql.ToString(), parameters);
                                        var obj = cmd.ExecuteScalar();
                                        cmd.Parameters.Clear();
                                        if (Equals(obj, null) || (Equals(obj, DBNull.Value)))
                                        {
                                            //return null;
                                        }
                                    }
                                    catch (SqlException e)
                                    {
                                        //throw e;
                                    }
                                }
                            }
                        }
                    });

            var guid = Guid.NewGuid().ToString();
            if (IsByPass == 2)
            {
                if (barcodeDgv.RowCount > 0)
                    for (var i = 0; i < barcodeDgv.RowCount; i++)
                        savelocalAction.BeginInvoke(guid, barcodeDgv.Rows[i].Cells[1].Value.ToString(), prNo, checkDatas2, null, null);
                else
                    savelocalAction.BeginInvoke(guid, string.Empty, prNo, checkDatas2, null, null);
            }

            saveRemoteAction.BeginInvoke(guid, prNo, barcodeList, checkDatas2, null, null);
        }

        #region device
        private readonly List<bool> _barcodeState = new List<bool>();

        private bool IsBarcodeScanExecute { get; set; }

        private bool IsBarcodeScanReset { get; set; }

        public bool IsRepeatTest { get; set; }

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
                        return true;
                    }
                    if (field.GetValue(c).ToString() == "0")
                    {
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
                        return true;
                    }
                    if (string.Equals(field.GetValue(c).ToString(), bool.FalseString, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        private void btnBarcodeScanReset_Click(object sender, EventArgs e)
        {
            if (IsBarcodeScanExecute && !IsBarcodeScanReset)
            {
                if (ShowAskDialog("是否重新开始扫码"))
                {
                    IsBarcodeScanReset = true;
                    ShowSuccessTip("扫码已经重置");
                }
            }
        }

        private void DeviceBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var step = 0;

            while (!DeviceBackgroundWorker.CancellationPending)
            {
                if (!IsChecking && ToRunVisionConfig != null && VisionCommon.NiImaqdLoadOk)
                {
                    var startType = ToRunVisionConfig.DeviceInfo.Actions.Start.Type;

                    if (IsRepeatTest)
                    {
                        if (step == 0)
                        {
                            step = 1;
                        }
                        else if (step == 1)
                        {
                            Start(2);
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
                                    step = 1;
                            }
                            else if (step == 1)
                            {
                                Start(2);
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
                                step = 0;
                            }
                        }
                        else if (startType == "扫码后按键启动")
                        {
                            if (step == 0)
                            {
                                if (WaitBarcode())
                                    step = 1;
                            }
                            else if (step == 1)
                            {
                                if (WaitDi())
                                {
                                    step = 2;
                                }
                            }
                            else if (step == 2)
                            {
                                Start(2);
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
                                {
                                    step = 2;
                                }
                            }
                            else if (step == 2)
                            {
                                Start(2);
                                step = 0;
                            }
                        }
                    }
                }
            }
        }

        private void DeviceBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void DeviceBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private bool WaitBarcode()
        {
            _barcodeState.Clear();

            Invoke(new Action(() =>
            {
                if (VisionCommon.VisionConfig.BarcodeInfo.Any())
                    lblBarcodeScanNow.Text = string.Format("请扫‘{0}’标签！", VisionCommon.VisionConfig.BarcodeInfo[0].Name);
            }));

            if (VisionCommon.VisionConfig.BarcodeInfo == null ||
                !VisionCommon.VisionConfig.BarcodeInfo.Any())
                return false;

            for (var i = 0; i < VisionCommon.VisionConfig.BarcodeInfo.Length; i++)
                _barcodeState.Add(false);

            while (true)
            {
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
                else
                {
                    IsBarcodeScanExecute = true;

                    VisionCommon.BarcodeScanReader.ReadBarcodeTimeoutMs = 2000;
                    VisionCommon.BarcodeScanReader.ReadBarcode(1);
                    if (string.IsNullOrEmpty(VisionCommon.BarcodeScanReader.GetBarcodeStr))
                    {
                        if (IsBarcodeScanReset)
                        {
                            IsBarcodeScanReset = false;
                            IsBarcodeScanExecute = false;
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
                            BeginInvoke(new Action(() =>
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

                                richTextBox1.Clear();
                            }));
                        }

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

                        if (VisionCommon.SelectBrocdes.ContainsKey(name))
                        {
                            var v = VisionCommon.SelectBrocdes[name];
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
                        BeginInvoke(new Action(() =>
                        {
                            uiTabControl1.SelectedIndex = 0;
                        }));

                        string analysisStr;
                        if (ConditionalCodeLine.GetStr(expecedStr, barcode, out analysisStr))
                        {
                            if (!IsRepeat(analysisStr))
                            {
                                _barcodeState[e] = true;

                                BeginInvoke(new Action(() =>
                                {
                                    barcodeDgv.AddRow(code.Name, analysisStr);
                                    barcodeDgv.AutoResizeColumns();

                                    richTextBox1.SelectionStart = richTextBox1.TextLength;
                                    richTextBox1.SelectionColor = Color.Green;
                                    richTextBox1.AppendText(string.Format("第{0}个/共{1}个二维码, 名称: {2}, 扫描标准: {3}, 长度: {4}{5}\r\n", e1 + 1,
                                        VisionCommon.VisionConfig.BarcodeInfo.Length, code.Name, expecedStr, int.Parse(code.Length) >= 100 ? ">" : "=", code.Length));

                                    richTextBox1.SelectionStart = richTextBox1.TextLength;
                                    richTextBox1.SelectionColor = Color.Green;
                                    richTextBox1.AppendText(string.Format("扫描数据: {0}, 长度: {1}\r\n", barcode, barcode.Length));

                                    richTextBox1.SelectionStart = richTextBox1.Text.Length;
                                    richTextBox1.ScrollToCaret();

                                    lblBarcodeScanNow.Text = e != VisionCommon.VisionConfig.BarcodeInfo.Length - 1
                                        ? string.Format("请扫‘{0}’标签！", VisionCommon.VisionConfig.BarcodeInfo[e + 1].Name)
                                        : string.Empty;
                                }));
                            }
                            else
                            {
                                BeginInvoke(new Action(() =>
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
                            BeginInvoke(new Action(() =>
                            {
                                richTextBox1.SelectionStart = richTextBox1.TextLength;
                                richTextBox1.SelectionColor = Color.Red;
                                richTextBox1.AppendText(string.Format("第{0}个/共{1}个二维码, 名称: {2}, 扫描标准: {3}, 长度: {4}{5}\r\n", e1 + 1,
                                    VisionCommon.VisionConfig.BarcodeInfo.Length, code.Name, expecedStr, int.Parse(code.Length) >= 100 ? ">" : "=", code.Length));

                                richTextBox1.SelectionStart = richTextBox1.TextLength;
                                richTextBox1.SelectionColor = Color.Red;
                                richTextBox1.AppendText(string.Format("扫描数据: {0}, 长度: {1}\r\n", barcode, barcode.Length));

                                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                                richTextBox1.ScrollToCaret();
                            }));
                        }

                        break;
                    }

                    if (_barcodeState.FindAll(f => f == false).Count == 0)
                    {
                        IsBarcodeScanExecute = false;
                        return true;
                    }

                    if (IsBarcodeScanReset)
                    {
                        IsBarcodeScanReset = false;
                        IsBarcodeScanExecute = false;
                        return false;
                    }
                }
            }
        }

        private bool IsRepeat(string barcode)
        {
            // select productBarcode from manufactureCheckData where productBarcode = 'ABCDE' and checkResult = '0001'

            if (Program.LocalSqlite != null)
            {
                try
                {
                    var sql =
                        string.Format(
                            "select productBarcode from manufactureCheckData where productBarcode = '{0}' and checkResult = '0001'",
                            barcode);

                    if (Program.LocalSqlite != null)
                    {
                        var getData = Program.LocalSqlite.GetRows(sql);
                        return getData.Length > 0;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return false;
        }

        #endregion
    }
}
