using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CommonUtility.FileOperator;
using Controller;
using HZH_Controls.IconFont;

namespace CheckSystem.RobotForms
{
    public partial class RobotFormPalletManagement : Form
    {
        private EditType _editType { get; set; }
        private RobotControllerPpMode RobotController { get; set; }
        public static int SelectedPalletIndex { get; set; }

        public RobotFormPalletManagement(RobotControllerPpMode robotController, string newBlock = "", int palletIndex = -1)
        {
            InitializeComponent();
            Icon = FontImages.GetIcon(FontIcons.E_arrow_move, 32,
                Color.DodgerBlue);

            Text = palletDgv.label.Text = @"点位信息管理";
            lblCmb.LabelString = @"Pallet选择";
            lblCmb.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            palletDgv.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Block" });
            palletDgv.dataGridView.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8, FontStyle.Regular);
            palletDgv.dataGridView.AllowUserToAddRows = false;
            palletDgv.dataGridView.AllowUserToDeleteRows = false;
            palletDgv.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            palletDgv.dataGridView.AllowUserToResizeColumns = true;
            palletDgv.dataGridView.AllowUserToResizeRows = false;
            palletDgv.dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            palletDgv.dataGridView.BackgroundColor = SystemColors.ActiveCaption;
            palletDgv.dataGridView.ReadOnly = true;
            palletDgv.dataGridView.RowHeadersVisible = true;
            palletDgv.dataGridView.MultiSelect = false;
            for (var i = 0; i < palletDgv.dataGridView.Columns.Count; i++)
                palletDgv.dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

            palletDgv.dataGridView.CellPainting += dataGridView_CellPainting;
            palletDgv.dataGridView.CellContentDoubleClick += dataGridView_CellContentClick;

            RobotController = robotController;
            if (string.IsNullOrEmpty(newBlock))
            {
                _editType = EditType.Mgr;

                btnAdd.Enabled = false;
                btnInsert.Enabled = false;
                btnUpdate.Enabled = false;
            }
            else
            {
                _editType = EditType.Edit;
                //lblCmb.comboBox.Enabled = false;

                lblCode.textBox.Text = newBlock;

                //if (RobotController != null && RobotController.RobotJogging != null && RobotController.RobotJogging.RobotPallets != null)
                //{
                //    var pallet =
                //        RobotController.RobotJogging.RobotPallets.ToList()
                //            .Find(f => f.PattetIndex != null && f.PattetIndex == palletIndex.ToString());
                //    if (pallet != null && pallet.Blocks != null)
                //    {
                //        foreach (var block in pallet.Blocks)
                //        {
                //            var newRow = palletDgv.dataGridView.Rows[palletDgv.dataGridView.Rows.Add()];
                //            newRow.Cells[0].Value = block;
                //        }
                //    }
                //}
            }

