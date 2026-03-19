using Sunny.UI;
using System.Windows.Forms;

namespace CheckSystem.HelperForms.Hvsm
{
    public partial class FrmCycleSet : UIForm
    {
        public HvsmEmcConfig.DeviceConfigSingleCycleParaCyclePara[][] _cyclePara;

        public FrmCycleSet(HvsmEmcConfig.DeviceConfigSingleCycleParaCyclePara[][] cyclePara)
        {
            _cyclePara = cyclePara;
            InitializeComponent();
            Load += FrmCycleSet_Load;
        }

        private void FrmCycleSet_Load(object sender, System.EventArgs e)
        {
            InitDgv(pointGgv1, 0);
            InitDgv(pointGgv2, 1);
            InitDgv(pointGgv3, 2);
            InitDgv(pointGgv4, 3);
            InitDgv(pointGgv5, 4);
            InitDgv(pointGgv6, 5);
            InitDgv(pointGgv7, 6);
            InitDgv(pointGgv8, 7);
            InitDgv(pointGgv9, 8);
        }

        private void InitDgv(UIDataGridView pointGgv, int index, bool isRegesterEvent = true)
        {
            pointGgv.Width = Width;
            pointGgv.Style = UIStyle.Gray;
            pointGgv.AllowUserToAddRows = false;
            pointGgv.AllowUserToResizeRows = true;
            pointGgv.AllowUserToDeleteRows = true;
            pointGgv.MultiSelect = true;
            pointGgv.RowHeadersVisible = true;
            pointGgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            pointGgv.ClearAll();

            pointGgv.AddColumn("Name", "Name", readOnly: true);
            pointGgv.AddColumn("Time/S", "Time", readOnly: false);
            pointGgv.AddColumn("HSD_Duty", "HSD_Duty", readOnly: false);
            pointGgv.AddColumn("HSD_Freq", "HSD_Freq", readOnly: false);
            pointGgv.AddColumn("FAN_Duty", "FAN_Duty", readOnly: false);

            for (int i = 0; i < _cyclePara[index].Length; i++)
            {
                var mode = _cyclePara[index][i].Mode == 0 ? "SLEEP模式" : "动作模式";
                var time = _cyclePara[index][i].Time;
                var hsd_duty = _cyclePara[index][i].Hsd.Duty.ToString();
                var hsd_freq = _cyclePara[index][i].Hsd.Freq.ToString();
                var fan_duty = _cyclePara[index][i].Fan.Duty.ToString();
                pointGgv.AddRow(new object[] { mode, time, hsd_duty, hsd_freq, fan_duty });
            }

            if (isRegesterEvent)
                pointGgv.CellValidating += PointGgv_CellValidating;
        }

        private void PointGgv_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            var pointGgv = sender as UIDataGridView;

            var cell = pointGgv[e.ColumnIndex, e.RowIndex];

            if (cell == null)
                return;

