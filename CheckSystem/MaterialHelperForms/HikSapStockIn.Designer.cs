namespace CheckSystem.MaterialHelperForms
{
    partial class HikSapStockIn
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
            this.mainPanel = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnStartSapStockIn = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ToStockIn = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.AllreadySotckIn = new System.Windows.Forms.RichTextBox();
            this.cmbMaterialNo = new UserControls.LabelCombox();
            this.cmbSupplyNo = new UserControls.LabelCombox();
            this.cmbStockInNo = new UserControls.LabelCombox();
            this.mainPanel.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.groupBox1);
            this.mainPanel.Controls.Add(this.groupBox2);
            this.mainPanel.Controls.Add(this.cmbMaterialNo);
            this.mainPanel.Controls.Add(this.btnRefresh);
            this.mainPanel.Controls.Add(this.btnStartSapStockIn);
            this.mainPanel.Controls.Add(this.cmbSupplyNo);
            this.mainPanel.Controls.Add(this.cmbStockInNo);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Margin = new System.Windows.Forms.Padding(1);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(734, 711);
            this.mainPanel.TabIndex = 0;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(164, 588);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(1);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(94, 77);
            this.btnRefresh.TabIndex = 11;
            this.btnRefresh.Text = "重新读取入库单号";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnStartSapStockIn
            // 
            this.btnStartSapStockIn.Location = new System.Drawing.Point(411, 588);
            this.btnStartSapStockIn.Name = "btnStartSapStockIn";
            this.btnStartSapStockIn.Size = new System.Drawing.Size(103, 77);
            this.btnStartSapStockIn.TabIndex = 9;
            this.btnStartSapStockIn.Text = "开始SAP入库";
            this.btnStartSapStockIn.UseVisualStyleBackColor = true;
            this.btnStartSapStockIn.Click += new System.EventHandler(this.btnStartSapStockIn_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "SAP入库操作";
            this.notifyIcon1.Visible = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ToStockIn);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 99);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(734, 216);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "待入库数据：";
            // 
            // ToStockIn
            // 
            this.ToStockIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ToStockIn.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ToStockIn.Location = new System.Drawing.Point(3, 17);
            this.ToStockIn.Name = "ToStockIn";
            this.ToStockIn.ReadOnly = true;
            this.ToStockIn.Size = new System.Drawing.Size(728, 196);
            this.ToStockIn.TabIndex = 16;
            this.ToStockIn.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.AllreadySotckIn);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 315);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(734, 238);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "已入库数据：";
            // 
            // AllreadySotckIn
            // 
            this.AllreadySotckIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AllreadySotckIn.Font = new System.Drawing.Font("宋体", 9.75F);
            this.AllreadySotckIn.Location = new System.Drawing.Point(3, 17);
            this.AllreadySotckIn.Name = "AllreadySotckIn";
            this.AllreadySotckIn.ReadOnly = true;
            this.AllreadySotckIn.Size = new System.Drawing.Size(728, 218);
            this.AllreadySotckIn.TabIndex = 17;
            this.AllreadySotckIn.Text = "";
            // 
            // cmbMaterialNo
            // 
            this.cmbMaterialNo.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbMaterialNo.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbMaterialNo.LabelString = "物料号";
            this.cmbMaterialNo.Location = new System.Drawing.Point(0, 66);
            this.cmbMaterialNo.Margin = new System.Windows.Forms.Padding(0);
            this.cmbMaterialNo.Name = "cmbMaterialNo";
            this.cmbMaterialNo.Size = new System.Drawing.Size(734, 33);
            this.cmbMaterialNo.TabIndex = 13;
            // 
            // cmbSupplyNo
            // 
            this.cmbSupplyNo.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbSupplyNo.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbSupplyNo.LabelString = "供应商编码";
            this.cmbSupplyNo.Location = new System.Drawing.Point(0, 33);
            this.cmbSupplyNo.Margin = new System.Windows.Forms.Padding(0);
            this.cmbSupplyNo.Name = "cmbSupplyNo";
            this.cmbSupplyNo.Size = new System.Drawing.Size(734, 33);
            this.cmbSupplyNo.TabIndex = 8;
            // 
            // cmbStockInNo
            // 
            this.cmbStockInNo.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbStockInNo.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbStockInNo.LabelString = "入库单号";
            this.cmbStockInNo.Location = new System.Drawing.Point(0, 0);
            this.cmbStockInNo.Margin = new System.Windows.Forms.Padding(0);
            this.cmbStockInNo.Name = "cmbStockInNo";
            this.cmbStockInNo.Size = new System.Drawing.Size(734, 33);
            this.cmbStockInNo.TabIndex = 7;
            // 
            // HikSapStockIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 711);
            this.Controls.Add(this.mainPanel);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(750, 750);
            this.MinimumSize = new System.Drawing.Size(750, 750);
            this.Name = "HikSapStockIn";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SAP入库操作";
            this.TopMost = true;
            this.mainPanel.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnStartSapStockIn;
        private UserControls.LabelCombox cmbSupplyNo;
        private UserControls.LabelCombox cmbStockInNo;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ToolTip toolTip1;
        private UserControls.LabelCombox cmbMaterialNo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox AllreadySotckIn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox ToStockIn;

    }
}