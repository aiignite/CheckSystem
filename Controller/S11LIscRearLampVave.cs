using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using CommonUtility;
using CommonUtility.BusLoader;

namespace Controller
{
    [Description("CAN-Product,S11L-ISC-后灯VAVE")]
    public sealed class S11LIscRearLampVave : ControllerBase
    {
        public CanBus CanFd;

        private VectorDbcEmulator VectorEmulator { get; set; }
        private bool IsCanStarted { get; set; }
        private LampType ThisLampType { get; set; }
        private readonly object _canSendLocker = new object();

        public S11LIscRearLampVave(string name)
            : base(name)
        {
            SysConfig(Directory.GetCurrentDirectory() + @"\ControllerConfig\S11L.dbc");
            CanBus.PushCanMsg += CanBus_PushCanMsg;
        }

        #region 主要控制

        [Description("设置为B灯")]
        public void SetRcmm()
        {
            _isSingle = false;
            _isSingleAll = false;
            _reqCanId = 0x7A2;
            _recvCanId = 0x7B2;
            ThisLampType = LampType.Rcmm;
            _pixelCoordA = new byte[128];
            _pixelCoordB = new byte[2048];
            f_A_Ctrl();
            f_B_Ctrl();

            _canFdResponseIdList.Clear();
            for (uint i = 0x6A1; i <= 0x6A4; i++)
                _canFdResponseIdList.Add(i);
        }

        [Description("开启CAN消息")]
        public void StartCanMsg()
        {
            IsCanStarted = true;
        }

        [Description("关闭CAN消息")]
        public void StopCanMsg()
        {
            IsCanStarted = false;
        }

        [Description("开启ISC硬线控制模式")]
        public void IscHardwareOn()
        {
            VectorEmulator.SetMessageVariableSignalValue(IscCtrl, "Responses_cmd", 1);
        }

        [Description("关闭ISC硬线控制模式")]
        public void IscHardwareOff()
        {
            VectorEmulator.SetMessageVariableSignalValue(IscCtrl, "Responses_cmd", 0);
        }

        #endregion

        #region 版本信息

        private uint _reqCanId = 0x7A0;
        private uint _recvCanId = 0x7B0;

        [Description("R,FblVer")]
        public string FblVer = string.Empty;

        [Description("R,AppVer")]
        public string AppVer = string.Empty;

        [Description("R,InternalAppVer")]
        public string InternalAppVer = string.Empty;

        [Description("R,HardwareVer")]
        public string HardwareVer = string.Empty;

        [Description("ReadAppVer")]
        public void ReadAppVer()
        {
            AppVer = string.Empty;

            if (CanFd == null)
                return;

            if (ThisLampType == LampType.Rcml || ThisLampType == LampType.Rcmr)
            {
                AppVer = ReadAppVer(_reqCanId, _recvCanId);
            }
            else if (ThisLampType == LampType.Rcmm)
            {
                // B左1
                AppVer += "B灯左侧1:" + ReadAppVer(0x7A2, 0x7B2) + ";";

                // B左2
                AppVer += "B灯左侧2:" + ReadAppVer(0x7A3, 0x7B3) + ";";

                // B中间
                AppVer += "B灯中间:" + ReadAppVer(0x7A4, 0x7B4) + ";";
            }
        }

        [Description("ReadFblVer")]
        public void ReadFblVer()
        {
            FblVer = string.Empty;

            if (CanFd == null)
                return;

            if (ThisLampType == LampType.Rcml || ThisLampType == LampType.Rcmr)
            {
                FblVer = ReadFblVer(_reqCanId, _recvCanId);
            }
            else if (ThisLampType == LampType.Rcmm)
            {
                // B左1
                FblVer += "B灯左侧1:" + ReadFblVer(0x7A2, 0x7B2) + ";";

                // B左2
                FblVer += "B灯左侧2:" + ReadFblVer(0x7A3, 0x7B3) + ";";

                // B中间
                FblVer += "B灯中间:" + ReadFblVer(0x7A4, 0x7B4) + ";";
            }
        }

        [Description("ReadInternalAppVer")]
        public void ReadInternalAppVer()
        {
            InternalAppVer = string.Empty;

            if (CanFd == null)
                return;

            if (ThisLampType == LampType.Rcml || ThisLampType == LampType.Rcmr)
            {
                InternalAppVer = ReadAppVer(_reqCanId, _recvCanId);
            }
            else if (ThisLampType == LampType.Rcmm)
            {
                // B左1
                InternalAppVer += "B灯左侧1:" + ReadInternalAppVer(0x7A2, 0x7B2) + ";";

                // B左2
                InternalAppVer += "B灯左侧2:" + ReadInternalAppVer(0x7A3, 0x7B3) + ";";

                // B中间
                InternalAppVer += "B灯中间:" + ReadInternalAppVer(0x7A4, 0x7B4) + ";";
            }
        }

        [Description("ReadHardwareVer")]
        public void ReadHardwareVer()
        {
            HardwareVer = string.Empty;

            if (CanFd == null)
                return;

            if (ThisLampType == LampType.Rcml || ThisLampType == LampType.Rcmr)
            {
                HardwareVer = ReadAppVer(_reqCanId, _recvCanId);
            }
            else if (ThisLampType == LampType.Rcmm)
            {
                // B左1
                HardwareVer += "B灯左侧1:" + ReadHardwareVer(0x7A2, 0x7B2) + ";";

                // B左2
                HardwareVer += "B灯左侧2:" + ReadHardwareVer(0x7A3, 0x7B3) + ";";

                // B中间
                HardwareVer += "B灯中间:" + ReadHardwareVer(0x7A4, 0x7B4) + ";";
            }
        }

        private string ReadAppVer(uint reqCanId, uint recvCanId)
        {
            const byte didiHi = (byte)0xF1;
            const byte didLo = (byte)0x95;
            var str = string.Empty;

            lock (_canSendLocker)
            {
                byte[] echo;
                if (CanFd.CanBusWithUds.TryReadData(reqCanId, recvCanId, CanBus.CanType.Standard,
                    CanBus.CanProtocol.CanFd, didiHi, didLo, out echo, 0x00))
                    str = echo.GetStringByAsciiBytes(false);
                else
                {
                    Thread.Sleep(150);
                    if (CanFd.CanBusWithUds.TryReadData(reqCanId, recvCanId, CanBus.CanType.Standard,
                        CanBus.CanProtocol.CanFd, didiHi, didLo, out echo, 0x00))
                        str = echo.GetStringByAsciiBytes(false);
                }
            }

            return str;
        }

        private string ReadFblVer(uint reqCanId, uint recvCanId)
        {
            const byte didiHi = (byte)0x01;
            const byte didLo = (byte)0x00;
            var str = string.Empty;

            lock (_canSendLocker)
            {
                byte[] echo;
                if (CanFd.CanBusWithUds.TryReadData(reqCanId, recvCanId, CanBus.CanType.Standard,
                    CanBus.CanProtocol.CanFd, didiHi, didLo, out echo))
                    str = echo.GetStringByAsciiBytes(false);
                else
                {
                    Thread.Sleep(150);
                    if (CanFd.CanBusWithUds.TryReadData(reqCanId, recvCanId, CanBus.CanType.Standard,
                        CanBus.CanProtocol.CanFd, didiHi, didLo, out echo))
                        str = echo.GetStringByAsciiBytes(false);
                }
            }

            return str;
        }

        private string ReadInternalAppVer(uint reqCanId, uint recvCanId)
        {
            const byte didiHi = (byte)0xF1;
            const byte didLo = (byte)0x97;
            var str = string.Empty;

            lock (_canSendLocker)
            {
                byte[] echo;
                if (CanFd.CanBusWithUds.TryReadData(reqCanId, recvCanId, CanBus.CanType.Standard,
                    CanBus.CanProtocol.CanFd, didiHi, didLo, out echo))
                    str = echo.GetStringByAsciiBytes(false);
                else
                {
                    Thread.Sleep(150);
                    if (CanFd.CanBusWithUds.TryReadData(reqCanId, recvCanId, CanBus.CanType.Standard,
                        CanBus.CanProtocol.CanFd, didiHi, didLo, out echo))
                        str = echo.GetStringByAsciiBytes(false);
                }
            }

            return str;
        }

