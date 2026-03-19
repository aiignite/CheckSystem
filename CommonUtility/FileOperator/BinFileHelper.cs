using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CommonUtility.FileOperator
{
    public static class BinFileHelper
    {
        public static List<BinData> GetBinData(string filePath)
        {
            var listData = new List<BinData>();

            var addr = 0;//地址从0x00000000开始
            var count = 0;//换行显示计数

            if (!File.Exists(filePath))
                return new List<BinData>();

            try
            {
                using (var myFile = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    var binReader = new BinaryReader(myFile);
                    var fileLen = (int)myFile.Length;//bin文件长度
                    var str = new StringBuilder();
                    var binChar = binReader.ReadBytes(fileLen);

                    foreach (var j in binChar)
                    {
                        if (count % 16 == 0)
                        {
                            count = 0;
                            if (addr > 0)
                                str.Append("\r\n");
                            str.Append(addr.ToString("x8") + "      ");
                            listData.Add(new BinData { Address = Convert.ToUInt32(addr.ToString("x8"), 16) });
                            addr = addr + 16;
                        }

                        str.Append(j.ToString("X2") + " ");
                        listData[listData.Count - 1].Data.Add(Convert.ToByte(j.ToString("X2"), 16));
                        if (count == 8)
                            str.Append("  ");
                        count++;
                    }
                    binReader.Close();
                }

                return listData;
            }
            catch (Exception)
            {
                return new List<BinData>();
            }
        }
    }

    public class BinData
    {
        public uint Address;
        public List<byte> Data = new List<byte>();
    }
}
