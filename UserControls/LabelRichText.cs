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
    public partial class LabelRichText : UserControl
    {
        public override string Text
        {
            get
            {
                return userControlRichText1.richTextBox.Text;
            }

            set
            {
                userControlRichText1.richTextBox.Text = "";
                userControlRichText1.richTextBox.Text = value;
            }
        }

        public string LabelString
        {
            get { return label.Text; }
            set { label.Text = value; }
        }
        public LabelRichText()
        {
            InitializeComponent();
            userControlRichText1._lstVariant.Add("INNN");
        }
    }
}
