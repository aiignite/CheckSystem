namespace CheckSystem.HelperForms.Hvsm
{
    partial class FrmTempMonitorSet
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
            this.uiMarkLabel1 = new Sunny.UI.UIMarkLabel();
            this.cmbIsEnable = new Sunny.UI.UIComboBox();
            this.uiMarkLabel2 = new Sunny.UI.UIMarkLabel();
            this.cmbSymbol = new Sunny.UI.UIComboBox();
            this.uiMarkLabel3 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel4 = new Sunny.UI.UIMarkLabel();
            this.btnUpdate = new Sunny.UI.UISymbolButton();
            this.numValue = new System.Windows.Forms.NumericUpDown();
            this.cmbNtcChannel = new Sunny.UI.UIComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.numValue)).BeginInit();
            this.SuspendLayout();
            // 
            // uiMarkLabel1
            // 
            this.uiMarkLabel1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiMarkLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel1.Location = new System.Drawing.Point(29, 53);
            this.uiMarkLabel1.Name = "uiMarkLabel1";
            this.uiMarkLabel1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel1.Size = new System.Drawing.Size(100, 23);
            this.uiMarkLabel1.TabIndex = 0;
            this.uiMarkLabel1.Text = "是否起用：";
            this.uiMarkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbIsEnable
            // 
            this.cmbIsEnable.DataSource = null;
            this.cmbIsEnable.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cmbIsEnable.FillColor = System.Drawing.Color.White;
            this.cmbIsEnable.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbIsEnable.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.cmbIsEnable.Items.AddRange(new object[] {
            "禁用",
            "启用"});
            this.cmbIsEnable.ItemSelectForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.cmbIsEnable.Location = new System.Drawing.Point(136, 47);
            this.cmbIsEnable.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbIsEnable.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmbIsEnable.Name = "cmbIsEnable";
            this.cmbIsEnable.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbIsEnable.Size = new System.Drawing.Size(150, 29);
            this.cmbIsEnable.SymbolSize = 24;
            this.cmbIsEnable.TabIndex = 1;
            this.cmbIsEnable.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbIsEnable.Watermark = "";
            // 
            // uiMarkLabel2
            // 
            this.uiMarkLabel2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiMarkLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel2.Location = new System.Drawing.Point(29, 99);
            this.uiMarkLabel2.Name = "uiMarkLabel2";
            this.uiMarkLabel2.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel2.Size = new System.Drawing.Size(100, 23);
            this.uiMarkLabel2.TabIndex = 0;
            this.uiMarkLabel2.Text = "判断符号：";
            this.uiMarkLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbSymbol
            // 
            this.cmbSymbol.DataSource = null;
            this.cmbSymbol.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cmbSymbol.FillColor = System.Drawing.Color.White;
            this.cmbSymbol.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbSymbol.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.cmbSymbol.Items.AddRange(new object[] {
            ">",
            "<",
            ">=",
            "<="});
            this.cmbSymbol.ItemSelectForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.cmbSymbol.Location = new System.Drawing.Point(136, 93);
            this.cmbSymbol.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbSymbol.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmbSymbol.Name = "cmbSymbol";
            this.cmbSymbol.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbSymbol.Size = new System.Drawing.Size(150, 29);
            this.cmbSymbol.SymbolSize = 24;
            this.cmbSymbol.TabIndex = 1;
            this.cmbSymbol.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbSymbol.Watermark = "";
            // 
            // uiMarkLabel3
            // 
            this.uiMarkLabel3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiMarkLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel3.Location = new System.Drawing.Point(29, 139);
            this.uiMarkLabel3.Name = "uiMarkLabel3";
            this.uiMarkLabel3.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel3.Size = new System.Drawing.Size(100, 23);
            this.uiMarkLabel3.TabIndex = 0;
            this.uiMarkLabel3.Text = "阈值：";
            this.uiMarkLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiMarkLabel4
            // 
            this.uiMarkLabel4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiMarkLabel4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel4.Location = new System.Drawing.Point(29, 188);
            this.uiMarkLabel4.Name = "uiMarkLabel4";
            this.uiMarkLabel4.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel4.Size = new System.Drawing.Size(100, 23);
            this.uiMarkLabel4.TabIndex = 0;
            this.uiMarkLabel4.Text = "NTC通道：";
            this.uiMarkLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUpdate.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUpdate.Location = new System.Drawing.Point(64, 229);
            this.btnUpdate.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(162, 35);
            this.btnUpdate.TabIndex = 2;
            this.btnUpdate.Text = "更新";
            this.btnUpdate.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // numValue
            // 
            this.numValue.DecimalPlaces = 2;
            this.numValue.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numValue.Location = new System.Drawing.Point(136, 139);
            this.numValue.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.numValue.Minimum = new decimal(new int[] {
            99999999,
            0,
            0,
            -2147483648});
            this.numValue.Name = "numValue";
            this.numValue.Size = new System.Drawing.Size(150, 26);
            this.numValue.TabIndex = 3;
            // 
            // cmbNtcChannel
            // 
            this.cmbNtcChannel.DataSource = null;
            this.cmbNtcChannel.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cmbNtcChannel.FillColor = System.Drawing.Color.White;
            this.cmbNtcChannel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbNtcChannel.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.cmbNtcChannel.ItemSelectForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.cmbNtcChannel.Location = new System.Drawing.Point(136, 182);
            this.cmbNtcChannel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbNtcChannel.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmbNtcChannel.Name = "cmbNtcChannel";
            this.cmbNtcChannel.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbNtcChannel.Size = new System.Drawing.Size(150, 29);
            this.cmbNtcChannel.SymbolSize = 24;
            this.cmbNtcChannel.TabIndex = 1;
            this.cmbNtcChannel.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbNtcChannel.Watermark = "";
            // 
            // FrmTempMonitorSet
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(315, 280);
            this.Controls.Add(this.numValue);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.cmbNtcChannel);
            this.Controls.Add(this.cmbSymbol);
            this.Controls.Add(this.cmbIsEnable);
            this.Controls.Add(this.uiMarkLabel3);
            this.Controls.Add(this.uiMarkLabel4);
            this.Controls.Add(this.uiMarkLabel2);
            this.Controls.Add(this.uiMarkLabel1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(315, 280);
            this.MinimumSize = new System.Drawing.Size(315, 280);
            this.Name = "FrmTempMonitorSet";
            this.Text = "温度监控参数设置";
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 800, 450);
            ((System.ComponentModel.ISupportInitialize)(this.numValue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UIMarkLabel uiMarkLabel1;
        private Sunny.UI.UIComboBox cmbIsEnable;
        private Sunny.UI.UIMarkLabel uiMarkLabel2;
        private Sunny.UI.UIComboBox cmbSymbol;
        private Sunny.UI.UIMarkLabel uiMarkLabel3;
        private Sunny.UI.UIMarkLabel uiMarkLabel4;
        private Sunny.UI.UISymbolButton btnUpdate;
        private System.Windows.Forms.NumericUpDown numValue;
        private Sunny.UI.UIComboBox cmbNtcChannel;
    }
}