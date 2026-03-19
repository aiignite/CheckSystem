using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonUtility.BusLoader;
using Controller;
using Sunny.UI;

namespace CheckSystem.HelperForms.VW323RearLamp
{
    public sealed partial class Vw323RearLamp0Cs : UIForm
    {
        private float _x;//当前窗体的宽度
        private float _y;//当前窗体的高度

        private ControllerBase LinController { get; set; }
        //private readonly Controller.Vw323RearLamp _vw323RearLamp = new Controller.Vw323RearLamp("VW323");
        private Thread LinMsgThread { get; set; }
        public bool IsSleep;
        private readonly LinCommunicationMatrix.IntelMatrix _intelMatrix =
            new LinCommunicationMatrix.IntelMatrix(0x0A, 8);
        private readonly Dictionary<string, MatrixValDefinition> _matrixValDefinitions =
           new Dictionary<string, MatrixValDefinition>();

        private readonly string BCM2_SBBR_01_BZ = "BCM2_SBBR_01_BZ";

        private readonly string SBBR_Schlusslicht_Anf_Inv = "SBBR_Schlusslicht_Anf_Inv";
        private readonly string SBBR_Schlusslicht_Anf = "SBBR_Schlusslicht_Anf";
        private readonly string ZV_HD_offen = "ZV_HD_offen";
        private readonly string SBBR_FRA_WiBi_links = "SBBR_FRA_WiBi_links";
        private readonly string SBBR_FRA_links = "SBBR_FRA_links";
        private readonly string SBBR_FRA_WiBi_rechts = "SBBR_FRA_WiBi_rechts";
        private readonly string SBBR_FRA_rechts = "SBBR_FRA_rechts";

        private readonly string ZAS_Kl_15 = "ZAS_Kl_15";
        private readonly string ZAS_Kl_15_inv = "ZAS_Kl_15_inv";
        private readonly string SBBR_Parklicht_li_Anf = "SBBR_Parklicht_li_Anf";
        private readonly string SBBR_Parklicht_re_Anf = "SBBR_Parklicht_re_Anf";
        private readonly string SBBR_BRL_Anf = "SBBR_BRL_Anf";
        private readonly string SBBR_Nebelschluss_Fzg_Anf = "SBBR_Nebelschluss_Fzg_Anf";
        private readonly string SBBR_Rueckfahrlicht_Anf = "SBBR_Rueckfahrlicht_Anf";
        private readonly string SBBR_CH_LH_Animation = "SBBR_CH_LH_Animation";
        private readonly string BCM1_CH_aktiv = "BCM1_CH_aktiv";
        private readonly string BCM1_LH_aktiv = "BCM1_LH_aktiv";

        private readonly string SBBR_FRA_WiBi_Animation = "SBBR_FRA_WiBi_Animation";

        private readonly string SBBR_Helligkeit_Hoch = "SBBR_Helligkeit_Hoch";
        private readonly string SBBR_Helligkeit_Mitte = "SBBR_Helligkeit_Mitte";
        private readonly string SBBR_Helligkeit_Dunkel = "SBBR_Helligkeit_Dunkel";

