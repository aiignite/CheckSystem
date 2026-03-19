using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;

namespace Controller
{
    [Description("CAN-Product,VX1前灯高配")]
    public sealed class Vx1LedDriver : ControllerBase
    {
        public CanBus MainCan;

        private readonly Thread _mainWork;
        private readonly Thread _keepMlcThread;
        private bool _isSleep = true;
        private readonly byte[] _bytes0X50 = new byte[8];
        private readonly byte[] _bytes0X60 = new byte[8];

        public Vx1LedDriver(string name) :
            base(name)
        {
            _mainWork =
                new Thread(MainWork) { IsBackground = true };
            _mainWork.Start();

            _keepMlcThread =
                new Thread(KeepMlcTimer) { IsBackground = true };
            _keepMlcThread.Start();
        }

        ~Vx1LedDriver()
        {
            Dispose();
        }

        #region 点灯相关
        private void MainWork()
        {
            while (_mainWork.IsAlive)
            {
                if (!_mainWork.IsAlive)
                    break;

                Thread.Sleep(50);

                if (MainCan == null)
                    continue;

                if (_isSleep)
                    continue;

                MainCan.SendStandardCanData(0x50, _bytes0X50);
                MainCan.SendStandardCanData(0x60, _bytes0X60);
            }
        }

        [Description("模块唤醒")]
        public void Awake()
        {
            _isSleep = false;
        }

        [Description("模块休眠")]
        public void Sleep()
        {
            _isSleep = true;
        }

        [Description("打开AHB")]
        public void AHbOn()
        {
            _bytes0X50[0] = 0x64;
            _bytes0X60[0] = 0x64;
        }

        [Description("关闭AHB")]
        public void AHbOff()
        {
            _bytes0X50[0] = 0x00;
            _bytes0X60[0] = 0x00;
        }

        [Description("打开ALB1")]
        public void ALb1On()
        {
            _bytes0X50[1] = 0x64;
            _bytes0X60[1] = 0x64;
        }

        [Description("关闭ALB1")]
        public void ALb1Off()
        {
            _bytes0X50[1] = 0x00;
            _bytes0X60[1] = 0x00;
        }

        [Description("打开ALB2")]
        public void ALb2On()
        {
            _bytes0X50[2] = 0x64;
            _bytes0X60[2] = 0x64;
        }

        [Description("关闭ALB2")]
        public void ALb2Off()
        {
            _bytes0X50[2] = 0x00;
            _bytes0X60[2] = 0x00;
        }

        [Description("打开ALB3")]
        public void ALb3On()
        {
            _bytes0X50[3] = 0x64;
            _bytes0X60[3] = 0x64;
        }

        [Description("关闭ALB3")]
        public void ALb3Off()
        {
            _bytes0X50[3] = 0x00;
            _bytes0X60[3] = 0x00;
        }

        [Description("打开ALB4")]
        public void ALb4On()
        {
            _bytes0X50[4] = 0x64;
            _bytes0X60[4] = 0x64;
        }

        [Description("关闭ALB4")]
        public void ALb4Off()
        {
            _bytes0X50[4] = 0x00;
            _bytes0X60[4] = 0x00;
        }

        [Description("打开左灯AHB")]
        public void LeftAHbOn()
        {
            _bytes0X50[0] = 0x64;
        }

        [Description("打开左灯ALB1")]
        public void LeftALb1On()
        {
            _bytes0X50[1] = 0x64;
        }

        [Description("打开左灯ALB2")]
        public void LeftALb2On()
        {
            _bytes0X50[2] = 0x64;
        }

        [Description("打开左灯ALB3")]
        public void LeftALb3On()
        {
            _bytes0X50[3] = 0x64;
        }

        [Description("打开左灯ALB4")]
        public void LeftALb4On()
        {
            _bytes0X50[4] = 0x64;
        }

