using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

namespace Controller
{
    [Description("LIN-Product,VW416组合后灯-lin波特率19200")]
    public sealed class Vw416RearLamp : ControllerBase
    {
        public LinBus Lin19200;

        public Vw416RearLamp(string name) : base(name)
        {
            LinBus.PushLinMsg += LinBus_PushLinMsg;

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

            _matrixValDefinitions[SBBR_FRA_WiBi_Animation].Value = 0x02;

            Kl15On();
            RearLampOff();

            if (_th != null)
            {
                _th.Abort();
                _th.Join();
            }

            _th = new Thread(MainWork) { IsBackground = true };
            _th.Start();
        }

        private void LinBus_PushLinMsg(string name, LinBus.LinDataPackage data)
        {
            if (Lin19200 != null && Lin19200.Name == name)
            {
                if (data.LinId == 0x0B || data.LinId == LinBus.ConvertLinId(0x0B))
                {
                    Console.WriteLine("msg 0x0B: " + ValueHelper.GetHextStr(data.LinData));
                }
                else if (data.LinId == 0x0C || data.LinId == LinBus.ConvertLinId(0x0C))
                {
                    Console.WriteLine("msg 0x0C: " + ValueHelper.GetHextStr(data.LinData));
                }
                else if (data.LinId == 0x0D || data.LinId == LinBus.ConvertLinId(0x0D))
                {
                    Console.WriteLine("msg 0x0D: " + ValueHelper.GetHextStr(data.LinData));
                }
                else if (data.LinId == 0x0E || data.LinId == LinBus.ConvertLinId(0x0E))
                {
                    Console.WriteLine("msg 0x0E: " + ValueHelper.GetHextStr(data.LinData));
                }
            }
        }

        ~Vw416RearLamp()
        {
            Dispose();
        }

        #region 点灯相关

        private void Count()
        {
            _matrixValDefinitions[BCM2_SBBR_01_BZ].Value = (byte)_bzCount;
            _bzCount++;
            if (_bzCount == 16)
                _bzCount = 0;
        }

        /// <summary>
        /// Berechnet mit der 2x16 Byte Methode aus dem Lastenheft
        /// </summary>
        /// <param name="data"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private static byte Calculate_CRC(IReadOnlyList<byte> data, int length)
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

        [Description("唤醒")]
        public void LampAwake()
        {
            IsSleep = false;
        }

        [Description("休眠")]
        public void LampSleep()
        {
            IsSleep = true;
        }

        private bool _isKl15On = false;

        [Description("KL15开关ON")]
        public void Kl15On()
        {
            _isKl15On = true;
            _matrixValDefinitions[ZAS_Kl_15].Value = 0x01;
            _matrixValDefinitions[ZAS_Kl_15_inv].Value = 0x00; // 打开KL15开关
        }

        [Description("KL15开关OFF")]
        public void Kl15Off()
        {
            _isKl15On = false;
            _matrixValDefinitions[ZAS_Kl_15].Value = 0x00;
            _matrixValDefinitions[ZAS_Kl_15_inv].Value = 0x01; // 关闭KL15开关
        }

        [Description("TAIL 3D亮")]
        public void Tail3DOn()
        {
            SwitchTail3D(true);
        }

        [Description("TAIL 3D灭")]
        public void Tail3DOff()
        {
            SwitchTail3D(false);
        }

        private void SwitchTail3D(bool isOn)
        {
            _matrixValDefinitions[SBBR_Parklicht_li_Anf].Value = isOn ? (byte)0x01 : (byte)0x00;
            _matrixValDefinitions[SBBR_Parklicht_re_Anf].Value = isOn ? (byte)0x01 : (byte)0x00;

            //if (isOn)
            //{
            //    MotorolaMatrix0X0A.UpdateData(
            //        _lampOperaterDic[LampOnOffType.SbbrParklichtLiAnfOn]);
            //    MotorolaMatrix0X0A.UpdateData(
            //        _lampOperaterDic[LampOnOffType.SbbrParklichtReAnfOn]);
            //}
            //else
            //{
            //    MotorolaMatrix0X0A.UpdateData(
            //        _lampOperaterDic[LampOnOffType.SbbrParklichtLiAnfOff]);
            //    MotorolaMatrix0X0A.UpdateData(
            //        _lampOperaterDic[LampOnOffType.SbbrParklichtReAnfOff]);
            //}
        }

