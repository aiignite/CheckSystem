using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using CommonUtility;

namespace Controller
{
    public sealed class RtexControllerBase : ControllerBase
    {
        public RtexControllerBase RtexController;
        private string LocalIpAddress { get; set; }
        private string RemoteIpAddress { get; set; }
        private MyUdpClient RtexUdpClient { get; set; }
        private Thread MainWorkTh { get; set; }

        public readonly byte[] DataBytes = new byte[1024 * 1024];
        private readonly EventWaitHandle _dataReadWait = new AutoResetEvent(false);
        private readonly EventWaitHandle _flashWriteWait = new AutoResetEvent(false);
        private readonly EventWaitHandle _runEventWait = new AutoResetEvent(false);
        private readonly EventWaitHandle _runJogWait = new AutoResetEvent(false);

        public RtexControllerBase(string name)
            : base(name)
        {
            RtexController = this;
        }

        ~RtexControllerBase()
        {
            Dispose();
        }

        public void InitLocalLocalIpAddress(string ipPort)
        {
            LocalIpAddress = ipPort;
            //LocalIpAddress = "192.168.1.50:8088";
        }

        public void InitRemoteIpAddress(string ipPort)
        {
            RemoteIpAddress = ipPort;
            //RemoteIpAddress = "192.168.1.30:8088";

            var local = LocalIpAddress.Split(':');
            var remote = RemoteIpAddress.Split(':');

            var localIp = local[0];
            var localPort = int.Parse(local[1]);

            var remoteIp = remote[0];
            var remotePort = int.Parse(remote[1]);

            RtexUdpClient = new MyUdpClient(localIp, localPort);
            RtexUdpClient.AddRemoteClient(remoteIp, remotePort);
            RtexUdpClient.PushMsgEvent += _rtexUdpClient_PushMsgEvent;
            RtexUdpClient.BeginReceive();

            if (MainWorkTh != null)
            {
                MainWorkTh.Abort();
                MainWorkTh.Join();
            }

            MainWorkTh = new Thread(MainWork) { IsBackground = true };
            MainWorkTh.Start();
        }

