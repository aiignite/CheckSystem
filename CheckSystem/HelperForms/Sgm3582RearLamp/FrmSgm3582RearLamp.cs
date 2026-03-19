using Controller;
using Go;
using Sunny.UI;
using System;

namespace CheckSystem.HelperForms.Sgm3582RearLamp
{
    public partial class FrmSgm3582RearLamp : UIForm
    {
        private readonly SyControllerWith56Pin _controller = new SyControllerWith56Pin("LIN");
        //private readonly SyRenesasMcuControllerMaster _controller = new SyRenesasMcuControllerMaster("LIN");
        private readonly Controller.Sgm3582RearLamp _rearLamp = new Controller.Sgm3582RearLamp("SGM358-2");
        private generator _action;

        public FrmSgm3582RearLamp()
        {
            InitializeComponent();
            Load += FrmSgm3582RearLamp_Load;
            Closed += FrmSgm3582RearLamp_Closed;
        }

        private void FrmSgm3582RearLamp_Closed(object sender, EventArgs e)
        {
            _action.stop();

            if (_rearLamp != null)
                _rearLamp.Dispose();
            if (_controller != null)
                _controller.Dispose();

            Environment.Exit(0);
        }

        private void FrmSgm3582RearLamp_Load(object sender, EventArgs e)
        {
            _controller.InitRemoteIpAddress("192.168.1.29:8088");
            _rearLamp.LinWithBaudRate10417 = _controller.GatewayLin;
            //_controller.InitRemoteIpAddress("192.168.1.28:8088");
            //_rearLamp.LinWithBaudRate10417 = _controller.GatewayLin1;
            _rearLamp.IsAllControl = true;
            _rearLamp.LampAwake();

            rbTailOff.CheckedChangeEvent += Tail_CheckedChangeEvent;
            rbTailNormal.CheckedChangeEvent += Tail_CheckedChangeEvent;
            rbTailHighLightOn.CheckedChangeEvent += Tail_CheckedChangeEvent;

            rbTurnOff.CheckedChangeEvent += Turn_CheckedChangeEvent;
            rbTurnHighLightOn.CheckedChangeEvent += Turn_CheckedChangeEvent;
            rbTurnHoldOnLight.CheckedChangeEvent += Turn_CheckedChangeEvent;
            rbTurnSwipeTurn.CheckedChangeEvent += Turn_CheckedChangeEvent;

            rbLampShowAbort.CheckedChangeEvent += Animation_CheckedChangeEvent;
            rbSimpleComingHome.CheckedChangeEvent += Animation_CheckedChangeEvent;
            rbSimpleLeavingHome.CheckedChangeEvent += Animation_CheckedChangeEvent;
            rbComplexComingHome.CheckedChangeEvent += Animation_CheckedChangeEvent;
            rbComplexLeavingHome.CheckedChangeEvent += Animation_CheckedChangeEvent;

            UpdateErrorInfo();
        }

        private void Animation_CheckedChangeEvent(object sender, EventArgs e)
        {
            if (rbLampShowAbort.Checked)
            {
                _rearLamp.LampShowAbort();
                rbTailOff.Checked = true;
                gpTail.Enabled = true;
            }
            else
            {
                rbTailOff.Checked = true;
                gpTail.Enabled = false;

                if (rbComplexLeavingHome.Checked)
                    _rearLamp.ComplexLeavingHome();
                else if (rbComplexComingHome.Checked)
                    _rearLamp.ComplexComingHome();
                else if (rbSimpleComingHome.Checked)
                    _rearLamp.SimpleComingHome();
                else if (rbSimpleLeavingHome.Checked)
                    _rearLamp.SimpleLeavingHome();
            }
        }

        private void Turn_CheckedChangeEvent(object sender, EventArgs e)
        {
            if (rbTurnOff.Checked)
            {
                _rearLamp.TurnNotActive();
                swIsTurnOnOff.Active = false;
            }
            else if (rbTurnHighLightOn.Checked)
            {
                _rearLamp.TurnHighLightOn();
            }
            else if (rbTurnHoldOnLight.Checked)
            {
                _rearLamp.TurnHoldOnLight();
            }
            else if (rbTurnSwipeTurn.Checked)
            {
                _rearLamp.TurnSwipeTurn();
            }
        }

        private void Tail_CheckedChangeEvent(object sender, System.EventArgs e)
        {
            if (rbTailOff.Checked)
                _rearLamp.TailNoAction();
            else if (rbTailNormal.Checked)
                _rearLamp.TailNormalOn();
            else if (rbTailHighLightOn.Checked)
                _rearLamp.TailHighLightOn();
        }

        private void uiSwitch1_ActiveChanged(object sender, EventArgs e)
        {
            if (uiSwitch1.Active)
                _rearLamp.LampAwake();
            else
                _rearLamp.LampSleep();
        }

        private void UpdateErrorInfo()
        {
            _action = generator.tgo(FormSelection.MainStrand, async delegate
            {
                while (true)
                {
                    try
                    {
                        await generator.sleep(20);

                        lblLeftATurn.Text = _rearLamp.左侧A灯转向灯;
                        lblLeftAStop.Text = _rearLamp.左侧A灯制动灯;
                        lblLeftATail.Text = _rearLamp.左侧A灯位置灯;

                        lblRightATurn.Text = _rearLamp.右侧A灯转向灯;
                        lblRightAStop.Text = _rearLamp.右侧A灯制动灯;
                        lblRightATail.Text = _rearLamp.右侧A灯位置灯;

                        lblLeftBTurn.Text = _rearLamp.左侧B灯转向灯;
                        lblLeftBTail.Text = _rearLamp.左侧B灯位置灯;

                        lblRightBTurn.Text = _rearLamp.右侧B灯转向灯;
                        lblRightBTail.Text = _rearLamp.右侧B灯位置灯;
                    }
                    catch (AccessViolationException exception)
                    {
                        Console.WriteLine(@"0xC0000005 exception:" + exception.ToString());
                    }
                }
                // ReSharper disable once FunctionNeverReturns
            });
        }

        private void swIsTurnOnOff_ActiveChanged(object sender, EventArgs e)
        {
            if (swIsTurnOnOff.Active)
            {
                _rearLamp.StartSwitchTurn();
            }
            else
            {
                _rearLamp.StopSwitchTurn();
            }
        }
    }
}
