using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using CommonUtility.BusLoader;

namespace Controller
{
    [Description("CAN-Product,S12L高配前灯")]
    public sealed class S12LHighLevelHeadLamp : ControllerBase
    {
        public CanBus SignalLampCan1;
        public CanBus LbHbCan2;
        public LinBus IccLin6;

        [Description("R/W,是否为左灯")]
        public bool IsLeftLamp = false;

        [Description("R/W,LeftLBLedGray")]
        public string LeftLbLedGray;

        [Description("R/W,RightLBLedGray")]
        public string RightLbLedGray;

        [Description("R/W,LeftHBLedGray")]
        public string LeftHbLedGray;

        [Description("R/W,RightHBLedGray")]
        public string RightHbLedGray;

        public S12LHighLevelHeadLamp(string name)
            : base(name)
        {
            if (MainWorkThread != null)
            {
                MainWorkThread.Abort();
                MainWorkThread.Join();
            }

            MainWorkThread =
                new Thread(MainWork) { IsBackground = true };
            MainWorkThread.Start();
        }

        ~S12LHighLevelHeadLamp()
        {
            Dispose();
        }

        #region 内部方法

        private Thread MainWorkThread { get; set; }
        private int SendCount { get; set; }

        private readonly CanBus.CanDataPackage _controlDataPackage =
            new CanBus.CanDataPackage(0xFF, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data,
                new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });

        private bool _isSleep = true;

        private readonly byte[] _turnGray = new byte[8];
        private readonly byte[] _ledGray = new byte[(0x10A - 0x101 + 1) * 8];
        private byte _hdlmpLvlngReq;

        private void MainWork()
        {
            while (MainWorkThread.IsAlive)
            {
                if (!MainWorkThread.IsAlive)
                    break;

                Thread.Sleep(20);

                if (SendCount > 5)
                    SendCount = 0;
                SendCount++;

                try
                {
                    #region 信号灯CAN1
                    if (SignalLampCan1 != null && !_isSleep)
                    {
                        var sendPackage = new List<CanBus.CanDataPackage>();

                        if (SendCount == 1)
                            sendPackage.Add(_controlDataPackage);

                        if (!IsLeftLamp)
                        {
                            sendPackage.Add(
                                new CanBus.CanDataPackage(
                                    0x100, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, _turnGray));

                            var ledR = 0;
                            for (var i = 0x101; i <= 0x10A; i++)
                            {
                                var bs = new byte[8];
                                for (var j = 0; j < 8; j++)
                                {
                                    bs[j] = _ledGray[ledR];
                                    ledR++;
                                }

                                sendPackage.Add(
                                    new CanBus.CanDataPackage(
                                        (uint)i, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, bs));
                            }
                        }
                        else
                        {
                            sendPackage.Add(
                                new CanBus.CanDataPackage(
                                    0x200, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, _turnGray));

                            var ledL = 0;
                            for (var i = 0x201; i <= 0x20A; i++)
                            {
                                var bs = new byte[8];
                                for (var j = 0; j < 8; j++)
                                {
                                    bs[j] = _ledGray[ledL];
                                    ledL++;
                                }

                                sendPackage.Add(
                                    new CanBus.CanDataPackage(
                                        (uint)i, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, bs));
                            }
                        }

                        SignalLampCan1.SendCanDatas(sendPackage.ToArray());
                    }
                    #endregion

                    #region LB/HB CAN2
                    if (LbHbCan2 != null && !_isSleep)
                    {
                        byte leftLbLedGray;
                        byte rightLbLedGray;
                        byte leftHbLedGray;
                        byte rightHbLedGray;

                        if (!byte.TryParse(LeftLbLedGray, out leftLbLedGray))
                        {
                            leftLbLedGray = 0x00;
                        }
                        else
                        {
                            if (leftLbLedGray > 0x64)
                            {
                                leftLbLedGray = 0x64;
                            }
                        }

                        if (!byte.TryParse(RightLbLedGray, out rightLbLedGray))
                        {
                            rightLbLedGray = 0x00;
                        }
                        else
                        {
                            if (rightLbLedGray > 0x64)
                            {
                                rightLbLedGray = 0x64;
                            }
                        }

                        if (!byte.TryParse(LeftHbLedGray, out leftHbLedGray))
                        {
                            leftHbLedGray = 0x00;
                        }
                        else
                        {
                            if (leftHbLedGray > 0x64)
                            {
                                leftHbLedGray = 0x64;
                            }
                        }

                        if (!byte.TryParse(RightHbLedGray, out rightHbLedGray))
                        {
                            rightHbLedGray = 0x00;
                        }
                        else
                        {
                            if (rightHbLedGray > 0x64)
                            {
                                rightHbLedGray = 0x64;
                            }
                        }

                        LbHbCan2.SendStandardCanData(0x20, new byte[]
                        {
                            0x00, leftLbLedGray, rightLbLedGray, 0x00, leftHbLedGray, rightHbLedGray, 0x00, 0x00
                        });
                    }
                    #endregion

                    #region Motor LIN6
                    if (IccLin6 != null && !_isSleep)
                    {
                        var intelMatrix = new LinCommunicationMatrix.IntelMatrix(0x08, 8);
                        var md = new MatrixValDefinition(0, 3, _hdlmpLvlngReq);
                        intelMatrix.UpdateData(md);
                        IccLin6.SendMasterLin(intelMatrix.MasterLinId, intelMatrix.MatrixData);
                    }
                    #endregion
                }
                catch (Exception)
                {
                    // ignoWhite
                }
            }
        }

