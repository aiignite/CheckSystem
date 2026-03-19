using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace CommonUtility
{
    public class MySerialPort : SerialPort
    {
        private readonly MyAsyncSocketClient _asyncSocketClient;
        private readonly List<byte> _recvBuff = new List<byte>();

        public delegate void PushSciMsgEventHandle(string name, byte[] datas);
        public static event PushSciMsgEventHandle PushSciMsg;
        public string Name;

        protected MySerialPort(string name)
        {
            Name = name;
        }

        public MySerialPort(
            string strPort, int nBaudTate, Parity parity, int dataBits, StopBits stopBits)
        {
            PortName = strPort;
            BaudRate = nBaudTate;
            Parity = parity;
            DataBits = dataBits;
            StopBits = stopBits;
            DataReceived += MySerialPort_DataReceived;
            Name = strPort;
            this.WriteTimeout = 50;
        }

        public MySerialPort(string ipaddress, int port)
        {
            _asyncSocketClient = new MyAsyncSocketClient();
            _asyncSocketClient.InitSocket(ipaddress, port);
            _asyncSocketClient.OnPushSocketsToTcpClient += _asyncSocketClient_OnPushSocketsToTcpClient;
            Name = string.Format("{0}:{1}", ipaddress, port);
        }

        private void _asyncSocketClient_OnPushSocketsToTcpClient(BaseSocket sockets)
        {
            if (sockets == null || sockets.RecBuffer == null)
                return;

            if (sockets.RecBuffer.Length == 0)
                return;

            var buffer = new byte[sockets.Offset];
            Array.Copy(sockets.RecBuffer, 0, buffer, 0, sockets.Offset);
            _recvBuff.AddRange(buffer);
        }

        private void MySerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var serialPort = sender as SerialPort;
            if (serialPort == null)
                return;

            Thread.Sleep(100);

            var len = serialPort.BytesToRead;
            var buff = new byte[len];
            serialPort.Read(buff, 0, len);
            _recvBuff.AddRange(buff);
        }

        public virtual void SendCommand(string str)
        {
            try
            {
                if (_asyncSocketClient != null)
                    _asyncSocketClient.SendData(Encoding.ASCII.GetBytes(str));
                else
                {
                    if (IsOpen)
                    {
                        try
                        {
                            Write(str);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                }

                Thread.Sleep(25);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public virtual void SendCommand(byte[] bytes, int delayMs = 25)
        {
            try
            {
                if (_asyncSocketClient != null)
                    _asyncSocketClient.SendData(bytes);
                else
                {
                    if (IsOpen)
                    {
                        try
                        {
                            Write(bytes, 0, bytes.Length);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                }

                if (delayMs>0)
                    Thread.Sleep(delayMs);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public virtual void SendBreakSyncCmd(string str)
        {
            try
            {
                if (_asyncSocketClient != null)
                    _asyncSocketClient.SendData(Encoding.ASCII.GetBytes(str));
                else
                {
                    if (IsOpen)
                    {
                        try
                        {
                            Write(str);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                }

                Thread.Sleep(25);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public virtual void SendBreakSyncCmd(byte[] bytes)
        {
            try
            {
                if (_asyncSocketClient != null)
                    _asyncSocketClient.SendData(bytes);
                else
                {
                    if (IsOpen)
                    {
                        try
                        {
                            Write(bytes, 0, bytes.Length);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                }

                Thread.Sleep(25);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void MyOpen()
        {
            if (_asyncSocketClient != null)
                _asyncSocketClient.Start();
            else
            {
                try
                {
                    Open();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public virtual string ReadDataStr()
        {
            Thread.Sleep(250);
            var temp = _recvBuff.ToArray();
            _recvBuff.Clear();
            return Encoding.ASCII.GetString(temp);
        }

        public virtual byte[] ReadDataBytes()
        {
            Thread.Sleep(100);
            var temp = _recvBuff.ToArray();
            _recvBuff.Clear();
            return temp;
        }

        public virtual void ClearBuff()
        {
            _recvBuff.Clear();
        }

        public virtual void OnPushSciMsg(byte[] value)
        {
            if (PushSciMsg != null)
                PushSciMsg(Name, value);
        }
    }
}
