using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Sunny.UI;

namespace CheckSystem.Yfas.Config.Prodcut
{
    public partial class YfasDetectionBaseList : UIPage
    {
        private readonly List<Utility._3TierModel.YfasDetectionBase> _yfasDetectionBaseModels = new List<Utility._3TierModel.YfasDetectionBase>();

        public YfasDetectionBaseList()
        {
            InitializeComponent();

            uiDataGridView1.AllowUserToAddRows = false;
            uiDataGridView1.AllowUserToDeleteRows = false;
            uiDataGridView1.AllowUserToResizeColumns = true;
            uiDataGridView1.AllowUserToResizeRows = true;
            uiDataGridView1.RowHeadersVisible = true;
            uiDataGridView1.AllowUserToOrderColumns = false;
            uiDataGridView1.ReadOnly = true;
            uiDataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            uiDataGridView1.CellMouseUp += dataGridView1_CellMouseUp;
            Load += YfasProductListPage_Load;
        }

        private void YfasProductListPage_Load(object sender, EventArgs e)
        {
            LoadProducts();
        }

        private void LoadProducts()
        {
            _yfasDetectionBaseModels.Clear();
            uiDataGridView1.ClearRows();

            var bll = new Utility._3TierBll.YfasDetectionBase();
            _yfasDetectionBaseModels.AddRange(bll.GetModelList("").OrderBy(f => f.Row));

            // 需要优化 if(_yfasDetectionBaseModels.count==0)
            uiDataGridView1.DataSource = _yfasDetectionBaseModels;
            uiDataGridView1.AutoResizeRows();
        }

        /// <summary>
        /// dataGridView选中一行时右键出现菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == -1)
                {
                    uiDataGridView1.ClearSelection();
                    //uiDataGridView1.CurrentRow = uiDataGridView1.Rows[e.RowIndex];
                    uiDataGridView1.Rows[e.RowIndex].Selected = true;
                    uiDataGridView1.CurrentCell = uiDataGridView1.Rows[e.RowIndex].Cells[0];
                    btnAdd.Visible = false;
                    btnDelete.Visible = true;
                    btnEdit.Visible = true;
                    uiContextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                }
                else
                {
                    uiDataGridView1.ClearSelection();
                    btnAdd.Visible = true;
                    btnDelete.Visible = false;
                    btnEdit.Visible = false;
                    uiContextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var rows = new[] { "ROW1", "ROW2", "ROW3" };

            var option = new UIEditOption
            {
                AutoLabelWidth = true,
                Text = "添加"
            };
            option.AddText("Name", "Name", string.Empty, true);
            option.AddCombobox("Row", "Row", rows, 0, true, true);
            option.AddText("CarModel", "CarModel", string.Empty, true);

            var frm = new UIEditForm(option);
            frm.ShowDialog();

            if (frm.IsOK)
            {
                var name = frm["Name"].ToString();
                var row = rows[(int) frm["Row"]];
                var camModel = frm["CarModel"].ToString();
                var bll = new Utility._3TierBll.YfasDetectionBase();
                bll.Add(new Utility._3TierModel.YfasDetectionBase
                {
                    CarModel = camModel,
                    DetectionName = name,
                    IsDelete = 0,
                    Row = row
                });

                LoadProducts();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (uiDataGridView1.CurrentRow == null) return;
            var rowIndex = uiDataGridView1.CurrentRow.Index;

            if (rowIndex < 0) return;
            var id = uiDataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
            this.ShowErrorDialog("将删除：" + id);
            LoadProducts();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

        }
    }
}
