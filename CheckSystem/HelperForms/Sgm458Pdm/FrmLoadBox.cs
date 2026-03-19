using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using CommonUtility;
using CommonUtility.BusLoader;
using Controller;
using HZH_Controls.IconFont;
using Newtonsoft.Json;
using Sunny.UI;
using FontImages = HZH_Controls.IconFont.FontImages;

namespace CheckSystem.HelperForms.Sgm458Pdm
{
    public partial class FrmLoadBox : UIForm
    {
        private readonly List<CheckItem> _checkItems = new List<CheckItem>();

        //private readonly object _lockLinController = new object();
        private readonly SyControllerWith56Pin _linController =
            new SyControllerWith56Pin("LIN控制板");
        private readonly Dictionary<int, SyControllerWith56Pin> _dutControllers =
            new Dictionary<int, SyControllerWith56Pin>();
        private readonly Dictionary<int, CanBus> _dutCanBus =
          new Dictionary<int, CanBus>();

        private readonly Dictionary<int, CanBus.CanDataPackage> _pdmResponse1_0X201 =
            new Dictionary<int, CanBus.CanDataPackage>();
        private readonly Dictionary<int, CanBus.CanDataPackage> _pdmResponse2_0X202 =
            new Dictionary<int, CanBus.CanDataPackage>();
        private BackgroundWorker RefreshBackgroundWorker { get; set; }
        private BackgroundWorker LinControllerBackgroundWorker { get; set; }
        private bool RelayFlag { get; set; }
        private bool IsLockBackgroundWorker { get; set; }

        private const string Vbatt1 = "VBATT1";
        private const string Vbatt2 = "VBATT2";
        private const string Vcc2_5V = "VCC2_5V";
        private const string HallFeed = "HALL_FEED";
        private const string HallASpeed = "HALL_A_speed";
        private const string HallBDirection = "HALL_B_direction";
        private const string ActuatorCurrent = "Actuator_Current";
        private const string CinchCurrent = "Cinch_Current";
        private const string ReleaseCurrent = "Release_Current";
        private const string FullSwitch = "Full_Switch";
        private const string NeutSw = "NEUT_SW";
        private const string HalfSwitch = "Half_Switch";

        private const int MotorRunKeepTime = 5;
        private const int MotorNotRunKeepTime = 20;

        internal class CheckItem
        {
            public string Name { get; set; }
            public double Min { get; set; }
            public double Max { get; set; }
            public string Unit { get; set; }
            public string StandardValue { get; set; }
            public bool IsJudgeRange { get; private set; }

            public uint CanId { get; private set; }
            public MatrixValDefinition MatrixValDefinition { get; private set; }
            public double Factor { get; private set; }
            public double Offset { get; private set; }

            public CheckItem(
                string name, double min, double max, string unit,
                uint canId, int startBit, int len, double factor, double offset)
            {
                Name = name;
                Min = min;
                Max = max;
                Unit = unit;
                IsJudgeRange = true;
                SetCanDefinition(canId, startBit, len, factor, offset);
            }

            public CheckItem(
                string name, string standardValue,
                uint canId, int startBit, int len, double factor, double offset)
            {
                Name = name;
                StandardValue = standardValue;
                IsJudgeRange = false;
                SetCanDefinition(canId, startBit, len, factor, offset);
            }

            private void SetCanDefinition(
                uint canId, int startBit, int len, double factor, double offset)
            {
                CanId = canId;
                MatrixValDefinition = new MatrixValDefinition(startBit, len, 0);
                Factor = factor;
                Offset = offset;
            }

            public bool CheckInvoke(double actualValue)
            {
                return actualValue >= Min && actualValue <= Max;
            }

            public bool CheckInvoke(string actualValue)
            {
                return actualValue == StandardValue;
            }
        }

        public FrmLoadBox()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            _wokerCountPer10Ms = 0;

            txtMode1KeepTime.Minimum = MotorRunKeepTime + MotorNotRunKeepTime;
            txtMode1KeepTime.IntValue = MotorRunKeepTime + MotorNotRunKeepTime;

            Icon = FontImages.GetIcon(
                FontIcons.E_social_googledrive, 32, Color.DodgerBlue);
            Load += FrmLoadBox_Load;
            Closed += FrmLoadBox_Closed;
        }

        private void FrmLoadBox_Load(object sender, EventArgs e)
        {
            InitCheckItems();
            InigDgv();
            InitDevice();
            InitThread();
            CanBus.PushCanMsg += CanBus_PushCanMsg;
            btnMode1.PerformClick();
        }

        private void FrmLoadBox_Closed(object sender, EventArgs e)
        {
            if (RefreshBackgroundWorker != null)
            {
                RefreshBackgroundWorker.CancelAsync();
            }

            _linController.Dispose();

            for (var i = 1; i <= 3; i++)
                _dutControllers[i].Dispose();

            Environment.Exit(0);
        }

        private void CanBus_PushCanMsg(
            string name, CanBus.CanDataPackage data, CanBus.OnPushCanDataType onPushCanDataType)
        {
            for (var i = 1; i <= 6; i++)
            {
                if (_dutCanBus[i].Name != name ||
                    (data.CanId != 0x201 && data.CanId != 0x202) ||
                    onPushCanDataType == CanBus.OnPushCanDataType.Tx)
                    continue;

                if (data.CanId == 0x201)
                    _pdmResponse1_0X201[i] = data;
                else if (data.CanId == 0x202)
                    _pdmResponse2_0X202[i] = data;
            }
        }

