using System;
using System.Windows.Forms;

namespace CheckSystem.HelperForms.Hvsm
{
    public partial class UcSingleOffset : UserControl
    {
        public UcSingleOffset()
        {
            InitializeComponent();

            Hsd4OverShowValue = (double)numHsd4OverShowValue.Value;
            Hsd3OverShowValue = (double)numHsd3OverShowValue.Value;
            Hsd2OverShowValue = (double)numHsd2OverShowValue.Value;
            Hsd1OverShowValue = (double)numHsd1OverShowValue.Value;

            Hsd4OverShowThreshold = (double)numHsd4Threshold.Value;
            Hsd3OverShowThreshold = (double)numHsd3Threshold.Value;
            Hsd2OverShowThreshold = (double)numHsd2Threshold.Value;
            Hsd1OverShowThreshold = (double)numHsd1Threshold.Value;

            IsHsd4OverShow = checkBox4.Checked;
            IsHsd3OverShow = checkBox3.Checked;
            IsHsd2OverShow = checkBox2.Checked;
            IsHsd1OverShow = checkBox1.Checked;

            Hsd4B = (double)numDutHsd4B.Value;
            Hsd3B = (double)numDutHsd3B.Value;
            Hsd2B = (double)numDutHsd2B.Value;
            Hsd1B = (double)numDutHsd1B.Value;

            Hsd4K = (double)numDutHsd4K.Value;
            Hsd3K = (double)numDutHsd3K.Value;
            Hsd2K = (double)numDutHsd2K.Value;
            Hsd1K = (double)numDutHsd1K.Value;

            numDutHsd1K.ValueChanged += NumDutHsd1K_ValueChanged;
            numDutHsd2K.ValueChanged += NumDutHsd2K_ValueChanged;
            numDutHsd3K.ValueChanged += NumDutHsd3K_ValueChanged;
            numDutHsd4K.ValueChanged += NumDutHsd4K_ValueChanged;
            numDutHsd1B.ValueChanged += NumDutHsd1B_ValueChanged;
            numDutHsd2B.ValueChanged += NumDutHsd2B_ValueChanged;
            numDutHsd3B.ValueChanged += NumDutHsd3B_ValueChanged;
            numDutHsd4B.ValueChanged += NumDutHsd4B_ValueChanged;

            checkBox1.CheckedChanged += CheckBox1_CheckedChanged;
            checkBox2.CheckedChanged += CheckBox2_CheckedChanged;
            checkBox3.CheckedChanged += CheckBox3_CheckedChanged;
            checkBox4.CheckedChanged += CheckBox4_CheckedChanged;

            numHsd1Threshold.ValueChanged += NumHsd1Threshold_ValueChanged;
            numHsd2Threshold.ValueChanged += NumHsd2Threshold_ValueChanged;
            numHsd3Threshold.ValueChanged += NumHsd3Threshold_ValueChanged;
            numHsd4Threshold.ValueChanged += NumHsd4Threshold_ValueChanged;

            numHsd1OverShowValue.ValueChanged += NumHsd1OverShowValue_ValueChanged;
            numHsd2OverShowValue.ValueChanged += NumHsd2OverShowValue_ValueChanged;
            numHsd3OverShowValue.ValueChanged += NumHsd3OverShowValue_ValueChanged;
            numHsd4OverShowValue.ValueChanged += NumHsd4OverShowValue_ValueChanged;
        }

        public void SetValue(HvsmEmcConfig.DeviceConfigDutOffsetHsdOffset[] para)
        {
            numHsd4OverShowValue.Value = (decimal)para[3].ShowValue;
            numHsd3OverShowValue.Value = (decimal)para[2].ShowValue;
            numHsd2OverShowValue.Value = (decimal)para[1].ShowValue;
            numHsd1OverShowValue.Value = (decimal)para[0].ShowValue;

            numHsd4Threshold.Value = (decimal)para[3].Threshold;
            numHsd3Threshold.Value = (decimal)para[2].Threshold;
            numHsd2Threshold.Value = (decimal)para[1].Threshold;
            numHsd1Threshold.Value = (decimal)para[0].Threshold;

            checkBox4.Checked = bool.Parse(para[3].IsEnable);
            checkBox3.Checked = bool.Parse(para[2].IsEnable);
            checkBox2.Checked = bool.Parse(para[1].IsEnable);
            checkBox1.Checked = bool.Parse(para[0].IsEnable);

            numDutHsd4B.Value = (decimal)para[3].B;
            numDutHsd3B.Value = (decimal)para[2].B;
            numDutHsd2B.Value = (decimal)para[1].B;
            numDutHsd1B.Value = (decimal)para[0].B;

            numDutHsd4K.Value = (decimal)para[3].K;
            numDutHsd3K.Value = (decimal)para[2].K;
            numDutHsd2K.Value = (decimal)para[1].K;
            numDutHsd1K.Value = (decimal)para[0].K;
        }

