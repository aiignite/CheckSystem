using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.ComponentModel;
using System.Threading;

namespace Controller
{
    [Description("LIN-Product,HVSM-EMC版本")]
    public sealed class HvsmEmcModule : ControllerBase
    {
        public LinBus Lin;

        [Description("R,LIN丢失")]
        public bool IsLinLoss = true;

        [Description("R,HSD1_Current")]
        public double HSD1_Current = double.MinValue;

        [Description("R,HSD2_Current")]
        public double HSD2_Current = double.MinValue;

        [Description("R,HSD3_Current")]
        public double HSD3_Current = double.MinValue;

        [Description("R,HSD4_Current")]
        public double HSD4_Current = double.MinValue;

        [Description("R,VBAT1")]
        public double Vbat1 = double.MinValue;

        [Description("R,VBAT2")]
        public double Vbat2 = double.MinValue;

        [Description("R,NTC1")]
        public double Ntc1 = double.MinValue;

        [Description("R,NTC2")]
        public double Ntc2 = double.MinValue;

        [Description("R,NTC3")]
        public double Ntc3 = double.MinValue;

        [Description("R,NTC4")]
        public double Ntc4 = double.MinValue;

        [Description("R,INPUT")]
        public string Input = string.Empty;

        [Description("R,VbatNTC12_Status_0x10")]
        public string Msg0x10 = string.Empty;

        [Description("R,HSD1234_Current_Status_0x11")]
        public string Msg0x11 = string.Empty;

        [Description("R,HSD1234_Fault_NTC1234_Fault_Status_0x12")]
        public string Msg0x12 = string.Empty;

        [Description("R,FAN12_Fault_NTC34_Status_0x13")]
        public string Msg0x13 = string.Empty;

        public HvsmEmcModule(string name) : base(name)
        {
            MonitorValue = double.MinValue;
            MonitorThreshold = double.MinValue;

            LinBus.PushLinMsg += LinBus_PushLinMsg;
            MainWork();
            SchedulerAsync();
        }

        ~HvsmEmcModule()
        {
            Dispose();
        }

