namespace CommonUtility
{
    public class CrcHelper
    {
        public struct CrcInfo
        {
            /// <summary>
            /// CRC位数
            /// </summary>
            public byte Width;

            /// <summary>
            /// CRC多项式
            /// </summary>
            public uint Poly;

            /// <summary>
            /// CRC初始值
            /// </summary>
            public uint InitReg;

            /// <summary>
            /// 输入数据反转
            /// </summary>
            public bool Refin;

            /// <summary>
            /// 输出数据反转
            /// </summary>
            public bool Refout;

            /// <summary>
            /// 结果异或值
            /// </summary>
            public uint Xorout;
        };

        public readonly CrcInfo Crc4Itu = new CrcInfo
        {
            Width = 4,
            Poly = 0x03,
            InitReg = 0x00,
            Refin = true,
            Refout = true,
            Xorout = 0x00
        };

        public readonly CrcInfo Crc5Epc = new CrcInfo
        {
            Width = 5,
            Poly = 0x09,
            InitReg = 0x09,
            Refin = false,
            Refout = false,
            Xorout = 0x00
        };

        public readonly CrcInfo Crc5Itu = new CrcInfo
        {
            Width = 5,
            Poly = 0x15,
            InitReg = 0x00,
            Refin = true,
            Refout = true,
            Xorout = 0x00
        };

        public readonly CrcInfo Crc5Usb = new CrcInfo
        {
            Width = 5,
            Poly = 0x05,
            InitReg = 0x1f,
            Refin = true,
            Refout = true,
            Xorout = 0x1f
        };

        public readonly CrcInfo Crc6Itu = new CrcInfo
        {
            Width = 6,
            Poly = 0x03,
            InitReg = 0x00,
            Refin = true,
            Refout = true,
            Xorout = 0x00
        };

        public readonly CrcInfo Crc7Mmc = new CrcInfo
        {
            Width = 7,
            Poly = 0x09,
            InitReg = 0x00,
            Refin = false,
            Refout = false,
            Xorout = 0x00
        };

        public readonly CrcInfo Crc8 = new CrcInfo
        {
            Width = 8,
            Poly = 0x07,
            InitReg = 0x00,
            Refin = false,
            Refout = false,
            Xorout = 0x00
        };

        public readonly CrcInfo Crc8Itu = new CrcInfo
        {
            Width = 8,
            Poly = 0x07,
            InitReg = 0x00,
            Refin = false,
            Refout = false,
            Xorout = 0x55
        };

        public readonly CrcInfo Crc8Rohc = new CrcInfo
        {
            Width = 8,
            Poly = 0x07,
            InitReg = 0xff,
            Refin = true,
            Refout = true,
            Xorout = 0x00
        };

        public readonly CrcInfo Crc8Maxim = new CrcInfo
        {
            Width = 8,
            Poly = 0x31,
            InitReg = 0x00,
            Refin = true,
            Refout = true,
            Xorout = 0x00
        };

        public readonly CrcInfo Crc16Ibm = new CrcInfo
        {
            Width = 16,
            Poly = 0x8005,
            InitReg = 0x0000,
            Refin = true,
            Refout = true,
            Xorout = 0x0000
        };

        public readonly CrcInfo Crc16Maxim = new CrcInfo
        {
            Width = 16,
            Poly = 0x8005,
            InitReg = 0x0000,
            Refin = true,
            Refout = true,
            Xorout = 0xffff
        };

        public readonly CrcInfo Crc16Usb = new CrcInfo
        {
            Width = 16,
            Poly = 0x8005,
            InitReg = 0xffff,
            Refin = true,
            Refout = true,
            Xorout = 0xffff
        };

        public readonly CrcInfo Crc16Modbus = new CrcInfo
        {
            Width = 16,
            Poly = 0x8005,
            InitReg = 0xffff,
            Refin = true,
            Refout = true,
            Xorout = 0x0000
        };

        public readonly CrcInfo Crc16Ccitt = new CrcInfo
        {
            Width = 16,
            Poly = 0x1021,
            InitReg = 0x0000,
            Refin = true,
            Refout = true,
            Xorout = 0x0000
        };

        public readonly CrcInfo Crc16CcittFalse = new CrcInfo
        {
            Width = 16,
            Poly = 0x1021,
            InitReg = 0xffff,
            Refin = false,
            Refout = false,
            Xorout = 0x0000
        };

        public readonly CrcInfo Crc16X25 = new CrcInfo
        {
            Width = 16,
            Poly = 0x1021,
            InitReg = 0xffff,
            Refin = true,
            Refout = true,
            Xorout = 0xffff
        };

        public readonly CrcInfo Crc16Xmodem = new CrcInfo
        {
            Width = 16,
            Poly = 0x1021,
            InitReg = 0x0000,
            Refin = false,
            Refout = false,
            Xorout = 0x0000
        };

        public readonly CrcInfo Crc16Dnp = new CrcInfo
        {
            Width = 16,
            Poly = 0x3d65,
            InitReg = 0x0000,
            Refin = true,
            Refout = true,
            Xorout = 0xffff
        };

        public readonly CrcInfo Crc32 = new CrcInfo
        {
            Width = 32,
            Poly = 0x04c11db7,
            InitReg = 0xffffffff,
            Refin = true,
            Refout = true,
            Xorout = 0xffffffff
        };

        public readonly CrcInfo Crc32Mpeg = new CrcInfo
        {
            Width = 32,
            Poly = 0x04c11db7,
            InitReg = 0xffffffff,
            Refin = false,
            Refout = false,
            Xorout = 0x00000000
        };

        public uint[] Table = new uint[256];

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

        public void CRC_Table_Init(CrcInfo crcInfo)
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
                    Table[i] = value & validBits;
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
                    Table[i] = value & (crcInfo.Width < 8 ? 0xff : validBits);
                }
            }

            // var info = crcInfo;
        }

        public uint CALC_CRC(CrcInfo info, byte[] memBlock)
        {
            var memBlockLen = (uint)memBlock.Length;
            uint value;

            if (info.Refin)
            {
                value = BitReflected(info.InitReg, info.Width);
                if (info.Width > 8)
                {
                    var i = 0;
                    while (memBlockLen-- != 0)
                    {
                        value = (value >> 8) ^ Table[value & 0xff ^ memBlock[i++]];
                    }
                }
                else
                {
                    var i = 0;
                    while (memBlockLen-- != 0)
                    {
                        value = Table[value ^ memBlock[i++]];
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
                        value = (value << 8) ^ Table[high ^ memBlock[i++]];
                    }
                }
                else
                {
                    value = info.InitReg << (8 - info.Width);
                    var i = 0;
                    while (memBlockLen-- != 0)
                    {
                        value = Table[value ^ memBlock[i++]];
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

        //public UInt32 CALC_CRC(byte[] memBlock)
        //{
        //    return CALC_CRC(info, memBlock);
        //}
    }
}
