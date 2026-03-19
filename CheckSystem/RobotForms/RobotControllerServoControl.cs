using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Controller;
using HZH_Controls.IconFont;

namespace CheckSystem.RobotForms
{
    public partial class RobotControllerServoControl : Form
    {
        private RobotControllerPpMode RobotController { get; set; }

        public RobotControllerServoControl(RobotControllerPpMode robotController, string programName)
        {
            InitializeComponent();
            Icon = FontImages.GetIcon(FontIcons.E_icon_tools, 32,
               Color.DodgerBlue);
            RobotController = robotController;

            dgvServo.label.Text = @"伺服";
            dgvServo.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "轴号" });
            dgvServo.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "轴名称" });
            dgvServo.dataGridView.Columns.Add(new DataGridViewButtonColumn { Name = "使能" });
            dgvServo.dataGridView.Columns.Add(new DataGridViewButtonColumn { Name = "使能" });
            dgvServo.dataGridView.Columns.Add(new DataGridViewButtonColumn { Name = "设置原点偏移" });
            dgvServo.dataGridView.ReadOnly = false;
            dgvServo.dataGridView.RowHeadersVisible = false;
            dgvServo.dataGridView.AllowUserToAddRows = false;
            dgvServo.dataGridView.AllowUserToDeleteRows = false;
            dgvServo.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvServo.dataGridView.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvServo.dataGridView.AllowUserToResizeColumns = true;
            dgvServo.dataGridView.AllowUserToResizeRows = false;
            dgvServo.dataGridView.BackgroundColor = SystemColors.ActiveCaption;
            for (var i = 0; i < dgvServo.dataGridView.Columns.Count; i++)
            {
                dgvServo.dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvServo.dataGridView.Columns[i].ReadOnly = true;
            }

            dgvServo.dataGridView.Columns[2].ReadOnly =
                dgvServo.dataGridView.Columns[3].ReadOnly = dgvServo.dataGridView.Columns[4].ReadOnly = false;

            var program = robotController.RobotJogging.RobotPrograms.ToList().Find(f => f.Name == programName);

            if (program == null || program.AxisList == null) return;

            foreach (var t in program.AxisList)
            {
                var rowNum = dgvServo.dataGridView.Rows.Add();
                dgvServo.dataGridView.Rows[rowNum].Cells[0].Value = t.AxisNo;
                dgvServo.dataGridView.Rows[rowNum].Cells[1].Value = t.AxisNote;
                dgvServo.dataGridView.Rows[rowNum].Cells[2].Value = "使能";
                dgvServo.dataGridView.Rows[rowNum].Cells[3].Value = "失能";
                dgvServo.dataGridView.Rows[rowNum].Cells[4].Value = "设置原点偏移";
            }

            {
                var rowNum = dgvServo.dataGridView.Rows.Add();
                dgvServo.dataGridView.Rows[rowNum].Cells[0].Value = "全";
                dgvServo.dataGridView.Rows[rowNum].Cells[1].Value = "全";
                dgvServo.dataGridView.Rows[rowNum].Cells[2].Value = "使能";
                dgvServo.dataGridView.Rows[rowNum].Cells[3].Value = "失能";
                dgvServo.dataGridView.Rows[rowNum].Cells[4].Value = "NULL";
            }

            for (var i = 0; i < dgvServo.dataGridView.RowCount; i++)
                dgvServo.dataGridView.Rows[i].Height = 50;

            dgvServo.dataGridView.CellContentClick += dataGridView_CellContentClick;
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var dgv = sender as DataGridView;
            if (dgv == null)
                return;

            if (e.RowIndex == -1 || (e.ColumnIndex != 2 && e.ColumnIndex != 3 && e.ColumnIndex != 4))
                return;

            if (!RobotControllerMainForm.ShowBox())
                return;

            var row = dgv.Rows[e.RowIndex];
            if (row.Cells[0].Value != null && row.Cells[1].Value != null)
            {
                if (row.Cells[0].Value.ToString() == "全")
                {
                    var axixList = new List<int>();

                    for (var i = 0; i < dgv.RowCount; i++)
                    {
                        var r = dgv.Rows[i];
                        if (r.Cells[0].Value != null && r.Cells[0].Value.ToString() != @"全")
                            axixList.Add(int.Parse(r.Cells[0].Value.ToString()));
                    }

                    if (e.ColumnIndex == 2)
                        RobotController.RunEventServoOnMultiple(axixList);
                    else if (e.ColumnIndex == 3)
                        RobotController.RunEventServoOffMultiple(axixList);
                }
                else
                {
                    if (e.ColumnIndex == 2)
                        RobotController.RunEventServoOn(row.Cells[0].Value.ToString());
                    else if (e.ColumnIndex == 3)
                        RobotController.RunEventServoOff(row.Cells[0].Value.ToString());
                    else if (e.ColumnIndex == 4)
                        RobotController.RunEventSetHomeOffset(row.Cells[0].Value.ToString());
                }
            }
        }
    }
}
