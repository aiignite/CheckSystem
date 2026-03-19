namespace CheckSystem.RobotForms
{
    partial class RobotFormPalletManagement
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
            this.tableMain = new System.Windows.Forms.TableLayoutPanel();
            this.palletDgv = new UserControls.UserDataGrid();
            this.lblCmb = new UserControls.LabelCombox();
            this.tableBtns = new System.Windows.Forms.TableLayoutPanel();
            this.btnDelete = new HZH_Controls.Controls.Btn.UCBtnExt();
            this.btnUpdate = new HZH_Controls.Controls.Btn.UCBtnExt();
            this.btnInsert = new HZH_Controls.Controls.Btn.UCBtnExt();
            this.btnAdd = new HZH_Controls.Controls.Btn.UCBtnExt();
            this.btnSave = new HZH_Controls.Controls.Btn.UCBtnExt();
            this.btnOk = new HZH_Controls.Controls.Btn.UCBtnExt();
            this.lblCode = new UserControls.LabelText();
            this.tableMain.SuspendLayout();
            this.tableBtns.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableMain
            // 
            this.tableMain.ColumnCount = 1;
            this.tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableMain.Controls.Add(this.palletDgv, 0, 1);
            this.tableMain.Controls.Add(this.lblCmb, 0, 0);
            this.tableMain.Controls.Add(this.tableBtns, 0, 2);
            this.tableMain.Controls.Add(this.lblCode, 0, 3);
            this.tableMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableMain.Location = new System.Drawing.Point(0, 0);
            this.tableMain.Name = "tableMain";
            this.tableMain.RowCount = 4;
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableMain.Size = new System.Drawing.Size(784, 561);
            this.tableMain.TabIndex = 0;
            // 
            // palletDgv
            // 
            this.palletDgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.palletDgv.Location = new System.Drawing.Point(3, 53);
            this.palletDgv.Name = "palletDgv";
            this.palletDgv.Size = new System.Drawing.Size(778, 380);
            this.palletDgv.TabIndex = 1;
            // 
            // lblCmb
            // 
            this.lblCmb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCmb.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCmb.LabelString = "label";
            this.lblCmb.Location = new System.Drawing.Point(0, 0);
            this.lblCmb.Margin = new System.Windows.Forms.Padding(0);
            this.lblCmb.Name = "lblCmb";
            this.lblCmb.Size = new System.Drawing.Size(784, 50);
            this.lblCmb.TabIndex = 2;
            // 
            // tableBtns
            // 
            this.tableBtns.ColumnCount = 6;
            this.tableBtns.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableBtns.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableBtns.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableBtns.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableBtns.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableBtns.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 104F));
            this.tableBtns.Controls.Add(this.btnDelete, 3, 0);
            this.tableBtns.Controls.Add(this.btnUpdate, 2, 0);
            this.tableBtns.Controls.Add(this.btnInsert, 1, 0);
            this.tableBtns.Controls.Add(this.btnAdd, 0, 0);
            this.tableBtns.Controls.Add(this.btnSave, 4, 0);
            this.tableBtns.Controls.Add(this.btnOk, 5, 0);
            this.tableBtns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableBtns.Location = new System.Drawing.Point(0, 436);
            this.tableBtns.Margin = new System.Windows.Forms.Padding(0);
            this.tableBtns.Name = "tableBtns";
            this.tableBtns.RowCount = 1;
            this.tableBtns.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableBtns.Size = new System.Drawing.Size(784, 75);
            this.tableBtns.TabIndex = 3;
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.White;
            this.btnDelete.BtnBackColor = System.Drawing.Color.White;
            this.btnDelete.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDelete.BtnForeColor = System.Drawing.Color.White;
            this.btnDelete.BtnText = "删除当前行";
            this.btnDelete.ConerRadius = 5;
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDelete.EnabledMouseEffect = false;
            this.btnDelete.FillColor = System.Drawing.Color.DarkCyan;
            this.btnDelete.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnDelete.IsRadius = true;
            this.btnDelete.IsShowRect = true;
            this.btnDelete.IsShowTips = false;
            this.btnDelete.Location = new System.Drawing.Point(408, 0);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.RectColor = System.Drawing.Color.DarkOliveGreen;
            this.btnDelete.RectWidth = 1;
            this.btnDelete.Size = new System.Drawing.Size(136, 75);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.TabStop = false;
            this.btnDelete.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.btnDelete.TipsText = "";
            this.btnDelete.BtnClick += new System.EventHandler(this.btnDelete_BtnClick);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.White;
            this.btnUpdate.BtnBackColor = System.Drawing.Color.White;
            this.btnUpdate.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUpdate.BtnForeColor = System.Drawing.Color.White;
            this.btnUpdate.BtnText = "更新当前行";
            this.btnUpdate.ConerRadius = 5;
            this.btnUpdate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnUpdate.EnabledMouseEffect = false;
            this.btnUpdate.FillColor = System.Drawing.Color.DarkCyan;
            this.btnUpdate.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnUpdate.IsRadius = true;
            this.btnUpdate.IsShowRect = true;
            this.btnUpdate.IsShowTips = false;
            this.btnUpdate.Location = new System.Drawing.Point(272, 0);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(0);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.RectColor = System.Drawing.Color.DarkOliveGreen;
            this.btnUpdate.RectWidth = 1;
            this.btnUpdate.Size = new System.Drawing.Size(136, 75);
            this.btnUpdate.TabIndex = 2;
            this.btnUpdate.TabStop = false;
            this.btnUpdate.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.btnUpdate.TipsText = "";
            this.btnUpdate.BtnClick += new System.EventHandler(this.btnUpdate_BtnClick);
            // 
            // btnInsert
            // 
            this.btnInsert.BackColor = System.Drawing.Color.White;
            this.btnInsert.BtnBackColor = System.Drawing.Color.White;
            this.btnInsert.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnInsert.BtnForeColor = System.Drawing.Color.White;
            this.btnInsert.BtnText = "插入到上一行";
            this.btnInsert.ConerRadius = 5;
            this.btnInsert.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnInsert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnInsert.EnabledMouseEffect = false;
            this.btnInsert.FillColor = System.Drawing.Color.DarkCyan;
            this.btnInsert.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnInsert.IsRadius = true;
            this.btnInsert.IsShowRect = true;
            this.btnInsert.IsShowTips = false;
            this.btnInsert.Location = new System.Drawing.Point(136, 0);
            this.btnInsert.Margin = new System.Windows.Forms.Padding(0);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.RectColor = System.Drawing.Color.DarkOliveGreen;
            this.btnInsert.RectWidth = 1;
            this.btnInsert.Size = new System.Drawing.Size(136, 75);
            this.btnInsert.TabIndex = 1;
            this.btnInsert.TabStop = false;
            this.btnInsert.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.btnInsert.TipsText = "";
            this.btnInsert.BtnClick += new System.EventHandler(this.btnInsert_BtnClick);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.White;
            this.btnAdd.BtnBackColor = System.Drawing.Color.White;
            this.btnAdd.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAdd.BtnForeColor = System.Drawing.Color.White;
            this.btnAdd.BtnText = "添加到末尾";
            this.btnAdd.ConerRadius = 5;
            this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAdd.EnabledMouseEffect = false;
            this.btnAdd.FillColor = System.Drawing.Color.DarkCyan;
            this.btnAdd.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnAdd.IsRadius = true;
            this.btnAdd.IsShowRect = true;
            this.btnAdd.IsShowTips = false;
            this.btnAdd.Location = new System.Drawing.Point(0, 0);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.RectColor = System.Drawing.Color.DarkOliveGreen;
            this.btnAdd.RectWidth = 1;
            this.btnAdd.Size = new System.Drawing.Size(136, 75);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.TabStop = false;
            this.btnAdd.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.btnAdd.TipsText = "";
            this.btnAdd.BtnClick += new System.EventHandler(this.btnAdd_BtnClick);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.White;
            this.btnSave.BtnBackColor = System.Drawing.Color.White;
            this.btnSave.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.BtnForeColor = System.Drawing.Color.White;
            this.btnSave.BtnText = "保存";
            this.btnSave.ConerRadius = 5;
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSave.EnabledMouseEffect = false;
            this.btnSave.FillColor = System.Drawing.Color.DarkCyan;
            this.btnSave.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnSave.IsRadius = true;
            this.btnSave.IsShowRect = true;
            this.btnSave.IsShowTips = false;
            this.btnSave.Location = new System.Drawing.Point(544, 0);
            this.btnSave.Margin = new System.Windows.Forms.Padding(0);
            this.btnSave.Name = "btnSave";
            this.btnSave.RectColor = System.Drawing.Color.DarkOliveGreen;
            this.btnSave.RectWidth = 1;
            this.btnSave.Size = new System.Drawing.Size(136, 75);
            this.btnSave.TabIndex = 4;
            this.btnSave.TabStop = false;
            this.btnSave.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.btnSave.TipsText = "";
            this.btnSave.BtnClick += new System.EventHandler(this.btnSave_BtnClick);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.White;
            this.btnOk.BtnBackColor = System.Drawing.Color.White;
            this.btnOk.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOk.BtnForeColor = System.Drawing.Color.White;
            this.btnOk.BtnText = "确认";
            this.btnOk.ConerRadius = 5;
            this.btnOk.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOk.EnabledMouseEffect = false;
            this.btnOk.FillColor = System.Drawing.Color.DarkCyan;
            this.btnOk.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnOk.IsRadius = true;
            this.btnOk.IsShowRect = true;
            this.btnOk.IsShowTips = false;
            this.btnOk.Location = new System.Drawing.Point(680, 0);
            this.btnOk.Margin = new System.Windows.Forms.Padding(0);
            this.btnOk.Name = "btnOk";
            this.btnOk.RectColor = System.Drawing.Color.DarkOliveGreen;
            this.btnOk.RectWidth = 1;
            this.btnOk.Size = new System.Drawing.Size(104, 75);
            this.btnOk.TabIndex = 5;
            this.btnOk.TabStop = false;
            this.btnOk.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.btnOk.TipsText = "";
            this.btnOk.BtnClick += new System.EventHandler(this.btnOk_BtnClick);
            // 
            // lblCode
            // 
            this.lblCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCode.Enabled = false;
            this.lblCode.Font = new System.Drawing.Font("微软雅黑", 6F);
            this.lblCode.LabelString = "CodePreview";
            this.lblCode.Location = new System.Drawing.Point(2, 513);
            this.lblCode.Margin = new System.Windows.Forms.Padding(2);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(780, 46);
            this.lblCode.TabIndex = 4;
            // 
            // RobotFormPalletManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.tableMain);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(800, 600);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "RobotFormPalletManagement";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RobotFormPalletManagement";
            this.tableMain.ResumeLayout(false);
            this.tableBtns.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableMain;
        private UserControls.UserDataGrid palletDgv;
        private UserControls.LabelCombox lblCmb;
        private System.Windows.Forms.TableLayoutPanel tableBtns;
        private HZH_Controls.Controls.Btn.UCBtnExt btnDelete;
        private HZH_Controls.Controls.Btn.UCBtnExt btnUpdate;
        private HZH_Controls.Controls.Btn.UCBtnExt btnInsert;
        private HZH_Controls.Controls.Btn.UCBtnExt btnAdd;
        private HZH_Controls.Controls.Btn.UCBtnExt btnSave;
        private UserControls.LabelText lblCode;
        private HZH_Controls.Controls.Btn.UCBtnExt btnOk;

    }
}