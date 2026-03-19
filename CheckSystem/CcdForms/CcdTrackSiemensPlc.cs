using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using HslCommunication.ModBus;

namespace CheckSystem.CcdForms
{
    public sealed class CcdTrackSiemensPlc : Controller.ControllerBase
    {
        public delegate void PushValueEventHandle(string controllerName, string fieldName, string value);
        public static event PushValueEventHandle PushValue;

        #region 输入信号
        [Description("R,输入%I0.0")]
        public bool I10001;
        [Description("R,输入%I0.1")]
        public bool I10002;
        [Description("R,输入%I0.2")]
        public bool I10003;
        [Description("R,输入%I0.3")]
        public bool I10004;
        [Description("R,输入%I0.4")]
        public bool I10005;
        [Description("R,输入%I0.5")]
        public bool I10006;
        [Description("R,输入%I0.6")]
        public bool I10007;
        [Description("R,输入%I0.7")]
        public bool I10008;
        [Description("R,输入%I1.0")]
        public bool I10009;
        [Description("R,输入%I1.1")]
        public bool I10010;
        [Description("R,输入%I1.2")]
        public bool I10011;
        [Description("R,输入%I1.3")]
        public bool I10012;
        [Description("R,输入%I1.4")]
        public bool I10013;
        [Description("R,输入%I1.5")]
        public bool I10014;
        [Description("R,输入%I1.6")]
        public bool I10015;
        [Description("R,输入%I1.7")]
        public bool I10016;
        [Description("R,输入%I2.0")]
        public bool I10017;
        [Description("R,输入%I2.1")]
        public bool I10018;
        [Description("R,输入%I2.2")]
        public bool I10019;
        [Description("R,输入%I2.3")]
        public bool I10020;
        [Description("R,输入%I2.4")]
        public bool I10021;
        [Description("R,输入%I2.5")]
        public bool I10022;
        [Description("R,输入%I2.6")]
        public bool I10023;
        [Description("R,输入%I2.7")]
        public bool I10024;
        [Description("R,输入%I3.0")]
        public bool I10025;
        [Description("R,输入%I3.1")]
        public bool I10026;
        [Description("R,输入%I3.2")]
        public bool I10027;
        [Description("R,输入%I3.3")]
        public bool I10028;
        [Description("R,输入%I3.4")]
        public bool I10029;
        [Description("R,输入%I3.5")]
        public bool I10030;
        [Description("R,输入%I3.6")]
        public bool I10031;
        [Description("R,输入%I3.7")]
        public bool I10032;
        [Description("R,输入%I4.0")]
        public bool I10033;
        [Description("R,输入%I4.1")]
        public bool I10034;
        [Description("R,输入%I4.2")]
        public bool I10035;
        [Description("R,输入%I4.3")]
        public bool I10036;
        [Description("R,输入%I4.4")]
        public bool I10037;
        [Description("R,输入%I4.5")]
        public bool I10038;
        [Description("R,输入%I4.6")]
        public bool I10039;
        [Description("R,输入%I4.7")]
        public bool I10040;
        [Description("R,输入%I5.0")]
        public bool I10041;
        [Description("R,输入%I5.1")]
        public bool I10042;
        [Description("R,输入%I5.2")]
        public bool I10043;
        [Description("R,输入%I5.3")]
        public bool I10044;
        [Description("R,输入%I5.4")]
        public bool I10045;
        [Description("R,输入%I5.5")]
        public bool I10046;
        [Description("R,输入%I5.6")]
        public bool I10047;
        [Description("R,输入%I5.7")]
        public bool I10048;
        [Description("R,输入%I6.0")]
        public bool I10049;
        [Description("R,输入%I6.1")]
        public bool I10050;
        [Description("R,输入%I6.2")]
        public bool I10051;
        [Description("R,输入%I6.3")]
        public bool I10052;
        [Description("R,输入%I6.4")]
        public bool I10053;
        [Description("R,输入%I6.5")]
        public bool I10054;
        [Description("R,输入%I6.6")]
        public bool I10055;
        [Description("R,输入%I6.7")]
        public bool I10056;
        [Description("R,输入%I7.0")]
        public bool I10057;
        [Description("R,输入%I7.1")]
        public bool I10058;
        [Description("R,输入%I7.2")]
        public bool I10059;
        [Description("R,输入%I7.3")]
        public bool I10060;
        [Description("R,输入%I7.4")]
        public bool I10061;
        [Description("R,输入%I7.5")]
        public bool I10062;
        [Description("R,输入%I7.6")]
        public bool I10063;
        [Description("R,输入%I7.7")]
        public bool I10064;
        [Description("R,输入%I8.0")]
        public bool I10065;
        [Description("R,输入%I8.1")]
        public bool I10066;
        [Description("R,输入%I8.2")]
        public bool I10067;
        [Description("R,输入%I8.3")]
        public bool I10068;
        [Description("R,输入%I8.4")]
        public bool I10069;
        [Description("R,输入%I8.5")]
        public bool I10070;
        [Description("R,输入%I8.6")]
        public bool I10071;
        [Description("R,输入%I8.7")]
        public bool I10072;
        [Description("R,输入%I9.0")]
        public bool I10073;
        [Description("R,输入%I9.1")]
        public bool I10074;
        [Description("R,输入%I9.2")]
        public bool I10075;
        [Description("R,输入%I9.3")]
        public bool I10076;
        [Description("R,输入%I9.4")]
        public bool I10077;
        [Description("R,输入%I9.5")]
        public bool I10078;
        [Description("R,输入%I9.6")]
        public bool I10079;
        [Description("R,输入%I9.7")]
        public bool I10080;
        [Description("R,输入%I10.0")]
        public bool I10081;
        [Description("R,输入%I10.1")]
        public bool I10082;
        [Description("R,输入%I10.2")]
        public bool I10083;
        [Description("R,输入%I10.3")]
        public bool I10084;
        [Description("R,输入%I10.4")]
        public bool I10085;
        [Description("R,输入%I10.5")]
        public bool I10086;
        [Description("R,输入%I10.6")]
        public bool I10087;
        [Description("R,输入%I10.7")]
        public bool I10088;
        [Description("R,输入%I11.0")]
        public bool I10089;
        [Description("R,输入%I11.1")]
        public bool I10090;
        [Description("R,输入%I11.2")]
        public bool I10091;
        [Description("R,输入%I11.3")]
        public bool I10092;
        [Description("R,输入%I11.4")]
        public bool I10093;
        [Description("R,输入%I11.5")]
        public bool I10094;
        [Description("R,输入%I11.6")]
        public bool I10095;
        [Description("R,输入%I11.7")]
        public bool I10096;
        [Description("R,输入%I12.0")]
        public bool I10097;
        [Description("R,输入%I12.1")]
        public bool I10098;
        [Description("R,输入%I12.2")]
        public bool I10099;
        [Description("R,输入%I12.3")]
        public bool I10100;
        [Description("R,输入%I12.4")]
        public bool I10101;
        [Description("R,输入%I12.5")]
        public bool I10102;
        [Description("R,输入%I12.6")]
        public bool I10103;
        [Description("R,输入%I12.7")]
        public bool I10104;
        [Description("R,输入%I13.0")]
        public bool I10105;
        [Description("R,输入%I13.1")]
        public bool I10106;
        [Description("R,输入%I13.2")]
        public bool I10107;
        [Description("R,输入%I13.3")]
        public bool I10108;
        [Description("R,输入%I13.4")]
        public bool I10109;
        [Description("R,输入%I13.5")]
        public bool I10110;
        [Description("R,输入%I13.6")]
        public bool I10111;
        [Description("R,输入%I13.7")]
        public bool I10112;
        [Description("R,输入%I14.0")]
        public bool I10113;
        [Description("R,输入%I14.1")]
        public bool I10114;
        [Description("R,输入%I14.2")]
        public bool I10115;
        [Description("R,输入%I14.3")]
        public bool I10116;
        [Description("R,输入%I14.4")]
        public bool I10117;
        [Description("R,输入%I14.5")]
        public bool I10118;
        [Description("R,输入%I14.6")]
        public bool I10119;
        [Description("R,输入%I14.7")]
        public bool I10120;
        [Description("R,输入%I15.0")]
        public bool I10121;
        [Description("R,输入%I15.1")]
        public bool I10122;
        [Description("R,输入%I15.2")]
        public bool I10123;
        [Description("R,输入%I15.3")]
        public bool I10124;
        [Description("R,输入%I15.4")]
        public bool I10125;
        [Description("R,输入%I15.5")]
        public bool I10126;
        [Description("R,输入%I15.6")]
        public bool I10127;
        [Description("R,输入%I15.7")]
        public bool I10128;
        [Description("R,输入%I16.0")]
        public bool I10129;
        [Description("R,输入%I16.1")]
        public bool I10130;
        [Description("R,输入%I16.2")]
        public bool I10131;
        [Description("R,输入%I16.3")]
        public bool I10132;
        [Description("R,输入%I16.4")]
        public bool I10133;
        [Description("R,输入%I16.5")]
        public bool I10134;
        [Description("R,输入%I16.6")]
        public bool I10135;
        [Description("R,输入%I16.7")]
        public bool I10136;
        [Description("R,输入%I17.0")]
        public bool I10137;
        [Description("R,输入%I17.1")]
        public bool I10138;
        [Description("R,输入%I17.2")]
        public bool I10139;
        [Description("R,输入%I17.3")]
        public bool I10140;
        [Description("R,输入%I17.4")]
        public bool I10141;
        [Description("R,输入%I17.5")]
        public bool I10142;
        [Description("R,输入%I17.6")]
        public bool I10143;
        [Description("R,输入%I17.7")]
        public bool I10144;
        [Description("R,输入%I18.0")]
        public bool I10145;
        [Description("R,输入%I18.1")]
        public bool I10146;
        [Description("R,输入%I18.2")]
        public bool I10147;
        [Description("R,输入%I18.3")]
        public bool I10148;
        [Description("R,输入%I18.4")]
        public bool I10149;
        [Description("R,输入%I18.5")]
        public bool I10150;
        [Description("R,输入%I18.6")]
        public bool I10151;
        [Description("R,输入%I18.7")]
        public bool I10152;
        [Description("R,输入%I19.0")]
        public bool I10153;
        [Description("R,输入%I19.1")]
        public bool I10154;
        [Description("R,输入%I19.2")]
        public bool I10155;
        [Description("R,输入%I19.3")]
        public bool I10156;
        [Description("R,输入%I19.4")]
        public bool I10157;
        [Description("R,输入%I19.5")]
        public bool I10158;
        [Description("R,输入%I19.6")]
        public bool I10159;
        [Description("R,输入%I19.7")]
        public bool I10160;
        [Description("R,输入%I20.0")]
        public bool I10161;
        [Description("R,输入%I20.1")]
        public bool I10162;
        [Description("R,输入%I20.2")]
        public bool I10163;
        [Description("R,输入%I20.3")]
        public bool I10164;
        [Description("R,输入%I20.4")]
        public bool I10165;
        [Description("R,输入%I20.5")]
        public bool I10166;
        [Description("R,输入%I20.6")]
        public bool I10167;
        [Description("R,输入%I20.7")]
        public bool I10168;
        [Description("R,输入%I21.0")]
        public bool I10169;
        [Description("R,输入%I21.1")]
        public bool I10170;
        [Description("R,输入%I21.2")]
        public bool I10171;
        [Description("R,输入%I21.3")]
        public bool I10172;
        [Description("R,输入%I21.4")]
        public bool I10173;
        [Description("R,输入%I21.5")]
        public bool I10174;
        [Description("R,输入%I21.6")]
        public bool I10175;
        [Description("R,输入%I21.7")]
        public bool I10176;
        [Description("R,输入%I22.0")]
        public bool I10177;
        [Description("R,输入%I22.1")]
        public bool I10178;
        [Description("R,输入%I22.2")]
        public bool I10179;
        [Description("R,输入%I22.3")]
        public bool I10180;
        [Description("R,输入%I22.4")]
        public bool I10181;
        [Description("R,输入%I22.5")]
        public bool I10182;
        [Description("R,输入%I22.6")]
        public bool I10183;
        [Description("R,输入%I22.7")]
        public bool I10184;
        #endregion