        private string ReadHardwareVer(uint reqCanId, uint recvCanId)
        {
            const byte didiHi = (byte)0xF1;
            const byte didLo = (byte)0x93;
            var str = string.Empty;

            lock (_canSendLocker)
            {
                byte[] echo;
                if (CanFd.CanBusWithUds.TryReadData(reqCanId, recvCanId, CanBus.CanType.Standard,
                    CanBus.CanProtocol.CanFd, didiHi, didLo, out echo))
                    str = echo.GetStringByAsciiBytes(false);
                else
                {
                    Thread.Sleep(150);
                    if (CanFd.CanBusWithUds.TryReadData(reqCanId, recvCanId, CanBus.CanType.Standard,
                        CanBus.CanProtocol.CanFd, didiHi, didLo, out echo))
                        str = echo.GetStringByAsciiBytes(false);
                }
            }

            return str;
        }

        #endregion

        #region 点灯

        [Description("全部点亮")]
        public void AllLedOn(string gray)
        {
            if (ThisLampType == LampType.Rcmm)
            {
                byte value;
                if (byte.TryParse(gray, out value))
                    f_B_All(value);
            }
            else if (ThisLampType == LampType.Rcml || ThisLampType == LampType.Rcmr)
            {
                if (_isSingle || _isSingleAll)
                {
                    SingleAllOff();
                    SingleOff();
                }

                byte value;
                if (byte.TryParse(gray, out value))
                    f_A_All(value);
            }
        }

        [Description("全部奇数点亮")]
        public void AllOddOn(string gray)
        {
            byte value;
            if (!byte.TryParse(gray, out value))
                return;

            if (_isSingle || _isSingleAll)
            {
                SingleAllOff();
                SingleOff();
            }

            if (ThisLampType == LampType.Rcmm)
            {
                f_B_All(0);

                for (var i = 0; i < 1795; i++)
                {
                    #region 位置灯部分
                    if (i <= 94)
                    {
                        if (i == 0 || i == 6 || i == 12 || i == 18 || i == 24 || i == 30 || i == 36 || i == 42 ||
                            i == 48 || i == 54 || i == 60 || i == 66 || i == 72 || i == 78 || i == 84 || i == 90)
                        {
                            _pixelCoordB[i] = value;
                        }
                    }
                    else if ((i == 508) || (i == 653))
                    {
                        _pixelCoordB[i] = value;
                    }
                    else if ((i >= 821) && (i <= 842))
                    {
                        if (i == 821 || i == 827 || i == 833 || i == 839)
                        {
                            _pixelCoordB[i] = value;
                        }
                    }
                    else if ((i >= 654) && (i <= 675))
                    {
                        if (i == 654 || i == 660 || i == 666 || i == 672)
                        {
                            _pixelCoordB[i] = value;
                        }
                    }
                    #endregion

                    #region 转向灯部分
                    if (
                        (i == 510)
                        || (i == 990)
                        || (i == 1139)
                        || (i == 1443)

                        || (i == 507)
                        || (i == 652)
                        || (i == 1289)
                        || (i == 1138)
                        )
                    {
                        _pixelCoordB[i] = value;
                    }
                    else if ((i >= 1598) && (i <= 1618))
                    {
                        if ((i - 1598) % 4 == 0)
                        {
                            _pixelCoordB[i] = value;
                        }
                    }
                    else if ((i >= 1774) && (i <= 1794))
                    {
                        if ((i - 1774) % 4 == 0)
                        {
                            _pixelCoordB[i] = value;
                        }
                    }
                    #endregion

                    #region ISC部分
                    if ((i >= 96) && (i <= 222))
                    {
                        if ((i - 96) % 2 == 0)
                        {
                            _pixelCoordB[i] = value;
                        }
                    }
                    else if ((i >= 227) && (i <= 363))
                    {
                        if ((i - 227) % 2 == 0)
                        {
                            _pixelCoordB[i] = value;
                        }
                    }
                    else if ((i >= 368) && (i <= 506))
                    {
                        if ((i - 368) % 2 == 0)
                        {
                            _pixelCoordB[i] = value;
                        }
                    }
                    else if ((i >= 511) && (i <= 651))
                    {
                        if ((i - 511) % 2 == 0)
                        {
                            _pixelCoordB[i] = value;
                        }
                    }
                    else if ((i >= 677) && (i <= 819))
                    {
                        if ((i - 677) % 2 == 0)
                        {
                            _pixelCoordB[i] = value;
                        }
                    }
                    else if ((i >= 844) && (i <= 988))
                    {
                        if ((i - 844) % 2 == 0)
                        {
                            _pixelCoordB[i] = value;
                        }
                    }
                    else if ((i >= 991) && (i <= 1137))
                    {
                        if ((i - 991) % 2 == 0)
                        {
                            _pixelCoordB[i] = value;
                        }
                    }
                    else if ((i >= 1140) && (i <= 1288))
                    {
                        if ((i - 1140) % 2 == 0)
                        {
                            _pixelCoordB[i] = value;
                        }
                    }
                    else if ((i >= 1291) && (i <= 1441))
                    {
                        if ((i - 1291) % 2 == 0)
                        {
                            _pixelCoordB[i] = value;
                        }
                    }
                    else if ((i >= 1444) && (i <= 1596))
                    {
                        if ((i - 1444) % 2 == 0)
                        {
                            _pixelCoordB[i] = value;
                        }
                    }
                    else if ((i >= 1619) && (i <= 1773))
                    {
                        if ((i - 1619) % 2 == 0)
                        {
                            _pixelCoordB[i] = value;
                        }
                    }
                    #endregion
                }

                f_B_Ctrl();
            }
            else if (ThisLampType == LampType.Rcml || ThisLampType == LampType.Rcmr)
            {
                f_A_All(0);
                for (var i = 0; i < 68; i++)
                {
                    if (i == 0 || i == 6 || i == 12 || i == 18 || i == 24 || i == 30 || i == 38 || i == 42 || i == 46 || i == 50 || i == 54 || i == 58 || i == 62 || i == 66)
                    {
                        _pixelCoordA[i] = value;
                    }

                    //if (_ledIndexList.Contains(i))
                    //{
                    //    _pixelCoordA[i] = value;
                    //}
                }
                f_A_Ctrl();
            }
        }

        //private List<int> _ledIndexList = new List<int>();

        //[Description("奇数点亮++")]
        //public void AddOddIndex(string index)
        //{
        //    try
        //    {
        //        var value = int.Parse(index);
        //        if (value >= 0 && value < 68)
        //        {
        //            if (!_ledIndexList.Contains(value))
        //            {
        //                _ledIndexList.Add(value);

        //                AllOddOn(20.ToString());
        //            }
        //        }

        //    }
        //    catch (Exception)
        //    {

        //    }
        //}

        //[Description("奇数点亮--")]
        //public void Remove(string index)
        //{
        //    try
        //    {
        //        var value = int.Parse(index);
        //        if (value >= 0 && value < 68)
        //        {
        //            if (_ledIndexList.Contains(value))
        //            {
        //                _ledIndexList.Remove(value);
        //                AllOddOn(20.ToString());
        //            }
        //        }

        //    }
        //    catch (Exception)
        //    {

        //    }
        //}

