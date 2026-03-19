using NationalInstruments.Vision;
using NationalInstruments.Vision.Analysis;
using NationalInstruments.Vision.WindowsForms;
using OpenCvSharp;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using CommonUtility.FileOperator;
using Controller;
using Mat = OpenCvSharp.Mat;
using Point = OpenCvSharp.Point;

namespace CheckSystem.PesVision
{
    public partial class PesVisionConfig : UIForm
    {
        private readonly ImageViewer _mainImageViewer = new ImageViewer { Dock = DockStyle.Fill };
        private readonly ImageViewer _lowBeamImageViewer0 = new ImageViewer { Dock = DockStyle.Fill };
        private readonly ImageViewer _lowBeamImageViewer1 = new ImageViewer { Dock = DockStyle.Fill };
        private readonly ImageViewer _highBeamImageViewer = new ImageViewer { Dock = DockStyle.Fill };

        private readonly Dictionary<string, ImageViewer> _imageViewers = new Dictionary<string, ImageViewer>();
        private readonly ImageViewer _toShowDetailImageViewer = new ImageViewer { Dock = DockStyle.Fill };

        private RectangleContour _defaultContour = new RectangleContour(1435, 550, 100, 65);

        private readonly Mat _defaultBlackMat = new Mat(300, 300, MatType.CV_8UC3, new Scalar(0, 0, 0));
        private readonly string _defaulBlackMatText = @"NO IMAGE!";
        private VisionImage _defaultBlackVisionImage;

        private readonly PesVisionAnalysis _pesVisionAnalysis;
        private IniFileHelper _iniFile;

        public PesVisionConfig(PesVisionAnalysis pesVisionAnalysis, IniFileHelper iniFile)
        {
            InitializeComponent();
            _pesVisionAnalysis = pesVisionAnalysis;
            _iniFile = iniFile;
            WindowState = FormWindowState.Maximized;
            CommonUtility.HikSdk.MyCamera.PushMat += MyCamera_PushMat;
            Load += PesVisionConfig_Load;
            Closed += PesVisionConfig_Closed;
        }

        private void PesVisionConfig_Closed(object sender, EventArgs e)
        {
            CommonUtility.HikSdk.MyCamera.PushMat -= MyCamera_PushMat;
            _pesVisionAnalysis.Release();
            if (_pesVisionAnalysis.SelectedCamera != null)
                _pesVisionAnalysis.SelectedCamera.CloseCamera();
        }

        private void PesVisionConfig_Load(object sender, EventArgs e)
        {
            // 设置字体、文本内容、字体大小和颜色
            const HersheyFonts fontFace = HersheyFonts.HersheySimplex;
            const double fontScale = 1.0;
            int baseline;
            // 获取文字的尺寸
            var textSize = Cv2.GetTextSize(_defaulBlackMatText, fontFace, fontScale, 1, out baseline);
            // 计算文字位置，使其居中
            var textOrg = new Point((_defaultBlackMat.Width - textSize.Width) / 2, (_defaultBlackMat.Height + textSize.Height) / 2);
            // 打印文字到图像上
            Cv2.PutText(_defaultBlackMat, _defaulBlackMatText, textOrg, fontFace, fontScale, Scalar.Red, 1, LineTypes.AntiAlias);

            _defaultContour = new RectangleContour(
                _pesVisionAnalysis.InterestedRoi.X,
                _pesVisionAnalysis.InterestedRoi.Y,
                _pesVisionAnalysis.InterestedRoi.Width,
                _pesVisionAnalysis.InterestedRoi.Height);

            _defaultBlackVisionImage = CommonUtility.HikSdk.MyCamera.MatToVisionImage(_defaultBlackMat);
            _lowBeamImageViewer0.Roi.Add(_defaultContour);

            //_mainImageViewer.ContextMenuStrip = contextMenuStrip1;
            _mainImageViewer.ImageMouseDown += _mainImageViewer_ImageMouseDown;
            InitImageViewer(_mainImageViewer, gpCamera);
            InitImageViewer(_lowBeamImageViewer0, gpLowBeamWithHighShutter);
            InitImageViewer(_lowBeamImageViewer1, gpLowBeam);
            InitImageViewer(_highBeamImageViewer, gpHighBeam);
            InitImageViewer(_toShowDetailImageViewer, gpToShowDetail);

            InitAnalysisFlowPanel();
        }

        private void _mainImageViewer_ImageMouseDown(object sender, ImageMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                contextMenuStrip1.Show(MousePosition);
        }

