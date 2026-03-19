using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace UserControls
{
    public partial class UserChartDataViewer : UserControl
    {
        //public bool BInitMaxMinLine;
        public Series SMax = new Series();
        public Series SMin = new Series();

        public UserChartDataViewer()
        {
            InitializeComponent();
            InitDataGridView(userDataGridCheckData.dataGridView);

            SMax.ChartType = SeriesChartType.Line;
            SMax.BorderWidth = 1;
            SMax.ShadowOffset = 1;
            SMax.Color = Color.Blue;
            SMax.Name = "Max";
            SMin.ChartType = SeriesChartType.Line;
            SMin.BorderWidth = 1;
            SMin.ShadowOffset = 1;
            SMin.Color = Color.DodgerBlue;
            SMin.Name = "Min";

            chart1.Series.Add(SMax);
            chart1.Series.Add(SMin);

            chart1.Series[0].ChartType = SeriesChartType.Line;
            chart1.Series[0].BorderWidth = 1;
            chart1.Series[0].Color = Color.Red;
            chart1.Series[0].ShadowOffset = 1;

            //chart1.ChartAreas[0].AxisX.Interval = 10;
            //chart1.ChartAreas[0].AxisX.IntervalOffset = 10;
            //chart1.ChartAreas[0].AxisX.LabelStyle.IsStaggered = true;

            //chart1.ChartAreas[0].AxisY.Interval = 0.1;
            //chart1.ChartAreas[0].AxisY.IntervalOffset = 0.1;
            //chart1.ChartAreas[0].AxisY.LabelStyle.IsStaggered = true;
        }

        private static void InitDataGridView(DataGridView dgv)
        {
            var dataGridView = dgv;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.AllowUserToResizeColumns = true;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            //userDataGridGrayList.dataGridView.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridView.Margin = new Padding(3, 4, 3, 4);
            dataGridView.RowTemplate.Height = 30;
            dataGridView.RowsDefaultCellStyle.Font = new Font("微软雅黑", 12, FontStyle.Regular);
            dataGridView.EnableHeadersVisualStyles = false;
            dataGridView.RowHeadersVisible = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToResizeRows = false;
            dataGridView.ReadOnly = true;
            dataGridView.BackgroundColor = SystemColors.ActiveCaption;
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSkyBlue;
            dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.LightYellow;

            //获取控件的Type,设置双缓存
            var dgvType = dataGridView.GetType();
            var properInfo = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            //为控件的属性设置值
            if (properInfo != null) properInfo.SetValue(dataGridView, true, null);
        }
    }
}
