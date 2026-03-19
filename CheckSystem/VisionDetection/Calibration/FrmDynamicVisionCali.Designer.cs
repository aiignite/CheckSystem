namespace CheckSystem.VisionDetection.Calibration
{
    partial class FrmDynamicVisionCali
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
            this.ThresholdTxtMin = new Sunny.UI.UITextBox();
            this.uiMarkLabel1 = new Sunny.UI.UIMarkLabel();
            this.ContourAreaTxtMin = new Sunny.UI.UITextBox();
            this.uiMarkLabel2 = new Sunny.UI.UIMarkLabel();
            this.uiMarkLabel4 = new Sunny.UI.UIMarkLabel();
            this.uiSymbolButton1 = new Sunny.UI.UISymbolButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.uiButton1 = new Sunny.UI.UIButton();
            this.ThresholdTxtMax = new Sunny.UI.UITextBox();
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.uiLabel2 = new Sunny.UI.UILabel();
            this.ContourAreaTxtMax = new Sunny.UI.UITextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // ThresholdTxtMin
            // 
            this.ThresholdTxtMin.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ThresholdTxtMin.DoubleValue = 200D;
            this.ThresholdTxtMin.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ThresholdTxtMin.IntValue = 200;
            this.ThresholdTxtMin.Location = new System.Drawing.Point(162, 160);
            this.ThresholdTxtMin.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ThresholdTxtMin.Maximum = 255D;
            this.ThresholdTxtMin.Minimum = 0D;
            this.ThresholdTxtMin.MinimumSize = new System.Drawing.Size(1, 16);
            this.ThresholdTxtMin.Name = "ThresholdTxtMin";
            this.ThresholdTxtMin.Padding = new System.Windows.Forms.Padding(5);
            this.ThresholdTxtMin.ShowText = false;
            this.ThresholdTxtMin.Size = new System.Drawing.Size(66, 29);
            this.ThresholdTxtMin.TabIndex = 0;
            this.ThresholdTxtMin.Text = "200";
            this.ThresholdTxtMin.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.ThresholdTxtMin.Type = Sunny.UI.UITextBox.UIEditType.Integer;
            this.ThresholdTxtMin.Watermark = "";
            // 
            // uiMarkLabel1
            // 
            this.uiMarkLabel1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiMarkLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel1.Location = new System.Drawing.Point(28, 166);
            this.uiMarkLabel1.Name = "uiMarkLabel1";
            this.uiMarkLabel1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel1.Size = new System.Drawing.Size(100, 23);
            this.uiMarkLabel1.TabIndex = 1;
            this.uiMarkLabel1.Text = "Threshold:";
            this.uiMarkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ContourAreaTxtMin
            // 
            this.ContourAreaTxtMin.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ContourAreaTxtMin.DoubleValue = 50D;
            this.ContourAreaTxtMin.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ContourAreaTxtMin.IntValue = 50;
            this.ContourAreaTxtMin.Location = new System.Drawing.Point(162, 219);
            this.ContourAreaTxtMin.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ContourAreaTxtMin.Maximum = 800000D;
            this.ContourAreaTxtMin.Minimum = 0D;
            this.ContourAreaTxtMin.MinimumSize = new System.Drawing.Size(1, 16);
            this.ContourAreaTxtMin.Name = "ContourAreaTxtMin";
            this.ContourAreaTxtMin.Padding = new System.Windows.Forms.Padding(5);
            this.ContourAreaTxtMin.ShowText = false;
            this.ContourAreaTxtMin.Size = new System.Drawing.Size(66, 29);
            this.ContourAreaTxtMin.TabIndex = 0;
            this.ContourAreaTxtMin.Text = "50";
            this.ContourAreaTxtMin.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.ContourAreaTxtMin.Type = Sunny.UI.UITextBox.UIEditType.Integer;
            this.ContourAreaTxtMin.Watermark = "";
            // 
            // uiMarkLabel2
            // 
            this.uiMarkLabel2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiMarkLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel2.Location = new System.Drawing.Point(28, 225);
            this.uiMarkLabel2.Name = "uiMarkLabel2";
            this.uiMarkLabel2.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel2.Size = new System.Drawing.Size(121, 23);
            this.uiMarkLabel2.TabIndex = 1;
            this.uiMarkLabel2.Text = "Contour area:";
            this.uiMarkLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiMarkLabel4
            // 
            this.uiMarkLabel4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiMarkLabel4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiMarkLabel4.Location = new System.Drawing.Point(28, 109);
            this.uiMarkLabel4.Name = "uiMarkLabel4";
            this.uiMarkLabel4.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.uiMarkLabel4.Size = new System.Drawing.Size(100, 23);
            this.uiMarkLabel4.TabIndex = 1;
            this.uiMarkLabel4.Text = "Open file:";
            this.uiMarkLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiSymbolButton1
            // 
            this.uiSymbolButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiSymbolButton1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiSymbolButton1.Location = new System.Drawing.Point(162, 97);
            this.uiSymbolButton1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiSymbolButton1.Name = "uiSymbolButton1";
            this.uiSymbolButton1.Size = new System.Drawing.Size(156, 35);
            this.uiSymbolButton1.TabIndex = 2;
            this.uiSymbolButton1.Text = "uiSymbolButton1";
            this.uiSymbolButton1.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiSymbolButton1.Click += new System.EventHandler(this.uiSymbolButton1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(485, 38);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(725, 352);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(485, 406);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(725, 369);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 3;
            this.pictureBox2.TabStop = false;
            // 
            // uiButton1
            // 
            this.uiButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton1.Location = new System.Drawing.Point(98, 319);
            this.uiButton1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton1.Name = "uiButton1";
            this.uiButton1.Size = new System.Drawing.Size(100, 35);
            this.uiButton1.TabIndex = 4;
            this.uiButton1.Text = "uiButton1";
            this.uiButton1.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton1.Click += new System.EventHandler(this.uiButton1_Click);
            // 
            // ThresholdTxtMax
            // 
            this.ThresholdTxtMax.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ThresholdTxtMax.DoubleValue = 255D;
            this.ThresholdTxtMax.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ThresholdTxtMax.IntValue = 255;
            this.ThresholdTxtMax.Location = new System.Drawing.Point(305, 160);
            this.ThresholdTxtMax.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ThresholdTxtMax.Maximum = 255D;
            this.ThresholdTxtMax.Minimum = 0D;
            this.ThresholdTxtMax.MinimumSize = new System.Drawing.Size(1, 16);
            this.ThresholdTxtMax.Name = "ThresholdTxtMax";
            this.ThresholdTxtMax.Padding = new System.Windows.Forms.Padding(5);
            this.ThresholdTxtMax.ShowText = false;
            this.ThresholdTxtMax.Size = new System.Drawing.Size(66, 29);
            this.ThresholdTxtMax.TabIndex = 0;
            this.ThresholdTxtMax.Text = "255";
            this.ThresholdTxtMax.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.ThresholdTxtMax.Type = Sunny.UI.UITextBox.UIEditType.Integer;
            this.ThresholdTxtMax.Watermark = "";
            // 
            // uiLabel1
            // 
            this.uiLabel1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel1.Location = new System.Drawing.Point(241, 164);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(49, 23);
            this.uiLabel1.TabIndex = 5;
            this.uiLabel1.Text = "到";
            this.uiLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel2
            // 
            this.uiLabel2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel2.Location = new System.Drawing.Point(241, 219);
            this.uiLabel2.Name = "uiLabel2";
            this.uiLabel2.Size = new System.Drawing.Size(49, 23);
            this.uiLabel2.TabIndex = 5;
            this.uiLabel2.Text = "到";
            this.uiLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ContourAreaTxtMax
            // 
            this.ContourAreaTxtMax.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ContourAreaTxtMax.DoubleValue = 1000D;
            this.ContourAreaTxtMax.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ContourAreaTxtMax.IntValue = 1000;
            this.ContourAreaTxtMax.Location = new System.Drawing.Point(305, 219);
            this.ContourAreaTxtMax.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ContourAreaTxtMax.Maximum = 100000D;
            this.ContourAreaTxtMax.Minimum = 0D;
            this.ContourAreaTxtMax.MinimumSize = new System.Drawing.Size(1, 16);
            this.ContourAreaTxtMax.Name = "ContourAreaTxtMax";
            this.ContourAreaTxtMax.Padding = new System.Windows.Forms.Padding(5);
            this.ContourAreaTxtMax.ShowText = false;
            this.ContourAreaTxtMax.Size = new System.Drawing.Size(66, 29);
            this.ContourAreaTxtMax.TabIndex = 0;
            this.ContourAreaTxtMax.Text = "1000";
            this.ContourAreaTxtMax.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.ContourAreaTxtMax.Type = Sunny.UI.UITextBox.UIEditType.Integer;
            this.ContourAreaTxtMax.Watermark = "";
            // 
            // FrmDynamicVisionCali
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1229, 791);
            this.Controls.Add(this.uiLabel2);
            this.Controls.Add(this.uiLabel1);
            this.Controls.Add(this.uiButton1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.uiSymbolButton1);
            this.Controls.Add(this.uiMarkLabel2);
            this.Controls.Add(this.ContourAreaTxtMin);
            this.Controls.Add(this.uiMarkLabel4);
            this.Controls.Add(this.uiMarkLabel1);
            this.Controls.Add(this.ContourAreaTxtMax);
            this.Controls.Add(this.ThresholdTxtMax);
            this.Controls.Add(this.ThresholdTxtMin);
            this.Name = "FrmDynamicVisionCali";
            this.Text = "FrmDynamicVisionCali";
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 800, 450);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UITextBox ThresholdTxtMin;
        private Sunny.UI.UIMarkLabel uiMarkLabel1;
        private Sunny.UI.UITextBox ContourAreaTxtMin;
        private Sunny.UI.UIMarkLabel uiMarkLabel2;
        private Sunny.UI.UIMarkLabel uiMarkLabel4;
        private Sunny.UI.UISymbolButton uiSymbolButton1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private Sunny.UI.UIButton uiButton1;
        private Sunny.UI.UITextBox ThresholdTxtMax;
        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UILabel uiLabel2;
        private Sunny.UI.UITextBox ContourAreaTxtMax;
    }
}