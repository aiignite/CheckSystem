using CheckSystem.HelperForms.S12LDataTrack;
using CheckSystem.RobotForms;
using CheckSystem.VisionDetection;
using CommonUtility;
using Controller;
using DBUtility;
using NationalInstruments.Vision;
using Newtonsoft.Json;
using StateMachine;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserControls;

namespace CheckSystem
{
    public partial class FormCheck : UIForm
    {
        public static readonly State St = new State();

        private Thread Th { get; set; }
        private string XmlFile { get; set; }

        #region 界面变量
        private readonly TableLayoutPanel _tableLayoutPanel = new TableLayoutPanel();
        private readonly List<Control> _lstControls = new List<Control>();
        private readonly Dictionary<string, DateTime> _dateTimeList = new Dictionary<string, DateTime>();
        private Form _canMsgForm = new FormCanMsgMonitor();
        private Form _linMsgForm = new FormLinMsgMonitor();
        private Form _debugForm;
        private Form _partMonitorForm;
        #endregion

        public class CheckNameAndData
        {
            public string Name;
            public string Data;
            //public string Judge;
        }

        public sealed override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        public FormCheck(string xmlFile)
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            WindowState = FormWindowState.Maximized;
            FormClosed += FormMain_FormClosed;
            Load += FormCheck_Load;

            var path = Program.SysDir;
            XmlFile = xmlFile;
            St.PushDisplay += St_PushDisplay;
            St.PushEndResult += St_PushEndResult;
            St.PushEnter += St_PushEnter;
            St.Init<ControllerBase>(XmlFile, "Controller.dll");
            InitStateInfo(St.DeviceConfig);
            Text = buttonTitle.Text = St.DeviceConfig.DeviceInfo.DeviceName;
            Icon = new Icon(path + @"\icon.ico");
            InitFormControls(St.DeviceConfig);
            TaskCheck();
            PingNetWork();
        }

        private void FormCheck_Load(object sender, EventArgs e)
        {
            ImageProcessing.PushVisionImgMsg += ImageProcessing_PushVisionImgMsg;
            ControllerBase.PushControllerMsg += ControllerBase_PushMsg;

            打开图像标定ToolStripMenuItem.Visible = false;
            foreach (var t in St.LstControllers)
            {
                //if (t is LedVisionAnalysisByDaHengCamera)
                //    打开图像标定ToolStripMenuItem.Visible = true;

                if (t is VisionAnalysis)
                    打开图像标定ToolStripMenuItem.Visible = true;

                if (t is RobotControllerPpMode)
                    打开机器人监控ToolStripMenuItem.Visible = true;

                if (t is S12LHeatPumpController)
                    s12L数据查询ToolStripMenuItem.Visible = true;
            }

            if (_tableLayoutPanel.Controls.Count != 0)
                return;
            using (var form = new FormStateMonitor(St))
                form.ShowDialog();
        }

        private async void PingNetWork()
        {
            await Task.Run(() =>
            {
                // ping 192.168.0.136
                // 远程有响应就是用远程SERVER
                // 远程无响应就是用本地SERVER
                var remoteServer = ConfigurationManager.AppSettings["RemoteServer"];
                var p1 = new Ping();
                try
                {
                    var reply = p1.Send(remoteServer); //发送主机名或Ip地址
                    if (reply != null && reply.Status == IPStatus.Success)
                    {
                        PubConstant.ConnectionStringKey = "RemoteConnectionString";
                        ControllerBase_PushMsg(@"主程序",
                            string.Format(@"PING服务器：[{0}]响应,将使用为服务器：[{1}]存储检测数据", remoteServer, remoteServer));
                        Text += @" (Using Remote SQL Server)";
                    }
                    else
                    {
                        ControllerBase_PushMsg(@"主程序", string.Format(@"PING服务器：[{0}]无响应,将使用为本地服务器存储检测数据", remoteServer));
                        Text += @" (Using Local SQL Server)";
                    }
                }
                catch (Exception ex)
                {
                    ControllerBase_PushMsg(@"主程序",
                        string.Format(@"PING服务器：[{0}]异常：{1},将使用为本地服务器存储检测数据", remoteServer, ex.Message));
                    Text += @" (Using Local SQL Server)";
                }
            });
        }

        private void ImageProcessing_PushVisionImgMsg(
            VisionImage visionImage)
        {
            foreach (var imageViewer in
                _lstControls.Where(control => control.GetType() ==
                                              typeof(UserImageDataViewer)).OfType<UserImageDataViewer>())
                imageViewer.ShowImg(visionImage);
        }

