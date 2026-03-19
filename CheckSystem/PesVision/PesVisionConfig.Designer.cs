namespace CheckSystem.PesVision
{
    partial class PesVisionConfig
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PesVisionConfig));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnStartGrab = new System.Windows.Forms.ToolStripButton();
            this.btnStopGrab = new System.Windows.Forms.ToolStripButton();
            this.btnSnapshot = new System.Windows.Forms.ToolStripButton();
            this.btnSetShutter = new System.Windows.Forms.ToolStripButton();
            this.btnLampControl = new System.Windows.Forms.ToolStripButton();
            this.btnLoadLocalLbImage = new System.Windows.Forms.ToolStripButton();
            this.btnLoadLocalHbImage = new System.Windows.Forms.ToolStripButton();
            this.btnExecute = new System.Windows.Forms.ToolStripButton();
            this.uiTabControl1 = new Sunny.UI.UITabControl();
            this.tbPageCamera = new System.Windows.Forms.TabPage();
            this.mainImageTable = new Sunny.UI.UITableLayoutPanel();
            this.gpHighBeam = new Sunny.UI.UIGroupBox();
            this.gpLowBeam = new Sunny.UI.UIGroupBox();
            this.gpLowBeamWithHighShutter = new Sunny.UI.UIGroupBox();
            this.gpCamera = new Sunny.UI.UIGroupBox();
            this.tbPageParaSet = new System.Windows.Forms.TabPage();
            this.uiTableLayoutPanel1 = new Sunny.UI.UITableLayoutPanel();
            this.gpToShowDetail = new Sunny.UI.UIGroupBox();
            this.analysisFlowPanel = new Sunny.UI.UIFlowLayoutPanel();
            this.uiRichTextBox1 = new Sunny.UI.UIRichTextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiAddLowBeamHighShutter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAddLowBeam = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAddHighBeam = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.uiTabControl1.SuspendLayout();
            this.tbPageCamera.SuspendLayout();
            this.mainImageTable.SuspendLayout();
            this.tbPageParaSet.SuspendLayout();
            this.uiTableLayoutPanel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnStartGrab,
            this.btnStopGrab,
            this.btnSnapshot,
            this.btnSetShutter,
            this.btnLampControl,
            this.btnLoadLocalLbImage,
            this.btnLoadLocalHbImage,
            this.btnExecute});
            this.toolStrip1.Location = new System.Drawing.Point(0, 35);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1536, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnStartGrab
            // 
            this.btnStartGrab.Image = ((System.Drawing.Image)(resources.GetObject("btnStartGrab.Image")));
            this.btnStartGrab.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStartGrab.Name = "btnStartGrab";
            this.btnStartGrab.Size = new System.Drawing.Size(76, 22);
            this.btnStartGrab.Text = "开始采集";
            this.btnStartGrab.Click += new System.EventHandler(this.btnStartGrab_Click);
            // 
            // btnStopGrab
            // 
            this.btnStopGrab.Enabled = false;
            this.btnStopGrab.Image = ((System.Drawing.Image)(resources.GetObject("btnStopGrab.Image")));
            this.btnStopGrab.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStopGrab.Name = "btnStopGrab";
            this.btnStopGrab.Size = new System.Drawing.Size(76, 22);
            this.btnStopGrab.Text = "停止采集";
            this.btnStopGrab.Click += new System.EventHandler(this.btnStopGrab_Click);
            // 
            // btnSnapshot
            // 
            this.btnSnapshot.Image = ((System.Drawing.Image)(resources.GetObject("btnSnapshot.Image")));
            this.btnSnapshot.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSnapshot.Name = "btnSnapshot";
            this.btnSnapshot.Size = new System.Drawing.Size(76, 22);
            this.btnSnapshot.Text = "抓拍一张";
            this.btnSnapshot.Click += new System.EventHandler(this.btnSnapshot_Click);
            // 
            // btnSetShutter
            // 
            this.btnSetShutter.Image = ((System.Drawing.Image)(resources.GetObject("btnSetShutter.Image")));
            this.btnSetShutter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSetShutter.Name = "btnSetShutter";
            this.btnSetShutter.Size = new System.Drawing.Size(76, 22);
            this.btnSetShutter.Text = "调整曝光";
            this.btnSetShutter.Click += new System.EventHandler(this.btnSetShutter_Click);
            // 
            // btnLampControl
            // 
            this.btnLampControl.Image = ((System.Drawing.Image)(resources.GetObject("btnLampControl.Image")));
            this.btnLampControl.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLampControl.Name = "btnLampControl";
            this.btnLampControl.Size = new System.Drawing.Size(88, 22);
            this.btnLampControl.Text = "点灯与控制";
            // 
            // btnLoadLocalLbImage
            // 
            this.btnLoadLocalLbImage.Image = ((System.Drawing.Image)(resources.GetObject("btnLoadLocalLbImage.Image")));
            this.btnLoadLocalLbImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLoadLocalLbImage.Name = "btnLoadLocalLbImage";
            this.btnLoadLocalLbImage.Size = new System.Drawing.Size(124, 22);
            this.btnLoadLocalLbImage.Text = "选取本地近光图片";
            this.btnLoadLocalLbImage.Click += new System.EventHandler(this.btnLoadLocalLbImage_Click);
            // 
            // btnLoadLocalHbImage
            // 
            this.btnLoadLocalHbImage.Image = ((System.Drawing.Image)(resources.GetObject("btnLoadLocalHbImage.Image")));
            this.btnLoadLocalHbImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLoadLocalHbImage.Name = "btnLoadLocalHbImage";
            this.btnLoadLocalHbImage.Size = new System.Drawing.Size(124, 22);
            this.btnLoadLocalHbImage.Text = "选取本地远光图片";
            this.btnLoadLocalHbImage.Click += new System.EventHandler(this.btnLoadLocalHbImage_Click);
            // 
            // btnExecute
            // 
            this.btnExecute.Image = ((System.Drawing.Image)(resources.GetObject("btnExecute.Image")));
            this.btnExecute.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(52, 22);
            this.btnExecute.Text = "执行";
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // uiTabControl1
            // 
            this.uiTabControl1.Controls.Add(this.tbPageCamera);
            this.uiTabControl1.Controls.Add(this.tbPageParaSet);
            this.uiTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.uiTabControl1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiTabControl1.ItemSize = new System.Drawing.Size(150, 40);
            this.uiTabControl1.Location = new System.Drawing.Point(0, 60);
            this.uiTabControl1.MainPage = "";
            this.uiTabControl1.Name = "uiTabControl1";
            this.uiTabControl1.SelectedIndex = 0;
            this.uiTabControl1.Size = new System.Drawing.Size(1536, 764);
            this.uiTabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.uiTabControl1.TabIndex = 1;
            this.uiTabControl1.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 
            // tbPageCamera
            // 
            this.tbPageCamera.Controls.Add(this.mainImageTable);
            this.tbPageCamera.Location = new System.Drawing.Point(0, 40);
            this.tbPageCamera.Name = "tbPageCamera";
            this.tbPageCamera.Size = new System.Drawing.Size(1536, 724);
            this.tbPageCamera.TabIndex = 0;
            this.tbPageCamera.Text = "图像采集";
            this.tbPageCamera.UseVisualStyleBackColor = true;
            // 
            // mainImageTable
            // 
            this.mainImageTable.ColumnCount = 2;
            this.mainImageTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainImageTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainImageTable.Controls.Add(this.gpHighBeam, 1, 1);
            this.mainImageTable.Controls.Add(this.gpLowBeam, 0, 1);
            this.mainImageTable.Controls.Add(this.gpLowBeamWithHighShutter, 1, 0);
            this.mainImageTable.Controls.Add(this.gpCamera, 0, 0);
            this.mainImageTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainImageTable.Location = new System.Drawing.Point(0, 0);
            this.mainImageTable.Name = "mainImageTable";
            this.mainImageTable.RowCount = 2;
            this.mainImageTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainImageTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainImageTable.Size = new System.Drawing.Size(1536, 724);
            this.mainImageTable.TabIndex = 0;
            this.mainImageTable.TagString = null;
            // 
            // gpHighBeam
            // 
            this.gpHighBeam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gpHighBeam.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gpHighBeam.Location = new System.Drawing.Point(769, 363);
            this.gpHighBeam.Margin = new System.Windows.Forms.Padding(1);
            this.gpHighBeam.MinimumSize = new System.Drawing.Size(1, 1);
            this.gpHighBeam.Name = "gpHighBeam";
            this.gpHighBeam.Padding = new System.Windows.Forms.Padding(1, 32, 1, 1);
            this.gpHighBeam.Size = new System.Drawing.Size(766, 360);
            this.gpHighBeam.TabIndex = 3;
            this.gpHighBeam.Text = "远光";
            this.gpHighBeam.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gpLowBeam
            // 
            this.gpLowBeam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gpLowBeam.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gpLowBeam.Location = new System.Drawing.Point(1, 363);
            this.gpLowBeam.Margin = new System.Windows.Forms.Padding(1);
            this.gpLowBeam.MinimumSize = new System.Drawing.Size(1, 1);
            this.gpLowBeam.Name = "gpLowBeam";
            this.gpLowBeam.Padding = new System.Windows.Forms.Padding(1, 32, 1, 1);
            this.gpLowBeam.Size = new System.Drawing.Size(766, 360);
            this.gpLowBeam.TabIndex = 2;
            this.gpLowBeam.Text = "近光";
            this.gpLowBeam.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gpLowBeamWithHighShutter
            // 
            this.gpLowBeamWithHighShutter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gpLowBeamWithHighShutter.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gpLowBeamWithHighShutter.Location = new System.Drawing.Point(769, 1);
            this.gpLowBeamWithHighShutter.Margin = new System.Windows.Forms.Padding(1);
            this.gpLowBeamWithHighShutter.MinimumSize = new System.Drawing.Size(1, 1);
            this.gpLowBeamWithHighShutter.Name = "gpLowBeamWithHighShutter";
            this.gpLowBeamWithHighShutter.Padding = new System.Windows.Forms.Padding(1, 32, 1, 1);
            this.gpLowBeamWithHighShutter.Size = new System.Drawing.Size(766, 360);
            this.gpLowBeamWithHighShutter.TabIndex = 1;
            this.gpLowBeamWithHighShutter.Text = "近光（用于识别截止线）";
            this.gpLowBeamWithHighShutter.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gpCamera
            // 
            this.gpCamera.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gpCamera.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gpCamera.Location = new System.Drawing.Point(1, 1);
            this.gpCamera.Margin = new System.Windows.Forms.Padding(1);
            this.gpCamera.MinimumSize = new System.Drawing.Size(1, 1);
            this.gpCamera.Name = "gpCamera";
            this.gpCamera.Padding = new System.Windows.Forms.Padding(1, 32, 1, 1);
            this.gpCamera.Size = new System.Drawing.Size(766, 360);
            this.gpCamera.TabIndex = 0;
            this.gpCamera.Text = "相机采集";
            this.gpCamera.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbPageParaSet
            // 
            this.tbPageParaSet.Controls.Add(this.uiTableLayoutPanel1);
            this.tbPageParaSet.Location = new System.Drawing.Point(0, 40);
            this.tbPageParaSet.Name = "tbPageParaSet";
            this.tbPageParaSet.Size = new System.Drawing.Size(200, 60);
            this.tbPageParaSet.TabIndex = 4;
            this.tbPageParaSet.Text = "执行过程";
            this.tbPageParaSet.UseVisualStyleBackColor = true;
            // 
            // uiTableLayoutPanel1
            // 
            this.uiTableLayoutPanel1.ColumnCount = 3;
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 400F));
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.uiTableLayoutPanel1.Controls.Add(this.gpToShowDetail, 0, 0);
            this.uiTableLayoutPanel1.Controls.Add(this.analysisFlowPanel, 0, 0);
            this.uiTableLayoutPanel1.Controls.Add(this.uiRichTextBox1, 1, 1);
            this.uiTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.uiTableLayoutPanel1.Name = "uiTableLayoutPanel1";
            this.uiTableLayoutPanel1.RowCount = 2;
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.uiTableLayoutPanel1.Size = new System.Drawing.Size(200, 60);
            this.uiTableLayoutPanel1.TabIndex = 0;
            this.uiTableLayoutPanel1.TagString = null;
            // 
            // gpToShowDetail
            // 
            this.uiTableLayoutPanel1.SetColumnSpan(this.gpToShowDetail, 2);
            this.gpToShowDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gpToShowDetail.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gpToShowDetail.Location = new System.Drawing.Point(401, 1);
            this.gpToShowDetail.Margin = new System.Windows.Forms.Padding(1);
            this.gpToShowDetail.MinimumSize = new System.Drawing.Size(1, 1);
            this.gpToShowDetail.Name = "gpToShowDetail";
            this.gpToShowDetail.Padding = new System.Windows.Forms.Padding(1, 32, 1, 1);
            this.gpToShowDetail.Size = new System.Drawing.Size(1, 40);
            this.gpToShowDetail.TabIndex = 1;
            this.gpToShowDetail.Text = "待详细显示图像";
            this.gpToShowDetail.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // analysisFlowPanel
            // 
            this.analysisFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.analysisFlowPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.analysisFlowPanel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.analysisFlowPanel.Location = new System.Drawing.Point(4, 5);
            this.analysisFlowPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.analysisFlowPanel.MinimumSize = new System.Drawing.Size(1, 1);
            this.analysisFlowPanel.Name = "analysisFlowPanel";
            this.analysisFlowPanel.Padding = new System.Windows.Forms.Padding(2);
            this.uiTableLayoutPanel1.SetRowSpan(this.analysisFlowPanel, 2);
            this.analysisFlowPanel.ShowText = false;
            this.analysisFlowPanel.Size = new System.Drawing.Size(392, 50);
            this.analysisFlowPanel.TabIndex = 0;
            this.analysisFlowPanel.Text = "uiFlowLayoutPanel1";
            this.analysisFlowPanel.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiRichTextBox1
            // 
            this.uiTableLayoutPanel1.SetColumnSpan(this.uiRichTextBox1, 2);
            this.uiRichTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiRichTextBox1.FillColor = System.Drawing.Color.White;
            this.uiRichTextBox1.Font = new System.Drawing.Font("黑体", 10F);
            this.uiRichTextBox1.Location = new System.Drawing.Point(404, 47);
            this.uiRichTextBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiRichTextBox1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiRichTextBox1.Name = "uiRichTextBox1";
            this.uiRichTextBox1.Padding = new System.Windows.Forms.Padding(2);
            this.uiRichTextBox1.ReadOnly = true;
            this.uiRichTextBox1.ScrollBarStyleInherited = false;
            this.uiRichTextBox1.ShowText = false;
            this.uiRichTextBox1.Size = new System.Drawing.Size(1, 8);
            this.uiRichTextBox1.TabIndex = 3;
            this.uiRichTextBox1.Text = "uiRichTextBox1";
            this.uiRichTextBox1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAddLowBeamHighShutter,
            this.tsmiAddLowBeam,
            this.tsmiAddHighBeam});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(299, 70);
            // 
            // tsmiAddLowBeamHighShutter
            // 
            this.tsmiAddLowBeamHighShutter.Name = "tsmiAddLowBeamHighShutter";
            this.tsmiAddLowBeamHighShutter.Size = new System.Drawing.Size(298, 22);
            this.tsmiAddLowBeamHighShutter.Text = "将此图片设置为‘近光（用于识别截止线）’";
            this.tsmiAddLowBeamHighShutter.Click += new System.EventHandler(this.tsmiAddLowBeamHighShutter_Click);
            // 
            // tsmiAddLowBeam
            // 
            this.tsmiAddLowBeam.Name = "tsmiAddLowBeam";
            this.tsmiAddLowBeam.Size = new System.Drawing.Size(298, 22);
            this.tsmiAddLowBeam.Text = "将此图片设置为‘近光’";
            this.tsmiAddLowBeam.Click += new System.EventHandler(this.tsmiAddLowBeam_Click);
            // 
            // tsmiAddHighBeam
            // 
            this.tsmiAddHighBeam.Name = "tsmiAddHighBeam";
            this.tsmiAddHighBeam.Size = new System.Drawing.Size(298, 22);
            this.tsmiAddHighBeam.Text = "将此图片设置为‘远光’";
            this.tsmiAddHighBeam.Click += new System.EventHandler(this.tsmiAddHighBeam_Click);
            // 
            // PesVisionConfig
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1536, 824);
            this.Controls.Add(this.uiTabControl1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "PesVisionConfig";
            this.Text = "PES光型标定";
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 800, 450);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.uiTabControl1.ResumeLayout(false);
            this.tbPageCamera.ResumeLayout(false);
            this.mainImageTable.ResumeLayout(false);
            this.tbPageParaSet.ResumeLayout(false);
            this.uiTableLayoutPanel1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnStartGrab;
        private System.Windows.Forms.ToolStripButton btnStopGrab;
        private System.Windows.Forms.ToolStripButton btnSnapshot;
        private System.Windows.Forms.ToolStripButton btnLoadLocalLbImage;
        private System.Windows.Forms.ToolStripButton btnLoadLocalHbImage;
        private Sunny.UI.UITabControl uiTabControl1;
        private System.Windows.Forms.TabPage tbPageCamera;
        private Sunny.UI.UITableLayoutPanel mainImageTable;
        private Sunny.UI.UIGroupBox gpCamera;
        private Sunny.UI.UIGroupBox gpHighBeam;
        private Sunny.UI.UIGroupBox gpLowBeam;
        private Sunny.UI.UIGroupBox gpLowBeamWithHighShutter;
        private System.Windows.Forms.TabPage tbPageParaSet;
        private Sunny.UI.UITableLayoutPanel uiTableLayoutPanel1;
        private Sunny.UI.UIRichTextBox uiRichTextBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmiAddLowBeamHighShutter;
        private System.Windows.Forms.ToolStripMenuItem tsmiAddLowBeam;
        private System.Windows.Forms.ToolStripMenuItem tsmiAddHighBeam;
        private System.Windows.Forms.ToolStripButton btnSetShutter;
        private System.Windows.Forms.ToolStripButton btnLampControl;
        private System.Windows.Forms.ToolStripButton btnExecute;
        private Sunny.UI.UIGroupBox gpToShowDetail;
        private Sunny.UI.UIFlowLayoutPanel analysisFlowPanel;
    }
}