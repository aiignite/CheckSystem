using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using MT6501MODULELib;

namespace Controller
{
    public sealed class Pb006Mt6501 : ControllerBase
    {
        //public int ChipIndex = 0;
        private int Boffset = 0;
        private IMT6501Device _mt;
        private MT6501Advanced _advanced;
        private MT6501Solver _solver;

        [Description("R,烧写器连接结果")]
        public string ConnectResult = string.Empty;

        [Description("R,VDD")]
        public float VddVolt;

        [Description("R,DAC_calbiation")]
        public string DacCalbiation = string.Empty;

        [Description("R,ERRROM写入结果")]
        public string ProgramReuslt = string.Empty;

        [Description("R,读取角度值")]
        public float AngleRead;

        [Description("R,测量芯片模拟输出百分比")]
        public float MeasPercentVdd;

        [Description("R,测量芯片模拟输出电压")]
        public float MeasVolt;

        public Pb006Mt6501(string name)
            : base(name) { }

        ~Pb006Mt6501()
        {
            Destroy();
            Dispose();
        }

        /// <summary>
        /// 连接烧录器
        /// </summary>
        /// <param name="comNo">串口ID</param>
        /// <returns></returns>
        [Description("连接烧录器")]
        public int Connect(int comNo)
        {
            ConnectResult = string.Empty;

            var res = -1;

            try
            {
                _mt = new MT6501Device();
                _advanced = _mt.Advanced;
                _solver = _mt.Solver;

                Thread.Sleep(500);
                res = _mt.ConnectChannel(comNo);
                if (res != 0)
                {
                    ConnectResult = "NG";
                    return res;
                }
                ConnectResult = "OK";
            }
            catch (Exception ex)
            {
                ConnectResult = "NG " + ex.Message;
                return res;
            }
            return res;
        }

