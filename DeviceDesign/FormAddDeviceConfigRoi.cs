using ICSharpCode.TextEditor.Actions;
using StateMachine;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserControls;

namespace DeviceDesign
{
    public partial class FormAddDeviceConfigRoi : Form
    {
        public FormAddDeviceConfigRoi()
        {
            InitializeComponent();
            OtherPropertiesInit();
            Load += FormAddDeviceConfigVisionRoi_Load;
        }

        private void FormAddDeviceConfigVisionRoi_Load(object sender, EventArgs e)
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
            var lst = ClassComm.DeviceConfig.Rois.ToList();
            if (string.IsNullOrEmpty(_lcmbRoiName.Text) ||
                string.IsNullOrEmpty(_lcmbRoiGroup.Text) ||
                string.IsNullOrEmpty(_lcmbRoiX.Text) ||
                string.IsNullOrEmpty(_lcmbRoiY.Text) ||
                string.IsNullOrEmpty(_lcmbRoiWidth.Text) ||
                string.IsNullOrEmpty(_lcmbRoiHeight.Text) ||
                string.IsNullOrEmpty(_lcmbRoiGrayMin.Text) ||
                string.IsNullOrEmpty(_lcmbRoiGrayMax.Text))
            {
                MessageBox.Show("有参数为空");
                return;
            }

            uint group;
            if (!uint.TryParse(_lcmbRoiGroup.Text, out group))
            {
                MessageBox.Show("Group参数错误");
                return;
            }

            uint x;
            uint y;
            uint width;
            uint height;
            uint grayMin;
            uint grayMax;
            if (!uint.TryParse(_lcmbRoiX.Text, out x))
            {
                MessageBox.Show("X参数错误");
                return;
            }
            if (!uint.TryParse(_lcmbRoiY.Text, out y))
            {
                MessageBox.Show("Y参数错误");
                return;
            }
            if (!uint.TryParse(_lcmbRoiWidth.Text, out width))
            {
                MessageBox.Show("Width参数错误");
                return;
            }
            if (!uint.TryParse(_lcmbRoiHeight.Text, out height))
            {
                MessageBox.Show("Height参数错误");
                return;
            }
            if (!uint.TryParse(_lcmbRoiGrayMin.Text, out grayMin))
            {
                MessageBox.Show("GrayMin参数错误");
                return;
            }
            if (!uint.TryParse(_lcmbRoiGrayMax.Text, out grayMax))
            {
                MessageBox.Show("GrayMax参数错误");
                return;
            }

            if (grayMin < 0 || grayMin > 255 || grayMin > grayMax || grayMax > 255 || grayMax < 0)
            {
                MessageBox.Show("GrayMin/Gray参数错误");
                return;
            }

            var newRoi = new DeviceConfigRoi
            {
                Name = _lcmbRoiName.Text,
                Group = group.ToString(),
                RectX = x.ToString(),
                RectY = y.ToString(),
                RectWidth = width.ToString(),
                RectHeight = height.ToString(),
                Min = grayMin.ToString(),
                Max = grayMax.ToString()
            };

            lst.Add(newRoi);

            ClassComm.DeviceConfig.Rois = lst.ToArray();
            ClassComm.SaveDeviceConfigToFile(ClassComm.DeviceConfig, ClassComm.FilePathDeviceConfig, Encoding.UTF8);

            Close();
        }
        #endregion

        #region Controls
        private readonly LabelText _lcmbRoiName = new LabelText { LabelString = @"ROI名称", Dock = DockStyle.Top };
        private readonly LabelText _lcmbRoiGroup = new LabelText { LabelString = @"ROI组号", Dock = DockStyle.Top };
        private readonly LabelText _lcmbRoiX = new LabelText { LabelString = @"ROI位置X", Dock = DockStyle.Top };
        private readonly LabelText _lcmbRoiY = new LabelText { LabelString = @"ROI位置Y", Dock = DockStyle.Top };
        private readonly LabelText _lcmbRoiWidth = new LabelText { LabelString = @"ROI尺寸Width", Dock = DockStyle.Top };
        private readonly LabelText _lcmbRoiHeight = new LabelText { LabelString = @"ROI尺寸Height", Dock = DockStyle.Top };
        private readonly LabelText _lcmbRoiGrayMin = new LabelText { LabelString = @"ROI灰度Min", Dock = DockStyle.Top };
        private readonly LabelText _lcmbRoiGrayMax = new LabelText { LabelString = @"ROI灰度Max", Dock = DockStyle.Top };

        private void ControlsInit()
        {
            Controls.Add(_lcmbRoiGrayMax);
            Controls.Add(_lcmbRoiGrayMin);

            Controls.Add(_lcmbRoiHeight);
            Controls.Add(_lcmbRoiWidth);
            Controls.Add(_lcmbRoiY);
            Controls.Add(_lcmbRoiX);

            Controls.Add(_lcmbRoiGroup);
            Controls.Add(_lcmbRoiName);
        }
        #endregion
    }
}
