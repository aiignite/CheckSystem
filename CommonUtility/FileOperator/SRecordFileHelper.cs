using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CommonUtility.FileOperator
{
    public static class SRecordFileHelper
    {
        public static IEnumerable<SRecordLineData> GetSRecordLineData(string filePath)
        {
            var temp = new List<string>();
            using (var sr = new StreamReader(filePath, Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    try
                    {
                        //if (!line.StartsWith("S1") && !line.StartsWith("S2") && !line.StartsWith("S3"))
                        //    continue;

                        var str = line.Trim(' ');
                        if (!string.IsNullOrEmpty(str))
                            temp.Add(str);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }

            return GetSRecordLineData(temp);

            //var listLineData = new List<SRecordLineData>();
            
            //foreach (var t in temp)
            //{
            //    var thisLineData = new SRecordLineData();
            //    var tempCheckSum = (byte)0;

            //    var stype = t.Substring(0, 2);

            //    // 获取Type
            //    for (var i = 0; i < (int)SRecordType.Smax; i++)
            //    {
            //        if (stype != SRecordTypeTable[i])
            //            continue;
            //        thisLineData.Type = EnumOperater.GetEnum<SRecordType>(SRecordTypeTable[i]);
            //        break;
            //    }

            //    // 获取Count
            //    thisLineData.Count = Convert.ToByte(t.Substring(2, 2), 16);
            //    tempCheckSum += thisLineData.Count;
            //    if (thisLineData.Count != t.Length / 2 - 2)
            //    {
            //        // error
            //        return new List<SRecordLineData>();
            //    }

            //    // 获取Address
            //    if (thisLineData.Type == SRecordType.S0)
            //    {
            //        thisLineData.Address = 0x00;
            //    }
            //    else if (thisLineData.Type == SRecordType.S1 || thisLineData.Type == SRecordType.S5 || thisLineData.Type == SRecordType.S9)
            //    {
            //        thisLineData.Address = Convert.ToUInt32(t.Substring(4, 4), 16);

            //        tempCheckSum += (byte)(thisLineData.Address >> 8 & 0x0FF);
            //        tempCheckSum += (byte)(thisLineData.Address & 0x0FF);

            //        var dataLen = t.Length / 2 - 1 - 1 - 1 - 2;
            //        thisLineData.Data = new byte[dataLen];
            //        for (var i = 0; i < thisLineData.Count - 3; i++)
            //        {
            //            var data = Convert.ToByte(t.Substring(2 * i + 8, 2), 16);
            //            thisLineData.Data[i] = data;
            //            tempCheckSum += data;
            //        }

            //        thisLineData.CheckSum = Convert.ToByte(t.Substring(t.Length - 2, 2), 16);
            //        tempCheckSum = (byte)(0xFF - tempCheckSum);
            //        if (thisLineData.CheckSum != tempCheckSum)
            //        {
            //            // checksum error
            //            return new List<SRecordLineData>();
            //        }
            //    }
            //    else if (thisLineData.Type == SRecordType.S2 || thisLineData.Type == SRecordType.S8)
            //    {
            //        thisLineData.Address = Convert.ToUInt32(t.Substring(4, 6), 16);

            //        tempCheckSum += (byte)(thisLineData.Address >> 16 & 0xFF);
            //        tempCheckSum += (byte)(thisLineData.Address >> 8 & 0xFF);
            //        tempCheckSum += (byte)(thisLineData.Address & 0xFF);

            //        var dataLen = t.Length / 2 - 1 - 1 - 1 - 3;
            //        thisLineData.Data = new byte[dataLen];
            //        for (var i = 0; i < thisLineData.Count - 4; i++)
            //        {
            //            var data = Convert.ToByte(t.Substring(2 * i + 10, 2), 16);
            //            thisLineData.Data[i] = data;
            //            tempCheckSum += data;
            //        }

            //        thisLineData.CheckSum = Convert.ToByte(t.Substring(t.Length - 2, 2), 16);
            //        tempCheckSum = (byte)(0xFF - tempCheckSum);
            //        if (thisLineData.CheckSum != tempCheckSum)
            //        {
            //            // checksum error
            //            return new List<SRecordLineData>();
            //        }
            //    }
            //    else if (thisLineData.Type == SRecordType.S3 || thisLineData.Type == SRecordType.S7)
            //    {
            //        thisLineData.Address = Convert.ToUInt32(t.Substring(4, 8), 16);

            //        tempCheckSum += (byte)(thisLineData.Address >> 24 & 0xFF);
            //        tempCheckSum += (byte)(thisLineData.Address >> 16 & 0xFF);
            //        tempCheckSum += (byte)(thisLineData.Address >> 8 & 0xFF);
            //        tempCheckSum += (byte)(thisLineData.Address & 0xFF);

            //        var dataLen = t.Length / 2 - 1 - 1 - 1 - 4;
            //        thisLineData.Data = new byte[dataLen];
            //        for (var i = 0; i < thisLineData.Count - 5; i++)
            //        {
            //            var data = Convert.ToByte(t.Substring(2 * i + 12, 2), 16);
            //            thisLineData.Data[i] = data;
            //            tempCheckSum += data;
            //        }

            //        thisLineData.CheckSum = Convert.ToByte(t.Substring(t.Length - 2, 2), 16);
            //        tempCheckSum = (byte)(0xFF - tempCheckSum);
            //        if (thisLineData.CheckSum != tempCheckSum)
            //        {
            //            // checksum error
            //            return new List<SRecordLineData>();
            //        }
            //    }

            //    // 获取DataLen
            //    if (thisLineData.Type == SRecordType.S1)
            //        thisLineData.DataLen = (byte)(thisLineData.Count - 0x03);
            //    else if (thisLineData.Type == SRecordType.S2)
            //        thisLineData.DataLen = (byte)(thisLineData.Count - 0x04);
            //    else if (thisLineData.Type == SRecordType.S3)
            //        thisLineData.DataLen = (byte)(thisLineData.Count - 0x05);
            //    else
            //        thisLineData.DataLen = 0x00;

            //    listLineData.Add(thisLineData);
            //}

            //return listLineData;
        }

        public static IEnumerable<SRecordLineData> GetSRecordLineData(List<string> temp)
        {
            //var temp = new List<string>();
            //using (var sr = new StreamReader(filePath, Encoding.Default))
            //{
            //    string line;
            //    while ((line = sr.ReadLine()) != null)
            //    {
            //        try
            //        {
            //            //if (!line.StartsWith("S1") && !line.StartsWith("S2") && !line.StartsWith("S3"))
            //            //    continue;

            //            var str = line.Trim(' ');
            //            if (!string.IsNullOrEmpty(str))
            //                temp.Add(str);
            //        }
            //        catch (Exception)
            //        {
            //            // ignored
            //        }
            //    }
            //}

            var listLineData = new List<SRecordLineData>();

            foreach (var t in temp)
            {
                var thisLineData = new SRecordLineData();
                var tempCheckSum = (byte)0;

                var stype = t.Substring(0, 2);

                // 获取Type
                for (var i = 0; i < (int)SRecordType.Smax; i++)
                {
                    if (stype != SRecordTypeTable[i])
                        continue;
                    thisLineData.Type = EnumOperater.GetEnum<SRecordType>(SRecordTypeTable[i]);
                    break;
                }

                // 获取Count
                thisLineData.Count = Convert.ToByte(t.Substring(2, 2), 16);
                tempCheckSum += thisLineData.Count;
                if (thisLineData.Count != t.Length / 2 - 2)
                {
                    // error
                    return new List<SRecordLineData>();
                }

                // 获取Address
                if (thisLineData.Type == SRecordType.S0)
                {
                    thisLineData.Address = 0x00;
                }
                else if (thisLineData.Type == SRecordType.S1 || thisLineData.Type == SRecordType.S5 || thisLineData.Type == SRecordType.S9)
                {
                    thisLineData.Address = Convert.ToUInt32(t.Substring(4, 4), 16);

                    tempCheckSum += (byte)(thisLineData.Address >> 8 & 0x0FF);
                    tempCheckSum += (byte)(thisLineData.Address & 0x0FF);

                    var dataLen = t.Length / 2 - 1 - 1 - 1 - 2;
                    thisLineData.Data = new byte[dataLen];
                    for (var i = 0; i < thisLineData.Count - 3; i++)
                    {
                        var data = Convert.ToByte(t.Substring(2 * i + 8, 2), 16);
                        thisLineData.Data[i] = data;
                        tempCheckSum += data;
                    }

                    thisLineData.CheckSum = Convert.ToByte(t.Substring(t.Length - 2, 2), 16);
                    tempCheckSum = (byte)(0xFF - tempCheckSum);
                    if (thisLineData.CheckSum != tempCheckSum)
                    {
                        // checksum error
                        return new List<SRecordLineData>();
                    }
                }
                else if (thisLineData.Type == SRecordType.S2 || thisLineData.Type == SRecordType.S8)
                {
                    thisLineData.Address = Convert.ToUInt32(t.Substring(4, 6), 16);

                    tempCheckSum += (byte)(thisLineData.Address >> 16 & 0xFF);
                    tempCheckSum += (byte)(thisLineData.Address >> 8 & 0xFF);
                    tempCheckSum += (byte)(thisLineData.Address & 0xFF);

                    var dataLen = t.Length / 2 - 1 - 1 - 1 - 3;
                    thisLineData.Data = new byte[dataLen];
                    for (var i = 0; i < thisLineData.Count - 4; i++)
                    {
                        var data = Convert.ToByte(t.Substring(2 * i + 10, 2), 16);
                        thisLineData.Data[i] = data;
                        tempCheckSum += data;
                    }

                    thisLineData.CheckSum = Convert.ToByte(t.Substring(t.Length - 2, 2), 16);
                    tempCheckSum = (byte)(0xFF - tempCheckSum);
                    if (thisLineData.CheckSum != tempCheckSum)
                    {
                        // checksum error
                        return new List<SRecordLineData>();
                    }
                }
                else if (thisLineData.Type == SRecordType.S3 || thisLineData.Type == SRecordType.S7)
                {
                    thisLineData.Address = Convert.ToUInt32(t.Substring(4, 8), 16);

                    tempCheckSum += (byte)(thisLineData.Address >> 24 & 0xFF);
                    tempCheckSum += (byte)(thisLineData.Address >> 16 & 0xFF);
                    tempCheckSum += (byte)(thisLineData.Address >> 8 & 0xFF);
                    tempCheckSum += (byte)(thisLineData.Address & 0xFF);

                    var dataLen = t.Length / 2 - 1 - 1 - 1 - 4;
                    thisLineData.Data = new byte[dataLen];
                    for (var i = 0; i < thisLineData.Count - 5; i++)
                    {
                        var data = Convert.ToByte(t.Substring(2 * i + 12, 2), 16);
                        thisLineData.Data[i] = data;
                        tempCheckSum += data;
                    }

                    thisLineData.CheckSum = Convert.ToByte(t.Substring(t.Length - 2, 2), 16);
                    tempCheckSum = (byte)(0xFF - tempCheckSum);
                    if (thisLineData.CheckSum != tempCheckSum)
                    {
                        // checksum error
                        return new List<SRecordLineData>();
                    }
                }

                // 获取DataLen
                if (thisLineData.Type == SRecordType.S1)
                    thisLineData.DataLen = (byte)(thisLineData.Count - 0x03);
                else if (thisLineData.Type == SRecordType.S2)
                    thisLineData.DataLen = (byte)(thisLineData.Count - 0x04);
                else if (thisLineData.Type == SRecordType.S3)
                    thisLineData.DataLen = (byte)(thisLineData.Count - 0x05);
                else
                    thisLineData.DataLen = 0x00;

                listLineData.Add(thisLineData);
            }

            return listLineData;
        }

        public static List<List<SRecordLineData>> GetBlocks(IEnumerable<SRecordLineData> s19Lines)
        {
            var dataLines = s19Lines.Where(
                t => t.Type == SRecordType.S1 || t.Type == SRecordType.S2 || t.Type == SRecordType.S3)
                .ToList().OrderBy(s => s.Address);

            var blockList = new List<List<SRecordLineData>>();

            var addrBase = (uint)0;
            foreach (var t in dataLines)
            {
                if (addrBase != t.Address)
                    blockList.Add(new List<SRecordLineData>());
                else
                {
                    if (addrBase == 0 && t.Address == 0)
                    {
                        blockList.Add(new List<SRecordLineData>());
                    }
                }

                blockList[blockList.Count - 1].Add(t);

                addrBase = t.Address + t.DataLen;
            }

            return blockList;
        }

        private static readonly string[] SRecordTypeTable =
        {
            "S0","S1","S2","S3","S5","S7","S8","S9",
        };

        public enum SRecordType
        {
            S0 = 0,

            S1,

            S2,

            S3,

            S5,

            S7,

            S8,

            S9,

            Smax
        }

        public struct SRecordLineData
        {
            public SRecordType Type;

            public byte Count;

            public uint Address;

            public byte[] Data;

            public byte CheckSum;

            public byte DataLen;
        }
    }
}
