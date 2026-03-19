using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace Controller
{
    [Description("LIN-Product,NDLB-PTC")]
    public class NDLB19200 : ControllerBase
    {
        #region 

        private string Part1_UpdateTimesinaDay;
        private string Part2_2SWUpdateMonth;
        private string Part2_3SWUpdateDay;
        private string Part3ASWVersion;
        private string Part4BSWVersion;
        private string Part2PlatformVoltage;
        private string Part6ProjectInformation;
        private string Part1OrPart5_2HWVersion;
        private string Part5_1ProjectPhase;
        private string Part2_1SWUpdateYear;
        private string FaultFlagsByte2;
        private string FaultFlagsByte3;

        [Description("R,版本号")]
        public string Vertion;
        [Description("R,软件版本号")]
        public string SoftVer;
        [Description("R,硬件版本号")]
        public string HardVer;

        #endregion

        public LinBus linBus19200;
        public NDLB19200(string name) : base(name)
        { }

        ~NDLB19200()
        {
            Dispose();
        }

        [Description("读取版本号")]
        public void ReadVar()
        {
            try
            {
                //先唤醒
                var sendBytes = new byte[] { 0x63, 0x06, 0xB2, 0x20, 0x91, 0x00, 0x06, 0x06 };
                linBus19200.SendMasterLin(0x3C, sendBytes); Thread.Sleep(10);
                linBus19200.SendMasterLin(0x3C, sendBytes); Thread.Sleep(10);
                linBus19200.SendMasterLin(0x3C, sendBytes); Thread.Sleep(10);
                linBus19200.SendMasterLin(0x3C, sendBytes); Thread.Sleep(10);
                linBus19200.SendMasterLin(0x3C, sendBytes); Thread.Sleep(10);
                linBus19200.SendMasterLin(0x3C, sendBytes); Thread.Sleep(10);
                linBus19200.SendMasterLin(0x3C, sendBytes); Thread.Sleep(10);
                Thread.Sleep(10);
                Part3ASWVersion = string.Empty;
                Part3ASWVersion = string.Empty;
                Part1OrPart5_2HWVersion = string.Empty;
                Part5_1ProjectPhase = string.Empty;
                Part1_UpdateTimesinaDay = string.Empty;
                Part2_2SWUpdateMonth = string.Empty;
                Part2_3SWUpdateDay = string.Empty;
                Part3ASWVersion = string.Empty;
                Part4BSWVersion = string.Empty;
                Part2PlatformVoltage = string.Empty;
                Part6ProjectInformation = string.Empty;
                Part1OrPart5_2HWVersion = string.Empty;
                Part5_1ProjectPhase = string.Empty;
                Part2_1SWUpdateYear = string.Empty;
                SoftVer = string.Empty;
                HardVer = string.Empty;
                Thread.Sleep(100);
                for (int i = 0; i < 3; i++)
                {
                    linBus19200.SendMasterLin(0x3C, sendBytes);
                    byte[] recv;
                    var isReadFirstFrameSucceed = linBus19200.SendSlaveLin(0x3D, out recv);
                    if (isReadFirstFrameSucceed && recv != null && recv.Length == 8 && recv[0] == 0x63 && recv[1] == 0x06 && recv[2] == 0xF2)
                    {
                        var newrecv = new byte[] { recv[4] };
                        Console.WriteLine(ValueHelper.GetHextStr(recv));

                        Part1_UpdateTimesinaDay = GetintValueByBytes(newrecv, 0, 4).ToString();
                        Part2_2SWUpdateMonth = GetintValueByBytes(newrecv, 4, 4).ToString().PadLeft(2, '0');
                        Part2_3SWUpdateDay = Convert.ToInt32(ValueHelper.GetHextStr(recv[5]), 16).ToString().PadLeft(2, '0');
                        Part3ASWVersion = Convert.ToInt32(ValueHelper.GetHextStr(recv[6]), 16).ToString().PadLeft(2, '0');
                        Part4BSWVersion = Convert.ToInt32(ValueHelper.GetHextStr(recv[7]), 16).ToString().PadLeft(2, '0');
                    }
                    if (!string.IsNullOrEmpty(Part3ASWVersion) && !string.IsNullOrEmpty(Part4BSWVersion))
                    {
                        break;
                    }
                    Thread.Sleep(10);
                }

                for (int i = 0; i < 3; i++)
                {
                    sendBytes = new byte[] { 0x63, 0x06, 0xB2, 0x21, 0x91, 0x00, 0x06, 0x06 };
                    linBus19200.SendMasterLin(0x3C, sendBytes);
                    Thread.Sleep(100);
                    byte[] recv21;
                    var isReadFirstFrameSucceed = linBus19200.SendSlaveLin(0x3D, out recv21);
                    //recv21 = new byte[] { 0x63, 0x06, 0xf2, 0x21, 0x44, 0x10, 0x41, 0x19 };
                    if (isReadFirstFrameSucceed && recv21 != null && recv21.Length == 8 && recv21[0] == 0x63 && recv21[1] == 0x06 && recv21[2] == 0xF2)
                    {
                        var newrecv21 = new byte[] { recv21[4], recv21[5] };

                        Console.WriteLine(ValueHelper.GetHextStr(recv21));

                        Part2PlatformVoltage = GetintValueByBytes(newrecv21, 0, 4).ToString();
                        Part6ProjectInformation = GetintValueByBytes(newrecv21, 4, 6).ToString();
                        Part1OrPart5_2HWVersion = GetintValueByBytes(newrecv21, 10, 6).ToString().PadLeft(2, '0');
                        Part5_1ProjectPhase = ValueHelper.GetStringByAsciiByte(recv21[6], false);
                        Part2_1SWUpdateYear = (Convert.ToInt32((ValueHelper.GetHextStr(recv21[7])), 16) + 2000).ToString();
                    }
                    if (!string.IsNullOrEmpty(Part1OrPart5_2HWVersion) && !string.IsNullOrEmpty(Part5_1ProjectPhase))
                    {
                        break;
                    }
                    Thread.Sleep(10);
                }

                var part6 = "";
                switch (Part6ProjectInformation)
                {
                    case "0":
                        part6 = "ORIGIN400V2Z";
                        break;
                    case "1":
                        part6 = "SGM_400V1Z";
                        break;
                    case "2":
                        part6 = "SGM_APP800V1Z";
                        break;
                    case "3":
                        part6 = "SGM_APP800V2Z";
                        break;
                    case "4":
                        part6 = "SGM_NDLB400V1Z";
                        break;
                    default:
                        part6 = "Invalid_Info";
                        break;
                }

                SoftVer = Vertion = part6 + "." + Part5_1ProjectPhase + Part1OrPart5_2HWVersion + "." + Part4BSWVersion + "." + Part3ASWVersion + "." + Part2_1SWUpdateYear + Part2_2SWUpdateMonth + Part2_3SWUpdateDay + "." + Part1_UpdateTimesinaDay;
                HardVer = Part2PlatformVoltage + Part1OrPart5_2HWVersion;
            }
            catch
            { }
        }

        private string GetBitValueByBytes(byte[] bytes, int startIndex, int len)
        {
            var bsList = new List<string>();
            bytes = bytes.Reverse().ToArray();
            foreach (var bs in bytes.Select(t => Convert.ToString(t, 2).PadLeft(8, '0')))
                for (var i = 0; i < bs.Length; i++)
                    bsList.Add(bs[i].ToString());

            var tempBs = new List<string>();
            for (var i = 0; i < len; i++)
                tempBs.Add(bsList[startIndex + i]);

            return tempBs.Aggregate(string.Empty, (T, P) => T + P.ToString());
        }
        private int GetintValueByBytes(byte[] bytes, int startIndex, int len)
        {
            var bsList = new List<string>();
            //bytes = bytes.Reverse().ToArray();
            foreach (var bs in bytes.Select(t => Convert.ToString(t, 2).PadLeft(8, '0')))
                for (var i = bs.Length - 1; i >= 0; i--)
                    bsList.Add(bs[i].ToString());

            var tempBs = new List<string>();
            for (var i = 0; i < len; i++)
                tempBs.Add(bsList[startIndex + i]);
            var str = string.Empty;
            for (var i = tempBs.Count - 1; i >= 0; i--)
                str += tempBs[i];
            return Convert.ToInt32(str, 2); ;
        }
    }
}
