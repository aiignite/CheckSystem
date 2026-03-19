using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using CommonUtility;
using System.Text;
using CommonUtility.BusLoader;
using Controller.HardwareController;

namespace Controller
{
    public sealed class ControllerWithGateway :
        ControllerBase,
        IGatewaySocketViaUdpSubscriber, ICcdIoController
    {
        public bool SetOutputs()
        {
            return SetRelays();
        }

        public void GetInputs()
        {
            ReadInput();
            UpdateCurrsAndVolts();
        }

        public void ResetOutPuts()
        {
            ResetAllRelays();
        }

        private readonly object _operaterLocker = new object();

        public GatewaySocketViaUdp MySocketViaUdp;
        public GatewaySerailPort VirtualSerialPort;
        public CanBus GatewayCan1;
        public LinBus GatewayLin1;

        [Description("R,电流1")]
        public float Current1;

        [Description("R,电流2")]
        public float Current2;

        [Description("R,电流3")]
        public float Current3;

        [Description("R,电流4")]
        public float Current4;

        [Description("R,电压1")]
        public float Voltage1;

        [Description("R,电压2")]
        public float Voltage2;

        [Description("R,电压3")]
        public float Voltage3;

        [Description("R,电压4")]
        public float Voltage4;

        [Description("R,电压5")]
        public float Voltage5;

        public ushort DelayTimeChannel;
        public ushort DelayTimeJudgment; // <是1 >是0
        public ushort DelayTimeRange; // 最小值的0.3倍再除以采样倍数
        public float DelayTime;

        [Description("R/W,继电器1")]
        public bool Relay1;

        [Description("R/W,继电器2")]
        public bool Relay2;

        [Description("R/W,继电器3")]
        public bool Relay3;

        [Description("R/W,继电器4")]
        public bool Relay4;

        [Description("R/W,继电器5")]
        public bool Relay5;

        [Description("R/W,继电器6")]
        public bool Relay6;

        [Description("R/W,继电器7")]
        public bool Relay7;

        [Description("R/W,继电器8")]
        public bool Relay8;

        [Description("R/W,继电器9")]
        public bool Relay9;

        [Description("R,DI")]
        public bool? Input1;

        public ControllerWithGateway(string name)
            : base(name)
        {
            foreach (var temp in EnumOperater.GetEnumValueList<ControllerOperationType>())
                _gatewayOperaterWhDictionary.Add(temp, new AutoResetEvent(false));
        }

        ~ControllerWithGateway()
        {
            Dispose();
        }

        private readonly Dictionary<ControllerOperationType, EventWaitHandle> _gatewayOperaterWhDictionary =
            new Dictionary<ControllerOperationType, EventWaitHandle>();

        private ushort _samplingFrequency = 1000;
        private float _currentCheckFactor = (float)0.53192;
        private float _voltageCheckFactor = (float)16.9231;

        private ushort _current1Channel = 4;
        private ushort _current2Channel = 8;
        private ushort _current3Channel = 9;
        private ushort _current4Channel = 15;
        private ushort _voltage1Channel = 5;
        private ushort _voltage2Channel = 6;
        private ushort _voltage3Channel = 260;
        private ushort _voltage4Channel = 261;
        private ushort _voltage5Channel = 262;

        public void InitRemoteIpAddress(string ipPort)
        {
            MySocketViaUdp = new GatewaySocketViaUdp(ipPort);
            MySocketViaUdp.AddOvserver(ReceiveMsg);

            InitCanPort(1);
            InitLinPort(1);
        }

        public void ReceiveMsg(
            EndPoint ipEndPoint, byte[] bytes)
        {
            try
            {
                switch (bytes[7])
                {
                    case 0x67:
                        if (bytes[6] == 0x00) // DelayTime
                        {
                            DelayTime =
                                bytes[13] * 256 + bytes[14];
                            _gatewayOperaterWhDictionary[ControllerOperationType.UpdateDelayTime].Set();
                        }
                        else if (bytes[6] == 0x01) // Curr and Volt
                        {
                            Current1 =
                                (float)
                                    Math.Round(_currentCheckFactor * (bytes[15] * 256 + bytes[16]), 4,
                                        MidpointRounding.AwayFromZero);
                            Current2 =
                                (float)
                                    Math.Round(_currentCheckFactor * (bytes[17] * 256 + bytes[18]), 4,
                                        MidpointRounding.AwayFromZero);
                            Current3 =
                                (float)
                                    Math.Round(_currentCheckFactor * (bytes[19] * 256 + bytes[20]), 4,
                                        MidpointRounding.AwayFromZero);
                            Current4 =
                                (float)
                                    Math.Round(_currentCheckFactor * (bytes[21] * 256 + bytes[22]), 4,
                                        MidpointRounding.AwayFromZero);
                            Voltage1 =
                                (float)
                                    Math.Round(_voltageCheckFactor * (bytes[23] * 256 + bytes[24]) / 1000, 4,
                                        MidpointRounding.AwayFromZero);
                            Voltage2 =
                                (float)
                                    Math.Round(_voltageCheckFactor * (bytes[25] * 256 + bytes[26]) / 1000, 4,
                                        MidpointRounding.AwayFromZero);
                            Voltage3 =
                                (float)
                                    Math.Round(_voltageCheckFactor * (bytes[27] * 256 + bytes[28]) / 1000, 4,
                                        MidpointRounding.AwayFromZero);
                            Voltage4 =
                                (float)
                                    Math.Round(_voltageCheckFactor * (bytes[29] * 256 + bytes[30]) / 1000, 4,
                                        MidpointRounding.AwayFromZero);
                            Voltage5 =
                                (float)
                                    Math.Round(_voltageCheckFactor * (bytes[31] * 256 + bytes[32]) / 1000, 4,
                                        MidpointRounding.AwayFromZero);

                            _gatewayOperaterWhDictionary[ControllerOperationType.UpdateCurrAndVolts].Set();
                        }
                        break;

                    case 0x10:
                        if (bytes[8] * 256 + bytes[9] == 0x0202 || bytes[8] * 256 + bytes[9] == 0x0200)
                            _gatewayOperaterWhDictionary[ControllerOperationType.Pwm].Set();
                        else
                            _gatewayOperaterWhDictionary[ControllerOperationType.SetRepleys].Set();
                        break;

                    case 0x03:
                        var val = bytes[13] * 256 + bytes[14];
                        Input1 = val == 1;
                        _gatewayOperaterWhDictionary[ControllerOperationType.ReadInput].Set();
                        break;
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// 初始化下位机的串口
        /// </summary>
        /// <param name="strPort">波特率</param>
        public void InitSerialPort(string strPort)
        {
            VirtualSerialPort = new GatewaySerailPort(MySocketViaUdp);
        }

        private void InitLinPort(byte linId)
        {
            var memberInfo = GetType()
                .GetField("GatewayLin" + linId);
            if (memberInfo == null)
                return;
            var createLin =
                Activator.CreateInstance(typeof(GatewayLin), linId, MySocketViaUdp);
            memberInfo.SetValue(this, createLin);
        }

        private void InitCanPort(byte canChannelId)
        {
            var memberInfo = GetType()
                .GetField("GatewayCan" + canChannelId);
            if (memberInfo == null)
                return;
            var createCan =
                Activator.CreateInstance(typeof(GatewayCan), canChannelId, MySocketViaUdp);
            memberInfo.SetValue(this, createCan);
        }

        /// <summary>
        /// 设置继电器
        /// </summary>
        /// <returns></returns>
        [Description("设置继电器")]
        public bool SetRelays()
        {
            var relayBytes = new byte[18];
            for (var i = 1; i < 10; i++)
            {
                var fieldInfo =
                    GetType().GetField("Relay" + i).GetValue(this);
                relayBytes[(i - 1) * 2 + 1] =
                    (bool)fieldInfo ? (byte)1 : (byte)0;
            }

            return GatewayOperater(
                 ControllerOperationType.SetRepleys, "256", 9, relayBytes);
        }

        //public void SetRelays(string val)
        //{
        //    if (val.Length != 9)
        //        throw new Exception("");

        //    if (val.Where(w => !w.ToString().Equals("1") && !w.ToString().Equals("0")).ToArray().Length != 0)
        //        throw new Exception("");

        //    for (var i = 0; i < val.Length; i++)
        //    {
        //        if (i + 1 == 5)
        //            continue;
        //        var field = GetType().GetField("Relay" + (i + 1));
        //        field.SetValue(this, val[i].ToString().Equals("1"));
        //    }

        //    SetRelays();
        //}

        [Description("重置所有继电器")]
        public void ResetAllRelays()
        {
            Relay1 = false;
            Relay2 = false;
            Relay3 = false;
            Relay4 = false;
            Relay5 = false;
            Relay6 = false;
            Relay7 = false;
            Relay8 = false;
            Relay9 = false;
            SetRelays();
        }

        /// <summary>
        /// 更新电流和电压
        /// </summary>
        /// <returns></returns>
        [Description("更新电流和电压")]
        public bool UpdateCurrsAndVolts()
        {
            var updateBytes = new List<byte>();

            var samplingFrequencyBytes =
                BitConverter.GetBytes(_samplingFrequency);
            Array.Reverse(samplingFrequencyBytes);
            updateBytes.AddRange(samplingFrequencyBytes);

            for (var i = 1; i < 5; i++)
            {
                var memberInfo = GetType()
                    .GetField("_current" + i + "Channel", BindingFlags.NonPublic | BindingFlags.Instance);
                if (memberInfo == null)
                    continue;
                var fieldInfo = memberInfo.GetValue(this);
                var currentChannel = BitConverter.GetBytes((ushort)fieldInfo);
                Array.Reverse(currentChannel);
                updateBytes.AddRange(currentChannel);
            }

            for (var i = 1; i < 6; i++)
            {
                var memberInfo = GetType()
                    .GetField("_voltage" + i + "Channel", BindingFlags.NonPublic | BindingFlags.Instance);
                if (memberInfo == null)
                    continue;
                var fieldInfo = memberInfo.GetValue(this);
                var voltChannel =
                    BitConverter.GetBytes((ushort)fieldInfo);
                Array.Reverse(voltChannel);
                updateBytes.AddRange(voltChannel);
            }

            var isUpdateSuccess = GatewayOperater(
                ControllerOperationType.UpdateCurrAndVolts,
                "0",
                (ushort)(updateBytes.Count / 2), updateBytes.ToArray(),
                1000);

            if (isUpdateSuccess)
                return true;

            Current1 = -99999;
            Current2 = -99999;
            Current3 = -99999;
            Current4 = -99999;
            Voltage1 = -99999;
            Voltage2 = -99999;
            Voltage3 = -99999;
            Voltage4 = -99999;
            Voltage5 = -99999;
            return false;
        }

        /// <summary>
        /// 更新延时时间
        /// </summary>
        public bool UpdateDelayTime()
        {
            var relayBytes = new byte[18];
            for (var i = 1; i < 10; i++)
            {
                var fieldInfo =
                    GetType().GetField("Relay" + i).GetValue(this);
                relayBytes[(i - 1) * 2 + 1] =
                    (bool)fieldInfo ? (byte)1 : (byte)0;
            }

            var judgeBytes =
                BitConverter.GetBytes(DelayTimeJudgment);
            Array.Reverse(judgeBytes);

            var channelBytes =
                BitConverter.GetBytes(DelayTimeChannel);
            Array.Reverse(channelBytes);

            var rangeBytes =
                BitConverter.GetBytes(DelayTimeRange);
            Array.Reverse(rangeBytes);

            var readDelayTimeBytes = new List<byte>();
            readDelayTimeBytes.AddRange(relayBytes);
            readDelayTimeBytes.AddRange(judgeBytes);
            readDelayTimeBytes.AddRange(channelBytes);
            readDelayTimeBytes.AddRange(rangeBytes);

            var isUpdateSuccess = GatewayOperater(
                ControllerOperationType.UpdateDelayTime,
                "0",
                (ushort)(readDelayTimeBytes.Count / 2),
                readDelayTimeBytes.ToArray(),
                800);

            if (isUpdateSuccess)
                return true;

            DelayTime = -99999;
            return false;
        }

        [Description("读取DI状态")]
        public bool ReadInput()
        {
            Input1 = null;

            var readBytes = new List<byte>();
            //readBytes.AddRange(new byte[] { 0x01, 0x00 });
            //readBytes.AddRange(new byte[] { 0x00, 0x01 });
            readBytes.AddRange(new byte[] { 0x00, 0x06 });
            readBytes.Add(0x01);
            readBytes.Add(0x03);
            readBytes.AddRange(new byte[] { 0x00, 0x90 });
            readBytes.AddRange(new byte[] { 0x00, 0x01 });

            var isSuccess = GatewayOperater(ControllerOperationType.ReadInput, "", 0, readBytes.ToArray());

            if (isSuccess)
                return true;

            Input1 = null;
            return false;
        }

        [Description("打开通道1的PWM输出")]
        public bool OpenOut1Pwm(string hzPercentage)
        {
            return SetPwn(1, hzPercentage);
        }

        [Description("关闭通道1的PWM输出")]
        public bool CloseOut1Pwm()
        {
            return SetPwn(1, "1:0");
        }

        [Description("打开通道2的PWM输出")]
        public bool OpenOut2Pwm(string hzPercentage)
        {
            return SetPwn(2, hzPercentage);
        }

        [Description("关闭通道2的PWM输出")]
        public bool CloseOut2Pwm()
        {
            return SetPwn(2, "1:0");
        }

        private bool SetPwn(int outIndex, string hzPercentage)
        {
            var hz = Convert.ToUInt16(hzPercentage.Split(':')[0]);
            var percentage = Convert.ToUInt16(hzPercentage.Split(':')[1]);

            var hzBytes = BitConverter.GetBytes(hz);
            Array.Reverse(hzBytes);

            var percentageBytes = BitConverter.GetBytes(percentage);
            Array.Reverse(percentageBytes);

            var startAddress = "512";
            if (outIndex == 2)
                startAddress = "514";

            var val = new List<byte>();
            val.AddRange(hzBytes);
            val.AddRange(percentageBytes);

            return GatewayOperater(ControllerOperationType.Pwm, startAddress, (ushort)(val.Count / 2), val.ToArray());
        }

        private bool GatewayOperater(
            ControllerOperationType opType,
            string startAddress,
            ushort len,
            byte[] values = null,
            int timeOut = 500)
        {
            if (MySocketViaUdp == null)
                return false;

            lock (_operaterLocker)
            {
                foreach (
                var op
                in _gatewayOperaterWhDictionary.Values)
                    op.Reset();

                var sendBytes = new List<byte>();
                sendBytes.AddRange(new byte[] { 0x01, 0x00 });
                sendBytes.AddRange(new byte[] { 0x00, 0x01 });

                switch (opType)
                {
                    case ControllerOperationType.SetRepleys:
                        sendBytes.AddRange(ModbusOperater.GetModbusWriteValues(
                            0x10, startAddress, len, values));
                        break;

                    case ControllerOperationType.UpdateCurrAndVolts:
                        sendBytes.AddRange(ModbusOperater.GetModbusWriteValues(
                            0x67, startAddress, len, values));
                        break;

                    case ControllerOperationType.UpdateDelayTime:
                        sendBytes.AddRange(ModbusOperater.GetModbusWriteValues(
                            0x67, startAddress, len, values, 0x00));
                        break;

                    case ControllerOperationType.Pwm:
                        sendBytes.AddRange(ModbusOperater.GetModbusWriteValues(
                            0x10, startAddress, len, values));
                        break;

                    case ControllerOperationType.ReadInput:
                        if (values != null) sendBytes.AddRange(values);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException("opType", opType, null);
                }

                MySocketViaUdp.SendMsg(sendBytes.ToArray());
                return
                    _gatewayOperaterWhDictionary[opType].WaitOne(timeOut);
            }
        }

        private enum ControllerOperationType
        {
            /// <summary>
            /// 操作继电器
            /// </summary>
            SetRepleys,

            /// <summary>
            /// 更新电流和电压
            /// </summary>
            UpdateCurrAndVolts,

            /// <summary>
            /// 更新延时时间
            /// </summary>
            UpdateDelayTime,

            /// <summary>
            /// 操作PWN
            /// </summary>
            Pwm,

            /// <summary>
            /// 读输入
            /// </summary>
            ReadInput
        }
    }

    public class GatewaySerailPort : MySerialPort, IGatewaySocketViaUdpSubscriber
    {
        private readonly GatewaySocketViaUdp _mySocketViaUdp;
        private string _strTemp;

        public GatewaySerailPort(GatewaySocketViaUdp mySocketViaUdp)
            : base(mySocketViaUdp.RemoteIpPort)
        {
            _mySocketViaUdp = mySocketViaUdp;
            _mySocketViaUdp.AddOvserver(ReceiveMsg);
        }

        public void ReceiveMsg(EndPoint ipEndPointFrom, byte[] bytes)
        {
            var str = GetGatewaySerialPortStr(bytes);
            if (!string.IsNullOrEmpty(str))
                _strTemp += str;
        }

        public override void SendCommand(string str)
        {
            var bytes = GetGatewaySeriallPortBytes(str);
            _mySocketViaUdp.SendMsg(bytes);
        }

        public override void SendCommand(byte[] bytes, int delyMs = 25)
        {
            _mySocketViaUdp.SendMsg(GetGatewaySeriallPortBytes(bytes));
        }

        public override string ReadDataStr()
        {
            Thread.Sleep(250);
            lock (_strTemp)
            {
                var returnMsg = _strTemp;
                _strTemp = string.Empty;
                return returnMsg;
            }
        }

        public override byte[] ReadDataBytes()
        {
            Thread.Sleep(50);
            lock (_strTemp)
            {
                var returnMsg = Encoding.ASCII.GetBytes(_strTemp);
                _strTemp = string.Empty;
                return returnMsg;
            }
        }

        private static IEnumerable<byte> GetGatewaySeriallPortBytes(string command)
        {
            var remoteSetBytes = Encoding.ASCII.GetBytes(command);
            var len = remoteSetBytes.Length;
            var remoteSetUshorts = new ushort[len];
            Array.Copy(remoteSetBytes, remoteSetUshorts, len);
            var registersBytes = new List<byte>();
            foreach (var temp in remoteSetUshorts.Select(BitConverter.GetBytes))
            {
                Array.Reverse(temp);
                registersBytes.AddRange(temp);
            }

            var sendSetRemoteSetBytes = new List<byte>();
            sendSetRemoteSetBytes.AddRange(new byte[] { 0x01, 0x00, 0x00, 0x01 });

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

            return sendSetRemoteSetBytes.ToArray();
        }

        private static IEnumerable<byte> GetGatewaySeriallPortBytes(byte[] remoteSetBytes)
        {
            //var remoteSetBytes = command;
            var len = remoteSetBytes.Length;
            var remoteSetUshorts = new ushort[len];
            Array.Copy(remoteSetBytes, remoteSetUshorts, len);
            var registersBytes = new List<byte>();
            foreach (var temp in remoteSetUshorts.Select(BitConverter.GetBytes))
            {
                Array.Reverse(temp);
                registersBytes.AddRange(temp);
            }

            var sendSetRemoteSetBytes = new List<byte>();
            sendSetRemoteSetBytes.AddRange(new byte[] { 0x01, 0x00, 0x00, 0x01 });

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

            return sendSetRemoteSetBytes.ToArray();
        }

        private static string GetGatewaySerialPortStr(IReadOnlyList<byte> bytes)
        {
            if (bytes == null)
                return null;

            if (bytes.Count <= 13)
                return null;

            if (bytes[7] != 0x64)
                return null;
            //throw new Exception("串口接收到的数据长度不正确，当前长度是：" + bytes.Count);

            var totoalLen = bytes[4] * 256 + bytes[5];
            var registerCount = bytes[10] * 256 + bytes[11];

            if (totoalLen != 7 + registerCount * 2)
                return null;
            //throw new Exception("总长度与寄存器数量不符合，总长度是：" + totoalLen + "，寄存器数量是：" + registerCount);

            if (bytes.Count - 6 - 7 != totoalLen)
                return null;
            //throw new Exception("数组长度不正确");

            var contentBytes = new byte[registerCount * 2];
            Array.Copy(bytes.ToArray(), 13, contentBytes, 0, registerCount * 2);

            var ascii = new List<byte>();
            for (var i = 0; i < contentBytes.Length; i = i + 2)
                ascii.Add(Convert.ToByte(contentBytes[i] * 256 + contentBytes[i + 1]));

            var str = Encoding.ASCII.GetString(ascii.ToArray());

            return str;
        }
    }

    /// <summary>
    /// CAN网关
    /// </summary>
    public class GatewayCan : CanBus, IGatewaySocketViaUdpSubscriber
    {
        /// <summary>
        /// CAN通道ID
        /// </summary>
        private static byte _canChannelId;

        /// <summary>
        /// CAN网关功能码
        /// </summary>
        private const byte CodeId = 0x69;

        /// <summary>
        /// CAN转发网络
        /// </summary>
        private readonly GatewaySocketViaUdp _mySocketViaUdp;

        /// <summary>
        /// CAN网关
        /// </summary>
        /// <param name="canChannelId">CAN通道ID</param>
        /// <param name="mySocketViaUdp">CAN转发网络</param>
        public GatewayCan(
            byte canChannelId, GatewaySocketViaUdp mySocketViaUdp)
            : base(mySocketViaUdp.RemoteIpPort, 19)
        {
            _canChannelId = canChannelId;
            _mySocketViaUdp = mySocketViaUdp;
            _mySocketViaUdp.AddOvserver(ReceiveMsg);
            RegisterSendAction(SendMultipleCans);
        }

        public void ReceiveMsg(
            EndPoint ipEndPointFrom, byte[] bytes)
        {
            try
            {
                if (!bytes[7].Equals(0x69))
                    return;

                if (bytes.Length < 13)
                    return;

                var regCounts = bytes[10] * 256 + bytes[11];
                if (12 + 1 + regCounts * 2 + 7 != bytes.Length)
                    return;

                var content = new byte[regCounts * 2];
                Array.Copy(bytes, 13, content, 0, content.Length);

                var recv = content.ToList();
                var lstRecvPackages = new List<CanDataPackage>();

                while (true)
                {
                    if (recv.Count <= 0)
                        break;

                    var canLen = recv[0] * 256 + recv[1];
                    var canId = BitConverter.ToUInt32(
                        new[] { recv[5], recv[4], recv[3], recv[2] }, 0);
                    var canType = recv[7];
                    var canFormat = recv[9];
                    var data = new List<byte>();
                    for (var i = 10; i < 10 + canLen * 2; i = i + 2)
                        data.Add(recv[i + 1]);

                    lstRecvPackages.Add(new CanDataPackage(canId, CanProtocol.Can, canType == 0x00 ? CanType.Standard : CanType.Extended,
                            canFormat == 0x00 ? CanFormat.Data : CanFormat.Remote, data.ToArray()));

                    recv.RemoveRange(0, 10 + canLen * 2);
                }

                ReceiveCanDatas(lstRecvPackages.ToArray());
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void SendMultipleCans(CanDataPackage[] dataPackages)
        {
            if (dataPackages == null || !dataPackages.Any())
                return;

            var sendList = new List<List<CanDataPackage>>();
            var tempPackages = new List<CanDataPackage>();

            foreach (var p in dataPackages)
            {
                if (tempPackages.Count == 0)
                {
                    tempPackages.Add(p);
                }
                else
                {
                    if (tempPackages[tempPackages.Count - 1].CanType != p.CanType)
                    {
                        var tempArray = new CanDataPackage[tempPackages.Count];
                        Array.Copy(tempPackages.ToArray(), tempArray, tempArray.Length);
                        sendList.Add(tempArray.ToList());
                        tempPackages.Clear();
                        tempPackages.Add(p);
                    }
                    else if (p.CanType == CanType.Extended)
                    {
                        var tempArray = new CanDataPackage[tempPackages.Count];
                        Array.Copy(tempPackages.ToArray(), tempArray, tempArray.Length);
                        sendList.Add(tempArray.ToList());
                        tempPackages.Clear();
                        tempPackages.Add(p);
                    }
                    else
                    {
                        tempPackages.Add(p);
                    }
                }
            }

            sendList.Add(tempPackages);

            foreach (var s in sendList)
            {
                if (s[0].CanType == CanType.Standard)
                {
                    var sendBytes = new List<byte>();
                    sendBytes.AddRange(new byte[] { 0x01, 0x00 });
                    sendBytes.AddRange(new byte[] { 0x00, 0x01 });

                    var datas = new List<byte>();

                    foreach (var p in s)
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

                    sendBytes.AddRange(ModbusOperater.GetModbusWriteValues(
                        CodeId, "0", (ushort)(datas.Count / 2), datas.ToArray(), _canChannelId));
                    _mySocketViaUdp.SendMsg(sendBytes);
                }
                else
                {
                    foreach (var p in s)
                    {
                        var sendBytes = new List<byte>();
                        sendBytes.AddRange(new byte[] { 0x01, 0x00 });
                        sendBytes.AddRange(new byte[] { 0x00, 0x01 });

                        var datas = new List<byte>();

                        var canIdBytes = BitConverter.GetBytes(p.CanId);
                        Array.Reverse(canIdBytes);

                        var canLen = BitConverter.GetBytes((byte)p.CanDataLen);
                        Array.Reverse(canLen);

                        var sendCanBytes = new List<byte>();
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

                        sendBytes.AddRange(ModbusOperater.GetModbusWriteValues(0x65, "0", (ushort)(datas.Count / 2),
                            datas.ToArray(), _canChannelId));
                        _mySocketViaUdp.SendMsg(sendBytes);
                    }
                }
            }
        }
    }

    /// <summary>
    /// LIN网关
    /// </summary>
    public class GatewayLin :
        LinBus, IGatewaySocketViaUdpSubscriber
    {
        /// <summary>
        /// LIN通道ID
        /// </summary>
        private readonly byte _linChannelId;

        /// <summary>
        /// CAN转发网络
        /// </summary>
        private readonly GatewaySocketViaUdp _mySocketViaUdp;

        /// <summary>
        /// LIN网关功能码
        /// </summary>
        private const byte CodeId = 0x66;

        public GatewayLin(byte linChannelId, GatewaySocketViaUdp mySocketViaUdp)
            : base(mySocketViaUdp.RemoteIpPort)
        {
            _linChannelId = linChannelId;
            _mySocketViaUdp = mySocketViaUdp;
            _mySocketViaUdp.AddOvserver(ReceiveMsg);

            RegisterSendMasterLinFunc(SendMasterLinMessage);
            RegisterSendSlaveLinACtion(SendSlaveLinMessage);
        }

        public void ReceiveMsg(EndPoint ipEndPointFrom, byte[] bytes)
        {
            try
            {
                if (!bytes[7].Equals(CodeId))
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
                    var function = temp[7];
                    var contentBytes = new byte[registerCount * 2];
                    Array.Copy(bytes.ToArray(), 13, contentBytes, 0, registerCount * 2);

                    if (function == CodeId)
                    {
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

                            var bys = new byte[tempB.Count];
                            Array.Copy(tempB.ToArray(), bys, bys.Length);

                            var linDataPackage = new LinDataPackage(linId, bys);
                            RecviveLinDatas(new[] { linDataPackage });

                            if (_currenSendtMasterLinId == linDataPackage.LinId &&
                                _currenSendtMasterLinId != 0xFF)
                                MasterLinEventWaitHandle.Set();
                        }
                        catch (Exception)
                        {
                            // ignored
                        }
                    }
                    #endregion
                } while (buffLst.Count != 0);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private static byte _currenSendtMasterLinId;
        private static readonly EventWaitHandle MasterLinEventWaitHandle = new AutoResetEvent(false);

        private bool SendMasterLinMessage(LinDataPackage linDataPackage, bool isWaitSlaveLin)
        {
            if (linDataPackage == null)
                return false;

            MasterLinEventWaitHandle.Reset();
            _currenSendtMasterLinId = ConvertLinId(linDataPackage.LinId);
            SendLinMsg(linDataPackage.LinId, linDataPackage.LinData);

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
            SendLinMsg(slaveLinId, new byte[] { });
        }

        private void SendLinMsg(byte linId, IEnumerable<byte> value)
        {
            var msgValues = value as byte[] ?? value.ToArray();
            if (msgValues.Length > 8)
                return;

            var sendBytes = new List<byte>();
            sendBytes.AddRange(new byte[] { 0x01, 0x00 });
            sendBytes.AddRange(new byte[] { 0x00, 0x01 });

            var linIdBytes = new byte[] { 0x00, linId };
            var sendLinBytes = linIdBytes.ToList();

            var linRegistersValues = new List<byte>();
            foreach (var t in msgValues)
            {
                linRegistersValues.Add(0x00);
                linRegistersValues.Add(t);
            }
            sendLinBytes.AddRange(linRegistersValues);

            sendBytes.AddRange(ModbusOperater.GetModbusWriteValues(CodeId, "0", (ushort)(sendLinBytes.Count / 2),
                sendLinBytes.ToArray(), _linChannelId));
            _mySocketViaUdp.SendMsg(sendBytes);
        }
    }

    public class GatewaySocketViaUdp
    {
        public delegate void NotifyEventHandler(EndPoint ipEndPointFrom, byte[] bytes); // 需改成推送MSG TYPE

        private NotifyEventHandler _notifyEvent;

        private readonly MyUdpClient _myUdpClient;
        public readonly string RemoteIpPort;

        public GatewaySocketViaUdp(string ipPort)
        {
            RemoteIpPort = ipPort;

            var split = RemoteIpPort.Split(':');
            var ipAddressStr = split[0];
            var port = Convert.ToInt32(split[1]);

            if (ipPort.Equals("192.168.1.150:8088"))
            {
                _myUdpClient = new MyUdpClient("192.168.1.150", 8089);
            }
            else if (ipPort.Equals("127.0.0.1:8088"))
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
        }

        ~GatewaySocketViaUdp()
        {
            if (_myUdpClient != null)
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

        private void Update(EndPoint ipEndPointFrom, byte[] bytes)
        {
            if (_notifyEvent != null)
                _notifyEvent(ipEndPointFrom, bytes);
        }

        /// <summary>
        /// 接收消息
        /// 拆包和校验
        /// </summary>
        /// <param name="ipEndPointFrom">发送消息的节点</param>
        /// <param name="bytes">消息内容</param>
        private void _myUdpClient_PushMsgEvent(EndPoint ipEndPointFrom, byte[] bytes)
        {
            if (bytes.Length < 7)
                return;

            var buffLst = bytes.ToList();

            do
            {
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
                var expectedCrc16 = ValueHelper.Crc16(tempContent).ToArray();
                var actualCrc16 = new[] { temp[tempLen - 2], temp[tempLen - 1] };

                if (expectedCrc16[0] != actualCrc16[0] || expectedCrc16[1] != actualCrc16[1])
                    continue;

                Update(ipEndPointFrom, temp);
            } while (buffLst.Count != 0);
        }

        private byte _sendTrackId;
        private DateTime _powerOnTime = DateTime.Now;

        /// <summary>
        /// 发送数据
        /// 在此处打包数据，添加索引、时间戳和CRC16-MODBUS校验
        /// </summary>
        /// <param name="bytes">需要打包发送的数据</param>
        public void SendMsg(IEnumerable<byte> bytes)
        {
            var sendBytes = bytes.ToList();
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
                sendBytes.ToArray(), 5);
        }
    }

    public interface IGatewaySocketViaUdpSubscriber
    {
        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="ipEndPointFrom">消息发送过来的远程节点</param>
        /// <param name="bytes">消息内容</param>
        void ReceiveMsg(EndPoint ipEndPointFrom, byte[] bytes);
    }
}