        public Vw323RearLamp0Cs(bool isToomoss)
        {
            InitializeComponent();
            //Style = UIStyle.Black;

            if (!isToomoss)
                LinController = new SyControllerWith56Pin("56PIN");
            else
            {
                LinController = new ToomossUsb2XxxCanLin("Tomoss");
                //(LinController as ToomossUsb2XxxCanLin).CheckType = 0;
                (LinController as ToomossUsb2XxxCanLin).SetBaudRate("1", "19200");
                Text += @" (图莫斯)";
            }

            _matrixValDefinitions.Add(SBBR_FRA_WiBi_links, new MatrixValDefinition(16, 1, 0));
            _matrixValDefinitions.Add(SBBR_FRA_WiBi_rechts, new MatrixValDefinition(17, 1, 0));
            _matrixValDefinitions.Add(SBBR_FRA_links, new MatrixValDefinition(20, 1, 0));
            _matrixValDefinitions.Add(SBBR_FRA_rechts, new MatrixValDefinition(21, 1, 0));
            _matrixValDefinitions.Add(BCM1_CH_aktiv, new MatrixValDefinition(24, 1, 0));
            _matrixValDefinitions.Add(BCM1_LH_aktiv, new MatrixValDefinition(25, 1, 0));
            _matrixValDefinitions.Add(SBBR_CH_LH_Animation, new MatrixValDefinition(30, 2, 0));
            _matrixValDefinitions.Add(SBBR_BRL_Anf, new MatrixValDefinition(32, 1, 0));
            _matrixValDefinitions.Add(SBBR_Schlusslicht_Anf, new MatrixValDefinition(49, 1, 0));
            _matrixValDefinitions.Add(SBBR_Schlusslicht_Anf_Inv, new MatrixValDefinition(48, 1, 1));
            _matrixValDefinitions.Add(SBBR_Parklicht_li_Anf, new MatrixValDefinition(50, 1, 0));
            _matrixValDefinitions.Add(SBBR_Parklicht_re_Anf, new MatrixValDefinition(51, 1, 0));
            _matrixValDefinitions.Add(SBBR_Rueckfahrlicht_Anf, new MatrixValDefinition(52, 1, 0));
            _matrixValDefinitions.Add(SBBR_Nebelschluss_Fzg_Anf, new MatrixValDefinition(53, 1, 0));

            _matrixValDefinitions.Add(SBBR_FRA_WiBi_Animation, new MatrixValDefinition(22, 2, 0));

            _matrixValDefinitions.Add(ZV_HD_offen, new MatrixValDefinition(14, 1, 0));
            _matrixValDefinitions.Add(ZAS_Kl_15, new MatrixValDefinition(12, 1, 1));
            _matrixValDefinitions.Add(ZAS_Kl_15_inv, new MatrixValDefinition(15, 1, 0));

            _matrixValDefinitions.Add(BCM2_SBBR_01_BZ, new MatrixValDefinition(8, 4, 0));

            _matrixValDefinitions.Add(SBBR_Helligkeit_Hoch, new MatrixValDefinition(40, 1, 0));
            _matrixValDefinitions.Add(SBBR_Helligkeit_Mitte, new MatrixValDefinition(41, 1, 0));
            _matrixValDefinitions.Add(SBBR_Helligkeit_Dunkel, new MatrixValDefinition(42, 1, 0));

            cmbFRABoff.Items.Add("A & B active");
            cmbFRABoff.Items.Add("Only A active");
            cmbFRABoff.SelectedIndex = 0;
            cmbFRABoff.SelectedIndexChanged += cmbFRABoff_SelectedIndexChanged;

            cmbChLhAnimation.Items.Add("Default_CH_LH_Animation");
            cmbChLhAnimation.Items.Add("CH_LH_Animation_1");
            cmbChLhAnimation.Items.Add("CH_LH_Animation_2");
            cmbChLhAnimation.Items.Add("CH_LH_Animation_3");
            cmbChLhAnimation.SelectedIndex = 0;
            cmbChLhAnimation.SelectedIndex = 0;

            cmbFRAWiBiAnimation.Items.Add("Default_Animation_WiBi");
            cmbFRAWiBiAnimation.Items.Add("WiBi_Animation_1");
            cmbFRAWiBiAnimation.Items.Add("WiBi_Animation_2");
            cmbFRAWiBiAnimation.Items.Add("WiBi_Animation_3");
            cmbFRAWiBiAnimation.SelectedIndex = 0;
            cmbFRAWiBiAnimation.SelectedIndexChanged += cmbFRAWiBiAnimation_SelectedIndexChanged;
        }

        private void FrmStaticVisionCali_Resize(object sender, EventArgs e)
        {
            var newX = (this.Width) / _x; //窗体宽度缩放比例
            var newY = (this.Height) / _y;//窗体高度缩放比例
            SetControls(newX, newY, this);//随窗体改变控件大小
        }

        private void Vw323RearLamp0Cs_Load(object sender, EventArgs e)
        {
            _x = this.Width;//获取窗体的宽度
            _y = this.Height;//获取窗体的高度
            SetTag(this);//调用方法

            Resize += FrmStaticVisionCali_Resize;

            try
            {
                if (LinController == null)
                    return;

                var with56Pin = LinController as SyControllerWith56Pin;
                if (with56Pin != null)
                {
                    var pin = with56Pin;
                    pin.InitRemoteIpAddress("192.168.1.28:8088");
                }
                else
                {
                    var lin = LinController as ToomossUsb2XxxCanLin;
                    if (lin != null)
                    {
                        var pin = lin;
                        pin.SetBaudRate(1.ToString(), 19200.ToString());
                    }
                }

                //_vw323RearLamp.Lin19200 = _linController.GatewayLin;
                //_vw323RearLamp.LampAwake();

                if (LinMsgThread != null)
                {
                    LinMsgThread.Abort();
                    LinMsgThread.Join();
                }

                LinMsgThread = new Thread(MainWork) { IsBackground = true };
                LinMsgThread.Start();
            }
            catch (Exception ex)
            {

                this.ShowErrorDialog(ex.Message);
            }
        }

