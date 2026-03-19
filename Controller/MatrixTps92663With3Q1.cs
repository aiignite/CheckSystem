using CommonUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace Controller
{
    public sealed class MatrixTps92663With3Q1 : ControllerBase
    {
        public MySerialPort MySerialPort;
        private float _hz = 500f;
        private readonly Dictionary<int, int> _ptcntDictionary = new Dictionary<int, int>();

        [Description("R,ICID")]
        public string Icid = string.Empty;

        public float LedCount1Vout;
        public float LedCount2Vout;
        public float LedCount3Vout;
        public float LedCount4Vout;
        public float LedCount5Vout;
        public float LedCount6Vout;
        public float LedCount7Vout;
        public float LedCount8Vout;
        public float LedCount9Vout;
        public float LedCount10Vout;
        public float LedCount11Vout;
        public float LedCount12Vout;

        public float LedCount1Iin;
        public float LedCount2Iin;
        public float LedCount3Iin;
        public float LedCount4Iin;
        public float LedCount5Iin;
        public float LedCount6Iin;
        public float LedCount7Iin;
        public float LedCount8Iin;
        public float LedCount9Iin;
        public float LedCount10Iin;
        public float LedCount11Iin;
        public float LedCount12Iin;

        private Thread MainThread { get; set; }

        public MatrixTps92663With3Q1(string name)
            : base(name)
        {
            #region HZ数据字典
            _ptcntDictionary.Add(0, 2);
            _ptcntDictionary.Add(1, 3);
            _ptcntDictionary.Add(2, 4);
            _ptcntDictionary.Add(3, 6);
            _ptcntDictionary.Add(4, 8);
            _ptcntDictionary.Add(5, 9);
            _ptcntDictionary.Add(6, 10);
            _ptcntDictionary.Add(7, 11);
            _ptcntDictionary.Add(8, 12);
            _ptcntDictionary.Add(9, 13);
            _ptcntDictionary.Add(10, 14);
            _ptcntDictionary.Add(11, 15);
            _ptcntDictionary.Add(12, 16);
            _ptcntDictionary.Add(13, 17);
            _ptcntDictionary.Add(14, 18);
            _ptcntDictionary.Add(15, 19);
            _ptcntDictionary.Add(16, 20);
            _ptcntDictionary.Add(17, 21);
            _ptcntDictionary.Add(18, 22);
            _ptcntDictionary.Add(19, 23);
            _ptcntDictionary.Add(20, 24);
            _ptcntDictionary.Add(21, 25);
            _ptcntDictionary.Add(22, 26);
            _ptcntDictionary.Add(23, 27);
            _ptcntDictionary.Add(24, 28);
            _ptcntDictionary.Add(25, 29);
            _ptcntDictionary.Add(26, 30);
            _ptcntDictionary.Add(27, 31);
            _ptcntDictionary.Add(28, 32);
            _ptcntDictionary.Add(29, 33);
            _ptcntDictionary.Add(30, 35);
            _ptcntDictionary.Add(31, 35);
            _ptcntDictionary.Add(32, 36);
            _ptcntDictionary.Add(33, 37);
            _ptcntDictionary.Add(34, 38);
            _ptcntDictionary.Add(35, 39);
            _ptcntDictionary.Add(36, 40);
            _ptcntDictionary.Add(37, 41);
            _ptcntDictionary.Add(38, 42);
            _ptcntDictionary.Add(39, 43);
            _ptcntDictionary.Add(40, 44);
            _ptcntDictionary.Add(41, 45);
            _ptcntDictionary.Add(42, 46);
            _ptcntDictionary.Add(43, 47);
            _ptcntDictionary.Add(44, 49);
            _ptcntDictionary.Add(45, 50);
            _ptcntDictionary.Add(46, 51);
            _ptcntDictionary.Add(47, 52);
            _ptcntDictionary.Add(48, 53);
            _ptcntDictionary.Add(49, 54);
            _ptcntDictionary.Add(50, 55);
            _ptcntDictionary.Add(51, 56);
            _ptcntDictionary.Add(52, 57);
            _ptcntDictionary.Add(53, 58);
            _ptcntDictionary.Add(54, 59);
            _ptcntDictionary.Add(55, 60);
            _ptcntDictionary.Add(56, 62);
            _ptcntDictionary.Add(57, 63);
            _ptcntDictionary.Add(58, 65);
            _ptcntDictionary.Add(59, 68);
            _ptcntDictionary.Add(60, 71);
            _ptcntDictionary.Add(61, 74);
            _ptcntDictionary.Add(62, 78);
            _ptcntDictionary.Add(63, 85);
            #endregion

            MySerialPort.PushSciMsg += MySerialPort_PushSciMsg;

            MainThread = new Thread(MainWork) { IsBackground = true };
            MainThread.Start();
        }

        ~MatrixTps92663With3Q1()
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
                    continue;

                try
                {
                    if (MySerialPort == null)
                        continue;

                    foreach (
                        var data in
                            _lstMatrix.Values.SelectMany(
                                t => t.LstRegWithData.Select(j => SendWrtieData(t.DeviceId.DevId, j.Key, j.Value))))
                    {
                        MySerialPort.SendCommand(data);
                        Thread.Sleep(5);
                    }

                    _sendCount++;
                    if (_sendCount < 50)
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

        private bool _isSycConfiged;
        private int _sendCount;

        private readonly Dictionary<int, SingleMatrix> _lstMatrix =
            new Dictionary<int, SingleMatrix>();

        //private readonly Dictionary<int, DeviceIdUpto8Tps92662DevicesOnOneBus> _lstTpsAddr =
        //    new Dictionary<int, DeviceIdUpto8Tps92662DevicesOnOneBus>();

        //private readonly Dictionary<int, LedDutyCycle> LstLeds =
        //    new Dictionary<int, LedDutyCycle>();

        public void Init(string devAddr)
        {
            if (devAddr.Length != 2 && devAddr.Length != 3 && devAddr.Length != 4)
                return;

            if (_lstMatrix.Count > 3)
                return;

            var upToDeviceType = 0;
            if (devAddr.Length == 3)
                upToDeviceType = 1;
            else if (devAddr.Length == 4)
                upToDeviceType = 2;

            var isBroadcast = devAddr == "FFF";

            if (!isBroadcast)
            {
                foreach (var t in devAddr)
                {
                    try
                    {
                        var b = Convert.ToByte(t.ToString());
                        if (b != 1 && b != 0)
                        {
                            isBroadcast = true;
                            break;
                        }
                    }
                    catch (Exception)
                    {
                        isBroadcast = true;
                        break;
                    }
                }
            }

            if (upToDeviceType == 0)
            {
                var maxId = _lstMatrix.Count + 1;
                var deviceAddr = new DeviceIdUpto8Tps92662DevicesOnOneBus(
                    devAddr[1].ToString(),
                    devAddr[0].ToString(),
                    0.ToString(),
                    isBroadcast, 0);

                var single = new SingleMatrix(maxId, deviceAddr);
                _lstMatrix.Add(maxId, single);
            }
            else if (upToDeviceType == 1)
            {
                var maxId = _lstMatrix.Count + 1;
                var deviceAddr = new DeviceIdUpto8Tps92662DevicesOnOneBus(
                    0.ToString(),
                    devAddr[0].ToString(),
                    0.ToString(),
                    isBroadcast, 1, devAddr[1].ToString(), devAddr[2].ToString());

                var single = new SingleMatrix(maxId, deviceAddr);
                _lstMatrix.Add(maxId, single);
            }
            else
            {
                var maxId = _lstMatrix.Count + 1;
                var deviceAddr = new DeviceIdUpto8Tps92662DevicesOnOneBus(
                    0.ToString(),
                    devAddr[0].ToString(),
                    0.ToString(),
                    isBroadcast, 2, devAddr[1].ToString(), devAddr[2].ToString(), devAddr[3].ToString());

                var single = new SingleMatrix(maxId, deviceAddr);
                _lstMatrix.Add(maxId, single);
            }
        }

        private void MySerialPort_PushSciMsg(string name, byte[] datas)
        {
            if (MySerialPort == null || MySerialPort.Name != name)
                return;

            var str = datas.Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
            Console.WriteLine(str);

            if (_isReadIcid)
            {
                if (str.StartsWith("4B") && str.Length == 16)
                {
                    Icid = string.Format("{0}{1}", str[10], str[11]);
                    _waitHandle.Set();
                    _isReadIcid = true;
                }
            }

            if (_isReadDstr)
            {
                if (str.StartsWith("4B") && str.Length == 16)
                {
                    Dstr = string.Format("{0}{1}", str[10], str[11]);
                    _waitHandle.Set();
                    _isReadIcid = true;
                }
            }

            if (_isReadAdc8Bits)
                _readAdc8BitsBuff = ValueHelper.GetHextStr(datas).Replace(" ", "");
        }

        [Description("广播发送写ADCID命令")]
        public void BroadcastWriteAdcid(int value)
        {
            if (value == 0 || value == 1 || value == 2)
            {
                if (MySerialPort != null)
                {
                    for (int i = 0; i < 64; i++)
                    {
                        MySerialPort.SendCommand(SendWrtieData(0xBF, 0X83, new[] { (byte)value }));
                    }
                }
            }
        }

        [Description("发送初始化命令")]
        public void SysConfig()
        {
            //var p1 = 0 ^ 0 ^ 0;
            //var p0 = 1 ^ 0 ^ 0 ^ 0;

            //var str = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", p1, p0, 1, 0, 0, 0, 0, 1);
            //var hex = Convert.ToByte(str, 2);

            _isSycConfiged = false;

            if (MySerialPort == null)
                return;

            //var data2 = SendWrtieData(0xBF, 0x83, new byte[] { 0x01 });
            //MySerialPort.SendCommand(data2);
            //Thread.Sleep(5);

            for (var i = 1; i < _lstMatrix.Count + 1; i++)
            {
                var data = SendWrtieData(_lstMatrix[i].DeviceId.DevId, 0x80, new byte[] { 0x16 });
                MySerialPort.SendCommand(data);
                Thread.Sleep(5);

                var data1 = SendWrtieData(_lstMatrix[i].DeviceId.DevId, 0x82, new[] { GetHzByte(_hz) });
                MySerialPort.SendCommand(data1);
                Thread.Sleep(5);
            }

            _isSycConfiged = true;
        }

        private byte GetHzByte(float hz)
        {
            var getPtcnt = new Func<int, bool>(p => _ptcntDictionary.ContainsValue(p));

            const float mhz = 8000000f;
            var currentHz = 1024f * hz;
            const float ptbase1 = 1f;
            const float ptbase2 = 50f;
            const float ptbase3 = 125f;
            const float ptbase4 = 200f;
            var ptcnt = Convert.ToInt32(mhz / currentHz / ptbase1);

            if (getPtcnt(ptcnt))
                return Convert.ToByte("00" + Convert.ToString((from t in _ptcntDictionary.Keys
                                                               where _ptcntDictionary[t] == ptcnt
                                                               select (byte)_ptcntDictionary[t]).FirstOrDefault(), 2).PadLeft(6, '0'), 2);
            //return false;
            ptcnt = Convert.ToInt32(mhz / currentHz / ptbase2);
            if (getPtcnt(ptcnt))
                return Convert.ToByte("01" + Convert.ToString((from t in _ptcntDictionary.Keys
                                                               where _ptcntDictionary[t] == ptcnt
                                                               select (byte)_ptcntDictionary[t]).FirstOrDefault(), 2).PadLeft(6, '0'), 2);
            ptcnt = Convert.ToInt32(mhz / currentHz / ptbase3);
            if (getPtcnt(ptcnt))
                return Convert.ToByte("10" + Convert.ToString((from t in _ptcntDictionary.Keys
                                                               where _ptcntDictionary[t] == ptcnt
                                                               select (byte)_ptcntDictionary[t]).FirstOrDefault(), 2).PadLeft(6, '0'), 2);
            ptcnt = Convert.ToInt32(mhz / currentHz / ptbase4);
            if (!getPtcnt(ptcnt))
                return 0;

            return Convert.ToByte("11" + Convert.ToString((from t in _ptcntDictionary.Keys
                                                           where _ptcntDictionary[t] == ptcnt
                                                           select (byte)_ptcntDictionary[t]).FirstOrDefault(), 2).PadLeft(6, '0'), 2);
        }

        public bool ChangeHz(float hz)
        {
            var getPtcnt = new Func<int, bool>(p => _ptcntDictionary.ContainsValue(p));

            const float mhz = 8000000f;
            var currentHz = 1024f * hz;
            const float ptbase1 = 1f;
            const float ptbase2 = 50f;
            const float ptbase3 = 125f;
            const float ptbase4 = 200f;

            var ptcnt = Convert.ToInt32(mhz / currentHz / ptbase1);
            if (!getPtcnt(ptcnt))
            {
                //return false;
                ptcnt = Convert.ToInt32(mhz / currentHz / ptbase2);
                if (!getPtcnt(ptcnt))
                {
                    ptcnt = Convert.ToInt32(mhz / currentHz / ptbase3);
                    if (!getPtcnt(ptcnt))
                    {
                        ptcnt = Convert.ToInt32(mhz / currentHz / ptbase4);
                        if (!getPtcnt(ptcnt))
                        {
                            return false;
                        }

                        _hz = hz;
                        return true;
                    }

                    _hz = hz;
                    return true;
                }

                _hz = hz;
                return true;
            }

            _hz = hz;
            return true;
        }

        [Description("停止周期帧")]
        public void SysNotActive()
        {
            _isSycConfiged = false;
        }

        [Description("打开所有LED")]
        public void Allon()
        {
            SwitchLed(1, true);
            SwitchLed(2, true);
            SwitchLed(3, true);
            SwitchLed(4, true);
            SwitchLed(5, true);
            SwitchLed(6, true);
            SwitchLed(7, true);
            SwitchLed(8, true);
            SwitchLed(9, true);
            SwitchLed(10, true);
            SwitchLed(11, true);
            SwitchLed(12, true);
        }

        [Description("关闭所有LED")]
        public void Alloff()
        {
            SwitchLed(1, false);
            SwitchLed(2, false);
            SwitchLed(3, false);
            SwitchLed(4, false);
            SwitchLed(5, false);
            SwitchLed(6, false);
            SwitchLed(7, false);
            SwitchLed(8, false);
            SwitchLed(9, false);
            SwitchLed(10, false);
            SwitchLed(11, false);
            SwitchLed(12, false);
            Thread.Sleep(1500);
        }

        public void Calculate()
        {
            var temp1 = LedCount1Vout;
            var temp2 = LedCount2Vout;
            var temp3 = LedCount3Vout;
            var temp4 = LedCount4Vout;
            var temp5 = LedCount5Vout;
            var temp6 = LedCount6Vout;
            var temp7 = LedCount7Vout;
            var temp8 = LedCount8Vout;
            var temp9 = LedCount9Vout;
            var temp10 = LedCount10Vout;
            var temp11 = LedCount11Vout;
            var temp12 = LedCount12Vout;

            LedCount1Vout = 0;
            LedCount2Vout = 0;
            LedCount3Vout = 0;
            LedCount4Vout = 0;
            LedCount5Vout = 0;
            LedCount6Vout = 0;
            LedCount7Vout = 0;
            LedCount8Vout = 0;
            LedCount9Vout = 0;
            LedCount10Vout = 0;
            LedCount11Vout = 0;
            LedCount12Vout = 0;

            LedCount1Vout = temp1;
            LedCount2Vout = temp2 - temp1;
            LedCount3Vout = temp3 - temp2;
            LedCount4Vout = temp4 - temp3;
            LedCount5Vout = temp5 - temp4;
            LedCount6Vout = temp6 - temp5;
            LedCount7Vout = temp7 - temp6;
            LedCount8Vout = temp8 - temp7;
            LedCount9Vout = temp9 - temp8;
            LedCount10Vout = temp10 - temp9;
            LedCount11Vout = temp11 - temp10;
            LedCount12Vout = temp12 - temp11;
        }

        [Description("打开LED")]
        public void LedOn(string index)
        {
            SwitchLed(int.Parse(index), true);
        }

        [Description("关闭LED")]
        public void LedOff(string index)
        {
            SwitchLed(int.Parse(index), false);
        }

        public void LedOnWithPhaseAndWidth(string index, ushort phaseVal = 50, ushort widthVal = 1023)
        {
            SwitchLed(int.Parse(index), true, phaseVal, widthVal);
        }

        private void SwitchLed(
            int index, bool isOn, ushort phaseVal = 50, ushort widthVal = 1023)
        {
            if (MySerialPort == null)
                return;

            if (_lstMatrix.Count == 1 && index > 12)
                return;

            if (_lstMatrix.Count == 2 && index > 24)
                return;

            if (_lstMatrix.Count == 3 && index > 36)
                return;

            var idx = index;
            var matrixId = 1;
            if (idx >= 1 && idx <= 12)
                matrixId = 1;
            else if (idx >= 13 && idx <= 24)
                matrixId = 2;
            else if (idx >= 25 && idx <= 36)
                matrixId = 3;

            if (isOn)
            {
                _lstMatrix[matrixId].LedTurnOn(idx, phaseVal, widthVal);
            }
            else
            {
                _lstMatrix[matrixId].LedTurnOff(idx);
            }

            //foreach (
            //    var data in
            //        _lstMatrix.Values.SelectMany(
            //            t => t.LstRegWithData.Select(j => SendData(t.DeviceId.DevId, j.Key, j.Value))))
            //{
            //    MySerialPort.SendCommand(data);
            //    Thread.Sleep(5);
            //}
        }

        private bool _isReadIcid;
        private readonly EventWaitHandle _waitHandle = new AutoResetEvent(false);

        [Description("Read ICID")]
        public void ReadIcIdentificationRegister(string devAddr)
        {
            Icid = string.Empty;

            if (devAddr == "FFF" || (devAddr.Length != 2 && devAddr.Length != 3 && devAddr.Length != 4))
                return;

            var upToDeviceType = 0;
            if (devAddr.Length == 3)
                upToDeviceType = 1;
            else if (devAddr.Length == 4)
                upToDeviceType = 2;

            if (upToDeviceType == 0)
            {
                var deviceAddr = new DeviceIdUpto8Tps92662DevicesOnOneBus(
                    devAddr[1].ToString(),
                    devAddr[0].ToString(),
                    0.ToString(),
                    false, 0);

                var data2 = SendReadData(deviceAddr.DevId, 0xFF, 1);
                for (var i = 0; i < 10; i++)
                    MySerialPort.SendCommand(data2);
                _isReadIcid = true;
                MySerialPort.SendCommand(data2);
                _waitHandle.WaitOne(500);
                _isReadIcid = false;
            }
            else if (upToDeviceType == 1)
            {
                var deviceAddr = new DeviceIdUpto8Tps92662DevicesOnOneBus(
                    0.ToString(),
                    devAddr[0].ToString(),
                    0.ToString(),
                    false, 1, devAddr[1].ToString(), devAddr[2].ToString());

                var data2 = SendReadData(deviceAddr.DevId, 0xFF, 1);
                for (var i = 0; i < 10; i++)
                    MySerialPort.SendCommand(data2);
                _isReadIcid = true;
                MySerialPort.SendCommand(data2);
                _waitHandle.WaitOne(500);
                _isReadIcid = false;
            }
            else
            {
                var deviceAddr = new DeviceIdUpto8Tps92662DevicesOnOneBus(
                    0.ToString(),
                    devAddr[0].ToString(),
                    0.ToString(),
                    false, 2, devAddr[1].ToString(), devAddr[2].ToString(), devAddr[3].ToString());

                var data2 = SendReadData(deviceAddr.DevId, 0xFF, 1);
                for (var i = 0; i < 10; i++)
                    MySerialPort.SendCommand(data2);
                _isReadIcid = true;
                MySerialPort.SendCommand(data2);
                _waitHandle.WaitOne(500);
                _isReadIcid = false;
            }
        }

        private bool _isReadDstr;

        [Description("R,DSTR")]
        public string Dstr = string.Empty;

        [Description("Read DSTR")]
        public void ReadDstrRegister(string devAddr)
        {
            Dstr = string.Empty;

            if (devAddr == "FFF" || (devAddr.Length != 2 && devAddr.Length != 3 && devAddr.Length != 4))
                return;

            var upToDeviceType = 0;
            if (devAddr.Length == 3)
                upToDeviceType = 1;
            else if (devAddr.Length == 4)
                upToDeviceType = 2;

            if (upToDeviceType == 0)
            {
                var deviceAddr = new DeviceIdUpto8Tps92662DevicesOnOneBus(
                    devAddr[1].ToString(),
                    devAddr[0].ToString(),
                    0.ToString(),
                    false, 0);

                var data2 = SendReadData(deviceAddr.DevId, 0xA2, 1);
                for (var i = 0; i < 10; i++)
                    MySerialPort.SendCommand(data2);
                _isReadDstr = true;
                MySerialPort.SendCommand(data2);
                _waitHandle.WaitOne(500);
                _isReadDstr = false;
            }
            else if (upToDeviceType == 1)
            {
                var deviceAddr = new DeviceIdUpto8Tps92662DevicesOnOneBus(
                    0.ToString(),
                    devAddr[0].ToString(),
                    0.ToString(),
                    false, 1, devAddr[1].ToString(), devAddr[2].ToString());

                var data2 = SendReadData(deviceAddr.DevId, 0xA2, 1);
                for (var i = 0; i < 10; i++)
                    MySerialPort.SendCommand(data2);
                _isReadDstr = true;
                MySerialPort.SendCommand(data2);
                _waitHandle.WaitOne(500);
                _isReadDstr = false;
            }
            else
            {
                var deviceAddr = new DeviceIdUpto8Tps92662DevicesOnOneBus(
                    0.ToString(),
                    devAddr[0].ToString(),
                    0.ToString(),
                    false, 2, devAddr[1].ToString(), devAddr[2].ToString(), devAddr[3].ToString());

                var data2 = SendReadData(deviceAddr.DevId, 0xA2, 1);
                for (var i = 0; i < 10; i++)
                    MySerialPort.SendCommand(data2);
                _isReadDstr = true;
                MySerialPort.SendCommand(data2);
                _waitHandle.WaitOne(500);
                _isReadDstr = false;
            }
        }

        private bool _isReadAdc8Bits;
        private string _readAdc8BitsBuff = string.Empty;

        [Description("R,读ADC1_8Bits")]
        public double Adc18Bits = -9999;
        [Description("R,读ADC2_8Bits")]
        public double Adc28Bits = -9999;
        [Description("R,读ADC1_8Bits转电阻值")]
        public double Adc18BitsToResValue = -9999;
        [Description("R,读ADC2_8Bits转电阻值")]
        public double Adc28BitsToResValue = -9999;

        [Description("读ADC_8Bits")]
        public void ReadAdc_8bits(string devAddr)
        {
            Adc18Bits = -9999;
            Adc28Bits = -9999;

            Adc18BitsToResValue = -9999;
            Adc28BitsToResValue = -9999;

            _readAdc8BitsBuff = string.Empty;

            if (devAddr == "FFF" || (devAddr.Length != 2 && devAddr.Length != 3 && devAddr.Length != 4))
                return;

            var upToDeviceType = 0;
            if (devAddr.Length == 3)
                upToDeviceType = 1;
            else if (devAddr.Length == 4)
                upToDeviceType = 2;

            int slaveAddr;
            //if (int.TryParse(addr, out slaveAddr))
            {
                //if (slaveAddr >= 0 && slaveAddr <= 7)
                {
                    if (MySerialPort != null)
                    {
                        if (upToDeviceType == 0)
                        {
                            //var bits = Convert.ToString(slaveAddr, 2).PadLeft(8, '0');

                            var dev = new DeviceIdUpto8Tps92662DevicesOnOneBus(
                                devAddr[1].ToString(),
                                devAddr[0].ToString(),
                                0.ToString(),
                                false, 0);

                            var sendBytes = SendReadData(dev.DevId, 0xA0, 2);
                            var sendStr = ValueHelper.GetHextStr(sendBytes).Replace(" ", "");
                            for (var i = 0; i < 10; i++)
                                MySerialPort.SendCommand(sendBytes);
                            Thread.Sleep(25);
                            _isReadAdc8Bits = true;
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
                        else if (upToDeviceType == 1)
                        {
                            //var bits = Convert.ToString(slaveAddr, 2).PadLeft(8, '0');

                            var dev = new DeviceIdUpto8Tps92662DevicesOnOneBus(
                                0.ToString(),
                                devAddr[0].ToString(),
                                0.ToString(),
                                false, 1, devAddr[1].ToString(), devAddr[2].ToString());

                            var sendBytes = SendReadData(dev.DevId, 0xA0, 2);
                            var sendStr = ValueHelper.GetHextStr(sendBytes).Replace(" ", "");
                            for (var i = 0; i < 10; i++)
                                MySerialPort.SendCommand(sendBytes);
                            Thread.Sleep(25);
                            _isReadAdc8Bits = true;
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
                        else
                        {
                            //var bits = Convert.ToString(slaveAddr, 2).PadLeft(8, '0');

                            var dev = new DeviceIdUpto8Tps92662DevicesOnOneBus(
                                0.ToString(),
                                devAddr[0].ToString(),
                                0.ToString(),
                                false, 2, devAddr[1].ToString(), devAddr[2].ToString(), devAddr[3].ToString());

                            var sendBytes = SendReadData(dev.DevId, 0xA0, 2);
                            var sendStr = ValueHelper.GetHextStr(sendBytes).Replace(" ", "");
                            for (var i = 0; i < 10; i++)
                                MySerialPort.SendCommand(sendBytes);
                            Thread.Sleep(25);
                            _isReadAdc8Bits = true;
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
        }

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

        private class SingleMatrix
        {
            public readonly int MatrixIndex;
            public DeviceIdUpto8Tps92662DevicesOnOneBus DeviceId { get; private set; }
            private readonly LedDutyCycle[,] _lstLeds = new LedDutyCycle[3, 4];
            public readonly Dictionary<byte, byte[]> LstRegWithData = new Dictionary<byte, byte[]>();

            public SingleMatrix(
                int matrixIndex,
                DeviceIdUpto8Tps92662DevicesOnOneBus deviceId)
            {
                MatrixIndex = matrixIndex;
                DeviceId = deviceId;

                var ledIndex = 1;
                if (matrixIndex == 1)
                    ledIndex = 1;
                else if (matrixIndex == 2)
                    ledIndex = 13;
                else if (matrixIndex == 3)
                    ledIndex = 25;

                for (var i = 0; i < 4; i++)
                    _lstLeds[0, i] = new LedDutyCycle((byte)ledIndex++);
                for (var i = 0; i < 4; i++)
                    _lstLeds[1, i] = new LedDutyCycle((byte)ledIndex++);
                for (var i = 0; i < 4; i++)
                    _lstLeds[2, i] = new LedDutyCycle((byte)ledIndex++);

                for (var i = 0; i <= 2; i++)
                    LstRegWithData.Add(_lstLeds[i, 0].RegAddr, new byte[2 * 2 * 4]);

                //var temp = LstRegWithData.OrderBy(t => t.Key).ToDictionary();
                //LstRegWithData.Clear();
            }

            public void LedTurnOn(int ledIndex, ushort phaseVal = 50, ushort widthVal = 1023)
            {
                SwitchLed(ledIndex, true, phaseVal, widthVal);
            }

            public void LedTurnOff(int ledIndex)
            {
                SwitchLed(ledIndex, false);
            }

            private void SwitchLed(int ledIndex, bool isOn, ushort phaseVal = 50, ushort widthVal = 1023)
            {
                if (MatrixIndex == 1 && ledIndex >= 1 && ledIndex <= 12)
                {
                    ledIndex = 12 - ledIndex + 1;

                    if (isOn)
                        _lstLeds[(ledIndex - 1) / 4, (ledIndex - 1) % 4].TurnOn(phaseVal, widthVal);
                    else
                        _lstLeds[(ledIndex - 1) / 4, (ledIndex - 1) % 4].TurnOff();
                }
                else if (MatrixIndex == 2 && ledIndex >= 13 && ledIndex <= 24)
                {
                    ledIndex = ledIndex - 12;
                    ledIndex = 12 - ledIndex + 1;

                    if (isOn)
                        _lstLeds[(ledIndex - 1) / 4, (ledIndex - 1) % 4].TurnOn(phaseVal, widthVal);
                    else
                        _lstLeds[(ledIndex - 1) / 4, (ledIndex - 1) % 4].TurnOff();
                }
                else if (MatrixIndex == 3 && ledIndex >= 25 && ledIndex <= 36)
                {
                    ledIndex = ledIndex - 24;
                    ledIndex = 12 - ledIndex + 1;

                    if (isOn)
                        _lstLeds[(ledIndex - 1) / 4, (ledIndex - 1) % 4].TurnOn(phaseVal, widthVal);
                    else
                        _lstLeds[(ledIndex - 1) / 4, (ledIndex - 1) % 4].TurnOff();
                }

                for (var i = 0; i < 3; i++)
                {
                    for (var j = 0; j < 4; j++)
                    {
                        LstRegWithData[_lstLeds[i, 0].RegAddr][j * 4] = _lstLeds[i, j].RegVal[0];
                        LstRegWithData[_lstLeds[i, 0].RegAddr][j * 4 + 1] = _lstLeds[i, j].RegVal[1];
                        LstRegWithData[_lstLeds[i, 0].RegAddr][j * 4 + 2] = _lstLeds[i, j].RegVal[2];
                        LstRegWithData[_lstLeds[i, 0].RegAddr][j * 4 + 3] = _lstLeds[i, j].RegVal[3];
                    }
                }
            }
        }

        private class DeviceIdUpto8Tps92662DevicesOnOneBus
        {
            public byte DevId { get; private set; }
            public string DevIdBitD4 { get; private set; }
            public string DevIdBitD3 { get; private set; }
            public string DevIdBitD2 { get; private set; }
            public string DevIdBitD1 { get; private set; }
            public string DevIdBitD0 { get; private set; }

            /// <summary>
            /// upToDevicesCount=0: Up to 8 TPS92662 Devices on the Bus
            /// upToDevicesCount=1: Up to 16 TPS92662 Devices on the Bus
            /// upToDevicesCount=2: Up to 31 TPS92662 Devices on the Bus
            /// </summary>
            public int UpToDevicesCount { get; private set; }

            public DeviceIdUpto8Tps92662DevicesOnOneBus(
                string addr0, string addr1, string addr2, bool broadcast, int upToDevicesCount, string adcOut7 = "0", string adcOut6 = "0", string adcOut5 = "0")
            {
                // upToDevicesCount=0: Up to 8 TPS92662 Devices on the Bus
                // upToDevicesCount=1: Up to 16 TPS92662 Devices on the Bus
                // upToDevicesCount=2: Up to 31 TPS92662 Devices on the Bus

                if (broadcast)
                {
                    DevIdBitD0 = 0.ToString();
                    DevIdBitD1 = 0.ToString();
                    DevIdBitD2 = 0.ToString();
                    DevIdBitD3 = 0.ToString();
                    DevIdBitD4 = 0.ToString();
                    DevId = 0xBF;
                }
                else
                {
                    if (upToDevicesCount == 0)
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
                    }
                    else if (upToDevicesCount == 1)
                    {
                        DevIdBitD0 = adcOut6;
                        DevIdBitD1 = adcOut7;
                        DevIdBitD2 = addr1;
                        DevIdBitD3 = addr2;
                        DevIdBitD4 = 0.ToString();

                        var p1 =
                            byte.Parse(DevIdBitD1) ^ byte.Parse(DevIdBitD3) ^ byte.Parse(DevIdBitD4);
                        var p0 = byte.Parse(DevIdBitD0) ^ byte.Parse(DevIdBitD1) ^
                                 byte.Parse(DevIdBitD2) ^ byte.Parse(DevIdBitD4);

                        var str = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", p1, p0, 1, DevIdBitD4,
                            DevIdBitD3, DevIdBitD2, DevIdBitD1, DevIdBitD0);
                        DevId = Convert.ToByte(str, 2);
                    }
                    else if (upToDevicesCount == 2)
                    {
                        DevIdBitD0 = adcOut5;
                        DevIdBitD1 = adcOut6;
                        DevIdBitD2 = adcOut7;
                        DevIdBitD3 = addr1;
                        DevIdBitD4 = addr2;

                        var p1 =
                            byte.Parse(DevIdBitD1) ^ byte.Parse(DevIdBitD3) ^ byte.Parse(DevIdBitD4);
                        var p0 = byte.Parse(DevIdBitD0) ^ byte.Parse(DevIdBitD1) ^
                                 byte.Parse(DevIdBitD2) ^ byte.Parse(DevIdBitD4);

                        var str = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", p1, p0, 1, DevIdBitD4,
                            DevIdBitD3, DevIdBitD2, DevIdBitD1, DevIdBitD0);
                        DevId = Convert.ToByte(str, 2);
                    }
                }
            }
        }

        private class LedDutyCycle
        {
            public int LedIndex { get; private set; }
            public byte RegAddr { get; private set; }
            public byte[] RegVal { get; private set; }

            public LedDutyCycle(byte index)
            {
                LedIndex = index;
                RegVal = new byte[4];

                //switch (index)
                //{
                //    case 1:
                //        RegAddr = (byte)108;
                //        break;

                //    case 2:
                //        RegAddr = (byte)104;
                //        break;

                //    case 3:
                //        RegAddr = (byte)100;
                //        break;

                //    case 4:
                //        RegAddr = (byte)96;
                //        break;

                //    case 5:
                //        RegAddr = (byte)92;
                //        break;

                //    case 6:
                //        RegAddr = (byte)88;
                //        break;

                //    case 7:
                //        RegAddr = (byte)84;
                //        break;

                //    case 8:
                //        RegAddr = (byte)80;
                //        break;

                //    case 9:
                //        RegAddr = (byte)76;
                //        break;

                //    case 10:
                //        RegAddr = (byte)72;
                //        break;

                //    case 11:
                //        RegAddr = (byte)68;
                //        break;

                //    case 12:
                //        RegAddr = (byte)64;
                //        break;

                //    default:
                //        break;
                //}
                RegAddr = (byte)(4 * (index - 1) + 0x40);
            }

            public void TurnOn(
                ushort phaseVal = 50, ushort widthVal = 1023)
            {
                UpdateRegData(phaseVal, widthVal);
            }

            public void TurnOff()
            {
                UpdateRegData(0, 0);
            }

            private void UpdateRegData(ushort ledPhase, ushort ledWidth)
            {
                var tempBytes1 = BitConverter.GetBytes(ledPhase);
                Array.Copy(tempBytes1, 0, RegVal, 0, 2);
                var tempBytes2 = BitConverter.GetBytes(ledWidth);
                Array.Copy(tempBytes2, 0, RegVal, 2, 2);
            }
        }

        private enum Commands : byte
        {
            WriteCmdOf1Byte = 0x87,

            WriteCmdOf2Byte = 0x99,

            WriteCmdOf3Byte = 0x1E,

            WriteCmdOf4Byte = 0xAA,

            WriteCmdOf12Byte = 0x2D,

            WriteCmdOf16Byte = 0x33,

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
