using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using CommonUtility;
using CommonUtility.BusLoader;

namespace Controller
{
    [Description("CAN-Product,防火墙")]
    internal class Firewall : ControllerBase
    {
        public CanBus DiagnosisCan;

        private uint _reqCanID = 0x258;
        private uint _recvCanId = 0x658;
        private uint _networkCanId = 0x629;
        private byte[] _networkDatas = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };

        public Firewall(string name)
            : base(name)
        {
            if (_networkThread != null)
            {
                _networkThread.Abort();
                _networkThread.Join();
            }
            _networkThread = new Thread(Can1NetWork);
            _networkThread.Start();
        }

        public void SetDiagnosisCanId(string reqCanId, string recvCanId)
        {
        }

        public void SetNetWorkCanData(uint canId, string datas)
        {
        }

        #region 周期帧

        private readonly object _can1Locker = new object();
        private int _periodCount;
        private bool _isSleeping = true;
        private bool _isInExtendedSession;
        private readonly Thread _networkThread;

        private void Can1NetWork()
        {
            while (_networkThread.IsAlive)
            {
                if (!_networkThread.IsAlive)
                    break;

                Thread.Sleep(20);

                _periodCount++;

                if (_periodCount > 50)
                    _periodCount = 0;

                try
                {
                    if (DiagnosisCan == null)
                        continue;

                    lock (_can1Locker)
                    {
                        if (!_isSleeping)
                        {
                            var lstPages = new List<CanBus.CanDataPackage>
                            {
                                new CanBus.CanDataPackage(
                                    _networkCanId,
                                    CanBus.CanProtocol.Can,
                                    CanBus.CanType.Standard,
                                    CanBus.CanFormat.Data,
                                    _networkDatas)
                            };

                            DiagnosisCan.SendCanDatas(lstPages.ToArray());
                        }

                        if (_isInExtendedSession && _periodCount == 50)
                            DiagnosisCan.SendStandardCanData(_networkCanId,
                                new byte[] { 0x02, 0x3E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        #endregion

        #region 版本读取

        [Description("R,引导程序零件号-01C0")]
        public string FblPartNo;
        [Description("R,应用程序零件号-01C1")]
        public string AppPartNo;
        [Description("R,配置文件1零件号-01C2")]
        public string Cali1PartNo;
        [Description("R,引导程序版本号-01D0")]
        public string FblVer;
        [Description("R,应用程序版本号-01D1")]
        public string AppVer;
        [Description("R,配置文件1版本号-01D2")]
        public string Cali1Ver;
        [Description("R,当前零件号-EndModulePartNumber-01CB")]
        public string EndModulePartNumber;
        [Description("R,当前版本号-当前版本号-01DB")]
        public string EndModuleAlphaCode;

        [Description("ReadF1C0-引导程序零件号")]
        public void ReadFblPartNo()
        {
            FblPartNo = string.Empty;
            FblPartNo = ValueHelper.GetDecimal(ReadDidViaCan(0x01, 0xC0).ToArray()).ToString();
        }

        [Description("Read01C1-应用程序零件号")]
        public void ReadAppPartNo()
        {
            AppPartNo = string.Empty;
            AppPartNo = ValueHelper.GetDecimal(ReadDidViaCan(0x01, 0xC1).ToArray()).ToString();
        }

        [Description("Read01C2-配置文件1零件号")]
        public void ReadCali1PartNo()
        {
            Cali1PartNo = string.Empty;
            Cali1PartNo = ValueHelper.GetDecimal(ReadDidViaCan(0x01, 0xC2).ToArray()).ToString();
        }

        [Description("Read01D0-引导程序版本号")]
        public void ReadFblVer()
        {
            FblVer = string.Empty;
            FblVer = ReadDidViaCan(0x01, 0xD0).GetStringByAsciiBytes(true);
        }

        [Description("Read01D1-应用程序版本号")]
        public void ReadAppVer()
        {
            AppVer = string.Empty;
            AppVer = ReadDidViaCan(0x01, 0xD1).GetStringByAsciiBytes(true);
        }

        [Description("Read01D2-配置文件1版本号")]
        public void ReadCali1Ver()
        {
            Cali1Ver = string.Empty;
            Cali1Ver = ReadDidViaCan(0x01, 0xD2).GetStringByAsciiBytes(true);
        }

        [Description("Read01CB-当前零件号-EndModulePartNumber")]
        public void ReadEndModulePartNumber()
        {
            EndModulePartNumber = string.Empty;
            EndModulePartNumber = ValueHelper.GetDecimal(ReadDidViaCan(0x01, 0xCB).ToArray()).ToString();
        }

        [Description("Read01DB-当前版本号-EndModulePartNumber")]
        public void ReadEndModuleAlphaCode()
        {
            EndModuleAlphaCode = string.Empty;
            EndModuleAlphaCode = ReadDidViaCan(0x01, 0xDB).GetStringByAsciiBytes(true);
        }

        private byte[] ReadDidViaCan(byte didHi, byte didLo)
        {
            if (DiagnosisCan == null)
                return new List<byte>().ToArray();

            lock (_can1Locker)
            {
                byte[] echoBytes;
                if (DiagnosisCan.CanBusWithUds.TryReadData(
                    _reqCanID,
                    _recvCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can, didHi, didLo, out echoBytes))
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
                if (!DiagnosisCan.CanBusWithUds.TryReadData(
                    _reqCanID,
                    _recvCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    didHi, didLo, out echoBytes))
                    return new List<byte>().ToArray();

                if (echoBytes != null)
                    return echoBytes;

                Thread.Sleep(250);
                if (!DiagnosisCan.CanBusWithUds.TryReadData(
                    _reqCanID,
                    _recvCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    didHi, didLo, out echoBytes))
                    return new List<byte>().ToArray();

                if (echoBytes != null)
                    return echoBytes;

                return new List<byte>().ToArray();

                Thread.Sleep(250);
                if (!DiagnosisCan.CanBusWithUds.TryReadData(
                    _reqCanID,
                    _recvCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    didHi, didLo, out echoBytes))
                    return new List<byte>().ToArray();
                return echoBytes ?? new List<byte>().ToArray();
            }
        }

        #endregion

        #region 控制

        [Description("休眠")]
        public void StopCanMsg()
        {
            _isSleeping = true;
        }

        [Description("唤醒")]
        public void StartCanMsg()
        {
            _isSleeping = false;
        }

        [Description("进入正常模式")]
        public void EnterDefaultSession()
        {
            if (DiagnosisCan == null)
                return;

            lock (_can1Locker)
            {
                _isInExtendedSession = false;

                if (DiagnosisCan.CanBusWithUds.TryEnterDefaultSession(
                    _reqCanID,
                    _recvCanId, CanBus.CanType.Standard))
                    return;

                Thread.Sleep(500);

                DiagnosisCan.CanBusWithUds.TryEnterDefaultSession(
                    _reqCanID,
                    _recvCanId, CanBus.CanType.Standard);
            }
        }

        [Description("进入拓展模式")]
        public void EnterExtendedSession()
        {
            if (DiagnosisCan == null)
                return;

            lock (_can1Locker)
            {
                if (DiagnosisCan.CanBusWithUds.TryEnterExtendedSession(
                    _reqCanID,
                    _recvCanId, CanBus.CanType.Standard))
                {
                    _isInExtendedSession = true;
                    return;
                }

                Thread.Sleep(500);
                if (DiagnosisCan.CanBusWithUds.TryEnterExtendedSession(
                    _reqCanID,
                    _recvCanId, CanBus.CanType.Standard))
                {
                    _isInExtendedSession = true;
                }
            }
        }

        [Description("进入编程模式")]
        public void EnterProgrammingSession()
        {
            if (DiagnosisCan == null)
                return;

            lock (_can1Locker)
            {
                if (DiagnosisCan.CanBusWithUds.TryEnterProgrammingSession(
                    _reqCanID,
                    _recvCanId, CanBus.CanType.Standard))
                {
                    _isInExtendedSession = true;
                    return;
                }

                Thread.Sleep(500);
                if (DiagnosisCan.CanBusWithUds.TryEnterProgrammingSession(
                    _reqCanID,
                    _recvCanId, CanBus.CanType.Standard))
                {
                    _isInExtendedSession = true;
                }
            }
        }

        [Description("在BBIN网络上发送二次0x620帧")]
        public void SendBbin()
        {
            if (DiagnosisCan == null)
                return;
            for (var i = 0; i < 2; i++)
                DiagnosisCan.SendStandardCanData(0x620, new List<byte> { 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00 });
        }

        [Description("根据二维码写入MTC和ECUID-F0B4和F0F3")]
        public void WriteMtcAndEcuIdByBarcode()
        {
            var mtc = "1A17108AAA000001";
            var mtcBytes = Encoding.ASCII.GetBytes(mtc);

            lock (_can1Locker)
            {
                DiagnosisCan.SendStandardCanData(_reqCanID, new List<byte> { 0x10, 0x12, 0x3B, 0xF3, 0x01, 0x02, 0x03, 0x04 });
                Thread.Sleep(5);
                DiagnosisCan.SendStandardCanData(_reqCanID, new List<byte> { 0x21, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B });
                Thread.Sleep(5);
                DiagnosisCan.SendStandardCanData(_reqCanID, new List<byte> { 0x22, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0xAA, 0xAA });
            }
        }

        #endregion
    }
}
