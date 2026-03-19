using CommonUtility;
using CommonUtility.BusLoader;
using Controller.HardwareController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using static CommonUtility.BusLoader.CanBus;

namespace Controller
{
    [Description("CAN-Device")]
    public sealed class SyControllerWith56Pin : ControllerBase, ICcdIoController
    {
        #region Fields
        public CanBus GatwayCan1;
        public CanBus GatwayCan2;
        public LinBus GatewayLin;
        public MySerialPort GatewaySci2;
        public MySerialPort GatewaySci3;
        public MySerialPort GatewaySci6;

        [Description("R/W,单次采集次数")]
        public int GatheringFrequency = 10;

        [Description("R/W,单次采样时间")]
        public ushort GatheringTime = 250;

        [Description("R,电流1")]
        public float Current1;

        [Description("R,电流2")]
        public float Current2;

        [Description("R,电流3")]
        public float Current3;

        [Description("R,电流4")]
        public float Current4;

        [Description("R,电压1")]
        public float Voltage1;

        [Description("R,电压2")]
        public float Voltage2;

        [Description("R,电压3")]
        public float Voltage3;

        [Description("R,电压4")]
        public float Voltage4;

        [Description("R,电压5")]
        public float Voltage5;

        [Description("R,电压6")]
        public float Voltage6;

        [Description("R,输入1")]
        public string Di1;

        [Description("R,输入2")]
        public string Di2;

        [Description("R/W,输出频率占空比_high_side1_frequency")]
        public float HighSide1Frequency;

        [Description("R/W,输出频率占空比_high_side1_duty")]
        public float HighSide1Duty;

        [Description("R/W,输出频率占空比_high_side2_frequency")]
        public float HighSide2Frequency;

        [Description("R/W,输出频率占空比_high_side2_duty")]
        public float HighSide2Duty;

        [Description("R/W,高边输出1")]
        public bool DoHighSide1;

        [Description("R/W,高边输出2")]
        public bool DoHighSide2;

        [Description("R/W,低边输出1")]
        public bool DoLowSide1;

        [Description("R/W,低边输出2")]
        public bool DoLowSide2;

        [Description("R/W,低边输出3")]
        public bool DoLowSide3;

        [Description("R/W,低边输出4")]
        public bool DoLowSide4;

        [Description("R/W,继电器1")]
        public bool Relay1;

        [Description("R/W,继电器2")]
        public bool Relay2;

        [Description("R/W,继电器3")]
        public bool Relay3;

        [Description("R/W,继电器4")]
        public bool Relay4;

        [Description("R/W,继电器5")]
        public bool Relay5;

        [Description("R/W,继电器6")]
        public bool Relay6;

        [Description("R/W,继电器7")]
        public bool Relay7;

        [Description("R/W,继电器8")]
        public bool Relay8;

        [Description("R/W,继电器9")]
        public bool Relay9;

        [Description("R/W,继电器10")]
        public bool Relay10;

        [Description("R/W,继电器11")]
        public bool Relay11;

        [Description("R/W,继电器12")]
        public bool Relay12;

        [Description("R,输入捕获频率占空比_PWM1_Capture_Freq")]
        public float Pwm1CaptureFrequency;

        [Description("R,输入捕获频率占空比_PWM1_Capture_Duty")]
        public float Pwm1CaptureDuty;

        [Description("R,输入捕获频率占空比_PWM2_Capture_Freq")]
        public float Pwm2CaptureFrequency;

        [Description("R,输入捕获频率占空比_PWM2_Capture_Duty")]
        public float Pwm2CaptureDuty;

        [Description("R,电流1-AD")]
        public float Current1Ad;

        [Description("R,电流2-AD")]
        public float Current2Ad;

        [Description("R,电流3-AD")]
        public float Current3Ad;

        [Description("R,电流4-AD")]
        public float Current4Ad;

        [Description("R,电压1-AD")]
        public float Voltage1Ad;

        [Description("R,电压2-AD")]
        public float Voltage2Ad;

        [Description("R,电压3-AD")]
        public float Voltage3Ad;

        [Description("R,电压4-AD")]
        public float Voltage4Ad;

        [Description("R,电压5-AD")]
        public float Voltage5Ad;

        [Description("R,电压6-AD")]
        public float Voltage6Ad;

        [Description("R,CAN1到CAN2转发时间")]
        public float Can1ToCan2TransferTime;

        [Description("R,CAN2到CAN1转发时间")]
        public float Can2ToCan1TransferTime;

        [Description("R,启动时间")]
        public float Mcrotime;

        #endregion

        public SyControllerWith56Pin(string name)
            : base(name)
        {
            CurrentFunction = ControllerFunction.Free;
            WaitHandle = new AutoResetEvent(false);
            ControllerLocker = new object();
        }

        ~SyControllerWith56Pin()
        {
            Dispose();
        }

        [Description("R/W,FilterType")]
        public int FilterType = 0;
        public byte[] FlashReadBytes { get; set; }
        public ControllerFunction CurrentFunction { get; set; }
        public EventWaitHandle WaitHandle { get; set; }
        public object ControllerLocker { get; set; }
        public bool IsConnected { get; set; }
        private MyUdpClient MyUdpClient { get; set; }
        public string RemoteIpPort { get; set; }
        private IPAddress RemoteIpAddress { get; set; }
        private int RemotePort { get; set; }
        private Thread DaemonTh { get; set; }

        public void InitRemoteIpAddress(string ipPort)
        {
            RemoteIpPort = ipPort;

            try
            {
                var sp = ipPort.Split(':');
                var ipAddrStr = sp[0];
                var port = int.Parse(sp[1]);
                RemotePort = port;

                RemoteIpAddress = IPAddress.Parse(ipAddrStr);

                MyUdpClient = ipAddrStr.Equals("127.0.0.1")
                    ? new MyUdpClient("127.0.0.1", port + 1)
                    : new MyUdpClient("192.168.1.50", 5000 + int.Parse(ipAddrStr.Split('.')[3]));

                MyUdpClient.PushMsgEvent += _myUdpClient_PushMsgEvent;
                MyUdpClient.AddRemoteClient(ipAddrStr, port);
                MyUdpClient.BeginReceive();
                IsConnected = true;

                GatewayLin = new ControllerWith56PinGatewayLin(this);
                GatwayCan1 = new ControllerWith56PinGatewayCan(1, this);
                GatwayCan2 = new ControllerWith56PinGatewayCan(2, this);
                GatewaySci2 = new ControllerWith56PinGatewaySci(2, this);
                GatewaySci3 = new ControllerWith56PinGatewaySci(3, this);
                GatewaySci6 = new ControllerWith56PinGatewaySci(6, this);

                if (DaemonTh != null)
                {
                    DaemonTh.Abort();
                    DaemonTh.Join();
                }

                DaemonTh = new Thread(Daemon) { IsBackground = true };
                DaemonTh.Start();
            }
            catch (Exception)
            {
                IsConnected = false;
            }
        }

        private string _lastOutputState = @"000000000000000000";
        private string LasthighSide1Frequency { get; set; }
        private string LastHighSide1Duty { get; set; }
        private string LasthighSide2Frequency { get; set; }
        private string LastHighSide2Duty { get; set; }
        private string CurrentReadCurrIndex { get; set; }
        private string CurrentReadVoltIndex { get; set; }
        private bool IsWaitSlaveLinMsg { get; set; }
        private byte WaitSlaveLinId { get; set; }

        private void Daemon()
        {
            _lastOutputState = @"000000000000000000";
            LasthighSide1Frequency = HighSide1Frequency.ToString(CultureInfo.InvariantCulture);
            LastHighSide1Duty = HighSide1Duty.ToString(CultureInfo.InvariantCulture);
            LasthighSide2Frequency = HighSide2Frequency.ToString(CultureInfo.InvariantCulture);
            LastHighSide2Duty = HighSide2Duty.ToString(CultureInfo.InvariantCulture);

            while (DaemonTh.IsAlive)
            {
                Thread.Sleep(50);

                if (!DaemonTh.IsAlive)
                    break;

                if (MyUdpClient == null)
                    continue;

                if (CurrentFunction != ControllerFunction.Free)
                    continue;

                var currentHighSide1Frequency = HighSide1Frequency.ToString(CultureInfo.InvariantCulture);
                if (currentHighSide1Frequency != LasthighSide1Frequency)
                    SetHighside1FreqencyDudy();
                LasthighSide1Frequency = currentHighSide1Frequency;

                var currentHighSide1Duty = HighSide1Duty.ToString(CultureInfo.InvariantCulture);
                if (currentHighSide1Duty != LastHighSide1Duty)
                    SetHighside1FreqencyDudy();
                LastHighSide1Duty = currentHighSide1Duty;

                var currentHighSide2Frequency = HighSide2Frequency.ToString(CultureInfo.InvariantCulture);
                if (currentHighSide2Frequency != LasthighSide2Frequency)
                    SetHighside2FreqencyDudy();
                LasthighSide2Frequency = currentHighSide2Frequency;

                var currentHighSide2Duty = HighSide2Duty.ToString(CultureInfo.InvariantCulture);
                if (currentHighSide2Duty != LastHighSide2Duty)
                    SetHighside2FreqencyDudy();
                LastHighSide2Duty = currentHighSide2Duty;

                if (_isAutoRefresh)
                {
                    var currentOutputState = string.Empty;
                    currentOutputState += DoHighSide1 ? "1" : "0";
                    currentOutputState += DoHighSide2 ? "1" : "0";
                    currentOutputState += DoLowSide1 ? "1" : "0";
                    currentOutputState += DoLowSide2 ? "1" : "0";
                    currentOutputState += DoLowSide3 ? "1" : "0";
                    currentOutputState += DoLowSide4 ? "1" : "0";
                    currentOutputState += Relay1 ? "1" : "0";
                    currentOutputState += Relay2 ? "1" : "0";
                    currentOutputState += Relay3 ? "1" : "0";
                    currentOutputState += Relay4 ? "1" : "0";
                    currentOutputState += Relay5 ? "1" : "0";
                    currentOutputState += Relay6 ? "1" : "0";
                    currentOutputState += Relay7 ? "1" : "0";
                    currentOutputState += Relay8 ? "1" : "0";
                    currentOutputState += Relay9 ? "1" : "0";
                    currentOutputState += Relay10 ? "1" : "0";
                    currentOutputState += Relay11 ? "1" : "0";
                    currentOutputState += Relay12 ? "1" : "0";
                    if (currentOutputState != _lastOutputState)
                    {
                        lock (ControllerLocker)
                        {
                            CurrentFunction = ControllerFunction.WriteOutput;

                            var values = new List<byte>();
                            values.AddRange(!DoHighSide1 ? new byte[] { 0x00, 0x00 } : new byte[] { 0x00, 0x01 });
                            values.AddRange(!DoHighSide2 ? new byte[] { 0x00, 0x00 } : new byte[] { 0x00, 0x01 });
                            values.AddRange(!DoLowSide1 ? new byte[] { 0x00, 0x00 } : new byte[] { 0x00, 0x01 });
                            values.AddRange(!DoLowSide2 ? new byte[] { 0x00, 0x00 } : new byte[] { 0x00, 0x01 });
                            values.AddRange(!DoLowSide3 ? new byte[] { 0x00, 0x00 } : new byte[] { 0x00, 0x01 });
                            values.AddRange(!DoLowSide4 ? new byte[] { 0x00, 0x00 } : new byte[] { 0x00, 0x01 });
                            values.AddRange(!Relay1 ? new byte[] { 0x00, 0x00 } : new byte[] { 0x00, 0x01 });
                            values.AddRange(!Relay2 ? new byte[] { 0x00, 0x00 } : new byte[] { 0x00, 0x01 });
                            values.AddRange(!Relay3 ? new byte[] { 0x00, 0x00 } : new byte[] { 0x00, 0x01 });
                            values.AddRange(!Relay4 ? new byte[] { 0x00, 0x00 } : new byte[] { 0x00, 0x01 });
                            values.AddRange(!Relay5 ? new byte[] { 0x00, 0x00 } : new byte[] { 0x00, 0x01 });
                            values.AddRange(!Relay6 ? new byte[] { 0x00, 0x00 } : new byte[] { 0x00, 0x01 });
                            values.AddRange(!Relay7 ? new byte[] { 0x00, 0x00 } : new byte[] { 0x00, 0x01 });
                            values.AddRange(!Relay8 ? new byte[] { 0x00, 0x00 } : new byte[] { 0x00, 0x01 });
                            values.AddRange(!Relay9 ? new byte[] { 0x00, 0x00 } : new byte[] { 0x00, 0x01 });
                            values.AddRange(!Relay10 ? new byte[] { 0x00, 0x00 } : new byte[] { 0x00, 0x01 });
                            values.AddRange(!Relay11 ? new byte[] { 0x00, 0x00 } : new byte[] { 0x00, 0x01 });
                            values.AddRange(!Relay12 ? new byte[] { 0x00, 0x00 } : new byte[] { 0x00, 0x01 });

                            var sendBytes = ModbusOperater.GetModbusWriteValues(0x10, 0x110.ToString(), 18, values.ToArray());

                            SendData(sendBytes);
                            WaitHandle.WaitOne(500);
                            CurrentFunction = ControllerFunction.Free;
                        }
                    }
                    _lastOutputState = currentOutputState;

                    GetCurrVoltDi();
                }
            }
        }

