using CommonUtility;
using CommonUtility.BusLoader;
using Controller.HardwareController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace Controller
{
    [Description("CAN-Device")]
    public sealed class SyRenesasMcuControllerMaster : ControllerBase, ICcdIoController
    {
        public bool SetOutputs()
        {
            return false;
        }

        public void GetInputs()
        {
            MasterReadDIs();
        }

        public void ResetOutPuts()
        {
            Do1 = false;
            Do2 = false;
            Do3 = false;
            Do4 = false;
            MasetSetDOs();
        }

        #region Fields
        public CanBus GatewayCan1;
        public CanBus GatewayCan2;
        public CanBus GatewayCan3;
        public CanBus GatewayCan4;

        public LinBus GatewayLin1;
        public LinBus GatewayLin2;

        public MySerialPort UartCan;

        [Description("R,输入1")]
        public string Di1;
        [Description("R,输入2")]
        public string Di2;
        [Description("R,输入3")]
        public string Di3;
        [Description("R,输入4")]
        public string Di4;
        [Description("R,输入5")]
        public string Di5;
        [Description("R,输入6")]
        public string Di6;

        [Description("R/W,输出1")]
        public bool Do1;
        [Description("R/W,输出2")]
        public bool Do2;
        [Description("R/W,输出3")]
        public bool Do3;
        [Description("R/W,输出4")]
        public bool Do4;

        [Description("R,频率1")]
        public double Freq1;
        [Description("R,占空比1")]
        public double Duty1;

        [Description("R,频率2")]
        public double Freq2;
        [Description("R,占空比2")]
        public double Duty2;


        [Description("R,频率3")]
        public double Freq3;
        [Description("R,占空比3")]
        public double Duty3;

        [Description("R,频率4")]
        public double Freq4;
        [Description("R,占空比4")]
        public double Duty4;


        [Description("R,频率5")]
        public double Freq5;
        [Description("R,占空比5")]
        public double Duty5;


        [Description("R,频率6")]
        public double Freq6;
        [Description("R,占空比6")]
        public double Duty6;

        [Description("R,HSD输出电流1")]
        public float HSDCurrent1;

        [Description("R,HSD输出电流2")]
        public float HSDCurrent2;

        [Description("R,HSD输出电流3")]
        public float HSDCurrent3;

        [Description("R,HSD输出电流4")]
        public float HSDCurrent4;

        #endregion

        public bool IsConnected { get; set; }
        private MyUdpClient MyUdpClient { get; set; }
        public string RemoteIpPort { get; set; }
        private int RemotePort { get; set; }
        private IPAddress RemoteIpAddress { get; set; }

        private readonly Dictionary<uint, SyRenesasMcuControllerSlaveWith8RLs> _controllerSlaveWith8RLsMap =
            new Dictionary<uint, SyRenesasMcuControllerSlaveWith8RLs>();
        private readonly Dictionary<uint, SyRenesasMcuControllerSlaveWith12ADs> _controllerSlaveWith12ADsMap =
            new Dictionary<uint, SyRenesasMcuControllerSlaveWith12ADs>();
        private readonly Dictionary<uint, SyRenesasMcuControllerSlaveWith19PhotoSensor> _controllerSlaveWith19PhotoSensorsMap =
            new Dictionary<uint, SyRenesasMcuControllerSlaveWith19PhotoSensor>();
        private static readonly Dictionary<string, SyRenesasMcuControllerMaster> RenesasMcuControllerMasters =
            new Dictionary<string, SyRenesasMcuControllerMaster>();

        public SyRenesasMcuControllerMaster(string name) : base(name)
        {
            //var str1 = "55aa55aa001701610000000007bb00";
            //var bs1 = new List<byte>();
            //for (var i = 0; i < str1.Length; i = i + 2)
            //    bs1.Add(Convert.ToByte(str1.Substring(i, 2), 16));
            //_myUdpClient_PushMsgEvent(null, bs1.ToArray());

            //var str2 = "0008037f367800000000fe364602";
            //var bs2 = new List<byte>();
            //for (var i = 0; i < str2.Length; i = i + 2)
            //    bs2.Add(Convert.ToByte(str2.Substring(i, 2), 16));
            //_myUdpClient_PushMsgEvent(null, bs2.ToArray());
        }

        ~SyRenesasMcuControllerMaster()
        {
            Dispose();
        }

        public enum MasterFunc : byte
        {
            Free,

            SendCanMsg = 0x60,

            RecvCanMsg = 0x61,

            SendLinMsg = 0x62,

            RecvLinMsg = 0x63,

            /// <summary>
            /// 设置所有从站继电器
            /// </summary>
            SetSlavesRLs,

            /// <summary>
            /// 读所有从站AD值
            /// </summary>
            ReadSlavesADs,

            /// <summary>
            /// 读所有从站光敏传感器采样值
            /// </summary>
            ReadSlavesSensors,

            /// <summary>
            /// 单个从站设置继电器
            /// </summary>
            SlaveSetRLs,

            /// <summary>
            /// 单个从站读AD值
            /// </summary>
            SlaveReadADs,

            /// <summary>
            /// 单个从站读光敏传感器采样值
            /// </summary>
            SlaveReadSensorValues,

            MasterReadDIs,

            MasterReadCurs,

            MasterSetDOs,

            SetSlaveId,

            SetSlaveCanFunc,

            SendUartCanMsg = 0x66,

            RecvUartCanMsg = 0x67,

            SpecialCmd = 0xFF,

            WaitLin1Err = 0xF1,

            WaitLin2Err = 0xF1
        }

        #region UDP连接、接收、发送处理

        public void InitRemoteIpAddress(string ipPort)
        {
            RemoteIpPort = ipPort;

            try
            {
                var sp = ipPort.Split(':');
                var ipAddrStr = sp[0];
                var port = int.Parse(sp[1]);

                if (!RenesasMcuControllerMasters.ContainsKey(ipAddrStr))
                {
                    RemotePort = port;
                    RemoteIpAddress = IPAddress.Parse(ipAddrStr);

                    MyUdpClient = ipAddrStr.Equals("127.0.0.1")
                        ? new MyUdpClient("127.0.0.1", port + 1)
                        : new MyUdpClient("192.168.1.50", 5000 + int.Parse(ipAddrStr.Split('.')[3]));

                    MyUdpClient.PushMsgEvent += _myUdpClient_PushMsgEvent;
                    MyUdpClient.AddRemoteClient(ipAddrStr, port);
                    MyUdpClient.BeginReceive();
                    IsConnected = true;

                    GatewayCan1 = new RenesasMcuControllerGatewayCan(0, this);
                    GatewayCan2 = new RenesasMcuControllerGatewayCan(1, this);
                    GatewayCan3 = new RenesasMcuControllerGatewayCan(2, this);
                    GatewayCan4 = new RenesasMcuControllerGatewayCan(3, this);

                    GatewayLin1 = new RenesasMcuControllerGatewayLin(0, this);
                    GatewayLin2 = new RenesasMcuControllerGatewayLin(1, this);

                    UartCan = new RenesasMcuControllerGatewayUartCan(this);

                    RenesasMcuControllerMasters.Add(ipAddrStr, this);
                }
            }
            catch (Exception)
            {
                IsConnected = false;
            }
        }

        private readonly List<byte> _lastRestBytes = new List<byte>();
        private readonly object _lockRecv = new object();
        private readonly object _lockSend = new object();
        private MasterFunc _currentFunc = MasterFunc.Free;
        private readonly EventWaitHandle _waitHandle = new AutoResetEvent(false);

        private void _myUdpClient_PushMsgEvent(EndPoint ipEndPoint, byte[] dataBytes)
        {
            lock (_lockRecv)
            {
                var debugStr = ValueHelper.GetHextStr(dataBytes);
                //Console.WriteLine("RECV: " + debugStr);

                //if (debugStr.Contains("00 00 07 58"))
                //{
                //    Console.WriteLine(debugStr);
                //}

                debugStr = debugStr.Replace("55 AA 55 AA 00 08 01 60 00 00 00 5B 57", "").Replace(" ", "");

                var bb = new List<byte>();
                bb.AddRange(_lastRestBytes);
                _lastRestBytes.Clear();

                for (var i = 0; i < debugStr.Length; i = i + 2)
                    bb.Add(Convert.ToByte(debugStr.Substring(i, 2), 16));

                var cc = bb.ToArray();

                //if (debugStr.Contains("00000758000008300000"))
                //{
                //    Console.WriteLine(ValueHelper.GetHextStr(cc));
                //}

                var startIndex = 0;

                while (true)
                {
                    if (cc.Length > startIndex + 6 &&
                        cc[startIndex + 0] == 0x55 && cc[startIndex + 2] == 0x55 &&
                        cc[startIndex + 1] == 0xAA && cc[startIndex + 3] == 0xAA)
                    {
                        var lenOfData = cc[startIndex + 4] * 256 + cc[startIndex + 5];

                        if (cc.Length >= startIndex + lenOfData + 6)
                        {
                            var bytes = new byte[lenOfData + 6];
                            Array.Copy(cc, startIndex, bytes, 0, bytes.Length);
                            startIndex += bytes.Length;

                            var crc = new byte[] { bytes[bytes.Length - 2], bytes[bytes.Length - 1] };
                            var toCrcDatas = new byte[bytes.Length - 2];
                            Array.Copy(bytes.ToArray(), 0, toCrcDatas, 0, toCrcDatas.Length);
                            var toCrc = ValueHelper.Crc16(toCrcDatas).ToArray();

                            //if (crc[0] == toCrc[0] && crc[1] == toCrc[1])
                            {
                                var funcCode = bytes[7];

                                //if (funcCode == 0x61) // CAN转网口
                                //{
                                //    // 每帧CAN数据占17个字节
                                //    if (bytes.Length > (4 + 2 + 1 + 1 + 1 + 1 + 2))
                                //    {
                                //        var lenOfCanDatas = bytes.Length - (4 + 2 + 1 + 1 + 1 + 1 + 2);
                                //        if (lenOfCanDatas > 0 && lenOfCanDatas % 17 == 0)
                                //        {
                                //            var canPackagesCount = lenOfCanDatas / 17;
                                //            var chIndex = bytes[9];
                                //            var listCanPackages = new List<CanBus.CanDataPackage>();

                                //            var baseIndex = 10;
                                //            for (var i = 0; i < canPackagesCount; i++)
                                //            {
                                //                var canId = BitConverter.ToUInt32(
                                //                    new[] { bytes[baseIndex + 3], bytes[baseIndex + 2], bytes[baseIndex + 1], bytes[baseIndex + 0] }, 0);  //ID 00 00 00 00

                                //                var isStandard =
                                //                    Convert.ToString(bytes[baseIndex + 4], 2).PadLeft(8, '0')[7]
                                //                        .ToString() == "0";

                                //                var len = bytes[baseIndex + 6];

                                //                var canData = new byte[len];
                                //                for (var j = 0; j < len; j++)
                                //                    canData[j] = bytes[baseIndex + 7 + j];

                                //                listCanPackages.Add(new CanBus.CanDataPackage(canId, CanBus.CanProtocol.Can,
                                //                    isStandard ? CanBus.CanType.Standard : CanBus.CanType.Extended, CanBus.CanFormat.Data, canData));

                                //                baseIndex = baseIndex + 17;
                                //            }

                                //            if (chIndex == 0)
                                //                GatewayCan1.ReceiveCanDatas(listCanPackages.ToArray());
                                //            else if (chIndex == 1)
                                //                GatewayCan2.ReceiveCanDatas(listCanPackages.ToArray());
                                //            else if (chIndex == 2)
                                //                GatewayCan3.ReceiveCanDatas(listCanPackages.ToArray());
                                //            else if (chIndex == 3)
                                //                GatewayCan4.ReceiveCanDatas(listCanPackages.ToArray());
                                //        }
                                //    }
                                //}
                                if (funcCode == 0x61) // CAN转网口
                                {
                                    //Console.WriteLine("CAN网口数据" + ValueHelper.GetHextStr(bytes));
                                    // 每帧CAN数据占17个字节
                                    if (bytes.Length > (4 + 2 + 1 + 1 + 1 + 1 + 2))
                                    {

                                        var lenOfCanDatas = bytes.Length - (4 + 2 + 1 + 1 + 1 + 1 + 2);
                                        byte[] newCanBytes = new byte[lenOfCanDatas];
                                        Array.Copy(bytes, 10, newCanBytes, 0, lenOfCanDatas);
                                        //Console.WriteLine("截取" + ValueHelper.GetHextStr(newCanBytes));
                                        var chIndex = bytes[9];
                                        var listCanPackages = new List<CanBus.CanDataPackage>();
                                        if (lenOfCanDatas > 0)
                                        {
                                            var baseIndex = 0;
                                            while (baseIndex + 4 + 2 + 1 + 1 + 1 + 1 + 2 < bytes.Length)
                                            {
                                                var canId = BitConverter.ToUInt32(
                                                       new[] { newCanBytes[3], newCanBytes[2], newCanBytes[1], newCanBytes[0] }, 0);  //ID 00 00 00 00
                                                var isStandard =
                                                          Convert.ToString(newCanBytes[4], 2).PadLeft(8, '0')[7]
                                                              .ToString() == "0";
                                                var len = newCanBytes[6];
                                                var canData = new byte[len];
                                                for (var j = 0; j < len; j++)
                                                    canData[j] = newCanBytes[7 + j];

                                                listCanPackages.Add(new CanBus.CanDataPackage(canId, CanBus.CanProtocol.Can,
                                                    isStandard ? CanBus.CanType.Standard : CanBus.CanType.Extended, CanBus.CanFormat.Data, canData));
                                                baseIndex += 9 + len;
                                            }
                                        }
                                        if (chIndex == 0)
                                            GatewayCan1.ReceiveCanDatas(listCanPackages.ToArray());
                                        else if (chIndex == 1)
                                            GatewayCan2.ReceiveCanDatas(listCanPackages.ToArray());
                                        else if (chIndex == 2)
                                            GatewayCan3.ReceiveCanDatas(listCanPackages.ToArray());
                                        else if (chIndex == 3)
                                            GatewayCan4.ReceiveCanDatas(listCanPackages.ToArray());

                                        //if (lenOfCanDatas > 0 && lenOfCanDatas % 17 == 0)
                                        //{
                                        //    var canPackagesCount = lenOfCanDatas / 17;
                                        //    var chIndex = bytes[9];
                                        //    var listCanPackages = new List<CanBus.CanDataPackage>();

                                        //    var baseIndex = 10;
                                        //    for (var i = 0; i < canPackagesCount; i++)
                                        //    {
                                        //        var canId = BitConverter.ToUInt32(
                                        //            new[] { bytes[baseIndex + 3], bytes[baseIndex + 2], bytes[baseIndex + 1], bytes[baseIndex + 0] }, 0);  //ID 00 00 00 00

                                        //        var isStandard =
                                        //            Convert.ToString(bytes[baseIndex + 4], 2).PadLeft(8, '0')[7]
                                        //                .ToString() == "0";

                                        //        var len = bytes[baseIndex + 6];

                                        //        var canData = new byte[len];
                                        //        for (var j = 0; j < len; j++)
                                        //            canData[j] = bytes[baseIndex + 7 + j];

                                        //        listCanPackages.Add(new CanBus.CanDataPackage(canId, CanBus.CanProtocol.Can,
                                        //            isStandard ? CanBus.CanType.Standard : CanBus.CanType.Extended, CanBus.CanFormat.Data, canData));

                                        //        baseIndex = baseIndex + 17;
                                        //    }

                                        //    if (chIndex == 0)
                                        //        GatewayCan1.ReceiveCanDatas(listCanPackages.ToArray());
                                        //    else if (chIndex == 1)
                                        //        GatewayCan2.ReceiveCanDatas(listCanPackages.ToArray());
                                        //    else if (chIndex == 2)
                                        //        GatewayCan3.ReceiveCanDatas(listCanPackages.ToArray());
                                        //    else if (chIndex == 3)
                                        //        GatewayCan4.ReceiveCanDatas(listCanPackages.ToArray());
                                        //}
                                    }
                                }
                                else if (funcCode == 0x45) // 主站读从站寄存器 从站回复
                                {
                                    if (_currentFunc == MasterFunc.ReadSlavesADs || _currentFunc == MasterFunc.SlaveReadADs)
                                    {
                                        var adSlaveCount = 1;

                                        if (_currentFunc == MasterFunc.ReadSlavesADs)
                                            adSlaveCount = 1;//_controllerSlaveWith12ADsMap.Count;

                                        const int regCountPer = 13 * 2;

                                        if (bytes.Length >= (regCountPer + 2 + 2 + 2) * adSlaveCount + 2 + 1 + 1 + 1 + 2 + 2 + 2)
                                        {
                                            for (var i = 0; i < regCountPer; i++)
                                            {
                                                var firstIndex = 9;
                                                var thisSlaveAddr = bytes[firstIndex] * 256 + bytes[firstIndex + 1];
                                                var thisRegAddr = bytes[firstIndex + 2] * 256 + bytes[firstIndex + 3];
                                                var thisRegCount = bytes[firstIndex + 4] * 256 + bytes[firstIndex + 5];

                                                if (thisRegAddr == 0x401A &&
                                                    thisRegCount == 13 &&
                                                    _controllerSlaveWith12ADsMap.ContainsKey((uint)thisSlaveAddr))
                                                {
                                                    _controllerSlaveWith12ADsMap[(uint)thisSlaveAddr].Current1 =
                                                        bytes[firstIndex + 6] * 256 + bytes[firstIndex + 7];
                                                    _controllerSlaveWith12ADsMap[(uint)thisSlaveAddr].Current2 =
                                                        bytes[firstIndex + 8] * 256 + bytes[firstIndex + 9];
                                                    _controllerSlaveWith12ADsMap[(uint)thisSlaveAddr].Current3 =
                                                        bytes[firstIndex + 10] * 256 + bytes[firstIndex + 11];
                                                    _controllerSlaveWith12ADsMap[(uint)thisSlaveAddr].Current4 =
                                                        bytes[firstIndex + 12] * 256 + bytes[firstIndex + 13];
                                                    _controllerSlaveWith12ADsMap[(uint)thisSlaveAddr].Current5 =
                                                        bytes[firstIndex + 14] * 256 + bytes[firstIndex + 15];
                                                    _controllerSlaveWith12ADsMap[(uint)thisSlaveAddr].Current6 =
                                                        bytes[firstIndex + 16] * 256 + bytes[firstIndex + 17];

                                                    _controllerSlaveWith12ADsMap[(uint)thisSlaveAddr].Resistance1 =
                                                        bytes[firstIndex + 18] * 256 + bytes[firstIndex + 19];

                                                    _controllerSlaveWith12ADsMap[(uint)thisSlaveAddr].Voltage2 =
                                                        Math.Round((bytes[firstIndex + 20] * 256 + bytes[firstIndex + 21]) * 0.001f, 2, MidpointRounding.AwayFromZero);
                                                    _controllerSlaveWith12ADsMap[(uint)thisSlaveAddr].Voltage3 =
                                                        Math.Round((bytes[firstIndex + 22] * 256 + bytes[firstIndex + 23]) * 0.001f, 2, MidpointRounding.AwayFromZero);
                                                    _controllerSlaveWith12ADsMap[(uint)thisSlaveAddr].Voltage4 =
                                                        Math.Round((bytes[firstIndex + 24] * 256 + bytes[firstIndex + 25]) * 0.001f, 2, MidpointRounding.AwayFromZero);
                                                    _controllerSlaveWith12ADsMap[(uint)thisSlaveAddr].Voltage5 =
                                                        Math.Round((bytes[firstIndex + 26] * 256 + bytes[firstIndex + 27]) * 0.001f, 2, MidpointRounding.AwayFromZero);
                                                    _controllerSlaveWith12ADsMap[(uint)thisSlaveAddr].Voltage6 =
                                                        Math.Round((bytes[firstIndex + 28] * 256 + bytes[firstIndex + 29]) * 0.001f, 2, MidpointRounding.AwayFromZero);
                                                    _controllerSlaveWith12ADsMap[(uint)thisSlaveAddr].Voltage7 =
                                                        Math.Round((bytes[firstIndex + 30] * 256 + bytes[firstIndex + 31]) * 0.001f, 2, MidpointRounding.AwayFromZero);
                                                }

                                                firstIndex += regCountPer + 2 + 2 + 2;
                                            }

                                            if (_currentFunc == MasterFunc.ReadSlavesADs)
                                            {
                                                _readAdCount++;
                                                if (_controllerSlaveWith12ADsMap.Count == _readAdCount)
                                                    _waitHandle.Set();
                                            }
                                            else if (_currentFunc == MasterFunc.SlaveReadADs)
                                            {
                                                _waitHandle.Set();
                                            }
                                        }
                                    }
                                    else if (_currentFunc == MasterFunc.SlaveReadSensorValues || _currentFunc == MasterFunc.ReadSlavesSensors)
                                    {
                                        var adSlaveCount = 1;

                                        if (_currentFunc == MasterFunc.SlaveReadSensorValues)
                                            adSlaveCount = 1;//_controllerSlaveWith12ADsMap.Count;

                                        const int regCountPer = 19 * 2;

                                        if (bytes.Length >= (regCountPer + 2 + 2 + 2) * adSlaveCount + 2 + 1 + 1 + 1 + 2 + 2 + 2)
                                        {
                                            for (var i = 0; i < regCountPer; i++)
                                            {
                                                var firstIndex = 9;
                                                var thisSlaveAddr = bytes[firstIndex] * 256 + bytes[firstIndex + 1];
                                                var thisRegAddr = bytes[firstIndex + 2] * 256 + bytes[firstIndex + 3];
                                                var thisRegCount = bytes[firstIndex + 4] * 256 + bytes[firstIndex + 5];

                                                if (thisRegAddr == 0x4000 &&
                                                    thisRegCount == 19 &&
                                                    _controllerSlaveWith19PhotoSensorsMap.ContainsKey((uint)thisSlaveAddr))
                                                {
                                                    _controllerSlaveWith19PhotoSensorsMap[(uint)thisSlaveAddr].PhotoSensor1 =
                                                        bytes[firstIndex + 6] * 256 + bytes[firstIndex + 7];
                                                    _controllerSlaveWith19PhotoSensorsMap[(uint)thisSlaveAddr].PhotoSensor2 =
                                                        bytes[firstIndex + 8] * 256 + bytes[firstIndex + 9];
                                                    _controllerSlaveWith19PhotoSensorsMap[(uint)thisSlaveAddr].PhotoSensor3 =
                                                        bytes[firstIndex + 10] * 256 + bytes[firstIndex + 11];
                                                    _controllerSlaveWith19PhotoSensorsMap[(uint)thisSlaveAddr].PhotoSensor4 =
                                                        bytes[firstIndex + 12] * 256 + bytes[firstIndex + 13];
                                                    _controllerSlaveWith19PhotoSensorsMap[(uint)thisSlaveAddr].PhotoSensor5 =
                                                        bytes[firstIndex + 14] * 256 + bytes[firstIndex + 15];
                                                    _controllerSlaveWith19PhotoSensorsMap[(uint)thisSlaveAddr].PhotoSensor6 =
                                                        bytes[firstIndex + 16] * 256 + bytes[firstIndex + 17];
                                                    _controllerSlaveWith19PhotoSensorsMap[(uint)thisSlaveAddr].PhotoSensor7 =
                                                        bytes[firstIndex + 18] * 256 + bytes[firstIndex + 19];
                                                    _controllerSlaveWith19PhotoSensorsMap[(uint)thisSlaveAddr].PhotoSensor8 =
                                                        bytes[firstIndex + 20] * 256 + bytes[firstIndex + 21];
                                                    _controllerSlaveWith19PhotoSensorsMap[(uint)thisSlaveAddr].PhotoSensor9 =
                                                        bytes[firstIndex + 22] * 256 + bytes[firstIndex + 23];
                                                    _controllerSlaveWith19PhotoSensorsMap[(uint)thisSlaveAddr].PhotoSensor10 =
                                                        bytes[firstIndex + 24] * 256 + bytes[firstIndex + 25];
                                                    _controllerSlaveWith19PhotoSensorsMap[(uint)thisSlaveAddr].PhotoSensor11 =
                                                        bytes[firstIndex + 26] * 256 + bytes[firstIndex + 27];
                                                    _controllerSlaveWith19PhotoSensorsMap[(uint)thisSlaveAddr].PhotoSensor12 =
                                                        bytes[firstIndex + 28] * 256 + bytes[firstIndex + 29];
                                                    _controllerSlaveWith19PhotoSensorsMap[(uint)thisSlaveAddr].PhotoSensor13 =
                                                        bytes[firstIndex + 30] * 256 + bytes[firstIndex + 31];
                                                    _controllerSlaveWith19PhotoSensorsMap[(uint)thisSlaveAddr].PhotoSensor14 =
                                                        bytes[firstIndex + 32] * 256 + bytes[firstIndex + 33];
                                                    _controllerSlaveWith19PhotoSensorsMap[(uint)thisSlaveAddr].PhotoSensor15 =
                                                        bytes[firstIndex + 34] * 256 + bytes[firstIndex + 35];
                                                    _controllerSlaveWith19PhotoSensorsMap[(uint)thisSlaveAddr].PhotoSensor16 =
                                                        bytes[firstIndex + 36] * 256 + bytes[firstIndex + 37];
                                                    _controllerSlaveWith19PhotoSensorsMap[(uint)thisSlaveAddr].PhotoSensor17 =
                                                        bytes[firstIndex + 38] * 256 + bytes[firstIndex + 39];
                                                    _controllerSlaveWith19PhotoSensorsMap[(uint)thisSlaveAddr].PhotoSensor18 =
                                                        bytes[firstIndex + 40] * 256 + bytes[firstIndex + 41];
                                                    _controllerSlaveWith19PhotoSensorsMap[(uint)thisSlaveAddr].PhotoSensor19 =
                                                        bytes[firstIndex + 42] * 256 + bytes[firstIndex + 43];
                                                }

                                                firstIndex += regCountPer + 2 + 2 + 2;
                                            }

                                            if (_currentFunc == MasterFunc.ReadSlavesSensors)
                                            {
                                                _readAdCount++;
                                                if (_controllerSlaveWith19PhotoSensorsMap.Count == _readAdCount)
                                                    _waitHandle.Set();
                                            }
                                            else if (_currentFunc == MasterFunc.SlaveReadSensorValues)
                                            {
                                                _waitHandle.Set();
                                            }
                                        }
                                    }
                                }
                                else if (funcCode == 0x47) // 主站写从站寄存器 从站回复
                                {
                                    if (_currentFunc == MasterFunc.SlaveSetRLs ||
                                        _currentFunc == MasterFunc.SlaveSetRLs ||
                                        _currentFunc == MasterFunc.SetSlaveId)
                                        _waitHandle.Set();
                                }
                                else if (funcCode == 0x63)
                                {
                                    if ((bytes.Length - 12) % 10 == 0)
                                    {
                                        var recvLinPackages = new List<LinBus.LinDataPackage>();

                                        for (var j = 10; j < bytes.Length - 2; j = j + 10)
                                        {
                                            var linDataLen = bytes[j + 1];
                                            var linId = LinBus.ConvertPid(bytes[j + 0]);

                                            byte[] linData;
                                            if (linDataLen == 0)
                                            {
                                                linData = new byte[0];
                                            }
                                            else
                                            {
                                                linData = new byte[linDataLen];
                                                Array.Copy(bytes, j + 2, linData, 0, linDataLen);
                                            }

                                            var linPackage = new LinBus.LinDataPackage(linId, linData);
                                            recvLinPackages.Add(linPackage);
                                        }

                                        var linCh = bytes[9];

                                        if (linCh == 0 && GatewayLin1 != null)
                                        {
                                            ((RenesasMcuControllerGatewayLin)GatewayLin1).ReceivedMsg(recvLinPackages.ToArray());

                                            //GatewayLin1.RecviveLinDatas(recvLinPackages.ToArray());
                                            //if (_currentFunc == MasterFunc.SendLinMsg || _currentFunc == MasterFunc.RecvLinMsg)
                                            //    _waitHandle.Set();
                                        }
                                        else if (linCh == 1 && GatewayLin2 != null)
                                        {
                                            ((RenesasMcuControllerGatewayLin)GatewayLin2).ReceivedMsg(recvLinPackages.ToArray());
                                            //((RenesasMcuControllerGatewayLin)GatewayLin2).LinMsgBuffer = linPackage;
                                            //GatewayLin2.RecviveLinDatas(recvLinPackages.ToArray());
                                            //if (_currentFunc == MasterFunc.SendLinMsg || _currentFunc == MasterFunc.RecvLinMsg)
                                            //    _waitHandle.Set();
                                        }
                                    }
                                }
                                else if (funcCode == 0x64) // 主站读寄存器
                                {
                                    if (_currentFunc == MasterFunc.MasterReadDIs)
                                    {
                                        const int regCountPer = 2;

                                        if (bytes.Length >= 13 && bytes.Length >= (regCountPer + 2 + 2) * (bytes[11] * 256 + bytes[12]) + 2 + 1 + 1 + 1 + 2 + 2 + 2)
                                        {
                                            var firstIndex = 9;

                                            for (var i = 0; i < regCountPer; i++)
                                            {
                                                var thisRegAddr = bytes[firstIndex + 0] * 256 + bytes[firstIndex + 1];
                                                var thisRegCount = bytes[firstIndex + 2] * 256 + bytes[firstIndex + 3];

                                                if (thisRegAddr == 0x3000 &&
                                                    thisRegCount == 1)
                                                {
                                                    var diState = Convert.ToString(bytes[firstIndex + 5], 2)
                                                        .PadLeft(8, '0');

                                                    Di1 = diState[7].ToString();
                                                    Di2 = diState[6].ToString();
                                                    Di3 = diState[5].ToString();
                                                    Di4 = diState[4].ToString();
                                                    Di5 = diState[3].ToString();
                                                    Di6 = diState[2].ToString();

                                                    _waitHandle.Set();
                                                }

                                                firstIndex += regCountPer + 2 + 2;
                                            }
                                        }
                                        else if (bytes.Length >= 38)
                                        {
                                            var firstIndex = 9;
                                            for (var i = 0; i < regCountPer; i++)
                                            {
                                                var thisRegAddr = bytes[firstIndex + 0] * 256 + bytes[firstIndex + 1];
                                                var thisRegCount = bytes[firstIndex + 2] * 256 + bytes[firstIndex + 3];
                                                if (thisRegAddr == 0x3000 &&
                                                    thisRegCount == 13)
                                                {
                                                    var diState = Convert.ToString(bytes[firstIndex + 5], 2)
                                                        .PadLeft(8, '0');
                                                    Di1 = diState[7].ToString();
                                                    Di2 = diState[6].ToString();
                                                    Di3 = diState[5].ToString();
                                                    Di4 = diState[4].ToString();
                                                    Di5 = diState[3].ToString();
                                                    Di6 = diState[2].ToString();

                                                    Freq1 = Math.Round((bytes[firstIndex + 6] * 256 + bytes[firstIndex + 7]) / 10f, 0, MidpointRounding.AwayFromZero);
                                                    Duty1 = Math.Round((bytes[firstIndex + 8] * 256 + bytes[firstIndex + 9]) / 100f, 0, MidpointRounding.AwayFromZero);
                                                    Freq2 = Math.Round((bytes[firstIndex + 10] * 256 + bytes[firstIndex + 11]) / 10f, 0, MidpointRounding.AwayFromZero);
                                                    Duty2 = Math.Round((bytes[firstIndex + 12] * 256 + bytes[firstIndex + 13]) / 100f, 0, MidpointRounding.AwayFromZero);
                                                    Freq3 = Math.Round((bytes[firstIndex + 14] * 256 + bytes[firstIndex + 15]) / 10f, 0, MidpointRounding.AwayFromZero);
                                                    Duty3 = Math.Round((bytes[firstIndex + 16] * 256 + bytes[firstIndex + 17]) / 100f, 0, MidpointRounding.AwayFromZero);
                                                    Freq4 = Math.Round((bytes[firstIndex + 18] * 256 + bytes[firstIndex + 19]) / 10f, 0, MidpointRounding.AwayFromZero);
                                                    Duty4 = Math.Round((bytes[firstIndex + 20] * 256 + bytes[firstIndex + 21]) / 100f, 0, MidpointRounding.AwayFromZero);
                                                    Freq5 = Math.Round((bytes[firstIndex + 22] * 256 + bytes[firstIndex + 23]) / 10f, 0, MidpointRounding.AwayFromZero);
                                                    Duty5 = Math.Round((bytes[firstIndex + 24] * 256 + bytes[firstIndex + 25]) / 100f, 0, MidpointRounding.AwayFromZero);
                                                    Freq6 = Math.Round((bytes[firstIndex + 26] * 256 + bytes[firstIndex + 27]) / 10f, 0, MidpointRounding.AwayFromZero);
                                                    Duty6 = Math.Round((bytes[firstIndex + 28] * 256 + bytes[firstIndex + 29]) / 100f, 0, MidpointRounding.AwayFromZero);
                                                    _waitHandle.Set();
                                                }

                                                firstIndex += regCountPer + 2 + 2;
                                            }
                                        }
                                    }
                                    else if (_currentFunc == MasterFunc.MasterReadCurs)
                                    {
                                        const int regCountPer = 2;

                                        //  if (bytes.Length >= 13 && bytes.Length >= (regCountPer + 2 + 2) * (bytes[11] * 256 + bytes[12]) + 2 + 1 + 1 + 1 + 2 + 2 + 2)
                                        if (bytes.Length >= 13)

                                        {
                                            var firstIndex = 9;

                                            for (var i = 0; i < regCountPer; i++)
                                            {
                                                var thisRegAddr = bytes[firstIndex + 0] * 256 + bytes[firstIndex + 1];
                                                var thisRegCount = bytes[firstIndex + 2] * 256 + bytes[firstIndex + 3];

                                                if (thisRegAddr == 0x4420 &&
                                                    thisRegCount == 4)
                                                {
                                                    HSDCurrent1 = bytes[firstIndex + 4] * 256 + bytes[firstIndex + 5];
                                                    HSDCurrent2 = bytes[firstIndex + 6] * 256 + bytes[firstIndex + 7];
                                                    HSDCurrent3 = bytes[firstIndex + 8] * 256 + bytes[firstIndex + 9];
                                                    HSDCurrent4 = bytes[firstIndex + 10] * 256 + bytes[firstIndex + 11];

                                                    _waitHandle.Set();
                                                }
                                                firstIndex += regCountPer + 2 + 2;
                                            }
                                        }
                                    }
                                }
                                else if (funcCode == 0x65) // 主站写寄存器
                                {
                                    if (_currentFunc == MasterFunc.MasterSetDOs)
                                        _waitHandle.Set();
                                    else if (_currentFunc == MasterFunc.SetSlaveCanFunc)
                                        _waitHandle.Set();
                                }
                                else if (funcCode == 0x67) // UartCAN转网口
                                {
                                    if (bytes.Length > 11)
                                    {
                                        var len = bytes[4] * 256 + bytes[5];

                                        if (len == bytes.Length - 6)
                                        {
                                            var uartBytes = new byte[bytes.Length - 6 - 1 - 1 - 1 - 2];
                                            Array.Copy(bytes, 9, uartBytes, 0, uartBytes.Length);
                                            UartCan.OnPushSciMsg(uartBytes);
                                        }
                                    }
                                }
                                else if (funcCode == 0x80)
                                {
                                    try
                                    {
                                        if (bytes[9] == 0x50)
                                        {
                                            ((RenesasMcuControllerGatewayLin)GatewayLin1).HitError();
                                        }
                                        else if (bytes[9] == 0x51)
                                        {
                                            ((RenesasMcuControllerGatewayLin)GatewayLin2).HitError();
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e);
                                    }
                                }
                            }

                            continue;
                        }
                        else
                        {
                            for (var i = startIndex; i < cc.Length; i++)
                                _lastRestBytes.Add(cc[i]);

                            break;
                        }
                    }
                    else
                    {
                        try
                        {
                            for (var i = startIndex; i < cc.Length; i++)
                                _lastRestBytes.Add(cc[i]);
                        }
                        catch (Exception e)
                        {
                            _lastRestBytes.Clear();
                        }

                        break;
                    }

                    break;
                }
            }
        }

        private byte _mrcCount = 0x01;

        /// <summary>
        /// 发送函数，会打包帧头、数据长度、CRC;
        /// 调用以功能码开始
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="masterFunc"></param>
        /// <param name="echoBytes"></param>
        /// <param name="delayMs"></param>
        /// <param name="controllerType">1=主站，2=从站</param>
        /// <returns></returns>
        public bool SendData(
            List<byte> bytes, MasterFunc masterFunc, out byte[] echoBytes, int delayMs = 10, byte controllerType = 0x01)
        {
            echoBytes = new byte[0];
            var isSuccess = false;

            lock (_lockSend)
            {
                var sendBytes = new List<byte>();
                sendBytes.AddRange(new byte[] { 0x55, 0xAA }); // data0,data1
                sendBytes.AddRange(new byte[] { 0x55, 0xAA }); // data2,data3

                // 除帧头外的所有数据字节长度
                var lenOfData = BitConverter.GetBytes((ushort)(bytes.Count + 1 + 2)).Reverse();
                sendBytes.AddRange(lenOfData);

                // 控制器码
                sendBytes.Add(controllerType);

                bytes[1] = _mrcCount;
                if (_mrcCount == 0xFF)
                    _mrcCount = (byte)0x01;
                else
                    _mrcCount = (byte)(_mrcCount + 0x01);
                sendBytes.AddRange(bytes);

                var crc = ValueHelper.Crc16(sendBytes);
                sendBytes.AddRange(crc);
                //sendBytes.AddRange(new byte[] { 0x5A, 0x5A });

                _currentFunc = masterFunc;

                var debugStr = ValueHelper.GetHextStr(sendBytes.ToArray());
                //Console.WriteLine("SEND: " + debugStr);

                MyUdpClient.SendMsgTo(
                    new IPEndPoint(RemoteIpAddress, RemotePort), sendBytes.ToArray(), delayMs);

                if (_currentFunc != MasterFunc.SendCanMsg && _currentFunc != MasterFunc.SendUartCanMsg)
                    isSuccess = _waitHandle.WaitOne(200);

                _currentFunc = MasterFunc.Free;
            }

            return isSuccess;
        }

        public void SpecialCmd(byte[] bytes)
        {
            lock (_lockSend)
            {
                _currentFunc = MasterFunc.SpecialCmd;

                MyUdpClient.SendMsgTo(
                    new IPEndPoint(RemoteIpAddress, RemotePort), bytes, 10);

                _currentFunc = MasterFunc.Free;
            }
        }

        public void SetRecvLinMsgEvent()
        {
            if (_currentFunc == MasterFunc.SendLinMsg || _currentFunc == MasterFunc.RecvLinMsg)
                _waitHandle.Set();
        }

        #endregion

        #region 从站相关

        /// <summary>
        /// 操作从站继电器
        /// </summary>
        [Description("操作从站继电器")]
        public void SetSlavesRLs()
        {
            if (_controllerSlaveWith8RLsMap.Any())
            {
                if (_currentFunc != MasterFunc.Free)
                    while (true)
                        if (_currentFunc == MasterFunc.Free)
                            break;

                var setBs = new List<byte>
                {
                    0x46, // 功能码
                    0x00, // 序号
                    BitConverter.GetBytes(_controllerSlaveWith8RLsMap.Count).Reverse().ToArray()[1]
                };

                foreach (var t in _controllerSlaveWith8RLsMap.Keys.ToList())
                {
                    var rlStateStr = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                        _controllerSlaveWith8RLsMap[t].Relay8 ? "1" : "0",
                        _controllerSlaveWith8RLsMap[t].Relay7 ? "1" : "0",
                        _controllerSlaveWith8RLsMap[t].Relay6 ? "1" : "0",
                        _controllerSlaveWith8RLsMap[t].Relay5 ? "1" : "0",
                        _controllerSlaveWith8RLsMap[t].Relay4 ? "1" : "0",
                        _controllerSlaveWith8RLsMap[t].Relay3 ? "1" : "0",
                        _controllerSlaveWith8RLsMap[t].Relay2 ? "1" : "0",
                        _controllerSlaveWith8RLsMap[t].Relay1 ? "1" : "0");
                    var rlStateByte = Convert.ToByte(rlStateStr, 2);

                    setBs.AddRange(BitConverter.GetBytes((ushort)t).Reverse());
                    setBs.AddRange(BitConverter.GetBytes((ushort)0x4000).Reverse());
                    setBs.AddRange(BitConverter.GetBytes((ushort)1).Reverse());
                    setBs.AddRange(new byte[] { 0x00, rlStateByte });
                }

                byte[] echoBytes;
                if (!SendData(setBs, MasterFunc.SetSlavesRLs, out echoBytes, controllerType: 0x00))
                    SendData(setBs, MasterFunc.SetSlavesRLs, out echoBytes, controllerType: 0x00);
            }
        }

        private int _readAdCount;

        /// <summary>
        /// 读取从站AD值
        /// </summary>
        [Description("读取从站AD值")]
        public void ReadSlavesADs()
        {
            if (!_controllerSlaveWith12ADsMap.Any())
                return;

            if (_currentFunc != MasterFunc.Free)
                while (true)
                    if (_currentFunc == MasterFunc.Free)
                        break;

            _readAdCount = 0;

            var keys = _controllerSlaveWith12ADsMap.Keys.ToList();
            foreach (var key in keys)
            {
                _controllerSlaveWith12ADsMap[key].Current1 = -9999;
                _controllerSlaveWith12ADsMap[key].Current2 = -9999;
                _controllerSlaveWith12ADsMap[key].Current3 = -9999;
                _controllerSlaveWith12ADsMap[key].Current4 = -9999;
                _controllerSlaveWith12ADsMap[key].Current5 = -9999;
                _controllerSlaveWith12ADsMap[key].Current6 = -9999;

                _controllerSlaveWith12ADsMap[key].Resistance1 = -9999;

                _controllerSlaveWith12ADsMap[key].Voltage2 = -9999;
                _controllerSlaveWith12ADsMap[key].Voltage3 = -9999;
                _controllerSlaveWith12ADsMap[key].Voltage4 = -9999;
                _controllerSlaveWith12ADsMap[key].Voltage5 = -9999;
                _controllerSlaveWith12ADsMap[key].Voltage6 = -9999;
                _controllerSlaveWith12ADsMap[key].Voltage7 = -9999;
            }

            var readBs = new List<byte>
            {
                0x44, // 功能码
                0x00, // 序号
                BitConverter.GetBytes(_controllerSlaveWith12ADsMap.Count).Reverse().ToArray()[1]
            };

            foreach (var t in _controllerSlaveWith12ADsMap.Keys.ToList())
            {
                readBs.AddRange(BitConverter.GetBytes((ushort)t).Reverse());
                readBs.AddRange(BitConverter.GetBytes((ushort)0x401A).Reverse());
                readBs.AddRange(BitConverter.GetBytes((ushort)13).Reverse());
            }

            byte[] echoBytes;
            SendData(readBs, MasterFunc.ReadSlavesADs, out echoBytes, controllerType: 0x00);
        }

        /// <summary>
        /// 读取从站光敏传感器采样值
        /// </summary>
        [Description("读取从站光敏传感器采样值")]
        public void ReadSlavesSensorValues()
        {
            if (!_controllerSlaveWith19PhotoSensorsMap.Any())
                return;

            if (_currentFunc != MasterFunc.Free)
                while (true)
                    if (_currentFunc == MasterFunc.Free)
                        break;

            _readAdCount = 0;

            var keys = _controllerSlaveWith19PhotoSensorsMap.Keys.ToList();
            foreach (var key in keys)
            {
                _controllerSlaveWith19PhotoSensorsMap[key].PhotoSensor1 = -9999;
                _controllerSlaveWith19PhotoSensorsMap[key].PhotoSensor2 = -9999;
                _controllerSlaveWith19PhotoSensorsMap[key].PhotoSensor3 = -9999;
                _controllerSlaveWith19PhotoSensorsMap[key].PhotoSensor4 = -9999;
                _controllerSlaveWith19PhotoSensorsMap[key].PhotoSensor5 = -9999;
                _controllerSlaveWith19PhotoSensorsMap[key].PhotoSensor6 = -9999;
                _controllerSlaveWith19PhotoSensorsMap[key].PhotoSensor7 = -9999;
                _controllerSlaveWith19PhotoSensorsMap[key].PhotoSensor8 = -9999;
                _controllerSlaveWith19PhotoSensorsMap[key].PhotoSensor9 = -9999;
                _controllerSlaveWith19PhotoSensorsMap[key].PhotoSensor10 = -9999;
                _controllerSlaveWith19PhotoSensorsMap[key].PhotoSensor11 = -9999;
                _controllerSlaveWith19PhotoSensorsMap[key].PhotoSensor12 = -9999;
                _controllerSlaveWith19PhotoSensorsMap[key].PhotoSensor13 = -9999;
                _controllerSlaveWith19PhotoSensorsMap[key].PhotoSensor14 = -9999;
                _controllerSlaveWith19PhotoSensorsMap[key].PhotoSensor15 = -9999;
                _controllerSlaveWith19PhotoSensorsMap[key].PhotoSensor16 = -9999;
                _controllerSlaveWith19PhotoSensorsMap[key].PhotoSensor17 = -9999;
                _controllerSlaveWith19PhotoSensorsMap[key].PhotoSensor18 = -9999;
                _controllerSlaveWith19PhotoSensorsMap[key].PhotoSensor19 = -9999;
            }

            var readBs = new List<byte>
            {
                0x44, // 功能码
                0x00, // 序号
                BitConverter.GetBytes(_controllerSlaveWith19PhotoSensorsMap.Count).Reverse().ToArray()[1]
            };

            foreach (var t in _controllerSlaveWith19PhotoSensorsMap.Keys.ToList())
            {
                readBs.AddRange(BitConverter.GetBytes((ushort)t).Reverse());
                readBs.AddRange(BitConverter.GetBytes((ushort)0x4000).Reverse());
                readBs.AddRange(BitConverter.GetBytes((ushort)0x13).Reverse());
            }

            byte[] echoBytes;
            SendData(readBs, MasterFunc.ReadSlavesSensors, out echoBytes, controllerType: 0x00);
        }

        /// <summary>
        /// 从站连接主站
        /// </summary>
        /// <param name="masterIp"></param>
        /// <param name="slaveId"></param>
        /// <param name="slaveInstance"></param>
        /// <returns></returns>
        public static bool SlaveTryConnectMaster(
            string masterIp, string slaveId, object slaveInstance)
        {
            if (!slaveId.StartsWith("0x", StringComparison.CurrentCultureIgnoreCase) &&
                !slaveId.StartsWith("&H", StringComparison.CurrentCultureIgnoreCase))
                return false;

            slaveId = slaveId.Substring(2);

            uint id;
            if (!uint.TryParse(slaveId, NumberStyles.HexNumber, null, out id))
                return false;

            if (!RenesasMcuControllerMasters.ContainsKey(masterIp) || slaveInstance == null)
                return false;

            if (slaveInstance is SyRenesasMcuControllerSlaveWith8RLs &&
                !RenesasMcuControllerMasters[masterIp]._controllerSlaveWith8RLsMap.ContainsKey(id))
            {
                RenesasMcuControllerMasters[masterIp]._controllerSlaveWith8RLsMap
                    .Add(id, (SyRenesasMcuControllerSlaveWith8RLs)slaveInstance);

                return true;
            }
            else if (slaveInstance is SyRenesasMcuControllerSlaveWith12ADs &&
                     !RenesasMcuControllerMasters[masterIp]._controllerSlaveWith12ADsMap.ContainsKey(id))
            {
                RenesasMcuControllerMasters[masterIp]._controllerSlaveWith12ADsMap
                    .Add(id, (SyRenesasMcuControllerSlaveWith12ADs)slaveInstance);

                return true;
            }
            else if (slaveInstance is SyRenesasMcuControllerSlaveWith19PhotoSensor &&
                     !RenesasMcuControllerMasters[masterIp]._controllerSlaveWith19PhotoSensorsMap.ContainsKey(id))
            {
                RenesasMcuControllerMasters[masterIp]._controllerSlaveWith19PhotoSensorsMap
                    .Add(id, (SyRenesasMcuControllerSlaveWith19PhotoSensor)slaveInstance);

                return true;
            }

            return false;
        }

        /// <summary>
        /// RL从站单独控制继电器
        /// </summary>
        /// <param name="masterIp"></param>
        /// <param name="slaveId"></param>
        public static void SlaveSetRLs(
            string masterIp, uint slaveId)
        {
            if (RenesasMcuControllerMasters.ContainsKey(masterIp) &&
                RenesasMcuControllerMasters[masterIp]._controllerSlaveWith8RLsMap.ContainsKey(slaveId))
            {
                //var rl = RenesasMcuControllerMasters[masterIp]._controllerSlaveWith8RLsMap[slaveId].Relay1;
                //Console.WriteLine(rl);

                var rlStateStr = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                    RenesasMcuControllerMasters[masterIp]._controllerSlaveWith8RLsMap[slaveId].Relay8 ? "1" : "0",
                    RenesasMcuControllerMasters[masterIp]._controllerSlaveWith8RLsMap[slaveId].Relay7 ? "1" : "0",
                    RenesasMcuControllerMasters[masterIp]._controllerSlaveWith8RLsMap[slaveId].Relay6 ? "1" : "0",
                    RenesasMcuControllerMasters[masterIp]._controllerSlaveWith8RLsMap[slaveId].Relay5 ? "1" : "0",
                    RenesasMcuControllerMasters[masterIp]._controllerSlaveWith8RLsMap[slaveId].Relay4 ? "1" : "0",
                    RenesasMcuControllerMasters[masterIp]._controllerSlaveWith8RLsMap[slaveId].Relay3 ? "1" : "0",
                    RenesasMcuControllerMasters[masterIp]._controllerSlaveWith8RLsMap[slaveId].Relay2 ? "1" : "0",
                    RenesasMcuControllerMasters[masterIp]._controllerSlaveWith8RLsMap[slaveId].Relay1 ? "1" : "0");
                var rlStateByte = Convert.ToByte(rlStateStr, 2);

                var setBs = new List<byte>
                {
                    0x46, // 功能码
                    0x00, // 序号
                    0x01
                };

                setBs.AddRange(BitConverter.GetBytes((ushort)slaveId).Reverse());
                setBs.AddRange(BitConverter.GetBytes((ushort)0x4000).Reverse());
                setBs.AddRange(BitConverter.GetBytes((ushort)1).Reverse());
                setBs.AddRange(new byte[] { 0x00, rlStateByte });

                byte[] echoBytes;
                if (!RenesasMcuControllerMasters[masterIp]
                        .SendData(setBs, MasterFunc.SlaveSetRLs, out echoBytes, controllerType: 0x00))
                    RenesasMcuControllerMasters[masterIp].SendData(setBs, MasterFunc.SlaveSetRLs, out echoBytes,
                            controllerType: 0x00);
            }
        }

        /// <summary>
        /// AD从站单独刷新AD值
        /// </summary>
        /// <param name="masterIp"></param>
        /// <param name="slaveId"></param>
        public static void SlaveReadADs(
            string masterIp, uint slaveId)
        {
            if (RenesasMcuControllerMasters.ContainsKey(masterIp) &&
                RenesasMcuControllerMasters[masterIp]._controllerSlaveWith12ADsMap.ContainsKey(slaveId))
            {
                var readBs = new List<byte>
                {
                    0x44, // 功能码
                    0x00, // 序号
                    0x01
                };

                readBs.AddRange(BitConverter.GetBytes((ushort)slaveId).Reverse());
                readBs.AddRange(BitConverter.GetBytes((ushort)0x401A).Reverse());
                readBs.AddRange(BitConverter.GetBytes((ushort)13).Reverse());

                byte[] echoBytes;
                RenesasMcuControllerMasters[masterIp].SendData(readBs, MasterFunc.SlaveReadADs, out echoBytes, controllerType: 0x00);
            }
        }

        /// <summary>
        /// 从站单独刷新光敏电阻采样值
        /// </summary>
        /// <param name="masterIp"></param>
        /// <param name="slaveId"></param>
        public static void SlaveReadSensorValues(string masterIp, uint slaveId)
        {
            if (RenesasMcuControllerMasters.ContainsKey(masterIp) &&
                RenesasMcuControllerMasters[masterIp]._controllerSlaveWith19PhotoSensorsMap.ContainsKey(slaveId))
            {
                var readBs = new List<byte>
                {
                    0x44, // 功能码
                    0x00, // 序号
                    0x01
                };

                readBs.AddRange(BitConverter.GetBytes((ushort)slaveId).Reverse());
                readBs.AddRange(BitConverter.GetBytes((ushort)0x4000).Reverse());
                readBs.AddRange(BitConverter.GetBytes((ushort)19).Reverse());

                byte[] echoBytes;
                RenesasMcuControllerMasters[masterIp].SendData(readBs, MasterFunc.SlaveReadSensorValues, out echoBytes, controllerType: 0x00);
            }
        }

        public void SetSlaveId(string slaveId)
        {
            if (!slaveId.StartsWith("0x", StringComparison.CurrentCultureIgnoreCase) &&
                !slaveId.StartsWith("&H", StringComparison.CurrentCultureIgnoreCase))
                return;

            slaveId = slaveId.Substring(2);

            uint id;
            if (!uint.TryParse(slaveId, NumberStyles.HexNumber, null, out id))
                return;

            var setBs = new List<byte>
            {
                0x46, // 功能码
                0x00, // 序号
                0x01
            };

            setBs.AddRange(BitConverter.GetBytes((ushort)0x0101).Reverse());
            setBs.AddRange(BitConverter.GetBytes((ushort)0x0000).Reverse());
            setBs.AddRange(BitConverter.GetBytes((ushort)1).Reverse());
            setBs.AddRange(BitConverter.GetBytes((ushort)id).Reverse());

            byte[] echoBytes;
            SendData(setBs, MasterFunc.SetSlaveId, out echoBytes, controllerType: 0x00);
        }

        public void SetSlaveCanFunc(string type, uint bauteRate)
        {
            int funcType;
            if (int.TryParse(type, out funcType))
            {
                if (funcType == 0 || funcType == 1)
                {
                    var setBs = new List<byte>
                    {
                        0x65, // 功能码
                        0x00, // 序号
                    };
                    setBs.AddRange(BitConverter.GetBytes((ushort)0x1000).Reverse());
                    setBs.AddRange(BitConverter.GetBytes((ushort)1).Reverse());
                    setBs.Add(0x00);
                    setBs.Add((byte)funcType);
                    byte[] echoBytes;
                    SendData(setBs, MasterFunc.SetSlaveCanFunc, out echoBytes, controllerType: 0x01);

                    setBs.Clear();
                    setBs.Add(0x65);
                    setBs.Add(0x00);
                    setBs.AddRange(BitConverter.GetBytes((ushort)0x6040).Reverse());
                    setBs.AddRange(BitConverter.GetBytes((ushort)1).Reverse());
                    setBs.Add(0x00);
                    setBs.Add((byte)0x02);
                    SendData(setBs, MasterFunc.SetSlaveCanFunc, out echoBytes, controllerType: 0x01);

                    setBs.Clear();
                    setBs.Add(0x65);
                    setBs.Add(0x00);
                    setBs.AddRange(BitConverter.GetBytes((ushort)0x6042).Reverse());
                    setBs.AddRange(BitConverter.GetBytes((ushort)2).Reverse());
                    //setBs.Add((byte)0xA1);
                    //setBs.Add((byte)0x20);
                    //setBs.Add((byte)0x00);
                    //setBs.Add((byte)0x07);

                    //uint x = 500000;
                    //uint x = 500000 * 2;
                    uint x = bauteRate;
                    var bs = BitConverter.GetBytes(x);
                    var br = new byte[] { bs[1], bs[0], bs[3], bs[2] };
                    for (var i = 0; i < br.Length; i++)
                        setBs.Add(br[i]);

                    SendData(setBs, MasterFunc.SetSlaveCanFunc, out echoBytes, controllerType: 0x01);

                }
            }
        }

        public void SetLinBauteRate(int type, uint bauteRate)
        {
            var addr1 = (ushort)0x6000;
            var addr2 = (ushort)0x6002;

            var isMaster = type == 0 || type == 2;

            if (type == 2 || type == 3)
            {
                addr1 = (ushort)0x6020;
                addr2 = (ushort)0x6022;
            }

            byte[] echoBytes;

            var bauteRateBs = BitConverter.GetBytes(bauteRate);
            var bauteRateBs0 = bauteRateBs[1];
            var bauteRateBs1 = bauteRateBs[0];
            var bauteRateBs2 = bauteRateBs[3];
            var bauteRateBs3 = bauteRateBs[2];

            var setBs = new List<byte>();

            setBs.Clear();
            setBs.Add(0x65);
            setBs.Add(0x00);
            setBs.AddRange(BitConverter.GetBytes(addr1).Reverse());
            setBs.AddRange(BitConverter.GetBytes((ushort)1).Reverse());
            setBs.Add(0x00);
            setBs.Add(isMaster ? (byte)0x00 : (byte)0x01);
            SendData(setBs, MasterFunc.SetSlaveCanFunc, out echoBytes, controllerType: 0x01);

            setBs.Clear();
            setBs.Add(0x65);
            setBs.Add(0x00);
            setBs.AddRange(BitConverter.GetBytes(addr2).Reverse());
            setBs.AddRange(BitConverter.GetBytes((ushort)2).Reverse());
            setBs.Add((byte)bauteRateBs0);
            setBs.Add((byte)bauteRateBs1);
            setBs.Add((byte)bauteRateBs2);
            setBs.Add((byte)bauteRateBs3);

            SendData(setBs, MasterFunc.SetSlaveCanFunc, out echoBytes, controllerType: 0x01);
        }

        #endregion

        #region 主站相关

        [Description("主站读取DI")]
        public void MasterReadDIs()
        {
            if (_currentFunc != MasterFunc.Free)
                while (true)
                    if (_currentFunc == MasterFunc.Free)
                        break;

            Di1 = string.Empty;
            Di2 = string.Empty;
            Di3 = string.Empty;
            Di4 = string.Empty;
            Di5 = string.Empty;
            Di6 = string.Empty;

            var readBs = new List<byte>
            {
                0x64, // 功能码
                0x00, // 序号
            };

            readBs.AddRange(BitConverter.GetBytes((ushort)0x3000).Reverse());
            readBs.AddRange(BitConverter.GetBytes((ushort)1).Reverse());

            byte[] echoBytes;
            SendData(readBs, MasterFunc.MasterReadDIs, out echoBytes, controllerType: 0x01);
        }

        [Description("主站读取DI和Duty")]
        public void MasterReadDIsAndDuty()
        {
            if (_currentFunc != MasterFunc.Free)
                while (true)
                    if (_currentFunc == MasterFunc.Free)
                        break;

            Di1 = string.Empty;
            Di2 = string.Empty;
            Di3 = string.Empty;
            Di4 = string.Empty;
            Di5 = string.Empty;
            Di6 = string.Empty;

            var readBs = new List<byte>
            {
                0x64, // 功能码
                0x00, // 序号
            };

            readBs.AddRange(BitConverter.GetBytes((ushort)0x3000).Reverse());
            readBs.AddRange(BitConverter.GetBytes((ushort)13).Reverse());

            byte[] echoBytes;
            SendData(readBs, MasterFunc.MasterReadDIs, out echoBytes, controllerType: 0x01);
        }

        [Description("主站读取电流")]
        public void MasterReadHSDCurr()
        {
            if (_currentFunc != MasterFunc.Free)
                while (true)
                    if (_currentFunc == MasterFunc.Free)
                        break;
            HSDCurrent1 = -9999;
            HSDCurrent2 = -9999;
            HSDCurrent3 = -9999;
            HSDCurrent4 = -9999;
            var readBs = new List<byte>
            {
                0x64, // 功能码
                0x00, // 序号
            };
            readBs.AddRange(BitConverter.GetBytes((ushort)0x4420).Reverse());
            readBs.AddRange(BitConverter.GetBytes((ushort)4).Reverse());
            byte[] echoBytes;
            SendData(readBs, MasterFunc.MasterReadCurs, out echoBytes, controllerType: 0x01);
        }

        [Description("主站设置DO")]
        public void MasetSetDOs()
        {
            if (_currentFunc != MasterFunc.Free)
                while (true)
                    if (_currentFunc == MasterFunc.Free)
                        break;

            var setBs = new List<byte>
            {
                0x65, // 功能码
                0x00, // 序号
            };

            setBs.AddRange(BitConverter.GetBytes((ushort)0x4400).Reverse());
            setBs.AddRange(BitConverter.GetBytes((ushort)1).Reverse());

            var doStates = string.Format("0000{0}{1}{2}{3}", Do4 ? "1" : "0", Do3 ? "1" : "0", Do2 ? "1" : "0", Do1 ? "1" : "0");
            setBs.Add(0x00);
            setBs.Add(Convert.ToByte(doStates, 2));

            byte[] echBytes;
            if (!SendData(setBs, MasterFunc.MasterSetDOs, out echBytes, controllerType: 0x01))
                SendData(setBs, MasterFunc.MasterSetDOs, out echBytes, controllerType: 0x01);
        }

        #endregion

        public class RenesasMcuControllerGatewayCan : CanBus
        {
            private byte ChId { get; set; }
            private SyRenesasMcuControllerMaster ParentController { get; set; }

            private new const int MaxFrameCount = 15;

            public RenesasMcuControllerGatewayCan(
                int canChannel, SyRenesasMcuControllerMaster controller)
                : base(string.Format("{0}_Can{1}", controller.RemoteIpPort, canChannel + 1), MaxFrameCount)
            {
                ChId = (byte)canChannel;
                ParentController = controller;
                RegisterSendAction(SendMultipleCans);
            }

            private void SendMultipleCans(CanDataPackage[] dataPackages)
            {
                if (dataPackages == null || !dataPackages.Any())
                    return;

                var sendAct = new Action<CanDataPackage[]>(dp =>
                {
                    const byte funcCode = (byte)MasterFunc.SendCanMsg;

                    var bs = new List<byte> { funcCode, 0x00, ChId };

                    foreach (var canDataPackage in dp)
                    {
                        var canDataPackageList = new List<byte>();

                        // can id
                        canDataPackageList.AddRange(BitConverter.GetBytes(canDataPackage.CanId).Reverse());

                        // flag, bit0=>0是标准帧，1是远程帧
                        canDataPackageList.Add(canDataPackage.CanType == CanType.Standard ? (byte)0x00 : (byte)0x01);
                        //canDataPackageList.Add(0x00);
                        //canDataPackageList.Add(canDataPackage.CanProtocol == CanProtocol.CanFd ? (byte)0x01 : (byte)0x00);
                        switch (canDataPackage.CanProtocol)
                        {
                            case CanProtocol.CanFd:
                                canDataPackageList.Add((byte)0x01);
                                break;
                            case CanProtocol.Can:
                                canDataPackageList.Add((byte)0x00);
                                break;
                            case CanProtocol.SpeedCanFd:
                                canDataPackageList.Add((byte)0x03);
                                break;
                        }
                        // data len
                        var datalen = (ushort)canDataPackage.CanDataLen;
                        var canDatas = new byte[datalen];
                        for (var i = 0; i < datalen; i++)
                            canDatas[i] = canDataPackage.CanData[i];
                        canDataPackageList.Add(BitConverter.GetBytes(datalen).Reverse().ToArray()[1]);

                        // can datas
                        canDataPackageList.AddRange(canDatas);

                        bs.AddRange(canDataPackageList);
                    }

                    byte[] echoBytes;
                    ParentController.SendData(bs, MasterFunc.SendCanMsg, out echoBytes, 3);
                });

                if (dataPackages.Length <= MaxFrameCount)
                    sendAct(dataPackages);
                else
                {
                    var count = dataPackages.Length / MaxFrameCount;
                    var rest = dataPackages.Length % MaxFrameCount;

                    var temp = dataPackages.ToList();
                    for (var i = 0; i < count; i++)
                    {
                        var sendTemp = new CanDataPackage[MaxFrameCount];
                        Array.Copy(temp.ToArray(), sendTemp, MaxFrameCount);
                        temp.RemoveRange(0, MaxFrameCount);
                        sendAct(sendTemp);
                    }

                    if (temp.Any())
                        sendAct(temp.ToArray());
                }
            }

            public void SendStandardCanMsg(
                uint canId, IEnumerable<byte> values)
            {
                SendMultipleCans(new CanDataPackage[]
                    { new CanDataPackage(canId, CanProtocol.Can, CanType.Standard, CanFormat.Data, values.ToArray()) });
            }

            public void SendExtendedCanMsg(
                uint canId, IEnumerable<byte> values)
            {
                SendMultipleCans(new CanDataPackage[]
                    { new CanDataPackage(canId, CanProtocol.Can, CanType.Extended, CanFormat.Data, values.ToArray()) });
            }

            private void SendCanMsg(
                uint canId,
                CanType canMsgType,
                IEnumerable<byte> canMsgValues)
            {
                SendMultipleCans(new CanDataPackage[]
                    { new CanDataPackage(canId, CanProtocol.Can, canMsgType, CanFormat.Data, canMsgValues.ToArray()) });
            }
        }

        public class RenesasMcuControllerGatewayLin : LinBus
        {
            //public LinBus.LinDataPackage LinMsgBuffer;

            private byte ChId { get; set; }
            private SyRenesasMcuControllerMaster ParentController { get; set; }

            public RenesasMcuControllerGatewayLin(
                int linChannel, SyRenesasMcuControllerMaster controller) :
                base(string.Format("{0}_Lin{1}", controller.RemoteIpPort, linChannel + 1))
            {
                ChId = (byte)linChannel;
                ParentController = controller;
                RegisterSendMasterLinFunc(SendMasterLinMessage);
                RegisterSendSlaveLinACtion(SendSlaveLinMessage);
            }

            private bool SendMasterLinMessage(LinDataPackage linDataPackage, bool isWaitSlaveLin)
            {
                if (linDataPackage == null)
                    return false;

                //LinMsgBuffer = null;
                SendLinMessage(linDataPackage.LinId, linDataPackage.LinData);

                return true;

                //if (LinMsgBuffer != null &&
                //    LinBus.ConvertLinId(linDataPackage.LinId) == LinMsgBuffer.LinId &&
                //    linDataPackage.LinDataLen == LinMsgBuffer.LinDataLen &&
                //    string.Equals(ValueHelper.GetHextStr(LinMsgBuffer.LinData), ValueHelper.GetHextStr(linDataPackage.LinData), StringComparison.CurrentCultureIgnoreCase))
                //{
                //    if (isWaitSlaveLin)
                //    {
                //        LinMsgBuffer = null;

                //        return true;
                //    }
                //    else
                //    {
                //        return true;
                //    }
                //}

                //return false;
            }

            private void SendSlaveLinMessage(byte slaveLinId)
            {
                //LinMsgBuffer = null;
                SendLinMessage(slaveLinId, new byte[0]);
            }

            private void SendLinMessage(byte linId, byte[] valueBytes)
            {
                if (valueBytes.Length > 0)
                {
                    const byte funcCode = (byte)MasterFunc.SendLinMsg;

                    var bs = new List<byte>
                    {
                        funcCode, 0x00, ChId,
                        linId,
                        (byte)valueBytes.Length
                    };

                    var actBs = new byte[8];
                    Array.Copy(valueBytes, 0, actBs, 0, valueBytes.Length);
                    bs.AddRange(actBs);

                    byte[] echoBytes;
                    ParentController.SendData(bs, MasterFunc.SendLinMsg, out echoBytes);
                }
                else
                {
                    const byte funcCode = (byte)MasterFunc.RecvLinMsg;

                    var bs = new List<byte>
                    {
                        funcCode, 0x00, ChId,
                        linId,
                        (byte)0x08
                    };

                    byte[] echoBytes;
                    ParentController.SendData(bs, MasterFunc.SendLinMsg, out echoBytes);
                }
            }

            private EventWaitHandle WaitErrHandle = new AutoResetEvent(false);
            private long _hitErrorTs = HighPrecisionTimer.GetTimestamp();

            public void HitError()
            {
                _hitErrorTs = HighPrecisionTimer.GetTimestamp();
            }

            public void ReceivedMsg(LinDataPackage[] tempRecvPackages)
            {
                if (tempRecvPackages != null && tempRecvPackages.Any())
                {
                    #region 注释取消这段代码，否则刷新app、cal时会很慢，20251203

                    //foreach (var t in tempRecvPackages)
                    //{
                    //    Console.WriteLine("CH={0}, RECV DATA: LIN ID = {1}, LIN DATA={2}", ChId, ValueHelper.GetHextStrWithOx(t.LinId), ValueHelper.GetHextStrWithOx(t.LinData));
                    //}

                    //var waitErr = WaitErrHandle.WaitOne(50);
                    //if (waitErr)
                    //    return;

                    ////Thread.Sleep(25);
                    //var nowTs = HighPrecisionTimer.GetTimestamp();
                    //var ts = HighPrecisionTimer.GetTimestampIntervalMs(_hitErrorTs, nowTs);
                    //if (Math.Abs(ts) <= 100)
                    //    return;

                    #endregion

                    //for (var i = 0; i < 25; i++)
                    //{
                    //    var nowTs = HighPrecisionTimer.GetTimestamp();
                    //    var ts = HighPrecisionTimer.GetTimestampIntervalMs(_hitErrorTs, nowTs);
                    //    if (Math.Abs(ts) <= 2500)
                    //        return;

                    //    Thread.Sleep(1);
                    //}

                    //if (ChId == 0x01)
                    //{
                    //    var nowTs = HighPrecisionTimer.GetTimestamp();
                    //    var ts = HighPrecisionTimer.GetTimestampIntervalMs(_hitErrorTs, nowTs);
                    //    Console.WriteLine(ts);
                    //}
                    RecviveLinDatas(tempRecvPackages);
                    ParentController.SetRecvLinMsgEvent();
                }
            }
        }

        public class RenesasMcuControllerGatewayUartCan : MySerialPort
        {
            private SyRenesasMcuControllerMaster ParentController { get; set; }

            public RenesasMcuControllerGatewayUartCan(SyRenesasMcuControllerMaster controller) : base(controller
                .RemoteIpPort)
            {
                ParentController = controller;
            }

            public override void SendCommand(string str)
            {
                var bytes = Encoding.ASCII.GetBytes(str);
                SendUartCanBytes(bytes.ToArray(), 10);
            }

            public override void SendCommand(byte[] bytes, int delayMs = 10)
            {
                SendUartCanBytes(bytes, delayMs);
            }

            private void SendUartCanBytes(IEnumerable<byte> bytes, int delayMs)
            {
                byte[] echoBytes;
                const byte funcCode = (byte)MasterFunc.SendUartCanMsg;
                var bs = new List<byte> { funcCode, 0x00 };
                bs.AddRange(bytes);

                ParentController.SendData(bs, MasterFunc.SendUartCanMsg, out echoBytes, delayMs: delayMs, controllerType: 0x01);
            }
        }
    }
}
