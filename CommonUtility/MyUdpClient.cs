using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CommonUtility
{
    public sealed class MyUdpClient : IDisposable
    {
        public delegate void PushMsgDelegate(EndPoint ipEndPoint, byte[] bytes);
        public event PushMsgDelegate PushMsgEvent;

        private readonly UdpClient _udpClient;
        private const uint IocIn = 0x80000000;
        private const uint IocVendor = 0x18000000;
        private readonly uint _sioUdpConnereset = IocIn | IocVendor | 12;

        private readonly Dictionary<string, IPEndPoint> _remoteEpsDictionary =
            new Dictionary<string, IPEndPoint>();

        private readonly Queue<MySendQueue> _sendTasks = new Queue<MySendQueue>();
        private readonly object _locker = new object();
        private readonly EventWaitHandle _wh = new AutoResetEvent(false);
        private readonly Thread _worker;

        public MyUdpClient(string localIpAddress, int localPort)
        {
            try
            {
                _udpClient =
                    new UdpClient(new IPEndPoint(IPAddress.Parse(localIpAddress), localPort));
                _udpClient.Client.IOControl(
                    (int)_sioUdpConnereset, new[] { Convert.ToByte(false) }, null);
            }
            catch (Exception)
            {
                // ignored
            }
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

            try
            {
                if (_wh != null)
                    _wh.Close();

                if (_udpClient != null)
                    _udpClient.Close();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void AddRemoteClient(string remoteIpAddress, int remotePort)
        {
            var remote = string.Format(
                "{0}:{1}", remoteIpAddress, remotePort);
            if (!_remoteEpsDictionary.ContainsKey(remote))
                _remoteEpsDictionary.Add(
                    remote,
                    new IPEndPoint(
                        IPAddress.Parse(remoteIpAddress), remotePort));
        }

        public void BeginReceive()
        {
            if (_udpClient is null)
                return;

            var state = new State(_udpClient);
            _udpClient.BeginReceive(
                EndReceiveFromCallback, state);
        }

        private void EndReceiveFromCallback(IAsyncResult ir)
        {
            var state = ir.AsyncState as State;
            if (state == null)
                return;
            var udp = state.UdpClient;

            try
            {
                // push msg
                var receiveBytes = udp.EndReceive(ir, ref state.RemoteEp);
                OnPushMsgEvent(state.RemoteEp, receiveBytes);
            }
            catch (Exception ex)
            {
                // push error msg
                Console.WriteLine(ex.Message);
            }
            finally
            {
                try
                {
                    _udpClient.BeginReceive(EndReceiveFromCallback, state);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        public void EqueueSendTask(string remoteIpPort, byte[] value)
        {
            if (!_remoteEpsDictionary.ContainsKey(remoteIpPort))
                return;

            lock (_locker)
                _sendTasks.Enqueue(
                       new MySendQueue(
                           _remoteEpsDictionary[remoteIpPort], value));
            _wh.Set();
        }

        private void EqueueTaskNull()
        {
            lock (_locker)
                _sendTasks.Enqueue(null);
            _wh.Set();
        }

        public void SendMsgTo(IPEndPoint remoteEp, byte[] value, int delayMs = 10)
        {
            if (_udpClient is null)
                return;

            lock (_udpClient)
            {
                try
                {
                    _udpClient.Send(value, value.Length, remoteEp);
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

    internal class State
    {
        public UdpClient UdpClient { get; private set; }

        public byte[] Buffer { get; private set; }

        public IPEndPoint RemoteEp;

        public State(UdpClient udpClient)
        {
            Buffer = new byte[3072 * 2048];
            UdpClient = udpClient;
            RemoteEp = new IPEndPoint(IPAddress.Any, 0);
        }
    }

    internal class MySendQueue
    {
        public IPEndPoint EndPoint { get; private set; }
        public byte[] Msg { get; private set; }

        public MySendQueue(IPEndPoint endPoint, byte[] msg)
        {
            EndPoint = endPoint;
            Msg = msg;
        }
    }
}