        [Description("关闭左灯AHB")]
        public void LeftAHbOff()
        {
            _bytes0X50[0] = 0x00;
        }

        [Description("关闭左灯ALB1")]
        public void LeftALb1Off()
        {
            _bytes0X50[1] = 0x00;
        }

        [Description("关闭左灯ALB2")]
        public void LeftALb2Off()
        {
            _bytes0X50[2] = 0x00;
        }

        [Description("关闭左灯ALB3")]
        public void LeftALb3Off()
        {
            _bytes0X50[3] = 0x00;
        }

        [Description("关闭左灯ALB4")]
        public void LeftALb4Off()
        {
            _bytes0X50[4] = 0x00;
        }

        [Description("打开右灯AHB")]
        public void RightAHbOn()
        {
            _bytes0X60[0] = 0x64;
        }

        [Description("打开右灯ALB1")]
        public void RightALb1On()
        {
            _bytes0X60[1] = 0x64;
        }

        [Description("打开右灯ALB2")]
        public void RightALb2On()
        {
            _bytes0X60[2] = 0x64;
        }

        [Description("打开右灯ALB3")]
        public void RightALb3On()
        {
            _bytes0X60[3] = 0x64;
        }

        [Description("打开右灯ALB4")]
        public void RightALb4On()
        {
            _bytes0X60[4] = 0x64;
        }

        [Description("关闭右灯AHB")]
        public void RightAHbOff()
        {
            _bytes0X60[0] = 0x00;
        }

        [Description("关闭右灯ALB1")]
        public void RightALb1Off()
        {
            _bytes0X60[1] = 0x00;
        }

        [Description("关闭右灯ALB2")]
        public void RightALb2Off()
        {
            _bytes0X60[2] = 0x00;
        }

        [Description("关闭右灯ALB3")]
        public void RightALb3Off()
        {
            _bytes0X60[3] = 0x00;
        }

        [Description("关闭右灯ALB4")]
        public void RightALb4Off()
        {
            _bytes0X60[4] = 0x00;
        }
        #endregion

        #region 模块诊断相关
        public GatewayCan SubCan;

        public string DownloadResultStr = string.Empty;
        public string PartNumberStr = string.Empty;
        public string SerialNumerStr = string.Empty;
        public string FblPartNoStr = string.Empty;
        public string FblVersionStr = string.Empty;
        public string AppPartNoStr = string.Empty;
        public string AppVersionStr = string.Empty;
        public string ConfigPartNoStr = string.Empty;
        public string ConfigVersionStr = string.Empty;

        public double Ntc1ReadDouble;
        public double Ntc2ReadDouble;
        public double Ntc3ReadDouble;
        public double Ntc4ReadDouble;