        public bool IsReadO = true;
        public bool IsWriteO = true;

        #region 输出信号
        [Description("R/W,输出%Q0.0")]
        public bool O00001;
        [Description("R/W,输出%Q0.1")]
        public bool O00002;
        [Description("R/W,输出%Q0.2")]
        public bool O00003;
        [Description("R/W,输出%Q0.3")]
        public bool O00004;
        [Description("R/W,输出%Q0.4")]
        public bool O00005;
        [Description("R/W,输出%Q0.5")]
        public bool O00006;
        [Description("R/W,输出%Q0.6")]
        public bool O00007;
        [Description("R/W,输出%Q0.7")]
        public bool O00008;
        [Description("R/W,输出%Q1.0")]
        public bool O00009;
        [Description("R/W,输出%Q1.1")]
        public bool O00010;
        [Description("R/W,输出%Q1.2")]
        public bool O00011;
        [Description("R/W,输出%Q1.3")]
        public bool O00012;
        [Description("R/W,输出%Q1.4")]
        public bool O00013;
        [Description("R/W,输出%Q1.5")]
        public bool O00014;
        [Description("R/W,输出%Q1.6")]
        public bool O00015;
        [Description("R/W,输出%Q1.7")]
        public bool O00016;
        [Description("R/W,输出%Q2.0")]
        public bool O00017;
        [Description("R/W,输出%Q2.1")]
        public bool O00018;
        [Description("R/W,输出%Q2.2")]
        public bool O00019;
        [Description("R/W,输出%Q2.3")]
        public bool O00020;
        [Description("R/W,输出%Q2.4")]
        public bool O00021;
        [Description("R/W,输出%Q2.5")]
        public bool O00022;
        [Description("R/W,输出%Q2.6")]
        public bool O00023;
        [Description("R/W,输出%Q2.7")]
        public bool O00024;
        [Description("R/W,输出%Q3.0")]
        public bool O00025;
        [Description("R/W,输出%Q3.1")]
        public bool O00026;
        [Description("R/W,输出%Q3.2")]
        public bool O00027;
        [Description("R/W,输出%Q3.3")]
        public bool O00028;
        [Description("R/W,输出%Q3.4")]
        public bool O00029;
        [Description("R/W,输出%Q3.5")]
        public bool O00030;
        [Description("R/W,输出%Q3.6")]
        public bool O00031;
        [Description("R/W,输出%Q3.7")]
        public bool O00032;
        [Description("R/W,输出%Q4.0")]
        public bool O00033;
        [Description("R/W,输出%Q4.1")]
        public bool O00034;
        [Description("R/W,输出%Q4.2")]
        public bool O00035;
        [Description("R/W,输出%Q4.3")]
        public bool O00036;
        [Description("R/W,输出%Q4.4")]
        public bool O00037;
        [Description("R/W,输出%Q4.5")]
        public bool O00038;
        [Description("R/W,输出%Q4.6")]
        public bool O00039;
        [Description("R/W,输出%Q4.7")]
        public bool O00040;
        [Description("R/W,输出%Q5.0")]
        public bool O00041;
        [Description("R/W,输出%Q5.1")]
        public bool O00042;
        [Description("R/W,输出%Q5.2")]
        public bool O00043;
        [Description("R/W,输出%Q5.3")]
        public bool O00044;
        [Description("R/W,输出%Q5.4")]
        public bool O00045;
        [Description("R/W,输出%Q5.5")]
        public bool O00046;
        [Description("R/W,输出%Q5.6")]
        public bool O00047;
        [Description("R/W,输出%Q5.7")]
        public bool O00048;
        [Description("R/W,输出%Q6.0")]
        public bool O00049;
        [Description("R/W,输出%Q6.1")]
        public bool O00050;
        [Description("R/W,输出%Q6.2")]
        public bool O00051;
        [Description("R/W,输出%Q6.3")]
        public bool O00052;
        [Description("R/W,输出%Q6.4")]
        public bool O00053;
        [Description("R/W,输出%Q6.5")]
        public bool O00054;
        [Description("R/W,输出%Q6.6")]
        public bool O00055;
        [Description("R/W,输出%Q6.7")]
        public bool O00056;
        [Description("R/W,输出%Q7.0")]
        public bool O00057;
        [Description("R/W,输出%Q7.1")]
        public bool O00058;
        [Description("R/W,输出%Q7.2")]
        public bool O00059;
        [Description("R/W,输出%Q7.3")]
        public bool O00060;
        [Description("R/W,输出%Q7.4")]
        public bool O00061;
        [Description("R/W,输出%Q7.5")]
        public bool O00062;
        [Description("R/W,输出%Q7.6")]
        public bool O00063;
        [Description("R/W,输出%Q7.7")]
        public bool O00064;
        [Description("R/W,输出%Q8.0")]
        public bool O00065;
        [Description("R/W,输出%Q8.1")]
        public bool O00066;
        [Description("R/W,输出%Q8.2")]
        public bool O00067;
        [Description("R/W,输出%Q8.3")]
        public bool O00068;
        [Description("R/W,输出%Q8.4")]
        public bool O00069;
        [Description("R/W,输出%Q8.5")]
        public bool O00070;
        [Description("R/W,输出%Q8.6")]
        public bool O00071;
        [Description("R/W,输出%Q8.7")]
        public bool O00072;
        [Description("R/W,输出%Q9.0")]
        public bool O00073;
        [Description("R/W,输出%Q9.1")]
        public bool O00074;
        [Description("R/W,输出%Q9.2")]
        public bool O00075;
        [Description("R/W,输出%Q9.3")]
        public bool O00076;
        [Description("R/W,输出%Q9.4")]
        public bool O00077;
        [Description("R/W,输出%Q9.5")]
        public bool O00078;
        [Description("R/W,输出%Q9.6")]
        public bool O00079;
        [Description("R/W,输出%Q9.7")]
        public bool O00080;
        [Description("R/W,输出%Q10.0")]
        public bool O00081;
        [Description("R/W,输出%Q10.1")]
        public bool O00082;
        [Description("R/W,输出%Q10.2")]
        public bool O00083;
        [Description("R/W,输出%Q10.3")]
        public bool O00084;
        [Description("R/W,输出%Q10.4")]
        public bool O00085;
        [Description("R/W,输出%Q10.5")]
        public bool O00086;
        [Description("R/W,输出%Q10.6")]
        public bool O00087;
        [Description("R/W,输出%Q10.7")]
        public bool O00088;
        [Description("R/W,输出%Q11.0")]
        public bool O00089;
        [Description("R/W,输出%Q11.1")]
        public bool O00090;
        [Description("R/W,输出%Q11.2")]
        public bool O00091;
        [Description("R/W,输出%Q11.3")]
        public bool O00092;
        [Description("R/W,输出%Q11.4")]
        public bool O00093;
        [Description("R/W,输出%Q11.5")]
        public bool O00094;
        [Description("R/W,输出%Q11.6")]
        public bool O00095;
        [Description("R/W,输出%Q11.7")]
        public bool O00096;
        [Description("R/W,输出%Q12.0")]
        public bool O00097;
        [Description("R/W,输出%Q12.1")]
        public bool O00098;
        [Description("R/W,输出%Q12.2")]
        public bool O00099;
        [Description("R/W,输出%Q12.3")]
        public bool O00100;
        [Description("R/W,输出%Q12.4")]
        public bool O00101;
        [Description("R/W,输出%Q12.5")]
        public bool O00102;
        [Description("R/W,输出%Q12.6")]
        public bool O00103;
        [Description("R/W,输出%Q12.7")]
        public bool O00104;
        [Description("R/W,输出%Q13.0")]
        public bool O00105;
        [Description("R/W,输出%Q13.1")]
        public bool O00106;
        [Description("R/W,输出%Q13.2")]
        public bool O00107;
        [Description("R/W,输出%Q13.3")]
        public bool O00108;
        [Description("R/W,输出%Q13.4")]
        public bool O00109;
        [Description("R/W,输出%Q13.5")]
        public bool O00110;
        [Description("R/W,输出%Q13.6")]
        public bool O00111;
        [Description("R/W,输出%Q13.7")]
        public bool O00112;
        [Description("R/W,输出%Q14.0")]
        public bool O00113;
        [Description("R/W,输出%Q14.1")]
        public bool O00114;
        [Description("R/W,输出%Q14.2")]
        public bool O00115;
        [Description("R/W,输出%Q14.3")]
        public bool O00116;
        [Description("R/W,输出%Q14.4")]
        public bool O00117;
        [Description("R/W,输出%Q14.5")]
        public bool O00118;
        [Description("R/W,输出%Q14.6")]
        public bool O00119;
        [Description("R/W,输出%Q14.7")]
        public bool O00120;
        [Description("R/W,输出%Q15.0")]
        public bool O00121;
        [Description("R/W,输出%Q15.1")]
        public bool O00122;
        [Description("R/W,输出%Q15.2")]
        public bool O00123;
        [Description("R/W,输出%Q15.3")]
        public bool O00124;
        [Description("R/W,输出%Q15.4")]
        public bool O00125;
        [Description("R/W,输出%Q15.5")]
        public bool O00126;
        [Description("R/W,输出%Q15.6")]
        public bool O00127;
        [Description("R/W,输出%Q15.7")]
        public bool O00128;
        [Description("R/W,输出%Q16.0")]
        public bool O00129;
        [Description("R/W,输出%Q16.1")]
        public bool O00130;
        [Description("R/W,输出%Q16.2")]
        public bool O00131;
        [Description("R/W,输出%Q16.3")]
        public bool O00132;
        [Description("R/W,输出%Q16.4")]
        public bool O00133;
        [Description("R/W,输出%Q16.5")]
        public bool O00134;
        [Description("R/W,输出%Q16.6")]
        public bool O00135;
        [Description("R/W,输出%Q16.7")]
        public bool O00136;
        [Description("R/W,输出%Q17.0")]
        public bool O00137;
        [Description("R/W,输出%Q17.1")]
        public bool O00138;
        [Description("R/W,输出%Q17.2")]
        public bool O00139;
        [Description("R/W,输出%Q17.3")]
        public bool O00140;
        [Description("R/W,输出%Q17.4")]
        public bool O00141;
        [Description("R/W,输出%Q17.5")]
        public bool O00142;
        [Description("R/W,输出%Q17.6")]
        public bool O00143;
        [Description("R/W,输出%Q17.7")]
        public bool O00144;
        [Description("R/W,输出%Q18.0")]
        public bool O00145;
        [Description("R/W,输出%Q18.1")]
        public bool O00146;
        [Description("R/W,输出%Q18.2")]
        public bool O00147;
        [Description("R/W,输出%Q18.3")]
        public bool O00148;
        [Description("R/W,输出%Q18.4")]
        public bool O00149;
        [Description("R/W,输出%Q18.5")]
        public bool O00150;
        [Description("R/W,输出%Q18.6")]
        public bool O00151;
        [Description("R/W,输出%Q18.7")]
        public bool O00152;
        [Description("R/W,输出%Q19.0")]
        public bool O00153;
        [Description("R/W,输出%Q19.1")]
        public bool O00154;
        [Description("R/W,输出%Q19.2")]
        public bool O00155;
        [Description("R/W,输出%Q19.3")]
        public bool O00156;
        [Description("R/W,输出%Q19.4")]
        public bool O00157;
        [Description("R/W,输出%Q19.5")]
        public bool O00158;
        [Description("R/W,输出%Q19.6")]
        public bool O00159;
        [Description("R/W,输出%Q19.7")]
        public bool O00160;
        [Description("R/W,输出%Q20.0")]
        public bool O00161;
        [Description("R/W,输出%Q20.1")]
        public bool O00162;
        [Description("R/W,输出%Q20.2")]
        public bool O00163;
        [Description("R/W,输出%Q20.3")]
        public bool O00164;
        [Description("R/W,输出%Q20.4")]
        public bool O00165;
        [Description("R/W,输出%Q20.5")]
        public bool O00166;
        [Description("R/W,输出%Q20.6")]
        public bool O00167;
        [Description("R/W,输出%Q20.7")]
        public bool O00168;
        [Description("R/W,输出%Q21.0")]
        public bool O00169;
        [Description("R/W,输出%Q21.1")]
        public bool O00170;
        [Description("R/W,输出%Q21.2")]
        public bool O00171;
        [Description("R/W,输出%Q21.3")]
        public bool O00172;
        [Description("R/W,输出%Q21.4")]
        public bool O00173;
        [Description("R/W,输出%Q21.5")]
        public bool O00174;
        [Description("R/W,输出%Q21.6")]
        public bool O00175;
        [Description("R/W,输出%Q21.7")]
        public bool O00176;
        [Description("R/W,输出%Q22.0")]
        public bool O00177;
        [Description("R/W,输出%Q22.1")]
        public bool O00178;
        [Description("R/W,输出%Q22.2")]
        public bool O00179;
        [Description("R/W,输出%Q22.3")]
        public bool O00180;
        [Description("R/W,输出%Q22.4")]
        public bool O00181;
        [Description("R/W,输出%Q22.5")]
        public bool O00182;
        [Description("R/W,输出%Q22.6")]
        public bool O00183;
        [Description("R/W,输出%Q22.7")]
        public bool O00184;