        /// 将控件的宽，高，左边距，顶边距和字体大小暂存到tag属性中
        /// </summary>
        /// <param name="cons">递归控件中的控件</param>
        private static void SetTag(System.Windows.Forms.Control cons)
        {
            foreach (System.Windows.Forms.Control con in cons.Controls)//循环窗体中的控件
            {
                con.Tag = con.Width + ":" + con.Height + ":" + con.Left + ":" + con.Top + ":" + con.Font.Size;
                if (con.Controls.Count > 0)
                    SetTag(con);
            }
        }

        /// <summary>
        /// 根据窗体大小调整控件大小
        /// </summary>
        /// <param name="newx">窗体宽度缩放比例</param>
        /// <param name="newy">窗体高度缩放比例</param>
        /// <param name="cons">随窗体改变控件大小</param>
        private static void SetControls(float newx, float newy, System.Windows.Forms.Control cons)
        {
            //遍历窗体中的控件，重新设置控件的值
            foreach (System.Windows.Forms.Control con in cons.Controls)
            {
                var mytag = con.Tag.ToString().Split(new char[] { ':' });//获取控件的Tag属性值，并分割后存储字符串数组
                var a = System.Convert.ToSingle(mytag[0]) * newx;//根据窗体缩放比例确定控件的值，宽度
                con.Width = (int)a;//宽度
                a = System.Convert.ToSingle(mytag[1]) * newy;//高度
                con.Height = (int)(a);
                a = System.Convert.ToSingle(mytag[2]) * newx;//左边距离
                con.Left = (int)(a);
                a = System.Convert.ToSingle(mytag[3]) * newy;//上边缘距离
                con.Top = (int)(a);
                var currentSize = System.Convert.ToSingle(mytag[4]) * newy;//字体大小
                con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                if (con.Controls.Count > 0)
                    SetControls(newx, newy, con);
            }
        }

        private void Vw323RearLamp0Cs_Closed(object sender, EventArgs e)
        {
            if (LinMsgThread != null)
            {
                LinMsgThread.Abort();
                LinMsgThread.Join();
            }

            if (LinController != null)
                LinController.Dispose();
        }

        private bool _isTurnSwitchOnOff;
        private int _isTurnOnOffCount;

        private int _bzCount;

