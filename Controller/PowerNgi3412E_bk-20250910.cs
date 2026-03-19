using Controller.HardwareController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Controller
{
    public class PowerNgi3412E : ControllerBase, ICcdPower
    {
        private UdpClient _udp;
        private IPAddress _currentIp;
        private const string ConnectCmd = ";:";
        IPEndPoint _iPEndPoint;

        [Description("R,电压读取")]
        public float CurrentV = float.MinValue;

        [Description("R,电流读取")]
        public float CurrentC;

        public PowerNgi3412E(string name)
            : base(name) { }

        ~PowerNgi3412E()
        {
            Dispose();
        }

        public void InitPower(string ip)
        {
            _udp = new UdpClient();
            _currentIp = IPAddress.Parse(ip);
            _udp.Client.ReceiveTimeout = 1000;
            _iPEndPoint = new IPEndPoint(_currentIp, 7000);
        }

        [Description("打开电源")]
        public void PowerOn(byte cl)
        {
            if (_udp != null)
            {
                var sendByte = new byte[] { 0x49, 0x4E, 0x53, 0x54, 0x3A, 0x4E, 0x53, 0x45, 0x4C, (byte)(0x31 + cl), 0x0D, 0x0A };
                _udp.Send(sendByte, sendByte.Length, new IPEndPoint(_currentIp, 7000));
                Thread.Sleep(100);
            }
        }

        [Description("关闭电源")]
        public void PowerOff(byte cl)
        {
            if (_udp != null)
            {
                var sendByte = new byte[] { 0x49, 0x4E, 0x53, 0x54, 0x3A, 0x4E, 0x53, 0x45, 0x4C, (byte)(0x31 + cl), 0x0D, 0x0A };
                _udp.Send(sendByte, sendByte.Length, new IPEndPoint(_currentIp, 7000));
                Thread.Sleep(100);
            }
        }

        [Description("选择通道")]
        public void SelectChannel(byte cl)
        {
            if (_udp != null)
            {
                var sendByte = new byte[] { 0x49, 0x4E, 0x53, 0x54, 0x3A, 0x4E, 0x53, 0x45, 0x4C, 0x20, (byte)(0x31 + cl), 0x0D, 0x0A };
                _udp.Send(sendByte, sendByte.Length, new IPEndPoint(_currentIp, 7000));
                Thread.Sleep(100);
            }
        }

        [Description("打开通道")]
        public void ChannelOn()
        {
            if (_udp != null)
            {
                var sendByte = new byte[] { 0x4F, 0x55, 0x54, 0x50, 0x20, 0x31, 0x0D, 0x0A };
                _udp.Send(sendByte, sendByte.Length, new IPEndPoint(_currentIp, 7000));
                Thread.Sleep(100);
            }
        }

        [Description("关闭通道")]
        public void ChannelOff()
        {
            if (_udp != null)
            {
                var sendByte = new byte[] { 0x4F, 0x55, 0x54, 0x50, 0x20, 0x30, 0x0D, 0x0A };
                _udp.Send(sendByte, sendByte.Length, new IPEndPoint(_currentIp, 7000));
                Thread.Sleep(100);
            }
        }

        [Description("设置电压")]
        public void SetV(float v)
        {
            if (_udp != null)
            {
                var sendByte = new List<byte>();
                sendByte.AddRange(new byte[] { 0x56, 0x4F, 0x4C, 0x54, 0x20 });
                sendByte.AddRange(Encoding.ASCII.GetBytes(v.ToString("f3")));
                sendByte.AddRange(new byte[] { 0x0D, 0x0A });
                _udp.Send(sendByte.ToArray(), sendByte.ToArray().Length, new IPEndPoint(_currentIp, 7000));
                Thread.Sleep(100);
            }
        }

        [Description("设置电流")]
        public void SetC(float c)
        {
            if (_udp != null)
            {
                var sendByte = new List<byte>();
                sendByte.AddRange(new byte[] { 0x43, 0x55, 0x52, 0x52, 0x20 });
                sendByte.AddRange(Encoding.ASCII.GetBytes(c.ToString("f3")));
                sendByte.AddRange(new byte[] { 0x0D, 0x0A });
                _udp.Send(sendByte.ToArray(), sendByte.ToArray().Length, new IPEndPoint(_currentIp, 7000));
                Thread.Sleep(100);
            }
        }

        public void ConnectPower(string protocolValue)
        {
            InitPower(protocolValue);
        }

        public void PowerOn()
        {
            if (_udp == null)
                return;

            const string cmd = "INST:NSEL 1" + ConnectCmd + "OUTP 1" + ConnectCmd + "INST:NSEL 2" + ConnectCmd +
                               "OUTP 2" + ConnectCmd + "INST:NSEL 3" + ConnectCmd + "OUTP 3";
            var sendBytes = Encoding.ASCII.GetBytes(cmd).ToList();
            sendBytes.AddRange(new byte[] { 0x0D, 0x0A });

            _udp.Send(sendBytes.ToArray(), sendBytes.ToArray().Length, new IPEndPoint(_currentIp, 7000));
            Thread.Sleep(100);

            //SelectChannel(0);
            //ChannelOn();
            //SelectChannel(1);
            //ChannelOn();
            //SelectChannel(2);
            //ChannelOn();
        }

        public void PowerOff()
        {
            if (_udp == null)
                return;

            const string cmd = "INST:NSEL 1" + ConnectCmd + "OUTP 0" + ConnectCmd + "INST:NSEL 2" + ConnectCmd +
                               "OUTP 0" + ConnectCmd + "INST:NSEL 3" + ConnectCmd + "OUTP 0";
            var sendBytes = Encoding.ASCII.GetBytes(cmd).ToList();
            sendBytes.AddRange(new byte[] { 0x0D, 0x0A });

            _udp.Send(sendBytes.ToArray(), sendBytes.ToArray().Length, new IPEndPoint(_currentIp, 7000));
            Thread.Sleep(100);

            //SelectChannel(0);
            //ChannelOff();
            //SelectChannel(1);
            //ChannelOff();
            //SelectChannel(2);
            //ChannelOff();
        }

        public void SetCombSerOn()
        {
            if (_udp == null)
                return;

            const string cmd = "INST:NSEL 1" + ConnectCmd + "OUTP:SERI 1";
            var sendBytes = Encoding.ASCII.GetBytes(cmd).ToList();
            sendBytes.AddRange(new byte[] { 0x0D, 0x0A });

            _udp.Send(sendBytes.ToArray(), sendBytes.ToArray().Length, new IPEndPoint(_currentIp, 7000));
            Thread.Sleep(100);
        }

        public void SetCombParaOn()
        {
            if (_udp == null)
                return;

            const string cmd = "INST:NSEL 1" + ConnectCmd + "OUTP:PARA 1";
            var sendBytes = Encoding.ASCII.GetBytes(cmd).ToList();
            sendBytes.AddRange(new byte[] { 0x0D, 0x0A });

            _udp.Send(sendBytes.ToArray(), sendBytes.ToArray().Length, new IPEndPoint(_currentIp, 7000));
            Thread.Sleep(100);
        }

        public void SetCombOff()
        {
            if (_udp == null)
                return;

            const string cmd = "INST:NSEL 1" + ConnectCmd + "OUTP:SERI 0" + ConnectCmd + "INST:NSEL 1" + ConnectCmd + "OUTP:PARA 0";
            var sendBytes = Encoding.ASCII.GetBytes(cmd).ToList();
            sendBytes.AddRange(new byte[] { 0x0D, 0x0A });

            _udp.Send(sendBytes.ToArray(), sendBytes.ToArray().Length, new IPEndPoint(_currentIp, 7000));
            Thread.Sleep(100);
        }

        public void SetVoltage1(float voltage)
        {
            if (_udp == null)
                return;

            var cmd = "INST:NSEL 1" + ConnectCmd + "VOLT " + voltage;
            var sendBytes = Encoding.ASCII.GetBytes(cmd).ToList();
            sendBytes.AddRange(new byte[] { 0x0D, 0x0A });

            _udp.Send(sendBytes.ToArray(), sendBytes.ToArray().Length, new IPEndPoint(_currentIp, 7000));
            Thread.Sleep(100);

            //SelectChannel(0);
            //SetV(voltage);
        }

        public void SetVoltage2(float voltage)
        {
            if (_udp == null)
                return;

            var cmd = "INST:NSEL 2" + ConnectCmd + "VOLT " + voltage;
            var sendBytes = Encoding.ASCII.GetBytes(cmd).ToList();
            sendBytes.AddRange(new byte[] { 0x0D, 0x0A });

            _udp.Send(sendBytes.ToArray(), sendBytes.ToArray().Length, new IPEndPoint(_currentIp, 7000));
            Thread.Sleep(100);

            //SelectChannel(1);
            //SetV(voltage);
        }

        public void SetVoltage3(float voltage)
        {
            if (_udp == null)
                return;

            var cmd = "INST:NSEL 3" + ConnectCmd + "VOLT " + voltage;
            var sendBytes = Encoding.ASCII.GetBytes(cmd).ToList();
            sendBytes.AddRange(new byte[] { 0x0D, 0x0A });

            _udp.Send(sendBytes.ToArray(), sendBytes.ToArray().Length, new IPEndPoint(_currentIp, 7000));
            Thread.Sleep(100);

            //SelectChannel(2);
            //SetV(voltage);
        }

        public void SetCurrent1(float current)
        {
            if (_udp == null)
                return;

            var cmd = "INST:NSEL 1" + ConnectCmd + "CURR " + current;
            var sendBytes = Encoding.ASCII.GetBytes(cmd).ToList();
            sendBytes.AddRange(new byte[] { 0x0D, 0x0A });

            _udp.Send(sendBytes.ToArray(), sendBytes.ToArray().Length, new IPEndPoint(_currentIp, 7000));
            Thread.Sleep(100);

            //SelectChannel(0);
            //SetC(current);
        }

        public void SetCurrent2(float current)
        {
            if (_udp == null)
                return;

            var cmd = "INST:NSEL 2" + ConnectCmd + "CURR " + current;
            var sendBytes = Encoding.ASCII.GetBytes(cmd).ToList();
            sendBytes.AddRange(new byte[] { 0x0D, 0x0A });

            _udp.Send(sendBytes.ToArray(), sendBytes.ToArray().Length, new IPEndPoint(_currentIp, 7000));
            Thread.Sleep(100);

            //SelectChannel(1);
            //SetC(current);
        }

        public void SetCurrent3(float current)
        {
            if (_udp == null)
                return;

            var cmd = "INST:NSEL 3" + ConnectCmd + "CURR " + current;
            var sendBytes = Encoding.ASCII.GetBytes(cmd).ToList();
            sendBytes.AddRange(new byte[] { 0x0D, 0x0A });

            _udp.Send(sendBytes.ToArray(), sendBytes.ToArray().Length, new IPEndPoint(_currentIp, 7000));
            Thread.Sleep(100);

            //SelectChannel(2);
            //SetC(current);
        }

        [Description("读取输入")]
        public void ReadCurrAndVolt()
        {
            SelectChannel(0);
            Thread.Sleep(100);
            string sendStr = "MEAS:VOLT?";
            List<byte> sendByte = CreatByteList(sendStr);
            _udp.Send(sendByte.ToArray(), sendByte.Count, new IPEndPoint(_currentIp, 7000));
            var value = -999d;
            value = GetReciveValue();

            CurrentV = (float)value * 1000f;
        }

        /// <summary>
        /// 根据字符串指令创建byte集合
        /// </summary>
        /// <param name="sendStr"></param>
        /// <returns></returns>
        private static List<byte> CreatByteList(string sendStr)
        {
            List<char> charList = new List<char>(sendStr.ToCharArray());
            List<byte> sendByte = charList.Select(t => (byte)t).ToList();
            sendByte.AddRange(new byte[] { 0x0D, 0x0A });
            return sendByte;
        }


        /// <summary>
        /// 获取接收到的数据
        /// </summary>
        /// <returns></returns>
        private double GetReciveValue()
        {
            double value;
            List<byte> allRcvByte = new List<byte>();
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    byte[] rcvData = _udp.Receive(ref _iPEndPoint);
                    if (rcvData != null && rcvData.Length > 0)
                    {
                        allRcvByte.AddRange(rcvData);
                    }
                    if (allRcvByte.Count > 2)
                    {
                        if (allRcvByte[allRcvByte.Count - 2] == 0x0D && allRcvByte[allRcvByte.Count - 1] == 0x0A)
                        {
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
            string rcvMsg = "";
            if (allRcvByte.Count > 0)
            {
                allRcvByte.ForEach(t => rcvMsg += Convert.ToChar(t));
            }
            rcvMsg = rcvMsg.Trim();

            double.TryParse(rcvMsg, out value);
            return value;
        }
    }
}
