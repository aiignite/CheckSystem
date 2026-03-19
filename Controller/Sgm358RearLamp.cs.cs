using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using CommonUtility;
using CommonUtility.BusLoader;

namespace Controller
{
    [Description("LIN-Product,SGM358后灯")]
    public sealed class Sgm358RearLamp : ControllerBase
    {
        public LinBus LinWithBaudRate10417;

        public Sgm358RearLamp(string name)
            : base(name)
        {
            foreach (var temp
               in Enum.GetValues(typeof(LampOnOffType)).Cast<LampOnOffType>())
                _lampOperaterDic.Add(temp, temp.GetCustomAttribute<MatrixValDefinition>());

            foreach (var t in Enum.GetValues(typeof(LampOnOffType)))
            {
                var memberInfo = GetType().GetField(t.ToString());
                if (memberInfo == null)
                    continue;
                memberInfo.SetValue(this, t);
            }

            _mainWorkThread = new Thread(CyNormalCyclicTimer);
            _mainWorkThread.Start();
        }

        ~Sgm358RearLamp()
        {
            Dispose();
        }

        [Description("LIN唤醒")]
        public void LampAwake()
        {
            _isSleep = false;
        }

        [Description("LIN休眠")]
        public void LampSleep()
        {
            _isSleep = true;
        }

        [Description("左后转向灯常亮")]
        public void LeftRearTurnLampHoldOn()
        {
            SwitchTurnLampSession(true, LampOnOffType.LeftRearTurnLampHoldOnLight);
        }

        [Description("左后转向灯熄灭")]
        public void LeftRearTurnLampOff()
        {
            SwitchTurnLampSession(true, LampOnOffType.LeftRearTurnLampNotActive);
        }

        [Description("左后转向灯时序点亮")]
        public void LeftRearTurnLampSwipeOn()
        {
            SwitchTurnLampSession(true, LampOnOffType.LeftRearTurnLampSwipeTurn);
        }

        //[Description("左后转向灯熄灭")]
        //public void LeftRearTurnLampSwipeOff()
        //{
        //    SwitchTurnLampSession(true, LampOnOffType.LeftRearTurnLampNotActive);
        //}

        [Description("左后转向灯1.33hz正常频率闪烁点亮")]
        public void LeftRearTurnLampNormalFlashOn()
        {
            SwitchTurnLampSession(true, LampOnOffType.LeftRearTurnLampNormalFlash);
        }

        //[Description("左后转向灯熄灭")]
        //public void LeftRearTurnLampNormalFlashOff()
        //{
        //    SwitchTurnLampSession(true, LampOnOffType.LeftRearTurnLampNotActive);
        //}

        [Description("左后转向灯1hz频率闪烁点亮")]
        public void LeftRearTurnLampNormalFlash1HzOn()
        {
            SwitchTurnLampSession(true, LampOnOffType.LeftRearTurnLampNormalFlash1Hz);
        }

        //[Description("")]
        //public void LeftRearTurnLampNormalFlash1HzOff()
        //{
        //    SwitchTurnLampSession(true, LampOnOffType.LeftRearTurnLampNotActive);
        //}

        [Description("左后转向灯3hz频率闪烁点亮")]
        public void LeftRearTurnLampNormalFlashMode3On()
        {
            SwitchTurnLampSession(true, LampOnOffType.LeftRearTurnLampNormalFlashMode3);
        }

        [Description("左后转向灯3hz频率闪烁点亮")]
        public void LeftRearTurnLampNormalFlashMode3Off()
        {
            SwitchTurnLampSession(true, LampOnOffType.LeftRearTurnLampNotActive);
        }

        [Description("左后转向灯3hz频率闪烁点亮")]
        public void LeftRearTurnLampFaultFlashOn()
        {
            SwitchTurnLampSession(true, LampOnOffType.LeftRearTurnLampFaultFlash);
        }

        //[Description("")]
        //public void LeftRearTurnLampFaultFlashOff()
        //{
        //    SwitchTurnLampSession(true, LampOnOffType.LeftRearTurnLampNotActive);
        //}

