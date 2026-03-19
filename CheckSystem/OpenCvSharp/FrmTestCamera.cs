using CommonUtility.HikSdk;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Threading;
using Emgu.CV.Ocl;

namespace CheckSystem.OpenCvSharp
{
    public partial class FrmTestCamera : Form
    {
        private readonly CameraControl _hik = new CameraControl();

        // 连续获取帧
        bool isCapturing = false;

        public FrmTestCamera()
        {
            InitializeComponent();
            MyCamera.PushMat += MyCamera_PushMat;
            _hik.DeviceListAcq();
            Closed += FrmTestCamera_Closed;


            Thread captureThread = new Thread(() =>
            {
                //while (true)
                //{
                //    if (isCapturing)
                //    {
                //        // 获取一帧图像
                //        MvCameraSdk.MV_FRAME_OUT_INFO_EX stFrameInfo = new MvCameraSdk.MV_FRAME_OUT_INFO_EX();
                //        var MV_FRAME_OUT = new MvCameraSdk.MV_FRAME_OUT();

                //        var device = _hik.CameraList[0];
                //        int nRet = device._mvCameraSdk.MV_CC_GetImageBuffer_NET(ref MV_FRAME_OUT, 50000);//camera.MV_CC_GetOneFrameTimeout_NET(pData, 1280 * 960, ref stFrameInfo, 1000);
                //        if (nRet == 0)
                //        {
                //            // 转换为OpenCVSharp的Mat
                //            using (Mat mat = new Mat(stFrameInfo.nHeight, stFrameInfo.nWidth, MatType.CV_8UC1, MV_FRAME_OUT.pBufAddr))
                //            {
                //                // 显示图像
                //                SetPictureBoxImage(pictureBox1, mat.ToBitmap());
                //            }
                //        }
                //        else
                //        {
                //            Console.WriteLine("获取图像失败：" + nRet);
                //        }
                //    }

                //}
            });

            captureThread.Start();
        }

        private void FrmTestCamera_Closed(object sender, EventArgs e)
        {
            _hik.CloseAllCamera();
        }

        private void MyCamera_PushMat(MvCameraSdk.MV_GIGE_DEVICE_INFO gigeInfo, Mat mat)
        {
            //if (gigeInfo.chSerialNumber == "GZ0200090004")
            //    SetPictureBoxImage(pictureBox1, mat.ToBitmap());
            //mat.Dispose();
            //GC.Collect();
        }

