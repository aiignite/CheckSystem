using Controller;
using MiniExcelLibs;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CheckSystem.HelperForms.Psi5Reader
{
    public partial class UcSingleChannel : UIUserControl
    {
        private readonly int _channelIndex;
        private readonly SyPsi5InterfaceWithE52140 _psi5Interface;

        public UcSingleChannel(int channelIndex, SyPsi5InterfaceWithE52140 psi5Interface)
        {
            InitializeComponent();

            _channelIndex = channelIndex;
            _psi5Interface = psi5Interface;

            //uiGroupBox1.Style = UIStyle.LayuiGreen;
            uiGroupBox1.Text = @"Channel" + _channelIndex;

            cmbBitLen.SelectedIndexChanged += CmbBitLen_SelectedIndexChanged;
            cmbBitLen.SelectedIndex = 0;

            uiDataGridView1.ReadOnly = true;
            uiDataGridView1.RowHeadersVisible = false;
            uiDataGridView1.AllowUserToAddRows = false;
            uiDataGridView1.AllowUserToResizeRows = false;
            uiDataGridView1.MultiSelect = true;
            uiDataGridView1.RowHeadersVisible = false;
            uiDataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            uiDataGridView1.ClearRows();
            uiDataGridView1.ClearColumns();

            //uiDataGridView1.DataSource = _bindingList;
            //uiDataGridView1.Refresh();
        }

        private void CmbBitLen_SelectedIndexChanged(object sender, EventArgs e)
        {
            uiProcessBar1.Value = 0;
            txt14BitData.Text = string.Empty;
            txtErrorReporting.Text = string.Empty;
            txtRollingCount.Text = string.Empty;
            txtPsi5Data.Text = string.Empty;
            txtStatusBit.Text = string.Empty;

            btnInitChannel.Enabled = true;
            btnReadPsi5Data.Enabled = false;
        }

        private void btnInitChannel_Click(object sender, EventArgs e)
        {
            if (_psi5Interface == null)
                return;

            if (cmbBitLen.SelectedIndex == 0) // 25bits
            {
                _psi5Interface.ConfigChTimeslot(_channelIndex, 25);
                _psi5Interface.ConfigSpiBuffer(_channelIndex, 32);
            }
            else
            {
                _psi5Interface.ConfigChTimeslot(_channelIndex, 23);
                _psi5Interface.ConfigSpiBuffer(_channelIndex, 24);
            }

            //for (var i = 0; i < 50; i++)
            //{
            //    if (!_psi5Interface.ShortSyncPulseCh(_channelIndex))
            //        return;
            //}

            if (!_psi5Interface.ShortSyncPulseCh(_channelIndex, 50))
                return;

            btnReadPsi5Data.Enabled = true;
        }

        private void btnReadPsi5Data_Click(object sender, EventArgs e)
        {
            if (_psi5Interface == null)
                return;

            if (cmbBitLen.SelectedIndex == 0) // 25bits
                _psi5Interface.ReadSensorData32Bit(_channelIndex, 1);
            else
                _psi5Interface.ReadSensorData24Bit(_channelIndex, 1);

            var psi5OutputData = (double)_psi5Interface.GetType().GetField(string.Format("Channel{0}Psi5OutputData", _channelIndex)).GetValue(_psi5Interface);
            var psi5Data14Bit = (string)_psi5Interface.GetType().GetField(string.Format("Channel{0}Psi5Data14Bit", _channelIndex)).GetValue(_psi5Interface);
            var psi5RollingCount = (double)_psi5Interface.GetType().GetField(string.Format("Channel{0}Psi5RollingCounter", _channelIndex)).GetValue(_psi5Interface);
            var psi5StatusBit = (string)_psi5Interface.GetType().GetField(string.Format("Channel{0}Psi5StatusBit", _channelIndex)).GetValue(_psi5Interface);
            var outPutPercent = (double)_psi5Interface.GetType().GetField(string.Format("Channel{0}OutPutPercent", _channelIndex)).GetValue(_psi5Interface);

            var psi5RollingCountBits = (string)_psi5Interface.GetType().GetField(string.Format("Channel{0}Psi5RollingCounterBits", _channelIndex)).GetValue(_psi5Interface);
            var psi5CrcBits = (string)_psi5Interface.GetType().GetField(string.Format("Channel{0}Psi5CrcBits", _channelIndex)).GetValue(_psi5Interface);

            txt14BitData.Text = psi5Data14Bit;

            if (psi5StatusBit == "0")
            {
                if (outPutPercent <= uiProcessBar1.Maximum && outPutPercent >= 0)
                    uiProcessBar1.Value = (int)outPutPercent;
                else
                    uiProcessBar1.Value = 0;

                txtRollingCount.Text = psi5RollingCount.ToString(CultureInfo.InvariantCulture);
                txtStatusBit.Text = psi5StatusBit;
                txtPsi5Data.Text = psi5OutputData.ToString(CultureInfo.InvariantCulture);
                txtErrorReporting.Text = string.Empty;
            }
            else
            {
                uiProcessBar1.Value = 0;
                txtRollingCount.Text = psi5RollingCount.ToString(CultureInfo.InvariantCulture);
                txtStatusBit.Text = psi5StatusBit;
                txtPsi5Data.Text = string.Empty;
                txtErrorReporting.Text = string.Empty;

                if (SyPsi5InterfaceWithE52140.Psi5ErrorReporting.ContainsKey(psi5Data14Bit))
                    txtErrorReporting.Text = SyPsi5InterfaceWithE52140.Psi5ErrorReporting[psi5Data14Bit];
            }

            if (cmbBitLen.SelectedIndex == 0) // 25bits需要显示
            {
                _bindingList.Add(new SyPsi5InterfaceWithE52140.Psi5DataPackage(
                    psi5Data14Bit.Substring(0, 14), psi5RollingCountBits[0].ToString(), psi5RollingCountBits[1].ToString(),
                    psi5RollingCountBits[2].ToString(), psi5StatusBit, zerobits: string.Format("{0}{1}", psi5Data14Bit[14], psi5Data14Bit[15]), crcBits: psi5CrcBits));
                uiDataGridView1.DataSource = _bindingList.ToArray();
                uiDataGridView1.Refresh();
            }
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_bindingList.Any())
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    var result = fbd.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        var path = Path.Combine(fbd.SelectedPath, string.Format("ch{0}_export_on_{1}_{2}.xlsx",
                            _channelIndex, DateTime.Now.ToString("yyyyMMdd-hhmmss"), Guid.NewGuid().ToString().Substring(24, 12)));
                        MiniExcel.SaveAs(path, _bindingList);
                    }
                }
            }
        }

        private void 清空ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _bindingList.Clear();
            uiDataGridView1.DataSource = _bindingList.ToArray();
            uiDataGridView1.Refresh();
        }

        private readonly List<SyPsi5InterfaceWithE52140.Psi5DataPackage> _bindingList =
            new List<SyPsi5InterfaceWithE52140.Psi5DataPackage>();
    }
}