        #endregion

        public ushort ReadMaxLenth = 100;
        public ushort ReadStartAddr = 0;
        public ushort WriteStartAddr = 100;

        #region Holding Regs
        [Description("R/W,Hr40001")]
        public ushort Hr40001;
        [Description("R/W,Hr40002")]
        public ushort Hr40002;
        [Description("R/W,Hr40003")]
        public ushort Hr40003;
        [Description("R/W,Hr40004")]
        public ushort Hr40004;
        [Description("R/W,Hr40005")]
        public ushort Hr40005;
        [Description("R/W,Hr40006")]
        public ushort Hr40006;
        [Description("R/W,Hr40007")]
        public ushort Hr40007;
        [Description("R/W,Hr40008")]
        public ushort Hr40008;
        [Description("R/W,Hr40009")]
        public ushort Hr40009;
        [Description("R/W,Hr40010")]
        public ushort Hr40010;
        [Description("R/W,Hr40011")]
        public ushort Hr40011;
        [Description("R/W,Hr40012")]
        public ushort Hr40012;
        [Description("R/W,Hr40013")]
        public ushort Hr40013;
        [Description("R/W,Hr40014")]
        public ushort Hr40014;
        [Description("R/W,Hr40015")]
        public ushort Hr40015;
        [Description("R/W,Hr40016")]
        public ushort Hr40016;
        [Description("R/W,Hr40017")]
        public ushort Hr40017;
        [Description("R/W,Hr40018")]
        public ushort Hr40018;
        [Description("R/W,Hr40019")]
        public ushort Hr40019;
        [Description("R/W,Hr40020")]
        public ushort Hr40020;
        [Description("R/W,Hr40021")]
        public ushort Hr40021;
        public ushort Hr40022;
        public ushort Hr40023;
        public ushort Hr40024;
        public ushort Hr40025;
        public ushort Hr40026;
        public ushort Hr40027;
        public ushort Hr40028;
        public ushort Hr40029;
        public ushort Hr40030;
        public ushort Hr40031;
        public ushort Hr40032;
        public ushort Hr40033;
        public ushort Hr40034;
        public ushort Hr40035;
        public ushort Hr40036;
        public ushort Hr40037;
        public ushort Hr40038;
        public ushort Hr40039;
        public ushort Hr40040;
        public ushort Hr40041;
        public ushort Hr40042;
        public ushort Hr40043;
        public ushort Hr40044;
        public ushort Hr40045;
        public ushort Hr40046;
        public ushort Hr40047;
        public ushort Hr40048;
        public ushort Hr40049;
        public ushort Hr40050;
        public ushort Hr40051;
        public ushort Hr40052;
        public ushort Hr40053;
        public ushort Hr40054;
        public ushort Hr40055;
        public ushort Hr40056;
        public ushort Hr40057;
        public ushort Hr40058;
        public ushort Hr40059;
        public ushort Hr40060;
        public ushort Hr40061;
        public ushort Hr40062;
        public ushort Hr40063;
        public ushort Hr40064;
        public ushort Hr40065;
        public ushort Hr40066;
        public ushort Hr40067;
        public ushort Hr40068;
        public ushort Hr40069;
        public ushort Hr40070;
        public ushort Hr40071;
        public ushort Hr40072;
        public ushort Hr40073;
        public ushort Hr40074;
        public ushort Hr40075;
        public ushort Hr40076;
        public ushort Hr40077;
        public ushort Hr40078;
        public ushort Hr40079;
        public ushort Hr40080;
        public ushort Hr40081;
        public ushort Hr40082;
        public ushort Hr40083;
        public ushort Hr40084;
        public ushort Hr40085;
        public ushort Hr40086;
        public ushort Hr40087;
        public ushort Hr40088;
        public ushort Hr40089;
        public ushort Hr40090;
        public ushort Hr40091;
        public ushort Hr40092;
        public ushort Hr40093;
        public ushort Hr40094;
        public ushort Hr40095;
        public ushort Hr40096;
        public ushort Hr40097;
        public ushort Hr40098;
        public ushort Hr40099;
        public ushort Hr40100;
        public ushort Hr40101;
        public ushort Hr40102;
        public ushort Hr40103;
        public ushort Hr40104;
        public ushort Hr40105;
        public ushort Hr40106;
        public ushort Hr40107;
        public ushort Hr40108;
        public ushort Hr40109;
        public ushort Hr40110;
        public ushort Hr40111;
        public ushort Hr40112;
        public ushort Hr40113;
        public ushort Hr40114;
        public ushort Hr40115;
        public ushort Hr40116;
        public ushort Hr40117;
        public ushort Hr40118;
        public ushort Hr40119;
        public ushort Hr40120;
        [Description("R/W,Hr400121")]
        public ushort Hr40121;
        [Description("R/W,Hr400122")]
        public ushort Hr40122;
        [Description("R/W,Hr400123")]
        public ushort Hr40123;
        [Description("R/W,Hr400124")]
        public ushort Hr40124;
        [Description("R/W,Hr400125")]
        public ushort Hr40125;
        [Description("R/W,Hr400126")]
        public ushort Hr40126;
        [Description("R/W,Hr400127")]
        public ushort Hr40127;
        [Description("R/W,Hr400128")]
        public ushort Hr40128;
        [Description("R/W,Hr400129")]
        public ushort Hr40129;
        [Description("R/W,Hr400130")]
        public ushort Hr40130;
        public ushort Hr40131;
        public ushort Hr40132;
        public ushort Hr40133;
        public ushort Hr40134;
        public ushort Hr40135;
        public ushort Hr40136;
        public ushort Hr40137;
        public ushort Hr40138;
        public ushort Hr40139;
        public ushort Hr40140;
        public ushort Hr40141;
        public ushort Hr40142;
        public ushort Hr40143;
        public ushort Hr40144;
        public ushort Hr40145;
        public ushort Hr40146;
        public ushort Hr40147;
        public ushort Hr40148;
        public ushort Hr40149;
        public ushort Hr40150;
        public ushort Hr40151;
        public ushort Hr40152;
        public ushort Hr40153;
        public ushort Hr40154;
        public ushort Hr40155;
        public ushort Hr40156;
        public ushort Hr40157;
        public ushort Hr40158;
        public ushort Hr40159;
        public ushort Hr40160;
        public ushort Hr40161;
        public ushort Hr40162;
        public ushort Hr40163;
        public ushort Hr40164;
        public ushort Hr40165;
        public ushort Hr40166;
        public ushort Hr40167;
        public ushort Hr40168;
        public ushort Hr40169;
        public ushort Hr40170;
        public ushort Hr40171;
        public ushort Hr40172;
        public ushort Hr40173;
        public ushort Hr40174;
        public ushort Hr40175;
        public ushort Hr40176;
        public ushort Hr40177;
        public ushort Hr40178;
        public ushort Hr40179;
        public ushort Hr40180;
        public ushort Hr40181;
        public ushort Hr40182;
        public ushort Hr40183;
        public ushort Hr40184;
        public ushort Hr40185;
        public ushort Hr40186;
        public ushort Hr40187;
        public ushort Hr40188;
        public ushort Hr40189;
        public ushort Hr40190;
        public ushort Hr40191;
        public ushort Hr40192;
        public ushort Hr40193;
        public ushort Hr40194;
        public ushort Hr40195;
        public ushort Hr40196;
        public ushort Hr40197;
        public ushort Hr40198;
        public ushort Hr40199;
        public ushort Hr40200;
        public ushort Hr40201;
        public ushort Hr40202;
        public ushort Hr40203;
        public ushort Hr40204;
        public ushort Hr40205;
        public ushort Hr40206;
        public ushort Hr40207;
        public ushort Hr40208;
        public ushort Hr40209;
        public ushort Hr40210;
        public ushort Hr40211;
        public ushort Hr40212;
        public ushort Hr40213;
        public ushort Hr40214;
        public ushort Hr40215;
        public ushort Hr40216;
        public ushort Hr40217;
        public ushort Hr40218;
        public ushort Hr40219;
        public ushort Hr40220;
        public ushort Hr40221;
        public ushort Hr40222;
        public ushort Hr40223;
        public ushort Hr40224;
        public ushort Hr40225;
        public ushort Hr40226;
        public ushort Hr40227;
        public ushort Hr40228;
        public ushort Hr40229;
        public ushort Hr40230;
        public ushort Hr40231;
        public ushort Hr40232;
        public ushort Hr40233;
        public ushort Hr40234;
        public ushort Hr40235;
        public ushort Hr40236;
        public ushort Hr40237;
        public ushort Hr40238;
        public ushort Hr40239;
        public ushort Hr40240;
        public ushort Hr40241;
        public ushort Hr40242;
        public ushort Hr40243;
        public ushort Hr40244;
        public ushort Hr40245;
        public ushort Hr40246;
        public ushort Hr40247;
        public ushort Hr40248;
        public ushort Hr40249;
        public ushort Hr40250;

