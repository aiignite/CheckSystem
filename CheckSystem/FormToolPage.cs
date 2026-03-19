//using CheckSystem.AlcmAutoDevice;
using CheckSystem.CAN;
using CheckSystem.CcdForms;
using CheckSystem.DebugCCD;
using CheckSystem.EmulatorForms;
using CheckSystem.HelperForms;
using CheckSystem.HelperForms.AirOutlet;
using CheckSystem.HelperForms.AnalogMax25608;
using CheckSystem.HelperForms.Asl5115;
using CheckSystem.HelperForms.AsuvReWrite;
using CheckSystem.HelperForms.BD18331;
using CheckSystem.HelperForms.F03RearLamp;
using CheckSystem.HelperForms.GeeleyRgbLampControl;
using CheckSystem.HelperForms.HalHac3980;
using CheckSystem.HelperForms.P12LHeadLamp;
using CheckSystem.HelperForms.Psi5Reader;
using CheckSystem.HelperForms.S12LHeadLamp;
using CheckSystem.HelperForms.Sgm3582RearLamp;
using CheckSystem.HelperForms.Sgm458Plg;
using CheckSystem.HelperForms.SgmWirelessCharging;
using CheckSystem.HelperForms.Solenoid;
using CheckSystem.HelperForms.Tld7002;
using CheckSystem.HelperForms.U557HeadLamp;
using CheckSystem.HelperForms.VW323RearLamp;
using CheckSystem.HelperForms.VW336HeadLamp;
using CheckSystem.HelperForms.VW426RearLamp;
using CheckSystem.HelperForms.VW491RearLamp;
using CheckSystem.HelperForms.XiaoPeng;
using CheckSystem.LIN;
using CheckSystem.MaterialHelperForms;
using CheckSystem.OpenCvSharp;
using CheckSystem.PesVision;
using CheckSystem.RobotForms;
using CheckSystem.SmtForms;
using CheckSystem.SmtForms.DataUploader;
using CheckSystem.SmtForms.LaserCarving;
using CheckSystem.SyController;
using CheckSystem.SyRenesasMcuController;
using CheckSystem.UART;
using CheckSystem.VisionDetection;
using CheckSystem.VisionDetection.Calibration;
using CheckSystem.VisionDetection.Calibration.StaticVision;
//using CheckSystem.Yfas;
//using CheckSystem.YfasLogo;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CheckSystem.HelperForms.BD18333;

namespace CheckSystem
{
    public partial class FormToolPage : UIPage
    {
        public static List<string> ListTools = new List<string>();

        public FormToolPage()
        {
            InitializeComponent();
            uiListBox1.DoubleClick += uiListBox1_ItemDoubleClick;
        }

