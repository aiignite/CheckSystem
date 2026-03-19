using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace Controller
{
    [Description("LIN-Product,VW316组合后灯-lin波特率19200")]
    public sealed class Vw316RearLamp : ControllerBase
    {
        public LinBus Lin19200;

        public Vw316RearLamp(string name)
            : base(name)
        {
            foreach (var temp
               in Enum.GetValues(typeof(LampOnOffType)).Cast<LampOnOffType>())
                _lampOperaterDic.Add(temp, temp.GetCustomAttribute<MatrixValDefinition>());

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

        ~Vw316RearLamp()
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

        [Description("离家动画")]
        public void LevingHome()
        {
            AllLedOff();

            // LHO动画的触发需要位置灯信号状态从0变为1，与此同时发送动画模式与动画类型信号
            // Step1：LHO动画触发前需要先发送如下信号
            SwitchSignal(LampOnOffType.SbbrSchlusslichtAnfInvOn);
            SwitchSignal(LampOnOffType.SbbrSchlusslichtAnfOff);
            SwitchSignal(LampOnOffType.ZasKl15Off);
            SwitchSignal(LampOnOffType.ZasKl15InvOn);
            SwitchSignal(LampOnOffType.SbbrShlStatischStatusOff);
            SwitchSignal(LampOnOffType.SbbrChLhAnimationOff);

            Thread.Sleep(500);

            // Step2：转向灯动态时序2次，并且发送如下动画信号
            SwitchSignal(LampOnOffType.SbbrSchlusslichtAnfInvOff);
            SwitchSignal(LampOnOffType.SbbrSchlusslichtAnfOn);
            SwitchSignal(LampOnOffType.ZasKl15Off);
            SwitchSignal(LampOnOffType.ZasKl15InvOn);
            SwitchSignal(LampOnOffType.SbbrShlStatischStatusOff);
            SwitchSignal(LampOnOffType.SbbrChLhAnimationOn);
        }

        [Description("回家动画")]
        public void ComingHome()
        {
            AllLedOff();

            // CHO动画的触发需要位置灯信号状态从1变为0，与此同时发送动画模式与动画类型信号
            // Step1：CHO动画触发前需要先发送如下信号
            SwitchSignal(LampOnOffType.SbbrSchlusslichtAnfInvOff);
            SwitchSignal(LampOnOffType.SbbrSchlusslichtAnfOn);
            SwitchSignal(LampOnOffType.ZasKl15Off);
            SwitchSignal(LampOnOffType.ZasKl15InvOn);
            SwitchSignal(LampOnOffType.SbbrShlStatischStatusOff);
            SwitchSignal(LampOnOffType.SbbrChLhAnimationOff);

            Thread.Sleep(500);

            // Step2：转向灯动态时序2次，并且发送如下动画信号
            SwitchSignal(LampOnOffType.SbbrSchlusslichtAnfInvOn);
            SwitchSignal(LampOnOffType.SbbrSchlusslichtAnfOff);
            SwitchSignal(LampOnOffType.ZasKl15Off);
            SwitchSignal(LampOnOffType.ZasKl15InvOn);
            SwitchSignal(LampOnOffType.SbbrShlStatischStatusOff);
            SwitchSignal(LampOnOffType.SbbrChLhAnimationOn);
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
            Kl15On();
            ResetAnimation();
        }

        private void ResetAnimation()
        {
            //AllLedOff();
            SwitchSignal(LampOnOffType.SbbrChLhAnimationOff);
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
            SbbrChLhAnimationOn,

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
            SbbrVarianteOn
        }
        #endregion
    }
}
