using CommonUtility;
using CommonUtility.HikSdk;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace Controller
{
    public sealed class VisionAnalysis : ControllerBase
    {
        [Description("R/W,最大拍摄数量")]
        public int _maxStaticCaptureCount = 3;

        [Description("R/W,相机SN")]
        public string CameraSn { get; set; } = string.Empty;

        [Description("R/W,是否使用灰度图")]
        public bool IsUseGray;

        public VisionAnalysis(string name) : base(name)
        {
            if (_cameraControl == null)
            {
                _cameraControl = new CameraControl();
                _cameraControl.DeviceListAcq();
            }

            //// debug
            //{
            //    var tp = string.Empty;
            //    var liststr = new string[3];
            //    if (name.Contains("相机3"))
            //    {
            //        liststr[0] = "相机3全部点亮缓存";
            //        liststr[1] = "相机3单数点亮缓存";
            //        liststr[2] = "相机3双数点亮缓存";
            //        CameraSn = "DA7521507";
            //    }
            //    else if (name.Contains("相机4"))
            //    {
            //        liststr[0] = "相机4全部点亮缓存";
            //        liststr[1] = "相机4单数点亮缓存";
            //        liststr[2] = "相机4双数点亮缓存";
            //        CameraSn = "DA7521508";
            //    }
            //    else 
            //    {
            //        return;
            //    }

            //    //var liststr = new string[] { "相机3全部点亮缓存", "相机3单数点亮缓存", "相机3双数点亮缓存" };
            //    foreach (var buffName in liststr)
            //    {
            //        if (buffName.Contains("全部点亮缓存") || buffName.Contains("单数点亮缓存") || buffName.Contains("双数点亮缓存"))
            //        {
            //            if (StaticImageBuff.ContainsKey(buffName))
            //                StaticImageBuff[buffName] = new string[_maxStaticCaptureCount];
            //            else
            //                StaticImageBuff.Add(buffName, new string[_maxStaticCaptureCount]);

            //            var mat = new Mat();
            //            if (buffName.Contains("全部点亮缓存"))
            //                mat = Cv2.ImRead(@"E:\360MoveData\Users\B1438\Pictures\Camera Roll\test20250910\0.bmp");
            //            else if (buffName.Contains("单数点亮缓存"))
            //                mat = Cv2.ImRead(@"E:\360MoveData\Users\B1438\Pictures\Camera Roll\test20250910\BBB-1.jpg");
            //            else if (buffName.Contains("双数点亮缓存"))
            //                mat = Cv2.ImRead(@"E:\360MoveData\Users\B1438\Pictures\Camera Roll\test20250910\Image0.bmp");
            //            else
            //            {
            //                mat?.Dispose();
            //                continue;
            //            }

            //            for (int i = 0; i < _maxStaticCaptureCount; i++)
            //            {
            //                using (var bitmap = BitmapConverter.ToBitmap(mat))
            //                {
            //                    StaticImageBuff[buffName][i] = MyCamera.BitmapToBase64String(bitmap);
            //                    bitmap.Dispose();
            //                }
            //            }

            //            mat?.Dispose();
            //            continue;
            //        }
            //    }
            //}
        }

        ~VisionAnalysis() => CloseCamera();

        private static CameraControl _cameraControl;
        private bool _isGrabing;
        private MyCamera _selectedCamera;
        public Dictionary<string, string[]> DynamicImageBuff = new Dictionary<string, string[]>();
        public Dictionary<string, Tuple<long, string>[]> StaticImageBuff = new Dictionary<string, Tuple<long, string>[]>();
        //public Dictionary<string, string[]> StaticImageBuff = new Dictionary<string, string[]>();
        private static readonly List<string> _usedSn = new List<string>();

        [Description("通过相机sn连接相机")]
        public void ConnectCamera(string sn)
        {
            if (_cameraControl == null || _usedSn.Contains(sn))
                return;
            var index = _cameraControl.CameraList.FindIndex(f => string.Equals(f.GigeInfo.chSerialNumber, sn, StringComparison.CurrentCultureIgnoreCase));

            if (index >= 0)
            {
                CameraSn = sn;
                _selectedCamera = _cameraControl.CameraList[index];
                _selectedCamera.IsUseCustomSize = false;
            }
            else
            {
                if (sn != "DA0174564" && sn != "GZ0200090004")
                {
                    return;
                }
                else
                {
                    CameraSn = sn;
                    _selectedCamera = new MyCamera(IntPtr.Zero, new MvCameraSdk.MV_GIGE_DEVICE_INFO());
                    _selectedCamera.IsUseCustomSize = false;
                }
            }
        }

        public void OpenCamera()
        {
            _selectedCamera.IsGray = true;
            _selectedCamera.OpenCamera();
        }

        [Description("关闭相机")]
        public void CloseCamera()
        {
            _selectedCamera?.CloseCamera();
            //_selectedCamera = null;
            //CameraSn = string.Empty;
        }

        [Description("设置增益")]
        public void SetGain(int gain)
        {
            _selectedCamera?.SetGain((short)gain);
        }

        [Description("设置曝光")]
        public void SetExposure(int exposure)
        {
            _selectedCamera?.SetExposureTime((ushort)exposure);
        }

        [Description("抓拍一张")]
        public void Capture(string buffName)
        {
            //if ((CameraSn == "DA0174564" || CameraSn == "GZ0200090004"))
            //{
            //    if (buffName.Contains("TAIL") || buffName.Contains("TURN") || buffName.Contains("测试图"))
            //    {
            //        if (StaticImageBuff.ContainsKey(buffName))
            //            StaticImageBuff[buffName] = new string[_maxStaticCaptureCount];
            //        else
            //            StaticImageBuff.Add(buffName, new string[_maxStaticCaptureCount]);

            //        var mat = new Mat();
            //        if (buffName.Contains("TAIL"))
            //            mat = Cv2.ImRead(@"E:\\CCD测试样张\TAIL.bmp");
            //        else if (buffName.Contains("TURN"))
            //            mat = Cv2.ImRead(@"E:\\CCD测试样张\TURN.bmp");
            //        else if (buffName.Contains("测试图"))
            //            mat = Cv2.ImRead(@"E:\CCD测试样张\动画3-转向流水\Image10.bmp");
            //        else
            //        {
            //            mat?.Dispose();
            //            return;
            //        }

            //        for (int i = 0; i < _maxStaticCaptureCount; i++)
            //        {
            //            using (var bitmap = BitmapConverter.ToBitmap(mat))
            //            {
            //                StaticImageBuff[buffName][i] = MyCamera.BitmapToBase64String(bitmap);
            //                bitmap.Dispose();
            //            }
            //        }

            //        mat?.Dispose();
            //        return;
            //    }
            //}

            if (string.IsNullOrEmpty(buffName) || _isGrabing || _selectedCamera == null)
                return;

            if (StaticImageBuff.ContainsKey(buffName))
                StaticImageBuff[buffName] = new Tuple<long, string>[_maxStaticCaptureCount];
            else
                StaticImageBuff.Add(buffName, new Tuple<long, string>[_maxStaticCaptureCount]);

            OpenCamera();
            _selectedCamera.ClearBuffer();

            var cpCount = 3;
            if (_maxStaticCaptureCount < 1)
            {
                cpCount = 1;
            }
            else if (_maxStaticCaptureCount > 5)
            {
                cpCount = 5;
            }
            else
            {
                cpCount = _maxStaticCaptureCount;
            }

            var t1 = HighPrecisionTimer.GetTimestamp();

            //for (var i = 0; i < _maxStaticCaptureCount; i++)
            //{
            //    //var mat = _selectedCamera.GetImageBase64FromBuff(i, out int row, out int col);
            //    //StaticImageBuff[buffName][i] = Tuple.Create(HighPrecisionTimer.GetTimestamp(), mat);

            //    var testMat = Cv2.ImRead(@"E:\360MoveData\Users\B1438\Pictures\Camera Roll\Image0.bmp");
            //    var testMatStr = MyCamera.MatToBase64String(testMat);
            //    testMat.Dispose();
            //    testMat = null;
            //    StaticImageBuff[buffName][i] = Tuple.Create(HighPrecisionTimer.GetTimestamp(), testMatStr);
            //}

            if (_selectedCamera.Capture((uint)_maxStaticCaptureCount, 6000))
            {
                for (var i = 0; i < _maxStaticCaptureCount; i++)
                {
                    var mat = _selectedCamera.GetImageBase64FromBuff(i, out int row, out int col);
                    StaticImageBuff[buffName][i] = Tuple.Create(HighPrecisionTimer.GetTimestamp(), mat);
                }
            }
            else
            {
                _selectedCamera.ClearBuffer();
                if (_selectedCamera.Capture((uint)_maxStaticCaptureCount, 6000))
                {
                    for (var i = 0; i < _maxStaticCaptureCount; i++)
                    {
                        var mat = _selectedCamera.GetImageBase64FromBuff(i, out int row, out int col);
                        StaticImageBuff[buffName][i] = Tuple.Create(HighPrecisionTimer.GetTimestamp(), mat);
                    }
                }
                else
                {
                    for (var i = 0; i < _maxStaticCaptureCount; i++)
                    {
                        StaticImageBuff[buffName][i] = Tuple.Create(HighPrecisionTimer.GetTimestamp(), string.Empty); ;
                    }
                }
            }
            var t2 = HighPrecisionTimer.GetTimestamp();
            var ts = HighPrecisionTimer.GetTimestampIntervalMs(t1, t2);
            Console.WriteLine("capture {0} pictures cost {1}/ms", _maxStaticCaptureCount, ts);

            _selectedCamera.ClearBuffer();
            //CloseCamera();
        }

        [Description("启动连续抓拍")]
        public void StartGrab(string buffName, int count)
        {
            if ((CameraSn == "DA0174564" || CameraSn == "GZ0200090004"))
            {
                if (buffName.Equals("产品1动画"))
                {
                    count = 200;

                    if (DynamicImageBuff.ContainsKey(buffName))
                        DynamicImageBuff[buffName] = new string[count];
                    else
                        DynamicImageBuff.Add(buffName, new string[count]);

                    for (var i = 0; i < count; i++)
                    {
                        using (var mat = Cv2.ImRead(string.Format(@"E:\CCD测试样张\动画\CaptureImage\Image{0}.bmp", i)))
                        using (var bitmat = BitmapConverter.ToBitmap(mat))
                        {
                            DynamicImageBuff[buffName][i] = MyCamera.BitmapToBase64String(bitmat);
                            bitmat.Dispose();
                            mat.Dispose();
                        }
                    }

                    return;
                }

                if (buffName.Equals("动画3-转向流水"))
                {
                    var repeat = 1;
                    var oneCount = 11;
                    count = oneCount * repeat;

                    if (DynamicImageBuff.ContainsKey(buffName))
                        DynamicImageBuff[buffName] = new string[count];
                    else
                        DynamicImageBuff.Add(buffName, new string[count]);

                    var imageIndex = 0;
                    for (int c = 0; c < repeat; c++)
                    {
                        for (var i = 0; i < oneCount; i++)
                        {
                            using (var mat = Cv2.ImRead(string.Format(@"E:\CCD测试样张\动画3-转向流水\Image{0}.bmp", i)))
                            using (var bitmat = BitmapConverter.ToBitmap(mat))
                            {
                                DynamicImageBuff[buffName][imageIndex] = MyCamera.BitmapToBase64String(bitmat);
                                bitmat.Dispose();
                                mat.Dispose();
                            }

                            imageIndex++;
                        }
                    }

                    return;
                }
            }

            if (string.IsNullOrEmpty(buffName) || _isGrabing || _selectedCamera == null)
                return;

            if (count < 0)
                count = 50;
            else if (count > 500)
                count = 500;

            _isGrabing = true;

            if (DynamicImageBuff.ContainsKey(buffName))
                DynamicImageBuff[buffName] = new string[count];
            else
                DynamicImageBuff.Add(buffName, new string[count]);

            _selectedCamera.ClearBuffer();
            _isGrabing = true;

            var capture = new Action(() =>
            {
                if (_selectedCamera.Capture((uint)count))
                {
                    for (var i = 0; i < count; i++)
                    {
                        var mat = _selectedCamera.GetImageFromBuff(i, out int row, out int col);
                        using (var bitmap = BitmapConverter.ToBitmap(mat))
                        {
                            DynamicImageBuff[buffName][i] = MyCamera.BitmapToBase64String(bitmap);
                            bitmap.Dispose();
                        }
                    }
                }
            });

            capture.BeginInvoke((ir) =>
            {
                if (!ir.IsCompleted)
                    return;
                capture.EndInvoke(ir);
                _selectedCamera.ClearBuffer();
                _isGrabing = false;
            }, null);
        }

        public void WaitGrabEnd()
        {
            while (_isGrabing)
                Thread.Sleep(25);

            // 测试效果
            //{
            //    if (DynamicImageBuff.Keys.Count > 0)
            //    {
            //        var ts = HighPrecisionTimer.GetTimestamp();
            //        var key = DynamicImageBuff.Keys.First();
            //        var buffer = DynamicImageBuff[key];
            //        for (int i = 0; i < buffer.Length; i++)
            //        {
            //            using (var bitmap = MyCamera.Base64StringToBitmap(buffer[i]))
            //            using (var mat = BitmapConverter.ToMat(bitmap))
            //            {
            //                Cv2.ImWrite(@"E:\test123\ccd20250822\img" + ts.ToString() + i + ".bmp", mat);

            //                mat?.Dispose();
            //                bitmap?.Dispose();
            //            }
            //        }
            //    }
            //}
        }

        [Description("释放图像缓存")]
        public void ReleaseBuff()
        {
            //StaticImageBuff.Clear();
            //DynamicImageBuff.Clear();
        }
    }
}
