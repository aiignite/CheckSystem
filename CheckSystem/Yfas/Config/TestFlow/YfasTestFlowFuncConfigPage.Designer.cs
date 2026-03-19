namespace CheckSystem.Yfas.Config.TestFlow
{
    partial class FormFuncConfigPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFuncConfigPage));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbtnConfirm = new System.Windows.Forms.ToolStripButton();
            this.tsbtnCancel = new System.Windows.Forms.ToolStripButton();
            this.uiTextBox2 = new Sunny.UI.UITextBox();
            this.uiTitlePanel1 = new Sunny.UI.UITitlePanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.toolStrip1.SuspendLayout();
            this.uiTitlePanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnConfirm,
            this.tsbtnCancel});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(802, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbtnConfirm
            // 
            this.tsbtnConfirm.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnConfirm.Image")));
            this.tsbtnConfirm.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnConfirm.Name = "tsbtnConfirm";
            this.tsbtnConfirm.Size = new System.Drawing.Size(52, 22);
            this.tsbtnConfirm.Text = "确认";
            this.tsbtnConfirm.Click += new System.EventHandler(this.tsbtnConfirm_Click);
            // 
            // tsbtnCancel
            // 
            this.tsbtnCancel.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnCancel.Image")));
            this.tsbtnCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnCancel.Name = "tsbtnCancel";
            this.tsbtnCancel.Size = new System.Drawing.Size(52, 22);
            this.tsbtnCancel.Text = "取消";
            this.tsbtnCancel.Click += new System.EventHandler(this.tsbtnCancel_Click);
            // 
            // uiTextBox2
            // 
            this.uiTextBox2.ButtonWidth = 100;
            this.uiTextBox2.CanEmpty = true;
            this.uiTextBox2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.uiTextBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.uiTextBox2.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiTextBox2.Location = new System.Drawing.Point(0, 25);
            this.uiTextBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiTextBox2.Maximum = 999999D;
            this.uiTextBox2.Minimum = -1D;
            this.uiTextBox2.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiTextBox2.Name = "uiTextBox2";
            this.uiTextBox2.Padding = new System.Windows.Forms.Padding(5);
            this.uiTextBox2.ShowText = false;
            this.uiTextBox2.Size = new System.Drawing.Size(802, 29);
            this.uiTextBox2.TabIndex = 4;
            this.uiTextBox2.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiTextBox2.Type = Sunny.UI.UITextBox.UIEditType.Integer;
            this.uiTextBox2.Watermark = "请输入超时时间，-1为无超时时间";
            // 
            // uiTitlePanel1
            // 
            this.uiTitlePanel1.Controls.Add(this.flowLayoutPanel1);
            this.uiTitlePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTitlePanel1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiTitlePanel1.Location = new System.Drawing.Point(0, 54);
            this.uiTitlePanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiTitlePanel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiTitlePanel1.Name = "uiTitlePanel1";
            this.uiTitlePanel1.Padding = new System.Windows.Forms.Padding(0, 35, 0, 0);
            this.uiTitlePanel1.ShowText = false;
            this.uiTitlePanel1.Size = new System.Drawing.Size(802, 406);
            this.uiTitlePanel1.TabIndex = 5;
            this.uiTitlePanel1.Text = "请选择：";
            this.uiTitlePanel1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.uiTitlePanel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 35);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(802, 371);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // FormFuncConfigPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(802, 460);
            this.Controls.Add(this.uiTitlePanel1);
            this.Controls.Add(this.uiTextBox2);
            this.Controls.Add(this.toolStrip1);
            this.Name = "FormFuncConfigPage";
            this.Text = "FormWaitConfigPage";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.uiTitlePanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbtnConfirm;
        private System.Windows.Forms.ToolStripButton tsbtnCancel;
        private Sunny.UI.UITextBox uiTextBox2;
        private Sunny.UI.UITitlePanel uiTitlePanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}