using CommonUtility.BusLoader;
using System;
using System.ComponentModel;
using System.Threading;

namespace Controller
{
    [Description("LIN-Product,H53B点灯")]
    public sealed class H53BHeadLamp : ControllerBase
    {
        public LinBus LinWithBaudRate10417;

        public H53BHeadLamp(string name) : base(name)
        {
            _mainWorkThread = new Thread(CyNormalCyclicTimer);
            _mainWorkThread.Start();
        }

        ~H53BHeadLamp()
        {
            Dispose();
        }


        private readonly Thread _mainWorkThread;
        private bool _isSleep = true;
        private byte[] _sendDatas = new byte[] { 0x00, 0x80, 0x80 };

        private void CyNormalCyclicTimer()
        {
            while (_mainWorkThread.IsAlive)
            {
                if (!_mainWorkThread.IsAlive)
                    break;

                Thread.Sleep(25);

                //lock (_lockLin)
                {
                    try
                    {
                        if (LinWithBaudRate10417 == null)
                            continue;

                        if (!_isSleep)
                        {
                            LinWithBaudRate10417.SendMasterLin(0x00, _sendDatas);
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
        }

        [Description("打开LIN发送")]
        public void LinStartScheduler()
        {
            _isSleep = false;
        }

        [Description("关闭LIN发送")]
        public void LinStopScheduler()
        {
            _isSleep = true;
        }

        [Description("日行灯亮")]
        public void DrlOn()
        {
            _sendDatas[2] = 0x81;
        }

        //[Description("日行灯灭")]
        //public void DrlOff()
        //{

        //}

        [Description("位置灯亮")]
        public void PlOn()
        {
            _sendDatas[2] = 0x87;
        }

        //[Description("位置灯灭")]
        //public void PlOff() { }

        [Description("转向流水亮")]
        public void TurnRunningOn()
        {
            _sendDatas[2] = 0x90;
        }

        [Description("转向常亮")]
        public void TurnHoldingOn()
        {
            _sendDatas[2] = 0xA0;
        }

        //[Description("转向灭")]
        //public void TurnOff()
        //{
        //}

        [Description("灯熄灭")]
        public void LedOff()
        {
            _sendDatas[2] = 0x80;
        }
    }
}
