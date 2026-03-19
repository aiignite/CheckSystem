using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CommonUtility.HikSdk;
using NationalInstruments.Vision;
using NationalInstruments.Vision.Analysis;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Sunny.UI;

namespace CheckSystem
{
    public partial class FormTestUi : UIForm
    {
        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        MvCameraSdk.MV_CC_DEVICE_INFO_LIST m_pDeviceList;
        private MvCameraSdk[] m_pMyCamera = new MvCameraSdk[4];
        MvCameraSdk.MV_CC_DEVICE_INFO[] m_pDeviceInfo = new MvCameraSdk.MV_CC_DEVICE_INFO[4];
        MvCameraSdk.cbOutputExdelegate cbImage;
        MvCameraSdk.MV_FRAME_OUT_INFO_EX[] m_stFrameInfo = new MvCameraSdk.MV_FRAME_OUT_INFO_EX[4];
        private Object[] m_BufForSaveImageLock = new Object[4];
        public uint[] m_nSaveImageBufSize = new UInt32[4] { 0, 0, 0, 0 };
        public IntPtr[] m_pSaveImageBuf = new IntPtr[4] { IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero };

        public FormTestUi()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            cbImage = ImageCallBack;

            for (int i = 0; i < 4; ++i)
            {
                m_BufForSaveImageLock[i] = new Object();
            }

            WindowState = FormWindowState.Maximized;
            Load += FormTestUi_Load;
            Closed += FormTestUi_Closed;
        }

        private void FormTestUi_Load(object sender, EventArgs e)
        {
            DeviceListAcq();
        }

        private void FormTestUi_Closed(object sender, EventArgs e)
        {
            for (int i = 0; i < 1; ++i)
            {
                int nRet;

                nRet = m_pMyCamera[i].MV_CC_CloseDevice_NET();
                if (MvCameraSdk.MV_OK != nRet)
                {
                    return;
                }

                nRet = m_pMyCamera[i].MV_CC_DestroyDevice_NET();
                if (MvCameraSdk.MV_OK != nRet)
                {
                    return;
                }
            }
        }

        //多线程设置PictureBox的图像
        private static void SetPictureBoxImage(ISynchronizeInvoke control, Bitmap value)
        {
            control.Invoke(new Action<PictureBox, Bitmap>((ct, v) => { ct.Image = v; }), new object[] { control, value });
        }

        private void txtExposure_ValueChanged(object sender, int value)
        {
            //var y = 0 - (Math.Log(640 / (value * 0.001),2) + 1);
            //Console.WriteLine(@"set Exposure=" + y);

            //_mVCapture.Set(CaptureProperty.AutoExposure, 0);
            //_mVCapture.Set(CaptureProperty.Exposure, y);
            //_mVCapture.Set(CaptureProperty.Gain, 3);
            //GetCaptureProperty();
            //Console.WriteLine(@"Get Exposure: " + _mVCapture.Get(CaptureProperty.Exposure));
        }

        /// <summary>
        /// 枚举设备
        /// </summary>
        private void DeviceListAcq()
        {
            GC.Collect();
            int nRet = MvCameraSdk.MV_CC_EnumDevices_NET(MvCameraSdk.MV_GIGE_DEVICE | MvCameraSdk.MV_USB_DEVICE, ref m_pDeviceList);
            if (0 != nRet)
            {
                Console.WriteLine(@"Enumerate devices fail!");
                return;
            }

            for (var i = 0; i < (int)m_pDeviceList.nDeviceNum; i++)
                cmbCameraList.Items.Add(i);
        }

