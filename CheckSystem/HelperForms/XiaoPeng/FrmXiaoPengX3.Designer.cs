namespace CheckSystem.HelperForms.XiaoPeng
{
    partial class FrmXiaoPengX3
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
            this.gpLampControl = new Sunny.UI.UIGroupBox();
            this.cmbDrl = new Sunny.UI.UIComboBox();
            this.swPos = new Sunny.UI.UISwitch();
            this.swHb = new Sunny.UI.UISwitch();
            this.swTurnFlashEnable = new Sunny.UI.UISwitch();
            this.swTurnOnOffEnable = new Sunny.UI.UISwitch();
            this.swTurnR = new Sunny.UI.UISwitch();
            this.swTurnL = new Sunny.UI.UISwitch();
            this.swLb = new Sunny.UI.UISwitch();
            this.uiMarkLabel6 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel12 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel8 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel5 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel4 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel3 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel2 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel1 = new Sunny.UI.UIMarkLabel();
            this.gpCdcuControl = new Sunny.UI.UIGroupBox();
            this.txtRearYellow = new Sunny.UI.UIIntegerUpDown();
            this.txtRearRed = new Sunny.UI.UIIntegerUpDown();
            this.txtMiddleWhite = new Sunny.UI.UIIntegerUpDown();
            this.txtFrontYellow = new Sunny.UI.UIIntegerUpDown();
            this.txtFrontWhite = new Sunny.UI.UIIntegerUpDown();
            this.uiMarkLabel7 = new Sunny.UI.UIMarkLabel();
            this.swCdcuControl = new Sunny.UI.UISwitch();
            this.uiMarkLabel14 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel10 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel13 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel11 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel9 = new Sunny.UI.UIMarkLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.uiTableLayoutPanel1 = new Sunny.UI.UITableLayoutPanel();
            this.uiTableLayoutPanel2 = new Sunny.UI.UITableLayoutPanel();
            this.uiLabel5 = new Sunny.UI.UILabel();
            this.uiLabel4 = new Sunny.UI.UILabel();
            this.uiLabel3 = new Sunny.UI.UILabel();
            this.uiLabel2 = new Sunny.UI.UILabel();
            this.ledRclr = new Sunny.UI.UILight();
            this.ledRcll = new Sunny.UI.UILight();
            this.ledChlr = new Sunny.UI.UILight();
            this.ledChlm = new Sunny.UI.UILight();
            this.ledChll = new Sunny.UI.UILight();
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.gpExperimentMode = new Sunny.UI.UIGroupBox();
            this.uiRichTextBox1 = new Sunny.UI.UIRichTextBox();
            this.cmbMode = new Sunny.UI.UIComboBox();
            this.uiMarkLabel15 = new Sunny.UI.UIMarkLabel();
            this.gpLampControl.SuspendLayout();
            this.gpCdcuControl.SuspendLayout();
            this.uiTableLayoutPanel2.SuspendLayout();
            this.gpExperimentMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // gpLampControl
            // 
            this.gpLampControl.Controls.Add(this.cmbDrl);
            this.gpLampControl.Controls.Add(this.swPos);
            this.gpLampControl.Controls.Add(this.swHb);
            this.gpLampControl.Controls.Add(this.swTurnFlashEnable);
            this.gpLampControl.Controls.Add(this.swTurnOnOffEnable);
            this.gpLampControl.Controls.Add(this.swTurnR);
            this.gpLampControl.Controls.Add(this.swTurnL);
            this.gpLampControl.Controls.Add(this.swLb);
            this.gpLampControl.Controls.Add(this.uiMarkLabel6);
            this.gpLampControl.Controls.Add(this.uiMarkLabel12);
            this.gpLampControl.Controls.Add(this.uiMarkLabel8);
            this.gpLampControl.Controls.Add(this.uiMarkLabel5);
            this.gpLampControl.Controls.Add(this.uiMarkLabel4);
            this.gpLampControl.Controls.Add(this.uiMarkLabel3);
            this.gpLampControl.Controls.Add(this.uiMarkLabel2);
            this.gpLampControl.Controls.Add(this.uiMarkLabel1);
            this.gpLampControl.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.gpLampControl.Location = new System.Drawing.Point(15, 90);
            this.gpLampControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gpLampControl.MinimumSize = new System.Drawing.Size(1, 1);
            this.gpLampControl.Name = "gpLampControl";
            this.gpLampControl.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.gpLampControl.Size = new System.Drawing.Size(425, 303);
            this.gpLampControl.TabIndex = 0;
            this.gpLampControl.Text = "灯控";
            this.gpLampControl.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbDrl
            // 
            this.cmbDrl.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cmbDrl.DataSource = null;
            this.cmbDrl.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cmbDrl.FillColor = System.Drawing.Color.White;
            this.cmbDrl.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.cmbDrl.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.cmbDrl.Items.AddRange(new object[] {
            "关闭",
            "DRL_MODE",
            "SABER_DAY",
            "SABER_NIGHT"});
            this.cmbDrl.ItemSelectForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.cmbDrl.Location = new System.Drawing.Point(120, 171);
            this.cmbDrl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbDrl.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmbDrl.Name = "cmbDrl";
            this.cmbDrl.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbDrl.Size = new System.Drawing.Size(138, 29);
            this.cmbDrl.SymbolSize = 24;
            this.cmbDrl.TabIndex = 13;
            this.cmbDrl.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbDrl.Watermark = "";
            // 
            // swPos
            // 
            this.swPos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.swPos.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.swPos.Location = new System.Drawing.Point(120, 129);
            this.swPos.MinimumSize = new System.Drawing.Size(1, 1);
            this.swPos.Name = "swPos";
            this.swPos.Size = new System.Drawing.Size(138, 29);
            this.swPos.TabIndex = 9;
            this.swPos.Text = "uiSwitch1";
            // 
            // swHb
            // 
            this.swHb.Cursor = System.Windows.Forms.Cursors.Hand;
            this.swHb.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.swHb.Location = new System.Drawing.Point(120, 85);
            this.swHb.MinimumSize = new System.Drawing.Size(1, 1);
            this.swHb.Name = "swHb";
            this.swHb.Size = new System.Drawing.Size(138, 29);
            this.swHb.TabIndex = 10;
            this.swHb.Text = "uiSwitch1";
            // 
            // swTurnFlashEnable
            // 
            this.swTurnFlashEnable.Cursor = System.Windows.Forms.Cursors.Hand;
            this.swTurnFlashEnable.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.swTurnFlashEnable.Location = new System.Drawing.Point(277, 123);
            this.swTurnFlashEnable.MinimumSize = new System.Drawing.Size(1, 1);
            this.swTurnFlashEnable.Name = "swTurnFlashEnable";
            this.swTurnFlashEnable.Size = new System.Drawing.Size(127, 29);
            this.swTurnFlashEnable.TabIndex = 11;
            this.swTurnFlashEnable.Text = "uiSwitch1";
            // 
            // swTurnOnOffEnable
            // 
            this.swTurnOnOffEnable.Cursor = System.Windows.Forms.Cursors.Hand;
            this.swTurnOnOffEnable.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.swTurnOnOffEnable.Location = new System.Drawing.Point(269, 256);
            this.swTurnOnOffEnable.MinimumSize = new System.Drawing.Size(1, 1);
            this.swTurnOnOffEnable.Name = "swTurnOnOffEnable";
            this.swTurnOnOffEnable.Size = new System.Drawing.Size(127, 29);
            this.swTurnOnOffEnable.TabIndex = 11;
            this.swTurnOnOffEnable.Text = "uiSwitch1";
            // 
            // swTurnR
            // 
            this.swTurnR.Cursor = System.Windows.Forms.Cursors.Hand;
            this.swTurnR.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.swTurnR.Location = new System.Drawing.Point(106, 262);
            this.swTurnR.MinimumSize = new System.Drawing.Size(1, 1);
            this.swTurnR.Name = "swTurnR";
            this.swTurnR.Size = new System.Drawing.Size(152, 29);
            this.swTurnR.TabIndex = 11;
            this.swTurnR.Text = "uiSwitch1";
            // 
            // swTurnL
            // 
            this.swTurnL.Cursor = System.Windows.Forms.Cursors.Hand;
            this.swTurnL.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.swTurnL.Location = new System.Drawing.Point(106, 218);
            this.swTurnL.MinimumSize = new System.Drawing.Size(1, 1);
            this.swTurnL.Name = "swTurnL";
            this.swTurnL.Size = new System.Drawing.Size(152, 29);
            this.swTurnL.TabIndex = 11;
            this.swTurnL.Text = "uiSwitch1";
            // 
            // swLb
            // 
            this.swLb.Cursor = System.Windows.Forms.Cursors.Hand;
            this.swLb.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.swLb.Location = new System.Drawing.Point(120, 43);
            this.swLb.MinimumSize = new System.Drawing.Size(1, 1);
            this.swLb.Name = "swLb";
            this.swLb.Size = new System.Drawing.Size(138, 29);
            this.swLb.TabIndex = 11;
            this.swLb.Text = "uiSwitch1";
            // 
            // uiMarkLabel6
            // 
            this.uiMarkLabel6.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiMarkLabel6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel6.Location = new System.Drawing.Point(13, 262);
            this.uiMarkLabel6.Name = "uiMarkLabel6";
            this.uiMarkLabel6.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel6.Size = new System.Drawing.Size(100, 23);
            this.uiMarkLabel6.TabIndex = 3;
            this.uiMarkLabel6.Text = "转向灯R：";
            this.uiMarkLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiMarkLabel12
            // 
            this.uiMarkLabel12.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.uiMarkLabel12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel12.Location = new System.Drawing.Point(274, 85);
            this.uiMarkLabel12.MarkPos = Sunny.UI.UIMarkLabel.UIMarkPos.Bottom;
            this.uiMarkLabel12.Name = "uiMarkLabel12";
            this.uiMarkLabel12.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.uiMarkLabel12.Size = new System.Drawing.Size(133, 23);
            this.uiMarkLabel12.TabIndex = 4;
            this.uiMarkLabel12.Text = "TURN 闪烁使能：";
            this.uiMarkLabel12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiMarkLabel8
            // 
            this.uiMarkLabel8.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.uiMarkLabel8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel8.Location = new System.Drawing.Point(265, 218);
            this.uiMarkLabel8.MarkPos = Sunny.UI.UIMarkLabel.UIMarkPos.Bottom;
            this.uiMarkLabel8.Name = "uiMarkLabel8";
            this.uiMarkLabel8.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.uiMarkLabel8.Size = new System.Drawing.Size(142, 23);
            this.uiMarkLabel8.TabIndex = 4;
            this.uiMarkLabel8.Text = "TURN 400ms开关：";
            this.uiMarkLabel8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiMarkLabel5
            // 
            this.uiMarkLabel5.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiMarkLabel5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel5.Location = new System.Drawing.Point(13, 218);
            this.uiMarkLabel5.Name = "uiMarkLabel5";
            this.uiMarkLabel5.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel5.Size = new System.Drawing.Size(100, 23);
            this.uiMarkLabel5.TabIndex = 4;
            this.uiMarkLabel5.Text = "转向灯L：";
            this.uiMarkLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiMarkLabel4
            // 
            this.uiMarkLabel4.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiMarkLabel4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel4.Location = new System.Drawing.Point(13, 171);
            this.uiMarkLabel4.Name = "uiMarkLabel4";
            this.uiMarkLabel4.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel4.Size = new System.Drawing.Size(100, 23);
            this.uiMarkLabel4.TabIndex = 5;
            this.uiMarkLabel4.Text = "日行灯：";
            this.uiMarkLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiMarkLabel3
            // 
            this.uiMarkLabel3.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiMarkLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel3.Location = new System.Drawing.Point(13, 129);
            this.uiMarkLabel3.Name = "uiMarkLabel3";
            this.uiMarkLabel3.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel3.Size = new System.Drawing.Size(100, 23);
            this.uiMarkLabel3.TabIndex = 6;
            this.uiMarkLabel3.Text = "位置灯：";
            this.uiMarkLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiMarkLabel2
            // 
            this.uiMarkLabel2.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiMarkLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel2.Location = new System.Drawing.Point(13, 85);
            this.uiMarkLabel2.Name = "uiMarkLabel2";
            this.uiMarkLabel2.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel2.Size = new System.Drawing.Size(100, 23);
            this.uiMarkLabel2.TabIndex = 7;
            this.uiMarkLabel2.Text = "远光灯：";
            this.uiMarkLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiMarkLabel1
            // 
            this.uiMarkLabel1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiMarkLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel1.Location = new System.Drawing.Point(13, 43);
            this.uiMarkLabel1.Name = "uiMarkLabel1";
            this.uiMarkLabel1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel1.Size = new System.Drawing.Size(100, 23);
            this.uiMarkLabel1.TabIndex = 8;
            this.uiMarkLabel1.Text = "近光灯：";
            this.uiMarkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gpCdcuControl
            // 
            this.gpCdcuControl.Controls.Add(this.txtRearYellow);
            this.gpCdcuControl.Controls.Add(this.txtRearRed);
            this.gpCdcuControl.Controls.Add(this.txtMiddleWhite);
            this.gpCdcuControl.Controls.Add(this.txtFrontYellow);
            this.gpCdcuControl.Controls.Add(this.txtFrontWhite);
            this.gpCdcuControl.Controls.Add(this.uiMarkLabel7);
            this.gpCdcuControl.Controls.Add(this.swCdcuControl);
            this.gpCdcuControl.Controls.Add(this.uiMarkLabel14);
            this.gpCdcuControl.Controls.Add(this.uiMarkLabel10);
            this.gpCdcuControl.Controls.Add(this.uiMarkLabel13);
            this.gpCdcuControl.Controls.Add(this.uiMarkLabel11);
            this.gpCdcuControl.Controls.Add(this.uiMarkLabel9);
            this.gpCdcuControl.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.gpCdcuControl.Location = new System.Drawing.Point(15, 429);
            this.gpCdcuControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gpCdcuControl.MinimumSize = new System.Drawing.Size(1, 1);
            this.gpCdcuControl.Name = "gpCdcuControl";
            this.gpCdcuControl.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.gpCdcuControl.Size = new System.Drawing.Size(768, 166);
            this.gpCdcuControl.TabIndex = 1;
            this.gpCdcuControl.Text = "CDCU控制";
            this.gpCdcuControl.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtRearYellow
            // 
            this.txtRearYellow.Enabled = false;
            this.txtRearYellow.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtRearYellow.Location = new System.Drawing.Point(637, 126);
            this.txtRearYellow.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtRearYellow.Maximum = 10;
            this.txtRearYellow.Minimum = 0;
            this.txtRearYellow.MinimumSize = new System.Drawing.Size(100, 0);
            this.txtRearYellow.Name = "txtRearYellow";
            this.txtRearYellow.ShowText = false;
            this.txtRearYellow.Size = new System.Drawing.Size(116, 29);
            this.txtRearYellow.Style = Sunny.UI.UIStyle.Custom;
            this.txtRearYellow.TabIndex = 12;
            this.txtRearYellow.Text = "uiIntegerUpDown1";
            this.txtRearYellow.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtRearRed
            // 
            this.txtRearRed.Enabled = false;
            this.txtRearRed.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtRearRed.Location = new System.Drawing.Point(637, 77);
            this.txtRearRed.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtRearRed.Maximum = 10;
            this.txtRearRed.Minimum = 0;
            this.txtRearRed.MinimumSize = new System.Drawing.Size(100, 0);
            this.txtRearRed.Name = "txtRearRed";
            this.txtRearRed.ShowText = false;
            this.txtRearRed.Size = new System.Drawing.Size(116, 29);
            this.txtRearRed.Style = Sunny.UI.UIStyle.Custom;
            this.txtRearRed.TabIndex = 12;
            this.txtRearRed.Text = "uiIntegerUpDown1";
            this.txtRearRed.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtMiddleWhite
            // 
            this.txtMiddleWhite.Enabled = false;
            this.txtMiddleWhite.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtMiddleWhite.Location = new System.Drawing.Point(369, 77);
            this.txtMiddleWhite.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtMiddleWhite.Maximum = 10;
            this.txtMiddleWhite.Minimum = 0;
            this.txtMiddleWhite.MinimumSize = new System.Drawing.Size(100, 0);
            this.txtMiddleWhite.Name = "txtMiddleWhite";
            this.txtMiddleWhite.ShowText = false;
            this.txtMiddleWhite.Size = new System.Drawing.Size(116, 29);
            this.txtMiddleWhite.Style = Sunny.UI.UIStyle.Custom;
            this.txtMiddleWhite.TabIndex = 12;
            this.txtMiddleWhite.Text = "uiIntegerUpDown1";
            this.txtMiddleWhite.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtFrontYellow
            // 
            this.txtFrontYellow.Enabled = false;
            this.txtFrontYellow.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtFrontYellow.Location = new System.Drawing.Point(120, 126);
            this.txtFrontYellow.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtFrontYellow.Maximum = 10;
            this.txtFrontYellow.Minimum = 0;
            this.txtFrontYellow.MinimumSize = new System.Drawing.Size(100, 0);
            this.txtFrontYellow.Name = "txtFrontYellow";
            this.txtFrontYellow.ShowText = false;
            this.txtFrontYellow.Size = new System.Drawing.Size(116, 29);
            this.txtFrontYellow.Style = Sunny.UI.UIStyle.Custom;
            this.txtFrontYellow.TabIndex = 12;
            this.txtFrontYellow.Text = "uiIntegerUpDown1";
            this.txtFrontYellow.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtFrontWhite
            // 
            this.txtFrontWhite.Enabled = false;
            this.txtFrontWhite.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtFrontWhite.Location = new System.Drawing.Point(120, 77);
            this.txtFrontWhite.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtFrontWhite.Maximum = 10;
            this.txtFrontWhite.Minimum = 0;
            this.txtFrontWhite.MinimumSize = new System.Drawing.Size(100, 0);
            this.txtFrontWhite.Name = "txtFrontWhite";
            this.txtFrontWhite.ShowText = false;
            this.txtFrontWhite.Size = new System.Drawing.Size(116, 29);
            this.txtFrontWhite.Style = Sunny.UI.UIStyle.Custom;
            this.txtFrontWhite.TabIndex = 12;
            this.txtFrontWhite.Text = "uiIntegerUpDown1";
            this.txtFrontWhite.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel7
            // 
            this.uiMarkLabel7.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiMarkLabel7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel7.Location = new System.Drawing.Point(13, 32);
            this.uiMarkLabel7.Name = "uiMarkLabel7";
            this.uiMarkLabel7.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel7.Size = new System.Drawing.Size(100, 23);
            this.uiMarkLabel7.TabIndex = 8;
            this.uiMarkLabel7.Text = "CDCU：";
            this.uiMarkLabel7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // swCdcuControl
            // 
            this.swCdcuControl.Cursor = System.Windows.Forms.Cursors.Hand;
            this.swCdcuControl.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.swCdcuControl.Location = new System.Drawing.Point(137, 32);
            this.swCdcuControl.MinimumSize = new System.Drawing.Size(1, 1);
            this.swCdcuControl.Name = "swCdcuControl";
            this.swCdcuControl.Size = new System.Drawing.Size(138, 29);
            this.swCdcuControl.TabIndex = 11;
            this.swCdcuControl.Text = "uiSwitch1";
            // 
            // uiMarkLabel14
            // 
            this.uiMarkLabel14.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiMarkLabel14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel14.Location = new System.Drawing.Point(531, 126);
            this.uiMarkLabel14.Name = "uiMarkLabel14";
            this.uiMarkLabel14.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel14.Size = new System.Drawing.Size(100, 23);
            this.uiMarkLabel14.TabIndex = 5;
            this.uiMarkLabel14.Text = "尾灯黄光：";
            this.uiMarkLabel14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiMarkLabel10
            // 
            this.uiMarkLabel10.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiMarkLabel10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel10.Location = new System.Drawing.Point(13, 126);
            this.uiMarkLabel10.Name = "uiMarkLabel10";
            this.uiMarkLabel10.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel10.Size = new System.Drawing.Size(100, 23);
            this.uiMarkLabel10.TabIndex = 5;
            this.uiMarkLabel10.Text = "前灯黄光：";
            this.uiMarkLabel10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiMarkLabel13
            // 
            this.uiMarkLabel13.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiMarkLabel13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel13.Location = new System.Drawing.Point(531, 77);
            this.uiMarkLabel13.Name = "uiMarkLabel13";
            this.uiMarkLabel13.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel13.Size = new System.Drawing.Size(100, 23);
            this.uiMarkLabel13.TabIndex = 5;
            this.uiMarkLabel13.Text = "尾灯红光：";
            this.uiMarkLabel13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiMarkLabel11
            // 
            this.uiMarkLabel11.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiMarkLabel11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel11.Location = new System.Drawing.Point(273, 77);
            this.uiMarkLabel11.Name = "uiMarkLabel11";
            this.uiMarkLabel11.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel11.Size = new System.Drawing.Size(100, 23);
            this.uiMarkLabel11.TabIndex = 5;
            this.uiMarkLabel11.Text = "中灯白光：";
            this.uiMarkLabel11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiMarkLabel9
            // 
            this.uiMarkLabel9.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiMarkLabel9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel9.Location = new System.Drawing.Point(13, 77);
            this.uiMarkLabel9.Name = "uiMarkLabel9";
            this.uiMarkLabel9.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel9.Size = new System.Drawing.Size(100, 23);
            this.uiMarkLabel9.TabIndex = 5;
            this.uiMarkLabel9.Text = "前灯白光：";
            this.uiMarkLabel9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // uiTableLayoutPanel1
            // 
            this.uiTableLayoutPanel1.ColumnCount = 5;
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.uiTableLayoutPanel1.Location = new System.Drawing.Point(448, 61);
            this.uiTableLayoutPanel1.Name = "uiTableLayoutPanel1";
            this.uiTableLayoutPanel1.RowCount = 2;
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.uiTableLayoutPanel1.Size = new System.Drawing.Size(335, 105);
            this.uiTableLayoutPanel1.TabIndex = 2;
            this.uiTableLayoutPanel1.TagString = null;
            // 
            // uiTableLayoutPanel2
            // 
            this.uiTableLayoutPanel2.ColumnCount = 5;
            this.uiTableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.uiTableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.uiTableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.uiTableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.uiTableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.uiTableLayoutPanel2.Controls.Add(this.uiLabel5, 4, 0);
            this.uiTableLayoutPanel2.Controls.Add(this.uiLabel4, 3, 0);
            this.uiTableLayoutPanel2.Controls.Add(this.uiLabel3, 2, 0);
            this.uiTableLayoutPanel2.Controls.Add(this.uiLabel2, 1, 0);
            this.uiTableLayoutPanel2.Controls.Add(this.ledRclr, 4, 1);
            this.uiTableLayoutPanel2.Controls.Add(this.ledRcll, 3, 1);
            this.uiTableLayoutPanel2.Controls.Add(this.ledChlr, 2, 1);
            this.uiTableLayoutPanel2.Controls.Add(this.ledChlm, 1, 1);
            this.uiTableLayoutPanel2.Controls.Add(this.ledChll, 0, 1);
            this.uiTableLayoutPanel2.Controls.Add(this.uiLabel1, 0, 0);
            this.uiTableLayoutPanel2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiTableLayoutPanel2.Location = new System.Drawing.Point(448, 61);
            this.uiTableLayoutPanel2.Name = "uiTableLayoutPanel2";
            this.uiTableLayoutPanel2.RowCount = 2;
            this.uiTableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.uiTableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.uiTableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.uiTableLayoutPanel2.Size = new System.Drawing.Size(335, 105);
            this.uiTableLayoutPanel2.TabIndex = 2;
            this.uiTableLayoutPanel2.TagString = null;
            // 
            // uiLabel5
            // 
            this.uiLabel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiLabel5.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiLabel5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel5.Location = new System.Drawing.Point(271, 3);
            this.uiLabel5.Margin = new System.Windows.Forms.Padding(3);
            this.uiLabel5.Name = "uiLabel5";
            this.uiLabel5.Size = new System.Drawing.Size(61, 46);
            this.uiLabel5.TabIndex = 11;
            this.uiLabel5.Text = "RCLR";
            this.uiLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiLabel4
            // 
            this.uiLabel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiLabel4.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiLabel4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel4.Location = new System.Drawing.Point(204, 3);
            this.uiLabel4.Margin = new System.Windows.Forms.Padding(3);
            this.uiLabel4.Name = "uiLabel4";
            this.uiLabel4.Size = new System.Drawing.Size(61, 46);
            this.uiLabel4.TabIndex = 10;
            this.uiLabel4.Text = "RCLL";
            this.uiLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiLabel3
            // 
            this.uiLabel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiLabel3.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel3.Location = new System.Drawing.Point(137, 3);
            this.uiLabel3.Margin = new System.Windows.Forms.Padding(3);
            this.uiLabel3.Name = "uiLabel3";
            this.uiLabel3.Size = new System.Drawing.Size(61, 46);
            this.uiLabel3.TabIndex = 9;
            this.uiLabel3.Text = "CHLR";
            this.uiLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiLabel2
            // 
            this.uiLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiLabel2.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel2.Location = new System.Drawing.Point(70, 3);
            this.uiLabel2.Margin = new System.Windows.Forms.Padding(3);
            this.uiLabel2.Name = "uiLabel2";
            this.uiLabel2.Size = new System.Drawing.Size(61, 46);
            this.uiLabel2.TabIndex = 8;
            this.uiLabel2.Text = "CHLM";
            this.uiLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ledRclr
            // 
            this.ledRclr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ledRclr.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ledRclr.Location = new System.Drawing.Point(271, 55);
            this.ledRclr.MinimumSize = new System.Drawing.Size(1, 1);
            this.ledRclr.Name = "ledRclr";
            this.ledRclr.Radius = 47;
            this.ledRclr.Size = new System.Drawing.Size(61, 47);
            this.ledRclr.State = Sunny.UI.UILightState.Off;
            this.ledRclr.TabIndex = 6;
            this.ledRclr.Text = "uiLight5";
            // 
            // ledRcll
            // 
            this.ledRcll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ledRcll.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ledRcll.Location = new System.Drawing.Point(204, 55);
            this.ledRcll.MinimumSize = new System.Drawing.Size(1, 1);
            this.ledRcll.Name = "ledRcll";
            this.ledRcll.Radius = 47;
            this.ledRcll.Size = new System.Drawing.Size(61, 47);
            this.ledRcll.State = Sunny.UI.UILightState.Off;
            this.ledRcll.TabIndex = 5;
            this.ledRcll.Text = "uiLight4";
            // 
            // ledChlr
            // 
            this.ledChlr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ledChlr.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ledChlr.Location = new System.Drawing.Point(137, 55);
            this.ledChlr.MinimumSize = new System.Drawing.Size(1, 1);
            this.ledChlr.Name = "ledChlr";
            this.ledChlr.Radius = 47;
            this.ledChlr.Size = new System.Drawing.Size(61, 47);
            this.ledChlr.State = Sunny.UI.UILightState.Off;
            this.ledChlr.TabIndex = 4;
            this.ledChlr.Text = "uiLight3";
            // 
            // ledChlm
            // 
            this.ledChlm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ledChlm.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ledChlm.Location = new System.Drawing.Point(70, 55);
            this.ledChlm.MinimumSize = new System.Drawing.Size(1, 1);
            this.ledChlm.Name = "ledChlm";
            this.ledChlm.Radius = 47;
            this.ledChlm.Size = new System.Drawing.Size(61, 47);
            this.ledChlm.State = Sunny.UI.UILightState.Off;
            this.ledChlm.TabIndex = 3;
            this.ledChlm.Text = "uiLight2";
            // 
            // ledChll
            // 
            this.ledChll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ledChll.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ledChll.Location = new System.Drawing.Point(3, 55);
            this.ledChll.MinimumSize = new System.Drawing.Size(1, 1);
            this.ledChll.Name = "ledChll";
            this.ledChll.Radius = 47;
            this.ledChll.Size = new System.Drawing.Size(61, 47);
            this.ledChll.State = Sunny.UI.UILightState.Off;
            this.ledChll.TabIndex = 2;
            this.ledChll.Text = "uiLight1";
            // 
            // uiLabel1
            // 
            this.uiLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiLabel1.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel1.Location = new System.Drawing.Point(3, 3);
            this.uiLabel1.Margin = new System.Windows.Forms.Padding(3);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(61, 46);
            this.uiLabel1.TabIndex = 7;
            this.uiLabel1.Text = "CHLL";
            this.uiLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gpExperimentMode
            // 
            this.gpExperimentMode.Controls.Add(this.uiRichTextBox1);
            this.gpExperimentMode.Controls.Add(this.cmbMode);
            this.gpExperimentMode.Controls.Add(this.uiMarkLabel15);
            this.gpExperimentMode.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gpExperimentMode.Location = new System.Drawing.Point(448, 175);
            this.gpExperimentMode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gpExperimentMode.MinimumSize = new System.Drawing.Size(1, 1);
            this.gpExperimentMode.Name = "gpExperimentMode";
            this.gpExperimentMode.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.gpExperimentMode.Size = new System.Drawing.Size(335, 262);
            this.gpExperimentMode.TabIndex = 3;
            this.gpExperimentMode.Text = "实验模式";
            this.gpExperimentMode.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.gpExperimentMode.Visible = false;
            // 
            // uiRichTextBox1
            // 
            this.uiRichTextBox1.FillColor = System.Drawing.Color.White;
            this.uiRichTextBox1.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.uiRichTextBox1.Location = new System.Drawing.Point(6, 69);
            this.uiRichTextBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiRichTextBox1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiRichTextBox1.Name = "uiRichTextBox1";
            this.uiRichTextBox1.Padding = new System.Windows.Forms.Padding(2);
            this.uiRichTextBox1.ReadOnly = true;
            this.uiRichTextBox1.ScrollBarStyleInherited = false;
            this.uiRichTextBox1.ShowText = false;
            this.uiRichTextBox1.Size = new System.Drawing.Size(325, 188);
            this.uiRichTextBox1.TabIndex = 14;
            this.uiRichTextBox1.Text = "前灯：\n    实验模式1：HI、Low、Turn(200ms流水；200ms ON；400ms OFF)、Pos\n    实验模式2：DRL\n\n尾灯：\n    " +
    "实验模式1：位置灯点亮，其他灯熄灭\n    实验模式2：转向灯400ms开关，其他灯熄灭\n    实验模式3：转向灯200ms流水，200常亮，其他灯熄灭";
            this.uiRichTextBox1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbMode
            // 
            this.cmbMode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cmbMode.DataSource = null;
            this.cmbMode.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cmbMode.FillColor = System.Drawing.Color.White;
            this.cmbMode.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.cmbMode.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.cmbMode.Items.AddRange(new object[] {
            "实验模式关闭",
            "实验模式1",
            "实验模式2",
            "实验模式3"});
            this.cmbMode.ItemSelectForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.cmbMode.Location = new System.Drawing.Point(140, 32);
            this.cmbMode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbMode.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmbMode.Name = "cmbMode";
            this.cmbMode.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbMode.Size = new System.Drawing.Size(180, 29);
            this.cmbMode.SymbolSize = 24;
            this.cmbMode.TabIndex = 13;
            this.cmbMode.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbMode.Watermark = "";
            // 
            // uiMarkLabel15
            // 
            this.uiMarkLabel15.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiMarkLabel15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel15.Location = new System.Drawing.Point(17, 32);
            this.uiMarkLabel15.Name = "uiMarkLabel15";
            this.uiMarkLabel15.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel15.Size = new System.Drawing.Size(100, 23);
            this.uiMarkLabel15.TabIndex = 8;
            this.uiMarkLabel15.Text = "实验模式：";
            this.uiMarkLabel15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FrmXiaoPengX3
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.gpExperimentMode);
            this.Controls.Add(this.uiTableLayoutPanel2);
            this.Controls.Add(this.uiTableLayoutPanel1);
            this.Controls.Add(this.gpCdcuControl);
            this.Controls.Add(this.gpLampControl);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(800, 600);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "FrmXiaoPengX3";
            this.Style = Sunny.UI.UIStyle.Custom;
            this.Text = "小鹏X3前后灯";
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 800, 450);
            this.gpLampControl.ResumeLayout(false);
            this.gpCdcuControl.ResumeLayout(false);
            this.uiTableLayoutPanel2.ResumeLayout(false);
            this.gpExperimentMode.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UIGroupBox gpLampControl;
        private Sunny.UI.UISwitch swPos;
        private Sunny.UI.UISwitch swHb;
        private Sunny.UI.UISwitch swLb;
        private Sunny.UI.UIMarkLabel uiMarkLabel6;
        private Sunny.UI.UIMarkLabel uiMarkLabel5;
        private Sunny.UI.UIMarkLabel uiMarkLabel4;
        private Sunny.UI.UIMarkLabel uiMarkLabel3;
        private Sunny.UI.UIMarkLabel uiMarkLabel2;
        private Sunny.UI.UIMarkLabel uiMarkLabel1;
        private Sunny.UI.UIGroupBox gpCdcuControl;
        private Sunny.UI.UIMarkLabel uiMarkLabel7;
        private Sunny.UI.UISwitch swCdcuControl;
        private System.Windows.Forms.Timer timer1;
        private Sunny.UI.UIMarkLabel uiMarkLabel8;
        private Sunny.UI.UISwitch swTurnOnOffEnable;
        private Sunny.UI.UITableLayoutPanel uiTableLayoutPanel1;
        private Sunny.UI.UITableLayoutPanel uiTableLayoutPanel2;
        private Sunny.UI.UILabel uiLabel5;
        private Sunny.UI.UILabel uiLabel4;
        private Sunny.UI.UILabel uiLabel3;
        private Sunny.UI.UILabel uiLabel2;
        private Sunny.UI.UILight ledRclr;
        private Sunny.UI.UILight ledRcll;
        private Sunny.UI.UILight ledChlr;
        private Sunny.UI.UILight ledChlm;
        private Sunny.UI.UILight ledChll;
        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UIComboBox cmbDrl;
        private Sunny.UI.UIMarkLabel uiMarkLabel10;
        private Sunny.UI.UIMarkLabel uiMarkLabel9;
        private Sunny.UI.UIIntegerUpDown txtFrontWhite;
        private Sunny.UI.UIMarkLabel uiMarkLabel14;
        private Sunny.UI.UIMarkLabel uiMarkLabel13;
        private Sunny.UI.UIMarkLabel uiMarkLabel11;
        private Sunny.UI.UIIntegerUpDown txtRearYellow;
        private Sunny.UI.UIIntegerUpDown txtRearRed;
        private Sunny.UI.UIIntegerUpDown txtMiddleWhite;
        private Sunny.UI.UIIntegerUpDown txtFrontYellow;
        private Sunny.UI.UISwitch swTurnR;
        private Sunny.UI.UISwitch swTurnL;
        private Sunny.UI.UISwitch swTurnFlashEnable;
        private Sunny.UI.UIMarkLabel uiMarkLabel12;
        private Sunny.UI.UIGroupBox gpExperimentMode;
        private Sunny.UI.UIRichTextBox uiRichTextBox1;
        private Sunny.UI.UIComboBox cmbMode;
        private Sunny.UI.UIMarkLabel uiMarkLabel15;
    }
}