        [Description("释放烧录器")]
        public void Destroy()
        {
            if (_mt == null)
                return;
            try
            {
                PownerOff(0);
                PownerOff(1);
                _mt.Destroy(true);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// 上电
        /// </summary>
        /// <param name="chipIndex"></param>
        /// <returns></returns>
        [Description("上电")]
        public int PownerOn(int chipIndex)
        {
            var res = _mt.Customized_PowerOn(chipIndex);
            if (res != 0)
                return res;
            Thread.Sleep(20);

            res = _mt.Customized_Mode0V(chipIndex);
            if (res != 0)
                return res;
            Thread.Sleep(20);

            //res = _mt.Customized_EnterOWI(chipIndex);
            //if (res != 0)
            //    return res;
            //Thread.Sleep(20);

            return res;
        }

        [Description("获取VDD")]
        public void GetVdd(int chipIndex)
        {
            VddVolt = -9999;
            VddVolt = _mt.MeasureVdd(chipIndex);
        }

        [Description("进入OWI")]
        public void EnterOwi(int chipIndex)
        {
            var res = _mt.Customized_EnterOWI(chipIndex);
            Thread.Sleep(20);
        }

        [Description("退出OWI")]
        public void QuitOwi(int chipIndex)
        {
            var res = _mt.Customized_QuitOWI(chipIndex);
            Thread.Sleep(20);
        }

        /// <summary>
        /// 断电
        /// </summary>
        /// <param name="chinIndex"></param>
        /// <returns></returns>
        [Description("断电")]
        public int PownerOff(int chinIndex)
        {
            var res = _mt.Customized_PowerOff(chinIndex);
            if (res != 0)
                return res;
            Thread.Sleep(20);

            return res;
        }

        [Description("设置Point")]
        public bool SetPoint(int chipIndex, string codeValue)
        {
            SolverSettingCodes code;

            if (codeValue == "DP")
                code = SolverSettingCodes
                    .SolverSettingDP;
            else if (!Enum.TryParse("SolverTargetAngle" + codeValue.ToUpper(), out code))
                return false;

            var angle = GetAng(chipIndex);

            var res = _advanced.SetSolverSetting(chipIndex, code, angle, Boffset);
            if (res != 0)
                return false;
            return res == 0;
        }

        [Description("设置DpPoint")]
        public bool SetDpPoint(int chipIndex, string angleOffset)
        {
            const SolverSettingCodes code = SolverSettingCodes.SolverSettingDP;

            float offset;
            if (!float.TryParse(angleOffset, out offset))
                return false;
            var getAngle = GetAng(chipIndex);
            if (getAngle + offset >= 0 && getAngle + offset <= 360)
            {
                AngleRead = AngleRead + offset;

                var res = _advanced.SetSolverSetting(chipIndex, code, AngleRead, Boffset);
                if (res != 0)
                    return false;
                return res == 0;
            }
            else
            {
                var res = _advanced.SetSolverSetting(chipIndex, code, AngleRead, Boffset);
                if (res != 0)
                    return false;
                return res == 0;
            }
        }

        public bool SetPoint(int chipIndex, string codeValue, float value)
        {
            SolverSettingCodes code;

            if (codeValue == "DP")
                code = SolverSettingCodes
                    .SolverSettingDP;
            else if (!Enum.TryParse("SolverTargetAngle" + codeValue.ToUpper(), out code))
                return false;

            //var angle = GetAng(chipIndex);

            var res = _advanced.SetSolverSetting(chipIndex, code, value, Boffset);
            if (res != 0)
                return false;
            return res == 0;
        }

        [Description("设置Level")]
        public bool SetLevel(int chinIndex, string codeValue, float vddPercent)
        {
            SolverSettingCodes code;

            if (!Enum.TryParse("SolverTargetLevelPoint" + codeValue.ToUpper(), out code))
                return false;

            var res = _advanced.SetSolverSetting(chinIndex, code, vddPercent, Boffset);
            if (res != 0)
                return false;
            return res == 0;
        }

        [Description("获取当前角度")]
        public float GetAng(int chipIndex)
        {
            AngleRead = -9999;

            AngleRead = (float)Math.Round(_mt.GetAngle(chipIndex), 2, MidpointRounding.AwayFromZero);//_mt.GetAngle(chipIndex);
            return AngleRead;
        }

        /// <summary>
        /// 该方法测量芯片模拟输出
        /// </summary>
        /// <returns>测量输出类型为电压的百分比</returns>
        [Description("测量芯片模拟输出百分比")]
        public float MesurePercentVdd(int chipIndex)
        {
            MeasPercentVdd = -9999;
            // MeasPercentVDD = 测量输出类型为电压的百分比
            // MeasVolt = 测量输出类型为电压值
            MeasPercentVdd = (float)Math.Round(_mt.Measure(chipIndex, MeasureTypes.MeasPercentVDD), 2, MidpointRounding.AwayFromZero);
            return MeasPercentVdd;
        }

        /// <summary>
        /// 该方法测量芯片模拟输出
        /// </summary>
        /// <returns>测量输出类型为电压值</returns>
        [Description("测量芯片模拟输出电压值")]
        public float MesureMeasVolt(int chipIndex)
        {
            MeasVolt = -9999;
            // MeasPercentVDD = 测量输出类型为电压的百分比
            // MeasVolt = 测量输出类型为电压值
            MeasVolt = (float)Math.Round(_mt.Measure(chipIndex, MeasureTypes.MeasVolt), 2, MidpointRounding.AwayFromZero);
            return MeasVolt;
        }

        /// <summary>
        /// 该方法清空软件缓冲区及芯片RAM里的solver参数
        /// 如果该芯片已经做过角度编程
        /// 该操作在新的一次编程之前是必要的
        /// </summary>
        /// <returns></returns>
        [Description("清空软件缓冲区及芯片RAM里的solver参数")]
        public void NewDevice(int chipIndex)
        {
            DacCalbiation = string.Empty;

            _solver.ClearSolverSettings(chipIndex);
            _solver.CopySolverSettingsToParameters(chipIndex);

            var read0X04 = _advanced.MemRead(chipIndex, 0x04);
            var bits = Convert.ToString(read0X04, 2).PadLeft(32, '0').ToCharArray().Reverse().ToList();
            if (bits[5].ToString() != 0.ToString())
            {
                read0X04 = read0X04 & 0xC0;
                _advanced.MemWrite(chipIndex, 0x04, read0X04);
            }

            var read0X15 = _advanced.MemRead(chipIndex, 0x15);
            if (read0X15 != 0x80)
            {
                _advanced.MemWrite(chipIndex, 0x15, 0x80);
            }

            var read0X16 = _advanced.MemRead(chipIndex, 0x16);
            if (read0X16 != 0x1C)
            {
                _advanced.MemWrite(chipIndex, 0x16, 0x1C);
            }

            _mt.ProgramDevice(chipIndex);
            Thread.Sleep(250);

            float sysOsK0;
            float sysOsB0;
            float sysOsK1;
            float sysOsB1;
            DacCalbiation =
                _mt.DAC_calibration(chipIndex, out sysOsK0, out sysOsB0, out sysOsK1, out sysOsB1).ToString();

            if (DacCalbiation!="0")
            {
                Debug.WriteLine(DacCalbiation+"  "+ string.Format("K0:{0},B0:{1},K1:{2},B1:{3}", sysOsK0, sysOsB0, sysOsK1, sysOsB1));
            }
            else
            {
                Debug.WriteLine(DacCalbiation + "  " + string.Format("K0:{0},B0:{1},K1:{2},B1:{3}", sysOsK0, sysOsB0, sysOsK1, sysOsB1));

            }
        }

        [Description("设置钳位电压")]
        public int SetClamp(int chipIndex, float clmapLow, float clampHigh)
        {
            var res = _advanced.SetSolverSetting(
                chipIndex, SolverSettingCodes.SolverSettingClampingLow, clmapLow, Boffset);
            if (res != 0)
                return res;
            res = _advanced.SetSolverSetting(chipIndex, SolverSettingCodes.SolverSettingClampingHigh, clampHigh, Boffset);
            //_mt.Customized_Mode0V(chipIndex);

            return res;
        }

        /// <summary>
        /// 该方法将指定的芯片设置为测量模式。
        /// </summary>
        /// <param name="chipIndex"></param>
        [Description("设置为测量模式")]
        public void SetAdcMode(int chipIndex)
        {
            _mt.Customized_SetADCMode(chipIndex);
        }

        [Description("EPPROM写入")]
        public void Program(int chipIndex)
        {
            ProgramReuslt = string.Empty;
            _solver.CopySolverSettingsToParameters(chipIndex);
            if (_mt.ProgramDevice(chipIndex) != 0)
            {
                ProgramReuslt = "NG";
                return;
            }
            //ProgramReuslt = _mt.ProgramDevice(chipIndex) == 0 ? "OK" : "NG";

            PownerOff(chipIndex);
            Thread.Sleep(2000);
            PownerOn(chipIndex);
            EnterOwi(chipIndex);

            var read0X04 = _advanced.MemRead(chipIndex, 0x04);
            var bits = Convert.ToString(read0X04, 2).PadLeft(32, '0').ToCharArray().Reverse().ToList();
            if (bits[5].ToString() == 0.ToString())
            {
                var read0X15 = _advanced.MemRead(chipIndex, 0x15);
                if (read0X15 == 0x80)
                {
                    var read0X16 = _advanced.MemRead(chipIndex, 0x16);
                    if (read0X16 == 0x1C)
                    {
                        ProgramReuslt = "OK";
                        return;
                    }
                }
            }

            ProgramReuslt = "NG";

            //_mt.Customized_QuitOWI(chipIndex);
            //return _mt.MeasureVdd(chipIndex);
        }

        public string MeasPercentVddSerials;
        public string MeasVoltSerials;

        /// <summary>
        /// 该方法测量芯片模拟输出
        /// </summary>
        /// <returns>测量输出类型为电压的百分比</returns>
        public string MesurePercentVddSerials(int chipIndex)
        {

            // MeasPercentVDD = 测量输出类型为电压的百分比
            // MeasVolt = 测量输出类型为电压值
            var measPercentVdd = _mt.Measure(chipIndex, MeasureTypes.MeasPercentVDD);

            if (!string.IsNullOrEmpty(MeasPercentVddSerials))
                MeasPercentVddSerials +=
                    Math.Round(measPercentVdd, 2, MidpointRounding.AwayFromZero).ToString(CultureInfo.InvariantCulture);
            else
                MeasPercentVddSerials += "," +
                                         Math.Round(measPercentVdd, 2, MidpointRounding.AwayFromZero)
                                             .ToString(CultureInfo.InvariantCulture);

            return MeasPercentVddSerials;
        }

        /// <summary>
        /// 该方法测量芯片模拟输出
        /// </summary>
        /// <returns>测量输出类型为电压值</returns>
        public string MesureMeasVoltSerials(int chipIndex)
        {
            //var x = -45f;
            //for (var i = 0; i < 9; i++)
            //{
            //    MeasVoltSerials += (x * -0.045 + 11.3).ToString(CultureInfo.InvariantCulture) + ",";
            //    x = x + 10;
            //}

            //for (var i = 0; i < 9; i++)
            //{
            //    MeasVoltSerials += 4.5f.ToString(CultureInfo.InvariantCulture) + ",";
            //}

            //x = 135f;
            //for (var i = 0; i < 9; i++)
            //{
            //    MeasVoltSerials += (x * -0.045 + 10.575).ToString(CultureInfo.InvariantCulture) + ",";
            //    x = x + 10;
            //}

            //for (var i = 0; i < 9; i++)
            //{
            //    MeasVoltSerials += 0.5f.ToString(CultureInfo.InvariantCulture) + ",";
            //}

            //MeasVoltSerials = MeasVoltSerials.TrimEnd(',');

            //MeasVoltSerials =
            //    "1.382294,0.9634399,0.5172729,0.5062866,0.5062866,0.5062866,0.5062866,0.5064392,0.5064392,0.5062866,0.5062866,0.5062866,0.9576416,1.400604,1.831665,2.271423,2.725983,3.200989,3.635254,4.071198,4.498138,4.497986,4.497986,4.497986,4.497986,4.497986,4.498138,4.497986,4.497986,4.471283,4.001923,3.565674,3.153992,2.712402,2.264252,1.80954";

            //return "";

            // MeasPercentVDD = 测量输出类型为电压的百分比
            // MeasVolt = 测量输出类型为电压值
            var measVolt = _mt.Measure(chipIndex, MeasureTypes.MeasVolt);
            if (string.IsNullOrEmpty(MeasVoltSerials))
                MeasVoltSerials += Math.Round(measVolt, 2, MidpointRounding.AwayFromZero).ToString(CultureInfo.InvariantCulture);
            else
                MeasVoltSerials += "," + Math.Round(measVolt, 2, MidpointRounding.AwayFromZero).ToString(CultureInfo.InvariantCulture);

            return MeasVoltSerials;
        }

        public void ClearMesurePercentVddSerials()
        {
            MeasPercentVddSerials = string.Empty;
        }

        public void ClearMesureMeasVoltSerials()
        {
            MeasVoltSerials = string.Empty;
        }

        public void ReadMerory(int chipIndex)
        {

        }
    }
}
