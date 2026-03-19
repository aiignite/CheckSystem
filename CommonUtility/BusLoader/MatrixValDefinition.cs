using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonUtility.BusLoader
{
    /// <summary>
    /// 矩阵数据定义
    /// </summary>
    public class MatrixValDefinition : Attribute
    {
        /// <summary>
        /// 起始位
        /// </summary>
        public int StartBit { get; private set; }

        /// <summary>
        /// 长度
        /// </summary>
        public int Len { get; private set; }

        /// <summary>
        /// 内容
        /// </summary>
        public byte Value { get; set; }

        /// <summary>
        /// 矩阵定义
        /// </summary>
        /// <param name="startBit">起始位</param>
        /// <param name="len">长度</param>
        /// <param name="value">内容</param>
        public MatrixValDefinition(int startBit, int len, byte value)
        {
            StartBit = startBit;
            Len = len;
            Value = value;
        }

        //public MatrixValDefinition(int startBit, int len, ulong value)
        //{
        //    StartBit = startBit;
        //    Len = len;
        //    Value = value;
        //}

        public void MotorolaUpdate(byte[] motorolaMatrixData)
        {
            var originData = new byte[motorolaMatrixData.Length];
            Array.Copy(motorolaMatrixData, 0, originData, 0, originData.Length);

            try
            {
                const int horizontal = 8;
                var vertical = motorolaMatrixData.Length;
                var strs = new string[vertical, horizontal];

                for (var i = 0; i < vertical; i++)
                {
                    var bChars =
                        Convert.ToString(motorolaMatrixData[i], 2).PadLeft(8, '0');
                    for (var j = 7; j >= 0; j--)
                        strs[i, 8 - j - 1] = bChars[j].ToString();
                }

                var valBools =
                    Convert.ToString(Value, 2).PadLeft(Len, '0').ToCharArray();
                Array.Reverse(valBools);

                var mV = StartBit / 8;
                var mH = StartBit;
                var mI = 0;
                do
                {
                    strs[mV, mH % 8] =
                        valBools[mI].ToString();
                    mH++;
                    mI++;
                    if (mH % 8 != 0)
                        continue;
                    mH = mH - 16;
                    mV--;
                } while (mI < Len);

                for (var i = 0; i < vertical; i++)
                {
                    var tempList = new List<string>();
                    for (var j = 0; j < horizontal; j++)
                        tempList.Add(strs[i, j]);
                    var tempArray = tempList.ToArray();
                    Array.Reverse(tempArray);
                    var bsStr = tempArray.Aggregate(
                        string.Empty, (current, t) => current + t.ToString());
                    motorolaMatrixData[i] = Convert.ToByte(bsStr, 2);
                }
            }
            catch (Exception)
            {
                Array.Copy(originData, 0, motorolaMatrixData, 0, originData.Length);
            }
        }

        public void IntelUpdate(byte[] intelMatrixData)
        {
            var originData = new byte[intelMatrixData.Length];
            Array.Copy(intelMatrixData, 0, originData, 0, originData.Length);

            try
            {
                var bsList = new List<string>();
                foreach (var bs in intelMatrixData.Select(t => Convert.ToString(t, 2).PadLeft(8, '0')))
                    for (var i = bs.Length - 1; i >= 0; i--)
                        bsList.Add(bs[i].ToString());

                var valueBsList = new List<string>();
                var valueBs = Convert.ToString(Value, 2).PadLeft(8, '0');
                for (var i = valueBs.Length - 1; i >= 0; i--)
                    valueBsList.Add(valueBs[i].ToString());

                var temp = 0;
                for (var i = 0; i < Len; i++)
                {
                    bsList[StartBit + i] = valueBsList[temp];
                    temp++;
                }

                var tempB = new List<byte>();
                for (var i = 0; i < bsList.Count; i = i + 8)
                {
                    var b0 = bsList[i];
                    var b1 = bsList[i + 1];
                    var b2 = bsList[i + 2];
                    var b3 = bsList[i + 3];
                    var b4 = bsList[i + 4];
                    var b5 = bsList[i + 5];
                    var b6 = bsList[i + 6];
                    var b7 = bsList[i + 7];
                    var b = Convert.ToByte(
                        string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", b7, b6, b5, b4, b3, b2, b1, b0), 2);
                    tempB.Add(b);
                }

                Array.Copy(tempB.ToArray(), 0, intelMatrixData, 0, intelMatrixData.Length);
            }
            catch (Exception)
            {
                Array.Copy(originData, 0, intelMatrixData, 0, originData.Length);
            }
        }

        public ulong GetMotorolaSignalValue(byte[] data)
        {
            var bitLength = Len;
            var startBit = StartBit;

            var bits = new bool[bitLength];
            var currentByte = startBit / 8;
            var currentBitInByte = startBit % 8;
            for (var i = 0; i < bitLength; i++)
            {
                bits[i] = ((data[currentByte] >> currentBitInByte) & 1) == 1;
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
            //for (int i = data.Length - 1, j = 0; i >= 0; i--, j++)
            //    canSignalValue += (ulong)data[j] << (i * 8);

            //var x = StartBit / 8;
            //var y = StartBit % 8;
            //var z = x * 8 + 8 - y;
            //var rightMoveCount = data.Length * 8 - z;

            //canSignalValue >>= rightMoveCount;

            //return canSignalValue & ulong.MaxValue >> 64 - Len;
        }

        public ulong GetIntelSignalValue(byte[] data)
        {
            var bsList = new List<string>();
            foreach (var bs in data.Select(t => Convert.ToString(t, 2).PadLeft(8, '0')))
                for (var i = bs.Length - 1; i >= 0; i--)
                    bsList.Add(bs[i].ToString());

            var tempBs = new List<string>();
            for (var i = 0; i < Len; i++)
                tempBs.Add(bsList[StartBit + i]);

            var str = string.Empty;
            for (var i = tempBs.Count - 1; i >= 0; i--)
                str += tempBs[i];
            return Convert.ToUInt64(str, 2);
        }
    }
}
