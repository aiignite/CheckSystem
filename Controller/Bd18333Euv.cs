using CommonUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime;
using System.Threading;
using static Controller.S59HeadLampBox;

namespace Controller
{
    public sealed class Bd18333Euv : ControllerBase
    {
        public MySerialPort UartCan;
        public bool IsDcMode;

        public Bd18333Euv(string name) : base(name) => MainWork();

        ~Bd18333Euv() => Dispose();

        #region 通道控制

        /// <summary>
        /// 初始化地址
        /// We can set from “0000b” to “1111b”
        /// </summary>
        /// <param name="addr"></param>
        [Description("添加一个地址的芯片")]
        public void AddDev(string addr)
        {
            _deviceEnumerationSemaphore.Wait();

            try
            {
                if (ushort.TryParse(addr, out var addrUshortValue) && addr.Length != 4)
                {
                    if (addrUshortValue > 15)
                        return;
                    var index = _devList.FindIndex(f => f.DevAddr == addrUshortValue);
                    if (index is -1)
                        return;

                    _devList[index].IsOnline = true;
                }
                else if (addr.Length == 4)
                {
                    var cs0 = addr.Substring(3, 1);
                    var cs1 = addr.Substring(2, 1);
                    var cs2 = addr.Substring(1, 1);
                    var cs3 = addr.Substring(0, 1);

                    var devAddr = Convert.ToUInt16($"0000{cs3}{cs2}{cs1}{cs0}", 2);

                    if (devAddr > 15)
                        return;

                    var index = _devList.FindIndex(f => f.DevAddr == devAddr);
                    if (index is -1)
                        return;
                    _devList[index].IsOnline = true;
                }
            }
            catch (Exception)
            {
                // ignored
            }
            finally
            {
                _deviceEnumerationSemaphore.Release();
            }
        }

        [Description("设置目标芯片的目标通道的PWM")]
        public void SetPwmDuty(string addr, string chIndex, string pwm)
        {
            _deviceEnumerationSemaphore.Wait();

            try
            {
                if (!_devList.Any())
                    return;

                if (!int.TryParse(chIndex, out var ledLedIndex))
                    return;
                if (ledLedIndex < 0 || ledLedIndex > 24)
                    return;

                if (!float.TryParse(pwm, out var pwmValue))
                    return;
                if (!(pwmValue > 0) || !(pwmValue <= 100))
                    return;

                if (ushort.TryParse(addr, out var addrUshortValue))
                {
                    if (addrUshortValue > 15)
                        return;

                    var findDevIndex = _devList.FindIndex(f => f.DevAddr == addrUshortValue);
                    if (findDevIndex == -1)
                        return;
                    _devList[findDevIndex].PwmSet(ledLedIndex, pwmValue);
                }
                else if (addr.Length == 4)
                {
                    var cs0 = addr.Substring(3, 1);
                    var cs1 = addr.Substring(2, 1);
                    var cs2 = addr.Substring(1, 1);
                    var cs3 = addr.Substring(0, 1);
                    var devAddr = Convert.ToUInt16($"0000{cs3}{cs2}{cs1}{cs0}", 2);

                    if (addrUshortValue > 15)
                        return;
                    var findDevIndex = _devList.FindIndex(f => f.DevAddr == devAddr);
                    if (findDevIndex == -1)
                        return;
                    _devList[findDevIndex].PwmSet(ledLedIndex, pwmValue);
                }
            }
            catch (Exception)
            {
                // ignored
            }
            finally
            {
                _deviceEnumerationSemaphore.Release();
            }
        }

