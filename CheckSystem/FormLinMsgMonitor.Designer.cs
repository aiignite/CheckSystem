namespace CheckSystem
{
    partial class FormLinMsgMonitor
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
            this.linMsgTable = new System.Windows.Forms.TableLayoutPanel();
            this.LinListView = new CheckSystem.CAN.ListViewEx();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblCmbMaxLinCount = new UserControls.LabelCombox();
            this.btnLinIdRange = new System.Windows.Forms.Button();
            this.lblCmbLinIdMax = new UserControls.LabelCombox();
            this.lblCmbLinIdMin = new UserControls.LabelCombox();
            this.lblCmbLinChannel = new UserControls.LabelCombox();
            this.btnSaveLinMsg = new System.Windows.Forms.Button();
            this.btnClearLinData = new System.Windows.Forms.Button();
            this.btnLinStartMonitor = new System.Windows.Forms.Button();
            this.linMsgTable.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // linMsgTable
            // 
            this.linMsgTable.ColumnCount = 1;
            this.linMsgTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.linMsgTable.Controls.Add(this.LinListView, 0, 1);
            this.linMsgTable.Controls.Add(this.panel1, 0, 0);
            this.linMsgTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.linMsgTable.Location = new System.Drawing.Point(0, 0);
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
            this.linMsgTable.Size = new System.Drawing.Size(1188, 684);
            this.linMsgTable.TabIndex = 2;
            // 
            // LinListView
            // 
            this.LinListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LinListView.Location = new System.Drawing.Point(3, 53);
            this.LinListView.Name = "LinListView";
            this.LinListView.Size = new System.Drawing.Size(1182, 628);
            this.LinListView.TabIndex = 0;
            this.LinListView.UseCompatibleStateImageBehavior = false;
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
            this.panel1.Size = new System.Drawing.Size(1186, 48);
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
            // 
            // FormLinMsgMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1188, 684);
            this.Controls.Add(this.linMsgTable);
            this.Name = "FormLinMsgMonitor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormLinMsgMonitor";
            this.linMsgTable.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

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