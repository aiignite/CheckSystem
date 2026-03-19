using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Controller;
using HZH_Controls.IconFont;

namespace CheckSystem.HelperForms
{
    public partial class Pb06Guimt6501 : Form
    {
        private readonly Pb006Mt6501 _pb006Mt6501 = new Pb006Mt6501("Pb006Mt6501");
        private int _chipIndex = 0;

        public Pb06Guimt6501()
        {
            InitializeComponent();
            Icon = FontImages.GetIcon(FontIcons.A_fa_spoon, 32,
               Color.DodgerBlue);
            CheckForIllegalCrossThreadCalls = false;
            Closed += Pb06Guimt6501_Closed;

            txtClampHi.Text = @"90.00";
            txtClampLo.Text = @"10.00";
            txtAngle.textBox.ReadOnly = true;
            txtVdd.textBox.ReadOnly = true;
        }

        private void Pb06Guimt6501_Closed(object sender, EventArgs e)
        {
            _pb006Mt6501.Dispose();
            Environment.Exit(0);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (_pb006Mt6501.Connect(3) == 0)
            {
                btnConnect.Enabled = false;
                SwitchPower(false);
            }
        }

        private void btnPowerOn_Click(object sender, EventArgs e)
        {
            if (_pb006Mt6501.PownerOn(_chipIndex) == 0)
            {
                SwitchPower(true);
                _pb006Mt6501.EnterOwi(_chipIndex);
                _pb006Mt6501.GetVdd(_chipIndex);
                txtVdd.Text = _pb006Mt6501.VddVolt.ToString(CultureInfo.InvariantCulture);
            }
        }

        private void btnPowerOff_Click(object sender, EventArgs e)
        {
            _pb006Mt6501.PownerOff(_chipIndex);
            SwitchPower(false);
        }

        private void btnNewDevice_Click(object sender, EventArgs e)
        {
            _pb006Mt6501.NewDevice(_chipIndex);
        }

        private void btnReadAngle_Click(object sender, EventArgs e)
        {
            txtAngle.Text = _pb006Mt6501.GetAng(_chipIndex).ToString(CultureInfo.InvariantCulture);
        }

        private void SwitchPower(bool isOn)
        {
            if (isOn)
            {
                btnPowerOn.Enabled = false;
                btnPowerOff.Enabled = true;
                btnNewDevice.Enabled = true;

                groupBox1.Enabled = true;
                //groupBox2.Enabled = true;
                groupBox3.Enabled = true;

                btnProgram.Enabled = true;
            }
            else
            {
                btnPowerOn.Enabled = true;
                btnPowerOff.Enabled = false;
                btnNewDevice.Enabled = false;

                groupBox1.Enabled = false;
                //groupBox2.Enabled = false;
                groupBox3.Enabled = false;

                btnProgram.Enabled = false;
            }
        }

        private void btnGetDp_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAngle.textBox.Text))
            {
                float angle;
                if (float.TryParse(txtAngle.textBox.Text, out angle))
                {
                    if (angle - 180f >= 0)
                    {
                        txtDp.textBox.Text = (angle - 180f).ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        txtDp.textBox.Text = (angle + 180f).ToString(CultureInfo.InvariantCulture);
                    }
                }
            }
        }

        private void btnSetPointA_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAngle.textBox.Text))
            {
                float angle;
                if (float.TryParse(txtAngle.textBox.Text, out angle))
                {
                    txtAngleA.Text = angle.ToString(CultureInfo.InvariantCulture);
                }
            }
        }

        private void btnSetPointB_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAngle.textBox.Text))
            {
                float angle;
                if (float.TryParse(txtAngle.textBox.Text, out angle))
                {
                    txtAngleB.Text = angle.ToString(CultureInfo.InvariantCulture);
                }
            }
        }

        private void btnSetPointC_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAngle.textBox.Text))
            {
                float angle;
                if (float.TryParse(txtAngle.textBox.Text, out angle))
                {
                    txtAngleC.Text = angle.ToString(CultureInfo.InvariantCulture);
                }
            }
        }

        private void btnSetPointD_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAngle.textBox.Text))
            {
                float angle;
                if (float.TryParse(txtAngle.textBox.Text, out angle))
                {
                    txtAngleD.Text = angle.ToString(CultureInfo.InvariantCulture);
                }
            }
        }

        private void btnProgram_Click(object sender, EventArgs e)
        {
            try
            {
                _pb006Mt6501.SetClamp(_chipIndex, float.Parse(txtClampLo.Text), float.Parse(txtClampHi.Text));
                _pb006Mt6501.SetPoint(_chipIndex, "DP", float.Parse(txtDp.Text));

                _pb006Mt6501.SetPoint(_chipIndex, "A", float.Parse(txtAngleA.Text));
                _pb006Mt6501.SetLevel(_chipIndex, "A", float.Parse(txtLevelA.Text));

                _pb006Mt6501.SetPoint(_chipIndex, "B", float.Parse(txtAngleB.Text));
                _pb006Mt6501.SetLevel(_chipIndex, "B", float.Parse(txtLevelB.Text));

                _pb006Mt6501.SetPoint(_chipIndex, "C", float.Parse(txtAngleC.Text));
                _pb006Mt6501.SetLevel(_chipIndex, "C", float.Parse(txtLevelC.Text));

                _pb006Mt6501.SetPoint(_chipIndex, "D", float.Parse(txtAngleD.Text));
                _pb006Mt6501.SetLevel(_chipIndex, "D", float.Parse(txtLevelD.Text));

                _pb006Mt6501.Program(_chipIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"标定失败：" + ex.Message);
            }
        }

        private void btnReadVoltPer_Click(object sender, EventArgs e)
        {
            txtVddPercent.Text = string.Empty;

            _pb006Mt6501.QuitOwi(_chipIndex);
            _pb006Mt6501.SetAdcMode(_chipIndex);

            //_pb006Mt6501.SetAdcMode(_chipIndex);
            var strVolt = _pb006Mt6501.MesureMeasVolt(_chipIndex).ToString(CultureInfo.InvariantCulture);
            var strPer = _pb006Mt6501.MesurePercentVdd(_chipIndex).ToString(CultureInfo.InvariantCulture);

            txtVddPercent.Text = string.Format("Percent of Output Volt: {0}%;\r\n Output: {1}V", strPer, strVolt);

            _pb006Mt6501.EnterOwi(_chipIndex);
        }
    }
}
