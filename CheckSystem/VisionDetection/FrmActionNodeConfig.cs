using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Sunny.UI;

namespace CheckSystem.VisionDetection
{
    public partial class FrmActionNodeConfig : UIForm
    {
        private UIDataGridView DetectionDataGridView { get; set; }

        public FrmActionNodeConfig(UIDataGridView dgv, TreeNode node = null)
        {
            InitializeComponent();
            DetectionDataGridView = dgv;
            InitDataGrid();

            if (node != null)
            {
                for (var i = 0; i < node.Nodes.Count; i++)
                {
                    if (node.Nodes[i].Text.StartsWith("检测："))
                    {
                        var str = node.Nodes[i].Nodes.OfType<TreeNode>().Aggregate("检测：", (current, treeNode) => current + treeNode.Text + "，");

                        str = str.TrimEnd('，');
                        DataGridAddContent(str);
                    }
                    else
                    {
                        DataGridAddContent(node.Nodes[i].Text);
                    }
                }
            }
        }

        private void InitDataGrid()
        {
            uiDataGridView1.Style = UIStyle.Gray;
            uiDataGridView1.ReadOnly = true;
            uiDataGridView1.RowHeadersVisible = false;
            uiDataGridView1.AllowUserToAddRows = false;
            uiDataGridView1.AllowUserToResizeRows = false;
            uiDataGridView1.MultiSelect = false;
            uiDataGridView1.RowHeadersVisible = true;
            uiDataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            uiDataGridView1.CellDoubleClick += uiDataGridView1_CellDoubleClick;

            uiDataGridView1.AddButtonColumn("↑", "UP", 20);
            uiDataGridView1.AddButtonColumn("↓", "Down", 20);
            uiDataGridView1.AddColumn("执行内容", "Content");
            uiDataGridView1.AddColumn("L/R", "L/R");
            //uiDataGridView1.AddColumn("执行后检测", "Detection");
            uiDataGridView1.AutoResizeColumns();

            //for (var i = 0; i < 10; i++)
            //    uiDataGridView1.AddRow("↑", "↓", "内容XXXXXXXXXXXXXXXXXXXXXXX：" + (i + 1));

            //uiDataGridView1.AutoResizeRows();
        }

