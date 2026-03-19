namespace CheckSystem.AlcmAutoDevice
{
    partial class FrmAlcmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAlcmMain));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbtnA2BLedParaSet = new System.Windows.Forms.ToolStripButton();
            this.tsbtnLedParaSet = new System.Windows.Forms.ToolStripButton();
            this.tsbtnStateConfig = new System.Windows.Forms.ToolStripButton();
            this.tsbtnStateWatch = new System.Windows.Forms.ToolStripButton();
            this.tsbtnControllerOperation = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.uiSplitContainer2 = new Sunny.UI.UISplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtJumpInfo = new Sunny.UI.UITextBox();
            this.uiMarkLabel1 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel2 = new Sunny.UI.UIMarkLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.dataShowTablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.ws1Panel = new Sunny.UI.UITitlePanel();
            this.mainDgv1 = new Sunny.UI.UIDataGridView();
            this.lblA2bLedResult = new Sunny.UI.UIMarkLabel();
            this.lblLedResult = new Sunny.UI.UIMarkLabel();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiSplitContainer2)).BeginInit();
            this.uiSplitContainer2.Panel1.SuspendLayout();
            this.uiSplitContainer2.Panel2.SuspendLayout();
            this.uiSplitContainer2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.dataShowTablePanel.SuspendLayout();
            this.ws1Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainDgv1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnA2BLedParaSet,
            this.tsbtnLedParaSet,
            this.tsbtnStateConfig,
            this.tsbtnStateWatch,
            this.tsbtnControllerOperation,
            this.toolStripButton4});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbtnA2BLedParaSet
            // 
            this.tsbtnA2BLedParaSet.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnA2BLedParaSet.Image")));
            this.tsbtnA2BLedParaSet.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnA2BLedParaSet.Name = "tsbtnA2BLedParaSet";
            this.tsbtnA2BLedParaSet.Size = new System.Drawing.Size(147, 22);
            this.tsbtnA2BLedParaSet.Text = "A2B主从板指示灯标定";
            this.tsbtnA2BLedParaSet.Click += new System.EventHandler(this.tsbtnA2BLedParaSet_Click);
            // 
            // tsbtnLedParaSet
            // 
            this.tsbtnLedParaSet.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnLedParaSet.Image")));
            this.tsbtnLedParaSet.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnLedParaSet.Name = "tsbtnLedParaSet";
            this.tsbtnLedParaSet.Size = new System.Drawing.Size(148, 22);
            this.tsbtnLedParaSet.Text = "负载灯板音乐律动标定";
            this.tsbtnLedParaSet.Click += new System.EventHandler(this.tsbtnLedParaSet_Click);
            // 
            // tsbtnStateConfig
            // 
            this.tsbtnStateConfig.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnStateConfig.Image")));
            this.tsbtnStateConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnStateConfig.Name = "tsbtnStateConfig";
            this.tsbtnStateConfig.Size = new System.Drawing.Size(100, 22);
            this.tsbtnStateConfig.Text = "打开流程编辑";
            this.tsbtnStateConfig.Click += new System.EventHandler(this.tsbtnStateConfig_Click);
            // 
            // tsbtnStateWatch
            // 
            this.tsbtnStateWatch.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnStateWatch.Image")));
            this.tsbtnStateWatch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnStateWatch.Name = "tsbtnStateWatch";
            this.tsbtnStateWatch.Size = new System.Drawing.Size(100, 22);
            this.tsbtnStateWatch.Text = "查看流程状态";
            this.tsbtnStateWatch.Click += new System.EventHandler(this.tsbtnStateWatch_Click);
            // 
            // tsbtnControllerOperation
            // 
            this.tsbtnControllerOperation.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnControllerOperation.Image")));
            this.tsbtnControllerOperation.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnControllerOperation.Name = "tsbtnControllerOperation";
            this.tsbtnControllerOperation.Size = new System.Drawing.Size(136, 22);
            this.tsbtnControllerOperation.Text = "打开控制器调试界面";
            this.tsbtnControllerOperation.Click += new System.EventHandler(this.tsbtnControllerOperation_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(76, 22);
            this.toolStripButton4.Text = "参数设置";
            // 
            // uiSplitContainer2
            // 
            this.uiSplitContainer2.CollapsePanel = Sunny.UI.UISplitContainer.UICollapsePanel.Panel2;
            this.uiSplitContainer2.Cursor = System.Windows.Forms.Cursors.Default;
            this.uiSplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiSplitContainer2.Location = new System.Drawing.Point(0, 25);
            this.uiSplitContainer2.MinimumSize = new System.Drawing.Size(20, 20);
            this.uiSplitContainer2.Name = "uiSplitContainer2";
            // 
            // uiSplitContainer2.Panel1
            // 
            this.uiSplitContainer2.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // uiSplitContainer2.Panel2
            // 
            this.uiSplitContainer2.Panel2.Controls.Add(this.dataShowTablePanel);
            this.uiSplitContainer2.Size = new System.Drawing.Size(800, 455);
            this.uiSplitContainer2.SplitterDistance = 448;
            this.uiSplitContainer2.SplitterWidth = 11;
            this.uiSplitContainer2.TabIndex = 18;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.txtJumpInfo, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.uiMarkLabel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.uiMarkLabel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(448, 455);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // txtJumpInfo
            // 
            this.txtJumpInfo.ButtonWidth = 100;
            this.tableLayoutPanel1.SetColumnSpan(this.txtJumpInfo, 2);
            this.txtJumpInfo.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtJumpInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtJumpInfo.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtJumpInfo.Location = new System.Drawing.Point(4, 340);
            this.txtJumpInfo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtJumpInfo.MinimumSize = new System.Drawing.Size(1, 1);
            this.txtJumpInfo.Multiline = true;
            this.txtJumpInfo.Name = "txtJumpInfo";
            this.txtJumpInfo.Padding = new System.Windows.Forms.Padding(5);
            this.txtJumpInfo.ReadOnly = true;
            this.txtJumpInfo.ShowScrollBar = true;
            this.txtJumpInfo.ShowText = false;
            this.txtJumpInfo.Size = new System.Drawing.Size(440, 110);
            this.txtJumpInfo.TabIndex = 15;
            this.txtJumpInfo.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtJumpInfo.Watermark = "律动信息";
            // 
            // uiMarkLabel1
            // 
            this.uiMarkLabel1.BackColor = System.Drawing.Color.DarkSlateGray;
            this.uiMarkLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMarkLabel1.Font = new System.Drawing.Font("黑体", 15.75F, System.Drawing.FontStyle.Bold);
            this.uiMarkLabel1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.uiMarkLabel1.Location = new System.Drawing.Point(1, 1);
            this.uiMarkLabel1.Margin = new System.Windows.Forms.Padding(1);
            this.uiMarkLabel1.Name = "uiMarkLabel1";
            this.uiMarkLabel1.Padding = new System.Windows.Forms.Padding(1);
            this.uiMarkLabel1.Size = new System.Drawing.Size(222, 73);
            this.uiMarkLabel1.Style = Sunny.UI.UIStyle.Custom;
            this.uiMarkLabel1.TabIndex = 14;
            this.uiMarkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel2
            // 
            this.uiMarkLabel2.BackColor = System.Drawing.Color.DarkGoldenrod;
            this.uiMarkLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMarkLabel2.Font = new System.Drawing.Font("黑体", 15.75F, System.Drawing.FontStyle.Bold);
            this.uiMarkLabel2.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.uiMarkLabel2.Location = new System.Drawing.Point(225, 1);
            this.uiMarkLabel2.Margin = new System.Windows.Forms.Padding(1);
            this.uiMarkLabel2.Name = "uiMarkLabel2";
            this.uiMarkLabel2.Padding = new System.Windows.Forms.Padding(1);
            this.uiMarkLabel2.Size = new System.Drawing.Size(222, 73);
            this.uiMarkLabel2.Style = Sunny.UI.UIStyle.Custom;
            this.uiMarkLabel2.TabIndex = 12;
            this.uiMarkLabel2.Text = "请选择产品";
            this.uiMarkLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.pictureBox1, 2);
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(3, 78);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(442, 254);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            // 
            // dataShowTablePanel
            // 
            this.dataShowTablePanel.ColumnCount = 1;
            this.dataShowTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.dataShowTablePanel.Controls.Add(this.ws1Panel, 0, 2);
            this.dataShowTablePanel.Controls.Add(this.lblA2bLedResult, 0, 1);
            this.dataShowTablePanel.Controls.Add(this.lblLedResult, 0, 0);
            this.dataShowTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataShowTablePanel.Location = new System.Drawing.Point(0, 0);
            this.dataShowTablePanel.Name = "dataShowTablePanel";
            this.dataShowTablePanel.RowCount = 6;
            this.dataShowTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.dataShowTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.dataShowTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.dataShowTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.dataShowTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.dataShowTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.dataShowTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.dataShowTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.dataShowTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.dataShowTablePanel.Size = new System.Drawing.Size(341, 455);
            this.dataShowTablePanel.TabIndex = 0;
            // 
            // ws1Panel
            // 
            this.ws1Panel.Controls.Add(this.mainDgv1);
            this.ws1Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ws1Panel.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.ws1Panel.Location = new System.Drawing.Point(4, 105);
            this.ws1Panel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ws1Panel.MinimumSize = new System.Drawing.Size(1, 1);
            this.ws1Panel.Name = "ws1Panel";
            this.ws1Panel.Padding = new System.Windows.Forms.Padding(0, 35, 0, 0);
            this.dataShowTablePanel.SetRowSpan(this.ws1Panel, 4);
            this.ws1Panel.ShowText = false;
            this.ws1Panel.Size = new System.Drawing.Size(333, 345);
            this.ws1Panel.TabIndex = 14;
            this.ws1Panel.Text = "检测数据";
            this.ws1Panel.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.ws1Panel.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mainDgv1
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            this.mainDgv1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.mainDgv1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            this.mainDgv1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.mainDgv1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.mainDgv1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(236)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.mainDgv1.DefaultCellStyle = dataGridViewCellStyle3;
            this.mainDgv1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainDgv1.EnableHeadersVisualStyles = false;
            this.mainDgv1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mainDgv1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(173)))), ((int)(((byte)(255)))));
            this.mainDgv1.Location = new System.Drawing.Point(0, 35);
            this.mainDgv1.Name = "mainDgv1";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.mainDgv1.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(236)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.mainDgv1.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.mainDgv1.RowTemplate.Height = 23;
            this.mainDgv1.ScrollBarStyleInherited = false;
            this.mainDgv1.SelectedIndex = -1;
            this.mainDgv1.Size = new System.Drawing.Size(333, 310);
            this.mainDgv1.TabIndex = 4;
            // 
            // lblA2bLedResult
            // 
            this.lblA2bLedResult.BackColor = System.Drawing.Color.DarkGoldenrod;
            this.lblA2bLedResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblA2bLedResult.Font = new System.Drawing.Font("黑体", 15.75F, System.Drawing.FontStyle.Bold);
            this.lblA2bLedResult.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblA2bLedResult.Location = new System.Drawing.Point(1, 51);
            this.lblA2bLedResult.Margin = new System.Windows.Forms.Padding(1);
            this.lblA2bLedResult.Name = "lblA2bLedResult";
            this.lblA2bLedResult.Padding = new System.Windows.Forms.Padding(1);
            this.lblA2bLedResult.Size = new System.Drawing.Size(339, 48);
            this.lblA2bLedResult.Style = Sunny.UI.UIStyle.Custom;
            this.lblA2bLedResult.TabIndex = 13;
            this.lblA2bLedResult.Text = "A2B主从板指示灯";
            this.lblA2bLedResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLedResult
            // 
            this.lblLedResult.BackColor = System.Drawing.Color.DarkGoldenrod;
            this.lblLedResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLedResult.Font = new System.Drawing.Font("黑体", 15.75F, System.Drawing.FontStyle.Bold);
            this.lblLedResult.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblLedResult.Location = new System.Drawing.Point(1, 1);
            this.lblLedResult.Margin = new System.Windows.Forms.Padding(1);
            this.lblLedResult.Name = "lblLedResult";
            this.lblLedResult.Padding = new System.Windows.Forms.Padding(1);
            this.lblLedResult.Size = new System.Drawing.Size(339, 48);
            this.lblLedResult.Style = Sunny.UI.UIStyle.Custom;
            this.lblLedResult.TabIndex = 12;
            this.lblLedResult.Text = "负载灯板音乐律动";
            this.lblLedResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FrmAlcmMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(800, 480);
            this.Controls.Add(this.uiSplitContainer2);
            this.Controls.Add(this.toolStrip1);
            this.Name = "FrmAlcmMain";
            this.Text = "ALCM音乐氛围灯模块";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.uiSplitContainer2.Panel1.ResumeLayout(false);
            this.uiSplitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiSplitContainer2)).EndInit();
            this.uiSplitContainer2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.dataShowTablePanel.ResumeLayout(false);
            this.ws1Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainDgv1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbtnA2BLedParaSet;
        private System.Windows.Forms.ToolStripButton tsbtnLedParaSet;
        private Sunny.UI.UISplitContainer uiSplitContainer2;
        private System.Windows.Forms.TableLayoutPanel dataShowTablePanel;
        private Sunny.UI.UIMarkLabel lblLedResult;
        private System.Windows.Forms.ToolStripButton tsbtnStateConfig;
        private System.Windows.Forms.ToolStripButton tsbtnStateWatch;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Sunny.UI.UIMarkLabel uiMarkLabel1;
        private Sunny.UI.UIMarkLabel uiMarkLabel2;
        private System.Windows.Forms.ToolStripButton tsbtnControllerOperation;
        private Sunny.UI.UITextBox txtJumpInfo;
        private Sunny.UI.UIMarkLabel lblA2bLedResult;
        private Sunny.UI.UITitlePanel ws1Panel;
        private Sunny.UI.UIDataGridView mainDgv1;
    }
}