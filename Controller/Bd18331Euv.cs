using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using CommonUtility;

namespace Controller
{
    public sealed class Bd18331Euv : ControllerBase
    {
        public MySerialPort MySerialPort;

        [Description("R,是否是DC模式")] public bool IsDcMode;

        public Bd18331Euv(string name)
            : base(name)
        {
            MySerialPort.PushSciMsg += MySerialPort_PushSciMsg;

            _keepNetworkThread = new Thread(KeepNetwork) { IsBackground = true };
            _keepNetworkThread.Start();
        }

        ~Bd18331Euv() => Dispose();

        private readonly Thread _keepNetworkThread;
        private readonly object _lockDevList = new object();
        private int _periodCount;

        private void KeepNetwork()
        {
            while (_keepNetworkThread.IsAlive)
            {
                if (!_keepNetworkThread.IsAlive)
                    break;

                Thread.Sleep(15);
                _periodCount++;
                if (_periodCount > 10000 * 15)
                    _periodCount = 0;

                if (MySerialPort == null)
                    continue;

                lock (_lockDevList)
                {
                    if (!_devList.Any())
                        continue;

                    //BroadcastExitLmp();

                    foreach (var dev in _devList)
                    {
                        #region sys init

                        SysInit(dev);

                        #endregion

                        #region PWM设置

                        var pwmSettings = new List<byte>();
                        //pwmSettings.AddRange(new byte[] { 0xDB, 0xB6, 0x6D });
                        pwmSettings.AddRange(new byte[] { 0xFF, 0xFF, 0xFF });

                        for (var j = 1; j <= 24; j++)
                        {
                            var pwmDuty = dev.PwmDuty[j];
                            pwmSettings.Add((byte)(Math.Round(pwmDuty / 100 * 256, 0) - 1));
                        }

                        WriteReg(0x30, pwmSettings, dev: dev);
                        Thread.Sleep(1);

                        #endregion

                        //#region DC电流设置

                        //var dcSettings = new List<byte>();
                        //for (var j = 1; j <= 24; j = j + 2)
                        //{
                        //    var hi = dev.DimSet[j + 1];
                        //    var lo = dev.DimSet[j];

                        //    var bitHi = Convert.ToString(hi, 2).PadLeft(8, '0').Substring(4, 4);
                        //    var bitLo = Convert.ToString(lo, 2).PadLeft(8, '0').Substring(4, 4);
                        //    var bits = string.Format("{0}{1}", bitHi, bitLo);
                        //    var b = Convert.ToByte(bits, 2);
                        //    dcSettings.Add(b);
                        //}
                        //WriteReg(0x24, dcSettings, dev: dev);
                        //Thread.Sleep(1);

                        //#endregion

                        #region 开关LED

                        var led1Toled8Byte =
                            Convert.ToByte(string.Format("{7}{6}{5}{4}{3}{2}{1}{0}",
                                dev.ChannelEnable[1], dev.ChannelEnable[2],
                                dev.ChannelEnable[3], dev.ChannelEnable[4],
                                dev.ChannelEnable[5], dev.ChannelEnable[6],
                                dev.ChannelEnable[7], dev.ChannelEnable[8]), 2);
                        var led9Toled16Byte =
                            Convert.ToByte(string.Format("{7}{6}{5}{4}{3}{2}{1}{0}",
                                dev.ChannelEnable[9], dev.ChannelEnable[10],
                                dev.ChannelEnable[11], dev.ChannelEnable[12],
                                dev.ChannelEnable[13], dev.ChannelEnable[14],
                                dev.ChannelEnable[15], dev.ChannelEnable[16]), 2);
                        var led17Toled24Byte =
                            Convert.ToByte(string.Format("{7}{6}{5}{4}{3}{2}{1}{0}",
                                dev.ChannelEnable[17], dev.ChannelEnable[18],
                                dev.ChannelEnable[19], dev.ChannelEnable[20],
                                dev.ChannelEnable[21], dev.ChannelEnable[22],
                                dev.ChannelEnable[23], dev.ChannelEnable[24]), 2);
                        WriteReg(0x04, new[] { led1Toled8Byte, led9Toled16Byte, led17Toled24Byte }, dev: dev);
                        Thread.Sleep(1);

                        #endregion
                    }
                }
            }
        }