        /// <summary>
        /// 设置高低边以及继电器输出
        /// 测试响应时间小于15ms
        /// </summary>
        [Description("设置高低边以及继电器输出")]
        public bool SetOutputs()
        {
            if (_isAutoRefresh)
                return true;

            lock (ControllerLocker)
            {
                CurrentFunction = ControllerFunction.WriteOutput;

                var values = new List<byte>();
                values.AddRange(!DoHighSide1 ? new byte[] { 0x00, 0x00 } : new byte[] { 0x00, 0x01 });
                values.AddRange(!DoHighSide2 ? new byte[] { 0x00, 0x00 } : new byte[] { 0x00, 0x01 });
                values.AddRange(!DoLowSide1 ? new byte[] { 0x00, 0x00 } : new byte[] { 0x00, 0x01 });
                values.AddRange(!DoLowSide2 ? new byte[] { 0x00, 0x00 } : new byte[] { 0x00, 0x01 });
                values.AddRange(!DoLowSide3 ? new byte[] { 0x00, 0x00 } : new byte[] { 0x00, 0x01 });
                values.AddRange(!DoLowSide4 ? new byte[] { 0x00, 0x00 } : new byte[] { 0x00, 0x01 });
                values.AddRange(!Relay1 ? new byte[] { 0x00, 0x00 } : new byte[] { 0x00, 0x01 });
                values.AddRange(!Relay2 ? new byte[] { 0x00, 0x00 } : new byte[] { 0x00, 0x01 });
                values.AddRange(!Relay3 ? new byte[] { 0x00, 0x00 } : new byte[] { 0x00, 0x01 });
                values.AddRange(!Relay4 ? new byte[] { 0x00, 0x00 } : new byte[] { 0x00, 0x01 });
                values.AddRange(!Relay5 ? new byte[] { 0x00, 0x00 } : new byte[] { 0x00, 0x01 });
                values.AddRange(!Relay6 ? new byte[] { 0x00, 0x00 } : new byte[] { 0x00, 0x01 });
                values.AddRange(!Relay7 ? new byte[] { 0x00, 0x00 } : new byte[] { 0x00, 0x01 });
                values.AddRange(!Relay8 ? new byte[] { 0x00, 0x00 } : new byte[] { 0x00, 0x01 });
                values.AddRange(!Relay9 ? new byte[] { 0x00, 0x00 } : new byte[] { 0x00, 0x01 });
                values.AddRange(!Relay10 ? new byte[] { 0x00, 0x00 } : new byte[] { 0x00, 0x01 });
                values.AddRange(!Relay11 ? new byte[] { 0x00, 0x00 } : new byte[] { 0x00, 0x01 });
                values.AddRange(!Relay12 ? new byte[] { 0x00, 0x00 } : new byte[] { 0x00, 0x01 });

                var sendBytes = ModbusOperater.GetModbusWriteValues(0x10, 0x110.ToString(), 18, values.ToArray());

                SendData(sendBytes);
                var isSuccess = WaitHandle.WaitOne(500);
                CurrentFunction = ControllerFunction.Free;

                return isSuccess;
            }
        }

        public void GetInputs()
        {
            GetCurrentVoltageDi();
        }

        public void ResetOutPuts()
        {
            Relay1 = false;
            Relay2 = false;
            Relay3 = false;
            Relay4 = false;
            Relay5 = false;
            Relay6 = false;
            Relay7 = false;
            Relay8 = false;
            Relay9 = false;
            Relay10 = false;
            Relay11 = false;
            Relay12 = false;
            SetOutputs();
        }

        [Description("获取高低边以及继电器输出状态")]
        public void GetOutputs()
        {
            lock (ControllerLocker)
            {
                CurrentFunction = ControllerFunction.ReadOutput;
                SendData(GetReadValues(0x03, 0x110.ToString(), 18));
                WaitHandle.WaitOne(500);
                CurrentFunction = ControllerFunction.Free;
            }
        }

        private bool _isAutoRefresh;
        private float _current1;
        private float _current2;
        private float _current3;
        private float _current4;
        private float _voltage1;
        private float _voltage2;
        private float _voltage3;
        private float _voltage4;
        private float _voltage5;
        private float _voltage6;

        [Description("开启自动获取电流电压及DI")]
        public void StartAutoRefresh()
        {
            _isAutoRefresh = true;
        }

        [Description("取消自动获取电流电压及DI")]
        public void CanelAutoRefresh()
        {
            _isAutoRefresh = false;
        }

        /// <summary>
        /// 获取电流电压及DI
        /// 测试响应时间小于15ms
        /// </summary>
        [Description("获取电流电压及DI")]
        public void GetCurrentVoltageDi()
        {
            if (!_isAutoRefresh)
                GetCurrVoltDi();
        }