        private void InitCheckItems()
        {
            _checkItems.Clear();

            _checkItems.Add(new CheckItem(Vbatt1, 9, 16, "V", 0x201, 14, 10, 0.03125, 0));
            _checkItems.Add(new CheckItem(Vbatt2, 9, 16, "V", 0x201, 20, 10, 0.03125, 0));
            _checkItems.Add(new CheckItem(Vcc2_5V, 4.5, 5.5, "V", 0x201, 28, 8, 0.03125, 0));
            _checkItems.Add(new CheckItem(HallFeed, 8, 16, "V", 0x0201, 40, 10, 0.03125, 0));
            _checkItems.Add(new CheckItem(HallASpeed, 80, 120, "HZ", 0x202, 8, 16, 1, 0));
            _checkItems.Add(new CheckItem(HallBDirection, "1", 0x202, 23, 1, 1, 0));
            _checkItems.Add(new CheckItem(ActuatorCurrent, 0, 15, "A", 0x202, 41, 10, 0.03125, 0));
            _checkItems.Add(new CheckItem(CinchCurrent, 0, 8, "A", 0x202, 35, 10, 0.03125, 0));
            _checkItems.Add(new CheckItem(ReleaseCurrent, 0, 8, "A", 0x202, 63, 10, 0.03125, 0));
            _checkItems.Add(new CheckItem(FullSwitch, "1", 0x201, 61, 1, 1, 0));
            _checkItems.Add(new CheckItem(NeutSw, "1", 0x201, 60, 1, 1, 0));
            _checkItems.Add(new CheckItem(HalfSwitch, 2.5, 16, "V", 0x202, 29, 10, 0.03125, 0));
        }

        private void InigDgv()
        {
            mainDgv.Style = UIStyle.Blue;
            mainDgv.ReadOnly = true;
            mainDgv.RowHeadersVisible = false;
            mainDgv.AllowUserToAddRows = false;
            mainDgv.AllowUserToResizeRows = false;
            mainDgv.MultiSelect = true;
            mainDgv.RowHeadersVisible = false;
            mainDgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            mainDgv.ClearRows();
            mainDgv.ClearColumns();
            mainDgv.AddColumn("测试项目", "测试项目");
            mainDgv.AddColumn("范围", "范围");
            mainDgv.AddColumn("单位", "单位");
            for (var i = 0; i < 6; i++)
            {
                var str = string.Format("Dut{0}", i + 1);
                mainDgv.AddColumn(str, str);
            }

            foreach (var t in _checkItems)
            {
                if (t.IsJudgeRange)
                {
                    mainDgv.AddRow(t.Name, string.Format("{0}~{1}", t.Min, t.Max), t.Unit, "", "", "", "", "", "");
                }
                else
                {
                    mainDgv.AddRow(t.Name, t.StandardValue, "/", "", "", "", "", "", "");
                }
            }

            mainDgv.AddRow("CAN通信", "", "", "", "", "", "", "", "");

            //mainDgv.AutoResizeColumns();

            var witdth = Screen.GetWorkingArea(this).Width;
            var height = Screen.GetWorkingArea(this).Height;

            var cellWidth = 200;
            var rowHeight = 60;
            if (witdth <= 1024 || height <= 768)
            {
                rowHeight = 35;
                cellWidth = 160;
            }

            mainDgv.Columns[0].Width = cellWidth;
            for (var i = 0; i < mainDgv.RowCount; i++)
                mainDgv.Rows[i].Height = rowHeight;
            //mainDgv.AutoResizeRows();
        }

        private void InitDevice()
        {
            _linController.InitRemoteIpAddress("192.168.1.31:8088");
            _linController.SetOutputs();
            Thread.Sleep(1000);

            var baseIp = 28;
            var canbusIndex = 0;
            for (var i = 0; i < 3; i++)
            {
                var ip = string.Format("192.168.1.{0}:8088", baseIp);
                _dutControllers.Add(i + 1, new SyControllerWith56Pin(ip));
                _dutControllers[i + 1].InitRemoteIpAddress(ip);

                canbusIndex++;
                _dutCanBus.Add(canbusIndex, _dutControllers[i + 1].GatwayCan1);
                canbusIndex++;
                _dutCanBus.Add(canbusIndex, _dutControllers[i + 1].GatwayCan2);

                baseIp++;
            }

            for (var i = 0; i < 6; i++)
            {
                _pdmResponse1_0X201.Add(i + 1, null);
                _pdmResponse2_0X202.Add(i + 1, null);

                _lossCanCount.Add(i + 1, 0);
                _checkNgCount.Add(i + 1, 0);
            }
        }

        private void InitThread()
        {
            RefreshBackgroundWorker = new BackgroundWorker();
            RefreshBackgroundWorker.DoWork += RefreshBackgroundWorker_DoWork;
            RefreshBackgroundWorker.WorkerReportsProgress = true;
            RefreshBackgroundWorker.WorkerSupportsCancellation = true;
            RefreshBackgroundWorker.RunWorkerAsync();

            //LinControllerBackgroundWorker = new BackgroundWorker();
            //LinControllerBackgroundWorker.DoWork += LinControllerBackgroundWorker_DoWork;
            //LinControllerBackgroundWorker.WorkerReportsProgress = true;
            //LinControllerBackgroundWorker.WorkerSupportsCancellation = true;
            //LinControllerBackgroundWorker.RunWorkerAsync();
        }

        private void tsBtn_Click(object sender, EventArgs e)
        {
            var tbBtn = sender as ToolStripButton;
            if (tbBtn == null) return;
            tbBtn.BackColor = tbBtn.BackColor == Color.Green ? Color.Gray : Color.Green;
        }

