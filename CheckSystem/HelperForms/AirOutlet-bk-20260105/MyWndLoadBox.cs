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
        private const int DutCount = 6;
        private AirOutletAutomotiveActuator _airOutletAutomotiveActuator = new AirOutletAutomotiveActuator("出风口执行器");
        private ControllerBase _linController;
        private DutInfo[] _dutInfos = new DutInfo[DutCount];
        private CsvFileHelper<SlaveLogData>[] _logWriter = new CsvFileHelper<SlaveLogData>[DutCount];
        private int _boardType = 0;
        private ushort _offset = (6400 / 360 * 30);
        private int _bCycleMaxCount = 900;
        private int _bPeriodMaxCount = 700;
        private int _bDefaultSpeed = 3;
        private int _bDefaultStartPos = 32000;
        private int _bDefaultCycleDelay = 100;
        private int _bMonitorStall = 0;

        // 上半部分控件 - 通道参数表格 
        private UIMarkLabel[] lblRowHeaders;
        private UIMarkLabel[] _lblNad = new UIMarkLabel[DutCount];
        private UIMarkLabel[] _lblCid = new UIMarkLabel[DutCount];
        private UIMarkLabel[] _lblSid = new UIMarkLabel[DutCount];
        private UIMarkLabel[] _lblActualPos = new UIMarkLabel[DutCount];
        private UIMarkLabel[] _lblTargetPos = new UIMarkLabel[DutCount];
        private UIMarkLabel[] _lblDirection = new UIMarkLabel[DutCount];
        private UIMarkLabel[] _lblSpeed = new UIMarkLabel[DutCount];
        private UIMarkLabel[] _lblStallState = new UIMarkLabel[DutCount];
        private UIMarkLabel[] _lblCurrentState = new UIMarkLabel[DutCount];
        private UIMarkLabel[] _lblCycleCount = new UIMarkLabel[DutCount];
        private UIMarkLabel[] _lblPeriodCount = new UIMarkLabel[DutCount];
        private UIMarkLabel[] _lblAutoCaliCount = new UIMarkLabel[DutCount];
        private UIButton[] _btnClear = new UIButton[DutCount];
        private UIButton[] _btnStartStop = new UIButton[DutCount];

        // 下半部分控件 - 循环模式参数
        private UIMarkLabel[] lblBottomRowHeaders;
        private NumericUpDown nudBoardType;
        private NumericUpDown nudSpeed, nudInitPos;
        private NumericUpDown nudCycleCount, nudCycleDelay;
        private NumericUpDown nudPeriodCount;
        private UIButton btnParaReset, btnParaInvoke, btnParaSave;
        private NumericUpDown nudIsMonitorStall;

        private UIMarkLabel[] _lblBottomRowHeaders2;
        private UIMarkLabel[] _lblBottomNad = new UIMarkLabel[DutCount];
        private UIMarkLabel[] _lblBottomSend = new UIMarkLabel[DutCount];
        private UIMarkLabel[] _lblBottomRecv = new UIMarkLabel[DutCount];
        private UIMarkLabel[] _lblBottomCaliA = new UIMarkLabel[DutCount];
        private UIMarkLabel[] _lblBottomCaliB = new UIMarkLabel[DutCount];
        private UIMarkLabel[] _lblBottomMode = new UIMarkLabel[DutCount];

        private UIMarkLabel[] _lblBottomRErr = new UIMarkLabel[DutCount];
        private UIMarkLabel[] _lblBottomOT = new UIMarkLabel[DutCount];
        private UIMarkLabel[] _lblBottomEleD = new UIMarkLabel[DutCount];
        private UIMarkLabel[] _lblBottomSupL = new UIMarkLabel[DutCount];
        private UIMarkLabel[] _lblBottomEmeRO = new UIMarkLabel[DutCount];
        private UIMarkLabel[] _lblBottomRes = new UIMarkLabel[DutCount];
        private UIMarkLabel[] _lblBottomStallDR = new UIMarkLabel[DutCount];

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
                dut._cts?.Cancel();
                dut._cts?.Dispose();
                dut._cts = null;
            }

            m_WorkThread?.Abort();
            m_WorkThread?.Join();
            m_WorkThread = null;

            _linController?.Dispose();
        }

        private void MyWndLoadBox_Load(object sender, EventArgs e)
        {
            LoadPara();
            InitTopPanel();
            InitBottomPanel();
            ResizeUI();
            ResizeTopPanel();
            ResizeBottomPanel();
            panelMainCycle.SizeChanged += (ss, ee) => ResizeTopPanel();
            panelCyclePara.SizeChanged += (ss, ee) => ResizeBottomPanel();
            Resize += (ss, ee) => ResizeUI();

            if (_boardType == 1)
            {
                _linController = new SyControllerWith56Pin("56pin_lin控制器");
                ((SyControllerWith56Pin)_linController).InitRemoteIpAddress("192.168.1.28:8088");
                _airOutletAutomotiveActuator.Lin = ((SyControllerWith56Pin)_linController).GatewayLin;
            }
            else
            {
                _linController = new SyRenesasMcuControllerMaster("瑞萨_lin控制器");
                ((SyRenesasMcuControllerMaster)_linController).InitRemoteIpAddress("192.168.1.28:8088");
                _airOutletAutomotiveActuator.Lin = ((SyRenesasMcuControllerMaster)_linController).GatewayLin1;
            }

            for (var d = 0; d < DutCount; d++)
            {
                var i = d + 1;
                var nad = (byte)(0x26 - (i - 1));
                var cid = (byte)(0x06 - (i - 1));
                _dutInfos[d] = new DutInfo
                {
                    CID = cid,
                    NAD = nad,
                    SID = (byte)(cid + 0x10)
                };
                _airOutletAutomotiveActuator.AddSlave(ValueHelper.GetHextStrWithOx(cid), ValueHelper.GetHextStrWithOx(nad));
                SetDefaultValue(nad);
                _airOutletAutomotiveActuator.Auto_Speed(nad);

                var slaveIndex = _airOutletAutomotiveActuator._slaves.FindIndex(f => f.BindingNad == nad);
                _dutInfos[d].Slave = _airOutletAutomotiveActuator._slaves[slaveIndex];
                _dutInfos[d].CycleCount = _cycleRead[d];
                _dutInfos[d].PeriodCount = _periodRead[d];

                _lblNad[d].Text = ValueHelper.GetHextStrWithOx(_dutInfos[d].NAD);
                _lblCid[d].Text = ValueHelper.GetHextStrWithOx(_dutInfos[d].CID);
                _lblSid[d].Text = ValueHelper.GetHextStrWithOx(_dutInfos[d].SID);
            }

            _airOutletAutomotiveActuator.StartLin();
            CreateWorkder();
        }

        private string _logDir = "AirOutletTestLog";
        private string _setupLog = "TestSetup.ini";
        private IniFileHelper _ini;
        private int[] _cycleRead = new int[DutCount];
        private int[] _periodRead = new int[DutCount];
        private readonly object _lockIni = new object();

        private void LoadPara()
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
                            list.Add("Offset=" + _offset);
                            list.Add("CycleMaxCount=" + _bCycleMaxCount);
                            list.Add("PeriodMaxCount=" + _bPeriodMaxCount);
                            list.Add("DefaultSpeed=" + _bDefaultSpeed);
                            list.Add("DefaultStartPos=" + _bDefaultStartPos);
                            list.Add("DefaultCycleDelay=" + _bDefaultCycleDelay);
                            list.Add("IsMonitorStall=" + _bMonitorStall);

                            for (var i = 0; i < DutCount; i++)
                            {
                                var dutCycle = string.Format("Dut{0}Cycle", i + 1);
                                var dutPeriod = string.Format("Dut{0}Period", i + 1);
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

            if (isDirExist)
            {
                for (var i = 0; i < DutCount; i++)
                {
                    try
                    {
                        var logDir = string.Format(@"{0}\DUT{1}", dir, i + 1);
                        if (!Directory.Exists(logDir))
                            Directory.CreateDirectory(logDir);
                        _logWriter[i] = new CsvFileHelper<SlaveLogData>(logDir);
                        _logWriter[i].Start();
                    }
                    catch (Exception)
                    {
                        _logWriter[i] = null;
                    }
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

                    var cycleTp = new int[DutCount];
                    var periodTp = new int[DutCount];

                    for (var i = 0; i < DutCount; i++)
                    {
                        var dutCycle = string.Format("Dut{0}Cycle", i + 1);
                        var dutPeriod = string.Format("Dut{0}Period", i + 1);

                        cycleTp[i] = int.Parse(_ini.IniReadValue("Sys", dutCycle));
                        periodTp[i] = int.Parse(_ini.IniReadValue("Sys", dutPeriod));
                    }

                    _offset = t1;
                    _bCycleMaxCount = t2;
                    _bPeriodMaxCount = t3;
                    _bDefaultSpeed = t4;
                    _bDefaultStartPos = t5;
                    _bDefaultCycleDelay = t6;
                    _boardType = t7;
                    _bMonitorStall = t8;

                    for (var i = 0; i < DutCount; i++)
                    {
                        _periodRead[i] = periodTp[i];
                        _cycleRead[i] = cycleTp[i];
                    }
                }
                catch (Exception ex)
                {
                    UIMessageBox.ShowError("读取参数配置文件失败，将使用默认参数：" + ex.Message);
                }
            }
        }

        private void SetDefaultValue(byte nad)
        {
            _airOutletAutomotiveActuator.Stop_Mode(nad);
            _airOutletAutomotiveActuator.Start_position_valid(nad);
            _airOutletAutomotiveActuator.Sta_Pos_65535_Signal(nad, (ushort)_bDefaultStartPos);
            _airOutletAutomotiveActuator.Stall_detection_on(nad);
            switch (_bDefaultSpeed)
            {
                case 1:
                    _airOutletAutomotiveActuator.Speed_level_1__2_25_rpm(nad);
                    break;

                case 2:
                    _airOutletAutomotiveActuator.Speed_level_2_2_25_rpm_LessThan_x__3_0_rpm(nad);
                    break;

                case 3:
                    _airOutletAutomotiveActuator.Speed_level_3_3_0_rpm_LessThan_x__4_0_rpm(nad);
                    break;

                case 4:
                    _airOutletAutomotiveActuator.Speed_level_4_4_0_rpm_LessThan_x(nad);
                    break;

                case 5:
                    _airOutletAutomotiveActuator.Auto_Speed(nad);
                    break;

                default:
                    break;
            }
        }

        private async void CreateWorkder()
        {
            for (var dutIndex = 0; dutIndex < DutCount; dutIndex++)
            {
                await WorkTask(dutIndex);
                await WorkThreadProc(dutIndex);
            }
        }

        private void InitTopPanel()
        {
            string[] rowLabels = { "NAD:", "CID:", "SID:", "当前位置(R):", "目标位置(w):", "旋转方向(R):", "速度状态(R):", "堵转状态(R):", "当前进程:", "循环计数:", "周期计数:", "自学习计数:", "计数清零:", "开始/停止:" };
            lblRowHeaders = new UIMarkLabel[rowLabels.Length];

            // 创建行标题
            for (var i = 0; i < rowLabels.Length; i++)
            {
                lblRowHeaders[i] = CreateLabel(rowLabels[i], 0, 0, 70, 22, isBold: true);
                panelMainCycle.Controls.Add(lblRowHeaders[i]);
            }

            // 创建列标题和每列控件
            for (var col = 0; col < DutCount; col++)
            {
                _lblNad[col] = CreateLabel(ValueHelper.GetHextStrWithOx((byte)(0x21 + col)), 0, 0, 60, 20, true, true, true);
                panelMainCycle.Controls.Add(_lblNad[col]);

                _lblCid[col] = CreateLabel(ValueHelper.GetHextStrWithOx((byte)(col + 1)), 0, 0, 60, 20, true, true, true);
                panelMainCycle.Controls.Add(_lblCid[col]);

                _lblSid[col] = CreateLabel(ValueHelper.GetHextStrWithOx((byte)(0x11 + col)), 0, 0, 60, 20, true, true, true);
                panelMainCycle.Controls.Add(_lblSid[col]);

                _lblActualPos[col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, true);
                panelMainCycle.Controls.Add(_lblActualPos[col]);

                _lblTargetPos[col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, true);
                panelMainCycle.Controls.Add(_lblTargetPos[col]);

                _lblDirection[col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, true);
                panelMainCycle.Controls.Add(_lblDirection[col]);

                _lblSpeed[col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, true);
                panelMainCycle.Controls.Add(_lblSpeed[col]);

                _lblStallState[col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, true);
                panelMainCycle.Controls.Add(_lblStallState[col]);

                _lblCurrentState[col] = CreateLabel("等待开始", 0, 0, 60, 20, true, true);
                panelMainCycle.Controls.Add(_lblCurrentState[col]);

                _lblCycleCount[col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, true);
                panelMainCycle.Controls.Add(_lblCycleCount[col]);

                _lblPeriodCount[col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, true);
                panelMainCycle.Controls.Add(_lblPeriodCount[col]);

                _lblAutoCaliCount[col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, true);
                panelMainCycle.Controls.Add(_lblAutoCaliCount[col]);

                _btnClear[col] = CreateButton("Clear", 0, 0, 60, 20, UIStyle.Red);
                _btnClear[col].Tag = col;
                _btnClear[col].Click += (ss, ee) =>
                {
                    var btnClear = ss as UIButton;
                    if (btnClear != null && btnClear.Tag != null)
                    {
                        var dutIndex = (int)btnClear.Tag;
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
                            _dutInfos[dutIndex].CycleCount = 0;
                            UIMessageTip.Show("循环计数已清零");
                        }
                        else if (type == 1)
                        {
                            _dutInfos[dutIndex].CycleCount = 0;
                            _dutInfos[dutIndex].PeriodCount = 0;
                            UIMessageTip.Show("周期计数已清零");
                        }
                        else if (type == 2)
                        {
                            _dutInfos[dutIndex].CaliCount = 0;
                            UIMessageTip.Show("自學習计数已清零");
                        }
                    }
                };
                panelMainCycle.Controls.Add(_btnClear[col]);

                _btnStartStop[col] = CreateButton("#" + (col + 1), 0, 0, 60, 22, UIStyle.Orange);
                _btnStartStop[col].Tag = col;
                _btnStartStop[col].Click += StartStop_Click;
                panelMainCycle.Controls.Add(_btnStartStop[col]);
            }
        }

        private void InitBottomPanel()
        {
            {
                string[] rowLabels = { "通信卡类型", "速度等级:", "初始化位置:", "循环次数:", "循环延时:", "周期次数:", "是否循环中监控堵转:" };
                lblBottomRowHeaders = new UIMarkLabel[rowLabels.Length];

                // 创建行标题
                for (var i = 0; i < lblBottomRowHeaders.Length; i++)
                {
                    lblBottomRowHeaders[i] = CreateLabel(rowLabels[i], 0, 0, 70, 22, isBold: true);
                    panelCyclePara.Controls.Add(lblBottomRowHeaders[i]);
                }

                // 创建列标题和每列控件
                nudBoardType = CreateNumericUpDown(0, 0, 80, 0, 1, _boardType);
                panelCyclePara.Controls.Add(nudBoardType);

                nudSpeed = CreateNumericUpDown(0, 0, 80, 1, 5, _bDefaultSpeed);
                panelCyclePara.Controls.Add(nudSpeed);

                nudInitPos = CreateNumericUpDown(0, 0, 80, 0, 65535, _bDefaultStartPos);
                panelCyclePara.Controls.Add(nudInitPos);

                nudCycleCount = CreateNumericUpDown(0, 0, 80, 1, 10000, _bCycleMaxCount);
                panelCyclePara.Controls.Add(nudCycleCount);

                nudCycleDelay = CreateNumericUpDown(0, 0, 80, 1, 5000, _bDefaultCycleDelay);
                panelCyclePara.Controls.Add(nudCycleDelay);

                nudPeriodCount = CreateNumericUpDown(0, 0, 80, 1, 1000000, _bPeriodMaxCount);
                panelCyclePara.Controls.Add(nudPeriodCount);

                nudIsMonitorStall = CreateNumericUpDown(0, 0, 80, 0, 1, _bMonitorStall);
                panelCyclePara.Controls.Add(nudIsMonitorStall);

                btnParaReset = CreateButton("一键重置", 0, 0, 70, 22, UIStyle.Blue);
                btnParaReset.Click += (ss, ee) =>
                {
                    nudSpeed.Value = _bDefaultSpeed;
                    nudInitPos.Value = _bDefaultStartPos;
                    nudCycleCount.Value = _bCycleMaxCount;
                    nudCycleDelay.Value = _bDefaultCycleDelay;
                    nudPeriodCount.Value = _bPeriodMaxCount;
                    nudIsMonitorStall.Value = _bMonitorStall;
                };
                panelCyclePara.Controls.Add(btnParaReset);

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

                    for (var i = 0; i < DutCount; i++)
                    {
                        _dutInfos[i].Slave.SetStartPosition((ushort)_bDefaultStartPos);
                        _dutInfos[i].Slave.SetSpeedLevel((byte)_bDefaultSpeed);
                    }

                    UIMessageTip.Show("已应用");
                };
                panelCyclePara.Controls.Add(btnParaInvoke);

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
                        _ini.IniWriteValue("Sys", "Offset", _offset.ToString());
                        _ini.IniWriteValue("Sys", "CycleMaxCount", _bCycleMaxCount.ToString());
                        _ini.IniWriteValue("Sys", "PeriodMaxCount", _bPeriodMaxCount.ToString());
                        _ini.IniWriteValue("Sys", "DefaultSpeed", _bDefaultSpeed.ToString());
                        _ini.IniWriteValue("Sys", "DefaultStartPos", _bDefaultStartPos.ToString());
                        _ini.IniWriteValue("Sys", "DefaultCycleDelay", _bDefaultCycleDelay.ToString());
                        _ini.IniWriteValue("Sys", "BoardType", _boardType.ToString());
                        _ini.IniWriteValue("Sys", "IsMonitorStall", _bMonitorStall.ToString());
                    }
                    catch (Exception ex)
                    {
                        UIMessageBox.ShowError("保存失败：" + ex.Message);
                    }
                };
                panelCyclePara.Controls.Add(btnParaSave);
            }

            {
                string[] rowLabels = { "NAD:", "SEND(W):", "RECV(R):", "堵轉點A:", "堵轉點B:", "啓停狀態(R):", "LIN故障(R)", "过温故障(R)", "电器故障(R)", "电压故障(R)", "紧急操作(R)", "复位状态(R)", "堵转使能(W)" };
                _lblBottomRowHeaders2 = new UIMarkLabel[rowLabels.Length];

                // 创建行标题
                for (var i = 0; i < _lblBottomRowHeaders2.Length; i++)
                {
                    _lblBottomRowHeaders2[i] = CreateLabel(rowLabels[i], 0, 0, 70, 22, isBold: true);
                    panelCyclePara.Controls.Add(_lblBottomRowHeaders2[i]);
                }

                // 创建列标题和每列控件
                for (var col = 0; col < DutCount; col++)
                {
                    _lblBottomNad[col] = CreateLabel(ValueHelper.GetHextStrWithOx((byte)(0x21 + col)), 0, 0, 60, 20, true, true, true);
                    panelCyclePara.Controls.Add(_lblBottomNad[col]);

                    _lblBottomSend[col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, true, false);
                    panelCyclePara.Controls.Add(_lblBottomSend[col]);

                    _lblBottomRecv[col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, true, false);
                    panelCyclePara.Controls.Add(_lblBottomRecv[col]);

                    _lblBottomCaliA[col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, false);
                    panelCyclePara.Controls.Add(_lblBottomCaliA[col]);

                    _lblBottomCaliB[col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, false);
                    panelCyclePara.Controls.Add(_lblBottomCaliB[col]);

                    _lblBottomMode[col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, false);
                    panelCyclePara.Controls.Add(_lblBottomMode[col]);

                    _lblBottomRErr[col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, false);
                    panelCyclePara.Controls.Add(_lblBottomRErr[col]);

                    _lblBottomOT[col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, false);
                    panelCyclePara.Controls.Add(_lblBottomOT[col]);

                    _lblBottomEleD[col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, false);
                    panelCyclePara.Controls.Add(_lblBottomEleD[col]);

                    _lblBottomSupL[col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, false);
                    panelCyclePara.Controls.Add(_lblBottomSupL[col]);

                    _lblBottomEmeRO[col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, false);
                    panelCyclePara.Controls.Add(_lblBottomEmeRO[col]);

                    _lblBottomRes[col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, false);
                    panelCyclePara.Controls.Add(_lblBottomRes[col]);

                    _lblBottomStallDR[col] = CreateLabel(string.Empty, 0, 0, 60, 20, true, false);
                    panelCyclePara.Controls.Add(_lblBottomStallDR[col]);
                }
            }
        }

        private UIMarkLabel CreateLabel(string text, int x, int y, int width, int height, bool isBottomLine = false, bool isShowBorder = false, bool isBold = false)
        {
            return new UIMarkLabel
            {
                Text = text,
                Location = new Point(x, y),
                //Font = new Font("微软雅黑", isLowDpi ? 7 : 9, isLowDpi ? FontStyle.Italic : (isBold ? FontStyle.Bold : FontStyle.Italic)),
                Font = new Font("微软雅黑", 9, isBold ? FontStyle.Bold : FontStyle.Italic),
                TextAlign = ContentAlignment.MiddleCenter,
                MarkPos = isBottomLine ? UIMarkLabel.UIMarkPos.Bottom : UIMarkLabel.UIMarkPos.Left,
                BorderStyle = !isShowBorder ? BorderStyle.None : BorderStyle.FixedSingle,
                ForeColor = Color.WhiteSmoke,
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
            {
                uiSplitContainer1.SplitterDistance = Height / 2;
            }
        }

        const int lowDpiFontSize = 6;
        const int highDpiFontSize = 9;

        private void ResizeTopPanel()
        {
            var screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            var screenHigh = Screen.PrimaryScreen.WorkingArea.Height;
            var isLowDpi = screenWidth < 1280 || screenHigh < (1024 - 0);

            var panelWidth = panelMainCycle.ClientSize.Width;
            var panelHeight = panelMainCycle.ClientSize.Height;

            var labelWidth = isLowDpi ? 120 : 180;
            var availableWidth = panelWidth - labelWidth - 20;
            var colWidth = availableWidth / DutCount;
            var rowHeight = Math.Max(20, (panelHeight - 10) / 14);
            var startX = labelWidth + 5;
            var startY = 5;

            // 布局行标题
            for (var i = 0; i < lblRowHeaders.Length; i++)
            {
                //var y = startY + (i + 1) * rowHeight;
                var y = startY + (i + 0) * rowHeight;
                lblRowHeaders[i].Location = new Point(5, y);
                lblRowHeaders[i].Size = new Size(labelWidth - 5, rowHeight - 2);
            }

            // 布局每列控件
            for (var col = 0; col < DutCount; col++)
            {
                var x = startX + col * colWidth;
                var w = colWidth - 4;
                var row = -1;

                _lblNad[col].Location = new Point(x, startY + (++row) * rowHeight);
                _lblNad[col].Size = new Size(w, rowHeight - 2);
                _lblNad[col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblNad[col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                _lblCid[col].Location = new Point(x, startY + (++row) * rowHeight);
                _lblCid[col].Size = new Size(w, rowHeight - 2);
                _lblCid[col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblCid[col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                _lblSid[col].Location = new Point(x, startY + (++row) * rowHeight);
                _lblSid[col].Size = new Size(w, rowHeight - 2);
                _lblSid[col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblSid[col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                _lblActualPos[col].Location = new Point(x, startY + (++row) * rowHeight);
                _lblActualPos[col].Size = new Size(w, rowHeight - 2);
                _lblActualPos[col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblActualPos[col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                _lblTargetPos[col].Location = new Point(x, startY + (++row) * rowHeight);
                _lblTargetPos[col].Size = new Size(w, rowHeight - 2);
                _lblTargetPos[col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblTargetPos[col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                _lblDirection[col].Location = new Point(x, startY + (++row) * rowHeight);
                _lblDirection[col].Size = new Size(w, rowHeight - 2);
                _lblDirection[col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblDirection[col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                _lblSpeed[col].Location = new Point(x, startY + (++row) * rowHeight);
                _lblSpeed[col].Size = new Size(w, rowHeight - 2);
                _lblSpeed[col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblSpeed[col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                _lblStallState[col].Location = new Point(x, startY + (++row) * rowHeight);
                _lblStallState[col].Size = new Size(w, rowHeight - 2);
                _lblStallState[col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblStallState[col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                _lblCurrentState[col].Location = new Point(x, startY + (++row) * rowHeight);
                _lblCurrentState[col].Size = new Size(w, rowHeight - 2);
                _lblCurrentState[col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblCurrentState[col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                _lblCycleCount[col].Location = new Point(x, startY + (++row) * rowHeight);
                _lblCycleCount[col].Size = new Size(w, rowHeight - 2);
                _lblCycleCount[col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblCycleCount[col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                _lblPeriodCount[col].Location = new Point(x, startY + (++row) * rowHeight);
                _lblPeriodCount[col].Size = new Size(w, rowHeight - 2);
                _lblPeriodCount[col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblPeriodCount[col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                _lblAutoCaliCount[col].Location = new Point(x, startY + (++row) * rowHeight);
                _lblAutoCaliCount[col].Size = new Size(w, rowHeight - 2);
                _lblAutoCaliCount[col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblAutoCaliCount[col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                _btnClear[col].Location = new Point(x, startY + (++row) * rowHeight);
                _btnClear[col].Size = new Size(w, rowHeight - 2);
                //_btnClear[col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _btnClear[col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                _btnStartStop[col].Location = new Point(x, startY + (++row) * rowHeight);
                _btnStartStop[col].Size = new Size(w, rowHeight - 2);
                //_btnStartStop[col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _btnStartStop[col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);
            }
        }

        private void ResizeBottomPanel()
        {
            var screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            var screenHigh = Screen.PrimaryScreen.WorkingArea.Height;
            var isLowDpi = screenWidth < 1280 || screenHigh < (1024 - 0);

            {
                var panelWidth = panelCyclePara.ClientSize.Width;
                var panelHeight = panelCyclePara.ClientSize.Height;

                var labelWidth = isLowDpi ? 100 : 180;
                var availableWidth = panelWidth - labelWidth - 20;
                var colWidth = 100;
                var rowHeight = isLowDpi ? 30 : 35;
                var startX = labelWidth + 5;
                var startY = 5;

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
                var panelWidth = panelCyclePara.ClientSize.Width;
                var panelHeight = panelCyclePara.ClientSize.Height;

                var labelWidth = isLowDpi ? 100 : 180;
                var availableWidth = panelWidth - (labelWidth + nudPeriodCount.Location.X + nudPeriodCount.Size.Width + 5) - 20;
                var colWidth = availableWidth / DutCount;
                var rowHeight = Math.Max(20, (panelHeight - 10) / 14);
                var startX = labelWidth + 5;
                var startY = 5;

                // 布局行标题
                for (var i = 0; i < _lblBottomRowHeaders2.Length; i++)
                {
                    var y = startY + (i + 0) * rowHeight;
                    _lblBottomRowHeaders2[i].Location = new Point(nudPeriodCount.Location.X + nudPeriodCount.Size.Width + 5, y);
                    _lblBottomRowHeaders2[i].Size = new Size(labelWidth - 5, rowHeight - 2);
                }

                // 布局每列控件
                for (var col = 0; col < DutCount; col++)
                {
                    var x = nudPeriodCount.Location.X + nudPeriodCount.Size.Width + startX + col * colWidth;
                    var w = colWidth - 4;
                    var row = -1;

                    _lblBottomNad[col].Location = new Point(x, startY + (++row) * rowHeight);
                    _lblBottomNad[col].Size = new Size(w, rowHeight - 2);
                    _lblBottomNad[col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblBottomNad[col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                    _lblBottomSend[col].Location = new Point(x, startY + (++row) * rowHeight);
                    _lblBottomSend[col].Size = new Size(w, rowHeight - 2);
                    _lblBottomSend[col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblBottomSend[col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                    _lblBottomRecv[col].Location = new Point(x, startY + (++row) * rowHeight);
                    _lblBottomRecv[col].Size = new Size(w, rowHeight - 2);
                    _lblBottomRecv[col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblBottomRecv[col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                    _lblBottomCaliA[col].Location = new Point(x, startY + (++row) * rowHeight);
                    _lblBottomCaliA[col].Size = new Size(w, rowHeight - 2);
                    _lblBottomCaliA[col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblBottomCaliA[col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                    _lblBottomCaliB[col].Location = new Point(x, startY + (++row) * rowHeight);
                    _lblBottomCaliB[col].Size = new Size(w, rowHeight - 2);
                    _lblBottomCaliB[col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblBottomCaliB[col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                    _lblBottomMode[col].Location = new Point(x, startY + (++row) * rowHeight);
                    _lblBottomMode[col].Size = new Size(w, rowHeight - 2);
                    _lblBottomMode[col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblBottomMode[col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                    _lblBottomRErr[col].Location = new Point(x, startY + (++row) * rowHeight);
                    _lblBottomRErr[col].Size = new Size(w, rowHeight - 2);
                    _lblBottomRErr[col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblBottomRErr[col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                    _lblBottomOT[col].Location = new Point(x, startY + (++row) * rowHeight);
                    _lblBottomOT[col].Size = new Size(w, rowHeight - 2);
                    _lblBottomOT[col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblBottomOT[col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                    _lblBottomEleD[col].Location = new Point(x, startY + (++row) * rowHeight);
                    _lblBottomEleD[col].Size = new Size(w, rowHeight - 2);
                    _lblBottomEleD[col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblBottomEleD[col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                    _lblBottomSupL[col].Location = new Point(x, startY + (++row) * rowHeight);
                    _lblBottomSupL[col].Size = new Size(w, rowHeight - 2);
                    _lblBottomSupL[col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblBottomSupL[col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                    _lblBottomEmeRO[col].Location = new Point(x, startY + (++row) * rowHeight);
                    _lblBottomEmeRO[col].Size = new Size(w, rowHeight - 2);
                    _lblBottomEmeRO[col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblBottomEmeRO[col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                    _lblBottomRes[col].Location = new Point(x, startY + (++row) * rowHeight);
                    _lblBottomRes[col].Size = new Size(w, rowHeight - 2);
                    _lblBottomRes[col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblBottomRes[col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);

                    _lblBottomStallDR[col].Location = new Point(x, startY + (++row) * rowHeight);
                    _lblBottomStallDR[col].Size = new Size(w, rowHeight - 2);
                    _lblBottomStallDR[col].Font = new Font("微软雅黑", isLowDpi ? lowDpiFontSize : highDpiFontSize, isLowDpi ? FontStyle.Italic : _lblBottomStallDR[col].Font.Bold ? FontStyle.Bold : FontStyle.Italic);
                }
            }
        }

        [DllImport("Kernel32.dll")] public static extern IntPtr GetCurrentThread();
        [DllImport("Kernel32.dll")] public static extern int SetThreadIdealProcessor(IntPtr hThread, uint dwIdealProcessor);
        private Thread m_WorkThread;

        private long[] _logTs = new long[DutCount];

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

        private async Task WorkThreadProc(int dutIndex)
        {
            await Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    await Task.Delay(100);
                    var slave = _dutInfos[dutIndex].Slave;
                    if (slave != null)
                    {
                        var nowTs = HighPrecisionTimer.GetTimestamp();
                        await UpdateLbl(_lblTargetPos[dutIndex], slave.TargetPos.ToString());
                        await UpdateLbl(_lblCycleCount[dutIndex], string.Format("{0}/{1}", _dutInfos[dutIndex].CycleCount.ToString(), _bCycleMaxCount));
                        await UpdateLbl(_lblPeriodCount[dutIndex], string.Format("{0}/{1}", _dutInfos[dutIndex].PeriodCount.ToString(), _bPeriodMaxCount));
                        await UpdateLbl(_lblAutoCaliCount[dutIndex], _dutInfos[dutIndex].CaliCount.ToString());
                        await UpdateLbl(_lblBottomNad[dutIndex], ValueHelper.GetHextStrWithOx(_dutInfos[dutIndex].NAD));
                        await UpdateLbl(_lblBottomSend[dutIndex], _dutInfos[dutIndex].Slave.SendBytes is null ? string.Empty : ValueHelper.GetHextStr(_dutInfos[dutIndex].Slave.SendBytes));
                        await UpdateLbl(_lblBottomStallDR[dutIndex], _stall_d_r.ContainsKey(slave.Stall_D_R) ? _stall_d_r[slave.Stall_D_R] : ValueHelper.GetHextStrWithOx(slave.Stall_D_R));

                        await Task.Run(() =>
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
                        });

                        var isLossLin = HighPrecisionTimer.GetTimestampIntervalMs(_dutInfos[dutIndex].Slave.RecvTs, nowTs) > 3500;
                        if (isLossLin)
                        {
                            await UpdateLbl(_lblBottomRecv[dutIndex], string.Empty);
                            await UpdateLbl(_lblBottomMode[dutIndex], string.Empty);
                            await UpdateLbl(_lblActualPos[dutIndex], string.Empty);
                            await UpdateLbl(_lblDirection[dutIndex], string.Empty);
                            await UpdateLbl(_lblSpeed[dutIndex], string.Empty);
                            await UpdateLbl(_lblStallState[dutIndex], string.Empty);

                            await UpdateLbl(_lblBottomRErr[dutIndex], string.Empty);
                            await UpdateLbl(_lblBottomOT[dutIndex], string.Empty);
                            await UpdateLbl(_lblBottomEleD[dutIndex], string.Empty);
                            await UpdateLbl(_lblBottomSupL[dutIndex], string.Empty);
                            await UpdateLbl(_lblBottomEmeRO[dutIndex], string.Empty);
                            await UpdateLbl(_lblBottomRes[dutIndex], string.Empty);

                            _dutInfos[dutIndex].BRes = 1;
                        }
                        else
                        {
                            await UpdateLbl(_lblBottomRecv[dutIndex], _dutInfos[dutIndex].Slave.RecvBytes is null ? string.Empty : ValueHelper.GetHextStr(_dutInfos[dutIndex].Slave.RecvBytes));
                            await UpdateLbl(_lblBottomMode[dutIndex], ValueHelper.GetHextStrWithOx(_dutInfos[dutIndex].Slave.Mode));
                            await UpdateLbl(_lblActualPos[dutIndex], slave.ActualPos.ToString());
                            await UpdateLbl(_lblDirection[dutIndex], _rot_dir.ContainsKey(slave.Rot_Dir) ? _rot_dir[slave.Rot_Dir] : ValueHelper.GetHextStrWithOx(slave.Rot_Dir));
                            await UpdateLbl(_lblSpeed[dutIndex], _spe_s.ContainsKey(slave.Spe_S) ? _spe_s[slave.Spe_S] : ValueHelper.GetHextStrWithOx(slave.Spe_S));
                            await UpdateLbl(_lblStallState[dutIndex], _stall_o.ContainsKey(slave.Stall_O) ? _stall_o[slave.Stall_O] : ValueHelper.GetHextStrWithOx(slave.Stall_O));

                            await UpdateLbl(_lblBottomRErr[dutIndex], _r_err.ContainsKey(slave.R_Err) ? _r_err[slave.R_Err] : ValueHelper.GetHextStrWithOx(slave.R_Err));
                            await UpdateLbl(_lblBottomOT[dutIndex], _o_t.ContainsKey(slave.O_T) ? _o_t[slave.O_T] : ValueHelper.GetHextStrWithOx(slave.O_T));
                            await UpdateLbl(_lblBottomEleD[dutIndex], _ele_d.ContainsKey(slave.Ele_D) ? _ele_d[slave.Ele_D] : ValueHelper.GetHextStrWithOx(slave.Ele_D));
                            await UpdateLbl(_lblBottomSupL[dutIndex], _sup_v.ContainsKey(slave.Sup_V) ? _sup_v[slave.Sup_V] : ValueHelper.GetHextStrWithOx(slave.Sup_V));
                            await UpdateLbl(_lblBottomEmeRO[dutIndex], _eme_r_o.ContainsKey(slave.Eme_R_O) ? _eme_r_o[slave.Eme_R_O] : ValueHelper.GetHextStrWithOx(slave.Eme_R_O));
                            await UpdateLbl(_lblBottomRes[dutIndex], _res.ContainsKey(slave.Res) ? _res[slave.Res] : ValueHelper.GetHextStrWithOx(slave.Res));
                            _dutInfos[dutIndex].BRes = slave.Res;
                        }

                        _dutInfos[dutIndex].IsLinLoss = isLossLin;

                        await UpdateLbl(_lblBottomCaliA[dutIndex], _dutInfos[dutIndex].Slave.CaliStallA.ToString());
                        await UpdateLbl(_lblBottomCaliB[dutIndex], _dutInfos[dutIndex].Slave.CaliStallB.ToString());

                        if (_dutInfos[dutIndex].State != DutState.Idle && _lblCurrentState[dutIndex].Text != "等待开始" && HighPrecisionTimer.GetTimestampIntervalMs(_logTs[dutIndex], nowTs) > 300)
                        {
                            var logData = new SlaveLogData
                            {
                                Ts = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"),
                                Mode = _lblCurrentState[dutIndex].Text,
                                SendData = ValueHelper.GetHextStrWithOx(slave.SendBytes),
                                RecvData = isLossLin ? string.Empty : ValueHelper.GetHextStrWithOx(slave.RecvBytes),
                                CycleCount = _dutInfos[dutIndex].CycleCount,
                                PeriodCount = _dutInfos[dutIndex].PeriodCount,
                                CaliCount = _dutInfos[dutIndex].CaliCount,
                                CaliA = slave.CaliStallA,
                                CaliB = slave.CaliStallB,
                            };
                            _logWriter[dutIndex]?.EnqueueData(logData);
                        }
                    }
                }
            }, _dutInfos[dutIndex]._cts.Token);
        }

        private async Task WorkTask(int index)
        {
            await Task.Factory.StartNew(async () => await Worker(index), _dutInfos[index]._cts.Token);
        }

        private async Task Worker(int index)
        {
            switch (_dutInfos[index].State)
            {
                case DutState.Idle:
                    await WaitStart(index);
                    break;

                case DutState.FindStall:
                    _dutInfos[index].Slave.SetValidPosition(0);
                    _dutInfos[index].Slave.SetStallDetection(0x01);
                    await TaskClearStallOccurred(index);
                    await TaskClearRes(index);

                    if (_dutInfos[index].State != DutState.FindStall)
                    {
                        await Worker(index);
                        break;
                    }

                    var retryFind = 0;
                    var isResOccurred = false;

                    while (true)
                    {
                        await Task.Delay(100);

                        var isBreak = false;
                        var isCaliAOk = false;
                        var isCaliBOk = false;

                        for (var i = 0; i < 2; i++)
                        {
                            await TaskClearStallOccurred(index);
                            await UpdateLbl(_lblCurrentState[index], string.Format("寻找堵转点{0}，重試次數{1}", (i + 1), retryFind));
                            var pos = await TaskFindStall(index, (ushort)(i % 2 == 0 ? 0 + 1 : 65535 - 1));
                            var isCaliCOk = true;

                            if (_dutInfos[index].State == DutState.Idle)
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
                                await TaskClearRes(index);
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
                                await SetCaliStall(index, (ushort)pos, i);
                            await TaskClearStallOccurred(index);
                        }

                        if (isBreak)
                            break;

                        if (isCaliAOk && isCaliBOk)
                            break;

                        retryFind++;
                    }

                    if (_dutInfos[index].State == DutState.Idle)
                    {
                        await Worker(index);
                        break;
                    }

                    if (isResOccurred)
                    {
                        await WaitStart(index);
                        break;
                    }

                    _dutInfos[index].State = DutState.RunCycle;

                    if (_bMonitorStall == 1)
                    {
                        _dutInfos[index].Slave.SetStallDetection(0x01);
                        await Task.Delay(200);
                    }
                    else
                    {
                        _dutInfos[index].Slave.SetStallDetection(0x00);
                        await Task.Delay(200);
                    }

                    await UpdateLbl(_lblCurrentState[index], @"寻找堵转点结束，到循环起点");
                    await TaskMoveTargetPost(index, (ushort)((_dutInfos[index].Slave.CaliStallA + _dutInfos[index].Slave.CaliStallB) / 2 - _offset));
                    var runToBeginPosResult = await Task.Run(async () =>
                     {
                         while (true)
                         {
                             await Task.Delay(50);

                             if (_bMonitorStall == 1 && _dutInfos[index].Slave.Stall_O == 0x01)
                             {
                                 _dutInfos[index].Slave.SetMode(1);
                                 await Task.Delay(100);
                                 await TaskClearStallOccurred(index);
                                 _dutInfos[index].State = DutState.FindStall;
                                 return 1;
                             }
                             else if (_dutInfos[index].BRes > 0)
                             {
                                 _dutInfos[index].Slave.SetMode(1);
                                 await Task.Delay(100);
                                 await TaskClearRes(index);
                                 _dutInfos[index].State = DutState.FindStall;
                                 return -9999;
                             }
                             else if (Math.Abs(_dutInfos[index].Slave.ActualPos - ((ushort)((_dutInfos[index].Slave.CaliStallA + _dutInfos[index].Slave.CaliStallB) / 2 - _offset))) < 10)
                             {
                                 return 0;
                             }

                             if (_dutInfos[index].State == DutState.Idle)
                             {
                                 return -1;
                             }
                         }
                     });

                    if (runToBeginPosResult == 1 || runToBeginPosResult == -9999)
                    {
                        _dutInfos[index].CaliCount++;
                        await Worker(index);
                        break;
                    }

                    if (runToBeginPosResult == -1)
                    {
                        await Worker(index);
                        break;
                    }

                    await Worker(index);
                    break;

                case DutState.RunCycle:
                    for (int i = 0; i < 2; i++)
                    {
                        await TaskClearStallOccurred(index);
                        var middle = (ushort)((_dutInfos[index].Slave.CaliStallA + _dutInfos[index].Slave.CaliStallB) / 2);
                        var targetPos = (ushort)(i % 2 == 0 ? middle + _offset : middle - _offset);
                        await UpdateLbl(_lblCurrentState[index], string.Format("运动到循环点{0}/{1}（{2}{3}）", i + 1, 2, i % 2 == 0 ? "+" : "-", _offset));
                        await TaskMoveTargetPost(index, targetPos);
                        await Task.Delay(200);

                        var runResult = await Task.Run(async () =>
                        {
                            while (true)
                            {
                                await Task.Delay(5);

                                if (_bMonitorStall == 1 && _dutInfos[index].Slave.Stall_O == 0x01)
                                {
                                    _dutInfos[index].Slave.SetMode(1);
                                    await Task.Delay(100);
                                    await TaskClearStallOccurred(index);
                                    _dutInfos[index].State = DutState.FindStall;
                                    return 1;
                                }
                                else if (_dutInfos[index].BRes > 0)
                                {
                                    _dutInfos[index].Slave.SetMode(1);
                                    await Task.Delay(100);
                                    await TaskClearRes(index);
                                    _dutInfos[index].State = DutState.FindStall;
                                    return -9999;
                                }
                                else if (Math.Abs(_dutInfos[index].Slave.ActualPos - targetPos) < 10)
                                {
                                    return 0;
                                }

                                if (_dutInfos[index].State == DutState.Idle)
                                {
                                    return -1;
                                }
                            }
                        });

                        if (runResult == 1 | runResult == -9999)
                        {
                            _dutInfos[index].CaliCount++;
                            await Worker(index);
                            break;
                        }

                        if (runResult == -1)
                        {
                            await Worker(index);
                            break;
                        }

                        await Task.Delay(5);
                    }

                    _dutInfos[index].CycleCount++;
                    if (_dutInfos[index].CycleCount >= _bCycleMaxCount)
                    {
                        _dutInfos[index].PeriodCount++;
                        if (_dutInfos[index].PeriodCount >= _bPeriodMaxCount)
                        {
                            UpdateIniCount(index);
                            ExecStop(index, _btnStartStop[index]);
                            await Worker(index);
                            break;
                        }
                        else
                        {
                            _dutInfos[index].CycleCount = 0;

                            if (_bMonitorStall == 1)
                            {
                                UpdateIniCount(index);
                                _dutInfos[index].State = DutState.FindStall;
                                await Worker(index);
                                break;
                            }
                        }
                    }
                    UpdateIniCount(index);

                    await Task.Delay(_bDefaultCycleDelay);
                    await Worker(index);
                    break;

                default:
                    break;
            }
        }

        private void UpdateIniCount(int index)
        {
            lock (_lockIni)
            {
                var i = index;
                var dutCycle = string.Format("Dut{0}Cycle", i + 1);
                var dutPeriod = string.Format("Dut{0}Period", i + 1);
                _ini?.IniWriteValue("Sys", dutCycle, _dutInfos[i].CycleCount.ToString());
                _ini?.IniWriteValue("Sys", dutPeriod, _dutInfos[i].PeriodCount.ToString());
            }
        }

        private async Task WaitStart(int index)
        {
            SetDefaultValue(_dutInfos[index].NAD);
            await Task.Delay(50);
            await UpdateLbl(_lblCurrentState[index], @"等待开始");
            await Worker(index);
        }

        private async Task<int> TaskFindStall(int index, ushort targetPos)
        {
            var pos = await Task.Run(async () =>
             {
                 var beginTs = HighPrecisionTimer.GetTimestamp();
                 await TaskMoveTargetPost(index, targetPos);
                 await Task.Delay(500);
                 var slave = _dutInfos[index].Slave;
                 return await Task.Run(async () =>
                 {
                     while (true)
                     {
                         await Task.Delay(50);
                         if (slave.Stall_O == 0x01)
                         {
                             slave.SetMode(1);

                             await Task.Run(async () =>
                             {
                                 while (true)
                                 {
                                     await Task.Delay(50);
                                     if (_dutInfos[index].State == DutState.Idle || _dutInfos[index].Slave.Mode == 1)
                                         break;
                                 }
                             });

                             await Task.Delay(250);
                             return slave.ActualPos;
                         }

                         if (_dutInfos[index].State == DutState.Idle)
                             return 0;

                         if (HighPrecisionTimer.GetTimestampIntervalMs(beginTs, HighPrecisionTimer.GetTimestamp()) > 20000)
                             return -1;

                         if (_dutInfos[index].BRes > 0)
                             return -9999;
                     }
                 }, _dutInfos[index]._cts.Token);
             });

            return pos;
        }

        private async Task SetCaliStall(int index, ushort pos, int stallIndex)
        {
            await Task.Run(() =>
            {
                var cid = _dutInfos[index].CID;
                var slaveIndex = _airOutletAutomotiveActuator._slaves.FindIndex(f => f.MasterLinId == cid && f.IsOnBus);
                if (slaveIndex != -1)
                {
                    if (stallIndex == 0)
                    {
                        _airOutletAutomotiveActuator._slaves[slaveIndex].CaliStallA = pos;
                    }
                    else
                    {
                        _airOutletAutomotiveActuator._slaves[slaveIndex].CaliStallB = pos;
                    }
                }
            }, _dutInfos[index]._cts.Token);
        }

        private async Task TaskClearStallOccurred(int index)
        {
            await Task.Run(async () =>
            {
                var cid = _dutInfos[index].CID;
                var slaveIndex = _airOutletAutomotiveActuator._slaves.FindIndex(f => f.MasterLinId == cid && f.IsOnBus);
                if (slaveIndex != -1)
                {
                    var slave = _airOutletAutomotiveActuator._slaves[slaveIndex];
                    if (slave.Stall_O > 0)
                    {
                        slave.ClearEvent(4);
                        await Task.Run(async () =>
                        {
                            while (true)
                            {
                                await Task.Delay(50);
                                if (slave.Stall_O == 0x00)
                                    break;

                                if (_dutInfos[index].State == DutState.Idle)
                                    break;

                            }
                        }, _dutInfos[index]._cts.Token);
                    }
                    slave.ClearEvent(0);
                }
            }, _dutInfos[index]._cts.Token);
        }

        private async Task TaskClearRes(int index)
        {
            await Task.Run(async () =>
            {
                var cid = _dutInfos[index].CID;
                var slaveIndex = _airOutletAutomotiveActuator._slaves.FindIndex(f => f.MasterLinId == cid && f.IsOnBus);
                if (slaveIndex != -1)
                {
                    var slave = _airOutletAutomotiveActuator._slaves[slaveIndex];
                    //if (slave.Res > 0 )
                    {
                        slave.ClearEvent(14);
                        await UpdateLbl(_lblCurrentState[index], string.Format("发送Clear信号，等待复位完成..."));
                        await Task.Run(async () =>
                        {
                            while (true)
                            {
                                await Task.Delay(50);
                                if (_dutInfos[index].BRes == 0x00 && !_dutInfos[slaveIndex].IsLinLoss)
                                    break;

                                if (_dutInfos[index].State == DutState.Idle)
                                    break;
                            }
                        }, _dutInfos[index]._cts.Token);
                    }
                    await UpdateLbl(_lblCurrentState[index], string.Format("Clear信号清零..."));
                    slave.ClearEvent(0);
                }
            }, _dutInfos[index]._cts.Token);
        }

        private async Task TaskMoveTargetPost(int index, ushort pos)
        {
            await Task.Run(async () =>
            {
                var cid = _dutInfos[index].CID;
                var slaveIndex = _airOutletAutomotiveActuator._slaves.FindIndex(f => f.MasterLinId == cid && f.IsOnBus);
                if (slaveIndex != -1)
                {
                    var slave = _airOutletAutomotiveActuator._slaves[slaveIndex];
                    slave.SetTargetPosition(pos);
                    await Task.Delay(200);
                    slave.SetMode(0);
                }
            });
        }

        private async Task UpdateLbl(UILabel lbl, string value)
        {
            await Task.Run(() =>
            {
                var updateAct = new Action(() =>
                {
                    if (lbl.Text != value)
                        lbl.Text = value;
                });

                if (InvokeRequired)
                    lbl.Invoke(updateAct);
                else
                    updateAct.Invoke();
            });
        }

        private void StartStop_Click(object sender, EventArgs e)
        {
            var btn = sender as UIButton;
            if (btn != null && btn.Tag != null)
            {
                var dutIndex = (int)btn.Tag;
                if (_dutInfos[dutIndex].State is DutState.Idle)
                {
                    //if (UIMessageBox.ShowAsk(string.Format("确定要开始'#{0}'的测试吗", dutIndex + 1)))
                    //{
                    //    ExecStart(dutIndex, btn);
                    //}
                    //else
                    //{
                    //    UIMessageTip.Show("操作取消");
                    //}

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
                    ExecStart(dutIndex, btn, type);
                }
                else
                {
                    if (UIMessageBox.ShowAsk(string.Format("确定要停止'#{0}'的测试吗", dutIndex + 1)))
                    {
                        ExecStop(dutIndex, btn);
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

        private void ExecStart(int dutIndex, UIButton btn, ClearType clearType = ClearType.NoClear)
        {
            if (clearType == ClearType.ClearPeriod)
            {
                _dutInfos[dutIndex].PeriodCount = 0;
                _dutInfos[dutIndex].CycleCount = 0;
                _dutInfos[dutIndex].CaliCount = 0;
            }
            else if (clearType == ClearType.ClearCycle)
            {
                _dutInfos[dutIndex].CycleCount = 0;
            }
            else
            {
                if (_dutInfos[dutIndex].CycleCount >= _bCycleMaxCount)
                {
                    _dutInfos[dutIndex].PeriodCount++;
                    if (_dutInfos[dutIndex].PeriodCount >= _bPeriodMaxCount)
                    {
                        _dutInfos[dutIndex].PeriodCount = 0;
                        _dutInfos[dutIndex].CycleCount = 0;
                        _dutInfos[dutIndex].CaliCount = 0;
                    }
                    else
                    {
                        _dutInfos[dutIndex].CycleCount = 0;
                    }
                }
            }

            _dutInfos[dutIndex].State = DutState.FindStall;
            if (InvokeRequired)
                btn.Invoke(new Action(() => { btn.Style = UIStyle.Green; }));
            else
                btn.Style = UIStyle.Green;
        }

        private void ExecStop(int dutIndex, UIButton btn)
        {
            _dutInfos[dutIndex].State = DutState.Idle;
            if (InvokeRequired)
                btn.Invoke(new Action(() => { btn.Style = UIStyle.Orange; }));
            else
                btn.Style = UIStyle.Orange;
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

            for (int dutIndex = 0; dutIndex < DutCount; dutIndex++)
            {
                if (_dutInfos[dutIndex].State is DutState.Idle)
                {
                    ExecStart(dutIndex, _btnStartStop[dutIndex], type);
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

            for (int dutIndex = 0; dutIndex < DutCount; dutIndex++)
            {
                if (_dutInfos[dutIndex].State != DutState.Idle)
                {
                    ExecStop(dutIndex, _btnStartStop[dutIndex]);
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
