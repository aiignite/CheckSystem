namespace CheckSystem.EmulatorForms
{
    partial class EmulatorProductionSelectForm
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
            this.ucmbIpAddrList = new UserControls.LabelCombox();
            this.btnConnect = new Sunny.UI.UISymbolButton();
            this.SuspendLayout();
            // 
            // ucmbIpAddrList
            // 
            this.ucmbIpAddrList.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucmbIpAddrList.LabelString = "产品列表：";
            this.ucmbIpAddrList.Location = new System.Drawing.Point(36, 68);
            this.ucmbIpAddrList.Margin = new System.Windows.Forms.Padding(0);
            this.ucmbIpAddrList.Name = "ucmbIpAddrList";
            this.ucmbIpAddrList.Size = new System.Drawing.Size(527, 37);
            this.ucmbIpAddrList.TabIndex = 7;
            // 
            // btnConnect
            // 
            this.btnConnect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConnect.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConnect.Location = new System.Drawing.Point(108, 138);
            this.btnConnect.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(366, 68);
            this.btnConnect.TabIndex = 8;
            this.btnConnect.Text = "确认";
            this.btnConnect.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_BtnClick);
            // 
            // EmulatorProductionSelectForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(600, 250);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.ucmbIpAddrList);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(600, 250);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 250);
            this.Name = "EmulatorProductionSelectForm";
            this.Text = "产品选择";
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 600, 400);
            this.ResumeLayout(false);

        }

        #endregion
        private UserControls.LabelCombox ucmbIpAddrList;
        private Sunny.UI.UISymbolButton btnConnect;
    }
}