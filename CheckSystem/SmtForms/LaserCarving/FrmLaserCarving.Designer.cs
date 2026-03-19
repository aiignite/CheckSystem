namespace CheckSystem.SmtForms.LaserCarving
{
    partial class FrmLaserCarving
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
            this.cmbDeviceSn = new Sunny.UI.UIComboBox();
            this.uiMarkLabel1 = new Sunny.UI.UIMarkLabel();
            this.dgvCodeDetail = new Sunny.UI.UIDataGridView();
            this.btnConfig = new Sunny.UI.UISymbolButton();
            this.txtDeviceSn = new Sunny.UI.UITextBox();
            this.txtTrayNo = new Sunny.UI.UITextBox();
            this.txtGroupName = new Sunny.UI.UITextBox();
            this.txtProgramName = new Sunny.UI.UITextBox();
            this.btnClearError = new Sunny.UI.UISymbolButton();
            this.ledStart = new Sunny.UI.UILedBulb();
            this.ledComplete = new Sunny.UI.UILedBulb();
            this.ledError = new Sunny.UI.UILedBulb();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCodeDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbDeviceSn
            // 
            this.cmbDeviceSn.DataSource = null;
            this.cmbDeviceSn.Enabled = false;
            this.cmbDeviceSn.FillColor = System.Drawing.Color.White;
            this.cmbDeviceSn.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbDeviceSn.Location = new System.Drawing.Point(113, 45);
            this.cmbDeviceSn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbDeviceSn.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmbDeviceSn.Name = "cmbDeviceSn";
            this.cmbDeviceSn.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbDeviceSn.Size = new System.Drawing.Size(368, 29);
            this.cmbDeviceSn.TabIndex = 0;
            this.cmbDeviceSn.Text = "uiComboBox1";
            this.cmbDeviceSn.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbDeviceSn.Watermark = "";
            // 
            // uiMarkLabel1
            // 
            this.uiMarkLabel1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiMarkLabel1.Location = new System.Drawing.Point(13, 45);
            this.uiMarkLabel1.Name = "uiMarkLabel1";
            this.uiMarkLabel1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel1.Size = new System.Drawing.Size(100, 23);
            this.uiMarkLabel1.TabIndex = 1;
            this.uiMarkLabel1.Text = "设备号：";
            this.uiMarkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvCodeDetail
            // 
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            this.dgvCodeDetail.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvCodeDetail.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            this.dgvCodeDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCodeDetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvCodeDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(236)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCodeDetail.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgvCodeDetail.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvCodeDetail.EnableHeadersVisualStyles = false;
            this.dgvCodeDetail.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgvCodeDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(173)))), ((int)(((byte)(255)))));
            this.dgvCodeDetail.Location = new System.Drawing.Point(0, 140);
            this.dgvCodeDetail.Name = "dgvCodeDetail";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle9.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCodeDetail.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(236)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.dgvCodeDetail.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.dgvCodeDetail.RowTemplate.Height = 23;
            this.dgvCodeDetail.ScrollBarRectColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            this.dgvCodeDetail.SelectedIndex = -1;
            this.dgvCodeDetail.Size = new System.Drawing.Size(800, 340);
            this.dgvCodeDetail.TabIndex = 3;
            // 
            // btnConfig
            // 
            this.btnConfig.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConfig.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnConfig.Location = new System.Drawing.Point(664, 45);
            this.btnConfig.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Size = new System.Drawing.Size(100, 35);
            this.btnConfig.Style = Sunny.UI.UIStyle.Custom;
            this.btnConfig.StyleCustomMode = true;
            this.btnConfig.Symbol = 61529;
            this.btnConfig.TabIndex = 85;
            this.btnConfig.Text = "Config";
            this.btnConfig.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // txtDeviceSn
            // 
            this.txtDeviceSn.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtDeviceSn.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtDeviceSn.Location = new System.Drawing.Point(4, 95);
            this.txtDeviceSn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtDeviceSn.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtDeviceSn.Name = "txtDeviceSn";
            this.txtDeviceSn.ReadOnly = true;
            this.txtDeviceSn.ShowText = false;
            this.txtDeviceSn.Size = new System.Drawing.Size(150, 29);
            this.txtDeviceSn.TabIndex = 88;
            this.txtDeviceSn.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtDeviceSn.Watermark = "DEVICE_SN";
            // 
            // txtTrayNo
            // 
            this.txtTrayNo.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTrayNo.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtTrayNo.Location = new System.Drawing.Point(162, 95);
            this.txtTrayNo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTrayNo.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtTrayNo.Name = "txtTrayNo";
            this.txtTrayNo.ReadOnly = true;
            this.txtTrayNo.ShowText = false;
            this.txtTrayNo.Size = new System.Drawing.Size(150, 29);
            this.txtTrayNo.TabIndex = 88;
            this.txtTrayNo.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtTrayNo.Watermark = "TRAYNO";
            // 
            // txtGroupName
            // 
            this.txtGroupName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtGroupName.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtGroupName.Location = new System.Drawing.Point(320, 95);
            this.txtGroupName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtGroupName.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtGroupName.Name = "txtGroupName";
            this.txtGroupName.ReadOnly = true;
            this.txtGroupName.ShowText = false;
            this.txtGroupName.Size = new System.Drawing.Size(150, 29);
            this.txtGroupName.TabIndex = 88;
            this.txtGroupName.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtGroupName.Watermark = "GROUP_NUM";
            // 
            // txtProgramName
            // 
            this.txtProgramName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtProgramName.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.txtProgramName.Location = new System.Drawing.Point(478, 95);
            this.txtProgramName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtProgramName.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtProgramName.Name = "txtProgramName";
            this.txtProgramName.ReadOnly = true;
            this.txtProgramName.ShowText = false;
            this.txtProgramName.Size = new System.Drawing.Size(150, 29);
            this.txtProgramName.TabIndex = 88;
            this.txtProgramName.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtProgramName.Watermark = "PROGRAM_NAME";
            // 
            // btnClearError
            // 
            this.btnClearError.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClearError.Enabled = false;
            this.btnClearError.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.btnClearError.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.btnClearError.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(175)))), ((int)(((byte)(83)))));
            this.btnClearError.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.btnClearError.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.btnClearError.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnClearError.Location = new System.Drawing.Point(504, 45);
            this.btnClearError.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnClearError.Name = "btnClearError";
            this.btnClearError.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))));
            this.btnClearError.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(175)))), ((int)(((byte)(83)))));
            this.btnClearError.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.btnClearError.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(124)))), ((int)(((byte)(32)))));
            this.btnClearError.Size = new System.Drawing.Size(142, 35);
            this.btnClearError.Style = Sunny.UI.UIStyle.Orange;
            this.btnClearError.StyleCustomMode = true;
            this.btnClearError.Symbol = 61553;
            this.btnClearError.TabIndex = 89;
            this.btnClearError.Text = "Clear Error";
            this.btnClearError.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClearError.Click += new System.EventHandler(this.btnClearError_Click);
            // 
            // ledStart
            // 
            this.ledStart.Location = new System.Drawing.Point(652, 95);
            this.ledStart.Name = "ledStart";
            this.ledStart.Size = new System.Drawing.Size(32, 32);
            this.ledStart.TabIndex = 90;
            this.ledStart.Text = "uiLedBulb1";
            // 
            // ledComplete
            // 
            this.ledComplete.Location = new System.Drawing.Point(699, 95);
            this.ledComplete.Name = "ledComplete";
            this.ledComplete.Size = new System.Drawing.Size(32, 32);
            this.ledComplete.TabIndex = 90;
            this.ledComplete.Text = "uiLedBulb1";
            // 
            // ledError
            // 
            this.ledError.Color = System.Drawing.Color.Red;
            this.ledError.Location = new System.Drawing.Point(746, 95);
            this.ledError.Name = "ledError";
            this.ledError.Size = new System.Drawing.Size(32, 32);
            this.ledError.TabIndex = 90;
            this.ledError.Text = "uiLedBulb1";
            // 
            // FrmLaserCarving
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(800, 480);
            this.Controls.Add(this.ledError);
            this.Controls.Add(this.ledComplete);
            this.Controls.Add(this.ledStart);
            this.Controls.Add(this.btnClearError);
            this.Controls.Add(this.txtProgramName);
            this.Controls.Add(this.txtGroupName);
            this.Controls.Add(this.txtTrayNo);
            this.Controls.Add(this.txtDeviceSn);
            this.Controls.Add(this.btnConfig);
            this.Controls.Add(this.dgvCodeDetail);
            this.Controls.Add(this.uiMarkLabel1);
            this.Controls.Add(this.cmbDeviceSn);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(800, 480);
            this.Name = "FrmLaserCarving";
            this.Text = "SMT激光打标";
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 800, 480);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCodeDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UIComboBox cmbDeviceSn;
        private Sunny.UI.UIMarkLabel uiMarkLabel1;
        private Sunny.UI.UIDataGridView dgvCodeDetail;
        private Sunny.UI.UISymbolButton btnConfig;
        private Sunny.UI.UITextBox txtDeviceSn;
        private Sunny.UI.UITextBox txtTrayNo;
        private Sunny.UI.UITextBox txtGroupName;
        private Sunny.UI.UITextBox txtProgramName;
        private Sunny.UI.UISymbolButton btnClearError;
        private Sunny.UI.UILedBulb ledStart;
        private Sunny.UI.UILedBulb ledComplete;
        private Sunny.UI.UILedBulb ledError;

    }
}