        private void LinBus_PushLinMsg(string name, LinBus.LinDataPackage data)
        {
            if (Lin == null || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(Lin.Name))
                return;

            try
            {
                if (Lin.Name == name)
                {
                    if (data.LinDataLen == 8 && data.LinId == 0x10)
                    {
                        var emptyBytesStr = ValueHelper.GetHextStr(new byte[8]);
                        var msg = ValueHelper.GetHextStr(data.LinData);
                        if (msg == emptyBytesStr)
                            return;

                        lock (_lockTs)
                        {
                            Msg0x10 = msg;
                            _lastRecvLinMsg = HighPrecisionTimer.GetTimestamp();

                            Vbat1 = data.LinData[1] * 256 + data.LinData[0];
                            Vbat2 = data.LinData[3] * 256 + data.LinData[2];
                            Ntc1 = data.LinData[5] * 256 + data.LinData[4];
                            Ntc2 = data.LinData[7] * 256 + data.LinData[6];
                        }
                    }
                    else if (data.LinDataLen == 8 && data.LinId == 0x11)
                    {
                        lock (_lockTs)
                        {
                            if (IsLinLoss)
                                return;

                            Msg0x11 = ValueHelper.GetHextStr(data.LinData);
                            HSD1_Current = data.LinData[1] * 256 + data.LinData[0];
                            HSD2_Current = data.LinData[3] * 256 + data.LinData[2];
                            HSD3_Current = data.LinData[5] * 256 + data.LinData[4];
                            HSD4_Current = data.LinData[7] * 256 + data.LinData[6];
                        }
                    }
                    else if (data.LinDataLen == 8 && data.LinId == 0x12)
                    {
                        lock (_lockTs)
                        {
                            if (IsLinLoss)
                                return;

                            Msg0x12 = ValueHelper.GetHextStr(data.LinData);
                        }
                    }
                    else if (data.LinDataLen == 8 && data.LinId == 0x13)
                    {
                        lock (_lockTs)
                        {
                            if (IsLinLoss)
                                return;

                            Msg0x13 = ValueHelper.GetHextStr(data.LinData);

                            Ntc3 = data.LinData[3] * 256 + data.LinData[2];
                            Ntc4 = data.LinData[5] * 256 + data.LinData[4];

                            Input = ValueHelper.GetHextStr(data.LinData[6] == 1 ? (byte)0x00 : (byte)0x01);
                            //Input = ValueHelper.GetHextStr(data.LinData[6] );
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        [Description("打开LIN")]
        public void StartLin()
        {
            _isLinStart = true;
        }

        [Description("关闭LIN")]
        public void StopLin()
        {
            _isLinStart = false;
        }

        [Description("发送休眠指令")]
        public void SendSleepCmd()
        {
            try
            {
                lock (_lockLinSend)
                {
                    if (Lin != null)
                    {
                        bool b1 = Lin.SendMasterLin(0x01, new byte[] { 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF });
                        Thread.Sleep(10);
                        bool b2 = Lin.SendMasterLin(0x02, new byte[] { 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF });
                        Thread.Sleep(10);
                        bool b3 = Lin.SendMasterLin(0x3C, new byte[] { 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF });
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        [Description("设置高边占空比")]
        public void HsdDuty(int index, byte duty)
        {
            switch (index)
            {
                case 1:
                    _hsd1_duty = duty;
                    break;

                case 2:
                    _hsd2_duty = duty;
                    break;

                case 3:
                    _hsd3_duty = duty;
                    break;

                case 4:
                    _hsd4_duty = duty;
                    break;
            }
        }

        [Description("设置高边频率")]
        public void HsdFreq(int index, ushort freq)
        {
            switch (index)
            {
                case 1:
                    _hsd1_freq = freq;
                    break;

                case 2:
                    _hsd2_freq = freq;
                    break;

                case 3:
                    _hsd3_freq = freq;
                    break;

                case 4:
                    _hsd4_freq = freq;
                    break;
            }
        }

        [Description("设置风扇占空比")]
        public void FanDuty(int index, byte duty)
        {
            switch (index)
            {
                case 1:
                    _fan1_duty = duty;
                    break;

                case 2:
                    _fan2_duty = duty;
                    break;
            }
        }

        private readonly object _lockLinSend = new object();
        private bool _isLinStart;

        private byte _hsd1_duty;
        private ushort _hsd1_freq;
        private byte _hsd2_duty;
        private ushort _hsd2_freq;

        private byte _fan1_duty;
        private byte _fan2_duty;

        private byte _hsd3_duty;
        private ushort _hsd3_freq;
        private byte _hsd4_duty;
        private ushort _hsd4_freq;

        public bool IsEnableTempMonitor { get; set; }
        public string MonitorSymbol { get; set; }
        public double MonitorValue { get; set; }
        public double MonitorThreshold { get; set; }

        private void MainWork()
        {
            MonitorLinMsg();
            HSD12_FAN();
            HSD34();
            VbatNTC12_Status();
            HSD1234_Current_Status();
            HSD1234_Fault_NTC1234_Fault_Status();
            FAN12_Fault_NTC34_Status();
        }

        private long _lastRecvLinMsg = 0;
        private readonly object _lockTs = new object();

        private void MonitorLinMsg(int ms = 50)
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = () =>
                {
                    try
                    {
                        lock (_lockTs)
                        {
                            var nowTs = HighPrecisionTimer.GetTimestamp();
                            var ts = HighPrecisionTimer.GetTimestampIntervalMs(_lastRecvLinMsg, nowTs);
                            IsLinLoss = ts > 2000;
                            if (IsLinLoss)
                            {
                                Msg0x10 = string.Empty;
                                Msg0x11 = string.Empty;
                                Msg0x12 = string.Empty;
                                Msg0x13 = string.Empty;
                            }
                            else
                            {
                                //Console.WriteLine("LIN正常");
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                },
                Interval = ms
            });
        }

        private void HSD12_FAN(int ms = 50)
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = () =>
                {
                    try
                    {
                        var hsd1Duty = _hsd1_duty;
                        var hsd2Duty = _hsd2_duty;
                        var hsd1Freq = _hsd1_freq;
                        var hsd2Freq = _hsd2_freq;

                        if (IsEnableTempMonitor && !string.IsNullOrEmpty(MonitorSymbol) && MonitorValue != double.MinValue && MonitorValue != double.MaxValue)
                        {
                            if (MonitorSymbol == ">" && MonitorValue > MonitorThreshold)
                            {
                                hsd1Duty = 0;
                                hsd2Duty = 0;
                                hsd1Freq = 0;
                                hsd2Freq = 0;
                            }
                            else if (MonitorSymbol == ">=" && MonitorValue >= MonitorThreshold)
                            {
                                hsd1Duty = 0;
                                hsd2Duty = 0;
                                hsd1Freq = 0;
                                hsd2Freq = 0;
                            }
                            else if (MonitorSymbol == "<" && MonitorValue < MonitorThreshold)
                            {
                                hsd1Duty = 0;
                                hsd2Duty = 0;
                                hsd1Freq = 0;
                                hsd2Freq = 0;
                            }
                            else if (MonitorSymbol == "<=" && MonitorValue <= MonitorThreshold)
                            {
                                hsd1Duty = 0;
                                hsd2Duty = 0;
                                hsd1Freq = 0;
                                hsd2Freq = 0;
                            }
                            else if (MonitorSymbol == "==" && MonitorValue == MonitorThreshold)
                            {
                                hsd1Duty = 0;
                                hsd2Duty = 0;
                                hsd1Freq = 0;
                                hsd2Freq = 0;
                            }
                        }

                        var b = new byte[]
                        {
                            hsd1Duty, (byte)(hsd1Freq % 256), (byte)(hsd1Freq / 256) ,
                            hsd2Duty, (byte)(hsd2Freq % 256), (byte)(hsd2Freq / 256),
                            _fan1_duty, _fan2_duty
                        };
                        SendLinMsg(0x01, b);
                    }
                    catch (Exception)
                    {
                    }
                },
                Interval = ms
            });
        }

        private void HSD34(int ms = 50)
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = () =>
                {
                    try
                    {
                        var hsd3Duty = _hsd3_duty;
                        var hsd4Duty = _hsd4_duty;
                        var hsd3Freq = _hsd3_freq;
                        var hsd4Freq = _hsd4_freq;

                        if (IsEnableTempMonitor && !string.IsNullOrEmpty(MonitorSymbol) &&
                        MonitorValue != double.MinValue && MonitorValue != double.MaxValue &&
                         MonitorThreshold != double.MinValue && MonitorThreshold != double.MaxValue)
                        {
                            if (MonitorSymbol == ">" && MonitorValue > MonitorThreshold)
                            {
                                hsd3Duty = 0;
                                hsd4Duty = 0;
                                hsd3Freq = 0;
                                hsd4Freq = 0;
                            }
                            else if (MonitorSymbol == ">=" && MonitorValue >= MonitorThreshold)
                            {
                                hsd3Duty = 0;
                                hsd4Duty = 0;
                                hsd3Freq = 0;
                                hsd4Freq = 0;
                            }
                            else if (MonitorSymbol == "<" && MonitorValue < MonitorThreshold)
                            {
                                hsd3Duty = 0;
                                hsd4Duty = 0;
                                hsd3Freq = 0;
                                hsd4Freq = 0;
                            }
                            else if (MonitorSymbol == "<=" && MonitorValue <= MonitorThreshold)
                            {
                                hsd3Duty = 0;
                                hsd4Duty = 0;
                                hsd3Freq = 0;
                                hsd4Freq = 0;
                            }
                            else if (MonitorSymbol == "==" && MonitorValue == MonitorThreshold)
                            {
                                hsd3Duty = 0;
                                hsd4Duty = 0;
                                hsd3Freq = 0;
                                hsd4Freq = 0;
                            }
                        }

                        var b = new byte[]
                        {
                            hsd3Duty, (byte)(hsd3Freq % 256), (byte)(hsd3Freq / 256) ,
                            hsd4Duty, (byte)(hsd4Freq % 256), (byte)(hsd4Freq / 256),
                            0x00, 0x00
                        };
                        SendLinMsg(0x02, b);
                    }
                    catch (Exception)
                    {
                    }
                },
                Interval = ms
            });
        }

        private void VbatNTC12_Status(int ms = 50)
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = () =>
                {
                    SendLinMsg(0x10, new byte[0]);
                },
                Interval = ms
            });
        }

        private void HSD1234_Current_Status(int ms = 50)
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = () =>
                {
                    try
                    {
                        if (IsLinLoss)
                            return;
                        SendLinMsg(0x11, new byte[0]);
                    }
                    catch (Exception)
                    {

                    }
                },
                Interval = ms
            });
        }

        private void HSD1234_Fault_NTC1234_Fault_Status(int ms = 50)
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = () =>
                {
                    try
                    {
                        if (IsLinLoss)
                            return;
                        SendLinMsg(0x12, new byte[0]);
                    }
                    catch (Exception)
                    {
                    }
                },
                Interval = ms
            });
        }

        private void FAN12_Fault_NTC34_Status(int ms = 50)
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = () =>
                {
                    try
                    {
                        if (IsLinLoss)
                            return;
                        SendLinMsg(0x13, new byte[0]);
                    }
                    catch (Exception)
                    {
                    }
                },
                Interval = ms
            });
        }

        private void SendLinMsg(byte linId, byte[] linData)
        {
            if (_isLinStart && Lin != null)
            {
                try
                {
                    lock (_lockLinSend)
                    {
                        Lin.SendMasterLin(linId, linData);
                        Thread.Sleep(10);
                    }
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
