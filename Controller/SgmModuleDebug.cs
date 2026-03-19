using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using CommonUtility;
using CommonUtility.BusLoader;

namespace Controller
{
    [Description("CAN-Product,SGM模块调试类")]
    public sealed class SgmModuleDebug : ControllerBase
    {
        public CanBus Can;

        public SgmModuleDebug(string name)
            : base(name)
        {
            if (_keepNetworkThread != null)
            {
                _keepNetworkThread.Abort();
                _keepNetworkThread.Join();
            }

            _keepNetworkThread = new Thread(KeepNetwork) { IsBackground = true };
            _keepNetworkThread.Start();
        }

        ~SgmModuleDebug()
        {
            Dispose();
        }

        #region network
        private readonly Thread _keepNetworkThread;
        private readonly object _lockSend = new object();
        private bool _isSleep = true;
        private bool _isInExtendedSession;
        private const uint CanDiagnosisRequestPhyCanId = 0x14DAA4F1;
        //private const uint CanDiagnosisRequestFunCanId = 0x7DF;
        private const uint CanDiagnosisResponseCanId = 0x14DAF1A4;

        private void KeepNetwork()
        {
            while (_keepNetworkThread.IsAlive)
            {
                if (!_keepNetworkThread.IsAlive)
                    break;

                Thread.Sleep(100);

                if (Can == null)
                    continue;

                if (_isSleep)
                    continue;
                lock (_lockSend)
                {
                    Can.SendStandardCanData(
                       0x621, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                }
            }
        }
        #endregion

        #region 唤醒休眠，session选择
        [Description("唤醒")]
        public void Awake()
        {
            _isSleep = false;
        }

        [Description("休眠")]
        public void Sleep()
        {
            _isSleep = true;
        }

        [Description("进入正常模式")]
        public void EnterDefaultSession()
        {
            if (Can == null)
                return;

            lock (_lockSend)
            {
                _isInExtendedSession = false;

                if (Can.CanBusWithUds.TryEnterDefaultSession(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId, CanBus.CanType.Extended, CanBus.CanProtocol.Can))
                    return;

                Thread.Sleep(500);

                Can.CanBusWithUds.TryEnterDefaultSession(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId, CanBus.CanType.Extended, CanBus.CanProtocol.Can);
            }
        }

        [Description("进入拓展模式")]
        public void EnterExtendedSession()
        {
            if (Can == null)
                return;

            lock (_lockSend)
            {
                if (Can.CanBusWithUds.TryEnterExtendedSession(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId, CanBus.CanType.Extended))
                {
                    _isInExtendedSession = true;
                    return;
                }

                Thread.Sleep(500);
                if (Can.CanBusWithUds.TryEnterExtendedSession(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId, CanBus.CanType.Extended))
                {
                    _isInExtendedSession = true;
                }
            }
        }

        //[Description("进入编程模式")]
        //public void EnterProgramSession()
        //{
        //    if (Can == null)
        //        return;

        //    _isInExtendedSession = false;

        //    lock (_lockSend)
        //    {
        //        if (Can.CanBusWithUds.TryEnterProgrammingSession(
        //            CanDiagnosisRequestPhyCanId,
        //            CanDiagnosisResponseCanId, CanBus.CanType.Extended))
        //        {
        //            return;
        //        }

        //        Thread.Sleep(500);
        //        Can.CanBusWithUds.TryEnterProgrammingSession(
        //            CanDiagnosisRequestPhyCanId,
        //            CanDiagnosisResponseCanId, CanBus.CanType.Extended);
        //    }
        //}

        //[Description("关闭正常通信")]
        //public void DisableRxAndTxCommunication()
        //{
        //    Can.CanBusWithUds.TryCommunicationControl(
        //        CanDiagnosisRequestPhyCanId,
        //        CanDiagnosisResponseCanId,
        //        CanBus.CanType.Extended,
        //        Uds14229Helper.CommunicationControl.DisableRxAndTx);
        //}

        [Description("关闭正常通信")]
        public void DisableRxAndTxCommunication()
        {
            Can.CanBusWithUds.TryCommunicationControl(
                CanDiagnosisRequestPhyCanId,
                CanDiagnosisResponseCanId,
                CanBus.CanType.Extended,
                Uds14229Helper.CommunicationControl.DisableRxAndTx);
        }

        #endregion

        #region DID读取
        [Description("R,ECU_ID-F0F3")]
        public string EcuId;
        [Description("R,追溯信息MTC-F0B4")]
        public string TrackInfo;

        [Description("ReadF0F3和F0B4-ECU_ID和追溯信息")]
        public void ReadEcuIdAndTrackInfoThenTrackSql()
        {
            EcuId = string.Empty;
            TrackInfo = string.Empty;

            EcuId = ValueHelper.GetHextStr(ReadDidViaCan(0xF0, 0xF3)).Replace(" ", "");
            TrackInfo = ReadDidViaCan(0xF0, 0xB4).GetStringByAsciiBytes(true);
        }

        private byte[] ReadDidViaCan(byte didHi, byte didLo)
        {
            if (Can == null)
                return new List<byte>().ToArray();

            lock (Can)
            {
                byte[] echoBytes;
                if (Can.CanBusWithUds.TryReadData(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId,
                    CanBus.CanType.Extended, CanBus.CanProtocol.Can, didHi, didLo, out echoBytes))
                {
                    if (echoBytes != null)
                        return echoBytes;

                    return new List<byte>().ToArray();

                    //Thread.Sleep(250);
                    //if (!Can.CanBusWithUds.TryReadData(
                    //    CanDiagnosisRequestPhyCanId,
                    //    CanDiagnosisResponseCanId,
                    //    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    //    didHi, didLo, out echoBytes))
                    //    return new List<byte>().ToArray();
                    //return echoBytes ?? new List<byte>().ToArray();
                }

                //return new List<byte>().ToArray();

                Thread.Sleep(250);
                if (!Can.CanBusWithUds.TryReadData(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId,
                    CanBus.CanType.Extended, CanBus.CanProtocol.Can,
                    didHi, didLo, out echoBytes))
                    return new List<byte>().ToArray();

                if (echoBytes != null)
                    return echoBytes;

                Thread.Sleep(250);
                if (!Can.CanBusWithUds.TryReadData(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    didHi, didLo, out echoBytes))
                    return new List<byte>().ToArray();

                if (echoBytes != null)
                    return echoBytes;

                return new List<byte>().ToArray();

                Thread.Sleep(250);
                if (!Can.CanBusWithUds.TryReadData(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId,
                    CanBus.CanType.Extended, CanBus.CanProtocol.Can,
                    didHi, didLo, out echoBytes))
                    return new List<byte>().ToArray();
                return echoBytes ?? new List<byte>().ToArray();
            }
        }
        #endregion

        #region 安全信息

        //[Description("R,SeedKey注入结果")]
        //public string GeneralKey = string.Empty;

        [Description("R,安全访问0D0E结果")]
        public string SecurityAccessResult = string.Empty;

        [Description("R,Step1-MasterKey注入结果")]
        public string MasterKey = string.Empty;

        [Description("R,Step2-UnlockKey注入结果")]
        public string UnlockKey = string.Empty;

        [Description("R,Step3-MacKey注入结果")]
        public string MacKey = string.Empty;

        //[Description("检查SeedKey注入")]
        //public void CheckGeneralKey()
        //{
        //    GeneralKey = string.Empty;

        //    //读取F080
        //    byte[] responseData;
        //    Can.CanBusWithUds.TryReadData(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId, CanBus.CanType.Extended, CanBus.CanProtocol.Can, 0xF0, 0x80, out responseData);
        //    if (responseData == null || responseData[0] != 0x00)
        //    {
        //        GeneralKey = "NG";
        //        return;
        //    }

        //    //读取F081
        //    Can.CanBusWithUds.TryReadData(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId, CanBus.CanType.Extended, CanBus.CanProtocol.Can, 0xF0, 0x81, out responseData);
        //    var temp = ValueHelper.GetHextStr(responseData.Skip(3).ToArray()).Replace(" ", "");
        //    if (temp != "FFFFE1")
        //    {
        //        GeneralKey = "NG";
        //        return;
        //    }

        //    GeneralKey = "OK";
        //}

        [Description("安全访问0D0E")]
        public void SecurityAccess0D0E()
        {
            {
                SecurityAccessResult = string.Empty;

                byte[] seedBytes;
                if (Can.CanBusWithUds.TryRequestSeed(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                    CanBus.CanType.Extended, 0x0D, out seedBytes))
                {
                    var keyBytes = new byte[] { 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0x00, 0x11, 0x22 };

                    if (Can.CanBusWithUds.TrySendKey(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                        CanBus.CanType.Extended, 0x0E, keyBytes))
                    {
                        SecurityAccessResult = "OK";
                        return;
                    }
                }

                SecurityAccessResult = "NG";
            }

            if (SecurityAccessResult == "OK")
            {
                return;
            }
            else
            {
                SecurityAccessResult = string.Empty;

                byte[] seedBytes;
                if (Can.CanBusWithUds.TryRequestSeed(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                    CanBus.CanType.Extended, 0x0D, out seedBytes))
                {
                    var keyBytes = new byte[] { 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0x00, 0x11, 0x22 };

                    if (Can.CanBusWithUds.TrySendKey(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                        CanBus.CanType.Extended, 0x0E, keyBytes))
                    {
                        SecurityAccessResult = "OK";
                        return;
                    }
                }

                SecurityAccessResult = "NG";
            }
        }

        private bool UnlockSeedKey(
            byte keySlot,
            IEnumerable<byte> m1, IEnumerable<byte> m2, IEnumerable<byte> m3,
            out byte wrtieHistory, out byte[] m4, out byte[] m5)
        {
            if (Can == null)
            {
                m4 = new byte[] { };
                m5 = new byte[] { };
                wrtieHistory = 0xFF;
                return false;
            }

            lock (_lockSend)
            {
                EnterExtendedSession();
                Thread.Sleep(50);

                DisableRxAndTxCommunication();
                Thread.Sleep(50);

                SecurityAccess0D0E();
                Thread.Sleep(50);

                if (SecurityAccessResult == "OK")
                {
                    var routineControlOptBytes = new List<byte> { keySlot };
                    routineControlOptBytes.AddRange(m1);
                    routineControlOptBytes.AddRange(m2);
                    routineControlOptBytes.AddRange(m3);

                    byte[] echoBytes;
                    if (Can.CanBusWithUds.TryStartRoutineControl(
                        CanDiagnosisRequestPhyCanId,
                        CanDiagnosisResponseCanId,
                        CanBus.CanType.Extended,
                        0x02, 0x72, out echoBytes, routineControlOptBytes.ToArray()))
                    {
                        if (echoBytes.Length == 49)
                        {
                            wrtieHistory = echoBytes[0];

                            m4 = new byte[32];
                            Array.Copy(echoBytes, 1, m4, 0, 32);

                            m5 = new byte[16];
                            Array.Copy(echoBytes, 33, m5, 0, 16);

                            //if (wrtieHistory == 0x31 || wrtieHistory == 0x00)
                            //    return true;

                            Console.WriteLine(Name + " keySlot: " + ValueHelper.GetHextStrWithOx(keySlot) + ", WriteHistory: " + ValueHelper.GetHextStrWithOx(wrtieHistory));

                            if (wrtieHistory == 0x00 || wrtieHistory == 0x31)
                            {
                                EnterDefaultSession();
                                return true;
                            }
                        }
                    }
                }

                EnterDefaultSession();
                m4 = new byte[] { };
                m5 = new byte[] { };
                wrtieHistory = 0xFF;
                return false;
            }
        }

        [Description("调试解锁Key")]
        public void PrivateTestKey()
        {
            {
                var ecuIdStruct = new EcuIdStruct
                {
                    EucIdHex = "0000017880002E001A104974210114A6",
                    MasterEcuKeySlot = "FF",
                    MasterEcuKeyM1Hex = "00000000000000000000000000000011",
                    MasterEcuKeyM2Hex = "A4BA2FEECF1A77BB8297C389E4DDCD6B7D064A15D79155FC3E965E0626AB8007",
                    MasterEcuKeyM3Hex = "416E2C2022CB51DF88911B23CA1B61D1",
                    MasterEcuKeyM4Hex = "003C00980A2031352D38375539334E11E1B015F005536BC8E70DE5D1C845BC3A",
                    MasterEcuKeyM5Hex = "28F8DC0D459D677921813CD8E51CCB36",
                    UnlockEcuKeySlot = "01",
                    UnlockEcuKeyM1Hex = "00000000000000000000000000000044",
                    UnlockEcuKeyM2Hex = "1BAFA3B95A64DF9B40B8FD17A47433E10171956A97F01D2CBC4F67D1816B139A",
                    UnlockEcuKeyM3Hex = "78C5D137C8D7DD65D59D9D1669AFAF8A",
                    UnlockEcuKeyM4Hex = "003C00980A2031352D38375539334E444E23F090F23DAC23963065D7BA3159F3",
                    UnlockEcuKeyM5Hex = "28F8DC0D459D677921813CD8E51CCB36",
                };

                //if (keySlot == "FF")
                {
                    var m1 = new List<byte>();
                    var m2 = new List<byte>();
                    var m3 = new List<byte>();

                    for (var i = 0; i < ecuIdStruct.MasterEcuKeyM1Hex.Length; i = i + 2)
                        m1.Add(
                                Convert.ToByte(string.Format("{0}{1}", ecuIdStruct.MasterEcuKeyM1Hex[i],
                                    ecuIdStruct.MasterEcuKeyM1Hex[i + 1]), 16));

                    for (var i = 0; i < ecuIdStruct.MasterEcuKeyM2Hex.Length; i = i + 2)
                        m2.Add(
                                 Convert.ToByte(string.Format("{0}{1}", ecuIdStruct.MasterEcuKeyM2Hex[i],
                                     ecuIdStruct.MasterEcuKeyM2Hex[i + 1]), 16));

                    for (var i = 0; i < ecuIdStruct.MasterEcuKeyM3Hex.Length; i = i + 2)
                        m3.Add(
                                Convert.ToByte(string.Format("{0}{1}", ecuIdStruct.MasterEcuKeyM3Hex[i],
                                    ecuIdStruct.MasterEcuKeyM3Hex[i + 1]), 16));

                    byte[] m4;
                    byte[] m5;
                    byte writeHistory;
                    if (UnlockSeedKey(0xFF, m1, m2, m3, out writeHistory, out m4, out m5))
                    {
                        if (writeHistory == 0x00 || writeHistory == 0x31)
                        {
                            Console.WriteLine("MasterKeySlot-0x{0} M4：{1}", ValueHelper.GetHextStr(0xFF), ValueHelper.GetHextStr(m4));
                            Console.WriteLine("MasterKeySlot-0x{0} M5：{1}", ValueHelper.GetHextStr(0xFF), ValueHelper.GetHextStr(m5));

                            if (writeHistory == 0x00)
                            {
                                ecuIdStruct.MasterEcuKeyM4Hex = ValueHelper.GetHextStr(m4).Replace(" ", "");
                                ecuIdStruct.MasterEcuKeyM5Hex = ValueHelper.GetHextStr(m5).Replace(" ", "");
                            }
                            else if (writeHistory == 0x31)
                            {
                                if (string.IsNullOrEmpty(ecuIdStruct.MasterEcuKeyM4Hex) || string.IsNullOrEmpty(ecuIdStruct.MasterEcuKeyM5Hex))
                                {
                                    Console.WriteLine("NG MasterKey已注入，但未从数据库中查询到数据");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("MasterKeySlot-0x{0} M4,M5注入失败,代码-{1}", ValueHelper.GetHextStr(0xFF), ValueHelper.GetHextStrWithOx(writeHistory));
                            Console.WriteLine("NG " + ValueHelper.GetHextStrWithOx(writeHistory));
                        }
                    }
                    else
                    {
                        Console.WriteLine(" MasterKey = NG");
                    }
                }
                //else if (keySlot == "01")
                {
                    var m1 = new List<byte>();
                    var m2 = new List<byte>();
                    var m3 = new List<byte>();

                    for (var i = 0; i < ecuIdStruct.UnlockEcuKeyM1Hex.Length; i = i + 2)
                        m1.Add(
                                Convert.ToByte(string.Format("{0}{1}", ecuIdStruct.UnlockEcuKeyM1Hex[i],
                                    ecuIdStruct.UnlockEcuKeyM1Hex[i + 1]), 16));

                    for (var i = 0; i < ecuIdStruct.UnlockEcuKeyM2Hex.Length; i = i + 2)
                        m2.Add(
                                 Convert.ToByte(string.Format("{0}{1}", ecuIdStruct.UnlockEcuKeyM2Hex[i],
                                     ecuIdStruct.UnlockEcuKeyM2Hex[i + 1]), 16));

                    for (var i = 0; i < ecuIdStruct.UnlockEcuKeyM3Hex.Length; i = i + 2)
                        m3.Add(
                                Convert.ToByte(string.Format("{0}{1}", ecuIdStruct.UnlockEcuKeyM3Hex[i],
                                    ecuIdStruct.UnlockEcuKeyM3Hex[i + 1]), 16));

                    byte[] m4;
                    byte[] m5;
                    byte writeHistory;
                    if (UnlockSeedKey(0x01, m1, m2, m3, out writeHistory, out m4, out m5))
                    {
                        if (writeHistory == 0x00 || writeHistory == 0x31)
                        {
                            Console.WriteLine("UnlockKeySlot-0x{0} M4：{1}", ValueHelper.GetHextStr(0x01), ValueHelper.GetHextStr(m4));
                            Console.WriteLine("UnlockKeySlot-0x{0} M5：{1}", ValueHelper.GetHextStr(0x01), ValueHelper.GetHextStr(m5));

                            if (writeHistory == 0x00)
                            {
                                ecuIdStruct.UnlockEcuKeyM4Hex = ValueHelper.GetHextStr(m4).Replace(" ", "");
                                ecuIdStruct.UnlockEcuKeyM5Hex = ValueHelper.GetHextStr(m5).Replace(" ", "");
                            }
                            else if (writeHistory == 0x31)
                            {
                                if (string.IsNullOrEmpty(ecuIdStruct.UnlockEcuKeyM4Hex) ||
                                    string.IsNullOrEmpty(ecuIdStruct.UnlockEcuKeyM5Hex))
                                {
                                    Console.WriteLine("NG UnlockKey已注入，但未从数据库中查询到数据");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("UnlockKeySlot-0x{0} M4,M5注入失败,代码-{1}", ValueHelper.GetHextStr(0x01), ValueHelper.GetHextStr(writeHistory));
                            Console.WriteLine("NG " + ValueHelper.GetHextStrWithOx(writeHistory));
                        }
                    }
                    else
                    {
                        Console.WriteLine("UnlockKey = NG");
                    }
                }
            }

            EnterDefaultSession();
        }

        #endregion

        internal class EcuIdStruct
        {
            public string EucIdHex;

            public string MasterEcuKeySlot;

            public string MasterEcuKeyM1Hex;
            public string MasterEcuKeyM2Hex;
            public string MasterEcuKeyM3Hex;
            public string MasterEcuKeyM4Hex;
            public string MasterEcuKeyM5Hex;

            public string UnlockEcuKeySlot;

            public string UnlockEcuKeyM1Hex;
            public string UnlockEcuKeyM2Hex;
            public string UnlockEcuKeyM3Hex;
            public string UnlockEcuKeyM4Hex;
            public string UnlockEcuKeyM5Hex;

            public string MacKeyInfo;

            public string RandomSeed;

            /// <summary>
            /// 经过SHA256算法得到的(32Byte)
            /// </summary>
            public string Sha256Total;

            /// <summary>
            /// 经过SHA256算法得到的(前20Byte)
            /// </summary>
            public string Sha256Sub20;

            public string IdCodeAscii;
            public string IdCode;

            public string TrackInfo;
        }
    }
}
