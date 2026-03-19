using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserControls
{
    public partial class UserControlTextEdit : UserControl
    {
        public UserControlTextEdit()
        {
            InitializeComponent();
        }

        List<string> lstKeyWord = new List<string>() { "if", "else", "switch", "case" };

        List<string> lstInputPort = new List<string>() {"I_Cyclinder1_Sensor1", "I_Cyclinder1_Sensor2", "I_Cyclinder2_Sensor1", "I_Cyclinder2_Sensor2" };

        private bool _bListBoxFocus = false;
        private void UserControlTextEdit_Load(object sender, EventArgs e)
        {
            richTextBox.Text = @"StatusUnitIndex:
StatusName:
StatusCnName:
ENTER:


DURING:


EXIT:
	TargetStatusUnit:

	TargetStatusCondition:

	TargetStatusUnit1:

	TargetStatusCondition1:";

            UpdateTextColor();

            listBox.Items.AddRange(lstInputPort.ToArray());
            SizeF size = listBox.CreateGraphics().MeasureString(listBox.Items[0].ToString(), listBox.Font);
            listBox.Width = Convert.ToInt32(size.Width) + 10;
            listBox.Hide();

            richTextBox.TextChanged += RichTextBox_TextChanged;
            richTextBox.KeyDown += RichTextBox_KeyDown;
            listBox.KeyDown += ListBox_KeyDown;

        }

        private void RichTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down) //如果输入的是回车键  
            {
                if (listBox.Visible)
                {
                    listBox.Focus();
                    _bListBoxFocus = true;
                }
            }
        }

        private void RichTextBox_TextChanged(object sender, EventArgs e)
        {
            //richTextBox.GetLineFromCharIndex(richTextBox.SelectionStart);
            //string strLine = richTextBox.Lines[richTextBox.GetLineFromCharIndex(richTextBox.SelectionStart)];
            //int nCol = richTextBox.SelectionStart -
            //                (richTextBox.GetFirstCharIndexFromLine(
            //                    1 + richTextBox.GetLineFromCharIndex(richTextBox.SelectionStart) - 1));
            //int nTemp = richTextBox.Lines[richTextBox.GetLineFromCharIndex(richTextBox.SelectionStart)].LastIndexOf(" ");
            //int a = richTextBox.SelectionStart;

            if (_bListBoxFocus)
            {
                return;
            }


            int nTemp;
            nTemp = richTextBox.Text.Substring(0, richTextBox.SelectionStart).LastIndexOf(" ");
            if (nTemp <= 0 || nTemp == richTextBox.SelectionStart)
            {
                return;
            }

            string strWord = richTextBox.Text.Substring(nTemp + 1, richTextBox.SelectionStart - nTemp - 1);

            #region 关键字颜色

            string strFindWord = lstKeyWord.Find(ee => ee.Equals(strWord));

            if (strFindWord != null)
            {
                richTextBox.Select(nTemp + 1, strWord.Length);
                richTextBox.SelectionColor = Color.Red;
                richTextBox.Select(nTemp + 1 + strWord.Length, 0);
                richTextBox.SelectionColor = Color.Black;
            }
            else
            {
                strFindWord = lstInputPort.Find(ee => ee.Equals(strWord));
                if (strFindWord != null)
                {
                    richTextBox.Select(nTemp + 1, strWord.Length);
                    richTextBox.SelectionColor = Color.Blue;
                    richTextBox.Select(nTemp + 1 + strWord.Length, 0);
                    richTextBox.SelectionColor = Color.Black;
                }
                else
                {
                    richTextBox.Select(nTemp + 1, strWord.Length);
                    richTextBox.SelectionColor = Color.Black;
                    richTextBox.Select(nTemp + 1 + strWord.Length, 0);
                    richTextBox.SelectionColor = Color.Black;
                }
     
            }



            #endregion

            #region 自动填充listbox

            List<string> listNew = new List<string>();

            if (strWord.Length < 3)
            {
                listBox.Hide();
                return;
            }

            foreach (var item in lstInputPort)
            {
                if (item.Contains(strWord))
                {
                    listNew.Add(item);
                }
            }

            if (listNew.Count > 0)
            {
                listBox.Items.Clear();
                listBox.Items.AddRange(listNew.ToArray());
                listBox.SelectedIndex = 0;
                listBox.Location = richTextBox.GetPositionFromCharIndex(richTextBox.SelectionStart);
                listBox.Top += 40;
                listBox.Left += 20;
                listBox.Show();
            }


            #endregion



        }

        private  void UpdateTextColor()
        {
            List<string> lstTemp = new List<string>() { "StatusUnitIndex:", "StatusName:", "StatusCnName:", "ENTER:", "DURING:", "EXIT:", "TargetStatusUnit:", "TargetStatusCondition:", "TargetStatusUnit1:", "TargetStatusCondition1:" };

            foreach (var item in lstTemp)
            {
                int nTemp = richTextBox.Text.LastIndexOf(item);
                if (nTemp < 0)
                {
                    continue;
                }
                richTextBox.Select(nTemp, item.Length);
                richTextBox.SelectionColor = Color.DarkGray;
                Font oldFont = richTextBox.SelectionFont;
                richTextBox.SelectionFont = new Font(oldFont, oldFont.Style | FontStyle.Italic);
                richTextBox.SelectionProtected = true;

            }
        }


        private void ListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)//如果输入的是回车键  
            {
                //richTextBox.Text =   richTextBox.Text.Insert(richTextBox.SelectionStart, listBox.SelectedItem.ToString());
                this.listBox.Hide();
                int nTemp;
                nTemp = richTextBox.Text.Substring(0, richTextBox.SelectionStart).LastIndexOf(" ");
                
                string strWord = richTextBox.Text.Substring(nTemp + 1, richTextBox.SelectionStart - nTemp - 1);

                //txtMessageContent.AppendText(tips
                //    = tips.Substring(lstTips.Prefix.Length, tips.Length
                //                                            - lstTips.Prefix.Length));

                for (int i = 0; i < strWord.Length; i++)
                {
                    SendKeys.Send("{BS}");
                }
     

                SendKeys.Send(listBox.SelectedItem.ToString()+ " ");
                listBox.Hide();
                richTextBox.Focus();

                Thread.Sleep(200);

                _bListBoxFocus = false;
            }
        }
    }

}
