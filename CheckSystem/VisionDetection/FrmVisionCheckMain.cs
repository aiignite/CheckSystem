using CheckSystem.VisionDetection.Calibration;
using CheckSystem.VisionDetection.Vision;
using CommonUtility.FileOperator;
using CommonUtility.HikSdk;
using HZH_Controls.IconFont;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckSystem.VisionDetection
{
    public partial class FrmVisionCheckMain : UIHeaderMainFooterFrame
    {
        private FrmVisionCheckTestView FrmVisionCheckTestView { get; set; }
        private bool IsLoadConfigOk { get; set; }

        public FrmVisionCheckMain()
        {
            InitializeComponent();
            UIStyles.InitColorful(Color.FromArgb(80, 126, 164), Color.White);
            Icon = HZH_Controls.IconFont.FontImages.GetIcon(FontIcons.A_fa_low_vision, 32, Color.DodgerBlue);
            //uiLabel2.Text += VisionCommon.LoginUser;
            uiLabel2.Text += VisionCommon.VisionDeviceConfig.DeviceInfo.DeviceNo;

            ShowSelectPrFrm(string.Empty);
            Load += FrmVisionCheckMain_Load;
            Closed += FrmVisionCheckMain_Closed;
        }

        public FrmVisionCheckMain(string ccdConfigFilePath)
        {
            InitializeComponent();
            UIStyles.InitColorful(Color.FromArgb(80, 126, 164), Color.White);
            Icon = HZH_Controls.IconFont.FontImages.GetIcon(FontIcons.A_fa_low_vision, 32, Color.DodgerBlue);
            //uiLabel2.Text += VisionCommon.LoginUser;
            uiLabel2.Text += VisionCommon.VisionDeviceConfig.DeviceInfo.DeviceNo;

            ShowSelectPrFrm(ccdConfigFilePath);
            Load += FrmVisionCheckMain_Load;
            Closed += FrmVisionCheckMain_Closed;
        }

        private void ShowSelectPrFrm(string filePath = "")
        {
            if (string.IsNullOrEmpty(filePath))
            {
                var optionPr = new UIEditOption { Text = "请选择产品" };
                if (!Directory.Exists(VisionCommon.FilePath))
                    Directory.CreateDirectory(VisionCommon.FilePath);
                var listCcds = new List<string>();
                foreach (
                    var ccd in Directory.GetFiles(VisionCommon.FilePath).Where(f => f.EndsWith(@".ccd")))
                {
                    try
                    {
                        var deviceConfig = XmlHelper.Deserialize<VisionConfig>(ccd);
                        listCcds.Add(new FileInfo(ccd).Name.Replace(".ccd", ""));
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
                listCcds.Add("新增配置......");
                listCcds.Add("硬件配置......");

                optionPr.AddCombobox("CcdFile", "请选择文件", listCcds.ToArray(), 0);

                if (!listCcds.Any())
                {
                    this.ShowErrorDialog("无产品文件，无法启动");
                }
                else
                {
                    var frmPrs = new UIEditForm(optionPr);
                    frmPrs.Render();
                    frmPrs.ShowDialog();
                    if (frmPrs.IsOK)
                    {
                        var fileName = listCcds[(int)frmPrs["CcdFile"]];
                        if (fileName != "新增配置......" && fileName != "硬件配置......")
                        {
                            VisionCommon.FilePath += listCcds[(int)frmPrs["CcdFile"]] + ".ccd";
                            VisionCommon.VisionConfig = XmlHelper.Deserialize<VisionConfig>(VisionCommon.FilePath);
                            uiLblTitle.Text = VisionCommon.VisionConfig.DeviceInfo.DeviceName;

                            if (VisionCommon.VisionConfig.DeviceInfo.DeviceNo2 == null ||
                                string.IsNullOrEmpty(VisionCommon.VisionConfig.DeviceInfo.DeviceNo2))
                            {
                                VisionCommon.VisionConfig.DeviceInfo.DeviceNo2 =
                                    VisionCommon.VisionConfig.DeviceInfo.DeviceNo;
                            }

                            foreach (
                                var item in
                                    VisionCommon.VisionConfig.ParaInfo.ToList()
                                        .FindAll(f => f.ParaType.Contains("静态图像"))
                                        .Select(t => new ToolStripMenuItem { Text = t.ParaName }))
                            {
                                item.Click += 静态图像标定ToolStripMenuItem_Click;
                                静态图像标定ToolStripMenuItem.DropDownItems.Add(item);
                            }

                            foreach (
                                var item in
                                    VisionCommon.VisionConfig.ParaInfo.ToList()
                                        .FindAll(f => f.ParaType.Contains("动态图像"))
                                        .Select(t => new ToolStripMenuItem { Text = t.ParaName }))
                            {
                                item.Click += 动态图像标定ToolStripMenuItem_Click;
                                动态图像标定ToolStripMenuItem.DropDownItems.Add(item);
                            }

                            var frm = new FrmGroupSelect();
                            if (frm.ShowDialog() == DialogResult.OK)
                            {
                                IsLoadConfigOk = true;
                            }
                            else
                            {
                                IsLoadConfigOk = false;
                                this.ShowErrorDialog("档位信息未选择，无法启动");
                            }
                        }
                        else if (fileName == "新增配置......") // add new
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
                                uiLblTitle.Text = VisionCommon.VisionConfig.DeviceInfo.DeviceName;
                                IsLoadConfigOk = true;
                            }
                            else
                            {
                                IsLoadConfigOk = false;
                                this.ShowErrorDialog("产品未选择，无法启动");
                            }
                        }
                        else if (fileName == "硬件配置......") // device config
                        {
                            var frmDeviceConfig = new FrmDeviceConfig();
                            frmDeviceConfig.ShowDialog();
                        }
                    }
                    else
                    {
                        IsLoadConfigOk = false;
                        this.ShowErrorDialog("产品未选择，无法启动");
                    }
                }
            }
            else
            {
                VisionCommon.FilePath = filePath;
                VisionCommon.VisionConfig = XmlHelper.Deserialize<VisionConfig>(VisionCommon.FilePath);
                uiLblTitle.Text = VisionCommon.VisionConfig.DeviceInfo.DeviceName;

                if (VisionCommon.VisionConfig.DeviceInfo.DeviceNo2 == null ||
                    string.IsNullOrEmpty(VisionCommon.VisionConfig.DeviceInfo.DeviceNo2))
                {
                    VisionCommon.VisionConfig.DeviceInfo.DeviceNo2 =
                        VisionCommon.VisionConfig.DeviceInfo.DeviceNo;
                }

                foreach (
                    var item in
                    VisionCommon.VisionConfig.ParaInfo.ToList()
                        .FindAll(f => f.ParaType.Contains("静态图像"))
                        .Select(t => new ToolStripMenuItem { Text = t.ParaName }))
                {
                    item.Click += 静态图像标定ToolStripMenuItem_Click;
                    静态图像标定ToolStripMenuItem.DropDownItems.Add(item);
                }

                foreach (
                    var item in
                    VisionCommon.VisionConfig.ParaInfo.ToList()
                        .FindAll(f => f.ParaType.Contains("动态图像"))
                        .Select(t => new ToolStripMenuItem { Text = t.ParaName }))
                {
                    item.Click += 动态图像标定ToolStripMenuItem_Click;
                    动态图像标定ToolStripMenuItem.DropDownItems.Add(item);
                }

                var frm = new FrmGroupSelect();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    IsLoadConfigOk = true;
                }
                else
                {
                    IsLoadConfigOk = false;
                    this.ShowErrorDialog("档位信息未选择，无法启动");
                }
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

        private void FrmVisionCheckMain_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;

            if (IsLoadConfigOk)
            {
                LoadCamera();

                if (VisionCommon.VisionConfig.DeviceInfo.DeviceLeftOrRight == null ||
                    string.Equals(VisionCommon.VisionConfig.DeviceInfo.DeviceLeftOrRight, @"区分",
                        StringComparison.CurrentCultureIgnoreCase))
                {
                    if (VisionCommon.IsLeft)
                        uiLblTitle.Text += @"（L）";
                    else
                        uiLblTitle.Text += @"（R）";
                }

                VisionCommon.Title = uiLblTitle.Text;

                FrmVisionCheckTestView = new FrmVisionCheckTestView(true);
                AddPage(FrmVisionCheckTestView, 1001);
            }
            else
            {
                FrmVisionCheckTestView = new FrmVisionCheckTestView(false);
                AddPage(FrmVisionCheckTestView, 1001);
            }
        }

        private void FrmVisionCheckMain_Closed(object sender, EventArgs e)
        {
            if (VisionCommon.NiImaqd == null)
            {
                Environment.Exit(0);
            }

            VisionCommon.NiImaqd.CloseAllCamera();
            Environment.Exit(0);
        }

        private async void LoadCamera()
        {
            await Task.Run(() =>
            {
                var cameraTreeNode = new TreeNode { Text = @"相机列表" };

                VisionCommon.NiImaqd = new CameraControl();
                VisionCommon.NiImaqd.DeviceListAcq();
                VisionCommon.NiImaqd.OpenAllCamera();

                for (var i = 0; i < VisionCommon.NiImaqd.CameraList.Count; i++)
                    cameraTreeNode.Nodes.Add(new TreeNode { Text = string.Format("相机{0}：{1}", i + 1, VisionCommon.NiImaqd.CameraList[i].GigeInfo.chSerialNumber) });

                VisionCommon.NiImaqdLoadOk = true;
            });
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Footer.Text =
                string.Format("当前时间：{0} {1}", DateTime.Now.ToString(CultureInfo.InvariantCulture), GetWeekZhCn());
        }

        private static string GetWeekZhCn()
        {
            switch (DateTime.Now.DayOfWeek.ToString())
            {
                case "Monday":
                    return "星期一";

                case "Tuesday":
                    return "星期二";

                case "Wednesday":
                    return "星期三";

                case "Thursday":
                    return "星期四";

                case "Friday":
                    return "星期五";

                case "Saturday":
                    return "星期六";

                default:
                    return "星期日";
            }
        }

        private void 参数设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (VisionCommon.VisionConfig != null)
            {
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

                var frm = new FrmDetectionMainConfig();
                frm.ShowDialog();
            }
            else
            {
                this.ShowErrorTip("当前未载入参数配置文件");
            }
        }

        private async void 静态图像标定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                var control = sender as ToolStripMenuItem;
                if (control != null && control.Text != @"静态图像标定")
                {
                    if (FrmVisionCheckTestView.ToRunVisionConfig != null && FrmVisionCheckTestView.ToRunVisionConfig.ParaInfo != null)
                    {
                        if (FrmVisionCheckTestView.ToRunVisionConfig.DeviceInfo != null && FrmVisionCheckTestView.ToRunVisionConfig.DeviceInfo.Actions != null && FrmVisionCheckTestView.ToRunVisionConfig.DeviceInfo.Actions.PropulsionCylinder != null)
                        {
                            try
                            {
                                if (string.Equals(FrmVisionCheckTestView.ToRunVisionConfig.DeviceInfo.Actions.PropulsionCylinder.IsPropulsionCylinder, "true", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    VisionCommon.InvokeRelays(new VisionConfigParaParaReleysList { ParaReleysOffList = string.Empty, ParaReleysOnList = FrmVisionCheckTestView.ToRunVisionConfig.DeviceInfo.Actions.PropulsionCylinder.Binding });
                                    Thread.Sleep(int.Parse(FrmVisionCheckTestView.ToRunVisionConfig.DeviceInfo.Actions.PropulsionCylinder.Delay));
                                }
                            }
                            catch (Exception a)
                            {
                                Console.WriteLine(a);
                            }
                        }
                    }

                    var frm = new FrmStaticVisionParaConfig(control.Text, VisionCommon.IsLeft);
                    frm.ShowDialog();

                    if (FrmVisionCheckTestView != null)
                        FrmVisionCheckTestView.UpdateVisionPara(VisionCommon.VisionConfig);

                    if (FrmVisionCheckTestView.ToRunVisionConfig != null && FrmVisionCheckTestView.ToRunVisionConfig.DeviceInfo != null &&
                        FrmVisionCheckTestView.ToRunVisionConfig.DeviceInfo.Actions != null &&
                        FrmVisionCheckTestView.ToRunVisionConfig.DeviceInfo.Actions.PropulsionCylinder != null)
                    {
                        try
                        {
                            if (string.Equals(FrmVisionCheckTestView.ToRunVisionConfig.DeviceInfo.Actions.PropulsionCylinder.IsPropulsionCylinder, "true", StringComparison.CurrentCultureIgnoreCase))
                            {
                                VisionCommon.InvokeRelays(new VisionConfigParaParaReleysList
                                { ParaReleysOffList = string.Empty, ParaReleysOnList = FrmVisionCheckTestView.ToRunVisionConfig.DeviceInfo.Actions.PropulsionCylinder.Binding }, true);
                                Thread.Sleep(450);
                            }
                        }
                        catch (Exception a)
                        {
                            Console.WriteLine(a);
                        }
                    }
                }
            });
        }

        private async void 动态图像标定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                var control = sender as ToolStripMenuItem;
                if (control != null && control.Text != @"动态图像标定")
                {
                    if (FrmVisionCheckTestView.ToRunVisionConfig != null && FrmVisionCheckTestView.ToRunVisionConfig.ParaInfo != null)
                    {
                        if (FrmVisionCheckTestView.ToRunVisionConfig.DeviceInfo != null &&
                            FrmVisionCheckTestView.ToRunVisionConfig.DeviceInfo.Actions != null &&
                            FrmVisionCheckTestView.ToRunVisionConfig.DeviceInfo.Actions.PropulsionCylinder != null)
                        {
                            try
                            {
                                if (string.Equals(FrmVisionCheckTestView.ToRunVisionConfig.DeviceInfo.Actions.PropulsionCylinder.IsPropulsionCylinder, "true", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    VisionCommon.InvokeRelays(new VisionConfigParaParaReleysList { ParaReleysOffList = string.Empty, ParaReleysOnList = FrmVisionCheckTestView.ToRunVisionConfig.DeviceInfo.Actions.PropulsionCylinder.Binding });
                                    Thread.Sleep(int.Parse(FrmVisionCheckTestView.ToRunVisionConfig.DeviceInfo.Actions.PropulsionCylinder.Delay));
                                }
                            }
                            catch (Exception a)
                            {
                                Console.WriteLine(a);
                            }
                        }
                    }

                    var frm = new FrmDynamicVsionParaConfig(control.Text, VisionCommon.IsLeft);
                    frm.ShowDialog();
                    if (FrmVisionCheckTestView != null)
                        FrmVisionCheckTestView.UpdateVisionPara(VisionCommon.VisionConfig);

                    if (FrmVisionCheckTestView.ToRunVisionConfig != null && FrmVisionCheckTestView.ToRunVisionConfig.DeviceInfo != null &&
                        FrmVisionCheckTestView.ToRunVisionConfig.DeviceInfo.Actions != null && FrmVisionCheckTestView.ToRunVisionConfig.DeviceInfo.Actions.PropulsionCylinder != null)
                    {
                        try
                        {
                            if (string.Equals(FrmVisionCheckTestView.ToRunVisionConfig.DeviceInfo.Actions.PropulsionCylinder.IsPropulsionCylinder, "true", StringComparison.CurrentCultureIgnoreCase))
                            {
                                VisionCommon.InvokeRelays(new VisionConfigParaParaReleysList
                                { ParaReleysOffList = string.Empty, ParaReleysOnList = FrmVisionCheckTestView.ToRunVisionConfig.DeviceInfo.Actions.PropulsionCylinder.Binding }, true);
                                Thread.Sleep(450);
                            }
                        }
                        catch (Exception a)
                        {
                            Console.WriteLine(a);
                        }
                    }
                }
            });
        }

        private void tsbtnStart_Click(object sender, EventArgs e)
        {
            if (VisionCommon.VisionConfig != null)
            {
                int isBypass;
                var control = sender as ToolStripMenuItem;
                if (control != null && control.Text.EndsWith("NG后继续"))
                    isBypass = 0;
                else
                    isBypass = 1;
                FrmVisionCheckTestView.Start(isBypass);
            }
        }

        private void 档位选择ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new FrmGroupSelect();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (VisionCommon.VisionConfig.DeviceInfo.DeviceLeftOrRight == null ||
                    string.Equals(VisionCommon.VisionConfig.DeviceInfo.DeviceLeftOrRight, @"区分",
                        StringComparison.CurrentCultureIgnoreCase))
                {
                    uiLblTitle.Text = VisionCommon.VisionConfig.DeviceInfo.DeviceName;

                    if (VisionCommon.IsLeft)
                        uiLblTitle.Text += @"（L）";
                    else
                        uiLblTitle.Text += @"（R）";
                }

                VisionCommon.Title = uiLblTitle.Text;

                FrmVisionCheckTestView.InitUi();
                FrmVisionCheckTestView.ReInitMainDgv();
            }
        }

        private void btnCheckData_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmVisionCheckDataHistory())
            {
                frm.ShowDialog();
            }
        }

        private void 反复测试ToolStripMenuItem_Click(object sender, EventArgs e)
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

            if (反复测试ToolStripMenuItem.Text == @"反复测试开始")
            {
                FrmVisionCheckTestView.IsRepeatTest = true;
                反复测试ToolStripMenuItem.Text = @"反复测试结束";
                btnDeviceInfo.Enabled = false;
                tsbtnStartByPass.Enabled = false;
                tsbtnStartNoByPass.Enabled = false;
                档位选择ToolStripMenuItem.Enabled = false;
            }
            else
            {
                FrmVisionCheckTestView.IsRepeatTest = false;
                反复测试ToolStripMenuItem.Text = @"反复测试开始";
                btnDeviceInfo.Enabled = true;
                tsbtnStartByPass.Enabled = true;
                tsbtnStartNoByPass.Enabled = true;
                档位选择ToolStripMenuItem.Enabled = true;
            }
        }
    }
}
