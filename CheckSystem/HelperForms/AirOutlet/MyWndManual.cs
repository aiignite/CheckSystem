using Sunny.UI;
using System.Threading;
using static CheckSystem.HelperForms.AirOutlet.MyWndLoadBox;

namespace CheckSystem.HelperForms.AirOutlet
{
    public partial class MyWndManual : UIForm
    {
        private DutInfo[,] _dutInfos;
        private int[,] DutCount;
        private bool _isRunning;

        public MyWndManual(DutInfo[,] duts, int[,] dutCount)
        {
            InitializeComponent();
            _dutInfos = duts;
            DutCount = dutCount;
            FormClosing += MyWndManual_FormClosing;
            numericUpDown1.ValueChanged += NumericUpDown1_ValueChanged;
            numericUpDown2.ValueChanged += NumericUpDown2_ValueChanged;
        }

        private void NumericUpDown2_ValueChanged(object sender, System.EventArgs e)
        {
            if (_isRunning)
            {
                for (var ch = 0; ch < DutCount.GetLength(0); ch++)
                {
                    if (DutCount[ch, 0] is 0)
                        continue;

                    for (var i = 0; i < DutCount[ch, 0]; i++)
                    {
                        _dutInfos[ch, i].Slave?.SetSpeedLevel((byte)numericUpDown2.Value);
                    }
                }
            }
        }

        private void NumericUpDown1_ValueChanged(object sender, System.EventArgs e)
        {
            if (_isRunning)
            {
                for (var ch = 0; ch < DutCount.GetLength(0); ch++)
                {
                    if (DutCount[ch, 0] is 0)
                        continue;

                    for (var i = 0; i < DutCount[ch, 0]; i++)
                    {
                        _dutInfos[ch, i].Slave?.SetTargetPosition((ushort)numericUpDown1.Value);
                    }
                }
            }
        }

        private void MyWndManual_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (!UIMessageBox.ShowAsk("确定要退出吗"))
            {
                UIMessageTip.Show("取消退出");
                e.Cancel = true;
                return;
            }

            for (var ch = 0; ch < DutCount.GetLength(0); ch++)
            {
                if (DutCount[ch, 0] is 0)
                    continue;

                for (var i = 0; i < DutCount[ch, 0]; i++)
                {
                    _dutInfos[ch, i].Slave?.SetStallDetection(1);
                    _dutInfos[ch, i].Slave?.SetMode(1);
                }
            }
        }

        private void uiButton1_Click(object sender, System.EventArgs e)
        {
            for (var ch = 0; ch < DutCount.GetLength(0); ch++)
            {
                if (DutCount[ch, 0] is 0)
                    continue;

                for (var i = 0; i < DutCount[ch, 0]; i++)
                {
                    _dutInfos[ch, i].Slave?.ClearEvent(14);
                }
            }

            Thread.Sleep(500);

            for (var ch = 0; ch < DutCount.GetLength(0); ch++)
            {
                if (DutCount[ch, 0] is 0)
                    continue;

                for (var i = 0; i < DutCount[ch, 0]; i++)
                {
                    _dutInfos[ch, i].Slave?.ClearEvent(0);
                }
            }

            Thread.Sleep(500);

            for (var ch = 0; ch < DutCount.GetLength(0); ch++)
            {
                if (DutCount[ch, 0] is 0)
                    continue;

                for (var i = 0; i < DutCount[ch, 0]; i++)
                {
                    _dutInfos[ch, i].Slave?.SetStallDetection(0);
                    _dutInfos[ch, i].Slave?.SetValidPosition(0);
                    _dutInfos[ch, i].Slave?.SetSpeedLevel((byte)numericUpDown2.Value);
                    _dutInfos[ch, i].Slave?.SetTargetPosition((ushort)numericUpDown1.Value);
                    _dutInfos[ch, i].Slave?.SetMode(0);
                }
            }

            uiButton1.Enabled = false;
            uiButton2.Enabled = true;
            _isRunning = true;
        }

        private void uiButton2_Click(object sender, System.EventArgs e)
        {
            for (var ch = 0; ch < DutCount.GetLength(0); ch++)
            {
                if (DutCount[ch, 0] is 0)
                    continue;

                for (var i = 0; i < DutCount[ch, 0]; i++)
                {
                    _dutInfos[ch, i].Slave?.SetStallDetection(1);
                    _dutInfos[ch, i].Slave?.SetMode(1);
                }
            }

            uiButton1.Enabled = true;
            uiButton2.Enabled = false;
            _isRunning = false;
        }
    }
}
