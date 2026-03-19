//using CheckSystem.AlcmAutoDevice;
using CheckSystem.CAN;
using CheckSystem.CcdForms;
using CheckSystem.EmulatorForms;
using CheckSystem.HelperForms;
using CheckSystem.HelperForms.AnalogMax25608;
using CheckSystem.HelperForms.Asl5115;
using CheckSystem.HelperForms.BD18331;
using CheckSystem.HelperForms.GeeleyRgbLampControl;
using CheckSystem.HelperForms.HalHac3980;
using CheckSystem.HelperForms.P12LHeadLamp;
using CheckSystem.HelperForms.Psi5Reader;
using CheckSystem.HelperForms.S12LHeadLamp;
using CheckSystem.HelperForms.Sgm3582RearLamp;
using CheckSystem.HelperForms.Sgm458Plg;
using CheckSystem.HelperForms.Tld7002;
using CheckSystem.HelperForms.U557HeadLamp;
using CheckSystem.HelperForms.VW323RearLamp;
using CheckSystem.HelperForms.VW336HeadLamp;
using CheckSystem.HelperForms.VW426RearLamp;
using CheckSystem.HelperForms.VW491RearLamp;
using CheckSystem.LIN;
using CheckSystem.MaterialHelperForms;
using CheckSystem.OpenCvSharp;
using CheckSystem.PesVision;
using CheckSystem.RobotForms;
using CheckSystem.SmtForms.DataUploader;
using CheckSystem.SmtForms.LaserCarving;
using CheckSystem.SyController;
using CheckSystem.SyRenesasMcuController;
using CheckSystem.UART;
using CheckSystem.VisionDetection;
using CheckSystem.VisionDetection.Calibration;
using CheckSystem.VisionDetection.Calibration.StaticVision;
using CheckSystem.VisionDetection.Vision;
using CommonUtility;
//using CheckSystem.Yfas;
//using CheckSystem.YfasLogo;
using CommonUtility.FileOperator;
using Go;
using HZH_Controls.IconFont;
using StateMachine;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FontImages = HZH_Controls.IconFont.FontImages;

namespace CheckSystem
{
    public partial class FormLoadXml : UIForm
    {
        public static control_strand MainStrand;
        private readonly List<DeviceConfig> _listDeviceConfig = new List<DeviceConfig>();
        private readonly List<string> _listFilePaths = new List<string>();
        //private string _searchLogFilePath = string.Format(@"{0}\ControllerConfig\XmlSearchLog.txt", Directory.GetCurrentDirectory());

        public FormLoadXml()
        {
            InitializeComponent();
            uiSplitContainer1.Collapse();
            Load += FormLoadXml_Load;

            var witdth = Screen.GetWorkingArea(this).Width;
            var height = Screen.GetWorkingArea(this).Height;

            if (witdth <= 1024 || height <= 768)
            {
                Size = MinimumSize = MaximumSize = new Size(1000, 600);
                lstXml.Font = new Font("黑体", 12F, FontStyle.Bold, GraphicsUnit.Point, 134);
            }
            //lstXml.ItemHeight = 30;
            //lstXml.DrawMode = DrawMode.OwnerDrawFixed;
            //lstXml.DrawItem += lstXml_DrawItem;

            Icon = FontImages.GetIcon(
                FontIcons.E_icon_documents, 32,
                Color.DodgerBlue);
            //InitXmls();
            //LoadXmls();
            ShowOnLineFile();
        }

