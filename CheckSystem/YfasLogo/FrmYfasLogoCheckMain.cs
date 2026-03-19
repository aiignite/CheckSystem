using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonUtility;
using CommonUtility.FileOperator;
using Controller;
using Go;
using HZH_Controls.IconFont;
using NationalInstruments.Vision;
using Newtonsoft.Json;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using StateMachine;
using Sunny.UI;
using FontImages = HZH_Controls.IconFont.FontImages;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace CheckSystem.YfasLogo
{
    public partial class FrmYfasLogoCheckMain : UIForm
    {
        public static double FrontMaxSize;// = (double)11000;
        public static double RearMaxSize;// = (double)7000;

        public static double FrontBaseAngle;// = -64;
        public static double RearBaseAngle;// = -62.5;

        public static IniFileHelper Setup =
            new IniFileHelper(string.Format(@"{0}\YfasLogoConfig\{1}", Program.SysDir, "DeviceSetup.ini"));

        public static Rect Roi = new Rect(950, 750, 3600, 3600);
        public static double Angle = 180;

        private Thread Th { get; set; }
        private BackgroundWorker RefreshBackgroundWorker { get; set; }
        private YfasLogoDevice YfasLogoDeviceController { get; set; }
        private SyControllerWith56Pin SyControllerWith56Pin { get; set; }
        private SerialPort ScanSerialPort { get; set; }

        private static readonly State St = new State();
        private string _xmlFileName;

        private string _scanPortName = string.Empty;
        private int _scanBaudRate;
        //private bool _scanEnable;

        private int _rearLogoExposure;
        private int _rearLogoGain;
        private double _rearLogoCurrMin;
        private double _rearLogoCurrMax;
        private double _rearLogoVoltMin;
        private double _rearLogoVoltMax;
        private string _rearLogoImageSavePath;

        private int _frontLogoExposure;
        private int _frontLogoGain;
        private double _frontLogoCurrMin;
        private double _frontLogoCurrMax;
        private double _frontLogoVoltMin;
        private double _frontLogoVoltMax;
        private string _frontLogoImageSavePath;

        private string _frontLogoCurrentBinding;
        private string _frontLogoVoltBinding;

        private string _rearLogoCurrentBinding;
        private string _rearLogoVoltBinding;

        private bool _rearLogoIsCheckV;
        private bool _rearLogoIsCheckW;
        private bool _rearLogoIsCheckOuterOval;

        private bool _frontLogoIsCheckV;
        private bool _frontLogoIsCheckW;
        private bool _frontLogoIsCheckOuterOval;

        private generator _timeAction;

        #region camera
        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        public uint[] MNSaveImageBufSize = new uint[4] { 0, 0, 0, 0 };
        public IntPtr[] MPSaveImageBuf = new IntPtr[4] { IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero };
        private readonly object[] _mBufForSaveImageLock = new object[4];
        private readonly MyCamera.MV_FRAME_OUT_INFO_EX[] _mStFrameInfo = new MyCamera.MV_FRAME_OUT_INFO_EX[4];

        private MyCamera.cbOutputExdelegate _cbImage;
        private MyCamera.MV_CC_DEVICE_INFO_LIST _mPDeviceList;
        private MyCamera[] _mPMyCamera;
        private MyCamera.MV_CC_DEVICE_INFO[] _mPDeviceInfo;
        bool m_bGrabbing;
        int m_nCanOpenDeviceNum;        // ch:设备使用数量 | en:Used Device Number
        int m_nDevNum;        // ch:在线设备数量 | en:Online Device Number
        int[] m_nFrames;      // ch:帧数 | en:Frame Number
        bool m_bTimerFlag;     // ch:定时器开始计时标志位 | en:Timer Start Timing Flag Bit
        IntPtr[] m_hDisplayHandle;
        #endregion

        public FrmYfasLogoCheckMain()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            Icon = FontImages.GetIcon(
                FontIcons.A_fa_camera, 32, Color.DodgerBlue);

            InitSetupIni();
            InitScanPort();
            InitMainDgv();
            InitCamera();
            Load += FrmYfasLogoCheckMain_Load;
            Closed += FrmYfasLogoCheckMain_Closed;
        }

        private void InitScanPort()
        {
            try
            {
                if (ScanSerialPort != null && ScanSerialPort.IsOpen)
                {
                    ScanSerialPort.DataReceived -= _scanSerialPort_DataReceived;
                    ScanSerialPort.Close();
                }

                ScanSerialPort = new SerialPort(_scanPortName, _scanBaudRate);
                ScanSerialPort.Open();
                ScanSerialPort.DataReceived += _scanSerialPort_DataReceived;
                Updatetxt("扫码枪串口打开成功");
            }
            catch (Exception ex)
            {
                Updatetxt("扫码枪串口打开失败：" + ex.Message);
            }
        }

        private void _scanSerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (YfasLogoDeviceController == null || YfasLogoDeviceController.Execute)
                return;
            var serialPort = sender as SerialPort;
            if (serialPort == null)
                return;

            Thread.Sleep(100);

            var len = serialPort.BytesToRead;
            var buff = new byte[len];
            serialPort.Read(buff, 0, len);

            var barcode = Encoding.ASCII.GetString(buff).TrimEnd();
            Updatetxt(string.Format("扫描到二维码信息：{0}", barcode));
            BeginInvoke(new Action(() =>
            {
                lblBarcodeScan.Text = @"二维码扫描：" + barcode;
            }));
        }

        private void InitSetupIni()
        {
            _xmlFileName = Setup.IniReadValue("DeviceParas", "XmlName");

            var readRoi = Setup.IniReadValue("DeviceParas", "Roi");
            Roi = new Rect(int.Parse(readRoi.Split(',')[0]), int.Parse(readRoi.Split(',')[1]), int.Parse(readRoi.Split(',')[2]), int.Parse(readRoi.Split(',')[3]));
            Angle = double.Parse(Setup.IniReadValue("DeviceParas", "Angle"));

            FrontMaxSize = double.Parse(Setup.IniReadValue("DeviceParas", "FrontMaxSize"));
            RearMaxSize = double.Parse(Setup.IniReadValue("DeviceParas", "RearMaxSize"));
            FrontBaseAngle = double.Parse(Setup.IniReadValue("DeviceParas", "FrontBaseAngle"));
            RearBaseAngle = double.Parse(Setup.IniReadValue("DeviceParas", "RearBaseAngle"));

            _scanPortName = Setup.IniReadValue("DeviceParas", "ScanPortName");
            _scanBaudRate = int.Parse(Setup.IniReadValue("DeviceParas", "ScanBaudRate"));
            //_scanEnable = bool.Parse(Setup.IniReadValue("DeviceParas", "ScanEnable"));

            _frontLogoCurrentBinding = Setup.IniReadValue("DeviceParas", "FrontLogoCurrBinding");
            _frontLogoVoltBinding = Setup.IniReadValue("DeviceParas", "FrontLogoVoltBinding");

            _rearLogoCurrentBinding = Setup.IniReadValue("DeviceParas", "RearLogoCurrBinding");
            _rearLogoVoltBinding = Setup.IniReadValue("DeviceParas", "RearLogoVoltBinding");

            _frontLogoExposure = int.Parse(Setup.IniReadValue("DeviceParas", "FrontLogoExposure"));
            _frontLogoGain = int.Parse(Setup.IniReadValue("DeviceParas", "FrontLogoGain"));
            _frontLogoCurrMin = double.Parse(Setup.IniReadValue("DeviceParas", "FrontLogoCurrMin"));
            _frontLogoCurrMax = double.Parse(Setup.IniReadValue("DeviceParas", "FrontLogoCurrMax"));
            _frontLogoVoltMin = double.Parse(Setup.IniReadValue("DeviceParas", "FrontLogoVoltMin"));
            _frontLogoVoltMax = double.Parse(Setup.IniReadValue("DeviceParas", "FrontLogoVoltMax"));
            _frontLogoImageSavePath = Setup.IniReadValue("DeviceParas", "FrontLogoSavePath");

            _rearLogoExposure = int.Parse(Setup.IniReadValue("DeviceParas", "RearLogoExposure"));
            _rearLogoGain = int.Parse(Setup.IniReadValue("DeviceParas", "RearLogoGain"));
            _rearLogoCurrMin = double.Parse(Setup.IniReadValue("DeviceParas", "RearLogoCurrMin"));
            _rearLogoCurrMax = double.Parse(Setup.IniReadValue("DeviceParas", "RearLogoCurrMax"));
            _rearLogoVoltMin = double.Parse(Setup.IniReadValue("DeviceParas", "RearLogoVoltMin"));
            _rearLogoVoltMax = double.Parse(Setup.IniReadValue("DeviceParas", "RearLogoVoltMax"));
            _rearLogoImageSavePath = Setup.IniReadValue("DeviceParas", "RearLogoSavePath");

            _rearLogoIsCheckV = Setup.IniReadValue("DeviceParas", "RearLogoIsCheckV") == "1" ? true : false;
            _rearLogoIsCheckW = Setup.IniReadValue("DeviceParas", "RearLogoIsCheckW") == "1" ? true : false;
            _rearLogoIsCheckOuterOval = Setup.IniReadValue("DeviceParas", "RearLogoIsCheckOutOval") == "1" ? true : false;

            ckRearLogoV.Checked = _rearLogoIsCheckV;
            ckRearLogoW.Checked = _rearLogoIsCheckW;
            ckRearLogoOuterOval.Checked = _rearLogoIsCheckOuterOval;

            _frontLogoIsCheckV = Setup.IniReadValue("DeviceParas", "FrontLogoIsCheckV") == "1" ? true : false;
            _frontLogoIsCheckW = Setup.IniReadValue("DeviceParas", "FrontLogoIsCheckW") == "1" ? true : false;
            _frontLogoIsCheckOuterOval = Setup.IniReadValue("DeviceParas", "FrontLogoIsCheckOutOval") == "1" ? true : false;

            ckFrontLogoV.Checked = _frontLogoIsCheckV;
            ckFrontLogoW.Checked = _frontLogoIsCheckW;
            ckFrontLogoOuterOval.Checked = _frontLogoIsCheckOuterOval;
        }

        private void InitCamera()
        {
            _mPDeviceList = new MyCamera.MV_CC_DEVICE_INFO_LIST();
            m_bGrabbing = false;
            m_nCanOpenDeviceNum = 0;
            m_nDevNum = 0;
            DeviceListAcq();
            _mPMyCamera = new MyCamera[4];
            _mPDeviceInfo = new MyCamera.MV_CC_DEVICE_INFO[4];
            m_nFrames = new int[4];
            _cbImage = ImageCallBack;
            for (var i = 0; i < 4; ++i)
                _mBufForSaveImageLock[i] = new object();

            m_bTimerFlag = false;
            m_hDisplayHandle = new IntPtr[4];
        }

        private void InitMainDgv()
        {
            //uiMarkLabel1.Text = string.Format(@"测试时间：等待开始{0}测试结果：等待开始{1}测试耗时：等待开始", Environment.NewLine, Environment.NewLine);
            //uiLedBulb1.Color = Color.DarkGoldenrod;

            mainDgv.Style = UIStyle.Gray;
            mainDgv.ReadOnly = true;
            mainDgv.RowHeadersVisible = false;
            mainDgv.AllowUserToAddRows = false;
            mainDgv.AllowUserToResizeRows = false;
            mainDgv.MultiSelect = true;
            mainDgv.RowHeadersVisible = false;
            mainDgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            mainDgv.ClearRows();
            mainDgv.ClearColumns();
            mainDgv.AddColumn("名称", "名称");
            mainDgv.AddColumn("标准", "标准");
            mainDgv.AddColumn("测试值", "测试值");
            mainDgv.AddColumn("结果", "结果");

            mainDgv.AutoResizeColumns();
        }

        private void FrmYfasLogoCheckMain_Load(object sender, EventArgs e)
        {
            //toolStripButton3_Click(null, null);

            OpenCamera();
            SetTriggerMode();
            StatGrap();

            var xmlFile = Program.SysDir + @"\流程配置文件\" + _xmlFileName + ".xml";
            St.Init<ControllerBase>(xmlFile, "Controller.dll");
            St.PushEnter += St_PushEnter;

            var yfas = St.LstControllers.Find(f => f is YfasLogoDevice);
            if (yfas != null)
                YfasLogoDeviceController = yfas as YfasLogoDevice;

            var c = St.LstControllers.Find(f => f is SyControllerWith56Pin);
            if (c != null)
                SyControllerWith56Pin = c as SyControllerWith56Pin;

            foreach (var p in St.DeviceConfig.Parts)
            {
                if (p.Name.StartsWith("I-") || p.Name.StartsWith("Q-"))
                {
                    var lbl = new Label();
                    lbl.Font = new Font("微软雅黑", 8);
                    lbl.Text =
                        p.Name;
                    lbl.Name = p.Name;
                    lbl.BackColor = Color.Gray;
                    lbl.Margin = new Padding(1);

                    flowLayoutPanel1.Controls.Add(lbl);
                }
            }

            _timeAction = generator.tgo(FormSelection.MainStrand, TimeAction);

            TaskCheck();
        }

        private void St_PushEnter(string wsName, string enterStaus, string[] enterAction)
        {
            if (wsName == "主程序")
            {
                BeginInvoke(new Action(() =>
                {
                    topMsgRichTxt.Text = enterStaus;
                }));
            }
        }

        private void FrmYfasLogoCheckMain_Closed(object sender, EventArgs e)
        {
            if (_timeAction != null)
                _timeAction.stop();

            if (RefreshBackgroundWorker != null)
                RefreshBackgroundWorker.CancelAsync();
            StopGrab();
            CloseCamera();
            Environment.Exit(0);
        }

        private void TaskCheck()
        {
            if (Th != null)
            {
                Th.Abort();
                Th.Join();
            }

            Th = new Thread(() => { St.TaskCheckStatus(); }) { IsBackground = true };
            Th.Start();

            RefreshBackgroundWorker = new BackgroundWorker();
            RefreshBackgroundWorker.DoWork += RefreshBackgroundWorker_DoWork;
            RefreshBackgroundWorker.WorkerReportsProgress = true;
            RefreshBackgroundWorker.WorkerSupportsCancellation = true;
            RefreshBackgroundWorker.RunWorkerAsync();
        }

        //private int _las

        private async Task TimeAction()
        {
            uiLedStopwatch1.ShowType = UILedStopwatch.TimeShowType.mmssfff;

            while (true)
            {
                //uiLedStopwatch1.Active

                if (uiLedStopwatch1.IsWorking &&
                    uiLedStopwatch1.TimeSpan >= TimeSpan.FromSeconds(10 * 60))
                {
                    uiLedStopwatch1.Stop();
                }

                if (YfasLogoDeviceController != null)
                    YfasLogoDeviceController.IsMatch10Min = !uiLedStopwatch1.IsWorking;

                foreach (var c in flowLayoutPanel1.Controls)
                {
                    await generator.sleep(5);
                    if (c is Label)
                    {
                        var lbl = c as Label;

                        var pName = lbl.Text;
                        var binding = St.DeviceConfig.Parts.ToList().Find(f => f.Name == pName);
                        var sp = binding.ControllerField.Split(new[] { ".Field." },
                            StringSplitOptions.RemoveEmptyEntries);
                        var controller = sp[0];
                        var field = sp[1];

                        var findController = St.LstControllers.Find(f => ((ControllerBase)f).Name == controller);

                        if (findController == null)
                            continue;
                        {
                            var f = findController.GetType().GetField(field);
                            if (f == null || f.FieldType != typeof(bool)) continue;
                            var v = (bool)f.GetValue(findController);
                            lbl.BackColor = v ? Color.Green : Color.Gray;

                            if (pName.Contains("光栅"))
                            {
                                if (v)
                                {
                                    btnSafetyGrating.BackColor = Color.DarkGoldenrod;
                                    btnSafetyGrating.Text = @"光栅触碰";
                                }
                                else
                                {
                                    btnSafetyGrating.BackColor = Color.DarkCyan;
                                    btnSafetyGrating.Text = @"光栅未触碰";
                                }
                            }
                        }
                    }
                }
            }
        }

        private void RefreshBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;
            string productionName;
            double currMin;
            double currMax;
            double voltMin;
            double voltMax;
            string fileSavePath;
            int gain;
            int exposure;

            bool isCheckV;
            bool isCheckW;
            bool isCheckOuterOval;

            string currentBinding;
            string voltBinding;

            while (!worker.CancellationPending)
            {
                Thread.Sleep(50);

                if (YfasLogoDeviceController == null)
                    continue;

                if (YfasLogoDeviceController.ProductType == 0) // 前灯
                {
                    var name =
                        YfasLogoSqlHelper.ProductType.FrontLamp.GetCustomAttribute<DescriptionAttribute>()
                            .Description;

                    if (!btnProductName.Text.EndsWith(name))
                    {
                        var okCount = YfasLogoSqlHelper.GetTodayCount(true, name);
                        var ngCount = YfasLogoSqlHelper.GetTodayCount(false, name);

                        BeginInvoke(new Action(() =>
                        {
                            uiLedStopwatch1.Stop();
                            uiLedStopwatch1.Start();

                            btnProductName.Text = string.Format(@"当前选择产品：{0}", name);
                            btnCount.Text = string.Format(@"合格={0}/总数={1}", okCount, okCount + ngCount);
                        }));
                    }
                }
                else if (YfasLogoDeviceController.ProductType == 1) // 后灯
                {
                    var name =
                        YfasLogoSqlHelper.ProductType.RearLamp.GetCustomAttribute<DescriptionAttribute>()
                            .Description;

                    if (!btnProductName.Text.EndsWith(name))
                    {
                        var okCount = YfasLogoSqlHelper.GetTodayCount(true, name);
                        var ngCount = YfasLogoSqlHelper.GetTodayCount(false, name);

                        BeginInvoke(new Action(() =>
                        {
                            uiLedStopwatch1.Stop();
                            uiLedStopwatch1.Start();

                            btnProductName.Text = string.Format(@"当前选择产品：{0}", name);
                            btnCount.Text = string.Format(@"合格={0}/总数={1}", okCount, okCount + ngCount);
                        }));
                    }
                }

                if (!YfasLogoDeviceController.Execute)
                    continue;

                // 测试过程
                {
                    BeginInvoke(new Action(() =>
                    {
                        InitMainDgv();
                        mainPictureBox.Image = null;
                    }));

                    var stopWatch = new Stopwatch();
                    stopWatch.Start();

                    var ngCount = 0;

                    // 先根据产品选择的型号，读取处相应配置
                    var config = YfasLogoSqlHelper.GetConfigModels();
                    var toUseConfig = new List<YfasLogoSqlHelper.LogoConfigModel>();

                    if (YfasLogoDeviceController.ProductType == 1) // 红灯
                    {
                        productionName =
                            YfasLogoSqlHelper.ProductType.RearLamp.GetCustomAttribute<DescriptionAttribute>().Description;

                        currMin = _rearLogoCurrMin;
                        currMax = _rearLogoCurrMax;
                        voltMin = _rearLogoVoltMin;
                        voltMax = _rearLogoVoltMax;
                        fileSavePath = _rearLogoImageSavePath;
                        gain = _rearLogoGain;
                        exposure = _rearLogoExposure;

                        currentBinding = _rearLogoCurrentBinding;
                        voltBinding = _rearLogoVoltBinding;

                        isCheckV = _rearLogoIsCheckV;
                        isCheckW = _rearLogoIsCheckW;
                        isCheckOuterOval = _rearLogoIsCheckOuterOval;

                        toUseConfig.AddRange(config.FindAll(f => f.ProductionName == productionName));
                    }
                    else // 白灯
                    {
                        productionName =
                            YfasLogoSqlHelper.ProductType.FrontLamp.GetCustomAttribute<DescriptionAttribute>().Description;

                        currMin = _frontLogoCurrMin;
                        currMax = _frontLogoCurrMax;
                        voltMin = _frontLogoVoltMin;
                        voltMax = _frontLogoVoltMax;
                        fileSavePath = _frontLogoImageSavePath;
                        gain = _frontLogoGain;
                        exposure = _frontLogoExposure;

                        currentBinding = _frontLogoCurrentBinding;
                        voltBinding = _frontLogoVoltBinding;

                        isCheckV = _frontLogoIsCheckV;
                        isCheckW = _frontLogoIsCheckW;
                        isCheckOuterOval = _frontLogoIsCheckOuterOval;

                        toUseConfig.AddRange(config.FindAll(f => f.ProductionName == productionName));
                    }

                    // 设置增益和曝光
                    SetGainExposureTime(gain, exposure);
                    Thread.Sleep(1000);
                    SnapShot();
                    Thread.Sleep(1000);

                    if (mainPictureBox.Image == null)
                    {
                        ngCount++;

                        var toShowProductName = productionName;
                        BeginInvoke(new Action(() =>
                        {
                            //uiLedBulb1.Color = Color.Red;
                            //uiMarkLabel1.Text =
                            //    string.Format("测试时间：{0}{1}测试结果：图片采集失败{2}测试耗时：{3}秒",
                            //        DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"), Environment.NewLine,
                            //        Environment.NewLine, stopWatch.ElapsedMilliseconds / 1000f);

                            var todayOkCount = YfasLogoSqlHelper.GetTodayCount(true, toShowProductName);
                            var todayNgCount = YfasLogoSqlHelper.GetTodayCount(false, toShowProductName);
                            btnCount.Text = string.Format(@"合格={0}/总数={1}", todayOkCount, todayNgCount + todayOkCount);
                        }));
                    }
                    else
                    {
                        string barcode;
                        if (!YfasLogoDeviceController.IsScanEnable)
                        {
                            barcode = Guid.NewGuid().ToString().Substring(24, 12);
                            BeginInvoke(new Action(() =>
                            {
                                lblBarcodeScan.Text = @"二维码扫描：" + barcode;
                            }));
                        }
                        else
                        {
                            barcode = lblBarcodeScan.Text.Replace(@"二维码扫描：", "");
                        }

                        //var barcode = lblBarcodeScan.Text.Replace(@"二维码扫描：", "");
                        if (string.IsNullOrEmpty(barcode))
                        {
                            ngCount++;

                            var toShowProductName = productionName;
                            BeginInvoke(new Action(() =>
                            {
                                //uiLedBulb1.Color = Color.Red;
                                //uiMarkLabel1.Text =
                                //    string.Format("测试时间：{0}{1}测试结果：未扫描二维码{2}测试耗时：{3}秒",
                                //        DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"), Environment.NewLine,
                                //        Environment.NewLine, stopWatch.ElapsedMilliseconds / 1000f);

                                var todayOkCount = YfasLogoSqlHelper.GetTodayCount(true, toShowProductName);
                                var todayNgCount = YfasLogoSqlHelper.GetTodayCount(false, toShowProductName);
                                btnCount.Text = string.Format(@"合格={0}/总数={1}", todayOkCount, todayNgCount + todayOkCount);
                            }));
                        }
                        else
                        {
                            // 先将Image转换成Mat
                            var toCheckMat = BitmapConverter.ToMat(new Bitmap(mainPictureBox.Image));
                            // 使用 COLOR_BGRA2BGR 转换代码将 8UC4 转换为 8UC3
                            Cv2.CvtColor(toCheckMat, toCheckMat, ColorConversionCodes.BGRA2BGR);

                            var baseAngle = RearBaseAngle;
                            var maxSize = RearMaxSize;

                            if (productionName == YfasLogoSqlHelper.ProductType.FrontLamp.GetCustomAttribute<DescriptionAttribute>().Description)
                            {
                                baseAngle = FrontBaseAngle;
                                maxSize = FrontMaxSize;
                            }

                            YfasLogoGrayHelper.AutoRotate(baseAngle, maxSize, ref toCheckMat);

                            var toDrawActions = new List<Action<Mat>>();
                            var txtFont = new Font("Arial", 50, FontStyle.Regular); // 字体（需要确保系统支持该中文字体）
                            var txtColor = Color.Blue; // 文本的颜色

                            var dataModel = new YfasLogoSqlHelper.LogoDataLogMode
                            {
                                ProductName = productionName,
                                Uid = barcode
                            };

                            // 测电流、电压
                            {
                                var curr = Math.Round(
                                    double.Parse(
                                        SyControllerWith56Pin.GetType()
                                            .GetField(currentBinding)
                                            .GetValue(SyControllerWith56Pin)
                                            .ToString()), 2,
                                    MidpointRounding.AwayFromZero);
                                var volt = Math.Round(
                                    double.Parse(
                                        SyControllerWith56Pin.GetType()
                                            .GetField(voltBinding)
                                            .GetValue(SyControllerWith56Pin)
                                            .ToString()), 2,
                                    MidpointRounding.AwayFromZero);

                                if (!JudgeIsOkAndShow(YfasLogoSqlHelper.CheckType.Curr, currMin, currMax, curr, ref dataModel, "mA"))
                                    ngCount++;
                                if (!JudgeIsOkAndShow(YfasLogoSqlHelper.CheckType.Volt, voltMin, voltMax, volt, ref dataModel, "V"))
                                    ngCount++;
                            }
                            // 测八点（内圈）
                            {
                                var findInnerOvals =
                                    toUseConfig.FindAll(
                                        f =>
                                            f.Position.StartsWith("圈-") && f.Type == "Oval" &&
                                            f.ProductionName == productionName);

                                foreach (var findOuterOval in findInnerOvals)
                                {
                                    // 绘制内圈
                                    var oval = JsonConvert.DeserializeObject<OvalContour>(findOuterOval.Object);
                                    var centerX = (int)(oval.Left + oval.Width / 2);
                                    var centerY = (int)(oval.Top + oval.Height / 2);
                                    var center = new Point(centerX, centerY);
                                    var size = new Size(oval.Width / 2, oval.Height / 2);
                                    // 画椭圆
                                    var outerOval = findOuterOval;
                                    toDrawActions.Add(p =>
                                    {
                                        //Cv2.PutText(p, outerOval.Position, new Point(oval.Left + 100, oval.Top + 100),
                                        //    HersheyFonts.HersheyComplex, 2.0, Scalar.Green, 2);
                                        toCheckMat = YfasLogoGrayHelper.DrawChineseTextOnImage(toCheckMat,
                                            outerOval.Position,
                                            new System.Drawing.Point((int)(oval.Left + 10), (int)(oval.Top + 10)),
                                            txtFont, txtColor);
                                        Cv2.Ellipse(toCheckMat, center, size, 0, 0, 360, Scalar.Green, 2);
                                    });
                                }

                                YfasLogoGrayHelper.GrayResult[] innerOvalGrayResults;
                                double innerOvalUnifAll;
                                double innerOvalAverageAll;
                                YfasLogoGrayHelper.OvalsGray(toCheckMat, findInnerOvals.Select(t => t.Object).ToArray(),
                                    out innerOvalGrayResults, out innerOvalUnifAll, out innerOvalAverageAll);

                                var avgGrays = innerOvalGrayResults
                                    .Select(innerOvalGrayResultsItem => innerOvalGrayResultsItem.Average).ToList();

                                // 各自平均值
                                ngCount +=
                                    innerOvalGrayResults.Where(
                                        (t, i) =>
                                            !JudgeIsOkAndShow(
                                                EnumOperater.GetEnum<YfasLogoSqlHelper.CheckType>(
                                                    string.Format("Inner{0}OvalAverage", i + 1)),
                                                findInnerOvals[i].MinAverage, findInnerOvals[i].MaxAverage, t.Average,
                                                ref dataModel))
                                        .Count();
                                // 均匀度
                                var innerOvalAvgGrayMin = avgGrays.Min();
                                var innerOvalAvgGrayMax = avgGrays.Max();
                                var innerOvalUnifByAllAvg = Math.Round(innerOvalAvgGrayMin / innerOvalAvgGrayMax * 100, 2, MidpointRounding.AwayFromZero);
                                if (
                                    !JudgeIsOkAndShow(YfasLogoSqlHelper.CheckType.InnerUnif, findInnerOvals[0].MinHomo,
                                        double.PositiveInfinity, innerOvalUnifByAllAvg, ref dataModel, "%"))
                                    ngCount++;
                                // 整体平均值
                                if (
                                    !JudgeIsOkAndShow(YfasLogoSqlHelper.CheckType.InnerAvarage,
                                        findInnerOvals[0].MinAllAverage, findInnerOvals[0].MaxAllAverage,
                                        innerOvalAverageAll, ref dataModel))
                                    ngCount++;
                            }
                            // 测外圈
                            {
                                var findOuterOval = toUseConfig.Find(f => f.Position == "外圈" && f.Type == "Oval" && f.ProductionName == productionName);

                                // 绘制外圈
                                var oval = JsonConvert.DeserializeObject<OvalContour>(findOuterOval.Object);
                                var centerX = (int)(oval.Left + oval.Width / 2);
                                var centerY = (int)(oval.Top + oval.Height / 2);
                                var center = new Point(centerX, centerY);
                                var size = new Size(oval.Width / 2, oval.Height / 2);
                                // 画椭圆
                                if (isCheckOuterOval)
                                {
                                    toDrawActions.Add(p =>
                                    {
                                        //Cv2.PutText(p, findOuterOval.Position, new Point(oval.Left + 100, oval.Top + 100),
                                        //        HersheyFonts.HersheyComplex, 2.0, Scalar.Green, 2);
                                        toCheckMat = YfasLogoGrayHelper.DrawChineseTextOnImage(toCheckMat,
                                            findOuterOval.Position,
                                            new System.Drawing.Point((int)(oval.Left + 100), (int)(oval.Top + 100)),
                                            txtFont, txtColor);
                                        Cv2.Ellipse(toCheckMat, center, size, 0, 0, 360, Scalar.Green, 2);
                                    });
                                }

                                var outerOvalResult = YfasLogoGrayHelper.OvalGray(toCheckMat, findOuterOval.Object);
                                if (
                                    !JudgeIsOkAndShow(YfasLogoSqlHelper.CheckType.OuterOvalUnif, findOuterOval.MinHomo,
                                        findOuterOval.MaxHomo, outerOvalResult.Unif4, ref dataModel, "%", !isCheckOuterOval))
                                    ngCount++;
                                if (
                                    !JudgeIsOkAndShow(YfasLogoSqlHelper.CheckType.OuterOvalAverage, findOuterOval.MinAverage,
                                        findOuterOval.MaxAverage, outerOvalResult.Average, ref dataModel, isByPass: !isCheckOuterOval))
                                    ngCount++;
                            }
                            // 测V
                            {
                                var findVs =
                                    toUseConfig.FindAll(
                                        f =>
                                            f.Position.StartsWith("V-") && f.Type == "Line" &&
                                            f.ProductionName == productionName);
                                YfasLogoGrayHelper.GrayResult[] vGrayResults;
                                double vUnifAll;
                                double vAverageAll;
                                YfasLogoGrayHelper.LinesGray(toCheckMat, findVs.Select(t => t.Object).ToArray(),
                                    out vGrayResults,
                                    out vUnifAll, out vAverageAll);

                                // 画V线
                                if (isCheckV)
                                {
                                    toDrawActions.AddRange(from v in findVs
                                                           let line1 = JsonConvert.DeserializeObject<LineContour>(v.Object)
                                                           select (Action<Mat>)(p =>
                                                           {
                                                               toCheckMat = YfasLogoGrayHelper.DrawChineseTextOnImage(toCheckMat,
                                                                   v.Position,
                                                                   new System.Drawing.Point((int)line1.Start.X, (int)line1.Start.Y),
                                                                   txtFont,
                                                                   txtColor);
                                                               Cv2.Line(toCheckMat, new Point(line1.Start.X, line1.Start.Y),
                                                                   new Point(line1.End.X, line1.End.Y), Scalar.Green, 2);
                                                           }));
                                }

                                // 各自平均值
                                ngCount +=
                                    vGrayResults.Where(
                                        (t, i) =>
                                            !JudgeIsOkAndShow(
                                                EnumOperater.GetEnum<YfasLogoSqlHelper.CheckType>(
                                                    string.Format("V{0}Average", i + 1)),
                                                findVs[i].MinAverage, findVs[i].MaxAverage, t.Average,
                                                ref dataModel, isByPass: !isCheckV))
                                        .Count();
                                // 均匀度
                                if (
                                    !JudgeIsOkAndShow(YfasLogoSqlHelper.CheckType.VUnif, findVs[0].MinHomo,
                                        double.PositiveInfinity, vUnifAll, ref dataModel, "%", !isCheckV))
                                    ngCount++;
                                // 整体平均值
                                if (
                                    !JudgeIsOkAndShow(YfasLogoSqlHelper.CheckType.VAvarage, findVs[0].MinAllAverage,
                                        findVs[0].MaxAllAverage, vAverageAll, ref dataModel, isByPass: !isCheckV))
                                    ngCount++;
                            }
                            // 测W
                            {
                                var findWs = toUseConfig.FindAll(f => f.Position.StartsWith("W-") && f.Type == "Line" && f.ProductionName == productionName);
                                YfasLogoGrayHelper.GrayResult[] wGrayResults;
                                double wUnifAll;
                                double wAverageAll;
                                YfasLogoGrayHelper.LinesGray(toCheckMat, findWs.Select(t => t.Object).ToArray(), out wGrayResults,
                                    out wUnifAll, out wAverageAll);

                                // 画W线
                                if (isCheckW)
                                {
                                    toDrawActions.AddRange(from v in findWs
                                                           let line1 = JsonConvert.DeserializeObject<LineContour>(v.Object)
                                                           select (Action<Mat>)(p =>
                                                           {
                                                               toCheckMat = YfasLogoGrayHelper.DrawChineseTextOnImage(toCheckMat,
                                                                   v.Position,
                                                                   new System.Drawing.Point((int)line1.Start.X, (int)line1.Start.Y),
                                                                   txtFont,
                                                                   txtColor);
                                                               Cv2.Line(toCheckMat, new Point(line1.Start.X, line1.Start.Y),
                                                                   new Point(line1.End.X, line1.End.Y), Scalar.Green, 2);
                                                           }));
                                }

                                // 各自平均值
                                ngCount +=
                                    wGrayResults.Where(
                                        (t, i) =>
                                            !JudgeIsOkAndShow(
                                                EnumOperater.GetEnum<YfasLogoSqlHelper.CheckType>(
                                                    string.Format("W{0}Average", i + 1)),
                                                findWs[i].MinAverage, findWs[i].MaxAverage, t.Average,
                                                ref dataModel, isByPass: !isCheckW))
                                        .Count();
                                // 均匀度
                                if (
                                    !JudgeIsOkAndShow(YfasLogoSqlHelper.CheckType.WUnif, findWs[0].MinHomo,
                                        double.PositiveInfinity, wUnifAll, ref dataModel, "%", !isCheckW))
                                    ngCount++;
                                // 整体平均值
                                if (
                                    !JudgeIsOkAndShow(YfasLogoSqlHelper.CheckType.WAverage, findWs[0].MinAllAverage,
                                        findWs[0].MaxAllAverage, wAverageAll, ref dataModel, isByPass: !isCheckW))
                                    ngCount++;
                            }

                            // 保存图像
                            if (Directory.Exists(fileSavePath))
                            {
                                try
                                {
                                    // 使用DirectoryInfo类获取目录信息
                                    var directoryInfo = new DirectoryInfo(fileSavePath);

                                    // 获取所有文件信息并排序
                                    var files = directoryInfo.GetFiles()
                                        .OrderBy(f => f.CreationTime) // 按创建日期升序排序
                                        .ToArray(); // 或者使用OrderByDescending(f => f.CreationTime)进行降序排序

                                    if (files.Any() && files.Length > 500)
                                    {
                                        var firstFile = files[0];
                                        firstFile.Delete();
                                    }

                                    toCheckMat.SaveImage(string.Format(@"{0}\{1}_{2}_{3}_{4}_{5}.bmp", fileSavePath,
                                        productionName, ngCount > 0 ? "NG" : "OK",
                                        DateTime.Now.ToString("yyyyMMdd-hhmmss"),
                                        toCheckMat.Width + "_h" + toCheckMat.Height + "_fn", barcode));
                                }
                                catch (Exception)
                                {
                                    // ignored
                                }
                            }

                            // 画roi
                            foreach (var drawAction in toDrawActions)
                            {
                                try
                                {
                                    drawAction(toCheckMat);
                                }
                                catch (Exception exception)
                                {
                                    Console.WriteLine(exception);
                                }
                            }

                            SetPictureBoxImage(mainPictureBox, toCheckMat.ToBitmap());
                            dataModel.Result = ngCount > 0 ? "NG" : "OK";
                            YfasLogoSqlHelper.AddData(dataModel);

                            var toShowProductName = productionName;
                            BeginInvoke(new Action(() =>
                            {
                                mainDgv.AutoResizeColumns();

                                //uiMarkLabel1.Text =
                                //        string.Format("测试时间：{0}{1}测试二维码：{2}{3}测试结果：{4}{5}测试耗时：{6}秒",
                                //            DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"), Environment.NewLine,
                                //            barcode, Environment.NewLine,
                                //            ngCount > 0 ? "NG" : "OK", Environment.NewLine,
                                //            stopWatch.ElapsedMilliseconds / 1000f);

                                //lblBarcodeScan.Text = @"二维码扫描：";
                                //uiLedBulb1.Color = ngCount > 0 ? Color.Red : Color.Green;

                                var todayOkCount = YfasLogoSqlHelper.GetTodayCount(true, toShowProductName);
                                var todayNgCount = YfasLogoSqlHelper.GetTodayCount(false, toShowProductName);
                                btnCount.Text = string.Format(@"合格={0}/总数={1}", todayOkCount, todayNgCount + todayOkCount);
                            }));

                            toCheckMat.Dispose();
                            GC.Collect();
                        }
                    }

                    stopWatch.Stop();
                    YfasLogoDeviceController.IsOk = ngCount == 0;
                    YfasLogoDeviceController.Complete = true;

                    while (true)
                    {
                        if (YfasLogoDeviceController.IsOk || YfasLogoDeviceController.Complete)
                            continue;
                        YfasLogoDeviceController.Execute = false;
                        break;
                    }
                }
            }
        }
        
        private bool JudgeIsOkAndShow(
            YfasLogoSqlHelper.CheckType checkType, double min, double max, double value, ref YfasLogoSqlHelper.LogoDataLogMode dataModel, string unit = "", bool isByPass = false)
        {
            var isOk = false;
            var showName = checkType.GetCustomAttribute<DescriptionAttribute>().Description;
            var range = string.Format("{0}~{1}", min, max);

            var showValue = value.ToString(CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(unit))
            {
                range += "/" + unit;
                showValue += "/" + unit;
            }

            switch (checkType)
            {
                case YfasLogoSqlHelper.CheckType.Curr:
                    dataModel.Curr = value.ToString(CultureInfo.InvariantCulture);
                    break;

                case YfasLogoSqlHelper.CheckType.Volt:
                    dataModel.Volt = value.ToString(CultureInfo.InvariantCulture);
                    break;

                case YfasLogoSqlHelper.CheckType.OuterOvalAverage:
                    dataModel.OuterRingAverage = value.ToString(CultureInfo.InvariantCulture);
                    break;

                case YfasLogoSqlHelper.CheckType.OuterOvalUnif:
                    dataModel.OuterRingUnif = value.ToString(CultureInfo.InvariantCulture);
                    break;

                case YfasLogoSqlHelper.CheckType.V1Average:
                    dataModel.V1Average = value.ToString(CultureInfo.InvariantCulture);
                    break;

                case YfasLogoSqlHelper.CheckType.V2Average:
                    dataModel.V2Average = value.ToString(CultureInfo.InvariantCulture);
                    break;

                case YfasLogoSqlHelper.CheckType.V3Average:
                    dataModel.V3Average = value.ToString(CultureInfo.InvariantCulture);
                    break;

                case YfasLogoSqlHelper.CheckType.VUnif:
                    dataModel.VUnif = value.ToString(CultureInfo.InvariantCulture);
                    break;

                case YfasLogoSqlHelper.CheckType.VAvarage:
                    dataModel.VAverage = value.ToString(CultureInfo.InvariantCulture);
                    break;

                case YfasLogoSqlHelper.CheckType.Inner1OvalAverage:
                    dataModel.InnerOval1Average = value.ToString(CultureInfo.InvariantCulture);
                    break;

                case YfasLogoSqlHelper.CheckType.Inner2OvalAverage:
                    dataModel.InnerOval2Average = value.ToString(CultureInfo.InvariantCulture);
                    break;

                case YfasLogoSqlHelper.CheckType.Inner3OvalAverage:
                    dataModel.InnerOval3Average = value.ToString(CultureInfo.InvariantCulture);
                    break;

                case YfasLogoSqlHelper.CheckType.Inner4OvalAverage:
                    dataModel.InnerOval4Average = value.ToString(CultureInfo.InvariantCulture);
                    break;

                case YfasLogoSqlHelper.CheckType.Inner5OvalAverage:
                    dataModel.InnerOval5Average = value.ToString(CultureInfo.InvariantCulture);
                    break;

                case YfasLogoSqlHelper.CheckType.Inner6OvalAverage:
                    dataModel.InnerOval6Average = value.ToString(CultureInfo.InvariantCulture);
                    break;

                case YfasLogoSqlHelper.CheckType.Inner7OvalAverage:
                    dataModel.InnerOval7Average = value.ToString(CultureInfo.InvariantCulture);
                    break;

                case YfasLogoSqlHelper.CheckType.Inner8OvalAverage:
                    dataModel.InnerOval8Average = value.ToString(CultureInfo.InvariantCulture);
                    break;

                case YfasLogoSqlHelper.CheckType.Inner9OvalAverage:
                    dataModel.InnerOval9Average = value.ToString(CultureInfo.InvariantCulture);
                    break;

                case YfasLogoSqlHelper.CheckType.Inner10OvalAverage:
                    dataModel.InnerOval10Average = value.ToString(CultureInfo.InvariantCulture);
                    break;

                case YfasLogoSqlHelper.CheckType.Inner11OvalAverage:
                    dataModel.InnerOval11Average = value.ToString(CultureInfo.InvariantCulture);
                    break;

                case YfasLogoSqlHelper.CheckType.Inner12OvalAverage:
                    dataModel.InnerOval12Average = value.ToString(CultureInfo.InvariantCulture);
                    break;

                case YfasLogoSqlHelper.CheckType.Inner13OvalAverage:
                    dataModel.InnerOval13Average = value.ToString(CultureInfo.InvariantCulture);
                    break;

                case YfasLogoSqlHelper.CheckType.Inner14OvalAverage:
                    dataModel.InnerOval14Average = value.ToString(CultureInfo.InvariantCulture);
                    break;

                case YfasLogoSqlHelper.CheckType.Inner15OvalAverage:
                    dataModel.InnerOval15Average = value.ToString(CultureInfo.InvariantCulture);
                    break;

                case YfasLogoSqlHelper.CheckType.Inner16OvalAverage:
                    dataModel.InnerOval16Average = value.ToString(CultureInfo.InvariantCulture);
                    break;

                case YfasLogoSqlHelper.CheckType.InnerUnif:
                    dataModel.InnerOvalUnif = value.ToString(CultureInfo.InvariantCulture);
                    break;

                case YfasLogoSqlHelper.CheckType.InnerAvarage:
                    dataModel.InnerOvalAverage = value.ToString(CultureInfo.InvariantCulture);
                    break;

                case YfasLogoSqlHelper.CheckType.W1Average:
                    dataModel.W1Average = value.ToString(CultureInfo.InvariantCulture);
                    break;

                case YfasLogoSqlHelper.CheckType.W2Average:
                    dataModel.W2Average = value.ToString(CultureInfo.InvariantCulture);
                    break;

                case YfasLogoSqlHelper.CheckType.W3Average:
                    dataModel.W3Average = value.ToString(CultureInfo.InvariantCulture);
                    break;

                case YfasLogoSqlHelper.CheckType.W4Average:
                    dataModel.W4Average = value.ToString(CultureInfo.InvariantCulture);
                    break;

                case YfasLogoSqlHelper.CheckType.W5Average:
                    dataModel.W5Average = value.ToString(CultureInfo.InvariantCulture);
                    break;

                case YfasLogoSqlHelper.CheckType.WUnif:
                    dataModel.WUnif = value.ToString(CultureInfo.InvariantCulture);
                    break;

                case YfasLogoSqlHelper.CheckType.WAverage:
                    dataModel.WAverage = value.ToString(CultureInfo.InvariantCulture);
                    break;

                default:
                    throw new ArgumentOutOfRangeException("checkType", checkType, null);
            }

            if (isByPass)
            {
                isOk = true;
            }
            else
            {
                if (value >= min && value <= max)
                    isOk = true;

                BeginInvoke(new Action(() =>
                {
                    var rowIndex = mainDgv.AddRow(showName, range, showValue, isOk ? "OK" : "NG");
                    mainDgv.Rows[rowIndex].DefaultCellStyle.BackColor =
                        isOk ? mainDgv.Rows[rowIndex].DefaultCellStyle.BackColor : Color.Red;
                }));
            }

            return isOk;
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
                var device = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(_mPDeviceList.pDeviceInfo[j], typeof(MyCamera.MV_CC_DEVICE_INFO));

                var strTemp = string.Empty;
                if (device.nTLayerType == MyCamera.MV_GIGE_DEVICE)
                {
                    var gigeInfo = (MyCamera.MV_GIGE_DEVICE_INFO)MyCamera.ByteToStruct(device.SpecialInfo.stGigEInfo, typeof(MyCamera.MV_GIGE_DEVICE_INFO));

                    if (gigeInfo.chUserDefinedName != "")
                    {
                        strTemp = "GEV: " + gigeInfo.chUserDefinedName + " (" + gigeInfo.chSerialNumber + ")";
                    }
                    else
                    {
                        strTemp = "GEV: " + gigeInfo.chManufacturerName + " " + gigeInfo.chModelName + " (" + gigeInfo.chSerialNumber + ")";
                    }
                }
                else if (device.nTLayerType == MyCamera.MV_USB_DEVICE)
                {
                    var usbInfo = (MyCamera.MV_USB3_DEVICE_INFO)MyCamera.ByteToStruct(device.SpecialInfo.stUsb3VInfo, typeof(MyCamera.MV_USB3_DEVICE_INFO));
                    if (usbInfo.chUserDefinedName != "")
                    {
                        strTemp = "U3V: " + usbInfo.chUserDefinedName + " (" + usbInfo.chSerialNumber + ")";
                    }
                    else
                    {
                        strTemp = "U3V: " + usbInfo.chManufacturerName + " " + usbInfo.chModelName + " (" + usbInfo.chSerialNumber + ")";
                    }
                }

                //ch:打开设备 | en:Open Device
                if (null == _mPMyCamera[i])
                {
                    _mPMyCamera[i] = new MyCamera();
                    if (null == _mPMyCamera[i])
                    {
                        return;
                    }
                }

                var nRet = _mPMyCamera[i].MV_CC_CreateDevice_NET(ref device);
                if (MyCamera.MV_OK != nRet)
                {
                    return;
                }

                nRet = _mPMyCamera[i].MV_CC_OpenDevice_NET();
                if (MyCamera.MV_OK != nRet)
                {
                    Updatetxt(string.Format("Open Device[{0}] failed! nRet=0x{1}", strTemp, nRet.ToString("X")));
                    continue;
                }
                else
                {
                    Updatetxt(string.Format("Open Device[{0}] success!", strTemp));

                    m_nCanOpenDeviceNum++;
                    _mPDeviceInfo[i] = device;
                    // ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
                    if (device.nTLayerType == MyCamera.MV_GIGE_DEVICE)
                    {
                        var nPacketSize = _mPMyCamera[i].MV_CC_GetOptimalPacketSize_NET();
                        if (nPacketSize > 0)
                        {
                            nRet = _mPMyCamera[i].MV_CC_SetIntValueEx_NET("GevSCPSPacketSize", nPacketSize);
                            if (nRet != MyCamera.MV_OK)
                            {
                                Updatetxt(string.Format("Set Packet Size failed! nRet=0x{0}", nRet.ToString("X")));
                            }
                        }
                        else
                        {
                            Updatetxt(string.Format("Get Packet Size failed! nRet=0x{0}", nPacketSize.ToString("X")));
                        }
                    }

                    _mPMyCamera[i].MV_CC_SetEnumValue_NET("TriggerMode", (uint)MyCamera.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF);
                    _mPMyCamera[i].MV_CC_RegisterImageCallBackEx_NET(_cbImage, (IntPtr)i);
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

                nRet = _mPMyCamera[i].MV_CC_CloseDevice_NET();
                if (MyCamera.MV_OK != nRet)
                {
                    return;
                }

                nRet = _mPMyCamera[i].MV_CC_DestroyDevice_NET();
                if (MyCamera.MV_OK != nRet)
                {
                    return;
                }
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
            var nRet = MyCamera.MV_CC_EnumDevices_NET(MyCamera.MV_GIGE_DEVICE | MyCamera.MV_USB_DEVICE, ref _mPDeviceList);
            if (0 != nRet)
            {
                //richTextBox.Text += "Enumerate devices fail!\r\n";
                return;
            }

            m_nDevNum = (int)_mPDeviceList.nDeviceNum;

            if (m_nDevNum == 0)
            {
                Updatetxt("no device found!");
            }
            //tbDevNum.Text = m_nDevNum.ToString("d");
        }

        /// <summary>
        /// ch:打开触发模式 | en:Open Trigger Mode
        /// </summary>
        private void SetTriggerMode()
        {
            for (var i = 0; i < m_nCanOpenDeviceNum; ++i)
            {
                _mPMyCamera[i].MV_CC_SetEnumValue_NET("TriggerMode", (uint)MyCamera.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_ON);

                // ch:触发源选择:0 - Line0; | en:Trigger source select:0 - Line0;
                //           1 - Line1;
                //           2 - Line2;
                //           3 - Line3;
                //           4 - Counter;
                //           7 - Software;
                _mPMyCamera[i].MV_CC_SetEnumValue_NET("TriggerSource", (uint)MyCamera.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_LINE0);
            }

            // ch:触发源设为软触发 | en:Set Trigger Source As Software
            for (var i = 0; i < m_nCanOpenDeviceNum; ++i)
            {
                _mPMyCamera[i].MV_CC_SetEnumValue_NET("TriggerSource", (uint)MyCamera.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_SOFTWARE);
            }
        }

        /// <summary>
        /// ch:开始采集 | en:Stop Grabbing
        /// </summary>
        private void StatGrap()
        {
            int nRet;
            //m_hDisplayHandle[0] = mainPictureBox.Handle;
            //m_hDisplayHandle[1] = pictureBox2.Handle;
            //m_hDisplayHandle[2] = pictureBox3.Handle;
            //m_hDisplayHandle[3] = pictureBox4.Handle;

            // ch:开始采集 | en:Start Grabbing
            for (var i = 0; i < m_nCanOpenDeviceNum; i++)
            {
                m_nFrames[i] = 0;
                _mStFrameInfo[i].nFrameLen = 0; //取流之前先清除帧长度
                _mStFrameInfo[i].enPixelType = MyCamera.MvGvspPixelType.PixelType_Gvsp_Undefined;
                nRet = _mPMyCamera[i].MV_CC_StartGrabbing_NET();
                if (MyCamera.MV_OK != nRet)
                {
                    Updatetxt(string.Format("Start Grabbing failed! nRet=0x{0}", nRet.ToString("X")));
                }
            }

            //ch:开始计时  | en:Start Timing
            m_bTimerFlag = true;
            // ch:控件操作 | en:Control Operation
            //SetCtrlWhenStartGrab();
            // ch:标志位置位true | en:Set Position Bit true
            m_bGrabbing = true;
        }

        /// <summary>
        /// ch: 停止采集 | en:Stop Grabbing
        /// </summary>
        private void StopGrab()
        {
            for (var i = 0; i < m_nCanOpenDeviceNum; ++i)
            {
                _mPMyCamera[i].MV_CC_StopGrabbing_NET();
            }
            //ch:标志位设为false  | en:Set Flag Bit false
            m_bGrabbing = false;
            // ch:停止计时 | en:Stop Timing
            m_bTimerFlag = false;

            // ch:控件操作 | en:Control Operation
            //SetCtrlWhenStopGrab();
        }

        private void SnapShot()
        {
            for (var i = 0; i < m_nCanOpenDeviceNum; ++i)
            {
                var nRet = _mPMyCamera[i].MV_CC_SetCommandValue_NET("TriggerSoftware");
                if (MyCamera.MV_OK != nRet)
                    Updatetxt(string.Format("Set software trigger failed! nRet=0x{0}", nRet.ToString("X")));
            }
        }

        private void SetGainExposureTime(int gain, int exposuireTime)
        {
            int nRet;
            for (int i = 0; i < m_nCanOpenDeviceNum; ++i)
            {
                bool bSuccess = true;
                _mPMyCamera[i].MV_CC_SetEnumValue_NET("ExposureAuto", 0);

                nRet = _mPMyCamera[i].MV_CC_SetFloatValue_NET("ExposureTime", float.Parse(exposuireTime.ToString()));
                if (nRet != MyCamera.MV_OK)
                {
                    Updatetxt(string.Format("Set Exposure Time Failed! nRet=0x{0}", nRet.ToString("X")));
                    bSuccess = false;
                }

                _mPMyCamera[i].MV_CC_SetEnumValue_NET("GainAuto", 0);
                nRet = _mPMyCamera[i].MV_CC_SetFloatValue_NET("Gain", float.Parse(gain.ToString()));
                if (nRet != MyCamera.MV_OK)
                {
                    Updatetxt(string.Format("Set Gain Failed! nRet=0x{0}", nRet.ToString("X")));
                    bSuccess = false;
                }

                if (bSuccess)
                {
                    Updatetxt(string.Format("Set Parameters Succeed! Gaint={0}, ExposureTime={1}", gain, exposuireTime));
                }
            }
        }

        private void ResetMember()
        {
            _mPDeviceList = new MyCamera.MV_CC_DEVICE_INFO_LIST();
            m_bGrabbing = false;
            m_nCanOpenDeviceNum = 0;
            m_nDevNum = 0;
            DeviceListAcq();
            m_nFrames = new int[4];
            _cbImage = ImageCallBack;
            m_bTimerFlag = false;
            m_hDisplayHandle = new IntPtr[4];
            _mPDeviceInfo = new MyCamera.MV_CC_DEVICE_INFO[4];
        }

        /// <summary>
        /// ch:取流回调函数 | en:Aquisition Callback Function
        /// </summary>
        /// <param name="pData"></param>
        /// <param name="pFrameInfo"></param>
        /// <param name="pUser"></param>
        private void ImageCallBack(IntPtr pData, ref MyCamera.MV_FRAME_OUT_INFO_EX pFrameInfo, IntPtr pUser)
        {
            var mat = new Mat(pFrameInfo.nHeight, pFrameInfo.nWidth, MatType.CV_8UC1, pData);
            var grayImg = new Mat();
            Cv2.CvtColor(mat, grayImg, ColorConversionCodes.BayerRG2BGR);
            var matImage = new Mat(pFrameInfo.nHeight, pFrameInfo.nWidth, MatType.CV_8UC3, grayImg.Data);

            matImage = matImage.Clone(Roi);
            var center = new Point2f(matImage.Width / 2, matImage.Height / 2);
            var matrix = Cv2.GetRotationMatrix2D(center, 180, 1.0);
            Cv2.WarpAffine(matImage, matImage, matrix, new Size(matImage.Width, matImage.Height));

            SetPictureBoxImage(mainPictureBox, matImage.ToBitmap());
            matImage.Dispose();
            mat.Dispose();
            grayImg.Dispose();
            GC.Collect();
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

        private void tsbtnCameraCali_Click(object sender, EventArgs e)
        {
            if (YfasLogoDeviceController.IsInAuto)
            {
                this.ShowErrorTip("请先将开关旋转到手动");
                return;
            }

            var pwd = string.Empty;
            if (this.InputPasswordDialog(ref pwd))
            {
                if (pwd.ToLower() != "admin")
                {
                    this.ShowInfoTip("密码输入错误");
                    return;
                }
            }
            else
            {
                this.ShowInfoTip("取消密码输入");
                return;
            }

            YfasLogoDeviceController.IsInConfig = true;

            StopGrab();
            CloseCamera();

            int gain;
            int exposure;
            if (YfasLogoDeviceController.ProductType == 0)
            {
                gain = _frontLogoGain;
                exposure = _frontLogoExposure;
            }
            else
            {
                gain = _rearLogoGain;
                exposure = _rearLogoExposure;
            }

            using (var frm = new FrmYfasCameraConfig(exposure, gain, St, YfasLogoDeviceController.ProductType, YfasLogoDeviceController))
                frm.ShowDialog();

            OpenCamera();
            SetTriggerMode();
            StatGrap();

            if (YfasLogoDeviceController.ProductType == 0)
            {
                _frontLogoExposure = int.Parse(Setup.IniReadValue("DeviceParas", "FrontLogoExposure"));
                _frontLogoGain = int.Parse(Setup.IniReadValue("DeviceParas", "FrontLogoGain"));

                _frontLogoExposure = int.Parse(Setup.IniReadValue("DeviceParas", "FrontLogoExposure"));
                _frontLogoGain = int.Parse(Setup.IniReadValue("DeviceParas", "FrontLogoGain"));
                _frontLogoCurrMin = double.Parse(Setup.IniReadValue("DeviceParas", "FrontLogoCurrMin"));
                _frontLogoCurrMax = double.Parse(Setup.IniReadValue("DeviceParas", "FrontLogoCurrMax"));
                _frontLogoVoltMin = double.Parse(Setup.IniReadValue("DeviceParas", "FrontLogoVoltMin"));
                _frontLogoVoltMax = double.Parse(Setup.IniReadValue("DeviceParas", "FrontLogoVoltMax"));
                _frontLogoImageSavePath = Setup.IniReadValue("DeviceParas", "FrontLogoSavePath");
            }
            else
            {
                _rearLogoExposure = int.Parse(Setup.IniReadValue("DeviceParas", "RearLogoExposure"));
                _rearLogoGain = int.Parse(Setup.IniReadValue("DeviceParas", "RearLogoGain"));

                _rearLogoExposure = int.Parse(Setup.IniReadValue("DeviceParas", "RearLogoExposure"));
                _rearLogoGain = int.Parse(Setup.IniReadValue("DeviceParas", "RearLogoGain"));
                _rearLogoCurrMin = double.Parse(Setup.IniReadValue("DeviceParas", "RearLogoCurrMin"));
                _rearLogoCurrMax = double.Parse(Setup.IniReadValue("DeviceParas", "RearLogoCurrMax"));
                _rearLogoVoltMin = double.Parse(Setup.IniReadValue("DeviceParas", "RearLogoVoltMin"));
                _rearLogoVoltMax = double.Parse(Setup.IniReadValue("DeviceParas", "RearLogoVoltMax"));
                _rearLogoImageSavePath = Setup.IniReadValue("DeviceParas", "RearLogoSavePath");
            }

            YfasLogoDeviceController.IsInConfig = false;
        }

        private void tsbtnRoiCali_Click(object sender, EventArgs e)
        {
            var pwd = string.Empty;
            if (this.InputPasswordDialog(ref pwd))
            {
                if (pwd.ToLower() != "admin")
                {
                    this.ShowInfoTip("密码输入错误");
                    return;
                }
            }
            else
            {
                this.ShowInfoTip("取消密码输入");
                return;
            }

            var option = new UIEditOption { AutoLabelWidth = true, Text = @"选择产品" };
            var prMode = new[] { "前灯", "后灯" };
            option.AddCombobox("Product", "产品：", prMode, 0, true, true);

            var frm = new UIEditForm(option);
            frm.Render();
            frm.ShowDialog();

            if (!frm.IsOK)
            {
                this.ShowInfoTip("用户取消了操作");
                return;
            }

            var prIndex = (int)frm["Product"];

            using (var fm = new FrmYfasLogoContourConfig(EnumOperater.GetEnumByValue<YfasLogoSqlHelper.ProductType>(prIndex)))
                fm.ShowDialog();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new FormStateMonitor(St))
                    form.ShowDialog();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void tsbtnDataLog_Click(object sender, EventArgs e)
        {
            using (var form = new FrmYfasLogoDataLog())
                form.ShowDialog();
        }

        private delegate void UpdatetxtDelegate(string text);
        private void Updatetxt(string text)
        {
            var updateRichTextBoxDelegate = new UpdatetxtDelegate(Updatetxt);

            if (bottomMsgRichTxt.InvokeRequired)
            {
                Invoke(updateRichTextBoxDelegate, text);
            }
            else
            {
                if (bottomMsgRichTxt.Text.Length > 10 * 2500)
                    bottomMsgRichTxt.Clear();

                bottomMsgRichTxt.AppendText(string.Format("[{0}]:{1}{2}", DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"), text, Environment.NewLine));
                //uiTextBox4.Text = text;
            }
        }

        private void tsbtnScanConfig_Click(object sender, EventArgs e)
        {
            var pwd = string.Empty;
            if (this.InputPasswordDialog(ref pwd))
            {
                if (pwd.ToLower() != "admin")
                {
                    this.ShowInfoTip("密码输入错误");
                    return;
                }
            }
            else
            {
                this.ShowInfoTip("取消密码输入");
                return;
            }

            var option = new UIEditOption { AutoLabelWidth = true, Text = @"扫码配置" };
            var listPortName = new List<string>();
            var index = 0;
            for (var i = 0; i <= 20; i++)
            {
                var comName = "COM" + (i + 1);
                if (comName == _scanPortName)
                    index = i;
                listPortName.Add(comName);
            }
            option.AddCombobox("端口名称", "端口名称：", listPortName.ToArray(), index, true, true);
            option.AddInteger("波特率", "波特率", _scanBaudRate);
            //option.AddSwitch("扫码启用", "扫码启用", _scanEnable);

            var frm = new UIEditForm(option);
            frm.Render();
            frm.ShowDialog();

            if (!frm.IsOK)
            {
                this.ShowInfoTip("扫码配置取消");
                return;
            }

            var newPortName = listPortName[(int)frm["端口名称"]];
            var newBaudRate = (int)frm["波特率"];
            //var newIsEnable = (bool)frm["扫码启用"];

            Setup.IniWriteValue("DeviceParas", "ScanPortName", newPortName);
            Setup.IniWriteValue("DeviceParas", "ScanBaudRate", newBaudRate.ToString());
            //Setup.IniWriteValue("DeviceParas", "ScanEnable", newIsEnable.ToString());

            _scanPortName = newPortName;//Setup.IniReadValue("DeviceParas", "ScanPortName");
            _scanBaudRate = newBaudRate;//int.Parse(Setup.IniReadValue("DeviceParas", "ScanBaudRate"));
            //_scanEnable = newIsEnable;//bool.Parse(Setup.IniReadValue("DeviceParas", "ScanEnable"));

            InitScanPort();

            this.ShowSuccessTip("扫码配置成功");
        }

        private void tsbtnCheckParasConfig_Click(object sender, EventArgs e)
        {
            var pwd = string.Empty;
            if (this.InputPasswordDialog(ref pwd))
            {
                if (pwd.ToLower() != "admin")
                {
                    this.ShowInfoTip("密码输入错误");
                    return;
                }
            }
            else
            {
                this.ShowInfoTip("取消密码输入");
                return;
            }

            var option = new UIEditOption { AutoLabelWidth = true, Text = @"选择产品" };
            var prMode = new[] { "前灯", "后灯" };
            option.AddCombobox("Product", "产品：", prMode, 0, true, true);

            var frm = new UIEditForm(option);
            frm.Render();
            frm.ShowDialog();

            if (!frm.IsOK)
            {
                this.ShowInfoTip("用户取消了操作");
                return;
            }

            var prIndex = (int)frm["Product"];

            var prType = EnumOperater.GetEnumByValue<YfasLogoSqlHelper.ProductType>(prIndex);

            var isCheckV = false;
            var isCheckW = false;
            var isCheckOuterOval = false;

            if (prIndex == 0) // 前灯
            {
                isCheckV = _frontLogoIsCheckV;
                isCheckW = _frontLogoIsCheckW;
                isCheckOuterOval = _frontLogoIsCheckOuterOval;
            }
            else if (prIndex == 1) // 后灯
            {
                isCheckV = _rearLogoIsCheckV;
                isCheckW = _rearLogoIsCheckW;
                isCheckOuterOval = _rearLogoIsCheckOuterOval;
            }


            var option2 = new UIEditOption { AutoLabelWidth = true, Text = string.Format("{0}：配置检测项", prMode[prIndex]) };
            option2.AddSwitch("IsCheckOuterOval", "是否输出外圈结果：", isCheckOuterOval, "是", "否");
            option2.AddSwitch("IsCheckV", "是否输出“V”结果：", isCheckV, "是", "否");
            option2.AddSwitch("IsCheckW", "是否输出“W”结果：", isCheckW, "是", "否");

            var frm2 = new UIEditForm(option2);
            frm2.Render();
            frm2.ShowDialog();

            if (!frm2.IsOK)
            {
                this.ShowInfoTip("用户取消了操作");
                return;
            }

            isCheckV = (bool)frm2["IsCheckV"];
            isCheckW = (bool)frm2["IsCheckW"];
            isCheckOuterOval = (bool)frm2["IsCheckOuterOval"];

            if (prIndex == 0) // 前灯
            {
                Setup.IniWriteValue("DeviceParas", "FrontLogoIsCheckV", isCheckV ? "1" : "0");
                Setup.IniWriteValue("DeviceParas", "FrontLogoIsCheckW", isCheckW ? "1" : "0");
                Setup.IniWriteValue("DeviceParas", "FrontLogoIsCheckOutOval", isCheckOuterOval ? "1" : "0");
            }
            else if (prIndex == 1) // 后灯
            {
                Setup.IniWriteValue("DeviceParas", "RearLogoIsCheckV", isCheckV ? "1" : "0");
                Setup.IniWriteValue("DeviceParas", "RearLogoIsCheckW", isCheckW ? "1" : "0");
                Setup.IniWriteValue("DeviceParas", "RearLogoIsCheckOutOval", isCheckOuterOval ? "1" : "0");
            }

            _rearLogoIsCheckV = Setup.IniReadValue("DeviceParas", "RearLogoIsCheckV") == "1" ? true : false;
            _rearLogoIsCheckW = Setup.IniReadValue("DeviceParas", "RearLogoIsCheckW") == "1" ? true : false;
            _rearLogoIsCheckOuterOval = Setup.IniReadValue("DeviceParas", "RearLogoIsCheckOutOval") == "1" ? true : false;

            ckRearLogoV.Checked = _rearLogoIsCheckV;
            ckRearLogoW.Checked = _rearLogoIsCheckW;
            ckRearLogoOuterOval.Checked = _rearLogoIsCheckOuterOval;

            _frontLogoIsCheckV = Setup.IniReadValue("DeviceParas", "FrontLogoIsCheckV") == "1" ? true : false;
            _frontLogoIsCheckW = Setup.IniReadValue("DeviceParas", "FrontLogoIsCheckW") == "1" ? true : false;
            _frontLogoIsCheckOuterOval = Setup.IniReadValue("DeviceParas", "FrontLogoIsCheckOutOval") == "1" ? true : false;

            ckFrontLogoV.Checked = _frontLogoIsCheckV;
            ckFrontLogoW.Checked = _frontLogoIsCheckW;
            ckFrontLogoOuterOval.Checked = _frontLogoIsCheckOuterOval;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            //if (this.ShowAskDialog("确定跳过？"))
            //    uiLedStopwatch1.Stop();

            var pwd = string.Empty;
            if (this.InputPasswordDialog(ref pwd))
            {
                if (pwd.ToLower() != "admin")
                {
                    this.ShowErrorTip("密码输入错误");
                    return;
                }
            }
            else
            {
                this.ShowInfoTip("取消密码输入");
                return;
            }

            uiLedStopwatch1.Stop();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (YfasLogoDeviceController.Execute)
            {
                YfasLogoDeviceController.IsOk = false;
                YfasLogoDeviceController.Complete = false;
            }
            else
            {
                YfasLogoDeviceController.Execute = true;
            }
        }
    }
}
