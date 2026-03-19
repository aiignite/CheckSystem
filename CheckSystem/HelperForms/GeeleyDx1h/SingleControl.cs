using CommonUtility;
using Controller;
using System;
using System.Threading;

namespace CheckSystem.HelperForms.GeeleyDx1h
{
    public partial class SingleControl
    {
        public delegate void PushMsgDelegate(string name, int index, string currMode, string cycleTime, string cycleNext);
        public event PushMsgDelegate PushMsgEvent;

        public bool IsRunning { get; set; }
        public string NowMode { get; set; }
        public long ModeStartTs { get; set; }

        private Thread _backgroundTh;

        private string _name;
        private int _index;
        private DX1H_V3_EMC _emcModule;

        public SingleControl(string name, int index, DX1H_V3_EMC emcModule)
        {
            _emcModule = emcModule;
            _name = name;
            _index = index;

            if (_backgroundTh != null)
            {
                _backgroundTh.Abort();
                _backgroundTh.Join();
            }
            _backgroundTh = new Thread(BackgroundWorker_DoWork);
            _backgroundTh.Start();
        }

        public void Start()
        {
            if (IsRunning)
                return;

            if (_backgroundTh != null)
                _step = 1;
        }

        public void Stop()
        {
            if (!IsRunning)
                return;

            _step = 3;
        }

        private long _startTs = HighPrecisionTimer.GetTimestamp();
        private int _step;

        private void BackgroundWorker_DoWork()
        {
            var nts = HighPrecisionTimer.GetTimestamp();
            var nowCycleIndex = 0;

            while (_backgroundTh.IsAlive)
            {
                if (_step == 1)
                {
                    nts = HighPrecisionTimer.GetTimestamp();
                    nowCycleIndex = 0;

                    //_emcModule.SeatHeatgCmdBackSecRightOn("100");
                    //_emcModule.SeatHeatgCmdCushSecRightOn("100");
                    //_emcModule.SeatVentnCmdBackSecRightOn("100");
                    //_emcModule.SeatVentnCmdCushSecRightOn("100");
                    //_emcModule.Group1Motro2Start_BackrestMotOn();
                    //_emcModule.Group2Motro4Start_LenMotOn();
                    //_emcModule.Group3Motro6Start_BackrestMotOn();
                    //_emcModule.Group4Motro8Start_LenMotOn();
                    _emcModule.StartScheduler();

                    NowMode = "马达动作";

                    _startTs = HighPrecisionTimer.GetTimestamp();
                    ModeStartTs = HighPrecisionTimer.GetTimestamp();
                    BackgroundWorker_ProgressChanged(0, nowCycleIndex);

                    _step++;
                }
                else if (_step == 2)
                {
                    var nowTs = HighPrecisionTimer.GetTimestamp();
                    if (HighPrecisionTimer.GetMillisecondsElapsed(nts) > 100)
                    {
                        if (HighPrecisionTimer.GetTimestampIntervalMs(_startTs, nts) >= 10 * 1000)
                        {
                            nowCycleIndex++;
                            if (nowCycleIndex == 2)
                                nowCycleIndex = 0;

                            //if (nowCycleIndex == 0)
                            //{
                            //    _emcModule.SeatHeatgCmdBackSecRightOn("100");
                            //    _emcModule.SeatHeatgCmdCushSecRightOn("100");
                            //    _emcModule.SeatVentnCmdBackSecRightOn("100");
                            //    _emcModule.SeatVentnCmdCushSecRightOn("100");
                            //    _emcModule.Group1Motro2Start_BackrestMotOn();
                            //    _emcModule.Group2Motro4Start_LenMotOn();
                            //    _emcModule.Group3Motro6Start_BackrestMotOn();
                            //    _emcModule.Group4Motro8Start_LenMotOn();

                            //    NowMode = "马达动作";
                            //}
                            //else
                            //{
                            //    _emcModule.SeatHeatgCmdBackSecRightOn("100");
                            //    _emcModule.SeatHeatgCmdCushSecRightOn("100");
                            //    _emcModule.SeatVentnCmdBackSecRightOn("100");
                            //    _emcModule.SeatVentnCmdCushSecRightOn("100");
                            //    _emcModule.Group1Motro2_BackrestMotOff();
                            //    _emcModule.Group2Motro4_LenMotOff();
                            //    _emcModule.Group3Motro6_BackrestMotOff();
                            //    _emcModule.Group4Motro8_LenMotOff();

                            //    NowMode = "马达休息";
                            //}

                            _startTs = HighPrecisionTimer.GetTimestamp();
                            ModeStartTs = HighPrecisionTimer.GetTimestamp();
                        }

                        BackgroundWorker_ProgressChanged(0, nowCycleIndex);
                        nts = HighPrecisionTimer.GetTimestamp();
                    }
                }
                else if (_step == 3)
                {
                    //_emcModule.Group1Motro2_BackrestMotOff();
                    //_emcModule.Group2Motro4_LenMotOff();
                    //_emcModule.Group3Motro6_BackrestMotOff();
                    //_emcModule.Group4Motro8_LenMotOff();

                    //_emcModule.SeatVentnCmdCushSecRightOff();
                    //_emcModule.SeatVentnCmdBackSecRightOff();
                    //_emcModule.SeatHeatgCmdBackSecRightOff();
                    //_emcModule.SeatHeatgCmdCushSecRightOff();

                    _emcModule.StopScheduler();

                    _step = 0;
                }

                Thread.Sleep(50);
            }
        }

        private void BackgroundWorker_ProgressChanged(object sender, int e)
        {
            var nowCycleIndex = e;
            var txt = Math.Round(HighPrecisionTimer.GetMillisecondsElapsed(_startTs) / 1000d, 2, MidpointRounding.AwayFromZero).ToString();
            //UpdateLblTxt(lblCurrentCycleMode, string.Format("{0}({1}/{2})", uiComboBox1.Items[_para[nowCycleIndex].Mode], nowCycleIndex + 1, _para.Length));
            //UpdateLblTxt(lblCycleTime, string.Format("{0}/{1}", _para[nowCycleIndex].Time, txt));
            //UpdateLblTxt(lblCycleNext, string.Format("{0}", uiComboBox1.Items[_para[nowCycleIndex == _para.Length - 1 ? 0 : nowCycleIndex + 1].Mode]));

            if (PushMsgEvent != null)
                PushMsgEvent(_name, _index, NowMode, string.Format("{0}/{1}", "10", txt), NowMode == "马达休息" ? "马达休息" : "马达动作");
        }
    }
}