        private void btnStartModeSwitch_Click(object sender, EventArgs e)
        {
            if (!_isAutoMode)
            {
                IsLockBackgroundWorker = true;

                lblMode1RemainTime.BackColor = Color.Green;
                lblMode4RemainTime.BackColor = Color.Gray;

                btnMode1.PerformClick();
                btnMode1.Enabled = false;
                btnMode2.Enabled = false;
                btnMode4.Enabled = false;

                btnUpdateSwitchTime.Enabled = true;

                btnStartModeSwitch.Text = @"循环模式停止";

                txtMode1RemainTime.IntValue = txtMode1KeepTime.IntValue;
                txtMode4RemainTime.IntValue = txtMode4KeepTime.IntValue;
                Mode1RemainTime = txtMode1RemainTime.IntValue;
                Mode4RemainTime = txtMode4RemainTime.IntValue;

                _wokerCountPer10Ms = 0;
                _mode1KeepTime = 0;
                _mode4KeepTime = 0;
                _isAutoMode = true;

                IsLockBackgroundWorker = false;
            }
            else
            {
                IsLockBackgroundWorker = true;

                _wokerCountPer10Ms = 0;
                _mode1KeepTime = 0;
                _mode4KeepTime = 0;
                _isAutoMode = false;

                lblMode1RemainTime.BackColor = Color.Gray;
                lblMode4RemainTime.BackColor = Color.Gray;

                btnMode1.Enabled = true;
                btnMode2.Enabled = true;
                btnMode4.Enabled = true;

                btnUpdateSwitchTime.Enabled = false;

                btnStartModeSwitch.Text = @"循环模式开始";
                btnMode4.PerformClick();

                IsLockBackgroundWorker = false;
            }
        }

        private void btnUpdateSwitchTime_Click(object sender, EventArgs e)
        {
            if (this.ShowAskDialog("是否确定更新？"))
            {
                if (_isAutoMode)
                {
                    IsLockBackgroundWorker = true;

                    lblMode1RemainTime.BackColor = Color.Green;
                    lblMode4RemainTime.BackColor = Color.Gray;

                    txtMode1RemainTime.IntValue = txtMode1KeepTime.IntValue;
                    txtMode4RemainTime.IntValue = txtMode4KeepTime.IntValue;
                    Mode1RemainTime = txtMode1RemainTime.IntValue;
                    Mode4RemainTime = txtMode4RemainTime.IntValue;

                    _wokerCountPer10Ms = 0;
                    _mode1KeepTime = 0;
                    _mode4KeepTime = 0;

                    GoToMode1_100();

                    IsLockBackgroundWorker = false;
                }
                else
                {
                    txtMode1RemainTime.IntValue = txtMode1KeepTime.IntValue;
                    txtMode4RemainTime.IntValue = txtMode4KeepTime.IntValue;
                }
            }
            else
            {
                this.ShowInfoTip("取消了操作");
            }
        }

        private int Mode1RemainTime { get; set; }
        private int Mode4RemainTime { get; set; }

        #region LIN控制帧

        /// <summary>
        /// LIN控制帧
        /// </summary>
        private readonly LinCommunicationMatrix.IntelMatrix _pdmCtrlCmd =
            new LinCommunicationMatrix.IntelMatrix(0x01, 1);

        /// <summary>
        /// Mode
        /// </summary>
        private readonly MatrixValDefinition _runMd =
            new MatrixValDefinition(5, 3, 1);

        /// <summary>
        /// 执行器电机
        /// </summary>
        private readonly MatrixValDefinition _actMt =
            new MatrixValDefinition(2, 1, 0);

        /// <summary>
        /// 吸合电机
        /// </summary>
        private readonly MatrixValDefinition _cinMt =
            new MatrixValDefinition(3, 1, 0);

        /// <summary>
        /// 释放电机
        /// </summary>
        private readonly MatrixValDefinition _rlsMt =
            new MatrixValDefinition(4, 1, 0);

        private void btnMode1_Click(object sender, EventArgs e)
        {
            GoToMode1_100();
        }

        private void btnMode2_Click(object sender, EventArgs e)
        {
            GoToMode2_0();
        }

        private void btnMode4_Click(object sender, EventArgs e)
        {
            GoToMode4_Sleep();
        }

        /// <summary>
        /// 释放电机
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReleaseMotor_Click(object sender, EventArgs e)
        {
            if (_rlsMt.Value == 1)
            {
                _rlsMt.Value = 0x00;
                BtnShowGray(sender as UIButton);
            }
            else
            {
                _rlsMt.Value = 0x01;
                BtnShowBlue(sender as UIButton);
            }
        }

        /// <summary>
        /// 吸合电机
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCinchMotor_Click(object sender, EventArgs e)
        {
            if (_cinMt.Value == 1)
            {
                _cinMt.Value = 0x00;
                BtnShowGray(sender as UIButton);
            }
            else
            {
                _cinMt.Value = 0x01;
                BtnShowBlue(sender as UIButton);
            }
        }

        /// <summary>
        /// 执行器电机
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnActuatorMotor_Click(object sender, EventArgs e)
        {
            if (_actMt.Value == 1)
            {
                _actMt.Value = 0x00;
                BtnShowGray(sender as UIButton);
            }
            else
            {
                _actMt.Value = 0x01;
                BtnShowBlue(sender as UIButton);
            }
        }

        private static void BtnShowGray(UIButton btn)
        {
            btn.FillColor = Color.FromArgb(140, 140, 140);
            btn.FillColor2 = Color.FromArgb(140, 140, 140);
            btn.FillHoverColor = Color.FromArgb(163, 163, 163);
            btn.FillPressColor = Color.FromArgb(112, 112, 112);
            btn.FillSelectedColor = Color.FromArgb(112, 112, 112);
        }

        private static void BtnShowBlue(UIButton btn)
        {
            btn.FillColor = Color.FromArgb(80, 160, 255);
            btn.FillColor2 = Color.FromArgb(80, 160, 255);
            btn.FillHoverColor = Color.FromArgb(115, 179, 255);
            btn.FillPressColor = Color.FromArgb(64, 128, 204);
            btn.FillSelectedColor = Color.FromArgb(64, 128, 204);
        }


        private int _mode4SendCount;

