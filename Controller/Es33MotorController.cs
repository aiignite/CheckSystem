using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using CommonUtility;
using CommonUtility.BusLoader;

namespace Controller
{
    [Description("LIN-Product,ES33调光执行器")]
    public sealed class Es33MotorController : ControllerBase
    {
        public LinBus Lin;

        public Es33MotorController(string name)
            : base(name)
        {
            SlaveSleep();

            if (MainWorkThread != null)
            {
                MainWorkThread.Abort();
                MainWorkThread.Join();
            }

            MainWorkThread = new Thread(MainWork);
            MainWorkThread.Start();
        }

        ~Es33MotorController()
        {
            Dispose();
        }

        [Description("R,状态反馈")]
        public string RecvData;

        [Description("Slave唤醒")]
        public void SlaveAwake()
        {
            IsSleep = false;
        }

        [Description("Slave休眠")]
        public void SlaveSleep()
        {
            IsSleep = true;
        }

        [Description("开启自动读取状态")]
        public void StartAutoScanStatus()
        {
            _isAutoScaning = true;
        }

        [Description("停止自动读取状态")]
        public void StopAutoScanStatus()
        {
            _isAutoScaning = false;
        }

        /// <summary>
        /// 电机移动到档位
        /// </summary>
        /// <param name="pos"></param>
        [Description("电机移动到档位")]
        public void MotorMove(int pos)
        {
            lock (_linLocker)
            {
                Lin.SendMasterLin(0x16, new byte[] { (byte)pos, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF });
            }
        }

        private Thread MainWorkThread { get; set; }
        private bool IsSleep { get; set; }
        private volatile bool _isAutoScaning;
        private readonly object _linLocker = new object();

