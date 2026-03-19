using CommonUtility;
using CommonUtility.FileOperator;
using CommonUtility.HikSdk;
using NationalInstruments.Vision;
using NationalInstruments.Vision.Analysis;
using NationalInstruments.Vision.WindowsForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckSystem.CcdForms
{
    public partial class FormLedParaSet : Form
    {
        internal enum SetStep
        {
            /// <summary>
            /// 选取图片
            /// </summary>
            [Description("选取图片")]
            Step0,

            /// <summary>
            /// 图像处理
            /// </summary>
            [Description("图像处理")]
            Step1,

            /// <summary>
            /// 设置动画
            /// </summary>
            [Description("设置动画")]
            Step2,

            /// <summary>
            /// 标定图像
            /// </summary>
            [Description("标定图像")]
            Step3,

            /// <summary>
            /// 保存参数
            /// </summary>
            [Description("保存参数")]
            Step4
        }

        private string XmlFilePath { get; set; }
        private string FuncName { get; set; }
        private SetStep CurrentStep { get; set; }
        private Point CenterPoint { get; set; }
        private RectangleContour TemplateRectangle { get; set; }
        private VisionImage TemplateImage { get; set; }
        //private readonly DahengCameraClass _daheng = new DahengCameraClass();
        private readonly CameraControl _camera;

        private readonly List<VisionImage> _imgList = new List<VisionImage>();
        private readonly List<KeyValuePair<int, LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroup>> _animationImgLst =
            new List<KeyValuePair<int, LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroup>>();
        private readonly List<LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroupShape> _cpoyShapes =
            new List<LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroupShape>();
        private int _currentIndexGroupIndex;

        private bool _isSnapShotExecute;
        private int _snapShotCount;
        private readonly List<VisionImage> _listCaptureImgs = new List<VisionImage>();

        private bool _isAutoGetGray = true;

        public FormLedParaSet(string xmlFilePath, string funcName, CameraControl camera = null)
        {
            InitializeComponent();
            if (camera == null)
            {
                _camera = new CameraControl();
            }
            else
            {
                _camera = camera;
            }

            WindowState = FormWindowState.Maximized;
            CheckForIllegalCrossThreadCalls = false;
            MaximumSize = new Size(Size.Width + 200, Size.Height + 200);
            MinimumSize = new Size(Size.Width + 200, Size.Height + 200);
            lblWithCmbStepsList.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            lblWithCmbStepsList.comboBox.Enabled = false;
            //lblWithCmbStepsList.BackColor = Color.DarkGoldenrod;
            Load += FormLedParaSetMultiplePic_Load;
            Closed += FormLedParaSet_Closed;
            toolStripCmbImgList.SelectedIndexChanged += toolStripCmbImgList_SelectedIndexChanged;
            XmlFilePath = xmlFilePath;

            funcNameTxt.Text = FuncName = funcName;

            userDataGrid.label.Text = @"参数列表";
            userDataGrid.label.Height = 30;

            userDataGrid.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            userDataGrid.dataGridView.AllowUserToResizeColumns = true;
            userDataGrid.dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            //userDataGridGrayList.dataGridView.EditMode = DataGridViewEditMode.EditOnEnter;
            userDataGrid.dataGridView.Margin = new Padding(3, 4, 3, 4);
            userDataGrid.dataGridView.RowTemplate.Height = 30;
            userDataGrid.dataGridView.RowsDefaultCellStyle.Font = new Font("微软雅黑", 12, FontStyle.Regular);
            userDataGrid.dataGridView.EnableHeadersVisualStyles = false;
            userDataGrid.dataGridView.RowHeadersVisible = false;
            userDataGrid.dataGridView.AllowUserToDeleteRows = false;
            userDataGrid.dataGridView.AllowUserToAddRows = false;
            userDataGrid.dataGridView.BackgroundColor = SystemColors.ActiveCaption;
            userDataGrid.dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSkyBlue;
            userDataGrid.dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.LightYellow;

            //获取控件的Type,设置双缓存
            var dgvType = userDataGrid.dataGridView.GetType();
            var properInfo = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            //为控件的属性设置值
            if (properInfo != null) properInfo.SetValue(userDataGrid.dataGridView, true, null);

            if (toolStripComboBoxColorPlaneExtraction.ComboBox != null)
            {
                toolStripComboBoxColorPlaneExtraction.DropDownStyle = ComboBoxStyle.DropDownList;
                toolStripComboBoxColorPlaneExtraction.ComboBox.DataSource =
                    EnumOperater.GetEnumValueList<ImageProcessing.ColorPlaneExtractionType>();
                toolStripComboBoxColorPlaneExtraction.SelectedIndexChanged +=
                    ColorPlaneExtractionComboBox_SelectedIndexChanged;
            }

            if (toolStripComboBoxLookupTable.ComboBox != null)
            {
                toolStripComboBoxLookupTable.DropDownStyle = ComboBoxStyle.DropDownList;
                toolStripComboBoxLookupTable.ComboBox.DataSource =
                    EnumOperater.GetEnumValueList<ImageProcessing.LookupTableType>();
                toolStripComboBoxLookupTable.SelectedIndexChanged += LookupTableComboBox_SelectedIndexChanged;
            }

            lblWithCmbStepsList.comboBox.Items.Add(
                CurrentStep.GetCustomAttribute<DescriptionAttribute>().Description);
            lblWithCmbStepsList.comboBox.SelectedIndex = 0;

            if (toolStripCmbAnimationSpeed.ComboBox != null)
            {
                toolStripCmbAnimationSpeed.ComboBox.Items.Add(1);
                toolStripCmbAnimationSpeed.ComboBox.Items.Add(10);
                toolStripCmbAnimationSpeed.ComboBox.Items.Add(15);
                toolStripCmbAnimationSpeed.ComboBox.Items.Add(25);
                toolStripCmbAnimationSpeed.ComboBox.Items.Add(50);
                toolStripCmbAnimationSpeed.ComboBox.Items.Add(100);
                toolStripCmbAnimationSpeed.ComboBox.Items.Add(200);
                toolStripCmbAnimationSpeed.ComboBox.Items.Add(250);
                toolStripCmbAnimationSpeed.ComboBox.Items.Add(500);
                toolStripCmbAnimationSpeed.ComboBox.Items.Add(1000);
                toolStripCmbAnimationSpeed.ComboBox.SelectedIndex = 4;
                toolStripCmbAnimationSpeed.ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            }

            if (toolStripCmbRadiusRange.ComboBox != null)
            {
                toolStripCmbRadiusRange.ComboBox.Items.Add(5);
                toolStripCmbRadiusRange.ComboBox.Items.Add(6);
                toolStripCmbRadiusRange.ComboBox.Items.Add(7);
                toolStripCmbRadiusRange.ComboBox.Items.Add(8);
                toolStripCmbRadiusRange.ComboBox.Items.Add(9);
                toolStripCmbRadiusRange.ComboBox.Items.Add(10);
                toolStripCmbRadiusRange.ComboBox.SelectedIndex = 0;
                toolStripCmbRadiusRange.ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            }

            if (toolStripCmbRrayPercent.ComboBox != null)
            {
                toolStripCmbRrayPercent.ComboBox.Items.Add("±40%");
                toolStripCmbRrayPercent.ComboBox.Items.Add("±35%");
                toolStripCmbRrayPercent.ComboBox.Items.Add("±30%");
                toolStripCmbRrayPercent.ComboBox.Items.Add("±25%");
                toolStripCmbRrayPercent.ComboBox.Items.Add("±20%");
                toolStripCmbRrayPercent.ComboBox.Items.Add("±15%");
                toolStripCmbRrayPercent.ComboBox.Items.Add("±10%");
                toolStripCmbRrayPercent.ComboBox.SelectedIndex = 0;
                toolStripCmbRrayPercent.ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            }
        }

        private void FormLedParaSetMultiplePic_Load(object sender, EventArgs e)
        {
            ImageViewer.Dock = DockStyle.Fill;
            ImageViewer.ToolsShown = ViewerTools.ZoomIn |
                                     ViewerTools.ZoomOut |
                                     ViewerTools.Pan |
                                     ViewerTools.Selection;
            ImageViewer.ActiveTool = ViewerTools.Selection;
            ImageViewer.ZoomToFit = true;
            ImageViewer.ShowToolbar = true;
            ImageViewer.ShowScrollbars = true;
            ImageViewer.ShowImageInfo = true;
            ImageViewer.RoiChanged += ImageViewer_RoiChanged;

            ChangetStep(SetStep.Step0, CurrentStep);

            var ledChekPara = XmlHelper.Deserialize<LedCheckPara>(XmlFilePath);
            var listFuncs = new List<LedCheckParaVisionFuncsVisionFunc>();
            if (ledChekPara.VisionFuncs.VisionFunc != null)
                listFuncs.AddRange(ledChekPara.VisionFuncs.VisionFunc);
            var findFuncIndex = listFuncs.FindIndex(f => f.FuncName.Equals(FuncName));
            //cameraDelayTxt.Text = listFuncs[findFuncIndex].CameraPara.DelayTime.ToString();
            //cameraFrameCountTxt.Text = listFuncs[findFuncIndex].CameraPara.FrameCount.ToString();
            //cameraFrameRateTxt.Text = listFuncs[findFuncIndex].CameraPara.FrameRate.ToString();
            //cameraShutterTxt.Text = listFuncs[findFuncIndex].CameraPara.Shutter.ToString();

            cameraSnTxt.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            cameraDelayTxt.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            cameraFrameCountTxt.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            cameraFrameRateTxt.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            cameraShutterTxt.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            _camera.OpenAllCamera();
            //_daheng.OpenDevices();
            DahengCameraClass.PushImageBytes += DahengCameraClass_PushImageBytes;
            MyCamera.PushMat += MyCamera_PushMat;

            //foreach (var t in DahengCameraClass.MListCCamerInfo)
            //    cameraSnTxt.comboBox.Items.Add(t.MStrSn);

            foreach (var t in _camera.CameraList)
            {
                cameraSnTxt.comboBox.Items.Add(t.GigeInfo.chSerialNumber);
            }

            cameraSnTxt.comboBox.SelectedIndexChanged += cameraSnComboBox_SelectedIndexChanged;
            if (cameraSnTxt.comboBox.Items.Count > 0)
            {
                cameraSnTxt.comboBox.SelectedIndex = 0;
                for (var i = 0; i < cameraSnTxt.comboBox.Items.Count; i++)
                {
                    if (cameraSnTxt.comboBox.Items[i].ToString() != listFuncs[findFuncIndex].CameraPara.CameraSn)
                        continue;
                    cameraSnTxt.comboBox.SelectedIndex = i;
                    break;
                }
            }

            cameraShutterTxt.comboBox.Items.Add(50);
            if (cameraShutterTxt.comboBox != null)
            {
                for (var i = 0; i < 2000; i++)
                    cameraShutterTxt.comboBox.Items.Add(500 + 500 * i);

                cameraShutterTxt.comboBox.SelectedIndexChanged += cameraShutterComboBox_SelectedIndexChanged;
                cameraShutterTxt.comboBox.SelectedIndex = 0;
                for (var i = 0; i < cameraShutterTxt.comboBox.Items.Count; i++)
                {
                    if (cameraShutterTxt.comboBox.Items[i].ToString() !=
                        listFuncs[findFuncIndex].CameraPara.Shutter.ToString())
                        continue;

                    cameraShutterTxt.comboBox.SelectedIndex = i;
                    break;
                }
            }

            if (cameraFrameRateTxt.comboBox != null)
            {
                cameraFrameRateTxt.comboBox.Items.Add("自动帧率");
                for (var i = 1; i < 144; i++)
                    cameraFrameRateTxt.comboBox.Items.Add(i);

                cameraFrameRateTxt.comboBox.SelectedIndexChanged += cameraFrameRateComboBox_SelectedIndexChanged;

                if (listFuncs[findFuncIndex].CameraPara.FrameRate == 0)
                    cameraFrameRateTxt.comboBox.SelectedIndex = 0;
                else
                {
                    for (var i = 0; i < cameraFrameRateTxt.comboBox.Items.Count; i++)
                    {
                        if (cameraFrameRateTxt.comboBox.Items[i].ToString() !=
                            listFuncs[findFuncIndex].CameraPara.FrameRate.ToString())
                            continue;

                        cameraFrameRateTxt.comboBox.SelectedIndex = i;
                        break;
                    }
                }
            }

            if (cameraDelayTxt.comboBox != null)
            {
                for (var i = 0; i < 100; i++)
                    cameraDelayTxt.comboBox.Items.Add(10 + 50 * i);

                cameraDelayTxt.comboBox.SelectedIndex = 0;
                for (var i = 0; i < cameraDelayTxt.comboBox.Items.Count; i++)
                {
                    if (cameraDelayTxt.comboBox.Items[i].ToString() !=
                        listFuncs[findFuncIndex].CameraPara.DelayTime.ToString())
                        continue;

                    cameraDelayTxt.comboBox.SelectedIndex = i;
                    break;
                }
            }

            if (cameraFrameCountTxt.comboBox != null)
            {
                for (var i = 1; i <= 300; i++)
                    cameraFrameCountTxt.comboBox.Items.Add(i);

                cameraFrameCountTxt.comboBox.SelectedIndex = 0;
                for (var i = 0; i < cameraFrameCountTxt.comboBox.Items.Count; i++)
                {
                    if (cameraFrameCountTxt.comboBox.Items[i].ToString() !=
                        listFuncs[findFuncIndex].CameraPara.FrameCount.ToString())
                        continue;

                    cameraFrameCountTxt.comboBox.SelectedIndex = i;
                    break;
                }
            }
        }

        private void cameraSnComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (toolStripBtnAcquisition.Text != @"停止采集" ||
                string.IsNullOrEmpty(cameraSnTxt.comboBox.Text))
                return;

            foreach (var c in _camera.CameraList)
            {
                if (c.GigeInfo.chSerialNumber == cameraSnTxt.comboBox.Text)
                {
                    c.StartGrab();
                }
                else
                {
                    c.StopGrab();
                }
            }

            //foreach (var c in DahengCameraClass.MListCCamerInfo)
            //    _daheng.AcquisitionStop(c.MStrSn);

            //_daheng.AcquisitionStart(cameraSnTxt.comboBox.Text);
            cameraShutterComboBox_SelectedIndexChanged(null, null);
            cameraFrameRateComboBox_SelectedIndexChanged(null, null);
        }

        private void cameraFrameRateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (toolStripBtnAcquisition.Text == @"停止采集" && !string.IsNullOrEmpty(cameraSnTxt.comboBox.Text))
            {
                if (cameraFrameRateTxt.comboBox.SelectedIndex == 0)
                    DahengCameraClass.CloseFrameRateMode(cameraSnTxt.comboBox.Text);
                else
                    DahengCameraClass.OpenFrameRateMode(cameraSnTxt.comboBox.Text, int.Parse(cameraFrameRateTxt.comboBox.Text));
            }
        }

        private void cameraShutterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (toolStripBtnAcquisition.Text == @"停止采集" && !string.IsNullOrEmpty(cameraSnTxt.comboBox.Text))
            {
                var sn = cameraSnTxt.comboBox.Text;
                var value = int.Parse(cameraShutterTxt.comboBox.Text);
                var device = _camera.CameraList.Find(f => f.GigeInfo.chSerialNumber == sn);
                if (device != null)
                    device.SetExposureTime(value);

                //DahengCameraClass.SetShutter(cameraSnTxt.comboBox.Text, int.Parse(cameraShutterTxt.comboBox.Text));
            }
        }

        private void FormLedParaSet_Closed(object sender, EventArgs e)
        {
            DahengCameraClass.PushImageBytes -= DahengCameraClass_PushImageBytes;
            MyCamera.PushMat -= MyCamera_PushMat;
            //_daheng.CloseDevices();
            _camera.CloseAllCamera();
        }

        private void DahengCameraClass_PushImageBytes(
            byte[] imgBytes, int width, int heigh, bool iscolor)
        {
            var img = ImageProcessing.ConvertBytesToVisionImg(imgBytes, width, heigh, iscolor);
            Algorithms.Copy(img, ImageViewer.Image);

            if (_isSnapShotExecute)
            {
                var captureImg = new VisionImage();
                Algorithms.Copy(img, captureImg);

                _listCaptureImgs.Add(captureImg);
                cameraFrameCountTxt.comboBox.SelectedIndex = _listCaptureImgs.Count - 1;

                if (_listCaptureImgs.Count == _snapShotCount)
                {
                    _isSnapShotExecute = false;
                    _snapShotCount = 0;

                    var thisProgramFolder = new FileInfo(XmlFilePath).Directory;
                    var captureImageFolder = string.Format(@"{0}\{1}-CaptureImage", thisProgramFolder, FuncName);
                    if (!Directory.Exists(captureImageFolder))
                        Directory.CreateDirectory(captureImageFolder);
                    var caliImageFolder = string.Format(@"{0}\标定图片", captureImageFolder);
                    if (!Directory.Exists(caliImageFolder))
                        Directory.CreateDirectory(caliImageFolder);

                    for (var i = 0; i < _listCaptureImgs.Count; i++)
                    {
                        _listCaptureImgs[i].WriteBmpFile(string.Format(@"{0}\Image{1}.bmp", caliImageFolder, i));
                        _listCaptureImgs[i].Dispose();
                    }

                    _listCaptureImgs.Clear();
                }
            }

            img.Dispose();
        }

        private void MyCamera_PushMat(MvCameraSdk.MV_GIGE_DEVICE_INFO gigeInfo, global::OpenCvSharp.Mat mat)
        {
            var img = MyCamera.MatToVisionImage(mat);
            Algorithms.Copy(img, ImageViewer.Image);

            if (_isSnapShotExecute)
            {
                var captureImg = new VisionImage();
                Algorithms.Copy(img, captureImg);

                _listCaptureImgs.Add(captureImg);
                cameraFrameCountTxt.comboBox.SelectedIndex = _listCaptureImgs.Count - 1;

                if (_listCaptureImgs.Count == _snapShotCount)
                {
                    _isSnapShotExecute = false;
                    _snapShotCount = 0;

                    var thisProgramFolder = new FileInfo(XmlFilePath).Directory;
                    var captureImageFolder = string.Format(@"{0}\{1}-CaptureImage", thisProgramFolder, FuncName);
                    if (!Directory.Exists(captureImageFolder))
                        Directory.CreateDirectory(captureImageFolder);
                    var caliImageFolder = string.Format(@"{0}\标定图片", captureImageFolder);
                    if (!Directory.Exists(caliImageFolder))
                        Directory.CreateDirectory(caliImageFolder);

                    for (var i = 0; i < _listCaptureImgs.Count; i++)
                    {
                        _listCaptureImgs[i].WriteBmpFile(string.Format(@"{0}\Image{1}.bmp", caliImageFolder, i));
                        _listCaptureImgs[i].Dispose();
                    }

                    _listCaptureImgs.Clear();
                }
            }

            img.Dispose();
        }

        private void ImageViewer_RoiChanged(
            object sender, ContoursChangedEventArgs e)
        {
            if (ImageViewer.Roi.Count == 0)
                ImageProcessing.DrawContourCountInOverlay(ImageViewer.Image, ImageViewer.Roi);

            if (CurrentStep == SetStep.Step3 && _isAutoGetGray)
            {
                ImageProcessing.DrawContourCountInOverlay(ImageViewer.Image, ImageViewer.Roi);
                toolStripBtnShowGray_Click(null, null);
            }
        }

        private void ColorPlaneExtractionComboBox_SelectedIndexChanged(
           object sender, EventArgs e)
        {
            if (toolStripComboBoxColorPlaneExtraction == null)
                return;

            using (var srcImg = new VisionImage(_imgList[toolStripCmbImgList.SelectedIndex].Type))
            {
                Algorithms.Copy(_imgList[toolStripCmbImgList.SelectedIndex], srcImg);

                var colorTypeToExtraction =
                    EnumOperater.GetEnumByValue<ImageProcessing.ColorPlaneExtractionType>(
                        toolStripComboBoxColorPlaneExtraction.Text);

                var colorExtractedImg = ImageProcessing.ColorPlaneExtraction(srcImg, colorTypeToExtraction);
                Algorithms.Copy(colorExtractedImg, ImageViewer.Image);
                colorExtractedImg.Dispose();

                toolStripComboBoxLookupTable.SelectedIndex = 0;
            }
        }

        private void LookupTableComboBox_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            if (EnumOperater.GetEnumByValue<ImageProcessing.ColorPlaneExtractionType>(
                toolStripComboBoxColorPlaneExtraction.Text) ==
                ImageProcessing.ColorPlaneExtractionType.Default)
            {
                toolStripComboBoxLookupTable.SelectedIndex = 0;
                return;
            }

            using (var srcImg = new VisionImage(_imgList[toolStripCmbImgList.SelectedIndex].Type))
            {
                Algorithms.Copy(_imgList[toolStripCmbImgList.SelectedIndex], srcImg);

                var colorTypeToExtraction =
                        EnumOperater.GetEnumByValue<ImageProcessing.ColorPlaneExtractionType>(
                            toolStripComboBoxColorPlaneExtraction.Text);
                var colorExtractedImg = ImageProcessing.ColorPlaneExtraction(srcImg, colorTypeToExtraction);
                Algorithms.Copy(colorExtractedImg, srcImg);
                colorExtractedImg.Dispose();

                var typeToLookup =
                       EnumOperater.GetEnumByValue<ImageProcessing.LookupTableType>(
                           toolStripComboBoxLookupTable.Text);
                var lookupImg = ImageProcessing.LookupTable(srcImg, typeToLookup);
                Algorithms.Copy(lookupImg, srcImg);
                lookupImg.Dispose();

                Algorithms.Copy(srcImg, ImageViewer.Image);
            }
        }

        private void toolStripCmbImgList_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            using (var srcImg = new VisionImage(_imgList[toolStripCmbImgList.SelectedIndex].Type))
            {
                Algorithms.Copy(_imgList[toolStripCmbImgList.SelectedIndex], srcImg);

                var colorTypeToExtraction =
                        EnumOperater.GetEnumByValue<ImageProcessing.ColorPlaneExtractionType>(
                            toolStripComboBoxColorPlaneExtraction.Text);
                var colorExtractedImg = ImageProcessing.ColorPlaneExtraction(srcImg, colorTypeToExtraction);
                Algorithms.Copy(colorExtractedImg, srcImg);
                colorExtractedImg.Dispose();

                var typeToLookup =
                       EnumOperater.GetEnumByValue<ImageProcessing.LookupTableType>(
                           toolStripComboBoxLookupTable.Text);
                var lookupImg = ImageProcessing.LookupTable(srcImg, typeToLookup);
                Algorithms.Copy(lookupImg, srcImg);
                lookupImg.Dispose();

                Algorithms.Copy(srcImg, ImageViewer.Image);
            }
        }

        public sealed override Size MinimumSize
        {
            get { return base.MinimumSize; }
            set { base.MinimumSize = value; }
        }

        public sealed override Size MaximumSize
        {
            get { return base.MaximumSize; }
            set { base.MaximumSize = value; }
        }

        /// <summary>
        /// 切换步骤
        /// </summary>
        /// <param name="srcStep"></param>
        /// <param name="desSetStep"></param>
        private void ChangetStep(SetStep srcStep, SetStep desSetStep)
        {
            //toolStripCmbImgList

            //toolStripBtnShowGray
            //toolStripBtnOpenPics

            //toolStripButtonAddRect
            //toolStripButtonAddShape

            //toolStripLabel2
            //toolStripComboBoxColorPlaneExtraction

            //toolStripLabel3
            //toolStripComboBoxLookupTable

            //toolStripBtnAddAnimation
            //toolStripBtnLastAnimation
            //toolStripBtnNextAnimation
            //toolStripBtnFindCircle
            //toolStripBtnCopyRoi
            //toolStripBtnPasteRoi

            //toolStripBtnShowAnimation
            //toolStripLabel1
            //toolStripCmbAnimationSpeed

            var filePath = XmlFilePath;
            var ledChekPara = XmlHelper.Deserialize<LedCheckPara>(filePath);

            var listFuncs = new List<LedCheckParaVisionFuncsVisionFunc>();
            if (ledChekPara.VisionFuncs.VisionFunc != null)
                listFuncs.AddRange(ledChekPara.VisionFuncs.VisionFunc);

            var findFuncIndex = listFuncs.FindIndex(f => f.FuncName.Equals(FuncName));

            switch (desSetStep)
            {
                case SetStep.Step0:
                    //foreach (var t in _imgList)
                    //    t.Dispose();
                    //_imgList.Clear();

                    toolStripBtnOpenPics.Visible = true;
                    toolStripBtnAcquisition.Visible = true;
                    toolStripBtnCapture.Visible = true;
                    toolStripBtnOpenControllerDebuger.Visible = true;

                    toolStripCmbImgList.Enabled = true;
                    if (toolStripCmbImgList.Items.Count > 0)
                        toolStripCmbImgList.SelectedIndex = 0;

                    toolStripButtonAddRect.Visible = false;
                    toolStripButtonAddShape.Visible = false;

                    toolStripLabel2.Visible = false;
                    toolStripComboBoxColorPlaneExtraction.Enabled = false;
                    toolStripComboBoxColorPlaneExtraction.SelectedIndex = 0;
                    toolStripLabel3.Visible = false;
                    toolStripComboBoxLookupTable.Enabled = false;
                    toolStripComboBoxLookupTable.SelectedIndex = 0;

                    toolStripBtnStopAutoGetRrays.Visible = false;
                    toolStripBtnGetRrays.Visible = false;
                    toolStripCmbRrayPercent.Visible = false;

                    toolStripBtnAddAnimation.Visible = false;
                    toolStripBtnClearAnimation.Visible = false;
                    toolStripBtnLastAnimation.Visible = false;
                    toolStripBtnNextAnimation.Visible = false;

                    toolStripBtnFindCircle.Visible = false;
                    toolStripCmbRadiusRange.Visible = false;
                    toolStripButtonCopyFrom.Visible = false;

                    toolStripBtnCopyRoi.Visible = false;
                    toolStripBtnPasteRoi.Visible = false;
                    toolStripButtonDeleteRect.Visible = false;

                    toolStripLabel2.Visible = false;
                    toolStripComboBoxColorPlaneExtraction.Visible = false;

                    toolStripLabel3.Visible = false;
                    toolStripComboBoxLookupTable.Visible = false;

                    toolStripBtnShowAnimation.Visible = false;
                    toolStripLabel1.Visible = false;
                    toolStripCmbAnimationSpeed.Visible = false;

                    btnSaveParas.Enabled = false;
                    btnSaveParas.BackColor = Color.DarkGoldenrod;

                    ImageViewer.Roi.Clear();

                    userDataGrid.dataGridView.Rows.Clear();
                    userDataGrid.dataGridView.Columns.Clear();
                    break;

                case SetStep.Step1:
                    if (_imgList.Count == 0)
                    {
                        MessageBox.Show(@"请先选择图片");
                        return;
                    }

                    toolStripBtnOpenPics.Visible = false;
                    toolStripBtnAcquisition.Visible = false;
                    toolStripBtnCapture.Visible = false;
                    toolStripBtnOpenControllerDebuger.Visible = false;

                    toolStripCmbImgList.Enabled = true;
                    toolStripCmbImgList.SelectedIndex = 0;

                    toolStripButtonAddRect.Visible = false;
                    toolStripButtonAddShape.Visible = false;

                    toolStripLabel2.Visible = true;
                    toolStripComboBoxColorPlaneExtraction.Visible = true;
                    toolStripComboBoxColorPlaneExtraction.Enabled = true;
                    toolStripComboBoxColorPlaneExtraction.SelectedIndex =
                         (int)
                             EnumOperater.GetEnumByValue<ImageProcessing.ColorPlaneExtractionType>(
                                 listFuncs[findFuncIndex].VisionPara.ColorPlaneExtraction);

                    toolStripLabel3.Visible = true;
                    toolStripComboBoxLookupTable.Visible = true;
                    toolStripComboBoxLookupTable.Enabled = true;
                    toolStripComboBoxLookupTable.SelectedIndex = (int)
                            EnumOperater.GetEnumByValue<ImageProcessing.LookupTableType>(
                                listFuncs[findFuncIndex].VisionPara.LookupTable);

                    toolStripBtnStopAutoGetRrays.Visible = false;
                    toolStripBtnGetRrays.Visible = false;
                    toolStripCmbRrayPercent.Visible = false;

                    toolStripBtnAddAnimation.Visible = false;
                    toolStripBtnClearAnimation.Visible = false;
                    toolStripBtnLastAnimation.Visible = false;
                    toolStripBtnNextAnimation.Visible = false;

                    toolStripBtnFindCircle.Visible = false;
                    toolStripCmbRadiusRange.Visible = false;
                    toolStripButtonCopyFrom.Visible = false;

                    toolStripBtnCopyRoi.Visible = false;
                    toolStripBtnPasteRoi.Visible = false;
                    toolStripButtonDeleteRect.Visible = false;

                    if (_imgList.Count == 1)
                    {
                        toolStripBtnShowAnimation.Visible = false;
                        toolStripLabel1.Visible = false;
                        toolStripCmbAnimationSpeed.Visible = false;
                    }
                    else
                    {
                        toolStripBtnShowAnimation.Visible = true;
                        toolStripLabel1.Visible = true;
                        toolStripCmbAnimationSpeed.Visible = true;
                    }

                    btnSaveParas.Enabled = false;
                    btnSaveParas.BackColor = Color.DarkGoldenrod;

                    ImageViewer.Roi.Clear();

                    userDataGrid.dataGridView.Rows.Clear();
                    userDataGrid.dataGridView.Columns.Clear();
                    break;

                case SetStep.Step2:
                    if (
                        EnumOperater.GetEnum<ImageProcessing.ColorPlaneExtractionType>(
                            toolStripComboBoxColorPlaneExtraction.Text) ==
                        ImageProcessing.ColorPlaneExtractionType.Default)
                    {
                        MessageBox.Show(@"请先提取色素");
                        return;
                    }

                    #region 存储模板
                    if (srcStep == SetStep.Step1)
                    {
                        TemplateImage = new VisionImage(ImageViewer.Image.Type);
                        var roi = new RectangleContour(0, 0, ImageViewer.Image.Width, ImageViewer.Image.Height);
                        Algorithms.Extract(ImageViewer.Image, TemplateImage, roi);

                        var left = roi.Left;
                        left = left * 0.8;
                        var top = roi.Top;
                        top = top * 0.8;
                        var width = roi.Width;
                        width = width * 1.2;
                        var height = roi.Height;
                        height = height * 1.2;

                        TemplateRectangle = new RectangleContour((int)left, (int)top, (int)width, (int)height);

                        var centerX = roi.Left + 0.5 * roi.Width;
                        var centerY = roi.Top + 0.5 * roi.Height;
                        CenterPoint = new Point((int)centerX, (int)centerY);
                    }
                    #endregion

                    if (_imgList.Count == 1 && srcStep == SetStep.Step1)
                    {
                        // ReSharper disable once TailRecursiveCall
                        ChangetStep(SetStep.Step2, SetStep.Step3);
                        return;
                    }

                    if (_imgList.Count == 1 && srcStep == SetStep.Step3)
                    {
                        // ReSharper disable once TailRecursiveCall
                        ChangetStep(SetStep.Step2, SetStep.Step1);
                        return;
                    }

                    #region 刷新界面显示设置动画
                    toolStripCmbImgList.Enabled = true;
                    toolStripCmbImgList.SelectedIndex = 0;

                    toolStripButtonAddRect.Visible = false;
                    toolStripButtonAddShape.Visible = false;

                    toolStripLabel2.Visible = false;
                    toolStripComboBoxColorPlaneExtraction.Visible = false;
                    toolStripLabel3.Visible = false;
                    toolStripComboBoxLookupTable.Visible = false;

                    //toolStripBtnShowGray.Visible = false;
                    toolStripBtnAddAnimation.Visible = true;
                    toolStripBtnClearAnimation.Visible = true;
                    toolStripBtnLastAnimation.Visible = false;
                    toolStripBtnNextAnimation.Visible = false;
                    toolStripBtnFindCircle.Visible = false;
                    toolStripButtonCopyFrom.Visible = false;
                    toolStripBtnCopyRoi.Visible = false;
                    toolStripBtnPasteRoi.Visible = false;

                    toolStripBtnShowAnimation.Visible = true;
                    toolStripLabel1.Visible = true;
                    toolStripCmbAnimationSpeed.Visible = true;

                    btnSaveParas.Enabled = false;
                    btnSaveParas.BackColor = Color.DarkGoldenrod;
                    #endregion

                    ImageViewer.Roi.Clear();

                    userDataGrid.dataGridView.Rows.Clear();
                    userDataGrid.dataGridView.Columns.Clear();

                    userDataGrid.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "组号" });
                    userDataGrid.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "图像索引" });
                    userDataGrid.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "图像名" });

                    for (var i = 0; i < userDataGrid.dataGridView.Columns.Count; i++)
                        userDataGrid.dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    userDataGrid.dataGridView.Columns[0].ReadOnly = true;
                    userDataGrid.dataGridView.Columns[1].ReadOnly = true;
                    userDataGrid.dataGridView.Columns[2].ReadOnly = true;

                    ImageViewer_RoiChanged(null, null);
                    break;

                case SetStep.Step3:
                    #region 刷新界面显示图像标定
                    toolStripCmbImgList.Enabled = false;

                    toolStripBtnAcquisition.Visible = false;
                    toolStripBtnCapture.Visible = false;
                    toolStripBtnOpenControllerDebuger.Visible = false;

                    toolStripButtonAddRect.Visible = true;
                    toolStripButtonAddShape.Visible = true;

                    toolStripLabel2.Visible = false;
                    toolStripComboBoxColorPlaneExtraction.Visible = false;
                    toolStripLabel3.Visible = false;
                    toolStripComboBoxLookupTable.Visible = false;

                    toolStripLabel2.Visible = false;
                    toolStripComboBoxColorPlaneExtraction.Visible = false;
                    toolStripLabel3.Visible = false;
                    toolStripComboBoxLookupTable.Visible = false;

                    toolStripBtnStopAutoGetRrays.Visible = true;
                    toolStripBtnGetRrays.Visible = true;
                    toolStripCmbRrayPercent.Visible = true;
                    toolStripButtonDeleteRect.Visible = true;

                    toolStripBtnAddAnimation.Visible = false;
                    toolStripBtnClearAnimation.Visible = false;

                    toolStripBtnFindCircle.Visible = true;
                    toolStripCmbRadiusRange.Visible = true;
                    toolStripButtonCopyFrom.Visible = true;

                    if (_imgList.Count > 1)
                    {
                        toolStripBtnLastAnimation.Visible = true;
                        toolStripBtnNextAnimation.Visible = true;

                        toolStripBtnCopyRoi.Visible = true;
                        toolStripBtnPasteRoi.Visible = true;
                    }

                    toolStripBtnShowAnimation.Visible = false;
                    toolStripLabel1.Visible = false;
                    toolStripCmbAnimationSpeed.Visible = false;

                    btnSaveParas.Enabled = false;
                    btnSaveParas.BackColor = Color.DarkGoldenrod;
                    #endregion

                    if (srcStep == SetStep.Step2)
                    {
                        if (_imgList.Count == 1)
                        {
                            _animationImgLst.Clear();

                            if (listFuncs[findFuncIndex].VisionPara != null &&
                                listFuncs[findFuncIndex].VisionPara.ShapesGroups != null &&
                                listFuncs[findFuncIndex].VisionPara.ShapesGroups.Any() &&
                                listFuncs[findFuncIndex].VisionPara.ShapesGroups[0].Shapes != null &&
                                listFuncs[findFuncIndex].VisionPara.ShapesGroups[0].Shapes.Any())
                            {
                                _animationImgLst.Add(
                                    new KeyValuePair<int, LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroup>(0,
                                        new LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroup
                                        {
                                            GroupIndex = 0,
                                            Shapes = listFuncs[findFuncIndex].VisionPara.ShapesGroups[0].Shapes
                                        }));
                            }
                            else
                            {
                                _animationImgLst.Add(
                                    new KeyValuePair<int, LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroup>(0,
                                        new LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroup
                                        {
                                            GroupIndex = 0,
                                            Shapes = new LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroupShape[0],
                                        }));
                            }
                        }
                        else
                        {
                            _animationImgLst.Clear();
                            for (var i = 0; i < userDataGrid.dataGridView.Rows.Count; i++)
                            {
                                var row = userDataGrid.dataGridView.Rows[i];
                                var groupIndex = int.Parse(row.Cells[0].Value.ToString());
                                var imgIndex = int.Parse(row.Cells[1].Value.ToString());

                                if (listFuncs[findFuncIndex].VisionPara != null &&
                                    listFuncs[findFuncIndex].VisionPara.ShapesGroups != null &&
                                    listFuncs[findFuncIndex].VisionPara.ShapesGroups.Length > groupIndex &&
                                    listFuncs[findFuncIndex].VisionPara.ShapesGroups[groupIndex].Shapes != null &&
                                    listFuncs[findFuncIndex].VisionPara.ShapesGroups[groupIndex].Shapes.Any())
                                {
                                    _animationImgLst.Add(
                                    new KeyValuePair<int, LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroup>(imgIndex,
                                        new LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroup
                                        {
                                            GroupIndex = groupIndex,
                                            Shapes = listFuncs[findFuncIndex].VisionPara.ShapesGroups[groupIndex].Shapes,
                                        }));
                                }
                                else
                                {
                                    _animationImgLst.Add(
                                        new KeyValuePair<int, LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroup>(
                                            imgIndex,
                                            new LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroup
                                            {
                                                GroupIndex = groupIndex,
                                                Shapes =
                                                    new LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroupShape[0],
                                            }));
                                }
                            }
                        }
                    }

                    ImageViewer.Roi.Clear();

                    userDataGrid.dataGridView.Rows.Clear();
                    userDataGrid.dataGridView.Columns.Clear();

                    userDataGrid.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "组号" });
                    userDataGrid.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "序号" });
                    userDataGrid.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "最小值" });
                    userDataGrid.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "最大值" });
                    userDataGrid.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "实际值" });

                    for (var i = 0; i < userDataGrid.dataGridView.Columns.Count; i++)
                        userDataGrid.dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    userDataGrid.dataGridView.Columns[0].ReadOnly = true;
                    userDataGrid.dataGridView.Columns[1].ReadOnly = true;
                    userDataGrid.dataGridView.Columns[2].ReadOnly = false;
                    userDataGrid.dataGridView.Columns[3].ReadOnly = false;
                    userDataGrid.dataGridView.Columns[4].ReadOnly = true;

                    toolStripCmbImgList.SelectedIndex = _animationImgLst[0].Key;
                    _currentIndexGroupIndex = 0;

                    foreach (var item in _animationImgLst[_currentIndexGroupIndex].Value.Shapes)
                    {
                        if (item.Contour.Type == "PolygonContour")
                            ImageViewer.Roi.Add(ImageProcessing.GetPolygonContourByString(item.Contour.Rect));
                        else if (item.Contour.Type == "RectangleContour")
                            ImageViewer.Roi.Add(ImageProcessing.GetRectByString(item.Contour.Rect));
                    }

                    ImageProcessing.DrawContourCountInOverlay(ImageViewer.Image, ImageViewer.Roi);
                    toolStripBtnShowGray_Click(null, null);
                    break;

                case SetStep.Step4:
                    if (MessageBox.Show(@"是否确认要保存？", @"确认", MessageBoxButtons.OKCancel) != DialogResult.OK)
                        return;

                    toolStripCmbImgList.Enabled = false;

                    toolStripBtnAcquisition.Visible = false;
                    toolStripBtnCapture.Visible = false;
                    toolStripBtnOpenControllerDebuger.Visible = false;

                    toolStripButtonAddRect.Visible = false;
                    toolStripButtonAddShape.Visible = false;

                    toolStripLabel2.Visible = false;
                    toolStripComboBoxColorPlaneExtraction.Visible = false;
                    toolStripLabel3.Visible = false;
                    toolStripComboBoxLookupTable.Visible = false;

                    toolStripLabel2.Visible = false;
                    toolStripComboBoxColorPlaneExtraction.Visible = false;
                    toolStripLabel3.Visible = false;
                    toolStripComboBoxLookupTable.Visible = false;

                    toolStripBtnStopAutoGetRrays.Visible = false;
                    toolStripBtnGetRrays.Visible = false;
                    toolStripCmbRrayPercent.Visible = false;

                    toolStripBtnAddAnimation.Visible = false;
                    toolStripBtnClearAnimation.Visible = false;
                    toolStripBtnLastAnimation.Visible = false;
                    toolStripBtnNextAnimation.Visible = false;

                    toolStripBtnFindCircle.Visible = false;
                    toolStripCmbRadiusRange.Visible = false;
                    toolStripButtonCopyFrom.Visible = false;

                    toolStripBtnCopyRoi.Visible = false;
                    toolStripBtnPasteRoi.Visible = false;
                    toolStripButtonDeleteRect.Visible = false;

                    toolStripBtnShowAnimation.Visible = false;
                    toolStripLabel1.Visible = false;
                    toolStripCmbAnimationSpeed.Visible = false;

                    //btnSaveParas.Enabled = true;
                    btnSaveParas.BackColor = Color.DarkGreen;

                    _animationImgLst[_currentIndexGroupIndex].Value.Shapes =
                                    new LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroupShape[ImageViewer.Roi.Count];

                    for (var i = 0; i < ImageViewer.Roi.Count; i++)
                    {
                        var shape = ImageViewer.Roi[i].Shape;

                        if (shape is PolygonContour) // 多边形
                        {
                            var polygonContour = (PolygonContour)shape;

                            _animationImgLst[_currentIndexGroupIndex].Value.Shapes[i] =
                                new LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroupShape
                                {
                                    Contour = new LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroupShapeContour
                                    {
                                        Type = typeof(PolygonContour).Name,
                                        Rect = ImageProcessing.GetStringPolygonContour(polygonContour)
                                    },
                                    Min =
                                        Math.Round(
                                            double.Parse(userDataGrid.dataGridView.Rows[i].Cells[2].Value.ToString()), 2,
                                            MidpointRounding.AwayFromZero),
                                    Max =
                                        Math.Round(
                                            double.Parse(userDataGrid.dataGridView.Rows[i].Cells[3].Value.ToString()), 2,
                                            MidpointRounding.AwayFromZero),
                                    Value =
                                        Math.Round(
                                            double.Parse(userDataGrid.dataGridView.Rows[i].Cells[4].Value.ToString()), 2,
                                            MidpointRounding.AwayFromZero),
                                };
                        }
                        else if (shape is RectangleContour) // 矩形
                        {
                            var rectangleContour = (RectangleContour)shape;

                            _animationImgLst[_currentIndexGroupIndex].Value.Shapes[i] =
                                new LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroupShape
                                {
                                    Contour = new LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroupShapeContour
                                    {
                                        Type = typeof(RectangleContour).Name,
                                        Rect = ImageProcessing.GetStringRect(rectangleContour)
                                    },
                                    Min = Math.Round(double.Parse(userDataGrid.dataGridView.Rows[i].Cells[2].Value.ToString()), 2, MidpointRounding.AwayFromZero),
                                    Max = Math.Round(double.Parse(userDataGrid.dataGridView.Rows[i].Cells[3].Value.ToString()), 2, MidpointRounding.AwayFromZero),
                                    Value = Math.Round(double.Parse(userDataGrid.dataGridView.Rows[i].Cells[4].Value.ToString()), 2, MidpointRounding.AwayFromZero),
                                };
                        }
                    }

                    ImageViewer.Roi.Clear();
                    toolStripCmbImgList.SelectedIndex = 0;

                    userDataGrid.dataGridView.Rows.Clear();
                    userDataGrid.dataGridView.Columns.Clear();

                    ImageViewer_RoiChanged(null, null);

                    listFuncs[findFuncIndex] = new LedCheckParaVisionFuncsVisionFunc
                    {
                        FuncName = FuncName,
                        FuncIndex = (ushort)findFuncIndex,
                        FuncImgPath = listFuncs[findFuncIndex].FuncImgPath,
                        FuncType = "Vision",
                        VisionPara = new LedCheckParaVisionFuncsVisionFuncVisionPara
                        {
                            TemplatePara = new LedCheckParaVisionFuncsVisionFuncVisionParaTemplatePara
                            {
                                //TemplateImgPath =
                                //   listFuncs[findFuncIndex].VisionPara.TemplatePara.TemplateImgPath,
                                TemplateCanterPoint = string.Format(@"{0},{1}", CenterPoint.X, CenterPoint.Y),
                                TemplateRoiRectangle = ImageProcessing.GetStringRect(TemplateRectangle)
                            },
                            ShapesGroups = new LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroup[_animationImgLst.Count]
                        },
                        CameraPara = new LedCheckParaVisionFuncsVisionFuncCameraPara
                        {
                            CameraSn = cameraSnTxt.comboBox.Text,
                            DelayTime = int.Parse(cameraDelayTxt.comboBox.Text),
                            FrameCount = int.Parse(cameraFrameCountTxt.comboBox.Text),
                            FrameRate = cameraFrameRateTxt.comboBox.SelectedIndex,
                            Shutter = int.Parse(cameraShutterTxt.comboBox.Text)
                        }
                    };

                    listFuncs[findFuncIndex].VisionPara.ColorPlaneExtraction =
                                toolStripComboBoxColorPlaneExtraction.Text;
                    listFuncs[findFuncIndex].VisionPara.LookupTable =
                            toolStripComboBoxLookupTable.Text;

                    for (var i = 0; i < listFuncs[findFuncIndex].VisionPara.ShapesGroups.Length; i++)
                    {
                        listFuncs[findFuncIndex].VisionPara.ShapesGroups[i] =
                        new LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroup
                        {
                            GroupIndex = i,
                            Shapes = _animationImgLst[i].Value.Shapes
                        };
                    }

                    //var tempFilePath = string.Format(@"{0}\图像检测配置文件\{1}\{2}", Directory.GetCurrentDirectory(),
                    //    ledChekPara.VisionFuncs.ProductName,
                    //    listFuncs[findFuncIndex].VisionPara.TemplatePara.TemplateImgPath);
                    //TemplateImage.WritePngFile(tempFilePath);

                    ledChekPara.VisionFuncs.VisionFunc = listFuncs.ToArray();
                    XmlHelper.SerializeToFile(ledChekPara, filePath, Encoding.UTF8);
                    MessageBox.Show(@"参数保存成功");

                    btnLastStep.Enabled = false;
                    btnNextStep.Enabled = false;
                    break;

                default:
                    throw new ArgumentOutOfRangeException("desSetStep", desSetStep, null);
            }

            CurrentStep = desSetStep;
            lblWithCmbStepsList.comboBox.Items.Clear();
            lblWithCmbStepsList.comboBox.Items.Add(
                CurrentStep.GetCustomAttribute<DescriptionAttribute>().Description);
            lblWithCmbStepsList.comboBox.SelectedIndex = 0;
        }

        #region 按钮事件
        /// <summary>
        /// 播放动画效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void toolStripBtnShowAnimation_Click(object sender, EventArgs e)
        {
            Enabled = false;

            await Task.Run(() =>
            {
                if (CurrentStep == SetStep.Step1)
                {
                    for (var i = 0; i < _imgList.Count; i++)
                    {
                        toolStripCmbImgList.SelectedIndex = i;
                        Thread.Sleep(int.Parse(toolStripCmbAnimationSpeed.Text));
                    }
                }
                else if (CurrentStep == SetStep.Step2)
                {
                    for (var i = 0; i < userDataGrid.dataGridView.Rows.Count; i++)
                    {
                        var imgIndex = int.Parse(userDataGrid.dataGridView.Rows[i].Cells[1].Value.ToString());
                        toolStripCmbImgList.SelectedIndex = imgIndex;
                        Thread.Sleep(int.Parse(toolStripCmbAnimationSpeed.Text));
                    }
                }
            });

            Enabled = true;
        }

        /// <summary>
        /// 上一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLastStep_Click(object sender, EventArgs e)
        {
            var stepInt = (int)CurrentStep;
            if (stepInt == (int)SetStep.Step0)
                return;
            ChangetStep(CurrentStep, EnumOperater.GetEnumByValue<SetStep>(--stepInt));
        }

        /// <summary>
        /// 下一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNextStep_Click(object sender, EventArgs e)
        {
            var stepInt = (int)CurrentStep;
            if (stepInt == (int)SetStep.Step4)
                return;
            ChangetStep(CurrentStep, EnumOperater.GetEnumByValue<SetStep>(++stepInt));

            Text = CurrentStep.GetCustomAttribute<DescriptionAttribute>().Description;
        }

        /// <summary>
        /// 添加到动画样本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripBtnAddAnimation_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < userDataGrid.dataGridView.Rows.Count; i++)
            {
                var row = userDataGrid.dataGridView.Rows[i];
                if (row.Cells[1].Value.ToString().Equals((toolStripCmbImgList.SelectedIndex + 1).ToString()))
                    return;
            }

            var newRowIndex = userDataGrid.dataGridView.Rows.Add();
            var newRow = userDataGrid.dataGridView.Rows[newRowIndex];

            newRow.Cells[0].Value = newRowIndex;
            newRow.Cells[1].Value = toolStripCmbImgList.SelectedIndex;
            newRow.Cells[2].Value = toolStripCmbImgList.Text;
        }

        /// <summary>
        /// 清空动画样本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripBtnClearAnimation_Click(object sender, EventArgs e)
        {
            ChangetStep(SetStep.Step1, SetStep.Step2);
        }

        /// <summary>
        /// 上一帧动画
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripBtnLastAnimation_Click(object sender, EventArgs e)
        {
            if (_currentIndexGroupIndex == 0)
                return;

            _animationImgLst[_currentIndexGroupIndex].Value.Shapes =
                new LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroupShape[ImageViewer.Roi.Count];

            for (var i = 0; i < ImageViewer.Roi.Count; i++)
            {
                var shape = ImageViewer.Roi[i].Shape;

                if (shape is PolygonContour) // 多边形
                {
                    var polygonContour = (PolygonContour)shape;

                    var str = polygonContour.Points.Aggregate(string.Empty, (current, p) => current + (p.X + "," + p.Y) + ",");
                    str = str.Trim(',');

                    _animationImgLst[_currentIndexGroupIndex].Value.Shapes[i] = new LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroupShape
                    {
                        Contour = new LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroupShapeContour
                        {
                            Type = @"PolygonContour",
                            Rect = str
                        },
                        Index = i + 1,
                        Min = Math.Round(double.Parse(userDataGrid.dataGridView.Rows[i].Cells[2].Value.ToString()), 2,
                            MidpointRounding.AwayFromZero),
                        Max = Math.Round(double.Parse(userDataGrid.dataGridView.Rows[i].Cells[3].Value.ToString()), 2,
                            MidpointRounding.AwayFromZero),
                        Value = Math.Round(double.Parse(userDataGrid.dataGridView.Rows[i].Cells[4].Value.ToString()), 2,
                            MidpointRounding.AwayFromZero),
                    };
                }
                else if (shape is RectangleContour) // 矩形
                {
                    var rectangleContour = (RectangleContour)shape;

                    var str = string.Format(@"{0},{1},{2},{3}", rectangleContour.Left, rectangleContour.Top,
                        rectangleContour.Width, rectangleContour.Height);

                    _animationImgLst[_currentIndexGroupIndex].Value.Shapes[i] = new LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroupShape
                    {
                        Contour = new LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroupShapeContour
                        {
                            Type = @"RectangleContour",
                            Rect = str
                        },
                        Index = i + 1,
                        Min = Math.Round(double.Parse(userDataGrid.dataGridView.Rows[i].Cells[2].Value.ToString()), 2,
                            MidpointRounding.AwayFromZero),
                        Max = Math.Round(double.Parse(userDataGrid.dataGridView.Rows[i].Cells[3].Value.ToString()), 2,
                            MidpointRounding.AwayFromZero),
                        Value = Math.Round(double.Parse(userDataGrid.dataGridView.Rows[i].Cells[4].Value.ToString()), 2,
                            MidpointRounding.AwayFromZero),
                    };
                }
            }

            ImageViewer.Roi.Clear();
            userDataGrid.dataGridView.Rows.Clear();
            toolStripCmbImgList.SelectedIndex = _animationImgLst[--_currentIndexGroupIndex].Key;

            foreach (var item in _animationImgLst[_currentIndexGroupIndex].Value.Shapes)
            {
                if (item.Contour.Type == "PolygonContour")
                    ImageViewer.Roi.Add(ImageProcessing.GetPolygonContourByString(item.Contour.Rect));
                else if (item.Contour.Type == "RectangleContour")
                    ImageViewer.Roi.Add(ImageProcessing.GetRectByString(item.Contour.Rect));

                ImageProcessing.DrawContourCountInOverlay(ImageViewer.Image, ImageViewer.Roi);
            }

            if (_animationImgLst[_currentIndexGroupIndex].Value.Shapes.Length <= 0)
            {
                ImageViewer_RoiChanged(null, null);
                return;
            }

            for (var i = 0; i < _animationImgLst[_currentIndexGroupIndex].Value.Shapes.Length; i++)
            {
                var item = _animationImgLst[_currentIndexGroupIndex].Value.Shapes[i];

                var gray = 0.0;
                if (item.Contour.Type == "PolygonContour")
                    gray = ImageProcessing.GetGrayValue(ImageViewer.Image, ImageProcessing.GetPolygonContourByString(item.Contour.Rect));
                else if (item.Contour.Type == "RectangleContour")
                    gray = ImageProcessing.GetGrayValue(ImageViewer.Image, ImageProcessing.GetRectByString(item.Contour.Rect));

                var newRowIndex = userDataGrid.dataGridView.Rows.Add();
                var row = userDataGrid.dataGridView.Rows[newRowIndex];

                row.Cells[0].Value = _currentIndexGroupIndex.ToString();
                row.Cells[1].Value = i.ToString();
                row.Cells[2].Value = item.Min;
                row.Cells[3].Value = item.Max;
                row.Cells[4].Value = gray;
            }
        }

        /// <summary>
        /// 下一帧动画
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripBtnNextAnimation_Click(object sender, EventArgs e)
        {
            if (_currentIndexGroupIndex == _animationImgLst.Count - 1)
                return;

            _animationImgLst[_currentIndexGroupIndex].Value.Shapes =
                new LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroupShape[ImageViewer.Roi.Count];

            for (var i = 0; i < ImageViewer.Roi.Count; i++)
            {
                var shape = ImageViewer.Roi[i].Shape;

                if (shape is PolygonContour) // 多边形
                {
                    var polygonContour = (PolygonContour)shape;

                    var str = polygonContour.Points.Aggregate(string.Empty, (current, p) => current + (p.X + "," + p.Y) + ",");
                    str = str.Trim(',');

                    _animationImgLst[_currentIndexGroupIndex].Value.Shapes[i] = new LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroupShape
                    {
                        Contour = new LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroupShapeContour
                        {
                            Type = @"PolygonContour",
                            Rect = str
                        },
                        Index = i + 1,
                        Min = Math.Round(double.Parse(userDataGrid.dataGridView.Rows[i].Cells[2].Value.ToString()), 2,
                            MidpointRounding.AwayFromZero),
                        Max = Math.Round(double.Parse(userDataGrid.dataGridView.Rows[i].Cells[3].Value.ToString()), 2,
                            MidpointRounding.AwayFromZero),
                        Value = Math.Round(double.Parse(userDataGrid.dataGridView.Rows[i].Cells[4].Value.ToString()), 2,
                            MidpointRounding.AwayFromZero),
                    };
                }
                else if (shape is RectangleContour) // 矩形
                {
                    var rectangleContour = (RectangleContour)shape;

                    var str = string.Format(@"{0},{1},{2},{3}", rectangleContour.Left, rectangleContour.Top,
                        rectangleContour.Width, rectangleContour.Height);

                    _animationImgLst[_currentIndexGroupIndex].Value.Shapes[i] = new LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroupShape
                    {
                        Contour = new LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroupShapeContour
                        {
                            Type = @"RectangleContour",
                            Rect = str
                        },
                        Index = i + 1,
                        Min = Math.Round(double.Parse(userDataGrid.dataGridView.Rows[i].Cells[2].Value.ToString()), 2,
                            MidpointRounding.AwayFromZero),
                        Max = Math.Round(double.Parse(userDataGrid.dataGridView.Rows[i].Cells[3].Value.ToString()), 2,
                            MidpointRounding.AwayFromZero),
                        Value = Math.Round(double.Parse(userDataGrid.dataGridView.Rows[i].Cells[4].Value.ToString()), 2,
                            MidpointRounding.AwayFromZero),
                    };
                }
            }

            ImageViewer.Roi.Clear();
            userDataGrid.dataGridView.Rows.Clear();
            toolStripCmbImgList.SelectedIndex = _animationImgLst[++_currentIndexGroupIndex].Key;

            foreach (var item in _animationImgLst[_currentIndexGroupIndex].Value.Shapes)
            {
                if (item.Contour.Type == "PolygonContour")
                    ImageViewer.Roi.Add(ImageProcessing.GetPolygonContourByString(item.Contour.Rect));
                else if (item.Contour.Type == "RectangleContour")
                    ImageViewer.Roi.Add(ImageProcessing.GetRectByString(item.Contour.Rect));

                ImageProcessing.DrawContourCountInOverlay(ImageViewer.Image, ImageViewer.Roi);
            }

            if (_animationImgLst[_currentIndexGroupIndex].Value.Shapes.Length <= 0)
            {
                ImageViewer_RoiChanged(null, null);
                return;
            }

            for (var i = 0; i < _animationImgLst[_currentIndexGroupIndex].Value.Shapes.Length; i++)
            {
                var item = _animationImgLst[_currentIndexGroupIndex].Value.Shapes[i];

                var gray = 0.0;
                if (item.Contour.Type == "PolygonContour")
                    gray = ImageProcessing.GetGrayValue(ImageViewer.Image, ImageProcessing.GetPolygonContourByString(item.Contour.Rect));
                else if (item.Contour.Type == "RectangleContour")
                    gray = ImageProcessing.GetGrayValue(ImageViewer.Image, ImageProcessing.GetRectByString(item.Contour.Rect));

                var newRowIndex = userDataGrid.dataGridView.Rows.Add();
                var row = userDataGrid.dataGridView.Rows[newRowIndex];

                row.Cells[0].Value = _currentIndexGroupIndex.ToString();
                row.Cells[1].Value = i.ToString();
                row.Cells[2].Value = item.Min;
                row.Cells[3].Value = item.Max;
                row.Cells[4].Value = gray;
            }
        }

        /// <summary>
        /// 添加矩形
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonAddRect_Click(object sender, EventArgs e)
        {
            var newRectangleContour = new RectangleContour(0, 0, 200, 100);
            ImageViewer.Roi.Add(newRectangleContour);

            ImageViewer_RoiChanged(null, null);
        }

        /// <summary>
        /// 添加多边形
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonAddShape_Click(object sender, EventArgs e)
        {
            var newPolygonContour = ImageProcessing.GetPolygonContour();
            ImageViewer.Roi.Add(newPolygonContour);

            ImageViewer_RoiChanged(null, null);
        }

        /// <summary>
        /// 找圆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripBtnFindCircle_Click(object sender, EventArgs e)
        {
            if (toolStripCmbRadiusRange.ComboBox == null)
                return;

            ImageViewer.Roi.Clear();
            userDataGrid.dataGridView.Rows.Clear();
            _isAutoGetGray = true;

            var radiusMin = int.Parse(toolStripCmbRadiusRange.ComboBox.Text);

            var radiusRange = new Range(radiusMin, 100);
            var thresholdRange = new Range(128, 255);
            var getCircleList = ImageProcessing.GetCircle(ImageViewer.Image, radiusRange, thresholdRange);

            var rectList = new List<RectangleContour>();
            const int matchRange = 20;
            const int radutsExpandPixel = 5;

            foreach (var rect in from t in getCircleList
                                 let left = t.Center.X - (t.Radius + radutsExpandPixel)
                                 let top = t.Center.Y - (t.Radius + radutsExpandPixel)
                                 let width = (t.Radius + radutsExpandPixel) * 2
                                 let height = (t.Radius + radutsExpandPixel) * 2
                                 where rectList.Find(
                                     f => Math.Abs(f.Left - left) <= matchRange && Math.Abs(f.Top - top) <= matchRange) == null
                                 select new RectangleContour(left, top, width, height))
                rectList.Add(rect);

            foreach (var r in rectList)
                ImageViewer.Roi.Add(r);

            //foreach (var newRectangleContour in from t in getCircleList
            //                                    let left = t.Center.X - (t.Radius + 5)
            //                                    let top = t.Center.Y - (t.Radius + 5)
            //                                    let width = (t.Radius + 5) * 2
            //                                    let height = (t.Radius + 5) * 2
            //                                    select new RectangleContour(left, top, width, height))
            //{
            //    ImageViewer.Roi.Add(newRectangleContour);
            //    //toolStripBtnShowGray_Click(null, null);
            //}

            ImageViewer_RoiChanged(null, null);
        }

        /// <summary>
        /// 复制ROI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripBtnCopyRoi_Click(object sender, EventArgs e)
        {
            _cpoyShapes.Clear();

            foreach (var shape in ImageViewer.Roi.Select(t => t.Shape))
            {
                if (shape is PolygonContour) // 多边形
                {
                    var polygonContour = (PolygonContour)shape;

                    var str = polygonContour.Points.Aggregate(string.Empty, (current, p) => current + (p.X + "," + p.Y) + ",");
                    str = str.Trim(',');

                    _cpoyShapes.Add(new LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroupShape
                    {
                        Contour = new LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroupShapeContour
                        {
                            Type = typeof(PolygonContour).Name,
                            Rect = str
                        }
                    });
                }
                else if (shape is RectangleContour) // 矩形
                {
                    var rectangleContour = (RectangleContour)shape;

                    var str = string.Format(@"{0},{1},{2},{3}", rectangleContour.Left, rectangleContour.Top,
                        rectangleContour.Width, rectangleContour.Height);

                    _cpoyShapes.Add(new LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroupShape
                    {
                        Contour = new LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroupShapeContour
                        {
                            Type = typeof(RectangleContour).Name,
                            Rect = str
                        }
                    });
                }
            }
        }

        /// <summary>
        /// 粘贴ROI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripBtnPasteRoi_Click(object sender, EventArgs e)
        {
            _isAutoGetGray = true;

            foreach (var item in _cpoyShapes)
            {
                if (item.Contour.Type.Equals(typeof(PolygonContour).Name))
                    ImageViewer.Roi.Add(ImageProcessing.GetPolygonContourByString(item.Contour.Rect));
                else if (item.Contour.Type.Equals(typeof(RectangleContour).Name))
                    ImageViewer.Roi.Add(ImageProcessing.GetRectByString(item.Contour.Rect));
            }

            ImageViewer_RoiChanged(null, null);
            toolStripBtnShowGray_Click(null, null);
        }

        /// <summary>
        /// 保存参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveParas_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 打开图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void toolStripBtnOpenPics_Click(object sender, EventArgs e)
        {
            if (toolStripBtnAcquisition.Text == @"停止采集")
                toolStripBtnAcquisition_Click(null, null);

            var thisProgramFolder = new FileInfo(XmlFilePath).Directory;
            var captureImageFolder = string.Format(@"{0}\{1}-CaptureImage", thisProgramFolder, FuncName);
            if (!Directory.Exists(captureImageFolder))
                Directory.CreateDirectory(captureImageFolder);
            var caliImageFolder = string.Format(@"{0}\标定图片", captureImageFolder);
            if (!Directory.Exists(caliImageFolder))
                Directory.CreateDirectory(caliImageFolder);

            var dialog = new OpenFileDialog
            {
                InitialDirectory = caliImageFolder,
                Multiselect = true,
                Title = @"请选择文件",
                Filter = @"BMP格式图像文件(*.bmp)|*.bmp;"
            };

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            if (dialog.FileNames.Length == 0)
                return;

            Enabled = false;

            await Task.Run(() =>
            {
                foreach (var t in _imgList)
                    t.Dispose();
                _imgList.Clear();
                toolStripCmbImgList.Items.Clear();

                foreach (var t in dialog.FileNames)
                {
                    var fileInfo = Algorithms.GetFileInformation(t);

                    var img = new VisionImage(fileInfo.ImageType);
                    img.ReadFile(t);
                    _imgList.Add(img);

                    var imgFile = new FileInfo(t);
                    toolStripCmbImgList.Items.Add(imgFile.Name);
                }
                Algorithms.Copy(_imgList[0], ImageViewer.Image);
                toolStripCmbImgList.SelectedIndex = 0;
            });

            Enabled = true;
        }

        /// <summary>
        /// 刷新灰度值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripBtnShowGray_Click(object sender, EventArgs e)
        {
            if (toolStripCmbRrayPercent.ComboBox == null)
                return;

            userDataGrid.dataGridView.Rows.Clear();
            var perentage = double.Parse(toolStripCmbRrayPercent.ComboBox.Text.Substring(1, 2)) * 0.01;

            for (var i = 0; i < ImageViewer.Roi.Count; i++)
            {
                var shape = ImageViewer.Roi[i].Shape;

                var gray = ImageProcessing.GetGrayValue(ImageViewer.Image, shape);

                var minGray = Math.Round(gray * (1 - perentage), MidpointRounding.AwayFromZero);
                var maxGray = Math.Round(gray * (1 + perentage), MidpointRounding.AwayFromZero);

                var newRowIndex = userDataGrid.dataGridView.Rows.Add();
                var row = userDataGrid.dataGridView.Rows[newRowIndex];
                row.Cells[0].Value = _currentIndexGroupIndex.ToString();
                row.Cells[1].Value = (i + 1).ToString();
                row.Cells[2].Value = minGray < 35 ? 0 : minGray;
                row.Cells[3].Value = maxGray < 35 ? 35 : maxGray;
                row.Cells[4].Value = gray;
            }
        }

        private void toolStripBtnStopAutoGetRrays_Click(object sender, EventArgs e)
        {
            _isAutoGetGray = !_isAutoGetGray;
        }

        /// <summary>
        /// 打开控制器调试界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripBtnOpenControllerDebuger_Click(object sender, EventArgs e)
        {
            using (var form = new FormControllerDebuger(FormCheck.St))
                form.ShowDialog();
        }

        /// <summary>
        /// 图像采集
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripBtnAcquisition_Click(object sender, EventArgs e)
        {
            var sn = cameraSnTxt.comboBox.Text;
            if (string.IsNullOrEmpty(sn))
                return;

            if (toolStripCmbImgList.ComboBox != null)
                toolStripCmbImgList.ComboBox.Items.Clear();

            foreach (var t in _imgList)
                t.Dispose();
            _imgList.Clear();

            if (toolStripBtnAcquisition.Text == @"开始采集")
            {
                cameraShutterComboBox_SelectedIndexChanged(null, null);
                cameraFrameRateComboBox_SelectedIndexChanged(null, null);

                var device = _camera.CameraList.Find(f => f.GigeInfo.chSerialNumber == sn);
                if (device != null)
                    device.StartGrab();

                //_daheng.AcquisitionStart(sn);
                toolStripBtnAcquisition.Text = @"停止采集";
                toolStripBtnCapture.Enabled = true;
            }
            else if (toolStripBtnAcquisition.Text == @"停止采集")
            {
                foreach (var t in _camera.CameraList)
                    t.StopGrab();

                //_daheng.AcquisitionStop(sn);
                toolStripBtnAcquisition.Text = @"开始采集";
                toolStripBtnCapture.Enabled = false;
            }
        }

        /// <summary>
        /// 拍照
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void toolStripBtnCapture_Click(object sender, EventArgs e)
        {
            _snapShotCount = int.Parse(cameraFrameCountTxt.Text);
            _listCaptureImgs.Clear();
            _isSnapShotExecute = true;
            Enabled = false;

            await Task.Run(() =>
            {
                while (true)
                    if (!_isSnapShotExecute)
                        break;
            });

            Enabled = true;
        }

        /// <summary>
        /// 删除ROI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonDeleteRect_Click(object sender, EventArgs e)
        {
            _isAutoGetGray = true;

            ImageViewer.Roi.Clear();
            userDataGrid.dataGridView.Rows.Clear();
            ImageViewer_RoiChanged(null, null);
        }
        #endregion

        private void toolStripButtonCopyFrom_Click(object sender, EventArgs e)
        {
            var form = new Form { Width = 200, Height = 200, StartPosition = FormStartPosition.CenterScreen };

            var list = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, 120),
                Dock = DockStyle.Top
            };

            var ledChekPara = XmlHelper.Deserialize<LedCheckPara>(XmlFilePath);
            if (ledChekPara != null)
            {
                if (ledChekPara.VisionFuncs != null)
                {
                    foreach (
                        var t in
                            ledChekPara.VisionFuncs.VisionFunc.Where(
                                t =>
                                    t != null && t.VisionPara != null && t.VisionPara.ShapesGroups != null &&
                                    t.VisionPara.ShapesGroups.Any()))
                        list.Items.Add(t.FuncName);
                }
            }

            if (list.Items.Count > 0)
                list.SelectedIndex = 0;

            var btn = new Button
            {
                Text = @"确认",
                Dock = DockStyle.Top,
                Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, 120),
                Width = 50,
                Height = 50,
                BackColor = Color.DarkGray
            };
            btn.Click += btn_Click;
            form.Controls.Add(btn);
            form.Controls.Add(list);

            var result = form.ShowDialog();

            if (result == DialogResult.OK)
            {
                var funcName = list.Text;

                if (ledChekPara != null && ledChekPara.VisionFuncs != null)
                {
                    var ledCheckParaVisionFuncsVisionFunc = ledChekPara.VisionFuncs.VisionFunc.ToList().Find(f => f.FuncName == funcName);

                    if (ledCheckParaVisionFuncsVisionFunc != null &&
                        ledCheckParaVisionFuncsVisionFunc.VisionPara != null &&
                        ledCheckParaVisionFuncsVisionFunc.VisionPara.ShapesGroups != null &&
                        ledCheckParaVisionFuncsVisionFunc.VisionPara.ShapesGroups.Any() &&
                        ledCheckParaVisionFuncsVisionFunc.VisionPara.ShapesGroups[0].Shapes != null &&
                        ledCheckParaVisionFuncsVisionFunc.VisionPara.ShapesGroups[0].Shapes.Any())
                    {
                        _isAutoGetGray = true;

                        foreach (var item in ledCheckParaVisionFuncsVisionFunc.VisionPara.ShapesGroups[0].Shapes)
                        {
                            if (item.Contour.Type == "PolygonContour")
                                ImageViewer.Roi.Add(ImageProcessing.GetPolygonContourByString(item.Contour.Rect));
                            else if (item.Contour.Type == "RectangleContour")
                                ImageViewer.Roi.Add(ImageProcessing.GetRectByString(item.Contour.Rect));
                        }

                        ImageViewer_RoiChanged(null, null);
                        toolStripBtnShowGray_Click(null, null);
                    }
                }
            }
        }

        private static void btn_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn == null)
                return;

            var parent = btn.Parent as Form;
            if (parent != null)
                parent.DialogResult = DialogResult.OK;
        }
    }
}
