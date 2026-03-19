using CommonUtility;
using Controller;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CheckSystem.HelperForms.VW336HeadLamp
{
    public partial class Vw336HeadLamp : UIForm
    {
        private readonly SyControllerWith56Pin _canController = new SyControllerWith56Pin("CAN控制器");
        //private readonly SyRenesasMcuControllerMaster _controller = new SyRenesasMcuControllerMaster("CAN控制器");
        private readonly KebodaLdmSeries _kebodaLdmSeries = new KebodaLdmSeries("科博达模块");
        private SerialPort _serialPort;

        private readonly object _serialPortLocker = new object();
        private readonly object _buffLocker = new object();
        private readonly List<byte> _buff = new List<byte>();

        private Thread _stateMonitorTh;
        private Thread _keepModeTh;

        public Vw336HeadLamp()
        {
            InitializeComponent();
            Load += Vw336HeadLamp_Load;
            Closed += Vw336HeadLamp_Closed;
        }

        private void Vw336HeadLamp_Closed(object sender, EventArgs e)
        {
            if (_stateMonitorTh != null)
            {
                _stateMonitorTh.Abort();
                _stateMonitorTh.Join();
            }

            if (_keepModeTh != null)
            {
                _keepModeTh.Abort();
                _keepModeTh.Join();
            }
        }

        private void Vw336HeadLamp_Load(object sender, EventArgs e)
        {
            _canController.InitRemoteIpAddress("192.168.1.28:8088");
            _kebodaLdmSeries.Can = _canController.GatwayCan1;

            //_controller.InitRemoteIpAddress("192.168.1.28:8088");
            //_kebodaLdmSeries.Can = _controller.GatewayCan1;

            try
            {
                //_serialPort.PortName = "COM1";
                //_serialPort.BaudRate = 19200;
                _serialPort = new SerialPort("COM1", 19200, Parity.None, 8, StopBits.One);
                _serialPort.Open();
            }
            catch (Exception exception)
            {
                MessageBox.Show(@"串口打开失败：" + exception.Message);
                //Console.WriteLine(exception);
                //throw;
            }
            finally
            {
                if (_serialPort != null && _serialPort.IsOpen)
                {
                    _serialPort.DataReceived += _serialPort_DataReceived;
                }
            }

            swCan.ActiveChanged += SwCan_ActiveChanged;
            swKl15.ActiveChanged += SwKl15_ActiveChanged;
            swDrlL.ActiveChanged += SWDrlL_ActiveChanged;
            swDrlR.ActiveChanged += SWDrlR_ActiveChanged;
            swPlL.ActiveChanged += SwPlL_ActiveChanged;
            swPlR.ActiveChanged += SwPlR_ActiveChanged;
            swClL.ActiveChanged += SwClL_ActiveChanged;
            swClR.ActiveChanged += SwClR_ActiveChanged;
            swHbL.ActiveChanged += SwHbL_ActiveChanged;
            swHbR.ActiveChanged += SwHbR_ActiveChanged;
            swLb.ActiveChanged += SwLb_ActiveChanged;
            swParkL.ActiveChanged += SwParkL_ActiveChanged;
            swParkR.ActiveChanged += SwParkR_ActiveChanged;
            swTurnL.ActiveChanged += SwTurnL_ActiveChanged;
            swTurnR.ActiveChanged += SwTurnR_ActiveChanged;
            swTurnRunning.ActiveChanged += SwTurnRunning_ActiveChanged;

            swKl15.Active = true;
            swCan.Active = true;

            if (_stateMonitorTh != null)
            {
                _stateMonitorTh.Abort();
                _stateMonitorTh.Join();
            }
            _stateMonitorTh = new Thread(StateMonitor) { IsBackground = true };
            _stateMonitorTh.Start();

            if (_keepModeTh != null)
            {
                _keepModeTh.Abort();
                _keepModeTh.Join();
            }
            _keepModeTh = new Thread(KeepModeWork) { IsBackground = true };
            _keepModeTh.Start();
        }

        public void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var serialPort = sender as SerialPort;
            if (serialPort == null)
                return;

            //Thread.Sleep(40);

            var len = serialPort.BytesToRead;
            var buff = new byte[len];
            serialPort.Read(buff, 0, len);

            lock (_buffLocker)
                _buff.AddRange(buff);
        }

        private void StateMonitor()
        {
            var count = 0;
            const int period = 25;

            while (_stateMonitorTh.IsAlive)
            {
                if (!_stateMonitorTh.IsAlive)
                    break;

                Thread.Sleep(period);
                count++;

                // L
                UpdateUiLedBulb(ledDrlLState, _kebodaLdmSeries.DrlLState);
                UpdateUiLedBulb(ledPlLState, _kebodaLdmSeries.PlLState);
                UpdateUiLedBulb(ledTurnLState, _kebodaLdmSeries.TurnLState);
                UpdateUiLedBulb(ledParkLState, _kebodaLdmSeries.ParkLState);

                // R
                UpdateUiLedBulb(ledDrlRState, _kebodaLdmSeries.DrlRState);
                UpdateUiLedBulb(ledPlRState, _kebodaLdmSeries.PlRState);
                UpdateUiLedBulb(ledTurnRState, _kebodaLdmSeries.TurnRState);
                UpdateUiLedBulb(ledParkRState, _kebodaLdmSeries.ParkRState);

                lock (_buffLocker)
                {
                    if (_buff.Count >= 2)
                    {
                        try
                        {
                            var str = ValueHelper.GetHextStr(_buff.ToArray()).Replace(" ", "");
                            if (str.StartsWith("55AA"))
                            {
                                var findEndIndex = str.IndexOf("0D0A", StringComparison.Ordinal);
                                if (findEndIndex != -1)
                                {
                                    if (findEndIndex % 2 == 0)
                                    {
                                        //var startIndex = 0;
                                        var endIndex = findEndIndex / 2 + 1;
                                        var len = endIndex - 0 + 1;

                                        var bs = new byte[len];
                                        Array.Copy(_buff.ToArray(), 0, bs, 0, len);
                                        for (var i = 0; i < len; i++)
                                            _buff.RemoveAt(0);

                                        if (bs.Length == 5)
                                        {
                                            var func = bs[2];
                                            //Console.WriteLine("Recv func: {0}", ValueHelper.GetHextStrWithOx(func));
                                            SwitchLed(func);
                                        }
                                        else if (bs.Length == 7)
                                        {
                                            var func = bs[2];
                                            var pendingBytes = new byte[] { bs[3], bs[4] };
                                            SwitchLed(func, pendingBytes);
                                        }
                                    }
                                    else
                                    {
                                        _buff.Clear();
                                    }
                                }
                            }
                            else
                            {
                                _buff.Clear();
                            }
                        }
                        catch (Exception a)
                        {
                            _buff.Clear();
                        }
                    }
                }

                if (count == 10) // 250ms 串口发送一次状态信息
                {
                    count = 0;

                    var sendBs = new List<byte>
                    {
                        0x55, 0xAA,

                        GetStateByte( _kebodaLdmSeries.DrlLState),
                        GetStateByte( _kebodaLdmSeries.PlLState),
                        GetStateByte( _kebodaLdmSeries.TurnLState),
                        GetStateByte( _kebodaLdmSeries.ParkLState),

                        GetStateByte( _kebodaLdmSeries.DrlRState),
                        GetStateByte( _kebodaLdmSeries.PlRState),
                        GetStateByte( _kebodaLdmSeries.TurnRState),
                        GetStateByte( _kebodaLdmSeries.ParkRState),

                        0x0D,0x0A
                    };

                    lock (_serialPortLocker)
                    {
                        if (!_serialPort.IsOpen)
                            continue;

                        try
                        {
                            _serialPort.Write(sendBs.ToArray(), 0, sendBs.Count);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                }
            }
        }

        private void SwitchLed(byte func, byte[] pendingBytes = null)
        {
            switch (func)
            {
                default:
                    break;

                case 0x00:
                    UpdateSwitch(swLb, true);
                    break;

                case 0x30:
                    UpdateSwitch(swLb, false);
                    break;

                case 0x01:
                    UpdateSwitch(swHbL, true);
                    break;

                case 0x31:
                    UpdateSwitch(swHbL, false);
                    break;

                case 0x02:
                    UpdateSwitch(swHbR, true);
                    break;

                case 0x32:
                    UpdateSwitch(swHbR, false);
                    break;

                case 0x03:
                    UpdateSwitch(swDrlL, true);
                    break;

                case 0x33:
                    UpdateSwitch(swDrlL, false);
                    break;

                case 0x04:
                    UpdateSwitch(swDrlR, true);
                    break;

                case 0x34:
                    UpdateSwitch(swDrlR, false);
                    break;

                case 0x05:
                    UpdateSwitch(swPlL, true);
                    break;

                case 0x35:
                    UpdateSwitch(swPlL, false);
                    break;

                case 0x06:
                    UpdateSwitch(swPlR, true);
                    break;

                case 0x36:
                    UpdateSwitch(swPlR, false);
                    break;

                case 0x07:
                    UpdateSwitch(swClL, true);
                    break;

                case 0x37:
                    UpdateSwitch(swClL, false);
                    break;

                case 0x08:
                    UpdateSwitch(swClR, true);
                    break;

                case 0x38:
                    UpdateSwitch(swClR, false);
                    break;

                case 0x09:
                    UpdateSwitch(swTurnRunning, true);
                    break;

                case 0x39:
                    UpdateSwitch(swTurnRunning, false);
                    break;

                case 0x0A:
                    rbTurnLHolding.Checked = true;
                    break;

                case 0x0B:
                    rbTurnLBlink.Checked = true;
                    break;

                case 0x0C:
                    UpdateSwitch(swTurnL, true);
                    break;

                case 0x3C:
                    UpdateSwitch(swTurnL, false);
                    break;

                case 0x0D:
                    UpdateSwitch(swTurnR, true);
                    break;

                case 0x3D:
                    UpdateSwitch(swTurnR, false);
                    break;

                case 0x0E:
                    UpdateSwitch(swParkL, true);
                    break;

                case 0x3E:
                    UpdateSwitch(swParkL, false);
                    break;

                case 0x0F:
                    UpdateSwitch(swParkR, true);
                    break;

                case 0x3F:
                    UpdateSwitch(swParkR, false);
                    break;

                case 0x10:
                    if (InvokeRequired)
                        BeginInvoke(new Action(() => { btnReadVersionL_Click(null, null); }));
                    break;

                case 0x11:
                    if (InvokeRequired)
                        BeginInvoke(new Action(() => { btnReadVersionR_Click(null, null); }));
                    break;

                case 0x12:
                    UpdateSwitch(swLeftMotorReset, true);
                    break;

                case 0x42:
                    UpdateSwitch(swLeftMotorReset, false);
                    break;

                case 0x13:
                    UpdateSwitch(swRightMotorReset, true);
                    break;

                case 0x43:
                    UpdateSwitch(swRightMotorReset, false);
                    break;

                case 0x14:
                    UpdateSwitch(swMotorCycle, true);
                    break;

                case 0x44:
                    UpdateSwitch(swMotorCycle, false);
                    break;

                case 0x15:
                    if (InvokeRequired)
                        BeginInvoke(new Action(() =>
                        {
                            if (pendingBytes != null && pendingBytes.Length == 2)
                            {
                                var value = pendingBytes[0] * 256 + pendingBytes[1];
                                if (value > 2046)
                                {
                                    value = 2046;
                                }

                                txtLeftMotorPos.Value = value;
                            }
                        }));
                    break;

                case 0x16:
                    BeginInvoke(new Action(() =>
                    {
                        if (pendingBytes != null && pendingBytes.Length == 2)
                        {
                            var value = pendingBytes[0] * 256 + pendingBytes[1];
                            if (value > 2046)
                            {
                                value = 2046;
                            }

                            txtRightMotorPos.Value = value;
                        }
                    }));
                    break;
            }
        }

        private delegate void UpdateSwitchDelegate(UISwitch control, bool value);

        private void UpdateSwitch(UISwitch control, bool value)
        {
            var de = new UpdateSwitchDelegate(UpdateSwitch);
            if (control.InvokeRequired)
            {
                Invoke(de, control, value);
            }
            else
            {
                if (control.Active != value)
                {
                    control.Active = value;
                }
            }
        }

        private delegate void UpdateUiLedBulbDelegate(UILedBulb control, string value);

        private void UpdateUiLedBulb(UILedBulb control, string value)
        {
            var updateTxtVoltValueDelegate = new UpdateUiLedBulbDelegate(UpdateUiLedBulb);

            if (control.InvokeRequired)
            {
                Invoke(updateTxtVoltValueDelegate, control, value);
            }
            else
            {
                if (string.IsNullOrEmpty(value))
                {
                    control.Color = Color.Gray;
                }
                else
                {
                    if (value == 0.ToString())
                    {
                        control.Color = Color.DarkGoldenrod;
                    }
                    else
                    {
                        control.Color = Color.Green;
                    }
                }
            }
        }

        /// <summary>
        /// 00代表未接收到模块发送的对应灯板的状态信息；01代表模块发送的对应灯板的状态为打开；02代表模块发送的对应灯板的状态为关闭。
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private static byte GetStateByte(string state)
        {
            if (string.IsNullOrEmpty(state))
            {
                return 0x00;
            }
            else
            {
                if (state == 0.ToString())
                {
                    return 0x02;
                }
                else
                {
                    return 0x01;
                }
            }
        }

        private void SwTurnRunning_ActiveChanged(object sender, EventArgs e)
        {
            if (swTurnRunning.Active)
            {
                _kebodaLdmSeries.LCM_Blinkerwischen_Anf_On();
            }
            else
            {
                _kebodaLdmSeries.LCM_Blinkerwischen_Anf_Off();
            }
        }

        private void SwLb_ActiveChanged(object sender, EventArgs e)
        {
            if (swLb.Active)
            {
                _kebodaLdmSeries.LLP_L_LowBeamOn();
            }
            else
            {
                _kebodaLdmSeries.LLP_L_LowBeamOff();
            }
        }

        private void SwHbR_ActiveChanged(object sender, EventArgs e)
        {
            if (swHbR.Active)
            {
                _kebodaLdmSeries.RightHighBeamOn();
            }
            else
            {
                _kebodaLdmSeries.RightHighBeamOff();
            }
        }

        private void SwHbL_ActiveChanged(object sender, EventArgs e)
        {
            if (swHbL.Active)
            {
                _kebodaLdmSeries.LeftHighBeamOn();
            }
            else
            {
                _kebodaLdmSeries.LeftHighBeamOff();
            }
        }

        private void SwClR_ActiveChanged(object sender, EventArgs e)
        {
            if (swClR.Active)
            {
                _kebodaLdmSeries.RightCornerOn();
            }
            else
            {
                _kebodaLdmSeries.RightCornerOff();
            }
        }

        private void SwClL_ActiveChanged(object sender, EventArgs e)
        {
            if (swClL.Active)
            {
                _kebodaLdmSeries.LeftCornerOn();
            }
            else
            {
                _kebodaLdmSeries.LeftCornerOff();
            }
        }

        private void SwPlR_ActiveChanged(object sender, EventArgs e)
        {
            if (swPlR.Active)
            {
                _kebodaLdmSeries.RightPlOn();
            }
            else
            {
                _kebodaLdmSeries.RightPlOff();
            }
        }

        private void SwPlL_ActiveChanged(object sender, EventArgs e)
        {
            if (swPlL.Active)
            {
                _kebodaLdmSeries.LeftPlOn();
            }
            else
            {
                _kebodaLdmSeries.LeftPlOff();
            }
        }

        private void SWDrlR_ActiveChanged(object sender, EventArgs e)
        {
            if (swDrlR.Active)
            {
                _kebodaLdmSeries.RightDrlOn();
            }
            else
            {
                _kebodaLdmSeries.RightDrlOff();
            }
        }

        private void SWDrlL_ActiveChanged(object sender, EventArgs e)
        {
            if (swDrlL.Active)
            {
                _kebodaLdmSeries.LeftDrlOn();
            }
            else
            {
                _kebodaLdmSeries.LeftDrlOff();
            }
        }

        private void SwParkR_ActiveChanged(object sender, EventArgs e)
        {
            if (swParkR.Active)
            {
                _kebodaLdmSeries.RightParkOn();
            }
            else
            {
                _kebodaLdmSeries.RightParkOff();
            }
        }

        private void SwParkL_ActiveChanged(object sender, EventArgs e)
        {
            if (swParkL.Active)
            {
                _kebodaLdmSeries.LeftParkOn();
            }
            else
            {
                _kebodaLdmSeries.LeftParkOff();
            }
        }

        private void SwKl15_ActiveChanged(object sender, EventArgs e)
        {
            _kebodaLdmSeries.Kl15 = swKl15.Active;
        }

        private void SwCan_ActiveChanged(object sender, EventArgs e)
        {
            if (swCan.Active)
            {
                _kebodaLdmSeries.ModuleAwake();
            }
            else
            {
                _kebodaLdmSeries.ModuleSleep();
            }
        }

        private void btnReadVersionL_Click(object sender, EventArgs e)
        {
            _kebodaLdmSeries.ReadLeftSwVersion();
            lblVersionL.Text = _kebodaLdmSeries.LeftSwVersion;
            ResponseVersion(true, _kebodaLdmSeries.LeftSwVersion);
        }

        private void btnReadVersionR_Click(object sender, EventArgs e)
        {
            _kebodaLdmSeries.ReadRightSwVersion();
            lblVersionR.Text = _kebodaLdmSeries.RightSwVersion;
            ResponseVersion(false, _kebodaLdmSeries.RightSwVersion);
        }

        private void ResponseVersion(bool isL, string value)
        {
            lock (_serialPortLocker)
            {
                if (!_serialPort.IsOpen)
                    return;

                try
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        var asciiBs = Encoding.ASCII.GetBytes(value);

                        var toSendBs = new List<byte> { 0x55, 0xAA, isL ? (byte)0x10 : (byte)0x11 };

                        for (var i = 0; i < 10; i++)
                            toSendBs.Add(0xFF);

                        if (asciiBs.Length >= 10)
                        {
                            for (var i = 0; i < 10; i++)
                            {
                                toSendBs[i + 3] = asciiBs[i];
                            }
                        }
                        else
                        {
                            for (var i = 0; i < asciiBs.Length; i++)
                            {
                                toSendBs[i + 3] = asciiBs[i];
                            }
                        }

                        toSendBs.AddRange(new byte[] { 0x0D, 0x0A });

                        _serialPort.Write(toSendBs.ToArray(), 0, toSendBs.Count);
                    }
                    else
                    {
                        var toSendBs = new List<byte> { 0x55, 0xAA, isL ? (byte)0x10 : (byte)0x11 };

                        for (var i = 0; i < 10; i++)
                            toSendBs.Add(0xFF);

                        toSendBs.AddRange(new byte[] { 0x0D, 0x0A });

                        _serialPort.Write(toSendBs.ToArray(), 0, toSendBs.Count);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private void SwTurnR_ActiveChanged(object sender, EventArgs e)
        {
            if (swTurnR.Active)
            {
                _kebodaLdmSeries.RightTurnOn();
            }
            else
            {
                _kebodaLdmSeries.RightTurnOff();
            }
        }

        private void SwTurnL_ActiveChanged(object sender, EventArgs e)
        {
            if (swTurnL.Active)
            {
                _kebodaLdmSeries.LeftTurnOn();
            }
            else
            {
                _kebodaLdmSeries.LeftTurnOff();
            }
        }

        private void turn_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTurnLBlink.Checked)
            {
                _kebodaLdmSeries.TurnBlinkEnable();
            }
            else if (rbTurnLHolding.Checked)
            {
                _kebodaLdmSeries.TurnBlinkDisable();
            }
        }

        private void swMotorCycle_ActiveChanged(object sender, EventArgs e)
        {
            if (swMotorCycle.Active)
            {
                _kebodaLdmSeries.StartMotorCycle();
            }
            else
            {
                _kebodaLdmSeries.EndMotorCycle();
            }
        }

        private void swLeftMotorReset_ActiveChanged(object sender, EventArgs e)
        {
            if (swLeftMotorReset.Active)
            {
                _kebodaLdmSeries.LeftMotorResetOn();
                txtLeftMotorPos.Value = 2046;
            }
            else
            {
                _kebodaLdmSeries.LeftMotorResetOff();
            }
        }

        private void swRightMotorReset_ActiveChanged(object sender, EventArgs e)
        {
            if (swRightMotorReset.Active)
            {
                _kebodaLdmSeries.RightMotorResetOn();
                txtRightMotorPos.Value = 2046;
            }
            else
            {
                _kebodaLdmSeries.RightMotorResetOff();
            }
        }

        private void txtRightMotorPos_ValueChanged(object sender, EventArgs e)
        {
            var value = (ushort)txtRightMotorPos.Value;
            _kebodaLdmSeries.RightMotorRunTo((double)((value * 0.5f - 1023)));
        }

        private void txtLeftMotorPos_ValueChanged(object sender, EventArgs e)
        {
            var value = (ushort)txtLeftMotorPos.Value;
            _kebodaLdmSeries.LeftMotorRunTo((double)((value * 0.5f - 1023)));
        }

        #region 新增白天/夜晚模式切换

        /// <summary>
        /// 0=off,1=day2night,2=night2day
        /// </summary>
        private int _keepMode;

        #endregion

        private void rdKeepMode_CheckedChangeEvent(object sender, EventArgs e)
        {
            if (rdKeepModeOff.Checked)
            {
                _keepMode = 0;
                txtClOffTimeKeep.Enabled = true;
                txtClOnTimeKeep.Enabled = true;
                txtNightTimeKeep.Enabled = true;
                txtDayTimeKeep.Enabled = true;

                txtCurrentKeepMode.Text = string.Empty;
                txtCurrentKeepModeClRest.Text = string.Empty;
                _currentKeepMode = MyEnum.Off;

                // 关灯
                swLb.Active = false; // lb
                swHbL.Active = swHbR.Active = false; // hb
                swDrlL.Active = swDrlR.Active = false; // drl
                swParkL.Active = swParkR.Active = false; // park
                swPlL.Active = swPlR.Active = false; // pl
                swClL.Active = swClR.Active = false; // cl
                swTurnR.Active = swTurnL.Active = false; // turn
            }
            else if (rdKeepModeDay2Night.Checked)
            {
                _keepMode = 1;
                txtClOffTimeKeep.Enabled = false;
                txtClOnTimeKeep.Enabled = false;
                txtNightTimeKeep.Enabled = false;
                txtDayTimeKeep.Enabled = false;

                txtCurrentKeepMode.Text = string.Empty;
                txtCurrentKeepModeClRest.Text = string.Empty;

                _keepDayNightStartTime = HighPrecisionTimer.GetTimestamp();
                _keepClOnOffStartTime = HighPrecisionTimer.GetTimestamp();
                _currentKeepMode = MyEnum.Day;

                // 关灯
                swLb.Active = false; // lb
                swHbL.Active = swHbR.Active = false; // hb
                swDrlL.Active = swDrlR.Active = false; // drl
                swParkL.Active = swParkR.Active = false; // park
                swPlL.Active = swPlR.Active = false; // pl
                swClL.Active = swClR.Active = false; // cl
                swTurnR.Active = swTurnL.Active = false; // turn
            }
            else if (rdKeepModeNight2Day.Checked)
            {
                _keepMode = 2;
                txtClOffTimeKeep.Enabled = false;
                txtClOnTimeKeep.Enabled = false;
                txtNightTimeKeep.Enabled = false;
                txtDayTimeKeep.Enabled = false;

                txtCurrentKeepMode.Text = string.Empty;
                txtCurrentKeepModeClRest.Text = string.Empty;

                _keepDayNightStartTime = HighPrecisionTimer.GetTimestamp();
                _keepClOnOffStartTime = HighPrecisionTimer.GetTimestamp();
                _currentKeepMode = MyEnum.NightWithClOn;

                // 关灯,开cl/lb/hb
                swLb.Active = true; // lb
                swHbL.Active = swHbR.Active = true; // hb
                swDrlL.Active = swDrlR.Active = false; // drl
                swParkL.Active = swParkR.Active = false; // park
                swPlL.Active = swPlR.Active = false; // pl
                swClL.Active = swClR.Active = true; // cl
                swTurnR.Active = swTurnL.Active = false; // turn
            }
        }

        private long _keepDayNightStartTime;
        private long _keepClOnOffStartTime;
        private MyEnum _currentKeepMode = MyEnum.Off;

        private void KeepModeWork()
        {
            while (_keepModeTh.IsAlive)
            {
                if (!_keepModeTh.IsAlive)
                    break;

                Thread.Sleep(50);

                if (_keepMode == 0)
                    continue;

                if (_keepMode == 1 || _keepMode == 2)
                {
                    if (_currentKeepMode == MyEnum.Day)
                    {
                        UpdateKeepModeTxt(txtCurrentKeepMode, "白天模式");

                        var timeIntervalMs = HighPrecisionTimer.GetTimestampIntervalMs(_keepDayNightStartTime, HighPrecisionTimer.GetTimestamp());
                        var timeIntervalS = timeIntervalMs / 1000;

                        var timeResetS = txtDayTimeKeep.Value - (decimal)timeIntervalS;
                        UpdateKeepModeTxt(txtCurrentKeepModeDayNightRest, timeResetS.ToString(CultureInfo.InvariantCulture));

                        if (timeResetS <= 0)
                        {
                            _keepClOnOffStartTime = _keepDayNightStartTime = HighPrecisionTimer.GetTimestamp();
                            _currentKeepMode = MyEnum.NightWithClOn;

                            BeginInvoke(new Action(() =>
                            {
                                // 关灯,开cl/lb/hb
                                swLb.Active = true; // lb
                                swHbL.Active = swHbR.Active = true; // hb
                                swDrlL.Active = swDrlR.Active = false; // drl
                                swParkL.Active = swParkR.Active = false; // park
                                swPlL.Active = swPlR.Active = false; // pl
                                swClL.Active = swClR.Active = true; // cl
                                swTurnR.Active = swTurnL.Active = false; // turn
                            }));
                        }
                    }
                    else if (_currentKeepMode == MyEnum.NightWithClOn)
                    {
                        UpdateKeepModeTxt(txtCurrentKeepMode, "夜晚模式/角灯亮");

                        var timeNightIntervalMs = HighPrecisionTimer.GetTimestampIntervalMs(_keepDayNightStartTime, HighPrecisionTimer.GetTimestamp());
                        var timeNightIntervalS = timeNightIntervalMs / 1000;
                        var timeResetSNight = txtNightTimeKeep.Value - (decimal)timeNightIntervalS;

                        var timeClOnIntervalMs = HighPrecisionTimer.GetTimestampIntervalMs(_keepClOnOffStartTime, HighPrecisionTimer.GetTimestamp());
                        var timeClOnIntervalS = timeClOnIntervalMs / 1000;
                        var timeResetSClOn = txtClOnTimeKeep.Value - (decimal)timeClOnIntervalS;

                        UpdateKeepModeTxt(txtCurrentKeepModeClRest, timeResetSClOn.ToString(CultureInfo.InvariantCulture));
                        UpdateKeepModeTxt(txtCurrentKeepModeDayNightRest, timeResetSNight.ToString(CultureInfo.InvariantCulture));

                        if (timeResetSNight <= 0)
                        {
                            UpdateKeepModeTxt(txtCurrentKeepModeClRest, string.Empty);
                            _keepDayNightStartTime = HighPrecisionTimer.GetTimestamp();
                            _currentKeepMode = MyEnum.Day;

                            BeginInvoke(new Action(() =>
                            {
                                // 关灯
                                swLb.Active = false; // lb
                                swHbL.Active = swHbR.Active = false; // hb
                                swDrlL.Active = swDrlR.Active = false; // drl
                                swParkL.Active = swParkR.Active = false; // park
                                swPlL.Active = swPlR.Active = false; // pl
                                swClL.Active = swClR.Active = false; // cl
                                swTurnR.Active = swTurnL.Active = false; // turn
                            }));
                        }
                        else
                        {
                            if (timeResetSClOn <= 0)
                            {
                                _keepClOnOffStartTime = HighPrecisionTimer.GetTimestamp();
                                _currentKeepMode = MyEnum.NightWithClOff;

                                BeginInvoke(new Action(() =>
                                {
                                    // 关灯,开lb/hb
                                    swLb.Active = true; // lb
                                    swHbL.Active = swHbR.Active = true; // hb
                                    swDrlL.Active = swDrlR.Active = false; // drl
                                    swParkL.Active = swParkR.Active = false; // park
                                    swPlL.Active = swPlR.Active = false; // pl
                                    swClL.Active = swClR.Active = false; // cl
                                    swTurnR.Active = swTurnL.Active = false; // turn
                                }));
                            }
                        }
                    }
                    else if (_currentKeepMode == MyEnum.NightWithClOff)
                    {
                        UpdateKeepModeTxt(txtCurrentKeepMode, "夜晚模式/角灯灭");

                        var timeNightIntervalMs = HighPrecisionTimer.GetTimestampIntervalMs(_keepDayNightStartTime, HighPrecisionTimer.GetTimestamp());
                        var timeNightIntervalS = timeNightIntervalMs / 1000;
                        var timeResetSNight = txtNightTimeKeep.Value - (decimal)timeNightIntervalS;

                        var timeClOffIntervalMs = HighPrecisionTimer.GetTimestampIntervalMs(_keepClOnOffStartTime, HighPrecisionTimer.GetTimestamp());
                        var timeClOffIntervalS = timeClOffIntervalMs / 1000;
                        var timeResetSClOff = txtClOffTimeKeep.Value - (decimal)timeClOffIntervalS;

                        UpdateKeepModeTxt(txtCurrentKeepModeClRest, timeResetSClOff.ToString(CultureInfo.InvariantCulture));
                        UpdateKeepModeTxt(txtCurrentKeepModeDayNightRest, timeResetSNight.ToString(CultureInfo.InvariantCulture));

                        if (timeResetSNight <= 0)
                        {
                            UpdateKeepModeTxt(txtCurrentKeepModeClRest, string.Empty);
                            _keepDayNightStartTime = HighPrecisionTimer.GetTimestamp();
                            _currentKeepMode = MyEnum.Day;

                            BeginInvoke(new Action(() =>
                            {
                                // 关灯
                                swLb.Active = false; // lb
                                swHbL.Active = swHbR.Active = false; // hb
                                swDrlL.Active = swDrlR.Active = false; // drl
                                swParkL.Active = swParkR.Active = false; // park
                                swPlL.Active = swPlR.Active = false; // pl
                                swClL.Active = swClR.Active = false; // cl
                                swTurnR.Active = swTurnL.Active = false; // turn
                            }));
                        }
                        else
                        {
                            if (timeResetSClOff <= 0)
                            {
                                _keepClOnOffStartTime = HighPrecisionTimer.GetTimestamp();
                                _currentKeepMode = MyEnum.NightWithClOn;

                                BeginInvoke(new Action(() =>
                                {
                                    // 关灯,开cl/lb/hb
                                    swLb.Active = true; // lb
                                    swHbL.Active = swHbR.Active = true; // hb
                                    swDrlL.Active = swDrlR.Active = false; // drl
                                    swParkL.Active = swParkR.Active = false; // park
                                    swPlL.Active = swPlR.Active = false; // pl
                                    swClL.Active = swClR.Active = true; // cl
                                    swTurnR.Active = swTurnL.Active = false; // turn
                                }));
                            }
                        }
                    }
                }
            }
        }

        private delegate void UpdateKeepModeTxtDelegate(UIMarkLabel control, string value);

        private void UpdateKeepModeTxt(UIMarkLabel control, string value)
        {
            var de = new UpdateKeepModeTxtDelegate(UpdateKeepModeTxt);
            if (control.InvokeRequired)
            {
                Invoke(de, control, value);
            }
            else
            {
                control.Text = value;
            }
        }

        internal enum MyEnum
        {
            Off,

            Day,

            NightWithClOn,

            NightWithClOff
        }

        private void swMatrix_ActiveChanged(object sender, EventArgs e)
        {
            if (swMatrix.Active)
            {
                _kebodaLdmSeries.IsHaveMatrix = true;
                _kebodaLdmSeries.HbAllOn("63");
            }
            else
            {
                _kebodaLdmSeries.HbAllOn("0");
                _kebodaLdmSeries.IsHaveMatrix = false;
            }
        }
    }
}
