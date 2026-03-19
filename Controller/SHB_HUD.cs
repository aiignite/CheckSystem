using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Controller
{
    [Description("CAN-Product,SH-B_HUD")]
    public sealed class SHB_HUD : ControllerBase
    {
        public CanBus Can;

        public SHB_HUD(string name) : base(name)
        {
            //var str = "85 C7 02 E1 45 D1 1B 88 27 16 9F 5A C3 36 99 D2";
            //var seedBytes = new List<byte>();
            //for (var i = 0; i < str.Replace(" ", "").Length; i = i + 2)
            //{
            //    var b = Convert.ToByte(str.Replace(" ", "").Substring(i, 2), 16);
            //    seedBytes.Add(b);
            //}

            //var keyBytes = new byte[16];
            //int keyLen;
            //CalcKey.GenerateKeyEx(seedBytes.ToArray(), 16, 0x01, null, keyBytes, 16, out keyLen);
            //Console.WriteLine("seed: " + ValueHelper.GetHextStrWithOx(seedBytes.ToArray()));
            //Console.WriteLine("key: " + ValueHelper.GetHextStrWithOx(keyBytes));

            CanBus.PushCanMsg += CanBus_PushCanMsg;
            MainWork();
            SchedulerAsync();
        }

        ~SHB_HUD() => Dispose();

        private void MainWork()
        {
            //SetTimer100Ms();
            SetTimer50Ms();
        }

        public void SetTimer50Ms()
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                //Action = SendMsg(new CanBus.CanDataPackage[]
                //{
                //    new CanBus.CanDataPackage(0x638, CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8]),
                //    new CanBus.CanDataPackage(0x34A, CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8]{ 0x00,0x00,0x00,0x00,0x00,0x00,0x02,0x00}),
                //    new CanBus.CanDataPackage(0x111, CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8]{ 0x00,0x00,0x00,0x00,0x00,0x00,0x02,0x00})
                //}),
                //Interval = 50

                Action = SendMsg(() =>
                {
                    return new CanBus.CanDataPackage[]
                    {
                        //new CanBus.CanDataPackage(0x70A, CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8]),
                          new CanBus.CanDataPackage(0x70A, CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8]{ 0x03,0x22,0xF1,0x89,0x00,0x00,0x00,0x00}),
                    };
                }),

                Interval = 50
            });
        }

        private Action SendMsg(Func<CanBus.CanDataPackage[]> msgFunc)
        {
            return () =>
            {
                if (Can is null)
                    return;

                if (_bCanStart && msgFunc != null)
                {
                    lock (_lockCanSender)
                        Can.SendCanDatas(msgFunc.Invoke());
                }
            };
        }

        private void CanBus_PushCanMsg(string name, CanBus.CanDataPackage data, CanBus.OnPushCanDataType onPushCanDataType)
        {
            if (Can is null)
                return;

            if (Can.Name != name)
                return;

            if (data is null || data.CanData.Length < 8)
                return;

            if (data.CanId == DiagnosticReqCanId || data.CanId == DiagnosticRecvCanId)
            {
                if ((data.CanData[3] == 0xF1 && data.CanData[4] == 0x89))
                {

                }
                else if (data.CanData[2] == 0xF1 && data.CanData[3] == 0x89)
                {

                }
                else
                {
                    Console.WriteLine(ValueHelper.GetHextStrWithOx(data.CanData));
                }
            }

            if (data.CanId == DiagnosticRecvCanId)
            {
                //Console.WriteLine("rx 0x14DAF1BE: " + ValueHelper.GetHextStr(data.CanData));

                if (_bReadDid)
                {
                    _uds22Buffer.AddRange(data.CanData);
                    _bReadDidWaitHandle.Set();
                }

                if (_bSeedKeySubFunc)
                {
                    _uds27Buffer.AddRange(data.CanData);
                    _bseedKeyWaitHandle.Set();
                }
            }
        }

        internal static class CalcKey
        {
            [DllImport(@"\DllImport\SeednKey_SHB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr GenerateKeyEx(
                byte[] iSeedArray, /* Array for the seed [in] */
                short iSeedArraySize, /* Length of the array for the seed [in] */
                int iSecurityLevel, /* Security level [in] */
                byte[] iVariant, /* Name of the active variant [in] */
                byte[] ioKeyArray, /* Array for the key [in, out] */
                int iKeyArraySize, /* Maximum length of the array for the key [in] */
                out int oSize /* Length of the key [out] */);
        }

        private bool _bCanStart;

        [Description("StartCan")]
        public void StartCan() => _bCanStart = true;

        [Description("StopCan")]
        public void StopCan() => _bCanStart = false;

        [Description("R,SoftwareVer")]
        public string SoftwareVer = string.Empty;
        [Description("R,PartNo")]
        public string PartNo = string.Empty;
        [Description("R,BootVer")]
        public string BootVer = string.Empty;

        [Description("读取软件版本信息")]
        public void SoftwareVerRead()
        {
            SoftwareVer = string.Empty;
            SoftwareVer = Encoding.ASCII.GetString(ReadDid(0xF1, 0x89));
        }

        [Description("读取硬件号")]
        public void PartNoRead()
        {
            PartNo = string.Empty;
            PartNo = Encoding.ASCII.GetString(ReadDid(0xF1, 0x87));
        }

        [Description("读取Boot版本信息")]
        public void BootVerRead()
        {
            BootVer = string.Empty;
            BootVer = Encoding.ASCII.GetString(ReadDid(0xF1, 0x80));
        }

        [Description("R,SoftID-ASCII")]
        public string SoftIDASCII = string.Empty;
        [Description("R,SoftID-hex")]
        public string SoftIDHex = string.Empty;
        [Description("R,EcuHardwareNo")]
        public string EcuHardwareNo = string.Empty;

        [Description("寫入软件识别码softID")]
        public void SoftIDWrite(string str)
        {
            if (Can is null)
                return;

            //var str = "CDC051001";
            var bs = Encoding.ASCII.GetBytes(str);
            lock (_lockCanSender)
            {
                Can.SendStandardCanFdData(DiagnosticReqCanId, new byte[16] { 0x00, 0x0C, 0x2E, 0xF0, 0xFA, bs[0], bs[1], bs[2], bs[3], bs[4], bs[5], bs[6], bs[7], bs[8], 0x00, 0x00 });
                Thread.Sleep(50);
            }
        }

        [Description("读取软件识别码softID")]
        public void SoftIDRead()
        {
            SoftIDASCII = string.Empty;
            SoftIDHex = string.Empty;
            var r = ReadDid(0xF0, 0xFA);
            SoftIDASCII = Encoding.ASCII.GetString(r);
            SoftIDHex = ValueHelper.GetHextStr(r).Replace(" ", "");
        }

        [Description("寫入供应商定义ECU硬件版本号")]
        public void EcuHardwareNoWrite(string str)
        {
            if (Can is null)
                return;

            //var str = "00000001";
            var bs = new List<byte>();
            for (var i = 0; i < str.Length; i = i + 2)
            {
                var b = Convert.ToByte(str.Substring(i, 2));
                bs.Add(b);
            }
            lock (_lockCanSender)
            {
                Can.SendStandardCanFdData(DiagnosticReqCanId, new byte[] { 0x07, 0x2E, 0xF1, 0x93, bs[0], bs[1], bs[2], bs[3] });
                Thread.Sleep(50);
            }
        }

        [Description("读取供应商定义ECU硬件版本号")]
        public void EcuHardwareNoRead()
        {
            EcuHardwareNo = string.Empty;
            var r = ReadDid(0xF1, 0x93);
            EcuHardwareNo = ValueHelper.GetHextStr(r).Replace(" ", "");
        }

        [Description("R,volt")]
        public double Voltage = double.MinValue;

        [Description("讀取供電電壓")]
        public void VoltageRead()
        {
            Voltage = double.MinValue;
            var r = ReadDid(0x56, 0x92);
            if (r.Length >= 1)
            {
                Voltage = r[0] / 10.0;
            }
        }

        [Description("R,State")]
        public string State = string.Empty;

        [Description("讀取狀態")]
        public void StateRead()
        {
            State = string.Empty;
            State = ValueHelper.GetHextStr(ReadDid(0x56, 0x97));
        }

        private byte[] ReadDid(byte didHi, byte didLo)
        {
            if (Can is null)
                return new byte[0];
            lock (_lockCanSender)
            {
                byte[] echo;
                if (CanFdUds22(didHi, didLo, out echo))
                {
                    Thread.Sleep(1);
                    return echo;
                }
            }

            Thread.Sleep(100);

            lock (_lockCanSender)
            {
                byte[] echo;
                if (CanFdUds22(didHi, didLo, out echo))
                {
                    Thread.Sleep(1);
                    return echo;
                }
            }


            return new byte[0];
        }

        private readonly object _lockCanSender = new object();
        private List<byte> _uds22Buffer = new List<byte>();
        private bool _bReadDid;
        private bool _bReadSecodnFrame;
        private ManualResetEventSlim _bReadDidWaitHandle = new ManualResetEventSlim(false);
        private uint DiagnosticReqCanId = 0x70A;
        private uint DiagnosticRecvCanId = 0x78A;

        private bool CanFdUds22(byte didHi, byte didLo, out byte[] echo)
        {
            _uds22Buffer.Clear();

            if (Can is null)
            {
                echo = new byte[0];
                return false;
            }

            _bReadDidWaitHandle.Reset();
            _bReadDid = true;

            var firstSend = new byte[] { 0x03, 0x22, didHi, didLo, 0x00, 0x00, 0x00, 0x00 };

            var t1 = HighPrecisionTimer.GetTimestamp();

            //使用 ThreadPool 异步发送，减少 Task 创建开销
            ThreadPool.QueueUserWorkItem(_ => Can.SendStandardCanFdData(DiagnosticReqCanId, firstSend));

            // 同步等待响应
            var isFirstRecvOk = _bReadDidWaitHandle.Wait(250);

            _bReadDid = false;
            _bReadDidWaitHandle.Reset();

            var t2 = HighPrecisionTimer.GetTimestamp();
            Console.WriteLine("read did cost: {0}/ms", HighPrecisionTimer.GetTimestampIntervalMs(t1, t2));

            if (isFirstRecvOk)
            {
                var bufferArray = _uds22Buffer.ToArray();
                var count = bufferArray.Length;

                if (count >= 4)
                {
                    var b1 = bufferArray[0];
                    var b2 = bufferArray[1];
                    var b3 = bufferArray[2];
                    var b4 = bufferArray[3];

                    if (count == 8 && b1.GetByteHighOrder() == 0x00 && b2 == 0x62 && b3 == didHi && b4 == didLo)
                    {
                        var datalen = b1.GetByteLowOrder();
                        if (datalen >= 3 && datalen <= 7)
                        {
                            echo = new byte[datalen - 3];
                            Array.Copy(bufferArray, 4, echo, 0, datalen - 3);
                            return true;
                        }
                    }
                    else if (count > 8 && b1.GetByteHighOrder() == 0x00)
                    {
                        var datalen = b2;
                        if (datalen >= 3 && datalen <= 62 && count >= datalen + 2)
                        {
                            echo = new byte[datalen - 3];
                            Array.Copy(bufferArray, 5, echo, 0, datalen - 3);
                            return true;
                        }
                    }
                }
            }

            echo = new byte[0];
            return false;
        }

        [Description("进入拓展模式")]
        public void EnterExtendMode()
        {
            if (Can is null)
                return;

            Can.CanBusWithUds.TryEnterExtendedSession(DiagnosticReqCanId, DiagnosticRecvCanId, CanBus.CanType.Standard, CanBus.CanProtocol.CanFd, pendingByte: 0x00);
        }

        [Description("R,安全访问解锁0102结果")]
        public string SecurityAccess0102Result = string.Empty;

        [Description("安全访问解锁0102")]
        public void SecurityAccess0102()
        {
            lock (_lockCanSender)
            {
                SecurityAccess0102Result = string.Empty;
                SecurityAccess0102Result = SecurityAccess(0x01, 0x02);
            }
        }

        private List<byte> _uds27Buffer = new List<byte>();
        private bool _bSeedKeySubFunc;
        private ManualResetEventSlim _bseedKeyWaitHandle = new ManualResetEventSlim(false);
        private bool _bInSeedKey;

        [Description("测试-全部读取")]
        public void AllReadTest(int x)
        {
            SecurityAccess0102Result = string.Empty;
            SoftwareVer = string.Empty;
            BootVer = string.Empty;
            PartNo = string.Empty;
            SoftIDASCII = string.Empty;
            SoftIDHex = string.Empty;
            EcuHardwareNo = string.Empty;
            Voltage = double.MinValue;
            State = string.Empty;

            StartCan();
            Thread.Sleep(1200);
            StopCan();
            Thread.Sleep(500);
            EnterExtendMode();
            Thread.Sleep(500);
            SecurityAccess0102();
            Thread.Sleep(500);
            SoftIDWrite("CDC051001");
            Thread.Sleep(500);
            //EcuHardwareNoWrite("00000002");
            EcuHardwareNoWrite(x.ToString().PadLeft(8, '0'));
            //EcuHardwareNoWrite(1.ToString().PadLeft(8, '0'));
            ////Thread.Sleep(500);
            //SoftwareVerRead();
            //PartNoRead();
            //BootVerRead();
            //SoftIDRead();
            //EcuHardwareNoRead();
            //VoltageRead();
            //StateRead();
        }

        private string SecurityAccess(byte requesetSeedSubFunc, byte sendKeySubunc)
        {
            if (Can is null)
                return string.Empty;

            //var keyBytes = new byte[12];
            //for (var i = 0; i < keyBytes.Length; i++)
            //    keyBytes[i] = (byte)(i + 1);

            _bseedKeyWaitHandle.Reset();
            _uds27Buffer.Clear();
            _bSeedKeySubFunc = true;
            _bInSeedKey = true;
            var firstSend = new byte[] { 0x02, 0x27, requesetSeedSubFunc, 0x00, 0x00, 0x00, 0x00, 0x00 };
            //var taskFirstWait = Task.Factory.StartNew(() =>
            //{
            //    return _bseedKeyWaitHandle.Wait(1500);
            //});
            var taskFirstWait = Task<bool>.Factory.StartNew(() => _bseedKeyWaitHandle.Wait(1500));
            var taskFirstSend = Task.Factory.StartNew(() =>
            {
                Can.SendStandardCanFdData(DiagnosticReqCanId, firstSend);
            });
            Task.WaitAll(taskFirstSend, taskFirstWait);
            _bSeedKeySubFunc = false;
            _bseedKeyWaitHandle.Reset();
            _bInSeedKey = false;

            var isFirstRecvOk = taskFirstWait.Result;
            if (isFirstRecvOk)
            {
                if (_uds27Buffer.Count >= 4)
                {
                    var b1 = _uds27Buffer[0];
                    var b2 = _uds27Buffer[1];
                    var b3 = _uds27Buffer[2];
                    var b4 = _uds27Buffer[3];

                    if (b1.GetByteHighOrder() == 0x00 && b3 == 0x67 && b4 == requesetSeedSubFunc)
                    {
                        var seedBytes = new byte[16];
                        Array.Copy(_uds27Buffer.ToArray(), 4, seedBytes, 0, 16);

                        var keyBytes = new byte[16];
                        int keyLen;
                        CalcKey.GenerateKeyEx(seedBytes, 16, 0x01, null, keyBytes, 16, out keyLen);

                        _bseedKeyWaitHandle.Reset();
                        _uds27Buffer.Clear();
                        _bSeedKeySubFunc = true;
                        //firstSend = new byte[16] { 0x00, 0x12, 0x27, sendKeySubunc, keyBytes[0], keyBytes[1], keyBytes[2], keyBytes[3], keyBytes[4], keyBytes[5], keyBytes[6], keyBytes[7], keyBytes[8], keyBytes[9], keyBytes[10], keyBytes[11] };
                        firstSend = new byte[20];
                        firstSend[0] = 0x00;
                        firstSend[1] = 16 + 2;
                        firstSend[2] = 0x27;
                        firstSend[3] = sendKeySubunc;
                        Array.Copy(keyBytes, 0, firstSend, 4, 16);
                        //var firstSend2 = new byte[8];
                        //firstSend2[0] = keyBytes[12];
                        //firstSend2[1] = keyBytes[13];
                        //firstSend2[2] = keyBytes[14];
                        //firstSend2[3] = keyBytes[15];
                        taskFirstWait = Task.Factory.StartNew(() =>
                        {
                            return _bseedKeyWaitHandle.Wait(1500);
                        });
                        taskFirstSend = Task.Factory.StartNew(() =>
                        {
                            Can.SendStandardCanFdData(DiagnosticReqCanId, firstSend);
                            //Thread.Sleep(15);
                            //Can.SendStandardCanFdData(DiagnosticReqCanId, firstSend2);
                        });
                        Task.WaitAll(taskFirstSend, taskFirstWait);
                        _bSeedKeySubFunc = false;

                        var isSecondRecvOk = taskFirstWait.Result;
                        if (_uds27Buffer.Count >= 4)
                        {
                            b1 = _uds27Buffer[0];
                            b2 = _uds27Buffer[1];
                            b3 = _uds27Buffer[2];
                            b4 = _uds27Buffer[3];

                            if (b1.GetByteHighOrder() == 0x00 && b1.GetByteLowOrder() == 0x02 && b2 == 0x67 && b3 == sendKeySubunc)
                            {
                                return "OK";
                            }
                            else
                            {
                                return "NG 解锁KEY失败";
                            }
                        }
                        else
                        {
                            return "NG 解锁KEY失败";
                        }
                    }
                    else
                    {
                        return "NG 获取SEED失败";
                    }
                }
                else
                {
                    return "NG 获取SEED失败";
                }
            }
            else
            {
                return "NG 获取SEED失败";
            }

            //return CanFD.CanBusWithUds.TryRequestSeed(DiagnosticReqCanId, DiagnosticRecvCanId, CanBus.CanType.Extended, requesetSeedSubFunc, out _, pendingByte: 0x00, canProtocol: CanProtocol.CanFd) ?
            //    (CanFD.CanBusWithUds.TrySendKey(DiagnosticReqCanId, DiagnosticRecvCanId, CanBus.CanType.Extended, sendKeySubunc, keyBytes, pendingByte: 0x00, canProtocol: CanProtocol.CanFd) ? "OK" : "NG 解锁KEY失败") :
            //    "NG 获取SEED失败";
        }
    }
}