        [Description("设置目标芯片的目标通道在PWM模式下的电流")]
        public void SetPwmCurr(string addr, string chIndex, string curr)
        {
            _deviceEnumerationSemaphore.Wait();

            try
            {
                if (!_devList.Any())
                    return;

                if (!int.TryParse(chIndex, out var ledLedIndex))
                    return;
                if (ledLedIndex < 0 || ledLedIndex > 24)
                    return;

                if (!byte.TryParse(curr, out var dcValue))
                    return;

                if (ushort.TryParse(addr, out var addrUshortValue))
                {
                    if (addrUshortValue > 15)
                        return;

                    var findDevIndex = _devList.FindIndex(f => f.DevAddr == addrUshortValue);
                    if (findDevIndex == -1)
                        return;
                    _devList[findDevIndex].DcDimSetValue(ledLedIndex, dcValue);
                }
                else if (addr.Length == 4)
                {
                    var cs0 = addr.Substring(3, 1);
                    var cs1 = addr.Substring(2, 1);
                    var cs2 = addr.Substring(1, 1);
                    var cs3 = addr.Substring(0, 1);
                    var devAddr = Convert.ToUInt16($"0000{cs3}{cs2}{cs1}{cs0}", 2);

                    if (addrUshortValue > 15)
                        return;
                    var findDevIndex = _devList.FindIndex(f => f.DevAddr == devAddr);
                    if (findDevIndex == -1)
                        return;
                    _devList[findDevIndex].DcDimSetValue(ledLedIndex, dcValue);
                }
            }
            catch (Exception e)
            {
                // ignored
            }
            finally
            {
                _deviceEnumerationSemaphore.Release();
            }
        }

        [Description("设置目标芯片的目标通道的DC电流值")]
        public void DimSet(string addr, string chIndex, string dc)
        {
            _deviceEnumerationSemaphore.Wait();

            try
            {
                if (!_devList.Any())
                    return;

                if (!int.TryParse(chIndex, out var ledLedIndex))
                    return;
                if (ledLedIndex < 0 || ledLedIndex > 24)
                    return;

                if (!byte.TryParse(dc, out var dcValue))
                    return;

                //if (Math.Abs(dcValue - 3.75) < 0.1 || Math.Abs(dcValue - 7.5) < 0.1 ||
                //    Math.Abs(dcValue - 11.25) < 0.1 || Math.Abs(dcValue - 15) < 0.1 ||
                //    Math.Abs(dcValue - 18.75) < 0.1 || Math.Abs(dcValue - 22.50) < 0.1 ||
                //    Math.Abs(dcValue - 26.25) < 0.1 || Math.Abs(dcValue - 30.00) < 0.1 ||
                //    Math.Abs(dcValue - 33.75) < 0.1 || Math.Abs(dcValue - 37.50) < 0.1 ||
                //    Math.Abs(dcValue - 41.25) < 0.1 || Math.Abs(dcValue - 45) < 0.1 ||
                //    Math.Abs(dcValue - 48.75) < 0.1 || Math.Abs(dcValue - 52.5) < 0.1 ||
                //    Math.Abs(dcValue - 56.25) < 0.1 || Math.Abs(dcValue - 60) < 0.1)

                if (ushort.TryParse(addr, out var addrUshortValue))
                {
                    if (addrUshortValue > 15)
                        return;

                    var findDevIndex = _devList.FindIndex(f => f.DevAddr == addrUshortValue);
                    if (findDevIndex == -1)
                        return;
                    _devList[findDevIndex].DimSetValue(ledLedIndex, dcValue);
                }
                else if (addr.Length == 4)
                {
                    var cs0 = addr.Substring(3, 1);
                    var cs1 = addr.Substring(2, 1);
                    var cs2 = addr.Substring(1, 1);
                    var cs3 = addr.Substring(0, 1);
                    var devAddr = Convert.ToUInt16($"0000{cs3}{cs2}{cs1}{cs0}", 2);

                    if (addrUshortValue > 15)
                        return;
                    var findDevIndex = _devList.FindIndex(f => f.DevAddr == devAddr);
                    if (findDevIndex == -1)
                        return;
                    _devList[findDevIndex].DimSetValue(ledLedIndex, dcValue);
                }
            }
            catch (Exception e)
            {
                // ignored
            }
            finally
            {
                _deviceEnumerationSemaphore.Release();
            }
        }

