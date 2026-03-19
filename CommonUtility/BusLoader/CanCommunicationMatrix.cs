using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonUtility.BusLoader
{
    /// <summary>
    /// CAN通讯矩阵
    /// </summary>
    public abstract class CanCommunicationMatrix
    {
        /// <summary>
        /// CAN ID
        /// </summary>
        public uint CanId { get; set; }

        /// <summary>
        /// 消息长度
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public byte[] MatrixData { get; set; }

        /// <summary>
        /// CAN通讯矩阵
        /// </summary>
        /// <param name="canId">CAN ID</param>
        /// <param name="size">消息长度</param>
        protected CanCommunicationMatrix(uint canId, int size)
        {
            CanId = canId;
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
        /// CAN通讯矩阵
        /// Motorola格式
        /// </summary>
        public sealed class MotorolaMatrix : CanCommunicationMatrix
        {
            /// <summary>
            /// CAN通讯矩阵
            /// Motorola格式
            /// </summary>
            /// <param name="canId">CAN ID</param>
            /// <param name="size">消息长度</param>
            public MotorolaMatrix(
                uint canId, int size)
                : base(canId, size) { }

            /// <summary>
            /// 更新矩阵数据
            /// </summary>
            /// <param name="mvd">矩阵数据定义</param>
            public override void UpdateData(MatrixValDefinition mvd) => mvd.MotorolaUpdate(MatrixData);

            public override ulong GetMatrixData(int startBit, int bitLength)
            {
                var bits = new bool[bitLength];
                var currentByte = startBit / 8;
                var currentBitInByte = startBit % 8;
                for (var i = 0; i < bitLength; i++)
                {
                    bits[i] = ((MatrixData[currentByte] >> currentBitInByte) & 1) == 1;
                    currentBitInByte++;
                    if (currentBitInByte >= 8)
                    {
                        currentByte--;
                        currentBitInByte = 0;
                    }
                }

                Array.Reverse(bits);
                ulong result = 0;

                for (var i = 0; i < bitLength; i++)
                {
                    if (bits[i])
                    {
                        result |= (1UL << (bitLength - 1 - i));
                    }
                }

                return result;

                //ulong canSignalValue = 0;
                //for (int i = MatrixData.Length - 1, j = 0; i >= 0; i--, j++)
                //    canSignalValue += (ulong)MatrixData[j] << (i * 8);

                //var x = startBit / 8;
                //var y = startBit % 8;
                //var z = x * 8 + 8 - y;
                //var rightMoveCount = MatrixData.Length * 8 - z;

                //canSignalValue >>= rightMoveCount;

                //return canSignalValue & ulong.MaxValue >> 64 - bitLength;
            }
        }

        /// <summary>
        /// CAN通讯矩阵
        /// Intel格式
        /// </summary>
        public sealed class IntelMatrix : CanCommunicationMatrix
        {
            /// <summary>
            /// CAN通讯矩阵
            /// Intel格式
            /// </summary>
            /// <param name="canId">CAN ID</param>
            /// <param name="size">消息长度</param>
            public IntelMatrix(
                uint canId, int size) :
                base(canId, size) { }

            /// <summary>
            /// 更新矩阵数据
            /// </summary>
            /// <param name="mvd">矩阵数据定义</param>
            public override void UpdateData(MatrixValDefinition mvd) => mvd.IntelUpdate(MatrixData);

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