        public static void InitTools()
        {
            ListTools.Clear();
            ListTools.Add("CAN调试器");
            ListTools.Add("LIN调试器");
            ListTools.Add("56PIN控制器调试器");
            ListTools.Add("瑞萨MCU控制器调试工具");
            ListTools.Add("Uart调试器");
            ListTools.Add("机器人控制器调试器");
            ListTools.Add("DBC数据视图");
            ListTools.Add("获取IP及MAC地址");
            ListTools.Add("查看系统图标");
            ListTools.Add("CCD参数设置");
            ListTools.Add("视觉检测系统");
            ListTools.Add("EXCEL检测数据解析明细");
            ListTools.Add("TLD7002");
            ListTools.Add("TPS929120");
            ListTools.Add("TPS92662");
            ListTools.Add("TPS92664");
            ListTools.Add("ASL5115");
            ListTools.Add("BD18331");
            ListTools.Add("BD18333_EUV");
            ListTools.Add("ANALOG NAX25608");
            ListTools.Add("Geeley氛围灯");
            ListTools.Add("P12L前灯");
            ListTools.Add("S12L前灯");
            ListTools.Add("AUDI427前灯");
            ListTools.Add("HELA后灯写入");
            ListTools.Add("VW336前灯");
            ListTools.Add("VW323后灯");
            ListTools.Add("VW323后灯-图莫斯");
            ListTools.Add("VW323-0CS后灯");
            ListTools.Add("VW323-0CS后灯-图莫斯");
            ListTools.Add("VW323高低配点灯盒");
            ListTools.Add("VW491后灯");
            ListTools.Add("VW491后灯-图莫斯");
            ListTools.Add("VW491后灯-瑞萨芯片单片机");
            ListTools.Add("VW426后灯");
            ListTools.Add("VW426后灯-图莫斯");
            ListTools.Add("SGM358-2后灯");
            ListTools.Add("仿真器Emulator");
            ListTools.Add("SGM458PLG生成可用的追溯信息");
            ListTools.Add("SGM45PLG数据查询");
            ListTools.Add("仓库电子料标签生成");
            ListTools.Add("SAP入库");
            ListTools.Add("延锋座椅模块");
            ListTools.Add("延锋LOGO灯检测");
            ListTools.Add("SMT激光打标");
            ListTools.Add("SMT数据采集上传");
            ListTools.Add("SMT_高银AOI数据补上传");
            ListTools.Add("SGM458PDM-LoadBox");
            ListTools.Add("GeeleyCX1E-LoadBox");
            ListTools.Add("OpenCvSharp");
            ListTools.Add("音乐氛围灯模块自动线终检");
            ListTools.Add("PSI5_Reader");
            ListTools.Add("PSI5_Programmer");
            ListTools.Add("HAL_3980配置升级");
            ListTools.Add("TEST_Camera");
            ListTools.Add("TEST_DynamicVisionCali");
            ListTools.Add("TEST_StaticVisionCali");
            ListTools.Add("U557_EMC");
            ListTools.Add("U557_EMC_双通道LIN");
            ListTools.Add("HELA_MDF");
            ListTools.Add("PES_Vision");
            ListTools.Add("BaseString64ToBitmap");
            ListTools.Add("OSRAM光型测试工具");
            ListTools.Add("电磁阀盒子");
            ListTools.Add("小鹏X3前后灯");
            ListTools.Add("PES光型测试工具");
            ListTools.Add("MDF光型测试工具");
            ListTools.Add("HVSM-LoadBox");
            ListTools.Add("DX1H-LoadBox");
            ListTools.Add("相机测试");
            ListTools.Add("SL-Display");
            ListTools.Add("CCD追溯");
            ListTools.Add("无线充数据查询");
            ListTools.Add("循环扫风控制系统");
            ListTools.Add("F03尾灯");
            ListTools.Add("无线充读取追溯");
        }

        public override void Init()
        {
            uiListBox1.Items.Clear();
            foreach (var t in ListTools)
                uiListBox1.Items.Add(t);
        }

        private void uiListBox1_ItemDoubleClick(object sender, EventArgs e)
        {
            if (uiListBox1.SelectedItem == null)
            {
                ShowErrorTip("请选择");
                return;
            }
            var name = uiListBox1.SelectedItem.ToString();
            OpenToolForm(name);
        }