        #endregion

        #region Input Regs
        public ushort Ir40001;
        public ushort Ir40002;
        public ushort Ir40003;
        public ushort Ir40004;
        public ushort Ir40005;
        public ushort Ir40006;
        public ushort Ir40007;
        public ushort Ir40008;
        public ushort Ir40009;
        public ushort Ir40010;
        public ushort Ir40011;
        public ushort Ir40012;
        public ushort Ir40013;
        public ushort Ir40014;
        public ushort Ir40015;
        public ushort Ir40016;
        public ushort Ir40017;
        public ushort Ir40018;
        public ushort Ir40019;
        public ushort Ir40020;
        public ushort Ir40021;
        public ushort Ir40022;
        public ushort Ir40023;
        public ushort Ir40024;
        public ushort Ir40025;
        public ushort Ir40026;
        public ushort Ir40027;
        public ushort Ir40028;
        public ushort Ir40029;
        public ushort Ir40030;
        public ushort Ir40031;
        public ushort Ir40032;
        public ushort Ir40033;
        public ushort Ir40034;
        public ushort Ir40035;
        public ushort Ir40036;
        public ushort Ir40037;
        public ushort Ir40038;
        public ushort Ir40039;
        public ushort Ir40040;
        public ushort Ir40041;
        public ushort Ir40042;
        public ushort Ir40043;
        public ushort Ir40044;
        public ushort Ir40045;
        public ushort Ir40046;
        public ushort Ir40047;
        public ushort Ir40048;
        public ushort Ir40049;
        public ushort Ir40050;
        public ushort Ir40051;
        public ushort Ir40052;
        public ushort Ir40053;
        public ushort Ir40054;
        public ushort Ir40055;
        public ushort Ir40056;
        public ushort Ir40057;
        public ushort Ir40058;
        public ushort Ir40059;
        public ushort Ir40060;
        public ushort Ir40061;
        public ushort Ir40062;
        public ushort Ir40063;
        public ushort Ir40064;
        public ushort Ir40065;
        public ushort Ir40066;
        public ushort Ir40067;
        public ushort Ir40068;
        public ushort Ir40069;
        public ushort Ir40070;
        public ushort Ir40071;
        public ushort Ir40072;
        public ushort Ir40073;
        public ushort Ir40074;
        public ushort Ir40075;
        public ushort Ir40076;
        public ushort Ir40077;
        public ushort Ir40078;
        public ushort Ir40079;
        public ushort Ir40080;
        public ushort Ir40081;
        public ushort Ir40082;
        public ushort Ir40083;
        public ushort Ir40084;
        public ushort Ir40085;
        public ushort Ir40086;
        public ushort Ir40087;
        public ushort Ir40088;
        public ushort Ir40089;
        public ushort Ir40090;
        public ushort Ir40091;
        public ushort Ir40092;
        public ushort Ir40093;
        public ushort Ir40094;
        public ushort Ir40095;
        public ushort Ir40096;
        public ushort Ir40097;
        public ushort Ir40098;
        public ushort Ir40099;
        public ushort Ir40100;
        public ushort Ir40101;
        public ushort Ir40102;
        public ushort Ir40103;
        public ushort Ir40104;
        public ushort Ir40105;
        public ushort Ir40106;
        public ushort Ir40107;
        public ushort Ir40108;
        public ushort Ir40109;
        public ushort Ir40110;
        public ushort Ir40111;
        public ushort Ir40112;
        public ushort Ir40113;
        public ushort Ir40114;
        public ushort Ir40115;
        public ushort Ir40116;
        public ushort Ir40117;
        public ushort Ir40118;
        public ushort Ir40119;
        public ushort Ir40120;
        #endregion

