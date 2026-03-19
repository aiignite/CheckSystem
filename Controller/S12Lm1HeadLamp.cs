using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace Controller
{
    [Description("CAN-Product,S12L-M1前灯")]
    public sealed class S12Lm1HeadLamp : ControllerBase
    {
        public CanBus Can;

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

        public S12Lm1HeadLamp(string name) : base(name)
        {
            for (var i = 0; i < 30; i++)
                _grayDrlPlLeft.Add(0x00);

            for (var i = 0; i < 33; i++)
                _grayDrlPlRight.Add(0x00);

            MainWorkThread =
                new Thread(MainWork) { IsBackground = true };
            MainWorkThread.Start();
        }

        ~S12Lm1HeadLamp()
        {
            Dispose();
        }

        [Description("开启CAN消息")]
        public void StartCanMsg()
        {
            //ResetLed();
            _isSleep = false;
        }

        [Description("关闭CAN消息")]
        public void StopCaMsg()
        {
            _isSleep = true;
            //ResetLed();
        }

        #region 内部方法

        private Thread MainWorkThread { get; set; }
        private int SendCount { get; set; }
        private bool _isSleep = true;
        private byte _hdlmpLvlngReq;

        private readonly object _lock = new object();

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
                    #region 信号灯
                    if (Can != null && !_isSleep)
                    {
                        var sendPackage = new List<CanBus.CanDataPackage>();

                        //if (SendCount == 1)
                        //    sendPackage.Add(_controlDataPackage);

                        if (IsLeftLamp)
                        {
                            if (_drlPlControlType == DrlPlControlType.DrlEvenOn ||
                                _drlPlControlType == DrlPlControlType.DrlOddOn)
                            {
                                sendPackage.Add(new CanBus.CanDataPackage(0x12D, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { _grayDrlPlLeft[0], 0x00, _grayDrlPlLeft[1], 0x00, _grayDrlPlLeft[2], 0x00, _grayDrlPlLeft[3], _grayDrlPlLeft[4] }));
                                sendPackage.Add(new CanBus.CanDataPackage(0x12E, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, _grayDrlPlLeft[5], 0x00, _grayDrlPlLeft[6], 0x00, _grayDrlPlLeft[7], 0x00, _grayDrlPlLeft[8] }));
                                sendPackage.Add(new CanBus.CanDataPackage(0x12F, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, _grayDrlPlLeft[9], 0x00, _grayDrlPlLeft[10], 0x00, _grayDrlPlLeft[11], 0x00, _grayDrlPlLeft[12] }));
                                sendPackage.Add(new CanBus.CanDataPackage(0x130, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, _grayDrlPlLeft[13], 0x00, _grayDrlPlLeft[14], 0x00, _grayDrlPlLeft[15], 0x00, _grayDrlPlLeft[16] }));
                                sendPackage.Add(new CanBus.CanDataPackage(0x131, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, _grayDrlPlLeft[17], 0x00, _grayDrlPlLeft[18], 0x00, _grayDrlPlLeft[19], 0x00, _grayDrlPlLeft[20] }));
                                sendPackage.Add(new CanBus.CanDataPackage(0x13B, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x00, 0x00, _grayDrlPlLeft[21], 0x00, _grayDrlPlLeft[22], 0x00 }));
                                sendPackage.Add(new CanBus.CanDataPackage(0x13D, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { _grayDrlPlLeft[23], 0x00, _grayDrlPlLeft[24], 0x00, _grayDrlPlLeft[25], 0x00, _grayDrlPlLeft[26], 0x00 }));
                                sendPackage.Add(new CanBus.CanDataPackage(0x13E, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { _grayDrlPlLeft[27], 0x00, _grayDrlPlLeft[28], 0x00, _grayDrlPlLeft[29], 0x00, 0x00, 0x00 }));
                            }
                            else
                            {
                                switch (_drlPlControlType)
                                {
                                    case DrlPlControlType.Off:
                                        sendPackage.Add(new CanBus.CanDataPackage(0x12D, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x12E, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x12F, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x130, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x131, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x13B, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x13D, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x13E, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }));
                                        break;
                                        
                                    case DrlPlControlType.DrlNormalOn:
                                        sendPackage.Add(new CanBus.CanDataPackage(0x12D, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0xFF, 0x00, 0xFF, 0x00, 0xFF, 0x00, 0xFF, 0xFF }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x12E, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0xFF, 0x00, 0xFF, 0x00, 0xFF, 0x00, 0xFF }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x12F, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0xFF, 0x00, 0xFF, 0x00, 0xFF, 0x00, 0xFF }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x130, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0xFF, 0x00, 0xFF, 0x00, 0xFF, 0x00, 0xFF }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x131, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0xFF, 0x00, 0xFF, 0x00, 0xFF, 0x00, 0xFF }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x13B, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x00, 0x00, 0xFF, 0x00, 0xFF, 0x00 }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x13D, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0xFF, 0x00, 0xFF, 0x00, 0xFF, 0x00, 0xFF, 0x00 }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x13E, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0xFF, 0x00, 0xFF, 0x00, 0xFF, 0x00, 0x00, 0x00 }));
                                        break;

                                    case DrlPlControlType.PlNormalOn:
                                        sendPackage.Add(new CanBus.CanDataPackage(0x12D, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x1F, 0x00, 0x1F, 0x00, 0x1F, 0x00, 0x1F, 0x1F }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x12E, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x1F, 0x00, 0x1F, 0x00, 0x1F, 0x00, 0x1F }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x12F, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x1F, 0x00, 0x1F, 0x00, 0x1F, 0x00, 0x1F }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x130, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x1F, 0x00, 0x1F, 0x00, 0x1F, 0x00, 0x1F }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x131, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x1F, 0x00, 0x29, 0x00, 0x1F, 0x00, 0x00 }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x13B, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x29, 0x00, 0x29, 0x00 }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x13D, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x29, 0x00, 0x29, 0x00, 0x29, 0x00, 0x29, 0x00 }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x13E, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x29, 0x00, 0x29, 0x00, 0x29, 0x00, 0x00, 0x00 }));
                                        break;

                                    case DrlPlControlType.DrlOddOn:
                                    case DrlPlControlType.DrlEvenOn:
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                            }

                            sendPackage.Add(new CanBus.CanDataPackage(0x137, CanBus.CanProtocol.Can,
                                CanBus.CanType.Standard, CanBus.CanFormat.Data,
                                new byte[]
                                {
                                    0x00, 0x00, 0x00, 0x00, _isTiOn ? (byte)0x01 : (byte)0x00, 0x00, 0x00, 0x00
                                }));
                        }
                        else
                        {
                            if (_drlPlControlType == DrlPlControlType.DrlEvenOn || _drlPlControlType == DrlPlControlType.DrlOddOn)
                            {
                                sendPackage.Add(new CanBus.CanDataPackage(0x120, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, _grayDrlPlRight[0], 0x00, _grayDrlPlRight[1] }));
                                sendPackage.Add(new CanBus.CanDataPackage(0x121, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { _grayDrlPlRight[2], 0x00, _grayDrlPlRight[3], 0x00, _grayDrlPlRight[4], 0x00, _grayDrlPlRight[5], _grayDrlPlRight[6] }));
                                sendPackage.Add(new CanBus.CanDataPackage(0x122, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, _grayDrlPlRight[7], 0x00, _grayDrlPlRight[8], _grayDrlPlRight[9], _grayDrlPlRight[10], 0x00, _grayDrlPlRight[11] }));
                                sendPackage.Add(new CanBus.CanDataPackage(0x123, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, _grayDrlPlRight[12], 0x00, _grayDrlPlRight[13], _grayDrlPlRight[14], _grayDrlPlRight[15], 0x00, _grayDrlPlRight[16] }));
                                sendPackage.Add(new CanBus.CanDataPackage(0x124, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, _grayDrlPlRight[17], 0x00, _grayDrlPlRight[18], _grayDrlPlRight[19], _grayDrlPlRight[20], 0x00, _grayDrlPlRight[21] }));
                                sendPackage.Add(new CanBus.CanDataPackage(0x125, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, _grayDrlPlRight[22], 0x00, _grayDrlPlRight[23], _grayDrlPlRight[24], _grayDrlPlRight[25], 0x00, 0x00 }));
                                sendPackage.Add(new CanBus.CanDataPackage(0x13A, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, _grayDrlPlRight[26], 0x00, _grayDrlPlRight[27], 0x00, _grayDrlPlRight[28], 0x00 }));
                                sendPackage.Add(new CanBus.CanDataPackage(0x139, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, _grayDrlPlRight[29], 0x00, _grayDrlPlRight[30], 0x00, _grayDrlPlRight[31], 0x00, _grayDrlPlRight[32] }));
                            }
                            else
                            {
                                switch (_drlPlControlType)
                                {
                                    case DrlPlControlType.Off:
                                        sendPackage.Add(new CanBus.CanDataPackage(0x120, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x121, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x122, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x123, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x124, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x125, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x13A, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x139, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }));
                                        break;

                                    case DrlPlControlType.DrlNormalOn:
                                        sendPackage.Add(new CanBus.CanDataPackage(0x120, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0x00, 0xFF }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x121, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0xFF, 0x00, 0xFF, 0x00, 0xFF, 0x00, 0xFF, 0xFF }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x122, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0xFF, 0x00, 0xFF, 0x00, 0xFF, 0x00, 0xFF }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x123, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0xFF, 0x00, 0xFF, 0x00, 0xFF, 0x00, 0xFF }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x124, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0xFF, 0x00, 0xFF, 0x00, 0xFF, 0x00, 0xFF }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x125, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0xFF, 0x00, 0xFF, 0x00, 0xFF, 0x00, 0x00 }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x13A, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0xFF, 0x00, 0xFF, 0x00, 0xFF, 0x00 }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x139, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0xFF, 0x00, 0xFF, 0x00, 0xFF, 0x00, 0xFF }));
                                        break;

                                    case DrlPlControlType.PlNormalOn:
                                        sendPackage.Add(new CanBus.CanDataPackage(0x120, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x29, 0x00, 0x29 }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x121, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x1F, 0x00, 0x1F, 0x00, 0x1F, 0x00, 0x1F, 0x1F }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x122, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x1F, 0x00, 0x1F, 0x00, 0x1F, 0x00, 0x1F }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x123, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x1F, 0x00, 0x1F, 0x00, 0x1F, 0x00, 0x1F }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x124, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x1F, 0x00, 0x1F, 0x00, 0x1F, 0x00, 0x1F }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x125, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x1F, 0x00, 0x29, 0x00, 0x1F, 0x00, 0x00 }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x13A, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x29, 0x00, 0x29, 0x00, 0x29, 0x00 }));
                                        sendPackage.Add(new CanBus.CanDataPackage(0x139, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x29, 0x00, 0x29, 0x00, 0x29, 0x00, 0x29 }));
                                        break;

                                    case DrlPlControlType.DrlOddOn:
                                    case DrlPlControlType.DrlEvenOn:
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                            }

                            sendPackage.Add(new CanBus.CanDataPackage(0x12B, CanBus.CanProtocol.Can,
                                CanBus.CanType.Standard, CanBus.CanFormat.Data,
                                new byte[]
                                {
                                    0x00, 0x00, 0x00, 0x00, _isTiOn ? (byte)0x01 : (byte)0x00, 0x00, 0x00, 0x00
                                }));
                        }

                        lock (_lock)
                            Can.SendCanDatas(sendPackage.ToArray());
                    }
                    #endregion

                    #region LB/HB
                    if (Can != null && !_isSleep)
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

                        lock (_lock)
                        {
                            Can.SendStandardCanData(0x13C, new byte[]
                            {
                                leftLbLedGray, rightLbLedGray, leftHbLedGray, rightHbLedGray, 0x00, 0x00, 0x00, 0x00
                            });
                        }
                    }
                    #endregion

                    #region Motor
                    if (Can != null && !_isSleep)
                    {
                        lock (_lock)
                        {
                            Can.SendStandardCanData(0x303, new byte[] { _hdlmpLvlngReq, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                        }
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

        #region LED control

        private bool _isTiOn = false;
        private DrlPlControlType _drlPlControlType = DrlPlControlType.Off;
        private List<byte> _grayDrlPlLeft = new List<byte>();
        private List<byte> _grayDrlPlRight = new List<byte>();

        [Description("DRL打开")]
        public void DrlOn()
        {
            DrlPlOff();
            _drlPlControlType = DrlPlControlType.DrlNormalOn;
        }

        [Description("PL打开")]
        public void PlOn()
        {
            DrlPlOff();
            _drlPlControlType = DrlPlControlType.PlNormalOn;
        }

        [Description("DRL单数打开")]
        public void DrlOddOn()
        {
            DrlPlOff();

            _grayDrlPlLeft[2] = 0x05;
            _grayDrlPlLeft[4] = 0x05;
            _grayDrlPlLeft[6] = 0x05;
            _grayDrlPlLeft[8] = 0x05;
            _grayDrlPlLeft[11] = 0x05;
            _grayDrlPlLeft[13] = 0x05;
            _grayDrlPlLeft[16] = 0x05;
            _grayDrlPlLeft[18] = 0x05;
            _grayDrlPlLeft[21] = 0x05;
            _grayDrlPlLeft[23] = 0x05;
            _grayDrlPlLeft[1] = 0x05;
            _grayDrlPlLeft[27] = 0x05;
            _grayDrlPlLeft[29] = 0x05;

            _grayDrlPlRight[2] = 0x05;
            _grayDrlPlRight[4] = 0x05;
            _grayDrlPlRight[6] = 0x05;
            _grayDrlPlRight[8] = 0x05;
            _grayDrlPlRight[11] = 0x05;
            _grayDrlPlRight[13] = 0x05;
            _grayDrlPlRight[16] = 0x05;
            _grayDrlPlRight[18] = 0x05;
            _grayDrlPlRight[21] = 0x05;
            _grayDrlPlRight[23] = 0x05;
            _grayDrlPlRight[1] = 0x05;
            _grayDrlPlRight[27] = 0x05;
            _grayDrlPlRight[29] = 0x05;
            _grayDrlPlRight[31] = 0x05;

            _drlPlControlType = DrlPlControlType.DrlOddOn;
        }

        [Description("DRL双数打开")]
        public void DrlEvenOn()
        {
            DrlPlOff();

            _grayDrlPlLeft[3] = 0x05;
            _grayDrlPlLeft[5] = 0x05;
            _grayDrlPlLeft[7] = 0x05;
            _grayDrlPlLeft[10] = 0x05;
            _grayDrlPlLeft[12] = 0x05;
            _grayDrlPlLeft[15] = 0x05;
            _grayDrlPlLeft[17] = 0x05;
            _grayDrlPlLeft[20] = 0x05;
            _grayDrlPlLeft[22] = 0x05;
            _grayDrlPlLeft[0] = 0x05;
            _grayDrlPlLeft[26] = 0x05;
            _grayDrlPlLeft[28] = 0x05;

            _grayDrlPlRight[3] = 0x05;
            _grayDrlPlRight[5] = 0x05;
            _grayDrlPlRight[7] = 0x05;
            _grayDrlPlRight[10] = 0x05;
            _grayDrlPlRight[12] = 0x05;
            _grayDrlPlRight[15] = 0x05;
            _grayDrlPlRight[17] = 0x05;
            _grayDrlPlRight[20] = 0x05;
            _grayDrlPlRight[22] = 0x05;
            _grayDrlPlRight[0] = 0x05;
            _grayDrlPlRight[26] = 0x05;
            _grayDrlPlRight[28] = 0x05;
            _grayDrlPlRight[30] = 0x05;
            _grayDrlPlRight[32] = 0x05;

            _drlPlControlType = DrlPlControlType.DrlEvenOn;
        }

        [Description("DRL/PL关闭")]
        public void DrlPlOff()
        {
            for (var i = 0; i < _grayDrlPlLeft.Count; i++)
                _grayDrlPlLeft[i] = 0x00;
            for (var i = 0; i < _grayDrlPlRight.Count; i++)
                _grayDrlPlRight[i] = 0x00;

            _drlPlControlType = DrlPlControlType.Off;
        }

        [Description("TI打开")]
        public void TiOn()
        {
            _isTiOn = true;
        }

        [Description("TI关闭")]
        public void TiOff()
        {
            _isTiOn = false;
        }

        internal enum DrlPlControlType
        {
            Off,

            /// <summary>
            /// 单数
            /// </summary>
            DrlOddOn,

            /// <summary>
            /// 双数
            /// </summary>
            DrlEvenOn,

            DrlNormalOn,

            PlNormalOn
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

        #region Version

        [Description("R,Software Version")]
        public string SwVer = string.Empty;

        [Description("READ SwVer")]
        public void ReadSwVer()
        {
            SwVer = string.Empty;

            if (Can == null)
                return;

            uint reqCanID = 0x711;
            uint recvCanId = 0x719;

            if (!IsLeftLamp)
            {
                reqCanID = 0x712;
                recvCanId = 0x71A;
            }

            lock (_lock)
            {
                byte[] echoBytes;
                if (Can.CanBusWithUds.TryReadData(reqCanID, recvCanId, CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xF1, 0x89, out echoBytes))
                {
                    SwVer = echoBytes.GetStringByAsciiBytes(false);
                }
                else
                {
                    Thread.Sleep(50);

                    if (Can.CanBusWithUds.TryReadData(reqCanID, recvCanId, CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xF1, 0x89, out echoBytes))
                    {
                        SwVer = echoBytes.GetStringByAsciiBytes(false);
                    }
                }
            }
        }

        #endregion
    }
}
