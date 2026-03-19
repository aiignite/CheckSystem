using CommonUtility;
using CommonUtility.FileOperator;
using Controller;
using CsvHelper.Configuration.Attributes;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckSystem.HelperForms.AirOutlet
{
    public partial class MyWndLoadBox : UIForm
    {
        private int[,] DutCount;
        private const int _maxDutCount = 32;
        private BackgroundWorker[,] RefreshBackgroundWorker = new BackgroundWorker[2, 32];
        private Thread[,] _thBackRunThread = new Thread[2, 32];
        private AirOutletAutomotiveActuator[,] _airOutletAutomotiveActuator = new AirOutletAutomotiveActuator[2, 1];
        private ControllerBase _linController;
        private DutInfo[,] _dutInfos = new DutInfo[2, _maxDutCount];
        private CsvFileHelper<SlaveLogData>[,] _logWriter = new CsvFileHelper<SlaveLogData>[2, _maxDutCount];
        private int _boardType = 0;
        private ushort _offsetNegative = Reg2Offset(30);
        private ushort _offsetPositive = Reg2Offset(30);
        private int _bCycleMaxCount = 900;
        private int _bPeriodMaxCount = 700;
        private int _bDefaultSpeed = 4;
        private int _bDefaultStartPos = 32000;
        private int _bDefaultCycleDelay = 100;
        private int _bMonitorStall = 1;

        CancellationTokenSource _cts = new CancellationTokenSource();

        private UIPanel[,] _topPanel = new UIPanel[2, 1];
        private UIPanel[,] _bottomPanel = new UIPanel[2, 1];

        // 上半部分控件 - 通道参数表格 
        private UIMarkLabel[,] lblRowHeaders = new UIMarkLabel[2, _maxDutCount];
        private UIMarkLabel[,] _lblNad = new UIMarkLabel[2, _maxDutCount];
        private UIMarkLabel[,] _lblCid = new UIMarkLabel[2, _maxDutCount];
        private UIMarkLabel[,] _lblSid = new UIMarkLabel[2, _maxDutCount];
        private UIMarkLabel[,] _lblActualPos = new UIMarkLabel[2, _maxDutCount];
        private UIMarkLabel[,] _lblTargetPos = new UIMarkLabel[2, _maxDutCount];
        private UIMarkLabel[,] _lblDirection = new UIMarkLabel[2, _maxDutCount];
        private UIMarkLabel[,] _lblSpeed = new UIMarkLabel[2, _maxDutCount];
        private UIMarkLabel[,] _lblStallState = new UIMarkLabel[2, _maxDutCount];
        private UIMarkLabel[,] _lblCurrentState = new UIMarkLabel[2, _maxDutCount];
        private UIMarkLabel[,] _lblCycleCount = new UIMarkLabel[2, _maxDutCount];
        private UIMarkLabel[,] _lblPeriodCount = new UIMarkLabel[2, _maxDutCount];
        private UIMarkLabel[,] _lblAutoCaliCount = new UIMarkLabel[2, _maxDutCount];
        private UIButton[,] _btnClear = new UIButton[2, _maxDutCount];
        private UIButton[,] _btnStartStop = new UIButton[2, _maxDutCount];

        // 下半部分控件 - 循环模式参数
        private UIMarkLabel[] lblBottomRowHeaders;
        private NumericUpDown nudBoardType;
        private NumericUpDown nudSpeed, nudInitPos;
        private NumericUpDown nudCycleCount, nudCycleDelay;
        private NumericUpDown nudPeriodCount;
        private UIButton btnParaReset, btnParaInvoke, btnParaSave;
        private NumericUpDown nudIsMonitorStall;

        private UIMarkLabel[] lblBottomRowHeaders2;
        private NumericUpDown nudOffsetNegative, nudOffsetPositive;
        private NumericUpDown nudLin1Count;
        private NumericUpDown nudLin2Count;

        private UIMarkLabel[,] _lblBottomRowHeaders2 = new UIMarkLabel[2, _maxDutCount];
        private UIMarkLabel[,] _lblBottomNad = new UIMarkLabel[2, _maxDutCount];
        private UIMarkLabel[,] _lblBottomSend = new UIMarkLabel[2, _maxDutCount];
        private UIMarkLabel[,] _lblBottomRecv = new UIMarkLabel[2, _maxDutCount];
        private UIMarkLabel[,] _lblBottomCaliA = new UIMarkLabel[2, _maxDutCount];
        private UIMarkLabel[,] _lblBottomCaliB = new UIMarkLabel[2, _maxDutCount];
        private UIMarkLabel[,] _lblBottomMode = new UIMarkLabel[2, _maxDutCount];

        private UIMarkLabel[,] _lblBottomRErr = new UIMarkLabel[2, _maxDutCount];
        private UIMarkLabel[,] _lblBottomOT = new UIMarkLabel[2, _maxDutCount];
        private UIMarkLabel[,] _lblBottomEleD = new UIMarkLabel[2, _maxDutCount];
        private UIMarkLabel[,] _lblBottomSupL = new UIMarkLabel[2, _maxDutCount];
        private UIMarkLabel[,] _lblBottomEmeRO = new UIMarkLabel[2, _maxDutCount];
        private UIMarkLabel[,] _lblBottomRes = new UIMarkLabel[2, _maxDutCount];
        private UIMarkLabel[,] _lblBottomStallDR = new UIMarkLabel[2, _maxDutCount];

        internal enum DutState
        {
            [Description("等待开始")]
            Idle,

            [Description("寻找堵转点")]
            FindStall,

            [Description("循环动作")]
            RunCycle,
        };

        public class SlaveLogData
        {
            [Name("时间戳")]
            public string Ts { get; set; }

            [Name("模式")]
            public string Mode { get; set; }

            [Name("发送数据")]
            public string SendData { get; set; }

            [Name("接收数据")]
            public string RecvData { get; set; }

            [Name("循环计数")]
            public int CycleCount { get; set; }

            [Name("周期计数")]
            public int PeriodCount { get; set; }

            [Name("自学习计数")]
            public int CaliCount { get; set; }

            [Name("堵转点A")]
            public ushort CaliA { get; set; }

            [Name("堵转点B")]
            public ushort CaliB { get; set; }
        }

        internal class DutInfo
        {
            public byte SID { get; set; }
            public byte CID { get; set; }
            public byte NAD { get; set; }
            public DutState State { get; set; }

            public byte BRes { get; set; }
            public bool IsLinLoss { get; set; }

            public AirOutletAutomotiveActuator.Slave Slave { get; set; }
            public CancellationTokenSource _cts = new CancellationTokenSource();

            public int CycleCount { get; set; }
            public int PeriodCount { get; set; }
            public int CaliCount { get; set; }
        }

        public MyWndLoadBox()
        {
            InitializeComponent();
            Load += MyWndLoadBox_Load;
            FormClosing += MyWndLoadBox_FormClosing;
        }

        private void MyWndLoadBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!UIMessageBox.ShowAsk("确定要退出吗"))
            {
                UIMessageTip.Show("取消退出");
                e.Cancel = true;
                return;
            }

            foreach (var dut in _dutInfos)
            {
                dut?._cts?.Cancel();
                dut?._cts?.Dispose();
                //dut._cts = null;
            }

            for (var ch = 0; ch < DutCount.GetLength(0); ch++)
            {
                if (DutCount[ch, 0] is 0)
                    continue;

                for (var dutIndex = 0; dutIndex < DutCount[ch, 0]; dutIndex++)
                    RefreshBackgroundWorker[ch, dutIndex]?.CancelAsync();
            }

            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;

            m_WorkThread?.Abort();
            m_WorkThread?.Join();
            m_WorkThread = null;

            _linController?.Dispose();
        }

        private void MyWndLoadBox_Load(object sender, EventArgs e)
        {
            LoadParaFromIni();
            InitPara();
            InitTopPanel();
            InitBottomPanel();
            ResizeUI();
            ResizeTopPanel();
            ResizeBottomPanel();
            for (var ch = 0; ch < DutCount.GetLength(0); ch++)
            {
                _topPanel[ch, 0].SizeChanged += (ss, ee) => ResizeTopPanel();
                _bottomPanel[ch, 0].SizeChanged += (ss, ee) => ResizeBottomPanel();
            }
            Resize += (ss, ee) => ResizeUI();

            if (_boardType == 1)
            {
                _linController = new SyControllerWith56Pin("56pin_lin控制器");
                ((SyControllerWith56Pin)_linController).InitRemoteIpAddress("192.168.1.28:8088");

                for (var ch = 0; ch < DutCount.GetLength(0); ch++)
                {
                    _airOutletAutomotiveActuator[ch, 0] = new AirOutletAutomotiveActuator("出风口执行器_LIN" + (ch + 1));
                    if (DutCount[ch, 0] is 0 || ch is 1)
                        continue;

                    _airOutletAutomotiveActuator[ch, 0].Lin = ((SyControllerWith56Pin)_linController).GatewayLin;
                }
            }
            else
            {
                _linController = new SyRenesasMcuControllerMaster("瑞萨_lin控制器");
                ((SyRenesasMcuControllerMaster)_linController).InitRemoteIpAddress("192.168.1.28:8088");

                for (var ch = 0; ch < DutCount.GetLength(0); ch++)
                {
                    _airOutletAutomotiveActuator[ch, 0] = new AirOutletAutomotiveActuator("出风口执行器_LIN" + (ch + 1));
                    if (DutCount[ch, 0] is 0)
                        continue;
                    _airOutletAutomotiveActuator[ch, 0].Lin = ch == 0 ? ((SyRenesasMcuControllerMaster)_linController).GatewayLin1 : ((SyRenesasMcuControllerMaster)_linController).GatewayLin2;
                }
            }

            for (var ch = 0; ch < DutCount.GetLength(0); ch++)
            {
                if (DutCount[ch, 0] is 0)
                    continue;

                for (var d = 0; d < DutCount[ch, 0]; d++)
                {
                    var i = d + 1;
                    var nad = (byte)((0x20 + DutCount[ch, 0]) - (i - 0));
                    var cid = (byte)(DutCount[ch, 0] - (i - 0));
                    _dutInfos[ch, d] = new DutInfo
                    {
                        CID = cid,
                        NAD = nad,
                        SID = (byte)(cid + 0x10)
                    };
                    _airOutletAutomotiveActuator[ch, 0].AddSlave(ValueHelper.GetHextStrWithOx(cid), ValueHelper.GetHextStrWithOx(nad));
                    SetDefaultValue(ch, nad);
                    _airOutletAutomotiveActuator[ch, 0].Auto_Speed(nad);

                    var slaveIndex = _airOutletAutomotiveActuator[ch, 0]._slaves.FindIndex(f => f.BindingNad == nad);
                    _dutInfos[ch, d].Slave = _airOutletAutomotiveActuator[ch, 0]._slaves[slaveIndex];
                    _dutInfos[ch, d].CycleCount = _cycleRead[ch, d];
                    _dutInfos[ch, d].PeriodCount = _periodRead[ch, d];

                    _lblNad[ch, d].Text = ValueHelper.GetHextStrWithOx(_dutInfos[ch, d].NAD);
                    _lblCid[ch, d].Text = ValueHelper.GetHextStrWithOx(_dutInfos[ch, d].CID);
                    _lblSid[ch, d].Text = ValueHelper.GetHextStrWithOx(_dutInfos[ch, d].SID);

                    RefreshBackgroundWorker[ch, d] = new BackgroundWorker();
                    RefreshBackgroundWorker[ch, d].DoWork += RefreshBackgroundWorker_DoWork;
                    RefreshBackgroundWorker[ch, d].WorkerSupportsCancellation = true;
                    RefreshBackgroundWorker[ch, d].RunWorkerAsync(new int[] { ch, d });
                    //_logWriter[ch, d]?.Start();
                }

                _airOutletAutomotiveActuator[ch, 0].StartLin();
            }

            //CreateWorkder();
        }

        private string _logDir = "AirOutletTestLog";
        private string _setupLog = "TestSetup.ini";
        private IniFileHelper _ini;
        private int[,] _cycleRead = new int[2, _maxDutCount];
        private int[,] _periodRead = new int[2, _maxDutCount];
        private readonly object _lockIni = new object();

        private void LoadParaFromIni()
        {
            var rootDir = Path.GetPathRoot(Program.SysDir);
            var dir = string.Format(@"{0}\{1}", rootDir, _logDir);
            var isDirExist = false;
            var isIniExist = false;
            if (!Directory.Exists(dir))
            {
                try
                {
                    Directory.CreateDirectory(dir);
                    isDirExist = true;
                }
                catch (Exception)
                {
                    isDirExist = false;
                }
            }
            else
            {
                isDirExist = true;
            }

            if (isDirExist)
            {
                if (!File.Exists(string.Format(@"{0}\{1}", dir, _setupLog)))
                {
                    try
                    {
                        var ini = string.Format(@"{0}\{1}", dir, _setupLog);
                        using (var fs = new FileStream(ini, FileMode.Append, FileAccess.Write, FileShare.None))
                        {
                            var list = new List<string>();
                            list.Clear();
                            list.Add("[Sys]");
                            list.Add("BoardType=" + _boardType);
                            //list.Add("Offset=" + _offset);

                            list.Add("OffsetNegative=" + _offsetNegative);
                            list.Add("OffsetPositive=" + _offsetPositive);

                            list.Add("CycleMaxCount=" + _bCycleMaxCount);
                            list.Add("PeriodMaxCount=" + _bPeriodMaxCount);
                            list.Add("DefaultSpeed=" + _bDefaultSpeed);
                            list.Add("DefaultStartPos=" + _bDefaultStartPos);
                            list.Add("DefaultCycleDelay=" + _bDefaultCycleDelay);
                            list.Add("IsMonitorStall=" + _bMonitorStall);
                            list.Add("Lin1DutCount=" + 8);
                            list.Add("Lin1DutCount=" + 8);

                            for (var i = 0; i < _maxDutCount; i++)
                            {
                                var dutCycle = string.Format("Dut{0}Cycle", i + 1);
                                var dutPeriod = string.Format("Dut{0}Period", i + 1);
                                list.Add("" + dutCycle + "=" + 0);
                                list.Add("" + dutPeriod + "=" + 0);
                            }

                            for (var i = 0; i < _maxDutCount; i++)
                            {
                                var dutCycle = string.Format("Lin2Dut{0}Cycle", i + 1);
                                var dutPeriod = string.Format("Lin2Dut{0}Period", i + 1);
                                list.Add("" + dutCycle + "=" + 0);
                                list.Add("" + dutPeriod + "=" + 0);
                            }

                            foreach (var item in list)
                            {
                                var tp = item + "\r\n";
                                fs.Write(Encoding.Default.GetBytes(tp), 0, tp.Length);//写数据
                            }
                        }

                        isIniExist = true;
                    }
                    catch (Exception)
                    {
                        isIniExist = false;
                    }
                }
                else
                {
                    isIniExist = true;
                }
            }

            if (isIniExist)
            {
                try
                {
                    _ini = new IniFileHelper(string.Format(@"{0}\{1}", dir, _setupLog));
                    var t1 = ushort.Parse(_ini.IniReadValue("Sys", "Offset"));
                    var t2 = int.Parse(_ini.IniReadValue("Sys", "CycleMaxCount"));
                    var t3 = int.Parse(_ini.IniReadValue("Sys", "PeriodMaxCount"));
                    var t4 = int.Parse(_ini.IniReadValue("Sys", "DefaultSpeed"));
                    var t5 = int.Parse(_ini.IniReadValue("Sys", "DefaultStartPos"));
                    var t6 = int.Parse(_ini.IniReadValue("Sys", "DefaultCycleDelay"));
                    var t7 = int.Parse(_ini.IniReadValue("Sys", "BoardType"));
                    var t8 = int.Parse(_ini.IniReadValue("Sys", "IsMonitorStall"));

                    var lin1Count = 8;
                    var lin2Count = 8;

                    try
                    {
                        lin1Count = int.Parse(_ini.IniReadValue("Sys", "Lin1DutCount"));
                    }
                    catch (Exception)
                    {
                        lin1Count = 8;
                        var ini = string.Format(@"{0}\{1}", dir, _setupLog);
                        using (var fs = new FileStream(ini, FileMode.Append, FileAccess.Write, FileShare.None))
                        {
                            var tp = "" + "Lin1DutCount" + "=" + lin1Count + "\r\n";
                            fs.Write(Encoding.Default.GetBytes(tp), 0, tp.Length);//写数据
                        }
                    }

                    try
                    {
                        lin2Count = int.Parse(_ini.IniReadValue("Sys", "Lin2DutCount"));
                    }
                    catch (Exception)
                    {
                        lin2Count = 8;
                        var ini = string.Format(@"{0}\{1}", dir, _setupLog);
                        using (var fs = new FileStream(ini, FileMode.Append, FileAccess.Write, FileShare.None))
                        {
                            var tp = "" + "Lin2DutCount" + "=" + lin1Count + "\r\n";
                            fs.Write(Encoding.Default.GetBytes(tp), 0, tp.Length);//写数据
                        }
                    }

                    try
                    {
                        _offsetNegative = ushort.Parse(_ini.IniReadValue("Sys", "OffsetNegative"));
                    }
                    catch (Exception)
                    {
                        _offsetNegative = Reg2Offset(30);
                        var ini = string.Format(@"{0}\{1}", dir, _setupLog);
                        using (var fs = new FileStream(ini, FileMode.Append, FileAccess.Write, FileShare.None))
                        {
                            var tp = "" + "OffsetNegative" + "=" + _offsetNegative + "\r\n";
                            fs.Write(Encoding.Default.GetBytes(tp), 0, tp.Length);//写数据
                        }
                    }

                    try
                    {
                        _offsetPositive = ushort.Parse(_ini.IniReadValue("Sys", "OffsetPositive"));
                    }
                    catch (Exception)
                    {
                        _offsetPositive = Reg2Offset(30);
                        var ini = string.Format(@"{0}\{1}", dir, _setupLog);
                        using (var fs = new FileStream(ini, FileMode.Append, FileAccess.Write, FileShare.None))
                        {
                            var tp = "" + "OffsetPositive" + "=" + _offsetPositive + "\r\n";
                            fs.Write(Encoding.Default.GetBytes(tp), 0, tp.Length);//写数据
                        }
                    }

                    DutCount = new int[2, 1];
                    DutCount[0, 0] = lin1Count;
                    DutCount[1, 0] = lin2Count;

                    var cycleTp = new int[2, _maxDutCount];
                    var periodTp = new int[2, _maxDutCount];

                    for (var l = 0; l < 2; l++)
                    {
                        if (DutCount[l, 0] is 0)
                            continue;

                        for (var i = 0; i < DutCount[l, 0]; i++)
                        {
                            if (l == 0)
                            {
                                var dutCycle = string.Format("Dut{0}Cycle", i + 1);
                                var dutPeriod = string.Format("Dut{0}Period", i + 1);

                                try
                                {
                                    cycleTp[l, i] = int.Parse(_ini.IniReadValue("Sys", dutCycle));
                                }
                                catch (Exception)
                                {
                                    var ini = string.Format(@"{0}\{1}", dir, _setupLog);
                                    using (var fs = new FileStream(ini, FileMode.Append, FileAccess.Write, FileShare.None))
                                    {
                                        var tp = "" + dutCycle + "=" + 0 + "\r\n";
                                        fs.Write(Encoding.Default.GetBytes(tp), 0, tp.Length);//写数据
                                    }
                                }

                                try
                                {
                                    periodTp[l, i] = int.Parse(_ini.IniReadValue("Sys", dutPeriod));
                                }
                                catch (Exception)
                                {
                                    var ini = string.Format(@"{0}\{1}", dir, _setupLog);
                                    using (var fs = new FileStream(ini, FileMode.Append, FileAccess.Write, FileShare.None))
                                    {
                                        var tp = "" + dutPeriod + "=" + 0 + "\r\n";
                                        fs.Write(Encoding.Default.GetBytes(tp), 0, tp.Length);//写数据
                                    }
                                }
                            }
                            else if (l == 1)
                            {
                                var dutCycleLin2 = string.Format("Lin2Dut{0}Cycle", i + 1);
                                var dutPeriodLin2 = string.Format("Lin2Dut{0}Period", i + 1);

                                try
                                {
                                    cycleTp[l, i] = int.Parse(_ini.IniReadValue("Sys", dutCycleLin2));
                                }
                                catch (Exception)
                                {
                                    var ini = string.Format(@"{0}\{1}", dir, _setupLog);
                                    using (var fs = new FileStream(ini, FileMode.Append, FileAccess.Write, FileShare.None))
                                    {
                                        var tp = "" + dutCycleLin2 + "=" + 0 + "\r\n";
                                        fs.Write(Encoding.Default.GetBytes(tp), 0, tp.Length);//写数据
                                    }
                                }

                                try
                                {
                                    periodTp[l, i] = int.Parse(_ini.IniReadValue("Sys", dutPeriodLin2));
                                }
                                catch (Exception)
                                {
                                    var ini = string.Format(@"{0}\{1}", dir, _setupLog);
                                    using (var fs = new FileStream(ini, FileMode.Append, FileAccess.Write, FileShare.None))
                                    {
                                        var tp = "" + dutPeriodLin2 + "=" + 0 + "\r\n";
                                        fs.Write(Encoding.Default.GetBytes(tp), 0, tp.Length);//写数据
                                    }
                                }
                            }
                        }
                    }

                    //_offset = t1;
                    _bCycleMaxCount = t2;
                    _bPeriodMaxCount = t3;
                    _bDefaultSpeed = t4;
                    _bDefaultStartPos = t5;
                    _bDefaultCycleDelay = t6;
                    _boardType = t7;
                    _bMonitorStall = t8;

                    for (var l = 0; l < 2; l++)
                    {
                        if (DutCount[l, 0] is 0)
                            continue;

                        for (var i = 0; i < DutCount[l, 0]; i++)
                        {
                            _periodRead[l, i] = periodTp[l, i];
                            _cycleRead[l, i] = cycleTp[l, i];
                        }
                    }
                }
                catch (Exception ex)
                {
                    UIMessageBox.ShowError("读取参数配置文件失败，将使用默认参数：" + ex.Message);
                }
            }

            if (isDirExist)
            {
                for (var j = 0; j < _logWriter.GetLength(0); j++)
                {
                    if (DutCount[j, 0] is 0)
                        continue;

                    for (var i = 0; i < DutCount[j, 0]; i++)
                    {
                        try
                        {
                            var logDir = string.Format(@"{0}\Lin{1}_DUT{2}", dir, j + 1, i + 1);
                            if (!Directory.Exists(logDir))
                                Directory.CreateDirectory(logDir);
                            _logWriter[j, i] = new CsvFileHelper<SlaveLogData>(logDir);
                        }
                        catch (Exception)
                        {
                            _logWriter[j, i] = null;
                        }
                    }
                }
            }
        }

        private void InitPara()
        {
            _topPanel[0, 0] = panelMainCycle;
            _topPanel[1, 0] = panelMainCycle2;

            _bottomPanel[0, 0] = panelCyclePara;
            _bottomPanel[1, 0] = panelCyclePara2;
        }

        private void SetDefaultValue(int ch, byte nad)
        {
            _airOutletAutomotiveActuator[ch, 0].Stop_Mode(nad);
            _airOutletAutomotiveActuator[ch, 0].Start_position_valid(nad);
            _airOutletAutomotiveActuator[ch, 0].Sta_Pos_65535_Signal(nad, (ushort)_bDefaultStartPos);
            _airOutletAutomotiveActuator[ch, 0].Stall_detection_on(nad);
            switch (_bDefaultSpeed)
            {
                case 1:
                    _airOutletAutomotiveActuator[ch, 0].Speed_level_1__2_25_rpm(nad);
                    break;

                case 2:
                    _airOutletAutomotiveActuator[ch, 0].Speed_level_2_2_25_rpm_LessThan_x__3_0_rpm(nad);
                    break;

                case 3:
                    _airOutletAutomotiveActuator[ch, 0].Speed_level_3_3_0_rpm_LessThan_x__4_0_rpm(nad);
                    break;

                case 4:
                    _airOutletAutomotiveActuator[ch, 0].Speed_level_4_4_0_rpm_LessThan_x(nad);
                    break;

                case 5:
                    _airOutletAutomotiveActuator[ch, 0].Auto_Speed(nad);
                    break;

                default:
                    break;
            }
        }

        private async void CreateWorkder()
        {
            //m_WorkThread?.Abort();
            //m_WorkThread?.Join();

            //m_WorkThread = new Thread(Worker) { IsBackground = true };
            //m_WorkThread.Start();

            //WindowState = FormWindowState.Minimized;
            toolStrip1.Enabled = panelMainCycle.Enabled = panelMainCycle2.Enabled = panelCyclePara.Enabled = panelCyclePara2.Enabled = btnAutomaticAddressing.Enabled = false;
            await Task.Delay(500);
            this.ShowWaitForm("正在启动......");
            //this.ShowProcessForm(200);
            //await Task.WhenAll(t);
            //Worker();
            this.HideWaitForm();
            //this.HideProcessForm();
            toolStrip1.Enabled = panelMainCycle.Enabled = panelMainCycle2.Enabled = panelCyclePara.Enabled = panelCyclePara2.Enabled = btnAutomaticAddressing.Enabled = true;
            //WindowState = FormWindowState.Maximized;
            //Location = new Point(0 + 5, 0 + 5);

            await Task.Run(async () =>
            {
                while (_cts != null)
                {
                    var showStr = "是否循环中监控堵转信号：" + (_bMonitorStall == 1 ? "是" : "否");

                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            if (toolStripLabel1.Text != showStr)
                                toolStripLabel1.Text = showStr;
                        }));
                    }
                    else
                    {
                        if (toolStripLabel1.Text != showStr)
                            toolStripLabel1.Text = showStr;
                    }

                    await Task.Delay(500);
                }
            }, _cts.Token);
        }

        private void Worker()
        {
            //await Task.Delay(50);         

            for (var l = 0; l < DutCount.GetLength(0); l++)
            {
                if (DutCount[l, 0] is 0)
                    continue;

                for (var dutIndex = 0; dutIndex < DutCount[l, 0]; dutIndex++)
                {
                    //await WorkTask(l, dutIndex);
                    //await WorkThreadProc(l, dutIndex);
                    RefreshBackgroundWorker[l, dutIndex] = new BackgroundWorker();
                    RefreshBackgroundWorker[l, dutIndex].DoWork += RefreshBackgroundWorker_DoWork;
                    RefreshBackgroundWorker[l, dutIndex].RunWorkerAsync(new int[] { l, dutIndex });
                }
            }
        }

        private void RefreshBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;
            int[] args = (int[])e.Argument;
            Console.WriteLine("ch{0}_dut{1} start", args[0], args[1]);

            var ch = args[0];
            var dutIndex = args[1];

            if (_thBackRunThread[ch, dutIndex] != null)
            {
                _thBackRunThread[ch, dutIndex].Abort();
                _thBackRunThread[ch, dutIndex].Join();
            }

            _thBackRunThread[ch, dutIndex] = new Thread(() => { WorkTask(ch, dutIndex); }) { IsBackground = true };
            _thBackRunThread[ch, dutIndex].Start();

            //WorkTask(ch, dutIndex);
            RefreshWork(HighPrecisionTimer.GetTimestamp(), ch, dutIndex, true);

            while (!worker.CancellationPending)
            {
                Thread.Sleep(50);

                var slave = _dutInfos[ch, dutIndex].Slave;
                var nowTs = HighPrecisionTimer.GetTimestamp();
                if (slave != null)
                {
                    var isLossLin = HighPrecisionTimer.GetTimestampIntervalMs(_dutInfos[ch, dutIndex].Slave.RecvTs, nowTs) > 3850;

                    //if (HighPrecisionTimer.GetTimestampIntervalMs(_refreshTs[ch, dutIndex], nowTs) >= 250)
                    RefreshWork(nowTs, ch, dutIndex, isLossLin);

                    if (_dutInfos[ch, dutIndex].State != DutState.Idle && _lblCurrentState[ch, dutIndex].Text != "等待开始" && HighPrecisionTimer.GetTimestampIntervalMs(_logTs[ch, dutIndex], nowTs) > 1000)
                    {
                        _logTs[ch, dutIndex] = nowTs;

                        var logData = new SlaveLogData
                        {
                            Ts = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"),
                            Mode = _lblCurrentState[ch, dutIndex].Text,
                            SendData = ValueHelper.GetHextStrWithOx(slave.SendBytes),
                            RecvData = isLossLin ? string.Empty : ValueHelper.GetHextStrWithOx(slave.RecvBytes),
                            CycleCount = _dutInfos[ch, dutIndex].CycleCount,
                            PeriodCount = _dutInfos[ch, dutIndex].PeriodCount,
                            CaliCount = _dutInfos[ch, dutIndex].CaliCount,
                            CaliA = slave.CaliStallA,
                            CaliB = slave.CaliStallB,
                        };
                        _logWriter[ch, dutIndex]?.EnqueueData(logData);
                    }
                }
            }
        }

        private void RefreshWork(long nowTs, int ch, int dutIndex, bool isLossLin)
        {
            _refreshTs[ch, dutIndex] = nowTs;

            var slave = _dutInfos[ch, dutIndex].Slave;

            UpdateLbl(ch, _lblTargetPos[ch, dutIndex], slave.TargetPos.ToString());
            UpdateLbl(ch, _lblCycleCount[ch, dutIndex], string.Format("{0}/{1}", _dutInfos[ch, dutIndex].CycleCount.ToString(), _bCycleMaxCount));
            UpdateLbl(ch, _lblPeriodCount[ch, dutIndex], string.Format("{0}/{1}", _dutInfos[ch, dutIndex].PeriodCount.ToString(), _bPeriodMaxCount));
            UpdateLbl(ch, _lblAutoCaliCount[ch, dutIndex], _dutInfos[ch, dutIndex].CaliCount.ToString());
            UpdateLbl(ch, _lblBottomNad[ch, dutIndex], ValueHelper.GetHextStrWithOx(_dutInfos[ch, dutIndex].NAD));
            UpdateLbl(ch, _lblBottomSend[ch, dutIndex], _dutInfos[ch, dutIndex].Slave.SendBytes is null ? string.Empty : ValueHelper.GetHextStr(_dutInfos[ch, dutIndex].Slave.SendBytes).Replace(" ", ""));
            UpdateLbl(ch, _lblBottomStallDR[ch, dutIndex], _stall_d_r.ContainsKey(slave.Stall_D_R) ? _stall_d_r[slave.Stall_D_R] : ValueHelper.GetHextStrWithOx(slave.Stall_D_R));
            if (isLossLin)
            {
                UpdateLbl(ch, _lblBottomRecv[ch, dutIndex], string.Empty);
                UpdateLbl(ch, _lblBottomMode[ch, dutIndex], string.Empty);
                UpdateLbl(ch, _lblActualPos[ch, dutIndex], string.Empty);
                UpdateLbl(ch, _lblDirection[ch, dutIndex], string.Empty);
                UpdateLbl(ch, _lblSpeed[ch, dutIndex], string.Empty);
                UpdateLbl(ch, _lblStallState[ch, dutIndex], string.Empty);

                UpdateLbl(ch, _lblBottomRErr[ch, dutIndex], string.Empty);
                UpdateLbl(ch, _lblBottomOT[ch, dutIndex], string.Empty);
                UpdateLbl(ch, _lblBottomEleD[ch, dutIndex], string.Empty);
                UpdateLbl(ch, _lblBottomSupL[ch, dutIndex], string.Empty);
                UpdateLbl(ch, _lblBottomEmeRO[ch, dutIndex], string.Empty);
                UpdateLbl(ch, _lblBottomRes[ch, dutIndex], string.Empty);

                _dutInfos[ch, dutIndex].BRes = 1;
            }
            else
            {
                UpdateLbl(ch, _lblBottomRecv[ch, dutIndex], _dutInfos[ch, dutIndex].Slave.RecvBytes is null ? string.Empty : ValueHelper.GetHextStr(_dutInfos[ch, dutIndex].Slave.RecvBytes).Replace(" ", ""));
                UpdateLbl(ch, _lblBottomMode[ch, dutIndex], ValueHelper.GetHextStrWithOx(_dutInfos[ch, dutIndex].Slave.Mode));
                UpdateLbl(ch, _lblActualPos[ch, dutIndex], slave.ActualPos.ToString());
                UpdateLbl(ch, _lblDirection[ch, dutIndex], _rot_dir.ContainsKey(slave.Rot_Dir) ? _rot_dir[slave.Rot_Dir] : ValueHelper.GetHextStrWithOx(slave.Rot_Dir));
                UpdateLbl(ch, _lblSpeed[ch, dutIndex], _spe_s.ContainsKey(slave.Spe_S) ? _spe_s[slave.Spe_S] : ValueHelper.GetHextStrWithOx(slave.Spe_S));
                UpdateLbl(ch, _lblStallState[ch, dutIndex], _stall_o.ContainsKey(slave.Stall_O) ? _stall_o[slave.Stall_O] : ValueHelper.GetHextStrWithOx(slave.Stall_O));

                UpdateLbl(ch, _lblBottomRErr[ch, dutIndex], _r_err.ContainsKey(slave.R_Err) ? _r_err[slave.R_Err] : ValueHelper.GetHextStrWithOx(slave.R_Err));
                UpdateLbl(ch, _lblBottomOT[ch, dutIndex], _o_t.ContainsKey(slave.O_T) ? _o_t[slave.O_T] : ValueHelper.GetHextStrWithOx(slave.O_T));
                UpdateLbl(ch, _lblBottomEleD[ch, dutIndex], _ele_d.ContainsKey(slave.Ele_D) ? _ele_d[slave.Ele_D] : ValueHelper.GetHextStrWithOx(slave.Ele_D));
                UpdateLbl(ch, _lblBottomSupL[ch, dutIndex], _sup_v.ContainsKey(slave.Sup_V) ? _sup_v[slave.Sup_V] : ValueHelper.GetHextStrWithOx(slave.Sup_V));
                UpdateLbl(ch, _lblBottomEmeRO[ch, dutIndex], _eme_r_o.ContainsKey(slave.Eme_R_O) ? _eme_r_o[slave.Eme_R_O] : ValueHelper.GetHextStrWithOx(slave.Eme_R_O));
                UpdateLbl(ch, _lblBottomRes[ch, dutIndex], _res.ContainsKey(slave.Res) ? _res[slave.Res] : ValueHelper.GetHextStrWithOx(slave.Res));
                _dutInfos[ch, dutIndex].BRes = slave.Res;
            }

            _dutInfos[ch, dutIndex].IsLinLoss = isLossLin;

            UpdateLbl(ch, _lblBottomCaliA[ch, dutIndex], _dutInfos[ch, dutIndex].Slave.CaliStallA.ToString());
            UpdateLbl(ch, _lblBottomCaliB[ch, dutIndex], _dutInfos[ch, dutIndex].Slave.CaliStallB.ToString());
        }

        private void InitTopPanel()
        {
            string[] rowLabels = { "NAD:", "CID:", "SID:", "当前位置(R):", "目标位置(w):", "旋转方向(R):", "速度状态(R):", "堵转状态(R):", "当前进程:", "循环计数:", "周期计数:", "自学习计数:", "计数清零:", "开始/停止:" };
            //lblRowHeaders = new UIMarkLabel[2, rowLabels.Length];

            for (var ch = 0; ch < DutCount.GetLength(0); ch++)
            {
                // 创建行标题
                for (var i = 0; i < rowLabels.Length; i++)
                {
                    lblRowHeaders[ch, i] = CreateLabel(rowLabels[i], 0, 0, 70, 22, isBold: true);
                    _topPanel[ch, 0].Controls.Add(lblRowHeaders[ch, i]);
                }

                if (DutCount[ch, 0] is 0)
                    continue;

                // 创建列标题和每列控件
                for (var col = 0; col < DutCount[ch, 0]; col++)
                {
                    _lblNad[ch, col] = CreateLabel(ValueHelper.GetHextStrWithOx((byte)(0x20 + col)), 0, 0, 60, 20, true, true, true, isEvenCol: col % 2 == 0);
                    _topPanel[ch, 0].Controls.Add(_lblNad[ch, col]);

                    _lblCid[ch, col] = CreateLabel(ValueHelper.GetHextStrWithOx((byte)(col + 1)), 0, 0, 60, 20, true, true, true, isEvenCol: col % 2 == 0);
                    _topPanel[ch, 0].Controls.Add(_lblCid[ch, col]);

                    _lblSid[ch, col] = CreateLabel(ValueHelper.GetHextStrWithOx((byte)(0x10 + col)), 0, 0, 60, 20, true, true, true, isEvenCol: col % 2 == 0);
                    _topPanel[ch, 0].Controls.Add(_lblSid[ch, col]);

                    _lblActualPos[ch, col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, true, isEvenCol: col % 2 == 0);
                    _topPanel[ch, 0].Controls.Add(_lblActualPos[ch, col]);

                    _lblTargetPos[ch, col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, true, isEvenCol: col % 2 == 0);
                    _topPanel[ch, 0].Controls.Add(_lblTargetPos[ch, col]);

                    _lblDirection[ch, col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, true, isEvenCol: col % 2 == 0);
                    _topPanel[ch, 0].Controls.Add(_lblDirection[ch, col]);

                    _lblSpeed[ch, col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, true, isEvenCol: col % 2 == 0);
                    _topPanel[ch, 0].Controls.Add(_lblSpeed[ch, col]);

                    _lblStallState[ch, col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, true, isEvenCol: col % 2 == 0);
                    _topPanel[ch, 0].Controls.Add(_lblStallState[ch, col]);

                    _lblCurrentState[ch, col] = CreateLabel("等待开始", 0, 0, 60, 20, true, true, isEvenCol: col % 2 == 0);
                    _topPanel[ch, 0].Controls.Add(_lblCurrentState[ch, col]);

                    _lblCycleCount[ch, col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, true, isEvenCol: col % 2 == 0);
                    _topPanel[ch, 0].Controls.Add(_lblCycleCount[ch, col]);

                    _lblPeriodCount[ch, col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, true, isEvenCol: col % 2 == 0);
                    _topPanel[ch, 0].Controls.Add(_lblPeriodCount[ch, col]);

                    _lblAutoCaliCount[ch, col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, true, isEvenCol: col % 2 == 0);
                    _topPanel[ch, 0].Controls.Add(_lblAutoCaliCount[ch, col]);

                    _btnClear[ch, col] = CreateButton("Clear", 0, 0, 60, 20, UIStyle.Red);
                    _btnClear[ch, col].Tag = new int[] { ch, col };
                    _btnClear[ch, col].Click += (ss, ee) =>
                    {
                        var btnClear = ss as UIButton;
                        if (btnClear != null && btnClear.Tag != null)
                        {
                            var dutTag = (int[])btnClear.Tag;
                            var tagDutCh = dutTag[0];
                            var tagDutIndex = dutTag[1];
                            var option = new UIEditOption();
                            option.AddCombobox("type", "选择清零类型", new string[] { "循环计数", "周期计数", "自學習计数" }, 0);

                            var frm = new UIEditForm(option);
                            frm.Render();
                            frm.ShowDialog();

                            if (!frm.IsOK)
                            {
                                UIMessageTip.Show("操作已取消");
                                return;
                            }

                            var type = (int)frm["type"];
                            if (type == 0)
                            {
                                _dutInfos[tagDutCh, tagDutIndex].CycleCount = 0;
                                UIMessageTip.Show("循环计数已清零");
                            }
                            else if (type == 1)
                            {
                                _dutInfos[tagDutCh, tagDutIndex].CycleCount = 0;
                                _dutInfos[tagDutCh, tagDutIndex].PeriodCount = 0;
                                UIMessageTip.Show("周期计数已清零");
                            }
                            else if (type == 2)
                            {
                                _dutInfos[tagDutCh, tagDutIndex].CaliCount = 0;
                                UIMessageTip.Show("自學習计数已清零");
                            }
                        }
                    };
                    _topPanel[ch, 0].Controls.Add(_btnClear[ch, col]);

                    _btnStartStop[ch, col] = CreateButton("#" + (col + 1), 0, 0, 60, 22, UIStyle.Orange);
                    _btnStartStop[ch, col].Tag = new int[] { ch, col };
                    _btnStartStop[ch, col].Click += StartStop_Click;
                    _topPanel[ch, 0].Controls.Add(_btnStartStop[ch, col]);
                }
            }
        }

        private void InitBottomPanel()
        {
            {
                string[] rowLabels = { "通信卡类型", "速度等级:", "初始化位置:", "循环次数:", "循环延时:", "周期次数:", "是否循环中监控堵转:" };
                lblBottomRowHeaders = new UIMarkLabel[rowLabels.Length];

                // 创建行标题
                for (var i = 0; i < rowLabels.Length; i++)
                {
                    lblBottomRowHeaders[i] = CreateLabel(rowLabels[i], 0, 0, 70, 22, isBold: true);
                    panelCycleParaSet.Controls.Add(lblBottomRowHeaders[i]);
                }

                // 创建列标题和每列控件
                nudBoardType = CreateNumericUpDown(0, 0, 80, 0, 1, _boardType);
                panelCycleParaSet.Controls.Add(nudBoardType);

                nudSpeed = CreateNumericUpDown(0, 0, 80, 1, 5, _bDefaultSpeed);
                panelCycleParaSet.Controls.Add(nudSpeed);

                nudInitPos = CreateNumericUpDown(0, 0, 80, 0, 65535, _bDefaultStartPos);
                panelCycleParaSet.Controls.Add(nudInitPos);

                nudCycleCount = CreateNumericUpDown(0, 0, 80, 1, 10000, _bCycleMaxCount);
                panelCycleParaSet.Controls.Add(nudCycleCount);

                nudCycleDelay = CreateNumericUpDown(0, 0, 80, 1, 5000, _bDefaultCycleDelay);
                panelCycleParaSet.Controls.Add(nudCycleDelay);

                nudPeriodCount = CreateNumericUpDown(0, 0, 80, 1, 1000000, _bPeriodMaxCount);
                panelCycleParaSet.Controls.Add(nudPeriodCount);

                nudIsMonitorStall = CreateNumericUpDown(0, 0, 80, 0, 1, _bMonitorStall);
                panelCycleParaSet.Controls.Add(nudIsMonitorStall);

                btnParaReset = CreateButton("一键重置", 0, 0, 70, 22, UIStyle.Blue);
                btnParaReset.Click += (ss, ee) =>
                {
                    nudSpeed.Value = _bDefaultSpeed;
                    nudInitPos.Value = _bDefaultStartPos;
                    nudCycleCount.Value = _bCycleMaxCount;
                    nudCycleDelay.Value = _bDefaultCycleDelay;
                    nudPeriodCount.Value = _bPeriodMaxCount;
                    nudIsMonitorStall.Value = _bMonitorStall;

                    //nudOffset.Value = (decimal)Offet2Reg(_offset);

                    nudOffsetNegative.Value = (decimal)Offet2Reg(_offsetNegative);
                    nudOffsetPositive.Value = (decimal)Offet2Reg(_offsetPositive);

                    nudLin1Count.Value = DutCount[0, 0];
                    nudLin2Count.Value = DutCount[1, 0];
                };
                panelCycleParaSet.Controls.Add(btnParaReset);

                btnParaInvoke = CreateButton("一键生效", 0, 0, 70, 22, UIStyle.Blue);
                btnParaInvoke.Click += (ss, ee) =>
                {
                    _bDefaultSpeed = (int)nudSpeed.Value;
                    _bDefaultStartPos = (int)nudInitPos.Value;
                    _bCycleMaxCount = (int)nudCycleCount.Value;
                    _bDefaultCycleDelay = (int)nudCycleDelay.Value;
                    _bPeriodMaxCount = (int)nudPeriodCount.Value;
                    _boardType = (int)nudBoardType.Value;
                    _bMonitorStall = (int)nudIsMonitorStall.Value;
                    //_offset = Reg2Offset((double)nudOffset.Value);

                    _offsetNegative = Reg2Offset((double)nudOffsetNegative.Value);
                    _offsetPositive = Reg2Offset((double)nudOffsetPositive.Value);

                    for (var l = 0; l < DutCount.GetLength(0); l++)
                    {
                        if (DutCount[l, 0] is 0)
                            continue;

                        for (var i = 0; i < DutCount[l, 0]; i++)
                        {
                            _dutInfos[l, i].Slave.SetStartPosition((ushort)_bDefaultStartPos);
                            _dutInfos[l, i].Slave.SetSpeedLevel((byte)_bDefaultSpeed);
                        }
                    }

                    UIMessageTip.Show("已应用");
                };
                panelCycleParaSet.Controls.Add(btnParaInvoke);

                btnParaSave = CreateButton("一键保存", 0, 0, 70, 22, UIStyle.Blue);
                btnParaSave.Click += (ss, ee) =>
                {
                    if (!UIMessageBox.ShowAsk("确定保存？"))
                    {
                        UIMessageTip.Show("取消保存");
                        return;
                    }

                    if (_ini is null)
                    {
                        UIMessageBox.ShowError("配置文件不存在，无法保存");
                        return;
                    }

                    try
                    {
                        btnParaInvoke.PerformClick();
                        //_ini.IniWriteValue("Sys", "Offset", _offset.ToString());
                        _ini.IniWriteValue("Sys", "OffsetNegative", _offsetNegative.ToString());
                        _ini.IniWriteValue("Sys", "OffsetPositive", _offsetPositive.ToString());

                        _ini.IniWriteValue("Sys", "CycleMaxCount", _bCycleMaxCount.ToString());
                        _ini.IniWriteValue("Sys", "PeriodMaxCount", _bPeriodMaxCount.ToString());
                        _ini.IniWriteValue("Sys", "DefaultSpeed", _bDefaultSpeed.ToString());
                        _ini.IniWriteValue("Sys", "DefaultStartPos", _bDefaultStartPos.ToString());
                        _ini.IniWriteValue("Sys", "DefaultCycleDelay", _bDefaultCycleDelay.ToString());
                        _ini.IniWriteValue("Sys", "BoardType", _boardType.ToString());
                        _ini.IniWriteValue("Sys", "IsMonitorStall", _bMonitorStall.ToString());

                        _ini.IniWriteValue("Sys", "Lin1DutCount", nudLin1Count.Value.ToString());
                        _ini.IniWriteValue("Sys", "Lin2DutCount", nudLin2Count.Value.ToString());
                        //_ini.IniWriteValue("Sys", "Offset", _offset.ToString());
                    }
                    catch (Exception ex)
                    {
                        UIMessageBox.ShowError("保存失败：" + ex.Message);
                    }
                };
                panelCycleParaSet.Controls.Add(btnParaSave);
            }

            {
                string[] rowLabels = { "循环偏移量(-)/°", "循环偏移量(+)/°", "LIN1监控数量:", "LIN2监控数量:" };
                lblBottomRowHeaders2 = new UIMarkLabel[rowLabels.Length];

                // 创建行标题
                for (var i = 0; i < rowLabels.Length; i++)
                {
                    lblBottomRowHeaders2[i] = CreateLabel(rowLabels[i], 0, 0, 70, 22, isBold: true);
                    panelCycleParaSet.Controls.Add(lblBottomRowHeaders2[i]);
                }

                // 创建列标题和每列控件                
                nudOffsetPositive = CreateNumericUpDown(0, 0, 80, 1, 90, Offet2Reg(_offsetPositive));
                panelCycleParaSet.Controls.Add(nudOffsetPositive);

                nudOffsetNegative = CreateNumericUpDown(0, 0, 80, 1, 90, Offet2Reg(_offsetNegative));
                panelCycleParaSet.Controls.Add(nudOffsetNegative);

                nudLin1Count = CreateNumericUpDown(0, 0, 80, 0, 16, DutCount[0, 0]);
                panelCycleParaSet.Controls.Add(nudLin1Count);

                nudLin2Count = CreateNumericUpDown(0, 0, 80, 0, 16, DutCount[1, 0]);
                panelCycleParaSet.Controls.Add(nudLin2Count);
            }

            for (var ch = 0; ch < DutCount.GetLength(0); ch++)
            {
                string[] rowLabels = { "NAD:", "SEND(W):", "RECV(R):", "堵轉點A:", "堵轉點B:", "啓停狀態(R):", "LIN故障(R)", "过温故障(R)", "电器故障(R)", "电压故障(R)", "紧急操作(R)", "复位状态(R)", "堵转使能(W)" };
                //_lblBottomRowHeaders2 = new UIMarkLabel[2, rowLabels.Length];

                // 创建行标题
                for (var i = 0; i < rowLabels.Length; i++)
                {
                    _lblBottomRowHeaders2[ch, i] = CreateLabel(rowLabels[i], 0, 0, 70, 22, isBold: true);
                    _bottomPanel[ch, 0].Controls.Add(_lblBottomRowHeaders2[ch, i]);
                }

                if (DutCount[ch, 0] is 0)
                    continue;

                // 创建列标题和每列控件
                for (var col = 0; col < DutCount[ch, 0]; col++)
                {
                    _lblBottomNad[ch, col] = CreateLabel(ValueHelper.GetHextStrWithOx((byte)(0x20 + col)), 0, 0, 60, 20, true, true, true, isEvenCol: col % 2 == 0);
                    _bottomPanel[ch, 0].Controls.Add(_lblBottomNad[ch, col]);

                    _lblBottomSend[ch, col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, true, false, isEvenCol: col % 2 == 0);
                    _bottomPanel[ch, 0].Controls.Add(_lblBottomSend[ch, col]);

                    _lblBottomRecv[ch, col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, true, false, isEvenCol: col % 2 == 0);
                    _bottomPanel[ch, 0].Controls.Add(_lblBottomRecv[ch, col]);

                    _lblBottomCaliA[ch, col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, false, isEvenCol: col % 2 == 0);
                    _bottomPanel[ch, 0].Controls.Add(_lblBottomCaliA[ch, col]);

                    _lblBottomCaliB[ch, col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, false, isEvenCol: col % 2 == 0);
                    _bottomPanel[ch, 0].Controls.Add(_lblBottomCaliB[ch, col]);

                    _lblBottomMode[ch, col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, false, isEvenCol: col % 2 == 0);
                    _bottomPanel[ch, 0].Controls.Add(_lblBottomMode[ch, col]);

                    _lblBottomRErr[ch, col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, false, isEvenCol: col % 2 == 0);
                    _bottomPanel[ch, 0].Controls.Add(_lblBottomRErr[ch, col]);

                    _lblBottomOT[ch, col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, false, isEvenCol: col % 2 == 0);
                    _bottomPanel[ch, 0].Controls.Add(_lblBottomOT[ch, col]);

                    _lblBottomEleD[ch, col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, false, isEvenCol: col % 2 == 0);
                    _bottomPanel[ch, 0].Controls.Add(_lblBottomEleD[ch, col]);

                    _lblBottomSupL[ch, col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, false, isEvenCol: col % 2 == 0);
                    _bottomPanel[ch, 0].Controls.Add(_lblBottomSupL[ch, col]);

                    _lblBottomEmeRO[ch, col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, false, isEvenCol: col % 2 == 0);
                    _bottomPanel[ch, 0].Controls.Add(_lblBottomEmeRO[ch, col]);

                    _lblBottomRes[ch, col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, false, isEvenCol: col % 2 == 0);
                    _bottomPanel[ch, 0].Controls.Add(_lblBottomRes[ch, col]);

                    _lblBottomStallDR[ch, col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, false, isEvenCol: col % 2 == 0);
                    _bottomPanel[ch, 0].Controls.Add(_lblBottomStallDR[ch, col]);
                }
            }
        }

        private static double Offet2Reg(ushort offset)
        {
            return Math.Round((360f / 6400) * offset, 1);
        }

        private static ushort Reg2Offset(double reg)
        {
            return (ushort)((6400f / 360) * reg);
        }

        private UIMarkLabel CreateLabel(string text, int x, int y, int width, int height, bool isBottomLine = false, bool isShowBorder = false, bool isBold = false, ContentAlignment textAlign = ContentAlignment.MiddleCenter, bool isEvenCol = false)
        {
            return new UIMarkLabel
            {
                Text = text,
                Location = new Point(x, y),
                //Font = new Font("微软雅黑", isLowDpi ? 7 : 9, isLowDpi ? FontStyle.Italic : (isBold ? FontStyle.Bold : FontStyle.Italic)),
                Font = new Font("微软雅黑", 9, isBold ? FontStyle.Bold : FontStyle.Italic),
                TextAlign = textAlign,
                MarkPos = isBottomLine ? UIMarkLabel.UIMarkPos.Bottom : UIMarkLabel.UIMarkPos.Left,
                BorderStyle = !isShowBorder ? BorderStyle.None : BorderStyle.FixedSingle,
                ForeColor = Color.WhiteSmoke,
                BackColor = isEvenCol ? Color.DarkCyan : Color.Teal
            };
        }

        private UIButton CreateButton(string text, int x, int y, int width, int height, UIStyle uIStyle)
        {
            return new UIButton
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(width, height),
                Font = new Font("微软雅黑", 9),
                Style = uIStyle
            };
        }

        private NumericUpDown CreateNumericUpDown(int x, int y, int width, int min, int max, int value)
        {
            return new NumericUpDown
            {
                Location = new Point(x, y),
                Size = new Size(width, 26),
                Minimum = min,
                Maximum = max,
                Value = value
            };
        }

        private NumericUpDown CreateNumericUpDown(int x, int y, int width, int min, int max, double value)
        {
            return new NumericUpDown
            {
                Location = new Point(x, y),
                Size = new Size(width, 26),
                Minimum = min,
                Maximum = max,
                Value = (decimal)value,
                DecimalPlaces = 1
            };
        }

        private void ResizeUI()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);

            Width = (int)(Screen.PrimaryScreen.WorkingArea.Width * 1.0) - 10;
            Height = Screen.PrimaryScreen.WorkingArea.Height - 10;

            Location = new Point(0 + 5, 0 + 5);

            var dis = Height / 2;
            if (dis >= uiSplitContainer1.Panel1MinSize && dis <= uiSplitContainer1.Height - uiSplitContainer1.Panel2MinSize)
                uiSplitContainer1.SplitterDistance = Height / 2;
        }

        const int lowDpiFontSize = 6;
        const int highDpiFontSize = 7;

        private void ResizeTopPanel()
        {
            var screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            var screenHigh = Screen.PrimaryScreen.WorkingArea.Height;
            var isLowDpi = screenWidth < 1280 || screenHigh < (1024 - 0);

            for (var ch = 0; ch < DutCount.GetLength(0); ch++)
            {
                var panelWidth = _topPanel[ch, 0].ClientSize.Width;
                var panelHeight = _topPanel[ch, 0].ClientSize.Height;

                var labelWidth = isLowDpi ? 120 : 125;
                var availableWidth = panelWidth - labelWidth - 3;
                var rowHeight = Math.Max(20, (panelHeight - 10) / 14);
                var startX = labelWidth + 5;
                var startY = 5;

                // 布局行标题
                for (var i = 0; i < _maxDutCount; i++)
                {
                    if (lblRowHeaders[ch, i] is null)
                        continue;

                    //var y = startY + (i + 1) * rowHeight;
                    var y = startY + (i + 0) * rowHeight;
                    lblRowHeaders[ch, i].Location = new Point(5, y);
                    lblRowHeaders[ch, i].Size = new Size(labelWidth - 5, rowHeight - 2);
                }

                if (DutCount[ch, 0] is 0)
                    continue;

                var colWidth = availableWidth / DutCount[ch, 0];

                // 布局每列控件
                for (var col = 0; col < DutCount[ch, 0]; col++)
                {
                    var x = startX + col * colWidth;
                    var w = colWidth - 4;
                    var row = -1;

                    _lblNad[ch, col].Location = new Point(x, startY + (++row) * rowHeight);
                    _lblNad[ch, col].Size = new Size(w, rowHeight - 2);
                    _lblNad[ch, col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblNad[ch, col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                    _lblCid[ch, col].Location = new Point(x, startY + (++row) * rowHeight);
                    _lblCid[ch, col].Size = new Size(w, rowHeight - 2);
                    _lblCid[ch, col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblCid[ch, col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                    _lblSid[ch, col].Location = new Point(x, startY + (++row) * rowHeight);
                    _lblSid[ch, col].Size = new Size(w, rowHeight - 2);
                    _lblSid[ch, col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblSid[ch, col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                    _lblActualPos[ch, col].Location = new Point(x, startY + (++row) * rowHeight);
                    _lblActualPos[ch, col].Size = new Size(w, rowHeight - 2);
                    _lblActualPos[ch, col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblActualPos[ch, col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                    _lblTargetPos[ch, col].Location = new Point(x, startY + (++row) * rowHeight);
                    _lblTargetPos[ch, col].Size = new Size(w, rowHeight - 2);
                    _lblTargetPos[ch, col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblTargetPos[ch, col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                    _lblDirection[ch, col].Location = new Point(x, startY + (++row) * rowHeight);
                    _lblDirection[ch, col].Size = new Size(w, rowHeight - 2);
                    _lblDirection[ch, col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblDirection[ch, col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                    _lblSpeed[ch, col].Location = new Point(x, startY + (++row) * rowHeight);
                    _lblSpeed[ch, col].Size = new Size(w, rowHeight - 2);
                    _lblSpeed[ch, col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblSpeed[ch, col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                    _lblStallState[ch, col].Location = new Point(x, startY + (++row) * rowHeight);
                    _lblStallState[ch, col].Size = new Size(w, rowHeight - 2);
                    _lblStallState[ch, col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblStallState[ch, col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                    _lblCurrentState[ch, col].Location = new Point(x, startY + (++row) * rowHeight);
                    _lblCurrentState[ch, col].Size = new Size(w, rowHeight - 2);
                    _lblCurrentState[ch, col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblCurrentState[ch, col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                    _lblCycleCount[ch, col].Location = new Point(x, startY + (++row) * rowHeight);
                    _lblCycleCount[ch, col].Size = new Size(w, rowHeight - 2);
                    _lblCycleCount[ch, col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblCycleCount[ch, col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                    _lblPeriodCount[ch, col].Location = new Point(x, startY + (++row) * rowHeight);
                    _lblPeriodCount[ch, col].Size = new Size(w, rowHeight - 2);
                    _lblPeriodCount[ch, col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblPeriodCount[ch, col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                    _lblAutoCaliCount[ch, col].Location = new Point(x, startY + (++row) * rowHeight);
                    _lblAutoCaliCount[ch, col].Size = new Size(w, rowHeight - 2);
                    _lblAutoCaliCount[ch, col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblAutoCaliCount[ch, col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                    _btnClear[ch, col].Location = new Point(x, startY + (++row) * rowHeight);
                    _btnClear[ch, col].Size = new Size(w, rowHeight - 2);
                    //_btnClear[col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _btnClear[col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                    _btnStartStop[ch, col].Location = new Point(x, startY + (++row) * rowHeight);
                    _btnStartStop[ch, col].Size = new Size(w, rowHeight - 2);
                    //_btnStartStop[col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _btnStartStop[col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);
                }
            }
        }

        private void ResizeBottomPanel()
        {
            var screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            var screenHigh = Screen.PrimaryScreen.WorkingArea.Height;
            var isLowDpi = screenWidth < 1280 || screenHigh < (1024 - 0);

            // panelCycleParaSet
            {
                var panelWidth = panelCycleParaSet.ClientSize.Width;
                var panelHeight = panelCycleParaSet.ClientSize.Height;

                var labelWidth = isLowDpi ? 100 : 180;
                var availableWidth = panelWidth - labelWidth - 20;
                var colWidth = 100;
                var rowHeight = isLowDpi ? 30 : 35;
                var startX = labelWidth + 5;
                var startY = 5;

                {
                    // 布局行标题
                    for (var i = 0; i < lblBottomRowHeaders.Length; i++)
                    {
                        //var y = startY + (i + 1) * rowHeight;
                        var y = startY + (i + 0) * rowHeight;
                        lblBottomRowHeaders[i].Location = new Point(5, y);
                        lblBottomRowHeaders[i].Size = new Size(labelWidth - 5, rowHeight - 2);
                    }

                    var x = startX;
                    var w = colWidth - 4;
                    var row = -1;

                    nudBoardType.Location = new Point(x, startY + (++row) * rowHeight);
                    nudBoardType.Size = new Size(w, rowHeight - 2);

                    nudSpeed.Location = new Point(x, startY + (++row) * rowHeight);
                    nudSpeed.Size = new Size(w, rowHeight - 2);

                    nudInitPos.Location = new Point(x, startY + (++row) * rowHeight);
                    nudInitPos.Size = new Size(w, rowHeight - 2);

                    nudCycleCount.Location = new Point(x, startY + (++row) * rowHeight);
                    nudCycleCount.Size = new Size(w, rowHeight - 2);

                    nudCycleDelay.Location = new Point(x, startY + (++row) * rowHeight);
                    nudCycleDelay.Size = new Size(w, rowHeight - 2);

                    nudPeriodCount.Location = new Point(x, startY + (++row) * rowHeight);
                    nudPeriodCount.Size = new Size(w, rowHeight - 2);

                    nudIsMonitorStall.Location = new Point(x, startY + (++row) * rowHeight);
                    nudIsMonitorStall.Size = new Size(w, rowHeight - 2);

                    btnParaReset.Location = new Point(5, startY + (++row) * rowHeight);
                    btnParaReset.Size = new Size((int)((w * 2) / (isLowDpi ? 3 : 2.3)), rowHeight - 2);

                    btnParaInvoke.Location = new Point(btnParaReset.Location.X + btnParaReset.Width + (isLowDpi ? 2 : 5), btnParaReset.Location.Y);
                    btnParaInvoke.Size = new Size((int)((w * 2) / (isLowDpi ? 3 : 2.3)), rowHeight - 2);

                    btnParaSave.Location = new Point(btnParaInvoke.Location.X + btnParaInvoke.Width + (isLowDpi ? 2 : 5), btnParaReset.Location.Y);
                    btnParaSave.Size = new Size((int)((w * 2) / (isLowDpi ? 3 : 2.3)), rowHeight - 2);
                }

                {
                    startY = 5;

                    // 布局行标题
                    for (var i = 0; i < lblBottomRowHeaders2.Length; i++)
                    {
                        var y = startY + (i + 0) * rowHeight;
                        lblBottomRowHeaders2[i].Location = new Point(nudBoardType.Location.X + nudBoardType.Width + 5, y);
                        lblBottomRowHeaders2[i].Size = new Size(labelWidth - 5, rowHeight - 2);
                    }

                    var x = startX + nudBoardType.Location.X + nudBoardType.Width;
                    var w = colWidth - 4;
                    var row = -1;

                    //nudOffset.Location = new Point(x, startY + (++row) * rowHeight);
                    //nudOffset.Size = new Size(w, rowHeight - 2);

                    nudOffsetNegative.Location = new Point(x, startY + (++row) * rowHeight);
                    nudOffsetNegative.Size = new Size(w, rowHeight - 2);

                    nudOffsetPositive.Location = new Point(x, startY + (++row) * rowHeight);
                    nudOffsetPositive.Size = new Size(w, rowHeight - 2);

                    nudLin1Count.Location = new Point(x, startY + (++row) * rowHeight);
                    nudLin1Count.Size = new Size(w, rowHeight - 2);

                    nudLin2Count.Location = new Point(x, startY + (++row) * rowHeight);
                    nudLin2Count.Size = new Size(w, rowHeight - 2);
                }
            }

            for (var ch = 0; ch < DutCount.GetLength(0); ch++)
            {
                var panelWidth = _bottomPanel[ch, 0].ClientSize.Width;
                var panelHeight = _bottomPanel[ch, 0].ClientSize.Height;
                var labelWidth = isLowDpi ? 100 : 105;
                var availableWidth = panelWidth - (labelWidth + 0 + 0) - 1;

                var rowHeight = Math.Max(20, (panelHeight - 10) / 14);
                var startX = labelWidth + 5;
                var startY = 5;

                // 布局行标题
                for (var i = 0; i < _maxDutCount; i++)
                {
                    if (_lblBottomRowHeaders2[ch, i] is null)
                        continue;

                    var y = startY + (i + 0) * rowHeight;
                    _lblBottomRowHeaders2[ch, i].Location = new Point(0 + 3, y);
                    _lblBottomRowHeaders2[ch, i].Size = new Size(labelWidth - 5, rowHeight - 2);
                }

                if (DutCount[ch, 0] is 0)
                    continue;

                var colWidth = availableWidth / DutCount[ch, 0];

                // 布局每列控件
                for (var col = 0; col < DutCount[ch, 0]; col++)
                {
                    var x = 0 + startX + col * colWidth;
                    var w = colWidth - 2;
                    var row = -1;

                    _lblBottomNad[ch, col].Location = new Point(x, startY + (++row) * rowHeight);
                    _lblBottomNad[ch, col].Size = new Size(w, rowHeight - 2);
                    _lblBottomNad[ch, col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblBottomNad[ch, col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                    _lblBottomSend[ch, col].Location = new Point(x, startY + (++row) * rowHeight);
                    _lblBottomSend[ch, col].Size = new Size(w, rowHeight - 2);
                    _lblBottomSend[ch, col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblBottomSend[ch, col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                    _lblBottomRecv[ch, col].Location = new Point(x, startY + (++row) * rowHeight);
                    _lblBottomRecv[ch, col].Size = new Size(w, rowHeight - 2);
                    _lblBottomRecv[ch, col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblBottomRecv[ch, col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                    _lblBottomCaliA[ch, col].Location = new Point(x, startY + (++row) * rowHeight);
                    _lblBottomCaliA[ch, col].Size = new Size(w, rowHeight - 2);
                    _lblBottomCaliA[ch, col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblBottomCaliA[ch, col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                    _lblBottomCaliB[ch, col].Location = new Point(x, startY + (++row) * rowHeight);
                    _lblBottomCaliB[ch, col].Size = new Size(w, rowHeight - 2);
                    _lblBottomCaliB[ch, col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblBottomCaliB[ch, col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                    _lblBottomMode[ch, col].Location = new Point(x, startY + (++row) * rowHeight);
                    _lblBottomMode[ch, col].Size = new Size(w, rowHeight - 2);
                    _lblBottomMode[ch, col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblBottomMode[ch, col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                    _lblBottomRErr[ch, col].Location = new Point(x, startY + (++row) * rowHeight);
                    _lblBottomRErr[ch, col].Size = new Size(w, rowHeight - 2);
                    _lblBottomRErr[ch, col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblBottomRErr[ch, col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                    _lblBottomOT[ch, col].Location = new Point(x, startY + (++row) * rowHeight);
                    _lblBottomOT[ch, col].Size = new Size(w, rowHeight - 2);
                    _lblBottomOT[ch, col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblBottomOT[ch, col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                    _lblBottomEleD[ch, col].Location = new Point(x, startY + (++row) * rowHeight);
                    _lblBottomEleD[ch, col].Size = new Size(w, rowHeight - 2);
                    _lblBottomEleD[ch, col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblBottomEleD[ch, col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                    _lblBottomSupL[ch, col].Location = new Point(x, startY + (++row) * rowHeight);
                    _lblBottomSupL[ch, col].Size = new Size(w, rowHeight - 2);
                    _lblBottomSupL[ch, col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblBottomSupL[ch, col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                    _lblBottomEmeRO[ch, col].Location = new Point(x, startY + (++row) * rowHeight);
                    _lblBottomEmeRO[ch, col].Size = new Size(w, rowHeight - 2);
                    _lblBottomEmeRO[ch, col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblBottomEmeRO[ch, col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                    _lblBottomRes[ch, col].Location = new Point(x, startY + (++row) * rowHeight);
                    _lblBottomRes[ch, col].Size = new Size(w, rowHeight - 2);
                    _lblBottomRes[ch, col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblBottomRes[ch, col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                    _lblBottomStallDR[ch, col].Location = new Point(x, startY + (++row) * rowHeight);
                    _lblBottomStallDR[ch, col].Size = new Size(w, rowHeight - 2);
                    _lblBottomStallDR[ch, col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblBottomStallDR[ch, col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);
                }
            }
        }

        [DllImport("Kernel32.dll")] public static extern IntPtr GetCurrentThread();
        [DllImport("Kernel32.dll")] public static extern int SetThreadIdealProcessor(IntPtr hThread, uint dwIdealProcessor);
        private Thread m_WorkThread;

        private long[,] _logTs = new long[2, _maxDutCount];
        private long[,] _refreshTs = new long[2, _maxDutCount];

        private readonly Dictionary<byte, string> _r_err = new Dictionary<byte, string>
        {
            {0x00,"0_No_error" },
            {0x01,"1_Error_present" },
        };

        private readonly Dictionary<byte, string> _o_t = new Dictionary<byte, string>
        {
            {0x00,"0_No_over_temperature" },
            {0x01,"1_over_temperature" },
            {0x02,"2_reserved" },
            {0x03,"3_Signal_invalid" },
        };

        private readonly Dictionary<byte, string> _ele_d = new Dictionary<byte, string>
        {
            {0x00,"0_No_defect" },
            {0x01,"1_Electrical_defect_present" },
            {0x02,"2_Permanent_electrical_defect_present" },
            {0x03,"3_Signal_invalid" },
        };

        private readonly Dictionary<byte, string> _sup_v = new Dictionary<byte, string>
        {
            {0x00,"0_Voltage_in_permissible_range" },
            {0x01,"1_Under_voltage" },
            {0x02,"2_Over_voltage" },
            {0x03,"3_Signal_invalid" },
        };

        private readonly Dictionary<byte, string> _eme_r_o = new Dictionary<byte, string>
        {
            {0x00,"0_No_Emergency_run_occurred" },
            {0x01,"1_Emergency_run_occurred" },
            {0x02,"2_reserved" },
            {0x03,"3_Signal_invalid" },
        };

        private readonly Dictionary<byte, string> _res = new Dictionary<byte, string>
        {
            {0x00,"0_No_reset" },
            {0x01,"1_Reset_performed" },
            {0x02,"2_reserved" },
            {0x03,"3_Signal_invalid" },
        };

        private readonly Dictionary<byte, string> _rot_dir = new Dictionary<byte, string>
        {
            {0x00,"0_CW" },
            {0x01,"1_CCW" },
            {0x02,"2_reserved" },
            {0x03,"3_Signal_invalid" },
        };

        private readonly Dictionary<byte, string> _spe_s = new Dictionary<byte, string>
        {
            {0x00,"0_Actuator_stopped" },
            {0x01,"1_Speed_level_1" },
            {0x02,"2_Speed_level_2" },
            {0x03,"3_Speed_level_3" },
            {0x04,"4_Speed_level_4" },
            {0x05,"5_Auto_Speed" },
            {0x06,"6_reserved6" },
            {0x07,"7_reserved7" },
            {0x08,"8_reserved8" },
            {0x09,"9_reserved9" },
            {0x0A,"10_reservedA" },
            {0x0B,"11_reservedB" },
            {0x0C,"12_reservedC" },
            {0x0D,"13_reservedD" },
            {0x0E,"14_reservedE" },
            {0x0F,"15_Signal_invalid" },
        };

        private readonly Dictionary<byte, string> _stall_o = new Dictionary<byte, string>
        {
            {0x00,"0_No_stall" },
            {0x01,"1_Stall_detected" },
            {0x02,"2_reserved" },
            {0x03,"3_Signal_invalid" },
        };

        private readonly Dictionary<byte, string> _stall_d_r = new Dictionary<byte, string>
        {
            {0x00,"0_Stall_detection_off" },
            {0x01,"1_Stall_detection_on" },
            {0x02,"2_reserved" },
            {0x03,"3_Signal_invalid" },
        };

        private Task WorkThreadProc(int ch, int dutIndex)
        {
            return Task.Factory.StartNew(async () =>
              {
                  while (true)
                  {
                      await Task.Delay(100);
                      var slave = _dutInfos[ch, dutIndex].Slave;
                      var nowTs = HighPrecisionTimer.GetTimestamp();
                      if (slave != null && HighPrecisionTimer.GetTimestampIntervalMs(_refreshTs[ch, dutIndex], nowTs) >= 1000)
                      {
                          _refreshTs[ch, dutIndex] = nowTs;

                          UpdateLbl(ch, _lblTargetPos[ch, dutIndex], slave.TargetPos.ToString());
                          UpdateLbl(ch, _lblCycleCount[ch, dutIndex], string.Format("{0}/{1}", _dutInfos[ch, dutIndex].CycleCount.ToString(), _bCycleMaxCount));
                          UpdateLbl(ch, _lblPeriodCount[ch, dutIndex], string.Format("{0}/{1}", _dutInfos[ch, dutIndex].PeriodCount.ToString(), _bPeriodMaxCount));
                          UpdateLbl(ch, _lblAutoCaliCount[ch, dutIndex], _dutInfos[ch, dutIndex].CaliCount.ToString());
                          UpdateLbl(ch, _lblBottomNad[ch, dutIndex], ValueHelper.GetHextStrWithOx(_dutInfos[ch, dutIndex].NAD));
                          UpdateLbl(ch, _lblBottomSend[ch, dutIndex], _dutInfos[ch, dutIndex].Slave.SendBytes is null ? string.Empty : ValueHelper.GetHextStr(_dutInfos[ch, dutIndex].Slave.SendBytes).Replace(" ", ""));
                          UpdateLbl(ch, _lblBottomStallDR[ch, dutIndex], _stall_d_r.ContainsKey(slave.Stall_D_R) ? _stall_d_r[slave.Stall_D_R] : ValueHelper.GetHextStrWithOx(slave.Stall_D_R));

                          var isLossLin = HighPrecisionTimer.GetTimestampIntervalMs(_dutInfos[ch, dutIndex].Slave.RecvTs, nowTs) > 3850;
                          if (isLossLin)
                          {
                              UpdateLbl(ch, _lblBottomRecv[ch, dutIndex], string.Empty);
                              UpdateLbl(ch, _lblBottomMode[ch, dutIndex], string.Empty);
                              UpdateLbl(ch, _lblActualPos[ch, dutIndex], string.Empty);
                              UpdateLbl(ch, _lblDirection[ch, dutIndex], string.Empty);
                              UpdateLbl(ch, _lblSpeed[ch, dutIndex], string.Empty);
                              UpdateLbl(ch, _lblStallState[ch, dutIndex], string.Empty);

                              UpdateLbl(ch, _lblBottomRErr[ch, dutIndex], string.Empty);
                              UpdateLbl(ch, _lblBottomOT[ch, dutIndex], string.Empty);
                              UpdateLbl(ch, _lblBottomEleD[ch, dutIndex], string.Empty);
                              UpdateLbl(ch, _lblBottomSupL[ch, dutIndex], string.Empty);
                              UpdateLbl(ch, _lblBottomEmeRO[ch, dutIndex], string.Empty);
                              UpdateLbl(ch, _lblBottomRes[ch, dutIndex], string.Empty);

                              _dutInfos[ch, dutIndex].BRes = 1;
                          }
                          else
                          {
                              UpdateLbl(ch, _lblBottomRecv[ch, dutIndex], _dutInfos[ch, dutIndex].Slave.RecvBytes is null ? string.Empty : ValueHelper.GetHextStr(_dutInfos[ch, dutIndex].Slave.RecvBytes).Replace(" ", ""));
                              UpdateLbl(ch, _lblBottomMode[ch, dutIndex], ValueHelper.GetHextStrWithOx(_dutInfos[ch, dutIndex].Slave.Mode));
                              UpdateLbl(ch, _lblActualPos[ch, dutIndex], slave.ActualPos.ToString());
                              UpdateLbl(ch, _lblDirection[ch, dutIndex], _rot_dir.ContainsKey(slave.Rot_Dir) ? _rot_dir[slave.Rot_Dir] : ValueHelper.GetHextStrWithOx(slave.Rot_Dir));
                              UpdateLbl(ch, _lblSpeed[ch, dutIndex], _spe_s.ContainsKey(slave.Spe_S) ? _spe_s[slave.Spe_S] : ValueHelper.GetHextStrWithOx(slave.Spe_S));
                              UpdateLbl(ch, _lblStallState[ch, dutIndex], _stall_o.ContainsKey(slave.Stall_O) ? _stall_o[slave.Stall_O] : ValueHelper.GetHextStrWithOx(slave.Stall_O));

                              UpdateLbl(ch, _lblBottomRErr[ch, dutIndex], _r_err.ContainsKey(slave.R_Err) ? _r_err[slave.R_Err] : ValueHelper.GetHextStrWithOx(slave.R_Err));
                              UpdateLbl(ch, _lblBottomOT[ch, dutIndex], _o_t.ContainsKey(slave.O_T) ? _o_t[slave.O_T] : ValueHelper.GetHextStrWithOx(slave.O_T));
                              UpdateLbl(ch, _lblBottomEleD[ch, dutIndex], _ele_d.ContainsKey(slave.Ele_D) ? _ele_d[slave.Ele_D] : ValueHelper.GetHextStrWithOx(slave.Ele_D));
                              UpdateLbl(ch, _lblBottomSupL[ch, dutIndex], _sup_v.ContainsKey(slave.Sup_V) ? _sup_v[slave.Sup_V] : ValueHelper.GetHextStrWithOx(slave.Sup_V));
                              UpdateLbl(ch, _lblBottomEmeRO[ch, dutIndex], _eme_r_o.ContainsKey(slave.Eme_R_O) ? _eme_r_o[slave.Eme_R_O] : ValueHelper.GetHextStrWithOx(slave.Eme_R_O));
                              UpdateLbl(ch, _lblBottomRes[ch, dutIndex], _res.ContainsKey(slave.Res) ? _res[slave.Res] : ValueHelper.GetHextStrWithOx(slave.Res));
                              _dutInfos[ch, dutIndex].BRes = slave.Res;
                          }

                          _dutInfos[ch, dutIndex].IsLinLoss = isLossLin;

                          UpdateLbl(ch, _lblBottomCaliA[ch, dutIndex], _dutInfos[ch, dutIndex].Slave.CaliStallA.ToString());
                          UpdateLbl(ch, _lblBottomCaliB[ch, dutIndex], _dutInfos[ch, dutIndex].Slave.CaliStallB.ToString());

                          if (_dutInfos[ch, dutIndex].State != DutState.Idle && _lblCurrentState[ch, dutIndex].Text != "等待开始" && HighPrecisionTimer.GetTimestampIntervalMs(_logTs[ch, dutIndex], nowTs) > 300)
                          {
                              var logData = new SlaveLogData
                              {
                                  Ts = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"),
                                  Mode = _lblCurrentState[ch, dutIndex].Text,
                                  SendData = ValueHelper.GetHextStrWithOx(slave.SendBytes),
                                  RecvData = isLossLin ? string.Empty : ValueHelper.GetHextStrWithOx(slave.RecvBytes),
                                  CycleCount = _dutInfos[ch, dutIndex].CycleCount,
                                  PeriodCount = _dutInfos[ch, dutIndex].PeriodCount,
                                  CaliCount = _dutInfos[ch, dutIndex].CaliCount,
                                  CaliA = slave.CaliStallA,
                                  CaliB = slave.CaliStallB,
                              };
                              _logWriter[ch, dutIndex]?.EnqueueData(logData);
                          }
                      }
                  }
              }, _dutInfos[ch, dutIndex]._cts.Token);
        }

        private void WorkTask(int ch, int index)
        {
            while (_thBackRunThread[ch, index].IsAlive)
            {
                Thread.Sleep(50);

                if (!_thBackRunThread[ch, index].IsAlive)
                    break;

                switch (_dutInfos[ch, index].State)
                {
                    case DutState.Idle:
                        WaitStart(ch, index);
                        break;

                    case DutState.FindStall:
                        {
                            _dutInfos[ch, index].Slave.SetValidPosition(0);
                            _dutInfos[ch, index].Slave.SetStallDetection(0x01);
                            TaskClearStallOccurred(ch, index);
                            TaskClearRes(ch, index);

                            if (_dutInfos[ch, index].State != DutState.FindStall)
                                break;

                            var retryFind = 0;
                            var isResOccurred = false;

                            while (true)
                            {
                                Thread.Sleep(100);

                                var isBreak = false;
                                var isCaliAOk = false;
                                var isCaliBOk = false;

                                for (var i = 0; i < 2; i++)
                                {
                                    TaskClearStallOccurred(ch, index);
                                    UpdateLbl(ch, _lblCurrentState[ch, index], string.Format("寻找堵转点{0}，\r\n重試次數{1}", (i + 1), retryFind));
                                    var pos = TaskFindStall(ch, index, (ushort)(i % 2 == 0 ? 0 + 1 : 65535 - 1)).Result;
                                    var isCaliCOk = true;

                                    if (_dutInfos[ch, index].State == DutState.Idle)
                                    {
                                        isBreak = true;
                                        break;
                                    }

                                    if (pos == 0)
                                    {
                                        isBreak = true;
                                        break;
                                    }
                                    else if (pos == -9999)
                                    {
                                        isResOccurred = true;
                                        isBreak = true;
                                        TaskClearRes(ch, index);
                                        break;
                                    }
                                    else
                                    {
                                        if (i == 0)
                                        {
                                            if (pos == -1)
                                            {
                                                isCaliAOk = false;
                                                isCaliCOk = false;
                                            }
                                            else
                                            {
                                                isCaliAOk = true;
                                                isCaliCOk = true;
                                            }
                                        }
                                        else
                                        {
                                            if (pos == -1)
                                            {
                                                isCaliBOk = false;
                                                isCaliCOk = false;
                                            }
                                            else
                                            {
                                                isCaliBOk = true;
                                                isCaliCOk = true;
                                            }
                                        }
                                    }

                                    if (isCaliCOk)
                                        SetCaliStall(ch, index, (ushort)pos, i);
                                    TaskClearStallOccurred(ch, index);
                                }

                                if (isBreak)
                                    break;

                                if (isCaliAOk && isCaliBOk)
                                    break;

                                retryFind++;
                            }

                            if (_dutInfos[ch, index].State == DutState.Idle)
                            {
                                break;
                            }

                            if (isResOccurred)
                            {
                                break;
                            }

                            _dutInfos[ch, index].State = DutState.RunCycle;

                            if (_bMonitorStall == 1)
                            {
                                _dutInfos[ch, index].Slave.SetStallDetection(0x01);
                                Thread.Sleep(200);
                            }
                            else
                            {
                                _dutInfos[ch, index].Slave.SetStallDetection(0x00);
                                Thread.Sleep(200);
                            }

                            UpdateLbl(ch, _lblCurrentState[ch, index], @"寻找堵转点结束，\r\n到循环起点");
                            TaskMoveTargetPost(ch, index, (ushort)((_dutInfos[ch, index].Slave.CaliStallA + _dutInfos[ch, index].Slave.CaliStallB) / 2 - _offsetNegative));
                            var runToBeginPosResult = Task.Run(() =>
                            {
                                while (true)
                                {
                                    Thread.Sleep(50);

                                    if (_bMonitorStall == 1 && _dutInfos[ch, index].Slave.Stall_O == 0x01)
                                    {
                                        _dutInfos[ch, index].Slave.SetMode(1);
                                        Thread.Sleep(100);
                                        TaskClearStallOccurred(ch, index);
                                        _dutInfos[ch, index].State = DutState.FindStall;
                                        return 1;
                                    }
                                    else if (_dutInfos[ch, index].BRes > 0)
                                    {
                                        _dutInfos[ch, index].Slave.SetMode(1);
                                        Thread.Sleep(100);
                                        TaskClearRes(ch, index);
                                        _dutInfos[ch, index].State = DutState.FindStall;
                                        return -9999;
                                    }
                                    else if (Math.Abs(_dutInfos[ch, index].Slave.ActualPos - ((ushort)((_dutInfos[ch, index].Slave.CaliStallA + _dutInfos[ch, index].Slave.CaliStallB) / 2 - _offsetNegative))) < 10)
                                    {
                                        return 0;
                                    }

                                    if (_dutInfos[ch, index].State == DutState.Idle)
                                    {
                                        return -1;
                                    }
                                }
                            }).Result;

                            if (runToBeginPosResult == 1 || runToBeginPosResult == -9999)
                            {
                                _dutInfos[ch, index].CaliCount++;
                                break;
                            }

                            if (runToBeginPosResult == -1)
                            {
                                break;
                            }
                        }
                        break;

                    case DutState.RunCycle:
                        for (int i = 0; i < 2; i++)
                        {
                            TaskClearStallOccurred(ch, index);
                            var middle = (ushort)((_dutInfos[ch, index].Slave.CaliStallA + _dutInfos[ch, index].Slave.CaliStallB) / 2);
                            var targetPos = (ushort)(i % 2 == 0 ? middle + _offsetPositive : middle - _offsetNegative);
                            UpdateLbl(ch, _lblCurrentState[ch, index], string.Format("运动到循环点{0}/{1}\r\n（{2}{3}）", i + 1, 2, i % 2 == 0 ? "+" : "-", i % 2 == 0 ? _offsetPositive : _offsetNegative));
                            TaskMoveTargetPost(ch, index, targetPos);
                            Thread.Sleep(200);

                            var runResult = Task.Run(async () =>
                            {
                                while (true)
                                {
                                    Thread.Sleep(5);

                                    if (_bMonitorStall == 1 && _dutInfos[ch, index].Slave.Stall_O == 0x01)
                                    {
                                        _dutInfos[ch, index].Slave.SetMode(1);
                                        Thread.Sleep(100);
                                        TaskClearStallOccurred(ch, index);
                                        _dutInfos[ch, index].State = DutState.FindStall;
                                        return 1;
                                    }
                                    else if (_dutInfos[ch, index].BRes > 0)
                                    {
                                        _dutInfos[ch, index].Slave.SetMode(1);
                                        Thread.Sleep(100);
                                        TaskClearRes(ch, index);
                                        _dutInfos[ch, index].State = DutState.FindStall;
                                        return -9999;
                                    }
                                    else if (Math.Abs(_dutInfos[ch, index].Slave.ActualPos - targetPos) < 10)
                                    {
                                        return 0;
                                    }

                                    if (_dutInfos[ch, index].State == DutState.Idle)
                                    {
                                        return -1;
                                    }
                                }
                            }).Result;

                            if (runResult == 1 | runResult == -9999)
                            {
                                _dutInfos[ch, index].CaliCount++;
                                break;
                            }

                            if (runResult == -1)
                            {
                                break;
                            }

                            Thread.Sleep(5);
                        }

                        _dutInfos[ch, index].CycleCount++;
                        if (_dutInfos[ch, index].CycleCount >= _bCycleMaxCount)
                        {
                            _dutInfos[ch, index].PeriodCount++;
                            if (_dutInfos[ch, index].PeriodCount >= _bPeriodMaxCount)
                            {
                                UpdateIniCount(ch, index);
                                ExecStop(ch, index, _btnStartStop[ch, index]);
                                break;
                            }
                            else
                            {
                                _dutInfos[ch, index].CycleCount = 0;

                                if (_bMonitorStall == 1)
                                {
                                    UpdateIniCount(ch, index);
                                    _dutInfos[ch, index].State = DutState.FindStall;
                                    break;
                                }
                            }
                        }
                        UpdateIniCount(ch, index);
                        Thread.Sleep(_bDefaultCycleDelay);
                        break;

                    default:
                        break;
                }
            }
        }

        //private async Task Worker(int ch, int index)
        //{
        //    await Task.Run(async () =>
        //    {
        //        switch (_dutInfos[ch, index].State)
        //        {
        //            case DutState.Idle:
        //                await WaitStart(ch, index);
        //                break;

        //            case DutState.FindStall:
        //                _dutInfos[ch, index].Slave.SetValidPosition(0);
        //                _dutInfos[ch, index].Slave.SetStallDetection(0x01);
        //                await TaskClearStallOccurred(ch, index);
        //                await TaskClearRes(ch, index);

        //                if (_dutInfos[ch, index].State != DutState.FindStall)
        //                {
        //                    await Worker(ch, index);
        //                    break;
        //                }

        //                var retryFind = 0;
        //                var isResOccurred = false;

        //                while (true)
        //                {
        //                    Thread.Sleep(100);

        //                    var isBreak = false;
        //                    var isCaliAOk = false;
        //                    var isCaliBOk = false;

        //                    for (var i = 0; i < 2; i++)
        //                    {
        //                        await TaskClearStallOccurred(ch, index);
        //                        UpdateLbl(ch, _lblCurrentState[ch, index], string.Format("寻找堵转点{0}，\r\n重試次數{1}", (i + 1), retryFind));
        //                        var pos = await TaskFindStall(ch, index, (ushort)(i % 2 == 0 ? 0 + 1 : 65535 - 1));
        //                        var isCaliCOk = true;

        //                        if (_dutInfos[ch, index].State == DutState.Idle)
        //                        {
        //                            isBreak = true;
        //                            break;
        //                        }

        //                        if (pos == 0)
        //                        {
        //                            isBreak = true;
        //                            break;
        //                        }
        //                        else if (pos == -9999)
        //                        {
        //                            isResOccurred = true;
        //                            isBreak = true;
        //                            await TaskClearRes(ch, index);
        //                            break;
        //                        }
        //                        else
        //                        {
        //                            if (i == 0)
        //                            {
        //                                if (pos == -1)
        //                                {
        //                                    isCaliAOk = false;
        //                                    isCaliCOk = false;
        //                                }
        //                                else
        //                                {
        //                                    isCaliAOk = true;
        //                                    isCaliCOk = true;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (pos == -1)
        //                                {
        //                                    isCaliBOk = false;
        //                                    isCaliCOk = false;
        //                                }
        //                                else
        //                                {
        //                                    isCaliBOk = true;
        //                                    isCaliCOk = true;
        //                                }
        //                            }
        //                        }

        //                        if (isCaliCOk)
        //                            await SetCaliStall(ch, index, (ushort)pos, i);
        //                        await TaskClearStallOccurred(ch, index);
        //                    }

        //                    if (isBreak)
        //                        break;

        //                    if (isCaliAOk && isCaliBOk)
        //                        break;

        //                    retryFind++;
        //                }

        //                if (_dutInfos[ch, index].State == DutState.Idle)
        //                {
        //                    await Worker(ch, index);
        //                    break;
        //                }

        //                if (isResOccurred)
        //                {
        //                    await WaitStart(ch, index);
        //                    break;
        //                }

        //                _dutInfos[ch, index].State = DutState.RunCycle;

        //                if (_bMonitorStall == 1)
        //                {
        //                    _dutInfos[ch, index].Slave.SetStallDetection(0x01);
        //                    Thread.Sleep(200);
        //                }
        //                else
        //                {
        //                    _dutInfos[ch, index].Slave.SetStallDetection(0x00);
        //                    Thread.Sleep(200);
        //                }

        //                UpdateLbl(ch, _lblCurrentState[ch, index], @"寻找堵转点结束，\r\n到循环起点");
        //                await TaskMoveTargetPost(ch, index, (ushort)((_dutInfos[ch, index].Slave.CaliStallA + _dutInfos[ch, index].Slave.CaliStallB) / 2 - _offsetNegative));
        //                var runToBeginPosResult = await Task.Run(async () =>
        //                {
        //                    while (true)
        //                    {
        //                        Thread.Sleep(50);

        //                        if (_bMonitorStall == 1 && _dutInfos[ch, index].Slave.Stall_O == 0x01)
        //                        {
        //                            _dutInfos[ch, index].Slave.SetMode(1);
        //                            Thread.Sleep(100);
        //                            await TaskClearStallOccurred(ch, index);
        //                            _dutInfos[ch, index].State = DutState.FindStall;
        //                            return 1;
        //                        }
        //                        else if (_dutInfos[ch, index].BRes > 0)
        //                        {
        //                            _dutInfos[ch, index].Slave.SetMode(1);
        //                            Thread.Sleep(100);
        //                            await TaskClearRes(ch, index);
        //                            _dutInfos[ch, index].State = DutState.FindStall;
        //                            return -9999;
        //                        }
        //                        else if (Math.Abs(_dutInfos[ch, index].Slave.ActualPos - ((ushort)((_dutInfos[ch, index].Slave.CaliStallA + _dutInfos[ch, index].Slave.CaliStallB) / 2 - _offsetNegative))) < 10)
        //                        {
        //                            return 0;
        //                        }

        //                        if (_dutInfos[ch, index].State == DutState.Idle)
        //                        {
        //                            return -1;
        //                        }
        //                    }
        //                });

        //                if (runToBeginPosResult == 1 || runToBeginPosResult == -9999)
        //                {
        //                    _dutInfos[ch, index].CaliCount++;
        //                    await Worker(ch, index);
        //                    break;
        //                }

        //                if (runToBeginPosResult == -1)
        //                {
        //                    await Worker(ch, index);
        //                    break;
        //                }

        //                await Worker(ch, index);
        //                break;

        //            case DutState.RunCycle:
        //                for (int i = 0; i < 2; i++)
        //                {
        //                    await TaskClearStallOccurred(ch, index);
        //                    var middle = (ushort)((_dutInfos[ch, index].Slave.CaliStallA + _dutInfos[ch, index].Slave.CaliStallB) / 2);
        //                    var targetPos = (ushort)(i % 2 == 0 ? middle + _offsetPositive : middle - _offsetNegative);
        //                    UpdateLbl(ch, _lblCurrentState[ch, index], string.Format("运动到循环点{0}/{1}\r\n（{2}{3}）", i + 1, 2, i % 2 == 0 ? "+" : "-", i % 2 == 0 ? _offsetPositive : _offsetNegative));
        //                    await TaskMoveTargetPost(ch, index, targetPos);
        //                    Thread.Sleep(200);

        //                    var runResult = await Task.Run(async () =>
        //                    {
        //                        while (true)
        //                        {
        //                            Thread.Sleep(5);

        //                            if (_bMonitorStall == 1 && _dutInfos[ch, index].Slave.Stall_O == 0x01)
        //                            {
        //                                _dutInfos[ch, index].Slave.SetMode(1);
        //                                Thread.Sleep(100);
        //                                await TaskClearStallOccurred(ch, index);
        //                                _dutInfos[ch, index].State = DutState.FindStall;
        //                                return 1;
        //                            }
        //                            else if (_dutInfos[ch, index].BRes > 0)
        //                            {
        //                                _dutInfos[ch, index].Slave.SetMode(1);
        //                                Thread.Sleep(100);
        //                                await TaskClearRes(ch, index);
        //                                _dutInfos[ch, index].State = DutState.FindStall;
        //                                return -9999;
        //                            }
        //                            else if (Math.Abs(_dutInfos[ch, index].Slave.ActualPos - targetPos) < 10)
        //                            {
        //                                return 0;
        //                            }

        //                            if (_dutInfos[ch, index].State == DutState.Idle)
        //                            {
        //                                return -1;
        //                            }
        //                        }
        //                    });

        //                    if (runResult == 1 | runResult == -9999)
        //                    {
        //                        _dutInfos[ch, index].CaliCount++;
        //                        await Worker(ch, index);
        //                        break;
        //                    }

        //                    if (runResult == -1)
        //                    {
        //                        await Worker(ch, index);
        //                        break;
        //                    }

        //                    Thread.Sleep(5);
        //                }

        //                _dutInfos[ch, index].CycleCount++;
        //                if (_dutInfos[ch, index].CycleCount >= _bCycleMaxCount)
        //                {
        //                    _dutInfos[ch, index].PeriodCount++;
        //                    if (_dutInfos[ch, index].PeriodCount >= _bPeriodMaxCount)
        //                    {
        //                        UpdateIniCount(ch, index);
        //                        ExecStop(ch, index, _btnStartStop[ch, index]);
        //                        await Worker(ch, index);
        //                        break;
        //                    }
        //                    else
        //                    {
        //                        _dutInfos[ch, index].CycleCount = 0;

        //                        if (_bMonitorStall == 1)
        //                        {
        //                            UpdateIniCount(ch, index);
        //                            _dutInfos[ch, index].State = DutState.FindStall;
        //                            await Worker(ch, index);
        //                            break;
        //                        }
        //                    }
        //                }
        //                UpdateIniCount(ch, index);

        //                Thread.Sleep(_bDefaultCycleDelay);
        //                await Worker(ch, index);
        //                break;

        //            default:
        //                break;
        //        }
        //    }, _cts.Token);
        //}

        private void UpdateIniCount(int ch, int index)
        {
            lock (_lockIni)
            {
                var i = index;
                var dutCycle = ch == 0 ? string.Format("Dut{0}Cycle", i + 1) : string.Format("Lin2Dut{0}Cycle", i + 1);
                var dutPeriod = ch == 0 ? string.Format("Dut{0}Period", i + 1) : string.Format("Lin2Dut{0}Period", i + 1);
                _ini?.IniWriteValue("Sys", dutCycle, _dutInfos[ch, i].CycleCount.ToString());
                _ini?.IniWriteValue("Sys", dutPeriod, _dutInfos[ch, i].PeriodCount.ToString());
            }
        }

        private void WaitStart(int ch, int index)
        {
            SetDefaultValue(ch, _dutInfos[ch, index].NAD);
            Thread.Sleep(15);
            UpdateLbl(ch, _lblCurrentState[ch, index], @"等待开始");
        }

        private async Task<int> TaskFindStall(int ch, int index, ushort targetPos)
        {
            var res = await Task<int>.Run(async () =>
            {
                var beginTs = HighPrecisionTimer.GetTimestamp();
                TaskMoveTargetPost(ch, index, targetPos);
                Thread.Sleep(500);
                var slave = _dutInfos[ch, index].Slave;
                return await Task<int>.Run(async () =>
                {
                    while (true)
                    {
                        Thread.Sleep(50);
                        if (slave.Stall_O == 0x01)
                        {
                            slave.SetMode(1);

                            await Task.Run(() =>
                            {
                                while (true)
                                {
                                    Thread.Sleep(50);
                                    if (_dutInfos[ch, index].State == DutState.Idle || _dutInfos[ch, index].Slave.Mode == 1)
                                        break;
                                }
                            });

                            Thread.Sleep(250);
                            return slave.ActualPos;
                        }

                        if (_dutInfos[ch, index].State == DutState.Idle)
                            return 0;

                        if (HighPrecisionTimer.GetTimestampIntervalMs(beginTs, HighPrecisionTimer.GetTimestamp()) > 20000)
                            return -1;

                        if (_dutInfos[ch, index].BRes > 0)
                            return -9999;
                    }
                }, _dutInfos[ch, index]._cts.Token);
            });

            return res;
        }

        private Task SetCaliStall(int ch, int index, ushort pos, int stallIndex)
        {
            return Task.Run(() =>
            {
                var cid = _dutInfos[ch, index].CID;
                var slaveIndex = _airOutletAutomotiveActuator[ch, 0]._slaves.FindIndex(f => f.MasterLinId == cid && f.IsOnBus);
                if (slaveIndex != -1)
                {
                    if (stallIndex == 0)
                    {
                        _airOutletAutomotiveActuator[ch, 0]._slaves[slaveIndex].CaliStallA = pos;
                    }
                    else
                    {
                        _airOutletAutomotiveActuator[ch, 0]._slaves[slaveIndex].CaliStallB = pos;
                    }
                }
            }, _dutInfos[ch, index]._cts.Token);
        }

        private async void TaskClearStallOccurred(int ch, int index)
        {
            await Task.Run(async () =>
            {
                var cid = _dutInfos[ch, index].CID;
                var slaveIndex = _airOutletAutomotiveActuator[ch, 0]._slaves.FindIndex(f => f.MasterLinId == cid && f.IsOnBus);
                if (slaveIndex != -1)
                {
                    var slave = _airOutletAutomotiveActuator[ch, 0]._slaves[slaveIndex];
                    if (slave.Stall_O > 0)
                    {
                        slave.ClearEvent(4);
                        await Task.Run(() =>
                        {
                            while (true)
                            {
                                Thread.Sleep(50);
                                if (slave.Stall_O == 0x00)
                                    break;

                                if (_dutInfos[ch, index].State == DutState.Idle)
                                    break;

                            }
                        }, _dutInfos[ch, index]._cts.Token);
                    }
                    slave.ClearEvent(0);
                }
            }, _dutInfos[ch, index]._cts.Token);
        }

        private async void TaskClearRes(int ch, int index)
        {
            await Task.Run(async () =>
            {
                var cid = _dutInfos[ch, index].CID;
                var slaveIndex = _airOutletAutomotiveActuator[ch, 0]._slaves.FindIndex(f => f.MasterLinId == cid && f.IsOnBus);
                if (slaveIndex != -1)
                {
                    var slave = _airOutletAutomotiveActuator[ch, 0]._slaves[slaveIndex];
                    //if (slave.Res > 0 )
                    {
                        slave.ClearEvent(14);
                        UpdateLbl(ch, _lblCurrentState[ch, index], string.Format("发送Clear信号，\r\n等待复位完成..."));
                        await Task.Run(() =>
                        {
                            while (true)
                            {
                                Thread.Sleep(50);
                                if (_dutInfos[ch, index].BRes == 0x00 && !_dutInfos[ch, index].IsLinLoss)
                                    break;

                                if (_dutInfos[ch, index].State == DutState.Idle)
                                    break;
                            }
                        }, _dutInfos[ch, index]._cts.Token);
                    }
                    UpdateLbl(ch, _lblCurrentState[ch, index], string.Format("Clear信号清零..."));
                    slave.ClearEvent(0);
                }
            }, _dutInfos[ch, index]._cts.Token);
        }

        private async void TaskMoveTargetPost(int ch, int index, ushort pos)
        {
            await Task.Run(() =>
            {
                var cid = _dutInfos[ch, index].CID;
                var slaveIndex = _airOutletAutomotiveActuator[ch, 0]._slaves.FindIndex(f => f.MasterLinId == cid && f.IsOnBus);
                if (slaveIndex != -1)
                {
                    var slave = _airOutletAutomotiveActuator[ch, 0]._slaves[slaveIndex];
                    slave.SetTargetPosition(pos);
                    Thread.Sleep(200);
                    slave.SetMode(0);
                }
            });
        }

        private void UpdateLbl(int ch, UILabel lbl, string value)
        {
            var updateAct = new Action(() =>
            {
                var parent = lbl.Parent;
                if (_topPanel[ch, 0].Name == parent.Name)
                {
                    if (uiTabControl2.SelectedIndex != ch)
                    {
                        return;
                    }
                }
                else if (_bottomPanel[ch, 0].Name == parent.Name)
                {
                    if (uiTabControl1.SelectedIndex != ch)
                    {
                        return;
                    }
                }

                if (lbl.Text != value)
                    lbl.Text = value;
            });

            if (lbl.InvokeRequired)
                lbl.Invoke(updateAct);
            else
                updateAct.Invoke();
        }

        private void StartStop_Click(object sender, EventArgs e)
        {
            var btn = sender as UIButton;
            if (btn != null && btn.Tag != null)
            {
                var dutIndex = (int[])btn.Tag;
                var ch = dutIndex[0];
                var dut = dutIndex[1];
                if (_dutInfos[ch, dut].State is DutState.Idle)
                {
                    var option = new UIEditOption();
                    option.AddCombobox("type", "是否清零", new string[] { "當前計數不清零", "僅循環计数清零", "周期計數清零" }, 0);

                    var frm = new UIEditForm(option);
                    frm.Render();
                    frm.ShowDialog();

                    if (!frm.IsOK)
                    {
                        UIMessageTip.Show("操作已取消");
                        return;
                    }

                    var type = (ClearType)frm["type"];
                    ExecStart(ch, dut, btn, type);
                }
                else
                {
                    if (UIMessageBox.ShowAsk(string.Format("确定要停止'LIN{0}_#{1}'的测试吗", ch + 1, dut + 1)))
                    {
                        ExecStop(ch, dut, btn);
                    }
                    else
                    {
                        UIMessageTip.Show("操作取消");
                    }
                }
            }
        }

        internal enum ClearType
        {
            NoClear,

            ClearCycle,

            ClearPeriod
        }

        private void ExecStart(int ch, int dutIndex, UIButton btn, ClearType clearType = ClearType.NoClear)
        {
            if (clearType == ClearType.ClearPeriod)
            {
                _dutInfos[ch, dutIndex].PeriodCount = 0;
                _dutInfos[ch, dutIndex].CycleCount = 0;
                _dutInfos[ch, dutIndex].CaliCount = 0;
            }
            else if (clearType == ClearType.ClearCycle)
            {
                _dutInfos[ch, dutIndex].CycleCount = 0;
            }
            else
            {
                if (_dutInfos[ch, dutIndex].CycleCount >= _bCycleMaxCount)
                {
                    _dutInfos[ch, dutIndex].PeriodCount++;
                    if (_dutInfos[ch, dutIndex].PeriodCount >= _bPeriodMaxCount)
                    {
                        _dutInfos[ch, dutIndex].PeriodCount = 0;
                        _dutInfos[ch, dutIndex].CycleCount = 0;
                        _dutInfos[ch, dutIndex].CaliCount = 0;
                    }
                    else
                    {
                        _dutInfos[ch, dutIndex].CycleCount = 0;
                    }
                }
            }

            _dutInfos[ch, dutIndex].State = DutState.FindStall;
            if (InvokeRequired)
                btn.Invoke(new Action(() => { btn.Style = UIStyle.Green; }));
            else
                btn.Style = UIStyle.Green;
        }

        private void ExecStop(int ch, int dutIndex, UIButton btn)
        {
            _dutInfos[ch, dutIndex].State = DutState.Idle;
            if (InvokeRequired)
                btn.Invoke(new Action(() => { btn.Style = UIStyle.Orange; }));
            else
                btn.Style = UIStyle.Orange;
        }

        private async void btnAutomaticAddressing_Click(object sender, EventArgs e)
        {
            for (var ch1 = 0; ch1 < DutCount.GetLength(0); ch1++)
            {
                var isAllFree = true;
                for (var i = 0; i < DutCount[ch1, 0]; i++)
                {
                    if (_dutInfos[ch1, i].State != DutState.Idle)
                    {
                        isAllFree = false;
                        break;
                    }
                }

                if (!isAllFree)
                {
                    UIMessageTip.ShowError("请先停止循环测试");
                    return;
                }
            }

            var option = new UIEditOption();
            option.AddCombobox("type", "使用LIN通道", new string[] { "使用LIN1", "使用LIN2" }, 0);

            var frm = new UIEditForm(option);
            frm.Render();
            frm.ShowDialog();

            if (!frm.IsOK)
            {
                UIMessageTip.Show("操作已取消");
                return;
            }

            var ch = (int)frm["type"];

            var pwd = string.Empty;
            if (!this.InputPasswordDialog(ref pwd))
            {
                UIMessageTip.Show("操作已取消");
                return;
            }
            if (pwd == "098765")
            {
                //ShowSuccessTip("密码正确");
            }
            else
            {
                ShowErrorTip("密码错误");
                return;
            }

            UIMessageTip.Show("密码正确，开始发送自动寻址指令");

            for (var ch1 = 0; ch1 < DutCount.GetLength(0); ch1++)
            {
                if (DutCount[ch1, 0] is 0)
                    continue;
                _airOutletAutomotiveActuator[ch1, 0]?.StopLin();
            }

            var ms = numericUpDown1.Value;
            ms = 200;

            uiTabControl2.Enabled = uiTabControl1.Enabled = false;
            this.ShowProcessForm(200);
            await Task.Run(() => _airOutletAutomotiveActuator[ch, 0].AutomaticAddressing((int)ms));
            this.HideProcessForm();
            uiTabControl2.Enabled = uiTabControl1.Enabled = true;

            for (var ch1 = 0; ch1 < DutCount.GetLength(0); ch1++)
            {
                if (DutCount[ch1, 0] is 0)
                    continue;
                _airOutletAutomotiveActuator[ch1, 0]?.StartLin();
            }

            UIMessageBox.Show("自动寻址指令发送结束");
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            var option = new UIEditOption();
            option.AddCombobox("type", "是否清零", new string[] { "當前計數不清零", "僅循環计数清零", "周期計數清零" }, 0);

            var frm = new UIEditForm(option);
            frm.Render();
            frm.ShowDialog();

            if (!frm.IsOK)
            {
                UIMessageTip.Show("操作已取消");
                return;
            }

            var type = (ClearType)frm["type"];

            for (var ch = 0; ch < DutCount.GetLength(0); ch++)
            {
                if (DutCount[ch, 0] is 0)
                    continue;

                for (var dutIndex = 0; dutIndex < DutCount[ch, 0]; dutIndex++)
                {
                    if (_dutInfos[ch, dutIndex].State is DutState.Idle)
                    {
                        ExecStart(ch, dutIndex, _btnStartStop[ch, dutIndex], type);
                    }
                }
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (!UIMessageBox.ShowAsk("是否要全部停止"))
            {
                UIMessageTip.Show("操作取消");
                return;
            }

            for (var ch = 0; ch < DutCount.GetLength(0); ch++)
            {
                for (int dutIndex = 0; dutIndex < DutCount[ch, 0]; dutIndex++)
                {
                    if (DutCount[ch, 0] is 0)
                        continue;

                    if (_dutInfos[ch, dutIndex].State != DutState.Idle)
                    {
                        ExecStop(ch, dutIndex, _btnStartStop[ch, dutIndex]);
                    }
                }
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            var rootDir = Path.GetPathRoot(Program.SysDir);
            var dir = string.Format(@"{0}\{1}", rootDir, _logDir);

            if (Directory.Exists(dir))
            {
                try
                {
                    ShellExecute(IntPtr.Zero, "explore", dir, null, null, 1); // 1表示正常窗口
                }
                catch (Exception ex)
                {
                    this.ShowErrorTip("打开文件夹失败：" + ex.Message);
                }
            }
            else
            {
                this.ShowErrorTip("路径不存在");
            }
        }

        [DllImport("shell32.dll")]
        static extern IntPtr ShellExecute(IntPtr hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, int nShowCmd);
    }
}
