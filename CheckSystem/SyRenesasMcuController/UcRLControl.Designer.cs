namespace CheckSystem.SyRenesasMcuController
{
    partial class UcRlControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.uiTableLayoutPanel1 = new Sunny.UI.UITableLayoutPanel();
            this.RL8 = new Sunny.UI.UISwitch();
            this.RL7 = new Sunny.UI.UISwitch();
            this.RL6 = new Sunny.UI.UISwitch();
            this.RL5 = new Sunny.UI.UISwitch();
            this.RL4 = new Sunny.UI.UISwitch();
            this.RL3 = new Sunny.UI.UISwitch();
            this.RL2 = new Sunny.UI.UISwitch();
            this.lblSlaveId = new Sunny.UI.UILabel();
            this.RL1 = new Sunny.UI.UISwitch();
            this.btnSlaveSetRLs = new Sunny.UI.UIButton();
            this.uiTableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiTableLayoutPanel1
            // 
            this.uiTableLayoutPanel1.ColumnCount = 10;
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.uiTableLayoutPanel1.Controls.Add(this.RL8, 8, 0);
            this.uiTableLayoutPanel1.Controls.Add(this.RL7, 7, 0);
            this.uiTableLayoutPanel1.Controls.Add(this.RL6, 6, 0);
            this.uiTableLayoutPanel1.Controls.Add(this.RL5, 5, 0);
            this.uiTableLayoutPanel1.Controls.Add(this.RL4, 4, 0);
            this.uiTableLayoutPanel1.Controls.Add(this.RL3, 3, 0);
            this.uiTableLayoutPanel1.Controls.Add(this.RL2, 2, 0);
            this.uiTableLayoutPanel1.Controls.Add(this.lblSlaveId, 0, 0);
            this.uiTableLayoutPanel1.Controls.Add(this.RL1, 1, 0);
            this.uiTableLayoutPanel1.Controls.Add(this.btnSlaveSetRLs, 9, 0);
            this.uiTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.uiTableLayoutPanel1.Name = "uiTableLayoutPanel1";
            this.uiTableLayoutPanel1.RowCount = 1;
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.uiTableLayoutPanel1.Size = new System.Drawing.Size(763, 34);
            this.uiTableLayoutPanel1.TabIndex = 0;
            this.uiTableLayoutPanel1.TagString = null;
            // 
            // RL8
            // 
            this.RL8.ActiveText = "RL8开";
            this.RL8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RL8.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.RL8.InActiveText = "RL8关";
            this.RL8.Location = new System.Drawing.Point(608, 0);
            this.RL8.Margin = new System.Windows.Forms.Padding(0);
            this.RL8.MinimumSize = new System.Drawing.Size(1, 1);
            this.RL8.Name = "RL8";
            this.RL8.Size = new System.Drawing.Size(76, 34);
            this.RL8.TabIndex = 8;
            this.RL8.Tag = "8";
            this.RL8.Text = "uiSwitch8";
            this.RL8.ActiveChanged += new System.EventHandler(this.RL_ActiveChanged);
            // 
            // RL7
            // 
            this.RL7.ActiveText = "RL7开";
            this.RL7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RL7.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.RL7.InActiveText = "RL7关";
            this.RL7.Location = new System.Drawing.Point(532, 0);
            this.RL7.Margin = new System.Windows.Forms.Padding(0);
            this.RL7.MinimumSize = new System.Drawing.Size(1, 1);
            this.RL7.Name = "RL7";
            this.RL7.Size = new System.Drawing.Size(76, 34);
            this.RL7.TabIndex = 7;
            this.RL7.Tag = "7";
            this.RL7.Text = "uiSwitch7";
            this.RL7.ActiveChanged += new System.EventHandler(this.RL_ActiveChanged);
            // 
            // RL6
            // 
            this.RL6.ActiveText = "RL6开";
            this.RL6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RL6.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.RL6.InActiveText = "RL6关";
            this.RL6.Location = new System.Drawing.Point(456, 0);
            this.RL6.Margin = new System.Windows.Forms.Padding(0);
            this.RL6.MinimumSize = new System.Drawing.Size(1, 1);
            this.RL6.Name = "RL6";
            this.RL6.Size = new System.Drawing.Size(76, 34);
            this.RL6.TabIndex = 6;
            this.RL6.Tag = "6";
            this.RL6.Text = "uiSwitch6";
            this.RL6.ActiveChanged += new System.EventHandler(this.RL_ActiveChanged);
            // 
            // RL5
            // 
            this.RL5.ActiveText = "RL5开";
            this.RL5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RL5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.RL5.InActiveText = "RL5关";
            this.RL5.Location = new System.Drawing.Point(380, 0);
            this.RL5.Margin = new System.Windows.Forms.Padding(0);
            this.RL5.MinimumSize = new System.Drawing.Size(1, 1);
            this.RL5.Name = "RL5";
            this.RL5.Size = new System.Drawing.Size(76, 34);
            this.RL5.TabIndex = 5;
            this.RL5.Tag = "5";
            this.RL5.Text = "uiSwitch5";
            this.RL5.ActiveChanged += new System.EventHandler(this.RL_ActiveChanged);
            // 
            // RL4
            // 
            this.RL4.ActiveText = "RL4开";
            this.RL4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RL4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.RL4.InActiveText = "RL4关";
            this.RL4.Location = new System.Drawing.Point(304, 0);
            this.RL4.Margin = new System.Windows.Forms.Padding(0);
            this.RL4.MinimumSize = new System.Drawing.Size(1, 1);
            this.RL4.Name = "RL4";
            this.RL4.Size = new System.Drawing.Size(76, 34);
            this.RL4.TabIndex = 4;
            this.RL4.Tag = "4";
            this.RL4.Text = "uiSwitch4";
            this.RL4.ActiveChanged += new System.EventHandler(this.RL_ActiveChanged);
            // 
            // RL3
            // 
            this.RL3.ActiveText = "RL3开";
            this.RL3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RL3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.RL3.InActiveText = "RL3关";
            this.RL3.Location = new System.Drawing.Point(228, 0);
            this.RL3.Margin = new System.Windows.Forms.Padding(0);
            this.RL3.MinimumSize = new System.Drawing.Size(1, 1);
            this.RL3.Name = "RL3";
            this.RL3.Size = new System.Drawing.Size(76, 34);
            this.RL3.TabIndex = 3;
            this.RL3.Tag = "3";
            this.RL3.Text = "uiSwitch3";
            this.RL3.ActiveChanged += new System.EventHandler(this.RL_ActiveChanged);
            // 
            // RL2
            // 
            this.RL2.ActiveText = "RL2开";
            this.RL2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RL2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.RL2.InActiveText = "RL2关";
            this.RL2.Location = new System.Drawing.Point(152, 0);
            this.RL2.Margin = new System.Windows.Forms.Padding(0);
            this.RL2.MinimumSize = new System.Drawing.Size(1, 1);
            this.RL2.Name = "RL2";
            this.RL2.Size = new System.Drawing.Size(76, 34);
            this.RL2.TabIndex = 2;
            this.RL2.Tag = "2";
            this.RL2.Text = "uiSwitch2";
            this.RL2.ActiveChanged += new System.EventHandler(this.RL_ActiveChanged);
            // 
            // lblSlaveId
            // 
            this.lblSlaveId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSlaveId.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSlaveId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.lblSlaveId.Location = new System.Drawing.Point(0, 0);
            this.lblSlaveId.Margin = new System.Windows.Forms.Padding(0);
            this.lblSlaveId.Name = "lblSlaveId";
            this.lblSlaveId.Size = new System.Drawing.Size(76, 34);
            this.lblSlaveId.TabIndex = 0;
            this.lblSlaveId.Text = "0x101:";
            this.lblSlaveId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RL1
            // 
            this.RL1.ActiveText = "RL1开";
            this.RL1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RL1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.RL1.InActiveText = "RL1关";
            this.RL1.Location = new System.Drawing.Point(76, 0);
            this.RL1.Margin = new System.Windows.Forms.Padding(0);
            this.RL1.MinimumSize = new System.Drawing.Size(1, 1);
            this.RL1.Name = "RL1";
            this.RL1.Size = new System.Drawing.Size(76, 34);
            this.RL1.TabIndex = 1;
            this.RL1.Tag = "1";
            this.RL1.Text = "uiSwitch1";
            this.RL1.ActiveChanged += new System.EventHandler(this.RL_ActiveChanged);
            // 
            // btnSlaveSetRLs
            // 
            this.btnSlaveSetRLs.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSlaveSetRLs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSlaveSetRLs.Font = new System.Drawing.Font("宋体", 8F);
            this.btnSlaveSetRLs.Location = new System.Drawing.Point(684, 0);
            this.btnSlaveSetRLs.Margin = new System.Windows.Forms.Padding(0);
            this.btnSlaveSetRLs.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnSlaveSetRLs.Name = "btnSlaveSetRLs";
            this.btnSlaveSetRLs.Size = new System.Drawing.Size(79, 34);
            this.btnSlaveSetRLs.TabIndex = 9;
            this.btnSlaveSetRLs.Text = "设置当前从站";
            this.btnSlaveSetRLs.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSlaveSetRLs.Click += new System.EventHandler(this.btnSlaveSetRLs_Click);
            // 
            // UcRLControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.uiTableLayoutPanel1);
            this.Name = "UcRlControl";
            this.Size = new System.Drawing.Size(763, 34);
            this.uiTableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UITableLayoutPanel uiTableLayoutPanel1;
        private Sunny.UI.UISwitch RL8;
        private Sunny.UI.UISwitch RL7;
        private Sunny.UI.UISwitch RL6;
        private Sunny.UI.UISwitch RL5;
        private Sunny.UI.UISwitch RL4;
        private Sunny.UI.UISwitch RL3;
        private Sunny.UI.UISwitch RL2;
        private Sunny.UI.UILabel lblSlaveId;
        private Sunny.UI.UISwitch RL1;
        private Sunny.UI.UIButton btnSlaveSetRLs;
    }
}
