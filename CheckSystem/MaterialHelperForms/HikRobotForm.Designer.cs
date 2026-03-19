using HZH_Controls.Controls.DateTime;
using HZH_Controls.Controls.Text;
using HZH_Controls.Helpers;

namespace CheckSystem.MaterialHelperForms
{
    partial class HikRobotForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.海康威视摄像头参数ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.串口码枪参数ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.串口打印机参数ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripMenuItem();
            this.关闭设备ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.重新打开设备ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.buttonTablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.stockInStatusPanel = new System.Windows.Forms.TableLayoutPanel();
            this.btnReStockInScan = new Sunny.UI.UISymbolButton();
            this.btnStockInStatus1 = new Sunny.UI.UITitlePanel();
            this.textBox1 = new Sunny.UI.UIMarkLabel();
            this.cmbStockInType = new Sunny.UI.UIComboBox();
            this.uiMarkLabel4 = new Sunny.UI.UIMarkLabel();
            this.btnStockInStatus3 = new System.Windows.Forms.RichTextBox();
            this.btnStockInStatus2 = new Sunny.UI.UITitlePanel();
            this.txtBoxMaterialCount = new Sunny.UI.UITextBox();
            this.textBox2 = new Sunny.UI.UIMarkLabel();
            this.stockDetailTablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.cmbDetailSearchType = new UserControls.LabelCombox();
            this.txtSearchMaterialNo = new UCTextBoxEx();
            this.dateStartSearch = new UCDatePickerExt2();
            this.dateEndSearch = new UCDatePickerExt2();
            this.btnDetailSearch = new System.Windows.Forms.Button();
            this.stockInDetailsPanel = new Sunny.UI.UITitlePanel();
            this.dgvStockInDetail = new Sunny.UI.UIDataGridView();
            this.mainTablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.topTablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelDataValidation = new Sunny.UI.UITitlePanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tsLblState = new System.Windows.Forms.ToolStripLabel();
            this.tableCmbList = new System.Windows.Forms.TableLayoutPanel();
            this.btnManualStockIn = new Sunny.UI.UISymbolButton();
            this.btnRefresh = new Sunny.UI.UISymbolButton();
            this.btnReConfirm = new Sunny.UI.UISymbolButton();
            this.btnConfirm = new Sunny.UI.UISymbolButton();
            this.cmbMaterialNo = new Sunny.UI.UIComboBox();
            this.uiMarkLabel3 = new Sunny.UI.UIMarkLabel();
            this.cmbSupplyNo = new Sunny.UI.UIComboBox();
            this.uiMarkLabel2 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel1 = new Sunny.UI.UIMarkLabel();
            this.cmbStockInNo = new Sunny.UI.UIComboBox();
            this.generatedResultTablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.txtTotoalStockInCount = new Sunny.UI.UITextBox();
            this.txtCurrentStockInCount = new Sunny.UI.UITextBox();
            this.uiMarkLabel7 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel5 = new Sunny.UI.UIMarkLabel();
            this.uiTitlePanel1 = new Sunny.UI.UITitlePanel();
            this.dgvGeneratedResult = new Sunny.UI.UIDataGridView();
            this.scanResultTablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.txtGeneratedBarcode = new TextBoxEx();
            this.txtBarcodeScanResults = new Sunny.UI.UIRichTextBox();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.buttonTablePanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.stockInStatusPanel.SuspendLayout();
            this.btnStockInStatus1.SuspendLayout();
            this.btnStockInStatus2.SuspendLayout();
            this.stockDetailTablePanel.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.stockInDetailsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStockInDetail)).BeginInit();
            this.mainTablePanel.SuspendLayout();
            this.topTablePanel.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelDataValidation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.toolStrip2.SuspendLayout();
            this.tableCmbList.SuspendLayout();
            this.generatedResultTablePanel.SuspendLayout();
            this.uiTitlePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGeneratedResult)).BeginInit();
            this.scanResultTablePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton6});
            this.toolStrip1.Location = new System.Drawing.Point(0, 35);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(864, 25);
            this.toolStrip1.TabIndex = 15;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.海康威视摄像头参数ToolStripMenuItem,
            this.串口码枪参数ToolStripMenuItem,
            this.串口打印机参数ToolStripMenuItem});
            this.toolStripButton1.Image = global::CheckSystem.Properties.Resources.TSB_AddRectangle_Image;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(85, 22);
            this.toolStripButton1.Text = "参数设置";
            // 
            // 海康威视摄像头参数ToolStripMenuItem
            // 
            this.海康威视摄像头参数ToolStripMenuItem.Image = global::CheckSystem.Properties.Resources.TSB_Capture_Image;
            this.海康威视摄像头参数ToolStripMenuItem.Name = "海康威视摄像头参数ToolStripMenuItem";
            this.海康威视摄像头参数ToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.海康威视摄像头参数ToolStripMenuItem.Text = "海康威视摄像头参数";
            this.海康威视摄像头参数ToolStripMenuItem.Click += new System.EventHandler(this.海康威视摄像头参数ToolStripMenuItem_Click);
            // 
            // 串口码枪参数ToolStripMenuItem
            // 
            this.串口码枪参数ToolStripMenuItem.Image = global::CheckSystem.Properties.Resources.TSB_Detect_Image;
            this.串口码枪参数ToolStripMenuItem.Name = "串口码枪参数ToolStripMenuItem";
            this.串口码枪参数ToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.串口码枪参数ToolStripMenuItem.Text = "扫码枪、打印机连接参数";
            this.串口码枪参数ToolStripMenuItem.Click += new System.EventHandler(this.串口码枪参数ToolStripMenuItem_Click);
            // 
            // 串口打印机参数ToolStripMenuItem
            // 
            this.串口打印机参数ToolStripMenuItem.Image = global::CheckSystem.Properties.Resources.TSB_Save_Image;
            this.串口打印机参数ToolStripMenuItem.Name = "串口打印机参数ToolStripMenuItem";
            this.串口打印机参数ToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.串口打印机参数ToolStripMenuItem.Text = "打印模板参数";
            this.串口打印机参数ToolStripMenuItem.Click += new System.EventHandler(this.串口打印机参数ToolStripMenuItem_Click);
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton5,
            this.关闭设备ToolStripMenuItem,
            this.重新打开设备ToolStripMenuItem});
            this.toolStripButton6.Image = global::CheckSystem.Properties.Resources.TSB_AddRectangle_Image;
            this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new System.Drawing.Size(109, 22);
            this.toolStripButton6.Text = "相机操作列表";
            this.toolStripButton6.Visible = false;
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.Image = global::CheckSystem.Properties.Resources.TSB_Capture_Image;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(148, 22);
            this.toolStripButton5.Text = "软触发一次";
            this.toolStripButton5.Visible = false;
            // 
            // 关闭设备ToolStripMenuItem
            // 
            this.关闭设备ToolStripMenuItem.Image = global::CheckSystem.Properties.Resources.TSB_DeleteShape_Image;
            this.关闭设备ToolStripMenuItem.Name = "关闭设备ToolStripMenuItem";
            this.关闭设备ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.关闭设备ToolStripMenuItem.Text = "关闭设备";
            this.关闭设备ToolStripMenuItem.Click += new System.EventHandler(this.关闭设备ToolStripMenuItem_Click);
            // 
            // 重新打开设备ToolStripMenuItem
            // 
            this.重新打开设备ToolStripMenuItem.Image = global::CheckSystem.Properties.Resources.TSB_Detect_Image;
            this.重新打开设备ToolStripMenuItem.Name = "重新打开设备ToolStripMenuItem";
            this.重新打开设备ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.重新打开设备ToolStripMenuItem.Text = "重新打开设备";
            this.重新打开设备ToolStripMenuItem.Click += new System.EventHandler(this.重新打开设备ToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 592);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(864, 22);
            this.statusStrip1.TabIndex = 17;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.lblStatus.ForeColor = System.Drawing.Color.Red;
            this.lblStatus.Image = global::CheckSystem.Properties.Resources.TSB_DeleteShape_Image;
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // buttonTablePanel
            // 
            this.buttonTablePanel.ColumnCount = 3;
            this.mainTablePanel.SetColumnSpan(this.buttonTablePanel, 2);
            this.buttonTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.buttonTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.buttonTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 350F));
            this.buttonTablePanel.Controls.Add(this.tableLayoutPanel1, 2, 0);
            this.buttonTablePanel.Controls.Add(this.stockDetailTablePanel, 0, 0);
            this.buttonTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTablePanel.Location = new System.Drawing.Point(1, 278);
            this.buttonTablePanel.Margin = new System.Windows.Forms.Padding(1);
            this.buttonTablePanel.Name = "buttonTablePanel";
            this.buttonTablePanel.RowCount = 1;
            this.buttonTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.buttonTablePanel.Size = new System.Drawing.Size(862, 275);
            this.buttonTablePanel.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(513, 1);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(348, 273);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.stockInStatusPanel);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(1, 1);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(1);
            this.groupBox1.Size = new System.Drawing.Size(346, 251);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "入库扫描状态";
            // 
            // stockInStatusPanel
            // 
            this.stockInStatusPanel.ColumnCount = 4;
            this.stockInStatusPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.stockInStatusPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.stockInStatusPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.stockInStatusPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.stockInStatusPanel.Controls.Add(this.btnReStockInScan, 2, 5);
            this.stockInStatusPanel.Controls.Add(this.btnStockInStatus1, 0, 3);
            this.stockInStatusPanel.Controls.Add(this.cmbStockInType, 0, 0);
            this.stockInStatusPanel.Controls.Add(this.uiMarkLabel4, 0, 0);
            this.stockInStatusPanel.Controls.Add(this.btnStockInStatus3, 0, 5);
            this.stockInStatusPanel.Controls.Add(this.btnStockInStatus2, 0, 1);
            this.stockInStatusPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stockInStatusPanel.Location = new System.Drawing.Point(1, 23);
            this.stockInStatusPanel.Margin = new System.Windows.Forms.Padding(1);
            this.stockInStatusPanel.Name = "stockInStatusPanel";
            this.stockInStatusPanel.RowCount = 6;
            this.stockInStatusPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.stockInStatusPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.stockInStatusPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.stockInStatusPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.stockInStatusPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.stockInStatusPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.stockInStatusPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.stockInStatusPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.stockInStatusPanel.Size = new System.Drawing.Size(344, 227);
            this.stockInStatusPanel.TabIndex = 2;
            // 
            // btnReStockInScan
            // 
            this.stockInStatusPanel.SetColumnSpan(this.btnReStockInScan, 2);
            this.btnReStockInScan.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReStockInScan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReStockInScan.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.btnReStockInScan.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.btnReStockInScan.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(175)))), ((int)(((byte)(83)))));
            this.btnReStockInScan.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.btnReStockInScan.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.btnReStockInScan.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnReStockInScan.Location = new System.Drawing.Point(175, 203);
            this.btnReStockInScan.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnReStockInScan.Name = "btnReStockInScan";
            this.btnReStockInScan.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.btnReStockInScan.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(175)))), ((int)(((byte)(83)))));
            this.btnReStockInScan.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.btnReStockInScan.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.btnReStockInScan.Size = new System.Drawing.Size(166, 21);
            this.btnReStockInScan.Style = Sunny.UI.UIStyle.Orange;
            this.btnReStockInScan.StyleCustomMode = true;
            this.btnReStockInScan.Symbol = 61553;
            this.btnReStockInScan.TabIndex = 84;
            this.btnReStockInScan.Text = "更换箱体";
            this.btnReStockInScan.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnReStockInScan.Click += new System.EventHandler(this.btnReStockInScan_Click);
            // 
            // btnStockInStatus1
            // 
            this.stockInStatusPanel.SetColumnSpan(this.btnStockInStatus1, 4);
            this.btnStockInStatus1.Controls.Add(this.textBox1);
            this.btnStockInStatus1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStockInStatus1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStockInStatus1.Location = new System.Drawing.Point(1, 126);
            this.btnStockInStatus1.Margin = new System.Windows.Forms.Padding(1);
            this.btnStockInStatus1.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnStockInStatus1.Name = "btnStockInStatus1";
            this.btnStockInStatus1.Padding = new System.Windows.Forms.Padding(0, 35, 0, 0);
            this.stockInStatusPanel.SetRowSpan(this.btnStockInStatus1, 2);
            this.btnStockInStatus1.ShowText = false;
            this.btnStockInStatus1.Size = new System.Drawing.Size(342, 73);
            this.btnStockInStatus1.Style = Sunny.UI.UIStyle.Custom;
            this.btnStockInStatus1.TabIndex = 17;
            this.btnStockInStatus1.Text = "待入库物料扫码";
            this.btnStockInStatus1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnStockInStatus1.TitleColor = System.Drawing.Color.DarkRed;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.LightGray;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Font = new System.Drawing.Font("黑体", 15F, System.Drawing.FontStyle.Bold);
            this.textBox1.Location = new System.Drawing.Point(0, 35);
            this.textBox1.Name = "textBox1";
            this.textBox1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.textBox1.Size = new System.Drawing.Size(342, 38);
            this.textBox1.Style = Sunny.UI.UIStyle.Custom;
            this.textBox1.TabIndex = 1;
            this.textBox1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbStockInType
            // 
            this.stockInStatusPanel.SetColumnSpan(this.cmbStockInType, 3);
            this.cmbStockInType.DataSource = null;
            this.cmbStockInType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbStockInType.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cmbStockInType.FillColor = System.Drawing.Color.White;
            this.cmbStockInType.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbStockInType.Location = new System.Drawing.Point(87, 1);
            this.cmbStockInType.Margin = new System.Windows.Forms.Padding(1);
            this.cmbStockInType.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmbStockInType.Name = "cmbStockInType";
            this.cmbStockInType.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbStockInType.Size = new System.Drawing.Size(256, 48);
            this.cmbStockInType.TabIndex = 15;
            this.cmbStockInType.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbStockInType.Watermark = "";
            // 
            // uiMarkLabel4
            // 
            this.uiMarkLabel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMarkLabel4.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiMarkLabel4.Location = new System.Drawing.Point(3, 0);
            this.uiMarkLabel4.Name = "uiMarkLabel4";
            this.uiMarkLabel4.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel4.Size = new System.Drawing.Size(80, 50);
            this.uiMarkLabel4.TabIndex = 13;
            this.uiMarkLabel4.Text = "入库方式";
            this.uiMarkLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnStockInStatus3
            // 
            this.btnStockInStatus3.BackColor = System.Drawing.Color.DarkGray;
            this.stockInStatusPanel.SetColumnSpan(this.btnStockInStatus3, 2);
            this.btnStockInStatus3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStockInStatus3.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.btnStockInStatus3.Location = new System.Drawing.Point(0, 200);
            this.btnStockInStatus3.Margin = new System.Windows.Forms.Padding(0);
            this.btnStockInStatus3.Name = "btnStockInStatus3";
            this.btnStockInStatus3.ReadOnly = true;
            this.btnStockInStatus3.Size = new System.Drawing.Size(172, 27);
            this.btnStockInStatus3.TabIndex = 11;
            this.btnStockInStatus3.Text = "";
            // 
            // btnStockInStatus2
            // 
            this.stockInStatusPanel.SetColumnSpan(this.btnStockInStatus2, 4);
            this.btnStockInStatus2.Controls.Add(this.txtBoxMaterialCount);
            this.btnStockInStatus2.Controls.Add(this.textBox2);
            this.btnStockInStatus2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStockInStatus2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStockInStatus2.Location = new System.Drawing.Point(1, 51);
            this.btnStockInStatus2.Margin = new System.Windows.Forms.Padding(1);
            this.btnStockInStatus2.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnStockInStatus2.Name = "btnStockInStatus2";
            this.btnStockInStatus2.Padding = new System.Windows.Forms.Padding(0, 35, 0, 0);
            this.stockInStatusPanel.SetRowSpan(this.btnStockInStatus2, 2);
            this.btnStockInStatus2.ShowText = false;
            this.btnStockInStatus2.Size = new System.Drawing.Size(342, 73);
            this.btnStockInStatus2.Style = Sunny.UI.UIStyle.Custom;
            this.btnStockInStatus2.TabIndex = 16;
            this.btnStockInStatus2.Text = "待入库箱体扫码";
            this.btnStockInStatus2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnStockInStatus2.TitleColor = System.Drawing.Color.DarkRed;
            // 
            // txtBoxMaterialCount
            // 
            this.txtBoxMaterialCount.ButtonWidth = 100;
            this.txtBoxMaterialCount.CanEmpty = true;
            this.txtBoxMaterialCount.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtBoxMaterialCount.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtBoxMaterialCount.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold);
            this.txtBoxMaterialCount.Location = new System.Drawing.Point(0, 44);
            this.txtBoxMaterialCount.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtBoxMaterialCount.Minimum = 0D;
            this.txtBoxMaterialCount.MinimumSize = new System.Drawing.Size(1, 1);
            this.txtBoxMaterialCount.Name = "txtBoxMaterialCount";
            this.txtBoxMaterialCount.Padding = new System.Windows.Forms.Padding(5);
            this.txtBoxMaterialCount.ReadOnly = true;
            this.txtBoxMaterialCount.ShowText = false;
            this.txtBoxMaterialCount.Size = new System.Drawing.Size(342, 29);
            this.txtBoxMaterialCount.Style = Sunny.UI.UIStyle.Custom;
            this.txtBoxMaterialCount.TabIndex = 25;
            this.txtBoxMaterialCount.Text = "0";
            this.txtBoxMaterialCount.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtBoxMaterialCount.Type = Sunny.UI.UITextBox.UIEditType.Integer;
            this.txtBoxMaterialCount.Watermark = "";
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.LightGray;
            this.textBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox2.Location = new System.Drawing.Point(0, 35);
            this.textBox2.Name = "textBox2";
            this.textBox2.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.textBox2.Size = new System.Drawing.Size(342, 38);
            this.textBox2.Style = Sunny.UI.UIStyle.Custom;
            this.textBox2.TabIndex = 0;
            this.textBox2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // stockDetailTablePanel
            // 
            this.stockDetailTablePanel.ColumnCount = 1;
            this.buttonTablePanel.SetColumnSpan(this.stockDetailTablePanel, 2);
            this.stockDetailTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.stockDetailTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.stockDetailTablePanel.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.stockDetailTablePanel.Controls.Add(this.stockInDetailsPanel, 0, 1);
            this.stockDetailTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stockDetailTablePanel.Location = new System.Drawing.Point(1, 1);
            this.stockDetailTablePanel.Margin = new System.Windows.Forms.Padding(1);
            this.stockDetailTablePanel.Name = "stockDetailTablePanel";
            this.stockDetailTablePanel.RowCount = 2;
            this.stockDetailTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.stockDetailTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.stockDetailTablePanel.Size = new System.Drawing.Size(510, 273);
            this.stockDetailTablePanel.TabIndex = 5;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.cmbDetailSearchType);
            this.flowLayoutPanel1.Controls.Add(this.txtSearchMaterialNo);
            this.flowLayoutPanel1.Controls.Add(this.dateStartSearch);
            this.flowLayoutPanel1.Controls.Add(this.dateEndSearch);
            this.flowLayoutPanel1.Controls.Add(this.btnDetailSearch);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(1, 1);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(1);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(508, 48);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // cmbDetailSearchType
            // 
            this.cmbDetailSearchType.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbDetailSearchType.LabelString = "查找类型：";
            this.cmbDetailSearchType.Location = new System.Drawing.Point(0, 0);
            this.cmbDetailSearchType.Margin = new System.Windows.Forms.Padding(0);
            this.cmbDetailSearchType.Name = "cmbDetailSearchType";
            this.cmbDetailSearchType.Size = new System.Drawing.Size(348, 37);
            this.cmbDetailSearchType.TabIndex = 8;
            // 
            // txtSearchMaterialNo
            // 
            this.txtSearchMaterialNo.BackColor = System.Drawing.Color.Transparent;
            this.txtSearchMaterialNo.ConerRadius = 5;
            this.txtSearchMaterialNo.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSearchMaterialNo.DecLength = 2;
            this.txtSearchMaterialNo.FillColor = System.Drawing.Color.Empty;
            this.txtSearchMaterialNo.FocusBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.txtSearchMaterialNo.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtSearchMaterialNo.InputText = "";
            this.txtSearchMaterialNo.InputType = TextInputType.NotControl;
            this.txtSearchMaterialNo.IsFocusColor = true;
            this.txtSearchMaterialNo.IsRadius = true;
            this.txtSearchMaterialNo.IsShowClearBtn = true;
            this.txtSearchMaterialNo.IsShowKeyboard = false;
            this.txtSearchMaterialNo.IsShowRect = true;
            this.txtSearchMaterialNo.IsShowSearchBtn = false;
            this.txtSearchMaterialNo.KeyBoardType = KeyBoardType.UCKeyBorderAll_EN;
            this.txtSearchMaterialNo.Location = new System.Drawing.Point(4, 42);
            this.txtSearchMaterialNo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtSearchMaterialNo.MaxValue = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.txtSearchMaterialNo.MinValue = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.txtSearchMaterialNo.Name = "txtSearchMaterialNo";
            this.txtSearchMaterialNo.Padding = new System.Windows.Forms.Padding(5);
            this.txtSearchMaterialNo.PasswordChar = '\0';
            this.txtSearchMaterialNo.PromptColor = System.Drawing.Color.Gray;
            this.txtSearchMaterialNo.PromptFont = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtSearchMaterialNo.PromptText = "";
            this.txtSearchMaterialNo.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.txtSearchMaterialNo.RectWidth = 1;
            this.txtSearchMaterialNo.RegexPattern = "";
            this.txtSearchMaterialNo.Size = new System.Drawing.Size(322, 42);
            this.txtSearchMaterialNo.TabIndex = 9;
            // 
            // dateStartSearch
            // 
            this.dateStartSearch.BackColor = System.Drawing.Color.Transparent;
            this.dateStartSearch.ConerRadius = 5;
            this.dateStartSearch.CurrentTime = new System.DateTime(2023, 2, 15, 11, 11, 56, 0);
            this.dateStartSearch.FillColor = System.Drawing.Color.White;
            this.dateStartSearch.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.dateStartSearch.IsRadius = true;
            this.dateStartSearch.IsShowRect = true;
            this.dateStartSearch.Location = new System.Drawing.Point(4, 94);
            this.dateStartSearch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dateStartSearch.Name = "dateStartSearch";
            this.dateStartSearch.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.dateStartSearch.RectWidth = 1;
            this.dateStartSearch.SelectColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.dateStartSearch.Size = new System.Drawing.Size(336, 39);
            this.dateStartSearch.TabIndex = 13;
            this.dateStartSearch.TimeFontSize = 20;
            this.dateStartSearch.TimeType = DateTimePickerType.Date;
            // 
            // dateEndSearch
            // 
            this.dateEndSearch.BackColor = System.Drawing.Color.Transparent;
            this.dateEndSearch.ConerRadius = 5;
            this.dateEndSearch.CurrentTime = new System.DateTime(2023, 2, 15, 11, 11, 56, 0);
            this.dateEndSearch.FillColor = System.Drawing.Color.White;
            this.dateEndSearch.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.dateEndSearch.IsRadius = true;
            this.dateEndSearch.IsShowRect = true;
            this.dateEndSearch.Location = new System.Drawing.Point(4, 143);
            this.dateEndSearch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dateEndSearch.Name = "dateEndSearch";
            this.dateEndSearch.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.dateEndSearch.RectWidth = 1;
            this.dateEndSearch.SelectColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.dateEndSearch.Size = new System.Drawing.Size(336, 39);
            this.dateEndSearch.TabIndex = 12;
            this.dateEndSearch.TimeFontSize = 20;
            this.dateEndSearch.TimeType = DateTimePickerType.Date;
            // 
            // btnDetailSearch
            // 
            this.btnDetailSearch.Location = new System.Drawing.Point(347, 141);
            this.btnDetailSearch.Name = "btnDetailSearch";
            this.btnDetailSearch.Size = new System.Drawing.Size(130, 43);
            this.btnDetailSearch.TabIndex = 11;
            this.btnDetailSearch.Text = "查找";
            this.btnDetailSearch.UseVisualStyleBackColor = true;
            this.btnDetailSearch.Click += new System.EventHandler(this.btnDetailSearch_Click);
            // 
            // stockInDetailsPanel
            // 
            this.stockInDetailsPanel.Controls.Add(this.dgvStockInDetail);
            this.stockInDetailsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stockInDetailsPanel.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.stockInDetailsPanel.Location = new System.Drawing.Point(1, 51);
            this.stockInDetailsPanel.Margin = new System.Windows.Forms.Padding(1);
            this.stockInDetailsPanel.MinimumSize = new System.Drawing.Size(1, 1);
            this.stockInDetailsPanel.Name = "stockInDetailsPanel";
            this.stockInDetailsPanel.Padding = new System.Windows.Forms.Padding(0, 35, 0, 0);
            this.stockInDetailsPanel.ShowText = false;
            this.stockInDetailsPanel.Size = new System.Drawing.Size(508, 221);
            this.stockInDetailsPanel.TabIndex = 6;
            this.stockInDetailsPanel.Text = "入库明细";
            this.stockInDetailsPanel.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.stockInDetailsPanel.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvStockInDetail
            // 
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            this.dgvStockInDetail.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvStockInDetail.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            this.dgvStockInDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("微软雅黑", 8F);
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvStockInDetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvStockInDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(236)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvStockInDetail.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgvStockInDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStockInDetail.EnableHeadersVisualStyles = false;
            this.dgvStockInDetail.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgvStockInDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(173)))), ((int)(((byte)(255)))));
            this.dgvStockInDetail.Location = new System.Drawing.Point(0, 35);
            this.dgvStockInDetail.Name = "dgvStockInDetail";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle9.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvStockInDetail.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(236)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.dgvStockInDetail.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.dgvStockInDetail.RowTemplate.Height = 23;
            this.dgvStockInDetail.ScrollBarRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            this.dgvStockInDetail.SelectedIndex = -1;
            this.dgvStockInDetail.Size = new System.Drawing.Size(508, 186);
            this.dgvStockInDetail.TabIndex = 0;
            // 
            // mainTablePanel
            // 
            this.mainTablePanel.ColumnCount = 2;
            this.mainTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainTablePanel.Controls.Add(this.topTablePanel, 0, 0);
            this.mainTablePanel.Controls.Add(this.buttonTablePanel, 0, 1);
            this.mainTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTablePanel.Location = new System.Drawing.Point(0, 60);
            this.mainTablePanel.Name = "mainTablePanel";
            this.mainTablePanel.RowCount = 2;
            this.mainTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.mainTablePanel.Size = new System.Drawing.Size(864, 554);
            this.mainTablePanel.TabIndex = 16;
            // 
            // topTablePanel
            // 
            this.topTablePanel.ColumnCount = 3;
            this.mainTablePanel.SetColumnSpan(this.topTablePanel, 2);
            this.topTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.topTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.topTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.topTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.topTablePanel.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.topTablePanel.Controls.Add(this.generatedResultTablePanel, 2, 0);
            this.topTablePanel.Controls.Add(this.scanResultTablePanel, 1, 0);
            this.topTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topTablePanel.Location = new System.Drawing.Point(0, 0);
            this.topTablePanel.Margin = new System.Windows.Forms.Padding(0);
            this.topTablePanel.Name = "topTablePanel";
            this.topTablePanel.RowCount = 1;
            this.topTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.topTablePanel.Size = new System.Drawing.Size(864, 277);
            this.topTablePanel.TabIndex = 1;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.tableCmbList, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(288, 277);
            this.tableLayoutPanel3.TabIndex = 13;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightGray;
            this.panel1.Controls.Add(this.panelDataValidation);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 180);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(288, 97);
            this.panel1.TabIndex = 5;
            // 
            // panelDataValidation
            // 
            this.panelDataValidation.Controls.Add(this.pictureBox1);
            this.panelDataValidation.Controls.Add(this.toolStrip2);
            this.panelDataValidation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDataValidation.Font = new System.Drawing.Font("微软雅黑", 7F);
            this.panelDataValidation.Location = new System.Drawing.Point(0, 0);
            this.panelDataValidation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelDataValidation.MinimumSize = new System.Drawing.Size(1, 1);
            this.panelDataValidation.Name = "panelDataValidation";
            this.panelDataValidation.Padding = new System.Windows.Forms.Padding(0, 35, 0, 0);
            this.panelDataValidation.ShowText = false;
            this.panelDataValidation.Size = new System.Drawing.Size(288, 97);
            this.panelDataValidation.TabIndex = 20;
            this.panelDataValidation.Text = "入库校验";
            this.panelDataValidation.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.panelDataValidation.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 60);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(288, 37);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.tsLblState});
            this.toolStrip2.Location = new System.Drawing.Point(0, 35);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(288, 25);
            this.toolStrip2.TabIndex = 5;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(44, 22);
            this.toolStripLabel1.Text = "状态：";
            // 
            // tsLblState
            // 
            this.tsLblState.Name = "tsLblState";
            this.tsLblState.Size = new System.Drawing.Size(64, 22);
            this.tsLblState.Text = "tsLblState";
            // 
            // tableCmbList
            // 
            this.tableCmbList.ColumnCount = 4;
            this.tableCmbList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableCmbList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableCmbList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableCmbList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableCmbList.Controls.Add(this.btnManualStockIn, 3, 3);
            this.tableCmbList.Controls.Add(this.btnRefresh, 2, 3);
            this.tableCmbList.Controls.Add(this.btnReConfirm, 1, 3);
            this.tableCmbList.Controls.Add(this.btnConfirm, 0, 3);
            this.tableCmbList.Controls.Add(this.cmbMaterialNo, 1, 2);
            this.tableCmbList.Controls.Add(this.uiMarkLabel3, 0, 2);
            this.tableCmbList.Controls.Add(this.cmbSupplyNo, 1, 1);
            this.tableCmbList.Controls.Add(this.uiMarkLabel2, 0, 1);
            this.tableCmbList.Controls.Add(this.uiMarkLabel1, 0, 0);
            this.tableCmbList.Controls.Add(this.cmbStockInNo, 1, 0);
            this.tableCmbList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableCmbList.Location = new System.Drawing.Point(1, 1);
            this.tableCmbList.Margin = new System.Windows.Forms.Padding(1);
            this.tableCmbList.Name = "tableCmbList";
            this.tableCmbList.RowCount = 4;
            this.tableCmbList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableCmbList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableCmbList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableCmbList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableCmbList.Size = new System.Drawing.Size(286, 178);
            this.tableCmbList.TabIndex = 6;
            // 
            // btnManualStockIn
            // 
            this.btnManualStockIn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnManualStockIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnManualStockIn.Enabled = false;
            this.btnManualStockIn.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnManualStockIn.Location = new System.Drawing.Point(216, 102);
            this.btnManualStockIn.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnManualStockIn.Name = "btnManualStockIn";
            this.btnManualStockIn.Size = new System.Drawing.Size(67, 73);
            this.btnManualStockIn.Style = Sunny.UI.UIStyle.Custom;
            this.btnManualStockIn.StyleCustomMode = true;
            this.btnManualStockIn.Symbol = 61529;
            this.btnManualStockIn.TabIndex = 85;
            this.btnManualStockIn.Text = "手动入库";
            this.btnManualStockIn.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnManualStockIn.Click += new System.EventHandler(this.btnManualStockIn_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRefresh.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.btnRefresh.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.btnRefresh.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(175)))), ((int)(((byte)(83)))));
            this.btnRefresh.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.btnRefresh.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.btnRefresh.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnRefresh.Location = new System.Drawing.Point(145, 102);
            this.btnRefresh.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.btnRefresh.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(175)))), ((int)(((byte)(83)))));
            this.btnRefresh.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.btnRefresh.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.btnRefresh.Size = new System.Drawing.Size(65, 73);
            this.btnRefresh.Style = Sunny.UI.UIStyle.Orange;
            this.btnRefresh.StyleCustomMode = true;
            this.btnRefresh.Symbol = 61553;
            this.btnRefresh.TabIndex = 84;
            this.btnRefresh.Text = "刷新入库单";
            this.btnRefresh.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnReConfirm
            // 
            this.btnReConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReConfirm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReConfirm.Enabled = false;
            this.btnReConfirm.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnReConfirm.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnReConfirm.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(115)))), ((int)(((byte)(115)))));
            this.btnReConfirm.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnReConfirm.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnReConfirm.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnReConfirm.Location = new System.Drawing.Point(74, 102);
            this.btnReConfirm.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnReConfirm.Name = "btnReConfirm";
            this.btnReConfirm.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnReConfirm.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(115)))), ((int)(((byte)(115)))));
            this.btnReConfirm.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnReConfirm.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnReConfirm.Size = new System.Drawing.Size(65, 73);
            this.btnReConfirm.Style = Sunny.UI.UIStyle.Red;
            this.btnReConfirm.StyleCustomMode = true;
            this.btnReConfirm.Symbol = 61453;
            this.btnReConfirm.TabIndex = 81;
            this.btnReConfirm.Text = "重新选择";
            this.btnReConfirm.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnReConfirm.Click += new System.EventHandler(this.btnReConfirm_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConfirm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnConfirm.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnConfirm.Location = new System.Drawing.Point(3, 102);
            this.btnConfirm.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(65, 73);
            this.btnConfirm.StyleCustomMode = true;
            this.btnConfirm.TabIndex = 80;
            this.btnConfirm.Text = "确认";
            this.btnConfirm.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // cmbMaterialNo
            // 
            this.tableCmbList.SetColumnSpan(this.cmbMaterialNo, 3);
            this.cmbMaterialNo.DataSource = null;
            this.cmbMaterialNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbMaterialNo.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cmbMaterialNo.FillColor = System.Drawing.Color.White;
            this.cmbMaterialNo.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbMaterialNo.Location = new System.Drawing.Point(72, 67);
            this.cmbMaterialNo.Margin = new System.Windows.Forms.Padding(1);
            this.cmbMaterialNo.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmbMaterialNo.Name = "cmbMaterialNo";
            this.cmbMaterialNo.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbMaterialNo.Size = new System.Drawing.Size(213, 31);
            this.cmbMaterialNo.TabIndex = 14;
            this.cmbMaterialNo.Text = "uiComboBox1";
            this.cmbMaterialNo.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbMaterialNo.Watermark = "";
            // 
            // uiMarkLabel3
            // 
            this.uiMarkLabel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMarkLabel3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiMarkLabel3.Location = new System.Drawing.Point(3, 66);
            this.uiMarkLabel3.Name = "uiMarkLabel3";
            this.uiMarkLabel3.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel3.Size = new System.Drawing.Size(65, 33);
            this.uiMarkLabel3.TabIndex = 13;
            this.uiMarkLabel3.Text = "物料号";
            this.uiMarkLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbSupplyNo
            // 
            this.tableCmbList.SetColumnSpan(this.cmbSupplyNo, 3);
            this.cmbSupplyNo.DataSource = null;
            this.cmbSupplyNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbSupplyNo.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cmbSupplyNo.FillColor = System.Drawing.Color.White;
            this.cmbSupplyNo.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbSupplyNo.Location = new System.Drawing.Point(72, 34);
            this.cmbSupplyNo.Margin = new System.Windows.Forms.Padding(1);
            this.cmbSupplyNo.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmbSupplyNo.Name = "cmbSupplyNo";
            this.cmbSupplyNo.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbSupplyNo.Size = new System.Drawing.Size(213, 31);
            this.cmbSupplyNo.TabIndex = 12;
            this.cmbSupplyNo.Text = "uiComboBox1";
            this.cmbSupplyNo.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbSupplyNo.Watermark = "";
            // 
            // uiMarkLabel2
            // 
            this.uiMarkLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMarkLabel2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiMarkLabel2.Location = new System.Drawing.Point(3, 33);
            this.uiMarkLabel2.Name = "uiMarkLabel2";
            this.uiMarkLabel2.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel2.Size = new System.Drawing.Size(65, 33);
            this.uiMarkLabel2.TabIndex = 11;
            this.uiMarkLabel2.Text = "供应商编码";
            this.uiMarkLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel1
            // 
            this.uiMarkLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMarkLabel1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiMarkLabel1.Location = new System.Drawing.Point(3, 0);
            this.uiMarkLabel1.Name = "uiMarkLabel1";
            this.uiMarkLabel1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel1.Size = new System.Drawing.Size(65, 33);
            this.uiMarkLabel1.TabIndex = 9;
            this.uiMarkLabel1.Text = "入库单号";
            this.uiMarkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbStockInNo
            // 
            this.tableCmbList.SetColumnSpan(this.cmbStockInNo, 3);
            this.cmbStockInNo.DataSource = null;
            this.cmbStockInNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbStockInNo.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cmbStockInNo.FillColor = System.Drawing.Color.White;
            this.cmbStockInNo.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbStockInNo.Location = new System.Drawing.Point(72, 1);
            this.cmbStockInNo.Margin = new System.Windows.Forms.Padding(1);
            this.cmbStockInNo.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmbStockInNo.Name = "cmbStockInNo";
            this.cmbStockInNo.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbStockInNo.Size = new System.Drawing.Size(213, 31);
            this.cmbStockInNo.TabIndex = 10;
            this.cmbStockInNo.Text = "uiComboBox1";
            this.cmbStockInNo.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbStockInNo.Watermark = "";
            // 
            // generatedResultTablePanel
            // 
            this.generatedResultTablePanel.ColumnCount = 4;
            this.generatedResultTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.generatedResultTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.generatedResultTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.generatedResultTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.generatedResultTablePanel.Controls.Add(this.txtTotoalStockInCount, 3, 1);
            this.generatedResultTablePanel.Controls.Add(this.txtCurrentStockInCount, 1, 1);
            this.generatedResultTablePanel.Controls.Add(this.uiMarkLabel7, 2, 1);
            this.generatedResultTablePanel.Controls.Add(this.uiMarkLabel5, 0, 1);
            this.generatedResultTablePanel.Controls.Add(this.uiTitlePanel1, 0, 0);
            this.generatedResultTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.generatedResultTablePanel.Location = new System.Drawing.Point(577, 1);
            this.generatedResultTablePanel.Margin = new System.Windows.Forms.Padding(1);
            this.generatedResultTablePanel.Name = "generatedResultTablePanel";
            this.generatedResultTablePanel.RowCount = 2;
            this.generatedResultTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.generatedResultTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.generatedResultTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.generatedResultTablePanel.Size = new System.Drawing.Size(286, 275);
            this.generatedResultTablePanel.TabIndex = 14;
            // 
            // txtTotoalStockInCount
            // 
            this.txtTotoalStockInCount.ButtonWidth = 100;
            this.txtTotoalStockInCount.CanEmpty = true;
            this.txtTotoalStockInCount.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTotoalStockInCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTotoalStockInCount.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold);
            this.txtTotoalStockInCount.Location = new System.Drawing.Point(217, 230);
            this.txtTotoalStockInCount.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTotoalStockInCount.Minimum = 0D;
            this.txtTotoalStockInCount.MinimumSize = new System.Drawing.Size(1, 1);
            this.txtTotoalStockInCount.Name = "txtTotoalStockInCount";
            this.txtTotoalStockInCount.Padding = new System.Windows.Forms.Padding(5);
            this.txtTotoalStockInCount.ReadOnly = true;
            this.txtTotoalStockInCount.ShowText = false;
            this.txtTotoalStockInCount.Size = new System.Drawing.Size(65, 40);
            this.txtTotoalStockInCount.TabIndex = 25;
            this.txtTotoalStockInCount.Text = "0";
            this.txtTotoalStockInCount.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtTotoalStockInCount.Type = Sunny.UI.UITextBox.UIEditType.Integer;
            this.txtTotoalStockInCount.Watermark = "";
            // 
            // txtCurrentStockInCount
            // 
            this.txtCurrentStockInCount.ButtonWidth = 100;
            this.txtCurrentStockInCount.CanEmpty = true;
            this.txtCurrentStockInCount.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCurrentStockInCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCurrentStockInCount.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold);
            this.txtCurrentStockInCount.Location = new System.Drawing.Point(75, 230);
            this.txtCurrentStockInCount.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtCurrentStockInCount.Minimum = 0D;
            this.txtCurrentStockInCount.MinimumSize = new System.Drawing.Size(1, 1);
            this.txtCurrentStockInCount.Name = "txtCurrentStockInCount";
            this.txtCurrentStockInCount.Padding = new System.Windows.Forms.Padding(5);
            this.txtCurrentStockInCount.ReadOnly = true;
            this.txtCurrentStockInCount.ShowText = false;
            this.txtCurrentStockInCount.Size = new System.Drawing.Size(63, 40);
            this.txtCurrentStockInCount.TabIndex = 24;
            this.txtCurrentStockInCount.Text = "0";
            this.txtCurrentStockInCount.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtCurrentStockInCount.Type = Sunny.UI.UITextBox.UIEditType.Integer;
            this.txtCurrentStockInCount.Watermark = "";
            // 
            // uiMarkLabel7
            // 
            this.uiMarkLabel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMarkLabel7.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold);
            this.uiMarkLabel7.Location = new System.Drawing.Point(145, 225);
            this.uiMarkLabel7.Name = "uiMarkLabel7";
            this.uiMarkLabel7.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel7.Size = new System.Drawing.Size(65, 50);
            this.uiMarkLabel7.TabIndex = 22;
            this.uiMarkLabel7.Text = "订单总数：";
            this.uiMarkLabel7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel5
            // 
            this.uiMarkLabel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMarkLabel5.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold);
            this.uiMarkLabel5.Location = new System.Drawing.Point(3, 225);
            this.uiMarkLabel5.Name = "uiMarkLabel5";
            this.uiMarkLabel5.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel5.Size = new System.Drawing.Size(65, 50);
            this.uiMarkLabel5.TabIndex = 20;
            this.uiMarkLabel5.Text = "已入库：";
            this.uiMarkLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiTitlePanel1
            // 
            this.generatedResultTablePanel.SetColumnSpan(this.uiTitlePanel1, 4);
            this.uiTitlePanel1.Controls.Add(this.dgvGeneratedResult);
            this.uiTitlePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTitlePanel1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiTitlePanel1.Location = new System.Drawing.Point(4, 5);
            this.uiTitlePanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiTitlePanel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiTitlePanel1.Name = "uiTitlePanel1";
            this.uiTitlePanel1.Padding = new System.Windows.Forms.Padding(0, 35, 0, 0);
            this.uiTitlePanel1.ShowText = false;
            this.uiTitlePanel1.Size = new System.Drawing.Size(278, 215);
            this.uiTitlePanel1.TabIndex = 19;
            this.uiTitlePanel1.Text = "生成条码内容";
            this.uiTitlePanel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dgvGeneratedResult
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            this.dgvGeneratedResult.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvGeneratedResult.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            this.dgvGeneratedResult.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvGeneratedResult.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvGeneratedResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(236)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvGeneratedResult.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvGeneratedResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGeneratedResult.EnableHeadersVisualStyles = false;
            this.dgvGeneratedResult.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgvGeneratedResult.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(173)))), ((int)(((byte)(255)))));
            this.dgvGeneratedResult.Location = new System.Drawing.Point(0, 35);
            this.dgvGeneratedResult.Name = "dgvGeneratedResult";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvGeneratedResult.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(236)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.dgvGeneratedResult.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvGeneratedResult.RowTemplate.Height = 23;
            this.dgvGeneratedResult.ScrollBarRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            this.dgvGeneratedResult.SelectedIndex = -1;
            this.dgvGeneratedResult.Size = new System.Drawing.Size(278, 180);
            this.dgvGeneratedResult.TabIndex = 0;
            // 
            // scanResultTablePanel
            // 
            this.scanResultTablePanel.ColumnCount = 1;
            this.scanResultTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.scanResultTablePanel.Controls.Add(this.txtGeneratedBarcode, 0, 1);
            this.scanResultTablePanel.Controls.Add(this.txtBarcodeScanResults, 0, 0);
            this.scanResultTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scanResultTablePanel.Location = new System.Drawing.Point(289, 1);
            this.scanResultTablePanel.Margin = new System.Windows.Forms.Padding(1);
            this.scanResultTablePanel.Name = "scanResultTablePanel";
            this.scanResultTablePanel.RowCount = 2;
            this.scanResultTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.scanResultTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.scanResultTablePanel.Size = new System.Drawing.Size(286, 275);
            this.scanResultTablePanel.TabIndex = 15;
            // 
            // txtGeneratedBarcode
            // 
            this.txtGeneratedBarcode.DecLength = 2;
            this.txtGeneratedBarcode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtGeneratedBarcode.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtGeneratedBarcode.InputType = TextInputType.NotControl;
            this.txtGeneratedBarcode.Location = new System.Drawing.Point(1, 176);
            this.txtGeneratedBarcode.Margin = new System.Windows.Forms.Padding(1);
            this.txtGeneratedBarcode.MaxValue = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.txtGeneratedBarcode.MinValue = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.txtGeneratedBarcode.Multiline = true;
            this.txtGeneratedBarcode.MyRectangle = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.txtGeneratedBarcode.Name = "txtGeneratedBarcode";
            this.txtGeneratedBarcode.OldText = null;
            this.txtGeneratedBarcode.PromptColor = System.Drawing.Color.Gray;
            this.txtGeneratedBarcode.PromptFont = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtGeneratedBarcode.PromptText = "";
            this.txtGeneratedBarcode.ReadOnly = true;
            this.txtGeneratedBarcode.RegexPattern = "";
            this.txtGeneratedBarcode.Size = new System.Drawing.Size(284, 98);
            this.txtGeneratedBarcode.TabIndex = 17;
            // 
            // txtBarcodeScanResults
            // 
            this.txtBarcodeScanResults.BackColor = System.Drawing.Color.Cornsilk;
            this.txtBarcodeScanResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBarcodeScanResults.FillColor = System.Drawing.Color.White;
            this.txtBarcodeScanResults.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.txtBarcodeScanResults.Location = new System.Drawing.Point(1, 1);
            this.txtBarcodeScanResults.Margin = new System.Windows.Forms.Padding(1);
            this.txtBarcodeScanResults.MinimumSize = new System.Drawing.Size(1, 1);
            this.txtBarcodeScanResults.Name = "txtBarcodeScanResults";
            this.txtBarcodeScanResults.Padding = new System.Windows.Forms.Padding(2);
            this.txtBarcodeScanResults.ReadOnly = true;
            this.txtBarcodeScanResults.ShowText = false;
            this.txtBarcodeScanResults.Size = new System.Drawing.Size(284, 173);
            this.txtBarcodeScanResults.Style = Sunny.UI.UIStyle.Custom;
            this.txtBarcodeScanResults.TabIndex = 18;
            this.txtBarcodeScanResults.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // HikRobotForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.LemonChiffon;
            this.ClientSize = new System.Drawing.Size(864, 614);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.mainTablePanel);
            this.Controls.Add(this.toolStrip1);
            this.Name = "HikRobotForm";
            this.ShowRadius = false;
            this.ShowShadow = true;
            this.Style = Sunny.UI.UIStyle.Custom;
            this.Text = "电子仓库料标签生成";
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 864, 614);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.buttonTablePanel.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.stockInStatusPanel.ResumeLayout(false);
            this.btnStockInStatus1.ResumeLayout(false);
            this.btnStockInStatus2.ResumeLayout(false);
            this.stockDetailTablePanel.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.stockInDetailsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStockInDetail)).EndInit();
            this.mainTablePanel.ResumeLayout(false);
            this.topTablePanel.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panelDataValidation.ResumeLayout(false);
            this.panelDataValidation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.tableCmbList.ResumeLayout(false);
            this.generatedResultTablePanel.ResumeLayout(false);
            this.uiTitlePanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGeneratedResult)).EndInit();
            this.scanResultTablePanel.ResumeLayout(false);
            this.scanResultTablePanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripDropDownButton toolStripButton6;
        private System.Windows.Forms.ToolStripMenuItem toolStripButton5;
        private System.Windows.Forms.ToolStripMenuItem 重新打开设备ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关闭设备ToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripButton1;
        private System.Windows.Forms.ToolStripMenuItem 海康威视摄像头参数ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 串口码枪参数ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 串口打印机参数ToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel buttonTablePanel;
        private System.Windows.Forms.TableLayoutPanel mainTablePanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel stockDetailTablePanel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private UserControls.LabelCombox cmbDetailSearchType;
        private UCTextBoxEx txtSearchMaterialNo;
        private UCDatePickerExt2 dateStartSearch;
        private UCDatePickerExt2 dateEndSearch;
        private System.Windows.Forms.Button btnDetailSearch;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel stockInStatusPanel;
        private Sunny.UI.UIComboBox cmbStockInType;
        private Sunny.UI.UIMarkLabel uiMarkLabel4;
        private System.Windows.Forms.RichTextBox btnStockInStatus3;
        private Sunny.UI.UITitlePanel btnStockInStatus2;
        private Sunny.UI.UIMarkLabel textBox2;
        private Sunny.UI.UITextBox txtBoxMaterialCount;
        private Sunny.UI.UITitlePanel btnStockInStatus1;
        private Sunny.UI.UIMarkLabel textBox1;
        private Sunny.UI.UISymbolButton btnReStockInScan;
        private Sunny.UI.UITitlePanel stockInDetailsPanel;
        private Sunny.UI.UIDataGridView dgvStockInDetail;
        private System.Windows.Forms.TableLayoutPanel topTablePanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Panel panel1;
        private Sunny.UI.UITitlePanel panelDataValidation;
        private System.Windows.Forms.TableLayoutPanel tableCmbList;
        private Sunny.UI.UISymbolButton btnManualStockIn;
        private Sunny.UI.UISymbolButton btnRefresh;
        private Sunny.UI.UISymbolButton btnReConfirm;
        private Sunny.UI.UISymbolButton btnConfirm;
        private Sunny.UI.UIComboBox cmbMaterialNo;
        private Sunny.UI.UIMarkLabel uiMarkLabel3;
        private Sunny.UI.UIComboBox cmbSupplyNo;
        private Sunny.UI.UIMarkLabel uiMarkLabel2;
        private Sunny.UI.UIMarkLabel uiMarkLabel1;
        private Sunny.UI.UIComboBox cmbStockInNo;
        private System.Windows.Forms.TableLayoutPanel generatedResultTablePanel;
        private Sunny.UI.UITextBox txtTotoalStockInCount;
        private Sunny.UI.UITextBox txtCurrentStockInCount;
        private Sunny.UI.UIMarkLabel uiMarkLabel7;
        private Sunny.UI.UIMarkLabel uiMarkLabel5;
        private Sunny.UI.UITitlePanel uiTitlePanel1;
        private Sunny.UI.UIDataGridView dgvGeneratedResult;
        private System.Windows.Forms.TableLayoutPanel scanResultTablePanel;
        private TextBoxEx txtGeneratedBarcode;
        private Sunny.UI.UIRichTextBox txtBarcodeScanResults;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel tsLblState;
        private System.Windows.Forms.PictureBox pictureBox1;

    }
}