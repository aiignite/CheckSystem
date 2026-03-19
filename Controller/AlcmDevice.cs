using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using CommonUtility;

namespace Controller
{
    public sealed class AlcmDevice : ControllerBase
    {
        public AlcmDevice(string name)
            : base(name) { }

        ~AlcmDevice()
        {
            if (AudioPlayer != null)
            {
                AudioPlayer.Dispose();
            }
        }

        public int ProductType; // 0=d2uc, 1=c1yb, 2=ndlb
        public bool IsNeedRefresh;

        public string DrvFilePath;
        public string AppFilePath;
        public string CaliFilePAth;

        [Description("R,工位1静态电流")]
        public double Ws1QuiescentCurrent;
        [Description("R,工位2静态电流")]
        public double Ws2QuiescentCurrent;
        [Description("R,工位3静态电流")]
        public double Ws3QuiescentCurrent;
        [Description("R,工位4静态电流")]
        public double Ws4QuiescentCurrent;

        [Description("R,工位1是否OK")]
        public ushort IsWs1Ok;
        [Description("R,工位2是否OK")]
        public ushort IsWs2Ok;
        [Description("R,工位3是否OK")]
        public ushort IsWs3Ok;
        [Description("R,工位4是否OK")]
        public ushort IsWs4Ok;

        [Description("R,工位1二维码")]
        public string Ws1Barcode;
        [Description("R,工位2二维码")]
        public string Ws2Barcode;
        [Description("R,工位3二维码")]
        public string Ws3Barcode;
        [Description("R,工位4二维码")]
        public string Ws4Barcode;

        [Description("R,工位1二维码")]
        public string Ws1PcbaBarcode;
        [Description("R,工位2二维码")]
        public string Ws2PcbaBarcode;
        [Description("R,工位3二维码")]
        public string Ws3PcbaBarcode;
        [Description("R,工位4二维码")]
        public string Ws4PcbaBarcode;

        [Description("R,工位1是否完成")]
        public ushort IsWs1Complete;
        [Description("R,工位2是否完成")]
        public ushort IsWs2Complete;
        [Description("R,工位3是否完成")]
        public ushort IsWs3Complete;
        [Description("R,工位4是否完成")]
        public ushort IsWs4Complete;

        public string Ws1ModbusBarcode;
        public string Ws2ModbusBarcode;
        public string Ws3ModbusBarcode;
        public string Ws4ModbusBarcode;

        public bool Execute;
        public bool Complete;

        [Description("R/W,音乐文件路径")]
        public string MusicFilePath = @"Dreamitpossible.mp3";

        private AudioPlayer AudioPlayer { get; set; }

        [Description("播放音乐")]
        public void PlayMusic()
        {
            var filePath = string.Format(@"{0}\{1}", Directory.GetCurrentDirectory(), MusicFilePath);

            if (!File.Exists(filePath))
                return;
            if (AudioPlayer != null)
                AudioPlayer.Dispose();

            AudioPlayer = new AudioPlayer(filePath);
            AudioPlayer.Play();
        }

        [Description("停止音乐")]
        public void StopMusic()
        {
            if (AudioPlayer == null)
                return;
            AudioPlayer.Stop();
            AudioPlayer.Dispose();
            AudioPlayer = null;
        }