        private void GoToMode4_Sleep()
        {
            BtnShowBlue(btnMode4);
            BtnShowGray(btnMode1);
            BtnShowGray(btnMode2);

            RelayFlag = false;

            if (_runMd.Value != 4)
            {
                _mode4SendCount = 5;
            }

            _runMd.Value = 4;
            label_status.Text = @"休眠模式";

            if (_rlsMt.Value == 1)
            {
                _rlsMt.Value = 0x00;
                BtnShowGray(btnReleaseMotor);
            }

            if (_cinMt.Value == 1)
            {
                _cinMt.Value = 0x00;
                BtnShowGray(btnCinchMotor);
            }

            if (_actMt.Value == 1)
            {
                _actMt.Value = 0x00;
                BtnShowGray(btnActuatorMotor);
            }

            _motorRunCount = 0;
        }

        private void GoToMode1_100()
        {
            BtnShowBlue(btnMode1);
            BtnShowGray(btnMode2);
            BtnShowGray(btnMode4);

            _runMd.Value = 1;
            label_status.Text = @"正常模式100%";

            if (_rlsMt.Value == 0)
            {
                _rlsMt.Value = 0x01;
                BtnShowBlue(btnReleaseMotor);
            }

            if (_cinMt.Value == 0)
            {
                _cinMt.Value = 0x01;
                BtnShowBlue(btnCinchMotor);
            }

            if (_actMt.Value == 0)
            {
                _actMt.Value = 0x01;
                BtnShowBlue(btnActuatorMotor);
            }

            _motorRunCount = 0;
        }

        private void GoToMode2_0()
        {
            BtnShowBlue(btnMode2);
            BtnShowGray(btnMode1);
            BtnShowGray(btnMode4);

            _runMd.Value = 1;
            label_status.Text = @"正常模式0%";

            if (_rlsMt.Value == 1)
            {
                _rlsMt.Value = 0x00;
                BtnShowGray(btnReleaseMotor);
            }

            if (_cinMt.Value == 1)
            {
                _cinMt.Value = 0x00;
                BtnShowGray(btnCinchMotor);
            }

            if (_actMt.Value == 1)
            {
                _actMt.Value = 0x00;
                BtnShowGray(btnActuatorMotor);
            }
        }

        #endregion

        #region 主控制

        private bool _isAutoMode;
        private int _wokerCountPer10Ms;
        private int _motorRunCount;
        private int _mode1KeepTime;
        private int _mode4KeepTime;

        private readonly Dictionary<int, int> _lossCanCount = new Dictionary<int, int>();
        private readonly Dictionary<int, int> _checkNgCount = new Dictionary<int, int>();

        private int _isControllerFailed;

        private void LinControllerBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;

