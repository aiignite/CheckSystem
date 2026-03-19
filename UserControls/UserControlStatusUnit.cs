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
    public partial class UserControlStatusUnit : UserControl
    {
        public UserControlStatusUnit()
        {
            InitializeComponent();
        }

        private void UserControlStatusUnit_Enter(object sender, EventArgs e)
        {

        }

        private void UserControlStatusUnit_Load(object sender, EventArgs e)
        {
            textEditorControl1.SetHighlighting("C#");
            textEditorControl2.SetHighlighting("C#");
            textEditorControl3.SetHighlighting("C#");
            textEditorControl1.HorizontalScroll.Visible = false;
            textEditorControl1.VerticalScroll.Visible = false;
        }
    }
}