        private void GetCurrVoltDi()
        {
            Di1 = string.Empty;
            Di2 = string.Empty;

            lock (ControllerLocker)
            {
                CurrentFunction = ControllerFunction.ReadAdAndDi;

                var maxCount = GatheringFrequency <= 4 ? 4 : GatheringFrequency;

                var getAdc1Sum = new List<float>();
                var getAdc2Sum = new List<float>();
                var getAdc3Sum = new List<float>();
                var getAdc4Sum = new List<float>();

                var getAdv1Sum = new List<float>();
                var getAdv2Sum = new List<float>();
                var getAdv3Sum = new List<float>();
                var getAdv4Sum = new List<float>();
                var getAdv5Sum = new List<float>();
                var getAdv6Sum = new List<float>();

                var tempFailedCount = 0;

                for (var i = 0; i < maxCount; i++)
                {
                    var readBytes = GetReadValues(0x03, 0x100.ToString(), 12);
                    SendData(readBytes);

                    if (WaitHandle.WaitOne(500))
                    {
                        getAdc1Sum.Add(_current1);
                        getAdc2Sum.Add(_current2);
                        getAdc3Sum.Add(_current3);
                        getAdc4Sum.Add(_current4);

                        getAdv1Sum.Add(_voltage1);
                        getAdv2Sum.Add(_voltage2);
                        getAdv3Sum.Add(_voltage3);
                        getAdv4Sum.Add(_voltage4);
                        getAdv5Sum.Add(_voltage5);
                        getAdv6Sum.Add(_voltage6);
                    }
                    else
                        tempFailedCount++;

                    Thread.Sleep(5);
                }

                if (tempFailedCount == maxCount)
                {
                    Current1 = -9999;
                    Current2 = -9999;
                    Current3 = -9999;
                    Current4 = -9999;

                    Voltage1 = -9999;
                    Voltage2 = -9999;
                    Voltage3 = -9999;
                    Voltage4 = -9999;
                    Voltage5 = -9999;
                    Voltage6 = -9999;
                }
                else
                {
                    if (FilterType == 0)
                    {
                        Current1 = (float)Math.Round(WaveFilter.MidianAverageFileter(MedianFilterAd(getAdc1Sum.ToArray())), 4, MidpointRounding.AwayFromZero);
                        Current2 = (float)Math.Round(WaveFilter.MidianAverageFileter(MedianFilterAd(getAdc2Sum.ToArray())), 4, MidpointRounding.AwayFromZero);
                        Current3 = (float)Math.Round(WaveFilter.MidianAverageFileter(MedianFilterAd(getAdc3Sum.ToArray())), 4, MidpointRounding.AwayFromZero);
                        Current4 = (float)Math.Round(WaveFilter.MidianAverageFileter(MedianFilterAd(getAdc4Sum.ToArray())), 4, MidpointRounding.AwayFromZero);

                        Voltage1 = (float)Math.Round(WaveFilter.MidianAverageFileter(MedianFilterAd(getAdv1Sum.ToArray())), 4, MidpointRounding.AwayFromZero);
                        Voltage2 = (float)Math.Round(WaveFilter.MidianAverageFileter(MedianFilterAd(getAdv2Sum.ToArray())), 4, MidpointRounding.AwayFromZero);
                        Voltage3 = (float)Math.Round(WaveFilter.MidianAverageFileter(MedianFilterAd(getAdv3Sum.ToArray())), 4, MidpointRounding.AwayFromZero);
                        Voltage4 = (float)Math.Round(WaveFilter.MidianAverageFileter(MedianFilterAd(getAdv4Sum.ToArray())), 4, MidpointRounding.AwayFromZero);
                        Voltage5 = (float)Math.Round(WaveFilter.MidianAverageFileter(MedianFilterAd(getAdv5Sum.ToArray())), 4, MidpointRounding.AwayFromZero);
                        Voltage6 = (float)Math.Round(WaveFilter.MidianAverageFileter(MedianFilterAd(getAdv6Sum.ToArray())), 4, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        Current1 = (float)Math.Round(WaveFilter.MidianAverageFileter(SmoothingAd(getAdc1Sum.ToArray())), 4, MidpointRounding.AwayFromZero);
                        Current2 = (float)Math.Round(WaveFilter.MidianAverageFileter(SmoothingAd(getAdc2Sum.ToArray())), 4, MidpointRounding.AwayFromZero);
                        Current3 = (float)Math.Round(WaveFilter.MidianAverageFileter(SmoothingAd(getAdc3Sum.ToArray())), 4, MidpointRounding.AwayFromZero);
                        Current4 = (float)Math.Round(WaveFilter.MidianAverageFileter(SmoothingAd(getAdc4Sum.ToArray())), 4, MidpointRounding.AwayFromZero);

                        Voltage1 = (float)Math.Round(WaveFilter.MidianAverageFileter(SmoothingAd(getAdv1Sum.ToArray())), 4, MidpointRounding.AwayFromZero);
                        Voltage2 = (float)Math.Round(WaveFilter.MidianAverageFileter(SmoothingAd(getAdv2Sum.ToArray())), 4, MidpointRounding.AwayFromZero);
                        Voltage3 = (float)Math.Round(WaveFilter.MidianAverageFileter(SmoothingAd(getAdv3Sum.ToArray())), 4, MidpointRounding.AwayFromZero);
                        Voltage4 = (float)Math.Round(WaveFilter.MidianAverageFileter(SmoothingAd(getAdv4Sum.ToArray())), 4, MidpointRounding.AwayFromZero);
                        Voltage5 = (float)Math.Round(WaveFilter.MidianAverageFileter(SmoothingAd(getAdv5Sum.ToArray())), 4, MidpointRounding.AwayFromZero);
                        Voltage6 = (float)Math.Round(WaveFilter.MidianAverageFileter(SmoothingAd(getAdv6Sum.ToArray())), 4, MidpointRounding.AwayFromZero);
                    }
                    //Current1 = (float)Math.Round(WaveFilter.MidianAverageFileter(getAdc1Sum.ToArray()), 4, MidpointRounding.AwayFromZero);
                    //Current2 = (float)Math.Round(WaveFilter.MidianAverageFileter(getAdc2Sum.ToArray()), 4, MidpointRounding.AwayFromZero);
                    //Current3 = (float)Math.Round(WaveFilter.MidianAverageFileter(getAdc3Sum.ToArray()), 4, MidpointRounding.AwayFromZero);
                    //Current4 = (float)Math.Round(WaveFilter.MidianAverageFileter(getAdc4Sum.ToArray()), 4, MidpointRounding.AwayFromZero);

                    //Voltage1 = (float)Math.Round(WaveFilter.MidianAverageFileter(getAdv1Sum.ToArray()), 4, MidpointRounding.AwayFromZero);
                    //Voltage2 = (float)Math.Round(WaveFilter.MidianAverageFileter(getAdv2Sum.ToArray()), 4, MidpointRounding.AwayFromZero);
                    //Voltage3 = (float)Math.Round(WaveFilter.MidianAverageFileter(getAdv3Sum.ToArray()), 4, MidpointRounding.AwayFromZero);
                    //Voltage4 = (float)Math.Round(WaveFilter.MidianAverageFileter(getAdv4Sum.ToArray()), 4, MidpointRounding.AwayFromZero);
                    //Voltage5 = (float)Math.Round(WaveFilter.MidianAverageFileter(getAdv5Sum.ToArray()), 4, MidpointRounding.AwayFromZero);
                    //Voltage6 = (float)Math.Round(WaveFilter.MidianAverageFileter(getAdv6Sum.ToArray()), 4, MidpointRounding.AwayFromZero);
                }

                //var readBytes = GetReadValues(0x03, 0x100.ToString(), 12);
                //SendData(readBytes);

                //if (!WaitHandle.WaitOne(500))
                //{
                //    Current1 = -9999f;
                //    Current2 = -9999f;
                //    Current3 = -9999f;
                //    Current4 = -9999f;
                //    Voltage1 = -9999f;
                //    Voltage2 = -9999f;
                //    Voltage3 = -9999f;
                //    Voltage4 = -9999f;
                //    Voltage5 = -9999f;
                //    Voltage6 = -9999f;
                //}
                //else
                //{
                //    Current1 = _current1;
                //    Current2 = _current2;
                //    Current3 = _current3;
                //    Current4 = _current4;
                //    Voltage1 = _voltage1;
                //    Voltage2 = _voltage2;
                //    Voltage3 = _voltage3;
                //    Voltage4 = _voltage4;
                //    Voltage5 = _voltage5;
                //    Voltage6 = _voltage6;
                //}

                CurrentFunction = ControllerFunction.Free;
            }
        }

        [Description("获取AD值")]
        public void GetAds()
        {
            lock (ControllerLocker)
            {
                CurrentFunction = ControllerFunction.GetAd;

                var readBytes = GetReadValues(0x03, 0x130.ToString(), 10);
                SendData(readBytes);

                var isReadOk = WaitHandle.WaitOne(500);
                if (!isReadOk)
                {
                    Current1Ad = -9999f;
                    Current2Ad = -9999f;
                    Current3Ad = -9999f;
                    Current4Ad = -9999f;
                    Voltage1Ad = -9999f;
                    Voltage2Ad = -9999f;
                    Voltage3Ad = -9999f;
                    Voltage4Ad = -9999f;
                    Voltage5Ad = -9999f;
                    Voltage6Ad = -9999f;
                }
                CurrentFunction = ControllerFunction.Free;
            }
        }

        [Description("按频率采集电流")]
        public void GetFrequencyCurr()
        {
            for (var i = 1; i < 5; i++)
                GetSingleCurr(i.ToString());
        }

        [Description("采集单通道电流")]
        public void GetSingleCurr(string currIndex)
        {
            var func = new Func<bool>(() =>
            {
                bool isReadOk;
                int index;
                if (!int.TryParse(currIndex, out index))
                    return false;
                if (index < 1 && index > 4)
                    return false;
                lock (ControllerLocker)
                {
                    CurrentFunction = ControllerFunction.GetSingleCurr;

                    CurrentReadCurrIndex = currIndex;

                    var samplingFrequency = BitConverter.GetBytes(GatheringTime);
                    var bs = new byte[] { 0x00, 0x0C, 0x00, 0x72, 0x00, 0x00, 0x00, 0x00, 0x05, (byte)(index - 1), samplingFrequency[1], samplingFrequency[0] };
                    SendData(bs);
                    //var st = new Stopwatch();
                    //st.Start();
                    isReadOk = WaitHandle.WaitOne(5000);
                    //st.Stop();
                    //Console.WriteLine(st.ElapsedMilliseconds);

                    var fieldName = string.Format("Current{0}", index);
                    var field = GetType().GetField(fieldName);
                    if (field != null)
                    {
                        if (!isReadOk)
                            field.SetValue(this, -9999f);
                    }
                    CurrentReadCurrIndex = string.Empty;
                }

                return isReadOk;
            });

            if (!func.Invoke())
                func.Invoke();
        }

        [Description("按频率采集电压")]
        public void GetFrequencyVolt()
        {
            for (var i = 1; i < 5; i++)
                GetSingleVolt(i.ToString());
        }

        [Description("采集单通道电压")]
        public void GetSingleVolt(string voltIndex)
        {
            int index;
            if (int.TryParse(voltIndex, out index))
            {
                if (index >= 1 || index <= 6)
                {
                    lock (ControllerLocker)
                    {
                        CurrentFunction = ControllerFunction.GetSingleVolt;

                        CurrentReadVoltIndex = voltIndex;
                        var samplingFrequency = BitConverter.GetBytes(GatheringTime);
                        var bs = new byte[] { 0x00, 0x0C, 0x00, 0x72, 0x00, 0x00, 0x00, 0x00, 0x05, (byte)(index + 3), samplingFrequency[1], samplingFrequency[0] };
                        SendData(bs);
                        //var st = new Stopwatch();
                        //st.Start();
                        var isReadOk = WaitHandle.WaitOne(GatheringTime + 500);
                        //st.Stop();
                        //Console.WriteLine(st.ElapsedMilliseconds);

                        var fieldName = string.Format("Voltage{0}", index);
                        var field = GetType().GetField(fieldName);
                        if (field == null)
                            return;
                        if (!isReadOk)
                            field.SetValue(this, -9999f);
                        CurrentReadVoltIndex = string.Empty;
                    }
                }
            }
        }

        /// <summary>
        /// 读输出频率占空比
        /// </summary>
        [Description("读高边输出频率占空比")]
        public void ReadHighsideFrequencyDuty()
        {
            HighSide1Duty = -9999;
            HighSide1Frequency = -9999;
            HighSide2Duty = -9999;
            HighSide2Frequency = -9999;

            lock (ControllerLocker)
            {
                CurrentFunction = ControllerFunction.ReadHighsideFrequencyDuty;

                var readBytes = GetReadValues(0x03, 0x10C.ToString(), 4);
                SendData(readBytes);

                WaitHandle.WaitOne(1500);
                CurrentFunction = ControllerFunction.Free;
            }
        }

        private void SetHighside1FreqencyDudy()
        {
            lock (ControllerLocker)
            {
                CurrentFunction = ControllerFunction.WriteHighsideFreqencyDudy;
                var values = new List<byte>();
                values.AddRange(new byte[] { 0x00, (byte)HighSide1Frequency });
                values.AddRange(new byte[] { 0x00, (byte)HighSide1Duty });

                var sendBytes = ModbusOperater.GetModbusWriteValues(0x10, 0x10C.ToString(), 2, values.ToArray());

                SendData(sendBytes);
                WaitHandle.WaitOne(500);
                CurrentFunction = ControllerFunction.Free;
            }
        }

        private void SetHighside2FreqencyDudy()
        {
            lock (ControllerLocker)
            {
                CurrentFunction = ControllerFunction.WriteHighsideFreqencyDudy;
                var values = new List<byte>();
                values.AddRange(new byte[] { 0x00, (byte)HighSide2Frequency });
                values.AddRange(new byte[] { 0x00, (byte)HighSide2Duty });

                var sendBytes = ModbusOperater.GetModbusWriteValues(0x10, 0x10E.ToString(), 2, values.ToArray());

                SendData(sendBytes);
                WaitHandle.WaitOne(500);
                CurrentFunction = ControllerFunction.Free;
            }
        }

        /// <summary>
        /// 读输入捕获频率占空比
        /// </summary>
        [Description("读输入捕获频率占空比")]
        public void ReadPwm()
        {
            Pwm1CaptureDuty = -9999;
            Pwm1CaptureFrequency = -9999;
            Pwm2CaptureDuty = -9999;
            Pwm2CaptureFrequency = -9999;

            lock (ControllerLocker)
            {
                CurrentFunction = ControllerFunction.ReadPwm;

                var readBytes = ModbusOperater.GetModbusWriteValues(0x03, 0x122.ToString(), 4, new byte[] { });
                SendData(readBytes);

                WaitHandle.WaitOne(1500);
                CurrentFunction = ControllerFunction.Free;
            }
        }

        /// <summary>
        /// 读FLASH
        /// </summary>
        public bool ReadFlash()
        {
            FlashReadBytes = new byte[4096];

            lock (ControllerLocker)
            {
                CurrentFunction = ControllerFunction.ReadFlash;
                MyUdpClient.SendMsgTo(
                    new IPEndPoint(RemoteIpAddress, RemotePort),
                    new byte[] { 0x01, 0x00, 0x00, 0x01, 0x00, 0x08, 0x00, 0x69, 0x00, 0x00, 0x00, 0xFB, 0x00, 0x1B });
                var isReadOk = WaitHandle.WaitOne(2000);
                CurrentFunction = ControllerFunction.Free;

                return isReadOk;
            }
        }

        [Description("获取CAN1到CAN2转发时间")]
        public bool GetCan1ToCan2TransferTime(string msg)
        {
            Can1ToCan2TransferTime = -9999;

            try
            {
                var sp = msg.Split(',');
                var canId = Convert.ToUInt16(sp[0], 16);
                var bs = new List<byte>();

                for (var i = 0; i < sp[1].Length; i = i + 2)
                {
                    var b1 = sp[1][i];
                    var b2 = sp[1][i + 1];

                    var b =
                        Convert.ToByte(string.Format("{0}{1}", b1, b2), 16);
                    bs.Add(b);
                }

                return GetCanTransferTime(1,
                    new CanBus.CanDataPackage(canId, CanBus.CanProtocol.Can,
                        CanBus.IsExtendedCan(canId) ? CanBus.CanType.Extended : CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, bs.ToArray()));
            }
            catch (Exception)
            {
                Can1ToCan2TransferTime = -9999;
                return false;
            }
        }

        [Description("获取CAN2到CAN1转发时间")]
        public bool GetCan2ToCan1TransferTime(string msg)
        {
            Can2ToCan1TransferTime = -9999;

            try
            {
                var sp = msg.Split(',');
                var canId = Convert.ToUInt16(sp[0], 16);
                var bs = new List<byte>();

                for (var i = 0; i < sp[1].Length; i = i + 2)
                {
                    var b1 = sp[1][i];
                    var b2 = sp[1][i + 1];

                    var b =
                        Convert.ToByte(string.Format("{0}{1}", b1, b2), 16);
                    bs.Add(b);
                }

                return GetCanTransferTime(2,
                    new CanBus.CanDataPackage(canId, CanBus.CanProtocol.Can,
                        CanBus.IsExtendedCan(canId) ? CanBus.CanType.Extended : CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, bs.ToArray()));
            }
            catch (Exception)
            {
                Can2ToCan1TransferTime = -9999;
                return false;
            }
        }

        [Description("读电流上升时间")]
        public void GetCurrentRisingTime(string adIndex, ushort maxValue)
        {
            int index;
            if (int.TryParse(adIndex, out index))
            {
                if (index >= 1 || index <= 4)
                {
                    lock (ControllerLocker)
                    {
                        var str1 = string.Format("000000{0}{1}", Relay12 ? 1 : 0, Relay11 ? 1 : 0);
                        var str2 = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", Relay10 ? 1 : 0, Relay9 ? 1 : 0,
                            Relay8 ? 1 : 0, Relay7 ? 1 : 0, Relay6 ? 1 : 0, Relay5 ? 1 : 0, Relay4 ? 1 : 0,
                            Relay3 ? 1 : 0);
                        var str3 = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", Relay2 ? 1 : 0, Relay1 ? 1 : 0,
                            DoLowSide4 ? 1 : 0, DoLowSide3 ? 1 : 0, DoLowSide2 ? 1 : 0, DoLowSide1 ? 1 : 0,
                            DoHighSide2 ? 1 : 0, DoHighSide1 ? 1 : 0);

                        CurrentFunction = ControllerFunction.GetMicrotime;
                        //CurrentReadVoltIndex = voltIndex;

                        var bs = new byte[]
                        {
                            0x00, 0x12, // 帧长度
                            0x00,
                            0x70, // 功能码
                            0x00, 0x00, 0x00,0x00,
                            0x0B, // 数据段字节数
                            (byte)(index-1), // AD通道序号,
                            0x11,
                            BitConverter.GetBytes(maxValue)[1],BitConverter.GetBytes(maxValue)[0],
                            0x01,
                            0x05,
                            Convert.ToByte(str1,2), Convert.ToByte(str2,2),Convert.ToByte(str3,2), // 继电器编号
                        };
                        SendData(bs);

                        var isReadOk = WaitHandle.WaitOne(1000);
                        if (!isReadOk)
                            Mcrotime = -9999f;

                        CurrentFunction = ControllerFunction.Free;
                    }
                }
            }
        }

