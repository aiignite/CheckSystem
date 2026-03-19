using Sunny.UI;
using System.Windows.Forms;

namespace CheckSystem.HelperForms.Tps92662
{
    partial class LedControlWithPwm
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new Sunny.UI.UIGroupBox();
            this.uiTableLayoutPanel1 = new Sunny.UI.UITableLayoutPanel();
            this.txtWidthValue = new System.Windows.Forms.NumericUpDown();
            this.widthTrackBar = new System.Windows.Forms.TrackBar();
            this.uiMarkLabel2 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel1 = new Sunny.UI.UIMarkLabel();
            this.btnOnOff = new Sunny.UI.UISwitch();
            this.phaseTrackBar = new System.Windows.Forms.TrackBar();
            this.txtPhaseValue = new System.Windows.Forms.NumericUpDown();
            this.groupBox1.SuspendLayout();
            this.uiTableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtWidthValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.phaseTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPhaseValue)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.uiTableLayoutPanel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.MinimumSize = new System.Drawing.Size(1, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(5, 32, 5, 5);
            this.groupBox1.Size = new System.Drawing.Size(500, 500);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.Text = "uiGroupBox1";
            this.groupBox1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiTableLayoutPanel1
            // 
            this.uiTableLayoutPanel1.ColumnCount = 2;
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.uiTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.uiTableLayoutPanel1.Controls.Add(this.txtWidthValue, 0, 5);
            this.uiTableLayoutPanel1.Controls.Add(this.widthTrackBar, 0, 4);
            this.uiTableLayoutPanel1.Controls.Add(this.uiMarkLabel2, 0, 3);
            this.uiTableLayoutPanel1.Controls.Add(this.uiMarkLabel1, 0, 0);
            this.uiTableLayoutPanel1.Controls.Add(this.btnOnOff, 0, 6);
            this.uiTableLayoutPanel1.Controls.Add(this.phaseTrackBar, 0, 1);
            this.uiTableLayoutPanel1.Controls.Add(this.txtPhaseValue, 0, 2);
            this.uiTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTableLayoutPanel1.Location = new System.Drawing.Point(5, 32);
            this.uiTableLayoutPanel1.Name = "uiTableLayoutPanel1";
            this.uiTableLayoutPanel1.RowCount = 9;
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.uiTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.uiTableLayoutPanel1.Size = new System.Drawing.Size(490, 463);
            this.uiTableLayoutPanel1.TabIndex = 0;
            this.uiTableLayoutPanel1.TagString = null;
            // 
            // txtWidthValue
            // 
            this.uiTableLayoutPanel1.SetColumnSpan(this.txtWidthValue, 2);
            this.txtWidthValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtWidthValue.Font = new System.Drawing.Font("宋体", 8F);
            this.txtWidthValue.Location = new System.Drawing.Point(3, 258);
            this.txtWidthValue.Maximum = new decimal(new int[] {
            1023,
            0,
            0,
            0});
            this.txtWidthValue.Name = "txtWidthValue";
            this.txtWidthValue.Size = new System.Drawing.Size(484, 20);
            this.txtWidthValue.TabIndex = 8;
            this.txtWidthValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // widthTrackBar
            // 
            this.uiTableLayoutPanel1.SetColumnSpan(this.widthTrackBar, 2);
            this.widthTrackBar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.widthTrackBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.widthTrackBar.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.widthTrackBar.Location = new System.Drawing.Point(5, 209);
            this.widthTrackBar.Margin = new System.Windows.Forms.Padding(5);
            this.widthTrackBar.Maximum = 1023;
            this.widthTrackBar.MinimumSize = new System.Drawing.Size(1, 1);
            this.widthTrackBar.Name = "widthTrackBar";
            this.widthTrackBar.Size = new System.Drawing.Size(480, 41);
            this.widthTrackBar.TabIndex = 6;
            this.widthTrackBar.Text = "uiTrackBar2";
            // 
            // uiMarkLabel2
            // 
            this.uiMarkLabel2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.uiTableLayoutPanel1.SetColumnSpan(this.uiMarkLabel2, 2);
            this.uiMarkLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMarkLabel2.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiMarkLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel2.Location = new System.Drawing.Point(3, 156);
            this.uiMarkLabel2.Margin = new System.Windows.Forms.Padding(3);
            this.uiMarkLabel2.MarkPos = Sunny.UI.UIMarkLabel.UIMarkPos.Bottom;
            this.uiMarkLabel2.Name = "uiMarkLabel2";
            this.uiMarkLabel2.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.uiMarkLabel2.Size = new System.Drawing.Size(484, 45);
            this.uiMarkLabel2.TabIndex = 1;
            this.uiMarkLabel2.Text = "Width";
            this.uiMarkLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel1
            // 
            this.uiMarkLabel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.uiTableLayoutPanel1.SetColumnSpan(this.uiMarkLabel1, 2);
            this.uiMarkLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMarkLabel1.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiMarkLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel1.Location = new System.Drawing.Point(3, 3);
            this.uiMarkLabel1.Margin = new System.Windows.Forms.Padding(3);
            this.uiMarkLabel1.MarkPos = Sunny.UI.UIMarkLabel.UIMarkPos.Bottom;
            this.uiMarkLabel1.Name = "uiMarkLabel1";
            this.uiMarkLabel1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.uiMarkLabel1.Size = new System.Drawing.Size(484, 45);
            this.uiMarkLabel1.TabIndex = 0;
            this.uiMarkLabel1.Text = "Phase";
            this.uiMarkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnOnOff
            // 
            this.uiTableLayoutPanel1.SetColumnSpan(this.btnOnOff, 2);
            this.btnOnOff.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOnOff.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOnOff.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOnOff.Location = new System.Drawing.Point(10, 316);
            this.btnOnOff.Margin = new System.Windows.Forms.Padding(10);
            this.btnOnOff.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnOnOff.Name = "btnOnOff";
            this.uiTableLayoutPanel1.SetRowSpan(this.btnOnOff, 3);
            this.btnOnOff.Size = new System.Drawing.Size(470, 137);
            this.btnOnOff.TabIndex = 4;
            this.btnOnOff.Text = "uiSwitch1";
            // 
            // phaseTrackBar
            // 
            this.uiTableLayoutPanel1.SetColumnSpan(this.phaseTrackBar, 2);
            this.phaseTrackBar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.phaseTrackBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.phaseTrackBar.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.phaseTrackBar.Location = new System.Drawing.Point(5, 56);
            this.phaseTrackBar.Margin = new System.Windows.Forms.Padding(5);
            this.phaseTrackBar.Maximum = 1023;
            this.phaseTrackBar.MinimumSize = new System.Drawing.Size(1, 1);
            this.phaseTrackBar.Name = "phaseTrackBar";
            this.phaseTrackBar.Size = new System.Drawing.Size(480, 41);
            this.phaseTrackBar.TabIndex = 5;
            this.phaseTrackBar.Text = "uiTrackBar1";
            // 
            // txtPhaseValue
            // 
            this.uiTableLayoutPanel1.SetColumnSpan(this.txtPhaseValue, 2);
            this.txtPhaseValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPhaseValue.Font = new System.Drawing.Font("宋体", 8F);
            this.txtPhaseValue.Location = new System.Drawing.Point(3, 105);
            this.txtPhaseValue.Maximum = new decimal(new int[] {
            1023,
            0,
            0,
            0});
            this.txtPhaseValue.Name = "txtPhaseValue";
            this.txtPhaseValue.Size = new System.Drawing.Size(484, 20);
            this.txtPhaseValue.TabIndex = 7;
            this.txtPhaseValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // LedControlWithPwm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "LedControlWithPwm";
            this.Size = new System.Drawing.Size(500, 500);
            this.groupBox1.ResumeLayout(false);
            this.uiTableLayoutPanel1.ResumeLayout(false);
            this.uiTableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtWidthValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.phaseTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPhaseValue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public UIGroupBox groupBox1;
        public UITableLayoutPanel uiTableLayoutPanel1;
        public UIMarkLabel uiMarkLabel2;
        public UIMarkLabel uiMarkLabel1;
        public UISwitch btnOnOff;
        public System.Windows.Forms.NumericUpDown txtWidthValue;
        public TrackBar widthTrackBar;
        public TrackBar phaseTrackBar;
        public System.Windows.Forms.NumericUpDown txtPhaseValue;
    }
}