        private void MainWork()
        {
            while (LinMsgThread.IsAlive)
            {
                if (!LinMsgThread.IsAlive)
                    break;

                Thread.Sleep(20);

                LinBus linBus = null;

                if (LinController is SyControllerWith56Pin)
                {
                    var syControllerWith56Pin = LinController as SyControllerWith56Pin;
                    if (syControllerWith56Pin.GatewayLin == null)
                        continue;
                    linBus = syControllerWith56Pin.GatewayLin;
                }
                else if (LinController is ToomossUsb2XxxCanLin)
                {
                    var tomoss = LinController as ToomossUsb2XxxCanLin;
                    if (tomoss.Lin1 == null)
                        continue;
                    linBus = tomoss.Lin1;
                }

                if (linBus == null)
                    continue;

                if (IsSleep)
                {
                    _bzCount = 0;
                    _isTurnOnOffCount = 0;
                    continue;
                }

                if (_isTurnSwitchOnOff)
                {
                    _isTurnOnOffCount++;

                    if (uiSwitchTurnL.Active)
                    {
                        if (_isTurnOnOffCount == 20)
                        {
                            _matrixValDefinitions[SBBR_FRA_links].Value = 0x00;
                        }
                        else if (_isTurnOnOffCount == 40)
                        {
                            _matrixValDefinitions[SBBR_FRA_links].Value = 0x01;
                        }
                    }

                    if (uiSwitchTurnWibiL.Active)
                    {
                        if (_isTurnOnOffCount == 20)
                        {
                            _matrixValDefinitions[SBBR_FRA_WiBi_links].Value = 0x00;
                        }
                        else if (_isTurnOnOffCount == 40)
                        {
                            _matrixValDefinitions[SBBR_FRA_WiBi_links].Value = 0x01;
                        }
                    }

                    if (uiSwitchTurnR.Active)
                    {
                        if (_isTurnOnOffCount == 20)
                        {
                            _matrixValDefinitions[SBBR_FRA_rechts].Value = 0x00;
                        }
                        else if (_isTurnOnOffCount == 40)
                        {
                            _matrixValDefinitions[SBBR_FRA_rechts].Value = 0x01;
                        }
                    }

                    if (uiSwitchTurnWibiR.Active)
                    {
                        if (_isTurnOnOffCount == 20)
                        {
                            _matrixValDefinitions[SBBR_FRA_WiBi_rechts].Value = 0x00;
                        }
                        else if (_isTurnOnOffCount == 40)
                        {
                            _matrixValDefinitions[SBBR_FRA_WiBi_rechts].Value = 0x01;
                        }
                    }

                    if (_isTurnOnOffCount > 40)
                    {
                        _isTurnOnOffCount = 0;
                    }
                }

                _matrixValDefinitions[BCM2_SBBR_01_BZ].Value = (byte)_bzCount;

                //if (_bzCount % 2 != 0)
                //{
                //    linBus.SendMasterLin(0x2E, new byte[] { 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00 });
                //}
                //else
                //{
                //    linBus.SendMasterLin(0x2F, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x82, 0xFF, 0xFF, 0xFF });
                //}

                _bzCount++;
                if (_bzCount == 16)
                    _bzCount = 0;

                foreach (var key in _matrixValDefinitions.Keys)
                    _intelMatrix.UpdateData(_matrixValDefinitions[key]);

                var crc = Calculate_CRC(_intelMatrix.MatrixData, 7);
                _intelMatrix.MatrixData[0] = crc;

                linBus.SendMasterLin(
                   _intelMatrix.MasterLinId, _intelMatrix.MatrixData);

                //linBus.SendMasterLin(0x0A, new byte[] { 0x00, 0x80, 0x00, 0x00, 0xE0, 0xB8, 0x0D, 0x03 });
            }
        }

        private static byte Calculate_CRC(IReadOnlyList<byte> data, int length) //Berechnet mit der 2x16 Byte Methode aus dem Lastenheft
        {
            byte byteIndex;
            byte tableIx;
            byte[] cba16Tab1 = { 0x42, 0x6d, 0x1c, 0x33, 0xfe, 0xd1, 0xa0, 0x8f, 0x15, 0x3a, 0x4b, 0x64, 0xa9, 0x86, 0xf7, 0xd8 };
            byte[] cba16Tab2 = { 0x42, 0xec, 0x31, 0x9f, 0xa4, 0x0a, 0xd7, 0x79, 0xa1, 0x0f, 0xd2, 0x7c, 0x47, 0xe9, 0x34, 0x9a };
            var bz = (byte)(data[1] & 0x0F);
            byte crc = 0xFF;
            for (byteIndex = 1; byteIndex <= length; byteIndex++)
            {
                tableIx = (byte)(data[byteIndex] ^ crc);
                crc = (byte)(cba16Tab1[tableIx & 0x0F] ^ cba16Tab2[tableIx >> 4]);

            }

            var crctable = new byte[] { 0x13, 0x33, 0x7e, 0x41, 0x09, 0xe7, 0x85, 0x46, 0xad, 0x03, 0x12, 0xd3, 0xb7, 0xb8, 0xd7, 0x58 };

            tableIx = (byte)(crctable[bz] ^ crc);
            crc = (byte)(cba16Tab1[tableIx & 0x0F] ^ cba16Tab2[tableIx >> 4]);

            crc = (byte)~crc;
            return crc;
        }

        private void cmbFRABoff_SelectedIndexChanged(object sender, EventArgs e)
        {
            _matrixValDefinitions[BCM1_CH_aktiv].Value = (byte)cmbFRABoff.SelectedIndex;
        }

        private void cmbFRAWiBiAnimation_SelectedIndexChanged(object sender, EventArgs e)
        {
            _matrixValDefinitions[SBBR_FRA_WiBi_Animation].Value = (byte)cmbFRAWiBiAnimation.SelectedIndex;
        }

