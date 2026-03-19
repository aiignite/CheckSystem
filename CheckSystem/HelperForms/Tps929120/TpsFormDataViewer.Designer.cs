namespace CheckSystem.HelperForms.Tps929120
{
    partial class TpsFormDataViewer
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
            this.mainPanel = new System.Windows.Forms.TableLayoutPanel();
            this.dataTablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.titleTablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.onOffLineTablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.cmbAddrList = new UserControls.LabelCombox();
            this.btnOnOffLine = new System.Windows.Forms.Button();
            this.btnFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.buttomTablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.btnSaveParas = new HZH_Controls.Controls.Btn.UCBtnExt();
            this.btnWriteThisTps = new HZH_Controls.Controls.Btn.UCBtnExt();
            this.uiTableLayoutPanel1 = new Sunny.UI.UITableLayoutPanel();
            this.dgvIoutPwm = new UserControls.UserDataGrid();
            this.grpEepm = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.dgvChConfig = new UserControls.UserDataGrid();
            this.dgvOtherConfig = new UserControls.UserDataGrid();
            this.uiRichTextBox1 = new Sunny.UI.UIRichTextBox();
            this.mainPanel.SuspendLayout();
            this.dataTablePanel.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.titleTablePanel.SuspendLayout();
            this.onOffLineTablePanel.SuspendLayout();
            this.buttomTablePanel.SuspendLayout();
            this.uiTableLayoutPanel1.SuspendLayout();
            this.grpEepm.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.ColumnCount = 1;
            this.mainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainPanel.Controls.Add(this.dataTablePanel, 0, 1);
            this.mainPanel.Controls.Add(this.titleTablePanel, 0, 0);
            this.mainPanel.Controls.Add(this.buttomTablePanel, 0, 2);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.RowCount = 3;
            this.mainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.mainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.mainPanel.Size = new System.Drawing.Size(884, 731);
            this.mainPanel.TabIndex = 0;
            // 
            // dataTablePanel
            // 
            this.dataTablePanel.ColumnCount = 2;
            this.dataTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.dataTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.dataTablePanel.Controls.Add(this.tabControl1, 0, 0);
            this.dataTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataTablePanel.Location = new System.Drawing.Point(3, 103);
            this.dataTablePanel.Name = "dataTablePanel";
            this.dataTablePanel.RowCount = 1;
            this.dataTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.dataTablePanel.Size = new System.Drawing.Size(878, 525);
            this.dataTablePanel.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.dataTablePanel.SetColumnSpan(this.tabControl1, 2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(872, 519);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.uiTableLayoutPanel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(864, 493);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "显示";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.uiRichTextBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(864, 493);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "LOG";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // titleTablePanel
            // 
            this.titleTablePanel.ColumnCount = 2;
            this.titleTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.titleTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.titleTablePanel.Controls.Add(this.onOffLineTablePanel, 0, 0);
            this.titleTablePanel.Controls.Add(this.btnFlowPanel, 1, 0);
            this.titleTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleTablePanel.Location = new System.Drawing.Point(3, 3);
            this.titleTablePanel.Name = "titleTablePanel";
            this.titleTablePanel.RowCount = 1;
            this.titleTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.titleTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 94F));
            this.titleTablePanel.Size = new System.Drawing.Size(878, 94);
            this.titleTablePanel.TabIndex = 1;
            // 
            // onOffLineTablePanel
            // 
            this.onOffLineTablePanel.ColumnCount = 1;
            this.onOffLineTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.onOffLineTablePanel.Controls.Add(this.cmbAddrList, 0, 0);
            this.onOffLineTablePanel.Controls.Add(this.btnOnOffLine, 0, 1);
            this.onOffLineTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.onOffLineTablePanel.Location = new System.Drawing.Point(3, 3);
            this.onOffLineTablePanel.Name = "onOffLineTablePanel";
            this.onOffLineTablePanel.RowCount = 2;
            this.onOffLineTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.onOffLineTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.onOffLineTablePanel.Size = new System.Drawing.Size(433, 88);
            this.onOffLineTablePanel.TabIndex = 0;
            // 
            // cmbAddrList
            // 
            this.cmbAddrList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbAddrList.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbAddrList.LabelString = "地址列表：";
            this.cmbAddrList.Location = new System.Drawing.Point(0, 0);
            this.cmbAddrList.Margin = new System.Windows.Forms.Padding(0);
            this.cmbAddrList.Name = "cmbAddrList";
            this.cmbAddrList.Size = new System.Drawing.Size(433, 35);
            this.cmbAddrList.TabIndex = 0;
            // 
            // btnOnOffLine
            // 
            this.btnOnOffLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOnOffLine.Enabled = false;
            this.btnOnOffLine.Font = new System.Drawing.Font("黑体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOnOffLine.ForeColor = System.Drawing.Color.White;
            this.btnOnOffLine.Location = new System.Drawing.Point(0, 35);
            this.btnOnOffLine.Margin = new System.Windows.Forms.Padding(0);
            this.btnOnOffLine.Name = "btnOnOffLine";
            this.btnOnOffLine.Size = new System.Drawing.Size(433, 53);
            this.btnOnOffLine.TabIndex = 1;
            this.btnOnOffLine.Text = "button1";
            this.btnOnOffLine.UseVisualStyleBackColor = true;
            // 
            // btnFlowPanel
            // 
            this.btnFlowPanel.AutoScroll = true;
            this.btnFlowPanel.BackColor = System.Drawing.Color.BurlyWood;
            this.btnFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFlowPanel.Location = new System.Drawing.Point(442, 3);
            this.btnFlowPanel.Name = "btnFlowPanel";
            this.btnFlowPanel.Size = new System.Drawing.Size(433, 88);
            this.btnFlowPanel.TabIndex = 1;
            // 
            // buttomTablePanel
            // 
            this.buttomTablePanel.ColumnCount = 2;
            this.buttomTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.buttomTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.buttomTablePanel.Controls.Add(this.btnSaveParas, 0, 0);
            this.buttomTablePanel.Controls.Add(this.btnWriteThisTps, 1, 0);
            this.buttomTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttomTablePanel.Location = new System.Drawing.Point(0, 631);
            this.buttomTablePanel.Margin = new System.Windows.Forms.Padding(0);
            this.buttomTablePanel.Name = "buttomTablePanel";
            this.buttomTablePanel.RowCount = 1;
            this.buttomTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.buttomTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.buttomTablePanel.Size = new System.Drawing.Size(884, 100);
            this.buttomTablePanel.TabIndex = 2;
            // 
            // btnSaveParas
            // 
            this.btnSaveParas.BackColor = System.Drawing.Color.White;
            this.btnSaveParas.BtnBackColor = System.Drawing.Color.White;
            this.btnSaveParas.BtnFont = new System.Drawing.Font("黑体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSaveParas.BtnForeColor = System.Drawing.Color.White;
            this.btnSaveParas.BtnText = "保存当前参数";
            this.btnSaveParas.ConerRadius = 5;
            this.btnSaveParas.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSaveParas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaveParas.EnabledMouseEffect = false;
            this.btnSaveParas.FillColor = System.Drawing.Color.DarkCyan;
            this.btnSaveParas.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnSaveParas.IsRadius = true;
            this.btnSaveParas.IsShowRect = true;
            this.btnSaveParas.IsShowTips = false;
            this.btnSaveParas.Location = new System.Drawing.Point(1, 1);
            this.btnSaveParas.Margin = new System.Windows.Forms.Padding(1);
            this.btnSaveParas.Name = "btnSaveParas";
            this.btnSaveParas.RectColor = System.Drawing.Color.DarkOliveGreen;
            this.btnSaveParas.RectWidth = 1;
            this.btnSaveParas.Size = new System.Drawing.Size(440, 98);
            this.btnSaveParas.TabIndex = 0;
            this.btnSaveParas.TabStop = false;
            this.btnSaveParas.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.btnSaveParas.TipsText = "";
            this.btnSaveParas.BtnClick += new System.EventHandler(this.btnSaveParas_BtnClick);
            // 
            // btnWriteThisTps
            // 
            this.btnWriteThisTps.BackColor = System.Drawing.Color.White;
            this.btnWriteThisTps.BtnBackColor = System.Drawing.Color.White;
            this.btnWriteThisTps.BtnFont = new System.Drawing.Font("黑体", 15F, System.Drawing.FontStyle.Bold);
            this.btnWriteThisTps.BtnForeColor = System.Drawing.Color.White;
            this.btnWriteThisTps.BtnText = "更新当前芯片EPPROM";
            this.btnWriteThisTps.ConerRadius = 5;
            this.btnWriteThisTps.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnWriteThisTps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnWriteThisTps.EnabledMouseEffect = false;
            this.btnWriteThisTps.FillColor = System.Drawing.Color.DarkCyan;
            this.btnWriteThisTps.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnWriteThisTps.IsRadius = true;
            this.btnWriteThisTps.IsShowRect = true;
            this.btnWriteThisTps.IsShowTips = false;
            this.btnWriteThisTps.Location = new System.Drawing.Point(443, 1);
            this.btnWriteThisTps.Margin = new System.Windows.Forms.Padding(1);
            this.btnWriteThisTps.Name = "btnWriteThisTps";
            this.btnWriteThisTps.RectColor = System.Drawing.Color.DarkOliveGreen;
            this.btnWriteThisTps.RectWidth = 1;
            this.btnWriteThisTps.Size = new System.Drawing.Size(440, 98);
            this.btnWriteThisTps.TabIndex = 1;
            this.btnWriteThisTps.TabStop = false;
            this.btnWriteThisTps.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.btnWriteThisTps.TipsText = "";
            this.btnWriteThisTps.BtnClick += new System.EventHandler(this.btnWriteThisTps_BtnClick);
            // 
            // uiTableLayoutPanel1
            // 
            this.uiTableLayoutPanel1.ColumnCount = 2;
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.uiTableLayoutPanel1.Controls.Add(this.dgvIoutPwm, 0, 0);
            this.uiTableLayoutPanel1.Controls.Add(this.grpEepm, 1, 0);
            this.uiTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.uiTableLayoutPanel1.Name = "uiTableLayoutPanel1";
            this.uiTableLayoutPanel1.RowCount = 1;
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.uiTableLayoutPanel1.Size = new System.Drawing.Size(858, 487);
            this.uiTableLayoutPanel1.TabIndex = 0;
            this.uiTableLayoutPanel1.TagString = null;
            // 
            // dgvIoutPwm
            // 
            this.dgvIoutPwm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvIoutPwm.Location = new System.Drawing.Point(3, 3);
            this.dgvIoutPwm.Name = "dgvIoutPwm";
            this.dgvIoutPwm.Size = new System.Drawing.Size(423, 481);
            this.dgvIoutPwm.TabIndex = 2;
            // 
            // grpEepm
            // 
            this.grpEepm.Controls.Add(this.tableLayoutPanel2);
            this.grpEepm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpEepm.Font = new System.Drawing.Font("微软雅黑", 15F);
            this.grpEepm.Location = new System.Drawing.Point(432, 3);
            this.grpEepm.Name = "grpEepm";
            this.grpEepm.Size = new System.Drawing.Size(423, 481);
            this.grpEepm.TabIndex = 3;
            this.grpEepm.TabStop = false;
            this.grpEepm.Text = "EEPM Paras";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.dgvChConfig, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.dgvOtherConfig, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 30);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(417, 448);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // dgvChConfig
            // 
            this.dgvChConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvChConfig.Location = new System.Drawing.Point(6, 7);
            this.dgvChConfig.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.dgvChConfig.Name = "dgvChConfig";
            this.dgvChConfig.Size = new System.Drawing.Size(405, 254);
            this.dgvChConfig.TabIndex = 0;
            // 
            // dgvOtherConfig
            // 
            this.dgvOtherConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOtherConfig.Location = new System.Drawing.Point(6, 275);
            this.dgvOtherConfig.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.dgvOtherConfig.Name = "dgvOtherConfig";
            this.dgvOtherConfig.Size = new System.Drawing.Size(405, 166);
            this.dgvOtherConfig.TabIndex = 1;
            // 
            // uiRichTextBox1
            // 
            this.uiRichTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiRichTextBox1.FillColor = System.Drawing.Color.White;
            this.uiRichTextBox1.Font = new System.Drawing.Font("宋体", 10F);
            this.uiRichTextBox1.Location = new System.Drawing.Point(3, 3);
            this.uiRichTextBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiRichTextBox1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiRichTextBox1.Name = "uiRichTextBox1";
            this.uiRichTextBox1.Padding = new System.Windows.Forms.Padding(2);
            this.uiRichTextBox1.ReadOnly = true;
            this.uiRichTextBox1.ScrollBarStyleInherited = false;
            this.uiRichTextBox1.ShowText = false;
            this.uiRichTextBox1.Size = new System.Drawing.Size(858, 487);
            this.uiRichTextBox1.TabIndex = 0;
            this.uiRichTextBox1.Text = "uiRichTextBox1";
            this.uiRichTextBox1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TpsFormDataViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 731);
            this.Controls.Add(this.mainPanel);
            this.Name = "TpsFormDataViewer";
            this.Text = "TPS929120 EEPROM Configuration Tool";
            this.mainPanel.ResumeLayout(false);
            this.dataTablePanel.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.titleTablePanel.ResumeLayout(false);
            this.onOffLineTablePanel.ResumeLayout(false);
            this.buttomTablePanel.ResumeLayout(false);
            this.uiTableLayoutPanel1.ResumeLayout(false);
            this.grpEepm.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainPanel;
        private System.Windows.Forms.TableLayoutPanel dataTablePanel;
        private System.Windows.Forms.TableLayoutPanel titleTablePanel;
        private System.Windows.Forms.TableLayoutPanel onOffLineTablePanel;
        private UserControls.LabelCombox cmbAddrList;
        private System.Windows.Forms.Button btnOnOffLine;
        private System.Windows.Forms.FlowLayoutPanel btnFlowPanel;
        private System.Windows.Forms.TableLayoutPanel buttomTablePanel;
        private HZH_Controls.Controls.Btn.UCBtnExt btnSaveParas;
        private HZH_Controls.Controls.Btn.UCBtnExt btnWriteThisTps;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private Sunny.UI.UITableLayoutPanel uiTableLayoutPanel1;
        private UserControls.UserDataGrid dgvIoutPwm;
        private System.Windows.Forms.GroupBox grpEepm;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private UserControls.UserDataGrid dgvChConfig;
        private UserControls.UserDataGrid dgvOtherConfig;
        private Sunny.UI.UIRichTextBox uiRichTextBox1;
    }
}