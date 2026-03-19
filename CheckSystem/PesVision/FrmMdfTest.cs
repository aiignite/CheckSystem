using Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NationalInstruments.Vision.WindowsForms;
using CommonUtility.HikSdk;
using NationalInstruments.Vision.Analysis;
using NationalInstruments.Vision;
using OpenCvSharp;

namespace CheckSystem.PesVision
{
    public partial class FrmMdfTest : Form
    {
        private readonly ImageViewer _mainImageViewer = new ImageViewer { Dock = DockStyle.Fill };
        private readonly MdfHeadlampAnalysis _headlampAnalysisByGb2591 = new MdfHeadlampAnalysis("MDF光型检测");
        private readonly CameraControl _hikcamera = new CameraControl();

        public FrmMdfTest()
        {
            InitializeComponent();
            ImagePanel.Controls.Add(_mainImageViewer);
            _hikcamera.DeviceListAcq();
            Load += FrmMdfTest_Load;
        }

        private void FrmMdfTest_Load(object sender, EventArgs e)
        {
            _x = Width;//获取窗体的宽度
            _y = Height;//获取窗体的高度
            SetTag(funcPanel);//调用方法
            Resize += FrmPesTest_Resize;
            cmbImageList.SelectedIndexChanged += CmbImageList_SelectedIndexChanged;
            BindDictionaryToComboBox();
            InitImageViewer(_mainImageViewer);

            btnClearAllImage.Click += BtnClearAllImage_Click;
            btnAnalysis.Click += BtnAnalysis_Click;

            WindowState = FormWindowState.Maximized;
        }

        private void CmbImageList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbImageList.SelectedItem == null)
                return;

            // 获取选中的键和值
            var selectedItem = (KeyValuePair<string, Mat>)cmbImageList.SelectedItem;
            var key = selectedItem.Key;
            var value = selectedItem.Value;

