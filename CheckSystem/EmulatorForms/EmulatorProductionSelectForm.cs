using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CommonUtility;
using CommonUtility.FileOperator;
using HZH_Controls.IconFont;
using Sunny.UI;
using FontImages = HZH_Controls.IconFont.FontImages;

namespace CheckSystem.EmulatorForms
{
    public partial class EmulatorProductionSelectForm : UIForm
    {
        private readonly List<string> _list = new List<string>();

        public EmulatorProductionSelectForm()
        {
            InitializeComponent();
            Icon = FontImages.GetIcon(FontIcons.A_fa_lightbulb_o, 32,
                Color.DodgerBlue);
            Text = @"请选择";

            var folder = string.Format(@"{0}\EmulatorConfig", Program.SysDir);

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            foreach (
                var f in Directory.GetFiles(folder).Where(f => f.EndsWith(@".xml")))
            {
                try
                {
                    var emulatorConfig = XmlHelper.Deserialize<EmulatorConfig>(f);
                    ucmbIpAddrList.comboBox.Items.Add(emulatorConfig.ShowTitle);
                    _list.Add(f);
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            ucmbIpAddrList.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            if (ucmbIpAddrList.comboBox.Items.Count > 0)
                ucmbIpAddrList.comboBox.SelectedIndex = 0;
        }

        public sealed override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private void btnConnect_BtnClick(object sender, EventArgs e)
        {
            if (ucmbIpAddrList.comboBox.Text == null || ucmbIpAddrList.comboBox.Items.Count <= 0 ||
                ucmbIpAddrList.comboBox.SelectedIndex == -1) return;
            var form = new EmulatorMainForm(_list[ucmbIpAddrList.comboBox.SelectedIndex]);
            form.ShowDialog();
            DialogResult = DialogResult.OK;
        }
    }
}
