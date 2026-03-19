using Sunny.UI;
using System;
using System.Windows.Forms;

namespace CheckSystem
{
    public partial class FormLogin : UILoginForm
    {
        private readonly Func<string, string, bool> _customLoginFunc;

        public FormLogin()
        {
            InitializeComponent();
        }

        public FormLogin(Func<string, string, bool> loginFunc)
        {
            InitializeComponent();
            _customLoginFunc = loginFunc;
        }

        private void FLogin_ButtonLoginClick(object sender, EventArgs e)
        {
            if (_customLoginFunc != null)
            {
                IsLogin = _customLoginFunc.Invoke(UserName, Password);
            }
            else
            {
                // UserName就是封装了界面里用户名输入框的值
                // Password就是封装了界面里密码输入框的值
                if (UserName == "admin" && Password == "admin")
                {
                    IsLogin = true;
                    //var frmSelection = new FormSelection();
                    //frmSelection.ShowDialog();
                    //Close();
                }
            }

            if (IsLogin)
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                this.ShowErrorTip("用户名或者密码错误。");
            }
        }
    }
}
