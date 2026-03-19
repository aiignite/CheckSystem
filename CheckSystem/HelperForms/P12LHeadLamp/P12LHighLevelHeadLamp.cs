using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using CommonUtility.BusLoader;
using HZH_Controls.IconFont;
using Sunny.UI;
using FontImages = HZH_Controls.IconFont.FontImages;

namespace CheckSystem.HelperForms.P12LHeadLamp
{
    public sealed partial class P12LHighLevelHeadLamp : UIForm
    {
        private readonly Controller.ControllerBase _p12LHighLevelHeadLampController;// = new Controller.P12LHighLevelHeadLamp("P12L高配前灯");
        private bool _isHighLevel;

        public P12LHighLevelHeadLamp(CanBus can1, CanBus can2, LinBus lin6, bool isHighLevel)
        {
            InitializeComponent();
            Icon = FontImages.GetIcon(FontIcons.E_icon_lightbulb, 32,
                Color.DodgerBlue);
            _isHighLevel = isHighLevel;

            if (can1 != null && can2 != null && isHighLevel)
            {
                _p12LHighLevelHeadLampController = new Controller.P12LHighLevelHeadLamp("P12L高配前灯");
                Text = _p12LHighLevelHeadLampController.Name;

                Closed += P12LHighLevelHeadLamp_Closed;

                uiRadioButton1.Checked = true;
                uiRadioButtonTiHolding.Checked = true;

                ((Controller.P12LHighLevelHeadLamp)_p12LHighLevelHeadLampController).SignalLampCan1 = can1;
                ((Controller.P12LHighLevelHeadLamp)_p12LHighLevelHeadLampController).LbHbCan2 = can2;
                ((Controller.P12LHighLevelHeadLamp)_p12LHighLevelHeadLampController).IccLin6 = lin6;

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
            else if (can1 != null && !isHighLevel)
            {
                _p12LHighLevelHeadLampController = new Controller.P12LHeadLamp("P12L前灯-融合方案");
                Text = _p12LHighLevelHeadLampController.Name;

                Closed += P12LHighLevelHeadLamp_Closed;

                uiRadioButton1.Checked = true;
                uiRadioButtonTiHolding.Checked = true;

                ((Controller.P12LHeadLamp)_p12LHighLevelHeadLampController).Can = can1;

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
                        if (_tiSt.ElapsedMilliseconds >= 25)
                        {
                            _tiIndex++;
                            if (_tiIndex == 22)
                            {
                                _tiIndex = -1;
                                _p12LHighLevelHeadLampController.InvokeFuncByName("TiOff", null);
                            }
                            else
                            {
                                var ledIndex = uiDataGridViewTi.Rows[_tiIndex].Cells[0].Value.ToString().Replace("S-", "");
                                if (!_isHighLevel)
                                    ledIndex = (_p12lMergeTiIndex[int.Parse(ledIndex)] - 1).ToString();
                                var gray = uiTrackBarTi.Value.ToString();
                                _p12LHighLevelHeadLampController.InvokeFuncByName(
                                    _isHighLevel ? "LedSingleOn" : "YLedSingleOn", new object[] { ledIndex, gray });
                            }

                            _tiSt.Restart();
                        }
                    }
                    else if (uiRadioButtonTiFlash.Checked)
                    {
                        if (_tiSt.ElapsedMilliseconds >= 400 && _tiSt.ElapsedMilliseconds < 800)
                        {
                            _p12LHighLevelHeadLampController.InvokeFuncByName("TiOn",
                                new object[] { uiTrackBarTi.Value.ToString() });
                        }
                        else if (_tiSt.ElapsedMilliseconds >= 800)
                        {
                            _p12LHighLevelHeadLampController.InvokeFuncByName("TiOff", null);
                            _tiSt.Restart();
                        }
                    }

                    if (uiRadioButton2.Checked)
                    {
                        if (_drlPlSt.ElapsedMilliseconds >= 2000 && _drlPlSt.ElapsedMilliseconds < 4000)
                        {
                            _p12LHighLevelHeadLampController.InvokeFuncByName("DrlPlOn",
                                new object[] { uiTrackBarDrl.Value.ToString() });
                        }
                        else if (_drlPlSt.ElapsedMilliseconds >= 4000)
                        {
                            _p12LHighLevelHeadLampController.InvokeFuncByName("DrlPlOn",
                               new object[] { uiTrackBarPl.Value.ToString() });
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

                if (!_isHighLevel)
                    ledIndex = (_p12lMergeTiIndex[int.Parse(ledIndex)] - 1).ToString();

                _p12LHighLevelHeadLampController.InvokeFuncByName(_isHighLevel ? "LedSingleOn" : "YLedSingleOn",
                    new object[] { ledIndex, gray });
            }
        }

        private void uiDataGridViewDrlPl_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var ledIndex = uiDataGridViewDrlPl.Rows[e.RowIndex].Cells[0].Value.ToString().Replace("S-", "");
                var gray = uiDataGridViewDrlPl.Rows[e.RowIndex].Cells[2].Value.ToString();

                if (!_isHighLevel)
                    ledIndex = (_p12lMergeDrlIndex[int.Parse(ledIndex)] - 1).ToString();

                _p12LHighLevelHeadLampController.InvokeFuncByName(_isHighLevel ? "LedSingleOn" : "WLedSingleOn",
                   new object[] { ledIndex, gray });
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
            _p12LHighLevelHeadLampController.InvokeFuncByName("LedAllOff", null);
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
            uiDataGridViewDrlPl.AddRow("S-" + 1, "S-" + 2, 0);
            uiDataGridViewDrlPl.AddRow("S-" + 3, "S-" + 4, 0);
            uiDataGridViewDrlPl.AddRow("S-" + 5, "S-" + 6, 0);
            for (var i = 8; i <= 92; i = i + 4)
                uiDataGridViewDrlPl.AddRow("S-" + i, "S-" + (i + 2), 0);
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
            for (var i = 7; i <= 91; i = i + 4)
                uiDataGridViewTi.AddRow("S-" + i, "S-" + (i + 2), 0);
            uiDataGridViewTi.Columns[2].ReadOnly = false;
        }

        private void uiTrackBarTi_ValueChanged(object sender, int value)
        {
            uiMarkLabelTi.Text = uiTrackBarTi.Value + @"/灰度值";

            if (uiTrackBarTi.Value == 0)
                _p12LHighLevelHeadLampController.InvokeFuncByName("TiOff", null);
            else
                _p12LHighLevelHeadLampController.InvokeFuncByName("TiOn", new object[] { uiTrackBarTi.Value.ToString() });
        }

        private void uiTrackBarDrl_ValueChanged(object sender, int value)
        {
            uiMarkLabelDrl.Text = uiTrackBarDrl.Value + @"/灰度值";

            if (uiTrackBarDrl.Value == 0)
                _p12LHighLevelHeadLampController.InvokeFuncByName("DrlPlOff", null);
            else
                _p12LHighLevelHeadLampController.InvokeFuncByName("DrlPlOn", new object[] { uiTrackBarDrl.Value.ToString() });
        }

        private void uiTrackBarPl_ValueChanged(object sender, int value)
        {
            uiMarkLabelPl.Text = uiTrackBarPl.Value + @"/灰度值";

            if (uiTrackBarPl.Value == 0)
                _p12LHighLevelHeadLampController.InvokeFuncByName("DrlPlOff", null);
            else
                _p12LHighLevelHeadLampController.InvokeFuncByName("DrlPlOn",
                    new object[] { uiTrackBarPl.Value.ToString() });
        }

        private void uiSwitchSideTurn_ActiveChanged(object sender, EventArgs e)
        {
            if (uiSwitchSingleControl.Active)
                _p12LHighLevelHeadLampController.InvokeFuncByName("SideTurnOn", null);
            else
                _p12LHighLevelHeadLampController.InvokeFuncByName("SideTurnOff", null);
        }

        private void uiSwitchSignalLampLeftRight_ActiveChanged(object sender, EventArgs e)
        {
            if (_isHighLevel)
                ((Controller.P12LHighLevelHeadLamp)_p12LHighLevelHeadLampController).IsLeftLamp = uiSwitchSignalLampLeftRight.Active;
            else
                ((Controller.P12LHeadLamp)_p12LHighLevelHeadLampController).IsLeftLamp = uiSwitchSignalLampLeftRight.Active;
            uiTrackBarDrl.Value = 0;
            uiTrackBarPl.Value = 0;
            uiTrackBarTi.Value = 0;
        }

        private void P12LHighLevelHeadLamp_Closed(object sender, EventArgs e)
        {
            if (_p12LHighLevelHeadLampController != null)
            {
                _p12LHighLevelHeadLampController.Dispose();
            }
        }

        private void uiTrackBarRightgHb_ValueChanged(object sender, int value)
        {
            uiMarkLabelRightHb.Text = uiTrackBarRightgHb.Value + @"/%";
            if (_isHighLevel)
                ((Controller.P12LHighLevelHeadLamp)_p12LHighLevelHeadLampController).RightHbLedGray = uiTrackBarRightgHb.Value.ToString();
            else
                ((Controller.P12LHeadLamp)_p12LHighLevelHeadLampController).RightHbLedGray = uiTrackBarRightgHb.Value.ToString();
        }

        private void uiTrackBarRightLb_ValueChanged(object sender, int value)
        {
            uiMarkLabelRightLb.Text = uiTrackBarRightLb.Value + @"/%";
            if (_isHighLevel)
                ((Controller.P12LHighLevelHeadLamp)_p12LHighLevelHeadLampController).RightLbLedGray = uiTrackBarRightLb.Value.ToString();
            else
                ((Controller.P12LHeadLamp)_p12LHighLevelHeadLampController).RightLbLedGray = uiTrackBarRightLb.Value.ToString();
        }

        private void uiTrackBarLeftHb_ValueChanged(object sender, int value)
        {
            uiMarkLabelLeftHB.Text = uiTrackBarLeftHb.Value + @"/%";
            if (_isHighLevel)
                ((Controller.P12LHighLevelHeadLamp)_p12LHighLevelHeadLampController).LeftHbLedGray = uiTrackBarLeftHb.Value.ToString();
            else
                ((Controller.P12LHeadLamp)_p12LHighLevelHeadLampController).LeftHbLedGray = uiTrackBarLeftHb.Value.ToString();
        }

        private void uiTrackBarLeftLb_ValueChanged(object sender, int value)
        {
            uiMarkLabelLeftLb.Text = uiTrackBarLeftLb.Value + @"%";
            if (_isHighLevel)
                ((Controller.P12LHighLevelHeadLamp)_p12LHighLevelHeadLampController).LeftLbLedGray = uiTrackBarLeftLb.Value.ToString();
            else
                ((Controller.P12LHeadLamp)_p12LHighLevelHeadLampController).LeftLbLedGray = uiTrackBarLeftLb.Value.ToString();
        }

        private void uiSwitchCan_ActiveChanged(object sender, EventArgs e)
        {
            if (uiSwitchCan.Active)
                _p12LHighLevelHeadLampController.InvokeFuncByName("StartCanLinMsg", null);
            else
                _p12LHighLevelHeadLampController.InvokeFuncByName("StopCanLinMsg", null);
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
                _p12LHighLevelHeadLampController.InvokeFuncByName("TiOff", null);
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
                _p12LHighLevelHeadLampController.InvokeFuncByName("TiOff", null);
                uiTrackBarTi.Enabled = false;
                _tiIndex = -1;

                //var ledIndex = uiDataGridViewTi.Rows[_tiIndex].Cells[0].Value.ToString().Replace("S-", "");
                //if (!_isHighLevel)
                //    ledIndex = (_p12lMergeTiIndex[int.Parse(ledIndex)] - 1).ToString();
                //var gray = uiTrackBarTi.Value.ToString();
                //_p12LHighLevelHeadLampController.InvokeFuncByName(
                //    _isHighLevel ? "LedSingleOn" : "YLedSingleOn", new object[] { ledIndex, gray });

                _tiSt.Start();
            }
        }

