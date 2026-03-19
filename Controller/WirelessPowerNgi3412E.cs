using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CommonUtility;
using System.Net;
using System.Threading;
using System.ComponentModel;

namespace Controller
{
    public class WirelessPowerNgi3412E : ControllerBase
    {
        //Example:
        //PowerNgi3412E it39220 = new PowerNgi3412E("1");
        //it39220.InitPower("192.168.1.123");
        //it39220.PowerOn();
        //it39220.ReadC();
        //it39220.ReadV();
        //var tc = it39220.currentC;
        //var tv = it39220.currentV;
        //it39220.SetV(13.123);
        //it39220.SetC(1);
        //it39220.PowerOff();


        UdpClient udp;
        IPAddress currentIP;
        public float currentV;
        public float currentC;
        public WirelessPowerNgi3412E(string name)
            : base(name)
        {

        }
        public void InitPower(string ip)
        {
            udp = new UdpClient();
            currentIP = IPAddress.Parse(ip);
        }


        private readonly object _lockUse = new object();
        private const string ConnectCmd = ";:";


        [Description("打开并联模式")]
        public void SetCombParaOn()
        {
            if (udp == null)
                return;
            lock (_lockUse)
            {
                const string cmd = "INST:NSEL 1" + ConnectCmd + "OUTP:PARA 1";
                var sendBytes = Encoding.ASCII.GetBytes(cmd).ToList();
                sendBytes.AddRange(new byte[] { 0x0D, 0x0A });
                udp.Send(sendBytes.ToArray(), sendBytes.ToArray().Length, new System.Net.IPEndPoint(currentIP, 7000));
                Thread.Sleep(150);
            }
        }





        [Description("打开串联模式")]
        public void SetCombSerOn()
        {
            if (udp == null)
                return;
            lock (_lockUse)
            {
                const string cmd = "INST:NSEL 1" + ConnectCmd + "OUTP:SERI 1";
                var sendBytes = Encoding.ASCII.GetBytes(cmd).ToList();
                sendBytes.AddRange(new byte[] { 0x0D, 0x0A });
                udp.Send(sendBytes.ToArray(), sendBytes.ToArray().Length, new System.Net.IPEndPoint(currentIP, 7000));
                Thread.Sleep(150);
            }
        }








        public void PowerOn(byte cl = 0)
        {

            var sendByte = new byte[] { 0x49, 0x4E, 0x53, 0x54, 0x3A, 0x4E, 0x53, 0x45, 0x4C, (byte)(0x31 + cl), 0x0D, 0x0A };
            udp.Send(sendByte, sendByte.Length, new System.Net.IPEndPoint(currentIP, 7000));
            Thread.Sleep(150);

        }



        public void PowerOff(byte cl = 0)
        {
            var sendByte = new byte[] { 0x49, 0x4E, 0x53, 0x54, 0x3A, 0x4E, 0x53, 0x45, 0x4C, (byte)(0x31 + cl), 0x0D, 0x0A };
            udp.Send(sendByte, sendByte.Length, new System.Net.IPEndPoint(currentIP, 7000));
            Thread.Sleep(150);


        }




        public void SelectChannel(byte cl = 0)
        {
            var sendByte = new byte[] { 0x49, 0x4E, 0x53, 0x54, 0x3A, 0x4E, 0x53, 0x45, 0x4C, 0x20, (byte)(0x31 + cl), 0x0D, 0x0A };
            udp.Send(sendByte, sendByte.Length, new System.Net.IPEndPoint(currentIP, 7000));
            Thread.Sleep(150);

        }


        public void ChannelOn(byte cl = 0)
        {
            var sendByte = new byte[] { 0x4F, 0x55, 0x54, 0x50, 0x20, 0x31, 0x0D, 0x0A };
            udp.Send(sendByte, sendByte.Length, new System.Net.IPEndPoint(currentIP, 7000));
            Thread.Sleep(150);

        }


        public void ChannelOff(byte cl = 0)
        {
            var sendByte = new byte[] { 0x4F, 0x55, 0x54, 0x50, 0x20, 0x30, 0x0D, 0x0A };
            udp.Send(sendByte, sendByte.Length, new System.Net.IPEndPoint(currentIP, 7000));
            Thread.Sleep(150);

        }



        public void SetV(float v)
        {
            var sendByte = new List<byte>();
            sendByte.AddRange(new byte[] { 0x56, 0x4F, 0x4C, 0x54, 0x20 });
            sendByte.AddRange(ASCIIEncoding.ASCII.GetBytes(v.ToString("f3")));
            sendByte.AddRange(new byte[] { 0x0D, 0x0A });
            var _sendByte = sendByte.ToArray();
            udp.Send(_sendByte, _sendByte.Length, new System.Net.IPEndPoint(currentIP, 7000));
            Thread.Sleep(150);

        }



