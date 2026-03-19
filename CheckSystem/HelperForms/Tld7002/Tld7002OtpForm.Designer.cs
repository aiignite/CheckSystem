namespace CheckSystem.HelperForms.Tld7002
{
    partial class Tld7002OtpForm
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
            this.tableMain = new System.Windows.Forms.TableLayoutPanel();
            this.tbaleLeft = new System.Windows.Forms.TableLayoutPanel();
            this.btnSaveCfgList = new System.Windows.Forms.Button();
            this.btnAddCfgToList = new System.Windows.Forms.Button();
            this.txtSelectedFilePath = new System.Windows.Forms.TextBox();
            this.ckFilesList = new System.Windows.Forms.Panel();
            this.btnClearCfgList = new System.Windows.Forms.Button();
            this.tableRight = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSaveParas = new System.Windows.Forms.Button();
            this.btnEditParas = new System.Windows.Forms.Button();
            this.txtInputBtn = new UserControls.LabelCombox();
            this.txtPowerDelay = new UserControls.LabelCombox();
            this.txtGpin0 = new UserControls.LabelCombox();
            this.txtVs = new UserControls.LabelCombox();
            this.txtControllerType = new UserControls.LabelText();
            this.txtIp = new UserControls.LabelText();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnOtpRead = new System.Windows.Forms.Button();
            this.btnOtpWrite = new System.Windows.Forms.Button();
            this.btnEmulate = new System.Windows.Forms.Button();
            this.txtOtpCostTime = new UserControls.LabelText();
            this.txtOtpResult = new UserControls.LabelText();
            this.txtOtpStatus = new UserControls.LabelText();
            this.tableMain.SuspendLayout();
            this.tbaleLeft.SuspendLayout();
            this.tableRight.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableMain
            // 
            this.tableMain.ColumnCount = 2;
            this.tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableMain.Controls.Add(this.tbaleLeft, 0, 0);
            this.tableMain.Controls.Add(this.tableRight, 1, 0);
            this.tableMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableMain.Location = new System.Drawing.Point(0, 0);
            this.tableMain.Margin = new System.Windows.Forms.Padding(1);
            this.tableMain.Name = "tableMain";
            this.tableMain.RowCount = 1;
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableMain.Size = new System.Drawing.Size(1008, 761);
            this.tableMain.TabIndex = 0;
            // 
            // tbaleLeft
            // 
            this.tbaleLeft.ColumnCount = 3;
            this.tbaleLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tbaleLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tbaleLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tbaleLeft.Controls.Add(this.btnSaveCfgList, 2, 2);
            this.tbaleLeft.Controls.Add(this.btnAddCfgToList, 1, 2);
            this.tbaleLeft.Controls.Add(this.txtSelectedFilePath, 0, 1);
            this.tbaleLeft.Controls.Add(this.ckFilesList, 0, 0);
            this.tbaleLeft.Controls.Add(this.btnClearCfgList, 0, 2);
            this.tbaleLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbaleLeft.Location = new System.Drawing.Point(1, 1);
            this.tbaleLeft.Margin = new System.Windows.Forms.Padding(1);
            this.tbaleLeft.Name = "tbaleLeft";
            this.tbaleLeft.RowCount = 3;
            this.tbaleLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tbaleLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tbaleLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tbaleLeft.Size = new System.Drawing.Size(602, 759);
            this.tbaleLeft.TabIndex = 0;
            // 
            // btnSaveCfgList
            // 
            this.btnSaveCfgList.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.btnSaveCfgList.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSaveCfgList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaveCfgList.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold);
            this.btnSaveCfgList.Location = new System.Drawing.Point(401, 710);
            this.btnSaveCfgList.Margin = new System.Windows.Forms.Padding(1);
            this.btnSaveCfgList.Name = "btnSaveCfgList";
            this.btnSaveCfgList.Size = new System.Drawing.Size(200, 48);
            this.btnSaveCfgList.TabIndex = 6;
            this.btnSaveCfgList.Text = "保存列表";
            this.btnSaveCfgList.UseVisualStyleBackColor = false;
            this.btnSaveCfgList.Click += new System.EventHandler(this.btnSaveCfgList_Click);
            // 
            // btnAddCfgToList
            // 
            this.btnAddCfgToList.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.btnAddCfgToList.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddCfgToList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddCfgToList.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold);
            this.btnAddCfgToList.Location = new System.Drawing.Point(201, 710);
            this.btnAddCfgToList.Margin = new System.Windows.Forms.Padding(1);
            this.btnAddCfgToList.Name = "btnAddCfgToList";
            this.btnAddCfgToList.Size = new System.Drawing.Size(198, 48);
            this.btnAddCfgToList.TabIndex = 5;
            this.btnAddCfgToList.Text = "添加文件";
            this.btnAddCfgToList.UseVisualStyleBackColor = false;
            this.btnAddCfgToList.Click += new System.EventHandler(this.btnAddCfgToList_Click);
            // 
            // txtSelectedFilePath
            // 
            this.txtSelectedFilePath.BackColor = System.Drawing.Color.LightGray;
            this.tbaleLeft.SetColumnSpan(this.txtSelectedFilePath, 3);
            this.txtSelectedFilePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSelectedFilePath.Enabled = false;
            this.txtSelectedFilePath.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSelectedFilePath.Location = new System.Drawing.Point(1, 610);
            this.txtSelectedFilePath.Margin = new System.Windows.Forms.Padding(1);
            this.txtSelectedFilePath.Multiline = true;
            this.txtSelectedFilePath.Name = "txtSelectedFilePath";
            this.txtSelectedFilePath.Size = new System.Drawing.Size(600, 98);
            this.txtSelectedFilePath.TabIndex = 2;
            // 
            // ckFilesList
            // 
            this.ckFilesList.AutoScroll = true;
            this.ckFilesList.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tbaleLeft.SetColumnSpan(this.ckFilesList, 3);
            this.ckFilesList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ckFilesList.Location = new System.Drawing.Point(1, 1);
            this.ckFilesList.Margin = new System.Windows.Forms.Padding(1);
            this.ckFilesList.Name = "ckFilesList";
            this.ckFilesList.Size = new System.Drawing.Size(600, 607);
            this.ckFilesList.TabIndex = 3;
            // 
            // btnClearCfgList
            // 
            this.btnClearCfgList.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.btnClearCfgList.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClearCfgList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClearCfgList.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClearCfgList.Location = new System.Drawing.Point(1, 710);
            this.btnClearCfgList.Margin = new System.Windows.Forms.Padding(1);
            this.btnClearCfgList.Name = "btnClearCfgList";
            this.btnClearCfgList.Size = new System.Drawing.Size(198, 48);
            this.btnClearCfgList.TabIndex = 4;
            this.btnClearCfgList.Text = "清空列表";
            this.btnClearCfgList.UseVisualStyleBackColor = false;
            this.btnClearCfgList.Click += new System.EventHandler(this.btnClearCfgList_Click);
            // 
            // tableRight
            // 
            this.tableRight.BackColor = System.Drawing.SystemColors.Control;
            this.tableRight.ColumnCount = 1;
            this.tableRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableRight.Controls.Add(this.groupBox1, 0, 0);
            this.tableRight.Controls.Add(this.groupBox2, 0, 1);
            this.tableRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableRight.Location = new System.Drawing.Point(605, 1);
            this.tableRight.Margin = new System.Windows.Forms.Padding(1);
            this.tableRight.Name = "tableRight";
            this.tableRight.RowCount = 2;
            this.tableRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableRight.Size = new System.Drawing.Size(402, 759);
            this.tableRight.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.LightBlue;
            this.groupBox1.Controls.Add(this.btnSaveParas);
            this.groupBox1.Controls.Add(this.btnEditParas);
            this.groupBox1.Controls.Add(this.txtInputBtn);
            this.groupBox1.Controls.Add(this.txtPowerDelay);
            this.groupBox1.Controls.Add(this.txtGpin0);
            this.groupBox1.Controls.Add(this.txtVs);
            this.groupBox1.Controls.Add(this.txtControllerType);
            this.groupBox1.Controls.Add(this.txtIp);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(396, 373);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "硬件参数";
            // 
            // btnSaveParas
            // 
            this.btnSaveParas.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSaveParas.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSaveParas.Enabled = false;
            this.btnSaveParas.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSaveParas.Location = new System.Drawing.Point(3, 289);
            this.btnSaveParas.Name = "btnSaveParas";
            this.btnSaveParas.Size = new System.Drawing.Size(390, 50);
            this.btnSaveParas.TabIndex = 34;
            this.btnSaveParas.Text = "保存硬件参数";
            this.btnSaveParas.UseVisualStyleBackColor = true;
            this.btnSaveParas.Click += new System.EventHandler(this.buttonParaSave_Click);
            // 
            // btnEditParas
            // 
            this.btnEditParas.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEditParas.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnEditParas.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnEditParas.Location = new System.Drawing.Point(3, 239);
            this.btnEditParas.Name = "btnEditParas";
            this.btnEditParas.Size = new System.Drawing.Size(390, 50);
            this.btnEditParas.TabIndex = 33;
            this.btnEditParas.Text = "编辑";
            this.btnEditParas.UseVisualStyleBackColor = true;
            this.btnEditParas.Click += new System.EventHandler(this.buttonParaEdit_Click);
            // 
            // txtInputBtn
            // 
            this.txtInputBtn.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtInputBtn.Enabled = false;
            this.txtInputBtn.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtInputBtn.LabelString = "InputButton";
            this.txtInputBtn.Location = new System.Drawing.Point(3, 202);
            this.txtInputBtn.Margin = new System.Windows.Forms.Padding(0);
            this.txtInputBtn.Name = "txtInputBtn";
            this.txtInputBtn.Size = new System.Drawing.Size(390, 37);
            this.txtInputBtn.TabIndex = 32;
            // 
            // txtPowerDelay
            // 
            this.txtPowerDelay.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtPowerDelay.Enabled = false;
            this.txtPowerDelay.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPowerDelay.LabelString = "PowerDelay";
            this.txtPowerDelay.Location = new System.Drawing.Point(3, 165);
            this.txtPowerDelay.Margin = new System.Windows.Forms.Padding(0);
            this.txtPowerDelay.Name = "txtPowerDelay";
            this.txtPowerDelay.Size = new System.Drawing.Size(390, 37);
            this.txtPowerDelay.TabIndex = 31;
            // 
            // txtGpin0
            // 
            this.txtGpin0.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtGpin0.Enabled = false;
            this.txtGpin0.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtGpin0.LabelString = "GPIN0";
            this.txtGpin0.Location = new System.Drawing.Point(3, 128);
            this.txtGpin0.Margin = new System.Windows.Forms.Padding(0);
            this.txtGpin0.Name = "txtGpin0";
            this.txtGpin0.Size = new System.Drawing.Size(390, 37);
            this.txtGpin0.TabIndex = 30;
            // 
            // txtVs
            // 
            this.txtVs.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtVs.Enabled = false;
            this.txtVs.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtVs.LabelString = "VS";
            this.txtVs.Location = new System.Drawing.Point(3, 91);
            this.txtVs.Margin = new System.Windows.Forms.Padding(0);
            this.txtVs.Name = "txtVs";
            this.txtVs.Size = new System.Drawing.Size(390, 37);
            this.txtVs.TabIndex = 29;
            // 
            // txtControllerType
            // 
            this.txtControllerType.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtControllerType.Enabled = false;
            this.txtControllerType.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtControllerType.LabelString = "Controller";
            this.txtControllerType.Location = new System.Drawing.Point(3, 54);
            this.txtControllerType.Margin = new System.Windows.Forms.Padding(2);
            this.txtControllerType.Name = "txtControllerType";
            this.txtControllerType.Size = new System.Drawing.Size(390, 37);
            this.txtControllerType.TabIndex = 28;
            // 
            // txtIp
            // 
            this.txtIp.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtIp.Enabled = false;
            this.txtIp.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtIp.LabelString = "IP";
            this.txtIp.Location = new System.Drawing.Point(3, 17);
            this.txtIp.Margin = new System.Windows.Forms.Padding(2);
            this.txtIp.Name = "txtIp";
            this.txtIp.Size = new System.Drawing.Size(390, 37);
            this.txtIp.TabIndex = 21;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.groupBox2.Controls.Add(this.btnOtpRead);
            this.groupBox2.Controls.Add(this.btnOtpWrite);
            this.groupBox2.Controls.Add(this.btnEmulate);
            this.groupBox2.Controls.Add(this.txtOtpCostTime);
            this.groupBox2.Controls.Add(this.txtOtpResult);
            this.groupBox2.Controls.Add(this.txtOtpStatus);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 382);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(396, 374);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "OTP操作";
            // 
            // btnOtpRead
            // 
            this.btnOtpRead.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOtpRead.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnOtpRead.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOtpRead.Location = new System.Drawing.Point(3, 228);
            this.btnOtpRead.Name = "btnOtpRead";
            this.btnOtpRead.Size = new System.Drawing.Size(390, 50);
            this.btnOtpRead.TabIndex = 17;
            this.btnOtpRead.Text = "OTP Read";
            this.btnOtpRead.UseVisualStyleBackColor = true;
            this.btnOtpRead.Click += new System.EventHandler(this.btnOtpRead_Click);
            // 
            // btnOtpWrite
            // 
            this.btnOtpWrite.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOtpWrite.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnOtpWrite.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOtpWrite.Location = new System.Drawing.Point(3, 178);
            this.btnOtpWrite.Name = "btnOtpWrite";
            this.btnOtpWrite.Size = new System.Drawing.Size(390, 50);
            this.btnOtpWrite.TabIndex = 16;
            this.btnOtpWrite.Text = "OTP Write";
            this.btnOtpWrite.UseVisualStyleBackColor = true;
            this.btnOtpWrite.Click += new System.EventHandler(this.btnOtpWrite_Click);
            // 
            // btnEmulate
            // 
            this.btnEmulate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEmulate.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnEmulate.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnEmulate.Location = new System.Drawing.Point(3, 128);
            this.btnEmulate.Name = "btnEmulate";
            this.btnEmulate.Size = new System.Drawing.Size(390, 50);
            this.btnEmulate.TabIndex = 15;
            this.btnEmulate.Text = "Emulate";
            this.btnEmulate.UseVisualStyleBackColor = true;
            this.btnEmulate.Click += new System.EventHandler(this.btnEmulate_Click);
            // 
            // txtOtpCostTime
            // 
            this.txtOtpCostTime.BackColor = System.Drawing.SystemColors.Control;
            this.txtOtpCostTime.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtOtpCostTime.Enabled = false;
            this.txtOtpCostTime.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtOtpCostTime.LabelString = "OTP Time";
            this.txtOtpCostTime.Location = new System.Drawing.Point(3, 91);
            this.txtOtpCostTime.Margin = new System.Windows.Forms.Padding(5);
            this.txtOtpCostTime.Name = "txtOtpCostTime";
            this.txtOtpCostTime.Size = new System.Drawing.Size(390, 37);
            this.txtOtpCostTime.TabIndex = 14;
            // 
            // txtOtpResult
            // 
            this.txtOtpResult.BackColor = System.Drawing.SystemColors.Control;
            this.txtOtpResult.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtOtpResult.Enabled = false;
            this.txtOtpResult.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtOtpResult.LabelString = "OTP Result";
            this.txtOtpResult.Location = new System.Drawing.Point(3, 54);
            this.txtOtpResult.Margin = new System.Windows.Forms.Padding(5);
            this.txtOtpResult.Name = "txtOtpResult";
            this.txtOtpResult.Size = new System.Drawing.Size(390, 37);
            this.txtOtpResult.TabIndex = 1;
            // 
            // txtOtpStatus
            // 
            this.txtOtpStatus.BackColor = System.Drawing.SystemColors.Control;
            this.txtOtpStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtOtpStatus.Enabled = false;
            this.txtOtpStatus.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtOtpStatus.LabelString = "OTP Status";
            this.txtOtpStatus.Location = new System.Drawing.Point(3, 17);
            this.txtOtpStatus.Margin = new System.Windows.Forms.Padding(5);
            this.txtOtpStatus.Name = "txtOtpStatus";
            this.txtOtpStatus.Size = new System.Drawing.Size(390, 37);
            this.txtOtpStatus.TabIndex = 0;
            // 
            // Tld7002OtpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 761);
            this.Controls.Add(this.tableMain);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1024, 800);
            this.MinimumSize = new System.Drawing.Size(1024, 800);
            this.Name = "Tld7002OtpForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TLD7002-OTP刷写工具";
            this.tableMain.ResumeLayout(false);
            this.tbaleLeft.ResumeLayout(false);
            this.tbaleLeft.PerformLayout();
            this.tableRight.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableMain;
        private System.Windows.Forms.TableLayoutPanel tbaleLeft;
        private System.Windows.Forms.TextBox txtSelectedFilePath;
        private System.Windows.Forms.Panel ckFilesList;
        private System.Windows.Forms.Button btnAddCfgToList;
        private System.Windows.Forms.Button btnClearCfgList;
        private System.Windows.Forms.Button btnSaveCfgList;
        private System.Windows.Forms.TableLayoutPanel tableRight;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private UserControls.LabelText txtOtpResult;
        private UserControls.LabelText txtOtpStatus;
        private UserControls.LabelText txtIp;
        private UserControls.LabelText txtControllerType;
        private System.Windows.Forms.Button btnSaveParas;
        private System.Windows.Forms.Button btnEditParas;
        private UserControls.LabelCombox txtInputBtn;
        private UserControls.LabelCombox txtPowerDelay;
        private UserControls.LabelCombox txtGpin0;
        private UserControls.LabelCombox txtVs;
        private System.Windows.Forms.Button btnOtpRead;
        private System.Windows.Forms.Button btnOtpWrite;
        private System.Windows.Forms.Button btnEmulate;
        private UserControls.LabelText txtOtpCostTime;

    }
}