        private ModbusTcpNet _sienmensModbusTcpNet;
        private Thread _daemonTh;

        public CcdTrackSiemensPlc(string name) : base(name)
        { }

        ~CcdTrackSiemensPlc()
        {
            Dispose();
        }

        public void InitRemoteIpAddress(string ipport)
        {
            //连接到siemens PLC的socket server上
            var split = ipport.Split(':');
            var ip = split[0];
            var port = Convert.ToInt32(split[1]);
            _sienmensModbusTcpNet = new ModbusTcpNet(ip, port) { ReceiveTimeOut = 250 };
        }

        public void CycleUpdate()
        {
            if (_daemonTh != null)
            {
                _daemonTh.Abort();
                _daemonTh.Join();
            }

            _daemonTh = new Thread(Daemon) { IsBackground = true };
            _daemonTh.Start();
        }

        private void Daemon()
        {
            _sienmensModbusTcpNet.ConnectServer();

            while (_daemonTh.IsAlive)
            {
                if (!_daemonTh.IsAlive)
                    break;

                Thread.Sleep(10);
                if (IsReadO)
                {
                    Read();
                }

                if (IsWriteO)
                {
                    Thread.Sleep(10);
                    Write();
                }

                //if (ReadMaxLenth == 0)
                //    continue;
                Thread.Sleep(10);
                var readHoldingRegs = _sienmensModbusTcpNet.Read(ReadStartAddr.ToString(),
                    ReadMaxLenth > 100 ? (ushort)100 : ReadMaxLenth);
                if (!readHoldingRegs.IsSuccess)
                    continue;
                var ushortList = new List<ushort>();
                for (var i = 0; i < readHoldingRegs.Content.Length; i = i + 2)
                {
                    var bs = new[] { readHoldingRegs.Content[i], readHoldingRegs.Content[i + 1] };
                    Array.Reverse(bs);
                    var ushortR = BitConverter.ToUInt16(bs, 0);
                    ushortList.Add(ushortR);
                }

                for (var i = 0; i < ushortList.Count; i++)
                {
                    var addr = string.Format("Hr{0}", 40000 + ReadStartAddr + i + 1);
                    GetType().GetField(addr).SetValue(this, ushortList[i]);
                }

                //if (ReadMaxLenth >= 120)
                //    continue;
                {
                    var startAddr = WriteStartAddr;

                    var list = new List<ushort>();

                    for (var i = startAddr; i < startAddr + ReadMaxLenth; i++)
                    {
                        var str = string.Format("Hr{0}", 40000 + i + 1);
                        var f = GetType().GetField(str).GetValue(this);
                        list.Add((ushort)f);
                    }

                    Thread.Sleep(10);
                    _sienmensModbusTcpNet.Write(startAddr.ToString(), list.ToArray());
                }
            }
        }

