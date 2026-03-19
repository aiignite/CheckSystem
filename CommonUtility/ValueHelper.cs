using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUtility
{
    public static class ValueHelper
    {
        /// <summary>
        /// CRC-32 校验
        /// </summary>
        /// <param name="bytes">CRC-32校验内容</param>
        /// <returns>CRC-32校验结果</returns>
        public static IEnumerable<byte> Crc32(byte[] bytes)
        {
            const uint defaultPolynomial = 0xEDB88320u;
            const uint defaultSeed = 0xFFFFFFFFu;
            var defaultTable = new uint[256];
            //uint crc;

            for (var i = 0; i < 256; i++)
            {
                var entry = (uint)i;
                for (var j = 0; j < 8; j++)
                {
                    if ((entry & 1) == 1)
                    {
                        entry = (entry >> 1) ^ defaultPolynomial;
                    }
                    else
                    {
                        entry >>= 1;
                    }
                }
                defaultTable[i] = entry;
            }

            var hash = defaultSeed;

            var nLen = bytes.Length;
            for (var j = 0; j < nLen; j++)
                hash = (hash >> 8) ^ defaultTable[bytes[j] ^ hash & 0xFF];

            return BitConverter.GetBytes(defaultSeed - hash);
        }

        /// <summary>
        /// CRC-16 MODBUS校验
        /// </summary>
        /// <param name="bytes">CRC-16校验内容</param>
        /// <returns>CRC-16校验结果</returns>
        public static IEnumerable<byte> Crc16(IEnumerable<byte> bytes)
        {
            ushort newLoad = 0xffff;
            var count = 0;

            foreach (var value in bytes.Select(t => (ushort)t))
            {
                newLoad = (ushort)(Convert.ToInt32(value) ^ Convert.ToInt32(newLoad));
                const ushort inValue = 0xA001;

                while (count < 8)
                {
                    if (Convert.ToInt32(newLoad) % 2 == 1)
                    {
                        newLoad -= 0x00001;
                        newLoad = (ushort)(Convert.ToInt32(newLoad) / 2);
                        count++;
                        newLoad = (ushort)(Convert.ToInt32(newLoad) ^ Convert.ToInt32(inValue));
                    }
                    else
                    {
                        newLoad = (ushort)(Convert.ToInt32(newLoad) / 2);
                        count++;
                    }
                }
                count = 0;
            }

            return BitConverter.GetBytes(newLoad);
        }

        private static readonly byte[] aucCRCHi = {
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
             0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
             0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40
         };

        private static readonly byte[] aucCRCLo = {
             0x00, 0xC0, 0xC1, 0x01, 0xC3, 0x03, 0x02, 0xC2, 0xC6, 0x06, 0x07, 0xC7,
             0x05, 0xC5, 0xC4, 0x04, 0xCC, 0x0C, 0x0D, 0xCD, 0x0F, 0xCF, 0xCE, 0x0E,
             0x0A, 0xCA, 0xCB, 0x0B, 0xC9, 0x09, 0x08, 0xC8, 0xD8, 0x18, 0x19, 0xD9,
             0x1B, 0xDB, 0xDA, 0x1A, 0x1E, 0xDE, 0xDF, 0x1F, 0xDD, 0x1D, 0x1C, 0xDC,
             0x14, 0xD4, 0xD5, 0x15, 0xD7, 0x17, 0x16, 0xD6, 0xD2, 0x12, 0x13, 0xD3,
             0x11, 0xD1, 0xD0, 0x10, 0xF0, 0x30, 0x31, 0xF1, 0x33, 0xF3, 0xF2, 0x32,
             0x36, 0xF6, 0xF7, 0x37, 0xF5, 0x35, 0x34, 0xF4, 0x3C, 0xFC, 0xFD, 0x3D,
             0xFF, 0x3F, 0x3E, 0xFE, 0xFA, 0x3A, 0x3B, 0xFB, 0x39, 0xF9, 0xF8, 0x38,
             0x28, 0xE8, 0xE9, 0x29, 0xEB, 0x2B, 0x2A, 0xEA, 0xEE, 0x2E, 0x2F, 0xEF,
             0x2D, 0xED, 0xEC, 0x2C, 0xE4, 0x24, 0x25, 0xE5, 0x27, 0xE7, 0xE6, 0x26,
             0x22, 0xE2, 0xE3, 0x23, 0xE1, 0x21, 0x20, 0xE0, 0xA0, 0x60, 0x61, 0xA1,
             0x63, 0xA3, 0xA2, 0x62, 0x66, 0xA6, 0xA7, 0x67, 0xA5, 0x65, 0x64, 0xA4,
             0x6C, 0xAC, 0xAD, 0x6D, 0xAF, 0x6F, 0x6E, 0xAE, 0xAA, 0x6A, 0x6B, 0xAB,
             0x69, 0xA9, 0xA8, 0x68, 0x78, 0xB8, 0xB9, 0x79, 0xBB, 0x7B, 0x7A, 0xBA,
             0xBE, 0x7E, 0x7F, 0xBF, 0x7D, 0xBD, 0xBC, 0x7C, 0xB4, 0x74, 0x75, 0xB5,
             0x77, 0xB7, 0xB6, 0x76, 0x72, 0xB2, 0xB3, 0x73, 0xB1, 0x71, 0x70, 0xB0,
             0x50, 0x90, 0x91, 0x51, 0x93, 0x53, 0x52, 0x92, 0x96, 0x56, 0x57, 0x97,
             0x55, 0x95, 0x94, 0x54, 0x9C, 0x5C, 0x5D, 0x9D, 0x5F, 0x9F, 0x9E, 0x5E,
             0x5A, 0x9A, 0x9B, 0x5B, 0x99, 0x59, 0x58, 0x98, 0x88, 0x48, 0x49, 0x89,
             0x4B, 0x8B, 0x8A, 0x4A, 0x4E, 0x8E, 0x8F, 0x4F, 0x8D, 0x4D, 0x4C, 0x8C,
             0x44, 0x84, 0x85, 0x45, 0x87, 0x47, 0x46, 0x86, 0x82, 0x42, 0x43, 0x83,
             0x41, 0x81, 0x80, 0x40
         };

        /// <summary>
        ///  CRC-16 MODBUS校验
        /// </summary>
        /// <param name="pucFrame">CRC-16校验内容</param>
        /// <param name="usLen">CRC-16校验结果,一般2个Byte</param>
        /// <returns></returns>
        public static byte[] GetCrc16(List<byte> pucFrame, int usLen)
        {
            byte[] crcLoHi = new byte[2];
            if (pucFrame == null) return null;
            int i = 0;
            byte ucCRCHi = 0xFF;
            byte ucCRCLo = 0xFF;
            UInt16 iIndex = 0x0000;
            while (usLen-- > 0)
            {
                iIndex = (UInt16)(ucCRCLo ^ pucFrame[i++]);
                ucCRCLo = (byte)(ucCRCHi ^ aucCRCHi[iIndex]);
                ucCRCHi = aucCRCLo[iIndex];
            }
            crcLoHi[0] = ucCRCLo;
            crcLoHi[1] = ucCRCHi;
            return crcLoHi;
        }

        /// <summary>
        /// CRC-16 IBM校验
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static IEnumerable<byte> IbmCrc16(IEnumerable<byte> bytes)
        {
            int[] table =
            {
                0x0000, 0xC0C1, 0xC181, 0x0140, 0xC301, 0x03C0,
                0x0280, 0xC241, 0xC601, 0x06C0, 0x0780, 0xC741,
                0x0500, 0xC5C1, 0xC481, 0x0440, 0xCC01, 0x0CC0,
                0x0D80, 0xCD41, 0x0F00, 0xCFC1, 0xCE81, 0x0E40,
                0x0A00, 0xCAC1, 0xCB81, 0x0B40, 0xC901, 0x09C0,
                0x0880, 0xC841, 0xD801, 0x18C0, 0x1980, 0xD941,
                0x1B00, 0xDBC1, 0xDA81, 0x1A40, 0x1E00, 0xDEC1,
                0xDF81, 0x1F40, 0xDD01, 0x1DC0, 0x1C80, 0xDC41,
                0x1400, 0xD4C1, 0xD581, 0x1540, 0xD701, 0x17C0,
                0x1680, 0xD641, 0xD201, 0x12C0, 0x1380, 0xD341,
                0x1100, 0xD1C1, 0xD081, 0x1040, 0xF001, 0x30C0,
                0x3180, 0xF141, 0x3300, 0xF3C1, 0xF281, 0x3240,
                0x3600, 0xF6C1, 0xF781, 0x3740, 0xF501, 0x35C0,
                0x3480, 0xF441, 0x3C00, 0xFCC1, 0xFD81, 0x3D40,
                0xFF01, 0x3FC0, 0x3E80, 0xFE41, 0xFA01, 0x3AC0,
                0x3B80, 0xFB41, 0x3900, 0xF9C1, 0xF881, 0x3840,
                0x2800, 0xE8C1, 0xE981, 0x2940, 0xEB01, 0x2BC0,
                0x2A80, 0xEA41, 0xEE01, 0x2EC0, 0x2F80, 0xEF41,
                0x2D00, 0xEDC1, 0xEC81, 0x2C40, 0xE401, 0x24C0,
                0x2580, 0xE541, 0x2700, 0xE7C1, 0xE681, 0x2640,
                0x2200, 0xE2C1, 0xE381, 0x2340, 0xE101, 0x21C0,
                0x2080, 0xE041, 0xA001, 0x60C0, 0x6180, 0xA141,
                0x6300, 0xA3C1, 0xA281, 0x6240, 0x6600, 0xA6C1,
                0xA781, 0x6740, 0xA501, 0x65C0, 0x6480, 0xA441,
                0x6C00, 0xACC1, 0xAD81, 0x6D40, 0xAF01, 0x6FC0,
                0x6E80, 0xAE41, 0xAA01, 0x6AC0, 0x6B80, 0xAB41,
                0x6900, 0xA9C1, 0xA881, 0x6840, 0x7800, 0xB8C1,
                0xB981, 0x7940, 0xBB01, 0x7BC0, 0x7A80, 0xBA41,
                0xBE01, 0x7EC0, 0x7F80, 0xBF41, 0x7D00, 0xBDC1,
                0xBC81, 0x7C40, 0xB401, 0x74C0, 0x7580, 0xB541,
                0x7700, 0xB7C1, 0xB681, 0x7640, 0x7200, 0xB2C1,
                0xB381, 0x7340, 0xB101, 0x71C0, 0x7080, 0xB041,
                0x5000, 0x90C1, 0x9181, 0x5140, 0x9301, 0x53C0,
                0x5280, 0x9241, 0x9601, 0x56C0, 0x5780, 0x9741,
                0x5500, 0x95C1, 0x9481, 0x5440, 0x9C01, 0x5CC0,
                0x5D80, 0x9D41, 0x5F00, 0x9FC1, 0x9E81, 0x5E40,
                0x5A00, 0x9AC1, 0x9B81, 0x5B40, 0x9901, 0x59C0,
                0x5880, 0x9841, 0x8801, 0x48C0, 0x4980, 0x8941,
                0x4B00, 0x8BC1, 0x8A81, 0x4A40, 0x4E00, 0x8EC1,
                0x8F81, 0x4F40, 0x8D01, 0x4DC0, 0x4C80, 0x8C41,
                0x4400, 0x84C1, 0x8581, 0x4540, 0x8701, 0x47C0,
                0x4680, 0x8641, 0x8201, 0x42C0, 0x4380, 0x8341,
                0x4100, 0x81C1, 0x8081, 0x4040
            };

            var crc = bytes.Aggregate(0x0000, (current, b) => (current >> 8) ^ table[(current ^ b) & 0xff]);
            return new[] { (byte)(0xff & crc), (byte)((0xff00 & crc) >> 8) };
        }

        /// <summary>
        /// 获取16进制字符串，以0x开头
        /// </summary>
        /// <param name="val">需要转换的字节</param>
        /// <returns></returns>
        public static string GetHextStrWithOx(byte val)
        {
            return string.Format("0x{0}", string.Format("{0:X}", val).PadLeft(2, '0'));
        }

        /// <summary>
        /// 获取16进制字符串，以0x开头
        /// </summary>
        /// <param name="vals">需要转换的字节</param>
        /// <returns></returns>
        public static string GetHextStrWithOx(byte[] vals)
        {
            return vals == null
                ? string.Empty
                : vals.Aggregate(string.Empty, (current, t) => current + GetHextStrWithOx(t) + " ").Trim(' ');
        }

        /// <summary>
        /// 获取16进制字符串
        /// </summary>
        /// <param name="val">需要转换的字节</param>
        /// <returns></returns>
        public static string GetHextStr(byte val)
        {
            return string.Format("{0:X}", val).PadLeft(2, '0');
        }

        /// <summary>
        /// 获取16进制字符串
        /// </summary>
        /// <param name="vals">需要转换的字节</param>
        /// <returns></returns>
        public static string GetHextStr(byte[] vals)
        {
            return vals == null
                ? string.Empty
                : vals.Aggregate(string.Empty, (current, t) => current + GetHextStr(t) + " ").Trim(' ');
        }

        /// <summary>
        /// 获取高位
        /// </summary>
        /// <param name="val">输入的字节</param>
        /// <returns></returns>
        public static byte GetByteHighOrder(this byte val)
        {
            return val.GetBytesWithHiLo()[0];
            //return (byte) ((val & 0xf0) >> 4);
        }

        /// <summary>
        /// 获取低位
        /// </summary>
        /// <param name="val">输入的字节</param>
        /// <returns></returns>
        public static byte GetByteLowOrder(this byte val)
        {
            return val.GetBytesWithHiLo()[1];
        }

        public static byte[] GetBytesWithHiLo(this byte val)
        {
            var h = (byte)((val & 0xf0) >> 4);
            var l = (byte)(val & 0x0f);
            return new[] { h, l };
        }

        /// <summary>
        /// 获取ASCII字符串
        /// </summary>
        /// <param name="val">ASCII码</param>
        /// <param name="isConvertSpecialCharacter">是否显示不可见字符</param>
        /// <returns></returns>
        public static string GetStringByAsciiByte(this byte val, bool isConvertSpecialCharacter)
        {
            if (!isConvertSpecialCharacter)
                return Encoding.ASCII.GetString(new[] { val });

            if (val > 0x7F)
                return string.Empty;

            if (val >= 0x21 && val <= 0x7E)
                return Encoding.ASCII.GetString(new[] { val });

            switch (val)
            {
                case 0x00:
                    return "<NUL>";

                case 0x01:
                    return "<SOH>";

                case 0x02:
                    return "<STX>";

                case 0x03:
                    return "<ETX>";

                case 0x04:
                    return "<EOT>";

                case 0x05:
                    return "<ENQ>";

                case 0x06:
                    return "<ACK>";

                case 0x07:
                    return "<BEL>";

                case 0x08:
                    return "<BS>";

                case 0x09:
                    return "<HT>";

                case 0x0A:
                    return "<LF>";

                case 0x0B:
                    return "<VT>";

                case 0x0C:
                    return "<FF>";

                case 0x0D:
                    return "<CR>";

                case 0x0E:
                    return "<SO>";

                case 0x0F:
                    return "<SI>";

                case 0x10:
                    return "<DLE>";

                case 0x11:
                    return "<DC1>";

                case 0x12:
                    return "<DC2>";

                case 0x13:
                    return "<DC3>";

                case 0x14:
                    return "<DC4>";

                case 0x15:
                    return "<NAK>";

                case 0x16:
                    return "<SYN>";

                case 0x17:
                    return "<ETB>";

                case 0x18:
                    return "<CAN>";

                case 0x19:
                    return "<EM>";

                case 0x1A:
                    return "<SUB>";

                case 0x1B:
                    return "<ESCC>";

                case 0x1C:
                    return "<FS>";

                case 0x1D:
                    return "<GS>";

                case 0x1E:
                    return "<RS>";

                case 0x1F:
                    return "<US>";

                case 0x20:
                    return "<SP>";

                case 0x7F:
                    return "<DEL>";

                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// 获取ASCII字符串
        /// </summary>
        /// <param name="vals">ASCII码</param>
        /// <param name="isConvertSpecialCharacter">是否显示不可见字符</param>
        /// <returns></returns>
        public static string GetStringByAsciiBytes(this byte[] vals, bool isConvertSpecialCharacter)
        {
            if (vals == null)
                return string.Empty;

            return !isConvertSpecialCharacter
                ? Encoding.ASCII.GetString(vals, 0, vals.Length)
                : vals.Aggregate(string.Empty, (current, t) => current + t.GetStringByAsciiByte(true));
        }

        public static string[] GetBits(byte[] data)
        {
            if (data is null || data.Length is 0)
                return new string[0];

            var bits = new List<string>();
            for (var i = 0; i < 8; i++)
            {
                var bt = Convert.ToString(data[i], 2).PadLeft(8, '0');
                for (var j = bt.Length - 1; j >= 0; j--)
                {
                    bits.Add(bt[j].ToString());
                }
            }

            return bits.ToArray();
        }

        public static int GetTimeSpanMs(DateTime startTime, DateTime endTime)
        {
            try
            {
                var ts = endTime - startTime;
                return (int)ts.TotalMilliseconds;
            }
            catch (Exception)
            {
                return -9999;
            }
        }

        public static int GetDecimal(byte[] value)
        {
            if (value == null)
                return 0;

            var temp = 0;
            for (var i = 0; i < value.Length; i++)
            {
                var t = (int)value[i];
                //temp = result[i];
                for (var j = value.Length - 1; j > i; j--)
                {
                    t = t * 256;
                }

                temp = temp + t;
            }

            return temp;
        }
    }
}