        [Description("全部偶数点亮")]
        public void AllEvenOn(string gray)
        {
            byte value;
            if (!byte.TryParse(gray, out value))
                return;

            if (value == 0)
                value = 1;

            if (_isSingle || _isSingleAll)
            {
                SingleAllOff();
                SingleOff();
            }

            if (ThisLampType == LampType.Rcmm)
            {
                f_B_All(0);

                for (var i = 0; i < 1795; i++)
                {
                    #region 位置灯部分
                    if (i <= 94)
                    {
                        if (i == 3 || i == 9 || i == 15 || i == 21 || i == 27 || i == 33 || i == 39 || i == 45 ||
                            i == 51 || i == 57 || i == 63 || i == 69 || i == 75 || i == 81 || i == 87 || i == 93)
                        {
                            _pixelCoordB[i] = value;
                        }
                    }
                    else if ((i == 224) || (i == 225) || (i == 365) || (i == 509))
                    {
                        _pixelCoordB[i] = value;
                    }
                    else if ((i >= 821) && (i <= 842))
                    {
                        if (i == 824 || i == 830 || i == 836 || i == 842)
                        {
                            _pixelCoordB[i] = value;
                        }
                    }
                    else if ((i >= 654) && (i <= 675))
                    {
                        if (i == 657 || i == 663 || i == 669 || i == 675)
                        {
                            _pixelCoordB[i] = value;
                        }
                    }

                    #endregion

                    #region 转向灯部分

                    if (
                        (i == 95)
                        || (i == 226)
                        || (i == 676)
                        || (i == 843)
                        || (i == 1290)

                        || (i == 223)
                        || (i == 820)
                        || (i == 989)
                        || (i == 1442)
                        || (i == 1597)
                        )
                    {
                        _pixelCoordB[i] = value;
                    }
                    else if ((i >= 1598) && (i <= 1618))
                    {
                        if ((i - (1598 + 2)) % 4 == 0)
                        {
                            _pixelCoordB[i] = value;
                        }
                    }
                    else if ((i >= 1774) && (i <= 1794))
                    {
                        if ((i - (1774 + 2)) % 4 == 0)
                        {
                            _pixelCoordB[i] = value;
                        }
                    }

                    #endregion

                    #region ISC部分
                    if ((i >= 96) && (i <= 222))
                    {
                        if ((i - (96 + 1)) % 2 == 0)
                        {
                            _pixelCoordB[i] = value;
                        }
                    }
                    else if ((i >= 227) && (i <= 363))
                    {
                        if ((i - (227 + 1)) % 2 == 0)
                        {
                            _pixelCoordB[i] = value;
                        }
                    }
                    else if ((i >= 368) && (i <= 506))
                    {
                        if ((i - (368 + 1)) % 2 == 0)
                        {
                            _pixelCoordB[i] = value;
                        }
                    }
                    else if ((i >= 511) && (i <= 651))
                    {
                        if ((i - (511 + 1)) % 2 == 0)
                        {
                            _pixelCoordB[i] = value;
                        }
                    }
                    else if ((i >= 677) && (i <= 819))
                    {
                        if ((i - (677 + 1)) % 2 == 0)
                        {
                            _pixelCoordB[i] = value;
                        }
                    }
                    else if ((i >= 844) && (i <= 988))
                    {
                        if ((i - (844 + 1)) % 2 == 0)
                        {
                            _pixelCoordB[i] = value;
                        }
                    }
                    else if ((i >= 991) && (i <= 1137))
                    {
                        if ((i - (991 + 1)) % 2 == 0)
                        {
                            _pixelCoordB[i] = value;
                        }
                    }
                    else if ((i >= 1140) && (i <= 1288))
                    {
                        if ((i - (1140 + 1)) % 2 == 0)
                        {
                            _pixelCoordB[i] = value;
                        }
                    }
                    else if ((i >= 1291) && (i <= 1441))
                    {
                        if ((i - (1291 + 1)) % 2 == 0)
                        {
                            _pixelCoordB[i] = value;
                        }
                    }
                    else if ((i >= 1444) && (i <= 1596))
                    {
                        if ((i - (1444 + 1)) % 2 == 0)
                        {
                            _pixelCoordB[i] = value;
                        }
                    }
                    else if ((i >= 1619) && (i <= 1773))
                    {
                        if ((i - (1619 + 1)) % 2 == 0)
                        {
                            _pixelCoordB[i] = value;
                        }
                    }
                    #endregion
                }

                f_B_Ctrl();
            }
            else if (ThisLampType == LampType.Rcml || ThisLampType == LampType.Rcmr)
            {
                f_A_All(0);
                for (var i = 0; i < 68; i++)
                {
                    if (i == 3 || i == 9 || i == 15 || i == 21 || i == 27 || i == 33 || i == 36 || i == 40 || i == 44 || i == 48 || i == 52 || i == 56 || i == 60 || i == 64)
                    {
                        _pixelCoordA[i] = value;
                    }
                }
                f_A_Ctrl();
            }
        }

        [Description("全部关闭")]
        public void AllLedOff()
        {
            if (ThisLampType == LampType.Rcmm)
                f_B_All(0);
            else if (ThisLampType == LampType.Rcml || ThisLampType == LampType.Rcmr)
                f_A_All(0);

            SingleOff();
            SingleAllOff();
        }

        private bool _isSingle;
        private bool _isSingleAll;
        private int _ledIndex;

        [Description("单颗扫描开")]
        public void SingleOn()
        {
            _ledIndex = 0;
            f_A_All(0);
            f_B_All(0);
            Thread.Sleep(50);
            _isSingle = true;
            _isSingleAll = false;
        }

        [Description("单颗扫描关")]
        public void SingleOff()
        {
            _isSingle = false;
            _isSingleAll = false;
            f_A_All(0);
            f_B_All(0);
            _ledIndex = 0;
        }

        [Description("逐颗点亮开")]
        public void SingleAllOn()
        {
            _ledIndex = 0;
            f_A_All(0);
            f_B_All(0);
            Thread.Sleep(50);
            _isSingle = false;
            _isSingleAll = true;
        }

        [Description("逐颗点亮关")]
        public void SingleAllOff()
        {
            _isSingle = false;
            _isSingleAll = false;
            f_A_All(0);
            f_B_All(0);
            _ledIndex = 0;
        }

        private void f_A_All(byte value)
        {
            for (var i = 0; i < 128; i++)
                _pixelCoordA[i] = value;
            f_A_Ctrl();
        }

        private void f_B_All(byte value)
        {
            for (var i = 0; i < 2048; i++)
                _pixelCoordB[i] = value;
            f_B_Ctrl();
        }

        [Description("位置灯亮")]
        public void TailNormalOn(string gray)
        {
            if (_isSingle || _isSingleAll)
            {
                SingleAllOff();
                SingleOff();
            }

            if (ThisLampType == LampType.Rcmm)
            {
                byte value;
                if (byte.TryParse(gray, out value))
                    f_B_Position_BREAK(value);
            }
            else if (ThisLampType == LampType.Rcml || ThisLampType == LampType.Rcmr)
            {
                byte value;
                if (byte.TryParse(gray, out value))
                    f_A_Position_Break(value);
            }
        }

        [Description("位置灯高亮-仅A灯")]
        public void TailHdOn()
        {
            if (_isSingle || _isSingleAll)
            {
                SingleAllOff();
                SingleOff();
            }

            if (ThisLampType == LampType.Rcml || ThisLampType == LampType.Rcmr)
            {

                f_A_Position_Break(102);
            }
        }

        [Description("位置灯低亮-仅A灯")]
        public void TailLdOn()
        {
            if (ThisLampType == LampType.Rcml || ThisLampType == LampType.Rcmr)
            {
                if (_isSingle || _isSingleAll)
                {
                    SingleAllOff();
                    SingleOff();
                }
                f_A_Position_Break(22);
            }
        }

        [Description("左位置灯亮-仅B灯")]
        public void TailLeftOn(string gray)
        {
            if (_isSingle || _isSingleAll)
            {
                SingleAllOff();
                SingleOff();
            }

            if (ThisLampType == LampType.Rcmm)
            {
                byte value;
                if (byte.TryParse(gray, out value))
                    f_B_Position_BREAK(value, 1);
            }
        }

