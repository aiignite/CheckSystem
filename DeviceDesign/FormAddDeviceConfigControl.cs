using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StateMachine;

namespace DeviceDesign
{
    public partial class FormAddDeviceConfigControl : Form
    {
        public FormAddDeviceConfigControl()
        {
            InitializeComponent();
            OtherPropertiesInit();
        }

        private void FormAddDeviceConfigControl_Load(object sender, EventArgs e)
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
            var lst = ClassComm.DeviceConfig.Controls.ToList();
            var item = lst.Find(t => t.Name == ltName.Text && t.Type == lcmbType.Text);
            if (item != null)
            {
                MessageBox.Show("已经包含该控制器的当前属性");
                return;
            }
            DeviceConfigControl control = new DeviceConfigControl();
            control.Name = ltName.Text;
            control.Type = lcmbType.Text;
            control.RowPosition = ltRowPosition.Text;
            control.RowSpan = ltRowSpan.Text;
            control.ColumnPosition = ltColumnPosition.Text;
            control.ColumnSpan = ltColumnSpan.Text;
            lst.Add( control);
            ClassComm.DeviceConfig.Controls = lst.ToArray();
            ClassComm.SaveDeviceConfigToFile(ClassComm.DeviceConfig, ClassComm.FilePathDeviceConfig, Encoding.UTF8);

            Close();
        }
        #endregion

        #region Controls

        UserControls.LabelCombox lcmbType = new UserControls.LabelCombox();
        UserControls.LabelText ltName = new UserControls.LabelText();
        UserControls.LabelText ltRowPosition = new UserControls.LabelText();
        UserControls.LabelText ltColumnPosition = new UserControls.LabelText();
        UserControls.LabelText ltRowSpan = new UserControls.LabelText();
        UserControls.LabelText ltColumnSpan = new UserControls.LabelText();

        private void ControlsInit()
        {
            lcmbType.Name = "控件类型";
            lcmbType.label.Text = lcmbType.Name;
            foreach (var con in ClassComm.LstUserControls)
            {
                lcmbType.comboBox.Items.Add(con);
            }
            lcmbType.ListText = ClassComm.LstUserControls;
            lcmbType.Dock = DockStyle.Top;

            ltName.label.Text = "名称";
            ltName.Dock = DockStyle.Top;

            var layout = ClassComm.DeviceConfig.FormLayout;

            ltColumnPosition.label.Text = "列开始位置";
            ltColumnPosition.Dock = DockStyle.Top;
            ltColumnPosition.TipOfTextBox = "<"+layout.ColumnCount;

            ltRowPosition.label.Text = "行开始位置";
            ltRowPosition.Dock = DockStyle.Top;
            ltRowPosition.TipOfTextBox = "<" + layout.RowCount;

            ltRowSpan.label.Text = "行数";
            ltRowSpan.Dock = DockStyle.Top;
            ltRowSpan.TipOfTextBox = "<" + layout.RowCount;

            ltColumnSpan.label.Text = "列数";
            ltColumnSpan.Dock = DockStyle.Top;
            ltColumnSpan.TipOfTextBox = "<" + layout.ColumnCount;

            Controls.Add(ltColumnSpan);
            Controls.Add(ltRowSpan);
            Controls.Add(ltColumnPosition);
            Controls.Add(ltRowPosition);
            Controls.Add(lcmbType);
            Controls.Add(ltName);
        }



        #endregion
    }
}
