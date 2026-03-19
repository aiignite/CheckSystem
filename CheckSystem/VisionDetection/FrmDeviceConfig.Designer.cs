namespace CheckSystem.VisionDetection
{
    partial class FrmDeviceConfig
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
            this.uiMarkLabel2 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel3 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel4 = new Sunny.UI.UIMarkLabel();
            this.cmbPowerType = new Sunny.UI.UIComboBox();
            this.cmbPowerAddress = new Sunny.UI.UIComboBox();
            this.cmbBarcodeScanAddress = new Sunny.UI.UIComboBox();
            this.cmbControllerType = new Sunny.UI.UIComboBox();
            this.btnSave = new Sunny.UI.UIButton();
            this.uiMarkLabel5 = new Sunny.UI.UIMarkLabel();
            this.txtDeviceInNo = new Sunny.UI.UITextBox();
            this.SuspendLayout();
            // 
            // uiMarkLabel1
            // 
            this.uiMarkLabel1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiMarkLabel1.Location = new System.Drawing.Point(148, 132);
            this.uiMarkLabel1.Name = "uiMarkLabel1";
            this.uiMarkLabel1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel1.Size = new System.Drawing.Size(100, 23);
            this.uiMarkLabel1.TabIndex = 0;
            this.uiMarkLabel1.Text = "电源型号：";
            this.uiMarkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiMarkLabel2
            // 
            this.uiMarkLabel2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiMarkLabel2.Location = new System.Drawing.Point(149, 189);
            this.uiMarkLabel2.Name = "uiMarkLabel2";
            this.uiMarkLabel2.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel2.Size = new System.Drawing.Size(100, 23);
            this.uiMarkLabel2.TabIndex = 0;
            this.uiMarkLabel2.Text = "电源地址：";
            this.uiMarkLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiMarkLabel3
            // 
            this.uiMarkLabel3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiMarkLabel3.Location = new System.Drawing.Point(149, 241);
            this.uiMarkLabel3.Name = "uiMarkLabel3";
            this.uiMarkLabel3.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel3.Size = new System.Drawing.Size(100, 23);
            this.uiMarkLabel3.TabIndex = 0;
            this.uiMarkLabel3.Text = "码枪地址：";
            this.uiMarkLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiMarkLabel4
            // 
            this.uiMarkLabel4.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiMarkLabel4.Location = new System.Drawing.Point(149, 300);
            this.uiMarkLabel4.Name = "uiMarkLabel4";
            this.uiMarkLabel4.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel4.Size = new System.Drawing.Size(118, 23);
            this.uiMarkLabel4.TabIndex = 0;
            this.uiMarkLabel4.Text = "单片机配置：";
            this.uiMarkLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbPowerType
            // 
            this.cmbPowerType.DataSource = null;
            this.cmbPowerType.FillColor = System.Drawing.Color.White;
            this.cmbPowerType.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbPowerType.Location = new System.Drawing.Point(290, 126);
            this.cmbPowerType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbPowerType.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmbPowerType.Name = "cmbPowerType";
            this.cmbPowerType.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbPowerType.Size = new System.Drawing.Size(389, 29);
            this.cmbPowerType.TabIndex = 1;
            this.cmbPowerType.Text = "uiComboBox1";
            this.cmbPowerType.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbPowerType.Watermark = "";
            // 
            // cmbPowerAddress
            // 
            this.cmbPowerAddress.DataSource = null;
            this.cmbPowerAddress.FillColor = System.Drawing.Color.White;
            this.cmbPowerAddress.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbPowerAddress.Location = new System.Drawing.Point(290, 183);
            this.cmbPowerAddress.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbPowerAddress.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmbPowerAddress.Name = "cmbPowerAddress";
            this.cmbPowerAddress.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbPowerAddress.Size = new System.Drawing.Size(389, 29);
            this.cmbPowerAddress.TabIndex = 1;
            this.cmbPowerAddress.Text = "uiComboBox1";
            this.cmbPowerAddress.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbPowerAddress.Watermark = "";
            // 
            // cmbBarcodeScanAddress
            // 
            this.cmbBarcodeScanAddress.DataSource = null;
            this.cmbBarcodeScanAddress.FillColor = System.Drawing.Color.White;
            this.cmbBarcodeScanAddress.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbBarcodeScanAddress.Location = new System.Drawing.Point(290, 235);
            this.cmbBarcodeScanAddress.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbBarcodeScanAddress.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmbBarcodeScanAddress.Name = "cmbBarcodeScanAddress";
            this.cmbBarcodeScanAddress.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbBarcodeScanAddress.Size = new System.Drawing.Size(389, 29);
            this.cmbBarcodeScanAddress.TabIndex = 1;
            this.cmbBarcodeScanAddress.Text = "uiComboBox1";
            this.cmbBarcodeScanAddress.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbBarcodeScanAddress.Watermark = "";
            // 
            // cmbControllerType
            // 
            this.cmbControllerType.DataSource = null;
            this.cmbControllerType.FillColor = System.Drawing.Color.White;
            this.cmbControllerType.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbControllerType.Location = new System.Drawing.Point(290, 294);
            this.cmbControllerType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbControllerType.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmbControllerType.Name = "cmbControllerType";
            this.cmbControllerType.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbControllerType.Size = new System.Drawing.Size(389, 29);
            this.cmbControllerType.TabIndex = 1;
            this.cmbControllerType.Text = "uiComboBox1";
            this.cmbControllerType.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbControllerType.Watermark = "";
            // 
            // btnSave
            // 
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.Location = new System.Drawing.Point(290, 364);
            this.btnSave.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(145, 66);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "保存";
            this.btnSave.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // uiMarkLabel5
            // 
            this.uiMarkLabel5.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiMarkLabel5.Location = new System.Drawing.Point(80, 75);
            this.uiMarkLabel5.Name = "uiMarkLabel5";
            this.uiMarkLabel5.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel5.Size = new System.Drawing.Size(169, 23);
            this.uiMarkLabel5.TabIndex = 0;
            this.uiMarkLabel5.Text = "设备编号（IN号）：";
            this.uiMarkLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDeviceInNo
            // 
            this.txtDeviceInNo.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtDeviceInNo.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDeviceInNo.Location = new System.Drawing.Point(290, 66);
            this.txtDeviceInNo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtDeviceInNo.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtDeviceInNo.Name = "txtDeviceInNo";
            this.txtDeviceInNo.ShowText = false;
            this.txtDeviceInNo.Size = new System.Drawing.Size(389, 32);
            this.txtDeviceInNo.TabIndex = 39;
            this.txtDeviceInNo.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtDeviceInNo.Watermark = "设备编号（IN号）：";
            // 
            // FrmDeviceConfig
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(800, 480);
            this.Controls.Add(this.txtDeviceInNo);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.cmbControllerType);
            this.Controls.Add(this.cmbBarcodeScanAddress);
            this.Controls.Add(this.cmbPowerAddress);
            this.Controls.Add(this.cmbPowerType);
            this.Controls.Add(this.uiMarkLabel4);
            this.Controls.Add(this.uiMarkLabel3);
            this.Controls.Add(this.uiMarkLabel2);
            this.Controls.Add(this.uiMarkLabel5);
            this.Controls.Add(this.uiMarkLabel1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(800, 480);
            this.MinimumSize = new System.Drawing.Size(800, 480);
            this.Name = "FrmDeviceConfig";
            this.Text = "硬件配置";
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 800, 480);
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UIMarkLabel uiMarkLabel1;
        private Sunny.UI.UIMarkLabel uiMarkLabel2;
        private Sunny.UI.UIMarkLabel uiMarkLabel3;
        private Sunny.UI.UIMarkLabel uiMarkLabel4;
        private Sunny.UI.UIComboBox cmbPowerType;
        private Sunny.UI.UIComboBox cmbPowerAddress;
        private Sunny.UI.UIComboBox cmbBarcodeScanAddress;
        private Sunny.UI.UIComboBox cmbControllerType;
        private Sunny.UI.UIButton btnSave;
        private Sunny.UI.UIMarkLabel uiMarkLabel5;
        private Sunny.UI.UITextBox txtDeviceInNo;
    }
}