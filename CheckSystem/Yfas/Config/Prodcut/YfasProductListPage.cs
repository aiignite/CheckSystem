using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CheckSystem.Yfas.Config.TestFlow;
using Sunny.UI;

namespace CheckSystem.Yfas.Config.Prodcut
{
    public partial class YfasProductListPage : UIPage
    {
        private readonly List<Utility._3TierModel.YfasProductInfo> _yfasProductListModels = new List<Utility._3TierModel.YfasProductInfo>();

        public YfasProductListPage()
        {
            InitializeComponent();

            //uiDataGridView1.AutoGenerateColumns = false;
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
            _yfasProductListModels.Clear();
            uiDataGridView1.ClearRows();

            var bll = new Utility._3TierBll.YfasProductInfo();
            _yfasProductListModels.AddRange(bll.GetModelList(""));
            uiDataGridView1.DataSource = _yfasProductListModels;

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
                    btnEditParas.Visible = true;
                    btnEditTestFlow.Visible = true;
                    uiContextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                }
                else
                {
                    uiDataGridView1.ClearSelection();
                    btnAdd.Visible = true;
                    btnDelete.Visible = false;
                    btnEditParas.Visible = false;
                    btnEditTestFlow.Visible = false;
                    uiContextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var rows = new[] { "Row1", "Row2", "Row3" };
            var pos = new[] { "DR", "PA", "2L", "2R", "3L", "3R", "3L60%", "3R40%" };

            var option = new UIEditOption
            {
                AutoLabelWidth = true,
                Text = "添加"
            };
            option.AddText("Name", "Name", string.Empty, true);
            option.AddText("YfasPn", "YfasPn", string.Empty, true);
            option.AddText("GmPn", "GmPn", string.Empty, true);
            option.AddCombobox("Row", "Row", rows, 0, true, true);
            option.AddCombobox("Pos", "Pos", pos, 0, true, true);
            option.AddText("CarModel", "CarModel", string.Empty, true);
            option.AddText("Barcode", "Barcode", string.Empty, true);

            var frm = new UIEditForm(option);
            frm.ShowDialog();

            if (frm.IsOK)
            {
                var model = new Utility._3TierModel.YfasProductInfo
                {
                    Name = frm["Name"].ToString(),
                    YfasPn = frm["YfasPn"].ToString(),
                    GmPn = frm["GmPn"].ToString(),
                    RowIndex = rows[(int)frm["Row"]],
                    Pos = pos[(int)frm["Pos"]],
                    CarMode = frm["CarModel"].ToString(),
                    Barcode = frm["Barcode"].ToString()
                };
                var bll = new Utility._3TierBll.YfasProductInfo();
                bll.Add(model);
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

        private void btnEditParas_Click(object sender, EventArgs e)
        {
            if (uiDataGridView1.CurrentRow == null) return;
            var rowIndex = uiDataGridView1.CurrentRow.Index;

            if (rowIndex < 0) return;
            var id = uiDataGridView1.Rows[rowIndex].Cells[0].Value.ToString();

            var form = new YfasProductParaEditForm(int.Parse(id));
            form.ShowDialog();
            LoadProducts();
        }

        private void btnEditTestFlow_Click(object sender, EventArgs e)
        {
            if (uiDataGridView1.CurrentRow == null) return;
            var rowIndex = uiDataGridView1.CurrentRow.Index;

            if (rowIndex < 0) return;
            var id = uiDataGridView1.Rows[rowIndex].Cells[0].Value.ToString();

            var testFlowPage = new YfasTestFlowMainForm(int.Parse(id));
            testFlowPage.ShowDialog();

            LoadProducts();
        }
    }
}