        private void btn_play_Click(object sender, EventArgs e)
        {
            {
                for (int i = 0, j = 0; j < 1; j++)
                {
                    //ch:获取选择的设备信息 | en:Get Selected Device Information
                    MvCameraSdk.MV_CC_DEVICE_INFO device =
                        (MvCameraSdk.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_pDeviceList.pDeviceInfo[j], typeof(MvCameraSdk.MV_CC_DEVICE_INFO));

                    String StrTemp = "";
                    if (device.nTLayerType == MvCameraSdk.MV_GIGE_DEVICE)
                    {
                        MvCameraSdk.MV_GIGE_DEVICE_INFO gigeInfo = (MvCameraSdk.MV_GIGE_DEVICE_INFO)MvCameraSdk.ByteToStruct(device.SpecialInfo.stGigEInfo, typeof(MvCameraSdk.MV_GIGE_DEVICE_INFO));

                        if (gigeInfo.chUserDefinedName != "")
                        {
                            StrTemp = "GEV: " + gigeInfo.chUserDefinedName + " (" + gigeInfo.chSerialNumber + ")";
                        }
                        else
                        {
                            StrTemp = "GEV: " + gigeInfo.chManufacturerName + " " + gigeInfo.chModelName + " (" + gigeInfo.chSerialNumber + ")";
                        }
                    }
                    else if (device.nTLayerType == MvCameraSdk.MV_USB_DEVICE)
                    {
                        MvCameraSdk.MV_USB3_DEVICE_INFO usbInfo = (MvCameraSdk.MV_USB3_DEVICE_INFO)MvCameraSdk.ByteToStruct(device.SpecialInfo.stUsb3VInfo, typeof(MvCameraSdk.MV_USB3_DEVICE_INFO));
                        if (usbInfo.chUserDefinedName != "")
                        {
                            StrTemp = "U3V: " + usbInfo.chUserDefinedName + " (" + usbInfo.chSerialNumber + ")";
                        }
                        else
                        {
                            StrTemp = "U3V: " + usbInfo.chManufacturerName + " " + usbInfo.chModelName + " (" + usbInfo.chSerialNumber + ")";
                        }
                    }

                    //ch:打开设备 | en:Open Device
                    if (null == m_pMyCamera[i])
                    {
                        m_pMyCamera[i] = new MvCameraSdk();
                        if (null == m_pMyCamera[i])
                        {
                            return;
                        }
                    }

                    int nRet = m_pMyCamera[i].MV_CC_CreateDevice_NET(ref device);
                    if (MvCameraSdk.MV_OK != nRet)
                    {
                        return;
                    }

                    nRet = m_pMyCamera[i].MV_CC_OpenDevice_NET();
                    if (MvCameraSdk.MV_OK != nRet)
                    {
                        Console.WriteLine(@"Open Device[{0}] failed! nRet=0x{1}\r\n", StrTemp, nRet.ToString("X"));
                        continue;
                    }
                    else
                    {
                        Console.WriteLine(@"Open Device[{0}] success!\r\n", StrTemp);

                        m_pDeviceInfo[i] = device;
                        // ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
                        if (device.nTLayerType == MvCameraSdk.MV_GIGE_DEVICE)
                        {
                            int nPacketSize = m_pMyCamera[i].MV_CC_GetOptimalPacketSize_NET();
                            if (nPacketSize > 0)
                            {
                                nRet = m_pMyCamera[i].MV_CC_SetIntValueEx_NET("GevSCPSPacketSize", nPacketSize);
                                if (nRet != MvCameraSdk.MV_OK)
                                {
                                    Console.WriteLine(@"Set Packet Size failed! nRet=0x{0}\r\n", nRet.ToString("X"));
                                }
                            }
                            else
                            {
                                Console.WriteLine(@"Get Packet Size failed! nRet=0x{0}\r\n", nPacketSize.ToString("X"));
                            }
                        }

                        m_pMyCamera[i].MV_CC_SetEnumValue_NET("TriggerMode", (uint)MvCameraSdk.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF);
                        m_pMyCamera[i].MV_CC_RegisterImageCallBackEx_NET(cbImage, (IntPtr)i);
                        i++;
                    }
                }
            }

            {
                int nRet;

                // ch:开始采集 | en:Start Grabbing
                for (int i = 0; i < 1; i++)
                {
                    //m_nFrames[i] = 0;
                    m_stFrameInfo[i].nFrameLen = 0;//取流之前先清除帧长度
                    m_stFrameInfo[i].enPixelType = MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_Undefined;
                    nRet = m_pMyCamera[i].MV_CC_StartGrabbing_NET();
                    if (MvCameraSdk.MV_OK != nRet)
                    {
                        Console.WriteLine(@"No.{0} Start Grabbing failed! nRet=0x{1}\r\n", (i + 1).ToString(), nRet.ToString("X"));
                    }
                }
            }
        }

        // ch:取流回调函数 | en:Aquisition Callback Function
        private void ImageCallBack(IntPtr pData, ref MvCameraSdk.MV_FRAME_OUT_INFO_EX pFrameInfo, IntPtr pUser)
        {
            int nIndex = (int)pUser;

            // ch:抓取的帧数 | en:Aquired Frame Number
            //++m_nFrames[nIndex];

            lock (m_BufForSaveImageLock[nIndex])
            {
                if (m_pSaveImageBuf[nIndex] == IntPtr.Zero || pFrameInfo.nFrameLen > m_nSaveImageBufSize[nIndex])
                {
                    if (m_pSaveImageBuf[nIndex] != IntPtr.Zero)
                    {
                        Marshal.Release(m_pSaveImageBuf[nIndex]);
                        m_pSaveImageBuf[nIndex] = IntPtr.Zero;
                    }

                    m_pSaveImageBuf[nIndex] = Marshal.AllocHGlobal((Int32)pFrameInfo.nFrameLen);
                    if (m_pSaveImageBuf[nIndex] == IntPtr.Zero)
                    {
                        return;
                    }
                    m_nSaveImageBufSize[nIndex] = pFrameInfo.nFrameLen;
                }

                m_stFrameInfo[nIndex] = pFrameInfo;
                CopyMemory(m_pSaveImageBuf[nIndex], pData, pFrameInfo.nFrameLen);
            }

            // 创建图像对象

            // 根据获取的像素类型确定OpenCV的MatType
            if (pFrameInfo.enPixelType == MvCameraSdk.MvGvspPixelType.PixelType_Gvsp_BayerRG8)
            {
                var mat = new Mat(pFrameInfo.nHeight, pFrameInfo.nWidth, MatType.CV_8UC1, pData);

                Mat grayImg = new Mat();
                Cv2.CvtColor(mat, grayImg, ColorConversionCodes.BayerRG2BGR);

                var matImage = new Mat(pFrameInfo.nHeight, pFrameInfo.nWidth, MatType.CV_8UC3, grayImg.Data);


                Cv2.Flip(matImage, matImage, FlipMode.Y);
                Rect cMaxrect = new Rect(170, 90, 300, 300);

                //绘制指定区域(人脸框)
                Scalar color = new Scalar(0, 100, 0);
                Cv2.Rectangle(matImage, cMaxrect, color, 2);

                SetPictureBoxImage(pic_cam, matImage.ToBitmap());

                {
                    var channel = matImage.Channels();

                    byte[] bytes = new byte[matImage.Total() * matImage.ElemSize()];// 创建与图像大小相匹配的字节数组

                    Marshal.Copy(matImage.Data, bytes, 0, bytes.Length);// 将Mat对象的数据复制到字节数组中

                    var visionImg = CommonUtility.ImageProcessing.ConvertBytesToVisionImg(bytes, matImage.Cols, matImage.Rows, channel > 1);

                    Algorithms.Copy(visionImg, MainImageViewer.Image);

                    visionImg.Dispose();
                }
            }
        }
    }
}
