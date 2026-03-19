using CommonUtility;
using Controller.HardwareController;
using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace Controller
{
    public class PowerNgi3412E : ControllerBase, ICcdPower
    {
        [Description("R/W,每次发送消息后延时一段时间")]
        public int MsgDelayMs = 120;
        private MyUdpClient _udp;
        private IPAddress _currentConnectIp;
        private const string ConnectCmd = ";:";
        IPEndPoint _iPEndPoint;
        private bool _bReading;
        private float[] _bBuffer;
        private readonly EventWaitHandle _waitHandle = new AutoResetEvent(false);
        private readonly object _lockUse = new object();

        private int _sendCount = 0;

        [Description("R,CH1电压读取")]
        public float Volt1 = float.MinValue;
        [Description("R,CH2电压读取")]
        public float Volt2 = float.MinValue;
        [Description("R,CH3电压读取")]
        public float Volt3 = float.MinValue;

        [Description("R,CH1电流读取")]
        public float Curr1 = float.MinValue;
        [Description("R,CH2电流读取")]
        public float Curr2 = float.MinValue;
        [Description("R,CH3电流读取")]
        public float Curr3 = float.MinValue;

        public PowerNgi3412E(string name)
            : base(name) { }

        ~PowerNgi3412E()
        {
            Dispose();
        }

        private string _usedIp = string.Empty;

        public void InitPower(string ip)
        {
            if (IPAddress.TryParse(ip, out _currentConnectIp))
            {
                _currentConnectIp = IPAddress.Parse(ip);
                _iPEndPoint = new IPEndPoint(_currentConnectIp, 7000);

                var host = Dns.GetHostEntry(Dns.GetHostName());

                var ipv4Addr = Array.FindAll(host.AddressList, l => l.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                var ipv4Array = Array.ConvertAll(ipv4Addr, ipp => ipp.ToString());

                var locapIp = string.Empty;
                for (var i = 0; i < ipv4Array.Length; i++)
                {
                    var ipsp = ip.Split('.');
                    var expectedIp = string.Format("{0}.{1}.{2}.", ipsp[0], ipsp[1], ipsp[2]);
                    var getipsp = ipv4Array[i].Split('.');
                    var getip = string.Format("{0}.{1}.{2}.", getipsp[0], getipsp[1], getipsp[2]);

                    if (expectedIp == getip)
                    {
                        locapIp = ipv4Array[i];
                        _usedIp = locapIp;
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(locapIp))
                {
                    //_udp = new MyUdpClient(locapIp, 7000);
                    //_udp.AddRemoteClient(ip, _iPEndPoint.Port);
                    //_udp.BeginReceive();
                    ConnectUDP();

                }
            }
        }

        private void ConnectUDP()
        {
            try
            {
                _sendCount = 0;
                if (_udp != null)
                    _udp.PushMsgEvent -= MyUdpClient_PushMsgEvent;
                _udp?.Dispose();
                _udp = null;
                _udp = new MyUdpClient(_usedIp, 7001);
                var ip = _currentConnectIp?.ToString();
                if (!string.IsNullOrEmpty(ip))
                {
                    _udp.AddRemoteClient(ip, _iPEndPoint.Port);
                    _udp.BeginReceive();
                    _udp.PushMsgEvent += MyUdpClient_PushMsgEvent;
                }
            }
            catch (Exception)
            {
                _udp = null;
                _usedIp = string.Empty;
            }
        }

        private void MyUdpClient_PushMsgEvent(EndPoint ipEndPoint, byte[] bytes)
        {
            if (_iPEndPoint is null || _currentConnectIp is null || _udp is null)
                return;

            OnPushControllerMsg(String.Format("ngi3413E_{0} recv msg: {1}", ipEndPoint.ToString(), ValueHelper.GetStringByAsciiBytes(bytes, true)));

            if (_bReading)
            {
                if (bytes != null && bytes.Length > 2 && bytes[bytes.Length - 2] == 0x0D && bytes[bytes.Length - 1] == 0x0A)
                {
                    var str = Encoding.ASCII.GetString(bytes).Trim();
                    var strSp = str.Split(';');
                    if (strSp.Length == _bBuffer.Length)
                    {
                        for (var i = 0; i < strSp.Length; i++)
                        {
                            float bFloat;
                            if (float.TryParse(strSp[i], out bFloat))
                            {
                                _bBuffer[i] = bFloat;
                            }
                            else
                            {
                                return;
                            }

                            _waitHandle.Set();
                        }
                    }
                }
            }
        }

        private void MsgDelay()
        {
            _sendCount++;
            if (_sendCount >= 500)
                ConnectUDP();

            if (MsgDelayMs <= 0)
                return;

            Thread.Sleep(MsgDelayMs);
        }

        [Description("打开电源通道1")]
        public void PowerOnCH1()
        {
            PowerOn(0);
        }

        [Description("打开电源通道2")]
        public void PowerOnCH2()
        {
            PowerOn(1);
        }

        [Description("打开电源通道3")]
        public void PowerOnCH3()
        {
            PowerOn(2);
        }

        [Description("关闭电源通道1")]
        public void PowerOffCH1()
        {
            PowerOff(0);
        }

        [Description("关闭电源通道2")]
        public void PowerOffCH2()
        {
            PowerOff(1);
        }

        [Description("关闭电源通道3")]
        public void PowerOffCH3()
        {
            PowerOff(2);
        }

        private void PowerOn(byte cl)
        {
            lock (_lockUse)
            {
                var cmd = "INST:NSEL " + (cl + 1) + ConnectCmd + "OUTP 1"; ;
                var sendBytes = Encoding.ASCII.GetBytes(cmd).ToList();
                sendBytes.AddRange(new byte[] { 0x0D, 0x0A });
                _udp?.SendMsgTo(_iPEndPoint, sendBytes.ToArray());
                MsgDelay();
            }
        }

        private void PowerOff(byte cl)
        {
            lock (_lockUse)
            {
                var cmd = "INST:NSEL " + (cl + 1) + ConnectCmd + "OUTP 0"; ;
                var sendBytes = Encoding.ASCII.GetBytes(cmd).ToList();
                sendBytes.AddRange(new byte[] { 0x0D, 0x0A });
                _udp?.SendMsgTo(_iPEndPoint, sendBytes.ToArray());
                MsgDelay();
            }
        }

        public void ConnectPower(string protocolValue)
        {
            InitPower(protocolValue);
        }

        [Description("打开电源所有通道")]
        public void PowerOn()
        {
            lock (_lockUse)
            {
                const string cmd = "INST:NSEL 1" + ConnectCmd + "OUTP 1" + ConnectCmd + "INST:NSEL 2" + ConnectCmd +
                              "OUTP 2" + ConnectCmd + "INST:NSEL 3" + ConnectCmd + "OUTP 3";
                var sendBytes = Encoding.ASCII.GetBytes(cmd).ToList();
                sendBytes.AddRange(new byte[] { 0x0D, 0x0A });
                _udp?.SendMsgTo(_iPEndPoint, sendBytes.ToArray());
                MsgDelay();
            }
        }

        [Description("关闭电源所有通道")]
        public void PowerOff()
        {
            lock (_lockUse)
            {
                const string cmd = "INST:NSEL 1" + ConnectCmd + "OUTP 0" + ConnectCmd + "INST:NSEL 2" + ConnectCmd +
                              "OUTP 0" + ConnectCmd + "INST:NSEL 3" + ConnectCmd + "OUTP 0";
                var sendBytes = Encoding.ASCII.GetBytes(cmd).ToList();
                sendBytes.AddRange(new byte[] { 0x0D, 0x0A });
                _udp?.SendMsgTo(_iPEndPoint, sendBytes.ToArray());
                MsgDelay();
            }
        }

        [Description("打开串联模式")]
        public void SetCombSerOn()
        {
            if (_udp == null)
                return;
            lock (_lockUse)
            {
                const string cmd = "INST:NSEL 1" + ConnectCmd + "OUTP:SERI 1";
                var sendBytes = Encoding.ASCII.GetBytes(cmd).ToList();
                sendBytes.AddRange(new byte[] { 0x0D, 0x0A });
                _udp?.SendMsgTo(_iPEndPoint, sendBytes.ToArray());
                MsgDelay();
            }
        }

        [Description("打开并联模式")]
        public void SetCombParaOn()
        {
            if (_udp == null)
                return;
            lock (_lockUse)
            {
                const string cmd = "INST:NSEL 1" + ConnectCmd + "OUTP:PARA 1";
                var sendBytes = Encoding.ASCII.GetBytes(cmd).ToList();
                sendBytes.AddRange(new byte[] { 0x0D, 0x0A });
                _udp?.SendMsgTo(_iPEndPoint, sendBytes.ToArray());
                MsgDelay();
            }
        }

        [Description("关闭并联模式")]
        public void SetCombOff()
        {
            if (_udp == null)
                return;
            lock (_lockUse)
            {
                const string cmd = "INST:NSEL 1" + ConnectCmd + "OUTP:SERI 0" + ConnectCmd + "INST:NSEL 1" + ConnectCmd + "OUTP:PARA 0";
                var sendBytes = Encoding.ASCII.GetBytes(cmd).ToList();
                sendBytes.AddRange(new byte[] { 0x0D, 0x0A });
                _udp?.SendMsgTo(_iPEndPoint, sendBytes.ToArray());
                MsgDelay();
            }
        }

        [Description("设置通道1电压")]
        public void SetVoltage1(float voltage)
        {
            if (_udp == null)
                return;
            lock (_lockUse)
            {
                var cmd = "INST:NSEL 1" + ConnectCmd + "VOLT " + voltage;
                var sendBytes = Encoding.ASCII.GetBytes(cmd).ToList();
                sendBytes.AddRange(new byte[] { 0x0D, 0x0A });
                _udp?.SendMsgTo(_iPEndPoint, sendBytes.ToArray());
                MsgDelay();
            }
        }

        [Description("设置通道2电压")]
        public void SetVoltage2(float voltage)
        {
            if (_udp == null)
                return;
            lock (_lockUse)
            {
                var cmd = "INST:NSEL 2" + ConnectCmd + "VOLT " + voltage;
                var sendBytes = Encoding.ASCII.GetBytes(cmd).ToList();
                sendBytes.AddRange(new byte[] { 0x0D, 0x0A });
                _udp?.SendMsgTo(_iPEndPoint, sendBytes.ToArray());
                MsgDelay();
            }
        }

        [Description("设置通道3电压")]
        public void SetVoltage3(float voltage)
        {
            if (_udp == null)
                return;
            lock (_lockUse)
            {
                var cmd = "INST:NSEL 3" + ConnectCmd + "VOLT " + voltage;
                var sendBytes = Encoding.ASCII.GetBytes(cmd).ToList();
                sendBytes.AddRange(new byte[] { 0x0D, 0x0A });
                _udp?.SendMsgTo(_iPEndPoint, sendBytes.ToArray());
                MsgDelay();
            }
        }

        [Description("设置通道1电流")]
        public void SetCurrent1(float current)
        {
            if (_udp == null)
                return;
            lock (_lockUse)
            {
                var cmd = "INST:NSEL 1" + ConnectCmd + "CURR " + current;
                var sendBytes = Encoding.ASCII.GetBytes(cmd).ToList();
                sendBytes.AddRange(new byte[] { 0x0D, 0x0A });
                _udp?.SendMsgTo(_iPEndPoint, sendBytes.ToArray());
                MsgDelay();
            }
        }

        [Description("设置通道2电流")]
        public void SetCurrent2(float current)
        {
            if (_udp == null)
                return;
            lock (_lockUse)
            {
                var cmd = "INST:NSEL 2" + ConnectCmd + "CURR " + current;
                var sendBytes = Encoding.ASCII.GetBytes(cmd).ToList();
                sendBytes.AddRange(new byte[] { 0x0D, 0x0A });
                _udp?.SendMsgTo(_iPEndPoint, sendBytes.ToArray());
                MsgDelay();
            }
        }

        [Description("设置通道3电流")]
        public void SetCurrent3(float current)
        {
            if (_udp == null)
                return;
            lock (_lockUse)
            {
                var cmd = "INST:NSEL 3" + ConnectCmd + "CURR " + current;
                var sendBytes = Encoding.ASCII.GetBytes(cmd).ToList();
                sendBytes.AddRange(new byte[] { 0x0D, 0x0A });
                _udp?.SendMsgTo(_iPEndPoint, sendBytes.ToArray());
                MsgDelay();
            }
        }

        [Description("设置所有通道电压")]
        public void SetVoltAll(float ch1Volt, float ch2Volt, float ch3Volt)
        {
            lock (_lockUse)
            {
                var cmd = "INST:NSEL 1" + ConnectCmd + "VOLT " + ch1Volt + ConnectCmd + "INST:NSEL 2" + ConnectCmd + "VOLT " + ch2Volt + ConnectCmd + "INST:NSEL 3" + ConnectCmd + "VOLT " + ch3Volt;
                var sendBytes = Encoding.ASCII.GetBytes(cmd).ToList();
                sendBytes.AddRange(new byte[] { 0x0D, 0x0A });
                _udp?.SendMsgTo(_iPEndPoint, sendBytes.ToArray());
                MsgDelay();
            }
        }

        [Description("设置所有通道电流")]
        public void SetCurrAll(float ch1Curr, float ch2Curr, float ch3Curr)
        {
            lock (_lockUse)
            {
                var cmd = "INST:NSEL 1" + ConnectCmd + "CURR " + ch1Curr + ConnectCmd + "INST:NSEL 2" + ConnectCmd + "CURR " + ch2Curr + ConnectCmd + "INST:NSEL 3" + ConnectCmd + "CURR " + ch3Curr;
                var sendBytes = Encoding.ASCII.GetBytes(cmd).ToList();
                sendBytes.AddRange(new byte[] { 0x0D, 0x0A });
                _udp?.SendMsgTo(_iPEndPoint, sendBytes.ToArray());
                MsgDelay();
            }
        }

        [Description("读取输入")]
        public void ReadCurrAndVolt()
        {
            if (ReadFunc())
                return;

            ConnectUDP();
            ReadFunc();
        }

        private bool ReadFunc()
        {
            lock (_lockUse)
            {
                var cmd = "INST:NSEL 1" + ConnectCmd + "MEAS:VOLT?" + ConnectCmd + "INST:NSEL 1" + ConnectCmd + "MEAS:CURR?" + ConnectCmd + "INST:NSEL 2" + ConnectCmd + "MEAS:VOLT?" + ConnectCmd + "INST:NSEL 2" + ConnectCmd + "MEAS:CURR?" + ConnectCmd + "INST:NSEL 3" + ConnectCmd + "MEAS:VOLT?" + ConnectCmd + "INST:NSEL 3" + ConnectCmd + "MEAS:CURR?";
                _bBuffer = new float[6];
                _bReading = true;
                var sendBytes = Encoding.ASCII.GetBytes(cmd).ToList();
                sendBytes.AddRange(new byte[] { 0x0D, 0x0A });
                _udp?.SendMsgTo(_iPEndPoint, sendBytes.ToArray());
                var isOk = _waitHandle.WaitOne(1000);
                _bReading = false;
                if (isOk)
                {
                    Volt1 = (float)Math.Round(_bBuffer[0], 4, MidpointRounding.AwayFromZero);
                    Volt2 = (float)Math.Round(_bBuffer[2], 4, MidpointRounding.AwayFromZero);
                    Volt3 = (float)Math.Round(_bBuffer[4], 4, MidpointRounding.AwayFromZero);

                    Curr1 = (float)Math.Round(_bBuffer[1] * 1000, 4, MidpointRounding.AwayFromZero);
                    Curr2 = (float)Math.Round(_bBuffer[3] * 1000, 4, MidpointRounding.AwayFromZero);
                    Curr3 = (float)Math.Round(_bBuffer[5] * 1000, 4, MidpointRounding.AwayFromZero);

                    return true;
                }
                else
                {
                    Volt1 = Volt2 = Volt3 = Curr1 = Curr2 = Curr3 = float.MinValue;

                    return false;
                }
            }
        }
    }
}
