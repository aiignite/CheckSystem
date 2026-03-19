using HZH_Controls.Controls.Btn;

namespace CheckSystem.HelperForms.P12LHeadLamp
{
    partial class P12LHeadLampDeviceMgr
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
            this.btnConnect = new HZH_Controls.Controls.Btn.UCBtnExt();
            this.ucmbCAN2List = new UserControls.LabelCombox();
            this.ucmbCAN1List = new UserControls.LabelCombox();
            this.ucmbProductLevel = new UserControls.LabelCombox();
            this.ucmLin6List = new UserControls.LabelCombox();
            this.ucmLin5List = new UserControls.LabelCombox();
            this.SuspendLayout();
            // 
            // ucmbDevicesList
            // 
            this.ucmbDevicesList.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucmbDevicesList.LabelString = "设备类型：";
            this.ucmbDevicesList.Location = new System.Drawing.Point(29, 77);
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
            this.btnConnect.ConerRadius = 25;
            this.btnConnect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConnect.EnabledMouseEffect = false;
            this.btnConnect.FillColor = System.Drawing.Color.DarkCyan;
            this.btnConnect.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnConnect.IsRadius = true;
            this.btnConnect.IsShowRect = true;
            this.btnConnect.IsShowTips = false;
            this.btnConnect.Location = new System.Drawing.Point(138, 350);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(0);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.RectColor = System.Drawing.Color.DarkOliveGreen;
            this.btnConnect.RectWidth = 1;
            this.btnConnect.Size = new System.Drawing.Size(293, 89);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.TabStop = false;
            this.btnConnect.TipsColor = System.Drawing.Color.DarkSeaGreen;
            this.btnConnect.TipsText = "建立连接";
            this.btnConnect.BtnClick += new System.EventHandler(this.btnConnect_BtnClick);
            // 
            // ucmbCAN2List
            // 
            this.ucmbCAN2List.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucmbCAN2List.LabelString = "远近光CAN2通道：";
            this.ucmbCAN2List.Location = new System.Drawing.Point(29, 292);
            this.ucmbCAN2List.Margin = new System.Windows.Forms.Padding(0);
            this.ucmbCAN2List.Name = "ucmbCAN2List";
            this.ucmbCAN2List.Size = new System.Drawing.Size(527, 37);
            this.ucmbCAN2List.TabIndex = 0;
            // 
            // ucmbCAN1List
            // 
            this.ucmbCAN1List.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucmbCAN1List.LabelString = "信号灯CAN1通道：";
            this.ucmbCAN1List.Location = new System.Drawing.Point(29, 235);
            this.ucmbCAN1List.Margin = new System.Windows.Forms.Padding(0);
            this.ucmbCAN1List.Name = "ucmbCAN1List";
            this.ucmbCAN1List.Size = new System.Drawing.Size(527, 37);
            this.ucmbCAN1List.TabIndex = 0;
            // 
            // ucmbProductLevel
            // 
            this.ucmbProductLevel.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucmbProductLevel.LabelString = "高/低配：";
            this.ucmbProductLevel.Location = new System.Drawing.Point(29, 22);
            this.ucmbProductLevel.Margin = new System.Windows.Forms.Padding(0);
            this.ucmbProductLevel.Name = "ucmbProductLevel";
            this.ucmbProductLevel.Size = new System.Drawing.Size(527, 37);
            this.ucmbProductLevel.TabIndex = 0;
            // 
            // ucmLin6List
            // 
            this.ucmLin6List.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucmLin6List.LabelString = "LIN6通道：";
            this.ucmLin6List.Location = new System.Drawing.Point(29, 128);
            this.ucmLin6List.Margin = new System.Windows.Forms.Padding(0);
            this.ucmLin6List.Name = "ucmLin6List";
            this.ucmLin6List.Size = new System.Drawing.Size(527, 37);
            this.ucmLin6List.TabIndex = 0;
            // 
            // ucmLin5List
            // 
            this.ucmLin5List.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucmLin5List.LabelString = "LIN5通道：";
            this.ucmLin5List.Location = new System.Drawing.Point(29, 183);
            this.ucmLin5List.Margin = new System.Windows.Forms.Padding(0);
            this.ucmLin5List.Name = "ucmLin5List";
            this.ucmLin5List.Size = new System.Drawing.Size(527, 37);
            this.ucmLin5List.TabIndex = 0;
            // 
            // P12LHighLevelHeadLampDeviceMgr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 461);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.ucmLin5List);
            this.Controls.Add(this.ucmLin6List);
            this.Controls.Add(this.ucmbCAN1List);
            this.Controls.Add(this.ucmbCAN2List);
            this.Controls.Add(this.ucmbProductLevel);
            this.Controls.Add(this.ucmbDevicesList);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(600, 500);
            this.MinimumSize = new System.Drawing.Size(600, 500);
            this.Name = "P12LHighLevelHeadLampDeviceMgr";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设备管理";
            this.Load += new System.EventHandler(this.CanDeviceMgrForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private UserControls.LabelCombox ucmbDevicesList;
        private UCBtnExt btnConnect;
        private UserControls.LabelCombox ucmbCAN2List;
        private UserControls.LabelCombox ucmbCAN1List;
        private UserControls.LabelCombox ucmbProductLevel;
        private UserControls.LabelCombox ucmLin6List;
        private UserControls.LabelCombox ucmLin5List;
    }
}