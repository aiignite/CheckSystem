using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using CommonUtility.BusLoader;
using HZH_Controls.IconFont;
using Sunny.UI;
using FontImages = HZH_Controls.IconFont.FontImages;

namespace CheckSystem.HelperForms.S12LHeadLamp
{
    public partial class S12LHighLevelHeadLamp : UIForm
    {
        private readonly Controller.S12LHighLevelHeadLamp _s12LHighLevelHeadLampController = new Controller.S12LHighLevelHeadLamp("S12L高配前灯");

        public S12LHighLevelHeadLamp(CanBus can1, CanBus can2, LinBus lin6)
        {
            InitializeComponent();
            Icon = FontImages.GetIcon(FontIcons.E_icon_lightbulb, 32,
                Color.DodgerBlue);

            if (can1 != null && can2 != null)
            {
                Closed += P12LHighLevelHeadLamp_Closed;

                uiRadioButton1.Checked = true;
                uiRadioButtonTiHolding.Checked = true;

                _s12LHighLevelHeadLampController.SignalLampCan1 = can1;
                _s12LHighLevelHeadLampController.LbHbCan2 = can2;
                _s12LHighLevelHeadLampController.IccLin6 = lin6;

                uiTitlePanel1.Style = UIStyle.LayuiGreen;
                uiSwitchCan.ActiveChanged += uiSwitchCan_ActiveChanged;
                uiSwitchSignalLampLeftRight.ActiveColor = Color.Green;
                uiSwitchSignalLampLeftRight.InActiveColor = Color.Blue;
                uiSwitchSignalLampLeftRight.ActiveChanged += uiSwitchSignalLampLeftRight_ActiveChanged;
                uiSwitchSideTurn.ActiveChanged += uiSwitchSideTurn_ActiveChanged;
                uiTrackBarDrl.ValueChanged += uiTrackBarDrl_ValueChanged;
                uiTrackBarPl.ValueChanged += uiTrackBarPl_ValueChanged;
                uiTrackBarTi.ValueChanged += uiTrackBarTi_ValueChanged;

                uiTrackBarLeftLb.ValueChanged += uiTrackBarLeftLb_ValueChanged;
                uiTrackBarLeftHb.ValueChanged += uiTrackBarLeftHb_ValueChanged;
                uiTrackBarRightLb.ValueChanged += uiTrackBarRightLb_ValueChanged;
                uiTrackBarRightgHb.ValueChanged += uiTrackBarRightgHb_ValueChanged;

                btnHdlmpLvlngReq.ValueChanged += btnHdlmpLvlngReq_ValueChanged;

                IiniSingleOnDgv();
                uiDataGridViewDrlPl.Enabled = false;
                uiDataGridViewTi.Enabled = false;
                uiSwitchSingleControl.ActiveChanged += uiSwitchSingleControl_ActiveChanged;
                uiDataGridViewDrlPl.CellEndEdit += uiDataGridViewDrlPl_CellEndEdit;
                uiDataGridViewTi.CellEndEdit += uiDataGridViewTi_CellEndEdit;

                var tiWorker = new BackgroundWorker();
                tiWorker.DoWork += _tiWorker_DoWork;
                tiWorker.RunWorkerCompleted += _tiWorker_RunWorkerCompleted;
                tiWorker.ProgressChanged += _tiWorker_ProgressChanged;
                tiWorker.WorkerReportsProgress = true;
                tiWorker.WorkerSupportsCancellation = true;
                tiWorker.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show(@"CAN卡未连接");
                Close();
            }
        }

        void _tiWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        void _tiWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private readonly Stopwatch _drlPlSt = new Stopwatch();
        private readonly Stopwatch _tiSt = new Stopwatch();
        private int _tiIndex;

        private void _tiWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;

