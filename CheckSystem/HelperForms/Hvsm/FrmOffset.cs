using Sunny.UI;

namespace CheckSystem.HelperForms.Hvsm
{
    public partial class FrmOffset : UIForm
    {
        private UcSingleOffset[] ucSingleOffsets = new UcSingleOffset[9];
        public HvsmEmcConfig.DeviceConfigDutOffsetHsdOffset[][] _paras;

        public FrmOffset(HvsmEmcConfig.DeviceConfigDutOffsetHsdOffset[][] paras)
        {
            _paras = paras;
            InitializeComponent();
            Load += FrmOffset_Load;
        }

        private void FrmOffset_Load(object sender, System.EventArgs e)
        {
            for (var i = 0; i < ucSingleOffsets.Length; i++)
            {
                ucSingleOffsets[i] = new UcSingleOffset();
                ucSingleOffsets[i].Dock = System.Windows.Forms.DockStyle.Fill;
                ucSingleOffsets[i].SetTitle("DUT" + (i + 1));

                ucSingleOffsets[i].SetValue(_paras[i]);
            }

            dutPanel.Controls.Add(ucSingleOffsets[0], 0, 0);
            dutPanel.Controls.Add(ucSingleOffsets[1], 1, 0);
            dutPanel.Controls.Add(ucSingleOffsets[2], 2, 0);
            dutPanel.Controls.Add(ucSingleOffsets[3], 0, 1);
            dutPanel.Controls.Add(ucSingleOffsets[4], 1, 1);
            dutPanel.Controls.Add(ucSingleOffsets[5], 2, 1);
            dutPanel.Controls.Add(ucSingleOffsets[6], 0, 2);
            dutPanel.Controls.Add(ucSingleOffsets[7], 1, 2);
            dutPanel.Controls.Add(ucSingleOffsets[8], 2, 2);
        }

        private void btnUptade_Click(object sender, System.EventArgs e)
        {
            if (!this.ShowAskDialog("是否确定更新"))
            {
                UIMessageTip.Show("取消更新");
                return;
            }

            for (var i = 0; i < ucSingleOffsets.Length; i++)
            {
                _paras[i][0].K = ucSingleOffsets[i].Hsd1K;
                _paras[i][1].K = ucSingleOffsets[i].Hsd2K;
                _paras[i][2].K = ucSingleOffsets[i].Hsd3K;
                _paras[i][3].K = ucSingleOffsets[i].Hsd4K;

                _paras[i][0].B = ucSingleOffsets[i].Hsd1B;
                _paras[i][1].B = ucSingleOffsets[i].Hsd2B;
                _paras[i][2].B = ucSingleOffsets[i].Hsd3B;
                _paras[i][3].B = ucSingleOffsets[i].Hsd4B;

                _paras[i][0].IsEnable = ucSingleOffsets[i].IsHsd1OverShow.ToString();
                _paras[i][1].IsEnable = ucSingleOffsets[i].IsHsd2OverShow.ToString();
                _paras[i][2].IsEnable = ucSingleOffsets[i].IsHsd3OverShow.ToString();
                _paras[i][3].IsEnable = ucSingleOffsets[i].IsHsd4OverShow.ToString();

                _paras[i][0].Threshold = ucSingleOffsets[i].Hsd1OverShowThreshold;
                _paras[i][1].Threshold = ucSingleOffsets[i].Hsd2OverShowThreshold;
                _paras[i][2].Threshold = ucSingleOffsets[i].Hsd3OverShowThreshold;
                _paras[i][3].Threshold = ucSingleOffsets[i].Hsd4OverShowThreshold;

                _paras[i][0].ShowValue = ucSingleOffsets[i].Hsd1OverShowValue;
                _paras[i][1].ShowValue = ucSingleOffsets[i].Hsd2OverShowValue;
                _paras[i][2].ShowValue = ucSingleOffsets[i].Hsd3OverShowValue;
                _paras[i][3].ShowValue = ucSingleOffsets[i].Hsd4OverShowValue;
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnSetAll_Click(object sender, System.EventArgs e)
        {
            using (var frm = new FrmOffsetAll())
            {
                var result = frm.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    UIMessageTip.ShowOk(@"统一设定成功");

                    for (var i = 0; i < ucSingleOffsets.Length; i++)
                    {
                        for (var j = 0; j < frm._para.Length; j++)
                        {
                            _paras[i][j].K = frm._para[j].K;
                            _paras[i][j].B = frm._para[j].B;
                            _paras[i][j].IsEnable = frm._para[j].IsEnable;
                            _paras[i][j].Threshold = frm._para[j].Threshold;
                            _paras[i][j].ShowValue = frm._para[j].ShowValue;
                        }

                        ucSingleOffsets[i].SetValue(_paras[i]);
                    }
                }
            }
        }
    }
}
