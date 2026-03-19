using HZH_Controls.Controls.Btn;

namespace CheckSystem.CAN
{
    partial class CanDataViewForm
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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.清空数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnTablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.ucBtnProduct = new UCBtnFillet();
            this.ucBtnIntelMatrix = new UCBtnFillet();
            this.ucBtnMotorolaMatrix = new UCBtnFillet();
            this.ucBtnUdsSendData = new UCBtnFillet();
            this.ucBtnNormalSendData = new UCBtnFillet();
            this.ucBtnOpenDevice = new UCBtnFillet();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.uiMillisecondTimer1 = new Sunny.UI.UIMillisecondTimer(this.components);
            this.uiPanel1 = new Sunny.UI.UIPanel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.uiDataGridView1 = new Sunny.UI.UIDataGridView();
            this.btnStartMonitor = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnClear = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSaveCanMsg = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.lblCmbCanChannel = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.lblCmbCanIdMin = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.lblCmbCanIdMax = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.btnCanIdRange = new System.Windows.Forms.ToolStripButton();
            this.lblCmbMaxCount = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.contextMenuStrip1.SuspendLayout();
            this.btnTablePanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.uiPanel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiDataGridView1)).BeginInit();
            this.SuspendLayout();
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
            this.btnTablePanel.Size = new System.Drawing.Size(1519, 94);
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
            this.ucBtnProduct.Location = new System.Drawing.Point(759, 5);
            this.ucBtnProduct.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ucBtnProduct.Name = "ucBtnProduct";
            this.ucBtnProduct.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ucBtnProduct.RectWidth = 1;
            this.ucBtnProduct.Size = new System.Drawing.Size(143, 84);
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
            this.ucBtnIntelMatrix.Location = new System.Drawing.Point(608, 5);
            this.ucBtnIntelMatrix.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ucBtnIntelMatrix.Name = "ucBtnIntelMatrix";
            this.ucBtnIntelMatrix.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ucBtnIntelMatrix.RectWidth = 1;
            this.ucBtnIntelMatrix.Size = new System.Drawing.Size(143, 84);
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
            this.ucBtnMotorolaMatrix.Location = new System.Drawing.Point(457, 5);
            this.ucBtnMotorolaMatrix.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ucBtnMotorolaMatrix.Name = "ucBtnMotorolaMatrix";
            this.ucBtnMotorolaMatrix.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ucBtnMotorolaMatrix.RectWidth = 1;
            this.ucBtnMotorolaMatrix.Size = new System.Drawing.Size(143, 84);
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
            this.ucBtnUdsSendData.Location = new System.Drawing.Point(306, 5);
            this.ucBtnUdsSendData.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ucBtnUdsSendData.Name = "ucBtnUdsSendData";
            this.ucBtnUdsSendData.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ucBtnUdsSendData.RectWidth = 1;
            this.ucBtnUdsSendData.Size = new System.Drawing.Size(143, 84);
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
            this.ucBtnNormalSendData.Location = new System.Drawing.Point(155, 5);
            this.ucBtnNormalSendData.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ucBtnNormalSendData.Name = "ucBtnNormalSendData";
            this.ucBtnNormalSendData.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ucBtnNormalSendData.RectWidth = 1;
            this.ucBtnNormalSendData.Size = new System.Drawing.Size(143, 84);
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
            this.ucBtnOpenDevice.Size = new System.Drawing.Size(143, 84);
            this.ucBtnOpenDevice.TabIndex = 0;
            this.ucBtnOpenDevice.BtnClick += new System.EventHandler(this.ucBtnFillet1_BtnClick);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.btnTablePanel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.uiPanel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1525, 644);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // uiMillisecondTimer1
            // 
            this.uiMillisecondTimer1.Enabled = true;
            this.uiMillisecondTimer1.Interval = 500;
            this.uiMillisecondTimer1.Tick += new System.EventHandler(this.uiMillisecondTimer1_Tick);
            // 
            // uiPanel1
            // 
            this.uiPanel1.Controls.Add(this.tableLayoutPanel2);
            this.uiPanel1.Controls.Add(this.toolStrip1);
            this.uiPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiPanel1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiPanel1.Location = new System.Drawing.Point(1, 101);
            this.uiPanel1.Margin = new System.Windows.Forms.Padding(1);
            this.uiPanel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Size = new System.Drawing.Size(1523, 542);
            this.uiPanel1.TabIndex = 4;
            this.uiPanel1.Text = "uiPanel1";
            this.uiPanel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnStartMonitor,
            this.toolStripSeparator1,
            this.btnClear,
            this.toolStripSeparator2,
            this.btnSaveCanMsg,
            this.toolStripSeparator3,
            this.lblCmbCanChannel,
            this.toolStripSeparator4,
            this.btnCanIdRange,
            this.toolStripSeparator7,
            this.toolStripLabel1,
            this.lblCmbCanIdMin,
            this.toolStripSeparator6,
            this.toolStripLabel2,
            this.lblCmbCanIdMax,
            this.toolStripSeparator5,
            this.toolStripLabel3,
            this.lblCmbMaxCount,
            this.toolStripSeparator8});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1523, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.uiDataGridView1, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 25);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1523, 517);
            this.tableLayoutPanel2.TabIndex = 5;
            // 
            // uiDataGridView1
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.uiDataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.uiDataGridView1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.uiDataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.uiDataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.uiDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.uiDataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(234)))), ((int)(((byte)(232)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.uiDataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            this.uiDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiDataGridView1.EnableHeadersVisualStyles = false;
            this.uiDataGridView1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiDataGridView1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(164)))), ((int)(((byte)(152)))));
            this.uiDataGridView1.Location = new System.Drawing.Point(3, 3);
            this.uiDataGridView1.Name = "uiDataGridView1";
            this.uiDataGridView1.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.uiDataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(234)))), ((int)(((byte)(232)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiDataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.uiDataGridView1.RowTemplate.Height = 23;
            this.uiDataGridView1.ScrollBarBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.uiDataGridView1.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.uiDataGridView1.ScrollBarRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.uiDataGridView1.SelectedIndex = -1;
            this.uiDataGridView1.Size = new System.Drawing.Size(1517, 511);
            this.uiDataGridView1.StripeOddColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.uiDataGridView1.Style = Sunny.UI.UIStyle.LayuiGreen;
            this.uiDataGridView1.TabIndex = 9;
            // 
            // btnStartMonitor
            // 
            this.btnStartMonitor.Image = global::CheckSystem.Properties.Resources.TSB_AddRectangle_Image;
            this.btnStartMonitor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStartMonitor.Name = "btnStartMonitor";
            this.btnStartMonitor.Size = new System.Drawing.Size(76, 22);
            this.btnStartMonitor.Text = "开启监控";
            this.btnStartMonitor.Click += new System.EventHandler(this.btnStartMonitor_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnClear
            // 
            this.btnClear.Image = global::CheckSystem.Properties.Resources.TSB_ClearShape_Image;
            this.btnClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(76, 22);
            this.btnClear.Text = "清空数据";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnSaveCanMsg
            // 
            this.btnSaveCanMsg.Enabled = false;
            this.btnSaveCanMsg.Image = global::CheckSystem.Properties.Resources.TSB_Save_Image;
            this.btnSaveCanMsg.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveCanMsg.Name = "btnSaveCanMsg";
            this.btnSaveCanMsg.Size = new System.Drawing.Size(52, 22);
            this.btnSaveCanMsg.Text = "保存";
            this.btnSaveCanMsg.Click += new System.EventHandler(this.btnSaveCanMsg_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // lblCmbCanChannel
            // 
            this.lblCmbCanChannel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lblCmbCanChannel.Name = "lblCmbCanChannel";
            this.lblCmbCanChannel.Size = new System.Drawing.Size(121, 25);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(87, 22);
            this.toolStripLabel1.Text = "最小CAN ID：";
            // 
            // lblCmbCanIdMin
            // 
            this.lblCmbCanIdMin.Enabled = false;
            this.lblCmbCanIdMin.Name = "lblCmbCanIdMin";
            this.lblCmbCanIdMin.Size = new System.Drawing.Size(100, 25);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(87, 22);
            this.toolStripLabel2.Text = "最大CAN ID：";
            // 
            // lblCmbCanIdMax
            // 
            this.lblCmbCanIdMax.Enabled = false;
            this.lblCmbCanIdMax.Name = "lblCmbCanIdMax";
            this.lblCmbCanIdMax.Size = new System.Drawing.Size(100, 25);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // btnCanIdRange
            // 
            this.btnCanIdRange.Image = global::CheckSystem.Properties.Resources.TSB_Detect_Image;
            this.btnCanIdRange.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCanIdRange.Name = "btnCanIdRange";
            this.btnCanIdRange.Size = new System.Drawing.Size(119, 22);
            this.btnCanIdRange.Text = "监控CAN ID范围";
            this.btnCanIdRange.Click += new System.EventHandler(this.btnCanIdRange_Click_1);
            // 
            // lblCmbMaxCount
            // 
            this.lblCmbMaxCount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lblCmbMaxCount.Name = "lblCmbMaxCount";
            this.lblCmbMaxCount.Size = new System.Drawing.Size(121, 25);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(92, 22);
            this.toolStripLabel3.Text = "最大监控数量：";
            // 
            // CanDataViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1525, 644);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "CanDataViewForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CAN视图";
            this.contextMenuStrip1.ResumeLayout(false);
            this.btnTablePanel.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.uiPanel1.ResumeLayout(false);
            this.uiPanel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiDataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 清空数据ToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel btnTablePanel;
        private UCBtnFillet ucBtnProduct;
        private UCBtnFillet ucBtnIntelMatrix;
        private UCBtnFillet ucBtnMotorolaMatrix;
        private UCBtnFillet ucBtnUdsSendData;
        private UCBtnFillet ucBtnNormalSendData;
        private UCBtnFillet ucBtnOpenDevice;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Sunny.UI.UIMillisecondTimer uiMillisecondTimer1;
        private Sunny.UI.UIPanel uiPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Sunny.UI.UIDataGridView uiDataGridView1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnStartMonitor;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnClear;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnSaveCanMsg;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripComboBox lblCmbCanChannel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox lblCmbCanIdMin;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripTextBox lblCmbCanIdMax;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton btnCanIdRange;
        private System.Windows.Forms.ToolStripComboBox lblCmbMaxCount;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;

    }
}