        private void InitAnalysisFlowPanel()
        {
            analysisFlowPanel.AutoScroll = false;
            analysisFlowPanel.FlowDirection = FlowDirection.TopDown;
            analysisFlowPanel.WrapContents = false;
            analysisFlowPanel.HorizontalScroll.Maximum = 0;
            analysisFlowPanel.AutoScroll = true;

            foreach (var t in _pesVisionAnalysis.ImgMats)
            {
                _imageViewers.Add(t.Key, new ImageViewer { Dock = DockStyle.Fill });
                analysisFlowPanel.Controls.Add(InitAnalysisGroupWithImgViewer(t.Key, _imageViewers[t.Key]));
            }
        }

        private UIGroupBox InitAnalysisGroupWithImgViewer(string text, ImageViewer imageViewer)
        {
            var gp = new UIGroupBox
            {
                Margin = new Padding(1),
                Padding = new Padding(1),
                Width = analysisFlowPanel.Width,
                Height = analysisFlowPanel.Width,
                Text = text
            };
            InitImageViewer(imageViewer, gp, true);
            return gp;
        }

        private void InitImageViewer(ImageViewer imageViewer, Control parentControl, bool isNeedClick = false)
        {
            parentControl.Controls.Add(imageViewer);

            ImageShowTool(imageViewer, true);
            imageViewer.SizeChanged += imageViewer_SizeChanged;
            imageViewer.RoiChanged += ImageViewer_RoiChanged;
            if (isNeedClick) imageViewer.ImageMouseDown += ImageViewer_ImageMouseDown;
        }

        private void ImageShowTool(ImageViewer imageViewer, bool isNew = false)
        {
            imageViewer.ToolsShown = ViewerTools.ZoomIn |
                                     ViewerTools.ZoomOut |
                                     ViewerTools.Pan |
                                     ViewerTools.Selection;
            imageViewer.ActiveTool = ViewerTools.Selection;
            imageViewer.ZoomToFit = true;
            imageViewer.ShowToolbar = true;
            imageViewer.ShowScrollbars = true;
            imageViewer.ShowImageInfo = true;
            imageViewer.AutoDelete = true;

            if (isNew)
                MatCopyToImageViewer(_defaultBlackMat, imageViewer);
        }

        private void ImageViewer_ImageMouseDown(object sender, ImageMouseEventArgs e)
        {
            //return;
            var imageViewer = sender as ImageViewer;
            if (imageViewer != null)
            {
                var parentGp = imageViewer.Parent;
                if (parentGp != null && parentGp.GetType() == typeof(UIGroupBox))
                {
                    var uiGp = (parentGp as UIGroupBox);
                    if (uiGp != null)
                    {
                        gpToShowDetail.Text = uiGp.Text;
                        Algorithms.Copy(imageViewer.Image, _toShowDetailImageViewer.Image);
                    }
                }
            }
        }

        private void ImageViewer_RoiChanged(object sender, ContoursChangedEventArgs e)
        {
            if (_lowBeamImageViewer0.Roi.Count != 1)
            {
                _lowBeamImageViewer0.Roi.Clear();
                _lowBeamImageViewer0.Roi.Add(_defaultContour);
            }
        }

        private void imageViewer_SizeChanged(object sender, EventArgs e)
        {
            var imageViewer = sender as ImageViewer;
            if (imageViewer != null)
                ImageShowTool(imageViewer);
        }

        private void MyCamera_PushMat(
            CommonUtility.HikSdk.MvCameraSdk.MV_GIGE_DEVICE_INFO gigeInfo, global::OpenCvSharp.Mat mat)
        {
            //if (gigeInfo.chSerialNumber == PesVisionMain.CameraSn && !btnStartGrab.Enabled)
            //{
            //    var visionImg = CommonUtility.HikSdk.MyCamera.MatToVisionImage(mat);
            //    Algorithms.Copy(visionImg, _mainImageViewer.Image);
            //    visionImg.Dispose();
            //    mat.Dispose();
            //    //GC.Collect();

            //    //var device = VisionCommon.NiImaqd.CameraList.Find(f => f.GigeInfo.chSerialNumber == PesVisionMain.CameraSn);
            //    //if (device != null)
            //    //{
            //    //    var visionImg = CommonUtility.HikSdk.MyCamera.MatToVisionImage(mat);
            //    //    Algorithms.Copy(visionImg, _mainImageViewer.Image);
            //    //    visionImg.Dispose();
            //    //    mat.Dispose();
            //    //    GC.Collect();
            //    //}
            //}
        }