        [Description("打开目标芯片的目标通道")]
        public void ChannelOn(string devAddr, string chIndex) => SwitchLed(devAddr, chIndex, true);

        [Description("关闭目标芯片的目标通道")]
        public void ChannelOff(string devAddr, string chIndex) => SwitchLed(devAddr, chIndex, false);

        [Description("打开目标芯片的所有通道")]
        public void ChannelAllOn(string devAddr)
        {
            for (var i = 1; i < 25; i++)
                SwitchLed(devAddr, i.ToString(), true);
        }

        [Description("关闭目标芯片的所有通道")]
        public void ChannelAllOff(string devAddr)
        {
            for (var i = 1; i < 25; i++)
                SwitchLed(devAddr, i.ToString(), false);
        }

        [Description("打开目标芯片的单数通道")]
        public void ChannelOddOn(string devAddr)
        {
            for (var i = 1; i < 25; i++)
                if (i % 2 != 0)
                    SwitchLed(devAddr, i.ToString(), true);
        }

        [Description("关闭目标芯片的双数通道")]
        public void ChannelEvenOn(string devAddr)
        {
            for (var i = 1; i < 25; i++)
                if (i % 2 == 0)
                    SwitchLed(devAddr, i.ToString(), true);
        }

        private void SwitchLed(string addr, string chIndex, bool isOn)
        {
            _deviceEnumerationSemaphore.Wait();

            try
            {
                if (!_devList.Any())
                    return;

                if (!int.TryParse(chIndex, out var ledLedIndex))
                    return;
                if (ledLedIndex < 0 || ledLedIndex > 24)
                    return;

                if (ushort.TryParse(addr, out var addrUshortValue))
                {
                    if (addrUshortValue > 15)
                        return;

                    var findDevIndex = _devList.FindIndex(f => f.DevAddr == addrUshortValue);
                    if (findDevIndex == -1)
                        return;
                    _devList[findDevIndex].ChannelSet(ledLedIndex, isOn);
                }
                else if (addr.Length == 4)
                {
                    var cs0 = addr.Substring(3, 1);
                    var cs1 = addr.Substring(2, 1);
                    var cs2 = addr.Substring(1, 1);
                    var cs3 = addr.Substring(0, 1);

                    var devAddr = Convert.ToUInt16($"0000{cs3}{cs2}{cs1}{cs0}", 2);

                    if (addrUshortValue > 15)
                        return;
                    var findDevIndex = _devList.FindIndex(f => f.DevAddr == devAddr);
                    if (findDevIndex == -1)
                        return;
                    _devList[findDevIndex].ChannelSet(ledLedIndex, isOn);
                }
            }
            catch (Exception)
            {
                // ignored
            }
            finally
            {
                _deviceEnumerationSemaphore.Release();
            }
        }

        #endregion

        #region 周期帧

        private void MainWork()
        {
            for (ushort i = 0; i < 15; i++)
            {
                var bits = Convert.ToString(i, 2).PadLeft(8, '0');
                _devList.Add(new Dev
                {
                    DevAddr = i,
                    Cs0Setting = bits.Substring(7, 1),
                    Cs1Setting = bits.Substring(6, 1),
                    Cs2Setting = bits.Substring(5, 1),
                    Cs3Setting = bits.Substring(4, 1)
                });

                SingleDevInitCtrl(i);
                SingleDevLedEnCtrl(i);
                SingleDevLedParaCtrl(i);
            }
            SchedulerAsync();
            MySerialPort.PushSciMsg += MySerialPort_PushSciMsg;
        }

        private void MySerialPort_PushSciMsg(string name, byte[] datas)
        {
            if (UartCan != null && UartCan.Name == name && datas != null && datas.Any())
            {
                Console.WriteLine(ValueHelper.GetHextStrWithOx(datas));
            }
        }

