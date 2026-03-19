using System;
using System.Collections.Generic;
using System.Threading;
using CommonUtility;
using CommonUtility.BusLoader;
using Controller;
using Sunny.UI;

namespace CheckSystem.HelperForms.VW323RearLamp
{
    public sealed partial class Vw323RearLamp : UIForm
    {
        private ControllerBase LinController { get; set; }
        //private readonly Controller.Vw323RearLamp _vw323RearLamp = new Controller.Vw323RearLamp("VW323");
        private Thread LinMsgThread { get; set; }
        public bool IsSleep;
        private readonly LinCommunicationMatrix.IntelMatrix _intelMatrix =
            new LinCommunicationMatrix.IntelMatrix(0x0A, 8);
        private readonly Dictionary<string, MatrixValDefinition> _matrixValDefinitions =
           new Dictionary<string, MatrixValDefinition>();

        private readonly string BCM2_SBBR_01_BZ = "BCM2_SBBR_01_BZ";
        private readonly string ZAS_Kl_15 = "ZAS_Kl_15";
        private readonly string ZAS_Kl_15_inv = "ZAS_Kl_15_inv";
        private readonly string ZV_HD_offen = "ZV_HD_offen";

        private readonly string SBBR_Helligkeit_Hoch = "SBBR_Helligkeit_Hoch";
        private readonly string SBBR_Helligkeit_Mitte = "SBBR_Helligkeit_Mitte";
        private readonly string SBBR_Helligkeit_Dunkel = "SBBR_Helligkeit_Dunkel";

        private readonly string SBBR_FRA_WiBi_links = "SBBR_FRA_WiBi_links";
        private readonly string SBBR_FRA_WiBi_rechts = "SBBR_FRA_WiBi_rechts";
        private readonly string SBBR_FRA_links = "SBBR_FRA_links";
        private readonly string SBBR_FRA_rechts = "SBBR_FRA_rechts";
        private readonly string SBBR_FRA_WiBi_Animation = "SBBR_FRA_WiBi_Animation";
        private readonly string BCM1_CH_aktiv = "BCM1_CH_aktiv";
        private readonly string BCM1_LH_aktiv = "BCM1_LH_aktiv";
        private readonly string SBBR_CH_LH_Animation = "SBBR_CH_LH_Animation";
        private readonly string SBBR_BRL_Anf = "SBBR_BRL_Anf";
        private readonly string SBBR_Schlusslicht_Anf = "SBBR_Schlusslicht_Anf";
        private readonly string SBBR_Parklicht_li_Anf = "SBBR_Parklicht_li_Anf";
        private readonly string SBBR_Parklicht_re_Anf = "SBBR_Parklicht_re_Anf";
        private readonly string SBBR_Rueckfahrlicht_Anf = "SBBR_Rueckfahrlicht_Anf";
        private readonly string SBBR_Schlusslicht_Anf_Inv = "SBBR_Schlusslicht_Anf_Inv";
        private readonly string SBBR_Nebelschluss_Fzg_Anf = "SBBR_Nebelschluss_Fzg_Anf";
        private readonly string SBBR_FRA_B_off = "SBBR_FRA_B_off";

        private bool _isTurnSwitchOnOff;
        private int _isTurnOnOffCount;
        private int _bzCount;

