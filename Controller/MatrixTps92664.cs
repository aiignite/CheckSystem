using CommonUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace Controller
{
    public sealed class MatrixTps92664 : ControllerBase
    {
        public MySerialPort MySerialPort;

        public MatrixTps92664(string name)
            : base(name)
        {
            MySerialPort.PushSciMsg += MySerialPort_PushSciMsg;

            MainThread = new Thread(MainWork) { IsBackground = true };
            MainThread.Start();
        }

        ~MatrixTps92664()
        {
            Dispose();
        }

        private void MainWork()
        {
            while (MainThread.IsAlive)
            {
                Thread.Sleep(15);

                if (!MainThread.IsAlive)
                    break;

                if (!_isSycConfiged)
                {
                    _sendCount = 0;
                    continue;
                }

                try
                {
                    if (MySerialPort == null)
                        continue;

                    lock (_lockDevList)
                    {
                        foreach (var dev in _lstMatrix)
                        {
                            var sendPhaseH = new List<byte>();
                            var sendPhaseL = new List<byte>();

                            var sendWitdhH = new List<byte>();
                            var sendWitdhL = new List<byte>();

                            var phase04To01LBits = new[] { "0", "0", "0", "0", "0", "0", "0", "0" };
                            var phase08To05LBits = new[] { "0", "0", "0", "0", "0", "0", "0", "0" };
                            var phase12To09LBits = new[] { "0", "0", "0", "0", "0", "0", "0", "0" };
                            var phase16To13LBits = new[] { "0", "0", "0", "0", "0", "0", "0", "0" };

                            var width04To01LBits = new[] { "0", "0", "0", "0", "0", "0", "0", "0" };
                            var width08To05LBits = new[] { "0", "0", "0", "0", "0", "0", "0", "0" };
                            var width12To09LBits = new[] { "0", "0", "0", "0", "0", "0", "0", "0" };
                            var width16To13LBits = new[] { "0", "0", "0", "0", "0", "0", "0", "0" };

                            for (var i = 0; i < 16; i++)
                            {
                                var widthFieldName = string.Format("Led{0}Width", i + 1);
                                var widthField = dev.GetType().GetProperty(widthFieldName);
                                var widthValue = (ushort)0;
                                if (widthField != null)
                                    widthValue = (ushort)widthField.GetValue(dev);

                                var phaseFieldName = string.Format("Led{0}Phase", i + 1);
                                var phaseField = dev.GetType().GetProperty(phaseFieldName);
                                var phaseValue = (ushort)0;
                                if (phaseField != null)
                                    phaseValue = (ushort)phaseField.GetValue(dev);

                                var ledPhaseBits = Convert.ToString(phaseValue, 2).PadLeft(10, '0');
                                var ledWidthBits = Convert.ToString(widthValue, 2).PadLeft(10, '0');

                                //phaseBits.Add(ledPhaseBits);
                                //widthBits.Add(ledWidthBits);

                                sendPhaseH.Add(Convert.ToByte(ledPhaseBits.Substring(0, 8), 2));
                                sendWitdhH.Add(Convert.ToByte(ledWidthBits.Substring(0, 8), 2));

                                switch (i)
                                {
                                    case 0:
                                        phase04To01LBits[6] = ledPhaseBits.Substring(8, 1);
                                        phase04To01LBits[7] = ledPhaseBits.Substring(9, 1);

                                        width04To01LBits[6] = ledWidthBits.Substring(8, 1);
                                        width04To01LBits[7] = ledWidthBits.Substring(9, 1);
                                        break;

                                    case 1:
                                        phase04To01LBits[4] = ledPhaseBits.Substring(8, 1);
                                        phase04To01LBits[5] = ledPhaseBits.Substring(9, 1);

                                        width04To01LBits[4] = ledWidthBits.Substring(8, 1);
                                        width04To01LBits[5] = ledWidthBits.Substring(9, 1);
                                        break;

                                    case 2:
                                        phase04To01LBits[2] = ledPhaseBits.Substring(8, 1);
                                        phase04To01LBits[3] = ledPhaseBits.Substring(9, 1);

                                        width04To01LBits[2] = ledWidthBits.Substring(8, 1);
                                        width04To01LBits[3] = ledWidthBits.Substring(9, 1);
                                        break;

                                    case 3:
                                        phase04To01LBits[0] = ledPhaseBits.Substring(8, 1);
                                        phase04To01LBits[1] = ledPhaseBits.Substring(9, 1);

                                        width04To01LBits[0] = ledWidthBits.Substring(8, 1);
                                        width04To01LBits[1] = ledWidthBits.Substring(9, 1);
                                        break;

                                    case 4:
                                        phase08To05LBits[6] = ledPhaseBits.Substring(8, 1);
                                        phase08To05LBits[7] = ledPhaseBits.Substring(9, 1);

                                        width08To05LBits[6] = ledWidthBits.Substring(8, 1);
                                        width08To05LBits[7] = ledWidthBits.Substring(9, 1);
                                        break;

                                    case 5:
                                        phase08To05LBits[4] = ledPhaseBits.Substring(8, 1);
                                        phase08To05LBits[5] = ledPhaseBits.Substring(9, 1);

                                        width08To05LBits[4] = ledWidthBits.Substring(8, 1);
                                        width08To05LBits[5] = ledWidthBits.Substring(9, 1);
                                        break;

                                    case 6:
                                        phase08To05LBits[2] = ledPhaseBits.Substring(8, 1);
                                        phase08To05LBits[3] = ledPhaseBits.Substring(9, 1);

                                        width08To05LBits[2] = ledWidthBits.Substring(8, 1);
                                        width08To05LBits[3] = ledWidthBits.Substring(9, 1);
                                        break;

                                    case 7:
                                        phase08To05LBits[0] = ledPhaseBits.Substring(8, 1);
                                        phase08To05LBits[1] = ledPhaseBits.Substring(9, 1);

                                        width08To05LBits[0] = ledWidthBits.Substring(8, 1);
                                        width08To05LBits[1] = ledWidthBits.Substring(9, 1);
                                        break;

                                    case 8:
                                        phase12To09LBits[6] = ledPhaseBits.Substring(8, 1);
                                        phase12To09LBits[7] = ledPhaseBits.Substring(9, 1);

                                        width12To09LBits[6] = ledWidthBits.Substring(8, 1);
                                        width12To09LBits[7] = ledWidthBits.Substring(9, 1);
                                        break;

                                    case 9:
                                        phase12To09LBits[4] = ledPhaseBits.Substring(8, 1);
                                        phase12To09LBits[5] = ledPhaseBits.Substring(9, 1);

                                        width12To09LBits[4] = ledWidthBits.Substring(8, 1);
                                        width12To09LBits[5] = ledWidthBits.Substring(9, 1);
                                        break;

                                    case 10:
                                        phase12To09LBits[2] = ledPhaseBits.Substring(8, 1);
                                        phase12To09LBits[3] = ledPhaseBits.Substring(9, 1);

                                        width12To09LBits[2] = ledWidthBits.Substring(8, 1);
                                        width12To09LBits[3] = ledWidthBits.Substring(9, 1);
                                        break;

                                    case 11:
                                        phase12To09LBits[0] = ledPhaseBits.Substring(8, 1);
                                        phase12To09LBits[1] = ledPhaseBits.Substring(9, 1);

                                        width12To09LBits[0] = ledWidthBits.Substring(8, 1);
                                        width12To09LBits[1] = ledWidthBits.Substring(9, 1);
                                        break;

                                    case 12:
                                        phase16To13LBits[6] = ledPhaseBits.Substring(8, 1);
                                        phase16To13LBits[7] = ledPhaseBits.Substring(9, 1);

                                        width16To13LBits[6] = ledWidthBits.Substring(8, 1);
                                        width16To13LBits[7] = ledWidthBits.Substring(9, 1);
                                        break;

                                    case 13:
                                        phase16To13LBits[4] = ledPhaseBits.Substring(8, 1);
                                        phase16To13LBits[5] = ledPhaseBits.Substring(9, 1);

                                        width16To13LBits[4] = ledWidthBits.Substring(8, 1);
                                        width16To13LBits[5] = ledWidthBits.Substring(9, 1);
                                        break;

                                    case 14:
                                        phase16To13LBits[2] = ledPhaseBits.Substring(8, 1);
                                        phase16To13LBits[3] = ledPhaseBits.Substring(9, 1);

                                        width16To13LBits[2] = ledWidthBits.Substring(8, 1);
                                        width16To13LBits[3] = ledWidthBits.Substring(9, 1);
                                        break;

                                    case 15:
                                        phase16To13LBits[0] = ledPhaseBits.Substring(8, 1);
                                        phase16To13LBits[1] = ledPhaseBits.Substring(9, 1);

                                        width16To13LBits[0] = ledWidthBits.Substring(8, 1);
                                        width16To13LBits[1] = ledWidthBits.Substring(9, 1);
                                        break;
                                }
                            }

                            var p1 = string.Empty;
                            var p2 = string.Empty;
                            var p3 = string.Empty;
                            var p4 = string.Empty;
                            p1 = phase04To01LBits.Aggregate(p1, (current, t) => current + t);
                            p2 = phase08To05LBits.Aggregate(p2, (current, t) => current + t);
                            p3 = phase12To09LBits.Aggregate(p3, (current, t) => current + t);
                            p4 = phase16To13LBits.Aggregate(p4, (current, t) => current + t);

                            sendPhaseL.Add(Convert.ToByte(p1, 2));
                            sendPhaseL.Add(Convert.ToByte(p2, 2));
                            sendPhaseL.Add(Convert.ToByte(p3, 2));
                            sendPhaseL.Add(Convert.ToByte(p4, 2));

                            var w1 = string.Empty;
                            var w2 = string.Empty;
                            var w3 = string.Empty;
                            var w4 = string.Empty;
                            w1 = width04To01LBits.Aggregate(w1, (current, t) => current + t);
                            w2 = width08To05LBits.Aggregate(w2, (current, t) => current + t);
                            w3 = width12To09LBits.Aggregate(w3, (current, t) => current + t);
                            w4 = width16To13LBits.Aggregate(w4, (current, t) => current + t);

                            sendWitdhL.Add(Convert.ToByte(w1, 2));
                            sendWitdhL.Add(Convert.ToByte(w2, 2));
                            sendWitdhL.Add(Convert.ToByte(w3, 2));
                            sendWitdhL.Add(Convert.ToByte(w4, 2));

                            var pDatas = new List<byte>();
                            pDatas.AddRange(sendPhaseH);
                            pDatas.AddRange(sendPhaseL);
                            if (_sendCount % 2 == 0)
                                MySerialPort.SendCommand(SendWrtieData(dev.DevId, 0x15, pDatas), 5);

                            var wDatas = new List<byte>();
                            wDatas.AddRange(sendWitdhH);
                            wDatas.AddRange(sendWitdhL);
                            if (_sendCount % 2 != 0)
                                MySerialPort.SendCommand(SendWrtieData(dev.DevId, 0x29, wDatas), 5);
                        }
                    }

                    _sendCount++;
                    if (_sendCount < 25)
                        continue;

                    SysConfig();
                    _sendCount = 0;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private Thread MainThread { get; }
        private readonly object _lockDevList = new object();
        private bool _isSycConfiged;
        private int _sendCount;

        private readonly List<DeviceIdUpto8Tps92662DevicesOnOneBus> _lstMatrix =
            new List<DeviceIdUpto8Tps92662DevicesOnOneBus>();

        private void MySerialPort_PushSciMsg(string name, byte[] datas)
        {
            if (MySerialPort == null || MySerialPort.Name != name)
                return;

            var str = datas.Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
            //Console.WriteLine("TPS92664/TPS92665:" + str);

            //if (str.StartsWith("87") || str.StartsWith("AA") || str.StartsWith("33") || str.StartsWith("B5") || str.StartsWith("1E"))
            //{
            //    Console.WriteLine("TPS92664:" + str);
            //}

            //if (_isReadIcid)
            //{
            //    if (str.StartsWith("4B") && str.Length >= 16)
            //    {
            //        Icid = string.Format("{0}{1}", str[10], str[11]);
            //        _waitHandle.Set();
            //        _isReadIcid = true;
            //    }
            //}

            if (_isReadAdc8Bits)
                _readAdc8BitsBuff = ValueHelper.GetHextStr(datas).Replace(" ", "");

            if (_isReadAdc10Bits)
                _readAdc10BitsBuff = ValueHelper.GetHextStr(datas).Replace(" ", "");

            if (_isReadIcid)
                _readIcidBuff = ValueHelper.GetHextStr(datas).Replace(" ", "");
        }

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
                    if (ushort.TryParse(addr, out addrUshortValue) && addr.Length != 3)
                    {
                        if (addrUshortValue > 7)
                            return;
                        if (_lstMatrix.FindIndex(f => f.AddressByte == addrUshortValue) != -1)
                            return;
                        var bits = Convert.ToString(addrUshortValue, 2).PadLeft(8, '0');

                        _lstMatrix.Add(new DeviceIdUpto8Tps92662DevicesOnOneBus(
                            bits[7].ToString(), bits[6].ToString(), bits[5].ToString(), false));
                    }
                    else if (addr.Length == 3)
                    {
                        var cs0 = addr.Substring(2, 1);
                        var cs1 = addr.Substring(1, 1);
                        var cs2 = addr.Substring(0, 1);

                        var devAddr = Convert.ToUInt16(string.Format("00000{0}{1}{2}", cs2, cs1, cs0), 2);

                        if (devAddr > 7)
                            return;

                        if (_lstMatrix.FindIndex(f => f.AddressByte == devAddr) == -1)
                            _lstMatrix.Add(new DeviceIdUpto8Tps92662DevicesOnOneBus(cs0, cs1, cs2, false));
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
                    if (ushort.TryParse(addr, out addrUshortValue) && addr.Length != 3)
                    {
                        if (addrUshortValue > 7)
                            return;
                        if (_lstMatrix.FindIndex(f => f.AddressByte == addrUshortValue) == -1)
                            return;
                        //var bits = Convert.ToString(addrUshortValue, 2).PadLeft(8, '0');

                        _lstMatrix.RemoveAt(_lstMatrix.FindIndex(f => f.AddressByte == addrUshortValue));
                    }
                    else if (addr.Length == 3)
                    {
                        var cs0 = addr.Substring(2, 1);
                        var cs1 = addr.Substring(1, 1);
                        var cs2 = addr.Substring(0, 1);

                        var devAddr = Convert.ToUInt16(string.Format("0000{0}{1}{2}", cs2, cs1, cs0), 2);

                        if (devAddr > 7)
                            return;
                        if (_lstMatrix.FindIndex(f => f.AddressByte == devAddr) == -1)
                            return;

                        if (_lstMatrix.FindIndex(f => f.AddressByte == devAddr) == -1)
                            _lstMatrix.RemoveAt(_lstMatrix.FindIndex(f => f.AddressByte == devAddr));
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        [Description("发送初始化命令")]
        public void SysConfig()
        {
            _isSycConfiged = false;

            if (MySerialPort == null)
                return;

            //for (var i = 1; i < _lstMatrix.Count + 1; i++)
            {
                MySerialPort.SendCommand(SendWrtieData(0xBF, 0x80, new byte[] { 0x12 }), 5);
                Thread.Sleep(5);

                //MySerialPort.SendCommand(SendWrtieData(0xBF, 0x01, new byte[] { 0x00 }));
                //Thread.Sleep(5);

                //MySerialPort.SendCommand(SendWrtieData(0xBF, 0x03, new byte[] { 0x00, 0x00 }));
                //Thread.Sleep(5);

                //MySerialPort.SendCommand(SendWrtieData(0xBF, 0x82, new byte[] { 0x24 }));
                //Thread.Sleep(5);

                //MySerialPort.SendCommand(SendWrtieData(0xBF, 0x8C, new byte[] { 0xAA }));
                //Thread.Sleep(5);

                MySerialPort.SendCommand(SendWrtieData(0xBF, 0x00, new byte[] { 0x20 }), 5);
                Thread.Sleep(5);
            }

            _isSycConfiged = true;
        }

        [Description("停止报文")]
        public void SysNotActive()
        {
            _isSycConfiged = false;
        }

        [Description("打开目标地址的所有LED")]
        public void Allon(string addr)
        {
            SwitchSignleLed(addr, 1.ToString());
            SwitchSignleLed(addr, 2.ToString());
            SwitchSignleLed(addr, 3.ToString());
            SwitchSignleLed(addr, 4.ToString());
            SwitchSignleLed(addr, 5.ToString());
            SwitchSignleLed(addr, 6.ToString());
            SwitchSignleLed(addr, 7.ToString());
            SwitchSignleLed(addr, 8.ToString());
            SwitchSignleLed(addr, 9.ToString());
            SwitchSignleLed(addr, 10.ToString());
            SwitchSignleLed(addr, 11.ToString());
            SwitchSignleLed(addr, 12.ToString());
            SwitchSignleLed(addr, 13.ToString());
            SwitchSignleLed(addr, 14.ToString());
            SwitchSignleLed(addr, 15.ToString());
            SwitchSignleLed(addr, 16.ToString());
        }

        [Description("关闭目标地址的所有LED")]
        public void Alloff(string addr)
        {
            SwitchSignleLed(addr, 1.ToString(), widthVal: 0);
            SwitchSignleLed(addr, 2.ToString(), widthVal: 0);
            SwitchSignleLed(addr, 3.ToString(), widthVal: 0);
            SwitchSignleLed(addr, 4.ToString(), widthVal: 0);
            SwitchSignleLed(addr, 5.ToString(), widthVal: 0);
            SwitchSignleLed(addr, 6.ToString(), widthVal: 0);
            SwitchSignleLed(addr, 7.ToString(), widthVal: 0);
            SwitchSignleLed(addr, 8.ToString(), widthVal: 0);
            SwitchSignleLed(addr, 9.ToString(), widthVal: 0);
            SwitchSignleLed(addr, 10.ToString(), widthVal: 0);
            SwitchSignleLed(addr, 11.ToString(), widthVal: 0);
            SwitchSignleLed(addr, 12.ToString(), widthVal: 0);
            SwitchSignleLed(addr, 13.ToString(), widthVal: 0);
            SwitchSignleLed(addr, 14.ToString(), widthVal: 0);
            SwitchSignleLed(addr, 15.ToString(), widthVal: 0);
            SwitchSignleLed(addr, 16.ToString(), widthVal: 0);
        }

        [Description("打开目标芯片的第X个通道")]
        public void TargetDevSingleChOn(string addr, string ledIndex)
        {
            SwitchSignleLed(addr, ledIndex);
        }

        [Description("关闭目标芯片的第X个通道")]
        public void TargetDevSingleChOff(string addr, string ledIndex)
        {
            SwitchSignleLed(addr, ledIndex, widthVal: 0);
        }

        [Description("打开目标芯片的单数通道")]
        public void TargetDevOddChOn(string addr)
        {
            for (var i = 1; i < 17; i = i + 2)
                SwitchSignleLed(addr, i.ToString());
        }

        [Description("关闭目标芯片的单数通道")]
        public void TargetDevOddChOff(string addr)
        {
            for (var i = 1; i < 17; i = i + 2)
                SwitchSignleLed(addr, i.ToString(), widthVal: 0);
        }

        [Description("打开目标芯片的双数通道")]
        public void TargetDevEvenChOn(string addr)
        {
            for (var i = 2; i < 17; i = i + 2)
                SwitchSignleLed(addr, i.ToString());
        }

        [Description("关闭目标芯片的双数通道")]
        public void TargetDevEvenChOff(string addr)
        {
            for (var i = 2; i < 17; i = i + 2)
                SwitchSignleLed(addr, i.ToString(), widthVal: 0);
        }

        [Description("打开目标芯片的所有通道")]
        public void TargetDevAllChOn(string addr)
        {
            for (var i = 1; i < 17; i = i + 1)
                SwitchSignleLed(addr, i.ToString());
        }

        [Description("关闭目标芯片的所有通道")]
        public void TargetDevAllChOff(string addr)
        {
            for (var i = 1; i < 17; i = i + 1)
                SwitchSignleLed(addr, i.ToString(), widthVal: 0);
        }

        [Description("设置目标芯片的目标通道的width")]
        public void TargetDevSingleChWidth(string addr, string ledIndex, int width) => SwichSignleLedWidth(addr, ledIndex, (ushort)width);

        [Description("设置目标芯片的目标通道的phase")]
        public void TargetDevSingleChPhase(string addr, string ledIndex, int phase) => SwichSignleLedPhase(addr, ledIndex, (ushort)phase);

        private void SwitchSignleLed(
            string addr, string ledIndex, ushort phaseVal = 0, ushort widthVal = 1023)
        {
            lock (_lockDevList)
            {
                try
                {
                    DeviceIdUpto8Tps92662DevicesOnOneBus dev = null;
                    ushort addrUshortValue;
                    if (ushort.TryParse(addr, out addrUshortValue) && addr.Length != 3)
                    {
                        if (addrUshortValue > 7)
                            return;
                        if (_lstMatrix.FindIndex(f => f.AddressByte == addrUshortValue) == -1)
                            return;
                        //var bits = Convert.ToString(addrUshortValue, 2).PadLeft(8, '0');

                        dev = _lstMatrix[_lstMatrix.FindIndex(f => f.AddressByte == addrUshortValue)];
                    }
                    else if (addr.Length == 4)
                    {
                        var cs0 = addr.Substring(2, 1);
                        var cs1 = addr.Substring(1, 1);
                        var cs2 = addr.Substring(0, 1);

                        var devAddr = Convert.ToUInt16(string.Format("0000{0}{1}{2}", cs2, cs1, cs0), 2);

                        if (devAddr > 15)
                            return;
                        if (_lstMatrix.FindIndex(f => f.AddressByte == devAddr) == -1)
                            return;
                        dev = _lstMatrix[_lstMatrix.FindIndex(f => f.AddressByte == devAddr)];
                    }

                    if (dev == null)
                        return;
                    int index;
                    if (int.TryParse(ledIndex, out index))
                        dev.SetLed(index, widthVal, phaseVal);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private void SwichSignleLedWidth(string addr, string ledIndex, ushort widthVal)
        {
            lock (_lockDevList)
            {
                try
                {
                    DeviceIdUpto8Tps92662DevicesOnOneBus dev = null;
                    ushort addrUshortValue;
                    if (ushort.TryParse(addr, out addrUshortValue) && addr.Length != 3)
                    {
                        if (addrUshortValue > 7)
                            return;
                        if (_lstMatrix.FindIndex(f => f.AddressByte == addrUshortValue) == -1)
                            return;
                        //var bits = Convert.ToString(addrUshortValue, 2).PadLeft(8, '0');

                        dev = _lstMatrix[_lstMatrix.FindIndex(f => f.AddressByte == addrUshortValue)];
                    }
                    else if (addr.Length == 4)
                    {
                        var cs0 = addr.Substring(2, 1);
                        var cs1 = addr.Substring(1, 1);
                        var cs2 = addr.Substring(0, 1);

                        var devAddr = Convert.ToUInt16(string.Format("0000{0}{1}{2}", cs2, cs1, cs0), 2);

                        if (devAddr > 15)
                            return;
                        if (_lstMatrix.FindIndex(f => f.AddressByte == devAddr) == -1)
                            return;
                        dev = _lstMatrix[_lstMatrix.FindIndex(f => f.AddressByte == devAddr)];
                    }

                    if (dev == null)
                        return;
                    int index;
                    if (int.TryParse(ledIndex, out index))
                        dev.SetWidth(index, widthVal);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private void SwichSignleLedPhase(string addr, string ledIndex, ushort phaseVal)
        {
            lock (_lockDevList)
            {
                try
                {
                    DeviceIdUpto8Tps92662DevicesOnOneBus dev = null;
                    ushort addrUshortValue;
                    if (ushort.TryParse(addr, out addrUshortValue) && addr.Length != 3)
                    {
                        if (addrUshortValue > 7)
                            return;
                        if (_lstMatrix.FindIndex(f => f.AddressByte == addrUshortValue) == -1)
                            return;
                        //var bits = Convert.ToString(addrUshortValue, 2).PadLeft(8, '0');

                        dev = _lstMatrix[_lstMatrix.FindIndex(f => f.AddressByte == addrUshortValue)];
                    }
                    else if (addr.Length == 4)
                    {
                        var cs0 = addr.Substring(2, 1);
                        var cs1 = addr.Substring(1, 1);
                        var cs2 = addr.Substring(0, 1);

                        var devAddr = Convert.ToUInt16(string.Format("0000{0}{1}{2}", cs2, cs1, cs0), 2);

                        if (devAddr > 15)
                            return;
                        if (_lstMatrix.FindIndex(f => f.AddressByte == devAddr) == -1)
                            return;
                        dev = _lstMatrix[_lstMatrix.FindIndex(f => f.AddressByte == devAddr)];
                    }

                    if (dev == null)
                        return;
                    int index;
                    if (int.TryParse(ledIndex, out index))
                        dev.SetPhase(index, phaseVal);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private class DeviceIdUpto8Tps92662DevicesOnOneBus
        {
            public byte AddressByte { get; private set; }
            public byte DevId { get; private set; }
            public string DevIdBitD4 { get; private set; }
            public string DevIdBitD3 { get; private set; }
            public string DevIdBitD2 { get; private set; }
            public string DevIdBitD1 { get; private set; }
            public string DevIdBitD0 { get; private set; }

            public ushort Led1Width { get; private set; }
            public ushort Led2Width { get; private set; }
            public ushort Led3Width { get; private set; }
            public ushort Led4Width { get; private set; }
            public ushort Led5Width { get; private set; }
            public ushort Led6Width { get; private set; }
            public ushort Led7Width { get; private set; }
            public ushort Led8Width { get; private set; }
            public ushort Led9Width { get; private set; }
            public ushort Led10Width { get; private set; }
            public ushort Led11Width { get; private set; }
            public ushort Led12Width { get; private set; }
            public ushort Led13Width { get; private set; }
            public ushort Led14Width { get; private set; }
            public ushort Led15Width { get; private set; }
            public ushort Led16Width { get; private set; }

            public ushort Led1Phase { get; private set; }
            public ushort Led2Phase { get; private set; }
            public ushort Led3Phase { get; private set; }
            public ushort Led4Phase { get; private set; }
            public ushort Led5Phase { get; private set; }
            public ushort Led6Phase { get; private set; }
            public ushort Led7Phase { get; private set; }
            public ushort Led8Phase { get; private set; }
            public ushort Led9Phase { get; private set; }
            public ushort Led10Phase { get; private set; }
            public ushort Led11Phase { get; private set; }
            public ushort Led12Phase { get; private set; }
            public ushort Led13Phase { get; private set; }
            public ushort Led14Phase { get; private set; }
            public ushort Led15Phase { get; private set; }
            public ushort Led16Phase { get; private set; }

            public DeviceIdUpto8Tps92662DevicesOnOneBus(
                string addr0, string addr1, string addr2, bool broadcast)
            {
                if (broadcast)
                {
                    DevIdBitD0 = 0.ToString();
                    DevIdBitD1 = 0.ToString();
                    DevIdBitD2 = 0.ToString();
                    DevIdBitD3 = 0.ToString();
                    DevIdBitD4 = 0.ToString();
                    DevId = 0xBF;

                    AddressByte = 0xBF;
                }
                else
                {
                    DevIdBitD0 = addr0;
                    DevIdBitD1 = addr1;
                    DevIdBitD2 = addr2;
                    DevIdBitD3 = 0.ToString();
                    DevIdBitD4 = 0.ToString();

                    var p1 =
                        byte.Parse(DevIdBitD1) ^ byte.Parse(DevIdBitD3) ^ byte.Parse(DevIdBitD4);
                    var p0 = byte.Parse(DevIdBitD0) ^ byte.Parse(DevIdBitD1) ^
                             byte.Parse(DevIdBitD2) ^ byte.Parse(DevIdBitD4);

                    var str = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", p1, p0, 1, DevIdBitD4,
                        DevIdBitD3, DevIdBitD2, DevIdBitD1, DevIdBitD0);
                    DevId = Convert.ToByte(str, 2);

                    AddressByte = Convert.ToByte(string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", "0", "0", "0", "0",
                        "0", addr2, addr1, addr0), 2);
                }
            }

            public void SetLed(int ledIndex, ushort width, ushort phase = 0)
            {
                var widthFieldName = string.Format("Led{0}Width", ledIndex);
                var widthField = GetType().GetProperty(widthFieldName);
                if (widthField != null)
                    widthField.SetValue(this, width);

                var phaseFieldName = string.Format("Led{0}Phase", ledIndex);
                var phaseField = GetType().GetProperty(phaseFieldName);
                if (phaseField != null)
                    phaseField.SetValue(this, phase);
            }

            public void SetPhase(int ledIndex, ushort phase)
            {
                var phaseFieldName = string.Format("Led{0}Phase", ledIndex);
                var phaseField = GetType().GetProperty(phaseFieldName);
                if (phaseField != null)
                    phaseField.SetValue(this, phase);
            }

            public void SetWidth(int ledIndex, ushort width)
            {
                var widthFieldName = string.Format("Led{0}Width", ledIndex);
                var widthField = GetType().GetProperty(widthFieldName);
                if (widthField != null)
                    widthField.SetValue(this, width);
            }
        }

        #endregion

        #region 读寄存器

        [Description("R,读ADC1_8Bits")]
        public double Adc18Bits = -9999;
        [Description("R,读ADC2_8Bits")]
        public double Adc28Bits = -9999;
        [Description("R,读ADC1_10Bits")]
        public double Adc110Bits = -9999;
        [Description("R,读ADC2_10Bits")]
        public double Adc210Bits = -9999;

        [Description("R,读ADC1_8Bits转电阻值")]
        public double Adc18BitsToResValue = -9999;
        [Description("R,读ADC2_8Bits转电阻值")]
        public double Adc28BitsToResValue = -9999;
        [Description("R,读ADC1_10Bits转电阻值")]
        public double Adc110BitsToResValue = -9999;
        [Description("R,读ADC2_10Bits转电阻值")]
        public double Adc210BitsToResValue = -9999;

        [Description("R,ICID")]
        public string Icid = string.Empty;

        public bool _isReadIcid;
        public string _readIcidBuff = string.Empty;

        private bool _isReadAdc8Bits;
        private bool _isReadAdc10Bits;

        private string _readAdc8BitsBuff = string.Empty;
        private string _readAdc10BitsBuff = string.Empty;

        [Description("读ADC_8Bits")]
        public void ReadAdc_8bits(string addr)
        {
            Adc18Bits = -9999;
            Adc28Bits = -9999;

            Adc18BitsToResValue = -9999;
            Adc28BitsToResValue = -9999;

            _readAdc8BitsBuff = string.Empty;

            int slaveAddr;
            if (int.TryParse(addr, out slaveAddr))
            {
                if (slaveAddr >= 0 && slaveAddr <= 7)
                {
                    if (MySerialPort != null)
                    {
                        var bits = Convert.ToString(slaveAddr, 2).PadLeft(8, '0');

                        var dev = new DeviceIdUpto8Tps92662DevicesOnOneBus(
                            bits[7].ToString(), bits[6].ToString(), bits[5].ToString(), false);

                        _isReadAdc8Bits = true;
                        var sendBytes = SendReadData(dev.DevId, 0x8D, 2);
                        var sendStr = ValueHelper.GetHextStr(sendBytes).Replace(" ", "");
                        MySerialPort.SendCommand(sendBytes);
                        Thread.Sleep(200);

                        if (_readAdc8BitsBuff.StartsWith(sendStr)
                            && _readAdc8BitsBuff.Length == sendStr.Length + 8)
                        {
                            Adc18Bits = Convert.ToByte(_readAdc8BitsBuff.Substring(10, 2), 16);
                            Adc28Bits = Convert.ToByte(_readAdc8BitsBuff.Substring(12, 2), 16);

                            Adc18BitsToResValue = Adc18Bits / 255 * 5 / ((5 - Adc18Bits / 255 * 5) / 6800);
                            Adc28BitsToResValue = Adc28Bits / 255 * 5 / ((5 - Adc28Bits / 255 * 5) / 6800);
                        }

                        _isReadAdc8Bits = false;
                    }
                }
            }
        }

        [Description("读ADC_10Bits")]
        public void ReadAdc_10Bits(string addr)
        {
            Adc110Bits = -9999;
            Adc210Bits = -9999;

            Adc110BitsToResValue = -9999;
            Adc210BitsToResValue = -9999;

            _readAdc10BitsBuff = string.Empty;

            int slaveAddr;
            if (int.TryParse(addr, out slaveAddr))
            {
                if (slaveAddr >= 0 && slaveAddr <= 7)
                {
                    if (MySerialPort != null)
                    {
                        var bits = Convert.ToString(slaveAddr, 2).PadLeft(8, '0');

                        var dev = new DeviceIdUpto8Tps92662DevicesOnOneBus(
                            bits[7].ToString(), bits[6].ToString(), bits[5].ToString(), false);

                        _isReadAdc10Bits = true;
                        var sendBytes = SendReadData(dev.DevId, 0xB3, 3);
                        var sendStr = ValueHelper.GetHextStr(sendBytes).Replace(" ", "");
                        MySerialPort.SendCommand(sendBytes);
                        Thread.Sleep(200);

                        if (_readAdc10BitsBuff.StartsWith(sendStr)
                            && _readAdc10BitsBuff.Length == sendStr.Length + 10)
                        {
                            var adc1LBits =
                                Convert.ToString(Convert.ToByte(_readAdc10BitsBuff.Substring(10, 2), 16), 2)
                                    .PadLeft(8, '0');
                            var adc2LBits =
                                Convert.ToString(Convert.ToByte(_readAdc10BitsBuff.Substring(12, 2), 16), 2)
                                    .PadLeft(8, '0');

                            var adc2H1HBits =
                                Convert.ToString(Convert.ToByte(_readAdc10BitsBuff.Substring(14, 2), 16), 2)
                                    .PadLeft(8, '0');

                            adc1LBits = adc2H1HBits.Substring(6, 2) + adc1LBits;
                            adc2LBits = adc2H1HBits.Substring(4, 2) + adc2LBits; 

                            Adc110Bits = Convert.ToUInt16(adc1LBits, 2);
                            Adc210Bits = Convert.ToUInt16(adc2LBits, 2);

                            Adc110BitsToResValue = Adc110Bits / 1023 * 5 / ((5 - Adc110Bits / 1023 * 5) / 6800);
                            Adc210BitsToResValue = Adc210Bits / 1023 * 5 / ((5 - Adc210Bits / 1023 * 5) / 6800);
                        }

                        _isReadAdc10Bits = false;
                    }
                }
            }
        }

        [Description("读ICID")]
        public void ReadIcid(string addr)
        {
            Icid = string.Empty;
            _isReadIcid = false;
            _readIcidBuff = string.Empty;

            int slaveAddr;
            if (int.TryParse(addr, out slaveAddr))
            {
                if (slaveAddr >= 0 && slaveAddr <= 7)
                {
                    if (MySerialPort != null)
                    {
                        var bits = Convert.ToString(slaveAddr, 2).PadLeft(8, '0');

                        var dev = new DeviceIdUpto8Tps92662DevicesOnOneBus(
                            bits[7].ToString(), bits[6].ToString(), bits[5].ToString(), false);

                        _isReadIcid = true;
                        var sendBytes = SendReadData(dev.DevId, 0xFF, 1);
                        var sendStr = ValueHelper.GetHextStr(sendBytes).Replace(" ", "");
                        MySerialPort.SendCommand(sendBytes);
                        Thread.Sleep(200);

                        if (_readIcidBuff.StartsWith(sendStr)
                            && _readIcidBuff.Length == sendStr.Length + 4 + 2)
                        {
                            Icid = "0x" + _readIcidBuff.Substring(10, 2);
                        }

                        _isReadIcid = false;
                    }
                }
            }
        }

        #endregion

        private static byte[] SendWrtieData(
            byte devId, byte startAddr, IReadOnlyCollection<byte> datas)
        {
            var len = datas.Count;
            var sendBytes = new List<byte>();

            switch (len)
            {
                case 1:
                    sendBytes.Add((byte)Commands.WriteCmdOf1Byte);
                    sendBytes.Add(devId);
                    sendBytes.Add(startAddr);
                    sendBytes.AddRange(datas);
                    sendBytes.AddRange(ValueHelper.IbmCrc16(sendBytes));
                    break;

                case 2:
                    sendBytes.Add((byte)Commands.WriteCmdOf2Byte);
                    sendBytes.Add(devId);
                    sendBytes.Add(startAddr);
                    sendBytes.AddRange(datas);
                    sendBytes.AddRange(ValueHelper.IbmCrc16(sendBytes));
                    break;

                case 3:
                    sendBytes.Add((byte)Commands.WriteCmdOf3Byte);
                    sendBytes.Add(devId);
                    sendBytes.Add(startAddr);
                    sendBytes.AddRange(datas);
                    sendBytes.AddRange(ValueHelper.IbmCrc16(sendBytes));
                    break;

                case 4:
                    sendBytes.Add((byte)Commands.WriteCmdOf4Byte);
                    sendBytes.Add(devId);
                    sendBytes.Add(startAddr);
                    sendBytes.AddRange(datas);
                    sendBytes.AddRange(ValueHelper.IbmCrc16(sendBytes));
                    break;

                case 12:
                    sendBytes.Add((byte)Commands.WriteCmdOf12Byte);
                    sendBytes.Add(devId);
                    sendBytes.Add(startAddr);
                    sendBytes.AddRange(datas);
                    sendBytes.AddRange(ValueHelper.IbmCrc16(sendBytes));
                    break;

                case 16:
                    sendBytes.Add((byte)Commands.WriteCmdOf16Byte);
                    sendBytes.Add(devId);
                    sendBytes.Add(startAddr);
                    sendBytes.AddRange(datas);
                    sendBytes.AddRange(ValueHelper.IbmCrc16(sendBytes));
                    break;

                case 20:
                    sendBytes.Add((byte)Commands.WriteCmdOf20Byte);
                    sendBytes.Add(devId);
                    sendBytes.Add(startAddr);
                    sendBytes.AddRange(datas);
                    sendBytes.AddRange(ValueHelper.IbmCrc16(sendBytes));
                    break;

                case 32:
                    sendBytes.Add((byte)Commands.WriteCmdOf32Byte);
                    sendBytes.Add(devId);
                    sendBytes.Add(startAddr);
                    sendBytes.AddRange(datas);
                    sendBytes.AddRange(ValueHelper.IbmCrc16(sendBytes));
                    break;
            }

            return sendBytes.ToArray();
        }

        private static byte[] SendReadData(
            byte devId, byte startAddr, int bytesLen)
        {
            var sendBytes = new List<byte>();

            switch (bytesLen)
            {
                case 1:
                    sendBytes.Add((byte)Commands.ReadCmdOf1Byte);
                    sendBytes.Add(devId);
                    sendBytes.Add(startAddr);
                    sendBytes.AddRange(ValueHelper.IbmCrc16(sendBytes));
                    break;

                case 2:
                    sendBytes.Add((byte)Commands.ReadCmdOf2Byte);
                    sendBytes.Add(devId);
                    sendBytes.Add(startAddr);
                    sendBytes.AddRange(ValueHelper.IbmCrc16(sendBytes));
                    break;

                case 3:
                    sendBytes.Add((byte)Commands.ReadCmdOf3Byte);
                    sendBytes.Add(devId);
                    sendBytes.Add(startAddr);
                    sendBytes.AddRange(ValueHelper.IbmCrc16(sendBytes));
                    break;

                case 4:
                    sendBytes.Add((byte)Commands.ReadCmdOf4Byte);
                    sendBytes.Add(devId);
                    sendBytes.Add(startAddr);
                    sendBytes.AddRange(ValueHelper.IbmCrc16(sendBytes));
                    break;

                case 12:
                    sendBytes.Add((byte)Commands.ReadCmdOf12Byte);
                    sendBytes.Add(devId);
                    sendBytes.Add(startAddr);
                    sendBytes.AddRange(ValueHelper.IbmCrc16(sendBytes));
                    break;

                case 16:
                    sendBytes.Add((byte)Commands.ReadCmdOf16Byte);
                    sendBytes.Add(devId);
                    sendBytes.Add(startAddr);
                    sendBytes.AddRange(ValueHelper.IbmCrc16(sendBytes));
                    break;

                case 32:
                    sendBytes.Add((byte)Commands.ReadCmdOf32Byte);
                    sendBytes.Add(devId);
                    sendBytes.Add(startAddr);
                    sendBytes.AddRange(ValueHelper.IbmCrc16(sendBytes));
                    break;
            }

            return sendBytes.ToArray();
        }

        private enum Commands : byte
        {
            WriteCmdOf1Byte = 0x87,

            WriteCmdOf2Byte = 0x99,

            WriteCmdOf3Byte = 0x1E,

            WriteCmdOf4Byte = 0xAA,

            WriteCmdOf12Byte = 0x2D,

            WriteCmdOf16Byte = 0x33,

            WriteCmdOf20Byte = 0xB5,

            WriteCmdOf32Byte = 0xB4,

            ReadCmdOf1Byte = 0x4B,

            ReadCmdOf2Byte = 0xCC,

            ReadCmdOf3Byte = 0xD2,

            ReadCmdOf4Byte = 0x55,

            ReadCmdOf12Byte = 0xE1,

            ReadCmdOf16Byte = 0x66,

            ReadCmdOf32Byte = 0x78,
        }
    }
}
