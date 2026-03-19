using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CommonUtility
{
    /// <summary>
    /// 自定义Socket对象
    /// </summary>
    public class BaseSocket
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 活跃时间
        /// </summary>
        public DateTime ActiveDateTime { get; set; }

        /// <summary>
        /// 接收缓冲区
        /// </summary>
        public byte[] RecBuffer = new byte[65535 * 2];

        /// <summary>
        /// 发送缓冲区
        /// </summary>
        public byte[] SendBuffer = new byte[65535 * 2];

        /// <summary>
        /// 异步接收后包的大小
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseSocket() { }

        /// <summary>
        /// 创建Sockets对象
        /// </summary>
        /// <param name="ip">Ip地址</param>
        /// <param name="client">TcpClient</param>
        /// <param name="ns">承载客户端Socket的网络流</param>
        public BaseSocket(IPEndPoint ip, TcpClient client, NetworkStream ns)
        {
            Ip = ip;
            Client = client;
            NStream = ns;
        }

        /// <summary>
        /// 当前IP地址,端口号
        /// </summary>
        public IPEndPoint Ip { get; set; }

        /// <summary>
        /// 客户端主通信程序
        /// </summary>
        public TcpClient Client { get; set; }

        /// <summary>
        /// 承载客户端Socket的网络流
        /// </summary>
        public NetworkStream NStream { get; set; }

        /// <summary>
        /// 发生异常时不为null.
        /// </summary>
        public Exception Ex { get; set; }

        /// <summary>
        /// 新客户端标识.true,客户端上线
        /// 仅服务端有效
        /// </summary>
        public bool NewClientFlag { get; set; }

        /// <summary>
        /// 客户端退出标识.服务端发现true,客户端下线
        /// 客户端接收此标识时,认为客户端异常.
        /// </summary>
        public bool ClientDispose { get; set; }
    }

    /// <summary>
    /// Socket基类(抽象类)
    /// </summary>
    public abstract class SocketObject
    {
        public abstract void InitSocket(IPAddress ipaddress, int port);
        public abstract void InitSocket(string ipaddress, int port);
        public abstract void InitSocket(int port);
        public abstract void Start();
        public abstract void Stop();
    }

    public class MyAsyncSocketClient : SocketObject
    {
        public delegate void PushSocketsToTcpClient(BaseSocket sockets);
        public event PushSocketsToTcpClient OnPushSocketsToTcpClient;

        private bool _isClose;

        /// <summary>
        /// 当前管理对象
        /// </summary>
        private BaseSocket _sk;

        /// <summary>
        /// 客户端
        /// </summary>
        private TcpClient _client;

        /// <summary>
        /// 当前连接服务端地址
        /// </summary>
        private IPAddress _ipaddress;

        /// <summary>
        /// 当前连接服务端端口号
        /// </summary>
        private int _port;

        /// <summary>
        /// 服务端IP+端口
        /// </summary>
        private IPEndPoint _ip;

        /// <summary>
        /// 发送与接收使用的流
        /// </summary>
        private NetworkStream _nStream;

        /// <summary>
        /// 初始化Socket
        /// </summary>
        /// <param name="ipaddress"></param>
        /// <param name="port"></param>
        public override void InitSocket(string ipaddress, int port)
        {
            _ipaddress = IPAddress.Parse(ipaddress);
            _port = port;
            _ip = new IPEndPoint(_ipaddress, _port);
            _client = new TcpClient();

            Connect();
        }

        public override void InitSocket(int port)
        {
            _port = port;
        }

        public void SendData(byte[] sendData)
        {
            try
            {
                //如果连接则发送
                if (_client != null)
                {
                    if (_client.Connected)
                    {
                        if (_nStream == null)
                        {
                            _nStream = _client.GetStream();
                        }
                        var buffer = sendData;
                        _nStream.Write(buffer, 0, buffer.Length);
                    }
                    else
                    {
                        var sks = new BaseSocket
                        {
                            Ex = new Exception("客户端发送时无连接,开始进行重连上端.."),
                            ClientDispose = true
                        };
                        if (OnPushSocketsToTcpClient != null)
                            OnPushSocketsToTcpClient.Invoke(sks);
                        RestartInit();
                    }
                }
                else
                {
                    var sks = new BaseSocket
                    {
                        Ex = new Exception("客户端无连接.."),
                        ClientDispose = true
                    };
                    if (OnPushSocketsToTcpClient != null)
                        OnPushSocketsToTcpClient.Invoke(sks);
                }
            }
            catch (Exception skex)
            {
                var sks = new BaseSocket
                {
                    Ex = new Exception("客户端出现异常,开始重连上端..异常信息:" + skex.Message),
                    ClientDispose = true
                };
                if (OnPushSocketsToTcpClient != null)
                    OnPushSocketsToTcpClient.Invoke(sks);
                RestartInit();
            }
        }

        /// <summary>
        /// 初始化Socket
        /// </summary>
        /// <param name="ipaddress"></param>
        /// <param name="port"></param>
        public override void InitSocket(IPAddress ipaddress, int port)
        {
            _ipaddress = ipaddress;
            _port = port;
            _ip = new IPEndPoint(_ipaddress, _port);
            _client = new TcpClient();
        }

        public void RestartInit()
        {
            InitSocket(_ipaddress, _port);
            Connect();
        }

        private void Connect()
        {
            try
            {
                _client.Connect(_ip);
                _nStream = new NetworkStream(_client.Client, true);
                _sk = new BaseSocket(_ip, _client, _nStream);
                _sk.NStream.BeginRead(_sk.RecBuffer, 0, _sk.RecBuffer.Length, EndReader, _sk);
                var sks = new BaseSocket
                {
                    Ex = new Exception("客户端连接成功."),
                    ClientDispose = false
                };
                if (OnPushSocketsToTcpClient != null)
                    OnPushSocketsToTcpClient.Invoke(sks);
            }
            catch (Exception skex)
            {
                var sks = new BaseSocket
                {
                    Ex = new Exception("客户端连接失败..异常信息:" + skex.Message),
                    ClientDispose = true
                };
                if (OnPushSocketsToTcpClient != null)
                    OnPushSocketsToTcpClient.Invoke(sks);
            }
        }

        private void EndReader(IAsyncResult ir)
        {
            var s =
                ir.AsyncState as BaseSocket;
            try
            {
                if (s == null) return;
                if (_isClose && _client == null)
                {
                    _sk.NStream.Close();
                    _sk.NStream.Dispose();
                    return;
                }
                s.Offset = s.NStream.EndRead(ir);
                if (OnPushSocketsToTcpClient != null)
                    OnPushSocketsToTcpClient.Invoke(s);
                _sk.NStream.BeginRead(_sk.RecBuffer, 0, _sk.RecBuffer.Length, EndReader, _sk);
            }
            catch (Exception skex)
            {
                var sks = s;
                if (sks != null)
                {
                    sks.Ex = skex;
                    sks.ClientDispose = true;
                    if (OnPushSocketsToTcpClient != null)
                        OnPushSocketsToTcpClient.Invoke(sks);
                }
            }
        }

        /// <summary>
        /// 连接服务端
        /// </summary>
        public override void Start()
        {
            Connect();
        }

        public override void Stop()
        {
            var sks = new BaseSocket();
            if (_client != null)
            {
                try
                {
                    _client.Client.Shutdown(SocketShutdown.Both);
                    Thread.Sleep(10);
                    _client.Close();
                    _isClose = true;
                    _client = null;
                }
                catch (Exception ex)
                {
                    sks.Ex = new Exception(ex.Message);
                }
            }
            else
            {
                sks.Ex = new Exception("客户端没有初始化.!");
            }
            sks.Ex = new Exception("客户端与上端断开连接..");
            if (OnPushSocketsToTcpClient != null)
                OnPushSocketsToTcpClient.Invoke(sks);
        }
    }

    public class MyAsyncSocketServer : SocketObject
    {
        public delegate void PushSocketsToTcpServer(BaseSocket sockets);
        public event PushSocketsToTcpServer OnPushSocketsToTcpServer;

        public bool IsStop;

        public readonly object Obj = new object();

        /// <summary>
        /// 信号量
        /// </summary>
        private readonly Semaphore _semap = new Semaphore(5, 5000);

        /// <summary>
        /// 客户端队列集合
        /// </summary>
        public List<BaseSocket> ClientList = new List<BaseSocket>();

        /// <summary>
        /// 服务端
        /// </summary>
        private TcpListener _listener;

        /// <summary>
        /// 当前IP地址
        /// </summary>
        private IPAddress _ipaddress;

        /// <summary>
        /// 当前监听端口
        /// </summary>
        private int _port;

        /// <summary>
        /// 当前IP,端口对象
        /// </summary>
        private IPEndPoint ip;

        /// <summary>
        /// 初始化服务端对象
        /// </summary>
        /// <param name="ipaddress">IP地址</param>
        /// <param name="port">监听端口</param>
        public override void InitSocket(IPAddress ipaddress, int port)
        {
            _ipaddress = ipaddress;
            _port = port;
            _listener = new TcpListener(_ipaddress, _port);
        }

        /// <summary>
        /// 初始化服务端对象 监听Any即所有网卡
        /// </summary>
        /// <param name="port">监听端口</param>
        public override void InitSocket(int port)
        {
            _ipaddress = IPAddress.Any;
            _port = port;
            _listener = new TcpListener(_ipaddress, _port);
        }

        /// <summary>
        /// 初始化服务端对象
        /// </summary>
        /// <param name="ipaddress">IP地址</param>
        /// <param name="port">监听端口</param>
        public override void InitSocket(string ipaddress, int port)
        {
            _ipaddress = IPAddress.Parse(ipaddress);
            _port = port;
            ip = new IPEndPoint(_ipaddress, _port);
            _listener = new TcpListener(_ipaddress, _port);
        }

        /// <summary>
        /// 启动监听,并处理连接
        /// </summary>
        public override void Start()
        {
            try
            {
                _listener.Start();
                var accTh = new Thread(new ThreadStart(delegate
                {
                    while (true)
                    {
                        if (IsStop)
                        {
                            break;
                        }
                        GetAcceptTcpClient();
                        Thread.Sleep(1);
                    }
                }));
                accTh.Start();
            }
            catch (SocketException skex)
            {
                var sks = new BaseSocket { Ex = skex };
                if (OnPushSocketsToTcpServer != null) OnPushSocketsToTcpServer.Invoke(sks); // 推送至UI
            }
        }

        /// <summary>
        /// 等待处理新的连接
        /// </summary>
        private void GetAcceptTcpClient()
        {
            try
            {
                _semap.WaitOne();
                var tclient = _listener.AcceptTcpClient();
                //维护客户端队列
                var socket = tclient.Client;
                var stream = new NetworkStream(socket, true); //承载这个Socket
                var sks = new BaseSocket(tclient.Client.RemoteEndPoint as IPEndPoint, tclient, stream)
                {
                    NewClientFlag = true
                };
                //加入客户端集合.
                AddClientList(sks);
                //推送新客户端
                if (OnPushSocketsToTcpServer != null) OnPushSocketsToTcpServer.Invoke(sks);
                //客户端异步接收
                sks.NStream.BeginRead(sks.RecBuffer, 0, sks.RecBuffer.Length, EndReader, sks);
                
                _semap.Release();
            }
            catch (Exception exs)
            {
                _semap.Release();
                var sk = new BaseSocket
                {
                    ClientDispose = true,
                    Ex = new Exception(exs + "新连接监听出现异常")
                };
                // 客户端退出
                if (OnPushSocketsToTcpServer != null)
                {
                    OnPushSocketsToTcpServer.Invoke(sk);//推送至UI
                }
            }
        }

        /// <summary>
        /// 异步接收发送的信息.
        /// </summary>
        /// <param name="ir"></param>
        private void EndReader(IAsyncResult ir)
        {
            var sks = ir.AsyncState as BaseSocket;
            if (sks == null || _listener == null) return;
            try
            {
                if (!sks.NewClientFlag && sks.Offset == 0) 
                    return;
                sks.NewClientFlag = false;
                sks.Offset = sks.NStream.EndRead(ir);
                if (OnPushSocketsToTcpServer != null) OnPushSocketsToTcpServer.Invoke(sks); // 推送至UI
                sks.NStream.BeginRead(sks.RecBuffer, 0, sks.RecBuffer.Length, EndReader, sks);
            }
            catch (Exception skex)
            {
                lock (Obj)
                {
                    //移除异常类
                    ClientList.Remove(sks);
                    var sk = sks;
                    sk.ClientDispose = true;//客户端退出
                    sk.Ex = skex;
                    if (OnPushSocketsToTcpServer != null) OnPushSocketsToTcpServer.Invoke(sks);//推送至UI
                }
            }
        }

        /// <summary>
        /// 加入队列.
        /// </summary>
        /// <param name="sk"></param>
        private void AddClientList(BaseSocket sk)
        {
            var sockets = ClientList.Find(o => Equals(o.Ip, sk.Ip));
            //如果不存在则添加,否则更新
            if (sockets == null)
            {
                ClientList.Add(sk);
            }
            else
            {
                ClientList.Remove(sockets);
                ClientList.Add(sk);
            }
        }

        public override void Stop()
        {
            if (_listener != null)
            {
                SendToAll("ServerOff");
                _listener.Stop();
                _listener = null;
                IsStop = true;
                //SocketHelper.pushSockets = null;
            }
        }

        /// <summary>
        /// 向所有在线的客户端发送信息.
        /// </summary>
        /// <param name="sendData">发送的文本</param>
        public void SendToAll(string sendData)
        {
            for (var i = 0; i < ClientList.Count; i++)
            {
                SendToClient(ClientList[i].Ip, sendData);
            }
        }

        /// <summary>
        /// 向所有在线的客户端发送信息.
        /// </summary>
        /// <param name="sendDataBuffer"></param>
        public void SendToAll(byte[] sendDataBuffer)
        {
            for (var i = 0; i < ClientList.Count; i++)
            {
                SendToClient(ClientList[i].Ip, sendDataBuffer);
            }
        }

        /// <summary>
        /// 向某一位客户端发送信息
        /// </summary>
        /// <param name="ipEndPoint">客户端IP+端口地址</param>
        /// <param name="sendDataBuffer"></param>
        public void SendToClient(IPEndPoint ipEndPoint, byte[] sendDataBuffer)
        {
            try
            {
                var sks = ClientList.Find(o => Equals(o.Ip, ipEndPoint));
                if (sks != null)
                {
                    if (sks.Client.Connected)
                    {
                        //获取当前流进行写入.
                        var nStream = sks.NStream;
                        if (nStream.CanWrite)
                        {
                            var buffer = sendDataBuffer;
                            nStream.Write(buffer, 0, buffer.Length);
                            nStream.ReadTimeout = 5;
                            nStream.Read(new byte[1024], 0, 1024);
                        }
                        else
                        {
                            //避免流被关闭,重新从对象中获取流
                            nStream = sks.Client.GetStream();
                            if (nStream.CanWrite)
                            {
                                var buffer = sendDataBuffer;
                                nStream.Write(buffer, 0, buffer.Length);
                            }
                            else
                            {
                                //如果还是无法写入,那么认为客户端中断连接.
                                ClientList.Remove(sks);
                            }
                        }
                    }
                    else
                    {
                        //没有连接时,标识退出 
                        sks.ClientDispose = true;//如果出现异常,标识客户端下线
                        sks.Ex = new Exception("客户端无连接");
                        if (OnPushSocketsToTcpServer != null) OnPushSocketsToTcpServer.Invoke(sks);//推送至UI
                    }
                }
            }
            catch (Exception skex)
            {
                var sks = new BaseSocket
                {
                    ClientDispose = true,
                    Ex = skex
                };
                //如果出现异常,标识客户端退出
                if (OnPushSocketsToTcpServer != null) OnPushSocketsToTcpServer.Invoke(sks);//推送至UI
            }
        }

        /// <summary>
        /// 向某一位客户端发送信息
        /// </summary>
        /// <param name="ipEndPoint">客户端IP+端口地址</param>
        /// <param name="sendData">发送的数据包</param>
        public void SendToClient(IPEndPoint ipEndPoint, string sendData)
        {
            try
            {
                var sks = ClientList.Find(o => Equals(o.Ip, ipEndPoint));
                if (sks != null)
                {
                    if (sks.Client.Connected)
                    {
                        //获取当前流进行写入.
                        var nStream = sks.NStream;
                        if (nStream.CanWrite)
                        {
                            var buffer = Encoding.UTF8.GetBytes(sendData);
                            nStream.Write(buffer, 0, buffer.Length);
                        }
                        else
                        {
                            //避免流被关闭,重新从对象中获取流
                            nStream = sks.Client.GetStream();
                            if (nStream.CanWrite)
                            {
                                var buffer = Encoding.UTF8.GetBytes(sendData);
                                nStream.Write(buffer, 0, buffer.Length);
                            }
                            else
                            {
                                //如果还是无法写入,那么认为客户端中断连接.
                                ClientList.Remove(sks);
                            }
                        }
                    }
                    else
                    {
                        //没有连接时,标识退出
                        var ks = new BaseSocket
                        {
                            ClientDispose = true,
                            Ex = new Exception("客户端无连接")
                        };
                        //如果出现异常,标识客户端下线
                        if (OnPushSocketsToTcpServer != null) OnPushSocketsToTcpServer.Invoke(ks);//推送至UI
                    }
                }
            }
            catch (Exception skex)
            {
                var sks = new BaseSocket
                {
                    ClientDispose = true,
                    Ex = skex
                };
                //如果出现异常,标识客户端退出
                if (OnPushSocketsToTcpServer != null)
                {
                    OnPushSocketsToTcpServer.Invoke(sks);//推送至UI
                }
            }
        }
    }
}