            // Keep acquiring until we error or cancel
            while (!worker.CancellationPending)
            {
                try
                {
                    if (uiRadioButtonTiRuning.Checked)
                    {
                        // 共22颗
                        if (_tiSt.ElapsedMilliseconds >= 150)
                        {
                            _tiIndex++;
                            if (_tiIndex == 22)
                            {
                                _tiIndex = 0;
                                _s12LHighLevelHeadLampController.TiOff();
                            }
                            else
                            {
                                var ledIndex = uiDataGridViewTi.Rows[_tiIndex].Cells[0].Value.ToString().Replace("S-", "");
                                var gray = uiTrackBarTi.Value.ToString();
                                _s12LHighLevelHeadLampController.LedSingleOn(ledIndex, gray);
                            }

                            _tiSt.Restart();
                        }
                    }
                    else if (uiRadioButtonTiFlash.Checked)
                    {
                        if (_tiSt.ElapsedMilliseconds >= 400 && _tiSt.ElapsedMilliseconds < 800)
                        {
                            _s12LHighLevelHeadLampController.TiOn(uiTrackBarTi.Value.ToString());
                        }
                        else if (_tiSt.ElapsedMilliseconds >= 800)
                        {
                            _s12LHighLevelHeadLampController.TiOff();
                            _tiSt.Restart();
                        }
                    }

                    if (uiRadioButton2.Checked)
                    {
                        if (_drlPlSt.ElapsedMilliseconds >= 2000 && _drlPlSt.ElapsedMilliseconds < 4000)
                        {
                            _s12LHighLevelHeadLampController.DrlPlOn(uiTrackBarDrl.Value.ToString());
                        }
                        else if (_drlPlSt.ElapsedMilliseconds >= 4000)
                        {
                            _s12LHighLevelHeadLampController.DrlPlOn(uiTrackBarPl.Value.ToString());
                            _drlPlSt.Restart();
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

        private void uiDataGridViewTi_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var ledIndex = uiDataGridViewTi.Rows[e.RowIndex].Cells[0].Value.ToString().Replace("S-", "");
                var gray = uiDataGridViewTi.Rows[e.RowIndex].Cells[2].Value.ToString();
                _s12LHighLevelHeadLampController.LedSingleOn(ledIndex, gray);
            }
        }

        private void uiDataGridViewDrlPl_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var ledIndex = uiDataGridViewDrlPl.Rows[e.RowIndex].Cells[0].Value.ToString().Replace("S-", "");
                var gray = uiDataGridViewDrlPl.Rows[e.RowIndex].Cells[2].Value.ToString();
                _s12LHighLevelHeadLampController.LedSingleOn(ledIndex, gray);
            }
        }

        private void uiSwitchSingleControl_ActiveChanged(object sender, EventArgs e)
        {
            uiTrackBarDrl.Value = 0;
            uiTrackBarPl.Value = 0;
            uiTrackBarTi.Value = 0;
            uiRadioButtonTiHolding.Checked = true;
            uiRadioButton1.Checked = true;
            _tiIndex = 0;
            _s12LHighLevelHeadLampController.LedAllOff();
            IiniSingleOnDgv();

            if (uiSwitchSingleControl.Active)
            {
                uiDataGridViewDrlPl.Enabled = true;
                uiDataGridViewTi.Enabled = true;

                uiRadioButton1.Enabled = false;
                uiRadioButton2.Enabled = false;

                uiRadioButtonTiHolding.Enabled = false;
                uiRadioButtonTiFlash.Enabled = false;
                uiRadioButtonTiRuning.Enabled = false;

                uiTrackBarDrl.Enabled = false;
                uiTrackBarPl.Enabled = false;
                uiTrackBarTi.Enabled = false;
            }
            else
            {
                uiDataGridViewDrlPl.Enabled = false;
                uiDataGridViewTi.Enabled = false;

                uiRadioButton1.Enabled = true;
                uiRadioButton2.Enabled = true;

                uiTrackBarDrl.Enabled = true;
                uiTrackBarPl.Enabled = true;
                uiTrackBarTi.Enabled = true;

                uiRadioButtonTiHolding.Enabled = true;
                uiRadioButtonTiFlash.Enabled = true;
                uiRadioButtonTiRuning.Enabled = true;
            }
        }

