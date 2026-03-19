using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using CommonUtility;
using CommonUtility.BusLoader;

namespace Controller
{
    [Description("CAN-Product,SGM458座椅模块")]
    public sealed class Sgm458Msm : ControllerBase
    {
        public CanBus DiagnosticCan1;
        public CanBus AppCan2;
        public LinBus LedLin;
        public object MsmLin;

        public Sgm458Msm(string name)
            : base(name)
        {
            _motorLearnTasks.Add(new MotorLearnTask(MotorType.FrwdBkwdMotor));
            _motorLearnTasks.Add(new MotorLearnTask(MotorType.BkRclnMotor));
            _motorLearnTasks.Add(new MotorLearnTask(MotorType.CushnFrtMotor));
            _motorLearnTasks.Add(new MotorLearnTask(MotorType.CushnRrMotor));
            _motorLearnTasks.Add(new MotorLearnTask(MotorType.FtrstMotor));
            _motorLearnTasks.Add(new MotorLearnTask(MotorType.SeatArmscreenInOutMotor));
            _motorLearnTasks.Add(new MotorLearnTask(MotorType.SeatLeftwardRightwardMotor));
            _motorLearnTasks.Add(new MotorLearnTask(MotorType.SeatLegrestUpwardDownwardMotor));
            _motorLearnTasks.Add(new MotorLearnTask(MotorType.SeatCushionFoldMotor));
            CanBus.PushCanMsg += CanBus_PushCanMsg;

            if (_keepNetworkThread != null)
            {
                _keepNetworkThread.Abort();
                _keepNetworkThread.Join();
            }

            _keepNetworkThread = new Thread(KeepNetwork) { IsBackground = true };
            _keepNetworkThread.Start();

            if (_keepExtendedSessionThread != null)
            {
                _keepExtendedSessionThread.Abort();
                _keepExtendedSessionThread.Join();
            }

            _keepExtendedSessionThread = new Thread(KeepExtendedSession) { IsBackground = true };
            _keepExtendedSessionThread.Start();

            if (_keepLinNetworkThread != null)
            {
                _keepLinNetworkThread.Abort();
                _keepLinNetworkThread.Join();
            }

            _keepLinNetworkThread = new Thread(KeepLinNetwork) { IsBackground = true };
            _keepLinNetworkThread.Start();
        }

        ~Sgm458Msm()
        {
            Dispose();
        }

        private readonly Thread _keepExtendedSessionThread;
        private readonly Thread _keepNetworkThread;
        private readonly object _lockSend = new object();
        private bool _isInExtendedSession;
        private bool _isSleep = true;

        /// <summary>
        /// 诊断命令帧CANID
        /// </summary>
        private uint _canDiagnosisRequestPhyCanId = 0x14DAA6F1;

        /// <summary>
        /// 诊断响应帧CANID
        /// </summary>
        private uint _canDiagnosisResponseCanId = 0x14DAF1A6;

        /// <summary>
        /// Hmi诊断命令帧CANID
        /// </summary>
        private uint _hmiCanDiagnosisRequestPhyCanId = 0x14DA76F1;

        /// <summary>
        /// Hmi诊断响应帧CANID
        /// </summary>
        private uint _hmiCanDiagnosisResponseCanId = 0x14DAF176;

        private void KeepExtendedSession()
        {
            while (_keepExtendedSessionThread.IsAlive)
            {
                if (!_keepExtendedSessionThread.IsAlive)
                    break;

                Thread.Sleep(850);

                if (DiagnosticCan1 == null)
                    continue;

                if (!_isInExtendedSession)
                    continue;
                lock (_lockSend)
                {
                    DiagnosticCan1.SendExtendedCanData(_canDiagnosisRequestPhyCanId, CanBus.MyUds.KeepExtendedSessionBytes());
                    //Console.WriteLine(Name + "Keep session");
                }
            }
        }

        #region 座椅模式选择

        [Description("R,当前选择的产品类别")]
        public string SelectModeName = @"副驾驶";
        private SeatMode _selectedMode = SeatMode.SeatPa;

        //[Description("主驾驶")]
        public void SetDr()
        {
            SelectMode(SeatMode.SeatDr);
        }

        public void SetPa()
        {
            SelectMode(SeatMode.SeatPa);
        }

        [Description("2排L")]
        public void Set2L()
        {
            SelectMode(SeatMode.Seat2L);
        }

        [Description("2排R")]
        public void Set2R()
        {
            SelectMode(SeatMode.Seat2R);
        }

        [Description("3排L")]
        public void Set3L()
        {
            SelectMode(SeatMode.Seat3L);
        }

        [Description("3排R")]
        public void Set3R()
        {
            SelectMode(SeatMode.Seat3R);
        }

        public void SetHmi2L()
        {
            SelectMode(SeatMode.SeatDr);
        }

        public void SetHmi2R()
        {
            SelectMode(SeatMode.SeatDr);
        }

