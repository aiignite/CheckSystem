using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace Controller
{
    [Description("CAN-Product,D058高配前灯控制模块")]
    public sealed class D058FrontLampControlModule : ControllerBase
    {
        public CanBus Can;

        #region Fields

        /// <summary>
        /// 读取模式配置
        /// </summary>
        [Description("R,读取节点配置")]
        public string NodeStr;

        /// <summary>
        /// 读取节点配置
        /// </summary>
        [Description("R,读取节点配置")]
        public string LrdfStr;

        /// <summary>
        /// 条码
        /// </summary>
        [Description("R,条码")]
        public string BarcodeStr;

        /// <summary>
        /// 总成零件号
        /// </summary>
        [Description("R,总成零件号")]
        public string HwPn;

        /// <summary>
        /// 引导程序零件号
        /// </summary>
        [Description("R,引导程序零件号")]
        public string BootAppSwPn;

        /// <summary>
        /// 引导程序版本号
        /// </summary>
        [Description("R,引导程序版本号")]
        public string BootAppSwVer;

        /// <summary>
        /// 应用程序零件号
        /// </summary>
        [Description("R,应用程序零件号")]
        public string AppSwPn;

        /// <summary>
        /// 配置文件零件号
        /// </summary>
        [Description("R,配置文件零件号")]
        public string CfgPn;

        /// <summary>
        /// 应用程序版本号
        /// </summary>
        [Description("R,应用程序版本号")]
        public string AppSwVer;

        /// <summary>
        /// 配置文件版本号
        /// </summary>
        [Description("R,配置文件版本号")]
        public string CfgVer;

        /// <summary>
        /// 生产追溯信息
        /// </summary>
        [Description("R,生产追溯信息")]
        public string ProductData;

        #endregion

        public D058FrontLampControlModule(string name)
            : base(name)
        {
            foreach (var temp
               in Enum.GetValues(typeof(LightOnOffType)).Cast<LightOnOffType>())
                _lightOperaterDic.Add(temp, temp.GetCustomAttribute<MatrixValDefinition>());

            _lampTh = new Thread(LightOnOffTimer) { IsBackground = true };
            _lampTh.Start();
        }

        ~D058FrontLampControlModule()
        {
            Dispose();
        }

        [Description("模块休眠")]
        public void EnterSleepMode()
        {
            _isSleeping = true;
        }

        [Description("模块唤醒")]
        public void ExitSleepMode()
        {
            _isSleeping = false;
        }

        [Description("配置为左节点")]
        public void ChangeLeftNode()
        {
            _currentReqCanId = 0x1F6;
            _currentRecvCanId = 0x1F7;
        }

        [Description("配置为右节点")]
        public void ChangeRightNode()
        {
            _currentReqCanId = 0x1F8;
            _currentRecvCanId = 0x1F9;
        }

        private uint _currentReqCanId = 0x1F6;
        private uint _currentRecvCanId = 0x1F7;

        private readonly Thread _lampTh;
        private readonly Dictionary<LightOnOffType, MatrixValDefinition> _lightOperaterDic =
            new Dictionary<LightOnOffType, MatrixValDefinition>();
        private int _periodCount;
        private bool _isSleeping = true;
        private bool _isInExtendedSession;

        #region LED相关

        private readonly CanCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X241 =
            new CanCommunicationMatrix.MotorolaMatrix(0x241, 6);
        private readonly CanCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X102 =
            new CanCommunicationMatrix.MotorolaMatrix(0x102, 8);
        private readonly CanCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X300 =
            new CanCommunicationMatrix.MotorolaMatrix(0x300, 8);

        [Description("近光开")]
        public void LbOn()
        {
            ChangeLightType(LightOnOffType.LeftLowBmOn);
            ChangeLightType(LightOnOffType.RightLowBmOn);
        }

        [Description("近光关")]
        public void LbOff()
        {
            ChangeLightType(LightOnOffType.LeftLowBmOff);
            ChangeLightType(LightOnOffType.RightLowBmOff);
        }

        [Description("远光开")]
        public void HbOn()
        {
            ChangeLightType(LightOnOffType.HighBmOn);
        }

        [Description("远光关")]
        public void HbOff()
        {
            ChangeLightType(LightOnOffType.HighBmOff);
        }

        [Description("转向普通开")]
        public void TurnNormalOn()
        {
            ChangeLightType(LightOnOffType.LeftTurnOn);
            ChangeLightType(LightOnOffType.RightTurnOn);
        }

        [Description("转向跑马开")]
        public void TurnWaterFlash()
        {
            ChangeLightType(LightOnOffType.LeftTurnWaterFlash);
            ChangeLightType(LightOnOffType.RightTurnWaterFlash);
        }

        [Description("转向关")]
        public void TurnOff()
        {
            ChangeLightType(LightOnOffType.LeftTurnOff);
            ChangeLightType(LightOnOffType.RightTurnOff);
        }

        [Description("日行灯开")]
        public void DrlOn()
        {
            ChangeLightType(LightOnOffType.LeftDrlOn);
            ChangeLightType(LightOnOffType.RightDrlOn);
        }

        [Description("行的关")]
        public void DrlOff()
        {
            ChangeLightType(LightOnOffType.LeftDrlOff);
            ChangeLightType(LightOnOffType.RightDrlOff);
        }

        [Description("位置灯开")]
        public void PlOn()
        {
            ChangeLightType(LightOnOffType.LeftParkOn);
            ChangeLightType(LightOnOffType.RightParkOn);
        }

        [Description("位置灯关")]
        public void PlOff()
        {
            ChangeLightType(LightOnOffType.LeftParkOff);
            ChangeLightType(LightOnOffType.RightParkOff);
        }

        [Description("角灯开")]
        public void CornerOn()
        {
            ChangeLightType(LightOnOffType.LeftCornerOn);
            ChangeLightType(LightOnOffType.RightCornerOn);
        }

        [Description("角灯关")]
        public void CornerOff()
        {
            ChangeLightType(LightOnOffType.LeftCornerOff);
            ChangeLightType(LightOnOffType.RightCornerOff);
        }

        [Description("关闭所有")]
        public void AllLedOff()
        {
            CornerOff();
            DrlOff();
            HbOff();
            LbOff();
            PlOff();
            TurnOff();
        }

        private void ChangeLightType(LightOnOffType typeInt)
        {
            var str = typeInt.ToString();
            var lightOnOffType =
                (LightOnOffType)Enum.Parse(typeof(LightOnOffType), str);

            var des = Convert.ToUInt32(
                       lightOnOffType.GetCustomAttribute<DescriptionAttribute>().Description, 16);

            if (des.Equals(_motorolaMatrix0X241.CanId))
                _motorolaMatrix0X241.UpdateData(
                    _lightOperaterDic[lightOnOffType]);
            else if (des.Equals(_motorolaMatrix0X102.CanId))
                _motorolaMatrix0X102.UpdateData(
                    _lightOperaterDic[lightOnOffType]);
        }

        private void LightOnOffTimer()
        {
            while (_lampTh.IsAlive)
            {
                if (!_lampTh.IsAlive)
                    break;

                Thread.Sleep(4);

                if (Can == null || _isSleeping)
                {
                    _isInExtendedSession = false;
                    _periodCount = 0;
                    continue;
                }

                try
                {
                    Can.SendStandardCanData(
                        _motorolaMatrix0X102.CanId, _motorolaMatrix0X102.MatrixData);
                    Can.SendStandardCanData(
                        _motorolaMatrix0X241.CanId, _motorolaMatrix0X241.MatrixData);

                    if (_periodCount % 3 == 0)
                        Can.SendStandardCanData(
                            _motorolaMatrix0X300.CanId, _motorolaMatrix0X300.MatrixData);

                    if (_periodCount % 100 == 0 && _isInExtendedSession) // Period = 1000ms
                        Can.SendStandardCanData(
                                _currentReqCanId, new byte[] { 0x02, 0x3E, 0x00, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA });

                    _periodCount++;
                    if (_periodCount > 10000)
                        _periodCount = 0;
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        #endregion

        #region 其他诊断

        [Description("进入拓展")]
        public void EnterExtendedSession()
        {
            if (Can == null)
                return;

            if (
                !Can.CanBusWithUds.TryEnterExtendedSession(_currentReqCanId, _currentRecvCanId,
                    CanBus.CanType.Standard))
            {
                Thread.Sleep(500);
                if (Can.CanBusWithUds.TryEnterExtendedSession(_currentReqCanId, _currentRecvCanId,
                    CanBus.CanType.Standard))
                    _isInExtendedSession = true;
            }
            else
                _isInExtendedSession = true;
        }

        [Description("退出拓展")]
        public void ExitExtendedSession()
        {
            if (Can == null)
                return;

            _isInExtendedSession = false;

            if (Can.CanBusWithUds.TryEnterDefaultSession(_currentReqCanId, _currentRecvCanId,
                CanBus.CanType.Standard))
                return;

            Thread.Sleep(500);
            Can.CanBusWithUds.TryEnterDefaultSession(_currentReqCanId, _currentRecvCanId,
                CanBus.CanType.Standard);
        }

        [Description("禁止故障诊断")]
        public void DisableDtc()
        {
            Can.CanBusWithUds.TryControlDtcSetting(_currentReqCanId, _currentRecvCanId,
                CanBus.CanType.Standard, Uds14229Helper.ControlDtcSetting.Disable);
        }

        [Description("节点配置")]
        public void WriteNode(string node)
        {
            try
            {
                var hex = Convert.ToByte(node, 16);
                Can.CanBusWithUds.TryWriteData(_currentReqCanId, _currentRecvCanId, CanBus.CanType.Standard,
                    CanBus.CanProtocol.Can, 0xD0, 0x08, new[] { hex });
            }
            catch (Exception)
            {
                // ignored
            }
        }

        [Description("节点读取")]
        public void ReadNode()
        {
            NodeStr = string.Empty;
            var result = ReadDid(_currentReqCanId, _currentRecvCanId, 0xD0, 0x08);
            NodeStr = ValueHelper.GetHextStr(result);
        }

        #endregion

        #region 确认追溯

        /// <summary>
        /// 读引导程序零件号
        /// </summary>
        [Description("读引导程序零件号")]
        public void ReadBootAppSwPn()
        {
            BootAppSwPn = string.Empty;
            var result = ReadDid(_currentReqCanId, _currentRecvCanId, 0x01, 0xC0);
            BootAppSwPn = "SW" + ValueHelper.GetDecimal(result).ToString().PadLeft(7, '0');
        }

        /// <summary>
        /// 读引导程序版本号
        /// </summary>
        [Description("读引导程序版本号")]
        public void ReadBootAppSwVer()
        {
            BootAppSwVer = string.Empty;
            var result = ReadDid(_currentReqCanId, _currentRecvCanId, 0x01, 0xD0);
            BootAppSwVer = result.GetStringByAsciiBytes(false);
        }

        /// <summary>
        /// 读总成零件号
        /// </summary>
        [Description("读总成零件号")]
        public void ReadHwPn()
        {
            HwPn = string.Empty;
            var result = ReadDid(_currentReqCanId, _currentRecvCanId, 0xD0, 0x0F);
            HwPn = result.GetStringByAsciiBytes(false);
        }

        /// <summary>
        /// 读应用程序零件号
        /// </summary>
        [Description("读应用程序零件号")]
        public void ReadAppSwPn()
        {
            AppSwPn = string.Empty;
            var result = ReadDid(_currentReqCanId, _currentRecvCanId, 0x01, 0xC1);
            AppSwPn = "SW" + ValueHelper.GetDecimal(result).ToString().PadLeft(7, '0');
        }

        /// <summary>
        /// 读配置文件零件号
        /// </summary>
        [Description("读配置文件零件号")]
        public void ReadCfgPn()
        {
            CfgPn = string.Empty;
            var result = ReadDid(_currentReqCanId, _currentRecvCanId, 0x01, 0xC2);
            CfgPn = "SW" + ValueHelper.GetDecimal(result).ToString().PadLeft(7, '0');
        }

        /// <summary>
        /// 读应用程序版本号
        /// </summary>
        [Description("读应用程序版本号")]
        public void ReadAppSwVer()
        {
            AppSwVer = string.Empty;
            var result = ReadDid(_currentReqCanId, _currentRecvCanId, 0x01, 0xD1);
            AppSwVer = result.GetStringByAsciiBytes(false);
        }

        /// <summary>
        /// 配置文件版本号
        /// </summary>
        [Description("配置文件版本号")]
        public void ReadCfgVer()
        {
            CfgVer = string.Empty;
            var result = ReadDid(_currentReqCanId, _currentRecvCanId, 0x01, 0xD2);
            CfgVer = result.GetStringByAsciiBytes(false);
        }

        /// <summary>
        /// 读生产追溯信息
        /// </summary>
        [Description("读生产追溯信息")]
        public void ReadProductData()
        {
            ProductData = string.Empty;
            //ProductData = "NG";

            //if (string.IsNullOrEmpty(BarcodeStr))
            //    return;
            var result = ReadDid(_currentReqCanId, _currentRecvCanId, 0x01, 0xB4);
            ProductData = result.GetStringByAsciiBytes(false);
            //if (result.Any())
            //{
            //ProductData = "OK ";
            //foreach (var item in result)
            //{
            //    ProductData += ValueHelper.GetHextStr(item);
            //}
            //}
        }

        #endregion

        #region 读写电感

        public double Led1Current;
        public double Led2Current;
        public double Led3Current;
        public double Led4Current;

        public double Led1K;
        public double Led1B;
        public double Led2K;
        public double Led2B;
        public double Led3K;
        public double Led3B;
        public double Led4K;
        public double Led4B;

        [Description("R,电感1读取值")]
        public double Inductance1;
        [Description("R,电感2读取值")]
        public double Inductance2;
        [Description("R,电感3读取值")]
        public double Inductance3;
        [Description("R,电感4读取值")]
        public double Inductance4;

        public void SetInductance()
        {
            if (Can == null)
                return;

            Led1K = 0.0001 * Led1K;
            Led1B = 0.0001 * Led1B;
            Led2K = 0.0001 * Led2K;
            Led2B = 0.0001 * Led2B;
            Led3K = 0.0001 * Led3K;
            Led3B = 0.0001 * Led3B;

            Led4K = 0.0001 * Led4K;
            Led4B = 0.0001 * Led4B;

            var led1Curr = Convert.ToUInt16(Led1Current * Led1K + Led1B);
            var led2Curr = Convert.ToUInt16(Led2Current * Led2K + Led2B);

            var led3Curr = Convert.ToUInt16(Led3Current * Led3K + Led3B);
            var led4Curr = Convert.ToUInt16(Led4Current * Led4K + Led4B);


            var led1Bytes = BitConverter.GetBytes(led1Curr);
            var led2Bytes = BitConverter.GetBytes(led2Curr);
            var led3Bytes = BitConverter.GetBytes(led3Curr);
            var led4Bytes = BitConverter.GetBytes(led4Curr);

            Array.Reverse(led1Bytes);
            Array.Reverse(led2Bytes);
            Array.Reverse(led3Bytes);
            Array.Reverse(led4Bytes);

            ReadInductance();

            if (Math.Abs(Inductance1 - -9999) < 1 && Math.Abs(Inductance2 - -9999) < 1)
            {
                Thread.Sleep(25);

                if (
                !Can.CanBusWithUds.TryStartRoutineControl(_currentReqCanId, _currentRecvCanId,
                    CanBus.CanType.Standard, Uds14229Helper.RoutineControl.Unknown1,
                    new[] { led1Bytes[0], led1Bytes[1], led2Bytes[0], led2Bytes[1] }))
                {
                    //Thread.Sleep(500);
                    //Can.CanBusWithUds.TryStartRoutineControl(_currentReqCanId, _currentRecvCanId,
                    //    CanBus.CanType.Standard, Uds14229Helper.RoutineControl.Unknown1,
                    //    new[] { led1Bytes[0], led1Bytes[1], led2Bytes[0], led2Bytes[01] });
                }
            }

            if (Math.Abs(Inductance3 - -9999) < 1 && Math.Abs(Inductance4 - -9999) < 1)
            {
                Thread.Sleep(25);

                if (
                    !Can.CanBusWithUds.TryStartRoutineControl(_currentReqCanId, _currentRecvCanId,
                        CanBus.CanType.Standard, Uds14229Helper.RoutineControl.Unknown2,
                        new[] { led3Bytes[0], led3Bytes[1], led4Bytes[0], led4Bytes[1] }))
                {
                    //Thread.Sleep(500);
                    //Can.CanBusWithUds.TryStartRoutineControl(_currentReqCanId, _currentRecvCanId,
                    //    CanBus.CanType.Standard, Uds14229Helper.RoutineControl.Unknown2,
                    //    new[] { led3Bytes[0], led3Bytes[1], led4Bytes[0], led4Bytes[01] });
                }
            }
        }

        [Description("读取电感")]
        public void ReadInductance()
        {
            Inductance1 = -9999;
            Inductance2 = -9999;
            Inductance3 = -9999;
            Inductance4 = -9999;

            if (Can == null)
                return;

            var read12Func = new Func<byte[]>(() =>
            {
                byte[] readBytes;
                Can.CanBusWithUds.TryReadData(
                    _currentReqCanId, _currentRecvCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    0xD0, 0x26,
                    out readBytes);
                return readBytes;
            });

            var read12 = read12Func.Invoke() ?? read12Func.Invoke();
            if (read12 != null && read12.Length >= 5)
                if (read12[0] == 1)
                {
                    Inductance1 = read12[1];
                    Inductance2 = read12[3];
                }

            var read34Func = new Func<byte[]>(() =>
            {
                byte[] readBytes;
                Can.CanBusWithUds.TryReadData(
                    _currentReqCanId, _currentRecvCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    0xD0, 0x27,
                    out readBytes);
                return readBytes;
            });

            var read34 = read34Func.Invoke() ?? read34Func.Invoke();
            if (read34 != null && read34.Length >= 5)
                if (read34[0] == 1)
                {
                    Inductance3 = read34[1];
                    Inductance4 = read34[3];
                }
        }

        #endregion

        private byte[] ReadDid(uint reqCanId, uint recvCanId, byte didHi, byte didLo)
        {
            if (Can == null)
                return new byte[0];

            byte[] echo;
            if (Can.CanBusWithUds.TryReadData(reqCanId, recvCanId, CanBus.CanType.Standard,
                CanBus.CanProtocol.Can, didHi, didLo, out echo))
                return echo;
            Thread.Sleep(500);
            return Can.CanBusWithUds.TryReadData(reqCanId, recvCanId, CanBus.CanType.Standard,
                CanBus.CanProtocol.Can, didHi, didLo, out echo) ? echo : new byte[0];
        }

        /// <summary>
        /// 点灯控制类型
        /// </summary>
        internal enum LightOnOffType
        {
            /// <summary>
            /// 左近光灯亮
            /// </summary>
            [MatrixValDefinition(7, 1, 1)]
            [Description("0x241")]
            LeftLowBmOn,

            /// <summary>
            /// 左近光灯灭
            /// </summary>
            [MatrixValDefinition(7, 1, 0)]
            [Description("0x241")]
            LeftLowBmOff,

            /// <summary>
            /// 左转向灯常亮
            /// </summary>
            [MatrixValDefinition(23, 3, 5)]
            [Description("0x241")]
            LeftTurnOn,

            /// <summary>
            /// 左转向灯灭
            /// </summary>
            [MatrixValDefinition(23, 3, 0)]
            [Description("0x241")]
            LeftTurnOff,

            /// <summary>
            /// 左转向灯跑马
            /// </summary>
            [MatrixValDefinition(23, 3, 1)]
            [Description("0x241")]
            LeftTurnWaterFlash,

            /// <summary>
            /// 左转日行灯亮
            /// </summary>
            [MatrixValDefinition(13, 1, 1)]
            [Description("0x241")]
            LeftDrlOn,

            /// <summary>
            /// 左转日行灯灭
            /// </summary>
            [MatrixValDefinition(13, 1, 0)]
            [Description("0x241")]
            LeftDrlOff,

            /// <summary>
            /// 左转位置灯亮
            /// </summary>
            [MatrixValDefinition(11, 1, 1)]
            [Description("0x241")]
            LeftParkOn,

            /// <summary>
            /// 左转位置灯灭
            /// </summary>
            [MatrixValDefinition(11, 1, 0)]
            [Description("0x241")]
            LeftParkOff,

            /// <summary>
            /// 左转角灯亮
            /// </summary>
            [MatrixValDefinition(16, 8, 100)]
            [Description("0x102")]
            LeftCornerOn,

            /// <summary>
            /// 左转角灯灭
            /// </summary>
            [MatrixValDefinition(16, 8, 0)]
            [Description("0x102")]
            LeftCornerOff,

            /// <summary>
            /// 远光亮
            /// 远光灯的点灯命令没有左右区别
            /// </summary>
            [MatrixValDefinition(12, 4, 1)]
            [Description("0x102")]
            HighBmOn,

            /// <summary>
            /// 远光灭
            /// 远光灯的点灯命令没有左右区别
            /// </summary>
            [MatrixValDefinition(12, 4, 0)]
            [Description("0x102")]
            HighBmOff,

            /// <summary>
            /// 右近光灯亮
            /// </summary>
            [MatrixValDefinition(22, 1, 1)]
            [Description("0x241")]
            RightLowBmOn,

            /// <summary>
            /// 右近光灯灭
            /// </summary>
            [MatrixValDefinition(22, 1, 0)]
            [Description("0x241")]
            RightLowBmOff,

            /// <summary>
            /// 右转向灯常亮
            /// </summary>
            [MatrixValDefinition(26, 3, 5)]
            [Description("0x241")]
            RightTurnOn,

            /// <summary>
            /// 右转向灯灭
            /// </summary>
            [MatrixValDefinition(26, 3, 0)]
            [Description("0x241")]
            RightTurnOff,

            /// <summary>
            /// 右转向灯跑马
            /// </summary>
            [MatrixValDefinition(26, 3, 1)]
            [Description("0x241")]
            RightTurnWaterFlash,

            /// <summary>
            /// 右转日行灯亮
            /// </summary>
            [MatrixValDefinition(16, 1, 1)]
            [Description("0x241")]
            RightDrlOn,

            /// <summary>
            /// 右转日行灯灭
            /// </summary>
            [MatrixValDefinition(16, 1, 0)]
            [Description("0x241")]
            RightDrlOff,

            /// <summary>
            /// 右转位置灯亮
            /// </summary>
            [MatrixValDefinition(30, 1, 1)]
            [Description("0x241")]
            RightParkOn,

            /// <summary>
            /// 右转位置灯灭
            /// </summary>
            [MatrixValDefinition(30, 1, 0)]
            [Description("0x241")]
            RightParkOff,

            /// <summary>
            /// 右转角灯亮
            /// </summary>
            [MatrixValDefinition(24, 8, 100)]
            [Description("0x102")]
            RightCornerOn,

            /// <summary>
            /// 右转角灯灭
            /// </summary>
            [MatrixValDefinition(24, 8, 0)]
            [Description("0x102")]
            RightCornerOff
        }
    }
}
