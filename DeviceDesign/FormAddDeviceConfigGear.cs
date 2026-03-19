using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StateMachine;
using UserControls;

namespace DeviceDesign
{
    public partial class FormAddDeviceConfigGear : Form
    {
        public FormAddDeviceConfigGear()
        {
            InitializeComponent();
            OtherPropertiesInit();
        }

        private void FormAddDeviceConfigGear_Load(object sender, EventArgs e)
        {
            ControlsInit();
            ToolStripInit();
        }

        private void OtherPropertiesInit()
        {
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("微软雅黑", 14F, FontStyle.Regular, GraphicsUnit.Point, 134);
            Size = new Size(800, 900);
        }

        #region toolstrip
        private ToolStrip _toolStrip;
        private void ToolStripInit()
        {
            _toolStrip = new ToolStrip();
            _toolStrip.Dock = DockStyle.Top;
            _toolStrip.Size = new Size(500, 40);
            _toolStrip.BackColor = Color.LightBlue;

            var toolStripButtonEdit = new ToolStripButton();
            var toolStripButtonCancel = new ToolStripButton();
            var toolStripButtonSave = new ToolStripButton();
            // 
            // toolStrip
            // 
            _toolStrip.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            _toolStrip.ImageScalingSize = new Size(32, 32);
            _toolStrip.Items.AddRange(new ToolStripItem[] {
            toolStripButtonSave,
            toolStripButtonCancel,
            });
            _toolStrip.Location = new Point(0, 0);
            _toolStrip.Name = "toolStrip";

            toolStripButtonSave.Image = Properties.Resources.Save;
            toolStripButtonSave.ImageTransparentColor = Color.Magenta;
            toolStripButtonSave.Name = "toolStripButtonSave";
            toolStripButtonSave.Size = new Size(78, 36);
            toolStripButtonSave.Text = @"保存";
            toolStripButtonSave.Click += ToolStripButtonSave_Click;


            toolStripButtonCancel.Image = Properties.Resources.delete;
            toolStripButtonCancel.ImageTransparentColor = Color.Magenta;
            toolStripButtonCancel.Name = "toolStripButtonCancel";
            toolStripButtonCancel.Size = new Size(78, 36);
            toolStripButtonCancel.Text = @"取消";
            toolStripButtonCancel.Click += ToolStripButtonCancel_Click;

            Controls.Add(_toolStrip);
        }

        private void ToolStripButtonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ToolStripButtonSave_Click(object sender, EventArgs e)
        {
            var lst = ClassComm.DeviceConfig.Gears.ToList();
            var newGear = new DeviceConfigGear
            {
                Name = _lcmbBarcodeName.comboBox.Text,
            };

            if (!string.IsNullOrEmpty(_lcmbMatchCodeContent.textBox.Text))
            {
                newGear.MatchingCodeContent = _lcmbMatchCodeContent.textBox.Text;
                newGear.MatchingCodeIndex = _lcmbMatchCodeIndex.comboBox.Text;
            }

            if (!string.IsNullOrEmpty(_lcmbGear1Content.textBox.Text))
            {
                newGear.Gear1Content = _lcmbGear1Content.textBox.Text;
                newGear.Gear1Index = _lcmbGear1Index.comboBox.Text;
            }

            if (!string.IsNullOrEmpty(_lcmbGear2Content.textBox.Text))
            {
                newGear.Gear2Content = _lcmbGear2Content.textBox.Text;
                newGear.Gear2Index = _lcmbGear2Index.comboBox.Text;
            }

            if (!string.IsNullOrEmpty(_lcmbGear3Content.textBox.Text))
            {
                newGear.Gear3Content = _lcmbGear3Content.textBox.Text;
                newGear.Gear3Index = _lcmbGear3Index.comboBox.Text;
            }

            if (!string.IsNullOrEmpty(_lcmbGear4Content.textBox.Text))
            {
                newGear.Gear4Content = _lcmbGear4Content.textBox.Text;
                newGear.Gear4Index = _lcmbGear4Index.comboBox.Text;
            }

            lst.Add(newGear);

            ClassComm.DeviceConfig.Gears = lst.ToArray();
            ClassComm.SaveDeviceConfigToFile(ClassComm.DeviceConfig, ClassComm.FilePathDeviceConfig, Encoding.UTF8);

            Close();
        }
        #endregion

        #region Controls

        private readonly LabelCombox _lcmbBarcodeName = new LabelCombox { LabelString = @"二维码名称", Dock = DockStyle.Top };
        private readonly LabelText _lcmbMatchCodeContent = new LabelText { LabelString = @"匹配码内容", Dock = DockStyle.Top };
        private readonly LabelCombox _lcmbMatchCodeIndex = new LabelCombox { LabelString = @"匹配码位置", Dock = DockStyle.Top };
        private readonly LabelText _lcmbGear1Content = new LabelText { LabelString = @"档位1内容", Dock = DockStyle.Top };
        private readonly LabelCombox _lcmbGear1Index = new LabelCombox { LabelString = @"档位1位置", Dock = DockStyle.Top };
        private readonly LabelText _lcmbGear2Content = new LabelText { LabelString = @"档位2内容", Dock = DockStyle.Top };
        private readonly LabelCombox _lcmbGear2Index = new LabelCombox { LabelString = @"档位2位置", Dock = DockStyle.Top };
        private readonly LabelText _lcmbGear3Content = new LabelText { LabelString = @"档位3内容", Dock = DockStyle.Top };
        private readonly LabelCombox _lcmbGear3Index = new LabelCombox { LabelString = @"档位3位置", Dock = DockStyle.Top };
        private readonly LabelText _lcmbGear4Content = new LabelText { LabelString = @"档位4内容", Dock = DockStyle.Top };
        private readonly LabelCombox _lcmbGear4Index = new LabelCombox { LabelString = @"档位4位置", Dock = DockStyle.Top };

        private void ControlsInit()
        {
            if (ClassComm.DeviceConfig.Paras != null)
            {
                var deviceConfigParas =
                    ClassComm.DeviceConfig.Paras.ToList()
                        .FindAll(
                            f => string.Equals(f.DataType, "BarcodeGroup", StringComparison.CurrentCultureIgnoreCase));
                foreach (var t in deviceConfigParas)
                    _lcmbBarcodeName.comboBox.Items.Add(t.Name);
            }
            _lcmbBarcodeName.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            for (var i = 0; i < 100; i++)
            {
                _lcmbMatchCodeIndex.comboBox.Items.Add((i + 1).ToString());
                _lcmbGear1Index.comboBox.Items.Add((i + 1).ToString());
                _lcmbGear2Index.comboBox.Items.Add((i + 1).ToString());
                _lcmbGear3Index.comboBox.Items.Add((i + 1).ToString());
                _lcmbGear4Index.comboBox.Items.Add((i + 1).ToString());
            }

            _lcmbMatchCodeIndex.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            _lcmbGear1Index.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            _lcmbGear2Index.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            _lcmbGear3Index.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            _lcmbGear4Index.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            Controls.Add(_lcmbGear4Index);
            Controls.Add(_lcmbGear4Content);
            Controls.Add(_lcmbGear3Index);
            Controls.Add(_lcmbGear3Content);
            Controls.Add(_lcmbGear2Index);
            Controls.Add(_lcmbGear2Content);
            Controls.Add(_lcmbGear1Index);
            Controls.Add(_lcmbGear1Content);
            Controls.Add(_lcmbMatchCodeIndex);
            Controls.Add(_lcmbMatchCodeContent);
            Controls.Add(_lcmbBarcodeName);
        }

        #endregion
    }
}
