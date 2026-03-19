namespace CheckSystem.HelperForms.BD18333
{
    partial class Bd18333CanDeviceMgrForm
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
            this.uiTableLayoutPanel1 = new Sunny.UI.UITableLayoutPanel();
            this.uiPanel1 = new Sunny.UI.UIPanel();
            this.cmbPwmCurr = new Sunny.UI.UIComboBox();
            this.allPwm = new System.Windows.Forms.NumericUpDown();
            this.allDimSet = new System.Windows.Forms.NumericUpDown();
            this.uiRadioButton2 = new Sunny.UI.UIRadioButton();
            this.uiRadioButton1 = new Sunny.UI.UIRadioButton();
            this.mainPnl = new Sunny.UI.UIPanel();
            this.uiTabControl1 = new Sunny.UI.UITabControl();
            this.allSw = new Sunny.UI.UISwitch();
            this.uiTableLayoutPanel1.SuspendLayout();
            this.uiPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.allPwm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.allDimSet)).BeginInit();
            this.mainPnl.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiTableLayoutPanel1
            // 
            this.uiTableLayoutPanel1.ColumnCount = 1;
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.uiTableLayoutPanel1.Controls.Add(this.uiPanel1, 0, 0);
            this.uiTableLayoutPanel1.Controls.Add(this.mainPnl, 0, 1);
            this.uiTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTableLayoutPanel1.Location = new System.Drawing.Point(0, 35);
            this.uiTableLayoutPanel1.Name = "uiTableLayoutPanel1";
            this.uiTableLayoutPanel1.RowCount = 2;
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.uiTableLayoutPanel1.Size = new System.Drawing.Size(900, 565);
            this.uiTableLayoutPanel1.TabIndex = 0;
            this.uiTableLayoutPanel1.TagString = null;
            // 
            // uiPanel1
            // 
            this.uiPanel1.Controls.Add(this.allSw);
            this.uiPanel1.Controls.Add(this.cmbPwmCurr);
            this.uiPanel1.Controls.Add(this.allPwm);
            this.uiPanel1.Controls.Add(this.allDimSet);
            this.uiPanel1.Controls.Add(this.uiRadioButton2);
            this.uiPanel1.Controls.Add(this.uiRadioButton1);
            this.uiPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiPanel1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiPanel1.Location = new System.Drawing.Point(4, 5);
            this.uiPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPanel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Size = new System.Drawing.Size(892, 40);
            this.uiPanel1.TabIndex = 0;
            this.uiPanel1.Text = null;
            this.uiPanel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbPwmCurr
            // 
            this.cmbPwmCurr.DataSource = null;
            this.cmbPwmCurr.FillColor = System.Drawing.Color.White;
            this.cmbPwmCurr.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbPwmCurr.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.cmbPwmCurr.ItemSelectForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.cmbPwmCurr.Location = new System.Drawing.Point(462, 5);
            this.cmbPwmCurr.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbPwmCurr.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmbPwmCurr.Name = "cmbPwmCurr";
            this.cmbPwmCurr.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbPwmCurr.Size = new System.Drawing.Size(150, 29);
            this.cmbPwmCurr.SymbolSize = 24;
            this.cmbPwmCurr.TabIndex = 2;
            this.cmbPwmCurr.Text = "uiComboBox1";
            this.cmbPwmCurr.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbPwmCurr.Watermark = "";
            // 
            // allPwm
            // 
            this.allPwm.DecimalPlaces = 2;
            this.allPwm.Location = new System.Drawing.Point(335, 6);
            this.allPwm.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.allPwm.Minimum = new decimal(new int[] {
            23,
            0,
            0,
            131072});
            this.allPwm.Name = "allPwm";
            this.allPwm.Size = new System.Drawing.Size(120, 26);
            this.allPwm.TabIndex = 1;
            this.allPwm.Value = new decimal(new int[] {
            23,
            0,
            0,
            131072});
            // 
            // allDimSet
            // 
            this.allDimSet.DecimalPlaces = 2;
            this.allDimSet.Location = new System.Drawing.Point(209, 6);
            this.allDimSet.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.allDimSet.Minimum = new decimal(new int[] {
            23,
            0,
            0,
            131072});
            this.allDimSet.Name = "allDimSet";
            this.allDimSet.Size = new System.Drawing.Size(120, 26);
            this.allDimSet.TabIndex = 1;
            this.allDimSet.Value = new decimal(new int[] {
            23,
            0,
            0,
            131072});
            // 
            // uiRadioButton2
            // 
            this.uiRadioButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiRadioButton2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiRadioButton2.Location = new System.Drawing.Point(108, 6);
            this.uiRadioButton2.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiRadioButton2.Name = "uiRadioButton2";
            this.uiRadioButton2.Size = new System.Drawing.Size(83, 29);
            this.uiRadioButton2.TabIndex = 0;
            this.uiRadioButton2.Text = "DC模式";
            this.uiRadioButton2.Click += new System.EventHandler(this.uiRadioButton1_Click);
            // 
            // uiRadioButton1
            // 
            this.uiRadioButton1.Checked = true;
            this.uiRadioButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiRadioButton1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiRadioButton1.Location = new System.Drawing.Point(8, 6);
            this.uiRadioButton1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiRadioButton1.Name = "uiRadioButton1";
            this.uiRadioButton1.Size = new System.Drawing.Size(94, 29);
            this.uiRadioButton1.TabIndex = 0;
            this.uiRadioButton1.Text = "PWM模式";
            this.uiRadioButton1.Click += new System.EventHandler(this.uiRadioButton1_Click);
            // 
            // mainPnl
            // 
            this.mainPnl.Controls.Add(this.uiTabControl1);
            this.mainPnl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPnl.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mainPnl.Location = new System.Drawing.Point(4, 55);
            this.mainPnl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.mainPnl.MinimumSize = new System.Drawing.Size(1, 1);
            this.mainPnl.Name = "mainPnl";
            this.mainPnl.Size = new System.Drawing.Size(892, 505);
            this.mainPnl.TabIndex = 1;
            this.mainPnl.Text = null;
            this.mainPnl.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiTabControl1
            // 
            this.uiTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.uiTabControl1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiTabControl1.ItemSize = new System.Drawing.Size(150, 40);
            this.uiTabControl1.Location = new System.Drawing.Point(0, 0);
            this.uiTabControl1.MainPage = "";
            this.uiTabControl1.Name = "uiTabControl1";
            this.uiTabControl1.SelectedIndex = 0;
            this.uiTabControl1.Size = new System.Drawing.Size(892, 505);
            this.uiTabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.uiTabControl1.TabIndex = 0;
            this.uiTabControl1.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 
            // allSw
            // 
            this.allSw.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.allSw.Location = new System.Drawing.Point(648, 5);
            this.allSw.MinimumSize = new System.Drawing.Size(1, 1);
            this.allSw.Name = "allSw";
            this.allSw.Size = new System.Drawing.Size(75, 29);
            this.allSw.TabIndex = 3;
            this.allSw.Text = "uiSwitch1";
            // 
            // Bd18333CanDeviceMgrForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.uiTableLayoutPanel1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(900, 600);
            this.MinimumSize = new System.Drawing.Size(900, 600);
            this.Name = "Bd18333CanDeviceMgrForm";
            this.Text = "LED_CTRL";
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 800, 450);
            this.uiTableLayoutPanel1.ResumeLayout(false);
            this.uiPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.allPwm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.allDimSet)).EndInit();
            this.mainPnl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UITableLayoutPanel uiTableLayoutPanel1;
        private Sunny.UI.UIPanel uiPanel1;
        private Sunny.UI.UIRadioButton uiRadioButton2;
        private Sunny.UI.UIRadioButton uiRadioButton1;
        private Sunny.UI.UIPanel mainPnl;
        private Sunny.UI.UITabControl uiTabControl1;
        private System.Windows.Forms.NumericUpDown allDimSet;
        private Sunny.UI.UIComboBox cmbPwmCurr;
        private System.Windows.Forms.NumericUpDown allPwm;
        private Sunny.UI.UISwitch allSw;
    }
}