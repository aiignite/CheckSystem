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
using Sunny.UI;
using FontImages = HZH_Controls.IconFont.FontImages;

namespace CheckSystem.HelperForms.Sgm458Pdm
{
    public partial class FrmLoadBox1BoardUsing2Can : UIForm
    {
        //private Dictionary<int,> 
        private readonly List<CheckItem> _checkItems = new List<CheckItem>();

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
        private bool RelayFlag { get; set; }

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

        public FrmLoadBox1BoardUsing2Can()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            Icon = FontImages.GetIcon(
                HZH_Controls.FontIcons.E_social_googledrive, 32, Color.DodgerBlue);
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
            _linController.Dispose();

            for (var i = 1; i <= 6; i++)
                _dutControllers[i].Dispose();
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
                var showCanErrorAction = new Action(() =>
                {
                    for (var i = 0; i < mainDgv.RowCount; i++)
                    {
                        mainDgv.Rows[i].Cells[cellIndex].Style.BackColor = Color.DarkRed;
                        mainDgv.Rows[i].Cells[cellIndex].Style.ForeColor = Color.WhiteSmoke;
                        mainDgv.Rows[i].Cells[cellIndex].Value = i == mainDgv.RowCount - 1 ? "异常" : string.Empty;
                    }
                });

                if (pdmResponse1 == null || pdmResponse2 == null)
                {
                    showCanErrorAction();
                }
                else
                {
                    var dateTimeNow = DateTime.Now;
                    var dt1 = ValueHelper.GetTimeSpanMs(pdmResponse1.DateTime, dateTimeNow);
                    var dt2 = ValueHelper.GetTimeSpanMs(pdmResponse2.DateTime, dateTimeNow);
                    if (dt1 > 250 || dt2 > 250)
                    {
                        showCanErrorAction();
                    }
                    else
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

        private void InitCheckItems()
        {
            _checkItems.Clear();

            _checkItems.Add(new CheckItem(Vbatt1, 9, 16, "V", 0x201, 14, 10, 0.03125, 0));
            _checkItems.Add(new CheckItem(Vbatt2, 9, 16, "V", 0x201, 20, 10, 0.03125, 0));
            _checkItems.Add(new CheckItem(Vcc2_5V, 4.5, 5.5, "V", 0x201, 28, 8, 0.03125, 0));
            _checkItems.Add(new CheckItem(HallFeed, 9, 16, "V", 0x0201, 40, 10, 0.03125, 0));
            _checkItems.Add(new CheckItem(HallASpeed, 80, 120, "HZ", 0x202, 8, 16, 1, 0));
            _checkItems.Add(new CheckItem(HallBDirection, "1", 0x202, 23, 1, 1, 0));
            _checkItems.Add(new CheckItem(ActuatorCurrent, 0, 15, "A", 0x202, 41, 10, 0.03125, 0));
            _checkItems.Add(new CheckItem(CinchCurrent, 0, 8, "A", 0x202, 35, 10, 0.03125, 0));
            _checkItems.Add(new CheckItem(ReleaseCurrent, 0, 8, "A", 0x202, 63, 10, 0.03125, 0));
            _checkItems.Add(new CheckItem(FullSwitch, "1", 0x201, 61, 1, 1, 0));
            _checkItems.Add(new CheckItem(NeutSw, "1", 0x201, 60, 1, 1, 0));
            _checkItems.Add(new CheckItem(HalfSwitch, 2.5, 12.8, "V", 0x202, 29, 10, 0.03125, 0));
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
            _linController.InitRemoteIpAddress("192.168.1.34:8088");

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
            }
        }

        private void InitThread()
        {
            if (LinMsgThread != null)
            {
                LinMsgThread.Abort();
                LinMsgThread.Join();
            }

            LinMsgThread = new Thread(LinMsgWork) { IsBackground = true };
            LinMsgThread.Start();

            RefreshBackgroundWorker = new BackgroundWorker();
            RefreshBackgroundWorker.DoWork += RefreshBackgroundWorker_DoWork;
            RefreshBackgroundWorker.WorkerReportsProgress = true;
            RefreshBackgroundWorker.WorkerSupportsCancellation = true;
            RefreshBackgroundWorker.RunWorkerAsync();
        }

        private void RefreshBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;
            var relayFlagTemp = RelayFlag;

