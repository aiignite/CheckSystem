using CommonUtility;
using System;
using System.ComponentModel;
using System.Net;
using System.Text;
using System.Threading;

namespace Controller
{
    public sealed class PowerNgi : ControllerBase
    {
        [Description("R,电压1")]
        public float Volt1;

        [Description("R,电压2")]
        public float Volt2;

        [Description("R,电流1")]
        public float Curr1;

        [Description("R,电流2")]
        public float Curr2;

        private MyUdpClient MyUdpClient { get; set; }
        private IPEndPoint RemoteIpEndPoint { get; set; }
        private readonly EventWaitHandle _wh = new AutoResetEvent(false);
        public int ReadIndex;
        private float _recv;

        public PowerNgi(string name)
            : base(name)
        {

        }

        public void ConnectPower(string ipPort)
        {
            var split = ipPort.Split(':');
            var ipAddressStr = split[0];
            var port = Convert.ToInt32(split[1]);

            MyUdpClient = ipPort.StartsWith("127.0.0.1") ?
                new MyUdpClient("127.0.0.1", port + 1) :
                new MyUdpClient("192.168." + ipAddressStr.Split('.')[2] + ".50", 7000);

            MyUdpClient.PushMsgEvent += MyUdpClient_PushMsgEvent;
            MyUdpClient.AddRemoteClient(ipAddressStr, port);
            MyUdpClient.BeginReceive();

            RemoteIpEndPoint = new IPEndPoint(IPAddress.Parse(ipAddressStr), port);
        }

        [Description("打开通道1输出")]
        public void PowerOnCh1() => SwitchPower(1, true);

        [Description("关闭通道1输出")]
        public void PowerOffCh1() => SwitchPower(1, false);

        [Description("打开通道2输出")]
        public void PowerOnCh2() => SwitchPower(2, true);

        [Description("关闭通道2输出")]
        public void PowerOffCh2() => SwitchPower(2, false);

        private void SwitchPower(int index, bool isOn)
        {
            var cmd = string.Format("id00{0}:output {1}\r\n", index, isOn ? 1 : 0);
            if (MyUdpClient != null)
            {
                MyUdpClient.SendMsgTo(RemoteIpEndPoint, Encoding.ASCII.GetBytes(cmd));
            }
        }

        [Description("设置通道1电压")]
        public void SetVoltCh1(string volt)
        {
            float v;
            if (float.TryParse(volt, out v))
            {
                SetV(1, v);
            }
        }

        [Description("设置通道1电流")]
        public void SetCurrCh1(string curr)
        {
            float c;
            if (float.TryParse(curr, out c))
            {
                SetC(1, c);
            }
        }

        [Description("设置通道2电压")]
        public void SetVoltCh2(string volt)
        {
            float v;
            if (float.TryParse(volt, out v))
            {
                SetV(2, v);
            }
        }

        [Description("设置通道2电流")]
        public void SetCurrCh2(string curr)
        {
            float c;
            if (float.TryParse(curr, out c))
            {
                SetC(2, c);
            }
        }

        private void SetV(int index, float volt)
        {
            var cmd = string.Format("id00{0}:output:voltage {1}\r\n", index, volt);
            if (MyUdpClient != null)
                MyUdpClient.SendMsgTo(RemoteIpEndPoint, Encoding.ASCII.GetBytes(cmd));
        }

        private void SetC(int index, float curr)
        {
            var cmd = string.Format("id00{0}:output:current {1}\r\n", index, curr);
            if (MyUdpClient != null)
                MyUdpClient.SendMsgTo(RemoteIpEndPoint, Encoding.ASCII.GetBytes(cmd));
        }

        [Description("读取电流电压")]
        public void ReadCurrVolt()
        {
            Volt1 = -9999;
            Volt2 = -9999;
            Curr1 = -9999;
            Curr2 = -9999;

            if (MyUdpClient == null)
            {
                return;
            }

            for (var k = 0; k < 2; k++)
            {
                for (var i = 0; i < 2; i++)
                {
                    _recv = -9999;
                    ReadIndex = i + 1;

                    if (k == 0) // 先读电流
                    {
                        var cmd = string.Format("id00{0}:echo:current?\r\n", i + 1);
                        MyUdpClient.SendMsgTo(RemoteIpEndPoint, Encoding.ASCII.GetBytes(cmd));
                        var isReadOk = _wh.WaitOne(500);
                        if (!isReadOk || !(Math.Abs(_recv - -9999) > 0))
                            continue;
                        if (i == 0)
                            Curr1 = _recv;
                        else
                            Curr2 = _recv;
                    }
                    else
                    {
                        var cmd = string.Format("id00{0}:echo:voltage?\r\n", i + 1);
                        MyUdpClient.SendMsgTo(RemoteIpEndPoint, Encoding.ASCII.GetBytes(cmd));
                        var isReadOk = _wh.WaitOne(500);
                        if (!isReadOk || !(Math.Abs(_recv - -9999) > 0))
                            continue;
                        if (i == 0)
                            Volt1 = _recv;
                        else
                            Volt2 = _recv;
                    }

                }
            }
        }

        private void MyUdpClient_PushMsgEvent(EndPoint ipEndPoint, byte[] bytes)
        {
            if (!(Math.Abs(_recv - -9999) < 0.001)) return;
            if (bytes == null) return;
            var str = Encoding.ASCII.GetString(bytes);
            if (string.IsNullOrEmpty(str) || !str.StartsWith("id00" + ReadIndex))
                return;
            try
            {
                var value = float.Parse(str.Split(':')[1]);
                _recv = value;
                _wh.Set();
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}