        public Vw323RearLamp(bool isToomoss)
        {
            InitializeComponent();
            Style = UIStyle.Black;

            if (!isToomoss)
                LinController = new SyControllerWith56Pin("56PIN");
            else
            {
                LinController = new ToomossUsb2XxxCanLin("Tomoss");
                Text += @" (图莫斯)";
            }

            Load += Vw323RearLamp_Load;
            Closed += Vw323RearLamp_Closed;

            _matrixValDefinitions.Add(BCM2_SBBR_01_BZ, new MatrixValDefinition(8, 4, 0));
            _matrixValDefinitions.Add(ZV_HD_offen, new MatrixValDefinition(14, 1, 0));
            _matrixValDefinitions.Add(ZAS_Kl_15, new MatrixValDefinition(12, 1, 0));
            _matrixValDefinitions.Add(ZAS_Kl_15_inv, new MatrixValDefinition(15, 1, 1));

            _matrixValDefinitions.Add(SBBR_Helligkeit_Hoch, new MatrixValDefinition(40, 1, 0));
            _matrixValDefinitions.Add(SBBR_Helligkeit_Mitte, new MatrixValDefinition(41, 1, 0));
            _matrixValDefinitions.Add(SBBR_Helligkeit_Dunkel, new MatrixValDefinition(42, 1, 0));

            _matrixValDefinitions.Add(SBBR_FRA_WiBi_links, new MatrixValDefinition(16, 1, 0));
            _matrixValDefinitions.Add(SBBR_FRA_WiBi_rechts, new MatrixValDefinition(17, 1, 0));
            _matrixValDefinitions.Add(SBBR_FRA_links, new MatrixValDefinition(20, 1, 0));
            _matrixValDefinitions.Add(SBBR_FRA_rechts, new MatrixValDefinition(21, 1, 0));
            _matrixValDefinitions.Add(SBBR_FRA_WiBi_Animation, new MatrixValDefinition(22, 2, 0));
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
            _matrixValDefinitions.Add(SBBR_FRA_B_off, new MatrixValDefinition(18, 1, 0));

            uiSwitchLinMsg.ActiveChanged += uiSwitchLinMsg_ActiveChanged;
            uiSwitchStop.ActiveChanged += uiSwitchStop_ActiveChanged;
            uiSwitchBul.ActiveChanged += uiSwitchBul_ActiveChanged;
            uiSwitchFog.ActiveChanged += uiSwitchFog_ActiveChanged;
            uiSwitchTail.ActiveChanged += uiSwitchTail_ActiveChanged;
            uiSwitchParkL.ActiveChanged += uiSwitchParkL_ActiveChanged;
            uiSwitchParkR.ActiveChanged += uiSwitchParkR_ActiveChanged;

            cmbChLhAnimation.Items.Add("Default_CH_LH_Animation");
            cmbChLhAnimation.Items.Add("CH_LH_Animation_1");
            cmbChLhAnimation.Items.Add("CH_LH_Animation_2");
            cmbChLhAnimation.Items.Add("CH_LH_Animation_3");
            cmbChLhAnimation.SelectedIndex = 0;
            cmbChLhAnimation.SelectedIndexChanged += cmbChLhAnimation_SelectedIndexChanged;

            uiSwitchCh.ActiveChanged += uiSwitchCh_ActiveChanged;
            uiSwitchLh.ActiveChanged += uiSwitchLh_ActiveChanged;

            cmbFRAWiBiAnimation.Items.Add("Default_Animation_WiBi");
            cmbFRAWiBiAnimation.Items.Add("WiBi_Animation_1");
            cmbFRAWiBiAnimation.Items.Add("WiBi_Animation_2");
            cmbFRAWiBiAnimation.Items.Add("WiBi_Animation_3");
            cmbFRAWiBiAnimation.SelectedIndex = 0;
            cmbFRAWiBiAnimation.SelectedIndexChanged += cmbFRAWiBiAnimation_SelectedIndexChanged;

            cmbFRABoff.Items.Add("A & B active");
            cmbFRABoff.Items.Add("Only A active");
            cmbFRABoff.SelectedIndex = 0;
            cmbFRABoff.SelectedIndexChanged += cmbFRABoff_SelectedIndexChanged;

            cmb_SBBR_Helligkeit_Hoch.Items.Add("Inactive");
            cmb_SBBR_Helligkeit_Hoch.Items.Add("Actice");
            cmb_SBBR_Helligkeit_Hoch.SelectedIndex = 0;
            cmb_SBBR_Helligkeit_Hoch.SelectedIndexChanged += cmb_SBBR_Helligkeit_Hoch_SelectedIndexChanged;

            cmb_SBBR_Helligkeit_Mitte.Items.Add("Inactive");
            cmb_SBBR_Helligkeit_Mitte.Items.Add("Actice");
            cmb_SBBR_Helligkeit_Mitte.SelectedIndex = 0;
            cmb_SBBR_Helligkeit_Mitte.SelectedIndexChanged += cmb_SBBR_Helligkeit_Mitte_SelectedIndexChanged;

            cmb_SBBR_Helligkeit_Dunkel.Items.Add("Inactive");
            cmb_SBBR_Helligkeit_Dunkel.Items.Add("Actice");
            cmb_SBBR_Helligkeit_Dunkel.SelectedIndex = 0;
            cmb_SBBR_Helligkeit_Dunkel.SelectedIndexChanged += cmb_SBBR_Helligkeit_Dunkel_SelectedIndexChanged;

            uiSwitchTurnWibiL.ActiveChanged += uiSwitchTurnWibiL_ActiveChanged;
            uiSwitchTurnWibiR.ActiveChanged += uiSwitchTurnWibiR_ActiveChanged;
            uiSwitchTurnL.ActiveChanged += uiSwitchTurnL_ActiveChanged;
            uiSwitchTurnR.ActiveChanged += uiSwitchTurnR_ActiveChanged;
            uiSwitchTurnWibiLR.ActiveChanged += uiSwitchTurnWibiLR_ActiveChanged;
            uiSwitchTurnLR.ActiveChanged += uiSwitchTurnLR_ActiveChanged;

            uiSwitchTurnOnOff.ActiveChanged += uiSwitchTurnOnOffLR_ActiveChanged;
        }

