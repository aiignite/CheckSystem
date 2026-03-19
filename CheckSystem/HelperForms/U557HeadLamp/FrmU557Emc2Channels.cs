using Controller;
using Sunny.UI;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckSystem.HelperForms.U557HeadLamp
{
    public partial class FrmU557Emc2Channels : UIForm
    {
        private Controller.U557HeadLamp _u557HeadLamplLin1 = new Controller.U557HeadLamp("U557_EMC_Lin1");
        private Controller.U557HeadLamp _u557HeadLamplLin2 = new Controller.U557HeadLamp("U557_EMC_Lin2");
        private ToomossUsb2XxxCanLin _toomossUsb2XxxCanLin = new ToomossUsb2XxxCanLin("LIN_Transfer");

        /// <summary>
        /// 当前窗体的宽度
        /// </summary>
        private float _x;

        /// <summary>
        /// 当前窗体的高度
        /// </summary>
        private float _y;

        public FrmU557Emc2Channels()
        {
            InitializeComponent();
            Load += FrmU557Emc2Channels_Load;
            Closed += FrmU557Emc2Channels_Closed;
        }

        private void FrmU557Emc2Channels_Closed(object sender, EventArgs e)
        {
            _u557HeadLamplLin1.Dispose();
            _u557HeadLamplLin2.Dispose();
            _toomossUsb2XxxCanLin.Dispose();
        }

        private void FrmU557Emc2Channels_Load(object sender, EventArgs e)
        {
            #region UI缩放
            _x = Width;//获取窗体的宽度
            _y = Height;//获取窗体的高度
            SetTag(this);//调用方法
            Resize += FrmU557Emc2Channels_Resize;

            Width = (int)(Screen.PrimaryScreen.Bounds.Width * 0.75);
            Height = (int)(Screen.PrimaryScreen.Bounds.Height * 0.75);
            Location = new Point((int)(Screen.PrimaryScreen.Bounds.Width * 0.1),
                (int)(Screen.PrimaryScreen.Bounds.Height * 0.1));
            #endregion

            _u557HeadLamplLin1.LinWithBaudRate10417 = _toomossUsb2XxxCanLin.Lin1;
            _toomossUsb2XxxCanLin.SetBaudRate("1", "10417");
            _u557HeadLamplLin1.LinStartScheduler();

            _u557HeadLamplLin2.LinWithBaudRate10417 = _toomossUsb2XxxCanLin.Lin2;
            _toomossUsb2XxxCanLin.SetBaudRate("2", "10417");
            _u557HeadLamplLin2.LinStartScheduler();

            RegisterEvent();
            InitControl();
        }

        private void FrmU557Emc2Channels_Resize(object sender, EventArgs e)
        {
            var newX = (Width) / _x; //窗体宽度缩放比例
            var newY = ((Height) / _y) / 1;//窗体高度缩放比例
            SetControls(newX, newY, this);//随窗体改变控件大小
        }

        /// <summary>
        /// 将控件的宽，高，左边距，顶边距和字体大小暂存到tag属性中
        /// </summary>
        /// <param name="cons">递归控件中的控件</param>
        private static void SetTag(Control cons)
        {
            foreach (Control con in cons.Controls)//循环窗体中的控件
            {
                con.Tag = con.Width + ":" + con.Height + ":" + con.Left + ":" + con.Top + ":" + con.Font.Size;
                if (con.Controls.Count > 0)
                    SetTag(con);
            }
        }

        /// <summary>
        /// 根据窗体大小调整控件大小
        /// </summary>
        /// <param name="newX">窗体宽度缩放比例</param>
        /// <param name="newY">窗体高度缩放比例</param>
        /// <param name="cons">随窗体改变控件大小</param>
        private static void SetControls(float newX, float newY, Control cons)
        {
            //遍历窗体中的控件，重新设置控件的值
            foreach (Control con in cons.Controls)
            {
                if (con.Tag != null)
                {
                    try
                    {
                        var myTag = con.Tag.ToString().Split(new char[] { ':' }); //获取控件的Tag属性值，并分割后存储字符串数组
                        var a = Convert.ToSingle(myTag[0]) * newX; //根据窗体缩放比例确定控件的值，宽度
                        con.Width = (int)a; //宽度
                        a = Convert.ToSingle(myTag[1]) * newY; //高度
                        con.Height = (int)(a);
                        a = Convert.ToSingle(myTag[2]) * newX; //左边距离
                        con.Left = (int)(a);
                        a = Convert.ToSingle(myTag[3]) * newY; //上边缘距离
                        con.Top = (int)(a);
                        var currentSize = Convert.ToSingle(myTag[4]) * newY; //字体大小
                        con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                        if (con.Controls.Count > 0)
                            SetControls(newX, newY, con);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
        }

        private void RegisterEvent()
        {
            swMode5Lin1.ActiveChanged += SwMode5Lin1_ActiveChanged;
            cmbEmcModeLin1.SelectedIndexChanged += CmbEmcModeLin1_SelectedIndexChanged;

            swMode5Lin2.ActiveChanged += SwMode5Lin2_ActiveChanged;
            cmbEmcModeLin2.SelectedIndexChanged += CmbEmcModeLin2_SelectedIndexChanged;
        }

        private void InitControl()
        {
            cmbEmcModeLin1.SelectedIndex = 0;
            cmbEmcModeLin2.SelectedIndex = 0;
        }

        private async void SwMode5Lin1_ActiveChanged(object sender, EventArgs e)
        {
            gpEmcLin1.Enabled = false;
            cmbEmcModeLin1.Enabled = !swMode5Lin1.Active;

            await Task.Run(() =>
            {
                if (swMode5Lin1.Active)
                {
                    _u557HeadLamplLin1.EmcSleep();
                    Thread.Sleep(1000);
                }
                else
                {
                    var mode = cmbEmcModeLin1.Text.Replace("Mode", string.Empty);
                    _u557HeadLamplLin1.EmcMode(mode);
                    Thread.Sleep(500);
                }
            });

            gpEmcLin1.Enabled = true;
        }

        private void CmbEmcModeLin1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!swMode5Lin1.Active)
            {
                var mode = cmbEmcModeLin1.Text.Replace("Mode", string.Empty);
                _u557HeadLamplLin1.EmcMode(mode);
            }
        }

        private async void SwMode5Lin2_ActiveChanged(object sender, EventArgs e)
        {
            gpEmcLin2.Enabled = false;
            cmbEmcModeLin2.Enabled = !swMode5Lin2.Active;

            await Task.Run(() =>
            {
                if (swMode5Lin2.Active)
                {
                    _u557HeadLamplLin2.EmcSleep();
                    Thread.Sleep(1000);
                }
                else
                {
                    var mode = cmbEmcModeLin2.Text.Replace("Mode", string.Empty);
                    _u557HeadLamplLin2.EmcMode(mode);
                    Thread.Sleep(500);
                }
            });

            gpEmcLin2.Enabled = true;
        }

        private void CmbEmcModeLin2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!swMode5Lin2.Active)
            {
                var mode = cmbEmcModeLin2.Text.Replace("Mode", string.Empty);
                _u557HeadLamplLin2.EmcMode(mode);
            }
        }
    }
}
