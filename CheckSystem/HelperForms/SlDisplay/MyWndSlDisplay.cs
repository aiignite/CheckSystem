using Controller;
using Sunny.UI;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace CheckSystem.HelperForms.SlDisplay
{
    public partial class MyWndSlDisplay : UIForm
    {
        private SmartGlassDisplay _smartGlassDisplay = new SmartGlassDisplay("SL-Display");
        private SyRenesasMcuControllerMaster syRenesasMcuControllerMaster = new SyRenesasMcuControllerMaster("CANFD");

        public MyWndSlDisplay()
        {
            Width = (int)(Screen.PrimaryScreen.WorkingArea.Width * 1.0);
            Height = Screen.PrimaryScreen.WorkingArea.Height;
            Size = MinimumSize = MaximumSize = new System.Drawing.Size(Width, Height);
            WindowState = FormWindowState.Maximized;
            InitializeComponent();
            Load += MyWndSlDisplay_Load;
        }

        private void MyWndSlDisplay_Load(object sender, System.EventArgs e)
        {
            try
            {
                syRenesasMcuControllerMaster.InitRemoteIpAddress("192.168.1.28:8088");

                if (!syRenesasMcuControllerMaster.IsConnected)
                {
                    UIMessageDialog.ShowErrorDialog(this, "CAN连接失败");
                    return;
                }

                _smartGlassDisplay.CanFd = syRenesasMcuControllerMaster.GatewayCan1;
                _smartGlassDisplay.StartCan();

                #region 信号

                cmbLamp_ExtrnlTailLmpOnReq.SelectedIndex = 0;
                cmbLamp_HdLmpWlcmCmd.SelectedIndex = 0;
                cmbLamp_HdLmpEscortShortCmd.SelectedIndex = 0;
                cmbLamp_AutoLtSnsrNightSta.SelectedIndex = 0;
                cmbCF_AVN_IFSActVehSpd_Set.SelectedIndex = 0;
                cmbCF_AVN_DWL_SelectNvalueSet.SelectedIndex = 0;
                cmbWarn_DrvDrSwSta.SelectedIndex = 0;
                cmbWarn_AsstDrSwSta.SelectedIndex = 0;
                cmbWarn_RrLftDrSwSta.SelectedIndex = 0;
                cmbWarn_RrRtDrSwSta.SelectedIndex = 0;

                swCanMsg.ActiveChanged += SwCanMsg_ActiveChanged;

                cmbLamp_ExtrnlTailLmpOnReq.SelectedIndexChanged += CmbLamp_ExtrnlTailLmpOnReq_SelectedIndexChanged;
                cmbLamp_HdLmpWlcmCmd.SelectedIndexChanged += CmbLamp_HdLmpWlcmCmd_SelectedIndexChanged;
                cmbLamp_HdLmpEscortShortCmd.SelectedIndexChanged += CmbLamp_HdLmpEscortShortCmd_SelectedIndexChanged;
                cmbLamp_AutoLtSnsrNightSta.SelectedIndexChanged += CmbLamp_AutoLtSnsrNightSta_SelectedIndexChanged;
                cmbCF_AVN_IFSActVehSpd_Set.SelectedIndexChanged += CmbCF_AVN_IFSActVehSpd_Set_SelectedIndexChanged;
                cmbCF_AVN_DWL_SelectNvalueSet.SelectedIndexChanged += CmbCF_AVN_DWL_SelectNvalueSet_SelectedIndexChanged;
                cmbWarn_DrvDrSwSta.SelectedIndexChanged += CmbWarn_DrvDrSwSta_SelectedIndexChanged;
                cmbWarn_AsstDrSwSta.SelectedIndexChanged += CmbWarn_AsstDrSwSta_SelectedIndexChanged;
                cmbWarn_RrLftDrSwSta.SelectedIndexChanged += CmbWarn_RrLftDrSwSta_SelectedIndexChanged;
                cmbWarn_RrRtDrSwSta.SelectedIndexChanged += CmbWarn_RrRtDrSwSta_SelectedIndexChanged;

                numWHL_SpdFLVal.ValueChanged += NumWHL_SpdFLVal_ValueChanged;
                numWHL_SpdFRVal.ValueChanged += NumWHL_SpdFRVal_ValueChanged;
                numWHL_SpdRLVal.ValueChanged += NumWHL_SpdRLVal_ValueChanged;
                numWHL_SpdRRVal.ValueChanged += NumWHL_SpdRRVal_ValueChanged;

                #endregion‘’

                #region 功能

                rbStandard.CheckedChanged += RbStandard_CheckedChanged;
                rbSpringFestival.CheckedChanged += RbSpringFestival_CheckedChanged;
                rbMidAutumn.CheckedChanged += RbMidAutumn_CheckedChanged;
                swWelcome.ActiveChanged += SwWelcome_ActiveChanged;
                swPosLamp.ActiveChanged += SwPosLamp_ActiveChanged;

                swDoorLh.ActiveChanged += SwDoorLh_ActiveChanged;
                swDoorRh.ActiveChanged += SwDoorRh_ActiveChanged;

                turnSwLightSensor.ActiveChanged += TurnSwLightSensor_ActiveChanged;
                rbChargingWithSoc.CheckedChanged += RbChargingWithSoc_CheckedChanged;
                rbChargingWithoutSoc.CheckedChanged += RbChargingWithoutSoc_CheckedChanged;

                #endregion

                swPosLamp.Active = true;
                turnSwLightSensor.Active = true;
                cmbCF_AVN_DWL_SelectNvalueSet.SelectedIndex = 1;
            }
            catch (System.Exception)
            {
                UIMessageDialog.ShowErrorDialog(this, "CAN连接失败");
            }
        }

        #region 信号

        private void NumWHL_SpdRRVal_ValueChanged(object sender, System.EventArgs e) => _smartGlassDisplay.Set_WHL_SpdRRVal((double)numWHL_SpdRRVal.Value);

        private void NumWHL_SpdRLVal_ValueChanged(object sender, System.EventArgs e) => _smartGlassDisplay.Set_WHL_SpdRLVal((double)numWHL_SpdRLVal.Value);

        private void NumWHL_SpdFRVal_ValueChanged(object sender, System.EventArgs e) => _smartGlassDisplay.Set_WHL_SpdFRVal((double)numWHL_SpdFRVal.Value);

        private void NumWHL_SpdFLVal_ValueChanged(object sender, System.EventArgs e) => _smartGlassDisplay.Set_WHL_SpdFLVal((double)numWHL_SpdFLVal.Value);

        private void SwCanMsg_ActiveChanged(object sender, System.EventArgs e)
        {
            if (swCanMsg.Active)
                _smartGlassDisplay.StartCan();
            else
                _smartGlassDisplay.StopCan();
        }

        private void CmbWarn_RrRtDrSwSta_SelectedIndexChanged(object sender, System.EventArgs e) => _smartGlassDisplay.Set_Warn_RrRtDrSwSta((byte)(cmbWarn_RrRtDrSwSta.SelectedIndex));

        private void CmbWarn_RrLftDrSwSta_SelectedIndexChanged(object sender, System.EventArgs e) => _smartGlassDisplay.Set_Warn_RrLftDrSwSta((byte)(cmbWarn_RrLftDrSwSta.SelectedIndex));

        private void CmbWarn_AsstDrSwSta_SelectedIndexChanged(object sender, System.EventArgs e) => _smartGlassDisplay.Set_Warn_AsstDrSwSta((byte)(cmbWarn_AsstDrSwSta.SelectedIndex));

        private void CmbWarn_DrvDrSwSta_SelectedIndexChanged(object sender, System.EventArgs e) => _smartGlassDisplay.Set_Warn_DrvDrSwSta((byte)(cmbWarn_DrvDrSwSta.SelectedIndex));

        private void CmbCF_AVN_DWL_SelectNvalueSet_SelectedIndexChanged(object sender, System.EventArgs e) => _smartGlassDisplay.Set_CF_AVN_DWL_SelectNvalueSet((byte)(cmbCF_AVN_DWL_SelectNvalueSet.SelectedIndex));

        private void CmbCF_AVN_IFSActVehSpd_Set_SelectedIndexChanged(object sender, System.EventArgs e) => _smartGlassDisplay.Set_CF_AVN_IFSActVehSpd_Set((byte)(cmbCF_AVN_IFSActVehSpd_Set.SelectedIndex));

        private void CmbLamp_AutoLtSnsrNightSta_SelectedIndexChanged(object sender, System.EventArgs e) => _smartGlassDisplay.Set_Lamp_AutoLtSnsrNightSta((byte)(cmbLamp_AutoLtSnsrNightSta.SelectedIndex));

        private void CmbLamp_HdLmpEscortShortCmd_SelectedIndexChanged(object sender, System.EventArgs e) => _smartGlassDisplay.Set_Lamp_HdLmpEscortShortCmd((byte)(cmbLamp_HdLmpEscortShortCmd.SelectedIndex));

        private void CmbLamp_HdLmpWlcmCmd_SelectedIndexChanged(object sender, System.EventArgs e) => _smartGlassDisplay.Set_Lamp_HdLmpWlcmCmd((byte)(cmbLamp_HdLmpWlcmCmd.SelectedIndex));

        private void CmbLamp_ExtrnlTailLmpOnReq_SelectedIndexChanged(object sender, System.EventArgs e) => _smartGlassDisplay.Set_Lamp_ExtrnlTailLmpOnReq((byte)(cmbLamp_ExtrnlTailLmpOnReq.SelectedIndex));

        #endregion

        #region 功能

        private void RbMidAutumn_CheckedChanged(object sender, System.EventArgs e)
        {
            if (rbMidAutumn.Checked)
            {
                cmbCF_AVN_DWL_SelectNvalueSet.SelectedIndex = 3;
                txtNowMode.Text = @"中秋模式";
            }
        }

        private void RbSpringFestival_CheckedChanged(object sender, System.EventArgs e)
        {
            if (rbSpringFestival.Checked)
            {
                cmbCF_AVN_DWL_SelectNvalueSet.SelectedIndex = 2;
                txtNowMode.Text = @"春节模式";
            }
        }

        private void RbStandard_CheckedChanged(object sender, System.EventArgs e)
        {
            if (rbStandard.Checked)
            {
                cmbCF_AVN_DWL_SelectNvalueSet.SelectedIndex = 1;
                txtNowMode.Text = @"标准模式";
            }
        }

        private void SwPosLamp_ActiveChanged(object sender, System.EventArgs e) => cmbLamp_ExtrnlTailLmpOnReq.SelectedIndex = swPosLamp.Active ? 1 : 0;

        private void SwWelcome_ActiveChanged(object sender, System.EventArgs e) => cmbLamp_HdLmpWlcmCmd.SelectedIndex = swWelcome.Active ? 1 : 0;

        private void SwDoorRh_ActiveChanged(object sender, System.EventArgs e)
        {
            cmbWarn_AsstDrSwSta.SelectedIndex = swDoorRh.Active ? 1 : 0;
            cmbWarn_RrRtDrSwSta.SelectedIndex = swDoorRh.Active ? 1 : 0;
        }

        private void SwDoorLh_ActiveChanged(object sender, System.EventArgs e)
        {
            cmbWarn_DrvDrSwSta.SelectedIndex = swDoorLh.Active ? 1 : 0;
            cmbWarn_RrLftDrSwSta.SelectedIndex = swDoorLh.Active ? 1 : 0;
        }

        private void TurnSwLightSensor_ActiveChanged(object sender, System.EventArgs e)
        {
            cmbLamp_AutoLtSnsrNightSta.SelectedIndex = turnSwLightSensor.Active ? 1 : 0;
        }

        private void RbChargingWithoutSoc_CheckedChanged(object sender, System.EventArgs e)
        {
            if (rbChargingWithoutSoc.Checked)
            {
                cmbCF_AVN_DWL_SelectNvalueSet.SelectedIndex = 9;
                txtNowMode.Text = @"充电模式不显示电量";
            }
        }

        private void RbChargingWithSoc_CheckedChanged(object sender, System.EventArgs e)
        {
            if (rbChargingWithSoc.Checked)
            {
                cmbCF_AVN_DWL_SelectNvalueSet.SelectedIndex = 5;
                txtNowMode.Text = @"充电模式显示电量";
            }
        }

        #endregion
    }
}
