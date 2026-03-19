using CheckSystem.HelperForms.Tps92662;
using CommonUtility.BusLoader;
using Controller;
using HZH_Controls.IconFont;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FontImages = HZH_Controls.IconFont.FontImages;

namespace CheckSystem.HelperForms.Asl5115
{
    public partial class AslFormLedControl : Form
    {
        private readonly List<LedControlWithPwm> _ledtList =
             new List<LedControlWithPwm>();

        //protected readonly SyControllerWith56Pin Gw1 = new SyControllerWith56Pin("gw1");
        protected readonly CanBus Can;

        public static MatrixChipAsl5115 MatrixAsl5115 =
            new MatrixChipAsl5115("MATRIX-ASL5115");

        public AslFormLedControl(CanBus can)
        {
            InitializeComponent();
            Closed += AslFormLedControl_FormClosed;
            Icon = FontImages.GetIcon(FontIcons.A_fa_plug, 32,
               Color.DodgerBlue);
            if (can != null)
            {
                Can = can;

                for (var i = 0; i < 16; i++)
                    cmbAddr.Items.Add(Convert.ToString(i, 2).PadLeft(5, '0'));
                cmbAddr.SelectedIndex = 0;
                cmbAddr.SelectedIndexChanged += cmbAddr_SelectedIndexChanged;

                for (var i = 0; i < 12; i++)
                {
                    var led = new LedControlWithPwm(string.Format("LED{0}", i + 1), i)
                    {
                        Dock = DockStyle.Fill,
                        Enabled = false,
                    };
                    led.btnOnOff.Click += btnOnOff_Click;
                    led.phaseTrackBar.Scroll += led_Scroll;
                    led.widthTrackBar.Scroll += led_Scroll;
                    led.txtWidthValue.TextChanged += txtValue_TextChanged;
                    led.txtPhaseValue.TextChanged += txtValue_TextChanged;
                    _ledtList.Add(led);
                }

                ledTableLayout.Controls.Add(_ledtList[11], 0, 1);
                ledTableLayout.Controls.Add(_ledtList[10], 1, 1);
                ledTableLayout.Controls.Add(_ledtList[9], 2, 1);
                ledTableLayout.Controls.Add(_ledtList[8], 3, 1);
                ledTableLayout.Controls.Add(_ledtList[7], 4, 1);
                ledTableLayout.Controls.Add(_ledtList[6], 5, 1);
                ledTableLayout.Controls.Add(_ledtList[5], 0, 2);
                ledTableLayout.Controls.Add(_ledtList[4], 1, 2);
                ledTableLayout.Controls.Add(_ledtList[3], 2, 2);
                ledTableLayout.Controls.Add(_ledtList[2], 3, 2);
                ledTableLayout.Controls.Add(_ledtList[1], 4, 2);
                ledTableLayout.Controls.Add(_ledtList[0], 5, 2);

                //const string canGwIpPort = "192.168.1.28:8088";
                //const string canGwIpPort = "127.0.0.1:8088";
                Text += string.Format(" 358开关模块 控制器：{0}", can.Name);
                //try
                //{

                //    IPAddress.Parse(canGwIpPort.Split(':')[0]);
                //    int.Parse(canGwIpPort.Split(':')[1]);
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.Message);
                //    return;
                //}

                //Gw1.InitRemoteIpAddress(canGwIpPort);
                //if (Gw1.GatwayCan1 == null)
                //{
                //    MessageBox.Show(@"请先打开单片机电源");
                //    return;
                //}
                //Gw1.UpdateCurrsAndVolts();
                //Console.WriteLine(Gw1.Current1);
                //Gw1.InitRemoteIpAddress("127.0.0.1:8088");

                MatrixAsl5115.Can = Can;
            }
            else
            {
                MessageBox.Show(@"请先打开单片机电源");
            }
        }

        private void AslFormLedControl_FormClosed(object sender, EventArgs e)
        {
            MatrixAsl5115?.Dispose();
        }

        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            led_Scroll(null, null);
        }

        public sealed override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private void btnOnOff_Click(object sender, EventArgs e)
        {
            led_Scroll(null, null);

            var btn = sender as UISwitch;
            if (btn != null)
            {
                var index = btn.Text.Replace("LED", string.Empty)
                    .Replace(@"/ON", string.Empty)
                    .Replace(@"/OFF", string.Empty);

                if (index == 12.ToString())
                {
                    index = 1.ToString();
                }
                else if (index == 11.ToString())
                {
                    index = 2.ToString();
                }
                else if (index == 10.ToString())
                {
                    index = 3.ToString();
                }
                else if (index == 9.ToString())
                {
                    index = 4.ToString();
                }
                else if (index == 8.ToString())
                {
                    index = 5.ToString();
                }
                else if (index == 7.ToString())
                {
                    index = 6.ToString();
                }
                else if (index == 6.ToString())
                {
                    index = 7.ToString();
                }
                else if (index == 5.ToString())
                {
                    index = 8.ToString();
                }
                else if (index == 4.ToString())
                {
                    index = 9.ToString();
                }
                else if (index == 3.ToString())
                {
                    index = 10.ToString();
                }
                else if (index == 2.ToString())
                {
                    index = 11.ToString();
                }
                else if (index == 1.ToString())
                {
                    index = 12.ToString();
                }

                MatrixAsl5115.SwitchLed(int.Parse(index));
            }
        }

        private void led_Scroll(object sender, EventArgs e)
        {
            foreach (var t in _ledtList)
            {
                t.txtPhaseValue.Text = t.phaseTrackBar.Value.ToString();
                t.txtWidthValue.Text = t.widthTrackBar.Value.ToString();
            }
        }

        private void btnLMP_Click(object sender, EventArgs e)
        {
            foreach (var t in _ledtList.Where(t => t.btnOnOff.Text.Contains("ON")))
            {
                t.txtPhaseValue.Text = @"0";
                t.txtWidthValue.Text = @"0";
                t.btnOnOff.Text = t.btnOnOff.Text.Replace("ON", "OFF");
                t.btnOnOff.BackColor = Color.DarkGoldenrod;
            }

            foreach (var t in _ledtList)
                t.Enabled = false;
            btnMtp.Enabled = false;

            MatrixAsl5115.ExitMormalMode();
        }

        private void btnNormal_Click(object sender, EventArgs e)
        {
            MatrixAsl5115.EnterMormalMode();

            foreach (var t in _ledtList)
                t.Enabled = true;
            btnMtp.Enabled = true;
        }

        private void cmbAddr_SelectedIndexChanged(object sender, EventArgs e)
        {
            MatrixAsl5115.InitLightType(cmbAddr.Text);
        }

        private void btnMtp_Click(object sender, EventArgs e)
        {
            using (var mtpForm = new AslFormMtp())
                mtpForm.ShowDialog();
        }
    }
}
