using CommonUtility;
using CommonUtility.BusLoader;
using Controller.HardwareController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace Controller
{
    [Description("CAN-Device")]
    public sealed class SyControllerMaster :
        ControllerBase,
        ControllerMasterSocketByUdp.IControllerMasterSocketSubscriber, ICcdIoController
    {
        public bool Input1;
        public bool Input2;

        public bool SetOutputs()
        {
            //throw new NotImplementedException();
            return false;
        }

        public void GetInputs()
        {
            GetMasterDi();

            Input1 = !Di1;
            Input2 = !Di2;
        }

        public void ResetOutPuts()
        {
            //throw new NotImplementedException();
        }

        #region Public fields

        [Description("R/W,高边输出1")]
        public bool DoHighSide1;

        [Description("R/W,高边输出2")]
        public bool DoHighSide2;

        [Description("R/W,低边输出1")]
        public bool DoLowSide1;

        [Description("R/W,低边输出2")]
        public bool DoLowSide2;

        [Description("R,DI输入1")]
        public bool Di1 = true;

        [Description("R,DI输入2")]
        public bool Di2 = true;

        [Description("R,电流1")]
        public float AdCurrent1;

        [Description("R,电流2")]
        public float AdCurrent2;

        [Description("R,电压1")]
        public float AdVoltage1;

        [Description("R,电压2")]
        public float AdVoltage2;

        [Description("R,电压3")]
        public float AdVoltage3;

        [Description("R,电压4")]
        public float AdVoltage4;

        public CanBus MasterGatewayCan;
        public LinBus MasterGatewayLin;
        public ControllerMasterGatewaySci MasterGatewaySci;

        /// <summary>
        /// 需要操作寄存器的从站
        /// </summary>
        public SyControllerSlaveWith10R GetTimeOutput;

        /// <summary>
        /// 需要监控电压或电流的从站
        /// </summary>
        public SyControllerSlaveWith14Ad GetTimeInput;

        /// <summary>
        /// 时间
        /// </summary>
        public float Time;

        #endregion

        private ControllerMasterSocketByUdp _myControllerUdp;
        private readonly Dictionary<ControllerMasterOpType, EventWaitHandle> _gatewayOperaterWhDictionary =
            new Dictionary<ControllerMasterOpType, EventWaitHandle>();
        private bool _isGettingMasterAd;

        public SyControllerMaster(string name) :
            base(name)
        {
            foreach (var temp in EnumOperater.GetEnumValueList<ControllerMasterOpType>())
                _gatewayOperaterWhDictionary.Add(temp, new AutoResetEvent(false));
        }

        ~SyControllerMaster()
        {
            if (_myControllerUdp!=null)
                _myControllerUdp.Dispose();
            Dispose();
        }

        /// <summary>
        /// 定义远程Controller Master IP地址
        /// </summary>
        /// <param name="ipPort">IP:PORT</param>
        public void InitRemoteIpAddr(string ipPort)
        {
            _myControllerUdp = new ControllerMasterSocketByUdp(Name, ipPort);
            MasterGatewayCan = new ControllerMasterGatewayCan(_myControllerUdp);
            MasterGatewayLin = new ControllerMasterGatewayLin(_myControllerUdp);
            MasterGatewaySci = new ControllerMasterGatewaySci(_myControllerUdp);
            _myControllerUdp.AddOvserver(ReceiveMsg);
        }

        public void ReceiveMsg(
            EndPoint ipEndPointFrom, object dataPackage)
        {
            if (dataPackage.GetType() ==
                typeof(ControllerMasterSocketByUdp.MasterDataPackage))
            {
                var masterDataPackage =
                    dataPackage as ControllerMasterSocketByUdp.MasterDataPackage;

                if (masterDataPackage == null)
                    return;

                if (_isGettingMasterAd &&
                    masterDataPackage.StartAddress == (ushort)ControllerMasterOpType.GetAd &&
                    masterDataPackage.RegisterCount == 6 * 2)
                {
                    var listFloat = new List<float>();
                    for (var i = 0; i < masterDataPackage.Bytes.Length; i = i + 4)
                    {
                        var floatBytes = new[]
                    {
                        masterDataPackage.Bytes[i],
                        masterDataPackage.Bytes[i + 1],
                        masterDataPackage.Bytes[i + 2],
                        masterDataPackage.Bytes[i + 3]
                    };
                        Array.Reverse(floatBytes);
                        listFloat.Add(
                            (float)Math.Round(BitConverter.ToSingle(floatBytes, 0), 4, MidpointRounding.AwayFromZero));
                    }

                    AdCurrent1 = listFloat[0];
                    AdCurrent2 = listFloat[1];
                    AdVoltage1 = listFloat[2];
                    AdVoltage2 = listFloat[3];
                    AdVoltage3 = listFloat[4];
                    AdVoltage4 = listFloat[5];

                    _gatewayOperaterWhDictionary[ControllerMasterOpType.GetAd].Set();
                }
                else if (
                    masterDataPackage.StartAddress == (ushort)ControllerMasterOpType.GetDi &&
                    masterDataPackage.RegisterCount == 2)
                {
                    var di1 =
                        masterDataPackage.Bytes[0] * 256 + masterDataPackage.Bytes[1];
                    Di1 = di1 == 1;
                    var di2 =
                        masterDataPackage.Bytes[2] * 256 + masterDataPackage.Bytes[3];
                    Di2 = di2 == 1;

                    //Console.WriteLine("{0} Di1={1} Di2={2}", _myControllerUdp.RemoteIpPort, Di1, Di2);
                    _gatewayOperaterWhDictionary[ControllerMasterOpType.GetDi].Set();
                }
                else if (
                    masterDataPackage.StartAddress == (ushort)ControllerMasterOpType.GetPwm &&
                    masterDataPackage.RegisterCount == 2)
                {
                    In1Frequency = masterDataPackage.Bytes[0] * 256 + masterDataPackage.Bytes[1];
                    In1Duty = masterDataPackage.Bytes[2] * 256 + masterDataPackage.Bytes[3];
                }
            }
            else if (dataPackage.GetType() ==
                typeof(ControllerMasterSocketByUdp.MasterTimeDataPackage))
            {
                var timeDataPackage =
                    dataPackage as ControllerMasterSocketByUdp.MasterTimeDataPackage;

                if (timeDataPackage != null)
                    Time = timeDataPackage.GetTime;

                _gatewayOperaterWhDictionary[ControllerMasterOpType.GetTime].Set();
            }
        }

        /// <summary>
        /// 获取主站电流电压值
        /// </summary>
        [Description("获取主站电流电压值")]
        public bool GetMasterAd()
        {
            if (_myControllerUdp == null)
            {
                AdCurrent1 = -9999;
                AdCurrent2 = -9999;
                AdVoltage1 = -9999;
                AdVoltage2 = -9999;
                AdVoltage3 = -9999;
                AdVoltage4 = -9999;
                return false;
            }

            var maxCount = 1;

            var getAdc1Sum = new List<float>();
            var getAdc2Sum = new List<float>();
            var getAdv1Sum = new List<float>();
            var getAdv2Sum = new List<float>();
            var getAdv3Sum = new List<float>();
            var getAdv4Sum = new List<float>();

            var tempFailedCount = 0;

            for (var i = 0; i < maxCount; i++)
            {
                _isGettingMasterAd = true;
                _myControllerUdp.SendControllerDataRead(0x150.ToString(), 6 * 2);

                if (_gatewayOperaterWhDictionary[ControllerMasterOpType.GetAd].WaitOne(2500))
                {
                    getAdc1Sum.Add(AdCurrent1);
                    getAdc2Sum.Add(AdCurrent2);
                    getAdv1Sum.Add(AdVoltage1);
                    getAdv2Sum.Add(AdVoltage2);
                    getAdv3Sum.Add(AdVoltage3);
                    getAdv4Sum.Add(AdVoltage4);
                }
                else
                {
                    tempFailedCount++;
                }

                _isGettingMasterAd = false;
            }

            if (tempFailedCount == maxCount)
            {
                AdCurrent1 = -9999;
                AdCurrent2 = -9999;
                AdVoltage1 = -9999;
                AdVoltage2 = -9999;
                AdVoltage3 = -9999;
                AdVoltage4 = -9999;
            }
            else
            {
                var tempAdc1 = getAdc1Sum.ToArray();
                Array.Sort(tempAdc1);
                Array.Reverse(tempAdc1);
                AdCurrent1 = tempAdc1[0];

                var tempAdc2 = getAdc2Sum.ToArray();
                Array.Sort(tempAdc2);
                Array.Reverse(tempAdc2);
                AdCurrent2 = tempAdc2[0];

                var tempAdv1 = getAdv1Sum.ToArray();
                Array.Sort(tempAdv1);
                Array.Reverse(tempAdv1);
                AdVoltage1 = tempAdv1[0];

                var tempAdv2 = getAdv2Sum.ToArray();
                Array.Sort(tempAdv2);
                Array.Reverse(tempAdv2);
                AdVoltage2 = tempAdv2[0];

                var tempAdv3 = getAdv3Sum.ToArray();
                Array.Sort(tempAdv3);
                Array.Reverse(tempAdv3);
                AdVoltage3 = tempAdv3[0];

                var tempAdv4 = getAdv4Sum.ToArray();
                Array.Sort(tempAdv4);
                Array.Reverse(tempAdv4);
                AdVoltage4 = tempAdv4[0];

                //AdCurrent1 = WaveFilter.MidianAverageFileter(getAdc1Sum.ToArray());
                //AdCurrent2 = WaveFilter.MidianAverageFileter(getAdc2Sum.ToArray());
                //AdVoltage1 = WaveFilter.MidianAverageFileter(getAdv1Sum.ToArray());
                //AdVoltage2 = WaveFilter.MidianAverageFileter(getAdv2Sum.ToArray());
                //AdVoltage3 = WaveFilter.MidianAverageFileter(getAdv3Sum.ToArray());
                //AdVoltage4 = WaveFilter.MidianAverageFileter(getAdv4Sum.ToArray());
            }

            //MyControllerUdp.SendControllerDataRead(0x150.ToString(), 6 * 2);
            //if (_gatewayOperaterWhDictionary[ControllerMasterOpType.GetAd].WaitOne(2500))
            //    return true;

            //AdCurrent1 = -9999;
            //AdCurrent2 = -9999;
            //AdVoltage1 = -9999;
            //AdVoltage2 = -9999;
            //AdVoltage3 = -9999;
            //AdVoltage4 = -9999;

            return false;
        }

        /// <summary>
        /// 获取主站DI值
        /// </summary>
        [Description("获取主站DI值")]
        public bool GetMasterDi()
        {
            Di1 = true;
            Di2 = true;

            if (_myControllerUdp == null)
                return false;

            _myControllerUdp.SendControllerDataRead(0x90.ToString(), 2);
            return
                _gatewayOperaterWhDictionary[ControllerMasterOpType.GetDi].WaitOne(1500);
        }

        public bool GetMasterDoHighSide()
        {
            return false;
        }

        public bool GetMasterDoLowSide()
        {
            return false;
        }

        public float In1Frequency;
        public float In1Duty;

        public bool GetPwm()
        {
            // 01 00 00 01 00 06 01 03 02 10 00 02 00 28 60 03 45 0C E9

            In1Frequency = -9999;
            In1Duty = -9999;

            _myControllerUdp.SendControllerDataRead(0x210.ToString(), 2);
            return
                _gatewayOperaterWhDictionary[ControllerMasterOpType.GetPwm].WaitOne(1500);
        }

        /// <summary>
        /// 设置主站高边输出
        /// </summary>
        [Description("设置主站高边输出")]
        public void SetMasterDoHighSide()
        {
            if (_myControllerUdp == null)
                return;

            var listBs = new List<byte>();
            listBs.AddRange(DoHighSide1 ? new byte[] { 0x00, 0x01 } : new byte[] { 0x00, 0x00 });
            listBs.AddRange(DoHighSide2 ? new byte[] { 0x00, 0x01 } : new byte[] { 0x00, 0x00 });

            _myControllerUdp.SendControllerDataWrite(
                0x100.ToString(), (ushort)(listBs.Count / 2), listBs.ToArray());
        }

        /// <summary>
        /// 设置主站低边输出
        /// </summary>
        [Description("设置主站低边输出")]
        public void SetMasterDoLowSide()
        {
            if (_myControllerUdp == null)
                return;

            var listBs = new List<byte>();
            listBs.AddRange(DoLowSide1 ? new byte[] { 0x00, 0x01 } : new byte[] { 0x00, 0x00 });
            listBs.AddRange(DoLowSide2 ? new byte[] { 0x00, 0x01 } : new byte[] { 0x00, 0x00 });

            _myControllerUdp.SendControllerDataWrite(
                0x102.ToString(), (ushort)(listBs.Count / 2), listBs.ToArray());
        }

        /// <summary>
        /// 设置主站PWM1
        /// </summary>
        /// <param name="doHs1PwmFrequency">频率</param>
        /// <param name="doHs1PwmDuty">周期</param>
        public void OpenMasterDoHs1Pwm(
            ushort doHs1PwmFrequency, ushort doHs1PwmDuty)
        {
            SetMasterDoHsPwm(1, doHs1PwmFrequency, doHs1PwmDuty);
        }

        /// <summary>
        /// 关闭主站PWM1
        /// </summary>
        public void CloseMasterDoHs1Pwm()
        {
            SetMasterDoHsPwm(1, 0, 0);
        }

        /// <summary>
        /// 设置主站PWM2
        /// </summary>
        /// <param name="doHs2PwmFrequency">频率</param>
        /// <param name="doHs2PwmDuty">周期</param>
        public void OpenMasterDoHs2Pwm(
            ushort doHs2PwmFrequency, ushort doHs2PwmDuty)
        {
            SetMasterDoHsPwm(2, doHs2PwmFrequency, doHs2PwmDuty);
        }

        /// <summary>
        /// 关闭主站PWM2
        /// </summary>
        public void CloseMasterDoHs2Pwm()
        {
            SetMasterDoHsPwm(2, 0, 0);
        }

        private void SetMasterDoHsPwm(
            int outIndex, ushort pwmFrequency, ushort pwmDuty)
        {
            if (_myControllerUdp == null)
                return;

            var hzBytes = BitConverter.GetBytes(pwmFrequency);
            Array.Reverse(hzBytes);

            var percentageBytes = BitConverter.GetBytes(pwmDuty);
            Array.Reverse(percentageBytes);

            var startAddress = 0x200.ToString();
            if (outIndex == 2)
                startAddress = 0x202.ToString();

            var val = new List<byte>();
            val.AddRange(hzBytes);
            val.AddRange(percentageBytes);

            _myControllerUdp.SendControllerDataWrite(
                startAddress, (ushort)(val.Count / 2), val.ToArray());
        }

        public void GetMasterAdCurrent1RisingTime(string threshold)
        {
            GetTime(true, Convert.ToSingle(threshold), true, 1, GetTimeOutput, null);
        }

        public void GetMasterAdCurrent2RisingTime(string threshold)
        {
            GetTime(true, Convert.ToSingle(threshold), true, 2, GetTimeOutput, null);
        }

        public void GetMasterAdVoltage1RisingTime(string threshold)
        {
            GetTime(true, Convert.ToSingle(threshold), false, 1, GetTimeOutput, null);
        }

        public void GetMasterAdVoltage2RisingTime(string threshold)
        {
            GetTime(true, Convert.ToSingle(threshold), false, 2, GetTimeOutput, null);
        }

        public void GetMasterAdVoltage3RisingTime(string threshold)
        {
            GetTime(true, Convert.ToSingle(threshold), false, 3, GetTimeOutput, null);
        }

        public void GetMasterAdVoltage4RisingTime(string threshold)
        {
            GetTime(true, Convert.ToSingle(threshold), false, 4, GetTimeOutput, null);
        }

        public void GetMasterAdCurrent1FailingTime(string threshold)
        {
            GetTime(false, Convert.ToSingle(threshold), true, 1, GetTimeOutput, null);
        }

        public void GetMasterAdCurrent2FailingTime(string threshold)
        {
            GetTime(false, Convert.ToSingle(threshold), true, 2, GetTimeOutput, null);
        }

        public void GetMasterAdVoltage1FailingTime(string threshold)
        {
            GetTime(false, Convert.ToSingle(threshold), false, 1, GetTimeOutput, null);
        }

        public void GetMasterAdVoltage2FailingTime(string threshold)
        {
            GetTime(false, Convert.ToSingle(threshold), false, 2, GetTimeOutput, null);
        }

        public void GetMasterAdVoltage3FailingTime(string threshold)
        {
            GetTime(false, Convert.ToSingle(threshold), false, 3, GetTimeOutput, null);
        }

        public void GetMasterAdVoltage4FailingTime(string threshold)
        {
            GetTime(false, Convert.ToSingle(threshold), false, 4, GetTimeOutput, null);
        }

        #region 获取从站电流上升时间
        public void GetSlaveAdCurrent1RisingTime(string threshold)
        {
            GetTime(true, Convert.ToSingle(threshold), true, 1, GetTimeOutput, GetTimeInput);
        }

        public void GetSlaveAdCurrent2RisingTime(string threshold)
        {
            GetTime(true, Convert.ToSingle(threshold), true, 2, GetTimeOutput, GetTimeInput);
        }

        public void GetSlaveAdCurrent3RisingTime(string threshold)
        {
            GetTime(true, Convert.ToSingle(threshold), true, 3, GetTimeOutput, GetTimeInput);
        }

        public void GetSlaveAdCurrent4RisingTime(string threshold)
        {
            GetTime(true, Convert.ToSingle(threshold), true, 4, GetTimeOutput, GetTimeInput);
        }

        public void GetSlaveAdCurrent5RisingTime(string threshold)
        {
            GetTime(true, Convert.ToSingle(threshold), true, 5, GetTimeOutput, GetTimeInput);
        }

        public void GetSlaveAdCurrent6RisingTime(string threshold)
        {
            GetTime(true, Convert.ToSingle(threshold), true, 6, GetTimeOutput, GetTimeInput);
        }
        #endregion

        #region 获取从站电压上升时间
        public void GetSlaveAdVoltage1RisingTime(string threshold)
        {
            GetTime(true, Convert.ToSingle(threshold), false, 1, GetTimeOutput, GetTimeInput);
        }

        public void GetSlaveAdVoltage2RisingTime(string threshold)
        {
            GetTime(true, Convert.ToSingle(threshold), false, 2, GetTimeOutput, GetTimeInput);
        }

        public void GetSlaveAdVoltage3RisingTime(string threshold)
        {
            GetTime(true, Convert.ToSingle(threshold), false, 3, GetTimeOutput, GetTimeInput);
        }

        public void GetSlaveAdVoltage4RisingTime(string threshold)
        {
            GetTime(true, Convert.ToSingle(threshold), false, 4, GetTimeOutput, GetTimeInput);
        }

        public void GetSlaveAdVoltage5RisingTime(string threshold)
        {
            GetTime(true, Convert.ToSingle(threshold), false, 5, GetTimeOutput, GetTimeInput);
        }

        public void GetSlaveAdVoltage6RisingTime(string threshold)
        {
            GetTime(true, Convert.ToSingle(threshold), false, 6, GetTimeOutput, GetTimeInput);
        }

        public void GetSlaveAdVoltage7RisingTime(string threshold)
        {
            GetTime(true, Convert.ToSingle(threshold), false, 7, GetTimeOutput, GetTimeInput);
        }

        public void GetSlaveAdVoltage8RisingTime(string threshold)
        {
            GetTime(true, Convert.ToSingle(threshold), false, 8, GetTimeOutput, GetTimeInput);
        }
        #endregion

        #region 获取从站电流下降时间
        public void GetSlaveAdCurrent1FailingTime(string threshold)
        {
            GetTime(false, Convert.ToSingle(threshold), true, 1, GetTimeOutput, GetTimeInput);
        }

        public void GetSlaveAdCurrent2FailingTime(string threshold)
        {
            GetTime(false, Convert.ToSingle(threshold), true, 2, GetTimeOutput, GetTimeInput);
        }

        public void GetSlaveAdCurrent3FailingTime(string threshold)
        {
            GetTime(false, Convert.ToSingle(threshold), true, 3, GetTimeOutput, GetTimeInput);
        }

        public void GetSlaveAdCurrent4FailingTime(string threshold)
        {
            GetTime(false, Convert.ToSingle(threshold), true, 4, GetTimeOutput, GetTimeInput);
        }

        public void GetSlaveAdCurrent5FailingTime(string threshold)
        {
            GetTime(false, Convert.ToSingle(threshold), true, 5, GetTimeOutput, GetTimeInput);
        }

        public void GetSlaveAdCurrent6FailingTime(string threshold)
        {
            GetTime(false, Convert.ToSingle(threshold), true, 6, GetTimeOutput, GetTimeInput);
        }
        #endregion

        #region 获取从站电压下降时间
        public void GetSlaveAdVoltage1FailingTime(string threshold)
        {
            GetTime(false, Convert.ToSingle(threshold), false, 1, GetTimeOutput, GetTimeInput);
        }

        public void GetSlaveAdVoltage2FailingTime(string threshold)
        {
            GetTime(false, Convert.ToSingle(threshold), false, 2, GetTimeOutput, GetTimeInput);
        }

        public void GetSlaveAdVoltage3FailingTime(string threshold)
        {
            GetTime(false, Convert.ToSingle(threshold), false, 3, GetTimeOutput, GetTimeInput);
        }

        public void GetSlaveAdVoltage4FailingTime(string threshold)
        {
            GetTime(false, Convert.ToSingle(threshold), false, 4, GetTimeOutput, GetTimeInput);
        }

        public void GetSlaveAdVoltage5FailingTime(string threshold)
        {
            GetTime(false, Convert.ToSingle(threshold), false, 5, GetTimeOutput, GetTimeInput);
        }

        public void GetSlaveAdVoltage6FailingTime(string threshold)
        {
            GetTime(false, Convert.ToSingle(threshold), false, 6, GetTimeOutput, GetTimeInput);
        }

        public void GetSlaveAdVoltage7FailingTime(string threshold)
        {
            GetTime(false, Convert.ToSingle(threshold), false, 7, GetTimeOutput, GetTimeInput);
        }

        public void GetSlaveAdVoltage8FailingTime(string threshold)
        {
            GetTime(false, Convert.ToSingle(threshold), false, 8, GetTimeOutput, GetTimeInput);
        }
        #endregion

        private bool GetTime(
            bool isRising, float threshold, bool isAdCurrent, byte adIndex,
            SyControllerSlaveWith10R rlSlave, SyControllerSlaveWith14Ad adSlave)
        {
            if (_myControllerUdp == null)
            {
                Time = -9999;
                return false;
            }

            var relayBytes = new byte[2];

            if (rlSlave != null)
            {
                var relayCharList = new List<string>();
                for (var i = 0; i < 16; i++)
                    relayCharList.Add(0.ToString());

                for (var i = 0; i < 10; i++)
                {
                    var relay =
                        (bool)rlSlave.GetType().GetField(string.Format("Relay{0}", i + 1)).GetValue(rlSlave);
                    relayCharList[i] = relay ? 1.ToString() : 0.ToString();
                }

                var relayChars = new string[16];
                Array.Copy(relayCharList.ToArray(), relayChars, relayCharList.Count);
                Array.Reverse(relayChars);

                var str = relayChars.Aggregate(string.Empty, (current, s) => current + s);
                var b1 = Convert.ToUInt16(str, 2);
                var b2 = BitConverter.GetBytes(b1);
                Array.Reverse(b2);
                Array.Copy(b2, relayBytes, 2);
            }

            _myControllerUdp.SendControllerGetTime(
                rlSlave == null ? 0x000 : rlSlave.CanId,
                adSlave == null ? 0x000 : adSlave.CanId,
                relayBytes, isAdCurrent, adIndex,
                isRising, threshold);

            if (_gatewayOperaterWhDictionary[ControllerMasterOpType.GetTime].WaitOne(500))
                return true;

            Time = -9999;
            return false;
        }

        private enum ControllerMasterOpType : ushort
        {
            /// <summary>
            /// 获取AD
            /// </summary>
            GetAd = 0x150,

            /// <summary>
            /// 获取DI
            /// </summary>
            GetDi = 0x90,

            /// <summary>
            /// 设置高边
            /// </summary>
            SetDoHighSide = 0x100,

            /// <summary>
            /// 设置低边
            /// </summary>
            SetDoLowSide = 0x102,

            /// <summary>
            /// 设置PWM
            /// </summary>
            SetDoHsPwm = 0x200,

            /// <summary>
            /// 获取时间
            /// </summary>
            GetTime = 0x555,

            /// <summary>
            /// PWM捕获
            /// </summary>
            GetPwm = 0x210,
        }

        /// <summary>
        /// 主站串口网关
        /// </summary>
        public class ControllerMasterGatewaySci :
            MySerialPort, ControllerMasterSocketByUdp.IControllerMasterSocketSubscriber
        {
            public ControllerMasterSocketByUdp MyControllerUdp;
            private readonly List<byte> _btBytesTemp = new List<byte>();

            /// <summary>
            /// 主站串口网关
            /// </summary>
            public ControllerMasterGatewaySci(
                ControllerMasterSocketByUdp myControllerUdp)
                : base(myControllerUdp.RemoteIpPort)
            {
                MyControllerUdp = myControllerUdp;
                MyControllerUdp.AddOvserver(ReceiveMsg);
            }

            public void ReceiveMsg(EndPoint ipEndPointFrom, object dataPackage)
            {
                if (dataPackage.GetType() != typeof(ControllerMasterSocketByUdp.SciDataPackage))
                    return;

                var sciPackage = dataPackage as ControllerMasterSocketByUdp.SciDataPackage;
                lock (_btBytesTemp)
                {
                    if (sciPackage != null) _btBytesTemp.AddRange(sciPackage.RecvData);
                }
            }

            public override void SendCommand(string str)
            {
                MyControllerUdp.SendControllerMasterSci(
                    Encoding.ASCII.GetBytes(str).ToList());
                Thread.Sleep(25);
            }

            public override void SendCommand(byte[] bytes, int delyMs = 25)
            {
                MyControllerUdp.SendControllerMasterSci(bytes.ToList());
                Thread.Sleep(delyMs);
            }

            public override string ReadDataStr()
            {
                Thread.Sleep(250);

                lock (_btBytesTemp)
                {
                    var returnBs = new byte[_btBytesTemp.Count];
                    Array.Copy(_btBytesTemp.ToArray(), returnBs, _btBytesTemp.Count);
                    _btBytesTemp.Clear();
                    var returnMsg = Encoding.ASCII.GetString(
                        returnBs, 0, returnBs.Length);
                    return returnMsg;
                }
            }

            public override byte[] ReadDataBytes()
            {
                Thread.Sleep(250);

                lock (_btBytesTemp)
                {
                    var returnBs = new byte[_btBytesTemp.Count];
                    Array.Copy(_btBytesTemp.ToArray(), returnBs, _btBytesTemp.Count);
                    _btBytesTemp.Clear();
                    return returnBs.ToArray();
                }
            }
        }

        /// <summary>
        /// 主站CAN网关
        /// </summary>
        public class ControllerMasterGatewayCan :
            CanBus,
            ControllerMasterSocketByUdp.IControllerMasterSocketSubscriber
        {
            private readonly ControllerMasterSocketByUdp _myControllerUdp;

            /// <summary>
            /// 主站CAN网关
            /// </summary>
            public ControllerMasterGatewayCan(
                ControllerMasterSocketByUdp myControllerUdp)
                : base(myControllerUdp.RemoteIpPort, 9)
            {
                _myControllerUdp = myControllerUdp;
                RegisterSendAction(SendMultipleCans);
                _myControllerUdp.AddOvserver(ReceiveMsg);
            }

            public void ReceiveMsg(EndPoint ipEndPointFrom, object dataPackage)
            {
                if (dataPackage.GetType() != typeof(CanDataPackage))
                    return;

                var canDataPackage = dataPackage as CanDataPackage;
                ReceiveCanDatas(new[] { canDataPackage });
            }

            private void SendMultipleCans(CanDataPackage[] dataPackages)
            {
                if (dataPackages == null)
                    return;

                var sendAct = new Action<CanDataPackage[]>(dp =>
                {
                    var datas = new List<byte>();
                    foreach (var p in dp)
                    {
                        var canIdBytes = BitConverter.GetBytes(p.CanId);
                        Array.Reverse(canIdBytes);

                        var canLen = BitConverter.GetBytes((byte)p.CanDataLen);
                        Array.Reverse(canLen);

                        var sendCanBytes = new List<byte>();
                        sendCanBytes.AddRange(canLen); // CAN数据长度
                        sendCanBytes.AddRange(canIdBytes); // CAN ID
                        sendCanBytes.AddRange(new byte[] { 0x00, p.CanType == CanType.Standard ? (byte)0x00 : (byte)0x04 }); // 帧类型，0x0000是标准帧，0x0004是扩展帧
                        sendCanBytes.AddRange(new byte[] { 0x00, p.CanFormat == CanFormat.Data ? (byte)0x00 : (byte)0x01 }); // 帧格式，0x0000是数据帧，0x0001是远程帧，目前只使用数据帧

                        var canRegistersValues = new List<byte>();
                        foreach (var t in p.CanData)
                        {
                            canRegistersValues.Add(0x00);
                            canRegistersValues.Add(t);
                        }
                        sendCanBytes.AddRange(canRegistersValues);
                        datas.AddRange(sendCanBytes);
                    }

                    _myControllerUdp.SendControllerMaseterCan(datas);
                });

                const int maxCount = 9;

                if (dataPackages.Length <= maxCount)
                    sendAct(dataPackages);
                else
                {
                    var count = dataPackages.Length / maxCount;
                    var rest = dataPackages.Length % maxCount;

                    var temp = dataPackages.ToList();
                    for (var i = 0; i < count; i++)
                    {
                        var sendTemp = new CanDataPackage[maxCount];
                        Array.Copy(temp.ToArray(), sendTemp, maxCount);
                        temp.RemoveRange(0, maxCount);
                        sendAct(sendTemp);
                    }

                    if (temp.Any())
                        sendAct(temp.ToArray());
                }
            }

            /// <summary>
            /// 发送标准帧
            /// </summary>
            /// <param name="canId">CAN ID</param>
            /// <param name="values">CAN消息内容</param>
            public void SendStandardCanMsg(
                uint canId, IEnumerable<byte> values)
            {
                SendCanMsg(canId, CanType.Standard, values);
            }

            /// <summary>
            /// 发送扩展帧
            /// </summary>
            /// <param name="canId">CAN ID</param>
            /// <param name="values">CAN消息内容</param>
            public void SendExtendedCanMsg(
                uint canId, IEnumerable<byte> values)
            {
                SendCanMsg(canId, CanType.Extended, values);
            }

            /// <summary>
            /// 发送CAN消息
            /// </summary>
            /// <param name="canId">CAN ID</param>
            /// <param name="canMsgType">0x00标准帧,0x01数据帧</param>
            /// <param name="canMsgValues">CAN消息内容</param>
            private void SendCanMsg(
                uint canId,
                CanType canMsgType,
                IEnumerable<byte> canMsgValues)
            {
                SendMultipleCans(
                    new[] { new CanDataPackage(canId, CanProtocol.Can, canMsgType, CanFormat.Data, canMsgValues.ToArray()) });
            }
        }

        /// <summary>
        /// 主站LIN网关
        /// </summary>
        public class ControllerMasterGatewayLin :
            LinBus, ControllerMasterSocketByUdp.IControllerMasterSocketSubscriber
        {
            public readonly ControllerMasterSocketByUdp MyControllerUdp;

            /// <summary>
            /// 主站LIN网关
            /// </summary>
            public ControllerMasterGatewayLin(
                ControllerMasterSocketByUdp myControllerUdp)
                : base(myControllerUdp.RemoteIpPort)
            {
                MyControllerUdp = myControllerUdp;
                MyControllerUdp.AddOvserver(ReceiveMsg);
                RegisterSendMasterLinFunc(SendMasterLinMessage);
                RegisterSendSlaveLinACtion(SendSlaveLinMessage);
            }

            public void ReceiveMsg(EndPoint ipEndPointFrom, object dataPackage)
            {
                if (dataPackage.GetType() != typeof(LinDataPackage))
                    return;

                var linDataPackage = dataPackage as LinDataPackage;

                if (linDataPackage == null)
                    return;

                RecviveLinDatas(new[] { linDataPackage });

                if (_currenSendtMasterLinId == linDataPackage.LinId &&
                      _currenSendtMasterLinId != 0xFF)
                    MasterLinEventWaitHandle.Set();
            }

            private static byte _currenSendtMasterLinId;
            private static readonly EventWaitHandle MasterLinEventWaitHandle = new AutoResetEvent(false);

            private bool SendMasterLinMessage(LinDataPackage linDataPackage, bool isWaitSlaveLin)
            {
                if (linDataPackage == null)
                    return false;

                MasterLinEventWaitHandle.Reset();
                _currenSendtMasterLinId = ConvertLinId(linDataPackage.LinId);
                MyControllerUdp.SendControllerMasetrLin(linDataPackage.LinId, linDataPackage.LinData);

                if (MasterLinEventWaitHandle.WaitOne(500))
                {
                    _currenSendtMasterLinId = 0xFF;
                    return true;
                }

                _currenSendtMasterLinId = 0xFF;
                return false;
            }

            private void SendSlaveLinMessage(byte slaveLinId)
            {
                MyControllerUdp.SendControllerMasetrLin(slaveLinId, new byte[] { });
            }
        }
    }

    /// <summary>
    /// Controller Master UDP二次封装
    /// </summary>
    public class ControllerMasterSocketByUdp
    {
        public delegate void NotifyEventHandler(
            EndPoint ipEndPointFrom, object dataPackage); // 推送MSG TYPE
        private NotifyEventHandler _notifyEvent;

        public static Dictionary<string, ControllerMasterSocketByUdp> ControllerMasterList =
            new Dictionary<string, ControllerMasterSocketByUdp>();
        private readonly MyUdpClient _myUdpClient;
        public readonly string RemoteIpPort;

        /// <summary>
        /// Controller Master UDP二次封装
        /// </summary>
        public ControllerMasterSocketByUdp(string controllerMasterName, string ipPort)
        {
            if (ControllerMasterList.ContainsKey(controllerMasterName))
                return;

            try
            {
                RemoteIpPort = ipPort;

                var split = RemoteIpPort.Split(':');
                var ipAddressStr = split[0];
                var port = Convert.ToInt32(split[1]);

                if (ipPort.Equals("192.168.1.150:8088")) // 本机debug用
                    _myUdpClient = new MyUdpClient("192.168.1.150", 8089);
                else if (ipPort.Equals("127.0.0.1:8088")) // 本机debug用
                {
                    RemoteIpPort = ipPort;
                    _myUdpClient = new MyUdpClient(
                        "127.0.0.1", 8089);
                }
                else
                {
                    RemoteIpPort = ipPort;
                    _myUdpClient = new MyUdpClient("192.168.1.50",
                        Convert.ToInt32(ipAddressStr.Split('.')[3]) + 5000);
                }

                _myUdpClient.PushMsgEvent += _myUdpClient_PushMsgEvent;
                _myUdpClient.AddRemoteClient(ipAddressStr, port);
                _myUdpClient.BeginReceive();

                ControllerMasterList.Add(controllerMasterName, this);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void Dispose()
        {
            if (_myUdpClient!=null)
                _myUdpClient.Dispose();
        }

        public void AddOvserver(NotifyEventHandler ob)
        {
            _notifyEvent += ob;
        }

        public void RemoveOvserver(NotifyEventHandler ob)
        {
            // ReSharper disable once DelegateSubtraction
            if (_notifyEvent != null)
                _notifyEvent -= ob;
        }

        private void Update(EndPoint ipEndPointFrom, object dataPackage)
        {
            if (_notifyEvent != null)
                _notifyEvent(ipEndPointFrom, dataPackage);
        }

        /// <summary>
        /// 接收消息
        /// 拆包和校验
        /// 解析数据
        /// 推送
        /// </summary>
        /// <param name="ipEndPointFrom">发送消息的节点</param>
        /// <param name="bytes">消息内容</param>
        private void _myUdpClient_PushMsgEvent(
            EndPoint ipEndPointFrom, byte[] bytes)
        {
            if (bytes.Length < 7)
                return;

            var buffLst = bytes.ToList();

            do
            {
                #region 拆包
                if (buffLst.Count < 7)
                {
                    buffLst.Clear();
                    continue;
                }

                var len = buffLst[4] * 256 + buffLst[5];
                if (buffLst.Count < len + 6 + 7)
                {
                    buffLst.Clear();
                    continue;
                }

                var tempLen = len + 6 + 7;
                var temp = new byte[tempLen];
                Array.Copy(buffLst.ToArray(), temp, tempLen);
                buffLst.RemoveRange(0, tempLen);

                var tempContent = new byte[tempLen - 2];
                Array.Copy(temp, tempContent, tempLen - 2);
                #endregion

                #region CRC16-MODBUS校验
                var expectedCrc16 = ValueHelper.Crc16(tempContent).ToArray();
                var actualCrc16 = new[] { temp[tempLen - 2], temp[tempLen - 1] };

                if (expectedCrc16[0] != actualCrc16[0] ||
                    expectedCrc16[1] != actualCrc16[1])
                    continue;

                var totoalLen = temp[4] * 256 + temp[5];
                var registerCount = temp[10] * 256 + temp[11];
                if (totoalLen != 7 + registerCount * 2)
                    continue; // 总长度与寄存器数量不符合

                if (temp.Length - 6 - 7 != totoalLen)
                    continue; // 数组长度不正确
                #endregion

                #region 解析数据并推送
                var function = EnumOperater.GetEnumByValue<ControllerFunctionCode>(temp[7]);
                var contentBytes = new byte[registerCount * 2];
                Array.Copy(bytes.ToArray(), 13, contentBytes, 0, registerCount * 2);

                switch (function)
                {
                    case ControllerFunctionCode.DataRead:
                        var startAddrBs = new[] { temp[8], temp[9] };
                        Array.Reverse(startAddrBs);
                        var startAddr = BitConverter.ToUInt16(startAddrBs, 0);
                        Update(ipEndPointFrom, new MasterDataPackage(startAddr, contentBytes));
                        break;

                    case ControllerFunctionCode.DataWrite:
                        break;

                    case ControllerFunctionCode.GetTime:
                        if (contentBytes.Length != 12 * 2)
                            return;

                        var floatBytes = new[] { contentBytes[20], contentBytes[21], contentBytes[22], contentBytes[23] };
                        Array.Reverse(floatBytes);
                        Update(ipEndPointFrom,
                            new MasterTimeDataPackage { GetTime = BitConverter.ToSingle(floatBytes, 0) });
                        break;

                    case ControllerFunctionCode.GatewaySci:
                        var sciBytes = new List<byte>();
                        for (var i = 0; i < contentBytes.Length; i = i + 2)
                            sciBytes.Add(Convert.ToByte(contentBytes[i] * 256 + contentBytes[i + 1]));
                        Update(
                            ipEndPointFrom,
                            new SciDataPackage(sciBytes.ToArray()));
                        break;

                    case ControllerFunctionCode.GatewayCan:
                        var canBytes = contentBytes.ToList();
                        while (true)
                        {
                            if (canBytes.Count <= 0)
                                break;

                            var canLen = canBytes[0] * 256 + canBytes[1];
                            var canId = BitConverter.ToUInt32(
                                new[] { canBytes[5], canBytes[4], canBytes[3], canBytes[2] }, 0);
                            var canType = canBytes[7];
                            var canFormat = canBytes[9];
                            var data = new List<byte>();
                            for (var i = 10; i < 10 + canLen * 2; i = i + 2)
                                data.Add(canBytes[i + 1]);
                            canBytes.RemoveRange(0, 10 + canLen * 2);
                            Update(
                                ipEndPointFrom,
                                new CanBus.CanDataPackage(canId, CanBus.CanProtocol.Can,
                                    canType == 0x00 ? CanBus.CanType.Standard : CanBus.CanType.Extended,
                                    canFormat == 0x00 ? CanBus.CanFormat.Data : CanBus.CanFormat.Remote,
                                    data.ToArray()));
                        }
                        break;

                    case ControllerFunctionCode.GatewayLin:
                        try
                        {
                            var linBytes = contentBytes.ToList();
                            if (linBytes.Count < 6)
                                continue;

                            var linId = linBytes[5];
                            linBytes.RemoveRange(0, 6);

                            if (linBytes.Count % 2 != 0)
                                continue;

                            var tempB = new List<byte>();
                            for (var i = 0; i < linBytes.Count; i = i + 2)
                                tempB.Add(linBytes[i + 1]);

                            if (tempB.Count > 8)
                            {
                                var c = tempB.Count;
                                tempB.RemoveAt(c - 1);
                            }

                            var bys = new byte[tempB.Count >= 8 ? tempB.Count : tempB.Count - 1];
                            Array.Copy(tempB.ToArray(), bys, bys.Length);

                            //var str = bys.Aggregate(string.Empty, (current, t) => current + (ValueHelper.GetHextStrWithOx(t) + " "));

                            //Console.WriteLine("{0} {1}", ValueHelper.GetHextStrWithOx(linId), str);
                            Update(ipEndPointFrom, new LinBus.LinDataPackage(linId, bys));
                        }
                        catch (Exception)
                        {
                            // ignored
                        }
                        break;

                    case ControllerFunctionCode.Can1ToSlave:
                        if (registerCount <= 4)
                            continue;

                        var slaveCanId = BitConverter.ToUInt32(
                            new[] { contentBytes[3], contentBytes[2], contentBytes[1], contentBytes[0] }, 0);
                        var datas = new List<byte>();
                        for (var i = 8; i < contentBytes.Length; i = i + 2)
                            datas.Add(contentBytes[i + 1]);
                        Update(
                            ipEndPointFrom, new SlaveDataPackage(slaveCanId, datas.ToArray()));
                        break;

                    case ControllerFunctionCode.FlashWrite:
                        break;

                    case ControllerFunctionCode.FlashRead:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
                #endregion
            } while (buffLst.Count != 0);
        }

        private byte _sendTrackId;
        private DateTime _powerOnTime = DateTime.Now;

        /// <summary>
        /// 发送数据
        /// 在此处打包数据，添加索引、时间戳和CRC16-MODBUS校验
        /// </summary>
        /// <param name="bytes">需要打包发送的数据</param>
        private void SendMsg(IEnumerable<byte> bytes, int delayMs = 10)
        {
            var sendBytes = new List<byte>();
            sendBytes.AddRange(new byte[] { 0x01, 0x00 }); // data0,data1
            sendBytes.AddRange(new byte[] { 0x00, 0x01 }); // data2,data3

            sendBytes.AddRange(bytes);
            sendBytes.Add(_sendTrackId);
            _sendTrackId = _sendTrackId == byte.MaxValue ? (byte)0 : (byte)(_sendTrackId + 1);

            var timespanMs = (float)(DateTime.Now - _powerOnTime).TotalMilliseconds;
            if (float.IsInfinity(timespanMs))
            {
                timespanMs = 0;
                _powerOnTime = DateTime.Now;
            }
            sendBytes.AddRange(BitConverter.GetBytes(timespanMs));
            sendBytes.AddRange(ValueHelper.Crc16(sendBytes));

            _myUdpClient.SendMsgTo(
                new IPEndPoint(IPAddress.Parse(RemoteIpPort.Split(':')[0]), int.Parse(RemoteIpPort.Split(':')[1])),
                sendBytes.ToArray(), delayMs);
        }

        public void SendCan1ToSlave(uint canId, IEnumerable<byte> values)
        {
            var valueBytes = new List<byte>();
            var canIdBytes = BitConverter.GetBytes(canId);
            Array.Reverse(canIdBytes);
            valueBytes.AddRange(canIdBytes);

            valueBytes.AddRange(new byte[] { 0x00, 0x00 });
            valueBytes.AddRange(new byte[] { 0x00, 0x00 });

            foreach (var t in values)
                valueBytes.AddRange(new byte[] { 0x00, t });

            var lstBytes = new List<byte>();
            lstBytes.AddRange(
                ModbusOperater.GetModbusWriteValues((byte)ControllerFunctionCode.Can1ToSlave,
                0x00.ToString(),
                (ushort)(valueBytes.Count / 2), valueBytes.ToArray()));
            SendMsg(lstBytes);
        }

        public void SendControllerMaseterCan(
           IEnumerable<byte> values)
        {
            var sendBytes = new List<byte>();

            //sendBytes.AddRange(new byte[] { 0x01, 0x00 });
            //sendBytes.AddRange(new byte[] { 0x00, 0x01 });

            //sendBytes.AddRange(values);

            const byte codeId = (byte)ControllerFunctionCode.GatewayCan;
            var startAddr = 0.ToString();
            var enumerable = values as byte[] ?? values.ToArray();
            var len = (ushort)(enumerable.Length / 2);
            sendBytes.AddRange(
                ModbusOperater.GetModbusWriteValues(codeId, startAddr, len, enumerable.ToArray()));

            SendMsg(sendBytes, 5);
        }

        public void SendControllerMasetrLin(
            byte linId, IEnumerable<byte> value)
        {
            var msgValues = value as byte[] ?? value.ToArray();
            if (msgValues.Length > 8)
                return;

            var linIdBytes = new byte[] { 0x00, linId };
            var sendLinBytes = linIdBytes.ToList();

            var linRegistersValues = new List<byte>();
            foreach (var t in msgValues)
            {
                linRegistersValues.Add(0x00);
                linRegistersValues.Add(t);
            }
            sendLinBytes.AddRange(linRegistersValues);

            var sendBytes = new List<byte>();
            sendBytes.AddRange(
                ModbusOperater.GetModbusWriteValues((byte)ControllerFunctionCode.GatewayLin,
                "0", (ushort)(sendLinBytes.Count / 2), sendLinBytes.ToArray()));

            SendMsg(sendBytes);

            //var tempBytes = new List<byte> { linId };
            //tempBytes.AddRange(value);
            //_myUdpClient.SendMsgTo(
            //   new IPEndPoint(IPAddress.Parse(RemoteIpPort.Split(':')[0]), int.Parse(RemoteIpPort.Split(':')[1])),
            //   tempBytes.ToArray());
        }

        /// <summary>
        /// 发送串口数据
        /// </summary>
        /// <param name="values"></param>
        public void SendControllerMasterSci(IEnumerable<byte> values)
        {
            var enumerable = values as byte[] ?? values.ToArray();
            var len = enumerable.Length;
            var remoteSetUshorts = new ushort[len];
            Array.Copy(enumerable.ToArray(), remoteSetUshorts, len);
            var registersBytes = new List<byte>();
            foreach (var temp in remoteSetUshorts.Select(BitConverter.GetBytes))
            {
                Array.Reverse(temp);
                registersBytes.AddRange(temp);
            }

            var sendSetRemoteSetBytes = new List<byte>();

            // 总长度
            var totalLenBytes = BitConverter.GetBytes((ushort)(2 + 2 + 2 + 1 + registersBytes.Count));
            Array.Reverse(totalLenBytes);
            sendSetRemoteSetBytes.AddRange(totalLenBytes);

            sendSetRemoteSetBytes.AddRange(new byte[] { 0x02, 0x64, 0x00, 0x00 });

            // 寄存器个数
            var registersCountBytes = BitConverter.GetBytes((ushort)remoteSetUshorts.Length);
            Array.Reverse(registersCountBytes);
            sendSetRemoteSetBytes.AddRange(registersCountBytes);

            // 寄存器总字节数
            var registerLenBytes = BitConverter.GetBytes((ushort)registersBytes.Count);
            Array.Reverse(registerLenBytes);
            sendSetRemoteSetBytes.Add(registerLenBytes[1]);

            // 寄存器内容
            sendSetRemoteSetBytes.AddRange(registersBytes);

            SendMsg(sendSetRemoteSetBytes);
        }

        /// <summary>
        /// 0x03 读取主站数据
        /// </summary>
        /// <param name="startAddr"></param>
        /// <param name="registerCount"></param>
        public void SendControllerDataRead(
            string startAddr, ushort registerCount)
        {
            var readBytes = new List<byte>();
            readBytes.AddRange(new byte[] { 0x00, 0x06 });
            readBytes.Add(0x01);
            readBytes.Add((byte)ControllerFunctionCode.DataRead);

            var startAddressBs = BitConverter.GetBytes(Convert.ToUInt16(startAddr));
            Array.Reverse(startAddressBs);
            readBytes.AddRange(startAddressBs);

            var registerCountBs = BitConverter.GetBytes(Convert.ToUInt16(registerCount));
            Array.Reverse(registerCountBs);
            readBytes.AddRange(registerCountBs);

            SendMsg(readBytes);
        }

        /// <summary>
        /// 0x10 写主站数据
        /// </summary>
        /// <param name="startAddr"></param>
        /// <param name="registerCount"></param>
        /// <param name="values"></param>
        public void SendControllerDataWrite(
            string startAddr, ushort registerCount, IEnumerable<byte> values)
        {
            var sendBytes = new List<byte>();
            sendBytes.AddRange(ModbusOperater.GetModbusWriteValues((byte)ControllerFunctionCode.DataWrite,
                startAddr, registerCount, values.ToArray()));

            SendMsg(sendBytes);
        }

        /// <summary>
        /// 获取启动时间
        /// </summary>
        /// <param name="rlSlaveCanId"></param>
        /// <param name="adSlaveCanId"></param>
        /// <param name="relays"></param>
        /// <param name="isAdCurrent"></param>
        /// <param name="adIndex"></param>
        /// <param name="isRising"></param>
        /// <param name="threshold"></param>
        public void SendControllerGetTime(
            uint rlSlaveCanId,
            uint adSlaveCanId,
            byte[] relays, bool isAdCurrent, byte adIndex, bool isRising, float threshold)
        {
            var valueBytes = new List<byte>();

            var rlSlaveCanIdBytes = BitConverter.GetBytes(rlSlaveCanId);
            Array.Reverse(rlSlaveCanIdBytes);
            valueBytes.AddRange(rlSlaveCanIdBytes);

            valueBytes.AddRange(new byte[] { 0x00, 0x00 });
            valueBytes.AddRange(relays);

            var adSlaveCanIdBytes = BitConverter.GetBytes(adSlaveCanId);
            Array.Reverse(adSlaveCanIdBytes);
            valueBytes.AddRange(adSlaveCanIdBytes);

            valueBytes.Add(isAdCurrent ? (byte)0x00 : (byte)0x01);
            valueBytes.Add(adIndex);

            valueBytes.AddRange(new byte[] { 0x00, isRising ? (byte)0x00 : (byte)0x01 });

            var fb = BitConverter.GetBytes(threshold);
            Array.Reverse(fb);
            valueBytes.AddRange(fb);

            var lstBytes = new List<byte>();
            lstBytes.AddRange(
                ModbusOperater.GetModbusWriteValues((byte)ControllerFunctionCode.GetTime,
                0x00.ToString(),
                (ushort)(valueBytes.Count / 2), valueBytes.ToArray()));
            SendMsg(lstBytes);
        }

        /// <summary>
        /// 功能码
        /// </summary>
        public enum ControllerFunctionCode : byte
        {
            /// <summary>
            /// 读取Modbus寄存器
            /// </summary>
            DataRead = 0x03,

            /// <summary>
            /// 写Modbus寄存器
            /// </summary>
            DataWrite = 0x10,

            /// <summary>
            /// 转发串口
            /// </summary>
            GatewaySci = 0x64,

            /// <summary>
            /// 转发CAN
            /// </summary>
            GatewayCan = 0x65,

            /// <summary>
            /// 转发LIN
            /// </summary>
            GatewayLin = 0x66,

            /// <summary>
            /// 从站控制（连接CAN1接口）
            /// </summary>
            Can1ToSlave = 0x67,

            /// <summary>
            /// 写flash配置信息（IP等）
            /// </summary>
            FlashWrite = 0x68,

            /// <summary>
            /// 读flash配置信息（IP等）
            /// </summary>
            FlashRead = 0x69,

            /// <summary>
            /// 读延时时间
            /// </summary>
            GetTime = 0x6B
        }

        public interface IControllerMasterSocketSubscriber
        {
            /// <summary>
            /// 接收消息
            /// </summary>
            /// <param name="ipEndPointFrom">消息发送过来的远程节点</param>
            /// <param name="dataPackage">消息内容</param>
            void ReceiveMsg(EndPoint ipEndPointFrom, object dataPackage);
        }

        public class MasterDataPackage
        {
            public ushort StartAddress { get; set; }
            public ushort RegisterCount { get; set; }
            public byte[] Bytes { get; set; }

            public MasterDataPackage(ushort startAddress, IReadOnlyCollection<byte> valueBytes)
            {
                StartAddress = startAddress;
                RegisterCount = (ushort)(valueBytes.Count / 2);
                Bytes = new byte[valueBytes.Count];
                Array.Copy(valueBytes.ToArray(), Bytes, Bytes.Length);
            }
        }

        public class MasterTimeDataPackage
        {
            public float GetTime { get; set; }
        }

        public class MasterFlashDataPackage
        {
            public ushort StartAddress { get; set; }
            public ushort RegisterCount { get; set; }
            public byte[] Bytes { get; set; }
        }

        public class SciDataPackage
        {
            /// <summary>
            /// 消息唯一标识
            /// </summary>
            public string MessageKey { get; private set; }

            /// <summary>
            /// 收到消息的时间
            /// </summary>
            public DateTime RecvDateTime { get; private set; }

            /// <summary>
            /// 收到消息长度
            /// </summary>
            public int RecvLen { get; private set; }

            /// <summary>
            /// 收到消息内容
            /// </summary>
            public byte[] RecvData { get; private set; }

            public SciDataPackage(byte[] recvData)
            {
                MessageKey = Guid.NewGuid().ToString();
                RecvDateTime = DateTime.Now;
                RecvData = recvData;
                RecvLen = recvData.Length;
            }
        }

        public class SlaveDataPackage
        {
            public uint CanId { get; set; }

            public byte ServiceIdentifierByte { get; set; }

            public byte[] DataBytes { get; set; }

            public SlaveDataPackage(uint canId, byte[] dataBytes)
            {
                CanId = canId;
                DataBytes = dataBytes;
            }
        }

        public class SlaveFlashDataPackage
        {

        }
    }
}