            if (RobotController != null &&
                RobotController.RobotJogging != null &&
                RobotController.RobotJogging.RobotPallets != null)
            {
                foreach (var t in RobotController.RobotJogging.RobotPallets.OrderBy(o => int.Parse(o.PattetIndex)))
                    lblCmb.comboBox.Items.Add(string.Format("[{0}]:[{1}]", t.PattetIndex, t.PattetName));

                lblCmb.comboBox.SelectedIndexChanged += comboBox_SelectedIndexChanged;
                if (lblCmb.comboBox.Items.Count > 0)
                    lblCmb.comboBox.SelectedIndex = 0;

                if (palletIndex != -1)
                    lblCmb.comboBox.SelectedIndex = palletIndex;
            }
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex != 0)
                return;
            using (var frm = new RobotControllerProgramUpdate(
                palletDgv.dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString()))
                if (frm.ShowDialog() == DialogResult.OK)
                    palletDgv.dataGridView.Rows[e.RowIndex].Cells[0].Value = frm.UpdatedCode;
        }

        private void comboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(lblCmb.comboBox.Text))
                return;

            palletDgv.dataGridView.Rows.Clear();

            if (RobotController != null &&
                RobotController.RobotJogging != null &&
                RobotController.RobotJogging.RobotPallets != null)
            {
                var pallet =
                    RobotController.RobotJogging.RobotPallets.ToList()
                        .Find(f => f.PattetIndex != null && f.PattetIndex == lblCmb.comboBox.SelectedIndex.ToString());
                if (pallet != null && pallet.Blocks != null)
                {
                    if (RobotController.PalletDictionary.ContainsKey(lblCmb.comboBox.SelectedIndex))
                    {
                        BackColor = !RobotController.PalletDictionary[lblCmb.comboBox.SelectedIndex].IsRunning
                               ? Color.DarkGray
                               : Color.DarkGreen;

                        foreach (var block in pallet.Blocks)
                        {
                            var rowAdd = palletDgv.dataGridView.Rows.Add();
                            var newRow = palletDgv.dataGridView.Rows[rowAdd];
                            newRow.Cells[0].Value = block;

                            if (rowAdd == RobotController.PalletDictionary[lblCmb.comboBox.SelectedIndex].BlockIndex)
                                newRow.DefaultCellStyle.BackColor = Color.DarkGreen;
                        }
                    }
                }
            }
        }

        public sealed override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private void dataGridView_CellPainting(
            object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != -1)
                return;

            using (
                Brush gridBrush = new SolidBrush(palletDgv.dataGridView.GridColor),
                    backColorBrush = new SolidBrush(e.CellStyle.BackColor))
            {
                using (var gridLinePen = new Pen(gridBrush, 2))
                {
                    // Erase the cell.
                    e.Graphics.FillRectangle(backColorBrush, e.CellBounds);

                    //划线
                    var p1 = new Point(e.CellBounds.Left + e.CellBounds.Width, e.CellBounds.Top);
                    var p2 = new Point(e.CellBounds.Left + e.CellBounds.Width, e.CellBounds.Top + e.CellBounds.Height);
                    var p3 = new Point(e.CellBounds.Left, e.CellBounds.Top + e.CellBounds.Height);
                    var ps = new[] { p1, p2, p3 };
                    e.Graphics.DrawLines(gridLinePen, ps);

                    //画字符串
                    e.Graphics.DrawString(
                        string.Format("P{0}", e.RowIndex),
                        e.CellStyle.Font, Brushes.Blue,
                        e.CellBounds.Left + 20, e.CellBounds.Top, StringFormat.GenericDefault);
                    e.Handled = true;
                }
            }
        }

        internal enum EditType
        {
            Mgr,

            Edit
        }

        private void btnAdd_BtnClick(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(lblCode.Text))
                return;

            if (!RobotControllerMainForm.ShowBox())
                return;

            var newRow = palletDgv.dataGridView.Rows[palletDgv.dataGridView.Rows.Add()];
            newRow.Cells[0].Value = lblCode.Text;
        }

        private void btnInsert_BtnClick(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(lblCode.Text))
                return;

            if (!RobotControllerMainForm.ShowBox())
                return;

            if (palletDgv.dataGridView.CurrentRow != null && palletDgv.dataGridView.CurrentRow.Index != -1)
            {
                palletDgv.dataGridView.Rows.Insert(palletDgv.dataGridView.CurrentRow.Index, 1);
                var newRow = palletDgv.dataGridView.Rows[palletDgv.dataGridView.CurrentRow.Index - 1];
                newRow.Cells[0].Value = lblCode.Text;
            }
        }

        private void btnUpdate_BtnClick(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(lblCode.Text))
                return;

            if (!RobotControllerMainForm.ShowBox())
                return;

            if (palletDgv.dataGridView.CurrentRow != null && palletDgv.dataGridView.CurrentRow.Index != -1)
            {
                var newRow = palletDgv.dataGridView.Rows[palletDgv.dataGridView.CurrentRow.Index];
                newRow.Cells[0].Value = lblCode.Text;
            }
        }

        private void btnDelete_BtnClick(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(lblCode.Text))
                return;

            if (!RobotControllerMainForm.ShowBox())
                return;

            if (palletDgv.dataGridView.CurrentRow != null && palletDgv.dataGridView.CurrentRow.Index != -1)
            {
                palletDgv.dataGridView.Rows.RemoveAt(palletDgv.dataGridView.CurrentRow.Index);
            }
        }

        private void btnSave_BtnClick(object sender, System.EventArgs e)
        {
            if (!RobotControllerMainForm.ShowBox())
                return;

            var palletIndex = lblCmb.comboBox.SelectedIndex;

            if (RobotController != null && RobotController.RobotJogging != null && RobotController.RobotJogging.RobotPallets != null)
            {
                var temp = RobotController.RobotJogging.RobotPallets.ToList();

                var pallet = temp.Find(f => f.PattetIndex != null && f.PattetIndex == palletIndex.ToString());
                if (pallet != null)
                {
                    pallet.Blocks = new string[palletDgv.dataGridView.RowCount];
                    for (var i = 0; i < palletDgv.dataGridView.RowCount; i++)
                        pallet.Blocks[i] = palletDgv.dataGridView.Rows[i].Cells[0].Value.ToString();
                }

                RobotController.RobotJogging.RobotPallets = temp.ToArray();
                XmlHelper.SerializeToFile(RobotController.RobotJogging, RobotControllerPpMode.ConfilePath,
                    Encoding.UTF8);

                RobotController.ReLoadPallet();
            }
        }

        private void btnOk_BtnClick(object sender, System.EventArgs e)
        {
            SelectedPalletIndex = lblCmb.comboBox.SelectedIndex;
            var temp = RobotController.RobotJogging.RobotPallets.ToList();

            var pallet = temp.Find(f => f.PattetIndex != null && f.PattetIndex == SelectedPalletIndex.ToString());
            if (pallet != null && pallet.Blocks != null && pallet.Blocks.Any() && RobotControllerMainForm.ShowBox())
                DialogResult = DialogResult.OK;
        }
    }
}
