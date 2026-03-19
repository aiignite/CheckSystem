using System;
using System.ComponentModel;
using System.Threading;
using CommonUtility.BusLoader;

namespace Controller
{
    [Description("LIN-Product,SGM358-2-前灯-格栅灯")]
    public sealed class Sgm3582GrlleLamp : ControllerBase
    {
        public LinBus LinWithBaudRate10417;

        public Sgm3582GrlleLamp(string name)
            : base(name)
        {
            if (_mainWorkThread != null)
            {
                _mainWorkThread.Abort();
                _mainWorkThread.Join();
            }
            _mainWorkThread = new Thread(CyNormalCyclicTimer);
            _mainWorkThread.Start();
        }

        ~Sgm3582GrlleLamp()
        {
            Dispose();
        }

        private readonly Thread _mainWorkThread;
        private bool _isSleep = true;
        private readonly LinCommunicationMatrix.IntelMatrix _motorolaMatrix0X02 =
           new LinCommunicationMatrix.IntelMatrix(0x12, 4);

        private readonly object _lockLin = new object();

        [Description("LIN唤醒")]
        public void LampAwake()
        {
            _isSleep = false;
        }

        [Description("LIN休眠")]
        public void LampSleep()
        {
            _isSleep = true;
        }

        #region 点灯
        [Description("左格栅灯常亮")]
        public void LftGrlleLmpNormalOn()
        {
            _motorolaMatrix0X02.UpdateData(new MatrixValDefinition(8, 4, 1));
        }

        [Description("左格栅灯常亮关闭")]
        public void LftGrlleLmpNormalOff()
        {
            _motorolaMatrix0X02.UpdateData(new MatrixValDefinition(8, 4, 2));
        }

        [Description("右格栅灯常亮")]
        public void RtGrlleLmpNormalOn()
        {
            _motorolaMatrix0X02.UpdateData(new MatrixValDefinition(19, 4, 1));
        }

        [Description("右格栅灯常亮关闭")]
        public void RtGrlleLmpNormalOff()
        {
            _motorolaMatrix0X02.UpdateData(new MatrixValDefinition(19, 4, 2));
        }

        [Description("左格栅灯呼吸亮")]
        public void LftGrlleLmpRampOn()
        {
            _motorolaMatrix0X02.UpdateData(new MatrixValDefinition(8, 4, 3));
        }

        [Description("左格栅灯呼吸关闭")]
        public void LftGrlleLmpRampOff()
        {
            _motorolaMatrix0X02.UpdateData(new MatrixValDefinition(8, 4, 4));
        }

        [Description("右格栅灯呼吸亮")]
        public void RtGrlleLmpRampOn()
        {
            _motorolaMatrix0X02.UpdateData(new MatrixValDefinition(19, 4, 3));
        }

        [Description("右格栅灯呼吸关闭")]
        public void RtGrlleLmpRampOff()
        {
            _motorolaMatrix0X02.UpdateData(new MatrixValDefinition(19, 4, 4));
        }

        [Description("左格栅灯流水亮")]
        public void LftGrlleLmpSwipeOn()
        {
            _motorolaMatrix0X02.UpdateData(new MatrixValDefinition(8, 4, 5));
        }

        [Description("左格栅灯流水关闭")]
        public void LftGrlleLmpSwipeOff()
        {
            _motorolaMatrix0X02.UpdateData(new MatrixValDefinition(8, 4, 6));
        }

        [Description("右格栅灯流水亮")]
        public void RtGrlleLmpSwipeOn()
        {
            _motorolaMatrix0X02.UpdateData(new MatrixValDefinition(19, 4, 5));
        }

        [Description("右格栅灯流水关闭")]
        public void RtGrlleLmpSwipeOff()
        {
            _motorolaMatrix0X02.UpdateData(new MatrixValDefinition(19, 4, 6));
        }

        //[Description("左格栅灯高亮")]
        //public void LftGrlleLmpHighlightOn()
        //{
        //    _motorolaMatrix0X02.UpdateData(new MatrixValDefinition(8, 4, 7));
        //}

        //[Description("左格栅灯高亮关闭")]
        //public void LftGrlleLmpHighlightOff()
        //{
        //    _motorolaMatrix0X02.UpdateData(new MatrixValDefinition(8, 4, 8));
        //}

        //[Description("右格栅灯高亮")]
        //public void RtGrlleLmpHighlightOn()
        //{
        //    _motorolaMatrix0X02.UpdateData(new MatrixValDefinition(19, 4, 7));
        //}

        //[Description("右格栅灯高亮关闭")]
        //public void RtGrlleLmpHighlightOff()
        //{
        //    _motorolaMatrix0X02.UpdateData(new MatrixValDefinition(19, 4, 8));
        //}

        [Description("星悦PureHello")]
        public void PureHello()
        {
            _motorolaMatrix0X02.UpdateData(new MatrixValDefinition(8, 4, 0));
            _motorolaMatrix0X02.UpdateData(new MatrixValDefinition(19, 4, 0));
            _motorolaMatrix0X02.UpdateData(new MatrixValDefinition(0, 3, 2));
            _motorolaMatrix0X02.UpdateData(new MatrixValDefinition(3, 5, 3));
        }

        [Description("星耀ElaborateHello")]
        public void ElaborateHello()
        {
            _motorolaMatrix0X02.UpdateData(new MatrixValDefinition(0, 3, 3));
            //Thread.Sleep(25);
            _motorolaMatrix0X02.UpdateData(new MatrixValDefinition(3, 5, 3));
        }

        [Description("打断动画")]
        public void LampShowAbort()
        {
            _motorolaMatrix0X02.UpdateData(new MatrixValDefinition(3, 5, 2));
            _motorolaMatrix0X02.UpdateData(new MatrixValDefinition(0, 3, 0));
        }

        private void CyNormalCyclicTimer()
        {
            //_motorolaMatrix0X02.UpdateData(new MatrixValDefinition(3, 5, 2));

            while (_mainWorkThread.IsAlive)
            {
                if (!_mainWorkThread.IsAlive)
                    break;

                Thread.Sleep(100);

                lock (_lockLin)
                {
                    try
                    {
                        if (LinWithBaudRate10417 == null)
                            return;

                        if (!_isSleep)
                        {
                            LinWithBaudRate10417.SendMasterLin(_motorolaMatrix0X02.MasterLinId, _motorolaMatrix0X02.MatrixData);
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
        }
        #endregion
    }
}
