using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CheckSystem.Yfas.Utility;
using Sunny.UI;

namespace CheckSystem.Yfas
{
    public partial class YfasLoginForm : UILoginForm
    {
        public YfasLoginForm()
        {
            InitializeComponent();
        }

        private void FLogin_ButtonLoginClick(object sender, EventArgs e)
        {
            // UserName就是封装了界面里用户名输入框的值
            // Password就是封装了界面里密码输入框的值
            if (string.Equals(UserName, "admin", StringComparison.CurrentCultureIgnoreCase) && 
                string.Equals(Password, "admin", StringComparison.CurrentCultureIgnoreCase))
            {
                YfasDeviceBase.User = "Admin";
                YfasDeviceBase.Limit = YfasDeviceBase.LimitType.Admin;
                IsLogin = true;
            }
            else if (string.Equals(UserName, "systemadmin", StringComparison.CurrentCultureIgnoreCase) &&
                string.Equals(Password, "systemadmin", StringComparison.CurrentCultureIgnoreCase))
            {
                YfasDeviceBase.User = "SystemAdmin";
                YfasDeviceBase.Limit = YfasDeviceBase.LimitType.SystemAdmmin;
                IsLogin = true;
            }
            else if (string.Equals(UserName, "6024", StringComparison.CurrentCultureIgnoreCase) &&
                string.Equals(Password, "6024", StringComparison.CurrentCultureIgnoreCase))
            {
                YfasDeviceBase.User = "6024";
                YfasDeviceBase.Limit= YfasDeviceBase.LimitType.User;
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
