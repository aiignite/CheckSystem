using NationalInstruments.Vision.WindowsForms;
using Sunny.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CheckSystem.VisionDetection.Calibration.StaticVision
{
    public partial class FrmStaticVisionCali : UIForm
    {
        private readonly ImageViewer _mainImageViewer = new NationalInstruments.Vision.WindowsForms.ImageViewer();

        public FrmStaticVisionCali()
        {
            InitializeComponent();
            InitImageViewer(_mainImageViewer);
            Load += FrmStaticVisionCali_Load;
        }

        private void InitImageViewer(ImageViewer imageViewer)
        {
            _mainImageViewer.Dock = DockStyle.Fill;
            _mainImageViewer.Margin = new Padding(3);
            uiGroupBox3.Controls.Add(_mainImageViewer);
            ImageShowTool(imageViewer);
            imageViewer.SizeChanged += imageViewer_SizeChanged;
            //imageViewer.RoiChanged += imageViewer_RoiChanged;
        }

        private static void ImageShowTool(ImageViewer imageViewer)
        {
            imageViewer.ToolsShown = ViewerTools.ZoomIn |
                                     ViewerTools.ZoomOut |
                                     ViewerTools.Pan |
                                     ViewerTools.Selection;
            //ViewerTools.Rectangle;
            //ViewerTools.All;
            imageViewer.ActiveTool = ViewerTools.Selection;
            imageViewer.ZoomToFit = true;
            imageViewer.ShowToolbar = true;
            imageViewer.ShowScrollbars = true;
            imageViewer.ShowImageInfo = true;

            //imageViewer.AutoDelete = false;
        }

        private static void imageViewer_SizeChanged(object sender, EventArgs e)
        {
            var imageViewer = sender as ImageViewer;
            if (imageViewer != null)
                ImageShowTool(imageViewer);
        }

        private void FrmStaticVisionCali_Resize(object sender, EventArgs e)
        {
            var newX = (this.Width) / _x; //窗体宽度缩放比例
            var newY = (this.Height) / _y;//窗体高度缩放比例
            SetControls(newX, newY, this);//随窗体改变控件大小

            uiBreadcrumb1.ItemWidth = this.Width / uiBreadcrumb1.Count;
        }

        private void FrmStaticVisionCali_Load(object sender, EventArgs e)
        {
            _x = this.Width;//获取窗体的宽度
            _y = this.Height;//获取窗体的高度
            SetTag(this);//调用方法

            Resize += FrmStaticVisionCali_Resize;
            WindowState = FormWindowState.Maximized;
        }

        private float _x;//当前窗体的宽度
        private float _y;//当前窗体的高度

        /// <summary>
        /// 将控件的宽，高，左边距，顶边距和字体大小暂存到tag属性中
        /// </summary>
        /// <param name="cons">递归控件中的控件</param>
        private static void SetTag(System.Windows.Forms.Control cons)
        {
            foreach (System.Windows.Forms.Control con in cons.Controls)//循环窗体中的控件
            {
                con.Tag = con.Width + ":" + con.Height + ":" + con.Left + ":" + con.Top + ":" + con.Font.Size;
                if (con.Controls.Count > 0)
                    SetTag(con);
            }
        }

        /// <summary>
        /// 根据窗体大小调整控件大小
        /// </summary>
        /// <param name="newx">窗体宽度缩放比例</param>
        /// <param name="newy">窗体高度缩放比例</param>
        /// <param name="cons">随窗体改变控件大小</param>
        private static void SetControls(float newx, float newy, System.Windows.Forms.Control cons)
        {
            //遍历窗体中的控件，重新设置控件的值
            foreach (System.Windows.Forms.Control con in cons.Controls)
            {
                var mytag = con.Tag.ToString().Split(new char[] { ':' });//获取控件的Tag属性值，并分割后存储字符串数组
                var a = System.Convert.ToSingle(mytag[0]) * newx;//根据窗体缩放比例确定控件的值，宽度
                con.Width = (int)a;//宽度
                a = System.Convert.ToSingle(mytag[1]) * newy;//高度
                con.Height = (int)(a);
                a = System.Convert.ToSingle(mytag[2]) * newx;//左边距离
                con.Left = (int)(a);
                a = System.Convert.ToSingle(mytag[3]) * newy;//上边缘距离
                con.Top = (int)(a);
                var currentSize = System.Convert.ToSingle(mytag[4]) * newy;//字体大小
                con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                if (con.Controls.Count > 0)
                    SetControls(newx, newy, con);
            }
        }

        private void btnOpenLocalImg_Click(object sender, EventArgs e)
        {

        }
    }
}