        private void MainWork()
        {
            while (MainWorkThread.IsAlive)
            {
                if (!MainWorkThread.IsAlive)
                    break;

                Thread.Sleep(25);

                if (Lin == null)
                    continue;

                if (IsSleep)
                    continue;

                try
                {
                    lock (_linLocker)
                    {
                        Lin.SendMasterLin(0x20, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, });
                        Thread.Sleep(25);

                        var data0X30 = new byte[8];


                        byte[] echoBytes0X37;
                        RecvData = Lin.SendMasterLinAndRecvSingleSlaveLin(0x30, 0x37, data0X30,
                            out echoBytes0X37)
                            ? ValueHelper.GetHextStr(echoBytes0X37)
                            : string.Empty;

                        if (_isAutoScaning)
                        {
                            ReadStateMessage();
                        }

                        Thread.Sleep(150);
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        #region 读取状态

        [Description("R,霍尔学习标志")]
        public string 霍尔学习标志 = string.Empty;

        [Description("R,马达初始化完成标志位")]
        public string 马达初始化完成标志位 = string.Empty;

        [Description("R,马达状态")]
        public string 马达状态 = string.Empty;

        [Description("R,从节点低压故障检测开启")]
        public string 从节点低压故障检测开启 = string.Empty;

        [Description("R,从节点低压故障")]
        public string 从节点低压故障 = string.Empty;

        [Description("R,从节点过压故障检测开启")]
        public string 从节点过压故障检测开启 = string.Empty;

        [Description("R,从节点过压故障")]
        public string 从节点过压故障 = string.Empty;

        [Description("R,马达对地开路检测开启")]
        public string 马达对地开路检测开启 = string.Empty;

        [Description("R,马达对地开路")]
        public string 马达对地开路 = string.Empty;

        [Description("R,马达短路故障检测开启")]
        public string 马达短路故障检测开启 = string.Empty;

        [Description("R,马达短路")]
        public string 马达短路 = string.Empty;

        [Description("R,Hall故障检测开启")]
        public string Hall故障检测开启 = string.Empty;

        [Description("R,Hall故障")]
        public string Hall故障 = string.Empty;

        [Description("R,速度配置表ID")]
        public string 速度配置表id = string.Empty;

        [Description("R,温度报警检测开启")]
        public string 温度报警检测开启 = string.Empty;

        [Description("R,马达温度报警")]
        public string 马达温度报警 = string.Empty;

        [Description("R,Hid故障检测开启")]
        public string Hid故障检测开启 = string.Empty;

        [Description("R,Hid故障")]
        public string Hid故障 = string.Empty;

        [Description("R,马达逻辑位置")]
        public string 马达逻辑位置 = string.Empty;

        #endregion

        /// <summary>
        /// 读取状态信息
        /// </summary>
        [Description("读取状态信息")]
        public void ReadStateMessage()
        {
            lock (_linLocker)
            {
                var recvData = new List<byte>();
                var strData = RecvData;

                if (!string.IsNullOrEmpty(strData))
                {
                    var sp = strData.Split(' ');
                    recvData.AddRange(sp.Select(t => Convert.ToByte(t, 16)));

                    if (GetIntelValue(recvData.ToArray(), 0, 2) == 0)
                        霍尔学习标志 = "霍尔学习未实行";
                    else if (GetIntelValue(recvData.ToArray(), 0, 2) == 1)
                        霍尔学习标志 = "霍尔学习已完成";
                    else if (GetIntelValue(recvData.ToArray(), 0, 2) == 2)
                        霍尔学习标志 = "霍尔学习中";

                    马达初始化完成标志位 = GetIntelValue(recvData.ToArray(), 2, 1) == 0 ? "未完成" : "已完成";
                    马达状态 = GetIntelValue(recvData.ToArray(), 3, 1) == 0 ? "静止" : "运动";
                    从节点低压故障检测开启 = GetIntelValue(recvData.ToArray(), 4, 1) == 0 ? "关闭检测" : "开启检测";
                    从节点低压故障 = GetIntelValue(recvData.ToArray(), 5, 1) == 0 ? "正常（默认）" : "故障";
                    从节点过压故障检测开启 = GetIntelValue(recvData.ToArray(), 6, 1) == 0 ? "关闭检测" : "开启检测";
                    从节点过压故障 = GetIntelValue(recvData.ToArray(), 7, 1) == 0 ? "正常（默认）" : "故障";
                    马达对地开路检测开启 = GetIntelValue(recvData.ToArray(), 8, 1) == 0 ? "关闭检测" : "开启检测";
                    马达对地开路 = GetIntelValue(recvData.ToArray(), 9, 1) == 0 ? "正常（默认）" : "故障";
                    马达短路故障检测开启 = GetIntelValue(recvData.ToArray(), 10, 1) == 0 ? "关闭检测" : "开启检测";
                    马达短路 = GetIntelValue(recvData.ToArray(), 11, 1) == 0 ? "正常（默认）" : "故障";
                    Hall故障检测开启 = GetIntelValue(recvData.ToArray(), 12, 1) == 0 ? "关闭检测" : "开启检测";
                    Hall故障 = GetIntelValue(recvData.ToArray(), 13, 1) == 0 ? "正常（默认）" : "故障";
                    速度配置表id = GetIntelValue(recvData.ToArray(), 14, 2).ToString();
                    温度报警检测开启 = GetIntelValue(recvData.ToArray(), 16, 1) == 0 ? "关闭检测" : "开启检测";
                    马达温度报警 = GetIntelValue(recvData.ToArray(), 17, 1) == 0 ? "正常（默认）" : "故障";
                    Hid故障检测开启 = GetIntelValue(recvData.ToArray(), 18, 1) == 0 ? "关闭检测" : "开启检测";
                    Hid故障 = GetIntelValue(recvData.ToArray(), 19, 1) == 0 ? "正常（默认）" : "故障";
                    马达逻辑位置 = GetIntelValue(recvData.ToArray(), 21, 11).ToString();
                }
                else
                {
                    霍尔学习标志 = "未接受到数据";
                    马达初始化完成标志位 = "未接受到数据";
                    马达状态 = "未接受到数据";
                    从节点低压故障检测开启 = "未接受到数据";
                    从节点低压故障 = "未接受到数据";
                    从节点过压故障检测开启 = "未接受到数据";
                    从节点过压故障 = "未接受到数据";
                    马达对地开路检测开启 = "未接受到数据";
                    马达对地开路 = "未接受到数据";
                    马达短路故障检测开启 = "未接受到数据";
                    马达短路 = "未接受到数据";
                    Hall故障检测开启 = "未接受到数据";
                    Hall故障 = "未接受到数据";
                    速度配置表id = "未接受到数据";
                    温度报警检测开启 = "未接受到数据";
                    马达温度报警 = "未接受到数据";
                    Hid故障检测开启 = "未接受到数据";
                    Hid故障 = "未接受到数据";
                    马达逻辑位置 = "未接受到数据";
                }
            }
        }

        [Description("R,节点配置信息-01-供货零件号")]
        public string TraceInfo01;

        [Description("R,节点配置信息-02-生产日期")]
        public string TraceInfo02;

        [Description("R,节点配置信息-03-生产序列号")]
        public string TraceInfo03;

        [Description("R,节点配置信息-04-硬件版本")]
        public string TraceInfo04;

        [Description("R,节点配置信息-05-软件零件号")]
        public string TraceInfo05;

        [Description("R,节点配置信息-06-应用程序软件版本")]
        public string TraceInfo06;

        [Description("R,节点配置信息-07-保留")]
        public string TraceInfo07;

        [Description("R,节点配置信息-08-节点配置")]
        public string TraceInfo08;

        [Description("R,节点配置信息-09-保留")]
        public string TraceInfo09;

        [Description("R,节点配置信息-0A-DHL马达运动方向配置")]
        public string TraceInfo0A;

        [Description("R,节点配置信息-0B-马达复位位置(低8位)")]
        public string TraceInfo0B;

        [Description("R,节点配置信息-0C-马达复位位置(高8位)")]
        public string TraceInfo0C;

        [Description("R,供货零件号")]
        public string PartNum; //供货零件号

        [Description("R,生产日期")]
        public int ProductDate; //生产日期

        [Description("R,生产序列号")]
        public double ProductNum = -9999; //生产序列号

        [Description("R,硬件版本")]
        public string HardVersion; //硬件版本

        [Description("R,软件版本")]
        public string SoftVersion; //软件版本

        [Description("R,软件零件号")]
        public string SoftPart; //软件零件号

        [Description("R,当前节点")]
        public string CurrentNode; //当前节点

        [Description("R,应用程序版本号")]
        public string AppVersion;

        [Description("读节点配置信息")]
        public void ReadTraceInfo()
        {
            for (var i = 1; i <= 12; i++)
            {
                var strName = string.Format("TraceInfo" + ValueHelper.GetHextStr((byte)i));
                var fi = GetType().GetField(strName);
                if (fi != null)
                    fi.SetValue(this, string.Empty);
            }

            PartNum = string.Empty; //供货零件号
            ProductDate = -9999; //生产日期
            ProductNum = -9999; //生产序列号
            HardVersion = string.Empty; //硬件版本
            SoftVersion = string.Empty; //软件版本
            SoftPart = string.Empty; //软件零件号
            CurrentNode = string.Empty; //当前节点
            AppVersion = string.Empty;

            const int slaveLinId = 0x3B;

            lock (_linLocker)
            {
                #region EPPROM
                for (var i = 1; i <= 12; i++)
                {
                    var b = (byte)i;

                    byte[] value;
                    if (Lin.SendMasterLinAndRecvSingleSlaveLin(0x09, slaveLinId, new byte[] { b, 0x00 }, out value))
                    {
                        var strName = string.Format("TraceInfo" + ValueHelper.GetHextStr((byte)i));
                        var strValue = ValueHelper.GetHextStr(value);

                        var fi = GetType().GetField(strName);
                        if (fi != null)
                            fi.SetValue(this, strValue);

                        var aSciiEncoding = new ASCIIEncoding();
                        if (i == 1)
                        {
                            var tempstr = aSciiEncoding.GetString(new[] { value[0] });
                            PartNum = tempstr + value[1].ToString("x").PadLeft(2, '0') + value[2].ToString("x").PadLeft(2, '0') + value[3].ToString("x").PadLeft(2, '0') + value[4].ToString("x").PadLeft(2, '0');
                        }
                        else if (i == 2)
                        {
                            var str = aSciiEncoding.GetString(value);
                            ProductDate = int.Parse(str);
                        }
                        else if (i == 3)
                        {
                            ProductNum = double.Parse(value[0].ToString("x").PadLeft(2, '0') + value[1].ToString("x").PadLeft(2, '0'));
                        }
                        else if (i == 4)
                        {
                            var tempstr = aSciiEncoding.GetString(new[] { value[0] });
                            HardVersion = tempstr + value[1].ToString("00") + value[2];
                        }
                        else if (i == 5)
                        {
                            var temp = value[2].ToString("x") + value[3].ToString("x") + value[4].ToString("x");
                            var no = Convert.ToInt32(temp, 16);
                            SoftPart = "SW" + no.ToString().PadLeft(7, '0');
                        }
                        else if (i == 6)
                        {
                            SoftVersion = aSciiEncoding.GetString(new[] { value[0] }) + value[1].ToString("00") + value[2].ToString("x");
                        }
                        else if (i == 8)
                        {
                            if (value[0] == 0x97)
                                CurrentNode = "DHL左节点";
                            else if (value[0] == 0x03)
                                CurrentNode = "DHL右节点";
                            else if (value[0] == 0x31)
                                CurrentNode = "AFL左节点";
                            else if (value[0] == 0x59)
                                CurrentNode = "AFL右节点";
                            else if (value[0] == 0xFF)
                                CurrentNode = "默认数据";
                        }
                    }
                }
                #endregion

                byte[] appVerEcho;
                if (Lin.SendMasterLinAndRecvSingleSlaveLin(0x30, 0x3A, new byte[] { 0x14, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, out appVerEcho))
                {
                    if (appVerEcho != null && appVerEcho.Length == 8 && appVerEcho[0] == 0x14 && appVerEcho[1] == 0x03)
                    {
                        AppVersion =
                            Encoding.ASCII.GetString(new[] { appVerEcho[2], appVerEcho[3], appVerEcho[4], appVerEcho[5] });
                    }
                }
            }
        }

        private static int GetIntelValue(byte[] data, int startBit, int bitLen)
        {
            try
            {
                var bitData = new BitArray(data);

                var listBitStr = new List<string>();
                for (var i = 0; i < bitLen; i++)
                {
                    listBitStr.Add(bitData[startBit + i] ? "1" : "0");
                }

                var str = string.Empty;
                for (var i = listBitStr.Count - 1; i >= 0; i--)
                    str += listBitStr[i];

                var value = Convert.ToInt32(str, 2);

                return value;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        //private byte[] SetIntelValue(byte[] data, int startBit, int bitLen, uint value)
        //{
        //    try
        //    {
        //        var bits = Convert.ToString(value, 2).PadLeft(32, '0').ToCharArray().Reverse().ToArray();
        //        var bitsList = new List<string>();
        //        for (var i = 0; i < bitLen; i++)
        //            bitsList.Add(bits[i].ToString());

        //        var bitData = new BitArray(data);
        //        var listBitStr = new List<string>();
        //        for (var i = 0; i < bitData.Count; i++)
        //        {
        //            listBitStr.Add(bitData[i] ? "1" : "0");
        //        }

        //        for (var i = 0; i < bitLen; i++)
        //        {
        //            listBitStr[startBit + i] = bitsList[i];
        //        }

        //        var returnByte = new List<byte>();
        //        for (var i = 0; i < listBitStr.Count; i = i + 8)
        //        {
        //            var str = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", listBitStr[i + 7], listBitStr[i + 6], listBitStr[i + 5], listBitStr[i + 4], listBitStr[i + 3], listBitStr[i + 2], listBitStr[i + 1], listBitStr[i + 0]);

        //            var b = Convert.ToByte(str, 2);
        //            returnByte.Add(b);
        //        }

        //        return returnByte.ToArray();
        //    }
        //    catch (Exception)
        //    {
        //        return data;
        //    }
        //}

        #region 激光打标

        [Description("R/W,打印生产日期文本文件")]
        public string PrintProductDateFilePath = @"C:\Users\B765\Desktop\生产日期.txt";

        [Description("R/W,打印二维码文本文件")]
        public string PrintMatrixFilePath = @"C:\Users\B765\Desktop\二维码.txt";

        [Description("打印")]
        public void Print(string version)
        {
            //ProductDate = 22263;

            //var dateTime = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            //ProductDate = int.Parse(string.Format("{0}{1}", DateTime.Parse(dateTime).Year.ToString().Substring(2, 2), DateTime.Parse(dateTime).DayOfYear.ToString().PadLeft(3, '0')));
            //ProductNum = 2;
            //PartNum = "S00022342";
            //HardVersion = "H001";

            if (string.IsNullOrEmpty(PartNum))
                return;

            if (string.IsNullOrEmpty(HardVersion))
                return;

            if (string.IsNullOrEmpty(version))
                return;

            if (ProductDate != -9999 && Math.Abs(ProductNum - -9999) > 0)
            {
                try
                {
                    var year = "20" + ProductDate.ToString().Substring(0, 2);
                    var date =
                        DateTime.Parse(string.Format("{0} 01/01", year))
                            .AddDays(int.Parse(ProductDate.ToString().Substring(2, 3)) - 1).ToString("yyyyMMdd");
                    WriteTxt(PrintProductDateFilePath, date);

                    var str = string.Format("{0}{1}{2}{3}", PartNum, HardVersion, version, date);
                    WriteTxt(PrintMatrixFilePath, str);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private static void WriteTxt(string filePath, string value)
        {
            try
            {
                var code = value;

                var lines = File.ReadAllLines(filePath);
                var list = new List<string>();
                list.AddRange(lines);
                list.Clear();
                list.Add(code);
                lines = list.ToArray();

                File.WriteAllLines(filePath, lines);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        #endregion
    }
}
