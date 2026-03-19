using System.Collections.Generic;
using System.Windows.Forms;
using Sunny.UI;

namespace CheckSystem.Yfas.Config.Cmds
{
    public partial class YfasPreProgramPage : UIPage
    {
        private readonly List<Utility._3TierModel.YfasPreProgramAction> _yfasPreProgramActionModels = new List<Utility._3TierModel.YfasPreProgramAction>();

        public YfasPreProgramPage()
        {
            InitializeComponent();
            Load += YfasPreProgramActionPage_Load;
            uiDataGridView1.CellPainting += uiDataGridView1_CellPainting;
            //uiDataGridView1.CellClick += uiDataGridView1_CellClick;
            uiDataGridView1.CellContentClick += uiDataGridView1_CellContentClick;
        }

        private void YfasPreProgramActionPage_Load(object sender, System.EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            _yfasPreProgramActionModels.Clear();

            uiDataGridView1.AllowUserToAddRows = false;
            uiDataGridView1.AllowUserToDeleteRows = false;
            uiDataGridView1.AllowUserToResizeColumns = true;
            uiDataGridView1.AllowUserToResizeRows = true;
            uiDataGridView1.RowHeadersVisible = true;
            uiDataGridView1.AllowUserToOrderColumns = false;
            uiDataGridView1.ReadOnly = true;
            uiDataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            var bll = new Utility._3TierBll.YfasPreProgramAction();
            _yfasPreProgramActionModels.AddRange(bll.GetModelList(""));

            uiDataGridView1.ClearAll();
            uiDataGridView1.DataSource = _yfasPreProgramActionModels;
            uiDataGridView1.AddButtonColumn("编辑", "编辑");

            uiDataGridView1.AutoResizeRows();
        }

        private void uiDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (uiDataGridView1.Columns[e.ColumnIndex].HeaderText != @"编辑")
                return;
            //var id = _yfasPreProgramActionModels[e.RowIndex].id;
            var cmdForm = new YfasPreProgramCmdForm(_yfasPreProgramActionModels[e.RowIndex]);
            cmdForm.ShowDialog();
            LoadData();
        }

        public void uiDataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (uiDataGridView1.Columns[e.ColumnIndex].HeaderText == @"编辑")
                uiDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "Edit";
        }

        private void tsmAdd_Click(object sender, System.EventArgs e)
        {
            var type = new[] { "100000", "200000", "300000", "400000", "500000", "600000" };
            var row = new[] { "ROW1", "ROW2", "ROW3" };

            var option = new UIEditOption
            {
                AutoLabelWidth = true,
                Text = "添加动作"
            };
            option.AddText("Name", "名称", string.Empty, true);
            //option.AddCombobox("Row", "ROW", row, 0, true, true);
            //option.AddText("CarModel", "CarModel", string.Empty, true);
            option.AddCombobox("Type", "类型", type, 0, true, true);

            var frm = new UIEditForm(option);
            frm.Render();
            frm.CheckedData += frm_CheckedData;
            frm.ShowDialog();

            if (!frm.IsOK) 
                return;

            var model = new Utility._3TierModel.YfasPreProgramAction
            {
                ActionType = int.Parse(type[(int)frm["Type"]]),
                //CarModel = frm["CarModel"].ToString(),
                //Row = row[(int)frm["Row"]],
                CarModel = "SGM458",
                Row = "ROW1",
                IsDelete = 0,
                Name = frm["Name"].ToString()
            };

            var bll = new Utility._3TierBll.YfasPreProgramAction();
            bll.Add(model);
            LoadData();
        }

        private bool frm_CheckedData(object sender, UIEditForm.EditFormEventArgs e)
        {
            if (e.Form["Name"].ToString().ToLower().Contains("OnStart".ToLower()))
            {
                e.Form.SetEditorFocus("Name");
                this.ShowWarningTip("OnStart为系统关键字，无法添加");
                return false;
            }

            if (e.Form["Name"].ToString().ToLower().Contains(" ") ||
                e.Form["Name"].ToString().ToLower().Contains("[") ||
                e.Form["Name"].ToString().ToLower().Contains("]") ||
                e.Form["Name"].ToString().ToLower().Contains(">") ||
                e.Form["Name"].ToString().ToLower().Contains("<") ||
                e.Form["Name"].ToString().ToLower().Contains(":") ||
                e.Form["Name"].ToString().ToLower().Contains(",") ||
                e.Form["Name"].ToString().ToLower().Contains("，") ||
                e.Form["Name"].ToString().ToLower().Contains(".") ||
                e.Form["Name"].ToString().ToLower().Contains("。"))
            {
                e.Form.SetEditorFocus("Name");
                this.ShowWarningTip("包含特殊符号，无法添加");
                return false;
            }

            return true;
        }
    }
}
