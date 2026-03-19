using System;
using System.Text;
using CommonUtility;
using System.Threading;

namespace Controller
{
    public sealed class FlashRunner : ControllerBase
    {
        public string ProjName;
        private MyAsyncSocketClient SocketClient { get; set; }
        private static readonly object LockFlashRunner = new object();

        public FlashRunner(string name)
            : base(name) { }

        public void InitIpPort(string ipPort)
        {
            var remoteIpPort = ipPort;

            var split = remoteIpPort.Split(':');
            var ipAddressStr = split[0];
            var port = Convert.ToInt32(split[1]);

            SocketClient = new MyAsyncSocketClient();
            SocketClient.InitSocket(ipAddressStr, port);
            //_socketClient.Start();
        }

        public void Run(string index)
        {
            lock (LockFlashRunner)
            {
                var cmd = string.Format("#{0}*RUN {1}.prj\r\n", index, ProjName);
                var bs = Encoding.ASCII.GetBytes(cmd);
                SocketClient.SendData(bs);
                Thread.Sleep(2000);
            }
        }
    }
}
