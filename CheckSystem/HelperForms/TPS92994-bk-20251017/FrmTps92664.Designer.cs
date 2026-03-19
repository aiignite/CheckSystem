namespace CheckSystem.HelperForms.TPS92994
{
    partial class FrmTps92664
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTps92664));
            this.lblAddr = new Sunny.UI.UIMarkLabel();
            this.cmbAddrList = new Sunny.UI.UIComboBox();
            this.btnStart = new Sunny.UI.UIButton();
            this.gpLeds = new Sunny.UI.UICheckBoxGroup();
            this.btnStop = new Sunny.UI.UIButton();
            this.SuspendLayout();
            // 
            // lblAddr
            // 
            this.lblAddr.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblAddr.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.lblAddr.Location = new System.Drawing.Point(22, 54);
            this.lblAddr.Name = "lblAddr";
            this.lblAddr.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.lblAddr.Size = new System.Drawing.Size(75, 23);
            this.lblAddr.TabIndex = 0;
            this.lblAddr.Text = "ADDR:";
            this.lblAddr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbAddrList
            // 
            this.cmbAddrList.DataSource = null;
            this.cmbAddrList.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cmbAddrList.FillColor = System.Drawing.Color.White;
            this.cmbAddrList.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbAddrList.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.cmbAddrList.ItemSelectForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.cmbAddrList.Location = new System.Drawing.Point(104, 48);
            this.cmbAddrList.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbAddrList.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmbAddrList.Name = "cmbAddrList";
            this.cmbAddrList.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbAddrList.Size = new System.Drawing.Size(264, 29);
            this.cmbAddrList.SymbolSize = 24;
            this.cmbAddrList.TabIndex = 1;
            this.cmbAddrList.Text = "uiComboBox1";
            this.cmbAddrList.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbAddrList.Watermark = "";
            // 
            // btnStart
            // 
            this.btnStart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStart.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStart.Location = new System.Drawing.Point(388, 42);
            this.btnStart.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(100, 35);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "Start";
            this.btnStart.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 
            // gpLeds
            // 
            this.gpLeds.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gpLeds.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(236)))), ((int)(((byte)(255)))));
            this.gpLeds.Location = new System.Drawing.Point(4, 87);
            this.gpLeds.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gpLeds.MinimumSize = new System.Drawing.Size(1, 1);
            this.gpLeds.Name = "gpLeds";
            this.gpLeds.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.gpLeds.SelectedIndexes = ((System.Collections.Generic.List<int>)(resources.GetObject("gpLeds.SelectedIndexes")));
            this.gpLeds.Size = new System.Drawing.Size(970, 490);
            this.gpLeds.TabIndex = 3;
            this.gpLeds.Text = "LED Ctrl";
            this.gpLeds.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnStop
            // 
            this.btnStop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStop.Enabled = false;
            this.btnStop.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnStop.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnStop.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(115)))), ((int)(((byte)(115)))));
            this.btnStop.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnStop.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnStop.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStop.LightColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.btnStop.Location = new System.Drawing.Point(504, 42);
            this.btnStop.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnStop.Name = "btnStop";
            this.btnStop.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnStop.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(115)))), ((int)(((byte)(115)))));
            this.btnStop.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnStop.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnStop.Size = new System.Drawing.Size(100, 35);
            this.btnStop.Style = Sunny.UI.UIStyle.Custom;
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "Stop";
            this.btnStop.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 
            // FrmTps92664
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.MintCream;
            this.ClientSize = new System.Drawing.Size(978, 582);
            this.Controls.Add(this.gpLeds);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.cmbAddrList);
            this.Controls.Add(this.lblAddr);
            this.MaximizeBox = false;
            this.Name = "FrmTps92664";
            this.Text = "TPS_92664/92665";
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 800, 450);
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UIMarkLabel lblAddr;
        private Sunny.UI.UIComboBox cmbAddrList;
        private Sunny.UI.UIButton btnStart;
        private Sunny.UI.UICheckBoxGroup gpLeds;
        private Sunny.UI.UIButton btnStop;
    }
}