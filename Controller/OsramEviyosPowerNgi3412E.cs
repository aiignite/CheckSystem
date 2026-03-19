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
    public class OsramEviyosPowerNgi3412E : ControllerBase
    {
        private UdpClient _udp;
        private IPAddress _currentIp;
        IPEndPoint _iPEndPoint;

        [Description("R,电压读取")]
        public float CurrentV;

        [Description("R,电流读取")]
        public float CurrentC;

        [Description("R,电流读取")]
        public float CurrentC2;



        public float readCh1Curr = -9999;
        public float readCh2Curr = -9999;



        object Object = new object();

        /// <summary>
        /// 读取选定通道的输出电流
        /// </summary>
        /// <returns></returns>
        [Description("读取选定通道的输出电流")]
        public string ReadOutputC3(byte cl)
        {
            lock (Object)
            {
                if (cl == 0)
                {
                    readCh1Curr = -9999;
                }

                if (cl == 1)
                {
                    readCh2Curr = -9999;
                }

                SelectChannel(cl);
                Thread.Sleep(100);
                string sendStr = "MEAS:CURR?";
                List<byte> sendByte = CreatByteList(sendStr);
                _udp.Send(sendByte.ToArray(), sendByte.Count, new IPEndPoint(_currentIp, 7000));
                double value = -999;
                value = GetReciveValue();

                CurrentC = (float)value * 1000f;

                if (CurrentC < 10)
                {

                    SelectChannel(cl);
                    Thread.Sleep(100);
                    sendStr = "MEAS:CURR?";
                    sendByte = CreatByteList(sendStr);
                    _udp.Send(sendByte.ToArray(), sendByte.Count, new IPEndPoint(_currentIp, 7000));
                    value = -999;
                    value = GetReciveValue();

                    CurrentC = (float)value * 1000f;

                }

                if (CurrentC < 10)
                {

                    SelectChannel(cl);
                    Thread.Sleep(100);
                    sendStr = "MEAS:CURR?";
                    sendByte = CreatByteList(sendStr);
                    _udp.Send(sendByte.ToArray(), sendByte.Count, new IPEndPoint(_currentIp, 7000));
                    value = -999;
                    value = GetReciveValue();

                    CurrentC = (float)value * 1000f;
                }
                if (cl == 0)
                {
                    readCh1Curr = CurrentC;
                }

                if (cl == 1)
                {
                    readCh2Curr = CurrentC;
                }

                return value.ToString();

            }
        }


        /// <summary>
        /// 读取选定通道的输出电流
        /// </summary>
        /// <returns></returns>
        [Description("读取选定通道的输出电流")]
        public string ReadOutputC()
        {
            SelectChannel(0);
            Thread.Sleep(100);
            string sendStr = "MEAS:CURR?";
            List<byte> sendByte = CreatByteList(sendStr);
            _udp.Send(sendByte.ToArray(), sendByte.Count, new IPEndPoint(_currentIp, 7000));
            double value = -999;
            value = GetReciveValue();

            CurrentC = (float)value * 1000f;

            if (CurrentC == 0)
            {

                SelectChannel(0);
                Thread.Sleep(100);
                sendStr = "MEAS:CURR?";
                sendByte = CreatByteList(sendStr);
                _udp.Send(sendByte.ToArray(), sendByte.Count, new IPEndPoint(_currentIp, 7000));
                value = -999;
                value = GetReciveValue();

                CurrentC = (float)value * 1000f;



            }

            if (CurrentC == 0)
            {

                SelectChannel(0);
                Thread.Sleep(100);
                sendStr = "MEAS:CURR?";
                sendByte = CreatByteList(sendStr);
                _udp.Send(sendByte.ToArray(), sendByte.Count, new IPEndPoint(_currentIp, 7000));
                value = -999;
                value = GetReciveValue();

                CurrentC = (float)value * 1000f;
            }
            return value.ToString();
        }







        /// <summary>
        /// 读取选定通道的输出电流
        /// </summary>
        /// <returns></returns>
        [Description("读取选定通道的输出电流")]
        public string ReadOutputC2()
        {
            SelectChannel(1);
            Thread.Sleep(100);
            string sendStr = "MEAS:CURR?";
            List<byte> sendByte = CreatByteList(sendStr);
            _udp.Send(sendByte.ToArray(), sendByte.Count, new IPEndPoint(_currentIp, 7000));
            double value = -999;
            value = GetReciveValue();

            CurrentC2 = (float)value * 1000f;




            if (CurrentC2 == 0)
            {
                SelectChannel(1);
                Thread.Sleep(100);
                sendStr = "MEAS:CURR?";
                sendByte = CreatByteList(sendStr);
                _udp.Send(sendByte.ToArray(), sendByte.Count, new IPEndPoint(_currentIp, 7000));
                value = -999;
                value = GetReciveValue();
                CurrentC2 = (float)value * 1000f;
            }



            if (CurrentC2 == 0)
            {
                SelectChannel(1);
                Thread.Sleep(100);
                sendStr = "MEAS:CURR?";
                sendByte = CreatByteList(sendStr);
                _udp.Send(sendByte.ToArray(), sendByte.Count, new IPEndPoint(_currentIp, 7000));
                value = -999;
                value = GetReciveValue();
                CurrentC2 = (float)value * 1000f;
            }







            return value.ToString();
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


        public OsramEviyosPowerNgi3412E(string name)
            : base(name)
        {

        }

        ~OsramEviyosPowerNgi3412E()
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
            throw new System.NotImplementedException();
        }

        public void PowerOff()
        {
            throw new System.NotImplementedException();
        }

        public void SetCombSerOn()
        {
            throw new System.NotImplementedException();
        }

        public void SetCombParaOn()
        {
            throw new System.NotImplementedException();
        }

        public void SetCombOff()
        {
            throw new System.NotImplementedException();
        }

        public void SetVoltage1(float voltage)
        {
            throw new System.NotImplementedException();
        }

        public void SetVoltage2(float voltage)
        {
            throw new System.NotImplementedException();
        }

        public void SetVoltage3(float voltage)
        {
            throw new System.NotImplementedException();
        }

        public void SetCurrent1(float current)
        {
            throw new System.NotImplementedException();
        }

        public void SetCurrent2(float current)
        {
            throw new System.NotImplementedException();
        }

        public void SetCurrent3(float current)
        {
            throw new System.NotImplementedException();
        }

        public void ReadCurrAndVoit()
        {
            throw new System.NotImplementedException();
        }
    }
}