        [Description("LIN设置波特率")]
        public void LinSetBaudRate(int value)
        {
            if (value != 192 && value != 104)
                return;

            var listBs = new byte[6];

            var decimalVaue = BitConverter.GetBytes(int.Parse(value.ToString()));
            Array.Reverse(decimalVaue);

            var tempBytes = new List<byte>();
            var j = 4 - 2;
            for (var k = 0; k < 2; k++)
            {
                tempBytes.Add(decimalVaue[j]);
                j++;
            }

            if (tempBytes.Count == 2)
                Array.Copy(tempBytes.ToArray(), 0, listBs, 0, 2);

            //WriteFlah(listBs);

            lock (ControllerLocker)
            {
                CurrentFunction = ControllerFunction.WriteFlash;

                var sendBytes = ModbusOperater.GetModbusWriteValues(0x68, 0x7C.ToString(), (ushort)(listBs.Length / 2), listBs);
                var tempB = new List<byte>();
                tempB.AddRange(sendBytes);
                //tempB[7] = 0xFC;
                SendData(tempB);
                var isReadOk = WaitHandle.WaitOne(250);
            }
        }

        private bool GetCanTransferTime(
            int sourceCanIndex, CanBus.CanDataPackage canDataPackage)
        {
            if (MyUdpClient == null)
                return false;

            lock (ControllerLocker)
            {
                CurrentFunction = sourceCanIndex == 1
                    ? ControllerFunction.GetCan1ToCan2TransferTime
                    : ControllerFunction.GetCan2ToCan1TransferTime;

                var sendBytes = new List<byte>();
                sendBytes.AddRange(new byte[] { 0x01, 0x00, 0x00, 0x01 }); //固定值 4

                //sendBytes.AddRange(); //数据总长度 2
                sendBytes.Add(0x00); //固定值 1
                sendBytes.Add(sourceCanIndex == 1 ? (byte)0x78 : (byte)0x79); //功能码 1
                sendBytes.AddRange(new byte[] { 0x00, 0x00, 0x00, 0x00 }); //固定值 2

                //后面的数据长度
                sendBytes.Add(2 + 4 + 4 + 8 + 4);

                //添加包数据长度
                var datalen = (ushort)canDataPackage.CanDataLen;
                sendBytes.AddRange(BitConverter.GetBytes(datalen).Reverse());

                var byteId = BitConverter.GetBytes(canDataPackage.CanId).Reverse();
                //添加包数据
                sendBytes.AddRange(byteId);
                sendBytes.AddRange(new byte[] { 0x00, (byte)canDataPackage.CanType, 0x00, (byte)canDataPackage.CanFormat });

                var canData = new byte[] { 0x55, 0x55, 0x55, 0x55, 0x55, 0x55, 0x55, 0x55 };
                if (canDataPackage.CanDataLen <= canData.Length)
                    Array.Copy(canDataPackage.CanData, 0, canData, 0, canDataPackage.CanDataLen);
                sendBytes.AddRange(canData);
                //添加延迟时间
                const int timeOut = 500;
                sendBytes.AddRange(BitConverter.GetBytes((ushort)timeOut).Reverse());
                //插入数据长度
                var lengthData = (ushort)(sendBytes.Count - 4 + 2);
                sendBytes.InsertRange(4, BitConverter.GetBytes(lengthData).Reverse());
                //添加CRC校验
                sendBytes.AddRange(ValueHelper.Crc16(sendBytes));

                MyUdpClient.SendMsgTo(new IPEndPoint(RemoteIpAddress, RemotePort), sendBytes.ToArray());

                var isGot = WaitHandle.WaitOne(timeOut);

                CurrentFunction = ControllerFunction.Free;
                return isGot;
            }
        }

        public bool WriteFlah(byte[] sendValues)
        {
            lock (ControllerLocker)
            {
                CurrentFunction = ControllerFunction.WriteFlash;

                var sendBytes = ModbusOperater.GetModbusWriteValues(0x68, 0x00.ToString(), (ushort)(sendValues.Length / 2), sendValues);
                var tempB = new List<byte>();
                tempB.AddRange(sendBytes);
                tempB[7] = 0xFC;
                SendData(tempB);
                var isReadOk = WaitHandle.WaitOne(500);

                return isReadOk;
            }

        }

        /// <summary>
        /// 中值滤波,去毛刺
        /// </summary>
        /// <param name="floats"></param>
        public float[] MedianFilterAd(float[] floats)
        {
            var doubles = new double[floats.Length];
            for (var i = 0; i < floats.Length; i++)
                doubles[i] = floats[i];

            var getDoubles = WaveFilter.MedianFilter(doubles);

            var returnfs = new float[getDoubles.Length];
            for (var i = 0; i < getDoubles.Length; i++)
                returnfs[i] = (float)getDoubles[i];

            return returnfs;
        }

        /// <summary>
        /// 移动平均,曲线平滑
        /// </summary>
        /// <param name="floats"></param>
        /// <returns></returns>
        public float[] SmoothingAd(float[] floats)
        {
            var doubles = new double[floats.Length];
            for (var i = 0; i < floats.Length; i++)
                doubles[i] = floats[i];

            var getDoubles = WaveFilter.Smoothing(doubles);

            var returnfs = new float[getDoubles.Length];
            for (var i = 0; i < getDoubles.Length; i++)
                returnfs[i] = (float)getDoubles[i];

            return returnfs;
        }

