using Sunny.UI;

namespace CheckSystem.HelperForms.Hvsm
{
    public partial class FrmTempMonitorSet : UIForm
    {
        public HvsmEmcConfig.DeviceConfigTemperaturePara _para;

        public FrmTempMonitorSet(HvsmEmcConfig.DeviceConfigTemperaturePara para)
        {
            InitializeComponent();
            _para = para;
            Load += FrmTempMonitorSet_Load;
        }

        private void FrmTempMonitorSet_Load(object sender, System.EventArgs e)
        {
            cmbIsEnable.Items.Clear();
            cmbIsEnable.Items.Add("禁用");
            cmbIsEnable.Items.Add("启用");
            cmbIsEnable.SelectedIndex = bool.Parse(_para.IsEnable) ? 1 : 0;

            cmbSymbol.Items.Clear();
            cmbSymbol.Items.Add(">");
            cmbSymbol.Items.Add(">=");
            cmbSymbol.Items.Add("<");
            cmbSymbol.Items.Add("<=");
            cmbSymbol.Items.Add("==");
            for (var i = 0; i < cmbSymbol.Items.Count; i++)
            {
                if (_para.Sysmbol == cmbSymbol.Items[i].ToString())
                {
                    cmbSymbol.SelectedIndex = i;
                    break;
                }
            }

            numValue.Value = (decimal)_para.Value;

            cmbNtcChannel.Items.Clear();
            for (var i = 1; i <= 4; i++)
                cmbNtcChannel.Items.Add(i);
            for (var i = 0; i < cmbNtcChannel.Items.Count; i++)
            {
                if (_para.NtcChannel.ToString() == cmbNtcChannel.Items[i].ToString())
                {
                    cmbNtcChannel.SelectedIndex = i;
                    break;
                }
            }
        }

        private void btnUpdate_Click(object sender, System.EventArgs e)
        {
            if (this.ShowAskDialog("是否确定更新？"))
            {
                _para.IsEnable = cmbIsEnable.SelectedIndex == 0 ? "False" : "True";
                _para.Value = (double)numValue.Value;
                _para.Sysmbol = cmbSymbol.Text;
                _para.NtcChannel = int.Parse(cmbNtcChannel.Text);

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }
    }
}
