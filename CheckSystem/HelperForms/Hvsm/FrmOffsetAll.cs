using Sunny.UI;

namespace CheckSystem.HelperForms.Hvsm
{
    public partial class FrmOffsetAll : UIForm
    {
        public HvsmEmcConfig.DeviceConfigDutOffsetHsdOffset[] _para;

        public FrmOffsetAll()
        {
            InitializeComponent();
            Load += FrmOffsetAll_Load;
        }

        private void FrmOffsetAll_Load(object sender, System.EventArgs e)
        {
            ucSingleOffset1.SetTitle("DUT1~9统一设定");

            _para = new HvsmEmcConfig.DeviceConfigDutOffsetHsdOffset[4];
            for (var i = 0; i < _para.Length; i++)
            {
                _para[i] = new HvsmEmcConfig.DeviceConfigDutOffsetHsdOffset();
                _para[i].K = 1.0d;
                _para[i].B = 0.0d;
                _para[i].IsEnable = false.ToString();
                _para[i].ShowValue = 1.0d;
                _para[i].Threshold = 1.0d;
            }

            ucSingleOffset1.SetValue(_para);
        }

        private void btnUpdate_Click(object sender, System.EventArgs e)
        {
            if (!this.ShowAskDialog("是否确定更新"))
            {
                UIMessageTip.Show("取消更新");
                return;
            }

            _para[0].K = ucSingleOffset1.Hsd1K;
            _para[0].B = ucSingleOffset1.Hsd1B;
            _para[0].IsEnable = ucSingleOffset1.IsHsd1OverShow.ToString();
            _para[0].ShowValue = ucSingleOffset1.Hsd1OverShowValue;
            _para[0].Threshold = ucSingleOffset1.Hsd1OverShowThreshold;

            _para[1].K = ucSingleOffset1.Hsd1K;
            _para[1].B = ucSingleOffset1.Hsd1B;
            _para[1].IsEnable = ucSingleOffset1.IsHsd1OverShow.ToString();
            _para[1].ShowValue = ucSingleOffset1.Hsd1OverShowValue;
            _para[1].Threshold = ucSingleOffset1.Hsd1OverShowThreshold;

            _para[2].K = ucSingleOffset1.Hsd1K;
            _para[2].B = ucSingleOffset1.Hsd1B;
            _para[2].IsEnable = ucSingleOffset1.IsHsd1OverShow.ToString();
            _para[2].ShowValue = ucSingleOffset1.Hsd1OverShowValue;
            _para[2].Threshold = ucSingleOffset1.Hsd1OverShowThreshold;

            _para[3].K = ucSingleOffset1.Hsd1K;
            _para[3].B = ucSingleOffset1.Hsd1B;
            _para[3].IsEnable = ucSingleOffset1.IsHsd1OverShow.ToString();
            _para[3].ShowValue = ucSingleOffset1.Hsd1OverShowValue;
            _para[3].Threshold = ucSingleOffset1.Hsd1OverShowThreshold;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
