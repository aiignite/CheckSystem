using System;
using System.Threading;
using System.Threading.Tasks;
using Controller;
using Sunny.UI;

namespace CheckSystem.HelperForms
{
    public partial class WirelessReadData : UIForm
    {
        private readonly ToomossUsb2XxxCanLin _toomoss = new ToomossUsb2XxxCanLin("toomoss");
        private readonly WirelessChargingModule _module = new WirelessChargingModule("无线充电");

        public WirelessReadData()
        {
            InitializeComponent();
            Load += WirelessReadData_Load;
            FormClosing += WirelessReadData_FormClosing;
        }

        private void WirelessReadData_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            _module?.Dispose(); _toomoss?.Dispose();
        }

        private void WirelessReadData_Load(object sender, System.EventArgs e)
        {
            if (_toomoss.Can1 is null)
            {
                uiButton1.Enabled = false;
                UIMessageBox.ShowError("can卡打开失败，请重启程序");
                return;
            }
            _module.CanFD = _toomoss.Can1;
        }

        private async void uiButton1_Click(object sender, System.EventArgs e)
        {
            try
            {
                uiButton1.Enabled = false;
                uiRichTextBox1.Clear();
                uiRichTextBox1.AppendText(DateTime.Now.ToString("HH:mm:ss zz") + " 正在读取");
                uiRichTextBox1.AppendText(Environment.NewLine);

                await Task.Run(() =>
                {
                    _module.StartCan();
                    Thread.Sleep(1000);
                    _module.StopCan();
                    Thread.Sleep(500);
                    _module.DebugReadMtc();
                });

                uiRichTextBox1.AppendText(DateTime.Now.ToString("HH:mm:ss zz") + " 追溯信息读取结果：" + _module.MTC);
            }
            catch (Exception)
            {
                // ignored
            }
            finally
            {
                uiButton1.Enabled = true;
            }
        }
    }
}
