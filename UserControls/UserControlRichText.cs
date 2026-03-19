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
    public partial class UserControlRichText : UserControl
    {
        #region 变量

        private string _strLastWord;
        public List<string> _lstKeyWord = new List<string>() ;

        public List<string> _lstVariant = new List<string>();
        public List<string> LstKeyWord
        {
            get
            {
                return _lstKeyWord;
            }

            set
            {
                _lstKeyWord = value;
            }
        }

        public List<string> LstVariant
        {
            get
            {
                return _lstVariant;
            }

            set
            {
                _lstVariant = value;
            }
        }
        #endregion


        public UserControlRichText()
        {
            InitializeComponent();


        }


        private void CustomerInit()
        {

           // _lstVariant.Clear();
           // _lstKeyWord.Clear();
            _lstVariant.Add("_IN_");
            _lstVariant.Add("_OUT_");

            _lstKeyWord.Add("if");
            _lstKeyWord.Add("else");
            _lstKeyWord.Add("||");
            _lstKeyWord.Add("&&");



            listBox.Items.AddRange(_lstVariant.ToArray());
            if (listBox.Items.Count > 0)
            {
                float fWidth = 0;
                foreach (var item in listBox.Items)
                {
                    if (listBox.CreateGraphics().MeasureString(item.ToString(), listBox.Font).Width > fWidth)
                    {
                        fWidth = listBox.CreateGraphics().MeasureString(item.ToString(), listBox.Font).Width;
                    };
                }
                
                listBox.Width = (int)fWidth + 10;
            }

            listBox.Hide();         

            richTextBox.TextChanged += RichTextBox_TextChanged;
            richTextBox.KeyDown += RichTextBox_KeyDown;
            listBox.KeyDown += ListBox_KeyDown;
        }

        private void ListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)//如果输入的是回车键  
            {
                //跳转到list的向右的箭头，所有减去一个字符长度
                int nMousePos = richTextBox.SelectionStart - 1;
                //在把开始位置设置到开始的地方
                int nPosLastSpace;
                nPosLastSpace = richTextBox.Text.Substring(0, nMousePos).LastIndexOf(" "); //前面空格
                int nPosLastEnter;
                nPosLastEnter = richTextBox.Text.Substring(0, nMousePos).LastIndexOf("\n"); //前面回车
                int nPosStringStart;
                nPosStringStart = nPosLastEnter > nPosLastSpace ? nPosLastEnter : nPosLastSpace;

                if (richTextBox.SelectionStart == 0) //text start point 前面起始
                {
                    nPosStringStart = 0;
                    _strLastWord = "";
                }
                else
                {
                    _strLastWord = richTextBox.Text.Substring(nPosStringStart + 1,
                        richTextBox.SelectionStart - nPosStringStart - 1);
                }

                string strTemp = listBox.SelectedItem.ToString();
                string strText = richTextBox.Text.Substring(0, nPosStringStart + 1) + strTemp + richTextBox.Text.Substring(nMousePos);
                richTextBox.Text = strText;

                richTextBox.SelectionStart = nPosStringStart + strTemp.Length + 1;
 
                //for (int i = 0; i < _strLastWord.Length; i++)
                //{
                //    SendKeys.Send("{BS}");
                //}
                //SendKeys.Send(listBox.SelectedItem.ToString() + " ");
                listBox.Hide();
                richTextBox.Focus();
            }
        }

        private void RichTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right) //如果输入的是Right键  
            {
                if (listBox.Visible)
                {
                    listBox.Focus();
                    listBox.SelectedIndex = 0;
                }
            }
        }

        private void RichTextBox_TextChanged(object sender, EventArgs e)
        {
            int nOldSelectStart = richTextBox.SelectionStart;
            #region 整体着色

            string strText = richTextBox.Text;
            List<char> lstChar = new List<char>(strText.ToCharArray());
            int nWordBeginPos = 0;
            string strKeyWord = "";

            for (int i = 0; i < lstChar.Count; i++)
            {
                if (lstChar[i] == ' ' || lstChar[i] == '\n' || lstChar[i] == ';')
                {
                    strKeyWord = strText.Substring(nWordBeginPos, i - nWordBeginPos);

                    string strFindWordThis = _lstKeyWord.Find(ee => ee.Equals(strKeyWord));
                    if (strFindWordThis != null)
                    {
                        richTextBox.Select(nWordBeginPos, strKeyWord.Length);
                        richTextBox.SelectionColor = Color.Red;
                        richTextBox.Select(nWordBeginPos + strKeyWord.Length, 0);
                        richTextBox.SelectionColor = Color.Black;
                    }
                    else
                    {
                        strFindWordThis = _lstVariant.Find(ee => ee.Equals(strKeyWord));
                        if (strFindWordThis != null)
                        {
                            richTextBox.Select(nWordBeginPos, strKeyWord.Length);
                            richTextBox.SelectionColor = Color.Blue;
                            richTextBox.Select(nWordBeginPos + strKeyWord.Length, 0);
                            richTextBox.SelectionColor = Color.Black;
                        }
                        else
                        {
                            richTextBox.Select(nWordBeginPos, strKeyWord.Length);
                            richTextBox.SelectionColor = Color.Black;
                            richTextBox.Select(nWordBeginPos + strKeyWord.Length, 0);
                            richTextBox.SelectionColor = Color.Black;
                        }
                    }

                    nWordBeginPos += strKeyWord.Length+1;

                }
                else
                {
                   // nWordBeginPos = i + 1;
                }
            }

            #endregion

            //在把开始位置设置到开始的地方
            richTextBox.SelectionStart = nOldSelectStart;

            int nPosLastSpace;
            nPosLastSpace = richTextBox.Text.Substring(0, richTextBox.SelectionStart).LastIndexOf(" "); //前面空格
            int nPosLastEnter;
            nPosLastEnter = richTextBox.Text.Substring(0, richTextBox.SelectionStart).LastIndexOf("\n"); //前面回车
            int nPosStringStart;
            nPosStringStart = nPosLastEnter > nPosLastSpace ? nPosLastEnter : nPosLastSpace;

            if (richTextBox.SelectionStart == 0) //text start point 前面起始
            {
                nPosStringStart = 0;
                _strLastWord = "";
            }
            else
            {
                _strLastWord = richTextBox.Text.Substring(nPosStringStart + 1,
                    richTextBox.SelectionStart - nPosStringStart - 1);
            }



            #region 自动填充listbox

            List<string> listNew = new List<string>();

                if (_strLastWord.Length < 2)
                {
                    listBox.Hide();
                    return;
                }

                foreach (var item in _lstVariant)
                {
                    if (item.Contains(_strLastWord))
                    {
                        listNew.Add(item);
                    }
                }

                if (listNew.Count > 0)
                {
                    listBox.Items.Clear();
                    listBox.Items.AddRange(listNew.ToArray());
                    //listBox.SelectedIndex = 0;
                    listBox.Location = richTextBox.GetPositionFromCharIndex(richTextBox.SelectionStart);
                    listBox.Top += 40;
                    listBox.Left += 20;
                    SizeF size = listBox.CreateGraphics().MeasureString(listBox.Items[0].ToString(), listBox.Font);
                    listBox.Height = Convert.ToInt32(size.Height) * listBox.Items.Count;

                    listBox.Show();
                }


            #endregion


        }

        private void UserControlRichText_Load(object sender, EventArgs e)
        {
            CustomerInit();
            RichTextBox_TextChanged(null, null);
        }
    }
}
