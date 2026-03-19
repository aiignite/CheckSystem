using System.Drawing;

namespace CheckSystem.RobotForms
{
    partial class RobotControllerMainForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.mainRtexPanel = new System.Windows.Forms.TableLayoutPanel();
            this.writeInfoPanel = new System.Windows.Forms.TableLayoutPanel();
            this.writeDataGrid = new UserControls.UserDataGrid();
            this.writeAxisNo = new UserControls.LabelCombox();
            this.writeTypeCmb = new UserControls.LabelCombox();
            this.btnWrite = new System.Windows.Forms.Button();
            this.readInfoPanel = new System.Windows.Forms.TableLayoutPanel();
            this.readDataGrid = new UserControls.UserDataGrid();
            this.lblAxisSelect = new UserControls.LabelCombox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.eventDataGrid = new UserControls.UserDataGrid();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabControlTeach = new System.Windows.Forms.TabControl();
            this.tabPageProgram = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dataGridViewProgramList = new System.Windows.Forms.DataGridView();
            this.index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProgramName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Note = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridViewSelectedProgram = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button2 = new System.Windows.Forms.Button();
            this.toolStripProgram = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonProgramAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonProgramDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonOpenToDiDo = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonOpenPalletMgr = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonOpenToTeach = new System.Windows.Forms.ToolStripButton();
            this.tabPageTeach = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.dataGridViewProgramJog = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button3 = new System.Windows.Forms.Button();
            this.labelTextJogProgramName = new UserControls.LabelText();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.eventTeachDataGrid = new UserControls.UserDataGrid();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnMgrPallet = new HZH_Controls.Controls.Btn.UCBtnExt();
            this.btnServo = new HZH_Controls.Controls.Btn.UCBtnExt();
            this.btnAddPallet = new HZH_Controls.Controls.Btn.UCBtnExt();
            this.btnUpdatePallet = new HZH_Controls.Controls.Btn.UCBtnExt();
            this.ucBtnIo = new HZH_Controls.Controls.Btn.UCBtnExt();
            this.btnInsert = new HZH_Controls.Controls.Btn.UCBtnExt();
            this.btnUpdate = new HZH_Controls.Controls.Btn.UCBtnExt();
            this.btnAddLast = new HZH_Controls.Controls.Btn.UCBtnExt();
            this.jogTeachDataGrid = new UserControls.UserDataGrid();
            this.toolStripJoging = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonDeleteRow = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonUpdate = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSkip = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonNextRow = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonPreRow = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonMoveSelectedBlockLine = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonRunProgram = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonPauseProgram = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.lblSpeedSelect = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBoxSpeedPercent = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSaveProgram = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonExpot = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonImport = new System.Windows.Forms.ToolStripButton();
            this.mainTabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.mainRtexPanel.SuspendLayout();
            this.writeInfoPanel.SuspendLayout();
            this.readInfoPanel.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabControlTeach.SuspendLayout();
            this.tabPageProgram.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProgramList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSelectedProgram)).BeginInit();
            this.toolStripProgram.SuspendLayout();
            this.tabPageTeach.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProgramJog)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.toolStripJoging.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTabControl
            // 
            this.mainTabControl.Controls.Add(this.tabPage1);
            this.mainTabControl.Controls.Add(this.tabPage2);
            this.mainTabControl.Controls.Add(this.tabPage3);
            this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabControl.Location = new System.Drawing.Point(0, 0);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(1270, 691);
            this.mainTabControl.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.mainRtexPanel);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1262, 665);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "主界面";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // mainRtexPanel
            // 
            this.mainRtexPanel.ColumnCount = 2;
            this.mainRtexPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainRtexPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainRtexPanel.Controls.Add(this.writeInfoPanel, 0, 0);
            this.mainRtexPanel.Controls.Add(this.readInfoPanel, 0, 0);
            this.mainRtexPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainRtexPanel.Location = new System.Drawing.Point(3, 3);
            this.mainRtexPanel.Name = "mainRtexPanel";
            this.mainRtexPanel.RowCount = 1;
            this.mainRtexPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainRtexPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 659F));
            this.mainRtexPanel.Size = new System.Drawing.Size(1256, 659);
            this.mainRtexPanel.TabIndex = 0;
            // 
            // writeInfoPanel
            // 
            this.writeInfoPanel.BackColor = System.Drawing.Color.Beige;
            this.writeInfoPanel.ColumnCount = 2;
            this.writeInfoPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.writeInfoPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.writeInfoPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.writeInfoPanel.Controls.Add(this.writeDataGrid, 0, 2);
            this.writeInfoPanel.Controls.Add(this.writeAxisNo, 1, 0);
            this.writeInfoPanel.Controls.Add(this.writeTypeCmb, 0, 0);
            this.writeInfoPanel.Controls.Add(this.btnWrite, 0, 1);
            this.writeInfoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.writeInfoPanel.Location = new System.Drawing.Point(631, 3);
            this.writeInfoPanel.Name = "writeInfoPanel";
            this.writeInfoPanel.RowCount = 3;
            this.writeInfoPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.writeInfoPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.writeInfoPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.writeInfoPanel.Size = new System.Drawing.Size(622, 653);
            this.writeInfoPanel.TabIndex = 4;
            // 
            // writeDataGrid
            // 
            this.writeInfoPanel.SetColumnSpan(this.writeDataGrid, 2);
            this.writeDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.writeDataGrid.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Bold);
            this.writeDataGrid.Location = new System.Drawing.Point(4, 104);
            this.writeDataGrid.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.writeDataGrid.Name = "writeDataGrid";
            this.writeDataGrid.Size = new System.Drawing.Size(614, 549);
            this.writeDataGrid.TabIndex = 4;
            // 
            // writeAxisNo
            // 
            this.writeAxisNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.writeAxisNo.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.writeAxisNo.LabelString = "轴号：";
            this.writeAxisNo.Location = new System.Drawing.Point(312, 1);
            this.writeAxisNo.Margin = new System.Windows.Forms.Padding(1);
            this.writeAxisNo.Name = "writeAxisNo";
            this.writeAxisNo.Size = new System.Drawing.Size(309, 48);
            this.writeAxisNo.TabIndex = 2;
            // 
            // writeTypeCmb
            // 
            this.writeTypeCmb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.writeTypeCmb.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.writeTypeCmb.LabelString = "写入类型：";
            this.writeTypeCmb.Location = new System.Drawing.Point(1, 1);
            this.writeTypeCmb.Margin = new System.Windows.Forms.Padding(1);
            this.writeTypeCmb.Name = "writeTypeCmb";
            this.writeTypeCmb.Size = new System.Drawing.Size(309, 48);
            this.writeTypeCmb.TabIndex = 1;
            // 
            // btnWrite
            // 
            this.writeInfoPanel.SetColumnSpan(this.btnWrite, 2);
            this.btnWrite.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnWrite.Location = new System.Drawing.Point(3, 53);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(616, 44);
            this.btnWrite.TabIndex = 3;
            this.btnWrite.Text = "写入一次";
            this.btnWrite.UseVisualStyleBackColor = true;
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
            // 
            // readInfoPanel
            // 
            this.readInfoPanel.BackColor = System.Drawing.Color.AntiqueWhite;
            this.readInfoPanel.ColumnCount = 1;
            this.readInfoPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.readInfoPanel.Controls.Add(this.readDataGrid, 0, 1);
            this.readInfoPanel.Controls.Add(this.lblAxisSelect, 0, 0);
            this.readInfoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.readInfoPanel.Location = new System.Drawing.Point(3, 3);
            this.readInfoPanel.Name = "readInfoPanel";
            this.readInfoPanel.RowCount = 2;
            this.readInfoPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.readInfoPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.readInfoPanel.Size = new System.Drawing.Size(622, 653);
            this.readInfoPanel.TabIndex = 3;
            // 
            // readDataGrid
            // 
            this.readInfoPanel.SetColumnSpan(this.readDataGrid, 2);
            this.readDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.readDataGrid.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Bold);
            this.readDataGrid.ForeColor = System.Drawing.Color.Black;
            this.readDataGrid.Location = new System.Drawing.Point(4, 54);
            this.readDataGrid.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.readDataGrid.Name = "readDataGrid";
            this.readDataGrid.Size = new System.Drawing.Size(614, 595);
            this.readDataGrid.TabIndex = 4;
            // 
            // lblAxisSelect
            // 
            this.lblAxisSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAxisSelect.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblAxisSelect.LabelString = "label";
            this.lblAxisSelect.Location = new System.Drawing.Point(0, 0);
            this.lblAxisSelect.Margin = new System.Windows.Forms.Padding(0);
            this.lblAxisSelect.Name = "lblAxisSelect";
            this.lblAxisSelect.Size = new System.Drawing.Size(622, 50);
            this.lblAxisSelect.TabIndex = 5;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.eventDataGrid);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1262, 665);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "写轴事件";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // eventDataGrid
            // 
            this.eventDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eventDataGrid.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.eventDataGrid.Location = new System.Drawing.Point(3, 3);
            this.eventDataGrid.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.eventDataGrid.Name = "eventDataGrid";
            this.eventDataGrid.Size = new System.Drawing.Size(1256, 659);
            this.eventDataGrid.TabIndex = 5;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.tabControlTeach);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1262, 665);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "示教界面";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabControlTeach
            // 
            this.tabControlTeach.Controls.Add(this.tabPageProgram);
            this.tabControlTeach.Controls.Add(this.tabPageTeach);
            this.tabControlTeach.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlTeach.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tabControlTeach.Location = new System.Drawing.Point(3, 3);
            this.tabControlTeach.Name = "tabControlTeach";
            this.tabControlTeach.SelectedIndex = 0;
            this.tabControlTeach.Size = new System.Drawing.Size(1256, 659);
            this.tabControlTeach.TabIndex = 3;
            // 
            // tabPageProgram
            // 
            this.tabPageProgram.Controls.Add(this.splitContainer1);
            this.tabPageProgram.Controls.Add(this.toolStripProgram);
            this.tabPageProgram.Location = new System.Drawing.Point(4, 22);
            this.tabPageProgram.Name = "tabPageProgram";
            this.tabPageProgram.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProgram.Size = new System.Drawing.Size(1248, 633);
            this.tabPageProgram.TabIndex = 0;
            this.tabPageProgram.Text = "程序管理";
            this.tabPageProgram.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 42);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridViewProgramList);
            this.splitContainer1.Panel1.Controls.Add(this.button1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridViewSelectedProgram);
            this.splitContainer1.Panel2.Controls.Add(this.button2);
            this.splitContainer1.Size = new System.Drawing.Size(1242, 588);
            this.splitContainer1.SplitterDistance = 617;
            this.splitContainer1.TabIndex = 3;
            // 
            // dataGridViewProgramList
            // 
            this.dataGridViewProgramList.AllowUserToAddRows = false;
            this.dataGridViewProgramList.AllowUserToDeleteRows = false;
            this.dataGridViewProgramList.AllowUserToResizeRows = false;
            this.dataGridViewProgramList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewProgramList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewProgramList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.index,
            this.ProgramName,
            this.Note});
            this.dataGridViewProgramList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewProgramList.Location = new System.Drawing.Point(0, 37);
            this.dataGridViewProgramList.MultiSelect = false;
            this.dataGridViewProgramList.Name = "dataGridViewProgramList";
            this.dataGridViewProgramList.RowHeadersVisible = false;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dataGridViewProgramList.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewProgramList.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dataGridViewProgramList.RowTemplate.Height = 30;
            this.dataGridViewProgramList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewProgramList.Size = new System.Drawing.Size(617, 551);
            this.dataGridViewProgramList.TabIndex = 0;
            // 
            // index
            // 
            this.index.FillWeight = 10F;
            this.index.HeaderText = "序号";
            this.index.Name = "index";
            this.index.ReadOnly = true;
            // 
            // ProgramName
            // 
            this.ProgramName.FillWeight = 45F;
            this.ProgramName.HeaderText = "程序名";
            this.ProgramName.Name = "ProgramName";
            this.ProgramName.ReadOnly = true;
            // 
            // Note
            // 
            this.Note.FillWeight = 45F;
            this.Note.HeaderText = "注释";
            this.Note.Name = "Note";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.LightSalmon;
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.Enabled = false;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(617, 37);
            this.button1.TabIndex = 0;
            this.button1.Text = "程序清单";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // dataGridViewSelectedProgram
            // 
            this.dataGridViewSelectedProgram.AllowUserToAddRows = false;
            this.dataGridViewSelectedProgram.AllowUserToDeleteRows = false;
            this.dataGridViewSelectedProgram.AllowUserToResizeRows = false;
            this.dataGridViewSelectedProgram.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewSelectedProgram.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSelectedProgram.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.dataGridViewSelectedProgram.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewSelectedProgram.Location = new System.Drawing.Point(0, 37);
            this.dataGridViewSelectedProgram.Name = "dataGridViewSelectedProgram";
            this.dataGridViewSelectedProgram.RowHeadersVisible = false;
            this.dataGridViewSelectedProgram.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dataGridViewSelectedProgram.RowTemplate.Height = 30;
            this.dataGridViewSelectedProgram.Size = new System.Drawing.Size(621, 551);
            this.dataGridViewSelectedProgram.TabIndex = 2;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.FillWeight = 10F;
            this.dataGridViewTextBoxColumn1.HeaderText = "序号";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.FillWeight = 90F;
            this.dataGridViewTextBoxColumn2.HeaderText = "程序行代码";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.PeachPuff;
            this.button2.Dock = System.Windows.Forms.DockStyle.Top;
            this.button2.Enabled = false;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(0, 0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(621, 37);
            this.button2.TabIndex = 1;
            this.button2.Text = "程序内容";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // toolStripProgram
            // 
            this.toolStripProgram.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolStripProgram.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStripProgram.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonProgramAdd,
            this.toolStripSeparator8,
            this.toolStripButtonProgramDelete,
            this.toolStripSeparator11,
            this.toolStripButtonOpenToDiDo,
            this.toolStripSeparator12,
            this.toolStripButtonOpenPalletMgr,
            this.toolStripSeparator13,
            this.toolStripButtonExpot,
            this.toolStripSeparator15,
            this.toolStripButtonImport,
            this.toolStripSeparator14,
            this.toolStripButtonOpenToTeach});
            this.toolStripProgram.Location = new System.Drawing.Point(3, 3);
            this.toolStripProgram.Name = "toolStripProgram";
            this.toolStripProgram.Size = new System.Drawing.Size(1242, 39);
            this.toolStripProgram.TabIndex = 2;
            this.toolStripProgram.Text = "Program";
            // 
            // toolStripButtonProgramAdd
            // 
            this.toolStripButtonProgramAdd.Image = global::CheckSystem.Properties.Resources.TSB_AddPolygon_Image;
            this.toolStripButtonProgramAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonProgramAdd.Name = "toolStripButtonProgramAdd";
            this.toolStripButtonProgramAdd.Size = new System.Drawing.Size(110, 36);
            this.toolStripButtonProgramAdd.Text = "添加程序";
            this.toolStripButtonProgramAdd.Click += new System.EventHandler(this.toolStripProgram_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStripButtonProgramDelete
            // 
            this.toolStripButtonProgramDelete.Image = global::CheckSystem.Properties.Resources.TSB_ClearShape_Image;
            this.toolStripButtonProgramDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonProgramDelete.Name = "toolStripButtonProgramDelete";
            this.toolStripButtonProgramDelete.Size = new System.Drawing.Size(110, 36);
            this.toolStripButtonProgramDelete.Text = "删除程序";
            this.toolStripButtonProgramDelete.Click += new System.EventHandler(this.toolStripButtonProgramDelete_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStripButtonOpenToDiDo
            // 
            this.toolStripButtonOpenToDiDo.Image = global::CheckSystem.Properties.Resources.TSB_Detect_Image;
            this.toolStripButtonOpenToDiDo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOpenToDiDo.Name = "toolStripButtonOpenToDiDo";
            this.toolStripButtonOpenToDiDo.Size = new System.Drawing.Size(168, 36);
            this.toolStripButtonOpenToDiDo.Text = "打开调试DI和DO";
            this.toolStripButtonOpenToDiDo.ToolTipText = "打开调试DI和DO";
            this.toolStripButtonOpenToDiDo.Click += new System.EventHandler(this.ucBtnIo_BtnClick);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStripButtonOpenPalletMgr
            // 
            this.toolStripButtonOpenPalletMgr.Image = global::CheckSystem.Properties.Resources.Btn_GraySet_BackgroundImage;
            this.toolStripButtonOpenPalletMgr.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOpenPalletMgr.Name = "toolStripButtonOpenPalletMgr";
            this.toolStripButtonOpenPalletMgr.Size = new System.Drawing.Size(120, 36);
            this.toolStripButtonOpenPalletMgr.Text = "Pallet管理";
            this.toolStripButtonOpenPalletMgr.Click += new System.EventHandler(this.toolStripButtonOpenPalletMgr_Click);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStripButtonOpenToTeach
            // 
            this.toolStripButtonOpenToTeach.Image = global::CheckSystem.Properties.Resources.TSB_Capture_Image;
            this.toolStripButtonOpenToTeach.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOpenToTeach.Name = "toolStripButtonOpenToTeach";
            this.toolStripButtonOpenToTeach.Size = new System.Drawing.Size(110, 36);
            this.toolStripButtonOpenToTeach.Text = "打开示教";
            this.toolStripButtonOpenToTeach.Click += new System.EventHandler(this.toolStripButtonOpenToTeach_Click);
            // 
            // tabPageTeach
            // 
            this.tabPageTeach.BackColor = System.Drawing.Color.Transparent;
            this.tabPageTeach.Controls.Add(this.splitContainer2);
            this.tabPageTeach.Controls.Add(this.toolStripJoging);
            this.tabPageTeach.Location = new System.Drawing.Point(4, 22);
            this.tabPageTeach.Name = "tabPageTeach";
            this.tabPageTeach.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTeach.Size = new System.Drawing.Size(1248, 633);
            this.tabPageTeach.TabIndex = 1;
            this.tabPageTeach.Text = "示教程序";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 42);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.dataGridViewProgramJog);
            this.splitContainer2.Panel1.Controls.Add(this.button3);
            this.splitContainer2.Panel1.Controls.Add(this.labelTextJogProgramName);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer2.Size = new System.Drawing.Size(1242, 588);
            this.splitContainer2.SplitterDistance = 665;
            this.splitContainer2.TabIndex = 7;
            // 
            // dataGridViewProgramJog
            // 
            this.dataGridViewProgramJog.AllowUserToAddRows = false;
            this.dataGridViewProgramJog.AllowUserToDeleteRows = false;
            this.dataGridViewProgramJog.AllowUserToResizeRows = false;
            this.dataGridViewProgramJog.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewProgramJog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewProgramJog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
            this.dataGridViewProgramJog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewProgramJog.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewProgramJog.Location = new System.Drawing.Point(0, 71);
            this.dataGridViewProgramJog.MultiSelect = false;
            this.dataGridViewProgramJog.Name = "dataGridViewProgramJog";
            this.dataGridViewProgramJog.RowHeadersVisible = false;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("楷体", 5.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dataGridViewProgramJog.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewProgramJog.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dataGridViewProgramJog.RowTemplate.Height = 30;
            this.dataGridViewProgramJog.Size = new System.Drawing.Size(665, 517);
            this.dataGridViewProgramJog.TabIndex = 6;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.FillWeight = 5.076142F;
            this.dataGridViewTextBoxColumn3.HeaderText = "序号";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 10;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.FillWeight = 94.92386F;
            this.dataGridViewTextBoxColumn4.HeaderText = "程序行";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.PeachPuff;
            this.button3.Dock = System.Windows.Forms.DockStyle.Top;
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(0, 34);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(665, 37);
            this.button3.TabIndex = 5;
            this.button3.Text = "程序内容:";
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.UseVisualStyleBackColor = false;
            // 
            // labelTextJogProgramName
            // 
            this.labelTextJogProgramName.BackColor = System.Drawing.Color.LightSalmon;
            this.labelTextJogProgramName.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTextJogProgramName.Enabled = false;
            this.labelTextJogProgramName.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelTextJogProgramName.LabelString = "程序名称";
            this.labelTextJogProgramName.Location = new System.Drawing.Point(0, 0);
            this.labelTextJogProgramName.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.labelTextJogProgramName.Name = "labelTextJogProgramName";
            this.labelTextJogProgramName.Size = new System.Drawing.Size(665, 34);
            this.labelTextJogProgramName.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.eventTeachDataGrid, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.jogTeachDataGrid, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(573, 588);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // eventTeachDataGrid
            // 
            this.eventTeachDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eventTeachDataGrid.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Bold);
            this.eventTeachDataGrid.ForeColor = System.Drawing.Color.Black;
            this.eventTeachDataGrid.Location = new System.Drawing.Point(4, 421);
            this.eventTeachDataGrid.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.eventTeachDataGrid.Name = "eventTeachDataGrid";
            this.eventTeachDataGrid.Size = new System.Drawing.Size(565, 163);
            this.eventTeachDataGrid.TabIndex = 9;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Controls.Add(this.btnMgrPallet, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.btnServo, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.btnAddPallet, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.btnUpdatePallet, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.ucBtnIo, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnInsert, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnUpdate, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnAddLast, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 320);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(567, 94);
            this.tableLayoutPanel2.TabIndex = 8;
            // 
            // btnMgrPallet
            // 
            this.btnMgrPallet.BackColor = System.Drawing.Color.White;
            this.btnMgrPallet.BtnBackColor = System.Drawing.Color.White;
            this.btnMgrPallet.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnMgrPallet.BtnForeColor = System.Drawing.Color.White;
            this.btnMgrPallet.BtnText = "Pallet管理";
            this.btnMgrPallet.ConerRadius = 5;
            this.btnMgrPallet.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMgrPallet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMgrPallet.EnabledMouseEffect = false;
            this.btnMgrPallet.FillColor = System.Drawing.Color.DarkCyan;
            this.btnMgrPallet.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnMgrPallet.IsRadius = true;
            this.btnMgrPallet.IsShowRect = true;
            this.btnMgrPallet.IsShowTips = false;
            this.btnMgrPallet.Location = new System.Drawing.Point(283, 48);
            this.btnMgrPallet.Margin = new System.Windows.Forms.Padding(1);
            this.btnMgrPallet.Name = "btnMgrPallet";
            this.btnMgrPallet.RectColor = System.Drawing.Color.DarkOliveGreen;
            this.btnMgrPallet.RectWidth = 1;
            this.btnMgrPallet.Size = new System.Drawing.Size(139, 45);
            this.btnMgrPallet.TabIndex = 9;
            this.btnMgrPallet.TabStop = false;
            this.btnMgrPallet.TipsColor = System.Drawing.Color.DarkSeaGreen;
            this.btnMgrPallet.TipsText = "Pallet管理";
            this.btnMgrPallet.BtnClick += new System.EventHandler(this.btnMgrPallet_BtnClick);
            // 
            // btnServo
            // 
            this.btnServo.BackColor = System.Drawing.Color.White;
            this.btnServo.BtnBackColor = System.Drawing.Color.White;
            this.btnServo.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnServo.BtnForeColor = System.Drawing.Color.White;
            this.btnServo.BtnText = "伺服控制";
            this.btnServo.ConerRadius = 5;
            this.btnServo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnServo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnServo.EnabledMouseEffect = false;
            this.btnServo.FillColor = System.Drawing.Color.DarkCyan;
            this.btnServo.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnServo.IsRadius = true;
            this.btnServo.IsShowRect = true;
            this.btnServo.IsShowTips = false;
            this.btnServo.Location = new System.Drawing.Point(424, 48);
            this.btnServo.Margin = new System.Windows.Forms.Padding(1);
            this.btnServo.Name = "btnServo";
            this.btnServo.RectColor = System.Drawing.Color.DarkOliveGreen;
            this.btnServo.RectWidth = 1;
            this.btnServo.Size = new System.Drawing.Size(142, 45);
            this.btnServo.TabIndex = 8;
            this.btnServo.TabStop = false;
            this.btnServo.TipsColor = System.Drawing.Color.DarkSeaGreen;
            this.btnServo.TipsText = "伺服控制";
            this.btnServo.BtnClick += new System.EventHandler(this.btnServo_BtnClick);
            // 
            // btnAddPallet
            // 
            this.btnAddPallet.BackColor = System.Drawing.Color.White;
            this.btnAddPallet.BtnBackColor = System.Drawing.Color.White;
            this.btnAddPallet.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAddPallet.BtnForeColor = System.Drawing.Color.White;
            this.btnAddPallet.BtnText = "插入/添加一个Pallet";
            this.btnAddPallet.ConerRadius = 5;
            this.btnAddPallet.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddPallet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddPallet.EnabledMouseEffect = false;
            this.btnAddPallet.FillColor = System.Drawing.Color.DarkCyan;
            this.btnAddPallet.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnAddPallet.IsRadius = true;
            this.btnAddPallet.IsShowRect = true;
            this.btnAddPallet.IsShowTips = false;
            this.btnAddPallet.Location = new System.Drawing.Point(1, 48);
            this.btnAddPallet.Margin = new System.Windows.Forms.Padding(1);
            this.btnAddPallet.Name = "btnAddPallet";
            this.btnAddPallet.RectColor = System.Drawing.Color.DarkOliveGreen;
            this.btnAddPallet.RectWidth = 1;
            this.btnAddPallet.Size = new System.Drawing.Size(139, 45);
            this.btnAddPallet.TabIndex = 7;
            this.btnAddPallet.TabStop = false;
            this.btnAddPallet.TipsColor = System.Drawing.Color.DarkSeaGreen;
            this.btnAddPallet.TipsText = "插入一个Pallet";
            this.btnAddPallet.BtnClick += new System.EventHandler(this.btnAddPallet_BtnClick);
            // 
            // btnUpdatePallet
            // 
            this.btnUpdatePallet.BackColor = System.Drawing.Color.White;
            this.btnUpdatePallet.BtnBackColor = System.Drawing.Color.White;
            this.btnUpdatePallet.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUpdatePallet.BtnForeColor = System.Drawing.Color.White;
            this.btnUpdatePallet.BtnText = "更新到Pallet点位";
            this.btnUpdatePallet.ConerRadius = 5;
            this.btnUpdatePallet.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUpdatePallet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnUpdatePallet.EnabledMouseEffect = false;
            this.btnUpdatePallet.FillColor = System.Drawing.Color.DarkCyan;
            this.btnUpdatePallet.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnUpdatePallet.IsRadius = true;
            this.btnUpdatePallet.IsShowRect = true;
            this.btnUpdatePallet.IsShowTips = false;
            this.btnUpdatePallet.Location = new System.Drawing.Point(142, 48);
            this.btnUpdatePallet.Margin = new System.Windows.Forms.Padding(1);
            this.btnUpdatePallet.Name = "btnUpdatePallet";
            this.btnUpdatePallet.RectColor = System.Drawing.Color.DarkOliveGreen;
            this.btnUpdatePallet.RectWidth = 1;
            this.btnUpdatePallet.Size = new System.Drawing.Size(139, 45);
            this.btnUpdatePallet.TabIndex = 6;
            this.btnUpdatePallet.TabStop = false;
            this.btnUpdatePallet.TipsColor = System.Drawing.Color.DarkSeaGreen;
            this.btnUpdatePallet.TipsText = "更新到Pallet点位";
            this.btnUpdatePallet.BtnClick += new System.EventHandler(this.btnUpdatePallet_BtnClick);
            // 
            // ucBtnIo
            // 
            this.ucBtnIo.BackColor = System.Drawing.Color.White;
            this.ucBtnIo.BtnBackColor = System.Drawing.Color.White;
            this.ucBtnIo.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucBtnIo.BtnForeColor = System.Drawing.Color.White;
            this.ucBtnIo.BtnText = "输入输出/IO";
            this.ucBtnIo.ConerRadius = 5;
            this.ucBtnIo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucBtnIo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucBtnIo.EnabledMouseEffect = false;
            this.ucBtnIo.FillColor = System.Drawing.Color.DarkCyan;
            this.ucBtnIo.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ucBtnIo.IsRadius = true;
            this.ucBtnIo.IsShowRect = true;
            this.ucBtnIo.IsShowTips = false;
            this.ucBtnIo.Location = new System.Drawing.Point(424, 1);
            this.ucBtnIo.Margin = new System.Windows.Forms.Padding(1);
            this.ucBtnIo.Name = "ucBtnIo";
            this.ucBtnIo.RectColor = System.Drawing.Color.DarkOliveGreen;
            this.ucBtnIo.RectWidth = 1;
            this.ucBtnIo.Size = new System.Drawing.Size(142, 45);
            this.ucBtnIo.TabIndex = 5;
            this.ucBtnIo.TabStop = false;
            this.ucBtnIo.TipsColor = System.Drawing.Color.DarkSeaGreen;
            this.ucBtnIo.TipsText = "输入输出/IO";
            this.ucBtnIo.BtnClick += new System.EventHandler(this.ucBtnIo_BtnClick);
            // 
            // btnInsert
            // 
            this.btnInsert.BackColor = System.Drawing.Color.White;
            this.btnInsert.BtnBackColor = System.Drawing.Color.White;
            this.btnInsert.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnInsert.BtnForeColor = System.Drawing.Color.White;
            this.btnInsert.BtnText = "插入到上一行";
            this.btnInsert.ConerRadius = 5;
            this.btnInsert.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnInsert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnInsert.EnabledMouseEffect = false;
            this.btnInsert.FillColor = System.Drawing.Color.DarkCyan;
            this.btnInsert.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnInsert.IsRadius = true;
            this.btnInsert.IsShowRect = true;
            this.btnInsert.IsShowTips = false;
            this.btnInsert.Location = new System.Drawing.Point(142, 1);
            this.btnInsert.Margin = new System.Windows.Forms.Padding(1);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.RectColor = System.Drawing.Color.DarkOliveGreen;
            this.btnInsert.RectWidth = 1;
            this.btnInsert.Size = new System.Drawing.Size(139, 45);
            this.btnInsert.TabIndex = 4;
            this.btnInsert.TabStop = false;
            this.btnInsert.TipsColor = System.Drawing.Color.DarkSeaGreen;
            this.btnInsert.TipsText = "插入到上一行";
            this.btnInsert.BtnClick += new System.EventHandler(this.btnInsert_BtnClick);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.White;
            this.btnUpdate.BtnBackColor = System.Drawing.Color.White;
            this.btnUpdate.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUpdate.BtnForeColor = System.Drawing.Color.White;
            this.btnUpdate.BtnText = "更新当前行";
            this.btnUpdate.ConerRadius = 5;
            this.btnUpdate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnUpdate.EnabledMouseEffect = false;
            this.btnUpdate.FillColor = System.Drawing.Color.DarkCyan;
            this.btnUpdate.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnUpdate.IsRadius = true;
            this.btnUpdate.IsShowRect = true;
            this.btnUpdate.IsShowTips = false;
            this.btnUpdate.Location = new System.Drawing.Point(283, 1);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(1);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.RectColor = System.Drawing.Color.DarkOliveGreen;
            this.btnUpdate.RectWidth = 1;
            this.btnUpdate.Size = new System.Drawing.Size(139, 45);
            this.btnUpdate.TabIndex = 3;
            this.btnUpdate.TabStop = false;
            this.btnUpdate.TipsColor = System.Drawing.Color.DarkSeaGreen;
            this.btnUpdate.TipsText = "更新当前行";
            this.btnUpdate.BtnClick += new System.EventHandler(this.btnUpdate_BtnClick);
            // 
            // btnAddLast
            // 
            this.btnAddLast.BackColor = System.Drawing.Color.White;
            this.btnAddLast.BtnBackColor = System.Drawing.Color.White;
            this.btnAddLast.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAddLast.BtnForeColor = System.Drawing.Color.White;
            this.btnAddLast.BtnText = "添加到末尾";
            this.btnAddLast.ConerRadius = 5;
            this.btnAddLast.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddLast.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddLast.EnabledMouseEffect = false;
            this.btnAddLast.FillColor = System.Drawing.Color.DarkCyan;
            this.btnAddLast.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnAddLast.IsRadius = true;
            this.btnAddLast.IsShowRect = true;
            this.btnAddLast.IsShowTips = false;
            this.btnAddLast.Location = new System.Drawing.Point(1, 1);
            this.btnAddLast.Margin = new System.Windows.Forms.Padding(1);
            this.btnAddLast.Name = "btnAddLast";
            this.btnAddLast.RectColor = System.Drawing.Color.DarkOliveGreen;
            this.btnAddLast.RectWidth = 1;
            this.btnAddLast.Size = new System.Drawing.Size(139, 45);
            this.btnAddLast.TabIndex = 2;
            this.btnAddLast.TabStop = false;
            this.btnAddLast.TipsColor = System.Drawing.Color.DarkSeaGreen;
            this.btnAddLast.TipsText = "添加到末尾";
            this.btnAddLast.BtnClick += new System.EventHandler(this.btnAddLast_BtnClick);
            // 
            // jogTeachDataGrid
            // 
            this.jogTeachDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.jogTeachDataGrid.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.jogTeachDataGrid.ForeColor = System.Drawing.Color.Black;
            this.jogTeachDataGrid.Location = new System.Drawing.Point(4, 4);
            this.jogTeachDataGrid.Margin = new System.Windows.Forms.Padding(4);
            this.jogTeachDataGrid.Name = "jogTeachDataGrid";
            this.jogTeachDataGrid.Size = new System.Drawing.Size(565, 309);
            this.jogTeachDataGrid.TabIndex = 5;
            // 
            // toolStripJoging
            // 
            this.toolStripJoging.BackColor = System.Drawing.Color.Linen;
            this.toolStripJoging.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.toolStripJoging.GripMargin = new System.Windows.Forms.Padding(1);
            this.toolStripJoging.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStripJoging.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonDeleteRow,
            this.toolStripSeparator9,
            this.toolStripButtonUpdate,
            this.toolStripSeparator5,
            this.toolStripButtonSkip,
            this.toolStripSeparator6,
            this.toolStripButtonNextRow,
            this.toolStripSeparator7,
            this.toolStripButtonPreRow,
            this.toolStripSeparator1,
            this.toolStripButtonMoveSelectedBlockLine,
            this.toolStripSeparator10,
            this.toolStripButtonRunProgram,
            this.toolStripSeparator2,
            this.toolStripButtonPauseProgram,
            this.toolStripSeparator4,
            this.lblSpeedSelect,
            this.toolStripComboBoxSpeedPercent,
            this.toolStripSeparator3,
            this.toolStripButtonSaveProgram});
            this.toolStripJoging.Location = new System.Drawing.Point(3, 3);
            this.toolStripJoging.Name = "toolStripJoging";
            this.toolStripJoging.Size = new System.Drawing.Size(1242, 39);
            this.toolStripJoging.TabIndex = 5;
            this.toolStripJoging.Text = "Joging";
            // 
            // toolStripButtonDeleteRow
            // 
            this.toolStripButtonDeleteRow.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.toolStripButtonDeleteRow.Image = global::CheckSystem.Properties.Resources.TSB_ClearShape_Image;
            this.toolStripButtonDeleteRow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDeleteRow.Name = "toolStripButtonDeleteRow";
            this.toolStripButtonDeleteRow.Size = new System.Drawing.Size(99, 36);
            this.toolStripButtonDeleteRow.Text = "删除当前行";
            this.toolStripButtonDeleteRow.Click += new System.EventHandler(this.toolStripButtonDeleteRow_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStripButtonUpdate
            // 
            this.toolStripButtonUpdate.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.toolStripButtonUpdate.Image = global::CheckSystem.Properties.Resources.TSB_Detect_Image;
            this.toolStripButtonUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonUpdate.Name = "toolStripButtonUpdate";
            this.toolStripButtonUpdate.Size = new System.Drawing.Size(99, 36);
            this.toolStripButtonUpdate.Text = "更新当前行";
            this.toolStripButtonUpdate.Click += new System.EventHandler(this.toolStripButtonUpdate_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStripButtonSkip
            // 
            this.toolStripButtonSkip.Enabled = false;
            this.toolStripButtonSkip.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.toolStripButtonSkip.Image = global::CheckSystem.Properties.Resources.TSB_DeleteShape_Image;
            this.toolStripButtonSkip.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSkip.Name = "toolStripButtonSkip";
            this.toolStripButtonSkip.Size = new System.Drawing.Size(99, 36);
            this.toolStripButtonSkip.Text = "跳过当前行";
            this.toolStripButtonSkip.Click += new System.EventHandler(this.toolStripButtonSkip_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStripButtonNextRow
            // 
            this.toolStripButtonNextRow.Enabled = false;
            this.toolStripButtonNextRow.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.toolStripButtonNextRow.Image = global::CheckSystem.Properties.Resources.TSB_ImageFit_Image;
            this.toolStripButtonNextRow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNextRow.Name = "toolStripButtonNextRow";
            this.toolStripButtonNextRow.Size = new System.Drawing.Size(110, 36);
            this.toolStripButtonNextRow.Text = "运行到下一行";
            this.toolStripButtonNextRow.Click += new System.EventHandler(this.toolStripButtonNextRow_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStripButtonPreRow
            // 
            this.toolStripButtonPreRow.Enabled = false;
            this.toolStripButtonPreRow.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.toolStripButtonPreRow.Image = global::CheckSystem.Properties.Resources.TSB_AddRectangle_Image;
            this.toolStripButtonPreRow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPreRow.Name = "toolStripButtonPreRow";
            this.toolStripButtonPreRow.Size = new System.Drawing.Size(110, 36);
            this.toolStripButtonPreRow.Text = "运行到上一行";
            this.toolStripButtonPreRow.Click += new System.EventHandler(this.toolStripButtonPreRow_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStripButtonMoveSelectedBlockLine
            // 
            this.toolStripButtonMoveSelectedBlockLine.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.toolStripButtonMoveSelectedBlockLine.Image = global::CheckSystem.Properties.Resources.Btn_GraySet_BackgroundImage;
            this.toolStripButtonMoveSelectedBlockLine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMoveSelectedBlockLine.Name = "toolStripButtonMoveSelectedBlockLine";
            this.toolStripButtonMoveSelectedBlockLine.Size = new System.Drawing.Size(110, 36);
            this.toolStripButtonMoveSelectedBlockLine.Text = "运行到指定行";
            this.toolStripButtonMoveSelectedBlockLine.Click += new System.EventHandler(this.toolStripButtonMoveSelectedBlockLine_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStripButtonRunProgram
            // 
            this.toolStripButtonRunProgram.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.toolStripButtonRunProgram.Image = global::CheckSystem.Properties.Resources.TSB_AddPolygon_Image;
            this.toolStripButtonRunProgram.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRunProgram.Name = "toolStripButtonRunProgram";
            this.toolStripButtonRunProgram.Size = new System.Drawing.Size(88, 36);
            this.toolStripButtonRunProgram.Text = "运行程序";
            this.toolStripButtonRunProgram.Click += new System.EventHandler(this.toolStripButtonRunProgram_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStripButtonPauseProgram
            // 
            this.toolStripButtonPauseProgram.Enabled = false;
            this.toolStripButtonPauseProgram.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.toolStripButtonPauseProgram.Image = global::CheckSystem.Properties.Resources.TSB_DeleteShape_Image;
            this.toolStripButtonPauseProgram.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPauseProgram.Name = "toolStripButtonPauseProgram";
            this.toolStripButtonPauseProgram.Size = new System.Drawing.Size(66, 36);
            this.toolStripButtonPauseProgram.Text = "暂停";
            this.toolStripButtonPauseProgram.Click += new System.EventHandler(this.toolStripButtonPauseProgram_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 39);
            // 
            // lblSpeedSelect
            // 
            this.lblSpeedSelect.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.lblSpeedSelect.Image = global::CheckSystem.Properties.Resources.TSB_Capture_Image;
            this.lblSpeedSelect.Name = "lblSpeedSelect";
            this.lblSpeedSelect.Size = new System.Drawing.Size(95, 36);
            this.lblSpeedSelect.Text = "速度选择：";
            // 
            // toolStripComboBoxSpeedPercent
            // 
            this.toolStripComboBoxSpeedPercent.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.toolStripComboBoxSpeedPercent.Name = "toolStripComboBoxSpeedPercent";
            this.toolStripComboBoxSpeedPercent.Size = new System.Drawing.Size(121, 39);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStripButtonSaveProgram
            // 
            this.toolStripButtonSaveProgram.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            this.toolStripButtonSaveProgram.Image = global::CheckSystem.Properties.Resources.TSB_Save_Image;
            this.toolStripButtonSaveProgram.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSaveProgram.Name = "toolStripButtonSaveProgram";
            this.toolStripButtonSaveProgram.Size = new System.Drawing.Size(110, 36);
            this.toolStripButtonSaveProgram.Text = "保存当前程序";
            this.toolStripButtonSaveProgram.Click += new System.EventHandler(this.toolStripButtonSaveProgram_Click);
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            this.toolStripSeparator14.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStripButtonExpot
            // 
            this.toolStripButtonExpot.Image = global::CheckSystem.Properties.Resources.Green;
            this.toolStripButtonExpot.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonExpot.Name = "toolStripButtonExpot";
            this.toolStripButtonExpot.Size = new System.Drawing.Size(142, 36);
            this.toolStripButtonExpot.Text = "导出示教文件";
            this.toolStripButtonExpot.Click += new System.EventHandler(this.toolStripButtonExpot_Click);
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            this.toolStripSeparator15.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStripButtonImport
            // 
            this.toolStripButtonImport.Image = global::CheckSystem.Properties.Resources.Red;
            this.toolStripButtonImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonImport.Name = "toolStripButtonImport";
            this.toolStripButtonImport.Size = new System.Drawing.Size(126, 36);
            this.toolStripButtonImport.Text = "从文件导入";
            this.toolStripButtonImport.Click += new System.EventHandler(this.toolStripButtonImport_Click);
            // 
            // RobotControllerMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1270, 691);
            this.Controls.Add(this.mainTabControl);
            this.Name = "RobotControllerMainForm";
            this.Text = "机器人控制器调试工具";
            this.mainTabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.mainRtexPanel.ResumeLayout(false);
            this.writeInfoPanel.ResumeLayout(false);
            this.readInfoPanel.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabControlTeach.ResumeLayout(false);
            this.tabPageProgram.ResumeLayout(false);
            this.tabPageProgram.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProgramList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSelectedProgram)).EndInit();
            this.toolStripProgram.ResumeLayout(false);
            this.toolStripProgram.PerformLayout();
            this.tabPageTeach.ResumeLayout(false);
            this.tabPageTeach.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProgramJog)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.toolStripJoging.ResumeLayout(false);
            this.toolStripJoging.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TableLayoutPanel mainRtexPanel;
        private System.Windows.Forms.TableLayoutPanel writeInfoPanel;
        private UserControls.UserDataGrid writeDataGrid;
        private UserControls.LabelCombox writeAxisNo;
        private UserControls.LabelCombox writeTypeCmb;
        private System.Windows.Forms.Button btnWrite;
        private System.Windows.Forms.TableLayoutPanel readInfoPanel;
        private System.Windows.Forms.TabPage tabPage2;
        private UserControls.UserDataGrid eventDataGrid;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabControl tabControlTeach;
        private System.Windows.Forms.TabPage tabPageProgram;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dataGridViewProgramList;
        private System.Windows.Forms.DataGridViewTextBoxColumn index;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProgramName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Note;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridViewSelectedProgram;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ToolStrip toolStripProgram;
        private System.Windows.Forms.ToolStripButton toolStripButtonProgramAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonProgramDelete;
        private System.Windows.Forms.ToolStripButton toolStripButtonOpenToTeach;
        private System.Windows.Forms.TabPage tabPageTeach;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.DataGridView dataGridViewProgramJog;
        private System.Windows.Forms.Button button3;
        private UserControls.LabelText labelTextJogProgramName;
        private System.Windows.Forms.ToolStrip toolStripJoging;
        private System.Windows.Forms.ToolStripButton toolStripButtonDeleteRow;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripButton toolStripButtonNextRow;
        private System.Windows.Forms.ToolStripButton toolStripButtonPreRow;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripButton toolStripButtonMoveSelectedBlockLine;
        private System.Windows.Forms.ToolStripButton toolStripButtonRunProgram;
        private System.Windows.Forms.ToolStripButton toolStripButtonPauseProgram;
        private System.Windows.Forms.ToolStripLabel lblSpeedSelect;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxSpeedPercent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private UserControls.UserDataGrid jogTeachDataGrid;
        private UserControls.UserDataGrid eventTeachDataGrid;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private HZH_Controls.Controls.Btn.UCBtnExt btnInsert;
        private HZH_Controls.Controls.Btn.UCBtnExt btnUpdate;
        private HZH_Controls.Controls.Btn.UCBtnExt btnAddLast;
        private HZH_Controls.Controls.Btn.UCBtnExt ucBtnIo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolStripButtonOpenToDiDo;
        private HZH_Controls.Controls.Btn.UCBtnExt btnAddPallet;
        private HZH_Controls.Controls.Btn.UCBtnExt btnUpdatePallet;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.ToolStripButton toolStripButtonSaveProgram;
        private System.Windows.Forms.ToolStripButton toolStripButtonUpdate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private HZH_Controls.Controls.Btn.UCBtnExt btnServo;
        private UserControls.UserDataGrid readDataGrid;
        private UserControls.LabelCombox lblAxisSelect;
        private System.Windows.Forms.ToolStripButton toolStripButtonOpenPalletMgr;
        private System.Windows.Forms.ToolStripButton toolStripButtonSkip;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private HZH_Controls.Controls.Btn.UCBtnExt btnMgrPallet;
        private System.Windows.Forms.ToolStripButton toolStripButtonExpot;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
        private System.Windows.Forms.ToolStripButton toolStripButtonImport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
    }
}