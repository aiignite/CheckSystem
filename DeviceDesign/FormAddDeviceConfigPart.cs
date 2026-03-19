using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StateMachine;

namespace DeviceDesign
{
    public partial class FormAddDeviceConfigPart : Form
    {
        public FormAddDeviceConfigPart()
        {
            InitializeComponent();
            OtherPropertiesInit();
        }

        private void FormAddDeviceConfigPart_Load(object sender, EventArgs e)
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
            var lst = ClassComm.DeviceConfig.Parts.ToList();
            var item = lst.Find(t => t.Name == ltName.Text && t.ProcessNo == lcmbProcessNo.Text);
            if (item != null)
            {
                MessageBox.Show("已经包含该控制器的当前属性");
                return;
            }
            DeviceConfigPart part = new DeviceConfigPart();
            part.Name = ltName.Text;
            part.ControllerField = lcmbControllerField.Text;
            part.ControllerName = lcmbControllerName.Text;
            part.DataType = lcmbDataType.Text;
            part.ProcessNo = lcmbProcessNo.Text;
             lst.Add(part);
            ClassComm.DeviceConfig.Parts = lst.ToArray();
            ClassComm.SaveDeviceConfigToFile(ClassComm.DeviceConfig, ClassComm.FilePathDeviceConfig, Encoding.UTF8);

            Close();
        }
        #endregion

        #region Controls

        UserControls.LabelCombox lcmbProcessNo = new UserControls.LabelCombox();
        UserControls.LabelCombox lcmbDataType = new UserControls.LabelCombox();
        UserControls.LabelCombox lcmbControllerName = new UserControls.LabelCombox();
        UserControls.LabelCombox lcmbControllerField = new UserControls.LabelCombox();


        UserControls.LabelText ltName = new UserControls.LabelText() { LabelString = "名称" };


        private void ControlsInit()
        {
            lcmbProcessNo.Name = "工序编号";
            lcmbProcessNo.label.Text = lcmbProcessNo.Name;
            lcmbProcessNo.Dock = DockStyle.Top;
            var processes = ClassComm.DeviceConfig.Processes;
            lcmbProcessNo.comboBox.Items.AddRange(processes.Select(t => t.No).ToArray());
            lcmbProcessNo.ListText = processes.Select(t => t.No).ToList();


            lcmbDataType.Name = "数据类型";
            lcmbDataType.label.Text = lcmbDataType.Name;
            var lstDataTypes = ClassComm.LstDataTypes;
            lcmbDataType.comboBox.Items.AddRange(lstDataTypes.ToArray());
            lcmbDataType.ListText = lstDataTypes;
            lcmbDataType.Dock = DockStyle.Top;
 

            lcmbControllerName.Name = "控制器名称";
            lcmbControllerName.label.Text = lcmbControllerName.Name;
            lcmbControllerName.Dock = DockStyle.Top;
            var controllers = ClassComm.DeviceConfig.Controllers;
            lcmbControllerName.comboBox.Items.AddRange(controllers.Select(t => t.Name).ToArray());
            lcmbControllerName.ListText = controllers.Select(t => t.Name).ToList();
            lcmbControllerName.comboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;

            lcmbControllerField.Name = "对应控制器变量";
            lcmbControllerField.label.Text = lcmbControllerField.Name;
            lcmbControllerField.Dock = DockStyle.Top;
            foreach (var controller in controllers)
            {
                var fields = ClassComm.LstControllers.Find(t => t.Name == controller.Type).LstFields;
                lcmbControllerField.comboBox.Items.AddRange(fields.Select(t => controller.Name + ".Field." + t).ToArray());
                lcmbControllerField.ListText.AddRange(fields.Select(t => controller.Name + ".Field." + t));
            }

            Controls.Add(lcmbControllerField);
            Controls.Add(lcmbControllerName);
            Controls.Add(lcmbDataType);
            Controls.Add(lcmbProcessNo);
            Controls.Add(ltName);

        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            lcmbControllerField.comboBox.Items.Clear();
            var controller = ClassComm.DeviceConfig.Controllers.ToList().Find(t => t.Name == lcmbControllerName.Text);
            var fields = ClassComm.LstControllers.Find(t => t.Name == controller.Type).LstFields;
            lcmbControllerField.comboBox.Items.AddRange(fields.Select(t => lcmbControllerName.Text + ".Field." + t).ToArray());
            lcmbControllerField.ListText.AddRange(fields.Select(t => lcmbControllerName.Text + ".Field." + t));
            lcmbControllerField.comboBox.DroppedDown = true;
            lcmbControllerField.comboBox.Text = "";
        }
        #endregion
    }
}
