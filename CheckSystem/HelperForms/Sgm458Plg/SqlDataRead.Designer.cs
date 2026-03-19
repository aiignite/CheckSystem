namespace CheckSystem.HelperForms.Sgm458Plg
{
    partial class SqlDataRead
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.titlePanel = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.endTime = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.startTime = new System.Windows.Forms.DateTimePicker();
            this.btnSearch = new System.Windows.Forms.Button();
            this.cmbSqlIpAddr = new System.Windows.Forms.ComboBox();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnSearchNoUsedEcuIds = new System.Windows.Forms.Button();
            this.keyData = new UserControls.UserDataGrid();
            this.tableLayoutPanel1.SuspendLayout();
            this.titlePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.titlePanel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.keyData, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1169, 598);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // titlePanel
            // 
            this.titlePanel.BackColor = System.Drawing.Color.DarkGoldenrod;
            this.titlePanel.ColumnCount = 5;
            this.titlePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.titlePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.titlePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.titlePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.titlePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.titlePanel.Controls.Add(this.label3, 0, 1);
            this.titlePanel.Controls.Add(this.endTime, 3, 0);
            this.titlePanel.Controls.Add(this.label2, 2, 0);
            this.titlePanel.Controls.Add(this.label1, 0, 0);
            this.titlePanel.Controls.Add(this.startTime, 1, 0);
            this.titlePanel.Controls.Add(this.btnSearch, 4, 0);
            this.titlePanel.Controls.Add(this.cmbSqlIpAddr, 1, 1);
            this.titlePanel.Controls.Add(this.btnImport, 3, 1);
            this.titlePanel.Controls.Add(this.btnExport, 4, 1);
            this.titlePanel.Controls.Add(this.btnSearchNoUsedEcuIds, 2, 1);
            this.titlePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titlePanel.Location = new System.Drawing.Point(3, 3);
            this.titlePanel.Name = "titlePanel";
            this.titlePanel.RowCount = 2;
            this.titlePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.titlePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.titlePanel.Size = new System.Drawing.Size(1163, 83);
            this.titlePanel.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("黑体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(5, 46);
            this.label3.Margin = new System.Windows.Forms.Padding(5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(148, 21);
            this.label3.TabIndex = 5;
            this.label3.Text = "数据库地址：";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // endTime
            // 
            this.endTime.CalendarFont = new System.Drawing.Font("黑体", 15.75F, System.Drawing.FontStyle.Bold);
            this.endTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.endTime.Location = new System.Drawing.Point(699, 3);
            this.endTime.Name = "endTime";
            this.endTime.Size = new System.Drawing.Size(226, 21);
            this.endTime.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("黑体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(469, 5);
            this.label2.Margin = new System.Windows.Forms.Padding(5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 21);
            this.label2.TabIndex = 2;
            this.label2.Text = "结束日期：";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("黑体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(5, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "开始日期：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // startTime
            // 
            this.startTime.CalendarFont = new System.Drawing.Font("黑体", 15.75F, System.Drawing.FontStyle.Bold);
            this.startTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.startTime.Location = new System.Drawing.Point(235, 3);
            this.startTime.Name = "startTime";
            this.startTime.Size = new System.Drawing.Size(226, 21);
            this.startTime.TabIndex = 1;
            // 
            // btnSearch
            // 
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSearch.Font = new System.Drawing.Font("黑体", 15.75F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Location = new System.Drawing.Point(928, 0);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(0);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(235, 41);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "SEARCH";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cmbSqlIpAddr
            // 
            this.cmbSqlIpAddr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbSqlIpAddr.FormattingEnabled = true;
            this.cmbSqlIpAddr.Location = new System.Drawing.Point(235, 44);
            this.cmbSqlIpAddr.Name = "cmbSqlIpAddr";
            this.cmbSqlIpAddr.Size = new System.Drawing.Size(226, 20);
            this.cmbSqlIpAddr.TabIndex = 6;
            // 
            // btnImport
            // 
            this.btnImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnImport.Font = new System.Drawing.Font("黑体", 15.75F, System.Drawing.FontStyle.Bold);
            this.btnImport.Location = new System.Drawing.Point(696, 41);
            this.btnImport.Margin = new System.Windows.Forms.Padding(0);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(232, 42);
            this.btnImport.TabIndex = 7;
            this.btnImport.Text = "导入KEY";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnExport
            // 
            this.btnExport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExport.Font = new System.Drawing.Font("黑体", 15.75F, System.Drawing.FontStyle.Bold);
            this.btnExport.Location = new System.Drawing.Point(928, 41);
            this.btnExport.Margin = new System.Windows.Forms.Padding(0);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(235, 42);
            this.btnExport.TabIndex = 8;
            this.btnExport.Text = "导出KEY";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnSearchNoUsedEcuIds
            // 
            this.btnSearchNoUsedEcuIds.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSearchNoUsedEcuIds.Font = new System.Drawing.Font("黑体", 15.75F, System.Drawing.FontStyle.Bold);
            this.btnSearchNoUsedEcuIds.Location = new System.Drawing.Point(467, 44);
            this.btnSearchNoUsedEcuIds.Name = "btnSearchNoUsedEcuIds";
            this.btnSearchNoUsedEcuIds.Size = new System.Drawing.Size(226, 36);
            this.btnSearchNoUsedEcuIds.TabIndex = 9;
            this.btnSearchNoUsedEcuIds.Text = "查询可用的ECU-ID";
            this.btnSearchNoUsedEcuIds.UseVisualStyleBackColor = true;
            this.btnSearchNoUsedEcuIds.Click += new System.EventHandler(this.btnSearchNoUsedEcuIds_Click);
            // 
            // keyData
            // 
            this.keyData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.keyData.Location = new System.Drawing.Point(3, 92);
            this.keyData.Name = "keyData";
            this.keyData.Size = new System.Drawing.Size(1163, 503);
            this.keyData.TabIndex = 0;
            // 
            // SqlDataRead
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1169, 598);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SqlDataRead";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SGM458-PLG数据查询";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.titlePanel.ResumeLayout(false);
            this.titlePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private UserControls.UserDataGrid keyData;
        private System.Windows.Forms.TableLayoutPanel titlePanel;
        private System.Windows.Forms.DateTimePicker endTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker startTime;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbSqlIpAddr;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnSearchNoUsedEcuIds;
    }
}