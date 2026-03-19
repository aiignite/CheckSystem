namespace CheckSystem.HelperForms.TPS92994
{
    partial class FrmTps92664Config
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gpHighBeam = new Sunny.UI.UIGroupBox();
            this.dgvHighBeam = new System.Windows.Forms.DataGridView();
            this.btnSaveConfig = new Sunny.UI.UIButton();
            this.btnClose = new Sunny.UI.UIButton();
            this.uiMarkLabel1 = new Sunny.UI.UIMarkLabel();
            this.numKeepOnTs1 = new System.Windows.Forms.NumericUpDown();
            this.uiMarkLabel2 = new Sunny.UI.UIMarkLabel();
            this.numKeepOffTs1 = new System.Windows.Forms.NumericUpDown();
            this.uiMarkLabel3 = new Sunny.UI.UIMarkLabel();
            this.numKeepOnTs2 = new System.Windows.Forms.NumericUpDown();
            this.gpHighBeam.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHighBeam)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numKeepOnTs1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numKeepOffTs1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numKeepOnTs2)).BeginInit();
            this.SuspendLayout();
            // 
            // gpHighBeam
            // 
            this.gpHighBeam.Controls.Add(this.dgvHighBeam);
            this.gpHighBeam.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Bold);
            this.gpHighBeam.Location = new System.Drawing.Point(4, 40);
            this.gpHighBeam.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gpHighBeam.MinimumSize = new System.Drawing.Size(1, 1);
            this.gpHighBeam.Name = "gpHighBeam";
            this.gpHighBeam.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.gpHighBeam.Size = new System.Drawing.Size(692, 374);
            this.gpHighBeam.TabIndex = 0;
            this.gpHighBeam.Text = "远光控制 (High Beam)";
            this.gpHighBeam.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvHighBeam
            // 
            this.dgvHighBeam.AllowUserToAddRows = false;
            this.dgvHighBeam.AllowUserToDeleteRows = false;
            this.dgvHighBeam.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvHighBeam.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvHighBeam.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHighBeam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvHighBeam.Location = new System.Drawing.Point(0, 32);
            this.dgvHighBeam.Name = "dgvHighBeam";
            this.dgvHighBeam.RowTemplate.Height = 23;
            this.dgvHighBeam.Size = new System.Drawing.Size(692, 342);
            this.dgvHighBeam.TabIndex = 0;
            // 
            // btnSaveConfig
            // 
            this.btnSaveConfig.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSaveConfig.Font = new System.Drawing.Font("黑体", 10F, System.Drawing.FontStyle.Bold);
            this.btnSaveConfig.Location = new System.Drawing.Point(22, 422);
            this.btnSaveConfig.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Size = new System.Drawing.Size(120, 35);
            this.btnSaveConfig.TabIndex = 2;
            this.btnSaveConfig.Text = "保存配置";
            this.btnSaveConfig.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 
            // btnClose
            // 
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnClose.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnClose.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(115)))), ((int)(((byte)(115)))));
            this.btnClose.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnClose.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnClose.Font = new System.Drawing.Font("黑体", 10F, System.Drawing.FontStyle.Bold);
            this.btnClose.LightColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.btnClose.Location = new System.Drawing.Point(22, 458);
            this.btnClose.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnClose.Name = "btnClose";
            this.btnClose.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnClose.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(115)))), ((int)(((byte)(115)))));
            this.btnClose.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnClose.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnClose.Size = new System.Drawing.Size(120, 35);
            this.btnClose.Style = Sunny.UI.UIStyle.Custom;
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "关闭";
            this.btnClose.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 
            // uiMarkLabel1
            // 
            this.uiMarkLabel1.Font = new System.Drawing.Font("宋体", 8F);
            this.uiMarkLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel1.Location = new System.Drawing.Point(148, 434);
            this.uiMarkLabel1.Name = "uiMarkLabel1";
            this.uiMarkLabel1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel1.Size = new System.Drawing.Size(121, 23);
            this.uiMarkLabel1.TabIndex = 5;
            this.uiMarkLabel1.Text = "第一次点亮保持到(n)ms";
            this.uiMarkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numKeepOnTs1
            // 
            this.numKeepOnTs1.Location = new System.Drawing.Point(254, 431);
            this.numKeepOnTs1.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numKeepOnTs1.Name = "numKeepOnTs1";
            this.numKeepOnTs1.Size = new System.Drawing.Size(120, 26);
            this.numKeepOnTs1.TabIndex = 6;
            // 
            // uiMarkLabel2
            // 
            this.uiMarkLabel2.Font = new System.Drawing.Font("宋体", 8F);
            this.uiMarkLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel2.Location = new System.Drawing.Point(148, 466);
            this.uiMarkLabel2.Name = "uiMarkLabel2";
            this.uiMarkLabel2.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel2.Size = new System.Drawing.Size(121, 23);
            this.uiMarkLabel2.TabIndex = 5;
            this.uiMarkLabel2.Text = "第一次熄灭保持到(n)ms";
            this.uiMarkLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numKeepOffTs1
            // 
            this.numKeepOffTs1.Location = new System.Drawing.Point(254, 463);
            this.numKeepOffTs1.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numKeepOffTs1.Name = "numKeepOffTs1";
            this.numKeepOffTs1.Size = new System.Drawing.Size(120, 26);
            this.numKeepOffTs1.TabIndex = 6;
            // 
            // uiMarkLabel3
            // 
            this.uiMarkLabel3.Font = new System.Drawing.Font("宋体", 8F);
            this.uiMarkLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel3.Location = new System.Drawing.Point(380, 434);
            this.uiMarkLabel3.Name = "uiMarkLabel3";
            this.uiMarkLabel3.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel3.Size = new System.Drawing.Size(121, 23);
            this.uiMarkLabel3.TabIndex = 5;
            this.uiMarkLabel3.Text = "第二次点亮保持到(n)ms";
            this.uiMarkLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numKeepOnTs2
            // 
            this.numKeepOnTs2.Location = new System.Drawing.Point(500, 431);
            this.numKeepOnTs2.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numKeepOnTs2.Name = "numKeepOnTs2";
            this.numKeepOnTs2.Size = new System.Drawing.Size(120, 26);
            this.numKeepOnTs2.TabIndex = 6;
            // 
            // FrmTps92664Config
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.MintCream;
            this.ClientSize = new System.Drawing.Size(700, 500);
            this.Controls.Add(this.numKeepOffTs1);
            this.Controls.Add(this.numKeepOnTs2);
            this.Controls.Add(this.numKeepOnTs1);
            this.Controls.Add(this.uiMarkLabel2);
            this.Controls.Add(this.uiMarkLabel3);
            this.Controls.Add(this.uiMarkLabel1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSaveConfig);
            this.Controls.Add(this.gpHighBeam);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(700, 500);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(700, 500);
            this.Name = "FrmTps92664Config";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TPS92664 配置界面";
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 800, 450);
            this.gpHighBeam.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHighBeam)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numKeepOnTs1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numKeepOffTs1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numKeepOnTs2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UIGroupBox gpHighBeam;
        private System.Windows.Forms.DataGridView dgvHighBeam;
        private Sunny.UI.UIButton btnSaveConfig;
        private Sunny.UI.UIButton btnClose;
        private Sunny.UI.UIMarkLabel uiMarkLabel1;
        private System.Windows.Forms.NumericUpDown numKeepOnTs1;
        private Sunny.UI.UIMarkLabel uiMarkLabel2;
        private System.Windows.Forms.NumericUpDown numKeepOffTs1;
        private Sunny.UI.UIMarkLabel uiMarkLabel3;
        private System.Windows.Forms.NumericUpDown numKeepOnTs2;
    }
}

