using CommonUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Controller
{
    public sealed class Nsl23716x : ControllerBase
    {
        [Description("R,OTP结果")]
        public string OTPResult = string.Empty;
        [Description("R,OTP寄存器2F读取值")]
        public string ReadOtpReg2F = string.Empty;
        [Description("R,OTP寄存器30读取值")]
        public string ReadOtpReg30 = string.Empty;
        [Description("R,OTP寄存器31读取值")]
        public string ReadOtpReg31 = string.Empty;
        [Description("R,OTP寄存器32读取值")]
        public string ReadOtpReg32 = string.Empty;

        [Description("R/W,是否打印输入")]
        public bool IsPrintLog = true;

        public MySerialPort MySerialPort;

        public Nsl23716x(string name) : base(name)
        {
            CRC_Table_Init(_tps929120TiCrcInfo);
            MySerialPort.PushSciMsg += MySerialPort_PushSciMsg;
        }

        ~Nsl23716x() => Dispose();

        [Description("OTP")]
        public void Otp(byte devAddr, string toWriteReg2F, string toWriteReg30, string toWriteReg31, string toWriteReg32)
        {
            OTPResult = string.Empty;
            ReadOtpReg2F = string.Empty;
            ReadOtpReg30 = string.Empty;
            ReadOtpReg31 = string.Empty;
            ReadOtpReg32 = string.Empty;

            if (MySerialPort is null)
                return;

            // Un-lock host OTP registers（0X2D write 1 byte h93 and h65）
            if (TpsSendCmd(GetSendData(FormatWriteHeader(devAddr, 1), 0x2D, new byte[] { 0x93 }), out _))
            {
                if (TpsSendCmd(GetSendData(FormatWriteHeader(devAddr, 1), 0x2D, new byte[] { 0x65 }), out _))
                {
                    byte[] echo2F303132;
                    // Pre-Read OTP registers(0x2F~0x32)
                    if (TpsSendCmd(GetSendData(FormatReadHeader(devAddr, 8), 0x2F, new byte[] { }), out echo2F303132) && echo2F303132.Length == 8)
                    {
                        var readOtpReg2F = "0x" + ((ushort)(echo2F303132[1] * 256 + echo2F303132[0])).ToString("X4");
                        var readOtpReg30 = "0x" + ((ushort)(echo2F303132[3] * 256 + echo2F303132[2])).ToString("X4");
                        var readOtpReg31 = "0x" + ((ushort)(echo2F303132[5] * 256 + echo2F303132[4])).ToString("X4");
                        var readOtpReg32 = "0x" + ((ushort)(echo2F303132[7] * 256 + echo2F303132[6])).ToString("X4");

                        if (string.Equals(readOtpReg2F, toWriteReg2F, StringComparison.CurrentCultureIgnoreCase) &&
                            string.Equals(readOtpReg30, toWriteReg30, StringComparison.CurrentCultureIgnoreCase) &&
                            string.Equals(readOtpReg31, toWriteReg31, StringComparison.CurrentCultureIgnoreCase) &&
                            string.Equals(readOtpReg32, toWriteReg32, StringComparison.CurrentCultureIgnoreCase))
                        {
                            TpsSendCmd(GetSendData(FormatWriteHeader(devAddr, 1), 0x2D, new byte[] { 0xCF }), out _);
                            ReadOtpReg2F = readOtpReg2F;
                            ReadOtpReg30 = readOtpReg30;
                            ReadOtpReg31 = readOtpReg31;
                            ReadOtpReg32 = readOtpReg32;
                            OTPResult = "OK 预读取值与待烧写值一致,无需OTP";
                        }
                        else
                        {
                            echo2F303132 = null;

                            // Configurate host register(0x2F~0x32)
                            try
                            {
                                var toWriteReg2FBytes = BitConverter.GetBytes(Convert.ToUInt16(toWriteReg2F, 16));
                                var toWriteReg30Bytes = BitConverter.GetBytes(Convert.ToUInt16(toWriteReg30, 16));
                                var toWriteReg31Bytes = BitConverter.GetBytes(Convert.ToUInt16(toWriteReg31, 16));
                                var toWriteReg32Bytes = BitConverter.GetBytes(Convert.ToUInt16(toWriteReg32, 16));

                                var toWriteData1 = new byte[] { toWriteReg2FBytes[0], toWriteReg2FBytes[1], toWriteReg30Bytes[0], toWriteReg30Bytes[1], toWriteReg31Bytes[0], toWriteReg31Bytes[1], toWriteReg32Bytes[0], toWriteReg32Bytes[1] };
                                TpsSendCmd(GetSendData(FormatWriteHeader(devAddr, toWriteData1.Length), 0x2F, toWriteData1), out _);
                            }
                            catch (Exception)
                            {
                                OTPResult = "NG 待写入数据异常";
                                return;
                            }

                            // Read host register(0x2F-0x32)
                            if (TpsSendCmd(GetSendData(FormatReadHeader(devAddr, 8), 0x2F, new byte[] { }), out echo2F303132) && echo2F303132.Length == 8)
                            {
                                ReadOtpReg2F = "0x" + ((ushort)(echo2F303132[1] * 256 + echo2F303132[0])).ToString("X4");
                                ReadOtpReg30 = "0x" + ((ushort)(echo2F303132[3] * 256 + echo2F303132[2])).ToString("X4");
                                ReadOtpReg31 = "0x" + ((ushort)(echo2F303132[5] * 256 + echo2F303132[4])).ToString("X4");
                                ReadOtpReg32 = "0x" + ((ushort)(echo2F303132[7] * 256 + echo2F303132[6])).ToString("X4");

                                // Check registers value whether equal
                                if (string.Equals(ReadOtpReg2F, toWriteReg2F, StringComparison.CurrentCultureIgnoreCase) &&
                                    string.Equals(ReadOtpReg30, toWriteReg30, StringComparison.CurrentCultureIgnoreCase) &&
                                    string.Equals(ReadOtpReg31, toWriteReg31, StringComparison.CurrentCultureIgnoreCase) &&
                                    string.Equals(ReadOtpReg32, toWriteReg32, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    // OTP programming（Configure 0X2E HOST_OTP_PROGRAM bit = 1）
                                    var host_otp_prm_bit = BitConverter.GetBytes(Convert.ToUInt16("0x8000", 16));
                                    if (TpsSendCmd(GetSendData(FormatWriteHeader(devAddr, 2), 0x2E, host_otp_prm_bit), out _))
                                    {
                                        // Read 0X2E and check HOST_OTP_PROGRAM == 0 ?
                                        var bCheck_host_otp_prm_bit_ok = false;
                                        var tpReg2E = string.Empty;
                                        for (int i = 0; i < 5; i++)
                                        {
                                            Thread.Sleep(110);
                                            byte[] echoReg2E;
                                            if (TpsSendCmd(GetSendData(FormatReadHeader(devAddr, 2), 0x2E, new byte[] { }), out echoReg2E) && echoReg2E.Length == 2)
                                            {
                                                tpReg2E = "0x" + ((ushort)(echoReg2E[1] * 256 + echoReg2E[0])).ToString("X4");
                                                if (echoReg2E[0] == 0x00 && echoReg2E[1] == 0x00)
                                                {
                                                    bCheck_host_otp_prm_bit_ok = true;
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                OTPResult = "NG Read REG 0X2E failed";
                                                return;
                                            }
                                        }

                                        if (bCheck_host_otp_prm_bit_ok)
                                        {
                                            // Lock host OTP registers（0X2D write 1 byte hCF）
                                            if (TpsSendCmd(GetSendData(FormatWriteHeader(devAddr, 1), 0x2D, new byte[] { 0xCF }), out _))
                                            {
                                                OTPResult = "OK";
                                            }
                                            else
                                            {
                                                OTPResult = "NG Lock host OTP registers（0X2D write 1 byte hCF） failed";
                                            }
                                        }
                                        else
                                        {
                                            OTPResult = "NG Read 0X2E and check HOST_OTP_PROGRAM == 0 failed, Reg2E = " + tpReg2E;
                                        }
                                    }
                                    else
                                    {
                                        OTPResult = "NG Configure 0X2E HOST_OTP_PROGRAM bit = 1 failed";
                                    }
                                }
                                else
                                {
                                    OTPResult = "NG Check registers value not equal";
                                }
                            }
                            else
                            {
                                OTPResult = "NG";
                            }
                        }
                    }
                    else
                    {
                        OTPResult = "NG 预读取OTP寄存器失败,通讯异常";
                    }
                }
                else
                {
                    OTPResult = "NG Un-lock host OTP registers, enter: h65, failed";
                }
            }
            else
            {
                OTPResult = "NG Un-lock host OTP registers, enter: h93, failed";
            }
        }

        private byte FormatReadHeader(byte addr, int len)
        {
            var devBit = Convert.ToString(addr, 2).PadLeft(4, '0');

            var lenBit = "00";
            switch (len)
            {
                case 1:
                    lenBit = "00";
                    break;

                case 2:
                    lenBit = "01";
                    break;

                case 4:
                    lenBit = "10";
                    break;

                case 8:
                    lenBit = "11";
                    break;
            }

            var str = string.Format("{0}{1}{2}{3}01{4}{5}", devBit[0], devBit[1], devBit[2], devBit[3], lenBit[0], lenBit[1]);
            return new byte[] { Convert.ToByte(str, 2) }[0];
        }

        private byte FormatWriteHeader(byte addr, int len)
        {
            var devBit = Convert.ToString(addr, 2).PadLeft(4, '0');

            var lenBit = "00";
            switch (len)
            {
                case 1:
                    lenBit = "00";
                    break;

                case 2:
                    lenBit = "01";
                    break;

                case 4:
                    lenBit = "10";
                    break;

                case 8:
                    lenBit = "11";
                    break;
            }

            var str = string.Format("{0}{1}{2}{3}00{4}{5}", devBit[0], devBit[1], devBit[2], devBit[3], lenBit[0], lenBit[1]);
            return new byte[] { Convert.ToByte(str, 2) }[0];
        }

        private void MySerialPort_PushSciMsg(string name, byte[] datas)
        {
            if (MySerialPort == null || MySerialPort.Name != name)
                return;

            var str =
                datas.Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
            if (IsPrintLog)
            {
                Debug.WriteLine(str);
            }
            if (!_isSendCmd || string.IsNullOrEmpty(_rxStr))
                return;
            if (string.IsNullOrEmpty(str) || str.Length <= _rxStr.Length)
                return;
            if (str.Substring(0, _rxStr.Length) != _rxStr)
                return;
            _rxStr = string.Empty;
            _txStr = str;
            _waitHandle.Set();
        }

        /// <summary>
        /// 是否已发送命令
        /// </summary>
        private bool _isSendCmd;

        /// <summary>
        /// 发送的命令
        /// </summary>
        private string _rxStr = string.Empty;

        /// <summary>
        /// 收到的命令
        /// </summary>
        private string _txStr = string.Empty;

        /// <summary>
        /// 事件
        /// </summary>
        private readonly EventWaitHandle _waitHandle = new AutoResetEvent(false);

        private const byte SyncFrame = 0x55;
        private readonly uint[] _table = new uint[256];

        private void CRC_Table_Init(CrcHelper.CrcInfo crcInfo)
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
                    _table[i] = value & validBits;
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
                    _table[i] = value & (crcInfo.Width < 8 ? 0xff : validBits);
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

        private uint CALC_CRC(CrcHelper.CrcInfo info, IReadOnlyList<byte> memBlock)
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
                        value = (value >> 8) ^ _table[value & 0xff ^ memBlock[i++]];
                    }
                }
                else
                {
                    var i = 0;
                    while (memBlockLen-- != 0)
                    {
                        value = _table[value ^ memBlock[i++]];
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
                        value = (value << 8) ^ _table[high ^ memBlock[i++]];
                    }
                }
                else
                {
                    value = info.InitReg << (8 - info.Width);
                    var i = 0;
                    while (memBlockLen-- != 0)
                    {
                        value = _table[value ^ memBlock[i++]];
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

        private byte[] GetSendData(
            byte devAddrByte, byte regAddeByte, IEnumerable<byte> datas)
        {
            var sendData = new List<byte>();
            sendData.AddRange(datas);

            sendData.Insert(0, regAddeByte);
            sendData.Insert(0, devAddrByte);
            var crc = CALC_CRC(_tps929120TiCrcInfo, sendData.ToArray());
            sendData.Add((byte)crc);
            sendData.Insert(0, SyncFrame);

            return sendData.ToArray();
        }

        private readonly CrcHelper.CrcInfo _tps929120TiCrcInfo = new CrcHelper.CrcInfo
        {
            Width = 8,
            Poly = 0x31,
            InitReg = 0xff,
            Refin = false,
            Refout = false,
            Xorout = 0x00
        };

        private bool IsCheckCrcWhenWtire = true;

        private bool TpsSendCmd(byte[] cmdData, out byte[] recv)
        {
            recv = null;

            if (MySerialPort == null)
                return false;

            _isSendCmd = true;
            var str = cmdData.Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
            _rxStr = str;
            MySerialPort.SendCommand(cmdData, 1);
            //Thread.Sleep(25);
            if (_waitHandle.WaitOne(100))
            {
                if (!string.IsNullOrEmpty(_txStr) &&
                    _txStr.Length % 2 == 0 &&
                    _txStr.Length > str.Length &&
                    _txStr.Substring(0, str.Length) == str)
                {
                    var rxBytes = new List<byte>();
                    for (var i = 0; i < _txStr.Length; i = i + 2)
                    {
                        var b = string.Format("{0}{1}", _txStr[i], _txStr[i + 1]);
                        rxBytes.Add(Convert.ToByte(b, 16));
                    }

                    var bitStr = Convert.ToString(rxBytes[1], 2).PadLeft(8, '0');
                    var temp = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", bitStr[7], bitStr[6], bitStr[5], bitStr[4],
                        bitStr[3], bitStr[2], bitStr[1], bitStr[0]);
                    bitStr = temp;

                    var deviceAddr =
                        Convert.ToByte(string.Format("0000{3}{2}{1}{0}", bitStr[4], bitStr[5], bitStr[6], bitStr[7]), 2);
                    var dataLength = 1;
                    switch (string.Format("{0}{1}", bitStr[0], bitStr[1]))
                    {
                        case "00":
                            dataLength = 1;
                            break;

                        case "10":
                            dataLength = 2;
                            break;

                        case "01":
                            dataLength = 4;
                            break;

                        case "11":
                            dataLength = 8;
                            break;
                    }

                    var isRead = bitStr[2].ToString() == 1.ToString();

                    if (isRead)
                    {
                        if (rxBytes.Count == 1 + 1 + 1 + 1 + dataLength + 1 + 1)
                        {
                            var rxValues = new byte[dataLength + 1];
                            Array.Copy(rxBytes.ToArray(), 4, rxValues, 0, rxValues.Length);

                            var rxCrc = rxBytes[rxBytes.Count - 1];
                            if (CALC_CRC(_tps929120TiCrcInfo, rxValues) == rxCrc)
                            {
                                recv = new byte[dataLength];
                                for (var i = 0; i < dataLength; i++)
                                {
                                    var i1 = i;
                                    recv[i1] = rxValues[i + 1];
                                    //foreach (var f in from f in GetType().GetFields()
                                    //                  let findF = string.Format("DevAddr{0}hRegAddr{1}h",
                                    //                  ValueHelper.GetHextStr(deviceAddr),
                                    //                  ValueHelper.GetHextStr((byte)(rxBytes[2] + i1)))
                                    //                  where f.Name.StartsWith(findF)
                                    //                  select f)
                                    //    f.SetValue(this, ValueHelper.GetHextStr(rxValues[i]));
                                }
                                return true;
                            }
                        }
                    }
                    else
                    {
                        if (rxBytes.Count == 1 + 1 + 1 + dataLength + 1 + 2)
                        {
                            return IsCheckCrcWhenWtire ? (CALC_CRC(_tps929120TiCrcInfo, new[] { rxBytes[1 + 1 + 1 + dataLength + 1] }) == rxBytes[1 + 1 + 1 + dataLength + 1 + 1]) : true;

                            //var rxStatus = rxBytes[1 + 1 + 1 + dataLength + 1];
                            //var rxCrc = rxBytes[1 + 1 + 1 + dataLength + 1 + 1];

                            //if (IsCheckCrcWhenWtire)
                            //{
                            //    if (CALC_CRC(_tps929120TiCrcInfo, new[] { rxStatus }) == rxCrc)
                            //    {
                            //        return true;
                            //    }
                            //    else
                            //    {
                            //        return false;
                            //    }
                            //}
                            //else
                            //{
                            //    return true;
                            //}
                        }
                    }
                }
            }

            return false;
        }
    }
}
