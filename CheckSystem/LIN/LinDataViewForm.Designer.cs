using HZH_Controls.Controls.Btn;

namespace CheckSystem.LIN
{
    partial class LinDataViewForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.linMsgTable = new System.Windows.Forms.TableLayoutPanel();
            this.LinListView = new CheckSystem.CAN.ListViewEx();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.清空数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblCmbMaxLinCount = new UserControls.LabelCombox();
            this.btnLinIdRange = new System.Windows.Forms.Button();
            this.lblCmbLinIdMax = new UserControls.LabelCombox();
            this.lblCmbLinIdMin = new UserControls.LabelCombox();
            this.lblCmbLinChannel = new UserControls.LabelCombox();
            this.btnSaveLinMsg = new System.Windows.Forms.Button();
            this.btnClearLinData = new System.Windows.Forms.Button();
            this.btnLinStartMonitor = new System.Windows.Forms.Button();
            this.btnTablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.ucBtnProduct = new UCBtnFillet();
            this.ucBtnIntelMatrix = new UCBtnFillet();
            this.ucBtnMotorolaMatrix = new UCBtnFillet();
            this.ucBtnUdsSendData = new UCBtnFillet();
            this.ucBtnNormalSendData = new UCBtnFillet();
            this.ucBtnOpenDevice = new UCBtnFillet();
            this.tableLayoutPanel1.SuspendLayout();
            this.linMsgTable.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.btnTablePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.linMsgTable, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnTablePanel, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1070, 644);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // linMsgTable
            // 
            this.linMsgTable.ColumnCount = 1;
            this.linMsgTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.linMsgTable.Controls.Add(this.LinListView, 0, 1);
            this.linMsgTable.Controls.Add(this.panel1, 0, 0);
            this.linMsgTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.linMsgTable.Location = new System.Drawing.Point(3, 103);
            this.linMsgTable.Name = "linMsgTable";
            this.linMsgTable.RowCount = 2;
            this.linMsgTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.linMsgTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.linMsgTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.linMsgTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.linMsgTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.linMsgTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.linMsgTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.linMsgTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.linMsgTable.Size = new System.Drawing.Size(1064, 538);
            this.linMsgTable.TabIndex = 4;
            // 
            // LinListView
            // 
            this.LinListView.ContextMenuStrip = this.contextMenuStrip1;
            this.LinListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LinListView.Location = new System.Drawing.Point(3, 53);
            this.LinListView.Name = "LinListView";
            this.LinListView.Size = new System.Drawing.Size(1058, 482);
            this.LinListView.TabIndex = 0;
            this.LinListView.UseCompatibleStateImageBehavior = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.清空数据ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 26);
            // 
            // 清空数据ToolStripMenuItem
            // 
            this.清空数据ToolStripMenuItem.Name = "清空数据ToolStripMenuItem";
            this.清空数据ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.清空数据ToolStripMenuItem.Text = "清空数据";
            this.清空数据ToolStripMenuItem.Click += new System.EventHandler(this.清空数据ToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.Snow;
            this.panel1.Controls.Add(this.lblCmbMaxLinCount);
            this.panel1.Controls.Add(this.btnLinIdRange);
            this.panel1.Controls.Add(this.lblCmbLinIdMax);
            this.panel1.Controls.Add(this.lblCmbLinIdMin);
            this.panel1.Controls.Add(this.lblCmbLinChannel);
            this.panel1.Controls.Add(this.btnSaveLinMsg);
            this.panel1.Controls.Add(this.btnClearLinData);
            this.panel1.Controls.Add(this.btnLinStartMonitor);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(1, 1);
            this.panel1.Margin = new System.Windows.Forms.Padding(1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1062, 48);
            this.panel1.TabIndex = 1;
            // 
            // lblCmbMaxLinCount
            // 
            this.lblCmbMaxLinCount.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblCmbMaxLinCount.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.lblCmbMaxLinCount.LabelString = "最大监控数量";
            this.lblCmbMaxLinCount.Location = new System.Drawing.Point(1199, 0);
            this.lblCmbMaxLinCount.Margin = new System.Windows.Forms.Padding(0);
            this.lblCmbMaxLinCount.Name = "lblCmbMaxLinCount";
            this.lblCmbMaxLinCount.Size = new System.Drawing.Size(404, 31);
            this.lblCmbMaxLinCount.TabIndex = 19;
            // 
            // btnLinIdRange
            // 
            this.btnLinIdRange.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnLinIdRange.Location = new System.Drawing.Point(1124, 0);
            this.btnLinIdRange.Name = "btnLinIdRange";
            this.btnLinIdRange.Size = new System.Drawing.Size(75, 31);
            this.btnLinIdRange.TabIndex = 18;
            this.btnLinIdRange.Text = "修改监控LinId范围";
            this.btnLinIdRange.UseVisualStyleBackColor = true;
            this.btnLinIdRange.Click += new System.EventHandler(this.btnLinIdRange_Click);
            // 
            // lblCmbLinIdMax
            // 
            this.lblCmbLinIdMax.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblCmbLinIdMax.Enabled = false;
            this.lblCmbLinIdMax.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.lblCmbLinIdMax.LabelString = "LinIdMax";
            this.lblCmbLinIdMax.Location = new System.Drawing.Point(814, 0);
            this.lblCmbLinIdMax.Margin = new System.Windows.Forms.Padding(0);
            this.lblCmbLinIdMax.Name = "lblCmbLinIdMax";
            this.lblCmbLinIdMax.Size = new System.Drawing.Size(310, 31);
            this.lblCmbLinIdMax.TabIndex = 17;
            // 
            // lblCmbLinIdMin
            // 
            this.lblCmbLinIdMin.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblCmbLinIdMin.Enabled = false;
            this.lblCmbLinIdMin.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.lblCmbLinIdMin.LabelString = "LinIdMin";
            this.lblCmbLinIdMin.Location = new System.Drawing.Point(504, 0);
            this.lblCmbLinIdMin.Margin = new System.Windows.Forms.Padding(0);
            this.lblCmbLinIdMin.Name = "lblCmbLinIdMin";
            this.lblCmbLinIdMin.Size = new System.Drawing.Size(310, 31);
            this.lblCmbLinIdMin.TabIndex = 16;
            // 
            // lblCmbLinChannel
            // 
            this.lblCmbLinChannel.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblCmbLinChannel.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.lblCmbLinChannel.LabelString = "LIN通道";
            this.lblCmbLinChannel.Location = new System.Drawing.Point(225, 0);
            this.lblCmbLinChannel.Margin = new System.Windows.Forms.Padding(0);
            this.lblCmbLinChannel.Name = "lblCmbLinChannel";
            this.lblCmbLinChannel.Size = new System.Drawing.Size(279, 31);
            this.lblCmbLinChannel.TabIndex = 15;
            // 
            // btnSaveLinMsg
            // 
            this.btnSaveLinMsg.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnSaveLinMsg.Location = new System.Drawing.Point(150, 0);
            this.btnSaveLinMsg.Name = "btnSaveLinMsg";
            this.btnSaveLinMsg.Size = new System.Drawing.Size(75, 31);
            this.btnSaveLinMsg.TabIndex = 14;
            this.btnSaveLinMsg.Text = "保存";
            this.btnSaveLinMsg.UseVisualStyleBackColor = true;
            this.btnSaveLinMsg.Click += new System.EventHandler(this.btnSaveLinMsg_Click);
            // 
            // btnClearLinData
            // 
            this.btnClearLinData.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnClearLinData.Location = new System.Drawing.Point(75, 0);
            this.btnClearLinData.Name = "btnClearLinData";
            this.btnClearLinData.Size = new System.Drawing.Size(75, 31);
            this.btnClearLinData.TabIndex = 3;
            this.btnClearLinData.Text = "清空数据";
            this.btnClearLinData.UseVisualStyleBackColor = true;
            this.btnClearLinData.Click += new System.EventHandler(this.btnClearLinData_Click);
            // 
            // btnLinStartMonitor
            // 
            this.btnLinStartMonitor.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnLinStartMonitor.Location = new System.Drawing.Point(0, 0);
            this.btnLinStartMonitor.Name = "btnLinStartMonitor";
            this.btnLinStartMonitor.Size = new System.Drawing.Size(75, 31);
            this.btnLinStartMonitor.TabIndex = 2;
            this.btnLinStartMonitor.Text = "开启监控";
            this.btnLinStartMonitor.UseVisualStyleBackColor = true;
            this.btnLinStartMonitor.Click += new System.EventHandler(this.btnLinStartMonitor_Click);
            // 
            // btnTablePanel
            // 
            this.btnTablePanel.ColumnCount = 10;
            this.btnTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.btnTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.btnTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.btnTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.btnTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.btnTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.btnTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.btnTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.btnTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.btnTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.btnTablePanel.Controls.Add(this.ucBtnProduct, 5, 0);
            this.btnTablePanel.Controls.Add(this.ucBtnIntelMatrix, 4, 0);
            this.btnTablePanel.Controls.Add(this.ucBtnMotorolaMatrix, 3, 0);
            this.btnTablePanel.Controls.Add(this.ucBtnUdsSendData, 2, 0);
            this.btnTablePanel.Controls.Add(this.ucBtnNormalSendData, 1, 0);
            this.btnTablePanel.Controls.Add(this.ucBtnOpenDevice, 0, 0);
            this.btnTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTablePanel.Location = new System.Drawing.Point(3, 3);
            this.btnTablePanel.Name = "btnTablePanel";
            this.btnTablePanel.RowCount = 1;
            this.btnTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.btnTablePanel.Size = new System.Drawing.Size(1064, 94);
            this.btnTablePanel.TabIndex = 3;
            // 
            // ucBtnProduct
            // 
            this.ucBtnProduct.BackColor = System.Drawing.Color.Transparent;
            this.ucBtnProduct.BtnImage = global::CheckSystem.Properties.Resources.TSB_Capture_Image;
            this.ucBtnProduct.BtnText = "调用产品";
            this.ucBtnProduct.ConerRadius = 5;
            this.ucBtnProduct.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucBtnProduct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucBtnProduct.FillColor = System.Drawing.Color.Transparent;
            this.ucBtnProduct.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ucBtnProduct.IsRadius = true;
            this.ucBtnProduct.IsShowRect = true;
            this.ucBtnProduct.Location = new System.Drawing.Point(534, 5);
            this.ucBtnProduct.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ucBtnProduct.Name = "ucBtnProduct";
            this.ucBtnProduct.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ucBtnProduct.RectWidth = 1;
            this.ucBtnProduct.Size = new System.Drawing.Size(98, 84);
            this.ucBtnProduct.TabIndex = 5;
            this.ucBtnProduct.BtnClick += new System.EventHandler(this.ucBtnProduct_BtnClick);
            // 
            // ucBtnIntelMatrix
            // 
            this.ucBtnIntelMatrix.BackColor = System.Drawing.Color.Transparent;
            this.ucBtnIntelMatrix.BtnImage = global::CheckSystem.Properties.Resources.TSB_ImageFit_Image;
            this.ucBtnIntelMatrix.BtnText = "Intel矩阵";
            this.ucBtnIntelMatrix.ConerRadius = 5;
            this.ucBtnIntelMatrix.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucBtnIntelMatrix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucBtnIntelMatrix.FillColor = System.Drawing.Color.Transparent;
            this.ucBtnIntelMatrix.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ucBtnIntelMatrix.IsRadius = true;
            this.ucBtnIntelMatrix.IsShowRect = true;
            this.ucBtnIntelMatrix.Location = new System.Drawing.Point(428, 5);
            this.ucBtnIntelMatrix.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ucBtnIntelMatrix.Name = "ucBtnIntelMatrix";
            this.ucBtnIntelMatrix.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ucBtnIntelMatrix.RectWidth = 1;
            this.ucBtnIntelMatrix.Size = new System.Drawing.Size(98, 84);
            this.ucBtnIntelMatrix.TabIndex = 4;
            this.ucBtnIntelMatrix.BtnClick += new System.EventHandler(this.ucBtnIntelMatrix_BtnClick);
            // 
            // ucBtnMotorolaMatrix
            // 
            this.ucBtnMotorolaMatrix.BackColor = System.Drawing.Color.Transparent;
            this.ucBtnMotorolaMatrix.BtnImage = global::CheckSystem.Properties.Resources.TSB_AddRectangle_Image;
            this.ucBtnMotorolaMatrix.BtnText = "Motorala矩阵";
            this.ucBtnMotorolaMatrix.ConerRadius = 5;
            this.ucBtnMotorolaMatrix.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucBtnMotorolaMatrix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucBtnMotorolaMatrix.FillColor = System.Drawing.Color.Transparent;
            this.ucBtnMotorolaMatrix.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ucBtnMotorolaMatrix.IsRadius = true;
            this.ucBtnMotorolaMatrix.IsShowRect = true;
            this.ucBtnMotorolaMatrix.Location = new System.Drawing.Point(322, 5);
            this.ucBtnMotorolaMatrix.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ucBtnMotorolaMatrix.Name = "ucBtnMotorolaMatrix";
            this.ucBtnMotorolaMatrix.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ucBtnMotorolaMatrix.RectWidth = 1;
            this.ucBtnMotorolaMatrix.Size = new System.Drawing.Size(98, 84);
            this.ucBtnMotorolaMatrix.TabIndex = 3;
            this.ucBtnMotorolaMatrix.BtnClick += new System.EventHandler(this.ucBtnMotorolaMatrix_BtnClick);
            // 
            // ucBtnUdsSendData
            // 
            this.ucBtnUdsSendData.BackColor = System.Drawing.Color.Transparent;
            this.ucBtnUdsSendData.BtnImage = global::CheckSystem.Properties.Resources.TSB_AddPolygon_Image;
            this.ucBtnUdsSendData.BtnText = "UDS诊断";
            this.ucBtnUdsSendData.ConerRadius = 5;
            this.ucBtnUdsSendData.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucBtnUdsSendData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucBtnUdsSendData.FillColor = System.Drawing.Color.Transparent;
            this.ucBtnUdsSendData.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ucBtnUdsSendData.IsRadius = true;
            this.ucBtnUdsSendData.IsShowRect = true;
            this.ucBtnUdsSendData.Location = new System.Drawing.Point(216, 5);
            this.ucBtnUdsSendData.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ucBtnUdsSendData.Name = "ucBtnUdsSendData";
            this.ucBtnUdsSendData.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ucBtnUdsSendData.RectWidth = 1;
            this.ucBtnUdsSendData.Size = new System.Drawing.Size(98, 84);
            this.ucBtnUdsSendData.TabIndex = 2;
            this.ucBtnUdsSendData.BtnClick += new System.EventHandler(this.ucBtnUdsSendData_BtnClick);
            // 
            // ucBtnNormalSendData
            // 
            this.ucBtnNormalSendData.BackColor = System.Drawing.Color.Transparent;
            this.ucBtnNormalSendData.BtnImage = global::CheckSystem.Properties.Resources.TSB_Detect_Image;
            this.ucBtnNormalSendData.BtnText = "普通发送数据";
            this.ucBtnNormalSendData.ConerRadius = 5;
            this.ucBtnNormalSendData.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucBtnNormalSendData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucBtnNormalSendData.FillColor = System.Drawing.Color.Transparent;
            this.ucBtnNormalSendData.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ucBtnNormalSendData.IsRadius = true;
            this.ucBtnNormalSendData.IsShowRect = true;
            this.ucBtnNormalSendData.Location = new System.Drawing.Point(110, 5);
            this.ucBtnNormalSendData.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ucBtnNormalSendData.Name = "ucBtnNormalSendData";
            this.ucBtnNormalSendData.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ucBtnNormalSendData.RectWidth = 1;
            this.ucBtnNormalSendData.Size = new System.Drawing.Size(98, 84);
            this.ucBtnNormalSendData.TabIndex = 1;
            this.ucBtnNormalSendData.BtnClick += new System.EventHandler(this.ucBtnNormalSendData_BtnClick);
            // 
            // ucBtnOpenDevice
            // 
            this.ucBtnOpenDevice.BackColor = System.Drawing.Color.Transparent;
            this.ucBtnOpenDevice.BtnImage = global::CheckSystem.Properties.Resources.Btn_GraySet_BackgroundImage;
            this.ucBtnOpenDevice.BtnText = "连接设备";
            this.ucBtnOpenDevice.ConerRadius = 5;
            this.ucBtnOpenDevice.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucBtnOpenDevice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucBtnOpenDevice.FillColor = System.Drawing.Color.Transparent;
            this.ucBtnOpenDevice.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ucBtnOpenDevice.IsRadius = true;
            this.ucBtnOpenDevice.IsShowRect = true;
            this.ucBtnOpenDevice.Location = new System.Drawing.Point(4, 5);
            this.ucBtnOpenDevice.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ucBtnOpenDevice.Name = "ucBtnOpenDevice";
            this.ucBtnOpenDevice.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ucBtnOpenDevice.RectWidth = 1;
            this.ucBtnOpenDevice.Size = new System.Drawing.Size(98, 84);
            this.ucBtnOpenDevice.TabIndex = 0;
            this.ucBtnOpenDevice.BtnClick += new System.EventHandler(this.ucBtnFillet1_BtnClick);
            // 
            // LinDataViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1070, 644);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "LinDataViewForm";
            this.Text = "LIN视图";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.linMsgTable.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.btnTablePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel btnTablePanel;
        private UCBtnFillet ucBtnOpenDevice;
        private UCBtnFillet ucBtnIntelMatrix;
        private UCBtnFillet ucBtnMotorolaMatrix;
        private UCBtnFillet ucBtnUdsSendData;
        private UCBtnFillet ucBtnNormalSendData;
        private UCBtnFillet ucBtnProduct;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 清空数据ToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel linMsgTable;
        private CAN.ListViewEx LinListView;
        private System.Windows.Forms.Panel panel1;
        private UserControls.LabelCombox lblCmbMaxLinCount;
        private System.Windows.Forms.Button btnLinIdRange;
        private UserControls.LabelCombox lblCmbLinIdMax;
        private UserControls.LabelCombox lblCmbLinIdMin;
        private UserControls.LabelCombox lblCmbLinChannel;
        private System.Windows.Forms.Button btnSaveLinMsg;
        private System.Windows.Forms.Button btnClearLinData;
        private System.Windows.Forms.Button btnLinStartMonitor;

    }
}