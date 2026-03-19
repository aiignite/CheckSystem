using Controller;
using Sunny.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CheckSystem.HelperForms.AirOutlet
{
    public partial class LoadBox : UIForm
    {
        private const int ChannelCount = 8;
        private AirOutletAutomotiveActuator _dut = new AirOutletAutomotiveActuator(@"出风口执行器");
        private SyControllerWith56Pin _linCtrl = new SyControllerWith56Pin(@"lin控制器");

        // 上半部分控件 - 通道参数表格
        private Label[] lblNad = new Label[ChannelCount];
        private Label[] lblCid = new Label[ChannelCount];
        private Label[] lblSid = new Label[ChannelCount];
        private TextBox[] txtActualPos = new TextBox[ChannelCount];
        private TextBox[] txtTargetPos = new TextBox[ChannelCount];
        private TextBox[] txtDirection = new TextBox[ChannelCount];
        private TextBox[] txtRunState = new TextBox[ChannelCount];
        private TextBox[] txtStallState = new TextBox[ChannelCount];
        private Label[] lblCurrentState = new Label[ChannelCount];
        private TextBox[] txtCycleCount = new TextBox[ChannelCount];
        private TextBox[] txtCompensation = new TextBox[ChannelCount];
        private Button[] btnClear = new Button[ChannelCount];
        private Button[] btnStartStop = new Button[ChannelCount];
        private Label[] lblColHeaders = new Label[ChannelCount];
        private Label[] lblRowHeaders = new Label[13];

        // 下半部分控件 - 循环模式参数
        private UIIntegerUpDown nudSpeed, nudInitPos, nudEndPos1, nudEndPos2;
        private UIIntegerUpDown nudCycleCount, nudCycleDelay, nudCycleInterval;
        private UIIntegerUpDown nudCompensation, nudParticipateCount;
        private UIIntegerUpDown nudAutoLearnTrigger, nudAutoLearnCount;
        private UIButton btnReset, btnCompensate;
        private UILabel lblStatus;
        private Label[] lblParamLabels = new Label[11];
        private Label lblMs1, lblMs2;

        // 报文显示
        private Label lblMsgSendTitle, lblMsgRecvTitle, lblTip;
        private Label[] lblMsgNum = new Label[15];
        private TextBox[] txtMsgSend = new TextBox[15];
        private TextBox[] txtMsgRecv = new TextBox[15];
        private Label lblAutoLearnStatus;
        private TextBox txtAutoLearnCurrent, txtAutoLearnTotal;

        public LoadBox()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            Load += LoadBox_Load;
            FormClosing += LoadBox_FormClosing;
        }

        private void LoadBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!UIMessageBox.ShowAsk("是否确认退出？"))
            {
                e.Cancel = true;
                return;
            }
            timerRefresh.Stop();
            _dut?.Dispose();
            _linCtrl?.Dispose();
        }

        private void LoadBox_Load(object sender, EventArgs e)
        {
            InitTopPanel();
            InitBottomPanel();
            Resize += LoadBox_Resize;

            try
            {
                _linCtrl.InitRemoteIpAddress("192.168.1.28:8088");
                _dut.Lin = _linCtrl.GatewayLin;
                _dut.AddSlave("0x02", "0x21");
                _dut.StartLin();
                timerRefresh.Tick += TimerRefresh_Tick;
                timerRefresh.Start();
            }
            catch (Exception ex)
            {
                UIMessageBox.ShowError("初始化失败：" + ex.Message);
            }
        }

        private void LoadBox_Resize(object sender, EventArgs e)
        {
            if (WindowState != FormWindowState.Minimized)
            {
                LayoutTopPanel();
                LayoutBottomPanel();
            }
        }

        private void TimerRefresh_Tick(object sender, EventArgs e)
        {
            // 刷新显示数据
        }

        #region 上半部分 - 通道参数表格

        private void InitTopPanel()
        {
            string[] rowLabels = { "NAD:", "CID:", "SID:", "当前位置:", "目标位置:", "运动方向:",
                "运动状态:", "堵转状态:", "当前进程:", "循环计数:", "补偿参数:", "计数清零:", "开始/停止:" };

            // 创建行标题
            for (var i = 0; i < rowLabels.Length; i++)
            {
                lblRowHeaders[i] = CreateLabel(rowLabels[i], 0, 0, 70, 22);
                lblRowHeaders[i].ForeColor = Color.Yellow;
                panelTop.Controls.Add(lblRowHeaders[i]);
            }

            // 创建列标题和每列控件
            for (var col = 0; col < ChannelCount; col++)
            {
                lblColHeaders[col] = CreateLabel((col + 1).ToString("00"), 0, 0, 60, 22);
                lblColHeaders[col].ForeColor = Color.White;
                lblColHeaders[col].TextAlign = ContentAlignment.MiddleCenter;
                panelTop.Controls.Add(lblColHeaders[col]);

                lblNad[col] = CreateLabel("01", 0, 0, 60, 20);
                lblNad[col].BackColor = Color.FromArgb(0, 100, 100);
                lblNad[col].TextAlign = ContentAlignment.MiddleCenter;
                panelTop.Controls.Add(lblNad[col]);

                lblCid[col] = CreateLabel("03", 0, 0, 60, 20);
                lblCid[col].BackColor = Color.FromArgb(0, 100, 100);
                lblCid[col].TextAlign = ContentAlignment.MiddleCenter;
                panelTop.Controls.Add(lblCid[col]);

                lblSid[col] = CreateLabel((col + 2).ToString("00"), 0, 0, 60, 20);
                lblSid[col].BackColor = Color.FromArgb(0, 100, 100);
                lblSid[col].TextAlign = ContentAlignment.MiddleCenter;
                panelTop.Controls.Add(lblSid[col]);

                txtActualPos[col] = CreateTextBox(0, 0, 60, 40, true);
                txtActualPos[col].Text = "11256";
                panelTop.Controls.Add(txtActualPos[col]);

                txtTargetPos[col] = CreateTextBox(0, 0, 60, 20, true);
                txtTargetPos[col].Text = "11256";
                panelTop.Controls.Add(txtTargetPos[col]);

                txtDirection[col] = CreateTextBox(0, 0, 60, 20, true);
                txtDirection[col].Text = "CW(1)";
                panelTop.Controls.Add(txtDirection[col]);

                txtRunState[col] = CreateTextBox(0, 0, 60, 20);
                txtRunState[col].Text = "0";
                panelTop.Controls.Add(txtRunState[col]);

                txtStallState[col] = CreateTextBox(0, 0, 60, 20);
                txtStallState[col].Text = "0";
                panelTop.Controls.Add(txtStallState[col]);

                lblCurrentState[col] = CreateLabel("等待开始", 0, 0, 60, 20);
                lblCurrentState[col].BackColor = Color.FromArgb(0, 100, 100);
                lblCurrentState[col].ForeColor = Color.Lime;
                lblCurrentState[col].TextAlign = ContentAlignment.MiddleCenter;
                panelTop.Controls.Add(lblCurrentState[col]);

                txtCycleCount[col] = CreateTextBox(0, 0, 60, 20);
                txtCycleCount[col].Text = "390002";
                panelTop.Controls.Add(txtCycleCount[col]);

                txtCompensation[col] = CreateTextBox(0, 0, 60, 20);
                txtCompensation[col].Text = "350";
                panelTop.Controls.Add(txtCompensation[col]);

                btnClear[col] = CreateButton("Clear", 0, 0, 60, 22);
                btnClear[col].Tag = col;
                btnClear[col].Click += BtnClear_Click;
                panelTop.Controls.Add(btnClear[col]);

                btnStartStop[col] = CreateButton("#" + (col + 1), 0, 0, 60, 22);
                btnStartStop[col].BackColor = Color.Green;
                btnStartStop[col].ForeColor = Color.White;
                btnStartStop[col].Tag = col;
                btnStartStop[col].Click += BtnStartStop_Click;
                panelTop.Controls.Add(btnStartStop[col]);
            }

            LayoutTopPanel();
        }

        private void LayoutTopPanel()
        {
            int panelWidth = panelTop.ClientSize.Width;
            int panelHeight = panelTop.ClientSize.Height;

            int labelWidth = 75;
            int availableWidth = panelWidth - labelWidth - 20;
            int colWidth = availableWidth / ChannelCount;
            int rowHeight = Math.Max(20, (panelHeight - 10) / 14);
            int startX = labelWidth + 5;
            int startY = 5;

            // 布局行标题
            for (int i = 0; i < lblRowHeaders.Length; i++)
            {
                int y = startY + (i + 1) * rowHeight;
                //if (i >= 3) y += rowHeight; // 当前位置占2行
                lblRowHeaders[i].Location = new Point(5, y);
                lblRowHeaders[i].Size = new Size(labelWidth - 5, rowHeight - 2);
            }

            // 布局每列控件
            for (int col = 0; col < ChannelCount; col++)
            {
                int x = startX + col * colWidth;
                int w = colWidth - 4;
                int row = 0;

                lblColHeaders[col].Location = new Point(x, startY);
                lblColHeaders[col].Size = new Size(w, rowHeight - 2);

                lblNad[col].Location = new Point(x, startY + (++row) * rowHeight);
                lblNad[col].Size = new Size(w, rowHeight - 2);

                lblCid[col].Location = new Point(x, startY + (++row) * rowHeight);
                lblCid[col].Size = new Size(w, rowHeight - 2);

                lblSid[col].Location = new Point(x, startY + (++row) * rowHeight);
                lblSid[col].Size = new Size(w, rowHeight - 2);

                txtActualPos[col].Location = new Point(x, startY + (++row) * rowHeight);
                txtActualPos[col].Size = new Size(w, rowHeight * 2 - 2);
                row++;

                txtTargetPos[col].Location = new Point(x, startY + (++row) * rowHeight);
                txtTargetPos[col].Size = new Size(w, rowHeight - 2);

                txtDirection[col].Location = new Point(x, startY + (++row) * rowHeight);
                txtDirection[col].Size = new Size(w, rowHeight - 2);

                txtRunState[col].Location = new Point(x, startY + (++row) * rowHeight);
                txtRunState[col].Size = new Size(w, rowHeight - 2);

                txtStallState[col].Location = new Point(x, startY + (++row) * rowHeight);
                txtStallState[col].Size = new Size(w, rowHeight - 2);

                lblCurrentState[col].Location = new Point(x, startY + (++row) * rowHeight);
                lblCurrentState[col].Size = new Size(w, rowHeight - 2);

                txtCycleCount[col].Location = new Point(x, startY + (++row) * rowHeight);
                txtCycleCount[col].Size = new Size(w, rowHeight - 2);

                txtCompensation[col].Location = new Point(x, startY + (++row) * rowHeight);
                txtCompensation[col].Size = new Size(w, rowHeight - 2);

                btnClear[col].Location = new Point(x, startY + (++row) * rowHeight);
                btnClear[col].Size = new Size(w, rowHeight - 2);

                btnStartStop[col].Location = new Point(x, startY + (++row) * rowHeight);
                btnStartStop[col].Size = new Size(w, rowHeight - 2);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            int channel = (int)btn.Tag;
            txtCycleCount[channel].Text = "0";
        }

        private void BtnStartStop_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            int channel = (int)btn.Tag;
            btn.BackColor = btn.BackColor == Color.Green ? Color.Red : Color.Green;
            lblCurrentState[channel].Text = btn.BackColor == Color.Green ? "等待开始" : "运行中";
        }

        #endregion

        #region 下半部分 - 循环模式面板

        private void InitBottomPanel()
        {
            string[] paramLabels = { "速度等级:", "初始化位置:", "端点极值1:", "端点极值2:",
                "循环次数:", "循环延时:", "循环延时:", "补偿参数:", "参与数量:", "触发自学习:", "自学习次数:" };
            int[] defaultValues = { 4, 10000, 0, 60000, 390000, 0, 20, 350, 2, 900, 1 };

            // 创建参数标签
            for (int i = 0; i < paramLabels.Length; i++)
            {
                lblParamLabels[i] = CreateLabel(paramLabels[i], 0, 0, 80, 26);
                lblParamLabels[i].ForeColor = Color.Yellow;
                tabPage1.Controls.Add(lblParamLabels[i]);
            }

            // 创建数值输入控件
            nudSpeed = CreateNumericUpDown(0, 0, 80, 0, 10, defaultValues[0]);
            tabPage1.Controls.Add(nudSpeed);
            nudInitPos = CreateNumericUpDown(0, 0, 80, 0, 65535, defaultValues[1]);
            tabPage1.Controls.Add(nudInitPos);
            nudEndPos1 = CreateNumericUpDown(0, 0, 80, 0, 65535, defaultValues[2]);
            tabPage1.Controls.Add(nudEndPos1);
            nudEndPos2 = CreateNumericUpDown(0, 0, 80, 0, 65535, defaultValues[3]);
            tabPage1.Controls.Add(nudEndPos2);
            nudCycleCount = CreateNumericUpDown(0, 0, 80, 0, 999999, defaultValues[4]);
            tabPage1.Controls.Add(nudCycleCount);
            nudCycleDelay = CreateNumericUpDown(0, 0, 80, 0, 9999, defaultValues[5]);
            tabPage1.Controls.Add(nudCycleDelay);
            nudCycleInterval = CreateNumericUpDown(0, 0, 80, 0, 9999, defaultValues[6]);
            tabPage1.Controls.Add(nudCycleInterval);
            nudCompensation = CreateNumericUpDown(0, 0, 80, 0, 9999, defaultValues[7]);
            tabPage1.Controls.Add(nudCompensation);
            nudParticipateCount = CreateNumericUpDown(0, 0, 80, 0, 10, defaultValues[8]);
            tabPage1.Controls.Add(nudParticipateCount);
            nudAutoLearnTrigger = CreateNumericUpDown(0, 0, 80, 0, 9999, defaultValues[9]);
            tabPage1.Controls.Add(nudAutoLearnTrigger);
            nudAutoLearnCount = CreateNumericUpDown(0, 0, 80, 0, 99, defaultValues[10]);
            tabPage1.Controls.Add(nudAutoLearnCount);

            lblMs1 = CreateLabel("毫秒", 0, 0, 35, 26);
            lblMs1.ForeColor = Color.Yellow;
            tabPage1.Controls.Add(lblMs1);
            lblMs2 = CreateLabel("秒", 0, 0, 25, 26);
            lblMs2.ForeColor = Color.Yellow;
            tabPage1.Controls.Add(lblMs2);

            // 按钮
            btnReset = new UIButton { Text = "一键复位", Size = new Size(90, 32) };
            btnReset.Click += BtnReset_Click;
            tabPage1.Controls.Add(btnReset);

            btnCompensate = new UIButton { Text = "一键补偿", Size = new Size(90, 32) };
            btnCompensate.Click += BtnCompensate_Click;
            tabPage1.Controls.Add(btnCompensate);

            lblStatus = new UILabel
            {
                Text = "停止循环",
                ForeColor = Color.Cyan,
                Font = new Font("宋体", 14, FontStyle.Bold),
                Size = new Size(200, 30)
            };
            tabPage1.Controls.Add(lblStatus);

            // 提示标签
            lblTip = CreateLabel("如遇到无法启动或无法停机的", 0, 0, 280, 26);
            lblTip.ForeColor = Color.Red;
            tabPage1.Controls.Add(lblTip);

            // 报文标题
            lblMsgSendTitle = CreateLabel("报文发送:", 0, 0, 70, 20);
            lblMsgSendTitle.ForeColor = Color.White;
            tabPage1.Controls.Add(lblMsgSendTitle);
            lblMsgRecvTitle = CreateLabel("报文接收:", 0, 0, 70, 20);
            lblMsgRecvTitle.ForeColor = Color.White;
            tabPage1.Controls.Add(lblMsgRecvTitle);

            // 报文行
            for (int i = 0; i < 15; i++)
            {
                lblMsgNum[i] = CreateLabel("#" + (i + 1).ToString("00") + ":", 0, 0, 40, 20);
                lblMsgNum[i].ForeColor = Color.Yellow;
                tabPage1.Controls.Add(lblMsgNum[i]);

                txtMsgSend[i] = CreateTextBox(0, 0, 180, 20);
                txtMsgSend[i].Text = i < 8 ? $"0{i + 1} E4 40 FA 2B 10 27 00" : "";
                tabPage1.Controls.Add(txtMsgSend[i]);

                txtMsgRecv[i] = CreateTextBox(0, 0, 180, 20);
                txtMsgRecv[i].Text = i < 8 ? $"02 04 00 FA 2B 01 01 00" : "";
                tabPage1.Controls.Add(txtMsgRecv[i]);
            }

            // 自学习计次
            lblAutoLearnStatus = CreateLabel("自学习计次:", 0, 0, 80, 20);
            lblAutoLearnStatus.ForeColor = Color.White;
            tabPage1.Controls.Add(lblAutoLearnStatus);
            txtAutoLearnCurrent = CreateTextBox(0, 0, 50, 20);
            txtAutoLearnCurrent.Text = "0";
            tabPage1.Controls.Add(txtAutoLearnCurrent);
            txtAutoLearnTotal = CreateTextBox(0, 0, 50, 20);
            txtAutoLearnTotal.Text = "235";
            txtAutoLearnTotal.BackColor = Color.Red;
            tabPage1.Controls.Add(txtAutoLearnTotal);

            LayoutBottomPanel();
        }

        private void LayoutBottomPanel()
        {
            int tabWidth = tabPage1.ClientSize.Width;
            int tabHeight = tabPage1.ClientSize.Height;

            int leftX = 10, rowHeight = 30;
            int labelWidth = 85, inputWidth = 85;
            int rightX = Math.Max(280, tabWidth / 3);
            int msgWidth = Math.Max(150, (tabWidth - rightX - 150) / 2);

            // 左侧参数布局
            for (int i = 0; i < lblParamLabels.Length; i++)
            {
                lblParamLabels[i].Location = new Point(leftX, 10 + i * rowHeight);
                lblParamLabels[i].Size = new Size(labelWidth, rowHeight - 4);
            }

            int inputX = leftX + labelWidth;
            nudSpeed.Location = new Point(inputX, 10 + 0 * rowHeight);
            nudInitPos.Location = new Point(inputX, 10 + 1 * rowHeight);
            nudEndPos1.Location = new Point(inputX, 10 + 2 * rowHeight);
            nudEndPos2.Location = new Point(inputX, 10 + 3 * rowHeight);
            nudCycleCount.Location = new Point(inputX, 10 + 4 * rowHeight);
            nudCycleDelay.Location = new Point(inputX, 10 + 5 * rowHeight);
            nudCycleInterval.Location = new Point(inputX, 10 + 6 * rowHeight);
            nudCompensation.Location = new Point(inputX, 10 + 7 * rowHeight);
            nudParticipateCount.Location = new Point(inputX, 10 + 8 * rowHeight);
            nudAutoLearnTrigger.Location = new Point(inputX, 10 + 9 * rowHeight);
            nudAutoLearnCount.Location = new Point(inputX, 10 + 10 * rowHeight);

            lblMs1.Location = new Point(inputX + inputWidth + 5, 10 + 5 * rowHeight);
            lblMs2.Location = new Point(inputX + inputWidth + 5, 10 + 6 * rowHeight);

            btnReset.Location = new Point(leftX, 10 + 11 * rowHeight + 10);
            btnCompensate.Location = new Point(leftX + 100, 10 + 11 * rowHeight + 10);
            lblStatus.Location = new Point(leftX, 10 + 12 * rowHeight + 20);

            // 右侧报文布局
            lblTip.Location = new Point(rightX, 5);
            lblMsgSendTitle.Location = new Point(rightX + 45, 28);
            lblMsgRecvTitle.Location = new Point(rightX + 50 + msgWidth + 30, 28);

            int msgRowHeight = Math.Max(18, (tabHeight - 80) / 16);
            for (int i = 0; i < 15; i++)
            {
                int y = 50 + i * msgRowHeight;
                lblMsgNum[i].Location = new Point(rightX, y);
                txtMsgSend[i].Location = new Point(rightX + 45, y);
                txtMsgSend[i].Size = new Size(msgWidth, msgRowHeight - 2);
                txtMsgRecv[i].Location = new Point(rightX + 50 + msgWidth + 30, y);
                txtMsgRecv[i].Size = new Size(msgWidth, msgRowHeight - 2);
            }

            int bottomY = 50 + 15 * msgRowHeight + 5;
            lblAutoLearnStatus.Location = new Point(rightX + 50 + msgWidth + 30, bottomY);
            txtAutoLearnCurrent.Location = new Point(rightX + 50 + msgWidth + 115, bottomY);
            txtAutoLearnTotal.Location = new Point(rightX + 50 + msgWidth + 175, bottomY);
        }

        private void BtnReset_Click(object sender, EventArgs e) => lblStatus.Text = "正在复位...";
        private void BtnCompensate_Click(object sender, EventArgs e) => lblStatus.Text = "正在补偿...";

        #endregion

        #region 辅助方法

        private Label CreateLabel(string text, int x, int y, int width, int height)
        {
            return new Label
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(width, height),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Font = new Font("宋体", 9)
            };
        }

        private TextBox CreateTextBox(int x, int y, int width, int height, bool readOnly = false)
        {
            return new TextBox
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                BackColor = Color.FromArgb(0, 100, 100),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("宋体", 9),
                TextAlign = HorizontalAlignment.Center,
                ReadOnly = readOnly
            };
        }

        private Button CreateButton(string text, int x, int y, int width, int height)
        {
            return new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(width, height),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(200, 200, 200),
                ForeColor = Color.Black,
                Font = new Font("宋体", 9)
            };
        }

        private UIIntegerUpDown CreateNumericUpDown(int x, int y, int width, int min, int max, int value)
        {
            return new UIIntegerUpDown
            {
                Location = new Point(x, y),
                Size = new Size(width, 26),
                Minimum = min,
                Maximum = max,
                Value = value
            };
        }

        #endregion
    }
}
