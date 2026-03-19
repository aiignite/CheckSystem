using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckSystem.VisionDetection
{
    public partial class UcGroupSelect : UserControl
    {
        public UcGroupSelect()
        {
            InitializeComponent();
        }

        public string LabelString
        {
            get { return cmbName.Text; }
            set { cmbName.Text = value; }
        }

        public override string Text
        {
            get
            {
                return comboBox.Text;
            }

            set
            {
                comboBox.Text = string.Empty;
                comboBox.Text = value;
            }
        }
    }
}