        private void uiSwitchLinMsg_ValueChanged(object sender, bool value)
        {
            IsSleep = !uiSwitchLinMsg.Active;
        }

        private void uiSwitchParkL_ValueChanged(object sender, bool value)
        {
            _matrixValDefinitions[SBBR_Parklicht_li_Anf].Value = value ? (byte)0x01 : (byte)0x00;
        }

        private void uiSwitchParkR_ValueChanged(object sender, bool value)
        {
            _matrixValDefinitions[SBBR_Parklicht_re_Anf].Value = value ? (byte)0x01 : (byte)0x00;
        }

        private void uiSwitchStop_ValueChanged(object sender, bool value)
        {
            _matrixValDefinitions[SBBR_BRL_Anf].Value = value ? (byte)0x01 : (byte)0x00;
        }

        private void uiSwitchTail_ValueChanged(object sender, bool value)
        {
            if (value)
            {
                _matrixValDefinitions[SBBR_Schlusslicht_Anf_Inv].Value = 0x00;
                _matrixValDefinitions[SBBR_Schlusslicht_Anf].Value = 0x01;
            }
            else
            {
                _matrixValDefinitions[SBBR_Schlusslicht_Anf_Inv].Value = 0x01;
                _matrixValDefinitions[SBBR_Schlusslicht_Anf].Value = 0x00;
            }
        }

        private void uiSwitchFog_ValueChanged(object sender, bool value)
        {
            _matrixValDefinitions[SBBR_Nebelschluss_Fzg_Anf].Value = value ? (byte)0x01 : (byte)0x00;
        }

        private void uiSwitchBul_ValueChanged(object sender, bool value)
        {
            _matrixValDefinitions[SBBR_Rueckfahrlicht_Anf].Value = value ? (byte)0x01 : (byte)0x00;
        }

        private void cmbChLhAnimation_SelectedIndexChanged(object sender, EventArgs e)
        {
            //_matrixValDefinitions[SBBR_CH_LH_Animation].Value = (byte)cmbChLhAnimation.SelectedIndex;
        }

