using Controller;
using HZH_Controls.IconFont;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using FontImages = HZH_Controls.IconFont.FontImages;

namespace CheckSystem.HelperForms.Tps92662
{
    public partial class TpsFormLedControl : Form
    {
        private readonly List<LedControlWithPwm> _ledtList =
           new List<LedControlWithPwm>();

        private readonly RichTextBox _txtHz;
        protected readonly ControllerBase Gw1;
        private readonly MatrixTps92662With3Q1 _matrixTps92662With3Q1 =
            new MatrixTps92662With3Q1("MATRIX-TPS92662");

        public TpsFormLedControl(ControllerBase gw)
        {
            InitializeComponent();
            Closed += TpsFormLedControl_Closed;
            Icon = FontImages.GetIcon(FontIcons.A_fa_plug, 32,
               Color.DodgerBlue);

            if (gw != null)
            {
                Gw1 = gw;

                for (var i = 0; i < 12; i++)
                {
                    var led = new LedControlWithPwm(string.Format("LED{0}", i + 1), i)
                    {
                        Dock = DockStyle.Fill,
                    };
                    //led.btnOnOff.Click += btnOnOff_Click;
                    //led.phaseTrackBar.Scroll += led_Scroll;
                    //led.widthTrackBar.Scroll += led_Scroll;
                    //led.txtWidthValue.TextChanged += txtValue_TextChanged;
                    //led.txtPhaseValue.TextChanged += txtValue_TextChanged;
                    //led.lstPercent.SelectedIndexChanged += lstPercent_SelectedIndexChanged;
                    led.WidthValueChanged += Led_WidthValueChanged;
                    _ledtList.Add(led);
                }

                //ledTableLayout.Controls.Add(_ledtList[11], 5, 2);
                //ledTableLayout.Controls.Add(_ledtList[10], 4, 2);
                //ledTableLayout.Controls.Add(_ledtList[9], 3, 2);
                //ledTableLayout.Controls.Add(_ledtList[8], 2, 2);
                //ledTableLayout.Controls.Add(_ledtList[7], 1, 2);
                //ledTableLayout.Controls.Add(_ledtList[6], 0, 2);
                //ledTableLayout.Controls.Add(_ledtList[5], 5, 1);
                //ledTableLayout.Controls.Add(_ledtList[4], 4, 1);
                //ledTableLayout.Controls.Add(_ledtList[3], 3, 1);
                //ledTableLayout.Controls.Add(_ledtList[2], 2, 1);
                //ledTableLayout.Controls.Add(_ledtList[1], 1, 1);
                //ledTableLayout.Controls.Add(_ledtList[0], 0, 1);

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

                _txtHz = new RichTextBox
                {
                    Text = 500.ToString(),
                    Dock = DockStyle.Fill,
                    Font = new Font("黑体", 15)
                };
                ledTableLayout.Controls.Add(_txtHz, 1, 0);
                ledTableLayout.SetColumnSpan(_txtHz, 2);
                ledTableLayout.SetRowSpan(_txtHz, 1);

                //const string canGwIpPort = "192.168.1.28:8088";
                //var canGwIpPort = gw.RemoteIpPort;//"127.0.0.1:8088";
                Text += string.Format("addr：广播形式");

                //Gw1.UpdateCurrsAndVolts();
                //Console.WriteLine(Gw1.Current1);
                //Gw1.InitRemoteIpAddress("127.0.0.1:8088");
                _matrixTps92662With3Q1.MySerialPort = Gw1.GetType() == typeof(SyControllerWith56Pin) ? ((SyControllerWith56Pin)Gw1).GatewaySci2 : ((SyRenesasMcuControllerMaster)Gw1).UartCan;
                _matrixTps92662With3Q1.Init("FFF");
                _matrixTps92662With3Q1.SysConfig();
            }
        }

        private void Led_WidthValueChanged(object sender, LedControlWithPwm.WidthValueChangeActionEventArgs e)
        {
            var index = (e.Index + 1).ToString();

            Console.WriteLine("index = {0}, led = {1}", e.Index, index);

            var value = e.Value;

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

            if (value > 0)
            {
                _matrixTps92662With3Q1.LedOn(index);
            }
            else
            {
                _matrixTps92662With3Q1.LedOff(index);
            }
        }

        private void TpsFormLedControl_Closed(object sender, EventArgs e)
        {
            if (Gw1 != null)
                Gw1.Dispose();

            if (_matrixTps92662With3Q1 != null)
                _matrixTps92662With3Q1.Dispose();
        }

        private void lstPercent_SelectedIndexChanged(object sender, EventArgs e)
        {
            //for (var i = 0; i < _ledtList.Count; i++)
            //{
            //    var per = int.Parse(_ledtList[i].lstPercent.Text.TrimEnd('%'));
            //    var width = 1023 * per * 0.01;
            //    _ledtList[i].widthTrackBar.Value = (int)width;
            //}
            led_Scroll(null, null);
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
            if (btn != null && btn.Text.Contains("OFF"))
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

                _matrixTps92662With3Q1.LedOff(index);
            }
        }

        private void led_Scroll(object sender, EventArgs e)
        {
            for (var i = 0; i < _ledtList.Count; i++)
            {
                _ledtList[i].txtPhaseValue.Text = _ledtList[i].phaseTrackBar.Value.ToString();
                _ledtList[i].txtWidthValue.Text = _ledtList[i].widthTrackBar.Value.ToString();
                if (_ledtList[i].btnOnOff.Text.Contains("ON"))
                {
                    _matrixTps92662With3Q1.LedOnWithPhaseAndWidth((i + 1).ToString(), (ushort)_ledtList[i].phaseTrackBar.Value,
                    (ushort)_ledtList[i].widthTrackBar.Value);
                }

                //var per = (int)((float)_ledtList[i].widthTrackBar.Value / 1023 * 100);
                //for (var j = 0; j < _ledtList[i].lstPercent.Items.Count; j++)
                //{
                //    if (_ledtList[i].lstPercent.Items[j].ToString().TrimEnd('%') != per.ToString())
                //        continue;
                //    _ledtList[i].lstPercent.SelectedIndex = j;
                //    break;
                //}
            }
        }

        private void btnChangeHz_Click_1(object sender, EventArgs e)
        {
            try
            {
                var hz = float.Parse(_txtHz.Text.Trim(' '));

                if (_matrixTps92662With3Q1.ChangeHz(hz))
                {
                    _matrixTps92662With3Q1.SysConfig();
                    //label3.Text = hz.ToString(CultureInfo.InvariantCulture);
                }
                else
                    MessageBox.Show(@"切换频率失败，请更换频率");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnReadB0B1_Click(object sender, EventArgs e)
        {
            label3.Text = string.Empty;

            var option = new UIEditOption();
            option.AddCombobox("addr", "addr", new string[] { "0", "1", "2", "3", "4", "5", "6", "7" }, 0);

            var frm = new UIEditForm(option);
            frm.Render();
            frm.ShowDialog();

            if (!frm.IsOK)
            {
                UIMessageTip.Show("操作已取消");
                return;
            }

            var ch = (int)frm["addr"];
            _matrixTps92662With3Q1.ReadReg0XB0_0XB1_0XB2(ch);
            label3.Text = string.Format("{0},{1}", _matrixTps92662With3Q1.REG_0xB0, _matrixTps92662With3Q1.REG_0xB1);
        }
    }
}
