using Sunny.UI;
using System;
using System.Windows.Controls;
using System.Windows.Forms;

namespace CheckSystem.HelperForms.F03RearLamp
{
    public partial class WndF03RearLamp : UIForm
    {
        private readonly Controller.F03RearLamp _f03RearLamp = new Controller.F03RearLamp("F03");
        private Controller.ControllerBase _canFDCtrl { get; set; }

        public WndF03RearLamp()
        {
            InitializeComponent();
            Load += WndF03RearLamp_Load;
            FormClosing += WndF03RearLamp_FormClosing;
        }

        private void WndF03RearLamp_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!UIMessageBox.ShowAsk("确定？"))
            {
                UIMessageTip.Show("取消关闭");
                e.Cancel = true;
                return;
            }

            try
            {
                _f03RearLamp?.Dispose();
                _canFDCtrl?.Dispose();
            }
            catch (Exception)
            {

            }
            finally
            {
                Environment.Exit(0);
            }
        }

        private void WndF03RearLamp_Load(object sender, EventArgs e)
        {
            InitUI();
            if (InitCtrl())
            {
                InitEvents();
                InitDefault();
            }
            else
            {
                UIMessageBox.ShowError("can通讯卡初始化失败");
            }
        }

        private bool InitCtrl()
        {
            var option = new UIEditOption();
            option.AddCombobox("type", "选择类型", new string[] { "瑞萨", "ZLG", "TOOMOSS" }, 0);

            var frm = new UIEditForm(option);
            frm.Render();
            frm.ShowDialog();

            var type = 0;

            if (!frm.IsOK)
            {
                UIMessageTip.Show("操作已取消，默认瑞萨");
            }
            else
            {
                type = (int)frm["type"];
            }

            try
            {
                if (type == 1)
                {
                    _canFDCtrl = new Controller.ZlgUsbCanFd200U("zlg");
                    _f03RearLamp.CanFD = ((Controller.ZlgUsbCanFd200U)_canFDCtrl).ZlgCanChannel0;
                }
                else if (type == 2)
                {
                    _canFDCtrl = new Controller.ToomossUsb2XxxCanLin("toomoss");
                    _f03RearLamp.CanFD = ((Controller.ToomossUsb2XxxCanLin)_canFDCtrl).Can1;
                }
                else
                {
                    _canFDCtrl = new Controller.SyRenesasMcuControllerMaster("renesas");
                    ((Controller.SyRenesasMcuControllerMaster)_canFDCtrl).InitRemoteIpAddress("192.168.1.28:8088");
                    _f03RearLamp.CanFD = ((Controller.SyRenesasMcuControllerMaster)_canFDCtrl).GatewayCan1;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private void InitUI()
        {
            foreach (var item in Controls)
            {
                if (item.GetType() == typeof(UIGroupBox))
                {
                    foreach (var item2 in ((UIGroupBox)item)?.Controls)
                    {
                        if (item2.GetType() == typeof(UIComboBox))
                        {
                            var cmb = (UIComboBox)item2;
                            if (cmb is null)
                                continue;
                            cmb.SelectedIndex = 0;
                        }
                    }
                }
                else if (item.GetType() == typeof(UIComboBox))
                {
                    var cmb = (UIComboBox)item;
                    if (cmb is null)
                        continue;
                    cmb.SelectedIndex = 0;
                }
            }
        }

        private void InitEvents()
        {
            swCanMsg.ActiveChanged += SwCanMsg_ActiveChanged;
            swTi400msSwitch.ActiveChanged += SwTi400msSwitch_ActiveChanged;

            // turn
            cmbLeftTurnCtrl.SelectedIndexChanged += (s, e) => _f03RearLamp?.LeftTiOnOff(((UIComboBox)s).SelectedIndex);
            cmbRightTurnCtrl.SelectedIndexChanged += (s, e) => _f03RearLamp?.RightTiOnOff(((UIComboBox)s).SelectedIndex);
            cmbLeftTurnFLow.SelectedIndexChanged += (s, e) => _f03RearLamp?.LeftTiFlow(((UIComboBox)s).SelectedIndex);
            cmbRightTurnFLow.SelectedIndexChanged += (s, e) => _f03RearLamp?.RightTiFlow(((UIComboBox)s).SelectedIndex);
            nudLeftTurnBrit.ValueChanged += (s, e) => _f03RearLamp?.LeftTiBrightSet((int)((NumericUpDown)s).Value);
            nudRightTurnBrit.ValueChanged += (s, e) => _f03RearLamp?.RightTiBrightSet((int)((NumericUpDown)s).Value);

            // stop
            cmbLeftStopCtrl.SelectedIndexChanged += (s, e) => _f03RearLamp?.LeftStopOnOff(((UIComboBox)s).SelectedIndex);
            cmbRightStopCtrl.SelectedIndexChanged += (s, e) => _f03RearLamp?.RightStopOnOff(((UIComboBox)s).SelectedIndex);
            cmbMidLeftStopCtrl.SelectedIndexChanged += (s, e) => _f03RearLamp?.MiddleLeftStopOnOff(((UIComboBox)s).SelectedIndex);
            cmbMidRightStopCtrl.SelectedIndexChanged += (s, e) => _f03RearLamp?.MiddleRightStopOnOff(((UIComboBox)s).SelectedIndex);

            // bul
            cmbMidLeftBul.SelectedIndexChanged += (s, e) => _f03RearLamp?.MiddleLeftBulOnOff(((UIComboBox)s).SelectedIndex);
            cmbMidRightBul.SelectedIndexChanged += (s, e) => _f03RearLamp?.MiddleRightBulOnOff(((UIComboBox)s).SelectedIndex);

            // ads
            cmbAdsCtrl.SelectedIndexChanged += (s, e) => _f03RearLamp?.RearADSOnOff(((UIComboBox)s).SelectedIndex);
            nudAdsBrit.ValueChanged += (s, e) => _f03RearLamp?.RearADSBritSet((int)((NumericUpDown)s).Value);
            cmbAdsEffectCtrl.SelectedIndexChanged += (s, e) => _f03RearLamp?.RearADSEffectCtrl(((UIComboBox)s).SelectedIndex);
            cmbAdsEffectCtrlSel.SelectedIndexChanged += (s, e) => _f03RearLamp?.RearADSEffectSelect(((UIComboBox)s).SelectedIndex);
            cmbAdsEffectMode.SelectedIndexChanged += (s, e) => _f03RearLamp?.RearADSEffectModeSet(((UIComboBox)s).SelectedIndex);

            // 贯穿灯
            cmbRibbonEffectCtrl.SelectedIndexChanged += (s, e) => _f03RearLamp?.RearRibbonEffectCtrl(((UIComboBox)s).SelectedIndex);
            cmbRibbonEffectCtrlSel.SelectedIndexChanged += (s, e) => _f03RearLamp?.RearRibbonEffectSelect(((UIComboBox)s).SelectedIndex);
            cmbRibbonEffectMode.SelectedIndexChanged += (s, e) => _f03RearLamp?.RearRibbonEffectModeSet(((UIComboBox)s).SelectedIndex);

            // 位置灯
            nudLeftRearPosBrit.ValueChanged += (s, e) => _f03RearLamp?.LeftRearPosBritSet((int)((NumericUpDown)s).Value);
            nudRightRearPosBrit.ValueChanged += (s, e) => _f03RearLamp?.RightRearPosBritSet((int)((NumericUpDown)s).Value);
            cmbLeftRearPosFlow.SelectedIndexChanged += (s, e) => _f03RearLamp?.LeftRearPosFlow(((UIComboBox)s).SelectedIndex);
            cmbRightRearPosFlow.SelectedIndexChanged += (s, e) => _f03RearLamp?.RightRearPosFlow(((UIComboBox)s).SelectedIndex);
            cmbLeftRearPosCtrl.SelectedIndexChanged += (s, e) => _f03RearLamp?.LeftRearPosCtrl(((UIComboBox)s).SelectedIndex);
            cmbRightRearPosCtrl.SelectedIndexChanged += (s, e) => _f03RearLamp?.RightRearPosCtrl(((UIComboBox)s).SelectedIndex);
            cmbMidLeftRearPosFlow.SelectedIndexChanged += (s, e) => _f03RearLamp?.MidLeftRearPosFlow(((UIComboBox)s).SelectedIndex);
            cmbMidRightRearPosFlow.SelectedIndexChanged += (s, e) => _f03RearLamp?.MidRightRearPosFlow(((UIComboBox)s).SelectedIndex);
            nudMidLeftRearPosBrit.ValueChanged += (s, e) => _f03RearLamp?.MidLeftRearPosBritSet((int)((NumericUpDown)s).Value);
            nudMidRightRearPosBrit.ValueChanged += (s, e) => _f03RearLamp?.MidRightRearPosBritSet((int)((NumericUpDown)s).Value);
            cmbMidLeftRearPosCtrl.SelectedIndexChanged += (s, e) => _f03RearLamp?.MidLeftRearPosCtrl(((UIComboBox)s).SelectedIndex);
            cmbMidRightRearPosCtrl.SelectedIndexChanged += (s, e) => _f03RearLamp?.MidRightRearPosCtrl(((UIComboBox)s).SelectedIndex);

            cmbTailGate.SelectedIndexChanged += (s, e) => _f03RearLamp?.TailGateCtrl(((UIComboBox)s).SelectedIndex);
            cmbAdsL3.SelectedIndexChanged += (s, e) => _f03RearLamp?.ADS_L3(((UIComboBox)s).SelectedIndex);
            cmbLogo.SelectedIndexChanged += (s, e) => _f03RearLamp?.RearLogoCtrl(((UIComboBox)s).SelectedIndex);
            cmb_viu_UsageMode.SelectedIndexChanged += (s, e) => _f03RearLamp?.VIU_UsageMode(((UIComboBox)s).SelectedIndex);
        }

        private void InitDefault()
        {
            foreach (var item in Controls)
            {
                if (item.GetType() == typeof(UIGroupBox))
                {
                    foreach (var item2 in ((UIGroupBox)item)?.Controls)
                    {
                        if (item2.GetType() == typeof(UIComboBox))
                        {
                            var cmb = (UIComboBox)item2;
                            if (cmb is null)
                                continue;

                            if (cmb.Items.Count >= 2 && (!cmb.Items[1].ToString().ToLower().StartsWith("effect") && !cmb.Items[1].ToString().ToLower().StartsWith("l3")))
                                cmb.SelectedIndex = 1;
                        }
                        else if (item2.GetType() == typeof(NumericUpDown))
                        {
                            var nud = (NumericUpDown)item2;
                            if (nud is null)
                                continue;

                            nud.Value = 101;
                        }
                    }
                }
                else if (item.GetType() == typeof(UIComboBox))
                {
                    var cmb = (UIComboBox)item;
                    if (cmb is null)
                        continue;
                    if (cmb.Items.Count >= 2 && (!cmb.Items[1].ToString().ToLower().StartsWith("effect") && !cmb.Items[1].ToString().ToLower().StartsWith("l3")))
                        cmb.SelectedIndex = 1;
                }
                else if (item.GetType() == typeof(NumericUpDown))
                {
                    var nud = (NumericUpDown)item;
                    if (nud is null)
                        continue;

                    nud.Value = 101;
                }
            }

            swCanMsg.Active = true;
        }

        private void SwTi400msSwitch_ActiveChanged(object sender, EventArgs e)
        {
            if (swTi400msSwitch.Active)
            {
                cmbLeftTurnCtrl.Enabled = cmbRightTurnCtrl.Enabled = false;
                cmbLeftTurnCtrl.SelectedIndex = 1;
                cmbRightTurnCtrl.SelectedIndex = 1;
                _bLastTiFlag = false;
                timer1.Enabled = true;
            }
            else
            {
                cmbLeftTurnCtrl.Enabled = cmbRightTurnCtrl.Enabled = true;
                _bLastTiFlag = false;
                timer1.Enabled = false;
            }
        }

        private void SwCanMsg_ActiveChanged(object sender, EventArgs e)
        {
            if (swCanMsg.Active)
                _f03RearLamp?.StartCAN();
            else
                _f03RearLamp?.StopCAN();
        }

        private bool _bLastTiFlag;

        private void timer1_Tick(object sender, EventArgs e)
        {
            var updateAct = new Action(() =>
            {
                if (_bLastTiFlag)
                {
                    cmbLeftTurnCtrl.SelectedIndex = 1;
                    cmbRightTurnCtrl.SelectedIndex = 1;
                }
                else
                {
                    cmbLeftTurnCtrl.SelectedIndex = 2;
                    cmbRightTurnCtrl.SelectedIndex = 2;
                }

                _bLastTiFlag = !_bLastTiFlag;
            });

            if (InvokeRequired)
                Invoke(updateAct);
            else
                updateAct();
        }

        private void readToolStripMenuItem_Click(object sender, EventArgs e) => ReadVer();

        private void btnReadVer_Click(object sender, EventArgs e) => ReadVer();

        private void ReadVer()
        {
            txtVer.Clear();
            txtVer.AppendText("正在读取..." + Environment.NewLine);

            _f03RearLamp.ReadEcuName();
            _f03RearLamp.ReadSeeYaoECUHwVer();
            _f03RearLamp.ReadSeeYaoECUSwVer();
            _f03RearLamp.ReadBootLoaderVer();
            _f03RearLamp.ReadFactoryECUSwVer();
            _f03RearLamp.ReadBackupSwVer();
            _f03RearLamp.ReadFactoryECUHwVer();
            _f03RearLamp.ReadFactoryPartNo();
            _f03RearLamp.ReadSupplierCode();
            _f03RearLamp.ReadThumbprint();
            _f03RearLamp.ReadRunningAreaInfo();
            _f03RearLamp.ReadUpdateTriedCounter();
            _f03RearLamp.ReadDependecyCheckSuccessCounter();
            _f03RearLamp.ReadFactorySwPartNo();

            txtVer.AppendText("R,ECU名称读取[F197]: " + _f03RearLamp.ECUName + Environment.NewLine);
            txtVer.AppendText("R,信耀定义ECU硬件版本号[F193]: " + _f03RearLamp.SeeYaoECUHwVer + Environment.NewLine);
            txtVer.AppendText("R,信耀定义ECU软件版本号[F195]: " + _f03RearLamp.SeeYaoECUSwVer + Environment.NewLine);
            txtVer.AppendText("R,BootLoader软件版本号[F180]: " + _f03RearLamp.BootLoaderVer + Environment.NewLine);
            txtVer.AppendText("R, 整车厂ECU软件版本号[F189]: " + _f03RearLamp.FactoryECUSwVer + Environment.NewLine);
            txtVer.AppendText("R,备份软件版本号[F090]: " + _f03RearLamp.BackupSwVer + Environment.NewLine);
            txtVer.AppendText("R,整车厂ECU硬件版本号[F089]: " + _f03RearLamp.FactoryECUHwVer + Environment.NewLine);
            txtVer.AppendText("R,整车厂定义零件号[F187]: " + _f03RearLamp.FactoryPartNo + Environment.NewLine);
            txtVer.AppendText("R,供应商代码[F18A]: " + _f03RearLamp.SupplierCode + Environment.NewLine);
            txtVer.AppendText("R,读取指纹信息[F184]: " + _f03RearLamp.Thumbprint + Environment.NewLine);
            txtVer.AppendText("R,当前运行分区信息[F0F0]: " + _f03RearLamp.RunningAreaInfo + Environment.NewLine);
            txtVer.AppendText("R,刷写尝试计数器[F0F1]: " + _f03RearLamp.UpdateTriedCounter + Environment.NewLine);
            txtVer.AppendText("R,编程依赖检查成功计数器[F0F3]: " + _f03RearLamp.DependecyCheckSuccessCounter + Environment.NewLine);
            txtVer.AppendText("R,整车厂定义软件零件号[F013]: " + _f03RearLamp.FactorySwPartNo + Environment.NewLine);

            txtVer.AppendText("读取结束!");

            txtVer.Focus();
            txtVer.Select(txtVer.TextLength, 0);
            txtVer.ScrollToCaret();
        }

        private void btnClearDtc_Click(object sender, EventArgs e)
        {
            txtVer.Clear();
            txtVer.AppendText("正在清除DTC..." + Environment.NewLine);

            _f03RearLamp.ClearDtc();
            txtVer.AppendText("R,清除DTC结果: " + _f03RearLamp.ClearDtcResult + Environment.NewLine);

            txtVer.AppendText("清除DTC结束!");

            txtVer.Focus();
            txtVer.Select(txtVer.TextLength, 0);
            txtVer.ScrollToCaret();
        }

        private void btnReadDtc_Click(object sender, EventArgs e)
        {
            txtVer.Clear();
            txtVer.AppendText("正在读取DTC..." + Environment.NewLine);

            _f03RearLamp.ReadDtc();
            txtVer.AppendText("R,读取DTC: " + _f03RearLamp.ReadDtcResult + Environment.NewLine);

            txtVer.AppendText("读取DTC结束!");

            txtVer.Focus();
            txtVer.Select(txtVer.TextLength, 0);
            txtVer.ScrollToCaret();
        }
    }
}
