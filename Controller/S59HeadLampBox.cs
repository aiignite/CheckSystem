using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Controller
{
    public sealed class S59HeadLampBox : ControllerBase
    {
        private SyRenesasMcuControllerMaster ControllerMaster { get; set; }
        //private MyUdpClient MyUdpClient { get; set; }
        public string RemoteIpPort { get; set; }
        private int RemotePort { get; set; }
        private IPAddress RemoteIpAddress { get; set; }
        public bool IsConnected { get; set; }
        public bool IsLog;
        public int MaxLogCount = 10000;

        public LinBus Lin1;
        public LinBus Lin2;

        private string LIN1LFolder = string.Format(@"{0}/{1}_Lin1_L", Path.GetPathRoot(Directory.GetCurrentDirectory()), "s59_log");
        private string LIN1RFolder = string.Format(@"{0}/{1}_Lin1_R", Path.GetPathRoot(Directory.GetCurrentDirectory()), "s59_log");
        private string LIN2LFolder = string.Format(@"{0}/{1}_Lin2_L", Path.GetPathRoot(Directory.GetCurrentDirectory()), "s59_log");
        private string LIN2RFolder = string.Format(@"{0}/{1}_Lin2_R", Path.GetPathRoot(Directory.GetCurrentDirectory()), "s59_log");

        public S59HeadLampBox(string name) : base(name)
        {
            //for (var i = 0; i < 5; i++)
            //{
            //    _logLin1DataL.Add(new LogData { Time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:sss"), Value = i.ToString() });
            //    Thread.Sleep(1000);
            //}

            //SaveLog(true, 1);

            //for (var i = 5; i < 10; i++)
            //{
            //    _logLin1DataL.Add(new LogData { Time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:sss"), Value = i.ToString() });
            //    Thread.Sleep(1000);
            //}

            //SaveLog(true, 1);

            LinBus.PushLinMsg += LinBus_PushLinMsg;

            _modeTimer = new System.Timers.Timer();
            _modeTimer.Elapsed += _modeTimer_Elapsed;
            _modeTimer.Interval = 100;
            _modeTimer.AutoReset = true;
            _modeTimer.Enabled = false;
            _modeTimer.Start();

            _monitorTimer = new System.Timers.Timer();
            _monitorTimer.Elapsed += _monitorTimer_Elapsed;
            _monitorTimer.Interval = 100;
            _monitorTimer.AutoReset = true;
            _monitorTimer.Enabled = false;
            _monitorTimer.Start();

            _logTh = new Thread(LogWork) { IsBackground = true };
            _logTh.Start();
        }

        private void LinBus_PushLinMsg(string name, LinBus.LinDataPackage data)
        {

            if (name == "SlaveLin1" && data != null && data.LinData.Length == 8)
            {
                // LIN ID 0x20是左灯，0x21是右灯
                if (data.LinId == 0x20 ||
                    LinBus.ConvertLinId(data.LinId) == 0x20 ||
                    LinBus.ConvertLinId(0x20) == data.LinId)
                {
                    Lin1LShow = ValueHelper.GetHextStrWithOx(data.LinData);
                    var matrix = new LinCommunicationMatrix.MotorolaMatrix(0x01, 8) { MatrixData = data.LinData };
                    Lin1LTurnState = matrix.GetMatrixData(5, 1).ToString();
                    _tsLin1Left = HighPrecisionTimer.GetTimestamp();

                    lock (_lockSave)
                    {
                        var str = string.Format("{0}:{1}", ValueHelper.GetHextStrWithOx(0x20), Lin1LShow);
                        _logLin1DataL.Add(new LogData { Time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:sss"), Index = _logLin1DataL.Count.ToString(), Value = str });
                    }
                }
                else if (data.LinId == 0x21 ||
                         LinBus.ConvertLinId(data.LinId) == 0x21 ||
                         LinBus.ConvertLinId(0x21) == data.LinId)
                {
                    Lin1RShow = ValueHelper.GetHextStrWithOx(data.LinData);
                    var matrix = new LinCommunicationMatrix.MotorolaMatrix(0x01, 8) { MatrixData = data.LinData };
                    Lin1RTurnState = matrix.GetMatrixData(5, 1).ToString();

                    _tsLin1Right = HighPrecisionTimer.GetTimestamp();

                    lock (_lockSave)
                    {
                        var str = string.Format("{0}:{1}", ValueHelper.GetHextStrWithOx(0x21), Lin1RShow);
                        _logLin1DataR.Add(new LogData { Time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:sss"), Index = _logLin1DataR.Count.ToString(), Value = str });
                    }
                }
                else if (data.LinId == 0x13 ||
                         LinBus.ConvertLinId(data.LinId) == 0x13 ||
                         LinBus.ConvertLinId(0x13) == data.LinId)
                {
                    _tsLin10X13 = HighPrecisionTimer.GetTimestamp();
                    Lin10X13Show = ValueHelper.GetHextStrWithOx(data.LinData);

                    lock (_lockSave)
                    {
                        var str = string.Format("{0}:{1}", ValueHelper.GetHextStrWithOx(0x13), ValueHelper.GetHextStrWithOx(data.LinData));
                        _logLin1DataL.Add(new LogData { Time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:sss"), Index = _logLin1DataL.Count.ToString(), Value = str });
                        _logLin1DataR.Add(new LogData { Time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:sss"), Index = _logLin1DataR.Count.ToString(), Value = str });
                    }
                }
            }

            if (name == "SlaveLin2" && data != null && data.LinData.Length == 8)
            {
                // LIN ID 0x20是左灯，0x21是右灯
                if (data.LinId == 0x20 ||
                    LinBus.ConvertLinId(data.LinId) == 0x20 ||
                    LinBus.ConvertLinId(0x20) == data.LinId)
                {
                    Lin2LShow = ValueHelper.GetHextStrWithOx(data.LinData);
                    var matrix = new LinCommunicationMatrix.MotorolaMatrix(0x01, 8) { MatrixData = data.LinData };
                    Lin2LTurnState = matrix.GetMatrixData(5, 1).ToString();

                    _tsLin2Left = HighPrecisionTimer.GetTimestamp();

                    lock (_lockSave)
                    {
                        var str = string.Format("{0}:{1}", ValueHelper.GetHextStrWithOx(0x20), Lin2LShow);
                        _logLin2DataL.Add(new LogData { Time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:sss"), Index = _logLin2DataL.Count.ToString(), Value = str });
                    }
                }
                else if (data.LinId == 0x21 ||
                         LinBus.ConvertLinId(data.LinId) == 0x21 ||
                         LinBus.ConvertLinId(0x21) == data.LinId)
                {
                    Lin2RShow = ValueHelper.GetHextStrWithOx(data.LinData);
                    var matrix = new LinCommunicationMatrix.MotorolaMatrix(0x01, 8) { MatrixData = data.LinData };
                    Lin2RTurnState = matrix.GetMatrixData(5, 1).ToString();

                    _tsLin2Right = HighPrecisionTimer.GetTimestamp();

                    lock (_lockSave)
                    {
                        var str = string.Format("{0}:{1}", ValueHelper.GetHextStrWithOx(0x21), Lin2RShow);
                        _logLin2DataR.Add(new LogData { Time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:sss"), Index = _logLin2DataR.Count.ToString(), Value = str });
                    }
                }
                else if (data.LinId == 0x13 ||
                         LinBus.ConvertLinId(data.LinId) == 0x13 ||
                         LinBus.ConvertLinId(0x13) == data.LinId)
                {
                    _tsLin20X13 = HighPrecisionTimer.GetTimestamp();
                    Lin20X13Show = ValueHelper.GetHextStrWithOx(data.LinData);

                    lock (_lockSave)
                    {
                        var str = string.Format("{0}:{1}", ValueHelper.GetHextStrWithOx(0x13), ValueHelper.GetHextStrWithOx(data.LinData));
                        _logLin2DataR.Add(new LogData { Time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:sss"), Index = _logLin2DataR.Count.ToString(), Value = str });
                        _logLin2DataL.Add(new LogData { Time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:sss"), Index = _logLin2DataL.Count.ToString(), Value = str });
                    }
                }
            }
        }

        private long _tsLin1Left;
        private long _tsLin1Right;
        private long _tsLin2Left;
        private long _tsLin2Right;
        private long _tsLin10X13;
        private long _tsLin20X13;

        private void _monitorTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //LinBus_PushLinMsg("SlaveLin" + (0 + 1), new LinBus.LinDataPackage(0x20, new byte[8]));
            //LinBus_PushLinMsg("SlaveLin" + (0 + 1), new LinBus.LinDataPackage(0x13, new byte[8]));

            var nowTs = HighPrecisionTimer.GetTimestamp();

            if (HighPrecisionTimer.GetTimestampIntervalMs(_tsLin1Left, nowTs) > 2500)
            {
                Lin1LShow = string.Empty;
                Lin1LTurnState = string.Empty;
            }

            if (HighPrecisionTimer.GetTimestampIntervalMs(_tsLin1Right, nowTs) > 2500)
            {
                Lin1RShow = string.Empty;
                Lin1RTurnState = string.Empty;
            }

            if (HighPrecisionTimer.GetTimestampIntervalMs(_tsLin2Left, nowTs) > 2500)
            {
                Lin2LShow = string.Empty;
                Lin2LTurnState = string.Empty;
            }

            if (HighPrecisionTimer.GetTimestampIntervalMs(_tsLin2Right, nowTs) > 2500)
            {
                Lin2RShow = string.Empty;
                Lin2RTurnState = string.Empty;
            }

            if (HighPrecisionTimer.GetTimestampIntervalMs(_tsLin10X13, nowTs) > 2500)
            {
                Lin10X13Show = string.Empty;
            }

            if (HighPrecisionTimer.GetTimestampIntervalMs(_tsLin20X13, nowTs) > 2500)
            {
                Lin20X13Show = string.Empty;
            }
        }

        private Thread _logTh;

        private readonly List<LogData> _logLin1DataL = new List<LogData>();
        private readonly List<LogData> _logLin2DataL = new List<LogData>();
        private readonly List<LogData> _logLin1DataR = new List<LogData>();
        private readonly List<LogData> _logLin2DataR = new List<LogData>();

        public class LogData
        {
            public string Time { get; set; }
            public string Index { get; set; }
            public string Value { get; set; }
        }

        private void LogWork()
        {
            var count = 0;

            while (_logTh.IsAlive)
            {
                if (!_logTh.IsAlive)
                    break;

                if (!IsLog)
                    continue;

                Thread.Sleep(20);

                //// LIN ID 0x20是左灯，0x21是右灯
                //if (count == 50 || count == 250 || count == 450)
                //{
                //    var res = ReadData(true, Lin1, 1);
                //    //Lin1LShow = res.Item1;
                //    //Lin1LTurnState = res.Item2;
                //}
                //if (count == 100 || count == 300 || count == 500)
                //{
                //    var res = ReadData(false, Lin1, 1);
                //    //Lin1RShow = res.Item1;
                //    //Lin1RTurnState = res.Item2;
                //}
                //if (count == 150 || count == 350 || count == 550)
                //{
                //    var res = ReadData(true, Lin2, 2);
                //    //Lin2LShow = res.Item1;
                //    //Lin2LTurnState = res.Item2;
                //}
                //if (count == 200 || count == 400 || count == 600)
                //{
                //    var res = ReadData(false, Lin2, 2);
                //    //Lin2RShow = res.Item1;
                //    //Lin2RTurnState = res.Item2;
                //}

                count++;
                if (count > 300)
                {
                    lock (_lockSave)
                    {
                        SaveLog(true, 1);
                        SaveLog(true, 2);
                        SaveLog(false, 1);
                        SaveLog(false, 2);
                    }
                    count = 0;
                }
            }
        }

        public string Lin10X13Show = string.Empty;
        public string Lin1LShow = string.Empty;
        public string Lin1RShow = string.Empty;
        public string Lin2LShow = string.Empty;
        public string Lin2RShow = string.Empty;

        public string Lin20X13Show = string.Empty;
        public string Lin1LTurnState = string.Empty;
        public string Lin1RTurnState = string.Empty;
        public string Lin2LTurnState = string.Empty;
        public string Lin2RTurnState = string.Empty;

        private object _lockSave = new object();

        private Tuple<string, string> ReadData(bool isLeft, LinBus lin, int linIndex)
        {
            if (lin != null)
            {
                // LIN ID 0x20是左灯，0x21是右灯
                byte linId = 0x20;
                if (!isLeft)
                    linId = 0x21;

                byte[] echo;
                lin.SendSlaveLin(linId, out echo, 50);
                return Tuple.Create(string.Empty, string.Empty);

                byte[] readData;
                if (lin.SendSlaveLin(linId, out readData, timeOutMs: 100))
                {
                    var str = ValueHelper.GetHextStrWithOx(readData);
                    if (!string.IsNullOrEmpty(str) && readData != null && readData.Length == 8)
                    {
                        var readStr = str;
                        var matrix = new LinCommunicationMatrix.MotorolaMatrix(0x01, 8);
                        matrix.MatrixData = readData;
                        var turnState = matrix.GetMatrixData(5, 1).ToString();

                        str = string.Format("{0}:{1}", ValueHelper.GetHextStrWithOx(linId), str);

                        if (isLeft)
                        {
                            if (linIndex == 1)
                            {
                                _logLin1DataL.Add(new LogData { Time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:sss"), Value = str });
                            }
                            else
                            {
                                _logLin2DataL.Add(new LogData { Time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:sss"), Value = str });
                            }
                        }
                        else
                        {
                            if (linIndex == 1)
                            {
                                _logLin1DataR.Add(new LogData { Time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:sss"), Value = str });
                            }
                            else
                            {
                                _logLin2DataR.Add(new LogData { Time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:sss"), Value = str });
                            }
                        }

                        return Tuple.Create(readStr, turnState); ;
                    }
                }
            }

            return Tuple.Create(string.Empty, string.Empty);
        }

        private bool CreateFolder(bool isLeft, int linIndex)
        {
            try
            {
                var path = LIN1LFolder;
                if (isLeft)
                {
                    if (linIndex == 1)
                    {
                        path = LIN1LFolder;
                    }
                    else
                    {
                        path = LIN2LFolder;
                    }
                }
                else
                {
                    if (linIndex == 1)
                    {
                        path = LIN1RFolder;
                    }
                    else
                    {
                        path = LIN2RFolder;
                    }
                }

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                return true;
            }
            catch (Exception)
            {
                return false;
                // ignored
            }
        }

        private bool CreateTxt(string folder, ref string fileName)
        {
            fileName = string.Format(@"{0}/{1}.txt", folder, DateTime.Now.ToString("yyyyMMddHHmmss"));
            return true;
        }

        private bool WriteTxt(string filePath, List<LogData> logData)
        {
            try
            {
                using (var fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.None))
                {
                    var tp = new List<LogData>();
                    tp.AddRange(logData);
                    tp.Sort((x, y) => DateTime.Compare(DateTime.Parse(x.Time), DateTime.Parse(y.Time)));

                    foreach (var b in tp.Select(t => Encoding.Default.GetBytes(string.Format("{0} {1}: {2}\r\n", " ", t.Time, t.Value))))
                    {
                        fs.Write(b, 0, b.Length);//写数据
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
                // ignored
            }
        }

        private void SaveLog(bool isLeft, int linIndex)
        {
            if (CreateFolder(isLeft, linIndex))
            {
                List<LogData> logData;

                if (isLeft)
                {
                    logData = linIndex == 1 ? _logLin1DataL : _logLin2DataL;
                }
                else
                {
                    logData = linIndex == 1 ? _logLin1DataR : _logLin2DataR;
                }

                if (!logData.Any())
                    return;

                string path;
                if (isLeft)
                {
                    path = linIndex == 1 ? LIN1LFolder : LIN2LFolder;

                }
                else
                {
                    path = linIndex == 1 ? LIN1RFolder : LIN2RFolder;

                }

                int logCount;
                string lastPath;
                if (isLeft)
                {
                    lastPath = linIndex == 1 ? _lastLPathLin1 : _lastLPathLin2;
                    logCount = linIndex == 1 ? _lastLLogCountLin1 : _lastLLogCountLin2;
                }
                else
                {
                    lastPath = linIndex == 1 ? _lastRPathLin1 : _lastRPathLin2;
                    logCount = linIndex == 1 ? _lastRLogCountLin1 : _lastRLogCountLin2;
                }

                SaveLog(logData, path, ref lastPath, ref logCount);

                if (isLeft)
                {
                    if (linIndex == 1)
                    {
                        _lastLPathLin1 = lastPath;
                        _lastLLogCountLin1 = logCount;
                    }
                    else
                    {
                        _lastLPathLin2 = lastPath;
                        _lastLLogCountLin2 = logCount;
                    }
                }
                else
                {
                    if (linIndex == 1)
                    {
                        _lastRPathLin1 = lastPath;
                        _lastRLogCountLin1 = logCount;
                    }
                    else
                    {
                        _lastRPathLin2 = lastPath;
                        _lastRLogCountLin2 = logCount;
                    }
                }
            }
        }

        private string _lastLPathLin1 = string.Empty;
        private string _lastLPathLin2 = string.Empty;
        private string _lastRPathLin1 = string.Empty;
        private string _lastRPathLin2 = string.Empty;

        private int _lastLLogCountLin1;
        private int _lastRLogCountLin1;
        private int _lastLLogCountLin2;
        private int _lastRLogCountLin2;

        private void SaveLog(List<LogData> logData, string folder, ref string lastPath, ref int logCount)
        {
            if (!logData.Any())
            {
                return;
            }

            if (string.IsNullOrEmpty(lastPath))
            {
                var fileName = string.Empty;
                if (CreateTxt(folder, ref fileName))
                {
                    lastPath = fileName;
                    logCount = logData.Count;

                    if (WriteTxt(fileName, logData))
                    {
                        logData.Clear();
                    }
                    else if (logData.Count > MaxLogCount * 2)
                    {
                        logData.Clear();
                    }
                }
            }
            else
            {
                if (logCount > MaxLogCount)
                {
                    var fileName = string.Empty;
                    if (CreateTxt(folder, ref fileName))
                    {
                        lastPath = fileName;
                        logCount = logData.Count;
                        if (WriteTxt(fileName, logData))
                        {
                            logData.Clear();
                        }
                        else if (logData.Count > MaxLogCount * 2)
                        {
                            logData.Clear();
                            logCount = 0;
                        }
                    }
                    else if (logData.Count > MaxLogCount * 2)
                    {
                        logData.Clear();
                        logCount = 0;
                    }
                }
                else
                {
                    if (WriteTxt(lastPath, logData))
                    {
                        logCount += logData.Count;
                        logData.Clear();
                    }
                    else if (logData.Count > MaxLogCount * 2)
                    {
                        logData.Clear();
                        logCount = 0;
                    }
                }
            }
        }

        public void InitRemoteIpAddress(string ipPort)
        {
            try
            {
                ControllerMaster = new SyRenesasMcuControllerMaster("S59");
                ControllerMaster.InitRemoteIpAddress(ipPort);

                //Lin1 = ControllerMaster.GatewayLin1;
                //Lin2 = ControllerMaster.GatewayLin2;

                SendCmd();
                IsConnected = true;
            }
            catch (Exception e)
            {
                IsConnected = false;
            }

            try
            {
                var slaveLinTomoss = new ToomossUsb2XxxSlaveLin("slave lin");
                slaveLinTomoss.SetBaudRate("1", "19200");
                slaveLinTomoss.SetBaudRate("2", "19200");

                if (slaveLinTomoss._devHandle > 0)
                {
                    Task.Factory.StartNew(() =>
                    {
                        while (true)
                        {
                            for (var k = 0; k < 2; k++)
                            {
                                var LINMsg = new Usb2LinEx.LinExMsg[1024];//缓冲区尽量大一点，防止益处
                                //将数组转换成指针
                                var pt = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Usb2LinEx.LinExMsg)) * LINMsg.Length);

                                var ret = Usb2LinEx.LIN_EX_SlaveGetData(slaveLinTomoss._devHandle, (byte)k, pt);

                                for (var i = 0; i < ret; i++)
                                {
                                    LINMsg[i] = (Usb2LinEx.LinExMsg)Marshal.PtrToStructure(
                                        (IntPtr)((uint)pt + i * Marshal.SizeOf(typeof(Usb2LinEx.LinExMsg))),
                                        typeof(Usb2LinEx.LinExMsg));

                                    if (LINMsg[i].Check > 0)
                                    {
                                        var lindId = LINMsg[i].PID;
                                        var lindData = LINMsg[i].Data;
                                        var msg = new LinBus.LinDataPackage(lindId, lindData);
                                        LinBus_PushLinMsg("SlaveLin" + (k + 1), msg);
                                    }
                                }
                                //释放内存
                                Marshal.FreeHGlobal(pt);
                                Thread.Sleep(5);
                            }

                            Thread.Sleep(5);
                        }
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            //RemoteIpPort = ipPort;

            //try
            //{
            //    var sp = ipPort.Split(':');
            //    var ipAddrStr = sp[0];
            //    var port = int.Parse(sp[1]);

            //    RemotePort = port;
            //    RemoteIpAddress = IPAddress.Parse(ipAddrStr);

            //    MyUdpClient = ipAddrStr.Equals("127.0.0.1")
            //        ? new MyUdpClient("127.0.0.1", port + 1)
            //        : new MyUdpClient("192.168.1.50", 5000 + int.Parse(ipAddrStr.Split('.')[3]));

            //    MyUdpClient.AddRemoteClient(ipAddrStr, port);
            //    MyUdpClient.BeginReceive();


            //    SetTimer(new MyTaskScheduler.TaskInfo { Action = LinNetWork(), Interval = 50 });
            //    SchedulerAsync();

            //    IsConnected = true;
            //}
            //catch (Exception)
            //{
            //    IsConnected = false;
            //}
        }

        private Action LinNetWork()
        {
            //return SendCmd;

            return () => { };
        }

        [Description("R,近光是否打开")]
        public bool IsLbOn;
        [Description("R,远光是否打开")]
        public bool IsHbOn;
        [Description("R,转向是否打开")]
        public bool IsTurnOn;
        [Description("R,DRL是否打开")]
        public bool IsDrlOn;
        [Description("R,PL是否打开")]
        public bool IsPlOn;
        [Description("R,当前模式")]
        public string CurrentLightMode = LightMode.Normal.ToString();
        [Description("R,当前动画")]
        public string CurrentAnimation = string.Empty;
        [Description("R,当前动画剩余/S")]
        public string CurrentAnimationDuration = string.Empty;

        private int _modeCount;
        private readonly System.Timers.Timer _modeTimer;
        private readonly System.Timers.Timer _monitorTimer;
        private int _intCount;

        private byte _viulUnLockWelcomeLampReq;
        private byte _viulLockWelcomeLampReq;
        private byte _viulSayhiLampModeSetReq;
        private byte _viulMusicLampReq;


        private void _modeTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var mode = EnumOperater.GetEnumByValue<LightMode>(CurrentLightMode);

            switch (mode)
            {
                case LightMode.Normal:
                    _intCount = 0;
                    CurrentAnimation = string.Empty;
                    CurrentAnimationDuration = string.Empty;
                    return;

                case LightMode.Mode1:
                    _intCount = 0;
                    CurrentAnimation = string.Empty;
                    CurrentAnimationDuration = string.Empty;
                    return;

                case LightMode.Mode2:
                    _intCount++;
                    if (_modeCount == 0)
                    {
                        CurrentAnimation = @"迎宾1(8s)";

                        if (_intCount >= (8 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;

                            // 0==>1
                            IsLbOn = false;
                            IsHbOn = false;
                            IsDrlOn = false;
                            IsPlOn = false;
                            IsTurnOn = false;
                            _viulUnLockWelcomeLampReq = 0x02;
                            _viulLockWelcomeLampReq = 0x00;
                            _viulSayhiLampModeSetReq = 0x00;
                            SendCmd();
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 1)
                    {
                        CurrentAnimation = @"迎宾2(6.4s)";

                        if (_intCount >= (6.4 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;

                            // 1==>2
                            IsLbOn = false;
                            IsHbOn = false;
                            IsDrlOn = false;
                            IsPlOn = false;
                            IsTurnOn = false;
                            _viulUnLockWelcomeLampReq = 0x00;
                            _viulLockWelcomeLampReq = 0x01;
                            _viulSayhiLampModeSetReq = 0x00;
                            SendCmd();
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 2)
                    {
                        CurrentAnimation = @"闭锁1(2.88s)";

                        if (_intCount >= (2.88 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;

                            // 2==>3
                            IsLbOn = false;
                            IsHbOn = false;
                            IsDrlOn = false;
                            IsPlOn = false;
                            IsTurnOn = false;
                            _viulUnLockWelcomeLampReq = 0x00;
                            _viulLockWelcomeLampReq = 0x02;
                            _viulSayhiLampModeSetReq = 0x00;
                            SendCmd();
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 3)
                    {
                        CurrentAnimation = @"闭锁2(3s)";

                        if (_intCount >= (3 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;

                            // 3==>4
                            IsLbOn = false;
                            IsHbOn = false;
                            IsDrlOn = false;
                            IsPlOn = false;
                            IsTurnOn = false;
                            _viulUnLockWelcomeLampReq = 0x03;
                            _viulLockWelcomeLampReq = 0x00;
                            _viulSayhiLampModeSetReq = 0x00;
                            SendCmd();
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 4)
                    {
                        CurrentAnimation = @"SayHi1(61.48s)";

                        if (_intCount >= (61.48 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;

                            // 4==>5
                            IsLbOn = false;
                            IsHbOn = false;
                            IsDrlOn = false;
                            IsPlOn = false;
                            IsTurnOn = false;
                            _viulUnLockWelcomeLampReq = 0x04;
                            _viulLockWelcomeLampReq = 0x00;
                            _viulSayhiLampModeSetReq = 0x00;
                            SendCmd();
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 5)
                    {
                        CurrentAnimation = @"SayHi2(62.2s)";

                        if (_intCount >= (62.2 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;

                            // 5==>6
                            IsLbOn = false;
                            IsHbOn = false;
                            IsDrlOn = false;
                            IsPlOn = false;
                            IsTurnOn = false;
                            _viulUnLockWelcomeLampReq = 0x05;
                            _viulLockWelcomeLampReq = 0x00;
                            _viulSayhiLampModeSetReq = 0x00;
                            SendCmd();
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 6)
                    {
                        CurrentAnimation = @"SayHi3(61.48s)";

                        if (_intCount >= (61.48 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount = 0;
                            _intCount = 0;

                            // 6==>0
                            IsLbOn = false;
                            IsHbOn = false;
                            IsDrlOn = false;
                            IsPlOn = false;
                            IsTurnOn = false;
                            _viulUnLockWelcomeLampReq = 0x01;
                            _viulLockWelcomeLampReq = 0x00;
                            _viulSayhiLampModeSetReq = 0x00;
                            SendCmd();
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    break;

                case LightMode.Mode3:
                    _intCount = 0;
                    CurrentAnimation = string.Empty;
                    CurrentAnimationDuration = string.Empty;
                    return;

                case LightMode.Mode4:
                    _intCount = 0;
                    CurrentAnimation = string.Empty;
                    CurrentAnimationDuration = string.Empty;
                    return;

                case LightMode.Mode5:
                    _intCount = 0;
                    CurrentAnimation = string.Empty;
                    CurrentAnimationDuration = string.Empty;
                    return;

                case LightMode.Mode6:
                    _intCount++;
                    if (_modeCount == 0)
                    {
                        CurrentAnimation = @"迎宾1(8s)";

                        if (_intCount >= (8 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;

                            // 0==>1
                            IsLbOn = false;
                            IsHbOn = false;
                            IsDrlOn = false;
                            IsPlOn = false;
                            IsTurnOn = false;
                            _viulUnLockWelcomeLampReq = 0x00;
                            _viulLockWelcomeLampReq = 0x01;
                            _viulSayhiLampModeSetReq = 0x00;
                            SendCmd();
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 1)
                    {
                        CurrentAnimation = @"闭锁1(2.88s)";

                        if (_intCount >= (2.88 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount = 0;
                            _intCount = 0;

                            // 1==>0
                            IsLbOn = false;
                            IsHbOn = false;
                            IsDrlOn = false;
                            IsPlOn = false;
                            IsTurnOn = false;
                            _viulUnLockWelcomeLampReq = 0x01;
                            _viulLockWelcomeLampReq = 0x00;
                            _viulSayhiLampModeSetReq = 0x00;
                            SendCmd();
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    break;

                case LightMode.Mode7:
                    _intCount++;
                    if (_modeCount == 0)
                    {
                        CurrentAnimation = @"迎宾2(6.4s)";

                        if (_intCount >= (6.4 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;

                            // 0==>1
                            IsLbOn = false;
                            IsHbOn = false;
                            IsDrlOn = false;
                            IsPlOn = false;
                            IsTurnOn = false;
                            _viulUnLockWelcomeLampReq = 0x00;
                            _viulLockWelcomeLampReq = 0x02;
                            _viulSayhiLampModeSetReq = 0x00;
                            SendCmd();
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 1)
                    {
                        CurrentAnimation = @"闭锁2(3s)";

                        if (_intCount >= (3 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount = 0;
                            _intCount = 0;

                            // 1==>0
                            IsLbOn = false;
                            IsHbOn = false;
                            IsDrlOn = false;
                            IsPlOn = false;
                            IsTurnOn = false;
                            _viulUnLockWelcomeLampReq = 0x02;
                            _viulLockWelcomeLampReq = 0x00;
                            _viulSayhiLampModeSetReq = 0x00;
                            SendCmd();
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    break;

                case LightMode.Mode8:
                    _intCount++;
                    if (_modeCount == 0)
                    {
                        CurrentAnimation = @"SayHi1(61.48s)";

                        if (_intCount >= (61.48 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;

                            // 0==>1
                            IsLbOn = false;
                            IsHbOn = false;
                            IsDrlOn = false;
                            IsPlOn = false;
                            IsTurnOn = false;
                            _viulUnLockWelcomeLampReq = 0x04;
                            _viulLockWelcomeLampReq = 0x00;
                            _viulSayhiLampModeSetReq = 0x00;
                            SendCmd();
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 1)
                    {
                        CurrentAnimation = @"SayHi2(62.2s)";

                        if (_intCount >= (62.2 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;

                            // 1==>2
                            IsLbOn = false;
                            IsHbOn = false;
                            IsDrlOn = false;
                            IsPlOn = false;
                            IsTurnOn = false;
                            _viulUnLockWelcomeLampReq = 0x05;
                            _viulLockWelcomeLampReq = 0x00;
                            _viulSayhiLampModeSetReq = 0x00;
                            SendCmd();
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 2)
                    {
                        CurrentAnimation = @"SayHi3(61.48s)";

                        if (_intCount >= (61.48 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount = 0;
                            _intCount = 0;

                            // 2==>0
                            IsLbOn = false;
                            IsHbOn = false;
                            IsDrlOn = false;
                            IsPlOn = false;
                            IsTurnOn = false;
                            _viulUnLockWelcomeLampReq = 0x03;
                            _viulLockWelcomeLampReq = 0x00;
                            _viulSayhiLampModeSetReq = 0x00;
                            SendCmd();
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    break;

                case LightMode.Mode9:
                    _intCount = 0;
                    return;

                case LightMode.Mode10:
                    _intCount = 0;
                    CurrentAnimation = string.Empty;
                    CurrentAnimationDuration = string.Empty;
                    return;

                case LightMode.Mode11:
                    _intCount = 0;
                    CurrentAnimation = string.Empty;
                    CurrentAnimationDuration = string.Empty;
                    return;

                case LightMode.Mode12:
                    _intCount = 0;
                    CurrentAnimation = string.Empty;
                    CurrentAnimationDuration = string.Empty;
                    return;

                case LightMode.Mode13:
                    _intCount = 0;
                    CurrentAnimation = string.Empty;
                    CurrentAnimationDuration = string.Empty;
                    return;

                case LightMode.Mode14:
                    _intCount++;
                    if (_modeCount == 0)
                    {
                        CurrentAnimation = @"PL(10s)";

                        if (_intCount >= (10 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;

                            // 0==>1
                            IsLbOn = true;
                            IsHbOn = false;
                            IsDrlOn = false;
                            IsPlOn = true;
                            IsTurnOn = false;
                            _viulUnLockWelcomeLampReq = 0x00;
                            _viulLockWelcomeLampReq = 0x00;
                            _viulSayhiLampModeSetReq = 0x00;
                            SendCmd();
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 1)
                    {
                        CurrentAnimation = @"LB&PL(10s)";

                        if (_intCount >= (10 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;

                            // 1==>2
                            IsLbOn = true;
                            IsHbOn = true;
                            IsDrlOn = false;
                            IsPlOn = true;
                            IsTurnOn = false;
                            _viulUnLockWelcomeLampReq = 0x00;
                            _viulLockWelcomeLampReq = 0x00;
                            _viulSayhiLampModeSetReq = 0x00;
                            SendCmd();
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 2)
                    {
                        CurrentAnimation = @"LB&HB&PL(10s)";

                        if (_intCount >= (10 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;

                            // 2==>3
                            IsLbOn = true;
                            IsHbOn = true;
                            IsDrlOn = false;
                            IsPlOn = true;
                            IsTurnOn = true;
                            _viulUnLockWelcomeLampReq = 0x00;
                            _viulLockWelcomeLampReq = 0x00;
                            _viulSayhiLampModeSetReq = 0x00;
                            SendCmd();
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 3)
                    {
                        CurrentAnimation = @"LB&HB&PL&TURN(10s)";

                        if (_intCount >= (10 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;

                            // 3==>4
                            IsLbOn = false;
                            IsHbOn = false;
                            IsDrlOn = true;
                            IsPlOn = false;
                            IsTurnOn = false;
                            _viulUnLockWelcomeLampReq = 0x00;
                            _viulLockWelcomeLampReq = 0x00;
                            _viulSayhiLampModeSetReq = 0x00;
                            SendCmd();
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 4)
                    {
                        CurrentAnimation = @"DRL(10s)";

                        if (_intCount >= (10 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;

                            // 4==>5
                            IsLbOn = false;
                            IsHbOn = false;
                            IsDrlOn = false;
                            IsPlOn = false;
                            IsTurnOn = false;
                            _viulUnLockWelcomeLampReq = 0x00;
                            _viulLockWelcomeLampReq = 0x00;
                            _viulSayhiLampModeSetReq = 0x00;
                            SendCmd();
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 5)
                    {
                        CurrentAnimation = @"AllOff(1s)";

                        if (_intCount >= (1 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;

                            // 5==>6
                            IsLbOn = false;
                            IsHbOn = false;
                            IsDrlOn = false;
                            IsPlOn = false;
                            IsTurnOn = false;
                            _viulUnLockWelcomeLampReq = 0x01;
                            _viulLockWelcomeLampReq = 0x00;
                            _viulSayhiLampModeSetReq = 0x00;
                            SendCmd();
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    if (_modeCount == 6)
                    {
                        CurrentAnimation = @"迎宾1(8s)";

                        if (_intCount >= (8 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;

                            // 6==>7
                            IsLbOn = false;
                            IsHbOn = false;
                            IsDrlOn = false;
                            IsPlOn = false;
                            IsTurnOn = false;
                            _viulUnLockWelcomeLampReq = 0x02;
                            _viulLockWelcomeLampReq = 0x00;
                            _viulSayhiLampModeSetReq = 0x00;
                            SendCmd();
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 7)
                    {
                        CurrentAnimation = @"迎宾2(6.4s)";

                        if (_intCount >= (6.4 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;

                            // 7==>8
                            IsLbOn = false;
                            IsHbOn = false;
                            IsDrlOn = false;
                            IsPlOn = false;
                            IsTurnOn = false;
                            _viulUnLockWelcomeLampReq = 0x00;
                            _viulLockWelcomeLampReq = 0x01;
                            _viulSayhiLampModeSetReq = 0x00;
                            SendCmd();
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 8)
                    {
                        CurrentAnimation = @"闭锁1(2.88s)";

                        if (_intCount >= (2.88 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;

                            // 8==>9
                            IsLbOn = false;
                            IsHbOn = false;
                            IsDrlOn = false;
                            IsPlOn = false;
                            IsTurnOn = false;
                            _viulUnLockWelcomeLampReq = 0x00;
                            _viulLockWelcomeLampReq = 0x02;
                            _viulSayhiLampModeSetReq = 0x00;
                            SendCmd();
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 9)
                    {
                        CurrentAnimation = @"闭锁2(3s)";

                        if (_intCount >= (3 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;

                            // 9==>10
                            IsLbOn = false;
                            IsHbOn = false;
                            IsDrlOn = false;
                            IsPlOn = false;
                            IsTurnOn = false;
                            _viulUnLockWelcomeLampReq = 0x03;
                            _viulLockWelcomeLampReq = 0x00;
                            _viulSayhiLampModeSetReq = 0x00;
                            SendCmd();
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 10)
                    {
                        CurrentAnimation = @"SayHi1(61.48s)";

                        if (_intCount >= (61.48 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;

                            // 10==>11
                            IsLbOn = false;
                            IsHbOn = false;
                            IsDrlOn = false;
                            IsPlOn = false;
                            IsTurnOn = false;
                            _viulUnLockWelcomeLampReq = 0x04;
                            _viulLockWelcomeLampReq = 0x00;
                            _viulSayhiLampModeSetReq = 0x00;
                            SendCmd();
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 11)
                    {
                        CurrentAnimation = @"SayHi2(62.2s)";

                        if (_intCount >= (62.2 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount++;
                            _intCount = 0;

                            // 11==>12
                            IsLbOn = false;
                            IsHbOn = false;
                            IsDrlOn = false;
                            IsPlOn = false;
                            IsTurnOn = false;
                            _viulUnLockWelcomeLampReq = 0x05;
                            _viulLockWelcomeLampReq = 0x00;
                            _viulSayhiLampModeSetReq = 0x00;
                            SendCmd();
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    else if (_modeCount == 12)
                    {
                        CurrentAnimation = @"SayHi3(61.48s)";

                        if (_intCount >= (61.48 * 1000) / _modeTimer.Interval)
                        {
                            _modeCount = 0;
                            _intCount = 0;

                            // 121==>0
                            IsLbOn = false;
                            IsHbOn = false;
                            IsDrlOn = false;
                            IsPlOn = true;
                            IsTurnOn = false;
                            _viulUnLockWelcomeLampReq = 0x00;
                            _viulLockWelcomeLampReq = 0x00;
                            _viulSayhiLampModeSetReq = 0x00;
                            SendCmd();
                        }
                        else
                        {
                            CurrentAnimationDuration =
                                Math.Round((_intCount * _modeTimer.Interval) / 1000f, 2, MidpointRounding.AwayFromZero)
                                    .ToString(CultureInfo.InvariantCulture) + "S";
                        }
                    }
                    break;

                default:
                    _intCount = 0;
                    CurrentAnimation = string.Empty;
                    CurrentAnimationDuration = string.Empty;
                    return;
            }
        }

        internal enum LightMode
        {
            Normal,

            Mode1,

            Mode2,

            Mode3,

            Mode4,

            Mode5,

            Mode6,

            Mode7,

            Mode8,

            Mode9,

            Mode10,

            Mode11,

            Mode12,

            Mode13,

            Mode14
        }

        [Description("近光开")]
        public void LbOn()
        {
            CurrentLightMode = LightMode.Normal.ToString();
            IsLbOn = true;
            SendCmd();
        }

        [Description("近光关")]
        public void LbOff()
        {
            CurrentLightMode = LightMode.Normal.ToString();
            IsLbOn = false;
            SendCmd();
        }

        [Description("远光开")]
        public void HbOn()
        {
            CurrentLightMode = LightMode.Normal.ToString();
            IsHbOn = true;
            SendCmd();
        }

        [Description("远光关")]
        public void HbOff()
        {
            CurrentLightMode = LightMode.Normal.ToString();
            IsHbOn = false;
            SendCmd();
        }

        [Description("日行灯开")]
        public void DrlOn()
        {
            CurrentLightMode = LightMode.Normal.ToString();
            IsDrlOn = true;
            SendCmd();
        }

        [Description("日行灯关")]
        public void DrlOff()
        {
            CurrentLightMode = LightMode.Normal.ToString();
            IsDrlOn = false;
            SendCmd();
        }

        [Description("位置灯开")]
        public void PlOn()
        {
            CurrentLightMode = LightMode.Normal.ToString();
            IsPlOn = true;
            SendCmd();
        }

        [Description("位置灯关")]
        public void PlOff()
        {
            CurrentLightMode = LightMode.Normal.ToString();
            IsPlOn = false;
            SendCmd();
        }

        [Description("转向灯开")]
        public void TurnOn()
        {
            CurrentLightMode = LightMode.Normal.ToString();
            IsTurnOn = true;
            SendCmd();
        }

        [Description("转向灯L开")]
        public void TurnLOn()
        {
            CurrentLightMode = LightMode.Normal.ToString();
            IsTurnOn = false;
            _viulMusicLampReq = 14;
            SendCmd();
        }

        [Description("转向灯R开")]
        public void TurnROn()
        {
            CurrentLightMode = LightMode.Normal.ToString();
            IsTurnOn = false;
            _viulMusicLampReq = 15;
            SendCmd();
        }

        [Description("转向灯关")]
        public void TurnOff()
        {
            CurrentLightMode = LightMode.Normal.ToString();
            IsTurnOn = false;
            _viulMusicLampReq = 0;
            SendCmd();
        }

        [Description("解锁动态迎宾请求1")]
        public void UnLockWelcomeLampReq1()
        {
            _viulUnLockWelcomeLampReq = 0x01;
            SendCmd();
        }

        [Description("解锁动态迎宾请求2")]
        public void UnLockWelcomeLampReq2()
        {
            _viulUnLockWelcomeLampReq = 0x02;
            SendCmd();
        }

        [Description("解锁动态迎宾请求3")]
        public void UnLockWelcomeLampReq3()
        {
            _viulUnLockWelcomeLampReq = 0x03;
            SendCmd();
        }

        [Description("解锁动态迎宾请求4")]
        public void UnLockWelcomeLampReq4()
        {
            _viulUnLockWelcomeLampReq = 0x04;
            SendCmd();
        }

        [Description("解锁动态迎宾请求5")]
        public void UnLockWelcomeLampReq5()
        {
            _viulUnLockWelcomeLampReq = 0x05;
            SendCmd();
        }

        [Description("闭锁动态迎宾请求1")]
        public void LockWelcomeLampReq1()
        {
            _viulLockWelcomeLampReq = 0x01;
            SendCmd();
        }

        [Description("闭锁动态迎宾请求2")]
        public void LockWelcomeLampReq2()
        {
            _viulLockWelcomeLampReq = 0x02;
            SendCmd();
        }

        [Description("关闭迎宾请求")]
        public void ResetWelcomLampReq()
        {
            _viulLockWelcomeLampReq = 0x00;
            _viulLockWelcomeLampReq = 0x00;
            SendCmd();
        }

        [Description("打开特定模式")]
        public void StartSpecialMode(string index)
        {
            int value;
            if (!string.IsNullOrEmpty(index) && int.TryParse(index, out value) && value >= 1 && value <= 14)
            {
                var modeStr = "Mode" + index;
                var mode = EnumOperater.GetEnumByValue<LightMode>(modeStr);

                IsLbOn = false;
                IsHbOn = false;
                IsTurnOn = false;
                IsDrlOn = false;
                IsPlOn = false;
                _modeCount = 0;

                _viulUnLockWelcomeLampReq = 0x00;
                _viulLockWelcomeLampReq = 0x00;
                _viulSayhiLampModeSetReq = 0x00;
                _viulMusicLampReq = 0x00;

                SendCmd();
                Thread.Sleep(25);

                if (value == 1)
                {
                    IsLbOn = true;
                    IsHbOn = true;
                    IsDrlOn = false;
                    IsPlOn = true;
                    IsTurnOn = true;
                    _viulUnLockWelcomeLampReq = 0x00;
                    _viulLockWelcomeLampReq = 0x00;
                    _viulSayhiLampModeSetReq = 0x00;
                    _viulMusicLampReq = 0x00;
                    SendCmd();
                }
                else if (value == 2)
                {
                    IsLbOn = false;
                    IsHbOn = false;
                    IsDrlOn = false;
                    IsPlOn = false;
                    IsTurnOn = false;
                    _viulUnLockWelcomeLampReq = 0x01;
                    _viulLockWelcomeLampReq = 0x00;
                    _viulSayhiLampModeSetReq = 0x00;
                    _viulMusicLampReq = 0x00;
                    SendCmd();
                }
                else if (value == 3)
                {
                    IsLbOn = false;
                    IsHbOn = false;
                    IsDrlOn = true;
                    IsPlOn = false;
                    IsTurnOn = false;
                    _viulUnLockWelcomeLampReq = 0x00;
                    _viulLockWelcomeLampReq = 0x00;
                    _viulSayhiLampModeSetReq = 0x00;
                    _viulMusicLampReq = 0x00;
                    SendCmd();
                }
                else if (value == 4)
                {
                    IsLbOn = false;
                    IsHbOn = false;
                    IsDrlOn = false;
                    IsPlOn = false;
                    IsTurnOn = false;
                    _viulUnLockWelcomeLampReq = 0x00;
                    _viulLockWelcomeLampReq = 0x00;
                    _viulSayhiLampModeSetReq = 0x00;
                    _viulMusicLampReq = 0x00;
                    SendCmd();
                }
                else if (value == 5)
                {
                    IsLbOn = false;
                    IsHbOn = false;
                    IsDrlOn = false;
                    IsPlOn = false;
                    IsTurnOn = false;
                    _viulUnLockWelcomeLampReq = 0x00;
                    _viulLockWelcomeLampReq = 0x00;
                    _viulSayhiLampModeSetReq = 0x00;
                    _viulMusicLampReq = 0x00;
                    SendCmd();
                }
                else if (value == 6)
                {
                    IsLbOn = false;
                    IsHbOn = false;
                    IsDrlOn = false;
                    IsPlOn = false;
                    IsTurnOn = false;
                    _viulUnLockWelcomeLampReq = 0x01;
                    _viulLockWelcomeLampReq = 0x00;
                    _viulSayhiLampModeSetReq = 0x00;
                    _viulMusicLampReq = 0x00;
                    SendCmd();
                }
                else if (value == 7)
                {
                    IsLbOn = false;
                    IsHbOn = false;
                    IsDrlOn = false;
                    IsPlOn = false;
                    IsTurnOn = false;
                    _viulUnLockWelcomeLampReq = 0x02;
                    _viulLockWelcomeLampReq = 0x00;
                    _viulSayhiLampModeSetReq = 0x00;
                    _viulMusicLampReq = 0x00;
                    SendCmd();
                }
                else if (value == 8)
                {
                    IsLbOn = false;
                    IsHbOn = false;
                    IsDrlOn = false;
                    IsPlOn = false;
                    IsTurnOn = false;
                    _viulUnLockWelcomeLampReq = 0x03;
                    _viulLockWelcomeLampReq = 0x00;
                    _viulSayhiLampModeSetReq = 0x00;
                    _viulMusicLampReq = 0x00;
                    SendCmd();
                }
                else if (value == 9)
                {
                    IsLbOn = true;
                    IsHbOn = true;
                    IsDrlOn = false;
                    IsPlOn = true;
                    IsTurnOn = false;
                    _viulUnLockWelcomeLampReq = 0x00;
                    _viulLockWelcomeLampReq = 0x00;
                    _viulSayhiLampModeSetReq = 0x00;
                    _viulMusicLampReq = 0x00;
                    SendCmd();
                }
                else if (value == 10)
                {
                    IsLbOn = false;
                    IsHbOn = false;
                    IsDrlOn = false;
                    IsPlOn = true;
                    IsTurnOn = false;
                    _viulUnLockWelcomeLampReq = 0x00;
                    _viulLockWelcomeLampReq = 0x00;
                    _viulSayhiLampModeSetReq = 0x00;
                    _viulMusicLampReq = 0x00;
                    SendCmd();
                }
                else if (value == 11)
                {
                    IsLbOn = false;
                    IsHbOn = false;
                    IsDrlOn = false;
                    IsPlOn = false;
                    IsTurnOn = true;
                    _viulUnLockWelcomeLampReq = 0x00;
                    _viulLockWelcomeLampReq = 0x00;
                    _viulSayhiLampModeSetReq = 0x00;
                    _viulMusicLampReq = 0x00;
                    SendCmd();
                }
                else if (value == 12)
                {
                    IsLbOn = true;
                    IsHbOn = false;
                    IsDrlOn = false;
                    IsPlOn = true;
                    IsTurnOn = false;
                    _viulUnLockWelcomeLampReq = 0x00;
                    _viulLockWelcomeLampReq = 0x00;
                    _viulSayhiLampModeSetReq = 0x00;
                    _viulMusicLampReq = 0x00;
                    SendCmd();
                }
                else if (value == 13)
                {
                    IsLbOn = true;
                    IsHbOn = true;
                    IsDrlOn = false;
                    IsPlOn = true;
                    IsTurnOn = false;
                    _viulUnLockWelcomeLampReq = 0x00;
                    _viulLockWelcomeLampReq = 0x00;
                    _viulSayhiLampModeSetReq = 0x00;
                    _viulMusicLampReq = 0x00;
                    SendCmd();
                }
                else
                {
                    IsLbOn = false;
                    IsHbOn = false;
                    IsDrlOn = false;
                    IsPlOn = true;
                    IsTurnOn = false;
                    _viulUnLockWelcomeLampReq = 0x00;
                    _viulLockWelcomeLampReq = 0x00;
                    _viulSayhiLampModeSetReq = 0x00;
                    _viulMusicLampReq = 0x00;
                    SendCmd();
                }

                CurrentLightMode = mode.ToString();
            }
        }

        [Description("关闭特定模式")]
        public void StopSpecialMode()
        {
            IsLbOn = false;
            IsHbOn = false;
            IsTurnOn = false;
            IsDrlOn = false;
            IsPlOn = false;
            _modeCount = 0;

            _viulUnLockWelcomeLampReq = 0x00;
            _viulLockWelcomeLampReq = 0x00;
            _viulSayhiLampModeSetReq = 0x00;

            SendCmd();
            Thread.Sleep(25);
            CurrentLightMode = LightMode.Normal.ToString();
        }

        private void SendCmd()
        {
            if (ControllerMaster != null && IsConnected)
            {
                try
                {
                    var sendBytes = new byte[]
                    {
                        0x55,0xAA, 0x55, 0xAA,
                        0x00,0x0E,
                        0x01,
                        0x50,
                        0xFF,
                        IsLbOn?(byte)0x01:(byte)0x00,
                        IsHbOn ? (byte)0x01 : (byte)0x00,
                        IsPlOn? (byte)0x01 : (byte)0x00,
                        IsDrlOn? (byte)0x01 : (byte)0x00,
                        IsTurnOn? (byte)0x03 : (byte)0x00,
                        _viulUnLockWelcomeLampReq,
                        _viulLockWelcomeLampReq,
                        _viulSayhiLampModeSetReq,
                        _viulMusicLampReq,
                        0x5A,0x5A
                    };

                    ControllerMaster.SpecialCmd(sendBytes);

                    Thread.Sleep(1);

                    sendBytes = new byte[]
                    {
                        0x55,0xAA, 0x55, 0xAA,
                        0x00,0x0E,
                        0x01,
                        0x50,
                        0x01,
                        IsLbOn?(byte)0x01:(byte)0x00,
                        IsHbOn ? (byte)0x01 : (byte)0x00,
                        IsPlOn? (byte)0x01 : (byte)0x00,
                        IsDrlOn? (byte)0x01 : (byte)0x00,
                        IsTurnOn? (byte)0x03 : (byte)0x00,
                        _viulUnLockWelcomeLampReq,
                        _viulLockWelcomeLampReq,
                        _viulSayhiLampModeSetReq,
                        _viulMusicLampReq,
                        0x5A,0x5A
                    };
                    ControllerMaster.SpecialCmd(sendBytes);
                }
                catch (Exception e)
                {
                    // ignored
                }
            }

            //if (MyUdpClient != null && IsConnected)
            //{
            //    var sendBytes = new byte[]
            //    {
            //        0x55,0xAA, 0x55, 0xAA,
            //        0x00,0x0E,
            //        0x01,
            //        0x50,
            //        0xFF,
            //        IsLbOn?(byte)0x01:(byte)0x00,
            //        IsHbOn ? (byte)0x01 : (byte)0x00,
            //        IsPlOn? (byte)0x01 : (byte)0x00,
            //        IsDrlOn? (byte)0x01 : (byte)0x00,
            //        IsTurnOn? (byte)0x03 : (byte)0x00,
            //        _viulUnLockWelcomeLampReq,
            //        _viulLockWelcomeLampReq,
            //        _viulSayhiLampModeSetReq,
            //        _viulMusicLampReq,
            //        0x5A,0x5A
            //    };
            //    MyUdpClient.SendMsgTo(
            //        new IPEndPoint(RemoteIpAddress, RemotePort), sendBytes.ToArray(), 10);

            //    Thread.Sleep(1);

            //    sendBytes = new byte[]
            //    {
            //        0x55,0xAA, 0x55, 0xAA,
            //        0x00,0x0E,
            //        0x01,
            //        0x50,
            //        0x01,
            //        IsLbOn?(byte)0x01:(byte)0x00,
            //        IsHbOn ? (byte)0x01 : (byte)0x00,
            //        IsPlOn? (byte)0x01 : (byte)0x00,
            //        IsDrlOn? (byte)0x01 : (byte)0x00,
            //        IsTurnOn? (byte)0x03 : (byte)0x00,
            //        _viulUnLockWelcomeLampReq,
            //        _viulLockWelcomeLampReq,
            //        _viulSayhiLampModeSetReq,
            //        _viulMusicLampReq,
            //        0x5A,0x5A
            //    };
            //    MyUdpClient.SendMsgTo(
            //        new IPEndPoint(RemoteIpAddress, RemotePort), sendBytes.ToArray(), 10);
            //}
        }
    }
}
