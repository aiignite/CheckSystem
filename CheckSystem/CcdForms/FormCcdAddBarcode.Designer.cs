namespace CheckSystem.CcdForms
{
    partial class FormCcdAddBarcode
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvBarcodeGroupList = new UserControls.UserDataGrid();
            this.lblTxtSoftwareIndex = new UserControls.LabelCombox();
            this.lblTxtSoftware = new UserControls.LabelText();
            this.lblTxtHardwareIndex = new UserControls.LabelCombox();
            this.lblTxtHardware = new UserControls.LabelText();
            this.lblTxtPartNoIndex = new UserControls.LabelCombox();
            this.lblTxtPartNo = new UserControls.LabelText();
            this.lblTxtBarcodeLength = new UserControls.LabelCombox();
            this.lblTxtBarcodeName = new UserControls.LabelText();
            this.btnConfirm = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvBarcodeGroupList);
            this.splitContainer1.Panel1.Controls.Add(this.lblTxtSoftwareIndex);
            this.splitContainer1.Panel1.Controls.Add(this.lblTxtSoftware);
            this.splitContainer1.Panel1.Controls.Add(this.lblTxtHardwareIndex);
            this.splitContainer1.Panel1.Controls.Add(this.lblTxtHardware);
            this.splitContainer1.Panel1.Controls.Add(this.lblTxtPartNoIndex);
            this.splitContainer1.Panel1.Controls.Add(this.lblTxtPartNo);
            this.splitContainer1.Panel1.Controls.Add(this.lblTxtBarcodeLength);
            this.splitContainer1.Panel1.Controls.Add(this.lblTxtBarcodeName);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnConfirm);
            this.splitContainer1.Size = new System.Drawing.Size(1053, 522);
            this.splitContainer1.SplitterDistance = 450;
            this.splitContainer1.TabIndex = 0;
            // 
            // dgvBarcodeGroupList
            // 
            this.dgvBarcodeGroupList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBarcodeGroupList.Location = new System.Drawing.Point(0, 296);
            this.dgvBarcodeGroupList.Name = "dgvBarcodeGroupList";
            this.dgvBarcodeGroupList.Size = new System.Drawing.Size(1053, 154);
            this.dgvBarcodeGroupList.TabIndex = 21;
            // 
            // lblTxtSoftwareIndex
            // 
            this.lblTxtSoftwareIndex.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblTxtSoftwareIndex.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTxtSoftwareIndex.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTxtSoftwareIndex.LabelString = "软件版本号位置";
            this.lblTxtSoftwareIndex.Location = new System.Drawing.Point(0, 259);
            this.lblTxtSoftwareIndex.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lblTxtSoftwareIndex.Name = "lblTxtSoftwareIndex";
            this.lblTxtSoftwareIndex.Size = new System.Drawing.Size(1053, 37);
            this.lblTxtSoftwareIndex.TabIndex = 19;
            // 
            // lblTxtSoftware
            // 
            this.lblTxtSoftware.BackColor = System.Drawing.Color.Wheat;
            this.lblTxtSoftware.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTxtSoftware.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTxtSoftware.LabelString = "软件版本号";
            this.lblTxtSoftware.Location = new System.Drawing.Point(0, 222);
            this.lblTxtSoftware.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lblTxtSoftware.Name = "lblTxtSoftware";
            this.lblTxtSoftware.Size = new System.Drawing.Size(1053, 37);
            this.lblTxtSoftware.TabIndex = 20;
            // 
            // lblTxtHardwareIndex
            // 
            this.lblTxtHardwareIndex.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblTxtHardwareIndex.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTxtHardwareIndex.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTxtHardwareIndex.LabelString = "大硬件版本号位置";
            this.lblTxtHardwareIndex.Location = new System.Drawing.Point(0, 185);
            this.lblTxtHardwareIndex.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lblTxtHardwareIndex.Name = "lblTxtHardwareIndex";
            this.lblTxtHardwareIndex.Size = new System.Drawing.Size(1053, 37);
            this.lblTxtHardwareIndex.TabIndex = 18;
            // 
            // lblTxtHardware
            // 
            this.lblTxtHardware.BackColor = System.Drawing.Color.Wheat;
            this.lblTxtHardware.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTxtHardware.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTxtHardware.LabelString = "大硬件版本号";
            this.lblTxtHardware.Location = new System.Drawing.Point(0, 148);
            this.lblTxtHardware.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lblTxtHardware.Name = "lblTxtHardware";
            this.lblTxtHardware.Size = new System.Drawing.Size(1053, 37);
            this.lblTxtHardware.TabIndex = 17;
            // 
            // lblTxtPartNoIndex
            // 
            this.lblTxtPartNoIndex.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblTxtPartNoIndex.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTxtPartNoIndex.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTxtPartNoIndex.LabelString = "零件号位置";
            this.lblTxtPartNoIndex.Location = new System.Drawing.Point(0, 111);
            this.lblTxtPartNoIndex.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lblTxtPartNoIndex.Name = "lblTxtPartNoIndex";
            this.lblTxtPartNoIndex.Size = new System.Drawing.Size(1053, 37);
            this.lblTxtPartNoIndex.TabIndex = 16;
            // 
            // lblTxtPartNo
            // 
            this.lblTxtPartNo.BackColor = System.Drawing.Color.Wheat;
            this.lblTxtPartNo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTxtPartNo.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTxtPartNo.LabelString = "零件号";
            this.lblTxtPartNo.Location = new System.Drawing.Point(0, 74);
            this.lblTxtPartNo.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lblTxtPartNo.Name = "lblTxtPartNo";
            this.lblTxtPartNo.Size = new System.Drawing.Size(1053, 37);
            this.lblTxtPartNo.TabIndex = 15;
            // 
            // lblTxtBarcodeLength
            // 
            this.lblTxtBarcodeLength.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblTxtBarcodeLength.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTxtBarcodeLength.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTxtBarcodeLength.LabelString = "总长度";
            this.lblTxtBarcodeLength.Location = new System.Drawing.Point(0, 37);
            this.lblTxtBarcodeLength.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lblTxtBarcodeLength.Name = "lblTxtBarcodeLength";
            this.lblTxtBarcodeLength.Size = new System.Drawing.Size(1053, 37);
            this.lblTxtBarcodeLength.TabIndex = 14;
            // 
            // lblTxtBarcodeName
            // 
            this.lblTxtBarcodeName.BackColor = System.Drawing.Color.Wheat;
            this.lblTxtBarcodeName.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTxtBarcodeName.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTxtBarcodeName.LabelString = "名称";
            this.lblTxtBarcodeName.Location = new System.Drawing.Point(0, 0);
            this.lblTxtBarcodeName.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lblTxtBarcodeName.Name = "lblTxtBarcodeName";
            this.lblTxtBarcodeName.Size = new System.Drawing.Size(1053, 37);
            this.lblTxtBarcodeName.TabIndex = 13;
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.DarkCyan;
            this.btnConfirm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnConfirm.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConfirm.Location = new System.Drawing.Point(0, 0);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(1053, 68);
            this.btnConfirm.TabIndex = 3;
            this.btnConfirm.Text = "确认";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // FormCcdAddBarcode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1053, 522);
            this.Controls.Add(this.splitContainer1);
            this.Name = "FormCcdAddBarcode";
            this.Text = "添加二维码";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private UserControls.UserDataGrid dgvBarcodeGroupList;
        private UserControls.LabelCombox lblTxtSoftwareIndex;
        private UserControls.LabelText lblTxtSoftware;
        private UserControls.LabelCombox lblTxtHardwareIndex;
        private UserControls.LabelText lblTxtHardware;
        private UserControls.LabelCombox lblTxtPartNoIndex;
        private UserControls.LabelText lblTxtPartNo;
        private UserControls.LabelCombox lblTxtBarcodeLength;
        private UserControls.LabelText lblTxtBarcodeName;
        private System.Windows.Forms.Button btnConfirm;

    }
}