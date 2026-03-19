using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CommonUtility;
using HZH_Controls.IconFont;

namespace CheckSystem.RobotForms
{
    public partial class RobotControllerProgramUpdate : Form
    {
        private RobotJoggingRobotProgram RobotJoggingRobotProgram { get; set; }

        public string UpdatedCode = string.Empty;

        public RobotControllerProgramUpdate(
            RobotJoggingRobotProgram robotJoggingRobotProgram, string block)
        {
            InitializeComponent();
            RobotJoggingRobotProgram = robotJoggingRobotProgram;

            if (block.StartsWith("Move:"))
            {
                userDataGrid1.label.Text = @"Move";

                userDataGrid1.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "轴号" });
                userDataGrid1.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "轴名称" });
                userDataGrid1.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "目标位置" });
                userDataGrid1.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "运行速度" });
                userDataGrid1.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "提前到位偏移量" });

                userDataGrid1.dataGridView.AllowUserToAddRows = false;
                userDataGrid1.dataGridView.AllowUserToDeleteRows = false;
                userDataGrid1.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                userDataGrid1.dataGridView.AllowUserToResizeColumns = true;
                userDataGrid1.dataGridView.AllowUserToResizeRows = false;
                userDataGrid1.dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                userDataGrid1.dataGridView.BackgroundColor = SystemColors.ActiveCaption;
                //userDataGrid1.dataGridView.ReadOnly = true;
                userDataGrid1.dataGridView.RowHeadersVisible = false;
                for (var i = 0; i < userDataGrid1.dataGridView.Columns.Count; i++)
                {
                    userDataGrid1.dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    userDataGrid1.dataGridView.Columns[i].ReadOnly = true;
                }
                userDataGrid1.dataGridView.Columns[2].ReadOnly =
                    userDataGrid1.dataGridView.Columns[3].ReadOnly =
                        userDataGrid1.dataGridView.Columns[4].ReadOnly = false;

                var point = block.Replace("Move:", string.Empty)
                           .Split(new[] { "[", "]" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var p in point)
                {
                    if (!p.StartsWith("Axis="))
                        continue;

                    var row = userDataGrid1.dataGridView.Rows[userDataGrid1.dataGridView.Rows.Add()];

                    var index = 0;
                    var pos = 0.0;
                    var speed = 0.0;
                    var offset = 0.0;
                    var axixName = string.Empty;

                    foreach (var a in p.Split(';'))
                    {
                        if (a.StartsWith("Axis="))
                        {
                            index = int.Parse(a.Replace("Axis=", string.Empty));
                            if (robotJoggingRobotProgram == null || robotJoggingRobotProgram.AxisList == null)
                                continue;
                            var findAxix =
                                robotJoggingRobotProgram.AxisList.ToList()
                                    .Find(f => f.AxisNo != null && f.AxisNo == index.ToString());
                            if (findAxix != null && findAxix.AxisNo != null)
                                axixName = findAxix.AxisNote;
                        }
                        else if (a.StartsWith("Pos="))
                            pos = float.Parse(a.Replace("Pos=", string.Empty));
                        else if (a.StartsWith("Speed="))
                            speed = float.Parse(a.Replace("Speed=", string.Empty));
                        else if (a.StartsWith("Offset="))
                            offset = float.Parse(a.Replace("Offset=", string.Empty));
                    }

                    row.Cells[0].Value = index;
                    row.Cells[1].Value = axixName;
                    row.Cells[2].Value = pos;
                    row.Cells[3].Value = speed;
                    row.Cells[4].Value = offset;
                }
            }
            else if (block.StartsWith("Set:"))
            {
                userDataGrid1.label.Text = @"Set";

                userDataGrid1.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "信号名称" });
                userDataGrid1.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "信号值" });

                userDataGrid1.dataGridView.AllowUserToAddRows = false;
                userDataGrid1.dataGridView.AllowUserToDeleteRows = false;
                userDataGrid1.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                userDataGrid1.dataGridView.AllowUserToResizeColumns = true;
                userDataGrid1.dataGridView.AllowUserToResizeRows = false;
                userDataGrid1.dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                userDataGrid1.dataGridView.BackgroundColor = SystemColors.ActiveCaption;
                //userDataGrid1.dataGridView.ReadOnly = true;
                userDataGrid1.dataGridView.RowHeadersVisible = false;
                for (var i = 0; i < userDataGrid1.dataGridView.Columns.Count; i++)
                {
                    userDataGrid1.dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    userDataGrid1.dataGridView.Columns[i].ReadOnly = true;
                }
                userDataGrid1.dataGridView.Columns[1].ReadOnly = false;

                var set = block.Replace("Set:", string.Empty)
                            .Split(new[] { "[", "]" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var p in set)
                {
                    for (var i = 0; i < 8; i++)
                    {
                        var str = string.Format("Do{0}", i);

                        if (!p.StartsWith(str + "="))
                            continue;

                        var targetValue = int.Parse(p.Replace(str + "=", string.Empty));

                        var row = userDataGrid1.dataGridView.Rows[userDataGrid1.dataGridView.Rows.Add()];

                        row.Cells[0].Value = str;
                        row.Cells[1].Value = targetValue;
                        break;
                    }

                    for (var i = 0; i < 21; i++)
                    {
                        var str = string.Format("So{0}", i);

                        if (!p.StartsWith(str + "="))
                            continue;

                        var targetValue = int.Parse(p.Replace(str + "=", string.Empty));

                        var row = userDataGrid1.dataGridView.Rows[userDataGrid1.dataGridView.Rows.Add()];

                        row.Cells[0].Value = str;
                        row.Cells[1].Value = targetValue;
                        break;
                    }
                }
            }
            else if (block.StartsWith("Wait:"))
            {
                userDataGrid1.label.Text = @"Wait";

                userDataGrid1.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "信号名称" });
                userDataGrid1.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "信号值" });

                userDataGrid1.dataGridView.AllowUserToAddRows = false;
                userDataGrid1.dataGridView.AllowUserToDeleteRows = false;
                userDataGrid1.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                userDataGrid1.dataGridView.AllowUserToResizeColumns = true;
                userDataGrid1.dataGridView.AllowUserToResizeRows = false;
                userDataGrid1.dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                userDataGrid1.dataGridView.BackgroundColor = SystemColors.ActiveCaption;
                //userDataGrid1.dataGridView.ReadOnly = true;
                userDataGrid1.dataGridView.RowHeadersVisible = false;
                for (var i = 0; i < userDataGrid1.dataGridView.Columns.Count; i++)
                {
                    userDataGrid1.dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    userDataGrid1.dataGridView.Columns[i].ReadOnly = true;
                }
                userDataGrid1.dataGridView.Columns[1].ReadOnly = false;

                var wait = block.Replace("Wait:", string.Empty)
                          .Split(new[] { "[", "]" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var p in wait)
                {
                    for (var i = 0; i < 12; i++)
                    {
                        var str = string.Format("Di{0}", i);

                        if (!p.StartsWith(str + "="))
                            continue;

                        var targetValue = int.Parse(p.Replace(str + "=", string.Empty));

                        var row = userDataGrid1.dataGridView.Rows[userDataGrid1.dataGridView.Rows.Add()];

                        row.Cells[0].Value = str;
                        row.Cells[1].Value = targetValue;
                    }

                    for (var i = 0; i < 21; i++)
                    {
                        var str = string.Format("Si{0}", i);

                        if (!p.StartsWith(str + "="))
                            continue;

                        var targetValue = int.Parse(p.Replace(str + "=", string.Empty));

                        var row = userDataGrid1.dataGridView.Rows[userDataGrid1.dataGridView.Rows.Add()];

                        row.Cells[0].Value = str;
                        row.Cells[1].Value = targetValue;
                    }
                }
            }
            else if (block.StartsWith("Delay:"))
            {
                userDataGrid1.label.Text = @"Delay";

                userDataGrid1.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "信号名称" });
                userDataGrid1.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "信号值" });

                userDataGrid1.dataGridView.AllowUserToAddRows = false;
                userDataGrid1.dataGridView.AllowUserToDeleteRows = false;
                userDataGrid1.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                userDataGrid1.dataGridView.AllowUserToResizeColumns = true;
                userDataGrid1.dataGridView.AllowUserToResizeRows = false;
                userDataGrid1.dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                userDataGrid1.dataGridView.BackgroundColor = SystemColors.ActiveCaption;
                //userDataGrid1.dataGridView.ReadOnly = true;
                userDataGrid1.dataGridView.RowHeadersVisible = false;
                for (var i = 0; i < userDataGrid1.dataGridView.Columns.Count; i++)
                {
                    userDataGrid1.dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    userDataGrid1.dataGridView.Columns[i].ReadOnly = true;
                }
                userDataGrid1.dataGridView.Columns[1].ReadOnly = false;

                var delay = block.Replace("Delay:", string.Empty)
                           .Split(new[] { "[", "]" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var p in delay)
                {
                    if (!p.StartsWith("WaitTime="))
                        continue;

                    var targetMs = int.Parse(p.Replace("WaitTime=", string.Empty));
                    var row = userDataGrid1.dataGridView.Rows[userDataGrid1.dataGridView.Rows.Add()];

                    row.Cells[0].Value = "WaitTime";
                    row.Cells[1].Value = targetMs;
                }
            }

            userDataGrid1.dataGridView.EditingControlShowing += dataGridView_EditingControlShowing;
        }

        public RobotControllerProgramUpdate(string block)
        {
            InitializeComponent();

            if (block.StartsWith("Move:"))
            {
                userDataGrid1.label.Text = @"Move";

                userDataGrid1.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "轴号" });
                userDataGrid1.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "目标位置" });
                userDataGrid1.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "运行速度" });
                userDataGrid1.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "提前到位偏移量" });

                userDataGrid1.dataGridView.AllowUserToAddRows = false;
                userDataGrid1.dataGridView.AllowUserToDeleteRows = false;
                userDataGrid1.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                userDataGrid1.dataGridView.AllowUserToResizeColumns = true;
                userDataGrid1.dataGridView.AllowUserToResizeRows = false;
                userDataGrid1.dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                userDataGrid1.dataGridView.BackgroundColor = SystemColors.ActiveCaption;
                //userDataGrid1.dataGridView.ReadOnly = true;
                userDataGrid1.dataGridView.RowHeadersVisible = false;
                for (var i = 0; i < userDataGrid1.dataGridView.Columns.Count; i++)
                {
                    userDataGrid1.dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    userDataGrid1.dataGridView.Columns[i].ReadOnly = true;
                }

                userDataGrid1.dataGridView.Columns[1].ReadOnly = false;
                userDataGrid1.dataGridView.Columns[2].ReadOnly = false;
                userDataGrid1.dataGridView.Columns[3].ReadOnly = false;

                var point = block.Replace("Move:", string.Empty)
                           .Split(new[] { "[", "]" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var p in point)
                {
                    if (!p.StartsWith("Axis="))
                        continue;

                    var row = userDataGrid1.dataGridView.Rows[userDataGrid1.dataGridView.Rows.Add()];

                    var index = 0;
                    var pos = 0.0;
                    var speed = 0.0;
                    var offset = 0.0;
                    var axixName = string.Empty;

                    foreach (var a in p.Split(';'))
                    {
                        if (a.StartsWith("Axis="))
                        {
                            index = int.Parse(a.Replace("Axis=", string.Empty));
                        }
                        else if (a.StartsWith("Pos="))
                            pos = Math.Round(float.Parse(a.Replace("Pos=", string.Empty)), 2, MidpointRounding.AwayFromZero);
                        else if (a.StartsWith("Speed="))
                            speed = Math.Round(float.Parse(a.Replace("Speed=", string.Empty)), 2,
                                MidpointRounding.AwayFromZero);
                        else if (a.StartsWith("Offset="))
                            offset = Math.Round(float.Parse(a.Replace("Offset=", string.Empty)), 2,
                                MidpointRounding.AwayFromZero);
                    }

                    row.Cells[0].Value = index;
                    row.Cells[1].Value = pos;
                    row.Cells[2].Value = speed;
                    row.Cells[3].Value = offset;
                }
            }

            userDataGrid1.dataGridView.EditingControlShowing += dataGridView_EditingControlShowing;
        }

        private DataGridViewTextBoxEditingControl _cellEdit;

        private void dataGridView_EditingControlShowing(
            object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            _cellEdit = (DataGridViewTextBoxEditingControl)e.Control; // 赋值          
            _cellEdit.SelectAll();
            _cellEdit.KeyPress += Cells_KeyPress;
        }

        /// <summary>
        /// 绑定到事件
        /// 自定义事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cells_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (userDataGrid1.label.Text == @"Move")
            {
                if ((e.KeyChar < '0' && e.KeyChar != '.' && e.KeyChar != '-' || e.KeyChar > '9' && e.KeyChar != '.' && e.KeyChar != '-' || ((TextBox)sender).Text.IndexOf('.') >= 0 && e.KeyChar == '.' || ((TextBox)sender).Text.LastIndexOf('-') != -1 && e.KeyChar == '-') &&
                    e.KeyChar != (char)13 &&
                    e.KeyChar != (char)8)
                    e.Handled = true;
            }
            else if (userDataGrid1.label.Text == @"Wait" || userDataGrid1.label.Text == @"Set")
            {
                if (e.KeyChar != '1' && e.KeyChar != '0')
                    e.Handled = true;
            }
            else if (userDataGrid1.label.Text == @"Delay")
            {
                if (e.KeyChar < '0' || e.KeyChar > '9')
                    e.Handled = true;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void ucBtnExt1_BtnClick(object sender, EventArgs e)
        {
            try
            {
                userDataGrid1.dataGridView.EndEdit();

                if (userDataGrid1.label.Text == @"Wait" || userDataGrid1.label.Text == @"Set")
                    if (userDataGrid1.dataGridView.Rows[0].Cells[1].Value == null ||
                             (userDataGrid1.dataGridView.Rows[0].Cells[1].Value.ToString() != "1" &&
                             userDataGrid1.dataGridView.Rows[0].Cells[1].Value.ToString() != "0"))
                        return;

                if (!RobotControllerMainForm.ShowBox("请确认是否保存？"))
                    return;

                if (userDataGrid1.label.Text == @"Move")
                {
                    var str = string.Empty;

                    for (var i = 0; i < userDataGrid1.dataGridView.RowCount; i++)
                    {
                        var row = userDataGrid1.dataGridView.Rows[i];

                        var currentPos = 0f;
                        var speed = 0f;
                        var offset = 0f;
                        var axisNo = 0f;

                        for (var j = 0; j < row.Cells.Count; j++)
                        {
                            if (userDataGrid1.dataGridView.Columns[j].HeaderText == @"轴号")
                                axisNo = float.Parse(row.Cells[j].Value.ToString());
                            else if (userDataGrid1.dataGridView.Columns[j].HeaderText == @"目标位置")
                                currentPos = float.Parse(row.Cells[j].Value.ToString());
                            else if (userDataGrid1.dataGridView.Columns[j].HeaderText == @"运行速度")
                                speed = float.Parse(row.Cells[j].Value.ToString());
                            else if (userDataGrid1.dataGridView.Columns[j].HeaderText == @"提前到位偏移量")
                            {
                                if (row.Cells[j].Value == null || string.IsNullOrEmpty(row.Cells[j].Value.ToString()))
                                {
                                    offset = 0f;
                                }
                                else
                                {
                                    offset = float.Parse(row.Cells[j].Value.ToString());
                                }
                            }
                        }

                        if (Math.Abs(offset) <= 0)
                            str += string.Format("[Axis={0};Pos={1};Speed={2}]", axisNo, currentPos, speed);
                        else
                            str += string.Format("[Axis={0};Pos={1};Speed={2};Offset={3}]", axisNo, currentPos, speed,
                                   offset);
                    }

                    UpdatedCode = string.Format("Move:{0}//(未定义)", str);
                }
                else if (userDataGrid1.label.Text == @"Wait" || userDataGrid1.label.Text == @"Set")
                {
                    UpdatedCode = string.Format("{0}:[{1}={2}]//(未定义名称)",
                        userDataGrid1.label.Text,
                                    userDataGrid1.dataGridView.Rows[0].Cells[0].Value, userDataGrid1.dataGridView.Rows[0].Cells[1].Value);
                }
                else if (userDataGrid1.label.Text == @"Delay")
                {
                    UpdatedCode = string.Format("Delay:[{0}={1}]//(等待延时)",
                        "WaitTime", userDataGrid1.dataGridView.Rows[0].Cells[1].Value);
                }

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"保存失败：" + ex.Message);
            }
        }

        private void RobotControllerProgramUpdate_Load(object sender, EventArgs e)
        {
            Icon = FontImages.GetIcon(FontIcons.E_icon_like_alt, 32,
               Color.DodgerBlue);
        }
    }
}
