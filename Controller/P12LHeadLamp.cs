using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using CommonUtility.BusLoader;

namespace Controller
{
    [Description("CAN-Product,P12L前灯-融合方案")]
    public sealed class P12LHeadLamp : ControllerBase
    {
        public CanBus Can;

        [Description("R/W,是否为左灯")]
        public bool IsLeftLamp = false;

        /// <summary>
        /// 右远光
        /// </summary>
        [Description("R/W,右远光")]
        public string RightHbLedGray;

        /// <summary>
        /// 左远光
        /// </summary>
        [Description("R/W,左远光")]
        public string LeftHbLedGray;

        /// <summary>
        /// 右近光
        /// </summary>
        [Description("R/W,右近光")]
        public string RightLbLedGray;

        /// <summary>
        /// 左近光
        /// </summary>
        [Description("R/W,左近光")]
        public string LeftLbLedGray;

        [Description("R/W,是否影线控制控制")]
        public bool HdlampRespsCmd = false;

        public P12LHeadLamp(string name)
            : base(name)
        {
            for (var i = 0; i < 50; i++)
            {
                _leftDrlPlGray.Add(0);
                _rightDrlPlGray.Add(0);
            }

            for (var i = 0; i < 44; i++)
            {
                _leftTiGray.Add(0);
                _rightTiGray.Add(0);
            }

            if (MainWorkThread != null)
            {
                MainWorkThread.Abort();
                MainWorkThread.Join();
            }

            MainWorkThread =
                new Thread(MainWork) { IsBackground = true };
            MainWorkThread.Start();
        }

        ~P12LHeadLamp()
        {
            Dispose();
        }

        [Description("开启CAN消息")]
        public void StartCanLinMsg()
        {
            //ResetLed();
            _isSleep = false;
        }

        [Description("关闭CAN消息")]
        public void StopCanLinMsg()
        {
            _isSleep = true;
            //ResetLed();
        }

        #region 内部方法

        private readonly List<byte> _leftDrlPlGray = new List<byte>();
        private readonly List<byte> _rightDrlPlGray = new List<byte>();

        private readonly List<byte> _leftTiGray = new List<byte>();
        private readonly List<byte> _rightTiGray = new List<byte>();

        private Thread MainWorkThread { get; set; }
        private bool _isSleep = true;
        private byte _hdlmpLvlngReq;