        private void btnStartGrab_Click(object sender, EventArgs e)
        {
            if (_pesVisionAnalysis.SelectedCamera != null)
            {
                _pesVisionAnalysis.SelectedCamera.StartGrab();
                btnStartGrab.Enabled = false;
                btnStopGrab.Enabled = true;
                btnSnapshot.Enabled = false;
                ShowSuccessTip("相机采集已打开");
                uiTabControl1.SelectedIndex = 0;
            }
            else
            {
                ShowErrorTip("当前无相机连接");
            }
        }

        private void btnStopGrab_Click(object sender, EventArgs e)
        {
            if (_pesVisionAnalysis.SelectedCamera != null)
            {
                _pesVisionAnalysis.SelectedCamera.StopGrab();
                btnStartGrab.Enabled = true;
                btnStopGrab.Enabled = false;
                btnSnapshot.Enabled = true;
                ShowSuccessTip("相机采集已关闭");
                uiTabControl1.SelectedIndex = 0;
            }
            else
            {
                ShowErrorTip("当前无相机连接");
            }
        }

        private void btnSnapshot_Click(object sender, EventArgs e)
        {
            if (_pesVisionAnalysis.SelectedCamera != null)
            {
                _pesVisionAnalysis.SelectedCamera.Capture(1);
                int mRow, mCol;
                var mat = _pesVisionAnalysis.SelectedCamera.GetImageFromBuff(0, out mRow, out mCol);
                var visionImg = CommonUtility.HikSdk.MyCamera.MatToVisionImage(mat);
                Algorithms.Copy(visionImg, _mainImageViewer.Image);
                visionImg.Dispose();
                _pesVisionAnalysis.SelectedCamera.ClearBuffer();
                uiTabControl1.SelectedIndex = 0;
            }
            else
            {
                ShowErrorTip("当前无相机连接");
            }
        }

        private void btnSetShutter_Click(object sender, EventArgs e)
        {
            if (_pesVisionAnalysis.SelectedCamera != null)
            {
                var option = new UIEditOption
                {
                    AutoLabelWidth = true,
                    Text = "设置曝光"
                };

                // PesVisionMain.SelectedCamera

                option.AddInteger("Shutter", "曝光时间", 50000);

                var frm = new UIEditForm(option);
                frm.Render();
                frm.ShowDialog();

                if (frm.IsOK)
                {
                    var shutter = (int)frm["Shutter"];
                    _pesVisionAnalysis.SelectedCamera.SetExposureTime(shutter);
                    ShowSuccessTip(string.Format("当前曝光时间已更新为：{0}", shutter));
                }
                else
                {
                    ShowInfoTip("操作已取消！");
                }
            }
            else
            {
                ShowErrorTip("当前无相机连接");
            }
        }