        private void uiSwitchChLhInvoke_ValueChanged(object sender, bool value)
        {
            if (value)
            {
                if (!uiSwitchPlayChLh.Active)
                {
                    uiSwitchParkL.Active = false;
                    uiSwitchParkR.Active = false;
                    uiSwitchStop.Active = false;
                    uiSwitchTail.Active = false;
                    uiSwitchFog.Active = false;
                    uiSwitchBul.Active = false;
                    uiSwitchTailGate.Active = false;
                    uiSwitchTurnWibiLR.Active = false;
                    uiSwitchTurnLR.Active = false;

                    gpbBrakeLamp.Enabled = false;
                    gpbPlayChLh.Enabled = false;
                    gpbTurn.Enabled = false;

                    cmbChLhAnimation.Enabled = false;
                    rdBtnCh.Enabled = false;
                    rdBtnLh.Enabled = false;

                    uiSwitchKl15.Active = true;
                    uiSwitchKl15.Enabled = false;

                    uiSwitchEmcMode2.Active = false;
                    uiSwitchEmcMode2.Enabled = false;

                    Thread.Sleep(100);
                }

                if (rdBtnCh.Checked && !rdBtnLh.Checked) // CHO动画，转向灯闪烁两次
                {
                    _matrixValDefinitions[SBBR_Schlusslicht_Anf_Inv].Value = 0x00;
                    _matrixValDefinitions[SBBR_Schlusslicht_Anf].Value = 0x01; // 开启位置灯

                    uiSwitchKl15.Active = false;
                    //_matrixValDefinitions[ZAS_Kl_15].Value = 0x00;
                    //_matrixValDefinitions[ZAS_Kl_15_inv].Value = 0x01; // 关闭KL15开关

                    _matrixValDefinitions[SBBR_CH_LH_Animation].Value = 0x00; // 动画模式为模式0（默认模式）

                    Thread.Sleep(150);
                    _matrixValDefinitions[SBBR_CH_LH_Animation].Value = (byte)cmbChLhAnimation.SelectedIndex;
                    Thread.Sleep(100);

                    _matrixValDefinitions[SBBR_Schlusslicht_Anf_Inv].Value = 0x01;
                    _matrixValDefinitions[SBBR_Schlusslicht_Anf].Value = 0x00; // 关闭位置灯

                    uiSwitchKl15.Active = false;
                    //_matrixValDefinitions[ZAS_Kl_15].Value = 0x00;
                    //_matrixValDefinitions[ZAS_Kl_15_inv].Value = 0x01; // 关闭KL15开关
                }
                else if (!rdBtnCh.Checked && rdBtnLh.Checked) // LHO动画，转向灯闪烁两次
                {
                    _matrixValDefinitions[SBBR_Schlusslicht_Anf_Inv].Value = 0x01;
                    _matrixValDefinitions[SBBR_Schlusslicht_Anf].Value = 0x00; // 关闭位置灯

                    uiSwitchKl15.Active = false;
                    //_matrixValDefinitions[ZAS_Kl_15].Value = 0x00;
                    //_matrixValDefinitions[ZAS_Kl_15_inv].Value = 0x01; // 关闭KL15开关

                    _matrixValDefinitions[SBBR_CH_LH_Animation].Value = 0x00; // 动画模式为模式0（默认模式）

                    cmbFRAWiBiAnimation.SelectedIndex = 1;

                    Thread.Sleep(150);
                    _matrixValDefinitions[SBBR_CH_LH_Animation].Value = (byte)cmbChLhAnimation.SelectedIndex;
                    Thread.Sleep(100);

                    var taskTurn = new Task(() =>
                    {
                        uiSwitchTurnWibiLR.Active = true;
                        uiSwitchTurnLR.Active = true;
                        Thread.Sleep(400);

                        uiSwitchTurnLR.Active = false;
                        Thread.Sleep(400);

                        uiSwitchTurnLR.Active = true;
                        Thread.Sleep(400);

                        uiSwitchTurnLR.Active = false;
                    });
                    taskTurn.Start();

                    _matrixValDefinitions[SBBR_Schlusslicht_Anf_Inv].Value = 0x00;
                    _matrixValDefinitions[SBBR_Schlusslicht_Anf].Value = 0x01; // 开启位置灯

                    uiSwitchKl15.Active = false;
                    //_matrixValDefinitions[ZAS_Kl_15].Value = 0x00;
                    //_matrixValDefinitions[ZAS_Kl_15_inv].Value = 0x01; // 关闭KL15开关
                }
            }
            else
            {
                _matrixValDefinitions[SBBR_CH_LH_Animation].Value = 0; // 动画模式为模式0（默认模式）

                _matrixValDefinitions[SBBR_Schlusslicht_Anf_Inv].Value = 0x01;
                _matrixValDefinitions[SBBR_Schlusslicht_Anf].Value = 0x00; // 关闭位置灯

                uiSwitchKl15.Active = true;
                //_matrixValDefinitions[ZAS_Kl_15].Value = 0x01;
                //_matrixValDefinitions[ZAS_Kl_15_inv].Value = 0x00; // 打开KL15开关

                if (!uiSwitchPlayChLh.Active)
                {
                    gpbBrakeLamp.Enabled = true;
                    gpbPlayChLh.Enabled = true;
                    gpbTurn.Enabled = true;

                    cmbChLhAnimation.Enabled = true;
                    rdBtnCh.Enabled = true;
                    rdBtnLh.Enabled = true;

                    uiSwitchKl15.Enabled = true;

                    uiSwitchEmcMode2.Active = false;
                    uiSwitchEmcMode2.Enabled = true;
                }
            }
        }

        private void uiSwitchTurnWibiL_ValueChanged(object sender, bool value)
        {
            _matrixValDefinitions[SBBR_FRA_WiBi_links].Value = value ? (byte)0x01 : (byte)0x00;
        }

        private void uiSwitchTurnWibiR_ValueChanged(object sender, bool value)
        {
            _matrixValDefinitions[SBBR_FRA_WiBi_rechts].Value = value ? (byte)0x01 : (byte)0x00;
        }

        private void uiSwitchTurnWibiLR_ValueChanged(object sender, bool value)
        {
            uiSwitchTurnWibiL.Active = value;
            uiSwitchTurnWibiR.Active = value;
        }

        private void uiSwitchTurnL_ValueChanged(object sender, bool value)
        {
            _matrixValDefinitions[SBBR_FRA_links].Value = value ? (byte)0x01 : (byte)0x00;
        }

        private void uiSwitchTurnR_ValueChanged(object sender, bool value)
        {
            _matrixValDefinitions[SBBR_FRA_rechts].Value = value ? (byte)0x01 : (byte)0x00;
        }

