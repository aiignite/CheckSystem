namespace CheckSystem
{
    partial class FormCanMsgMonitor
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.canMsgTable = new System.Windows.Forms.TableLayoutPanel();
            this.canBtnPanel = new System.Windows.Forms.Panel();
            this.lblCmbMaxCanCount = new UserControls.LabelCombox();
            this.btnCanIdRange = new System.Windows.Forms.Button();
            this.lblCmbCanIdMax = new UserControls.LabelCombox();
            this.lblCmbCanIdMin = new UserControls.LabelCombox();
            this.lblCmbCanChannel = new UserControls.LabelCombox();
            this.btnSaveCanMsg = new System.Windows.Forms.Button();
            this.btnClearCanData = new System.Windows.Forms.Button();
            this.btnCanStartMonitor = new System.Windows.Forms.Button();
            this.uiMillisecondTimer1 = new Sunny.UI.UIMillisecondTimer(this.components);
            this.uiDataGridView1 = new Sunny.UI.UIDataGridView();
            this.canMsgTable.SuspendLayout();
            this.canBtnPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiDataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // canMsgTable
            // 
            this.canMsgTable.ColumnCount = 1;
            this.canMsgTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.canMsgTable.Controls.Add(this.canBtnPanel, 0, 0);
            this.canMsgTable.Controls.Add(this.uiDataGridView1, 0, 1);
            this.canMsgTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.canMsgTable.Location = new System.Drawing.Point(0, 0);
            this.canMsgTable.Name = "canMsgTable";
            this.canMsgTable.RowCount = 2;
            this.canMsgTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.canMsgTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.canMsgTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.canMsgTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.canMsgTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.canMsgTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.canMsgTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.canMsgTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.canMsgTable.Size = new System.Drawing.Size(1121, 716);
            this.canMsgTable.TabIndex = 1;
            // 
            // canBtnPanel
            // 
            this.canBtnPanel.AutoScroll = true;
            this.canBtnPanel.BackColor = System.Drawing.Color.Snow;
            this.canBtnPanel.Controls.Add(this.lblCmbMaxCanCount);
            this.canBtnPanel.Controls.Add(this.btnCanIdRange);
            this.canBtnPanel.Controls.Add(this.lblCmbCanIdMax);
            this.canBtnPanel.Controls.Add(this.lblCmbCanIdMin);
            this.canBtnPanel.Controls.Add(this.lblCmbCanChannel);
            this.canBtnPanel.Controls.Add(this.btnSaveCanMsg);
            this.canBtnPanel.Controls.Add(this.btnClearCanData);
            this.canBtnPanel.Controls.Add(this.btnCanStartMonitor);
            this.canBtnPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.canBtnPanel.Location = new System.Drawing.Point(1, 1);
            this.canBtnPanel.Margin = new System.Windows.Forms.Padding(1);
            this.canBtnPanel.Name = "canBtnPanel";
            this.canBtnPanel.Size = new System.Drawing.Size(1119, 48);
            this.canBtnPanel.TabIndex = 1;
            // 
            // lblCmbMaxCanCount
            // 
            this.lblCmbMaxCanCount.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblCmbMaxCanCount.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.lblCmbMaxCanCount.LabelString = "最大监控数量";
            this.lblCmbMaxCanCount.Location = new System.Drawing.Point(1199, 0);
            this.lblCmbMaxCanCount.Margin = new System.Windows.Forms.Padding(0);
            this.lblCmbMaxCanCount.Name = "lblCmbMaxCanCount";
            this.lblCmbMaxCanCount.Size = new System.Drawing.Size(404, 31);
            this.lblCmbMaxCanCount.TabIndex = 19;
            // 
            // btnCanIdRange
            // 
            this.btnCanIdRange.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCanIdRange.Location = new System.Drawing.Point(1124, 0);
            this.btnCanIdRange.Name = "btnCanIdRange";
            this.btnCanIdRange.Size = new System.Drawing.Size(75, 31);
            this.btnCanIdRange.TabIndex = 18;
            this.btnCanIdRange.Text = "修改监控CanId范围";
            this.btnCanIdRange.UseVisualStyleBackColor = true;
            // 
            // lblCmbCanIdMax
            // 
            this.lblCmbCanIdMax.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblCmbCanIdMax.Enabled = false;
            this.lblCmbCanIdMax.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.lblCmbCanIdMax.LabelString = "CanIdMax";
            this.lblCmbCanIdMax.Location = new System.Drawing.Point(814, 0);
            this.lblCmbCanIdMax.Margin = new System.Windows.Forms.Padding(0);
            this.lblCmbCanIdMax.Name = "lblCmbCanIdMax";
            this.lblCmbCanIdMax.Size = new System.Drawing.Size(310, 31);
            this.lblCmbCanIdMax.TabIndex = 17;
            // 
            // lblCmbCanIdMin
            // 
            this.lblCmbCanIdMin.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblCmbCanIdMin.Enabled = false;
            this.lblCmbCanIdMin.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.lblCmbCanIdMin.LabelString = "CanIdMin";
            this.lblCmbCanIdMin.Location = new System.Drawing.Point(504, 0);
            this.lblCmbCanIdMin.Margin = new System.Windows.Forms.Padding(0);
            this.lblCmbCanIdMin.Name = "lblCmbCanIdMin";
            this.lblCmbCanIdMin.Size = new System.Drawing.Size(310, 31);
            this.lblCmbCanIdMin.TabIndex = 16;
            // 
            // lblCmbCanChannel
            // 
            this.lblCmbCanChannel.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblCmbCanChannel.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.lblCmbCanChannel.LabelString = "CAN通道";
            this.lblCmbCanChannel.Location = new System.Drawing.Point(225, 0);
            this.lblCmbCanChannel.Margin = new System.Windows.Forms.Padding(0);
            this.lblCmbCanChannel.Name = "lblCmbCanChannel";
            this.lblCmbCanChannel.Size = new System.Drawing.Size(279, 31);
            this.lblCmbCanChannel.TabIndex = 15;
            // 
            // btnSaveCanMsg
            // 
            this.btnSaveCanMsg.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnSaveCanMsg.Location = new System.Drawing.Point(150, 0);
            this.btnSaveCanMsg.Name = "btnSaveCanMsg";
            this.btnSaveCanMsg.Size = new System.Drawing.Size(75, 31);
            this.btnSaveCanMsg.TabIndex = 14;
            this.btnSaveCanMsg.Text = "保存";
            this.btnSaveCanMsg.UseVisualStyleBackColor = true;
            // 
            // btnClearCanData
            // 
            this.btnClearCanData.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnClearCanData.Location = new System.Drawing.Point(75, 0);
            this.btnClearCanData.Name = "btnClearCanData";
            this.btnClearCanData.Size = new System.Drawing.Size(75, 31);
            this.btnClearCanData.TabIndex = 3;
            this.btnClearCanData.Text = "清空数据";
            this.btnClearCanData.UseVisualStyleBackColor = true;
            // 
            // btnCanStartMonitor
            // 
            this.btnCanStartMonitor.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCanStartMonitor.Location = new System.Drawing.Point(0, 0);
            this.btnCanStartMonitor.Name = "btnCanStartMonitor";
            this.btnCanStartMonitor.Size = new System.Drawing.Size(75, 31);
            this.btnCanStartMonitor.TabIndex = 2;
            this.btnCanStartMonitor.Text = "开启监控";
            this.btnCanStartMonitor.UseVisualStyleBackColor = true;
            // 
            // uiMillisecondTimer1
            // 
            this.uiMillisecondTimer1.Enabled = true;
            this.uiMillisecondTimer1.Interval = 500;
            this.uiMillisecondTimer1.Tick += new System.EventHandler(this.uiMillisecondTimer1_Tick);
            // 
            // uiDataGridView1
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.uiDataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.uiDataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.uiDataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.uiDataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.uiDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.uiDataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            this.uiDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiDataGridView1.EnableHeadersVisualStyles = false;
            this.uiDataGridView1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiDataGridView1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            this.uiDataGridView1.Location = new System.Drawing.Point(3, 53);
            this.uiDataGridView1.Name = "uiDataGridView1";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.uiDataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiDataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.uiDataGridView1.RowTemplate.Height = 23;
            this.uiDataGridView1.ScrollBarRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            this.uiDataGridView1.SelectedIndex = -1;
            this.uiDataGridView1.Size = new System.Drawing.Size(1115, 660);
            this.uiDataGridView1.StripeOddColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.uiDataGridView1.TabIndex = 2;
            // 
            // FormCanMsgMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1121, 716);
            this.Controls.Add(this.canMsgTable);
            this.Name = "FormCanMsgMonitor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormCanMsgMonitor";
            this.canMsgTable.ResumeLayout(false);
            this.canBtnPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiDataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel canMsgTable;
        private System.Windows.Forms.Panel canBtnPanel;
        private UserControls.LabelCombox lblCmbMaxCanCount;
        private System.Windows.Forms.Button btnCanIdRange;
        private UserControls.LabelCombox lblCmbCanIdMax;
        private UserControls.LabelCombox lblCmbCanIdMin;
        private UserControls.LabelCombox lblCmbCanChannel;
        private System.Windows.Forms.Button btnSaveCanMsg;
        private System.Windows.Forms.Button btnClearCanData;
        private System.Windows.Forms.Button btnCanStartMonitor;
        private Sunny.UI.UIMillisecondTimer uiMillisecondTimer1;
        private Sunny.UI.UIDataGridView uiDataGridView1;
    }
}