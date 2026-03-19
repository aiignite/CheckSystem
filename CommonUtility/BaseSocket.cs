using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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
}
