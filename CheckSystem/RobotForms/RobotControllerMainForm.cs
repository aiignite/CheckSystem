using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonUtility;
using CommonUtility.FileOperator;
using Controller;
using HZH_Controls.Controls.Btn;
using HZH_Controls.IconFont;

namespace CheckSystem.RobotForms
{
    public partial class RobotControllerMainForm : Form
    {
        private readonly List<string> _lstDelayStrs = new List<string> { "WaitTime" };
        private readonly List<string> _lstSetStrs = new List<string>();
        private readonly List<string> _lstWaitStrs = new List<string>();
        private readonly List<string> _lstStep = new List<string>();
        private readonly List<string> _lstSpeed = new List<string>();

        private Thread MainTh { get; set; }
        private readonly RobotControllerPpMode _robotController;
        private RobotFlashConfig RobotFlashConfig { get; set; }
        private readonly string _confilePath =
            Program.SysDir + @"\ControllerConfig\RobotFlashAddrConfig.xml";

        private Color _jogBlockSelectDefaultColor = Color.Aquamarine;

        public RobotControllerMainForm(RobotControllerPpMode robotController)
        {
            InitializeComponent();
            Icon = FontImages.GetIcon(FontIcons.A_fa_child, 32,
                Color.DodgerBlue);

            toolStripButtonDeleteRow.Image = FontImages.GetImage(FontIcons.A_fa_close, 32,
                Color.DodgerBlue);
            toolStripButtonUpdate.Image = FontImages.GetImage(FontIcons.E_icon_cloud_upload, 32,
                Color.DodgerBlue);

            toolStripButtonSkip.Image = FontImages.GetImage(FontIcons.E_arrow_triangle_down, 32,
                Color.DodgerBlue);
            toolStripButtonNextRow.Image = FontImages.GetImage(FontIcons.A_fa_angle_double_down, 32,
                Color.DodgerBlue);
            toolStripButtonPreRow.Image = FontImages.GetImage(FontIcons.A_fa_angle_double_up, 32,
                Color.DodgerBlue);
            toolStripButtonMoveSelectedBlockLine.Image = FontImages.GetImage(FontIcons.E_arrow_right_down, 32,
                Color.DodgerBlue);
            toolStripButtonRunProgram.Image = FontImages.GetImage(FontIcons.A_fa_play, 32,
                Color.DodgerBlue);
            toolStripButtonPauseProgram.Image = FontImages.GetImage(FontIcons.A_fa_hand_stop_o, 32,
                Color.DodgerBlue);
            lblSpeedSelect.Image = FontImages.GetImage(FontIcons.A_fa_lightbulb_o, 32,
                Color.DodgerBlue);

            toolStripButtonSaveProgram.Image = FontImages.GetImage(FontIcons.E_icon_floppy_alt, 32,
                Color.DodgerBlue);

            _robotController = robotController;
            WindowState = FormWindowState.Maximized;
            RobotFlashConfig = XmlHelper.Deserialize<RobotFlashConfig>(_confilePath);
            Closed += RobotControllerMainForm_Closed;

            InitMain();
            InitEvent();
            InitTeach();

            if (MainTh != null)
            {
                MainTh.Abort();
                MainTh.Join();
            }

            MainTh = new Thread(MainWork) { IsBackground = true };
            MainTh.Start();
        }

        private void RobotControllerMainForm_Closed(object sender, EventArgs e)
        {
            if (_robotController != null)
                _robotController.Dispose();
        }

