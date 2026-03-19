using System;
using System.Threading;
using HslCommunication.ModBus;

namespace Controller
{
    public sealed class MotorCheckCustomPlc : ControllerBase
    {
        public bool O00001;
        public bool O00002;
        public bool O00003;
        public bool O00004;
        public bool O00005;
        public bool O00006;
        public bool O00007;
        public bool O00008;
        public bool O00009;
        public bool O00010;
        public bool O00011;
        public bool O00012;
        public bool O00013;
        public bool O00014;
        public bool O00015;
        public bool O00016;
        public bool O00017;
        public bool O00018;
        public bool O00019;
        public bool O00020;
        public bool O00021;
        public bool O00022;
        public bool O00023;
        public bool O00024;
        public bool O00025;
        public bool O00026;
        public bool O00027;
        public bool O00028;
        public bool O00029;
        public bool O00030;
        public bool O00031;
        public bool O00032;
        public bool O00033;
        public bool O00034;
        public bool O00035;
        public bool O00036;
        public bool O00037;
        public bool O00038;
        public bool O00039;
        public bool O00040;
        public bool O00041;
        public bool O00042;
        public bool O00043;
        public bool O00044;
        public bool O00045;
        public bool O00046;
        public bool O00047;
        public bool O00048;
        public bool O00049;
        public bool O00050;
        public bool O00051;
        public bool O00052;
        public bool O00053;
        public bool O00054;
        public bool O00055;
        public bool O00056;
        public bool O00057;
        public bool O00058;
        public bool O00059;
        public bool O00060;
        public bool O00061;
        public bool O00062;
        public bool O00063;
        public bool O00064;
        public bool O00065;
        public bool O00066;
        public bool O00067;
        public bool O00068;
        public bool O00069;
        public bool O00070;
        public bool O00071;
        public bool O00072;
        public bool O00073;
        public bool O00074;
        public bool O00075;
        public bool O00076;
        public bool O00077;
        public bool O00078;
        public bool O00079;
        public bool O00080;
        public bool O00081;
        public bool O00082;
        public bool O00083;
        public bool O00084;
        public bool O00085;
        public bool O00086;
        public bool O00087;
        public bool O00088;
        public bool O00089;
        public bool O00090;
        public bool O00091;
        public bool O00092;
        public bool O00093;
        public bool O00094;
        public bool O00095;
        public bool O00096;
        public bool O00097;
        public bool O00098;
        public bool O00099;
        public bool O00100;
        public bool O00101;
        public bool O00102;
        public bool O00103;
        public bool O00104;
        public bool O00105;
        public bool O00106;
        public bool O00107;
        public bool O00108;
        public bool O00109;
        public bool O00110;
        public bool O00111;
        public bool O00112;
        public bool O00113;
        public bool O00114;
        public bool O00115;
        public bool O00116;
        public bool O00117;
        public bool O00118;
        public bool O00119;
        public bool O00120;
        public bool O00121;
        public bool O00122;
        public bool O00123;
        public bool O00124;
        public bool O00125;
        public bool O00126;
        public bool O00127;
        public bool O00128;
        public bool O00129;
        public bool O00130;
        public bool O00131;
        public bool O00132;
        public bool O00133;
        public bool O00134;
        public bool O00135;
        public bool O00136;
        public bool O00137;
        public bool O00138;
        public bool O00139;
        public bool O00140;
        public bool O00141;
        public bool O00142;
        public bool O00143;
        public bool O00144;
        public bool O00145;
        public bool O00146;
        public bool O00147;
        public bool O00148;
        public bool O00149;
        public bool O00150;
        //public bool O00151;
        //public bool O00152;
        //public bool O00153;
        //public bool O00154;
        //public bool O00155;
        //public bool O00156;
        //public bool O00157;
        //public bool O00158;
        //public bool O00159;
        //public bool O00160;
        //public bool O00161;
        //public bool O00162;
        //public bool O00163;
        //public bool O00164;
        //public bool O00165;
        //public bool O00166;
        //public bool O00167;
        //public bool O00168;
        //public bool O00169;
        //public bool O00170;
        //public bool O00171;
        //public bool O00172;
        //public bool O00173;
        //public bool O00174;
        //public bool O00175;
        //public bool O00176;
        //public bool O00177;
        //public bool O00178;
        //public bool O00179;
        //public bool O00180;
        //public bool O00181;
        //public bool O00182;
        //public bool O00183;
        //public bool O00184;
        //public bool O00185;
        //public bool O00186;
        //public bool O00187;
        //public bool O00188;
        //public bool O00189;
        //public bool O00190;
        //public bool O00191;
        //public bool O00192;
        //public bool O00193;
        //public bool O00194;
        //public bool O00195;
        //public bool O00196;
        //public bool O00197;
        //public bool O00198;
        //public bool O00199;
        //public bool O00200;