        private void SwitchTurnLampSession(bool isLeft, LampOnOffType lampOnOffType)
        {
            if (isLeft)
            {
                _currentLeftRearTurnLampSession = lampOnOffType;
                //_currentLeftRearTurnLampSessionIsNeedSwitch = true;
            }
            else
            {
                _currentRightRearTurnLampSession = lampOnOffType;
                //_currentRightRearTurnLampSessionIsNeedSwitch = true;
            }
        }

        [Description("右后转向灯常亮")]
        public void RightRearTurnLampHoldOn()
        {
            SwitchTurnLampSession(false, LampOnOffType.RightRearTurnLampHoldOnLight);
        }

        [Description("右后转向灯熄灭")]
        public void RightRearTurnLampOff()
        {
            SwitchTurnLampSession(false, LampOnOffType.RightRearTurnLampNotActive);
        }

        [Description("右后转向灯时序点亮")]
        public void RightRearTurnLampSwipeOn()
        {
            SwitchTurnLampSession(false, LampOnOffType.RightRearTurnLampSwipeTurn);
        }

        //[Description("")]
        //public void RightRearTurnLampSwipeOff()
        //{
        //    SwitchTurnLampSession(false, LampOnOffType.RightRearTurnLampNotActive);
        //}

        [Description("右后转向灯1.33hz正常频率闪烁点亮")]
        public void RightRearTurnLampNormalFlashOn()
        {
            SwitchTurnLampSession(false, LampOnOffType.RightRearTurnLampNormalFlash);
        }

        //[Description("")]
        //public void RightRearTurnLampNormalFlashOff()
        //{
        //    SwitchTurnLampSession(false, LampOnOffType.RightRearTurnLampNotActive);
        //}

        [Description("右后转向灯1hz频率闪烁点亮")]
        public void RightRearTurnLampNormalFlash1HzOn()
        {
            SwitchTurnLampSession(false, LampOnOffType.RightRearTurnLampNormalFlash1Hz);
        }

        //[Description("")]
        //public void RightRearTurnLampNormalFlash1HzOff()
        //{
        //    SwitchTurnLampSession(false, LampOnOffType.RightRearTurnLampNotActive);
        //}

        [Description("右后转向灯3hz频率闪烁点亮")]
        public void RightRearTurnLampNormalFlashMode3On()
        {
            SwitchTurnLampSession(false, LampOnOffType.RightRearTurnLampNormalFlashMode3);
        }

        //[Description("")]
        //public void RightRearTurnLampNormalFlashMode3Off()
        //{
        //    SwitchTurnLampSession(false, LampOnOffType.RightRearTurnLampNotActive);
        //}

        [Description("右后转向灯3hz频率闪烁点亮")]
        public void RightRearTurnLampFaultFlashOn()
        {
            SwitchTurnLampSession(false, LampOnOffType.RightRearTurnLampFaultFlash);
        }

        //[Description("")]
        //public void RightRearTurnLampFaultFlashOff()
        //{
        //    SwitchTurnLampSession(false, LampOnOffType.RightRearTurnLampNotActive);
        //}

        [Description("右尾灯打开")]
        public void RightRearParkLampOn()
        {
            SwitchRearParkLampSession(false, true);
        }

        [Description("右尾灯关闭")]
        public void RightRearParkLampOff()
        {
            SwitchRearParkLampSession(false, false);
        }

        [Description("左尾灯打开")]
        public void LeftRearParkLampOn()
        {
            SwitchRearParkLampSession(true, true);
        }

        [Description("左尾灯关闭")]
        public void LeftRearParkLampOff()
        {
            SwitchRearParkLampSession(true, false);
        }

        private void SwitchRearParkLampSession(bool isLeft, bool isOn)
        {
            if (isLeft)
            {
                _motorolaMatrix0X1A.UpdateData(
                    isOn
                    ? _lampOperaterDic[LampOnOffType.LeftRearParkLampOn]
                    : _lampOperaterDic[LampOnOffType.LeftRearParkLampOff]);
            }
            else
            {
                _motorolaMatrix0X1A.UpdateData(
                    isOn
                    ? _lampOperaterDic[LampOnOffType.RightRearParkLampOn]
                    : _lampOperaterDic[LampOnOffType.RightRearParkLampOff]);
            }
        }

