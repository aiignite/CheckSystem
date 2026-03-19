using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Controller
{
    [Description("LIN-Product,AFS_Slave_Controller")]
    public sealed class AfsSlaveController : ControllerBase
    {
        public LinBus Lin;

        private Thread MainWorkThread { get; set; }
        private bool IsSleep { get; set; }

        public AfsSlaveController(string name)
            : base(name)
        {
            SlaveSleep();
            _isLeft = true;

            if (MainWorkThread != null)
            {
                MainWorkThread.Abort();
                MainWorkThread.Join();
            }

            MainWorkThread = new Thread(MainWork);
            MainWorkThread.Start();
        }

        ~AfsSlaveController()
        {
            Dispose();
        }

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
                        try
                        {
                            data0X30 = SetIntelValue(data0X30, 0, 10, uint.Parse(LDhlConfPos));
                            data0X30 = SetIntelValue(data0X30, 10, 2, uint.Parse(DhlDirConf));
                            data0X30 = SetIntelValue(data0X30, 15, 1, uint.Parse(LDhlRefCmd));
                            data0X30 = SetIntelValue(data0X30, 16, 11, uint.Parse(LAflConfPos));
                            data0X30 = SetIntelValue(data0X30, 27, 2, uint.Parse(AflFeqCmd));
                            data0X30 = SetIntelValue(data0X30, 29, 2, uint.Parse(AflFeqTab));
                            data0X30 = SetIntelValue(data0X30, 31, 1, uint.Parse(LAflRefCmd));
                            data0X30 = SetIntelValue(data0X30, 32, 11, uint.Parse(RAflConfPos));
                            data0X30 = SetIntelValue(data0X30, 47, 1, uint.Parse(RAflRefCmd));
                            data0X30 = SetIntelValue(data0X30, 48, 10, uint.Parse(RDhlConfPos));
                            data0X30 = SetIntelValue(data0X30, 63, 1, uint.Parse(RdhLRefCmd));
                        }
                        catch (Exception)
                        {
                            // ignored
                        }

                        switch (_motorType)
                        {
                            case MotorType.LeftDhl:
                                byte[] echoBytes0X34;
                                RecvData0X34 = Lin.SendMasterLinAndRecvSingleSlaveLin(0x30, 0x34, data0X30,
                                    out echoBytes0X34)
                                    ? ValueHelper.GetHextStr(echoBytes0X34)
                                    : string.Empty;
                                break;

                            case MotorType.RightDhl:
                                byte[] echoBytes0X35;
                                RecvData0X35 = Lin.SendMasterLinAndRecvSingleSlaveLin(0x30, 0x35, data0X30,
                                    out echoBytes0X35)
                                    ? ValueHelper.GetHextStr(echoBytes0X35)
                                    : string.Empty;
                                break;

                            case MotorType.LeftAfl:
                                byte[] echoBytes0X36;
                                RecvData0X36 = Lin.SendMasterLinAndRecvSingleSlaveLin(0x30, 0x36, data0X30,
                                    out echoBytes0X36)
                                    ? ValueHelper.GetHextStr(echoBytes0X36)
                                    : string.Empty;
                                break;

                            case MotorType.RightAfl:
                                byte[] echoBytes0X37;
                                RecvData0X37 = Lin.SendMasterLinAndRecvSingleSlaveLin(0x30, 0x37, data0X30,
                                    out echoBytes0X37)
                                    ? ValueHelper.GetHextStr(echoBytes0X37)
                                    : string.Empty;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

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

        private byte[] RecData
        {
            get { return _data ?? new byte[8]; }
            set
            {
                if (value != null)
                    _data = value;
            }
        }

        private bool _isLeft;
        private byte[] _data;
        private int _position;
        private MotorType _motorType = MotorType.LeftDhl;
        private volatile bool _isAutoScaning;
        private readonly object _linLocker = new object();

        [Description("AFS唤醒")]
        public void SlaveAwake()
        {
            IsSleep = false;
        }

        [Description("AFS休眠")]
        public void SlaveSleep()
        {
            IsSleep = true;
        }

        [Description("开启自动读取AFS状态")]
        public void StartAutoScanStatus()
        {
            _isAutoScaning = true;
        }

        [Description("停止自动读取AFS状态")]
        public void StopAutoScanStatus()
        {
            _isAutoScaning = false;
        }

        /// <summary>
        /// 当前为DHL左节点
        /// </summary>
        /// <returns></returns>
        [Description("当前为DHL左节点")]
        public void CurrentDhlLeft()
        {
            _motorType = MotorType.LeftDhl;
            //lock (_linLocker)
            //{
            //    var i = 0;
            //    byte[] echo1;
            //    do
            //    {
            //        Lin.SendMasterLin(0x08, new byte[] { 0x08, 0x97 });
            //        Thread.Sleep(20);
            //        Lin.SendMasterLin(0x09, new byte[] { 0x08, 0x00 });
            //        Thread.Sleep(20);
            //        Lin.SendSlaveLin(0x38, out echo1);
            //    } while (i++ < 3 && echo1.FirstOrDefault() != 0x97);
            //    IsLeft = true;
            //    return echo1.FirstOrDefault() == 0x97;
            //}
        }

        /// <summary>
        /// 当前为DHL右节点
        /// </summary>
        /// <returns></returns>
        [Description("当前为DHL右节点")]
        public void CurrentDhlRight()
        {
            _motorType = MotorType.RightDhl;
            //lock (_linLocker)
            //{
            //    var i = 0;
            //    byte[] echo1;
            //    do
            //    {
            //        Lin.SendMasterLin(0x08, new byte[] { 0x08, 0x03 });
            //        Thread.Sleep(20);
            //        Lin.SendMasterLin(0x09, new byte[] { 0x08, 0x00 });
            //        Thread.Sleep(20);
            //        Lin.SendSlaveLin(0x38, out echo1);
            //    } while (i++ < 10 && echo1.FirstOrDefault() != 0x03);
            //    IsLeft = false;
            //    return echo1.FirstOrDefault() == 0x50;
            //}
        }

        /// <summary>
        /// 当前为AFL左节点
        /// </summary>
        /// <returns></returns>
        [Description("当前为AFL左节点")]
        public void CurrentAflLeft()
        {
            _motorType = MotorType.LeftAfl;
            //lock (_linLocker)
            //{
            //    var i = 0;
            //    byte[] echo1;
            //    do
            //    {
            //        Lin.SendMasterLin(0x08, new byte[] { 0x08, 0x97 });
            //        Thread.Sleep(20);
            //        Lin.SendMasterLin(0x09, new byte[] { 0x08, 0x00 });
            //        Thread.Sleep(20);
            //        Lin.SendSlaveLin(0x38, out echo1);
            //    } while (i++ < 3 && echo1.FirstOrDefault() != 0x97);
            //    IsLeft = true;
            //    return echo1.FirstOrDefault() == 0x97;
            //}
        }

        /// <summary>
        /// 当前为AFL右节点
        /// </summary>
        /// <returns></returns>
        [Description("当前为AFL右节点")]
        public void CurrentAflRight()
        {
            _motorType = MotorType.RightAfl;
            //lock (_linLocker)
            //{
            //    var i = 0;
            //    byte[] echo1;
            //    do
            //    {
            //        Lin.SendMasterLin(0x08, new byte[] { 0x08, 0x03 });
            //        Thread.Sleep(20);
            //        Lin.SendMasterLin(0x09, new byte[] { 0x08, 0x00 });
            //        Thread.Sleep(20);
            //        Lin.SendSlaveLin(0x38, out echo1);
            //    } while (i++ < 10 && echo1.FirstOrDefault() != 0x03);
            //    IsLeft = false;
            //    return echo1.FirstOrDefault() == 0x50;
            //}
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
            ProductDate = string.Empty; //生产日期
            ProductNum = -9999; //生产序列号
            HardVersion = string.Empty; //硬件版本
            SoftVersion = string.Empty; //软件版本
            SoftPart = string.Empty; //软件零件号
            CurrentNode = string.Empty; //当前节点

            int slaveLinId;
            switch (_motorType)
            {
                case MotorType.LeftDhl:
                    slaveLinId = 0x38;
                    break;

                case MotorType.RightDhl:
                    slaveLinId = 0x39;
                    break;

                case MotorType.LeftAfl:
                    slaveLinId = 0x3A;
                    break;

                case MotorType.RightAfl:
                    slaveLinId = 0x3B;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            lock (_linLocker)
            {
                for (var i = 1; i <= 12; i++)
                {
                    var b = (byte)i;

                    byte[] value;
                    if (Lin.SendMasterLinAndRecvSingleSlaveLin(0x09, (byte)slaveLinId, new byte[] { b, 0x00 }, out value))
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
                            ProductDate = str;
                        }
                        else if (i == 3)
                        {
                            ProductNum = double.Parse(value[0].ToString("x").PadLeft(2, '0') + value[1].ToString("x").PadLeft(2, '0'));
                        }
                        else if (i == 4)
                        {
                            var tempstr = aSciiEncoding.GetString(new byte[] { value[0] });
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
            }
        }

        /// <summary>
        /// 电机移动
        /// </summary>
        /// <param name="pos"></param>
        [Description("电机移动")]
        public void MotorMove(int pos)
        {
            Lin.SendMasterLin(0x16, new byte[] { (byte)pos, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF });
            //Send30Message(pos);
        }

        [Description("R,左侧DHL状态反馈")]
        public string RecvData0X34;

        [Description("R,右侧DHL状态反馈")]
        public string RecvData0X35;

        [Description("R,左侧AFL状态反馈")]
        public string RecvData0X36;

        [Description("R,右侧AFL状态反馈")]
        public string RecvData0X37;

        [Description("R,供货零件号")]
        public string PartNum; //供货零件号

        [Description("R,生产日期")]
        public string ProductDate; //生产日期

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

        /// <summary>
        /// 左马达运动到目标位置
        /// </summary>
        /// <param name="pos"></param>
        [Description("左马达运动到目标位置")]
        public void LeftMotorMove(int pos)
        {
            Send30Message(pos);
        }

        /// <summary>
        /// 右马达运动到目标位置
        /// </summary>
        /// <param name="pos"></param>
        [Description("右马达运动到目标位置")]
        public void RightMotorMove(int pos)
        {
            Send30Message(rdhlConfPos: pos);
        }

        #region DHL读取状态
        [Description("R,DHL马达初始化完成标志位")]
        public string Dhl马达初始化完成标志位 = string.Empty;

        [Description("R,DHL马达状态")]
        public string Dhl马达状态 = string.Empty;

        [Description("R,DHL从节点低压故障检测开启")]
        public string Dhl从节点低压故障检测开启 = string.Empty;

        [Description("R,DHL从节点低压故障")]
        public string Dhl从节点低压故障 = string.Empty;

        [Description("R,DHL从节点过压故障检测开启")]
        public string Dhl从节点过压故障检测开启 = string.Empty;

        [Description("R,DHL从节点过压故障")]
        public string Dhl从节点过压故障 = string.Empty;

        [Description("R,DHL开路故障检测开启")]
        public string Dhl开路故障检测开启 = string.Empty;

        [Description("R,DHL开路故障")]
        public string Dhl开路故障 = string.Empty;

        [Description("R,DHL对地短路故障检测开启")]
        public string Dhl对地短路故障检测开启 = string.Empty;

        [Description("R,DHL对地短路故障")]
        public string Dhl对地短路故障 = string.Empty;

        [Description("R,DHL马达芯片温度报警检测开启")]
        public string Dhl马达芯片温度报警检测开启 = string.Empty;

        [Description("R,DHL马达芯片温度报警")]
        public string Dhl马达芯片温度报警 = string.Empty;

        [Description("R,DHL马达逻辑位置")]
        public string Dhl马达逻辑位置 = string.Empty;
        #endregion

        #region AFL读取状态

        [Description("R,AFL霍尔学习标志")]
        public string Afl霍尔学习标志 = string.Empty;

        [Description("R,AFL马达初始化完成标志位")]
        public string Afl马达初始化完成标志位 = string.Empty;

        [Description("R,AFL马达状态")]
        public string Afl马达状态 = string.Empty;

        [Description("R,AFL从节点低压故障检测开启")]
        public string Afl从节点低压故障检测开启 = string.Empty;

        [Description("R,AFL从节点低压故障")]
        public string Afl从节点低压故障 = string.Empty;

        [Description("R,AFL从节点过压故障检测开启")]
        public string Afl从节点过压故障检测开启 = string.Empty;

        [Description("R,AFL从节点过压故障")]
        public string Afl从节点过压故障 = string.Empty;

        [Description("R,AFL马达对地开路检测开启")]
        public string Afl马达对地开路检测开启 = string.Empty;

        [Description("R,AFL马达对地开路")]
        public string Afl马达对地开路 = string.Empty;

        [Description("R,AFL马达短路故障检测开启")]
        public string Afl马达短路故障检测开启 = string.Empty;

        [Description("R,AFL马达短路")]
        public string Afl马达短路 = string.Empty;

        [Description("R,AFL-Hall故障检测开启")]
        public string AflHall故障检测开启 = string.Empty;

        [Description("R,AFL-Hall故障")]
        public string AflHall故障 = string.Empty;

        [Description("R,AFL速度配置表ID")]
        public string Afl速度配置表id = string.Empty;

        [Description("R,AFL温度报警检测开启")]
        public string Afl温度报警检测开启 = string.Empty;

        [Description("R,AFL马达温度报警")]
        public string Afl马达温度报警 = string.Empty;

        [Description("R,AFL-Hid故障检测开启")]
        public string AflHid故障检测开启 = string.Empty;

        [Description("R,AFL-Hid故障")]
        public string AflHid故障 = string.Empty;

        [Description("R,AFL马达逻辑位置")]
        public string Afl马达逻辑位置 = string.Empty;

        #endregion

        #region 读取状态

        /// <summary>
        /// 读取状态信息
        /// </summary>
        [Description("读取状态信息")]
        public void ReadStateMessage()
        {
            //GetIntelValue(new byte[] { 0x08, 0x03 }, 0, 10);

            lock (_linLocker)
            {
                //var i = 0;
                //DataTypeConvert.ConvertBytesToString(recData.ToList(), "BCD", out string outdata);
                //Console.WriteLine("接收数据" + outdata);

                var recvData = new List<byte>();
                string strData;
                switch (_motorType)
                {
                    case MotorType.LeftDhl:
                        strData = RecvData0X34;
                        break;

                    case MotorType.RightDhl:
                        strData = RecvData0X35;
                        break;

                    case MotorType.LeftAfl:
                        strData = RecvData0X36;
                        break;

                    case MotorType.RightAfl:
                        strData = RecvData0X37;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (!string.IsNullOrEmpty(strData))
                {
                    var sp = strData.Split(' ');
                    recvData.AddRange(sp.Select(t => Convert.ToByte(t, 16)));
                    var bitData = new BitArray(recvData.ToArray());

                    if (_motorType == MotorType.LeftDhl || _motorType == MotorType.RightDhl)
                    {
                        Dhl马达初始化完成标志位 = GetIntelValue(recvData.ToArray(), 2, 1) == 0 ? "未完成" : "已完成";
                        Dhl马达状态 = GetIntelValue(recvData.ToArray(), 3, 1) == 0 ? "DHL静止" : "DHL运动";
                        Dhl从节点低压故障检测开启 = GetIntelValue(recvData.ToArray(), 4, 1) == 0 ? "关闭检测" : "开启检测";
                        Dhl从节点低压故障 = GetIntelValue(recvData.ToArray(), 5, 1) == 0 ? "正常（默认）" : "故障";
                        Dhl从节点过压故障检测开启 = GetIntelValue(recvData.ToArray(), 6, 1) == 0 ? "关闭检测" : "开启检测";
                        Dhl从节点过压故障 = GetIntelValue(recvData.ToArray(), 7, 1) == 0 ? "正常（默认）" : "故障";
                        Dhl开路故障检测开启 = GetIntelValue(recvData.ToArray(), 8, 1) == 0 ? "关闭检测" : "开启检测";
                        Dhl开路故障 = GetIntelValue(recvData.ToArray(), 9, 1) == 0 ? "正常（默认）" : "故障";
                        Dhl对地短路故障检测开启 = GetIntelValue(recvData.ToArray(), 10, 1) == 0 ? "关闭检测" : "开启检测";
                        Dhl对地短路故障 = GetIntelValue(recvData.ToArray(), 11, 1) == 0 ? "正常（默认）" : "故障";
                        Dhl马达芯片温度报警检测开启 = GetIntelValue(recvData.ToArray(), 12, 1) == 0 ? "关闭检测" : "开启检测";
                        Dhl马达芯片温度报警 = GetIntelValue(recvData.ToArray(), 13, 1) == 0 ? "正常（默认）" : "故障";
                        Dhl马达逻辑位置 = GetIntelValue(recvData.ToArray(), 16, 10).ToString();

                        //Dhl马达初始化完成标志位 = "未接受到数据";
                        //Dhl马达状态 = "未接受到数据";
                        //Dhl从节点低压故障检测开启 = "未接受到数据";
                        //Dhl从节点低压故障 = "未接受到数据";
                        //Dhl从节点过压故障检测开启 = "未接受到数据";
                        //Dhl从节点过压故障 = "未接受到数据";
                        //Dhl开路故障检测开启 = "未接受到数据";
                        //Dhl开路故障 = "未接受到数据";
                        //Dhl对地短路故障检测开启 = "未接受到数据";
                        //Dhl对地短路故障 = "未接受到数据";
                        //Dhl马达芯片温度报警检测开启 = "未接受到数据";
                        //Dhl马达芯片温度报警 = "未接受到数据";
                        //Dhl马达逻辑位置 = "未接受到数据";

                        Afl霍尔学习标志 = "未接受到数据";
                        Afl马达初始化完成标志位 = "未接受到数据";
                        Afl马达状态 = "未接受到数据";
                        Afl从节点低压故障检测开启 = "未接受到数据";
                        Afl从节点低压故障 = "未接受到数据";
                        Afl从节点过压故障检测开启 = "未接受到数据";
                        Afl从节点过压故障 = "未接受到数据";
                        Afl马达对地开路检测开启 = "未接受到数据";
                        Afl马达对地开路 = "未接受到数据";
                        Afl马达短路故障检测开启 = "未接受到数据";
                        Afl马达短路 = "未接受到数据";
                        AflHall故障检测开启 = "未接受到数据";
                        AflHall故障 = "未接受到数据";
                        Afl速度配置表id = "未接受到数据";
                        Afl温度报警检测开启 = "未接受到数据";
                        Afl马达温度报警 = "未接受到数据";
                        AflHid故障检测开启 = "未接受到数据";
                        AflHid故障 = "未接受到数据";
                        Afl马达逻辑位置 = "未接受到数据";
                    }
                    else if (_motorType == MotorType.LeftAfl || _motorType == MotorType.RightAfl)
                    {
                        if (GetIntelValue(recvData.ToArray(), 0, 2) == 0)
                            Afl霍尔学习标志 = "霍尔学习未实行";
                        else if (GetIntelValue(recvData.ToArray(), 0, 2) == 1)
                            Afl霍尔学习标志 = "霍尔学习已完成";
                        else if (GetIntelValue(recvData.ToArray(), 0, 2) == 2)
                            Afl霍尔学习标志 = "霍尔学习中";

                        Afl马达初始化完成标志位 = GetIntelValue(recvData.ToArray(), 2, 1) == 0 ? "未完成" : "已完成";
                        Afl马达状态 = GetIntelValue(recvData.ToArray(), 3, 1) == 0 ? "AFL静止" : "AFL运动";
                        Afl从节点低压故障检测开启 = GetIntelValue(recvData.ToArray(), 4, 1) == 0 ? "关闭检测" : "开启检测";
                        Afl从节点低压故障 = GetIntelValue(recvData.ToArray(), 5, 1) == 0 ? "正常（默认）" : "故障";
                        Afl从节点过压故障检测开启 = GetIntelValue(recvData.ToArray(), 6, 1) == 0 ? "关闭检测" : "开启检测";
                        Afl从节点过压故障 = GetIntelValue(recvData.ToArray(), 7, 1) == 0 ? "正常（默认）" : "故障";
                        Afl马达对地开路检测开启 = GetIntelValue(recvData.ToArray(), 8, 1) == 0 ? "关闭检测" : "开启检测";
                        Afl马达对地开路 = GetIntelValue(recvData.ToArray(), 9, 1) == 0 ? "正常（默认）" : "故障";
                        Afl马达短路故障检测开启 = GetIntelValue(recvData.ToArray(), 10, 1) == 0 ? "关闭检测" : "开启检测";
                        Afl马达短路 = GetIntelValue(recvData.ToArray(), 11, 1) == 0 ? "正常（默认）" : "故障";
                        AflHall故障检测开启 = GetIntelValue(recvData.ToArray(), 12, 1) == 0 ? "关闭检测" : "开启检测";
                        AflHall故障 = GetIntelValue(recvData.ToArray(), 13, 1) == 0 ? "正常（默认）" : "故障";
                        Afl速度配置表id = GetIntelValue(recvData.ToArray(), 14, 2).ToString();
                        Afl温度报警检测开启 = GetIntelValue(recvData.ToArray(), 16, 1) == 0 ? "关闭检测" : "开启检测";
                        Afl马达温度报警 = GetIntelValue(recvData.ToArray(), 17, 1) == 0 ? "正常（默认）" : "故障";
                        AflHid故障检测开启 = GetIntelValue(recvData.ToArray(), 18, 1) == 0 ? "关闭检测" : "开启检测";
                        AflHid故障 = GetIntelValue(recvData.ToArray(), 19, 1) == 0 ? "正常（默认）" : "故障";
                        Afl马达逻辑位置 = GetIntelValue(recvData.ToArray(), 21, 11).ToString();

                        Dhl马达初始化完成标志位 = "未接受到数据";
                        Dhl马达状态 = "未接受到数据";
                        Dhl从节点低压故障检测开启 = "未接受到数据";
                        Dhl从节点低压故障 = "未接受到数据";
                        Dhl从节点过压故障检测开启 = "未接受到数据";
                        Dhl从节点过压故障 = "未接受到数据";
                        Dhl开路故障检测开启 = "未接受到数据";
                        Dhl开路故障 = "未接受到数据";
                        Dhl对地短路故障检测开启 = "未接受到数据";
                        Dhl对地短路故障 = "未接受到数据";
                        Dhl马达芯片温度报警检测开启 = "未接受到数据";
                        Dhl马达芯片温度报警 = "未接受到数据";
                        Dhl马达逻辑位置 = "未接受到数据";

                        //Afl霍尔学习标志 = "未接受到数据";
                        //Afl马达初始化完成标志位 = "未接受到数据";
                        //Afl马达状态 = "未接受到数据";
                        //Afl从节点低压故障检测开启 = "未接受到数据";
                        //Afl从节点低压故障 = "未接受到数据";
                        //Afl从节点过压故障检测开启 = "未接受到数据";
                        //Afl从节点过压故障 = "未接受到数据";
                        //Afl马达对地开路检测开启 = "未接受到数据";
                        //Afl马达对地开路 = "未接受到数据";
                        //Afl马达短路故障检测开启 = "未接受到数据";
                        //Afl马达短路 = "未接受到数据";
                        //AflHall故障检测开启 = "未接受到数据";
                        //AflHall故障 = "未接受到数据";
                        //Afl速度配置表id = "未接受到数据";
                        //Afl温度报警检测开启 = "未接受到数据";
                        //Afl马达温度报警 = "未接受到数据";
                        //AflHid故障检测开启 = "未接受到数据";
                        //AflHid故障 = "未接受到数据";
                        //Afl马达逻辑位置 = "未接受到数据";
                    }
                }
                else
                {
                    Dhl马达初始化完成标志位 = "未接受到数据";
                    Dhl马达状态 = "未接受到数据";
                    Dhl从节点低压故障检测开启 = "未接受到数据";
                    Dhl从节点低压故障 = "未接受到数据";
                    Dhl从节点过压故障检测开启 = "未接受到数据";
                    Dhl从节点过压故障 = "未接受到数据";
                    Dhl开路故障检测开启 = "未接受到数据";
                    Dhl开路故障 = "未接受到数据";
                    Dhl对地短路故障检测开启 = "未接受到数据";
                    Dhl对地短路故障 = "未接受到数据";
                    Dhl马达芯片温度报警检测开启 = "未接受到数据";
                    Dhl马达芯片温度报警 = "未接受到数据";
                    Dhl马达逻辑位置 = "未接受到数据";

                    Afl霍尔学习标志 = "未接受到数据";
                    Afl马达初始化完成标志位 = "未接受到数据";
                    Afl马达状态 = "未接受到数据";
                    Afl从节点低压故障检测开启 = "未接受到数据";
                    Afl从节点低压故障 = "未接受到数据";
                    Afl从节点过压故障检测开启 = "未接受到数据";
                    Afl从节点过压故障 = "未接受到数据";
                    Afl马达对地开路检测开启 = "未接受到数据";
                    Afl马达对地开路 = "未接受到数据";
                    Afl马达短路故障检测开启 = "未接受到数据";
                    Afl马达短路 = "未接受到数据";
                    AflHall故障检测开启 = "未接受到数据";
                    AflHall故障 = "未接受到数据";
                    Afl速度配置表id = "未接受到数据";
                    Afl温度报警检测开启 = "未接受到数据";
                    Afl马达温度报警 = "未接受到数据";
                    AflHid故障检测开启 = "未接受到数据";
                    AflHid故障 = "未接受到数据";
                    Afl马达逻辑位置 = "未接受到数据";
                }

                //do
                //{
                //    if (RecData != null && RecData.Length > 0)
                //    {
                //        //string temp = string.Empty;
                //        //for (int j = 0; j < recData.Length-1; j++)
                //        //{
                //        //    temp+= DataTypeConvert.ByteToBinaryStr(recData[0]);
                //        //}
                //        //Console.WriteLine("temp: "+temp);


                //        var bitData = new BitArray(RecData);

                //        //if (bitData.Count <= 32)
                //        {
                //            Dhl马达初始化完成标志位 = bitData[2] ? "1" : "0";
                //            Dhl马达状态 = bitData[3] ? "运动中" : "静止";
                //            从节点低压故障检测开启 = bitData[4] ? "1" : "0";
                //            从节点低压故障 = bitData[5] ? "1" : "0";
                //            从节点过压故障检测开启 = bitData[6] ? "1" : "0";
                //            从节点过压故障 = bitData[7] ? "1" : "0";
                //            Dhl开路故障检测开启 = bitData[8] ? "1" : "0";
                //            Dhl开路故障 = bitData[9] ? "1" : "0";
                //            Dhl对地短路故障检测开启 = bitData[10] ? "1" : "0";
                //            Dhl对地短路故障 = bitData[11] ? "1" : "0";
                //            Dhl马达芯片温度报警检测开启 = bitData[12] ? "1" : "0";
                //            Dhl马达芯片温度报警 = bitData[13] ? "1" : "0";
                //            Dhl马达逻辑位置 = GetPosition(bitData).ToString();
                //            _position = GetPosition(bitData);
                //            Thread.Sleep(100);
                //            break;
                //        }
                //    }
                //} while (RecData != null && RecData.Length < 1);
            }
        }

        private static int GetPosition(BitArray bitArray)
        {
            var res = 0;
            for (var i = 16; i <= 26; i++)
            {
                if (bitArray.Get(i))
                {
                    res |= 1 << i - 16;
                }
            }
            return res;
        }

        #endregion

        #region 马达控制

        [Description("R/W,运动到目标档位")]
        public int MotorPos;

        [Description("R/W,左DHL位置")]
        public string LDhlConfPos = "0";

        [Description("R/W,DHL马达运动方向配置")]
        public string DhlDirConf = "0";

        [Description("R/W,左DHL马达初始化命令位")]
        public string LDhlRefCmd = "0";

        [Description("R/W,左AFL位置")]
        public string LAflConfPos = "0";

        [Description("R/W,马达驱动左右速度比")]
        public string AflFeqCmd = "0";

        [Description("R/W,AFL速度配置表")]
        public string AflFeqTab = "0";

        [Description("R/W,左AFL马达初始化命令位")]
        public string LAflRefCmd = "0";

        [Description("R/W,右AFL位置")]
        public string RAflConfPos = "0";

        [Description("R/W,右AFL马达初始化命令位")]
        public string RAflRefCmd = "0";

        [Description("R/W,右DHL位置")]
        public string RDhlConfPos = "0";

        [Description("R/W,右DHL马达初始化命令位")]
        public string RdhLRefCmd = "0";

        [Description("写入配置为左DHL")]
        public void WriteLeftDhl()
        {
            WriteConfig(MotorType.LeftDhl);
        }

        [Description("写入配置为右DHL")]
        public void WriteRightDhl()
        {
            WriteConfig(MotorType.RightDhl);
        }

        [Description("写入配置为左AFL")]
        public void WriteLeftAfl()
        {
            WriteConfig(MotorType.LeftAfl);
        }

        [Description("写入配置为右AFL")]
        public void WriteRightAfl()
        {
            WriteConfig(MotorType.RightAfl);
        }

        private void WriteConfig(MotorType motorType)
        {
            byte b;
            switch (motorType)
            {
                case MotorType.LeftDhl:
                    b = 0x97;
                    break;

                case MotorType.RightDhl:
                    b = 0x03;
                    break;

                case MotorType.LeftAfl:
                    b = 0x31;
                    break;

                case MotorType.RightAfl:
                    b = 0x59;
                    break;

                default:
                    throw new ArgumentOutOfRangeException("motorType", motorType, null);
            }

            lock (_linLocker)
            {
                Lin.SendMasterLin(0x08, new byte[] { 0x08, b });
            }
        }

        #endregion

        #region parameter   运行方向

        private readonly byte[] _setLeft = { 0x08, 0x97 };
        private readonly byte[] _setRight = { 0x08, 0x03 };
        private readonly byte[] _hallaStudy = { 0x00, 0x00 };

        private readonly byte[] _initMotorRight = { 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x68, 0x81 };
        private readonly byte[] _initMotorLeft = { 0x68, 0x85, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        private readonly byte[] _runToNominalRight = { 0x00, 0x00, 0x00, 0x00, 0x18, 0x01, 0xB4, 0x00 };
        private readonly byte[] _runToNominalLeft = { 0xB4, 0x00, 0x8C, 0x00, 0x00, 0x00, 0x00, 0x00 };

        private readonly byte[] _runToTopRight = { 0x46, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        private readonly byte[] _runToTopLeft = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x46, 0x01 };

        private readonly byte[] _runToBottonRight = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        private readonly byte[] _runToBottonLetf = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        [Description("马达初始化1")]
        public void InitMotor()
        {
            Lin.SendMasterLin(0x20, new byte[] { 0xff, 0xff, 0xff, 0xff });
            if (_isLeft)
            {
                Send30Message(500, 1, 1);
            }
            else
            {
                Send30Message(rdhlConfPos: 500, dhlDirConf: 1, rdhlRefCmd: 1);
            }
            Thread.Sleep(2000);
        }

        [Description("马达初始化2")]
        public void InitMotor2()
        {
            if (_isLeft)
            {
                Send30Message(500, 2, 1);
            }
            else
            {
                Send30Message(rdhlConfPos: 500, dhlDirConf: 2, rdhlRefCmd: 1);
            }
            Thread.Sleep(2000);
        }

        [Description("运动到光学零位")]
        public void RunToNominal()
        {
            Run(_runToNominalLeft, _runToNominalRight, () => true);
        }

        [Description("运动到上极限")]
        public void RunToTop()
        {
            Run(_runToTopLeft, _runToTopRight, new Func<bool>(() => { return _position == 0; }));
        }

        [Description("运动到下极限")]
        public void RunToBotton()
        {
            Run(_runToBottonLetf, _runToBottonRight, new Func<bool>(() => { return _position >= 300; }));
        }

        public CancellationTokenSource Source = new CancellationTokenSource();

        private void Run(byte[] leftcmd, byte[] rightCmd, Func<bool> func)
        {
            try
            {
                Source.Cancel();
                Source = new CancellationTokenSource();
                var token = Source.Token;
                var task = new Task<bool>(() =>
                {
                    while (!Source.IsCancellationRequested)
                    {
                        try
                        {
                            if (_isLeft)
                            {
                                byte[] value;
                                Lin.SendMasterLinAndRecvSingleSlaveLin(0x30, 0x34, leftcmd, out value);
                                RecData = value;
                                Thread.Sleep(2000);
                                //Console.WriteLine(this.recData.FirstOrDefault().ToString());
                            }
                            else
                            {
                                byte[] value;
                                Lin.SendMasterLinAndRecvSingleSlaveLin(0x30, 0x35, rightCmd, out value);
                                RecData = value;
                                Thread.Sleep(2000);
                                //Console.WriteLine(this.recData.FirstOrDefault().ToString());
                            }
                            if (func())
                                return true;
                        }
                        catch (Exception ex)
                        {
                            // ignored
                        }
                    }
                    return false;
                }, token);
                task.Start();
                if (!task.Wait(3000))
                {
                    Source.Cancel();
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        #endregion

        /// <summary>
        /// 0X30控制命令
        /// 先设置节点，马达初始化
        /// </summary>
        /// <param name="ldhlConfPos"> 左DHL位置 </param>
        /// <param name="dhlDirConf">DHL运动方向配置</param>
        /// <param name="ldhlRefCmd">左DHL马达初始化命令位</param>
        /// <param name="laflConfPos">左AFL位置</param>
        /// <param name="aflFeqCmd">马达驱动左右速度比</param>
        /// <param name="aflFeqTab">AFL速度配置表</param>
        /// <param name="laflRefCmd">左AFL马达初始化命令位</param>
        /// <param name="raflConfPos">右AFL位置</param>
        /// <param name="raflRefCmd">右AFL位置</param>
        /// <param name="rdhlConfPos">右DHL位置</param>
        /// <param name="rdhlRefCmd">右DHL马达初始化位置</param>
        private void Send30Message(int ldhlConfPos = 0, int dhlDirConf = 0, int ldhlRefCmd = 0, int laflConfPos = 0, int aflFeqCmd = 0, int aflFeqTab = 0, int laflRefCmd = 0, int raflConfPos = 0, int raflRefCmd = 0, int rdhlConfPos = 0, int rdhlRefCmd = 0)
        {
            lock (_linLocker)
            {
                var bytes = new byte[8];
                //左dhl位置 0-10
                var cmd = new List<bool>();
                cmd.AddRange(Send30Message(ldhlConfPos, 10));
                cmd.AddRange(Send30Message(dhlDirConf, 2));
                cmd.AddRange(Send30Message(0, 3));
                cmd.AddRange(Send30Message(ldhlRefCmd, 1));
                cmd.AddRange(Send30Message(laflConfPos, 11));
                cmd.AddRange(Send30Message(aflFeqCmd, 2));
                cmd.AddRange(Send30Message(aflFeqTab, 2));
                cmd.AddRange(Send30Message(laflRefCmd, 1));
                cmd.AddRange(Send30Message(raflConfPos, 11));
                cmd.AddRange(Send30Message(0, 4));
                cmd.AddRange(Send30Message(raflRefCmd, 1));
                cmd.AddRange(Send30Message(rdhlConfPos, 10));
                cmd.AddRange(Send30Message(0, 5));
                cmd.AddRange(Send30Message(rdhlRefCmd, 1));
                var bit = new BitArray(cmd.ToArray());
                bit.CopyTo(bytes, 0);
                Lin.SendMasterLin(0x30, bytes);
            }
        }

        private static IEnumerable<bool> Send30Message(int num, int len)
        {
            var cmd = new List<bool>();
            var bit = new BitArray(new int[] { num });
            for (var i = 0; i < len; i++)
                cmd.Add(bit[i]);
            return cmd;
        }

        public int GetIntelValue(byte[] data, int startBit, int bitLen)
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

        public byte[] SetIntelValue(byte[] data, int startBit, int bitLen, uint value)
        {
            try
            {
                var bits = Convert.ToString(value, 2).PadLeft(32, '0').ToCharArray().Reverse().ToArray();
                var bitsList = new List<string>();
                for (var i = 0; i < bitLen; i++)
                    bitsList.Add(bits[i].ToString());

                var bitData = new BitArray(data);
                var listBitStr = new List<string>();
                for (var i = 0; i < bitData.Count; i++)
                {
                    listBitStr.Add(bitData[i] ? "1" : "0");
                }

                for (var i = 0; i < bitLen; i++)
                {
                    listBitStr[startBit + i] = bitsList[i];
                }

                var returnByte = new List<byte>();
                for (var i = 0; i < listBitStr.Count; i = i + 8)
                {
                    var str = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", listBitStr[i + 7], listBitStr[i + 6], listBitStr[i + 5], listBitStr[i + 4], listBitStr[i + 3], listBitStr[i + 2], listBitStr[i + 1], listBitStr[i + 0]);

                    var b = Convert.ToByte(str, 2);
                    returnByte.Add(b);
                }

                return returnByte.ToArray();
            }
            catch (Exception)
            {
                return data;
            }
        }

        #region 打印标签

        ///// <summary>
        ///// 打印标签
        ///// </summary>
        //public FireWallPrintLable fireWallPrintLable = null;

        //public void PrintLable()
        //{
        //    //var ProductBarcode = string.Format(
        //    //          "{0}{1}{2}{3}06{4}Y{5}{6}P{7}{8}12V547656863{9}T{10}{11}{12}",
        //    //          Convert.ToChar(0x5B),
        //    //          Convert.ToChar(0x29),
        //    //          Convert.ToChar(0x3E),
        //    //          Convert.ToChar(0x1E),
        //    //          Convert.ToChar(0x1D),

        //    //         // vppsiDmatrix,
        //    //          Convert.ToChar(0x1D),
        //    //         // partid,
        //    //          Convert.ToChar(0x1D),
        //    //          Convert.ToChar(0x1D),
        //    //         // productId_B4,
        //    //          Convert.ToChar(0x1E),
        //    //          Convert.ToChar(0x04));
        //    string code = "[)>06S00013476SAM13312V08mA004mmH006S015 W0960024";

        //    string font = "Font001";
        //    var command = new StringBuilder();

        //    command.Append("SIZE 25 mm,25 mm\r\n");
        //    command.Append("GAP 12 mm\r\n");
        //    //command.Append("GAP 3 mm,0\r\n");
        //    command.Append("DENSITY 10\r\n");
        //    command.Append("SPEED 1\r\n");
        //    command.Append("CLS\r\n");
        //    command.Append("DIRECTION 1,0\r\n");
        //    //command.Append("DMATRIX 170,80,100,100,");

        //    command.Append(string.Format("TEXT 40,80,\"2\",0,2,2,\"{0}\"\r\n", "S00020886"));
        //    //command.Append("654514868");
        //    command.Append(string.Format("TEXT 160,160,\"2\",0,2,2,\"{0}\"\r\n", "M133"));
        //    command.Append(string.Format("TEXT 160,190,\"2\",0,2,2,\"{0}\"\r\n", "0615"));
        //    command.Append(string.Format("TEXT 160,220,\"2\",0,2,2,\"{0}\"\r\n", "w096"));
        //    command.Append(string.Format("TEXT 160,250,\"2\",0,2,2,\"{0}\"\r\n", "0024"));
        //    command.Append(string.Format("DMATRIX 40,170,300,300,x4,\"{0}\"\r\n", code));


        //    //command.Append("SIZE 60 mm, 30 mm\r\n");
        //    //command.Append("GAP 3 mm\r\n");
        //    //command.Append("DENSITY 10\r\n");
        //    //command.Append("SPEED 2\r\n");
        //    //command.Append("CLS\r\n");
        //    //command.Append("DIRECTION 1,0\r\n");
        //    //command.Append(string.Format("TEXT 160,150,\"{0}\",0,2,2,\"{1}\"\r\n", font, "M133"));
        //    //command.Append(string.Format("TEXT 160,180,\"{0}\",0,2,2,\"{1}\"\r\n", font, "0615"));
        //    //command.Append(string.Format("TEXT 160,210,\"{0}\",0,2,2,\"{1}\"\r\n", font, "w096"));
        //    //command.Append(string.Format("TEXT 160,240,\"{0}\",0,2,2,\"{1}\"\r\n", font, "0024"));
        //    ////command.Append(string.Format("DMATRIX 140,170,300,300,x6,\"{0}\"\r\n", ProductBarcode));
        //    command.Append("PRINT 1,1");
        //    command.Append("\r\n");
        //    fireWallPrintLable.WriteData(command.ToString());
        //}

        #endregion

        internal enum MotorType
        {
            /// <summary>
            /// 0x34
            /// </summary>
            LeftDhl,

            /// <summary>
            /// 0x35
            /// </summary>
            RightDhl,

            /// <summary>
            /// 0x36
            /// </summary>
            LeftAfl,

            /// <summary>
            /// 0x37
            /// </summary>
            RightAfl
        }
    }
}
