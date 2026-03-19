using CommonUtility;
using CommonUtility.FileOperator;
using CommonUtility.HikSdk;
using NationalInstruments.Vision;
using NationalInstruments.Vision.Analysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Controller
{
    public sealed class LedVisionAnalysisByDaHengCamera :
        ControllerBase
    {
        public string LedCheckResult;

        public readonly CameraControl Camera = new CameraControl();
        //private readonly DahengCameraClass _dahengCamera = new DahengCameraClass();
        private LedCheckPara _ledCheckPara;
        private string _selectedCameraSn;
        private bool _isSnapShotting;
        private int _captureCount = 0;
        //private readonly List<byte[]> _imgbytesListToAnalysis = new List<byte[]>();
        private readonly string _thisDebugPath = Directory.GetCurrentDirectory();
        private string _thisProgram = string.Empty;
        private string _thisProgramFolder = string.Empty;
        private string _thisConfigXmlPath = string.Empty;
        private bool _isVisionAnalysisExecuted;

        public LedVisionAnalysisByDaHengCamera
            (string name)
            : base(name)
        {
            Camera.DeviceListAcq();
        }

        ~LedVisionAnalysisByDaHengCamera()
        {
            Camera.CloseAllCamera();
            Dispose();
        }

        private void OpenCamera()
        {
            var device = Camera.CameraList.Find(f => f.GigeInfo.chSerialNumber == _selectedCameraSn);
            if (device != null)
            {
                device.OpenCamera();
            }

            if (Camera != null)
                Camera.OpenAllCamera();

            //_dahengCamera.OpenDevices();
        }

        private void CloseCamera()
        {
            var device = Camera.CameraList.Find(f => f.GigeInfo.chSerialNumber == _selectedCameraSn);
            if (device != null)
            {
                device.CloseCamera();
            }

            //if (_camera != null)
            //    _camera.CloseAllCamera();

            //if (_dahengCamera != null)
            //    _dahengCamera.CloseDevices();
        }

        public void InitConfigFile(string programeName)
        {
            _thisProgram = programeName;

            try
            {
                _thisProgramFolder = string.Format(@"{0}\图像检测配置文件\{1}", _thisDebugPath, programeName);
                if (!Directory.Exists(_thisProgramFolder))
                    Directory.CreateDirectory(_thisProgramFolder);

                _thisConfigXmlPath = string.Format(@"{0}\{1}.xml", _thisProgramFolder, programeName);
                if (!File.Exists(_thisConfigXmlPath))
                    CreateNewParaFile(programeName, _thisConfigXmlPath);

                // 序列化为LedCheckPara
                _ledCheckPara = XmlHelper.Deserialize<LedCheckPara>(_thisConfigXmlPath);
            }
            catch (Exception)
            {
                _thisProgramFolder = string.Format(@"{0}\图像检测配置文件\{1}", _thisDebugPath, programeName);
                if (!Directory.Exists(_thisProgramFolder))
                    Directory.CreateDirectory(_thisProgramFolder);

                _thisConfigXmlPath = string.Format(@"{0}\{1}.xml", _thisProgramFolder, programeName);
                if (!File.Exists(_thisConfigXmlPath))
                    CreateNewParaFile(programeName, _thisConfigXmlPath);

                // 序列化为LedCheckPara
                _ledCheckPara = XmlHelper.Deserialize<LedCheckPara>(_thisConfigXmlPath);
            }
        }

        public async void ExecuteVisionAnalysis(string funcName)
        {
            if (_isVisionAnalysisExecuted)
                return;

            _isVisionAnalysisExecuted = true;

            #region 拍照并解析
            await Task.Run(() =>
            {
                InitConfigFile(_thisProgram);

                OpenCamera();
                LedCheckResult = string.Empty;

                if (_ledCheckPara.VisionFuncs.VisionFunc == null)
                    AddNewFunc(_ledCheckPara, funcName);

                if (_ledCheckPara.VisionFuncs.VisionFunc == null)
                    return;

                var getFunc =
                    _ledCheckPara.VisionFuncs.VisionFunc.ToList().Find(f => f.FuncName.Equals(funcName));
                if (getFunc == null)
                    AddNewFunc(_ledCheckPara, funcName);

                getFunc =
                    _ledCheckPara.VisionFuncs.VisionFunc.ToList().Find(f => f.FuncName.Equals(funcName));

                if (string.IsNullOrEmpty(getFunc.CameraPara.CameraSn))
                    return;

                SelectCamera(getFunc.CameraPara.CameraSn);
                SetShutter(getFunc.CameraPara.Shutter.ToString());

                if (getFunc.CameraPara.FrameRate == 0)
                    CloseFrameRateMode();
                else
                    OpenFrameRate(getFunc.CameraPara.FrameRate.ToString());

                Thread.Sleep(getFunc.CameraPara.DelayTime);

                SnapShot(getFunc.CameraPara.FrameCount.ToString());

                #region 等待相机拍照完成后处理图像
                while (true)
                    if (!_isSnapShotting)
                        break;

                CloseCamera();

                var imgListToTest = new List<VisionImage>();

                var captureImageFolder = string.Format(@"{0}\{1}-CaptureImage", _thisProgramFolder, funcName);
                if (!Directory.Exists(captureImageFolder))
                    Directory.CreateDirectory(captureImageFolder);

                var basicImageFolder = string.Format(@"{0}\基准图片", captureImageFolder);
                if (!Directory.Exists(basicImageFolder))
                    Directory.CreateDirectory(basicImageFolder);
                var templateImgPath =
                    string.Format(@"{0}\{1}", _thisProgramFolder,
                        string.Format(@"{0}-CaptureImage\基准图片\{1}-基准模板.png", funcName, funcName));
                if (!File.Exists(templateImgPath))
                    ImageProcessing.CreateNiTemplateImg(templateImgPath);

                var caliImageFolder = string.Format(@"{0}\标定图片", captureImageFolder);
                if (!Directory.Exists(caliImageFolder))
                    Directory.CreateDirectory(caliImageFolder);
                //var isNeedSaved = Directory.GetFiles(caliImageFolder).Length == 0;

                var device = Camera.CameraList.Find(f => f.GigeInfo.chSerialNumber == _selectedCameraSn);
                if (device == null)
                    return;

                for (var i = 0; i < _captureCount; i++)
                {
                    int row, col;
                    imgListToTest.Add(MyCamera.MatToVisionImage(device.GetImageFromBuff(i, out row, out col)));
                }

                //for (var i = 0; i < _imgbytesListToAnalysis.Count; i++)
                //{
                //    var t = _imgbytesListToAnalysis[i];

                //    var cameraDevice = _dahengCamera.GetDevice(_selectedCameraSn);
                //    imgListToTest.Add(ImageProcessing.ConvertBytesToVisionImg(t, cameraDevice.MnWidth, cameraDevice.MnHeigh,
                //        cameraDevice.MbIsColor));
                //}
                //_imgbytesListToAnalysis.Clear();

                if (!imgListToTest.Any())
                    return;

                // 先找出偏移
                var newRotation = (double)360;
                //if (imgListToTest.Count == 1)
                //{
                //    foreach (var t1 in imgListToTest)
                //    {
                //        using (var imgToTest = new VisionImage(t1.Type))
                //        {
                //            Algorithms.Copy(t1, imgToTest);

                //            var cImg = ImageProcessing.ColorPlaneExtraction(imgToTest,
                //                EnumOperater.GetEnumByValue<ImageProcessing.ColorPlaneExtractionType>(
                //                    getFunc.VisionPara.ColorPlaneExtraction));
                //            Algorithms.Copy(cImg, imgToTest);
                //            cImg.Dispose();

                //            var lImg = ImageProcessing.LookupTable(imgToTest,
                //                EnumOperater.GetEnumByValue<ImageProcessing.LookupTableType>(getFunc.VisionPara.LookupTable));
                //            Algorithms.Copy(lImg, imgToTest);
                //            lImg.Dispose();

                //            var centerPoint =
                //                ImageProcessing.GetPointByString(getFunc.VisionPara.TemplatePara.TemplateCanterPoint);
                //            var tempRect = new RectangleContour(0, 0, imgToTest.Width, imgToTest.Height);
                //            //ImageProcessing.GetRectByString(getFunc.VisionPara.TemplatePara.TemplateRoiRectangle);

                //            if (centerPoint == new Point(-9999, -9999))
                //                continue;

                //            ImageProcessing.GeometricMatchingResultStruct result;
                //            if (!ImageProcessing.GetGeometricMatchingResult(
                //                imgToTest, tempRect, templateImgPath, out result))
                //                continue;

                //            ImageProcessing.OnPushVisionImgMsg(imgToTest);

                //            if (!(result.Rotation <= 10.0) || !(360 - result.Rotation >= 350))
                //                continue;
                //            newRotation = result.Rotation;

                //            ImageProcessing.ImageRotationImage(imgToTest, 360 - newRotation, false);
                //            if (ImageProcessing.GetGeometricMatchingResult(imgToTest, tempRect, templateImgPath,
                //                out result))
                //            {
                //                var actualCenterX = result.Position.X;
                //                var actualCenterY = result.Position.Y;

                //                var offsetX = (double)(actualCenterX - centerPoint.X);
                //                var offsetY = (double)(actualCenterY - centerPoint.Y);

                //                if (offsetX >= -50 && offsetX <= 50 && offsetY >= -50 && offsetY <= 50)
                //                {
                //                    foreach (var gp in getFunc.VisionPara.ShapesGroups)
                //                    {
                //                        foreach (var t in gp.Shapes)
                //                        {
                //                            var sp = t;
                //                            var newRect = string.Empty;

                //                            if (sp.Contour.Type.Equals(typeof(PolygonContour).Name))
                //                            {
                //                                var pc = ImageProcessing.GetPolygonContourByString(sp.Contour.Rect);
                //                                var tempPs =
                //                                    pc.Points.Select(
                //                                        point => new PointContour(point.X + offsetX, point.Y + offsetY))
                //                                        .ToList();
                //                                var newRoi = new PolygonContour(tempPs);
                //                                newRect = ImageProcessing.GetStringPolygonContour(newRoi);
                //                            }
                //                            else if (sp.Contour.Type.Equals(typeof(RectangleContour).Name))
                //                            {
                //                                var pc = ImageProcessing.GetRectByString(sp.Contour.Rect);
                //                                var newRoi = new RectangleContour(pc.Left + offsetX, pc.Top + offsetY, pc.Width, pc.Height);
                //                                newRect = ImageProcessing.GetStringRect(newRoi);
                //                            }

                //                            t.Contour.Rect = newRect;
                //                        }
                //                    }
                //                }
                //            }
                //            break;
                //        }
                //    }
                //}

                if (getFunc.VisionPara.ShapesGroups == null ||
                    getFunc.VisionPara.ShapesGroups.Length == 0 ||
                    (getFunc.VisionPara.ShapesGroups.Length == 1 &&
                     (getFunc.VisionPara.ShapesGroups[0].Shapes == null ||
                      !getFunc.VisionPara.ShapesGroups[0].Shapes.Any())))
                {
                    foreach (var t in Directory.GetFiles(caliImageFolder))
                        File.Delete(t);

                    for (var i = 0; i < imgListToTest.Count; i++)
                    {
                        imgListToTest[i].WriteBmpFile(string.Format(@"{0}\Image{1}.bmp", caliImageFolder, i));
                        ImageProcessing.OnPushVisionImgMsg(imgListToTest[i]);
                    }
                    ClearVisionIamge(imgListToTest);
                    return;
                }

                var getIsImgMatch = new Func<int, int, bool>((imgIndex, groupIndex) =>
                {
                    LedCheckResult = string.Empty;
                    var isAllRoiGrayMatch = true;

                    using (var imgToTest = new VisionImage())
                    {
                        Algorithms.Copy(imgListToTest[imgIndex], imgToTest);

                        var cImg = ImageProcessing.ColorPlaneExtraction(imgToTest,
                            EnumOperater.GetEnumByValue<ImageProcessing.ColorPlaneExtractionType>(
                                getFunc.VisionPara.ColorPlaneExtraction));
                        Algorithms.Copy(cImg, imgToTest);
                        cImg.Dispose();

                        var lImg = ImageProcessing.LookupTable(imgToTest,
                            EnumOperater.GetEnumByValue<ImageProcessing.LookupTableType>(getFunc.VisionPara.LookupTable));
                        Algorithms.Copy(lImg, imgToTest);
                        lImg.Dispose();

                        if (360 - newRotation > 0)
                            ImageProcessing.ImageRotationImage(imgToTest, 360 - newRotation, false);

                        var shapeList = new List<object>();
                        foreach (var p in getFunc.VisionPara.ShapesGroups[groupIndex].Shapes)
                        {
                            if (p.Contour.Type == typeof(PolygonContour).Name)
                                shapeList.Add(ImageProcessing.GetPolygonContourByString(p.Contour.Rect));
                            else if (p.Contour.Type == typeof(RectangleContour).Name)
                                shapeList.Add(ImageProcessing.GetRectByString(p.Contour.Rect));
                        }

                        for (var i = 0; i < shapeList.Count; i++)
                        {
                            var minGray = getFunc.VisionPara.ShapesGroups[groupIndex].Shapes[i].Min;
                            var maxGray = getFunc.VisionPara.ShapesGroups[groupIndex].Shapes[i].Max;
                            var actualGray = -9999.0;

                            var shape = shapeList[i];
                            var shapeType = shape.GetType();
                            if (shapeType == typeof(PolygonContour)) // 多边形         
                            {
                                var actualRoi = (PolygonContour)shape;
                                var actualRect = ImageProcessing.GetStringPolygonContour(actualRoi);

                                actualGray = ImageProcessing.GetGrayValue(imgToTest, actualRoi);
                                if (double.IsNaN(actualGray))
                                    actualGray = 0;

                                LedCheckResult += string.Format(@"<Vision>{0},{1},{2},{3},{4},{5}</Vision>",
                                    groupIndex,
                                    getFunc.VisionPara.ShapesGroups[groupIndex].Shapes[i].Contour.Type,
                                    actualRect,
                                    getFunc.VisionPara.ShapesGroups[groupIndex].Shapes[i].Min,
                                    getFunc.VisionPara.ShapesGroups[groupIndex].Shapes[i].Max, actualGray);
                            }
                            else if (shapeType == typeof(RectangleContour)) // 矩形
                            {
                                var actualRoi = (RectangleContour)shape;
                                var actualRoiRect = ImageProcessing.GetStringRect(actualRoi);

                                actualGray = ImageProcessing.GetGrayValue(imgToTest, actualRoi);
                                if (double.IsNaN(actualGray))
                                    actualGray = 0;

                                LedCheckResult += string.Format(@"<Vision>{0},{1},{2},{3},{4},{5}</Vision>",
                                    groupIndex,
                                    getFunc.VisionPara.ShapesGroups[groupIndex].Shapes[i].Contour.Type,
                                    actualRoiRect,
                                    getFunc.VisionPara.ShapesGroups[groupIndex].Shapes[i].Min,
                                    getFunc.VisionPara.ShapesGroups[groupIndex].Shapes[i].Max, actualGray);
                            }

                            if (actualGray < minGray - 5 || actualGray > maxGray + 5)
                                isAllRoiGrayMatch = false;
                        }

                        ImageProcessing.OnPushVisionImgMsg(imgToTest);
                    }

                    return isAllRoiGrayMatch;
                });

                var currentImgIndex = 0;
                var currentGpIndex = 0;

                var maxImgIndex = imgListToTest.Count - 1;
                var maxGpIndex = getFunc.VisionPara.ShapesGroups.Length - 1;

                while (true)
                {
                    if (getIsImgMatch(currentImgIndex, currentGpIndex)) // 当前图像符合当前动画效果
                    {
                        if (currentGpIndex == maxGpIndex) // 当前图像匹配当前动画效果且为最后一组动画
                        {
                            ClearVisionIamge(imgListToTest);
                            break; // OK
                        }

                        if (currentImgIndex < maxImgIndex)
                            currentImgIndex++;
                        else // 最后一张图片匹配当前动画效果但不是最后一组动画效果
                        {
                            currentGpIndex++;
                            getIsImgMatch(currentImgIndex, currentGpIndex);
                            foreach (var t in Directory.GetFiles(caliImageFolder))
                                File.Delete(t);
                            for (var i = 0; i <= currentImgIndex; i++)
                                imgListToTest[currentImgIndex].WriteBmpFile(string.Format(@"{0}\Image{1}.bmp",
                                    caliImageFolder, i));
                            ClearVisionIamge(imgListToTest);
                            break; // NG
                        }
                    }
                    else
                    {
                        if (currentImgIndex == 0)
                        {
                            foreach (var t in Directory.GetFiles(caliImageFolder))
                                File.Delete(t);
                            for (var i = 0; i <= currentImgIndex; i++)
                                imgListToTest[currentImgIndex].WriteBmpFile(string.Format(@"{0}\Image{1}.bmp",
                                    caliImageFolder, i));
                            ClearVisionIamge(imgListToTest);
                            break; // NG
                        }

                        if (currentGpIndex < maxGpIndex)
                        {
                            currentGpIndex++;
                            if (!getIsImgMatch(currentImgIndex, currentGpIndex))
                            {
                                getIsImgMatch(currentImgIndex, --currentGpIndex);
                                foreach (var t in Directory.GetFiles(caliImageFolder))
                                    File.Delete(t);
                                for (var i = 0; i <= currentImgIndex; i++)
                                    imgListToTest[currentImgIndex].WriteBmpFile(string.Format(@"{0}\Image{1}.bmp",
                                        caliImageFolder, i));
                                ClearVisionIamge(imgListToTest);
                                break; // NG
                            }
                            if (currentGpIndex == maxGpIndex) // 当前图像匹配当前动画效果且为最后一组动画
                            {
                                ClearVisionIamge(imgListToTest);
                                break; // OK
                            }

                            if (currentImgIndex < maxImgIndex)
                                currentImgIndex++;
                            else // 最后一张图片匹配当前动画效果但不是最后一组动画效果
                            {
                                currentGpIndex++;
                                getIsImgMatch(currentImgIndex, currentGpIndex);
                                foreach (var t in Directory.GetFiles(caliImageFolder))
                                    File.Delete(t);
                                for (var i = 0; i <= currentImgIndex; i++)
                                    imgListToTest[currentImgIndex].WriteBmpFile(string.Format(@"{0}\Image{1}.bmp",
                                        caliImageFolder, i));
                                ClearVisionIamge(imgListToTest);
                                break; // NG
                            }
                        }
                        else
                        {
                            foreach (var t in Directory.GetFiles(caliImageFolder))
                                File.Delete(t);
                            for (var i = 0; i <= currentImgIndex; i++)
                                imgListToTest[currentImgIndex].WriteBmpFile(string.Format(@"{0}\Image{1}.bmp",
                                    caliImageFolder, i));
                            ClearVisionIamge(imgListToTest);
                            break; // NG
                        }
                    }
                }
                #endregion
            });
            #endregion

            _isVisionAnalysisExecuted = false;
        }

        public void WaitVisionAnalysisEnd()
        {
            while (true)
                if (!_isVisionAnalysisExecuted)
                    break;
        }

        private static void ClearVisionIamge(ICollection<VisionImage> imgList)
        {
            foreach (var t in imgList)
                t.Dispose();
            imgList.Clear();
        }

        private void SelectCamera(string cameraSn)
        {
            _selectedCameraSn = cameraSn;
        }

        private async void SnapShot(string frameCount)
        {
            _captureCount = 0;

            if (_isSnapShotting ||
                Camera == null ||
                string.IsNullOrEmpty(_selectedCameraSn))
                return;

            var device = Camera.CameraList.Find(f => f.GigeInfo.chSerialNumber == _selectedCameraSn);
            if (device == null)
                return;

            device.ClearBuffer();
            _isSnapShotting = true;

            await Task.Run(() =>
            {
                _captureCount = int.Parse(frameCount);
                device.Capture(uint.Parse(_captureCount.ToString()));

                //List<byte[]> getImgBytesList;
                //if (!_dahengCamera.TriggerSoftware(
                //    _selectedCameraSn, int.Parse(frameCount), out getImgBytesList, int.Parse(frameCount) > 1 ? 10000 : 3000))
                //    return;
                //foreach (var t in getImgBytesList)
                //    _imgbytesListToAnalysis.Add(t);
                //getImgBytesList.Clear();
            });

            _isSnapShotting = false;
        }

        private void SetShutter(string shutter)
        {
            var device = Camera.CameraList.Find(f => f.GigeInfo.chSerialNumber == _selectedCameraSn);

            if (device != null && !string.IsNullOrEmpty(_selectedCameraSn))
            {
                device.SetExposureTime(int.Parse(shutter));
                // DahengCameraClass.SetShutter(_selectedCameraSn, int.Parse(shutter));
            }
        }

        private void OpenFrameRate(string frameRate)
        {
            var device = Camera.CameraList.Find(f => f.GigeInfo.chSerialNumber == _selectedCameraSn);

            if (device != null && !string.IsNullOrEmpty(_selectedCameraSn))
            {
                //DahengCameraClass.OpenFrameRateMode(_selectedCameraSn, double.Parse(frameRate));
            }
        }

        private void CloseFrameRateMode()
        {
            var device = Camera.CameraList.Find(f => f.GigeInfo.chSerialNumber == _selectedCameraSn);

            if (device != null && !string.IsNullOrEmpty(_selectedCameraSn))
            {
                //DahengCameraClass.CloseFrameRateMode(_selectedCameraSn);
            }
        }

        private static void CreateNewParaFile(
            string programeName, string filePath)
        {
            var ledCheckPara = new LedCheckPara
            {
                VisionFuncs = new LedCheckParaVisionFuncs
                {
                    ProductName = programeName,
                    VisionFunc = new LedCheckParaVisionFuncsVisionFunc[1]
                }
            };

            XmlHelper.SerializeToFile(ledCheckPara, filePath, Encoding.UTF8);
        }

        private void AddNewFunc(LedCheckPara paraToAdd, string funcName)
        {
            var listFuncs = new List<LedCheckParaVisionFuncsVisionFunc>();
            if (paraToAdd.VisionFuncs.VisionFunc != null)
                listFuncs.AddRange(paraToAdd.VisionFuncs.VisionFunc);

            var captureImageFolder = string.Format(@"{0}\{1}-CaptureImage", _thisProgramFolder, funcName);
            if (!Directory.Exists(captureImageFolder))
                Directory.CreateDirectory(captureImageFolder);
            var basicImageFolder = string.Format(@"{0}\基准图片", captureImageFolder);
            if (!Directory.Exists(basicImageFolder))
                Directory.CreateDirectory(basicImageFolder);
            var caliImageFolder = string.Format(@"{0}\标定图片", captureImageFolder);
            if (!Directory.Exists(caliImageFolder))
                Directory.CreateDirectory(caliImageFolder);

            var newFunc = new LedCheckParaVisionFuncsVisionFunc
            {
                FuncName = funcName,
                FuncIndex = (ushort)listFuncs.Count,
                FuncImgPath = string.Format(@"{0}-CaptureImage\标定图片", funcName),
                FuncType = "Vision",
                VisionPara = new LedCheckParaVisionFuncsVisionFuncVisionPara
                {
                    TemplatePara = new LedCheckParaVisionFuncsVisionFuncVisionParaTemplatePara
                    {
                        //TemplateImgPath = string.Format(@"{0}-CaptureImage\基准图片\{1}-基准模板.png", funcName, funcName),
                        TemplateCanterPoint = string.Format(@"{0},{1}", -9999, -9999),
                        TemplateRoiRectangle =
                            string.Format(@"{0},{1},{2},{3}", -9999, -9999, -9999, -9999)
                    },
                    ShapesGroups = new LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroup[0]
                },
                CameraPara = new LedCheckParaVisionFuncsVisionFuncCameraPara
                {
                    CameraSn =
                        DahengCameraClass.MListCCamerInfo.Count > 0
                            ? DahengCameraClass.MListCCamerInfo[0].MStrSn
                            : string.Empty,
                    DelayTime = 500,
                    FrameCount = 1,
                    FrameRate = 0,
                    Shutter = 10000
                }
            };

            newFunc.VisionPara.ColorPlaneExtraction =
                ImageProcessing.ColorPlaneExtractionType.Green.ToString();
            newFunc.VisionPara.LookupTable =
                ImageProcessing.LookupTableType.ImageSource.ToString();

            for (var i = 0; i < newFunc.VisionPara.ShapesGroups.Length; i++)
            {
                newFunc.VisionPara.ShapesGroups[i] =
                new LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroup
                {
                    GroupIndex = i,
                    Shapes = new LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroupShape[0]
                };
            }

            var tempImgFile = string.Format(@"{0}\{1}", _thisProgramFolder,
                string.Format(@"{0}-CaptureImage\基准图片\{1}-基准模板.png", funcName, funcName));
            ImageProcessing.CreateNiTemplateImg(tempImgFile);

            listFuncs.Add(newFunc);
            paraToAdd.VisionFuncs.VisionFunc = listFuncs.ToArray();

            XmlHelper.SerializeToFile(paraToAdd, _thisConfigXmlPath, Encoding.UTF8);
        }
    }
}