        public void ReadCurrentStatus()
        {
            Read();
        }

        private void Read()
        {
            while (true)
            {
                #region Read

                //foreach (
                //    var p in
                //        from p in GetType().GetFields()
                //        let name = p.Name
                //        where name.StartsWith("I1") && name.Length == 6
                //        select p)
                //{
                //    p.SetValue(this, null);
                //}

                // mapping
                //Thread.Sleep(120);
                var r1Content = new bool[200];
                while (true)
                {
                    var r1 = _sienmensModbusTcpNet.ReadDiscrete("0", (ushort)r1Content.Length); // I10001
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
                    if (addr > r1Content.Length)
                        continue;

                    p.SetValue(this, r1Content[addr - 1]);
                    OnPushValue(Name, name, r1Content[addr - 1].ToString());
                }

                return;

                #endregion
            }
        }

        private void Write()
        {
            var bools = new bool[200];

            foreach (var p in GetType().GetFields())
            {
                var name = p.Name;

                if (!name.StartsWith("O0") || name.Length != 6)
                    continue;

                var addr = int.Parse(name.Substring("O".Length).Substring(1));
                if (addr > bools.Length)
                    continue;

                var value = Convert.ToString(p.GetValue(this));
                bools[addr - 1] = bool.Parse(value);
                OnPushValue(Name, name, value);
            }

            while (true)
            {
                var w1 = _sienmensModbusTcpNet.WriteCoil("0", bools); // O00001

                if (!w1.IsSuccess)
                    continue;

                break;
            }
        }