        #region 状态机事件
        private void ControllerBase_PushMsg(string controllerName, string msgContent)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    if (contrrollerMsgGrid.dataGridView.ColumnCount == 0)
                    {
                        contrrollerMsgGrid.label.Text = @"消息展示列表";
                        contrrollerMsgGrid.label.Visible = false;
                        contrrollerMsgGrid.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "接收时间" });
                        contrrollerMsgGrid.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "控制器名称" });
                        contrrollerMsgGrid.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "消息内容" });
                        contrrollerMsgGrid.dataGridView.ReadOnly = true;
                        contrrollerMsgGrid.dataGridView.RowHeadersVisible = false;
                        for (var i = 0; i < contrrollerMsgGrid.dataGridView.Columns.Count; i++)
                            contrrollerMsgGrid.dataGridView.Columns[i].SortMode =
                                DataGridViewColumnSortMode.NotSortable;
                    }

                    if (contrrollerMsgGrid.dataGridView.Rows.Count > 100)
                        contrrollerMsgGrid.dataGridView.Rows.Clear();

                    var index = contrrollerMsgGrid.dataGridView.Rows.Add();

                    contrrollerMsgGrid.dataGridView.Rows[index].Cells[0].Value =
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff");
                    contrrollerMsgGrid.dataGridView.Rows[index].Cells[1].Value = controllerName;
                    contrrollerMsgGrid.dataGridView.Rows[index].Cells[2].Value = msgContent;
                }));
            }
        }

        private void InitStateInfo(DeviceConfig deviceConfig)
        {
            //foreach (var t in deviceConfig.WorkStations)
            //    _statusInfo.Add(new StatusInfo { Name = t.Name, Value = t.InitStatusUnit });

            //var bs = new BindingList<StatusInfo>(_statusInfo);
            //statesGridView.DataSource = bs;
            //statesGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            //statesGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //statesGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            //statesGridView.AllowUserToResizeColumns = false;
            //statesGridView.AllowUserToResizeRows = false;
            //statesGridView.ReadOnly = true;
            //statesGridView.RowHeadersVisible = false;

            statesGridView.label.Text = @"状态列表";
            statesGridView.label.Visible = false;
            statesGridView.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "工作站" });
            statesGridView.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "当前状态" });
            statesGridView.dataGridView.ReadOnly = true;
            statesGridView.dataGridView.RowHeadersVisible = false;
            for (var i = 0; i < statesGridView.dataGridView.Columns.Count; i++)
                statesGridView.dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

            foreach (var t in deviceConfig.WorkStations)
            {
                var rowAddIndex = statesGridView.dataGridView.Rows.Add();

                var row = statesGridView.dataGridView.Rows[rowAddIndex];
                row.Cells[0].Value = t.Name;
                row.Cells[1].Value = StateMachineHelper.EDefaultStateUnits.Idle.ToString().ToUpper();
            }
        }

        private void St_PushEnter(
            string wsName, string enterStaus, string[] enterAction)
        {
            for (var i = 0; i < statesGridView.dataGridView.RowCount; i++)
            {
                var row = statesGridView.dataGridView.Rows[i];
                if (row.Cells[0].Value.ToString() != wsName)
                    continue;
                row.Cells[1].Value = enterStaus;
                break;
            }
        }
        #endregion

        private void TaskCheck()
        {
            if (St.BarcodeGroupList.Count != 0 || St.LedGroupList.Count != 0)
            {
                if (new FormGroupSelect(St).ShowDialog() != DialogResult.OK)
                {
                    MessageBox.Show(@"请重启程序选择档位！");
                    buttonTitle.BackColor = Color.DarkRed;
                    operationToolStripMenuItem.Enabled = false;
                    stateTab.Enabled = false;
                    return;
                }
            }

            //if (St.BarcodeGroupList.Count != 0 &&
            //    (St.BarcodeGroupList.Count == 0 || new FormGroupSelect(St).ShowDialog() != DialogResult.OK))
            //    return;

            //St.TaskCheckStatus();

            if (Th != null)
            {
                Th.Abort();
                Th.Join();
            }

            Th = new Thread(() => { St.TaskCheckStatus(); }) { IsBackground = true };
            Th.Start();
        }

        private static void FormMain_FormClosed(
            object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
            //if (InvokeRequired)
            //{
            //    Invoke(new Action(() =>
            //    {
            //        try
            //        {
            //            Environment.Exit(0);
            //        }
            //        catch (Exception)
            //        {
            //            // ignored
            //        }
            //    }));
            //}
        }

        #region 显示结果数据

        private void St_PushDisplay(List<CheckData> value)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    foreach (var v in value)
                    {
                        var v1 = v;

                        var para =
                            St.DeviceConfig.Paras.ToList()
                                .Find(t => t.ProcessNo == v1.ProcessNo && t.Name == v1.ParaName && t.DataType?.ToLower() != "roi");

                        if (para == null)
                        {
                            if (v1.Type?.ToLower() == "roi")
                            {
                                para = St.DeviceConfig.Paras.ToList().Find(t => t.ProcessNo == v1.ProcessNo && t.Name.ToLower() == v1.Unit.ToLower() && t.OkFormat.ToLower() == v1.Format.ToLower());
                                if (para == null)
                                    continue;
                            }
                            else if (v1.Type?.ToLower() == "静态图像".ToLower())
                            {
                                para = St.DeviceConfig.Paras.ToList().Find(t => t.ProcessNo == v1.ProcessNo && t.Name.ToLower() == v1.ParaName.ToLower() && t.DataType.ToLower() == "roi");
                                if (para == null)
                                    continue;
                            }
                            else
                            {
                                continue;
                            }
                        }

                        //查找参数表对应界面控件
                        var control = _lstControls.Find(t => t.Name == para.ControlName);
                        if (control == null)
                        {
                            if (
                                St.DeviceConfig.Controls.ToList()
                                    .Find(f => f.Name == para.ControlName && f.Type == "ListViewItem") != null)
                            {
                                var controlUi1 = St.DeviceConfig.Controls.ToList()
                                    .Find(t => t.Name == para.ControlName);
                                if (controlUi1.Type == "ListViewItem")
                                {
                                    #region ListViewItem

                                    var findListView =
                                        St.DeviceConfig.Controls.ToList()
                                            .Find(
                                                f =>
                                                    f.Type == "ListView" &&
                                                    f.ColumnPosition == controlUi1.ColumnPosition &&
                                                    f.ColumnSpan == controlUi1.ColumnSpan &&
                                                    f.RowPosition == controlUi1.RowPosition &&
                                                    f.RowSpan == controlUi1.RowSpan);

                                    if (findListView != null)
                                    {
                                        var listView = _lstControls.Find(t => t.Name == findListView.Name);
                                        var listView1 = listView as UserDataGrid;
                                        if (listView1 != null)
                                        {
                                            var cellIndex = -1;
                                            for (var i = 0; i < listView1.dataGridView.ColumnCount; i++)
                                            {
                                                if (listView1.dataGridView.Columns[i].HeaderText == controlUi1.Name)
                                                {
                                                    cellIndex = i;
                                                    break;
                                                }
                                            }

                                            if (cellIndex != -1)
                                            {
                                                if (listView1.dataGridView.Rows[0].Cells[cellIndex].Style.BackColor ==
                                                    Color.Green ||
                                                    listView1.dataGridView.Rows[0].Cells[cellIndex].Style.BackColor ==
                                                    Color.Red)
                                                {
                                                    listView1.dataGridView.Rows[0].Cells[cellIndex].Value = @"/";
                                                    listView1.dataGridView.Rows[0].Cells[cellIndex].Style.BackColor =
                                                        listView1.dataGridView.Rows[0].Cells[0].Style.BackColor;

                                                    listView1.dataGridView.Rows[1].Cells[cellIndex].Value = @"/";
                                                    listView1.dataGridView.Rows[1].Cells[cellIndex].Style.BackColor =
                                                        listView1.dataGridView.Rows[0].Cells[0].Style.BackColor;

                                                    for (var i = 2; i < listView1.dataGridView.RowCount; i++)
                                                    {
                                                        listView1.dataGridView.Rows[i].Cells[cellIndex].Value =
                                                            string.Empty;
                                                        listView1.dataGridView.Rows[i].Cells[cellIndex].Style.BackColor
                                                            = listView1.dataGridView.Rows[0].Cells[0].Style.BackColor;
                                                    }
                                                }

                                                //if (listView1.Items[0].SubItems[cellIndex].BackColor == Color.Green ||
                                                //    listView1.Items[0].SubItems[cellIndex].BackColor == Color.Red)
                                                //{
                                                //    listView1.Items[0].SubItems[cellIndex].Text = @"/";
                                                //    listView1.Items[0].SubItems[cellIndex].BackColor = Color.Azure;

                                                //    listView1.Items[1].SubItems[cellIndex].Text = @"/";
                                                //    listView1.Items[1].SubItems[cellIndex].BackColor = Color.Azure;

                                                //    for (var i = 2; i < listView1.Items.Count; i++)
                                                //    {
                                                //        listView1.Items[i].SubItems[cellIndex].Text = string.Empty;
                                                //        listView1.Items[i].SubItems[cellIndex].BackColor = Color.White;
                                                //    }
                                                //}

                                                var name = v1.ParaName;
                                                var range = v1.Range;
                                                var checkValue = v.Value;
                                                var checkResult = v1.Result.ToLower();

                                                if (!string.IsNullOrEmpty(v1.Unit))
                                                {
                                                    checkValue += v1.Type?.ToLower() == "roi" ? string.Empty : @"/" + v1.Unit;
                                                    range += v1.Type?.ToLower() == "roi" ? string.Empty : @"/" + v1.Unit;
                                                }

                                                var isHave = -1;
                                                for (var i = 2; i < listView1.dataGridView.RowCount; i++)
                                                {
                                                    if ((string)listView1.dataGridView.Rows[i].Cells[0].Value == name)
                                                    {
                                                        isHave = i;
                                                        break;
                                                    }
                                                }

                                                //for (var i = 2; i < listView1.Items.Count; i++)
                                                //{
                                                //    if (listView1.Items[i].SubItems[0].Text == para.Name)
                                                //    {
                                                //        isHave = i;
                                                //        break;
                                                //    }
                                                //}

                                                if (isHave != -1)
                                                {
                                                    listView1.dataGridView.Rows[isHave].Cells[cellIndex].Value =
                                                        checkValue;
                                                    listView1.dataGridView.Rows[isHave].Cells[cellIndex].Style.BackColor
                                                        =
                                                        checkResult == "false" || checkResult == "0"
                                                            ? Color.Red
                                                            : listView1.dataGridView.Rows[0].Cells[0].Style.BackColor;
                                                }
                                                else
                                                {
                                                    var rowsAddIndex = listView1.dataGridView.Rows.Add();
                                                    var rowAdd = listView1.dataGridView.Rows[rowsAddIndex];
                                                    rowAdd.Cells[0].Value = para.Name;
                                                    rowAdd.Cells[1].Value = range;

                                                    for (var i = 2; i < listView1.dataGridView.ColumnCount; i++)
                                                    {
                                                        if (i == cellIndex)
                                                        {
                                                            rowAdd.Cells[i].Value = checkValue;
                                                            rowAdd.Cells[i].Style.BackColor = checkResult == "false" ||
                                                                checkResult == "0"
                                                                    ? Color.Red
                                                                    : listView1.dataGridView.Rows[0].Cells[0].Style
                                                                        .BackColor;
                                                        }
                                                        else
                                                        {
                                                            rowAdd.Cells[i].Value = string.Empty;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    #endregion
                                }
                            }

                            continue;
                        }

                        //查找界面控件对应的配置
                        var controlUi =
                            St.DeviceConfig.Controls.ToList().Find(t => t.Name == control.Name);
                        if (controlUi == null)
                            continue;

                        if (controlUi.Type == "DataGrid")
                        {
                            #region DataGrid data

                            var dgv = (UserDataGrid)control;
                            if (dgv.label.BackColor != Color.Azure)
                                dgv.dataGridView.Rows.Clear();

                            if (dgv.dataGridView.Rows.Count == 0)
                                if (_dateTimeList.ContainsKey(control.Name))
                                    _dateTimeList[control.Name] = DateTime.Now;

                            dgv.label.BackColor = Color.Azure;
                            dgv.label.Text = control.Name;

                            var name = v1.ParaName;
                            var range = v1.Range;
                            var checkValue = v.Value;
                            var checkResult = v1.Result.ToLower();

                            if (!string.IsNullOrEmpty(v1.Unit))
                            {
                                checkValue += v1.Type?.ToLower() == "roi" ? string.Empty : @"/" + v1.Unit;
                                range += v1.Type?.ToLower() == "roi" ? string.Empty : @"/" + v1.Unit;
                            }

                            var isHave = -1;
                            for (var i = 0; i < dgv.dataGridView.RowCount; i++)
                            {
                                if (dgv.dataGridView.Rows[i].Cells[0].Value.ToString() != name)
                                    continue;

                                isHave = i;
                                break;
                            }

                            if (isHave > -1)
                            {
                                dgv.dataGridView.Rows[isHave].Cells[1].Value = range;
                                dgv.dataGridView.Rows[isHave].Cells[2].Value = checkValue;
                                dgv.dataGridView.Rows[isHave].Cells[3].Value =
                                    checkResult == "false" || checkResult == "0"
                                        ? "NG"
                                        : "OK";
                                dgv.dataGridView.Rows[isHave].DefaultCellStyle.BackColor =
                                    checkResult == "false" || checkResult == "0"
                                        ? Color.Red
                                        : Color.White;
                            }
                            else
                            {
                                var index = dgv.dataGridView.Rows.Add();
                                dgv.dataGridView.Rows[index].Cells[0].Value = name;
                                dgv.dataGridView.Rows[index].Cells[1].Value = range;
                                dgv.dataGridView.Rows[index].Cells[2].Value = checkValue;
                                dgv.dataGridView.Rows[index].Cells[3].Value =
                                    checkResult == "false" || checkResult == "0"
                                        ? "NG"
                                        : "OK";
                                dgv.dataGridView.Rows[index].DefaultCellStyle.BackColor =
                                    checkResult == "false" || checkResult == "0"
                                        ? Color.Red
                                        : Color.White;
                            }

                            dgv.dataGridView.FirstDisplayedScrollingRowIndex = dgv.dataGridView.RowCount - 1;

                            #endregion
                        }
                        else if (controlUi.Type == "NiImageViewer")
                        {
                            #region ImageViewer DataGrid data

                            var imgDgv = (UserImageDataViewer)control;
                            if (imgDgv.userDataGridCheckData.label.BackColor != Color.Azure)
                            {
                                imgDgv.userDataGridCheckData.dataGridView.Rows.Clear();
                                imgDgv.userGrayData.dataGridView.Rows.Clear();
                            }

                            if (imgDgv.userDataGridCheckData.dataGridView.Rows.Count == 0)
                                if (_dateTimeList.ContainsKey(control.Name))
                                    _dateTimeList[control.Name] = DateTime.Now;

                            imgDgv.SetOff();
                            imgDgv.userDataGridCheckData.label.BackColor = Color.Azure;
                            imgDgv.userDataGridCheckData.label.Text = control.Name;

                            var name = v1.ParaName;
                            var type = v1.Type;
                            var range = v1.Range;
                            var checkValue = v.Value;
                            var checkResult = v1.Result.ToLower();

                            if (!string.IsNullOrEmpty(v1.Unit))
                            {
                                checkValue += v1.Type?.ToLower() == "roi" ? string.Empty : @"/" + v1.Unit;
                                range += v1.Type?.ToLower() == "roi" ? string.Empty : @"/" + v1.Unit;
                            }

                            if (type.ToLower() == "vision") // imgae
                            {
                                imgDgv.imageViewer.Roi.Clear();
                                ImageProcessing.DrawContourCountInOverlay(
                                    imgDgv.imageViewer.Image, imgDgv.imageViewer.Roi);

                                var visionRegCount = new Regex(@"(?<=<Vision>)([\,\.\*\#\$\@\^\w]+)(?=</Vision>)+");

                                var visionCountMatches = visionRegCount.Matches(checkValue);

                                var visionData = new List<string>();

                                if (visionCountMatches.Count > 0)
                                    visionData.AddRange(from object m in visionCountMatches select m.ToString());

                                var isVisionCheckNg = false;

                                if (!visionData.Any())
                                    isVisionCheckNg = true;
                                else
                                {
                                    foreach (var c in visionData)
                                    {
                                        var sp = c.Split(',');

                                        var counterType = sp[1];
                                        string rect;

                                        var visionCheckName = string.Empty;
                                        var visionCheckResult = string.Empty;
                                        var visionCheckValue = string.Empty;
                                        var visionCheckRange = string.Empty;

                                        if (counterType.Equals("PolygonContour"))
                                        {
                                            rect =
                                                string.Format(@"{0},{1},{2},{3},{4},{5},{6},{7}", sp[2], sp[3], sp[4],
                                                    sp[5],
                                                    sp[6], sp[7], sp[8], sp[9]);
                                            imgDgv.imageViewer.Roi.Add(ImageProcessing.GetPolygonContourByString(rect));

                                            visionCheckName = string.Format("{0}-框{1}-组{2}", v1.ParaName,
                                                imgDgv.imageViewer.Roi.Count, int.Parse(sp[0]));
                                            visionCheckRange = string.Format("{0}~{1}", double.Parse(sp[10]),
                                                double.Parse(sp[11]));
                                            visionCheckValue =
                                                double.Parse(sp[12]).ToString(CultureInfo.InvariantCulture);

                                            if (double.Parse(sp[12]) < double.Parse(sp[10]) ||
                                                double.Parse(sp[12]) > double.Parse(sp[11]))
                                            {
                                                visionCheckResult = false.ToString().ToLower();
                                                isVisionCheckNg = true;
                                            }
                                            else
                                                visionCheckResult = true.ToString().ToLower();
                                        }
                                        else if (counterType.Equals("RectangleContour"))
                                        {
                                            rect = string.Format(@"{0},{1},{2},{3}", sp[2], sp[3], sp[4], sp[5]);
                                            imgDgv.imageViewer.Roi.Add(ImageProcessing.GetRectByString(rect));

                                            visionCheckName = string.Format("{0}-框{1}-组{2}", v1.ParaName,
                                                imgDgv.imageViewer.Roi.Count, int.Parse(sp[0]));
                                            visionCheckRange = string.Format("{0}~{1}", double.Parse(sp[6]),
                                                double.Parse(sp[7]));
                                            visionCheckValue = double.Parse(sp[8])
                                                .ToString(CultureInfo.InvariantCulture);

                                            if (double.Parse(sp[8]) < double.Parse(sp[6]) ||
                                                double.Parse(sp[8]) > double.Parse(sp[7]))
                                            {
                                                visionCheckResult = false.ToString().ToLower();
                                                isVisionCheckNg = true;
                                            }
                                            else
                                                visionCheckResult = true.ToString().ToLower();
                                        }

                                        var isHaveGray = -1;
                                        for (var i = 0; i < imgDgv.userGrayData.dataGridView.RowCount; i++)
                                        {
                                            if (imgDgv.userGrayData.dataGridView.Rows[i].Cells[0].Value.ToString() !=
                                                visionCheckName)
                                                continue;

                                            isHaveGray = i;
                                            break;
                                        }

                                        if (isHaveGray > -1)
                                        {
                                            imgDgv.userGrayData.dataGridView.Rows[isHaveGray].Cells[1].Value =
                                                visionCheckRange;
                                            imgDgv.userGrayData.dataGridView.Rows[isHaveGray].Cells[2].Value =
                                                visionCheckValue;
                                            imgDgv.userGrayData.dataGridView.Rows[isHaveGray].DefaultCellStyle.BackColor
                                                =
                                                visionCheckResult == "false" || checkResult == "0"
                                                    ? Color.Red
                                                    : Color.White;
                                        }
                                        else
                                        {
                                            var index = imgDgv.userGrayData.dataGridView.Rows.Add();
                                            imgDgv.userGrayData.dataGridView.Rows[index].Cells[0].Value =
                                                visionCheckName;
                                            imgDgv.userGrayData.dataGridView.Rows[index].Cells[1].Value =
                                                visionCheckRange;
                                            imgDgv.userGrayData.dataGridView.Rows[index].Cells[2].Value =
                                                visionCheckValue;
                                            imgDgv.userGrayData.dataGridView.Rows[index].DefaultCellStyle.BackColor =
                                                visionCheckResult == "false" || checkResult == "0"
                                                    ? Color.Red
                                                    : Color.White;
                                        }

                                        imgDgv.userGrayData.dataGridView.FirstDisplayedScrollingRowIndex =
                                            imgDgv.userGrayData.dataGridView.RowCount - 1;
                                    }
                                }

                                checkResult = isVisionCheckNg ? false.ToString().ToLower() : true.ToString().ToLower();
                                range = @"见右表";
                                checkValue = @"见右表";
                            }

                            var isHave = -1;
                            for (var i = 0; i < imgDgv.userDataGridCheckData.dataGridView.RowCount; i++)
                            {
                                if (imgDgv.userDataGridCheckData.dataGridView.Rows[i].Cells[0].Value.ToString() != name)
                                    continue;

                                isHave = i;
                                break;
                            }

                            if (isHave > -1)
                            {
                                imgDgv.userDataGridCheckData.dataGridView.Rows[isHave].Cells[1].Value = range;
                                imgDgv.userDataGridCheckData.dataGridView.Rows[isHave].Cells[2].Value = checkValue;
                                imgDgv.userDataGridCheckData.dataGridView.Rows[isHave].Cells[3].Value =
                                    checkResult == "false" || checkResult == "0"
                                        ? "NG"
                                        : "OK";
                                imgDgv.userDataGridCheckData.dataGridView.Rows[isHave].DefaultCellStyle.BackColor =
                                    checkResult == "false" || checkResult == "0"
                                        ? Color.Red
                                        : Color.White;
                            }
                            else
                            {
                                var index = imgDgv.userDataGridCheckData.dataGridView.Rows.Add();
                                imgDgv.userDataGridCheckData.dataGridView.Rows[index].Cells[0].Value = name;
                                imgDgv.userDataGridCheckData.dataGridView.Rows[index].Cells[1].Value = range;
                                imgDgv.userDataGridCheckData.dataGridView.Rows[index].Cells[2].Value = checkValue;
                                imgDgv.userDataGridCheckData.dataGridView.Rows[index].Cells[3].Value =
                                    checkResult == "false" || checkResult == "0"
                                        ? "NG"
                                        : "OK";
                                imgDgv.userDataGridCheckData.dataGridView.Rows[index].DefaultCellStyle.BackColor =
                                    checkResult == "false" || checkResult == "0"
                                        ? Color.Red
                                        : Color.White;
                            }

                            imgDgv.userDataGridCheckData.dataGridView.FirstDisplayedScrollingRowIndex =
                                imgDgv.userDataGridCheckData.dataGridView.RowCount - 1;

                            ImageProcessing.DrawContourCountInOverlay(imgDgv.imageViewer.Image, imgDgv.imageViewer.Roi);

                            #endregion
                        }
                        else if (controlUi.Type == "ChartViewer")
                        {
                            #region Chart DataGrid data

                            var chartDgv = (UserChartDataViewer)control;
                            if (chartDgv.userDataGridCheckData.label.BackColor != Color.Azure)
                            {
                                chartDgv.userDataGridCheckData.dataGridView.Rows.Clear();
                                chartDgv.chart1.Series[0].Points.Clear();
                                //chartDgv.BInitMaxMinLine = false;
                                chartDgv.SMax.Points.Clear();
                                chartDgv.SMin.Points.Clear();
                            }

                            if (chartDgv.userDataGridCheckData.dataGridView.Rows.Count == 0)
                                if (_dateTimeList.ContainsKey(control.Name))
                                    _dateTimeList[control.Name] = DateTime.Now;

                            chartDgv.userDataGridCheckData.label.BackColor = Color.Azure;
                            chartDgv.userDataGridCheckData.label.Text = control.Name;

                            var name = v1.ParaName;
                            var type = v1.Type;
                            var range = v1.Range;
                            var checkValue = v.Value;
                            var checkResult = v1.Result.ToLower();

                            if (!string.IsNullOrEmpty(v1.Unit))
                            {
                                checkValue += @"/" + v1.Unit;
                                range += @"/" + v1.Unit;
                            }

                            if (type.ToLower() == "chart")
                            {
                                //if (chartDgv.BInitMaxMinLine == false)
                                {
                                    //chartDgv.BInitMaxMinLine = true;

                                    chartDgv.chart1.Series[0].Points.Clear();
                                    //chartDgv.BInitMaxMinLine = false;
                                    chartDgv.SMax.Points.Clear();
                                    chartDgv.SMin.Points.Clear();

                                    var sp = v1.Value.Split(';');

                                    for (int k = 0; k < sp.Length; k++)
                                    {
                                        var s = sp[k];

                                        var tt = s.TrimStart('[').TrimEnd(']').Split(',');

                                        var x = float.Parse(tt[0]);
                                        var yMin = float.Parse(tt[1]);
                                        var yMax = float.Parse(tt[2]);
                                        var y = float.Parse(tt[3]);

                                        chartDgv.SMax.Points.AddXY(x, yMax);
                                        chartDgv.SMin.Points.AddXY(x, yMin);

                                        range = yMin + "~" + yMax;
                                        checkValue = y.ToString(CultureInfo.InvariantCulture);

                                        var dgv = chartDgv.userDataGridCheckData;
                                        var isHave = -1;
                                        for (var i = 0; i < dgv.dataGridView.RowCount; i++)
                                        {
                                            if (dgv.dataGridView.Rows[i].Cells[0].Value.ToString() != name + "P" + x)
                                                continue;

                                            isHave = i;
                                            break;
                                        }

                                        if (!string.IsNullOrEmpty(v1.Unit))
                                        {
                                            checkValue += @"/" + v1.Unit;
                                            range += @"/" + v1.Unit;
                                        }

                                        if (y >= yMin && y <= yMax)
                                        {
                                            checkResult = "true";
                                        }
                                        else
                                        {
                                            checkResult = "false";
                                        }

                                        if (isHave > -1)
                                        {
                                            dgv.dataGridView.Rows[isHave].Cells[1].Value = range;
                                            dgv.dataGridView.Rows[isHave].Cells[2].Value = checkValue;
                                            dgv.dataGridView.Rows[isHave].Cells[3].Value =
                                                checkResult == "false" || checkResult == "0"
                                                    ? "NG"
                                                    : "OK";
                                            dgv.dataGridView.Rows[isHave].DefaultCellStyle.BackColor =
                                                checkResult == "false" || checkResult == "0"
                                                    ? Color.Red
                                                    : Color.White;

                                            var line = chartDgv.chart1.Series[0];
                                            line.Name = controlUi.Name;
                                            var point = new { x, y };
                                            line.Points.AddXY(point.x, point.y);
                                        }
                                        else
                                        {
                                            var index = dgv.dataGridView.Rows.Add();
                                            dgv.dataGridView.Rows[index].Cells[0].Value = name + "P" + x;
                                            dgv.dataGridView.Rows[index].Cells[1].Value = range;
                                            dgv.dataGridView.Rows[index].Cells[2].Value = checkValue;
                                            dgv.dataGridView.Rows[index].Cells[3].Value =
                                                checkResult == "false" || checkResult == "0"
                                                    ? "NG"
                                                    : "OK";
                                            dgv.dataGridView.Rows[index].DefaultCellStyle.BackColor =
                                                checkResult == "false" || checkResult == "0"
                                                    ? Color.Red
                                                    : Color.White;

                                            var line = chartDgv.chart1.Series[0];
                                            line.Name = controlUi.Name;
                                            var point = new { x, y };
                                            line.Points.AddXY(point.x, point.y);
                                        }

                                        dgv.dataGridView.FirstDisplayedScrollingRowIndex =
                                            dgv.dataGridView.RowCount - 1;
                                    }
                                }
                            }
                            else
                            {
                                var dgv = chartDgv.userDataGridCheckData;
                                var isHave = -1;
                                for (var i = 0; i < dgv.dataGridView.RowCount; i++)
                                {
                                    if (dgv.dataGridView.Rows[i].Cells[0].Value.ToString() != name)
                                        continue;

                                    isHave = i;
                                    break;
                                }

                                if (isHave > -1)
                                {
                                    dgv.dataGridView.Rows[isHave].Cells[1].Value = range;
                                    dgv.dataGridView.Rows[isHave].Cells[2].Value = checkValue;
                                    dgv.dataGridView.Rows[isHave].Cells[3].Value =
                                        checkResult == "false" || checkResult == "0"
                                            ? "NG"
                                            : "OK";
                                    dgv.dataGridView.Rows[isHave].DefaultCellStyle.BackColor =
                                        checkResult == "false" || checkResult == "0"
                                            ? Color.Red
                                            : Color.White;
                                }
                                else
                                {
                                    var index = dgv.dataGridView.Rows.Add();
                                    dgv.dataGridView.Rows[index].Cells[0].Value = name;
                                    dgv.dataGridView.Rows[index].Cells[1].Value = range;
                                    dgv.dataGridView.Rows[index].Cells[2].Value = checkValue;
                                    dgv.dataGridView.Rows[index].Cells[3].Value =
                                        checkResult == "false" || checkResult == "0"
                                            ? "NG"
                                            : "OK";
                                    dgv.dataGridView.Rows[index].DefaultCellStyle.BackColor =
                                        checkResult == "false" || checkResult == "0"
                                            ? Color.Red
                                            : Color.White;
                                }

                                dgv.dataGridView.FirstDisplayedScrollingRowIndex = dgv.dataGridView.RowCount - 1;
                            }

                            #endregion
                        }
                        else if (controlUi.Type == "PictureBox")
                        {
                            var pictureBox = (PictureBox)control;

                            try
                            {
                                if (pictureBox.Image != null)
                                {
                                    pictureBox.Image.Dispose();
                                    pictureBox.Image = null;
                                }

                                var checktype = v.Type;
                                var checkValue = v.Value;
                                if (!string.IsNullOrEmpty(checkValue) && checktype.ToLower() == "bitmap")
                                {
                                    // 从Base64字符串转换回字节数组
                                    var convertedBytes = Convert.FromBase64String(checkValue);

                                    // 从字节数组转换回Bitmap
                                    using (var ms = new MemoryStream(convertedBytes))
                                    {
                                        var convertedBitmap = new Bitmap(ms);
                                        // 保存或使用转换后的Bitmap
                                        pictureBox.Image = convertedBitmap;
                                        pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                pictureBox.Image = null;
                            }
                        }
                        else if (controlUi.Type == "VisionTab")
                        {
                            lock (_lockChangeVisionTabIndex)
                            {
                                var tabCtrl = (UITabControl)control;
                                if (tabCtrl != null)
                                {
                                    try
                                    {
                                        var pageIndex = tabCtrl.TabPages.IndexOfKey(v1.ProcessNo);
                                        if (pageIndex == -1)
                                        {
                                            tabCtrl.TabPages.Add(v1.ProcessNo, v1.ProcessNo.Substring(v1.ProcessNo.Length - 3, 3));
                                            pageIndex = tabCtrl.TabPages.IndexOfKey(v1.ProcessNo);

                                            var tb = new TableLayoutPanel { Dock = DockStyle.Fill };
                                            for (var i = 0; i < 3; i++)
                                                tb.ColumnStyles.Add(
                                                    new ColumnStyle(SizeType.Percent, 3));
                                            tb.RowCount = 1;

                                            var newFont = new Font("微软雅黑", 8);
                                            var witdth = Screen.GetWorkingArea(this).Width;
                                            var height = Screen.GetWorkingArea(this).Height;
                                            var dg = new UserDataGrid
                                            {
                                                Name = control.Name,
                                                Dock = DockStyle.Fill,
                                                label = { Text = control.Name },
                                                dataGridView = { Font = newFont, },
                                            };
                                            dg.Name = "DataView";

                                            if (witdth <= 1024 || height <= 768)
                                                newFont = new Font("黑体", 8F, FontStyle.Bold, GraphicsUnit.Point, 134);
                                            if (witdth > 1024 && height > 768)
                                                dg.dataGridView.RowsDefaultCellStyle.Font = new Font("黑体", 8F, FontStyle.Regular,
                                                    GraphicsUnit.Point, 120);
                                            else
                                            {
                                                dg.dataGridView.RowsDefaultCellStyle.Font = new Font("黑体", 8F, FontStyle.Regular, GraphicsUnit.Point, 100);
                                                dg.label.Font = new Font("黑体", 10, FontStyle.Regular);
                                            }

                                            dg.dataGridView.Columns.Add(
                                                new DataGridViewTextBoxColumn { Name = "名称" });
                                            dg.dataGridView.Columns.Add(
                                                new DataGridViewTextBoxColumn { Name = "范围" });
                                            dg.dataGridView.Columns.Add(
                                                new DataGridViewTextBoxColumn { Name = "实际值" });
                                            dg.dataGridView.Columns.Add(
                                                new DataGridViewTextBoxColumn { Name = "结果" });

                                            dg.dataGridView.Columns[3].Width = 20;

                                            dg.dataGridView.ReadOnly = true;
                                            dg.dataGridView.RowHeadersVisible = false;
                                            dg.dataGridView.AllowUserToAddRows = false;

                                            tb.Controls.Add(dg, 0, 0);

                                            var visionTab = new UITabControl { Dock = DockStyle.Fill, Name = "visionTab" };
                                            tb.Controls.Add(visionTab, 1, 0);
                                            tb.SetColumnSpan(visionTab, 2);

                                            tabCtrl.TabPages[pageIndex].Controls.Add(tb);
                                        }

                                        {
                                            var dgv = (UserDataGrid)tabCtrl.TabPages[pageIndex].Controls.Find("DataView", true)[0];
                                            var visionTab = (UITabControl)tabCtrl.TabPages[pageIndex].Controls.Find("visionTab", true)[0];
                                            if (dgv.label.BackColor != Color.Azure)
                                            {
                                                dgv.dataGridView.Rows.Clear();

                                                for (var i = 0; i < visionTab.TabPages.Count; i++)
                                                {
                                                    for (var j = 0; j < visionTab.TabPages[i].Controls.Count; j++)
                                                    {
                                                        if (visionTab.TabPages[i].Controls[j] is UserCvDisplay)
                                                        {
                                                            ((UserCvDisplay)visionTab.TabPages[i].Controls[j]).ReleaseResult();
                                                            visionTab.SelectedIndex = visionTab.TabPages.Count > 0 ? 0 : -1;
                                                        }
                                                    }
                                                }
                                            }

                                            if (dgv.dataGridView.Rows.Count == 0)
                                            {
                                                if (_dateTimeList.ContainsKey(control.Name))
                                                {
                                                    _dateTimeList[control.Name] = DateTime.Now;
                                                }
                                            }

                                            dgv.label.BackColor = Color.Azure;
                                            dgv.label.Text = control.Name;

                                            var name = v1.ParaName;
                                            var range = v1.Range;
                                            var checkValue = string.Empty;
                                            if (v1.Type?.ToLower() == "静态图像".ToLower())
                                            {
                                                checkValue = v1.Result.ToLower() == "false" || v.Result.ToLower() == "0"
                                                            ? "NG"
                                                            : "OK";
                                            }
                                            else
                                            {
                                                checkValue = v1.Value;
                                            }
                                            var checkResult = v1.Result.ToLower();

                                            if (v1.Type?.ToLower() == "roi" || v1.Type?.ToLower() == "静态图像".ToLower())
                                            {
                                                var ppName = v1.Type?.ToLower() == "roi" ? v1.Unit : v1.ParaName;
                                                var visionPageIndex = visionTab.TabPages.IndexOfKey(ppName);
                                                if (visionPageIndex == -1)
                                                {
                                                    visionTab.TabPages.Add(ppName, ppName);
                                                    visionPageIndex = visionTab.TabPages.IndexOfKey(ppName);
                                                    visionTab.TabPages[visionPageIndex].Controls.Add(new UserCvDisplay(ppName) { Dock = DockStyle.Fill, Name = "ImageView" });
                                                }

                                                if (visionPageIndex > visionTab.SelectedIndex)
                                                    visionTab.SelectedIndex = visionPageIndex;

                                                var cv = (UserCvDisplay)visionTab.TabPages[visionPageIndex].Controls.Find("ImageView", true)[0];
                                                if (v1.Type?.ToLower() == "roi")
                                                {
                                                    cv.AppendRow(v1.ParaName, v1.Range, v1.Value, v1.Result);
                                                    continue;
                                                }
                                                else
                                                {
                                                    cv.AppendImage(v1.Value);
                                                }
                                            }

                                            {
                                                if (!string.IsNullOrEmpty(v1.Unit))
                                                {
                                                    checkValue += v1.Type?.ToLower() == "roi" ? string.Empty : @"/" + v1.Unit;
                                                    range += v1.Type?.ToLower() == "roi" ? string.Empty : @"/" + v1.Unit;
                                                }

                                                var isHave = -1;
                                                for (var i = 0; i < dgv.dataGridView.RowCount; i++)
                                                {
                                                    if (dgv.dataGridView.Rows[i].Cells[0].Value.ToString() != name)
                                                        continue;

                                                    isHave = i;
                                                    break;
                                                }

                                                if (isHave > -1)
                                                {
                                                    dgv.dataGridView.Rows[isHave].Cells[1].Value = range;
                                                    dgv.dataGridView.Rows[isHave].Cells[2].Value = checkValue;
                                                    dgv.dataGridView.Rows[isHave].Cells[3].Value =
                                                        checkResult == "false" || checkResult == "0"
                                                            ? "NG"
                                                            : "OK";
                                                    dgv.dataGridView.Rows[isHave].DefaultCellStyle.BackColor =
                                                        checkResult == "false" || checkResult == "0"
                                                            ? Color.Red
                                                            : Color.White;
                                                }
                                                else
                                                {
                                                    var index = dgv.dataGridView.Rows.Add();
                                                    dgv.dataGridView.Rows[index].Cells[0].Value = name;
                                                    dgv.dataGridView.Rows[index].Cells[1].Value = range;
                                                    dgv.dataGridView.Rows[index].Cells[2].Value = checkValue;
                                                    dgv.dataGridView.Rows[index].Cells[3].Value =
                                                        checkResult == "false" || checkResult == "0"
                                                            ? "NG"
                                                            : "OK";
                                                    dgv.dataGridView.Rows[index].DefaultCellStyle.BackColor =
                                                        checkResult == "false" || checkResult == "0"
                                                            ? Color.Red
                                                            : Color.White;
                                                }

                                                dgv.dataGridView.FirstDisplayedScrollingRowIndex = dgv.dataGridView.RowCount - 1;
                                            }
                                        }
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                            }
                        }
                    }
                }));
            }
        }

        private string _thisMac = string.Empty;
        private readonly object _lockChangeVisionTabIndex = new object();

        private void St_PushEndResult(string processNo, List<CheckData> checkValues)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    // var para = _systemconfig.processparas.paras.ToList().Find(t => t.Name == lstdata[0]);  //para.processNo
                    // 参数表中的工序编号需要改为工作站名称
                    // 根据字符中的第一个部分查找参数

                    var tempCheckValues =
                        checkValues.FindAll(f => f.ParaName != "样件检测结果" && f.ParaName != "检测结果OK" && f.ParaName != "检测结果NG");
                    var isNg =
                        tempCheckValues.FindAll(
                            f => string.Equals(f.Result, false.ToString(), StringComparison.CurrentCultureIgnoreCase)).Any();

                    var taskId = Guid.NewGuid().ToString();
                    int totalCount = 0;
                    int okCount = 0;
                    _thisMac = string.IsNullOrEmpty(_thisMac) ? MacAddressHelper.GetMacByIpConfig().Replace("-", string.Empty) : _thisMac;

                    var listBarcodeStrs = new List<string>();

                    try
                    {
                        var process = processNo.Substring(0, processNo.Length - 3);

                        var modelList = new List<Model.manufactureCheckData>();
                        var bll = new BLL.manufactureCheckData();

                        var barcodeList = tempCheckValues.FindAll(f => f.Type != null && f.Type.ToLower().Equals("barcodegroup"));
                        if (tempCheckValues.FindAll(f => f.Type != null && f.Type.ToLower().Equals("barcode")).Any())
                            barcodeList.AddRange(tempCheckValues.FindAll(f => f.Type.ToLower().Equals("barcode")));

                        if (barcodeList.Count > 0)
                        {
                            foreach (var t1 in barcodeList)
                            {
                                listBarcodeStrs.Add(t1.Value);

                                var model = new Model.manufactureCheckData
                                {
                                    taskNo = taskId,
                                    productBarcode = t1.Value,
                                    productUid = t1.Value,
                                    checkStaffNo = "admin",
                                    productNo = process,
                                    processNo = string.Format("{0}_{1}", process, processNo.Substring(processNo.Length - 3, 3)),
                                    checkResult = isNg ? "0002" : "0001",
                                    checkDate = DateTime.Now,
                                    createTime = DateTime.Now,
                                    creater = string.Format("{0}_{1}_Mac:{2}", St.DeviceConfig.DeviceInfo.DeviceName, processNo.Substring(processNo.Length - 3, 3), _thisMac)
                                };

                                var listCheckNameAndData =
                                    tempCheckValues.FindAll(
                                        f =>
                                            f.Type.ToLower() != "barcode" && f.Type.ToLower() != "barcodegroup" &&
                                            f.Type.ToLower() != "vision" && f.Type.ToLower() != "静态图像".ToLower() && f.Type.ToLower() != "bitmap".ToLower())
                                        .Select(t => new CheckNameAndData { Name = t.ParaName, Data = t.Value })
                                        .ToList();

                                model.checkData = JsonConvert.SerializeObject(listCheckNameAndData);

                                //if (model.checkData.Length > 4000)
                                //    model.checkData = model.checkData.Substring(0, 4000);
                                modelList.Add(model);

                                //bll.Add(model);
                                if (!processNo.EndsWith("000"))
                                {
                                    SaveDataLocal(model, !isNg, checkValues.FindAll(f => f.ParaName == "样件检测结果").Any(), St.DeviceConfig.DeviceInfo.DeviceName, string.Format("{0}_{1}", process, processNo.Substring(processNo.Length - 3, 3)));
                                }
                            }
                        }
                        else
                        {
                            var model = new Model.manufactureCheckData
                            {
                                taskNo = taskId,
                                productBarcode = string.Empty,
                                productUid = string.Empty,
                                checkStaffNo = "admin",
                                productNo = process,
                                processNo = string.Format("{0}_{1}", process, processNo.Substring(processNo.Length - 3, 3)),
                                checkResult = isNg ? "0002" : "0001",
                                checkDate = DateTime.Now,
                                createTime = DateTime.Now,
                                creater = string.Format("{0}_{1}_Mac:{2}", St.DeviceConfig.DeviceInfo.DeviceName,
                                    processNo.Substring(processNo.Length - 3, 3),
                                    _thisMac)
                            };

                            var listCheckNameAndData =
                                tempCheckValues.FindAll(
                                    f =>
                                        f.Type.ToLower() != "barcode" && f.Type.ToLower() != "barcodegroup" &&
                                        f.Type.ToLower() != "vision" && f.Type.ToLower() != "静态图像".ToLower())
                                    .Select(t => new CheckNameAndData { Name = t.ParaName, Data = t.Value })
                                    .ToList();

                            model.checkData = JsonConvert.SerializeObject(listCheckNameAndData);
                            //if (model.checkData.Length > 4000)
                            //    model.checkData = model.checkData.Substring(0, 4000);
                            modelList.Add(model);

                            //bll.Add(model);
                            if (!processNo.EndsWith("000"))
                                SaveDataLocal(model, !isNg, checkValues.FindAll(f => f.ParaName == "样件检测结果").Any(), St.DeviceConfig.DeviceInfo.DeviceName, string.Format("{0}_{1}", process, processNo.Substring(processNo.Length - 3, 3)));
                        }

                        var listCheckDetail = new List<SyProductionSaveCheckData.CheckDataDetail>();
                        var materialGroupBarcode = string.Empty;
                        foreach (var t in tempCheckValues)
                        {
                            if (t.Type.ToLower() != "barcode" && t.Type.ToLower() != "barcodegroup")
                            {
                                if (t.Type.ToLower() == "vision")
                                {
                                    listCheckDetail.Add(new SyProductionSaveCheckData.CheckDataDetail
                                    {
                                        Type = t.Type,
                                        ParaName = t.ParaName,
                                        Range = "/",
                                        Result = t.Result,
                                        Value = t.Result
                                    });
                                }
                                else if (t.Type.ToLower() == "platebarcode".ToLower())
                                {
                                    //listCheckDetail.Add(new SyProductionSaveCheckData.CheckDataDetail
                                    //{
                                    //    Type = t.Type,
                                    //    ParaName = t.ParaName,
                                    //    Range = t.Range,
                                    //    Result = t.Result,
                                    //    Value = t.Value
                                    //});
                                    materialGroupBarcode = t.Value;
                                }
                                else if (t.Type.ToLower() == "静态图像".ToLower())
                                {
                                    listCheckDetail.Add(new SyProductionSaveCheckData.CheckDataDetail
                                    {
                                        Type = t.Type,
                                        ParaName = t.ParaName,
                                        Range = "OK",
                                        Result = t.Result,
                                        Value = t.Result.ToLower() == "false" || t.Result.ToLower() == "0" ? "NG" : "OK"
                                    });
                                }
                                else if (t.Type.ToLower() == "bitmap".ToLower())
                                {
                                    listCheckDetail.Add(new SyProductionSaveCheckData.CheckDataDetail
                                    {
                                        Type = t.Type,
                                        ParaName = t.ParaName,
                                        Range = "/",
                                        Result = t.Result,
                                        Value = t.Result.ToLower() == "false" || t.Result.ToLower() == "0" ? "NG" : "OK"
                                    });
                                }
                                else
                                {
                                    listCheckDetail.Add(new SyProductionSaveCheckData.CheckDataDetail
                                    {
                                        Type = t.Type,
                                        ParaName = t.ParaName,
                                        Range = t.Range.TrimStart('~'),
                                        Result = t.Result,
                                        Value = t.Value
                                    });
                                }
                            }
                        }

                        string syCreator;
                        try
                        {
                            syCreator = string.Format("{0}_{1}_Mac:{2}", St.DeviceConfig.DeviceInfo.DeviceName,
                                processNo.Substring(processNo.Length - 3, 3),
                                _thisMac);
                        }
                        catch (Exception)
                        {
                            syCreator = string.Empty;
                        }

                        if (modelList.Any())
                        {
                            var model = modelList[0];
                            GetCount(St.DeviceConfig.DeviceInfo.DeviceName, string.Format("{0}_{1}", process, processNo.Substring(processNo.Length - 3, 3)), !isNg, out totalCount, out okCount);
                        }

                        if (!processNo.EndsWith("000"))
                        {
                            SyProductionSaveCheckData.SaveData(
                                !isNg,
                                checkValues.FindAll(f => f.ParaName == "样件检测结果").Any(),
                                Program.DeviceNo, St.DeviceConfig.DeviceInfo.DeviceName,
                                processNo.Substring(0, processNo.Length - 3),
                                listBarcodeStrs, listCheckDetail, syCreator, totalCount, materialGroupBarcode);
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }

                    var updatedControlUis = new List<string>();

                    foreach (var checkValue in tempCheckValues)
                    {
                        // 查找参数表对应的控件
                        var para =
                            St.DeviceConfig.Paras.ToList()
                                .Find(t => t.ProcessNo == checkValue.ProcessNo && t.Name == checkValue.ParaName && checkValue.Type?.ToLower() != "roi");

                        if (para == null)
                        {
                            if (checkValue.Type?.ToLower() == "roi")
                            {
                                para = St.DeviceConfig.Paras.ToList().Find(t => t.ProcessNo == checkValue.ProcessNo && t.Name.ToLower() == checkValue.Unit.ToLower() && t.OkFormat.ToLower() == checkValue.Format.ToLower());
                                if (para == null)
                                    continue;
                            }
                            else if (checkValue.Type?.ToLower() == "静态图像".ToLower())
                            {
                                para = St.DeviceConfig.Paras.ToList().Find(t => t.ProcessNo == checkValue.ProcessNo && t.Name.ToLower() == checkValue.ParaName.ToLower() && t.DataType.ToLower() == "roi");
                                if (para == null)
                                    continue;
                            }
                            else
                            {
                                continue;
                            }
                        }

                        // 查找参数表对应的控件
                        var control = _lstControls.Find(t => t.Name == para.ControlName);
                        if (control == null)
                        {
                            if (
                                St.DeviceConfig.Controls.ToList()
                                    .Find(f => f.Name == para.ControlName && f.Type == "ListViewItem") != null)
                            {
                                var controlUi1 = St.DeviceConfig.Controls.ToList().Find(t => t.Name == para.ControlName);

                                if (controlUi1.Type == "ListViewItem")
                                {
                                    #region ListViewItem

                                    var findListView =
                                                St.DeviceConfig.Controls.ToList()
                                                    .Find(
                                                        f =>
                                                            f.Type == "ListView" &&
                                                            f.ColumnPosition == controlUi1.ColumnPosition &&
                                                            f.ColumnSpan == controlUi1.ColumnSpan &&
                                                            f.RowPosition == controlUi1.RowPosition &&
                                                            f.RowSpan == controlUi1.RowSpan);

                                    if (findListView != null)
                                    {
                                        var listView = _lstControls.Find(t => t.Name == findListView.Name);
                                        var listView1 = listView as UserDataGrid;
                                        if (listView1 != null)
                                        {
                                            var cellIndex = -1;
                                            for (var i = 0; i < listView1.dataGridView.ColumnCount; i++)
                                            {
                                                if (listView1.dataGridView.Columns[i].HeaderText == controlUi1.Name)
                                                {
                                                    cellIndex = i;
                                                    break;
                                                }
                                            }

                                            if (cellIndex != -1)
                                            {
                                                listView1.dataGridView.Rows[0].Cells[cellIndex].Value = string.Format("[{0}] {1}",
                                                           DateTime.Now.ToShortTimeString(), isNg ? "NG" : "OK");
                                                listView1.dataGridView.Rows[0].Cells[cellIndex].Style.BackColor = isNg
                                                    ? Color.Red
                                                    : Color.Green;
                                                if (checkValues.FindAll(f => f.ParaName == "样件检测结果").Any())
                                                    break;
                                                listView1.dataGridView.Rows[1].Cells[cellIndex].Value =
                                                    string.Format("[OK={0},NG={1}/总: {2}]", okCount, totalCount - okCount, totalCount);
                                            }
                                        }
                                    }

                                    #endregion
                                }
                            }
                            continue;
                        }

                        // 查找实际界面的控件
                        var controlUi = St.DeviceConfig.Controls.ToList().Find(t => t.Name == control.Name);
                        if (controlUi == null)
                            return;

                        if (updatedControlUis.Contains(controlUi.Name))
                            continue;
                        updatedControlUis.Add(controlUi.Name);

                        var costTime =
                            Math.Round(ValueHelper.GetTimeSpanMs(_dateTimeList[control.Name], DateTime.Now) / (float)1000, 1,
                                MidpointRounding.AwayFromZero);

                        if (controlUi.Type == "DataGrid")
                        {
                            ((UserDataGrid)control).label.Text =
                                string.Format("{0}-[{2}] {3}({1}秒)", controlUi.Name, costTime, DateTime.Now.ToShortTimeString(),
                                  isNg ? "NG" : "OK");
                            ((UserDataGrid)control).label.BackColor = isNg ? Color.Red : Color.Green;

                            if (checkValues.FindAll(f => f.ParaName == "样件检测结果").Any())
                                continue;
                            var tempResult = ((UserDataGrid)control).label.Text;
                            ((UserDataGrid)control).label.Text = string.Format("{0} [OK={1},NG={2}/总: {3}]", tempResult, okCount, totalCount - okCount, totalCount);
                        }
                        else if (controlUi.Type == "ChartViewer")
                        {
                            var chartDgv = (UserChartDataViewer)control;
                            var line = chartDgv.chart1.Series[0];

                            var intervalX = (double)0;
                            var intervalY = (double)0;

                            for (var i = 0; i < line.Points.Count; i++)
                            {
                                if (i > 0)
                                {
                                    intervalX = intervalX +
                                                Math.Abs(line.Points[i].XValue - line.Points[i - 1].XValue);
                                    intervalY = intervalY +
                                                Math.Abs(line.Points[i].YValues[0] - line.Points[i - 1].YValues[0]);
                                }
                            }

                            chartDgv.chart1.ChartAreas[0].AxisX.Interval = Math.Round(intervalX / line.Points.Count, 2,
                                MidpointRounding.AwayFromZero);
                            chartDgv.chart1.ChartAreas[0].AxisX.IntervalOffset = Math.Round(intervalX / line.Points.Count, 2,
                                MidpointRounding.AwayFromZero);
                            chartDgv.chart1.ChartAreas[0].AxisX.LabelStyle.IsStaggered = true;

                            chartDgv.chart1.ChartAreas[0].AxisY.Interval = Math.Round(intervalY / line.Points.Count, 2,
                                MidpointRounding.AwayFromZero);
                            chartDgv.chart1.ChartAreas[0].AxisY.IntervalOffset = Math.Round(intervalY / line.Points.Count, 2,
                                MidpointRounding.AwayFromZero);
                            chartDgv.chart1.ChartAreas[0].AxisY.LabelStyle.IsStaggered = true;

                            ((UserChartDataViewer)control).userDataGridCheckData.label.Text =
                               string.Format("{0}-[{2}] {3}({1}秒)", controlUi.Name, costTime, DateTime.Now.ToShortTimeString(),
                                 isNg ? "NG" : "OK");
                            ((UserChartDataViewer)control).userDataGridCheckData.label.BackColor = isNg ? Color.Red : Color.Green;

                            if (checkValues.FindAll(f => f.ParaName == "样件检测结果").Any())
                                continue;
                            var tempResult = ((UserChartDataViewer)control).userDataGridCheckData.label.Text;
                            ((UserChartDataViewer)control).userDataGridCheckData.label.Text = string.Format(@"{0} [OK={1},NG={2}/总: {3}]", tempResult, okCount, totalCount - okCount, totalCount);
                        }
                        else if (controlUi.Type == "NiImageViewer")
                        {
                            var imgDgv = (UserImageDataViewer)control;

                            imgDgv.userDataGridCheckData.label.Text = string.Format(@"{0}-[{2}] {3}({1}秒)", controlUi.Name,
                                costTime,
                                DateTime.Now.ToShortTimeString(), isNg ? "NG" : "OK");
                            imgDgv.userDataGridCheckData.label.BackColor = isNg ? Color.Red : Color.Green;

                            if (isNg)
                                imgDgv.SetRed();
                            else
                                imgDgv.SetGreen();

                            if (checkValues.FindAll(f => f.ParaName == "样件检测结果").Any())
                                continue;
                            var tempResult = imgDgv.userDataGridCheckData.label.Text;
                            imgDgv.userDataGridCheckData.label.Text = string.Format(@"{0} [OK={1},NG={2}/总: {3}]", tempResult, okCount, totalCount - okCount, totalCount);
                        }
                        else if (controlUi.Type == "VisionTab")
                        {
                            lock (_lockChangeVisionTabIndex)
                            {
                                var tabCtrl = (UITabControl)control;
                                if (tabCtrl != null)
                                {
                                    var pageIndex = tabCtrl.TabPages.IndexOfKey(processNo);
                                    if (pageIndex != -1)
                                    {
                                        tabCtrl.SelectedIndex = pageIndex;

                                        var dgv = tabCtrl.TabPages[pageIndex].Controls[0].Controls.Find("DataView", false)[0];

                                        ((UserDataGrid)dgv).label.Text =
                                            string.Format("{0}-[{2}] {3}({1}秒)", controlUi.Name, costTime, DateTime.Now.ToShortTimeString(),
                                              isNg ? "NG" : "OK");
                                        ((UserDataGrid)dgv).label.BackColor = isNg ? Color.Red : Color.FromArgb(0, 128, 0);//Color.FromArgb(110, 190, 40);

                                        if (checkValues.FindAll(f => f.ParaName == "样件检测结果").Any())
                                            continue;
                                        var tempResult = ((UserDataGrid)dgv).label.Text;
                                        ((UserDataGrid)dgv).label.Text = string.Format("{0} [OK={1},NG={2}/总: {3}]", tempResult, okCount, totalCount - okCount, totalCount);
                                    }
                                }
                            }
                        }
                    }
                }));
            }
        }

        private void SaveDataLocal(Model.manufactureCheckData model, bool isOk, bool isSampleTest, string title, string controlName)
        {
            //var st = new Stopwatch();
            //st.Start();

            var taskid = model.taskNo;
            var productNo = model.productNo;
            var uid = model.productUid;
            var checkData = model.checkData;

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
                checkStaffNo = "admin",
                checkResult = isSampleTest ? (isOk ? "9001" : "9002") : (isOk ? "0001" : "0002"),
                creater = Program.DeviceNo + ":" + title + "_" + controlName,
                createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                checkDataText = string.Empty
            };

            LocalDbHelper.InsertData(new LocalDbHelper.manufactureCheckData[] { saveLocalData });

            //if (LocalDbHelper.LocalSqlite != null)
            //{
            //    try
            //    {
            //        var insert =
            //            string.Format(
            //                "insert into manufactureCheckData (taskNo,productNo,productUid,pcbaNo,pcbaBarcode,productBarcode,packageBarcode,processNo,checkData,checkDate,checkStaffNo,checkResult,creater,createTime,checkDataText) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}')",
            //                taskid, productNo, uid, string.Empty, uid, uid, string.Empty, productNo + "_001", JsonConvert.SerializeObject(checkData),
            //                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "admin", isSampleTest ? (isOk ? "9001" : "9002") : (isOk ? "0001" : "0002"), Program.DeviceNo + ":" + title + "_" + controlName,
            //                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), JsonConvert.SerializeObject(checkData));

            //        LocalDbHelper.LocalSqlite.Query(insert);
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e);
            //    }
            //}

            //st.Stop();
            //Console.WriteLine("SaveDataLocal耗时：" + st.ElapsedMilliseconds);
        }

        private Dictionary<string, int> _dicTotals = new Dictionary<string, int>();
        private Dictionary<string, int> _dicOks = new Dictionary<string, int>();

        private void GetCount(string title, string controlName, bool isOk, out int totalCount, out int okCount)
        {
            totalCount = 0;
            okCount = 0;

            var keyName = Program.DeviceNo + ":" + title + "_" + controlName;

            {
                if (_dicTotals.ContainsKey(keyName))
                {
                    _dicTotals[keyName]++;
                    totalCount = _dicTotals[keyName];
                }
                else
                {
                    //if (LocalDbHelper.LocalSqlite != null)
                    //{
                    try
                    {
                        var sql =
                            string.Format(
                                "select taskNo from manufactureCheckData where creater = '{0}' and (checkResult = '0001' or checkResult = '0002') and createTime between datetime('now','start of day','+1 seconds') and  datetime('now','start of day','+1 days','-1 seconds') group by taskNo",
                               Program.DeviceNo + ":" + title + "_" + controlName);

                        totalCount = LocalDbHelper.QueryBySqlString(sql).Count;
                        _dicTotals[keyName] = totalCount;

                        //if (LocalDbHelper.LocalSqlite != null)
                        //{
                        //    var getData = LocalDbHelper.LocalSqlite.GetRows(sql);
                        //    totalCount = getData.Length;
                        //    _dicTotals[keyName] = totalCount;
                        //}
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        totalCount = 0;
                    }
                    //}
                    //else
                    //{
                    //    totalCount = 0;
                    //}
                }
            }

            {
                if (_dicOks.ContainsKey(keyName))
                {
                    if (isOk)
                        _dicOks[keyName]++;
                    okCount = _dicOks[keyName];
                }
                else
                {
                    //if (LocalDbHelper.LocalSqlite != null)
                    //{
                    try
                    {
                        var sqlOk = string.Format("select taskNo from manufactureCheckData where creater = '{0}' and createTime between datetime('now','start of day','+1 seconds') and  datetime('now','start of day','+1 days','-1 seconds') and checkResult = '0001' group by taskNo", Program.DeviceNo + ":" + title + "_" + controlName);
                        okCount = LocalDbHelper.QueryBySqlString(sqlOk).Count;
                        _dicOks[keyName] = okCount;

                        //var getData = LocalDbHelper.LocalSqlite.GetRows(sqlOk);
                        //okCount = getData.Length;
                        //_dicOks[keyName] = okCount;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        okCount = 0;
                    }
                    //}
                    //else
                    //{
                    //    okCount = 0;
                    //}
                }
            }

            //st.Stop();
            //Console.WriteLine("GetCount耗时：" + st.ElapsedMilliseconds);
        }

        #endregion

        #region 根据配置文件初始化界面

        private void InitFormControls(DeviceConfig deviceConfig)
        {
            #region FormConfig

            var frmdesigner = deviceConfig.FormLayout;

            var witdth = Screen.GetWorkingArea(this).Width;
            var height = Screen.GetWorkingArea(this).Height;

            var newFont = Font;
            if (witdth <= 1024 || height <= 768)
                newFont = new Font("黑体", 8F, FontStyle.Bold, GraphicsUnit.Point, 134);

            // buttonTitle.Text = frmdesigner.;
            var lstColWidthPercentTemp =
                new List<string>(frmdesigner.ColumnPercent.Split(','));

            for (var i = 0; i < int.Parse(frmdesigner.ColumnCount); i++)
                _tableLayoutPanel.ColumnStyles.Add(
                    new ColumnStyle(SizeType.Percent, int.Parse(lstColWidthPercentTemp[i])));

            _tableLayoutPanel.RowCount = int.Parse(frmdesigner.RowCount);

            var perRow = 100f / int.Parse(frmdesigner.RowCount);

            for (var i = 0; i < int.Parse(frmdesigner.RowCount); i++)
                _tableLayoutPanel.RowStyles.Add(
                    new RowStyle(SizeType.Percent, perRow));

            _tableLayoutPanel.Dock = DockStyle.Fill;
            //_tableLayoutPanel.Size = new Size(1000, int.Parse(frmdesigner.RowCount) * int.Parse(frmdesigner.RowPixCount));

            if (deviceConfig.Controls == null ||
                deviceConfig.Controls.Length == 0)
            {
                stateTab.SelectedTab = tabPage4;
            }
            else
            {
                foreach (var control in deviceConfig.Controls)
                {
                    _dateTimeList.Add(control.Name, DateTime.Now);

                    if (control.Type == "Label")
                    {
                        var lbl = new Label
                        {
                            Name = control.Name,
                            Text = control.Name,
                            Dock = DockStyle.Fill,
                            TextAlign = ContentAlignment.MiddleCenter,
                            Font = newFont
                        };
                        _lstControls.Add(lbl);
                    }
                    else if (control.Type == "TextBox")
                    {
                        var tb = new TextBox
                        {
                            Name = control.Name,
                            Dock = DockStyle.Fill,
                            TextAlign = HorizontalAlignment.Center,
                            ReadOnly = true
                        };
                        _lstControls.Add(tb);
                    }
                    else if (control.Type == "LabelText")
                    {
                        var tb = new LabelText
                        {
                            Name = control.Name,
                            Dock = DockStyle.Fill,
                            //TextAlign = HorizontalAlignment.Center,
                            //ReadOnly = true
                        };
                        _lstControls.Add(tb);
                    }
                    else if (control.Type == "DataGrid")
                    {
                        var dg = new UserDataGrid
                        {
                            Name = control.Name,
                            Dock = DockStyle.Fill,
                            label = { Text = control.Name },
                            dataGridView = { Font = newFont },
                        };

                        if (witdth > 1024 && height > 768)
                            dg.dataGridView.RowsDefaultCellStyle.Font = new Font("黑体", 12F, FontStyle.Regular,
                                GraphicsUnit.Point, 120);
                        else
                        {
                            dg.dataGridView.RowsDefaultCellStyle.Font = new Font("黑体", 8F, FontStyle.Regular, GraphicsUnit.Point, 100);
                            dg.label.Font = new Font("黑体", 10, FontStyle.Regular);
                        }

                        dg.dataGridView.Columns.Add(
                            new DataGridViewTextBoxColumn { Name = "名称" });
                        dg.dataGridView.Columns.Add(
                            new DataGridViewTextBoxColumn { Name = "范围" });
                        dg.dataGridView.Columns.Add(
                            new DataGridViewTextBoxColumn { Name = "实际值" });
                        dg.dataGridView.Columns.Add(
                            new DataGridViewTextBoxColumn { Name = "结果" });

                        dg.dataGridView.Columns[3].Width = 20;

                        dg.dataGridView.ReadOnly = true;
                        dg.dataGridView.RowHeadersVisible = false;
                        dg.dataGridView.AllowUserToAddRows = false;

                        _lstControls.Add(dg);
                    }
                    else if (control.Type == "NiImageViewer")
                    {
                        var imgViewer = new UserImageDataViewer
                        {
                            Name = control.Name,
                            Dock = DockStyle.Fill,
                        };

                        imgViewer.userDataGridCheckData.label.Height = 30;
                        imgViewer.userDataGridCheckData.label.Font = new Font("微软雅黑", 20, FontStyle.Regular);
                        imgViewer.userDataGridCheckData.label.Text = control.Name;
                        imgViewer.userGrayData.label.Text = @"检测灰度值列表";

                        if (witdth > 1024 && height > 768)
                        {
                            //dg.dataGridView.RowsDefaultCellStyle.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, 120);

                            imgViewer.userDataGridCheckData.dataGridView.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, 120);
                            imgViewer.userDataGridCheckData.label.Font = new Font("微软雅黑", 20, FontStyle.Regular);
                        }
                        else
                        {
                            //dg.dataGridView.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8F, FontStyle.Regular, GraphicsUnit.Point, 100);
                            imgViewer.userDataGridCheckData.dataGridView.Font = new Font("微软雅黑", 8F, FontStyle.Regular, GraphicsUnit.Point, 100);
                            imgViewer.userDataGridCheckData.label.Font = new Font("微软雅黑", 10, FontStyle.Regular);
                        }

                        imgViewer.userDataGridCheckData.dataGridView.Columns.Add(
                            new DataGridViewTextBoxColumn { Name = "名称" });
                        imgViewer.userDataGridCheckData.dataGridView.Columns.Add(
                            new DataGridViewTextBoxColumn { Name = "范围" });
                        imgViewer.userDataGridCheckData.dataGridView.Columns.Add(
                            new DataGridViewTextBoxColumn { Name = "实际值" });
                        imgViewer.userDataGridCheckData.dataGridView.Columns.Add(
                            new DataGridViewTextBoxColumn { Name = "结果" });

                        imgViewer.userDataGridCheckData.dataGridView.Columns[3].Width = 20;

                        imgViewer.userGrayData.dataGridView.Columns.Add(
                            new DataGridViewTextBoxColumn { Name = "名称" });
                        imgViewer.userGrayData.dataGridView.Columns.Add(
                            new DataGridViewTextBoxColumn { Name = "范围" });
                        imgViewer.userGrayData.dataGridView.Columns.Add(
                            new DataGridViewTextBoxColumn { Name = "实际值" });

                        _lstControls.Add(imgViewer);
                    }
                    else if (control.Type == "ChartViewer")
                    {
                        var chartViewer = new UserChartDataViewer
                        {
                            Name = control.Name,
                            Dock = DockStyle.Fill,
                        };

                        chartViewer.userDataGridCheckData.label.Height = 30;
                        chartViewer.userDataGridCheckData.label.Font = new Font("微软雅黑", 20, FontStyle.Regular);
                        chartViewer.userDataGridCheckData.label.Text = control.Name;

                        if (witdth > 1024 && height > 768)
                        {
                            //dg.dataGridView.RowsDefaultCellStyle.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, 120);

                            chartViewer.userDataGridCheckData.dataGridView.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, 120);
                            chartViewer.userDataGridCheckData.label.Font = new Font("微软雅黑", 20, FontStyle.Regular);
                        }
                        else
                        {
                            //dg.dataGridView.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8F, FontStyle.Regular, GraphicsUnit.Point, 100);
                            chartViewer.userDataGridCheckData.dataGridView.Font = new Font("微软雅黑", 8F, FontStyle.Regular, GraphicsUnit.Point, 100);
                            chartViewer.userDataGridCheckData.label.Font = new Font("微软雅黑", 10, FontStyle.Regular);
                        }

                        chartViewer.userDataGridCheckData.dataGridView.Columns.Add(
                            new DataGridViewTextBoxColumn { Name = "名称" });
                        chartViewer.userDataGridCheckData.dataGridView.Columns.Add(
                            new DataGridViewTextBoxColumn { Name = "范围" });
                        chartViewer.userDataGridCheckData.dataGridView.Columns.Add(
                            new DataGridViewTextBoxColumn { Name = "实际值" });
                        chartViewer.userDataGridCheckData.dataGridView.Columns.Add(
                            new DataGridViewTextBoxColumn { Name = "结果" });

                        chartViewer.userDataGridCheckData.dataGridView.Columns[3].Width = 20;

                        _lstControls.Add(chartViewer);
                    }
                    else if (control.Type == "ListView")
                    {
                        var dg = new UserDataGrid
                        {
                            Name = control.Name,
                            Dock = DockStyle.Fill,
                            label = { Text = control.Name },
                            dataGridView = { Font = newFont },
                        };
                        dg.dataGridView.ReadOnly = true;
                        dg.dataGridView.RowHeadersVisible = false;
                        dg.dataGridView.AllowUserToAddRows = false;

                        if (witdth > 1024 && height > 768)
                            dg.dataGridView.RowsDefaultCellStyle.Font =
                                new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, 120);
                        else
                            dg.dataGridView.RowsDefaultCellStyle.Font =
                                new Font("微软雅黑", 8F, FontStyle.Regular, GraphicsUnit.Point, 100);

                        dg.dataGridView.Columns.Add(
                            new DataGridViewTextBoxColumn { Name = "检测项" });
                        dg.dataGridView.Columns.Add(
                            new DataGridViewTextBoxColumn { Name = "范围" });

                        var row1 = dg.dataGridView.Rows.Add();
                        dg.dataGridView.Rows[row1].Cells["检测项"].Value = @"检测结果";
                        dg.dataGridView.Rows[row1].Cells["范围"].Value = @"/";

                        var row2 = dg.dataGridView.Rows.Add();
                        dg.dataGridView.Rows[row2].Cells["检测项"].Value = @"计数";
                        dg.dataGridView.Rows[row2].Cells["范围"].Value = @"/";

                        _lstControls.Add(dg);
                    }
                    else if (control.Type == "ListViewItem")
                    {
                        var findListView =
                            deviceConfig.Controls.ToList()
                                .Find(
                                    f =>
                                        f.Type == "ListView" && f.ColumnPosition == control.ColumnPosition &&
                                        f.ColumnSpan == control.ColumnSpan && f.RowPosition == control.RowPosition &&
                                        f.RowSpan == control.RowSpan);

                        if (findListView != null)
                        {
                            var listView = _lstControls.Find(t => t.Name == findListView.Name);
                            var listView1 = listView as UserDataGrid;
                            if (listView1 != null)
                            {
                                listView1.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = control.Name });
                                for (var i = 0; i < listView1.dataGridView.Columns.Count; i++)
                                    listView1.dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                            }
                        }
                    }
                    else if (control.Type == "PictureBox")
                    {
                        var tb = new PictureBox
                        {
                            Name = control.Name,
                            Dock = DockStyle.Fill,
                            BackColor = Color.DarkGray,
                        };
                        _lstControls.Add(tb);
                    }
                    else if (control.Type == "VisionTab")
                    {
                        var dg = new UITabControl
                        {
                            Name = control.Name,
                            Dock = DockStyle.Fill,
                        };

                        _lstControls.Add(dg);
                    }

                    var cont = _lstControls.Find(t => t.Name == control.Name);
                    if (cont == null)
                        continue;

                    _tableLayoutPanel.Controls.Add(
                        cont, int.Parse(control.ColumnPosition), int.Parse(control.RowPosition));
                    _tableLayoutPanel.SetColumnSpan(
                        cont, int.Parse(control.ColumnSpan));
                    _tableLayoutPanel.SetRowSpan(
                        cont, int.Parse(control.RowSpan));
                }
                panelMain.Controls.Add(_tableLayoutPanel);
            }

            #endregion
        }

        #endregion

        #region 下拉列表按钮事件
        private void 工位1样件测试ToolStripMenuItem_Click(
            object sender, EventArgs e)
        {
            var toolStripMenuItem = sender as ToolStripMenuItem;

            if (toolStripMenuItem == null)
                return;
            var isChecked = toolStripMenuItem.Checked;

            buttonTitle.Text = isChecked
                ? buttonTitle.Text = string.Format("{0}_({1})", St.DeviceConfig.DeviceInfo.DeviceName, "样件检测模式")
                : buttonTitle.Text = string.Format("{0}", St.DeviceConfig.DeviceInfo.DeviceName);

            buttonTitle.BackColor = isChecked
                ? buttonTitle.BackColor = Color.DarkGoldenrod
                : buttonTitle.BackColor = Color.LightSeaGreen;

            foreach (var checkApp in
                St.LstControllers.OfType<ControllerBase>().Where(c => c.Name.Equals("检测程序")).OfType<CheckApp>())
                checkApp.IsByPass = isChecked;
        }

        private void 打开流程图ToolStripMenuItem_Click(
            object sender, EventArgs e)
        {
            St.IsPaused = true;

            try
            {
                var path = Program.SysDir;
                var xmlPath = XmlFile;
                //path + @"\流程配置文件\DeviceConfig.xml";
                var controllerPath =
                    Program.SysDir + @"\Controller.dll";
                var userControlPath =
                    Program.SysDir + @"\UserControls.dll";
                using (var form =
                    new DeviceDesign.FormMain(xmlPath, controllerPath, userControlPath))
                    form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"打开失败：" + ex.Message);
            }

            St.IsPaused = false;
        }

        private void 打开控制器调试界面ToolStripMenuItem_Click(
            object sender, EventArgs e)
        {
            //St.IsPaused = true;

            //using (var form = new FormControllerDebuger(St))
            //    form.ShowDialog();

            //St.IsPaused = false;


            try
            {
                if (_debugForm == null)
                {
                    _debugForm = new FormControllerDebuger(St);
                    _debugForm.Show();
                }
                else
                {
                    if (_debugForm.IsDisposed)
                    {
                        _debugForm = new FormControllerDebuger(St);
                    }
                    _debugForm.Show();
                    _debugForm.WindowState = FormWindowState.Normal;
                    _debugForm.Focus();
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void 打开流程监控界面ToolStripMenuItem_Click(
            object sender, EventArgs e)
        {
            //if (InvokeRequired)
            //{
            //    Invoke(new Action(() =>
            //    {
            //        try
            //        {
            //            using (var form = new FormStateMonitor(St))
            //                form.ShowDialog();
            //        }
            //        catch (Exception)
            //        {
            //            // ignored
            //        }
            //    }));
            //}
            try
            {
                using (var form = new FormStateMonitor(St))
                    form.ShowDialog();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void 打开机器人监控ToolStripMenuItem_Click(
            object sender, EventArgs e)
        {
            if (St == null)
                return;
            var findRobot = St.LstControllers.Find(f => f is RobotControllerPpMode);
            var robot = findRobot as RobotControllerPpMode;
            if (robot == null) return;
            var frm = new RobotControllerInfoTrace(robot);
            frm.ShowDialog();
        }

        private void cANMessageToolStripMenuItem_Click(
            object sender, EventArgs e)
        {
            try
            {
                if (_canMsgForm == null)
                {
                    _canMsgForm = new FormCanMsgMonitor();
                    _canMsgForm.Show();
                }
                else
                {
                    if (_canMsgForm.IsDisposed)
                    {
                        _canMsgForm = new FormCanMsgMonitor();
                    }
                    _canMsgForm.Show();
                    _canMsgForm.WindowState = FormWindowState.Normal;
                    _canMsgForm.Focus();
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void linMessageToolStripMenuItem_Click(
            object sender, EventArgs e)
        {
            try
            {
                if (_linMsgForm == null)
                {
                    _linMsgForm = new FormCanMsgMonitor();
                    _linMsgForm.Show();
                }
                else
                {
                    if (_linMsgForm.IsDisposed)
                    {
                        _linMsgForm = new FormLinMsgMonitor();
                    }
                    _linMsgForm.Show();
                    _linMsgForm.WindowState = FormWindowState.Normal;
                    _linMsgForm.Focus();
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void 信号ToolStripMenuItem_Click(
            object sender, EventArgs e)
        {
            try
            {
                if (_partMonitorForm == null)
                {
                    _partMonitorForm = new FormPartMonitor(St);
                    _partMonitorForm.Show();
                }
                else
                {
                    if (_partMonitorForm.IsDisposed)
                    {
                        _partMonitorForm = new FormPartMonitor(St);
                    }
                    _partMonitorForm.Show();
                    _partMonitorForm.WindowState = FormWindowState.Normal;
                    _partMonitorForm.Focus();
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
        #endregion

        private void s12L数据查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new S12LDataViewer();
            frm.ShowDialog();
        }

        private void 查看本地检测数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmVisionCheckDataHistory())
            {
                frm.ShowDialog();
            }
        }

        private void 单帧ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            St.IsPaused = true;

            var camera = St.LstControllers.Find(f => f is LedVisionAnalysisByDaHengCamera);

            var option = new UIEditOption
            {
                AutoLabelWidth = true,
                Text = "确认",
                ValueWidth = 800,
            };

            var listName = new List<string>();
            var listParaIndex = new List<int>();
            var allPara = St.DeviceConfig.Paras.ToList().FindAll(f => f.DataType?.ToLower() == "roi" || f.DataType?.ToLower() == "roi[]");
            for (var i = 0; i < allPara.Count; i++)
            {
                var para = allPara[i];
                var roiParaName = string.Format("{0}.para.{1}", para.ProcessNo, para.Name);
                if (St.DeviceConfig.Conditions.ToList().FindAll(f => f.ConditionFunction.ToLower().Contains(roiParaName.ToLower())).Count > 0)
                {
                    listName.Add(string.Format("{0}_{1}", para.ProcessNo.Substring(para.ProcessNo.Length - 3, 3), para.Name));
                    listParaIndex.Add(i);
                }
            }
            option.AddCombobox("para", "Name", listName.ToArray(), selectedIndex: 0);

            var frm = new UIEditForm(option) { Font = new Font("宋体", 6f) };
            frm.Render();
            frm.ShowDialog();

            if (!frm.IsOK)
            {
                ShowInfoTip("操作取消");
                St.IsPaused = false;
                return;
            }

            var paraIndex = (int)frm["para"];
            var editPara = allPara[listParaIndex[paraIndex]];

            if (editPara?.DataType.ToLower() == "roi")
            {
                using (var form = new FormStaticVisionCali(St, editPara))
                {
                    form.ShowDialog();
                }
            }
            else if (editPara?.DataType.ToLower() == "roi[]")
            {
                using (var form = new FormDynamicVisionCali(St, editPara))
                {
                    form.ShowDialog();
                }
            }

            St.IsPaused = false;
        }

        private void 批量ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            St.IsPaused = true;

            using (var form = new FormBatchStaticVisionCali(St))
            {
                form.ShowDialog();
            }

            St.IsPaused = false;
        }
    }
}
