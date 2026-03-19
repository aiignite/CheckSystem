using CommonUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace Controller
{
    public sealed class AnalogMax25608 : ControllerBase
    {
        public MySerialPort MySerialPort;

        public AnalogMax25608(string name)
            : base(name)
        {
            MySerialPort.PushSciMsg += MySerialPort_PushSciMsg;

            _keepNetworkThread = new Thread(KeepNetwork) { IsBackground = true };
            _keepNetworkThread.Start();
        }

        ~AnalogMax25608()
        {
            Dispose();
        }

        #region 内部方法
        private readonly Thread _keepNetworkThread;
        private readonly object _lockDevList = new object();
        private int _periodCount;
        private readonly List<Dev> _devList = new List<Dev>();

        private void KeepNetwork()
        {
            while (_keepNetworkThread.IsAlive)
            {
                if (!_keepNetworkThread.IsAlive)
                    break;

                Thread.Sleep(15);
                _periodCount++;
                if (_periodCount > 10000 * 15)
                    _periodCount = 0;

                if (MySerialPort == null)
                    continue;

                lock (_lockDevList)
                {
                    if (!_devList.Any())
                        continue;

                    foreach (var dev in _devList)
                    {
                        //#region sys init
                        //Max25608CmdWrite(0x01, 0x0000, (byte)dev.DevId);
                        //Max25608CmdWrite(0x02, 0x00F1, (byte)dev.DevId);
                        //Max25608CmdWrite(0x04, 0x0924, (byte)dev.DevId);
                        //Max25608CmdWrite(0x05, 0x0924, (byte)dev.DevId);
                        //Max25608CmdWrite(0x06, 0x0924, (byte)dev.DevId);
                        //Thread.Sleep(10);
                        //#endregion

                        #region LED Control
                        Max25608CmdWrite(0x01, 0x0001, (byte)dev.DevId);
                        Thread.Sleep(10);
                        Max25608CmdWrite(0x42, dev.Led1 ? (ushort)0x0FF1 : (ushort)0x0000, (byte)dev.DevId);
                        Max25608CmdWrite(0x43, dev.Led2 ? (ushort)0x0FF1 : (ushort)0x0000, (byte)dev.DevId);
                        Max25608CmdWrite(0x44, dev.Led3 ? (ushort)0x0FF1 : (ushort)0x0000, (byte)dev.DevId);
                        Max25608CmdWrite(0x45, dev.Led4 ? (ushort)0x0FF1 : (ushort)0x0000, (byte)dev.DevId);
                        Max25608CmdWrite(0x46, dev.Led5 ? (ushort)0x0FF1 : (ushort)0x0000, (byte)dev.DevId);
                        Max25608CmdWrite(0x47, dev.Led6 ? (ushort)0x0FF1 : (ushort)0x0000, (byte)dev.DevId);
                        Max25608CmdWrite(0x48, dev.Led7 ? (ushort)0x0FF1 : (ushort)0x0000, (byte)dev.DevId);
                        Max25608CmdWrite(0x49, dev.Led8 ? (ushort)0x0FF1 : (ushort)0x0000, (byte)dev.DevId);
                        Max25608CmdWrite(0x4A, dev.Led9 ? (ushort)0x0FF1 : (ushort)0x0000, (byte)dev.DevId);
                        Max25608CmdWrite(0x4B, dev.Led10 ? (ushort)0x0FF1 : (ushort)0x0000, (byte)dev.DevId);
                        Max25608CmdWrite(0x4C, dev.Led11 ? (ushort)0x0FF1 : (ushort)0x0000, (byte)dev.DevId);
                        Max25608CmdWrite(0x4D, dev.Led12 ? (ushort)0x0FF1 : (ushort)0x0000, (byte)dev.DevId);
                        Thread.Sleep(1);
                        #endregion
                    }

                    //Thread.Sleep(1111);
                }
            }
        }

        private void MySerialPort_PushSciMsg(string name, byte[] datas)
        {
            if (MySerialPort == null || MySerialPort.Name != name)
                return;
            Console.WriteLine("{0}: {1}", name, ValueHelper.GetHextStr(datas));
        }

        internal class Dev
        {
            public ushort DevId { get; set; }
            public bool Led1 { get; set; }
            public bool Led2 { get; set; }
            public bool Led3 { get; set; }
            public bool Led4 { get; set; }

            public bool Led5 { get; set; }
            public bool Led6 { get; set; }
            public bool Led7 { get; set; }
            public bool Led8 { get; set; }

            public bool Led9 { get; set; }
            public bool Led10 { get; set; }
            public bool Led11 { get; set; }
            public bool Led12 { get; set; }

            public Dev(ushort devId)
            {
                DevId = devId;
            }

            public void SwitchLed(int index, bool value)
            {
                if (index == 1)
                    Led1 = value;
                else if (index == 2)
                    Led2 = value;
                else if (index == 3)
                    Led3 = value;
                else if (index == 4)
                    Led4 = value;
                else if (index == 5)
                    Led5 = value;
                else if (index == 6)
                    Led6 = value;
                else if (index == 7)
                    Led7 = value;
                else if (index == 8)
                    Led8 = value;
                else if (index == 9)
                    Led9 = value;
                else if (index == 10)
                    Led10 = value;
                else if (index == 11)
                    Led11 = value;
                else if (index == 12)
                    Led12 = value;
            }
        }

        #endregion

        #region Led Control

        /// <summary>
        /// 初始化地址
        /// We can set from “0000b” to “1111b”
        /// </summary>
        /// <param name="addr"></param>
        [Description("添加一个地址的芯片")]
        public void AddDev(string addr)
        {
            lock (_lockDevList)
            {
                try
                {
                    ushort addrUshortValue;
                    if (ushort.TryParse(addr, out addrUshortValue) && addr.Length != 4)
                    {
                        if (addrUshortValue > 15)
                            return;
                        if (_devList.FindIndex(f => f.DevId == addrUshortValue) != -1)
                            return;
                        //var bits = Convert.ToString(addrUshortValue, 2).PadLeft(8, '0');

                        _devList.Add(new Dev(addrUshortValue));

                        #region sys init
                        Max25608CmdWrite(0x01, 0x0000, (byte)addrUshortValue);
                        Max25608CmdWrite(0x02, 0x00F1, (byte)addrUshortValue);
                        Max25608CmdWrite(0x04, 0x0924, (byte)addrUshortValue);
                        Max25608CmdWrite(0x05, 0x0924, (byte)addrUshortValue);
                        Max25608CmdWrite(0x06, 0x0924, (byte)addrUshortValue);
                        Thread.Sleep(10);
                        #endregion
                    }
                    else if (addr.Length == 4)
                    {
                        var cs0 = addr.Substring(3, 1);
                        var cs1 = addr.Substring(2, 1);
                        var cs2 = addr.Substring(1, 1);
                        var cs3 = addr.Substring(0, 1);

                        var devAddr = Convert.ToUInt16(string.Format("0000{0}{1}{2}{3}", cs3, cs2, cs1, cs0), 2);

                        if (devAddr > 15)
                            return;

                        if (_devList.FindIndex(f => f.DevId == devAddr) == -1)
                            _devList.Add(new Dev(devAddr));

                        #region sys init
                        Max25608CmdWrite(0x01, 0x0000, (byte)devAddr);
                        Max25608CmdWrite(0x02, 0x00F1, (byte)devAddr);
                        Max25608CmdWrite(0x04, 0x0924, (byte)devAddr);
                        Max25608CmdWrite(0x05, 0x0924, (byte)devAddr);
                        Max25608CmdWrite(0x06, 0x0924, (byte)devAddr);
                        Thread.Sleep(10);
                        #endregion
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        [Description("移除一个地址的芯片")]
        public void Remove(string addr)
        {
            lock (_lockDevList)
            {
                try
                {
                    ushort addrUshortValue;
                    if (ushort.TryParse(addr, out addrUshortValue) && addr.Length != 4)
                    {
                        if (addrUshortValue > 15)
                            return;
                        if (_devList.FindIndex(f => f.DevId == addrUshortValue) == -1)
                            return;
                        //var bits = Convert.ToString(addrUshortValue, 2).PadLeft(8, '0');

                        _devList.RemoveAt(_devList.FindIndex(f => f.DevId == addrUshortValue));
                    }
                    else if (addr.Length == 4)
                    {
                        var cs0 = addr.Substring(3, 1);
                        var cs1 = addr.Substring(2, 1);
                        var cs2 = addr.Substring(1, 1);
                        var cs3 = addr.Substring(0, 1);

                        var devAddr = Convert.ToUInt16(string.Format("0000{0}{1}{2}{3}", cs3, cs2, cs1, cs0), 2);

                        if (devAddr > 15)
                            return;
                        if (_devList.FindIndex(f => f.DevId == devAddr) == -1)
                            return;

                        if (_devList.FindIndex(f => f.DevId == devAddr) == -1)
                            _devList.RemoveAt(_devList.FindIndex(f => f.DevId == devAddr));
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        [Description("打开目标芯片的第X个通道")]
        public void TargetDevSingleChOn(string addr, string ledIndex)
        {
            SwitchSignleLed(addr, ledIndex, true);
        }

        [Description("关闭目标芯片的第X个通道")]
        public void TargetDevSingleChOff(string addr, string ledIndex)
        {
            SwitchSignleLed(addr, ledIndex, false);
        }

        [Description("打开目标芯片的单数通道")]
        public void TargetDevOddChOn(string addr)
        {
            for (var i = 1; i < 13; i = i + 2)
                SwitchSignleLed(addr, i.ToString(), true);
        }

        [Description("关闭目标芯片的单数通道")]
        public void TargetDevOddChOff(string addr)
        {
            for (var i = 1; i < 13; i = i + 2)
                SwitchSignleLed(addr, i.ToString(), false);
        }

        [Description("打开目标芯片的双数通道")]
        public void TargetDevEvenChOn(string addr)
        {
            for (var i = 2; i < 13; i = i + 2)
                SwitchSignleLed(addr, i.ToString(), true);
        }

        [Description("关闭目标芯片的双数通道")]
        public void TargetDevEvenChOff(string addr)
        {
            for (var i = 2; i < 13; i = i + 2)
                SwitchSignleLed(addr, i.ToString(), false);
        }

        [Description("打开目标芯片的所有通道")]
        public void TargetDevAllChOn(string addr)
        {
            for (var i = 1; i < 13; i = i + 1)
                SwitchSignleLed(addr, i.ToString(), true);
        }

        [Description("关闭目标芯片的所有通道")]
        public void TargetDevAllChOff(string addr)
        {
            for (var i = 1; i < 13; i = i + 1)
                SwitchSignleLed(addr, i.ToString(), false);
        }

        private void SwitchSignleLed(string addr, string ledIndex, bool isOn)
        {
            lock (_lockDevList)
            {
                try
                {
                    Dev dev = null;
                    ushort addrUshortValue;
                    if (ushort.TryParse(addr, out addrUshortValue) && addr.Length != 4)
                    {
                        if (addrUshortValue > 15)
                            return;
                        if (_devList.FindIndex(f => f.DevId == addrUshortValue) == -1)
                            return;
                        //var bits = Convert.ToString(addrUshortValue, 2).PadLeft(8, '0');

                        dev = _devList[_devList.FindIndex(f => f.DevId == addrUshortValue)];
                    }
                    else if (addr.Length == 4)
                    {
                        var cs0 = addr.Substring(3, 1);
                        var cs1 = addr.Substring(2, 1);
                        var cs2 = addr.Substring(1, 1);
                        var cs3 = addr.Substring(0, 1);

                        var devAddr = Convert.ToUInt16(string.Format("0000{0}{1}{2}{3}", cs3, cs2, cs1, cs0), 2);

                        if (devAddr > 15)
                            return;
                        if (_devList.FindIndex(f => f.DevId == devAddr) == -1)
                            return;
                        dev = _devList[_devList.FindIndex(f => f.DevId == devAddr)];
                    }

                    if (dev == null)
                        return;
                    int index;
                    if (int.TryParse(ledIndex, out index))
                        dev.SwitchLed(index, isOn);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private void Max25608CmdWrite(byte regAddr, ushort value, byte devId)
        {
            if (MySerialPort == null)
                return;

            var writeBuffer = new byte[5];

            writeBuffer[0] = 0x79;

            // When LSB of the DEVICE_ID frame is 0, packet is recognized as a write packet
            writeBuffer[1] = (byte)(((devId & 0x3F) << 1) | 0);
            writeBuffer[2] = regAddr;
            writeBuffer[3] = (byte)((byte)value & 0x00ff);
            writeBuffer[4] = (byte)((value & 0xff00) >> 8);

            // calculate crc0..2 
            byte crc0 = 0;
            byte crc1 = 0;
            byte crc2 = 0;

            var data = writeBuffer[1];
            crc0 ^= (byte)(data & 1);
            crc1 ^= (byte)(data & 1);
            data = (byte)(data >> 1);   // byte1[1]
            crc2 ^= (byte)(data & 1);
            data = (byte)(data >> 1);   // byte1[2]
            crc1 ^= (byte)(data & 1);
            data = (byte)(data >> 1);   // byte1[3]
            crc0 ^= (byte)(data & 1);
            data = (byte)(data >> 1);   // byte1[4]
            crc0 ^= (byte)(data & 1);
            crc2 ^= (byte)(data & 1);
            data = (byte)(data >> 1);   // byte1[5]
            crc0 ^= (byte)(data & 1);
            crc1 ^= (byte)(data & 1);
            crc2 ^= (byte)(data & 1);
            data = (byte)(data >> 1);   // byte1[6]
            crc1 ^= (byte)(data & 1);
            crc2 ^= (byte)(data & 1);
            data = (byte)(data >> 1);   // byte1[7]
            crc0 ^= (byte)(data & 1);
            crc1 ^= (byte)(data & 1);

            data = writeBuffer[2];  // byte2[0]
            crc2 ^= (byte)(data & 1);
            data = (byte)(data >> 1);   // byte2[1]
            crc1 ^= (byte)(data & 1);
            data = (byte)(data >> 1);   // byte2[2]
            crc0 ^= (byte)(data & 1);
            data = (byte)(data >> 1);   // byte2[3]
            crc0 ^= (byte)(data & 1);
            crc2 ^= (byte)(data & 1);
            data = (byte)(data >> 1);   // byte2[4]
            crc0 ^= (byte)(data & 1);
            crc1 ^= (byte)(data & 1);
            crc2 ^= (byte)(data & 1);
            data = (byte)(data >> 1);   // byte2[5]
            crc1 ^= (byte)(data & 1);
            crc2 ^= (byte)(data & 1);
            data = (byte)(data >> 1);   // byte2[6]
            crc0 ^= (byte)(data & 1);
            crc1 ^= (byte)(data & 1);
            data = (byte)(data >> 1);   // byte2[7]
            crc2 ^= (byte)(data & 1);

            data = writeBuffer[3];  // byte3[0]
            crc1 ^= (byte)(data & 1);
            data = (byte)(data >> 1);   // byte3[1]
            crc0 ^= (byte)(data & 1);
            data = (byte)(data >> 1);   // byte3[2]
            crc0 ^= (byte)(data & 1);
            crc2 ^= (byte)(data & 1);
            data = (byte)(data >> 1);   // byte3[3]
            crc0 ^= (byte)(data & 1);
            crc1 ^= (byte)(data & 1);
            crc2 ^= (byte)(data & 1);
            data = (byte)(data >> 1);   // byte3[4]
            crc1 ^= (byte)(data & 1);
            crc2 ^= (byte)(data & 1);
            data = (byte)(data >> 1);   // byte3[5]
            crc0 ^= (byte)(data & 1);
            crc1 ^= (byte)(data & 1);
            data = (byte)(data >> 1);   // byte3[6]
            crc2 ^= (byte)(data & 1);
            data = (byte)(data >> 1);   // byte3[7]
            crc1 ^= (byte)(data & 1);

            data = writeBuffer[4];  // byte4[0]
            crc0 ^= (byte)(data & 1);
            data = (byte)(data >> 1);   // byte4[1]
            crc0 ^= (byte)(data & 1);
            crc2 ^= (byte)(data & 1);
            data = (byte)(data >> 1);   // byte4[2]
            crc0 ^= (byte)(data & 1);
            crc1 ^= (byte)(data & 1);
            crc2 ^= (byte)(data & 1);
            data = (byte)(data >> 1);   // byte4[3]
            crc1 ^= (byte)(data & 1);
            crc2 ^= (byte)(data & 1);
            data = (byte)(data >> 1);   // byte4[4]
            crc0 ^= (byte)(data & 1);
            crc1 ^= (byte)(data & 1);

            byte bcrc = 0;
            bcrc = (byte)((bcrc | crc2) << 1);
            bcrc = (byte)((bcrc | crc1) << 1);
            bcrc = (byte)((bcrc | crc0) << 5);
            writeBuffer[4] = (byte)(writeBuffer[4] | bcrc);

            MySerialPort.SendCommand(writeBuffer);
        }

        #endregion
    }
}
