using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StateMachine;

namespace DeviceDesign
{
    public partial class FormEditDeviceConfigProperty : Form
    {

        private DeviceConfigProperty _property =new DeviceConfigProperty();
        public FormEditDeviceConfigProperty()
        {
            InitializeComponent();
            OtherPropertiesInit();
        }

        public FormEditDeviceConfigProperty(object obj)
        {
            InitializeComponent();
            OtherPropertiesInit();
            _property = (DeviceConfigProperty)obj;
        }

        private void FormEditDeviceConfigProperty_Load(object sender, EventArgs e)
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
            var lst = ClassComm.DeviceConfig.ControllerProperties.ToList();
            var prop = lst.Find(t => t.Name == _property.Name &&t.ControllerName==_property.ControllerName);
            var index = lst.IndexOf(prop);
            if(prop != null)
            {
                lst.Remove(prop);
            }
            else
            {
                Close();
            }
            DeviceConfigProperty property = new DeviceConfigProperty();
            property.ControllerName = lcmbController.Text;
            property.Name = lcmbProperty.Text;
            property.Value = ltValue.Text;
            property.Note = ltNote.Text;
            lst.Insert(index, property);
            ClassComm.DeviceConfig.ControllerProperties = lst.ToArray();
            ClassComm.SaveDeviceConfigToFile(ClassComm.DeviceConfig, ClassComm.FilePathDeviceConfig, Encoding.UTF8);

            Close();
        }
        #endregion

        #region Controls

        UserControls.LabelCombox lcmbController = new UserControls.LabelCombox();
        UserControls.LabelCombox lcmbProperty = new UserControls.LabelCombox();
        UserControls.LabelText ltValue = new UserControls.LabelText();
        UserControls.LabelText ltNote = new UserControls.LabelText();
        private void ControlsInit()
        {
            lcmbController.Name = "控制器名称";
            lcmbController.label.Text = lcmbController.Name;
            foreach (var con in ClassComm.LstControllers)
            {
                lcmbController.comboBox.Items.Add(con.Name);
            }
            lcmbController.ListText =ClassComm.LstControllers.Select(t => t.Name).ToList();
            lcmbController.comboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
            lcmbController.Dock = DockStyle.Top;
            lcmbController.Text = _property.ControllerName;

            lcmbProperty.Name = "属性";
            lcmbProperty.label.Text = lcmbProperty.Name;
            lcmbProperty.Dock = DockStyle.Top;
            lcmbProperty.Text = _property.Name;

            ltValue.label.Text = "值";
            ltValue.Dock = DockStyle.Top;
            ltValue.Text = _property.Value;

            ltNote.label.Text = "说明";
            ltNote.Dock = DockStyle.Top;
            ltNote.Text = _property.Note;


            Controls.Add(ltNote);
            Controls.Add(ltValue);
            Controls.Add(lcmbProperty);
            Controls.Add(lcmbController);
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            lcmbProperty.comboBox.Items.Clear();
            var controller = ClassComm.LstControllers.Find(t => t.Name == lcmbController.Text);
            if (controller == null) return;

            lcmbProperty.comboBox.Items.AddRange(controller.LstProperties.ToArray());
            lcmbProperty.ListText = controller.LstProperties;
        }


        #endregion
    }
}
