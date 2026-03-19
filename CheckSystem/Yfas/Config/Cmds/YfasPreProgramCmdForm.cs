using System.Collections.Generic;
using System.Windows.Forms;
using Sunny.UI;

namespace CheckSystem.Yfas.Config.Cmds
{
    public sealed partial class YfasPreProgramCmdForm : UIForm
    {
        private readonly Utility._3TierModel.YfasPreProgramAction _yfasPreProgramAction;
        private readonly List<Utility._3TierModel.YfasPreProgramActCmd> _yfasPreProgramActCmdModels = new List<Utility._3TierModel.YfasPreProgramActCmd>();

        public YfasPreProgramCmdForm(Utility._3TierModel.YfasPreProgramAction action)
        {
            InitializeComponent();
            _yfasPreProgramAction = action;
            Text = @"动作名称：" + _yfasPreProgramAction.Name;
            Load += YfasPreProgramActCmdForm_Load;
            uiDataGridView1.UserDeletedRow += uiDataGridView1_UserDeletedRow;
        }

        private void uiDataGridView1_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            var id = e.Row.Cells[0].Value.ToString();
            this.ShowErrorDialog(id);
        }

        private void YfasPreProgramActCmdForm_Load(object sender, System.EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {

            _yfasPreProgramActCmdModels.Clear();

            uiDataGridView1.AllowUserToDeleteRows = true;
            uiDataGridView1.AllowUserToAddRows = true;
            uiDataGridView1.AllowUserToResizeColumns = true;
            uiDataGridView1.AllowUserToResizeRows = true;
            uiDataGridView1.RowHeadersVisible = true;
            uiDataGridView1.AllowUserToOrderColumns = false;
            uiDataGridView1.ReadOnly = true;
            uiDataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            var bll = new Utility._3TierBll.YfasPreProgramActCmd();
            _yfasPreProgramActCmdModels.AddRange(bll.GetModelList("ActionId = '" + _yfasPreProgramAction.id + "'"));

            uiDataGridView1.ClearRows();
            uiDataGridView1.DataSource = _yfasPreProgramActCmdModels;
            //uiDataGridView1.AddButtonColumn("编辑", "编辑");

            uiDataGridView1.AutoResizeRows();
        }

        private void tsmAdd_Click(object sender, System.EventArgs e)
        {
            var controlls = new[] { "-1", "0", "1", "2", "3", "4", "5" };
            var type = new[] {"SetField", "InvokeMethod"};
            var cmdType = new[] { "100000", "200000", "300000", "400000", "500000", "600000" };

            var option = new UIEditOption
            {
                AutoLabelWidth = true,
                Text = "添加"
            };
            option.AddCombobox("Controller", "Controller", controlls, 0, true, true);
            option.AddCombobox("type", "类型", type, 0, true, true);
            option.AddText("TargetName", "TargetName", string.Empty, true);
            option.AddText("TargetParas", "TargetParas", string.Empty, false);
            option.AddCombobox("CmdType", "指令类别", cmdType, 0, true, true);

            var frm = new UIEditForm(option);
            frm.Render();
            //frm.CheckedData += frm_CheckedData;
            frm.ShowDialog();

            if (!frm.IsOK)
                return;

            var model = new Utility._3TierModel.YfasPreProgramActCmd
            {
                ActionId = _yfasPreProgramAction.id,
                Type = (int)frm["type"],
                ControllerId = int.Parse(controlls[(int)frm["Controller"]]),
                TargetName = frm["TargetName"].ToString(),
                TargetParas = frm["TargetParas"].ToString(),
                CmdType = int.Parse(cmdType[(int)frm["CmdType"]]),
                IsDelete = 0
            };

            var bll = new Utility._3TierBll.YfasPreProgramActCmd();
            bll.Add(model);
            LoadData();
        }
    }
}