        private void InitMain()
        {
            readDataGrid.label.Text = @"读取信息";
            readDataGrid.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "名称" });
            readDataGrid.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "数值" });

            readDataGrid.dataGridView.ForeColor = Color.AntiqueWhite;
            readDataGrid.dataGridView.AllowUserToAddRows = false;
            readDataGrid.dataGridView.AllowUserToDeleteRows = false;
            readDataGrid.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            readDataGrid.dataGridView.AllowUserToResizeColumns = true;
            readDataGrid.dataGridView.AllowUserToResizeRows = false;
            readDataGrid.dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            readDataGrid.dataGridView.BackgroundColor = SystemColors.ActiveCaption;
            readDataGrid.dataGridView.ReadOnly = true;
            readDataGrid.dataGridView.RowHeadersVisible = false;
            for (var i = 0; i < readDataGrid.dataGridView.Columns.Count; i++)
                readDataGrid.dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

            var fi = _robotController.GetType().GetFields();

            lock (RobotControllerPpMode.LockState)
            {
                for (var i = 0; i < 16; i++)
                {
                    var random = new Random();
                    var b = 16 * (i + 1);
                    var randomColor = Color.FromArgb(random.Next(0, b), random.Next(0, b), random.Next(0, b));

                    var str = "------" + i + "号轴------";

                    foreach (var t in _robotController.SingleAxisState[i].Keys)
                    {
                        var rowAdd = readDataGrid.dataGridView.Rows.Add();
                        var row = readDataGrid.dataGridView.Rows[rowAdd];
                        row.Cells[0].Value = str + "-" + t;
                        row.Cells[1].Value = _robotController.SingleAxisState[i][t];
                        row.DefaultCellStyle.BackColor = randomColor;
                    }
                }
            }

            foreach (var fieldInfo in fi)
            {
                if (Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)) == null)
                    continue;

                var des =
                    ((DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)))
                        .Description;

                if (des.StartsWith("R,RobotSate-") ||
                    des.StartsWith("R,输入_DI") ||
                    des.StartsWith("R/W,继电器_DO") ||
                    des.StartsWith("R/W,低边输出_DO") ||
                    des.StartsWith("R/W,通用输入SI") ||
                    des.StartsWith("R/W,通用输出SO0"))
                {
                    var rowAdd = readDataGrid.dataGridView.Rows.Add();
                    var row = readDataGrid.dataGridView.Rows[rowAdd];
                    row.DefaultCellStyle.BackColor = Color.DarkGoldenrod;

                    row.Cells[0].Value = fieldInfo.Name;
                    row.Cells[1].Value = fieldInfo.GetValue(_robotController) == null
                        ? string.Empty
                        : fieldInfo.GetValue(_robotController).ToString();
                }
            }

            writeTypeCmb.comboBox.SelectedIndexChanged += writeTypeComboBox_SelectedIndexChanged;
            writeAxisNo.comboBox.SelectedIndexChanged += writeAxisNoComboBox_SelectedIndexChanged1;

            writeTypeCmb.comboBox.Items.Add("单轴控制");
            writeTypeCmb.comboBox.Items.Add("写Flash");
            //writeTypeCmb.comboBox.Items.Add("写单轴事件");
            writeTypeCmb.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            writeTypeCmb.comboBox.SelectedIndex = 0;

            for (var i = 0; i < 16; i++)
                writeAxisNo.comboBox.Items.Add(string.Format("{0}号轴", i));
            writeAxisNo.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            writeAxisNo.comboBox.SelectedIndex = 0;

            lblAxisSelect.label.Text = @"选择轴：";
            lblAxisSelect.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            for (var i = 0; i < 16; i++)
                lblAxisSelect.comboBox.Items.Add(string.Format("轴{0}", i));
            lblAxisSelect.comboBox.Items.Add("All");
            lblAxisSelect.comboBox.SelectedIndex = 16;
            lblAxisSelect.comboBox.SelectedIndexChanged += comboBox_SelectedIndexChanged;
        }

        private void InitEvent()
        {
            eventDataGrid.label.Text = @"写单轴事件";
            eventDataGrid.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "轴号" });
            eventDataGrid.dataGridView.Columns.Add(new DataGridViewButtonColumn { Name = "空事件" });
            eventDataGrid.dataGridView.Columns.Add(new DataGridViewButtonColumn { Name = "使能" });
            eventDataGrid.dataGridView.Columns.Add(new DataGridViewButtonColumn { Name = "失能" });
            eventDataGrid.dataGridView.Columns.Add(new DataGridViewButtonColumn { Name = "软急停" });
            eventDataGrid.dataGridView.Columns.Add(new DataGridViewButtonColumn { Name = "重置" });
            eventDataGrid.dataGridView.Columns.Add(new DataGridViewButtonColumn { Name = "回原点/轴复位" });
            eventDataGrid.dataGridView.Columns.Add(new DataGridViewButtonColumn { Name = "设定原点偏移" });
            eventDataGrid.dataGridView.Columns.Add(new DataGridViewButtonColumn { Name = "停止" });
            eventDataGrid.dataGridView.Columns.Add(new DataGridViewButtonColumn { Name = "暂停" });
            eventDataGrid.dataGridView.Columns.Add(new DataGridViewButtonColumn { Name = "继续" });

            eventDataGrid.dataGridView.AllowUserToAddRows = false;
            eventDataGrid.dataGridView.AllowUserToDeleteRows = false;
            //eventDataGrid.dataGridView.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8, FontStyle.Regular);
            eventDataGrid.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            eventDataGrid.dataGridView.AllowUserToResizeColumns = true;
            eventDataGrid.dataGridView.AllowUserToResizeRows = false;
            eventDataGrid.dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            eventDataGrid.dataGridView.BackgroundColor = SystemColors.ActiveCaption;
            eventDataGrid.dataGridView.ReadOnly = true;
            eventDataGrid.dataGridView.RowHeadersVisible = false;
            for (var i = 0; i < eventDataGrid.dataGridView.Columns.Count; i++)
                eventDataGrid.dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            eventDataGrid.dataGridView.CellContentClick += eventDataGridView_CellContentClick;

            for (var i = 0; i < 16; i++)
            {
                var thisRowColor1 = Color.AntiqueWhite;
                var thisRowColor2 = Color.Azure;
                RowsAdd(
                    eventDataGrid.dataGridView,
                    new[] { string.Format(@"{0}号轴", i), @"空事件", @"使能", @"失能", @"软急停", @"重置", @"回原点/轴复位", @"设定原点偏移", @"停止", @"暂停", @"继续" }.ToList(),
                    i % 2 == 0 ? thisRowColor1 : thisRowColor2);
            }
            for (var i = 0; i < eventDataGrid.dataGridView.Rows.Count; i++)
            {
                var row = eventDataGrid.dataGridView.Rows[i];
                row.DefaultCellStyle.Font = new Font(new FontFamily("宋体"), 16, FontStyle.Regular);
                row.Height = 50;
            }

            //lblCmbWriteAllEvent.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            //lblCmbWriteAllEvent.comboBox.Items.AddRange(new object[] { @"空事件", @"使能", @"失能", @"软急停", @"重置", @"回原点/轴复位", @"停止", @"暂停", @"继续" });
            //lblCmbWriteAllEvent.comboBox.SelectedIndex = 0;
        }

        private void InitTeach()
        {
            for (var i = 0; i < _robotController.RobotJogging.RobotPrograms.Length; i++)
            {
                var rowAdd = dataGridViewProgramList.Rows[dataGridViewProgramList.Rows.Add()];
                rowAdd.Cells[0].Value = (i + 1).ToString();
                rowAdd.Cells[1].Value = _robotController.RobotJogging.RobotPrograms[i].Name;
                rowAdd.Cells[2].Value = _robotController.RobotJogging.RobotPrograms[i].Note;
            }
            dataGridViewProgramList.CellClick += dataGridViewProgramList_CellContentClick;

            for (var i = 0; i < 20; i++)
                toolStripComboBoxSpeedPercent.Items.Add(string.Format(@"{0}%", (i + 1) * 5));
            toolStripComboBoxSpeedPercent.DropDownStyle = ComboBoxStyle.DropDownList;
            toolStripComboBoxSpeedPercent.SelectedIndex = 0;

            for (var i = 0; i < 8; i++)
                _lstSetStrs.Add(string.Format("Do{0}", i));
            for (var i = 0; i < 41; i++)
                _lstSetStrs.Add(string.Format("So{0}", i));

            for (var i = 0; i < 12; i++)
                _lstWaitStrs.Add(string.Format("Di{0}", i));
            for (var i = 0; i < 41; i++)
                _lstWaitStrs.Add(string.Format("Si{0}", i));

            btnInsert.FillColor = btnUpdate.FillColor = Color.Gray;
            btnInsert.Enabled = btnUpdate.Enabled = false;

            eventTeachDataGrid.label.Text = @"添加事件";
            eventTeachDataGrid.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "事件名称" });
            eventTeachDataGrid.dataGridView.Columns.Add(new DataGridViewComboBoxColumn { Name = "信号名称" });
            eventTeachDataGrid.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "信号值" });
            eventTeachDataGrid.dataGridView.Columns.Add(new DataGridViewButtonColumn { Name = "添加到末尾" });
            eventTeachDataGrid.dataGridView.Columns.Add(new DataGridViewButtonColumn { Name = "插入到上一行" });
            eventTeachDataGrid.dataGridView.Columns.Add(new DataGridViewButtonColumn { Name = "更新当前行" });
            eventTeachDataGrid.dataGridView.ReadOnly = false;
            eventTeachDataGrid.dataGridView.RowHeadersVisible = false;
            eventTeachDataGrid.dataGridView.AllowUserToAddRows = false;
            eventTeachDataGrid.dataGridView.AllowUserToDeleteRows = false;
            eventTeachDataGrid.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            eventTeachDataGrid.dataGridView.EditMode = DataGridViewEditMode.EditOnEnter;
            eventTeachDataGrid.dataGridView.AllowUserToResizeColumns = true;
            eventTeachDataGrid.dataGridView.AllowUserToResizeRows = false;
            eventTeachDataGrid.dataGridView.BackgroundColor = SystemColors.ActiveCaption;
            for (var i = 0; i < eventTeachDataGrid.dataGridView.Columns.Count; i++)
            {
                eventTeachDataGrid.dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                eventTeachDataGrid.dataGridView.Columns[i].ReadOnly = true;
            }
            eventTeachDataGrid.dataGridView.Columns[1].ReadOnly = false;
            eventTeachDataGrid.dataGridView.Columns[2].ReadOnly = false;

            eventTeachDataGrid.dataGridView.CellContentClick += eventTeachDataGrid_CellContentClick;

            var rowDelay = eventTeachDataGrid.dataGridView.Rows.Add();
            eventTeachDataGrid.dataGridView.Rows[rowDelay].Cells[0].Value = "Delay";
            eventTeachDataGrid.dataGridView.Rows[rowDelay].Cells[3].Value =
                eventTeachDataGrid.dataGridView.Columns[3].Name;
            eventTeachDataGrid.dataGridView.Rows[rowDelay].Cells[4].Value =
                eventTeachDataGrid.dataGridView.Columns[4].Name;
            eventTeachDataGrid.dataGridView.Rows[rowDelay].Cells[5].Value =
                eventTeachDataGrid.dataGridView.Columns[5].Name;

            var comboxDelay = eventTeachDataGrid.dataGridView.Rows[rowDelay].Cells[1] as DataGridViewComboBoxCell;
            var dataGridViewCellStyleDelay = new DataGridViewCellStyle { NullValue = _lstDelayStrs[0] };
            if (comboxDelay != null)
            {
                comboxDelay.Style = dataGridViewCellStyleDelay;
                comboxDelay.DataSource = _lstDelayStrs;
                comboxDelay.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
            }

            var rowSet = eventTeachDataGrid.dataGridView.Rows.Add();
            eventTeachDataGrid.dataGridView.Rows[rowSet].Cells[0].Value = "Set";
            eventTeachDataGrid.dataGridView.Rows[rowSet].Cells[3].Value =
                eventTeachDataGrid.dataGridView.Columns[3].Name;
            eventTeachDataGrid.dataGridView.Rows[rowSet].Cells[4].Value =
                eventTeachDataGrid.dataGridView.Columns[4].Name;
            eventTeachDataGrid.dataGridView.Rows[rowSet].Cells[5].Value =
                eventTeachDataGrid.dataGridView.Columns[5].Name;
            var comboxSet = eventTeachDataGrid.dataGridView.Rows[rowSet].Cells[1] as DataGridViewComboBoxCell;
            var dataGridViewCellStyleSet = new DataGridViewCellStyle { NullValue = _lstSetStrs[0] };
            if (comboxSet != null)
            {
                comboxSet.Style = dataGridViewCellStyleSet;
                comboxSet.DataSource = _lstSetStrs;
                comboxSet.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
            }

            var rowWait = eventTeachDataGrid.dataGridView.Rows.Add();
            eventTeachDataGrid.dataGridView.Rows[rowWait].Cells[0].Value = "Wait";

            eventTeachDataGrid.dataGridView.Rows[rowWait].Cells[3].Value =
                eventTeachDataGrid.dataGridView.Columns[3].Name;
            eventTeachDataGrid.dataGridView.Rows[rowWait].Cells[4].Value =
                eventTeachDataGrid.dataGridView.Columns[4].Name;
            eventTeachDataGrid.dataGridView.Rows[rowWait].Cells[5].Value =
                eventTeachDataGrid.dataGridView.Columns[5].Name;
            var comboxWait = eventTeachDataGrid.dataGridView.Rows[rowWait].Cells[1] as DataGridViewComboBoxCell;
            var dataGridViewCellStyleWait = new DataGridViewCellStyle { NullValue = _lstWaitStrs[0] };
            if (comboxWait != null)
            {
                comboxWait.Style = dataGridViewCellStyleWait;
                comboxWait.DataSource = _lstWaitStrs;
                comboxWait.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
            }

            dataGridViewProgramJog.RowsAdded += dataGridViewProgramJog_RowsAdded;
            dataGridViewProgramJog.CellMouseUp += dataGridViewProgramJog_CellMouseUp;
            dataGridViewProgramJog.SelectionChanged += dataGridViewProgramJog_SelectionChanged;

            for (var i = 0; i < eventTeachDataGrid.dataGridView.RowCount; i++)
            {
                var row = eventTeachDataGrid.dataGridView.Rows[i];

                {
                    var btn = row.Cells[3] as DataGridViewButtonCell;
                    if (btn != null)
                    {
                        btn.FlatStyle = FlatStyle.Flat;
                        btn.Style.BackColor = Color.DarkCyan;
                        btn.Style.ForeColor = Color.White;
                    }
                }

                for (var j = 4; j < 6; j++)
                {
                    var btn = row.Cells[j] as DataGridViewButtonCell;
                    if (btn != null)
                    {
                        btn.FlatStyle = FlatStyle.Flat;
                        btn.Style.BackColor = Color.Gray;
                        btn.Style.ForeColor = Color.White;
                    }
                }
            }

            jogTeachDataGrid.label.Text = @"轴运动示教";
            jogTeachDataGrid.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "轴号" });
            jogTeachDataGrid.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "轴名称" });
            jogTeachDataGrid.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "当前位置" });
            jogTeachDataGrid.dataGridView.Columns.Add(new DataGridViewComboBoxColumn { Name = "速度" });
            jogTeachDataGrid.dataGridView.Columns.Add(new DataGridViewComboBoxColumn { Name = "步进距离" });
            jogTeachDataGrid.dataGridView.Columns.Add(new DataGridViewButtonColumn { Name = "+" });
            jogTeachDataGrid.dataGridView.Columns.Add(new DataGridViewButtonColumn { Name = "-" });
            jogTeachDataGrid.dataGridView.Columns.Add(new DataGridViewButtonColumn { Name = "单轴点位添加行" });
            jogTeachDataGrid.dataGridView.Columns.Add(new DataGridViewButtonColumn { Name = "单轴点位插入行" });
            jogTeachDataGrid.dataGridView.Columns.Add(new DataGridViewButtonColumn { Name = "单轴点位更新行" });

            _lstStep.Add("10.0");
            _lstStep.Add("20.0");
            _lstStep.Add("50.0");
            _lstStep.Add("100.0");
            _lstStep.Add("1.0");
            _lstStep.Add("5.0");
            _lstStep.Add("0.01");
            _lstStep.Add("0.05");
            _lstStep.Add("0.1");
            _lstStep.Add("0.2");
            _lstStep.Add("0.5");

            _lstSpeed.Add("10.0");
            _lstSpeed.Add("20.0");
            _lstSpeed.Add("50.0");
            _lstSpeed.Add("100.0");
            _lstSpeed.Add("200.0");
            _lstSpeed.Add("300.0");
            _lstSpeed.Add("400.0");
            _lstSpeed.Add("500.0");
            _lstSpeed.Add("1.0");
            _lstSpeed.Add("5.0");
            //_lstSpeed.Add("600.0");
            //_lstSpeed.Add("700.0");
            //_lstSpeed.Add("800.0");
            //_lstSpeed.Add("900.0");
            //_lstSpeed.Add("1000.0");
            //_lstSpeed.Add("0.01");
            //_lstSpeed.Add("0.05");
            //_lstSpeed.Add("0.1");
            //_lstSpeed.Add("0.2");
            //_lstSpeed.Add("0.5");

            jogTeachDataGrid.dataGridView.ForeColor = Color.Black;
            jogTeachDataGrid.dataGridView.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8, FontStyle.Regular);
            jogTeachDataGrid.dataGridView.AllowUserToAddRows = false;
            jogTeachDataGrid.dataGridView.AllowUserToDeleteRows = false;
            jogTeachDataGrid.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            jogTeachDataGrid.dataGridView.AllowUserToResizeColumns = true;
            jogTeachDataGrid.dataGridView.AllowUserToResizeRows = false;
            jogTeachDataGrid.dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            jogTeachDataGrid.dataGridView.BackgroundColor = SystemColors.ActiveCaption;
            jogTeachDataGrid.dataGridView.RowHeadersVisible = false;
            jogTeachDataGrid.dataGridView.EditMode = DataGridViewEditMode.EditOnEnter;
            for (var i = 0; i < jogTeachDataGrid.dataGridView.Columns.Count; i++)
            {
                jogTeachDataGrid.dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                jogTeachDataGrid.dataGridView.Columns[i].ReadOnly = true;
            }

            jogTeachDataGrid.dataGridView.Columns[3].ReadOnly = false;
            jogTeachDataGrid.dataGridView.Columns[4].ReadOnly = false;

            jogTeachDataGrid.dataGridView.CellContentClick += jogTeachDataGridView_CellContentClick;
        }

        #region Main
        private void MainWork()
        {
            while (MainTh.IsAlive)
            {
                if (!MainTh.IsAlive)
                    break;

                Thread.Sleep(50);

                try
                {
                    lock (readDataGrid)
                    {
                        var fi = _robotController.GetType().GetFields();

                        foreach (var fieldInfo in fi)
                        {
                            if (Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)) == null)
                                continue;

                            var des =
                                ((DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)))
                                    .Description;

                            if (!des.StartsWith("R,RobotSate-") &&
                                !des.StartsWith("R,输入_DI") &&
                                !des.StartsWith("R/W,继电器_DO") &&
                                !des.StartsWith("R/W,低边输出_DO") &&
                                !des.StartsWith("R/W,通用输入SI") &&
                                !des.StartsWith("R/W,通用输出SO0"))
                                continue;

                            for (var i = 0; i < readDataGrid.dataGridView.RowCount; i++)
                            {
                                //if (readDataGrid.dataGridView.Rows[i].Cells[0].Value == null ||
                                //    !des.ToUpper().Contains(readDataGrid.dataGridView.Rows[i].Cells[0].Value.ToString().ToUpper()))
                                //    continue;
                                if (readDataGrid.dataGridView.Rows[i].Cells[0].Value == null)
                                    continue;
                                if (readDataGrid.dataGridView.Rows[i].Cells[0].Value.ToString() != fieldInfo.Name)
                                    continue;

                                readDataGrid.dataGridView.Rows[i].Cells[1].Value =
                                   fieldInfo.GetValue(_robotController) == null
                                       ? string.Empty
                                       : fieldInfo.GetValue(_robotController).ToString();
                                break;
                            }
                        }

                        lock (RobotControllerPpMode.LockState)
                        {
                            for (var i = 0; i < 16; i++)
                            {
                                var str = "------" + i + "号轴------";

                                foreach (var t in _robotController.SingleAxisState[i].Keys)
                                {
                                    for (var j = 0; j < readDataGrid.dataGridView.RowCount; j++)
                                    {
                                        var row = readDataGrid.dataGridView.Rows[j];

                                        if (row.Cells[0].Value == null ||
                                            row.Cells[0].Value.ToString() != str + "-" + t)
                                            continue;

                                        row.Cells[1].Value = _robotController.SingleAxisState[i][t];
                                    }

                                    for (var j = 0; j < jogTeachDataGrid.dataGridView.RowCount; j++)
                                    {
                                        var row = jogTeachDataGrid.dataGridView.Rows[j];

                                        if (row.Cells[0].Value == null || row.Cells[0].Value.ToString() != i.ToString())
                                            continue;
                                        row.Cells[2].Value =
                                            _robotController.SingleAxisState[i][_robotController.CurrentPos];

                                        if (_robotController.SingleAxisState[i][_robotController.IsStandstill] == true.ToString())
                                            row.DefaultCellStyle.BackColor = Color.DarkGoldenrod;
                                        else if (_robotController.SingleAxisState[i][_robotController.IsMoving] == true.ToString())
                                            row.DefaultCellStyle.BackColor = Color.DarkGreen;
                                        else
                                            row.DefaultCellStyle.BackColor = Color.DarkGray;

                                        if (_dataGridViewProgramJogCurrentRowIndex == -1)
                                        {
                                            for (var k = 8; k < 10; k++)
                                            {
                                                var btn = row.Cells[k] as DataGridViewButtonCell;
                                                if (btn == null)
                                                    continue;
                                                btn.FlatStyle = FlatStyle.Flat;
                                                btn.Style.BackColor = Color.Gray;
                                            }
                                        }
                                        else
                                        {
                                            for (var k = 8; k < 10; k++)
                                            {
                                                var btn = row.Cells[k] as DataGridViewButtonCell;
                                                if (btn == null)
                                                    continue;
                                                btn.FlatStyle = FlatStyle.Flat;
                                                btn.Style.BackColor = Color.DarkCyan;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            if (_dataGridViewProgramJogCurrentRowIndex == -1)
                            {
                                toolStripButtonUpdate.Enabled = false;

                                for (var i = 0; i < dataGridViewProgramJog.RowCount; i++)
                                {
                                    if (dataGridViewProgramJog.Rows[i].DefaultCellStyle.BackColor != Color.White)
                                        dataGridViewProgramJog.Rows[i].DefaultCellStyle.BackColor = Color.White;
                                }

                                if (btnInsert.FillColor != Color.Gray)
                                    btnInsert.FillColor = Color.Gray;
                                if (btnUpdate.FillColor != Color.Gray)
                                    btnUpdate.FillColor = Color.Gray;
                                if (btnInsert.Enabled)
                                    btnInsert.Enabled = false;
                                if (btnUpdate.Enabled)
                                    btnUpdate.Enabled = false;

                                //btnInsert.FillColor = btnUpdate.FillColor = Color.Gray;
                                //btnInsert.Enabled = btnUpdate.Enabled = false;

                                for (var i = 0; i < eventTeachDataGrid.dataGridView.RowCount; i++)
                                {
                                    var row = eventTeachDataGrid.dataGridView.Rows[i];
                                    for (var j = 4; j < 6; j++)
                                    {
                                        var btn = row.Cells[j] as DataGridViewButtonCell;
                                        if (btn == null)
                                            continue;
                                        btn.FlatStyle = FlatStyle.Flat;
                                        btn.Style.BackColor = Color.Gray;
                                    }
                                }
                            }
                            else
                            {
                                toolStripButtonUpdate.Enabled = dataGridViewProgramJog.Enabled;

                                for (var i = 0; i < dataGridViewProgramJog.RowCount; i++)
                                {
                                    if (i != _dataGridViewProgramJogCurrentRowIndex && dataGridViewProgramJog.Rows[i].DefaultCellStyle.BackColor != Color.White)
                                        dataGridViewProgramJog.Rows[i].DefaultCellStyle.BackColor = Color.White;
                                    else if (i == _dataGridViewProgramJogCurrentRowIndex && dataGridViewProgramJog.Rows[i].DefaultCellStyle.BackColor != _jogBlockSelectDefaultColor)
                                        dataGridViewProgramJog.Rows[_dataGridViewProgramJogCurrentRowIndex].DefaultCellStyle.BackColor = _jogBlockSelectDefaultColor;
                                }

                                // 255, 77, 59
                                btnInsert.FillColor = btnUpdate.FillColor = Color.DarkCyan;
                                btnInsert.Enabled = btnUpdate.Enabled = true;

                                for (var i = 0; i < eventTeachDataGrid.dataGridView.RowCount; i++)
                                {
                                    var row = eventTeachDataGrid.dataGridView.Rows[i];
                                    for (var j = 4; j < 6; j++)
                                    {
                                        var btn = row.Cells[j] as DataGridViewButtonCell;
                                        if (btn == null) continue;
                                        btn.FlatStyle = FlatStyle.Flat;
                                        btn.Style.BackColor = Color.DarkCyan;
                                    }
                                }
                            }
                        }));
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            lock (readDataGrid)
            {
                readDataGrid.dataGridView.Rows.Clear();
                if (lblAxisSelect.comboBox.Text == @"All")
                {
                    lock (RobotControllerPpMode.LockState)
                    {
                        for (var i = 0; i < 16; i++)
                        {
                            var random = new Random();
                            var b = 16 * (i + 1);
                            var randomColor = Color.FromArgb(random.Next(0, b), random.Next(0, b), random.Next(0, b));

                            var str = "------" + i + "号轴------";

                            foreach (var t in _robotController.SingleAxisState[i].Keys)
                            {
                                var rowAdd = readDataGrid.dataGridView.Rows.Add();
                                var row = readDataGrid.dataGridView.Rows[rowAdd];
                                row.Cells[0].Value = str + "-" + t;
                                row.Cells[1].Value = _robotController.SingleAxisState[i][t];
                                row.DefaultCellStyle.BackColor = randomColor;
                            }
                        }
                    }
                }
                else
                {
                    var i = lblAxisSelect.comboBox.SelectedIndex;
                    var random = new Random();
                    var b = 16 * (i + 1);
                    var randomColor = Color.FromArgb(random.Next(0, b), random.Next(0, b), random.Next(0, b));

                    var str = "------" + i + "号轴------";

                    lock (RobotControllerPpMode.LockState)
                    {
                        foreach (var t in _robotController.SingleAxisState[i].Keys)
                        {
                            var rowAdd = readDataGrid.dataGridView.Rows.Add();
                            var row = readDataGrid.dataGridView.Rows[rowAdd];
                            row.Cells[0].Value = str + "-" + t;
                            row.Cells[1].Value = _robotController.SingleAxisState[i][t];
                            row.DefaultCellStyle.BackColor = randomColor;
                        }
                    }
                }

                var fi = _robotController.GetType().GetFields();

                foreach (var fieldInfo in fi)
                {
                    if (Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)) == null)
                        continue;

                    var des =
                        ((DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)))
                            .Description;

                    if (des.StartsWith("R,RobotSate-") ||
                        des.StartsWith("R,输入_DI") ||
                        des.StartsWith("R/W,继电器_DO") ||
                        des.StartsWith("R/W,低边输出_DO") ||
                        des.StartsWith("R/W,通用输入SI") ||
                        des.StartsWith("R/W,通用输出SO0"))
                    {
                        var rowAdd = readDataGrid.dataGridView.Rows.Add();
                        var row = readDataGrid.dataGridView.Rows[rowAdd];
                        row.DefaultCellStyle.BackColor = Color.DarkGoldenrod;

                        row.Cells[0].Value = fieldInfo.Name;
                        row.Cells[1].Value = fieldInfo.GetValue(_robotController) == null
                            ? string.Empty
                            : fieldInfo.GetValue(_robotController).ToString();
                    }
                }
            }
        }

        private void writeTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cmb = sender as ComboBox;
            if (cmb == null)
                return;

            var type = cmb.Text;

            try
            {
                if (type.Equals("单轴控制"))
                {
                    writeDataGrid.dataGridView.Rows.Clear();
                    writeDataGrid.dataGridView.Columns.Clear();
                    writeDataGrid.label.Text = @"写入信息";
                    writeDataGrid.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "名称" });
                    writeDataGrid.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "数值" });

                    writeDataGrid.dataGridView.ForeColor = Color.Black;
                    writeDataGrid.dataGridView.AllowUserToAddRows = false;
                    writeDataGrid.dataGridView.AllowUserToDeleteRows = false;
                    writeDataGrid.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    writeDataGrid.dataGridView.AllowUserToResizeColumns = true;
                    writeDataGrid.dataGridView.AllowUserToResizeRows = false;
                    writeDataGrid.dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                    writeDataGrid.dataGridView.BackgroundColor = SystemColors.ActiveCaption;
                    writeDataGrid.dataGridView.RowHeadersVisible = false;
                    for (var i = 0; i < writeDataGrid.dataGridView.Columns.Count; i++)
                    {
                        writeDataGrid.dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                        writeDataGrid.dataGridView.Columns[i].ReadOnly = true;
                    }
                    writeDataGrid.dataGridView.Columns[1].ReadOnly = false;

                    writeAxisNo.Enabled = true;
                    btnWrite.Enabled = true;

                    var axisNo = writeAxisNo.comboBox.SelectedIndex;
                    var titleRow = writeDataGrid.dataGridView.Rows[writeDataGrid.dataGridView.Rows.Add()];
                    titleRow.Cells[0].Value = "------" + axisNo + "号轴------";

                    var speedRow = writeDataGrid.dataGridView.Rows[writeDataGrid.dataGridView.Rows.Add()];
                    speedRow.Cells[0].Value = _robotController.Speed;

                    var positionRow = writeDataGrid.dataGridView.Rows[writeDataGrid.dataGridView.Rows.Add()];
                    positionRow.Cells[0].Value = _robotController.Position;
                }
                else if (type.Equals("写Flash"))
                {
                    writeAxisNo.Enabled = false;
                    btnWrite.Enabled = false;

                    writeDataGrid.dataGridView.Rows.Clear();
                    writeDataGrid.dataGridView.Columns.Clear();
                    writeDataGrid.label.Text = @"写入信息";
                    writeDataGrid.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Content" });
                    //writeDataGrid.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "ParaName" });
                    writeDataGrid.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "StartByte" });
                    writeDataGrid.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "ByteLength" });
                    writeDataGrid.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "IsReserve" });
                    writeDataGrid.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "DataType" });
                    writeDataGrid.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "ReadBytes" });
                    writeDataGrid.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Value" });

                    writeDataGrid.dataGridView.ForeColor = Color.Black;
                    writeDataGrid.dataGridView.AllowUserToAddRows = false;
                    writeDataGrid.dataGridView.AllowUserToDeleteRows = false;
                    writeDataGrid.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    writeDataGrid.dataGridView.AllowUserToResizeColumns = true;
                    writeDataGrid.dataGridView.AllowUserToResizeRows = false;
                    writeDataGrid.dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                    writeDataGrid.dataGridView.BackgroundColor = SystemColors.ActiveCaption;
                    writeDataGrid.dataGridView.RowHeadersVisible = false;
                    for (var i = 0; i < writeDataGrid.dataGridView.Columns.Count; i++)
                    {
                        writeDataGrid.dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                        writeDataGrid.dataGridView.Columns[i].ReadOnly = true;
                    }

                    if (_robotController.ReadFlash())
                    {
                        writeDataGrid.dataGridView.Columns[6].ReadOnly = false;
                        btnWrite.Enabled = true;
                    }

                    if (RobotFlashConfig != null)
                    {
                        foreach (var f in RobotFlashConfig.Flash)
                        {
                            var rowAddedIndex = writeDataGrid.dataGridView.Rows.Add();

                            var row = writeDataGrid.dataGridView.Rows[rowAddedIndex];

                            row.Cells[0].Value = f.Content;
                            //row.Cells[1].Value = f.ParameterName;
                            row.Cells[1].Value = f.StartByte;
                            row.Cells[2].Value = f.ByteLength;
                            row.Cells[3].Value = f.IsReserve;
                            row.Cells[4].Value = f.DataType;

                            if (_robotController.FlashBytes != null)
                            {
                                var startByte = int.Parse(f.StartByte);
                                var byteLen = int.Parse(f.ByteLength);
                                var tempBytes = new byte[byteLen];
                                Array.Copy(_robotController.FlashBytes, startByte, tempBytes, 0, tempBytes.Length);

                                var tempStr =
                                    tempBytes.Aggregate(string.Empty,
                                        (current, t) => current + ValueHelper.GetHextStrWithOx(t) + " ").TrimEnd();
                                row.Cells[5].Value = tempStr;

                                if (f.IsReserve == "1")
                                    Array.Reverse(tempBytes);

                                var value = string.Empty;
                                var memo = string.Empty;

                                switch (f.DataType.ToLower())
                                {
                                    case "hex":
                                        value = tempBytes.Aggregate(value,
                                            (current, t) => current + ValueHelper.GetHextStr(t));
                                        break;

                                    case "ascii":
                                        value = tempBytes.GetStringByAsciiBytes(true);
                                        break;

                                    case "decimal":
                                        value = ValueHelper.GetDecimal(tempBytes).ToString();
                                        break;

                                    case "byte":
                                        value = tempBytes.Aggregate(value,
                                            (current, t) =>
                                                current + ValueHelper.GetDecimal(new[] { t }).ToString().PadLeft(3, '0'));
                                        break;

                                    case "float":
                                        value = BitConverter.ToSingle(tempBytes, 0).ToString(CultureInfo.InvariantCulture);
                                        break;

                                    case "long":
                                        value = "long";
                                        break;
                                }

                                row.Cells[6].Value = value;
                            }
                        }

                        for (var i = 0; i < 16; i++)
                        {
                            foreach (var f in RobotFlashConfig.AxisFlash)
                            {
                                var rowAddedIndex = writeDataGrid.dataGridView.Rows.Add();

                                var row = writeDataGrid.dataGridView.Rows[rowAddedIndex];

                                row.Cells[0].Value = "轴" + i + "_" + f.Content;
                                //row.Cells[1].Value = f.ParameterName;
                                row.Cells[1].Value = (int.Parse(f.StartByte) + i * 56).ToString();
                                row.Cells[2].Value = f.ByteLength;
                                row.Cells[3].Value = f.IsReserve;
                                row.Cells[4].Value = f.DataType;

                                if (_robotController.FlashBytes != null)
                                {
                                    var startByte = int.Parse((int.Parse(f.StartByte) + i * 56).ToString());
                                    var byteLen = int.Parse(f.ByteLength);
                                    var tempBytes = new byte[byteLen];
                                    Array.Copy(_robotController.FlashBytes, startByte, tempBytes, 0, tempBytes.Length);

                                    var tempStr =
                                        tempBytes.Aggregate(string.Empty,
                                            (current, t) => current + ValueHelper.GetHextStrWithOx(t) + " ").TrimEnd();
                                    row.Cells[5].Value = tempStr;

                                    if (f.IsReserve == "1")
                                        Array.Reverse(tempBytes);

                                    var value = string.Empty;
                                    var memo = string.Empty;

                                    switch (f.DataType.ToLower())
                                    {
                                        case "hex":
                                            value = tempBytes.Aggregate(value,
                                                (current, t) => current + ValueHelper.GetHextStr(t));
                                            break;

                                        case "ascii":
                                            value = tempBytes.GetStringByAsciiBytes(true);
                                            break;

                                        case "decimal":
                                            value = ValueHelper.GetDecimal(tempBytes).ToString();
                                            break;

                                        case "byte":
                                            value = tempBytes.Aggregate(value,
                                                (current, t) =>
                                                    current + ValueHelper.GetDecimal(new[] { t }).ToString().PadLeft(3, '0'));
                                            break;

                                        case "float":
                                            value = BitConverter.ToSingle(tempBytes, 0).ToString(CultureInfo.InvariantCulture);
                                            break;

                                        case "long":
                                            value = "long";
                                            break;
                                    }

                                    row.Cells[6].Value = value;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // ignored
            }
        }

        private void writeAxisNoComboBox_SelectedIndexChanged1(object sender, EventArgs e)
        {
            var cmb = sender as ComboBox;
            if (cmb == null)
                return;

            try
            {
                writeDataGrid.dataGridView.Rows.Clear();

                if (writeTypeCmb.comboBox.Text.Equals("单轴控制"))
                {
                    var axisNo = writeAxisNo.comboBox.SelectedIndex;
                    var titleRow = writeDataGrid.dataGridView.Rows[writeDataGrid.dataGridView.Rows.Add()];
                    titleRow.Cells[0].Value = "------" + axisNo + "号轴------";

                    var speedRow = writeDataGrid.dataGridView.Rows[writeDataGrid.dataGridView.Rows.Add()];
                    speedRow.Cells[0].Value = _robotController.Speed;

                    var positionRow = writeDataGrid.dataGridView.Rows[writeDataGrid.dataGridView.Rows.Add()];
                    positionRow.Cells[0].Value = _robotController.Position;
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            if (writeTypeCmb.Text == @"单轴控制")
            {
                var axisIndex = writeAxisNo.comboBox.SelectedIndex;
                if (writeDataGrid.dataGridView.Rows[1].Cells[1].Value == null ||
                    writeDataGrid.dataGridView.Rows[2].Cells[1].Value == null)
                    return;
                float speed;
                float pos;
                if (!float.TryParse(writeDataGrid.dataGridView.Rows[1].Cells[1].Value.ToString(), out speed) ||
                    !float.TryParse(writeDataGrid.dataGridView.Rows[2].Cells[1].Value.ToString(), out pos)) return;
                var d = new Dictionary<int, Dictionary<string, string>>
                {
                    {axisIndex, new Dictionary<string, string>()},
                };
                //d[axisIndex].Add(_robotController.Position,
                //    BitConverter.ToSingle(new byte[] { 0x43, 0x96, 0x00, 0x00 }.Reverse().ToArray(), 0).ToString(CultureInfo.InvariantCulture));
                //d[axisIndex].Add(_robotController.Speed,
                //    BitConverter.ToSingle(new byte[] { 0x42, 0xC8, 0x00, 0x00 }.Reverse().ToArray(), 0).ToString(CultureInfo.InvariantCulture));
                d[axisIndex].Add(_robotController.Position,
                    pos.ToString(CultureInfo.InvariantCulture));
                d[axisIndex].Add(_robotController.Speed,
                    speed.ToString(CultureInfo.InvariantCulture));
                _robotController.RobotRun(d);
            }
            else
            {
                var listBs = new byte[4096];
                var errorMsg = string.Empty;

                for (var i = 0; i < writeDataGrid.dataGridView.RowCount; i++)
                {
                    var row = writeDataGrid.dataGridView.Rows[i];

                    var content = row.Cells[0].Value.ToString();
                    var startByte = int.Parse(row.Cells[1].Value.ToString());
                    var byteLen = int.Parse(row.Cells[2].Value.ToString());
                    var dataTyepe = row.Cells[4].Value.ToString();
                    var isReserve = row.Cells[3].Value.ToString() == "1";

                    if (row.Cells[6].Value == null || string.IsNullOrEmpty(row.Cells[6].Value.ToString()))
                    {
                        errorMsg += string.Format("第{0}行，{1}，写入的数据是空，请检查。\r\n", i + 1, content);
                    }
                    else
                    {
                        var value = row.Cells[6].Value.ToString();
                        var tempBytes = new List<byte>();

                        switch (dataTyepe.ToLower())
                        {
                            case "hex":
                                if (value.Length != byteLen * 2)
                                {
                                    errorMsg += string.Format("第{0}行，{1}，应该填入{2}个hex字符，每个字符长度为2，不足的补0，请检查。\r\n", i + 1,
                                        content, byteLen);
                                }
                                else
                                {
                                    for (var j = 0; j < value.Length; j = j + 2)
                                    {
                                        try
                                        {
                                            tempBytes.Add(Convert.ToByte(string.Format("{0}{1}", value[j], value[j + 1]), 16));
                                        }
                                        catch (Exception ex)
                                        {
                                            errorMsg += string.Format("第{0}行，{1}，应该填入{2}个hex字符，{3}\r\n", i + 1, byteLen,
                                                content,
                                                ex.Message);
                                            break;
                                        }
                                    }

                                    if (tempBytes.Count == byteLen)
                                    {
                                        var tt = tempBytes.ToArray();
                                        if (isReserve)
                                            Array.Reverse(tt);
                                        Array.Copy(tt, 0, listBs, startByte, byteLen);
                                    }
                                }
                                break;

                            case "ascii":
                                if (value.Length != byteLen)
                                {
                                    errorMsg += string.Format("第{0}行，{1}，应该填入{2}个ascii字符，请检查。\r\n", i + 1,
                                        content, byteLen);
                                }
                                else
                                {
                                    try
                                    {
                                        tempBytes.AddRange(Encoding.ASCII.GetBytes(value));
                                        if (tempBytes.Count == byteLen)
                                        {
                                            var tt = tempBytes.ToArray();
                                            if (isReserve)
                                                Array.Reverse(tt);
                                            Array.Copy(tt, 0, listBs, startByte, byteLen);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        errorMsg += string.Format("第{0}行，{1}，应该填入{2}个ascii字符，{3}\r\n", i + 1, byteLen,
                                               content,
                                               ex.Message);
                                    }
                                }
                                break;

                            case "decimal":
                                try
                                {
                                    var decimalVaue = BitConverter.GetBytes(int.Parse(value));
                                    Array.Reverse(decimalVaue);

                                    if (isReserve)
                                    {
                                        var j = decimalVaue.Length - 1;
                                        for (var k = 0; k < byteLen; k++)
                                        {
                                            tempBytes.Add(decimalVaue[j]);
                                            j--;
                                        }
                                    }
                                    else
                                    {
                                        var j = 4 - byteLen;
                                        for (var k = 0; k < byteLen; k++)
                                        {
                                            tempBytes.Add(decimalVaue[j]);
                                            j++;
                                        }
                                    }

                                    if (tempBytes.Count == byteLen)
                                        Array.Copy(tempBytes.ToArray(), 0, listBs, startByte, byteLen);
                                }
                                catch (Exception ex)
                                {
                                    errorMsg += string.Format("第{0}行，{1}，应该填入整数字符，{2}\r\n", i + 1, byteLen, ex.Message);
                                }
                                break;

                            case "byte":
                                if (value.Length != byteLen * 3)
                                {
                                    errorMsg += string.Format("第{0}行，{1}，应该填入{2}个0~255的整数，每个字符长度为3，不足的补0，请检查。\r\n", i + 1,
                                        content, byteLen);
                                }
                                else
                                {
                                    for (var j = 0; j < value.Length; j = j + 3)
                                    {
                                        try
                                        {
                                            tempBytes.Add(
                                                byte.Parse(string.Format("{0}{1}{2}", value[j], value[j + 1], value[j + 2])));
                                        }
                                        catch (Exception ex)
                                        {
                                            errorMsg += string.Format("第{0}行，{1}，应该填入{2}个0~255的整数，{3}\r\n", i + 1, content,
                                                byteLen, ex.Message);
                                            break;
                                        }
                                    }

                                    if (tempBytes.Count == byteLen)
                                    {
                                        var tt = tempBytes.ToArray();
                                        if (isReserve)
                                            Array.Reverse(tt);
                                        Array.Copy(tt, 0, listBs, startByte, byteLen);
                                    }
                                }
                                break;

                            case "float":
                                try
                                {
                                    var fv = BitConverter.GetBytes(Convert.ToSingle(value));
                                    if (isReserve)
                                        Array.Reverse(fv);
                                    tempBytes.AddRange(fv);
                                    if (tempBytes.Count == byteLen)
                                        Array.Copy(tempBytes.ToArray(), 0, listBs, startByte, byteLen);
                                }
                                catch (Exception ex)
                                {
                                    errorMsg += string.Format("第{0}行，{1}，应该填入float字符，{2}\r\n", i + 1, content, ex.Message);
                                }
                                break;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(errorMsg))
                {
                    MessageBox.Show(errorMsg);
                }
                else
                {
                    var sendBytes = new byte[480 * 2];
                    Array.Copy(listBs, 0, sendBytes, 0, sendBytes.Length);

                    var debugStr = sendBytes.Aggregate(string.Empty,
                                   (current, t) => current + ValueHelper.GetHextStrWithOx(t) + " ").TrimEnd();
                    Debug.WriteLine(debugStr);

                    _robotController.WriteFlash(sendBytes);

                    //MessageBox.Show(_robotController.WriteFlash(sendBytes) ? @"写Flash成功，请重新上电后读取确认" : @"写Flash失败");
                }
            }
        }
        #endregion

        #region Event

        private void eventDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var dgv = sender as DataGridView;
            if (dgv == null)
                return;

            if (!ShowBox())
                return;

            switch (dgv.Columns[e.ColumnIndex].Name)
            {
                case "空事件":
                    _robotController.RunEventNull(e.RowIndex.ToString());
                    break;

                case "使能":
                    _robotController.RunEventServoOn(e.RowIndex.ToString());
                    break;

                case "失能":
                    _robotController.RunEventServoOff(e.RowIndex.ToString());
                    break;

                case "软急停":
                    _robotController.RunEventEmergencyStop(e.RowIndex.ToString());
                    break;

                case "重置":
                    _robotController.RunEventReset(e.RowIndex.ToString());
                    break;

                case "回原点/轴复位":
                    _robotController.RunEventHome(e.RowIndex.ToString());
                    break;

                case "设定原点偏移":
                    _robotController.RunEventSetHomeOffset(e.RowIndex.ToString());
                    break;

                case "停止":
                    _robotController.RunEventStop(e.RowIndex.ToString());
                    break;

                case "暂停":
                    _robotController.RunEventHalt(e.RowIndex.ToString());
                    break;

                case "继续":
                    _robotController.RunEventContinue(e.RowIndex.ToString());
                    break;
            }
        }

        private void ucBtnWriteAllEvent_BtnClick(object sender, EventArgs e)
        {
            // 00 01 00 00 00 C9 01 6B 00 20 00 61 C2 00 10 00 00 00 01 00 00 00 00 00 00 00 00 00 01 00 01 00 00 00 00 00 00 00 00 00 02 00 01 00 00 00 00 00 00 00 00 00 03 00 01 00 00 00 00 00 00 00 00 00 04 00 01 00 00 00 00 00 00 00 00 00 05 00 01 00 00 00 00 00 00 00 00 00 06 00 01 00 00 00 00 00 00 00 00 00 07 00 01 00 00 00 00 00 00 00 00 00 08 00 01 00 00 00 00 00 00 00 00 00 09 00 01 00 00 00 00 00 00 00 00 00 0A 00 01 00 00 00 00 00 00 00 00 00 0B 00 01 00 00 00 00 00 00 00 00 00 0C 00 01 00 00 00 00 00 00 00 00 00 0D 00 01 00 00 00 00 00 00 00 00 00 0E 00 01 00 00 00 00 00 00 00 00 00 0F 00 01 00 00 00 00 00 00 00 00

            var result =
               MessageBox.Show(@"你确定吗？？？", @"写入前确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            var btn = sender as UCBtnExt;
            if (btn != null)
            {
                switch (btn.BtnText)
                {
                    case "空事件":
                        _robotController.RunEventNullAll();
                        break;

                    case "使能":
                        _robotController.RunEventServoOnAll();
                        break;

                    case "失能":
                        _robotController.RunEventServoOffAll();
                        break;

                    case "软急停":
                        _robotController.RunEventEmergencyStopAll();
                        break;

                    case "重置":
                        _robotController.RunEventResetAll();
                        break;

                    case "回原点/轴复位":
                        _robotController.RunEventHomeAll();
                        break;

                    case "停止":
                        _robotController.RunEventStopAll();
                        break;

                    case "暂停":
                        _robotController.RunEventHaltAll();
                        break;

                    case "继续":
                        _robotController.RunEventContinueAll();
                        break;
                }
            }
        }
        #endregion

        #region Teach

        private void dataGridViewProgramList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridViewSelectedProgram.Rows.Clear();

            if (e.RowIndex != -1)
            {
                if (dataGridViewProgramList.Rows[e.RowIndex].Cells[1].Value != null)
                {
                    var programName = dataGridViewProgramList.Rows[e.RowIndex].Cells[1].Value.ToString();
                    if (!string.IsNullOrEmpty(programName) && _robotController.RobotJogging.RobotPrograms != null)
                    {
                        var program =
                            _robotController.RobotJogging.RobotPrograms.ToList().Find(f => f.Name == programName);
                        if (program != null)
                        {
                            if (program.Blocks != null)
                            {
                                for (var i = 0; i < program.Blocks.Length; i++)
                                {
                                    var b = program.Blocks[i];

                                    var newRow =
                                        dataGridViewSelectedProgram.Rows[dataGridViewSelectedProgram.Rows.Add()];

                                    newRow.Cells[0].Value = i;
                                    newRow.Cells[1].Value = b;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void toolStripButtonUpdate_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(labelTextJogProgramName.Text) && _dataGridViewProgramJogCurrentRowIndex != -1 && dataGridViewProgramJog.Enabled)
            {
                //MessageBox.Show(
                //    dataGridViewProgramJog.Rows[_dataGridViewProgramJogCurrentRowIndex].Cells[1].Value.ToString());

                if (dataGridViewProgramJog.Rows[_dataGridViewProgramJogCurrentRowIndex].Cells[1].Value.ToString().StartsWith("Pallet"))
                {
                    var pallet = dataGridViewProgramJog.Rows[_dataGridViewProgramJogCurrentRowIndex].Cells[1].Value.ToString().Replace("Pallet:", string.Empty)
                            .Split(new[] { "[", "]" }, StringSplitOptions.RemoveEmptyEntries);

                    var palletIndex = -1;
                    foreach (var s in from t in pallet
                                      where !t.StartsWith("//")
                                      select t.Split(';')
                                          into sp
                                          from s in sp.Where(s => s.StartsWith("PalletIndex="))
                                          select s)
                        palletIndex = int.Parse(s.Replace("PalletIndex=", string.Empty));

                    using (var frm = new RobotFormPalletManagement(_robotController, palletIndex: palletIndex))
                    {
                        if (frm.ShowDialog() != DialogResult.OK)
                            return;

                        //var str = string.Format("Pallet:[PalletName=N{0};PalletIndex={0}]//(码垛N{0})",
                        //    RobotFormPalletManagement.SelectedPalletIndex);

                        //if (_dataGridViewProgramJogCurrentRowIndex != -1)
                        //{
                        //    dataGridViewProgramJog.Rows.Insert(_dataGridViewProgramJogCurrentRowIndex, 1);
                        //    var row = dataGridViewProgramJog.Rows[_dataGridViewProgramJogCurrentRowIndex];
                        //    row.Cells[1].Value = str;
                        //    UpdateIndex();
                        //    _dataGridViewProgramJogCurrentRowIndex++;
                        //}
                        //else
                        //{
                        //    var newRow = dataGridViewProgramJog.Rows.Add();
                        //    var row = dataGridViewProgramJog.Rows[newRow];
                        //    row.Cells[1].Value = str;
                        //    UpdateIndex();
                        //}
                    }
                }
                else
                {
                    using (var frm = new RobotControllerProgramUpdate(
                    _robotController.RobotJogging.RobotPrograms.ToList().Find(f => f.Name == labelTextJogProgramName.Text),
                    dataGridViewProgramJog.Rows[_dataGridViewProgramJogCurrentRowIndex].Cells[1].Value.ToString()))
                    {
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            var selectProgramName = labelTextJogProgramName.Text;

                            var toUpdateIndex = _dataGridViewProgramJogCurrentRowIndex;

                            _dataGridViewProgramJogCurrentRowIndex = -1;
                            dataGridViewProgramJog.Rows.Clear();

                            var program = _robotController.RobotJogging.RobotPrograms.ToList().Find(f => f.Name == selectProgramName);
                            tabControlTeach.SelectedIndex = 1;

                            if (program == null) return;
                            if (program.Blocks == null)
                                return;

                            for (var i = 0; i < program.Blocks.Length; i++)
                            {
                                var rowNum = dataGridViewProgramJog.Rows.Add();
                                if (i == toUpdateIndex)
                                {
                                    dataGridViewProgramJog.Rows[rowNum].Cells[0].Value = i;
                                    dataGridViewProgramJog.Rows[rowNum].Cells[1].Value = frm.UpdatedCode;
                                }
                                else
                                {
                                    dataGridViewProgramJog.Rows[rowNum].Cells[0].Value = i;
                                    dataGridViewProgramJog.Rows[rowNum].Cells[1].Value = program.Blocks[i];
                                }
                            }

                            jogTeachDataGrid.dataGridView.Rows.Clear();

                            for (var i = 0; i < program.AxisList.Length; i++)
                            {
                                var jogRow = jogTeachDataGrid.dataGridView.Rows.Add();
                                jogTeachDataGrid.dataGridView.Rows[jogRow].Height = 50;
                                jogTeachDataGrid.dataGridView.Rows[jogRow].DefaultCellStyle.BackColor = Color.DarkGoldenrod;

                                jogTeachDataGrid.dataGridView.Rows[jogRow].Cells[0].Value = program.AxisList[i].AxisNo;
                                jogTeachDataGrid.dataGridView.Rows[jogRow].Cells[1].Value = program.AxisList[i].AxisNote;

                                var comboxSpeed = jogTeachDataGrid.dataGridView.Rows[jogRow].Cells[3] as DataGridViewComboBoxCell;
                                var dataGridViewCellStyleSpeed = new DataGridViewCellStyle { NullValue = _lstSpeed[0] };
                                if (comboxSpeed != null)
                                {
                                    comboxSpeed.Style = dataGridViewCellStyleSpeed;
                                    comboxSpeed.DataSource = _lstSpeed;
                                    comboxSpeed.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
                                }

                                var comboxStep = jogTeachDataGrid.dataGridView.Rows[jogRow].Cells[4] as DataGridViewComboBoxCell;
                                var dataGridViewCellStyleStep = new DataGridViewCellStyle { NullValue = _lstStep[0] };
                                if (comboxStep != null)
                                {
                                    comboxStep.Style = dataGridViewCellStyleStep;
                                    comboxStep.DataSource = _lstStep;
                                    comboxStep.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
                                }

                                jogTeachDataGrid.dataGridView.Rows[jogRow].Cells[5].Value = "+";
                                jogTeachDataGrid.dataGridView.Rows[jogRow].Cells[6].Value = "-";
                            }

                            var pmName = labelTextJogProgramName.textBox.Text;
                            if (!string.IsNullOrEmpty(pmName))
                            {
                                var tempList = _robotController.RobotJogging.RobotPrograms.ToList();
                                var pm = tempList.Find(f => f.Name == pmName);
                                if (pm == null)
                                    return;
                                pm.Blocks = new string[dataGridViewProgramJog.RowCount];
                                for (var i = 0; i < dataGridViewProgramJog.RowCount; i++)
                                    pm.Blocks[i] = dataGridViewProgramJog.Rows[i].Cells[1].Value.ToString();
                                _robotController.RobotJogging.RobotPrograms = tempList.ToArray();
                                XmlHelper.SerializeToFile(_robotController.RobotJogging, RobotControllerPpMode.ConfilePath, Encoding.UTF8);
                            }
                        }
                    }
                }
            }
        }

        private void toolStripButtonExpot_Click(object sender, EventArgs e)
        {
            var fileDialog = new SaveFileDialog { Filter = @"xml文件|*.xml" };
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                XmlHelper.SerializeToFile(_robotController.RobotJogging, fileDialog.FileName, Encoding.UTF8);
            }
        }

        private void toolStripButtonImport_Click(object sender, EventArgs e)
        {
            //XmlHelper.SerializeToFile(_robotController.RobotJogging, RobotControllerPpMode.ConfilePath, Encoding.UTF8);

            var fileDialog = new SaveFileDialog { Filter = @"xml文件|*.xml" };
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                // XmlHelper.Deserialize<RobotJogging>(ConfilePath);
                try
                {
                    var newRobotJogging = XmlHelper.Deserialize<RobotJogging>(fileDialog.FileName);
                    XmlHelper.SerializeToFile(newRobotJogging, RobotControllerPpMode.ConfilePath, Encoding.UTF8);
                    MessageBox.Show(@"导入成功，重启程序后生效！");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("导入失败：{0}", ex.Message));
                }
            }
        }

        private int _dataGridViewProgramJogCurrentRowIndex = -1;

        private void toolStripButtonOpenToTeach_Click(object sender, EventArgs e)
        {
            if (dataGridViewProgramList.SelectedRows.Count != 1)
                return;
            if (dataGridViewProgramList.RowCount < 1)
                return;

            if (!ShowBox())
                return;

            var row = dataGridViewProgramList.SelectedRows[0].Index;
            var selectProgramName = dataGridViewProgramList.Rows[row].Cells[1].Value.ToString();

            labelTextJogProgramName.Text = selectProgramName;

            _dataGridViewProgramJogCurrentRowIndex = -1;
            dataGridViewProgramJog.Rows.Clear();

            var program = _robotController.RobotJogging.RobotPrograms.ToList().Find(f => f.Name == selectProgramName);
            tabControlTeach.SelectedIndex = 1;

            if (program == null) return;
            if (program.Blocks == null)
                return;

            for (var i = 0; i < program.Blocks.Length; i++)
            {
                var rowNum = dataGridViewProgramJog.Rows.Add();
                dataGridViewProgramJog.Rows[rowNum].Cells[0].Value = i;
                dataGridViewProgramJog.Rows[rowNum].Cells[1].Value = program.Blocks[i];
                //dataGridViewProgramJog.Rows[rowNum].DefaultCellStyle.Font = new Font("微软雅黑", 8);
                //dataGridViewProgramJog.Rows[rowNum].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                //dataGridViewProgramJog.Rows[rowNum].Height = 50;
            }

            jogTeachDataGrid.dataGridView.Rows.Clear();

            for (var i = 0; i < program.AxisList.Length; i++)
            {
                var jogRow = jogTeachDataGrid.dataGridView.Rows.Add();
                jogTeachDataGrid.dataGridView.Rows[jogRow].Height = 50;
                jogTeachDataGrid.dataGridView.Rows[jogRow].DefaultCellStyle.BackColor = Color.DarkGoldenrod;

                jogTeachDataGrid.dataGridView.Rows[jogRow].Cells[0].Value = program.AxisList[i].AxisNo;
                jogTeachDataGrid.dataGridView.Rows[jogRow].Cells[1].Value = program.AxisList[i].AxisNote;

                var comboxSpeed = jogTeachDataGrid.dataGridView.Rows[jogRow].Cells[3] as DataGridViewComboBoxCell;
                var dataGridViewCellStyleSpeed = new DataGridViewCellStyle { NullValue = _lstSpeed[0] };
                if (comboxSpeed != null)
                {
                    comboxSpeed.Style = dataGridViewCellStyleSpeed;
                    comboxSpeed.DataSource = _lstSpeed;
                    comboxSpeed.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
                }

                var comboxStep = jogTeachDataGrid.dataGridView.Rows[jogRow].Cells[4] as DataGridViewComboBoxCell;
                var dataGridViewCellStyleStep = new DataGridViewCellStyle { NullValue = _lstStep[0] };
                if (comboxStep != null)
                {
                    comboxStep.Style = dataGridViewCellStyleStep;
                    comboxStep.DataSource = _lstStep;
                    comboxStep.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
                }

                jogTeachDataGrid.dataGridView.Rows[jogRow].Cells[5].Value = "+";
                jogTeachDataGrid.dataGridView.Rows[jogRow].Cells[6].Value = "-";

                jogTeachDataGrid.dataGridView.Rows[jogRow].Cells[7].Value = "添加行";
                jogTeachDataGrid.dataGridView.Rows[jogRow].Cells[8].Value = "插入行";
                jogTeachDataGrid.dataGridView.Rows[jogRow].Cells[9].Value = "更新行";

                for (var j = 7; j < 10; j++)
                {
                    var btn = jogTeachDataGrid.dataGridView.Rows[jogRow].Cells[j] as DataGridViewButtonCell;
                    if (btn != null && j != 7)
                    {
                        btn.FlatStyle = FlatStyle.Flat;
                        btn.Style.BackColor = Color.Gray;
                        btn.Style.ForeColor = Color.White;
                    }
                    else if (btn != null && j == 7)
                    {
                        btn.FlatStyle = FlatStyle.Flat;
                        btn.Style.BackColor = Color.DarkCyan;
                        btn.Style.ForeColor = Color.White;
                    }
                }
            }
        }

        private void dataGridViewProgramJog_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            //dataGridViewProgramJog.Rows[e.RowIndex].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewProgramJog.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("微软雅黑", 9);
            //dataGridViewProgramJog.AutoResizeColumns();
            //dataGridViewProgramJog.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader;
        }

        private void dataGridViewProgramJog_CellMouseUp(
            object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

            if (e.Button == MouseButtons.Right)
            {
                if (dataGridViewProgramJog.CurrentRow != null && e.RowIndex == _dataGridViewProgramJogCurrentRowIndex)
                {
                    _dataGridViewProgramJogCurrentRowIndex = -1;

                    //dataGridViewProgramJog.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;

                    //btnInsert.FillColor = btnUpdate.FillColor = Color.Gray;
                    //btnInsert.Enabled = btnUpdate.Enabled = false;

                    //for (var i = 0; i < eventTeachDataGrid.dataGridView.RowCount; i++)
                    //{
                    //    var row = eventTeachDataGrid.dataGridView.Rows[i];
                    //    for (var j = 4; j < 6; j++)
                    //    {
                    //        var btn = row.Cells[j] as DataGridViewButtonCell;
                    //        if (btn != null)
                    //        {
                    //            btn.FlatStyle = FlatStyle.Flat;
                    //            btn.Style.BackColor = Color.Gray;
                    //        }
                    //    }
                    //}
                }
            }
            else if (e.Button == MouseButtons.Left)
            {
                _dataGridViewProgramJogCurrentRowIndex = e.RowIndex;

                //for (var i = 0; i < dataGridViewProgramJog.RowCount; i++)
                //    dataGridViewProgramJog.Rows[i].DefaultCellStyle.BackColor = Color.White;

                //dataGridViewProgramJog.Rows[e.RowIndex].DefaultCellStyle.BackColor = _jogBlockSelectDefaultColor;

                //// 255, 77, 59
                //btnInsert.FillColor = btnUpdate.FillColor = Color.FromArgb(255, 77, 59);
                //btnInsert.Enabled = btnUpdate.Enabled = true;

                //for (var i = 0; i < eventTeachDataGrid.dataGridView.RowCount; i++)
                //{
                //    var row = eventTeachDataGrid.dataGridView.Rows[i];
                //    for (var j = 4; j < 6; j++)
                //    {
                //        var btn = row.Cells[j] as DataGridViewButtonCell;
                //        if (btn != null)
                //        {
                //            btn.FlatStyle = FlatStyle.Flat;
                //            btn.Style.BackColor = Color.FromArgb(255, 77, 59);
                //        }
                //    }
                //}
            }
        }

        private void dataGridViewProgramJog_SelectionChanged(
            object sender, EventArgs e)
        {
            dataGridViewProgramJog.ClearSelection();
        }

        private void jogTeachDataGridView_CellContentClick(
           object sender, DataGridViewCellEventArgs e)
        {
            var dgv = sender as DataGridView;
            if (dgv == null)
                return;

            if (dgv.Columns[e.ColumnIndex].Name == "+" || dgv.Columns[e.ColumnIndex].Name == "-")
            {
                try
                {
                    jogTeachDataGrid.dataGridView.EndEdit();

                    if (jogTeachDataGrid.dataGridView.Rows[e.RowIndex].Cells[2].Value != null &&
                        jogTeachDataGrid.dataGridView.Rows[e.RowIndex].Cells[3].FormattedValue != null &&
                        jogTeachDataGrid.dataGridView.Rows[e.RowIndex].Cells[4].FormattedValue != null)
                    {
                        var currentPos = float.Parse(jogTeachDataGrid.dataGridView.Rows[e.RowIndex].Cells[2].Value.ToString());
                        var speed = float.Parse(jogTeachDataGrid.dataGridView.Rows[e.RowIndex].Cells[3].FormattedValue.ToString());
                        var axisNo = int.Parse(jogTeachDataGrid.dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString());
                        var step = float.Parse(jogTeachDataGrid.dataGridView.Rows[e.RowIndex].Cells[4].FormattedValue.ToString());

                        if (speed > 0)
                        {
                            lock (RobotControllerPpMode.LockState)
                            {
                                if (_robotController.SingleAxisState[axisNo][_robotController.IsMoving] == false.ToString() &&
                            _robotController.SingleAxisState[axisNo][_robotController.IsStandstill] == true.ToString())
                                {
                                    switch (dgv.Columns[e.ColumnIndex].Name)
                                    {
                                        case "+":
                                            currentPos = currentPos + step;
                                            break;

                                        case "-":
                                            currentPos = currentPos - step;
                                            break;
                                    }

                                    var d = new Dictionary<int, Dictionary<string, string>>
                            {
                                {
                                    axisNo, new Dictionary<string, string>()
                                },
                            };
                                    d[axisNo].Add(_robotController.Speed, speed.ToString(CultureInfo.InvariantCulture));
                                    d[axisNo].Add(_robotController.Position, currentPos.ToString(CultureInfo.InvariantCulture));
                                    _robotController.RobotRun(d);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(@"输入的参数有误， " + ex.Message);
                }
            }
            else if (dgv.Columns[e.ColumnIndex].Name.Contains("添加") ||
                     dgv.Columns[e.ColumnIndex].Name.Contains("插入") ||
                     dgv.Columns[e.ColumnIndex].Name.Contains("更新"))
            {
                var btn = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewButtonCell;
                if (btn == null)
                    return;
                if (btn.Style.BackColor == Color.Gray)
                    return;

                //var block=string.Format()
                var currentPos = float.Parse(dgv.Rows[e.RowIndex].Cells[2].Value.ToString());
                var speed = float.Parse(dgv.Rows[e.RowIndex].Cells[3].FormattedValue == null ? 0.ToString() : dgv.Rows[e.RowIndex].Cells[3].FormattedValue.ToString());
                var axisNo = int.Parse(dgv.Rows[e.RowIndex].Cells[0].Value.ToString());

                var str = string.Format("[Axis={0};Pos={1};Speed={2}]", axisNo, currentPos, speed);
                var block = string.Format("Move:{0}//(未定义)", str);

                if (ShowBox())
                {
                    if (dgv.Columns[e.ColumnIndex].Name.Contains("添加"))
                    {
                        var newRow = dataGridViewProgramJog.Rows.Add();
                        var row = dataGridViewProgramJog.Rows[newRow];
                        row.Cells[1].Value = block;
                        UpdateIndex();
                    }
                    else if (dgv.Columns[e.ColumnIndex].Name.Contains("插入"))
                    {
                        if (dataGridViewProgramJog.CurrentRow != null)
                        {
                            if (_dataGridViewProgramJogCurrentRowIndex != -1)
                            {
                                dataGridViewProgramJog.Rows.Insert(_dataGridViewProgramJogCurrentRowIndex, 1);
                                var row = dataGridViewProgramJog.Rows[_dataGridViewProgramJogCurrentRowIndex];
                                row.Cells[1].Value = block;
                                UpdateIndex();
                                _dataGridViewProgramJogCurrentRowIndex++;
                            }
                        }
                    }
                    else if (dgv.Columns[e.ColumnIndex].Name.Contains("更新"))
                    {
                        if (dataGridViewProgramJog.CurrentRow != null)
                        {
                            if (_dataGridViewProgramJogCurrentRowIndex != -1)
                            {
                                var row = dataGridViewProgramJog.Rows[_dataGridViewProgramJogCurrentRowIndex];
                                row.Cells[1].Value = block;
                                UpdateIndex();
                            }
                        }
                    }
                }
            }
        }

        private void eventTeachDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var dgv = sender as DataGridView;
            if (dgv == null)
                return;

            if (dgv.Columns[e.ColumnIndex].Name != "添加到末尾" &&
                dgv.Columns[e.ColumnIndex].Name != "插入到上一行" &&
                dgv.Columns[e.ColumnIndex].Name != "更新当前行")
                return;

            var btn = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewButtonCell;
            if (btn == null)
                return;
            if (btn.Style.BackColor == Color.Gray)
                return;

            try
            {
                eventTeachDataGrid.dataGridView.EndEdit();

                var code = string.Empty;
                if (eventTeachDataGrid.dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString() == "Delay")
                {
                    // Delay:[WaitTime=500]//(等待延时)
                    if (eventTeachDataGrid.dataGridView.Rows[e.RowIndex].Cells[2].Value != null)
                    {
                        int ms;
                        if (int.TryParse(eventTeachDataGrid.dataGridView.Rows[e.RowIndex].Cells[2].Value.ToString(), out ms))
                        {
                            code = string.Format("Delay:[{0}={1}]//(等待延时)",
                                eventTeachDataGrid.dataGridView.Rows[e.RowIndex].Cells[1].FormattedValue, ms);
                        }
                    }
                }
                else if (eventTeachDataGrid.dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString() == "Set")
                {
                    // Set:[D01=1]//(输出夹爪信号)
                    if (eventTeachDataGrid.dataGridView.Rows[e.RowIndex].Cells[2].Value != null)
                    {
                        if (eventTeachDataGrid.dataGridView.Rows[e.RowIndex].Cells[2].Value.ToString() == 1.ToString() ||
                            eventTeachDataGrid.dataGridView.Rows[e.RowIndex].Cells[2].Value.ToString() == 0.ToString())
                        {
                            code = string.Format("Set:[{0}={1}]//(未定义名称)",
                                eventTeachDataGrid.dataGridView.Rows[e.RowIndex].Cells[1].FormattedValue, eventTeachDataGrid.dataGridView.Rows[e.RowIndex].Cells[2].Value);
                        }
                    }
                }
                else if (eventTeachDataGrid.dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString() == "Wait")
                {
                    // Wait:[DI1=1]//(等待夹爪信号))
                    if (eventTeachDataGrid.dataGridView.Rows[e.RowIndex].Cells[2].Value != null)
                    {
                        if (eventTeachDataGrid.dataGridView.Rows[e.RowIndex].Cells[2].Value.ToString() == 1.ToString() ||
                            eventTeachDataGrid.dataGridView.Rows[e.RowIndex].Cells[2].Value.ToString() == 0.ToString())
                        {
                            code = string.Format("Wait:[{0}={1}]//(未定义名称)",
                                eventTeachDataGrid.dataGridView.Rows[e.RowIndex].Cells[1].FormattedValue, eventTeachDataGrid.dataGridView.Rows[e.RowIndex].Cells[2].Value);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(code))
                {
                    if (dgv.Columns[e.ColumnIndex].Name == "添加到末尾")
                    {
                        var newRow = dataGridViewProgramJog.Rows.Add();
                        var row = dataGridViewProgramJog.Rows[newRow];
                        row.Cells[1].Value = code;
                        UpdateIndex();
                    }
                    else if (dgv.Columns[e.ColumnIndex].Name == "插入到上一行")
                    {
                        if (dataGridViewProgramJog.CurrentRow != null)
                        {
                            if (_dataGridViewProgramJogCurrentRowIndex != -1)
                            {
                                dataGridViewProgramJog.Rows.Insert(_dataGridViewProgramJogCurrentRowIndex, 1);
                                var row = dataGridViewProgramJog.Rows[_dataGridViewProgramJogCurrentRowIndex];
                                row.Cells[1].Value = code;
                                UpdateIndex();
                                _dataGridViewProgramJogCurrentRowIndex++;
                            }
                        }
                    }
                    else if (dgv.Columns[e.ColumnIndex].Name == "更新当前行")
                    {
                        if (dataGridViewProgramJog.CurrentRow != null)
                        {
                            if (_dataGridViewProgramJogCurrentRowIndex != -1)
                            {
                                var row = dataGridViewProgramJog.Rows[_dataGridViewProgramJogCurrentRowIndex];
                                row.Cells[1].Value = code;
                                UpdateIndex();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddLast_BtnClick(object sender, EventArgs e)
        {
            var newRow = dataGridViewProgramJog.Rows.Add();
            var row = dataGridViewProgramJog.Rows[newRow];
            row.Cells[1].Value = GetMoveBlock();
            UpdateIndex();
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInsert_BtnClick(object sender, EventArgs e)
        {
            if (dataGridViewProgramJog.CurrentRow != null)
            {
                if (_dataGridViewProgramJogCurrentRowIndex != -1)
                {
                    dataGridViewProgramJog.Rows.Insert(_dataGridViewProgramJogCurrentRowIndex, 1);
                    var row = dataGridViewProgramJog.Rows[_dataGridViewProgramJogCurrentRowIndex];
                    row.Cells[1].Value = GetMoveBlock();
                    UpdateIndex();
                    _dataGridViewProgramJogCurrentRowIndex++;
                }
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_BtnClick(object sender, EventArgs e)
        {
            if (dataGridViewProgramJog.CurrentRow != null)
            {
                if (_dataGridViewProgramJogCurrentRowIndex != -1)
                {
                    if (!ShowBox())
                        return;

                    var row = dataGridViewProgramJog.Rows[_dataGridViewProgramJogCurrentRowIndex];
                    row.Cells[1].Value = GetMoveBlock();
                    UpdateIndex();
                }
            }
        }

        private void btnServo_BtnClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(labelTextJogProgramName.Text))
                return;
            using (var frm = new RobotControllerServoControl(_robotController, labelTextJogProgramName.Text))
                frm.ShowDialog();
        }

        private void btnUpdatePallet_BtnClick(object sender, EventArgs e)
        {
            using (var frm = new RobotFormPalletManagement(_robotController, GetMoveBlock()))
                frm.ShowDialog();
        }

        private void btnAddPallet_BtnClick(object sender, EventArgs e)
        {
            using (var frm = new RobotFormPalletManagement(_robotController))
            {
                if (frm.ShowDialog() != DialogResult.OK)
                    return;

                var str = string.Format("Pallet:[PalletName=N{0};PalletIndex={0}]//(码垛N{0})",
                    RobotFormPalletManagement.SelectedPalletIndex);

                if (_dataGridViewProgramJogCurrentRowIndex != -1)
                {
                    dataGridViewProgramJog.Rows.Insert(_dataGridViewProgramJogCurrentRowIndex, 1);
                    var row = dataGridViewProgramJog.Rows[_dataGridViewProgramJogCurrentRowIndex];
                    row.Cells[1].Value = str;
                    UpdateIndex();
                    _dataGridViewProgramJogCurrentRowIndex++;
                }
                else
                {
                    var newRow = dataGridViewProgramJog.Rows.Add();
                    var row = dataGridViewProgramJog.Rows[newRow];
                    row.Cells[1].Value = str;
                    UpdateIndex();
                }
            }
        }

        private void btnMgrPallet_BtnClick(object sender, EventArgs e)
        {
            toolStripButtonOpenPalletMgr_Click(null, null);
        }

        private void toolStripButtonDeleteRow_Click(object sender, EventArgs e)
        {
            if (dataGridViewProgramJog.CurrentRow != null)
            {
                if (_dataGridViewProgramJogCurrentRowIndex != -1)
                {
                    if (!ShowBox())
                        return;

                    dataGridViewProgramJog.Rows.RemoveAt(_dataGridViewProgramJogCurrentRowIndex);
                    UpdateIndex();
                    _dataGridViewProgramJogCurrentRowIndex = -1;
                }
            }
        }

        private string GetMoveBlock()
        {
            // Move:[Axis=2;Pos=148.2;Speed=100][Axis=3;Pos=12.2;Speed=100]//(运动到悬停位置)

            jogTeachDataGrid.dataGridView.EndEdit();

            var str = string.Empty;

            for (var i = 0; i < jogTeachDataGrid.dataGridView.RowCount; i++)
            {
                var row = jogTeachDataGrid.dataGridView.Rows[i];

                var currentPos = float.Parse(row.Cells[2].Value.ToString());
                var speed = float.Parse(row.Cells[3].FormattedValue == null ? 0.ToString() : row.Cells[3].FormattedValue.ToString());
                var axisNo = int.Parse(row.Cells[0].Value.ToString());

                str += string.Format("[Axis={0};Pos={1};Speed={2}]", axisNo, currentPos, speed);
            }

            return string.Format("Move:{0}//(未定义)", str);
        }

        public void UpdateIndex()
        {
            for (var i = 0; i < dataGridViewProgramJog.RowCount; i++)
            {
                dataGridViewProgramJog.Rows[i].Cells[0].Value = i.ToString();
            }
        }

        private void ucBtnIo_BtnClick(object sender, EventArgs e)
        {
            using (IoForm = new Form { Width = 600, Height = 600, StartPosition = FormStartPosition.CenterScreen, Text = @"DI&DO" })
            {
                IoForm.MaximizeBox = false;
                IoForm.Icon = FontImages.GetIcon(FontIcons.E_icon_tools, 32,
                    Color.DodgerBlue);

                var table = new TableLayoutPanel { Dock = DockStyle.Fill };
                for (var i = 0; i < 2; i++)
                    table.ColumnStyles.Add(
                        new ColumnStyle(SizeType.Percent, 50));

                var diPanel = new TableLayoutPanel { Dock = DockStyle.Fill };
                for (var i = 0; i < 2; i++)
                    diPanel.ColumnStyles.Add(
                        new ColumnStyle(SizeType.Percent, 50));
                for (var i = 0; i < 6; i++)
                    diPanel.RowStyles.Add(
                        new RowStyle(SizeType.Percent, 100 / 6));

                for (var i = 0; i < 12; i++)
                {
                    var btn = new Button
                    {
                        Enabled = false,
                        Dock = DockStyle.Fill,
                        BackColor = Color.DarkGoldenrod,
                        Text = string.Format("Di" + i),
                        Font = new Font("微软雅黑", 12, FontStyle.Regular),
                        Margin = new Padding(1, 1, 1, 1)
                    };

                    var col = i % 2;
                    var row = i / 2;

                    diPanel.Controls.Add(btn, col, row);
                }

                var doPanel = new TableLayoutPanel { Dock = DockStyle.Fill };
                for (var i = 0; i < 2; i++)
                    doPanel.ColumnStyles.Add(
                        new ColumnStyle(SizeType.Percent, 50));
                for (var i = 0; i < 4; i++)
                    doPanel.RowStyles.Add(
                        new RowStyle(SizeType.Percent, 100 / 4));

                for (var i = 0; i < 8; i++)
                {
                    var btn = new Button
                    {
                        Dock = DockStyle.Fill,
                        BackColor = Color.DarkGray,
                        Text = string.Format("Do" + i),
                        Font = new Font("微软雅黑", 12, FontStyle.Regular)
                    };
                    btn.Click += doBtn_Click;

                    var col = i % 2;
                    var row = i / 2;

                    doPanel.Controls.Add(btn, col, row);
                }

                table.Controls.Add(diPanel, 0, 0);
                table.Controls.Add(doPanel, 1, 0);
                IoForm.Controls.Add(table);

                if (InputMonitorTh != null)
                {
                    InputMonitorTh.Abort();
                    InputMonitorTh.Join();
                }
                InputMonitorTh = new Thread(InputMonitor) { IsBackground = true };
                InputMonitorTh.Start();

                IoForm.ShowDialog();
            }

            if (InputMonitorTh == null) return;
            InputMonitorTh.Abort();
            InputMonitorTh.Join();
        }

        private void doBtn_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn == null) return;
            var str = btn.Text;

            var fi = _robotController.GetType().GetField(str);
            if (fi == null) return;
            var fiValue = fi.GetValue(_robotController);
            if (fiValue != null)
                fi.SetValue(_robotController, fiValue.ToString() == 1.ToString() ? 0.ToString() : 1.ToString());
        }

        private Thread InputMonitorTh { get; set; }
        private Form IoForm { get; set; }
        private void InputMonitor()
        {
            while (InputMonitorTh.IsAlive)
            {
                if (!InputMonitorTh.IsAlive)
                    break;

                Thread.Sleep(50);

                for (var i = 0; i < 12; i++)
                {
                    var fi = _robotController.GetType().GetField(string.Format("Di{0}", i));

                    foreach (var t in IoForm.Controls)
                    {
                        if (t.GetType() != typeof(TableLayoutPanel)) continue;
                        var table1 = t as TableLayoutPanel;
                        if (table1 == null) continue;
                        foreach (var tt in table1.Controls)
                        {
                            if (tt.GetType() != typeof(TableLayoutPanel)) continue;
                            var table2 = tt as TableLayoutPanel;
                            if (table2 == null) continue;
                            foreach (var b in table2.Controls)
                            {
                                if (b.GetType() != typeof(Button)) continue;
                                var btn = b as Button;

                                if (btn == null || btn.Text != string.Format("Di{0}", i)) continue;
                                if (fi == null) continue;
                                var fiValue = fi.GetValue(_robotController);
                                if (fiValue == null) continue;
                                if (fiValue.ToString() == 1.ToString())
                                    btn.BackColor = Color.DarkGreen;
                                else if (fiValue.ToString() == 0.ToString())
                                    btn.BackColor = Color.DarkGoldenrod;
                            }
                        }
                    }
                }

                for (var i = 0; i < 8; i++)
                {
                    var fi = _robotController.GetType().GetField(string.Format("Do{0}", i));

                    foreach (var t in IoForm.Controls)
                    {
                        if (t.GetType() != typeof(TableLayoutPanel)) continue;
                        var table1 = t as TableLayoutPanel;
                        if (table1 == null) continue;
                        foreach (var tt in table1.Controls)
                        {
                            if (tt.GetType() != typeof(TableLayoutPanel)) continue;
                            var table2 = tt as TableLayoutPanel;
                            if (table2 == null) continue;
                            foreach (var b in table2.Controls)
                            {
                                if (b.GetType() != typeof(Button)) continue;
                                var btn = b as Button;

                                if (btn == null || btn.Text != string.Format("Do{0}", i)) continue;
                                if (fi == null) continue;
                                var fiValue = fi.GetValue(_robotController);
                                if (fiValue == null) continue;
                                if (fiValue.ToString() == 1.ToString())
                                    btn.BackColor = Color.Green;
                                else if (fiValue.ToString() == 0.ToString())
                                    btn.BackColor = Color.Goldenrod;
                            }
                        }
                    }
                }
            }
        }

        private void toolStripButtonSaveProgram_Click(object sender, EventArgs e)
        {
            var pmName = labelTextJogProgramName.textBox.Text;
            if (!string.IsNullOrEmpty(pmName))
            {
                if (!ShowBox())
                    return;

                var tempList = _robotController.RobotJogging.RobotPrograms.ToList();
                var pm = tempList.Find(f => f.Name == pmName);
                if (pm == null)
                    return;
                pm.Blocks = new string[dataGridViewProgramJog.RowCount];
                for (var i = 0; i < dataGridViewProgramJog.RowCount; i++)
                    pm.Blocks[i] = dataGridViewProgramJog.Rows[i].Cells[1].Value.ToString();
                _robotController.RobotJogging.RobotPrograms = tempList.ToArray();
                XmlHelper.SerializeToFile(_robotController.RobotJogging, RobotControllerPpMode.ConfilePath, Encoding.UTF8);
            }
        }

        #endregion

        private static void RowsAdd(
           DataGridView dgv, IReadOnlyList<string> values, Color backColor = default(Color))
        {
            try
            {
                var rowIndex = dgv.Rows.Add();
                dgv.Rows[rowIndex].DefaultCellStyle.BackColor = backColor;

                for (var i = 0; i < values.Count; i++)
                    dgv.Rows[rowIndex].Cells[i].Value = values[i];
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void toolStripProgram_Click(object sender, EventArgs e)
        {
            var form = new RobotControllerProgramAdd(_robotController.RobotJogging);
            if (form.ShowDialog() == DialogResult.OK)
            {
                dataGridViewProgramList.Rows.Clear();
                for (var i = 0; i < _robotController.RobotJogging.RobotPrograms.Length; i++)
                {
                    var t = _robotController.RobotJogging.RobotPrograms[i];
                    RowsAdd(dataGridViewProgramList, new[] { (i + 1).ToString(), t.Name, t.Note });
                }
            }

            dataGridViewProgramList.ClearSelection();
        }

        private void toolStripButtonProgramDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewProgramList.SelectedRows.Count != 1)
                return;

            if (dataGridViewProgramList.RowCount < 1)
                return;

            if (!ShowBox())
                return;

            var rowIndex = dataGridViewProgramList.SelectedRows[0].Index;

            if (dataGridViewProgramList.Rows[rowIndex].Cells[1].Value != null)
            {
                var programName = dataGridViewProgramList.Rows[rowIndex].Cells[1].Value.ToString();
                if (!string.IsNullOrEmpty(programName) && _robotController.RobotJogging.RobotPrograms != null)
                {
                    var temp = _robotController.RobotJogging.RobotPrograms.ToList();
                    temp.RemoveAll(f => f.Name == programName);

                    _robotController.RobotJogging.RobotPrograms = temp.ToArray();

                    var robotJoggingFilePath = RobotControllerPpMode.ConfilePath;
                    //Directory.GetCurrentDirectory() + @"\ControllerConfig\RobotJogging.xml";
                    if (File.Exists(robotJoggingFilePath))
                        File.Delete(robotJoggingFilePath);
                    XmlHelper.SerializeToFile(_robotController.RobotJogging, robotJoggingFilePath, Encoding.UTF8);
                }
            }

            dataGridViewProgramList.Rows.Clear();
            dataGridViewSelectedProgram.Rows.Clear();

            if (_robotController.RobotJogging.RobotPrograms != null)
            {
                for (var i = 0; i < _robotController.RobotJogging.RobotPrograms.Length; i++)
                {
                    var rowAdd = dataGridViewProgramList.Rows[dataGridViewProgramList.Rows.Add()];
                    rowAdd.Cells[0].Value = (i + 1).ToString();
                    rowAdd.Cells[1].Value = _robotController.RobotJogging.RobotPrograms[i].Name;
                    rowAdd.Cells[2].Value = _robotController.RobotJogging.RobotPrograms[i].Note;
                }
            }
        }

        private async void toolStripButtonRunProgram_Click(object sender, EventArgs e)
        {
            if (dataGridViewProgramJog.RowCount <= 0)
                return;

            if (!ShowBox())
                return;

            toolStripButtonRunProgram.Enabled = false;
            toolStripButtonMoveSelectedBlockLine.Enabled = false;
            dataGridViewProgramJog.Enabled = false;
            tableLayoutPanel1.Enabled = false;
            toolStripButtonDeleteRow.Enabled = false;
            toolStripButtonUpdate.Enabled = false;
            toolStripButtonPauseProgram.Enabled = true;
            _dataGridViewProgramJogCurrentRowIndex = -1;
            _robotController.RobotRunProgram(labelTextJogProgramName.Text, 0);

            await Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        var i =
                            _robotController.CurrentRunProgeam[labelTextJogProgramName.Text];
                        _dataGridViewProgramJogCurrentRowIndex = i;

                        if (i == -1)
                            break;

                        if (dataGridViewProgramJog.Rows[_dataGridViewProgramJogCurrentRowIndex].Cells[1].Value != null &&
                            !string.IsNullOrEmpty(dataGridViewProgramJog.Rows[_dataGridViewProgramJogCurrentRowIndex].Cells[1].Value.ToString()))
                        {
                            var block =
                                dataGridViewProgramJog.Rows[_dataGridViewProgramJogCurrentRowIndex].Cells[1].Value
                                    .ToString();

                            if (InvokeRequired)
                            {
                                Invoke(new Action(() =>
                                {
                                    if (block.StartsWith("Wait"))
                                        toolStripButtonSkip.Enabled = true;
                                    else
                                        toolStripButtonSkip.Enabled = false;
                                }));
                            }
                        }
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }
            });

            _dataGridViewProgramJogCurrentRowIndex = -1;
            dataGridViewProgramJog.Enabled = true;
            toolStripButtonRunProgram.Enabled = true;
            toolStripButtonMoveSelectedBlockLine.Enabled = true;
            tableLayoutPanel1.Enabled = true;
            toolStripButtonDeleteRow.Enabled = true;
            toolStripButtonUpdate.Enabled = true;
            toolStripButtonPauseProgram.Enabled = false;
            toolStripButtonSkip.Enabled = false;
            toolStripButtonPauseProgram.Text = @"暂停";
            toolStripButtonPauseProgram.Image = FontImages.GetImage(FontIcons.A_fa_hand_stop_o, 32,
                Color.DodgerBlue);
            _jogBlockSelectDefaultColor = Color.Aquamarine;
        }

        public static bool ShowBox(string msg = "")
        {
            var showMsg = string.IsNullOrEmpty(msg) ? "你确定吗？？？" : msg;

            var result =
               MessageBox.Show(@showMsg, @"操作前确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            return result == DialogResult.Yes;
        }

        private void toolStripButtonOpenPalletMgr_Click(object sender, EventArgs e)
        {
            using (var frm = new RobotFormPalletManagement(_robotController))
            {
                frm.ShowDialog();
            }
        }

        /// <summary>
        /// 暂停或者继续
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonPauseProgram_Click(object sender, EventArgs e)
        {
            if (toolStripButtonPauseProgram.Text == @"暂停")
            {
                toolStripButtonPauseProgram.Text = @"继续";
                toolStripButtonPauseProgram.Image = FontImages.GetImage(FontIcons.A_fa_handshake_o, 32,
                    Color.DodgerBlue);
                _jogBlockSelectDefaultColor = Color.BurlyWood;
            }
            else if (toolStripButtonPauseProgram.Text == @"继续")
            {
                toolStripButtonPauseProgram.Text = @"暂停";
                toolStripButtonPauseProgram.Image = FontImages.GetImage(FontIcons.A_fa_hand_stop_o, 32,
                    Color.DodgerBlue);
                _jogBlockSelectDefaultColor = Color.Aquamarine;
            }
        }

        /// <summary>
        /// 运行到下一行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void toolStripButtonNextRow_Click(object sender, EventArgs e)
        {
            //if (toolStripButtonPauseProgram.Enabled && toolStripButtonPauseProgram.Text == @"暂停")
            //{

            //}

            if (_dataGridViewProgramJogCurrentRowIndex > -1 && _dataGridViewProgramJogCurrentRowIndex != dataGridViewProgramJog.RowCount - 1)
            {
                toolStripButtonMoveSelectedBlockLine.Enabled = false;
                toolStripButtonNextRow.Enabled = false;
                toolStripButtonPreRow.Enabled = false;
                var target = _dataGridViewProgramJogCurrentRowIndex + 1;
                _dataGridViewProgramJogCurrentRowIndex = -1;
                _robotController.RobotRunProgram(labelTextJogProgramName.Text, target, target);

                await Task.Run(() =>
                {
                    while (true)
                    {
                        try
                        {
                            var i =
                                 _robotController.CurrentRunProgeam[labelTextJogProgramName.Text];

                            if (i == -1)
                            {
                                _dataGridViewProgramJogCurrentRowIndex = target;
                                toolStripButtonSkip.Enabled = false;
                                break;
                            }

                            _dataGridViewProgramJogCurrentRowIndex = i;

                            if (dataGridViewProgramJog.Rows[_dataGridViewProgramJogCurrentRowIndex].Cells[1].Value != null &&
                                !string.IsNullOrEmpty(dataGridViewProgramJog.Rows[_dataGridViewProgramJogCurrentRowIndex].Cells[1].Value.ToString()))
                            {
                                var block =
                                    dataGridViewProgramJog.Rows[_dataGridViewProgramJogCurrentRowIndex].Cells[1].Value
                                        .ToString();

                                if (InvokeRequired)
                                {
                                    Invoke(new Action(() =>
                                    {
                                        if (block.StartsWith("Wait"))
                                            toolStripButtonSkip.Enabled = true;
                                        else
                                            toolStripButtonSkip.Enabled = false;
                                    }));
                                }
                            }
                        }
                        catch (Exception)
                        {
                            break;
                        }
                    }
                });

                toolStripButtonMoveSelectedBlockLine.Enabled = true;
                toolStripButtonNextRow.Enabled = true;
                toolStripButtonPreRow.Enabled = true;
            }
        }

        /// <summary>
        /// 运行到指定行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void toolStripButtonPreRow_Click(object sender, EventArgs e)
        {
            //if (toolStripButtonPauseProgram.Enabled && toolStripButtonPauseProgram.Text == @"暂停")
            //{

            //}

            if (_dataGridViewProgramJogCurrentRowIndex > 0)
            {
                toolStripButtonMoveSelectedBlockLine.Enabled = false;
                toolStripButtonNextRow.Enabled = false;
                toolStripButtonPreRow.Enabled = false;
                var target = _dataGridViewProgramJogCurrentRowIndex - 1;
                _dataGridViewProgramJogCurrentRowIndex = -1;
                _robotController.RobotRunProgram(labelTextJogProgramName.Text, target, target);

                await Task.Run(() =>
                {
                    while (true)
                    {
                        try
                        {
                            var i =
                                 _robotController.CurrentRunProgeam[labelTextJogProgramName.Text];

                            if (i == -1)
                            {
                                _dataGridViewProgramJogCurrentRowIndex = target;
                                toolStripButtonSkip.Enabled = false;
                                break;
                            }

                            _dataGridViewProgramJogCurrentRowIndex = i;

                            if (dataGridViewProgramJog.Rows[_dataGridViewProgramJogCurrentRowIndex].Cells[1].Value != null &&
                                !string.IsNullOrEmpty(dataGridViewProgramJog.Rows[_dataGridViewProgramJogCurrentRowIndex].Cells[1].Value.ToString()))
                            {
                                var block =
                                    dataGridViewProgramJog.Rows[_dataGridViewProgramJogCurrentRowIndex].Cells[1].Value
                                        .ToString();

                                if (InvokeRequired)
                                {
                                    Invoke(new Action(() =>
                                    {
                                        if (block.StartsWith("Wait"))
                                            toolStripButtonSkip.Enabled = true;
                                        else
                                            toolStripButtonSkip.Enabled = false;
                                    }));
                                }
                            }
                        }
                        catch (Exception)
                        {
                            break;
                        }
                    }
                });

                toolStripButtonMoveSelectedBlockLine.Enabled = true;
                toolStripButtonNextRow.Enabled = true;
                toolStripButtonPreRow.Enabled = true;
            }
        }

        /// <summary>
        /// 跳过当前行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonSkip_Click(object sender, EventArgs e)
        {
            _robotController.IsBreak = true;
        }

        /// <summary>
        /// 运行到指定行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void toolStripButtonMoveSelectedBlockLine_Click(object sender, EventArgs e)
        {
            if (toolStripButtonMoveSelectedBlockLine.Text == @"结束")
            {
                toolStripButtonMoveSelectedBlockLine.Text = @"运行到指定行";
            }
            else
            {
                if (dataGridViewProgramJog.RowCount <= 0 || _dataGridViewProgramJogCurrentRowIndex == -1)
                    return;

                if (!ShowBox())
                    return;

                toolStripButtonRunProgram.Enabled = false;
                toolStripButtonMoveSelectedBlockLine.Enabled = false;
                dataGridViewProgramJog.Enabled = false;
                tableLayoutPanel1.Enabled = false;
                toolStripButtonDeleteRow.Enabled = false;
                toolStripButtonUpdate.Enabled = false;
                toolStripButtonPauseProgram.Enabled = true;
                var target = _dataGridViewProgramJogCurrentRowIndex;
                _dataGridViewProgramJogCurrentRowIndex = -1;
                _robotController.RobotRunProgram(labelTextJogProgramName.Text, 0, target);

                await Task.Run(() =>
                {
                    while (true)
                    {
                        try
                        {
                            var i =
                                 _robotController.CurrentRunProgeam[labelTextJogProgramName.Text];

                            if (i == -1)
                            {
                                _dataGridViewProgramJogCurrentRowIndex = target;

                                if (InvokeRequired)
                                {
                                    Invoke(new Action(() =>
                                    {
                                        toolStripButtonMoveSelectedBlockLine.Enabled = true;
                                        toolStripButtonMoveSelectedBlockLine.Text = @"结束";

                                        toolStripButtonPreRow.Enabled = true;
                                        toolStripButtonNextRow.Enabled = true;
                                        toolStripButtonSkip.Enabled = false;
                                    }));
                                }

                                while (true)
                                {
                                    if (toolStripButtonMoveSelectedBlockLine.Text == @"运行到指定行")
                                        break;
                                }
                                break;
                            }

                            _dataGridViewProgramJogCurrentRowIndex = i;

                            if (dataGridViewProgramJog.Rows[_dataGridViewProgramJogCurrentRowIndex].Cells[1].Value != null &&
                                !string.IsNullOrEmpty(dataGridViewProgramJog.Rows[_dataGridViewProgramJogCurrentRowIndex].Cells[1].Value.ToString()))
                            {
                                var block =
                                    dataGridViewProgramJog.Rows[_dataGridViewProgramJogCurrentRowIndex].Cells[1].Value
                                        .ToString();

                                if (InvokeRequired)
                                {
                                    Invoke(new Action(() =>
                                    {
                                        if (block.StartsWith("Wait"))
                                            toolStripButtonSkip.Enabled = true;
                                        else
                                            toolStripButtonSkip.Enabled = false;
                                    }));
                                }
                            }
                        }
                        catch (Exception)
                        {
                            break;
                        }
                    }
                });

                _dataGridViewProgramJogCurrentRowIndex = -1;
                dataGridViewProgramJog.Enabled = true;
                toolStripButtonRunProgram.Enabled = true;
                toolStripButtonMoveSelectedBlockLine.Enabled = true;
                tableLayoutPanel1.Enabled = true;
                toolStripButtonDeleteRow.Enabled = true;
                toolStripButtonUpdate.Enabled = true;
                toolStripButtonPauseProgram.Enabled = false;
                toolStripButtonPreRow.Enabled = false;
                toolStripButtonNextRow.Enabled = false;
                toolStripButtonSkip.Enabled = false;
                toolStripButtonPauseProgram.Text = @"暂停";
                toolStripButtonPauseProgram.Image = FontImages.GetImage(FontIcons.A_fa_hand_stop_o, 32,
                    Color.DodgerBlue);
                _jogBlockSelectDefaultColor = Color.Aquamarine;
            }
        }
    }
}