            var visionImg = MyCamera.MatToVisionImage(value);
            Algorithms.Copy(visionImg, _mainImageViewer.Image);
            visionImg.Dispose();
            GC.Collect();
        }

        private void BindDictionaryToComboBox()
        {
            if (!_headlampAnalysisByGb2591.Images.Any())
            {
                // 清除当前绑定数据
                cmbImageList.DataSource = null;
                cmbImageList.Items.Clear();

                var emptyImage = new VisionImage();
                Algorithms.Copy(emptyImage, _mainImageViewer.Image);
                emptyImage.Dispose();
                return;
            }

            cmbImageList.DataSource = new BindingSource(_headlampAnalysisByGb2591.Images, null);
            cmbImageList.DisplayMember = "Key"; // 显示的文本
            cmbImageList.ValueMember = "Key"; // 实际绑定的值

            cmbImageList.SelectedIndex = cmbImageList.Items.Count - 1;
        }

        #region func 控件缩放

        private float _x;//当前窗体的宽度
        private float _y;//当前窗体的高度

        private void FrmPesTest_Resize(object sender, EventArgs e)
        {
            var newX = Width / _x; //窗体宽度缩放比例
            var newY = Height / _y;//窗体高度缩放比例
            SetControls(newX, newY, funcPanel);//随窗体改变控件大小
        }

        /// <summary>
        /// 将控件的宽，高，左边距，顶边距和字体大小暂存到tag属性中
        /// </summary>
        /// <param name="cons">递归控件中的控件</param>
        private static void SetTag(Control cons)
        {
            foreach (Control con in cons.Controls)//循环窗体中的控件
            {
                //if (con.GetType() != typeof(UIButton))
                {
                    con.Tag = con.Width + ":" + con.Height + ":" + con.Left + ":" + con.Top + ":" + con.Font.Size;
                    if (con.Controls.Count > 0)
                        SetTag(con);
                }
            }
        }

        /// <summary>
        /// 根据窗体大小调整控件大小
        /// </summary>
        /// <param name="newx">窗体宽度缩放比例</param>
        /// <param name="newy">窗体高度缩放比例</param>
        /// <param name="cons">随窗体改变控件大小</param>
        private static void SetControls(float newx, float newy, Control cons)
        {
            //遍历窗体中的控件，重新设置控件的值
            foreach (Control con in cons.Controls)
            {
                //if (con.GetType() != typeof(UIButton))
                {
                    var mytag = con.Tag.ToString().Split(new char[] { ':' });//获取控件的Tag属性值，并分割后存储字符串数组
                    var a = Convert.ToSingle(mytag[0]) * newx;//根据窗体缩放比例确定控件的值，宽度
                    con.Width = (int)a;//宽度
                    a = Convert.ToSingle(mytag[1]) * newy;//高度
                    con.Height = (int)(a);
                    a = Convert.ToSingle(mytag[2]) * newx;//左边距离
                    con.Left = (int)(a);
                    a = Convert.ToSingle(mytag[3]) * newy;//上边缘距离
                    con.Top = (int)(a);
                    var currentSize = Convert.ToSingle(mytag[4]) * newy;//字体大小
                    con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                    if (con.Controls.Count > 0)
                        SetControls(newx, newy, con);
                }
            }
        }

        #endregion

        #region ImageViewer

        private void InitImageViewer(ImageViewer imageViewer)
        {
            ImageShowTool(imageViewer);
            imageViewer.SizeChanged += ImageViewer_SizeChanged;
            //imageViewer.RoiChanged += imageViewer_RoiChanged;
        }

        private void ImageViewer_SizeChanged(object sender, EventArgs e)
        {
            var imageViewer = sender as ImageViewer;
            if (imageViewer != null)
                ImageShowTool(imageViewer);
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

        #endregion

        private void BtnClearAllImage_Click(object sender, EventArgs e)
        {
            rtxClassCPath.Text = string.Empty;
            rtxMdfPath.Text = string.Empty;

            _headlampAnalysisByGb2591.ClearAllImage();
            BindDictionaryToComboBox();
        }

        private void rtxClassCPath_DoubleClick(object sender, EventArgs e)
        {
            var openFi = new OpenFileDialog
            {
                Site = null,
                Tag = null,
                AddExtension = false,
                CheckPathExists = false,
                DefaultExt = null,
                DereferenceLinks = false,
                FileName = null,
                Filter = null,
                FilterIndex = 0,
                InitialDirectory = null,
                RestoreDirectory = false,
                ShowHelp = false,
                SupportMultiDottedExtensions = false,
                Title = null,
                ValidateNames = false,
                AutoUpgradeEnabled = false,
                CheckFileExists = false,
                Multiselect = false,
                ReadOnlyChecked = false,
                ShowReadOnly = false
            };
            openFi.Filter = "图像文件(JPeg, Gif, Bmp, etc.)|*.jpg;*.jpeg;*.gif;*.bmp;*.tif; *.tiff; *.png| JPeg 图像文件(*.jpg;*.jpeg)"
                            + "|*.jpg;*.jpeg |GIF 图像文件(*.gif)|*.gif |BMP图像文件(*.bmp)|*.bmp|Tiff图像文件(*.tif;*.tiff)|*.tif;*.tiff|Png图像文件(*.png)"
                            + "| *.png |所有文件(*.*)|*.*";
            if (openFi.ShowDialog() == DialogResult.OK)
            {
                rtxClassCPath.Text = openFi.FileName;
                _headlampAnalysisByGb2591.ReadImage("近光原图", openFi.FileName);
                BindDictionaryToComboBox();
            }
        }

        private void rtxMdfPath_MouseDoubleClick(object sender, EventArgs e)
        {
            var openFi = new OpenFileDialog
            {
                Site = null,
                Tag = null,
                AddExtension = false,
                CheckPathExists = false,
                DefaultExt = null,
                DereferenceLinks = false,
                FileName = null,
                Filter = null,
                FilterIndex = 0,
                InitialDirectory = null,
                RestoreDirectory = false,
                ShowHelp = false,
                SupportMultiDottedExtensions = false,
                Title = null,
                ValidateNames = false,
                AutoUpgradeEnabled = false,
                CheckFileExists = false,
                Multiselect = false,
                ReadOnlyChecked = false,
                ShowReadOnly = false
            };
            openFi.Filter = "图像文件(JPeg, Gif, Bmp, etc.)|*.jpg;*.jpeg;*.gif;*.bmp;*.tif; *.tiff; *.png| JPeg 图像文件(*.jpg;*.jpeg)"
                            + "|*.jpg;*.jpeg |GIF 图像文件(*.gif)|*.gif |BMP图像文件(*.bmp)|*.bmp|Tiff图像文件(*.tif;*.tiff)|*.tif;*.tiff|Png图像文件(*.png)"
                            + "| *.png |所有文件(*.*)|*.*";
            if (openFi.ShowDialog() == DialogResult.OK)
            {

                rtxMdfPath.Text = openFi.FileName;
                _headlampAnalysisByGb2591.ReadImage("MDF原图", openFi.FileName);
                BindDictionaryToComboBox();
            }
        }

        private void BtnAnalysis_Click(object sender, EventArgs e)
        {
            txtMdfResult.Text = string.Empty;

            _headlampAnalysisByGb2591.Ratio = 8000;
            _headlampAnalysisByGb2591.PixelScale = 3.45;
            _headlampAnalysisByGb2591.CameraDistanceCm = 34.2;
            _headlampAnalysisByGb2591.FresnelLensDistanceCm = 48;
            _headlampAnalysisByGb2591.ScanStartX = 1940;
            _headlampAnalysisByGb2591.ScanEndX = 2270;
            _headlampAnalysisByGb2591.ScanDegreeH = 10;
            _headlampAnalysisByGb2591.ScanDegreeV = 5;

            _headlampAnalysisByGb2591.AnalyzeImage("近光原图", "MDF原图");
            BindDictionaryToComboBox();
            txtMdfResult.Text = _headlampAnalysisByGb2591.MdfHDegree.ToString(CultureInfo.InvariantCulture);
        }
    }
}
