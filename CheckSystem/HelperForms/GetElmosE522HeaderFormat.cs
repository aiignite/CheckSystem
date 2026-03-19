using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CommonUtility;

namespace CheckSystem.HelperForms
{
    public partial class GetElmosE522HeaderFormat : Form
    {
        public GetElmosE522HeaderFormat()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                textBox2.Text = string.Empty;
                textBox3.Text = string.Empty;
                textBox4.Text = string.Empty;
                textBox5.Text = string.Empty;
                textBox6.Text = string.Empty;

                var str = textBox1.Text.Replace(" ", "");
                var listByte = new List<byte>();
                for (var i = 0; i < str.Length; i = i + 2)
                {
                    var b1 = Convert.ToByte(string.Format("{0}{1}", str[i], str[i + 1]), 16);
                    listByte.Add(b1);
                }

                textBox2.Text = ValueHelper.GetHextStr(listByte[0]);

                var data1Bits = Convert.ToString(listByte[1], 2).PadLeft(8, '0');
                var data2Bits = Convert.ToString(listByte[2], 2).PadLeft(8, '0');

                textBox3.Text =
                    ValueHelper.GetHextStr(
                        Convert.ToByte(
                            string.Format("000{0}{1}{2}{3}{4}", data1Bits[3], data1Bits[4], data1Bits[5], data1Bits[6],
                                data1Bits[7]), 2));

                textBox4.Text = data1Bits[2].ToString() == "0" ? "Read" : "Write";

                var n1 = string.Format("0000{0}{1}{2}{3}", data2Bits[6], data2Bits[7], data1Bits[0], data1Bits[0]);
                textBox5.Text = ValueHelper.GetHextStr(Convert.ToByte(n1, 2));

                textBox6.Text = ValueHelper.GetHextStr(listByte[2]);
            }
            catch (Exception ex)
            {
                textBox2.Text = ex.Message;
                textBox3.Text = ex.Message;
                textBox4.Text = ex.Message;
                textBox5.Text = ex.Message;
                textBox6.Text = ex.Message;
            }
        }
    }
}
