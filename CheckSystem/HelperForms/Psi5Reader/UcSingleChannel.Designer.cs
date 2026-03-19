namespace CheckSystem.HelperForms.Psi5Reader
{
    partial class UcSingleChannel
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.uiGroupBox1 = new Sunny.UI.UIGroupBox();
            this.uiTabControl1 = new Sunny.UI.UITabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.uiTableLayoutPanel1 = new Sunny.UI.UITableLayoutPanel();
            this.txtPsi5Data = new Sunny.UI.UITextBox();
            this.uiMarkLabel6 = new Sunny.UI.UIMarkLabel();
            this.txtErrorReporting = new Sunny.UI.UITextBox();
            this.txt14BitData = new Sunny.UI.UITextBox();
            this.txtStatusBit = new Sunny.UI.UITextBox();
            this.uiMarkLabel5 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel4 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel3 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel2 = new Sunny.UI.UIMarkLabel();
            this.btnReadPsi5Data = new Sunny.UI.UIButton();
            this.cmbBitLen = new Sunny.UI.UIComboBox();
            this.btnInitChannel = new Sunny.UI.UIButton();
            this.uiMarkLabel1 = new Sunny.UI.UIMarkLabel();
            this.uiProcessBar1 = new Sunny.UI.UIProcessBar();
            this.txtRollingCount = new Sunny.UI.UITextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.uiDataGridView1 = new Sunny.UI.UIDataGridView();
            this.uiContextMenuStrip1 = new Sunny.UI.UIContextMenuStrip();
            this.保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清空ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uiGroupBox1.SuspendLayout();
            this.uiTabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.uiTableLayoutPanel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiDataGridView1)).BeginInit();
            this.uiContextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.Controls.Add(this.uiTabControl1);
            this.uiGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.uiGroupBox1.Margin = new System.Windows.Forms.Padding(10);
            this.uiGroupBox1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.Padding = new System.Windows.Forms.Padding(3, 32, 3, 3);
            this.uiGroupBox1.Size = new System.Drawing.Size(702, 226);
            this.uiGroupBox1.TabIndex = 0;
            this.uiGroupBox1.Text = "uiGroupBox1";
            this.uiGroupBox1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiTabControl1
            // 
            this.uiTabControl1.Controls.Add(this.tabPage1);
            this.uiTabControl1.Controls.Add(this.tabPage2);
            this.uiTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.uiTabControl1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiTabControl1.ItemSize = new System.Drawing.Size(150, 40);
            this.uiTabControl1.Location = new System.Drawing.Point(3, 32);
            this.uiTabControl1.MainPage = "";
            this.uiTabControl1.Name = "uiTabControl1";
            this.uiTabControl1.SelectedIndex = 0;
            this.uiTabControl1.Size = new System.Drawing.Size(696, 191);
            this.uiTabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.uiTabControl1.TabIndex = 0;
            this.uiTabControl1.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.uiTableLayoutPanel1);
            this.tabPage1.Location = new System.Drawing.Point(0, 40);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(696, 151);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "PSI5读取";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // uiTableLayoutPanel1
            // 
            this.uiTableLayoutPanel1.ColumnCount = 3;
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.uiTableLayoutPanel1.Controls.Add(this.txtPsi5Data, 1, 5);
            this.uiTableLayoutPanel1.Controls.Add(this.uiMarkLabel6, 0, 5);
            this.uiTableLayoutPanel1.Controls.Add(this.txtErrorReporting, 1, 6);
            this.uiTableLayoutPanel1.Controls.Add(this.txt14BitData, 1, 4);
            this.uiTableLayoutPanel1.Controls.Add(this.txtStatusBit, 1, 3);
            this.uiTableLayoutPanel1.Controls.Add(this.uiMarkLabel5, 0, 6);
            this.uiTableLayoutPanel1.Controls.Add(this.uiMarkLabel4, 0, 4);
            this.uiTableLayoutPanel1.Controls.Add(this.uiMarkLabel3, 0, 3);
            this.uiTableLayoutPanel1.Controls.Add(this.uiMarkLabel2, 0, 2);
            this.uiTableLayoutPanel1.Controls.Add(this.btnReadPsi5Data, 2, 0);
            this.uiTableLayoutPanel1.Controls.Add(this.cmbBitLen, 0, 0);
            this.uiTableLayoutPanel1.Controls.Add(this.btnInitChannel, 1, 0);
            this.uiTableLayoutPanel1.Controls.Add(this.uiMarkLabel1, 0, 1);
            this.uiTableLayoutPanel1.Controls.Add(this.uiProcessBar1, 1, 1);
            this.uiTableLayoutPanel1.Controls.Add(this.txtRollingCount, 1, 2);
            this.uiTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.uiTableLayoutPanel1.Name = "uiTableLayoutPanel1";
            this.uiTableLayoutPanel1.RowCount = 7;
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.uiTableLayoutPanel1.Size = new System.Drawing.Size(696, 151);
            this.uiTableLayoutPanel1.TabIndex = 1;
            this.uiTableLayoutPanel1.TagString = null;
            // 
            // txtPsi5Data
            // 
            this.uiTableLayoutPanel1.SetColumnSpan(this.txtPsi5Data, 2);
            this.txtPsi5Data.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPsi5Data.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPsi5Data.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPsi5Data.Location = new System.Drawing.Point(233, 114);
            this.txtPsi5Data.Margin = new System.Windows.Forms.Padding(1);
            this.txtPsi5Data.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtPsi5Data.Name = "txtPsi5Data";
            this.txtPsi5Data.Padding = new System.Windows.Forms.Padding(5);
            this.txtPsi5Data.ReadOnly = true;
            this.txtPsi5Data.ShowText = false;
            this.txtPsi5Data.Size = new System.Drawing.Size(462, 16);
            this.txtPsi5Data.TabIndex = 14;
            this.txtPsi5Data.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtPsi5Data.Watermark = "";
            // 
            // uiMarkLabel6
            // 
            this.uiMarkLabel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMarkLabel6.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.uiMarkLabel6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel6.Location = new System.Drawing.Point(3, 113);
            this.uiMarkLabel6.MarkPos = Sunny.UI.UIMarkLabel.UIMarkPos.Bottom;
            this.uiMarkLabel6.Name = "uiMarkLabel6";
            this.uiMarkLabel6.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.uiMarkLabel6.Size = new System.Drawing.Size(226, 17);
            this.uiMarkLabel6.TabIndex = 13;
            this.uiMarkLabel6.Text = "PSI5_Data:";
            this.uiMarkLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtErrorReporting
            // 
            this.uiTableLayoutPanel1.SetColumnSpan(this.txtErrorReporting, 2);
            this.txtErrorReporting.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtErrorReporting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtErrorReporting.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtErrorReporting.Location = new System.Drawing.Point(233, 131);
            this.txtErrorReporting.Margin = new System.Windows.Forms.Padding(1);
            this.txtErrorReporting.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtErrorReporting.Name = "txtErrorReporting";
            this.txtErrorReporting.Padding = new System.Windows.Forms.Padding(5);
            this.txtErrorReporting.ReadOnly = true;
            this.txtErrorReporting.ShowText = false;
            this.txtErrorReporting.Size = new System.Drawing.Size(462, 19);
            this.txtErrorReporting.TabIndex = 12;
            this.txtErrorReporting.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtErrorReporting.Watermark = "";
            // 
            // txt14BitData
            // 
            this.uiTableLayoutPanel1.SetColumnSpan(this.txt14BitData, 2);
            this.txt14BitData.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txt14BitData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt14BitData.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt14BitData.Location = new System.Drawing.Point(233, 97);
            this.txt14BitData.Margin = new System.Windows.Forms.Padding(1);
            this.txt14BitData.MinimumSize = new System.Drawing.Size(1, 16);
            this.txt14BitData.Name = "txt14BitData";
            this.txt14BitData.Padding = new System.Windows.Forms.Padding(5);
            this.txt14BitData.ReadOnly = true;
            this.txt14BitData.ShowText = false;
            this.txt14BitData.Size = new System.Drawing.Size(462, 16);
            this.txt14BitData.TabIndex = 11;
            this.txt14BitData.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txt14BitData.Watermark = "";
            // 
            // txtStatusBit
            // 
            this.uiTableLayoutPanel1.SetColumnSpan(this.txtStatusBit, 2);
            this.txtStatusBit.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtStatusBit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtStatusBit.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtStatusBit.Location = new System.Drawing.Point(233, 80);
            this.txtStatusBit.Margin = new System.Windows.Forms.Padding(1);
            this.txtStatusBit.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtStatusBit.Name = "txtStatusBit";
            this.txtStatusBit.Padding = new System.Windows.Forms.Padding(5);
            this.txtStatusBit.ReadOnly = true;
            this.txtStatusBit.ShowText = false;
            this.txtStatusBit.Size = new System.Drawing.Size(462, 16);
            this.txtStatusBit.TabIndex = 10;
            this.txtStatusBit.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtStatusBit.Watermark = "";
            // 
            // uiMarkLabel5
            // 
            this.uiMarkLabel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMarkLabel5.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.uiMarkLabel5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel5.Location = new System.Drawing.Point(3, 130);
            this.uiMarkLabel5.MarkPos = Sunny.UI.UIMarkLabel.UIMarkPos.Bottom;
            this.uiMarkLabel5.Name = "uiMarkLabel5";
            this.uiMarkLabel5.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.uiMarkLabel5.Size = new System.Drawing.Size(226, 21);
            this.uiMarkLabel5.TabIndex = 7;
            this.uiMarkLabel5.Text = "ErrorReporting:";
            this.uiMarkLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiMarkLabel4
            // 
            this.uiMarkLabel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMarkLabel4.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.uiMarkLabel4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel4.Location = new System.Drawing.Point(3, 96);
            this.uiMarkLabel4.MarkPos = Sunny.UI.UIMarkLabel.UIMarkPos.Bottom;
            this.uiMarkLabel4.Name = "uiMarkLabel4";
            this.uiMarkLabel4.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.uiMarkLabel4.Size = new System.Drawing.Size(226, 17);
            this.uiMarkLabel4.TabIndex = 6;
            this.uiMarkLabel4.Text = "14/16_Bit_Data:";
            this.uiMarkLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiMarkLabel3
            // 
            this.uiMarkLabel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMarkLabel3.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.uiMarkLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel3.Location = new System.Drawing.Point(3, 79);
            this.uiMarkLabel3.MarkPos = Sunny.UI.UIMarkLabel.UIMarkPos.Bottom;
            this.uiMarkLabel3.Name = "uiMarkLabel3";
            this.uiMarkLabel3.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.uiMarkLabel3.Size = new System.Drawing.Size(226, 17);
            this.uiMarkLabel3.TabIndex = 5;
            this.uiMarkLabel3.Text = "StatusBit:";
            this.uiMarkLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiMarkLabel2
            // 
            this.uiMarkLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMarkLabel2.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.uiMarkLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel2.Location = new System.Drawing.Point(3, 62);
            this.uiMarkLabel2.MarkPos = Sunny.UI.UIMarkLabel.UIMarkPos.Bottom;
            this.uiMarkLabel2.Name = "uiMarkLabel2";
            this.uiMarkLabel2.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.uiMarkLabel2.Size = new System.Drawing.Size(226, 17);
            this.uiMarkLabel2.TabIndex = 4;
            this.uiMarkLabel2.Text = "RollingCount:";
            this.uiMarkLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnReadPsi5Data
            // 
            this.btnReadPsi5Data.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReadPsi5Data.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReadPsi5Data.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnReadPsi5Data.Location = new System.Drawing.Point(466, 3);
            this.btnReadPsi5Data.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnReadPsi5Data.Name = "btnReadPsi5Data";
            this.btnReadPsi5Data.Size = new System.Drawing.Size(227, 39);
            this.btnReadPsi5Data.TabIndex = 2;
            this.btnReadPsi5Data.Text = "Read PSI5 Data";
            this.btnReadPsi5Data.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnReadPsi5Data.Click += new System.EventHandler(this.btnReadPsi5Data_Click);
            // 
            // cmbBitLen
            // 
            this.cmbBitLen.DataSource = null;
            this.cmbBitLen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbBitLen.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cmbBitLen.FillColor = System.Drawing.Color.White;
            this.cmbBitLen.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbBitLen.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.cmbBitLen.Items.AddRange(new object[] {
            "25Bits",
            "23Bits"});
            this.cmbBitLen.ItemSelectForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.cmbBitLen.Location = new System.Drawing.Point(4, 5);
            this.cmbBitLen.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbBitLen.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmbBitLen.Name = "cmbBitLen";
            this.cmbBitLen.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbBitLen.Size = new System.Drawing.Size(224, 35);
            this.cmbBitLen.SymbolSize = 24;
            this.cmbBitLen.TabIndex = 0;
            this.cmbBitLen.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbBitLen.Watermark = "";
            this.cmbBitLen.SelectedIndexChanged += new System.EventHandler(this.CmbBitLen_SelectedIndexChanged);
            // 
            // btnInitChannel
            // 
            this.btnInitChannel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnInitChannel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnInitChannel.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnInitChannel.Location = new System.Drawing.Point(235, 3);
            this.btnInitChannel.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnInitChannel.Name = "btnInitChannel";
            this.btnInitChannel.Size = new System.Drawing.Size(225, 39);
            this.btnInitChannel.TabIndex = 1;
            this.btnInitChannel.Text = "Init Channel";
            this.btnInitChannel.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnInitChannel.Click += new System.EventHandler(this.btnInitChannel_Click);
            // 
            // uiMarkLabel1
            // 
            this.uiMarkLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMarkLabel1.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.uiMarkLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel1.Location = new System.Drawing.Point(3, 45);
            this.uiMarkLabel1.MarkPos = Sunny.UI.UIMarkLabel.UIMarkPos.Bottom;
            this.uiMarkLabel1.Name = "uiMarkLabel1";
            this.uiMarkLabel1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.uiMarkLabel1.Size = new System.Drawing.Size(226, 17);
            this.uiMarkLabel1.TabIndex = 3;
            this.uiMarkLabel1.Text = "Percent/%:";
            this.uiMarkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiProcessBar1
            // 
            this.uiTableLayoutPanel1.SetColumnSpan(this.uiProcessBar1, 2);
            this.uiProcessBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiProcessBar1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.uiProcessBar1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiProcessBar1.Location = new System.Drawing.Point(235, 48);
            this.uiProcessBar1.MinimumSize = new System.Drawing.Size(70, 3);
            this.uiProcessBar1.Name = "uiProcessBar1";
            this.uiProcessBar1.Size = new System.Drawing.Size(458, 11);
            this.uiProcessBar1.TabIndex = 8;
            this.uiProcessBar1.Text = "uiProcessBar1";
            // 
            // txtRollingCount
            // 
            this.uiTableLayoutPanel1.SetColumnSpan(this.txtRollingCount, 2);
            this.txtRollingCount.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtRollingCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRollingCount.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtRollingCount.Location = new System.Drawing.Point(233, 63);
            this.txtRollingCount.Margin = new System.Windows.Forms.Padding(1);
            this.txtRollingCount.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtRollingCount.Name = "txtRollingCount";
            this.txtRollingCount.Padding = new System.Windows.Forms.Padding(5);
            this.txtRollingCount.ReadOnly = true;
            this.txtRollingCount.ShowText = false;
            this.txtRollingCount.Size = new System.Drawing.Size(462, 16);
            this.txtRollingCount.TabIndex = 9;
            this.txtRollingCount.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtRollingCount.Watermark = "";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.uiDataGridView1);
            this.tabPage2.Location = new System.Drawing.Point(0, 40);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(200, 60);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "数据日志/仅25Bits";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // uiDataGridView1
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            this.uiDataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.uiDataGridView1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            this.uiDataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.uiDataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.uiDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.uiDataGridView1.ContextMenuStrip = this.uiContextMenuStrip1;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(236)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.uiDataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            this.uiDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiDataGridView1.EnableHeadersVisualStyles = false;
            this.uiDataGridView1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiDataGridView1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(173)))), ((int)(((byte)(255)))));
            this.uiDataGridView1.Location = new System.Drawing.Point(0, 0);
            this.uiDataGridView1.Name = "uiDataGridView1";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.uiDataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(236)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiDataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.uiDataGridView1.RowTemplate.Height = 23;
            this.uiDataGridView1.SelectedIndex = -1;
            this.uiDataGridView1.Size = new System.Drawing.Size(200, 60);
            this.uiDataGridView1.TabIndex = 0;
            // 
            // uiContextMenuStrip1
            // 
            this.uiContextMenuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            this.uiContextMenuStrip1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.保存ToolStripMenuItem,
            this.清空ToolStripMenuItem});
            this.uiContextMenuStrip1.Name = "uiContextMenuStrip1";
            this.uiContextMenuStrip1.Size = new System.Drawing.Size(113, 56);
            // 
            // 保存ToolStripMenuItem
            // 
            this.保存ToolStripMenuItem.Name = "保存ToolStripMenuItem";
            this.保存ToolStripMenuItem.Size = new System.Drawing.Size(112, 26);
            this.保存ToolStripMenuItem.Text = "保存";
            this.保存ToolStripMenuItem.Click += new System.EventHandler(this.保存ToolStripMenuItem_Click);
            // 
            // 清空ToolStripMenuItem
            // 
            this.清空ToolStripMenuItem.Name = "清空ToolStripMenuItem";
            this.清空ToolStripMenuItem.Size = new System.Drawing.Size(112, 26);
            this.清空ToolStripMenuItem.Text = "清空";
            this.清空ToolStripMenuItem.Click += new System.EventHandler(this.清空ToolStripMenuItem_Click);
            // 
            // UcSingleChannel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.uiGroupBox1);
            this.Name = "UcSingleChannel";
            this.Size = new System.Drawing.Size(702, 226);
            this.uiGroupBox1.ResumeLayout(false);
            this.uiTabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.uiTableLayoutPanel1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiDataGridView1)).EndInit();
            this.uiContextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UIGroupBox uiGroupBox1;
        private Sunny.UI.UITabControl uiTabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private Sunny.UI.UITableLayoutPanel uiTableLayoutPanel1;
        private Sunny.UI.UITextBox txtPsi5Data;
        private Sunny.UI.UIMarkLabel uiMarkLabel6;
        private Sunny.UI.UITextBox txtErrorReporting;
        private Sunny.UI.UITextBox txt14BitData;
        private Sunny.UI.UITextBox txtStatusBit;
        private Sunny.UI.UIMarkLabel uiMarkLabel5;
        private Sunny.UI.UIMarkLabel uiMarkLabel4;
        private Sunny.UI.UIMarkLabel uiMarkLabel3;
        private Sunny.UI.UIMarkLabel uiMarkLabel2;
        private Sunny.UI.UIButton btnReadPsi5Data;
        private Sunny.UI.UIComboBox cmbBitLen;
        private Sunny.UI.UIButton btnInitChannel;
        private Sunny.UI.UIMarkLabel uiMarkLabel1;
        private Sunny.UI.UIProcessBar uiProcessBar1;
        private Sunny.UI.UITextBox txtRollingCount;
        private System.Windows.Forms.TabPage tabPage2;
        private Sunny.UI.UIDataGridView uiDataGridView1;
        private Sunny.UI.UIContextMenuStrip uiContextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 保存ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 清空ToolStripMenuItem;
    }
}
