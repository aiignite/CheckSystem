namespace CheckSystem.PesVision
{
    partial class FrmMdfTest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMdfTest));
            this.cmbImageList = new Sunny.UI.UIComboBox();
            this.ImagePanel = new System.Windows.Forms.Panel();
            this.funcPanel = new Sunny.UI.UIPanel();
            this.txtMdfResult = new Sunny.UI.UITextBox();
            this.uiLabel3 = new Sunny.UI.UILabel();
            this.rtxMdfPath = new Sunny.UI.UIRichTextBox();
            this.rtxClassCPath = new Sunny.UI.UIRichTextBox();
            this.uiLabel2 = new Sunny.UI.UILabel();
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.btnAnalysis = new Sunny.UI.UIButton();
            this.MainSplitContainer = new Sunny.UI.UISplitContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnCaptureImage = new System.Windows.Forms.ToolStripMenuItem();
            this.btnReadImage = new System.Windows.Forms.ToolStripMenuItem();
            this.btnClearAllImage = new System.Windows.Forms.ToolStripMenuItem();
            this.保存当前图片ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ImagePanel.SuspendLayout();
            this.funcPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).BeginInit();
            this.MainSplitContainer.Panel1.SuspendLayout();
            this.MainSplitContainer.Panel2.SuspendLayout();
            this.MainSplitContainer.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
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
            this.cmbImageList.Size = new System.Drawing.Size(764, 29);
            this.cmbImageList.SymbolSize = 24;
            this.cmbImageList.TabIndex = 0;
            this.cmbImageList.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbImageList.Watermark = "";
            // 
            // ImagePanel
            // 
            this.ImagePanel.Controls.Add(this.cmbImageList);
            this.ImagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImagePanel.Location = new System.Drawing.Point(0, 0);
            this.ImagePanel.Name = "ImagePanel";
            this.ImagePanel.Size = new System.Drawing.Size(764, 694);
            this.ImagePanel.TabIndex = 0;
            // 
            // funcPanel
            // 
            this.funcPanel.Controls.Add(this.txtMdfResult);
            this.funcPanel.Controls.Add(this.uiLabel3);
            this.funcPanel.Controls.Add(this.rtxMdfPath);
            this.funcPanel.Controls.Add(this.rtxClassCPath);
            this.funcPanel.Controls.Add(this.uiLabel2);
            this.funcPanel.Controls.Add(this.uiLabel1);
            this.funcPanel.Controls.Add(this.btnAnalysis);
            this.funcPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.funcPanel.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.funcPanel.Location = new System.Drawing.Point(0, 0);
            this.funcPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.funcPanel.MinimumSize = new System.Drawing.Size(1, 1);
            this.funcPanel.Name = "funcPanel";
            this.funcPanel.Size = new System.Drawing.Size(387, 694);
            this.funcPanel.TabIndex = 0;
            this.funcPanel.Text = null;
            this.funcPanel.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtMdfResult
            // 
            this.txtMdfResult.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMdfResult.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtMdfResult.Location = new System.Drawing.Point(17, 628);
            this.txtMdfResult.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtMdfResult.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtMdfResult.Name = "txtMdfResult";
            this.txtMdfResult.Padding = new System.Windows.Forms.Padding(5);
            this.txtMdfResult.ReadOnly = true;
            this.txtMdfResult.ShowText = false;
            this.txtMdfResult.Size = new System.Drawing.Size(315, 29);
            this.txtMdfResult.TabIndex = 5;
            this.txtMdfResult.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtMdfResult.Watermark = "";
            // 
            // uiLabel3
            // 
            this.uiLabel3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel3.Location = new System.Drawing.Point(12, 585);
            this.uiLabel3.Name = "uiLabel3";
            this.uiLabel3.Size = new System.Drawing.Size(171, 23);
            this.uiLabel3.TabIndex = 4;
            this.uiLabel3.Text = "MDF检测结果：";
            this.uiLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rtxMdfPath
            // 
            this.rtxMdfPath.FillColor = System.Drawing.Color.White;
            this.rtxMdfPath.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rtxMdfPath.Location = new System.Drawing.Point(17, 169);
            this.rtxMdfPath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rtxMdfPath.MinimumSize = new System.Drawing.Size(1, 1);
            this.rtxMdfPath.Name = "rtxMdfPath";
            this.rtxMdfPath.Padding = new System.Windows.Forms.Padding(2);
            this.rtxMdfPath.ScrollBarStyleInherited = false;
            this.rtxMdfPath.ShowText = false;
            this.rtxMdfPath.Size = new System.Drawing.Size(357, 85);
            this.rtxMdfPath.TabIndex = 3;
            this.rtxMdfPath.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rtxMdfPath.DoubleClick += new System.EventHandler(this.rtxMdfPath_MouseDoubleClick);
            // 
            // rtxClassCPath
            // 
            this.rtxClassCPath.FillColor = System.Drawing.Color.White;
            this.rtxClassCPath.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rtxClassCPath.Location = new System.Drawing.Point(17, 51);
            this.rtxClassCPath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rtxClassCPath.MinimumSize = new System.Drawing.Size(1, 1);
            this.rtxClassCPath.Name = "rtxClassCPath";
            this.rtxClassCPath.Padding = new System.Windows.Forms.Padding(2);
            this.rtxClassCPath.ReadOnly = true;
            this.rtxClassCPath.ScrollBarStyleInherited = false;
            this.rtxClassCPath.ShowText = false;
            this.rtxClassCPath.Size = new System.Drawing.Size(357, 85);
            this.rtxClassCPath.TabIndex = 3;
            this.rtxClassCPath.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rtxClassCPath.DoubleClick += new System.EventHandler(this.rtxClassCPath_DoubleClick);
            // 
            // uiLabel2
            // 
            this.uiLabel2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel2.Location = new System.Drawing.Point(13, 141);
            this.uiLabel2.Name = "uiLabel2";
            this.uiLabel2.Size = new System.Drawing.Size(100, 23);
            this.uiLabel2.TabIndex = 2;
            this.uiLabel2.Text = "MDF";
            this.uiLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel1
            // 
            this.uiLabel1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel1.Location = new System.Drawing.Point(13, 24);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(100, 23);
            this.uiLabel1.TabIndex = 2;
            this.uiLabel1.Text = "ClassC";
            this.uiLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnAnalysis
            // 
            this.btnAnalysis.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAnalysis.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.btnAnalysis.Location = new System.Drawing.Point(12, 523);
            this.btnAnalysis.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnAnalysis.Name = "btnAnalysis";
            this.btnAnalysis.Size = new System.Drawing.Size(90, 35);
            this.btnAnalysis.TabIndex = 1;
            this.btnAnalysis.Text = "Analysis";
            this.btnAnalysis.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 
            // MainSplitContainer
            // 
            this.MainSplitContainer.Cursor = System.Windows.Forms.Cursors.Default;
            this.MainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainSplitContainer.Location = new System.Drawing.Point(0, 31);
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
            this.MainSplitContainer.Size = new System.Drawing.Size(1162, 694);
            this.MainSplitContainer.SplitterDistance = 387;
            this.MainSplitContainer.SplitterWidth = 11;
            this.MainSplitContainer.TabIndex = 3;
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.toolStripSeparator1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1162, 31);
            this.toolStrip1.TabIndex = 2;
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
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(103, 28);
            this.toolStripDropDownButton1.Text = "图片采集";
            // 
            // btnCaptureImage
            // 
            this.btnCaptureImage.Name = "btnCaptureImage";
            this.btnCaptureImage.Size = new System.Drawing.Size(182, 26);
            this.btnCaptureImage.Text = "从相机采集";
            // 
            // btnReadImage
            // 
            this.btnReadImage.Name = "btnReadImage";
            this.btnReadImage.Size = new System.Drawing.Size(182, 26);
            this.btnReadImage.Text = "从本地读取";
            // 
            // btnClearAllImage
            // 
            this.btnClearAllImage.Name = "btnClearAllImage";
            this.btnClearAllImage.Size = new System.Drawing.Size(182, 26);
            this.btnClearAllImage.Text = "清空所有";
            // 
            // 保存当前图片ToolStripMenuItem
            // 
            this.保存当前图片ToolStripMenuItem.Name = "保存当前图片ToolStripMenuItem";
            this.保存当前图片ToolStripMenuItem.Size = new System.Drawing.Size(182, 26);
            this.保存当前图片ToolStripMenuItem.Text = "保存当前图片";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // FrmMdfTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1162, 725);
            this.Controls.Add(this.MainSplitContainer);
            this.Controls.Add(this.toolStrip1);
            this.Name = "FrmMdfTest";
            this.Text = "MDF光型测试";
            this.ImagePanel.ResumeLayout(false);
            this.funcPanel.ResumeLayout(false);
            this.MainSplitContainer.Panel1.ResumeLayout(false);
            this.MainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).EndInit();
            this.MainSplitContainer.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Sunny.UI.UIComboBox cmbImageList;
        private System.Windows.Forms.Panel ImagePanel;
        private Sunny.UI.UIPanel funcPanel;
        private Sunny.UI.UIRichTextBox rtxMdfPath;
        private Sunny.UI.UIRichTextBox rtxClassCPath;
        private Sunny.UI.UILabel uiLabel2;
        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UIButton btnAnalysis;
        private Sunny.UI.UISplitContainer MainSplitContainer;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem btnCaptureImage;
        private System.Windows.Forms.ToolStripMenuItem btnReadImage;
        private System.Windows.Forms.ToolStripMenuItem btnClearAllImage;
        private System.Windows.Forms.ToolStripMenuItem 保存当前图片ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private Sunny.UI.UITextBox txtMdfResult;
        private Sunny.UI.UILabel uiLabel3;
    }
}