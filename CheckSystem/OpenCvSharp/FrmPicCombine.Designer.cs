namespace CheckSystem.OpenCvSharp
{
    partial class FrmPicCombine
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSelectImg1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtImg1 = new System.Windows.Forms.TextBox();
            this.txtImg2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSelectImg2 = new System.Windows.Forms.Button();
            this.pbImg1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pbImg2 = new System.Windows.Forms.PictureBox();
            this.pbResult = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbImg1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbImg2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbResult)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSelectImg1
            // 
            this.btnSelectImg1.Location = new System.Drawing.Point(905, 39);
            this.btnSelectImg1.Name = "btnSelectImg1";
            this.btnSelectImg1.Size = new System.Drawing.Size(75, 23);
            this.btnSelectImg1.TabIndex = 0;
            this.btnSelectImg1.Text = "选择";
            this.btnSelectImg1.UseVisualStyleBackColor = true;
            this.btnSelectImg1.Click += new System.EventHandler(this.btnSelectImg1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "图片1：";
            // 
            // txtImg1
            // 
            this.txtImg1.Location = new System.Drawing.Point(66, 42);
            this.txtImg1.Name = "txtImg1";
            this.txtImg1.ReadOnly = true;
            this.txtImg1.Size = new System.Drawing.Size(833, 21);
            this.txtImg1.TabIndex = 2;
            // 
            // txtImg2
            // 
            this.txtImg2.Location = new System.Drawing.Point(66, 67);
            this.txtImg2.Name = "txtImg2";
            this.txtImg2.ReadOnly = true;
            this.txtImg2.Size = new System.Drawing.Size(833, 21);
            this.txtImg2.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "图片2：";
            // 
            // btnSelectImg2
            // 
            this.btnSelectImg2.Location = new System.Drawing.Point(905, 66);
            this.btnSelectImg2.Name = "btnSelectImg2";
            this.btnSelectImg2.Size = new System.Drawing.Size(75, 23);
            this.btnSelectImg2.TabIndex = 3;
            this.btnSelectImg2.Text = "选择";
            this.btnSelectImg2.UseVisualStyleBackColor = true;
            this.btnSelectImg2.Click += new System.EventHandler(this.btnSelectImg2_Click);
            // 
            // pbImg1
            // 
            this.pbImg1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pbImg1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbImg1.Location = new System.Drawing.Point(12, 96);
            this.pbImg1.Name = "pbImg1";
            this.pbImg1.Size = new System.Drawing.Size(292, 264);
            this.pbImg1.TabIndex = 6;
            this.pbImg1.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(131, 368);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "图片1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(131, 708);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "图片1";
            // 
            // pbImg2
            // 
            this.pbImg2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pbImg2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbImg2.Location = new System.Drawing.Point(12, 441);
            this.pbImg2.Name = "pbImg2";
            this.pbImg2.Size = new System.Drawing.Size(292, 264);
            this.pbImg2.TabIndex = 8;
            this.pbImg2.TabStop = false;
            // 
            // pbResult
            // 
            this.pbResult.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pbResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbResult.Location = new System.Drawing.Point(335, 96);
            this.pbResult.Name = "pbResult";
            this.pbResult.Size = new System.Drawing.Size(645, 630);
            this.pbResult.TabIndex = 10;
            this.pbResult.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 13;
            this.label6.Text = "拼接方式：";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(172, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(86, 25);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "保存结果";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(78, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(88, 26);
            this.button1.TabIndex = 16;
            this.button1.Text = "开始拼接";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(274, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(88, 26);
            this.button2.TabIndex = 16;
            this.button2.Text = "棋盘格测试";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // FrmPicCombine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(992, 750);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.pbResult);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.pbImg2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pbImg1);
            this.Controls.Add(this.txtImg2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSelectImg2);
            this.Controls.Add(this.txtImg1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSelectImg1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmPicCombine";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "图片拼接";
            ((System.ComponentModel.ISupportInitialize)(this.pbImg1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbImg2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbResult)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelectImg1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtImg1;
        private System.Windows.Forms.TextBox txtImg2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSelectImg2;
        private System.Windows.Forms.PictureBox pbImg1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pbImg2;
        private System.Windows.Forms.PictureBox pbResult;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}