            try
            {
                if (e.ColumnIndex >= 1)
                {
                    if (string.IsNullOrWhiteSpace(e.FormattedValue.ToString()))
                    {
                        e.Cancel = true;
                        this.ShowErrorDialog(string.Format("第{0}行、第{1}列数据不能为空", e.RowIndex + 1, e.ColumnIndex + 1));
                        pointGgv.CurrentCell = cell;
                        pointGgv.Focus();
                        pointGgv.CancelEdit();
                    }
                    else
                    {
                        var newValue = double.Parse(e.FormattedValue.ToString());

                        if (e.ColumnIndex == 1)
                        {
                            if (newValue <= 1)
                            {
                                e.Cancel = true;
                                this.ShowErrorDialog(string.Format("第{0}行, 时间={1}, 请输入大于1S", e.RowIndex + 1, newValue));
                                pointGgv.CurrentCell = cell;
                                pointGgv.Focus();
                                pointGgv.CancelEdit();
                            }
                        }
                        else if (e.ColumnIndex == 2 || e.ColumnIndex == 4)
                        {
                            if (newValue < 0 || newValue > 100)
                            {
                                e.Cancel = true;
                                this.ShowErrorDialog(string.Format("第{0}行, Duty={1}, 请输入0~100", e.RowIndex + 1, newValue));
                                pointGgv.CurrentCell = cell;
                                pointGgv.Focus();
                                pointGgv.CancelEdit();
                            }
                        }
                        else if (e.ColumnIndex == 3)
                        {
                            if (newValue < 0)
                            {
                                e.Cancel = true;
                                this.ShowErrorDialog(string.Format("第{0}行, Freq={1}, 请输入大于1", e.RowIndex + 1, newValue));
                                pointGgv.CurrentCell = cell;
                                pointGgv.Focus();
                                pointGgv.CancelEdit();
                            }
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                e.Cancel = true;
                this.ShowErrorDialog(string.Format("第{0}行, 输入格式错误", e.RowIndex + 1));
                pointGgv.CurrentCell = cell;
                pointGgv.Focus();
                pointGgv.CancelEdit();
            }
        }

        private void btnUptade_Click(object sender, System.EventArgs e)
        {
            if (pointGgv1.RowCount < 1)
            {
                this.ShowErrorDialog("DUT1缺少测试点，请添加测试点");
                return;
            }
            if (pointGgv2.RowCount < 1)
            {
                this.ShowErrorDialog("DUT2缺少测试点，请添加测试点");
                return;
            }
            if (pointGgv3.RowCount < 1)
            {
                this.ShowErrorDialog("DUT3缺少测试点，请添加测试点");
                return;
            }
            if (pointGgv4.RowCount < 1)
            {
                this.ShowErrorDialog("DUT4缺少测试点，请添加测试点");
                return;
            }
            if (pointGgv5.RowCount < 1)
            {
                this.ShowErrorDialog("DUT5缺少测试点，请添加测试点");
                return;
            }
            if (pointGgv6.RowCount < 1)
            {
                this.ShowErrorDialog("DUT6缺少测试点，请添加测试点");
                return;
            }
            if (pointGgv7.RowCount < 1)
            {
                this.ShowErrorDialog("DUT7缺少测试点，请添加测试点");
                return;
            }
            if (pointGgv8.RowCount < 1)
            {
                this.ShowErrorDialog("DUT8缺少测试点，请添加测试点");
                return;
            }
            if (pointGgv9.RowCount < 1)
            {
                this.ShowErrorDialog("DUT9缺少测试点，请添加测试点");
                return;
            }

            if (this.ShowAskDialog("是否确定更新？"))
            {
                var dutCount = _cyclePara.Length;
                _cyclePara = new HvsmEmcConfig.DeviceConfigSingleCycleParaCyclePara[dutCount][];
                for (int kk = 0; kk < dutCount; kk++)
                {
                    var pointGgvControl = Controls.Find(string.Format("pointGgv{0}", kk + 1), true);
                    if (pointGgvControl.Length > 0)
                    {
                        var pointGgv = pointGgvControl[0] as UIDataGridView;

                        _cyclePara[kk] = new HvsmEmcConfig.DeviceConfigSingleCycleParaCyclePara[pointGgv.Rows.Count];

                        for (int i = 0; i < pointGgv.Rows.Count; i++)
                        {
                            var row = pointGgv.Rows[i];

                            var mode = row.Cells[0].Value.ToString().ToLower().Contains("sleep") ? 0 : 1;
                            var time = double.Parse(row.Cells[1].Value.ToString());
                            var hsd_duty = int.Parse(row.Cells[2].Value.ToString());
                            var hsd_freq = int.Parse(row.Cells[3].Value.ToString());
                            var fan_duty = int.Parse(row.Cells[4].Value.ToString());

                            _cyclePara[kk][i] = new HvsmEmcConfig.DeviceConfigSingleCycleParaCyclePara();
                            _cyclePara[kk][i].Mode = mode;
                            _cyclePara[kk][i].Time = time;
                            _cyclePara[kk][i].Hsd = new HvsmEmcConfig.DeviceConfigSingleCycleParaCycleParaHsd();
                            _cyclePara[kk][i].Hsd.Duty = hsd_duty;
                            _cyclePara[kk][i].Hsd.Freq = hsd_freq;
                            _cyclePara[kk][i].Fan = new HvsmEmcConfig.DeviceConfigSingleCycleParaCycleParaFan();
                            _cyclePara[kk][i].Fan.Duty = fan_duty;
                        }
                    }
                }

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        private void btnAddStandby_Click(object sender, System.EventArgs e)
        {
            switch (uiTabControl1.SelectedIndex)
            {
                case 0:
                    pointGgv1.AddRow(new object[] { "动作模式", "10", "50", "200", "50" });
                    break;

                case 1:
                    pointGgv2.AddRow(new object[] { "动作模式", "10", "50", "200", "50" });
                    break;

                case 2:
                    pointGgv3.AddRow(new object[] { "动作模式", "10", "50", "200", "50" });
                    break;

                case 3:
                    pointGgv4.AddRow(new object[] { "动作模式", "10", "50", "200", "50" });
                    break;

                case 4:
                    pointGgv5.AddRow(new object[] { "动作模式", "10", "50", "200", "50" });
                    break;

                case 5:
                    pointGgv6.AddRow(new object[] { "动作模式", "10", "50", "200", "50" });
                    break;

                case 6:
                    pointGgv7.AddRow(new object[] { "动作模式", "10", "50", "200", "50" });
                    break;

                case 7:
                    pointGgv8.AddRow(new object[] { "动作模式", "10", "50", "200", "50" });
                    break;

                case 8:
                    pointGgv9.AddRow(new object[] { "动作模式", "10", "50", "200", "50" });
                    break;
            }
        }

        private void btnAddSleep_Click(object sender, System.EventArgs e)
        {
            switch (uiTabControl1.SelectedIndex)
            {
                case 0:
                    pointGgv1.AddRow(new object[] { "SLEEP模式", "10", "0", "0", "0" });
                    break;

                case 1:
                    pointGgv2.AddRow(new object[] { "SLEEP模式", "10", "0", "0", "0" });
                    break;

                case 2:
                    pointGgv3.AddRow(new object[] { "SLEEP模式", "10", "0", "0", "0" });
                    break;

                case 3:
                    pointGgv4.AddRow(new object[] { "SLEEP模式", "10", "0", "0", "0" });
                    break;

                case 4:
                    pointGgv5.AddRow(new object[] { "SLEEP模式", "10", "0", "0", "0" });
                    break;

                case 5:
                    pointGgv6.AddRow(new object[] { "SLEEP模式", "10", "0", "0", "0" });
                    break;

                case 6:
                    pointGgv7.AddRow(new object[] { "SLEEP模式", "10", "0", "0", "0" });
                    break;

                case 7:
                    pointGgv8.AddRow(new object[] { "SLEEP模式", "10", "0", "0", "0" });
                    break;

                case 8:
                    pointGgv9.AddRow(new object[] { "SLEEP模式", "10", "0", "0", "0" });
                    break;
            }
        }

        private void btnSetAll_Click(object sender, System.EventArgs e)
        {
            var index = uiTabControl1.SelectedIndex;

            var pointGgvControl = Controls.Find(string.Format("pointGgv{0}", index + 1), true);
            if (pointGgvControl.Length > 0)
            {
                var pointGgv = pointGgvControl[0] as UIDataGridView;

                var allCyclePara = new HvsmEmcConfig.DeviceConfigSingleCycleParaCyclePara[pointGgv.Rows.Count];

                for (int i = 0; i < pointGgv.Rows.Count; i++)
                {
                    var row = pointGgv.Rows[i];

                    var mode = row.Cells[0].Value.ToString().ToLower().Contains("sleep") ? 0 : 1;
                    var time = double.Parse(row.Cells[1].Value.ToString());
                    var hsd_duty = int.Parse(row.Cells[2].Value.ToString());
                    var hsd_freq = int.Parse(row.Cells[3].Value.ToString());
                    var fan_duty = int.Parse(row.Cells[4].Value.ToString());

                    allCyclePara[i] = new HvsmEmcConfig.DeviceConfigSingleCycleParaCyclePara();
                    allCyclePara[i].Mode = mode;
                    allCyclePara[i].Time = time;
                    allCyclePara[i].Hsd = new HvsmEmcConfig.DeviceConfigSingleCycleParaCycleParaHsd();
                    allCyclePara[i].Hsd.Duty = hsd_duty;
                    allCyclePara[i].Hsd.Freq = hsd_freq;
                    allCyclePara[i].Fan = new HvsmEmcConfig.DeviceConfigSingleCycleParaCycleParaFan();
                    allCyclePara[i].Fan.Duty = fan_duty;
                }

                using (var frm = new FrmCycleAllSet(allCyclePara))
                {
                    var result = frm.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        var dutCount = _cyclePara.Length;
                        _cyclePara = new HvsmEmcConfig.DeviceConfigSingleCycleParaCyclePara[dutCount][];
                        for (int kk = 0; kk < dutCount; kk++)
                        {
                            _cyclePara[kk] = new HvsmEmcConfig.DeviceConfigSingleCycleParaCyclePara[frm._allPara.Length];

                            for (int i = 0; i < frm._allPara.Length; i++)
                            {
                                var mode = frm._allPara[i].Mode;
                                var time = frm._allPara[i].Time;
                                var hsd_duty = frm._allPara[i].Hsd.Duty;
                                var hsd_freq = frm._allPara[i].Hsd.Freq;
                                var fan_duty = frm._allPara[i].Fan.Duty;

                                _cyclePara[kk][i] = new HvsmEmcConfig.DeviceConfigSingleCycleParaCyclePara();
                                _cyclePara[kk][i].Mode = mode;
                                _cyclePara[kk][i].Time = time;
                                _cyclePara[kk][i].Hsd = new HvsmEmcConfig.DeviceConfigSingleCycleParaCycleParaHsd();
                                _cyclePara[kk][i].Hsd.Duty = hsd_duty;
                                _cyclePara[kk][i].Hsd.Freq = hsd_freq;
                                _cyclePara[kk][i].Fan = new HvsmEmcConfig.DeviceConfigSingleCycleParaCycleParaFan();
                                _cyclePara[kk][i].Fan.Duty = fan_duty;
                            }
                        }

                        InitDgv(pointGgv1, 0, false);
                        InitDgv(pointGgv2, 1, false);
                        InitDgv(pointGgv3, 2, false);
                        InitDgv(pointGgv4, 3, false);
                        InitDgv(pointGgv5, 4, false);
                        InitDgv(pointGgv6, 5, false);
                        InitDgv(pointGgv7, 6, false);
                        InitDgv(pointGgv8, 7, false);
                        InitDgv(pointGgv9, 8, false);
                    }
                }
            }
        }
    }
}
