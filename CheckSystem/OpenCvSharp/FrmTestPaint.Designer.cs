namespace CheckSystem.OpenCvSharp
{
    partial class FrmTestPaint
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTestPaint));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonOpen = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonClear = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonDrawCircle = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDrawDot = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDrawLine = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDrawRect = new System.Windows.Forms.ToolStripButton();
            this.textBox_Action = new System.Windows.Forms.ToolStripButton();
            this.MainImageViewer = new NationalInstruments.Vision.WindowsForms.ImageViewer();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonOpen,
            this.toolStripButtonClear,
            this.toolStripSeparator1,
            this.toolStripButtonDrawCircle,
            this.toolStripButtonDrawDot,
            this.toolStripButtonDrawLine,
            this.toolStripButtonDrawRect,
            this.textBox_Action});
            this.toolStrip1.Location = new System.Drawing.Point(0, 35);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 40);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonOpen
            // 
            this.toolStripButtonOpen.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonOpen.Image")));
            this.toolStripButtonOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOpen.Name = "toolStripButtonOpen";
            this.toolStripButtonOpen.Size = new System.Drawing.Size(60, 37);
            this.toolStripButtonOpen.Text = "打开图片";
            this.toolStripButtonOpen.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonOpen.Click += new System.EventHandler(this.toolStripButtonOpen_Click);
            // 
            // toolStripButtonClear
            // 
            this.toolStripButtonClear.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonClear.Image")));
            this.toolStripButtonClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonClear.Name = "toolStripButtonClear";
            this.toolStripButtonClear.Size = new System.Drawing.Size(49, 37);
            this.toolStripButtonClear.Text = "清空UI";
            this.toolStripButtonClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 40);
            // 
            // toolStripButtonDrawCircle
            // 
            this.toolStripButtonDrawCircle.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDrawCircle.Image")));
            this.toolStripButtonDrawCircle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDrawCircle.Name = "toolStripButtonDrawCircle";
            this.toolStripButtonDrawCircle.Size = new System.Drawing.Size(36, 37);
            this.toolStripButtonDrawCircle.Text = "画圆";
            this.toolStripButtonDrawCircle.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonDrawCircle.Click += new System.EventHandler(this.toolStripButtonDrawCircle_Click);
            // 
            // toolStripButtonDrawDot
            // 
            this.toolStripButtonDrawDot.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDrawDot.Image")));
            this.toolStripButtonDrawDot.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDrawDot.Name = "toolStripButtonDrawDot";
            this.toolStripButtonDrawDot.Size = new System.Drawing.Size(36, 37);
            this.toolStripButtonDrawDot.Text = "画点";
            this.toolStripButtonDrawDot.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolStripButtonDrawLine
            // 
            this.toolStripButtonDrawLine.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDrawLine.Image")));
            this.toolStripButtonDrawLine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDrawLine.Name = "toolStripButtonDrawLine";
            this.toolStripButtonDrawLine.Size = new System.Drawing.Size(36, 37);
            this.toolStripButtonDrawLine.Text = "画线";
            this.toolStripButtonDrawLine.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolStripButtonDrawRect
            // 
            this.toolStripButtonDrawRect.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDrawRect.Image")));
            this.toolStripButtonDrawRect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDrawRect.Name = "toolStripButtonDrawRect";
            this.toolStripButtonDrawRect.Size = new System.Drawing.Size(48, 37);
            this.toolStripButtonDrawRect.Text = "画矩形";
            this.toolStripButtonDrawRect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // textBox_Action
            // 
            this.textBox_Action.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.textBox_Action.Image = ((System.Drawing.Image)(resources.GetObject("textBox_Action.Image")));
            this.textBox_Action.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.textBox_Action.Name = "textBox_Action";
            this.textBox_Action.Size = new System.Drawing.Size(107, 37);
            this.textBox_Action.Text = "toolStripButton1";
            // 
            // MainImageViewer
            // 
            this.MainImageViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MainImageViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainImageViewer.Location = new System.Drawing.Point(0, 75);
            this.MainImageViewer.Name = "MainImageViewer";
            this.MainImageViewer.Size = new System.Drawing.Size(800, 405);
            this.MainImageViewer.TabIndex = 12;
            // 
            // FrmTestPaint
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(800, 480);
            this.Controls.Add(this.MainImageViewer);
            this.Controls.Add(this.toolStrip1);
            this.Name = "FrmTestPaint";
            this.Text = "测试画图";
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 800, 480);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonOpen;
        private System.Windows.Forms.ToolStripButton toolStripButtonClear;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonDrawCircle;
        private System.Windows.Forms.ToolStripButton toolStripButtonDrawDot;
        private System.Windows.Forms.ToolStripButton toolStripButtonDrawLine;
        private System.Windows.Forms.ToolStripButton toolStripButtonDrawRect;
        private NationalInstruments.Vision.WindowsForms.ImageViewer MainImageViewer;
        private System.Windows.Forms.ToolStripButton textBox_Action;
    }
}