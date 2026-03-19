using Controller;
using NationalInstruments.Vision.Analysis;
using NationalInstruments.Vision.WindowsForms;
using OpenCvSharp;
using Sunny.UI;
using System;
using System.Windows.Forms;

namespace CheckSystem.PesVision
{
    public partial class PesServoCali : UIForm
    {
        private readonly PesVisionAnalysisNDLB _pesVisionAnalysis;
        private readonly PesControlPlc _pesControlPlc;
        private readonly ImageViewer _imageViewer = new ImageViewer { Dock = DockStyle.Fill };

        public PesServoCali(PesVisionAnalysisNDLB pesVisionAnalysis, PesControlPlc pesControlPlc)
        {
            InitializeComponent();
            _pesVisionAnalysis = pesVisionAnalysis;
            _pesControlPlc = pesControlPlc;
            _pesVisionAnalysis.IsInServoCali = true;
            Load += PesServoCali_Load;
            Closed += PesServoCali_Closed;
        }

        private void PesServoCali_Closed(object sender, EventArgs e)
        {
            _pesVisionAnalysis.IsInServoCali = false;
            CommonUtility.HikSdk.MyCamera.PushMat -= MyCamera_PushMat;
        }

        private void PesServoCali_Load(object sender, EventArgs e)
        {
            uiPanel1.Controls.Add(_imageViewer);
            ImageShowTool(_imageViewer);
            _imageViewer.SizeChanged += imageViewer_SizeChanged;
            CommonUtility.HikSdk.MyCamera.PushMat += MyCamera_PushMat;
        }

        private void ImageShowTool(ImageViewer imageViewer)
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
            var showMat = mat.Clone();
            //DrawCircle(_pesVisionAnalysis.Ph0V0, showMat);
            //DrawCircle(_pesVisionAnalysis.Ph0V2, showMat);
            //DrawCircle(_pesVisionAnalysis.Ph0V4, showMat);
            //DrawCircle(_pesVisionAnalysis.PHn8V0, showMat);
            //DrawCircle(_pesVisionAnalysis.PHn8V4, showMat);
            //DrawCircle(_pesVisionAnalysis.PHn4V2, showMat);
            //DrawCircle(_pesVisionAnalysis.PHn4V0, showMat);
            //DrawCircle(_pesVisionAnalysis.Ph4V2, showMat);
            //DrawCircle(_pesVisionAnalysis.Ph8V4, showMat);

            //DrawCircle(_pesVisionAnalysis.Ph0Vn11, showMat);
            //DrawCircle(_pesVisionAnalysis.PHn11V0, showMat);
            //DrawCircle(_pesVisionAnalysis.Ph11V0, showMat);
            //DrawCircle(_pesVisionAnalysis.PHn20V0, showMat);
            //DrawCircle(_pesVisionAnalysis.Ph20V0, showMat);

            var visionImg = CommonUtility.HikSdk.MyCamera.MatToVisionImage(showMat);
            Algorithms.Copy(visionImg, _imageViewer.Image);
            visionImg.Dispose();
            mat.Dispose();
            showMat.Dispose();
            GC.Collect();
        }

        private void DrawCircle(Point point, Mat src)
        {
            if (point != default)
            {
                Cv2.Circle(src, point, 1, Scalar.Red, -1);
                var pName = string.Format("[{0}.{1}]", point.X, point.Y);
                Cv2.PutText(src, pName, new Point(point.X + 1, point.Y - 1 - 10), HersheyFonts.HersheySimplex, 0.3, Scalar.Red);
            }
        }

        private void txtX_ValueChanged(object sender, EventArgs e)
        {
            _pesControlPlc.PcWriteServoXPos = (float)txtX.Value;
        }

        private void txtY_ValueChanged(object sender, EventArgs e)
        {
            _pesControlPlc.PcWriteServoZPos = (float)txtY.Value;
        }
    }
}