        private void NumHsd4OverShowValue_ValueChanged(object sender, EventArgs e)
        {
            Hsd4OverShowValue = (double)numHsd4OverShowValue.Value;
        }

        private void NumHsd3OverShowValue_ValueChanged(object sender, EventArgs e)
        {
            Hsd3OverShowValue = (double)numHsd3OverShowValue.Value;
        }

        private void NumHsd2OverShowValue_ValueChanged(object sender, EventArgs e)
        {
            Hsd2OverShowValue = (double)numHsd2OverShowValue.Value;
        }

        private void NumHsd1OverShowValue_ValueChanged(object sender, EventArgs e)
        {
            Hsd1OverShowValue = (double)numHsd1OverShowValue.Value;
        }

        private void NumHsd4Threshold_ValueChanged(object sender, EventArgs e)
        {
            Hsd4OverShowThreshold = (double)numHsd4Threshold.Value;
        }

        private void NumHsd3Threshold_ValueChanged(object sender, EventArgs e)
        {
            Hsd3OverShowThreshold = (double)numHsd3Threshold.Value;
        }

        private void NumHsd2Threshold_ValueChanged(object sender, EventArgs e)
        {
            Hsd2OverShowThreshold = (double)numHsd2Threshold.Value;
        }

        private void NumHsd1Threshold_ValueChanged(object sender, EventArgs e)
        {
            Hsd1OverShowThreshold = (double)numHsd1Threshold.Value;
        }

        private void CheckBox4_CheckedChanged(object sender, EventArgs e)
        {
            IsHsd4OverShow = checkBox4.Checked;
        }

        private void CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            IsHsd3OverShow = checkBox3.Checked;
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            IsHsd2OverShow = checkBox2.Checked;
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            IsHsd1OverShow = checkBox1.Checked;
        }

        private void NumDutHsd4B_ValueChanged(object sender, EventArgs e)
        {
            Hsd4B = (double)numDutHsd4B.Value;
        }

        private void NumDutHsd3B_ValueChanged(object sender, EventArgs e)
        {
            Hsd3B = (double)numDutHsd3B.Value;
        }

        private void NumDutHsd2B_ValueChanged(object sender, EventArgs e)
        {
            Hsd2B = (double)numDutHsd2B.Value;
        }

        private void NumDutHsd1B_ValueChanged(object sender, EventArgs e)
        {
            Hsd1B = (double)numDutHsd1B.Value;
        }

        private void NumDutHsd4K_ValueChanged(object sender, EventArgs e)
        {
            Hsd4K = (double)numDutHsd4K.Value;
        }

        private void NumDutHsd3K_ValueChanged(object sender, EventArgs e)
        {
            Hsd3K = (double)numDutHsd3K.Value;
        }

        private void NumDutHsd2K_ValueChanged(object sender, EventArgs e)
        {
            Hsd2K = (double)numDutHsd2K.Value;
        }

        private void NumDutHsd1K_ValueChanged(object sender, EventArgs e)
        {
            Hsd1K = (double)numDutHsd1K.Value;
        }

        public void SetTitle(string title)
        {
            uiGroupBox1.Text = title;
        }

        public Double Hsd1K { get; set; }
        public Double Hsd1B { get; set; }

        public Double Hsd2K { get; set; }
        public Double Hsd2B { get; set; }

        public Double Hsd3K { get; set; }
        public Double Hsd3B { get; set; }

        public Double Hsd4K { get; set; }
        public Double Hsd4B { get; set; }

        public bool IsHsd1OverShow { get; set; }
        public double Hsd1OverShowThreshold { get; set; }
        public double Hsd1OverShowValue { get; set; }

        public bool IsHsd2OverShow { get; set; }
        public double Hsd2OverShowThreshold { get; set; }
        public double Hsd2OverShowValue { get; set; }

        public bool IsHsd3OverShow { get; set; }
        public double Hsd3OverShowThreshold { get; set; }
        public double Hsd3OverShowValue { get; set; }

        public bool IsHsd4OverShow { get; set; }
        public double Hsd4OverShowThreshold { get; set; }
        public double Hsd4OverShowValue { get; set; }
    }
}
