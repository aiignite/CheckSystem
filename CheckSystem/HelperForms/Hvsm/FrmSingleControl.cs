using CommonUtility;
using Controller;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckSystem.HelperForms.Hvsm
{
    public partial class FrmSingleControl : UserControl
    {
        public bool IsRunning { get; set; }
        public string NowMode { get; set; }
        public int Mode { get; set; }
        public long ModeStartTs { get; set; }
        //private BackgroundWorker BackgroundWorker;
        private HvsmEmcModule _hvsmEmcModule;
        private HvsmEmcConfig.DeviceConfigSingleCycleParaCyclePara[] _para;
        private string _name;
        private int _hsd1_duty;
        private int _hsd1_freq;
        private int _hsd2_duty;
        private int _hsd2_freq;
        private int _hsd3_duty;
        private int _hsd3_freq;
        private int _hsd4_duty;
        private int _hsd4_freq;
        private int _fan1_duty;
        private int _fan2_duty;

        private Thread _backgroundTh;

        public FrmSingleControl(string name, HvsmEmcModule hvsmEmcModule)
        {
            _hvsmEmcModule = hvsmEmcModule;
            InitializeComponent();
            _name = name;
            uiGroupBox1.Text = name;
            uiComboBox1.SelectedIndex = 0;
            NowMode = uiComboBox1.SelectedItem.ToString();
            _cmbSelectItem = NowMode;
            for (int i = 0; i < uiComboBox1.Items.Count; i++)
                _cmbItems.Add(uiComboBox1.Items[i].ToString());
            uiComboBox1.SelectedIndexChanged += UiComboBox1_SelectedIndexChanged;

            if (_backgroundTh != null)
            {
                _backgroundTh.Abort();
                _backgroundTh.Join();
            }
            _backgroundTh = new Thread(BackgroundWorker_DoWork);
            _backgroundTh.IsBackground = true;
            _backgroundTh.Start();

            btnStart.Click += btnStart_Click;
            btnStop.Click += btnStop_Click;
        }

        public void DisposeTh()
        {
            if (_backgroundTh != null)
            {
                _backgroundTh.Abort();
                _backgroundTh.Join();
            }
        }

        private string _cmbSelectItem;
        private List<string> _cmbItems = new List<string>();

        private void UiComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Mode = uiComboBox1.SelectedIndex;
            _cmbSelectItem = uiComboBox1.SelectedItem.ToString();
        }

        public void Start()
        {
            if (IsRunning)
                return;

            SwitchFunc(false);
            uiGroupBox1.Text = _name;

            if (_backgroundTh != null && Mode == 2)
            {
                _step = 1;
            }
            else if (Mode == 0)
            {
                _step = 0;
                ModeStartTs = HighPrecisionTimer.GetTimestamp();
                for (var i = 1; i <= 4; i++)
                {
                    _hvsmEmcModule.HsdDuty(i, (byte)0);
                    _hvsmEmcModule.HsdFreq(i, (ushort)0);
                }
                for (var i = 1; i <= 2; i++)
                    _hvsmEmcModule.FanDuty(i, (byte)0);
                RunSleep();
                NowMode = "SLEEP模式";
            }
            else if (Mode == 1)
            {
                _step = 0;
                ModeStartTs = HighPrecisionTimer.GetTimestamp();
                //for (var i = 1; i <= 4; i++)
                //{
                //    _hvsmEmcModule.HsdDuty(i, (byte)100);
                //    _hvsmEmcModule.HsdFreq(i, (ushort)200);
                //}
                //for (var i = 1; i <= 2; i++)
                //    _hvsmEmcModule.FanDuty(i, (byte)50);

                _hvsmEmcModule.HsdDuty(1, (byte)_hsd1_duty);
                _hvsmEmcModule.HsdFreq(1, (byte)_hsd1_freq);

                _hvsmEmcModule.HsdDuty(2, (byte)_hsd2_duty);
                _hvsmEmcModule.HsdFreq(2, (byte)_hsd2_freq);

                _hvsmEmcModule.HsdDuty(3, (byte)_hsd3_duty);
                _hvsmEmcModule.HsdFreq(3, (byte)_hsd3_freq);

                _hvsmEmcModule.HsdDuty(4, (byte)_hsd4_duty);
                _hvsmEmcModule.HsdFreq(4, (byte)_hsd4_freq);

                _hvsmEmcModule.FanDuty(1, (byte)_fan1_duty);
                _hvsmEmcModule.FanDuty(2, (byte)_fan2_duty);

                _hvsmEmcModule.StartLin();
                NowMode = "动作模式";
            }
            else
            {
                uiGroupBox1.Text = _name + "_" + "StartFail";
            }
        }

        private async void RunSleep()
        {
            await Task.Run(() =>
            {
                try
                {
                    _hvsmEmcModule.StopLin();
                    Thread.Sleep(200);
                    _hvsmEmcModule.SendSleepCmd();
                }
                catch (Exception)
                {
                }
            });
        }

        public void Stop()
        {
            if (!IsRunning)
                return;

            uiGroupBox1.Text = _name;
            _step = 3;
        }

        public void SetMode(int index)
        {
            uiComboBox1.SelectedIndex = index;
        }

        public void SetCycle(HvsmEmcConfig.DeviceConfigSingleCycleParaCyclePara[] para)
        {
            _para = para;
        }

        public void SetStandByModePara(
            int hsd1_duty, int hsd1_freq,
            int hsd2_duty, int hsd2_freq,
            int hsd3_duty, int hsd3_freq,
            int hsd4_duty, int hsd4_freq,
            int fan1_duty, int fan2_duty)
        {
            _hsd1_duty = hsd1_duty;
            _hsd2_duty = hsd2_duty;
            _hsd3_duty = hsd3_duty;
            _hsd4_duty = hsd4_duty;
            _fan1_duty = fan1_duty;
            _fan2_duty = fan2_duty;
            _hsd1_freq = hsd1_freq;
            _hsd2_freq = hsd2_freq;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private long _startTs = HighPrecisionTimer.GetTimestamp();
        private int _step;

        private void BackgroundWorker_DoWork()
        {
            var nts = HighPrecisionTimer.GetTimestamp();
            var nowCycleIndex = 0;

            while (_backgroundTh.IsAlive)
            {
                try
                {
                    if (_step == 1)
                    {
                        nts = HighPrecisionTimer.GetTimestamp();
                        nowCycleIndex = 0;

                        if (_para[nowCycleIndex].Mode == 0)
                        {
                            RunSleep();
                            NowMode = "SLEEP模式";
                        }
                        else
                        {
                            for (var i = 1; i <= 4; i++)
                            {
                                _hvsmEmcModule.HsdDuty(i, (byte)_para[nowCycleIndex].Hsd.Duty);
                                _hvsmEmcModule.HsdFreq(i, (ushort)_para[nowCycleIndex].Hsd.Freq);
                            }
                            for (var i = 1; i <= 2; i++)
                                _hvsmEmcModule.FanDuty(i, (byte)_para[nowCycleIndex].Fan.Duty);
                            _hvsmEmcModule.StartLin();
                            NowMode = "动作模式";
                        }

                        _startTs = HighPrecisionTimer.GetTimestamp();
                        ModeStartTs = HighPrecisionTimer.GetTimestamp();
                        BackgroundWorker_ProgressChanged(0, nowCycleIndex);
                        _lastBackgroundWorkerProgressChanged = 0;

                        _step++;
                    }
                    else if (_step == 2)
                    {
                        var nowTs = HighPrecisionTimer.GetTimestamp();
                        if (HighPrecisionTimer.GetMillisecondsElapsed(nts) > 100)
                        {
                            if (HighPrecisionTimer.GetTimestampIntervalMs(_startTs, nts) >= _para[nowCycleIndex].Time * 1000)
                            {
                                nowCycleIndex++;
                                if (nowCycleIndex == _para.Length)
                                    nowCycleIndex = 0;

                                if (_para[nowCycleIndex].Mode == 0)
                                {
                                    RunSleep();
                                    NowMode = "SLEEP模式";
                                }
                                else
                                {
                                    for (var i = 1; i <= 4; i++)
                                    {
                                        _hvsmEmcModule.HsdDuty(i, (byte)_para[nowCycleIndex].Hsd.Duty);
                                        _hvsmEmcModule.HsdFreq(i, (ushort)_para[nowCycleIndex].Hsd.Freq);
                                    }
                                    for (var i = 1; i <= 2; i++)
                                        _hvsmEmcModule.FanDuty(i, (byte)_para[nowCycleIndex].Fan.Duty);
                                    _hvsmEmcModule.StartLin();
                                    NowMode = "动作模式";
                                }

                                _startTs = HighPrecisionTimer.GetTimestamp();
                                ModeStartTs = HighPrecisionTimer.GetTimestamp();
                                _lastBackgroundWorkerProgressChanged = 0;
                            }

                            BackgroundWorker_ProgressChanged(0, nowCycleIndex);
                            nts = HighPrecisionTimer.GetTimestamp();
                        }
                    }
                    else if (_step == 3)
                    {
                        _lastBackgroundWorkerProgressChanged = 0;
                        RunSleep();
                        SwitchFunc(true);
                        _step = 0;
                    }
                }
                catch (Exception)
                {
                }

                Thread.Sleep(50);
            }
        }

        private long _lastBackgroundWorkerProgressChanged = 0;

        private void BackgroundWorker_ProgressChanged(object sender, int e)
        {
            try
            {
                var nowTs = HighPrecisionTimer.GetTimestamp();
                var ts = HighPrecisionTimer.GetTimestampIntervalMs(_lastBackgroundWorkerProgressChanged, nowTs);
                if (ts > 500)
                {
                    var nowCycleIndex = e;
                    var txt = Math.Round(HighPrecisionTimer.GetMillisecondsElapsed(_startTs) / 1000d, 2, MidpointRounding.AwayFromZero).ToString();

                    var value1 = string.Format("{0}({1}/{2})", _cmbItems[_para[nowCycleIndex].Mode], nowCycleIndex + 1, _para.Length);
                    var value2 = string.Format("{0}/{1}", _para[nowCycleIndex].Time, txt);
                    var value3 = _cmbItems[_para[nowCycleIndex == _para.Length - 1 ? 0 : nowCycleIndex + 1].Mode];
                    UpdateLblTxt(lblCurrentCycleMode, lblCycleTime, lblCycleNext, value1, value2, value3);

                    //UpdateLblTxt(lblCurrentCycleMode, string.Format("{0}({1}/{2})", uiComboBox1.Items[_para[nowCycleIndex].Mode], nowCycleIndex + 1, _para.Length));
                    //UpdateLblTxt(lblCycleTime, string.Format("{0}/{1}", _para[nowCycleIndex].Time, txt));
                    //UpdateLblTxt(lblCycleNext, string.Format("{0}", uiComboBox1.Items[_para[nowCycleIndex == _para.Length - 1 ? 0 : nowCycleIndex + 1].Mode]));

                    _lastBackgroundWorkerProgressChanged = HighPrecisionTimer.GetTimestamp();
                }
            }
            catch (Exception)
            {
            }
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SwitchFunc(true);
        }

        private delegate void UpdateLblTxtDelegate(UILabel lbl1, UILabel lbl2, UILabel lbl3, string value1, string value2, string value3);
        private void UpdateLblTxt(UILabel lbl1, UILabel lbl2, UILabel lbl3, string value1, string value2, string value3)
        {
            try
            {
                var updateDgvDelegate = new UpdateLblTxtDelegate(UpdateLblTxt);

                if (InvokeRequired)
                {
                    Invoke(updateDgvDelegate, lbl1, lbl2, lbl3, value1, value2, value3);
                }
                else
                {
                    BeginInvoke((MethodInvoker)delegate
                    {
                        lbl1.Text = value1;
                        lbl2.Text = value2;
                        lbl3.Text = value3;
                    });
                }
            }
            catch (Exception)
            {
            }
        }


        private delegate void SwitchFuncDelegate(bool isEnable);
        private void SwitchFunc(bool isEnable)
        {
            try
            {
                var switchFuncDelegate = new SwitchFuncDelegate(SwitchFunc);
                if (InvokeRequired)
                    Invoke(switchFuncDelegate, isEnable);
                else
                {
                    if (isEnable)
                    {
                        uiGroupBox1.Style = UIStyle.Blue;

                        uiComboBox1.Enabled = true;
                        IsRunning = false;

                        btnStart.Enabled = true;
                        btnStop.Enabled = false;

                        uiMarkLabel1.Visible = uiMarkLabel2.Visible = uiMarkLabel3.Visible = false;
                        lblCurrentCycleMode.Visible = lblCycleTime.Visible = lblCycleNext.Visible = false;
                        lblCurrentCycleMode.Text = lblCycleTime.Text = lblCycleNext.Text = string.Empty;
                    }
                    else
                    {
                        uiGroupBox1.Style = UIStyle.Purple;

                        uiComboBox1.Enabled = false;
                        IsRunning = true;

                        if (Mode != 2)
                        {
                            uiMarkLabel1.Visible = uiMarkLabel2.Visible = uiMarkLabel3.Visible = false;
                            lblCurrentCycleMode.Visible = lblCycleTime.Visible = lblCycleNext.Visible = false;
                        }
                        else
                        {
                            uiMarkLabel1.Visible = uiMarkLabel2.Visible = uiMarkLabel3.Visible = true;
                            lblCurrentCycleMode.Visible = lblCycleTime.Visible = lblCycleNext.Visible = true;
                            lblCurrentCycleMode.Text = lblCycleTime.Text = lblCycleNext.Text = string.Empty;
                        }

                        btnStart.Enabled = false;
                        btnStop.Enabled = true;
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