        public void RefreshOutputs()
        {
            while (true)
            {
                #region Read

                // mapping
                //Thread.Sleep(120);
                var r1Content = new bool[150];
                while (true)
                {
                    var r1 = _sienmensModbusTcpNet.ReadCoil("0", 150); // I10001
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
                    if (addr <= r1Content.Length)
                    {
                        p.SetValue(this, r1Content[addr - 1]);
                        OnPushValue(Name, name, r1Content[addr - 1].ToString());
                    }
                }

                return;

                #endregion
            }
        }

        public void ResetOutputs()
        {
            foreach (
                var p in
                    from p in GetType().GetFields()
                    let name = p.Name
                    where name.StartsWith("O0") && name.Length == 6
                    select p)
            {
                p.SetValue(this, false);
            }
        }

        public void ReadRegs()
        {
            while (true)
            {
                if (ReadMaxLenth == 0)
                    break;
                var readHoldingRegs = _sienmensModbusTcpNet.Read(0.ToString(), ReadMaxLenth);
                if (!readHoldingRegs.IsSuccess)
                    continue;
                var ushortList = new List<ushort>();
                for (var i = 0; i < readHoldingRegs.Content.Length; i = i + 2)
                {
                    var bs = new[] { readHoldingRegs.Content[i], readHoldingRegs.Content[i + 1] };
                    Array.Reverse(bs);
                    var ushortR = BitConverter.ToUInt16(bs, 0);
                    ushortList.Add(ushortR);
                }

                for (var i = 0; i < ushortList.Count; i++)
                {
                    var addr = string.Format("Hr{0}", 40000 + i + 1);
                    GetType().GetField(addr).SetValue(this, ushortList[i]);
                }

                break;
            }
        }

