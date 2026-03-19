using CommonUtility;
using Controller;
using Sunny.UI;
using System;
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
        private HvsmEmcConfig.DeviceConfigCyclePara[] _para;
        private string _name;

        private Thread _backgroundTh;

        public FrmSingleControl(string name, HvsmEmcModule hvsmEmcModule)
        {
            _hvsmEmcModule = hvsmEmcModule;
            InitializeComponent();
            _name = name;
            uiGroupBox1.Text = name;
            uiComboBox1.SelectedIndex = 0;
            NowMode = uiComboBox1.SelectedItem.ToString();
            uiComboBox1.SelectedIndexChanged += UiComboBox1_SelectedIndexChanged;

            if (_backgroundTh != null)
            {
                _backgroundTh.Abort();
                _backgroundTh.Join();
            }

            //BackgroundWorker = new BackgroundWorker();
            //BackgroundWorker.WorkerReportsProgress = true;
            //BackgroundWorker.WorkerSupportsCancellation = true;
            //BackgroundWorker.DoWork += BackgroundWorker_DoWork;
            //BackgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            //BackgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            btnStart.Click += btnStart_Click;
            btnStop.Click += btnStop_Click;
        }

        private void UiComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Mode = uiComboBox1.SelectedIndex;
        }

        public void Start()
        {
            if (IsRunning)
                return;

            SwitchFunc(false);
            uiGroupBox1.Text = _name;

            if (BackgroundWorker != null && Mode == 2)
            {
                if (BackgroundWorker.IsBusy)
                {
                    MessageBox.Show("启动失败，请先停止当前任务");
                    return;
                }
                BackgroundWorker.RunWorkerAsync();
            }
            else if (Mode == 0)
            {
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
                ModeStartTs = HighPrecisionTimer.GetTimestamp();
                for (var i = 1; i <= 4; i++)
                {
                    _hvsmEmcModule.HsdDuty(i, (byte)100);
                    _hvsmEmcModule.HsdFreq(i, (ushort)200);
                }
                for (var i = 1; i <= 2; i++)
                    _hvsmEmcModule.FanDuty(i, (byte)50);
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
                _hvsmEmcModule.StopLin();
                Thread.Sleep(200);
                _hvsmEmcModule.SendSleepCmd();
            });
        }

        public void Stop()
        {
            if (!IsRunning)
                return;

            uiGroupBox1.Text = _name;

            if (BackgroundWorker != null)
            {
                if (BackgroundWorker.IsBusy)
                    BackgroundWorker.CancelAsync();
                else
                    SwitchFunc(true);
            }
        }

        public void SetMode(int index)
        {
            uiComboBox1.SelectedIndex = index;
        }

        public void SetCycle(HvsmEmcConfig.DeviceConfigCyclePara[] para)
        {
            _para = para;
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

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            uiGroupBox1.Text = string.Format("{0}_{1}", _name, DateTime.Now.ToString("hh:mm:ss:fff"));

            var worker = (BackgroundWorker)sender;
            var nts = HighPrecisionTimer.GetTimestamp();

            var nowCycleIndex = 0;
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
            worker.ReportProgress(0, nowCycleIndex);

            // Keep acquiring until we error or cancel
            while (!worker.CancellationPending)
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
                    }

                    worker.ReportProgress(0, nowCycleIndex);
                    nts = HighPrecisionTimer.GetTimestamp();
                }

                Thread.Sleep(50);
            }
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var nowCycleIndex = (int)e.UserState;
            var txt = Math.Round(HighPrecisionTimer.GetMillisecondsElapsed(_startTs) / 1000d, 2, MidpointRounding.AwayFromZero).ToString();
            UpdateLblTxt(lblCurrentCycleMode, string.Format("{0}({1}/{2})", uiComboBox1.Items[_para[nowCycleIndex].Mode], nowCycleIndex + 1, _para.Length));
            UpdateLblTxt(lblCycleTime, string.Format("{0}/{1}", _para[nowCycleIndex].Time, txt));
            UpdateLblTxt(lblCycleNext, string.Format("{0}", uiComboBox1.Items[_para[nowCycleIndex == _para.Length - 1 ? 0 : nowCycleIndex + 1].Mode]));
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SwitchFunc(true);
        }

        private delegate void UpdateLblTxtDelegate(UILabel lbl, string value);
        private void UpdateLblTxt(UILabel lbl, string value)
        {
            var updateDgvDelegate = new UpdateLblTxtDelegate(UpdateLblTxt);

            if (lbl.InvokeRequired)
                lbl.Invoke(updateDgvDelegate, lbl, value);
            else
                lbl.Text = value;
        }

        private void SwitchFunc(bool isEnable)
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
}
