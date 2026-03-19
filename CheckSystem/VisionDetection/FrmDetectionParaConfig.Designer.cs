namespace CheckSystem.VisionDetection
{
    partial class FrmDetectionParaConfig
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("/");
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnConfirm = new System.Windows.Forms.ToolStripButton();
            this.btnCancel = new System.Windows.Forms.ToolStripButton();
            this.mainPanel = new Sunny.UI.UITableLayoutPanel();
            this.uiTableLayoutPanel1 = new Sunny.UI.UITableLayoutPanel();
            this.cmbBinding = new Sunny.UI.UIComboTreeView();
            this.btnPowerPara = new Sunny.UI.UISymbolButton();
            this.uiMarkLabel6 = new Sunny.UI.UIMarkLabel();
            this._txtUnit = new Sunny.UI.UITextBox();
            this.uiMarkLabel4 = new Sunny.UI.UIMarkLabel();
            this._txtDelay = new Sunny.UI.UITextBox();
            this.uiMarkLabel2 = new Sunny.UI.UIMarkLabel();
            this._txtGroupCount = new Sunny.UI.UIIntegerUpDown();
            this.uiMarkLabel17 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel15 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel13 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel11 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel9 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel7 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel5 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel3 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel1 = new Sunny.UI.UIMarkLabel();
            this._txtName = new Sunny.UI.UITextBox();
            this._cmbType = new Sunny.UI.UIComboBox();
            this._cmbLeftRightType = new Sunny.UI.UIComboBox();
            this.dgvGroupDetail = new Sunny.UI.UIDataGridView();
            this._txtRelaysOn = new Sunny.UI.UIComboTreeView();
            this._txtRelaysOff = new Sunny.UI.UIComboTreeView();
            this._txtMethodBefore = new Sunny.UI.UISymbolButton();
            this._txtMethodAfter = new Sunny.UI.UISymbolButton();
            this.toolStrip1.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.uiTableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGroupDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnConfirm,
            this.btnCancel});
            this.toolStrip1.Location = new System.Drawing.Point(0, 35);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1200, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnConfirm
            // 
            this.btnConfirm.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(36, 22);
            this.btnConfirm.Text = "确认";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(36, 22);
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // mainPanel
            // 
            this.mainPanel.ColumnCount = 2;
            this.mainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.mainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.mainPanel.Controls.Add(this.uiTableLayoutPanel1, 0, 0);
            this.mainPanel.Controls.Add(this.dgvGroupDetail, 1, 0);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 60);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.RowCount = 1;
            this.mainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainPanel.Size = new System.Drawing.Size(1200, 540);
            this.mainPanel.TabIndex = 1;
            this.mainPanel.TagString = null;
            // 
            // uiTableLayoutPanel1
            // 
            this.uiTableLayoutPanel1.ColumnCount = 2;
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.uiTableLayoutPanel1.Controls.Add(this._txtMethodAfter, 1, 8);
            this.uiTableLayoutPanel1.Controls.Add(this._txtMethodBefore, 1, 7);
            this.uiTableLayoutPanel1.Controls.Add(this._txtRelaysOff, 1, 5);
            this.uiTableLayoutPanel1.Controls.Add(this._txtRelaysOn, 1, 4);
            this.uiTableLayoutPanel1.Controls.Add(this.cmbBinding, 1, 9);
            this.uiTableLayoutPanel1.Controls.Add(this.btnPowerPara, 1, 11);
            this.uiTableLayoutPanel1.Controls.Add(this.uiMarkLabel6, 0, 11);
            this.uiTableLayoutPanel1.Controls.Add(this._txtUnit, 1, 10);
            this.uiTableLayoutPanel1.Controls.Add(this.uiMarkLabel4, 0, 10);
            this.uiTableLayoutPanel1.Controls.Add(this._txtDelay, 1, 6);
            this.uiTableLayoutPanel1.Controls.Add(this.uiMarkLabel2, 0, 9);
            this.uiTableLayoutPanel1.Controls.Add(this._txtGroupCount, 1, 2);
            this.uiTableLayoutPanel1.Controls.Add(this.uiMarkLabel17, 0, 8);
            this.uiTableLayoutPanel1.Controls.Add(this.uiMarkLabel15, 0, 7);
            this.uiTableLayoutPanel1.Controls.Add(this.uiMarkLabel13, 0, 6);
            this.uiTableLayoutPanel1.Controls.Add(this.uiMarkLabel11, 0, 5);
            this.uiTableLayoutPanel1.Controls.Add(this.uiMarkLabel9, 0, 4);
            this.uiTableLayoutPanel1.Controls.Add(this.uiMarkLabel7, 0, 3);
            this.uiTableLayoutPanel1.Controls.Add(this.uiMarkLabel5, 0, 2);
            this.uiTableLayoutPanel1.Controls.Add(this.uiMarkLabel3, 0, 1);
            this.uiTableLayoutPanel1.Controls.Add(this.uiMarkLabel1, 0, 0);
            this.uiTableLayoutPanel1.Controls.Add(this._txtName, 1, 0);
            this.uiTableLayoutPanel1.Controls.Add(this._cmbType, 1, 1);
            this.uiTableLayoutPanel1.Controls.Add(this._cmbLeftRightType, 1, 3);
            this.uiTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.uiTableLayoutPanel1.Name = "uiTableLayoutPanel1";
            this.uiTableLayoutPanel1.RowCount = 13;
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.977662F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.668528F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.668528F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.668528F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.668528F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.668528F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.668528F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.668528F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.668528F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.668528F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.668528F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.668528F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.668528F));
            this.uiTableLayoutPanel1.Size = new System.Drawing.Size(474, 534);
            this.uiTableLayoutPanel1.TabIndex = 0;
            this.uiTableLayoutPanel1.TagString = null;
            // 
            // cmbBinding
            // 
            this.cmbBinding.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbBinding.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cmbBinding.FillColor = System.Drawing.Color.White;
            this.cmbBinding.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.cmbBinding.Location = new System.Drawing.Point(193, 367);
            this.cmbBinding.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbBinding.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmbBinding.Name = "cmbBinding";
            treeNode3.Name = "节点0";
            treeNode3.Text = "/";
            this.cmbBinding.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode3});
            this.cmbBinding.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbBinding.ShowLines = true;
            this.cmbBinding.Size = new System.Drawing.Size(277, 30);
            this.cmbBinding.TabIndex = 83;
            this.cmbBinding.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbBinding.Watermark = "";
            // 
            // btnPowerPara
            // 
            this.btnPowerPara.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPowerPara.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPowerPara.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnPowerPara.Location = new System.Drawing.Point(192, 445);
            this.btnPowerPara.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnPowerPara.Name = "btnPowerPara";
            this.btnPowerPara.Size = new System.Drawing.Size(279, 34);
            this.btnPowerPara.Style = Sunny.UI.UIStyle.Custom;
            this.btnPowerPara.StyleCustomMode = true;
            this.btnPowerPara.Symbol = 61529;
            this.btnPowerPara.TabIndex = 82;
            this.btnPowerPara.Text = "点击查看详细";
            this.btnPowerPara.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPowerPara.Click += new System.EventHandler(this.btnPowerPara_Click);
            // 
            // uiMarkLabel6
            // 
            this.uiMarkLabel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMarkLabel6.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.uiMarkLabel6.Location = new System.Drawing.Point(10, 452);
            this.uiMarkLabel6.Margin = new System.Windows.Forms.Padding(10);
            this.uiMarkLabel6.MarkPos = Sunny.UI.UIMarkLabel.UIMarkPos.Right;
            this.uiMarkLabel6.MarkSize = 1;
            this.uiMarkLabel6.Name = "uiMarkLabel6";
            this.uiMarkLabel6.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.uiMarkLabel6.Size = new System.Drawing.Size(169, 20);
            this.uiMarkLabel6.TabIndex = 38;
            this.uiMarkLabel6.Text = "输入电源：";
            this.uiMarkLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _txtUnit
            // 
            this._txtUnit.Cursor = System.Windows.Forms.Cursors.IBeam;
            this._txtUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this._txtUnit.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this._txtUnit.Location = new System.Drawing.Point(193, 407);
            this._txtUnit.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this._txtUnit.MinimumSize = new System.Drawing.Size(1, 16);
            this._txtUnit.Name = "_txtUnit";
            this._txtUnit.ShowText = false;
            this._txtUnit.Size = new System.Drawing.Size(277, 30);
            this._txtUnit.TabIndex = 37;
            this._txtUnit.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this._txtUnit.Watermark = "显示单位，可不填";
            // 
            // uiMarkLabel4
            // 
            this.uiMarkLabel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMarkLabel4.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.uiMarkLabel4.Location = new System.Drawing.Point(10, 412);
            this.uiMarkLabel4.Margin = new System.Windows.Forms.Padding(10);
            this.uiMarkLabel4.MarkPos = Sunny.UI.UIMarkLabel.UIMarkPos.Right;
            this.uiMarkLabel4.MarkSize = 1;
            this.uiMarkLabel4.Name = "uiMarkLabel4";
            this.uiMarkLabel4.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.uiMarkLabel4.Size = new System.Drawing.Size(169, 20);
            this.uiMarkLabel4.TabIndex = 36;
            this.uiMarkLabel4.Text = "单位：";
            this.uiMarkLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _txtDelay
            // 
            this._txtDelay.ButtonWidth = 100;
            this._txtDelay.CanEmpty = true;
            this._txtDelay.Cursor = System.Windows.Forms.Cursors.IBeam;
            this._txtDelay.Dock = System.Windows.Forms.DockStyle.Fill;
            this._txtDelay.DoubleValue = 100D;
            this._txtDelay.Font = new System.Drawing.Font("微软雅黑", 12F);
            this._txtDelay.IntValue = 100;
            this._txtDelay.Location = new System.Drawing.Point(193, 247);
            this._txtDelay.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this._txtDelay.Maximum = 100000D;
            this._txtDelay.Minimum = 0D;
            this._txtDelay.MinimumSize = new System.Drawing.Size(1, 1);
            this._txtDelay.Name = "_txtDelay";
            this._txtDelay.Padding = new System.Windows.Forms.Padding(5);
            this._txtDelay.ShowText = false;
            this._txtDelay.Size = new System.Drawing.Size(277, 30);
            this._txtDelay.TabIndex = 35;
            this._txtDelay.Text = "100";
            this._txtDelay.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this._txtDelay.Type = Sunny.UI.UITextBox.UIEditType.Integer;
            this._txtDelay.Watermark = "延时/ms，可不填";
            // 
            // uiMarkLabel2
            // 
            this.uiMarkLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMarkLabel2.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.uiMarkLabel2.Location = new System.Drawing.Point(10, 372);
            this.uiMarkLabel2.Margin = new System.Windows.Forms.Padding(10);
            this.uiMarkLabel2.MarkPos = Sunny.UI.UIMarkLabel.UIMarkPos.Right;
            this.uiMarkLabel2.MarkSize = 1;
            this.uiMarkLabel2.Name = "uiMarkLabel2";
            this.uiMarkLabel2.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.uiMarkLabel2.Size = new System.Drawing.Size(169, 20);
            this.uiMarkLabel2.TabIndex = 33;
            this.uiMarkLabel2.Text = "映射字段：";
            this.uiMarkLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _txtGroupCount
            // 
            this._txtGroupCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this._txtGroupCount.Enabled = false;
            this._txtGroupCount.Font = new System.Drawing.Font("微软雅黑", 12F);
            this._txtGroupCount.Location = new System.Drawing.Point(193, 87);
            this._txtGroupCount.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this._txtGroupCount.Maximum = 20;
            this._txtGroupCount.Minimum = 1;
            this._txtGroupCount.MinimumSize = new System.Drawing.Size(100, 0);
            this._txtGroupCount.Name = "_txtGroupCount";
            this._txtGroupCount.ShowText = false;
            this._txtGroupCount.Size = new System.Drawing.Size(277, 30);
            this._txtGroupCount.TabIndex = 31;
            this._txtGroupCount.Text = "_uiIntegerUpDown1";
            this._txtGroupCount.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this._txtGroupCount.Value = 1;
            // 
            // uiMarkLabel17
            // 
            this.uiMarkLabel17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMarkLabel17.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.uiMarkLabel17.Location = new System.Drawing.Point(10, 332);
            this.uiMarkLabel17.Margin = new System.Windows.Forms.Padding(10);
            this.uiMarkLabel17.MarkPos = Sunny.UI.UIMarkLabel.UIMarkPos.Right;
            this.uiMarkLabel17.MarkSize = 1;
            this.uiMarkLabel17.Name = "uiMarkLabel17";
            this.uiMarkLabel17.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.uiMarkLabel17.Size = new System.Drawing.Size(169, 20);
            this.uiMarkLabel17.TabIndex = 17;
            this.uiMarkLabel17.Text = "测试后执行函数：";
            this.uiMarkLabel17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel15
            // 
            this.uiMarkLabel15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMarkLabel15.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.uiMarkLabel15.Location = new System.Drawing.Point(10, 292);
            this.uiMarkLabel15.Margin = new System.Windows.Forms.Padding(10);
            this.uiMarkLabel15.MarkPos = Sunny.UI.UIMarkLabel.UIMarkPos.Right;
            this.uiMarkLabel15.MarkSize = 1;
            this.uiMarkLabel15.Name = "uiMarkLabel15";
            this.uiMarkLabel15.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.uiMarkLabel15.Size = new System.Drawing.Size(169, 20);
            this.uiMarkLabel15.TabIndex = 15;
            this.uiMarkLabel15.Text = "测试前执行函数：";
            this.uiMarkLabel15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel13
            // 
            this.uiMarkLabel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMarkLabel13.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.uiMarkLabel13.Location = new System.Drawing.Point(10, 252);
            this.uiMarkLabel13.Margin = new System.Windows.Forms.Padding(10);
            this.uiMarkLabel13.MarkPos = Sunny.UI.UIMarkLabel.UIMarkPos.Right;
            this.uiMarkLabel13.MarkSize = 1;
            this.uiMarkLabel13.Name = "uiMarkLabel13";
            this.uiMarkLabel13.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.uiMarkLabel13.Size = new System.Drawing.Size(169, 20);
            this.uiMarkLabel13.TabIndex = 13;
            this.uiMarkLabel13.Text = "延时：";
            this.uiMarkLabel13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel11
            // 
            this.uiMarkLabel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMarkLabel11.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.uiMarkLabel11.Location = new System.Drawing.Point(10, 212);
            this.uiMarkLabel11.Margin = new System.Windows.Forms.Padding(10);
            this.uiMarkLabel11.MarkPos = Sunny.UI.UIMarkLabel.UIMarkPos.Right;
            this.uiMarkLabel11.MarkSize = 1;
            this.uiMarkLabel11.Name = "uiMarkLabel11";
            this.uiMarkLabel11.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.uiMarkLabel11.Size = new System.Drawing.Size(169, 20);
            this.uiMarkLabel11.TabIndex = 11;
            this.uiMarkLabel11.Text = "继电器组（OFF）：";
            this.uiMarkLabel11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel9
            // 
            this.uiMarkLabel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMarkLabel9.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.uiMarkLabel9.Location = new System.Drawing.Point(10, 172);
            this.uiMarkLabel9.Margin = new System.Windows.Forms.Padding(10);
            this.uiMarkLabel9.MarkPos = Sunny.UI.UIMarkLabel.UIMarkPos.Right;
            this.uiMarkLabel9.MarkSize = 1;
            this.uiMarkLabel9.Name = "uiMarkLabel9";
            this.uiMarkLabel9.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.uiMarkLabel9.Size = new System.Drawing.Size(169, 20);
            this.uiMarkLabel9.TabIndex = 9;
            this.uiMarkLabel9.Text = "继电器组（ON）：";
            this.uiMarkLabel9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel7
            // 
            this.uiMarkLabel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMarkLabel7.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.uiMarkLabel7.Location = new System.Drawing.Point(10, 132);
            this.uiMarkLabel7.Margin = new System.Windows.Forms.Padding(10);
            this.uiMarkLabel7.MarkPos = Sunny.UI.UIMarkLabel.UIMarkPos.Right;
            this.uiMarkLabel7.MarkSize = 1;
            this.uiMarkLabel7.Name = "uiMarkLabel7";
            this.uiMarkLabel7.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.uiMarkLabel7.Size = new System.Drawing.Size(169, 20);
            this.uiMarkLabel7.TabIndex = 7;
            this.uiMarkLabel7.Text = "L/R：";
            this.uiMarkLabel7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel5
            // 
            this.uiMarkLabel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMarkLabel5.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.uiMarkLabel5.Location = new System.Drawing.Point(10, 92);
            this.uiMarkLabel5.Margin = new System.Windows.Forms.Padding(10);
            this.uiMarkLabel5.MarkPos = Sunny.UI.UIMarkLabel.UIMarkPos.Right;
            this.uiMarkLabel5.MarkSize = 1;
            this.uiMarkLabel5.Name = "uiMarkLabel5";
            this.uiMarkLabel5.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.uiMarkLabel5.Size = new System.Drawing.Size(169, 20);
            this.uiMarkLabel5.TabIndex = 5;
            this.uiMarkLabel5.Text = "档位：";
            this.uiMarkLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel3
            // 
            this.uiMarkLabel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMarkLabel3.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.uiMarkLabel3.Location = new System.Drawing.Point(10, 52);
            this.uiMarkLabel3.Margin = new System.Windows.Forms.Padding(10);
            this.uiMarkLabel3.MarkPos = Sunny.UI.UIMarkLabel.UIMarkPos.Right;
            this.uiMarkLabel3.MarkSize = 1;
            this.uiMarkLabel3.Name = "uiMarkLabel3";
            this.uiMarkLabel3.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.uiMarkLabel3.Size = new System.Drawing.Size(169, 20);
            this.uiMarkLabel3.TabIndex = 3;
            this.uiMarkLabel3.Text = "类型：";
            this.uiMarkLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel1
            // 
            this.uiMarkLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMarkLabel1.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.uiMarkLabel1.Location = new System.Drawing.Point(10, 10);
            this.uiMarkLabel1.Margin = new System.Windows.Forms.Padding(10);
            this.uiMarkLabel1.MarkPos = Sunny.UI.UIMarkLabel.UIMarkPos.Bottom;
            this.uiMarkLabel1.MarkSize = 1;
            this.uiMarkLabel1.Name = "uiMarkLabel1";
            this.uiMarkLabel1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.uiMarkLabel1.Size = new System.Drawing.Size(169, 22);
            this.uiMarkLabel1.TabIndex = 1;
            this.uiMarkLabel1.Text = "名称：";
            this.uiMarkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _txtName
            // 
            this._txtName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this._txtName.Dock = System.Windows.Forms.DockStyle.Fill;
            this._txtName.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this._txtName.Location = new System.Drawing.Point(193, 5);
            this._txtName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this._txtName.MinimumSize = new System.Drawing.Size(1, 16);
            this._txtName.Name = "_txtName";
            this._txtName.ShowText = false;
            this._txtName.Size = new System.Drawing.Size(277, 32);
            this._txtName.TabIndex = 20;
            this._txtName.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this._txtName.Watermark = "名称，必填";
            // 
            // _cmbType
            // 
            this._cmbType.DataSource = null;
            this._cmbType.Dock = System.Windows.Forms.DockStyle.Fill;
            this._cmbType.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this._cmbType.FillColor = System.Drawing.Color.White;
            this._cmbType.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this._cmbType.Items.AddRange(new object[] {
            "电性能",
            "电性能，静态图像",
            "静态图像",
            "动态图像",
            "信息读取（==）",
            "信息读取（like）",
            "电阻"});
            this._cmbType.Location = new System.Drawing.Point(193, 47);
            this._cmbType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this._cmbType.MinimumSize = new System.Drawing.Size(63, 0);
            this._cmbType.Name = "_cmbType";
            this._cmbType.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this._cmbType.Size = new System.Drawing.Size(277, 30);
            this._cmbType.TabIndex = 30;
            this._cmbType.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this._cmbType.Watermark = "";
            // 
            // _cmbLeftRightType
            // 
            this._cmbLeftRightType.DataSource = null;
            this._cmbLeftRightType.Dock = System.Windows.Forms.DockStyle.Fill;
            this._cmbLeftRightType.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this._cmbLeftRightType.FillColor = System.Drawing.Color.White;
            this._cmbLeftRightType.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this._cmbLeftRightType.Items.AddRange(new object[] {
            "L&R",
            "仅L",
            "仅R"});
            this._cmbLeftRightType.Location = new System.Drawing.Point(193, 127);
            this._cmbLeftRightType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this._cmbLeftRightType.MinimumSize = new System.Drawing.Size(63, 0);
            this._cmbLeftRightType.Name = "_cmbLeftRightType";
            this._cmbLeftRightType.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this._cmbLeftRightType.Size = new System.Drawing.Size(277, 30);
            this._cmbLeftRightType.TabIndex = 32;
            this._cmbLeftRightType.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this._cmbLeftRightType.Watermark = "";
            // 
            // dgvGroupDetail
            // 
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            this.dgvGroupDetail.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle11;
            this.dgvGroupDetail.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            this.dgvGroupDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle12.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvGroupDetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.dgvGroupDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(236)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvGroupDetail.DefaultCellStyle = dataGridViewCellStyle13;
            this.dgvGroupDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGroupDetail.EnableHeadersVisualStyles = false;
            this.dgvGroupDetail.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgvGroupDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(173)))), ((int)(((byte)(255)))));
            this.dgvGroupDetail.Location = new System.Drawing.Point(483, 3);
            this.dgvGroupDetail.Name = "dgvGroupDetail";
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle14.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvGroupDetail.RowHeadersDefaultCellStyle = dataGridViewCellStyle14;
            dataGridViewCellStyle15.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(236)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.dgvGroupDetail.RowsDefaultCellStyle = dataGridViewCellStyle15;
            this.dgvGroupDetail.RowTemplate.Height = 23;
            this.dgvGroupDetail.ScrollBarRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            this.dgvGroupDetail.SelectedIndex = -1;
            this.dgvGroupDetail.Size = new System.Drawing.Size(714, 534);
            this.dgvGroupDetail.TabIndex = 1;
            // 
            // _txtRelaysOn
            // 
            this._txtRelaysOn.CheckBoxes = true;
            this._txtRelaysOn.Dock = System.Windows.Forms.DockStyle.Fill;
            this._txtRelaysOn.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this._txtRelaysOn.FillColor = System.Drawing.Color.White;
            this._txtRelaysOn.Font = new System.Drawing.Font("微软雅黑", 12F);
            this._txtRelaysOn.Location = new System.Drawing.Point(193, 167);
            this._txtRelaysOn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this._txtRelaysOn.MinimumSize = new System.Drawing.Size(63, 0);
            this._txtRelaysOn.Name = "_txtRelaysOn";
            this._txtRelaysOn.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this._txtRelaysOn.Size = new System.Drawing.Size(277, 30);
            this._txtRelaysOn.TabIndex = 84;
            this._txtRelaysOn.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this._txtRelaysOn.Watermark = "";
            // 
            // _txtRelaysOff
            // 
            this._txtRelaysOff.CheckBoxes = true;
            this._txtRelaysOff.Dock = System.Windows.Forms.DockStyle.Fill;
            this._txtRelaysOff.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this._txtRelaysOff.FillColor = System.Drawing.Color.White;
            this._txtRelaysOff.Font = new System.Drawing.Font("微软雅黑", 12F);
            this._txtRelaysOff.Location = new System.Drawing.Point(193, 207);
            this._txtRelaysOff.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this._txtRelaysOff.MinimumSize = new System.Drawing.Size(63, 0);
            this._txtRelaysOff.Name = "_txtRelaysOff";
            this._txtRelaysOff.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this._txtRelaysOff.ShowLines = true;
            this._txtRelaysOff.Size = new System.Drawing.Size(277, 30);
            this._txtRelaysOff.TabIndex = 85;
            this._txtRelaysOff.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this._txtRelaysOff.Watermark = "";
            // 
            // _txtMethodBefore
            // 
            this._txtMethodBefore.Cursor = System.Windows.Forms.Cursors.Hand;
            this._txtMethodBefore.Dock = System.Windows.Forms.DockStyle.Fill;
            this._txtMethodBefore.Font = new System.Drawing.Font("微软雅黑", 12F);
            this._txtMethodBefore.Location = new System.Drawing.Point(192, 285);
            this._txtMethodBefore.MinimumSize = new System.Drawing.Size(1, 1);
            this._txtMethodBefore.Name = "_txtMethodBefore";
            this._txtMethodBefore.Size = new System.Drawing.Size(279, 34);
            this._txtMethodBefore.Style = Sunny.UI.UIStyle.Custom;
            this._txtMethodBefore.StyleCustomMode = true;
            this._txtMethodBefore.Symbol = 61529;
            this._txtMethodBefore.TabIndex = 86;
            this._txtMethodBefore.Text = "点击查看详细";
            this._txtMethodBefore.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this._txtMethodBefore.Click += new System.EventHandler(this._txtMethodBefore_Click);
            // 
            // _txtMethodAfter
            // 
            this._txtMethodAfter.Cursor = System.Windows.Forms.Cursors.Hand;
            this._txtMethodAfter.Dock = System.Windows.Forms.DockStyle.Fill;
            this._txtMethodAfter.Font = new System.Drawing.Font("微软雅黑", 12F);
            this._txtMethodAfter.Location = new System.Drawing.Point(192, 325);
            this._txtMethodAfter.MinimumSize = new System.Drawing.Size(1, 1);
            this._txtMethodAfter.Name = "_txtMethodAfter";
            this._txtMethodAfter.Size = new System.Drawing.Size(279, 34);
            this._txtMethodAfter.Style = Sunny.UI.UIStyle.Custom;
            this._txtMethodAfter.StyleCustomMode = true;
            this._txtMethodAfter.Symbol = 61529;
            this._txtMethodAfter.TabIndex = 87;
            this._txtMethodAfter.Text = "点击查看详细";
            this._txtMethodAfter.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this._txtMethodAfter.Click += new System.EventHandler(this._txtMethodAfter_Click);
            // 
            // FrmDetectionParaConfig
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1200, 600);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.toolStrip1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1200, 600);
            this.MinimumSize = new System.Drawing.Size(1200, 600);
            this.Name = "FrmDetectionParaConfig";
            this.Text = "检测参数详细配置";
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 800, 480);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.mainPanel.ResumeLayout(false);
            this.uiTableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGroupDetail)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private Sunny.UI.UITableLayoutPanel mainPanel;
        private Sunny.UI.UITableLayoutPanel uiTableLayoutPanel1;
        private Sunny.UI.UITextBox _txtDelay;
        private Sunny.UI.UIMarkLabel uiMarkLabel2;
        private Sunny.UI.UIIntegerUpDown _txtGroupCount;
        private Sunny.UI.UIMarkLabel uiMarkLabel17;
        private Sunny.UI.UIMarkLabel uiMarkLabel15;
        private Sunny.UI.UIMarkLabel uiMarkLabel13;
        private Sunny.UI.UIMarkLabel uiMarkLabel11;
        private Sunny.UI.UIMarkLabel uiMarkLabel9;
        private Sunny.UI.UIMarkLabel uiMarkLabel7;
        private Sunny.UI.UIMarkLabel uiMarkLabel5;
        private Sunny.UI.UIMarkLabel uiMarkLabel3;
        private Sunny.UI.UIMarkLabel uiMarkLabel1;
        private Sunny.UI.UITextBox _txtName;
        private Sunny.UI.UIComboBox _cmbType;
        private Sunny.UI.UIComboBox _cmbLeftRightType;
        private System.Windows.Forms.ToolStripButton btnConfirm;
        private System.Windows.Forms.ToolStripButton btnCancel;
        private Sunny.UI.UIDataGridView dgvGroupDetail;
        private Sunny.UI.UITextBox _txtUnit;
        private Sunny.UI.UIMarkLabel uiMarkLabel4;
        private Sunny.UI.UIMarkLabel uiMarkLabel6;
        private Sunny.UI.UISymbolButton btnPowerPara;
        private Sunny.UI.UIComboTreeView cmbBinding;
        private Sunny.UI.UIComboTreeView _txtRelaysOff;
        private Sunny.UI.UIComboTreeView _txtRelaysOn;
        private Sunny.UI.UISymbolButton _txtMethodAfter;
        private Sunny.UI.UISymbolButton _txtMethodBefore;

    }
}