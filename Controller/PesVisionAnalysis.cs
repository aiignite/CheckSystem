using CommonUtility;
using CommonUtility.HikSdk;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using Mat = OpenCvSharp.Mat;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace Controller
{
    public sealed class PesVisionAnalysis : ControllerBase
    {
        private readonly CameraControl _cameraControl = new CameraControl();
        public MyCamera SelectedCamera { get; set; }

        public Dictionary<string, Mat> ImgMats = new Dictionary<string, Mat>();
        public const string LowBeamSrc0Name = "近光原图0";
        public const string EdgeImgName = "边缘提取";
        public const string CutOffLineContourImgName = "描绘截止线轮廓";
        public const string InflectionPointImgName = "描绘截止线拐点";
        public const string CoordinateSystemImgName = "建立坐标系";
        public const string LowBeamAnalysisResult0ImgName = "近光结果0";
        public const string LowBeamSrc1Name = "近光原图1";
        public const string LowBeamSrc2Name = "近光原图2";
        public const string LowBeamAnalysisResult1ImgName = "近光结果1";
        public const string LowBeamAnalysisResult2ImgName = "近光结果2";
        public const string LowBeamAnalysisResult3ImgName = "近光结果3";
        public const string LowBeamAnalysisResult4ImgName = "近光结果4";
        public const string LowBeamAnalysisResult5ImgName = "近光结果5-Grad";
        public const string HighBeamSrcName = "远光原图";
        public const string HighBeamAnalysisResultImgName = "远光结果";

        private readonly long _enterTs = HighPrecisionTimer.GetTimestamp();

        public PesVisionAnalysis(
            string name) : base(name)
        {
            ImgMats.Add(LowBeamSrc0Name, null);
            ImgMats.Add(EdgeImgName, null);
            ImgMats.Add(CutOffLineContourImgName, null);
            ImgMats.Add(InflectionPointImgName, null);
            ImgMats.Add(CoordinateSystemImgName, null);
            ImgMats.Add(LowBeamAnalysisResult0ImgName, null);
            ImgMats.Add(LowBeamSrc1Name, null);
            ImgMats.Add(LowBeamSrc2Name, null);
            ImgMats.Add(LowBeamAnalysisResult1ImgName, null);
            ImgMats.Add(LowBeamAnalysisResult2ImgName, null);
            ImgMats.Add(LowBeamAnalysisResult3ImgName, null);
            ImgMats.Add(LowBeamAnalysisResult4ImgName, null);
            ImgMats.Add(LowBeamAnalysisResult5ImgName, null);
            ImgMats.Add(HighBeamSrcName, null);
            ImgMats.Add(HighBeamAnalysisResultImgName, null);
            LoadCamera();
        }

        ~PesVisionAnalysis()
        {
            _cameraControl.Dispose();
        }

        public string CameraSn = "DA0121338";
        public bool IsLbOn;
        public bool IsHbOn;
        public bool IsInCali;
        public bool IsInAutoRun;
        public bool IsInServoCali;

        public string RoiStr = "400,200,2200,900";

        public double DenoiseSigma = 1.5;
        public int ScanPixelStep = 2;
        public double PerDegreePixel = 70;

        [Description("R/W,LowBeamShutter0")]
        public int LowBeamShutter0 = 3350;
        [Description("R/W,LowBeamShutter1")]
        public int LowBeamShutter1 = 10000;
        [Description("R/W,LowBeamShutter2")]
        public int LowBeamShutter2 = 45000;
        [Description("R/W,LowBeamShutter2")]
        public int HighBeamShutter2 = 1750;

        public Rect InterestedRoi { get; set; }

        public void SetInterestedRoi()
        {
            if (!string.IsNullOrEmpty(RoiStr))
            {
                try
                {
                    var x = int.Parse(RoiStr.Split(',')[0]);
                    var y = int.Parse(RoiStr.Split(',')[1]);
                    var width = int.Parse(RoiStr.Split(',')[2]);
                    var height = int.Parse(RoiStr.Split(',')[3]);

                    InterestedRoi = new Rect(x, y, width, height);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private void LoadCamera()
        {
            _cameraControl.DeviceListAcq();
            if (string.IsNullOrEmpty(CameraSn))
                return;

            var findCamera =
                _cameraControl.CameraList.Find(f =>
                    string.Equals(f.GigeInfo.chSerialNumber, CameraSn, StringComparison.CurrentCultureIgnoreCase));
            if (findCamera == null)
                return;
            SelectedCamera = findCamera;
            //SelectedCamera.OpenCamera();
        }

        public void OpenCamera()
        {
            if (SelectedCamera == null)
                return;
            SelectedCamera.OpenCamera();
        }

        public void CloseCamera()
        {
            if (SelectedCamera == null)
                return;
            SelectedCamera.CloseCamera();
        }

        public void StartGrab()
        {
            if (SelectedCamera == null)
                return;
            SelectedCamera.SetExposureTime(LowBeamShutter1);
            Thread.Sleep(400);
            SelectedCamera.StartGrab();
        }

        public void StopGrab()
        {
            if (SelectedCamera == null)
                return;
            SelectedCamera.StopGrab();
        }

        public void SnapLowBeam(int index)
        {
            if (SelectedCamera == null)
                return;
            if (index == 0)
            {
                SelectedCamera.SetExposureTime(LowBeamShutter0);
                Thread.Sleep(400);
                if (SelectedCamera.Capture(1))
                {
                    int row, col;
                    ImgMats[LowBeamSrc0Name] = SelectedCamera.GetImageFromBuff(0, out row, out col).Clone();
                }
                SelectedCamera.ClearBuffer();
            }
            else if (index == 1)
            {
                SelectedCamera.SetExposureTime(LowBeamShutter1);
                Thread.Sleep(400);
                if (SelectedCamera.Capture(1))
                {
                    int row, col;
                    ImgMats[LowBeamSrc1Name] = SelectedCamera.GetImageFromBuff(0, out row, out col).Clone();
                }
                SelectedCamera.ClearBuffer();
            }
            else
            {
                SelectedCamera.SetExposureTime(LowBeamShutter2);
                Thread.Sleep(400);
                if (SelectedCamera.Capture(1))
                {
                    int row, col;
                    ImgMats[LowBeamSrc2Name] = SelectedCamera.GetImageFromBuff(0, out row, out col).Clone();
                }
                SelectedCamera.ClearBuffer();
            }
        }

        public string ToUploadLowBeamFilePath = string.Empty;

        public void UploadLowBeam(int index)
        {
            if (index != 0 && index != 1)
                return;

            if (string.IsNullOrEmpty(ToUploadLowBeamFilePath))
                return;

            if (!File.Exists(ToUploadLowBeamFilePath))
                return;

            if (index == 0)
            {
                if (ImgMats[LowBeamSrc0Name] != null && !ImgMats[LowBeamSrc0Name].Empty())
                {
                    ImgMats[LowBeamSrc0Name].Dispose();
                    GC.Collect();
                }

                ImgMats[LowBeamSrc0Name] = new Mat(ToUploadLowBeamFilePath, ImreadModes.AnyColor);
            }
            else
            {
                if (ImgMats[LowBeamSrc1Name] != null && !ImgMats[LowBeamSrc1Name].Empty())
                {
                    ImgMats[LowBeamSrc1Name].Dispose();
                    GC.Collect();
                }

                ImgMats[LowBeamSrc1Name] = new Mat(ToUploadLowBeamFilePath, ImreadModes.AnyColor);
            }
        }

        public void SnapHighBeam()
        {
            if (SelectedCamera == null)
                return;
            SelectedCamera.SetExposureTime(HighBeamShutter2);
            Thread.Sleep(400);
            if (!SelectedCamera.Capture(1))
                return;
            int row, col;
            ImgMats[HighBeamSrcName] = SelectedCamera.GetImageFromBuff(0, out row, out col).Clone();
            SelectedCamera.ClearBuffer();
        }

        public string ToUploadHighBeamFilePath = string.Empty;

        public void UploadHighBeam()
        {
            if (string.IsNullOrEmpty(ToUploadHighBeamFilePath))
                return;

            if (!File.Exists(ToUploadHighBeamFilePath))
                return;

            if (ImgMats[HighBeamSrcName] != null && !ImgMats[HighBeamSrcName].Empty())
            {
                ImgMats[HighBeamSrcName].Dispose();
                GC.Collect();
            }

            ImgMats[HighBeamSrcName] = new Mat(ToUploadHighBeamFilePath, ImreadModes.AnyColor);
        }

        public void Release()
        {
            var keys = ImgMats.Keys.ToList();

            foreach (var key in keys.Where(key => ImgMats[key] != null && !ImgMats[key].Empty()))
            {
                ImgMats[key].Dispose();
                ImgMats[key] = null;
            }

            _pCutOffLineCenter = default;
            _cutOffLineP1 = default;
            _cutOffLineP2 = default;
            _cutOffLineP3 = default;
            _cutOffLineP4 = default;
            Ph0V0 = default;
            VisionAnalysisMsg = string.Empty;

            OutputLbH0V0Gray = -9999;
            OutputLbH0V2Gray = -9999;
            OutputLbH0V4Gray = -9999;
            OutputLbHn8V0Gray = -9999;
            OutputLbHn8V4Gray = -9999;
            OutputLbHn4V2Gray = -9999;
            OutputLbHn4V0Gray = -9999;
            OutputLbH4V2Gray = -9999;
            OutputLbH8V4Gray = -9999;
            OutputLbH0Vn11Gray = -9999;

            OutputHbHn11V0Gray = -9999;
            OutputHbH11V0Gray = -9999;
            OutputHbHn20V0Gray = -9999;
            OutputHbH20V0Gray = -9999;
            OutputHbH0V0Gray = -9999;

            HighBeamH0V0EmaxGrayRatio = -9999;

            GC.Collect();
        }

        private Point _pHn2d5V2;
        private Point _pHn2d5Vn2;

        private Point _pCutOffLineCenter;
        private Point _cutOffLineP1;
        private Point _cutOffLineP2;
        private Point _cutOffLineP3;
        private Point _cutOffLineP4;
        public double OutputCutOffLineAngle;

        public Point Ph0V0 { get; set; }

        public Point Ph0V2 { get; set; }
        public Point Ph0V4 { get; set; }
        public Point PHn8V0 { get; set; }
        public Point PHn8V4 { get; set; }
        public Point PHn4V2 { get; set; }
        public Point PHn4V0 { get; set; }
        public Point Ph4V2 { get; set; }
        public Point Ph8V4 { get; set; }

        public Point Ph0Vn11 { get; set; }

        public Point PHn11V0 { get; set; }
        public Point Ph11V0 { get; set; }
        public Point PHn20V0 { get; set; }
        public Point Ph20V0 { get; set; }

        private Point _pLbEmax;
        private Point _pHbEmax;

        private CircleWithGray _lbH0V0;
        private CircleWithGray _lbH0V2;
        private CircleWithGray _lbH0V4;
        private CircleWithGray _lbHn8V0;
        private CircleWithGray _lbHn8V4;
        private CircleWithGray _lbHn4V2;
        private CircleWithGray _lbHn4V0;
        private CircleWithGray _lbH4V2;
        private CircleWithGray _lbH8V4;
        private CircleWithGray _lbH0Vn11;

        private CircleWithGray _hbHn11V0;
        private CircleWithGray _hbH11V0;
        private CircleWithGray _hbHn20V0;
        private CircleWithGray _hbH20V0;
        private CircleWithGray _hbH0V0;

        private static long _startTs;
        //public string VisionAnalysisResult = string.Empty;
        public string VisionAnalysisMsg = string.Empty;

        public void VisionAnalysis()
        {
            _startTs = HighPrecisionTimer.GetTimestamp();

            VisionAnalysisMsg = string.Empty;

            ResetTransferServoPosition();
            ResetRegulatoryRequirementResult();

            _pHn2d5V2 = default;
            _pHn2d5Vn2 = default;

            _pCutOffLineCenter = default;
            _cutOffLineP1 = default;
            _cutOffLineP2 = default;
            _cutOffLineP3 = default;
            _cutOffLineP4 = default;
            OutputCutOffLineAngle = -9999;

            Ph0V0 = default;

            Ph0V2 = default;
            Ph0V4 = default;
            PHn8V0 = default;
            PHn8V4 = default;
            PHn4V2 = default;
            PHn4V0 = default;
            Ph4V2 = default;
            Ph8V4 = default;
            Ph0Vn11 = default;

            PHn11V0 = default;
            Ph11V0 = default;
            PHn20V0 = default;
            Ph20V0 = default;

            _pLbEmax = default;
            _pHbEmax = default;

            OutputLbVvSlope0 = -9999;
            OutputLbVvSlope1 = -9999;
            OutputLbHhSlope2 = -9999;
            OutputLbHhSlope3 = -9999;

            OutputLbH0V0Gray = -9999;
            OutputLbH0V2Gray = -9999;
            OutputLbH0V4Gray = -9999;
            OutputLbHn8V0Gray = -9999;
            OutputLbHn8V4Gray = -9999;
            OutputLbHn4V2Gray = -9999;
            OutputLbHn4V0Gray = -9999;
            OutputLbH4V2Gray = -9999;
            OutputLbH8V4Gray = -9999;
            OutputLbH0Vn11Gray = -9999;

            OutputHbHn11V0Gray = -9999;
            OutputHbH11V0Gray = -9999;
            OutputHbHn20V0Gray = -9999;
            OutputHbH20V0Gray = -9999;
            OutputHbH0V0Gray = -9999;

            HighBeamH0V0EmaxGrayRatio = -9999;

            if (!DenoiseLowBeamImg())
            {
                SaveImg();
                return;
            }

            if (!FindEdgeBrightnessPoints())
            {
                SaveImg();
                return;
            }

            if (!SetCoordinateSystem())
            {
                SaveImg();
                return;
            }

            LowBeamAnalysis();
            HighBeamAnalysis();
            GetRegulatoryRequirementResult();

            SaveImg();
        }

        public string ToSaveImgBarcode =
            string.Empty;
        //@"[)>06P2648013580.03.07.045476568631M24249A2B000018";//string.Empty;

        private void SaveImg()
        {
            try
            {
                var ts = HighPrecisionTimer.GetTimestamp().ToString();
                string name;
                if (!string.IsNullOrEmpty(Name))
                    name = Name;
                else
                    name = _enterTs + "_" + Name;
                name += "_保存图像";
                var driveLetter = Path.GetPathRoot(Directory.GetCurrentDirectory());
                var date = DateTime.Now.ToString("yyyyMMdd") + "_" + LowBeamShutter0 + "_" + LowBeamShutter1 + "_" + LowBeamShutter2 + "_" + HighBeamShutter2;
                var folder = string.Format(@"{0}/{1}", driveLetter, name);
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                var fileCount = Directory.GetDirectories(folder);
                if (fileCount.Length > 250)
                {
                    var di = new DirectoryInfo(folder);
                    var files = di.GetDirectories();
                    foreach (var file in files)
                        file.Delete(true);
                }

                string barcode;
                try
                {
                    barcode = ToSaveImgBarcode.Substring(39, 16);
                    barcode = barcode.Replace("?", "X");
                }
                catch (Exception e)
                {
                    barcode = string.Empty;
                }

                var folder1 = string.Format(@"{0}/{1}/{2}", driveLetter, name, date + (string.IsNullOrEmpty(barcode) ? _enterTs.ToString() : "_" + barcode));

                if (!Directory.Exists(folder1))
                    Directory.CreateDirectory(folder1);

                var folder2 = string.Format(@"{0}/{1}", folder1, ts);
                if (!Directory.Exists(folder2))
                    Directory.CreateDirectory(folder2);

                var index = 0;
                foreach (var t in ImgMats.Where(t => t.Value != null && !t.Value.Empty()))
                {
                    var jpg = string.Format(@"{0}/{1}_{2}_{3}.bmp", folder2, index++, t.Key, string.IsNullOrEmpty(barcode) ? _enterTs.ToString() : barcode);
                    if (!t.Value.ImWrite(jpg))
                    {
                        jpg = string.Format(@"{0}/{1}_{2}.bmp", folder2, index++, t.Key);
                        t.Value.ImWrite(jpg);
                    }
                }

                ToSaveImgBarcode = string.Empty;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static string FormatMsg(string content)
        {
            var msg = string.Format(@"{0},耗时:{1}ms", content,
                          HighPrecisionTimer.GetTimestampIntervalMs(_startTs, HighPrecisionTimer.GetTimestamp())) +
                      Environment.NewLine;
            _startTs = HighPrecisionTimer.GetTimestamp();
            return msg;
        }

        private bool DenoiseLowBeamImg()
        {
            if (ImgMats[LowBeamSrc0Name] == null || ImgMats[LowBeamSrc0Name].Empty())
            {
                VisionAnalysisMsg += FormatMsg("近光图像缺失");
                return false;
            }

            ImgMats[EdgeImgName] = ImgMats[LowBeamSrc0Name].Clone(InterestedRoi);

            if (ImgMats[EdgeImgName].Channels() == 3)
                Cv2.CvtColor(ImgMats[EdgeImgName], ImgMats[EdgeImgName], ColorConversionCodes.BGR2GRAY);

            var denoinseImg = ImageDenoise(ImgMats[EdgeImgName]);
            ImgMats[EdgeImgName] = denoinseImg.Clone();
            denoinseImg.Dispose();
            GC.Collect();

            return true;
        }

        private static Mat ImageDenoise(Mat src)
        {
            var grayImg = src.Clone();

            if (grayImg.Channels() == 3)
                Cv2.CvtColor(grayImg, grayImg, ColorConversionCodes.BGR2GRAY);

            //grayImg.ImWrite(@"C:\Users\B1438\Desktop\新建文件夹\grayImg.bmp");

            // 图像降噪
            Cv2.GaussianBlur(grayImg, grayImg, new Size(3, 3), 0, borderType: BorderTypes.Default);
            Cv2.MedianBlur(grayImg, grayImg, 3);

            //grayImg.ImWrite(@"C:\Users\B1438\Desktop\新建文件夹\ImageDenoise.bmp");

            return grayImg;
        }

        private bool FindEdgeBrightnessPoints()
        {
            var rows = ImgMats[EdgeImgName].Rows;
            var cols = ImgMats[EdgeImgName].Cols;

            var otsu = Cv2.Threshold(ImgMats[EdgeImgName], ImgMats[EdgeImgName], 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);
            Console.WriteLine(@"OTSU = {0}", otsu);

            // 找到轮廓
            Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(ImgMats[EdgeImgName], out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            if (!contours.Any())
            {
                VisionAnalysisMsg += FormatMsg("寻找拐点失败");
                return false;
            }

            Point resultCannyCenter = default;

            var maxContourArea = -1d;
            var maxContourIndex = -1;
            var contourIndex = 0;
            // 计算并绘制每个轮廓的几何中心
            foreach (var contour in contours)
            {
                var area = Cv2.ContourArea(contour);

                if (area > maxContourArea)
                {
                    maxContourArea = area;
                    maxContourIndex = contourIndex;
                }

                contourIndex++;
            }

            if (maxContourIndex == -1 || Math.Abs(maxContourArea - (-1)) <= 0)
            {
                VisionAnalysisMsg += FormatMsg("寻找拐点失败");
                return false;
            }

            // 计算矩
            var moments = Cv2.Moments(contours[maxContourIndex]);

            // 计算中心坐标
            var cx = moments.M10 / moments.M00;
            var cy = moments.M01 / moments.M00;

            resultCannyCenter = new Point(cx, cy);

            if (resultCannyCenter == default)
            {
                VisionAnalysisMsg += FormatMsg("寻找拐点失败");
                return false;
            }

            DrawXOnPoint(ImgMats[EdgeImgName], resultCannyCenter, Scalar.Black);

            // 在 ROI 中应用 Canny 边缘检测
            var edges = new Mat();
            Cv2.Canny(ImgMats[EdgeImgName], edges, 0, 255, L2gradient: true);
            //edges.ImWrite(@"C:\Users\B1438\Desktop\新建文件夹\edge.bmp");

            // 创建一个HashSet来存储唯一的点
            var uniquePointsInRoi = new HashSet<Point>();
            var pointsLeft = new List<Point>();
            var pointsRight = new List<Point>();

            var searchOffset = (int)(PerDegreePixel * 2);

            for (var x = resultCannyCenter.X - searchOffset; x < resultCannyCenter.X + searchOffset; x++)
            {
                for (var y = 0; y < edges.Height; y++)
                {
                    // 获取当前像素的灰度值
                    var pixelValue = edges.At<byte>(y, x);

                    // 如果灰度值为255，则添加该点的坐标到列表中
                    if (pixelValue != 255)
                        continue;

                    uniquePointsInRoi.Add(new Point(x, y));
                    break;
                }
            }
            edges.Dispose();
            GC.Collect();

            // 将HashSet转换为List并按X坐标排序
            var sortedPoints = uniquePointsInRoi.OrderBy(p => p.X).ToList();
            var sortedPointsToFilter = new List<Point>();
            // 每隔3个像素取一个点
            for (var i = 0; i < sortedPoints.Count - sortedPoints.Count % 3; i = i + 3)
                sortedPointsToFilter.Add(sortedPoints[i]);
            // 每3个像素点计算一致性，去除异常像素点
            var tempSortedPoints = new List<Point>();

            for (var i = 0; i < sortedPointsToFilter.Count - sortedPointsToFilter.Count % 3; i = i + 3)
            {
                var p1 = sortedPointsToFilter[i];
                var p2 = sortedPointsToFilter[i + 1];
                var p3 = sortedPointsToFilter[i + 2];

                if (p1.Y == p2.Y && p2.Y == p3.Y)
                {
                    tempSortedPoints.Add(p2);
                }
                else if ((p1.Y < p2.Y && p2.Y < p3.Y) || (p1.Y > p2.Y && p2.Y > p3.Y))
                {
                    tempSortedPoints.Add(p1);
                    tempSortedPoints.Add(p2);
                    tempSortedPoints.Add(p3);
                }
                else
                {
                    tempSortedPoints.Add(p1);
                    tempSortedPoints.Add(p3);
                }
            }
            sortedPointsToFilter.Clear();
            sortedPointsToFilter.AddRange(tempSortedPoints);

            var breakAt = -1;
            var stopAt = -1;
            for (var i = 0; i < sortedPointsToFilter.Count - sortedPointsToFilter.Count % 3; i = i + 3)
            {
                var p1 = sortedPointsToFilter[i];
                var p2 = sortedPointsToFilter[i + 1];
                var p3 = sortedPointsToFilter[i + 2];

                if (breakAt == -1)
                {
                    if (p1.Y > p2.Y && p2.Y > p3.Y)
                    {
                        breakAt = i;
                    }
                }
                else
                {
                    if (!(p1.Y > p2.Y && p2.Y > p3.Y))
                    {
                        stopAt = i;
                    }
                }
            }

            // 如果breakAt后面还有3点连续递增，再增加3个，便于拟合
            //if (stopAt <= sortedPointsToFilter.Count - 1 - 3)
            //{
            //    var p1 = sortedPointsToFilter[stopAt + 1];
            //    var p2 = sortedPointsToFilter[stopAt + 2];
            //    var p3 = sortedPointsToFilter[stopAt + 3];

            //    if (expr)
            //    {


            //    }
            //}

            if (breakAt != -1)
            {
                for (var i = 0; i <= breakAt; i++)
                {
                    pointsLeft.Add(sortedPointsToFilter[i]);
                }
            }
            else
            {
                VisionAnalysisMsg += FormatMsg("寻找拐点失败");
                return false;
            }

            if (stopAt == -1)
            {
                for (var i = breakAt; i <= sortedPointsToFilter.Count - 1; i++)
                {
                    pointsRight.Add(sortedPointsToFilter[i]);
                }
            }
            else
            {
                if (stopAt >= breakAt)
                {
                    for (var i = breakAt; i <= stopAt; i++)
                    {
                        pointsRight.Add(sortedPointsToFilter[i]);
                    }
                }
            }

            if (sortedPoints.Count <= 4)
            {
                VisionAnalysisMsg += FormatMsg("寻找拐点失败");
                return false;
            }

            ImgMats[CutOffLineContourImgName] = ImgMats[LowBeamSrc0Name].Clone(InterestedRoi);
            foreach (var point in pointsLeft)
                ImgMats[CutOffLineContourImgName].Set(point.Y, point.X, new Vec3b(0, 0, 255));  // 注意：OpenCV使用BGR顺序
            foreach (var point in pointsRight)
                ImgMats[CutOffLineContourImgName].Set(point.Y, point.X, new Vec3b(0, 255, 0));  // 注意：OpenCV使用BGR顺序

            if (pointsLeft.Count < 2 || pointsRight.Count < 2)
            {
                VisionAnalysisMsg += FormatMsg("寻找拐点失败");
                return false;
            }

            // 存储拟合的直线信息
            var allPoints = new List<List<Point>>
            {
                new List<Point>(),
                new List<Point>()
            };

            allPoints[0].AddRange(pointsLeft);
            allPoints[1].AddRange(pointsRight);

            var edgeLines = (from points in allPoints
                             select Cv2.FitLine(points, DistanceTypes.L2, 0, 0.01, 0.01)
                into lineParams
                             let vx = lineParams.Vx
                             let vy = lineParams.Vy
                             let x = lineParams.X1
                             let y = lineParams.Y1
                             let leftY = (int)(-x * vy / vx + y)
                             let rightY = (int)((cols - x) * vy / vx + y)
                             select new LineSegmentPoint(new Point(0, leftY), new Point(cols - 1, rightY))).ToList();

            if (edgeLines.Count == 2)
            {
                var p1 = GetIntersection(edgeLines[0].P1, edgeLines[0].P2, edgeLines[1].P1, edgeLines[1].P2);

                if (p1 == default)
                {
                    VisionAnalysisMsg += FormatMsg("寻找拐点失败");
                    return false;
                }

                //ImgMats[CutOffLineContourImgName].Set(p1.Y, p1.X, new Vec3b(0, 255, 255));  // 注意：OpenCV使用BGR顺序
                DrawXOnPoint(ImgMats[CutOffLineContourImgName], p1, Scalar.Yellow);

                _cutOffLineP1 = edgeLines[0].P1;
                _cutOffLineP2 = edgeLines[0].P2;
                _cutOffLineP3 = edgeLines[1].P1;
                _cutOffLineP4 = edgeLines[1].P2;

                // 计算水平截止线偏移旋转的角度
                {
                    var isLeftHigher = false;
                    if (_cutOffLineP1.Y != _cutOffLineP2.Y)
                    {
                        isLeftHigher = _cutOffLineP1.Y > _cutOffLineP2.Y;
                    }

                    float x1 = _cutOffLineP1.X, y1 = _cutOffLineP1.Y;
                    float x2 = _cutOffLineP2.X, y2 = _cutOffLineP2.Y;
                    float x3, y3, x4, y4;
                    if (isLeftHigher)
                    {
                        x3 = 0f;
                        y3 = _cutOffLineP2.Y;
                        x4 = rows;
                        y4 = _cutOffLineP2.Y;
                    }
                    else
                    {
                        x3 = 0f;
                        y3 = _cutOffLineP1.Y;
                        x4 = rows;
                        y4 = _cutOffLineP1.Y;
                    }

                    // 计算方向向量
                    float[] v1 = { x2 - x1, y2 - y1 }; // 第一条直线的方向向量
                    float[] v2 = { x4 - x3, y4 - y3 }; // 第二条直线的方向向量

                    // 计算方向向量的点积
                    var dotProduct = v1[0] * v2[0] + v1[1] * v2[1];

                    // 计算向量的模长
                    var magnitudeV1 = (float)Math.Sqrt(v1[0] * v1[0] + v1[1] * v1[1]);
                    var magnitudeV2 = (float)Math.Sqrt(v2[0] * v2[0] + v2[1] * v2[1]);

                    // 计算夹角的余弦值
                    var cosTheta = dotProduct / (magnitudeV1 * magnitudeV2);

                    // 确保余弦值在合法范围内
                    cosTheta = Math.Max(-1.0f, Math.Min(1.0f, cosTheta));

                    // 计算夹角（弧度）
                    var angle = (float)Math.Acos(cosTheta);

                    var toRotateAngle = isLeftHigher ? 360 - angle : angle;

                    if (ImgMats[LowBeamSrc0Name] != null && !ImgMats[LowBeamSrc0Name].Empty())
                    {
                        RotateImage(ImgMats[LowBeamSrc0Name], toRotateAngle);
                        //ImgMats[LowBeamSrc0Name].ImWrite(@"C:\Users\B1438\Desktop\新建文件夹\LowBeamSrc0Name+" + _enterTs.ToString() + ".jpg");
                    }

                    if (ImgMats[LowBeamSrc1Name] != null && !ImgMats[LowBeamSrc1Name].Empty())
                    {
                        RotateImage(ImgMats[LowBeamSrc1Name], toRotateAngle);
                        //ImgMats[LowBeamSrc1Name].ImWrite(@"C:\Users\B1438\Desktop\新建文件夹\LowBeamSrc1Name+" + _enterTs.ToString() + ".jpg");
                    }

                    if (ImgMats[LowBeamSrc2Name] != null && !ImgMats[LowBeamSrc2Name].Empty())
                    {
                        RotateImage(ImgMats[LowBeamSrc2Name], toRotateAngle);
                        //ImgMats[LowBeamSrc2Name].ImWrite(@"C:\Users\B1438\Desktop\新建文件夹\LowBeamSrc2Name+" + _enterTs.ToString() + ".jpg");
                    }

                    if (ImgMats[HighBeamSrcName] != null && !ImgMats[HighBeamSrcName].Empty())
                    {
                        RotateImage(ImgMats[HighBeamSrcName], toRotateAngle);
                        //ImgMats[HighBeamSrcName].ImWrite(@"C:\Users\B1438\Desktop\新建文件夹\HighBeamSrcName+" + _enterTs.ToString() + ".jpg");
                    }
                }

                _pCutOffLineCenter = p1;
                return true;
            }

            VisionAnalysisMsg += FormatMsg("寻找拐点失败");
            return false;
        }

        private static Point GetIntersection(Point a1, Point a2, Point b1, Point b2)
        {
            double deltaA = a2.X - a1.X;
            double deltaB = b2.X - b1.X;

            if (deltaA == 0 || deltaB == 0) // Check if either line is vertical
            {
                // If both lines are vertical, they are parallel or coincide
                if (Math.Abs(deltaA - deltaB) <= 0)
                    return default;
            }

            // Line A represented as a1x + a2y = c1
            double a1Line = a2.Y - a1.Y;
            double a2Line = a1.X - a2.X;
            var c1Line = a1Line * a1.X + a2Line * a1.Y;

            // Line B represented as b1x + b2y = c2
            double b1Line = b2.Y - b1.Y;
            double b2Line = b1.X - b2.X;
            var c2Line = b1Line * b1.X + b2Line * b1.Y;

            // Calculate the determinant of the coefficient matrix
            var det = a1Line * b2Line - b1Line * a2Line;

            if (det == 0) // Lines are parallel
                return default;

            // Calculate intersection point using Cramer's Rule
            var x = (b2Line * c1Line - a2Line * c2Line) / det;
            var y = (a1Line * c2Line - b1Line * c1Line) / det;

            return new Point((int)x, (int)y);
        }

        private static IEnumerable<Point> FilterAnomalousPoints(List<Point> points)
        {
            var filteredPoints = new List<Point>();
            var columnToRow = new Dictionary<int, int>();

            foreach (var point in points)
            {
                if (!columnToRow.ContainsKey(point.X))
                {
                    columnToRow[point.X] = point.Y;
                }
                else
                {
                    // 如果在同一列有多个点，选择Y值最小的（最上面的点）
                    columnToRow[point.X] = Math.Min(columnToRow[point.X], point.Y);
                }
            }

            var sortedColumns = columnToRow.Keys.OrderBy(k => k).ToList();

            for (var i = 0; i < sortedColumns.Count; i++)
            {
                var currentCol = sortedColumns[i];

                if (i == 0 || i == sortedColumns.Count - 1)
                {
                    // 总是保留第一列和最后一列的点
                    filteredPoints.Add(new Point(currentCol, columnToRow[currentCol]));
                }
                else
                {
                    var prevCol = sortedColumns[i - 1];
                    var nextCol = sortedColumns[i + 1];

                    var prevY = columnToRow[prevCol];
                    var currentY = columnToRow[currentCol];
                    var nextY = columnToRow[nextCol];

                    // 检查是否单调
                    if ((prevY <= currentY && currentY <= nextY) || (prevY >= currentY && currentY >= nextY))
                    {
                        filteredPoints.Add(new Point(currentCol, currentY));
                    }
                    // 如果不单调，这个点会被跳过（即被剔除）
                }
            }

            return filteredPoints;
        }

        private bool SetCoordinateSystem()
        {
            Ph0V0 = new Point((double)_pCutOffLineCenter.X, (double)_pCutOffLineCenter.Y - PerDegreePixel * 0.57);

            // 判断[H0,V0]点是否在图像内
            if (Ph0V0.X <= 0 || Ph0V0.X >= ImgMats[EdgeImgName].Width || Ph0V0.Y <= 0 || Ph0V0.Y >= ImgMats[EdgeImgName].Height)
            {
                VisionAnalysisMsg += FormatMsg("计算[H0,V0]点失败,超出图像范围");
                return false;
            }

            ImgMats[InflectionPointImgName] = ImgMats[LowBeamSrc0Name].Clone(InterestedRoi);
            Cv2.Line(ImgMats[InflectionPointImgName], _cutOffLineP1, _cutOffLineP2, Scalar.Red);
            Cv2.Line(ImgMats[InflectionPointImgName], _cutOffLineP3, _cutOffLineP4, Scalar.Blue);
            Cv2.Circle(ImgMats[InflectionPointImgName], Ph0V0, 1, Scalar.Green, -1);
            Cv2.Circle(ImgMats[InflectionPointImgName], _pCutOffLineCenter, 1, Scalar.Green, -1);

            // 计算截止线夹角
            {
                float x1 = _cutOffLineP1.X, y1 = _cutOffLineP1.Y;
                float x2 = _cutOffLineP2.X, y2 = _cutOffLineP2.Y;
                float x3 = _cutOffLineP3.X, y3 = _cutOffLineP3.Y;
                float x4 = _cutOffLineP4.X, y4 = _cutOffLineP4.Y;

                // 计算方向向量
                float[] v1 = { x2 - x1, y2 - y1 }; // 第一条直线的方向向量
                float[] v2 = { x4 - x3, y4 - y3 }; // 第二条直线的方向向量

                // 计算方向向量的点积
                var dotProduct = v1[0] * v2[0] + v1[1] * v2[1];

                // 计算向量的模长
                var magnitudeV1 = (float)Math.Sqrt(v1[0] * v1[0] + v1[1] * v1[1]);
                var magnitudeV2 = (float)Math.Sqrt(v2[0] * v2[0] + v2[1] * v2[1]);

                // 计算夹角的余弦值
                var cosTheta = dotProduct / (magnitudeV1 * magnitudeV2);

                // 确保余弦值在合法范围内
                cosTheta = Math.Max(-1.0f, Math.Min(1.0f, cosTheta));

                // 计算夹角（弧度）
                var angle = (float)Math.Acos(cosTheta);

                // 转换为度数
                OutputCutOffLineAngle = Math.Round(angle * (180.0f / (float)Math.PI), 2, MidpointRounding.AwayFromZero);
            }

            Cv2.PutText(ImgMats[InflectionPointImgName], string.Format(@"Angle: {0}", OutputCutOffLineAngle),
                new Point(_pCutOffLineCenter.X, _pCutOffLineCenter.Y + 10), HersheyFonts.HersheySimplex, 0.3, Scalar.Red);
            VisionAnalysisMsg += FormatMsg("绘制拐点结束");

            ImgMats[CoordinateSystemImgName] = ImgMats[LowBeamSrc0Name].Clone(InterestedRoi);
            DrawCoordinateSystem(ImgMats[CoordinateSystemImgName], Ph0V0, _pCutOffLineCenter);

            var toCheckLineX = (double)Ph0V0.X - PerDegreePixel * 2.5;
            _pHn2d5V2 = new Point(toCheckLineX, (double)Ph0V0.Y - PerDegreePixel * 2);
            _pHn2d5Vn2 = new Point(toCheckLineX, (double)Ph0V0.Y + PerDegreePixel * 2);
            DrawLowBeamGradientLine(ImgMats[CoordinateSystemImgName], _pHn2d5V2, _pHn2d5Vn2);
            var toCheckGradientImg = ImgMats[LowBeamSrc0Name].Clone(InterestedRoi);
            double hhSlope;
            var pH2d5Vn2d5 = new Point((double)Ph0V0.X + PerDegreePixel * 2.5, (double)Ph0V0.Y + PerDegreePixel * 2.5);
            OutputLbVvSlope0 = GetGradient(
                toCheckGradientImg, _pHn2d5V2, _pHn2d5Vn2, ImgMats[CoordinateSystemImgName], out hhSlope, Ph0V0, pH2d5Vn2d5);
            OutputLbHhSlope2 = hhSlope;
            toCheckGradientImg.Dispose();
            VisionAnalysisMsg += FormatMsg(string.Format("计算'{0}'的垂直梯度={1}", LowBeamSrc0Name, OutputLbVvSlope0));
            VisionAnalysisMsg += FormatMsg(string.Format("计算'{0}'的拐点梯度={1}", LowBeamSrc0Name, OutputLbHhSlope2));

            _lbH0V0 = DrawPointCircleText(ImgMats[CoordinateSystemImgName], Ph0V0, "0", "0", PerDegreePixel);

            _lbH0V2 = DrawPointCircleText(ImgMats[CoordinateSystemImgName], Ph0V0, "0", "2", PerDegreePixel);
            _lbH0V4 = DrawPointCircleText(ImgMats[CoordinateSystemImgName], Ph0V0, "0", "4", PerDegreePixel);
            _lbHn8V0 = DrawPointCircleText(ImgMats[CoordinateSystemImgName], Ph0V0, "-8", "0", PerDegreePixel);
            _lbHn8V4 = DrawPointCircleText(ImgMats[CoordinateSystemImgName], Ph0V0, "-8", "4", PerDegreePixel);
            _lbHn4V2 = DrawPointCircleText(ImgMats[CoordinateSystemImgName], Ph0V0, "-4", "2", PerDegreePixel);
            _lbHn4V0 = DrawPointCircleText(ImgMats[CoordinateSystemImgName], Ph0V0, "-4", "0", PerDegreePixel);
            _lbH4V2 = DrawPointCircleText(ImgMats[CoordinateSystemImgName], Ph0V0, "4", "2", PerDegreePixel);
            _lbH8V4 = DrawPointCircleText(ImgMats[CoordinateSystemImgName], Ph0V0, "8", "4", PerDegreePixel);

            Ph0V2 = new Point(_lbH0V2.Circle.Center.X, _lbH0V2.Circle.Center.Y);
            Ph0V4 = new Point(_lbH0V4.Circle.Center.X, _lbH0V4.Circle.Center.Y);
            PHn8V0 = new Point(_lbHn8V0.Circle.Center.X, _lbHn8V0.Circle.Center.Y);
            PHn8V4 = new Point(_lbHn8V4.Circle.Center.X, _lbHn8V4.Circle.Center.Y);
            PHn4V2 = new Point(_lbHn4V2.Circle.Center.X, _lbHn4V2.Circle.Center.Y);
            PHn4V0 = new Point(_lbHn4V0.Circle.Center.X, _lbHn4V0.Circle.Center.Y);
            Ph4V2 = new Point(_lbH4V2.Circle.Center.X, _lbH4V2.Circle.Center.Y);
            Ph8V4 = new Point(_lbH8V4.Circle.Center.X, _lbH8V4.Circle.Center.Y);

            _lbH0Vn11 = DrawPointCircleText(ImgMats[CoordinateSystemImgName], Ph0V0, "0", "-11", PerDegreePixel);

            Ph0Vn11 = new Point(_lbH0Vn11.Circle.Center.X, _lbH0Vn11.Circle.Center.Y);

            VisionAnalysisMsg += FormatMsg("绘制坐标系结束");

            return true;
        }

        private static void RotateImage(Mat src, double degree)
        {
            //旋转中心为图像中心    
            var center = new Point2f
            {
                X = (float)(src.Cols / 2.0),
                Y = (float)(src.Rows / 2.0)
            };
            //int length = 0;
            //length = (int)Math.Sqrt(src.Cols * src.Cols + src.Rows * src.Rows);
            //计算二维旋转的仿射变换矩阵  
            var m = Cv2.GetRotationMatrix2D(center, degree, 1);
            Size size;
            size.Width = src.Cols;
            size.Height = src.Rows;
            Cv2.WarpAffine(src, src, m, size, InterpolationFlags.Linear, 0, Scalar.Black);//仿射变换，背景色填充为黑 
        }

        private static void DrawCoordinateSystem(Mat src, Point pH0V0, Point? cutOffLineCenter = null)
        {
            Cv2.Line(src, new Point(0, pH0V0.Y), new Point(src.Width, pH0V0.Y), Scalar.Yellow, 2);
            Cv2.Line(src, new Point(pH0V0.X, 0), new Point(pH0V0.X, src.Height), Scalar.Yellow, 2);
            Cv2.Circle(src, pH0V0, 1, Scalar.Green, -1);

            if (cutOffLineCenter == null)
                return;
            var pCutOffLineCenter = (Point)cutOffLineCenter;
            Cv2.Circle(src, pCutOffLineCenter, 1, Scalar.Green, -1);
        }

        private void LowBeamAnalysis()
        {
            if (ImgMats[LowBeamSrc1Name] == null || ImgMats[LowBeamSrc1Name].Empty())
            {
                VisionAnalysisMsg += FormatMsg("近光图像缺失");
                return;
            }

            var toGetGrayMat = ImgMats[LowBeamSrc1Name].Clone(InterestedRoi);
            ImgMats[LowBeamAnalysisResult1ImgName] = ImgMats[LowBeamSrc1Name].Clone(InterestedRoi);
            DrawCoordinateSystem(ImgMats[LowBeamAnalysisResult1ImgName], Ph0V0, _pCutOffLineCenter);

            DrawLowBeamGradientLine(ImgMats[LowBeamAnalysisResult1ImgName], _pHn2d5V2, _pHn2d5Vn2);
            double hhSlope;
            var pH2d5Vn2d5 = new Point((double)Ph0V0.X + PerDegreePixel * 2.5, (double)Ph0V0.Y + PerDegreePixel * 2.5);
            OutputLbVvSlope1 = GetGradient(toGetGrayMat, _pHn2d5V2, _pHn2d5Vn2, ImgMats[LowBeamAnalysisResult1ImgName], out hhSlope, Ph0V0, pH2d5Vn2d5);
            OutputLbHhSlope3 = hhSlope;
            VisionAnalysisMsg += FormatMsg(string.Format("计算'{0}'垂直梯度={1}", LowBeamSrc1Name, OutputLbVvSlope1));
            VisionAnalysisMsg += FormatMsg(string.Format("计算'{0}'拐点梯度={1}", LowBeamSrc1Name, OutputLbHhSlope3));

            _lbH0V0.Gray = GetCircularRoiMeanGray(toGetGrayMat, _lbH0V0);
            _lbH0V2.Gray = GetCircularRoiMeanGray(toGetGrayMat, _lbH0V2);
            _lbH0V4.Gray = GetCircularRoiMeanGray(toGetGrayMat, _lbH0V4);
            _lbHn8V0.Gray = GetCircularRoiMeanGray(toGetGrayMat, _lbHn8V0);
            _lbHn8V4.Gray = GetCircularRoiMeanGray(toGetGrayMat, _lbHn8V4);
            _lbHn4V2.Gray = GetCircularRoiMeanGray(toGetGrayMat, _lbHn4V2);
            _lbHn4V0.Gray = GetCircularRoiMeanGray(toGetGrayMat, _lbHn4V0);
            _lbH4V2.Gray = GetCircularRoiMeanGray(toGetGrayMat, _lbH4V2);
            _lbH8V4.Gray = GetCircularRoiMeanGray(toGetGrayMat, _lbH8V4);

            OutputLbH0V0Gray = _lbH0V0.Gray;
            OutputLbH0V2Gray = _lbH0V2.Gray;
            OutputLbH0V4Gray = _lbH0V4.Gray;
            OutputLbHn8V0Gray = _lbHn8V0.Gray;
            OutputLbHn8V4Gray = _lbHn8V4.Gray;
            OutputLbHn4V2Gray = _lbHn4V2.Gray;
            OutputLbHn4V0Gray = _lbHn4V0.Gray;
            OutputLbH4V2Gray = _lbH4V2.Gray;
            OutputLbH8V4Gray = _lbH8V4.Gray;

            _lbH0Vn11.Gray = GetCircularRoiMeanGray(toGetGrayMat, _lbH0Vn11);
            OutputLbH0Vn11Gray = _lbH0Vn11.Gray;

            VisionAnalysisMsg += FormatMsg(string.Format("计算LB_{0}灰度值='{1}'", _lbH0V0.Name, _lbH0V0.Gray));
            DrawPointCircleText(ImgMats[LowBeamAnalysisResult1ImgName], Ph0V0, _lbH0V0.DegreeH, _lbH0V0.DegreeV, PerDegreePixel, true, _lbH0V0.Gray);

            VisionAnalysisMsg += FormatMsg(string.Format("计算LB_{0}灰度值='{1}'", _lbH0V2.Name, _lbH0V2.Gray));
            DrawPointCircleText(ImgMats[LowBeamAnalysisResult1ImgName], Ph0V0, _lbH0V2.DegreeH, _lbH0V2.DegreeV, PerDegreePixel, true, _lbH0V2.Gray);

            VisionAnalysisMsg += FormatMsg(string.Format("计算LB_{0}灰度值='{1}'", _lbH0V4.Name, _lbH0V4.Gray));
            DrawPointCircleText(ImgMats[LowBeamAnalysisResult1ImgName], Ph0V0, _lbH0V4.DegreeH, _lbH0V4.DegreeV, PerDegreePixel, true, _lbH0V4.Gray);

            VisionAnalysisMsg += FormatMsg(string.Format("计算LB_{0}灰度值='{1}'", _lbHn8V0.Name, _lbHn8V0.Gray));
            DrawPointCircleText(ImgMats[LowBeamAnalysisResult1ImgName], Ph0V0, _lbHn8V0.DegreeH, _lbHn8V0.DegreeV, PerDegreePixel, true, _lbHn8V0.Gray);

            VisionAnalysisMsg += FormatMsg(string.Format("计算LB_{0}灰度值='{1}'", _lbHn8V4.Name, _lbHn8V4.Gray));
            DrawPointCircleText(ImgMats[LowBeamAnalysisResult1ImgName], Ph0V0, _lbHn8V4.DegreeH, _lbHn8V4.DegreeV, PerDegreePixel, true, _lbHn8V4.Gray);

            VisionAnalysisMsg += FormatMsg(string.Format("计算LB_{0}灰度值='{1}'", _lbHn4V2.Name, _lbHn4V2.Gray));
            DrawPointCircleText(ImgMats[LowBeamAnalysisResult1ImgName], Ph0V0, _lbHn4V2.DegreeH, _lbHn4V2.DegreeV, PerDegreePixel, true, _lbHn4V2.Gray);

            VisionAnalysisMsg += FormatMsg(string.Format("计算LB_{0}灰度值='{1}'", _lbHn4V0.Name, _lbHn4V0.Gray));
            DrawPointCircleText(ImgMats[LowBeamAnalysisResult1ImgName], Ph0V0, _lbHn4V0.DegreeH, _lbHn4V0.DegreeV, PerDegreePixel, true, _lbHn4V0.Gray);

            VisionAnalysisMsg += FormatMsg(string.Format("计算LB_{0}灰度值='{1}'", _lbH4V2.Name, _lbH4V2.Gray));
            DrawPointCircleText(ImgMats[LowBeamAnalysisResult1ImgName], Ph0V0, _lbH4V2.DegreeH, _lbH4V2.DegreeV, PerDegreePixel, true, _lbH4V2.Gray);

            VisionAnalysisMsg += FormatMsg(string.Format("计算LB_{0}灰度值='{1}'", _lbH8V4.Name, _lbH8V4.Gray));
            DrawPointCircleText(ImgMats[LowBeamAnalysisResult1ImgName], Ph0V0, _lbH8V4.DegreeH, _lbH8V4.DegreeV, PerDegreePixel, true, _lbH8V4.Gray);

            VisionAnalysisMsg += FormatMsg(string.Format("计算LB_{0}灰度值='{1}'", _lbH0Vn11.Name, _lbH0Vn11.Gray));
            DrawPointCircleText(ImgMats[LowBeamAnalysisResult1ImgName], Ph0V0, _lbH0Vn11.DegreeH, _lbH0Vn11.DegreeV, PerDegreePixel, true, _lbH0Vn11.Gray);

            toGetGrayMat.Dispose();
            GC.Collect();

            if (ImgMats[LowBeamSrc0Name] == null || ImgMats[LowBeamSrc0Name].Empty())
            {
                VisionAnalysisMsg += FormatMsg("近光图像缺失");
                return;
            }

            toGetGrayMat = ImgMats[LowBeamSrc0Name].Clone(InterestedRoi);
            ImgMats[LowBeamAnalysisResult0ImgName] = ImgMats[LowBeamSrc0Name].Clone(InterestedRoi);
            DrawCoordinateSystem(ImgMats[LowBeamAnalysisResult0ImgName], Ph0V0, _pCutOffLineCenter);
            var perPixelDegree = 1 / PerDegreePixel;

            var pTopLeft = FormatPoint(Ph0V0, 0, 0.5, PerDegreePixel);
            var pTopRight = FormatPoint(Ph0V0, 5, 0.5, PerDegreePixel);
            var pButtomLeft = FormatPoint(Ph0V0, 0, -2, PerDegreePixel);

            Rect roi;
            var maxBrightestCenterLb = FindBrightestCirclesInRoi(toGetGrayMat, pTopLeft, pTopRight, pButtomLeft, (int)(0.1 * PerDegreePixel), out roi);
            _pLbEmax = new Point(maxBrightestCenterLb.Center.X, maxBrightestCenterLb.Center.Y);
            var maxBrghtestCenterLbDegreeH = Math.Round((maxBrightestCenterLb.Center.X - Ph0V0.X) * perPixelDegree, 1, MidpointRounding.AwayFromZero);
            var maxBrghtestCenterLbDegreeV = Math.Round((0 - (maxBrightestCenterLb.Center.Y - Ph0V0.Y)) * perPixelDegree, 1, MidpointRounding.AwayFromZero);
            var pEMaxLb = DrawPointCircleText(ImgMats[LowBeamAnalysisResult0ImgName], Ph0V0, maxBrghtestCenterLbDegreeH.ToString(CultureInfo.InvariantCulture), maxBrghtestCenterLbDegreeV.ToString(CultureInfo.InvariantCulture), PerDegreePixel); // LowBeam EMax
            var pEMaxGrayLb = GetCircularRoiMeanGray(toGetGrayMat, pEMaxLb);
            pEMaxLb.Gray = pEMaxGrayLb;
            //Cv2.Rectangle(ImgMats[LowBeamAnalysisResult1ImgName], roi, Scalar.Yellow);
            DrawPointCircleText(ImgMats[LowBeamAnalysisResult0ImgName], Ph0V0, pEMaxLb.DegreeH, pEMaxLb.DegreeV, PerDegreePixel, true, pEMaxLb.Gray);
            VisionAnalysisMsg += FormatMsg(string.Format("计算Lb_EMax结束，EMax在{0}，灰度值为{1}", pEMaxLb.Name, pEMaxLb.Gray));

            OutputLbEMaxH = maxBrghtestCenterLbDegreeH;
            OutputLbEMaxV = maxBrghtestCenterLbDegreeV;
            OutputLbEMaxGray = pEMaxLb.Gray;

            TransferServoPosistionLb();
        }

        private static void DrawLowBeamGradientLine(Mat src, Point p1, Point p2)
        {
            Cv2.Line(src, p1, p2, Scalar.Blue);
            Cv2.PutText(src, "[HV(-2.5,2)]", new Point(p1.X + 1, p1.Y - 1), HersheyFonts.HersheySimplex, 0.3, Scalar.Blue);
            Cv2.PutText(src, "[HV(-2.5,-2)]", new Point(p2.X + 1, p2.Y - 1), HersheyFonts.HersheySimplex, 0.3, Scalar.Blue);
        }

        private static double GetGradient(
            Mat toCheckSrc, Point pHn2d5V2, Point pHn2d5Vn2, Mat toShowSrc, out double hhGradient, Point pH0V0 = default, Point pH2d5Vn2d5 = default)
        {
            var grayImage = ImageDenoise(toCheckSrc).Clone();
            var otsuImg = grayImage.Clone();
            var otsu = Cv2.Threshold(otsuImg, otsuImg, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);
            otsuImg.Dispose();

            hhGradient = -9999;
            if (pH0V0 != default && pH2d5Vn2d5 != default)
            {
                // 计算拐点斜率
                {
                    var listPs = new List<Point>();

                    // 生成直线上的所有点
                    const float step = 1.0f; // 步长（间隔）
                    for (float t = 0; t <= 1; t += step / Math.Max(Math.Abs(pH2d5Vn2d5.X - pH0V0.X), Math.Abs(pH2d5Vn2d5.Y - pH0V0.Y)))
                    {
                        // 计算直线上的点
                        var xx = pH0V0.X + t * (pH2d5Vn2d5.X - pH0V0.X);
                        var yy = pH0V0.Y + t * (pH2d5Vn2d5.Y - pH0V0.Y);

                        // 打印点的坐标
                        listPs.Add(new Point(xx, yy));
                    }

                    listPs = listPs.OrderBy(f => f.X).ToList();

                    var grays = listPs.Select(p => grayImage.Get<byte>(p.Y, p.X)).Select(gray => (double)gray).ToList();

                    var firstIndex = grays.FindIndex(f => f >= otsu);
                    var maxGray = grays.Max();
                    var lastIndex = 0;

                    var newGrays = new List<double>();
                    for (var i = firstIndex; i < grays.Count; i++)
                    {
                        if (grays[i] < maxGray)
                        {
                            newGrays.Add(grays[i]);
                        }
                        else if (Math.Abs(grays[i] - maxGray) <= 0)

                        {
                            lastIndex = i;
                            newGrays.Add(grays[i]);
                            break;
                        }
                    }

                    var pX0 = firstIndex + pH0V0.X;
                    var pX1 = lastIndex + pH0V0.X;

                    for (var i = firstIndex; i <= lastIndex; i++)
                        toShowSrc.Set(listPs[i].Y, listPs[i].X, new Vec3b(0, 0, 255));

                    var x = new List<double>();
                    var y = new List<double>();

                    for (var i = 0; i < newGrays.Count; i++)
                    {
                        x.Add(i);
                        y.Add(newGrays[i]);
                    }

                    var n = x.Count;

                    // 计算必要的统计量
                    var sumX = x.Sum();
                    var sumY = y.Sum();
                    var sumXy = x.Zip(y, (a, b) => a * b).Sum();
                    var sumXSquare = x.Select(a => a * a).Sum();

                    // 计算斜率
                    hhGradient = Math.Round((n * sumXy - sumX * sumY) / (n * sumXSquare - sumX * sumX), 2, MidpointRounding.AwayFromZero);
                    Cv2.PutText(toShowSrc, string.Format("Slope={0}", hhGradient), new Point(listPs[lastIndex].X + 2, listPs[lastIndex].Y + 2), HersheyFonts.HersheySimplex, 0.4, Scalar.Red);
                }
            }

            // 计算V线斜率
            {
                var grays = new List<double>();

                for (var i = pHn2d5V2.Y; i <= pHn2d5Vn2.Y; i++)
                {
                    var gray = grayImage.Get<byte>(i, pHn2d5V2.X);
                    grays.Add(gray);
                }

                var firstIndex = grays.FindIndex(f => f >= otsu);
                var maxGray = grays.Max();
                var lastIndex = 0;

                var newGrays = new List<double>();
                for (var i = firstIndex; i < grays.Count; i++)
                {
                    if (grays[i] < maxGray)
                    {
                        newGrays.Add(grays[i]);
                    }
                    else if (Math.Abs(grays[i] - maxGray) <= 0)

                    {
                        lastIndex = i;
                        newGrays.Add(grays[i]);
                        break;
                    }
                }

                var pY0 = firstIndex + pHn2d5V2.Y;
                var pY1 = lastIndex + pHn2d5V2.Y;

                for (var i = pY0; i <= pY1; i++)
                    toShowSrc.Set(i, pHn2d5V2.X, new Vec3b(0, 0, 255));

                var x = new List<double>();
                var y = new List<double>();

                for (var i = 0; i < newGrays.Count; i++)
                {
                    x.Add(i);
                    y.Add(newGrays[i]);
                }

                var n = x.Count;

                // 计算必要的统计量
                var sumX = x.Sum();
                var sumY = y.Sum();
                var sumXy = x.Zip(y, (a, b) => a * b).Sum();
                var sumXSquare = x.Select(a => a * a).Sum();

                // 计算斜率
                var slope = Math.Round((n * sumXy - sumX * sumY) / (n * sumXSquare - sumX * sumX), 2,
                    MidpointRounding.AwayFromZero);
                Cv2.PutText(toShowSrc, string.Format("Slope={0}", slope), new Point(pHn2d5V2.X + 2, pY1), HersheyFonts.HersheySimplex, 0.4, Scalar.Red);

                return slope;
            }
        }

        private static double GetGradient(Mat toCheckSrc, Point pTopStart, Point pBottomEnd, Mat toShowSrc, double showResultOffsetY = 0)
        {
            var grayImage = ImageDenoise(toCheckSrc).Clone();
            var otsuImg = grayImage.Clone();
            var otsu = Cv2.Threshold(otsuImg, otsuImg, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);
            otsuImg.Dispose();
            var grays = new List<double>();

            for (var i = pTopStart.Y; i <= pBottomEnd.Y; i++)
            {
                var gray = grayImage.Get<byte>(i, pTopStart.X);
                grays.Add(gray);
            }

            var firstIndex = grays.FindIndex(f => f >= otsu);
            var maxGray = grays.Max();
            var lastIndex = 0;

            var newGrays = new List<double>();
            for (var i = firstIndex; i < grays.Count; i++)
            {
                if (grays[i] < maxGray)
                {
                    newGrays.Add(grays[i]);
                }
                else if (Math.Abs(grays[i] - maxGray) <= 0)

                {
                    lastIndex = i;
                    newGrays.Add(grays[i]);
                    break;
                }
            }

            var pY0 = firstIndex + pTopStart.Y;
            var pY1 = lastIndex + pTopStart.Y;

            for (var i = pY0; i <= pY1; i++)
                toShowSrc.Set(i, pTopStart.X, new Vec3b(0, 0, 255));

            var x = new List<double>();
            var y = new List<double>();

            for (var i = 0; i < newGrays.Count; i++)
            {
                x.Add(i);
                y.Add(newGrays[i]);
            }

            var n = x.Count;

            // 计算必要的统计量
            var sumX = x.Sum();
            var sumY = y.Sum();
            var sumXy = x.Zip(y, (a, b) => a * b).Sum();
            var sumXSquare = x.Select(a => a * a).Sum();

            // 计算斜率
            var slope = Math.Round((n * sumXy - sumX * sumY) / (n * sumXSquare - sumX * sumX), 2,
                MidpointRounding.AwayFromZero);
            //var slope = Math.Round(grays[firstIndex] / maxGray, 2, MidpointRounding.AwayFromZero);
            Cv2.PutText(toShowSrc, string.Format("Grad={0}", slope), new Point(pTopStart.X + 2, pY1 + showResultOffsetY), HersheyFonts.Italic, 0.32, Scalar.Red);

            return slope;
        }

        private void HighBeamAnalysis()
        {
            if (ImgMats[HighBeamSrcName] == null || ImgMats[HighBeamSrcName].Empty())
            {
                VisionAnalysisMsg += FormatMsg("远光图像缺失");
                return;
            }

            var toGetGrayMat = ImgMats[HighBeamSrcName].Clone(InterestedRoi);
            ImgMats[HighBeamAnalysisResultImgName] = ImgMats[HighBeamSrcName].Clone(InterestedRoi);
            DrawCoordinateSystem(ImgMats[HighBeamAnalysisResultImgName], Ph0V0, _pCutOffLineCenter);

            _hbHn11V0 = DrawPointCircleText(ImgMats[HighBeamAnalysisResultImgName], Ph0V0, "-2.58", "0", PerDegreePixel, isDraw: false, specialName: "H2");
            _hbH11V0 = DrawPointCircleText(ImgMats[HighBeamAnalysisResultImgName], Ph0V0, "2.58", "0", PerDegreePixel, isDraw: false, specialName: "H3");
            _hbHn20V0 = DrawPointCircleText(ImgMats[HighBeamAnalysisResultImgName], Ph0V0, "-5.14", "0", PerDegreePixel, isDraw: false, specialName: "H1");
            _hbH20V0 = DrawPointCircleText(ImgMats[HighBeamAnalysisResultImgName], Ph0V0, "5.14", "0", PerDegreePixel, isDraw: false, specialName: "H4");
            _hbH0V0 = DrawPointCircleText(ImgMats[HighBeamAnalysisResultImgName], Ph0V0, "0", "0", PerDegreePixel, isDraw: false, specialName: "HV");

            _hbHn11V0.Gray = GetCircularRoiMeanGray(toGetGrayMat, _hbHn11V0);
            _hbH11V0.Gray = GetCircularRoiMeanGray(toGetGrayMat, _hbH11V0);
            _hbHn20V0.Gray = GetCircularRoiMeanGray(toGetGrayMat, _hbHn20V0);
            _hbH20V0.Gray = GetCircularRoiMeanGray(toGetGrayMat, _hbH20V0);
            _hbH0V0.Gray = GetCircularRoiMeanGray(toGetGrayMat, _hbH0V0);

            OutputHbHn11V0Gray = _hbHn11V0.Gray;
            OutputHbH11V0Gray = _hbH11V0.Gray;
            OutputHbHn20V0Gray = _hbHn20V0.Gray;
            OutputHbH20V0Gray = _hbH20V0.Gray;
            OutputHbH0V0Gray = _hbH0V0.Gray;

            PHn11V0 = new Point(_hbHn11V0.Circle.Center.X, _hbHn11V0.Circle.Center.Y);
            Ph11V0 = new Point(_hbH11V0.Circle.Center.X, _hbH11V0.Circle.Center.Y);
            PHn20V0 = new Point(_hbHn20V0.Circle.Center.X, _hbHn20V0.Circle.Center.Y);
            Ph20V0 = new Point(_hbH20V0.Circle.Center.X, _hbH20V0.Circle.Center.Y);

            VisionAnalysisMsg += FormatMsg(string.Format("计算HB_{0}灰度值='{1}'", _hbHn11V0.Name, _hbHn11V0.Gray));
            DrawPointCircleText(ImgMats[HighBeamAnalysisResultImgName], Ph0V0, _hbHn11V0.DegreeH, _hbHn11V0.DegreeV, PerDegreePixel, true, _hbHn11V0.Gray, isDraw: true, specialName: "H2");

            VisionAnalysisMsg += FormatMsg(string.Format("计算HB_{0}灰度值='{1}'", _hbH11V0.Name, _hbH11V0.Gray));
            DrawPointCircleText(ImgMats[HighBeamAnalysisResultImgName], Ph0V0, _hbH11V0.DegreeH, _hbH11V0.DegreeV, PerDegreePixel, true, _hbH11V0.Gray, isDraw: true, specialName: "H3");

            VisionAnalysisMsg += FormatMsg(string.Format("计算HB_{0}灰度值='{1}'", _hbHn20V0.Name, _hbHn20V0.Gray));
            DrawPointCircleText(ImgMats[HighBeamAnalysisResultImgName], Ph0V0, _hbHn20V0.DegreeH, _hbHn20V0.DegreeV, PerDegreePixel, true, _hbHn20V0.Gray, isDraw: true, specialName: "H1");

            VisionAnalysisMsg += FormatMsg(string.Format("计算HB_{0}灰度值='{1}'", _hbH20V0.Name, _hbH20V0.Gray));
            DrawPointCircleText(ImgMats[HighBeamAnalysisResultImgName], Ph0V0, _hbH20V0.DegreeH, _hbH20V0.DegreeV, PerDegreePixel, true, _hbH20V0.Gray, isDraw: true, specialName: "H4");

            VisionAnalysisMsg += FormatMsg(string.Format("计算HB_{0}灰度值='{1}'", _hbH0V0.Name, _hbH0V0.Gray));
            DrawPointCircleText(ImgMats[HighBeamAnalysisResultImgName], Ph0V0, _hbH0V0.DegreeH, _hbH0V0.DegreeV, PerDegreePixel, true, _hbH0V0.Gray, isDraw: true, specialName: "HV");

            var perPixelDegree = 1 / PerDegreePixel;
            var pTopLeft = FormatPoint(Ph0V0, -5, 5, PerDegreePixel);
            var pTopRight = FormatPoint(Ph0V0, 5, 5, PerDegreePixel);
            var pButtomLeft = FormatPoint(Ph0V0, -5, -5, PerDegreePixel);

            Rect roi;
            var maxBrightestCenterHb = FindBrightestCirclesInRoi(toGetGrayMat, pTopLeft, pTopRight, pButtomLeft, (int)(0.1 * PerDegreePixel), out roi);
            _pHbEmax = maxBrightestCenterHb.Center;
            var maxBrghtestCenterHbDegreeH = Math.Round((maxBrightestCenterHb.Center.X - Ph0V0.X) * perPixelDegree, 1, MidpointRounding.AwayFromZero);
            var maxBrghtestCenterHbDegreeV = Math.Round((0 - (maxBrightestCenterHb.Center.Y - Ph0V0.Y)) * perPixelDegree, 1, MidpointRounding.AwayFromZero);
            var pEMaxHb = DrawPointCircleText(ImgMats[HighBeamAnalysisResultImgName], Ph0V0, maxBrghtestCenterHbDegreeH.ToString(CultureInfo.InvariantCulture), maxBrghtestCenterHbDegreeV.ToString(CultureInfo.InvariantCulture), PerDegreePixel, isDraw: false); // LowBeam EMax
            var pEMaxGrayHb = GetCircularRoiMeanGray(toGetGrayMat, pEMaxHb);
            pEMaxHb.Gray = pEMaxGrayHb;
            //Cv2.Rectangle(ImgMats[HighBeamAnalysisResultImgName], roi, Scalar.Yellow);
            DrawPointCircleText(ImgMats[HighBeamAnalysisResultImgName], Ph0V0, pEMaxHb.DegreeH, pEMaxHb.DegreeV, PerDegreePixel, true, pEMaxHb.Gray, specialName: "EMax");
            VisionAnalysisMsg += FormatMsg(string.Format("计算Hb_EMax结束，EMax在{0}，灰度值为{1}", pEMaxHb.Name, pEMaxHb.Gray));

            OutputHbEMaxH = maxBrghtestCenterHbDegreeH;
            OutputHbEMaxV = maxBrghtestCenterHbDegreeV;
            OutputHbEMaxGray = pEMaxHb.Gray;

            HighBeamH0V0EmaxGrayRatio = Math.Round((double)OutputHbH0V0Gray / (double)pEMaxHb.Gray, 2, MidpointRounding.AwayFromZero);
            VisionAnalysisMsg += FormatMsg(string.Format("计算HB_‘[HV(0,0)]={0}’/‘EMax={1}’=‘{2}’", OutputHbH0V0Gray, pEMaxHb.Gray, HighBeamH0V0EmaxGrayRatio));

            toGetGrayMat.Dispose();
            GC.Collect();

            TransferServoPosistionHb();
        }

        private static CircleWithGray DrawPointCircleText(Mat mat, Point pH0V0, string degreeH, string degreeV,
            double perDegreePixel, bool isShowGray = false, byte gray = 0, string specialName = "", bool isDraw = true)
        {
            var h = double.Parse(degreeH);
            var v = double.Parse(degreeV);
            var radius = (int)(0.05 * perDegreePixel);

            var pToDraw0 = FormatPoint(pH0V0, h, v, perDegreePixel);
            var pName = string.IsNullOrEmpty(specialName) ? string.Format("[HV({0},{1})]", degreeH, degreeV) : specialName;
            if (isShowGray)
                pName += string.Format("='{0}'", gray);

            if (isDraw)
            {
                if (string.IsNullOrEmpty(specialName))
                {
                    Cv2.Circle(mat, pToDraw0, 1, Scalar.Red, -1);
                    Cv2.PutText(mat, pName, new Point(pToDraw0.X + 1, pToDraw0.Y - radius - 10), HersheyFonts.Italic, 0.35, Scalar.Red);
                    Cv2.Circle(mat, pToDraw0, radius, Scalar.Red);
                }
                else
                {
                    Cv2.Circle(mat, pToDraw0, 1, Scalar.Red, -1);
                    Cv2.PutText(mat, pName, new Point(pToDraw0.X + radius + 1, pToDraw0.Y - 1), HersheyFonts.Italic, 0.35, Scalar.Red);
                    Cv2.Circle(mat, pToDraw0, radius, Scalar.Red);
                }
            }

            var circle = new CircleWithGray
            {
                Circle = new CircleSegment(pToDraw0, radius),
                Name = pName,
                DegreeH = degreeH,
                DegreeV = degreeV
            };
            return circle;
        }

        private static Point FormatPoint(Point pH0V0, double degreeH, double degreeV, double perPixelDegree)
        {
            var x = pH0V0.X + (degreeH * perPixelDegree);
            var y = pH0V0.Y - (degreeV * perPixelDegree);

            return new Point(x, y);
        }

        private static byte GetCircularRoiMeanGray(Mat src, CircleWithGray circle)
        {
            var grayImage = src.Clone();

            if (grayImage.Channels() == 3)
                Cv2.CvtColor(grayImage, grayImage, ColorConversionCodes.BGR2GRAY);

            // 图像降噪
            Cv2.GaussianBlur(grayImage, grayImage, new Size(3, 3), 0, borderType: BorderTypes.Default);
            Cv2.MedianBlur(grayImage, grayImage, 3);

            // 创建一个与原始图像相同大小的掩膜
            var mask = new Mat(grayImage.Size(), MatType.CV_8UC1, Scalar.Black);
            // 在掩膜上绘制白色圆形
            Cv2.Circle(mask, new Point((int)circle.Circle.Center.X, (int)circle.Circle.Center.Y), (int)circle.Circle.Radius, Scalar.White, -1);
            // 计算圆形区域的平均值
            var meanVal = Cv2.Mean(grayImage, mask);

            grayImage.Dispose();
            mask.Dispose();
            GC.Collect();

            // 返回平均灰度值
            return (byte)Math.Round(meanVal.Val0, 0, MidpointRounding.AwayFromZero);
        }

        public CircleInfo FindBrightestCirclesInRoi(
            Mat src, Point pTopLeft, Point pTopRight, Point pButtomLeft, int circleRadius, out Rect roi)
        {
            roi = new Rect(pTopLeft, new Size(pTopRight.X - pTopLeft.X, pButtomLeft.Y - pTopLeft.Y));

            var grayMat = src.Clone();
            if (grayMat.Channels() == 3)
                Cv2.CvtColor(grayMat, grayMat, ColorConversionCodes.BGR2GRAY);

            // 图像降噪
            Cv2.GaussianBlur(grayMat, grayMat, new Size(3, 3), 0, borderType: BorderTypes.Default);
            Cv2.MedianBlur(grayMat, grayMat, 3);

            // 找出图像中的最大亮度值
            var toFindMaxMat = new Mat();
            var tileGridSize = new Size(8, 8);
            var clahe = Cv2.CreateCLAHE(40, tileGridSize);
            clahe.Apply(grayMat, toFindMaxMat);

            double minVal, maxVal;
            Point minP, maxP;
            Cv2.MinMaxLoc(toFindMaxMat, out minVal, out maxVal, out minP, out maxP);
            // 设置阈值，例如取最大亮度的80%
            var threshold = maxVal - 20;

            // 阈值分割
            var binaryImage = new Mat();
            Cv2.Threshold(toFindMaxMat, binaryImage, threshold, 255, ThresholdTypes.Binary);
            toFindMaxMat.Dispose();
            GC.Collect();

            // 在二值图像中找到连通组件
            var labels = new Mat();
            var stats = new Mat();
            var centroids = new Mat();
            var nLabels = Cv2.ConnectedComponentsWithStats(binaryImage, labels, stats, centroids);

            var maxArea = -1;
            var maxAreaIndex = -1;

            for (var i = 1; i < nLabels; i++) // 跳过背景（标签0）
            {
                // 使用 Mat.Get<T> 方法获取统计数据
                var x = stats.Get<int>(i, 0);
                var y = stats.Get<int>(i, 1);
                var width = stats.Get<int>(i, 2);
                var height = stats.Get<int>(i, 3);
                var area = stats.Get<int>(i, 4);

                if (area >= maxArea)
                {
                    maxAreaIndex = i;
                    maxArea = area;
                }
            }

            binaryImage.Dispose();
            GC.Collect();

            Point brightnessPoint = default;
            var circleInfo = new CircleInfo { Brightness = -9999, Center = default };

            if (maxAreaIndex != -1)
            {
                // 计算精确中心
                var mask = new Mat();
                Cv2.InRange(labels, new Scalar(maxAreaIndex), new Scalar(maxAreaIndex), mask);
                var moments = Cv2.Moments(mask, true);
                var center = new Point(moments.M10 / moments.M00, moments.M01 / moments.M00);
                brightnessPoint = center;

                var roiMat = new Rect((int)(brightnessPoint.X - 1), (int)(brightnessPoint.Y - 1), 3, 3);
                var roiMat11111 = new Mat(grayMat, roiMat);
                var gray = Cv2.Mean(roiMat11111);

                circleInfo.Brightness = Math.Round(gray.Val0, 0, MidpointRounding.AwayFromZero);
                circleInfo.Center = brightnessPoint;
            }

            grayMat.Dispose();
            return circleInfo;
        }

        private static void DrawXOnPoint(Mat image, Point p, Scalar color)
        {
            var p1 = new Point(p.X - 1, p.Y - 1);
            var p2 = new Point(p.X + 1, p.Y - 1);
            var p3 = new Point(p.X - 1, p.Y + 1);
            var p4 = new Point(p.X + 1, p.Y + 1);
            var vec3B = color.ToVec3b();
            image.Set(p1.Y, p1.X, vec3B);  // 注意：OpenCV使用BGR顺序
            image.Set(p2.Y, p2.X, vec3B);  // 注意：OpenCV使用BGR顺序
            image.Set(p3.Y, p3.X, vec3B);  // 注意：OpenCV使用BGR顺序
            image.Set(p4.Y, p4.X, vec3B);  // 注意：OpenCV使用BGR顺序
            image.Set(p.Y, p.X, vec3B);  // 注意：OpenCV使用BGR顺序
        }

        public class CircleInfo
        {
            public Point Center { get; set; }
            public double Brightness { get; set; }
        }

        public double OutputLbVvSlope0;
        public double OutputLbVvSlope1;
        public double OutputLbHhSlope2;
        public double OutputLbHhSlope3;

        public double OutputLbH0V0Gray;
        public double OutputLbH0V2Gray;
        public double OutputLbH0V4Gray;
        public double OutputLbHn8V0Gray;
        public double OutputLbHn8V4Gray;
        public double OutputLbHn4V2Gray;
        public double OutputLbHn4V0Gray;
        public double OutputLbH4V2Gray;
        public double OutputLbH8V4Gray;
        public double OutputLbH0Vn11Gray;

        public double OutputLbEMaxH;
        public double OutputLbEMaxV;
        public double OutputLbEMaxGray;

        public double OutputHbHn11V0Gray; // H2
        public double OutputHbH11V0Gray; // H3
        public double OutputHbHn20V0Gray; // H1
        public double OutputHbH20V0Gray; // H4
        public double OutputHbH0V0Gray;

        public double HighBeamH0V0EmaxGrayRatio;

        public double OutputHbEMaxH;
        public double OutputHbEMaxV;
        public double OutputHbEMaxGray;

        #region 法规测试/灰度值 20240923

        public double OutputGrad2d5L = -9999; // -2.5, -1.50~1.50
        public double OutputGrad1R = -9999; // 1.00, -2.00~2.00
        public double OutputGrad2R = -9999; // 2.00, -2.00~2.00
        public double OutputGrad3R = -9999; // 3.00, -2.00~2.00

        public double OutputB50LGray = -9999; // -3.43, 0.57
        public double Output75RGray = -9999; // 1.15, 0.57
        public double Output75LGray = -9999; // -3.43, -0.57
        public double Output50LGray = -9999; // -3.43, -0.86
        public double Output50RGray = -9999; // 1.72, -0.86
        public double Output50VGray = -9999; // 0.00, -0.86
        public double Output25LGray = -9999; // -9.00, -1.72
        public double Output25RGray = -9999; // 9.00, -1.72
        public double OutputHvGray = -9999; // 0.00, 0.00
        public double OutputP1Gray = -9999; // -8.00, 4.00
        public double OutputP2Gray = -9999; // 0.00, 4.00
        public double OutputP3Gray = -9999; // 8.00, 4.00
        public double OutputP4Gray = -9999; // -4.00, 2.00
        public double OutputP5Gray = -9999; // 0.00, 2.00
        public double OutputP6Gray = -9999; // 4.00, 2.00
        public double OutputP7Gray = -9999; // -8.00, 0.00
        public double OutputP8Gray = -9999;// -4.00, 0.00

        public float OutputB50LLuminance = -9999; // -3.43, 0.57
        public float Output75RLuminance = -9999; // 1.15, 0.57
        public float Output75LLuminance = -9999; // -3.43, -0.57
        public float Output50LLuminance = -9999; // -3.43, -0.86
        public float Output50RLuminance = -9999; // 1.72, -0.86
        public float Output50VLuminance = -9999; // 0.00, -0.86
        public float Output25LLuminance = -9999; // -9.00, -1.72
        public float Output25RLuminance = -9999; // 9.00, -1.72
        public float OutputHvLuminance = -9999; // 0.00, 0.00
        public float OutputP1Luminance = -9999; // -8.00, 4.00
        public float OutputP2Luminance = -9999; // 0.00, 4.00
        public float OutputP3Luminance = -9999; // 8.00, 4.00
        public float OutputP4Luminance = -9999; // -4.00, 2.00
        public float OutputP5Luminance = -9999; // 0.00, 2.00
        public float OutputP6Luminance = -9999; // 4.00, 2.00
        public float OutputP7Luminance = -9999; // -8.00, 0.00
        public float OutputP8Luminance = -9999;// -4.00, 0.00

        public float OutputB50LColor = -9999; // -3.43, 0.57
        public float Output75RColor = -9999; // 1.15, 0.57
        public float Output75LColor = -9999; // -3.43, -0.57
        public float Output50LColor = -9999; // -3.43, -0.86
        public float Output50RColor = -9999; // 1.72, -0.86
        public float Output50VColor = -9999; // 0.00, -0.86
        public float Output25LColor = -9999; // -9.00, -1.72
        public float Output25RColor = -9999; // 9.00, -1.72
        public float OutputHvColor = -9999; // 0.00, 0.00
        public float OutputP1Color = -9999; // -8.00, 4.00
        public float OutputP2Color = -9999; // 0.00, 4.00
        public float OutputP3Color = -9999; // 8.00, 4.00
        public float OutputP4Color = -9999; // -4.00, 2.00
        public float OutputP5Color = -9999; // 0.00, 2.00
        public float OutputP6Color = -9999; // 4.00, 2.00
        public float OutputP7Color = -9999; // -8.00, 0.00
        public float OutputP8Color = -9999;// -4.00, 0.00

        public float OutputTransferB50LServoX = -9999; // -3.43, 0.57
        public float OutputTransfer75RServoX = -9999; // 1.15, 0.57
        public float OutputTransfer75LServoX = -9999; // -3.43, -0.57
        public float OutputTransfer50LServoX = -9999; // -3.43, -0.86
        public float OutputTransfer50RServoX = -9999; // 1.72, -0.86
        public float OutputTransfer50VServoX = -9999; // 0.00, -0.86
        public float OutputTransfer25LServoX = -9999; // -9.00, -1.72
        public float OutputTransfer25RServoX = -9999; // 9.00, -1.72
        public float OutputTransferHvServoX = -9999; // 0.00, 0.00
        public float OutputTransferP1ServoX = -9999; // -8.00, 4.00
        public float OutputTransferP2ServoX = -9999; // 0.00, 4.00
        public float OutputTransferP3ServoX = -9999; // 8.00, 4.00
        public float OutputTransferP4ServoX = -9999; // -4.00, 2.00
        public float OutputTransferP5ServoX = -9999; // 0.00, 2.00
        public float OutputTransferP6ServoX = -9999; // 4.00, 2.00
        public float OutputTransferP7ServoX = -9999; // -8.00, 0.00
        public float OutputTransferP8ServoX = -9999;// -4.00, 0.00

        public float OutputTransferB50LServoZ = -9999; // -3.43, 0.57
        public float OutputTransfer75RServoZ = -9999; // 1.15, 0.57
        public float OutputTransfer75LServoZ = -9999; // -3.43, -0.57
        public float OutputTransfer50LServoZ = -9999; // -3.43, -0.86
        public float OutputTransfer50RServoZ = -9999; // 1.72, -0.86
        public float OutputTransfer50VServoZ = -9999; // 0.00, -0.86
        public float OutputTransfer25LServoZ = -9999; // -9.00, -1.72
        public float OutputTransfer25RServoZ = -9999; // 9.00, -1.72
        public float OutputTransferHvServoZ = -9999; // 0.00, 0.00
        public float OutputTransferP1ServoZ = -9999; // -8.00, 4.00
        public float OutputTransferP2ServoZ = -9999; // 0.00, 4.00
        public float OutputTransferP3ServoZ = -9999; // 8.00, 4.00
        public float OutputTransferP4ServoZ = -9999; // -4.00, 2.00
        public float OutputTransferP5ServoZ = -9999; // 0.00, 2.00
        public float OutputTransferP6ServoZ = -9999; // 4.00, 2.00
        public float OutputTransferP7ServoZ = -9999; // -8.00, 0.00
        public float OutputTransferP8ServoZ = -9999;// -4.00, 0.00

        private void ResetRegulatoryRequirementResult()
        {
            OutputB50LGray = -9999; // -3.43, 0.57
            Output75RGray = -9999; // 1.15, 0.57
            Output75LGray = -9999; // -3.43, -0.57
            Output50LGray = -9999; // -3.43, -0.86
            Output50RGray = -9999; // 1.72, -0.86
            Output50VGray = -9999; // 0.00, -0.86
            Output25LGray = -9999; // -9.00, -1.72
            Output25RGray = -9999; // 9.00, -1.72
            OutputHvGray = -9999; // 0.00, 0.00
            OutputP1Gray = -9999; // -8.00, 4.00
            OutputP2Gray = -9999; // 0.00, 4.00
            OutputP3Gray = -9999; // 8.00, 4.00
            OutputP4Gray = -9999; // -4.00, 2.00
            OutputP5Gray = -9999; // 0.00, 2.00
            OutputP6Gray = -9999; // 4.00, 2.00
            OutputP7Gray = -9999; // -8.00, 0.00
            OutputP8Gray = -9999;// -4.00, 0.00

            OutputGrad2d5L = -9999; // -2.5, -1.50~1.50
            OutputGrad1R = -9999; // 1.00, -2.00~2.00
            OutputGrad2R = -9999; // 2.00, -2.00~2.00
            OutputGrad3R = -9999; // 3.00, -2.00~2.00

            // 亮色度数据
            {
                OutputB50LLuminance = -9999; // -3.43, 0.57
                Output75RLuminance = -9999; // 1.15, 0.57
                Output75LLuminance = -9999; // -3.43, -0.57
                Output50LLuminance = -9999; // -3.43, -0.86
                Output50RLuminance = -9999; // 1.72, -0.86
                Output50VLuminance = -9999; // 0.00, -0.86
                Output25LLuminance = -9999; // -9.00, -1.72
                Output25RLuminance = -9999; // 9.00, -1.72
                OutputHvLuminance = -9999; // 0.00, 0.00
                OutputP1Luminance = -9999; // -8.00, 4.00
                OutputP2Luminance = -9999; // 0.00, 4.00
                OutputP3Luminance = -9999; // 8.00, 4.00
                OutputP4Luminance = -9999; // -4.00, 2.00
                OutputP5Luminance = -9999; // 0.00, 2.00
                OutputP6Luminance = -9999; // 4.00, 2.00
                OutputP7Luminance = -9999; // -8.00, 0.00
                OutputP8Luminance = -9999;// -4.00, 0.00

                OutputB50LColor = -9999; // -3.43, 0.57
                Output75RColor = -9999; // 1.15, 0.57
                Output75LColor = -9999; // -3.43, -0.57
                Output50LColor = -9999; // -3.43, -0.86
                Output50RColor = -9999; // 1.72, -0.86
                Output50VColor = -9999; // 0.00, -0.86
                Output25LColor = -9999; // -9.00, -1.72
                Output25RColor = -9999; // 9.00, -1.72
                OutputHvColor = -9999; // 0.00, 0.00
                OutputP1Color = -9999; // -8.00, 4.00
                OutputP2Color = -9999; // 0.00, 4.00
                OutputP3Color = -9999; // 8.00, 4.00
                OutputP4Color = -9999; // -4.00, 2.00
                OutputP5Color = -9999; // 0.00, 2.00
                OutputP6Color = -9999; // 4.00, 2.00
                OutputP7Color = -9999; // -8.00, 0.00
                OutputP8Color = -9999;// -4.00, 0.00
            }

            _pointB50L = new Point(); // -3.43, 0.57
            _point75R = new Point(); // 1.15, 0.57
            _point75L = new Point(); // -3.43, -0.57
            _point50L = new Point(); // -3.43, -0.86
            _point50R = new Point(); // 1.72, -0.86
            _point50V = new Point(); // 0.00, -0.86
            _point25L = new Point(); // -9.00, -1.72
            _point25R = new Point(); // 9.00, -1.72
            _pointHv = new Point(); // 0.00, 0.00
            _pointP1 = new Point(); // -8.00, 4.00
            _pointP2 = new Point(); // 0.00, 4.00
            _pointP3 = new Point(); // 8.00, 4.00
            _pointP4 = new Point(); // -4.00, 2.00
            _pointP5 = new Point(); // 0.00, 2.00
            _pointP6 = new Point(); // 4.00, 2.00
            _pointP7 = new Point(); // -8.00, 0.00
            _pointP8 = new Point();// -4.00, 0.00

            _pointGrad2d5L = new Point(); // -2.5, -1.50~1.50
            _pointGrad1R = new Point(); // 1.00, -2.00~2.00
            _pointGrad2R = new Point(); // 2.00, -2.00~2.00
            _pointGrad3R = new Point(); // 3.00, -2.00~2.00
        }

        private Point _pointB50L = new Point(); // -3.43, 0.57
        private Point _point75R = new Point(); // 1.15, 0.57
        private Point _point75L = new Point(); // -3.43, -0.57
        private Point _point50L = new Point(); // -3.43, -0.86
        private Point _point50R = new Point(); // 1.72, -0.86
        private Point _point50V = new Point(); // 0.00, -0.86
        private Point _point25L = new Point(); // -9.00, -1.72
        private Point _point25R = new Point(); // 9.00, -1.72
        private Point _pointHv = new Point(); // 0.00, 0.00
        private Point _pointP1 = new Point(); // -8.00, 4.00
        private Point _pointP2 = new Point(); // 0.00, 4.00
        private Point _pointP3 = new Point(); // 8.00, 4.00
        private Point _pointP4 = new Point(); // -4.00, 2.00
        private Point _pointP5 = new Point(); // 0.00, 2.00
        private Point _pointP6 = new Point(); // 4.00, 2.00
        private Point _pointP7 = new Point(); // -8.00, 0.00
        private Point _pointP8 = new Point(); // -4.00, 0.00
        private Point _pointGrad2d5L = new Point(); // -2.5, -1.50~1.50
        private Point _pointGrad1R = new Point(); // 1.00, -2.00~2.00
        private Point _pointGrad2R = new Point(); // 2.00, -2.00~2.00
        private Point _pointGrad3R = new Point(); // 3.00, -2.00~2.00

        private void GetRegulatoryRequirementResult()
        {
            if (ImgMats[LowBeamSrc1Name] == null || ImgMats[LowBeamSrc1Name].Empty())
            {
                VisionAnalysisMsg += FormatMsg("近光图像1缺失");
                return;
            }

            if (ImgMats[LowBeamSrc2Name] == null || ImgMats[LowBeamSrc2Name].Empty())
            {
                VisionAnalysisMsg += FormatMsg("近光图像2缺失");
                return;
            }

            if (Ph0V0 != default)
            {
                // 低曝光
                {
                    var toGetGrayMat = ImgMats[LowBeamSrc0Name].Clone(InterestedRoi);
                    ImgMats[LowBeamAnalysisResult2ImgName] = ImgMats[LowBeamSrc0Name].Clone(InterestedRoi);
                    DrawCoordinateSystem(ImgMats[LowBeamAnalysisResult2ImgName], Ph0V0, _pCutOffLineCenter);

                    var p75R = DrawPointCircleText(ImgMats[LowBeamAnalysisResult2ImgName], Ph0V0, "1.15", "-0.57", PerDegreePixel, specialName: "75R", isDraw: false);
                    Output75RGray = p75R.Gray = GetCircularRoiMeanGray(toGetGrayMat, p75R);
                    DrawPointCircleText(ImgMats[LowBeamAnalysisResult2ImgName], Ph0V0, p75R.DegreeH, p75R.DegreeV, PerDegreePixel, true, p75R.Gray, specialName: "75R", isDraw: true);
                    _point75R = new Point(p75R.Circle.Center.X, p75R.Circle.Center.Y);

                    var p50L = DrawPointCircleText(ImgMats[LowBeamAnalysisResult2ImgName], Ph0V0, "-3.43", "-0.86", PerDegreePixel, specialName: "50L", isDraw: false);
                    Output50LGray = p50L.Gray = GetCircularRoiMeanGray(toGetGrayMat, p50L);
                    DrawPointCircleText(ImgMats[LowBeamAnalysisResult2ImgName], Ph0V0, p50L.DegreeH, p50L.DegreeV, PerDegreePixel, true, p50L.Gray, specialName: "50L", isDraw: true);
                    _point50L = new Point(p50L.Circle.Center.X, p50L.Circle.Center.Y);

                    var p50R = DrawPointCircleText(ImgMats[LowBeamAnalysisResult2ImgName], Ph0V0, "1.72", "-0.86", PerDegreePixel, specialName: "B_50_L", isDraw: false);
                    Output50RGray = p50R.Gray = GetCircularRoiMeanGray(toGetGrayMat, p50R);
                    DrawPointCircleText(ImgMats[LowBeamAnalysisResult2ImgName], Ph0V0, p50R.DegreeH, p50R.DegreeV, PerDegreePixel, true, p50R.Gray, specialName: "50R", isDraw: true);
                    _point50R = new Point(p50R.Circle.Center.X, p50R.Circle.Center.Y);

                    var p50V = DrawPointCircleText(ImgMats[LowBeamAnalysisResult2ImgName], Ph0V0, "0", "-0.86", PerDegreePixel, specialName: "50V", isDraw: false);
                    Output50VGray = p50V.Gray = GetCircularRoiMeanGray(toGetGrayMat, p50V);
                    DrawPointCircleText(ImgMats[LowBeamAnalysisResult2ImgName], Ph0V0, p50V.DegreeH, p50V.DegreeV, PerDegreePixel, true, p50V.Gray, specialName: "50V", isDraw: true);
                    _point50V = new Point(p50V.Circle.Center.X, p50V.Circle.Center.Y);

                    var p25L = DrawPointCircleText(ImgMats[LowBeamAnalysisResult2ImgName], Ph0V0, "-9.00", "-1.72", PerDegreePixel, specialName: "25L", isDraw: false);
                    Output25LGray = p25L.Gray = GetCircularRoiMeanGray(toGetGrayMat, p25L);
                    DrawPointCircleText(ImgMats[LowBeamAnalysisResult2ImgName], Ph0V0, p25L.DegreeH, p25L.DegreeV, PerDegreePixel, true, p25L.Gray, specialName: "25L", isDraw: true);
                    _point25L = new Point(p25L.Circle.Center.X, p25L.Circle.Center.Y);

                    var p25R = DrawPointCircleText(ImgMats[LowBeamAnalysisResult2ImgName], Ph0V0, "9.00", "-1.72", PerDegreePixel, specialName: "25R", isDraw: false);
                    Output25RGray = p25R.Gray = GetCircularRoiMeanGray(toGetGrayMat, p25R);
                    DrawPointCircleText(ImgMats[LowBeamAnalysisResult2ImgName], Ph0V0, p25R.DegreeH, p25R.DegreeV, PerDegreePixel, true, p25R.Gray, specialName: "25R", isDraw: true);
                    _point25R = new Point(p25R.Circle.Center.X, p25R.Circle.Center.Y);

                    toGetGrayMat.Dispose();
                }

                // 中等曝光
                {
                    var toGetGrayMat = ImgMats[LowBeamSrc1Name].Clone(InterestedRoi);
                    ImgMats[LowBeamAnalysisResult3ImgName] = ImgMats[LowBeamSrc1Name].Clone(InterestedRoi);
                    DrawCoordinateSystem(ImgMats[LowBeamAnalysisResult3ImgName], Ph0V0, _pCutOffLineCenter);

                    var p75L = DrawPointCircleText(ImgMats[LowBeamAnalysisResult3ImgName], Ph0V0, "-3.43", "-0.59", PerDegreePixel, specialName: "75L", isDraw: false);
                    Output75LGray = p75L.Gray = GetCircularRoiMeanGray(toGetGrayMat, p75L);
                    DrawPointCircleText(ImgMats[LowBeamAnalysisResult3ImgName], Ph0V0, p75L.DegreeH, p75L.DegreeV, PerDegreePixel, true, p75L.Gray, specialName: "75L", isDraw: true);
                    _point75L = new Point(p75L.Circle.Center.X, p75L.Circle.Center.Y);

                    var p7 = DrawPointCircleText(ImgMats[LowBeamAnalysisResult3ImgName], Ph0V0, "-8", "0.02", PerDegreePixel, specialName: "P7", isDraw: false);
                    OutputP7Gray = p7.Gray = GetCircularRoiMeanGray(toGetGrayMat, p7);
                    DrawPointCircleText(ImgMats[LowBeamAnalysisResult3ImgName], Ph0V0, p7.DegreeH, p7.DegreeV, PerDegreePixel, true, p7.Gray, specialName: "P7", isDraw: true);
                    _pointP7 = new Point(p7.Circle.Center.X, p7.Circle.Center.Y);

                    toGetGrayMat.Dispose();
                }

                // 高曝光
                {
                    var toGetGrayMat = ImgMats[LowBeamSrc2Name].Clone(InterestedRoi);
                    ImgMats[LowBeamAnalysisResult4ImgName] = ImgMats[LowBeamSrc2Name].Clone(InterestedRoi);
                    DrawCoordinateSystem(ImgMats[LowBeamAnalysisResult4ImgName], Ph0V0, _pCutOffLineCenter);

                    var pB50L = DrawPointCircleText(ImgMats[LowBeamAnalysisResult4ImgName], Ph0V0, "-3.43", "0.57", PerDegreePixel, specialName: "B_50_L", isDraw: false);
                    OutputB50LGray = pB50L.Gray = GetCircularRoiMeanGray(toGetGrayMat, pB50L);
                    DrawPointCircleText(ImgMats[LowBeamAnalysisResult4ImgName], Ph0V0, pB50L.DegreeH, pB50L.DegreeV, PerDegreePixel, true, pB50L.Gray, specialName: "B_50_L", isDraw: true);
                    _pointB50L = new Point(pB50L.Circle.Center.X, pB50L.Circle.Center.Y);

                    var pHv = DrawPointCircleText(ImgMats[LowBeamAnalysisResult4ImgName], Ph0V0, "0", "0", PerDegreePixel, specialName: "HV", isDraw: false);
                    OutputHvGray = pHv.Gray = GetCircularRoiMeanGray(toGetGrayMat, pHv);
                    DrawPointCircleText(ImgMats[LowBeamAnalysisResult4ImgName], Ph0V0, pHv.DegreeH, pHv.DegreeV, PerDegreePixel, true, pHv.Gray, specialName: "HV", isDraw: true);
                    _pointHv = new Point(pHv.Circle.Center.X, pHv.Circle.Center.Y);

                    var p1 = DrawPointCircleText(ImgMats[LowBeamAnalysisResult4ImgName], Ph0V0, "-8", "4", PerDegreePixel, specialName: "P1", isDraw: false);
                    OutputP1Gray = p1.Gray = GetCircularRoiMeanGray(toGetGrayMat, p1);
                    DrawPointCircleText(ImgMats[LowBeamAnalysisResult4ImgName], Ph0V0, p1.DegreeH, p1.DegreeV, PerDegreePixel, true, p1.Gray, specialName: "P1", isDraw: true);
                    _pointP1 = new Point(p1.Circle.Center.X, p1.Circle.Center.Y);

                    var p2 = DrawPointCircleText(ImgMats[LowBeamAnalysisResult4ImgName], Ph0V0, "0", "4", PerDegreePixel, specialName: "P2", isDraw: false);
                    OutputP2Gray = p2.Gray = GetCircularRoiMeanGray(toGetGrayMat, p2);
                    DrawPointCircleText(ImgMats[LowBeamAnalysisResult4ImgName], Ph0V0, p2.DegreeH, p2.DegreeV, PerDegreePixel, true, p2.Gray, specialName: "P2", isDraw: true);
                    _pointP2 = new Point(p2.Circle.Center.X, p2.Circle.Center.Y);

                    var p3 = DrawPointCircleText(ImgMats[LowBeamAnalysisResult4ImgName], Ph0V0, "8", "4", PerDegreePixel, specialName: "P3", isDraw: false);
                    OutputP3Gray = p3.Gray = GetCircularRoiMeanGray(toGetGrayMat, p3);
                    DrawPointCircleText(ImgMats[LowBeamAnalysisResult4ImgName], Ph0V0, p3.DegreeH, p3.DegreeV, PerDegreePixel, true, p3.Gray, specialName: "P3", isDraw: true);
                    _pointP3 = new Point(p3.Circle.Center.X, p3.Circle.Center.Y);

                    var p4 = DrawPointCircleText(ImgMats[LowBeamAnalysisResult4ImgName], Ph0V0, "-4", "2", PerDegreePixel, specialName: "P4", isDraw: false);
                    OutputP4Gray = p4.Gray = GetCircularRoiMeanGray(toGetGrayMat, p4);
                    DrawPointCircleText(ImgMats[LowBeamAnalysisResult4ImgName], Ph0V0, p4.DegreeH, p4.DegreeV, PerDegreePixel, true, p4.Gray, specialName: "P4", isDraw: true);
                    _pointP4 = new Point(p4.Circle.Center.X, p4.Circle.Center.Y);

                    var p5 = DrawPointCircleText(ImgMats[LowBeamAnalysisResult4ImgName], Ph0V0, "0", "2", PerDegreePixel, specialName: "P5", isDraw: false);
                    OutputP5Gray = p5.Gray = GetCircularRoiMeanGray(toGetGrayMat, p5);
                    DrawPointCircleText(ImgMats[LowBeamAnalysisResult4ImgName], Ph0V0, p5.DegreeH, p5.DegreeV, PerDegreePixel, true, p5.Gray, specialName: "P5", isDraw: true);
                    _pointP5 = new Point(p5.Circle.Center.X, p5.Circle.Center.Y);

                    var p6 = DrawPointCircleText(ImgMats[LowBeamAnalysisResult4ImgName], Ph0V0, "4", "2", PerDegreePixel, specialName: "P6", isDraw: false);
                    OutputP6Gray = p6.Gray = GetCircularRoiMeanGray(toGetGrayMat, p6);
                    DrawPointCircleText(ImgMats[LowBeamAnalysisResult4ImgName], Ph0V0, p6.DegreeH, p6.DegreeV, PerDegreePixel, true, p6.Gray, specialName: "P6", isDraw: true);
                    _pointP6 = new Point(p6.Circle.Center.X, p6.Circle.Center.Y);

                    // p7改为中等曝光

                    var p8 = DrawPointCircleText(ImgMats[LowBeamAnalysisResult4ImgName], Ph0V0, "-4", "0", PerDegreePixel, specialName: "P8", isDraw: false);
                    OutputP8Gray = p8.Gray = GetCircularRoiMeanGray(toGetGrayMat, p8);
                    DrawPointCircleText(ImgMats[LowBeamAnalysisResult4ImgName], Ph0V0, p8.DegreeH, p8.DegreeV, PerDegreePixel, true, p8.Gray, specialName: "P8", isDraw: true);
                    _pointP8 = new Point(p8.Circle.Center.X, p8.Circle.Center.Y);

                    toGetGrayMat.Dispose();
                }

                // 求梯度-高曝光
                {
                    var toCheckGradientImg = ImgMats[LowBeamSrc1Name].Clone(InterestedRoi);

                    ImgMats[LowBeamAnalysisResult5ImgName] = ImgMats[LowBeamSrc1Name].Clone(InterestedRoi);
                    DrawCoordinateSystem(ImgMats[LowBeamAnalysisResult5ImgName], Ph0V0, _pCutOffLineCenter);

                    var toCheckGrad2d5L = (double)Ph0V0.X - PerDegreePixel * 2.5;
                    var pGrad2d5Lp1 = new Point(toCheckGrad2d5L, (double)Ph0V0.Y - PerDegreePixel * 1.5);
                    var pGrad2d5Lp2 = new Point(toCheckGrad2d5L, (double)Ph0V0.Y + PerDegreePixel * 1.5);
                    DrawLowBeamGradientLine(ImgMats[LowBeamAnalysisResult5ImgName], pGrad2d5Lp1, pGrad2d5Lp2);
                    OutputGrad2d5L = GetGradient(toCheckGradientImg, pGrad2d5Lp1, pGrad2d5Lp2, ImgMats[LowBeamAnalysisResult5ImgName]);

                    var toCheckGrad1R = (double)Ph0V0.X + PerDegreePixel * 1;
                    var pGrad1Rp1 = new Point(toCheckGrad1R, (double)Ph0V0.Y - PerDegreePixel * 2);
                    var pGrad1Rp2 = new Point(toCheckGrad1R, (double)Ph0V0.Y + PerDegreePixel * 2);
                    DrawLowBeamGradientLine(ImgMats[LowBeamAnalysisResult5ImgName], pGrad1Rp1, pGrad1Rp2);
                    OutputGrad1R = GetGradient(toCheckGradientImg, pGrad1Rp1, pGrad1Rp2, ImgMats[LowBeamAnalysisResult5ImgName], showResultOffsetY: +5);

                    var toCheckGrad2R = (double)Ph0V0.X + PerDegreePixel * 2;
                    var pGrad2Rp1 = new Point(toCheckGrad2R, (double)Ph0V0.Y - PerDegreePixel * 2);
                    var pGrad2Rp2 = new Point(toCheckGrad2R, (double)Ph0V0.Y + PerDegreePixel * 2);
                    DrawLowBeamGradientLine(ImgMats[LowBeamAnalysisResult5ImgName], pGrad2Rp1, pGrad2Rp2);
                    OutputGrad2R = GetGradient(toCheckGradientImg, pGrad2Rp1, pGrad2Rp2, ImgMats[LowBeamAnalysisResult5ImgName], showResultOffsetY: -5);

                    var toCheckGrad3R = (double)Ph0V0.X + PerDegreePixel * 3;
                    var pGrad3Rp1 = new Point(toCheckGrad3R, (double)Ph0V0.Y - PerDegreePixel * 2);
                    var pGrad3Rp2 = new Point(toCheckGrad3R, (double)Ph0V0.Y + PerDegreePixel * 2);
                    DrawLowBeamGradientLine(ImgMats[LowBeamAnalysisResult5ImgName], pGrad3Rp1, pGrad3Rp2);
                    OutputGrad3R = GetGradient(toCheckGradientImg, pGrad3Rp1, pGrad3Rp2, ImgMats[LowBeamAnalysisResult5ImgName], showResultOffsetY: +5);

                    Console.WriteLine(@"求梯度-高曝光: grad2d5L={0}, grad1R={1}, grad2R={2}, grad3R={3}", OutputGrad2d5L, OutputGrad1R, OutputGrad2R, OutputGrad3R);
                    toCheckGradientImg.Dispose();
                }

                // 求梯度-低曝光
                {
                    //var toCheckGradientImg = ImgMats[LowBeamSrc0Name].Clone(InterestedRoi);

                    //ImgMats[LowBeamAnalysisResult5ImgName] = ImgMats[LowBeamSrc0Name].Clone(InterestedRoi);
                    //DrawCoordinateSystem(ImgMats[LowBeamAnalysisResult5ImgName], Ph0V0, _pCutOffLineCenter);

                    //var toCheckGrad2d5L = (double)Ph0V0.X - PerDegreePixel * 2.5;
                    //var pGrad2d5Lp1 = new Point(toCheckGrad2d5L, (double)Ph0V0.Y - PerDegreePixel * 1.5);
                    //var pGrad2d5Lp2 = new Point(toCheckGrad2d5L, (double)Ph0V0.Y + PerDegreePixel * 1.5);
                    //DrawLowBeamGradientLine(ImgMats[LowBeamAnalysisResult5ImgName], pGrad2d5Lp1, pGrad2d5Lp2);
                    //var grad2d5L = GetGradient(toCheckGradientImg, pGrad2d5Lp1, pGrad2d5Lp2, ImgMats[LowBeamAnalysisResult5ImgName]);

                    //var toCheckGrad1R = (double)Ph0V0.X + PerDegreePixel * 1;
                    //var pGrad1Rp1 = new Point(toCheckGrad1R, (double)Ph0V0.Y - PerDegreePixel * 2);
                    //var pGrad1Rp2 = new Point(toCheckGrad1R, (double)Ph0V0.Y + PerDegreePixel * 2);
                    //DrawLowBeamGradientLine(ImgMats[LowBeamAnalysisResult5ImgName], pGrad1Rp1, pGrad1Rp2);
                    //var grad1R = GetGradient(toCheckGradientImg, pGrad1Rp1, pGrad1Rp2, ImgMats[LowBeamAnalysisResult5ImgName], showResultOffsetY: +5);

                    //var toCheckGrad2R = (double)Ph0V0.X + PerDegreePixel * 2;
                    //var pGrad2Rp1 = new Point(toCheckGrad2R, (double)Ph0V0.Y - PerDegreePixel * 2);
                    //var pGrad2Rp2 = new Point(toCheckGrad2R, (double)Ph0V0.Y + PerDegreePixel * 2);
                    //DrawLowBeamGradientLine(ImgMats[LowBeamAnalysisResult5ImgName], pGrad2Rp1, pGrad2Rp2);
                    //var grad2R = GetGradient(toCheckGradientImg, pGrad2Rp1, pGrad2Rp2, ImgMats[LowBeamAnalysisResult5ImgName], showResultOffsetY: -5);

                    //var toCheckGrad3R = (double)Ph0V0.X + PerDegreePixel * 3;
                    //var pGrad3Rp1 = new Point(toCheckGrad3R, (double)Ph0V0.Y - PerDegreePixel * 2);
                    //var pGrad3Rp2 = new Point(toCheckGrad3R, (double)Ph0V0.Y + PerDegreePixel * 2);
                    //DrawLowBeamGradientLine(ImgMats[LowBeamAnalysisResult5ImgName], pGrad3Rp1, pGrad3Rp2);
                    //var grad3R = GetGradient(toCheckGradientImg, pGrad3Rp1, pGrad3Rp2, ImgMats[LowBeamAnalysisResult5ImgName], showResultOffsetY: +5);

                    //Console.WriteLine(@"求梯度-低曝光: grad2d5L={0}, grad1R={1}, grad2R={2}, grad3R={3}", grad2d5L, grad1R, grad2R, grad3R);
                    //toCheckGradientImg.Dispose();
                }
            }
        }

        #endregion

        public class OpticalPointWithGray
        {
            public Point OpticalPoint;
            public byte GrayMin;
            public byte GrayMax;

            public CircleWithGray Result;
        }

        public class CircleWithGray
        {
            public string Name;
            public byte Gray;
            public CircleSegment Circle;
            public string DegreeH;
            public string DegreeV;
        }

        public bool IsTransferLbOk;
        public bool IsTransferHbOk;

        public double OutPutTransferServoXk = 0.16;
        public double OutPutTransferServoXb = 15.89;

        public double OutPutTransferServoZk = 0.16;
        public double OutPutTransferServoZb = 166.39 - 65;

        public double OutPutTransferServoXMax = 400;

        public double OutPutTransferServoZMax = 270;
        public double OutPutTransferServoZMin = 75;

        public float OutputTransferLbEMaxX;
        public float OutputTransferLbEMaxZ;

        public float OutputTransferHbEMaxX;
        public float OutputTransferHbEMaxZ;

        public float OutputTransferH0V0X;
        public float OutputTransferH0V0Z;

        public float OutputTransferHn8V0X;
        public float OutputTransferHn8V0Z;

        public float OutputTransferHn8V4X;
        public float OutputTransferHn8V4Z;

        public float OutputTransferHn4V0X;
        public float OutputTransferHn4V0Z;

        public float OutputTransferHn4V2X;
        public float OutputTransferHn4V2Z;

        public float OutputTransferH0V2X;
        public float OutputTransferH0V2Z;

        public float OutputTransferH0V4X;
        public float OutputTransferH0V4Z;

        public float OutputTransferH4V2X;
        public float OutputTransferH4V2Z;

        public float OutputTransferH8V4X;
        public float OutputTransferH8V4Z;

        public float OutputTransferH0Vn11X;
        public float OutputTransferH0Vn11Z;

        public float OutputTransferHn20V0X;
        public float OutputTransferHn20V0Z;

        public float OutputTransferHn11V0X;
        public float OutputTransferHn11V0Z;

        public float OutputTransferH20V0X;
        public float OutputTransferH20V0Z;

        public float OutputTransferH11V0X;
        public float OutputTransferH11V0Z;

        public float OutputLbEMaxLuminance;
        public float OutputLbH0V0Luminance;
        public float OutputLbHn8V0Luminance;
        public float OutputLbHn8V4Luminance;
        public float OutputLbHn4V0Luminance;
        public float OutputLbHn4V2Luminance;
        public float OutputLbH0V2Luminance;
        public float OutputLbH0V4Luminance;
        public float OutputLbH4V2Luminance;
        public float OutputLbH8V4Luminance;
        public float OutputLbH0Vn11Luminance;

        public float OutputHbEMaxLuminance;
        public float OutputHbH0V0Luminance;
        public float OutputHbHn20V0Luminance; // H1
        public float OutputHbH20V0Luminance; // H4
        public float OutputHbHn11V0Luminance; // H2
        public float OutputHbH11V0Luminance; // H3

        public float OutputLbEMaxColor;
        public float OutputLbH0V0Color;
        public float OutputLbHn8V0Color;
        public float OutputLbHn8V4Color;
        public float OutputLbHn4V0Color;
        public float OutputLbHn4V2Color;
        public float OutputLbH0V2Color;
        public float OutputLbH0V4Color;
        public float OutputLbH4V2Color;
        public float OutputLbH8V4Color;
        public float OutputLbH0Vn11Color;

        public float OutputHbEMaxColor;
        public float OutputHbH0V0Color;
        public float OutputHbHn20V0Color; // H1
        public float OutputHbH20V0Color; // H4
        public float OutputHbHn11V0Color; // H2
        public float OutputHbH11V0Color; // H3

        public double HighBeamH0V0EmaxLuminanceRatio;

        public void CalculateHbH0V0DivideEmax()
        {
            HighBeamH0V0EmaxLuminanceRatio = Math.Round((OutputHbH0V0Luminance / OutputHbEMaxLuminance), 2,
                MidpointRounding.AwayFromZero);
        }

        private void ResetTransferServoPosition()
        {
            IsTransferLbOk = false;
            IsTransferHbOk = false;

            // 近光
            {
                OutputTransferLbEMaxX = -9999;
                OutputTransferLbEMaxZ = -9999;

                OutputTransferH0V0X = -9999;
                OutputTransferH0V0Z = -9999;

                OutputTransferHn8V0X = -9999;
                OutputTransferHn8V0Z = -9999;

                OutputTransferHn8V4X = -9999;
                OutputTransferHn8V4Z = -9999;

                OutputTransferHn4V0X = -9999;
                OutputTransferHn4V0Z = -9999;

                OutputTransferHn4V2X = -9999;
                OutputTransferHn4V2Z = -9999;

                OutputTransferH0V2X = -9999;
                OutputTransferH0V2Z = -9999;

                OutputTransferH0V4X = -9999;
                OutputTransferH0V4Z = -9999;

                OutputTransferH4V2X = -9999;
                OutputTransferH4V2Z = -9999;

                OutputTransferH8V4X = -9999;
                OutputTransferH8V4Z = -9999;

                OutputTransferH0Vn11X = -9999;
                OutputTransferH0Vn11Z = -9999;
            }

            // 法规测试坐标转换
            {
                OutputTransferB50LServoX = -9999; // -3.43, 0.57
                OutputTransfer75RServoX = -9999; // 1.15, 0.57
                OutputTransfer75LServoX = -9999; // -3.43, -0.57
                OutputTransfer50LServoX = -9999; // -3.43, -0.86
                OutputTransfer50RServoX = -9999; // 1.72, -0.86
                OutputTransfer50VServoX = -9999; // 0.00, -0.86
                OutputTransfer25LServoX = -9999; // -9.00, -1.72
                OutputTransfer25RServoX = -9999; // 9.00, -1.72
                OutputTransferHvServoX = -9999; // 0.00, 0.00
                OutputTransferP1ServoX = -9999; // -8.00, 4.00
                OutputTransferP2ServoX = -9999; // 0.00, 4.00
                OutputTransferP3ServoX = -9999; // 8.00, 4.00
                OutputTransferP4ServoX = -9999; // -4.00, 2.00
                OutputTransferP5ServoX = -9999; // 0.00, 2.00
                OutputTransferP6ServoX = -9999; // 4.00, 2.00
                OutputTransferP7ServoX = -9999; // -8.00, 0.00
                OutputTransferP8ServoX = -9999;// -4.00, 0.00

                OutputTransferB50LServoZ = -9999; // -3.43, 0.57
                OutputTransfer75RServoZ = -9999; // 1.15, 0.57
                OutputTransfer75LServoZ = -9999; // -3.43, -0.57
                OutputTransfer50LServoZ = -9999; // -3.43, -0.86
                OutputTransfer50RServoZ = -9999; // 1.72, -0.86
                OutputTransfer50VServoZ = -9999; // 0.00, -0.86
                OutputTransfer25LServoZ = -9999; // -9.00, -1.72
                OutputTransfer25RServoZ = -9999; // 9.00, -1.72
                OutputTransferHvServoZ = -9999; // 0.00, 0.00
                OutputTransferP1ServoZ = -9999; // -8.00, 4.00
                OutputTransferP2ServoZ = -9999; // 0.00, 4.00
                OutputTransferP3ServoZ = -9999; // 8.00, 4.00
                OutputTransferP4ServoZ = -9999; // -4.00, 2.00
                OutputTransferP5ServoZ = -9999; // 0.00, 2.00
                OutputTransferP6ServoZ = -9999; // 4.00, 2.00
                OutputTransferP7ServoZ = -9999; // -8.00, 0.00
                OutputTransferP8ServoZ = -9999;// -4.00, 0.00
            }

            // 远光
            {
                OutputTransferHbEMaxX = -9999;
                OutputTransferHbEMaxZ = -9999;

                OutputTransferHn20V0X = -9999;
                OutputTransferHn20V0Z = -9999;

                OutputTransferH20V0X = -9999;
                OutputTransferH20V0Z = -9999;

                OutputTransferHn11V0X = -9999;
                OutputTransferHn11V0Z = -9999;

                OutputTransferH11V0X = -9999;
                OutputTransferH11V0Z = -9999;

                HighBeamH0V0EmaxLuminanceRatio = -9999;
            }
        }

        /// <summary>
        /// 近光像素结果坐标转换为模组坐标
        /// </summary>
        private void TransferServoPosistionLb()
        {
            // LB EMax
            {
                OutputTransferLbEMaxX = CalculateServoPoint(_pLbEmax).X;
                OutputTransferLbEMaxZ = CalculateServoPoint(_pLbEmax).Y;

                if (OutputTransferLbEMaxX > OutPutTransferServoXMax ||
                    OutputTransferLbEMaxZ > OutPutTransferServoZMax ||
                    OutputTransferLbEMaxZ < OutPutTransferServoZMin)
                    return;
            }

            // H0,V0
            {
                OutputTransferH0V0X = CalculateServoPoint(Ph0V0).X;
                OutputTransferH0V0Z = CalculateServoPoint(Ph0V0).Y;

                if (OutputTransferH0V0X > OutPutTransferServoXMax ||
                    OutputTransferH0V0Z > OutPutTransferServoZMax ||
                    OutputTransferH0V0Z < OutPutTransferServoZMin)
                    return;
            }

            // H-8,V0
            {
                OutputTransferHn8V0X = CalculateServoPoint(PHn8V0).X;
                OutputTransferHn8V0Z = CalculateServoPoint(PHn8V0).Y;

                if (OutputTransferHn8V0X > OutPutTransferServoXMax ||
                    OutputTransferHn8V0Z > OutPutTransferServoZMax ||
                    OutputTransferHn8V0Z < OutPutTransferServoZMin)
                    return;
            }

            // H-8,V4
            {
                OutputTransferHn8V4X = CalculateServoPoint(PHn8V4).X;
                OutputTransferHn8V4Z = CalculateServoPoint(PHn8V4).Y;

                if (OutputTransferHn8V4X > OutPutTransferServoXMax ||
                    OutputTransferHn8V4Z > OutPutTransferServoZMax ||
                    OutputTransferHn8V4Z < OutPutTransferServoZMin)
                    return;
            }

            // H-4,V0
            {
                OutputTransferHn4V0X = CalculateServoPoint(PHn4V0).X;
                OutputTransferHn4V0Z = CalculateServoPoint(PHn4V0).Y;

                if (OutputTransferHn4V0X > OutPutTransferServoXMax ||
                    OutputTransferHn4V0Z > OutPutTransferServoZMax ||
                    OutputTransferHn4V0Z < OutPutTransferServoZMin)
                    return;
            }

            // H-4,V2
            {
                OutputTransferHn4V2X = CalculateServoPoint(PHn4V2).X;
                OutputTransferHn4V2Z = CalculateServoPoint(PHn4V2).Y;

                if (OutputTransferHn4V2X > OutPutTransferServoXMax ||
                    OutputTransferHn4V2Z > OutPutTransferServoZMax ||
                    OutputTransferHn4V2Z < OutPutTransferServoZMin)
                    return;
            }

            // H0,V2
            {
                OutputTransferH0V2X = CalculateServoPoint(Ph0V2).X;
                OutputTransferH0V2Z = CalculateServoPoint(Ph0V2).Y;

                if (OutputTransferH0V2X > OutPutTransferServoXMax ||
                    OutputTransferH0V2Z > OutPutTransferServoZMax ||
                    OutputTransferH0V2Z < OutPutTransferServoZMin)
                    return;
            }

            // H0,V4
            {
                OutputTransferH0V4X = CalculateServoPoint(Ph0V4).X;
                OutputTransferH0V4Z = CalculateServoPoint(Ph0V4).Y;

                if (OutputTransferH0V4X > OutPutTransferServoXMax ||
                    OutputTransferH0V4Z > OutPutTransferServoZMax ||
                    OutputTransferH0V4Z < OutPutTransferServoZMin)
                    return;
            }

            // H4,V2
            {
                OutputTransferH4V2X = CalculateServoPoint(Ph4V2).X;
                OutputTransferH4V2Z = CalculateServoPoint(Ph4V2).Y;

                if (OutputTransferH4V2X > OutPutTransferServoXMax ||
                    OutputTransferH4V2Z > OutPutTransferServoZMax ||
                    OutputTransferH4V2X < OutPutTransferServoZMin)
                    return;
            }

            // H8,V4
            {
                OutputTransferH8V4X = CalculateServoPoint(Ph8V4).X;
                OutputTransferH8V4Z = CalculateServoPoint(Ph8V4).Y;

                if (OutputTransferH8V4X > OutPutTransferServoXMax ||
                    OutputTransferH8V4Z > OutPutTransferServoZMax ||
                    OutputTransferH8V4Z < OutPutTransferServoZMin)
                    return;
            }

            // H0,V-11
            {
                //OutputTransferH0Vn11X = CalculateServoPoint(Ph0Vn11).X;
                //OutputTransferH0Vn11Z = CalculateServoPoint(Ph0Vn11).Y;

                //if (OutputTransferH0Vn11X > OutPutTransferServoXMax ||
                //    OutputTransferH0Vn11Z > OutPutTransferServoZMax ||
                //    OutputTransferH0Vn11Z < OutPutTransferServoZMin)
                //    return;
            }

            // 法规测试坐标转换
            {
                // B_50_L
                {
                    OutputTransferB50LServoX = CalculateServoPoint(_pointB50L).X;
                    OutputTransferB50LServoZ = CalculateServoPoint(_pointB50L).Y;

                    if (OutputTransferB50LServoX > OutPutTransferServoXMax ||
                        OutputTransferB50LServoZ > OutPutTransferServoZMax ||
                        OutputTransferB50LServoZ < OutPutTransferServoZMin)
                        return;
                }

                // 75R
                {
                    OutputTransfer75RServoX = CalculateServoPoint(_point75R).X;
                    OutputTransfer75RServoZ = CalculateServoPoint(_point75R).Y;

                    if (OutputTransfer75RServoX > OutPutTransferServoXMax ||
                        OutputTransfer75RServoZ > OutPutTransferServoZMax ||
                        OutputTransfer75RServoZ < OutPutTransferServoZMin)
                        return;
                }

                // 75L
                {
                    OutputTransfer75LServoX = CalculateServoPoint(_point75L).X;
                    OutputTransfer75LServoZ = CalculateServoPoint(_point75L).Y;

                    if (OutputTransfer75LServoX > OutPutTransferServoXMax ||
                        OutputTransfer75LServoZ > OutPutTransferServoZMax ||
                        OutputTransfer75LServoZ < OutPutTransferServoZMin)
                        return;
                }

                // 50L
                {
                    OutputTransfer50LServoX = CalculateServoPoint(_point50L).X;
                    OutputTransfer50LServoZ = CalculateServoPoint(_point50L).Y;

                    if (OutputTransfer50LServoX > OutPutTransferServoXMax ||
                        OutputTransfer50LServoZ > OutPutTransferServoZMax ||
                        OutputTransfer50LServoZ < OutPutTransferServoZMin)
                        return;
                }

                // 50R
                {
                    OutputTransfer50RServoX = CalculateServoPoint(_point50R).X;
                    OutputTransfer50RServoZ = CalculateServoPoint(_point50R).Y;

                    if (OutputTransfer50RServoX > OutPutTransferServoXMax ||
                        OutputTransfer50RServoZ > OutPutTransferServoZMax ||
                        OutputTransfer50RServoZ < OutPutTransferServoZMin)
                        return;
                }

                // 50V
                {
                    OutputTransfer50VServoX = CalculateServoPoint(_point50V).X;
                    OutputTransfer50VServoZ = CalculateServoPoint(_point50V).Y;

                    if (OutputTransfer50VServoX > OutPutTransferServoXMax ||
                        OutputTransfer50VServoZ > OutPutTransferServoZMax ||
                        OutputTransfer50VServoZ < OutPutTransferServoZMin)
                        return;
                }

                // 25L
                {
                    OutputTransfer25LServoX = CalculateServoPoint(_point25L).X;
                    OutputTransfer25LServoZ = CalculateServoPoint(_point25L).Y;

                    if (OutputTransfer25LServoX > OutPutTransferServoXMax ||
                        OutputTransfer25LServoZ > OutPutTransferServoZMax ||
                        OutputTransfer25LServoZ < OutPutTransferServoZMin)
                        return;
                }

                // 25R
                {
                    OutputTransfer25RServoX = CalculateServoPoint(_point25R).X;
                    OutputTransfer25RServoZ = CalculateServoPoint(_point25R).Y;

                    if (OutputTransfer25RServoX > OutPutTransferServoXMax ||
                        OutputTransfer25RServoZ > OutPutTransferServoZMax ||
                        OutputTransfer25RServoZ < OutPutTransferServoZMin)
                        return;
                }

                // HV
                {
                    OutputTransferHvServoX = CalculateServoPoint(_pointHv).X;
                    OutputTransferHvServoZ = CalculateServoPoint(_pointHv).Y;

                    if (OutputTransferHvServoX > OutPutTransferServoXMax ||
                        OutputTransferHvServoZ > OutPutTransferServoZMax ||
                        OutputTransferHvServoZ < OutPutTransferServoZMin)
                        return;
                }

                // P1
                {
                    OutputTransferP1ServoX = CalculateServoPoint(_pointP1).X;
                    OutputTransferP1ServoZ = CalculateServoPoint(_pointP1).Y;

                    if (OutputTransferP1ServoX > OutPutTransferServoXMax ||
                        OutputTransferP1ServoZ > OutPutTransferServoZMax ||
                        OutputTransferP1ServoZ < OutPutTransferServoZMin)
                        return;
                }

                // P2
                {
                    OutputTransferP2ServoX = CalculateServoPoint(_pointP2).X;
                    OutputTransferP2ServoZ = CalculateServoPoint(_pointP2).Y;

                    if (OutputTransferP2ServoX > OutPutTransferServoXMax ||
                        OutputTransferP2ServoZ > OutPutTransferServoZMax ||
                        OutputTransferP2ServoZ < OutPutTransferServoZMin)
                        return;
                }

                // P3
                {
                    OutputTransferP3ServoX = CalculateServoPoint(_pointP3).X;
                    OutputTransferP3ServoZ = CalculateServoPoint(_pointP3).Y;

                    if (OutputTransferP3ServoX > OutPutTransferServoXMax ||
                        OutputTransferP3ServoZ > OutPutTransferServoZMax ||
                        OutputTransferP3ServoZ < OutPutTransferServoZMin)
                        return;
                }

                // P4
                {
                    OutputTransferP4ServoX = CalculateServoPoint(_pointP4).X;
                    OutputTransferP4ServoZ = CalculateServoPoint(_pointP4).Y;

                    if (OutputTransferP4ServoX > OutPutTransferServoXMax ||
                        OutputTransferP4ServoZ > OutPutTransferServoZMax ||
                        OutputTransferP4ServoZ < OutPutTransferServoZMin)
                        return;
                }

                // P5
                {
                    OutputTransferP5ServoX = CalculateServoPoint(_pointP5).X;
                    OutputTransferP5ServoZ = CalculateServoPoint(_pointP5).Y;

                    if (OutputTransferP5ServoX > OutPutTransferServoXMax ||
                        OutputTransferP5ServoZ > OutPutTransferServoZMax ||
                        OutputTransferP5ServoZ < OutPutTransferServoZMin)
                        return;
                }

                // P6
                {
                    OutputTransferP6ServoX = CalculateServoPoint(_pointP6).X;
                    OutputTransferP6ServoZ = CalculateServoPoint(_pointP6).Y;

                    if (OutputTransferP6ServoX > OutPutTransferServoXMax ||
                        OutputTransferP6ServoZ > OutPutTransferServoZMax ||
                        OutputTransferP6ServoZ < OutPutTransferServoZMin)
                        return;
                }

                // P7
                {
                    OutputTransferP7ServoX = CalculateServoPoint(_pointP7).X;
                    OutputTransferP7ServoZ = CalculateServoPoint(_pointP7).Y;

                    if (OutputTransferP7ServoX > OutPutTransferServoXMax ||
                        OutputTransferP7ServoZ > OutPutTransferServoZMax ||
                        OutputTransferP7ServoZ < OutPutTransferServoZMin)
                        return;
                }

                // P8
                {
                    OutputTransferP8ServoX = CalculateServoPoint(_pointP8).X;
                    OutputTransferP8ServoZ = CalculateServoPoint(_pointP8).Y;

                    if (OutputTransferP8ServoX > OutPutTransferServoXMax ||
                        OutputTransferP8ServoZ > OutPutTransferServoZMax ||
                        OutputTransferP8ServoZ < OutPutTransferServoZMin)
                        return;
                }
            }

            IsTransferLbOk = true;
        }

        /// <summary>
        /// 远光光像素结果坐标转换为模组坐标
        /// </summary>
        private void TransferServoPosistionHb()
        {
            // HB EMax
            {
                OutputTransferHbEMaxX = CalculateServoPoint(_pHbEmax).X;
                OutputTransferHbEMaxZ = CalculateServoPoint(_pHbEmax).Y;

                if (OutputTransferHbEMaxX > OutPutTransferServoXMax ||
                    OutputTransferHbEMaxZ > OutPutTransferServoZMax ||
                    OutputTransferHbEMaxZ < OutPutTransferServoZMin)
                    return;
            }

            // H-20,V0
            {
                OutputTransferHn20V0X = CalculateServoPoint(PHn20V0).X;
                OutputTransferHn20V0Z = CalculateServoPoint(PHn20V0).Y;

                if (OutputTransferHn20V0X > OutPutTransferServoXMax ||
                    OutputTransferHn20V0Z > OutPutTransferServoZMax ||
                    OutputTransferHn20V0Z < OutPutTransferServoZMin)
                    return;
            }

            // H20,V0
            {
                OutputTransferH20V0X = CalculateServoPoint(Ph20V0).X;
                OutputTransferH20V0Z = CalculateServoPoint(Ph20V0).Y;

                if (OutputTransferH20V0X > OutPutTransferServoXMax ||
                    OutputTransferH20V0Z > OutPutTransferServoZMax ||
                    OutputTransferH20V0Z < OutPutTransferServoZMin)
                    return;
            }

            // H-11,V0
            {
                OutputTransferHn11V0X = CalculateServoPoint(PHn11V0).X;
                OutputTransferHn11V0Z = CalculateServoPoint(PHn11V0).Y;

                if (OutputTransferHn11V0X > OutPutTransferServoXMax ||
                    OutputTransferHn11V0Z > OutPutTransferServoZMax ||
                    OutputTransferHn11V0Z < OutPutTransferServoZMin)
                    return;
            }

            // H11,V0
            {
                OutputTransferH11V0X = CalculateServoPoint(Ph11V0).X;
                OutputTransferH11V0Z = CalculateServoPoint(Ph11V0).Y;

                if (OutputTransferH11V0X > OutPutTransferServoXMax ||
                    OutputTransferH11V0Z > OutPutTransferServoZMax ||
                    OutputTransferH11V0Z < OutPutTransferServoZMin)
                    return;
            }

            IsTransferHbOk = true;
        }

        private Point2f CalculateServoPoint(Point imgPoint)
        {
            const int offsetX = 0; //InterestedRoi.X;
            const int offsetY = 0; //InterestedRoi.Y;

            var x = (imgPoint.X + offsetX) * OutPutTransferServoXk + OutPutTransferServoXb;
            var z = (imgPoint.Y + offsetY) * OutPutTransferServoZk + OutPutTransferServoZb;

            //z = z - 67;

            //var x = ((imgPoint.X - 69F) / 51) * 10f;
            //var z = ((imgPoint.Y + 167f) / 37.5f) * 10f;

            return new Point2f((float)x, (float)z);
        }
    }
}