        #endregion

        [Description("开启CAN/LIN消息")]
        public void StartCanLinMsg()
        {
            //ResetLed();
            _isSleep = false;
        }

        [Description("关闭CAN/LIN消息")]
        public void StopCanLinMsg()
        {
            _isSleep = true;
            //ResetLed();
        }

        #region LED control

        //private void ResetLed()
        //{
        //    for (var i = 0; i < _turnGray.Length; i++)
        //        _turnGray[i] = 0x00;
        //    for (var i = 0; i < _ledGray.Length; i++)
        //        _ledGray[i] = 0x00;
        //}

        [Description("DRL/PL打开")]
        public void DrlPlOn(string grayValue)
        {
            for (var i = 1; i <= 6; i = i + 2)
                LedSingleControl(i.ToString(), grayValue);

            for (var i = 8; i <= 35; i = i + 2)
                LedSingleControl(i.ToString(), grayValue);

            for (var i = 38; i <= 52; i = i + 4)
                LedSingleControl(i.ToString(), grayValue);

            for (var i = 54; i <= 69; i = i + 2)
                LedSingleControl(i.ToString(), grayValue);

            LedSingleControl(7.ToString(), grayValue);
            LedSingleControl(36.ToString(), grayValue);
            LedSingleControl(70.ToString(), grayValue);
        }

        [Description("DRL/PL关闭")]
        public void DrlPlOff()
        {
            for (var i = 1; i <= 6; i = i + 2)
                LedSingleControl(i.ToString(), "0");

            for (var i = 8; i <= 35; i = i + 2)
                LedSingleControl(i.ToString(), "0");

            for (var i = 38; i <= 52; i = i + 4)
                LedSingleControl(i.ToString(), "0");

            for (var i = 54; i <= 69; i = i + 2)
                LedSingleControl(i.ToString(), "0");

            LedSingleControl(7.ToString(), "0");
            LedSingleControl(36.ToString(), "0");
            LedSingleControl(70.ToString(), "0");
        }

        [Description("TI打开")]
        public void TiOn(string grayValue)
        {
            LedSingleControl(0.ToString(), grayValue);

            for (var i = 37; i <= 51; i = i + 4)
                LedSingleControl(i.ToString(), grayValue);
        }

        [Description("TI关闭")]
        public void TiOff()
        {
            LedSingleControl(0.ToString(), "0");

            for (var i = 37; i <= 51; i = i + 4)
                LedSingleControl(i.ToString(), "0");
        }

        //[Description("Led全部打开")]
        //public void LedAllOn(string grayValue)
        //{
        //    if (string.IsNullOrEmpty(grayValue))
        //        return;

        //    byte gray;
        //    if (!byte.TryParse(grayValue, out gray))
        //        return;

        //    for (var i = 0; i < _ledGray.Length; i++)
        //        _ledGray[i] = gray;
        //}

        //[Description("LED流水")]
        //public void LedFlash()
        //{
        //    for (var i = 0; i < _ledGray.Length; i++)
        //    {
        //        LedSingleOn((i + 1).ToString(), "55");
        //        Thread.Sleep(100);
        //    }
        //}

        [Description("Led单独打开")]
        public void LedSingleOn(string ledIndex, string grayValue)
        {
            LedSingleControl(ledIndex, grayValue);
        }

        [Description("Led单独关闭")]
        public void LedSingleOff(string ledIndex)
        {
            LedSingleControl(ledIndex, "0");
        }

        private void LedSingleControl(string ledIndex, string grayValue)
        {
            if (string.IsNullOrEmpty(grayValue))
                return;

            byte gray;
            if (!byte.TryParse(grayValue, out gray))
                return;

            int index;
            if (!int.TryParse(ledIndex, out index))
                return;

            if (index <= 0 || index > _ledGray.Length)
                return;
            index--;
            _ledGray[index] = gray;
        }

        [Description("Led全部关闭")]
        public void LedAllOff()
        {
            for (var i = 0; i < _ledGray.Length; i++)
                _ledGray[i] = 0x00;
        }

        [Description("打开侧转")]
        public void SideTurnOn()
        {
            for (var i = 0; i < _turnGray.Length; i++)
                _turnGray[i] = 0xFF;
        }

        [Description("关闭侧转")]
        public void SideTurnOff()
        {
            for (var i = 0; i < _turnGray.Length; i++)
                _turnGray[i] = 0x00;
        }

        #endregion

        #region Motor Control

        [Description("HdlmpLvlngReq")]
        public void HdlmpLvlngReq(string value)
        {
            byte hdlmpLvlngReq;
            if (!byte.TryParse(value, out hdlmpLvlngReq))
                return;
            if (hdlmpLvlngReq <= 0x07)
                _hdlmpLvlngReq = hdlmpLvlngReq;
        }

        #endregion
    }
}
