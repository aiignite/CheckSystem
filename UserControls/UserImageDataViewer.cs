using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using NationalInstruments.Vision;
using NationalInstruments.Vision.Analysis;

namespace UserControls
{
    public partial class UserImageDataViewer : UserControl
    {
        public UserImageDataViewer()
        {
            InitializeComponent();
            
            //imageViewer.ToolsShown = ViewerTools.Point;
            //imageViewer.ActiveTool = ViewerTools.Point;
            imageViewer.Dock = DockStyle.Fill;
            imageViewer.ZoomToFit = true;
            imageViewer.ShowToolbar = false;
            imageViewer.ShowScrollbars = true;
            imageViewer.ShowImageInfo = true;

            InitDataGridView(userDataGridCheckData.dataGridView);
            InitDataGridView(userGrayData.dataGridView);

            //dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "组号" });
            //dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "序号" });
            //dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "最小值" });
            //dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "最大值" });
            //dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "实际值" });

            //for (var i = 0; i < dataGridView.Columns.Count; i++)
            //    dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
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

        public void ShowImg(VisionImage img)
        {
            Algorithms.Copy(img, imageViewer.Image);

            //var dataGridView = userDataGrid1.dataGridView;
            //dataGridView.Rows.Clear();

            //imageViewer.Roi.Clear();
            //ImageProcessing.DrawContourCountInOverlay(imageViewer.Image, imageViewer.Roi);
        }

        public void SetOff()
        {
            //button1.BackColor = Color.Gray;
        }

        public void SetGreen()
        {
            //button1.BackColor = Color.Green;
        }

        public void SetRed()
        {
            //button1.BackColor = Color.Red;
        }

        //public void ShowData(string title, List<UserImageDataViewerDataStruct> datas)
        //{
        //    var dataGridView = userDataGrid1.dataGridView;

        //    dataGridView.Rows.Clear();
        //    imageViewer.Roi.Clear();
        //    ImageProcessing.DrawContourCountInOverlay(imageViewer.Image, imageViewer.Roi);

        //    TitleString = title;

        //    var isOk = true;

        //    for (var i = 0; i < datas.Count; i++)
        //    {
        //        var d = datas[i];
        //        if (d.CounterType.Equals(typeof(PolygonContour).Name))
        //            imageViewer.Roi.Add(ImageProcessing.GetPolygonContourByString(d.Rect));
        //        else if (d.CounterType.Equals(typeof(RectangleContour).Name))
        //            imageViewer.Roi.Add(ImageProcessing.GetRectByString(d.Rect));

        //        var rowAddIndex = dataGridView.Rows.Add();

        //        var newRow = dataGridView.Rows[rowAddIndex];
        //        newRow.Cells[0].Value = d.GroupIndex.ToString(CultureInfo.InvariantCulture);
        //        newRow.Cells[1].Value = (i + 1).ToString(CultureInfo.InvariantCulture);
        //        newRow.Cells[2].Value = d.Min.ToString(CultureInfo.InvariantCulture);
        //        newRow.Cells[3].Value = d.Max.ToString(CultureInfo.InvariantCulture);
        //        newRow.Cells[4].Value = d.Value.ToString(CultureInfo.InvariantCulture);

        //        if (d.Value < d.Min || d.Value > d.Max)
        //        {
        //            newRow.DefaultCellStyle.BackColor = Color.Red;
        //            isOk = false;
        //        }

        //        dataGridView.FirstDisplayedScrollingRowIndex = dataGridView.RowCount - 1;
        //    }

        //    if (!isOk || dataGridView.Rows.Count == 0)
        //    {
        //        userDataGrid1.label.BackColor = Color.Red;
        //        TitleString += @"(NG)";
        //    }
        //    else
        //    {
        //        userDataGrid1.label.BackColor = Color.Green;
        //        TitleString += @"(OK)";
        //    }
        //    ImageProcessing.DrawContourCountInOverlay(imageViewer.Image, imageViewer.Roi);

        //    for (var i = 0; i < userDataGrid2.dataGridView.Rows.Count; i++)
        //    {
        //        if (userDataGrid2.dataGridView.Rows[i].Cells[0].Value.ToString() == title)
        //        {
        //            if (!isOk || dataGridView.Rows.Count == 0)
        //            {
        //                userDataGrid2.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Red;
        //                userDataGrid2.dataGridView.Rows[i].Cells[1].Value = @"NG";
        //            }
        //            else
        //            {
        //                userDataGrid2.dataGridView.Rows[i].Cells[1].Value = @"OK";
        //            }

        //            return;
        //        }
        //    }

        //    var radd = userDataGrid2.dataGridView.Rows.Add();
        //    var r = userDataGrid2.dataGridView.Rows[radd];

        //    r.Cells[0].Value = title.ToString();

        //    if (!isOk || dataGridView.Rows.Count == 0)
        //    {
        //        r.DefaultCellStyle.BackColor = Color.Red;
        //        r.Cells[1].Value = @"NG";
        //    }
        //    else
        //    {
        //        r.Cells[1].Value = @"OK";
        //    }
        //}

        //public void SetTitle(string title)
        //{
        //    userDataGrid2.label.Text = title;
        //}

        //public void ClearData()
        //{
        //    TitleString = string.Empty;
        //    userDataGrid1.dataGridView.Rows.Clear();
        //    userDataGrid2.dataGridView.Rows.Clear();
        //}

        //public struct UserImageDataViewerDataStruct
        //{
        //    public int GroupIndex { get; set; }
        //    public string CounterType { get; set; }
        //    public string Rect { get; set; }
        //    public double Min { get; set; }
        //    public double Max { get; set; }
        //    public double Value { get; set; }
        //}
    }
}
