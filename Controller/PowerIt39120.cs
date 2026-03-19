using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Controller
{
    public class PowerIt39120 : ControllerBase
    {
        private Socket _udp;
        private IPEndPoint _currentIp;

        [Description("R,读取电压值")]
        public float CurrentV;
        [Description("R,读取电流值")]
        public float CurrentC;

        public PowerIt39120(string name)
            : base(name)
        {

        }

        public void InitPower(string ipPort)
        {
            var ip = ipPort.Split(':')[0];
            var port = ipPort.Split(':')[1];
            _udp = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _currentIp = new IPEndPoint(IPAddress.Parse(ip), int.Parse(port));
        }

        /// <summary>
        /// 电源通道打开
        /// </summary>
        [Description("电源通道打开")]
        public void PowerOn1()
        {
            var data = Encoding.ASCII.GetBytes("id001:output 1\r\n");
            _udp.SendTo(data, _currentIp);
        }

        /// <summary>
        /// 电源通道关闭
        /// </summary>
        [Description("电源通道关闭")]
        public void PowerOff()
        {
            var data = Encoding.ASCII.GetBytes("id001:output 0\r\n");
            _udp.SendTo(data, _currentIp);
        }

        /// <summary>
        /// 设置电压值
        /// </summary>
        /// <param name="v"></param>
        [Description("设置电压值")]
        public void PowerSetV(float v)
        {
            var data = Encoding.ASCII.GetBytes(string.Format("id001:output:voltage {0}\r\n", v));
            _udp.SendTo(data, _currentIp);
        }

        /// <summary>
        /// 读取电压值
        /// </summary>
        /// <returns></returns>
        [Description("读取电压值")]
        public float ReadV()
        {
            var data = Encoding.ASCII.GetBytes("id001:echo:voltage?\r\n");
            _udp.SendTo(data, _currentIp);
            var buffer = new byte[1024];
            var length = _udp.Receive(buffer);
            var RES = Encoding.ASCII.GetString(buffer, 0, length);
            var res = RES.Split(':')[1].Split(',')[0];
            return CurrentV = Convert.ToSingle(res);
        }

        /// <summary>
        /// 设置电流值
        /// </summary>
        /// <param name="c"></param>
        [Description("设置电流值")]
        public void PowerSetC(float c)
        {
            var data = Encoding.ASCII.GetBytes(string.Format("id001:output:current {0}\r\n", c));
            _udp.SendTo(data, _currentIp);
        }

        /// <summary>
        /// 读取电流值
        /// </summary>
        /// <returns></returns>
        [Description("读取电流值")]
        public float ReadC()
        {
            var data = Encoding.ASCII.GetBytes("id001:echo:current?\r\n");
            _udp.SendTo(data, _currentIp);
            var buffer = new byte[1024];
            var length = _udp.Receive(buffer);
            var RES = Encoding.ASCII.GetString(buffer, 0, length);
            var res = RES.Split(':')[1].Split(',')[0];
            return CurrentC = Convert.ToSingle(res);
        }
    }
}