        [Description("生成二维码信息")]
        public void Generate4Barcode(string generalPartNo, string generalVpps, string seeyaoDuns, string track1, string track2, string track3)
        {
            Ws1Barcode = string.Empty;
            Ws2Barcode = string.Empty;
            Ws3Barcode = string.Empty;
            Ws4Barcode = string.Empty;

            Ws1ModbusBarcode = string.Empty;
            Ws2ModbusBarcode = string.Empty;
            Ws3ModbusBarcode = string.Empty;
            Ws4ModbusBarcode = string.Empty;

            Ws1Barcode = Generate1Barcode(generalPartNo, generalVpps, seeyaoDuns, track1, track2, track3);
            Ws2Barcode = Generate1Barcode(generalPartNo, generalVpps, seeyaoDuns, track1, track2, track3);
            Ws3Barcode = Generate1Barcode(generalPartNo, generalVpps, seeyaoDuns, track1, track2, track3);
            Ws4Barcode = Generate1Barcode(generalPartNo, generalVpps, seeyaoDuns, track1, track2, track3);

            var sp1 = Ws1Barcode.Split(new[] { Encoding.ASCII.GetString(new byte[] { 0x1D }) }, StringSplitOptions.RemoveEmptyEntries);
            Ws1ModbusBarcode = string.Format("{0},{1},{2},{3}", sp1[1].TrimStart('Y'), sp1[2].TrimStart('P'), sp1[3].TrimStart('1', '2', 'V'), sp1[4].TrimStart('T')).TrimEnd((char)0x1E, (char)0x04);

            var sp2 = Ws2Barcode.Split(new[] { Encoding.ASCII.GetString(new byte[] { 0x1D }) }, StringSplitOptions.RemoveEmptyEntries);
            Ws2ModbusBarcode = string.Format("{0},{1},{2},{3}", sp2[1].TrimStart('Y'), sp2[2].TrimStart('P'), sp2[3].TrimStart('1', '2', 'V'), sp2[4].TrimStart('T')).TrimEnd((char)0x1E, (char)0x04);

            var sp3 = Ws3Barcode.Split(new[] { Encoding.ASCII.GetString(new byte[] { 0x1D }) }, StringSplitOptions.RemoveEmptyEntries);
            Ws3ModbusBarcode = string.Format("{0},{1},{2},{3}", sp3[1].TrimStart('Y'), sp3[2].TrimStart('P'), sp3[3].TrimStart('1', '2', 'V'), sp3[4].TrimStart('T')).TrimEnd((char)0x1E, (char)0x04);

            var sp4 = Ws4Barcode.Split(new[] { Encoding.ASCII.GetString(new byte[] { 0x1D }) }, StringSplitOptions.RemoveEmptyEntries);
            Ws4ModbusBarcode = string.Format("{0},{1},{2},{3}", sp4[1].TrimStart('Y'), sp4[2].TrimStart('P'), sp4[3].TrimStart('1', '2', 'V'), sp4[4].TrimStart('T')).TrimEnd((char)0x1E, (char)0x04);
        }

        private string Generate1Barcode(string generalPartNo, string generalVpps, string seeyaoDuns, string track1, string track2, string track3)
        {
            if (string.IsNullOrEmpty(generalPartNo) || generalPartNo.Length != 8)
                return string.Empty;

            if (string.IsNullOrEmpty(generalVpps))
                return string.Empty;

            if (string.IsNullOrEmpty(seeyaoDuns) || seeyaoDuns.Length != 9)
                return string.Empty;

            if (string.IsNullOrEmpty(track1) || track1.Length != 1)
                return string.Empty;

            if (string.IsNullOrEmpty(track2) || track2.Length != 2)
                return string.Empty;

            if (string.IsNullOrEmpty(track3) || track3.Length != 2)
                return string.Empty;

            //if (string.IsNullOrEmpty(TrackInfo) || !TrackInfo.StartsWith("OK "))
            //    return string.Empty;

            //var tracInfo = TrackInfo.Replace("OK ", "");

            var matrixBytes = new List<byte>();
            matrixBytes.AddRange(new byte[] { 0x5B, 0x29, 0x3E, 0x1E, 0x30, 0x36, 0x1D, 0x59 }); // header:[)><RS>06<GS>Y
            matrixBytes.AddRange(Encoding.ASCII.GetBytes(generalVpps)); // 通用VPPS号
            matrixBytes.Add(0x1D); //<GS>
            matrixBytes.AddRange(Encoding.ASCII.GetBytes("P" + generalPartNo)); //  P+通用零件号
            matrixBytes.Add(0x1D); //<GS>
            matrixBytes.AddRange(Encoding.ASCII.GetBytes("12V" + seeyaoDuns)); //  12V+DUNS邓氏码
            matrixBytes.Add(0x1D); //<GS>

            string outDate;
            string outSerialNo;
            if (!GetDateAndSerialNumber(false, generalPartNo, out outDate, out outSerialNo))
                return string.Empty;
            var tracInfo = string.Format("1A{0}{1}{2}{3}{4}{5}", DateTime.Parse(outDate).Year.ToString().Substring(2, 2),
                DateTime.Parse(outDate).DayOfYear.ToString().PadLeft(3, '0'), track1, track2, track3,
                outSerialNo.PadLeft(4, '0'));

            matrixBytes.AddRange(Encoding.ASCII.GetBytes("T" + tracInfo)); //  追溯码
            matrixBytes.AddRange(new byte[] { 0x1E, 0x04 }); // trailer <RS>

            var generatedBarcode = matrixBytes.ToArray().GetStringByAsciiBytes(false);

            return generatedBarcode;
        }
    }
}
