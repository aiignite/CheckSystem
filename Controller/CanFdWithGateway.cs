using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CommonUtility;
using CommonUtility.BusLoader;

namespace Controller
{
    public sealed class CanFdWithGateway :
        ControllerBase, IGatewaySocketViaUdpSubscriber
    {
        public GatewaySocketViaUdp MySocketViaUdp;
        public CanBus GatewayCanFd0; //11
        public CanBus GatewayCanFd1; //12
        public CanBus GatewayCanFd2; //21
        public CanBus GatewayCanFd3; //22
        public CanBus GatewayCanFd4; //31
        public CanBus GatewayCanFd5; //32
        public CanBus GatewayCanFd6; //41
        public CanBus GatewayCanFd7; //42
        public CanBus GatewayCanFd8; //51
        public CanBus GatewayCanFd9; //52

        public CanFdWithGateway(string name)
            : base(name) { }

        ~CanFdWithGateway()
        {
            Dispose();
        }

        private string _localIpPort;
        private string _remoteIpPort;

        public void InitLocalIpAddress(string localIpPort)
        {
            _localIpPort = localIpPort;
        }

        public void InitRemoteIpAddress(string remoteIpPort)
        {
            _remoteIpPort = remoteIpPort;
        }

        public void ConnectCanFdWithGateway()
        {
            MySocketViaUdp =
                new GatewaySocketViaUdp(_localIpPort, _remoteIpPort);
            MySocketViaUdp.AddOvserver(ReceiveMsg);

            for (var i = 0; i < 10; i++)
                InitCanPort((byte)i);

            var can = GatewayCanFd0 as GatewayCanFd;
            if (can != null) can.SelectCan();
        }

        public void ReceiveMsg(
            EndPoint ipEndPoint, byte[] bytes)
        {
            try
            {

            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void InitCanPort(byte canChannelId)
        {
            #region 在此打包发送函数
            var sendCanDataAction = new Action<CanBus.CanDataPackage[]>(dataPackages =>
            {
                if (dataPackages == null)
                    return;

                foreach (var p in dataPackages)
                {
                    var sendBytes = new List<byte>();
                    sendBytes.AddRange(new byte[] { 0x01, 0x00 });

                    switch (canChannelId)
                    {
                        case 0x00:
                            sendBytes.Add(0x01);
                            sendBytes.Add(0x01);
                            break;

                        case 0x01:
                            sendBytes.Add(0x01);
                            sendBytes.Add(0x02);
                            break;

                        case 0x02:
                            sendBytes.Add(0x02);
                            sendBytes.Add(0x01);
                            break;

                        case 0x03:
                            sendBytes.Add(0x02);
                            sendBytes.Add(0x02);
                            break;

                        case 0x04:
                            sendBytes.Add(0x03);
                            sendBytes.Add(0x01);
                            break;

                        case 0x05:
                            sendBytes.Add(0x03);
                            sendBytes.Add(0x02);
                            break;

                        case 0x06:
                            sendBytes.Add(0x04);
                            sendBytes.Add(0x01);
                            break;

                        case 0x07:
                            sendBytes.Add(0x04);
                            sendBytes.Add(0x02);
                            break;

                        case 0x08:
                            sendBytes.Add(0x05);
                            sendBytes.Add(0x01);
                            break;

                        case 0x09:
                            sendBytes.Add(0x05);
                            sendBytes.Add(0x02);
                            break;
                    }

                    var datas = new List<byte>();

                    var canIdBytes = BitConverter.GetBytes(p.CanId);
                    Array.Reverse(canIdBytes);

                    var canLen = BitConverter.GetBytes((byte)p.CanDataLen);
                    Array.Reverse(canLen);

                    var sendCanBytes = new List<byte>();
                    sendCanBytes.AddRange(canLen); // CAN数据长度
                    sendCanBytes.AddRange(canIdBytes); // CAN ID
                    sendCanBytes.AddRange(new byte[] { 0x00, p.CanType == CanBus.CanType.Standard ? (byte)0x00 : (byte)0x04 });
                    // 帧类型，0x0000是标准帧，0x0004是扩展帧
                    sendCanBytes.AddRange(new byte[] { 0x00, p.CanFormat == CanBus.CanFormat.Data ? (byte)0x00 : (byte)0x01 });
                    // 帧格式，0x0000是数据帧，0x0001是远程帧，目前只使用数据帧

                    var canRegistersValues = new List<byte>();
                    foreach (var t in p.CanData)
                    {
                        canRegistersValues.Add(0x00);
                        canRegistersValues.Add(t);
                    }
                    sendCanBytes.AddRange(canRegistersValues);
                    datas.AddRange(sendCanBytes);

                    var codeId = (byte)0x00;
                    switch (p.CanProtocol)
                    {
                        case CanBus.CanProtocol.Can:
                            codeId = 0x65;
                            break;

                        case CanBus.CanProtocol.CanFd:
                            codeId = 0x70;
                            break;
                    }

                    sendBytes.AddRange(
                        ModbusOperater.GetModbusWriteValues(codeId, "0", (ushort)(datas.Count / 2), datas.ToArray(),
                            canChannelId));

                    MySocketViaUdp.SendMsg(sendBytes);
                }
            });
            #endregion

            var memberInfo = GetType()
                .GetField("GatewayCanFd" + canChannelId);
            if (memberInfo == null)
                return;

            var createCan =
                Activator.CreateInstance(typeof(GatewayCanFd), canChannelId, sendCanDataAction, MySocketViaUdp);
            memberInfo.SetValue(this, createCan);
        }

        public void SelectCanChannel(int canChannelId)
        {
            var memberInfo = GetType()
               .GetField("GatewayCanFd" + canChannelId);
            if (memberInfo == null)
                return;

            var can = memberInfo.GetValue(this) as GatewayCanFd;

            if (can == null)
                return;

            can.IsSelected = true;
            can.SelectCan();
        }

        /// <summary>
        /// CAN网关
        /// </summary>
        public class GatewayCanFd :
            CanBus, IGatewaySocketViaUdpSubscriber
        {
            public bool IsSelected;

            /// <summary>
            /// CAN通道ID
            /// </summary>
            private byte _canChannelId;

            /// <summary>
            /// 发送消息方式
            /// </summary>
            private readonly Action<CanDataPackage[]> _thisGatewaySendCanPackagesAction;

            /// <summary>
            /// CANFD网关
            /// </summary>
            /// <param name="canChannelId"></param>
            /// <param name="sendCanPackagesAction"></param>
            /// <param name="mySocketViaUdp">CAN转发网络</param>
            public GatewayCanFd(
                byte canChannelId, Action<CanDataPackage[]> sendCanPackagesAction, GatewaySocketViaUdp mySocketViaUdp)
                : base(mySocketViaUdp.RemoteIpPort)
            {
                _canChannelId = canChannelId;
                _thisGatewaySendCanPackagesAction = sendCanPackagesAction;
                RegisterSendAction(_thisGatewaySendCanPackagesAction);
                mySocketViaUdp.AddOvserver(ReceiveMsg);
            }

            /// <summary>
            /// 01 00 01 01 00 21 00 70 00 00 00 0D 1A 00 08 00 00 00 07 00 00 00 00 00 FF 00 FF 00 FF 00 FF 00 FF 00 FF 00 FF 00 FF 00 08 3D B0 41 B1 73
            /// </summary>
            public void SelectCan()
            {
                IsSelected = true;

                SendMultipleCans(new[]
                {
                    new CanDataPackage(
                        _canChannelId,
                        CanProtocol.CanFd,
                        CanType.Standard,
                        CanFormat.Data,
                        new byte[] {0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF})
                });
            }

            public void ReceiveMsg(
                EndPoint ipEndPointFrom, byte[] bytes)
            {
                try
                {
                    if (!IsSelected)
                        return;

                    if (!bytes[7].Equals(0x65) && !bytes[7].Equals(0x70))
                        return;

                    if (bytes.Length < 15)
                        return;

                    var recv = bytes.ToList();

                    var canLen = recv[13] * 256 + recv[14];

                    var canId = BitConverter.ToUInt32(
                        new[] { recv[18], recv[17], recv[16], recv[15] }, 0);

                    var canType = recv[20];

                    var canFormat = recv[22];

                    if (bytes.Length < canLen * 2 + 23)
                        return;

                    var data = new List<byte>();

                    if (canLen > 0)
                    {
                        for (var i = 23; i < 23 + canLen * 2; i = i + 2)
                            data.Add(recv[i + 1]);
                    }

                    var dataPackage =
                        new CanDataPackage(canId,
                            bytes[7] == 0x65 ? CanProtocol.Can : CanProtocol.CanFd,
                            canType == 0x00 ? CanType.Standard : CanType.Extended,
                            canFormat == 0x00 ? CanFormat.Data : CanFormat.Remote, data.ToArray());
                    ReceiveCanDatas(new[] { dataPackage });

                    //recv.RemoveRange(0, 10 + canLen * 2);
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            private void SendMultipleCans(CanDataPackage[] dataPackages)
            {
                if (dataPackages == null)
                    return;

                if (_thisGatewaySendCanPackagesAction != null)
                    _thisGatewaySendCanPackagesAction(dataPackages);
            }
        }

        public class GatewaySocketViaUdp
        {
            public delegate void NotifyEventHandler(EndPoint ipEndPointFrom, byte[] bytes); // 需改成推送MSG TYPE

            private NotifyEventHandler _notifyEvent;

            private readonly MyUdpClient _myUdpClient;
            public readonly string RemoteIpPort;

            public GatewaySocketViaUdp(string localIpPort, string remoteIpPort)
            {
                RemoteIpPort = remoteIpPort;

                var split = RemoteIpPort.Split(':');
                var ipAddressStr = split[0];
                var port = Convert.ToInt32(split[1]);

                _myUdpClient = new MyUdpClient(localIpPort.Split(':')[0], Convert.ToInt32(localIpPort.Split(':')[1]));

                _myUdpClient.PushMsgEvent += _myUdpClient_PushMsgEvent;
                _myUdpClient.AddRemoteClient(ipAddressStr, port);
                _myUdpClient.BeginReceive();
            }

            public void AddOvserver(NotifyEventHandler ob)
            {
                _notifyEvent += ob;
            }

            public void RemoveOvserver(NotifyEventHandler ob)
            {
                // ReSharper disable once DelegateSubtraction
                if (_notifyEvent != null)
                    _notifyEvent -= ob;
            }

            private void Update(EndPoint ipEndPointFrom, byte[] bytes)
            {
                if (_notifyEvent != null)
                    _notifyEvent(ipEndPointFrom, bytes);
            }

            /// <summary>
            /// 接收消息
            /// 拆包和校验
            /// </summary>
            /// <param name="ipEndPointFrom">发送消息的节点</param>
            /// <param name="bytes">消息内容</param>
            private void _myUdpClient_PushMsgEvent(EndPoint ipEndPointFrom, byte[] bytes)
            {
                if (bytes.Length < 7)
                    return;

                var buffLst = bytes.ToList();

                do
                {
                    if (buffLst.Count < 7)
                    {
                        buffLst.Clear();
                        continue;
                    }

                    var len = buffLst[4] * 256 + buffLst[5];
                    if (buffLst.Count < len + 6 + 7)
                    {
                        buffLst.Clear();
                        continue;
                    }

                    var tempLen = len + 6 + 7;
                    var temp = new byte[tempLen];
                    Array.Copy(buffLst.ToArray(), temp, tempLen);
                    buffLst.RemoveRange(0, tempLen);

                    var tempContent = new byte[tempLen - 2];
                    Array.Copy(temp, tempContent, tempLen - 2);
                    var expectedCrc16 = ValueHelper.Crc16(tempContent).ToArray();
                    var actualCrc16 = new[] { temp[tempLen - 2], temp[tempLen - 1] };

                    //if (expectedCrc16[0] != actualCrc16[0] || expectedCrc16[1] != actualCrc16[1])
                    //    continue;

                    Update(ipEndPointFrom, temp);
                } while (buffLst.Count != 0);
            }

            private byte _sendTrackId;
            private DateTime _powerOnTime = DateTime.Now;

            /// <summary>
            /// 发送数据
            /// 在此处打包数据，添加索引、时间戳和CRC16-MODBUS校验
            /// </summary>
            /// <param name="bytes">需要打包发送的数据</param>
            public void SendMsg(IEnumerable<byte> bytes)
            {
                var sendBytes = bytes.ToList();
                sendBytes.Add(_sendTrackId);
                _sendTrackId = _sendTrackId == byte.MaxValue ? (byte)0 : (byte)(_sendTrackId + 1);

                var timespanMs = (float)(DateTime.Now - _powerOnTime).TotalMilliseconds;
                if (float.IsInfinity(timespanMs))
                {
                    timespanMs = 0;
                    _powerOnTime = DateTime.Now;
                }
                sendBytes.AddRange(BitConverter.GetBytes(timespanMs));
                sendBytes.AddRange(ValueHelper.Crc16(sendBytes));

                var remoteIpEndpoint = new IPEndPoint(IPAddress.Parse(RemoteIpPort.Split(':')[0]),
                    int.Parse(RemoteIpPort.Split(':')[1]));
                _myUdpClient.SendMsgTo(remoteIpEndpoint, sendBytes.ToArray(), 15);
            }
        }

        public interface IGatewaySocketViaUdpSubscriber
        {
            /// <summary>
            /// 接收消息
            /// </summary>
            /// <param name="ipEndPointFrom">消息发送过来的远程节点</param>
            /// <param name="bytes">消息内容</param>
            void ReceiveMsg(EndPoint ipEndPointFrom, byte[] bytes);
        }
    }
}