        private void MySerialPort_PushSciMsg(string name, byte[] datas)
        {
            if (MySerialPort == null || MySerialPort.Name != name)
                return;
            var str =
                datas.Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
            Console.WriteLine(str);

            if (str.Length != (4 + 2 + 7 + 2) * 2 || !str.StartsWith("558"))
                return;
            var toCrcSendBytes = new List<byte>();
            for (var i = 2; i < 8; i = i + 2)
            {
                toCrcSendBytes.Add(Convert.ToByte(str.Substring(i, 2), 16));
            }

            var toCrcSend = GetCrc(toCrcSendBytes).Reverse().ToArray();
            if (toCrcSend[0] != Convert.ToByte(str.Substring(8, 2), 16) ||
                toCrcSend[1] != Convert.ToByte(str.Substring(10, 2), 16))
                return;
            var toCrcRecvBytes = new List<byte>();
            for (var j = 12; j < 26; j = j + 2)
            {
                toCrcRecvBytes.Add(Convert.ToByte(str.Substring(j, 2), 16));
            }

            var toCrcRecv = GetCrc(toCrcRecvBytes).Reverse().ToArray();
            if (toCrcRecv[0] != Convert.ToByte(str.Substring(26, 2), 16) ||
                toCrcRecv[1] != Convert.ToByte(str.Substring(28, 2), 16))
                return;
            var header = toCrcSendBytes[0];
            var headerBits = Convert.ToString(header, 2).PadLeft(8, '0');
            var cs0 = headerBits[7].ToString();
            var cs1 = headerBits[6].ToString();
            var cs2 = headerBits[5].ToString();
            var cs3 = headerBits[4].ToString();

            var devAddr = Convert.ToUInt16(string.Format("0000{0}{1}{2}{3}", cs3, cs2, cs1, cs0), 2);

            #region Short error status

            var short1Bits = Convert.ToString(toCrcRecvBytes[0], 2).PadLeft(8, '0');
            short1Bits = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", short1Bits[7], short1Bits[6],
                short1Bits[5], short1Bits[4], short1Bits[3], short1Bits[2], short1Bits[1], short1Bits[0]);
            var short2Bits = Convert.ToString(toCrcRecvBytes[1], 2).PadLeft(8, '0');
            short2Bits = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", short2Bits[7], short2Bits[6],
                short2Bits[5], short2Bits[4], short2Bits[3], short2Bits[2], short2Bits[1], short2Bits[0]);
            var short3Bits = Convert.ToString(toCrcRecvBytes[2], 2).PadLeft(8, '0');
            short3Bits = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", short3Bits[7], short3Bits[6],
                short3Bits[5], short3Bits[4], short3Bits[3], short3Bits[2], short3Bits[1], short3Bits[0]);

            var shortStatus = string.Format("Dev{0}Led1To24ShortErrorStatus", devAddr);

            if (GetType().GetField(shortStatus) != null)
                GetType().GetField(shortStatus).SetValue(this, short1Bits + short2Bits + short3Bits);

            #endregion

            #region Open error status

            var open1Bits = Convert.ToString(toCrcRecvBytes[3], 2).PadLeft(8, '0');
            open1Bits = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", open1Bits[7], open1Bits[6],
                open1Bits[5], open1Bits[4], open1Bits[3], open1Bits[2], open1Bits[1], open1Bits[0]);
            var open2Bits = Convert.ToString(toCrcRecvBytes[4], 2).PadLeft(8, '0');
            open2Bits = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", open2Bits[7], open2Bits[6],
                open2Bits[5], open2Bits[4], open2Bits[3], open2Bits[2], open2Bits[1], open2Bits[0]);
            var open3Bits = Convert.ToString(toCrcRecvBytes[5], 2).PadLeft(8, '0');
            open3Bits = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", open3Bits[7], open3Bits[6],
                open3Bits[5], open3Bits[4], open3Bits[3], open3Bits[2], open3Bits[1], open3Bits[0]);

            var openStatus = string.Format("Dev{0}Led1To24OpenErrorStatus", devAddr);

            if (GetType().GetField(openStatus) != null)
                GetType().GetField(openStatus).SetValue(this, open1Bits + open2Bits + open3Bits);

            #endregion
        }

        private readonly List<Dev> _devList = new List<Dev>();

        /// <summary>
        /// 初始化地址
        /// We can set from “0000b” to “1111b”
        /// </summary>
        /// <param name="addr"></param>
        [Description("添加一个地址的芯片")]
        public void AddDev(string addr)
        {
            lock (_lockDevList)
            {
                try
                {
                    ushort addrUshortValue;
                    if (ushort.TryParse(addr, out addrUshortValue) && addr.Length != 4)
                    {
                        if (addrUshortValue > 15)
                            return;
                        if (_devList.FindIndex(f => f.DevAddr == addrUshortValue) != -1)
                            return;
                        var bits = Convert.ToString(addrUshortValue, 2).PadLeft(8, '0');

                        _devList.Add(new Dev
                        {
                            DevAddr = addrUshortValue,

                            Cs0Setting = bits.Substring(7, 1),
                            Cs1Setting = bits.Substring(6, 1),
                            Cs2Setting = bits.Substring(5, 1),
                            Cs3Setting = bits.Substring(4, 1)
                        });
                    }
                    else if (addr.Length == 4)
                    {
                        var cs0 = addr.Substring(3, 1);
                        var cs1 = addr.Substring(2, 1);
                        var cs2 = addr.Substring(1, 1);
                        var cs3 = addr.Substring(0, 1);

                        var devAddr = Convert.ToUInt16(string.Format("0000{0}{1}{2}{3}", cs3, cs2, cs1, cs0), 2);

                        if (devAddr > 15)
                            return;

                        if (_devList.FindIndex(f => f.DevAddr == devAddr) == -1)
                        {
                            _devList.Add(new Dev
                            {
                                DevAddr = devAddr,
                                Cs0Setting = cs0,
                                Cs1Setting = cs1,
                                Cs2Setting = cs2,
                                Cs3Setting = cs3
                            });
                        }
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        [Description("读目标芯片的LED状态")]
        public void ReadStatus(string addr)
        {
            lock (_lockDevList)
            {
                if (!_devList.Any())
                    return;

                try
                {
                    ushort addrUshortValue;
                    if (!ushort.TryParse(addr, out addrUshortValue))
                        return;
                    if (addrUshortValue > 15)
                        return;

                    var findDevIndex = _devList.FindIndex(f => f.DevAddr == addrUshortValue);
                    if (findDevIndex == -1)
                        return;

                    var shortStatus = string.Format("Dev{0}Led1To24ShortErrorStatus", addrUshortValue);
                    var openStatus = string.Format("Dev{0}Led1To24OpenErrorStatus", addrUshortValue);

                    if (GetType().GetField(openStatus) != null)
                        GetType().GetField(openStatus).SetValue(this, string.Empty);
                    if (GetType().GetField(shortStatus) != null)
                        GetType().GetField(shortStatus).SetValue(this, string.Empty);

                    ReadReg(_devList[findDevIndex], 0x4B, 7);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        [Description("设置目标芯片的目标通道的PWM")]
        public void SetPwmDuty(string addr, string chIndex, string pwm)
        {
            lock (_lockDevList)
            {
                if (!_devList.Any())
                    return;

                int ledLedIndex;
                if (!int.TryParse(chIndex, out ledLedIndex))
                    return;
                if (ledLedIndex < 0 || ledLedIndex > 24)
                    return;

                float pwmValue;
                if (!float.TryParse(pwm, out pwmValue))
                    return;
                if (!(pwmValue > 0) || !(pwmValue <= 100))
                    return;
                try
                {
                    ushort addrUshortValue;
                    if (ushort.TryParse(addr, out addrUshortValue))
                    {
                        if (addrUshortValue > 15)
                            return;

                        var findDevIndex = _devList.FindIndex(f => f.DevAddr == addrUshortValue);
                        if (findDevIndex == -1)
                            return;
                        _devList[findDevIndex].PwmDuty[ledLedIndex] = pwmValue;
                    }
                    else if (addr.Length == 4)
                    {
                        var cs0 = addr.Substring(3, 1);
                        var cs1 = addr.Substring(2, 1);
                        var cs2 = addr.Substring(1, 1);
                        var cs3 = addr.Substring(0, 1);

                        var devAddr = Convert.ToUInt16(string.Format("0000{0}{1}{2}{3}", cs3, cs2, cs1, cs0), 2);

                        if (addrUshortValue > 15)
                            return;
                        var findDevIndex = _devList.FindIndex(f => f.DevAddr == devAddr);
                        if (findDevIndex == -1)
                            return;
                        _devList[findDevIndex].PwmDuty[ledLedIndex] = pwmValue;
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        [Description("设置目标芯片的目标通道的DC电流值")]
        public void DimSet(string addr, string chIndex, string dc)
        {
            lock (_lockDevList)
            {
                if (!_devList.Any())
                    return;

                int ledLedIndex;
                if (!int.TryParse(chIndex, out ledLedIndex))
                    return;
                if (ledLedIndex < 0 || ledLedIndex > 24)
                    return;

                float dcValue;
                if (!float.TryParse(dc, out dcValue))
                    return;

                if (Math.Abs(dcValue - 3.75) < 0.1 || Math.Abs(dcValue - 7.5) < 0.1 ||
                    Math.Abs(dcValue - 11.25) < 0.1 || Math.Abs(dcValue - 15) < 0.1 ||
                    Math.Abs(dcValue - 18.75) < 0.1 || Math.Abs(dcValue - 22.50) < 0.1 ||
                    Math.Abs(dcValue - 26.25) < 0.1 || Math.Abs(dcValue - 30.00) < 0.1 ||
                    Math.Abs(dcValue - 33.75) < 0.1 || Math.Abs(dcValue - 37.50) < 0.1 ||
                    Math.Abs(dcValue - 41.25) < 0.1 || Math.Abs(dcValue - 45) < 0.1 ||
                    Math.Abs(dcValue - 48.75) < 0.1 || Math.Abs(dcValue - 52.5) < 0.1 ||
                    Math.Abs(dcValue - 56.25) < 0.1 || Math.Abs(dcValue - 60) < 0.1)
                {
                    var dcdim = (byte)0x00;
                    if (Math.Abs(dcValue - 3.75) < 0.1)
                        dcdim = 0x00;
                    else if (Math.Abs(dcValue - 7.5) < 0.1)
                        dcdim = 0x01;
                    else if (Math.Abs(dcValue - 11.25) < 0.1)
                        dcdim = 0x02;
                    else if (Math.Abs(dcValue - 15) < 0.1)
                        dcdim = 0x03;
                    else if (Math.Abs(dcValue - 18.75) < 0.1)
                        dcdim = 0x04;
                    else if (Math.Abs(dcValue - 22.5) < 0.1)
                        dcdim = 0x05;
                    else if (Math.Abs(dcValue - 26.25) < 0.1)
                        dcdim = 0x06;
                    else if (Math.Abs(dcValue - 30) < 0.1)
                        dcdim = 0x07;
                    else if (Math.Abs(dcValue - 33.75) < 0.1)
                        dcdim = 0x08;
                    else if (Math.Abs(dcValue - 37.5) < 0.1)
                        dcdim = 0x09;
                    else if (Math.Abs(dcValue - 41.25) < 0.1)
                        dcdim = 0x0A;
                    else if (Math.Abs(dcValue - 45) < 0.1)
                        dcdim = 0x0B;
                    else if (Math.Abs(dcValue - 48.75) < 0.1)
                        dcdim = 0x0C;
                    else if (Math.Abs(dcValue - 52.5) < 0.1)
                        dcdim = 0x0D;
                    else if (Math.Abs(dcValue - 56.25) < 0.1)
                        dcdim = 0x0E;
                    else if (Math.Abs(dcValue - 60) < 0.1)
                        dcdim = 0x0F;

                    try
                    {
                        ushort addrUshortValue;
                        if (ushort.TryParse(addr, out addrUshortValue))
                        {
                            if (addrUshortValue > 15)
                                return;

                            var findDevIndex = _devList.FindIndex(f => f.DevAddr == addrUshortValue);
                            if (findDevIndex == -1)
                                return;
                            _devList[findDevIndex].DimSet[ledLedIndex] = dcdim;
                        }
                        else if (addr.Length == 4)
                        {
                            var cs0 = addr.Substring(3, 1);
                            var cs1 = addr.Substring(2, 1);
                            var cs2 = addr.Substring(1, 1);
                            var cs3 = addr.Substring(0, 1);

                            var devAddr = Convert.ToUInt16(string.Format("0000{0}{1}{2}{3}", cs3, cs2, cs1, cs0), 2);

                            if (addrUshortValue > 15)
                                return;
                            var findDevIndex = _devList.FindIndex(f => f.DevAddr == devAddr);
                            if (findDevIndex == -1)
                                return;
                            _devList[findDevIndex].DimSet[ledLedIndex] = dcdim;
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
        }

        [Description("打开目标芯片的目标通道")]
        public void ChannelOn(string devAddr, string chIndex)
        {
            SwitchLed(devAddr, chIndex, true);
        }

        [Description("关闭目标芯片的目标通道")]
        public void ChannelOff(string devAddr, string chIndex)
        {
            SwitchLed(devAddr, chIndex, false);
        }

        [Description("打开目标芯片的所有通道")]
        public void ChannelAllOn(string devAddr)
        {
            for (int i = 1; i < 25; i++)
            {
                SwitchLed(devAddr, i.ToString(), true);
            }
        }

        [Description("关闭目标芯片的所有通道")]
        public void ChannelAllOff(string devAddr)
        {
            for (int i = 1; i < 25; i++)
            {
                SwitchLed(devAddr, i.ToString(), false);
            }
        }

        [Description("打开目标芯片的单数通道")]
        public void ChannelOddOn(string devAddr)
        {
            for (int i = 1; i < 25; i++)
            {
                if (i % 2 != 0)
                {
                    SwitchLed(devAddr, i.ToString(), true);
                }
            }
        }

        [Description("关闭目标芯片的双数通道")]
        public void ChannelEvenOn(string devAddr)
        {
            for (int i = 1; i < 25; i++)
            {
                if (i % 2 == 0)
                {
                    SwitchLed(devAddr, i.ToString(), true);
                }
            }
        }

        private void SwitchLed(string addr, string chIndex, bool isOn)
        {
            lock (_lockDevList)
            {
                if (!_devList.Any())
                    return;

                int ledLedIndex;
                if (!int.TryParse(chIndex, out ledLedIndex))
                    return;
                if (ledLedIndex < 0 || ledLedIndex > 24)
                    return;

                try
                {
                    ushort addrUshortValue;
                    if (ushort.TryParse(addr, out addrUshortValue))
                    {
                        if (addrUshortValue > 15)
                            return;

                        var findDevIndex = _devList.FindIndex(f => f.DevAddr == addrUshortValue);
                        if (findDevIndex == -1)
                            return;
                        _devList[findDevIndex].ChannelEnable[ledLedIndex] = isOn ? 1.ToString() : 0.ToString();
                    }
                    else if (addr.Length == 4)
                    {
                        var cs0 = addr.Substring(3, 1);
                        var cs1 = addr.Substring(2, 1);
                        var cs2 = addr.Substring(1, 1);
                        var cs3 = addr.Substring(0, 1);

                        var devAddr = Convert.ToUInt16(string.Format("0000{0}{1}{2}{3}", cs3, cs2, cs1, cs0), 2);

                        if (addrUshortValue > 15)
                            return;
                        var findDevIndex = _devList.FindIndex(f => f.DevAddr == devAddr);
                        if (findDevIndex == -1)
                            return;
                        _devList[findDevIndex].ChannelEnable[ledLedIndex] = isOn ? 1.ToString() : 0.ToString();
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        /// <summary>
        /// Disable Lmp
        /// </summary>
        //private void BroadcastExitLmp()
        //{
        //    WriteReg(0x00, new byte[] { 0x01 }, true);
        //    //WriteReg(0x52, new byte[] { 0x52 }, true);
        //    WriteReg(0x5E, new byte[] { 0x00 }, true);
        //}
        private void SysInit(Dev dev)
        {
            WriteReg(0x07, new byte[] { 0x14, 0x0B }, dev: dev);
            //Thread.Sleep(1);
            WriteReg(0x17, new byte[] { 0x05 }, dev: dev);
            //Thread.Sleep(1);
            WriteReg(0x00, new byte[] { IsDcMode ? (byte)0x3E : (byte)0x26, 0x01 }, dev: dev);
            //Thread.Sleep(1);
            //WriteReg(0x24, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF },
            //    dev: dev);
            //Thread.Sleep(1);
            //WriteReg(0x18, new byte[12]);
            //Thread.Sleep(1);


            //WriteReg(0x07, new byte[] { 0x14, 0x0B }, dev: dev);
            //Thread.Sleep(1);
            //WriteReg(0x17, new byte[] { 0x05 }, dev: dev);
            //Thread.Sleep(1);
            //WriteReg(0x01, new byte[] { 0x01 }, dev: dev);
            //Thread.Sleep(1);
            //WriteReg(0x24, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, dev: dev);
            ////Thread.Sleep(1);
            ////WriteReg(0x18, new byte[12]);
            //Thread.Sleep(1);
            //WriteReg(0x00, new byte[] { 0x26 }, dev: dev);
            //Thread.Sleep(1);
        }

        private void ReadReg(Dev dev, byte startAddr, byte len)
        {
            var sendBytes = new List<byte>();

            var str = string.Format(
                "{0}{1}{2}{3}{4}{5}{6}{7}",
                1, 0, 0, 0, dev.Cs3Setting, dev.Cs2Setting, dev.Cs1Setting, dev.Cs0Setting);
            sendBytes.Add(Convert.ToByte(str, 2)); // Device address, Broadcast, Write / Read
            sendBytes.Add(len); // Number of Data
            sendBytes.Add(startAddr); // Register Address
            SendDatas(sendBytes.ToArray());
        }

        private void WriteReg(
            byte addr, IReadOnlyCollection<byte> values, bool isBroadcast = false, Dev dev = null)
        {
            var sendBytes = new List<byte>();

            var str = string.Empty;
            if (!isBroadcast)
            {
                if (dev != null)
                {
                    str = string.Format(
                        "{0}{1}{2}{3}{4}{5}{6}{7}",
                        0, 0, 0, 0, dev.Cs3Setting, dev.Cs2Setting, dev.Cs1Setting, dev.Cs0Setting);
                }
            }
            else
                str = "01000000";

            if (string.IsNullOrEmpty(str))
                return;

            sendBytes.Add(Convert.ToByte(str, 2)); // Device address, Broadcast, Write / Read
            sendBytes.Add((byte)values.Count); // Number of Data
            sendBytes.Add(addr); // Register Address

            var dataBytes = new List<byte>();
            dataBytes.AddRange(values);

            sendBytes.AddRange(dataBytes); // Data
            SendDatas(sendBytes.ToArray());
        }

        private void SendDatas(IReadOnlyList<byte> datas)
        {
            if (MySerialPort == null || datas == null)
                return;
            var sendBytes = new List<byte>();

            sendBytes.AddRange(datas);
            sendBytes.AddRange(GetCrc(datas).Reverse());
            sendBytes.Insert(0, 0x55);
            MySerialPort.SendCommand(sendBytes.ToArray(), 5);
        }

        #region 计算CRC相关

        private static IEnumerable<byte> GetCrc(IEnumerable<byte> toCrcBytes)
        {
            var returnCrc = (uint)0x00;
            foreach (var t in toCrcBytes)
            {
                var crcInfo = new CrcHelper.CrcInfo
                {
                    Width = 16,
                    Poly = 0x8005,
                    InitReg = returnCrc,
                    Refin = true,
                    Refout = false,
                    Xorout = 0x00
                };
                CRC_Table_Init(crcInfo);

                returnCrc = CALC_CRC(crcInfo, new[] { t });
            }

            var returnCrcBytes = BitConverter.GetBytes(returnCrc);
            return new byte[] { returnCrcBytes[1], returnCrcBytes[0] };
        }

        private static readonly uint[] Table = new uint[256];

        private static void CRC_Table_Init(CrcHelper.CrcInfo crcInfo)
        {
            uint poly, value;
            var validBits = ((uint)2 << (crcInfo.Width - 1)) - 1;

            if (crcInfo.Refin)
            {
                poly = BitReflected(crcInfo.Poly, crcInfo.Width);
                for (uint i = 0; i < 256; i++)
                {
                    value = i;
                    for (uint j = 0; j < 8; j++)
                    {
                        if ((value & 0x0001) != 0)
                            value = (value >> 1) ^ poly;
                        else
                            value >>= 1;
                    }

                    Table[i] = value & validBits;
                }
            }
            else
            {
                poly = crcInfo.Width < 8 ? crcInfo.Poly << (8 - crcInfo.Width) : crcInfo.Poly;
                var bit = crcInfo.Width > 8 ? (uint)1 << (crcInfo.Width - 1) : 0x80;

                for (uint i = 0; i < 256; i++)
                {
                    value = crcInfo.Width > 8 ? i << (crcInfo.Width - 8) : i;

                    for (uint j = 0; j < 8; j++)
                    {
                        if ((value & bit) != 0)
                            value = (value << 1) ^ poly;
                        else
                            value <<= 1;
                    }

                    Table[i] = value & (crcInfo.Width < 8 ? 0xff : validBits);
                }
            }
        }

        private static uint BitReflected(uint input, byte bits)
        {
            uint res = 0;
            while (bits-- > 0)
            {
                res <<= 1;
                if ((input & 0x01) != 0)
                    res |= 1;
                input >>= 1;
            }

            return res;
        }

        private static uint CALC_CRC(CrcHelper.CrcInfo info, IReadOnlyList<byte> memBlock)
        {
            var memBlockLen = (uint)memBlock.Count;
            uint value;

            if (info.Refin)
            {
                value = BitReflected(info.InitReg, info.Width);
                if (info.Width > 8)
                {
                    var i = 0;
                    while (memBlockLen-- != 0)
                    {
                        value = (value >> 8) ^ Table[value & 0xff ^ memBlock[i++]];
                    }
                }
                else
                {
                    var i = 0;
                    while (memBlockLen-- != 0)
                    {
                        value = Table[value ^ memBlock[i++]];
                    }
                }
            }
            else
            {
                if (info.Width > 8)
                {
                    value = info.InitReg;
                    var i = 0;
                    while (memBlockLen-- != 0)
                    {
                        var high = (byte)(value >> (info.Width - 8));
                        value = (value << 8) ^ Table[high ^ memBlock[i++]];
                    }
                }
                else
                {
                    value = info.InitReg << (8 - info.Width);
                    var i = 0;
                    while (memBlockLen-- != 0)
                    {
                        value = Table[value ^ memBlock[i++]];
                    }

                    value >>= 8 - info.Width;
                }
            }

            if (info.Refout != info.Refin)
            {
                value = BitReflected(value, info.Width);
            }

            value ^= info.Xorout;
            return value & (((uint)2 << (info.Width - 1)) - 1);
        }

        #endregion

        #region Internal

        internal class Dev
        {
            public ushort DevAddr { get; set; }

            public string Cs0Setting { get; set; }
            public string Cs1Setting { get; set; }
            public string Cs2Setting { get; set; }
            public string Cs3Setting { get; set; }

            public Action CmdAction { get; set; }

            public Dictionary<int, float> PwmDuty = new Dictionary<int, float>();

            public Dictionary<int, byte> DimSet = new Dictionary<int, byte>();

            public Dictionary<int, string> ChannelEnable = new Dictionary<int, string>();

            public Dev()
            {
                for (var i = 1; i <= 24; i++)
                {
                    PwmDuty.Add(i, 100f);
                    DimSet.Add(i, 0x00);
                    ChannelEnable.Add(i, "0");
                }
            }
        }

        #endregion

        #region public fields - status

        [Description("R,status of LED short error for LED1 to LED24 of Dev-0")]
        public string Dev0Led1To24ShortErrorStatus;

        [Description("R,status of LED short error for LED1 to LED24 of Dev-1")]
        public string Dev1Led1To24ShortErrorStatus;

        [Description("R,status of LED short error for LED1 to LED24 of Dev-2")]
        public string Dev2Led1To24ShortErrorStatus;

        [Description("R,status of LED short error for LED1 to LED24 of Dev-3")]
        public string Dev3Led1To24ShortErrorStatus;

        [Description("R,status of LED short error for LED1 to LED24 of Dev-4")]
        public string Dev4Led1To24ShortErrorStatus;

        [Description("R,status of LED short error for LED1 to LED24 of Dev-5")]
        public string Dev5Led1To24ShortErrorStatus;

        [Description("R,status of LED short error for LED1 to LED24 of Dev-6")]
        public string Dev6Led1To24ShortErrorStatus;

        [Description("R,status of LED short error for LED1 to LED24 of Dev-7")]
        public string Dev7Led1To24ShortErrorStatus;

        [Description("R,status of LED short error for LED1 to LED24 of Dev-8")]
        public string Dev8Led1To24ShortErrorStatus;

        [Description("R,status of LED short error for LED1 to LED24 of Dev-9")]
        public string Dev9Led1To24ShortErrorStatus;

        [Description("R,status of LED short error for LED1 to LED24 of Dev-10")]
        public string Dev10Led1To24ShortErrorStatus;

        [Description("R,status of LED short error for LED1 to LED24 of Dev-11")]
        public string Dev11Led1To24ShortErrorStatus;

        [Description("R,status of LED short error for LED1 to LED24 of Dev-12")]
        public string Dev12Led1To24ShortErrorStatus;

        [Description("R,status of LED short error for LED1 to LED24 of Dev-13")]
        public string Dev13Led1To24ShortErrorStatus;

        [Description("R,status of LED short error for LED1 to LED24 of Dev-14")]
        public string Dev14Led1To24ShortErrorStatus;

        [Description("R,status of LED short error for LED1 to LED24 of Dev-15")]
        public string Dev15Led1To24ShortErrorStatus;

        [Description("R,status of LED open error for LED1 to LED24 of Dev-0")]
        public string Dev0Led1To24OpenErrorStatus;

        [Description("R,status of LED open error for LED1 to LED24 of Dev-1")]
        public string Dev1Led1To24OpenErrorStatus;

        [Description("R,status of LED open error for LED1 to LED24 of Dev-2")]
        public string Dev2Led1To24OpenErrorStatus;

        [Description("R,status of LED open error for LED1 to LED24 of Dev-3")]
        public string Dev3Led1To24OpenErrorStatus;

        [Description("R,status of LED open error for LED1 to LED24 of Dev-4")]
        public string Dev4Led1To24OpenErrorStatus;

        [Description("R,status of LED open error for LED1 to LED24 of Dev-5")]
        public string Dev5Led1To24OpenErrorStatus;

        [Description("R,status of LED open error for LED1 to LED24 of Dev-6")]
        public string Dev6Led1To24OpenErrorStatus;

        [Description("R,status of LED open error for LED1 to LED24 of Dev-7")]
        public string Dev7Led1To24OpenErrorStatus;

        [Description("R,status of LED open error for LED1 to LED24 of Dev-8")]
        public string Dev8Led1To24OpenErrorStatus;

        [Description("R,status of LED open error for LED1 to LED24 of Dev-9")]
        public string Dev9Led1To24OpenErrorStatus;

        [Description("R,status of LED open error for LED1 to LED24 of Dev-10")]
        public string Dev10Led1To24OpenErrorStatus;

        [Description("R,status of LED open error for LED1 to LED24 of Dev-11")]
        public string Dev11Led1To24OpenErrorStatus;

        [Description("R,status of LED open error for LED1 to LED24 of Dev-12")]
        public string Dev12Led1To24OpenErrorStatus;

        [Description("R,status of LED open error for LED1 to LED24 of Dev-13")]
        public string Dev13Led1To24OpenErrorStatus;

        [Description("R,status of LED open error for LED1 to LED24 of Dev-14")]
        public string Dev14Led1To24OpenErrorStatus;

        [Description("R,status of LED open error for LED1 to LED24 of Dev-15")]
        public string Dev15Led1To24OpenErrorStatus;

        #endregion

        #region foreach ReadErrors

        [Description("R,循环读取ShortError")] public string ForeachReadShortErrors;

        [Description("R,循环读取OpenError")] public string ForeachReadOpenErrors;

        [Description("循环读取Errors")]
        public void ForeachReadErrors(string devAddr, string delayMs)
        {
            ForeachReadShortErrors = string.Empty;
            ForeachReadOpenErrors = string.Empty;

            lock (_lockDevList)
            {
                if (!_devList.Any())
                    return;
            }

            ushort addrUshortValue;
            if (!ushort.TryParse(devAddr, out addrUshortValue))
                return;
            if (addrUshortValue > 15)
                return;

            lock (_lockDevList)
            {
                if (_devList.FindIndex(f => f.DevAddr == addrUshortValue) == -1)
                    return;

                foreach (var d in _devList)
                {
                    for (var i = 1; i < 25; i++)
                    {
                        ChannelOff(d.DevAddr.ToString(), i.ToString());
                    }
                }
            }

            int ms;
            if (!int.TryParse(delayMs, out ms))
                return;

            {
                if (ms <= 0 || ms > 1000)
                {
                    ms = 1000;
                }

                for (var i = 1; i <= 24; i++)
                    ChannelOff(addrUshortValue.ToString(), i.ToString());

                var dicOpenErrorResult = new Dictionary<ushort, string> { { addrUshortValue, string.Empty } };
                var dicShortErrorResult = new Dictionary<ushort, string> { { addrUshortValue, string.Empty } };

                for (var i = 1; i <= 24; i++)
                {
                    ChannelOn(addrUshortValue.ToString(), i.ToString());

                    Thread.Sleep(ms);

                    ReadStatus(addrUshortValue.ToString());
                    var shortStatus = string.Format("Dev{0}Led1To24ShortErrorStatus", addrUshortValue);
                    var openStatus = string.Format("Dev{0}Led1To24OpenErrorStatus", addrUshortValue);

                    if (GetType().GetField(openStatus) != null && GetType().GetField(shortStatus) != null)
                    {
                        try
                        {
                            var openError = GetType().GetField(openStatus).GetValue(this).ToString()
                                .Substring(i - 1, 1);
                            var shortError = GetType().GetField(shortStatus).GetValue(this).ToString()
                                .Substring(i - 1, 1);
                            dicOpenErrorResult[addrUshortValue] += openError;
                            dicShortErrorResult[addrUshortValue] += shortError;

                            //stropenError += string.Format("Dev{0},CH{1},{2};", d.DevAddr, i, openError);
                            //strshortError += string.Format("Dev{0},CH{1},{2};", d.DevAddr, i, shortError);
                        }
                        catch (Exception)
                        {
                            ForeachReadShortErrors = string.Empty;
                            ForeachReadOpenErrors = string.Empty;

                            for (var j = 1; j <= 24; j++)
                                ChannelOff(addrUshortValue.ToString(), j.ToString());
                            return;
                        }
                    }

                    ChannelOff(addrUshortValue.ToString(), i.ToString());
                }

                ForeachReadShortErrors +=
                    string.Format("Dev{0}:{1};", addrUshortValue, dicShortErrorResult[addrUshortValue]);
                ForeachReadOpenErrors +=
                    string.Format("Dev{0}:{1};", addrUshortValue, dicOpenErrorResult[addrUshortValue]);
            }
        }

        #endregion
    }
}