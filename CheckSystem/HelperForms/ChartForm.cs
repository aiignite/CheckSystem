using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace CheckSystem.HelperForms
{
    public partial class ChartForm : Form
    {
        public Series sMax = new Series();
        public Series sMin = new Series();

        public ChartForm()
        {
            InitializeComponent();

            sMax.ChartType = SeriesChartType.Line;
            sMax.BorderWidth = 2;
            sMax.Color = Color.Blue;
            sMax.Name = "MaxValue";
            sMin.ChartType = SeriesChartType.Line;
            sMin.BorderWidth = 2;
            sMin.Color = Color.Blue;
            sMin.Name = "MinValue";
            
            sMin.Points.AddXY(0, 0.3);
            sMin.Points.AddXY(90, 4.3);
            sMin.Points.AddXY(180, 4.3);
            sMin.Points.AddXY(270, 0.3);

            sMax.Points.AddXY(0, 0.7);
            sMax.Points.AddXY(90, 4.7);
            sMax.Points.AddXY(180, 4.7);
            sMax.Points.AddXY(270, 0.7);

            chart1.Series.Add(sMax);
            chart1.Series.Add(sMin);

            chart1.Series[0].ChartType = SeriesChartType.Line;
            chart1.Series[0].BorderWidth = 2;
            chart1.Series[0].Color = Color.Red;

            var line = chart1.Series[0];
            var point = new { x = 0, y = 0.5};
            line.Points.AddXY(point.x, point.y);

            point = new { x = 90, y = 4.5 };
            line.Points.AddXY(point.x, point.y);

            point = new { x = 180, y = 4.5 };
            line.Points.AddXY(point.x, point.y);

            point = new { x = 270, y = 0.5 };
            line.Points.AddXY(point.x, point.y);

            chart1.ChartAreas[0].AxisX.ScaleView.Size = chart1.ChartAreas[0].AxisX.ScaleView.Size*2.5f;
        }
    }
}
