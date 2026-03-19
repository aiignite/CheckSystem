namespace CheckSystem.Yfas.Config.TestFlow
{
    partial class FormParasConfigPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormParasConfigPage));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbtnConfirm = new System.Windows.Forms.ToolStripButton();
            this.tsbtnCancel = new System.Windows.Forms.ToolStripButton();
            this.uiTransferParas = new Sunny.UI.UITransfer();
            this.toolStrip1.SuspendLayout();
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
            // uiTransferParas
            // 
            this.uiTransferParas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTransferParas.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiTransferParas.ItemsLeft.AddRange(new object[] {
            ""});
            this.uiTransferParas.ItemsRight.AddRange(new object[] {
            ""});
            this.uiTransferParas.Location = new System.Drawing.Point(0, 25);
            this.uiTransferParas.Margin = new System.Windows.Forms.Padding(7, 9, 7, 9);
            this.uiTransferParas.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiTransferParas.Name = "uiTransferParas";
            this.uiTransferParas.Padding = new System.Windows.Forms.Padding(1);
            this.uiTransferParas.RadiusSides = Sunny.UI.UICornerRadiusSides.None;
            this.uiTransferParas.RectSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.None;
            this.uiTransferParas.ShowText = false;
            this.uiTransferParas.Size = new System.Drawing.Size(802, 435);
            this.uiTransferParas.TabIndex = 22;
            this.uiTransferParas.Text = "uiTransfer1";
            this.uiTransferParas.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormParasConfigPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(802, 460);
            this.Controls.Add(this.uiTransferParas);
            this.Controls.Add(this.toolStrip1);
            this.Name = "FormParasConfigPage";
            this.Text = "FormParasConfigPage";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbtnConfirm;
        private System.Windows.Forms.ToolStripButton tsbtnCancel;
        private Sunny.UI.UITransfer uiTransferParas;
    }
}