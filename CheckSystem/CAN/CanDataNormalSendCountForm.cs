using System.Windows.Forms;
using HZH_Controls.Forms;

namespace CheckSystem.CAN
{
    public partial class CanDataNormalSendCountForm : FrmLoading
    {
        public CanDataNormalSendCountForm()
        {
            InitializeComponent();
        }

        protected override void BindingProcessMsg(string strText, int intValue)
        {
            label1.Text = strText;
            ucProcessLineExt1.Value = intValue;
        }

        public void SendMaxValue(int maxValue)
        {
            ucProcessLineExt1.MaxValue = maxValue;
        }
    }
}