        private void SingleDevInitCtrl(int id)
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = SendUartCanMsg(() =>
                {
                    _deviceEnumerationSemaphore.Wait();

                    try
                    {
                        var slaveIndex = _devList.FindIndex(f => f.DevAddr == id && f.IsOnline);
                        if (slaveIndex == -1)
                            return null;

                        var dev = _devList[slaveIndex];
                        var rtb = new List<byte[]>
                        {
                            WriteReg(0x07, new byte[] { 0x14, 0x0B }, dev: dev),
                            WriteReg(0x17, new byte[] { 0x05 }, dev: dev),
                            WriteReg(0x00, new byte[] { IsDcMode ? (byte)0x3E : (byte)0x26, 0x01 }, dev: dev)
                        };

                        return rtb;
                    }
                    catch (Exception e)
                    {
                        return null;
                    }
                    finally
                    {
                        _deviceEnumerationSemaphore.Release();
                    }
                }),
                Interval = 500
            });
        }

        private void SingleDevLedEnCtrl(int id)
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = SendUartCanMsg(() =>
                {
                    _deviceEnumerationSemaphore.Wait();

                    try
                    {
                        var slaveIndex = _devList.FindIndex(f => f.DevAddr == id && f.IsOnline);
                        if (slaveIndex == -1)
                            return null;

                        var dev = _devList[slaveIndex];
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
                        return new List<byte[]>
                            { WriteReg(0x04, new[] { led1Toled8Byte, led9Toled16Byte, led17Toled24Byte }, dev: dev) };
                    }
                    catch (Exception e)
                    {
                        return null;
                    }
                    finally
                    {
                        _deviceEnumerationSemaphore.Release();
                    }
                }),
                Interval = 50
            });
        }

        private void SingleDevLedParaCtrl(int id)
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = SendUartCanMsg(() =>
                {
                    _deviceEnumerationSemaphore.Wait();

                    try
                    {
                        var slaveIndex = _devList.FindIndex(f => f.DevAddr == id && f.IsOnline);
                        if (slaveIndex == -1)
                            return null;

                        var dev = _devList[slaveIndex];
                        var regData1 = new List<byte>();
                        //var regData2 = new List<byte>();
                        var regData3 = new List<byte>();

                        #region DCDIM

                        {
                            var dcSettings = new List<byte>();
                            for (var j = 1; j <= 24; j = j + 2)
                            {
                                var hi = dev.DcDim[j + 1];
                                var lo = dev.DcDim[j];

                                var bitHi = Convert.ToString(hi, 2).PadLeft(8, '0').Substring(4, 4);
                                var bitLo = Convert.ToString(lo, 2).PadLeft(8, '0').Substring(4, 4);
                                var bits = string.Format("{0}{1}", bitHi, bitLo);
                                var b = Convert.ToByte(bits, 2);
                                dcSettings.Add(b);
                            }

                            regData1.AddRange(dcSettings);
                        }

                        #endregion

                        #region PWMOUT

                        regData1.AddRange(new byte[] { 0xFF, 0xFF, 0xFF });

                        #endregion

                        #region DIMSET

                        if (IsDcMode)
                        {
                            var dimset = new List<byte>();

                            for (var j = 1; j <= 24; j++)
                            {
                                var dismsetvalue = dev.DimSet[j];
                                dimset.Add(dismsetvalue);
                            }

                            regData3.AddRange(dimset);
                        }
                        else
                        {
                            var pwmSettings = new List<byte>();

                            for (var j = 1; j <= 24; j++)
                            {
                                var pwmDuty = dev.PwmDuty[j];
                                pwmSettings.Add((byte)(Math.Round(pwmDuty / 100 * 256, 0) - 1));
                            }

                            regData3.AddRange(pwmSettings);
                        }

                        #endregion

                        var rtb = new List<byte[]>
                        {
                            WriteReg(0x24, regData1.ToArray(), dev: dev),
                            //WriteReg(0x30, regData2.ToArray(), dev: dev),
                            WriteReg(0x33, regData3.ToArray(), dev: dev)
                        };

                        return rtb;
                    }
                    catch (Exception e)
                    {
                        return null;
                    }
                    finally
                    {
                        _deviceEnumerationSemaphore.Release();
                    }
                }),
                Interval = 50
            });
        }

        private Action SendUartCanMsg(Func<IReadOnlyCollection<byte[]>> data) => () => SendMsg(data);

        private async void SendMsg(Func<IReadOnlyCollection<byte[]>> data)
        {
            await _msgEnumerationSemaphore.WaitAsync();

            try
            {
                if (data == null)
                    return;

                var msg = data.Invoke();

                if (UartCan is null || msg is null)
                    return;

                foreach (var t in msg)
                {
                    if (!t.Any())
                        continue;

                    var sendBytes = new List<byte>();
                    sendBytes.AddRange(t);
                    sendBytes.AddRange(GetCrc(t).Reverse());
                    sendBytes.Insert(0, 0x55);
                    UartCan.SendCommand(sendBytes.ToArray(), 5);
                }
            }
            catch (Exception e)
            {
                // ignored
            }
            finally
            {
                _msgEnumerationSemaphore.Release();
            }
        }

        #endregion

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

        private readonly SemaphoreSlim _deviceEnumerationSemaphore = new SemaphoreSlim(1, 1);
        private readonly SemaphoreSlim _msgEnumerationSemaphore = new SemaphoreSlim(1, 1);
        private readonly List<Dev> _devList = new List<Dev>();

        internal class Dev
        {
            public bool IsOnline { get; set; }

            public ushort DevAddr { get; set; }

            public string Cs0Setting { get; set; }
            public string Cs1Setting { get; set; }
            public string Cs2Setting { get; set; }
            public string Cs3Setting { get; set; }

            public Action CmdAction { get; set; }
            public Dictionary<int, float> PwmDuty = new Dictionary<int, float>();
            public Dictionary<int, byte> DcDim = new Dictionary<int, byte>();
            public Dictionary<int, byte> DimSet = new Dictionary<int, byte>();
            public Dictionary<int, string> ChannelEnable = new Dictionary<int, string>();

            public Dev()
            {
                for (var i = 1; i <= 24; i++)
                {
                    PwmDuty.Add(i, 100f);
                    DimSet.Add(i, 0x00);
                    ChannelEnable.Add(i, "0");
                    DcDim.Add(i, 0xFF);
                }
            }

            public void ChannelSet(int index, bool isOn)
            {
                if (ChannelEnable.ContainsKey(index))
                    ChannelEnable[index] = isOn ? 1.ToString() : 0.ToString();
            }

            public void PwmSet(int index, float pwm)
            {
                if (PwmDuty.ContainsKey(index))
                    PwmDuty[index] = pwm;
            }

            public void DimSetValue(int index, byte dim)
            {
                if (DimSet.ContainsKey(index))
                    DimSet[index] = dim;
            }

            public void DcDimSetValue(int index, byte dcDim)
            {
                if (DcDim.ContainsKey(index))
                    DcDim[index] = dcDim;
            }
        }

        private static byte[] WriteReg(byte addr, IReadOnlyCollection<byte> values, bool isBroadcast = false, Dev dev = null)
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
                return new byte[0];

            sendBytes.Add(Convert.ToByte(str, 2)); // Device address, Broadcast, Write / Read
            sendBytes.Add((byte)values.Count); // Number of Data
            sendBytes.Add(addr); // Register Address

            var dataBytes = new List<byte>();
            dataBytes.AddRange(values);

            sendBytes.AddRange(dataBytes); // Data

            return sendBytes.ToArray();
        }

        #endregion
    }
}
