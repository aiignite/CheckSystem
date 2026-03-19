namespace CheckSystem.HelperForms.Solenoid
{
    partial class FrmSolenoid
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
            this.uiPanel1 = new Sunny.UI.UIPanel();
            this.uiButton3 = new Sunny.UI.UIButton();
            this.uiButton2 = new Sunny.UI.UIButton();
            this.uiButton1 = new Sunny.UI.UIButton();
            this.uiMarkLabel3 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel2 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel1 = new Sunny.UI.UIMarkLabel();
            this.txtResult = new Sunny.UI.UITextBox();
            this.txtCaliFilePath = new Sunny.UI.UITextBox();
            this.txtAppFilePath = new Sunny.UI.UITextBox();
            this.btnWriteFirstUp = new Sunny.UI.UIButton();
            this.btnReadFirst = new Sunny.UI.UIButton();
            this.btnReadSecond = new Sunny.UI.UIButton();
            this.btnWriteFirstDown = new Sunny.UI.UIButton();
            this.btnWriteSecondUp = new Sunny.UI.UIButton();
            this.btnWriteSecondDown = new Sunny.UI.UIButton();
            this.uiMarkLabel4 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel5 = new Sunny.UI.UIMarkLabel();
            this.txtFirstFlag = new Sunny.UI.UITextBox();
            this.txtSecondFlag = new Sunny.UI.UITextBox();
            this.txtFirstOutput = new Sunny.UI.UITextBox();
            this.txtSecondOutput = new Sunny.UI.UITextBox();
            this.uiGroupBox1 = new Sunny.UI.UIGroupBox();
            this.uiPanel1.SuspendLayout();
            this.uiGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiPanel1
            // 
            this.uiPanel1.Controls.Add(this.uiGroupBox1);
            this.uiPanel1.Controls.Add(this.btnWriteSecondDown);
            this.uiPanel1.Controls.Add(this.btnWriteSecondUp);
            this.uiPanel1.Controls.Add(this.btnWriteFirstDown);
            this.uiPanel1.Controls.Add(this.btnWriteFirstUp);
            this.uiPanel1.Controls.Add(this.uiButton3);
            this.uiPanel1.Controls.Add(this.uiButton2);
            this.uiPanel1.Controls.Add(this.uiButton1);
            this.uiPanel1.Controls.Add(this.uiMarkLabel3);
            this.uiPanel1.Controls.Add(this.uiMarkLabel2);
            this.uiPanel1.Controls.Add(this.uiMarkLabel1);
            this.uiPanel1.Controls.Add(this.txtResult);
            this.uiPanel1.Controls.Add(this.txtCaliFilePath);
            this.uiPanel1.Controls.Add(this.txtAppFilePath);
            this.uiPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiPanel1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiPanel1.Location = new System.Drawing.Point(0, 35);
            this.uiPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPanel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Size = new System.Drawing.Size(800, 415);
            this.uiPanel1.TabIndex = 0;
            this.uiPanel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiButton3
            // 
            this.uiButton3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton3.Location = new System.Drawing.Point(687, 138);
            this.uiButton3.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton3.Name = "uiButton3";
            this.uiButton3.Size = new System.Drawing.Size(100, 35);
            this.uiButton3.TabIndex = 9;
            this.uiButton3.Text = "开始刷新";
            this.uiButton3.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton3.Click += new System.EventHandler(this.uiButton3_Click);
            // 
            // uiButton2
            // 
            this.uiButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton2.Location = new System.Drawing.Point(687, 76);
            this.uiButton2.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton2.Name = "uiButton2";
            this.uiButton2.Size = new System.Drawing.Size(100, 35);
            this.uiButton2.TabIndex = 10;
            this.uiButton2.Text = "选择CAL";
            this.uiButton2.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton2.Click += new System.EventHandler(this.uiButton2_Click);
            // 
            // uiButton1
            // 
            this.uiButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton1.Location = new System.Drawing.Point(687, 25);
            this.uiButton1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton1.Name = "uiButton1";
            this.uiButton1.Size = new System.Drawing.Size(100, 35);
            this.uiButton1.TabIndex = 11;
            this.uiButton1.Text = "选择APP";
            this.uiButton1.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton1.Click += new System.EventHandler(this.uiButton1_Click);
            // 
            // uiMarkLabel3
            // 
            this.uiMarkLabel3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiMarkLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel3.Location = new System.Drawing.Point(18, 149);
            this.uiMarkLabel3.Name = "uiMarkLabel3";
            this.uiMarkLabel3.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel3.Size = new System.Drawing.Size(100, 23);
            this.uiMarkLabel3.TabIndex = 6;
            this.uiMarkLabel3.Text = "刷新结果：";
            this.uiMarkLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiMarkLabel2
            // 
            this.uiMarkLabel2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiMarkLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel2.Location = new System.Drawing.Point(18, 88);
            this.uiMarkLabel2.Name = "uiMarkLabel2";
            this.uiMarkLabel2.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel2.Size = new System.Drawing.Size(100, 23);
            this.uiMarkLabel2.TabIndex = 7;
            this.uiMarkLabel2.Text = "Cal:";
            this.uiMarkLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiMarkLabel1
            // 
            this.uiMarkLabel1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiMarkLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel1.Location = new System.Drawing.Point(18, 36);
            this.uiMarkLabel1.Name = "uiMarkLabel1";
            this.uiMarkLabel1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel1.Size = new System.Drawing.Size(100, 23);
            this.uiMarkLabel1.TabIndex = 8;
            this.uiMarkLabel1.Text = "APP:";
            this.uiMarkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtResult
            // 
            this.txtResult.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtResult.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtResult.Location = new System.Drawing.Point(129, 149);
            this.txtResult.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtResult.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtResult.Name = "txtResult";
            this.txtResult.Padding = new System.Windows.Forms.Padding(5);
            this.txtResult.ReadOnly = true;
            this.txtResult.ShowText = false;
            this.txtResult.Size = new System.Drawing.Size(536, 24);
            this.txtResult.TabIndex = 3;
            this.txtResult.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtResult.Watermark = "";
            // 
            // txtCaliFilePath
            // 
            this.txtCaliFilePath.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCaliFilePath.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtCaliFilePath.Location = new System.Drawing.Point(129, 87);
            this.txtCaliFilePath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtCaliFilePath.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtCaliFilePath.Name = "txtCaliFilePath";
            this.txtCaliFilePath.Padding = new System.Windows.Forms.Padding(5);
            this.txtCaliFilePath.ReadOnly = true;
            this.txtCaliFilePath.ShowText = false;
            this.txtCaliFilePath.Size = new System.Drawing.Size(536, 24);
            this.txtCaliFilePath.TabIndex = 4;
            this.txtCaliFilePath.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtCaliFilePath.Watermark = "";
            // 
            // txtAppFilePath
            // 
            this.txtAppFilePath.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtAppFilePath.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtAppFilePath.Location = new System.Drawing.Point(129, 36);
            this.txtAppFilePath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtAppFilePath.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtAppFilePath.Name = "txtAppFilePath";
            this.txtAppFilePath.Padding = new System.Windows.Forms.Padding(5);
            this.txtAppFilePath.ReadOnly = true;
            this.txtAppFilePath.ShowText = false;
            this.txtAppFilePath.Size = new System.Drawing.Size(536, 24);
            this.txtAppFilePath.TabIndex = 5;
            this.txtAppFilePath.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtAppFilePath.Watermark = "";
            // 
            // btnWriteFirstUp
            // 
            this.btnWriteFirstUp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnWriteFirstUp.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnWriteFirstUp.Location = new System.Drawing.Point(10, 257);
            this.btnWriteFirstUp.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnWriteFirstUp.Name = "btnWriteFirstUp";
            this.btnWriteFirstUp.Size = new System.Drawing.Size(201, 35);
            this.btnWriteFirstUp.TabIndex = 12;
            this.btnWriteFirstUp.Text = "写入第1个点-占空比+1";
            this.btnWriteFirstUp.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnWriteFirstUp.Click += new System.EventHandler(this.btnWriteFirstUp_Click);
            // 
            // btnReadFirst
            // 
            this.btnReadFirst.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReadFirst.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnReadFirst.Location = new System.Drawing.Point(20, 72);
            this.btnReadFirst.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnReadFirst.Name = "btnReadFirst";
            this.btnReadFirst.Size = new System.Drawing.Size(100, 35);
            this.btnReadFirst.TabIndex = 12;
            this.btnReadFirst.Text = "读取第1个点";
            this.btnReadFirst.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnReadFirst.Click += new System.EventHandler(this.btnReadFirst_Click);
            // 
            // btnReadSecond
            // 
            this.btnReadSecond.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReadSecond.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnReadSecond.Location = new System.Drawing.Point(20, 138);
            this.btnReadSecond.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnReadSecond.Name = "btnReadSecond";
            this.btnReadSecond.Size = new System.Drawing.Size(100, 35);
            this.btnReadSecond.TabIndex = 12;
            this.btnReadSecond.Text = "读取第2个点";
            this.btnReadSecond.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnReadSecond.Click += new System.EventHandler(this.btnReadSecond_Click);
            // 
            // btnWriteFirstDown
            // 
            this.btnWriteFirstDown.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnWriteFirstDown.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnWriteFirstDown.Location = new System.Drawing.Point(10, 323);
            this.btnWriteFirstDown.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnWriteFirstDown.Name = "btnWriteFirstDown";
            this.btnWriteFirstDown.Size = new System.Drawing.Size(201, 35);
            this.btnWriteFirstDown.TabIndex = 12;
            this.btnWriteFirstDown.Text = "写入第1个点-占空比-1";
            this.btnWriteFirstDown.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnWriteFirstDown.Click += new System.EventHandler(this.btnWriteFirstDown_Click);
            // 
            // btnWriteSecondUp
            // 
            this.btnWriteSecondUp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnWriteSecondUp.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnWriteSecondUp.Location = new System.Drawing.Point(228, 257);
            this.btnWriteSecondUp.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnWriteSecondUp.Name = "btnWriteSecondUp";
            this.btnWriteSecondUp.Size = new System.Drawing.Size(201, 35);
            this.btnWriteSecondUp.TabIndex = 12;
            this.btnWriteSecondUp.Text = "写入第2个点-占空比+1";
            this.btnWriteSecondUp.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnWriteSecondUp.Click += new System.EventHandler(this.btnWriteSecondUp_Click);
            // 
            // btnWriteSecondDown
            // 
            this.btnWriteSecondDown.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnWriteSecondDown.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnWriteSecondDown.Location = new System.Drawing.Point(228, 323);
            this.btnWriteSecondDown.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnWriteSecondDown.Name = "btnWriteSecondDown";
            this.btnWriteSecondDown.Size = new System.Drawing.Size(201, 35);
            this.btnWriteSecondDown.TabIndex = 12;
            this.btnWriteSecondDown.Text = "写入第2个点-占空比-1";
            this.btnWriteSecondDown.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnWriteSecondDown.Click += new System.EventHandler(this.btnWriteSecondDown_Click);
            // 
            // uiMarkLabel4
            // 
            this.uiMarkLabel4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiMarkLabel4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel4.Location = new System.Drawing.Point(154, 35);
            this.uiMarkLabel4.MarkPos = Sunny.UI.UIMarkLabel.UIMarkPos.Bottom;
            this.uiMarkLabel4.Name = "uiMarkLabel4";
            this.uiMarkLabel4.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.uiMarkLabel4.Size = new System.Drawing.Size(80, 23);
            this.uiMarkLabel4.TabIndex = 13;
            this.uiMarkLabel4.Text = "写入标志";
            this.uiMarkLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiMarkLabel5
            // 
            this.uiMarkLabel5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiMarkLabel5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel5.Location = new System.Drawing.Point(249, 35);
            this.uiMarkLabel5.MarkPos = Sunny.UI.UIMarkLabel.UIMarkPos.Bottom;
            this.uiMarkLabel5.Name = "uiMarkLabel5";
            this.uiMarkLabel5.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.uiMarkLabel5.Size = new System.Drawing.Size(80, 23);
            this.uiMarkLabel5.TabIndex = 13;
            this.uiMarkLabel5.Text = "输出占空比";
            this.uiMarkLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtFirstFlag
            // 
            this.txtFirstFlag.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtFirstFlag.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtFirstFlag.Location = new System.Drawing.Point(157, 83);
            this.txtFirstFlag.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtFirstFlag.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtFirstFlag.Name = "txtFirstFlag";
            this.txtFirstFlag.Padding = new System.Windows.Forms.Padding(5);
            this.txtFirstFlag.ReadOnly = true;
            this.txtFirstFlag.ShowText = false;
            this.txtFirstFlag.Size = new System.Drawing.Size(77, 24);
            this.txtFirstFlag.TabIndex = 3;
            this.txtFirstFlag.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtFirstFlag.Watermark = "";
            // 
            // txtSecondFlag
            // 
            this.txtSecondFlag.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSecondFlag.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSecondFlag.Location = new System.Drawing.Point(157, 138);
            this.txtSecondFlag.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtSecondFlag.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtSecondFlag.Name = "txtSecondFlag";
            this.txtSecondFlag.Padding = new System.Windows.Forms.Padding(5);
            this.txtSecondFlag.ReadOnly = true;
            this.txtSecondFlag.ShowText = false;
            this.txtSecondFlag.Size = new System.Drawing.Size(77, 24);
            this.txtSecondFlag.TabIndex = 3;
            this.txtSecondFlag.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtSecondFlag.Watermark = "";
            // 
            // txtFirstOutput
            // 
            this.txtFirstOutput.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtFirstOutput.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtFirstOutput.Location = new System.Drawing.Point(252, 83);
            this.txtFirstOutput.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtFirstOutput.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtFirstOutput.Name = "txtFirstOutput";
            this.txtFirstOutput.Padding = new System.Windows.Forms.Padding(5);
            this.txtFirstOutput.ReadOnly = true;
            this.txtFirstOutput.ShowText = false;
            this.txtFirstOutput.Size = new System.Drawing.Size(77, 24);
            this.txtFirstOutput.TabIndex = 3;
            this.txtFirstOutput.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtFirstOutput.Watermark = "";
            // 
            // txtSecondOutput
            // 
            this.txtSecondOutput.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSecondOutput.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSecondOutput.Location = new System.Drawing.Point(252, 138);
            this.txtSecondOutput.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtSecondOutput.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtSecondOutput.Name = "txtSecondOutput";
            this.txtSecondOutput.Padding = new System.Windows.Forms.Padding(5);
            this.txtSecondOutput.ReadOnly = true;
            this.txtSecondOutput.ShowText = false;
            this.txtSecondOutput.Size = new System.Drawing.Size(77, 24);
            this.txtSecondOutput.TabIndex = 3;
            this.txtSecondOutput.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtSecondOutput.Watermark = "";
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.Controls.Add(this.btnReadFirst);
            this.uiGroupBox1.Controls.Add(this.uiMarkLabel5);
            this.uiGroupBox1.Controls.Add(this.btnReadSecond);
            this.uiGroupBox1.Controls.Add(this.uiMarkLabel4);
            this.uiGroupBox1.Controls.Add(this.txtFirstFlag);
            this.uiGroupBox1.Controls.Add(this.txtSecondFlag);
            this.uiGroupBox1.Controls.Add(this.txtFirstOutput);
            this.uiGroupBox1.Controls.Add(this.txtSecondOutput);
            this.uiGroupBox1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox1.Location = new System.Drawing.Point(436, 183);
            this.uiGroupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox1.Size = new System.Drawing.Size(351, 227);
            this.uiGroupBox1.TabIndex = 14;
            this.uiGroupBox1.Text = "读取";
            this.uiGroupBox1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FrmSolenoid
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.uiPanel1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(800, 450);
            this.MinimumSize = new System.Drawing.Size(800, 450);
            this.Name = "FrmSolenoid";
            this.Text = "电磁阀模块";
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 800, 450);
            this.uiPanel1.ResumeLayout(false);
            this.uiGroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UIPanel uiPanel1;
        private Sunny.UI.UIButton btnReadSecond;
        private Sunny.UI.UIButton btnReadFirst;
        private Sunny.UI.UIButton btnWriteSecondDown;
        private Sunny.UI.UIButton btnWriteSecondUp;
        private Sunny.UI.UIButton btnWriteFirstDown;
        private Sunny.UI.UIButton btnWriteFirstUp;
        private Sunny.UI.UIButton uiButton3;
        private Sunny.UI.UIButton uiButton2;
        private Sunny.UI.UIButton uiButton1;
        private Sunny.UI.UIMarkLabel uiMarkLabel3;
        private Sunny.UI.UIMarkLabel uiMarkLabel2;
        private Sunny.UI.UIMarkLabel uiMarkLabel1;
        private Sunny.UI.UITextBox txtResult;
        private Sunny.UI.UITextBox txtCaliFilePath;
        private Sunny.UI.UITextBox txtAppFilePath;
        private Sunny.UI.UIMarkLabel uiMarkLabel5;
        private Sunny.UI.UIMarkLabel uiMarkLabel4;
        private Sunny.UI.UITextBox txtSecondOutput;
        private Sunny.UI.UITextBox txtFirstOutput;
        private Sunny.UI.UITextBox txtSecondFlag;
        private Sunny.UI.UITextBox txtFirstFlag;
        private Sunny.UI.UIGroupBox uiGroupBox1;
    }
}