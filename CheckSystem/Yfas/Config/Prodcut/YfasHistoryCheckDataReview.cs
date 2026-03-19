using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CheckSystem.Yfas.Utility;
using Newtonsoft.Json;
using Sunny.UI;

namespace CheckSystem.Yfas.Config.Prodcut
{
    public partial class YfasHistoryCheckDataReview : UIForm
    {
        private readonly List<Utility._3TierModel.YfasCheckData> _yfasCheckDataModels = new List<Utility._3TierModel.YfasCheckData>();

        public YfasHistoryCheckDataReview()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;

            uiDataGridView1.AllowUserToAddRows = false;
            uiDataGridView1.AllowUserToDeleteRows = false;
            uiDataGridView1.AllowUserToResizeColumns = true;
            uiDataGridView1.AllowUserToResizeRows = true;
            uiDataGridView1.RowHeadersVisible = true;
            uiDataGridView1.AllowUserToOrderColumns = false;
            uiDataGridView1.ReadOnly = true;
            uiDataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            uiDataGridView1.CellMouseUp += uiDataGridView1_CellMouseUp;

            Load += YfasCheckDataReview_Load;
        }

        private void uiDataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                uiDataGridView1.EndEdit();

                if (e.RowIndex >= 0 && e.ColumnIndex == -1)
                {
                    uiDataGridView1.ClearSelection();
                    //uiDataGridView1.CurrentRow = uiDataGridView1.Rows[e.RowIndex];
                    uiDataGridView1.Rows[e.RowIndex].Selected = true;
                    uiDataGridView1.CurrentCell = uiDataGridView1.Rows[e.RowIndex].Cells[0];
                    btnReview.Visible = true;
                    uiContextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                }
                else
                {
                    uiDataGridView1.ClearSelection();
                    btnReview.Visible = false;
                    uiContextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }

        public void YfasCheckDataReview_Load(object sender, EventArgs e)
        {
            LoadProducts();
        }

        private void LoadProducts()
        {
            _yfasCheckDataModels.Clear();
            uiDataGridView1.ClearRows();

            var bll = new Utility._3TierBll.YfasCheckData();
            _yfasCheckDataModels.AddRange(bll.GetModelList("").OrderByDescending(f => f.createTime));

            // 需要优化 if(_yfasDetectionBaseModels.count==0)
            if (_yfasCheckDataModels.Count > 0)
            {
                uiDataGridView1.DataSource = _yfasCheckDataModels;
                uiDataGridView1.AutoResizeRows();
            }
        }

        private void btnReview_Click(object sender, EventArgs e)
        {
            if (uiDataGridView1.CurrentRow == null) return;
            var rowIndex = uiDataGridView1.CurrentRow.Index;

            if (rowIndex < 0) return;

            try
            {
                var id = 0;
                List<YfasCheckStateMachine.CheckDataView> checkData = null;
                for (var i = 0; i < uiDataGridView1.ColumnCount; i++)
                {
                    if (string.Equals(uiDataGridView1.Columns[i].HeaderText, @"productno", StringComparison.CurrentCultureIgnoreCase))
                    {
                        id = int.Parse(uiDataGridView1.Rows[rowIndex].Cells[i].Value.ToString());
                    }
                    else if (string.Equals(uiDataGridView1.Columns[i].HeaderText, @"checkData", StringComparison.CurrentCultureIgnoreCase))
                    {
                        checkData =
                            JsonConvert.DeserializeObject<List<YfasCheckStateMachine.CheckDataView>>(
                                uiDataGridView1.Rows[rowIndex].Cells[i].Value.ToString());
                    }
                }

                if (checkData != null)
                {
                    var detailForm = new YfasHistoryCheckDetailReview(id, checkData);
                    detailForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                this.ShowErrorDialog("打开失败：" + ex.Message);
            }

        }
    }
}
