using System;
using System.Drawing;
using Sunny.UI;

namespace CheckSystem.HelperForms.Sgm458Plg
{
    public partial class GetPlgCompareVersionValue : UIForm
    {
        public GetPlgCompareVersionValue()
        {
            InitializeComponent();
        }

        private void uiButton1_Click(object sender, System.EventArgs e)
        {
            var defaultBackcolor = Color.FromArgb(243, 249, 255);
            var okBackColor = Color.Green;
            var ngBackColor = Color.Red;

            txtGetValue.FillReadOnlyColor = defaultBackcolor;
            txtGetValue.Text = string.Empty;
            txtGetValue.ForeReadOnlyColor = Color.FromArgb(109, 109, 103);

            if (string.IsNullOrEmpty(txtDuns.Text) ||
                string.IsNullOrEmpty(txtVpps.Text) ||
                string.IsNullOrEmpty(txtBasePartNo.Text) ||
                string.IsNullOrEmpty(txtBaseAlphaCode.Text) ||
                string.IsNullOrEmpty(txtEndPartNo.Text) ||
                string.IsNullOrEmpty(txtEndAlphaCode.Text))
            {
                ShowErrorTip("信息不能为空");
                return;
            }

            var duns = txtDuns.Text;
            var vpps = txtVpps.Text;
            var basePartNo = txtBasePartNo.Text;
            var baseAlphaCode = txtBaseAlphaCode.Text;
            var endPartNo = txtEndPartNo.Text;
            var endAlphaCode = txtEndAlphaCode.Text;

            if (duns.Length != 9)
            {
                ShowErrorTip("DUNS长度错误");
                return;
            }

            for (var i = 0; i < duns.Length; i++)
            {
                var s = duns[i].ToString();
                var intValue = 0;
                if (!int.TryParse(s, out intValue))
                {
                    ShowErrorTip("DUNS格式错误");
                    return;
                }
            }

            if (vpps.Length != 14)
            {
                ShowErrorTip("VPPS长度错误");
                return;
            }

            if (basePartNo.Length != 8)
            {
                ShowErrorTip("BaseModulePartNumber长度错误");
                return;
            }
            if (endPartNo.Length != 8)
            {
                ShowErrorTip("BaseModulePartNumber长度错误");
                return;
            }
            if (baseAlphaCode.Length != 2)
            {
                ShowErrorTip("BaseModuleAlphaCode长度错误");
                return;
            }
            if (endAlphaCode.Length != 2)
            {
                ShowErrorTip("EndModuleAlphaCode长度错误");
                return;
            }

            var plg458AmNoMos = new Controller.Sgm458Plg("");
            plg458AmNoMos.ServerIp = "192.168.1.150";
            plg458AmNoMos.ServerDataBase = "IPMS";
            plg458AmNoMos.ServerUid = "sa";
            plg458AmNoMos.ServerPwd = "123456";
            plg458AmNoMos.ToWriteDunsAscii = duns;
            plg458AmNoMos.ToWriteVppsAscii = vpps;
            plg458AmNoMos.ToWriteBaseModulePartNumberDec = basePartNo;
            plg458AmNoMos.ToWriteBaseModuleAlphaCodeAscii = baseAlphaCode;
            plg458AmNoMos.ToWriteEndModulePartNumberDec = endPartNo;
            plg458AmNoMos.ToWriteEndModuleAlphaCodeAscii = endAlphaCode;

            plg458AmNoMos.GenerateVersion("ppp");
            var generatedToCompare = plg458AmNoMos.GeneratedWriteVersionHex.Substring(50, 52);
            //Console.WriteLine(generatedToCompare);

            var failValue = string.Empty;
            txtGetValue.ForeReadOnlyColor = Color.WhiteSmoke;
            for (var i = 0; i < 51; i++)
                failValue += "FF";
            if (failValue == plg458AmNoMos.GeneratedWriteVersionHex)
            {
                txtGetValue.FillReadOnlyColor = ngBackColor;
                ShowErrorTip("生成失败");
            }
            else
            {
                txtGetValue.FillReadOnlyColor = okBackColor;
                txtGetValue.Text = generatedToCompare;
                ShowSuccessTip("生成成功");
            }
        }
    }
}