        public void SetC(float c)
        {
            var sendByte = new List<byte>();
            sendByte.AddRange(new byte[] { 0x43, 0x55, 0x52, 0x52, 0x20 });
            sendByte.AddRange(ASCIIEncoding.ASCII.GetBytes(c.ToString("f3")));
            sendByte.AddRange(new byte[] { 0x0D, 0x0A });
            var _sendByte = sendByte.ToArray();
            udp.Send(_sendByte, _sendByte.Length, new System.Net.IPEndPoint(currentIP, 7000));
            Thread.Sleep(150);



        }







        //public void PowerOn(byte cl = 0) {

        //    var sendByte = new byte[] { 0x69, 0x64, 0x30, 0x30, (byte)(0x31 + cl), 0x3A, 0x6F, 0x75, 0x74, 0x70, 0x75, 0x74, 0x20, 0x31, 0x0D, 0x0A };
        //    udp.Send(sendByte, sendByte.Length, new System.Net.IPEndPoint(currentIP, 7000));
        //}
        //public void PowerOff(byte cl = 0) {
        //    var sendByte = new byte[] { 0x69, 0x64, 0x30, 0x30, (byte)(0x31 + cl), 0x3A, 0x6F, 0x75, 0x74, 0x70, 0x75, 0x74, 0x20, 0x30, 0x0D, 0x0A };
        //    udp.Send(sendByte, sendByte.Length, new System.Net.IPEndPoint(currentIP, 7000));
        //}
        //public void SetV(float v, byte cl = 0) {
        //    var sendByte = new List<byte>();
        //    sendByte.AddRange(new byte[] { 0x69, 0x64, 0x30, 0x30, (byte)(0x31 + cl), 0x3A, 0x6F, 0x75, 0x74, 0x70, 0x75, 0x74, 0x3A, 0x76, 0x6F, 0x6C, 0x74, 0x61, 0x67, 0x65, 0x20 });
        //    sendByte.AddRange(ASCIIEncoding.ASCII.GetBytes(v.ToString("f3")));
        //    sendByte.AddRange(new byte[] { 0x0D, 0x0A });
        //    var _sendByte = sendByte.ToArray();
        //    udp.Send(_sendByte, _sendByte.Length, new System.Net.IPEndPoint(currentIP, 7000));
        //}

        //public void SetC(float c, byte cl = 0) {
        //    var sendByte = new List<byte>();
        //    sendByte.AddRange(new byte[] { 0x69,0x64,0x30,0x30, (byte)(0x31 + cl), 0x3A,0x6F,0x75,0x74,0x70,0x75,0x74,0x3A,0x63,0x75,0x72,0x72,0x65,0x6E,0x74,0x20 });
        //    sendByte.AddRange(ASCIIEncoding.ASCII.GetBytes(c.ToString("f3")));
        //    sendByte.AddRange(new byte[] { 0x0D, 0x0A });
        //    var _sendByte = sendByte.ToArray();
        //    udp.Send(_sendByte, _sendByte.Length, new System.Net.IPEndPoint(currentIP, 7000));
        //}
        //public float ReadV(byte cl = 0)
        //{
        //    var sendByte = new byte[] { 0x69, 0x64, 0x30, 0x30, (byte)(0x31 + cl), 0x3A, 0x65, 0x63, 0x68, 0x6F, 0x3A, 0x76, 0x6F, 0x6C, 0x74, 0x61, 0x67, 0x65, 0x3F, 0x0D, 0x0A };
        //    udp.Send(sendByte, sendByte.Length, new System.Net.IPEndPoint(currentIP, 7000));
        //    var t = udp.ReceiveAsync();
        //    if (t.Wait(500))
        //    {
        //        var tempRec = t.Result.Buffer.GetStringByASCIIBytes(false);
        //        float.TryParse(tempRec.Split(':')[1].Split(',')[0], out currentV);
        //    }
        //    else
        //    {
        //        currentV = -9999f;
        //    }
        //    return currentV;
        //}
        //public float ReadC(byte cl = 0)
        //{
        //    var sendByte = new byte[] { 0x69, 0x64, 0x30, 0x30, (byte)(0x31 + cl), 0x3A, 0x65, 0x63, 0x68, 0x6F, 0x3A, 0x63, 0x75, 0x72, 0x72, 0x65, 0x6E, 0x74, 0x3F, 0x0D, 0x0A };
        //    udp.Send(sendByte, sendByte.Length, new System.Net.IPEndPoint(currentIP, 7000));
        //    var t = udp.ReceiveAsync();
        //    if (t.Wait(500))
        //    {
        //        var tempRec = t.Result.Buffer.GetStringByASCIIBytes(false);
        //        float.TryParse(tempRec.Split(':')[1].Split(',')[0], out currentC);
        //    }
        //    else
        //    {
        //        currentC = -9999f;
        //    }
        //    return currentC;
        //}


    }
}
