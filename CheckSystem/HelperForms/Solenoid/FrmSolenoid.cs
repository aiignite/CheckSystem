using System.Threading;
using Controller;
using OpenCvSharp.Util;
using Sunny.UI;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckSystem.HelperForms.Solenoid
{
    public partial class FrmSolenoid : UIForm
    {
        private readonly SyControllerWith56Pin _controller = new SyControllerWith56Pin("LIN");
        private readonly SolenoidValueAppDownload _appDownload = new SolenoidValueAppDownload("App");
        private bool _bStart;

        public FrmSolenoid()
        {
            InitializeComponent();

            _controller.InitRemoteIpAddress("192.168.1.28:8088");
            _appDownload.Lin = _controller.GatewayLin;
            txtAppFilePath.Text = _appDownload.AppFilePath;
            txtCaliFilePath.Text = _appDownload.CalFilePath;

            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    await Task.Delay(50);
                    if (!_bStart)
                    {
                        _controller.GetInputs();
                        if (_controller.Di1 == "1")
                            _bStart = true;
                    }
                }
            });

            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    await Task.Delay(50);
                    if (_bStart)
                    {
                        BeginInvoke(new Action(() =>
                        {
                            uiPanel1.Enabled = false;
                            txtResult.Text = @"正在下载";
                        }));

                        _appDownload.StartAppDownload();

                        BeginInvoke(new Action(() =>
                        {
                            txtResult.Text = string.Format(@"{0} : {1}", _appDownload.DownloadResult, _appDownload.DownloadCostTime);
                            uiPanel1.Enabled = true;
                        }));

                        _bStart = false;
                    }
                }
            });
        }

        private void uiButton1_Click(object sender, System.EventArgs e)
        {
            var openFi = new OpenFileDialog
            {
                Site = null,
                Tag = null,
                AddExtension = false,
                CheckPathExists = false,
                DefaultExt = null,
                DereferenceLinks = false,
                FileName = null,
                Filter = null,
                FilterIndex = 0,
                InitialDirectory = null,
                RestoreDirectory = false,
                ShowHelp = false,
                SupportMultiDottedExtensions = false,
                Title = null,
                ValidateNames = false,
                AutoUpgradeEnabled = false,
                CheckFileExists = false,
                Multiselect = false,
                ReadOnlyChecked = false,
                ShowReadOnly = false
            };
            openFi.Filter = "文件(sx,s19)|*.sx;*.s19;";
            if (openFi.ShowDialog() == DialogResult.OK)
            {
                _appDownload.AppFilePath = openFi.FileName;
                txtAppFilePath.Text = openFi.FileName;
            }
        }

        private void uiButton2_Click(object sender, System.EventArgs e)
        {
            var openFi = new OpenFileDialog
            {
                Site = null,
                Tag = null,
                AddExtension = false,
                CheckPathExists = false,
                DefaultExt = null,
                DereferenceLinks = false,
                FileName = null,
                Filter = null,
                FilterIndex = 0,
                InitialDirectory = null,
                RestoreDirectory = false,
                ShowHelp = false,
                SupportMultiDottedExtensions = false,
                Title = null,
                ValidateNames = false,
                AutoUpgradeEnabled = false,
                CheckFileExists = false,
                Multiselect = false,
                ReadOnlyChecked = false,
                ShowReadOnly = false
            };
            openFi.Filter = "文件(sx,s19)|*.sx;*.s19;";
            if (openFi.ShowDialog() == DialogResult.OK)
            {
                _appDownload.CalFilePath = openFi.FileName;
                txtCaliFilePath.Text = openFi.FileName;
            }
        }

        private void uiButton3_Click(object sender, System.EventArgs e)
        {
            if (!_bStart)
                _bStart = true;
        }

        private async void btnWriteFirstUp_Click(object sender, System.EventArgs e)
        {
            if (!_bStart)
            {
                uiPanel1.Enabled = false;

                await Task.Run(() =>
                {
                    _appDownload.WriteFirstPointUp();
                    Thread.Sleep(1500);
                });

                uiPanel1.Enabled = true;
                btnReadFirst.PerformClick();
            }
        }

        private async void btnWriteFirstDown_Click(object sender, System.EventArgs e)
        {
            if (!_bStart)
            {
                uiPanel1.Enabled = false;

                await Task.Run(() =>
                {
                    _appDownload.WriteFirstPointDown();
                    Thread.Sleep(1500);
                });

                uiPanel1.Enabled = true;
                btnReadFirst.PerformClick();
            }
        }

        private async void btnWriteSecondUp_Click(object sender, System.EventArgs e)
        {
            if (!_bStart)
            {
                uiPanel1.Enabled = false;

                await Task.Run(() =>
                {
                    _appDownload.WriteSecondPointUp();
                    Thread.Sleep(1500);
                });

                uiPanel1.Enabled = true;
                btnReadSecond.PerformClick();
            }
        }

        private async void btnWriteSecondDown_Click(object sender, System.EventArgs e)
        {
            if (!_bStart)
            {
                uiPanel1.Enabled = false;

                await Task.Run(() =>
                {
                    _appDownload.WriteSecondPointDown();
                    Thread.Sleep(1500);
                });

                uiPanel1.Enabled = true;
                btnReadSecond.PerformClick();
            }
        }

        private async void btnReadFirst_Click(object sender, System.EventArgs e)
        {
            if (!_bStart)
            {
                uiPanel1.Enabled = false;
                txtFirstFlag.Text = string.Empty;
                txtFirstOutput.Text = string.Empty;

                await Task.Run(() =>
                {
                    if (!_bStart)
                        _appDownload.ReadFirstPoint();
                });

                txtFirstFlag.Text = _appDownload.FirstPointWriteFlag;
                txtFirstOutput.Text = _appDownload.FirstPointOutputDuty;
                uiPanel1.Enabled = true;
            }
        }

        private async void btnReadSecond_Click(object sender, System.EventArgs e)
        {
            if (!_bStart)
            {
                uiPanel1.Enabled = false;
                txtSecondFlag.Text = string.Empty;
                txtSecondOutput.Text = string.Empty;

                await Task.Run(() =>
                {
                    if (!_bStart)
                        _appDownload.ReadSecondPoint();
                });

                txtSecondFlag.Text = _appDownload.SecondPointWriteFlag;
                txtSecondOutput.Text = _appDownload.SecondPointOutputDuty;
                uiPanel1.Enabled = true;
            }
        }
    }
}
