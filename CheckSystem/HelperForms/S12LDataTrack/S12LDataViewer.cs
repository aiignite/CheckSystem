using System;
using System.Windows.Forms;
using Sunny.UI;

namespace CheckSystem.HelperForms.S12LDataTrack
{
    public partial class S12LDataViewer : UIForm
    {
        public S12LDataViewer()
        {
            InitializeComponent();

            uiDataGridView1.Style = UIStyle.Gray;
            uiDataGridView1.ReadOnly = true;
            uiDataGridView1.RowHeadersVisible = false;
            uiDataGridView1.AllowUserToAddRows = false;
            uiDataGridView1.AllowUserToResizeRows = false;
            uiDataGridView1.AllowUserToResizeColumns = true;
            uiDataGridView1.AllowUserToDeleteRows = false;
            uiDataGridView1.MultiSelect = true;
            uiDataGridView1.RowHeadersVisible = false;
            uiDataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            uiDataGridView1.AddColumn("UID", "UID");
            uiDataGridView1.AddColumn("CreateTime", "CreateTime");
            uiDataGridView1.AddColumn("CheckResult", "CheckResult");
            uiDataGridView1.AddColumn("Creater", "Creater");
            uiDataGridView1.AddColumn("CheckData", "CheckData");
        }

        private void btnSerach_Click(object sender, EventArgs e)
        {
            uiDataGridView1.ClearRows();

            if (!string.IsNullOrEmpty(uiTextBox1.Text))
            {
                var bll = new BLL.manufactureCheckData();
                var models = bll.GetModelList("creater like '%S12L%' AND productUid LIKE  '%" + uiTextBox1.Text + "%'");

                foreach (var t in models)
                {
                    uiDataGridView1.AddRow(new object[] { t.productUid, t.createTime, t.checkResult == "0001" ? "OK" : "NG", t.creater, t.checkData });
                }
            }
        }
    }
}