        [Description("右位置灯亮-仅B灯")]
        public void TailRightOn(string gray)
        {
            if (_isSingle || _isSingleAll)
            {
                SingleAllOff();
                SingleOff();
            }

            if (ThisLampType == LampType.Rcmm)
            {
                byte value;
                if (byte.TryParse(gray, out value))
                    f_B_Position_BREAK(value, 2);
            }
        }

        [Description("位置灯关闭")]
        public void TailOff()
        {
            if (_isSingle || _isSingleAll)
            {
                SingleAllOff();
                SingleOff();
            }

            if (ThisLampType == LampType.Rcmm)
                f_B_Position_BREAK(0);
            else if (ThisLampType == LampType.Rcml || ThisLampType == LampType.Rcmr)
            {
                f_A_Position_Break(0);
            }
        }

        private void f_A_Position_Break(byte value)
        {
            for (var i = 0; i < 35; i++)
                _pixelCoordA[i] = value;
            f_A_Ctrl();
        }

        private void f_B_Position_BREAK(byte value)
        {
            for (var i = 0; i < 1795; i++)
            {
                if ((i <= 94) || (i == 224) || (i == 225) || (i == 366) ||
                    (i == 509) || (i == 365) || (i == 508) || (i == 653))
                    _pixelCoordB[i] = value;
                else if ((i >= 821) && (i <= 842))
                    _pixelCoordB[i] = value;
                else if ((i >= 654) && (i <= 675))
                    _pixelCoordB[i] = value;
            }

            f_B_Ctrl();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">灰度值</param>
        /// <param name="pos">1=L，2=R</param>
        private void f_B_Position_BREAK(byte value, int pos)
        {
            for (var i = 0; i < 1795; i++)
            {
                if ((i <= 94) || (i == 224) || (i == 225) || (i == 366) ||
                    (i == 509) || (i == 365) || (i == 508) || (i == 653))
                {
                    if (pos == 2)
                    {
                        // 224 365 508
                        if (i == 224 || i == 365 || i == 508 || i == 91 || i == 93 || (i >= 46 && i <= 90))
                        {
                            _pixelCoordB[i] = value;
                        }
                        else
                        {
                            _pixelCoordB[i] = 0x00;
                        }
                    }
                    else if (pos == 1)
                    {
                        if (i == 225 || i == 366 || i == 509 || i == 94 || i == 92 || (i >= 0 && i <= 45))
                        {
                            _pixelCoordB[i] = value;
                        }
                        else
                        {
                            _pixelCoordB[i] = 0x00;
                        }
                    }
                }
                else if ((i >= 821) && (i <= 842))
                {
                    if (pos == 2)
                    {
                        _pixelCoordB[i] = value;
                    }
                    else
                    {
                        _pixelCoordB[i] = 0x00;
                    }
                }
                else if ((i >= 654) && (i <= 675))
                {
                    if (pos == 1)
                    {
                        _pixelCoordB[i] = value;
                    }
                    else
                    {
                        _pixelCoordB[i] = 0x00;
                    }
                }
            }

            f_B_Ctrl();
        }

        [Description("转向灯亮")]
        public void TurnNormalOn(string gray)
        {
            if (_isSingle || _isSingleAll)
            {
                SingleAllOff();
                SingleOff();
            }

            if (ThisLampType == LampType.Rcmm)
            {
                byte value;
                if (byte.TryParse(gray, out value))
                    f_B_Turn(value);
            }
            else if (ThisLampType == LampType.Rcml || ThisLampType == LampType.Rcmr)
            {
                byte value;
                if (byte.TryParse(gray, out value))
                    f_A_Turn(value);
            }
        }

        [Description("转向灯高亮-仅A灯")]
        public void TurnHdOn()
        {
            if (ThisLampType == LampType.Rcml || ThisLampType == LampType.Rcmr)
            {
                if (_isSingle || _isSingleAll)
                {
                    SingleAllOff();
                    SingleOff();
                }
                f_A_Turn(102);
            }
        }

        [Description("转向灯低亮-仅A灯")]
        public void TurnLdOn()
        {
            if (ThisLampType == LampType.Rcml || ThisLampType == LampType.Rcmr)
            {
                if (_isSingle || _isSingleAll)
                {
                    SingleAllOff();
                    SingleOff();
                }
                f_A_Turn(63);
            }
        }

        [Description("左转向灯亮-仅B灯")]
        public void TurnLeftOn(string gray)
        {
            if (_isSingle || _isSingleAll)
            {
                SingleAllOff();
                SingleOff();
            }

            if (ThisLampType == LampType.Rcmm)
            {
                byte value;
                if (byte.TryParse(gray, out value))
                    f_B_Turn(value, true);
            }
        }

        [Description("右转向灯亮-仅B灯")]
        public void TurnRightOn(string gray)
        {
            if (_isSingle || _isSingleAll)
            {
                SingleAllOff();
                SingleOff();
            }

            if (ThisLampType == LampType.Rcmm)
            {
                byte value;
                if (byte.TryParse(gray, out value))
                    f_B_Turn(value, false);
            }
        }

        [Description("转向灯关闭")]
        public void TurnOff()
        {
            if (_isSingle || _isSingleAll)
            {
                SingleAllOff();
                SingleOff();
            }

            if (ThisLampType == LampType.Rcmm)
                f_B_Turn(0);
            else if (ThisLampType == LampType.Rcml || ThisLampType == LampType.Rcmr)
                f_A_Turn(0);
        }

        private void f_A_Turn(byte value)
        {
            for (var i = 35; i < 128; i++)
                _pixelCoordA[i] = value;
            f_A_Ctrl();
        }

        private void f_B_Turn(byte value)
        {
            for (var i = 0; i < 1795; i++)
            {
                if ((i == 95) || (i == 223) || (i == 226) || (i == 364) || (i == 367)
                    || (i == 507) || (i == 510) || (i == 652) || (i == 676) || (i == 820)
                    || (i == 843) || (i == 989) || (i == 990) || (i == 1138) || (i == 1139)
                    || (i == 1289) || (i == 1290) || (i == 1442) || (i == 1443) || (i == 1597))
                    _pixelCoordB[i] = value;
                else if ((i >= 1598) && (i <= 1618))
                    _pixelCoordB[i] = value;
                else if ((i >= 1774) && (i <= 1794))
                    _pixelCoordB[i] = value;
            }
            f_B_Ctrl();
        }

        private void f_B_Turn(byte value, bool isLeft)
        {
            for (var i = 0; i < 1795; i++)
            {
                if ((i == 95) || (i == 223) || (i == 226) || (i == 364) || (i == 367)
                    || (i == 507) || (i == 510) || (i == 652) || (i == 676) || (i == 820)
                    || (i == 843) || (i == 989) || (i == 990) || (i == 1138) || (i == 1139)
                    || (i == 1289) || (i == 1290) || (i == 1442) || (i == 1443) || (i == 1597))
                {
                    if (isLeft)
                    {
                        if (i == 95 || i == 226 || i == 367 || i == 510 || i == 676 || i == 843 || i == 990 || i == 1139 || i == 1290 || i == 1443)
                        {
                            _pixelCoordB[i] = value;
                        }
                        else
                        {
                            _pixelCoordB[i] = 0x00;
                        }
                    }
                    else
                    {
                        if (i == 95 || i == 226 || i == 367 || i == 510 || i == 676 || i == 843 || i == 990 || i == 1139 || i == 1290 || i == 1443)
                        {
                            _pixelCoordB[i] = 0x00;
                        }
                        else
                        {
                            _pixelCoordB[i] = value;
                        }
                    }
                }
                else if ((i >= 1598) && (i <= 1618))
                {
                    if (isLeft)
                    {
                        _pixelCoordB[i] = value;
                    }
                    else
                    {
                        _pixelCoordB[i] = 0x00;
                    }

                }
                else if ((i >= 1774) && (i <= 1794))
                {
                    if (isLeft)
                    {
                        _pixelCoordB[i] = 0x00;
                    }
                    else
                    {
                        _pixelCoordB[i] = value;
                    }
                }
            }
            f_B_Ctrl();
        }

        [Description("制动灯亮")]
        public void StopOn(string gray)
        {
            if (_isSingle || _isSingleAll)
            {
                SingleAllOff();
                SingleOff();
            }

            if (ThisLampType == LampType.Rcmm)
            {
                byte value;
                if (byte.TryParse(gray, out value))
                    f_B_Position_BREAK(value);
            }
            else if (ThisLampType == LampType.Rcml || ThisLampType == LampType.Rcmr)
            {
                byte value;
                if (byte.TryParse(gray, out value))
                    f_A_Position_Break(value);
            }
        }

        [Description("左制动灯亮-仅B灯")]
        public void StopLeftOn(string gray)
        {
            if (_isSingle || _isSingleAll)
            {
                SingleAllOff();
                SingleOff();
            }

            if (ThisLampType == LampType.Rcmm)
            {
                byte value;
                if (byte.TryParse(gray, out value))
                    f_B_Position_BREAK(value, 1);
            }
        }

        [Description("右制动灯亮-仅B灯")]
        public void StopRightOn(string gray)
        {
            if (_isSingle || _isSingleAll)
            {
                SingleAllOff();
                SingleOff();
            }

            if (ThisLampType == LampType.Rcmm)
            {
                byte value;
                if (byte.TryParse(gray, out value))
                    f_B_Position_BREAK(value, 2);
            }
        }

        [Description("制动灯灭")]
        public void StopOff()
        {
            if (_isSingle || _isSingleAll)
            {
                SingleAllOff();
                SingleOff();
            }

            if (ThisLampType == LampType.Rcmm)
                f_B_Position_BREAK(0);
            else if (ThisLampType == LampType.Rcml || ThisLampType == LampType.Rcmr)
                f_A_Position_Break(0);
        }

        [Description("ISC打开-仅B灯")]
        public void IscOn(string gray)
        {
            if (ThisLampType == LampType.Rcmm)
            {
                if (_isSingle || _isSingleAll)
                {
                    SingleAllOff();
                    SingleOff();
                }

                byte value;
                if (byte.TryParse(gray, out value))
                    f_B_ISC(value);
            }
        }

        [Description("ISC关闭-仅B灯")]
        public void IscOff()
        {
            if (ThisLampType == LampType.Rcmm)
            {
                if (_isSingle || _isSingleAll)
                {
                    SingleAllOff();
                    SingleOff();
                }

                f_B_ISC(0);
            }
        }

        private void f_B_ISC(byte value)
        {
            for (var i = 0; i < 1795; i++)
            {
                if ((i >= 96) && (i <= 222))
                    _pixelCoordB[i] = value;
                else if ((i >= 227) && (i <= 363))
                    _pixelCoordB[i] = value;
                else if ((i >= 368) && (i <= 506))
                    _pixelCoordB[i] = value;
                else if ((i >= 511) && (i <= 651))
                    _pixelCoordB[i] = value;
                else if ((i >= 677) && (i <= 819))
                    _pixelCoordB[i] = value;
                else if ((i >= 844) && (i <= 988))
                    _pixelCoordB[i] = value;
                else if ((i >= 991) && (i <= 1137))
                    _pixelCoordB[i] = value;
                else if ((i >= 1140) && (i <= 1288))
                    _pixelCoordB[i] = value;
                else if ((i >= 1291) && (i <= 1441))
                    _pixelCoordB[i] = value;
                else if ((i >= 1444) && (i <= 1596))
                    _pixelCoordB[i] = value;
                else if ((i >= 1619) && (i <= 1773))
                    _pixelCoordB[i] = value;
            }
            f_B_Ctrl();
        }

        private byte[] _pixelCoordA = new byte[128];
        private byte[] _pixelCoordB = new byte[2048];

        private void f_A_Ctrl()
        {
            for (var i = 0; i < 64; i++)
            {
                for (var j = 0; j <= 1; j++)
                {
                    var strL = string.Format("ISC_Ctrl_Left_LED_A_{0}", j.ToString().PadLeft(2, '0'));
                    VectorEmulator.SetMessageVariableByteValue(strL, i, _pixelCoordA[i + j * 64]);

                    var strR = string.Format("ISC_Ctrl_Right_LED_A_{0}", j.ToString().PadLeft(2, '0'));
                    VectorEmulator.SetMessageVariableByteValue(strR, i, _pixelCoordA[i + j * 64]);
                }
            }
        }

        private void f_B_Ctrl()
        {
            for (var i = 0; i < 64; i++)
            {
                for (var j = 0; j <= 28; j++)
                {
                    var strB = string.Format("ISC_Ctrl_LED_B_{0}", j.ToString().PadLeft(2, '0'));
                    VectorEmulator.SetMessageVariableByteValue(strB, i, _pixelCoordB[i + 64 * j]);
                }
            }
        }

        #endregion

        #region 状态信息

        [Description("R,RCMM-LedDriver工作状态")]
        public string RcmmLedDriverWorkSts;
        [Description("R,RCMM-转向灯信号状态")]
        public string RcmmTurnSts;
        [Description("R,RCMM-刹车灯信号状态")]
        public string RcmmBreakSts;
        [Description("R,RCMM-LedDriver内部故障")]
        public string RcmmLedDriverInternalFault;

        [Description("R,RCML-LedDriver工作状态")]
        public string RcmlLedDriverWorkSts;
        [Description("R,RCML-转向灯信号状态")]
        public string RcmlTurnSts;
        [Description("R,RCML-刹车灯信号状态")]
        public string RcmlBreakSts;
        [Description("R,RCML-LedDriver内部故障")]
        public string RcmlLedDriverInternalFault;

        [Description("R,RCMR-LedDriver工作状态")]
        public string RcmrLedDriverWorkSts;
        [Description("R,RCMR-转向灯信号状态")]
        public string RcmrTurnSts;
        [Description("R,RCMR-刹车灯信号状态")]
        public string RcmrBreakSts;
        [Description("R,RCMR-LedDriver内部故障")]
        public string RcmrLedDriverInternalFault;

        private readonly object _lockError = new object();
        private DateTime _lastErrorDateTimeRcml;
        private DateTime _lastErrorDateTimeRcmr;
        private DateTime _lastErrorDateTimeRcmm;

        private readonly List<uint> _canFdResponseIdList = new List<uint>();

        [Description("R,故障信息读取")]
        public string FaultRead = string.Empty;

        /// <summary>
        /// 读取故障信息
        /// </summary>
        [Description("读取故障信息")]
        public void FaultDetect()
        {
            FaultRead = string.Empty;
            if (CanFd == null)
                return;

            foreach (var t in _canFdResponseIdList)
                CanFd.AddDoNotFilterCanId(t);

            CanFd.CanRecvDataPackages.Clear();
            Thread.Sleep(2000);

            if (CanFd.CanRecvDataPackages.Any())
            {
                var temp = CanFd.CanRecvDataPackages.ToArray();

                try
                {
                    foreach (var find in _canFdResponseIdList.Select(item => temp.ToList().Find(f => f.CanId == item)))
                    {
                        if (find == null)
                        {
                            FaultRead = string.Empty;
                            break;
                        }
                        var datas = new List<byte>();
                        datas.AddRange(find.CanData);
                        FaultRead += ValueHelper.GetHextStr(datas.ToArray());
                        FaultRead += " ";
                    }

                    FaultRead = FaultRead.TrimEnd(' ');
                }
                catch (Exception)
                {
                    FaultRead = string.Empty;
                }
            }

            foreach (var t in _canFdResponseIdList)
                CanFd.RemoveDoNotFilterCanId(t);
        }

        #endregion

        #region App启用

        [Description("R,写APP标准位结果")]
        public string WriteFlagResult = string.Empty;

        [Description("写B灯APP标志位0x0101")]
        public void WriteAppFlagRcmm()
        {
            //AppFlag = string.Empty;
            WriteFlagResult = string.Empty;

            // B左1
            if (WrtieAppFlag(0x7A2, 0x7B2))
                WriteFlagResult += "B灯左侧1:OK;";
            else
                WriteFlagResult += "B灯左侧1:NG;";

            // B左2
            if (WrtieAppFlag(0x7A3, 0x7B3))
                WriteFlagResult += "B灯左侧2:OK;";
            else
                WriteFlagResult += "B灯左侧2:NG;";

            // B中间
            if (WrtieAppFlag(0x7A4, 0x7B4))
                WriteFlagResult += "B灯中间:OK;";
            else
                WriteFlagResult += "B灯中间:NG;";
        }

        private bool WrtieAppFlag(uint reqCanId, uint recvCanId)
        {
            if (CanFd == null)
                return false;

            lock (_canSendLocker)
            {
                if (CanFd.CanBusWithUds.TryEnterExtendedSession(reqCanId, recvCanId, CanBus.CanType.Standard, CanBus.CanProtocol.CanFd, 0x00))
                {
                    Thread.Sleep(500);
                    byte[] seedBytes;
                    if (CanFd.CanBusWithUds.TryRequestSeed(reqCanId, recvCanId, CanBus.CanType.Standard, 0x01, out seedBytes, CanBus.CanProtocol.CanFd, 0x00))
                    {
                        Thread.Sleep(500);
                        if (CanFd.CanBusWithUds.TrySendKey(reqCanId, recvCanId, CanBus.CanType.Standard, 0x02, new byte[4], CanBus.CanProtocol.CanFd, 0x00))
                        {
                            Thread.Sleep(500);

                            var b = 0x3CDE685A;
                            var b0 = (byte)(b >> 24);
                            var b1 = (byte)(b >> 16);
                            var b2 = (byte)(b >> 8);
                            var b3 = (byte)(b >> 0);
                            if (CanFd.CanBusWithUds.TryWriteData(reqCanId, recvCanId, CanBus.CanType.Standard, CanBus.CanProtocol.CanFd, 0x01, 0x01, new[] { b0, b1, b2, b3 }, 0x00))
                            {
                                Thread.Sleep(500);
                                byte[] ecuResetEcho;
                                CanFd.CanBusWithUds
                                    .TesterTryRequest(reqCanId, recvCanId, new byte[] { 0x11, 0x01 }, out ecuResetEcho, CanBus.CanType.Standard, CanBus.CanProtocol.CanFd, pendingByte: 0x00);

                                Thread.Sleep(200);
                                return true;
                            }
                        }
                    }
                }
            }

            Thread.Sleep(500);
            byte[] ecuResetEcho1;
            CanFd.CanBusWithUds
                .TesterTryRequest(reqCanId, recvCanId, new byte[] { 0x11, 0x01 }, out ecuResetEcho1, CanBus.CanType.Standard, CanBus.CanProtocol.CanFd, pendingByte: 0x00);
            Thread.Sleep(200);
            return false;
        }

        [Description("R,写项目号标志位结果")]
        public string WriteProjFlagRcmmResult = string.Empty;
        [Description("R,读项目号标志位结果")]
        public string ReadProjFlagRcmmResult = string.Empty;

        [Description("读写B灯项目号标志位0x0103")]
        public void WriteProjFlagRcmm()
        {
            WriteProjFlagRcmmResult = string.Empty;
            ReadProjFlagRcmmResult = string.Empty;

            // B左1
            if (WrtieProjFlag(0x7A2, 0x7B2))
                WriteProjFlagRcmmResult += "B灯左侧1:OK;";
            else
                WriteProjFlagRcmmResult += "B灯左侧1:NG;";

            ReadProjFlagRcmmResult += "B灯左侧1:" + ReadProjFlag(0x7A2, 0x7B2) + ";";

            // B左2
            if (WrtieProjFlag(0x7A3, 0x7B3))
                WriteProjFlagRcmmResult += "B灯左侧2:OK;";
            else
                WriteProjFlagRcmmResult += "B灯左侧2:NG;";

            ReadProjFlagRcmmResult += "B灯左侧2:" + ReadProjFlag(0x7A3, 0x7B3) + ";";

            // B中间
            if (WrtieProjFlag(0x7A4, 0x7B4))
                WriteProjFlagRcmmResult += "B灯中间:OK;";
            else
                WriteProjFlagRcmmResult += "B灯中间:NG;";

            ReadProjFlagRcmmResult += "B灯中间:" + ReadProjFlag(0x7A4, 0x7B4) + ";";
        }

        private bool WrtieProjFlag(uint reqCanId, uint recvCanId)
        {
            if (CanFd == null)
                return false;

            lock (_canSendLocker)
            {
                if (CanFd.CanBusWithUds.TryEnterExtendedSession(reqCanId, recvCanId, CanBus.CanType.Standard, CanBus.CanProtocol.CanFd, 0x00))
                {
                    Thread.Sleep(500);
                    byte[] seedBytes;
                    if (CanFd.CanBusWithUds.TryRequestSeed(reqCanId, recvCanId, CanBus.CanType.Standard, 0x01, out seedBytes, CanBus.CanProtocol.CanFd, 0x00))
                    {
                        Thread.Sleep(500);
                        if (CanFd.CanBusWithUds.TrySendKey(reqCanId, recvCanId, CanBus.CanType.Standard, 0x02, new byte[4], CanBus.CanProtocol.CanFd, 0x00))
                        {
                            Thread.Sleep(500);

                            var b = 0x5331314c;
                            var b0 = (byte)(b >> 24);
                            var b1 = (byte)(b >> 16);
                            var b2 = (byte)(b >> 8);
                            var b3 = (byte)(b >> 0);
                            if (CanFd.CanBusWithUds.TryWriteData(reqCanId, recvCanId, CanBus.CanType.Standard, CanBus.CanProtocol.CanFd, 0x01, 0x03, new[] { b0, b1, b2, b3 }, 0x00))
                            {
                                Thread.Sleep(500);
                                byte[] ecuResetEcho;
                                CanFd.CanBusWithUds
                                    .TesterTryRequest(reqCanId, recvCanId, new byte[] { 0x11, 0x01 }, out ecuResetEcho, CanBus.CanType.Standard, CanBus.CanProtocol.CanFd, pendingByte: 0x00);

                                Thread.Sleep(200);
                                return true;
                            }
                        }
                    }
                }
            }

            Thread.Sleep(500);
            byte[] ecuResetEcho1;
            CanFd.CanBusWithUds
                .TesterTryRequest(reqCanId, recvCanId, new byte[] { 0x11, 0x01 }, out ecuResetEcho1, CanBus.CanType.Standard, CanBus.CanProtocol.CanFd, pendingByte: 0x00);
            Thread.Sleep(200);
            return false;
        }

        private string ReadProjFlag(uint reqCanId, uint recvCanId)
        {
            var str = string.Empty;
            const byte didiHi = (byte)0x01;
            const byte didLo = (byte)0x03;

            lock (_canSendLocker)
            {
                byte[] echo;
                if (CanFd.CanBusWithUds.TryReadData(reqCanId, recvCanId, CanBus.CanType.Standard,
                    CanBus.CanProtocol.CanFd, didiHi, didLo, out echo, 0x00))
                    str = ValueHelper.GetHextStr(echo).Replace(" ", "");
                else
                {
                    Thread.Sleep(150);
                    if (CanFd.CanBusWithUds.TryReadData(reqCanId, recvCanId, CanBus.CanType.Standard,
                        CanBus.CanProtocol.CanFd, didiHi, didLo, out echo, 0x00))
                        str = ValueHelper.GetHextStr(echo).Replace(" ", "");
                }
            }

            return str;
        }

        [Description("R,写电流信息结果")]
        public string WriteCurrRcmmResult = string.Empty;
        [Description("R,读电流信息结果")]
        public string ReadCurrRcmmResult = string.Empty;

        [Description("读写B灯电流信息0x0104")]
        public void WriteCurrInfo()
        {
            WriteCurrRcmmResult = string.Empty;
            ReadCurrRcmmResult = string.Empty;

            // B左1
            if (WrtieCurr(0x7A2, 0x7B2))
                WriteCurrRcmmResult += "B灯左侧1:OK;";
            else
                WriteCurrRcmmResult += "B灯左侧1:NG;";

            ReadCurrRcmmResult += "B灯左侧1:" + ReadCurr(0x7A2, 0x7B2) + ";";

            // B左2
            if (WrtieCurr(0x7A3, 0x7B3))
                WriteCurrRcmmResult += "B灯左侧2:OK;";
            else
                WriteCurrRcmmResult += "B灯左侧2:NG;";

            ReadCurrRcmmResult += "B灯左侧2:" + ReadCurr(0x7A3, 0x7B3) + ";";

            // B中间
            if (WrtieCurr(0x7A4, 0x7B4))
                WriteCurrRcmmResult += "B灯中间:OK;";
            else
                WriteCurrRcmmResult += "B灯中间:NG;";

            ReadCurrRcmmResult += "B灯中间:" + ReadCurr(0x7A4, 0x7B4) + ";";


        }

        private bool WrtieCurr(uint reqCanId, uint recvCanId)
        {
            if (CanFd == null)
                return false;

            lock (_canSendLocker)
            {
                if (CanFd.CanBusWithUds.TryEnterExtendedSession(reqCanId, recvCanId, CanBus.CanType.Standard, CanBus.CanProtocol.CanFd, 0x00))
                {
                    Thread.Sleep(500);
                    byte[] seedBytes;
                    if (CanFd.CanBusWithUds.TryRequestSeed(reqCanId, recvCanId, CanBus.CanType.Standard, 0x01, out seedBytes, CanBus.CanProtocol.CanFd, 0x00))
                    {
                        Thread.Sleep(500);
                        if (CanFd.CanBusWithUds.TrySendKey(reqCanId, recvCanId, CanBus.CanType.Standard, 0x02, new byte[4], CanBus.CanProtocol.CanFd, 0x00))
                        {
                            Thread.Sleep(500);

                            var b = (byte)0x04;
                            //var b0 = (byte)(b >> 24);
                            //var b1 = (byte)(b >> 16);
                            //var b2 = (byte)(b >> 8);
                            //var b3 = (byte)(b >> 0);
                            if (CanFd.CanBusWithUds.TryWriteData(reqCanId, recvCanId, CanBus.CanType.Standard, CanBus.CanProtocol.CanFd, 0x01, 0x04, new[] { b }, 0x00))
                            {
                                Thread.Sleep(500);
                                byte[] ecuResetEcho;
                                CanFd.CanBusWithUds
                                    .TesterTryRequest(reqCanId, recvCanId, new byte[] { 0x11, 0x01 }, out ecuResetEcho, CanBus.CanType.Standard, CanBus.CanProtocol.CanFd, pendingByte: 0x00);

                                Thread.Sleep(200);
                                return true;
                            }
                        }
                    }
                }
            }

            Thread.Sleep(500);
            byte[] ecuResetEcho1;
            CanFd.CanBusWithUds
                .TesterTryRequest(reqCanId, recvCanId, new byte[] { 0x11, 0x01 }, out ecuResetEcho1, CanBus.CanType.Standard, CanBus.CanProtocol.CanFd, pendingByte: 0x00);
            Thread.Sleep(200);
            return false;
        }

        private string ReadCurr(uint reqCanId, uint recvCanId)
        {
            var str = string.Empty;
            const byte didiHi = (byte)0x01;
            const byte didLo = (byte)0x04;

            lock (_canSendLocker)
            {
                byte[] echo;
                if (CanFd.CanBusWithUds.TryReadData(reqCanId, recvCanId, CanBus.CanType.Standard,
                    CanBus.CanProtocol.CanFd, didiHi, didLo, out echo, 0x00))
                    str = ValueHelper.GetHextStr(echo).Replace(" ", "");
                else
                {
                    Thread.Sleep(150);
                    if (CanFd.CanBusWithUds.TryReadData(reqCanId, recvCanId, CanBus.CanType.Standard,
                        CanBus.CanProtocol.CanFd, didiHi, didLo, out echo, 0x00))
                        str = ValueHelper.GetHextStr(echo).Replace(" ", "");
                }
            }

            return str;
        }

        #endregion

        #region Code from CANoe and DBC

        private const string IscCtrl = "ISC_Ctrl";
        private readonly Dictionary<string, uint> _iscCtrlLedB =
            new Dictionary<string, uint>();
        private readonly Dictionary<string, uint> _iscCtrlLeftLedA =
            new Dictionary<string, uint>();
        private readonly Dictionary<string, uint> _iscCtrlRightLedA =
            new Dictionary<string, uint>();

        private void SysConfig(string dbcFilePath)
        {
            VectorEmulator =
                new VectorDbcEmulator(new[] { dbcFilePath });
            InitVariables();
            Prestart();
            Start();
        }

        private void InitVariables()
        {
            var baseIndexB = 0;
            for (var i = 0x400; i <= 0x41C; i++)
            {
                var str = string.Format("ISC_Ctrl_LED_B_{0}", baseIndexB.ToString().PadLeft(2, '0'));
                var canId = (uint)i;

                _iscCtrlLedB.Add(str, canId);
                baseIndexB++;
                VectorEmulator.InitMessageVariable(str, str);
            }

            var baseIndexLa = 0;
            for (var i = 0x300; i <= 0x301; i++)
            {
                var str = string.Format("ISC_Ctrl_Left_LED_A_{0}", baseIndexLa.ToString().PadLeft(2, '0'));
                var canId = (uint)i;

                _iscCtrlLeftLedA.Add(str, canId);
                baseIndexLa++;
                VectorEmulator.InitMessageVariable(str, str);
            }

            var baseIndexRa = 0;
            for (var i = 0x500; i <= 0x501; i++)
            {
                var str = string.Format("ISC_Ctrl_Right_LED_A_{0}", baseIndexRa.ToString().PadLeft(2, '0'));
                var canId = (uint)i;

                _iscCtrlRightLedA.Add(str, canId);
                baseIndexRa++;
                VectorEmulator.InitMessageVariable(str, str);
            }

            VectorEmulator.InitMessageVariable(IscCtrl, IscCtrl);

            VectorEmulator.InitMessageVariable("LedDriver_FeedBack_Rear_B", "LedDriver_FeedBack_Rear_B");
            VectorEmulator.InitMessageVariable("LedDriver_FeedBack_Rear_Left_A", "LedDriver_FeedBack_Rear_Left_A");
            VectorEmulator.InitMessageVariable("LedDriver_FeedBack_Rear_Right_A", "LedDriver_FeedBack_Rear_Right_A");

            VectorEmulator.InitMessageVariable("Rear_FaultLed_Feedback_B_01", "Rear_FaultLed_Feedback_B_01");
            VectorEmulator.InitMessageVariable("Rear_FaultLed_Feedback_B_02", "Rear_FaultLed_Feedback_B_02");
            VectorEmulator.InitMessageVariable("Rear_FaultLed_Feedback_B_03", "Rear_FaultLed_Feedback_B_03");
            VectorEmulator.InitMessageVariable("Rear_FaultLed_Feedback_B_04", "Rear_FaultLed_Feedback_B_04");
            VectorEmulator.InitMessageVariable("Rear_FaultLed_Feedback_B_05", "Rear_FaultLed_Feedback_B_05");
            VectorEmulator.InitMessageVariable("Rear_FaultLed_Feedback_Left_A", "Rear_FaultLed_Feedback_Left_A");
            VectorEmulator.InitMessageVariable("Rear_FaultLed_Feedback_Right_A", "Rear_FaultLed_Feedback_Right_A");
        }

        private void Prestart()
        {

        }

        private void Start()
        {
            VectorEmulator.SetTimer(Tmr_Refresh50Ms(), 50);
            VectorEmulator.SetTimer(Tmr_Refresh100Ms(), 100);
        }

        private Action Tmr_Refresh100Ms()
        {
            return () =>
            {
                lock (_lockError)
                {
                    var endTime = DateTime.Now;
                    if (ValueHelper.GetTimeSpanMs(_lastErrorDateTimeRcmm, endTime) >= 2500)
                    {
                        RcmmLedDriverWorkSts = string.Empty;
                        RcmmTurnSts = string.Empty;
                        RcmmBreakSts = string.Empty;
                        RcmmLedDriverInternalFault = string.Empty;
                    }

                    if (ValueHelper.GetTimeSpanMs(_lastErrorDateTimeRcml, endTime) >= 2500)
                    {
                        RcmlLedDriverWorkSts = string.Empty;
                        RcmlTurnSts = string.Empty;
                        RcmlBreakSts = string.Empty;
                        RcmlLedDriverInternalFault = string.Empty;
                    }

                    if (ValueHelper.GetTimeSpanMs(_lastErrorDateTimeRcmr, endTime) >= 2500)
                    {
                        RcmrLedDriverWorkSts = string.Empty;
                        RcmrTurnSts = string.Empty;
                        RcmrBreakSts = string.Empty;
                        RcmrLedDriverInternalFault = string.Empty;
                    }
                }
            };
        }

        private Action Tmr_Refresh50Ms()
        {
            return () =>
            {
                if (ThisLampType == LampType.Null || !IsCanStarted)
                    return;

                lock (_canSendLocker)
                {
                    var msgStrs = new List<string> { IscCtrl };

                    //VectorEmulator.OutPut(msgStrs.ToArray(), CanFd, canProtocol: CanBus.CanProtocol.CanFd);

                    if (_isSingle || _isSingleAll)
                    {
                        if (ThisLampType == LampType.Rcmm)
                        {
                            if (_isSingle)
                            {
                                if (_ledIndex < 1795)
                                {
                                    _pixelCoordB = new byte[2048];
                                    _pixelCoordB[_ledIndex++] = 0xFF;
                                    f_B_Ctrl();
                                }
                                else
                                {
                                    _ledIndex = 0;
                                    f_B_All(0);
                                }
                            }
                            else if (_isSingleAll)
                            {
                                if (_ledIndex < 1795)
                                {
                                    _pixelCoordB[_ledIndex++] = 100;
                                    f_B_Ctrl();
                                }
                                else
                                {
                                    _ledIndex = 0;
                                    f_B_All(0);
                                }
                            }
                        }
                        else if (ThisLampType == LampType.Rcml || ThisLampType == LampType.Rcmr)
                        {
                            if (_isSingle)
                            {
                                if (_ledIndex < 68)
                                {
                                    _pixelCoordA = new byte[128];
                                    _pixelCoordA[_ledIndex++] = 0xFF;
                                    f_A_Ctrl();
                                }
                                else
                                {
                                    _ledIndex = 0;
                                    f_A_All(0);
                                }
                            }
                            else if (_isSingleAll)
                            {
                                if (_ledIndex < 68)
                                {
                                    _pixelCoordA[_ledIndex++] = 0xFF;
                                    f_A_Ctrl();
                                }
                                else
                                {
                                    _ledIndex = 0;
                                    f_A_All(0);
                                }
                            }
                        }
                    }

                    //msgStrs.Clear();
                    if (ThisLampType == LampType.Rcmm)
                        msgStrs.AddRange(_iscCtrlLedB.Keys);
                    else if (ThisLampType == LampType.Rcml)
                        msgStrs.AddRange(_iscCtrlLeftLedA.Keys);
                    else if (ThisLampType == LampType.Rcmr)
                        msgStrs.AddRange(_iscCtrlRightLedA.Keys);

                    //msgStrs.Clear();
                    //msgStrs.AddRange(_iscCtrlLedB.Keys);
                    //msgStrs.AddRange(_iscCtrlLeftLedA.Keys);
                    //msgStrs.AddRange(_iscCtrlRightLedA.Keys);

                    VectorEmulator.OutPut(msgStrs.ToArray(), CanFd, canProtocol: CanBus.CanProtocol.CanFd);
                }
            };
        }

        private void CanBus_PushCanMsg(
            string name, CanBus.CanDataPackage data, CanBus.OnPushCanDataType onPushCanDataType)
        {
            if (CanFd != null && CanFd.Name == name)
            {
                if (data.CanId == 0x6A0) // B
                {
                    lock (_lockError)
                    {
                        RcmmLedDriverWorkSts =
                            VectorEmulator.GetMessageVariableSignalValue(data.CanId, "Led_Driver_WorkSts").ToString();
                        RcmmTurnSts =
                            VectorEmulator.GetMessageVariableSignalValue(data.CanId, "Turn_Sts").ToString();
                        RcmmBreakSts =
                            VectorEmulator.GetMessageVariableSignalValue(data.CanId, "Break_Sts").ToString();
                        RcmmLedDriverInternalFault =
                            VectorEmulator.GetMessageVariableSignalValue(data.CanId, "Led_Driver_Internal_Fault").ToString();

                        _lastErrorDateTimeRcmm = DateTime.Now;
                    }
                }
                else if (data.CanId == 0x690) // A左
                {
                    lock (_lockError)
                    {
                        RcmlLedDriverWorkSts =
                             VectorEmulator.GetMessageVariableSignalValue(data.CanId, "Led_Driver_WorkSts").ToString();
                        RcmlTurnSts =
                            VectorEmulator.GetMessageVariableSignalValue(data.CanId, "Turn_Sts").ToString();
                        RcmlBreakSts =
                            VectorEmulator.GetMessageVariableSignalValue(data.CanId, "Break_Sts").ToString();
                        RcmlLedDriverInternalFault =
                            VectorEmulator.GetMessageVariableSignalValue(data.CanId, "Led_Driver_Internal_Fault").ToString();

                        _lastErrorDateTimeRcml = DateTime.Now;
                    }
                }
                else if (data.CanId == 0x6B0) // A右
                {
                    lock (_lockError)
                    {
                        RcmrLedDriverWorkSts =
                             VectorEmulator.GetMessageVariableSignalValue(data.CanId, "Led_Driver_WorkSts").ToString();
                        RcmrTurnSts =
                            VectorEmulator.GetMessageVariableSignalValue(data.CanId, "Turn_Sts").ToString();
                        RcmrBreakSts =
                            VectorEmulator.GetMessageVariableSignalValue(data.CanId, "Break_Sts").ToString();
                        RcmrLedDriverInternalFault =
                            VectorEmulator.GetMessageVariableSignalValue(data.CanId, "Led_Driver_Internal_Fault").ToString();

                        _lastErrorDateTimeRcmr = DateTime.Now;
                    }
                }

                //VectorEmulator.GetMessageVariableSignalValue(data.CanId, "Led_Driver_WorkSts");
            }
        }

        internal enum LampType
        {
            /// <summary>
            /// 默认未选择
            /// </summary>
            Null,

            /// <summary>
            /// B灯
            /// </summary>
            Rcmm,

            /// <summary>
            /// A灯左
            /// </summary>
            Rcml,

            /// <summary>
            /// A灯右
            /// </summary>
            Rcmr
        }

        #endregion
    }
}