        private void FormLoadXml_Load(object sender, EventArgs e)
        {
            MainStrand = new control_strand(this);

            var listTools = new List<string>
            {
                "CAN调试器",
                "LIN调试器",
                "56PIN控制器调试器",
                "瑞萨MCU控制器调试工具",
                "Uart调试器",
                "机器人控制器调试器",
                "DBC数据视图",
                "获取IP及MAC地址",
                "查看系统图标",
                "CCD参数设置",
                "视觉检测系统",
                "TLD7002刷写工具",
                "TPS929120刷写工具",
                "TPS92662点灯工具",
                "ASL5115点灯工具",
                "BD18331点灯工具",
                "ANALOG NAX25608点灯工具",
                "Geeley氛围灯",
                "P12L前灯",
                "S12L前灯",
                "VW336前灯",
                "VW323后灯",
                "VW323后灯-图莫斯",
                "VW323-0CS后灯",
                "VW323-0CS后灯-图莫斯",
                "VW323高低配点灯盒",
                "VW491后灯",
                "VW491后灯-图莫斯",
                "VW491后灯-瑞萨芯片单片机",
                "VW426后灯",
                "VW426后灯-图莫斯",
                "SGM358-2后灯",
                "仿真器Emulator",
                "SGM45PLG数据查询",
                "仓库电子料标签生成",
                "SAP入库",
                "延锋座椅模块",
                "延锋LOGO灯检测",
                "SMT激光打标",
                "SMT数据采集上传",
                "SGM458PDM-LoadBox",
                "GeeleyCX1E-LoadBox",
                "OpenCvSharp",
                "音乐氛围灯模块自动线终检",
                "PSI5_Reader",
                "PSI5_Programmer",
                "TEST_Camera",
                "TEST_DynamicVisionCali",
                "TEST_StaticVisionCali",
                "U557_EMC",
                "U557_EMC_双通道LIN",
                "HELA_MDF",
                "PES_Vision",
            };

            foreach (var label in listTools.Select(tool => new UIMarkLabel
            {
                Text = tool,
                Width = 250,
                Cursor = Cursors.Hand,
                MarkPos = UIMarkLabel.UIMarkPos.Bottom,
                Margin = new Padding(5),
                Font = new Font("微软雅黑", 12, FontStyle.Regular),
            }))
            {
                label.MouseHover += label_MouseHover;
                label.MouseLeave += label_MouseLeave;

                label.MouseDoubleClick += label_MouseDoubleClick;
                label.SetDPIScale();
                toolFlowPanel.Controls.Add(label);
            }

            var iniForm = Program.SysSetup.IniReadValue("System", "InitializeForm");

            var isContain = -1;
            if (!string.IsNullOrEmpty(iniForm) &&
                !string.Equals(iniForm, "default", StringComparison.CurrentCultureIgnoreCase))
            {
                for (var i = 0; i < listTools.Count; i++)
                {
                    if (!string.Equals(listTools[i], iniForm, StringComparison.CurrentCultureIgnoreCase))
                        continue;
                    isContain = i;
                    break;
                }
            }

            if (isContain != -1)
                OpenToolForm(iniForm);
        }

        private static void label_MouseLeave(object sender, EventArgs e)
        {
            var lbl = sender as UILabel;
            if (lbl == null) return;
            lbl.ForeColor = Color.Black;
            lbl.Font = new Font("微软雅黑", 12, FontStyle.Regular);
        }

        private static void label_MouseHover(object sender, EventArgs e)
        {
            var lbl = sender as UILabel;
            if (lbl == null) return;
            lbl.ForeColor = Color.Blue;
            lbl.Font = new Font("微软雅黑", 12, FontStyle.Bold);
        }

        private static void label_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var lbl = sender as UILabel;
            if (lbl != null)
            {
                OpenToolForm(lbl.Text);
            }
        }

