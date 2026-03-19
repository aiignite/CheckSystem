using Sunny.UI;
using System.Windows.Forms;

namespace CheckSystem.HelperForms.Hvsm
{
    public partial class FrmCycleAllSet : UIForm
    {
        public HvsmEmcConfig.DeviceConfigSingleCycleParaCyclePara[] _allPara;

        public FrmCycleAllSet(HvsmEmcConfig.DeviceConfigSingleCycleParaCyclePara[] allPara)
        {
            _allPara = allPara;
            InitializeComponent();
            Load += FrmCycleAllSet_Load;
        }

        private void FrmCycleAllSet_Load(object sender, System.EventArgs e)
        {
            InitDgv(pointGgv);
        }

        private void InitDgv(UIDataGridView pointGgv)
        {
            pointGgv.Width = Width;
            pointGgv.Style = UIStyle.Gray;
            pointGgv.AllowUserToAddRows = false;
            pointGgv.AllowUserToResizeRows = true;
            pointGgv.AllowUserToDeleteRows = true;
            pointGgv.MultiSelect = true;
            pointGgv.RowHeadersVisible = true;
            pointGgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            pointGgv.AddColumn("Name", "Name", readOnly: true);
            pointGgv.AddColumn("Time/S", "Time", readOnly: false);
            pointGgv.AddColumn("HSD_Duty", "HSD_Duty", readOnly: false);
            pointGgv.AddColumn("HSD_Freq", "HSD_Freq", readOnly: false);
            pointGgv.AddColumn("FAN_Duty", "FAN_Duty", readOnly: false);

            for (int i = 0; i < _allPara.Length; i++)
            {
                var mode = _allPara[i].Mode == 0 ? "SLEEP模式" : "动作模式";
                var time = _allPara[i].Time;
                var hsd_duty = _allPara[i].Hsd.Duty.ToString();
                var hsd_freq = _allPara[i].Hsd.Freq.ToString();
                var fan_duty = _allPara[i].Fan.Duty.ToString();
                pointGgv.AddRow(new object[] { mode, time, hsd_duty, hsd_freq, fan_duty });
            }

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

        private void btnAddSleep_Click(object sender, System.EventArgs e)
        {
            pointGgv.AddRow(new object[] { "SLEEP模式", "10", "0", "0", "0" });
        }

        private void btnAddStandby_Click(object sender, System.EventArgs e)
        {
            pointGgv.AddRow(new object[] { "动作模式", "10", "50", "200", "50" });
        }

        private void btnUptade_Click(object sender, System.EventArgs e)
        {
            if (pointGgv.RowCount < 1)
            {
                this.ShowErrorDialog("缺少测试点，请添加测试点");
                return;
            }

            if (this.ShowAskDialog("是否确定更新？"))
            {
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

                _allPara = allCyclePara;
                DialogResult = DialogResult.OK;
            }
        }
    }
}