        void uiDataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2 && e.RowIndex > -1 && e.RowIndex <= uiDataGridView1.RowCount - 1)
            {
                var cellValue = uiDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                var checkContent = cellValue == null ? string.Empty : cellValue.ToString();

                if (!string.IsNullOrEmpty(checkContent) && checkContent.StartsWith("检测："))
                {
                    UpdateCheck(checkContent, e.RowIndex, e.ColumnIndex);
                }
                else
                {
                    this.ShowErrorTip("请双击包含“检测：”列中的具体内容");
                }
            }
            else
            {
                this.ShowErrorTip("请双击包含“检测：”列中的具体内容");
            }
        }

        private void AddCheck()
        {
            //value = string.Empty;
            var option = new UIEditOption { AutoLabelWidth = true, Text = @"检测配置" };

            var items = new ComboCheckedListBoxItem[DetectionDataGridView.RowCount - 1];
            for (var i = 0; i < items.Length; i++)
                items[i] = new ComboCheckedListBoxItem(DetectionDataGridView.Rows[i].Cells[0].Value.ToString(),
                    false);

            option.AddComboCheckedListBox("Detection", "检测项：", items, "");

            var frmDetection = new UIEditForm(option);
            frmDetection.Render();
            frmDetection.ShowDialog();

            if (frmDetection.IsOK)
            {
                var outCheckedItems = (ComboCheckedListBoxItem[])frmDetection["Detection"];

                var addCount = 0;
                var str = string.Empty;

                foreach (var item in outCheckedItems)
                {
                    if (item.Checked)
                    {
                        addCount++;
                        str += item.Text + "，";
                    }
                }
                str = str.TrimEnd('，');

                if (addCount > 0)
                {
                    DataGridAddContent("检测：" + str);
                    uiDataGridView1.AutoResizeRows();
                    this.ShowSuccessTip("数据已更新，请保存");
                }
                else
                {
                    this.ShowErrorTip("取消了所有检测项，请确认");
                }
            }
            else
            {
                this.ShowInfoTip("用户取消操作");
            }
        }

        private void UpdateCheck(string value, int rowIndex, int cellIndex)
        {
            var checkList = new List<string>();
            var showStr = string.Empty;
            var valueSp = value.Split("：")[1].Split("，");
            for (var i = 0; i < valueSp.Length; i++)
            {
                if (!string.IsNullOrEmpty(valueSp[i]))
                {
                    checkList.Add(valueSp[i]);
                    showStr += checkList[i] + "；";
                }

            }
            showStr = showStr.TrimEnd('；');

            var option = new UIEditOption { AutoLabelWidth = true, Text = @"检测配置" };

            var items = new ComboCheckedListBoxItem[DetectionDataGridView.RowCount - 1];
            for (var i = 0; i < items.Length; i++)
            {
                var checkName = DetectionDataGridView.Rows[i].Cells[0].Value.ToString();
                items[i] = new ComboCheckedListBoxItem(checkName, checkList.Contains(checkName));
            }

            option.AddComboCheckedListBox("Detection", "检测项：", items, showStr);

            var frmDetection = new UIEditForm(option);
            frmDetection.Render();
            frmDetection.ShowDialog();

            if (frmDetection.IsOK)
            {
                var outCheckedItems = (ComboCheckedListBoxItem[])frmDetection["Detection"];

                var addCount = 0;
                var str = string.Empty;
                foreach (var item in outCheckedItems)
                {
                    if (item.Checked)
                    {
                        addCount++;
                        str += item.Text + "，";
                    }
                }
                str = str.TrimEnd('，');

                if (addCount > 0)
                {
                    uiDataGridView1.Rows[rowIndex].Cells[cellIndex].Value = "检测：" + str;
                    uiDataGridView1.AutoResizeRows();
                    this.ShowSuccessTip("数据已更新，请保存");
                }
                else
                {
                    uiDataGridView1.Rows.RemoveAt(rowIndex);
                    this.ShowErrorTip("取消了所有检测项，请确认");
                }
            }
            else
            {
                this.ShowInfoTip("用户取消操作");
            }
        }

        private void DataGridAddContent(string value, string detection = "")
        {
            uiDataGridView1.AddRow("↑", "↓", value, value.StartsWith("检测：") ? "/" : "L&R", detection);
            uiDataGridView1.AutoResizeColumns();
            uiDataGridView1.AutoResizeRows();
        }

        private void btnAddRelayAction_Click(object sender, EventArgs e)
        {
            var option = new UIEditOption { AutoLabelWidth = true };
            var relays = new string[50];
            for (var i = 0; i < relays.Length; i++)
                relays[i] = string.Format("继电器{0}", i + 1);

            option.AddCombobox("Relay", "继电器：", relays, 0, true, true);
            option.AddSwitch("Switch", "打开/关闭：", true, "打开", "关闭");

            var frmRelay = new UIEditForm(option);
            frmRelay.Render();
            frmRelay.ShowDialog();

            if (frmRelay.IsOK)
            {
                var relay = relays[(int)frmRelay["Relay"]];
                var @switch = string.Equals(frmRelay["Switch"].ToString(), true.ToString(), StringComparison.CurrentCultureIgnoreCase) ? "ON" : "OFF";

                var content = string.Format("{0}：{1}", relay, @switch);
                DataGridAddContent(content);
            }
        }

        private void btnAddDelayAction_Click(object sender, EventArgs e)
        {
            var option = new UIEditOption { AutoLabelWidth = true };
            option.AddInteger("Delay", "延时/ms：", 100);

            var frmDelay = new UIEditForm(option);
            frmDelay.Render();
            frmDelay.ShowDialog();

            if (frmDelay.IsOK)
            {
                var delay = frmDelay["Delay"];

                var content = string.Format("延时：{0}", delay);
                DataGridAddContent(content);
            }
        }

        private void btnAddMethodAction_Click(object sender, EventArgs e)
        {
            var option = new UIEditOption
            {
                AutoLabelWidth = true,
                Text = "添加函数"
            };
            option.AddText("MethodContent", "函数内容", null, true);

            var frmAddMethod = new UIEditForm(option);
            frmAddMethod.Render();
            frmAddMethod.ShowDialog();

            if (frmAddMethod.IsOK)
            {
                var content = string.Format("函数：{0}", frmAddMethod["MethodContent"]);
                DataGridAddContent(content);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (uiDataGridView1.RowCount > 0)
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                this.ShowErrorTip("请添加动作逻辑");
            }
        }

        private void btnAddCheck_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < uiDataGridView1.RowCount; i++)
            {
                var rowValue = uiDataGridView1.Rows[i].Cells[2].Value;

                if (rowValue == null || string.IsNullOrEmpty(rowValue.ToString()) ||
                    !rowValue.ToString().StartsWith("检测：")) continue;
                this.ShowErrorTip("已有检测动作");
                return;
            }
            AddCheck();
        }
    }
}
