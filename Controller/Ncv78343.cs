using CommonUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace Controller
{
    public sealed class Ncv78343 : ControllerBase
    {
        [Description("R,Read data result from OTP")]
        public string OtpDataReadResult = string.Empty;

        public MySerialPort MySerialPort500K;
        public MySerialPort MySerialPort250K;

        public Ncv78343(string name) :
            base(name)
        {
            MySerialPort.PushSciMsg += MySerialPort_PushSciMsg;
        }

        ~Ncv78343()
        {
            Dispose();
        }

        private bool _isReadingOtp;
        private byte _readNa = 0xFF;
        private string _readOtpBuff = string.Empty;
        private readonly EventWaitHandle _waitHandle = new AutoResetEvent(false);

        private void MySerialPort_PushSciMsg(string name, byte[] datas)
        {
            if (MySerialPort500K == null || MySerialPort500K.Name != name)
                return;

            var str =
                datas.Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
            Console.WriteLine(str);

            if (_isReadingOtp && _readNa != 0xFF && datas.Length == 11 && datas[0] == 0x00 && datas[1] == 0x55)
            {
                var data = new[] { datas[4], datas[5], datas[6], datas[7], datas[8], datas[9] };
                var tempBuff = PxnConfigurationFrameFormat(_readNa, 0x0B, data);

                if (ValueHelper.GetHextStr(datas).EndsWith(ValueHelper.GetHextStr(tempBuff)))
                {
                    _readOtpBuff = ValueHelper.GetHextStr(tempBuff);
                    _waitHandle.Set();
                }
            }
        }

        [Description("Slave/repeater-slave PXN mode selection")]
        public void SlavePxnModeSelection(byte na, bool isSlaveMode)
        {
            if (MySerialPort250K == null)
                return;

            var pmc = isSlaveMode ? (byte)0x00 : (byte)0x01;
            try
            {
                MySerialPort250K.SendBreakSyncCmd(PxnConfigurationFrameFormat(na, 0x07, new[] { pmc }));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Write data to OTP
        /// </summary>
        /// <param name="lb">OTP_LOCK_BIT</param>
        /// <param name="naLb">OTP_NODE_ADRLCKBIT</param>
        /// <param name="na">OTP_NODE_ADDR</param>
        /// <param name="fssLb">FAIL_SAFE_ST_LCK_BIT</param>
        /// <param name="fss">FAIL_SAFE_ST_LED</param>
        /// <param name="pxnLb">PXN_LOCK_BIT</param>
        /// <param name="mode">MODE</param>
        /// <param name="cs">COMMUNI_SPEED</param>
        /// <param name="gbed">GLOBAL_BIT_ERR_DET_DIS</param>
        /// <param name="mLvdsOff">LVDS_OFF</param>
        /// <param name="uartOff">UART_OFF</param>
        /// <param name="eeLb">EEOPROM_LOCK_BIT</param>
        /// <param name="crc">CRC1</param>
        [Description("Write data to OTP")]
        public void WriteDataToOtp(
            byte lb, byte naLb, byte na, byte fssLb, byte fss, byte pxnLb, byte mode, byte cs, byte gbed, byte mLvdsOff, byte uartOff, byte eeLb, byte crc)
        {
            if (MySerialPort250K == null)
                return;

            try
            {
                var dataBits = new byte[4];
                dataBits[0] = 0x07;
                dataBits[1] = (byte)(0x0F << 4 | eeLb << 3 | uartOff << 2 | mLvdsOff << 1 | gbed << 0);
                dataBits[2] = (byte)(cs << 6 | mode << 5 | pxnLb << 4 | fss << 0);
                dataBits[3] = (byte)(fssLb << 7 | na << 2 | naLb << 1 | lb << 0);

                var len = dataBits.Length / 1;
                var checkCrc = l343_otp_crc(dataBits, len);

                if (checkCrc != crc)
                    return;

                var data = lb |
                           (naLb << 1) |
                           (na << 2) |
                           (fssLb << 7) |
                           (fss << 8) |
                           (pxnLb << 12) |
                           (mode << 13) |
                           (cs << 14) |
                           (gbed << 16) |
                           (mLvdsOff << 17) |
                           (uartOff << 18) |
                           (eeLb << 19) |
                           (crc << 20);

                var dataBytes = new List<byte>
                {
                    0x02,
                    (byte)((data >> 0) & 0xFF),
                    (byte)((data >> 8) & 0xFF),
                    (byte)((data >> 16) & 0xFF),
                    (byte)((data >> 24) & 0xFF),
                    0x00
                };

                MySerialPort250K.SendBreakSyncCmd(PxnConfigurationFrameFormat(na, 0x09, dataBytes.ToArray()));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        [Description("Read data from OTP")]
        public void ReadDataFromOtp(byte na)
        {
            //PxnConfigurationFrameFormat(na, 0x0B);

            OtpDataReadResult = string.Empty;
            _isReadingOtp = false;
            _readNa = 0xFF;
            _readOtpBuff = string.Empty;

            if (MySerialPort500K == null)
                return;

            _readNa = na;
            _isReadingOtp = true;

            l343_otp_request(na);
            Thread.Sleep(15);
            MySerialPort500K.SendBreakSyncCmd(PxnConfigurationFrameFormat(na, 0x0B));

            //Task.Run(() =>
            //{
            //    try
            //    {
            //        l343_otp_request(na);
            //        Thread.Sleep(15);
            //        MySerialPort.SendBreakSyncCmd(PxnConfigurationFrameFormat(na, 0x0B));
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e);
            //    }
            //});

            var isSuccess = _waitHandle.WaitOne(500);

            if (isSuccess)
                OtpDataReadResult = _readOtpBuff;

            _isReadingOtp = false;
            _readNa = 0xFF;
            _readOtpBuff = string.Empty;
        }

        private void l343_otp_request(byte na, byte data1 = 0x02)
        {
            if (MySerialPort500K == null)
                return;
            MySerialPort500K.SendBreakSyncCmd(PxnConfigurationFrameFormat(na, 0x0A, new[] { data1 }));
        }

        private static byte l343_parity(byte val)
        {
            var par = ((val >> 0) & 1) ^ ((val >> 1) & 1) ^ ((val >> 2) & 1) ^ ((val >> 3) & 1) ^ ((val >> 4) & 1) ^
                      ((val >> 5) & 1) ^ ((val >> 6) & 1);
            par = (par ^ 1) & 1;
            return (byte)par;
        }

        private static byte l343_byte_reverse(byte b)
        {
            b = (byte)((b & 0xF0) >> 4 | (b & 0x0F) << 4);
            b = (byte)((b & 0xCC) >> 2 | (b & 0x33) << 2);
            b = (byte)((b & 0xAA) >> 1 | (b & 0x55) << 1);
            return b;
        }

        /// <summary>
        /// Calculate the OTP CRC
        /// </summary>
        /// <param name="dataBits"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private static byte l343_otp_crc(IReadOnlyList<byte> dataBits, int length)
        {
            byte crc7 = 0;
            for (byte a = 0; a < length; ++a)
            {
                crc7 = (byte)(crc7 ^ dataBits[a]);
                for (byte i = 0; i < 8; ++i)
                {
                    if ((crc7 & 0x80) != 0)
                        crc7 = (byte)(((0x7F & crc7) * 2) ^ (0x37 * 2));
                    else
                        crc7 = (byte)(crc7 * 2);
                }
            }
            return (byte)(crc7 / 2);
        }

        /// <summary>
        /// Calculate the PXN CRC
        /// </summary>
        /// <param name="dataBits"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private static byte l343_pxn_crc(IReadOnlyList<byte> dataBits, int length)
        {
            byte crc8 = 0;
            for (byte a = 0; a < length; ++a)
            {
                crc8 = (byte)(crc8 ^ dataBits[a]);
                for (byte i = 0; i < 8; ++i)
                {
                    if ((crc8 & 0x80) != 0)
                        crc8 = (byte)(((0x7F & crc8) * 2) ^ 0x07);
                    else
                        crc8 = (byte)(crc8 * 2);
                }
            }
            return crc8;
        }

        private static byte[] PxnConfigurationFrameFormat(byte na, byte csid, byte[] data = null)
        {
            var pdata = new List<byte>
            {
                0x55  // sync
            };

            // pid1 
            var pid1 = (byte)(3 << 5 | na);
            var p = l343_parity(pid1);
            pdata.Add((byte)((p << 7) | pid1));

            // pid2 
            p = l343_parity(csid);
            pdata.Add((byte)((p << 7) | csid));

            if (data != null)
            {
                pdata.AddRange(data);

                var pdataCrc = new byte[pdata.Count];
                for (var a = 0; a < pdata.Count; a++)
                    pdataCrc[a] = l343_byte_reverse(pdata[a]);
                pdataCrc[0] = 0xFF;
                var crcResult = l343_pxn_crc(pdataCrc, pdataCrc.Length);

                var returnBytes = new List<byte>();
                for (var i = 1; i < pdata.Count; i++)
                    returnBytes.Add(pdata[i]);
                returnBytes.Add(crcResult);

                return returnBytes.ToArray();
            }
            else
            {
                var returnBytes = new List<byte>();
                for (var i = 1; i < pdata.Count; i++)
                    returnBytes.Add(pdata[i]);

                return returnBytes.ToArray();
            }
        }

        internal enum FrameType : byte
        {
            ReadFrame = 0x00,

            WriteframeToAddressNodeOnly = 0x01,

            WriteFrameToAllNodes = 0x02
        }
    }
}