        //private Dictionary<int, float[]> GetAdArrays(int count, int interval)
        //{
        //    var getAdc1Sum = new List<float>();
        //    var getAdc2Sum = new List<float>();
        //    var getAdc3Sum = new List<float>();
        //    var getAdc4Sum = new List<float>();

        //    var getAdv1Sum = new List<float>();
        //    var getAdv2Sum = new List<float>();
        //    var getAdv3Sum = new List<float>();
        //    var getAdv4Sum = new List<float>();
        //    var getAdv5Sum = new List<float>();
        //    var getAdv6Sum = new List<float>();

        //    for (var i = 0; i < count; i++)
        //    {
        //        GetCurrentVoltageDi();

        //        getAdc1Sum.Add(Current1);
        //        getAdc2Sum.Add(Current2);
        //        getAdc3Sum.Add(Current3);
        //        getAdc4Sum.Add(Current4);

        //        getAdv1Sum.Add(Voltage1);
        //        getAdv2Sum.Add(Voltage2);
        //        getAdv3Sum.Add(Voltage3);
        //        getAdv4Sum.Add(Voltage4);
        //        getAdv5Sum.Add(Voltage5);
        //        getAdv6Sum.Add(Voltage6);
        //        Thread.Sleep(interval);
        //    }

        //    var dic = new Dictionary<int, float[]>();
        //    dic.Add(1, getAdc1Sum.ToArray());
        //    dic.Add(2, getAdc2Sum.ToArray());
        //    dic.Add(3, getAdc3Sum.ToArray());
        //    dic.Add(4, getAdc4Sum.ToArray());
        //    dic.Add(5, getAdv1Sum.ToArray());
        //    dic.Add(6, getAdv2Sum.ToArray());
        //    dic.Add(7, getAdv3Sum.ToArray());
        //    dic.Add(8, getAdv4Sum.ToArray());
        //    dic.Add(9, getAdv5Sum.ToArray());
        //    dic.Add(10, getAdv6Sum.ToArray());

        //    return dic;
        //}

        #region 发送和接收
        private void _myUdpClient_PushMsgEvent(EndPoint ipEndPoint, byte[] bytes)
        {
            try
            {
                var str = bytes.Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));

                if (bytes.Length < 9)
                {
                    OnPushControllerMsg(string.Format("接收到异常数据，{0}{1},{2}", RemoteIpPort, RemoteIpPort, str));
                    return;
                }

                var debugStr = bytes.Aggregate(string.Empty,
                                (current, t) => current + ValueHelper.GetHextStrWithOx(t) + " ").TrimEnd();
                //Debug.WriteLine(debugStr);

                var functionCode = bytes[7];

                var tempLen = bytes.Length;
                var tempContent = new byte[tempLen - 2];
                Array.Copy(bytes, tempContent, tempLen - 2);

                var expectedCrc16 = ValueHelper.Crc16(tempContent).ToArray();
                var actualCrc16 = new[] { bytes[tempLen - 2], bytes[tempLen - 1] };
                if (expectedCrc16[0] != actualCrc16[0] ||
                    expectedCrc16[1] != actualCrc16[1])
                {
                    if (functionCode != 0x69)
                    {
                        OnPushControllerMsg(string.Format("接收到CRC校验失败数据，{0}{1},{2}", RemoteIpPort, RemoteIpPort, str));
                        return;
                    }
                }

                var contentBytes = new byte[2048];
                byte[] dataBytes;
                byte len;

