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
    [Description("LIN-Product,VW413组合后灯-lin波特率19200")]
    public sealed class Vw413RearLamp : ControllerBase
    {
        public LinBus Lin19200;

        public Vw413RearLamp(string name)
            : base(name)
        {
            foreach (var temp
               in Enum.GetValues(typeof(LampOnOffType)).Cast<LampOnOffType>())
                _lampOperaterDic.Add(temp, temp.GetCustomAttribute<MatrixValDefinition>());

            Kl15Off();
            RearLampOff();

            _th = new Thread(MainWork) { IsBackground = true };
            _th.Start();
        }

        ~Vw413RearLamp()
        {
            Dispose();
        }

        #region 点灯相关

        private void Count()
        {
            _lampOperaterDic[LampOnOffType.BcmSbbr01Bz].Value++;
            if (_lampOperaterDic[LampOnOffType.BcmSbbr01Bz].Value > 15)
                _lampOperaterDic[LampOnOffType.BcmSbbr01Bz].Value = 0;
            MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.BcmSbbr01Bz]);
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

        [Description("KL15开关ON")]
        public void Kl15On()
        {
            MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.ZasKl15On]);
            MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.ZasKl15InvOff]);
        }

        [Description("KL15开关OFF")]
        public void Kl15Off()
        {
            MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.ZasKl15Off]);
            MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.ZasKl15InvOn]);
        }

        /// <summary>
        /// 制动灯亮
        /// </summary>
        [Description("制动灯亮")]
        public void StopOn()
        {
            MotorolaMatrix0X0A.UpdateData(
                _lampOperaterDic[LampOnOffType.SbbrBrlAnfOn]);
        }

        /// <summary>
        /// 制动灯灭
        /// </summary>
        [Description("制动灯灭")]
        public void StopOff()
        {
            MotorolaMatrix0X0A.UpdateData(
                _lampOperaterDic[LampOnOffType.SbbrBrlAnfOff]);
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
            if (isOn)
            {
                MotorolaMatrix0X0A.UpdateData(
                    _lampOperaterDic[LampOnOffType.SbbrParklichtLiAnfOn]);
                MotorolaMatrix0X0A.UpdateData(
                    _lampOperaterDic[LampOnOffType.SbbrParklichtReAnfOn]);
            }
            else
            {
                MotorolaMatrix0X0A.UpdateData(
                    _lampOperaterDic[LampOnOffType.SbbrParklichtLiAnfOff]);
                MotorolaMatrix0X0A.UpdateData(
                    _lampOperaterDic[LampOnOffType.SbbrParklichtReAnfOff]);
            }
        }

        [Description("尾门信号开")]
        public void TailGateOn() => MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.ZvHdOffenOn]);

        [Description("尾门信号关")]
        public void TailGateOff() => MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.ZvHdOffenOff]);

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
            if (isOn)
            {
                MotorolaMatrix0X0A.UpdateData(
                    _lampOperaterDic[LampOnOffType.SbbrSchlusslichtAnfOn]);
                MotorolaMatrix0X0A.UpdateData(
                    _lampOperaterDic[LampOnOffType.SbbrSchlusslichtAnfInvOff]);
            }
            else
            {
                MotorolaMatrix0X0A.UpdateData(
                    _lampOperaterDic[LampOnOffType.SbbrSchlusslichtAnfOff]);
                MotorolaMatrix0X0A.UpdateData(
                    _lampOperaterDic[LampOnOffType.SbbrSchlusslichtAnfInvOn]);
            }
        }

        private void SwitchRearLampHd(bool isOn)
        {
            MotorolaMatrix0X0A.UpdateData(isOn
                ? _lampOperaterDic[LampOnOffType.ZvHdOffenOn]
                : _lampOperaterDic[LampOnOffType.ZvHdOffenOff]);
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
            MotorolaMatrix0X0A.UpdateData(isOn
               ? _lampOperaterDic[LampOnOffType.SbbrRueckfahrlichtAnfOn]
               : _lampOperaterDic[LampOnOffType.SbbrRueckfahrlichtAnfOff]);
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
            MotorolaMatrix0X0A.UpdateData(isOn
              ? _lampOperaterDic[LampOnOffType.FogLampOn]
              : _lampOperaterDic[LampOnOffType.FogLampOff]);
        }

        /// <summary>
        /// 左转向灯顺序亮
        /// </summary>
        [Description("左转向灯顺序亮")]
        public void LeftRearTurnRunningOn()
        {
            SwitchLeftTurnFlicker(false);
            SwitchLeftRearTurnRunningLamp(true);
        }

        private void SwitchLeftRearTurnRunningLamp(bool isOn)
        {
            if (isOn)
            {
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiLinksOn]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiRechtsOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraLinksOn]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraRechtsOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiAnimationOn]);
            }
            else
            {
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiLinksOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiRechtsOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraLinksOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraRechtsOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiAnimationOff]);
            }
        }

        /// <summary>
        /// 左转向灯闪烁亮
        /// </summary>
        [Description("左转向灯闪烁亮")]
        public void LeftRearTurnFlickerOn()
        {
            SwitchLeftRearTurnRunningLamp(false);
            SwitchLeftTurnFlicker(true);
        }

        [Description("左转向灯灭")]
        public void LeftRearTurnOff()
        {
            SwitchRightTurnFlicker(false);
            SwitchLeftRearTurnRunningLamp(false);
        }

        private void SwitchLeftTurnFlicker(bool isOn)
        {
            if (isOn)
            {
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiLinksOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiRechtsOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraLinksOn]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraRechtsOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiAnimationOff]);
            }
            else
            {
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiLinksOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiRechtsOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraLinksOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraRechtsOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiAnimationOff]);
            }
        }

        [Description("右转向灯顺序亮")]
        public void RightRearTurnRunningOn()
        {
            SwitchRightTurnFlicker(false);
            SwitchRightRearTurnRunningLamp(true);
        }

        private void SwitchRightRearTurnRunningLamp(bool isOn)
        {
            if (isOn)
            {
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiLinksOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiRechtsOn]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraLinksOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraRechtsOn]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiAnimationOn]);
            }
            else
            {
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiLinksOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiRechtsOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraLinksOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraRechtsOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiAnimationOff]);
            }
        }

        [Description("右转向灯闪烁亮")]
        public void RightRearTurnFlickerOn()
        {
            SwitchRightRearTurnRunningLamp(false);
            SwitchRightTurnFlicker(true);
        }

        [Description("右转向灯灭")]
        public void RightRearTurnOff()
        {
            SwitchRightTurnFlicker(false);
            SwitchRightRearTurnRunningLamp(false);
        }

        private void SwitchRightTurnFlicker(bool isOn)
        {
            if (isOn)
            {
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiLinksOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiRechtsOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraLinksOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraRechtsOn]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiAnimationOff]);
            }
            else
            {
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiLinksOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiRechtsOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraLinksOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraRechtsOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiAnimationOff]);
            }
        }

        private void SwitchSignal(LampOnOffType type)
        {
            MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[type]);
        }

        public int AnimationIndex = 2;

        [Description("离家动画")]
        public void LevingHome()
        {
            AllLedOff();
            //Thread.Sleep(10);
            //SwitchSignal(LampOnOffType.Bcm1LhAktivOn);
            SwitchSignal(LampOnOffType.SbbrChLhAnimationOff);
            RearLampOff();
            Thread.Sleep(150);

            switch (AnimationIndex)
            {
                case 1:
                    SwitchSignal(LampOnOffType.SbbrChLhAnimationOn1);
                    break;

                case 2:
                    SwitchSignal(LampOnOffType.SbbrChLhAnimationOn2);
                    break;

                case 3:
                    SwitchSignal(LampOnOffType.SbbrChLhAnimationOn3);
                    break;

                default:
                    break;
            }

            Thread.Sleep(100);
            RearLampOn();
            //SwitchSignal(LampOnOffType.Bcm1LhAktivOn);
            //SwitchSignal(LampOnOffType.Bcm1ChAktivOff);
        }

        [Description("回家动画")]
        public void ComingHome()
        {
            AllLedOff();
            //SwitchSignal(LampOnOffType.Bcm1ChAktivOn);
            SwitchSignal(LampOnOffType.SbbrChLhAnimationOff);
            RearLampOn();
            Thread.Sleep(150);

            switch (AnimationIndex)
            {
                case 1:
                    SwitchSignal(LampOnOffType.SbbrChLhAnimationOn1);
                    break;

                case 2:
                    SwitchSignal(LampOnOffType.SbbrChLhAnimationOn2);
                    break;

                case 3:
                    SwitchSignal(LampOnOffType.SbbrChLhAnimationOn3);
                    break;

                default:
                    break;
            }

            Thread.Sleep(100);
            RearLampOff();
            //SwitchSignal(LampOnOffType.Bcm1LhAktivOff);
            //SwitchSignal(LampOnOffType.Bcm1ChAktivOn);
        }

        private int _lastAnimationIndex = 0;

        [Description("离家动画-由SchlusslichtSignatur信号触发")]
        public void LeavingHomeTriggerBySchlusslichtSignatur(int type)
        {
            if (type != 1 && type != 2 && type != 3)
                return;

            AllLedOff();
            Thread.Sleep(25);
            SwitchSignal(LampOnOffType.SbbrChLhAnimationOff);
            switch (_lastAnimationIndex)
            {
                case 1:
                    SwitchSignal(LampOnOffType.SbbrSchlusslichtSignaturOn2);
                    break;

                case 2:
                    SwitchSignal(LampOnOffType.SbbrSchlusslichtSignaturOn3);
                    break;

                case 3:
                    SwitchSignal(LampOnOffType.SbbrSchlusslichtSignaturOn1);
                    break;

                default:
                    break;
            }
            Thread.Sleep(20);
            SwitchSignal(LampOnOffType.SbbrSchlusslichtSignaturOff);
            RearLampOff();
            Thread.Sleep(150);

            SwitchSignal(LampOnOffType.SbbrChLhAnimationOn1);
            switch (type)
            {
                case 1:
                    SwitchSignal(LampOnOffType.SbbrSchlusslichtSignaturOn1);
                    _lastAnimationIndex = 1;
                    break;

                case 2:
                    SwitchSignal(LampOnOffType.SbbrSchlusslichtSignaturOn2);
                    _lastAnimationIndex = 2;
                    break;

                case 3:
                    SwitchSignal(LampOnOffType.SbbrSchlusslichtSignaturOn3);
                    _lastAnimationIndex = 3;
                    break;
            }

            Thread.Sleep(155);
            RearLampOn();
        }

        [Description("回家动画-由SchlusslichtSignatur信号触发")]
        public void ComingHomeTriggerBySchlusslichtSignatur(int type)
        {
            if (type != 1 && type != 2 && type != 3)
                return;

            AllLedOff();
            Thread.Sleep(25);
            SwitchSignal(LampOnOffType.SbbrChLhAnimationOff);
            switch (_lastAnimationIndex)
            {
                case 1:
                    SwitchSignal(LampOnOffType.SbbrSchlusslichtSignaturOn2);
                    break;

                case 2:
                    SwitchSignal(LampOnOffType.SbbrSchlusslichtSignaturOn3);
                    break;

                case 3:
                    SwitchSignal(LampOnOffType.SbbrSchlusslichtSignaturOn1);
                    break;

                default:
                    break;
            }
            Thread.Sleep(50);
            SwitchSignal(LampOnOffType.SbbrSchlusslichtSignaturOff);
            RearLampOn();
            Thread.Sleep(150);

            SwitchSignal(LampOnOffType.SbbrChLhAnimationOn1);
            switch (type)
            {
                case 1:
                    SwitchSignal(LampOnOffType.SbbrSchlusslichtSignaturOn1);
                    _lastAnimationIndex = 1;
                    break;

                case 2:
                    SwitchSignal(LampOnOffType.SbbrSchlusslichtSignaturOn2);
                    _lastAnimationIndex = 2;
                    break;

                case 3:
                    SwitchSignal(LampOnOffType.SbbrSchlusslichtSignaturOn3);
                    _lastAnimationIndex = 3;
                    break;
            }

            Thread.Sleep(155);
            RearLampOff();
            //SwitchSignal(LampOnOffType.Bcm1LhAktivOff);
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
            Kl15Off();
            ResetAnimation();
        }

        private void ResetAnimation()
        {
            //AllLedOff();
            SwitchSignal(LampOnOffType.SbbrChLhAnimationOff);
            SwitchSignal(LampOnOffType.Bcm1ChAktivOff);
            SwitchSignal(LampOnOffType.Bcm1LhAktivOff);
            SwitchSignal(LampOnOffType.SbbrSchlusslichtSignaturOff);
            Thread.Sleep(50);
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
                var sendBytes = new byte[] { data0, 0x03, 0x22, didHi, didLo, 0xFF, 0xFF, 0xFF };
                Lin19200.SendMasterLin(0x3C, sendBytes);

                var resultBs = new List<byte>();

                Thread.Sleep(100);
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

        private readonly Dictionary<LampOnOffType, MatrixValDefinition> _lampOperaterDic =
            new Dictionary<LampOnOffType, MatrixValDefinition>();

        public readonly LinCommunicationMatrix.IntelMatrix MotorolaMatrix0X0A =
            new LinCommunicationMatrix.IntelMatrix(0x0A, 8);

        private void MainWork()
        {
            while (_th.IsAlive)
            {
                if (!_th.IsAlive)
                    break;

                Thread.Sleep(25);

                if (Lin19200 == null)
                    continue;

                Count();

                if (IsSleep)
                    continue;

                Lin19200.SendMasterLin(
                   MotorolaMatrix0X0A.MasterLinId, MotorolaMatrix0X0A.MatrixData);
            }
        }

        internal enum LampOnOffType
        {
            ///// <summary>
            ///// 左转向灯时序点亮启动
            ///// </summary>
            //[MatrixValDefinition(16, 1, 1)]
            //[Description("0x0A")]
            //LeftRearTurnRunningOn,

            ///// <summary>
            ///// 左转向灯时序点亮结束
            ///// </summary>
            //[MatrixValDefinition(16, 1, 0)]
            //[Description("0x0A")]
            //LeftRearTurnRunningOff,

            ///// <summary>
            ///// 右转向灯时序点亮启动
            ///// </summary>
            //[MatrixValDefinition(17, 1, 1)]
            //[Description("0x0A")]
            //RightRearTurnRunningOn,

            ///// <summary>
            ///// 右转向灯时序点亮结束
            ///// </summary>
            //[MatrixValDefinition(17, 1, 0)]
            //[Description("0x0A")]
            //RightRearTurnRunningOff,

            ///// <summary>
            ///// 左转向灯闪烁点亮启动
            ///// </summary>
            //[MatrixValDefinition(18, 1, 1)]
            //[Description("0x0A")]
            //LeftRearTurnFlickerOn,

            ///// <summary>
            ///// 左转向灯闪烁点亮结束
            ///// </summary>
            //[MatrixValDefinition(18, 1, 0)]
            //[Description("0x0A")]
            //LeftRearTurnFlickerOff,

            ///// <summary>
            ///// 右转向灯闪烁点亮启动
            ///// </summary>
            //[MatrixValDefinition(21, 1, 1)]
            //[Description("0x0A")]
            //RigthRearTurnFlickerOn,

            ///// <summary>
            ///// 右转向灯闪烁点亮结束
            ///// </summary>
            //[MatrixValDefinition(21, 1, 0)]
            //[Description("0x0A")]
            //RigthRearTurnFlickerOff,

            /// <summary>
            /// 位置灯的开关信号
            /// </summary>
            [MatrixValDefinition(48, 1, 1)]
            [Description("0x0A")]
            SbbrSchlusslichtAnfInvOn,

            /// <summary>
            /// 位置灯的开关信号
            /// </summary>
            [MatrixValDefinition(48, 1, 0)]
            [Description("0x0A")]
            SbbrSchlusslichtAnfInvOff,

            /// <summary>
            /// 位置灯的开关信号
            /// </summary>
            [MatrixValDefinition(49, 1, 1)]
            [Description("0x0A")]
            SbbrSchlusslichtAnfOn,

            /// <summary>
            /// 位置灯的开关信号
            /// </summary>
            [MatrixValDefinition(49, 1, 0)]
            [Description("0x0A")]
            SbbrSchlusslichtAnfOff,

            /// <summary>
            /// KL15的开关信号
            /// </summary>
            [MatrixValDefinition(12, 1, 1)]
            [Description("0x0A")]
            ZasKl15On,

            /// <summary>
            /// KL15的开关信号
            /// </summary>
            [MatrixValDefinition(12, 1, 0)]
            [Description("0x0A")]
            ZasKl15Off,

            /// <summary>
            /// KL15的开关信号
            /// </summary>
            [MatrixValDefinition(15, 1, 1)]
            [Description("0x0A")]
            ZasKl15InvOn,

            /// <summary>
            /// KL15的开关信号
            /// </summary>
            [MatrixValDefinition(15, 1, 0)]
            [Description("0x0A")]
            ZasKl15InvOff,

            /// <summary>
            /// 位置灯静态开关
            /// </summary>
            [MatrixValDefinition(43, 1, 1)]
            [Description("0x0A")]
            SbbrShlStatischStatusOn,

            /// <summary>
            /// 位置灯静态开关
            /// </summary>
            [MatrixValDefinition(43, 1, 0)]
            [Description("0x0A")]
            SbbrShlStatischStatusOff,

            /// <summary>
            /// 动画模式
            /// </summary>
            [MatrixValDefinition(30, 2, 1)]
            [Description("0x0A")]
            SbbrChLhAnimationOn1,

            /// <summary>
            /// 动画模式
            /// </summary>
            [MatrixValDefinition(30, 2, 2)]
            [Description("0x0A")]
            SbbrChLhAnimationOn2,

            /// <summary>
            /// 动画模式
            /// </summary>
            [MatrixValDefinition(30, 2, 3)]
            [Description("0x0A")]
            SbbrChLhAnimationOn3,

            /// <summary>
            /// 动画模式
            /// </summary>
            [MatrixValDefinition(30, 2, 0)]
            [Description("0x0A")]
            SbbrChLhAnimationOff,

            /// <summary>
            /// 尾灯高亮
            /// </summary>
            [MatrixValDefinition(14, 1, 0)]
            [Description("0x0A")]
            ZvHdOffenOff,

            /// <summary>
            /// 尾灯高亮
            /// </summary>
            [MatrixValDefinition(14, 1, 1)]
            [Description("0x0A")]
            ZvHdOffenOn,

            /// <summary>
            /// 倒车灯
            /// </summary>
            [MatrixValDefinition(52, 1, 1)]
            [Description("0x0A")]
            SbbrRueckfahrlichtAnfOn,

            /// <summary>
            /// 倒车灯
            /// </summary>
            [MatrixValDefinition(52, 1, 0)]
            [Description("0x0A")]
            SbbrRueckfahrlichtAnfOff,

            /// <summary>
            /// 雾灯亮
            /// </summary>
            [MatrixValDefinition(53, 1, 1)]
            [Description("0x0A")]
            FogLampOn,

            /// <summary>
            /// 雾灯灭
            /// </summary>
            [MatrixValDefinition(53, 1, 0)]
            [Description("0x0A")]
            FogLampOff,

            /// <summary>
            /// 制动灯灭
            /// </summary>
            [MatrixValDefinition(32, 1, 0)]
            [Description("0x0A")]
            SbbrBrlAnfOff,

            /// <summary>
            /// 制动灯亮
            /// </summary>
            [MatrixValDefinition(32, 1, 1)]
            [Description("0x0A")]
            SbbrBrlAnfOn,

            /// <summary>
            /// 计数器
            /// </summary>
            [MatrixValDefinition(8, 4, 0)]
            [Description("0x0A")]
            BcmSbbr01Bz,

            /// <summary>
            /// 左转向
            /// </summary>
            [MatrixValDefinition(16, 1, 0)]
            [Description("0x0A")]
            SbbrFraWiBiLinksOff,

            /// <summary>
            /// 左转向
            /// </summary>
            [MatrixValDefinition(16, 1, 1)]
            [Description("0x0A")]
            SbbrFraWiBiLinksOn,

            /// <summary>
            /// 左转向
            /// </summary>
            [MatrixValDefinition(17, 1, 0)]
            [Description("0x0A")]
            SbbrFraWiBiRechtsOff,

            /// <summary>
            /// 左转向
            /// </summary>
            [MatrixValDefinition(17, 1, 1)]
            [Description("0x0A")]
            SbbrFraWiBiRechtsOn,

            /// <summary>
            /// 左转向
            /// </summary>
            [MatrixValDefinition(20, 1, 0)]
            [Description("0x0A")]
            SbbrFraLinksOff,

            /// <summary>
            /// 左转向
            /// </summary>
            [MatrixValDefinition(20, 1, 1)]
            [Description("0x0A")]
            SbbrFraLinksOn,

            /// <summary>
            /// 左转向
            /// </summary>
            [MatrixValDefinition(21, 1, 0)]
            [Description("0x0A")]
            SbbrFraRechtsOff,

            /// <summary>
            /// 左转向
            /// </summary>
            [MatrixValDefinition(21, 1, 1)]
            [Description("0x0A")]
            SbbrFraRechtsOn,

            /// <summary>
            /// 左转向
            /// </summary>
            [MatrixValDefinition(22, 2, 0)]
            [Description("0x0A")]
            SbbrFraWiBiAnimationOff,

            /// <summary>
            /// 左转向
            /// </summary>
            [MatrixValDefinition(22, 1, 1)]
            [Description("0x0A")]
            SbbrFraWiBiAnimationOn,

            [MatrixValDefinition(24, 1, 0)]
            [Description("0x0A")]
            Bcm1ChAktivOff,

            [MatrixValDefinition(24, 1, 1)]
            [Description("0x0A")]
            Bcm1ChAktivOn,

            [MatrixValDefinition(25, 1, 0)]
            [Description("0x0A")]
            Bcm1LhAktivOff,

            [MatrixValDefinition(25, 1, 1)]
            [Description("0x0A")]
            Bcm1LhAktivOn,

            [MatrixValDefinition(26, 4, 0)]
            [Description("0x0A")]
            SbbrVarianteOff,

            [MatrixValDefinition(26, 4, 0)]
            [Description("0x0A")]
            SbbrVarianteOn,

            [MatrixValDefinition(50, 1, 1)]
            [Description("0x0A")]
            SbbrParklichtLiAnfOn,

            [MatrixValDefinition(51, 1, 1)]
            [Description("0x0A")]
            SbbrParklichtReAnfOn,

            [MatrixValDefinition(50, 1, 0)]
            [Description("0x0A")]
            SbbrParklichtLiAnfOff,

            [MatrixValDefinition(51, 1, 0)]
            [Description("0x0A")]
            SbbrParklichtReAnfOff,

            /// <summary>
            /// 动画模式
            /// </summary>
            [MatrixValDefinition(60, 4, 1)]
            [Description("0x0A")]
            SbbrSchlusslichtSignaturOn1,

            /// <summary>
            /// 动画模式
            /// </summary>
            [MatrixValDefinition(60, 4, 2)]
            [Description("0x0A")]
            SbbrSchlusslichtSignaturOn2,

            /// <summary>
            /// 动画模式
            /// </summary>
            [MatrixValDefinition(60, 4, 3)]
            [Description("0x0A")]
            SbbrSchlusslichtSignaturOn3,

            /// <summary>
            /// 动画模式
            /// </summary>
            [MatrixValDefinition(60, 4, 0)]
            [Description("0x0A")]
            SbbrSchlusslichtSignaturOff,
        }
        #endregion
    }
}