        private void KeepMlcTimer()
        {
            while (_keepMlcThread.IsAlive)
            {
                if (!_keepMlcThread.IsAlive)
                    break;

                Thread.Sleep(100);

                try
                {
                    if (SubCan == null)
                        return;

                    var mlcData = new CanBus.CanDataPackage(
                        0x7E3, CanBus.CanProtocol.Can,
                        CanBus.CanType.Standard,
                        CanBus.CanFormat.Data,
                        new byte[] { 0x00, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80 });

                    SubCan.SendStandardCanData(mlcData.CanId, mlcData.CanData);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        public string DrvFlashFilePath = string.Empty;
        public string AppFilePath = string.Empty;
        public string CaliFailPath = string.Empty;

        private int _periodCount;
        private bool _isLeftNode = true;
        private bool _isSleeping = true;
        private bool _isInExtendedSession;

        private readonly CanCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X300 =
            new CanCommunicationMatrix.MotorolaMatrix(0x300, 8);

        public void ChangeToLeftNode()
        {
            _isLeftNode = true;
            AddCanNotFilter();
        }

        public void ChangeToRightNode()
        {
            _isLeftNode = false;
            AddCanNotFilter();
        }

        private void AddCanNotFilter()
        {
            if (MainCan == null)
                return;
            MainCan.AddDoNotFilterCanId(0x1F7);
            MainCan.AddDoNotFilterCanId(0x1F8);
        }

        public void Download()
        {
            //DownloadResultStr = "NG";

            //if (MainCan == null)
            //    return;

            //var st = new Stopwatch();
            //st.Start();

            //if (MainCan.CanBusWithIso14229.TryDataTransferFromFile(
            //        GetCurrentSeesionCanId(), GetCurrentSeesionRecvCanId(), GatewayCan.CanType.Standard, DrvFlashFilePath,
            //        new[] { AppFilePath, CaliFailPath }, 0x11, 0x12,
            //        para => Uds14229Operater.CommonGenerateKey(para).ToArray()))
            //    DownloadResultStr = "OK";

            //st.Stop();

            //DownloadResultStr += " 耗时：" + st.ElapsedMilliseconds * 0.001 + "s";
        }

        public void SetupTestMode()
        {
            if (MainCan == null)
                return;

            MainCan.CanBusWithUds.TryWriteData(
                GetCurrentSeesionCanId(),
                GetCurrentSeesionRecvCanId(),
                CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xD0, 0x2A, new byte[] { 0xFF });
        }

        public bool Led1Control;
        public bool Led2Control;
        public bool Slave1Control;

        public void InputOutPutControl()
        {
            var b1Str = string.Format(
                "000000{0}{1}",
                Led2Control ? "1" : "0",
                Led1Control ? "1" : "0");

            var b2Str = string.Format(
                "000000{0}0",
                Slave1Control ? "1" : "0");

            var b1 = Convert.ToByte(b1Str, 2);
            var b2 = Convert.ToByte(b2Str, 2);

            if (MainCan == null)
                return;

            var bs = new[] { b1, b2 };
            if (MainCan.CanBusWithUds.TryInputOutputControl(
                GetCurrentSeesionCanId(),
                GetCurrentSeesionRecvCanId(),
                CanBus.CanType.Standard,
                0x50, 0x94,
                Uds14229Helper.InputOutputControlParameter.ShortTermAdjustment,
                bs.ToList()))
                return;

            Thread.Sleep(500);
            MainCan.CanBusWithUds.TryInputOutputControl(
                GetCurrentSeesionCanId(),
                GetCurrentSeesionRecvCanId(),
                CanBus.CanType.Standard,
                0x50, 0x94,
                Uds14229Helper.InputOutputControlParameter.ShortTermAdjustment,
                bs.ToList());
        }

        private uint GetCurrentSeesionCanId()
        {
            return _isLeftNode ? (uint)0x1F6 : 0x1F8;
        }

        private uint GetCurrentSeesionRecvCanId()
        {
            return _isLeftNode ? (uint)0x1F7 : 0x1F9;
        }

        /// <summary>
        /// 读NTC
        /// </summary>
        /// <param name="index"></param>
        public void ReadNtc(string index)
        {
            byte didHi = 0x00;
            byte didLo = 0x00;

            switch (int.Parse(index))
            {
                case 1:
                    didHi = 0xD0;
                    didLo = 0x20;
                    Ntc1ReadDouble = -9999;
                    break;

                case 2:
                    didHi = 0xD0;
                    didLo = 0x21;
                    Ntc2ReadDouble = -9999;
                    break;

                case 3:
                    didHi = 0xD0;
                    didLo = 0x22;
                    Ntc3ReadDouble = -9999;
                    break;

                case 4:
                    didHi = 0xD0;
                    didLo = 0x23;
                    Ntc4ReadDouble = -9999;
                    break;
            }

            if (MainCan == null)
                return;

            var readFunc = new Func<byte[]>(() =>
            {
                byte[] readNtc;
                MainCan.CanBusWithUds.TryReadData(
                    GetCurrentSeesionCanId(),
                    GetCurrentSeesionRecvCanId(),
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    didHi, didLo,
                    out readNtc);
                return readNtc;
            });

            var result = readFunc.Invoke() ?? readFunc.Invoke();

            if (result == null)
                return;

            var ntc = result[0] * 256 + result[1];
            switch (int.Parse(index))
            {
                case 1:
                    Ntc1ReadDouble = ntc / 1024.0 * 5.0;
                    break;

                case 2:
                    Ntc2ReadDouble = ntc / 1024.0 * 5.0;
                    break;

                case 3:
                    Ntc3ReadDouble = ntc / 1024.0 * 5.0;
                    break;

                case 4:
                    Ntc4ReadDouble = ntc / 1024.0 * 5.0;
                    break;
            }
        }

        /// <summary>
        /// 写总成零件号
        /// </summary>
        /// <param name="partNumber"></param>
        public void WritePartNo(string partNumber)
        {
            if (MainCan == null)
                return;

            if (string.IsNullOrEmpty(partNumber))
                return;

            var bs = Encoding.ASCII.GetBytes(partNumber);

            var writeFunc = new Func<bool>(
                () =>
                    MainCan.CanBusWithUds.TryWriteData(
                        GetCurrentSeesionCanId(),
                        GetCurrentSeesionRecvCanId(),
                        CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xD0, 0x0F, bs.ToArray()));

            if (writeFunc.Invoke())
                return;
            Thread.Sleep(500);
            writeFunc.Invoke();
        }

        /// <summary>
        /// 读总成零件号
        /// </summary>
        public void ReadPartNo()
        {
            PartNumberStr = string.Empty;

            if (MainCan == null)
                return;

            var readFunc = new Func<byte[]>(() =>
            {
                byte[] readPartNo;
                MainCan.CanBusWithUds.TryReadData(
                    GetCurrentSeesionCanId(),
                    GetCurrentSeesionRecvCanId(),
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    0xD0, 0x0F,
                    out readPartNo);
                return readPartNo;
            });

            var result = readFunc.Invoke() ?? readFunc.Invoke();

            if (result == null)
                return;

            foreach (var item in result.Where(item => item >= 0x30 && item <= 0x7A))
                PartNumberStr += Encoding.ASCII.GetString(new[] { item });
        }

        /// <summary>
        /// 写生产系列号
        /// </summary>
        /// <param name="productNoandPartNo"></param>
        public void WriteSerialNo(string productNoandPartNo)
        {
            if (MainCan == null)
                return;

            if (string.IsNullOrEmpty(productNoandPartNo))
                return;

            if (productNoandPartNo.Split(':').Length != 2)
                return;

            var partNo = productNoandPartNo.Split(':')[1];

            if (partNo.Length != 6)
                return;

            var productNo = productNoandPartNo.Split(':')[0];
            var serialNo = string.Empty;
            var date = string.Empty;

            using (var conn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                try
                {
                    var cmd = new SqlCommand("GetCheckNo", conn) { CommandType = CommandType.StoredProcedure };
                    cmd.Parameters.AddWithValue("@Productno", productNo);  //给输入参数赋值
                    //cmd.Parameters.AddWithValue("@checkStaff", "admin");  //给输入参数赋值
                    //cmd.Parameters.AddWithValue("@creater", "admin");  //给输入参数赋值
                    //var parOutputSerialNo = cmd.Parameters.Add("@serialNumber", SqlDbType.Int);  //定义输出参数 
                    //parOutputSerialNo.Direction = ParameterDirection.Output;  //参数类型为Output  
                    var parOutputDate = cmd.Parameters.Add("@date", SqlDbType.DateTime);  //定义输出参数 
                    parOutputDate.Direction = ParameterDirection.Output;  //参数类型为Output 
                    var returnSerialNo = cmd.Parameters.Add("@return_value", SqlDbType.Int);
                    returnSerialNo.Direction = ParameterDirection.ReturnValue;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    serialNo = returnSerialNo.Value.ToString();
                    date = parOutputDate.Value.ToString();
                    //serialNp = parOutputSerialNo.Value.ToString();
                    //date = parOutputDate.Value.ToString();
                    //MessageBox.Show(parOutputSerialNo.Value.ToString());   //显示输出参数的值  
                    //MessageBox.Show(parOutputDate.Value.ToString());  //显示返回值  
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            var year = DateTime.Now.Year;
            var yearStr = string.Empty;
            if (year == 2020)
            {
                yearStr = "W";
            }
            else if (year == 2021)
            {
                yearStr = "X";
            }
            else if (year == 2022)
            {
                yearStr = "Y";
            }
            else if (year == 2023)
            {
                yearStr = "Z";
            }


            var str = string.Format("{0}{1}{2}{3}{4}{5}", partNo, "A", "1", yearStr,
                DateTime.Parse(date).DayOfYear.ToString().PadLeft(3, '0'), serialNo.PadLeft(4, '0'));

            var bs = Encoding.ASCII.GetBytes(str);

            var writeFunc = new Func<bool>(
                () =>
                    MainCan.CanBusWithUds.TryWriteData(
                        GetCurrentSeesionCanId(),
                        GetCurrentSeesionRecvCanId(),
                        CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0x01, 0xB4, bs.ToArray()));

            if (writeFunc.Invoke())
                return;
            Thread.Sleep(500);
            writeFunc.Invoke();
        }

        /// <summary>
        /// 读生产序列号
        /// </summary>
        public void ReadSerialNo()
        {
            SerialNumerStr = string.Empty;

            if (MainCan == null)
                return;

            var readFunc = new Func<byte[]>(() =>
            {
                byte[] readSerialNo;
                MainCan.CanBusWithUds.TryReadData(
                    GetCurrentSeesionCanId(),
                    GetCurrentSeesionRecvCanId(),
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    0x01, 0xB4,
                    out readSerialNo);
                return readSerialNo;
            });

            var result = readFunc.Invoke() ?? readFunc.Invoke();

            if (result == null)
                return;

            foreach (var item in result.Where(item => item >= 0x30 && item <= 0x7A))
                SerialNumerStr += Encoding.ASCII.GetString(new[] { item });
        }

        /// <summary>
        /// 读引导程序零件号
        /// </summary>
        public void ReadFblPartNo()
        {
            FblPartNoStr = string.Empty;

            if (MainCan == null)
                return;

            var readFunc = new Func<byte[]>(() =>
            {
                byte[] readFblPartNo;
                MainCan.CanBusWithUds.TryReadData(
                    GetCurrentSeesionCanId(),
                    GetCurrentSeesionRecvCanId(),
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    0x01, 0xC0,
                    out readFblPartNo);
                return readFblPartNo;
            });

            var result = readFunc.Invoke() ?? readFunc.Invoke();

            if (result == null)
                return;

            FblPartNoStr = "SW" + ValueHelper.GetDecimal(result).ToString().PadLeft(7, '0');
        }

        /// <summary>
        /// 读引导程序版本号
        /// </summary>
        public void ReadFblVersion()
        {
            FblVersionStr = string.Empty;

            if (MainCan == null)
                return;

            var readFunc = new Func<byte[]>(() =>
            {
                byte[] readFblNo;
                MainCan.CanBusWithUds.TryReadData(
                    GetCurrentSeesionCanId(),
                    GetCurrentSeesionRecvCanId(),
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    0x01, 0xD0,
                    out readFblNo);
                return readFblNo;
            });

            var result = readFunc.Invoke() ?? readFunc.Invoke();

            if (result == null)
                return;
            foreach (var item in result.Where(item => item >= 0x30 && item <= 0x7A))
                FblVersionStr += Encoding.ASCII.GetString(new[] { item });
        }

        /// <summary>
        /// 读应用程序零件号
        /// </summary>
        public void ReadAppPartNo()
        {
            AppPartNoStr = string.Empty;

            if (MainCan == null)
                return;

            var readFunc = new Func<byte[]>(() =>
            {
                byte[] readAppPartNo;
                MainCan.CanBusWithUds.TryReadData(
                    GetCurrentSeesionCanId(),
                    GetCurrentSeesionRecvCanId(),
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    0x01, 0xC1,
                    out readAppPartNo);
                return readAppPartNo;
            });

            var result = readFunc.Invoke() ?? readFunc.Invoke();

            if (result == null)
                return;
            AppPartNoStr = "SW" + ValueHelper.GetDecimal(result).ToString().PadLeft(7, '0');
        }

        /// <summary>
        /// 读应用程序版本号
        /// </summary>
        public void ReadAppVersion()
        {
            AppVersionStr = string.Empty;

            if (MainCan == null)
                return;

            var readFunc = new Func<byte[]>(() =>
            {
                byte[] readAppNo;
                MainCan.CanBusWithUds.TryReadData(
                    GetCurrentSeesionCanId(),
                    GetCurrentSeesionRecvCanId(),
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    0x01, 0xD1,
                    out readAppNo);
                return readAppNo;
            });

            var result = readFunc.Invoke() ?? readFunc.Invoke();

            if (result == null)
                return;
            foreach (var item in result.Where(item => item >= 0x30 && item <= 0x7A))
                AppVersionStr += Encoding.ASCII.GetString(new[] { item });
        }

        /// <summary>
        /// 读取配置文件零件号
        /// </summary>
        public void ReadConfigPartNo()
        {
            ConfigPartNoStr = string.Empty;

            if (MainCan == null)
                return;

            var readFunc = new Func<byte[]>(() =>
            {
                byte[] readConfigPartNo;
                MainCan.CanBusWithUds.TryReadData(
                    GetCurrentSeesionCanId(),
                    GetCurrentSeesionRecvCanId(),
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    0x01, 0xC2,
                    out readConfigPartNo);
                return readConfigPartNo;
            });

            var result = readFunc.Invoke() ?? readFunc.Invoke();

            if (result == null)
                return;

            ConfigPartNoStr = "SW" + ValueHelper.GetDecimal(result).ToString().PadLeft(7, '0');
        }

        /// <summary>
        /// 读取配置文件版本号
        /// </summary>
        public void ReadConfigVersion()
        {
            ConfigVersionStr = string.Empty;

            if (MainCan == null)
                return;

            var readFunc = new Func<byte[]>(() =>
            {
                byte[] readConfigVersion;
                MainCan.CanBusWithUds.TryReadData(
                    GetCurrentSeesionCanId(),
                    GetCurrentSeesionRecvCanId(),
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    0x01, 0xD2,
                    out readConfigVersion);
                return readConfigVersion;
            });

            var result = readFunc.Invoke() ?? readFunc.Invoke();

            if (result == null)
                return;
            foreach (var item in result.Where(item => item >= 0x30 && item <= 0x7A))
                ConfigVersionStr += Encoding.ASCII.GetString(new[] { item });
        }

        public void MotorRun()
        {
            //_motorolaMatrix0X300.UpdateData(_lightOperaterDic[LampOnOffType.DhlDirConfAbove]);
            //_motorolaMatrix0X300.UpdateData(_lightOperaterDic[LampOnOffType.LdhlRefCmdActive]);
            //_motorolaMatrix0X300.UpdateData(_lightOperaterDic[LampOnOffType.LdhlConfPosStep255]);

            _motorolaMatrix0X300.MatrixData = new byte[] { 0xA1, 0x68, 0x00, 0x00, 0x00, 0x00, 0x81, 0x68 };
            Thread.Sleep(2000);
            _motorolaMatrix0X300.MatrixData = new byte[] { 0x20, 0x16, 0x00, 0x00, 0x00, 0x00, 0x00, 0x16 };
        }

        public void MotorNotRun()
        {
            _motorolaMatrix0X300.MatrixData = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        }

        public void EnterExtendedSession()
        {
            if (MainCan.CanBusWithUds.TryEnterExtendedSession(
                GetCurrentSeesionCanId(),
                GetCurrentSeesionRecvCanId(),
                CanBus.CanType.Standard, CanBus.CanProtocol.Can))
            {
                _isInExtendedSession = true;
                return;
            }

            Thread.Sleep(500);

            if (MainCan.CanBusWithUds.TryEnterExtendedSession(
                GetCurrentSeesionCanId(),
                GetCurrentSeesionRecvCanId(),
                CanBus.CanType.Standard, CanBus.CanProtocol.Can))
                _isInExtendedSession = true;
        }

        public void EnterDefaultSession()
        {
            _isInExtendedSession = false;

            if (MainCan.CanBusWithUds.TryEnterDefaultSession(
                GetCurrentSeesionCanId(),
                GetCurrentSeesionRecvCanId(),
                CanBus.CanType.Standard, CanBus.CanProtocol.Can))
                return;

            Thread.Sleep(500);
            MainCan.CanBusWithUds.TryEnterDefaultSession(
                GetCurrentSeesionCanId(),
                GetCurrentSeesionRecvCanId(),
                CanBus.CanType.Standard, CanBus.CanProtocol.Can);

        }

        #region 读写电感

        public double Led1Current;
        public double Led2Current;

        public double Led1K;
        public double Led1B;
        public double Led2K;
        public double Led2B;

        public double Inductance1;
        public double Inductance2;

        public void SetInductance()
        {
            if (MainCan == null)
                return;

            Led1K = 0.0001 * Led1K;
            Led1B = 0.0001 * Led1B;
            Led2K = 0.0001 * Led2K;
            Led2B = 0.0001 * Led2B;

            var led1Curr = Convert.ToUInt16(Led1Current * Led1K + Led1B);
            var led2Curr = Convert.ToUInt16(Led2Current * Led2K + Led2B);

            var led1Bytes = BitConverter.GetBytes(led1Curr);
            var led2Bytes = BitConverter.GetBytes(led2Curr);

            Array.Reverse(led1Bytes);
            Array.Reverse(led2Bytes);

            if (
                !MainCan.CanBusWithUds.TryStartRoutineControl(
                GetCurrentSeesionCanId(),
                GetCurrentSeesionRecvCanId(),
                    CanBus.CanType.Standard, Uds14229Helper.RoutineControl.Unknown1,
                    new[] { led1Bytes[0], led1Bytes[1], led2Bytes[0], led2Bytes[1] }))
            {
                Thread.Sleep(500);
                MainCan.CanBusWithUds.TryStartRoutineControl(
                    GetCurrentSeesionCanId(),
                    GetCurrentSeesionRecvCanId(),
                    CanBus.CanType.Standard, Uds14229Helper.RoutineControl.Unknown1,
                    new[] { led1Bytes[0], led1Bytes[1], led2Bytes[0], led2Bytes[1] });
            }
        }

        public void ReadInductance()
        {
            Inductance1 = -9999;
            Inductance2 = -9999;

            if (MainCan == null)
                return;

            var read12Func = new Func<byte[]>(() =>
            {
                byte[] readBytes;
                MainCan.CanBusWithUds.TryReadData(
                    GetCurrentSeesionCanId(),
                    GetCurrentSeesionRecvCanId(),
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    0xD0, 0x26,
                    out readBytes);
                return readBytes;
            });

            var read12 = read12Func.Invoke() ?? read12Func.Invoke();
            if (read12 != null && read12.Length >= 5)
            {
                if (read12[0] == 1)
                {
                    Inductance1 = read12[2] * 256 + read12[1];
                    Inductance2 = read12[4] * 256 + read12[1];
                }
            }
        }

        #endregion
        #endregion
    }
}