        private void uiSwitchTurnLR_ValueChanged(object sender, bool value)
        {
            uiSwitchTurnL.Active = value;
            uiSwitchTurnR.Active = value;
        }

        private void uiSwitchTurnOnOff_ValueChanged(object sender, bool value)
        {
            if (value)
            {
                gpbAnimation.Enabled = false;
                gpbPlayChLh.Enabled = false;

                uiSwitchTurnWibiL.Enabled = false;
                uiSwitchTurnWibiR.Enabled = false;
                uiSwitchTurnWibiLR.Enabled = false;
                uiSwitchTurnL.Enabled = false;
                uiSwitchTurnR.Enabled = false;
                uiSwitchTurnLR.Enabled = false;

                uiSwitchEmcMode2.Enabled = false;
            }
            else
            {
                gpbAnimation.Enabled = true;
                gpbPlayChLh.Enabled = true;

                uiSwitchTurnWibiL.Enabled = true;
                uiSwitchTurnWibiR.Enabled = true;
                uiSwitchTurnWibiLR.Enabled = true;
                uiSwitchTurnL.Enabled = true;
                uiSwitchTurnR.Enabled = true;
                uiSwitchTurnLR.Enabled = true;

                uiSwitchEmcMode2.Enabled = true;
            }

            _isTurnSwitchOnOff = value;
        }

        private void uiSwitchTailGate_ValueChanged(object sender, bool value)
        {
            if (value)
            {
                _matrixValDefinitions[ZV_HD_offen].Value = 0x01;
            }
            else
            {
                _matrixValDefinitions[ZV_HD_offen].Value = 0x00;
            }

            //uiSwitchTail.Active = value;
        }

        private void uiSwitchPlayChLh_ValueChanged(object sender, bool value)
        {
            if (value)
            {
                uiSwitchParkL.Active = false;
                uiSwitchParkR.Active = false;
                uiSwitchStop.Active = false;
                uiSwitchTail.Active = false;
                uiSwitchFog.Active = false;
                uiSwitchBul.Active = false;
                uiSwitchTailGate.Active = false;
                uiSwitchTurnWibiLR.Active = false;
                uiSwitchTurnLR.Active = false;

                gpbBrakeLamp.Enabled = false;
                gpbAnimation.Enabled = false;
                gpbTurn.Enabled = false;

                uiSwitchChLhInvoke.Active = false;
                rdBtnLh.Checked = true;
                cmbChLhAnimation.SelectedIndex = 1;
                Thread.Sleep(100);
                uiSwitchChLhInvoke.Active = true;
                _animationIndex = 1;

                playAnimationTimer.Enabled = true;
                playAnimationTimer.Interval = txtDelay.Value;
                playAnimationTimer.Start();
            }
            else
            {
                playAnimationTimer.Enabled = false;
                playAnimationTimer.Stop();

                _animationIndex = 0;

                gpbBrakeLamp.Enabled = true;
                gpbAnimation.Enabled = true;
                gpbTurn.Enabled = true;
                uiSwitchChLhInvoke.Active = false;
            }
        }

        private int _animationIndex = -1;

        private void playAnimationTimer_Tick(object sender, EventArgs e)
        {
            BeginInvoke(new Action(() =>
            {
                switch (_animationIndex)
                {
                    case 1:
                        uiSwitchChLhInvoke.Active = false;
                        rdBtnLh.Checked = true;
                        cmbChLhAnimation.SelectedIndex = 2;
                        Thread.Sleep(100);
                        uiSwitchChLhInvoke.Active = true;
                        _animationIndex = 2;
                        break;

                    case 2:
                        uiSwitchChLhInvoke.Active = false;
                        rdBtnLh.Checked = true;
                        cmbChLhAnimation.SelectedIndex = 3;
                        Thread.Sleep(100);
                        uiSwitchChLhInvoke.Active = true;
                        _animationIndex = 3;
                        break;

                    case 3:
                        uiSwitchChLhInvoke.Active = false;
                        rdBtnCh.Checked = true;
                        cmbChLhAnimation.SelectedIndex = 1;
                        Thread.Sleep(100);
                        uiSwitchChLhInvoke.Active = true;
                        _animationIndex = 4;
                        break;

                    case 4:
                        uiSwitchChLhInvoke.Active = false;
                        rdBtnCh.Checked = true;
                        cmbChLhAnimation.SelectedIndex = 2;
                        Thread.Sleep(100);
                        uiSwitchChLhInvoke.Active = true;
                        _animationIndex = 5;
                        break;

                    case 5:
                        uiSwitchChLhInvoke.Active = false;
                        rdBtnCh.Checked = true;
                        cmbChLhAnimation.SelectedIndex = 3;
                        Thread.Sleep(100);
                        uiSwitchChLhInvoke.Active = true;
                        _animationIndex = 1;
                        break;
                }
            }));
        }

