using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using CommonUtility;
using CommonUtility.BusLoader;

namespace Controller
{
    [Description("CAN-Product,HA1-ISD-前灯")]
    public sealed class Ha1IsdFrontLamp : ControllerBase
    {
        public CanBus Can;

        [Description("R/W,点亮灰度值0~255")]
        public string Gray = "255";

        [Description("R/W,单双切换基数")]
        public string EvenOddSwitch = "3";

        public Ha1IsdFrontLamp(string name)
            : base(name)
        {
            //_lampType = LampType.Fla;

            if (MainWorkThread != null)
            {
                MainWorkThread.Abort();
                MainWorkThread.Join();
            }

            MainWorkThread =
                new Thread(MainWork) { IsBackground = true };
            MainWorkThread.Start();
        }

        ~Ha1IsdFrontLamp()
        {
            Dispose();
        }

        #region 点灯控制
        [Description("唤醒")]
        public void LampAwake()
        {
            IsDrlOn = false;
            IsPlOn = false;
            IsTurnOn = false;

            SpecialContolType = SpecialContol.AllOff;

            SpecialControl = false;

            _isSleep = false;
        }

        [Description("休眠")]
        public void LampSleep()
        {
            IsDrlOn = false;
            IsPlOn = false;
            IsTurnOn = false;

            SpecialContolType = SpecialContol.AllOff;
            SpecialControl = false;

            SpecialControl = false;

            _isSleep = true;
        }

        [Description("转向灯开")]
        public void TurnOn()
        {
            SpecialContolType = SpecialContol.AllOff;
            SpecialControl = false;
            IsTurnOn = true;
        }

        [Description("转向灯关")]
        public void TurnOff()
        {
            SpecialContolType = SpecialContol.AllOff;
            SpecialControl = false;
            IsTurnOn = false;
        }

        [Description("日行灯开")]
        public void DrlOn()
        {
            SpecialContolType = SpecialContol.AllOff;
            SpecialControl = false;
            IsDrlOn = true;
        }

        [Description("日行灯关")]
        public void DrlOff()
        {
            SpecialContolType = SpecialContol.AllOff;
            SpecialControl = false;
            IsDrlOn = false;
        }

        [Description("位置灯开")]
        public void PlOn()
        {
            SpecialContolType = SpecialContol.AllOff;
            SpecialControl = false;
            IsPlOn = true;
        }

        [Description("位置灯关")]
        public void PlOff()
        {
            SpecialContolType = SpecialContol.AllOff;
            SpecialControl = false;
            IsPlOn = false;
        }

        [Description("所有LED全亮")]
        public void AllLedOn()
        {
            IsDrlOn = false;
            IsPlOn = false;
            IsTurnOn = false;

            SpecialContolType = SpecialContol.AllOn;
            SpecialControl = true;
        }

        [Description("所有LED全关")]
        public void AllLedOff()
        {
            IsDrlOn = false;
            IsPlOn = false;
            IsTurnOn = false;

            SpecialContolType = SpecialContol.AllOff;
            SpecialControl = true;
        }

        [Description("所有白光LED全亮")]
        public void AllWhiteOn()
        {
            IsDrlOn = false;
            IsPlOn = false;
            IsTurnOn = false;

            SpecialContolType = SpecialContol.AllWhiteOn;
            SpecialControl = true;
        }

        [Description("所有白光LED偶数点亮")]
        public void AllWhiteEvenOn()
        {
            IsDrlOn = false;
            IsPlOn = false;
            IsTurnOn = false;

            SpecialContolType = SpecialContol.AllWhiteEvenOn;
            SpecialControl = true;
        }

        [Description("所有白光LED奇数点亮")]
        public void AllWhiteOddOn()
        {
            IsDrlOn = false;
            IsPlOn = false;
            IsTurnOn = false;

            SpecialContolType = SpecialContol.AllWhiteOddOn;
            SpecialControl = true;
        }

        [Description("所有黄光LED全亮")]
        public void AllYellowOn()
        {
            IsDrlOn = false;
            IsPlOn = false;
            IsTurnOn = false;

            SpecialContolType = SpecialContol.AllYellowOn;
            SpecialControl = true;
        }

        [Description("所有黄光LED偶数点亮")]
        public void AllYellowEvenOn()
        {
            IsDrlOn = false;
            IsPlOn = false;
            IsTurnOn = false;

            SpecialContolType = SpecialContol.AllYellowEvenOn;
            SpecialControl = true;
        }

