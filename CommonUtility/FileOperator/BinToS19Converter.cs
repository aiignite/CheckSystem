using System;
using System.Collections.Generic;
using System.IO;

namespace CommonUtility.FileOperator
{
    public class BinToS19Converter
    {
        /// <summary>
        /// 将二进制文件转换为S3格式文件
        /// </summary>
        /// <param name="binFilePath">输入的二进制文件路径</param>
        /// <param name="s19FilePath">输出的S19文件路径</param>
        /// <param name="startAddress">起始地址，默认为0x00</param>
        public static void ConvertBinToS19(string binFilePath, string s19FilePath, uint startAddress = 0x00000000)
        {
            if (!File.Exists(binFilePath))
            {
                throw new FileNotFoundException($"二进制文件不存在: {binFilePath}");
            }

            byte[] binData = File.ReadAllBytes(binFilePath);

            using (StreamWriter writer = new StreamWriter(s19FilePath))
            {
                // 写入数据记录（使用S3格式，32位地址）
                uint currentAddress = startAddress;
                int dataIndex = 0;

                while (dataIndex < binData.Length)
                {
                    // 每行最多32字节数据
                    int bytesToWrite = Math.Min(32, binData.Length - dataIndex);

                    // 使用S3记录（32位地址）
                    WriteS3Record(writer, currentAddress, binData, dataIndex, bytesToWrite, out _);

                    currentAddress += (uint)bytesToWrite;
                    dataIndex += bytesToWrite;
                }

                // 写入S7记录（32位地址结束记录）
                WriteS7Record(writer, startAddress, out _);
            }
        }

        public static IEnumerable<SRecordFileHelper.SRecordLineData> ConvertBinToS19Lines(string binFilePath, uint startAddress = 0x00000000)
        {
            if (!File.Exists(binFilePath))
            {
                throw new FileNotFoundException($"二进制文件不存在: {binFilePath}");
            }

            byte[] binData = File.ReadAllBytes(binFilePath);

            var lines = new List<string>();

            {
                // 写入数据记录（使用S3格式，32位地址）
                uint currentAddress = startAddress;
                int dataIndex = 0;

                while (dataIndex < binData.Length)
                {
                    // 每行最多32字节数据
                    int bytesToWrite = Math.Min(32, binData.Length - dataIndex);

                    // S3
                    {
                        int recordLength = 4 + bytesToWrite; // 4字节地址 + 数据长度
                        byte[] record = new byte[1 + recordLength + 1]; // 长度 + 数据 + 校验和

                        record[0] = (byte)(recordLength + 1); // 记录长度（包括长度字节本身）
                        record[1] = (byte)((currentAddress >> 24) & 0xFF); // 地址最高字节
                        record[2] = (byte)((currentAddress >> 16) & 0xFF); // 地址次高字节
                        record[3] = (byte)((currentAddress >> 8) & 0xFF); // 地址次低字节
                        record[4] = (byte)(currentAddress & 0xFF); // 地址最低字节

                        Array.Copy(binData, dataIndex, record, 5, bytesToWrite);

                        // 计算校验和：对记录长度、地址和数据求和，然后取反
                        byte sum = 0;
                        for (int i = 0; i < record.Length - 1; i++)
                        {
                            sum += record[i];
                        }
                        record[record.Length - 1] = (byte)(~sum & 0xFF); // 取反

                        // 写入记录
                        var tpLine = string.Empty;
                        tpLine += "S3";
                        for (int i = 0; i < record.Length; i++)
                        {
                            tpLine += record[i].ToString("X2");
                        }
                        lines.Add(tpLine);
                    }

                    currentAddress += (uint)bytesToWrite;
                    dataIndex += bytesToWrite;
                }

                // 写入S7记录（32位地址结束记录）
                {
                    byte[] record = new byte[1 + 4 + 1]; // 长度 + 4字节地址 + 校验和

                    record[0] = 0x05; // 记录长度（4字节地址 + 长度字节）
                    record[1] = (byte)((startAddress >> 24) & 0xFF); // 地址最高字节
                    record[2] = (byte)((startAddress >> 16) & 0xFF); // 地址次高字节
                    record[3] = (byte)((startAddress >> 8) & 0xFF); // 地址次低字节
                    record[4] = (byte)(startAddress & 0xFF); // 地址最低字节

                    // 计算校验和：对记录长度和地址求和，然后取反
                    byte sum = 0;
                    for (int i = 0; i < record.Length - 1; i++)
                    {
                        sum += record[i];
                    }
                    record[record.Length - 1] = (byte)(~sum & 0xFF); // 取反

                    // 写入记录
                    var tpLine = string.Empty;
                    tpLine += "S7";
                    for (int i = 0; i < record.Length; i++)
                    {
                        tpLine += record[i].ToString("X2");
                    }
                    lines.Add(tpLine);
                }
            }

            return SRecordFileHelper.GetSRecordLineData(lines);

            //using (StreamWriter writer = new StreamWriter(s19FilePath))
            //{
            //    // 写入数据记录（使用S3格式，32位地址）
            //    uint currentAddress = startAddress;
            //    int dataIndex = 0;

            //    while (dataIndex < binData.Length)
            //    {
            //        // 每行最多32字节数据
            //        int bytesToWrite = Math.Min(32, binData.Length - dataIndex);

            //        // 使用S3记录（32位地址）
            //        List<string> s3Lines;
            //        WriteS3Record(writer, currentAddress, binData, dataIndex, bytesToWrite, out s3Lines);
            //        lines.AddRange(lines);

            //        currentAddress += (uint)bytesToWrite;
            //        dataIndex += bytesToWrite;
            //    }

            //    // 写入S7记录（32位地址结束记录）
            //    WriteS7Record(writer, startAddress, out _);
            //}
        }

