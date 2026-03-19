using HZH_Controls.Controls.RadioButton;

namespace CheckSystem.LIN
{
    partial class LinDataNormalSendForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.txtSlaveLinId = new UserControls.LabelText();
            this.txtLinData = new UserControls.LabelText();
            this.txtFrameName = new UserControls.LabelText();
            this.txtMasterLinId = new UserControls.LabelText();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.btnAddToList = new System.Windows.Forms.Button();
            this.btnSendData = new System.Windows.Forms.Button();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.rbSendSlaveLin = new UCRadioButton();
            this.rbSendMasterLin = new UCRadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.userDataGrid1 = new UserControls.UserDataGrid();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.button9 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.txtSendInterval = new UserControls.LabelText();
            this.txtSendTimes = new UserControls.LabelText();
            this.cmbMasterSlaveDelayMs = new UserControls.LabelCombox();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1034, 654);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(5, 5);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1024, 186);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "帧发送";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.txtSlaveLinId, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtLinData, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.txtFrameName, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.txtMasterLinId, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel5, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel6, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel7, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.cmbMasterSlaveDelayMs, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 17);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1018, 166);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // txtSlaveLinId
            // 
            this.txtSlaveLinId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSlaveLinId.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSlaveLinId.LabelString = "响应帧ID(0x)：";
            this.txtSlaveLinId.Location = new System.Drawing.Point(2, 43);
            this.txtSlaveLinId.Margin = new System.Windows.Forms.Padding(2);
            this.txtSlaveLinId.Name = "txtSlaveLinId";
            this.txtSlaveLinId.Size = new System.Drawing.Size(505, 37);
            this.txtSlaveLinId.TabIndex = 2;
            // 
            // txtLinData
            // 
            this.txtLinData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLinData.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtLinData.LabelString = "数据(0x)：";
            this.txtLinData.Location = new System.Drawing.Point(2, 125);
            this.txtLinData.Margin = new System.Windows.Forms.Padding(2);
            this.txtLinData.Name = "txtLinData";
            this.txtLinData.Size = new System.Drawing.Size(505, 39);
            this.txtLinData.TabIndex = 4;
            // 
            // txtFrameName
            // 
            this.txtFrameName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFrameName.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtFrameName.LabelString = "名称(可选)：";
            this.txtFrameName.Location = new System.Drawing.Point(2, 84);
            this.txtFrameName.Margin = new System.Windows.Forms.Padding(2);
            this.txtFrameName.Name = "txtFrameName";
            this.txtFrameName.Size = new System.Drawing.Size(505, 37);
            this.txtFrameName.TabIndex = 3;
            // 
            // txtMasterLinId
            // 
            this.txtMasterLinId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMasterLinId.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtMasterLinId.LabelString = "命令帧ID(0x)：";
            this.txtMasterLinId.Location = new System.Drawing.Point(2, 2);
            this.txtMasterLinId.Margin = new System.Windows.Forms.Padding(2);
            this.txtMasterLinId.Name = "txtMasterLinId";
            this.txtMasterLinId.Size = new System.Drawing.Size(505, 37);
            this.txtMasterLinId.TabIndex = 1;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Controls.Add(this.btnAddToList, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnSendData, 1, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(512, 126);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(503, 37);
            this.tableLayoutPanel5.TabIndex = 8;
            // 
            // btnAddToList
            // 
            this.btnAddToList.BackColor = System.Drawing.Color.DarkGoldenrod;
            this.btnAddToList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddToList.Font = new System.Drawing.Font("黑体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAddToList.Location = new System.Drawing.Point(3, 3);
            this.btnAddToList.Name = "btnAddToList";
            this.btnAddToList.Size = new System.Drawing.Size(245, 31);
            this.btnAddToList.TabIndex = 10;
            this.btnAddToList.Text = "添加至列表";
            this.btnAddToList.UseVisualStyleBackColor = false;
            // 
            // btnSendData
            // 
            this.btnSendData.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnSendData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSendData.Font = new System.Drawing.Font("黑体", 15F, System.Drawing.FontStyle.Bold);
            this.btnSendData.Location = new System.Drawing.Point(254, 3);
            this.btnSendData.Name = "btnSendData";
            this.btnSendData.Size = new System.Drawing.Size(246, 31);
            this.btnSendData.TabIndex = 11;
            this.btnSendData.Text = "立即发送";
            this.btnSendData.UseVisualStyleBackColor = false;
            this.btnSendData.Click += new System.EventHandler(this.btnSendData_Click);
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel6.Controls.Add(this.rbSendSlaveLin, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.rbSendMasterLin, 0, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(512, 85);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(503, 35);
            this.tableLayoutPanel6.TabIndex = 9;
            // 
            // rbSendSlaveLin
            // 
            this.rbSendSlaveLin.Checked = false;
            this.rbSendSlaveLin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbSendSlaveLin.GroupName = null;
            this.rbSendSlaveLin.Location = new System.Drawing.Point(254, 3);
            this.rbSendSlaveLin.Name = "rbSendSlaveLin";
            this.rbSendSlaveLin.Size = new System.Drawing.Size(246, 29);
            this.rbSendSlaveLin.TabIndex = 9;
            this.rbSendSlaveLin.TextValue = "发送命令帧与响应帧";
            // 
            // rbSendMasterLin
            // 
            this.rbSendMasterLin.Checked = true;
            this.rbSendMasterLin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbSendMasterLin.GroupName = null;
            this.rbSendMasterLin.Location = new System.Drawing.Point(3, 3);
            this.rbSendMasterLin.Name = "rbSendMasterLin";
            this.rbSendMasterLin.Size = new System.Drawing.Size(245, 29);
            this.rbSendMasterLin.TabIndex = 8;
            this.rbSendMasterLin.TextValue = "仅发送命令帧";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel3);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 199);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1028, 452);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "列表发送";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.userDataGrid1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 17);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1022, 432);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // userDataGrid1
            // 
            this.userDataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userDataGrid1.Location = new System.Drawing.Point(3, 3);
            this.userDataGrid1.Name = "userDataGrid1";
            this.userDataGrid1.Size = new System.Drawing.Size(1016, 376);
            this.userDataGrid1.TabIndex = 0;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 10;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel4.Controls.Add(this.button9, 6, 0);
            this.tableLayoutPanel4.Controls.Add(this.button8, 5, 0);
            this.tableLayoutPanel4.Controls.Add(this.button7, 9, 0);
            this.tableLayoutPanel4.Controls.Add(this.button6, 7, 0);
            this.tableLayoutPanel4.Controls.Add(this.button5, 4, 0);
            this.tableLayoutPanel4.Controls.Add(this.button4, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.button3, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.button2, 3, 0);
            this.tableLayoutPanel4.Controls.Add(this.button1, 2, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 385);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(1016, 44);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // button9
            // 
            this.button9.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.button9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button9.Font = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button9.Location = new System.Drawing.Point(609, 3);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(95, 38);
            this.button9.TabIndex = 18;
            this.button9.Text = "导入";
            this.button9.UseVisualStyleBackColor = false;
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.button8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button8.Font = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button8.Location = new System.Drawing.Point(508, 3);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(95, 38);
            this.button8.TabIndex = 17;
            this.button8.Text = "清空";
            this.button8.UseVisualStyleBackColor = false;
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.button7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button7.Font = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button7.Location = new System.Drawing.Point(912, 3);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(101, 38);
            this.button7.TabIndex = 20;
            this.button7.Text = "列表发送";
            this.button7.UseVisualStyleBackColor = false;
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.button6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button6.Font = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button6.Location = new System.Drawing.Point(710, 3);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(95, 38);
            this.button6.TabIndex = 19;
            this.button6.Text = "导出";
            this.button6.UseVisualStyleBackColor = false;
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.button5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button5.Font = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button5.Location = new System.Drawing.Point(407, 3);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(95, 38);
            this.button5.TabIndex = 16;
            this.button5.Text = "删除";
            this.button5.UseVisualStyleBackColor = false;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.button4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button4.Font = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button4.Location = new System.Drawing.Point(104, 3);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(95, 38);
            this.button4.TabIndex = 13;
            this.button4.Text = "反选";
            this.button4.UseVisualStyleBackColor = false;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.button3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button3.Font = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button3.Location = new System.Drawing.Point(3, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(95, 38);
            this.button3.TabIndex = 12;
            this.button3.Text = "全选";
            this.button3.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.button2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button2.Font = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.Location = new System.Drawing.Point(306, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(95, 38);
            this.button2.TabIndex = 15;
            this.button2.Text = "下移";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.Font = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(205, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(95, 38);
            this.button1.TabIndex = 14;
            this.button1.Text = "上移";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 2;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.Controls.Add(this.txtSendTimes, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.txtSendInterval, 1, 0);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(509, 41);
            this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(509, 41);
            this.tableLayoutPanel7.TabIndex = 11;
            // 
            // txtSendInterval
            // 
            this.txtSendInterval.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSendInterval.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSendInterval.LabelString = "发送间隔：";
            this.txtSendInterval.Location = new System.Drawing.Point(256, 2);
            this.txtSendInterval.Margin = new System.Windows.Forms.Padding(2);
            this.txtSendInterval.Name = "txtSendInterval";
            this.txtSendInterval.Size = new System.Drawing.Size(251, 37);
            this.txtSendInterval.TabIndex = 7;
            // 
            // txtSendTimes
            // 
            this.txtSendTimes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSendTimes.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSendTimes.LabelString = "发送次数：";
            this.txtSendTimes.Location = new System.Drawing.Point(2, 2);
            this.txtSendTimes.Margin = new System.Windows.Forms.Padding(2);
            this.txtSendTimes.Name = "txtSendTimes";
            this.txtSendTimes.Size = new System.Drawing.Size(250, 37);
            this.txtSendTimes.TabIndex = 6;
            // 
            // cmbMasterSlaveDelayMs
            // 
            this.cmbMasterSlaveDelayMs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbMasterSlaveDelayMs.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.cmbMasterSlaveDelayMs.LabelString = "命令帧与响应帧的帧间隔:";
            this.cmbMasterSlaveDelayMs.Location = new System.Drawing.Point(509, 0);
            this.cmbMasterSlaveDelayMs.Margin = new System.Windows.Forms.Padding(0);
            this.cmbMasterSlaveDelayMs.Name = "cmbMasterSlaveDelayMs";
            this.cmbMasterSlaveDelayMs.Size = new System.Drawing.Size(509, 41);
            this.cmbMasterSlaveDelayMs.TabIndex = 5;
            // 
            // LinDataNormalSendForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1034, 654);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MinimumSize = new System.Drawing.Size(600, 600);
            this.Name = "LinDataNormalSendForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LIN普通发送";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private UserControls.LabelText txtLinData;
        private UserControls.LabelText txtFrameName;
        private UserControls.LabelText txtMasterLinId;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private UserControls.UserDataGrid userDataGrid1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Button btnAddToList;
        private System.Windows.Forms.Button btnSendData;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private UCRadioButton rbSendSlaveLin;
        private UCRadioButton rbSendMasterLin;
        private UserControls.LabelText txtSlaveLinId;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private UserControls.LabelText txtSendTimes;
        private UserControls.LabelText txtSendInterval;
        private UserControls.LabelCombox cmbMasterSlaveDelayMs;

    }
}