        public static void OpenToolForm(string name)
        {
            switch (name)
            {
                case "BaseString64ToBitmap":
                    using (var fm = new FrmBaseString64ToBitmap())
                        fm.ShowDialog();
                    break;

                case "CAN调试器":
                    using (var fm = new CanDataViewForm())
                        fm.ShowDialog();
                    break;

                case "LIN调试器":
                    using (var fm = new LinDataViewForm())
                        fm.ShowDialog();
                    break;

                case "56PIN控制器调试器":
                    using (var fm = new ControllerWith56PinDataViewForm())
                        fm.ShowDialog();
                    break;

                case "瑞萨MCU控制器调试工具":
                    using (var fm = new FrmRenesasMcnMultipleControllers())
                        fm.ShowDialog();
                    break;

                case "Uart调试器":
                    using (var frm = new UartDataViewForm())
                        frm.ShowDialog();
                    break;

                case "机器人控制器调试器":
                    using (var fm = new RobotControllerDeviceMgrForm())
                        fm.ShowDialog();
                    break;

                case "DBC数据视图":
                    using (var fm = new CanDbcFormatViewer())
                        fm.ShowDialog();
                    break;

                case "EXCEL检测数据解析明细":
                    using (var fm = new MyWndExcelCheckDataJsonSplit())
                        fm.ShowDialog();
                    break;

                case "获取IP及MAC地址":
                    using (var fm = new GetIpList())
                        fm.ShowDialog();
                    break;

                case "查看系统图标":
                    using (var fm = new GetIcons())
                        fm.ShowDialog();
                    break;

                case "仿真器Emulator":
                    using (var fm = new EmulatorProductionSelectForm())
                        fm.ShowDialog();
                    break;

                case "TLD7002":
                    using (var fm = new Tld7002DeviceMgr())
                        fm.ShowDialog();
                    break;

                case "TPS929120":
                    using (var fm = new HelperForms.Tps929120.TpsFormDeviceMgr())
                        fm.ShowDialog();
                    break;

                case "TPS92662":
                    using (var fm = new HelperForms.Tps92662.TpsFormDeviceMgr())
                        fm.ShowDialog();
                    break;

                case "TPS92664":
                    using (var fm = new HelperForms.TPS92994.FrmTps92664())
                        fm.ShowDialog();
                    break;

                case "ASL5115":
                    using (var fm = new AslCanDeviceMgrForm())
                        fm.ShowDialog();
                    break;

                case "BD18331":
                    using (var fm = new Bd18331CanDeviceMgrForm())
                        fm.ShowDialog();
                    break;

                case "BD18333_EUV":
                    using (var fm = new Bd18333FormLedControl())
                        fm.ShowDialog();
                    break;

                case "ANALOG NAX25608":
                    using (var fm = new AnalogMax25608DeviceMgrcs())
                        fm.ShowDialog();
                    break;

                case "ElmosE522数据解析工具":
                    using (var fm = new GetElmosE522HeaderFormat())
                        fm.ShowDialog();
                    break;

                case "CCD参数设置":
                    using (var form = new FormLedParaXmlList())
                        form.ShowDialog();
                    break;

                case "高度传感器":
                    using (var form = new Pb06Guimt6501())
                        form.ShowDialog();
                    break;

                case "ChartForm":
                    using (var form = new ChartForm())
                        form.ShowDialog();
                    break;

                case "Geeley氛围灯":
                    using (var form = new GeeleyRgbLampControlForm())
                        form.ShowDialog();
                    break;

                case "P12L前灯":
                    using (var form = new P12LHeadLampDeviceMgr())
                        form.ShowDialog();
                    break;

                case "VW336前灯":
                    using (var frm = new Vw336HeadLamp())
                        frm.ShowDialog();
                    break;

                case "S12L前灯":
                    using (var form = new S12LHeadLampDeviceMgr())
                        form.ShowDialog();
                    break;

                case "HELA后灯写入":
                    using (var form = new FrmAsuvReWrite())
                        form.ShowDialog();
                    break;

                case "VW323后灯":
                    using (var form = new Vw323RearLamp(false))
                        form.ShowDialog();
                    break;

                case "VW323后灯-图莫斯":
                    using (var form = new Vw323RearLamp(true))
                        form.ShowDialog();
                    break;

                case "VW323-0CS后灯":
                    using (var form = new Vw323RearLamp0Cs(false))
                        form.ShowDialog();
                    break;

                case "VW323-0CS后灯-图莫斯":
                    using (var form = new Vw323RearLamp0Cs(true))
                        form.ShowDialog();
                    break;

                case "VW323高低配点灯盒":
                    using (var form = new Vw323RearLampWithRenesasController())
                        form.ShowDialog();
                    break;

                case "VW491后灯":
                    using (var form = new Vw491RearLamp(false))
                        form.ShowDialog();
                    break;

                case "VW491后灯-图莫斯":
                    using (var form = new Vw491RearLamp(true))
                        form.ShowDialog();
                    break;

                case "VW491后灯-瑞萨芯片单片机":
                    using (var form = new Vw491RearLamp(false, true))
                        form.ShowDialog();
                    break;

                case "VW426后灯":
                    using (var form = new Vw426RearLamp(false))
                        form.ShowDialog();
                    break;

                case "VW426后灯-图莫斯":
                    using (var form = new Vw426RearLamp(true))
                        form.ShowDialog();
                    break;

                case "SGM358-2后灯":
                    using (var form = new FrmSgm3582RearLamp())
                        form.ShowDialog();
                    break;

                case "SGM458PLG生成可用的追溯信息":
                    using (var form = new GetPlgCompareVersionValue())
                        form.ShowDialog();
                    break;

                case "SGM45PLG数据查询":
                    using (var form = new SqlDataRead())
                        form.ShowDialog();
                    break;

                case "仓库电子料标签生成":
                    using (var form = new HikRobotForm())
                        form.ShowDialog();
                    break;

                case "SAP入库":
                    using (var form = new HikSerialPortToSap())
                        form.ShowDialog();
                    break;

                //case "延锋座椅模块":
                //    var loginForm = new YfasLoginForm();
                //    if (loginForm.ShowDialog() == DialogResult.OK)
                //        using (var form = new YfasMainForm())
                //            form.ShowDialog();
                //    break;

                //case "延锋LOGO灯检测":
                //    using (var formLogo = new FrmYfasLogoCheckMain())
                //        formLogo.ShowDialog();
                //    //using (var formLogo = new FrmYfasLogoGrayConfig())
                //    //    formLogo.ShowDialog();
                //    break;

                case "视觉检测系统":
                    //var loginFrm = new FrmVisionLogin();
                    //if (loginFrm.ShowDialog() == DialogResult.OK)
                    {
                        using (var form = new FrmVisionCheckMain())
                            form.ShowDialog();
                    }
                    break;

                case "SMT激光打标":
                    using (var form = new FrmLaserCarving())
                        form.ShowDialog();
                    break;

                case "SMT数据采集上传":
                    using (var form = new FrmDataUpload())
                        form.ShowDialog();
                    break;

                case "SMT_高银AOI数据补上传":
                    using (var form = new FrmKohYoungReUpload())
                        form.ShowDialog();
                    break;

                case "SGM458PDM-LoadBox":
                    using (var frm = new HelperForms.Sgm458Pdm.FrmLoadBox())
                        frm.ShowDialog();
                    break;

                case "GeeleyCX1E-LoadBox":
                    using (var frm = new HelperForms.GeeleyCx1e.FrmDutMain())
                        frm.ShowDialog();
                    break;

                case "OpenCvSharp":
                    using (var frm = new FrmTestPaint())
                        frm.ShowDialog();
                    break;

                //case "音乐氛围灯模块自动线终检":
                //    using (var frm = new FrmAlcmMain())
                //        frm.ShowDialog();
                //    break;

                case "PSI5_Reader":
                    using (var frm = new FrmPsi5MultipleReader())
                        frm.ShowDialog();
                    break;

                case "PSI5_Programmer":
                    using (var frm = new FrmProgramming())
                        frm.ShowDialog();
                    break;

                case "HAL_3980配置升级":
                    using (var frm = new FrmHalReWrite())
                        frm.ShowDialog();
                    break;

                case "TEST_Camera":
                    using (var frm = new FrmTestCamera())
                        frm.ShowDialog();
                    break;

                case "TEST_DynamicVisionCali":
                    using (var frm = new FrmDynamicVisionCali())
                        frm.ShowDialog();
                    break;

                case "TEST_StaticVisionCali":
                    using (var frm = new FrmStaticVisionCali())
                        frm.ShowDialog();
                    break;

                case "U557_EMC":
                    using (var frm = new FrmU557Emc())
                        frm.ShowDialog();
                    break;

                case "U557_EMC_双通道LIN":
                    using (var frm = new FrmU557Emc2Channels())
                        frm.ShowDialog();
                    break;

                case "HELA_MDF":
                    using (var frm = new HelaMdf())
                        frm.ShowDialog();
                    break;

                case "PES_Vision":
                    using (var frm = new PesVisionMain())
                        frm.ShowDialog();

                    //using (var frm = new PesVisionConfig())
                    //    frm.ShowDialog();
                    break;

                case "PES光型测试工具":
                    using (var frm = new FrmPesTest())
                        frm.ShowDialog();
                    break;

                case "OSRAM光型测试工具":
                    using (var frm = new FrmOsramTest())
                        frm.ShowDialog();
                    break;

                case "电磁阀盒子":
                    using (var frm = new FrmSolenoid())
                        frm.ShowDialog();
                    break;

                case "小鹏X3前后灯":
                    using (var frm = new FrmXiaoPengX3())
                        frm.ShowDialog();
                    break;

                case "MDF光型测试工具":
                    using (var frm = new FrmMdfTest())
                        frm.ShowDialog();
                    break;

                case "HVSM-LoadBox":
                    using (var frm = new HelperForms.Hvsm.FrmLoadBox())
                        frm.ShowDialog();
                    break;

                case "DX1H-LoadBox":
                    using (var frm = new HelperForms.GeeleyDx1h.FrmLoadBox())
                        frm.ShowDialog();
                    break;

                case "相机测试":
                    using (var frm = new FormCameraTest())
                        frm.ShowDialog();
                    break;

                case "SL-Display":
                    using (var frm = new HelperForms.SlDisplay.MyWndSlDisplay())
                        frm.ShowDialog();
                    break;

                case "AUDI427前灯":
                    using (var frm = new HelperForms.AUDI427HeadLamp.MyWnd427Ctrl())
                        frm.ShowDialog();
                    break;

                case "CCD追溯":
                    using (var frm = new CcdForms.FormCcdTrack())
                        frm.ShowDialog();
                    break;

                case "无线充数据查询":
                    using (var frm = new FrmDataRead())
                        frm.ShowDialog();
                    break;

                case "循环扫风控制系统":
                    using (var frm = new MyWndLoadBox())
                        frm.ShowDialog();
                    break;

                case "F03尾灯":
                    using (var frm = new WndF03RearLamp())
                        frm.ShowDialog();
                    break;

                case "无线充读取追溯":
                    using (var frm = new WirelessReadData())
                        frm.ShowDialog();
                    break;

                default:
                    return;
            }

            Environment.Exit(0);
        }
    }
}
