namespace CheckSystem.PesVision
{
    partial class FrmPesTest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPesTest));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnCaptureImage = new System.Windows.Forms.ToolStripMenuItem();
            this.btnReadImage = new System.Windows.Forms.ToolStripMenuItem();
            this.btnClearAllImage = new System.Windows.Forms.ToolStripMenuItem();
            this.保存当前图片ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripLabel();
            this.MainSplitContainer = new Sunny.UI.UISplitContainer();
            this.funcPanel = new Sunny.UI.UIPanel();
            this.uiButton14 = new Sunny.UI.UIButton();
            this.btnCaptureHb = new Sunny.UI.UIButton();
            this.uiButton10 = new Sunny.UI.UIButton();
            this.uiButton8 = new Sunny.UI.UIButton();
            this.uiButton6 = new Sunny.UI.UIButton();
            this.uiButton4 = new Sunny.UI.UIButton();
            this.btnFindCutOffLine = new Sunny.UI.UIButton();
            this.btnAnalysisHb = new Sunny.UI.UIButton();
            this.btnAnalysis = new Sunny.UI.UIButton();
            this.btnCaptureLb = new Sunny.UI.UIButton();
            this.uiButton9 = new Sunny.UI.UIButton();
            this.uiButton7 = new Sunny.UI.UIButton();
            this.uiButton5 = new Sunny.UI.UIButton();
            this.uiButton2 = new Sunny.UI.UIButton();
            this.btnDeNoise = new Sunny.UI.UIButton();
            this.btnSetRoi = new Sunny.UI.UIButton();
            this.ImagePanel = new System.Windows.Forms.Panel();
            this.cmbImageList = new Sunny.UI.UIComboBox();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).BeginInit();
            this.MainSplitContainer.Panel1.SuspendLayout();
            this.MainSplitContainer.Panel2.SuspendLayout();
            this.MainSplitContainer.SuspendLayout();
            this.funcPanel.SuspendLayout();
            this.ImagePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.toolStripSeparator1,
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 35);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(900, 27);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCaptureImage,
            this.btnReadImage,
            this.btnClearAllImage,
            this.保存当前图片ToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(89, 24);
            this.toolStripDropDownButton1.Text = "图片采集";
            // 
            // btnCaptureImage
            // 
            this.btnCaptureImage.Name = "btnCaptureImage";
            this.btnCaptureImage.Size = new System.Drawing.Size(180, 22);
            this.btnCaptureImage.Text = "从相机采集";
            // 
            // btnReadImage
            // 
            this.btnReadImage.Name = "btnReadImage";
            this.btnReadImage.Size = new System.Drawing.Size(180, 22);
            this.btnReadImage.Text = "从本地读取";
            // 
            // btnClearAllImage
            // 
            this.btnClearAllImage.Name = "btnClearAllImage";
            this.btnClearAllImage.Size = new System.Drawing.Size(180, 22);
            this.btnClearAllImage.Text = "清空所有";
            // 
            // 保存当前图片ToolStripMenuItem
            // 
            this.保存当前图片ToolStripMenuItem.Name = "保存当前图片ToolStripMenuItem";
            this.保存当前图片ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.保存当前图片ToolStripMenuItem.Text = "保存当前图片";
            this.保存当前图片ToolStripMenuItem.Click += new System.EventHandler(this.保存当前图片ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(103, 24);
            this.toolStripButton1.Text = "toolStripButton1";
            // 
            // MainSplitContainer
            // 
            this.MainSplitContainer.Cursor = System.Windows.Forms.Cursors.Default;
            this.MainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainSplitContainer.Location = new System.Drawing.Point(0, 62);
            this.MainSplitContainer.MinimumSize = new System.Drawing.Size(20, 20);
            this.MainSplitContainer.Name = "MainSplitContainer";
            // 
            // MainSplitContainer.Panel1
            // 
            this.MainSplitContainer.Panel1.Controls.Add(this.funcPanel);
            // 
            // MainSplitContainer.Panel2
            // 
            this.MainSplitContainer.Panel2.Controls.Add(this.ImagePanel);
            this.MainSplitContainer.Size = new System.Drawing.Size(900, 638);
            this.MainSplitContainer.SplitterDistance = 300;
            this.MainSplitContainer.SplitterWidth = 11;
            this.MainSplitContainer.TabIndex = 1;
            // 
            // funcPanel
            // 
            this.funcPanel.Controls.Add(this.uiButton14);
            this.funcPanel.Controls.Add(this.btnCaptureHb);
            this.funcPanel.Controls.Add(this.uiButton10);
            this.funcPanel.Controls.Add(this.uiButton8);
            this.funcPanel.Controls.Add(this.uiButton6);
            this.funcPanel.Controls.Add(this.uiButton4);
            this.funcPanel.Controls.Add(this.btnFindCutOffLine);
            this.funcPanel.Controls.Add(this.btnAnalysisHb);
            this.funcPanel.Controls.Add(this.btnAnalysis);
            this.funcPanel.Controls.Add(this.btnCaptureLb);
            this.funcPanel.Controls.Add(this.uiButton9);
            this.funcPanel.Controls.Add(this.uiButton7);
            this.funcPanel.Controls.Add(this.uiButton5);
            this.funcPanel.Controls.Add(this.uiButton2);
            this.funcPanel.Controls.Add(this.btnDeNoise);
            this.funcPanel.Controls.Add(this.btnSetRoi);
            this.funcPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.funcPanel.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.funcPanel.Location = new System.Drawing.Point(0, 0);
            this.funcPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.funcPanel.MinimumSize = new System.Drawing.Size(1, 1);
            this.funcPanel.Name = "funcPanel";
            this.funcPanel.Size = new System.Drawing.Size(300, 638);
            this.funcPanel.TabIndex = 0;
            this.funcPanel.Text = null;
            this.funcPanel.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiButton14
            // 
            this.uiButton14.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton14.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiButton14.Location = new System.Drawing.Point(13, 450);
            this.uiButton14.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton14.Name = "uiButton14";
            this.uiButton14.Size = new System.Drawing.Size(90, 35);
            this.uiButton14.TabIndex = 1;
            this.uiButton14.Text = "获取极值";
            this.uiButton14.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton14.Click += new System.EventHandler(this.uiButton14_Click);
            // 
            // btnCaptureHb
            // 
            this.btnCaptureHb.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCaptureHb.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.btnCaptureHb.Location = new System.Drawing.Point(204, 306);
            this.btnCaptureHb.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnCaptureHb.Name = "btnCaptureHb";
            this.btnCaptureHb.Size = new System.Drawing.Size(90, 35);
            this.btnCaptureHb.TabIndex = 1;
            this.btnCaptureHb.Text = "拍远光";
            this.btnCaptureHb.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCaptureHb.Click += new System.EventHandler(this.btnCaptureHb_Click);
            // 
            // uiButton10
            // 
            this.uiButton10.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton10.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiButton10.Location = new System.Drawing.Point(204, 249);
            this.uiButton10.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton10.Name = "uiButton10";
            this.uiButton10.Size = new System.Drawing.Size(90, 35);
            this.uiButton10.TabIndex = 1;
            this.uiButton10.Text = "uiButton1";
            this.uiButton10.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 
            // uiButton8
            // 
            this.uiButton8.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton8.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiButton8.Location = new System.Drawing.Point(204, 194);
            this.uiButton8.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton8.Name = "uiButton8";
            this.uiButton8.Size = new System.Drawing.Size(90, 35);
            this.uiButton8.TabIndex = 1;
            this.uiButton8.Text = "uiButton1";
            this.uiButton8.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 
            // uiButton6
            // 
            this.uiButton6.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton6.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiButton6.Location = new System.Drawing.Point(204, 132);
            this.uiButton6.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton6.Name = "uiButton6";
            this.uiButton6.Size = new System.Drawing.Size(90, 35);
            this.uiButton6.TabIndex = 1;
            this.uiButton6.Text = "uiButton1";
            this.uiButton6.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 
            // uiButton4
            // 
            this.uiButton4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton4.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiButton4.Location = new System.Drawing.Point(204, 73);
            this.uiButton4.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton4.Name = "uiButton4";
            this.uiButton4.Size = new System.Drawing.Size(90, 35);
            this.uiButton4.TabIndex = 1;
            this.uiButton4.Text = "uiButton1";
            this.uiButton4.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 
            // btnFindCutOffLine
            // 
            this.btnFindCutOffLine.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFindCutOffLine.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.btnFindCutOffLine.Location = new System.Drawing.Point(204, 21);
            this.btnFindCutOffLine.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnFindCutOffLine.Name = "btnFindCutOffLine";
            this.btnFindCutOffLine.Size = new System.Drawing.Size(90, 35);
            this.btnFindCutOffLine.TabIndex = 1;
            this.btnFindCutOffLine.Text = "FindCutOffLine";
            this.btnFindCutOffLine.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 
            // btnAnalysisHb
            // 
            this.btnAnalysisHb.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAnalysisHb.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.btnAnalysisHb.Location = new System.Drawing.Point(109, 579);
            this.btnAnalysisHb.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnAnalysisHb.Name = "btnAnalysisHb";
            this.btnAnalysisHb.Size = new System.Drawing.Size(90, 35);
            this.btnAnalysisHb.TabIndex = 1;
            this.btnAnalysisHb.Text = "Analysis单HB";
            this.btnAnalysisHb.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 
            // btnAnalysis
            // 
            this.btnAnalysis.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAnalysis.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.btnAnalysis.Location = new System.Drawing.Point(13, 579);
            this.btnAnalysis.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnAnalysis.Name = "btnAnalysis";
            this.btnAnalysis.Size = new System.Drawing.Size(90, 35);
            this.btnAnalysis.TabIndex = 1;
            this.btnAnalysis.Text = "Analysis";
            this.btnAnalysis.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 
            // btnCaptureLb
            // 
            this.btnCaptureLb.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCaptureLb.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.btnCaptureLb.Location = new System.Drawing.Point(109, 306);
            this.btnCaptureLb.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnCaptureLb.Name = "btnCaptureLb";
            this.btnCaptureLb.Size = new System.Drawing.Size(90, 35);
            this.btnCaptureLb.TabIndex = 1;
            this.btnCaptureLb.Text = "拍近光";
            this.btnCaptureLb.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCaptureLb.Click += new System.EventHandler(this.btnCaptureLb_Click);
            // 
            // uiButton9
            // 
            this.uiButton9.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton9.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiButton9.Location = new System.Drawing.Point(109, 249);
            this.uiButton9.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton9.Name = "uiButton9";
            this.uiButton9.Size = new System.Drawing.Size(90, 35);
            this.uiButton9.TabIndex = 1;
            this.uiButton9.Text = "uiButton1";
            this.uiButton9.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 
            // uiButton7
            // 
            this.uiButton7.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton7.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiButton7.Location = new System.Drawing.Point(109, 194);
            this.uiButton7.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton7.Name = "uiButton7";
            this.uiButton7.Size = new System.Drawing.Size(90, 35);
            this.uiButton7.TabIndex = 1;
            this.uiButton7.Text = "uiButton1";
            this.uiButton7.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 
            // uiButton5
            // 
            this.uiButton5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton5.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiButton5.Location = new System.Drawing.Point(109, 132);
            this.uiButton5.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton5.Name = "uiButton5";
            this.uiButton5.Size = new System.Drawing.Size(90, 35);
            this.uiButton5.TabIndex = 1;
            this.uiButton5.Text = "uiButton1";
            this.uiButton5.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 
            // uiButton2
            // 
            this.uiButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton2.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiButton2.Location = new System.Drawing.Point(109, 73);
            this.uiButton2.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton2.Name = "uiButton2";
            this.uiButton2.Size = new System.Drawing.Size(90, 35);
            this.uiButton2.TabIndex = 1;
            this.uiButton2.Text = "uiButton1";
            this.uiButton2.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 
            // btnDeNoise
            // 
            this.btnDeNoise.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeNoise.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.btnDeNoise.Location = new System.Drawing.Point(109, 21);
            this.btnDeNoise.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnDeNoise.Name = "btnDeNoise";
            this.btnDeNoise.Size = new System.Drawing.Size(90, 35);
            this.btnDeNoise.TabIndex = 1;
            this.btnDeNoise.Text = "DeNoise";
            this.btnDeNoise.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 
            // btnSetRoi
            // 
            this.btnSetRoi.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSetRoi.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.btnSetRoi.Location = new System.Drawing.Point(13, 21);
            this.btnSetRoi.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnSetRoi.Name = "btnSetRoi";
            this.btnSetRoi.Size = new System.Drawing.Size(90, 35);
            this.btnSetRoi.TabIndex = 0;
            this.btnSetRoi.Text = "SetRoi";
            this.btnSetRoi.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 
            // ImagePanel
            // 
            this.ImagePanel.Controls.Add(this.cmbImageList);
            this.ImagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImagePanel.Location = new System.Drawing.Point(0, 0);
            this.ImagePanel.Name = "ImagePanel";
            this.ImagePanel.Size = new System.Drawing.Size(589, 638);
            this.ImagePanel.TabIndex = 0;
            // 
            // cmbImageList
            // 
            this.cmbImageList.DataSource = null;
            this.cmbImageList.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbImageList.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cmbImageList.FillColor = System.Drawing.Color.White;
            this.cmbImageList.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbImageList.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.cmbImageList.ItemSelectForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.cmbImageList.Location = new System.Drawing.Point(0, 0);
            this.cmbImageList.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbImageList.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmbImageList.Name = "cmbImageList";
            this.cmbImageList.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbImageList.Size = new System.Drawing.Size(589, 29);
            this.cmbImageList.SymbolSize = 24;
            this.cmbImageList.TabIndex = 0;
            this.cmbImageList.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbImageList.Watermark = "";
            // 
            // FrmPesTest
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(900, 700);
            this.Controls.Add(this.MainSplitContainer);
            this.Controls.Add(this.toolStrip1);
            this.Name = "FrmPesTest";
            this.Text = "PES光型测试工具";
            this.TitleFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 800, 450);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.MainSplitContainer.Panel1.ResumeLayout(false);
            this.MainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).EndInit();
            this.MainSplitContainer.ResumeLayout(false);
            this.funcPanel.ResumeLayout(false);
            this.ImagePanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem btnCaptureImage;
        private System.Windows.Forms.ToolStripMenuItem btnReadImage;
        private System.Windows.Forms.ToolStripMenuItem btnClearAllImage;
        private Sunny.UI.UISplitContainer MainSplitContainer;
        private System.Windows.Forms.Panel ImagePanel;
        private Sunny.UI.UIComboBox cmbImageList;
        private Sunny.UI.UIPanel funcPanel;
        private Sunny.UI.UIButton btnSetRoi;
        private Sunny.UI.UIButton uiButton14;
        private Sunny.UI.UIButton btnCaptureHb;
        private Sunny.UI.UIButton uiButton10;
        private Sunny.UI.UIButton uiButton8;
        private Sunny.UI.UIButton uiButton6;
        private Sunny.UI.UIButton uiButton4;
        private Sunny.UI.UIButton btnFindCutOffLine;
        private Sunny.UI.UIButton btnAnalysis;
        private Sunny.UI.UIButton btnCaptureLb;
        private Sunny.UI.UIButton uiButton9;
        private Sunny.UI.UIButton uiButton7;
        private Sunny.UI.UIButton uiButton5;
        private Sunny.UI.UIButton uiButton2;
        private Sunny.UI.UIButton btnDeNoise;
        private System.Windows.Forms.ToolStripMenuItem 保存当前图片ToolStripMenuItem;
        private System.Windows.Forms.ToolStripLabel toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private Sunny.UI.UIButton btnAnalysisHb;
    }
}