        private void MainWork()
        {
            while (MainWorkThread.IsAlive)
            {
                if (!MainWorkThread.IsAlive)
                    break;

                Thread.Sleep(20);

                if (Can == null)
                    continue;

                if (_isSleep)
                    continue;

                var sendCanDats = new List<CanBus.CanDataPackage>();

                #region 信号灯
                if (IsLeftLamp)
                {
                    // DRL PL
                    var listDrlPlBytes = new List<byte[]>();
                    for (var i = 0; i < _leftDrlPlGray.Count - 2; i = i + 8)
                    {
                        var bs = new byte[8];
                        bs[0] = _leftDrlPlGray[i];
                        bs[1] = _leftDrlPlGray[i + 1];
                        bs[2] = _leftDrlPlGray[i + 2];
                        bs[3] = _leftDrlPlGray[i + 3];
                        bs[4] = _leftDrlPlGray[i + 4];
                        bs[5] = _leftDrlPlGray[i + 5];
                        bs[6] = _leftDrlPlGray[i + 6];
                        bs[7] = _leftDrlPlGray[i + 7];
                        listDrlPlBytes.Add(bs);
                    }

                    for (var i = 0x12d; i <= 0x132; i++)
                        sendCanDats.Add(new CanBus.CanDataPackage((uint)i, CanBus.CanProtocol.Can,
                                 CanBus.CanType.Standard, CanBus.CanFormat.Data, listDrlPlBytes[i - 0x12D]));

                    sendCanDats.Add(new CanBus.CanDataPackage(0x12c, CanBus.CanProtocol.Can,
                        CanBus.CanType.Standard, CanBus.CanFormat.Data,
                        new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, _leftDrlPlGray[48], _leftDrlPlGray[49], 0x00 }));

                    //sendCanDats.Add(new CanBus.CanDataPackage(0x214, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { _leftDrlPlGray[0], 0x00, _leftDrlPlGray[0], 0x00, _leftDrlPlGray[0], 0x00, 0x00, 0x00 }));
                    //// 0x215
                    //// 0x216
                    //// ......

                    // TI
                    var listTiBytes = new List<byte[]>();
                    for (var i = 0; i < _leftTiGray.Count - 4; i += 8)
                    {
                        var bs = new byte[8];
                        bs[0] = _leftTiGray[i];
                        bs[1] = _leftTiGray[i + 1];
                        bs[2] = _leftTiGray[i + 2];
                        bs[3] = _leftTiGray[i + 3];
                        bs[4] = _leftTiGray[i + 4];
                        bs[5] = _leftTiGray[i + 5];
                        bs[6] = _leftTiGray[i + 6];
                        bs[7] = _leftTiGray[i + 7];
                        listTiBytes.Add(bs);
                    }

                    for (var i = 0x133; i <= 0x137; i++)
                        sendCanDats.Add(new CanBus.CanDataPackage((uint)i, CanBus.CanProtocol.Can,
                                     CanBus.CanType.Standard, CanBus.CanFormat.Data, listTiBytes[i - 0x133]));

                    sendCanDats.Add(new CanBus.CanDataPackage(0x138, CanBus.CanProtocol.Can,
                                 CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { _leftTiGray[40], _leftTiGray[41], _leftTiGray[42], _leftTiGray[43], 0x00, 0x00, 0x00, 0x00 }));
                }
                else
                {
                    // DRL PL
                    var listDrlPlBytes = new List<byte[]>();
                    for (var i = 0; i < _rightDrlPlGray.Count - 2; i = i + 8)
                    {
                        var bs = new byte[8];
                        bs[0] = _rightDrlPlGray[i];
                        bs[1] = _rightDrlPlGray[i + 1];
                        bs[2] = _rightDrlPlGray[i + 2];
                        bs[3] = _rightDrlPlGray[i + 3];
                        bs[4] = _rightDrlPlGray[i + 4];
                        bs[5] = _rightDrlPlGray[i + 5];
                        bs[6] = _rightDrlPlGray[i + 6];
                        bs[7] = _rightDrlPlGray[i + 7];
                        listDrlPlBytes.Add(bs);
                    }

                    for (var i = 0x121; i <= 0x126; i++)
                        sendCanDats.Add(new CanBus.CanDataPackage((uint)i, CanBus.CanProtocol.Can,
                                 CanBus.CanType.Standard, CanBus.CanFormat.Data, listDrlPlBytes[i - 0x121]));

                    sendCanDats.Add(new CanBus.CanDataPackage(0x120, CanBus.CanProtocol.Can,
                        CanBus.CanType.Standard, CanBus.CanFormat.Data,
                        new byte[] { _rightDrlPlGray[48], _rightDrlPlGray[49], 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }));

                    // TI
                    var listTiBytes = new List<byte[]>();
                    for (var i = 0; i < _rightTiGray.Count - 4; i = i + 8)
                    {
                        var bs = new byte[8];
                        bs[0] = _rightTiGray[i];
                        bs[1] = _rightTiGray[i + 1];
                        bs[2] = _rightTiGray[i + 2];
                        bs[3] = _rightTiGray[i + 3];
                        bs[4] = _rightTiGray[i + 4];
                        bs[5] = _rightTiGray[i + 5];
                        bs[6] = _rightTiGray[i + 6];
                        bs[7] = _rightTiGray[i + 7];
                        listTiBytes.Add(bs);
                    }

                    for (var i = 0x127; i <= 0x12B; i++)
                        sendCanDats.Add(new CanBus.CanDataPackage((uint)i, CanBus.CanProtocol.Can,
                                     CanBus.CanType.Standard, CanBus.CanFormat.Data, listTiBytes[i - 0x127]));

                    sendCanDats.Add(new CanBus.CanDataPackage(0x12C, CanBus.CanProtocol.Can,
                                 CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { _rightTiGray[40], _rightTiGray[41], _rightTiGray[42], _rightTiGray[43], 0x00, 0x00, 0x00, 0x00 }));
                }
                #endregion

                #region 远近光
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

                sendCanDats.Add(new CanBus.CanDataPackage(0x13C, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                    CanBus.CanFormat.Data,
                    new byte[] { leftLbLedGray, rightLbLedGray, leftHbLedGray, rightHbLedGray, 0x00, 0x00, 0x00, 0x00 }));
                #endregion

                #region motor

                byte motorValue = 0x00;
                switch (_hdlmpLvlngReq)
                {
                    case 0x00:
                        motorValue = 0x00;
                        break;

                    case 0x01:
                        motorValue = 0x20;
                        break;

                    case 0x02:
                        motorValue = 0x40;
                        break;

                    case 0x03:
                        motorValue = 0x60;
                        break;

                    case 0x04:
                        motorValue = 0x80;
                        break;

                    case 0x05:
                        motorValue = 0xA0;
                        break;

                    case 0x06:
                        motorValue = 0xC0;
                        break;

                    case 0x07:
                        motorValue = 0xE0;
                        break;
                }

                sendCanDats.Add(new CanBus.CanDataPackage(0x303, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                    CanBus.CanFormat.Data,
                    new byte[] { motorValue, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }));

                // HdlampRespsCmd

                sendCanDats.Add(new CanBus.CanDataPackage(0x139, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                   CanBus.CanFormat.Data,
                   new byte[] { HdlampRespsCmd ? (byte)0x01 : (byte)0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }));

                #endregion

                Can.SendCanDatas(sendCanDats.ToArray());
            }
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

