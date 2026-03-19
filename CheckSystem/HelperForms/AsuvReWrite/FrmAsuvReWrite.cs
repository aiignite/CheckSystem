using Controller;
using Sunny.UI;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckSystem.HelperForms.AsuvReWrite
{
    public partial class FrmAsuvReWrite : Form
    {
        private HelaASuvReWrite _helaAsuvReWriter = new HelaASuvReWrite("asuv");
        private SyControllerMaster _linController = new SyControllerMaster("lin");
        //private SyControllerWith56Pin _linController = new SyControllerWith56Pin("lin");

        public FrmAsuvReWrite()
        {
            InitializeComponent();
            Load += FrmAsuvReWrite_Load;
        }

        private void FrmAsuvReWrite_Load(object sender, EventArgs e)
        {
            var option = new UIEditOption
            {
                AutoLabelWidth = true,
                Text = "选择机种"
            };

            var str = new string[] { "左灯(L)", "右灯(R)" };
            option.AddCombobox("IsLeft", "Left/Right", str, 0);

            var frm = new UIEditForm(option) { Font = new Font("宋体", 8f) };
            frm.Render();
            frm.ShowDialog();

            if (!frm.IsOK)
            {
                MessageBox.Show("请重启后先选择机种后再开始");
                lblInfo.Text = "请重启后先选择机种后再开始";
                btnStart.Enabled = false;
                ledResult.On = false;
                ledResult.Color = Color.DarkRed;
                uiTitlePanel1.Style = UIStyle.Red;

                return;
            }

            var index = (int)frm["IsLeft"];
            var isLeft = index == 0;
            uiTitlePanel1.Text = str[index];
            if (isLeft)
                _helaAsuvReWriter.SetLeftRcla();
            else
                _helaAsuvReWriter.SetRightRcla();

            try
            {
                _linController.InitRemoteIpAddr("192.168.1.28:8088");
                _helaAsuvReWriter.Lin19200 = _linController.MasterGatewayLin;

                //_linController.InitRemoteIpAddress("192.168.1.28:8088");
                //_helaAsuvReWriter.Lin19200 = _linController.GatewayLin;

                lblInfo.Text = string.Format("[{0}] 等待开始...", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                btnStart.Enabled = true;
                ledResult.On = false;
                ledResult.Color = Color.DarkGoldenrod;
                btnStart.Click += BtnStart_Click;
            }
            catch (Exception)
            {
                MessageBox.Show("控制器连接失败");
                lblInfo.Text = "控制器连接失败";
                btnStart.Enabled = false;
                ledResult.On = false;
                ledResult.Color = Color.DarkRed;
            }
        }

        private async void BtnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            ledResult.On = true;
            ledResult.Color = Color.DarkGoldenrod;
            txtFazit.Text = string.Empty;
            txtSerialNo.Text = string.Empty;
            lblInfo.Text = string.Format("[{0}] 正在写入...", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

            var res = await WriteAsuv();
            btnStart.Enabled = true;
            txtFazit.Text = _helaAsuvReWriter.FazitId;
            txtSerialNo.Text = _helaAsuvReWriter.SerialNumber;

            if (res)
            {
                lblInfo.Text = string.Format("[{0}] 写入OK...", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                ledResult.On = true;
                ledResult.Color = Color.Green;
            }
            else
            {
                lblInfo.Text = string.Format("[{0}] 写入NG...", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                ledResult.On = true;
                ledResult.Color = Color.Red;
            }
        }

        private Task<bool> WriteAsuv()
        {
            return Task.Run(() =>
            {
                _helaAsuvReWriter.WriteFazitIdAndSerialNumber();
                return _helaAsuvReWriter.FazitId.StartsWith("OK") && _helaAsuvReWriter.SerialNumber.StartsWith("OK");
            });
        }
    }
}
