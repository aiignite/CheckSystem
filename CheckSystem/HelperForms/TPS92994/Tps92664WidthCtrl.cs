using System;
using System.Windows.Forms;
using static CheckSystem.HelperForms.Tps92662.LedControlWithPwm;

namespace CheckSystem.HelperForms.TPS92994
{
    public partial class Tps92664WidthCtrl : UserControl
    {
        public event EventHandler<WidthValueChangeActionEventArgs> WidthValueChanged;
        
        private int _maxWidth = 1023;

        public string ShowName { get; set; }
        public int LedIndex { get; set; }

        public int WidthVal
        {
            get => widthTrackBar.Value;
            set
            {
                if (value < 0)
                    value = 0;

                if (value > _maxWidth)
                    value = _maxWidth;

                if (widthTrackBar.Value != value)
                    widthTrackBar.Value = value;

                if (txtWidthValue.Value != value)
                    txtWidthValue.Value = value;

                if (value == 0)
                {
                    btnOnOff.Active = false;
                }
                else
                {
                    btnOnOff.Active = true;
                }

                //var per = Math.Round(value / 1023f * 100, 4, MidpointRounding.AwayFromZero);
                //if (per < 0)
                //{
                //    per = 0;
                //}
                //if (per > 100)
                //{
                //    per = 100;
                //}
                //pwmTrackBar.Value = (int)per;
                //txtPwm.Text = per + "%";

                if (WidthValueChanged != null)
                    WidthValueChanged(this, new WidthValueChangeActionEventArgs(LedIndex, value));
            }
        }

        private int _baseValue = 1023;

        public Tps92664WidthCtrl(string showName, int width, int ledIndex, int baseValue)
        {
            InitializeComponent();

            LedIndex = ledIndex;

            if (baseValue < 0)
            {
                _baseValue = 0;
            }
            else if (baseValue > 1023)
            {
                _baseValue = 1023;
            }
            else
            {
                _baseValue = baseValue;
            }

            ShowName = showName;
            groupBox1.Text = showName;
            //pwmTrackBar.ValueChanged += PwmTrackBar_ValueChanged;
            pwmTrackBar.Value = 0;
            txtPwm.Text = pwmTrackBar.Value.ToString() + "%";

            widthTrackBar.Maximum = _maxWidth;
            txtWidthValue.Maximum = _maxWidth;

            widthTrackBar.Minimum = 0;
            txtWidthValue.Minimum = 0;

            widthTrackBar.ValueChanged += WidthTrackBar_ValueChanged;
            txtWidthValue.ValueChanged += TxtWidthValue_ValueChanged;

            btnOnOff.Click += BtnOnOff_Click;
        }

        public void SetPer(int value)
        {
            var per = Math.Round(value / 1023f * 100, 4, MidpointRounding.AwayFromZero);
            if (per < 0)
            {
                per = 0;
            }
            if (per > 100)
            {
                per = 100;
            }
            var action = new Action(() =>
            {
                pwmTrackBar.Value = (int)per;
                txtPwm.Text = per + "%";
            });

            if (InvokeRequired)
            {
                Invoke(action);
            }
            else
            {
                action();
            }
        }

        public void LedOff()
        {
            if (WidthVal > 0)
                BtnOnOff_Click(null, null);
        }

        private void BtnOnOff_Click(object sender, EventArgs e)
        {
            btnOnOff.Active = (WidthVal == 0);
            WidthVal = btnOnOff.Active ? _baseValue : 0;
        }

        private void TxtWidthValue_ValueChanged(object sender, EventArgs e)
        {
            WidthVal = (int)txtWidthValue.Value;
        }

        private void WidthTrackBar_ValueChanged(object sender, EventArgs e)
        {
            WidthVal = widthTrackBar.Value;
        }

        private void PwmTrackBar_ValueChanged(object sender, EventArgs e)
        {
            txtPwm.Text = pwmTrackBar.Value.ToString() + "%";
        }
    }
}