        [Description("所有黄光LED奇数点亮")]
        public void AllYellowOddOn()
        {
            IsDrlOn = false;
            IsPlOn = false;
            IsTurnOn = false;

            SpecialContolType = SpecialContol.AllYellowOddOn;
            SpecialControl = true;
        }

        #endregion

        #region 内部方法
        private Thread MainWorkThread { get; set; }
        private int SendCount { get; set; }
        private int SendGroupIndex { get; set; }
        private bool _isSleep = true;
        private LampType _lampType;

        private readonly CanBus.CanDataPackage _controlDataPackage =
            new CanBus.CanDataPackage(
                0xFF, CanBus.CanProtocol.Can,
                CanBus.CanType.Standard,
                CanBus.CanFormat.Data,
                new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });

        private bool IsUp { get; set; }
        private bool IsTurnOn { get; set; }
        private bool IsDrlOn { get; set; }
        private bool IsPlOn { get; set; }
        private bool SpecialControl { get; set; }
        private SpecialContol SpecialContolType { get; set; }

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
                    if (Can == null || _isSleep)
                        continue;

                    var sendPackages = new List<CanBus.CanDataPackage>
                    {
                        _controlDataPackage
                    };

                    int baseId;

                    var turnStr = string.Empty;
                    string drlStr;
                    var plStr = string.Empty;

                    var drlLedList = new List<int>();
                    var plLedList = new List<int>();
                    var turnLedList = new List<int>();

                    var ledDataPackages =
                        new List<CanBus.CanDataPackage>();