        private void cmb_SBBR_Helligkeit_Dunkel_SelectedIndexChanged(object sender, EventArgs e)
        {
            _matrixValDefinitions[SBBR_Helligkeit_Dunkel].Value = (byte)cmb_SBBR_Helligkeit_Dunkel.SelectedIndex;
        }

        private void cmb_SBBR_Helligkeit_Mitte_SelectedIndexChanged(object sender, EventArgs e)
        {
            _matrixValDefinitions[SBBR_Helligkeit_Mitte].Value = (byte)cmb_SBBR_Helligkeit_Mitte.SelectedIndex;
        }

        private void cmb_SBBR_Helligkeit_Hoch_SelectedIndexChanged(object sender, EventArgs e)
        {
            _matrixValDefinitions[SBBR_Helligkeit_Hoch].Value = (byte)cmb_SBBR_Helligkeit_Hoch.SelectedIndex;
        }

        private void cmbFRABoff_SelectedIndexChanged(object sender, EventArgs e)
        {
            _matrixValDefinitions[SBBR_FRA_B_off].Value = (byte)cmbFRABoff.SelectedIndex;
            //_matrixValDefinitions[BCM1_CH_aktiv].Value = (byte)cmbFRABoff.SelectedIndex;
        }

        private void uiSwitchTurnOnOffLR_ActiveChanged(object sender, EventArgs e)
        {
            if (uiSwitchTurnOnOff.Active)
            {
                SwitchAnimationEnable(false);
                SwitchTurnEnable(false);
                SwitchTurnAnimationEnable(false);

                uiSwitchTurnOnOff.Enabled = true;
            }
            else
            {
                SwitchTurnSignal(false);

                SwitchAnimationEnable(true);
                SwitchTurnEnable(true);
                SwitchTurnAnimationEnable(true);
            }

            _isTurnSwitchOnOff = uiSwitchTurnOnOff.Active;
        }

        private void Vw323RearLamp_Closed(object sender, EventArgs e)
        {
            if (LinMsgThread != null)
            {
                LinMsgThread.Abort();
                LinMsgThread.Join();
            }

            if (LinController != null)
                LinController.Dispose();
        }

