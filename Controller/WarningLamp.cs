using CommonUtility;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;

namespace Controller
{
    public sealed class WarningLamp : ControllerBase
    {
        private readonly object _locker = new object();
        private MySerialPort _mySerialPort;
        public bool IsInUsing;
        public string GetBarcodeStr = string.Empty;
        public bool LockBarcode;

        public WarningLamp(string name)
            : base(name) { }

        public void ConnectWarningLamp(string portBaudRate)
        {
            try
            {
                if (portBaudRate.StartsWith("COM"))
                {
                    var port = portBaudRate.Split(':')[0];
                    var baudTate = portBaudRate.Split(':')[1];
                    _mySerialPort =
                        new MySerialPort(port, Convert.ToInt32(baudTate), Parity.None, 8, StopBits.One);
                    _mySerialPort.MyOpen();
                }
                else
                {
                    var split = portBaudRate.Split(':');
                    var ipAddressStr = split[0];
                    var port = Convert.ToInt32(split[1]);

                    _mySerialPort = new MySerialPort(ipAddressStr, port);
                }
            }
            catch (Exception ex)
            {
                OnPushControllerMsg(ex.Message);
            }
        }

        /// <summary>
        /// 红灯常亮
        /// </summary>

        public void OnRedLampLong()
        {
            try
            {
                var sendBytes = new List<byte> { 0xAA, (byte)OnType.RedLong };
                _mySerialPort.SendCommand(sendBytes.ToArray());

            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// 黄灯常亮
        /// </summary>

        public void OnYellowLampLong()
        {
            try
            {
                var sendBytes = new List<byte> { 0xAA, (byte)OnType.YellowLong };
                _mySerialPort.SendCommand(sendBytes.ToArray());
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// 绿灯常亮
        /// </summary>

        public void OnGreenLampLong()
        {
            try
            {
                var sendBytes = new List<byte> { 0xAA, (byte)OnType.GreenLong };
                _mySerialPort.SendCommand(sendBytes.ToArray());
            }
            catch (Exception)
            {
                // ignored
            }
        }


        /// <summary>
        /// 蜂鸣器常响
        /// </summary>

        public void OnBuzzerLong()
        {
            try
            {
                var sendBytes = new List<byte> { 0xAA, (byte)OnType.BuzzerLong };
                _mySerialPort.SendCommand(sendBytes.ToArray());
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// 红灯短亮
        /// </summary>

        public void OnRedLampShort()
        {
            try
            {
                var sendBytes = new List<byte> { 0xAA, (byte)OnType.RedShort };
                _mySerialPort.SendCommand(sendBytes.ToArray());
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// 黄灯短亮
        /// </summary>

        public void OnYellowLampShort()
        {
            try
            {
                var sendBytes = new List<byte> { 0xAA, (byte)OnType.YellowShort };
                _mySerialPort.SendCommand(sendBytes.ToArray());
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// 绿灯短亮
        /// </summary>

        public void OnGreenLampShort()
        {
            try
            {
                var sendBytes = new List<byte> { 0xAA, (byte)OnType.GreenShort };
                _mySerialPort.SendCommand(sendBytes.ToArray());
            }
            catch (Exception)
            {
                // ignored
            }
        }


        /// <summary>
        /// 蜂鸣器短响
        /// </summary>

        public void OnBuzzerShort()
        {
            try
            {
                var sendBytes = new List<byte> { 0xAA, (byte)OnType.BuzzerShort };
                _mySerialPort.SendCommand(sendBytes.ToArray());
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// 红灯灭
        /// </summary>
        public void OffRedLamp()
        {
            try
            {
                var sendBytes = new List<byte> { 0xAA, (byte)OffType.Red };
                _mySerialPort.SendCommand(sendBytes.ToArray());
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// 黄灯灭
        /// </summary>
        public void OffYellowLamp()
        {
            try
            {
                var sendBytes = new List<byte> { 0xAA, (byte)OffType.Yellow };
                _mySerialPort.SendCommand(sendBytes.ToArray());
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// 绿灯灭
        /// </summary>
        public void OffGreenLamp()
        {
            try
            {
                var sendBytes = new List<byte> { 0xAA, (byte)OffType.Green };
                _mySerialPort.SendCommand(sendBytes.ToArray());
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// 所有灯灭
        /// </summary>
        public void OffAllLamps()
        {
            try
            {
                var sendBytes = new List<byte> { 0xAA, (byte)OffType.All };
                _mySerialPort.SendCommand(sendBytes.ToArray());
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// 蜂鸣器关闭
        /// </summary>
        public void OffBuzzer()
        {
            try
            {
                var sendBytes = new List<byte> { 0xAA, (byte)OffType.Buzzer };
                _mySerialPort.SendCommand(sendBytes.ToArray());
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public enum OnType : byte
        {
            /// <summary>
            /// AA 11
            /// 红灯常亮
            /// </summary>
            RedLong = 0x11,

            /// <summary>
            /// AA 12
            /// 黄灯常亮
            /// </summary>
            YellowLong = 0x12,

            /// <summary>
            /// AA 14
            /// 绿灯常亮
            /// </summary>
            GreenLong = 0x14,

            /// <summary>
            /// AA 18
            /// 蜂鸣器常响
            /// </summary>
            BuzzerLong = 0x18,

            /// <summary>
            /// AA 41
            /// 红灯闪亮
            /// </summary>
            RedShort = 0x41,

            /// <summary>
            /// AA 42
            /// 黄灯闪亮
            /// </summary>
            YellowShort = 0x42,

            /// <summary>
            /// AA 44
            /// 绿灯闪亮
            /// </summary>
            GreenShort = 0x44,

            /// <summary>
            /// AA 48
            /// 蜂鸣器短响
            /// </summary>
            BuzzerShort = 0x48,
        }

        public enum OffType : byte
        {
            /// <summary>
            /// AA 21
            /// 红灯灭
            /// </summary>
            Red = 0x21,

            /// <summary>
            /// AA 22
            /// 黄灯灭
            /// </summary>
            Yellow = 0x22,

            /// <summary>
            /// AA 24
            /// 绿灯灭
            /// </summary>
            Green = 0x24,

            /// <summary>
            /// AA 28
            /// 蜂鸣器灭
            /// </summary>
            Buzzer = 0x28,

            /// <summary>
            /// AA 00
            /// 所有灯灭
            /// </summary>
            All = 0x00,
        }

        public void GetBarcode(string keyAndKeyindexAndLen)
        {
            lock (_locker)
            {
                GetBarcodeStr = string.Empty;

                try
                {
                    var sp = keyAndKeyindexAndLen.Split(':');
                    var key = sp[0];
                    var keyIndex = Convert.ToInt32(sp[1]);
                    var len = Convert.ToInt32(sp[2]);

                    var temp = ReadBuffer();
                    //"000ABCP001EFG000";
                    if (string.IsNullOrEmpty(temp))
                        return;

                    var findKeyIndex = temp.LastIndexOf(key, StringComparison.Ordinal);
                    if (findKeyIndex == -1)
                        return;
                    GetBarcodeStr = temp.Substring(findKeyIndex - keyIndex, len);
                }
                catch (Exception exception)
                {
                    // ignored
                    GetBarcodeStr = exception.Message;
                }
            }
        }

        public void ReadBarcode(int length)
        {
            lock (_locker)
            {
                GetBarcodeStr = string.Empty;
                if (_mySerialPort == null)
                    return;
                _mySerialPort.ClearBuff();

                var tempList = new List<string>();

                var waitCount = 0;

                while (true)
                {
                    if (tempList.Count == length)
                        break;

                    if (waitCount * 1000 > 60 * 1000)
                        break;

                    Thread.Sleep(1);
                    waitCount++;

                    var temp = ReadBuffer();
                    if (string.IsNullOrEmpty(temp))
                        continue;
                    tempList.Add(temp);
                }

                foreach (var str in tempList)
                    GetBarcodeStr += str;
            }
        }

        private string ReadBuffer()
        {
            return _mySerialPort.ReadDataStr();
        }

        public void LockBarcodeScanReader()
        {
            LockBarcode = true;
        }

        public void UnlockBarcodeScanReader()
        {
            LockBarcode = false;
            GetBarcodeStr = string.Empty;
        }
    }
}
