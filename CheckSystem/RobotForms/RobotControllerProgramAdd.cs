using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CommonUtility;
using CommonUtility.FileOperator;
using Controller;
using HZH_Controls.IconFont;

namespace CheckSystem.RobotForms
{
    public partial class RobotControllerProgramAdd : Form
    {
        private RobotJogging RobotPrograms { get; set; }

        public RobotControllerProgramAdd()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            Icon = FontImages.GetIcon(FontIcons.E_icon_folder_add, 32,
                Color.DodgerBlue);
        }

        public RobotControllerProgramAdd(RobotJogging robotPrograms)
        {
            RobotPrograms = robotPrograms;
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            Icon = FontImages.GetIcon(FontIcons.E_icon_folder_add, 32,
                Color.DodgerBlue);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(labelText1.textBox.Text) || string.IsNullOrEmpty(labelText2.textBox.Text))
            {
                MessageBox.Show(@"请添加程序名和备注");
                return;
            }

            var programName = labelText1.textBox.Text;
            if (RobotPrograms.RobotPrograms != null && RobotPrograms.RobotPrograms.ToList().FindIndex(f => f.Name == programName) != -1)
            {
                MessageBox.Show(@"程序名已经存在");
                return;
            }

            var asisList = new List<RobotJoggingRobotProgramAxis>();

            for (var i = 0; i < userDataGrid1.dataGridView.RowCount; i++)
            {
                var row = userDataGrid1.dataGridView.Rows[i];

                var ck = row.Cells[0].Value;

                if (ck == null || ck.ToString() == false.ToString())
                    continue;

                var axisNo = row.Cells[1].Value.ToString();
                var note = row.Cells[2].Value == null ? "NULL" : row.Cells[2].Value.ToString();

                asisList.Add(new RobotJoggingRobotProgramAxis
                {
                    AxisNo = axisNo,
                    AxisNote = note
                });
            }

            if (!asisList.Any())
            {
                MessageBox.Show(@"请选择至少一个轴");
                return;
            }

            if (!RobotControllerMainForm.ShowBox())
                return;

            if (RobotPrograms.RobotPrograms != null)
            {
                var temp = RobotPrograms.RobotPrograms.ToList();

                var toAdd = new RobotJoggingRobotProgram
                {
                    Note = labelText1.textBox.Text,
                    Name = labelText1.textBox.Text,
                    RobotName = "RobotName",
                    AxisList = asisList.ToArray(),
                    Blocks = new string[0]
                };

                temp.Add(toAdd);
                RobotPrograms.RobotPrograms = temp.ToArray();
            }

            var robotJoggingFilePath = RobotControllerPpMode.ConfilePath;
            //Directory.GetCurrentDirectory() + @"\ControllerConfig\RobotJogging.xml";
            if (File.Exists(robotJoggingFilePath))
                File.Delete(robotJoggingFilePath);
            XmlHelper.SerializeToFile(RobotPrograms, robotJoggingFilePath, Encoding.UTF8);

            DialogResult = DialogResult.OK;
        }

        private void FormProgramAdd_Load(object sender, EventArgs e)
        {
            userDataGrid1.label.Text = @"选择轴";
            userDataGrid1.dataGridView.Columns.Add(new DataGridViewCheckBoxColumn { Name = "选择" });
            userDataGrid1.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "轴号" });
            userDataGrid1.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "备注" });

            userDataGrid1.dataGridView.ForeColor = Color.Black;
            userDataGrid1.dataGridView.AllowUserToAddRows = false;
            userDataGrid1.dataGridView.AllowUserToDeleteRows = false;
            userDataGrid1.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            userDataGrid1.dataGridView.AllowUserToResizeColumns = true;
            userDataGrid1.dataGridView.AllowUserToResizeRows = false;
            userDataGrid1.dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            userDataGrid1.dataGridView.BackgroundColor = SystemColors.ActiveCaption;
            userDataGrid1.dataGridView.RowHeadersVisible = false;
            for (var i = 0; i < userDataGrid1.dataGridView.Columns.Count; i++)
            {
                userDataGrid1.dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                userDataGrid1.dataGridView.Columns[i].ReadOnly = true;
            }
            userDataGrid1.dataGridView.Columns[2].ReadOnly = false;
            userDataGrid1.dataGridView.CellClick += dataGridView_CellClick;

            for (var i = 0; i < 16; i++)
            {
                userDataGrid1.dataGridView.Rows.Add();
                var row = userDataGrid1.dataGridView.Rows[i];

                row.Cells[1].Value = i.ToString();
                row.Cells[2].Value = @"NULL";
            }
        }

        private static void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var dgv = sender as DataGridView;
            if (dgv == null)
                return;
            if (e.ColumnIndex != 0)
                return;

            var ck = dgv.Rows[e.RowIndex].Cells[0].Value;

            if (ck == null || ck.ToString() == false.ToString())
            {
                dgv.Rows[e.RowIndex].Cells[0].Value = true;
            }
            else
            {
                dgv.Rows[e.RowIndex].Cells[0].Value = false;
            }
        }
    }
}