        private static void OpenToolForm(string name)
        {
            switch (name)
            {
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

                case "TLD7002刷写工具":
                    using (var fm = new Tld7002DeviceMgr())
                        fm.ShowDialog();
                    break;

                case "TPS929120刷写工具":
                    using (var fm = new HelperForms.Tps929120.TpsFormDeviceMgr())
                        fm.ShowDialog();
                    break;

                case "TPS92662点灯工具":
                    using (var fm = new HelperForms.Tps92662.TpsFormDeviceMgr())
                        fm.ShowDialog();
                    break;

                case "ASL5115点灯工具":
                    using (var fm = new AslCanDeviceMgrForm())
                        fm.ShowDialog();
                    break;

                case "BD18331点灯工具":
                    using (var fm = new Bd18331CanDeviceMgrForm())
                        fm.ShowDialog();
                    break;

                case "ANALOG NAX25608点灯工具":
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
                    {
                        frm.ShowDialog();
                    }
                    break;

                case "S12L前灯":
                    using (var form = new S12LHeadLampDeviceMgr())
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
                //    {
                //        using (var form = new YfasMainForm())
                //            form.ShowDialog();
                //    }
                //    break;

                //case "延锋LOGO灯检测":
                //    using (var formLogo = new FrmYfasLogoCheckMain())
                //        formLogo.ShowDialog();
                //    //using (var formLogo = new FrmYfasLogoGrayConfig())
                //    //    formLogo.ShowDialog();
                //    break;

                case "视觉检测系统":
                    var loginFrm = new FrmVisionLogin();
                    if (loginFrm.ShowDialog() == DialogResult.OK)
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
            }

            Environment.Exit(0);
            //Application.Exit();
        }

        public override sealed Size MinimumSize
        {
            get { return base.MinimumSize; }
            set { base.MinimumSize = value; }
        }

        public override sealed Size MaximumSize
        {
            get { return base.MaximumSize; }
            set { base.MaximumSize = value; }
        }

