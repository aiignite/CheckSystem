using System.Drawing;
using HZH_Controls.Controls.Btn;

namespace CheckSystem.HelperForms.Asl5115
{
    partial class AslCanDeviceMgrForm
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
            this.btnConnect = new UCBtnExt();
            this.ucmbIpAddrList = new UserControls.LabelCombox();
            this.ucmbCANList = new UserControls.LabelCombox();
            this.SuspendLayout();
            // 
            // ucmbDevicesList
            // 
            this.ucmbDevicesList.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucmbDevicesList.LabelString = "设备类型：";
            this.ucmbDevicesList.Location = new System.Drawing.Point(29, 46);
            this.ucmbDevicesList.Margin = new System.Windows.Forms.Padding(0);
            this.ucmbDevicesList.Name = "ucmbDevicesList";
            this.ucmbDevicesList.Size = new System.Drawing.Size(527, 37);
            this.ucmbDevicesList.TabIndex = 0;
            // 
            // btnConnect
            // 
            this.btnConnect.BackColor = System.Drawing.Color.White;
            this.btnConnect.BtnBackColor = System.Drawing.Color.White;
            this.btnConnect.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConnect.BtnForeColor = System.Drawing.Color.White;
            this.btnConnect.BtnText = "建立连接";
            this.btnConnect.ConerRadius = 5;
            this.btnConnect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConnect.EnabledMouseEffect = false;
            this.btnConnect.FillColor = Color.DarkCyan;
            this.btnConnect.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnConnect.IsRadius = true;
            this.btnConnect.IsShowRect = true;
            this.btnConnect.IsShowTips = false;
            this.btnConnect.Location = new System.Drawing.Point(129, 234);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(0);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.RectColor = Color.DarkOliveGreen;
            this.btnConnect.RectWidth = 1;
            this.btnConnect.Size = new System.Drawing.Size(303, 100);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.TabStop = false;
            this.btnConnect.TipsColor = Color.DarkSeaGreen;
            this.btnConnect.TipsText = "建立连接";
            this.btnConnect.BtnClick += new System.EventHandler(this.btnConnect_BtnClick);
            // 
            // ucmbIpAddrList
            // 
            this.ucmbIpAddrList.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucmbIpAddrList.LabelString = "地址列表：";
            this.ucmbIpAddrList.Location = new System.Drawing.Point(29, 162);
            this.ucmbIpAddrList.Margin = new System.Windows.Forms.Padding(0);
            this.ucmbIpAddrList.Name = "ucmbIpAddrList";
            this.ucmbIpAddrList.Size = new System.Drawing.Size(527, 37);
            this.ucmbIpAddrList.TabIndex = 0;
            // 
            // ucmbCANList
            // 
            this.ucmbCANList.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucmbCANList.LabelString = "CAN通道：";
            this.ucmbCANList.Location = new System.Drawing.Point(29, 105);
            this.ucmbCANList.Margin = new System.Windows.Forms.Padding(0);
            this.ucmbCANList.Name = "ucmbCANList";
            this.ucmbCANList.Size = new System.Drawing.Size(527, 37);
            this.ucmbCANList.TabIndex = 0;
            // 
            // CanDeviceMgrForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.ucmbCANList);
            this.Controls.Add(this.ucmbIpAddrList);
            this.Controls.Add(this.ucmbDevicesList);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(600, 400);
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "CanDeviceMgrForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设备管理";
            this.Load += new System.EventHandler(this.CanDeviceMgrForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private UserControls.LabelCombox ucmbDevicesList;
        private UCBtnExt btnConnect;
        private UserControls.LabelCombox ucmbIpAddrList;
        private UserControls.LabelCombox ucmbCANList;
    }
}