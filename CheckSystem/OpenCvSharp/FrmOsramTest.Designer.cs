namespace CheckSystem.OpenCvSharp
{
    partial class FrmOsramTest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmOsramTest));
            this.cmbImageList = new Sunny.UI.UIComboBox();
            this.ImagePanel = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnCaptureImage = new System.Windows.Forms.ToolStripMenuItem();
            this.btnReadImage = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSaveImageBmp = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSaveImageJpg = new System.Windows.Forms.ToolStripMenuItem();
            this.btnClearAllImage = new System.Windows.Forms.ToolStripMenuItem();
            this.funcPanel = new Sunny.UI.UIPanel();
            this.uiCheckBox1 = new Sunny.UI.UICheckBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.btnPositioning = new Sunny.UI.UIButton();
            this.btnAnalysisAll = new Sunny.UI.UIButton();
            this.btnAnalysisNext = new Sunny.UI.UIButton();
            this.btnOpenOKFiles = new Sunny.UI.UIButton();
            this.btnFindBrightSpot = new Sunny.UI.UIButton();
            this.btnCalcLx = new Sunny.UI.UIButton();
            this.btnDrawContour = new Sunny.UI.UIButton();
            this.btnOuterRect = new Sunny.UI.UIButton();
            this.btnOtsu = new Sunny.UI.UIButton();
            this.MainSplitContainer = new Sunny.UI.UISplitContainer();
            this.ImagePanel.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.funcPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).BeginInit();
            this.MainSplitContainer.Panel1.SuspendLayout();
            this.MainSplitContainer.Panel2.SuspendLayout();
            this.MainSplitContainer.SuspendLayout();
            this.SuspendLayout();
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
            this.cmbImageList.Size = new System.Drawing.Size(738, 29);
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
            this.ImagePanel.Size = new System.Drawing.Size(738, 538);
            this.ImagePanel.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 35);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(999, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCaptureImage,
            this.btnReadImage,
            this.btnSaveImageBmp,
            this.btnSaveImageJpg,
            this.btnClearAllImage});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(85, 22);
            this.toolStripDropDownButton1.Text = "图片采集";
            // 
            // btnCaptureImage
            // 
            this.btnCaptureImage.Name = "btnCaptureImage";
            this.btnCaptureImage.Size = new System.Drawing.Size(210, 22);
            this.btnCaptureImage.Text = "从相机采集";
            this.btnCaptureImage.Click += new System.EventHandler(this.btnCaptureImage_Click);
            // 
            // btnReadImage
            // 
            this.btnReadImage.Name = "btnReadImage";
            this.btnReadImage.Size = new System.Drawing.Size(210, 22);
            this.btnReadImage.Text = "从本地读取";
            this.btnReadImage.Click += new System.EventHandler(this.btnReadImage_Click);
            // 
            // btnSaveImageBmp
            // 
            this.btnSaveImageBmp.Name = "btnSaveImageBmp";
            this.btnSaveImageBmp.Size = new System.Drawing.Size(210, 22);
            this.btnSaveImageBmp.Text = "将到前图像保存到(.bmp)";
            this.btnSaveImageBmp.Click += new System.EventHandler(this.btnSaveImageBmp_Click);
            // 
            // btnSaveImageJpg
            // 
            this.btnSaveImageJpg.Name = "btnSaveImageJpg";
            this.btnSaveImageJpg.Size = new System.Drawing.Size(210, 22);
            this.btnSaveImageJpg.Text = "将到前图像保存到(.jpg)";
            this.btnSaveImageJpg.Click += new System.EventHandler(this.btnSaveImageJpg_Click);
            // 
            // btnClearAllImage
            // 
            this.btnClearAllImage.Name = "btnClearAllImage";
            this.btnClearAllImage.Size = new System.Drawing.Size(210, 22);
            this.btnClearAllImage.Text = "清空所有";
            this.btnClearAllImage.Click += new System.EventHandler(this.btnClearAllImage_Click);
            // 
            // funcPanel
            // 
            this.funcPanel.Controls.Add(this.uiCheckBox1);
            this.funcPanel.Controls.Add(this.numericUpDown1);
            this.funcPanel.Controls.Add(this.btnPositioning);
            this.funcPanel.Controls.Add(this.btnAnalysisAll);
            this.funcPanel.Controls.Add(this.btnAnalysisNext);
            this.funcPanel.Controls.Add(this.btnOpenOKFiles);
            this.funcPanel.Controls.Add(this.btnFindBrightSpot);
            this.funcPanel.Controls.Add(this.btnCalcLx);
            this.funcPanel.Controls.Add(this.btnDrawContour);
            this.funcPanel.Controls.Add(this.btnOuterRect);
            this.funcPanel.Controls.Add(this.btnOtsu);
            this.funcPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.funcPanel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.funcPanel.Location = new System.Drawing.Point(0, 0);
            this.funcPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.funcPanel.MinimumSize = new System.Drawing.Size(1, 1);
            this.funcPanel.Name = "funcPanel";
            this.funcPanel.Size = new System.Drawing.Size(250, 538);
            this.funcPanel.TabIndex = 0;
            this.funcPanel.Text = null;
            this.funcPanel.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiCheckBox1
            // 
            this.uiCheckBox1.Checked = true;
            this.uiCheckBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiCheckBox1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiCheckBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiCheckBox1.Location = new System.Drawing.Point(108, 292);
            this.uiCheckBox1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiCheckBox1.Name = "uiCheckBox1";
            this.uiCheckBox1.Size = new System.Drawing.Size(128, 29);
            this.uiCheckBox1.TabIndex = 2;
            this.uiCheckBox1.Text = "显示过程";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(124, 234);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(101, 26);
            this.numericUpDown1.TabIndex = 1;
            this.numericUpDown1.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // btnPositioning
            // 
            this.btnPositioning.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPositioning.Font = new System.Drawing.Font("黑体", 8F);
            this.btnPositioning.Location = new System.Drawing.Point(13, 83);
            this.btnPositioning.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnPositioning.Name = "btnPositioning";
            this.btnPositioning.Size = new System.Drawing.Size(90, 35);
            this.btnPositioning.TabIndex = 0;
            this.btnPositioning.Text = "Positioning";
            this.btnPositioning.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPositioning.Click += new System.EventHandler(this.btnPositioning_Click);
            // 
            // btnAnalysisAll
            // 
            this.btnAnalysisAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAnalysisAll.Font = new System.Drawing.Font("黑体", 8F);
            this.btnAnalysisAll.Location = new System.Drawing.Point(135, 411);
            this.btnAnalysisAll.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnAnalysisAll.Name = "btnAnalysisAll";
            this.btnAnalysisAll.Size = new System.Drawing.Size(90, 35);
            this.btnAnalysisAll.TabIndex = 0;
            this.btnAnalysisAll.Text = "分析所有";
            this.btnAnalysisAll.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAnalysisAll.Click += new System.EventHandler(this.btnAnalysisAll_Click);
            // 
            // btnAnalysisNext
            // 
            this.btnAnalysisNext.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAnalysisNext.Font = new System.Drawing.Font("黑体", 8F);
            this.btnAnalysisNext.Location = new System.Drawing.Point(13, 468);
            this.btnAnalysisNext.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnAnalysisNext.Name = "btnAnalysisNext";
            this.btnAnalysisNext.Size = new System.Drawing.Size(90, 35);
            this.btnAnalysisNext.TabIndex = 0;
            this.btnAnalysisNext.Text = "分析下一张";
            this.btnAnalysisNext.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAnalysisNext.Click += new System.EventHandler(this.btnAnalysisNext_Click);
            // 
            // btnOpenOKFiles
            // 
            this.btnOpenOKFiles.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenOKFiles.Font = new System.Drawing.Font("黑体", 8F);
            this.btnOpenOKFiles.Location = new System.Drawing.Point(13, 411);
            this.btnOpenOKFiles.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnOpenOKFiles.Name = "btnOpenOKFiles";
            this.btnOpenOKFiles.Size = new System.Drawing.Size(90, 35);
            this.btnOpenOKFiles.TabIndex = 0;
            this.btnOpenOKFiles.Text = "打开OK列表";
            this.btnOpenOKFiles.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOpenOKFiles.Click += new System.EventHandler(this.btnOpenOKFiles_Click);
            // 
            // btnFindBrightSpot
            // 
            this.btnFindBrightSpot.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFindBrightSpot.Font = new System.Drawing.Font("黑体", 8F);
            this.btnFindBrightSpot.Location = new System.Drawing.Point(13, 336);
            this.btnFindBrightSpot.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnFindBrightSpot.Name = "btnFindBrightSpot";
            this.btnFindBrightSpot.Size = new System.Drawing.Size(90, 35);
            this.btnFindBrightSpot.TabIndex = 0;
            this.btnFindBrightSpot.Text = "寻亮斑";
            this.btnFindBrightSpot.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnFindBrightSpot.Click += new System.EventHandler(this.btnFindBrightSpot_Click);
            // 
            // btnCalcLx
            // 
            this.btnCalcLx.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCalcLx.Font = new System.Drawing.Font("黑体", 8F);
            this.btnCalcLx.Location = new System.Drawing.Point(13, 234);
            this.btnCalcLx.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnCalcLx.Name = "btnCalcLx";
            this.btnCalcLx.Size = new System.Drawing.Size(90, 35);
            this.btnCalcLx.TabIndex = 0;
            this.btnCalcLx.Text = "Calc";
            this.btnCalcLx.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCalcLx.Click += new System.EventHandler(this.btnCalcLx_Click);
            // 
            // btnDrawContour
            // 
            this.btnDrawContour.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDrawContour.Font = new System.Drawing.Font("黑体", 8F);
            this.btnDrawContour.Location = new System.Drawing.Point(124, 83);
            this.btnDrawContour.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnDrawContour.Name = "btnDrawContour";
            this.btnDrawContour.Size = new System.Drawing.Size(90, 35);
            this.btnDrawContour.TabIndex = 0;
            this.btnDrawContour.Text = "DrawContour";
            this.btnDrawContour.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDrawContour.Click += new System.EventHandler(this.btnDrawContour_Click);
            // 
            // btnOuterRect
            // 
            this.btnOuterRect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOuterRect.Font = new System.Drawing.Font("黑体", 8F);
            this.btnOuterRect.Location = new System.Drawing.Point(124, 21);
            this.btnOuterRect.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnOuterRect.Name = "btnOuterRect";
            this.btnOuterRect.Size = new System.Drawing.Size(90, 35);
            this.btnOuterRect.TabIndex = 0;
            this.btnOuterRect.Text = "OuterRect";
            this.btnOuterRect.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOuterRect.Click += new System.EventHandler(this.btnOuterRect_Click);
            // 
            // btnOtsu
            // 
            this.btnOtsu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOtsu.Font = new System.Drawing.Font("黑体", 8F);
            this.btnOtsu.Location = new System.Drawing.Point(13, 21);
            this.btnOtsu.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnOtsu.Name = "btnOtsu";
            this.btnOtsu.Size = new System.Drawing.Size(90, 35);
            this.btnOtsu.TabIndex = 0;
            this.btnOtsu.Text = "OTSU";
            this.btnOtsu.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOtsu.Click += new System.EventHandler(this.btnOtsu_Click);
            // 
            // MainSplitContainer
            // 
            this.MainSplitContainer.Cursor = System.Windows.Forms.Cursors.Default;
            this.MainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainSplitContainer.Location = new System.Drawing.Point(0, 60);
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
            this.MainSplitContainer.Size = new System.Drawing.Size(999, 538);
            this.MainSplitContainer.SplitterDistance = 250;
            this.MainSplitContainer.SplitterWidth = 11;
            this.MainSplitContainer.TabIndex = 3;
            // 
            // FrmOsramTest
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(999, 598);
            this.Controls.Add(this.MainSplitContainer);
            this.Controls.Add(this.toolStrip1);
            this.Name = "FrmOsramTest";
            this.Text = "FrmOsramTest";
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 800, 450);
            this.ImagePanel.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.funcPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.MainSplitContainer.Panel1.ResumeLayout(false);
            this.MainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).EndInit();
            this.MainSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Sunny.UI.UIComboBox cmbImageList;
        private System.Windows.Forms.Panel ImagePanel;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem btnReadImage;
        private System.Windows.Forms.ToolStripMenuItem btnClearAllImage;
        private Sunny.UI.UIPanel funcPanel;
        private Sunny.UI.UIButton btnOtsu;
        private Sunny.UI.UISplitContainer MainSplitContainer;
        private Sunny.UI.UIButton btnOuterRect;
        private Sunny.UI.UIButton btnCalcLx;
        private Sunny.UI.UIButton btnDrawContour;
        private Sunny.UI.UIButton btnPositioning;
        private System.Windows.Forms.ToolStripMenuItem btnCaptureImage;
        private System.Windows.Forms.ToolStripMenuItem btnSaveImageBmp;
        private System.Windows.Forms.ToolStripMenuItem btnSaveImageJpg;
        private Sunny.UI.UIButton btnFindBrightSpot;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private Sunny.UI.UICheckBox uiCheckBox1;
        private Sunny.UI.UIButton btnOpenOKFiles;
        private Sunny.UI.UIButton btnAnalysisNext;
        private Sunny.UI.UIButton btnAnalysisAll;
    }
}