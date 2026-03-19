namespace CheckSystem.PesVision
{
    partial class PesVisionMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PesVisionMain));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.pesConfigBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.tsbtnStateConfig = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnControllerOperation = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnStateWatch = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnServoTeach = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.btnLookLocalData = new System.Windows.Forms.ToolStripButton();
            this.mainButtomTablePanel = new Sunny.UI.UITableLayoutPanel();
            this.leftTablePanel = new Sunny.UI.UITableLayoutPanel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.uiTabControl1 = new Sunny.UI.UITabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.uiPanel1 = new Sunny.UI.UIPanel();
            this.cmbImageList = new Sunny.UI.UIComboBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.uiRichTextBox1 = new Sunny.UI.UIRichTextBox();
            this.lblTitle = new Sunny.UI.UIMarkLabel();
            this.rightTablePanel = new Sunny.UI.UITableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.mainDataShowTable = new Sunny.UI.UITableLayoutPanel();
            this.lblBarcodeScan = new Sunny.UI.UIMarkLabel();
            this.mainDgv = new Sunny.UI.UIDataGridView();
            this.toolStrip1.SuspendLayout();
            this.mainButtomTablePanel.SuspendLayout();
            this.leftTablePanel.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.uiTabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.uiPanel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.rightTablePanel.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.mainDataShowTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainDgv)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pesConfigBtn,
            this.toolStripButton1,
            this.tsbtnStateConfig,
            this.toolStripSeparator1,
            this.tsbtnControllerOperation,
            this.toolStripSeparator2,
            this.tsbtnStateWatch,
            this.toolStripSeparator4,
            this.btnServoTeach,
            this.toolStripSeparator5,
            this.btnLookLocalData});
            this.toolStrip1.Location = new System.Drawing.Point(0, 35);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1452, 27);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // pesConfigBtn
            // 
            this.pesConfigBtn.Image = ((System.Drawing.Image)(resources.GetObject("pesConfigBtn.Image")));
            this.pesConfigBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pesConfigBtn.Name = "pesConfigBtn";
            this.pesConfigBtn.Size = new System.Drawing.Size(80, 24);
            this.pesConfigBtn.Text = "参数标定";
            this.pesConfigBtn.Visible = false;
            this.pesConfigBtn.Click += new System.EventHandler(this.pesConfigBtn_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(127, 24);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.Visible = false;
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // tsbtnStateConfig
            // 
            this.tsbtnStateConfig.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnStateConfig.Image")));
            this.tsbtnStateConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnStateConfig.Name = "tsbtnStateConfig";
            this.tsbtnStateConfig.Size = new System.Drawing.Size(104, 24);
            this.tsbtnStateConfig.Text = "打开流程编辑";
            this.tsbtnStateConfig.Click += new System.EventHandler(this.tsbtnStateConfig_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // tsbtnControllerOperation
            // 
            this.tsbtnControllerOperation.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnControllerOperation.Image")));
            this.tsbtnControllerOperation.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnControllerOperation.Name = "tsbtnControllerOperation";
            this.tsbtnControllerOperation.Size = new System.Drawing.Size(140, 24);
            this.tsbtnControllerOperation.Text = "打开控制器调试界面";
            this.tsbtnControllerOperation.Click += new System.EventHandler(this.tsbtnControllerOperation_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // tsbtnStateWatch
            // 
            this.tsbtnStateWatch.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnStateWatch.Image")));
            this.tsbtnStateWatch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnStateWatch.Name = "tsbtnStateWatch";
            this.tsbtnStateWatch.Size = new System.Drawing.Size(104, 24);
            this.tsbtnStateWatch.Text = "查看流程状态";
            this.tsbtnStateWatch.Click += new System.EventHandler(this.tsbtnStateWatch_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
            // 
            // btnServoTeach
            // 
            this.btnServoTeach.Image = ((System.Drawing.Image)(resources.GetObject("btnServoTeach.Image")));
            this.btnServoTeach.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnServoTeach.Name = "btnServoTeach";
            this.btnServoTeach.Size = new System.Drawing.Size(80, 24);
            this.btnServoTeach.Text = "模组示教";
            this.btnServoTeach.Click += new System.EventHandler(this.btnServoTeach_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
            // 
            // btnLookLocalData
            // 
            this.btnLookLocalData.Image = ((System.Drawing.Image)(resources.GetObject("btnLookLocalData.Image")));
            this.btnLookLocalData.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLookLocalData.Name = "btnLookLocalData";
            this.btnLookLocalData.Size = new System.Drawing.Size(104, 24);
            this.btnLookLocalData.Text = "查看本地数据";
            this.btnLookLocalData.Click += new System.EventHandler(this.btnLookLocalData_Click);
            // 
            // mainButtomTablePanel
            // 
            this.mainButtomTablePanel.ColumnCount = 2;
            this.mainButtomTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.mainButtomTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.mainButtomTablePanel.Controls.Add(this.leftTablePanel, 0, 0);
            this.mainButtomTablePanel.Controls.Add(this.rightTablePanel, 1, 0);
            this.mainButtomTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainButtomTablePanel.Location = new System.Drawing.Point(0, 62);
            this.mainButtomTablePanel.Name = "mainButtomTablePanel";
            this.mainButtomTablePanel.RowCount = 1;
            this.mainButtomTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainButtomTablePanel.Size = new System.Drawing.Size(1452, 666);
            this.mainButtomTablePanel.TabIndex = 1;
            this.mainButtomTablePanel.TagString = null;
            // 
            // leftTablePanel
            // 
            this.leftTablePanel.ColumnCount = 1;
            this.leftTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.leftTablePanel.Controls.Add(this.groupBox3, 0, 1);
            this.leftTablePanel.Controls.Add(this.lblTitle, 0, 0);
            this.leftTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.leftTablePanel.Location = new System.Drawing.Point(3, 3);
            this.leftTablePanel.Name = "leftTablePanel";
            this.leftTablePanel.RowCount = 2;
            this.leftTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.leftTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85F));
            this.leftTablePanel.Size = new System.Drawing.Size(865, 660);
            this.leftTablePanel.TabIndex = 0;
            this.leftTablePanel.TagString = null;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.uiTabControl1);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 102);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(859, 555);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "图像显示";
            // 
            // uiTabControl1
            // 
            this.uiTabControl1.Controls.Add(this.tabPage1);
            this.uiTabControl1.Controls.Add(this.tabPage2);
            this.uiTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.uiTabControl1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiTabControl1.ItemSize = new System.Drawing.Size(150, 40);
            this.uiTabControl1.Location = new System.Drawing.Point(3, 22);
            this.uiTabControl1.MainPage = "";
            this.uiTabControl1.Name = "uiTabControl1";
            this.uiTabControl1.SelectedIndex = 0;
            this.uiTabControl1.Size = new System.Drawing.Size(853, 530);
            this.uiTabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.uiTabControl1.TabIndex = 0;
            this.uiTabControl1.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.uiPanel1);
            this.tabPage1.Location = new System.Drawing.Point(0, 40);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(853, 490);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Page1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // uiPanel1
            // 
            this.uiPanel1.Controls.Add(this.cmbImageList);
            this.uiPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiPanel1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiPanel1.Location = new System.Drawing.Point(0, 0);
            this.uiPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPanel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Size = new System.Drawing.Size(853, 490);
            this.uiPanel1.TabIndex = 1;
            this.uiPanel1.Text = null;
            this.uiPanel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbImageList
            // 
            this.cmbImageList.DataSource = null;
            this.cmbImageList.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbImageList.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cmbImageList.FillColor = System.Drawing.Color.White;
            this.cmbImageList.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbImageList.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.cmbImageList.ItemSelectForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.cmbImageList.Location = new System.Drawing.Point(0, 0);
            this.cmbImageList.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbImageList.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmbImageList.Name = "cmbImageList";
            this.cmbImageList.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbImageList.Size = new System.Drawing.Size(853, 29);
            this.cmbImageList.SymbolSize = 24;
            this.cmbImageList.TabIndex = 0;
            this.cmbImageList.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbImageList.Watermark = "";
            this.cmbImageList.SelectedIndexChanged += new System.EventHandler(this.uiComboBox1_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.uiRichTextBox1);
            this.tabPage2.Location = new System.Drawing.Point(0, 40);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(200, 60);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Page2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // uiRichTextBox1
            // 
            this.uiRichTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiRichTextBox1.FillColor = System.Drawing.Color.White;
            this.uiRichTextBox1.Font = new System.Drawing.Font("黑体", 10F);
            this.uiRichTextBox1.Location = new System.Drawing.Point(0, 0);
            this.uiRichTextBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiRichTextBox1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiRichTextBox1.Name = "uiRichTextBox1";
            this.uiRichTextBox1.Padding = new System.Windows.Forms.Padding(2);
            this.uiRichTextBox1.ReadOnly = true;
            this.uiRichTextBox1.ScrollBarStyleInherited = false;
            this.uiRichTextBox1.ShowText = false;
            this.uiRichTextBox1.Size = new System.Drawing.Size(200, 60);
            this.uiRichTextBox1.TabIndex = 4;
            this.uiRichTextBox1.Text = "uiRichTextBox1";
            this.uiRichTextBox1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.DodgerBlue;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("黑体", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(3, 0);
            this.lblTitle.MarkPos = Sunny.UI.UIMarkLabel.UIMarkPos.Bottom;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.lblTitle.Size = new System.Drawing.Size(859, 99);
            this.lblTitle.TabIndex = 5;
            this.lblTitle.Text = "uiMarkLabel1";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rightTablePanel
            // 
            this.rightTablePanel.ColumnCount = 1;
            this.rightTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.rightTablePanel.Controls.Add(this.groupBox2, 0, 0);
            this.rightTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightTablePanel.Location = new System.Drawing.Point(874, 3);
            this.rightTablePanel.Name = "rightTablePanel";
            this.rightTablePanel.RowCount = 4;
            this.rightTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.rightTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.rightTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.rightTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.rightTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.rightTablePanel.Size = new System.Drawing.Size(575, 660);
            this.rightTablePanel.TabIndex = 1;
            this.rightTablePanel.TagString = null;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.mainDataShowTable);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.rightTablePanel.SetRowSpan(this.groupBox2, 4);
            this.groupBox2.Size = new System.Drawing.Size(569, 654);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "测试数据";
            // 
            // mainDataShowTable
            // 
            this.mainDataShowTable.ColumnCount = 1;
            this.mainDataShowTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainDataShowTable.Controls.Add(this.lblBarcodeScan, 0, 0);
            this.mainDataShowTable.Controls.Add(this.mainDgv, 0, 1);
            this.mainDataShowTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainDataShowTable.Location = new System.Drawing.Point(3, 22);
            this.mainDataShowTable.Name = "mainDataShowTable";
            this.mainDataShowTable.RowCount = 3;
            this.mainDataShowTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.mainDataShowTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.mainDataShowTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainDataShowTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.mainDataShowTable.Size = new System.Drawing.Size(563, 629);
            this.mainDataShowTable.TabIndex = 1;
            this.mainDataShowTable.TagString = null;
            // 
            // lblBarcodeScan
            // 
            this.lblBarcodeScan.BackColor = System.Drawing.Color.LightGray;
            this.lblBarcodeScan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBarcodeScan.Font = new System.Drawing.Font("微软雅黑", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.lblBarcodeScan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.lblBarcodeScan.Location = new System.Drawing.Point(2, 2);
            this.lblBarcodeScan.Margin = new System.Windows.Forms.Padding(2);
            this.lblBarcodeScan.Name = "lblBarcodeScan";
            this.lblBarcodeScan.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.lblBarcodeScan.Size = new System.Drawing.Size(559, 46);
            this.lblBarcodeScan.TabIndex = 5;
            this.lblBarcodeScan.Text = "二维码扫描：";
            this.lblBarcodeScan.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // mainDgv
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            this.mainDgv.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.mainDgv.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            this.mainDgv.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.mainDgv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.mainDgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(236)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.mainDgv.DefaultCellStyle = dataGridViewCellStyle3;
            this.mainDgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainDgv.EnableHeadersVisualStyles = false;
            this.mainDgv.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mainDgv.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(173)))), ((int)(((byte)(255)))));
            this.mainDgv.Location = new System.Drawing.Point(3, 53);
            this.mainDgv.Name = "mainDgv";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.mainDgv.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.mainDgv.RowHeadersWidth = 51;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(236)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.mainDgv.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.mainDataShowTable.SetRowSpan(this.mainDgv, 2);
            this.mainDgv.RowTemplate.Height = 23;
            this.mainDgv.SelectedIndex = -1;
            this.mainDgv.Size = new System.Drawing.Size(557, 573);
            this.mainDgv.TabIndex = 3;
            // 
            // PesVisionMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1452, 728);
            this.Controls.Add(this.mainButtomTablePanel);
            this.Controls.Add(this.toolStrip1);
            this.Name = "PesVisionMain";
            this.Text = "PES光型检测";
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 800, 450);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.mainButtomTablePanel.ResumeLayout(false);
            this.leftTablePanel.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.uiTabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.uiPanel1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.rightTablePanel.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.mainDataShowTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainDgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton pesConfigBtn;
        private System.Windows.Forms.ToolStripButton tsbtnStateConfig;
        private System.Windows.Forms.ToolStripButton tsbtnControllerOperation;
        private Sunny.UI.UITableLayoutPanel mainButtomTablePanel;
        private Sunny.UI.UITableLayoutPanel leftTablePanel;
        private System.Windows.Forms.GroupBox groupBox3;
        private Sunny.UI.UITableLayoutPanel rightTablePanel;
        private System.Windows.Forms.GroupBox groupBox2;
        private Sunny.UI.UITableLayoutPanel mainDataShowTable;
        private Sunny.UI.UIMarkLabel lblBarcodeScan;
        private Sunny.UI.UIDataGridView mainDgv;
        private System.Windows.Forms.ToolStripButton tsbtnStateWatch;
        private Sunny.UI.UITabControl uiTabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private Sunny.UI.UIPanel uiPanel1;
        private Sunny.UI.UIComboBox cmbImageList;
        private System.Windows.Forms.TabPage tabPage2;
        private Sunny.UI.UIRichTextBox uiRichTextBox1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton btnServoTeach;
        private Sunny.UI.UIMarkLabel lblTitle;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton btnLookLocalData;
    }
}