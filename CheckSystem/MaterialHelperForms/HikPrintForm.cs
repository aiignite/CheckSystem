using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HZH_Controls.IconFont;
using UserControls;

namespace CheckSystem.MaterialHelperForms
{
    public partial class HikPrintForm : Form
    {
        public HikPrintForm()
        {
            InitializeComponent();
            Icon = FontImages.GetIcon(
                FontIcons.E_icon_document, 32, Color.DodgerBlue);
            Text = @"打印参数调试";
            LoadUi();
        }

        public sealed override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private void LoadUi()
        {
            var ints = new List<object>();
            for (var i = 0; i < 600; i++)
                ints.Add(i);
            foreach (
                var cmb in
                    (from object c in Controls where c.GetType() == typeof(LabelCombox) select c).OfType<LabelCombox>()
                )
            {
                cmb.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                cmb.comboBox.Items.AddRange(ints.ToArray());
            }

            cmbPnX.comboBox.SelectedIndex = GetPoint("PN").X;
            cmbPnY.comboBox.SelectedIndex = GetPoint("PN").Y;

            cmbDclX.comboBox.SelectedIndex = GetPoint("DCL").X;
            cmbDclY.comboBox.SelectedIndex = GetPoint("DCL").Y;

            cmbBinX.comboBox.SelectedIndex = GetPoint("BIN").X;
            cmbBinY.comboBox.SelectedIndex = GetPoint("BIN").Y;

            cmbPnoX.comboBox.SelectedIndex = GetPoint("PNO").X;
            cmbPnoY.comboBox.SelectedIndex = GetPoint("PNO").Y;

            cmbSupX.comboBox.SelectedIndex = GetPoint("SUP").X;
            cmbSupY.comboBox.SelectedIndex = GetPoint("SUP").Y;

            cmbQtyX.comboBox.SelectedIndex = GetPoint("QTY").X;
            cmbQtyY.comboBox.SelectedIndex = GetPoint("QTY").Y;

            cmbDateX.comboBox.SelectedIndex = GetPoint("DATE").X;
            cmbDateY.comboBox.SelectedIndex = GetPoint("DATE").Y;

            cmbNoX.comboBox.SelectedIndex = GetPoint("NO").X;
            cmbNoY.comboBox.SelectedIndex = GetPoint("NO").Y;

            cmbQuaX.comboBox.SelectedIndex = GetPoint("Qualevel").X;
            cmbQuaY.comboBox.SelectedIndex = GetPoint("Qualevel").Y;

            cmbQrcdoeX.comboBox.SelectedIndex = GetPoint("QRCODE").X;
            cmbQrcdoeY.comboBox.SelectedIndex = GetPoint("QRCODE").Y;
        }

        public Point GetPoint(string key)
        {
            var read = HikSetup.Setup.IniReadValue("PrintParas", key).Split(',');
            return new Point(int.Parse(read[0]), int.Parse(read[1]));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 试打
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            HikSetup.PrintLabel(
                string.Format("P/N:{0}", "XXXXXXXXXXXXXXX"),
                string.Format("L/N:{0}", "XXXXXXXXXXXXXXXXXX"),
                string.Format("BIN:{0}", "XXXXXXXXXXXXXXX"),
                string.Format("PNO:{0}", "XXXXXXXXXXXX"),
                string.Format("SUP:{0}", "XXXXXXXX"),
                string.Format("QTY:{0}", "XXXX"),
                string.Format("D/C:{0}", "XXXXXXXX"),
                string.Format("NO.:{0}", "XXXX"),
                string.Format("{0}", "XXX"),
                "ABCDEFGHIJKLMNOPQRSTUVWXYZ@ABCDEFGHIJKLMNOPQRSTUVWXYZ@ABCDEFGHIJKLMNOPQRSTUVWXYZ",
                FormatPara(cmbPnX.comboBox, cmbPnY.comboBox, "PN"),
                FormatPara(cmbDclX.comboBox, cmbDclY.comboBox, "DCL"),
                FormatPara(cmbBinX.comboBox, cmbBinY.comboBox, "BIN"),
                FormatPara(cmbPnoX.comboBox, cmbPnoY.comboBox, "PNO"),
                FormatPara(cmbSupX.comboBox, cmbSupY.comboBox, "SUP"),
                FormatPara(cmbQtyX.comboBox, cmbQtyY.comboBox, "QTY"),
                FormatPara(cmbDateX.comboBox, cmbDateY.comboBox, "DATE"),
                FormatPara(cmbNoX.comboBox, cmbNoY.comboBox, "NO"),
                FormatPara(cmbQuaX.comboBox, cmbQuaY.comboBox, "Qualevel"),
                FormatPara(cmbQrcdoeX.comboBox, cmbQrcdoeY.comboBox, "QRCODE"));
        }

        public string FormatPara(Control cmbX, Control cmbY, string key)
        {
            var point = new Point(int.Parse(cmbX.Text), int.Parse(cmbY.Text));

            var read = HikSetup.Setup.IniReadValue("PrintParas", key).Split(',');
            read[0] = point.X.ToString();
            read[1] = point.Y.ToString();

            var str = string.Empty;
            for (var i = 0; i < read.Length; i++)
            {
                str += read[i];
                if (i != read.Length - 1)
                    str += ",";
            }

            return str;
        }
    }
}
