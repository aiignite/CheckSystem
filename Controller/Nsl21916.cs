using CommonUtility;
using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace Controller
{
    public sealed class Nsl21916 : ControllerBase
    {
        public MySerialPort MySerialPort;

        [Description("R/W,写入时是否校验回复帧的CRC")]
        public bool IsCheckCrcWhenWtire = true;

        public Nsl21916(string name) : base(name)
        {
            CRC_Init();
            CRC_Table_Init(_tps929120TiCrcInfo);
            MySerialPort.PushSciMsg += MySerialPort_PushSciMsg;
            LedCtrlTimer();
        }

        ~Nsl21916() => Dispose();

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

        private void MySerialPort_PushSciMsg(string name, byte[] datas)
        {
            if (MySerialPort == null || MySerialPort.Name != name)
                return;

            var str =
                datas.Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
            //Debug.WriteLine(str);

            var str2 = datas.Aggregate(string.Empty, (current, t) => current + " " + ValueHelper.GetHextStr(t));
            Debug.WriteLine(str2);

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

        [Description("R/W,配置参数文件路径")]
        public string DataFilePath =
        //@"Y:\Projs\2022\芯片相关\NSL23924\奥迪427\427CSUV_NSL23924_EEPROM\亮度档5B3B\signal 2 U2_ADDR3.csv";
        //@"Y:\Projs\2022\芯片相关\NSL23924\奥迪427\427CSUV_NSL23924_EEPROM\亮度档6A3B\signal 1 U2_ADDR2.csv";
        //@"Y:\Projs\2022\芯片相关\NSL23924\奥迪427\427CSUV_NSL23924_EEPROM\亮度档5B3B\signal 1 U2_ADDR2.csv";
        @"Y:\Projs\2022\芯片相关\NSL21916\测试用-1011.csv";
        //@"E:\Projects\VW427\427资料\NSL\427-DRL-EPPROM-20250627\亮度档6A3B\TURNDRLPL1-6A3B-ADDR1-V001-20250822.csv";
        //@"Y:\Projs\2022\芯片相关\NSL23924\test2.csv";
        //string.Empty;

        [Description("R/W,REF引脚是否拉高")]
        public bool IsRefPinHigh = false;

        [Description("R,遍历刷写成功的芯片个数")]
        public int ErgodicProgramTpsCount;

        [Description("R,遍历刷写成功的芯片地址列表")]
        public string ErgodicProgramTpsDevAddrs;

        [Description("R,遍历刷写总耗时")]
        public string ErgodicProgramTpsCostMs;

        private const byte SyncFrame = 0x55;
        private const byte BroadcastWrite1BytesDevAddrByte = 0x80;
        private const byte BroadcastWrite4BytesDevAddrByte = 0x90;
        private const byte BroadcastWrite16BytesDevAddrByte = 0xA0;
        private const byte BroadcastWrite24BytesDevAddrByte = 0xB0;
        private readonly CrcHelper.CrcInfo _tps929120TiCrcInfo = new CrcHelper.CrcInfo
        {
            Width = 8,
            Poly = 0x31,
            InitReg = 0xff,
            Refin = true,
            Refout = false,
            Xorout = 0x00
        };

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

        private static byte CRC_For_929160(byte[] data)
        {
            byte[] array = new byte[99];
            byte[] array2 = new byte[25]
            {
            2, 5, 8, 11, 14, 17, 20, 23, 26, 29,
            32, 35, 38, 41, 44, 47, 55, 58, 61, 64,
            67, 70, 73, 76, 255
            };
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            for (num = 0; num < 99; num++)
            {
                if (num == array2[num2])
                {
                    array[num] = 0;
                    num2++;
                }
                else
                {
                    array[num] = data[num3];
                    num3++;
                }
            }
            return CRC(array, 99, iseep: true);
        }

        private static byte CRC(byte[] commandframe, byte length, bool iseep)
        {
            byte b = byte.MaxValue;
            for (uint num = ((!iseep) ? 1u : 0u); num < length; num++)
            {
                byte b2 = (byte)(b ^ commandframe[num]);
                b = CrcArray[b2];
            }
            return (byte)(((b & 0x80) >> 7) + ((b & 0x40) >> 5) + ((b & 0x20) >> 3) + ((b & 0x10) >> 1) + ((b & 8) << 1) + ((b & 4) << 3) + ((b & 2) << 5) + ((b & 1) << 7));
        }

        private static byte[] CrcArray = new byte[256];

        private static void CRC_Init()
        {
            for (uint num = 0u; num < 256; num++)
            {
                byte b = (byte)num;
                for (uint num2 = 8u; num2 != 0; num2--)
                {
                    b = (((b & 1) != 1) ? ((byte)(b >> 1)) : ((byte)((b >> 1) ^ 0x8C)));
                }
                CrcArray[num] = b;
            }
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

        private bool TpsSendCmd(byte[] cmdData)
        {
            if (MySerialPort == null)
                return false;

            _isSendCmd = true;
            var str = cmdData.Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
            _rxStr = str;
            MySerialPort.SendCommand(cmdData, 1);
            //Thread.Sleep(25);
            if (_waitHandle.WaitOne(50))
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
                        Convert.ToByte(string.Format("0000{3}{2}{1}{0}", bitStr[0], bitStr[1], bitStr[2], bitStr[3]), 2);
                    var dataLength = 1;
                    switch (string.Format("{0}{1}", bitStr[4], bitStr[5]))
                    {
                        case "00":
                            dataLength = 1;
                            break;

                        case "10":
                            dataLength = 4;
                            break;

                        case "01":
                            dataLength = 16;
                            break;

                        case "11":
                            dataLength = 24;
                            break;
                    }

                    var isRead = bitStr[7].ToString() == 0.ToString();

                    if (isRead)
                    {
                        if (rxBytes.Count == 1 + 1 + 1 + 1 + dataLength + 1)
                        {
                            var rxValues = new byte[dataLength];
                            Array.Copy(rxBytes.ToArray(), 4, rxValues, 0, rxValues.Length);

                            var rxCrc = rxBytes[4 + dataLength + 1 - 1];
                            if (CALC_CRC(_tps929120TiCrcInfo, rxValues) == rxCrc)
                            {
                                for (var i = 0; i < dataLength; i++)
                                {
                                    var i1 = i;
                                    foreach (var f in from f in GetType().GetFields()
                                                      let findF = string.Format("DevAddr{0}hRegAddr{1}h",
                                                      ValueHelper.GetHextStr(deviceAddr),
                                                      ValueHelper.GetHextStr((byte)(rxBytes[2] + i1)))
                                                      where f.Name.StartsWith(findF)
                                                      select f)
                                        f.SetValue(this, ValueHelper.GetHextStr(rxValues[i]));
                                }
                                return true;
                            }
                        }
                    }
                    else
                    {
                        if (rxBytes.Count == 1 + 1 + 1 + dataLength + 1 + 2)
                        {
                            var rxStatus = rxBytes[1 + 1 + 1 + dataLength + 1];
                            var rxCrc = rxBytes[1 + 1 + 1 + dataLength + 1 + 1];

                            if (IsCheckCrcWhenWtire)
                            {
                                if (CALC_CRC(_tps929120TiCrcInfo, new[] { rxStatus }) == rxCrc)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        #region EEPROM刷写

        [Description("从配置文件读取参数并单独刷写")]
        public void SingleErgodicProgramTpsFromDataFile(string index)
        {
            ErgodicProgramTpsCount = 0;
            ErgodicProgramTpsDevAddrs = string.Empty;
            ErgodicProgramTpsCostMs = string.Empty;

            if (string.IsNullOrEmpty(DataFilePath))
                return;

            if (!File.Exists(DataFilePath))
                return;

            if (MySerialPort == null)
                return;

            int matrixIndex;
            if (!int.TryParse(index, out matrixIndex))
                return;

            if (matrixIndex < 0 || matrixIndex > 15)
                return;

            var st = new Stopwatch();
            st.Start();

            var dataList = new List<Dictionary<byte, byte>>();

            var toCrcData = new List<byte>();
            var csvData = new Dictionary<byte, byte>();
            var eppromData = new List<byte>();
            var crc = 0xFF;

            try
            {
                var lines = MiniExcel.Query(DataFilePath, excelType: ExcelType.CSV).ToList();

                //if (lines.Count < 101)
                //return;

                if (lines[0].A.ToString().ToLower() != "ADDRESS".ToLower() &&
                    lines[0].B.ToString().ToLower() != "VALUE".ToLower())
                    return;

                for (var i = 1; i < lines.Count; i++)
                {
                    var l = lines[i];

                    var regAddr = Convert.ToByte(l.A.ToString(), 16).ToString();
                    var regData = Convert.ToByte(l.B.ToString(), 16).ToString();

                    if (string.IsNullOrEmpty(regAddr) || string.IsNullOrEmpty(regData))
                        return;

                    byte bRegAddr = 0;
                    byte bRegData = 0;
                    if (byte.TryParse(regAddr, out bRegAddr) && byte.TryParse(regData, out bRegData))
                    {
                        if (bRegAddr != 0x87)
                        {
                            if (csvData.ContainsKey(bRegAddr))
                                return;
                            csvData.Add(bRegAddr, bRegData);
                        }
                        else
                            crc = bRegData;
                    }
                    else
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                return;
            }

            for (var addr = 0x00; addr <= 0x0F; addr++)
            {
                if (!csvData.ContainsKey((byte)addr))
                    return;

                toCrcData.Add(csvData[(byte)addr]);
            }
            for (var addr = 0x20; addr <= 0x2F; addr++)
            {
                if (!csvData.ContainsKey((byte)addr))
                    return;

                toCrcData.Add(csvData[(byte)addr]);
            }
            for (var addr = 0x40; addr <= 0x44; addr++)
            {
                if (!csvData.ContainsKey((byte)addr))
                    return;

                toCrcData.Add(csvData[(byte)addr]);
            }
            for (var addr = 0x50; addr <= 0x5F; addr++)
            {
                if (!csvData.ContainsKey((byte)addr))
                    return;

                toCrcData.Add(csvData[(byte)addr]);
            }
            for (var addr = 0x70; addr <= 0x7E; addr++)
            {
                if (!csvData.ContainsKey((byte)addr))
                    return;

                toCrcData.Add(csvData[(byte)addr]);
            }
            for (var addr = 0x80; addr <= 0x86; addr++)
            {
                if (!csvData.ContainsKey((byte)addr))
                    return;

                toCrcData.Add(csvData[(byte)addr]);
            }

            var totalCrc = CRC_For_929160(toCrcData.ToArray());

            if (crc == 0xFF || totalCrc != crc)
                return;

            dataList.Add(new Dictionary<byte, byte>());
            for (var addr = 0x00; addr <= 0x0F; addr++)
                dataList[dataList.Count - 1].Add((byte)addr, csvData[(byte)addr]);

            dataList.Add(new Dictionary<byte, byte>());
            for (var addr = 0x20; addr <= 0x2F; addr++)
                dataList[dataList.Count - 1].Add((byte)addr, csvData[(byte)addr]);

            dataList.Add(new Dictionary<byte, byte>());
            for (var addr = 0x40; addr <= 0x44; addr++)
                dataList[dataList.Count - 1].Add((byte)addr, csvData[(byte)addr]);

            dataList.Add(new Dictionary<byte, byte>());
            for (var addr = 0x50; addr <= 0x5F; addr++)
                dataList[dataList.Count - 1].Add((byte)addr, csvData[(byte)addr]);

            dataList.Add(new Dictionary<byte, byte>());
            for (var addr = 0x70; addr <= 0x7E; addr++)
                dataList[dataList.Count - 1].Add((byte)addr, csvData[(byte)addr]);

            dataList.Add(new Dictionary<byte, byte>());
            for (var addr = 0x80; addr <= 0x86; addr++)
                dataList[dataList.Count - 1].Add((byte)addr, csvData[(byte)addr]);
            dataList[dataList.Count - 1].Add((byte)0x87, totalCrc);

            dataList.Add(new Dictionary<byte, byte>());
            for (var addr = 0xC0; addr <= 0xC5; addr++)
            {
                if (!csvData.ContainsKey((byte)addr))
                    continue;

                dataList[dataList.Count - 1].Add((byte)addr, csvData[(byte)addr]);
            }

            ErgodicProgram(matrixIndex.ToString(), matrixIndex.ToString(), dataList);

            st.Stop();
            ErgodicProgramTpsDevAddrs = ErgodicProgramTpsDevAddrs.Trim(',');
            ErgodicProgramTpsCostMs = st.ElapsedMilliseconds + "ms";
        }

        private void ErgodicProgram(
            string startIndex, string endIndex, List<Dictionary<byte, byte>> dataList)
        {
            for (var p = int.Parse(startIndex); p < int.Parse(endIndex) + 1; p++)
            {
                if (true)
                {
                    if (true)
                    {
                        //continue;

                        // access the EEPROM
                        if (IsRefPinHigh)
                        {
                            TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x98, new byte[] { 0x09 }));
                            TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x98, new byte[] { 0x02 }));
                            TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x98, new byte[] { 0x09 }));
                            TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x98, new byte[] { 0x02 }));
                            TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x98, new byte[] { 0x04 }));
                            TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x98, new byte[] { 0x00 }));
                        }
                        else
                        {
                            TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x98, new byte[] { 0x00 }));
                            TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x98, new byte[] { 0x04 }));
                            TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x98, new byte[] { 0x02 }));
                            TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x98, new byte[] { 0x09 }));
                            TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x98, new byte[] { 0x02 }));
                            TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x98, new byte[] { 0x09 }));
                        }

                        // Enable EEPMODE Programming State
                        if (TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x97, new byte[] { 0x01 })))
                        {
                            if (true)
                            {
                                var sendError = 0;

                                // • Wrtie data to EEPROM including CRC
                                foreach (var d in dataList)
                                {
                                    if (d.Count >= 24) // 按最大24字节分组
                                    {
                                        var groupCount = d.Count / 24;
                                        var rest = d.Count - groupCount * 24;

                                        var baseIndex = -1;

                                        for (var i = 0; i < groupCount; i++)
                                        {
                                            var sendData = new List<byte>();
                                            for (var j = 0; j < 24; j++)
                                            {
                                                baseIndex++;
                                                var key = d.Keys.ToList()[baseIndex];
                                                sendData.Add(d[key]);
                                            }

                                            if (!TpsSendCmd(GetSendData((byte)(BroadcastWrite24BytesDevAddrByte + p),
                                                d.Keys.ToList()[0 + 24 * i], sendData)))
                                                sendError++;
                                        }

                                        for (var i = 0; i < rest; i++)
                                        {
                                            var sendData = new List<byte>();
                                            baseIndex++;
                                            var key = d.Keys.ToList()[baseIndex];
                                            sendData.Add(d[key]);

                                            if (!TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), key,
                                                sendData)))
                                                sendError++;
                                        }
                                    }
                                    else if (d.Count == 16) // 按最大16字节分组
                                    {
                                        var addr = d.Keys.ToList()[0];
                                        var data = new byte[16];
                                        for (var writeCount = 0; writeCount < 16; writeCount++)
                                            data[writeCount] = d[d.Keys.ToList()[writeCount]];
                                        if (!TpsSendCmd(GetSendData((byte)(BroadcastWrite16BytesDevAddrByte + p), addr, data)))
                                            sendError++;
                                    }
                                    else if (d.Count == 8) // 按最大8字节分组
                                    {
                                        // 分2次4字节写
                                        var addr1 = d.Keys.ToList()[0];
                                        var data1 = new byte[4];
                                        for (var writeCount = 0; writeCount < 4; writeCount++)
                                            data1[writeCount] = d[d.Keys.ToList()[writeCount]];
                                        if (!TpsSendCmd(GetSendData((byte)(BroadcastWrite4BytesDevAddrByte + p), addr1, data1)))
                                            sendError++;

                                        var addr2 = d.Keys.ToList()[4];
                                        var data2 = new byte[4];
                                        for (var writeCount = 4; writeCount < 8; writeCount++)
                                            data2[writeCount - 4] = d[d.Keys.ToList()[writeCount]];
                                        if (!TpsSendCmd(GetSendData((byte)(BroadcastWrite4BytesDevAddrByte + p), addr2, data2)))
                                            sendError++;
                                    }
                                    else  // 按最大1字节分组
                                    {
                                        var baseIndex = -1;

                                        for (var i = 0; i < d.Count; i++)
                                        {
                                            var sendData = new List<byte>();
                                            baseIndex++;
                                            var key = d.Keys.ToList()[baseIndex];
                                            sendData.Add(d[key]);

                                            if (
                                                !TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p),
                                                    key,
                                                    sendData)))
                                                sendError++;
                                            //TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), key,
                                            //    sendData));
                                        }
                                    }
                                }

                                if (sendError == 0)
                                {
                                    // EEPROM burning start in EEPROM mode only, automatically returns to 0
                                    // • Write 01h to CONF_EEOOROG
                                    if (TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x97, new byte[] { 0x03 })))
                                    {
                                        // • Keep supply stable and wait for 250
                                        Thread.Sleep(250);

                                        // • Write 0h to CONF_STAYINEEP to Normal mode
                                        if (TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x97, new byte[] { 0x02 })))
                                        {
                                            Thread.Sleep(15);
                                        }

                                        // • Write 0h to CONF_STAYINEEP to Normal mode
                                        if (TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x97, new byte[] { 0x00 })))
                                        {
                                            Thread.Sleep(15);
                                        }
                                    }
                                }

                                var isOk = true;
                                foreach (var t in dataList)
                                {
                                    foreach (var regAddr in t.Keys)
                                    {
                                        var regValue = t[regAddr];

                                        //if (regAddr != 0xCF)
                                        {
                                            ReadSingleDevReg(p.ToString(), ValueHelper.GetHextStr(regAddr));

                                            var findFieldName = string.Format("DevAddr{0}hRegAddr{1}h",
                                                ValueHelper.GetHextStr(Convert.ToByte(p.ToString())),
                                                ValueHelper.GetHextStr(regAddr));

                                            foreach (var f in GetType().GetFields())
                                            {
                                                if (f.Name.StartsWith(findFieldName))
                                                {
                                                    var fv = f.GetValue(this);
                                                    if (fv == null || string.IsNullOrEmpty(fv.ToString()))
                                                    {
                                                        isOk = false;
                                                        break;
                                                    }

                                                    if (fv.ToString() != ValueHelper.GetHextStr(regValue))
                                                    {
                                                        isOk = false;
                                                    }

                                                    break;
                                                }
                                            }
                                        }

                                        if (!isOk)
                                            break;
                                    }

                                    if (!isOk)
                                        break;
                                }

                                if (isOk)
                                {
                                    ErgodicProgramTpsDevAddrs += ValueHelper.GetHextStr((byte)p) + ",";
                                    ErgodicProgramTpsCount++;
                                }
                            }
                        }
                    }
                }
            }
        }

        [Description("读寄存器")]
        public void ReadSingleDevReg(string devAddr, string regAddr)
        {
            var devAddrByte = Convert.ToByte(byte.Parse(devAddr).ToString());
            var regAddrByte = Convert.ToByte(regAddr, 16);
            for (var q = 0; q < 24; q++)
            {
                var i1 = q;
                var addrByte = devAddrByte;
                foreach (var f in from f in GetType().GetFields()
                                  let findF = string.Format("DevAddr{0}hRegAddr{1}h",
                                  ValueHelper.GetHextStr(addrByte),
                                  ValueHelper.GetHextStr((byte)(regAddrByte + i1)))
                                  where f.Name.StartsWith(findF)
                                  select f)
                    if (f != null)
                        f.SetValue(this, string.Empty);
            }

            devAddrByte = (byte)(0x30 + devAddrByte);
            TpsSendCmd(GetSendData(devAddrByte, regAddrByte, new byte[0]));
            //if (MySerialPort != null)
            //    MySerialPort.SendCommand(GetSendData(devAddrByte, regAddrByte, new byte[0])); //debug 55 06 80 E5 
        }

        [Description("读单个芯片所有寄存器")]
        public void ReadSingleChipAllRegs(string devAddr)
        {
            byte addr;
            if (byte.TryParse(devAddr, out addr))
            {
                if (addr >= 0 && addr <= 15)
                {
                    var devAddrByte = Convert.ToByte(byte.Parse(devAddr).ToString());
                    devAddrByte = (byte)(0x00 + devAddrByte);

                    {
                        var readCount = 0;

                        for (var i = 0x00; i <= 0x0F; i++)
                        {
                            TpsSendCmd(GetSendData(devAddrByte, (byte)i, new byte[0]));
                            readCount++;
                        }
                        //Console.WriteLine("读取寄存器{0}个", readCount);

                        for (var i = 0x10; i <= 0x17; i++)
                        {
                            TpsSendCmd(GetSendData(devAddrByte, (byte)i, new byte[0]));
                            readCount++;
                        }
                        //Console.WriteLine("读取寄存器{0}个", readCount);

                        for (var i = 0x20; i <= 0x2f; i++)
                        {
                            TpsSendCmd(GetSendData(devAddrByte, (byte)i, new byte[0]));
                            readCount++;
                        }
                        //Console.WriteLine("读取寄存器{0}个", readCount);

                        for (var i = 0x30; i <= 0x37; i++)
                        {
                            TpsSendCmd(GetSendData(devAddrByte, (byte)i, new byte[0]));
                            readCount++;
                        }
                        //Console.WriteLine("读取寄存器{0}个", readCount);

                        for (var i = 0x40; i <= 0x44; i++)
                        {
                            TpsSendCmd(GetSendData(devAddrByte, (byte)i, new byte[0]));
                            readCount++;
                        }
                        //Console.WriteLine("读取寄存器{0}个", readCount);

                        for (var i = 0x50; i <= 0x5f; i++)
                        {
                            TpsSendCmd(GetSendData(devAddrByte, (byte)i, new byte[0]));
                            readCount++;
                        }
                        //Console.WriteLine("读取寄存器{0}个", readCount);

                        for (var i = 0x60; i <= 0x67; i++)
                        {
                            TpsSendCmd(GetSendData(devAddrByte, (byte)i, new byte[0]));
                            readCount++;
                        }
                        //Console.WriteLine("读取寄存器{0}个", readCount);

                        for (var i = 0x70; i <= 0x7e; i++)
                        {
                            TpsSendCmd(GetSendData(devAddrByte, (byte)i, new byte[0]));
                            readCount++;
                        }
                        //Console.WriteLine("读取寄存器{0}个", readCount);

                        for (var i = 0x80; i <= 0x87; i++)
                        {
                            TpsSendCmd(GetSendData(devAddrByte, (byte)i, new byte[0]));
                            readCount++;
                        }
                        //Console.WriteLine("读取寄存器{0}个", readCount);

                        //Console.WriteLine("共读取寄存器{0}个", readCount);
                    }
                }
            }
        }

        [Description("读所有地址的寄存器")]
        public void ReadAllDevReg(string regAddr)
        {
            for (var i = 0; i < 16; i++)
            {
                var devAddrByte = Convert.ToByte(i.ToString());
                var regAddrByte = Convert.ToByte(regAddr, 16);

                var addrByte = devAddrByte;
                for (var q = 0; q < 8; q++)
                {
                    var i1 = q;
                    foreach (var f in from f in GetType().GetFields()
                                      let findF = string.Format("DevAddr{0}hRegAddr{1}h",
                                      ValueHelper.GetHextStr(addrByte),
                                      ValueHelper.GetHextStr((byte)(regAddrByte + i1)))
                                      where f.Name.StartsWith(findF)
                                      select f)
                        f.SetValue(this, string.Empty);
                }

                //foreach (var f in from f in GetType().GetFields()
                //                  let str = string.Format("DevAddr{0}hRegAddr{1}h", ValueHelper.GetHextStr(addrByte), ValueHelper.GetHextStr(addrByte))
                //                  where f.Name.StartsWith(str)
                //                  select f)
                //    f.SetValue(this, string.Empty);

                devAddrByte = (byte)(0x30 + devAddrByte);
                TpsSendCmd(GetSendData(devAddrByte, regAddrByte, new byte[0]));
                //if (MySerialPort != null)
                //{
                //    MySerialPort.SendCommand(GetSendData(devAddrByte, regAddrByte, new byte[0])); //debug 55 06 80 E5 
                //    Thread.Sleep(100);

                //}
            }
        }

        [Description("清除所有地址的寄存器读取结果")]
        public void ClearAllDevRegReadResult()
        {
            for (var i = 0; i < 16; i++)
            {
                var devAddrByte = Convert.ToByte(i.ToString());
                var addrByte = devAddrByte;
                foreach (var f in from f in GetType().GetFields()
                                  let findF = string.Format("DevAddr{0}hRegAddr",
                                  ValueHelper.GetHextStr(addrByte))
                                  where f.Name.StartsWith(findF)
                                  select f)
                    f.SetValue(this, string.Empty);
            }
        }

        #endregion

        #region REG字段

        [Description("R,DevAddr00hRegAddr00h")]
        public string DevAddr00hRegAddr00h;
        [Description("R,DevAddr00hRegAddr01h")]
        public string DevAddr00hRegAddr01h;
        [Description("R,DevAddr00hRegAddr02h")]
        public string DevAddr00hRegAddr02h;
        [Description("R,DevAddr00hRegAddr03h")]
        public string DevAddr00hRegAddr03h;
        [Description("R,DevAddr00hRegAddr04h")]
        public string DevAddr00hRegAddr04h;
        [Description("R,DevAddr00hRegAddr05h")]
        public string DevAddr00hRegAddr05h;
        [Description("R,DevAddr00hRegAddr06h")]
        public string DevAddr00hRegAddr06h;
        [Description("R,DevAddr00hRegAddr07h")]
        public string DevAddr00hRegAddr07h;
        [Description("R,DevAddr00hRegAddr08h")]
        public string DevAddr00hRegAddr08h;
        [Description("R,DevAddr00hRegAddr09h")]
        public string DevAddr00hRegAddr09h;
        [Description("R,DevAddr00hRegAddr0Ah")]
        public string DevAddr00hRegAddr0Ah;
        [Description("R,DevAddr00hRegAddr0Bh")]
        public string DevAddr00hRegAddr0Bh;
        [Description("R,DevAddr00hRegAddr0Ch")]
        public string DevAddr00hRegAddr0Ch;
        [Description("R,DevAddr00hRegAddr0Dh")]
        public string DevAddr00hRegAddr0Dh;
        [Description("R,DevAddr00hRegAddr0Eh")]
        public string DevAddr00hRegAddr0Eh;
        [Description("R,DevAddr00hRegAddr0Fh")]
        public string DevAddr00hRegAddr0Fh;
        [Description("R,DevAddr00hRegAddr10h")]
        public string DevAddr00hRegAddr10h;
        [Description("R,DevAddr00hRegAddr11h")]
        public string DevAddr00hRegAddr11h;
        [Description("R,DevAddr00hRegAddr12h")]
        public string DevAddr00hRegAddr12h;
        [Description("R,DevAddr00hRegAddr13h")]
        public string DevAddr00hRegAddr13h;
        [Description("R,DevAddr00hRegAddr14h")]
        public string DevAddr00hRegAddr14h;
        [Description("R,DevAddr00hRegAddr15h")]
        public string DevAddr00hRegAddr15h;
        [Description("R,DevAddr00hRegAddr16h")]
        public string DevAddr00hRegAddr16h;
        [Description("R,DevAddr00hRegAddr17h")]
        public string DevAddr00hRegAddr17h;
        [Description("R,DevAddr00hRegAddr20h")]
        public string DevAddr00hRegAddr20h;
        [Description("R,DevAddr00hRegAddr21h")]
        public string DevAddr00hRegAddr21h;
        [Description("R,DevAddr00hRegAddr22h")]
        public string DevAddr00hRegAddr22h;
        [Description("R,DevAddr00hRegAddr23h")]
        public string DevAddr00hRegAddr23h;
        [Description("R,DevAddr00hRegAddr24h")]
        public string DevAddr00hRegAddr24h;
        [Description("R,DevAddr00hRegAddr25h")]
        public string DevAddr00hRegAddr25h;
        [Description("R,DevAddr00hRegAddr26h")]
        public string DevAddr00hRegAddr26h;
        [Description("R,DevAddr00hRegAddr27h")]
        public string DevAddr00hRegAddr27h;
        [Description("R,DevAddr00hRegAddr28h")]
        public string DevAddr00hRegAddr28h;
        [Description("R,DevAddr00hRegAddr29h")]
        public string DevAddr00hRegAddr29h;
        [Description("R,DevAddr00hRegAddr2Ah")]
        public string DevAddr00hRegAddr2Ah;
        [Description("R,DevAddr00hRegAddr2Bh")]
        public string DevAddr00hRegAddr2Bh;
        [Description("R,DevAddr00hRegAddr2Ch")]
        public string DevAddr00hRegAddr2Ch;
        [Description("R,DevAddr00hRegAddr2Dh")]
        public string DevAddr00hRegAddr2Dh;
        [Description("R,DevAddr00hRegAddr2Eh")]
        public string DevAddr00hRegAddr2Eh;
        [Description("R,DevAddr00hRegAddr2Fh")]
        public string DevAddr00hRegAddr2Fh;
        [Description("R,DevAddr00hRegAddr30h")]
        public string DevAddr00hRegAddr30h;
        [Description("R,DevAddr00hRegAddr31h")]
        public string DevAddr00hRegAddr31h;
        [Description("R,DevAddr00hRegAddr32h")]
        public string DevAddr00hRegAddr32h;
        [Description("R,DevAddr00hRegAddr33h")]
        public string DevAddr00hRegAddr33h;
        [Description("R,DevAddr00hRegAddr34h")]
        public string DevAddr00hRegAddr34h;
        [Description("R,DevAddr00hRegAddr35h")]
        public string DevAddr00hRegAddr35h;
        [Description("R,DevAddr00hRegAddr36h")]
        public string DevAddr00hRegAddr36h;
        [Description("R,DevAddr00hRegAddr37h")]
        public string DevAddr00hRegAddr37h;
        [Description("R,DevAddr00hRegAddr40h")]
        public string DevAddr00hRegAddr40h;
        [Description("R,DevAddr00hRegAddr41h")]
        public string DevAddr00hRegAddr41h;
        [Description("R,DevAddr00hRegAddr42h")]
        public string DevAddr00hRegAddr42h;
        [Description("R,DevAddr00hRegAddr43h")]
        public string DevAddr00hRegAddr43h;
        [Description("R,DevAddr00hRegAddr44h")]
        public string DevAddr00hRegAddr44h;
        [Description("R,DevAddr00hRegAddr50h")]
        public string DevAddr00hRegAddr50h;
        [Description("R,DevAddr00hRegAddr51h")]
        public string DevAddr00hRegAddr51h;
        [Description("R,DevAddr00hRegAddr52h")]
        public string DevAddr00hRegAddr52h;
        [Description("R,DevAddr00hRegAddr53h")]
        public string DevAddr00hRegAddr53h;
        [Description("R,DevAddr00hRegAddr54h")]
        public string DevAddr00hRegAddr54h;
        [Description("R,DevAddr00hRegAddr55h")]
        public string DevAddr00hRegAddr55h;
        [Description("R,DevAddr00hRegAddr56h")]
        public string DevAddr00hRegAddr56h;
        [Description("R,DevAddr00hRegAddr57h")]
        public string DevAddr00hRegAddr57h;
        [Description("R,DevAddr00hRegAddr58h")]
        public string DevAddr00hRegAddr58h;
        [Description("R,DevAddr00hRegAddr59h")]
        public string DevAddr00hRegAddr59h;
        [Description("R,DevAddr00hRegAddr5Ah")]
        public string DevAddr00hRegAddr5Ah;
        [Description("R,DevAddr00hRegAddr5Bh")]
        public string DevAddr00hRegAddr5Bh;
        [Description("R,DevAddr00hRegAddr5Ch")]
        public string DevAddr00hRegAddr5Ch;
        [Description("R,DevAddr00hRegAddr5Dh")]
        public string DevAddr00hRegAddr5Dh;
        [Description("R,DevAddr00hRegAddr5Eh")]
        public string DevAddr00hRegAddr5Eh;
        [Description("R,DevAddr00hRegAddr5Fh")]
        public string DevAddr00hRegAddr5Fh;
        [Description("R,DevAddr00hRegAddr60h")]
        public string DevAddr00hRegAddr60h;
        [Description("R,DevAddr00hRegAddr61h")]
        public string DevAddr00hRegAddr61h;
        [Description("R,DevAddr00hRegAddr62h")]
        public string DevAddr00hRegAddr62h;
        [Description("R,DevAddr00hRegAddr63h")]
        public string DevAddr00hRegAddr63h;
        [Description("R,DevAddr00hRegAddr64h")]
        public string DevAddr00hRegAddr64h;
        [Description("R,DevAddr00hRegAddr65h")]
        public string DevAddr00hRegAddr65h;
        [Description("R,DevAddr00hRegAddr66h")]
        public string DevAddr00hRegAddr66h;
        [Description("R,DevAddr00hRegAddr67h")]
        public string DevAddr00hRegAddr67h;
        [Description("R,DevAddr00hRegAddr70h")]
        public string DevAddr00hRegAddr70h;
        [Description("R,DevAddr00hRegAddr71h")]
        public string DevAddr00hRegAddr71h;
        [Description("R,DevAddr00hRegAddr72h")]
        public string DevAddr00hRegAddr72h;
        [Description("R,DevAddr00hRegAddr73h")]
        public string DevAddr00hRegAddr73h;
        [Description("R,DevAddr00hRegAddr74h")]
        public string DevAddr00hRegAddr74h;
        [Description("R,DevAddr00hRegAddr75h")]
        public string DevAddr00hRegAddr75h;
        [Description("R,DevAddr00hRegAddr76h")]
        public string DevAddr00hRegAddr76h;
        [Description("R,DevAddr00hRegAddr77h")]
        public string DevAddr00hRegAddr77h;
        [Description("R,DevAddr00hRegAddr78h")]
        public string DevAddr00hRegAddr78h;
        [Description("R,DevAddr00hRegAddr79h")]
        public string DevAddr00hRegAddr79h;
        [Description("R,DevAddr00hRegAddr7Ah")]
        public string DevAddr00hRegAddr7Ah;
        [Description("R,DevAddr00hRegAddr7Bh")]
        public string DevAddr00hRegAddr7Bh;
        [Description("R,DevAddr00hRegAddr7Ch")]
        public string DevAddr00hRegAddr7Ch;
        [Description("R,DevAddr00hRegAddr7Dh")]
        public string DevAddr00hRegAddr7Dh;
        [Description("R,DevAddr00hRegAddr7Eh")]
        public string DevAddr00hRegAddr7Eh;
        [Description("R,DevAddr00hRegAddr7Fh")]
        public string DevAddr00hRegAddr7Fh;
        [Description("R,DevAddr00hRegAddr80h")]
        public string DevAddr00hRegAddr80h;
        [Description("R,DevAddr00hRegAddr81h")]
        public string DevAddr00hRegAddr81h;
        [Description("R,DevAddr00hRegAddr82h")]
        public string DevAddr00hRegAddr82h;
        [Description("R,DevAddr00hRegAddr83h")]
        public string DevAddr00hRegAddr83h;
        [Description("R,DevAddr00hRegAddr84h")]
        public string DevAddr00hRegAddr84h;
        [Description("R,DevAddr00hRegAddr85h")]
        public string DevAddr00hRegAddr85h;
        [Description("R,DevAddr00hRegAddr86h")]
        public string DevAddr00hRegAddr86h;
        [Description("R,DevAddr00hRegAddr87h")]
        public string DevAddr00hRegAddr87h;

        [Description("R,DevAddr01hRegAddr00h")]
        public string DevAddr01hRegAddr00h;
        [Description("R,DevAddr01hRegAddr01h")]
        public string DevAddr01hRegAddr01h;
        [Description("R,DevAddr01hRegAddr02h")]
        public string DevAddr01hRegAddr02h;
        [Description("R,DevAddr01hRegAddr03h")]
        public string DevAddr01hRegAddr03h;
        [Description("R,DevAddr01hRegAddr04h")]
        public string DevAddr01hRegAddr04h;
        [Description("R,DevAddr01hRegAddr05h")]
        public string DevAddr01hRegAddr05h;
        [Description("R,DevAddr01hRegAddr06h")]
        public string DevAddr01hRegAddr06h;
        [Description("R,DevAddr01hRegAddr07h")]
        public string DevAddr01hRegAddr07h;
        [Description("R,DevAddr01hRegAddr08h")]
        public string DevAddr01hRegAddr08h;
        [Description("R,DevAddr01hRegAddr09h")]
        public string DevAddr01hRegAddr09h;
        [Description("R,DevAddr01hRegAddr0Ah")]
        public string DevAddr01hRegAddr0Ah;
        [Description("R,DevAddr01hRegAddr0Bh")]
        public string DevAddr01hRegAddr0Bh;
        [Description("R,DevAddr01hRegAddr0Ch")]
        public string DevAddr01hRegAddr0Ch;
        [Description("R,DevAddr01hRegAddr0Dh")]
        public string DevAddr01hRegAddr0Dh;
        [Description("R,DevAddr01hRegAddr0Eh")]
        public string DevAddr01hRegAddr0Eh;
        [Description("R,DevAddr01hRegAddr0Fh")]
        public string DevAddr01hRegAddr0Fh;
        [Description("R,DevAddr01hRegAddr10h")]
        public string DevAddr01hRegAddr10h;
        [Description("R,DevAddr01hRegAddr11h")]
        public string DevAddr01hRegAddr11h;
        [Description("R,DevAddr01hRegAddr12h")]
        public string DevAddr01hRegAddr12h;
        [Description("R,DevAddr01hRegAddr13h")]
        public string DevAddr01hRegAddr13h;
        [Description("R,DevAddr01hRegAddr14h")]
        public string DevAddr01hRegAddr14h;
        [Description("R,DevAddr01hRegAddr15h")]
        public string DevAddr01hRegAddr15h;
        [Description("R,DevAddr01hRegAddr16h")]
        public string DevAddr01hRegAddr16h;
        [Description("R,DevAddr01hRegAddr17h")]
        public string DevAddr01hRegAddr17h;
        [Description("R,DevAddr01hRegAddr20h")]
        public string DevAddr01hRegAddr20h;
        [Description("R,DevAddr01hRegAddr21h")]
        public string DevAddr01hRegAddr21h;
        [Description("R,DevAddr01hRegAddr22h")]
        public string DevAddr01hRegAddr22h;
        [Description("R,DevAddr01hRegAddr23h")]
        public string DevAddr01hRegAddr23h;
        [Description("R,DevAddr01hRegAddr24h")]
        public string DevAddr01hRegAddr24h;
        [Description("R,DevAddr01hRegAddr25h")]
        public string DevAddr01hRegAddr25h;
        [Description("R,DevAddr01hRegAddr26h")]
        public string DevAddr01hRegAddr26h;
        [Description("R,DevAddr01hRegAddr27h")]
        public string DevAddr01hRegAddr27h;
        [Description("R,DevAddr01hRegAddr28h")]
        public string DevAddr01hRegAddr28h;
        [Description("R,DevAddr01hRegAddr29h")]
        public string DevAddr01hRegAddr29h;
        [Description("R,DevAddr01hRegAddr2Ah")]
        public string DevAddr01hRegAddr2Ah;
        [Description("R,DevAddr01hRegAddr2Bh")]
        public string DevAddr01hRegAddr2Bh;
        [Description("R,DevAddr01hRegAddr2Ch")]
        public string DevAddr01hRegAddr2Ch;
        [Description("R,DevAddr01hRegAddr2Dh")]
        public string DevAddr01hRegAddr2Dh;
        [Description("R,DevAddr01hRegAddr2Eh")]
        public string DevAddr01hRegAddr2Eh;
        [Description("R,DevAddr01hRegAddr2Fh")]
        public string DevAddr01hRegAddr2Fh;
        [Description("R,DevAddr01hRegAddr30h")]
        public string DevAddr01hRegAddr30h;
        [Description("R,DevAddr01hRegAddr31h")]
        public string DevAddr01hRegAddr31h;
        [Description("R,DevAddr01hRegAddr32h")]
        public string DevAddr01hRegAddr32h;
        [Description("R,DevAddr01hRegAddr33h")]
        public string DevAddr01hRegAddr33h;
        [Description("R,DevAddr01hRegAddr34h")]
        public string DevAddr01hRegAddr34h;
        [Description("R,DevAddr01hRegAddr35h")]
        public string DevAddr01hRegAddr35h;
        [Description("R,DevAddr01hRegAddr36h")]
        public string DevAddr01hRegAddr36h;
        [Description("R,DevAddr01hRegAddr37h")]
        public string DevAddr01hRegAddr37h;
        [Description("R,DevAddr01hRegAddr40h")]
        public string DevAddr01hRegAddr40h;
        [Description("R,DevAddr01hRegAddr41h")]
        public string DevAddr01hRegAddr41h;
        [Description("R,DevAddr01hRegAddr42h")]
        public string DevAddr01hRegAddr42h;
        [Description("R,DevAddr01hRegAddr43h")]
        public string DevAddr01hRegAddr43h;
        [Description("R,DevAddr01hRegAddr44h")]
        public string DevAddr01hRegAddr44h;
        [Description("R,DevAddr01hRegAddr50h")]
        public string DevAddr01hRegAddr50h;
        [Description("R,DevAddr01hRegAddr51h")]
        public string DevAddr01hRegAddr51h;
        [Description("R,DevAddr01hRegAddr52h")]
        public string DevAddr01hRegAddr52h;
        [Description("R,DevAddr01hRegAddr53h")]
        public string DevAddr01hRegAddr53h;
        [Description("R,DevAddr01hRegAddr54h")]
        public string DevAddr01hRegAddr54h;
        [Description("R,DevAddr01hRegAddr55h")]
        public string DevAddr01hRegAddr55h;
        [Description("R,DevAddr01hRegAddr56h")]
        public string DevAddr01hRegAddr56h;
        [Description("R,DevAddr01hRegAddr57h")]
        public string DevAddr01hRegAddr57h;
        [Description("R,DevAddr01hRegAddr58h")]
        public string DevAddr01hRegAddr58h;
        [Description("R,DevAddr01hRegAddr59h")]
        public string DevAddr01hRegAddr59h;
        [Description("R,DevAddr01hRegAddr5Ah")]
        public string DevAddr01hRegAddr5Ah;
        [Description("R,DevAddr01hRegAddr5Bh")]
        public string DevAddr01hRegAddr5Bh;
        [Description("R,DevAddr01hRegAddr5Ch")]
        public string DevAddr01hRegAddr5Ch;
        [Description("R,DevAddr01hRegAddr5Dh")]
        public string DevAddr01hRegAddr5Dh;
        [Description("R,DevAddr01hRegAddr5Eh")]
        public string DevAddr01hRegAddr5Eh;
        [Description("R,DevAddr01hRegAddr5Fh")]
        public string DevAddr01hRegAddr5Fh;
        [Description("R,DevAddr01hRegAddr60h")]
        public string DevAddr01hRegAddr60h;
        [Description("R,DevAddr01hRegAddr61h")]
        public string DevAddr01hRegAddr61h;
        [Description("R,DevAddr01hRegAddr62h")]
        public string DevAddr01hRegAddr62h;
        [Description("R,DevAddr01hRegAddr63h")]
        public string DevAddr01hRegAddr63h;
        [Description("R,DevAddr01hRegAddr64h")]
        public string DevAddr01hRegAddr64h;
        [Description("R,DevAddr01hRegAddr65h")]
        public string DevAddr01hRegAddr65h;
        [Description("R,DevAddr01hRegAddr66h")]
        public string DevAddr01hRegAddr66h;
        [Description("R,DevAddr01hRegAddr67h")]
        public string DevAddr01hRegAddr67h;
        [Description("R,DevAddr01hRegAddr70h")]
        public string DevAddr01hRegAddr70h;
        [Description("R,DevAddr01hRegAddr71h")]
        public string DevAddr01hRegAddr71h;
        [Description("R,DevAddr01hRegAddr72h")]
        public string DevAddr01hRegAddr72h;
        [Description("R,DevAddr01hRegAddr73h")]
        public string DevAddr01hRegAddr73h;
        [Description("R,DevAddr01hRegAddr74h")]
        public string DevAddr01hRegAddr74h;
        [Description("R,DevAddr01hRegAddr75h")]
        public string DevAddr01hRegAddr75h;
        [Description("R,DevAddr01hRegAddr76h")]
        public string DevAddr01hRegAddr76h;
        [Description("R,DevAddr01hRegAddr77h")]
        public string DevAddr01hRegAddr77h;
        [Description("R,DevAddr01hRegAddr78h")]
        public string DevAddr01hRegAddr78h;
        [Description("R,DevAddr01hRegAddr79h")]
        public string DevAddr01hRegAddr79h;
        [Description("R,DevAddr01hRegAddr7Ah")]
        public string DevAddr01hRegAddr7Ah;
        [Description("R,DevAddr01hRegAddr7Bh")]
        public string DevAddr01hRegAddr7Bh;
        [Description("R,DevAddr01hRegAddr7Ch")]
        public string DevAddr01hRegAddr7Ch;
        [Description("R,DevAddr01hRegAddr7Dh")]
        public string DevAddr01hRegAddr7Dh;
        [Description("R,DevAddr01hRegAddr7Eh")]
        public string DevAddr01hRegAddr7Eh;
        [Description("R,DevAddr01hRegAddr7Fh")]
        public string DevAddr01hRegAddr7Fh;
        [Description("R,DevAddr01hRegAddr80h")]
        public string DevAddr01hRegAddr80h;
        [Description("R,DevAddr01hRegAddr81h")]
        public string DevAddr01hRegAddr81h;
        [Description("R,DevAddr01hRegAddr82h")]
        public string DevAddr01hRegAddr82h;
        [Description("R,DevAddr01hRegAddr83h")]
        public string DevAddr01hRegAddr83h;
        [Description("R,DevAddr01hRegAddr84h")]
        public string DevAddr01hRegAddr84h;
        [Description("R,DevAddr01hRegAddr85h")]
        public string DevAddr01hRegAddr85h;
        [Description("R,DevAddr01hRegAddr86h")]
        public string DevAddr01hRegAddr86h;
        [Description("R,DevAddr01hRegAddr87h")]
        public string DevAddr01hRegAddr87h;

        [Description("R,DevAddr02hRegAddr00h")]
        public string DevAddr02hRegAddr00h;
        [Description("R,DevAddr02hRegAddr01h")]
        public string DevAddr02hRegAddr01h;
        [Description("R,DevAddr02hRegAddr02h")]
        public string DevAddr02hRegAddr02h;
        [Description("R,DevAddr02hRegAddr03h")]
        public string DevAddr02hRegAddr03h;
        [Description("R,DevAddr02hRegAddr04h")]
        public string DevAddr02hRegAddr04h;
        [Description("R,DevAddr02hRegAddr05h")]
        public string DevAddr02hRegAddr05h;
        [Description("R,DevAddr02hRegAddr06h")]
        public string DevAddr02hRegAddr06h;
        [Description("R,DevAddr02hRegAddr07h")]
        public string DevAddr02hRegAddr07h;
        [Description("R,DevAddr02hRegAddr08h")]
        public string DevAddr02hRegAddr08h;
        [Description("R,DevAddr02hRegAddr09h")]
        public string DevAddr02hRegAddr09h;
        [Description("R,DevAddr02hRegAddr0Ah")]
        public string DevAddr02hRegAddr0Ah;
        [Description("R,DevAddr02hRegAddr0Bh")]
        public string DevAddr02hRegAddr0Bh;
        [Description("R,DevAddr02hRegAddr0Ch")]
        public string DevAddr02hRegAddr0Ch;
        [Description("R,DevAddr02hRegAddr0Dh")]
        public string DevAddr02hRegAddr0Dh;
        [Description("R,DevAddr02hRegAddr0Eh")]
        public string DevAddr02hRegAddr0Eh;
        [Description("R,DevAddr02hRegAddr0Fh")]
        public string DevAddr02hRegAddr0Fh;
        [Description("R,DevAddr02hRegAddr10h")]
        public string DevAddr02hRegAddr10h;
        [Description("R,DevAddr02hRegAddr11h")]
        public string DevAddr02hRegAddr11h;
        [Description("R,DevAddr02hRegAddr12h")]
        public string DevAddr02hRegAddr12h;
        [Description("R,DevAddr02hRegAddr13h")]
        public string DevAddr02hRegAddr13h;
        [Description("R,DevAddr02hRegAddr14h")]
        public string DevAddr02hRegAddr14h;
        [Description("R,DevAddr02hRegAddr15h")]
        public string DevAddr02hRegAddr15h;
        [Description("R,DevAddr02hRegAddr16h")]
        public string DevAddr02hRegAddr16h;
        [Description("R,DevAddr02hRegAddr17h")]
        public string DevAddr02hRegAddr17h;
        [Description("R,DevAddr02hRegAddr20h")]
        public string DevAddr02hRegAddr20h;
        [Description("R,DevAddr02hRegAddr21h")]
        public string DevAddr02hRegAddr21h;
        [Description("R,DevAddr02hRegAddr22h")]
        public string DevAddr02hRegAddr22h;
        [Description("R,DevAddr02hRegAddr23h")]
        public string DevAddr02hRegAddr23h;
        [Description("R,DevAddr02hRegAddr24h")]
        public string DevAddr02hRegAddr24h;
        [Description("R,DevAddr02hRegAddr25h")]
        public string DevAddr02hRegAddr25h;
        [Description("R,DevAddr02hRegAddr26h")]
        public string DevAddr02hRegAddr26h;
        [Description("R,DevAddr02hRegAddr27h")]
        public string DevAddr02hRegAddr27h;
        [Description("R,DevAddr02hRegAddr28h")]
        public string DevAddr02hRegAddr28h;
        [Description("R,DevAddr02hRegAddr29h")]
        public string DevAddr02hRegAddr29h;
        [Description("R,DevAddr02hRegAddr2Ah")]
        public string DevAddr02hRegAddr2Ah;
        [Description("R,DevAddr02hRegAddr2Bh")]
        public string DevAddr02hRegAddr2Bh;
        [Description("R,DevAddr02hRegAddr2Ch")]
        public string DevAddr02hRegAddr2Ch;
        [Description("R,DevAddr02hRegAddr2Dh")]
        public string DevAddr02hRegAddr2Dh;
        [Description("R,DevAddr02hRegAddr2Eh")]
        public string DevAddr02hRegAddr2Eh;
        [Description("R,DevAddr02hRegAddr2Fh")]
        public string DevAddr02hRegAddr2Fh;
        [Description("R,DevAddr02hRegAddr30h")]
        public string DevAddr02hRegAddr30h;
        [Description("R,DevAddr02hRegAddr31h")]
        public string DevAddr02hRegAddr31h;
        [Description("R,DevAddr02hRegAddr32h")]
        public string DevAddr02hRegAddr32h;
        [Description("R,DevAddr02hRegAddr33h")]
        public string DevAddr02hRegAddr33h;
        [Description("R,DevAddr02hRegAddr34h")]
        public string DevAddr02hRegAddr34h;
        [Description("R,DevAddr02hRegAddr35h")]
        public string DevAddr02hRegAddr35h;
        [Description("R,DevAddr02hRegAddr36h")]
        public string DevAddr02hRegAddr36h;
        [Description("R,DevAddr02hRegAddr37h")]
        public string DevAddr02hRegAddr37h;
        [Description("R,DevAddr02hRegAddr40h")]
        public string DevAddr02hRegAddr40h;
        [Description("R,DevAddr02hRegAddr41h")]
        public string DevAddr02hRegAddr41h;
        [Description("R,DevAddr02hRegAddr42h")]
        public string DevAddr02hRegAddr42h;
        [Description("R,DevAddr02hRegAddr43h")]
        public string DevAddr02hRegAddr43h;
        [Description("R,DevAddr02hRegAddr44h")]
        public string DevAddr02hRegAddr44h;
        [Description("R,DevAddr02hRegAddr50h")]
        public string DevAddr02hRegAddr50h;
        [Description("R,DevAddr02hRegAddr51h")]
        public string DevAddr02hRegAddr51h;
        [Description("R,DevAddr02hRegAddr52h")]
        public string DevAddr02hRegAddr52h;
        [Description("R,DevAddr02hRegAddr53h")]
        public string DevAddr02hRegAddr53h;
        [Description("R,DevAddr02hRegAddr54h")]
        public string DevAddr02hRegAddr54h;
        [Description("R,DevAddr02hRegAddr55h")]
        public string DevAddr02hRegAddr55h;
        [Description("R,DevAddr02hRegAddr56h")]
        public string DevAddr02hRegAddr56h;
        [Description("R,DevAddr02hRegAddr57h")]
        public string DevAddr02hRegAddr57h;
        [Description("R,DevAddr02hRegAddr58h")]
        public string DevAddr02hRegAddr58h;
        [Description("R,DevAddr02hRegAddr59h")]
        public string DevAddr02hRegAddr59h;
        [Description("R,DevAddr02hRegAddr5Ah")]
        public string DevAddr02hRegAddr5Ah;
        [Description("R,DevAddr02hRegAddr5Bh")]
        public string DevAddr02hRegAddr5Bh;
        [Description("R,DevAddr02hRegAddr5Ch")]
        public string DevAddr02hRegAddr5Ch;
        [Description("R,DevAddr02hRegAddr5Dh")]
        public string DevAddr02hRegAddr5Dh;
        [Description("R,DevAddr02hRegAddr5Eh")]
        public string DevAddr02hRegAddr5Eh;
        [Description("R,DevAddr02hRegAddr5Fh")]
        public string DevAddr02hRegAddr5Fh;
        [Description("R,DevAddr02hRegAddr60h")]
        public string DevAddr02hRegAddr60h;
        [Description("R,DevAddr02hRegAddr61h")]
        public string DevAddr02hRegAddr61h;
        [Description("R,DevAddr02hRegAddr62h")]
        public string DevAddr02hRegAddr62h;
        [Description("R,DevAddr02hRegAddr63h")]
        public string DevAddr02hRegAddr63h;
        [Description("R,DevAddr02hRegAddr64h")]
        public string DevAddr02hRegAddr64h;
        [Description("R,DevAddr02hRegAddr65h")]
        public string DevAddr02hRegAddr65h;
        [Description("R,DevAddr02hRegAddr66h")]
        public string DevAddr02hRegAddr66h;
        [Description("R,DevAddr02hRegAddr67h")]
        public string DevAddr02hRegAddr67h;
        [Description("R,DevAddr02hRegAddr70h")]
        public string DevAddr02hRegAddr70h;
        [Description("R,DevAddr02hRegAddr71h")]
        public string DevAddr02hRegAddr71h;
        [Description("R,DevAddr02hRegAddr72h")]
        public string DevAddr02hRegAddr72h;
        [Description("R,DevAddr02hRegAddr73h")]
        public string DevAddr02hRegAddr73h;
        [Description("R,DevAddr02hRegAddr74h")]
        public string DevAddr02hRegAddr74h;
        [Description("R,DevAddr02hRegAddr75h")]
        public string DevAddr02hRegAddr75h;
        [Description("R,DevAddr02hRegAddr76h")]
        public string DevAddr02hRegAddr76h;
        [Description("R,DevAddr02hRegAddr77h")]
        public string DevAddr02hRegAddr77h;
        [Description("R,DevAddr02hRegAddr78h")]
        public string DevAddr02hRegAddr78h;
        [Description("R,DevAddr02hRegAddr79h")]
        public string DevAddr02hRegAddr79h;
        [Description("R,DevAddr02hRegAddr7Ah")]
        public string DevAddr02hRegAddr7Ah;
        [Description("R,DevAddr02hRegAddr7Bh")]
        public string DevAddr02hRegAddr7Bh;
        [Description("R,DevAddr02hRegAddr7Ch")]
        public string DevAddr02hRegAddr7Ch;
        [Description("R,DevAddr02hRegAddr7Dh")]
        public string DevAddr02hRegAddr7Dh;
        [Description("R,DevAddr02hRegAddr7Eh")]
        public string DevAddr02hRegAddr7Eh;
        [Description("R,DevAddr02hRegAddr7Fh")]
        public string DevAddr02hRegAddr7Fh;
        [Description("R,DevAddr02hRegAddr80h")]
        public string DevAddr02hRegAddr80h;
        [Description("R,DevAddr02hRegAddr81h")]
        public string DevAddr02hRegAddr81h;
        [Description("R,DevAddr02hRegAddr82h")]
        public string DevAddr02hRegAddr82h;
        [Description("R,DevAddr02hRegAddr83h")]
        public string DevAddr02hRegAddr83h;
        [Description("R,DevAddr02hRegAddr84h")]
        public string DevAddr02hRegAddr84h;
        [Description("R,DevAddr02hRegAddr85h")]
        public string DevAddr02hRegAddr85h;
        [Description("R,DevAddr02hRegAddr86h")]
        public string DevAddr02hRegAddr86h;
        [Description("R,DevAddr02hRegAddr87h")]
        public string DevAddr02hRegAddr87h;

        [Description("R,DevAddr03hRegAddr00h")]
        public string DevAddr03hRegAddr00h;
        [Description("R,DevAddr03hRegAddr01h")]
        public string DevAddr03hRegAddr01h;
        [Description("R,DevAddr03hRegAddr02h")]
        public string DevAddr03hRegAddr02h;
        [Description("R,DevAddr03hRegAddr03h")]
        public string DevAddr03hRegAddr03h;
        [Description("R,DevAddr03hRegAddr04h")]
        public string DevAddr03hRegAddr04h;
        [Description("R,DevAddr03hRegAddr05h")]
        public string DevAddr03hRegAddr05h;
        [Description("R,DevAddr03hRegAddr06h")]
        public string DevAddr03hRegAddr06h;
        [Description("R,DevAddr03hRegAddr07h")]
        public string DevAddr03hRegAddr07h;
        [Description("R,DevAddr03hRegAddr08h")]
        public string DevAddr03hRegAddr08h;
        [Description("R,DevAddr03hRegAddr09h")]
        public string DevAddr03hRegAddr09h;
        [Description("R,DevAddr03hRegAddr0Ah")]
        public string DevAddr03hRegAddr0Ah;
        [Description("R,DevAddr03hRegAddr0Bh")]
        public string DevAddr03hRegAddr0Bh;
        [Description("R,DevAddr03hRegAddr0Ch")]
        public string DevAddr03hRegAddr0Ch;
        [Description("R,DevAddr03hRegAddr0Dh")]
        public string DevAddr03hRegAddr0Dh;
        [Description("R,DevAddr03hRegAddr0Eh")]
        public string DevAddr03hRegAddr0Eh;
        [Description("R,DevAddr03hRegAddr0Fh")]
        public string DevAddr03hRegAddr0Fh;
        [Description("R,DevAddr03hRegAddr10h")]
        public string DevAddr03hRegAddr10h;
        [Description("R,DevAddr03hRegAddr11h")]
        public string DevAddr03hRegAddr11h;
        [Description("R,DevAddr03hRegAddr12h")]
        public string DevAddr03hRegAddr12h;
        [Description("R,DevAddr03hRegAddr13h")]
        public string DevAddr03hRegAddr13h;
        [Description("R,DevAddr03hRegAddr14h")]
        public string DevAddr03hRegAddr14h;
        [Description("R,DevAddr03hRegAddr15h")]
        public string DevAddr03hRegAddr15h;
        [Description("R,DevAddr03hRegAddr16h")]
        public string DevAddr03hRegAddr16h;
        [Description("R,DevAddr03hRegAddr17h")]
        public string DevAddr03hRegAddr17h;
        [Description("R,DevAddr03hRegAddr20h")]
        public string DevAddr03hRegAddr20h;
        [Description("R,DevAddr03hRegAddr21h")]
        public string DevAddr03hRegAddr21h;
        [Description("R,DevAddr03hRegAddr22h")]
        public string DevAddr03hRegAddr22h;
        [Description("R,DevAddr03hRegAddr23h")]
        public string DevAddr03hRegAddr23h;
        [Description("R,DevAddr03hRegAddr24h")]
        public string DevAddr03hRegAddr24h;
        [Description("R,DevAddr03hRegAddr25h")]
        public string DevAddr03hRegAddr25h;
        [Description("R,DevAddr03hRegAddr26h")]
        public string DevAddr03hRegAddr26h;
        [Description("R,DevAddr03hRegAddr27h")]
        public string DevAddr03hRegAddr27h;
        [Description("R,DevAddr03hRegAddr28h")]
        public string DevAddr03hRegAddr28h;
        [Description("R,DevAddr03hRegAddr29h")]
        public string DevAddr03hRegAddr29h;
        [Description("R,DevAddr03hRegAddr2Ah")]
        public string DevAddr03hRegAddr2Ah;
        [Description("R,DevAddr03hRegAddr2Bh")]
        public string DevAddr03hRegAddr2Bh;
        [Description("R,DevAddr03hRegAddr2Ch")]
        public string DevAddr03hRegAddr2Ch;
        [Description("R,DevAddr03hRegAddr2Dh")]
        public string DevAddr03hRegAddr2Dh;
        [Description("R,DevAddr03hRegAddr2Eh")]
        public string DevAddr03hRegAddr2Eh;
        [Description("R,DevAddr03hRegAddr2Fh")]
        public string DevAddr03hRegAddr2Fh;
        [Description("R,DevAddr03hRegAddr30h")]
        public string DevAddr03hRegAddr30h;
        [Description("R,DevAddr03hRegAddr31h")]
        public string DevAddr03hRegAddr31h;
        [Description("R,DevAddr03hRegAddr32h")]
        public string DevAddr03hRegAddr32h;
        [Description("R,DevAddr03hRegAddr33h")]
        public string DevAddr03hRegAddr33h;
        [Description("R,DevAddr03hRegAddr34h")]
        public string DevAddr03hRegAddr34h;
        [Description("R,DevAddr03hRegAddr35h")]
        public string DevAddr03hRegAddr35h;
        [Description("R,DevAddr03hRegAddr36h")]
        public string DevAddr03hRegAddr36h;
        [Description("R,DevAddr03hRegAddr37h")]
        public string DevAddr03hRegAddr37h;
        [Description("R,DevAddr03hRegAddr40h")]
        public string DevAddr03hRegAddr40h;
        [Description("R,DevAddr03hRegAddr41h")]
        public string DevAddr03hRegAddr41h;
        [Description("R,DevAddr03hRegAddr42h")]
        public string DevAddr03hRegAddr42h;
        [Description("R,DevAddr03hRegAddr43h")]
        public string DevAddr03hRegAddr43h;
        [Description("R,DevAddr03hRegAddr44h")]
        public string DevAddr03hRegAddr44h;
        [Description("R,DevAddr03hRegAddr50h")]
        public string DevAddr03hRegAddr50h;
        [Description("R,DevAddr03hRegAddr51h")]
        public string DevAddr03hRegAddr51h;
        [Description("R,DevAddr03hRegAddr52h")]
        public string DevAddr03hRegAddr52h;
        [Description("R,DevAddr03hRegAddr53h")]
        public string DevAddr03hRegAddr53h;
        [Description("R,DevAddr03hRegAddr54h")]
        public string DevAddr03hRegAddr54h;
        [Description("R,DevAddr03hRegAddr55h")]
        public string DevAddr03hRegAddr55h;
        [Description("R,DevAddr03hRegAddr56h")]
        public string DevAddr03hRegAddr56h;
        [Description("R,DevAddr03hRegAddr57h")]
        public string DevAddr03hRegAddr57h;
        [Description("R,DevAddr03hRegAddr58h")]
        public string DevAddr03hRegAddr58h;
        [Description("R,DevAddr03hRegAddr59h")]
        public string DevAddr03hRegAddr59h;
        [Description("R,DevAddr03hRegAddr5Ah")]
        public string DevAddr03hRegAddr5Ah;
        [Description("R,DevAddr03hRegAddr5Bh")]
        public string DevAddr03hRegAddr5Bh;
        [Description("R,DevAddr03hRegAddr5Ch")]
        public string DevAddr03hRegAddr5Ch;
        [Description("R,DevAddr03hRegAddr5Dh")]
        public string DevAddr03hRegAddr5Dh;
        [Description("R,DevAddr03hRegAddr5Eh")]
        public string DevAddr03hRegAddr5Eh;
        [Description("R,DevAddr03hRegAddr5Fh")]
        public string DevAddr03hRegAddr5Fh;
        [Description("R,DevAddr03hRegAddr60h")]
        public string DevAddr03hRegAddr60h;
        [Description("R,DevAddr03hRegAddr61h")]
        public string DevAddr03hRegAddr61h;
        [Description("R,DevAddr03hRegAddr62h")]
        public string DevAddr03hRegAddr62h;
        [Description("R,DevAddr03hRegAddr63h")]
        public string DevAddr03hRegAddr63h;
        [Description("R,DevAddr03hRegAddr64h")]
        public string DevAddr03hRegAddr64h;
        [Description("R,DevAddr03hRegAddr65h")]
        public string DevAddr03hRegAddr65h;
        [Description("R,DevAddr03hRegAddr66h")]
        public string DevAddr03hRegAddr66h;
        [Description("R,DevAddr03hRegAddr67h")]
        public string DevAddr03hRegAddr67h;
        [Description("R,DevAddr03hRegAddr70h")]
        public string DevAddr03hRegAddr70h;
        [Description("R,DevAddr03hRegAddr71h")]
        public string DevAddr03hRegAddr71h;
        [Description("R,DevAddr03hRegAddr72h")]
        public string DevAddr03hRegAddr72h;
        [Description("R,DevAddr03hRegAddr73h")]
        public string DevAddr03hRegAddr73h;
        [Description("R,DevAddr03hRegAddr74h")]
        public string DevAddr03hRegAddr74h;
        [Description("R,DevAddr03hRegAddr75h")]
        public string DevAddr03hRegAddr75h;
        [Description("R,DevAddr03hRegAddr76h")]
        public string DevAddr03hRegAddr76h;
        [Description("R,DevAddr03hRegAddr77h")]
        public string DevAddr03hRegAddr77h;
        [Description("R,DevAddr03hRegAddr78h")]
        public string DevAddr03hRegAddr78h;
        [Description("R,DevAddr03hRegAddr79h")]
        public string DevAddr03hRegAddr79h;
        [Description("R,DevAddr03hRegAddr7Ah")]
        public string DevAddr03hRegAddr7Ah;
        [Description("R,DevAddr03hRegAddr7Bh")]
        public string DevAddr03hRegAddr7Bh;
        [Description("R,DevAddr03hRegAddr7Ch")]
        public string DevAddr03hRegAddr7Ch;
        [Description("R,DevAddr03hRegAddr7Dh")]
        public string DevAddr03hRegAddr7Dh;
        [Description("R,DevAddr03hRegAddr7Eh")]
        public string DevAddr03hRegAddr7Eh;
        [Description("R,DevAddr03hRegAddr7Fh")]
        public string DevAddr03hRegAddr7Fh;
        [Description("R,DevAddr03hRegAddr80h")]
        public string DevAddr03hRegAddr80h;
        [Description("R,DevAddr03hRegAddr81h")]
        public string DevAddr03hRegAddr81h;
        [Description("R,DevAddr03hRegAddr82h")]
        public string DevAddr03hRegAddr82h;
        [Description("R,DevAddr03hRegAddr83h")]
        public string DevAddr03hRegAddr83h;
        [Description("R,DevAddr03hRegAddr84h")]
        public string DevAddr03hRegAddr84h;
        [Description("R,DevAddr03hRegAddr85h")]
        public string DevAddr03hRegAddr85h;
        [Description("R,DevAddr03hRegAddr86h")]
        public string DevAddr03hRegAddr86h;
        [Description("R,DevAddr03hRegAddr87h")]
        public string DevAddr03hRegAddr87h;

        [Description("R,DevAddr04hRegAddr00h")]
        public string DevAddr04hRegAddr00h;
        [Description("R,DevAddr04hRegAddr01h")]
        public string DevAddr04hRegAddr01h;
        [Description("R,DevAddr04hRegAddr02h")]
        public string DevAddr04hRegAddr02h;
        [Description("R,DevAddr04hRegAddr03h")]
        public string DevAddr04hRegAddr03h;
        [Description("R,DevAddr04hRegAddr04h")]
        public string DevAddr04hRegAddr04h;
        [Description("R,DevAddr04hRegAddr05h")]
        public string DevAddr04hRegAddr05h;
        [Description("R,DevAddr04hRegAddr06h")]
        public string DevAddr04hRegAddr06h;
        [Description("R,DevAddr04hRegAddr07h")]
        public string DevAddr04hRegAddr07h;
        [Description("R,DevAddr04hRegAddr08h")]
        public string DevAddr04hRegAddr08h;
        [Description("R,DevAddr04hRegAddr09h")]
        public string DevAddr04hRegAddr09h;
        [Description("R,DevAddr04hRegAddr0Ah")]
        public string DevAddr04hRegAddr0Ah;
        [Description("R,DevAddr04hRegAddr0Bh")]
        public string DevAddr04hRegAddr0Bh;
        [Description("R,DevAddr04hRegAddr0Ch")]
        public string DevAddr04hRegAddr0Ch;
        [Description("R,DevAddr04hRegAddr0Dh")]
        public string DevAddr04hRegAddr0Dh;
        [Description("R,DevAddr04hRegAddr0Eh")]
        public string DevAddr04hRegAddr0Eh;
        [Description("R,DevAddr04hRegAddr0Fh")]
        public string DevAddr04hRegAddr0Fh;
        [Description("R,DevAddr04hRegAddr10h")]
        public string DevAddr04hRegAddr10h;
        [Description("R,DevAddr04hRegAddr11h")]
        public string DevAddr04hRegAddr11h;
        [Description("R,DevAddr04hRegAddr12h")]
        public string DevAddr04hRegAddr12h;
        [Description("R,DevAddr04hRegAddr13h")]
        public string DevAddr04hRegAddr13h;
        [Description("R,DevAddr04hRegAddr14h")]
        public string DevAddr04hRegAddr14h;
        [Description("R,DevAddr04hRegAddr15h")]
        public string DevAddr04hRegAddr15h;
        [Description("R,DevAddr04hRegAddr16h")]
        public string DevAddr04hRegAddr16h;
        [Description("R,DevAddr04hRegAddr17h")]
        public string DevAddr04hRegAddr17h;
        [Description("R,DevAddr04hRegAddr20h")]
        public string DevAddr04hRegAddr20h;
        [Description("R,DevAddr04hRegAddr21h")]
        public string DevAddr04hRegAddr21h;
        [Description("R,DevAddr04hRegAddr22h")]
        public string DevAddr04hRegAddr22h;
        [Description("R,DevAddr04hRegAddr23h")]
        public string DevAddr04hRegAddr23h;
        [Description("R,DevAddr04hRegAddr24h")]
        public string DevAddr04hRegAddr24h;
        [Description("R,DevAddr04hRegAddr25h")]
        public string DevAddr04hRegAddr25h;
        [Description("R,DevAddr04hRegAddr26h")]
        public string DevAddr04hRegAddr26h;
        [Description("R,DevAddr04hRegAddr27h")]
        public string DevAddr04hRegAddr27h;
        [Description("R,DevAddr04hRegAddr28h")]
        public string DevAddr04hRegAddr28h;
        [Description("R,DevAddr04hRegAddr29h")]
        public string DevAddr04hRegAddr29h;
        [Description("R,DevAddr04hRegAddr2Ah")]
        public string DevAddr04hRegAddr2Ah;
        [Description("R,DevAddr04hRegAddr2Bh")]
        public string DevAddr04hRegAddr2Bh;
        [Description("R,DevAddr04hRegAddr2Ch")]
        public string DevAddr04hRegAddr2Ch;
        [Description("R,DevAddr04hRegAddr2Dh")]
        public string DevAddr04hRegAddr2Dh;
        [Description("R,DevAddr04hRegAddr2Eh")]
        public string DevAddr04hRegAddr2Eh;
        [Description("R,DevAddr04hRegAddr2Fh")]
        public string DevAddr04hRegAddr2Fh;
        [Description("R,DevAddr04hRegAddr30h")]
        public string DevAddr04hRegAddr30h;
        [Description("R,DevAddr04hRegAddr31h")]
        public string DevAddr04hRegAddr31h;
        [Description("R,DevAddr04hRegAddr32h")]
        public string DevAddr04hRegAddr32h;
        [Description("R,DevAddr04hRegAddr33h")]
        public string DevAddr04hRegAddr33h;
        [Description("R,DevAddr04hRegAddr34h")]
        public string DevAddr04hRegAddr34h;
        [Description("R,DevAddr04hRegAddr35h")]
        public string DevAddr04hRegAddr35h;
        [Description("R,DevAddr04hRegAddr36h")]
        public string DevAddr04hRegAddr36h;
        [Description("R,DevAddr04hRegAddr37h")]
        public string DevAddr04hRegAddr37h;
        [Description("R,DevAddr04hRegAddr40h")]
        public string DevAddr04hRegAddr40h;
        [Description("R,DevAddr04hRegAddr41h")]
        public string DevAddr04hRegAddr41h;
        [Description("R,DevAddr04hRegAddr42h")]
        public string DevAddr04hRegAddr42h;
        [Description("R,DevAddr04hRegAddr43h")]
        public string DevAddr04hRegAddr43h;
        [Description("R,DevAddr04hRegAddr44h")]
        public string DevAddr04hRegAddr44h;
        [Description("R,DevAddr04hRegAddr50h")]
        public string DevAddr04hRegAddr50h;
        [Description("R,DevAddr04hRegAddr51h")]
        public string DevAddr04hRegAddr51h;
        [Description("R,DevAddr04hRegAddr52h")]
        public string DevAddr04hRegAddr52h;
        [Description("R,DevAddr04hRegAddr53h")]
        public string DevAddr04hRegAddr53h;
        [Description("R,DevAddr04hRegAddr54h")]
        public string DevAddr04hRegAddr54h;
        [Description("R,DevAddr04hRegAddr55h")]
        public string DevAddr04hRegAddr55h;
        [Description("R,DevAddr04hRegAddr56h")]
        public string DevAddr04hRegAddr56h;
        [Description("R,DevAddr04hRegAddr57h")]
        public string DevAddr04hRegAddr57h;
        [Description("R,DevAddr04hRegAddr58h")]
        public string DevAddr04hRegAddr58h;
        [Description("R,DevAddr04hRegAddr59h")]
        public string DevAddr04hRegAddr59h;
        [Description("R,DevAddr04hRegAddr5Ah")]
        public string DevAddr04hRegAddr5Ah;
        [Description("R,DevAddr04hRegAddr5Bh")]
        public string DevAddr04hRegAddr5Bh;
        [Description("R,DevAddr04hRegAddr5Ch")]
        public string DevAddr04hRegAddr5Ch;
        [Description("R,DevAddr04hRegAddr5Dh")]
        public string DevAddr04hRegAddr5Dh;
        [Description("R,DevAddr04hRegAddr5Eh")]
        public string DevAddr04hRegAddr5Eh;
        [Description("R,DevAddr04hRegAddr5Fh")]
        public string DevAddr04hRegAddr5Fh;
        [Description("R,DevAddr04hRegAddr60h")]
        public string DevAddr04hRegAddr60h;
        [Description("R,DevAddr04hRegAddr61h")]
        public string DevAddr04hRegAddr61h;
        [Description("R,DevAddr04hRegAddr62h")]
        public string DevAddr04hRegAddr62h;
        [Description("R,DevAddr04hRegAddr63h")]
        public string DevAddr04hRegAddr63h;
        [Description("R,DevAddr04hRegAddr64h")]
        public string DevAddr04hRegAddr64h;
        [Description("R,DevAddr04hRegAddr65h")]
        public string DevAddr04hRegAddr65h;
        [Description("R,DevAddr04hRegAddr66h")]
        public string DevAddr04hRegAddr66h;
        [Description("R,DevAddr04hRegAddr67h")]
        public string DevAddr04hRegAddr67h;
        [Description("R,DevAddr04hRegAddr70h")]
        public string DevAddr04hRegAddr70h;
        [Description("R,DevAddr04hRegAddr71h")]
        public string DevAddr04hRegAddr71h;
        [Description("R,DevAddr04hRegAddr72h")]
        public string DevAddr04hRegAddr72h;
        [Description("R,DevAddr04hRegAddr73h")]
        public string DevAddr04hRegAddr73h;
        [Description("R,DevAddr04hRegAddr74h")]
        public string DevAddr04hRegAddr74h;
        [Description("R,DevAddr04hRegAddr75h")]
        public string DevAddr04hRegAddr75h;
        [Description("R,DevAddr04hRegAddr76h")]
        public string DevAddr04hRegAddr76h;
        [Description("R,DevAddr04hRegAddr77h")]
        public string DevAddr04hRegAddr77h;
        [Description("R,DevAddr04hRegAddr78h")]
        public string DevAddr04hRegAddr78h;
        [Description("R,DevAddr04hRegAddr79h")]
        public string DevAddr04hRegAddr79h;
        [Description("R,DevAddr04hRegAddr7Ah")]
        public string DevAddr04hRegAddr7Ah;
        [Description("R,DevAddr04hRegAddr7Bh")]
        public string DevAddr04hRegAddr7Bh;
        [Description("R,DevAddr04hRegAddr7Ch")]
        public string DevAddr04hRegAddr7Ch;
        [Description("R,DevAddr04hRegAddr7Dh")]
        public string DevAddr04hRegAddr7Dh;
        [Description("R,DevAddr04hRegAddr7Eh")]
        public string DevAddr04hRegAddr7Eh;
        [Description("R,DevAddr04hRegAddr7Fh")]
        public string DevAddr04hRegAddr7Fh;
        [Description("R,DevAddr04hRegAddr80h")]
        public string DevAddr04hRegAddr80h;
        [Description("R,DevAddr04hRegAddr81h")]
        public string DevAddr04hRegAddr81h;
        [Description("R,DevAddr04hRegAddr82h")]
        public string DevAddr04hRegAddr82h;
        [Description("R,DevAddr04hRegAddr83h")]
        public string DevAddr04hRegAddr83h;
        [Description("R,DevAddr04hRegAddr84h")]
        public string DevAddr04hRegAddr84h;
        [Description("R,DevAddr04hRegAddr85h")]
        public string DevAddr04hRegAddr85h;
        [Description("R,DevAddr04hRegAddr86h")]
        public string DevAddr04hRegAddr86h;
        [Description("R,DevAddr04hRegAddr87h")]
        public string DevAddr04hRegAddr87h;

        [Description("R,DevAddr05hRegAddr00h")]
        public string DevAddr05hRegAddr00h;
        [Description("R,DevAddr05hRegAddr01h")]
        public string DevAddr05hRegAddr01h;
        [Description("R,DevAddr05hRegAddr02h")]
        public string DevAddr05hRegAddr02h;
        [Description("R,DevAddr05hRegAddr03h")]
        public string DevAddr05hRegAddr03h;
        [Description("R,DevAddr05hRegAddr04h")]
        public string DevAddr05hRegAddr04h;
        [Description("R,DevAddr05hRegAddr05h")]
        public string DevAddr05hRegAddr05h;
        [Description("R,DevAddr05hRegAddr06h")]
        public string DevAddr05hRegAddr06h;
        [Description("R,DevAddr05hRegAddr07h")]
        public string DevAddr05hRegAddr07h;
        [Description("R,DevAddr05hRegAddr08h")]
        public string DevAddr05hRegAddr08h;
        [Description("R,DevAddr05hRegAddr09h")]
        public string DevAddr05hRegAddr09h;
        [Description("R,DevAddr05hRegAddr0Ah")]
        public string DevAddr05hRegAddr0Ah;
        [Description("R,DevAddr05hRegAddr0Bh")]
        public string DevAddr05hRegAddr0Bh;
        [Description("R,DevAddr05hRegAddr0Ch")]
        public string DevAddr05hRegAddr0Ch;
        [Description("R,DevAddr05hRegAddr0Dh")]
        public string DevAddr05hRegAddr0Dh;
        [Description("R,DevAddr05hRegAddr0Eh")]
        public string DevAddr05hRegAddr0Eh;
        [Description("R,DevAddr05hRegAddr0Fh")]
        public string DevAddr05hRegAddr0Fh;
        [Description("R,DevAddr05hRegAddr10h")]
        public string DevAddr05hRegAddr10h;
        [Description("R,DevAddr05hRegAddr11h")]
        public string DevAddr05hRegAddr11h;
        [Description("R,DevAddr05hRegAddr12h")]
        public string DevAddr05hRegAddr12h;
        [Description("R,DevAddr05hRegAddr13h")]
        public string DevAddr05hRegAddr13h;
        [Description("R,DevAddr05hRegAddr14h")]
        public string DevAddr05hRegAddr14h;
        [Description("R,DevAddr05hRegAddr15h")]
        public string DevAddr05hRegAddr15h;
        [Description("R,DevAddr05hRegAddr16h")]
        public string DevAddr05hRegAddr16h;
        [Description("R,DevAddr05hRegAddr17h")]
        public string DevAddr05hRegAddr17h;
        [Description("R,DevAddr05hRegAddr20h")]
        public string DevAddr05hRegAddr20h;
        [Description("R,DevAddr05hRegAddr21h")]
        public string DevAddr05hRegAddr21h;
        [Description("R,DevAddr05hRegAddr22h")]
        public string DevAddr05hRegAddr22h;
        [Description("R,DevAddr05hRegAddr23h")]
        public string DevAddr05hRegAddr23h;
        [Description("R,DevAddr05hRegAddr24h")]
        public string DevAddr05hRegAddr24h;
        [Description("R,DevAddr05hRegAddr25h")]
        public string DevAddr05hRegAddr25h;
        [Description("R,DevAddr05hRegAddr26h")]
        public string DevAddr05hRegAddr26h;
        [Description("R,DevAddr05hRegAddr27h")]
        public string DevAddr05hRegAddr27h;
        [Description("R,DevAddr05hRegAddr28h")]
        public string DevAddr05hRegAddr28h;
        [Description("R,DevAddr05hRegAddr29h")]
        public string DevAddr05hRegAddr29h;
        [Description("R,DevAddr05hRegAddr2Ah")]
        public string DevAddr05hRegAddr2Ah;
        [Description("R,DevAddr05hRegAddr2Bh")]
        public string DevAddr05hRegAddr2Bh;
        [Description("R,DevAddr05hRegAddr2Ch")]
        public string DevAddr05hRegAddr2Ch;
        [Description("R,DevAddr05hRegAddr2Dh")]
        public string DevAddr05hRegAddr2Dh;
        [Description("R,DevAddr05hRegAddr2Eh")]
        public string DevAddr05hRegAddr2Eh;
        [Description("R,DevAddr05hRegAddr2Fh")]
        public string DevAddr05hRegAddr2Fh;
        [Description("R,DevAddr05hRegAddr30h")]
        public string DevAddr05hRegAddr30h;
        [Description("R,DevAddr05hRegAddr31h")]
        public string DevAddr05hRegAddr31h;
        [Description("R,DevAddr05hRegAddr32h")]
        public string DevAddr05hRegAddr32h;
        [Description("R,DevAddr05hRegAddr33h")]
        public string DevAddr05hRegAddr33h;
        [Description("R,DevAddr05hRegAddr34h")]
        public string DevAddr05hRegAddr34h;
        [Description("R,DevAddr05hRegAddr35h")]
        public string DevAddr05hRegAddr35h;
        [Description("R,DevAddr05hRegAddr36h")]
        public string DevAddr05hRegAddr36h;
        [Description("R,DevAddr05hRegAddr37h")]
        public string DevAddr05hRegAddr37h;
        [Description("R,DevAddr05hRegAddr40h")]
        public string DevAddr05hRegAddr40h;
        [Description("R,DevAddr05hRegAddr41h")]
        public string DevAddr05hRegAddr41h;
        [Description("R,DevAddr05hRegAddr42h")]
        public string DevAddr05hRegAddr42h;
        [Description("R,DevAddr05hRegAddr43h")]
        public string DevAddr05hRegAddr43h;
        [Description("R,DevAddr05hRegAddr44h")]
        public string DevAddr05hRegAddr44h;
        [Description("R,DevAddr05hRegAddr50h")]
        public string DevAddr05hRegAddr50h;
        [Description("R,DevAddr05hRegAddr51h")]
        public string DevAddr05hRegAddr51h;
        [Description("R,DevAddr05hRegAddr52h")]
        public string DevAddr05hRegAddr52h;
        [Description("R,DevAddr05hRegAddr53h")]
        public string DevAddr05hRegAddr53h;
        [Description("R,DevAddr05hRegAddr54h")]
        public string DevAddr05hRegAddr54h;
        [Description("R,DevAddr05hRegAddr55h")]
        public string DevAddr05hRegAddr55h;
        [Description("R,DevAddr05hRegAddr56h")]
        public string DevAddr05hRegAddr56h;
        [Description("R,DevAddr05hRegAddr57h")]
        public string DevAddr05hRegAddr57h;
        [Description("R,DevAddr05hRegAddr58h")]
        public string DevAddr05hRegAddr58h;
        [Description("R,DevAddr05hRegAddr59h")]
        public string DevAddr05hRegAddr59h;
        [Description("R,DevAddr05hRegAddr5Ah")]
        public string DevAddr05hRegAddr5Ah;
        [Description("R,DevAddr05hRegAddr5Bh")]
        public string DevAddr05hRegAddr5Bh;
        [Description("R,DevAddr05hRegAddr5Ch")]
        public string DevAddr05hRegAddr5Ch;
        [Description("R,DevAddr05hRegAddr5Dh")]
        public string DevAddr05hRegAddr5Dh;
        [Description("R,DevAddr05hRegAddr5Eh")]
        public string DevAddr05hRegAddr5Eh;
        [Description("R,DevAddr05hRegAddr5Fh")]
        public string DevAddr05hRegAddr5Fh;
        [Description("R,DevAddr05hRegAddr60h")]
        public string DevAddr05hRegAddr60h;
        [Description("R,DevAddr05hRegAddr61h")]
        public string DevAddr05hRegAddr61h;
        [Description("R,DevAddr05hRegAddr62h")]
        public string DevAddr05hRegAddr62h;
        [Description("R,DevAddr05hRegAddr63h")]
        public string DevAddr05hRegAddr63h;
        [Description("R,DevAddr05hRegAddr64h")]
        public string DevAddr05hRegAddr64h;
        [Description("R,DevAddr05hRegAddr65h")]
        public string DevAddr05hRegAddr65h;
        [Description("R,DevAddr05hRegAddr66h")]
        public string DevAddr05hRegAddr66h;
        [Description("R,DevAddr05hRegAddr67h")]
        public string DevAddr05hRegAddr67h;
        [Description("R,DevAddr05hRegAddr70h")]
        public string DevAddr05hRegAddr70h;
        [Description("R,DevAddr05hRegAddr71h")]
        public string DevAddr05hRegAddr71h;
        [Description("R,DevAddr05hRegAddr72h")]
        public string DevAddr05hRegAddr72h;
        [Description("R,DevAddr05hRegAddr73h")]
        public string DevAddr05hRegAddr73h;
        [Description("R,DevAddr05hRegAddr74h")]
        public string DevAddr05hRegAddr74h;
        [Description("R,DevAddr05hRegAddr75h")]
        public string DevAddr05hRegAddr75h;
        [Description("R,DevAddr05hRegAddr76h")]
        public string DevAddr05hRegAddr76h;
        [Description("R,DevAddr05hRegAddr77h")]
        public string DevAddr05hRegAddr77h;
        [Description("R,DevAddr05hRegAddr78h")]
        public string DevAddr05hRegAddr78h;
        [Description("R,DevAddr05hRegAddr79h")]
        public string DevAddr05hRegAddr79h;
        [Description("R,DevAddr05hRegAddr7Ah")]
        public string DevAddr05hRegAddr7Ah;
        [Description("R,DevAddr05hRegAddr7Bh")]
        public string DevAddr05hRegAddr7Bh;
        [Description("R,DevAddr05hRegAddr7Ch")]
        public string DevAddr05hRegAddr7Ch;
        [Description("R,DevAddr05hRegAddr7Dh")]
        public string DevAddr05hRegAddr7Dh;
        [Description("R,DevAddr05hRegAddr7Eh")]
        public string DevAddr05hRegAddr7Eh;
        [Description("R,DevAddr05hRegAddr7Fh")]
        public string DevAddr05hRegAddr7Fh;
        [Description("R,DevAddr05hRegAddr80h")]
        public string DevAddr05hRegAddr80h;
        [Description("R,DevAddr05hRegAddr81h")]
        public string DevAddr05hRegAddr81h;
        [Description("R,DevAddr05hRegAddr82h")]
        public string DevAddr05hRegAddr82h;
        [Description("R,DevAddr05hRegAddr83h")]
        public string DevAddr05hRegAddr83h;
        [Description("R,DevAddr05hRegAddr84h")]
        public string DevAddr05hRegAddr84h;
        [Description("R,DevAddr05hRegAddr85h")]
        public string DevAddr05hRegAddr85h;
        [Description("R,DevAddr05hRegAddr86h")]
        public string DevAddr05hRegAddr86h;
        [Description("R,DevAddr05hRegAddr87h")]
        public string DevAddr05hRegAddr87h;

        [Description("R,DevAddr06hRegAddr00h")]
        public string DevAddr06hRegAddr00h;
        [Description("R,DevAddr06hRegAddr01h")]
        public string DevAddr06hRegAddr01h;
        [Description("R,DevAddr06hRegAddr02h")]
        public string DevAddr06hRegAddr02h;
        [Description("R,DevAddr06hRegAddr03h")]
        public string DevAddr06hRegAddr03h;
        [Description("R,DevAddr06hRegAddr04h")]
        public string DevAddr06hRegAddr04h;
        [Description("R,DevAddr06hRegAddr05h")]
        public string DevAddr06hRegAddr05h;
        [Description("R,DevAddr06hRegAddr06h")]
        public string DevAddr06hRegAddr06h;
        [Description("R,DevAddr06hRegAddr07h")]
        public string DevAddr06hRegAddr07h;
        [Description("R,DevAddr06hRegAddr08h")]
        public string DevAddr06hRegAddr08h;
        [Description("R,DevAddr06hRegAddr09h")]
        public string DevAddr06hRegAddr09h;
        [Description("R,DevAddr06hRegAddr0Ah")]
        public string DevAddr06hRegAddr0Ah;
        [Description("R,DevAddr06hRegAddr0Bh")]
        public string DevAddr06hRegAddr0Bh;
        [Description("R,DevAddr06hRegAddr0Ch")]
        public string DevAddr06hRegAddr0Ch;
        [Description("R,DevAddr06hRegAddr0Dh")]
        public string DevAddr06hRegAddr0Dh;
        [Description("R,DevAddr06hRegAddr0Eh")]
        public string DevAddr06hRegAddr0Eh;
        [Description("R,DevAddr06hRegAddr0Fh")]
        public string DevAddr06hRegAddr0Fh;
        [Description("R,DevAddr06hRegAddr10h")]
        public string DevAddr06hRegAddr10h;
        [Description("R,DevAddr06hRegAddr11h")]
        public string DevAddr06hRegAddr11h;
        [Description("R,DevAddr06hRegAddr12h")]
        public string DevAddr06hRegAddr12h;
        [Description("R,DevAddr06hRegAddr13h")]
        public string DevAddr06hRegAddr13h;
        [Description("R,DevAddr06hRegAddr14h")]
        public string DevAddr06hRegAddr14h;
        [Description("R,DevAddr06hRegAddr15h")]
        public string DevAddr06hRegAddr15h;
        [Description("R,DevAddr06hRegAddr16h")]
        public string DevAddr06hRegAddr16h;
        [Description("R,DevAddr06hRegAddr17h")]
        public string DevAddr06hRegAddr17h;
        [Description("R,DevAddr06hRegAddr20h")]
        public string DevAddr06hRegAddr20h;
        [Description("R,DevAddr06hRegAddr21h")]
        public string DevAddr06hRegAddr21h;
        [Description("R,DevAddr06hRegAddr22h")]
        public string DevAddr06hRegAddr22h;
        [Description("R,DevAddr06hRegAddr23h")]
        public string DevAddr06hRegAddr23h;
        [Description("R,DevAddr06hRegAddr24h")]
        public string DevAddr06hRegAddr24h;
        [Description("R,DevAddr06hRegAddr25h")]
        public string DevAddr06hRegAddr25h;
        [Description("R,DevAddr06hRegAddr26h")]
        public string DevAddr06hRegAddr26h;
        [Description("R,DevAddr06hRegAddr27h")]
        public string DevAddr06hRegAddr27h;
        [Description("R,DevAddr06hRegAddr28h")]
        public string DevAddr06hRegAddr28h;
        [Description("R,DevAddr06hRegAddr29h")]
        public string DevAddr06hRegAddr29h;
        [Description("R,DevAddr06hRegAddr2Ah")]
        public string DevAddr06hRegAddr2Ah;
        [Description("R,DevAddr06hRegAddr2Bh")]
        public string DevAddr06hRegAddr2Bh;
        [Description("R,DevAddr06hRegAddr2Ch")]
        public string DevAddr06hRegAddr2Ch;
        [Description("R,DevAddr06hRegAddr2Dh")]
        public string DevAddr06hRegAddr2Dh;
        [Description("R,DevAddr06hRegAddr2Eh")]
        public string DevAddr06hRegAddr2Eh;
        [Description("R,DevAddr06hRegAddr2Fh")]
        public string DevAddr06hRegAddr2Fh;
        [Description("R,DevAddr06hRegAddr30h")]
        public string DevAddr06hRegAddr30h;
        [Description("R,DevAddr06hRegAddr31h")]
        public string DevAddr06hRegAddr31h;
        [Description("R,DevAddr06hRegAddr32h")]
        public string DevAddr06hRegAddr32h;
        [Description("R,DevAddr06hRegAddr33h")]
        public string DevAddr06hRegAddr33h;
        [Description("R,DevAddr06hRegAddr34h")]
        public string DevAddr06hRegAddr34h;
        [Description("R,DevAddr06hRegAddr35h")]
        public string DevAddr06hRegAddr35h;
        [Description("R,DevAddr06hRegAddr36h")]
        public string DevAddr06hRegAddr36h;
        [Description("R,DevAddr06hRegAddr37h")]
        public string DevAddr06hRegAddr37h;
        [Description("R,DevAddr06hRegAddr40h")]
        public string DevAddr06hRegAddr40h;
        [Description("R,DevAddr06hRegAddr41h")]
        public string DevAddr06hRegAddr41h;
        [Description("R,DevAddr06hRegAddr42h")]
        public string DevAddr06hRegAddr42h;
        [Description("R,DevAddr06hRegAddr43h")]
        public string DevAddr06hRegAddr43h;
        [Description("R,DevAddr06hRegAddr44h")]
        public string DevAddr06hRegAddr44h;
        [Description("R,DevAddr06hRegAddr50h")]
        public string DevAddr06hRegAddr50h;
        [Description("R,DevAddr06hRegAddr51h")]
        public string DevAddr06hRegAddr51h;
        [Description("R,DevAddr06hRegAddr52h")]
        public string DevAddr06hRegAddr52h;
        [Description("R,DevAddr06hRegAddr53h")]
        public string DevAddr06hRegAddr53h;
        [Description("R,DevAddr06hRegAddr54h")]
        public string DevAddr06hRegAddr54h;
        [Description("R,DevAddr06hRegAddr55h")]
        public string DevAddr06hRegAddr55h;
        [Description("R,DevAddr06hRegAddr56h")]
        public string DevAddr06hRegAddr56h;
        [Description("R,DevAddr06hRegAddr57h")]
        public string DevAddr06hRegAddr57h;
        [Description("R,DevAddr06hRegAddr58h")]
        public string DevAddr06hRegAddr58h;
        [Description("R,DevAddr06hRegAddr59h")]
        public string DevAddr06hRegAddr59h;
        [Description("R,DevAddr06hRegAddr5Ah")]
        public string DevAddr06hRegAddr5Ah;
        [Description("R,DevAddr06hRegAddr5Bh")]
        public string DevAddr06hRegAddr5Bh;
        [Description("R,DevAddr06hRegAddr5Ch")]
        public string DevAddr06hRegAddr5Ch;
        [Description("R,DevAddr06hRegAddr5Dh")]
        public string DevAddr06hRegAddr5Dh;
        [Description("R,DevAddr06hRegAddr5Eh")]
        public string DevAddr06hRegAddr5Eh;
        [Description("R,DevAddr06hRegAddr5Fh")]
        public string DevAddr06hRegAddr5Fh;
        [Description("R,DevAddr06hRegAddr60h")]
        public string DevAddr06hRegAddr60h;
        [Description("R,DevAddr06hRegAddr61h")]
        public string DevAddr06hRegAddr61h;
        [Description("R,DevAddr06hRegAddr62h")]
        public string DevAddr06hRegAddr62h;
        [Description("R,DevAddr06hRegAddr63h")]
        public string DevAddr06hRegAddr63h;
        [Description("R,DevAddr06hRegAddr64h")]
        public string DevAddr06hRegAddr64h;
        [Description("R,DevAddr06hRegAddr65h")]
        public string DevAddr06hRegAddr65h;
        [Description("R,DevAddr06hRegAddr66h")]
        public string DevAddr06hRegAddr66h;
        [Description("R,DevAddr06hRegAddr67h")]
        public string DevAddr06hRegAddr67h;
        [Description("R,DevAddr06hRegAddr70h")]
        public string DevAddr06hRegAddr70h;
        [Description("R,DevAddr06hRegAddr71h")]
        public string DevAddr06hRegAddr71h;
        [Description("R,DevAddr06hRegAddr72h")]
        public string DevAddr06hRegAddr72h;
        [Description("R,DevAddr06hRegAddr73h")]
        public string DevAddr06hRegAddr73h;
        [Description("R,DevAddr06hRegAddr74h")]
        public string DevAddr06hRegAddr74h;
        [Description("R,DevAddr06hRegAddr75h")]
        public string DevAddr06hRegAddr75h;
        [Description("R,DevAddr06hRegAddr76h")]
        public string DevAddr06hRegAddr76h;
        [Description("R,DevAddr06hRegAddr77h")]
        public string DevAddr06hRegAddr77h;
        [Description("R,DevAddr06hRegAddr78h")]
        public string DevAddr06hRegAddr78h;
        [Description("R,DevAddr06hRegAddr79h")]
        public string DevAddr06hRegAddr79h;
        [Description("R,DevAddr06hRegAddr7Ah")]
        public string DevAddr06hRegAddr7Ah;
        [Description("R,DevAddr06hRegAddr7Bh")]
        public string DevAddr06hRegAddr7Bh;
        [Description("R,DevAddr06hRegAddr7Ch")]
        public string DevAddr06hRegAddr7Ch;
        [Description("R,DevAddr06hRegAddr7Dh")]
        public string DevAddr06hRegAddr7Dh;
        [Description("R,DevAddr06hRegAddr7Eh")]
        public string DevAddr06hRegAddr7Eh;
        [Description("R,DevAddr06hRegAddr7Fh")]
        public string DevAddr06hRegAddr7Fh;
        [Description("R,DevAddr06hRegAddr80h")]
        public string DevAddr06hRegAddr80h;
        [Description("R,DevAddr06hRegAddr81h")]
        public string DevAddr06hRegAddr81h;
        [Description("R,DevAddr06hRegAddr82h")]
        public string DevAddr06hRegAddr82h;
        [Description("R,DevAddr06hRegAddr83h")]
        public string DevAddr06hRegAddr83h;
        [Description("R,DevAddr06hRegAddr84h")]
        public string DevAddr06hRegAddr84h;
        [Description("R,DevAddr06hRegAddr85h")]
        public string DevAddr06hRegAddr85h;
        [Description("R,DevAddr06hRegAddr86h")]
        public string DevAddr06hRegAddr86h;
        [Description("R,DevAddr06hRegAddr87h")]
        public string DevAddr06hRegAddr87h;

        [Description("R,DevAddr07hRegAddr00h")]
        public string DevAddr07hRegAddr00h;
        [Description("R,DevAddr07hRegAddr01h")]
        public string DevAddr07hRegAddr01h;
        [Description("R,DevAddr07hRegAddr02h")]
        public string DevAddr07hRegAddr02h;
        [Description("R,DevAddr07hRegAddr03h")]
        public string DevAddr07hRegAddr03h;
        [Description("R,DevAddr07hRegAddr04h")]
        public string DevAddr07hRegAddr04h;
        [Description("R,DevAddr07hRegAddr05h")]
        public string DevAddr07hRegAddr05h;
        [Description("R,DevAddr07hRegAddr06h")]
        public string DevAddr07hRegAddr06h;
        [Description("R,DevAddr07hRegAddr07h")]
        public string DevAddr07hRegAddr07h;
        [Description("R,DevAddr07hRegAddr08h")]
        public string DevAddr07hRegAddr08h;
        [Description("R,DevAddr07hRegAddr09h")]
        public string DevAddr07hRegAddr09h;
        [Description("R,DevAddr07hRegAddr0Ah")]
        public string DevAddr07hRegAddr0Ah;
        [Description("R,DevAddr07hRegAddr0Bh")]
        public string DevAddr07hRegAddr0Bh;
        [Description("R,DevAddr07hRegAddr0Ch")]
        public string DevAddr07hRegAddr0Ch;
        [Description("R,DevAddr07hRegAddr0Dh")]
        public string DevAddr07hRegAddr0Dh;
        [Description("R,DevAddr07hRegAddr0Eh")]
        public string DevAddr07hRegAddr0Eh;
        [Description("R,DevAddr07hRegAddr0Fh")]
        public string DevAddr07hRegAddr0Fh;
        [Description("R,DevAddr07hRegAddr10h")]
        public string DevAddr07hRegAddr10h;
        [Description("R,DevAddr07hRegAddr11h")]
        public string DevAddr07hRegAddr11h;
        [Description("R,DevAddr07hRegAddr12h")]
        public string DevAddr07hRegAddr12h;
        [Description("R,DevAddr07hRegAddr13h")]
        public string DevAddr07hRegAddr13h;
        [Description("R,DevAddr07hRegAddr14h")]
        public string DevAddr07hRegAddr14h;
        [Description("R,DevAddr07hRegAddr15h")]
        public string DevAddr07hRegAddr15h;
        [Description("R,DevAddr07hRegAddr16h")]
        public string DevAddr07hRegAddr16h;
        [Description("R,DevAddr07hRegAddr17h")]
        public string DevAddr07hRegAddr17h;
        [Description("R,DevAddr07hRegAddr20h")]
        public string DevAddr07hRegAddr20h;
        [Description("R,DevAddr07hRegAddr21h")]
        public string DevAddr07hRegAddr21h;
        [Description("R,DevAddr07hRegAddr22h")]
        public string DevAddr07hRegAddr22h;
        [Description("R,DevAddr07hRegAddr23h")]
        public string DevAddr07hRegAddr23h;
        [Description("R,DevAddr07hRegAddr24h")]
        public string DevAddr07hRegAddr24h;
        [Description("R,DevAddr07hRegAddr25h")]
        public string DevAddr07hRegAddr25h;
        [Description("R,DevAddr07hRegAddr26h")]
        public string DevAddr07hRegAddr26h;
        [Description("R,DevAddr07hRegAddr27h")]
        public string DevAddr07hRegAddr27h;
        [Description("R,DevAddr07hRegAddr28h")]
        public string DevAddr07hRegAddr28h;
        [Description("R,DevAddr07hRegAddr29h")]
        public string DevAddr07hRegAddr29h;
        [Description("R,DevAddr07hRegAddr2Ah")]
        public string DevAddr07hRegAddr2Ah;
        [Description("R,DevAddr07hRegAddr2Bh")]
        public string DevAddr07hRegAddr2Bh;
        [Description("R,DevAddr07hRegAddr2Ch")]
        public string DevAddr07hRegAddr2Ch;
        [Description("R,DevAddr07hRegAddr2Dh")]
        public string DevAddr07hRegAddr2Dh;
        [Description("R,DevAddr07hRegAddr2Eh")]
        public string DevAddr07hRegAddr2Eh;
        [Description("R,DevAddr07hRegAddr2Fh")]
        public string DevAddr07hRegAddr2Fh;
        [Description("R,DevAddr07hRegAddr30h")]
        public string DevAddr07hRegAddr30h;
        [Description("R,DevAddr07hRegAddr31h")]
        public string DevAddr07hRegAddr31h;
        [Description("R,DevAddr07hRegAddr32h")]
        public string DevAddr07hRegAddr32h;
        [Description("R,DevAddr07hRegAddr33h")]
        public string DevAddr07hRegAddr33h;
        [Description("R,DevAddr07hRegAddr34h")]
        public string DevAddr07hRegAddr34h;
        [Description("R,DevAddr07hRegAddr35h")]
        public string DevAddr07hRegAddr35h;
        [Description("R,DevAddr07hRegAddr36h")]
        public string DevAddr07hRegAddr36h;
        [Description("R,DevAddr07hRegAddr37h")]
        public string DevAddr07hRegAddr37h;
        [Description("R,DevAddr07hRegAddr40h")]
        public string DevAddr07hRegAddr40h;
        [Description("R,DevAddr07hRegAddr41h")]
        public string DevAddr07hRegAddr41h;
        [Description("R,DevAddr07hRegAddr42h")]
        public string DevAddr07hRegAddr42h;
        [Description("R,DevAddr07hRegAddr43h")]
        public string DevAddr07hRegAddr43h;
        [Description("R,DevAddr07hRegAddr44h")]
        public string DevAddr07hRegAddr44h;
        [Description("R,DevAddr07hRegAddr50h")]
        public string DevAddr07hRegAddr50h;
        [Description("R,DevAddr07hRegAddr51h")]
        public string DevAddr07hRegAddr51h;
        [Description("R,DevAddr07hRegAddr52h")]
        public string DevAddr07hRegAddr52h;
        [Description("R,DevAddr07hRegAddr53h")]
        public string DevAddr07hRegAddr53h;
        [Description("R,DevAddr07hRegAddr54h")]
        public string DevAddr07hRegAddr54h;
        [Description("R,DevAddr07hRegAddr55h")]
        public string DevAddr07hRegAddr55h;
        [Description("R,DevAddr07hRegAddr56h")]
        public string DevAddr07hRegAddr56h;
        [Description("R,DevAddr07hRegAddr57h")]
        public string DevAddr07hRegAddr57h;
        [Description("R,DevAddr07hRegAddr58h")]
        public string DevAddr07hRegAddr58h;
        [Description("R,DevAddr07hRegAddr59h")]
        public string DevAddr07hRegAddr59h;
        [Description("R,DevAddr07hRegAddr5Ah")]
        public string DevAddr07hRegAddr5Ah;
        [Description("R,DevAddr07hRegAddr5Bh")]
        public string DevAddr07hRegAddr5Bh;
        [Description("R,DevAddr07hRegAddr5Ch")]
        public string DevAddr07hRegAddr5Ch;
        [Description("R,DevAddr07hRegAddr5Dh")]
        public string DevAddr07hRegAddr5Dh;
        [Description("R,DevAddr07hRegAddr5Eh")]
        public string DevAddr07hRegAddr5Eh;
        [Description("R,DevAddr07hRegAddr5Fh")]
        public string DevAddr07hRegAddr5Fh;
        [Description("R,DevAddr07hRegAddr60h")]
        public string DevAddr07hRegAddr60h;
        [Description("R,DevAddr07hRegAddr61h")]
        public string DevAddr07hRegAddr61h;
        [Description("R,DevAddr07hRegAddr62h")]
        public string DevAddr07hRegAddr62h;
        [Description("R,DevAddr07hRegAddr63h")]
        public string DevAddr07hRegAddr63h;
        [Description("R,DevAddr07hRegAddr64h")]
        public string DevAddr07hRegAddr64h;
        [Description("R,DevAddr07hRegAddr65h")]
        public string DevAddr07hRegAddr65h;
        [Description("R,DevAddr07hRegAddr66h")]
        public string DevAddr07hRegAddr66h;
        [Description("R,DevAddr07hRegAddr67h")]
        public string DevAddr07hRegAddr67h;
        [Description("R,DevAddr07hRegAddr70h")]
        public string DevAddr07hRegAddr70h;
        [Description("R,DevAddr07hRegAddr71h")]
        public string DevAddr07hRegAddr71h;
        [Description("R,DevAddr07hRegAddr72h")]
        public string DevAddr07hRegAddr72h;
        [Description("R,DevAddr07hRegAddr73h")]
        public string DevAddr07hRegAddr73h;
        [Description("R,DevAddr07hRegAddr74h")]
        public string DevAddr07hRegAddr74h;
        [Description("R,DevAddr07hRegAddr75h")]
        public string DevAddr07hRegAddr75h;
        [Description("R,DevAddr07hRegAddr76h")]
        public string DevAddr07hRegAddr76h;
        [Description("R,DevAddr07hRegAddr77h")]
        public string DevAddr07hRegAddr77h;
        [Description("R,DevAddr07hRegAddr78h")]
        public string DevAddr07hRegAddr78h;
        [Description("R,DevAddr07hRegAddr79h")]
        public string DevAddr07hRegAddr79h;
        [Description("R,DevAddr07hRegAddr7Ah")]
        public string DevAddr07hRegAddr7Ah;
        [Description("R,DevAddr07hRegAddr7Bh")]
        public string DevAddr07hRegAddr7Bh;
        [Description("R,DevAddr07hRegAddr7Ch")]
        public string DevAddr07hRegAddr7Ch;
        [Description("R,DevAddr07hRegAddr7Dh")]
        public string DevAddr07hRegAddr7Dh;
        [Description("R,DevAddr07hRegAddr7Eh")]
        public string DevAddr07hRegAddr7Eh;
        [Description("R,DevAddr07hRegAddr7Fh")]
        public string DevAddr07hRegAddr7Fh;
        [Description("R,DevAddr07hRegAddr80h")]
        public string DevAddr07hRegAddr80h;
        [Description("R,DevAddr07hRegAddr81h")]
        public string DevAddr07hRegAddr81h;
        [Description("R,DevAddr07hRegAddr82h")]
        public string DevAddr07hRegAddr82h;
        [Description("R,DevAddr07hRegAddr83h")]
        public string DevAddr07hRegAddr83h;
        [Description("R,DevAddr07hRegAddr84h")]
        public string DevAddr07hRegAddr84h;
        [Description("R,DevAddr07hRegAddr85h")]
        public string DevAddr07hRegAddr85h;
        [Description("R,DevAddr07hRegAddr86h")]
        public string DevAddr07hRegAddr86h;
        [Description("R,DevAddr07hRegAddr87h")]
        public string DevAddr07hRegAddr87h;

        [Description("R,DevAddr08hRegAddr00h")]
        public string DevAddr08hRegAddr00h;
        [Description("R,DevAddr08hRegAddr01h")]
        public string DevAddr08hRegAddr01h;
        [Description("R,DevAddr08hRegAddr02h")]
        public string DevAddr08hRegAddr02h;
        [Description("R,DevAddr08hRegAddr03h")]
        public string DevAddr08hRegAddr03h;
        [Description("R,DevAddr08hRegAddr04h")]
        public string DevAddr08hRegAddr04h;
        [Description("R,DevAddr08hRegAddr05h")]
        public string DevAddr08hRegAddr05h;
        [Description("R,DevAddr08hRegAddr06h")]
        public string DevAddr08hRegAddr06h;
        [Description("R,DevAddr08hRegAddr07h")]
        public string DevAddr08hRegAddr07h;
        [Description("R,DevAddr08hRegAddr08h")]
        public string DevAddr08hRegAddr08h;
        [Description("R,DevAddr08hRegAddr09h")]
        public string DevAddr08hRegAddr09h;
        [Description("R,DevAddr08hRegAddr0Ah")]
        public string DevAddr08hRegAddr0Ah;
        [Description("R,DevAddr08hRegAddr0Bh")]
        public string DevAddr08hRegAddr0Bh;
        [Description("R,DevAddr08hRegAddr0Ch")]
        public string DevAddr08hRegAddr0Ch;
        [Description("R,DevAddr08hRegAddr0Dh")]
        public string DevAddr08hRegAddr0Dh;
        [Description("R,DevAddr08hRegAddr0Eh")]
        public string DevAddr08hRegAddr0Eh;
        [Description("R,DevAddr08hRegAddr0Fh")]
        public string DevAddr08hRegAddr0Fh;
        [Description("R,DevAddr08hRegAddr10h")]
        public string DevAddr08hRegAddr10h;
        [Description("R,DevAddr08hRegAddr11h")]
        public string DevAddr08hRegAddr11h;
        [Description("R,DevAddr08hRegAddr12h")]
        public string DevAddr08hRegAddr12h;
        [Description("R,DevAddr08hRegAddr13h")]
        public string DevAddr08hRegAddr13h;
        [Description("R,DevAddr08hRegAddr14h")]
        public string DevAddr08hRegAddr14h;
        [Description("R,DevAddr08hRegAddr15h")]
        public string DevAddr08hRegAddr15h;
        [Description("R,DevAddr08hRegAddr16h")]
        public string DevAddr08hRegAddr16h;
        [Description("R,DevAddr08hRegAddr17h")]
        public string DevAddr08hRegAddr17h;
        [Description("R,DevAddr08hRegAddr20h")]
        public string DevAddr08hRegAddr20h;
        [Description("R,DevAddr08hRegAddr21h")]
        public string DevAddr08hRegAddr21h;
        [Description("R,DevAddr08hRegAddr22h")]
        public string DevAddr08hRegAddr22h;
        [Description("R,DevAddr08hRegAddr23h")]
        public string DevAddr08hRegAddr23h;
        [Description("R,DevAddr08hRegAddr24h")]
        public string DevAddr08hRegAddr24h;
        [Description("R,DevAddr08hRegAddr25h")]
        public string DevAddr08hRegAddr25h;
        [Description("R,DevAddr08hRegAddr26h")]
        public string DevAddr08hRegAddr26h;
        [Description("R,DevAddr08hRegAddr27h")]
        public string DevAddr08hRegAddr27h;
        [Description("R,DevAddr08hRegAddr28h")]
        public string DevAddr08hRegAddr28h;
        [Description("R,DevAddr08hRegAddr29h")]
        public string DevAddr08hRegAddr29h;
        [Description("R,DevAddr08hRegAddr2Ah")]
        public string DevAddr08hRegAddr2Ah;
        [Description("R,DevAddr08hRegAddr2Bh")]
        public string DevAddr08hRegAddr2Bh;
        [Description("R,DevAddr08hRegAddr2Ch")]
        public string DevAddr08hRegAddr2Ch;
        [Description("R,DevAddr08hRegAddr2Dh")]
        public string DevAddr08hRegAddr2Dh;
        [Description("R,DevAddr08hRegAddr2Eh")]
        public string DevAddr08hRegAddr2Eh;
        [Description("R,DevAddr08hRegAddr2Fh")]
        public string DevAddr08hRegAddr2Fh;
        [Description("R,DevAddr08hRegAddr30h")]
        public string DevAddr08hRegAddr30h;
        [Description("R,DevAddr08hRegAddr31h")]
        public string DevAddr08hRegAddr31h;
        [Description("R,DevAddr08hRegAddr32h")]
        public string DevAddr08hRegAddr32h;
        [Description("R,DevAddr08hRegAddr33h")]
        public string DevAddr08hRegAddr33h;
        [Description("R,DevAddr08hRegAddr34h")]
        public string DevAddr08hRegAddr34h;
        [Description("R,DevAddr08hRegAddr35h")]
        public string DevAddr08hRegAddr35h;
        [Description("R,DevAddr08hRegAddr36h")]
        public string DevAddr08hRegAddr36h;
        [Description("R,DevAddr08hRegAddr37h")]
        public string DevAddr08hRegAddr37h;
        [Description("R,DevAddr08hRegAddr40h")]
        public string DevAddr08hRegAddr40h;
        [Description("R,DevAddr08hRegAddr41h")]
        public string DevAddr08hRegAddr41h;
        [Description("R,DevAddr08hRegAddr42h")]
        public string DevAddr08hRegAddr42h;
        [Description("R,DevAddr08hRegAddr43h")]
        public string DevAddr08hRegAddr43h;
        [Description("R,DevAddr08hRegAddr44h")]
        public string DevAddr08hRegAddr44h;
        [Description("R,DevAddr08hRegAddr50h")]
        public string DevAddr08hRegAddr50h;
        [Description("R,DevAddr08hRegAddr51h")]
        public string DevAddr08hRegAddr51h;
        [Description("R,DevAddr08hRegAddr52h")]
        public string DevAddr08hRegAddr52h;
        [Description("R,DevAddr08hRegAddr53h")]
        public string DevAddr08hRegAddr53h;
        [Description("R,DevAddr08hRegAddr54h")]
        public string DevAddr08hRegAddr54h;
        [Description("R,DevAddr08hRegAddr55h")]
        public string DevAddr08hRegAddr55h;
        [Description("R,DevAddr08hRegAddr56h")]
        public string DevAddr08hRegAddr56h;
        [Description("R,DevAddr08hRegAddr57h")]
        public string DevAddr08hRegAddr57h;
        [Description("R,DevAddr08hRegAddr58h")]
        public string DevAddr08hRegAddr58h;
        [Description("R,DevAddr08hRegAddr59h")]
        public string DevAddr08hRegAddr59h;
        [Description("R,DevAddr08hRegAddr5Ah")]
        public string DevAddr08hRegAddr5Ah;
        [Description("R,DevAddr08hRegAddr5Bh")]
        public string DevAddr08hRegAddr5Bh;
        [Description("R,DevAddr08hRegAddr5Ch")]
        public string DevAddr08hRegAddr5Ch;
        [Description("R,DevAddr08hRegAddr5Dh")]
        public string DevAddr08hRegAddr5Dh;
        [Description("R,DevAddr08hRegAddr5Eh")]
        public string DevAddr08hRegAddr5Eh;
        [Description("R,DevAddr08hRegAddr5Fh")]
        public string DevAddr08hRegAddr5Fh;
        [Description("R,DevAddr08hRegAddr60h")]
        public string DevAddr08hRegAddr60h;
        [Description("R,DevAddr08hRegAddr61h")]
        public string DevAddr08hRegAddr61h;
        [Description("R,DevAddr08hRegAddr62h")]
        public string DevAddr08hRegAddr62h;
        [Description("R,DevAddr08hRegAddr63h")]
        public string DevAddr08hRegAddr63h;
        [Description("R,DevAddr08hRegAddr64h")]
        public string DevAddr08hRegAddr64h;
        [Description("R,DevAddr08hRegAddr65h")]
        public string DevAddr08hRegAddr65h;
        [Description("R,DevAddr08hRegAddr66h")]
        public string DevAddr08hRegAddr66h;
        [Description("R,DevAddr08hRegAddr67h")]
        public string DevAddr08hRegAddr67h;
        [Description("R,DevAddr08hRegAddr70h")]
        public string DevAddr08hRegAddr70h;
        [Description("R,DevAddr08hRegAddr71h")]
        public string DevAddr08hRegAddr71h;
        [Description("R,DevAddr08hRegAddr72h")]
        public string DevAddr08hRegAddr72h;
        [Description("R,DevAddr08hRegAddr73h")]
        public string DevAddr08hRegAddr73h;
        [Description("R,DevAddr08hRegAddr74h")]
        public string DevAddr08hRegAddr74h;
        [Description("R,DevAddr08hRegAddr75h")]
        public string DevAddr08hRegAddr75h;
        [Description("R,DevAddr08hRegAddr76h")]
        public string DevAddr08hRegAddr76h;
        [Description("R,DevAddr08hRegAddr77h")]
        public string DevAddr08hRegAddr77h;
        [Description("R,DevAddr08hRegAddr78h")]
        public string DevAddr08hRegAddr78h;
        [Description("R,DevAddr08hRegAddr79h")]
        public string DevAddr08hRegAddr79h;
        [Description("R,DevAddr08hRegAddr7Ah")]
        public string DevAddr08hRegAddr7Ah;
        [Description("R,DevAddr08hRegAddr7Bh")]
        public string DevAddr08hRegAddr7Bh;
        [Description("R,DevAddr08hRegAddr7Ch")]
        public string DevAddr08hRegAddr7Ch;
        [Description("R,DevAddr08hRegAddr7Dh")]
        public string DevAddr08hRegAddr7Dh;
        [Description("R,DevAddr08hRegAddr7Eh")]
        public string DevAddr08hRegAddr7Eh;
        [Description("R,DevAddr08hRegAddr7Fh")]
        public string DevAddr08hRegAddr7Fh;
        [Description("R,DevAddr08hRegAddr80h")]
        public string DevAddr08hRegAddr80h;
        [Description("R,DevAddr08hRegAddr81h")]
        public string DevAddr08hRegAddr81h;
        [Description("R,DevAddr08hRegAddr82h")]
        public string DevAddr08hRegAddr82h;
        [Description("R,DevAddr08hRegAddr83h")]
        public string DevAddr08hRegAddr83h;
        [Description("R,DevAddr08hRegAddr84h")]
        public string DevAddr08hRegAddr84h;
        [Description("R,DevAddr08hRegAddr85h")]
        public string DevAddr08hRegAddr85h;
        [Description("R,DevAddr08hRegAddr86h")]
        public string DevAddr08hRegAddr86h;
        [Description("R,DevAddr08hRegAddr87h")]
        public string DevAddr08hRegAddr87h;

        [Description("R,DevAddr09hRegAddr00h")]
        public string DevAddr09hRegAddr00h;
        [Description("R,DevAddr09hRegAddr01h")]
        public string DevAddr09hRegAddr01h;
        [Description("R,DevAddr09hRegAddr02h")]
        public string DevAddr09hRegAddr02h;
        [Description("R,DevAddr09hRegAddr03h")]
        public string DevAddr09hRegAddr03h;
        [Description("R,DevAddr09hRegAddr04h")]
        public string DevAddr09hRegAddr04h;
        [Description("R,DevAddr09hRegAddr05h")]
        public string DevAddr09hRegAddr05h;
        [Description("R,DevAddr09hRegAddr06h")]
        public string DevAddr09hRegAddr06h;
        [Description("R,DevAddr09hRegAddr07h")]
        public string DevAddr09hRegAddr07h;
        [Description("R,DevAddr09hRegAddr08h")]
        public string DevAddr09hRegAddr08h;
        [Description("R,DevAddr09hRegAddr09h")]
        public string DevAddr09hRegAddr09h;
        [Description("R,DevAddr09hRegAddr0Ah")]
        public string DevAddr09hRegAddr0Ah;
        [Description("R,DevAddr09hRegAddr0Bh")]
        public string DevAddr09hRegAddr0Bh;
        [Description("R,DevAddr09hRegAddr0Ch")]
        public string DevAddr09hRegAddr0Ch;
        [Description("R,DevAddr09hRegAddr0Dh")]
        public string DevAddr09hRegAddr0Dh;
        [Description("R,DevAddr09hRegAddr0Eh")]
        public string DevAddr09hRegAddr0Eh;
        [Description("R,DevAddr09hRegAddr0Fh")]
        public string DevAddr09hRegAddr0Fh;
        [Description("R,DevAddr09hRegAddr10h")]
        public string DevAddr09hRegAddr10h;
        [Description("R,DevAddr09hRegAddr11h")]
        public string DevAddr09hRegAddr11h;
        [Description("R,DevAddr09hRegAddr12h")]
        public string DevAddr09hRegAddr12h;
        [Description("R,DevAddr09hRegAddr13h")]
        public string DevAddr09hRegAddr13h;
        [Description("R,DevAddr09hRegAddr14h")]
        public string DevAddr09hRegAddr14h;
        [Description("R,DevAddr09hRegAddr15h")]
        public string DevAddr09hRegAddr15h;
        [Description("R,DevAddr09hRegAddr16h")]
        public string DevAddr09hRegAddr16h;
        [Description("R,DevAddr09hRegAddr17h")]
        public string DevAddr09hRegAddr17h;
        [Description("R,DevAddr09hRegAddr20h")]
        public string DevAddr09hRegAddr20h;
        [Description("R,DevAddr09hRegAddr21h")]
        public string DevAddr09hRegAddr21h;
        [Description("R,DevAddr09hRegAddr22h")]
        public string DevAddr09hRegAddr22h;
        [Description("R,DevAddr09hRegAddr23h")]
        public string DevAddr09hRegAddr23h;
        [Description("R,DevAddr09hRegAddr24h")]
        public string DevAddr09hRegAddr24h;
        [Description("R,DevAddr09hRegAddr25h")]
        public string DevAddr09hRegAddr25h;
        [Description("R,DevAddr09hRegAddr26h")]
        public string DevAddr09hRegAddr26h;
        [Description("R,DevAddr09hRegAddr27h")]
        public string DevAddr09hRegAddr27h;
        [Description("R,DevAddr09hRegAddr28h")]
        public string DevAddr09hRegAddr28h;
        [Description("R,DevAddr09hRegAddr29h")]
        public string DevAddr09hRegAddr29h;
        [Description("R,DevAddr09hRegAddr2Ah")]
        public string DevAddr09hRegAddr2Ah;
        [Description("R,DevAddr09hRegAddr2Bh")]
        public string DevAddr09hRegAddr2Bh;
        [Description("R,DevAddr09hRegAddr2Ch")]
        public string DevAddr09hRegAddr2Ch;
        [Description("R,DevAddr09hRegAddr2Dh")]
        public string DevAddr09hRegAddr2Dh;
        [Description("R,DevAddr09hRegAddr2Eh")]
        public string DevAddr09hRegAddr2Eh;
        [Description("R,DevAddr09hRegAddr2Fh")]
        public string DevAddr09hRegAddr2Fh;
        [Description("R,DevAddr09hRegAddr30h")]
        public string DevAddr09hRegAddr30h;
        [Description("R,DevAddr09hRegAddr31h")]
        public string DevAddr09hRegAddr31h;
        [Description("R,DevAddr09hRegAddr32h")]
        public string DevAddr09hRegAddr32h;
        [Description("R,DevAddr09hRegAddr33h")]
        public string DevAddr09hRegAddr33h;
        [Description("R,DevAddr09hRegAddr34h")]
        public string DevAddr09hRegAddr34h;
        [Description("R,DevAddr09hRegAddr35h")]
        public string DevAddr09hRegAddr35h;
        [Description("R,DevAddr09hRegAddr36h")]
        public string DevAddr09hRegAddr36h;
        [Description("R,DevAddr09hRegAddr37h")]
        public string DevAddr09hRegAddr37h;
        [Description("R,DevAddr09hRegAddr40h")]
        public string DevAddr09hRegAddr40h;
        [Description("R,DevAddr09hRegAddr41h")]
        public string DevAddr09hRegAddr41h;
        [Description("R,DevAddr09hRegAddr42h")]
        public string DevAddr09hRegAddr42h;
        [Description("R,DevAddr09hRegAddr43h")]
        public string DevAddr09hRegAddr43h;
        [Description("R,DevAddr09hRegAddr44h")]
        public string DevAddr09hRegAddr44h;
        [Description("R,DevAddr09hRegAddr50h")]
        public string DevAddr09hRegAddr50h;
        [Description("R,DevAddr09hRegAddr51h")]
        public string DevAddr09hRegAddr51h;
        [Description("R,DevAddr09hRegAddr52h")]
        public string DevAddr09hRegAddr52h;
        [Description("R,DevAddr09hRegAddr53h")]
        public string DevAddr09hRegAddr53h;
        [Description("R,DevAddr09hRegAddr54h")]
        public string DevAddr09hRegAddr54h;
        [Description("R,DevAddr09hRegAddr55h")]
        public string DevAddr09hRegAddr55h;
        [Description("R,DevAddr09hRegAddr56h")]
        public string DevAddr09hRegAddr56h;
        [Description("R,DevAddr09hRegAddr57h")]
        public string DevAddr09hRegAddr57h;
        [Description("R,DevAddr09hRegAddr58h")]
        public string DevAddr09hRegAddr58h;
        [Description("R,DevAddr09hRegAddr59h")]
        public string DevAddr09hRegAddr59h;
        [Description("R,DevAddr09hRegAddr5Ah")]
        public string DevAddr09hRegAddr5Ah;
        [Description("R,DevAddr09hRegAddr5Bh")]
        public string DevAddr09hRegAddr5Bh;
        [Description("R,DevAddr09hRegAddr5Ch")]
        public string DevAddr09hRegAddr5Ch;
        [Description("R,DevAddr09hRegAddr5Dh")]
        public string DevAddr09hRegAddr5Dh;
        [Description("R,DevAddr09hRegAddr5Eh")]
        public string DevAddr09hRegAddr5Eh;
        [Description("R,DevAddr09hRegAddr5Fh")]
        public string DevAddr09hRegAddr5Fh;
        [Description("R,DevAddr09hRegAddr60h")]
        public string DevAddr09hRegAddr60h;
        [Description("R,DevAddr09hRegAddr61h")]
        public string DevAddr09hRegAddr61h;
        [Description("R,DevAddr09hRegAddr62h")]
        public string DevAddr09hRegAddr62h;
        [Description("R,DevAddr09hRegAddr63h")]
        public string DevAddr09hRegAddr63h;
        [Description("R,DevAddr09hRegAddr64h")]
        public string DevAddr09hRegAddr64h;
        [Description("R,DevAddr09hRegAddr65h")]
        public string DevAddr09hRegAddr65h;
        [Description("R,DevAddr09hRegAddr66h")]
        public string DevAddr09hRegAddr66h;
        [Description("R,DevAddr09hRegAddr67h")]
        public string DevAddr09hRegAddr67h;
        [Description("R,DevAddr09hRegAddr70h")]
        public string DevAddr09hRegAddr70h;
        [Description("R,DevAddr09hRegAddr71h")]
        public string DevAddr09hRegAddr71h;
        [Description("R,DevAddr09hRegAddr72h")]
        public string DevAddr09hRegAddr72h;
        [Description("R,DevAddr09hRegAddr73h")]
        public string DevAddr09hRegAddr73h;
        [Description("R,DevAddr09hRegAddr74h")]
        public string DevAddr09hRegAddr74h;
        [Description("R,DevAddr09hRegAddr75h")]
        public string DevAddr09hRegAddr75h;
        [Description("R,DevAddr09hRegAddr76h")]
        public string DevAddr09hRegAddr76h;
        [Description("R,DevAddr09hRegAddr77h")]
        public string DevAddr09hRegAddr77h;
        [Description("R,DevAddr09hRegAddr78h")]
        public string DevAddr09hRegAddr78h;
        [Description("R,DevAddr09hRegAddr79h")]
        public string DevAddr09hRegAddr79h;
        [Description("R,DevAddr09hRegAddr7Ah")]
        public string DevAddr09hRegAddr7Ah;
        [Description("R,DevAddr09hRegAddr7Bh")]
        public string DevAddr09hRegAddr7Bh;
        [Description("R,DevAddr09hRegAddr7Ch")]
        public string DevAddr09hRegAddr7Ch;
        [Description("R,DevAddr09hRegAddr7Dh")]
        public string DevAddr09hRegAddr7Dh;
        [Description("R,DevAddr09hRegAddr7Eh")]
        public string DevAddr09hRegAddr7Eh;
        [Description("R,DevAddr09hRegAddr7Fh")]
        public string DevAddr09hRegAddr7Fh;
        [Description("R,DevAddr09hRegAddr80h")]
        public string DevAddr09hRegAddr80h;
        [Description("R,DevAddr09hRegAddr81h")]
        public string DevAddr09hRegAddr81h;
        [Description("R,DevAddr09hRegAddr82h")]
        public string DevAddr09hRegAddr82h;
        [Description("R,DevAddr09hRegAddr83h")]
        public string DevAddr09hRegAddr83h;
        [Description("R,DevAddr09hRegAddr84h")]
        public string DevAddr09hRegAddr84h;
        [Description("R,DevAddr09hRegAddr85h")]
        public string DevAddr09hRegAddr85h;
        [Description("R,DevAddr09hRegAddr86h")]
        public string DevAddr09hRegAddr86h;
        [Description("R,DevAddr09hRegAddr87h")]
        public string DevAddr09hRegAddr87h;

        [Description("R,DevAddr0AhRegAddr00h")]
        public string DevAddr0AhRegAddr00h;
        [Description("R,DevAddr0AhRegAddr01h")]
        public string DevAddr0AhRegAddr01h;
        [Description("R,DevAddr0AhRegAddr02h")]
        public string DevAddr0AhRegAddr02h;
        [Description("R,DevAddr0AhRegAddr03h")]
        public string DevAddr0AhRegAddr03h;
        [Description("R,DevAddr0AhRegAddr04h")]
        public string DevAddr0AhRegAddr04h;
        [Description("R,DevAddr0AhRegAddr05h")]
        public string DevAddr0AhRegAddr05h;
        [Description("R,DevAddr0AhRegAddr06h")]
        public string DevAddr0AhRegAddr06h;
        [Description("R,DevAddr0AhRegAddr07h")]
        public string DevAddr0AhRegAddr07h;
        [Description("R,DevAddr0AhRegAddr08h")]
        public string DevAddr0AhRegAddr08h;
        [Description("R,DevAddr0AhRegAddr09h")]
        public string DevAddr0AhRegAddr09h;
        [Description("R,DevAddr0AhRegAddr0Ah")]
        public string DevAddr0AhRegAddr0Ah;
        [Description("R,DevAddr0AhRegAddr0Bh")]
        public string DevAddr0AhRegAddr0Bh;
        [Description("R,DevAddr0AhRegAddr0Ch")]
        public string DevAddr0AhRegAddr0Ch;
        [Description("R,DevAddr0AhRegAddr0Dh")]
        public string DevAddr0AhRegAddr0Dh;
        [Description("R,DevAddr0AhRegAddr0Eh")]
        public string DevAddr0AhRegAddr0Eh;
        [Description("R,DevAddr0AhRegAddr0Fh")]
        public string DevAddr0AhRegAddr0Fh;
        [Description("R,DevAddr0AhRegAddr10h")]
        public string DevAddr0AhRegAddr10h;
        [Description("R,DevAddr0AhRegAddr11h")]
        public string DevAddr0AhRegAddr11h;
        [Description("R,DevAddr0AhRegAddr12h")]
        public string DevAddr0AhRegAddr12h;
        [Description("R,DevAddr0AhRegAddr13h")]
        public string DevAddr0AhRegAddr13h;
        [Description("R,DevAddr0AhRegAddr14h")]
        public string DevAddr0AhRegAddr14h;
        [Description("R,DevAddr0AhRegAddr15h")]
        public string DevAddr0AhRegAddr15h;
        [Description("R,DevAddr0AhRegAddr16h")]
        public string DevAddr0AhRegAddr16h;
        [Description("R,DevAddr0AhRegAddr17h")]
        public string DevAddr0AhRegAddr17h;
        [Description("R,DevAddr0AhRegAddr20h")]
        public string DevAddr0AhRegAddr20h;
        [Description("R,DevAddr0AhRegAddr21h")]
        public string DevAddr0AhRegAddr21h;
        [Description("R,DevAddr0AhRegAddr22h")]
        public string DevAddr0AhRegAddr22h;
        [Description("R,DevAddr0AhRegAddr23h")]
        public string DevAddr0AhRegAddr23h;
        [Description("R,DevAddr0AhRegAddr24h")]
        public string DevAddr0AhRegAddr24h;
        [Description("R,DevAddr0AhRegAddr25h")]
        public string DevAddr0AhRegAddr25h;
        [Description("R,DevAddr0AhRegAddr26h")]
        public string DevAddr0AhRegAddr26h;
        [Description("R,DevAddr0AhRegAddr27h")]
        public string DevAddr0AhRegAddr27h;
        [Description("R,DevAddr0AhRegAddr28h")]
        public string DevAddr0AhRegAddr28h;
        [Description("R,DevAddr0AhRegAddr29h")]
        public string DevAddr0AhRegAddr29h;
        [Description("R,DevAddr0AhRegAddr2Ah")]
        public string DevAddr0AhRegAddr2Ah;
        [Description("R,DevAddr0AhRegAddr2Bh")]
        public string DevAddr0AhRegAddr2Bh;
        [Description("R,DevAddr0AhRegAddr2Ch")]
        public string DevAddr0AhRegAddr2Ch;
        [Description("R,DevAddr0AhRegAddr2Dh")]
        public string DevAddr0AhRegAddr2Dh;
        [Description("R,DevAddr0AhRegAddr2Eh")]
        public string DevAddr0AhRegAddr2Eh;
        [Description("R,DevAddr0AhRegAddr2Fh")]
        public string DevAddr0AhRegAddr2Fh;
        [Description("R,DevAddr0AhRegAddr30h")]
        public string DevAddr0AhRegAddr30h;
        [Description("R,DevAddr0AhRegAddr31h")]
        public string DevAddr0AhRegAddr31h;
        [Description("R,DevAddr0AhRegAddr32h")]
        public string DevAddr0AhRegAddr32h;
        [Description("R,DevAddr0AhRegAddr33h")]
        public string DevAddr0AhRegAddr33h;
        [Description("R,DevAddr0AhRegAddr34h")]
        public string DevAddr0AhRegAddr34h;
        [Description("R,DevAddr0AhRegAddr35h")]
        public string DevAddr0AhRegAddr35h;
        [Description("R,DevAddr0AhRegAddr36h")]
        public string DevAddr0AhRegAddr36h;
        [Description("R,DevAddr0AhRegAddr37h")]
        public string DevAddr0AhRegAddr37h;
        [Description("R,DevAddr0AhRegAddr40h")]
        public string DevAddr0AhRegAddr40h;
        [Description("R,DevAddr0AhRegAddr41h")]
        public string DevAddr0AhRegAddr41h;
        [Description("R,DevAddr0AhRegAddr42h")]
        public string DevAddr0AhRegAddr42h;
        [Description("R,DevAddr0AhRegAddr43h")]
        public string DevAddr0AhRegAddr43h;
        [Description("R,DevAddr0AhRegAddr44h")]
        public string DevAddr0AhRegAddr44h;
        [Description("R,DevAddr0AhRegAddr50h")]
        public string DevAddr0AhRegAddr50h;
        [Description("R,DevAddr0AhRegAddr51h")]
        public string DevAddr0AhRegAddr51h;
        [Description("R,DevAddr0AhRegAddr52h")]
        public string DevAddr0AhRegAddr52h;
        [Description("R,DevAddr0AhRegAddr53h")]
        public string DevAddr0AhRegAddr53h;
        [Description("R,DevAddr0AhRegAddr54h")]
        public string DevAddr0AhRegAddr54h;
        [Description("R,DevAddr0AhRegAddr55h")]
        public string DevAddr0AhRegAddr55h;
        [Description("R,DevAddr0AhRegAddr56h")]
        public string DevAddr0AhRegAddr56h;
        [Description("R,DevAddr0AhRegAddr57h")]
        public string DevAddr0AhRegAddr57h;
        [Description("R,DevAddr0AhRegAddr58h")]
        public string DevAddr0AhRegAddr58h;
        [Description("R,DevAddr0AhRegAddr59h")]
        public string DevAddr0AhRegAddr59h;
        [Description("R,DevAddr0AhRegAddr5Ah")]
        public string DevAddr0AhRegAddr5Ah;
        [Description("R,DevAddr0AhRegAddr5Bh")]
        public string DevAddr0AhRegAddr5Bh;
        [Description("R,DevAddr0AhRegAddr5Ch")]
        public string DevAddr0AhRegAddr5Ch;
        [Description("R,DevAddr0AhRegAddr5Dh")]
        public string DevAddr0AhRegAddr5Dh;
        [Description("R,DevAddr0AhRegAddr5Eh")]
        public string DevAddr0AhRegAddr5Eh;
        [Description("R,DevAddr0AhRegAddr5Fh")]
        public string DevAddr0AhRegAddr5Fh;
        [Description("R,DevAddr0AhRegAddr60h")]
        public string DevAddr0AhRegAddr60h;
        [Description("R,DevAddr0AhRegAddr61h")]
        public string DevAddr0AhRegAddr61h;
        [Description("R,DevAddr0AhRegAddr62h")]
        public string DevAddr0AhRegAddr62h;
        [Description("R,DevAddr0AhRegAddr63h")]
        public string DevAddr0AhRegAddr63h;
        [Description("R,DevAddr0AhRegAddr64h")]
        public string DevAddr0AhRegAddr64h;
        [Description("R,DevAddr0AhRegAddr65h")]
        public string DevAddr0AhRegAddr65h;
        [Description("R,DevAddr0AhRegAddr66h")]
        public string DevAddr0AhRegAddr66h;
        [Description("R,DevAddr0AhRegAddr67h")]
        public string DevAddr0AhRegAddr67h;
        [Description("R,DevAddr0AhRegAddr70h")]
        public string DevAddr0AhRegAddr70h;
        [Description("R,DevAddr0AhRegAddr71h")]
        public string DevAddr0AhRegAddr71h;
        [Description("R,DevAddr0AhRegAddr72h")]
        public string DevAddr0AhRegAddr72h;
        [Description("R,DevAddr0AhRegAddr73h")]
        public string DevAddr0AhRegAddr73h;
        [Description("R,DevAddr0AhRegAddr74h")]
        public string DevAddr0AhRegAddr74h;
        [Description("R,DevAddr0AhRegAddr75h")]
        public string DevAddr0AhRegAddr75h;
        [Description("R,DevAddr0AhRegAddr76h")]
        public string DevAddr0AhRegAddr76h;
        [Description("R,DevAddr0AhRegAddr77h")]
        public string DevAddr0AhRegAddr77h;
        [Description("R,DevAddr0AhRegAddr78h")]
        public string DevAddr0AhRegAddr78h;
        [Description("R,DevAddr0AhRegAddr79h")]
        public string DevAddr0AhRegAddr79h;
        [Description("R,DevAddr0AhRegAddr7Ah")]
        public string DevAddr0AhRegAddr7Ah;
        [Description("R,DevAddr0AhRegAddr7Bh")]
        public string DevAddr0AhRegAddr7Bh;
        [Description("R,DevAddr0AhRegAddr7Ch")]
        public string DevAddr0AhRegAddr7Ch;
        [Description("R,DevAddr0AhRegAddr7Dh")]
        public string DevAddr0AhRegAddr7Dh;
        [Description("R,DevAddr0AhRegAddr7Eh")]
        public string DevAddr0AhRegAddr7Eh;
        [Description("R,DevAddr0AhRegAddr7Fh")]
        public string DevAddr0AhRegAddr7Fh;
        [Description("R,DevAddr0AhRegAddr80h")]
        public string DevAddr0AhRegAddr80h;
        [Description("R,DevAddr0AhRegAddr81h")]
        public string DevAddr0AhRegAddr81h;
        [Description("R,DevAddr0AhRegAddr82h")]
        public string DevAddr0AhRegAddr82h;
        [Description("R,DevAddr0AhRegAddr83h")]
        public string DevAddr0AhRegAddr83h;
        [Description("R,DevAddr0AhRegAddr84h")]
        public string DevAddr0AhRegAddr84h;
        [Description("R,DevAddr0AhRegAddr85h")]
        public string DevAddr0AhRegAddr85h;
        [Description("R,DevAddr0AhRegAddr86h")]
        public string DevAddr0AhRegAddr86h;
        [Description("R,DevAddr0AhRegAddr87h")]
        public string DevAddr0AhRegAddr87h;

        [Description("R,DevAddr0BhRegAddr00h")]
        public string DevAddr0BhRegAddr00h;
        [Description("R,DevAddr0BhRegAddr01h")]
        public string DevAddr0BhRegAddr01h;
        [Description("R,DevAddr0BhRegAddr02h")]
        public string DevAddr0BhRegAddr02h;
        [Description("R,DevAddr0BhRegAddr03h")]
        public string DevAddr0BhRegAddr03h;
        [Description("R,DevAddr0BhRegAddr04h")]
        public string DevAddr0BhRegAddr04h;
        [Description("R,DevAddr0BhRegAddr05h")]
        public string DevAddr0BhRegAddr05h;
        [Description("R,DevAddr0BhRegAddr06h")]
        public string DevAddr0BhRegAddr06h;
        [Description("R,DevAddr0BhRegAddr07h")]
        public string DevAddr0BhRegAddr07h;
        [Description("R,DevAddr0BhRegAddr08h")]
        public string DevAddr0BhRegAddr08h;
        [Description("R,DevAddr0BhRegAddr09h")]
        public string DevAddr0BhRegAddr09h;
        [Description("R,DevAddr0BhRegAddr0Ah")]
        public string DevAddr0BhRegAddr0Ah;
        [Description("R,DevAddr0BhRegAddr0Bh")]
        public string DevAddr0BhRegAddr0Bh;
        [Description("R,DevAddr0BhRegAddr0Ch")]
        public string DevAddr0BhRegAddr0Ch;
        [Description("R,DevAddr0BhRegAddr0Dh")]
        public string DevAddr0BhRegAddr0Dh;
        [Description("R,DevAddr0BhRegAddr0Eh")]
        public string DevAddr0BhRegAddr0Eh;
        [Description("R,DevAddr0BhRegAddr0Fh")]
        public string DevAddr0BhRegAddr0Fh;
        [Description("R,DevAddr0BhRegAddr10h")]
        public string DevAddr0BhRegAddr10h;
        [Description("R,DevAddr0BhRegAddr11h")]
        public string DevAddr0BhRegAddr11h;
        [Description("R,DevAddr0BhRegAddr12h")]
        public string DevAddr0BhRegAddr12h;
        [Description("R,DevAddr0BhRegAddr13h")]
        public string DevAddr0BhRegAddr13h;
        [Description("R,DevAddr0BhRegAddr14h")]
        public string DevAddr0BhRegAddr14h;
        [Description("R,DevAddr0BhRegAddr15h")]
        public string DevAddr0BhRegAddr15h;
        [Description("R,DevAddr0BhRegAddr16h")]
        public string DevAddr0BhRegAddr16h;
        [Description("R,DevAddr0BhRegAddr17h")]
        public string DevAddr0BhRegAddr17h;
        [Description("R,DevAddr0BhRegAddr20h")]
        public string DevAddr0BhRegAddr20h;
        [Description("R,DevAddr0BhRegAddr21h")]
        public string DevAddr0BhRegAddr21h;
        [Description("R,DevAddr0BhRegAddr22h")]
        public string DevAddr0BhRegAddr22h;
        [Description("R,DevAddr0BhRegAddr23h")]
        public string DevAddr0BhRegAddr23h;
        [Description("R,DevAddr0BhRegAddr24h")]
        public string DevAddr0BhRegAddr24h;
        [Description("R,DevAddr0BhRegAddr25h")]
        public string DevAddr0BhRegAddr25h;
        [Description("R,DevAddr0BhRegAddr26h")]
        public string DevAddr0BhRegAddr26h;
        [Description("R,DevAddr0BhRegAddr27h")]
        public string DevAddr0BhRegAddr27h;
        [Description("R,DevAddr0BhRegAddr28h")]
        public string DevAddr0BhRegAddr28h;
        [Description("R,DevAddr0BhRegAddr29h")]
        public string DevAddr0BhRegAddr29h;
        [Description("R,DevAddr0BhRegAddr2Ah")]
        public string DevAddr0BhRegAddr2Ah;
        [Description("R,DevAddr0BhRegAddr2Bh")]
        public string DevAddr0BhRegAddr2Bh;
        [Description("R,DevAddr0BhRegAddr2Ch")]
        public string DevAddr0BhRegAddr2Ch;
        [Description("R,DevAddr0BhRegAddr2Dh")]
        public string DevAddr0BhRegAddr2Dh;
        [Description("R,DevAddr0BhRegAddr2Eh")]
        public string DevAddr0BhRegAddr2Eh;
        [Description("R,DevAddr0BhRegAddr2Fh")]
        public string DevAddr0BhRegAddr2Fh;
        [Description("R,DevAddr0BhRegAddr30h")]
        public string DevAddr0BhRegAddr30h;
        [Description("R,DevAddr0BhRegAddr31h")]
        public string DevAddr0BhRegAddr31h;
        [Description("R,DevAddr0BhRegAddr32h")]
        public string DevAddr0BhRegAddr32h;
        [Description("R,DevAddr0BhRegAddr33h")]
        public string DevAddr0BhRegAddr33h;
        [Description("R,DevAddr0BhRegAddr34h")]
        public string DevAddr0BhRegAddr34h;
        [Description("R,DevAddr0BhRegAddr35h")]
        public string DevAddr0BhRegAddr35h;
        [Description("R,DevAddr0BhRegAddr36h")]
        public string DevAddr0BhRegAddr36h;
        [Description("R,DevAddr0BhRegAddr37h")]
        public string DevAddr0BhRegAddr37h;
        [Description("R,DevAddr0BhRegAddr40h")]
        public string DevAddr0BhRegAddr40h;
        [Description("R,DevAddr0BhRegAddr41h")]
        public string DevAddr0BhRegAddr41h;
        [Description("R,DevAddr0BhRegAddr42h")]
        public string DevAddr0BhRegAddr42h;
        [Description("R,DevAddr0BhRegAddr43h")]
        public string DevAddr0BhRegAddr43h;
        [Description("R,DevAddr0BhRegAddr44h")]
        public string DevAddr0BhRegAddr44h;
        [Description("R,DevAddr0BhRegAddr50h")]
        public string DevAddr0BhRegAddr50h;
        [Description("R,DevAddr0BhRegAddr51h")]
        public string DevAddr0BhRegAddr51h;
        [Description("R,DevAddr0BhRegAddr52h")]
        public string DevAddr0BhRegAddr52h;
        [Description("R,DevAddr0BhRegAddr53h")]
        public string DevAddr0BhRegAddr53h;
        [Description("R,DevAddr0BhRegAddr54h")]
        public string DevAddr0BhRegAddr54h;
        [Description("R,DevAddr0BhRegAddr55h")]
        public string DevAddr0BhRegAddr55h;
        [Description("R,DevAddr0BhRegAddr56h")]
        public string DevAddr0BhRegAddr56h;
        [Description("R,DevAddr0BhRegAddr57h")]
        public string DevAddr0BhRegAddr57h;
        [Description("R,DevAddr0BhRegAddr58h")]
        public string DevAddr0BhRegAddr58h;
        [Description("R,DevAddr0BhRegAddr59h")]
        public string DevAddr0BhRegAddr59h;
        [Description("R,DevAddr0BhRegAddr5Ah")]
        public string DevAddr0BhRegAddr5Ah;
        [Description("R,DevAddr0BhRegAddr5Bh")]
        public string DevAddr0BhRegAddr5Bh;
        [Description("R,DevAddr0BhRegAddr5Ch")]
        public string DevAddr0BhRegAddr5Ch;
        [Description("R,DevAddr0BhRegAddr5Dh")]
        public string DevAddr0BhRegAddr5Dh;
        [Description("R,DevAddr0BhRegAddr5Eh")]
        public string DevAddr0BhRegAddr5Eh;
        [Description("R,DevAddr0BhRegAddr5Fh")]
        public string DevAddr0BhRegAddr5Fh;
        [Description("R,DevAddr0BhRegAddr60h")]
        public string DevAddr0BhRegAddr60h;
        [Description("R,DevAddr0BhRegAddr61h")]
        public string DevAddr0BhRegAddr61h;
        [Description("R,DevAddr0BhRegAddr62h")]
        public string DevAddr0BhRegAddr62h;
        [Description("R,DevAddr0BhRegAddr63h")]
        public string DevAddr0BhRegAddr63h;
        [Description("R,DevAddr0BhRegAddr64h")]
        public string DevAddr0BhRegAddr64h;
        [Description("R,DevAddr0BhRegAddr65h")]
        public string DevAddr0BhRegAddr65h;
        [Description("R,DevAddr0BhRegAddr66h")]
        public string DevAddr0BhRegAddr66h;
        [Description("R,DevAddr0BhRegAddr67h")]
        public string DevAddr0BhRegAddr67h;
        [Description("R,DevAddr0BhRegAddr70h")]
        public string DevAddr0BhRegAddr70h;
        [Description("R,DevAddr0BhRegAddr71h")]
        public string DevAddr0BhRegAddr71h;
        [Description("R,DevAddr0BhRegAddr72h")]
        public string DevAddr0BhRegAddr72h;
        [Description("R,DevAddr0BhRegAddr73h")]
        public string DevAddr0BhRegAddr73h;
        [Description("R,DevAddr0BhRegAddr74h")]
        public string DevAddr0BhRegAddr74h;
        [Description("R,DevAddr0BhRegAddr75h")]
        public string DevAddr0BhRegAddr75h;
        [Description("R,DevAddr0BhRegAddr76h")]
        public string DevAddr0BhRegAddr76h;
        [Description("R,DevAddr0BhRegAddr77h")]
        public string DevAddr0BhRegAddr77h;
        [Description("R,DevAddr0BhRegAddr78h")]
        public string DevAddr0BhRegAddr78h;
        [Description("R,DevAddr0BhRegAddr79h")]
        public string DevAddr0BhRegAddr79h;
        [Description("R,DevAddr0BhRegAddr7Ah")]
        public string DevAddr0BhRegAddr7Ah;
        [Description("R,DevAddr0BhRegAddr7Bh")]
        public string DevAddr0BhRegAddr7Bh;
        [Description("R,DevAddr0BhRegAddr7Ch")]
        public string DevAddr0BhRegAddr7Ch;
        [Description("R,DevAddr0BhRegAddr7Dh")]
        public string DevAddr0BhRegAddr7Dh;
        [Description("R,DevAddr0BhRegAddr7Eh")]
        public string DevAddr0BhRegAddr7Eh;
        [Description("R,DevAddr0BhRegAddr7Fh")]
        public string DevAddr0BhRegAddr7Fh;
        [Description("R,DevAddr0BhRegAddr80h")]
        public string DevAddr0BhRegAddr80h;
        [Description("R,DevAddr0BhRegAddr81h")]
        public string DevAddr0BhRegAddr81h;
        [Description("R,DevAddr0BhRegAddr82h")]
        public string DevAddr0BhRegAddr82h;
        [Description("R,DevAddr0BhRegAddr83h")]
        public string DevAddr0BhRegAddr83h;
        [Description("R,DevAddr0BhRegAddr84h")]
        public string DevAddr0BhRegAddr84h;
        [Description("R,DevAddr0BhRegAddr85h")]
        public string DevAddr0BhRegAddr85h;
        [Description("R,DevAddr0BhRegAddr86h")]
        public string DevAddr0BhRegAddr86h;
        [Description("R,DevAddr0BhRegAddr87h")]
        public string DevAddr0BhRegAddr87h;

        [Description("R,DevAddr0ChRegAddr00h")]
        public string DevAddr0ChRegAddr00h;
        [Description("R,DevAddr0ChRegAddr01h")]
        public string DevAddr0ChRegAddr01h;
        [Description("R,DevAddr0ChRegAddr02h")]
        public string DevAddr0ChRegAddr02h;
        [Description("R,DevAddr0ChRegAddr03h")]
        public string DevAddr0ChRegAddr03h;
        [Description("R,DevAddr0ChRegAddr04h")]
        public string DevAddr0ChRegAddr04h;
        [Description("R,DevAddr0ChRegAddr05h")]
        public string DevAddr0ChRegAddr05h;
        [Description("R,DevAddr0ChRegAddr06h")]
        public string DevAddr0ChRegAddr06h;
        [Description("R,DevAddr0ChRegAddr07h")]
        public string DevAddr0ChRegAddr07h;
        [Description("R,DevAddr0ChRegAddr08h")]
        public string DevAddr0ChRegAddr08h;
        [Description("R,DevAddr0ChRegAddr09h")]
        public string DevAddr0ChRegAddr09h;
        [Description("R,DevAddr0ChRegAddr0Ah")]
        public string DevAddr0ChRegAddr0Ah;
        [Description("R,DevAddr0ChRegAddr0Bh")]
        public string DevAddr0ChRegAddr0Bh;
        [Description("R,DevAddr0ChRegAddr0Ch")]
        public string DevAddr0ChRegAddr0Ch;
        [Description("R,DevAddr0ChRegAddr0Dh")]
        public string DevAddr0ChRegAddr0Dh;
        [Description("R,DevAddr0ChRegAddr0Eh")]
        public string DevAddr0ChRegAddr0Eh;
        [Description("R,DevAddr0ChRegAddr0Fh")]
        public string DevAddr0ChRegAddr0Fh;
        [Description("R,DevAddr0ChRegAddr10h")]
        public string DevAddr0ChRegAddr10h;
        [Description("R,DevAddr0ChRegAddr11h")]
        public string DevAddr0ChRegAddr11h;
        [Description("R,DevAddr0ChRegAddr12h")]
        public string DevAddr0ChRegAddr12h;
        [Description("R,DevAddr0ChRegAddr13h")]
        public string DevAddr0ChRegAddr13h;
        [Description("R,DevAddr0ChRegAddr14h")]
        public string DevAddr0ChRegAddr14h;
        [Description("R,DevAddr0ChRegAddr15h")]
        public string DevAddr0ChRegAddr15h;
        [Description("R,DevAddr0ChRegAddr16h")]
        public string DevAddr0ChRegAddr16h;
        [Description("R,DevAddr0ChRegAddr17h")]
        public string DevAddr0ChRegAddr17h;
        [Description("R,DevAddr0ChRegAddr20h")]
        public string DevAddr0ChRegAddr20h;
        [Description("R,DevAddr0ChRegAddr21h")]
        public string DevAddr0ChRegAddr21h;
        [Description("R,DevAddr0ChRegAddr22h")]
        public string DevAddr0ChRegAddr22h;
        [Description("R,DevAddr0ChRegAddr23h")]
        public string DevAddr0ChRegAddr23h;
        [Description("R,DevAddr0ChRegAddr24h")]
        public string DevAddr0ChRegAddr24h;
        [Description("R,DevAddr0ChRegAddr25h")]
        public string DevAddr0ChRegAddr25h;
        [Description("R,DevAddr0ChRegAddr26h")]
        public string DevAddr0ChRegAddr26h;
        [Description("R,DevAddr0ChRegAddr27h")]
        public string DevAddr0ChRegAddr27h;
        [Description("R,DevAddr0ChRegAddr28h")]
        public string DevAddr0ChRegAddr28h;
        [Description("R,DevAddr0ChRegAddr29h")]
        public string DevAddr0ChRegAddr29h;
        [Description("R,DevAddr0ChRegAddr2Ah")]
        public string DevAddr0ChRegAddr2Ah;
        [Description("R,DevAddr0ChRegAddr2Bh")]
        public string DevAddr0ChRegAddr2Bh;
        [Description("R,DevAddr0ChRegAddr2Ch")]
        public string DevAddr0ChRegAddr2Ch;
        [Description("R,DevAddr0ChRegAddr2Dh")]
        public string DevAddr0ChRegAddr2Dh;
        [Description("R,DevAddr0ChRegAddr2Eh")]
        public string DevAddr0ChRegAddr2Eh;
        [Description("R,DevAddr0ChRegAddr2Fh")]
        public string DevAddr0ChRegAddr2Fh;
        [Description("R,DevAddr0ChRegAddr30h")]
        public string DevAddr0ChRegAddr30h;
        [Description("R,DevAddr0ChRegAddr31h")]
        public string DevAddr0ChRegAddr31h;
        [Description("R,DevAddr0ChRegAddr32h")]
        public string DevAddr0ChRegAddr32h;
        [Description("R,DevAddr0ChRegAddr33h")]
        public string DevAddr0ChRegAddr33h;
        [Description("R,DevAddr0ChRegAddr34h")]
        public string DevAddr0ChRegAddr34h;
        [Description("R,DevAddr0ChRegAddr35h")]
        public string DevAddr0ChRegAddr35h;
        [Description("R,DevAddr0ChRegAddr36h")]
        public string DevAddr0ChRegAddr36h;
        [Description("R,DevAddr0ChRegAddr37h")]
        public string DevAddr0ChRegAddr37h;
        [Description("R,DevAddr0ChRegAddr40h")]
        public string DevAddr0ChRegAddr40h;
        [Description("R,DevAddr0ChRegAddr41h")]
        public string DevAddr0ChRegAddr41h;
        [Description("R,DevAddr0ChRegAddr42h")]
        public string DevAddr0ChRegAddr42h;
        [Description("R,DevAddr0ChRegAddr43h")]
        public string DevAddr0ChRegAddr43h;
        [Description("R,DevAddr0ChRegAddr44h")]
        public string DevAddr0ChRegAddr44h;
        [Description("R,DevAddr0ChRegAddr50h")]
        public string DevAddr0ChRegAddr50h;
        [Description("R,DevAddr0ChRegAddr51h")]
        public string DevAddr0ChRegAddr51h;
        [Description("R,DevAddr0ChRegAddr52h")]
        public string DevAddr0ChRegAddr52h;
        [Description("R,DevAddr0ChRegAddr53h")]
        public string DevAddr0ChRegAddr53h;
        [Description("R,DevAddr0ChRegAddr54h")]
        public string DevAddr0ChRegAddr54h;
        [Description("R,DevAddr0ChRegAddr55h")]
        public string DevAddr0ChRegAddr55h;
        [Description("R,DevAddr0ChRegAddr56h")]
        public string DevAddr0ChRegAddr56h;
        [Description("R,DevAddr0ChRegAddr57h")]
        public string DevAddr0ChRegAddr57h;
        [Description("R,DevAddr0ChRegAddr58h")]
        public string DevAddr0ChRegAddr58h;
        [Description("R,DevAddr0ChRegAddr59h")]
        public string DevAddr0ChRegAddr59h;
        [Description("R,DevAddr0ChRegAddr5Ah")]
        public string DevAddr0ChRegAddr5Ah;
        [Description("R,DevAddr0ChRegAddr5Bh")]
        public string DevAddr0ChRegAddr5Bh;
        [Description("R,DevAddr0ChRegAddr5Ch")]
        public string DevAddr0ChRegAddr5Ch;
        [Description("R,DevAddr0ChRegAddr5Dh")]
        public string DevAddr0ChRegAddr5Dh;
        [Description("R,DevAddr0ChRegAddr5Eh")]
        public string DevAddr0ChRegAddr5Eh;
        [Description("R,DevAddr0ChRegAddr5Fh")]
        public string DevAddr0ChRegAddr5Fh;
        [Description("R,DevAddr0ChRegAddr60h")]
        public string DevAddr0ChRegAddr60h;
        [Description("R,DevAddr0ChRegAddr61h")]
        public string DevAddr0ChRegAddr61h;
        [Description("R,DevAddr0ChRegAddr62h")]
        public string DevAddr0ChRegAddr62h;
        [Description("R,DevAddr0ChRegAddr63h")]
        public string DevAddr0ChRegAddr63h;
        [Description("R,DevAddr0ChRegAddr64h")]
        public string DevAddr0ChRegAddr64h;
        [Description("R,DevAddr0ChRegAddr65h")]
        public string DevAddr0ChRegAddr65h;
        [Description("R,DevAddr0ChRegAddr66h")]
        public string DevAddr0ChRegAddr66h;
        [Description("R,DevAddr0ChRegAddr67h")]
        public string DevAddr0ChRegAddr67h;
        [Description("R,DevAddr0ChRegAddr70h")]
        public string DevAddr0ChRegAddr70h;
        [Description("R,DevAddr0ChRegAddr71h")]
        public string DevAddr0ChRegAddr71h;
        [Description("R,DevAddr0ChRegAddr72h")]
        public string DevAddr0ChRegAddr72h;
        [Description("R,DevAddr0ChRegAddr73h")]
        public string DevAddr0ChRegAddr73h;
        [Description("R,DevAddr0ChRegAddr74h")]
        public string DevAddr0ChRegAddr74h;
        [Description("R,DevAddr0ChRegAddr75h")]
        public string DevAddr0ChRegAddr75h;
        [Description("R,DevAddr0ChRegAddr76h")]
        public string DevAddr0ChRegAddr76h;
        [Description("R,DevAddr0ChRegAddr77h")]
        public string DevAddr0ChRegAddr77h;
        [Description("R,DevAddr0ChRegAddr78h")]
        public string DevAddr0ChRegAddr78h;
        [Description("R,DevAddr0ChRegAddr79h")]
        public string DevAddr0ChRegAddr79h;
        [Description("R,DevAddr0ChRegAddr7Ah")]
        public string DevAddr0ChRegAddr7Ah;
        [Description("R,DevAddr0ChRegAddr7Bh")]
        public string DevAddr0ChRegAddr7Bh;
        [Description("R,DevAddr0ChRegAddr7Ch")]
        public string DevAddr0ChRegAddr7Ch;
        [Description("R,DevAddr0ChRegAddr7Dh")]
        public string DevAddr0ChRegAddr7Dh;
        [Description("R,DevAddr0ChRegAddr7Eh")]
        public string DevAddr0ChRegAddr7Eh;
        [Description("R,DevAddr0ChRegAddr7Fh")]
        public string DevAddr0ChRegAddr7Fh;
        [Description("R,DevAddr0ChRegAddr80h")]
        public string DevAddr0ChRegAddr80h;
        [Description("R,DevAddr0ChRegAddr81h")]
        public string DevAddr0ChRegAddr81h;
        [Description("R,DevAddr0ChRegAddr82h")]
        public string DevAddr0ChRegAddr82h;
        [Description("R,DevAddr0ChRegAddr83h")]
        public string DevAddr0ChRegAddr83h;
        [Description("R,DevAddr0ChRegAddr84h")]
        public string DevAddr0ChRegAddr84h;
        [Description("R,DevAddr0ChRegAddr85h")]
        public string DevAddr0ChRegAddr85h;
        [Description("R,DevAddr0ChRegAddr86h")]
        public string DevAddr0ChRegAddr86h;
        [Description("R,DevAddr0ChRegAddr87h")]
        public string DevAddr0ChRegAddr87h;

        [Description("R,DevAddr0DhRegAddr00h")]
        public string DevAddr0DhRegAddr00h;
        [Description("R,DevAddr0DhRegAddr01h")]
        public string DevAddr0DhRegAddr01h;
        [Description("R,DevAddr0DhRegAddr02h")]
        public string DevAddr0DhRegAddr02h;
        [Description("R,DevAddr0DhRegAddr03h")]
        public string DevAddr0DhRegAddr03h;
        [Description("R,DevAddr0DhRegAddr04h")]
        public string DevAddr0DhRegAddr04h;
        [Description("R,DevAddr0DhRegAddr05h")]
        public string DevAddr0DhRegAddr05h;
        [Description("R,DevAddr0DhRegAddr06h")]
        public string DevAddr0DhRegAddr06h;
        [Description("R,DevAddr0DhRegAddr07h")]
        public string DevAddr0DhRegAddr07h;
        [Description("R,DevAddr0DhRegAddr08h")]
        public string DevAddr0DhRegAddr08h;
        [Description("R,DevAddr0DhRegAddr09h")]
        public string DevAddr0DhRegAddr09h;
        [Description("R,DevAddr0DhRegAddr0Ah")]
        public string DevAddr0DhRegAddr0Ah;
        [Description("R,DevAddr0DhRegAddr0Bh")]
        public string DevAddr0DhRegAddr0Bh;
        [Description("R,DevAddr0DhRegAddr0Ch")]
        public string DevAddr0DhRegAddr0Ch;
        [Description("R,DevAddr0DhRegAddr0Dh")]
        public string DevAddr0DhRegAddr0Dh;
        [Description("R,DevAddr0DhRegAddr0Eh")]
        public string DevAddr0DhRegAddr0Eh;
        [Description("R,DevAddr0DhRegAddr0Fh")]
        public string DevAddr0DhRegAddr0Fh;
        [Description("R,DevAddr0DhRegAddr10h")]
        public string DevAddr0DhRegAddr10h;
        [Description("R,DevAddr0DhRegAddr11h")]
        public string DevAddr0DhRegAddr11h;
        [Description("R,DevAddr0DhRegAddr12h")]
        public string DevAddr0DhRegAddr12h;
        [Description("R,DevAddr0DhRegAddr13h")]
        public string DevAddr0DhRegAddr13h;
        [Description("R,DevAddr0DhRegAddr14h")]
        public string DevAddr0DhRegAddr14h;
        [Description("R,DevAddr0DhRegAddr15h")]
        public string DevAddr0DhRegAddr15h;
        [Description("R,DevAddr0DhRegAddr16h")]
        public string DevAddr0DhRegAddr16h;
        [Description("R,DevAddr0DhRegAddr17h")]
        public string DevAddr0DhRegAddr17h;
        [Description("R,DevAddr0DhRegAddr20h")]
        public string DevAddr0DhRegAddr20h;
        [Description("R,DevAddr0DhRegAddr21h")]
        public string DevAddr0DhRegAddr21h;
        [Description("R,DevAddr0DhRegAddr22h")]
        public string DevAddr0DhRegAddr22h;
        [Description("R,DevAddr0DhRegAddr23h")]
        public string DevAddr0DhRegAddr23h;
        [Description("R,DevAddr0DhRegAddr24h")]
        public string DevAddr0DhRegAddr24h;
        [Description("R,DevAddr0DhRegAddr25h")]
        public string DevAddr0DhRegAddr25h;
        [Description("R,DevAddr0DhRegAddr26h")]
        public string DevAddr0DhRegAddr26h;
        [Description("R,DevAddr0DhRegAddr27h")]
        public string DevAddr0DhRegAddr27h;
        [Description("R,DevAddr0DhRegAddr28h")]
        public string DevAddr0DhRegAddr28h;
        [Description("R,DevAddr0DhRegAddr29h")]
        public string DevAddr0DhRegAddr29h;
        [Description("R,DevAddr0DhRegAddr2Ah")]
        public string DevAddr0DhRegAddr2Ah;
        [Description("R,DevAddr0DhRegAddr2Bh")]
        public string DevAddr0DhRegAddr2Bh;
        [Description("R,DevAddr0DhRegAddr2Ch")]
        public string DevAddr0DhRegAddr2Ch;
        [Description("R,DevAddr0DhRegAddr2Dh")]
        public string DevAddr0DhRegAddr2Dh;
        [Description("R,DevAddr0DhRegAddr2Eh")]
        public string DevAddr0DhRegAddr2Eh;
        [Description("R,DevAddr0DhRegAddr2Fh")]
        public string DevAddr0DhRegAddr2Fh;
        [Description("R,DevAddr0DhRegAddr30h")]
        public string DevAddr0DhRegAddr30h;
        [Description("R,DevAddr0DhRegAddr31h")]
        public string DevAddr0DhRegAddr31h;
        [Description("R,DevAddr0DhRegAddr32h")]
        public string DevAddr0DhRegAddr32h;
        [Description("R,DevAddr0DhRegAddr33h")]
        public string DevAddr0DhRegAddr33h;
        [Description("R,DevAddr0DhRegAddr34h")]
        public string DevAddr0DhRegAddr34h;
        [Description("R,DevAddr0DhRegAddr35h")]
        public string DevAddr0DhRegAddr35h;
        [Description("R,DevAddr0DhRegAddr36h")]
        public string DevAddr0DhRegAddr36h;
        [Description("R,DevAddr0DhRegAddr37h")]
        public string DevAddr0DhRegAddr37h;
        [Description("R,DevAddr0DhRegAddr40h")]
        public string DevAddr0DhRegAddr40h;
        [Description("R,DevAddr0DhRegAddr41h")]
        public string DevAddr0DhRegAddr41h;
        [Description("R,DevAddr0DhRegAddr42h")]
        public string DevAddr0DhRegAddr42h;
        [Description("R,DevAddr0DhRegAddr43h")]
        public string DevAddr0DhRegAddr43h;
        [Description("R,DevAddr0DhRegAddr44h")]
        public string DevAddr0DhRegAddr44h;
        [Description("R,DevAddr0DhRegAddr50h")]
        public string DevAddr0DhRegAddr50h;
        [Description("R,DevAddr0DhRegAddr51h")]
        public string DevAddr0DhRegAddr51h;
        [Description("R,DevAddr0DhRegAddr52h")]
        public string DevAddr0DhRegAddr52h;
        [Description("R,DevAddr0DhRegAddr53h")]
        public string DevAddr0DhRegAddr53h;
        [Description("R,DevAddr0DhRegAddr54h")]
        public string DevAddr0DhRegAddr54h;
        [Description("R,DevAddr0DhRegAddr55h")]
        public string DevAddr0DhRegAddr55h;
        [Description("R,DevAddr0DhRegAddr56h")]
        public string DevAddr0DhRegAddr56h;
        [Description("R,DevAddr0DhRegAddr57h")]
        public string DevAddr0DhRegAddr57h;
        [Description("R,DevAddr0DhRegAddr58h")]
        public string DevAddr0DhRegAddr58h;
        [Description("R,DevAddr0DhRegAddr59h")]
        public string DevAddr0DhRegAddr59h;
        [Description("R,DevAddr0DhRegAddr5Ah")]
        public string DevAddr0DhRegAddr5Ah;
        [Description("R,DevAddr0DhRegAddr5Bh")]
        public string DevAddr0DhRegAddr5Bh;
        [Description("R,DevAddr0DhRegAddr5Ch")]
        public string DevAddr0DhRegAddr5Ch;
        [Description("R,DevAddr0DhRegAddr5Dh")]
        public string DevAddr0DhRegAddr5Dh;
        [Description("R,DevAddr0DhRegAddr5Eh")]
        public string DevAddr0DhRegAddr5Eh;
        [Description("R,DevAddr0DhRegAddr5Fh")]
        public string DevAddr0DhRegAddr5Fh;
        [Description("R,DevAddr0DhRegAddr60h")]
        public string DevAddr0DhRegAddr60h;
        [Description("R,DevAddr0DhRegAddr61h")]
        public string DevAddr0DhRegAddr61h;
        [Description("R,DevAddr0DhRegAddr62h")]
        public string DevAddr0DhRegAddr62h;
        [Description("R,DevAddr0DhRegAddr63h")]
        public string DevAddr0DhRegAddr63h;
        [Description("R,DevAddr0DhRegAddr64h")]
        public string DevAddr0DhRegAddr64h;
        [Description("R,DevAddr0DhRegAddr65h")]
        public string DevAddr0DhRegAddr65h;
        [Description("R,DevAddr0DhRegAddr66h")]
        public string DevAddr0DhRegAddr66h;
        [Description("R,DevAddr0DhRegAddr67h")]
        public string DevAddr0DhRegAddr67h;
        [Description("R,DevAddr0DhRegAddr70h")]
        public string DevAddr0DhRegAddr70h;
        [Description("R,DevAddr0DhRegAddr71h")]
        public string DevAddr0DhRegAddr71h;
        [Description("R,DevAddr0DhRegAddr72h")]
        public string DevAddr0DhRegAddr72h;
        [Description("R,DevAddr0DhRegAddr73h")]
        public string DevAddr0DhRegAddr73h;
        [Description("R,DevAddr0DhRegAddr74h")]
        public string DevAddr0DhRegAddr74h;
        [Description("R,DevAddr0DhRegAddr75h")]
        public string DevAddr0DhRegAddr75h;
        [Description("R,DevAddr0DhRegAddr76h")]
        public string DevAddr0DhRegAddr76h;
        [Description("R,DevAddr0DhRegAddr77h")]
        public string DevAddr0DhRegAddr77h;
        [Description("R,DevAddr0DhRegAddr78h")]
        public string DevAddr0DhRegAddr78h;
        [Description("R,DevAddr0DhRegAddr79h")]
        public string DevAddr0DhRegAddr79h;
        [Description("R,DevAddr0DhRegAddr7Ah")]
        public string DevAddr0DhRegAddr7Ah;
        [Description("R,DevAddr0DhRegAddr7Bh")]
        public string DevAddr0DhRegAddr7Bh;
        [Description("R,DevAddr0DhRegAddr7Ch")]
        public string DevAddr0DhRegAddr7Ch;
        [Description("R,DevAddr0DhRegAddr7Dh")]
        public string DevAddr0DhRegAddr7Dh;
        [Description("R,DevAddr0DhRegAddr7Eh")]
        public string DevAddr0DhRegAddr7Eh;
        [Description("R,DevAddr0DhRegAddr7Fh")]
        public string DevAddr0DhRegAddr7Fh;
        [Description("R,DevAddr0DhRegAddr80h")]
        public string DevAddr0DhRegAddr80h;
        [Description("R,DevAddr0DhRegAddr81h")]
        public string DevAddr0DhRegAddr81h;
        [Description("R,DevAddr0DhRegAddr82h")]
        public string DevAddr0DhRegAddr82h;
        [Description("R,DevAddr0DhRegAddr83h")]
        public string DevAddr0DhRegAddr83h;
        [Description("R,DevAddr0DhRegAddr84h")]
        public string DevAddr0DhRegAddr84h;
        [Description("R,DevAddr0DhRegAddr85h")]
        public string DevAddr0DhRegAddr85h;
        [Description("R,DevAddr0DhRegAddr86h")]
        public string DevAddr0DhRegAddr86h;
        [Description("R,DevAddr0DhRegAddr87h")]
        public string DevAddr0DhRegAddr87h;

        [Description("R,DevAddr0EhRegAddr00h")]
        public string DevAddr0EhRegAddr00h;
        [Description("R,DevAddr0EhRegAddr01h")]
        public string DevAddr0EhRegAddr01h;
        [Description("R,DevAddr0EhRegAddr02h")]
        public string DevAddr0EhRegAddr02h;
        [Description("R,DevAddr0EhRegAddr03h")]
        public string DevAddr0EhRegAddr03h;
        [Description("R,DevAddr0EhRegAddr04h")]
        public string DevAddr0EhRegAddr04h;
        [Description("R,DevAddr0EhRegAddr05h")]
        public string DevAddr0EhRegAddr05h;
        [Description("R,DevAddr0EhRegAddr06h")]
        public string DevAddr0EhRegAddr06h;
        [Description("R,DevAddr0EhRegAddr07h")]
        public string DevAddr0EhRegAddr07h;
        [Description("R,DevAddr0EhRegAddr08h")]
        public string DevAddr0EhRegAddr08h;
        [Description("R,DevAddr0EhRegAddr09h")]
        public string DevAddr0EhRegAddr09h;
        [Description("R,DevAddr0EhRegAddr0Ah")]
        public string DevAddr0EhRegAddr0Ah;
        [Description("R,DevAddr0EhRegAddr0Bh")]
        public string DevAddr0EhRegAddr0Bh;
        [Description("R,DevAddr0EhRegAddr0Ch")]
        public string DevAddr0EhRegAddr0Ch;
        [Description("R,DevAddr0EhRegAddr0Dh")]
        public string DevAddr0EhRegAddr0Dh;
        [Description("R,DevAddr0EhRegAddr0Eh")]
        public string DevAddr0EhRegAddr0Eh;
        [Description("R,DevAddr0EhRegAddr0Fh")]
        public string DevAddr0EhRegAddr0Fh;
        [Description("R,DevAddr0EhRegAddr10h")]
        public string DevAddr0EhRegAddr10h;
        [Description("R,DevAddr0EhRegAddr11h")]
        public string DevAddr0EhRegAddr11h;
        [Description("R,DevAddr0EhRegAddr12h")]
        public string DevAddr0EhRegAddr12h;
        [Description("R,DevAddr0EhRegAddr13h")]
        public string DevAddr0EhRegAddr13h;
        [Description("R,DevAddr0EhRegAddr14h")]
        public string DevAddr0EhRegAddr14h;
        [Description("R,DevAddr0EhRegAddr15h")]
        public string DevAddr0EhRegAddr15h;
        [Description("R,DevAddr0EhRegAddr16h")]
        public string DevAddr0EhRegAddr16h;
        [Description("R,DevAddr0EhRegAddr17h")]
        public string DevAddr0EhRegAddr17h;
        [Description("R,DevAddr0EhRegAddr20h")]
        public string DevAddr0EhRegAddr20h;
        [Description("R,DevAddr0EhRegAddr21h")]
        public string DevAddr0EhRegAddr21h;
        [Description("R,DevAddr0EhRegAddr22h")]
        public string DevAddr0EhRegAddr22h;
        [Description("R,DevAddr0EhRegAddr23h")]
        public string DevAddr0EhRegAddr23h;
        [Description("R,DevAddr0EhRegAddr24h")]
        public string DevAddr0EhRegAddr24h;
        [Description("R,DevAddr0EhRegAddr25h")]
        public string DevAddr0EhRegAddr25h;
        [Description("R,DevAddr0EhRegAddr26h")]
        public string DevAddr0EhRegAddr26h;
        [Description("R,DevAddr0EhRegAddr27h")]
        public string DevAddr0EhRegAddr27h;
        [Description("R,DevAddr0EhRegAddr28h")]
        public string DevAddr0EhRegAddr28h;
        [Description("R,DevAddr0EhRegAddr29h")]
        public string DevAddr0EhRegAddr29h;
        [Description("R,DevAddr0EhRegAddr2Ah")]
        public string DevAddr0EhRegAddr2Ah;
        [Description("R,DevAddr0EhRegAddr2Bh")]
        public string DevAddr0EhRegAddr2Bh;
        [Description("R,DevAddr0EhRegAddr2Ch")]
        public string DevAddr0EhRegAddr2Ch;
        [Description("R,DevAddr0EhRegAddr2Dh")]
        public string DevAddr0EhRegAddr2Dh;
        [Description("R,DevAddr0EhRegAddr2Eh")]
        public string DevAddr0EhRegAddr2Eh;
        [Description("R,DevAddr0EhRegAddr2Fh")]
        public string DevAddr0EhRegAddr2Fh;
        [Description("R,DevAddr0EhRegAddr30h")]
        public string DevAddr0EhRegAddr30h;
        [Description("R,DevAddr0EhRegAddr31h")]
        public string DevAddr0EhRegAddr31h;
        [Description("R,DevAddr0EhRegAddr32h")]
        public string DevAddr0EhRegAddr32h;
        [Description("R,DevAddr0EhRegAddr33h")]
        public string DevAddr0EhRegAddr33h;
        [Description("R,DevAddr0EhRegAddr34h")]
        public string DevAddr0EhRegAddr34h;
        [Description("R,DevAddr0EhRegAddr35h")]
        public string DevAddr0EhRegAddr35h;
        [Description("R,DevAddr0EhRegAddr36h")]
        public string DevAddr0EhRegAddr36h;
        [Description("R,DevAddr0EhRegAddr37h")]
        public string DevAddr0EhRegAddr37h;
        [Description("R,DevAddr0EhRegAddr40h")]
        public string DevAddr0EhRegAddr40h;
        [Description("R,DevAddr0EhRegAddr41h")]
        public string DevAddr0EhRegAddr41h;
        [Description("R,DevAddr0EhRegAddr42h")]
        public string DevAddr0EhRegAddr42h;
        [Description("R,DevAddr0EhRegAddr43h")]
        public string DevAddr0EhRegAddr43h;
        [Description("R,DevAddr0EhRegAddr44h")]
        public string DevAddr0EhRegAddr44h;
        [Description("R,DevAddr0EhRegAddr50h")]
        public string DevAddr0EhRegAddr50h;
        [Description("R,DevAddr0EhRegAddr51h")]
        public string DevAddr0EhRegAddr51h;
        [Description("R,DevAddr0EhRegAddr52h")]
        public string DevAddr0EhRegAddr52h;
        [Description("R,DevAddr0EhRegAddr53h")]
        public string DevAddr0EhRegAddr53h;
        [Description("R,DevAddr0EhRegAddr54h")]
        public string DevAddr0EhRegAddr54h;
        [Description("R,DevAddr0EhRegAddr55h")]
        public string DevAddr0EhRegAddr55h;
        [Description("R,DevAddr0EhRegAddr56h")]
        public string DevAddr0EhRegAddr56h;
        [Description("R,DevAddr0EhRegAddr57h")]
        public string DevAddr0EhRegAddr57h;
        [Description("R,DevAddr0EhRegAddr58h")]
        public string DevAddr0EhRegAddr58h;
        [Description("R,DevAddr0EhRegAddr59h")]
        public string DevAddr0EhRegAddr59h;
        [Description("R,DevAddr0EhRegAddr5Ah")]
        public string DevAddr0EhRegAddr5Ah;
        [Description("R,DevAddr0EhRegAddr5Bh")]
        public string DevAddr0EhRegAddr5Bh;
        [Description("R,DevAddr0EhRegAddr5Ch")]
        public string DevAddr0EhRegAddr5Ch;
        [Description("R,DevAddr0EhRegAddr5Dh")]
        public string DevAddr0EhRegAddr5Dh;
        [Description("R,DevAddr0EhRegAddr5Eh")]
        public string DevAddr0EhRegAddr5Eh;
        [Description("R,DevAddr0EhRegAddr5Fh")]
        public string DevAddr0EhRegAddr5Fh;
        [Description("R,DevAddr0EhRegAddr60h")]
        public string DevAddr0EhRegAddr60h;
        [Description("R,DevAddr0EhRegAddr61h")]
        public string DevAddr0EhRegAddr61h;
        [Description("R,DevAddr0EhRegAddr62h")]
        public string DevAddr0EhRegAddr62h;
        [Description("R,DevAddr0EhRegAddr63h")]
        public string DevAddr0EhRegAddr63h;
        [Description("R,DevAddr0EhRegAddr64h")]
        public string DevAddr0EhRegAddr64h;
        [Description("R,DevAddr0EhRegAddr65h")]
        public string DevAddr0EhRegAddr65h;
        [Description("R,DevAddr0EhRegAddr66h")]
        public string DevAddr0EhRegAddr66h;
        [Description("R,DevAddr0EhRegAddr67h")]
        public string DevAddr0EhRegAddr67h;
        [Description("R,DevAddr0EhRegAddr70h")]
        public string DevAddr0EhRegAddr70h;
        [Description("R,DevAddr0EhRegAddr71h")]
        public string DevAddr0EhRegAddr71h;
        [Description("R,DevAddr0EhRegAddr72h")]
        public string DevAddr0EhRegAddr72h;
        [Description("R,DevAddr0EhRegAddr73h")]
        public string DevAddr0EhRegAddr73h;
        [Description("R,DevAddr0EhRegAddr74h")]
        public string DevAddr0EhRegAddr74h;
        [Description("R,DevAddr0EhRegAddr75h")]
        public string DevAddr0EhRegAddr75h;
        [Description("R,DevAddr0EhRegAddr76h")]
        public string DevAddr0EhRegAddr76h;
        [Description("R,DevAddr0EhRegAddr77h")]
        public string DevAddr0EhRegAddr77h;
        [Description("R,DevAddr0EhRegAddr78h")]
        public string DevAddr0EhRegAddr78h;
        [Description("R,DevAddr0EhRegAddr79h")]
        public string DevAddr0EhRegAddr79h;
        [Description("R,DevAddr0EhRegAddr7Ah")]
        public string DevAddr0EhRegAddr7Ah;
        [Description("R,DevAddr0EhRegAddr7Bh")]
        public string DevAddr0EhRegAddr7Bh;
        [Description("R,DevAddr0EhRegAddr7Ch")]
        public string DevAddr0EhRegAddr7Ch;
        [Description("R,DevAddr0EhRegAddr7Dh")]
        public string DevAddr0EhRegAddr7Dh;
        [Description("R,DevAddr0EhRegAddr7Eh")]
        public string DevAddr0EhRegAddr7Eh;
        [Description("R,DevAddr0EhRegAddr7Fh")]
        public string DevAddr0EhRegAddr7Fh;
        [Description("R,DevAddr0EhRegAddr80h")]
        public string DevAddr0EhRegAddr80h;
        [Description("R,DevAddr0EhRegAddr81h")]
        public string DevAddr0EhRegAddr81h;
        [Description("R,DevAddr0EhRegAddr82h")]
        public string DevAddr0EhRegAddr82h;
        [Description("R,DevAddr0EhRegAddr83h")]
        public string DevAddr0EhRegAddr83h;
        [Description("R,DevAddr0EhRegAddr84h")]
        public string DevAddr0EhRegAddr84h;
        [Description("R,DevAddr0EhRegAddr85h")]
        public string DevAddr0EhRegAddr85h;
        [Description("R,DevAddr0EhRegAddr86h")]
        public string DevAddr0EhRegAddr86h;
        [Description("R,DevAddr0EhRegAddr87h")]
        public string DevAddr0EhRegAddr87h;

        [Description("R,DevAddr0FhRegAddr00h")]
        public string DevAddr0FhRegAddr00h;
        [Description("R,DevAddr0FhRegAddr01h")]
        public string DevAddr0FhRegAddr01h;
        [Description("R,DevAddr0FhRegAddr02h")]
        public string DevAddr0FhRegAddr02h;
        [Description("R,DevAddr0FhRegAddr03h")]
        public string DevAddr0FhRegAddr03h;
        [Description("R,DevAddr0FhRegAddr04h")]
        public string DevAddr0FhRegAddr04h;
        [Description("R,DevAddr0FhRegAddr05h")]
        public string DevAddr0FhRegAddr05h;
        [Description("R,DevAddr0FhRegAddr06h")]
        public string DevAddr0FhRegAddr06h;
        [Description("R,DevAddr0FhRegAddr07h")]
        public string DevAddr0FhRegAddr07h;
        [Description("R,DevAddr0FhRegAddr08h")]
        public string DevAddr0FhRegAddr08h;
        [Description("R,DevAddr0FhRegAddr09h")]
        public string DevAddr0FhRegAddr09h;
        [Description("R,DevAddr0FhRegAddr0Ah")]
        public string DevAddr0FhRegAddr0Ah;
        [Description("R,DevAddr0FhRegAddr0Bh")]
        public string DevAddr0FhRegAddr0Bh;
        [Description("R,DevAddr0FhRegAddr0Ch")]
        public string DevAddr0FhRegAddr0Ch;
        [Description("R,DevAddr0FhRegAddr0Dh")]
        public string DevAddr0FhRegAddr0Dh;
        [Description("R,DevAddr0FhRegAddr0Eh")]
        public string DevAddr0FhRegAddr0Eh;
        [Description("R,DevAddr0FhRegAddr0Fh")]
        public string DevAddr0FhRegAddr0Fh;
        [Description("R,DevAddr0FhRegAddr10h")]
        public string DevAddr0FhRegAddr10h;
        [Description("R,DevAddr0FhRegAddr11h")]
        public string DevAddr0FhRegAddr11h;
        [Description("R,DevAddr0FhRegAddr12h")]
        public string DevAddr0FhRegAddr12h;
        [Description("R,DevAddr0FhRegAddr13h")]
        public string DevAddr0FhRegAddr13h;
        [Description("R,DevAddr0FhRegAddr14h")]
        public string DevAddr0FhRegAddr14h;
        [Description("R,DevAddr0FhRegAddr15h")]
        public string DevAddr0FhRegAddr15h;
        [Description("R,DevAddr0FhRegAddr16h")]
        public string DevAddr0FhRegAddr16h;
        [Description("R,DevAddr0FhRegAddr17h")]
        public string DevAddr0FhRegAddr17h;
        [Description("R,DevAddr0FhRegAddr20h")]
        public string DevAddr0FhRegAddr20h;
        [Description("R,DevAddr0FhRegAddr21h")]
        public string DevAddr0FhRegAddr21h;
        [Description("R,DevAddr0FhRegAddr22h")]
        public string DevAddr0FhRegAddr22h;
        [Description("R,DevAddr0FhRegAddr23h")]
        public string DevAddr0FhRegAddr23h;
        [Description("R,DevAddr0FhRegAddr24h")]
        public string DevAddr0FhRegAddr24h;
        [Description("R,DevAddr0FhRegAddr25h")]
        public string DevAddr0FhRegAddr25h;
        [Description("R,DevAddr0FhRegAddr26h")]
        public string DevAddr0FhRegAddr26h;
        [Description("R,DevAddr0FhRegAddr27h")]
        public string DevAddr0FhRegAddr27h;
        [Description("R,DevAddr0FhRegAddr28h")]
        public string DevAddr0FhRegAddr28h;
        [Description("R,DevAddr0FhRegAddr29h")]
        public string DevAddr0FhRegAddr29h;
        [Description("R,DevAddr0FhRegAddr2Ah")]
        public string DevAddr0FhRegAddr2Ah;
        [Description("R,DevAddr0FhRegAddr2Bh")]
        public string DevAddr0FhRegAddr2Bh;
        [Description("R,DevAddr0FhRegAddr2Ch")]
        public string DevAddr0FhRegAddr2Ch;
        [Description("R,DevAddr0FhRegAddr2Dh")]
        public string DevAddr0FhRegAddr2Dh;
        [Description("R,DevAddr0FhRegAddr2Eh")]
        public string DevAddr0FhRegAddr2Eh;
        [Description("R,DevAddr0FhRegAddr2Fh")]
        public string DevAddr0FhRegAddr2Fh;
        [Description("R,DevAddr0FhRegAddr30h")]
        public string DevAddr0FhRegAddr30h;
        [Description("R,DevAddr0FhRegAddr31h")]
        public string DevAddr0FhRegAddr31h;
        [Description("R,DevAddr0FhRegAddr32h")]
        public string DevAddr0FhRegAddr32h;
        [Description("R,DevAddr0FhRegAddr33h")]
        public string DevAddr0FhRegAddr33h;
        [Description("R,DevAddr0FhRegAddr34h")]
        public string DevAddr0FhRegAddr34h;
        [Description("R,DevAddr0FhRegAddr35h")]
        public string DevAddr0FhRegAddr35h;
        [Description("R,DevAddr0FhRegAddr36h")]
        public string DevAddr0FhRegAddr36h;
        [Description("R,DevAddr0FhRegAddr37h")]
        public string DevAddr0FhRegAddr37h;
        [Description("R,DevAddr0FhRegAddr40h")]
        public string DevAddr0FhRegAddr40h;
        [Description("R,DevAddr0FhRegAddr41h")]
        public string DevAddr0FhRegAddr41h;
        [Description("R,DevAddr0FhRegAddr42h")]
        public string DevAddr0FhRegAddr42h;
        [Description("R,DevAddr0FhRegAddr43h")]
        public string DevAddr0FhRegAddr43h;
        [Description("R,DevAddr0FhRegAddr44h")]
        public string DevAddr0FhRegAddr44h;
        [Description("R,DevAddr0FhRegAddr50h")]
        public string DevAddr0FhRegAddr50h;
        [Description("R,DevAddr0FhRegAddr51h")]
        public string DevAddr0FhRegAddr51h;
        [Description("R,DevAddr0FhRegAddr52h")]
        public string DevAddr0FhRegAddr52h;
        [Description("R,DevAddr0FhRegAddr53h")]
        public string DevAddr0FhRegAddr53h;
        [Description("R,DevAddr0FhRegAddr54h")]
        public string DevAddr0FhRegAddr54h;
        [Description("R,DevAddr0FhRegAddr55h")]
        public string DevAddr0FhRegAddr55h;
        [Description("R,DevAddr0FhRegAddr56h")]
        public string DevAddr0FhRegAddr56h;
        [Description("R,DevAddr0FhRegAddr57h")]
        public string DevAddr0FhRegAddr57h;
        [Description("R,DevAddr0FhRegAddr58h")]
        public string DevAddr0FhRegAddr58h;
        [Description("R,DevAddr0FhRegAddr59h")]
        public string DevAddr0FhRegAddr59h;
        [Description("R,DevAddr0FhRegAddr5Ah")]
        public string DevAddr0FhRegAddr5Ah;
        [Description("R,DevAddr0FhRegAddr5Bh")]
        public string DevAddr0FhRegAddr5Bh;
        [Description("R,DevAddr0FhRegAddr5Ch")]
        public string DevAddr0FhRegAddr5Ch;
        [Description("R,DevAddr0FhRegAddr5Dh")]
        public string DevAddr0FhRegAddr5Dh;
        [Description("R,DevAddr0FhRegAddr5Eh")]
        public string DevAddr0FhRegAddr5Eh;
        [Description("R,DevAddr0FhRegAddr5Fh")]
        public string DevAddr0FhRegAddr5Fh;
        [Description("R,DevAddr0FhRegAddr60h")]
        public string DevAddr0FhRegAddr60h;
        [Description("R,DevAddr0FhRegAddr61h")]
        public string DevAddr0FhRegAddr61h;
        [Description("R,DevAddr0FhRegAddr62h")]
        public string DevAddr0FhRegAddr62h;
        [Description("R,DevAddr0FhRegAddr63h")]
        public string DevAddr0FhRegAddr63h;
        [Description("R,DevAddr0FhRegAddr64h")]
        public string DevAddr0FhRegAddr64h;
        [Description("R,DevAddr0FhRegAddr65h")]
        public string DevAddr0FhRegAddr65h;
        [Description("R,DevAddr0FhRegAddr66h")]
        public string DevAddr0FhRegAddr66h;
        [Description("R,DevAddr0FhRegAddr67h")]
        public string DevAddr0FhRegAddr67h;
        [Description("R,DevAddr0FhRegAddr70h")]
        public string DevAddr0FhRegAddr70h;
        [Description("R,DevAddr0FhRegAddr71h")]
        public string DevAddr0FhRegAddr71h;
        [Description("R,DevAddr0FhRegAddr72h")]
        public string DevAddr0FhRegAddr72h;
        [Description("R,DevAddr0FhRegAddr73h")]
        public string DevAddr0FhRegAddr73h;
        [Description("R,DevAddr0FhRegAddr74h")]
        public string DevAddr0FhRegAddr74h;
        [Description("R,DevAddr0FhRegAddr75h")]
        public string DevAddr0FhRegAddr75h;
        [Description("R,DevAddr0FhRegAddr76h")]
        public string DevAddr0FhRegAddr76h;
        [Description("R,DevAddr0FhRegAddr77h")]
        public string DevAddr0FhRegAddr77h;
        [Description("R,DevAddr0FhRegAddr78h")]
        public string DevAddr0FhRegAddr78h;
        [Description("R,DevAddr0FhRegAddr79h")]
        public string DevAddr0FhRegAddr79h;
        [Description("R,DevAddr0FhRegAddr7Ah")]
        public string DevAddr0FhRegAddr7Ah;
        [Description("R,DevAddr0FhRegAddr7Bh")]
        public string DevAddr0FhRegAddr7Bh;
        [Description("R,DevAddr0FhRegAddr7Ch")]
        public string DevAddr0FhRegAddr7Ch;
        [Description("R,DevAddr0FhRegAddr7Dh")]
        public string DevAddr0FhRegAddr7Dh;
        [Description("R,DevAddr0FhRegAddr7Eh")]
        public string DevAddr0FhRegAddr7Eh;
        [Description("R,DevAddr0FhRegAddr7Fh")]
        public string DevAddr0FhRegAddr7Fh;
        [Description("R,DevAddr0FhRegAddr80h")]
        public string DevAddr0FhRegAddr80h;
        [Description("R,DevAddr0FhRegAddr81h")]
        public string DevAddr0FhRegAddr81h;
        [Description("R,DevAddr0FhRegAddr82h")]
        public string DevAddr0FhRegAddr82h;
        [Description("R,DevAddr0FhRegAddr83h")]
        public string DevAddr0FhRegAddr83h;
        [Description("R,DevAddr0FhRegAddr84h")]
        public string DevAddr0FhRegAddr84h;
        [Description("R,DevAddr0FhRegAddr85h")]
        public string DevAddr0FhRegAddr85h;
        [Description("R,DevAddr0FhRegAddr86h")]
        public string DevAddr0FhRegAddr86h;
        [Description("R,DevAddr0FhRegAddr87h")]
        public string DevAddr0FhRegAddr87h;

        #endregion

        #region led点亮

        //[Description("循环点亮")]
        //public void LedCycleOpen()
        //{
        //    var startAddr = 1;
        //    var endAddr = 3;

        //    for (int r = 0; r < 1; r++)
        //    {
        //        // PWM Duty-cycle Sharing for All Enabled Output
        //        for (var i = startAddr; i <= endAddr; i++)
        //        {
        //            TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + i), 0x44, new byte[] { 0x0F }));
        //            //TpsSendCmd(GetSendData((byte)(BroadcastWrite8BytesDevAddrByte + i), 0x00, new byte[24] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }));
        //            //TpsSendCmd(GetSendData((byte)(BroadcastWrite8BytesDevAddrByte + i), 0x20, new byte[24] { 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F }));
        //            //TpsSendCmd(GetSendData((byte)(BroadcastWrite8BytesDevAddrByte + i), 0x50, new byte[24] { 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F }));

        //            TpsSendCmd(GetSendData((byte)(BroadcastWrite24BytesDevAddrByte + i), 0x00, new byte[24] { 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F }));
        //            TpsSendCmd(GetSendData((byte)(BroadcastWrite24BytesDevAddrByte + i), 0x20, new byte[24] { 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F }));
        //            TpsSendCmd(GetSendData((byte)(BroadcastWrite24BytesDevAddrByte + i), 0x50, new byte[24] { 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F }));
        //        }

        //        for (var i = startAddr; i <= endAddr; i++)
        //        {
        //            TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + i), 0x91, new byte[] { 0x07 }));
        //            Thread.Sleep(5);
        //            for (var j = 0x40; j <= 0x43; j++)
        //            {
        //                TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + i), (byte)j, new byte[] { 0x00 }));
        //                Thread.Sleep(5);
        //            }
        //        }
        //        Thread.Sleep(50);

        //        for (var i = startAddr; i <= endAddr; i++)
        //        {
        //            var listBits = new Dictionary<byte, List<string>>();

        //            for (var j = 0x40; j <= 0x43; j++)
        //                listBits[(byte)j] = new List<string> { "0", "0", "0", "0", "0", "0", "0", "0" };

        //            for (var j = 0x40; j <= 0x43; j++)
        //            {
        //                for (var k = 0; k <= 2; k++)
        //                {
        //                    listBits[(byte)j][k] = "1";
        //                    TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + i), (byte)j, new byte[] { BitsStringToByte(listBits[(byte)j]) }));
        //                    Thread.Sleep(5);
        //                    Thread.Sleep(100);
        //                }

        //                for (var k = 4; k <= 6; k++)
        //                {
        //                    listBits[(byte)j][k] = "1";
        //                    TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + i), (byte)j, new byte[] { BitsStringToByte(listBits[(byte)j]) }));
        //                    Thread.Sleep(5);
        //                    Thread.Sleep(100);
        //                }
        //            }
        //        }
        //    }
        //}

        //[Description("LED全亮")]
        //public void LedAllOpen(int dev)
        //{
        //    // PWM Duty-cycle Sharing for All Enabled Output
        //    TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + dev), 0x44, new byte[] { 0x0F }));
        //    TpsSendCmd(GetSendData((byte)(BroadcastWrite24BytesDevAddrByte + dev), 0x00, new byte[24] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }));
        //    TpsSendCmd(GetSendData((byte)(BroadcastWrite24BytesDevAddrByte + dev), 0x20, new byte[24] { 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F }));
        //    TpsSendCmd(GetSendData((byte)(BroadcastWrite24BytesDevAddrByte + dev), 0x50, new byte[24] { 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F }));

        //    TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + dev), 0x91, new byte[] { 0x07 }));
        //    Thread.Sleep(5);
        //    for (var j = 0x40; j <= 0x43; j++)
        //    {
        //        TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + dev), (byte)j, new byte[] { 0x77 }));
        //        Thread.Sleep(5);
        //    }
        //}

        //[Description("LED全关")]
        //public void LedAllClose(int dev)
        //{
        //    // PWM Duty-cycle Sharing for All Enabled Output
        //    TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + dev), 0x44, new byte[] { 0x0F }));
        //    TpsSendCmd(GetSendData((byte)(BroadcastWrite24BytesDevAddrByte + dev), 0x00, new byte[24] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }));
        //    TpsSendCmd(GetSendData((byte)(BroadcastWrite24BytesDevAddrByte + dev), 0x20, new byte[24] { 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F, 0x0F }));
        //    TpsSendCmd(GetSendData((byte)(BroadcastWrite24BytesDevAddrByte + dev), 0x50, new byte[24] { 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F }));

        //    TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + dev), 0x91, new byte[] { 0x07 }));
        //    Thread.Sleep(5);
        //    for (var j = 0x40; j <= 0x43; j++)
        //    {
        //        TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + dev), (byte)j, new byte[] { 0x00 }));
        //        Thread.Sleep(5);
        //    }
        //}

        //private byte BitsStringToByte(List<string> listBits)
        //{
        //    //return Convert.ToByte(string.Join("", listBits), 2);

        //    var str = string.Empty;
        //    for (var i = listBits.Count - 1; i >= 0; i--)
        //        str += listBits[i];

        //    return Convert.ToByte(str, 2);
        //}

        [Description("R,当前在线")]
        public string CurrentOnline = string.Empty;

        [Description("开启LED控制")]
        public void StartLedCtrl() => _isLedCtrlTimerRun = true;

        [Description("关闭LED控制")]
        public void StopLedCtrl() => _isLedCtrlTimerRun = false;

        [Description("设置dev在线")]
        public void SetOnline(int dev)
        {
            if (dev >= 0 && dev <= 15)
            {
                lock (_devLocker)
                {
                    _devs[dev].IsOnline = true;
                    CurrentOnline = string.Empty;
                    for (var i = 0; i < _devs.Length; i++)
                        if (_devs[i].IsOnline)
                            CurrentOnline += i + ",";
                    CurrentOnline = CurrentOnline.TrimEnd(',');
                }
            }
        }

        [Description("设置dev离线")]
        public void SetOffline(int dev)
        {
            if (dev >= 0 && dev <= 15)
            {
                lock (_devLocker)
                {
                    _devs[dev].IsOnline = false;
                    CurrentOnline = string.Empty;
                    for (var i = 0; i < _devs.Length; i++)
                        if (_devs[i].IsOnline)
                            CurrentOnline += i + ",";
                    CurrentOnline = CurrentOnline.TrimEnd(',');
                }
            }
        }

        [Description("设置所有pwm")]
        public void SetAllPwm(int pwm)
        {
            if (pwm >= 0 && pwm <= 4095)
            {
                lock (_devLocker)
                {
                    for (var i = 0; i < _devs.Length; i++)
                    {
                        for (int j = 0; j < _devs[i].Pwm.Length; j++)
                        {
                            _devs[i].SetPwm(j, pwm);
                        }
                    }
                }
            }
        }

        [Description("设置pwm")]
        public void SetPwm(int dev, int ch, int pwm)
        {
            if (dev >= 0 && dev <= 15 && ch >= 0 && ch <= 23 && pwm >= 0 && pwm <= 4095)
                lock (_devLocker)
                    _devs[dev].SetPwm(ch, pwm);
        }

        [Description("打开通道")]
        public void OpenChannel(int dev, int ch)
        {
            if (dev >= 0 && dev <= 15 && ch >= 0 && ch <= 23)
                lock (_devLocker)
                    _devs[dev].SwitchOut(ch, true);
        }

        [Description("关闭通道")]
        public void CloseChannel(int dev, int ch)
        {
            if (dev >= 0 && dev <= 15 && ch >= 0 && ch <= 23)
                lock (_devLocker)
                    _devs[dev].SwitchOut(ch, false);
        }

        [Description("打开所有通道")]
        public void OpenAllChannels(int dev)
        {
            if (dev >= 0 && dev <= 15)
            {
                lock (_devLocker)
                {
                    for (var i = 0; i < _devs[dev].OutEn.Length; i++)
                    {
                        _devs[dev].SwitchOut(i, true);
                    }
                }
            }
        }

        [Description("关闭所有通道")]
        public void CloseAllChannels(int dev)
        {
            if (dev >= 0 && dev <= 15)
            {
                lock (_devLocker)
                {
                    for (var i = 0; i < _devs[dev].OutEn.Length; i++)
                    {
                        _devs[dev].SwitchOut(i, false);
                    }
                }
            }
        }

        [Description("打开偶数通道")]
        public void OpenEvenChannels(int dev)
        {
            if (dev >= 0 && dev <= 15)
            {
                lock (_devLocker)
                {
                    for (var i = 0; i < _devs[dev].OutEn.Length; i++)
                    {
                        if (i % 2 == 0)
                        {
                            _devs[dev].SwitchOut(i, true);
                        }
                    }
                }
            }
        }

        [Description("关闭偶数通道")]
        public void CloseEvenChannels(int dev)
        {
            if (dev >= 0 && dev <= 15)
            {
                lock (_devLocker)
                {
                    for (var i = 0; i < _devs[dev].OutEn.Length; i++)
                    {
                        if (i % 2 == 0)
                        {
                            _devs[dev].SwitchOut(i, false);
                        }
                    }
                }
            }
        }

        [Description("打开奇数通道")]
        public void OpenOddChannels(int dev)
        {
            if (dev >= 0 && dev <= 15)
            {
                lock (_devLocker)
                {
                    for (var i = 0; i < _devs[dev].OutEn.Length; i++)
                    {
                        if (i % 2 != 0)
                        {
                            _devs[dev].SwitchOut(i, true);
                        }
                    }
                }
            }
        }

        [Description("关闭奇数通道")]
        public void CloseOddChannels(int dev)
        {
            if (dev >= 0 && dev <= 15)
            {
                lock (_devLocker)
                {
                    for (var i = 0; i < _devs[dev].OutEn.Length; i++)
                    {
                        if (i % 2 != 0)
                        {
                            _devs[dev].SwitchOut(i, false);
                        }
                    }
                }
            }
        }

        private bool _isLedCtrlTimerRun;
        private readonly object _devLocker = new object();
        private readonly object _msgLocker = new object();
        private Dev[] _devs = new Dev[16];

        private void LedCtrlTimer()
        {
            for (var i = 0; i < 16; i++)
            {
                _devs[i] = new Dev(i);
                DevCtrl(i);
            }

            SchedulerAsync();
        }

        private void DevCtrl(int dev)
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = SendMsg(() =>
                {
                    lock (_devLocker)
                    {
                        if (_devs[dev].IsOnline)
                        {
                            var pwmHBytes = new List<byte>();
                            var pwmLBytes = new List<byte>();
                            for (var i = 0; i < _devs[dev].Pwm.Length; i++)
                            {
                                var bits = Convert.ToString(_devs[dev].Pwm[i], 2).PadLeft(12, '0').Select(f => f.ToString()).ToArray();
                                pwmHBytes.Add(Convert.ToByte(string.Join("", bits.Take(8)), 2));
                                pwmLBytes.Add(Convert.ToByte(string.Join("", bits.Skip(8)), 2));
                            }

                            var iOutByte = new List<byte>();
                            for (var i = 0; i < _devs[dev].IOut.Length; i++)
                            {
                                var bits = Convert.ToString(_devs[dev].IOut[i], 2).PadLeft(8, '0').Select(f => f.ToString()).ToArray();
                                iOutByte.Add(Convert.ToByte(string.Join("", bits.Take(8)), 2));
                            }

                            var outEnByte = new List<byte>();
                            for (var i = 0; i < _devs[dev].OutEn.Length; i = i + 6)
                            {
                                var bits = new string[8] { "0", _devs[dev].OutEn[i + 5] ? "1" : "0", _devs[dev].OutEn[i + 4] ? "1" : "0", _devs[dev].OutEn[i + 3] ? "1" : "0", "0", _devs[dev].OutEn[i + 2] ? "1" : "0", _devs[dev].OutEn[i + 1] ? "1" : "0", _devs[dev].OutEn[i + 0] ? "1" : "0" };
                                outEnByte.Add(Convert.ToByte(string.Join("", bits.Take(8)), 2));
                            }

                            return new List<byte[]>
                            {
                                GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + dev), 0x91, new byte[] { 0x07 }),
                                GetSendData((byte)(BroadcastWrite24BytesDevAddrByte + dev), 0x00, pwmHBytes),
                                GetSendData((byte)(BroadcastWrite24BytesDevAddrByte + dev), 0x20, pwmLBytes),
                                GetSendData((byte)(BroadcastWrite24BytesDevAddrByte + dev), 0x50, iOutByte),
                                GetSendData((byte)(BroadcastWrite4BytesDevAddrByte + dev), 0x40, outEnByte),
                            };
                        }
                        else
                        {
                            return null;
                        }
                    }
                }),
                Interval = 20
            });
        }

        private Action SendMsg(Func<List<byte[]>> date)
        {
            return () =>
            {
                if (MySerialPort is null)
                    return;

                if (date is null)
                    return;

                if (!_isLedCtrlTimerRun)
                    return;

                var msg = date.Invoke();
                if (msg is null)
                    return;

                if (!msg.Any())
                    return;

                lock (_msgLocker)
                {
                    for (var i = 0; i < msg.Count; i++)
                    {
                        var b = msg[i];
                        if (b is null)
                            continue;
                        MySerialPort.SendCommand(b, 1);
                    }
                }
            };
        }

        internal class Dev
        {
            public int DevId { get; private set; }
            public int[] Pwm { get; private set; }
            public int[] IOut { get; private set; }
            public bool[] OutEn { get; private set; }
            public bool IsOnline { get; set; }

            public Dev(int devId)
            {
                DevId = devId;
                const int ChCount = 24;
                Pwm = new int[ChCount];
                IOut = new int[ChCount];
                for (var i = 0; i < ChCount; i++)
                {
                    IOut[i] = 0x3F;
                    Pwm[i] = 0xFFF;
                }
                OutEn = new bool[24];
            }

            public void SetPwm(int ch, int pwm)
            {
                if (ch >= 0 && ch <= 23 && pwm >= 0 && pwm <= 4095)
                    Pwm[ch] = pwm;
            }

            public void SetIOut(int ch, int iOut)
            {
                if (ch >= 0 && ch <= 23 && iOut >= 0 && iOut <= 63)
                    IOut[ch] = iOut;
            }

            public void SwitchOut(int ch, bool isOn)
            {
                if (ch >= 0 && ch <= 23)
                    OutEn[ch] = isOn;
            }
        }

        #endregion
    }
}
