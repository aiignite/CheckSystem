using System;
using System.Windows.Forms;

namespace CheckSystem.HelperForms.Tps92662
{
    public partial class LedControlWithPwm : UserControl
    {
        public event EventHandler<PhaseValueChangeActionEventArgs> PhaseValueChanged;
        public event EventHandler<WidthValueChangeActionEventArgs> WidthValueChanged;

        public int LedIndex;
        public int PhaseVal
        {
            get => phaseTrackBar.Value;
            set
            {
                if (value < 0) value = 0;
                if (value > _maxPahase) value = _maxPahase;
                if (phaseTrackBar.Value != value)
                    phaseTrackBar.Value = value;
                txtPhaseValue.Text = value.ToString();
                if (PhaseValueChanged != null)
                    PhaseValueChanged(this, new PhaseValueChangeActionEventArgs(LedIndex, value));
            }
        }
        public int WidthVal
        {
            get => widthTrackBar.Value;
            set
            {
                if (value < 0) value = 0;
                if (value > _maxWidth) value = _maxWidth;
                if (widthTrackBar.Value != value)
                    widthTrackBar.Value = value;
                txtWidthValue.Text = value.ToString();
                if (WidthValueChanged != null)
                    WidthValueChanged(this, new WidthValueChangeActionEventArgs(LedIndex, value));
            }
        }

        private int _maxPahase = 1023;
        private int _maxWidth = 1023;
        private bool _isEnablePhaseWhenLedOn = true;

        public LedControlWithPwm(string name, int ledIndex)
        {
            InitializeComponent();
            groupBox1.Text = @"LED" + (12 - ledIndex);
            LedIndex = ledIndex;
            btnOnOff.Text = groupBox1.Text + @"/OFF";
            RegisterEvent();
            BindControls();
        }

        public LedControlWithPwm(int ledIndex, string name, ushort maxPhase, ushort maxWidth)
        {
            InitializeComponent();
            groupBox1.Text = name;
            LedIndex = ledIndex;
            btnOnOff.Text = groupBox1.Text + @"/OFF";

            _maxPahase = maxPhase;
            _maxWidth = maxWidth;
            _isEnablePhaseWhenLedOn = false;

            phaseTrackBar.Maximum = _maxPahase;
            widthTrackBar.Maximum = _maxWidth;

            RegisterEvent();
            BindControls();
        }

        private void RegisterEvent()
        {
            btnOnOff.ActiveChanged += BtnOnOff_ActiveChanged;
            txtPhaseValue.ValueChanged += TxtPhaseValue_ValueChanged;
            txtWidthValue.ValueChanged += TxtWidthValue_ValueChanged;
            phaseTrackBar.ValueChanged += PhaseTrackBar_ValueChanged;
            widthTrackBar.ValueChanged += WidthTrackBar_ValueChanged;
        }

        private void WidthTrackBar_ValueChanged(object sender, EventArgs e)
        {
            WidthVal = (int)widthTrackBar.Value;
        }

        private void PhaseTrackBar_ValueChanged(object sender, EventArgs e)
        {
            PhaseVal = (int)phaseTrackBar.Value;
        }

        private void TxtWidthValue_ValueChanged(object sender, EventArgs e)
        {
            WidthVal = (int)txtWidthValue.Value;
        }

        private void TxtPhaseValue_ValueChanged(object sender, EventArgs e)
        {
            PhaseVal = (int)txtPhaseValue.Value;
        }

        private void BtnOnOff_ActiveChanged(object sender, EventArgs e)
        {
            if (btnOnOff.Active)
            {
                PhaseVal = _isEnablePhaseWhenLedOn ? 50 : PhaseVal;
                WidthVal = _maxWidth;
            }
            else
            {
                PhaseVal = _isEnablePhaseWhenLedOn ? 0 : PhaseVal;
                WidthVal = 0;
            }
        }

        private void BindControls()
        {

        }

        public class PhaseValueChangeActionEventArgs : EventArgs
        {
            public int Index { get; protected set; }
            public int Value { get; protected set; }

            public PhaseValueChangeActionEventArgs(int index, int value)
            {
                Index = index;
                Value = value;
            }
        }

        public class WidthValueChangeActionEventArgs : EventArgs
        {
            public int Index { get; protected set; }
            public int Value { get; protected set; }

            public WidthValueChangeActionEventArgs(int index, int value)
            {
                Index = index;
                Value = value;
            }
        }
    }
}