            // Keep acquiring until we error or cancel
            while (!worker.CancellationPending)
            {
                try
                {
                    mainDgv.ClearSelection();

                    if (relayFlagTemp != RelayFlag)
                    {
                        relayFlagTemp = RelayFlag;
                        var acion = new Action(() =>
                        {
                            for (var i = 1; i <= 6; i++)
                            {
                                _linController.Relay1 = RelayFlag;
                                _linController.Relay2 = RelayFlag;
                                _linController.Relay3 = RelayFlag;
                                _linController.Relay4 = RelayFlag;
                                _linController.Relay5 = RelayFlag;
                                _linController.Relay6 = RelayFlag;
                                _linController.Relay7 = RelayFlag;
                                _linController.Relay8 = RelayFlag;
                                _linController.Relay9 = RelayFlag;
                                _linController.Relay10 = RelayFlag;
                                _linController.Relay11 = RelayFlag;
                                _linController.Relay12 = RelayFlag;
                                if (!IsSleep)
                                    _linController.SetOutputs();
                            }

                            UpdateDutSwitchStandardValue(RelayFlag ? "0" : "1");
                        });
                        //acion.BeginInvoke(null, null);
                        //acion.Invoke();
                    }

                    for (var i = 1; i <= 6; i++)
                    {
                        UpdateDutDgv(i, _pdmResponse1_0X201[i], _pdmResponse2_0X202[i]);

                        //if (_pdmResponse1_0X201[i] != null &&
                        //    _pdmResponse2_0X202[i] != null)
                        //{
                        //    UpdateDutDgv(i, _pdmResponse1_0X201[i], _pdmResponse2_0X202[i]);
                        //}
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

        private void tsBtn_Click(object sender, EventArgs e)
        {
            var tbBtn = sender as ToolStripButton;
            if (tbBtn == null) return;
            tbBtn.BackColor = tbBtn.BackColor == Color.Green ? Color.Gray : Color.Green;
        }

        private void SwitchSwitchRelayTimerTimer_Tick(object sender, EventArgs e)
        {
            if (IsSleep)
            {
                RelayFlag = false;
            }
            else
            {
                RelayFlag = !RelayFlag;
            }
        }

        #region LIN控制帧

        private Thread LinMsgThread { get; set; }
        private bool IsSleep { get; set; }

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

        private void LinMsgWork()
        {
            while (LinMsgThread.IsAlive)
            {
                if (!LinMsgThread.IsAlive)
                    break;

                if (_linController == null || _linController.GatewayLin == null)
                    continue;

                if (IsSleep)
                    continue;

                Thread.Sleep(10);

                _pdmCtrlCmd.UpdateData(_runMd);
                _pdmCtrlCmd.UpdateData(_actMt);
                _pdmCtrlCmd.UpdateData(_cinMt);
                _pdmCtrlCmd.UpdateData(_rlsMt);
                _linController.GatewayLin.SendMasterLin(_pdmCtrlCmd.MasterLinId, _pdmCtrlCmd.MatrixData);
            }
        }

        private void btnMode1_Click(object sender, EventArgs e)
        {
            BtnShowBlue(btnMode1);
            BtnShowGray(btnMode2);
            BtnShowGray(btnMode4);

            _runMd.Value = 1;
            IsSleep = false;
            label_status.Text = @"正常模式";

            GoToMode100();
        }

        private void btnMode2_Click(object sender, EventArgs e)
        {
            BtnShowBlue(btnMode2);
            BtnShowGray(btnMode1);
            BtnShowGray(btnMode4);

            _runMd.Value = 2;
            IsSleep = false;
            label_status.Text = @"待机模式";
        }

        private void btnMode4_Click(object sender, EventArgs e)
        {
            BtnShowBlue(btnMode4);
            BtnShowGray(btnMode1);
            BtnShowGray(btnMode2);

            _runMd.Value = 4;
            IsSleep = true;
            label_status.Text = @"休眠模式";

            GoToSleep();
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

        #endregion

        private void btnStartModeSwitch_Click(object sender, EventArgs e)
        {
            if (btnStartModeSwitch.Text.Contains("开始"))
            {
                btnMode1.PerformClick();
                TimePer100MsCount = 0;
                lblMode1RemainTime.BackColor = Color.Green;
                lblMode4RemainTime.BackColor = Color.Gray;

                txtMode1RemainTime.IntValue = txtMode1KeepTime.IntValue;
                txtMode4RemainTime.IntValue = txtMode4KeepTime.IntValue;
                Mode1RemainTime = txtMode1KeepTime.IntValue;
                Mode4RemainTime = txtMode4KeepTime.IntValue;

                SwitchModeTimer.Enabled = true;

                btnMode1.Enabled = false;
                btnMode2.Enabled = false;
                btnMode4.Enabled = false;
                btnUpdateSwitchTime.Enabled = true;
                btnStartModeSwitch.Text = @"停止";
            }
            else
            {
                btnMode1.Enabled = true;
                btnMode2.Enabled = true;
                btnMode4.Enabled = true;
                btnUpdateSwitchTime.Enabled = false;
                btnStartModeSwitch.Text = @"开始";

                SwitchModeTimer.Enabled = false;
                TimePer100MsCount = 0;
                btnMode4.PerformClick();
                lblMode1RemainTime.BackColor = Color.Gray;
                lblMode4RemainTime.BackColor = Color.Gray;
            }
        }

        private void btnUpdateSwitchTime_Click(object sender, EventArgs e)
        {
            if (ShowAskDialog("是否确定更新？"))
            {
                txtMode1RemainTime.IntValue = txtMode1KeepTime.IntValue;
                txtMode4RemainTime.IntValue = txtMode4KeepTime.IntValue;

                Mode1RemainTime = txtMode1KeepTime.IntValue;
                Mode4RemainTime = txtMode4KeepTime.IntValue;

                if (!btnStartModeSwitch.Text.Contains("开始"))
                {
                    SwitchModeTimer.Enabled = false;
                    TimePer100MsCount = 0;
                    lblMode1RemainTime.BackColor = Color.Green;
                    lblMode4RemainTime.BackColor = Color.Gray;

                    _runMd.Value = 1;
                    IsSleep = false;
                    label_status.Text = @"正常模式";
                    GoToMode100();

                    SwitchModeTimer.Enabled = true;
                }
            }
            else
            {
                ShowInfoTip("取消了操作");
            }
        }

        private int TimePer100MsCount { get; set; }
        private int Mode1RemainTime { get; set; }
        private int Mode4RemainTime { get; set; }

        private void SwitchModeTimer_Tick(object sender, EventArgs e)
        {
            TimePer100MsCount++;
            Console.WriteLine(TimePer100MsCount);
            if (TimePer100MsCount * 100 <= Mode1RemainTime * 1000)
            {
                if (TimePer100MsCount * 100 % 1000 == 0)
                    txtMode1RemainTime.IntValue = txtMode1RemainTime.IntValue - 1;

                if (TimePer100MsCount * 100 == Mode1RemainTime * 1000)
                {
                    IsSleep = true;
                    _runMd.Value = 4;
                    label_status.Text = @"休眠模式";
                    GoToSleep();

                    lblMode1RemainTime.BackColor = Color.Gray;
                    lblMode4RemainTime.BackColor = Color.Green;
                }
            }
            else if (TimePer100MsCount * 100 <= Mode1RemainTime * 1000 + Mode4RemainTime * 1000)
            {
                if (TimePer100MsCount * 100 % 1000 == 0)
                    txtMode4RemainTime.IntValue = txtMode4RemainTime.IntValue - 1;

                if (TimePer100MsCount * 100 == Mode1RemainTime * 1000 + Mode4RemainTime * 1000)
                {
                    TimePer100MsCount = 0;

                    lblMode1RemainTime.BackColor = Color.Green;
                    lblMode4RemainTime.BackColor = Color.Gray;

                    txtMode1RemainTime.IntValue = Mode1RemainTime;
                    txtMode4RemainTime.IntValue = Mode4RemainTime;

                    _runMd.Value = 1;
                    label_status.Text = @"正常模式";
                    IsSleep = false;
                    GoToMode100();
                }
            }
        }

        private void GoToSleep()
        {
            for (var i = 1; i <= 6; i++)
            {
                _dutControllers[i].Relay1 = false;
                _dutControllers[i].Relay2 = false;
                _dutControllers[i].Relay3 = false;
                _dutControllers[i].Relay4 = false;
                _dutControllers[i].Relay5 = false;
                _dutControllers[i].Relay6 = false;
                _dutControllers[i].Relay7 = false;
                _dutControllers[i].Relay8 = false;
                _dutControllers[i].Relay9 = false;
                _dutControllers[i].Relay10 = false;
                _dutControllers[i].Relay11 = false;
                _dutControllers[i].Relay12 = false;
                _dutControllers[i].SetOutputs();
            }

            if (_rlsMt.Value == 1)
            {
                _rlsMt.Value = 0x00;
                BtnShowGray(btnReleaseMotor as UIButton);
            }

            if (_cinMt.Value == 1)
            {
                _cinMt.Value = 0x00;
                BtnShowGray(btnCinchMotor as UIButton);
            }

            if (_actMt.Value == 1)
            {
                _actMt.Value = 0x00;
                BtnShowGray(btnActuatorMotor as UIButton);
            }

            for (var i = 0; i < 5; i++)
            {
                _pdmCtrlCmd.UpdateData(_runMd);
                _pdmCtrlCmd.UpdateData(_actMt);
                _pdmCtrlCmd.UpdateData(_cinMt);
                _pdmCtrlCmd.UpdateData(_rlsMt);
                _linController.GatewayLin.SendMasterLin(_pdmCtrlCmd.MasterLinId, _pdmCtrlCmd.MatrixData);
            }
        }

        private void GoToMode100()
        {
            if (_rlsMt.Value == 0)
            {
                _rlsMt.Value = 0x01;
                BtnShowBlue(btnReleaseMotor as UIButton);
            }

            if (_cinMt.Value == 0)
            {
                _cinMt.Value = 0x01;
                BtnShowBlue(btnCinchMotor as UIButton);
            }

            if (_actMt.Value == 0)
            {
                _actMt.Value = 0x01;
                BtnShowBlue(btnActuatorMotor as UIButton);
            }
        }
    }
}