        /// <summary>
        /// 制动灯亮
        /// </summary>
        [Description("制动灯亮")]
        public void StopOn()
        {
            _matrixValDefinitions[SBBR_BRL_Anf].Value = (byte)0x01;
            //MotorolaMatrix0X0A.UpdateData(
            //    _lampOperaterDic[LampOnOffType.SbbrBrlAnfOn]);
        }

        /// <summary>
        /// 制动灯灭
        /// </summary>
        [Description("制动灯灭")]
        public void StopOff()
        {
            _matrixValDefinitions[SBBR_BRL_Anf].Value = (byte)0x00;
            //MotorolaMatrix0X0A.UpdateData(
            //    _lampOperaterDic[LampOnOffType.SbbrBrlAnfOff]);
        }

        /// <summary>
        /// 位置灯亮
        /// </summary>
        [Description("位置灯低亮")]
        public void RearLampOn()
        {
            SwitchRearLamp(true);
            SwitchRearLampHd(false);
        }

        [Description("位置灯高亮开")]
        public void RearLampHdOn()
        {
            SwitchRearLamp(true);
            SwitchRearLampHd(true);
        }

        /// <summary>
        /// 位置灯灭
        /// </summary>
        [Description("位置灯灭")]
        public void RearLampOff()
        {
            SwitchRearLamp(false);
            SwitchRearLampHd(false);
        }

        private void SwitchRearLamp(bool isOn)
        {
            _matrixValDefinitions[SBBR_Schlusslicht_Anf].Value = isOn ? (byte)0x01 : (byte)0x00;
            _matrixValDefinitions[SBBR_Schlusslicht_Anf_Inv].Value = isOn ? (byte)0x00 : (byte)0x01;
            //if (isOn)
            //{
            //    MotorolaMatrix0X0A.UpdateData(
            //        _lampOperaterDic[LampOnOffType.SbbrSchlusslichtAnfOn]);
            //    MotorolaMatrix0X0A.UpdateData(
            //        _lampOperaterDic[LampOnOffType.SbbrSchlusslichtAnfInvOff]);
            //}
            //else
            //{
            //    MotorolaMatrix0X0A.UpdateData(
            //        _lampOperaterDic[LampOnOffType.SbbrSchlusslichtAnfOff]);
            //    MotorolaMatrix0X0A.UpdateData(
            //        _lampOperaterDic[LampOnOffType.SbbrSchlusslichtAnfInvOn]);
            //}
        }

        private void SwitchRearLampHd(bool isOn)
        {
            _matrixValDefinitions[ZV_HD_offen].Value = isOn ? (byte)0x01 : (byte)0x00;
            //MotorolaMatrix0X0A.UpdateData(isOn
            //    ? _lampOperaterDic[LampOnOffType.ZvHdOffenOn]
            //    : _lampOperaterDic[LampOnOffType.ZvHdOffenOff]);
        }

        /// <summary>
        /// 倒车灯亮
        /// </summary>
        [Description("倒车灯亮")]
        public void BulLampOn()
        {
            SwitchBulLamp(true);
        }

        /// <summary>
        /// 倒车灯灭
        /// </summary>
        [Description("倒车灯灭")]
        public void BulLampOff()
        {
            SwitchBulLamp(false);
        }

        /// <summary>
        /// 倒车灯
        /// </summary>
        /// <param name="isOn"></param>
        private void SwitchBulLamp(bool isOn)
        {
            _matrixValDefinitions[SBBR_Rueckfahrlicht_Anf].Value = isOn ? (byte)0x01 : (byte)0x00;
            //MotorolaMatrix0X0A.UpdateData(isOn
            //   ? _lampOperaterDic[LampOnOffType.SbbrRueckfahrlichtAnfOn]
            //   : _lampOperaterDic[LampOnOffType.SbbrRueckfahrlichtAnfOff]);
        }

        [Description("雾灯亮")]
        public void FogLampOn()
        {
            SwitchFogLamp(true);
        }

        [Description("雾灯灭")]
        public void FogLampOff()
        {
            SwitchFogLamp(false);
        }

        private void SwitchFogLamp(bool isOn)
        {
            _matrixValDefinitions[SBBR_Nebelschluss_Fzg_Anf].Value = isOn ? (byte)0x01 : (byte)0x00;
            //MotorolaMatrix0X0A.UpdateData(isOn
            //  ? _lampOperaterDic[LampOnOffType.FogLampOn]
            //  : _lampOperaterDic[LampOnOffType.FogLampOff]);
        }