        private void IiniSingleOnDgv()
        {
            uiTitlePanel2.Style = UIStyle.DarkBlue;
            uiTitlePanel3.Style = UIStyle.Black;

            uiDataGridViewDrlPl.ClearAll();
            uiDataGridViewTi.ClearAll();

            uiDataGridViewDrlPl.RowHeadersVisible = false;
            uiDataGridViewDrlPl.AllowUserToAddRows = false;
            uiDataGridViewDrlPl.AllowUserToResizeRows = false;
            uiDataGridViewDrlPl.AllowUserToOrderColumns = false;
            uiDataGridViewDrlPl.MultiSelect = true;
            uiDataGridViewDrlPl.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            uiDataGridViewDrlPl.AddColumn("有效编号", "name");
            uiDataGridViewDrlPl.AddColumn("同时点亮编号", "name");
            uiDataGridViewDrlPl.AddColumn("灰度值", "gray");
            //uiDataGridViewDrlPl.AddRow("S-" + 1, "S-" + 2, 0);
            //uiDataGridViewDrlPl.AddRow("S-" + 3, "S-" + 4, 0);
            //uiDataGridViewDrlPl.AddRow("S-" + 5, "S-" + 6, 0);
            for (var i = 1; i <= 6; i = i + 2)
                uiDataGridViewDrlPl.AddRow("S-" + i, "S-" + (i + 1), 0);

            uiDataGridViewDrlPl.AddRow("S-" + 7, "S-" + 7, 0);

            for (var i = 8; i <= 35; i = i + 2)
                uiDataGridViewDrlPl.AddRow("S-" + i, "S-" + (i + 1), 0);

            uiDataGridViewDrlPl.AddRow("S-" + 36, "S-" + 53, 0);

            for (var i = 38; i <= 52; i = i + 4)
                uiDataGridViewDrlPl.AddRow("S-" + i, "S-" + (i + 2), 0);

            for (var i = 54; i <= 69; i = i + 2)
                uiDataGridViewDrlPl.AddRow("S-" + i, "S-" + (i + 1), 0);

            uiDataGridViewDrlPl.AddRow("S-" + 70, "S-" + 70, 0);

            uiDataGridViewDrlPl.Columns[2].ReadOnly = false;

            uiDataGridViewTi.RowHeadersVisible = false;
            uiDataGridViewTi.AllowUserToAddRows = false;
            uiDataGridViewTi.AllowUserToResizeRows = false;
            uiDataGridViewTi.AllowUserToOrderColumns = false;
            uiDataGridViewTi.MultiSelect = true;
            uiDataGridViewTi.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            uiDataGridViewTi.AddColumn("有效编号", "name");
            uiDataGridViewTi.AddColumn("同时点亮编号", "name");
            uiDataGridViewTi.AddColumn("灰度值", "gray");
            uiDataGridViewTi.AddRow("S-" + 0, "S-" + 0, 0);
            for (var i = 37; i <= 51; i = i + 4)
                uiDataGridViewTi.AddRow("S-" + i, "S-" + (i + 2), 0);
            uiDataGridViewTi.Columns[2].ReadOnly = false;
        }

        private void uiTrackBarTi_ValueChanged(object sender, int value)
        {
            uiMarkLabelTi.Text = uiTrackBarTi.Value + @"/灰度值";

            if (uiTrackBarTi.Value == 0)
                _s12LHighLevelHeadLampController.TiOff();
            else
                _s12LHighLevelHeadLampController.TiOn(uiTrackBarTi.Value.ToString());
        }

        private void uiTrackBarDrl_ValueChanged(object sender, int value)
        {
            uiMarkLabelDrl.Text = uiTrackBarDrl.Value + @"/灰度值";

            if (uiTrackBarDrl.Value == 0)
                _s12LHighLevelHeadLampController.DrlPlOff();
            else
                _s12LHighLevelHeadLampController.DrlPlOn(uiTrackBarDrl.Value.ToString());
        }

        private void uiTrackBarPl_ValueChanged(object sender, int value)
        {
            uiMarkLabelPl.Text = uiTrackBarPl.Value + @"/灰度值";

            if (uiTrackBarPl.Value == 0)
                _s12LHighLevelHeadLampController.DrlPlOff();
            else
                _s12LHighLevelHeadLampController.DrlPlOn(uiTrackBarPl.Value.ToString());
        }

        private void uiSwitchSideTurn_ActiveChanged(object sender, EventArgs e)
        {
            if (uiSwitchSingleControl.Active)
                _s12LHighLevelHeadLampController.SideTurnOn();
            else
                _s12LHighLevelHeadLampController.SideTurnOff();
        }

        private void uiSwitchSignalLampLeftRight_ActiveChanged(object sender, EventArgs e)
        {
            _s12LHighLevelHeadLampController.IsLeftLamp = uiSwitchSignalLampLeftRight.Active;
            uiTrackBarDrl.Value = 0;
            uiTrackBarPl.Value = 0;
            uiTrackBarTi.Value = 0;
        }