        public bool I10001;
        public bool I10002;
        public bool I10003;
        public bool I10004;
        public bool I10005;
        public bool I10006;
        public bool I10007;
        public bool I10008;
        public bool I10009;
        public bool I10010;
        public bool I10011;
        public bool I10012;
        public bool I10013;
        public bool I10014;
        public bool I10015;
        public bool I10016;
        public bool I10017;
        public bool I10018;
        public bool I10019;
        public bool I10020;
        public bool I10021;
        public bool I10022;
        public bool I10023;
        public bool I10024;
        public bool I10025;
        public bool I10026;
        public bool I10027;
        public bool I10028;
        public bool I10029;
        public bool I10030;
        public bool I10031;
        public bool I10032;
        public bool I10033;
        public bool I10034;
        public bool I10035;
        public bool I10036;
        public bool I10037;
        public bool I10038;
        public bool I10039;
        public bool I10040;
        public bool I10041;
        public bool I10042;
        public bool I10043;
        public bool I10044;
        public bool I10045;
        public bool I10046;
        public bool I10047;
        public bool I10048;
        public bool I10049;
        public bool I10050;
        public bool I10051;
        public bool I10052;
        public bool I10053;
        public bool I10054;
        public bool I10055;
        public bool I10056;
        public bool I10057;
        public bool I10058;
        public bool I10059;
        public bool I10060;
        public bool I10061;
        public bool I10062;
        public bool I10063;
        public bool I10064;
        public bool I10065;
        public bool I10066;
        public bool I10067;
        public bool I10068;
        public bool I10069;
        public bool I10070;
        public bool I10071;
        public bool I10072;
        public bool I10073;
        public bool I10074;
        public bool I10075;
        public bool I10076;
        public bool I10077;
        public bool I10078;
        public bool I10079;
        public bool I10080;
        public bool I10081;
        public bool I10082;
        public bool I10083;
        public bool I10084;
        public bool I10085;
        public bool I10086;
        public bool I10087;
        public bool I10088;
        public bool I10089;
        public bool I10090;
        public bool I10091;
        public bool I10092;
        public bool I10093;
        public bool I10094;
        public bool I10095;
        public bool I10096;
        public bool I10097;
        public bool I10098;
        public bool I10099;
        public bool I10100;
        public bool I10101;
        public bool I10102;
        public bool I10103;
        public bool I10104;
        public bool I10105;
        public bool I10106;
        public bool I10107;
        public bool I10108;
        public bool I10109;
        public bool I10110;
        public bool I10111;
        public bool I10112;
        public bool I10113;
        public bool I10114;
        public bool I10115;
        public bool I10116;
        public bool I10117;
        public bool I10118;
        public bool I10119;
        public bool I10120;
        public bool I10121;
        public bool I10122;
        public bool I10123;
        public bool I10124;
        public bool I10125;
        public bool I10126;
        public bool I10127;
        public bool I10128;
        public bool I10129;
        public bool I10130;
        public bool I10131;
        public bool I10132;
        public bool I10133;
        public bool I10134;
        public bool I10135;
        public bool I10136;
        public bool I10137;
        public bool I10138;
        public bool I10139;
        public bool I10140;
        public bool I10141;
        public bool I10142;
        public bool I10143;
        public bool I10144;
        public bool I10145;
        public bool I10146;
        public bool I10147;
        public bool I10148;
        public bool I10149;
        public bool I10150;
        //public bool I10151;
        //public bool I10152;
        //public bool I10153;
        //public bool I10154;
        //public bool I10155;
        //public bool I10156;
        //public bool I10157;
        //public bool I10158;
        //public bool I10159;
        //public bool I10160;
        //public bool I10161;
        //public bool I10162;
        //public bool I10163;
        //public bool I10164;
        //public bool I10165;
        //public bool I10166;
        //public bool I10167;
        //public bool I10168;
        //public bool I10169;
        //public bool I10170;
        //public bool I10171;
        //public bool I10172;
        //public bool I10173;
        //public bool I10174;
        //public bool I10175;
        //public bool I10176;
        //public bool I10177;
        //public bool I10178;
        //public bool I10179;
        //public bool I10180;
        //public bool I10181;
        //public bool I10182;
        //public bool I10183;
        //public bool I10184;
        //public bool I10185;
        //public bool I10186;
        //public bool I10187;
        //public bool I10188;
        //public bool I10189;
        //public bool I10190;
        //public bool I10191;
        //public bool I10192;
        //public bool I10193;
        //public bool I10194;
        //public bool I10195;
        //public bool I10196;
        //public bool I10197;
        //public bool I10198;
        //public bool I10199;
        //public bool I10200;