        /// <summary>
        /// 左转向灯顺序亮
        /// </summary>
        [Description("左转向灯顺序亮")]
        public void LeftRearTurnRunningOn()
        {
            _matrixValDefinitions[SBBR_FRA_WiBi_links].Value = (byte)0x01;
            _matrixValDefinitions[SBBR_FRA_links].Value = (byte)0x01;
            SwitchLeftTurnFlicker(false);
            SwitchLeftRearTurnRunningLamp(true);
        }

        private void SwitchLeftRearTurnRunningLamp(bool isOn)
        {
            //if (isOn)
            //{
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiLinksOn]);
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiRechtsOff]);
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraLinksOn]);
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraRechtsOff]);
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiAnimationOn]);
            //}
            //else
            //{
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiLinksOff]);
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiRechtsOff]);
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraLinksOff]);
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraRechtsOff]);
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiAnimationOff]);
            //}
        }

        /// <summary>
        /// 左转向灯闪烁亮
        /// </summary>
        [Description("左转向灯闪烁亮")]
        public void LeftRearTurnFlickerOn()
        {
            _matrixValDefinitions[SBBR_FRA_WiBi_links].Value = (byte)0x00;
            _matrixValDefinitions[SBBR_FRA_links].Value = (byte)0x01;
            SwitchLeftRearTurnRunningLamp(false);
            SwitchLeftTurnFlicker(true);
        }

        [Description("左转向灯灭")]
        public void LeftRearTurnOff()
        {
            _matrixValDefinitions[SBBR_FRA_WiBi_links].Value = (byte)0x00;
            _matrixValDefinitions[SBBR_FRA_links].Value = (byte)0x00;
            SwitchRightTurnFlicker(false);
            SwitchLeftRearTurnRunningLamp(false);
        }

        private void SwitchLeftTurnFlicker(bool isOn)
        {
            //if (isOn)
            //{
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiLinksOff]);
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiRechtsOff]);
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraLinksOn]);
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraRechtsOff]);
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiAnimationOff]);
            //}
            //else
            //{
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiLinksOff]);
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiRechtsOff]);
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraLinksOff]);
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraRechtsOff]);
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiAnimationOff]);
            //}
        }

        [Description("右转向灯顺序亮")]
        public void RightRearTurnRunningOn()
        {
            _matrixValDefinitions[SBBR_FRA_WiBi_rechts].Value = (byte)0x01;
            _matrixValDefinitions[SBBR_FRA_rechts].Value = (byte)0x01;
            SwitchRightTurnFlicker(false);
            SwitchRightRearTurnRunningLamp(true);
        }

        private void SwitchRightRearTurnRunningLamp(bool isOn)
        {
            //if (isOn)
            //{
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiLinksOff]);
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiRechtsOn]);
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraLinksOff]);
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraRechtsOn]);
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiAnimationOn]);
            //}
            //else
            //{
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiLinksOff]);
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiRechtsOff]);
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraLinksOff]);
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraRechtsOff]);
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiAnimationOff]);
            //}
        }

        [Description("右转向灯闪烁亮")]
        public void RightRearTurnFlickerOn()
        {
            _matrixValDefinitions[SBBR_FRA_WiBi_rechts].Value = (byte)0x00;
            _matrixValDefinitions[SBBR_FRA_rechts].Value = (byte)0x01;
            SwitchRightRearTurnRunningLamp(false);
            SwitchRightTurnFlicker(true);
        }

        [Description("右转向灯灭")]
        public void RightRearTurnOff()
        {
            _matrixValDefinitions[SBBR_FRA_WiBi_rechts].Value = (byte)0x00;
            _matrixValDefinitions[SBBR_FRA_rechts].Value = (byte)0x00;
            SwitchRightTurnFlicker(false);
            SwitchRightRearTurnRunningLamp(false);
        }

        private void SwitchRightTurnFlicker(bool isOn)
        {
            //if (isOn)
            //{
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiLinksOff]);
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiRechtsOff]);
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraLinksOff]);
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraRechtsOn]);
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiAnimationOff]);
            //}
            //else
            //{
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiLinksOff]);
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiRechtsOff]);
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraLinksOff]);
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraRechtsOff]);
            //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiAnimationOff]);
            //}
        }

        //private void SwitchSignal(LampOnOffType type)
        //{
        //    MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[type]);
        //}

        [Description("离家动画")]
        public void LevingHome(int index)
        {
            AllLedOff();
            if (index >= 1 && index <= 3)
            {
                RearLampOff();
                Kl15Off();
                Thread.Sleep(150);
                _matrixValDefinitions[SBBR_CH_LH_Animation].Value = (byte)index;
                Thread.Sleep(100);
                RearLampOn();
                Kl15Off();
            }
            //Thread.Sleep(10);
            // SwitchSignal(LampOnOffType.Bcm1LhAktivOn);
        }

        [Description("回家动画")]
        public void ComingHome(int index)
        {
            AllLedOff();
            if (index >= 1 && index <= 3)
            {
                RearLampOn();
                Kl15Off();
                Thread.Sleep(150);
                _matrixValDefinitions[SBBR_CH_LH_Animation].Value = (byte)index;
                Thread.Sleep(100);
                RearLampOff();
                Kl15Off();
            }
            //SwitchSignal(LampOnOffType.Bcm1ChAktivOn);
        }

        [Description("关闭所有灯光并打断动画")]
        public void AllLedOff()
        {
            StopOff();
            RearLampOff();
            FogLampOff();
            BulLampOff();
            LeftRearTurnOff();
            RightRearTurnOff();
            ResetAnimation();
        }

        private void ResetAnimation()
        {
            _matrixValDefinitions[SBBR_CH_LH_Animation].Value = 0x00;
            //AllLedOff();
            //SwitchSignal(LampOnOffType.SbbrChLhAnimationOff);
            //SwitchSignal(LampOnOffType.Bcm1ChAktivOff);
            //SwitchSignal(LampOnOffType.Bcm1LhAktivOff);
        }

        #endregion

        #region 版本信息

        private string _lampType = "RIGHT RCLA";

        [Description("设置为RIGHT RCLA")]
        public void SetRightRcla()
        {
            _lampType = "RIGHT RCLA";
        }

        [Description("设置为LEFT RCLA")]
        public void SetLeftRcla()
        {
            _lampType = "LEFT RCLA";
        }

        [Description("设置为RIGHT RCLB")]
        public void SetRightRclb()
        {
            _lampType = "RIGHT RCLB";
        }

        [Description("设置为LEFT RCLB")]
        public void SetRigthRclb()
        {
            _lampType = "LEFT RCLB";
        }

        [Description("R,VW从节点零件号")]
        public string VW从节点零件号 = string.Empty;
        [Description("R,VW从节点软件版本号")]
        public string VW从节点软件版本号 = string.Empty;
        [Description("R,VW从节点硬件序号")]
        public string VW从节点硬件序号 = string.Empty;
        [Description("R,VW从节点硬件版本号")]
        public string VW从节点硬件版本号 = string.Empty;
        [Description("R,VW从节点序列号")]
        public string VW从节点序列号 = string.Empty;
        [Description("R,VW从节点系统名")]
        public string VW从节点系统名 = string.Empty;
        [Description("R,VWFAZIT标识")]
        public string VWFAZIT标识 = string.Empty;
        [Description("R,VW从节点编码序号")]
        public string VW从节点编码序号 = string.Empty;
        [Description("R,HASCO从节点硬件版本号")]
        public string HASCO从节点硬件版本号 = string.Empty;
        [Description("R,HASCO从节点总成零件号")]
        public string HASCO从节点总成零件号 = string.Empty;
        [Description("R,HASCO从节点软件版本号")]
        public string HASCO从节点软件版本号 = string.Empty;
        [Description("R,HASCO从节点软件零件号")]
        public string HASCO从节点软件零件号 = string.Empty;

        [Description("ReadVer")]
        public void ReadVer()
        {
            VW从节点零件号 = string.Empty;
            VW从节点软件版本号 = string.Empty;
            VW从节点硬件序号 = string.Empty;
            VW从节点硬件版本号 = string.Empty;
            VW从节点序列号 = string.Empty;
            VW从节点系统名 = string.Empty;
            VWFAZIT标识 = string.Empty;
            VW从节点编码序号 = string.Empty;
            HASCO从节点硬件版本号 = string.Empty;
            HASCO从节点总成零件号 = string.Empty;
            HASCO从节点软件版本号 = string.Empty;
            HASCO从节点软件零件号 = string.Empty;

            byte nad;
            switch (_lampType)
            {
                case "RIGHT RCLA":
                    nad = 0x68;
                    VW从节点零件号 = ReadDid(11, 0x63, 0x0F, nad, "ASCII");
                    VW从节点软件版本号 = ReadDid(4, 0x65, 0x0F, nad, "ASCII");
                    VW从节点硬件序号 = ReadDid(11, 0x67, 0x0F, nad, "ASCII");
                    VW从节点硬件版本号 = ReadDid(3, 0x69, 0x0F, nad, "ASCII");
                    VW从节点序列号 = ReadDid(20, 0x6B, 0x0F, nad, "ASCII");
                    VW从节点系统名 = ReadDid(13, 0x6D, 0x0F, nad, "ASCII");
                    VWFAZIT标识 = ReadDid(23, 0x6F, 0x0F, nad, "ASCII");
                    VW从节点编码序号 = ReadDid(3, 0x61, 0x0F, nad, "ASCII");
                    break;

                case "LEFT RCLA":
                    nad = 0x67;
                    VW从节点零件号 = ReadDid(11, 0x62, 0x25, nad, "ASCII");
                    VW从节点软件版本号 = ReadDid(4, 0x64, 0x25, nad, "ASCII");
                    VW从节点硬件序号 = ReadDid(11, 0x66, 0x25, nad, "ASCII");
                    VW从节点硬件版本号 = ReadDid(3, 0x68, 0x25, nad, "ASCII");
                    VW从节点序列号 = ReadDid(20, 0x6A, 0x25, nad, "ASCII");
                    VW从节点系统名 = ReadDid(13, 0x6C, 0x25, nad, "ASCII");
                    VWFAZIT标识 = ReadDid(23, 0x6E, 0x25, nad, "ASCII");
                    VW从节点编码序号 = ReadDid(3, 0x60, 0x25, nad, "ASCII");
                    break;

                case "RIGHT RCLB":
                    nad = 0x66;
                    VW从节点零件号 = ReadDid(11, 0x63, 0x10, nad, "ASCII");
                    VW从节点软件版本号 = ReadDid(4, 0x65, 0x10, nad, "ASCII");
                    VW从节点硬件序号 = ReadDid(11, 0x67, 0x10, nad, "ASCII");
                    VW从节点硬件版本号 = ReadDid(3, 0x69, 0x10, nad, "ASCII");
                    VW从节点序列号 = ReadDid(20, 0x6B, 0x10, nad, "ASCII");
                    VW从节点系统名 = ReadDid(13, 0x6D, 0x10, nad, "ASCII");
                    VWFAZIT标识 = ReadDid(23, 0x6F, 0x10, nad, "ASCII");
                    VW从节点编码序号 = ReadDid(3, 0x61, 0x10, nad, "ASCII");
                    break;

                case "LEFT RCLB":
                    nad = 0x65;
                    VW从节点零件号 = ReadDid(11, 0x63, 0x0E, nad, "ASCII");
                    VW从节点软件版本号 = ReadDid(4, 0x65, 0x0E, nad, "ASCII");
                    VW从节点硬件序号 = ReadDid(11, 0x67, 0x0E, nad, "ASCII");
                    VW从节点硬件版本号 = ReadDid(3, 0x69, 0x0E, nad, "ASCII");
                    VW从节点序列号 = ReadDid(20, 0x6B, 0x0E, nad, "ASCII");
                    VW从节点系统名 = ReadDid(13, 0x6D, 0x0E, nad, "ASCII");
                    VWFAZIT标识 = ReadDid(23, 0x6F, 0x0E, nad, "ASCII");
                    VW从节点编码序号 = ReadDid(3, 0x61, 0x0E, nad, "ASCII");
                    break;

                default:
                    return;
            }

            HASCO从节点硬件版本号 = ReadDid(3, 0xF1, 0x92, nad, "ASCII");
            HASCO从节点总成零件号 = ReadDid(9, 0xF1, 0x93, nad, "ASCII");
            HASCO从节点软件版本号 = ReadDid(20, 0xF1, 0x94, nad, "ASCII");
            HASCO从节点软件零件号 = ReadDid(9, 0xF1, 0x95, nad, "ASCII");
        }

        private string ReadDid(int dataLen, byte didHi, byte didLo, byte nad, string dataType)
        {
            if (Lin19200 == null)
            {
                return string.Empty;
            }

            var readValueDataLen = dataLen;
            var data0 = nad;

            //Console.WriteLine("config: " + infoName);
            //Console.WriteLine("config: " + standard);

            try
            {
                lock (_lockLin)
                {
                    var sendBytes = new byte[] { data0, 0x03, 0x22, didHi, didLo, 0xFF, 0xFF, 0xFF };
                    Lin19200.SendMasterLin(0x3C, sendBytes);

                    var resultBs = new List<byte>();

                    Thread.Sleep(50);
                    byte[] recv;
                    var isReadFirstFrameSucceed = Lin19200.SendSlaveLin(0x3D, out recv);
                    if (isReadFirstFrameSucceed && recv != null && recv.Length == 8)
                    {
                        if (recv[0] == data0)
                        {
                            if (recv[1] >= 0x10) // 多帧
                            {
                                if (recv[3] == 0x62 && recv[4] == didHi && recv[5] == didLo)
                                {
                                    resultBs.Add(recv[6]);
                                    resultBs.Add(recv[7]);
                                    var len = (recv[1] - 0x10) * 256 + recv[2];

                                    int count;
                                    if ((len - 5) % 6 == 0)
                                        count = (len - 5) / 6;
                                    else
                                        count = (len - 5) / 6 + 1;

                                    for (var i = 0; i < count; i++)
                                    {
                                        byte[] recvBytesRest;
                                        var isSucceed = Lin19200.SendSlaveLin(0x3D, out recvBytesRest);
                                        if (isSucceed && recvBytesRest != null && recvBytesRest.Length == 8)
                                        {
                                            for (var j = 2; j < 8; j++)
                                                resultBs.Add(recvBytesRest[j]);
                                        }
                                    }
                                }
                            }
                            else // 单帧
                            {
                                if (recv[2] == 0x62 && recv[3] == didHi && recv[4] == didLo)
                                {
                                    for (var i = 5; i < 5 + dataLen; i++)
                                    {
                                        resultBs.Add(recv[i]);
                                    }
                                }
                            }
                        }

                        if (resultBs.Any() && resultBs.Count >= readValueDataLen)
                        {
                            var temp3333 = new byte[readValueDataLen];
                            Array.Copy(resultBs.ToArray(), 0, temp3333, 0, readValueDataLen);
                            resultBs.Clear();
                            resultBs.AddRange(temp3333);
                        }

                        var getStr = string.Empty;
                        if (string.Equals(dataType, "ASCII", StringComparison.CurrentCultureIgnoreCase))
                            getStr = Encoding.ASCII.GetString(resultBs.ToArray());
                        else if (string.Equals(dataType, "Hex", StringComparison.CurrentCultureIgnoreCase))
                            getStr = resultBs.Aggregate(getStr, (current, t) => current + ValueHelper.GetHextStr(t));
                        return getStr;
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }

        #endregion

        #region 内部函数逻辑
        public bool IsSleep = true;
        private readonly Thread _th;
        private readonly object _lockLin = new object();
        private int _bzCount;

        private void MainWork()
        {
            var sendCount = 0;

            while (_th.IsAlive)
            {
                if (!_th.IsAlive)
                    break;

                Thread.Sleep(25);

                if (Lin19200 == null)
                    continue;

                if (IsSleep)
                {
                    _bzCount = 0;
                    continue;
                }

                Count();
                foreach (var key in _matrixValDefinitions.Keys)
                    _intelMatrix.UpdateData(_matrixValDefinitions[key]);
                var crc = Calculate_CRC(_intelMatrix.MatrixData, 7);
                _intelMatrix.MatrixData[0] = crc;
                lock (_lockLin)
                    Lin19200.SendMasterLin(_intelMatrix.MasterLinId, _intelMatrix.MatrixData);

                //Thread.Sleep(10);

                lock (_lockLin)
                {
                    switch (sendCount)
                    {
                        case 0:
                            Lin19200.SendMasterLin(0x0B, new byte[0]);
                            break;

                        case 1:
                            Lin19200.SendMasterLin(0x0C, new byte[0]);
                            break;

                        case 2:
                            Lin19200.SendMasterLin(0x0D, new byte[0]);
                            break;

                        case 3:
                            Lin19200.SendMasterLin(0x0E, new byte[0]);
                            break;
                    }
                }

                sendCount++;
                if (sendCount == 4)
                    sendCount = 0;

                //Lin19200.SendMasterLin(0x0A, new byte[] { 0x00, 0x80, 0x00, 0x00, 0xE0, 0xB8, 0x0D, 0x03 });
            }
        }
        #endregion

        #region 信号矩阵

        private readonly LinCommunicationMatrix.IntelMatrix _intelMatrix = new LinCommunicationMatrix.IntelMatrix(0x0A, 8);
        private readonly Dictionary<string, MatrixValDefinition> _matrixValDefinitions = new Dictionary<string, MatrixValDefinition>();

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

        #endregion
    }
}
