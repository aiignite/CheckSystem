namespace CheckSystem.HelperForms.AsuvReWrite
{
    partial class FrmAsuvReWrite
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
            this.uiTitlePanel1 = new Sunny.UI.UITitlePanel();
            this.txtSerialNo = new Sunny.UI.UITextBox();
            this.txtFazit = new Sunny.UI.UITextBox();
            this.ledResult = new Sunny.UI.UILedBulb();
            this.btnStart = new Sunny.UI.UIButton();
            this.lblInfo = new Sunny.UI.UIMarkLabel();
            this.lblSerialNo = new Sunny.UI.UIMarkLabel();
            this.lblFazit = new Sunny.UI.UIMarkLabel();
            this.uiTitlePanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiTitlePanel1
            // 
            this.uiTitlePanel1.Controls.Add(this.txtSerialNo);
            this.uiTitlePanel1.Controls.Add(this.txtFazit);
            this.uiTitlePanel1.Controls.Add(this.ledResult);
            this.uiTitlePanel1.Controls.Add(this.btnStart);
            this.uiTitlePanel1.Controls.Add(this.lblInfo);
            this.uiTitlePanel1.Controls.Add(this.lblSerialNo);
            this.uiTitlePanel1.Controls.Add(this.lblFazit);
            this.uiTitlePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTitlePanel1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiTitlePanel1.Location = new System.Drawing.Point(0, 0);
            this.uiTitlePanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiTitlePanel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiTitlePanel1.Name = "uiTitlePanel1";
            this.uiTitlePanel1.ShowText = false;
            this.uiTitlePanel1.Size = new System.Drawing.Size(784, 361);
            this.uiTitlePanel1.TabIndex = 0;
            this.uiTitlePanel1.Text = "当前未选择机种";
            this.uiTitlePanel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtSerialNo
            // 
            this.txtSerialNo.BackColor = System.Drawing.Color.LightGray;
            this.txtSerialNo.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSerialNo.Font = new System.Drawing.Font("黑体", 8F);
            this.txtSerialNo.Location = new System.Drawing.Point(110, 97);
            this.txtSerialNo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtSerialNo.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtSerialNo.Name = "txtSerialNo";
            this.txtSerialNo.Padding = new System.Windows.Forms.Padding(5);
            this.txtSerialNo.ReadOnly = true;
            this.txtSerialNo.ShowText = false;
            this.txtSerialNo.Size = new System.Drawing.Size(670, 29);
            this.txtSerialNo.TabIndex = 9;
            this.txtSerialNo.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtSerialNo.Watermark = "";
            // 
            // txtFazit
            // 
            this.txtFazit.BackColor = System.Drawing.Color.LightGray;
            this.txtFazit.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtFazit.Font = new System.Drawing.Font("黑体", 8F);
            this.txtFazit.Location = new System.Drawing.Point(110, 58);
            this.txtFazit.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtFazit.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtFazit.Name = "txtFazit";
            this.txtFazit.Padding = new System.Windows.Forms.Padding(5);
            this.txtFazit.ReadOnly = true;
            this.txtFazit.ShowText = false;
            this.txtFazit.Size = new System.Drawing.Size(670, 29);
            this.txtFazit.TabIndex = 10;
            this.txtFazit.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtFazit.Watermark = "";
            // 
            // ledResult
            // 
            this.ledResult.Location = new System.Drawing.Point(670, 163);
            this.ledResult.Name = "ledResult";
            this.ledResult.On = false;
            this.ledResult.Size = new System.Drawing.Size(77, 56);
            this.ledResult.TabIndex = 8;
            this.ledResult.Text = "uiLedBulb1";
            // 
            // btnStart
            // 
            this.btnStart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStart.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStart.Location = new System.Drawing.Point(29, 154);
            this.btnStart.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(604, 75);
            this.btnStart.TabIndex = 7;
            this.btnStart.Text = "START";
            this.btnStart.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 
            // lblInfo
            // 
            this.lblInfo.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.lblInfo.Location = new System.Drawing.Point(21, 247);
            this.lblInfo.MarkPos = Sunny.UI.UIMarkLabel.UIMarkPos.Top;
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.lblInfo.Size = new System.Drawing.Size(738, 79);
            this.lblInfo.TabIndex = 4;
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSerialNo
            // 
            this.lblSerialNo.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSerialNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.lblSerialNo.Location = new System.Drawing.Point(3, 103);
            this.lblSerialNo.Name = "lblSerialNo";
            this.lblSerialNo.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.lblSerialNo.Size = new System.Drawing.Size(100, 23);
            this.lblSerialNo.TabIndex = 5;
            this.lblSerialNo.Text = "SerialNo";
            this.lblSerialNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblFazit
            // 
            this.lblFazit.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblFazit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.lblFazit.Location = new System.Drawing.Point(3, 58);
            this.lblFazit.Name = "lblFazit";
            this.lblFazit.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.lblFazit.Size = new System.Drawing.Size(100, 23);
            this.lblFazit.TabIndex = 6;
            this.lblFazit.Text = "Fazit";
            this.lblFazit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FrmAsuvReWrite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 361);
            this.Controls.Add(this.uiTitlePanel1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(800, 400);
            this.MinimumSize = new System.Drawing.Size(800, 400);
            this.Name = "FrmAsuvReWrite";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Write";
            this.uiTitlePanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UITitlePanel uiTitlePanel1;
        private Sunny.UI.UITextBox txtSerialNo;
        private Sunny.UI.UITextBox txtFazit;
        private Sunny.UI.UILedBulb ledResult;
        private Sunny.UI.UIButton btnStart;
        private Sunny.UI.UIMarkLabel lblInfo;
        private Sunny.UI.UIMarkLabel lblSerialNo;
        private Sunny.UI.UIMarkLabel lblFazit;
    }
}