            // Keep acquiring until we error or cancel
            while (!worker.CancellationPending)
            {
                try
                {
                    Thread.Sleep(10);


                }
                catch (Exception exception)
                {
                    // If we have been stopped, ignore the exception since
                    // it is likely just an error indicating the acquisition has
                    // been stopped
                    if (!worker.CancellationPending)
                    {
                        e.Result = exception;
                    }
                    return;
                }
            }
        }

        private void RefreshBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;

            // Keep acquiring until we error or cancel
            while (!worker.CancellationPending)
            {
                try
                {
                    mainDgv.ClearSelection();
                    Thread.Sleep(10);

                    if (IsLockBackgroundWorker)
                        continue;
                    _wokerCountPer10Ms++;

                    if (_linController != null && _linController.GatewayLin != null)
                    {
                        //if (_isControllerFailed > 0 && _controllerState == 0)
                        //{
                        //    _linController.Relay1 = true;
                        //    _linController.Relay2 = true;
                        //    _linController.Relay3 = true;
                        //    _linController.SetOutputs();
                        //    _controllerState = 1;
                        //    _isControllerFailed = 0;
                        //}

                        if (_wokerCountPer10Ms == 25 ||
                            _wokerCountPer10Ms == 45 ||
                            _wokerCountPer10Ms == 65)
                        {
                            if (_runMd.Value != 4)
                            {
                                _pdmCtrlCmd.UpdateData(_runMd);
                                _pdmCtrlCmd.UpdateData(_actMt);
                                _pdmCtrlCmd.UpdateData(_cinMt);
                                _pdmCtrlCmd.UpdateData(_rlsMt);
                                _linController.GatewayLin.SendMasterLin(_pdmCtrlCmd.MasterLinId, _pdmCtrlCmd.MatrixData);
                                Thread.Sleep(10);
                            }
                            else
                            {
                                if (_mode4SendCount > 0)
                                {
                                    _mode4SendCount--;
                                    _pdmCtrlCmd.UpdateData(_runMd);
                                    _pdmCtrlCmd.UpdateData(_actMt);
                                    _pdmCtrlCmd.UpdateData(_cinMt);
                                    _pdmCtrlCmd.UpdateData(_rlsMt);
                                    _linController.GatewayLin.SendMasterLin(_pdmCtrlCmd.MasterLinId, _pdmCtrlCmd.MatrixData);
                                    Thread.Sleep(10);
                                }
                            }
                        }
                    }

                    if (_wokerCountPer10Ms >= 50)
                    {
                        if (!IsLockBackgroundWorker)
                        {
                            if (_wokerCountPer10Ms == 50) // 500ms
                            {
                                if (_runMd.Value == 4)
                                {
                                    RelayFlag = false;
                                }
                                else
                                {
                                    _motorRunCount++;

                                    if (_motorRunCount > MotorRunKeepTime &&
                                        _motorRunCount <= MotorRunKeepTime + MotorNotRunKeepTime)
                                    {
                                        Invoke(new Action(GoToMode2_0));
                                    }
                                    else if (_motorRunCount > MotorRunKeepTime + MotorNotRunKeepTime)
                                    {
                                        Invoke(new Action(GoToMode1_100));
                                    }

                                    if (_checkNgCount[1] == 0 &&
                                        _checkNgCount[2] == 0 &&
                                        _checkNgCount[3] == 0 &&
                                        _checkNgCount[4] == 0 &&
                                        _checkNgCount[5] == 0 &&
                                        _checkNgCount[6] == 0)
                                    {
                                        RelayFlag = !RelayFlag;
                                    }
                                }

                                if (_isControllerFailed == 0)
                                {
                                    for (var i = 1; i <= 3; i++)
                                    {
                                        if (_dutControllers[i] == null)
                                            continue;

                                        _dutControllers[i].Relay1 = RelayFlag;
                                        _dutControllers[i].Relay2 = RelayFlag;
                                        _dutControllers[i].Relay3 = RelayFlag;
                                        _dutControllers[i].Relay4 = RelayFlag;
                                        _dutControllers[i].Relay5 = RelayFlag;
                                        _dutControllers[i].Relay6 = RelayFlag;
                                        _dutControllers[i].Relay7 = RelayFlag;
                                        _dutControllers[i].Relay8 = RelayFlag;
                                        _dutControllers[i].Relay9 = RelayFlag;
                                        _dutControllers[i].Relay10 = RelayFlag;
                                        _dutControllers[i].Relay11 = RelayFlag;
                                        _dutControllers[i].Relay12 = RelayFlag;

                                        if (i == 1 &&
                                            (tsBtn1.BackColor == Color.Green || tsBtn2.BackColor == Color.Green))
                                        {
                                            if (!_dutControllers[i].SetOutputs())
                                            {
                                                if (_pdmResponse1_0X201[i] == null || _pdmResponse2_0X202[i] == null)
                                                {
                                                    _isControllerFailed++;
                                                }
                                                else
                                                {
                                                    var dateTimeNow = DateTime.Now;
                                                    var dt1 = ValueHelper.GetTimeSpanMs(_pdmResponse1_0X201[i].DateTime, dateTimeNow);
                                                    var dt2 = ValueHelper.GetTimeSpanMs(_pdmResponse2_0X202[i].DateTime, dateTimeNow);

                                                    if (dt1 > 500 && dt2 > 500)
                                                    {
                                                        _isControllerFailed++;
                                                    }
                                                }
                                            }
                                        }
                                        else if (i == 2 &&
                                            (tsBtn3.BackColor == Color.Green || tsBtn4.BackColor == Color.Green))
                                        {
                                            if (!_dutControllers[i].SetOutputs())
                                            {
                                                if (_pdmResponse1_0X201[i] == null || _pdmResponse2_0X202[i] == null)
                                                {
                                                    _isControllerFailed++;
                                                }
                                                else
                                                {
                                                    var dateTimeNow = DateTime.Now;
                                                    var dt1 = ValueHelper.GetTimeSpanMs(_pdmResponse1_0X201[i].DateTime, dateTimeNow);
                                                    var dt2 = ValueHelper.GetTimeSpanMs(_pdmResponse2_0X202[i].DateTime, dateTimeNow);

                                                    if (dt1 > 500 && dt2 > 500)
                                                    {
                                                        _isControllerFailed++;
                                                    }
                                                }
                                            }
                                        }
                                        else if (i == 3 &&
                                            (tsBtn5.BackColor == Color.Green || tsBtn6.BackColor == Color.Green))
                                        {
                                            if (!_dutControllers[i].SetOutputs())
                                            {
                                                if (_pdmResponse1_0X201[i] == null || _pdmResponse2_0X202[i] == null)
                                                {
                                                    _isControllerFailed++;
                                                }
                                                else
                                                {
                                                    var dateTimeNow = DateTime.Now;
                                                    var dt1 = ValueHelper.GetTimeSpanMs(_pdmResponse1_0X201[i].DateTime, dateTimeNow);
                                                    var dt2 = ValueHelper.GetTimeSpanMs(_pdmResponse2_0X202[i].DateTime, dateTimeNow);

                                                    if (dt1 > 500 && dt2 > 500)
                                                    {
                                                        _isControllerFailed++;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (_isControllerFailed > 0)
                                    {
                                        UpdateDutSwitchStandardValue(RelayFlag ? "0" : "1");
                                        Invoke(new Action(InigDgv));

                                        RelayFlag = false;
                                        _linController.Relay1 = true;
                                        _linController.Relay2 = true;
                                        _linController.Relay3 = true;
                                        _linController.SetOutputs();

                                        Thread.Sleep(250);

                                        _linController.Relay1 = false;
                                        _linController.Relay2 = false;
                                        _linController.Relay3 = false;
                                        _linController.SetOutputs();

                                        Thread.Sleep(2500);

                                        //_isControllerFailed = 0;
                                    }
                                }
                            }
                            else if (_wokerCountPer10Ms >= 100) // 1000ms
                            {
                                _wokerCountPer10Ms = 0;

                                if (_isAutoMode)
                                {
                                    if (_runMd.Value == 1 || _runMd.Value == 2)
                                    {
                                        _mode1KeepTime++;
                                        if (_mode1KeepTime == Mode1RemainTime)
                                        {
                                            Invoke(new Action(() =>
                                            {
                                                txtMode1RemainTime.IntValue = 0;
                                                txtMode4RemainTime.IntValue = Mode4RemainTime;
                                                lblMode1RemainTime.BackColor = Color.Gray;
                                                lblMode4RemainTime.BackColor = Color.Green;

                                                GoToMode4_Sleep();

                                                _mode1KeepTime = 0;
                                                _mode4KeepTime = 0;
                                            }));
                                        }
                                        else
                                        {
                                            var time = _mode1KeepTime;
                                            Invoke(new Action(() =>
                                            {
                                                txtMode1RemainTime.IntValue = Mode1RemainTime - time;
                                            }));
                                        }
                                    }
                                    else
                                    {
                                        _mode4KeepTime++;
                                        if (_mode4KeepTime == Mode4RemainTime)
                                        {
                                            Invoke(new Action(() =>
                                            {
                                                txtMode1RemainTime.IntValue = Mode1RemainTime;
                                                txtMode4RemainTime.IntValue = 0;
                                                lblMode1RemainTime.BackColor = Color.Green;
                                                lblMode4RemainTime.BackColor = Color.Gray;

                                                GoToMode1_100();

                                                _mode1KeepTime = 0;
                                                _mode4KeepTime = 0;
                                            }));
                                            continue;
                                        }

                                        var time = _mode4KeepTime;
                                        Invoke(new Action(() =>
                                        {
                                            txtMode4RemainTime.IntValue = Mode4RemainTime - time;
                                        }));
                                    }
                                }
                                else
                                {
                                    _mode1KeepTime = 0;
                                    _mode4KeepTime = 0;
                                }

                                if (_isControllerFailed > 0)
                                {
                                    _isControllerFailed = 0;
                                }
                                else
                                {
                                    UpdateDutSwitchStandardValue(RelayFlag ? "0" : "1");
                                    for (var i = 1; i <= 6; i++)
                                    {
                                        UpdateDutDgv(i, _pdmResponse1_0X201[i], _pdmResponse2_0X202[i]);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    // If we have been stopped, ignore the exception since
                    // it is likely just an error indicating the acquisition has
                    // been stopped
                    if (!worker.CancellationPending)
                    {
                        e.Result = exception;
                    }
                    return;
                }
            }
        }

        private delegate void UpdateDutDgvDelegate(
            int dutIndex, CanBus.CanDataPackage pdmResponse1, CanBus.CanDataPackage pdmResponse2);
        private void UpdateDutDgv(
            int dutIndex, CanBus.CanDataPackage pdmResponse1, CanBus.CanDataPackage pdmResponse2)
        {
            var cellIndex = -1;
            if (dutIndex == 1 &&
                tsBtn1.BackColor == Color.Green)
                cellIndex = 3;
            else if (dutIndex == 2 &&
                tsBtn2.BackColor == Color.Green)
                cellIndex = 4;
            else if (dutIndex == 3 &&
                tsBtn3.BackColor == Color.Green)
                cellIndex = 5;
            else if (dutIndex == 4 &&
                tsBtn4.BackColor == Color.Green)
                cellIndex = 6;
            else if (dutIndex == 5 &&
                tsBtn5.BackColor == Color.Green)
                cellIndex = 7;
            else if (dutIndex == 6 &&
                tsBtn6.BackColor == Color.Green)
                cellIndex = 8;

            if (cellIndex == -1)
            {
                if (dutIndex == 1)
                    cellIndex = 3;
                else if (dutIndex == 2)
                    cellIndex = 4;
                else if (dutIndex == 3)
                    cellIndex = 5;
                else if (dutIndex == 4)
                    cellIndex = 6;
                else if (dutIndex == 5)
                    cellIndex = 7;
                else if (dutIndex == 6)
                    cellIndex = 8;

                for (var i = 0; i < mainDgv.RowCount; i++)
                {
                    mainDgv.Rows[i].Cells[cellIndex].Style.BackColor = Color.Gray;
                    mainDgv.Rows[i].Cells[cellIndex].Value = string.Empty;
                }
                return;
            }

            var updateDutDgvDelegate = new UpdateDutDgvDelegate(UpdateDutDgv);

            if (mainDgv.InvokeRequired)
            {
                Invoke(updateDutDgvDelegate, dutIndex, pdmResponse1, pdmResponse2);
            }
            else
            {
                var model = new LoadBoxSqlHelper.LoadBoxErrorInfoModel
                {
                    DutIndex = dutIndex.ToString(),
                    Mode = label_status.Text,
                    ActuatorMotorState = _actMt.Value == 1 ? "ON" : "OFF",
                    CinchMotorState = _cinMt.Value == 1 ? "ON" : "OFF",
                    ReleaseMotorState = _rlsMt.Value == 1 ? "ON" : "OFF",
                    Detail = string.Empty,
                };

                var showCanErrorAction = new Action(() =>
                {
                    for (var i = 0; i < mainDgv.RowCount; i++)
                    {
                        if (label_status.Text == @"休眠模式")
                        {
                            mainDgv.Rows[i].Cells[cellIndex].Style.BackColor = Color.DarkGoldenrod;
                            mainDgv.Rows[i].Cells[cellIndex].Style.ForeColor = Color.WhiteSmoke;
                            mainDgv.Rows[i].Cells[cellIndex].Value = i == mainDgv.RowCount - 1 ? "无通信" : string.Empty;
                        }
                        else
                        {
                            mainDgv.Rows[i].Cells[cellIndex].Style.BackColor = Color.DarkRed;
                            mainDgv.Rows[i].Cells[cellIndex].Style.ForeColor = Color.WhiteSmoke;
                            mainDgv.Rows[i].Cells[cellIndex].Value = i == mainDgv.RowCount - 1 ? "异常" : string.Empty;
                        }
                    }

                    if (label_status.Text == @"休眠模式")
                    {
                        model.CheckResult = "OK";
                        model.CanCommunicationState = "无通信";
                    }
                    else
                    {
                        model.CheckResult = "NG";
                        model.CanCommunicationState = "异常";
                    }

                    if (btnLog.Text.Contains("停止") && model.CheckResult == "NG")
                        LoadBoxSqlHelper.SaveData(model);
                });

                if (pdmResponse1 == null || pdmResponse2 == null)
                {
                    showCanErrorAction();
                    return;
                }
                else
                {
                    var dateTimeNow = DateTime.Now;
                    var dt1 = ValueHelper.GetTimeSpanMs(pdmResponse1.DateTime, dateTimeNow);
                    var dt2 = ValueHelper.GetTimeSpanMs(pdmResponse2.DateTime, dateTimeNow);

                    var isCanNg = false;
                    var isCheckNg = 0;

                    if (dt1 > 1000 && dt2 > 1000)
                    {
                        _lossCanCount[dutIndex]++;
                        isCanNg = true;
                    }
                    else
                    {
                        _lossCanCount[dutIndex] = 0;

                        var isNg = false;

                        foreach (var t in _checkItems)
                        {
                            CanBus.CanDataPackage canDataPackage = null;

                            if (t.CanId == pdmResponse1.CanId)
                                canDataPackage = pdmResponse1;
                            else if (t.CanId == pdmResponse2.CanId)
                                canDataPackage = pdmResponse2;

                            if (canDataPackage == null)
                                continue;
                            bool isOk;
                            string showValue;

                            if (t.MatrixValDefinition.Len == 1)
                            {
                                var actualValue = (double)t.MatrixValDefinition.GetMotorolaSignalValue(canDataPackage.CanData);
                                actualValue = Math.Round(t.Factor * actualValue + t.Offset, 0, MidpointRounding.AwayFromZero);
                                showValue = actualValue.ToString(CultureInfo.InvariantCulture);
                                isOk = t.CheckInvoke(showValue);
                            }
                            else
                            {
                                var actualValue = (double)t.MatrixValDefinition.GetMotorolaSignalValue(canDataPackage.CanData);
                                actualValue = Math.Round(t.Factor * actualValue + t.Offset, 2, MidpointRounding.AwayFromZero);
                                showValue = actualValue.ToString(CultureInfo.InvariantCulture);
                                isOk = t.CheckInvoke(actualValue);
                            }

                            for (var i = 0; i < mainDgv.RowCount - 1; i++)
                            {
                                if (mainDgv.Rows[i].Cells[0].Value.ToString() != t.Name)
                                    continue;

                                if (!isOk)
                                    continue;
                                mainDgv.Rows[i].Cells[cellIndex].Value = showValue;
                                mainDgv.Rows[i].Cells[cellIndex].Style.BackColor = Color.Green;
                                mainDgv.Rows[i].Cells[cellIndex].Style.ForeColor = Color.WhiteSmoke;
                            }

                            mainDgv.Rows[mainDgv.RowCount - 1].Cells[cellIndex].Style.BackColor = Color.DarkGreen;
                            mainDgv.Rows[mainDgv.RowCount - 1].Cells[cellIndex].Style.ForeColor = Color.WhiteSmoke;
                            mainDgv.Rows[mainDgv.RowCount - 1].Cells[cellIndex].Value = "正常";

                            if (!isOk)
                            {
                                isNg = true;
                            }
                        }

                        if (isNg)
                        {
                            _checkNgCount[dutIndex]++;
                            isCheckNg++;
                        }
                        else
                        {
                            _checkNgCount[dutIndex] = 0;
                        }
                    }

                    if (isCanNg)
                    {
                        if (_lossCanCount[dutIndex] > 2)
                        {
                            _lossCanCount[dutIndex] = 0;
                            showCanErrorAction();
                        }
                        else
                        {
                            if (label_status.Text == @"休眠模式")
                            {
                                _lossCanCount[dutIndex] = 0;
                                showCanErrorAction();
                            }
                        }
                    }
                    else
                    {
                        if (isCheckNg == 0)
                        {
                            foreach (var t in _checkItems)
                            {
                                CanBus.CanDataPackage canDataPackage = null;

                                if (t.CanId == pdmResponse1.CanId)
                                    canDataPackage = pdmResponse1;
                                else if (t.CanId == pdmResponse2.CanId)
                                    canDataPackage = pdmResponse2;

                                if (canDataPackage == null)
                                    continue;
                                bool isOk;
                                string showValue;

                                if (t.MatrixValDefinition.Len == 1)
                                {
                                    var actualValue = (double)t.MatrixValDefinition.GetMotorolaSignalValue(canDataPackage.CanData);
                                    actualValue = Math.Round(t.Factor * actualValue + t.Offset, 0, MidpointRounding.AwayFromZero);
                                    showValue = actualValue.ToString(CultureInfo.InvariantCulture);
                                    isOk = t.CheckInvoke(showValue);
                                }
                                else
                                {
                                    var actualValue = (double)t.MatrixValDefinition.GetMotorolaSignalValue(canDataPackage.CanData);
                                    actualValue = Math.Round(t.Factor * actualValue + t.Offset, 2, MidpointRounding.AwayFromZero);
                                    showValue = actualValue.ToString(CultureInfo.InvariantCulture);
                                    isOk = t.CheckInvoke(actualValue);
                                }

                                for (var i = 0; i < mainDgv.RowCount - 1; i++)
                                {
                                    if (mainDgv.Rows[i].Cells[0].Value.ToString() != t.Name)
                                        continue;

                                    mainDgv.Rows[i].Cells[cellIndex].Value = showValue;
                                    mainDgv.Rows[i].Cells[cellIndex].Style.BackColor = isOk ? Color.Green : Color.Red;
                                    mainDgv.Rows[i].Cells[cellIndex].Style.ForeColor = Color.WhiteSmoke;
                                    break;
                                }

                                mainDgv.Rows[mainDgv.RowCount - 1].Cells[cellIndex].Style.BackColor = Color.DarkGreen;
                                mainDgv.Rows[mainDgv.RowCount - 1].Cells[cellIndex].Style.ForeColor = Color.WhiteSmoke;
                                mainDgv.Rows[mainDgv.RowCount - 1].Cells[cellIndex].Value = "正常";
                            }
                        }
                        else
                        {
                            if (_checkNgCount[dutIndex] > 2)
                            {
                                _checkNgCount[dutIndex] = 0;

                                model.CanCommunicationState = "正常";
                                var errorCount = 0;
                                var listCheckNameAndData = new List<SyProductionSaveCheckData.CheckDataDetail>();

                                foreach (var t in _checkItems)
                                {
                                    CanBus.CanDataPackage canDataPackage = null;

                                    if (t.CanId == pdmResponse1.CanId)
                                        canDataPackage = pdmResponse1;
                                    else if (t.CanId == pdmResponse2.CanId)
                                        canDataPackage = pdmResponse2;

                                    if (canDataPackage == null)
                                        continue;
                                    bool isOk;
                                    string showValue;

                                    if (t.MatrixValDefinition.Len == 1)
                                    {
                                        var actualValue = (double)t.MatrixValDefinition.GetMotorolaSignalValue(canDataPackage.CanData);
                                        actualValue = Math.Round(t.Factor * actualValue + t.Offset, 0, MidpointRounding.AwayFromZero);
                                        showValue = actualValue.ToString(CultureInfo.InvariantCulture);
                                        isOk = t.CheckInvoke(showValue);
                                    }
                                    else
                                    {
                                        var actualValue = (double)t.MatrixValDefinition.GetMotorolaSignalValue(canDataPackage.CanData);
                                        actualValue = Math.Round(t.Factor * actualValue + t.Offset, 2, MidpointRounding.AwayFromZero);
                                        showValue = actualValue.ToString(CultureInfo.InvariantCulture);
                                        isOk = t.CheckInvoke(actualValue);
                                    }

                                    for (var i = 0; i < mainDgv.RowCount - 1; i++)
                                    {
                                        if (mainDgv.Rows[i].Cells[0].Value.ToString() != t.Name)
                                            continue;

                                        listCheckNameAndData.Add(new SyProductionSaveCheckData.CheckDataDetail
                                        {
                                            ParaName = mainDgv.Rows[i].Cells[0].Value.ToString(),
                                            Range = mainDgv.Rows[i].Cells[1].Value.ToString(),
                                            Result = isOk ? "True" : "False",
                                            Type = mainDgv.Rows[i].Cells[2].Value.ToString(),
                                            Value = showValue
                                        });

                                        mainDgv.Rows[i].Cells[cellIndex].Value = showValue;
                                        mainDgv.Rows[i].Cells[cellIndex].Style.BackColor = isOk ? Color.Green : Color.Red;
                                        if (!isOk)
                                            errorCount++;
                                        mainDgv.Rows[i].Cells[cellIndex].Style.ForeColor = Color.WhiteSmoke;
                                        break;
                                    }

                                    mainDgv.Rows[mainDgv.RowCount - 1].Cells[cellIndex].Style.BackColor = Color.DarkGreen;
                                    mainDgv.Rows[mainDgv.RowCount - 1].Cells[cellIndex].Style.ForeColor = Color.WhiteSmoke;
                                    mainDgv.Rows[mainDgv.RowCount - 1].Cells[cellIndex].Value = "正常";
                                }

                                if (errorCount > 0)
                                {
                                    model.CheckResult = "NG";
                                }
                                else
                                {
                                    model.CheckResult = "OK";
                                }

                                if (btnLog.Text.Contains("停止") && model.CheckResult == "NG")
                                {
                                    model.Detail = JsonConvert.SerializeObject(listCheckNameAndData);
                                    LoadBoxSqlHelper.SaveData(model);
                                }
                            }
                        }
                    }

                }
            }
        }

        private delegate void UpdateDutSwitchStandardValueDelegate(string standardValue);
        private void UpdateDutSwitchStandardValue(string standardValue)
        {
            var updateDutSwitchStandardValueDelegate = new UpdateDutSwitchStandardValueDelegate(UpdateDutSwitchStandardValue);

            if (mainDgv.InvokeRequired)
            {
                Invoke(updateDutSwitchStandardValueDelegate, standardValue);
            }
            else
            {
                for (var i = 0; i < mainDgv.RowCount; i++)
                {
                    if ((string)mainDgv.Rows[i].Cells[0].Value == HallBDirection ||
                        (string)mainDgv.Rows[i].Cells[0].Value == FullSwitch ||
                        (string)mainDgv.Rows[i].Cells[0].Value == NeutSw)
                    {
                        mainDgv.Rows[i].Cells[1].Value = standardValue;
                    }
                }

                foreach (var t in _checkItems.Where(t => t.Name == HallBDirection ||
                                                         t.Name == FullSwitch ||
                                                         t.Name == NeutSw))
                    t.StandardValue = standardValue;
            }
        }

        #endregion

        #region 数据存储

        private void btnShowLog_Click(object sender, EventArgs e)
        {
            if (!btnLog.Text.Contains("停止"))
            {
                var value = string.Empty;
                if (this.InputPasswordDialog(ref value))
                {
                    if (!string.IsNullOrEmpty(value) && value == "098765")
                    {
                        var frm = new FrmLoadBoxLogData();
                        frm.ShowDialog();
                    }
                    else
                    {
                        this.ShowErrorTip("密码错误");
                    }
                }
                else
                {
                    this.ShowErrorTip("密码为空");
                }
            }
            else
            {
                this.ShowErrorTip("请先停止记录后再点击查看");
            }

        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            if (this.ShowAskDialog("是否确定？"))
            {
                if (btnLog.Text.Contains("开始"))
                {
                    btnLog.Text = @"停止记录";
                    BtnShowGray(btnLog);
                    lblLogState.BackColor = Color.Green;
                    lblLogState.Text = @"正在记录";
                }
                else
                {
                    btnLog.Text = @"开始记录";
                    BtnShowBlue(btnLog);
                    lblLogState.BackColor = Color.Red;
                    lblLogState.Text = @"停止记录";
                }
            }
        }

        #endregion
    }
}
