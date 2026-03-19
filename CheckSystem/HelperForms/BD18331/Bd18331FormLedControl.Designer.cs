using HZH_Controls.Controls.Tab;

namespace CheckSystem.HelperForms.BD18331
{
    partial class Bd18331FormLedControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Bd18331FormLedControl));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.mainTab = new HZH_Controls.Controls.Tab.TabControlExt();
            this.btnPwmMode = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnDcMode = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.lblMode = new System.Windows.Forms.ToolStripLabel();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPwmMode,
            this.toolStripSeparator1,
            this.btnDcMode,
            this.toolStripSeparator2,
            this.lblMode});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1008, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // mainTab
            // 
            this.mainTab.CloseBtnColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(85)))), ((int)(((byte)(51)))));
            this.mainTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTab.IsShowCloseBtn = false;
            this.mainTab.ItemSize = new System.Drawing.Size(0, 50);
            this.mainTab.Location = new System.Drawing.Point(0, 25);
            this.mainTab.Name = "mainTab";
            this.mainTab.SelectedIndex = 0;
            this.mainTab.Size = new System.Drawing.Size(1008, 704);
            this.mainTab.TabIndex = 1;
            this.mainTab.UncloseTabIndexs = null;
            // 
            // btnPwmMode
            // 
            this.btnPwmMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnPwmMode.Enabled = false;
            this.btnPwmMode.Image = ((System.Drawing.Image)(resources.GetObject("btnPwmMode.Image")));
            this.btnPwmMode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPwmMode.Name = "btnPwmMode";
            this.btnPwmMode.Size = new System.Drawing.Size(67, 22);
            this.btnPwmMode.Text = "PWM模式";
            this.btnPwmMode.Click += new System.EventHandler(this.btnPwmMode_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnDcMode
            // 
            this.btnDcMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnDcMode.Image = ((System.Drawing.Image)(resources.GetObject("btnDcMode.Image")));
            this.btnDcMode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDcMode.Name = "btnDcMode";
            this.btnDcMode.Size = new System.Drawing.Size(53, 22);
            this.btnDcMode.Text = "DC模式";
            this.btnDcMode.Click += new System.EventHandler(this.btnDcMode_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // lblMode
            // 
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(123, 22);
            this.lblMode.Text = "当前模式：PWM模式";
            // 
            // Bd18331FormLedControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.mainTab);
            this.Controls.Add(this.toolStrip1);
            this.MaximizeBox = false;
            this.Name = "Bd18331FormLedControl";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bd18331FormLedControl";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }





        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnPwmMode;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnDcMode;
        private TabControlExt mainTab;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel lblMode;
    }
}