        private static void OnPushValue(
            string controllername, string fieldname, string value)
        {
            var handler = PushValue;
            if (handler != null) handler(controllername, fieldname, value);
        }

        public string GetAsciiStr = string.Empty;

        public void GetAsciiFromUhort(string start, string len, string format)
        {
            try
            {
                GetAsciiStr = string.Empty;
                var startIndex = int.Parse(start);

                var bs = new List<byte>();
                for (var i = startIndex; i < startIndex + int.Parse(len); i++)
                {
                    var fieldName = string.Format("Hr{0}", i);

                    var fieldValie = (ushort)GetType().GetField(fieldName).GetValue(this);
                    bs.AddRange(BitConverter.GetBytes(fieldValie).Reverse());
                }

                var temp = Encoding.ASCII.GetString(bs.ToArray());

                var sp = format.Split(':');
                var asciiKey = sp[0];
                var asciiKeyIndex = Convert.ToInt32(sp[1]);
                var asciiLen = Convert.ToInt32(sp[2]);

                if (string.IsNullOrEmpty(temp))
                    return;

                var findKeyIndex = temp.LastIndexOf(asciiKey, StringComparison.Ordinal);
                if (findKeyIndex == -1)
                    return;
                GetAsciiStr = temp.Substring(findKeyIndex - asciiKeyIndex, asciiLen);
            }
            catch (Exception)
            {
                GetAsciiStr = string.Empty;
            }
        }
    }
}
