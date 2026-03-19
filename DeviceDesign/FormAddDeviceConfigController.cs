
using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StateMachine;

namespace DeviceDesign
{
    public partial class FormAddDeviceConfigController : Form
    {
        public FormAddDeviceConfigController()
        {
            InitializeComponent();
            OtherPropertiesInit();
        }

        private void FormAddDeviceConfigController_Load(object sender, EventArgs e)
        {
            ControlsInit();
            ToolStripInit();
        }

        private void OtherPropertiesInit()
        {
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            Size = new Size(800, 900);
        }

        #region toolstrip
        private ToolStrip toolStrip;
        private void ToolStripInit()
        {
            toolStrip = new ToolStrip();
            toolStrip.Dock = DockStyle.Top;
            toolStrip.Size = new Size(500, 40);
            toolStrip.BackColor = Color.LightBlue;

            ToolStripButton toolStripButtonEdit = new System.Windows.Forms.ToolStripButton();
            ToolStripButton toolStripButtonCancel = new ToolStripButton();
            ToolStripButton toolStripButtonSave = new ToolStripButton();
            // 
            // toolStrip
            // 
            this.toolStrip.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            toolStripButtonSave,
            toolStripButtonCancel,
            });
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";

            toolStripButtonSave.Image = Properties.Resources.Save;
            toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonSave.Name = "toolStripButtonSave";
            toolStripButtonSave.Size = new System.Drawing.Size(78, 36);
            toolStripButtonSave.Text = "保存";
            toolStripButtonSave.Click += ToolStripButtonSave_Click;


            toolStripButtonCancel.Image = Properties.Resources.delete;
            toolStripButtonCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonCancel.Name = "toolStripButtonCancel";
            toolStripButtonCancel.Size = new System.Drawing.Size(78, 36);
            toolStripButtonCancel.Text = "取消";
            toolStripButtonCancel.Click += ToolStripButtonCancel_Click;

            this.Controls.Add(this.toolStrip);
        }

        private void ToolStripButtonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ToolStripButtonSave_Click(object sender, EventArgs e)
        {
            var lst = ClassComm.DeviceConfig.Controllers.ToList();
            var item = lst.Find(t => t.Name == ltName.Text && t.Type == lcmbControllers.Text);
            if (item != null)
            {
                MessageBox.Show("已经包含该名称的控制器");
                return;
            }

            DeviceConfigController prop = new DeviceConfigController();
            prop.Name = ltName.Text;
            prop.Type = lcmbControllers.Text;
            prop.Note = ltNote.Text;
            lst.Add(prop);
            ClassComm.DeviceConfig.Controllers = lst.ToArray();
            ClassComm.SaveDeviceConfigToFile(ClassComm.DeviceConfig, ClassComm.FilePathDeviceConfig, Encoding.UTF8);

            Close();
        }
        #endregion

        #region Controls

        UserControls.LabelCombox lcmbControllers = new UserControls.LabelCombox();
        UserControls.LabelText ltName = new UserControls.LabelText();
        UserControls.LabelText ltNote = new UserControls.LabelText();
        private void ControlsInit()
        {
            lcmbControllers.Name = "控制器类型";
            lcmbControllers.label.Text = lcmbControllers.Name;
            foreach (var con in ClassComm.LstControllers)
            {
                lcmbControllers.comboBox.Items.Add(con.Name);
                lcmbControllers.ListText.Add(con.Name);
            }
            lcmbControllers.Dock = DockStyle.Top;

 
            ltName.label.Text = "名称";
            ltName.Dock = DockStyle.Top;

            ltNote.label.Text = "说明";
            ltNote.Dock = DockStyle.Top;

            Controls.Add(ltNote);
            Controls.Add(lcmbControllers);
            Controls.Add(ltName);

        }

  

        #endregion

    }
}