        private void btnLoadLocalLbImage_Click(object sender, EventArgs e)
        {
            var option = new UIEditOption
            {
                AutoLabelWidth = true,
                Text = "选取近光类型"
            };

            var lbType = new[] { "近光（用于识别截止线）", "近光" };
            option.AddCombobox("LbType", "近光类型", lbType, 0, true, false);

            var frm = new UIEditForm(option);
            frm.Render();
            frm.ShowDialog();

            if (frm.IsOK)
            {
                using (var ofd = new OpenFileDialog())
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        var typeIndex = (int)frm["LbType"];

                        if (typeIndex == 0)
                        {
                            _lowBeamImageViewer0.Image.ReadFile(ofd.FileName);
                        }
                        else
                        {
                            _lowBeamImageViewer1.Image.ReadFile(ofd.FileName);
                        }
                    }
                }
            }
            else
            {
                ShowInfoTip("操作已取消！");
            }
        }

        private void btnLoadLocalHbImage_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _highBeamImageViewer.Image.ReadFile(ofd.FileName);
                }
            }
        }

        private void MatCopyToImageViewer(Mat mat, ImageViewer imageViewer)
        {
            var visionImg = CommonUtility.HikSdk.MyCamera.MatToVisionImage(mat);
            Algorithms.Copy(visionImg, imageViewer.Image);
            visionImg.Dispose();
        }

        private void ClearImgaeViewer(ImageViewer imageViewer)
        {
            Algorithms.Copy(_defaultBlackVisionImage, imageViewer.Image);
        }

        private void ClearAnalysisImage(Control con)
        {
            foreach (Control c in con.Controls)
            {
                if (c is ImageViewer)
                {
                    var imgViewer = c as ImageViewer;
                    ClearImgaeViewer(imgViewer);
                }

                if (c.Controls.Count > 0)
                    ClearAnalysisImage(c);
            }
        }

        private void tsmiAddLowBeamHighShutter_Click(object sender, EventArgs e)
        {
            if (_mainImageViewer.Image != null &&
                _mainImageViewer.Image.Width != _defaultBlackMat.Width &&
                _mainImageViewer.Image.Height != _defaultBlackMat.Height)
            {
                var lb = CommonUtility.HikSdk.MyCamera.VisionImageToMat(_mainImageViewer.Image);
                _pesVisionAnalysis.ImgMats[PesVisionAnalysis.LowBeamSrc0Name] = lb.Clone();
                lb.Dispose();
                GC.Collect();
                uiTabControl1.SelectedIndex = 0;
            }
            else
            {
                ShowErrorTip("请先采集图像！");
            }
        }

        private void tsmiAddLowBeam_Click(object sender, EventArgs e)
        {
            if (_mainImageViewer.Image != null &&
                _mainImageViewer.Image.Width != _defaultBlackMat.Width &&
                _mainImageViewer.Image.Height != _defaultBlackMat.Height)
            {
                var lb = CommonUtility.HikSdk.MyCamera.VisionImageToMat(_mainImageViewer.Image);
                _pesVisionAnalysis.ImgMats[PesVisionAnalysis.LowBeamAnalysisResult1ImgName] = lb.Clone();
                lb.Dispose();
                GC.Collect();
                uiTabControl1.SelectedIndex = 0;
            }
            else
            {
                ShowErrorTip("请先采集图像！");
            }
        }

        private void tsmiAddHighBeam_Click(object sender, EventArgs e)
        {
            if (_mainImageViewer.Image != null &&
                _mainImageViewer.Image.Width != _defaultBlackMat.Width &&
                _mainImageViewer.Image.Height != _defaultBlackMat.Height)
            {
                var hb = CommonUtility.HikSdk.MyCamera.VisionImageToMat(_mainImageViewer.Image);
                _pesVisionAnalysis.ImgMats[PesVisionAnalysis.HighBeamAnalysisResultImgName] = hb.Clone();
                hb.Dispose();
                GC.Collect();
                uiTabControl1.SelectedIndex = 0;
            }
            else
            {
                ShowErrorTip("请先采集图像！");
            }
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            if (_lowBeamImageViewer0.Image.Width != _defaultBlackMat.Width &&
                _lowBeamImageViewer0.Image.Height != _defaultBlackMat.Height)
            {
                var lb0 = CommonUtility.HikSdk.MyCamera.VisionImageToMat(_lowBeamImageViewer0.Image);
                _pesVisionAnalysis.ImgMats[PesVisionAnalysis.LowBeamSrc0Name] = lb0.Clone();
                lb0.Dispose();
            }

            if (_lowBeamImageViewer1.Image.Width != _defaultBlackMat.Width &&
                _lowBeamImageViewer1.Image.Height != _defaultBlackMat.Height)
            {
                var lb1 = CommonUtility.HikSdk.MyCamera.VisionImageToMat(_lowBeamImageViewer1.Image);
                _pesVisionAnalysis.ImgMats[PesVisionAnalysis.LowBeamSrc1Name] = lb1.Clone();
                _pesVisionAnalysis.ImgMats[PesVisionAnalysis.LowBeamSrc2Name] = lb1.Clone(); // debug // 20240924
                lb1.Dispose();
            }

            if (_highBeamImageViewer.Image.Width != _defaultBlackMat.Width &&
                _highBeamImageViewer.Image.Height != _defaultBlackMat.Height)
            {
                var hb = CommonUtility.HikSdk.MyCamera.VisionImageToMat(_highBeamImageViewer.Image);
                _pesVisionAnalysis.ImgMats[PesVisionAnalysis.HighBeamSrcName] = hb.Clone();
                hb.Dispose();
            }

            uiRichTextBox1.Clear();
            ClearAnalysisImage(analysisFlowPanel);
            _pesVisionAnalysis.VisionAnalysis();
            foreach (var t in _pesVisionAnalysis.ImgMats.Where(t => t.Value != null && !t.Value.Empty()))
                MatCopyToImageViewer(t.Value, _imageViewers[t.Key]);
            uiRichTextBox1.AppendText(_pesVisionAnalysis.VisionAnalysisMsg);
            _pesVisionAnalysis.Release();

            uiTabControl1.SelectedIndex = 1;
        }
    }
}
