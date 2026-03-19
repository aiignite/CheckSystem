using TouchSocket.Sockets;

namespace CommonUtility
{
    public class TouchSocketUdp
    {
        private UdpSession m_udpSession = new UdpSession();

        public TouchSocketUdp()
        {
            //this.m_udpSession.Received = (remote, e) =>
            //{
            //    if (e.Len > 1024)
            //    {
            //        this.m_udpSession.Logger.Info("收到："+e.Len+"长度的数据。");
            //        this.m_udpSession.Send("收到");
            //    }
            //    else
            //    {
            //        //this.m_udpSession.Logger.Info("收到：{Encoding.UTF8.GetString(e.ByteBlock.Buffer, 0, e.ByteBlock.Len)}");
            //    }
            //    return EasyTask.CompletedTask;
            //};
        }

        //public void InitUdp()
        //{
        //    this.m_udpSession.Received = (remote, e) =>
        //    {
        //        if (e.ByteBlock.Len > 1024)
        //        {
        //            this.m_udpSession.Logger.Info($"收到：{e.ByteBlock.Len}长度的数据。");
        //            this.m_udpSession.Send("收到");
        //        }
        //        else
        //        {
        //            this.m_udpSession.Logger.Info(
        //                $"收到：{Encoding.UTF8.GetString(e.ByteBlock.Buffer, 0, e.ByteBlock.Len)}");
        //        }
        //        return EasyTask.CompletedTask;
        //    };

        //    this.m_udpSession.Setup(new TouchSocketConfig()
        //         .SetBindIPHost(new IPHost(this.textBox2.Text))
        //         .SetRemoteIPHost(new IPHost(this.textBox3.Text))
        //         .UseBroadcast()
        //         .SetUdpDataHandlingAdapter(() =>
        //         {
        //             if (this.checkBox1.Checked)
        //             {
        //                 return new UdpPackageAdapter();
        //             }
        //             else
        //             {
        //                 return new NormalUdpDataHandlingAdapter();
        //             }
        //         })
        //         .ConfigureContainer(a =>
        //         {
        //             a.SetSingletonLogger(new LoggerGroup(new EasyLogger(this.ShowMsg), new FileLogger()));
        //         }));
        //    m_udpSession.Start();
        //    this.m_udpSession.Logger.Info("等待接收");
        //}
    }
}