                    switch (_lampType)
                    {
                        case LampType.Fra:
                            baseId = 0x100;
                            turnStr =
                                "7,9,17,19,28," +
                                "30,39,41,51,53," +
                                "55,57,59,61,63," +
                                "65,67,69,71,73," +
                                "75,77,79,81,83," +
                                "85,87,89,91,93," +
                                "95,97,99,101,103," +
                                "105,107,109,111,113," +
                                "115";
                            drlStr = "8,10,18,20,29," +
                                    "31,40,42,52,54," +
                                    "56,58,60,62,64," +
                                    "66,68,70,72,74," +
                                    "76,78,80,82,84," +
                                    "86,88,90,92,94," +
                                    "96,98,100,102,104," +
                                    "106,108,110,112,114," +
                                    "116";
                            plStr =
                                "1,2,3,4,5,6,11,12,13,14,15,16,21,22,23,24,25,26,27,32,33,34,35,36,37,38,43,44,45,46,47,48,49,50";

                            break;

                        case LampType.Frb:
                            baseId = 0x140;
                            drlStr =
                                "1,2,3,4,5,6,7," +
                                "8,9,10,11,12,13,14," +
                                "15,16,17,18,19,20,21," +
                                "22,23,24,25,26,27,28," +
                                "29,30,31,32,33,34,35," +
                                "36,37,38,39,40,41,42," +
                                "43,44,45,46,47,48,49";
                            break;

                        case LampType.Fc:
                            baseId = 0x1C0;
                            drlStr =
                                "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,71,72,73,74,75,76,77,78,79,80,81,82,83,84,85,86,87,88,89,90,91,92,93,94,95,96,97,98,99,100,101,102,103,104,105,106,107,108,109,110,111,112,113,114,115,116,117,118,119,120,121,122,123,124,125,126,127,128,129,130,131,132,133,134,135,136,137,138,139,140,141,142,143,144";
                            break;

                        case LampType.Fla:
                            baseId = 0x200;
                            turnStr =
                                "1,9,11,19,21," +
                                "30,32,41,43,53," +
                                "55,57,59,61,63," +
                                "65,67,69,71,73," +
                                "75,77,79,81,83," +
                                "85,87,89,91,93," +
                                "95,97,99,101,103," +
                                "105,107,109,111,113," +
                                "115";
                            drlStr =
                                "2,10,12,20,22," +
                                "31,33,42,44,54," +
                                "56,58,60,62,64," +
                                "66,68,70,72,74," +
                                "76,78,80,82,84," +
                                "86,88,90,92,94," +
                                "96,98,100,102,104," +
                                "106,108,110,112,114," +
                                "116";
                            plStr =
                                "3,4,5,6,7,8,13,14,15,16,17,18,23,24,25,26,27,28,29,34,35,36,37,38,39,40,45,46,47,48,49,50,51,52";
                            break;

                        case LampType.Flb:
                            baseId = 0x240;
                            drlStr =
                                "1,2,3,4,5,6,7," +
                                "8,9,10,11,12,13,14," +
                                "15,16,17,18,19,20,21," +
                                "22,23,24,25,26,27,28," +
                                "29,30,31,32,33,34,35," +
                                "36,37,38,39,40,41,42," +
                                "43,44,45,46,47,48,49";
                            break;

                        case LampType.FrbUp:
                            baseId = 0x180;
                            drlStr =
                                "2,4,6,8,10,12,14,16,18,20,22,24,26,28,30,32,34,36,38,40,42,44,46,48,50,52,54,56,58,60,62,64,66,68,70,72,74,76,78,80,82,84,86,88,90,92,94,96,98,100,102,104,106,108,110,112,114,116,118,120,122,124,126,128,130,132,134,136,138,140,142,144,146,148,150,152,154,156,158,160,162,164,166,168,170,172,174,176,178,180,182,184,186,188,190,192,194,196,198,200,202,204,206,208,210,212,214,216,218,220,222,224,226,228,230,232,234,236,238,240,242,244,246,248,250,252,254,256,258,260,262,264,266,268,270,272,274,276,278,280,282,284,286,288,290,292,294,296,298,300,302,304,306,308,310,312,314,316,318,320,322,324,326,328,330,332,334,336,338,340,342,344,346,348,350,352,354,356,358,360,362,364,366,368,370,372,374,376,378,380,382,384,386,388,390,392,394,396,398,400,402,404,406,408,410";
                            turnStr =
                                "1,3,5,7,9,11,13,15,17,19,21,23,25,27,29,31,33,35,37,39,41,43,45,47,49,51,53,55,57,59,61,63,65,67,69,71,73,75,77,79,81,83,85,87,89,91,93,95,97,99,101,103,105,107,109,111,113,115,117,119,121,123,125,127,129,131,133,135,137,139,141,143,145,147,149,151,153,155,157,159,161,163,165,167,169,171,173,175,177,179,181,183,185,187,189,191,193,195,197,199,201,203,205,207,209,211,213,215,217,219,221,223,225,227,229,231,233,235,237,239,241,243,245,247,249,251,253,255,257,259,261,263,265,267,269,271,273,275,277,279,281,283,285,287,289,291,293,295,297,299,301,303,305,307,309,311,313,315,317,319,321,323,325,327,329,331,333,335,337,339,341,343,345,347,349,351,353,355,357,359,361,363,365,367,369,371,373,375,377,379,381,383,385,387,389,391,393,395,397,399,401,403,405,407,409";
                            break;

                        case LampType.FlbUp:
                            baseId = 0x280;
                            drlStr =
                                "2,4,6,8,10,12,14,16,18,20,22,24,26,28,30,32,34,36,38,40,42,44,46,48,50,52,54,56,58,60,62,64,66,68,70,72,74,76,78,80,82,84,86,88,90,92,94,96,98,100,102,104,106,108,110,112,114,116,118,120,122,124,126,128,130,132,134,136,138,140,142,144,146,148,150,152,154,156,158,160,162,164,166,168,170,172,174,176,178,180,182,184,186,188,190,192,194,196,198,200,202,204,206,208,210,212,214,216,218,220,222,224,226,228,230,232,234,236,238,240,242,244,246,248,250,252,254,256,258,260,262,264,266,268,270,272,274,276,278,280,282,284,286,288,290,292,294,296,298,300,302,304,306,308,310,312,314,316,318,320,322,324,326,328,330,332,334,336,338,340,342,344,346,348,350,352,354,356,358,360,362,364,366,368,370,372,374,376,378,380,382,384,386,388,390,392,394,396,398,400,402,404,406,408,410";
                            turnStr =
                                "1,3,5,7,9,11,13,15,17,19,21,23,25,27,29,31,33,35,37,39,41,43,45,47,49,51,53,55,57,59,61,63,65,67,69,71,73,75,77,79,81,83,85,87,89,91,93,95,97,99,101,103,105,107,109,111,113,115,117,119,121,123,125,127,129,131,133,135,137,139,141,143,145,147,149,151,153,155,157,159,161,163,165,167,169,171,173,175,177,179,181,183,185,187,189,191,193,195,197,199,201,203,205,207,209,211,213,215,217,219,221,223,225,227,229,231,233,235,237,239,241,243,245,247,249,251,253,255,257,259,261,263,265,267,269,271,273,275,277,279,281,283,285,287,289,291,293,295,297,299,301,303,305,307,309,311,313,315,317,319,321,323,325,327,329,331,333,335,337,339,341,343,345,347,349,351,353,355,357,359,361,363,365,367,369,371,373,375,377,379,381,383,385,387,389,391,393,395,397,399,401,403,405,407,409";
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    GetLedList(drlLedList, drlStr);
                    GetLedList(plLedList, plStr);
                    GetLedList(turnLedList, turnStr);
                    var max = FindMax(drlLedList, plLedList, turnLedList);

                    if (IsUp)
                    {
                        for (var i = 0; i <= 255; i++)
                        {
                            var groudIndex = max / 64;

                            if (i <= groudIndex)
                            {
                                ledDataPackages.Add(
                                    new CanBus.CanDataPackage(
                                        (uint)(baseId + i),
                                        CanBus.CanProtocol.CanFd,
                                        CanBus.CanType.Standard,
                                        CanBus.CanFormat.Data,
                                        new byte[64]));
                            }
                        }
                    }
                    else
                    {
                        for (var i = 0; i <= 255; i++)
                        {
                            var groudIndex = max / 8;

                            if (i <= groudIndex)
                            {
                                ledDataPackages.Add(
                                    new CanBus.CanDataPackage(
                                        (uint)(baseId + i),
                                        CanBus.CanProtocol.Can,
                                        CanBus.CanType.Standard,
                                        CanBus.CanFormat.Data,
                                        new byte[8]));
                            }
                        }
                    }

                    if (SpecialControl)
                    {
                        switch (SpecialContolType)
                        {
                            case SpecialContol.AllOn:
                                SetGray(true, turnLedList, ledDataPackages);
                                SetGray(true, drlLedList, ledDataPackages);
                                SetGray(true, plLedList, ledDataPackages);
                                break;

                            case SpecialContol.AllOff:
                                SetGray(false, turnLedList, ledDataPackages);
                                SetGray(false, drlLedList, ledDataPackages);
                                SetGray(false, plLedList, ledDataPackages);
                                break;

                            case SpecialContol.AllWhiteOn:
                                SetGray(false, turnLedList, ledDataPackages);
                                SetGray(true, drlLedList, ledDataPackages);
                                SetGray(true, plLedList, ledDataPackages);
                                break;

                            case SpecialContol.AllWhiteEvenOn:
                                SetGray(false, turnLedList, ledDataPackages);
                                SetGray(true, drlLedList, ledDataPackages, 1);
                                SetGray(true, plLedList, ledDataPackages, 1);
                                break;

                            case SpecialContol.AllWhiteOddOn:
                                SetGray(false, turnLedList, ledDataPackages);
                                SetGray(true, drlLedList, ledDataPackages, 2);
                                SetGray(true, plLedList, ledDataPackages, 2);
                                break;

                            case SpecialContol.AllYellowOn:
                                SetGray(true, turnLedList, ledDataPackages);
                                SetGray(false, drlLedList, ledDataPackages);
                                SetGray(false, plLedList, ledDataPackages);
                                break;

                            case SpecialContol.AllYellowEvenOn:
                                SetGray(true, turnLedList, ledDataPackages, 1);
                                SetGray(false, drlLedList, ledDataPackages);
                                SetGray(false, plLedList, ledDataPackages);
                                break;

                            case SpecialContol.AllYellowOddOn:
                                SetGray(true, turnLedList, ledDataPackages, 2);
                                SetGray(false, drlLedList, ledDataPackages);
                                SetGray(false, plLedList, ledDataPackages);
                                break;

                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    else
                    {
                        SetGray(IsTurnOn, turnLedList, ledDataPackages);
                        SetGray(IsDrlOn, drlLedList, ledDataPackages);
                        SetGray(IsPlOn, plLedList, ledDataPackages);
                    }

                    SendGroupIndex++;
                    if (SendGroupIndex == ledDataPackages.Count)
                        SendGroupIndex = 0;
                    sendPackages.Add(ledDataPackages[SendGroupIndex]);

                    Can.SendCanDatas(sendPackages.ToArray());
                }
                catch (Exception)
                {
                    // ignoWhite
                }
            }
        }