        #region LED Control

        [Description("DRL/PL打开")]
        public void DrlPlOn(string grayValue)
        {
            if (IsLeftLamp)
            {
                for (var i = 0; i < _leftDrlPlGray.Count; i++)
                    WLedSingleControl(i.ToString(), grayValue);
            }
            else
            {
                for (var i = 0; i < _rightDrlPlGray.Count; i++)
                    WLedSingleControl(i.ToString(), grayValue);
            }
        }

        [Description("DRL/PL关闭")]
        public void DrlPlOff()
        {
            if (IsLeftLamp)
            {
                for (var i = 0; i < _leftDrlPlGray.Count; i++)
                    WLedSingleControl(i.ToString(), "0");
            }
            else
            {
                for (var i = 0; i < _rightDrlPlGray.Count; i++)
                    WLedSingleControl(i.ToString(), "0");
            }
        }

        private void WLedSingleControl(string ledIndex, string grayValue)
        {
            if (string.IsNullOrEmpty(grayValue))
                return;

            byte gray;
            if (!byte.TryParse(grayValue, out gray))
                return;

            int index;
            if (!int.TryParse(ledIndex, out index))
                return;

            if (IsLeftLamp)
            {
                if (index < 0 || index > _leftDrlPlGray.Count)
                    return;

                //index--;
                _leftDrlPlGray[index] = gray;
            }
            else
            {
                if (index < 0 || index > _rightDrlPlGray.Count)
                    return;

                //index--;
                _rightDrlPlGray[index] = gray;
            }
        }

        [Description("TI打开")]
        public void TiOn(string grayValue)
        {
            if (IsLeftLamp)
            {
                for (var i = 0; i < _leftTiGray.Count; i++)
                {
                    YLedSingleControl(i.ToString(), grayValue);
                }
            }
            else
            {
                for (var i = 0; i < _rightTiGray.Count; i++)
                {
                    YLedSingleControl(i.ToString(), grayValue);
                }
            }
        }

        [Description("TI关闭")]
        public void TiOff()
        {
            if (IsLeftLamp)
            {
                for (var i = 0; i < _leftTiGray.Count; i++)
                {
                    YLedSingleControl(i.ToString(), "0");
                }
            }
            else
            {
                for (var i = 0; i < _rightTiGray.Count; i++)
                {
                    YLedSingleControl(i.ToString(), "0");
                }
            }
        }

        private void YLedSingleControl(string ledIndex, string grayValue)
        {
            if (string.IsNullOrEmpty(grayValue))
                return;

            byte gray;
            if (!byte.TryParse(grayValue, out gray))
                return;

            int index;
            if (!int.TryParse(ledIndex, out index))
                return;

            if (IsLeftLamp)
            {
                if (index < 0 || index > _leftTiGray.Count)
                    return;

                //index--;
                _leftTiGray[index] = gray;
            }
            else
            {
                if (index < 0 || index > _rightTiGray.Count)
                    return;

                //index--;
                _rightTiGray[index] = gray;
            }
        }

        [Description("WLed单独打开")]
        public void WLedSingleOn(string ledIndex, string grayValue)
        {
            WLedSingleControl(ledIndex, grayValue);
        }

        [Description("WLed单独关闭")]
        public void WLedSingleOff(string ledIndex)
        {
            WLedSingleControl(ledIndex, "0");
        }

        [Description("YLed单独打开")]
        public void YLedSingleOn(string ledIndex, string grayValue)
        {
            YLedSingleControl(ledIndex, grayValue);
        }

        [Description("YLed单独关闭")]
        public void YLedSingleOff(string ledIndex)
        {
            YLedSingleControl(ledIndex, "0");
        }

        [Description("Led全部关闭")]
        public void LedAllOff()
        {
            for (var i = 0; i < _leftDrlPlGray.Count; i++)
                _leftDrlPlGray[i] = 0x00;
            for (var i = 0; i < _leftTiGray.Count; i++)
                _leftTiGray[i] = 0x00;
            for (var i = 0; i < _rightDrlPlGray.Count; i++)
                _rightDrlPlGray[i] = 0x00;
            for (var i = 0; i < _rightTiGray.Count; i++)
                _rightTiGray[i] = 0x00;
        }

        [Description("打开侧转")]
        public void SideTurnOn()
        {
            //for (var i = 0; i < _turnGray.Length; i++)
            //    _turnGray[i] = 0xFF;
        }

        [Description("关闭侧转")]
        public void SideTurnOff()
        {
            //for (var i = 0; i < _turnGray.Length; i++)
            //    _turnGray[i] = 0x00;
        }

        #endregion
    }
}