        private void MainWork()
        {
            while (MainWorkTh.IsAlive)
            {
                if (!MainWorkTh.IsAlive)
                    break;

                if (RtexUdpClient == null)
                    continue;

                Thread.Sleep(50);

                try
                {
                    lock (RtexUdpClient)
                    {
                        var readBytes = new List<byte>();
                        readBytes.AddRange(new byte[] { 0x00, 0x06 });
                        readBytes.Add(0x01);
                        readBytes.Add(0x03);

                        var startAddressBs = BitConverter.GetBytes(Convert.ToUInt16(0x0200));
                        Array.Reverse(startAddressBs);
                        readBytes.AddRange(startAddressBs);

                        var registerCountBs = BitConverter.GetBytes(Convert.ToUInt16(272));
                        Array.Reverse(registerCountBs);
                        readBytes.AddRange(registerCountBs);

                        SendMsg(readBytes);

                        _dataReadWait.WaitOne(500);
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        public void RunEvent(int asixIndex, EventType eventType)
        {
            lock (RtexUdpClient)
            {
                Thread.Sleep(10);
                var bytes = new byte[4];
                var axisNo = asixIndex;
                bytes[1] = (byte)axisNo;

                bytes[3] = (byte)eventType;

                var baseAddr = 0x0010 + axisNo * 16;
                var sendBytes = new List<byte>();
                sendBytes.AddRange(ModbusOperater.GetModbusWriteValues(0x6D,
                    baseAddr.ToString(), 2, bytes));

                SendMsg(sendBytes);

                _runEventWait.WaitOne(500);
            }
        }

        public void RunJog(int asixIndex, float pos, float speed)
        {
            lock (RtexUdpClient)
            {
                //Thread.Sleep(20);
                const int regCount = 7;
                var axisNo = asixIndex;
                var baseAddr = 0x0010 + axisNo * 16;
                var bytes = new byte[regCount * 2];

                var asixNoAddr = 0x0010 + axisNo * 16 - baseAddr;
                var asixNoBs = BitConverter.GetBytes(ushort.Parse(asixIndex.ToString()));
                Array.Reverse(asixNoBs);
                Array.Copy(asixNoBs, 0, bytes, asixNoAddr * 2, asixNoBs.Length);

                var posAddr = 0x0013 + axisNo * 16 - baseAddr;
                var posBs = BitConverter.GetBytes(pos);
                Array.Reverse(posBs);
                Array.Copy(posBs, 0, bytes, posAddr * 2, posBs.Length);

                var speedAddr = 0x0015 + axisNo * 16 - baseAddr;
                var speedBs = BitConverter.GetBytes(speed);
                Array.Reverse(speedBs);
                Array.Copy(speedBs, 0, bytes, speedAddr * 2, speedBs.Length);

                var sendBytes = new List<byte>();
                sendBytes.AddRange(ModbusOperater.GetModbusWriteValues(0x6C,
                    baseAddr.ToString(), (ushort)regCount, bytes));

                SendMsg(sendBytes);

                _runJogWait.WaitOne(500);
            }
        }

        private void SendMsg(IEnumerable<byte> bytes)
        {
            var sendBytes = new List<byte>();
            sendBytes.AddRange(new byte[] { 0x00, 0x01 }); // data0,data1
            sendBytes.AddRange(new byte[] { 0x00, 0x00 }); // data2,data3

            sendBytes.AddRange(bytes);
            //sendBytes.Add(_sendTrackId);
            //_sendTrackId = _sendTrackId == byte.MaxValue ? (byte)0 : (byte)(_sendTrackId + 1);

            //var timespanMs = (float)(DateTime.Now - _powerOnTime).TotalMilliseconds;
            //if (float.IsInfinity(timespanMs))
            //{
            //    timespanMs = 0;
            //    _powerOnTime = DateTime.Now;
            //}
            //sendBytes.AddRange(BitConverter.GetBytes(timespanMs));
            //sendBytes.AddRange(ValueOperater.Crc16(sendBytes));

            RtexUdpClient.SendMsgTo(
                new IPEndPoint(IPAddress.Parse(RemoteIpAddress.Split(':')[0]), int.Parse(RemoteIpAddress.Split(':')[1])),
                sendBytes.ToArray());
        }

        private void _rtexUdpClient_PushMsgEvent(
            EndPoint ipEndPoint, byte[] bytes)
        {
            if (bytes.Length < 7)
                return;

            var buffLst = bytes.ToList();

            do
            {
                #region 拆包
                if (buffLst.Count < 7)
                {
                    buffLst.Clear();
                    continue;
                }

                var len = buffLst[4] * 256 + buffLst[5];
                if (buffLst.Count < len + 6)
                {
                    buffLst.Clear();
                    continue;
                }

                var tempLen = len + 6;
                var temp = new byte[tempLen];
                Array.Copy(buffLst.ToArray(), temp, tempLen);
                buffLst.RemoveRange(0, tempLen);

                var tempContent = new byte[tempLen - 2];
                Array.Copy(temp, tempContent, tempLen - 2);
                #endregion

                #region CRC16-MODBUS校验
                var totoalLen = temp[4] * 256 + temp[5];
                if (temp.Length - 6 != totoalLen)
                    continue; // 数组长度不正确
                #endregion

                #region 解析数据并推送

                var registerCount = (totoalLen - 9) / 2;

                var function = temp[7];
                var contentBytes = new byte[registerCount * 2];
                Array.Copy(bytes.ToArray(), 9, contentBytes, 0, registerCount * 2);

                switch (function)
                {
                    case 0x03:
                        Array.Copy(contentBytes, 0, DataBytes, 0, contentBytes.Length);
                        _dataReadWait.Set();
                        break;

                    case 0x6E:
                        //_flashWriteWait.Set();
                        break;

                    case 0x6D:
                        _runEventWait.Set();
                        break;

                    case 0x6C:
                        _runJogWait.Set();
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
                #endregion
            } while (buffLst.Count != 0);
        }

        public enum EventType : byte
        {
            Null = 0x00,

            ServoEnable = 0x01,

            ServoDisalbe = 0x02,

            EmergencyStop = 0x03,

            Reset = 0x04,

            Home = 0x05,

            SetHomeOffset = 0x06
        }
    }
}