        private void P12LHighLevelHeadLamp_Closed(object sender, EventArgs e)
        {
            if (_s12LHighLevelHeadLampController != null)
            {
                _s12LHighLevelHeadLampController.Dispose();
            }
        }

        private void uiTrackBarRightgHb_ValueChanged(object sender, int value)
        {
            uiMarkLabelRightHb.Text = uiTrackBarRightgHb.Value + @"/%";
            _s12LHighLevelHeadLampController.RightHbLedGray = uiTrackBarRightgHb.Value.ToString();
        }

        private void uiTrackBarRightLb_ValueChanged(object sender, int value)
        {
            uiMarkLabelRightLb.Text = uiTrackBarRightLb.Value + @"/%";
            _s12LHighLevelHeadLampController.RightLbLedGray = uiTrackBarRightLb.Value.ToString();
        }

        private void uiTrackBarLeftHb_ValueChanged(object sender, int value)
        {
            uiMarkLabelLeftHB.Text = uiTrackBarLeftHb.Value + @"/%";
            _s12LHighLevelHeadLampController.LeftHbLedGray = uiTrackBarLeftHb.Value.ToString();
        }

        private void uiTrackBarLeftLb_ValueChanged(object sender, int value)
        {
            uiMarkLabelLeftLb.Text = uiTrackBarLeftLb.Value + @"%";
            _s12LHighLevelHeadLampController.LeftLbLedGray = uiTrackBarLeftLb.Value.ToString();
        }

        private void uiSwitchCan_ActiveChanged(object sender, EventArgs e)
        {
            if (uiSwitchCan.Active)
                _s12LHighLevelHeadLampController.StartCanLinMsg();
            else
                _s12LHighLevelHeadLampController.StopCanLinMsg();
        }

        /// <summary>
        /// 闪烁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiRadioButtonTiFlash_CheckedChanged(object sender, EventArgs e)
        {
            if (uiRadioButtonTiFlash.Checked)
            {
                if (uiTrackBarTi.Value == 0)
                    uiTrackBarTi.Value = 255;
                _s12LHighLevelHeadLampController.TiOff();
                uiTrackBarTi.Enabled = false;
                _tiIndex = 0;
                _tiSt.Start();
            }
        }

        /// <summary>
        /// 流水
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiRadioButtonTiRuning_CheckedChanged(object sender, EventArgs e)
        {
            if (uiRadioButtonTiRuning.Checked)
            {
                if (uiTrackBarTi.Value == 0)
                    uiTrackBarTi.Value = 255;
                _s12LHighLevelHeadLampController.TiOff();
                uiTrackBarTi.Enabled = false;
                _tiIndex = 0;
                _tiSt.Start();
            }
        }

        private void uiRadioButtonTiHolding_CheckedChanged(object sender, EventArgs e)
        {
            if (uiRadioButtonTiHolding.Checked)
            {
                _s12LHighLevelHeadLampController.TiOn(uiTrackBarTi.Value.ToString());
                uiTrackBarTi.Enabled = true;
                _tiSt.Stop();
                _tiIndex = 0;
            }
        }

        private void uiRadioButton1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (uiRadioButton1.Checked)
            {
                _s12LHighLevelHeadLampController.DrlPlOn(uiTrackBarDrl.Value.ToString());
                uiTrackBarDrl.Enabled = true;
                uiTrackBarPl.Enabled = true;
                _drlPlSt.Stop();
            }
        }

        private void uiRadioButton2_CheckedChanged_1(object sender, EventArgs e)
        {
            if (uiRadioButton2.Checked)
            {
                if (uiTrackBarDrl.Value == 0)
                {
                    uiTrackBarDrl.Value = 255;
                }

                if (uiTrackBarPl.Value == 0)
                {
                    uiTrackBarPl.Value = 38;
                }
                _s12LHighLevelHeadLampController.DrlPlOn(uiTrackBarPl.Value.ToString());
                uiTrackBarDrl.Enabled = false;
                uiTrackBarPl.Enabled = false;
                _drlPlSt.Start();
            }
        }

        public void btnHdlmpLvlngReq_ValueChanged(object sender, int value)
        {
            _s12LHighLevelHeadLampController.HdlmpLvlngReq(value.ToString());
        }
    }
}