        [Description("默认值等待或开始动画模式")]
        public void RrLmpShwNotActive()
        {
            _motorolaMatrix0X1A.UpdateData(
                _lampOperaterDic[LampOnOffType.RrLmpShwNotActive]);
        }

        [Description("工厂测试模式")]
        public void RrLmpShwFactoryMode()
        {
            _motorolaMatrix0X1A.UpdateData(
                _lampOperaterDic[LampOnOffType.RrLmpShwFactoryMode]);
        }

        [Description("默认值，等待灯光秀开始或结束")]
        public void RrLampShowTypNotActive()
        {
            _motorolaMatrix0X1A.UpdateData(
                _lampOperaterDic[LampOnOffType.RrLmpShwTypNotActive]);
        }

        [Description("停止或打断灯光秀")]
        public void RrLampShowTypAbortShow()
        {
            _motorolaMatrix0X1A.UpdateData(
                _lampOperaterDic[LampOnOffType.RrLmpShwTypAbortShow]);
        }

        [Description("白天引擎关")]
        public void RrLampShowTypEngineOffDay()
        {
            _motorolaMatrix0X1A.UpdateData(
                _lampOperaterDic[LampOnOffType.RrLmpShwTypEngineOffDay]);
        }

        [Description("晚上引擎关")]
        public void RrLmpShwTypEngineOffNight()
        {
            _motorolaMatrix0X1A.UpdateData(
                _lampOperaterDic[LampOnOffType.RrLmpShwTypEngineOffNight]);
        }

        [Description("引擎开")]
        public void RrLmpShwTypEngineOn()
        {
            _motorolaMatrix0X1A.UpdateData(
                _lampOperaterDic[LampOnOffType.RrLmpShwTypEngineOn]);
        }

        [Description("白天解锁")]
        public void RrLmpShwTypUnlockDay()
        {
            _motorolaMatrix0X1A.UpdateData(
                _lampOperaterDic[LampOnOffType.RrLmpShwTypUnlockDay]);
        }

        [Description("晚上解锁")]
        public void RrLmpShwTypUnlockNight()
        {
            _motorolaMatrix0X1A.UpdateData(
                _lampOperaterDic[LampOnOffType.RrLmpShwTypUnlockNight]);
        }

        [Description("LMC下晚上解锁")]
        public void RrLmpShwTypUnlockNightOnlyLmc()
        {
            _motorolaMatrix0X1A.UpdateData(
               _lampOperaterDic[LampOnOffType.RrLmpShwTypUnlockNightOnlyLmc]);
        }

        [Description("白天锁车且引擎关")]
        public void RrLmpShwTypLockEngineOffDay()
        {
            _motorolaMatrix0X1A.UpdateData(
              _lampOperaterDic[LampOnOffType.RrLmpShwTypLockEngineOffDay]);
        }

        private readonly Dictionary<LampOnOffType, MatrixValDefinition> _lampOperaterDic =
           new Dictionary<LampOnOffType, MatrixValDefinition>();
        private readonly Thread _mainWorkThread;
        private bool _isSleep = true;

        private readonly LinCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X1A =
            new LinCommunicationMatrix.MotorolaMatrix(0x1A, 2);
        private readonly LinCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X15 =
            new LinCommunicationMatrix.MotorolaMatrix(0x15, 6);

        private readonly LinCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X05 =
            new LinCommunicationMatrix.MotorolaMatrix(0x05, 6);
        private readonly LinCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X06 =
            new LinCommunicationMatrix.MotorolaMatrix(0x06, 6);
        private readonly LinCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X07 =
            new LinCommunicationMatrix.MotorolaMatrix(0x07, 6);
        private readonly LinCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X08 =
            new LinCommunicationMatrix.MotorolaMatrix(0x08, 6);
        private readonly LinCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X09 =
            new LinCommunicationMatrix.MotorolaMatrix(0x09, 6);
        private readonly LinCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X10 =
            new LinCommunicationMatrix.MotorolaMatrix(0x10, 6);
        private readonly LinCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X11 =
            new LinCommunicationMatrix.MotorolaMatrix(0x11, 6);