        private void uiRadioButtonTiHolding_CheckedChanged(object sender, EventArgs e)
        {
            if (uiRadioButtonTiHolding.Checked)
            {
                _p12LHighLevelHeadLampController.InvokeFuncByName("TiOn", new object[] { uiTrackBarTi.Value.ToString() });
                uiTrackBarTi.Enabled = true;
                _tiSt.Stop();
                _tiIndex = 0;
            }
        }

        private void uiRadioButton1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (uiRadioButton1.Checked)
            {
                _p12LHighLevelHeadLampController.InvokeFuncByName("DrlPlOn",
                    new object[] { uiTrackBarDrl.Value.ToString() });
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
                _p12LHighLevelHeadLampController.InvokeFuncByName("DrlPlOn",
                    new object[] { uiTrackBarPl.Value.ToString() });
                uiTrackBarDrl.Enabled = false;
                uiTrackBarPl.Enabled = false;
                _drlPlSt.Start();
            }
        }

        public void btnHdlmpLvlngReq_ValueChanged(object sender, int value)
        {
            _p12LHighLevelHeadLampController.InvokeFuncByName("HdlmpLvlngReq", new object[] { value.ToString() });
        }

        private Dictionary<int, int> _p12lMergeDrlIndex = new Dictionary<int, int>
        {
            {1, 49},
            {3, 47},
            {5, 45},
            {8, 43},
            {12, 41},
            {16, 39},
            {20, 37},
            {24, 35},
            {28, 33},
            {32, 31},
            {36, 29},
            {40, 27},
            {44, 25},
            {48, 23},
            {52, 21},
            {56, 19},
            {60, 17},
            {64, 15},
            {68, 13},
            {72, 11},
            {76, 9},
            {80, 7},
            {84, 5},
            {88, 3},
            {92, 1}
        };

        private Dictionary<int, int> _p12lMergeTiIndex = new Dictionary<int, int>
        {
            {7, 43},
            {11, 41},
            {15, 39},
            {19, 37},
            {23, 35},
            {27, 33},
            {31, 31},
            {35, 29},
            {39, 27},
            {43, 25},
            {47, 23},
            {51, 21},
            {55, 19},
            {59, 17},
            {63, 15},
            {67, 13},
            {71, 11},
            {75, 9},
            {79, 7},
            {83, 5},
            {87, 3},
            {91, 1},
        };
    }
}