        private void uiSwitchSBBR_Helligkeit_Hoch_ValueChanged(object sender, bool value)
        {
            _matrixValDefinitions[SBBR_Helligkeit_Hoch].Value = value ? (byte)0x01 : (byte)0x00;
        }

        private void uiSwitchSBBR_Helligkeit_Dunkel_ValueChanged(object sender, bool value)
        {
            _matrixValDefinitions[SBBR_Helligkeit_Dunkel].Value = value ? (byte)0x01 : (byte)0x00;
        }

        private void uiSwitchSBBR_Helligkeit_Mitte_ValueChanged(object sender, bool value)
        {
            _matrixValDefinitions[SBBR_Helligkeit_Mitte].Value = value ? (byte)0x01 : (byte)0x00;
        }

        private void uiSwitchKl15_ValueChanged(object sender, bool value)
        {
            if (value)
            {
                _matrixValDefinitions[ZAS_Kl_15].Value = 0x01;
                _matrixValDefinitions[ZAS_Kl_15_inv].Value = 0x00; // 打开KL15开关
            }
            else
            {
                _matrixValDefinitions[ZAS_Kl_15].Value = 0x00;
                _matrixValDefinitions[ZAS_Kl_15_inv].Value = 0x01; // 关闭KL15开关
            }
        }

        private void uiSwitchEmcMode2_ValueChanged(object sender, bool value)
        {
            if (value)
            {
                foreach (var control in gpbBrakeLamp.Controls)
                {
                    if (control is UISwitch)
                    {
                        ((UISwitch)control).Active = false;
                    }
                }
                foreach (var control in gpbTurn.Controls)
                {
                    if (control is UISwitch)
                    {
                        ((UISwitch)control).Active = false;
                    }
                }
                foreach (var control in gpbAnimation.Controls)
                {
                    if (control is UISwitch)
                    {
                        ((UISwitch)control).Active = false;
                    }
                }
                foreach (var control in gpbPlayChLh.Controls)
                {
                    if (control is UISwitch)
                    {
                        ((UISwitch)control).Active = false;
                    }
                }

                uiSwitchChLhInvoke.Active = false;
                uiSwitchTurnOnOff.Active = false;
                uiSwitchLinMsg.Active = true;
                uiSwitchKl15.Active = true;

                gpbBrakeLamp.Enabled = false;
                gpbTurn.Enabled = false;
                gpbAnimation.Enabled = false;
                gpbPlayChLh.Enabled = false;

                uiSwitchLinMsg.Enabled = false;
                uiSwitchKl15.Enabled = false;

                cmbFRAWiBiAnimation.SelectedIndex = 1;
                uiSwitchTurnWibiLR.Active = true;
                Thread.Sleep(100);

                _mode2OnOffState = false;
                playMode2Timer.ReStart();
            }
            else
            {
                gpbBrakeLamp.Enabled = true;
                gpbTurn.Enabled = true;
                gpbAnimation.Enabled = true;
                gpbPlayChLh.Enabled = true;

                uiSwitchLinMsg.Enabled = true;
                uiSwitchKl15.Enabled = true;

                _mode2OnOffState = false;
                playMode2Timer.Stop();
            }
        }

        private bool _mode2OnOffState;

        private void playMode2Timer_Tick(object sender, EventArgs e)
        {
            BeginInvoke(new Action(() =>
            {
                _mode2OnOffState = !_mode2OnOffState;

                uiSwitchParkL.Active = _mode2OnOffState;
                uiSwitchParkR.Active = _mode2OnOffState;
                uiSwitchStop.Active = _mode2OnOffState;
                uiSwitchTail.Active = _mode2OnOffState;
                uiSwitchFog.Active = _mode2OnOffState;
                uiSwitchBul.Active = _mode2OnOffState;

                uiSwitchTurnLR.Active = _mode2OnOffState;
            }));
        }
    }
}