        private readonly LinCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X1B =
            new LinCommunicationMatrix.MotorolaMatrix(0x1B, 6);
        private readonly LinCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X1C =
            new LinCommunicationMatrix.MotorolaMatrix(0x1C, 6);
        private readonly LinCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X1D =
            new LinCommunicationMatrix.MotorolaMatrix(0x1D, 6);
        private readonly LinCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X1E =
            new LinCommunicationMatrix.MotorolaMatrix(0x1E, 6);

        private LampOnOffType _currentLeftRearTurnLampSession = LampOnOffType.LeftRearTurnLampNotActive;
        private bool _currentLeftRearTurnLampSessionIsNeedSwitch;
        private LampOnOffType _currentRightRearTurnLampSession = LampOnOffType.RightRearTurnLampNotActive;
        private bool _currentRightRearTurnLampSessionIsNeedSwitch;

        private void CyNormalCyclicTimer()
        {
            while (_mainWorkThread.IsAlive)
            {
                if (!_mainWorkThread.IsAlive)
                    break;

                Thread.Sleep(100);

                try
                {
                    if (LinWithBaudRate10417 == null)
                        return;

                    if (_currentLeftRearTurnLampSessionIsNeedSwitch)
                    {
                        _currentLeftRearTurnLampSessionIsNeedSwitch = false;
                        _motorolaMatrix0X1A.UpdateData(_lampOperaterDic[_currentLeftRearTurnLampSession]);
                    }
                    else
                    {
                        _currentLeftRearTurnLampSessionIsNeedSwitch = true;
                        if (_currentLeftRearTurnLampSession != LampOnOffType.LeftRearTurnLampHoldOnLight)
                            _motorolaMatrix0X1A.UpdateData(_lampOperaterDic[LampOnOffType.LeftRearTurnLampNotActive]);
                    }

                    if (_currentRightRearTurnLampSessionIsNeedSwitch)
                    {
                        _currentRightRearTurnLampSessionIsNeedSwitch = false;
                        _motorolaMatrix0X1A.UpdateData(_lampOperaterDic[_currentRightRearTurnLampSession]);
                    }
                    else
                    {
                        _currentRightRearTurnLampSessionIsNeedSwitch = true;
                        if (_currentRightRearTurnLampSession != LampOnOffType.RightRearTurnLampHoldOnLight)
                            _motorolaMatrix0X1A.UpdateData(_lampOperaterDic[LampOnOffType.RightRearTurnLampNotActive]);
                    }

                    if (!_isSleep)
                        LinWithBaudRate10417.SendMasterLin(_motorolaMatrix0X1A.MasterLinId, _motorolaMatrix0X1A.MatrixData);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        /// <summary>
        /// 点灯控制类型
        /// </summary>
        internal enum LampOnOffType
        {
            /// <summary>
            /// 左尾灯熄灭
            /// </summary>
            [MatrixValDefinition(11, 1, 0)]
            [Description("0x1A")]
            LeftRearParkLampOff,

            /// <summary>
            /// 左尾灯开
            /// </summary>
            [MatrixValDefinition(11, 1, 1)]
            [Description("0x1A")]
            LeftRearParkLampOn,

            /// <summary>
            /// 右尾灯开
            /// </summary>
            [MatrixValDefinition(7, 1, 0)]
            [Description("0x1A")]
            RightRearParkLampOff,

            /// <summary>
            /// 右尾灯熄灭
            /// </summary>
            [MatrixValDefinition(7, 1, 1)]
            [Description("0x1A")]
            RightRearParkLampOn,

            /// <summary>
            /// 右后转向灯熄灭
            /// </summary>
            [MatrixValDefinition(8, 3, 0)]
            [Description("0x1A")]
            RightRearTurnLampNotActive,

            /// <summary>
            /// 右后转向灯时序点亮
            /// </summary>
            [MatrixValDefinition(8, 3, 1)]
            [Description("0x1A")]
            RightRearTurnLampSwipeTurn,

            /// <summary>
            /// 右后转向灯1.33hz正常频率闪烁点亮
            /// </summary>
            [MatrixValDefinition(8, 3, 2)]
            [Description("0x1A")]
            RightRearTurnLampNormalFlash,

            /// <summary>
            /// 右后转向灯1hz频率闪烁点亮
            /// </summary>
            [MatrixValDefinition(8, 3, 3)]
            [Description("0x1A")]
            RightRearTurnLampNormalFlash1Hz,

            /// <summary>
            /// 右后转向灯3hz频率闪烁点亮
            /// </summary>
            [MatrixValDefinition(8, 3, 4)]
            [Description("0x1A")]
            RightRearTurnLampNormalFlashMode3,

            /// <summary>
            /// 右后转向灯常亮
            /// </summary>
            [MatrixValDefinition(8, 3, 5)]
            [Description("0x1A")]
            RightRearTurnLampHoldOnLight,

            /// <summary>
            /// 右后转向灯3hz频率闪烁点亮
            /// </summary>
            [MatrixValDefinition(8, 3, 6)]
            [Description("0x1A")]
            RightRearTurnLampFaultFlash,

            /// <summary>
            /// 左后转向灯熄灭
            /// </summary>
            [MatrixValDefinition(12, 3, 0)]
            [Description("0x1A")]
            LeftRearTurnLampNotActive,

            /// <summary>
            /// 左后转向灯时序点亮
            /// </summary>
            [MatrixValDefinition(12, 3, 1)]
            [Description("0x1A")]
            LeftRearTurnLampSwipeTurn,

            /// <summary>
            /// 左后转向灯1.33hz正常频率闪烁点亮
            /// </summary>
            [MatrixValDefinition(12, 3, 2)]
            [Description("0x1A")]
            LeftRearTurnLampNormalFlash,

            /// <summary>
            /// 左后转向灯1hz频率闪烁点亮
            /// </summary>
            [MatrixValDefinition(12, 3, 3)]
            [Description("0x1A")]
            LeftRearTurnLampNormalFlash1Hz,

            /// <summary>
            /// 左后转向灯3hz频率闪烁点亮
            /// </summary>
            [MatrixValDefinition(12, 3, 4)]
            [Description("0x1A")]
            LeftRearTurnLampNormalFlashMode3,

            /// <summary>
            /// 左后转向灯常亮
            /// </summary>
            [MatrixValDefinition(12, 3, 5)]
            [Description("0x1A")]
            LeftRearTurnLampHoldOnLight,

            /// <summary>
            /// 左后转向灯3hz频率闪烁点亮
            /// </summary>
            [MatrixValDefinition(12, 3, 6)]
            [Description("0x1A")]
            LeftRearTurnLampFaultFlash,

            [MatrixValDefinition(4, 3, 0)]
            [Description("0x1A")]
            RrLmpShwNotActive,

            [MatrixValDefinition(4, 3, 7)]
            [Description("0x1A")]
            RrLmpShwFactoryMode,

            [MatrixValDefinition(0, 4, 0)]
            [Description("0x1A")]
            RrLmpShwTypNotActive,

            [MatrixValDefinition(0, 4, 1)]
            [Description("0x1A")]
            RrLmpShwTypAbortShow,

            [MatrixValDefinition(0, 4, 2)]
            [Description("0x1A")]
            RrLmpShwTypEngineOffDay,

            [MatrixValDefinition(0, 4, 3)]
            [Description("0x1A")]
            RrLmpShwTypEngineOffNight,

            [MatrixValDefinition(0, 4, 4)]
            [Description("0x1A")]
            RrLmpShwTypEngineOn,

            [MatrixValDefinition(0, 4, 5)]
            [Description("0x1A")]
            RrLmpShwTypUnlockDay,

            [MatrixValDefinition(0, 4, 6)]
            [Description("0x1A")]
            RrLmpShwTypUnlockNight,

            [MatrixValDefinition(0, 4, 7)]
            [Description("0x1A")]
            RrLmpShwTypUnlockNightOnlyLmc,

            [MatrixValDefinition(0, 4, 8)]
            [Description("0x1A")]
            RrLmpShwTypLockEngineOffDay
        }
    }
}
