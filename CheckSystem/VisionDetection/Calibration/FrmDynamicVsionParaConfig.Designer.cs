namespace CheckSystem.VisionDetection.Calibration
{
    sealed partial class FrmDynamicVsionParaConfig
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.uiSplitContainer1 = new Sunny.UI.UISplitContainer();
            this.uiPanel1 = new Sunny.UI.UIPanel();
            this.uiTableLayoutPanel1 = new Sunny.UI.UITableLayoutPanel();
            this.btnSaveImage = new Sunny.UI.UIButton();
            this.uiMarkLabel7 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel6 = new Sunny.UI.UIMarkLabel();
            this.cmbGroup = new System.Windows.Forms.ComboBox();
            this.txtLearnView = new Sunny.UI.UIRichTextBox();
            this.btnSave = new Sunny.UI.UIButton();
            this.btnLearn = new Sunny.UI.UIButton();
            this.btnFindCircle = new Sunny.UI.UIButton();
            this.btnAddRect = new Sunny.UI.UIButton();
            this.btnLightAndSnap = new Sunny.UI.UIButton();
            this.toolStripCmbImgList = new System.Windows.Forms.ComboBox();
            this.txtFrameCount = new Sunny.UI.UIIntegerUpDown();
            this.txtSequenceDelay = new Sunny.UI.UIIntegerUpDown();
            this.cmbSequenceType = new Sunny.UI.UIComboBox();
            this.txtShutter = new Sunny.UI.UIIntegerUpDown();
            this.cmbCameras = new Sunny.UI.UIComboBox();
            this.uiMarkLabel5 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel4 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel3 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel2 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel1 = new Sunny.UI.UIMarkLabel();
            this.uiTabControl1 = new Sunny.UI.UITabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgvData = new Sunny.UI.UIDataGridView();
            this.uiContextMenuStrip1 = new Sunny.UI.UIContextMenuStrip();
            this.导出ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.uiSplitContainer1)).BeginInit();
            this.uiSplitContainer1.Panel1.SuspendLayout();
            this.uiSplitContainer1.Panel2.SuspendLayout();
            this.uiSplitContainer1.SuspendLayout();
            this.uiPanel1.SuspendLayout();
            this.uiTableLayoutPanel1.SuspendLayout();
            this.uiTabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.uiContextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiSplitContainer1
            // 
            this.uiSplitContainer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.uiSplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiSplitContainer1.Location = new System.Drawing.Point(0, 35);
            this.uiSplitContainer1.MinimumSize = new System.Drawing.Size(20, 20);
            this.uiSplitContainer1.Name = "uiSplitContainer1";
            // 
            // uiSplitContainer1.Panel1
            // 
            this.uiSplitContainer1.Panel1.Controls.Add(this.uiPanel1);
            // 
            // uiSplitContainer1.Panel2
            // 
            this.uiSplitContainer1.Panel2.Controls.Add(this.uiTabControl1);
            this.uiSplitContainer1.Size = new System.Drawing.Size(1210, 638);
            this.uiSplitContainer1.SplitterDistance = 324;
            this.uiSplitContainer1.SplitterWidth = 11;
            this.uiSplitContainer1.TabIndex = 0;
            // 
            // uiPanel1
            // 
            this.uiPanel1.Controls.Add(this.uiTableLayoutPanel1);
            this.uiPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiPanel1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiPanel1.Location = new System.Drawing.Point(0, 0);
            this.uiPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPanel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Size = new System.Drawing.Size(324, 638);
            this.uiPanel1.TabIndex = 0;
            this.uiPanel1.Text = null;
            this.uiPanel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiTableLayoutPanel1
            // 
            this.uiTableLayoutPanel1.ColumnCount = 2;
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.uiTableLayoutPanel1.Controls.Add(this.btnSaveImage, 0, 9);
            this.uiTableLayoutPanel1.Controls.Add(this.uiMarkLabel7, 0, 8);
            this.uiTableLayoutPanel1.Controls.Add(this.uiMarkLabel6, 0, 7);
            this.uiTableLayoutPanel1.Controls.Add(this.cmbGroup, 1, 8);
            this.uiTableLayoutPanel1.Controls.Add(this.txtLearnView, 0, 10);
            this.uiTableLayoutPanel1.Controls.Add(this.btnSave, 1, 9);
            this.uiTableLayoutPanel1.Controls.Add(this.btnLearn, 1, 5);
            this.uiTableLayoutPanel1.Controls.Add(this.btnFindCircle, 1, 6);
            this.uiTableLayoutPanel1.Controls.Add(this.btnAddRect, 0, 6);
            this.uiTableLayoutPanel1.Controls.Add(this.btnLightAndSnap, 0, 5);
            this.uiTableLayoutPanel1.Controls.Add(this.toolStripCmbImgList, 1, 7);
            this.uiTableLayoutPanel1.Controls.Add(this.txtFrameCount, 1, 4);
            this.uiTableLayoutPanel1.Controls.Add(this.txtSequenceDelay, 1, 3);
            this.uiTableLayoutPanel1.Controls.Add(this.cmbSequenceType, 1, 2);
            this.uiTableLayoutPanel1.Controls.Add(this.txtShutter, 1, 1);
            this.uiTableLayoutPanel1.Controls.Add(this.cmbCameras, 1, 0);
            this.uiTableLayoutPanel1.Controls.Add(this.uiMarkLabel5, 0, 4);
            this.uiTableLayoutPanel1.Controls.Add(this.uiMarkLabel4, 0, 3);
            this.uiTableLayoutPanel1.Controls.Add(this.uiMarkLabel3, 0, 2);
            this.uiTableLayoutPanel1.Controls.Add(this.uiMarkLabel2, 0, 1);
            this.uiTableLayoutPanel1.Controls.Add(this.uiMarkLabel1, 0, 0);
            this.uiTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.uiTableLayoutPanel1.Name = "uiTableLayoutPanel1";
            this.uiTableLayoutPanel1.RowCount = 13;
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333333F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333333F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333333F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333333F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333333F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333333F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333333F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333333F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333333F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333333F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333333F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333333F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.uiTableLayoutPanel1.Size = new System.Drawing.Size(324, 638);
            this.uiTableLayoutPanel1.TabIndex = 0;
            this.uiTableLayoutPanel1.TagString = null;
            // 
            // btnSaveImage
            // 
            this.btnSaveImage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSaveImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaveImage.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSaveImage.Location = new System.Drawing.Point(3, 462);
            this.btnSaveImage.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnSaveImage.Name = "btnSaveImage";
            this.btnSaveImage.Size = new System.Drawing.Size(123, 45);
            this.btnSaveImage.TabIndex = 172;
            this.btnSaveImage.Text = "保存图像";
            this.btnSaveImage.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSaveImage.Click += new System.EventHandler(this.btnSaveImage_Click);
            // 
            // uiMarkLabel7
            // 
            this.uiMarkLabel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMarkLabel7.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiMarkLabel7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel7.Location = new System.Drawing.Point(10, 418);
            this.uiMarkLabel7.Margin = new System.Windows.Forms.Padding(10);
            this.uiMarkLabel7.Name = "uiMarkLabel7";
            this.uiMarkLabel7.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel7.Size = new System.Drawing.Size(109, 31);
            this.uiMarkLabel7.TabIndex = 171;
            this.uiMarkLabel7.Text = "动画索引：";
            this.uiMarkLabel7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel6
            // 
            this.uiMarkLabel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMarkLabel6.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiMarkLabel6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel6.Location = new System.Drawing.Point(10, 367);
            this.uiMarkLabel6.Margin = new System.Windows.Forms.Padding(10);
            this.uiMarkLabel6.Name = "uiMarkLabel6";
            this.uiMarkLabel6.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel6.Size = new System.Drawing.Size(109, 31);
            this.uiMarkLabel6.TabIndex = 170;
            this.uiMarkLabel6.Text = "照片索引：";
            this.uiMarkLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbGroup
            // 
            this.cmbGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGroup.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.cmbGroup.FormattingEnabled = true;
            this.cmbGroup.Location = new System.Drawing.Point(132, 411);
            this.cmbGroup.Name = "cmbGroup";
            this.cmbGroup.Size = new System.Drawing.Size(189, 30);
            this.cmbGroup.TabIndex = 169;
            this.cmbGroup.SelectedIndexChanged += new System.EventHandler(this.cmbGroup_SelectedIndexChanged);
            // 
            // txtLearnView
            // 
            this.uiTableLayoutPanel1.SetColumnSpan(this.txtLearnView, 2);
            this.txtLearnView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLearnView.FillColor = System.Drawing.Color.White;
            this.txtLearnView.Font = new System.Drawing.Font("微软雅黑", 6.5F);
            this.txtLearnView.Location = new System.Drawing.Point(4, 515);
            this.txtLearnView.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtLearnView.MinimumSize = new System.Drawing.Size(1, 1);
            this.txtLearnView.Name = "txtLearnView";
            this.txtLearnView.Padding = new System.Windows.Forms.Padding(2);
            this.txtLearnView.ReadOnly = true;
            this.uiTableLayoutPanel1.SetRowSpan(this.txtLearnView, 3);
            this.txtLearnView.ScrollBarStyleInherited = false;
            this.txtLearnView.ShowText = false;
            this.txtLearnView.Size = new System.Drawing.Size(316, 118);
            this.txtLearnView.TabIndex = 167;
            this.txtLearnView.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSave
            // 
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSave.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.Location = new System.Drawing.Point(132, 462);
            this.btnSave.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(189, 45);
            this.btnSave.TabIndex = 166;
            this.btnSave.Text = "保存结果";
            this.btnSave.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLearn
            // 
            this.btnLearn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLearn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLearn.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLearn.Location = new System.Drawing.Point(132, 258);
            this.btnLearn.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnLearn.Name = "btnLearn";
            this.btnLearn.Size = new System.Drawing.Size(189, 45);
            this.btnLearn.TabIndex = 165;
            this.btnLearn.Text = "动画学习";
            this.btnLearn.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLearn.Click += new System.EventHandler(this.btnLearn_Click);
            // 
            // btnFindCircle
            // 
            this.btnFindCircle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFindCircle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFindCircle.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnFindCircle.Location = new System.Drawing.Point(132, 309);
            this.btnFindCircle.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnFindCircle.Name = "btnFindCircle";
            this.btnFindCircle.Size = new System.Drawing.Size(189, 45);
            this.btnFindCircle.TabIndex = 164;
            this.btnFindCircle.Text = "识别LED";
            this.btnFindCircle.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnFindCircle.Click += new System.EventHandler(this.btnFindCircle_Click);
            // 
            // btnAddRect
            // 
            this.btnAddRect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddRect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddRect.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAddRect.Location = new System.Drawing.Point(3, 309);
            this.btnAddRect.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnAddRect.Name = "btnAddRect";
            this.btnAddRect.Size = new System.Drawing.Size(123, 45);
            this.btnAddRect.TabIndex = 163;
            this.btnAddRect.Text = "添加矩形";
            this.btnAddRect.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAddRect.Click += new System.EventHandler(this.btnAddRect_Click);
            // 
            // btnLightAndSnap
            // 
            this.btnLightAndSnap.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLightAndSnap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLightAndSnap.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLightAndSnap.Location = new System.Drawing.Point(3, 258);
            this.btnLightAndSnap.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnLightAndSnap.Name = "btnLightAndSnap";
            this.btnLightAndSnap.Size = new System.Drawing.Size(123, 45);
            this.btnLightAndSnap.TabIndex = 162;
            this.btnLightAndSnap.Text = "点亮并抓拍";
            this.btnLightAndSnap.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLightAndSnap.Click += new System.EventHandler(this.btnLightAndSnap_Click);
            // 
            // toolStripCmbImgList
            // 
            this.toolStripCmbImgList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripCmbImgList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripCmbImgList.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.toolStripCmbImgList.FormattingEnabled = true;
            this.toolStripCmbImgList.Location = new System.Drawing.Point(132, 360);
            this.toolStripCmbImgList.Name = "toolStripCmbImgList";
            this.toolStripCmbImgList.Size = new System.Drawing.Size(189, 30);
            this.toolStripCmbImgList.TabIndex = 161;
            // 
            // txtFrameCount
            // 
            this.txtFrameCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFrameCount.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtFrameCount.Location = new System.Drawing.Point(133, 209);
            this.txtFrameCount.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtFrameCount.Maximum = 2500;
            this.txtFrameCount.Minimum = 10;
            this.txtFrameCount.MinimumSize = new System.Drawing.Size(100, 0);
            this.txtFrameCount.Name = "txtFrameCount";
            this.txtFrameCount.ShowText = false;
            this.txtFrameCount.Size = new System.Drawing.Size(187, 41);
            this.txtFrameCount.TabIndex = 160;
            this.txtFrameCount.Text = "uiIntegerUpDown1";
            this.txtFrameCount.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtFrameCount.Value = 50;
            // 
            // txtSequenceDelay
            // 
            this.txtSequenceDelay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSequenceDelay.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSequenceDelay.Location = new System.Drawing.Point(133, 158);
            this.txtSequenceDelay.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtSequenceDelay.Maximum = 60000;
            this.txtSequenceDelay.Minimum = 0;
            this.txtSequenceDelay.MinimumSize = new System.Drawing.Size(100, 0);
            this.txtSequenceDelay.Name = "txtSequenceDelay";
            this.txtSequenceDelay.ShowText = false;
            this.txtSequenceDelay.Size = new System.Drawing.Size(187, 41);
            this.txtSequenceDelay.TabIndex = 159;
            this.txtSequenceDelay.Text = "uiIntegerUpDown1";
            this.txtSequenceDelay.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtSequenceDelay.Value = 50;
            // 
            // cmbSequenceType
            // 
            this.cmbSequenceType.DataSource = null;
            this.cmbSequenceType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbSequenceType.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cmbSequenceType.FillColor = System.Drawing.Color.White;
            this.cmbSequenceType.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbSequenceType.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.cmbSequenceType.Items.AddRange(new object[] {
            "先点亮后抓拍",
            "先抓拍后点亮"});
            this.cmbSequenceType.ItemSelectForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.cmbSequenceType.Location = new System.Drawing.Point(133, 107);
            this.cmbSequenceType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbSequenceType.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmbSequenceType.Name = "cmbSequenceType";
            this.cmbSequenceType.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbSequenceType.Size = new System.Drawing.Size(187, 41);
            this.cmbSequenceType.SymbolSize = 24;
            this.cmbSequenceType.TabIndex = 157;
            this.cmbSequenceType.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbSequenceType.Watermark = "";
            // 
            // txtShutter
            // 
            this.txtShutter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtShutter.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtShutter.Location = new System.Drawing.Point(133, 56);
            this.txtShutter.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtShutter.Maximum = 1000000;
            this.txtShutter.Minimum = 0;
            this.txtShutter.MinimumSize = new System.Drawing.Size(100, 0);
            this.txtShutter.Name = "txtShutter";
            this.txtShutter.ShowText = false;
            this.txtShutter.Size = new System.Drawing.Size(187, 41);
            this.txtShutter.Step = 100;
            this.txtShutter.TabIndex = 149;
            this.txtShutter.Text = null;
            this.txtShutter.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtShutter.Value = 10000;
            // 
            // cmbCameras
            // 
            this.cmbCameras.DataSource = null;
            this.cmbCameras.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbCameras.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cmbCameras.FillColor = System.Drawing.Color.White;
            this.cmbCameras.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbCameras.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.cmbCameras.ItemSelectForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.cmbCameras.Location = new System.Drawing.Point(133, 5);
            this.cmbCameras.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbCameras.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmbCameras.Name = "cmbCameras";
            this.cmbCameras.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbCameras.Size = new System.Drawing.Size(187, 41);
            this.cmbCameras.SymbolSize = 24;
            this.cmbCameras.TabIndex = 148;
            this.cmbCameras.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbCameras.Watermark = "相机列表";
            // 
            // uiMarkLabel5
            // 
            this.uiMarkLabel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMarkLabel5.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiMarkLabel5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel5.Location = new System.Drawing.Point(10, 214);
            this.uiMarkLabel5.Margin = new System.Windows.Forms.Padding(10);
            this.uiMarkLabel5.Name = "uiMarkLabel5";
            this.uiMarkLabel5.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel5.Size = new System.Drawing.Size(109, 31);
            this.uiMarkLabel5.TabIndex = 5;
            this.uiMarkLabel5.Text = "抓拍帧数：";
            this.uiMarkLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel4
            // 
            this.uiMarkLabel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMarkLabel4.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiMarkLabel4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel4.Location = new System.Drawing.Point(10, 163);
            this.uiMarkLabel4.Margin = new System.Windows.Forms.Padding(10);
            this.uiMarkLabel4.Name = "uiMarkLabel4";
            this.uiMarkLabel4.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel4.Size = new System.Drawing.Size(109, 31);
            this.uiMarkLabel4.TabIndex = 4;
            this.uiMarkLabel4.Text = "提前/延后时间：";
            this.uiMarkLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel3
            // 
            this.uiMarkLabel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMarkLabel3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiMarkLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel3.Location = new System.Drawing.Point(10, 112);
            this.uiMarkLabel3.Margin = new System.Windows.Forms.Padding(10);
            this.uiMarkLabel3.Name = "uiMarkLabel3";
            this.uiMarkLabel3.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel3.Size = new System.Drawing.Size(109, 31);
            this.uiMarkLabel3.TabIndex = 3;
            this.uiMarkLabel3.Text = "抓拍时序：";
            this.uiMarkLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel2
            // 
            this.uiMarkLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMarkLabel2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiMarkLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel2.Location = new System.Drawing.Point(10, 61);
            this.uiMarkLabel2.Margin = new System.Windows.Forms.Padding(10);
            this.uiMarkLabel2.Name = "uiMarkLabel2";
            this.uiMarkLabel2.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel2.Size = new System.Drawing.Size(109, 31);
            this.uiMarkLabel2.TabIndex = 2;
            this.uiMarkLabel2.Text = "曝光值：";
            this.uiMarkLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel1
            // 
            this.uiMarkLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMarkLabel1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiMarkLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel1.Location = new System.Drawing.Point(10, 10);
            this.uiMarkLabel1.Margin = new System.Windows.Forms.Padding(10);
            this.uiMarkLabel1.Name = "uiMarkLabel1";
            this.uiMarkLabel1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel1.Size = new System.Drawing.Size(109, 31);
            this.uiMarkLabel1.TabIndex = 1;
            this.uiMarkLabel1.Text = "当前选择相机：";
            this.uiMarkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiTabControl1
            // 
            this.uiTabControl1.Controls.Add(this.tabPage1);
            this.uiTabControl1.Controls.Add(this.tabPage2);
            this.uiTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.uiTabControl1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiTabControl1.ItemSize = new System.Drawing.Size(150, 40);
            this.uiTabControl1.Location = new System.Drawing.Point(0, 0);
            this.uiTabControl1.MainPage = "";
            this.uiTabControl1.Name = "uiTabControl1";
            this.uiTabControl1.SelectedIndex = 0;
            this.uiTabControl1.Size = new System.Drawing.Size(875, 638);
            this.uiTabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.uiTabControl1.TabIndex = 0;
            this.uiTabControl1.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(0, 40);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(875, 598);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "图像";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(875, 598);
            this.panel1.TabIndex = 1;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dgvData);
            this.tabPage2.Location = new System.Drawing.Point(0, 40);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(200, 60);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "数据";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgvData
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.dgvData.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvData.BackgroundColor = System.Drawing.Color.White;
            this.dgvData.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvData.ContextMenuStrip = this.uiContextMenuStrip1;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 7.5F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvData.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvData.EnableHeadersVisualStyles = false;
            this.dgvData.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgvData.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            this.dgvData.Location = new System.Drawing.Point(0, 0);
            this.dgvData.Name = "dgvData";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvData.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvData.RowHeadersWidth = 51;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgvData.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvData.RowTemplate.Height = 23;
            this.dgvData.SelectedIndex = -1;
            this.dgvData.Size = new System.Drawing.Size(200, 60);
            this.dgvData.StripeOddColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.dgvData.TabIndex = 0;
            // 
            // uiContextMenuStrip1
            // 
            this.uiContextMenuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            this.uiContextMenuStrip1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.导出ToolStripMenuItem1});
            this.uiContextMenuStrip1.Name = "uiContextMenuStrip1";
            this.uiContextMenuStrip1.Size = new System.Drawing.Size(107, 26);
            // 
            // 导出ToolStripMenuItem1
            // 
            this.导出ToolStripMenuItem1.Name = "导出ToolStripMenuItem1";
            this.导出ToolStripMenuItem1.Size = new System.Drawing.Size(106, 22);
            this.导出ToolStripMenuItem1.Text = "导出";
            this.导出ToolStripMenuItem1.Click += new System.EventHandler(this.导出ToolStripMenuItem_Click);
            // 
            // FrmDynamicVsionParaConfig
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1210, 673);
            this.Controls.Add(this.uiSplitContainer1);
            this.Name = "FrmDynamicVsionParaConfig";
            this.Text = "动态图像检测-参数设置";
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 800, 480);
            this.uiSplitContainer1.Panel1.ResumeLayout(false);
            this.uiSplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiSplitContainer1)).EndInit();
            this.uiSplitContainer1.ResumeLayout(false);
            this.uiPanel1.ResumeLayout(false);
            this.uiTableLayoutPanel1.ResumeLayout(false);
            this.uiTabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.uiContextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UISplitContainer uiSplitContainer1;
        private Sunny.UI.UIPanel uiPanel1;
        private Sunny.UI.UITableLayoutPanel uiTableLayoutPanel1;
        private Sunny.UI.UIMarkLabel uiMarkLabel5;
        private Sunny.UI.UIMarkLabel uiMarkLabel4;
        private Sunny.UI.UIMarkLabel uiMarkLabel3;
        private Sunny.UI.UIMarkLabel uiMarkLabel2;
        private Sunny.UI.UIMarkLabel uiMarkLabel1;
        private Sunny.UI.UIComboBox cmbCameras;
        private Sunny.UI.UIIntegerUpDown txtShutter;
        private Sunny.UI.UIComboBox cmbSequenceType;
        private Sunny.UI.UIIntegerUpDown txtSequenceDelay;
        private Sunny.UI.UIIntegerUpDown txtFrameCount;
        private System.Windows.Forms.ComboBox toolStripCmbImgList;
        private Sunny.UI.UIButton btnLightAndSnap;
        private Sunny.UI.UIButton btnAddRect;
        private Sunny.UI.UIButton btnFindCircle;
        private Sunny.UI.UIButton btnLearn;
        private Sunny.UI.UIButton btnSave;
        private Sunny.UI.UIRichTextBox txtLearnView;
        private System.Windows.Forms.ComboBox cmbGroup;
        private Sunny.UI.UITabControl uiTabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabPage tabPage2;
        private Sunny.UI.UIDataGridView dgvData;
        private Sunny.UI.UIMarkLabel uiMarkLabel7;
        private Sunny.UI.UIMarkLabel uiMarkLabel6;
        private Sunny.UI.UIButton btnSaveImage;
        private Sunny.UI.UIContextMenuStrip uiContextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 导出ToolStripMenuItem1;
    }
}