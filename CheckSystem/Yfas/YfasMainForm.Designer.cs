namespace CheckSystem.Yfas
{
    partial class YfasMainForm
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
            this.uiLabel2 = new Sunny.UI.UILabel();
            this.uiAvatar1 = new Sunny.UI.UIAvatar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.下拉菜单ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSelectProduct = new System.Windows.Forms.ToolStripMenuItem();
            this.btnDeviceInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCheckData = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbtnStart = new System.Windows.Forms.ToolStripMenuItem();
            this.uiLblTitle = new Sunny.UI.UILabel();
            this.Header.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Footer
            // 
            this.Footer.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Footer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.Footer.Location = new System.Drawing.Point(0, 512);
            this.Footer.Size = new System.Drawing.Size(802, 24);
            this.Footer.Style = Sunny.UI.UIStyle.Custom;
            this.Footer.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Header
            // 
            this.Header.Controls.Add(this.uiLabel2);
            this.Header.Controls.Add(this.uiAvatar1);
            this.Header.Controls.Add(this.uiLblTitle);
            this.Header.Controls.Add(this.menuStrip1);
            this.Header.Size = new System.Drawing.Size(802, 110);
            // 
            // uiLabel2
            // 
            this.uiLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiLabel2.AutoSize = true;
            this.uiLabel2.BackColor = System.Drawing.Color.White;
            this.uiLabel2.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiLabel2.Location = new System.Drawing.Point(630, 77);
            this.uiLabel2.Name = "uiLabel2";
            this.uiLabel2.Size = new System.Drawing.Size(122, 21);
            this.uiLabel2.TabIndex = 31;
            this.uiLabel2.Text = "当前登录用户：";
            this.uiLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiAvatar1
            // 
            this.uiAvatar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiAvatar1.AvatarSize = 55;
            this.uiAvatar1.BackColor = System.Drawing.Color.White;
            this.uiAvatar1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiAvatar1.Location = new System.Drawing.Point(703, 14);
            this.uiAvatar1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiAvatar1.Name = "uiAvatar1";
            this.uiAvatar1.Size = new System.Drawing.Size(60, 60);
            this.uiAvatar1.SymbolSize = 48;
            this.uiAvatar1.TabIndex = 30;
            this.uiAvatar1.Text = "uiAvatar1";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.下拉菜单ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(802, 25);
            this.menuStrip1.TabIndex = 32;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 下拉菜单ToolStripMenuItem
            // 
            this.下拉菜单ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSelectProduct,
            this.btnDeviceInfo,
            this.btnCheckData,
            this.tsbtnStart});
            this.下拉菜单ToolStripMenuItem.Name = "下拉菜单ToolStripMenuItem";
            this.下拉菜单ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.下拉菜单ToolStripMenuItem.Text = "下拉菜单";
            // 
            // btnSelectProduct
            // 
            this.btnSelectProduct.Name = "btnSelectProduct";
            this.btnSelectProduct.Size = new System.Drawing.Size(152, 22);
            this.btnSelectProduct.Text = "选择产品";
            // 
            // btnDeviceInfo
            // 
            this.btnDeviceInfo.Name = "btnDeviceInfo";
            this.btnDeviceInfo.Size = new System.Drawing.Size(152, 22);
            this.btnDeviceInfo.Text = "设备信息";
            this.btnDeviceInfo.Click += new System.EventHandler(this.产品清单ToolStripMenuItem_Click);
            // 
            // btnCheckData
            // 
            this.btnCheckData.Name = "btnCheckData";
            this.btnCheckData.Size = new System.Drawing.Size(152, 22);
            this.btnCheckData.Text = "检测数据";
            this.btnCheckData.Click += new System.EventHandler(this.检测数据ToolStripMenuItem_Click);
            // 
            // tsbtnStart
            // 
            this.tsbtnStart.Name = "tsbtnStart";
            this.tsbtnStart.Size = new System.Drawing.Size(152, 22);
            this.tsbtnStart.Text = "开始测试";
            this.tsbtnStart.Click += new System.EventHandler(this.tsbtnStart_Click);
            // 
            // uiLblTitle
            // 
            this.uiLblTitle.BackColor = System.Drawing.Color.White;
            this.uiLblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiLblTitle.Font = new System.Drawing.Font("黑体", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.uiLblTitle.Image = global::CheckSystem.Properties.Resources.信耀LOGO3;
            this.uiLblTitle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiLblTitle.Location = new System.Drawing.Point(0, 25);
            this.uiLblTitle.Name = "uiLblTitle";
            this.uiLblTitle.Size = new System.Drawing.Size(802, 85);
            this.uiLblTitle.Style = Sunny.UI.UIStyle.Custom;
            this.uiLblTitle.TabIndex = 7;
            this.uiLblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // YfasMainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(802, 536);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "YfasMainForm";
            this.Text = "YFAS";
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 802, 536);
            this.Header.ResumeLayout(false);
            this.Header.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UILabel uiLblTitle;
        private Sunny.UI.UILabel uiLabel2;
        private Sunny.UI.UIAvatar uiAvatar1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 下拉菜单ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem btnDeviceInfo;
        private System.Windows.Forms.ToolStripMenuItem btnSelectProduct;
        private System.Windows.Forms.ToolStripMenuItem tsbtnStart;
        private System.Windows.Forms.ToolStripMenuItem btnCheckData;

    }
}