        private void InitXmls(string folder)
        {
            cmbSearchText.Clear();

            //var folder = string.Format(@"{0}\流程配置文件", Program.SysDir);

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));

            foreach (
                var f in Directory.GetFiles(folder).Where(f => f.EndsWith(@".xml")))
            {
                try
                {
                    var deviceConfig = XmlHelper.Deserialize<DeviceConfig>(f);
                    dt.Rows.Add(deviceConfig.DeviceInfo.DeviceName);
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            cmbSearchText.DataGridView.ClearAll();
            cmbSearchText.DataGridView.Init();
            cmbSearchText.ItemSize = new Size(360, 240);
            cmbSearchText.DataGridView.AddColumn("Name", "Name");
            cmbSearchText.DataGridView.ReadOnly = true;
            cmbSearchText.ValueChanged += cmbSearchText_ValueChanged;
            cmbSearchText.ShowFilter = true;
            cmbSearchText.DataGridView.DataSource = dt;
            cmbSearchText.FilterColumnName = "Name"; //不设置则全部列过滤
        }

        private void LoadXmls(string folder)
        {
            lstXml.Items.Clear();
            _listFilePaths.Clear();

            //var folder = string.Format(@"{0}\流程配置文件", Program.SysDir);

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            if (!string.IsNullOrEmpty(cmbSearchText.Text))
            {
                foreach (
                var f in Directory.GetFiles(folder).Where(f => f.EndsWith(@".xml")))
                {
                    try
                    {
                        var deviceConfig = XmlHelper.Deserialize<DeviceConfig>(f);

                        if (!f.ToLower().Contains(cmbSearchText.Text.ToLower()) &&
                            !deviceConfig.DeviceInfo.DeviceName.ToLower()
                                .Contains(cmbSearchText.Text.ToLower())) continue;
                        _listDeviceConfig.Add(deviceConfig);
                        lstXml.Items.Add(string.Format("{0} 【路径：{1}】", deviceConfig.DeviceInfo.DeviceName, new FileInfo(f).Name));
                        _listFilePaths.Add(f);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
            else
            {
                foreach (
                var f in Directory.GetFiles(folder).Where(f => f.EndsWith(@".xml")))
                {
                    try
                    {
                        var deviceConfig = XmlHelper.Deserialize<DeviceConfig>(f);
                        _listDeviceConfig.Add(deviceConfig);
                        lstXml.Items.Add(string.Format("{0} 【路径：{1}】", deviceConfig.DeviceInfo.DeviceName, new FileInfo(f).Name));
                        _listFilePaths.Add(f);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }

            if (lstXml.Items.Count > 0)
                lstXml.SelectedIndex = 0;
        }

        private void cmbSearchText_ValueChanged(object sender, object value)
        {
            cmbSearchText.Text = string.Empty;
            var viewRow = value as DataGridViewRow;
            if (viewRow != null)
            {
                var row = viewRow;
                cmbSearchText.Text = row.Cells["Name"].Value.ToString();
            }

            //LoadXmls();
        }

        private void btnLoad_BtnClick(object sender, EventArgs e)
        {
            if (lstXml.SelectedIndex < 0)
                return;
            Hide();
            var setForm =
                new FormCheck(_listFilePaths[lstXml.SelectedIndex]);
            setForm.Show();
        }

        private void btnDelete_BtnClick(object sender, EventArgs e)
        {
            if (lstXml.SelectedIndex < 0)
                return;

            var pwd = string.Empty;
            if (this.InputPasswordDialog(ref pwd))
            {
                if (pwd.ToLower() != "098765")
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

            var file = _listFilePaths[lstXml.SelectedIndex];

            if (
                MessageBox.Show(string.Format(@"是否确认要删除文件【{0}】吗？", file), @"确认",
                    MessageBoxButtons.OKCancel) != DialogResult.OK)
                return;

            if (File.Exists(file))
                File.Delete(file);

            MessageBox.Show(string.Format(@"文件【{0}】已删除！", file));

            //LoadXmls();
        }

        private void btnEdit_BtnClick(object sender, EventArgs e)
        {
            if (lstXml.SelectedIndex < 0)
            {
                this.ShowErrorTip("文件缺失");
                return;
            }

            if (!File.Exists(_listFilePaths[lstXml.SelectedIndex]))
            {
                this.ShowErrorTip("未加载任何文件");
                return;
            }

            var pwd = string.Empty;
            if (this.InputPasswordDialog(ref pwd))
            {
                if (pwd.ToLower() != "098765")
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

            try
            {
                var dirPath = Program.SysDir;
                var xmlPath = _listFilePaths[lstXml.SelectedIndex];
                var controllerPath = dirPath + @"\Controller.dll";
                var userControlPath = dirPath + @"\UserControls.dll";
                using (var form = new DeviceDesign.FormMain(xmlPath, controllerPath, userControlPath))
                {
                    form.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"打开失败：" + ex.Message);
            }

            //LoadXmls();
        }

        private void btnAddCdd_BtnClick(object sender, EventArgs e)
        {
            var optionNewPr = new UIEditOption { Text = "添加产品" };
            optionNewPr.AddText("PrName", "产品名称：", null, true);
            optionNewPr.AddText("PrNo1", "项目编号1:", null, true);
            optionNewPr.AddText("PrNo2", "项目编号2:", null, true);

            var leftOrRightList = new[] { "区分", "不区分" };
            optionNewPr.AddCombobox("LeftOrRight", "是否区分左右", leftOrRightList, 0);

            var frmNewPr = new UIEditForm(optionNewPr);
            frmNewPr.CheckedData += frmNewPr_CheckedData;
            frmNewPr.Render();
            frmNewPr.ShowDialog();

            if (frmNewPr.IsOK)
            {
                var prName = frmNewPr["PrName"].ToString();
                var prNo = frmNewPr["PrNo1"].ToString();
                var prNo2 = frmNewPr["PrNo2"].ToString();
                var leftOrRight = leftOrRightList[(int)frmNewPr["LeftOrRight"]];

                VisionCommon.VisionConfig = VisionCommon.CreateNewVisionConfig(prName, prNo, prNo2, leftOrRight);
                VisionCommon.FilePath += VisionCommon.VisionConfig.DeviceInfo.DeviceName + ".ccd";
                XmlHelper.SerializeToFile(VisionCommon.VisionConfig, VisionCommon.FilePath, Encoding.UTF8);

                var frm = new FrmDetectionMainConfig();
                frm.ShowDialog();
            }
        }

        private bool frmNewPr_CheckedData(object sender, UIEditForm.EditFormEventArgs e)
        {
            var prName = e.Form["PrName"].ToString();
            var prNo = e.Form["PrNo1"].ToString();
            var prNo2 = e.Form["PrNo2"].ToString();

            if (!Directory.Exists(VisionCommon.FilePath))
                Directory.CreateDirectory(VisionCommon.FilePath);
            foreach (
                var ccd in Directory.GetFiles(VisionCommon.FilePath).Where(f => f.EndsWith(@".ccd")))
            {
                try
                {
                    var deviceConfig = XmlHelper.Deserialize<VisionConfig>(ccd);

                    if (deviceConfig.DeviceInfo.DeviceName == prName || ccd.Replace(".ccd", "") == prName)
                    {
                        this.ShowErrorTip("名称重复");
                        return false;
                    }

                    if (prName.ToLower().Contains("-L".ToLower()) || prName.ToLower().Contains("-R".ToLower()))
                    {
                        this.ShowErrorTip("名称中不允许出现-L或-R");
                        return false;
                    }

                    if (!prNo.StartsWith("PR") || !prNo2.StartsWith("PR"))
                    {
                        this.ShowErrorTip("请输入以PR开头的项目编号");
                        return false;
                    }

                    if (prNo.Length == 2 || prNo.Length == 2)
                    {
                        this.ShowErrorTip("输入的项目编号格式不正确");
                        return false;
                    }

                    if (prNo.Length != prNo2.Length)
                    {
                        this.ShowErrorTip("项目编号1与项目编号2长度不一致");
                        return false;
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            return true;
        }

        private void btnAddNormal_BtnClick(object sender, EventArgs e)
        {
        }

        private void lstXml_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lstXml.SelectedIndex < 0)
                return;

            Hide();
            var setForm =
                new FormCheck(_listFilePaths[lstXml.SelectedIndex]);
            setForm.Show();
        }

        private void 打开配置文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dir = Program.SysDir + @"\流程配置文件";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            System.Diagnostics.Process.Start(dir);
        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //InitXmls();
            //LoadXmls();
        }

        private void lstXml_SizeChanged(object sender, EventArgs e)
        {
            lstXml.ItemHeight = 25;
        }

        private void btnShowOffLineFile_Click(object sender, EventArgs e)
        {
            ShowOffLineFile();
        }

        private void btnShowOnLineFile_Click(object sender, EventArgs e)
        {
            ShowOnLineFile();
        }

        private void ShowOnLineFile()
        {
            string workOrderNo;
            string pnNo;
            string filePath;
            string fileName;
            var errorCode = string.Empty;
            if (CommonUtility.SynchronizationParameters.GetWorkOrderNoByApi(
                    Program.DeviceNo, out workOrderNo, out pnNo, out filePath,out fileName,ref errorCode))
            {
                try
                {
                    var driveLetter = Path.GetPathRoot(Program.SysDir);
                    var wordOrderTemp = string.Format(@"{0}\{1}", driveLetter, "WorkOrderFileTemp");
                    if (!Directory.Exists(wordOrderTemp))
                        Directory.CreateDirectory(wordOrderTemp);

                    try
                    {
                        // 使用DirectoryInfo类获取目录信息
                        var directoryInfo = new DirectoryInfo(wordOrderTemp);

                        // 获取所有文件信息并排序
                        var files = directoryInfo.GetDirectories()
                            .OrderBy(f => f.CreationTime) // 按创建日期升序排序
                            .ToArray(); // 或者使用OrderByDescending(f => f.CreationTime)进行降序排序

                        if (files.Any() && files.Length > 5)
                        {
                            var firstFile = files[0];
                            firstFile.Delete(true);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    var tempFolder = string.Format(@"{0}\{1}_{2}_{3}", wordOrderTemp, pnNo, workOrderNo,
                        HighPrecisionTimer.GetTimestamp());

                    var srcFileInfo = new FileInfo(filePath);
                    var tempFilePath = string.Format(@"{0}\{1}", tempFolder, fileName);

                    Directory.CreateDirectory(tempFolder);
                    File.Copy(filePath, tempFilePath, true);

                    InitXmls(tempFolder);
                    LoadXmls(tempFolder);

                    ShowSuccessTip(string.Format("获取工单成功：\r\n工单={0}；\r\n工序={1}；\r\n文件名={2}", workOrderNo, pnNo, srcFileInfo.Name.Substring(0, srcFileInfo.Name.Length - srcFileInfo.Extension.Length)));
                }
                catch (Exception e)
                {
                    ShowErrorTip(string.Format("获取工单失败：{0}", e.Message));
                }
            }
            else
            {
                ShowErrorTip("获取工单失败！");
            }
        }

        private void ShowOffLineFile()
        {
            DateTime startTime;
            DateTime endTime;
            if (DateTime.TryParse(CommonUtility.DEncrypt.DEncrypt.Decrypt(Program.OffLineStartTime), out startTime) &&
                DateTime.TryParse(CommonUtility.DEncrypt.DEncrypt.Decrypt(Program.OffLineEndTime), out endTime))
            {
                var currentTime = DateTime.Now;
                if (currentTime >= startTime && currentTime <= endTime)
                {
                    this.ShowSuccessTip(string.Format("已显示离线文件，离线文件可用时间段{0}~{1}", startTime, endTime));
                }
                else
                {
                    this.ShowInfoTip("该设备当前时段不允许使用离线文件，请输入管理密码进行确认");
                    if (ShowOffLineAuthority(out startTime, out endTime))
                    {
                        var str1 = startTime.ToString(CultureInfo.InvariantCulture);
                        str1 = CommonUtility.DEncrypt.DEncrypt.Encrypt(str1);

                        var str2 = endTime.ToString(CultureInfo.InvariantCulture);
                        str2 = CommonUtility.DEncrypt.DEncrypt.Encrypt(str2);

                        Program.OffLineStartTime = str1;
                        Program.OffLineEndTime = str2;

                        Program.SysSetup.IniWriteValue("System", "OffLineStartTime", str1);
                        Program.SysSetup.IniWriteValue("System", "OffLineEndTime", str2);

                        this.ShowSuccessTip(string.Format("管理员权限获取成功，离线文件可用时间段{0}~{1}", startTime, endTime));
                    }
                    else
                    {
                        this.ShowErrorTip("管理员权限获取失败，无法加载本地文件");
                    }
                }
            }
            else
            {
                this.ShowInfoTip("该设备当前时段不允许使用离线文件，请输入管理密码进行确认");
                if (ShowOffLineAuthority(out startTime, out endTime))
                {
                    var str1 = startTime.ToString(CultureInfo.InvariantCulture);
                    str1 = CommonUtility.DEncrypt.DEncrypt.Encrypt(str1);

                    var str2 = endTime.ToString(CultureInfo.InvariantCulture);
                    str2 = CommonUtility.DEncrypt.DEncrypt.Encrypt(str2);

                    Program.OffLineStartTime = str1;
                    Program.OffLineEndTime = str2;

                    Program.SysSetup.IniWriteValue("System", "OffLineStartTime", str1);
                    Program.SysSetup.IniWriteValue("System", "OffLineEndTime", str2);

                    this.ShowSuccessTip(string.Format("管理员权限获取成功，离线文件可用时间段{0}~{1}", startTime, endTime));
                }
                else
                {
                    this.ShowErrorTip("管理员权限获取失败，无法加载本地文件");
                }
            }
        }

        private bool ShowOffLineAuthority(out DateTime startTime, out DateTime endTime)
        {
            startTime = DateTime.Parse(string.Format("{0} 00:00:00", DateTime.Now.ToString("yyyy/MM/dd")));
            endTime = DateTime.Parse(string.Format("{0} 23:59:59", DateTime.Now.ToString("yyyy/MM/dd")));

            var pwd = string.Empty;
            if (this.InputPasswordDialog(ref pwd))
            {
                if (pwd.ToLower() != "098765")
                {
                    this.ShowInfoTip("密码输入错误");
                    return false;
                }
            }
            else
            {
                this.ShowInfoTip("取消密码输入");
                return false;
            }

            return true;
        }
    }
}