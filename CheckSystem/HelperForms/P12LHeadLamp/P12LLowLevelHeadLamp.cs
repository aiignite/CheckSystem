using System;
using System.Drawing;
using CommonUtility.BusLoader;
using HZH_Controls.IconFont;
using Sunny.UI;
using FontImages = HZH_Controls.IconFont.FontImages;

namespace CheckSystem.HelperForms.P12LHeadLamp
{
    public partial class P12LLowLevelHeadLamp : UIForm
    {
        private readonly Controller.P12LLowLevelHeadLamp _controller = new Controller.P12LLowLevelHeadLamp("P12L低配前灯");

        public P12LLowLevelHeadLamp(LinBus lin6, LinBus lin5)
        {
            InitializeComponent();
            Icon = FontImages.GetIcon(FontIcons.E_icon_lightbulb, 32,
                 Color.DodgerBlue);
            Closed += P12LLowLevelHeadLamp_Closed;
            _controller.BcmLin5 = lin5;
            _controller.IccLin6 = lin6;

            uiSwitchCan.ActiveChanged += uiSwitchCan_ActiveChanged;
            uiSwitchLb.ActiveChanged += uiSwitchLb_ActiveChanged;
            uiSwitchHb.ActiveChanged += uiSwitchHb_ActiveChanged;
            uiSwitchDrl.ActiveChanged += uiSwitchDrl_ActiveChanged;
            uiSwitchWelcome.ActiveChanged += uiSwitchWelcome_ActiveChanged;
            uiIntegerUpDownPl.ValueChanged += uiIntegerUpDownPl_ValueChanged;
            uiSwitchDrlPlSwitch.ValueChanged += uiSwitchDrlPlSwitch_ValueChanged;

            uiSwitchTi.ActiveChanged += uiSwitchTi_ActiveChanged;
            uiRadioButtonTiRunning.CheckedChanged += uiRadioButtonTiRuning_CheckedChanged;
            uiRadioButtonTiFlash.CheckedChanged += uiRadioButtonTiFlash_CheckedChanged;
            uiSwitchTiOnOff.ActiveChanged += uiSwitchTiOnOff_ActiveChanged;

            btnHdlmpLvlngReq.ValueChanged += btnHdlmpLvlngReq_ValueChanged;
            uiSwitchMotor.ActiveChanged += uiSwitchMotor_ActiveChanged;
        }

        private void uiSwitchMotor_ActiveChanged(object sender, EventArgs e)
        {
            if (uiSwitchMotor.Active)
            {
                uiSwitchLb.Enabled = false;
                uiSwitchLb.Active = true;
                btnHdlmpLvlngReq.Enabled = false;
                btnHdlmpLvlngReq.Value = btnHdlmpLvlngReq.Value <= 1 ? 4 : 1;
                uiIntegerUpDownMotor.Enabled = false;
                uiMillisecondTimerMotor.Interval = uiIntegerUpDownMotor.Value;
                uiMillisecondTimerMotor.Start();
            }
            else
            {
                uiSwitchLb.Enabled = true;
                uiSwitchLb.Active = false;
                btnHdlmpLvlngReq.Enabled = true;
                uiIntegerUpDownMotor.Enabled = true;
                uiMillisecondTimerMotor.Stop();
            }
        }

        private void uiSwitchTiOnOff_ActiveChanged(object sender, EventArgs e)
        {
            if (uiSwitchTiOnOff.Active)
            {
                uiSwitchTi.Active = true;
                uiSwitchTi.Enabled = false;
                uiMillisecondTimerTi.Start();
            }
            else
            {
                uiSwitchTi.Active = false;
                uiSwitchTi.Enabled = true;
                uiMillisecondTimerTi.Stop();
            }
        }

        private void uiSwitchTi_ActiveChanged(object sender, EventArgs e)
        {
            _controller.Turn();
        }

        private void uiRadioButtonTiFlash_CheckedChanged(object sender, EventArgs e)
        {
            if (uiRadioButtonTiFlash.Checked)
                _controller.DiSucsLghtEnb = true;
        }

        private void uiRadioButtonTiRuning_CheckedChanged(object sender, EventArgs e)
        {
            if (uiRadioButtonTiRunning.Checked)
                _controller.DiSucsLghtEnb = false;
        }

        private void uiSwitchDrlPlSwitch_ValueChanged(object sender, bool value)
        {
            if (uiSwitchDrlPlSwitch.Active)
            {
                uiSwitchWelcome.Active = false;
                uiSwitchDrl.Active = false;
                uiIntegerUpDownPl.Value = 31;

                uiSwitchWelcome.Enabled = false;
                uiSwitchDrl.Enabled = false;
                uiIntegerUpDownPl.Enabled = false;

                uiMillisecondTimerDrlPl.Start();
            }
            else
            {
                uiSwitchWelcome.Active = false;
                uiSwitchDrl.Active = false;
                uiIntegerUpDownPl.Value = 0;

                uiSwitchWelcome.Enabled = true;
                uiSwitchDrl.Enabled = true;
                uiIntegerUpDownPl.Enabled = true;

                uiMillisecondTimerDrlPl.Stop();
            }
        }

        private void uiSwitchWelcome_ActiveChanged(object sender, EventArgs e)
        {
            _controller.WlcReq();
        }

        private void uiIntegerUpDownPl_ValueChanged(object sender, int value)
        {
            _controller.PlReq(value.ToString());
        }

        private void uiSwitchDrl_ActiveChanged(object sender, EventArgs e)
        {
            _controller.DrlReq();
        }

        private void uiSwitchHb_ActiveChanged(object sender, EventArgs e)
        {
            _controller.HighBeamReq();
        }

        private void uiSwitchLb_ActiveChanged(object sender, EventArgs e)
        {
            _controller.LowBeamReq();
        }

        private void uiSwitchCan_ActiveChanged(object sender, EventArgs e)
        {
            if (uiSwitchCan.Active)
                _controller.LampAwake();
            else
                _controller.LampSleep();
        }

        private void btnHdlmpLvlngReq_ValueChanged(object sender, int value)
        {
            _controller.HdlmpLvlngReq(value.ToString());
        }

        private void P12LLowLevelHeadLamp_Closed(object sender, EventArgs e)
        {
            if (_controller != null)
                _controller.Dispose();
        }

        private void uiMillisecondTimerDrlPl_Tick(object sender, EventArgs e)
        {
            if (uiSwitchDrl.Active)
            {
                uiSwitchDrl.Active = false;
                uiIntegerUpDownPl.Value = 31;
            }
            else
            {
                uiIntegerUpDownPl.Value = 0;
                uiSwitchDrl.Active = true;
            }
        }

        private void uiMillisecondTimerTi_Tick(object sender, EventArgs e)
        {
            uiSwitchTi.Active = !uiSwitchTi.Active;
        }

        private void uiMillisecondTimerMotor_Tick(object sender, EventArgs e)
        {
            Invoke(new Action(() =>
            {
                if (btnHdlmpLvlngReq.Value == 4)
                {
                    btnHdlmpLvlngReq.Value = 1;
                }
                else
                {
                    btnHdlmpLvlngReq.Value = 4;
                }
            }));
        }
    }
}
