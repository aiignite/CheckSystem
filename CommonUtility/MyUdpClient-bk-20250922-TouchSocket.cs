using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using TouchSocket.Core;
using TouchSocket.Sockets;

namespace CommonUtility
{
    /// <summary>
    /// 一对一udp
    /// </summary>
    public sealed class MyUdpClient : IDisposable
    {
        public delegate void PushMsgDelegate(EndPoint ipEndPoint, byte[] bytes);
        public event PushMsgDelegate PushMsgEvent;

        private readonly UdpSession _udp = new UdpSession();
        private readonly TouchSocketConfig _touchSocketConfig = new TouchSocketConfig();
        private IPHost _bindingRemoteHostEndPoint;

        /// <summary>
        /// 一对一udp
        /// </summary>
        /// <param name="localIpAddress"></param>
        /// <param name="localPort"></param>
        public MyUdpClient(string localIpAddress, int localPort)
        {
            _udp.Received = Received;

            _touchSocketConfig
                .SetBindIPHost(new IPHost(string.Format("{0}:{1}", localIpAddress, localPort)))
                .UseBroadcast()
                .SetUdpDataHandlingAdapter(() => new NormalUdpDataHandlingAdapter());
        }

        /// <summary>
        /// 垃圾回收
        /// </summary>
        ~MyUdpClient()
        {
            Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool isDisposeing)
        {
            if (!isDisposeing)
                return;

            _udp.Stop();
            _udp.ClearPlugins();
            _udp.Dispose();
        }

        public void AddRemoteClient(string remoteIpAddress, int remotePort)
        {
            _bindingRemoteHostEndPoint = new IPHost(string.Format("{0}:{1}", remoteIpAddress, remotePort));
            _touchSocketConfig.SetRemoteIPHost(_bindingRemoteHostEndPoint);
        }

        public void BeginReceive()
        {
            _udp.Setup(_touchSocketConfig);
            _udp.Start();
        }

        private void Received(EndPoint endpoint, ByteBlock block, IRequestInfo requestinfo)
        {
            if (_bindingRemoteHostEndPoint == null || !Equals(endpoint, _bindingRemoteHostEndPoint.EndPoint))
                return;

            var buff = new byte[block.Len];
            Array.Copy(block.Buffer, buff, block.Len);
            OnPushMsgEvent(endpoint, buff);
        }

        public void SendMsgTo(IPEndPoint remoteEp, byte[] value, int delayMs = 10)
        {
            lock (_udp)
            {
                try
                {
                    if (_udp.CanSend)
                        _udp.Send(value);

                    Thread.Sleep(delayMs);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("UDP发送失败，Remote：{0}，异常信息：{1}。", remoteEp.Address, ex.Message);
                }
            }
        }

        private void OnPushMsgEvent(EndPoint ipendpoint, byte[] bytes)
        {
            try
            {
                var handler = PushMsgEvent;
                if (handler != null) handler(ipendpoint, bytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
