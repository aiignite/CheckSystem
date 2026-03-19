using Sunny.UI;

namespace CheckSystem.HelperForms.Hvsm
{
    public partial class FrmStandbyParas : UIForm
    {
        public HvsmEmcConfig.DeviceConfigStandbyModePara _para;

        public FrmStandbyParas(HvsmEmcConfig.DeviceConfigStandbyModePara para)
        {
            _para = para;
            InitializeComponent();
            Load += FrmStandbyParas_Load;
        }

        private void FrmStandbyParas_Load(object sender, System.EventArgs e)
        {
            numHsd1Duty.Value = _para.Hsds[0].Duty;
            numHsd1Freq.Value = _para.Hsds[0].Freq;

            numHsd2Duty.Value = _para.Hsds[1].Duty;
            numHsd2Freq.Value = _para.Hsds[1].Freq;

            numHsd3Duty.Value = _para.Hsds[2].Duty;
            numHsd3Freq.Value = _para.Hsds[2].Freq;

            numHsd4Duty.Value = _para.Hsds[3].Duty;
            numHsd4Freq.Value = _para.Hsds[3].Freq;

            numFan1Duty.Value = _para.Fans[0].Duty;
            numFan2Duty.Value = _para.Fans[1].Duty;
        }

        private void btnUptade_Click(object sender, System.EventArgs e)
        {
            if (this.ShowAskDialog("是否确定更新？"))
            {
                _para.Hsds[0].Duty = (int)numHsd1Duty.Value;
                _para.Hsds[0].Freq = (int)numHsd1Freq.Value;

                _para.Hsds[1].Duty = (int)numHsd2Duty.Value;
                _para.Hsds[1].Freq = (int)numHsd2Freq.Value;

                _para.Hsds[2].Duty = (int)numHsd3Duty.Value;
                _para.Hsds[2].Freq = (int)numHsd3Freq.Value;

                _para.Hsds[3].Duty = (int)numHsd4Duty.Value;
                _para.Hsds[3].Freq = (int)numHsd4Freq.Value;

                _para.Fans[0].Duty = (int)numFan1Duty.Value;
                _para.Fans[1].Duty = (int)numFan2Duty.Value;

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }
    }
}
