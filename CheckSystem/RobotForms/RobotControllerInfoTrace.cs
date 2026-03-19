using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Controller;

namespace CheckSystem.RobotForms
{
    public partial class RobotControllerInfoTrace : Form
    {
        private Thread MainTh { get; set; }
        private RobotControllerPpMode RobotController { get; set; }

        public RobotControllerInfoTrace(RobotControllerPpMode robotController)
        {
            InitializeComponent();
            RobotController = robotController;
            Closed += RobotControllerInfoTrace_Closed;

            readDataGrid.label.Text = @"读取机器人控制器当前信息";
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

            var fi = robotController.GetType().GetFields();

            foreach (var fieldInfo in fi)
            {
                if (Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)) == null)
                    continue;

                var des =
                    ((DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)))
                        .Description;

                if (des.StartsWith("R,RobotSate-") || des.StartsWith("R,输入_DI"))
                {
                    var rowAdd = readDataGrid.dataGridView.Rows.Add();
                    var row = readDataGrid.dataGridView.Rows[rowAdd];
                    row.DefaultCellStyle.BackColor = Color.DarkGoldenrod;

                    row.Cells[0].Value = fieldInfo.Name;
                    row.Cells[1].Value = fieldInfo.GetValue(robotController) == null
                        ? string.Empty
                        : fieldInfo.GetValue(robotController).ToString();
                }
            }

            for (var i = 0; i < 16; i++)
            {
                var random = new Random();
                var b = (16 * (i + 1));
                var randomColor = Color.FromArgb(random.Next(0, b), random.Next(0, b), random.Next(0, b));

                var str = "------" + i + "号轴------";

                //{
                //    var rowAdd = readDataGrid.dataGridView.Rows.Add();
                //    var row = readDataGrid.dataGridView.Rows[rowAdd];
                //    row.Cells[0].Value = str;
                //    row.Cells[1].Value = str;
                //    row.DefaultCellStyle.BackColor = randomColor;
                //}

                foreach (var t in RobotController.SingleAxisState[i].Keys)
                {
                    var rowAdd = readDataGrid.dataGridView.Rows.Add();
                    var row = readDataGrid.dataGridView.Rows[rowAdd];
                    row.Cells[0].Value = str + "-" + t;
                    row.Cells[1].Value = RobotController.SingleAxisState[i][t];
                    row.DefaultCellStyle.BackColor = randomColor;
                }
            }

            if (MainTh != null)
            {
                MainTh.Abort();
                MainTh.Join();
            }

            MainTh = new Thread(MainWork) { IsBackground = true };
            MainTh.Start();
        }

        private void RobotControllerInfoTrace_Closed(object sender, EventArgs e)
        {
            if (MainTh == null)
                return;
            MainTh.Abort();
            MainTh.Join();
        }

        private void MainWork()
        {
            while (MainTh.IsAlive)
            {
                if (!MainTh.IsAlive)
                    break;

                Thread.Sleep(50);

                try
                {
                    var fi = RobotController.GetType().GetFields();

                    foreach (var fieldInfo in fi)
                    {
                        if (Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)) == null)
                            continue;

                        var des =
                            ((DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)))
                                .Description;

                        if (!des.StartsWith("R,RobotSate-") && !des.StartsWith("R,输入_DI"))
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
                               fieldInfo.GetValue(RobotController) == null
                                   ? string.Empty
                                   : fieldInfo.GetValue(RobotController).ToString();
                            break;
                        }
                    }

                    for (var i = 0; i < 16; i++)
                    {
                        var str = "------" + i + "号轴------";

                        foreach (var t in RobotController.SingleAxisState[i].Keys)
                        {
                            for (var j = 0; j < readDataGrid.dataGridView.RowCount; j++)
                            {
                                var row = readDataGrid.dataGridView.Rows[j];

                                if (row.Cells[0].Value == null ||
                                    row.Cells[0].Value.ToString() != str + "-" + t)
                                    continue;

                                row.Cells[1].Value = RobotController.SingleAxisState[i][t];
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }
    }
}
