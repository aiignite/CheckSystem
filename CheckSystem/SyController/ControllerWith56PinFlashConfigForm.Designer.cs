namespace CheckSystem.SyController
{
    partial class ControllerWith56PinFlashConfigForm
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
            this.btnTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.btnEditFlashConfig = new System.Windows.Forms.Button();
            this.btnWriteConfigFile = new System.Windows.Forms.Button();
            this.btnEditConfigFile = new System.Windows.Forms.Button();
            this.txtLastReadFlashTime = new UserControls.LabelText();
            this.flashConfigData = new UserControls.UserDataGrid();
            this.backgroundPanel = new System.Windows.Forms.TableLayoutPanel();
            this.btnReadFlashConfig = new Sunny.UI.UISymbolButton();
            this.btnWriteFlashConfig = new Sunny.UI.UISymbolButton();
            this.btnTableLayout.SuspendLayout();
            this.backgroundPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnTableLayout
            // 
            this.btnTableLayout.ColumnCount = 3;
            this.btnTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.btnTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.btnTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.btnTableLayout.Controls.Add(this.btnWriteFlashConfig, 2, 1);
            this.btnTableLayout.Controls.Add(this.btnEditFlashConfig, 0, 1);
            this.btnTableLayout.Controls.Add(this.btnWriteConfigFile, 1, 0);
            this.btnTableLayout.Controls.Add(this.btnEditConfigFile, 0, 0);
            this.btnTableLayout.Controls.Add(this.txtLastReadFlashTime, 0, 2);
            this.btnTableLayout.Controls.Add(this.btnReadFlashConfig, 1, 1);
            this.btnTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTableLayout.Location = new System.Drawing.Point(3, 3);
            this.btnTableLayout.Name = "btnTableLayout";
            this.btnTableLayout.RowCount = 3;
            this.btnTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.btnTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.btnTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.btnTableLayout.Size = new System.Drawing.Size(915, 144);
            this.btnTableLayout.TabIndex = 2;
            // 
            // btnEditFlashConfig
            // 
            this.btnEditFlashConfig.BackColor = System.Drawing.Color.DarkGoldenrod;
            this.btnEditFlashConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEditFlashConfig.Location = new System.Drawing.Point(0, 47);
            this.btnEditFlashConfig.Margin = new System.Windows.Forms.Padding(0);
            this.btnEditFlashConfig.Name = "btnEditFlashConfig";
            this.btnEditFlashConfig.Size = new System.Drawing.Size(305, 47);
            this.btnEditFlashConfig.TabIndex = 3;
            this.btnEditFlashConfig.Text = "编辑FLASH配置参数";
            this.btnEditFlashConfig.UseVisualStyleBackColor = false;
            this.btnEditFlashConfig.Click += new System.EventHandler(this.btnEditFlashConfig_Click);
            // 
            // btnWriteConfigFile
            // 
            this.btnWriteConfigFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnWriteConfigFile.Location = new System.Drawing.Point(305, 0);
            this.btnWriteConfigFile.Margin = new System.Windows.Forms.Padding(0);
            this.btnWriteConfigFile.Name = "btnWriteConfigFile";
            this.btnWriteConfigFile.Size = new System.Drawing.Size(305, 47);
            this.btnWriteConfigFile.TabIndex = 1;
            this.btnWriteConfigFile.Text = "更新FLASH地址配置文件";
            this.btnWriteConfigFile.UseVisualStyleBackColor = true;
            // 
            // btnEditConfigFile
            // 
            this.btnEditConfigFile.BackColor = System.Drawing.Color.DarkGoldenrod;
            this.btnEditConfigFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEditConfigFile.Location = new System.Drawing.Point(0, 0);
            this.btnEditConfigFile.Margin = new System.Windows.Forms.Padding(0);
            this.btnEditConfigFile.Name = "btnEditConfigFile";
            this.btnEditConfigFile.Size = new System.Drawing.Size(305, 47);
            this.btnEditConfigFile.TabIndex = 0;
            this.btnEditConfigFile.Text = "编辑FLASH地址配置文件";
            this.btnEditConfigFile.UseVisualStyleBackColor = false;
            this.btnEditConfigFile.Click += new System.EventHandler(this.btnEditConfigFile_Click);
            // 
            // txtLastReadFlashTime
            // 
            this.btnTableLayout.SetColumnSpan(this.txtLastReadFlashTime, 3);
            this.txtLastReadFlashTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLastReadFlashTime.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtLastReadFlashTime.LabelString = "上次读取时间：";
            this.txtLastReadFlashTime.Location = new System.Drawing.Point(2, 96);
            this.txtLastReadFlashTime.Margin = new System.Windows.Forms.Padding(2);
            this.txtLastReadFlashTime.Name = "txtLastReadFlashTime";
            this.txtLastReadFlashTime.Size = new System.Drawing.Size(911, 46);
            this.txtLastReadFlashTime.TabIndex = 5;
            // 
            // flashConfigData
            // 
            this.flashConfigData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flashConfigData.Location = new System.Drawing.Point(3, 153);
            this.flashConfigData.Name = "flashConfigData";
            this.flashConfigData.Size = new System.Drawing.Size(915, 436);
            this.flashConfigData.TabIndex = 1;
            // 
            // backgroundPanel
            // 
            this.backgroundPanel.ColumnCount = 1;
            this.backgroundPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.backgroundPanel.Controls.Add(this.flashConfigData, 0, 1);
            this.backgroundPanel.Controls.Add(this.btnTableLayout, 0, 0);
            this.backgroundPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.backgroundPanel.Location = new System.Drawing.Point(0, 35);
            this.backgroundPanel.Name = "backgroundPanel";
            this.backgroundPanel.RowCount = 2;
            this.backgroundPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.backgroundPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.backgroundPanel.Size = new System.Drawing.Size(921, 592);
            this.backgroundPanel.TabIndex = 0;
            // 
            // btnReadFlashConfig
            // 
            this.btnReadFlashConfig.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReadFlashConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReadFlashConfig.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnReadFlashConfig.Location = new System.Drawing.Point(306, 48);
            this.btnReadFlashConfig.Margin = new System.Windows.Forms.Padding(1);
            this.btnReadFlashConfig.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnReadFlashConfig.Name = "btnReadFlashConfig";
            this.btnReadFlashConfig.Size = new System.Drawing.Size(303, 45);
            this.btnReadFlashConfig.Symbol = 557539;
            this.btnReadFlashConfig.TabIndex = 6;
            this.btnReadFlashConfig.Text = "读取FLASH配置";
            this.btnReadFlashConfig.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnReadFlashConfig.Click += new System.EventHandler(this.btnReadFlashConfig_Click);
            // 
            // btnWriteFlashConfig
            // 
            this.btnWriteFlashConfig.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnWriteFlashConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnWriteFlashConfig.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnWriteFlashConfig.Location = new System.Drawing.Point(611, 48);
            this.btnWriteFlashConfig.Margin = new System.Windows.Forms.Padding(1);
            this.btnWriteFlashConfig.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnWriteFlashConfig.Name = "btnWriteFlashConfig";
            this.btnWriteFlashConfig.Size = new System.Drawing.Size(303, 45);
            this.btnWriteFlashConfig.Symbol = 559524;
            this.btnWriteFlashConfig.TabIndex = 7;
            this.btnWriteFlashConfig.Text = "写入FLASH配置";
            this.btnWriteFlashConfig.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnWriteFlashConfig.Click += new System.EventHandler(this.btnWriteFlashConfig_Click);
            // 
            // ControllerWith56PinFlashConfigForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(921, 627);
            this.Controls.Add(this.backgroundPanel);
            this.Name = "ControllerWith56PinFlashConfigForm";
            this.Text = "FLASH配置";
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 921, 627);
            this.btnTableLayout.ResumeLayout(false);
            this.backgroundPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel btnTableLayout;
        private System.Windows.Forms.Button btnEditFlashConfig;
        private System.Windows.Forms.Button btnWriteConfigFile;
        private System.Windows.Forms.Button btnEditConfigFile;
        private UserControls.UserDataGrid flashConfigData;
        private System.Windows.Forms.TableLayoutPanel backgroundPanel;
        private UserControls.LabelText txtLastReadFlashTime;
        private Sunny.UI.UISymbolButton btnWriteFlashConfig;
        private Sunny.UI.UISymbolButton btnReadFlashConfig;
    }
}