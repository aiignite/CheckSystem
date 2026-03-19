using CommonUtility.MyCameraSdk;
using CommonUtility.MyCameraSdk.Common.Enum;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckSystem.DebugCCD
{
    public partial class FormCameraTest : UIForm
    {
        private static List<string> _hkBrands;
        private static List<string> _dhBrands;
        private static readonly List<string> _usedSn = new List<string>();

        private ICamera _cameraL;
        private ICamera _cameraR;

        private string _snL = "DA0174564";
        private string _snR = "GZ0200090004";

        public FormCameraTest()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            Load += FormCameraTest_Load;
            btnStartGrab.Click += async (ns, ne) =>
            {
                btnStartGrab.Enabled = false;
                await Task.Run(() =>
                {
                    _cameraL?.StartWith_Continue_SetCallback((bmp) =>
                    {
                        //pictureBox1.Invoke(new Action(() => { pictureBox1.Image = bmp; }));
                        SetPictureBoxImage(pictureBox1, bmp);
                    });

                    _cameraR?.StartWith_Continue_SetCallback((bmp) =>
                    {
                        //pictureBox2.Invoke(new Action(() => { pictureBox2.Image = bmp; }));
                        SetPictureBoxImage(pictureBox2, bmp);
                    });
                });
            };
        }

        private void FormCameraTest_Load(object sender, EventArgs e)
        {
            SetDoubleBuffered(pictureBox1);
            SetDoubleBuffered(pictureBox2);

            if (_hkBrands is null)
                _hkBrands = CamFactory.GetDeviceEnum(CameraBrand.HIK);
            if (_dhBrands is null)
                _dhBrands = CamFactory.GetDeviceEnum(CameraBrand.DaHeng);

            if (_hkBrands != null && _hkBrands.Any(f => string.Equals(f, _snL, StringComparison.CurrentCultureIgnoreCase)))
            {
                if (!_usedSn.Contains(_snL))
                {
                    _usedSn.Add(_snL);
                    _cameraL = CamFactory.CreateCamera(CameraBrand.HIK);
                    _cameraL.InitDevice(_snL);
                }
            }

            if (_dhBrands != null && _dhBrands.Any(f => string.Equals(f, _snR, StringComparison.CurrentCultureIgnoreCase)))
            {
                if (!_usedSn.Contains(_snR))
                {
                    _usedSn.Add(_snR);
                    _cameraR = CamFactory.CreateCamera(CameraBrand.DaHeng);
                    _cameraR.InitDevice(_snR);
                }
            }
        }

        private static void SetDoubleBuffered(ISynchronizeInvoke control)
        {
            //获取控件的Type,设置双缓存
            Type dgvType = control.GetType();
            PropertyInfo properInfo = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            //为控件的属性设置值
            if (properInfo != null) properInfo.SetValue(control, true, null);
        }

        /// <summary>
        /// 多线程设置PictureBox的图像
        /// </summary>
        /// <param name="control"></param>
        /// <param name="value"></param>
        private static void SetPictureBoxImage(ISynchronizeInvoke control, Bitmap value)
        {
            control.Invoke(new Action<PictureBox, Bitmap>((ct, v) => { ct.Image = v; }), new object[] { control, value });
        }
    }
}
