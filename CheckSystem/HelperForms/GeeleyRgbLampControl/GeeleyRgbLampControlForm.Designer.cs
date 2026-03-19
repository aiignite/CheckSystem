namespace CheckSystem.HelperForms.GeeleyRgbLampControl
{
    partial class GeeleyRgbLampControlForm
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
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.tBarRed = new System.Windows.Forms.TrackBar();
            this.tBarGreen = new System.Windows.Forms.TrackBar();
            this.tBarBlue = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.btnBreathOn = new System.Windows.Forms.Button();
            this.btnLedOff = new System.Windows.Forms.Button();
            this.btnLedOn = new System.Windows.Forms.Button();
            this.btnCurrentColor = new System.Windows.Forms.Button();
            this.btnColorSelection = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.btnAddToList = new System.Windows.Forms.Button();
            this.btnAutoOn = new System.Windows.Forms.Button();
            this.btnAutoOff = new System.Windows.Forms.Button();
            this.btnClearList = new System.Windows.Forms.Button();
            this.cmbDelayTime = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tBarRed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tBarGreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tBarBlue)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tBarRed
            // 
            this.tBarRed.Location = new System.Drawing.Point(138, 409);
            this.tBarRed.Name = "tBarRed";
            this.tBarRed.Size = new System.Drawing.Size(756, 45);
            this.tBarRed.TabIndex = 1;
            // 
            // tBarGreen
            // 
            this.tBarGreen.Location = new System.Drawing.Point(138, 460);
            this.tBarGreen.Name = "tBarGreen";
            this.tBarGreen.Size = new System.Drawing.Size(756, 45);
            this.tBarGreen.TabIndex = 1;
            // 
            // tBarBlue
            // 
            this.tBarBlue.Location = new System.Drawing.Point(138, 505);
            this.tBarBlue.Name = "tBarBlue";
            this.tBarBlue.Size = new System.Drawing.Size(756, 45);
            this.tBarBlue.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(69, 409);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "RED:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(69, 460);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "GREEN:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(69, 505);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "BLUE:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.cmbDelayTime);
            this.panel1.Controls.Add(this.listBox1);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.btnBreathOn);
            this.panel1.Controls.Add(this.btnLedOff);
            this.panel1.Controls.Add(this.btnLedOn);
            this.panel1.Controls.Add(this.btnAutoOff);
            this.panel1.Controls.Add(this.btnAutoOn);
            this.panel1.Controls.Add(this.btnClearList);
            this.panel1.Controls.Add(this.btnAddToList);
            this.panel1.Controls.Add(this.btnCurrentColor);
            this.panel1.Controls.Add(this.btnColorSelection);
            this.panel1.Location = new System.Drawing.Point(31, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(879, 367);
            this.panel1.TabIndex = 3;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(155, 132);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(97, 71);
            this.button2.TabIndex = 1;
            this.button2.Text = "呼吸关";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnBreathOn
            // 
            this.btnBreathOn.Location = new System.Drawing.Point(28, 132);
            this.btnBreathOn.Name = "btnBreathOn";
            this.btnBreathOn.Size = new System.Drawing.Size(97, 71);
            this.btnBreathOn.TabIndex = 2;
            this.btnBreathOn.Text = "呼吸开";
            this.btnBreathOn.UseVisualStyleBackColor = true;
            this.btnBreathOn.Click += new System.EventHandler(this.btnBreathOn_Click);
            // 
            // btnLedOff
            // 
            this.btnLedOff.Location = new System.Drawing.Point(155, 31);
            this.btnLedOff.Name = "btnLedOff";
            this.btnLedOff.Size = new System.Drawing.Size(97, 71);
            this.btnLedOff.TabIndex = 3;
            this.btnLedOff.Text = "LED关闭";
            this.btnLedOff.UseVisualStyleBackColor = true;
            this.btnLedOff.Click += new System.EventHandler(this.btnLedOff_Click);
            // 
            // btnLedOn
            // 
            this.btnLedOn.Location = new System.Drawing.Point(28, 31);
            this.btnLedOn.Name = "btnLedOn";
            this.btnLedOn.Size = new System.Drawing.Size(97, 71);
            this.btnLedOn.TabIndex = 4;
            this.btnLedOn.Text = "LED打开";
            this.btnLedOn.UseVisualStyleBackColor = true;
            this.btnLedOn.Click += new System.EventHandler(this.btnLedOn_Click);
            // 
            // btnCurrentColor
            // 
            this.btnCurrentColor.BackColor = System.Drawing.Color.Transparent;
            this.btnCurrentColor.Enabled = false;
            this.btnCurrentColor.Location = new System.Drawing.Point(155, 241);
            this.btnCurrentColor.Name = "btnCurrentColor";
            this.btnCurrentColor.Size = new System.Drawing.Size(97, 71);
            this.btnCurrentColor.TabIndex = 5;
            this.btnCurrentColor.Text = "当前颜色";
            this.btnCurrentColor.UseVisualStyleBackColor = false;
            // 
            // btnColorSelection
            // 
            this.btnColorSelection.Location = new System.Drawing.Point(28, 241);
            this.btnColorSelection.Name = "btnColorSelection";
            this.btnColorSelection.Size = new System.Drawing.Size(97, 71);
            this.btnColorSelection.TabIndex = 6;
            this.btnColorSelection.Text = "颜色选择";
            this.btnColorSelection.UseVisualStyleBackColor = true;
            this.btnColorSelection.Click += new System.EventHandler(this.btnColorSelection_Click);
            // 
            // listBox1
            // 
            this.listBox1.Enabled = false;
            this.listBox1.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 28;
            this.listBox1.Location = new System.Drawing.Point(361, 141);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(488, 200);
            this.listBox1.TabIndex = 7;
            // 
            // btnAddToList
            // 
            this.btnAddToList.BackColor = System.Drawing.Color.Transparent;
            this.btnAddToList.Location = new System.Drawing.Point(361, 13);
            this.btnAddToList.Name = "btnAddToList";
            this.btnAddToList.Size = new System.Drawing.Size(97, 71);
            this.btnAddToList.TabIndex = 5;
            this.btnAddToList.Text = "添加到颜色列表";
            this.btnAddToList.UseVisualStyleBackColor = false;
            this.btnAddToList.Click += new System.EventHandler(this.btnAddToList_Click);
            // 
            // btnAutoOn
            // 
            this.btnAutoOn.BackColor = System.Drawing.Color.Transparent;
            this.btnAutoOn.Location = new System.Drawing.Point(638, 13);
            this.btnAutoOn.Name = "btnAutoOn";
            this.btnAutoOn.Size = new System.Drawing.Size(97, 71);
            this.btnAutoOn.TabIndex = 5;
            this.btnAutoOn.Text = "开启自动刷新";
            this.btnAutoOn.UseVisualStyleBackColor = false;
            this.btnAutoOn.Click += new System.EventHandler(this.btnAutoOn_Click);
            // 
            // btnAutoOff
            // 
            this.btnAutoOff.BackColor = System.Drawing.Color.Transparent;
            this.btnAutoOff.Location = new System.Drawing.Point(766, 13);
            this.btnAutoOff.Name = "btnAutoOff";
            this.btnAutoOff.Size = new System.Drawing.Size(97, 71);
            this.btnAutoOff.TabIndex = 5;
            this.btnAutoOff.Text = "关闭自动刷新";
            this.btnAutoOff.UseVisualStyleBackColor = false;
            this.btnAutoOff.Click += new System.EventHandler(this.btnAutoOff_Click);
            // 
            // btnClearList
            // 
            this.btnClearList.BackColor = System.Drawing.Color.Transparent;
            this.btnClearList.Location = new System.Drawing.Point(499, 13);
            this.btnClearList.Name = "btnClearList";
            this.btnClearList.Size = new System.Drawing.Size(97, 71);
            this.btnClearList.TabIndex = 5;
            this.btnClearList.Text = "清空颜色列表";
            this.btnClearList.UseVisualStyleBackColor = false;
            this.btnClearList.Click += new System.EventHandler(this.btnClearList_Click);
            // 
            // cmbDelayTime
            // 
            this.cmbDelayTime.FormattingEnabled = true;
            this.cmbDelayTime.Location = new System.Drawing.Point(499, 106);
            this.cmbDelayTime.Name = "cmbDelayTime";
            this.cmbDelayTime.Size = new System.Drawing.Size(236, 20);
            this.cmbDelayTime.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(369, 106);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "单步持续时间：";
            // 
            // GeeleyRgbLampControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(949, 606);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tBarBlue);
            this.Controls.Add(this.tBarGreen);
            this.Controls.Add(this.tBarRed);
            this.Name = "GeeleyRgbLampControlForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GeeleyRgbLampControlForm";
            ((System.ComponentModel.ISupportInitialize)(this.tBarRed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tBarGreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tBarBlue)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.TrackBar tBarRed;
        private System.Windows.Forms.TrackBar tBarGreen;
        private System.Windows.Forms.TrackBar tBarBlue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnBreathOn;
        private System.Windows.Forms.Button btnLedOff;
        private System.Windows.Forms.Button btnLedOn;
        private System.Windows.Forms.Button btnCurrentColor;
        private System.Windows.Forms.Button btnColorSelection;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button btnAutoOff;
        private System.Windows.Forms.Button btnAutoOn;
        private System.Windows.Forms.Button btnAddToList;
        private System.Windows.Forms.Button btnClearList;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbDelayTime;
    }
}