        public void GetLedList(List<int> ledList, string ledStr)
        {
            ledList.Clear();

            foreach (var t in ledStr.Split(','))
            {
                int index;
                if (!int.TryParse(t, out index))
                    continue;
                index = index - 1;
                if (!ledList.Contains(index))
                    ledList.Add(index);
            }

            ledList.Sort();
        }

        private static int FindMax(
            IReadOnlyList<int> a, IReadOnlyCollection<int> b, IReadOnlyCollection<int> c)
        {
            var list = new List<int>
            {
                a.Any() ? a[a.Count - 1] : 0,
                b.Any() ? a[b.Count - 1] : 0,
                c.Any() ? a[c.Count - 1] : 0
            };

            list.Sort();
            return list[2];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isOn"></param>
        /// <param name="ledList"></param>
        /// <param name="canDatas"></param>
        /// <param name="type">0=全亮，1=偶数，2=奇数</param>
        public void SetGray(
            bool isOn, List<int> ledList, List<CanBus.CanDataPackage> canDatas, int type = 0)
        {
            byte gray;
            if (!string.IsNullOrEmpty(Gray) && byte.TryParse(Gray, out gray))
            {

            }
            else
            {
                gray = 0xFF;
            }

            int evenOddSwitch;
            if (!string.IsNullOrEmpty(EvenOddSwitch) && int.TryParse(EvenOddSwitch, out evenOddSwitch))
            {
                if (evenOddSwitch >= 2 && evenOddSwitch <= 5)
                {

                }
                else
                {
                    evenOddSwitch = 3;
                }
            }
            else
            {
                evenOddSwitch = 3;
            }

            for (var i = 0; i < ledList.Count; i++)
            {
                var r = ledList[i];

                var groudIndex = IsUp ? r / 64 : r / 8;
                var ledIndex = IsUp ? r % 64 : r % 8;

                if (canDatas.Count < groudIndex)
                    continue;

                if (isOn)
                {
                    if (type == 1)
                    {
                        if (i % evenOddSwitch == 0)
                        {
                            canDatas[groudIndex].CanData[ledIndex] = gray;
                        }
                        else
                        {
                            canDatas[groudIndex].CanData[ledIndex] = 0x00;
                        }
                    }
                    else if (type == 2)
                    {
                        if (i % evenOddSwitch != 0)
                        {
                            canDatas[groudIndex].CanData[ledIndex] = gray;
                        }
                        else
                        {
                            canDatas[groudIndex].CanData[ledIndex] = 0x00;
                        }
                    }
                    else if (type == 0)
                    {
                        canDatas[groudIndex].CanData[ledIndex] = gray;
                    }
                }
                else
                {
                    canDatas[groudIndex].CanData[ledIndex] = 0x00;
                }
            }
        }

        #endregion

        #region 切换灯种

        [Description("切换成前灯A左")]
        public void ChangeToFrontALeft()
        {
            IsUp = false;
            SendGroupIndex = 0;
            _lampType = LampType.Fla;
        }

        [Description("切换成前灯A右")]
        public void ChangeToFrontARight()
        {
            IsUp = false;
            SendGroupIndex = 0;
            _lampType = LampType.Fra;
        }

        [Description("切换成前灯C")]
        public void ChangeToFrontC()
        {
            IsUp = false;
            SendGroupIndex = 0;
            _lampType = LampType.Fc;
        }

        [Description("切换成前灯B左")]
        public void ChangeToFrontBLeft()
        {
            IsUp = false;
            SendGroupIndex = 0;
            _lampType = LampType.Flb;
        }

        [Description("切换成前灯B右")]
        public void ChangeToFrontBRight()
        {
            IsUp = false;
            SendGroupIndex = 0;
            _lampType = LampType.Frb;
        }

        [Description("切换成高配前灯B左")]
        public void ChangeToFrontBLeftUp()
        {
            IsUp = true;
            SendGroupIndex = 0;
            _lampType = LampType.FlbUp;
        }

        [Description("切换成高配前灯B右")]
        public void ChangeToFrontBRightUp()
        {
            IsUp = true;
            SendGroupIndex = 0;
            _lampType = LampType.FrbUp;
        }

        #endregion

        #region 版本信息

        //ReadAppVersion("775","77D","F194")”

        [Description("R,FBL零件号")]
        public string FblPartNo;

        [Description("R,FBL版本号")]
        public string FblVersion;

        [Description("R,FBL内部版本号")]
        public string InternalFblVersion;

        [Description("R,APP零件号")]
        public string AppPartNo;

        [Description("R,APP版本号")]
        public string AppVersion;

        [Description("R,APP内部版本号")]
        public string InternalAppVersion;

        [Description("读取FBL零件号")]
        public void ReadFblPartNo(string reqCanId, string recvCanId, string did)
        {
            FblPartNo = string.Empty;
            FblPartNo = ValueHelper.GetHextStr(ReadDid(reqCanId, recvCanId, did)).Replace(" ", "");
        }

        [Description("读取FBL版本号")]
        public void ReadFblVersion(string reqCanId, string recvCanId, string did)
        {
            FblVersion = string.Empty;
            FblVersion = ValueHelper.GetHextStr(ReadDid(reqCanId, recvCanId, did)).Replace(" ", "");
        }

        [Description("读取FBL内部版本号")]
        public void ReadInternalFblVersion(string reqCanId, string recvCanId, string did)
        {
            InternalFblVersion = string.Empty;
            InternalFblVersion = ValueHelper.GetHextStr(ReadDid(reqCanId, recvCanId, did)).Replace(" ", "");
        }

        [Description("读取APP零件号")]
        public void ReadAppPartNo(string reqCanId, string recvCanId, string did)
        {
            AppPartNo = string.Empty;
            AppPartNo = ValueHelper.GetHextStr(ReadDid(reqCanId, recvCanId, did)).Replace(" ", "");
        }

        [Description("读取APP版本号")]
        public void ReadAppVersion(string reqCanId, string recvCanId, string did)
        {
            AppVersion = string.Empty;
            AppVersion = ValueHelper.GetHextStr(ReadDid(reqCanId, recvCanId, did)).Replace(" ", "");
        }

        [Description("读取APP内部版本号")]
        public void ReadInternalAppVersion(string reqCanId, string recvCanId, string did)
        {
            InternalAppVersion = string.Empty;
            InternalAppVersion = ValueHelper.GetHextStr(ReadDid(reqCanId, recvCanId, did)).Replace(" ", "");
        }

        private byte[] ReadDid(string reqCanId, string recvCanId, string did)
        {
            var returnList = new List<byte>();
            try
            {
                var ushortDid = Convert.ToUInt16(did, 16);
                var requestCanId = Convert.ToUInt32(reqCanId, 16);
                var responseCanId = Convert.ToUInt32(recvCanId, 16);

                var didHi = BitConverter.GetBytes(ushortDid)[1];
                var didLo = BitConverter.GetBytes(ushortDid)[0];

                if (Can != null)
                {
                    byte[] echo;
                    if (Can.CanBusWithUds.TryReadData(requestCanId, responseCanId, CanBus.CanType.Standard, CanBus.CanProtocol.Can, didHi, didLo, out echo))
                    {
                        if ( echo != null)
                        {
                            returnList.AddRange(echo);
                        }
                    }
                    else
                    {
                        Thread.Sleep(250);
                        if (Can.CanBusWithUds.TryReadData(requestCanId, responseCanId, CanBus.CanType.Standard, CanBus.CanProtocol.Can, didHi, didLo, out echo))
                        {
                            if (echo != null)
                            {
                                returnList.AddRange(echo);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return new byte[0];
            }

            return returnList.ToArray();
        }

        #endregion

        internal enum LampType
        {
            /// <summary>
            /// 前灯A右
            /// </summary>
            Fra,

            /// <summary>
            /// 前灯B右
            /// </summary>
            Frb,

            /// <summary>
            /// 前灯C
            /// </summary>
            Fc,

            /// <summary>
            /// 前灯A左
            /// </summary>
            Fla,

            /// <summary>
            /// 前灯B左
            /// </summary>
            Flb,

            /// <summary>
            /// 前灯B右
            /// 高配
            /// ISD部分
            /// </summary>
            FrbUp,

            /// <summary>
            /// 前灯B左
            /// 高配
            /// ISD部分
            /// </summary>
            FlbUp,
        }

        internal enum SpecialContol
        {
            /// <summary>
            /// 全亮
            /// </summary>
            AllOn,

            /// <summary>
            /// 全灭
            /// </summary>
            AllOff,

            /// <summary>
            /// 白光全亮
            /// </summary>
            AllWhiteOn,

            /// <summary>
            /// 白光偶数点亮
            /// </summary>
            AllWhiteEvenOn,

            /// <summary>
            /// 白光奇数点亮
            /// </summary>
            AllWhiteOddOn,

            /// <summary>
            /// 黄光全亮
            /// </summary>
            AllYellowOn,

            /// <summary>
            /// 黄光偶数点亮
            /// </summary>
            AllYellowEvenOn,

            /// <summary>
            /// 黄光奇数点亮
            /// </summary>
            AllYellowOddOn,
        }
    }
}
