using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommonUtility;

namespace Controller
{
    public sealed class PowerIt8500 : ControllerBase
    {
        private MyAsyncSocketClient _client;
        private readonly List<byte> _receiveBuf = new List<byte>();
        private readonly byte[] _data = new byte[26];
        private readonly AutoResetEvent _resetEvent = new AutoResetEvent(false);

        public PowerIt8500(string name)
            : base(name)
        {
        }

        #region 对外Public函数
        public void InitRemoteEndpoint(string host)
        {
            var server = host.Split(':')[0];
            var port = int.Parse(host.Split(':')[1]);
            _client = new MyAsyncSocketClient();
            _client.InitSocket(server, port);
            _client.OnPushSocketsToTcpClient += Client_OnPushSocketsToTcpClient;
        }

        /// <summary>
        /// 设置远程控制
        /// </summary>
        public void SetRemoteControl()
        {
            var send = new byte[22];
            send[0] = 1;
            SendCmd(0x20, send);
        }

        /// <summary>
        /// 设置本地控制
        /// </summary>
        public void SetLocalControl()
        {
            var send = new byte[22];
            SendCmd(0x20, send);
        }

        /// <summary>
        /// 设置负载模式
        /// </summary>
        /// <param name="model">0:CC,1:CV,2:CW,3:CR</param>
        public void SetPowerModel(int model)
        {
            var send = new byte[22];
            send[0] = (byte)model;
            SendCmd(0x28, send);
        }

        public void SetVoltage(float voltage)
        {
            int temp;
            if (voltage < 10)
            {
                temp = Convert.ToInt32(voltage * 10000);
            }
            else if (voltage < 100)
                temp = Convert.ToInt32(voltage * 1000);
            else if (voltage < 1000)
                temp = Convert.ToInt32(voltage * 100);
            else
                return;
            var buf = BitConverter.GetBytes(temp);
            SendCmd(0x2E, buf);
        }

        public void SetIo(float io)
        {
            for (var i = 1; i < io - 1; i++)
            {
                int temp;
                if (i < 10)
                {
                    temp = Convert.ToInt32(i * 10000);
                }
                else if (i < 100)
                {
                    temp = Convert.ToInt32(i * 1000);
                }
                else
                    return;
                var send = BitConverter.GetBytes(temp);

                SendCmd(0x2A, send);
                Thread.Sleep(100);
            }

            int temp1;
            if (io < 10)
            {
                temp1 = Convert.ToInt32(io * 10000);
            }
            else if (io < 100)
            {
                temp1 = Convert.ToInt32(io * 1000);
            }
            else
                return;
            var send1 = BitConverter.GetBytes(temp1);

            SendCmd(0x2A, send1);


        }

        public void SetMaxA(float i = 10)
        {
            int temp;
            if (i < 10)
            {
                temp = Convert.ToInt32(i * 10000);
            }
            else if (i < 100)
            {
                temp = Convert.ToInt32(i * 1000);
            }
            else
                return;
            var send = BitConverter.GetBytes(temp);
            SendCmd(0x24, send);
        }

        public void SetOutPutState(int state)
        {
            if (state != 0 && state != 1)
                return;
            var b = (byte)state;
            SendCmd(0x21, new[] { b });
        }

        #endregion

        #region 命令发送

        private bool SendCmd(byte cmd, IEnumerable<byte> content, byte synchead = 0xAA, byte address = 0)
        {
            //if (!Client.Connected)
            //    return false;
            return Task.Factory.StartNew(() =>
            {
                var data = new List<byte> { synchead, address, cmd };
                data.AddRange(content);
                while (data.Count < 25)
                    data.Add(0x00);
                data.Add(CalculateCrc(data)[0]);
                //发送
                _client.SendData(data.ToArray());
                if (_resetEvent.WaitOne(2000))
                {
                    if (_data[3] == 0xb0)
                    {
                        Console.WriteLine("命令发送失败!");
                        return false;
                    }

                    Console.WriteLine("命令发送成功!");
                    return true;
                }
                Console.WriteLine("未接收到反馈!");
                return false;
            }).Result;
        }

        private static byte[] CalculateCrc(IEnumerable<byte> data)
        {
            var cks = data.Aggregate(0, (current, b) => (current + b)%0xFFFF);
            var byteHi = (byte)((cks & 0xff00) >> 8);//高8位
            var byteLo = (byte)(cks & 0xff);
            return new[] { byteLo, byteHi };
        }

        #endregion

        #region 数据接收

        private void Client_OnPushSocketsToTcpClient(BaseSocket sockets)
        {
            if (sockets == null || sockets.RecBuffer == null)
                return;

            if (sockets.RecBuffer.Length == 0)
                return;

            try
            {
                var buffer = new byte[sockets.Offset];
                Array.Copy(sockets.RecBuffer, 0, buffer, 0, sockets.Offset);

                _receiveBuf.AddRange(buffer);
                var index = _receiveBuf.IndexOf(0xAA);
                if (index == -1)
                    return;
                _receiveBuf.RemoveRange(0, index);
                if (_receiveBuf.Count >= 26)
                {
                    //满26字节一条完整数据
                    _receiveBuf.CopyTo(0, _data, 0, _data.Length);
                    _receiveBuf.RemoveRange(0, 26);
                    _resetEvent.Set();
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        #endregion
    }
}
