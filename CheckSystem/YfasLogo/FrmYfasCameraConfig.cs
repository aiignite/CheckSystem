using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonUtility.FileOperator;
using Controller;
using Go;
using HZH_Controls.IconFont;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using StateMachine;
using Sunny.UI;
using FontImages = HZH_Controls.IconFont.FontImages;
using Size = OpenCvSharp.Size;

namespace CheckSystem.YfasLogo
{
    public partial class FrmYfasCameraConfig : UIForm
    {
        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        public uint[] m_nSaveImageBufSize = new uint[4] { 0, 0, 0, 0 };
        public IntPtr[] m_pSaveImageBuf = new IntPtr[4] { IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero };
        private object[] m_BufForSaveImageLock = new object[4];
        MyCamera.MV_FRAME_OUT_INFO_EX[] m_stFrameInfo = new MyCamera.MV_FRAME_OUT_INFO_EX[4];

        MyCamera.cbOutputExdelegate cbImage;
        MyCamera.MV_CC_DEVICE_INFO_LIST m_pDeviceList;
        private MyCamera[] m_pMyCamera;
        MyCamera.MV_CC_DEVICE_INFO[] m_pDeviceInfo;
        bool m_bGrabbing;
        int m_nCanOpenDeviceNum;        // ch:设备使用数量 | en:Used Device Number
        int m_nDevNum;        // ch:在线设备数量 | en:Online Device Number
        int[] m_nFrames;      // ch:帧数 | en:Frame Number
        bool m_bTimerFlag;     // ch:定时器开始计时标志位 | en:Timer Start Timing Flag Bit
        IntPtr[] m_hDisplayHandle;

        private int _productType;
        private State _state;
        private YfasLogoDevice _yfasLogoDeviceController;
        private generator _timeAction;

        public FrmYfasCameraConfig(int exposure, int gain, State st, int productType, YfasLogoDevice yfasLogoDeviceController)
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            Icon = FontImages.GetIcon(
                FontIcons.A_fa_camera, 32, Color.DodgerBlue);

            _productType = productType;
            _state = st;
            _yfasLogoDeviceController = yfasLogoDeviceController;

            tbExposure.Text = exposure.ToString();
            tbGain.Text = gain.ToString();

            if (productType == 0)
            {
                textBox1.Text = @"前灯";
                txtCurrMin.Text = Setup.IniReadValue("DeviceParas", "FrontLogoCurrMin");
                txtCurrMax.Text = Setup.IniReadValue("DeviceParas", "FrontLogoCurrMax");
                txtVoltMin.Text = Setup.IniReadValue("DeviceParas", "FrontLogoVoltMin");
                txtVoltMax.Text = Setup.IniReadValue("DeviceParas", "FrontLogoVoltMax");
                txtSaveFilePath.Text = Setup.IniReadValue("DeviceParas", "FrontLogoSavePath");
            }
            else
            {
                textBox1.Text = @"后灯";
                txtCurrMin.Text = Setup.IniReadValue("DeviceParas", "RearLogoCurrMin");
                txtCurrMax.Text = Setup.IniReadValue("DeviceParas", "RearLogoCurrMax");
                txtVoltMin.Text = Setup.IniReadValue("DeviceParas", "RearLogoVoltMin");
                txtVoltMax.Text = Setup.IniReadValue("DeviceParas", "RearLogoVoltMax");
                txtSaveFilePath.Text = Setup.IniReadValue("DeviceParas", "RearLogoSavePath");
            }

            m_pDeviceList = new MyCamera.MV_CC_DEVICE_INFO_LIST();
            m_bGrabbing = false;
            m_nCanOpenDeviceNum = 0;
            m_nDevNum = 0;
            DeviceListAcq();
            m_pMyCamera = new MyCamera[4];
            m_pDeviceInfo = new MyCamera.MV_CC_DEVICE_INFO[4];
            m_nFrames = new int[4];
            cbImage = ImageCallBack;
            for (var i = 0; i < 4; ++i)
                m_BufForSaveImageLock[i] = new object();

            m_bTimerFlag = false;
            m_hDisplayHandle = new IntPtr[4];

            Load += FrmYfasCameraConfig_Load;
            Closed += FrmYfasCameraConfig_Closed;
        }