        private void Vw323RearLamp_Load(object sender, EventArgs e)
        {
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

        private void cmbFRAWiBiAnimation_SelectedIndexChanged(object sender, EventArgs e)
        {
            _matrixValDefinitions[SBBR_FRA_WiBi_Animation].Value = (byte)cmbFRAWiBiAnimation.SelectedIndex;
        }

        private void uiSwitchTurnWibiR_ActiveChanged(object sender, EventArgs e)
        {
            _matrixValDefinitions[SBBR_FRA_WiBi_rechts].Value = uiSwitchTurnWibiR.Active ? (byte)0x01 : (byte)0x00;
        }

        private void uiSwitchTurnWibiL_ActiveChanged(object sender, EventArgs e)
        {
            _matrixValDefinitions[SBBR_FRA_WiBi_links].Value = uiSwitchTurnWibiL.Active ? (byte)0x01 : (byte)0x00;
        }

        private void uiSwitchTurnWibiLR_ActiveChanged(object sender, EventArgs e)
        {
            if (uiSwitchTurnWibiLR.Active)
            {
                uiSwitchTurnWibiL.Active = true;
                uiSwitchTurnWibiR.Active = true;
            }
            else
            {
                uiSwitchTurnWibiL.Active = false;
                uiSwitchTurnWibiR.Active = false;
            }
        }

        private void uiSwitchTurnR_ActiveChanged(object sender, EventArgs e)
        {
            _matrixValDefinitions[SBBR_FRA_rechts].Value = uiSwitchTurnR.Active ? (byte)0x01 : (byte)0x00;
        }

        private void uiSwitchTurnL_ActiveChanged(object sender, EventArgs e)
        {
            _matrixValDefinitions[SBBR_FRA_links].Value = uiSwitchTurnL.Active ? (byte)0x01 : (byte)0x00;
        }

        private void uiSwitchTurnLR_ActiveChanged(object sender, EventArgs e)
        {
            if (uiSwitchTurnLR.Active)
            {
                uiSwitchTurnL.Active = true;
                uiSwitchTurnR.Active = true;
            }
            else
            {
                uiSwitchTurnL.Active = false;
                uiSwitchTurnR.Active = false;
            }
        }

        private void uiSwitchLh_ActiveChanged(object sender, EventArgs e)
        {
            _matrixValDefinitions[BCM1_LH_aktiv].Value = uiSwitchLh.Active ? (byte)0x01 : (byte)0x00;

            if (uiSwitchLh.Active)
            {
                SwitchOtherLedSignal(false);
                SwitchOherLedEnable(false);

                SwitchTurnSignal(false);
                SwitchTurnEnable(false);
                SwitchTurnAnimationEnable(false);

                SwitchAnimationEnable(false);
                uiSwitchLh.Enabled = true;
            }
            else
            {
                SwitchOtherLedSignal(false);
                SwitchOherLedEnable(true);

                SwitchTurnSignal(false);
                SwitchTurnEnable(true);
                SwitchTurnAnimationEnable(true);

                SwitchAnimationEnable(true);
            }
        }

        private void uiSwitchCh_ActiveChanged(object sender, EventArgs e)
        {
            _matrixValDefinitions[BCM1_CH_aktiv].Value = uiSwitchCh.Active ? (byte)0x01 : (byte)0x00;

            if (uiSwitchCh.Active)
            {
                SwitchOtherLedSignal(false);
                SwitchOherLedEnable(false);

                SwitchTurnSignal(false);
                SwitchTurnEnable(false);
                SwitchTurnAnimationEnable(false);

                SwitchAnimationEnable(false);
                uiSwitchCh.Enabled = true;
            }
            else
            {
                SwitchOtherLedSignal(false);
                SwitchOherLedEnable(true);

                SwitchTurnSignal(false);
                SwitchTurnEnable(true);
                SwitchTurnAnimationEnable(true);

                SwitchAnimationEnable(true);
            }
        }

        /// <summary>
        /// Ch Lh Animation 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbChLhAnimation_SelectedIndexChanged(object sender, EventArgs e)
        {
            _matrixValDefinitions[SBBR_CH_LH_Animation].Value = (byte)cmbChLhAnimation.SelectedIndex;
        }

        /// <summary>
        /// park r
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiSwitchParkR_ActiveChanged(object sender, EventArgs e)
        {
            _matrixValDefinitions[SBBR_Parklicht_re_Anf].Value = uiSwitchParkR.Active ? (byte)0x01 : (byte)0x00;
        }

        /// <summary>
        /// parl l
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiSwitchParkL_ActiveChanged(object sender, EventArgs e)
        {
            _matrixValDefinitions[SBBR_Parklicht_li_Anf].Value = uiSwitchParkL.Active ? (byte)0x01 : (byte)0x00;
        }

        /// <summary>
        /// tail
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiSwitchTail_ActiveChanged(object sender, EventArgs e)
        {
            if (uiSwitchTail.Active)
            {
                _matrixValDefinitions[SBBR_Schlusslicht_Anf_Inv].Value = 0x00;
                _matrixValDefinitions[SBBR_Schlusslicht_Anf].Value = 0x01;
            }
            else
            {
                _matrixValDefinitions[SBBR_Schlusslicht_Anf_Inv].Value = 0x01;
                _matrixValDefinitions[SBBR_Schlusslicht_Anf].Value = 0x00;
            }

            //_matrixValDefinitions[SBBR_Schlusslicht_Anf].Value = uiSwitchTail.Active ? (byte)0x01 : (byte)0x00;
        }

        /// <summary>
        /// fog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiSwitchFog_ActiveChanged(object sender, EventArgs e)
        {
            _matrixValDefinitions[SBBR_Nebelschluss_Fzg_Anf].Value = uiSwitchFog.Active ? (byte)0x01 : (byte)0x00;
        }

        /// <summary>
        /// bul
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiSwitchBul_ActiveChanged(object sender, EventArgs e)
        {
            _matrixValDefinitions[SBBR_Rueckfahrlicht_Anf].Value = uiSwitchBul.Active ? (byte)0x01 : (byte)0x00;
        }

        /// <summary>
        /// stop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiSwitchStop_ActiveChanged(object sender, EventArgs e)
        {
            _matrixValDefinitions[SBBR_BRL_Anf].Value = uiSwitchStop.Active ? (byte)0x01 : (byte)0x00;
        }

        private void MainWork()
        {
            //_intelMatrix.UpdateData(new MatrixValDefinition(14, 1, 1));
            //_intelMatrix.UpdateData(new MatrixValDefinition(15, 1, 1));

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
                _bzCount++;
                if (_bzCount == 16)
                    _bzCount = 0;

                foreach (var key in _matrixValDefinitions.Keys)
                    _intelMatrix.UpdateData(_matrixValDefinitions[key]);

                var crc = Calculate_CRC(_intelMatrix.MatrixData, 7);
                _intelMatrix.MatrixData[0] = crc;

                linBus.SendMasterLin(
                   _intelMatrix.MasterLinId, _intelMatrix.MatrixData);
                Console.WriteLine(ValueHelper.GetHextStr(_intelMatrix.MatrixData));
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

        private void uiSwitchLinMsg_ActiveChanged(object sender, EventArgs e)
        {
            IsSleep = !uiSwitchLinMsg.Active;
        }

        private void SwitchOtherLedSignal(bool isActive)
        {
            uiSwitchStop.Active = isActive;
            uiSwitchBul.Active = isActive;
            uiSwitchFog.Active = isActive;
            uiSwitchTail.Active = isActive;
            uiSwitchParkL.Active = isActive;
            uiSwitchParkR.Active = isActive;
        }

        private void SwitchOherLedEnable(bool isEnable)
        {
            uiSwitchStop.Enabled = isEnable;
            uiSwitchBul.Enabled = isEnable;
            uiSwitchFog.Enabled = isEnable;
            uiSwitchTail.Enabled = isEnable;
            uiSwitchParkL.Enabled = isEnable;
            uiSwitchParkR.Enabled = isEnable;
        }

        private void SwitchTurnSignal(bool isActive)
        {
            uiSwitchTurnWibiL.Active = isActive;
            uiSwitchTurnWibiR.Active = isActive;
            uiSwitchTurnWibiLR.Active = isActive;
            uiSwitchTurnL.Active = isActive;
            uiSwitchTurnR.Active = isActive;
            uiSwitchTurnLR.Active = isActive;
        }

        private void SwitchTurnEnable(bool isEnable)
        {
            uiSwitchTurnWibiL.Enabled = isEnable;
            uiSwitchTurnWibiR.Enabled = isEnable;
            uiSwitchTurnWibiLR.Enabled = isEnable;
            uiSwitchTurnL.Enabled = isEnable;
            uiSwitchTurnR.Enabled = isEnable;
            uiSwitchTurnLR.Enabled = isEnable;
        }

        private void SwitchTurnAnimationEnable(bool isEnable)
        {
            uiSwitchTurnOnOff.Enabled = isEnable;
        }

        private void SwitchAnimationEnable(bool isEnable)
        {
            cmbChLhAnimation.Enabled = isEnable;
            uiSwitchCh.Enabled = isEnable;
            uiSwitchLh.Enabled = isEnable;
        }
    }
}
