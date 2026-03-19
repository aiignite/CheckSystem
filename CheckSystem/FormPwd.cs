using System;
using System.Drawing;
using System.Windows.Forms;
using HZH_Controls.IconFont;

namespace CheckSystem
{
    public partial class FormPwd : Form
    {
        public FormPwd()
        {
            InitializeComponent();
            Icon = FontImages.GetIcon(FontIcons.A_fa_code, 32,
                Color.DodgerBlue);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
                return;

            if (textBox1.Text == @"scae123456" || textBox1.Text == @"098765")
                DialogResult = DialogResult.OK;
            else
            {
                textBox1.Text = string.Empty;
                MessageBox.Show(@"密码错误");
                textBox1.Focus();
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(textBox1.Text))
                button1_Click(null, null);
        }
    }
}
