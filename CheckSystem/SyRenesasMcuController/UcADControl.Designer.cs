namespace CheckSystem.SyRenesasMcuController
{
    partial class UcAdControl
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
            this.btnSlaveReadADs = new Sunny.UI.UIButton();
            this.lblSlaveId = new Sunny.UI.UILabel();
            this.uiTableLayoutPanel1 = new Sunny.UI.UITableLayoutPanel();
            this.curr6 = new Sunny.UI.UITextBox();
            this.curr5 = new Sunny.UI.UITextBox();
            this.curr4 = new Sunny.UI.UITextBox();
            this.curr3 = new Sunny.UI.UITextBox();
            this.curr2 = new Sunny.UI.UITextBox();
            this.uiLabel9 = new Sunny.UI.UILabel();
            this.uiLabel8 = new Sunny.UI.UILabel();
            this.uiLabel7 = new Sunny.UI.UILabel();
            this.uiLabel6 = new Sunny.UI.UILabel();
            this.uiLabel5 = new Sunny.UI.UILabel();
            this.uiLabel4 = new Sunny.UI.UILabel();
            this.uiLabel3 = new Sunny.UI.UILabel();
            this.uiLabel2 = new Sunny.UI.UILabel();
            this.curr1 = new Sunny.UI.UITextBox();
            this.res1 = new Sunny.UI.UITextBox();
            this.volt2 = new Sunny.UI.UITextBox();
            this.volt3 = new Sunny.UI.UITextBox();
            this.volt4 = new Sunny.UI.UITextBox();
            this.volt5 = new Sunny.UI.UITextBox();
            this.volt6 = new Sunny.UI.UITextBox();
            this.uiLabel10 = new Sunny.UI.UILabel();
            this.uiLabel11 = new Sunny.UI.UILabel();
            this.uiLabel12 = new Sunny.UI.UILabel();
            this.uiLabel13 = new Sunny.UI.UILabel();
            this.uiLabel14 = new Sunny.UI.UILabel();
            this.volt7 = new Sunny.UI.UITextBox();
            this.uiTableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSlaveReadADs
            // 
            this.btnSlaveReadADs.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSlaveReadADs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSlaveReadADs.Font = new System.Drawing.Font("宋体", 8.45F);
            this.btnSlaveReadADs.Location = new System.Drawing.Point(620, 4);
            this.btnSlaveReadADs.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnSlaveReadADs.Name = "btnSlaveReadADs";
            this.uiTableLayoutPanel1.SetRowSpan(this.btnSlaveReadADs, 2);
            this.btnSlaveReadADs.Size = new System.Drawing.Size(83, 57);
            this.btnSlaveReadADs.TabIndex = 14;
            this.btnSlaveReadADs.Text = "刷新当前从站";
            this.btnSlaveReadADs.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSlaveReadADs.Click += new System.EventHandler(this.btnSlaveReadADs_Click);
            // 
            // lblSlaveId
            // 
            this.lblSlaveId.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblSlaveId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSlaveId.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSlaveId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.lblSlaveId.Location = new System.Drawing.Point(1, 1);
            this.lblSlaveId.Margin = new System.Windows.Forms.Padding(0);
            this.lblSlaveId.Name = "lblSlaveId";
            this.uiTableLayoutPanel1.SetRowSpan(this.lblSlaveId, 4);
            this.lblSlaveId.Size = new System.Drawing.Size(87, 127);
            this.lblSlaveId.TabIndex = 1;
            this.lblSlaveId.Text = "0x101:";
            this.lblSlaveId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiTableLayoutPanel1
            // 
            this.uiTableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.uiTableLayoutPanel1.ColumnCount = 8;
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.uiTableLayoutPanel1.Controls.Add(this.curr6, 6, 1);
            this.uiTableLayoutPanel1.Controls.Add(this.curr5, 5, 1);
            this.uiTableLayoutPanel1.Controls.Add(this.curr4, 4, 1);
            this.uiTableLayoutPanel1.Controls.Add(this.curr3, 3, 1);
            this.uiTableLayoutPanel1.Controls.Add(this.curr2, 2, 1);
            this.uiTableLayoutPanel1.Controls.Add(this.uiLabel9, 2, 2);
            this.uiTableLayoutPanel1.Controls.Add(this.uiLabel8, 1, 2);
            this.uiTableLayoutPanel1.Controls.Add(this.uiLabel7, 6, 0);
            this.uiTableLayoutPanel1.Controls.Add(this.uiLabel6, 5, 0);
            this.uiTableLayoutPanel1.Controls.Add(this.uiLabel5, 4, 0);
            this.uiTableLayoutPanel1.Controls.Add(this.uiLabel4, 3, 0);
            this.uiTableLayoutPanel1.Controls.Add(this.uiLabel3, 2, 0);
            this.uiTableLayoutPanel1.Controls.Add(this.uiLabel2, 1, 0);
            this.uiTableLayoutPanel1.Controls.Add(this.lblSlaveId, 0, 0);
            this.uiTableLayoutPanel1.Controls.Add(this.btnSlaveReadADs, 7, 0);
            this.uiTableLayoutPanel1.Controls.Add(this.curr1, 1, 1);
            this.uiTableLayoutPanel1.Controls.Add(this.res1, 1, 3);
            this.uiTableLayoutPanel1.Controls.Add(this.volt2, 2, 3);
            this.uiTableLayoutPanel1.Controls.Add(this.volt3, 3, 3);
            this.uiTableLayoutPanel1.Controls.Add(this.volt4, 4, 3);
            this.uiTableLayoutPanel1.Controls.Add(this.volt5, 5, 3);
            this.uiTableLayoutPanel1.Controls.Add(this.volt6, 6, 3);
            this.uiTableLayoutPanel1.Controls.Add(this.uiLabel10, 3, 2);
            this.uiTableLayoutPanel1.Controls.Add(this.uiLabel11, 4, 2);
            this.uiTableLayoutPanel1.Controls.Add(this.uiLabel12, 5, 2);
            this.uiTableLayoutPanel1.Controls.Add(this.uiLabel13, 6, 2);
            this.uiTableLayoutPanel1.Controls.Add(this.uiLabel14, 7, 2);
            this.uiTableLayoutPanel1.Controls.Add(this.volt7, 7, 3);
            this.uiTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.uiTableLayoutPanel1.Name = "uiTableLayoutPanel1";
            this.uiTableLayoutPanel1.RowCount = 4;
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.uiTableLayoutPanel1.Size = new System.Drawing.Size(707, 129);
            this.uiTableLayoutPanel1.TabIndex = 0;
            this.uiTableLayoutPanel1.TagString = null;
            // 
            // curr6
            // 
            this.curr6.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.curr6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.curr6.Font = new System.Drawing.Font("宋体", 10F);
            this.curr6.Location = new System.Drawing.Point(533, 38);
            this.curr6.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.curr6.MinimumSize = new System.Drawing.Size(1, 16);
            this.curr6.Name = "curr6";
            this.curr6.Padding = new System.Windows.Forms.Padding(5);
            this.curr6.ReadOnly = true;
            this.curr6.ShowText = false;
            this.curr6.Size = new System.Drawing.Size(79, 21);
            this.curr6.TabIndex = 28;
            this.curr6.Tag = "Current6";
            this.curr6.Text = "0.00";
            this.curr6.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.curr6.Type = Sunny.UI.UITextBox.UIEditType.Double;
            this.curr6.Watermark = "";
            // 
            // curr5
            // 
            this.curr5.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.curr5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.curr5.Font = new System.Drawing.Font("宋体", 10F);
            this.curr5.Location = new System.Drawing.Point(445, 38);
            this.curr5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.curr5.MinimumSize = new System.Drawing.Size(1, 16);
            this.curr5.Name = "curr5";
            this.curr5.Padding = new System.Windows.Forms.Padding(5);
            this.curr5.ReadOnly = true;
            this.curr5.ShowText = false;
            this.curr5.Size = new System.Drawing.Size(79, 21);
            this.curr5.TabIndex = 27;
            this.curr5.Tag = "Current5";
            this.curr5.Text = "0.00";
            this.curr5.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.curr5.Type = Sunny.UI.UITextBox.UIEditType.Double;
            this.curr5.Watermark = "";
            // 
            // curr4
            // 
            this.curr4.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.curr4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.curr4.Font = new System.Drawing.Font("宋体", 10F);
            this.curr4.Location = new System.Drawing.Point(357, 38);
            this.curr4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.curr4.MinimumSize = new System.Drawing.Size(1, 16);
            this.curr4.Name = "curr4";
            this.curr4.Padding = new System.Windows.Forms.Padding(5);
            this.curr4.ReadOnly = true;
            this.curr4.ShowText = false;
            this.curr4.Size = new System.Drawing.Size(79, 21);
            this.curr4.TabIndex = 26;
            this.curr4.Tag = "Current4";
            this.curr4.Text = "0.00";
            this.curr4.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.curr4.Type = Sunny.UI.UITextBox.UIEditType.Double;
            this.curr4.Watermark = "";
            // 
            // curr3
            // 
            this.curr3.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.curr3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.curr3.Font = new System.Drawing.Font("宋体", 10F);
            this.curr3.Location = new System.Drawing.Point(269, 38);
            this.curr3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.curr3.MinimumSize = new System.Drawing.Size(1, 16);
            this.curr3.Name = "curr3";
            this.curr3.Padding = new System.Windows.Forms.Padding(5);
            this.curr3.ReadOnly = true;
            this.curr3.ShowText = false;
            this.curr3.Size = new System.Drawing.Size(79, 21);
            this.curr3.TabIndex = 25;
            this.curr3.Tag = "Current3";
            this.curr3.Text = "0.00";
            this.curr3.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.curr3.Type = Sunny.UI.UITextBox.UIEditType.Double;
            this.curr3.Watermark = "";
            // 
            // curr2
            // 
            this.curr2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.curr2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.curr2.Font = new System.Drawing.Font("宋体", 10F);
            this.curr2.Location = new System.Drawing.Point(181, 38);
            this.curr2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.curr2.MinimumSize = new System.Drawing.Size(1, 16);
            this.curr2.Name = "curr2";
            this.curr2.Padding = new System.Windows.Forms.Padding(5);
            this.curr2.ReadOnly = true;
            this.curr2.ShowText = false;
            this.curr2.Size = new System.Drawing.Size(79, 21);
            this.curr2.TabIndex = 24;
            this.curr2.Tag = "Current2";
            this.curr2.Text = "0.00";
            this.curr2.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.curr2.Type = Sunny.UI.UITextBox.UIEditType.Double;
            this.curr2.Watermark = "";
            // 
            // uiLabel9
            // 
            this.uiLabel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiLabel9.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel9.Location = new System.Drawing.Point(177, 65);
            this.uiLabel9.Margin = new System.Windows.Forms.Padding(0);
            this.uiLabel9.Name = "uiLabel9";
            this.uiLabel9.Size = new System.Drawing.Size(87, 31);
            this.uiLabel9.TabIndex = 22;
            this.uiLabel9.Text = "Volt2:";
            this.uiLabel9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel8
            // 
            this.uiLabel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiLabel8.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel8.Location = new System.Drawing.Point(89, 65);
            this.uiLabel8.Margin = new System.Windows.Forms.Padding(0);
            this.uiLabel8.Name = "uiLabel8";
            this.uiLabel8.Size = new System.Drawing.Size(87, 31);
            this.uiLabel8.TabIndex = 21;
            this.uiLabel8.Text = "Res1:";
            this.uiLabel8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel7
            // 
            this.uiLabel7.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel7.Location = new System.Drawing.Point(529, 1);
            this.uiLabel7.Margin = new System.Windows.Forms.Padding(0);
            this.uiLabel7.Name = "uiLabel7";
            this.uiLabel7.Size = new System.Drawing.Size(77, 31);
            this.uiLabel7.TabIndex = 20;
            this.uiLabel7.Text = "Curr6:";
            this.uiLabel7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel6
            // 
            this.uiLabel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiLabel6.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel6.Location = new System.Drawing.Point(441, 1);
            this.uiLabel6.Margin = new System.Windows.Forms.Padding(0);
            this.uiLabel6.Name = "uiLabel6";
            this.uiLabel6.Size = new System.Drawing.Size(87, 31);
            this.uiLabel6.TabIndex = 19;
            this.uiLabel6.Text = "Curr5:";
            this.uiLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel5
            // 
            this.uiLabel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiLabel5.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel5.Location = new System.Drawing.Point(353, 1);
            this.uiLabel5.Margin = new System.Windows.Forms.Padding(0);
            this.uiLabel5.Name = "uiLabel5";
            this.uiLabel5.Size = new System.Drawing.Size(87, 31);
            this.uiLabel5.TabIndex = 18;
            this.uiLabel5.Text = "Curr4:";
            this.uiLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel4
            // 
            this.uiLabel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiLabel4.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel4.Location = new System.Drawing.Point(265, 1);
            this.uiLabel4.Margin = new System.Windows.Forms.Padding(0);
            this.uiLabel4.Name = "uiLabel4";
            this.uiLabel4.Size = new System.Drawing.Size(87, 31);
            this.uiLabel4.TabIndex = 17;
            this.uiLabel4.Text = "Curr3:";
            this.uiLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel3
            // 
            this.uiLabel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiLabel3.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel3.Location = new System.Drawing.Point(177, 1);
            this.uiLabel3.Margin = new System.Windows.Forms.Padding(0);
            this.uiLabel3.Name = "uiLabel3";
            this.uiLabel3.Size = new System.Drawing.Size(87, 31);
            this.uiLabel3.TabIndex = 16;
            this.uiLabel3.Text = "Curr2:";
            this.uiLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel2
            // 
            this.uiLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiLabel2.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel2.Location = new System.Drawing.Point(89, 1);
            this.uiLabel2.Margin = new System.Windows.Forms.Padding(0);
            this.uiLabel2.Name = "uiLabel2";
            this.uiLabel2.Size = new System.Drawing.Size(87, 31);
            this.uiLabel2.TabIndex = 15;
            this.uiLabel2.Text = "Curr1:";
            this.uiLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // curr1
            // 
            this.curr1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.curr1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.curr1.Font = new System.Drawing.Font("宋体", 10F);
            this.curr1.Location = new System.Drawing.Point(93, 38);
            this.curr1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.curr1.MinimumSize = new System.Drawing.Size(1, 16);
            this.curr1.Name = "curr1";
            this.curr1.Padding = new System.Windows.Forms.Padding(5);
            this.curr1.ReadOnly = true;
            this.curr1.ShowText = false;
            this.curr1.Size = new System.Drawing.Size(79, 21);
            this.curr1.TabIndex = 23;
            this.curr1.Tag = "Current1";
            this.curr1.Text = "0.00";
            this.curr1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.curr1.Type = Sunny.UI.UITextBox.UIEditType.Double;
            this.curr1.Watermark = "";
            // 
            // res1
            // 
            this.res1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.res1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.res1.Font = new System.Drawing.Font("宋体", 10F);
            this.res1.Location = new System.Drawing.Point(93, 102);
            this.res1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.res1.MinimumSize = new System.Drawing.Size(1, 16);
            this.res1.Name = "res1";
            this.res1.Padding = new System.Windows.Forms.Padding(5);
            this.res1.ReadOnly = true;
            this.res1.ShowText = false;
            this.res1.Size = new System.Drawing.Size(79, 21);
            this.res1.TabIndex = 29;
            this.res1.Tag = "Resistance1";
            this.res1.Text = "0.00";
            this.res1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.res1.Type = Sunny.UI.UITextBox.UIEditType.Double;
            this.res1.Watermark = "";
            // 
            // volt2
            // 
            this.volt2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.volt2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.volt2.Font = new System.Drawing.Font("宋体", 10F);
            this.volt2.Location = new System.Drawing.Point(181, 102);
            this.volt2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.volt2.MinimumSize = new System.Drawing.Size(1, 16);
            this.volt2.Name = "volt2";
            this.volt2.Padding = new System.Windows.Forms.Padding(5);
            this.volt2.ReadOnly = true;
            this.volt2.ShowText = false;
            this.volt2.Size = new System.Drawing.Size(79, 21);
            this.volt2.TabIndex = 31;
            this.volt2.Tag = "Voltage2";
            this.volt2.Text = "0.00";
            this.volt2.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.volt2.Type = Sunny.UI.UITextBox.UIEditType.Double;
            this.volt2.Watermark = "";
            // 
            // volt3
            // 
            this.volt3.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.volt3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.volt3.Font = new System.Drawing.Font("宋体", 10F);
            this.volt3.Location = new System.Drawing.Point(269, 102);
            this.volt3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.volt3.MinimumSize = new System.Drawing.Size(1, 16);
            this.volt3.Name = "volt3";
            this.volt3.Padding = new System.Windows.Forms.Padding(5);
            this.volt3.ReadOnly = true;
            this.volt3.ShowText = false;
            this.volt3.Size = new System.Drawing.Size(79, 21);
            this.volt3.TabIndex = 30;
            this.volt3.Tag = "Voltage3";
            this.volt3.Text = "0.00";
            this.volt3.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.volt3.Type = Sunny.UI.UITextBox.UIEditType.Double;
            this.volt3.Watermark = "";
            // 
            // volt4
            // 
            this.volt4.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.volt4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.volt4.Font = new System.Drawing.Font("宋体", 10F);
            this.volt4.Location = new System.Drawing.Point(357, 102);
            this.volt4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.volt4.MinimumSize = new System.Drawing.Size(1, 16);
            this.volt4.Name = "volt4";
            this.volt4.Padding = new System.Windows.Forms.Padding(5);
            this.volt4.ReadOnly = true;
            this.volt4.ShowText = false;
            this.volt4.Size = new System.Drawing.Size(79, 21);
            this.volt4.TabIndex = 32;
            this.volt4.Tag = "Voltage4";
            this.volt4.Text = "0.00";
            this.volt4.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.volt4.Type = Sunny.UI.UITextBox.UIEditType.Double;
            this.volt4.Watermark = "";
            // 
            // volt5
            // 
            this.volt5.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.volt5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.volt5.Font = new System.Drawing.Font("宋体", 10F);
            this.volt5.Location = new System.Drawing.Point(445, 102);
            this.volt5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.volt5.MinimumSize = new System.Drawing.Size(1, 16);
            this.volt5.Name = "volt5";
            this.volt5.Padding = new System.Windows.Forms.Padding(5);
            this.volt5.ReadOnly = true;
            this.volt5.ShowText = false;
            this.volt5.Size = new System.Drawing.Size(79, 21);
            this.volt5.TabIndex = 33;
            this.volt5.Tag = "Voltage5";
            this.volt5.Text = "0.00";
            this.volt5.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.volt5.Type = Sunny.UI.UITextBox.UIEditType.Double;
            this.volt5.Watermark = "";
            // 
            // volt6
            // 
            this.volt6.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.volt6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.volt6.Font = new System.Drawing.Font("宋体", 10F);
            this.volt6.Location = new System.Drawing.Point(533, 102);
            this.volt6.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.volt6.MinimumSize = new System.Drawing.Size(1, 16);
            this.volt6.Name = "volt6";
            this.volt6.Padding = new System.Windows.Forms.Padding(5);
            this.volt6.ReadOnly = true;
            this.volt6.ShowText = false;
            this.volt6.Size = new System.Drawing.Size(79, 21);
            this.volt6.TabIndex = 34;
            this.volt6.Tag = "Voltage6";
            this.volt6.Text = "0.00";
            this.volt6.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.volt6.Type = Sunny.UI.UITextBox.UIEditType.Double;
            this.volt6.Watermark = "";
            // 
            // uiLabel10
            // 
            this.uiLabel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiLabel10.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel10.Location = new System.Drawing.Point(265, 65);
            this.uiLabel10.Margin = new System.Windows.Forms.Padding(0);
            this.uiLabel10.Name = "uiLabel10";
            this.uiLabel10.Size = new System.Drawing.Size(87, 31);
            this.uiLabel10.TabIndex = 22;
            this.uiLabel10.Text = "Volt3:";
            this.uiLabel10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel11
            // 
            this.uiLabel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiLabel11.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel11.Location = new System.Drawing.Point(353, 65);
            this.uiLabel11.Margin = new System.Windows.Forms.Padding(0);
            this.uiLabel11.Name = "uiLabel11";
            this.uiLabel11.Size = new System.Drawing.Size(87, 31);
            this.uiLabel11.TabIndex = 22;
            this.uiLabel11.Text = "Volt4:";
            this.uiLabel11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel12
            // 
            this.uiLabel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiLabel12.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel12.Location = new System.Drawing.Point(441, 65);
            this.uiLabel12.Margin = new System.Windows.Forms.Padding(0);
            this.uiLabel12.Name = "uiLabel12";
            this.uiLabel12.Size = new System.Drawing.Size(87, 31);
            this.uiLabel12.TabIndex = 22;
            this.uiLabel12.Text = "Volt5:";
            this.uiLabel12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel13
            // 
            this.uiLabel13.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel13.Location = new System.Drawing.Point(529, 65);
            this.uiLabel13.Margin = new System.Windows.Forms.Padding(0);
            this.uiLabel13.Name = "uiLabel13";
            this.uiLabel13.Size = new System.Drawing.Size(77, 31);
            this.uiLabel13.TabIndex = 22;
            this.uiLabel13.Text = "Volt6:";
            this.uiLabel13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel14
            // 
            this.uiLabel14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiLabel14.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel14.Location = new System.Drawing.Point(617, 65);
            this.uiLabel14.Margin = new System.Windows.Forms.Padding(0);
            this.uiLabel14.Name = "uiLabel14";
            this.uiLabel14.Size = new System.Drawing.Size(89, 31);
            this.uiLabel14.TabIndex = 22;
            this.uiLabel14.Text = "Volt7:";
            this.uiLabel14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // volt7
            // 
            this.volt7.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.volt7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.volt7.Font = new System.Drawing.Font("宋体", 10F);
            this.volt7.Location = new System.Drawing.Point(621, 102);
            this.volt7.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.volt7.MinimumSize = new System.Drawing.Size(1, 16);
            this.volt7.Name = "volt7";
            this.volt7.Padding = new System.Windows.Forms.Padding(5);
            this.volt7.ReadOnly = true;
            this.volt7.ShowText = false;
            this.volt7.Size = new System.Drawing.Size(81, 21);
            this.volt7.TabIndex = 34;
            this.volt7.Tag = "Voltage7";
            this.volt7.Text = "0.00";
            this.volt7.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.volt7.Type = Sunny.UI.UITextBox.UIEditType.Double;
            this.volt7.Watermark = "";
            // 
            // UcAdControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.uiTableLayoutPanel1);
            this.Name = "UcAdControl";
            this.Size = new System.Drawing.Size(707, 129);
            this.uiTableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UIButton btnSlaveReadADs;
        private Sunny.UI.UITableLayoutPanel uiTableLayoutPanel1;
        private Sunny.UI.UILabel lblSlaveId;
        private Sunny.UI.UILabel uiLabel9;
        private Sunny.UI.UILabel uiLabel8;
        private Sunny.UI.UILabel uiLabel7;
        private Sunny.UI.UILabel uiLabel6;
        private Sunny.UI.UILabel uiLabel5;
        private Sunny.UI.UILabel uiLabel4;
        private Sunny.UI.UILabel uiLabel3;
        private Sunny.UI.UILabel uiLabel2;
        private Sunny.UI.UITextBox curr1;
        private Sunny.UI.UITextBox curr6;
        private Sunny.UI.UITextBox curr5;
        private Sunny.UI.UITextBox curr4;
        private Sunny.UI.UITextBox curr3;
        private Sunny.UI.UITextBox curr2;
        private Sunny.UI.UITextBox res1;
        private Sunny.UI.UITextBox volt2;
        private Sunny.UI.UITextBox volt3;
        private Sunny.UI.UITextBox volt4;
        private Sunny.UI.UITextBox volt5;
        private Sunny.UI.UITextBox volt6;
        private Sunny.UI.UILabel uiLabel10;
        private Sunny.UI.UILabel uiLabel11;
        private Sunny.UI.UILabel uiLabel12;
        private Sunny.UI.UILabel uiLabel13;
        private Sunny.UI.UILabel uiLabel14;
        private Sunny.UI.UITextBox volt7;
    }
}
