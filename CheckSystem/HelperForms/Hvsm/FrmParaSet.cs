using Sunny.UI;
using System.Windows.Forms;

namespace CheckSystem.HelperForms.Hvsm
{
    public partial class FrmParaSet : UIForm
    {
        public HvsmEmcConfig.DeviceConfigPara[] _deviceConfigParas;

        public FrmParaSet(string filePath, HvsmEmcConfig.DeviceConfigPara[] deviceConfigParas)
        {
            _deviceConfigParas = deviceConfigParas;
            InitializeComponent();
            Load += FrmParaSet_Load;
        }

        private void FrmParaSet_Load(object sender, System.EventArgs e)
        {
            InitDgv();
        }

        private void InitDgv()
        {
            pointGgv.Width = Width;
            pointGgv.Style = UIStyle.Gray;
            pointGgv.AllowUserToAddRows = false;
            pointGgv.AllowUserToResizeRows = false;
            pointGgv.AllowUserToDeleteRows = false;
            pointGgv.MultiSelect = true;
            pointGgv.RowHeadersVisible = true;
            pointGgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            pointGgv.AddColumn("Name", "Name", readOnly: true);
            pointGgv.AddColumn("Min", "Min", readOnly: false);
            pointGgv.AddColumn("Max", "Max", readOnly: false);

            for (var i = 0; i < _deviceConfigParas.Length; i++)
                pointGgv.AddRow(_deviceConfigParas[i].Name, _deviceConfigParas[i].Min, _deviceConfigParas[i].Max);
            pointGgv.CellValidating += PointGgv_CellValidating;
        }

        private void PointGgv_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            var cell = pointGgv[e.ColumnIndex, e.RowIndex];

            if (cell == null)
                return;

            try
            {
                if (e.ColumnIndex == 1 || e.ColumnIndex == 2)
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
                            var maxValue = double.Parse(pointGgv[2, e.RowIndex].Value.ToString());

                            if (newValue >= maxValue)
                            {
                                e.Cancel = true;
                                this.ShowErrorDialog(string.Format("第{0}行, 最小值={1}，最大值={2},最小值不能超过最大值", e.RowIndex + 1, newValue, maxValue));
                                pointGgv.CurrentCell = cell;
                                pointGgv.Focus();
                                pointGgv.CancelEdit();
                            }
                        }
                        else if (e.ColumnIndex == 2)
                        {
                            var minValue = double.Parse(pointGgv[1, e.RowIndex].Value.ToString());

                            if (newValue <= minValue)
                            {
                                e.Cancel = true;
                                this.ShowErrorDialog(string.Format("第{0}行, 最小值={1}，最大值={2},最大值不能低于最小值", e.RowIndex + 1, minValue, newValue));
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
            pointGgv.EndEdit();
            if (this.ShowAskDialog("是否确定更新？"))
            {
                for (int i = 0; i < pointGgv.Rows.Count; i++)
                {
                    var row = pointGgv.Rows[i];
                    _deviceConfigParas[i].Min = double.Parse(row.Cells[1].Value.ToString());
                    _deviceConfigParas[i].Max = double.Parse(row.Cells[2].Value.ToString());
                }

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }
    }
}