        /// <summary>
        /// 写入S3记录（32位地址数据记录）
        /// </summary>
        private static void WriteS3Record(StreamWriter writer, uint address, byte[] data, int dataIndex, int byteCount, out List<string> lines)
        {
            lines = new List<string>();

            int recordLength = 4 + byteCount; // 4字节地址 + 数据长度
            byte[] record = new byte[1 + recordLength + 1]; // 长度 + 数据 + 校验和

            record[0] = (byte)(recordLength + 1); // 记录长度（包括长度字节本身）
            record[1] = (byte)((address >> 24) & 0xFF); // 地址最高字节
            record[2] = (byte)((address >> 16) & 0xFF); // 地址次高字节
            record[3] = (byte)((address >> 8) & 0xFF); // 地址次低字节
            record[4] = (byte)(address & 0xFF); // 地址最低字节

            Array.Copy(data, dataIndex, record, 5, byteCount);

            // 计算校验和：对记录长度、地址和数据求和，然后取反
            byte sum = 0;
            for (int i = 0; i < record.Length - 1; i++)
            {
                sum += record[i];
            }
            record[record.Length - 1] = (byte)(~sum & 0xFF); // 取反

            // 写入记录
            var tpLine = string.Empty;
            writer.Write("S3");
            tpLine += "S3";
            for (int i = 0; i < record.Length; i++)
            {
                tpLine += record[i].ToString("X2");
                writer.Write(record[i].ToString("X2"));
            }
            lines.Add(tpLine);
            writer.WriteLine();
        }

        /// <summary>
        /// 写入S7记录（32位地址结束记录）
        /// </summary>
        private static void WriteS7Record(StreamWriter writer, uint startAddress, out List<string> lines)
        {
            lines = new List<string>();
            byte[] record = new byte[1 + 4 + 1]; // 长度 + 4字节地址 + 校验和

            record[0] = 0x05; // 记录长度（4字节地址 + 长度字节）
            record[1] = (byte)((startAddress >> 24) & 0xFF); // 地址最高字节
            record[2] = (byte)((startAddress >> 16) & 0xFF); // 地址次高字节
            record[3] = (byte)((startAddress >> 8) & 0xFF); // 地址次低字节
            record[4] = (byte)(startAddress & 0xFF); // 地址最低字节

            // 计算校验和：对记录长度和地址求和，然后取反
            byte sum = 0;
            for (int i = 0; i < record.Length - 1; i++)
            {
                sum += record[i];
            }
            record[record.Length - 1] = (byte)(~sum & 0xFF); // 取反

            // 写入记录
            var tpLine = string.Empty;
            tpLine += "S7";
            writer.Write("S7");
            for (int i = 0; i < record.Length; i++)
            {
                tpLine += record[i].ToString("X2");
                writer.Write(record[i].ToString("X2"));
            }
            lines.Add(tpLine);
            writer.WriteLine();
        }
    }
}
