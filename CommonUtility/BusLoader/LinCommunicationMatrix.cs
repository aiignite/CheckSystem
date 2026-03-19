using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonUtility.BusLoader
{
    /// <summary>
    /// LIN通讯矩阵
    /// </summary>
    public abstract class LinCommunicationMatrix
    {
        /// <summary>
        /// 主节点的命令帧ID
        /// </summary>
        public byte MasterLinId { get; private set; }

        /// <summary>
        /// 消息长度
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public byte[] MatrixData { get; set; }

        /// <summary>
        /// LIN通讯矩阵
        /// </summary>
        /// <param name="masterLinId">主节点的命令帧ID</param>
        /// <param name="size">消息长度</param>
        protected LinCommunicationMatrix(byte masterLinId, int size)
        {
            MasterLinId = masterLinId;
            Size = size;
            MatrixData = new byte[size];
        }

        /// <summary>
        /// 更新矩阵数据
        /// </summary>
        /// <param name="mvd">矩阵数据定义</param>
        public abstract void UpdateData(MatrixValDefinition mvd);

        public abstract ulong GetMatrixData(int startBit, int bitLength);

        /// <summary>
        /// LIN通讯矩阵
        /// Motorola格式
        /// </summary>
        public sealed class MotorolaMatrix : LinCommunicationMatrix
        {
            /// <summary>
            /// LIN通讯矩阵
            /// Motorola格式
            /// </summary>
            /// <param name="masterLinId">主节点的命令帧ID</param>
            /// <param name="size">消息长度</param>
            public MotorolaMatrix(
                byte masterLinId, int size)
                : base(masterLinId, size) { }

            /// <summary>
            /// 更新矩阵数据
            /// </summary>
            /// <param name="mvd">矩阵数据定义</param>
            public override void UpdateData(MatrixValDefinition mvd)
            {
                mvd.MotorolaUpdate(MatrixData);
            }

            public override ulong GetMatrixData(int startBit, int bitLength)
            {
                ulong canSignalValue = 0;
                for (int i = MatrixData.Length - 1, j = 0; i >= 0; i--, j++)
                    canSignalValue += (ulong)MatrixData[j] << (i * 8);

                var x = startBit / 8;
                var y = startBit % 8;
                var z = x * 8 + 8 - y;
                var rightMoveCount = MatrixData.Length * 8 - z;

                canSignalValue >>= rightMoveCount;

                return canSignalValue & ulong.MaxValue >> 64 - bitLength;
            }
        }

        /// <summary>
        /// LIN通讯矩阵
        /// Intel格式
        /// </summary>
        public sealed class IntelMatrix : LinCommunicationMatrix
        {
            /// <summary>
            /// LIN通讯矩阵
            /// Intel格式
            /// </summary>
            /// <param name="masterLinId">主节点的命令帧ID</param>
            /// <param name="size">消息长度</param>
            public IntelMatrix(
                 byte masterLinId, int size)
                : base(masterLinId, size) { }

            /// <summary>
            /// 更新矩阵数据
            /// </summary>
            /// <param name="mvd">矩阵数据定义</param>
            public override void UpdateData(MatrixValDefinition mvd)
            {
                mvd.IntelUpdate(MatrixData);
            }

            public override ulong GetMatrixData(int startBit, int bitLength)
            {
                var bsList = new List<string>();
                foreach (var bs in MatrixData.Select(t => Convert.ToString(t, 2).PadLeft(8, '0')))
                    for (var i = bs.Length - 1; i >= 0; i--)
                        bsList.Add(bs[i].ToString());

                var tempBs = new List<string>();
                for (var i = 0; i < bitLength; i++)
                    tempBs.Add(bsList[startBit + i]);

                var str = string.Empty;
                for (var i = tempBs.Count - 1; i >= 0; i--)
                    str += tempBs[i];
                return Convert.ToUInt64(str, 2);
            }
        }
    }
}