        private void FrmYfasCameraConfig_Load(object sender, EventArgs e)
        {
            OpenCamera();
            SetContinuesMode();
            StatGrap();
            bnSetParam_Click(null, null);

            foreach (var p in _state.DeviceConfig.Parts)
            {
                if (p.Name.StartsWith("I-"))
                {
                    var lbl = new Label();
                    lbl.Font = new Font("微软雅黑", 8);
                    lbl.Text =
                        p.Name;
                    lbl.Name = p.Name;
                    lbl.BackColor = Color.Gray;
                    lbl.Width = 75;
                    lbl.Height = 75;
                    lbl.Margin = new Padding(5);
                    lbl.TextAlign = ContentAlignment.MiddleCenter;

                    flowLayoutPanel1.Controls.Add(lbl);
                }
                else if (p.Name.StartsWith("Q-"))
                {
                    var btn = new Button();
                    btn.Font = new Font("微软雅黑", 8);
                    btn.Text =
                        p.Name;
                    btn.Name = p.Name;
                    btn.Width = 75;
                    btn.Height = 75;
                    btn.Margin = new Padding(5);
                    btn.TextAlign = ContentAlignment.MiddleCenter;

                    var binding = _state.DeviceConfig.Parts.ToList().Find(f => f.Name == p.Name);
                    var sp = binding.ControllerField.Split(new string[] { ".Field." },
                        StringSplitOptions.RemoveEmptyEntries);
                    var controller = sp[0];
                    var field = sp[1];

                    var findController = _state.LstControllers.Find(f => ((ControllerBase)f).Name == controller);

                    if (findController != null)
                    {
                        var f = findController.GetType().GetField(field);
                        if (f != null && f.FieldType == typeof(bool))
                        {
                            var v = (bool)f.GetValue(findController);
                            btn.BackColor = v ? Color.Green : Color.Gray;
                        }
                    }

                    btn.Click += btn_Click;
                    flowLayoutPanel1.Controls.Add(btn);
                }
            }

            var ledBtn = new Button();
            ledBtn.Font = new Font("微软雅黑", 8, FontStyle.Bold);
            ledBtn.Text = @"LOGO灯电源开关";
            ledBtn.Name = @"LOGO灯电源开关";
            ledBtn.Width = 75;
            ledBtn.Height = 75;
            ledBtn.Margin = new Padding(5);
            ledBtn.TextAlign = ContentAlignment.MiddleCenter;
            ledBtn.BackColor = _yfasLogoDeviceController.IsLedOpen ? Color.Green : Color.Gray;
            ledBtn.Click += ledBtn_Click;
            flowLayoutPanel1.Controls.Add(ledBtn);

            _timeAction = generator.tgo(FormSelection.MainStrand, TimeAction);
        }

        private void FrmYfasCameraConfig_Closed(object sender, EventArgs e)
        {
            if (_timeAction != null)
                _timeAction.stop();
            StopGrab();
            CloseCamera();
        }

