using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserControls
{
    public partial class LabelText : UserControl
    {

        public string LabelString
        {
            get { return label.Text; }
            set { label.Text = value; }
        }

        public string TipOfTextBox = "提示";


        public override string Text
        {
            get
            {
                return textBox.Text;
            }

            set
            {
                textBox.Text = value;
            }
        }

        //public string strToolTipText { get; set; }
        public LabelText()
        {
            InitializeComponent();

            Dock = DockStyle.Top;

            //this.textBox.Enter += TextBox_Enter;
            //this.textBox.Leave += TextBox_Leave;

        }

        private void LabelText_MouseLeave(object sender, EventArgs e)
        {
            toolTip1.Hide(this.textBox);
        }

        private void LabelText_MouseEnter(object sender, EventArgs e)
        {

            toolTip1.Show(TipOfTextBox, this.textBox);

        }

        //private void TextBox_Leave(object sender, EventArgs e)
        //{
        //    if (Text.Trim().Length==0)
        //    {
        //        this.textBox.ForeColor = Color.DarkGray;
        //        Text = strToolTipTitle;
        //    }
        //}

        //private void TextBox_Enter(object sender, EventArgs e)
        //{
        //    if (Text.Equals(strToolTipTitle))
        //    {
        //        this.textBox.ForeColor = Color.Black;
        //        Text = "";
        //    }
        //}


        private void textBox_MouseEnter(object sender, EventArgs e)
        {
            //   this.toolTip1.ToolTipTitle = strToolTipTitle;
            //
            Tag = "@TipText:提示";
            List<string> lstTag = new List<string>(Tag.ToString().Split(','));
            string tip = lstTag.Find(t => t.StartsWith("@TipText:")) == null ?
                ""
                : lstTag.Find(t => t.StartsWith("@TipText:")).Substring("@TipText:".Length);
            //  toolTip1.ToolTipTitle =  tip;
            toolTip1.Show(TipOfTextBox, this.textBox);
        }

        private void textBox_MouseLeave(object sender, EventArgs e)
        {
            this.toolTip1.Hide(textBox);     //隐藏提示窗口
        }

    }
}
