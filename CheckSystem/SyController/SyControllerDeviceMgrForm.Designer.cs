using System.Drawing;
using HZH_Controls.Controls.Btn;

namespace CheckSystem.SyController
{
    partial class SyControllerDeviceMgrForm
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
            this.ucmbDevicesList = new UserControls.LabelCombox();
            this.ucmbIpAddrList = new UserControls.LabelCombox();
            this.btnConnect = new Sunny.UI.UISymbolButton();
            this.SuspendLayout();
            // 
            // ucmbDevicesList
            // 
            this.ucmbDevicesList.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucmbDevicesList.LabelString = "设备类型：";
            this.ucmbDevicesList.Location = new System.Drawing.Point(32, 83);
            this.ucmbDevicesList.Margin = new System.Windows.Forms.Padding(0);
            this.ucmbDevicesList.Name = "ucmbDevicesList";
            this.ucmbDevicesList.Size = new System.Drawing.Size(527, 37);
            this.ucmbDevicesList.TabIndex = 0;
            // 
            // ucmbIpAddrList
            // 
            this.ucmbIpAddrList.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucmbIpAddrList.LabelString = "地址列表：";
            this.ucmbIpAddrList.Location = new System.Drawing.Point(32, 151);
            this.ucmbIpAddrList.Margin = new System.Windows.Forms.Padding(0);
            this.ucmbIpAddrList.Name = "ucmbIpAddrList";
            this.ucmbIpAddrList.Size = new System.Drawing.Size(527, 37);
            this.ucmbIpAddrList.TabIndex = 0;
            // 
            // btnConnect
            // 
            this.btnConnect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConnect.Font = new System.Drawing.Font("黑体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConnect.Location = new System.Drawing.Point(150, 254);
            this.btnConnect.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(293, 71);
            this.btnConnect.Symbol = 61515;
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "建立连接";
            this.btnConnect.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_BtnClick);
            // 
            // SyControllerDeviceMgrForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(600, 400);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.ucmbIpAddrList);
            this.Controls.Add(this.ucmbDevicesList);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(600, 400);
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "SyControllerDeviceMgrForm";
            this.Text = "设备管理";
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 600, 400);
            this.Load += new System.EventHandler(this.CanDeviceMgrForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private UserControls.LabelCombox ucmbDevicesList;
        private UserControls.LabelCombox ucmbIpAddrList;
        private Sunny.UI.UISymbolButton btnConnect;
    }
}