        private void SelectMode(SeatMode mode)
        {
            if (_isSleep && !_isInExtendedSession)
            {
                _selectedMode = mode;
                switch (_selectedMode)
                {
                    case SeatMode.SeatDr:
                        SlaveLinMatrix = new byte[] { 0x00, 0x00, 0x00, 0x00, 0xF0 };
                        SelectModeName = @"主驾驶";
                        _canDiagnosisRequestPhyCanId = 0x14DAA8F1;
                        _canDiagnosisResponseCanId = 0x14DAF1A8;
                        break;

                    case SeatMode.SeatPa:
                        SlaveLinMatrix = new byte[] { 0x00, 0x00, 0x00, 0x00, 0xF0 };
                        SelectModeName = @"副驾驶";
                        _canDiagnosisRequestPhyCanId = 0x14DAA6F1;
                        _canDiagnosisResponseCanId = 0x14DAF1A6;
                        break;

                    case SeatMode.Seat2L:
                        SlaveLinMatrix = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00 };
                        SelectModeName = @"二排左";
                        _canDiagnosisRequestPhyCanId = 0x14DAAAF1;
                        _canDiagnosisResponseCanId = 0x14DAF1AA;

                        _hmiCanDiagnosisRequestPhyCanId = 0x14DA76F1;
                        _hmiCanDiagnosisResponseCanId = 0x14DAF176;
                        break;

                    case SeatMode.Seat2R:
                        SlaveLinMatrix = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00 };
                        SelectModeName = @"二排右";
                        _canDiagnosisRequestPhyCanId = 0x14DAA7F1;
                        _canDiagnosisResponseCanId = 0x14DAF1A7;

                        _hmiCanDiagnosisRequestPhyCanId = 0x14DA77F1;
                        _hmiCanDiagnosisResponseCanId = 0x14DAF177;
                        break;

                    case SeatMode.Seat3L:
                        SelectModeName = @"三排左";
                        _canDiagnosisRequestPhyCanId = 0x14DAA5F1;
                        _canDiagnosisResponseCanId = 0x14DAF1A5;
                        break;

                    case SeatMode.Seat3R:
                        SelectModeName = @"三排右";
                        _canDiagnosisRequestPhyCanId = 0x14DAAEF1;
                        _canDiagnosisResponseCanId = 0x14DAF1AE;
                        break;

                    //case SeatMode.Hmi2L:
                    //    SelectModeName = @"二排触摸屏左";
                    //    _hmicanDiagnosisRequestPhyCanId = 0x14DA76F1;
                    //    _hmicanDiagnosisResponseCanId = 0x14DAF176; // 待定?
                    //    break;

                    //case SeatMode.Hmi2R:
                    //    SelectModeName = @"二排触摸屏右";
                    //    _hmicanDiagnosisRequestPhyCanId = 0x14DA77F1;
                    //    _hmicanDiagnosisResponseCanId = 0x14DAF177; // 待定?
                    //    break;

                    default:
                        throw new ArgumentOutOfRangeException("mode", mode, null);
                }
            }
        }

        internal enum SeatMode
        {
            SeatDr,
            SeatPa,
            Seat2L,
            Seat2R,
            Seat3L,
            Seat3R,
            //Hmi2L,
            //Hmi2R,
        }

        #endregion

        #region 唤醒休眠，session选择

        [Description("R,诊断-当前模式")]
        public string CurrentSession = @"DefaultSession";

        [Description("R,诊断-安全访问0D0E结果")]
        public string SecurityAccess0D0EResult;

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

        [Description("诊断-进入正常模式")]
        public void EnterDefaultSession()
        {
            if (DiagnosticCan1 == null)
                return;

            lock (_lockSend)
            {
                CurrentSession = @"DefaultSession";
                _isInExtendedSession = false;

                if (DiagnosticCan1.CanBusWithUds.TryEnterDefaultSession(_canDiagnosisRequestPhyCanId, _canDiagnosisResponseCanId, CanBus.CanType.Extended))
                    return;

                Thread.Sleep(500);

                DiagnosticCan1.CanBusWithUds.TryEnterDefaultSession(_canDiagnosisRequestPhyCanId, _canDiagnosisResponseCanId, CanBus.CanType.Extended);
            }
        }

        [Description("诊断-进入拓展模式")]
        public void EnterExtendedSession()
        {
            if (DiagnosticCan1 == null)
                return;

            lock (_lockSend)
            {
                if (DiagnosticCan1.CanBusWithUds.TryEnterExtendedSession(_canDiagnosisRequestPhyCanId, _canDiagnosisResponseCanId, CanBus.CanType.Extended))
                {
                    CurrentSession = @"ExtendedSession";
                    _isInExtendedSession = true;
                    return;
                }

                Thread.Sleep(500);
                if (DiagnosticCan1.CanBusWithUds.TryEnterExtendedSession(_canDiagnosisRequestPhyCanId, _canDiagnosisResponseCanId, CanBus.CanType.Extended))
                {
                    CurrentSession = @"ExtendedSession";
                    _isInExtendedSession = true;
                }
            }
        }

        [Description("诊断-进入编程模式")]
        public void EnterProgramSession()
        {
            if (DiagnosticCan1 == null)
                return;

            _isInExtendedSession = false;
            CurrentSession = @"ProgramSession";

            lock (_lockSend)
            {
                if (DiagnosticCan1.CanBusWithUds.TryEnterProgrammingSession(_canDiagnosisRequestPhyCanId, _canDiagnosisResponseCanId, CanBus.CanType.Extended))
                {
                    return;
                }

                Thread.Sleep(500);
                DiagnosticCan1.CanBusWithUds.TryEnterProgrammingSession(_canDiagnosisRequestPhyCanId, _canDiagnosisResponseCanId, CanBus.CanType.Extended);
            }
        }

        [Description("诊断-关闭正常通信")]
        public void DisableRxAndTxCommunication()
        {
            if (DiagnosticCan1 == null)
                return;

            lock (_lockSend)
            {
                DiagnosticCan1.CanBusWithUds.TryCommunicationControl(_canDiagnosisRequestPhyCanId, _canDiagnosisResponseCanId, CanBus.CanType.Extended, Uds14229Helper.CommunicationControl.DisableRxAndTx);
            }
        }

        [Description("诊断-软件复位")]
        public void EcuReset()
        {
            if (DiagnosticCan1 == null)
                return;

            lock (_lockSend)
            {
                DiagnosticCan1.SendExtendedCanData(_canDiagnosisRequestPhyCanId, new byte[] { 0x02, 0x11, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00 });
            }
        }

        [Description("诊断-安全访问0D0E")]
        public void SecurityAccess0D0E()
        {
            if (DiagnosticCan1 == null)
            {
                SecurityAccess0D0EResult = string.Empty;
                return;
            }

            const byte requesetSeedSubFunc = 0x0D;
            const byte sendKeySubunc = 0x0E;
            var keyBytes = new byte[] { 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0x00, 0x11, 0x22 };

            {
                SecurityAccess0D0EResult = string.Empty;

                lock (_lockSend)
                {
                    byte[] seedBytes;
                    if (DiagnosticCan1.CanBusWithUds.TryRequestSeed(_canDiagnosisRequestPhyCanId, _canDiagnosisResponseCanId, CanBus.CanType.Extended, requesetSeedSubFunc, out seedBytes))
                    {
                        if (DiagnosticCan1.CanBusWithUds.TrySendKey(_canDiagnosisRequestPhyCanId, _canDiagnosisResponseCanId, CanBus.CanType.Extended, sendKeySubunc, keyBytes))
                        {
                            SecurityAccess0D0EResult = "OK";
                            return;
                        }
                    }

                    SecurityAccess0D0EResult = "NG";
                }
            }

            if (SecurityAccess0D0EResult != "NG")
                return;
            {
                SecurityAccess0D0EResult = string.Empty;

                lock (_lockSend)
                {
                    byte[] seedBytes;
                    if (DiagnosticCan1.CanBusWithUds.TryRequestSeed(_canDiagnosisRequestPhyCanId, _canDiagnosisResponseCanId, CanBus.CanType.Extended, requesetSeedSubFunc, out seedBytes))
                    {
                        if (DiagnosticCan1.CanBusWithUds.TrySendKey(_canDiagnosisRequestPhyCanId, _canDiagnosisResponseCanId, CanBus.CanType.Extended, sendKeySubunc, keyBytes))
                        {
                            SecurityAccess0D0EResult = "OK";
                            return;
                        }
                    }

                    SecurityAccess0D0EResult = "NG";
                }
            }
        }

        #endregion

        #region 版本信息

        [Description("R,诊断-当前零件号-EndModulePartNumber-F1CB")]
        public string EndModulePartNumber;

        [Description("R,诊断-当前版本号-当前版本号-F1DB")]
        public string EndModuleAlphaCode;

        [Description("R,初始零件号-BaseModulePartNumber-F1CC")]
        public string BaseModulePartNumber;

        [Description("R,初始版本号-BaseModuleAlphaCode-F1DC")]
        public string BaseModuleAlphaCode;

        [Description("R,诊断-ApplicationSoftware-F181")]
        public string ApplicationSoftware;

        [Description("R,诊断-MEC-F1A0")]
        public int ManufacturersEnableCounter = -1;

        [Description("R,诊断-追溯信息-F0B4")]
        public string TrackInfo;
        [Description("R,诊断-ECU_ID-F0F3")]
        public string EcuId;

        [Description("R,诊断-当前座椅位置-FD16")]
        public string SeatPosConfig;

        [Description("R,诊断-当前座椅车型配置-FD18")]
        public string SeatNumConfig;

        [Description("诊断-ReadF1CB-当前零件号-EndModulePartNumber")]
        public void ReadEndModulePartNumber()
        {
            EndModulePartNumber = string.Empty;
            EndModulePartNumber = ValueHelper.GetDecimal(ReadDidViaCan(_canDiagnosisRequestPhyCanId, _canDiagnosisResponseCanId, 0xF1, 0xCB).ToArray()).ToString();
        }

        [Description("诊断-ReadF1DB-当前版本号-EndModuleAlphaCode")]
        public void ReadEndModuleAlphaCode()
        {
            EndModuleAlphaCode = string.Empty;
            EndModuleAlphaCode = ReadDidViaCan(_canDiagnosisRequestPhyCanId, _canDiagnosisResponseCanId, 0xF1, 0xDB).GetStringByAsciiBytes(true);
        }

        [Description("ReadF1CC-初始零件号-BaseModulePartNumber")]
        public void ReadBaseModulePartNumber()
        {
            BaseModulePartNumber = string.Empty;
            BaseModulePartNumber = ValueHelper.GetDecimal(ReadDidViaCan(_canDiagnosisRequestPhyCanId, _canDiagnosisResponseCanId, 0xF1, 0xCC).ToArray()).ToString();
        }

        [Description("ReadF1DC-初始版本号-BaseModuleAlphaCode")]
        public void ReadBaseModuleAlphaCode()
        {
            BaseModuleAlphaCode = string.Empty;
            BaseModuleAlphaCode = ReadDidViaCan(_canDiagnosisRequestPhyCanId, _canDiagnosisResponseCanId, 0xF1, 0xDC).GetStringByAsciiBytes(true);
        }

        [Description("诊断-ReadF0F3和F0B4-ECU_ID和追溯信息")]
        public void ReadEcuIdAndTrackInfo()
        {
            EcuId = string.Empty;
            TrackInfo = string.Empty;

            EcuId = ValueHelper.GetHextStr(ReadDidViaCan(_canDiagnosisRequestPhyCanId, _canDiagnosisResponseCanId, 0xF0, 0xF3)).Replace(" ", "");
            TrackInfo = ReadDidViaCan(_canDiagnosisRequestPhyCanId, _canDiagnosisResponseCanId, 0xF0, 0xB4).GetStringByAsciiBytes(true);
        }

        [Description("诊断-ReadF181-ApplicationSoftware")]
        public void ReadApplicationSoftware()
        {
            ApplicationSoftware = string.Empty;
            var result = ReadDidViaCan(_canDiagnosisRequestPhyCanId, _canDiagnosisResponseCanId, 0xF1, 0x81);
            if (result.Length <= 2)
                return;
            var versionBytes = new[] { result[result.Length - 2], result[result.Length - 1] };
            var noBytes = new List<byte>();
            for (var i = 0; i < result.Length - 2; i++)
                noBytes.Add(result[i]);

            ApplicationSoftware = ValueHelper.GetDecimal(noBytes.ToArray()) + versionBytes.GetStringByAsciiBytes(true);
        }

        [Description("诊断-写入MEC-F1A0")]
        public void WriteManufacturersEnableCounter(string mec)
        {
            ManufacturersEnableCounter = -1;

            if (DiagnosticCan1 == null)
                return;

            byte mecValue;
            if (!byte.TryParse(mec, out mecValue))
                return;
            var bs = new List<byte> { mecValue };

            lock (_lockSend)
                DiagnosticCan1.CanBusWithUds.TryWriteData(
                    _canDiagnosisRequestPhyCanId, _canDiagnosisResponseCanId, CanBus.CanType.Extended, CanBus.CanProtocol.Can, 0xF1, 0xA0, bs.ToArray());
        }

        [Description("诊断-读取MEC-F1A0")]
        public void ReadManufacturersEnableCounter()
        {
            ManufacturersEnableCounter = -1;

            if (DiagnosticCan1 == null)
                return;

            ManufacturersEnableCounter = ValueHelper.GetDecimal(ReadDidViaCan(_canDiagnosisRequestPhyCanId, _canDiagnosisResponseCanId, 0xF1, 0xA0));
        }

        [Description("诊断-写座椅位置-FD16")]
        public void WriteSeatPosConfig(string config)
        {
            // 写FD16 03 2L, 123456分别对应主驾，副驾，2L 2R 3L 3R
            // MSM位置默认为2L，
            // 通过发送2L座椅的FD16 2E服务配置BootLoader及App的六合一，
            // 等待软件回复肯定响应后（如果出现否定响应，则说明配置失败，需重新发送FD16进行重新配置），FD16肯定响应后，需发送11  03（Soft Reset）进行软件服务，
            // 再通过功能寻址的FD16 22服务读取当前配置是否为需要配置的座椅，如果读取非当前需要配置的座椅，则说明配置过程出问题，需要重新进行配置。
            // 仅当此值为255时，允许通过此DID写入修改，读没有限制

            if (DiagnosticCan1 == null)
                return;

            int value;
            if (!int.TryParse(config, out value))
                return;
            if (value < 1 || value > 6)
                return;
            lock (_lockSend)
                DiagnosticCan1.CanBusWithUds.TryWriteData(
                    _canDiagnosisRequestPhyCanId, _canDiagnosisResponseCanId, CanBus.CanType.Extended, CanBus.CanProtocol.Can, 0xFD, 0x16, new[] { (byte)value });
        }

        [Description("诊断-读座椅位置-FD16")]
        public void ReadSeatPosConfig()
        {
            SeatPosConfig = string.Empty;
            SeatPosConfig = ValueHelper.GetDecimal(ReadDidViaCan(_canDiagnosisRequestPhyCanId, _canDiagnosisResponseCanId, 0xFD, 0x16).ToArray()).ToString();
        }

        [Description("诊断-写座椅位置-FD16")]
        public void WriteSeatNumConfig(string config)
        {
            //"读写车型配置的外部标定：
            //KeSMS_e_SeatNumConfig=$0 四座非隔断版
            //KeSMS_e_SeatNumConfig=$1 六座版；
            //KeSMS_e_SeatNumConfig=$2 七座版；
            //KeSMS_e_SeatNumConfig=$3 四座隔断版；
            //默认值$255，仅当此值为255时，允许通过此DID进行写入修改，读没有限制"
            // 仅当此值为255时，允许通过此DID写入修改，读没有限制

            if (DiagnosticCan1 == null)
                return;

            int value;
            if (!int.TryParse(config, out value))
                return;
            if (value < 1 || value > 6)
                return;
            lock (_lockSend)
                DiagnosticCan1.CanBusWithUds.TryWriteData(_canDiagnosisRequestPhyCanId, _canDiagnosisResponseCanId, CanBus.CanType.Extended, CanBus.CanProtocol.Can, 0xFD, 0x18, new[] { (byte)value });
        }

        [Description("诊断-读座椅车型配置-FD18")]
        public void ReadSeatNumConfig()
        {
            SeatNumConfig = string.Empty;
            SeatNumConfig = ValueHelper.GetDecimal(ReadDidViaCan(_canDiagnosisRequestPhyCanId, _canDiagnosisResponseCanId, 0xFD, 0x18).ToArray()).ToString();
        }

        private byte[] ReadDidViaCan(uint reqCanId, uint recvCanId, byte didHi, byte didLo,
                int timeoutFromMilliseconds = 500)
        {
            if (DiagnosticCan1 == null)
                return new List<byte>().ToArray();

            lock (DiagnosticCan1)
            {
                byte[] echoBytes;
                if (DiagnosticCan1.CanBusWithUds.TryReadData(
                    reqCanId, recvCanId, CanBus.CanType.Extended, CanBus.CanProtocol.Can, didHi, didLo, out echoBytes, timeoutFromMilliseconds: timeoutFromMilliseconds))
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
                if (!DiagnosticCan1.CanBusWithUds.TryReadData(
                    reqCanId, recvCanId, CanBus.CanType.Extended, CanBus.CanProtocol.Can, didHi, didLo, out echoBytes, timeoutFromMilliseconds: timeoutFromMilliseconds))
                    return new List<byte>().ToArray();

                if (echoBytes != null)
                    return echoBytes;

                Thread.Sleep(250);
                if (!DiagnosticCan1.CanBusWithUds.TryReadData(
                    reqCanId, recvCanId, CanBus.CanType.Standard, CanBus.CanProtocol.Can, didHi, didLo, out echoBytes, timeoutFromMilliseconds: timeoutFromMilliseconds))
                    return new List<byte>().ToArray();

                if (echoBytes != null)
                    return echoBytes;

                return new List<byte>().ToArray();

                Thread.Sleep(250);
                if (!DiagnosticCan1.CanBusWithUds.TryReadData(reqCanId, recvCanId, CanBus.CanType.Extended, CanBus.CanProtocol.Can, didHi, didLo, out echoBytes))
                    return new List<byte>().ToArray();
                return echoBytes ?? new List<byte>().ToArray();
            }
        }

        #endregion

        #region HMI版本信息

        [Description("R,HMI-检测key写入成功flag")]
        public string HmiKeyFlag = string.Empty;
        [Description("R,HMI-ECU_ID")]
        public string HmiEcuId = string.Empty;
        [Description("R,HMI-AppPartNumber")]
        public string HmiAppPartNumber = string.Empty;
        [Description("R,HMI-AppAlphaCode")]
        public string HmiAppAlphaCode = string.Empty;
        [Description("R,HMI-BaseModulePartNumber")]
        public string HmiBaseModulePartNumber = string.Empty;
        [Description("R,HMI-BaseModuleAlphaCode")]
        public string HmiBaseModuleAlphaCode = string.Empty;
        [Description("R,HMI-EndModulePartNumber")]
        public string HmiEndModulePartNumber = string.Empty;
        [Description("R,HMI-EndModuleAlphaCode")]
        public string HmiEndModuleAlphaCode = string.Empty;
        [Description("R,HMI-Mec")]
        public double HmiMec = -1;
        [Description("R,HMI-Cvpps")]
        public string HmiCvpps = string.Empty;
        [Description("R,HMI-Duns")]
        public string HmiDuns = string.Empty;
        [Description("R,HMI-Mtc")]
        public string HmiMtc = string.Empty;

        [Description("HMI-检测key写入成功flag")]
        public void ReadKeyFlag()
        {
            HmiKeyFlag = "NG";
            if (DiagnosticCan1 == null)
                return;

            if (_selectedMode == SeatMode.Seat2L)
            {
                DiagnosticCan1.AddDoNotFilterCanId(0x497);
            }
            else if (_selectedMode == SeatMode.Seat2R)
            {
                DiagnosticCan1.AddDoNotFilterCanId(0x498);
            }
            else
            {
                return;
            }

            Thread.Sleep(5000);

            if (_selectedMode == SeatMode.Seat2L)
            {
                var tempData = DiagnosticCan1.CanRecvDataPackages.FindAll(f => f.CanId == 0x497 && f.CanDataLen >= 3);

                if (tempData.Count > 3)
                {
                    var d1 = tempData[0];
                    var d2 = tempData[1];
                    var d3 = tempData[2];

                    if (d1.CanData[0] == 0x00 && d1.CanData[1] == 0x00 && d1.CanData[2] == 0x00 &&
                        d2.CanData[0] == 0x00 && d2.CanData[1] == 0x00 && d2.CanData[2] == 0x00 &&
                        d3.CanData[0] == 0x00 && d3.CanData[1] == 0x00 && d3.CanData[2] == 0x00)
                    {
                        HmiKeyFlag = "NG";
                    }
                    else
                    {
                        HmiKeyFlag = "OK";
                    }
                }
            }
            else if (_selectedMode == SeatMode.Seat2R)
            {
                var tempData = DiagnosticCan1.CanRecvDataPackages.FindAll(f => f.CanId == 0x498 && f.CanDataLen >= 3);

                if (tempData.Count > 3)
                {
                    var d1 = tempData[0];
                    var d2 = tempData[1];
                    var d3 = tempData[2];

                    if (d1.CanData[0] == 0x00 && d1.CanData[1] == 0x00 && d1.CanData[2] == 0x00 &&
                        d2.CanData[0] == 0x00 && d2.CanData[1] == 0x00 && d2.CanData[2] == 0x00 &&
                        d3.CanData[0] == 0x00 && d3.CanData[1] == 0x00 && d3.CanData[2] == 0x00)
                    {
                        HmiKeyFlag = "NG";
                    }
                    else
                    {
                        HmiKeyFlag = "OK";
                    }
                }
            }

            DiagnosticCan1.RemoveDoNotFilterCanId(0x497);
            DiagnosticCan1.RemoveDoNotFilterCanId(0x498);
        }

        [Description("HMI-ECU_ID")]
        public void ReadHmiEcuId()
        {
            HmiEcuId = string.Empty;
            HmiEcuId = ValueHelper.GetHextStr(ReadDidViaCan(_hmiCanDiagnosisRequestPhyCanId, _hmiCanDiagnosisResponseCanId, 0xF0, 0xF3)).Replace(" ", "");
        }

        [Description("HMI-AppPartNumber")]
        public void ReadHmiAppPartNo()
        {
            HmiAppPartNumber = string.Empty;
            var result = ReadDidViaCan(_hmiCanDiagnosisRequestPhyCanId, _hmiCanDiagnosisResponseCanId, 0xF1, 0x81);
            if (result.Length <= 2)
                return;
            var versionBytes = new[] { result[result.Length - 2], result[result.Length - 1] };
            var noBytes = new List<byte>();
            for (var i = 0; i < result.Length - 2; i++)
                noBytes.Add(result[i]);

            HmiAppPartNumber = ValueHelper.GetDecimal(noBytes.ToArray()) + versionBytes.GetStringByAsciiBytes(true);
        }

        [Description("HMI-AppAlphaCode")]
        public void ReadHmiAppAlphaCode()
        {
            HmiAppAlphaCode = string.Empty;
            HmiAppAlphaCode = ReadDidViaCan(_hmiCanDiagnosisRequestPhyCanId, _hmiCanDiagnosisResponseCanId, 0xF1, 0xD1).GetStringByAsciiBytes(true);
        }

        [Description("HMI-BaseModulePartNumber")]
        public void ReadHmiBaseModulePartNumber()
        {
            HmiBaseModulePartNumber = string.Empty;
            HmiBaseModulePartNumber = ValueHelper.GetDecimal(ReadDidViaCan(_hmiCanDiagnosisRequestPhyCanId, _hmiCanDiagnosisResponseCanId, 0xF1, 0xCC).ToArray()).ToString();
        }

        [Description("HMI-BaseModuleAlphaCode")]
        public void ReadHmiBaseModuleAlphaCode()
        {
            HmiBaseModuleAlphaCode = string.Empty;
            HmiBaseModuleAlphaCode = ReadDidViaCan(_hmiCanDiagnosisRequestPhyCanId, _hmiCanDiagnosisResponseCanId, 0xF1, 0xDC).GetStringByAsciiBytes(true);
        }

        [Description("HMI-EndModulePartNumber")]
        public void ReadHmiEndModulePartNumber()
        {
            HmiEndModulePartNumber = string.Empty;
            HmiEndModulePartNumber = ValueHelper.GetDecimal(ReadDidViaCan(_hmiCanDiagnosisRequestPhyCanId, _hmiCanDiagnosisResponseCanId, 0xF1, 0xCB).ToArray()).ToString();
        }

        [Description("HMI-EndModuleAlphaCode")]
        public void ReadHmiEndModuleAlphaCode()
        {
            HmiEndModuleAlphaCode = string.Empty;
            HmiEndModuleAlphaCode = ReadDidViaCan(_hmiCanDiagnosisRequestPhyCanId, _hmiCanDiagnosisResponseCanId, 0xF1, 0xDB).GetStringByAsciiBytes(true);
        }

        [Description("诊断-Hmi写入MEC-F1A0")]
        public void WriteHmiManufacturersEnableCounter(string mec)
        {
            HmiMec = -1;

            if (DiagnosticCan1 == null)
                return;

            byte mecValue;
            if (!byte.TryParse(mec, out mecValue))
                return;
            var bs = new List<byte> { mecValue };

            lock (_lockSend)
                DiagnosticCan1.CanBusWithUds.TryWriteData(
                    _hmiCanDiagnosisRequestPhyCanId, _hmiCanDiagnosisResponseCanId, CanBus.CanType.Extended, CanBus.CanProtocol.Can, 0xF1, 0xA0, bs.ToArray());
        }

        [Description("诊断-Hmi读取MEC-F1A0")]
        public void ReadHmiManufacturersEnableCounter()
        {
            HmiMec = -1;

            if (DiagnosticCan1 == null)
                return;

            HmiMec = ValueHelper.GetDecimal(ReadDidViaCan(_hmiCanDiagnosisRequestPhyCanId, _hmiCanDiagnosisResponseCanId, 0xF1, 0xA0));
        }

        [Description("HMI-Cvpps")]
        public void ReadHmiCvpps()
        {
            HmiCvpps = string.Empty;
            HmiCvpps = ReadDidViaCan(_hmiCanDiagnosisRequestPhyCanId, _hmiCanDiagnosisResponseCanId, 0xF0, 0xAB).GetStringByAsciiBytes(true);
        }

        [Description("HMI-Duns")]
        public void ReadHmiDuns()
        {
            HmiDuns = string.Empty;
            HmiDuns = ReadDidViaCan(_hmiCanDiagnosisRequestPhyCanId, _hmiCanDiagnosisResponseCanId, 0xF0, 0xB3).GetStringByAsciiBytes(true);
        }

        [Description("HMI-Mtc")]
        public void ReadHmiMtc()
        {
            HmiMtc = string.Empty;
            HmiMtc = ValueHelper.GetHextStr(ReadDidViaCan(_hmiCanDiagnosisRequestPhyCanId, _hmiCanDiagnosisResponseCanId, 0xF0, 0xB4)).Replace(" ", "");
        }

        #endregion

        #region 无线充电

        [Description("R,无线充电状态")]
        public string WirelessChargingSystemChargingStatus = string.Empty;

        [Description("读取无线充电状态")]
        public void ReadWirelessChargingSystemChargingStatus()
        {
            WirelessChargingSystemChargingStatus = string.Empty;

            if (DiagnosticCan1 == null)
                return;

            if (_selectedMode == SeatMode.Seat2L)
            {
                DiagnosticCan1.AddDoNotFilterCanId(0x227);
            }
            else if (_selectedMode == SeatMode.Seat2R)
            {
                DiagnosticCan1.AddDoNotFilterCanId(0x226);
            }
            else
            {
                return;
            }

            Thread.Sleep(1500);

            if (_selectedMode == SeatMode.Seat2L)
            {
                var tempData = DiagnosticCan1.CanRecvDataPackages.FindLast(f => f.CanId == 0x227 && f.CanDataLen >= 1);

                if (tempData != null)
                {
                    var b1 = Convert.ToString(tempData.CanData[1], 2).PadLeft(8, '0').ToCharArray().Reverse().ToList();
                    var b2 = Convert.ToString(tempData.CanData[2], 2).PadLeft(8, '0').ToCharArray().Reverse().ToList();
                    var bits = string.Format("00000{0}{1}{2}", b1[0], b2[7], b2[6]);
                    WirelessChargingSystemChargingStatus = Convert.ToByte(bits, 2).ToString();
                }
            }
            else if (_selectedMode == SeatMode.Seat2R)
            {
                var tempData = DiagnosticCan1.CanRecvDataPackages.FindLast(f => f.CanId == 0x226 && f.CanDataLen >= 1);

                if (tempData != null)
                {
                    var b1 = Convert.ToString(tempData.CanData[1], 2).PadLeft(8, '0').ToCharArray().Reverse().ToList();
                    var b2 = Convert.ToString(tempData.CanData[2], 2).PadLeft(8, '0').ToCharArray().Reverse().ToList();
                    var bits = string.Format("00000{0}{1}{2}", b1[0], b2[7], b2[6]);
                    WirelessChargingSystemChargingStatus = Convert.ToByte(bits, 2).ToString();
                }
            }

            DiagnosticCan1.RemoveDoNotFilterCanId(0x226);
            DiagnosticCan1.RemoveDoNotFilterCanId(0x227);
        }

        #endregion

        #region 二排读按键信号 VIP/RESET /EZE开关信号

        private bool _isStartReadRow2InputSignal;

        [Description("R,Row2Vip")]
        public string Row2Vip = string.Empty;
        [Description("R,Row2Reset")]
        public string Row2Reset = string.Empty;
        [Description("R,Row2Eze")]
        public string Row2Eze = string.Empty;

        [Description("开始二排读按键信号")]
        public void StartReadRow2InputSignal()
        {
            Row2Vip = string.Empty;
            Row2Reset = string.Empty;
            Row2Eze = string.Empty;

            _isStartReadRow2InputSignal = true;
        }

        [Description("停止二排读按键信号")]
        public void StopReadRow2InputSignal()
        {
            _isStartReadRow2InputSignal = false;

            Row2Vip = string.Empty;
            Row2Reset = string.Empty;
            Row2Eze = string.Empty;
        }

        #endregion

        #region 三排读SBR STATUS

        [Description("R,Sbr1Status")]
        public string Sbr1Status = string.Empty;
        [Description("R,Sbr2Status")]
        public string Sbr2Status = string.Empty;

        [Description("读Sbr1Status")]
        public void ReadSbr1Status()
        {
            Sbr1Status = string.Empty;
            var result = ReadDidViaCan(_canDiagnosisRequestPhyCanId, _canDiagnosisResponseCanId, 0xFD, 0x14);
            if (result != null && result.Length >= 2)
            {
                Sbr1Status = ValueHelper.GetHextStr(result[0]);
            }
        }

        [Description("读Sbr2Status")]
        public void ReadSbr2Status()
        {
            Sbr2Status = string.Empty;
            var result = ReadDidViaCan(_canDiagnosisRequestPhyCanId, _canDiagnosisResponseCanId, 0xFD, 0x14);
            if (result != null && result.Length >= 2)
            {
                Sbr2Status = ValueHelper.GetHextStr(result[1]);
            }
        }

        #endregion

        #region DTC相关

        [Description("R,诊断-读取DTC结果")]
        public string ReadDtcResult;

        [Description("R/W,诊断-DTC黑名单文件路径")]
        public string DtcBlcakListFilePath =
            @"E:\Projects-2022\延锋-458座椅模块\模块资料-20230220\副驾驶DTC黑名单.txt";

        [Description("R,诊断-DTC白名单文件路径")]
        public string DtcWhtieListFilePath;

        private readonly List<Uds14229Helper.DtcData> _dtcBlackList = new List<Uds14229Helper.DtcData>();
        private readonly List<Uds14229Helper.DtcData> _dtcWhiteList = new List<Uds14229Helper.DtcData>();

        [Description("从配置文件中读取DTC黑名单")]
        public void ReadDtcBlackListFromFile()
        {
            _dtcBlackList.Clear();

            if (string.IsNullOrEmpty(DtcBlcakListFilePath) || !File.Exists(DtcBlcakListFilePath))
                return;
            foreach (var l in File.ReadAllLines(DtcBlcakListFilePath))
            {
                if (string.IsNullOrEmpty(l)) continue;
                var sp = l.Split(',');
                if (sp.Length != 2) continue;
                var code = sp[0];
                var failure = sp[1];
                _dtcBlackList.Add(new Uds14229Helper.DtcData(code, failure));
            }
        }

        [Description("从配置文件中读取DTC白名单")]
        public void ReadDtcWhiteListFromFile()
        {
            _dtcWhiteList.Clear();

            if (string.IsNullOrEmpty(DtcWhtieListFilePath) || !File.Exists(DtcWhtieListFilePath))
                return;
            foreach (var l in File.ReadAllLines(DtcWhtieListFilePath))
            {
                if (string.IsNullOrEmpty(l)) continue;
                var sp = l.Split(',');
                if (sp.Length != 2) continue;
                var code = sp[0];
                var failure = sp[1];
                _dtcBlackList.Add(new Uds14229Helper.DtcData(code, failure));
            }
        }

        [Description("诊断-读取黑名单中的DTC")]
        public void ReadBlackListDtc(string dtcStatusMask)
        {
            ReadDtcResult = string.Empty;

            if (DiagnosticCan1 == null)
                return;

            byte byteDtcStatusMask;
            try
            {
                byteDtcStatusMask = Convert.ToByte(dtcStatusMask, 16);
            }
            catch (Exception)
            {
                byteDtcStatusMask = 0x09;
            }

            lock (_lockSend)
            {
                byte[] echo;
                if (DiagnosticCan1.CanBusWithUds.TryReadDtcInfomation(
                    _canDiagnosisRequestPhyCanId,
                    _canDiagnosisResponseCanId,
                    CanBus.CanType.Extended, 0x02, byteDtcStatusMask,
                    out echo))
                {
                    if (echo != null)
                    {
                        // DTC CODE: B1DEF, (10)01110111101111
                        // CODE: 9DEF, 1001110111101111
                        // Failure Type: 0x13, dtc low byte

                        //ReadDtcResult = ValueHelper.GetHextStr(echo);

                        if (echo.Length % 4 == 0)
                        {
                            for (var i = 0; i < echo.Length; i = i + 4)
                            {
                                var dtcData = new Uds14229Helper.DtcData(echo[i], echo[i + 1], echo[i + 2], echo[i + 3]);
                                //Console.WriteLine(dtcData.Remark);

                                var findBlack =
                                    _dtcBlackList.FindAll(
                                        f => f.Code == dtcData.Code && f.FailureType == dtcData.FailureType);
                                if (findBlack.Any())
                                {
                                    foreach (var t in findBlack)
                                    {
                                        ReadDtcResult += string.Format("{0}:{1};", t.Code, t.FailureType);
                                    }
                                }
                            }

                            if (string.IsNullOrEmpty(ReadDtcResult))
                            {
                                ReadDtcResult = "NoError";
                            }
                        }
                        else
                        {
                            ReadDtcResult = "ReadDtcResLenError";
                        }
                    }
                }
                else
                {
                    ReadDtcResult = "NoRead";
                }
            }
        }

        [Description("诊断-读取非白名单中的DTC")]
        public void ReadWhiteListDtc(string dtcStatusMask)
        {
            ReadDtcResult = string.Empty;

            if (DiagnosticCan1 == null)
                return;

            byte byteDtcStatusMask;
            try
            {
                byteDtcStatusMask = Convert.ToByte(dtcStatusMask, 16);
            }
            catch (Exception)
            {
                byteDtcStatusMask = 0x09;
            }

            lock (_lockSend)
            {
                byte[] echo;
                if (DiagnosticCan1.CanBusWithUds.TryReadDtcInfomation(
                    _canDiagnosisRequestPhyCanId,
                    _canDiagnosisResponseCanId,
                    CanBus.CanType.Extended, 0x02, byteDtcStatusMask,
                    out echo))
                {
                    if (echo != null)
                    {
                        // DTC CODE: B1DEF, (10)01110111101111
                        // CODE: 9DEF, 1001110111101111
                        // Failure Type: 0x13, dtc low byte

                        //ReadDtcResult = ValueHelper.GetHextStr(echo);

                        if (echo.Length % 4 == 0)
                        {
                            for (var i = 0; i < echo.Length; i = i + 4)
                            {
                                var dtcData = new Uds14229Helper.DtcData(echo[i], echo[i + 1], echo[i + 2], echo[i + 3]);
                                //Console.WriteLine(dtcData.Remark);

                                var findWhiteIndex =
                                    _dtcWhiteList.FindIndex(
                                        f => f.Code == dtcData.Code && f.FailureType == dtcData.FailureType);
                                if (findWhiteIndex != -1)
                                {
                                    ReadDtcResult += string.Format("{0}:{1};", dtcData.Code, dtcData.FailureType);
                                }
                            }

                            if (string.IsNullOrEmpty(ReadDtcResult))
                            {
                                ReadDtcResult = "NoError";
                            }
                        }
                        else
                        {
                            ReadDtcResult = "ReadDtcResLenError";
                        }
                    }
                }
                else
                {
                    ReadDtcResult = "NoRead";
                }
            }
        }
        #endregion

        #region 功能应用

        /// <summary>
        /// 座椅加热
        /// </summary>
        private bool _stHtCshnDtyCycl;

        /// <summary>
        /// 靠背加热
        /// </summary>
        private bool _stHtBkDtyCycl;

        /// <summary>
        /// 座椅通风
        /// </summary>
        private bool _stVntDtyCycl;

        /// <summary>
        /// 脚蹬向下
        /// </summary>
        private bool _secRwRtStFtDnwdHmi2RReq;

        /// <summary>
        /// 脚蹬向上
        /// </summary>
        private bool _secRwRtStFtUpwdHmi2RReq;

        /// <summary>
        /// Boss键
        /// 滑道向前
        /// </summary>
        private bool _bosSwFrwdHmi2RReq;

        /// <summary>
        /// Boss键
        /// 滑道向后
        /// </summary>
        private bool _bosSwBkwdHmi2RReq;

        /// <summary>
        /// Boss键
        /// 靠背向上
        /// </summary>
        private bool _bosSwStBkReclnUpwdHmi2RReq;

        /// <summary>
        /// Boss键
        /// 靠背向下
        /// </summary>
        private bool _bosSwStBkReclnDnwdHmi2RReq;

        private void KeepNetwork()
        {
            while (_keepNetworkThread.IsAlive)
            {
                if (!_keepNetworkThread.IsAlive)
                    break;

                Thread.Sleep(100);

                if (AppCan2 == null)
                    continue;

                if (_isSleep)
                    continue;

                lock (_lockSend)
                {
                    var canMsgs = new List<CanBus.CanDataPackage>
                    {
                        new CanBus.CanDataPackage(0x621, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                            CanBus.CanFormat.Data, new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00}) ,// BCM
                         //new CanBus.CanDataPackage(0x111, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                         //   CanBus.CanFormat.Data, new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00}) // 0x1111
                    };

                    if (_selectedMode == SeatMode.Seat2L || _selectedMode == SeatMode.Seat2R)
                    {
                        canMsgs.Add(new CanBus.CanDataPackage(0x111, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                            CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00 }));

                        if (_selectedMode == SeatMode.Seat2R)
                        {
                            //_is腰托按摩电机向上 = false;
                            //_is腰托按摩电机向下 = false;
                            //_is腰托按摩电机向前 = false;
                            //_is腰托按摩电机向后 = true;
                            //_is腰托按摩电机按摩 = false;
                            //_is腰托按摩电机向后 = true;
                            if (_is腰托按摩电机向前)
                            {
                                canMsgs.Add(new CanBus.CanDataPackage(0X383, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                               CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00 }));
                            }
                            else if (_is腰托按摩电机向后)
                            {
                                canMsgs.Add(new CanBus.CanDataPackage(0X383, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                               CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00 }));
                            }
                            else if (_is腰托按摩电机向上)
                            {
                                canMsgs.Add(new CanBus.CanDataPackage(0X383, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                               CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x01, 0x80, 0x00, 0x00, 0x00, 0x00 }));
                            }
                            else if (_is腰托按摩电机向下)
                            {
                                canMsgs.Add(new CanBus.CanDataPackage(0X383, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                               CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00 }));
                            }
                            else if (_is腰托按摩电机按摩)
                            {
                                canMsgs.Add(new CanBus.CanDataPackage(0X383, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                               CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x00, 0x1b, 0x00, 0x00, 0x00, 0x00 }));
                            }
                            else
                            {
                                canMsgs.Add(new CanBus.CanDataPackage(0X383, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                               CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00 }));
                            }
                        }
                        else if(_selectedMode == SeatMode.Seat2L)
                        {
                            //_is腰托按摩电机向上 = false;
                            //_is腰托按摩电机向下 = false;
                            //_is腰托按摩电机向前 = false;
                            //_is腰托按摩电机向后 = true;
                            //_is腰托按摩电机按摩 = false;
                            //_is腰托按摩电机向后 = true;
                            if (_is腰托按摩电机向前)
                            {
                                canMsgs.Add(new CanBus.CanDataPackage(0X382, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                               CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00 }));
                            }
                            else if (_is腰托按摩电机向后)
                            {
                                canMsgs.Add(new CanBus.CanDataPackage(0X382, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                               CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00 }));
                            }
                            else if (_is腰托按摩电机向上)
                            {
                                canMsgs.Add(new CanBus.CanDataPackage(0X382, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                               CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x01, 0x80, 0x00, 0x00, 0x00, 0x00 }));
                            }
                            else if (_is腰托按摩电机向下)
                            {
                                canMsgs.Add(new CanBus.CanDataPackage(0X382, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                               CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00 }));
                            }
                            else if (_is腰托按摩电机按摩)
                            {
                                canMsgs.Add(new CanBus.CanDataPackage(0X382, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                               CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x00, 0x1b, 0x00, 0x00, 0x00, 0x00 }));
                            }
                            else
                            {
                                canMsgs.Add(new CanBus.CanDataPackage(0X382, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                               CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00 }));
                            }
                        }
                    }
                    //else
                    //{
                    //    canMsgs.Add(new CanBus.CanDataPackage(0x111, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                    //         CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0X00, 0x00 }));

                    //}

                    #region 主驾驶、副驾驶
                    if (_selectedMode == SeatMode.SeatDr || _selectedMode == SeatMode.SeatPa) // 主驾驶、副驾驶
                    {
                        var data429 = new byte[6];
                        var dataSecondRowRightSeatArmrestScreenHmiRequest = new CanCommunicationMatrix.MotorolaMatrix(0x382, 8);
                        if (_selectedMode == SeatMode.SeatDr)
                        {
                            if (_stHtCshnDtyCycl)
                                data429[0] = 0xE6;
                            if (_stHtBkDtyCycl)
                                data429[1] = 0xE6;
                            if (_stVntDtyCycl)
                                data429[2] = 0xE6;

                            dataSecondRowRightSeatArmrestScreenHmiRequest.CanId = 0x382;
                            if (_secRwRtStFtDnwdHmi2RReq && !_secRwRtStFtUpwdHmi2RReq)
                                dataSecondRowRightSeatArmrestScreenHmiRequest.MatrixData[3] = 0x80;
                            else if (!_secRwRtStFtDnwdHmi2RReq && _secRwRtStFtUpwdHmi2RReq)
                                dataSecondRowRightSeatArmrestScreenHmiRequest.MatrixData[3] = 0x40;
                            else if (!_secRwRtStFtDnwdHmi2RReq && !_secRwRtStFtUpwdHmi2RReq)
                                dataSecondRowRightSeatArmrestScreenHmiRequest.MatrixData[3] = 0x00;

                            canMsgs.Add(new CanBus.CanDataPackage(dataSecondRowRightSeatArmrestScreenHmiRequest.CanId,
                               CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data,
                               dataSecondRowRightSeatArmrestScreenHmiRequest.MatrixData));
                        }
                        else if (_selectedMode == SeatMode.SeatPa)
                        {
                            if (_stHtCshnDtyCycl)
                                data429[3] = 0xE6;
                            if (_stHtBkDtyCycl)
                                data429[4] = 0xE6;
                            if (_stVntDtyCycl)
                                data429[5] = 0xE6;

                            //var data383 = new CanCommunicationMatrix.MotorolaMatrix(0x383, 8);
                            dataSecondRowRightSeatArmrestScreenHmiRequest.CanId = 0x383;
                            if (_secRwRtStFtDnwdHmi2RReq && !_secRwRtStFtUpwdHmi2RReq)
                                dataSecondRowRightSeatArmrestScreenHmiRequest.MatrixData[3] = 0x80;
                            else if (!_secRwRtStFtDnwdHmi2RReq && _secRwRtStFtUpwdHmi2RReq)
                                dataSecondRowRightSeatArmrestScreenHmiRequest.MatrixData[3] = 0x40;
                            else if (!_secRwRtStFtDnwdHmi2RReq && !_secRwRtStFtUpwdHmi2RReq)
                                dataSecondRowRightSeatArmrestScreenHmiRequest.MatrixData[3] = 0x00;

                            // bossBtn
                            var bossBtnFrwd = new MatrixValDefinition(21, 1, _bosSwFrwdHmi2RReq ? (byte)1 : (byte)0);
                            var bossBtnBkwd = new MatrixValDefinition(20, 1, _bosSwBkwdHmi2RReq ? (byte)1 : (byte)0);
                            var boosBtnStBkReclnUpwd = new MatrixValDefinition(19, 1, _bosSwStBkReclnUpwdHmi2RReq ? (byte)1 : (byte)0);
                            var bossBtnStBkReclnDnwd = new MatrixValDefinition(18, 1, _bosSwStBkReclnDnwdHmi2RReq ? (byte)1 : (byte)0);
                            dataSecondRowRightSeatArmrestScreenHmiRequest.UpdateData(bossBtnFrwd);
                            dataSecondRowRightSeatArmrestScreenHmiRequest.UpdateData(bossBtnBkwd);
                            dataSecondRowRightSeatArmrestScreenHmiRequest.UpdateData(boosBtnStBkReclnUpwd);
                            dataSecondRowRightSeatArmrestScreenHmiRequest.UpdateData(bossBtnStBkReclnDnwd);

                            canMsgs.Add(new CanBus.CanDataPackage(dataSecondRowRightSeatArmrestScreenHmiRequest.CanId,
                                CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data,
                                dataSecondRowRightSeatArmrestScreenHmiRequest.MatrixData));
                        }

                        canMsgs.Add(new CanBus.CanDataPackage(0x429, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, data429));
                    }
                    #endregion
                    #region ROW2
                    else if (_selectedMode == SeatMode.Seat2L || _selectedMode == SeatMode.Seat2R) // 主驾驶、副驾驶
                    {
                        var data426 = new byte[6];

                        if (_selectedMode == SeatMode.Seat2L)
                        {
                            if (_stHtCshnDtyCycl)
                                data426[3] = 0xE6;
                            if (_stHtBkDtyCycl)
                                data426[4] = 0xE6;
                            if (_stVntDtyCycl)
                                data426[5] = 0xE6;
                        }
                        else if (_selectedMode == SeatMode.Seat2R)
                        {
                            if (_stHtCshnDtyCycl)
                                data426[0] = 0xE6;
                            if (_stHtBkDtyCycl)
                                data426[1] = 0xE6;
                            if (_stVntDtyCycl)
                                data426[2] = 0xE6;
                        }

                        canMsgs.Add(new CanBus.CanDataPackage(0x426, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, data426));
                    }
                    #endregion
                    #region ROW3
                    else if (_selectedMode == SeatMode.Seat3L || _selectedMode == SeatMode.Seat3R) // 主驾驶、副驾驶
                    {
                        var data488 = new byte[6];

                        if (_selectedMode == SeatMode.Seat3L)
                        {
                            if (_stHtCshnDtyCycl)
                                data488[3] = 0xE6;
                            if (_stHtBkDtyCycl)
                                data488[4] = 0xE6;
                            if (_stVntDtyCycl)
                                data488[5] = 0xE6;
                        }
                        else if (_selectedMode == SeatMode.Seat3R)
                        {
                            if (_stHtCshnDtyCycl)
                                data488[0] = 0xE6;
                            if (_stHtBkDtyCycl)
                                data488[1] = 0xE6;
                            if (_stVntDtyCycl)
                                data488[2] = 0xE6;
                        }

                        canMsgs.Add(new CanBus.CanDataPackage(0x488, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, data488));
                    }
                    #endregion

                    AppCan2.SendCanDatas(canMsgs.ToArray());
                }
            }
        }

        [Description("应用-打开座椅加热")]
        public void OpenStHtCshn()
        {
            _stHtCshnDtyCycl = true;
        }

        [Description("应用-关闭座椅加热")]
        public void CloseStHtCshn()
        {
            _stHtCshnDtyCycl = false;
        }

        [Description("应用-打开靠背加热")]
        public void OpenStHtBk()
        {
            _stHtBkDtyCycl = true;
        }

        [Description("应用-关闭靠背加热")]
        public void CloseStHtBk()
        {
            _stHtBkDtyCycl = false;
        }

        [Description("应用-打开座椅通风")]
        public void OpenStVnt()
        {
            _stVntDtyCycl = true;
        }

        [Description("应用-关闭座椅通风")]
        public void CloseStVnt()
        {
            _stVntDtyCycl = false;
        }

        [Description("应用-脚蹬打开")]
        public void FootrestOut()
        {
            _secRwRtStFtUpwdHmi2RReq = true;
            _secRwRtStFtDnwdHmi2RReq = false;
        }

        [Description("应用-脚蹬关闭")]
        public void FootrestIn()
        {
            _secRwRtStFtDnwdHmi2RReq = true;
            _secRwRtStFtUpwdHmi2RReq = false;
        }

        [Description("应用-脚蹬停止")]
        public void FootrestStop()
        {
            _secRwRtStFtDnwdHmi2RReq = false;
            _secRwRtStFtUpwdHmi2RReq = false;
        }

        [Description("应用-老板键-滑道电机向前")]
        public void BossButtonForw()
        {
            _bosSwBkwdHmi2RReq = false;
            _bosSwFrwdHmi2RReq = true;
        }

        [Description("应用-老板键-滑道电机向后")]
        public void BossButtonBackw()
        {
            if (_selectedMode == SeatMode.SeatPa)
            {

            }
            _bosSwFrwdHmi2RReq = false;
            _bosSwBkwdHmi2RReq = true;
        }

        [Description("应用-老板键-滑道电机停止")]
        public void BossButtonForwBackStop()
        {
            if (_selectedMode == SeatMode.SeatPa)
            {
                _bosSwBkwdHmi2RReq = false;
                _bosSwFrwdHmi2RReq = false;
            }
        }

        [Description("应用-老板键-靠背电机向上")]
        public void BossButtonStBkReclnUpwd()
        {
            if (_selectedMode == SeatMode.SeatPa)
            {
                _bosSwStBkReclnUpwdHmi2RReq = true;
                _bosSwStBkReclnDnwdHmi2RReq = false;
            }
        }

        [Description("应用-老板键-靠背电机向下")]
        public void BossButtonStBkReclnDnwd()
        {
            if (_selectedMode == SeatMode.SeatPa)
            {
                _bosSwStBkReclnUpwdHmi2RReq = false;
                _bosSwStBkReclnDnwdHmi2RReq = true;
            }
        }

        [Description("应用-老板键-靠背电机停止")]
        public void BossButtonStBkReclnUpDnStop()
        {
            if (_selectedMode == SeatMode.SeatPa)
            {
                _bosSwStBkReclnUpwdHmi2RReq = false;
                _bosSwStBkReclnDnwdHmi2RReq = false;
            }
        }

        #endregion

        #region 电机学习

        [Description("R,诊断-ClearMotorLearningState-FD17")]
        public string ClearMotorLearningState;
        [Description("R,诊断-MotorStateRead-FD0A")]
        public string MotorStateRead;

        [Description("R,诊断-读取电机状态-FD17和FD0A")]
        public string[] ReadMotorState()
        {
            ClearMotorLearningState = string.Empty;
            MotorStateRead = string.Empty;

            var t1 = ValueHelper.GetHextStr(ReadDidViaCan(_canDiagnosisRequestPhyCanId, _canDiagnosisResponseCanId, 0xFD, 0x17).ToArray()).Replace(" ", "");
            var t2 = ValueHelper.GetHextStr(ReadDidViaCan(_canDiagnosisRequestPhyCanId, _canDiagnosisResponseCanId, 0xFD, 0x0A, 1000).ToArray()).Replace(" ", "");
            var ts = new string[] { t1, t2 };

            ClearMotorLearningState = ts[0];
            MotorStateRead = ts[1];

            return ts;
        }

        [Description("R,诊断-滑道电机自学习RoutineControl")]
        public string FrwdBkwdMotorLearnRoutineControl;
        [Description("R,诊断-滑道电机ClearMotorLearningState")]
        public string FrwdBkwdMotorClearLearningState;
        [Description("R,诊断-滑道电机MotorStateRead")]
        public string FrwdBkwdMotorStateRead;
        [Description("R,诊断-滑道电机Learned")]
        public string FrwdBkwdMotorLearned;
        [Description("R,诊断-滑道电机Validity")]
        public string FrwdBkwdMotorValidity;

        [Description("R,诊断-靠背电机自学习RoutineControl")]
        public string BkRclnMotorLearnRoutineControl;
        [Description("R,诊断-靠背电机ClearMotorLearningState")]
        public string BkRclnMotorClearLearningState;
        [Description("R,诊断-靠背电机MotorStateRead")]
        public string BkRclnMotorStateRead;
        [Description("R,诊断-靠背电机Learned")]
        public string BkRclnMotorLearned;
        [Description("R,诊断-靠背电机Validity")]
        public string BkRclnMotorValidity;

        [Description("R,诊断-前提升电机自学习RoutineControl")]
        public string CushnFrtMotorLearnRoutineControl;
        [Description("R,诊断-前提升电机ClearMotorLearningState")]
        public string CushnFrtMotorClearLearningState;
        [Description("R,诊断-前提升电机MotorStateRead")]
        public string CushnFrtMotorStateRead;
        [Description("R,诊断-前提升电机Learned")]
        public string CushnFrtMotorLearned;
        [Description("R,诊断-前提升电机Validity")]
        public string CushnFrtMotorValidity;

        [Description("R,诊断-后提升电机自学习RoutineControl")]
        public string CushnRrMotorLearnRoutineControl;
        [Description("R,诊断-后提升电机ClearMotorLearningState")]
        public string CushnRrMotorClearLearningState;
        [Description("R,诊断-后提升电机MotorStateRead")]
        public string CushnRrMotorStateRead;
        [Description("R,诊断-后提升电机Learned")]
        public string CushnRrMotorLearned;
        [Description("R,诊断-后提升电机Validity")]
        public string CushnRrMotorValidity;

        [Description("R,诊断-脚蹬电机自学习RoutineControl")]
        public string FtrstMotorLearnRoutineControl;
        [Description("R,诊断-脚蹬电机ClearMotorLearningState")]
        public string FtrstMotorClearLearningState;
        [Description("R,诊断-脚蹬电机MotorStateRead")]
        public string FtrstMotorStateRead;
        [Description("R,诊断-脚蹬电机Learned")]
        public string FtrstMotorLearned;
        [Description("R,诊断-脚蹬电机Validity")]
        public string FtrstMotorValidity;

        [Description("R,诊断-腿托上下电机自学习RoutineControl")]
        public string SeatLegrestUpwardDownwardMotorLearnRoutineControl;
        [Description("R,诊断-腿托上下电机ClearMotorLearningState")]
        public string SeatLegrestUpwardDownwardMotorClearLearningState;
        [Description("R,诊断-腿托上下电机MotorStateRead")]
        public string SeatLegrestUpwardDownwardMotorStateRead;
        [Description("R,诊断-腿托上下电机Learned")]
        public string SeatLegrestUpwardDownwardMotorLearned;
        [Description("R,诊断-腿托上下电机Validity")]
        public string SeatLegrestUpwardDownwardMotorValidity;

        [Description("R,诊断-扶手电机自学习RoutineControl")]
        public string SeatArmscreenInOutMotorLearnRoutineControl;
        [Description("R,诊断-扶手电机ClearMotorLearningState")]
        public string SeatArmscreenInOutMotorClearLearningState;
        [Description("R,诊断-扶手电机MotorStateRead")]
        public string SeatArmscreenInOutMotorStateRead;
        [Description("R,诊断-扶手电机Learned")]
        public string SeatArmscreenInOutMotorLearned;
        [Description("R,诊断-扶手电机Validity")]
        public string SeatArmscreenInOutMotorValidity;

        [Description("R,诊断-横向电机自学习RoutineControl")]
        public string SeatLeftwardRightwardMotorLearnRoutineControl;
        [Description("R,诊断-横向电机ClearMotorLearningState")]
        public string SeatLeftwardRightwardMotorClearLearningState;
        [Description("R,诊断-横向电机MotorStateRead")]
        public string SeatLeftwardRightwardMotorStateRead;
        [Description("R,诊断-横向电机Learned")]
        public string SeatLeftwardRightwardMotorLearned;
        [Description("R,诊断-横向电机Validity")]
        public string SeatLeftwardRightwardMotorValidity;

        [Description("R,诊断-座垫翻折电机自学习RoutineControl")]
        public string SeatCushionFoldMotorLearnRoutineControl;
        [Description("R,诊断-座垫翻折电机ClearMotorLearningState")]
        public string SeatCushionFoldMotorClearLearningState;
        [Description("R,诊断-座垫翻折电机MotorStateRead")]
        public string SeatCushionFoldMotorStateRead;
        [Description("R,诊断-座垫翻折电机Learned")]
        public string SeatCushionFoldMotorLearned;
        [Description("R,诊断-座垫翻折电机Validity")]
        public string SeatCushionFoldMotorValidity;

        [Description("诊断-滑道电机自学习-通过A001学习")]
        public void FrwdBkwdMotorLearn(string timeoutMs)
        {
            FrwdBkwdMotorLearnRoutineControl = string.Empty;
            FrwdBkwdMotorClearLearningState = string.Empty;
            FrwdBkwdMotorStateRead = string.Empty;
            FrwdBkwdMotorLearned = string.Empty;
            FrwdBkwdMotorValidity = string.Empty;

            if (_selectedMode == SeatMode.SeatPa)
            {
                ExecuteMotorLearn(
                MotorType.FrwdBkwdMotor,
                timeoutMs,
                0xA0, 0x01,
                0, 0, // FD17
                2, // FD0A
                0x470, 1, 2, 1, 3,
                out FrwdBkwdMotorLearnRoutineControl,
                out FrwdBkwdMotorClearLearningState, out FrwdBkwdMotorStateRead,
                out FrwdBkwdMotorLearned, out FrwdBkwdMotorValidity);

                Thread.Sleep(15000);
            }
            else if (_selectedMode == SeatMode.SeatDr)
            {
                ExecuteMotorLearn(
                MotorType.FrwdBkwdMotor,
                timeoutMs,
                0xA0, 0x01,
                0, 0, // FD17
                2, // FD0A
                0x213, 7, 6, 7, 7,
                out FrwdBkwdMotorLearnRoutineControl,
                out FrwdBkwdMotorClearLearningState, out FrwdBkwdMotorStateRead,
                out FrwdBkwdMotorLearned, out FrwdBkwdMotorValidity);

                Thread.Sleep(15000);
            }
        }

        [Description("诊断-靠背电机自学习-通过A004学习")]
        public void BkRclnMotorLearn(string timeoutMs)
        {
            BkRclnMotorLearnRoutineControl = string.Empty;
            BkRclnMotorClearLearningState = string.Empty;
            BkRclnMotorStateRead = string.Empty;
            BkRclnMotorLearned = string.Empty;
            BkRclnMotorValidity = string.Empty;

            if (_selectedMode == SeatMode.SeatPa)
            {
                ExecuteMotorLearn(
                MotorType.BkRclnMotor,
                timeoutMs,
                0xA0, 0x04,
                0, 1,  // FD17
                10, // FD0A
                0x470, 3, 4, 3, 5,
                out BkRclnMotorLearnRoutineControl,
                out BkRclnMotorClearLearningState, out BkRclnMotorStateRead,
                out BkRclnMotorLearned, out BkRclnMotorValidity);

                Thread.Sleep(10000);
            }
            else if (_selectedMode == SeatMode.SeatDr)
            {
                ExecuteMotorLearn(
                MotorType.BkRclnMotor,
                timeoutMs,
                0xA0, 0x04,
                0, 1,  // FD17
                10, // FD0A
                0x213, 7, 4, 7, 5,
                out BkRclnMotorLearnRoutineControl,
                out BkRclnMotorClearLearningState, out BkRclnMotorStateRead,
                out BkRclnMotorLearned, out BkRclnMotorValidity);

                Thread.Sleep(10000);
            }
            else if (_selectedMode == SeatMode.Seat2L)
            {
                ExecuteMotorLearn(
                MotorType.BkRclnMotor,
                timeoutMs,
                0xA0, 0x04,
                0, 1,  // FD17
                10, // FD0A
                0x45D, 3, 4, 3, 5,
                out BkRclnMotorLearnRoutineControl,
                out BkRclnMotorClearLearningState, out BkRclnMotorStateRead,
                out BkRclnMotorLearned, out BkRclnMotorValidity);

                Thread.Sleep(3500);
            }
            else if (_selectedMode == SeatMode.Seat2R)
            {
                ExecuteMotorLearn(
                MotorType.BkRclnMotor,
                timeoutMs,
                0xA0, 0x04,
                0, 1,  // FD17
                10, // FD0A
                0x463, 3, 4, 3, 5,
                out BkRclnMotorLearnRoutineControl,
                out BkRclnMotorClearLearningState, out BkRclnMotorStateRead,
                out BkRclnMotorLearned, out BkRclnMotorValidity);

                Thread.Sleep(3500);
            }
            else if (_selectedMode == SeatMode.Seat3L)
            {
                ExecuteMotorLearn(
                    MotorType.BkRclnMotor,
                    timeoutMs,
                    0xA0, 0x04,
                    0, 1, // FD17
                    10, // FD0A
                    0x468, 3, 4, 3, 5,
                    out BkRclnMotorLearnRoutineControl,
                    out BkRclnMotorClearLearningState, out BkRclnMotorStateRead,
                    out BkRclnMotorLearned, out BkRclnMotorValidity);

                Thread.Sleep(10000);
            }
            else if (_selectedMode == SeatMode.Seat3R)
            {
                ExecuteMotorLearn(
                    MotorType.BkRclnMotor,
                    timeoutMs,
                    0xA0, 0x04,
                    0, 1, // FD17
                    10, // FD0A
                    0x46C, 3, 4, 3, 5,
                    out BkRclnMotorLearnRoutineControl,
                    out BkRclnMotorClearLearningState, out BkRclnMotorStateRead,
                    out BkRclnMotorLearned, out BkRclnMotorValidity);

                Thread.Sleep(10000);
            }
        }

        [Description("诊断-前提升电机自学习-通过A002学习")]
        public void CushnFrtMotorLearn(string timeoutMs)
        {
            CushnFrtMotorLearnRoutineControl = string.Empty;
            CushnFrtMotorClearLearningState = string.Empty;
            CushnFrtMotorStateRead = string.Empty;
            CushnFrtMotorLearned = string.Empty;
            CushnFrtMotorValidity = string.Empty;

            if (_selectedMode == SeatMode.SeatPa)
            {
                ExecuteMotorLearn(
                    MotorType.CushnFrtMotor,
                    timeoutMs,
                    0xA0, 0x02,
                    0, 2,  // FD17
                    18, // FD0A
                    0x471, 1, 2, 1, 3,
                    out CushnFrtMotorLearnRoutineControl,
                    out CushnFrtMotorClearLearningState, out CushnFrtMotorStateRead,
                    out CushnFrtMotorLearned, out CushnFrtMotorValidity);

                Thread.Sleep(2500);
            }
            else if (_selectedMode == SeatMode.SeatDr)
            {
                ExecuteMotorLearn(
                    MotorType.CushnFrtMotor,
                    timeoutMs,
                    0xA0, 0x02,
                    0, 2,  // FD17
                    18, // FD0A
                    0x214, 7, 6, 7, 7,
                    out CushnFrtMotorLearnRoutineControl,
                    out CushnFrtMotorClearLearningState, out CushnFrtMotorStateRead,
                    out CushnFrtMotorLearned, out CushnFrtMotorValidity);

                Thread.Sleep(2500);
            }
        }

        [Description("诊断-后提升电机自学习-通过A003学习")]
        public void CushnRrMotorLearn(string timeoutMs)
        {
            CushnRrMotorLearnRoutineControl = string.Empty;
            CushnRrMotorClearLearningState = string.Empty;
            CushnRrMotorStateRead = string.Empty;
            CushnRrMotorLearned = string.Empty;
            CushnRrMotorValidity = string.Empty;

            if (_selectedMode == SeatMode.SeatPa)
            {
                ExecuteMotorLearn(
                    MotorType.CushnRrMotor,
                    timeoutMs,
                    0xA0, 0x03,
                    0, 3,  // FD17
                    26, // FD0A
                    0x471, 3, 4, 3, 5,
                    out CushnRrMotorLearnRoutineControl,
                    out CushnRrMotorClearLearningState, out CushnRrMotorStateRead,
                    out CushnRrMotorLearned, out CushnRrMotorValidity);

                Thread.Sleep(2500);
            }
            else if (_selectedMode == SeatMode.SeatDr)
            {
                ExecuteMotorLearn(
                    MotorType.CushnRrMotor,
                    timeoutMs,
                    0xA0, 0x03,
                    0, 3,  // FD17
                    26, // FD0A
                    0x214, 7, 4, 7, 5,
                    out CushnRrMotorLearnRoutineControl,
                    out CushnRrMotorClearLearningState, out CushnRrMotorStateRead,
                    out CushnRrMotorLearned, out CushnRrMotorValidity);

                Thread.Sleep(2500);
            }
        }

        [Description("诊断-脚蹬电机自学习-通过A005学习")]
        public void FtrstMotorLearn(string timeoutMs)
        {
            FtrstMotorLearnRoutineControl = string.Empty;
            FtrstMotorClearLearningState = string.Empty;
            FtrstMotorStateRead = string.Empty;
            FtrstMotorLearned = string.Empty;
            FtrstMotorValidity = string.Empty;

            if (_selectedMode == SeatMode.SeatPa)
            {
                ExecuteMotorLearn(
                    MotorType.FtrstMotor,
                    timeoutMs,
                    0xA0, 0x05,
                    0, 7, // FD17
                    58, // FD0A
                    0x470, 6, 0, 6, 1,
                    out FtrstMotorLearnRoutineControl,
                    out FtrstMotorClearLearningState, out FtrstMotorStateRead,
                    out FtrstMotorLearned, out FtrstMotorValidity);

                Thread.Sleep(5000);
            }
            else if (_selectedMode == SeatMode.SeatDr)
            {
                ExecuteMotorLearn(
                    MotorType.FtrstMotor,
                    timeoutMs,
                    0xA0, 0x05,
                    0, 7, // FD17
                    58, // FD0A
                    0x262, 6, 2, 6, 6,
                    out FtrstMotorLearnRoutineControl,
                    out FtrstMotorClearLearningState, out FtrstMotorStateRead,
                    out FtrstMotorLearned, out FtrstMotorValidity);

                Thread.Sleep(5000);
            }
        }

        [Description("诊断-腿托上下电机自学习-通过A009学习")]
        public void SeatFootrestUpwardDownwardMotorLearn(string timeoutMs)
        {
            SeatLegrestUpwardDownwardMotorLearnRoutineControl = string.Empty;
            SeatLegrestUpwardDownwardMotorClearLearningState = string.Empty;
            SeatLegrestUpwardDownwardMotorStateRead = string.Empty;
            SeatLegrestUpwardDownwardMotorLearned = string.Empty;
            SeatLegrestUpwardDownwardMotorValidity = string.Empty;

            if (_selectedMode == SeatMode.Seat2L)
            {
                ExecuteMotorLearn(
                    MotorType.SeatLegrestUpwardDownwardMotor,
                    timeoutMs,
                    0xA0, 0x09,
                    0, 6, // FD17
                    42, // FD0A
                    0x45E, 6, 0, 6, 1,
                    out SeatLegrestUpwardDownwardMotorLearnRoutineControl,
                    out SeatLegrestUpwardDownwardMotorClearLearningState, out SeatLegrestUpwardDownwardMotorStateRead,
                    out SeatLegrestUpwardDownwardMotorLearned, out SeatLegrestUpwardDownwardMotorValidity);

                Thread.Sleep(2500);
            }
            else if (_selectedMode == SeatMode.Seat2R)
            {
                ExecuteMotorLearn(
                    MotorType.SeatLegrestUpwardDownwardMotor,
                    timeoutMs,
                    0xA0, 0x09,
                    0, 6, // FD17
                    42, // FD0A
                    0x464, 6, 0, 6, 1,
                    out SeatLegrestUpwardDownwardMotorLearnRoutineControl,
                    out SeatLegrestUpwardDownwardMotorClearLearningState, out SeatLegrestUpwardDownwardMotorStateRead,
                    out SeatLegrestUpwardDownwardMotorLearned, out SeatLegrestUpwardDownwardMotorValidity);

                Thread.Sleep(2500);
            }
        }

        [Description("诊断-扶手电机自学习-通过A008学习")]
        public void SeatArmscreenInOutMotorLearn(string timeoutMs)
        {
            SeatArmscreenInOutMotorLearnRoutineControl = string.Empty;
            SeatArmscreenInOutMotorClearLearningState = string.Empty;
            SeatArmscreenInOutMotorStateRead = string.Empty;
            SeatArmscreenInOutMotorLearned = string.Empty;
            SeatArmscreenInOutMotorValidity = string.Empty;

            if (_selectedMode == SeatMode.Seat2L)
            {
                ExecuteMotorLearn(
                    MotorType.SeatArmscreenInOutMotor,
                    timeoutMs,
                    0xA0, 0x08,
                    1, 0, // FD17
                    74, // FD0A
                    0x227, 1, 2, 1, 3,
                    out SeatArmscreenInOutMotorLearnRoutineControl,
                    out SeatArmscreenInOutMotorClearLearningState, out SeatArmscreenInOutMotorStateRead,
                    out SeatArmscreenInOutMotorLearned, out SeatArmscreenInOutMotorValidity);

                Thread.Sleep(3500);
            }
            else if (_selectedMode == SeatMode.Seat2R)
            {
                ExecuteMotorLearn(
                    MotorType.SeatArmscreenInOutMotor,
                    timeoutMs,
                    0xA0, 0x08,
                    1, 0, // FD17
                    74, // FD0A
                    0x226, 1, 2, 1, 3,
                    out SeatArmscreenInOutMotorLearnRoutineControl,
                    out SeatArmscreenInOutMotorClearLearningState, out SeatArmscreenInOutMotorStateRead,
                    out SeatArmscreenInOutMotorLearned, out SeatArmscreenInOutMotorValidity);

                Thread.Sleep(3500);
            }
        }

        [Description("诊断-横向电机自学习-通过A006学习")]
        public void SeatLeftwardRightwardMotorLearn(string timeoutMs)
        {
            SeatLeftwardRightwardMotorLearnRoutineControl = string.Empty;
            SeatLeftwardRightwardMotorClearLearningState = string.Empty;
            SeatLeftwardRightwardMotorStateRead = string.Empty;
            SeatLeftwardRightwardMotorLearned = string.Empty;
            SeatLeftwardRightwardMotorValidity = string.Empty;

            if (_selectedMode == SeatMode.Seat2L)
            {
                ExecuteMotorLearn(
                  MotorType.SeatLeftwardRightwardMotor,
                  timeoutMs,
                  0xA0, 0x06,
                  0, 4, // FD17
                  50, // FD0A
                  0x45D, 6, 0, 6, 1,
                  out SeatLeftwardRightwardMotorLearnRoutineControl,
                  out SeatLeftwardRightwardMotorClearLearningState, out SeatLeftwardRightwardMotorStateRead,
                  out SeatLeftwardRightwardMotorLearned, out SeatLeftwardRightwardMotorValidity);

                Thread.Sleep(5000);
            }
            else if (_selectedMode == SeatMode.Seat2R)
            {
                ExecuteMotorLearn(
                  MotorType.SeatLeftwardRightwardMotor,
                  timeoutMs,
                  0xA0, 0x06,
                  0, 4, // FD17
                  50, // FD0A
                  0x463, 6, 0, 6, 1,
                  out SeatLeftwardRightwardMotorLearnRoutineControl,
                  out SeatLeftwardRightwardMotorClearLearningState, out SeatLeftwardRightwardMotorStateRead,
                  out SeatLeftwardRightwardMotorLearned, out SeatLeftwardRightwardMotorValidity);

                Thread.Sleep(5000);
            }
        }

        [Description("诊断-座垫翻折电机自学习-通过A010学习")]
        public void SeatCushionFoldMotorPositionLearn(string timeoutMs)
        {
            SeatCushionFoldMotorLearnRoutineControl = string.Empty;
            SeatCushionFoldMotorClearLearningState = string.Empty;
            SeatCushionFoldMotorStateRead = string.Empty;
            SeatCushionFoldMotorLearned = string.Empty;
            SeatCushionFoldMotorValidity = string.Empty;

            if (_selectedMode == SeatMode.Seat3L)
            {
                ExecuteMotorLearn(
                    MotorType.SeatCushionFoldMotor,
                    timeoutMs,
                    0xA0, 0x10,
                    1, 3, // FD17
                    66, // FD0A
                    0x468, 5, 6, 5, 7,
                    out SeatCushionFoldMotorLearnRoutineControl,
                    out SeatCushionFoldMotorClearLearningState, out SeatCushionFoldMotorStateRead,
                    out SeatCushionFoldMotorLearned, out SeatCushionFoldMotorValidity);

                Thread.Sleep(10000);
            }
            else if (_selectedMode == SeatMode.Seat3R)
            {
                ExecuteMotorLearn(
                    MotorType.SeatCushionFoldMotor,
                    timeoutMs,
                    0xA0, 0x10,
                    1, 3, // FD17
                    66, // FD0A
                    0x46C, 5, 6, 5, 7,
                    out SeatCushionFoldMotorLearnRoutineControl,
                    out SeatCushionFoldMotorClearLearningState, out SeatCushionFoldMotorStateRead,
                    out SeatCushionFoldMotorLearned, out SeatCushionFoldMotorValidity);

                Thread.Sleep(10000);
            }
        }

        //public void ReadMotorLearnResult()
        //{
        //    if (!string.IsNullOrEmpty(FrwdBkwdMotorLearnedResult) &&
        //        !FrwdBkwdMotorLearnedResult.StartsWith("启动自学习诊断响应:NG"))
        //    {
        //        FrwdBkwdMotorLearnedResult = string.Empty;
        //    }

        //    if (!string.IsNullOrEmpty(BkRclnMotorLearnedResult) &&
        //        !BkRclnMotorLearnedResult.StartsWith("启动自学习诊断响应:NG"))
        //    {
        //        BkRclnMotorLearnedResult = string.Empty;
        //    }

        //    if (!string.IsNullOrEmpty(CushnFrtMotorLearnedResult) &&
        //        !CushnFrtMotorLearnedResult.StartsWith("启动自学习诊断响应:NG"))
        //    {
        //        CushnFrtMotorLearnedResult = string.Empty;
        //    }

        //    if (!string.IsNullOrEmpty(CushnRrMotorLearnedResult) &&
        //        !CushnRrMotorLearnedResult.StartsWith("启动自学习诊断响应:NG"))
        //    {
        //        CushnRrMotorLearnedResult = string.Empty;
        //    }

        //    if (!string.IsNullOrEmpty(FtrstMotorLearnedResult) &&
        //        !FtrstMotorLearnedResult.StartsWith("启动自学习诊断响应:NG"))
        //    {
        //        FtrstMotorLearnedResult = string.Empty;
        //    }

        //    {
        //        var learned = ExecuteReadMotorLearnFlag(1, 2, 0x470);
        //        var validity = ExecuteReadMotorLearnFlag(1, 3, 0x470);
        //        FrwdBkwdMotorLearnedResult = string.Format("自学习反馈:{0},位置有效性反馈:{1}", learned, validity);
        //    }
        //    {
        //        var learned = ExecuteReadMotorLearnFlag(3, 4, 0x470);
        //        var validity = ExecuteReadMotorLearnFlag(3, 5, 0x470);
        //        BkRclnMotorLearnedResult = string.Format("自学习反馈:{0},位置有效性反馈:{1}", learned, validity);
        //    }
        //    {
        //        var learned = ExecuteReadMotorLearnFlag(1, 2, 0x470);
        //        var validity = ExecuteReadMotorLearnFlag(1, 3, 0x470);
        //        CushnFrtMotorLearnedResult = string.Format("自学习反馈:{0},位置有效性反馈:{1}", learned, validity);
        //    }
        //    {
        //        var learned = ExecuteReadMotorLearnFlag(3, 4, 0x470);
        //        var validity = ExecuteReadMotorLearnFlag(3, 5, 0x470);
        //        CushnRrMotorLearnedResult = string.Format("自学习反馈:{0},位置有效性反馈:{1}", learned, validity);
        //    }
        //    {
        //        var learned = ExecuteReadMotorLearnFlag(6, 0, 0x470);
        //        var validity = ExecuteReadMotorLearnFlag(6, 1, 0x470);
        //        FtrstMotorLearnedResult = string.Format("自学习反馈:{0},位置有效性反馈:{1}", learned, validity);
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="motorType"></param>
        /// <param name="timeOutMs"></param>
        /// <param name="routineControlIdHi"></param>
        /// <param name="routineControlIdLo"></param>
        /// <param name="clearMotorLearningStateByteIndex">Clear Motor Learning state FD17</param>
        /// <param name="clearMotorLearningStateBitIndex">Clear Motor Learning state FD17</param>
        /// <param name="motorStateReadByteIndex">Motor State Read FD0A</param>
        /// <param name="flagCanId"></param>
        /// <param name="learnedByteIndex"></param>
        /// <param name="learnedBitIndex"></param>
        /// <param name="validityByteIndex"></param>
        /// <param name="validityBitIndex"></param>
        /// <param name="routineControlResult"></param>
        /// <param name="clearMotorLearningState0XFd17"></param>
        /// <param name="motorStateRead0XFd0A"></param>
        /// <param name="learnedBit"></param>
        /// <param name="validityBit"></param>
        private void ExecuteMotorLearn(
            MotorType motorType,
            string timeOutMs,
            byte routineControlIdHi, byte routineControlIdLo,
            int clearMotorLearningStateByteIndex, int clearMotorLearningStateBitIndex, int motorStateReadByteIndex,
            uint flagCanId, int learnedByteIndex, int learnedBitIndex, int validityByteIndex, int validityBitIndex,
            out string routineControlResult,
            out string clearMotorLearningState0XFd17, out string motorStateRead0XFd0A, out string learnedBit, out string validityBit)
        {
            routineControlResult = string.Empty;
            clearMotorLearningState0XFd17 = string.Empty;
            motorStateRead0XFd0A = string.Empty;
            learnedBit = string.Empty;
            validityBit = string.Empty;

            int ms;
            if (!int.TryParse(timeOutMs, out ms))
                return;
            if (ms < 0 || ms > 25000)
                ms = 15 * 1000;

            routineControlResult = MotorLearn(routineControlIdHi, routineControlIdLo);

            if (routineControlResult == string.Format("7103{0}{1}02", ValueHelper.GetHextStr(routineControlIdHi), ValueHelper.GetHextStr(routineControlIdLo)))
            {
                //var startTime = DateTime.Now;
                var step = 0;

                var stopWatch = new Stopwatch();
                stopWatch.Start();

                while (true)
                {
                    //if (ValueHelper.GetTimeSpanMs(startTime, DateTime.Now) > ms)
                    //    break;

                    if (stopWatch.ElapsedMilliseconds > ms)
                        break;

                    if (step == 0)
                    {
                        var validity = ExecuteReadMotorLearnFlag(motorType, validityByteIndex, validityBitIndex, flagCanId);
                        if (validity == 1.ToString())
                        {
                            step++;
                        }
                    }
                    else if (step == 1)
                    {
                        var validity = ExecuteReadMotorLearnFlag(motorType, validityByteIndex, validityBitIndex, flagCanId);
                        var learned = ExecuteReadMotorLearnFlag(motorType, learnedByteIndex, learnedBitIndex, flagCanId);
                        if (validity == 0.ToString() && learned == 1.ToString())
                        {
                            step++;
                        }
                    }
                    else if (step == 2)
                    {
                        break;
                    }
                }

                stopWatch.Stop();

                validityBit = ExecuteReadMotorLearnFlag(motorType, validityByteIndex, validityBitIndex, flagCanId);
                learnedBit = ExecuteReadMotorLearnFlag(motorType, learnedByteIndex, learnedBitIndex, flagCanId);

                var ts = ReadMotorState();
                var clearMotorLearningState = ts[0];
                var motorStateRead = ts[1];

                if (string.IsNullOrEmpty(clearMotorLearningState) || string.IsNullOrEmpty(motorStateRead) ||
                    clearMotorLearningState.Length != 2 * 2 || motorStateRead.Length != 116 * 2)
                    return;
                var clearMotorLearningStateBs = new List<byte>();
                for (var i = 0; i < clearMotorLearningState.Length; i = i + 2)
                    clearMotorLearningStateBs.Add(Convert.ToByte(clearMotorLearningState.Substring(0, 2), 16));

                clearMotorLearningState0XFd17 =
                    Convert.ToString(clearMotorLearningStateBs[clearMotorLearningStateByteIndex], 2)
                        .PadLeft(8, '0')
                        .ToCharArray()
                        .Reverse()
                        .ToArray()[clearMotorLearningStateBitIndex].ToString();

                var bs = new List<byte>();
                for (var i = 0; i < motorStateRead.Length; i = i + 2)
                    bs.Add(Convert.ToByte(motorStateRead.Substring(i, 2), 16));
                motorStateRead0XFd0A = ValueHelper.GetHextStr(bs[motorStateReadByteIndex]);
            }
            else
            {
                routineControlResult = string.Format("启动自学习诊断响应:{0}", "NG " + routineControlResult);
            }
        }

        private string MotorLearn(byte didHi, byte didLo)
        {
            if (DiagnosticCan1 == null)
                return string.Empty;

            lock (DiagnosticCan1)
            {
                //byte[] startEcho;
                if (!DiagnosticCan1.CanBusWithUds.TryStartRoutineControl(
                    _canDiagnosisRequestPhyCanId, _canDiagnosisResponseCanId, CanBus.CanType.Extended, didHi, didLo, pendingByte: 0x00, timeoutFromMilliseconds: 1 * 1000))
                    return "NG 发送3101A001失败";
                byte[] resultEcho;
                if (DiagnosticCan1.CanBusWithUds.TesterTryRequest(_canDiagnosisRequestPhyCanId, _canDiagnosisResponseCanId,
                    new byte[] { 0x31, 0x03, didHi, didLo }, out resultEcho, CanBus.CanType.Extended))
                {
                    return ValueHelper.GetHextStr(resultEcho).Replace(" ", "");
                }

                return "NG 读取3103A001失败";
            }
        }

        private string ExecuteReadMotorLearnFlag(MotorType motorType, int learnByteIndex, int learnBitIndex, uint learnCanId)
        {
            var find = _motorLearnTasks.Find(f => f.MotorType == motorType);
            if (find == null)
                return string.Empty;

            find.LearnFlag = string.Empty;
            find.LearnByteIndex = learnByteIndex;
            find.LearnBitIndex = learnBitIndex;
            find.LearnCanId = learnCanId;

            find.IsMotorLearnExecuted = true;
            var isSuccess = find.LearnWaitHandle.WaitOne(2500);
            find.IsMotorLearnExecuted = false;

            return isSuccess ? find.LearnFlag : string.Empty;

            //_learnFlag = string.Empty;
            //_learnByteIndex = learnByteIndex;
            //_learnBitIndex = learnBitIndex;
            //_learnCanId = learnCanId;

            //_isMotorLearnExecuted = true;
            //var isSuccess = _learnWaitHandle.WaitOne(500);
            //_isMotorLearnExecuted = false;

            //return isSuccess ? _learnFlag : string.Empty;
        }

        private readonly List<MotorLearnTask> _motorLearnTasks = new List<MotorLearnTask>();

        internal class MotorLearnTask
        {
            public MotorType MotorType { get; set; }
            public bool IsLearning { get; set; }

            public bool IsMotorLearnExecuted;
            public int LearnByteIndex;
            public int LearnBitIndex;
            public uint LearnCanId;
            public string LearnFlag;
            public readonly EventWaitHandle LearnWaitHandle = new AutoResetEvent(false);

            public MotorLearnTask(MotorType motorType)
            {
                MotorType = motorType;
            }
        }

        internal enum MotorType
        {
            /// <summary>
            /// 滑道
            /// </summary>
            FrwdBkwdMotor,

            /// <summary>
            /// 靠背
            /// </summary>
            BkRclnMotor,

            /// <summary>
            /// 前提升
            /// </summary>
            CushnFrtMotor,

            /// <summary>
            /// 后提升
            /// </summary>
            CushnRrMotor,

            /// <summary>
            /// 脚蹬
            /// </summary>
            FtrstMotor,

            /// <summary>
            /// 腿托
            /// </summary>
            SeatLegrestUpwardDownwardMotor,

            /// <summary>
            /// 横向
            /// </summary>
            SeatLeftwardRightwardMotor,

            /// <summary>
            /// 扶手
            /// </summary>
            SeatArmscreenInOutMotor,

            /// <summary>
            /// 座椅翻折
            /// </summary>
            SeatCushionFoldMotor
        }

        //private bool _isMotorLearnExecuted;
        //private int _learnByteIndex;
        //private int _learnBitIndex;
        //private uint _learnCanId;
        //private string _learnFlag;
        //private readonly EventWaitHandle _learnWaitHandle = new AutoResetEvent(false);
        private void CanBus_PushCanMsg(string name, CanBus.CanDataPackage data, CanBus.OnPushCanDataType onPushCanDataType)
        {
            if (((AppCan2 != null && AppCan2.Name == name) || (DiagnosticCan1 != null && DiagnosticCan1.Name == name)) && onPushCanDataType != CanBus.OnPushCanDataType.Tx)
            {
                foreach (var t in _motorLearnTasks)
                {
                    if (t.IsMotorLearnExecuted && t.LearnCanId == data.CanId)
                    {
                        if (t.LearnByteIndex != -1 && t.LearnBitIndex != -1 && t.LearnBitIndex >= 0 && t.LearnBitIndex <= 7 && t.LearnCanId != uint.MaxValue)
                        {
                            if (data.CanData != null && data.CanId == t.LearnCanId && data.CanData.Length - 1 >= t.LearnByteIndex)
                            {
                                var d = data.CanData[t.LearnByteIndex];
                                var bits = Convert.ToString(d, 2).PadLeft(8, '0').ToCharArray().Reverse().ToArray();
                                t.LearnFlag = bits[t.LearnBitIndex].ToString();

                                t.LearnByteIndex = -1;
                                t.LearnBitIndex = -1;
                                t.LearnCanId = uint.MaxValue;
                                t.LearnWaitHandle.Set();
                            }
                        }
                    }
                }

                if (_isStartReadRow2InputSignal)
                {
                    if (_selectedMode == SeatMode.Seat2L && data.CanId == 0x3A2 && data.CanData != null && data.CanDataLen >= 3)
                    {
                        var data0Bits = Convert.ToString(data.CanData[0], 2).PadLeft(8, '0').ToCharArray().Reverse().ToList();
                        var data2Bits = Convert.ToString(data.CanData[0], 2).PadLeft(8, '0').ToCharArray().Reverse().ToList();

                        Row2Eze = data0Bits[7].ToString();
                        Row2Reset = data0Bits[5].ToString();
                        Row2Vip = data2Bits[7].ToString();
                    }
                    else if (_selectedMode == SeatMode.Seat2R && data.CanId == 0x3A2 && data.CanData != null && data.CanDataLen >= 3)
                    {
                        var data0Bits = Convert.ToString(data.CanData[0], 2).PadLeft(8, '0').ToCharArray().Reverse().ToList();
                        var data2Bits = Convert.ToString(data.CanData[2], 2).PadLeft(8, '0').ToCharArray().Reverse().ToList();

                        Row2Eze = data0Bits[7].ToString();
                        Row2Reset = data2Bits[7].ToString();
                        Row2Vip = data0Bits[5].ToString();
                    }
                }
            }
        }

        #endregion

        #region Led Lin相关

        private readonly Thread _keepLinNetworkThread;
        private bool _isLampOn;

        //private bool _isLinMotorFrwd;
        //private bool _isLinMotorBkwd;
        //private int _motorIndex;
        private void KeepLinNetwork()
        {
            while (_keepNetworkThread.IsAlive)
            {
                if (!_keepNetworkThread.IsAlive)
                    break;

                Thread.Sleep(100);

                if (LedLin != null)
                {
                    if (!_isSleep)
                    {
                        if (!_isLampOn)
                        {
                            LedLin.SendMasterLin(0x11, new byte[] { 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00 });
                            LedLin.SendMasterLin(0x12, new byte[] { 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00 });
                            LedLin.SendMasterLin(0x13, new byte[] { 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00 });
                        }
                        else
                        {
                            //var r = (byte)(new Random().Next(0, 256));
                            //var g = (byte)(new Random().Next(0, 256));
                            //var b = (byte)(new Random().Next(0, 256));

                            LedLin.SendMasterLin(0x11, new byte[] { 0xFF, 0xFF, 0xFF, 0x64, 0x00, 0x00 });
                            LedLin.SendMasterLin(0x12, new byte[] { 0xFF, 0xFF, 0xFF, 0x64, 0x00, 0x00 });
                            LedLin.SendMasterLin(0x13, new byte[] { 0xFF, 0xFF, 0xFF, 0x64, 0x00, 0x00 });
                        }
                    }
                }

                //if (MsmLin != null)
                //{
                //    if (!_isSleep)
                //    {
                //        if (_motorIndex == -1 || (!_isLinMotorFrwd && !_isLinMotorBkwd))
                //        {
                //            MsmLin.SendMasterLin(0x17, new byte[] { 00, 0x00, 0x00, 0x00, 0x00 });
                //        }
                //        else if (_motorIndex > -1)
                //        {
                //            //var linId = (byte)0x17;
                //            //switch (_selectedMode)
                //            //{
                //            //    case SeatMode.SeatDr:
                //            //        break;
                //            //    case SeatMode.SeatPa:
                //            //        linId = (byte)0x17;
                //            //        break;
                //            //    case SeatMode.Seat2L:
                //            //        break;
                //            //    case SeatMode.Seat2R:
                //            //        break;
                //            //    case SeatMode.Seat3L:
                //            //        break;
                //            //    case SeatMode.Seat3R:
                //            //        break;
                //            //    case SeatMode.Hmi2L:
                //            //        break;
                //            //    case SeatMode.Hmi2R:
                //            //        break;
                //            //    default:
                //            //        throw new ArgumentOutOfRangeException();
                //            //}

                //            switch (_motorIndex)
                //            {
                //                case 0:
                //                    if (_isLinMotorFrwd && !_isLinMotorBkwd)
                //                        MsmLin.SendMasterLin(0x17, new byte[] { 0x00, 0x00, 0x10, 0x00, 0x00 });
                //                    else if (!_isLinMotorFrwd && _isLinMotorBkwd)
                //                        MsmLin.SendMasterLin(0x17, new byte[] { 0x00, 0x00, 0x20, 0x00, 0x00 });
                //                    else
                //                        MsmLin.SendMasterLin(0x17, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00 });
                //                    break;

                //                case 1:
                //                    if (_isLinMotorFrwd && !_isLinMotorBkwd)
                //                        MsmLin.SendMasterLin(0x17, new byte[] { 0x00, 0x00, 0x00, 0x20, 0x00 });
                //                    else if (!_isLinMotorFrwd && _isLinMotorBkwd)
                //                        MsmLin.SendMasterLin(0x17, new byte[] { 0x00, 0x00, 0x00, 0x40, 0x00 });
                //                    else
                //                        MsmLin.SendMasterLin(0x17, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00 });
                //                    break;

                //                case 2:
                //                    if (_isLinMotorFrwd && !_isLinMotorBkwd)
                //                        MsmLin.SendMasterLin(0x17, new byte[] { 0x00, 0x00, 0x40, 0x00, 0x00 });
                //                    else if (!_isLinMotorFrwd && _isLinMotorBkwd)
                //                        MsmLin.SendMasterLin(0x17, new byte[] { 0x00, 0x00, 0x80, 0x00, 0x00 });
                //                    else
                //                        MsmLin.SendMasterLin(0x17, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00 });
                //                    break;

                //                case 3:
                //                    if (_isLinMotorFrwd && !_isLinMotorBkwd)
                //                        MsmLin.SendMasterLin(0x17, new byte[] { 0x00, 0x00, 0x00, 0x02, 0x00 });
                //                    else if (!_isLinMotorFrwd && _isLinMotorBkwd)
                //                        MsmLin.SendMasterLin(0x17, new byte[] { 0x00, 0x00, 0x00, 0x04, 0x00 });
                //                    else
                //                        MsmLin.SendMasterLin(0x17, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00 });
                //                    break;
                //            }
                //        }
                //    }
                //}
            }
        }

        [Description("R,氛围灯软件版本信息")]
        public string LedAppVer;

        [Description("Read氛围灯软件版本信息")]
        public void ReadLedAppVer()
        {
            LedAppVer = string.Empty;
            if (LedLin != null)
            {
                byte[] echo;
                if (LedLin.SendMasterLinAndRecvSingleSlaveLin(0x3C, 0x3D, new byte[] { 0x7F, 0x02, 0xBA, 0x61, 0xFF, 0xFF, 0xFF, 0xFF }, out echo) && echo != null)
                {
                    LedAppVer = ValueHelper.GetHextStr(echo).Replace(" ", "");
                }
            }
        }

        [Description("打开氛围灯")]
        public void LedOn()
        {
            _isLampOn = true;
        }

        [Description("关闭氛围灯")]
        public void LedOff()
        {
            _isLampOn = false;
        }

        #endregion

        #region SlaveLin相关

        public byte[] SlaveLinMatrix = { 0x00, 0x00, 0x00, 0x00, 0xF0 };
        public byte[] SlaveLinMatrix2 = { 0x00, 0x00, 0x00, 0x00, 0X00 };
        public byte[] SlaveLinMatrix3 = { 0x00, 0x00, 0x00 };

        private bool _is滑道电机向前;
        private bool _is滑道电机向后;
        private bool _is靠背电机向前;
        private bool _is靠背电机向后;
        private bool _is后升电机向上;
        private bool _is后升电机向下;
        private bool _is前升电机向上;
        private bool _is前升电机向下;

        private bool _is腰托按摩电机向前;
        private bool _is腰托按摩电机向后;
        private bool _is腰托按摩电机向上;
        private bool _is腰托按摩电机向下;
        private bool _is腰托按摩电机按摩;

        private bool _is腰托按摩电机释放或翻折;

        public void StartSlaveLin()
        {
            //return;
            var slaveLin = MsmLin as ToomossUsb2XxxSlaveLin.ToomossUsbSlaveLinChannel;
            if (slaveLin == null) return;
            slaveLin.ConfigLinMsgCount(4);
            var str = SlaveLinMatrix.Aggregate(string.Empty, (current, b) => current + ValueHelper.GetHextStr(b));
            var str2 = SlaveLinMatrix2.Aggregate(string.Empty, (current, b) => current + ValueHelper.GetHextStr(b));
            var str3 = SlaveLinMatrix3.Aggregate(string.Empty, (current, b) => current + ValueHelper.GetHextStr(b));
            slaveLin.UpdateLinExMsg(ValueHelper.GetHextStr(0x17), str);
            slaveLin.UpdateLinExMsg(ValueHelper.GetHextStr(0x31), str);

            //slaveLin.UpdateLinExMsg(ValueHelper.GetHextStr(0x21), str2);
            //slaveLin.UpdateLinExMsg(ValueHelper.GetHextStr(0x11), str2);

            slaveLin.UpdateLinExMsg(ValueHelper.GetHextStr(0x15), str3);
            slaveLin.UpdateLinExMsg(ValueHelper.GetHextStr(0x27), str3);
        }

        public void StopSlaveLin()
        {
            var slaveLin = MsmLin as ToomossUsb2XxxSlaveLin.ToomossUsbSlaveLinChannel;
            if (slaveLin == null) return;
            slaveLin.ConfigLinMsgCount(0);
        }

        [Description("LIN模拟按钮-滑道电机向前")]
        public void LIN模拟按钮滑道电机向前()
        {
            _is滑道电机向前 = true;
            _is滑道电机向后 = false;
            UpdateLinMsg();
        }

        [Description("LIN模拟按钮-滑道电机向后")]
        public void LIN模拟按钮滑道电机向后()
        {
            _is滑道电机向前 = false;
            _is滑道电机向后 = true;
            UpdateLinMsg();
        }

        [Description("LIN模拟按钮-滑道电机停止")]
        public void LIN模拟按钮滑道电机停止()
        {
            _is滑道电机向前 = false;
            _is滑道电机向后 = false;
            UpdateLinMsg();
        }

        [Description("LIN模拟按钮-靠背电机向前")]
        public void LIN模拟按钮靠背电机向前()
        {
            _is靠背电机向前 = true;
            _is靠背电机向后 = false;
            UpdateLinMsg();
        }

        [Description("LIN模拟按钮-靠背电机向后")]
        public void LIN模拟按钮靠背电机向后()
        {
            _is靠背电机向前 = false;
            _is靠背电机向后 = true;
            UpdateLinMsg();
        }

        [Description("LIN模拟按钮-靠背电机停止")]
        public void LIN模拟按钮靠背电机停止()
        {
            _is靠背电机向前 = false;
            _is靠背电机向后 = false;
            UpdateLinMsg();
        }

        [Description("LIN模拟按钮-前升电机向上")]
        public void LIN模拟按钮前升电机向上()
        {
            _is前升电机向上 = true;
            _is前升电机向下 = false;
            UpdateLinMsg();
        }

        [Description("LIN模拟按钮-前升电机向下")]
        public void LIN模拟按钮前升电机向下()
        {
            _is前升电机向上 = false;
            _is前升电机向下 = true;
            UpdateLinMsg();
        }

        [Description("LIN模拟按钮-前升电机停止")]
        public void LIN模拟按钮前升电机停止()
        {
            _is前升电机向上 = false;
            _is前升电机向下 = false;
            UpdateLinMsg();
        }

        [Description("LIN模拟按钮-后升电机向上")]
        public void LIN模拟按钮后升电机向上()
        {
            _is后升电机向上 = true;
            _is后升电机向下 = false;
            UpdateLinMsg();
        }

        [Description("LIN模拟按钮-后升电机向下")]
        public void LIN模拟按钮后升电机向下()
        {
            _is后升电机向上 = false;
            _is后升电机向下 = true;
            UpdateLinMsg();
        }

        [Description("LIN模拟按钮-后升电机停止")]
        public void LIN模拟按钮后升电机停止()
        {
            _is后升电机向上 = false;
            _is后升电机向下 = false;
            UpdateLinMsg();
        }

        [Description("LIN模拟按钮-腰托按摩电机向前")]
        public void LIN模拟按钮腰托按摩电机向前()
        {
            _is腰托按摩电机向上 = false;
            _is腰托按摩电机向下 = false;
            _is腰托按摩电机向前 = true;
            _is腰托按摩电机向后 = false;
            _is腰托按摩电机按摩 = false;
            UpdateLinMsg();
        }

        [Description("LIN模拟按钮-腰托按摩电机向后")]
        public void LIN模拟按钮腰托按摩电机向后()
        {
            _is腰托按摩电机向上 = false;
            _is腰托按摩电机向下 = false;
            _is腰托按摩电机向前 = false;
            _is腰托按摩电机向后 = true;
            _is腰托按摩电机按摩 = false;
            _is腰托按摩电机向后 = true;
            UpdateLinMsg();
        }

        [Description("LIN模拟按钮-腰托按摩电机向前向后停止")]
        public void LIN模拟按钮腰托按摩电机向前向后停止()
        {
            _is腰托按摩电机向上 = false;
            _is腰托按摩电机向下 = false;
            _is腰托按摩电机向前 = false;
            _is腰托按摩电机向后 = false;
            _is腰托按摩电机按摩 = false;
            UpdateLinMsg();
        }

        [Description("LIN模拟按钮-腰托按摩电机向上")]
        public void LIN模拟按钮腰托按摩电机向上()
        {
            _is腰托按摩电机向上 = true;
            _is腰托按摩电机向下 = false;
            _is腰托按摩电机向前 = false;
            _is腰托按摩电机向后 = false;
            _is腰托按摩电机按摩 = false;
            UpdateLinMsg();
        }

        [Description("LIN模拟按钮-腰托按摩电机向下")]
        public void LIN模拟按钮腰托按摩电机向下()
        {
            _is腰托按摩电机向上 = false;
            _is腰托按摩电机向下 = true;
            _is腰托按摩电机向前 = false;
            _is腰托按摩电机向后 = false;
            _is腰托按摩电机按摩 = false;
            UpdateLinMsg();
        }

        [Description("LIN模拟按钮-腰托按摩电机向上向下停止")]
        public void LIN模拟按钮腰托按摩电机向上向下停止()
        {
            _is腰托按摩电机向上 = false;
            _is腰托按摩电机向下 = false;
            _is腰托按摩电机向前 = false;
            _is腰托按摩电机向后 = false;
            _is腰托按摩电机按摩 = false;
            UpdateLinMsg();
        }

        [Description("LIN模拟按钮-腰托按摩电机按摩打开")]
        public void LIN模拟按钮腰托按摩电机按摩打开()
        {
            _is腰托按摩电机向上 = false;
            _is腰托按摩电机向下 = false;
            _is腰托按摩电机向前 = false;
            _is腰托按摩电机向后 = false;
            _is腰托按摩电机按摩 = true;
            UpdateLinMsg();
        }

        [Description("LIN模拟按钮-腰托按摩电机按摩关闭")]
        public void LIN模拟按钮腰托按摩电机按摩关闭()
        {
            _is腰托按摩电机向上 = false;
            _is腰托按摩电机向下 = false;
            _is腰托按摩电机向前 = false;
            _is腰托按摩电机向后 = false;
            _is腰托按摩电机按摩 = false;
            UpdateLinMsg();
        }

        [Description("LIN模拟按钮-坐垫释放或翻折打开")]
        public void LIN模拟按钮坐垫释放或翻折打开()
        {
            _is腰托按摩电机释放或翻折 = true;
            UpdateLinMsg();
        }

        [Description("LIN模拟按钮-坐垫释放或翻折关闭")]
        public void LIN模拟按钮坐垫释放或翻折关闭()
        {
            _is腰托按摩电机释放或翻折 = false;
            UpdateLinMsg();
        }

        private void UpdateLinMsg()
        {
            if (_selectedMode == SeatMode.SeatPa)
            {
                if (_is滑道电机向前 && !_is滑道电机向后)
                    SlaveLinMatrix[2] = 0x10;
                else if (!_is滑道电机向前 && _is滑道电机向后)
                    SlaveLinMatrix[2] = 0x20;
                else if (!_is滑道电机向前 && !_is滑道电机向后)
                    SlaveLinMatrix[2] = 0x00;

                if (_is后升电机向上 && !_is后升电机向下)
                    SlaveLinMatrix[3] = 0x02;
                else if (!_is后升电机向上 && _is后升电机向下)
                    SlaveLinMatrix[3] = 0x04;
                else if (!_is后升电机向上 && !_is后升电机向下)
                    SlaveLinMatrix[3] = 0x00;

                if (_is靠背电机向前 && !_is靠背电机向后)
                    SlaveLinMatrix[3] = (byte)(SlaveLinMatrix[3] + 0x20);
                else if (!_is靠背电机向前 && _is靠背电机向后)
                    SlaveLinMatrix[3] = (byte)(SlaveLinMatrix[3] + 0x40);
                else if (!_is靠背电机向前 && !_is靠背电机向后)
                    SlaveLinMatrix[3] = (byte)(SlaveLinMatrix[3] + 0x00);

                var slaveLin = MsmLin as ToomossUsb2XxxSlaveLin.ToomossUsbSlaveLinChannel;
                if (slaveLin == null) return;
                var str = SlaveLinMatrix.Aggregate(string.Empty, (current, b) => current + ValueHelper.GetHextStr(b));
                slaveLin.UpdateLinExMsg(ValueHelper.GetHextStr(0x17), str);
            }
            else if (_selectedMode == SeatMode.SeatDr)
            {
                if (_is滑道电机向前 && !_is滑道电机向后)
                    SlaveLinMatrix[2] = 0x10;
                else if (!_is滑道电机向前 && _is滑道电机向后)
                    SlaveLinMatrix[2] = 0x20;
                else if (!_is滑道电机向前 && !_is滑道电机向后)
                    SlaveLinMatrix[2] = 0x00;

                if (_is后升电机向上 && !_is后升电机向下)
                    SlaveLinMatrix[3] = 0x02;
                else if (!_is后升电机向上 && _is后升电机向下)
                    SlaveLinMatrix[3] = 0x04;
                else if (!_is后升电机向上 && !_is后升电机向下)
                    SlaveLinMatrix[3] = 0x00;

                if (_is前升电机向上 && !_is前升电机向下)
                    SlaveLinMatrix[2] = (byte)(SlaveLinMatrix[2] + 0x40);
                else if (!_is前升电机向上 && _is前升电机向下)
                    SlaveLinMatrix[2] = (byte)(SlaveLinMatrix[2] + 0x80);
                else if (!_is前升电机向上 && !_is前升电机向下)
                    SlaveLinMatrix[2] = (byte)(SlaveLinMatrix[2] + 0x00);

                if (_is靠背电机向前 && !_is靠背电机向后)
                    SlaveLinMatrix[3] = (byte)(SlaveLinMatrix[3] + 0x20);
                else if (!_is靠背电机向前 && _is靠背电机向后)
                    SlaveLinMatrix[3] = (byte)(SlaveLinMatrix[3] + 0x40);
                else if (!_is靠背电机向前 && !_is靠背电机向后)
                    SlaveLinMatrix[3] = (byte)(SlaveLinMatrix[3] + 0x00);

                var slaveLin = MsmLin as ToomossUsb2XxxSlaveLin.ToomossUsbSlaveLinChannel;
                if (slaveLin == null) return;
                var str = SlaveLinMatrix.Aggregate(string.Empty, (current, b) => current + ValueHelper.GetHextStr(b));
                slaveLin.UpdateLinExMsg(ValueHelper.GetHextStr(0x31), str);
            }
            else if (_selectedMode == SeatMode.Seat2L || _selectedMode == SeatMode.Seat2R)
            {
                return;

                var strbit = new List<string>();
                for (var i = 0; i < 8; i++)
                    strbit.Add("0");

                if (_is腰托按摩电机向前 && !_is腰托按摩电机向后)
                {
                    strbit[0] = "1";
                    strbit[1] = "0";
                }
                else if (!_is腰托按摩电机向前 && _is腰托按摩电机向后)
                {
                    strbit[0] = "0";
                    strbit[1] = "1";
                }
                else if (!_is腰托按摩电机向前 && !_is腰托按摩电机向后)
                {
                    strbit[0] = "0";
                    strbit[1] = "0";
                }

                if (_is腰托按摩电机向上 && !_is腰托按摩电机向下)
                {
                    strbit[2] = "1";
                    strbit[3] = "0";
                }
                else if (!_is腰托按摩电机向上 && _is腰托按摩电机向下)
                {
                    strbit[2] = "0";
                    strbit[3] = "1";
                }
                else if (!_is腰托按摩电机向上 && !_is腰托按摩电机向下)
                {
                    strbit[2] = "0";
                    strbit[3] = "0";
                }

                var bStr = string.Empty;
                strbit.Reverse();
                bStr = strbit.Aggregate(bStr, (current, t) => current + t);

                var b3 = Convert.ToByte(bStr, 2);
                SlaveLinMatrix2[3] = b3;

                if (_is腰托按摩电机按摩)
                {
                    SlaveLinMatrix2[3] = (byte)(SlaveLinMatrix2[3] + 0x10);
                    SlaveLinMatrix2[4] = 0x03;
                }
                else
                {
                    SlaveLinMatrix2[3] = (byte)(SlaveLinMatrix2[3] + 0x00);
                    SlaveLinMatrix2[4] = 0x00;
                }

                var slaveLin = MsmLin as ToomossUsb2XxxSlaveLin.ToomossUsbSlaveLinChannel;
                if (slaveLin == null) return;
                var str = SlaveLinMatrix2.Aggregate(string.Empty, (current, b) => current + ValueHelper.GetHextStr(b));
                slaveLin.UpdateLinExMsg(
                    _selectedMode == SeatMode.Seat2L ? ValueHelper.GetHextStr(0x21) : ValueHelper.GetHextStr(0x11), str);
            }
            else if (_selectedMode == SeatMode.Seat3L||_selectedMode==SeatMode.Seat3R)
            {
                if (_is靠背电机向前 && !_is靠背电机向后)
                    SlaveLinMatrix3[2] = (byte)(0x00+ 0x10);
                else if (!_is靠背电机向前 && _is靠背电机向后)
                    SlaveLinMatrix3[2] = (byte)(0x00 + 0x20);
                else if (!_is靠背电机向前 && !_is靠背电机向后)
                    SlaveLinMatrix3[2] = (byte)(0x00 + 0x00);

                if (_is腰托按摩电机释放或翻折)
                {
                    SlaveLinMatrix3[2] = (byte)(SlaveLinMatrix3[2] + 0x02);
                }
                else
                {
                    SlaveLinMatrix3[2] = (byte)(SlaveLinMatrix3[2] + 0x00);
                }

                if (_selectedMode == SeatMode.Seat3L)
                {
                    var slaveLin = MsmLin as ToomossUsb2XxxSlaveLin.ToomossUsbSlaveLinChannel;
                    if (slaveLin == null) return;
                    var str = SlaveLinMatrix3.Aggregate(string.Empty, (current, b) => current + ValueHelper.GetHextStr(b));
                    slaveLin.UpdateLinExMsg(ValueHelper.GetHextStr(0x15), str);
                }
                else
                {
                    var slaveLin = MsmLin as ToomossUsb2XxxSlaveLin.ToomossUsbSlaveLinChannel;
                    if (slaveLin == null) return;
                    var str = SlaveLinMatrix3.Aggregate(string.Empty, (current, b) => current + ValueHelper.GetHextStr(b));
                    slaveLin.UpdateLinExMsg(ValueHelper.GetHextStr(0x27), str);
                }
            }
        }

        #endregion
    }
}
