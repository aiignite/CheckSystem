using Sunny.UI;
using System;

namespace CheckSystem.VisionDetection
{
    public partial class FrmVisionFileSelection : UIForm
    {
        public FrmVisionFileSelection()
        {
            InitializeComponent();
        }

        private void btnGetOrderNo_Click(object sender, EventArgs e)
        {
            CommonUtility.SynchronizationParameters.SendDeviceNoByApi("");

            //string workOrderNo;
            //string pnNo;
            //string filePath;
            //CommonUtility.SynchronizationParameters.GetWorkOrderNoByApi(“”, out workOrderNo, out pnNo, out filePath);
        }
    }
}