                if (functionCode == 0x03) // DataRead,读取Modbus寄存器
                {
                    if (
                        CurrentFunction == ControllerFunction.ReadAdAndDi)
                    {
                        #region 读AD和DI
                        Array.Copy(
                           bytes.ToArray(), 9, contentBytes, 0, bytes.Length - 7 - 9); // 时间位和校验位

                        var registerCount = (ushort)(contentBytes.Length / 2);
                        dataBytes = new byte[contentBytes.Length];
                        Array.Copy(contentBytes.ToArray(), dataBytes, dataBytes.Length);

                        var floatList = new List<byte[]>();
                        for (var i = 0; i < registerCount * 2; i = i + 4)
                        {
                            var d0 = dataBytes[i + 0];
                            var d1 = dataBytes[i + 1];
                            var d2 = dataBytes[i + 2];
                            var d3 = dataBytes[i + 3];

                            var floatData = new[] { d3, d2, d1, d0 };
                            floatList.Add(floatData);
                        }

                        _current1 =
                            (float)
                                Math.Round(
                                    double.Parse(
                                        BitConverter.ToSingle(floatList[0], 0).ToString(CultureInfo.InvariantCulture)), 4,
                                    MidpointRounding.AwayFromZero);
                        _current2 = (float)
                                Math.Round(
                                    double.Parse(
                                        BitConverter.ToSingle(floatList[1], 0).ToString(CultureInfo.InvariantCulture)), 4,
                                    MidpointRounding.AwayFromZero);
                        _current3 = (float)
                                Math.Round(
                                    double.Parse(
                                        BitConverter.ToSingle(floatList[2], 0).ToString(CultureInfo.InvariantCulture)), 4,
                                    MidpointRounding.AwayFromZero);
                        _current4 = (float)
                                Math.Round(
                                    double.Parse(
                                        BitConverter.ToSingle(floatList[3], 0).ToString(CultureInfo.InvariantCulture)), 4,
                                    MidpointRounding.AwayFromZero);
                        _voltage1 = (float)
                                Math.Round(
                                    double.Parse(
                                        BitConverter.ToSingle(floatList[4], 0).ToString(CultureInfo.InvariantCulture)), 4,
                                    MidpointRounding.AwayFromZero);
                        _voltage2 = (float)
                                Math.Round(
                                    double.Parse(
                                        BitConverter.ToSingle(floatList[5], 0).ToString(CultureInfo.InvariantCulture)), 4,
                                    MidpointRounding.AwayFromZero);
                        _voltage3 = (float)
                                Math.Round(
                                    double.Parse(
                                        BitConverter.ToSingle(floatList[6], 0).ToString(CultureInfo.InvariantCulture)), 4,
                                    MidpointRounding.AwayFromZero);
                        _voltage4 = (float)
                                Math.Round(
                                    double.Parse(
                                        BitConverter.ToSingle(floatList[7], 0).ToString(CultureInfo.InvariantCulture)), 4,
                                    MidpointRounding.AwayFromZero);
                        _voltage5 = (float)
                                Math.Round(
                                    double.Parse(
                                        BitConverter.ToSingle(floatList[8], 0).ToString(CultureInfo.InvariantCulture)), 4,
                                    MidpointRounding.AwayFromZero);
                        _voltage6 = (float)
                                Math.Round(
                                    double.Parse(
                                        BitConverter.ToSingle(floatList[9], 0).ToString(CultureInfo.InvariantCulture)), 4,
                                    MidpointRounding.AwayFromZero);

                        Di1 = floatList[10][0] == 0 ? "0" : "1";
                        Di2 = floatList[11][0] == 0 ? "0" : "1";

                        WaitHandle.Set();
                        #endregion
                    }
                    else if (
                        CurrentFunction == ControllerFunction.ReadOutput)
                    {
                        #region 读高低边和继电器状态
                        Array.Copy(
                            bytes.ToArray(), 9, contentBytes, 0, bytes.Length - 7 - 9); // 时间位和校验位

                        var registerCount = (ushort)(contentBytes.Length / 2);
                        dataBytes = new byte[contentBytes.Length];
                        Array.Copy(contentBytes.ToArray(), dataBytes, dataBytes.Length);

                        var btyeList = new List<byte>();
                        for (var i = 0; i < registerCount * 2; i = i + 4)
                        {
                            var d3 = dataBytes[i + 3];
                            btyeList.Add(d3);
                        }

                        DoHighSide1 = btyeList[0] == 0x01;
                        DoHighSide2 = btyeList[1] == 0x01;
                        DoLowSide1 = btyeList[2] == 0x01;
                        DoLowSide2 = btyeList[3] == 0x01;
                        DoLowSide3 = btyeList[4] == 0x01;
                        DoLowSide4 = btyeList[5] == 0x01;

                        Relay1 = btyeList[6] == 0x01;
                        Relay2 = btyeList[7] == 0x01;
                        Relay3 = btyeList[8] == 0x01;
                        Relay4 = btyeList[9] == 0x01;
                        Relay5 = btyeList[10] == 0x01;
                        Relay6 = btyeList[11] == 0x01;
                        Relay7 = btyeList[12] == 0x01;
                        Relay8 = btyeList[13] == 0x01;
                        Relay9 = btyeList[14] == 0x01;
                        Relay10 = btyeList[15] == 0x01;
                        Relay11 = btyeList[16] == 0x01;
                        Relay12 = btyeList[17] == 0x01;

                        _lastOutputState = string.Empty;
                        _lastOutputState += DoHighSide1 ? "1" : "0";
                        _lastOutputState += DoHighSide2 ? "1" : "0";
                        _lastOutputState += DoLowSide1 ? "1" : "0";
                        _lastOutputState += DoLowSide2 ? "1" : "0";
                        _lastOutputState += DoLowSide3 ? "1" : "0";
                        _lastOutputState += DoLowSide4 ? "1" : "0";
                        _lastOutputState += Relay1 ? "1" : "0";
                        _lastOutputState += Relay2 ? "1" : "0";
                        _lastOutputState += Relay3 ? "1" : "0";
                        _lastOutputState += Relay4 ? "1" : "0";
                        _lastOutputState += Relay5 ? "1" : "0";
                        _lastOutputState += Relay6 ? "1" : "0";
                        _lastOutputState += Relay7 ? "1" : "0";
                        _lastOutputState += Relay8 ? "1" : "0";
                        _lastOutputState += Relay9 ? "1" : "0";
                        _lastOutputState += Relay10 ? "1" : "0";
                        _lastOutputState += Relay11 ? "1" : "0";
                        _lastOutputState += Relay12 ? "1" : "0";

                        WaitHandle.Set();
                        #endregion
                    }
                    else if (
                        CurrentFunction == ControllerFunction.GetAd)
                    {
                        //var adDataFromEthStr = bytes.Aggregate(string.Empty,
                        //    (current, t) => current + ValueHelper.GetHextStr(t) + " ");
                        //Debug.WriteLine("AD DATA FROM ETH: " + adDataFromEthStr);

                        #region 读AD
                        Array.Copy(
                           bytes.ToArray(), 9, contentBytes, 0, bytes.Length - 7 - 9); // 时间位和校验位

                        var registerCount = (ushort)(contentBytes.Length / 2);
                        dataBytes = new byte[contentBytes.Length];
                        Array.Copy(contentBytes.ToArray(), dataBytes, dataBytes.Length);

                        var bsList = new List<byte[]>();
                        for (var i = 0; i < registerCount * 2; i = i + 4)
                        {
                            var d0 = dataBytes[i + 0];
                            var d1 = dataBytes[i + 1];
                            var d2 = dataBytes[i + 2];
                            var d3 = dataBytes[i + 3];

                            var floatData = new[] { d3, d2, d1, d0 };
                            bsList.Add(floatData);
                        }

                        Current1Ad = ValueHelper.GetDecimal(bsList[0].Reverse().ToArray());
                        Current2Ad = ValueHelper.GetDecimal(bsList[1].Reverse().ToArray());
                        Current3Ad = ValueHelper.GetDecimal(bsList[2].Reverse().ToArray());
                        Current4Ad = ValueHelper.GetDecimal(bsList[3].Reverse().ToArray());

                        Voltage1Ad = ValueHelper.GetDecimal(bsList[4].Reverse().ToArray());
                        Voltage2Ad = ValueHelper.GetDecimal(bsList[5].Reverse().ToArray());
                        Voltage3Ad = ValueHelper.GetDecimal(bsList[6].Reverse().ToArray());
                        Voltage4Ad = ValueHelper.GetDecimal(bsList[7].Reverse().ToArray());
                        Voltage5Ad = ValueHelper.GetDecimal(bsList[8].Reverse().ToArray());
                        Voltage6Ad = ValueHelper.GetDecimal(bsList[9].Reverse().ToArray());

                        //Current1Ad = bsList[0][0];
                        //Current2Ad = bsList[1][0];
                        //Current3Ad = bsList[2][0];
                        //Current4Ad = bsList[3][0];

                        //Voltage1Ad = bsList[4][0];
                        //Voltage2Ad = bsList[5][0];
                        //Voltage3Ad = bsList[6][0];
                        //Voltage4Ad = bsList[7][0];
                        //Voltage5Ad = bsList[8][0];
                        //Voltage6Ad = bsList[9][0];

                        WaitHandle.Set();
                        #endregion
                    }
                    else if (
                        CurrentFunction == ControllerFunction.ReadHighsideFrequencyDuty)
                    {
                        #region 读输出频率占空比
                        Array.Copy(
                          bytes.ToArray(), 9, contentBytes, 0, bytes.Length - 7 - 9); // 时间位和校验位

                        var registerCount = (ushort)(contentBytes.Length / 2);
                        dataBytes = new byte[contentBytes.Length];
                        Array.Copy(contentBytes.ToArray(), dataBytes, dataBytes.Length);

                        var floatList = new List<byte[]>();
                        for (var i = 0; i < registerCount * 2; i = i + 4)
                        {
                            var d0 = dataBytes[i + 0];
                            var d1 = dataBytes[i + 1];
                            var d2 = dataBytes[i + 2];
                            var d3 = dataBytes[i + 3];

                            var floatData = new[] { d3, d2, d1, d0 };
                            floatList.Add(floatData);
                        }

                        //HighSide1Frequency = floatList[0][0];
                        //HighSide1Duty = floatList[1][0];
                        //HighSide2Frequency = floatList[2][0];
                        //HighSide2Duty = floatList[3][0];

                        HighSide1Frequency = ValueHelper.GetDecimal(floatList[0].Reverse().ToArray());
                        HighSide1Duty = ValueHelper.GetDecimal(floatList[1].Reverse().ToArray());
                        HighSide2Frequency = ValueHelper.GetDecimal(floatList[2].Reverse().ToArray());
                        HighSide2Duty = ValueHelper.GetDecimal(floatList[3].Reverse().ToArray());

                        LasthighSide1Frequency = HighSide1Frequency.ToString(CultureInfo.InvariantCulture);
                        LastHighSide1Duty = HighSide1Duty.ToString(CultureInfo.InvariantCulture);
                        LasthighSide2Frequency = HighSide2Frequency.ToString(CultureInfo.InvariantCulture);
                        LastHighSide2Duty = HighSide2Duty.ToString(CultureInfo.InvariantCulture);

                        WaitHandle.Set();
                        #endregion
                    }
                    else if (CurrentFunction == ControllerFunction.ReadPwm)
                    {
                        #region 读输入捕获频率占空比
                        Array.Copy(
                          bytes.ToArray(), 9, contentBytes, 0, bytes.Length - 7 - 9); // 时间位和校验位

                        var registerCount = (ushort)(contentBytes.Length / 2);
                        dataBytes = new byte[contentBytes.Length];
                        Array.Copy(contentBytes.ToArray(), dataBytes, dataBytes.Length);

                        var bsList = new List<byte[]>();
                        for (var i = 0; i < registerCount * 2; i = i + 4)
                        {
                            var d0 = dataBytes[i + 0];
                            var d1 = dataBytes[i + 1];
                            var d2 = dataBytes[i + 2];
                            var d3 = dataBytes[i + 3];

                            var floatData = new[] { d3, d2, d1, d0 };
                            bsList.Add(floatData);
                        }

                        Pwm1CaptureFrequency = ValueHelper.GetDecimal(bsList[0].Reverse().ToArray());
                        Pwm1CaptureDuty = ValueHelper.GetDecimal(bsList[1].Reverse().ToArray());
                        Pwm2CaptureFrequency = ValueHelper.GetDecimal(bsList[2].Reverse().ToArray());
                        Pwm2CaptureDuty = ValueHelper.GetDecimal(bsList[3].Reverse().ToArray());

                        //Pwm1CaptureFrequency = bsList[0][0];
                        //Pwm1CaptureDuty = bsList[1][0];
                        //Pwm2CaptureFrequency = bsList[2][0];
                        //Pwm2CaptureDuty = bsList[3][0];

                        WaitHandle.Set();
                        #endregion
                    }
                }
                else if (functionCode == 0x10) // DataWrite,写Modbus寄存器
                {
                    #region DataWrite,写Modbus寄存器
                    if (CurrentFunction == ControllerFunction.WriteOutput &&
                        bytes[8] == 0x01 && bytes[9] == 0x10)
                        WaitHandle.Set();
                    if (CurrentFunction == ControllerFunction.WriteHighsideFreqencyDudy &&
                        ((bytes[8] == 0x01 && bytes[9] == 0x0C) || (bytes[8] == 0x01 && bytes[9] == 0x0E)))
                        WaitHandle.Set();
                    #endregion
                }
                else if (functionCode == 0x64) // GawtwayLin,转发LIN
                {
                    #region  GawtwayLin,转发LIN
                    var linDataFromEthStr = bytes.Aggregate(string.Empty,
                        (current, t) => current + ValueHelper.GetHextStr(t) + " ");
                    //Debug.WriteLine(
                    //    string.Format("{0} LIN DATA FROM ETH: " + linDataFromEthStr, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff")));

                    Array.Copy(bytes.ToArray(), 12, contentBytes, 0, bytes.Length - 13);
                    len = contentBytes[0];
                    dataBytes = new byte[len];
                    Array.Copy(contentBytes, 1, dataBytes, 0, len);

                    var recvLinId = LinBus.ConvertLinId(dataBytes.ToList()[2]);

                    var recvDataBytes = new byte[dataBytes.Length - 7];
                    Array.Copy(dataBytes.ToArray(), 0, recvDataBytes, 0, recvDataBytes.Length);

                    if (recvDataBytes.Length == 3 && recvDataBytes[0] == 0x00 && recvDataBytes[1] == 0x55)
                    {
                        //if (CurrentFunction == ControllerFunction.GatewayLin)
                        {
                            WaitSlaveLinId = recvLinId;
                            IsWaitSlaveLinMsg = true;

                            //Console.WriteLine("Recv Slave Data Step1: {0}",
                            //    ValueHelper.GetHextStr(recvDataBytes).Replace(" ", ""));
                            return;
                        }
                    }

                    var strSlaveLinData = ValueHelper.GetHextStr(recvDataBytes).Replace(" ", "");
                    //Console.WriteLine("Recv Slave Data Step2: {0}",
                    //      ValueHelper.GetHextStr(recvDataBytes).Replace(" ", ""));
                    //Console.WriteLine("Recv Slave Data Step2: IsWaitSlaveLinMsg={0}",
                    //      IsWaitSlaveLinMsg);
                    //Console.WriteLine("Recv Slave Data Step2: CurrentFunction={0}",
                    //      CurrentFunction);

                    if (IsWaitSlaveLinMsg && recvDataBytes.Length <= 9 && !strSlaveLinData.StartsWith("0055" + ValueHelper.GetHextStr(WaitSlaveLinId)))
                    {
                        var slaveLinData = new byte[recvDataBytes.Length > 8 ? 8 : recvDataBytes.Length - 1];
                        Array.Copy(recvDataBytes, 0, slaveLinData, 0, slaveLinData.Length);
                        IsWaitSlaveLinMsg = false;

                        GatewayLin.RecviveLinDatas(new[] { new LinBus.LinDataPackage(WaitSlaveLinId, slaveLinData) });
                        return;
                    }

                    IsWaitSlaveLinMsg = false;
                    WaitSlaveLinId = 0xFF;

                    var tempLinData = new byte[recvDataBytes.Length - 3];
                    Array.Copy(recvDataBytes, 3, tempLinData, 0, tempLinData.Length);

                    var linData = new byte[tempLinData.Length > 8 ? 8 : tempLinData.Length - 1];
                    Array.Copy(tempLinData, 0, linData, 0, linData.Length);
                    GatewayLin.RecviveLinDatas(new[] { new LinBus.LinDataPackage(recvLinId, linData) });

                    if (CurrentFunction == ControllerFunction.GatewayLin)
                    {
                        if (((ControllerWith56PinGatewayLin)GatewayLin).CurrenSendMasterLinId == recvLinId)
                        {
                            WaitHandle.Set();
                        }
                    }

                    #endregion
                }
                else if (functionCode == 0x67 || functionCode == 0x65) // GawtwayCan,转发CAN
                {
                    #region GawtwayCan,转发CAN
                    //var canDataFromEthStr = bytes.Aggregate(string.Empty,
                    //    (current, t) => current + ValueHelper.GetHextStr(t) + " ");
                    //Debug.WriteLine("CAN DATA FROM ETH: " + canDataFromEthStr);

                    var canBytes = new byte[bytes.Length - 13 - 7];
                    Array.Copy(bytes.ToArray(), 13, canBytes, 0, canBytes.Length);
                    //if (canBytes.Length % (2 + 4 + 2 + 2 + 8) != 0)
                    //    return;

                    var listRecvCanPackage = new List<CanBus.CanDataPackage>();
                    //for (var j = 0; j < canBytes.Length; j++)
                    //{
                    //    var canLen = canBytes[j] * 256 + canBytes[j + 1]; // 00 08
                    //    var canId = BitConverter.ToUInt32(
                    //        new[] { canBytes[j + 5], canBytes[j + 4], canBytes[j + 3], canBytes[j + 2] }, 0);  //ID 00 00 00 00
                    //    var canType = canBytes[7];
                    //    var canFormat = canBytes[9];
                    //    var data = new List<byte>();
                    //    for (var i = j + 10; i < j + 10 + canLen; i++)
                    //        data.Add(canBytes[i]);
                    //    var recvCanPackage = new CanBus.CanDataPackage(canId, CanBus.CanProtocol.Can,
                    //        canType == 0x00 ? CanBus.CanType.Standard : CanBus.CanType.Extended,
                    //        canFormat == 0x00 ? CanBus.CanFormat.Data : CanBus.CanFormat.Remote, data.ToArray());
                    //    listRecvCanPackage.Add(recvCanPackage);
                    //}

                    var j = 0;
                    var canLen = canBytes[j] * 256 + canBytes[j + 1]; // 00 08
                    var canId = BitConverter.ToUInt32(
                        new[] { canBytes[j + 5], canBytes[j + 4], canBytes[j + 3], canBytes[j + 2] }, 0);  //ID 00 00 00 00
                    var canType = canBytes[7];
                    var canFormat = canBytes[9];
                    var data = new List<byte>();
                    for (var i = j + 10; i < j + 10 + canLen; i++)
                        data.Add(canBytes[i]);
                    var recvCanPackage = new CanBus.CanDataPackage(canId, CanBus.CanProtocol.Can,
                        canType == 0x00 ? CanBus.CanType.Standard : CanBus.CanType.Extended,
                        canFormat == 0x00 ? CanBus.CanFormat.Data : CanBus.CanFormat.Remote, data.ToArray());
                    listRecvCanPackage.Add(recvCanPackage);

                    if (functionCode == 0x67)
                        GatwayCan1.ReceiveCanDatas(listRecvCanPackage.ToArray());
                    else if (functionCode == 0x65)
                        GatwayCan2.ReceiveCanDatas(listRecvCanPackage.ToArray());
                    #endregion
                }
                else if (functionCode == 0x62) // GatewaySci_USART2
                {
                    Array.Copy(bytes.ToArray(), 12, contentBytes, 0, bytes.Length - 13);
                    //len = contentBytes[0] - 7;
                    dataBytes = new byte[contentBytes[0] - 7];
                    Array.Copy(contentBytes, 1, dataBytes, 0, contentBytes[0] - 7);

                    var sci2Str = dataBytes.Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
                    //Console.WriteLine("sci2 data: " + sci2Str);
                    GatewaySci2.OnPushSciMsg(dataBytes);
                }
                else if (functionCode == 0x66) // GatewaySci_USART6
                {

                }
                else if (functionCode == 0x63) // GatewaySci_USART3
                {
                    Array.Copy(bytes.ToArray(), 12, contentBytes, 0, bytes.Length - 13);
                    len = contentBytes[0];
                    dataBytes = new byte[len];
                    Array.Copy(contentBytes, 1, dataBytes, 0, len);

                    //var sci3Str = dataBytes.Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
                    //Console.WriteLine("sci3 data: " + sci3Str);
                    GatewaySci3.OnPushSciMsg(dataBytes);
                }
                else if (functionCode == 0x68)
                {
                    if (CurrentFunction == ControllerFunction.WriteFlash)
                    {
                        #region 写FLASH
                        WaitHandle.Set();
                        #endregion
                    }
                }
                else if (functionCode == 0x69)
                {
                    var falshDataFromEthStr = bytes.Aggregate(string.Empty,
                        (current, t) => current + ValueHelper.GetHextStr(t) + " ");

                    //Debug.WriteLine("FLASH DATA FROM ETH: " + falshDataFromEthStr);

                    if (CurrentFunction == ControllerFunction.ReadFlash)
                    {
                        #region 读FLASH
                        Array.Copy(
                           bytes.ToArray(), 9, FlashReadBytes, 0, bytes.Length - 7 - 9); // 时间位和校验位
                        WaitHandle.Set();
                        #endregion
                    }
                }
                else if (functionCode == 0x78)  // CAN1到CAN2转发时间
                {
                    if (CurrentFunction == ControllerFunction.GetCan1ToCan2TransferTime)
                    {
                        var dataTimeBytes = new[] { bytes[13], bytes[14] };
                        Can1ToCan2TransferTime =
                            BitConverter.ToUInt16(dataTimeBytes.Reverse().ToArray(), 0);
                    }
                }
                else if (functionCode == 0x79)  // CAN2到CAN1转发时间
                {
                    if (CurrentFunction == ControllerFunction.GetCan2ToCan1TransferTime)
                    {
                        var dataTimeBytes = new[] { bytes[13], bytes[14] };
                        Can2ToCan1TransferTime =
                            BitConverter.ToUInt16(dataTimeBytes.Reverse().ToArray(), 0);
                    }
                }
                else if (functionCode == 0x70)
                {
                    if (
                        CurrentFunction == ControllerFunction.GetMicrotime)
                    {
                        Mcrotime = bytes[13] * 255 + bytes[14];
                        WaitHandle.Set();
                    }
                }
                else if (functionCode == 0x72) // PL电流检测
                {
                    if (CurrentFunction == ControllerFunction.GetSingleCurr &&
                        !string.IsNullOrEmpty(CurrentReadCurrIndex))
                    {
                        var bs = new[] { bytes[16], bytes[15], bytes[14], bytes[13] };

                        var curr =
                            (float)
                                Math.Round(
                                    double.Parse(
                                        BitConverter.ToSingle(bs, 0).ToString(CultureInfo.InvariantCulture)), 4,
                                    MidpointRounding.AwayFromZero);

                        var fieldName = string.Format("Current{0}", CurrentReadCurrIndex);
                        var field = GetType().GetField(fieldName);
                        if (field != null)
                            field.SetValue(this, curr);
                        WaitHandle.Set();
                    }
                    else if (CurrentFunction == ControllerFunction.GetSingleVolt &&
                             !string.IsNullOrEmpty(CurrentReadVoltIndex))
                    {
                        var bs = new[] { bytes[16], bytes[15], bytes[14], bytes[13] };

                        var volt =
                            (float)
                                Math.Round(
                                    double.Parse(
                                        BitConverter.ToSingle(bs, 0).ToString(CultureInfo.InvariantCulture)), 4,
                                    MidpointRounding.AwayFromZero);

                        var fieldName = string.Format("Voltage{0}", CurrentReadVoltIndex);
                        var field = GetType().GetField(fieldName);
                        if (field != null)
                            field.SetValue(this, volt);
                        WaitHandle.Set();
                    }
                }
            }
            catch (Exception)
            {
                var str = bytes.Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
                OnPushControllerMsg(string.Format("接收到异常数据，{0}{1},{2}", RemoteIpPort, RemoteIpPort, str));
            }
        }

        /// <summary>
        /// 添加帧头和crc校验
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="delayMs"></param>
        /// <param name="isCrc"></param>
        private void SendData(IEnumerable<byte> bytes, int delayMs = 10, bool isCrc = true)
        {
            var sendBytes = new List<byte>();
            sendBytes.AddRange(new byte[] { 0x01, 0x00 }); // data0,data1
            sendBytes.AddRange(new byte[] { 0x00, 0x01 }); // data2,data3
            sendBytes.AddRange(bytes);
            if (isCrc)
                sendBytes.AddRange(ValueHelper.Crc16(sendBytes));

            var debugStr = sendBytes.Aggregate(string.Empty,
                               (current, t) => current + ValueHelper.GetHextStrWithOx(t) + " ").TrimEnd();
            //Debug.WriteLine(debugStr);

            MyUdpClient.SendMsgTo(
                new IPEndPoint(RemoteIpAddress, RemotePort), sendBytes.ToArray(), delayMs);

            //MyUdpClient.SendMsgTo(
            //    new IPEndPoint(RemoteIpAddress, RemotePort), bytes.ToArray(), delayMs);
        }

        public static IEnumerable<byte> GetReadValues(
            byte codeId,
            string startAddress,
            ushort regLen,
            byte stationId = 0x00)
        {
            var sendBytes = new List<byte> { stationId, codeId };

            var startAddr =
                BitConverter.GetBytes(Convert.ToUInt16(startAddress));
            Array.Reverse(startAddr);
            sendBytes.AddRange(startAddr);

            sendBytes.Add(BitConverter.GetBytes(regLen)[1]);
            sendBytes.Add(BitConverter.GetBytes(regLen)[0]);

            sendBytes.Insert(0, BitConverter.GetBytes(sendBytes.Count + 2)[0]);
            sendBytes.Insert(0, BitConverter.GetBytes(sendBytes.Count + 2)[1]);

            return sendBytes.ToArray();
        }

        #endregion

        public class ControllerWith56PinGatewayCan : CanBus
        {
            private byte CodeId { get; set; }
            private SyControllerWith56Pin ParentController { get; set; }

            public ControllerWith56PinGatewayCan(int canChannel, SyControllerWith56Pin controller)
                : base(string.Format("{0}_Can{1}", controller.RemoteIpPort, canChannel))
            {
                CodeId = canChannel == 1 ? (byte)0x67 : (byte)0x65;
                ParentController = controller;
                RegisterSendAction(SendMultipleCans);
            }

            private void SendMultipleCans(CanDataPackage[] dataPackages)
            {
                if (dataPackages == null)
                    return;

                //return;

                lock (ParentController.ControllerLocker)
                {
                    ParentController.CurrentFunction = CodeId == 0x67
                        ? ControllerFunction.GatewayCan1
                        : ControllerFunction.GatewayCan2;

                    var sendBytes = new List<byte>
                    {
                        0x00, // 固定值
                        CodeId // 功能码
                    };
                    sendBytes.AddRange(new byte[] { 0x00, 0x00, 0x00, 0x00 }); // 固定值

                    foreach (var canDataPackage in dataPackages)
                    {
                        var canDataPachageList = new List<byte>();

                        //添加包数据长度
                        var datalen = (ushort)canDataPackage.CanDataLen;
                        canDataPachageList.AddRange(BitConverter.GetBytes(datalen).Reverse());

                        //添加包数据
                        canDataPachageList.AddRange(BitConverter.GetBytes(canDataPackage.CanId).Reverse());
                        canDataPachageList.AddRange(new byte[] { 0x00, (byte)canDataPackage.CanType, 0x00, (byte)canDataPackage.CanFormat });

                        var canData = new byte[] { 0x55, 0x55, 0x55, 0x55, 0x55, 0x55, 0x55, 0x55 };
                        if (canDataPackage.CanDataLen <= canData.Length)
                            Array.Copy(canDataPackage.CanData, 0, canData, 0, canDataPackage.CanDataLen);
                        canDataPachageList.AddRange(canData);
                        sendBytes.AddRange(canDataPachageList);
                    }
                    //插入数据长度
                    var lengthData = (ushort)(sendBytes.Count - 6);
                    sendBytes.InsertRange(0, BitConverter.GetBytes(lengthData).Reverse());
                    sendBytes.Insert(8, (byte)(sendBytes.Count - 14));
                    ParentController.SendData(sendBytes.ToArray(), 3); // 延迟3MS发送

                    ParentController.CurrentFunction = ControllerFunction.Free;
                }
            }

            /// <summary>
            /// 发送标准帧
            /// </summary>
            /// <param name="canId">CAN ID</param>
            /// <param name="values">CAN消息内容</param>
            public void SendStandardCanMsg(
                uint canId, IEnumerable<byte> values)
            {
                SendCanMsg(canId, CanType.Standard, values);
            }

            /// <summary>
            /// 发送扩展帧
            /// </summary>
            /// <param name="canId">CAN ID</param>
            /// <param name="values">CAN消息内容</param>
            public void SendExtendedCanMsg(
                uint canId, IEnumerable<byte> values)
            {
                SendCanMsg(canId, CanType.Extended, values);
            }

            /// <summary>
            /// 发送CAN消息
            /// </summary>
            /// <param name="canId">CAN ID</param>
            /// <param name="canMsgType">0x00标准帧,0x01数据帧</param>
            /// <param name="canMsgValues">CAN消息内容</param>
            private void SendCanMsg(
                uint canId,
                CanType canMsgType,
                IEnumerable<byte> canMsgValues)
            {
                SendMultipleCans(
                    new[] { new CanDataPackage(canId, CanProtocol.Can, canMsgType, CanFormat.Data, canMsgValues.ToArray()) });
            }
        }

        public class ControllerWith56PinGatewayLin : LinBus
        {
            private SyControllerWith56Pin ParentController { get; set; }
            public byte CurrenSendMasterLinId { get; set; }

            public ControllerWith56PinGatewayLin(
                SyControllerWith56Pin controller)
                : base(controller.RemoteIpPort)
            {
                ParentController = controller;
                RegisterSendMasterLinFunc(SendMasterLinMessage);
                RegisterSendSlaveLinACtion(SendSlaveLinMessage);
            }

            private bool SendMasterLinMessage(LinDataPackage linDataPackage, bool isWaitSlaveLin)
            {
                if (linDataPackage == null)
                    return false;

                lock (ParentController.ControllerLocker)
                {
                    ParentController.CurrentFunction = ControllerFunction.GatewayLin;

                    ParentController.WaitHandle.Reset();
                    CurrenSendMasterLinId = ConvertLinId(linDataPackage.LinId);
                    SendLinMessage(linDataPackage.LinId, linDataPackage.LinData);

                    //var linDataFromEthStr = linDataPackage.LinData.Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t) + " ");
                    //Debug.WriteLine(string.Format("{0} MASTER LIN DATA TO ETH: " + linDataFromEthStr, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff")));

                    if (ParentController.WaitHandle.WaitOne(200))
                    {
                        CurrenSendMasterLinId = 0xFF;

                        if (!isWaitSlaveLin)
                            ParentController.CurrentFunction = ControllerFunction.Free;
                        return true;
                    }

                    CurrenSendMasterLinId = 0xFF;
                    ParentController.CurrentFunction = ControllerFunction.Free;
                    return false;
                }
            }

            private void SendSlaveLinMessage(byte slaveLinId)
            {
                lock (ParentController.ControllerLocker)
                {
                    ParentController.CurrentFunction = ControllerFunction.GatewayLin;
                    SendLinMessage(slaveLinId, new byte[] { });
                    //Debug.WriteLine(string.Format("{0} SLAVE LIN ID TO ETH: " + ValueHelper.GetHextStrWithOx(slaveLinId), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff")));
                    ParentController.CurrentFunction = ControllerFunction.Free;
                }
            }

            private void SendLinMessage(byte linId, byte[] valueBytes)
            {
                var msgValues = valueBytes ?? new byte[] { };
                var sendBytes = new List<byte>();

                if (msgValues.Length > 8)
                    return;

                sendBytes.AddRange(new byte[] { 0x00, BitConverter.GetBytes(7 + msgValues.Length + 3)[0] });
                sendBytes.AddRange(new byte[] { 0x00, 0x64 });
                sendBytes.AddRange(new byte[] { 0x00, 0x00, 0x00, 0x00, BitConverter.GetBytes(msgValues.Length + 3)[0] });
                sendBytes.Add(linId);
                sendBytes.AddRange(msgValues);
                ParentController.SendData(sendBytes);
            }
        }

        public class ControllerWith56PinGatewaySci : MySerialPort
        {
            private SyControllerWith56Pin ParentController { get; set; }
            private byte CodeId { get; set; }

            public ControllerWith56PinGatewaySci(
                int canChannel,
                SyControllerWith56Pin controller)
                : base(controller.RemoteIpPort)
            {
                ParentController = controller;

                if (canChannel == 2)
                    CodeId = 0x62;
                else if (canChannel == 3)
                    CodeId = 0x63;
                else if (canChannel == 6)
                    CodeId = 0x66;
            }

            public override void SendCommand(string str)
            {
                lock (ParentController.ControllerLocker)
                {
                    if (CodeId == 0x62)
                        ParentController.CurrentFunction = ControllerFunction.GatewaySci2;
                    else if (CodeId == 0x63)
                        ParentController.CurrentFunction = ControllerFunction.GatewaySci3;
                    else if (CodeId == 0x66)
                        ParentController.CurrentFunction = ControllerFunction.GatewaySci6;

                    var bytes = GetGatewaySciSendBytes(str);
                    ParentController.SendData(bytes);

                    ParentController.CurrentFunction = ControllerFunction.Free;
                }
            }

            public override void SendCommand(byte[] bytes, int delyMs = 25)
            {
                lock (ParentController.ControllerLocker)
                {
                    if (CodeId == 0x62)
                        ParentController.CurrentFunction = ControllerFunction.GatewaySci2;
                    else if (CodeId == 0x63)
                        ParentController.CurrentFunction = ControllerFunction.GatewaySci3;
                    else if (CodeId == 0x66)
                        ParentController.CurrentFunction = ControllerFunction.GatewaySci6;

                    ParentController.SendData(GetGatewaySciSendBytes(bytes), delyMs);
                    //ParentController.SendData(bytes); // debug

                    ParentController.CurrentFunction = ControllerFunction.Free;
                }
            }

            public override void SendBreakSyncCmd(string str)
            {
                lock (ParentController.ControllerLocker)
                {
                    if (CodeId == 0x62)
                        ParentController.CurrentFunction = ControllerFunction.GatewaySci2;
                    else if (CodeId == 0x63)
                        ParentController.CurrentFunction = ControllerFunction.GatewaySci3;
                    else if (CodeId == 0x66)
                        ParentController.CurrentFunction = ControllerFunction.GatewaySci6;

                    var bytes = GetGatewaySciSendBytes(str, true);
                    ParentController.SendData(bytes);

                    ParentController.CurrentFunction = ControllerFunction.Free;
                }
            }

            public override void SendBreakSyncCmd(byte[] bytes)
            {
                lock (ParentController.ControllerLocker)
                {
                    if (CodeId == 0x62)
                        ParentController.CurrentFunction = ControllerFunction.GatewaySci2;
                    else if (CodeId == 0x63)
                        ParentController.CurrentFunction = ControllerFunction.GatewaySci3;
                    else if (CodeId == 0x66)
                        ParentController.CurrentFunction = ControllerFunction.GatewaySci6;

                    var bs = GetGatewaySciSendBytes(bytes, true);

                    var enumerable = bs as byte[] ?? bs.ToArray();
                    //var debugStr = enumerable.Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t) + " ");
                    //Debug.WriteLine(debugStr);

                    ParentController.SendData(enumerable, isCrc: true);
                    //ParentController.SendData(bytes); // debug

                    ParentController.CurrentFunction = ControllerFunction.Free;
                }
            }

            private IEnumerable<byte> GetGatewaySciSendBytes(
                string command, bool isSendBreakSync = false)
            {
                var sendBytes = new List<byte>();
                sendBytes.AddRange(new byte[] { 0x00, isSendBreakSync ? (byte)0x61 : CodeId });

                sendBytes.AddRange(new byte[] { 0x00, 0x00, 0x00, 0x00 }); // 固定值
                var remoteSetBytes = Encoding.ASCII.GetBytes(command);
                var setBytes = remoteSetBytes;
                var setLen = BitConverter.GetBytes(setBytes.Length + 2);
                sendBytes.Add(setLen[0]);
                sendBytes.AddRange(setBytes);

                //插入数据长度
                var lengthData = (ushort)(sendBytes.Count + 2);
                sendBytes.InsertRange(0, BitConverter.GetBytes(lengthData).Reverse());
                return sendBytes;
            }

            private IEnumerable<byte> GetGatewaySciSendBytes(
                IEnumerable<byte> remoteSetBytes, bool isSendBreakSync = false)
            {
                var sendBytes = new List<byte>();
                sendBytes.AddRange(new byte[] { 0x00, isSendBreakSync ? (byte)0x61 : CodeId });

                sendBytes.AddRange(new byte[] { 0x00, 0x00, 0x00, 0x00 }); // 固定值
                var setBytes = remoteSetBytes as byte[] ?? remoteSetBytes.ToArray();
                var setLen = BitConverter.GetBytes(isSendBreakSync ? setBytes.Length + 2 : setBytes.Length + 2);
                sendBytes.Add(setLen[0]);
                sendBytes.AddRange(setBytes);

                //插入数据长度
                var lengthData = (ushort)(sendBytes.Count + 2);
                sendBytes.InsertRange(0, BitConverter.GetBytes(lengthData).Reverse());
                return sendBytes;
            }
        }

        public enum ControllerFunction
        {
            Free,

            ReadAdAndDi,

            ReadOutput,

            WriteOutput,

            GatewayCan1,

            GatewayCan2,

            GatewayLin,

            ReadFlash,

            WriteFlash,

            GetAd,

            GetSingleCurr,

            GetSingleVolt,

            ReadHighsideFrequencyDuty,

            ReadPwm,

            WriteHighsideFreqencyDudy,

            GatewaySci2,

            GatewaySci3,

            GatewaySci6,

            GetCan1ToCan2TransferTime,

            GetCan2ToCan1TransferTime,

            /// <summary>
            /// 启动时间
            /// </summary>
            GetMicrotime
        }
    }
}