        private void ledBtn_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn != null && btn.Text == @"LOGO灯电源开关")
            {
                if (btn.BackColor == Color.Gray)
                {
                    btn.BackColor = Color.Green;
                    _yfasLogoDeviceController.IsLedOpen = true;
                    this.ShowInfoTip("LOGO电源打开");
                }
                else
                {
                    btn.BackColor = Color.Gray;
                    _yfasLogoDeviceController.IsLedOpen = false;
                    this.ShowInfoTip("LOGO电源关闭");
                }
            }
        }

        private void btn_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn != null)
            {
                if (btn.Text.StartsWith("Q-"))
                {
                    var isOpen = false;

                    if (btn.BackColor == Color.Gray)
                    {
                        btn.BackColor = Color.Green;
                        isOpen = true;
                    }
                    else
                    {
                        btn.BackColor = Color.Gray;
                        isOpen = false;
                    }

                    var pName = btn.Text;
                    var binding = _state.DeviceConfig.Parts.ToList().Find(f => f.Name == pName);
                    var sp = binding.ControllerField.Split(new string[] { ".Field." },
                        StringSplitOptions.RemoveEmptyEntries);
                    var controller = sp[0];
                    var field = sp[1];

                    var findController = _state.LstControllers.Find(f => ((ControllerBase)f).Name == controller);

                    if (findController != null)
                    {
                        var f = findController.GetType().GetField(field);
                        if (f != null && f.FieldType == typeof(bool))
                        {
                            f.SetValue(findController, isOpen);
                            this.ShowInfoTip(string.Format("{0}{1}", btn.Text, isOpen ? "打开" : "关闭"));
                        }
                    }

                }
            }
        }

        private async Task TimeAction()
        {
            while (true)
            {
                foreach (var c in flowLayoutPanel1.Controls)
                {
                    await generator.sleep(5);
                    if (c is Label)
                    {
                        var lbl = c as Label;

                        var pName = lbl.Text;
                        var binding = _state.DeviceConfig.Parts.ToList().Find(f => f.Name == pName);
                        var sp = binding.ControllerField.Split(new string[] { ".Field." },
                            StringSplitOptions.RemoveEmptyEntries);
                        var controller = sp[0];
                        var field = sp[1];

                        var findController = _state.LstControllers.Find(f => ((ControllerBase)f).Name == controller);

                        if (findController != null)
                        {
                            var f = findController.GetType().GetField(field);
                            if (f != null && f.FieldType == typeof(bool))
                            {
                                var v = (bool)f.GetValue(findController);
                                lbl.BackColor = v ? Color.Green : Color.Gray;
                            }
                        }
                    }
                    //
                    //textBox_Action.Text = DateTime.Now.ToString("yy-MM-dd HH:mm:ss.fff");
                }
            }
        }

        /// <summary>
        /// ch:初始化、打开相机 | en:Initialization and open devices
        /// </summary>
        private void OpenCamera()
        {
            var bOpened = false;

            // ch:获取使用设备的数量 | en:Get Used Device Number
            var nCameraUsingNum = 1;
            // ch:参数检测 | en:Parameters inspection
            if (nCameraUsingNum <= 0)
            {
                nCameraUsingNum = 1;
            }
            if (nCameraUsingNum > 4)
            {
                nCameraUsingNum = 4;
            }

            for (int i = 0, j = 0; j < m_nDevNum; j++)
            {
                //ch:获取选择的设备信息 | en:Get Selected Device Information
                var device =
                    (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_pDeviceList.pDeviceInfo[j], typeof(MyCamera.MV_CC_DEVICE_INFO));

                var StrTemp = "";
                if (device.nTLayerType == MyCamera.MV_GIGE_DEVICE)
                {
                    var gigeInfo = (MyCamera.MV_GIGE_DEVICE_INFO)MyCamera.ByteToStruct(device.SpecialInfo.stGigEInfo, typeof(MyCamera.MV_GIGE_DEVICE_INFO));

                    if (gigeInfo.chUserDefinedName != "")
                    {
                        StrTemp = "GEV: " + gigeInfo.chUserDefinedName + " (" + gigeInfo.chSerialNumber + ")";
                    }
                    else
                    {
                        StrTemp = "GEV: " + gigeInfo.chManufacturerName + " " + gigeInfo.chModelName + " (" + gigeInfo.chSerialNumber + ")";
                    }
                }
                else if (device.nTLayerType == MyCamera.MV_USB_DEVICE)
                {
                    var usbInfo = (MyCamera.MV_USB3_DEVICE_INFO)MyCamera.ByteToStruct(device.SpecialInfo.stUsb3VInfo, typeof(MyCamera.MV_USB3_DEVICE_INFO));
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
                    m_pMyCamera[i] = new MyCamera();
                    if (null == m_pMyCamera[i])
                    {
                        return;
                    }
                }

                var nRet = m_pMyCamera[i].MV_CC_CreateDevice_NET(ref device);
                if (MyCamera.MV_OK != nRet)
                {
                    return;
                }

                nRet = m_pMyCamera[i].MV_CC_OpenDevice_NET();
                if (MyCamera.MV_OK != nRet)
                {
                    richTextBox.Text += string.Format("Open Device[{0}] failed! nRet=0x{1}\r\n", StrTemp, nRet.ToString("X"));
                    continue;
                }
                else
                {
                    richTextBox.Text += string.Format("Open Device[{0}] success!\r\n", StrTemp);

                    m_nCanOpenDeviceNum++;
                    m_pDeviceInfo[i] = device;
                    // ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
                    if (device.nTLayerType == MyCamera.MV_GIGE_DEVICE)
                    {
                        var nPacketSize = m_pMyCamera[i].MV_CC_GetOptimalPacketSize_NET();
                        if (nPacketSize > 0)
                        {
                            nRet = m_pMyCamera[i].MV_CC_SetIntValueEx_NET("GevSCPSPacketSize", nPacketSize);
                            if (nRet != MyCamera.MV_OK)
                            {
                                richTextBox.Text += string.Format("Set Packet Size failed! nRet=0x{0}\r\n", nRet.ToString("X"));
                            }
                        }
                        else
                        {
                            richTextBox.Text += string.Format("Get Packet Size failed! nRet=0x{0}\r\n", nPacketSize.ToString("X"));
                        }
                    }

                    m_pMyCamera[i].MV_CC_SetEnumValue_NET("TriggerMode", (uint)MyCamera.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF);
                    m_pMyCamera[i].MV_CC_RegisterImageCallBackEx_NET(cbImage, (IntPtr)i);
                    bOpened = true;
                    if (m_nCanOpenDeviceNum == nCameraUsingNum)
                    {
                        break;
                    }
                    i++;
                }
            }

            // ch:只要有一台设备成功打开 | en:As long as there is a device successfully opened
            if (bOpened)
            {
                //tbUseNum.Text = m_nCanOpenDeviceNum.ToString();
                //SetCtrlWhenOpen();
            }
        }

        /// <summary>
        /// ch:关闭相机 | en:Close Device
        /// </summary>
        private void CloseCamera()
        {
            for (var i = 0; i < m_nCanOpenDeviceNum; ++i)
            {
                int nRet;

                nRet = m_pMyCamera[i].MV_CC_CloseDevice_NET();
                if (MyCamera.MV_OK != nRet)
                {
                    return;
                }

                //nRet = m_pMyCamera[i].MV_CC_DestroyDevice_NET();
                //if (MyCamera.MV_OK != nRet)
                //{
                //    return;
                //}
            }

            //控件操作 ch: | en:Control Operation
            //SetCtrlWhenClose();
            // ch:取流标志位清零 | en:Zero setting grabbing flag bit
            m_bGrabbing = false;
            // ch:重置成员变量 | en:Reset member variable
            ResetMember();
        }

        /// <summary>
        /// ch:枚举设备 | en:Create Device List
        /// </summary>
        private void DeviceListAcq()
        {
            GC.Collect();
            var nRet = MyCamera.MV_CC_EnumDevices_NET(MyCamera.MV_GIGE_DEVICE | MyCamera.MV_USB_DEVICE, ref m_pDeviceList);
            if (0 != nRet)
            {
                richTextBox.Text += "Enumerate devices fail!\r\n";
                return;
            }

            m_nDevNum = (int)m_pDeviceList.nDeviceNum;
            //tbDevNum.Text = m_nDevNum.ToString("d");
        }

        /// <summary>
        /// ch:打开触发模式 | en:Open Trigger Mode
        /// </summary>
        private void SetTriggerMode()
        {
            for (var i = 0; i < m_nCanOpenDeviceNum; ++i)
            {
                m_pMyCamera[i].MV_CC_SetEnumValue_NET("TriggerMode", (uint)MyCamera.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_ON);

                // ch:触发源选择:0 - Line0; | en:Trigger source select:0 - Line0;
                //           1 - Line1;
                //           2 - Line2;
                //           3 - Line3;
                //           4 - Counter;
                //           7 - Software;
                m_pMyCamera[i].MV_CC_SetEnumValue_NET("TriggerSource", (uint)MyCamera.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_LINE0);
            }

            // ch:触发源设为软触发 | en:Set Trigger Source As Software
            for (var i = 0; i < m_nCanOpenDeviceNum; ++i)
            {
                m_pMyCamera[i].MV_CC_SetEnumValue_NET("TriggerSource", (uint)MyCamera.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_SOFTWARE);
            }
        }

        /// <summary>
        /// // ch:连续采集 | en:
        /// </summary>
        private void SetContinuesMode()
        {
            for (var i = 0; i < m_nCanOpenDeviceNum; ++i)
            {
                m_pMyCamera[i].MV_CC_SetEnumValue_NET("TriggerMode", (uint)MyCamera.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF);
                //cbSoftTrigger.Enabled = false;
                //bnTriggerExec.Enabled = false;
                bnSaveBmp.Enabled = true;
            }
        }

        /// <summary>
        /// ch:开始采集 | en:Stop Grabbing
        /// </summary>
        private void StatGrap()
        {
            {
                int nRet;
                //m_hDisplayHandle[0] = pictureBox1.Handle;
                //m_hDisplayHandle[1] = pictureBox2.Handle;
                //m_hDisplayHandle[2] = pictureBox3.Handle;
                //m_hDisplayHandle[3] = pictureBox4.Handle;

                // ch:开始采集 | en:Start Grabbing
                for (var i = 0; i < m_nCanOpenDeviceNum; i++)
                {
                    m_nFrames[i] = 0;
                    m_stFrameInfo[i].nFrameLen = 0;//取流之前先清除帧长度
                    m_stFrameInfo[i].enPixelType = MyCamera.MvGvspPixelType.PixelType_Gvsp_Undefined;
                    nRet = m_pMyCamera[i].MV_CC_StartGrabbing_NET();
                    if (MyCamera.MV_OK != nRet)
                    {
                        richTextBox.Text += string.Format("No.{0} Start Grabbing failed! nRet=0x{1}\r\n", (i + 1).ToString(), nRet.ToString("X"));
                    }
                }

                //ch:开始计时  | en:Start Timing
                m_bTimerFlag = true;
                // ch:控件操作 | en:Control Operation
                //SetCtrlWhenStartGrab();
                // ch:标志位置位true | en:Set Position Bit true
                m_bGrabbing = true;
            }
        }

        /// <summary>
        /// ch: 停止采集 | en:Stop Grabbing
        /// </summary>
        private void StopGrab()
        {
            {
                for (var i = 0; i < m_nCanOpenDeviceNum; ++i)
                {
                    m_pMyCamera[i].MV_CC_StopGrabbing_NET();
                }
                //ch:标志位设为false  | en:Set Flag Bit false
                m_bGrabbing = false;
                // ch:停止计时 | en:Stop Timing
                m_bTimerFlag = false;

                // ch:控件操作 | en:Control Operation
                //SetCtrlWhenStopGrab();
            }
        }

        private void ResetMember()
        {
            m_pDeviceList = new MyCamera.MV_CC_DEVICE_INFO_LIST();
            m_bGrabbing = false;
            m_nCanOpenDeviceNum = 0;
            m_nDevNum = 0;
            DeviceListAcq();
            m_nFrames = new int[4];
            cbImage = new MyCamera.cbOutputExdelegate(ImageCallBack);
            m_bTimerFlag = false;
            m_hDisplayHandle = new IntPtr[4];
            m_pDeviceInfo = new MyCamera.MV_CC_DEVICE_INFO[4];
        }

        /// <summary>
        /// ch:取流回调函数 | en:Aquisition Callback Function
        /// </summary>
        /// <param name="pData"></param>
        /// <param name="pFrameInfo"></param>
        /// <param name="pUser"></param>
        private void ImageCallBack(IntPtr pData, ref MyCamera.MV_FRAME_OUT_INFO_EX pFrameInfo, IntPtr pUser)
        {
            var nIndex = (int)pUser;

            // ch:抓取的帧数 | en:Aquired Frame Number
            ++m_nFrames[nIndex];

            lock (m_BufForSaveImageLock[nIndex])
            {
                if (m_pSaveImageBuf[nIndex] == IntPtr.Zero || pFrameInfo.nFrameLen > m_nSaveImageBufSize[nIndex])
                {
                    if (m_pSaveImageBuf[nIndex] != IntPtr.Zero)
                    {
                        Marshal.Release(m_pSaveImageBuf[nIndex]);
                        m_pSaveImageBuf[nIndex] = IntPtr.Zero;
                    }

                    m_pSaveImageBuf[nIndex] = Marshal.AllocHGlobal((int)pFrameInfo.nFrameLen);
                    if (m_pSaveImageBuf[nIndex] == IntPtr.Zero)
                    {
                        return;
                    }
                    m_nSaveImageBufSize[nIndex] = pFrameInfo.nFrameLen;
                }

                m_stFrameInfo[nIndex] = pFrameInfo;
                CopyMemory(m_pSaveImageBuf[nIndex], pData, pFrameInfo.nFrameLen);
            }

            //var stDisplayInfo = new MyCamera.MV_DISPLAY_FRAME_INFO();
            //stDisplayInfo.hWnd = m_hDisplayHandle[nIndex];
            //stDisplayInfo.pData = pData;
            //stDisplayInfo.nDataLen = pFrameInfo.nFrameLen;
            //stDisplayInfo.nWidth = pFrameInfo.nWidth;
            //stDisplayInfo.nHeight = pFrameInfo.nHeight;
            //stDisplayInfo.enPixelType = pFrameInfo.enPixelType;

            //m_pMyCamera[nIndex].MV_CC_DisplayOneFrame_NET(ref stDisplayInfo);

            {
                try
                {
                    var mat = new Mat(pFrameInfo.nHeight, pFrameInfo.nWidth, MatType.CV_8UC1, pData);

                    var grayImg = new Mat();
                    Cv2.CvtColor(mat, grayImg, ColorConversionCodes.BayerRG2BGR);

                    var matImage = new Mat(pFrameInfo.nHeight, pFrameInfo.nWidth, MatType.CV_8UC3, grayImg.Data);

                    matImage = matImage.Clone(FrmYfasLogoCheckMain.Roi);
                    var center = new Point2f(matImage.Width / 2, matImage.Height / 2);
                    var matrix = Cv2.GetRotationMatrix2D(center, FrmYfasLogoCheckMain.Angle, 1.0);
                    Cv2.WarpAffine(matImage, matImage, matrix, new Size(matImage.Width, matImage.Height));

                    SetPictureBoxImage(pictureBox1, matImage.ToBitmap());
                    matImage.Dispose();
                    mat.Dispose();
                    grayImg.Dispose();
                    GC.Collect();
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        /// <summary>
        /// 多线程设置PictureBox的图像
        /// </summary>
        /// <param name="control"></param>
        /// <param name="value"></param>
        private static void SetPictureBoxImage(ISynchronizeInvoke control, Bitmap value)
        {
            try
            {
                control.Invoke(new Action<PictureBox, Bitmap>((ct, v) => { ct.Image = v; }), new object[] { control, value });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void bnStartGrab_Click(object sender, EventArgs e)
        {

        }

        private void bnStopGrab_Click(object sender, EventArgs e)
        {

        }

        private void bnTriggerExec_Click(object sender, EventArgs e)
        {

        }

        private void bnSaveBmp_Click(object sender, EventArgs e)
        {
            //using (var fbd = new FolderBrowserDialog())
            //{
            //    var result = fbd.ShowDialog();

            //    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            //    {
            //        var selectedPath = fbd.SelectedPath;


            //    }
            //}

            if (string.IsNullOrEmpty(txtSaveFilePath.Text) || !Directory.Exists(txtSaveFilePath.Text))
            {
                this.ShowErrorTip("保存失败，请填写正确的文件夹路径");
                return;
            }

            var stSaveParam = new MyCamera.MV_SAVE_IMG_TO_FILE_PARAM();
            lock (m_BufForSaveImageLock[0])
            {
                if (m_stFrameInfo[0].nFrameLen == 0)
                {
                    richTextBox.Text += "save image failed! No data!\r\n";
                    this.ShowErrorTip("保存失败");
                    return;
                }

                try
                {
                    stSaveParam.enImageType = MyCamera.MV_SAVE_IAMGE_TYPE.MV_Image_Bmp;
                    stSaveParam.enPixelType = m_stFrameInfo[0].enPixelType;
                    stSaveParam.pData = m_pSaveImageBuf[0];
                    stSaveParam.nDataLen = m_stFrameInfo[0].nFrameLen;
                    stSaveParam.nHeight = m_stFrameInfo[0].nHeight;
                    stSaveParam.nWidth = m_stFrameInfo[0].nWidth;

                    stSaveParam.pImagePath = string.Format(@"{0}\{1}_{2}_{3}.bmp", txtSaveFilePath.Text,
                        DateTime.Now.ToString("yyyyMMdd-hhmmss"),
                        stSaveParam.nWidth + "_h" + stSaveParam.nHeight + "_fn", Guid.NewGuid().ToString().Substring(24, 12));

                    // 根据获取的像素类型确定OpenCV的MatType
                    //if (stSaveParam.enPixelType == MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG8)
                    {
                        var mat = new Mat(stSaveParam.nHeight, stSaveParam.nWidth, MatType.CV_8UC1, stSaveParam.pData);
                        var grayImg = new Mat();
                        Cv2.CvtColor(mat, grayImg, ColorConversionCodes.BayerRG2BGR);
                        var matImage = new Mat(stSaveParam.nHeight, stSaveParam.nWidth, MatType.CV_8UC3, grayImg.Data);
                        if (File.Exists(stSaveParam.pImagePath))
                            File.Delete(stSaveParam.pImagePath);

                        matImage = matImage.Clone(FrmYfasLogoCheckMain.Roi);

                        var center = new Point2f(matImage.Width / 2, matImage.Height / 2);
                        var matrix = Cv2.GetRotationMatrix2D(center, 180, 1.0);
                        Cv2.WarpAffine(matImage, matImage, matrix, new Size(matImage.Width, matImage.Height));

                        matImage.SaveImage(stSaveParam.pImagePath);
                        matImage.Dispose();
                        mat.Dispose();
                        grayImg.Dispose();
                        GC.Collect();

                        this.ShowSuccessTip("保存成功");
                    }
                }
                catch (Exception ex)
                {
                    this.ShowErrorTip("保存失败：" + ex.Message);
                }
            }
        }

        private void bnSetParam_Click(object sender, EventArgs e)
        {
            try
            {
                float.Parse(tbExposure.Text);
                float.Parse(tbGain.Text);
            }
            catch
            {
                richTextBox.Text += @"Please Enter Correct Type.\r\n";
                return;
            }
            if (float.Parse(tbExposure.Text) < 0 || float.Parse(tbGain.Text) < 0)
            {
                richTextBox.Text += @"Set ExposureTime or Gain fail,Because ExposureTime or Gain less than zero.\r\n";
                return;
            }

            int nRet;
            for (int i = 0; i < m_nCanOpenDeviceNum; ++i)
            {
                bool bSuccess = true;
                m_pMyCamera[i].MV_CC_SetEnumValue_NET("ExposureAuto", 0);

                nRet = m_pMyCamera[i].MV_CC_SetFloatValue_NET("ExposureTime", float.Parse(tbExposure.Text));
                if (nRet != MyCamera.MV_OK)
                {
                    richTextBox.Text += string.Format("Set Exposure Time Failed! nRet=0x{0}\r\n", nRet.ToString("X"));
                    bSuccess = false;
                }

                m_pMyCamera[i].MV_CC_SetEnumValue_NET("GainAuto", 0);
                nRet = m_pMyCamera[i].MV_CC_SetFloatValue_NET("Gain", float.Parse(tbGain.Text));
                if (nRet != MyCamera.MV_OK)
                {
                    richTextBox.Text += string.Format("Set Gain Failed! nRet=0x{1}\r\n", nRet.ToString("X"));
                    bSuccess = false;
                }

                if (bSuccess)
                {
                    richTextBox.Text += string.Format("Set Parameters Succeed! Gaint={0},ExposureTime={1}\r\n", float.Parse(tbGain.Text), float.Parse(tbExposure.Text));
                }
            }
        }

        public static IniFileHelper Setup =
            new IniFileHelper(string.Format(@"{0}\YfasLogoConfig\{1}", Program.SysDir, "DeviceSetup.ini"));

        private void button1_Click(object sender, EventArgs e)
        {
            if (_productType == 0)
            {
                Setup.IniWriteValue("DeviceParas", "FrontLogoExposure", tbExposure.Text);
                Setup.IniWriteValue("DeviceParas", "FrontLogoGain", tbGain.Text);

                Setup.IniWriteValue("DeviceParas", "FrontLogoCurrMin", txtCurrMin.Text);
                Setup.IniWriteValue("DeviceParas", "FrontLogoCurrMax", txtCurrMax.Text);
                Setup.IniWriteValue("DeviceParas", "FrontLogoVoltMin", txtVoltMin.Text);
                Setup.IniWriteValue("DeviceParas", "FrontLogoVoltMax", txtVoltMax.Text);
                Setup.IniWriteValue("DeviceParas", "FrontLogoSavePath", txtSaveFilePath.Text);
            }
            else
            {
                Setup.IniWriteValue("DeviceParas", "RearLogoExposure", tbExposure.Text);
                Setup.IniWriteValue("DeviceParas", "RearLogoGain", tbGain.Text);

                Setup.IniWriteValue("DeviceParas", "RearLogoCurrMin", txtCurrMin.Text);
                Setup.IniWriteValue("DeviceParas", "RearLogoCurrMax", txtCurrMax.Text);
                Setup.IniWriteValue("DeviceParas", "RearLogoVoltMin", txtVoltMin.Text);
                Setup.IniWriteValue("DeviceParas", "RearLogoVoltMax", txtVoltMax.Text);
                Setup.IniWriteValue("DeviceParas", "RearLogoSavePath", txtSaveFilePath.Text);
            }

            this.ShowSuccessTip("保存成功");
        }

        private void txtSaveFilePath_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                var result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    txtSaveFilePath.Text = fbd.SelectedPath;
                }
            }
        }
    }
}
