using System.Drawing;

namespace CheckSystem
{
    partial class FormTestUi
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.txtExposure = new Sunny.UI.UIIntegerUpDown();
            this.cmbCameraList = new Sunny.UI.UIComboBox();
            this.btn_play = new Sunny.UI.UIButton();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.pic_cam = new System.Windows.Forms.PictureBox();
            this.MainImageViewer = new NationalInstruments.Vision.WindowsForms.ImageViewer();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_cam)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 35);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(744, 654);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.txtExposure);
            this.panel1.Controls.Add(this.cmbCameraList);
            this.panel1.Controls.Add(this.btn_play);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 558);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(738, 93);
            this.panel1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(482, 11);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 53);
            this.button1.TabIndex = 10;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // txtExposure
            // 
            this.txtExposure.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtExposure.Location = new System.Drawing.Point(248, 26);
            this.txtExposure.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtExposure.Maximum = 5000000;
            this.txtExposure.Minimum = -5000000;
            this.txtExposure.MinimumSize = new System.Drawing.Size(100, 0);
            this.txtExposure.Name = "txtExposure";
            this.txtExposure.ShowText = false;
            this.txtExposure.Size = new System.Drawing.Size(167, 29);
            this.txtExposure.TabIndex = 9;
            this.txtExposure.Text = null;
            this.txtExposure.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtExposure.ValueChanged += new Sunny.UI.UIIntegerUpDown.OnValueChanged(this.txtExposure_ValueChanged);
            // 
            // cmbCameraList
            // 
            this.cmbCameraList.DataSource = null;
            this.cmbCameraList.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cmbCameraList.FillColor = System.Drawing.Color.White;
            this.cmbCameraList.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbCameraList.Location = new System.Drawing.Point(18, 11);
            this.cmbCameraList.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbCameraList.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmbCameraList.Name = "cmbCameraList";
            this.cmbCameraList.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cmbCameraList.Size = new System.Drawing.Size(198, 29);
            this.cmbCameraList.TabIndex = 3;
            this.cmbCameraList.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbCameraList.Watermark = "";
            // 
            // btn_play
            // 
            this.btn_play.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_play.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_play.Location = new System.Drawing.Point(18, 48);
            this.btn_play.MinimumSize = new System.Drawing.Size(1, 1);
            this.btn_play.Name = "btn_play";
            this.btn_play.Size = new System.Drawing.Size(198, 36);
            this.btn_play.TabIndex = 2;
            this.btn_play.Text = "打开摄像头";
            this.btn_play.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_play.Click += new System.EventHandler(this.btn_play_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.MainImageViewer, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.pic_cam, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(738, 549);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // pic_cam
            // 
            this.pic_cam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pic_cam.Location = new System.Drawing.Point(3, 3);
            this.pic_cam.Name = "pic_cam";
            this.pic_cam.Size = new System.Drawing.Size(363, 543);
            this.pic_cam.TabIndex = 1;
            this.pic_cam.TabStop = false;
            // 
            // MainImageViewer
            // 
            this.MainImageViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MainImageViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainImageViewer.Location = new System.Drawing.Point(372, 3);
            this.MainImageViewer.Name = "MainImageViewer";
            this.MainImageViewer.Size = new System.Drawing.Size(363, 543);
            this.MainImageViewer.TabIndex = 12;
            // 
            // FormTestUi
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(744, 689);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormTestUi";
            this.Text = "FormTestUi";
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 744, 689);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pic_cam)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private Sunny.UI.UIButton btn_play;
        private Sunny.UI.UIComboBox cmbCameraList;
        private Sunny.UI.UIIntegerUpDown txtExposure;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.PictureBox pic_cam;
        private System.Windows.Forms.Button button1;
        private NationalInstruments.Vision.WindowsForms.ImageViewer MainImageViewer;








    }
}