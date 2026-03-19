using System;
using System.Windows.Forms;
using CheckSystem.VisionDetection.Vision;
using Sunny.UI;

namespace CheckSystem.VisionDetection
{
    public partial class FrmVisionLogin : UILoginForm
    {
        public FrmVisionLogin()
        {
            InitializeComponent();
        }

        private void FLogin_ButtonLoginClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
            {
                this.ShowErrorTip("请输入用户名或密码。");
                return;
            }

            // UserName就是封装了界面里用户名输入框的值
            // Password就是封装了界面里密码输入框的值
            if (string.Equals(UserName, "admin", StringComparison.CurrentCultureIgnoreCase) &&
                string.Equals(Password, "admin", StringComparison.CurrentCultureIgnoreCase))
            {
                VisionCommon.LoginUser = UserName;
                IsLogin = true;
            }
            else if (string.Equals(UserName, "systemadmin", StringComparison.CurrentCultureIgnoreCase) &&
                     string.Equals(Password, "systemadmin", StringComparison.CurrentCultureIgnoreCase))
            {
                VisionCommon.LoginUser = UserName;
                IsLogin = true;
            }
            else if (
                     string.Equals(Password, UserName, StringComparison.CurrentCultureIgnoreCase))
            {
                VisionCommon.LoginUser = UserName;
                IsLogin = true;
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
