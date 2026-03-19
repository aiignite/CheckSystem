using CommonUtility;
using Controller;
using Sunny.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckSystem.HelperForms.AirOutlet
{
    public partial class MyWndLoadBox : UIForm
    {
        private const int DutCount = 6;
        private AirOutletAutomotiveActuator _airOutletAutomotiveActuator = new AirOutletAutomotiveActuator("出风口执行器");
        private SyControllerWith56Pin _linController = new SyControllerWith56Pin("Lin控制器");
        private DutInfo[] _dutInfos = new DutInfo[DutCount];
        private ushort _offset = (6400 / 360 * 30);

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
        private NumericUpDown nudSpeed, nudInitPos;
        private NumericUpDown nudCycleCount, nudCycleDelay;
        private NumericUpDown nudPeriodCount;
        private UIButton btnParaReset, btnParaInvoke, btnParaSave;

        internal enum DutState
        {
            [Description("初始化")]
            Idle,

            [Description("等待开始")]
            WaitStart,

            [Description("寻找堵转点")]
            FindStall,

            [Description("循环动作")]
            RunCycle,
        };

        internal class DutInfo
        {
            public byte SID { get; set; }
            public byte CID { get; set; }
            public byte NAD { get; set; }
            public DutState State { get; set; }
            public AirOutletAutomotiveActuator.Slave Slave { get; set; }
            public CancellationTokenSource _cts = new CancellationTokenSource();
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
        }

        private void MyWndLoadBox_Load(object sender, EventArgs e)
        {
            InitTopPanel();
            InitBottomPanel();
            ResizeUI();
            ResizeTopPanel();
            ResizeBottomPanel();
            panelMainCycle.SizeChanged += (ss, ee) => ResizeTopPanel();
            panelCyclePara.SizeChanged += (ss, ee) => ResizeBottomPanel();
            Resize += (ss, ee) => ResizeUI();

            _linController.InitRemoteIpAddress("192.168.1.28:8088");
            _airOutletAutomotiveActuator.Lin = _linController.GatewayLin;

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

                _lblNad[d].Text = ValueHelper.GetHextStrWithOx(_dutInfos[d].NAD);
                _lblCid[d].Text = ValueHelper.GetHextStrWithOx(_dutInfos[d].CID);
                _lblSid[d].Text = ValueHelper.GetHextStrWithOx(_dutInfos[d].SID);
            }

            _airOutletAutomotiveActuator.StartLin();
            CreateWorkder();
        }

        private void SetDefaultValue(byte nad)
        {
            _airOutletAutomotiveActuator.Stop_Mode(nad);
            _airOutletAutomotiveActuator.Start_position_valid(nad);
            _airOutletAutomotiveActuator.Sta_Pos_65535_Signal(nad, 32000);
            _airOutletAutomotiveActuator.Stall_detection_on(nad);
            _airOutletAutomotiveActuator.Auto_Speed(nad);
        }

        private async void CreateWorkder()
        {
            for (var i = 0; i < DutCount; i++)
                await WorkThreadProc(i);
        }

        private void InitTopPanel()
        {
            string[] rowLabels = { "NAD:", "CID:", "SID:", "当前位置:", "目标位置:", "运动方向:", "速度状态:", "堵转状态:", "当前进程:", "循环计数:", "周期计数:", "自学习计数:", "计数清零:", "开始/停止:" };
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
                panelMainCycle.Controls.Add(_btnClear[col]);

                _btnStartStop[col] = CreateButton("#" + (col + 1), 0, 0, 60, 22, UIStyle.Orange);
                _btnStartStop[col].Tag = col;
                _btnStartStop[col].Click += StartStop_Click;
                panelMainCycle.Controls.Add(_btnStartStop[col]);
            }
        }

        private void InitBottomPanel()
        {
            string[] rowLabels = { "速度等级:", "初始化位置:", "循环次数:", "循环延时:", "周期次数:" };
            lblBottomRowHeaders = new UIMarkLabel[rowLabels.Length];

            // 创建行标题
            for (var i = 0; i < lblBottomRowHeaders.Length; i++)
            {
                lblBottomRowHeaders[i] = CreateLabel(rowLabels[i], 0, 0, 70, 22, isBold: true);
                panelCyclePara.Controls.Add(lblBottomRowHeaders[i]);
            }

            // 创建列标题和每列控件
            nudSpeed = CreateNumericUpDown(0, 0, 80, 0, 15, 4);
            panelCyclePara.Controls.Add(nudSpeed);

            nudInitPos = CreateNumericUpDown(0, 0, 80, 0, 65535, 32000);
            panelCyclePara.Controls.Add(nudInitPos);

            nudCycleCount = CreateNumericUpDown(0, 0, 80, 100, 10000, 900);
            panelCyclePara.Controls.Add(nudCycleCount);

            nudCycleDelay = CreateNumericUpDown(0, 0, 80, 1, 5000, 500);
            panelCyclePara.Controls.Add(nudCycleDelay);

            nudPeriodCount = CreateNumericUpDown(0, 0, 80, 1, 1000000, 600000);
            panelCyclePara.Controls.Add(nudPeriodCount);

            btnParaReset = CreateButton("一键重置", 0, 0, 70, 22, UIStyle.Blue);
            panelCyclePara.Controls.Add(btnParaReset);

            btnParaInvoke = CreateButton("一键生效", 0, 0, 70, 22, UIStyle.Blue);
            panelCyclePara.Controls.Add(btnParaInvoke);

            btnParaSave = CreateButton("一键保存", 0, 0, 70, 22, UIStyle.Blue);
            panelCyclePara.Controls.Add(btnParaSave);
        }

        private UIMarkLabel CreateLabel(string text, int x, int y, int width, int height, bool isBottomLine = false, bool isShowBorder = false, bool isBold = false)
        {
            return new UIMarkLabel
            {
                Text = text,
                Location = new Point(x, y),
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

        private void ResizeTopPanel()
        {
            var panelWidth = panelMainCycle.ClientSize.Width;
            var panelHeight = panelMainCycle.ClientSize.Height;

            var labelWidth = 180;
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

                _lblCid[col].Location = new Point(x, startY + (++row) * rowHeight);
                _lblCid[col].Size = new Size(w, rowHeight - 2);

                _lblSid[col].Location = new Point(x, startY + (++row) * rowHeight);
                _lblSid[col].Size = new Size(w, rowHeight - 2);

                _lblActualPos[col].Location = new Point(x, startY + (++row) * rowHeight);
                _lblActualPos[col].Size = new Size(w, rowHeight - 2);

                _lblTargetPos[col].Location = new Point(x, startY + (++row) * rowHeight);
                _lblTargetPos[col].Size = new Size(w, rowHeight - 2);

                _lblDirection[col].Location = new Point(x, startY + (++row) * rowHeight);
                _lblDirection[col].Size = new Size(w, rowHeight - 2);

                _lblSpeed[col].Location = new Point(x, startY + (++row) * rowHeight);
                _lblSpeed[col].Size = new Size(w, rowHeight - 2);

                _lblStallState[col].Location = new Point(x, startY + (++row) * rowHeight);
                _lblStallState[col].Size = new Size(w, rowHeight - 2);

                _lblCurrentState[col].Location = new Point(x, startY + (++row) * rowHeight);
                _lblCurrentState[col].Size = new Size(w, rowHeight - 2);

                _lblCycleCount[col].Location = new Point(x, startY + (++row) * rowHeight);
                _lblCycleCount[col].Size = new Size(w, rowHeight - 2);

                _lblPeriodCount[col].Location = new Point(x, startY + (++row) * rowHeight);
                _lblPeriodCount[col].Size = new Size(w, rowHeight - 2);

                _lblAutoCaliCount[col].Location = new Point(x, startY + (++row) * rowHeight);
                _lblAutoCaliCount[col].Size = new Size(w, rowHeight - 2);

                _btnClear[col].Location = new Point(x, startY + (++row) * rowHeight);
                _btnClear[col].Size = new Size(w, rowHeight - 2);

                _btnStartStop[col].Location = new Point(x, startY + (++row) * rowHeight);
                _btnStartStop[col].Size = new Size(w, rowHeight - 2);
            }
        }

        private void ResizeBottomPanel()
        {
            var panelWidth = panelCyclePara.ClientSize.Width;
            var panelHeight = panelCyclePara.ClientSize.Height;

            var labelWidth = 180;
            var availableWidth = panelWidth - labelWidth - 20;
            var colWidth = 100;
            var rowHeight = 35;
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

            btnParaReset.Location = new Point(5, startY + (++row) * rowHeight);
            btnParaReset.Size = new Size(w * 2, rowHeight - 2);

            btnParaInvoke.Location = new Point(5, startY + (++row) * rowHeight);
            btnParaInvoke.Size = new Size(w * 2, rowHeight - 2);

            btnParaSave.Location = new Point(5, startY + (++row) * rowHeight);
            btnParaSave.Size = new Size(w * 2, rowHeight - 2);
        }

        [DllImport("Kernel32.dll")] public static extern IntPtr GetCurrentThread();
        [DllImport("Kernel32.dll")] public static extern int SetThreadIdealProcessor(IntPtr hThread, uint dwIdealProcessor);
        private Thread m_WorkThread;

        private async Task WorkThreadProc(int dutIndex)
        {
            await Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    await Task.Delay(200);
                    var slave = _dutInfos[dutIndex].Slave;
                    if (slave != null)
                    {
                        await UpdateLbl(_lblActualPos[dutIndex], slave.ActualPos.ToString());
                        await UpdateLbl(_lblTargetPos[dutIndex], slave.TargetPos.ToString());
                        await UpdateLbl(_lblDirection[dutIndex], ValueHelper.GetHextStrWithOx(slave.Rot_Dir));
                        await UpdateLbl(_lblSpeed[dutIndex], ValueHelper.GetHextStrWithOx(slave.Spe_S));
                        await UpdateLbl(_lblStallState[dutIndex], ValueHelper.GetHextStrWithOx(slave.Stall_O));
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
                    for (var i = 0; i < 2; i++)
                    {
                        await UpdateLbl(_lblCurrentState[index], @"寻找堵转点" + (i + 1));
                        var pos = await TaskFindStall(index, (ushort)(i % 2 == 0 ? 0 + 1 : 65535 - 1));
                        await SetCaliStall(index, pos, i);
                        await TaskClearStallOccurred(index);
                    }
                    _dutInfos[index].State = DutState.RunCycle;

                    await TaskMoveTargetPost(index, (ushort)((_dutInfos[index].Slave.CaliStallA + _dutInfos[index].Slave.CaliStallB) / 2));
                    await Worker(index);
                    break;

                case DutState.RunCycle:
                    for (int i = 0; i < 3; i++)
                    {
                        await UpdateLbl(_lblCurrentState[index], @"运动到循环点" + (i + 1));
                        var middle = (ushort)((_dutInfos[index].Slave.CaliStallA + _dutInfos[index].Slave.CaliStallB) / 2);
                        var targetPos = (ushort)(i % 2 == 0 ? middle - _offset : middle + _offset);
                        await TaskMoveTargetPost(index, targetPos);
                        await Task.Delay(500);

                        var runResult = await Task.Run(async () =>
                        {
                            while (true)
                            {
                                await Task.Delay(50);

                                if (_dutInfos[index].Slave.Stall_O == 0x01)
                                {
                                    _dutInfos[index].Slave.SetMode(1);
                                    await Task.Delay(100);
                                    await TaskClearStallOccurred(index);
                                    _dutInfos[index].State = DutState.FindStall;
                                    return 1;
                                }
                                else
                                {
                                    if (Math.Abs(_dutInfos[index].Slave.ActualPos - targetPos) < 10)
                                    {
                                        return 0;
                                    }
                                }
                            }
                        });

                        if (runResult == 1)
                            break;

                        await Task.Delay(500);
                    }
                    await Worker(index);
                    break;

                default:
                    break;
            }
        }

        private async Task WaitStart(int index)
        {
            await Task.Delay(50);
            await UpdateLbl(_lblCurrentState[index], @"等待开始");
            await Worker(index);
        }

        private async Task<ushort> TaskFindStall(int index, ushort targetPos)
        {
            var pos = await Task.Run(async () =>
             {
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
                             await Task.Delay(250);
                             return slave.ActualPos;
                         }
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
                    slave.ClearEvent(4);
                    await Task.Run(async () =>
                    {
                        while (true)
                        {
                            await Task.Delay(50);
                            if (slave.Stall_O == 0x00)
                                break;
                        }
                    }, _dutInfos[index]._cts.Token);
                    slave.ClearEvent(0);
                    //await Task.Delay(500);
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

        private async void StartStop_Click(object sender, EventArgs e)
        {
            var btn = sender as UIButton;
            if (btn != null && btn.Tag != null)
            {
                var dutIndex = (int)btn.Tag;
                if (_dutInfos[dutIndex].State is DutState.Idle)
                {
                    if (UIMessageBox.ShowAsk(string.Format("确定要开始'#{0}'的测试吗", dutIndex)))
                    {
                        _dutInfos[dutIndex].State = DutState.FindStall;
                        btn.Style = UIStyle.Green;
                        await WorkTask(dutIndex);
                    }
                    else
                    {
                        UIMessageTip.Show("操作取消");
                    }
                }
                else
                {
                    if (UIMessageBox.ShowAsk(string.Format("确定要停止'#{0}'的测试吗", dutIndex)))
                    {
                        _dutInfos[dutIndex].State = DutState.Idle;
                        btn.Style = UIStyle.Orange;
                        SetDefaultValue(_dutInfos[dutIndex].Slave.BindingNad);
                        _dutInfos[dutIndex]._cts.Cancel();
                    }
                    else
                    {
                        UIMessageTip.Show("操作取消");
                    }
                }
            }
        }
    }
}
