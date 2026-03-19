using System;
using System.Threading.Tasks;
using Controller;
using Sunny.UI;

namespace CheckSystem.HelperForms.HalHac3980
{
    public partial class FrmProgramming : UIForm
    {
        private readonly HalHac3980Programmer _halHac3980Programmer = new HalHac3980Programmer("HalHac3980");

        public FrmProgramming()
        {
            InitializeComponent();
            _halHac3980Programmer.PushInfoMsg += _halHac3980Programmer_PushInfoMsg;
        }

        private delegate void UpdateInfoDelegate(UIRichTextBox control, string value);

        private void UpdateInfo(UIRichTextBox control, string value)
        {
            var de = new UpdateInfoDelegate(UpdateInfo);
            if (control.InvokeRequired)
                Invoke(de, control, value);
            else
            {
                control.AppendText(value);
                control.AppendText(Environment.NewLine);
            }
        }

        private void btnClearInfo_Click(object sender, EventArgs e)
        {
            txtSerialInfo.Clear();
        }

        private void _halHac3980Programmer_PushInfoMsg(string info)
        {
            UpdateInfo(txtSerialInfo, info);
        }

        private async void btnOpenCom_Click(object sender, System.EventArgs e)
        {
            await Task.Run(() => { _halHac3980Programmer.ConnectCom("COM5"); });
        }

        private async void btnGetFirmwareVersion_Click(object sender, System.EventArgs e)
        {
            await Task.Run(() => { _halHac3980Programmer.GetFirmwareVersion(); });
        }

        private async void btnSwitchModeB_Click(object sender, EventArgs e)
        {
            await Task.Run(() => { _halHac3980Programmer.SwitchModeB(); });
        }

        private async void btnSetBitTime_Click(object sender, EventArgs e)
        {
            await Task.Run((() => { _halHac3980Programmer.SetBitTime(); }));
        }

        private async void btnVsupOn_Click(object sender, EventArgs e)
        {
            await Task.Run((() => { _halHac3980Programmer.VsupOn(); }));
        }

        private async void btnVsupOff_Click(object sender, EventArgs e)
        {
            await Task.Run((() => { _halHac3980Programmer.VsupOff(); }));
        }

        private async void btnStartListeningAndProgramMode_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                //_halHac3980Programmer.StartListeningMode();
                //Task.Delay(50).Wait();
                //_halHac3980Programmer.StartProgrammingMode();
                //Task.Delay(50).Wait();
                //_halHac3980Programmer.ReadOneRegisterFromEEPROM(0x75);
            });
        }

        private async void btnTestWrite_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                //_halHac3980Programmer.ReadOneRegisterFromEEPROM(0x08);
                //Task.Delay(50).Wait();
                //_halHac3980Programmer.WriteEeprom(0x08, "36B6");
                //Task.Delay(50).Wait();
                //_halHac3980Programmer.ReadOneRegisterFromEEPROM(0x08);
            });
        }

        private async void btnSelectIoCh1_Click(object sender, EventArgs e)
        {
            await Task.Run(() => { _halHac3980Programmer.SelectIoChannel(1); });
        }

        private async void btnTestStore_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                //_halHac3980Programmer.StoringOneRegisterToEEPROM(0x08);
            });
        }
    }
}