        public float Gt1Read;
        public float Gt2Read;
        public float Gt3Read;
        public float Gt4Read;

        private ModbusTcpNet _modbusTcpClient { get; set; }
        private Thread DaemonTh { get; set; }

        public MotorCheckCustomPlc(string name)
            : base(name) { }

        public void InitRemoteAddress(string ipPort)
        {
            var split = ipPort.Split(':');
            var ipAddressStr = split[0];
            var port = Convert.ToInt32(split[1]);
            _modbusTcpClient = new ModbusTcpNet(ipAddressStr, port) { ReceiveTimeOut = 250 };
            _modbusTcpClient.ConnectServer();
        }

        public void ReadIo()
        {
            while (true)
            {
                #region Read

                var r1Content = new bool[150];
                while (true)
                {
                    var r1 = _modbusTcpClient.ReadCoil("0", 150); // O00001
                    if (!r1.IsSuccess)
                        continue;

                    Array.Copy(r1.Content, r1Content, r1.Content.Length);
                    break;
                }

                foreach (var p in GetType().GetFields())
                {
                    var name = p.Name;

                    if (!name.StartsWith("O0") || name.Length != 6)
                        continue;
                    var addr = int.Parse(name.Substring("O".Length).Substring(1));
                    p.SetValue(this, r1Content[addr - 1]);
                }

                break;

                #endregion
            }

            Read();
        }

        public void StartReadWriteTh()
        {
            if (DaemonTh != null)
            {
                DaemonTh.Abort();
                DaemonTh.Join();
            }

            DaemonTh = new Thread(ReadWriteWork) { IsBackground = true };
            DaemonTh.Start();
        }

        private void ReadWriteWork()
        {
            while (DaemonTh.IsAlive)
            {
                if (!DaemonTh.IsAlive)
                    break;

                Thread.Sleep(25);
                Read();
                Thread.Sleep(25);
                Write();
                Thread.Sleep(25);
                ReadGt();
            }
        }

        private void Read()
        {
            while (true)
            {
                #region Read

                var r1Content = new bool[150];
                while (true)
                {
                    var r1 = _modbusTcpClient.ReadDiscrete("0", 150); // I10001
                    if (!r1.IsSuccess)
                        continue;

                    Array.Copy(r1.Content, r1Content, r1.Content.Length);
                    break;
                }

                foreach (var p in GetType().GetFields())
                {
                    var name = p.Name;

                    if (!name.StartsWith("I1") || name.Length != 6)
                        continue;
                    var addr = int.Parse(name.Substring("I".Length).Substring(1));
                    p.SetValue(this, r1Content[addr - 1]);
                }

                return;

                #endregion
            }
        }

        private void Write()
        {
            var bools = new bool[150];

            foreach (var p in GetType().GetFields())
            {
                var name = p.Name;

                if (!name.StartsWith("O0") || name.Length != 6)
                    continue;

                var addr = int.Parse(name.Substring("O".Length).Substring(1));
                var value = Convert.ToString(p.GetValue(this));
                bools[addr - 1] = bool.Parse(value);
            }

            while (true)
            {
                var w1 = _modbusTcpClient.WriteCoil("0", bools); // O00001

                if (!w1.IsSuccess)
                    continue;
                break;
            }
        }

        public void ReadGt()
        {
            while (true)
            {
                #region Read

                var r1Content = new Int32[4];
                while (true)
                {
                    var r1 = _modbusTcpClient.ReadInt32("0", 4); // R40009
                    if (!r1.IsSuccess)
                        continue;

                    Array.Copy(r1.Content, r1Content, r1.Content.Length);
                    break;
                }

                Gt1Read = r1Content[0] * 0.0001f;
                Gt2Read = r1Content[1] * 0.0001f;
                Gt3Read = r1Content[2] * 0.0001f;
                Gt4Read = r1Content[3] * 0.0001f;

                return;

                #endregion
            }
        }
    }
}
