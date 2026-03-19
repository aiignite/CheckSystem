using Controller;
using Sunny.UI;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckSystem.HelperForms.U557HeadLamp
{
    public partial class FrmU557Emc : UIForm
    {
        private H77RearLamp _u557HeadLamp = new H77RearLamp("U557_EMC");
        //private SyRenesasMcuControllerMaster _syRenesasMcuControllerMaster = new SyRenesasMcuControllerMaster("LIN_Transfer");
        private SyControllerWith56Pin _syRenesasMcuControllerMaster = new SyControllerWith56Pin("LIN_Transfer");

        /// <summary>
        /// 当前窗体的宽度
        /// </summary>
        private float _x;

        /// <summary>
        /// 当前窗体的高度
        /// </summary>
        private float _y;

        public FrmU557Emc()
        {
            InitializeComponent();
            Load += FrmU557Emc_Load;
            Closed += FrmU557Emc_Closed;
        }

        private void FrmU557Emc_Closed(object sender, EventArgs e)
        {
            _u557HeadLamp.Dispose();
            _syRenesasMcuControllerMaster.Dispose();
        }

        private void FrmU557Emc_Load(object sender, EventArgs e)
        {
            #region UI缩放
            _x = Width;//获取窗体的宽度
            _y = Height;//获取窗体的高度
            SetTag(this);//调用方法
            Resize += FrmU557Emc_Resize;

            Width = (int)(Screen.PrimaryScreen.Bounds.Width * 0.75);
            Height = (int)(Screen.PrimaryScreen.Bounds.Height * 0.75);
            Location = new Point((int)(Screen.PrimaryScreen.Bounds.Width * 0.1),
                (int)(Screen.PrimaryScreen.Bounds.Height * 0.1));
            #endregion

            try
            {
                _syRenesasMcuControllerMaster.InitRemoteIpAddress("192.168.1.28:8088");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }

            #region LIN通道选择
            var option = new UIEditOption { AutoLabelWidth = true, Text = @"选择LIN通道" };
            var prMode = new[] { "LIN1", "LIN2" };
            option.AddCombobox("LinChannel", "LIN通道：", prMode, 0, true, true);

            var frm = new UIEditForm(option);
            frm.Render();
            frm.ShowDialog();

            var ch = 1;
            if (!frm.IsOK)
                ShowInfoTip("取消选择,默认通道LIN1");
            else
            {
                ch = ((int)frm["LinChannel"]) + 1;
                ShowSuccessTip(string.Format("当现选择通道: LIN{0}", ch));
            }

            Text += string.Format(" (当现选择通道: LIN{0})", ch);

            if (ch == 1)
            {
                //_u557HeadLamp.LinWithBaudRate10417 = _syRenesasMcuControllerMaster.GatewayLin1;
                _u557HeadLamp.LinWithBaudRate10417 = _syRenesasMcuControllerMaster.GatewayLin;
            }
            else if (ch == 2)
            {
                //_u557HeadLamp.LinWithBaudRate10417 = _syRenesasMcuControllerMaster.GatewayLin2;
            }

            #endregion

            #region 控件事件
            RegisterEvent();
            #endregion

            #region 界面初始化
            InitControl();
            #endregion
        }

        #region UI缩放


        private void FrmU557Emc_Resize(object sender, EventArgs e)
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
                    var myTag = con.Tag.ToString().Split(new char[] { ':' });//获取控件的Tag属性值，并分割后存储字符串数组
                    var a = Convert.ToSingle(myTag[0]) * newX;//根据窗体缩放比例确定控件的值，宽度
                    con.Width = (int)a;//宽度
                    a = Convert.ToSingle(myTag[1]) * newY;//高度
                    con.Height = (int)(a);
                    a = Convert.ToSingle(myTag[2]) * newX;//左边距离
                    con.Left = (int)(a);
                    a = Convert.ToSingle(myTag[3]) * newY;//上边缘距离
                    con.Top = (int)(a);
                    var currentSize = Convert.ToSingle(myTag[4]) * newY;//字体大小
                    con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                    if (con.Controls.Count > 0)
                        SetControls(newX, newY, con);
                }

            }
        }

        #endregion

        #region 控件事件

        private void RegisterEvent()
        {
            swLinScheduler.ActiveChanged += SwLinScheduler_ActiveChanged;
            swMode5.ActiveChanged += SwMode5_ActiveChanged;
            swMode5.ActiveChanging += SwMode5_ActiveChanging;

            swParkLamp1_2sOnOff.ActiveChanged += SwParkLamp1_2sOnOff_ActiveChanged;
            swParkLamp7_2sOnOff.ActiveChanged += SwParkLamp7_2sOnOff_ActiveChanged;

            swTurnLamp1_400MsOnOff.ActiveChanged += SwTurnLamp1_400MsOnOff_ActiveChanged;
            swTurnLamp2_400MsOnOff.ActiveChanged += SwTurnLamp2_400MsOnOff_ActiveChanged;
            swTurnLamp3_400MsOnOff.ActiveChanged += SwTurnLamp3_400MsOnOff_ActiveChanged;
            swTurnLamp4_400MsOnOff.ActiveChanged += SwTurnLamp4_400MsOnOff_ActiveChanged;

            swLmpShwTyp2Mod3.ActiveChanged += SwLmpShwTyp2Mod3_ActiveChanged;
            swLmpShwTyp2Mod4.ActiveChanged += SwLmpShwTyp2Mod4_ActiveChanged;
            swLmpShwTyp3Mod3.ActiveChanged += SwLmpShwTyp3Mod3_ActiveChanged;
            swLmpShwTyp3Mod4.ActiveChanged += SwLmpShwTyp3Mod4_ActiveChanged;

            cmbLeftParkLamp.SelectedIndexChanged += CmbLeftParkLamp_SelectedIndexChanged;
            cmbLeftTurnLamp.SelectedIndexChanged += CmbLeftTurnLamp_SelectedIndexChanged;
            cmbRightParkLamp.SelectedIndexChanged += CmbRightParkLamp_SelectedIndexChanged;
            cmbRightTurnLamp.SelectedIndexChanged += CmbRightTurnLamp_SelectedIndexChanged;

            cmbRrLmpShwTyp_64_5.SelectedIndexChanged += CmbRrLmpShwTyp_64_5_SelectedIndexChanged;
            cmbRrLmpShwMod_64_5.SelectedIndexChanged += CmbRrLmpShwMod_64_5_SelectedIndexChanged;
            cmbEmcMode.SelectedIndexChanged += CmbEmcMode_SelectedIndexChanged;

            cmbLeftStop.SelectedIndexChanged += CmbLeftStop_SelectedIndexChanged;
            cmbRightStop.SelectedIndexChanged += CmbRightStop_SelectedIndexChanged;
            cmbLogo.SelectedIndexChanged += CmbLogo_SelectedIndexChanged;
        }

        private void CmbLogo_SelectedIndexChanged(object sender, EventArgs e)
        {
            _u557HeadLamp.LogoOn(cmbLogo.SelectedIndex);
        }

        private void CmbRightStop_SelectedIndexChanged(object sender, EventArgs e)
        {
            _u557HeadLamp.RightStopOn(cmbRightStop.SelectedIndex);
        }

        private void CmbLeftStop_SelectedIndexChanged(object sender, EventArgs e)
        {
            _u557HeadLamp.LeftStopOn(cmbLeftStop.SelectedIndex);
        }

        private void InitControl()
        {
            swLinScheduler.Active = true;

            cmbEmcMode.SelectedIndex = 0;
            cmbRrLmpShwMod_64_5.SelectedIndex = 0;
            cmbRrLmpShwTyp_64_5.SelectedIndex = 0;

            cmbLeftParkLamp.SelectedIndex = 0;
            cmbLeftTurnLamp.SelectedIndex = 0;
            cmbRightParkLamp.SelectedIndex = 0;
            cmbRightTurnLamp.SelectedIndex = 0;

            cmbLeftStop.SelectedIndex = 0;
            cmbRightStop.SelectedIndex = 0;
            cmbLogo.SelectedIndex = 0;
        }

        private void SwLinScheduler_ActiveChanged(object sender, EventArgs e)
        {
            groupBox2.Enabled = swLinScheduler.Active;
            groupBox3.Enabled = swLinScheduler.Active;
            groupBox4.Enabled = swLinScheduler.Active;
            groupBox5.Enabled = swLinScheduler.Active;
            groupBox6.Enabled = swLinScheduler.Active;
            //groupBox7.Enabled = swLinScheduler.Active;
            groupBox8.Enabled = swLinScheduler.Active;

            if (swLinScheduler.Active)
                _u557HeadLamp.LinStartScheduler();
            else
                _u557HeadLamp.LinStopScheduler();
        }

        private void CmbRrLmpShwMod_64_5_SelectedIndexChanged(object sender, EventArgs e)
        {
            _u557HeadLamp.RrLmpShwMod_64_5(cmbRrLmpShwMod_64_5.SelectedIndex.ToString());
        }

        private void CmbRrLmpShwTyp_64_5_SelectedIndexChanged(object sender, EventArgs e)
        {
            _u557HeadLamp.RrLmpShwTyp_64_5(cmbRrLmpShwTyp_64_5.SelectedIndex.ToString());
        }

        private void CmbEmcMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!swMode5.Active)
            {
                var mode = cmbEmcMode.Text.Replace("Mode", string.Empty);
                _u557HeadLamp.EmcMode(mode);
            }
        }

        private async void SwMode5_ActiveChanged(object sender, EventArgs e)
        {
            Enabled = false;
            cmbEmcMode.Enabled = !swMode5.Active;

            await Task.Run(() =>
            {
                if (swMode5.Active)
                {
                    _u557HeadLamp.EmcSleep();
                    Thread.Sleep(1000);
                }
                else
                {
                    var mode = cmbEmcMode.Text.Replace("Mode", string.Empty);
                    _u557HeadLamp.EmcMode(mode);
                    Thread.Sleep(500);
                }
            });

            Enabled = true;
        }

        private void SwMode5_ActiveChanging(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Enabled = false;

            //cmbEmcMode.Enabled = swMode5.Active;
            //if (!swMode5.Active)
            //{
            //    _u557HeadLamp.EmcSleep();
            //    Thread.Sleep(2000);
            //}
            //else
            //{
            //    var mode = cmbEmcMode.Text.Replace("Mode", string.Empty);
            //    _u557HeadLamp.EmcMode(mode);
            //    Thread.Sleep(2000);
            //}

            //Enabled = true;
        }

        private void CmbRightTurnLamp_SelectedIndexChanged(object sender, EventArgs e)
        {
            _u557HeadLamp.RightTurnOn(cmbRightTurnLamp.SelectedIndex);

            //switch (cmbRightTurnLamp.Text)
            //{
            //    case "0":
            //        _u557HeadLamp.RightTurnOff();
            //        break;

            //    case "1":
            //        _u557HeadLamp.RightTurnLamp1();
            //        break;

            //    case "2":
            //        _u557HeadLamp.RightTurnLamp2();
            //        break;

            //    case "3":
            //        _u557HeadLamp.RightTurnLamp3();
            //        break;

            //    case "4":
            //        _u557HeadLamp.RightTurnLamp4();
            //        break;
            //}
        }

        private void CmbRightParkLamp_SelectedIndexChanged(object sender, EventArgs e)
        {
            _u557HeadLamp.RightParkLampOn(cmbRightParkLamp.SelectedIndex);

            //switch (cmbRightParkLamp.Text)
            //{
            //    case "0":
            //        _u557HeadLamp.RightParkLampOff();
            //        break;

            //    case "1":
            //        _u557HeadLamp.RightParkLamp1();
            //        break;

            //    case "7":
            //        _u557HeadLamp.RightParkLamp7();
            //        break;
            //}
        }

        private void CmbLeftTurnLamp_SelectedIndexChanged(object sender, EventArgs e)
        {
            _u557HeadLamp.LeftTurnOn(cmbLeftTurnLamp.SelectedIndex);

            //switch (cmbLeftTurnLamp.Text)
            //{
            //    case "0":
            //        _u557HeadLamp.LeftTurnOff();
            //        break;

            //    case "1":
            //        _u557HeadLamp.LeftTurnLamp1();
            //        break;

            //    case "2":
            //        _u557HeadLamp.LeftTurnLamp2();
            //        break;

            //    case "3":
            //        _u557HeadLamp.LeftTurnLamp3();
            //        break;

            //    case "4":
            //        _u557HeadLamp.LeftTurnLamp4();
            //        break;
            //}
        }

        private void CmbLeftParkLamp_SelectedIndexChanged(object sender, EventArgs e)
        {
            _u557HeadLamp.LeftParkLampOn(cmbLeftParkLamp.SelectedIndex);

            //switch (cmbLeftParkLamp.Text)
            //{
            //    case "0":
            //        _u557HeadLamp.LeftParkLampOff();
            //        break;

            //    case "1":
            //        _u557HeadLamp.LeftParkLamp1();
            //        break;

            //    case "7":
            //        _u557HeadLamp.LeftParkLamp7();
            //        break;
            //}
        }

        #endregion

        #region 位置灯压力测试定时器

        private void SwParkLamp7_2sOnOff_ActiveChanged(object sender, EventArgs e)
        {
            if (swParkLamp7_2sOnOff.Active)
            {
                cmbLeftParkLamp.SelectedIndex = 7;
                cmbRightParkLamp.SelectedIndex = 7;
                cmbLeftParkLamp.Enabled = false;
                cmbRightParkLamp.Enabled = false;

                swParkLamp1_2sOnOff.Enabled = false;

                timerParkLamp2SOnOff.ReStart();
            }
            else
            {
                cmbLeftParkLamp.SelectedIndex = 0;
                cmbRightParkLamp.SelectedIndex = 0;
                cmbLeftParkLamp.Enabled = true;
                cmbRightParkLamp.Enabled = true;

                swParkLamp1_2sOnOff.Enabled = true;

                timerParkLamp2SOnOff.Stop();
            }
        }

        private void SwParkLamp1_2sOnOff_ActiveChanged(object sender, EventArgs e)
        {
            if (swParkLamp1_2sOnOff.Active)
            {
                cmbLeftParkLamp.SelectedIndex = 1;
                cmbRightParkLamp.SelectedIndex = 1;
                cmbLeftParkLamp.Enabled = false;
                cmbRightParkLamp.Enabled = false;

                swParkLamp7_2sOnOff.Enabled = false;

                timerParkLamp2SOnOff.ReStart();
            }
            else
            {
                cmbLeftParkLamp.SelectedIndex = 0;
                cmbRightParkLamp.SelectedIndex = 0;
                cmbLeftParkLamp.Enabled = true;
                cmbRightParkLamp.Enabled = true;

                swParkLamp7_2sOnOff.Enabled = true;

                timerParkLamp2SOnOff.Stop();
            }
        }

        private void timerParkLamp2SOnOff_Tick(object sender, EventArgs e)
        {
            if (swParkLamp1_2sOnOff.Active)
            {
                cmbLeftParkLamp.SelectedIndex = cmbLeftParkLamp.SelectedIndex == 0 ? 1 : 0;
                cmbRightParkLamp.SelectedIndex = cmbRightParkLamp.SelectedIndex == 0 ? 1 : 0;
            }
            else if (swParkLamp7_2sOnOff.Active)
            {
                cmbLeftParkLamp.SelectedIndex = cmbLeftParkLamp.SelectedIndex == 0 ? 2 : 0;
                cmbRightParkLamp.SelectedIndex = cmbRightParkLamp.SelectedIndex == 0 ? 2 : 0;
            }
        }

        #endregion

        #region 转向灯压力测试定时器

        private void SwTurnLamp4_400MsOnOff_ActiveChanged(object sender, EventArgs e)
        {
            if (swTurnLamp4_400MsOnOff.Active)
            {
                cmbLeftTurnLamp.SelectedIndex = 4;
                cmbRightTurnLamp.SelectedIndex = 4;
                cmbLeftTurnLamp.Enabled = false;
                cmbRightTurnLamp.Enabled = false;

                swTurnLamp2_400MsOnOff.Enabled = false;
                swTurnLamp3_400MsOnOff.Enabled = false;
                swTurnLamp1_400MsOnOff.Enabled = false;

                timerTurnLamp400MsOnOff.ReStart();
            }
            else
            {
                cmbLeftTurnLamp.SelectedIndex = 0;
                cmbRightTurnLamp.SelectedIndex = 0;
                cmbLeftTurnLamp.Enabled = true;
                cmbRightTurnLamp.Enabled = true;

                swTurnLamp2_400MsOnOff.Enabled = true;
                swTurnLamp3_400MsOnOff.Enabled = true;
                swTurnLamp1_400MsOnOff.Enabled = true;

                timerTurnLamp400MsOnOff.Stop();
            }
        }

        private void SwTurnLamp3_400MsOnOff_ActiveChanged(object sender, EventArgs e)
        {
            if (swTurnLamp3_400MsOnOff.Active)
            {
                cmbLeftTurnLamp.SelectedIndex = 3;
                cmbRightTurnLamp.SelectedIndex = 3;
                cmbLeftTurnLamp.Enabled = false;
                cmbRightTurnLamp.Enabled = false;

                swTurnLamp2_400MsOnOff.Enabled = false;
                swTurnLamp1_400MsOnOff.Enabled = false;
                swTurnLamp4_400MsOnOff.Enabled = false;

                timerTurnLamp400MsOnOff.ReStart();
            }
            else
            {
                cmbLeftTurnLamp.SelectedIndex = 0;
                cmbRightTurnLamp.SelectedIndex = 0;
                cmbLeftTurnLamp.Enabled = true;
                cmbRightTurnLamp.Enabled = true;

                swTurnLamp2_400MsOnOff.Enabled = true;
                swTurnLamp1_400MsOnOff.Enabled = true;
                swTurnLamp4_400MsOnOff.Enabled = true;

                timerTurnLamp400MsOnOff.Stop();
            }
        }

        private void SwTurnLamp2_400MsOnOff_ActiveChanged(object sender, EventArgs e)
        {
            if (swTurnLamp2_400MsOnOff.Active)
            {
                cmbLeftTurnLamp.SelectedIndex = 2;
                cmbRightTurnLamp.SelectedIndex = 2;
                cmbLeftTurnLamp.Enabled = false;
                cmbRightTurnLamp.Enabled = false;

                swTurnLamp1_400MsOnOff.Enabled = false;
                swTurnLamp3_400MsOnOff.Enabled = false;
                swTurnLamp4_400MsOnOff.Enabled = false;

                timerTurnLamp400MsOnOff.ReStart();
            }
            else
            {
                cmbLeftTurnLamp.SelectedIndex = 0;
                cmbRightTurnLamp.SelectedIndex = 0;
                cmbLeftTurnLamp.Enabled = true;
                cmbRightTurnLamp.Enabled = true;

                swTurnLamp1_400MsOnOff.Enabled = true;
                swTurnLamp3_400MsOnOff.Enabled = true;
                swTurnLamp4_400MsOnOff.Enabled = true;

                timerTurnLamp400MsOnOff.Stop();
            }
        }

        private void SwTurnLamp1_400MsOnOff_ActiveChanged(object sender, EventArgs e)
        {
            if (swTurnLamp1_400MsOnOff.Active)
            {
                cmbLeftTurnLamp.SelectedIndex = 1;
                cmbRightTurnLamp.SelectedIndex = 1;
                cmbLeftTurnLamp.Enabled = false;
                cmbRightTurnLamp.Enabled = false;

                swTurnLamp2_400MsOnOff.Enabled = false;
                swTurnLamp3_400MsOnOff.Enabled = false;
                swTurnLamp4_400MsOnOff.Enabled = false;

                timerTurnLamp400MsOnOff.ReStart();
            }
            else
            {
                cmbLeftTurnLamp.SelectedIndex = 0;
                cmbRightTurnLamp.SelectedIndex = 0;
                cmbLeftTurnLamp.Enabled = true;
                cmbRightTurnLamp.Enabled = true;

                swTurnLamp2_400MsOnOff.Enabled = true;
                swTurnLamp3_400MsOnOff.Enabled = true;
                swTurnLamp4_400MsOnOff.Enabled = true;

                timerTurnLamp400MsOnOff.Stop();
            }
        }

        private void timerTurnLamp400MsOnOff_Tick(object sender, EventArgs e)
        {
            if (swTurnLamp1_400MsOnOff.Active)
            {
                cmbLeftTurnLamp.SelectedIndex = cmbLeftTurnLamp.SelectedIndex == 0 ? 1 : 0;
                cmbRightTurnLamp.SelectedIndex = cmbRightTurnLamp.SelectedIndex == 0 ? 1 : 0;
            }
            else if (swTurnLamp2_400MsOnOff.Active)
            {
                cmbLeftTurnLamp.SelectedIndex = cmbLeftTurnLamp.SelectedIndex == 0 ? 2 : 0;
                cmbRightTurnLamp.SelectedIndex = cmbRightTurnLamp.SelectedIndex == 0 ? 2 : 0;
            }
            else if (swTurnLamp3_400MsOnOff.Active)
            {
                cmbLeftTurnLamp.SelectedIndex = cmbLeftTurnLamp.SelectedIndex == 0 ? 3 : 0;
                cmbRightTurnLamp.SelectedIndex = cmbRightTurnLamp.SelectedIndex == 0 ? 3 : 0;
            }
            else if (swTurnLamp4_400MsOnOff.Active)
            {
                cmbLeftTurnLamp.SelectedIndex = cmbLeftTurnLamp.SelectedIndex == 0 ? 4 : 0;
                cmbRightTurnLamp.SelectedIndex = cmbRightTurnLamp.SelectedIndex == 0 ? 4 : 0;
            }
        }

        #endregion

        #region 灯光秀压力测试定时器

        private void SwLmpShwTyp3Mod4_ActiveChanged(object sender, EventArgs e)
        {
            if (swLmpShwTyp3Mod4.Active)
            {
                cmbRrLmpShwTyp_64_5.SelectedIndex = 3;
                cmbRrLmpShwMod_64_5.SelectedIndex = 4;
                cmbRrLmpShwTyp_64_5.Enabled = false;
                cmbRrLmpShwMod_64_5.Enabled = false;

                swLmpShwTyp2Mod3.Enabled = false;
                swLmpShwTyp2Mod4.Enabled = false;
                swLmpShwTyp3Mod3.Enabled = false;

                timerLmpShw8SOnOff.ReStart();
            }
            else
            {
                cmbRrLmpShwTyp_64_5.SelectedIndex = 0;
                cmbRrLmpShwMod_64_5.SelectedIndex = 0;
                cmbRrLmpShwTyp_64_5.Enabled = true;
                cmbRrLmpShwMod_64_5.Enabled = true;

                swLmpShwTyp2Mod3.Enabled = true;
                swLmpShwTyp2Mod4.Enabled = true;
                swLmpShwTyp3Mod3.Enabled = true;

                timerLmpShw8SOnOff.Stop();
            }
        }

        private void SwLmpShwTyp3Mod3_ActiveChanged(object sender, EventArgs e)
        {
            if (swLmpShwTyp3Mod3.Active)
            {
                cmbRrLmpShwTyp_64_5.SelectedIndex = 3;
                cmbRrLmpShwMod_64_5.SelectedIndex = 3;
                cmbRrLmpShwTyp_64_5.Enabled = false;
                cmbRrLmpShwMod_64_5.Enabled = false;

                swLmpShwTyp2Mod3.Enabled = false;
                swLmpShwTyp2Mod4.Enabled = false;
                swLmpShwTyp3Mod4.Enabled = false;

                timerLmpShw8SOnOff.ReStart();
            }
            else
            {
                cmbRrLmpShwTyp_64_5.SelectedIndex = 0;
                cmbRrLmpShwMod_64_5.SelectedIndex = 0;
                cmbRrLmpShwTyp_64_5.Enabled = true;
                cmbRrLmpShwMod_64_5.Enabled = true;

                swLmpShwTyp2Mod3.Enabled = true;
                swLmpShwTyp2Mod4.Enabled = true;
                swLmpShwTyp3Mod4.Enabled = true;

                timerLmpShw8SOnOff.Stop();
            }
        }

        private void SwLmpShwTyp2Mod4_ActiveChanged(object sender, EventArgs e)
        {
            if (swLmpShwTyp2Mod4.Active)
            {
                cmbRrLmpShwTyp_64_5.SelectedIndex = 2;
                cmbRrLmpShwMod_64_5.SelectedIndex = 4;
                cmbRrLmpShwTyp_64_5.Enabled = false;
                cmbRrLmpShwMod_64_5.Enabled = false;

                swLmpShwTyp2Mod3.Enabled = false;
                swLmpShwTyp3Mod3.Enabled = false;
                swLmpShwTyp3Mod4.Enabled = false;

                timerLmpShw8SOnOff.ReStart();
            }
            else
            {
                cmbRrLmpShwTyp_64_5.SelectedIndex = 0;
                cmbRrLmpShwMod_64_5.SelectedIndex = 0;
                cmbRrLmpShwTyp_64_5.Enabled = true;
                cmbRrLmpShwMod_64_5.Enabled = true;

                swLmpShwTyp2Mod3.Enabled = true;
                swLmpShwTyp3Mod3.Enabled = true;
                swLmpShwTyp3Mod4.Enabled = true;

                timerLmpShw8SOnOff.Stop();
            }
        }

        private void SwLmpShwTyp2Mod3_ActiveChanged(object sender, EventArgs e)
        {
            if (swLmpShwTyp2Mod3.Active)
            {
                cmbRrLmpShwTyp_64_5.SelectedIndex = 4;
                cmbRrLmpShwMod_64_5.SelectedIndex = 8;
                cmbRrLmpShwTyp_64_5.Enabled = false;
                cmbRrLmpShwMod_64_5.Enabled = false;

                swLmpShwTyp2Mod4.Enabled = false;
                swLmpShwTyp3Mod3.Enabled = false;
                swLmpShwTyp3Mod4.Enabled = false;

                _isTy4Mod8On = true;
                timerLmpShw8SOnOff.Stop();
                timerLmpShwTyp4Mod83SOff.Stop();
                timerLmpShwTyp4Mod815SOn.Start();
            }
            else
            {
                cmbRrLmpShwTyp_64_5.SelectedIndex = 0;
                cmbRrLmpShwMod_64_5.SelectedIndex = 0;
                cmbRrLmpShwTyp_64_5.Enabled = true;
                cmbRrLmpShwMod_64_5.Enabled = true;

                swLmpShwTyp2Mod4.Enabled = true;
                swLmpShwTyp3Mod3.Enabled = true;
                swLmpShwTyp3Mod4.Enabled = true;

                _isTy4Mod8On = false;
                timerLmpShw8SOnOff.Stop();
                timerLmpShwTyp4Mod815SOn.Stop();
            }
        }

        private void timerLmpShw8SOnOff_Tick(object sender, EventArgs e)
        {
            if (swLmpShwTyp2Mod4.Active)
            {
                cmbRrLmpShwTyp_64_5.SelectedIndex = cmbRrLmpShwTyp_64_5.SelectedIndex == 0 ? 2 : 0;
                cmbRrLmpShwMod_64_5.SelectedIndex = cmbRrLmpShwMod_64_5.SelectedIndex == 0 ? 4 : 0;
            }
            else if (swLmpShwTyp3Mod3.Active)
            {
                cmbRrLmpShwTyp_64_5.SelectedIndex = cmbRrLmpShwTyp_64_5.SelectedIndex == 0 ? 3 : 0;
                cmbRrLmpShwMod_64_5.SelectedIndex = cmbRrLmpShwMod_64_5.SelectedIndex == 0 ? 3 : 0;
            }
            else if (swLmpShwTyp3Mod4.Active)
            {
                cmbRrLmpShwTyp_64_5.SelectedIndex = cmbRrLmpShwTyp_64_5.SelectedIndex == 0 ? 3 : 0;
                cmbRrLmpShwMod_64_5.SelectedIndex = cmbRrLmpShwMod_64_5.SelectedIndex == 0 ? 4 : 0;
            }
        }

        private bool _isTy4Mod8On;

        private void timerLmpShwTyp4Mod815SOn_Tick(object sender, EventArgs e)
        {
            if (swLmpShwTyp2Mod3.Active)
            {
                cmbRrLmpShwTyp_64_5.SelectedIndex = 0;
                cmbRrLmpShwMod_64_5.SelectedIndex = 0;
                _isTy4Mod8On = false;
                timerLmpShwTyp4Mod815SOn.Stop();
                timerLmpShwTyp4Mod83SOff.Start();
            }
        }

        private void timerLmpShwTyp4Mod83SOff_Tick(object sender, EventArgs e)
        {
            if (swLmpShwTyp2Mod3.Active)
            {
                cmbRrLmpShwTyp_64_5.SelectedIndex = 4;
                cmbRrLmpShwMod_64_5.SelectedIndex = 8;
                _isTy4Mod8On = true;
                timerLmpShwTyp4Mod815SOn.Start();
                timerLmpShwTyp4Mod83SOff.Stop();
            }
        }

        #endregion

        #region 休眠唤醒

        private void btnCorrectSleepCmd_Click(object sender, EventArgs e)
        {
            if (ShowAskDialog("注: 测试休眠指令需先关闭LIN开关"))
            {
                _u557HeadLamp.SendCorrectSleepCmd();
                ShowSuccessTip("已发送正确休眠指令");
            }
            else
            {
                ShowErrorTip("操作取消");
            }
        }

        private void btnIncorrectSleepCmd_Click(object sender, EventArgs e)
        {
            if (ShowAskDialog("注: 测试休眠指令需先关闭LIN开关"))
            {
                _u557HeadLamp.SendIncorrectSleepCmd();
                ShowSuccessTip("已发送错误休眠指令");
            }
            else
            {
                ShowErrorTip("操作取消");
            }
        }

        private void btnLeftReset_Click(object sender, EventArgs e)
        {
            _u557HeadLamp.LeftReset();
            ShowSuccessTip("已发送左复位指令");
        }

        private void btnRightReset_Click(object sender, EventArgs e)
        {
            _u557HeadLamp.RightReset();
            ShowSuccessTip("已发送右复位指令");
        }

        #endregion

        private void btnLeftBoot_Click(object sender, EventArgs e)
        {
            txtLeftBoot.Text = string.Empty;
            _u557HeadLamp.ReadLeftBoot();
            txtLeftBoot.Text = _u557HeadLamp.LeftBoot;
        }

        private void btnLeftApp_Click(object sender, EventArgs e)
        {
            txtLeftApp.Text = string.Empty;
            _u557HeadLamp.ReadLeftApp();
            txtLeftApp.Text = _u557HeadLamp.LeftApp;
        }

        private void btnRightBoot_Click(object sender, EventArgs e)
        {
            txtRightBoot.Text = string.Empty;
            _u557HeadLamp.ReadRightBoot();
            txtRightBoot.Text = _u557HeadLamp.RightBoot;
        }

        private void btnRightApp_Click(object sender, EventArgs e)
        {
            txtRightApp.Text = string.Empty;
            _u557HeadLamp.ReadRightApp();
            txtRightApp.Text = _u557HeadLamp.RightApp;
        }
    }
}