        /// <summary>
        /// 多线程设置PictureBox的图像
        /// </summary>
        /// <param name="control"></param>
        /// <param name="value"></param>
        private static void SetPictureBoxImage(ISynchronizeInvoke control, Bitmap value)
        {
            control.Invoke(new Action<PictureBox, Bitmap>((ct, v) => { ct.Image = v; }), new object[] { control, value });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _hik.OpenAllCamera();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _hik.CloseAllCamera();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //_hik.StartCameraGrabBySerialNo("GZ0200090004");

            //if (_hik.CameraList.Any())
            //{
            //    var device = _hik.CameraList[0];

            //    device._mvCameraSdk.MV_CC_SetEnumValue_NET("AcquisitionMode", (uint)MvCameraSdk.MV_CAM_ACQUISITION_MODE.MV_ACQ_MODE_CONTINUOUS);
            //    device._mvCameraSdk.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MvCameraSdk.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_ON);
            //    device._mvCameraSdk.MV_CC_SetEnumValue_NET("TriggerSource", (uint)MvCameraSdk.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_LINE0);
            //    device._mvCameraSdk.MV_CC_SetEnumValue_NET("TriggerSource", (uint)MvCameraSdk.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_SOFTWARE);
            //    device._mvCameraSdk.MV_CC_StartGrabbing_NET();

            //    //device.StartGrab();
            //    isCapturing = true;

            //    Thread.Sleep(1000);
            //    int nRet = device._mvCameraSdk.MV_CC_SetCommandValue_NET("TriggerSoftware");

            //    if (nRet == 0)
            //    {
            //        var stFrameInfo = new MvCameraSdk.MV_FRAME_OUT();
            //        nRet = device._mvCameraSdk.MV_CC_GetImageBuffer_NET(ref stFrameInfo, 1000);
            //        if (nRet == 0)
            //        {
            //            var matSrc = new Mat(stFrameInfo.stFrameInfo.nWidth, stFrameInfo.stFrameInfo.nWidth, MatType.CV_8UC1, stFrameInfo.pBufAddr);
            //            var independentMat = matSrc.Clone();
            //            matSrc.Dispose();
            //            matSrc = null;

            //            ////var grayImg = new Mat();
            //            //if (pFrameInfo.enPixelType == MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_BayerRG8)
            //            //    Cv2.CvtColor(independentMat, independentMat, ColorConversionCodes.BayerRG2RGB);
            //            //else if (pFrameInfo.enPixelType == MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_BayerGR8)
            //            //    Cv2.CvtColor(independentMat, independentMat, ColorConversionCodes.BayerGR2RGB);
            //            //else
            //            //    Cv2.CvtColor(independentMat, independentMat, ColorConversionCodes.BayerRG2BGR);
            //        }
            //    }


            //    //// 获取一帧图像
            //    //MvCameraSdk.MV_FRAME_OUT_INFO_EX stFrameInfo = new MvCameraSdk.MV_FRAME_OUT_INFO_EX();
            //    //var MV_FRAME_OUT = new MvCameraSdk.MV_FRAME_OUT();

            //    ////int nRet = device._mvCameraSdk.MV_CC_GetImageBuffer_NET(ref MV_FRAME_OUT, 1000);
            //    //IntPtr pData = Marshal.AllocHGlobal((int)device.PayloadSize);
            //    //var nRet = device._mvCameraSdk.MV_CC_GetOneFrameTimeout_NET(pData, device.PayloadSize, ref stFrameInfo, 1000);
            //}

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (_hik.CameraList.Any())
            {
                var device = _hik.CameraList[0];
                isCapturing = false;
                device.StopGrab();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            _hik.CaptureBySerialNo("GZ0200090004");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //_hik.SaveImages("GZ0200090004");
            _hik.CaptureBySerialNo("GZ0200090004");
        }

        private bool isStart = false;

        private void uiButton1_Click(object sender, EventArgs e)
        {
            var camrera = _hik.CameraList[0];

            var m_MyCamera = camrera._mvCameraSdk;

            var device =
                (MvCameraSdk.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(camrera.PUser,
                    typeof(MvCameraSdk.MV_CC_DEVICE_INFO));

            m_MyCamera.MV_CC_CreateDevice_NET(ref device);
            m_MyCamera.MV_CC_OpenDevice_NET();
            int nPacketSize = 1500; //m_MyCamera.MV_CC_GetOptimalPacketSize_NET();
            if (nPacketSize > 0)
            {
                m_MyCamera.MV_CC_SetIntValueEx_NET("GevSCPSPacketSize", nPacketSize);

                var isSetMvCcSetGrabStrategyNetOk = m_MyCamera.MV_CC_SetGrabStrategy_NET(MvCameraSdk.MV_GRAB_STRATEGY.MV_GrabStrategy_OneByOne);
                Console.WriteLine("set MV_CC_SetGrabStrategy_NET result = {0}", isSetMvCcSetGrabStrategyNetOk);
                //Console.WriteLine("set resend result = {0}", m_MyCamera.MV_GIGE_SetResend_NET(0, 0, 0));

                //m_MyCamera.MV_CC_SetEnumValue_NET("AcquisitionMode", (uint)MvCameraSdk.MV_CAM_ACQUISITION_MODE.MV_ACQ_MODE_CONTINUOUS);
                m_MyCamera.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MvCameraSdk.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF);
            }

            m_MyCamera.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MvCameraSdk.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_ON);
            m_MyCamera.MV_CC_SetEnumValue_NET("TriggerSource", (uint)MvCameraSdk.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_LINE0);
            m_MyCamera.MV_CC_SetEnumValue_NET("TriggerSource", (uint)MvCameraSdk.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_SOFTWARE);

            m_MyCamera.MV_CC_StartGrabbing_NET();

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Thread.Sleep(500);
                    m_MyCamera.MV_CC_SetCommandValue_NET("TriggerSoftware");
                    Thread.Sleep(500);
                    MvCameraSdk.MV_FRAME_OUT stFrameInfo = new MvCameraSdk.MV_FRAME_OUT();
                    var nRet = m_MyCamera.MV_CC_GetImageBuffer_NET(ref stFrameInfo, 2500);

                    if (nRet == 0)
                    {
                        // 转换为OpenCVSharp的Mat
                        using (Mat matSrc = new Mat(stFrameInfo.stFrameInfo.nHeight, stFrameInfo.stFrameInfo.nWidth, MatType.CV_8UC1, stFrameInfo.pBufAddr))
                        {
                            var independentMat = matSrc.Clone();
                            //var grayImg = new Mat();
                            if (stFrameInfo.stFrameInfo.enPixelType == MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_BayerRG8)
                                Cv2.CvtColor(independentMat, independentMat, ColorConversionCodes.BayerRG2RGB);
                            else if (stFrameInfo.stFrameInfo.enPixelType == MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_BayerGR8)
                                Cv2.CvtColor(independentMat, independentMat, ColorConversionCodes.BayerGR2RGB);
                            else
                                Cv2.CvtColor(independentMat, independentMat, ColorConversionCodes.BayerRG2BGR);

                            // 显示图像
                            SetPictureBoxImage(pictureBox1, independentMat.ToBitmap());
                            independentMat.Dispose();
                        }
                    }
                    else
                    {
                        var independentMat = new Mat(600, 600, MatType.CV_8UC1, new Scalar(0, 0, 0));
                        SetPictureBoxImage(pictureBox1, independentMat.ToBitmap());
                        independentMat.Dispose();
                    }
                }
            });

            //m_MyCamera.MV_CC_StopGrabbing_NET();
            //m_MyCamera.MV_CC_CloseDevice_NET();
            //m_MyCamera.MV_CC_DestroyDevice_NET();
        }

        private List<IntPtr> _ptrs = new List<IntPtr>();

        private void uiButton2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 1000; i++)
            {
                _ptrs.Add(Marshal.AllocHGlobal((int)480000));
            }
        }

        private void uiButton3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < _ptrs.Count; i++)
            {
                var p = _ptrs[i];
                Marshal.FreeHGlobal(p);
            }

            _ptrs.Clear();
        }
    }
}
