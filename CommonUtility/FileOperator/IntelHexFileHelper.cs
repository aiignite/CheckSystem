using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CommonUtility.FileOperator
{
    public static class IntelHexFileHelper
    {
        public static IEnumerable<HexLine> ReadHexFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) ||
                string.IsNullOrEmpty(fileName.Trim()))
                return null;

            var returnList = new List<HexLine>();
            using (var fs = new FileStream(fileName, FileMode.Open))
            {
                var hexReader = new StreamReader(fs);

                while (true)
                {
                    var szLine = hexReader.ReadLine();
                    if (szLine == null) { break; } // 读取结束 退出          
                    if (szLine.Substring(0, 1) != ":")
                        continue;
                    if (szLine.Substring(1, 8) == "00000001") { break; } // 文件结束标识
                    // 直接解析数据类型标识为 : 00 和 01 的格式
                    //if ((szLine.Substring(8, 1) != "0") && (szLine.Substring(8, 1) != "1"))
                    //    continue;
                    szLine = szLine.Substring(1, szLine.Length - 1);
                    var bList = new List<byte>();
                    for (var i = 0; i < szLine.Length; i = i + 2)
                    {
                        var t1 = szLine[i].ToString();
                        var t2 = szLine[i + 1].ToString();
                        var tt = t1 + t2;
                        var b = Convert.ToByte(tt, 16);
                        bList.Add(b);
                    }

                    var hexLine = new HexLine(bList);
                    returnList.Add(hexLine);
                }

                hexReader.Close();
                fs.Close();
            }

            return returnList;
        }

        public static IEnumerable<Block> GetBlocks(HexLine[] hexLines)
        {
            if (hexLines == null)
                return null;

            hexLines =
                hexLines.Where(
                    w =>
                        w.DataType.Equals(HexDataType.DataRecord) ||
                        w.DataType.Equals(HexDataType.ExtendedLinearAddressRecord)).ToArray();

            var lstBlocks = new List<Block>();
            //var baseAddr = 0;
            //byte[] startsAt = null;
            var tempData = new List<byte>();
            var tempI = 0;
            int[] startsAtBase = { 0 };
            //var temp = hexLines.ToList();

            for (var i = 0; i < hexLines.Length; i++)
            {
                if (i < tempI)
                    continue;

                //if (!temp.Any())
                //    break;

                var action = new Action(() =>
                {
                    var startsAt = 0;
                    var offset = 0;
                    //(startsAtBase << 16) | (hexLines[i + 1].StartAddr[0] * 256 + hexLines[i + 1].StartAddr[1]);

                    for (var j = i + 1; j < hexLines.Length; j++)
                    {
                        if (hexLines[j].DataType.Equals(HexDataType.DataRecord) &&
                            hexLines[i].DataType.Equals(HexDataType.ExtendedLinearAddressRecord) &&
                            j == i + 1) // 04后面的第一个00
                        {
                            startsAt = (startsAtBase[0] << 16) | (hexLines[j].StartAddr[0] * 256 + hexLines[j].StartAddr[1]);
                            tempData.AddRange(hexLines[j].Data);
                            offset = startsAt + tempData.Count;
                        }
                        else if (hexLines[j].DataType.Equals(HexDataType.DataRecord) &&
                            hexLines[i].DataType.Equals(HexDataType.DataRecord) &&
                            j == i + 1) // 00后面的第一个00
                        {
                            startsAt = (startsAtBase[0] << 16) | (hexLines[i].StartAddr[0] * 256 + hexLines[i].StartAddr[1]);
                            if (!tempData.Any())
                                tempData.AddRange(hexLines[i].Data);
                            tempData.AddRange(hexLines[j].Data);
                            offset = startsAt + tempData.Count;
                        }
                        else if (hexLines[j].DataType.Equals(HexDataType.DataRecord))
                        {
                            if (
                                offset.Equals((startsAtBase[0] << 16) |
                                              (hexLines[j].StartAddr[0] * 256 + hexLines[j].StartAddr[1])))
                            {
                                tempData.AddRange(hexLines[j].Data);
                                offset = startsAt + tempData.Count;

                                if (j != hexLines.Length - 1)
                                    continue;

                                var block = new Block(lstBlocks.Count, (uint) startsAt, tempData.ToArray());
                                lstBlocks.Add(block);
                                tempData.Clear();
                                tempI = j;
                                break;
                            }
                            else
                            {
                                var block = new Block(lstBlocks.Count, (uint) startsAt, tempData.ToArray());
                                lstBlocks.Add(block);
                                tempData.Clear();
                                tempData.AddRange(hexLines[j].Data);
                                tempI = j;
                                break;
                            }
                        }
                        else if (hexLines[j].DataType.Equals(HexDataType.ExtendedLinearAddressRecord))
                        {
                            //var sx = hexLines[j].Data[0] * 256 + hexLines[j].Data[1];
                            //sx = (sx << 16) | (0x00 * 256 + 0x00);
                            //hexLines[j].Data[0] * 256 + hexLines[j].Data[1]
                            //startsAtBase = (hexLines[j].Data[0] * 256 + hexLines[j].Data[1] << 16) | 0x00 * 256 + 0x00;
                            //hexLines[j].Data[0] * 256 + hexLines[j].Data[1];
                            if (offset.Equals((hexLines[j].Data[0] * 256 + hexLines[j].Data[1] << 16) | 0x00 * 256 + 0x00))
                            {
                                var nextStartsAtBase = BitConverter.GetBytes(offset);
                                startsAtBase[0] = nextStartsAtBase[3] * 256 + nextStartsAtBase[2];
                                //startsAt = 0;
                                continue;
                            }

                            startsAtBase[0] = hexLines[j].Data[0] * 256 + hexLines[j].Data[1];
                            var block = new Block(lstBlocks.Count, (uint) startsAt, tempData.ToArray());
                            lstBlocks.Add(block);
                            tempData.Clear();
                            tempI = j;
                            break;
                        }
                    }
                });

                if (hexLines[i].DataType.Equals(HexDataType.ExtendedLinearAddressRecord))
                {
                    startsAtBase[0] = hexLines[i].Data[0] * 256 + hexLines[i].Data[1];
                    action.Invoke();
                }
                else if (hexLines[i].DataType.Equals(HexDataType.DataRecord))
                    action.Invoke();
            }

            return lstBlocks;
        }
    }

    public class Block
    {
        public int BlockIndex { get; private set; }

        public byte[] StartsAt { get; private set; }

        public byte[] EndsAt { get; private set; }

        public byte[] Length { get; private set; }

        public byte[] Datas { get; private set; }

        public byte[] CheckSumCrc32 { get; private set; }

        public Block(int blockIndex, uint startsAt, byte[] datas)
        {
            BlockIndex = blockIndex;
            StartsAt = BitConverter.GetBytes(startsAt).Reverse().ToArray();
            EndsAt = BitConverter.GetBytes(startsAt + (uint) datas.Length - 1).Reverse().ToArray();
            Datas = datas;
            Length = BitConverter.GetBytes((uint) datas.Length).Reverse().ToArray();
            CheckSumCrc32 = ValueHelper.Crc32(datas).ToArray();
        }
    }

    /// <summary>
    /// Hex 本行
    /// </summary>
    public class HexLine
    {
        /// <summary>
        /// 本行数据的字符串形式（0x??）
        /// </summary>
        public string LineHexStr { get; private set; }

        /// <summary>
        /// 本行的字节总长度
        /// </summary>
        public int LineLen { get; private set; }

        /// <summary>
        /// 本行的数据长度
        /// </summary>
        public byte DataLen { get; private set; }

        /// <summary>
        /// 本行数据的起始地址
        /// </summary>
        public byte[] StartAddr { get; private set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public HexDataType DataType { get; private set; }

        /// <summary>
        /// 数据
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        /// 本行的检验和
        /// 前所有16进制数的累加和，不计进位（即：低位单字节），检验和 = 0x100 - 累加和
        /// </summary>
        public byte Verification { get; private set; }

        /// <summary>
        /// 本行进行数据校验后是否有错误
        /// </summary>
        public bool IsErrorLine { get; set; }

        /// <summary>
        /// Hex 本行
        /// </summary>
        /// <param name="lineBytes">本行字节数组</param>
        public HexLine(IReadOnlyList<byte> lineBytes)
        {
            foreach (var strHex
                in lineBytes.Select(ValueHelper.GetHextStrWithOx))
                LineHexStr += strHex + " ";
            LineHexStr = LineHexStr.Trim();

            LineLen = lineBytes.Count;

            if (lineBytes.Count < 5)
            {
                IsErrorLine = true;
                return;
            }

            DataLen = lineBytes[0];
            StartAddr = new[] { lineBytes[1], lineBytes[2] };
            var str = Enum.GetName(typeof(HexDataType), lineBytes[3]);
            if (str != null)
                DataType = (HexDataType) Enum.Parse(typeof(HexDataType), str);

            var actualDataLen = LineLen - 1 - 2 - 1 - 1;
            if (actualDataLen != DataLen)
            {
                IsErrorLine = true;
                return;
            }

            Data = new byte[DataLen];
            Array.Copy(lineBytes.ToArray(), 4, Data, 0, DataLen);
            Verification = lineBytes[LineLen - 1];

            var expectedVerificationSum = 0;
            for (var i = 0; i < lineBytes.Count - 1; i++)
                expectedVerificationSum = expectedVerificationSum + lineBytes[i];

            var sum = (byte) (0x100 - BitConverter.GetBytes((ushort) expectedVerificationSum)[0]);
            if (sum != Verification)
                IsErrorLine = true;
        }
    }

    /// <summary>
    /// 数据类型
    /// </summary>
    public enum HexDataType : byte
    {
        /// <summary>
        /// 00
        /// 数据记录
        /// Data Record
        /// </summary>
        DataRecord = 0x00,

        /// <summary>
        /// 01
        /// 文件结束记录
        /// End of File Record
        /// </summary>
        EndOfFileRecord = 0x01,

        /// <summary>
        /// 02
        /// 扩展段地址记录
        /// Extended Segment Address Record
        /// </summary>
        ExtendedSegmentAddressRecord = 0x02,

        /// <summary>
        /// 03
        /// 开始段地址记录
        /// Start Segment Address Record
        /// </summary>
        StartSegmentAddressRecord = 0x03,

        /// <summary>
        /// 04
        /// 扩展线性地址记录
        /// Extended Linear Address Record
        /// </summary>
        ExtendedLinearAddressRecord = 0x04,

        /// <summary>
        /// 05
        /// 开始线性地址记录
        /// Start Linear Address Record
        /// </summary>
        StartLinearAddressRecord = 0x05
    }
}
