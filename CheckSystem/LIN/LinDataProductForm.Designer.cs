using System.Drawing;
using HZH_Controls.Controls.Btn;

namespace CheckSystem.LIN
{
    partial class LinDataProductForm
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
            this.titlePanel = new System.Windows.Forms.Panel();
            this.cmbLinList = new UserControls.LabelCombox();
            this.ucBtnExt1 = new HZH_Controls.Controls.Btn.UCBtnExt();
            this.cmbProductList = new UserControls.LabelCombox();
            this.mainPanel = new System.Windows.Forms.TableLayoutPanel();
            this.listMethods = new System.Windows.Forms.FlowLayoutPanel();
            this.dgvOutpuFields = new UserControls.UserDataGrid();
            this.dgvInputFields = new UserControls.UserDataGrid();
            this.titlePanel.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // titlePanel
            // 
            this.titlePanel.BackColor = System.Drawing.Color.AliceBlue;
            this.mainPanel.SetColumnSpan(this.titlePanel, 2);
            this.titlePanel.Controls.Add(this.cmbLinList);
            this.titlePanel.Controls.Add(this.ucBtnExt1);
            this.titlePanel.Controls.Add(this.cmbProductList);
            this.titlePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titlePanel.Location = new System.Drawing.Point(3, 3);
            this.titlePanel.Name = "titlePanel";
            this.titlePanel.Size = new System.Drawing.Size(1205, 94);
            this.titlePanel.TabIndex = 0;
            // 
            // cmbLinList
            // 
            this.cmbLinList.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbLinList.LabelString = "产品LIN接口：";
            this.cmbLinList.Location = new System.Drawing.Point(490, 23);
            this.cmbLinList.Margin = new System.Windows.Forms.Padding(0);
            this.cmbLinList.Name = "cmbLinList";
            this.cmbLinList.Size = new System.Drawing.Size(405, 46);
            this.cmbLinList.TabIndex = 2;
            // 
            // ucBtnExt1
            // 
            this.ucBtnExt1.BackColor = System.Drawing.Color.White;
            this.ucBtnExt1.BtnBackColor = System.Drawing.Color.White;
            this.ucBtnExt1.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucBtnExt1.BtnForeColor = System.Drawing.Color.White;
            this.ucBtnExt1.BtnText = "确认";
            this.ucBtnExt1.ConerRadius = 5;
            this.ucBtnExt1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucBtnExt1.EnabledMouseEffect = false;
            this.ucBtnExt1.FillColor = System.Drawing.Color.RoyalBlue;
            this.ucBtnExt1.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ucBtnExt1.IsRadius = true;
            this.ucBtnExt1.IsShowRect = true;
            this.ucBtnExt1.IsShowTips = false;
            this.ucBtnExt1.Location = new System.Drawing.Point(968, 12);
            this.ucBtnExt1.Margin = new System.Windows.Forms.Padding(0);
            this.ucBtnExt1.Name = "ucBtnExt1";
            this.ucBtnExt1.RectColor = System.Drawing.Color.DarkOliveGreen;
            this.ucBtnExt1.RectWidth = 1;
            this.ucBtnExt1.Size = new System.Drawing.Size(206, 60);
            this.ucBtnExt1.TabIndex = 1;
            this.ucBtnExt1.TabStop = false;
            this.ucBtnExt1.TipsColor = System.Drawing.Color.DarkSeaGreen;
            this.ucBtnExt1.TipsText = "";
            this.ucBtnExt1.BtnClick += new System.EventHandler(this.ucBtnExt1_BtnClick);
            // 
            // cmbProductList
            // 
            this.cmbProductList.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbProductList.LabelString = "产品列表：";
            this.cmbProductList.Location = new System.Drawing.Point(23, 23);
            this.cmbProductList.Margin = new System.Windows.Forms.Padding(0);
            this.cmbProductList.Name = "cmbProductList";
            this.cmbProductList.Size = new System.Drawing.Size(405, 46);
            this.cmbProductList.TabIndex = 0;
            // 
            // mainPanel
            // 
            this.mainPanel.ColumnCount = 2;
            this.mainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.mainPanel.Controls.Add(this.listMethods, 0, 2);
            this.mainPanel.Controls.Add(this.dgvOutpuFields, 1, 1);
            this.mainPanel.Controls.Add(this.titlePanel, 0, 0);
            this.mainPanel.Controls.Add(this.dgvInputFields, 0, 1);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.RowCount = 3;
            this.mainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.mainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.mainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.mainPanel.Size = new System.Drawing.Size(1211, 591);
            this.mainPanel.TabIndex = 0;
            // 
            // listMethods
            // 
            this.mainPanel.SetColumnSpan(this.listMethods, 2);
            this.listMethods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listMethods.Location = new System.Drawing.Point(3, 495);
            this.listMethods.Name = "listMethods";
            this.listMethods.Size = new System.Drawing.Size(1205, 93);
            this.listMethods.TabIndex = 5;
            // 
            // dgvOutpuFields
            // 
            this.dgvOutpuFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOutpuFields.Font = new System.Drawing.Font("宋体", 9F);
            this.dgvOutpuFields.Location = new System.Drawing.Point(608, 103);
            this.dgvOutpuFields.Name = "dgvOutpuFields";
            this.dgvOutpuFields.Size = new System.Drawing.Size(600, 386);
            this.dgvOutpuFields.TabIndex = 2;
            // 
            // dgvInputFields
            // 
            this.dgvInputFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvInputFields.Font = new System.Drawing.Font("宋体", 9F);
            this.dgvInputFields.Location = new System.Drawing.Point(3, 103);
            this.dgvInputFields.Name = "dgvInputFields";
            this.dgvInputFields.Size = new System.Drawing.Size(599, 386);
            this.dgvInputFields.TabIndex = 1;
            // 
            // LinDataProductForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1211, 591);
            this.Controls.Add(this.mainPanel);
            this.Name = "LinDataProductForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LinDataProductFrom";
            this.titlePanel.ResumeLayout(false);
            this.mainPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel titlePanel;
        private System.Windows.Forms.TableLayoutPanel mainPanel;
        private UserControls.UserDataGrid dgvOutpuFields;
        private UserControls.UserDataGrid dgvInputFields;
        private UCBtnExt ucBtnExt1;
        private UserControls.LabelCombox cmbProductList;
        private UserControls.LabelCombox cmbLinList;
        private System.Windows.Forms.FlowLayoutPanel listMethods;

    }
}