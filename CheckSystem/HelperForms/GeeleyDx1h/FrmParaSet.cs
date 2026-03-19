using Sunny.UI;
using System;
using System.Windows.Forms;

namespace CheckSystem.HelperForms.GeeleyDx1h
{
    public partial class FrmParaSet : UIForm
    {
        public Dx1hEmcConfig.DeviceConfigPara[] _p1;
        public Dx1hEmcConfig.DeviceConfigMotorPara[] _p2;

        public FrmParaSet(
            Dx1hEmcConfig.DeviceConfigPara[] p1, 
            Dx1hEmcConfig.DeviceConfigMotorPara[] p2)
        {
            _p1 = p1;
            _p2 = p2;
            InitializeComponent();
            Load += FrmParaSet_Load;
        }

        private void FrmParaSet_Load(object sender, EventArgs e)
        {
            InitDgv1();
            InitDgv2();
        }

        private void InitDgv1()
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

            for (var i = 0; i < _p1.Length; i++)
                pointGgv.AddRow(_p1[i].Name, _p1[i].Min, _p1[i].Max);
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

        private void InitDgv2()
        {
            pointGgv2.Width = Width;
            pointGgv2.Style = UIStyle.Gray;
            pointGgv2.AllowUserToAddRows = false;
            pointGgv2.AllowUserToResizeRows = false;
            pointGgv2.AllowUserToDeleteRows = false;
            pointGgv2.MultiSelect = true;
            pointGgv2.RowHeadersVisible = true;
            pointGgv2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            pointGgv2.AddColumn("Name", "Name", readOnly: true);
            pointGgv2.AddColumn("Min", "Min", readOnly: false);
            pointGgv2.AddColumn("Max", "Max", readOnly: false);

            for (var i = 0; i < _p2.Length; i++)
                pointGgv2.AddRow(_p2[i].Name, _p2[i].Min, _p2[i].Max);
            pointGgv2.CellValidating += PointGgv_CellValidating2;
        }

        private void PointGgv_CellValidating2(object sender, DataGridViewCellValidatingEventArgs e)
        {
            var cell = pointGgv2[e.ColumnIndex, e.RowIndex];

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
                        pointGgv2.CurrentCell = cell;
                        pointGgv2.Focus();
                        pointGgv2.CancelEdit();
                    }
                    else
                    {
                        var newValue = double.Parse(e.FormattedValue.ToString());

                        if (e.ColumnIndex == 1)
                        {
                            var maxValue = double.Parse(pointGgv2[2, e.RowIndex].Value.ToString());

                            if (newValue >= maxValue)
                            {
                                e.Cancel = true;
                                this.ShowErrorDialog(string.Format("第{0}行, 最小值={1}，最大值={2},最小值不能超过最大值", e.RowIndex + 1, newValue, maxValue));
                                pointGgv2.CurrentCell = cell;
                                pointGgv2.Focus();
                                pointGgv2.CancelEdit();
                            }
                        }
                        else if (e.ColumnIndex == 2)
                        {
                            var minValue = double.Parse(pointGgv2[1, e.RowIndex].Value.ToString());

                            if (newValue <= minValue)
                            {
                                e.Cancel = true;
                                this.ShowErrorDialog(string.Format("第{0}行, 最小值={1}，最大值={2},最大值不能低于最小值", e.RowIndex + 1, minValue, newValue));
                                pointGgv2.CurrentCell = cell;
                                pointGgv2.Focus();
                                pointGgv2.CancelEdit();
                            }
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                e.Cancel = true;
                this.ShowErrorDialog(string.Format("第{0}行, 输入格式错误", e.RowIndex + 1));
                pointGgv2.CurrentCell = cell;
                pointGgv2.Focus();
                pointGgv2.CancelEdit();
            }
        }

        private void btnUptade_Click(object sender, EventArgs e)
        {
            pointGgv.EndEdit();
            pointGgv2.EndEdit();

            if (this.ShowAskDialog("是否确定更新？"))
            {
                for (int i = 0; i < pointGgv.Rows.Count; i++)
                {
                    var row = pointGgv.Rows[i];
                    _p1[i].Min = double.Parse(row.Cells[1].Value.ToString());
                    _p1[i].Max = double.Parse(row.Cells[2].Value.ToString());
                }

                for (int i = 0; i < pointGgv2.Rows.Count; i++)
                {
                    var row = pointGgv2.Rows[i];
                    _p2[i].Min = double.Parse(row.Cells[1].Value.ToString());
                    _p2[i].Max = double.Parse(row.Cells[2].Value.ToString());
                }

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }
    }
}
