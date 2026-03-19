using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Controller;
using Go;
using HZH_Controls.IconFont;
using StateMachine;
using UserControls;

namespace CheckSystem
{
    public partial class FormPartMonitor : Form
    {
        private readonly UserDataGrid _partDataGrid = new UserDataGrid();
        private readonly List<PartInfo> _partInfo = new List<PartInfo>();
        private readonly State _status;
        private generator _action;
        //private Thread _refreshFieldValueThread;

        public class PartInfo
        {
            public string Name { get; set; }
            public string ControllerField { get; set; }
            public string Value { get; set; }
        }

        public FormPartMonitor(State status)
        {
            InitializeComponent();
            Icon = FontImages.GetIcon(FontIcons.A_fa_address_book, 32,
                Color.DodgerBlue);
            Text = @"信号监控界面";
            _status = status;
            InitPartInfo(_status.DeviceConfig);
            Load += FormPartMonitor_Load;
            Closed += FormPartMonitor_Closed;
        }

        public sealed override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private void FormPartMonitor_Load(object sender, EventArgs e)
        {
            //if (_refreshFieldValueThread != null)
            //{
            //    _refreshFieldValueThread.Abort();
            //    _refreshFieldValueThread.Join();
            //}

            //_refreshFieldValueThread =
            //    new Thread(GetPartInfoTimer) { IsBackground = true };
            //_refreshFieldValueThread.Start();

            _action = generator.tgo(FormSelection.MainStrand, GetPartInfoTimer);
        }

        private void FormPartMonitor_Closed(object sender, EventArgs e)
        {
            //if (_refreshFieldValueThread == null)
            //    return;

            //_refreshFieldValueThread.Abort();
            //_refreshFieldValueThread.Join();

            _action.stop();
        }

        private void InitPartInfo(DeviceConfig deviceConfig)
        {
            //_partInfo
            foreach (var t in deviceConfig.Parts)
                _partInfo.Add(new PartInfo { Name = t.Name, ControllerField = t.ControllerField, Value = "Null" });

            var bs = new BindingList<PartInfo>(_partInfo);
            _partDataGrid.Dock = DockStyle.Fill;
            _partDataGrid.label.Text = string.Format("上次刷新时间： {0}", DateTime.Now.ToString("yyyy-M-d HH:m:s tt zzz"));
            _partDataGrid.label.Visible = false;
            _partDataGrid.dataGridView.DataSource = bs;
            _partDataGrid.dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _partDataGrid.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _partDataGrid.dataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            _partDataGrid.dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            _partDataGrid.dataGridView.AllowUserToResizeColumns = false;
            _partDataGrid.dataGridView.AllowUserToResizeRows = false;
            _partDataGrid.dataGridView.ReadOnly = true;
            _partDataGrid.dataGridView.RowHeadersVisible = false;

            var tempTanbleLayoutPanel = new TableLayoutPanel { Dock = DockStyle.Fill };
            tempTanbleLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
            tempTanbleLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            var serchText = new TextBox { Dock = DockStyle.Fill };
            serchText.TextChanged += serchText_TextChanged;
            tempTanbleLayoutPanel.Controls.Add(serchText, 0, 0);
            tempTanbleLayoutPanel.Controls.Add(_partDataGrid, 0, 1);

            Controls.Add(tempTanbleLayoutPanel);
        }

        private void serchText_TextChanged(object sender, EventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null)
                return;
            lock (_partInfo)
            {
                _partInfo.Clear();
                if (string.IsNullOrEmpty(textBox.Text))
                    foreach (var t in _status.DeviceConfig.Parts)
                        _partInfo.Add(new PartInfo { Name = t.Name, ControllerField = t.ControllerField, Value = "Null" });
                else
                    foreach (var t in _status.DeviceConfig.Parts.Where(t => t.Name.Contains(textBox.Text)))
                        _partInfo.Add(new PartInfo { Name = t.Name, ControllerField = t.ControllerField, Value = "Null" });

                var bs = new BindingList<PartInfo>(_partInfo);
                _partDataGrid.dataGridView.DataSource = bs;
            }
        }

        private async Task GetPartInfoTimer()
        {
            while (true)
            {
                //if (!_refreshFieldValueThread.IsAlive)
                //    break;

                Thread.Sleep(500);

                await generator.sleep(50);

                try
                {
                    lock (_partInfo)
                    {
                        foreach (var p in _status.DeviceConfig.Parts)
                        {
                            var p1 = p;
                            var find =
                                _partInfo.Find(
                                    f =>
                                        !string.IsNullOrEmpty(f.Name) && f.Name.Equals(p1.Name) &&
                                        !string.IsNullOrEmpty(f.ControllerField) &&
                                        f.ControllerField == p1.ControllerField);
                            if (find == null)
                                continue;
                            var controllerName = p.ControllerName;
                            var fieldName = p.ControllerField.Split('.')[2];

                            foreach (var fieldValue in from t in _status.LstControllers
                                                       select t as ControllerBase
                                                           into c
                                                           where c != null && c.Name.Equals(controllerName) && c.GetType().GetField(fieldName).GetValue(c) != null
                                                           select c.GetType().GetField(fieldName).GetValue(c).ToString())
                                find.Value = fieldValue;
                        }

                        _partDataGrid.dataGridView.Refresh();
                        Application.DoEvents();
                    }
                }
                catch (Exception)
                {
                    // ignored
                }

                //if (InvokeRequired)
                //{
                //    Invoke(new Action(() =>
                //    {

                //    }));
                //}
            }
        }
    }
}
