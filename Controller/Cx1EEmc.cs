using CommonUtility;
using CommonUtility.BusLoader;
using Stateless;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Controller
{
    public sealed class Cx1EEmc : ControllerBase
    {
        public enum EmcState
        {
            Sleep,

            Awake,

            Diagnosis,

            Run,

            NoMessage
        }

        public enum EmcTrigger
        {
            StartCan,

            StopCan,

            StartDiagnosis,

            StartIoControl
        }

        public enum EmcType
        {
            Curr,

            State
        }

        private const uint ReqCanId = 0x712;
        private const uint RecvCanId = 0x612;

        /// <summary>
        ///     定义锁对象，用于同步访问共享资源
        /// </summary>
        private readonly object _lock = new object();

        private readonly CanCommunicationMatrix.MotorolaMatrix _cemBodyFr03_0X76 =
            new CanCommunicationMatrix.MotorolaMatrix(0x76, 8); // 20ms

        private readonly CanCommunicationMatrix.MotorolaMatrix _cemBodyFr09_0X41 =
            new CanCommunicationMatrix.MotorolaMatrix(0x41, 8); // 15ms

        private readonly CanCommunicationMatrix.MotorolaMatrix _cemBodyFr103_0X5B =
            new CanCommunicationMatrix.MotorolaMatrix(0x5B, 8); // 15ms

        private readonly CanCommunicationMatrix.MotorolaMatrix _cemBodyFr108_0X00F3 =
            new CanCommunicationMatrix.MotorolaMatrix(0xF3, 8); // 60ms

        private readonly CanCommunicationMatrix.MotorolaMatrix _cemBodyFr11_0X00E0 =
            new CanCommunicationMatrix.MotorolaMatrix(0xE0, 8); // 40ms 

        private readonly CanCommunicationMatrix.MotorolaMatrix _cemBodyFr12_0X00F0 =
            new CanCommunicationMatrix.MotorolaMatrix(0xF0, 8); // 65ms

        private readonly CanCommunicationMatrix.MotorolaMatrix _cemBodyFr124_0X194 =
            new CanCommunicationMatrix.MotorolaMatrix(0x194, 8); // 40ms

        private readonly CanCommunicationMatrix.MotorolaMatrix _cemBodyFr179_0X346 =
            new CanCommunicationMatrix.MotorolaMatrix(0x346, 8); // 60ms

        private readonly CanCommunicationMatrix.MotorolaMatrix _cemBodyFr19_0X210 =
            new CanCommunicationMatrix.MotorolaMatrix(0X210, 8); // 390ms

        private readonly CanCommunicationMatrix.MotorolaMatrix _cemBodyFr78_0X00B4 =
            new CanCommunicationMatrix.MotorolaMatrix(0xB4, 8); // 50ms

        private readonly List<MyTimer> _timers = new List<MyTimer>();
        private readonly List<MyTimer> _timers2F = new List<MyTimer>();
        private readonly List<MyTimer> _timers22 = new List<MyTimer>();
        private Thread _th;
        private Thread _th2F;
        private Thread _th22;
        private bool _isStop = true;

        private readonly CanCommunicationMatrix.MotorolaMatrix _wakeup0X53F =
            new CanCommunicationMatrix.MotorolaMatrix(0x53F, 8); // 500ms

        public readonly Dictionary<uint, byte[]> List2FBytes = new Dictionary<uint, byte[]>
        {
            { 0xFE0C, new byte[1] },
            { 0xD90D, new byte[2] },
            { 0x4002, new byte[1] },
            { 0x4003, new byte[1] },
            { 0x400E, new byte[1] },
            { 0x4010, new byte[1] },
            { 0x4051, new byte[1] },
            { 0x4052, new byte[1] },
            { 0x4053, new byte[1] },
            { 0x4055, new byte[1] },
            { 0x40AE, new byte[1] },
        };

        private byte _rbChdLockReLeCtrlHmiReq;
        private byte _rbPedal;

        private EmcState _state = EmcState.Sleep;
        public CanBus AppCan;
        public CanBus Can2;
        public LinBus Lin;

        public DateTime LastAppCanRecvDate = DateTime.Now.Subtract(TimeSpan.FromMinutes(10));
        public DateTime LastCan2RecvDate = DateTime.Now.Subtract(TimeSpan.FromMinutes(10));
        public DateTime LastLinRecvDate = DateTime.Now.Subtract(TimeSpan.FromMinutes(10));

        /// <summary>
        ///     22读的0x27的第八帧
        ///     this.byte(3);
        /// </summary>
        [EmcAttribute(EmcType.State, "老板键1")]
        public string BossIncSts = string.Empty;

        /// <summary>
        ///     22读的0x27的第八帧
        ///     this.byte(4);
        /// </summary>
        [EmcAttribute(EmcType.State, "老板键2")]
        public string BossSldSts = string.Empty;

        [EmcAttribute(EmcType.Curr, "电动门电机")]
        public double CurrDoor = -9999;

        [EmcAttribute(EmcType.Curr, "儿童锁/电释放电机")]
        public double CurrElectricRelese = -9999;

        [EmcAttribute(EmcType.Curr, "门把手电机")]
        public double CurrHandle = -9999;

        [EmcAttribute(EmcType.Curr, "吸合锁电机")]
        public double CurrLock2 = -9999;

        [EmcAttribute(EmcType.Curr, "中控锁/电释放复位电机")]
        public double CurrLockActvnLock = -9999;

        [EmcAttribute(EmcType.Curr, "后视镜X电机")]
        public double CurrMirrorX = -9999;

        [EmcAttribute(EmcType.Curr, "后视镜Y电机")]
        public double CurrMirrorY = -9999;

        [EmcAttribute(EmcType.Curr, "电动脚踏电机")]
        public double CurrPedal = -9999;

        [EmcAttribute(EmcType.Curr, "车窗电机")]
        public double CurrWindow = -9999;

        /// <summary>
        ///     外门把手位置显示,0x123,19,3
        /// </summary>
        [EmcAttribute(EmcType.State, "外门把手位置显示")]
        public string DoorDrvrHndlSts = string.Empty;

        /// <summary>
        ///     门Ajar开关状态,0x05,46,2
        /// </summary>
        [EmcAttribute(EmcType.State, "门Ajar开关状态")]
        public string DoorDrvrLatPosn = string.Empty;

        /// <summary>
        ///     中控锁状态反馈,0x05,44,2
        /// </summary>
        [EmcAttribute(EmcType.State, "中控锁状态反馈")]
        public string DoorDrvrLockSts = string.Empty;

        /// <summary>
        ///     外门把手1,0x05,22,2
        /// </summary>
        [EmcAttribute(EmcType.State, "外门把手1")]
        public string DoorDrvrOpenLowReqOutdSwt1 = string.Empty;

        /// <summary>
        ///     内门把手2,0x05,24,2
        /// </summary>
        [EmcAttribute(EmcType.State, "内门把手2")]
        public string DoorDrvrOpenReqInsdSwt1 = string.Empty;

        /// <summary>
        ///     内门把手1,0x256,28,2
        /// </summary>
        [EmcAttribute(EmcType.State, "内门把手1")]
        public string DoorDrvrOpenReqInsdSwtCan2 = string.Empty;

        /// <summary>
        ///     中控锁开关状态,0x05,42,2
        /// </summary>
        [EmcAttribute(EmcType.State, "中控锁开关状态")]
        public string DoorDrvrSwtIntrLockgReq = string.Empty;

        public byte FronHandleLight;

        public bool IsHavemsg = true;

        [EmcAttribute(EmcType.State, "吸合锁复位开关")]
        public string LockResetSts = string.Empty;

        public StateMachine<EmcState, EmcTrigger> Machine;

        public double MirrPosnStsAtDrvrMirrPosnAdjCldLeRi = -9999;
        public double MirrPosnStsAtDrvrMirrPosnAdjCldUpDwn = -9999;
        public byte MotorCmd;
        public double MotorDoorSpeed = -9999;
        public double MotorPedalSpeed = -9999;

        /// <summary>
        ///     门全开开关状态,0x123,59,1
        /// </summary>
        [EmcAttribute(EmcType.State, "门全开开关状态")]
        public string OpenDrvrSwt = string.Empty;

        /// <summary>
        ///     棘爪（半开）开关状态,0x256,3,1
        /// </summary>
        [EmcAttribute(EmcType.State, "棘爪（半开）开关状态")]
        public string PawlDrvrSwt = string.Empty;

        public bool SwBsd;
        public bool SwPocketLight;
        public bool SwPowSupplyCmdPos;
        public bool SwPowSupplyConDdSswitch;
        public bool SwPowSupplyConDoorHall;
        public bool SwPowSupplyConHandleHall;

        public bool SwPowSupplyConPendalHall;
        public bool SwStepLight;
        public double Temperature = -9999;

        public double Vba1 = -9999;
        public double Vba2 = -9999;
        public byte WindowLight;

        /// <summary>
        ///     车窗本地开关状态,0xC0,4,3
        /// </summary>
        [EmcAttribute(EmcType.State, "车窗本地开关状态")]
        public string WinSwtReqFrntLe = string.Empty;

        public Cx1EEmc(string name)
            : base(name)
        {
            CanBus.PushCanMsg += CanBus_PushCanMsg;
            LinBus.PushLinMsg += LinBus_PushLinMsg;
            InitScheduler();
            InitMachine();
        }

        private void LinBus_PushLinMsg(string name, LinBus.LinDataPackage data)
        {
            if (Lin != null && Lin.Name == name)
            {
                LastLinRecvDate = DateTime.Now;
            }
        }

        private void CanBus_PushCanMsg(
            string name, CanBus.CanDataPackage data, CanBus.OnPushCanDataType onPushCanDataType)
        {
            if (data.CanId == 0xffffffff)
                return;

            if (AppCan != null && AppCan.Name == name && onPushCanDataType != CanBus.OnPushCanDataType.Tx)
            {
                LastAppCanRecvDate = DateTime.Now;

                //if (_state == EmcState.Run)
                {
                    //if (data.CanId == 0x123 && data.CanDataLen == 8 &&
                    //    data.CanData[0] == 0x00 && data.CanData[1] == 0x00 && data.CanData[2] == 0x00 && data.CanData[3] == 0x00 && data.CanData[4] == 0x00 && data.CanData[5] == 0x00
                    //    && data.CanData[6] == 0x2B && data.CanData[7] == 0xC0)
                    //{
                    //    return;
                    //}

                    if (data.CanId == RecvCanId && data.CanDataLen == 8)
                    {
                        if (data.CanData[0] == 0x10 && data.CanData[1] == 0x35 && data.CanData[2] == 0x62)
                        {
                            if (CurrMirrorX >= 0)
                            {
                                var iMirrorX = (ushort)CurrMirrorX;
                                var iMirrorXBs = BitConverter.GetBytes(iMirrorX).Reverse().ToArray();
                                iMirrorXBs[0] = data.CanData[7];
                                CurrMirrorX = Math.Round((double)BitConverter.ToUInt16(iMirrorXBs.Reverse().ToArray(), 0),
                                    2, MidpointRounding.AwayFromZero);
                            }
                            else
                            {
                                var iMirrorXBs = new byte[2];
                                iMirrorXBs[0] = data.CanData[7];
                                CurrMirrorX = Math.Round((double)BitConverter.ToUInt16(iMirrorXBs.Reverse().ToArray(), 0),
                                    2, MidpointRounding.AwayFromZero);
                            }

                            CurrMirrorY = Math.Round((double)data.CanData[5] * 256 + data.CanData[6], 2,
                                MidpointRounding.AwayFromZero);
                        }
                        else if (data.CanData[0] == 0x21)
                        {
                            if (CurrMirrorX >= 0)
                            {
                                var iMirrorX = (ushort)CurrMirrorX;
                                var iMirrorXBs = BitConverter.GetBytes(iMirrorX).Reverse().ToArray();
                                iMirrorXBs[1] = data.CanData[1];
                                CurrMirrorX = Math.Round((double)BitConverter.ToUInt16(iMirrorXBs.Reverse().ToArray(), 0),
                                    2, MidpointRounding.AwayFromZero);
                            }
                            else
                            {
                                var iMirrorXBs = new byte[2];
                                iMirrorXBs[1] = data.CanData[1];
                                CurrMirrorX = Math.Round((double)BitConverter.ToUInt16(iMirrorXBs.Reverse().ToArray(), 0),
                                    2, MidpointRounding.AwayFromZero);
                            }

                            CurrWindow = Math.Round((double)data.CanData[2] * 256 + data.CanData[3], 2,
                                MidpointRounding.AwayFromZero);
                            CurrElectricRelese = Math.Round((double)data.CanData[6] * 256 + data.CanData[7], 2,
                                MidpointRounding.AwayFromZero);
                        }
                        else if (data.CanData[0] == 0x22)
                        {
                            CurrLockActvnLock = Math.Round((double)data.CanData[1] * 256 + data.CanData[2], 2,
                                MidpointRounding.AwayFromZero);
                            CurrHandle = Math.Round((double)data.CanData[3] * 256 + data.CanData[4], 2,
                                MidpointRounding.AwayFromZero);
                            CurrLock2 = Math.Round((double)data.CanData[5] * 256 + data.CanData[6], 2,
                                MidpointRounding.AwayFromZero);

                            if (CurrPedal >= 0)
                            {
                                var iPedal = (ushort)CurrPedal;
                                var iPedalBs = BitConverter.GetBytes(iPedal).Reverse().ToArray();
                                iPedalBs[0] = data.CanData[7];
                                CurrPedal = Math.Round((double)BitConverter.ToUInt16(iPedalBs.Reverse().ToArray(), 0),
                                    2, MidpointRounding.AwayFromZero);
                            }
                            else
                            {
                                var iPedalBs = new byte[2];
                                iPedalBs[0] = data.CanData[7];
                                CurrPedal = Math.Round((double)BitConverter.ToUInt16(iPedalBs.Reverse().ToArray(), 0),
                                    2, MidpointRounding.AwayFromZero);
                            }
                        }
                        else if (data.CanData[0] == 0x23)
                        {
                            Vba1 = Math.Round((double)data.CanData[6] * 256 + data.CanData[7], 2,
                                MidpointRounding.AwayFromZero);
                            Vba2 = Math.Round((double)data.CanData[4] * 256 + data.CanData[5], 2,
                                MidpointRounding.AwayFromZero);
                            CurrDoor = Math.Round((double)data.CanData[2] * 256 + data.CanData[3], 2,
                                MidpointRounding.AwayFromZero);

                            if (CurrPedal >= 0)
                            {
                                var iPedal = (ushort)CurrPedal;
                                var iPedalBs = BitConverter.GetBytes(iPedal).Reverse().ToArray();
                                iPedalBs[1] = data.CanData[1];
                                CurrPedal = Math.Round((double)BitConverter.ToUInt16(iPedalBs.Reverse().ToArray(), 0),
                                    2, MidpointRounding.AwayFromZero);
                            }
                            else
                            {
                                var iPedalBs = new byte[2];
                                iPedalBs[1] = data.CanData[1];
                                CurrPedal = Math.Round((double)BitConverter.ToUInt16(iPedalBs.Reverse().ToArray(), 0),
                                    2, MidpointRounding.AwayFromZero);
                            }
                        }
                        else if (data.CanData[0] == 0x24)
                        {
                            Temperature = Math.Round((double)data.CanData[2] * 256 + data.CanData[3], 2,
                                MidpointRounding.AwayFromZero);

                            LockResetSts = data.CanData[1].ToString();
                        }
                        else if (data.CanData[0] == 0x26)
                        {
                            MotorPedalSpeed = Math.Round((double)data.CanData[6] * 256 + data.CanData[7], 2,
                                MidpointRounding.AwayFromZero);
                        }
                        else if (data.CanData[0] == 0x27)
                        {
                            MotorDoorSpeed = Math.Round((double)data.CanData[1] * 256 + data.CanData[2], 2,
                                MidpointRounding.AwayFromZero);

                            BossIncSts = data.CanData[3].ToString();
                            BossSldSts = data.CanData[4].ToString();
                        }
                        else if (data.CanData[1] == 0x7F && data.CanData[2] == 0x2F)
                        {
                            var textToWrite = string.Format("{0}: {1}", name, ValueHelper.GetHextStrWithOx(data.CanData));
                            var logFilePath = string.Format(@"{0}\CX1E_2F_ErrorMsg_{1}.txt", Directory.GetCurrentDirectory(), Guid.NewGuid().ToString().Replace("-", ""));

                            Task.Run(() =>
                            {
                                // 使用StreamWriter类写入文本到文件
                                // 第二个参数为 false 表示若文件存在，则覆写；为 true 表示追加内容
                                //using (var writer = new StreamWriter(logFilePath, false))
                                //    writer.WriteLine(textToWrite);
                            });

                            if (Machine.CanFire(EmcTrigger.StartDiagnosis))
                                Machine.Fire(EmcTrigger.StartDiagnosis);
                        }
                    }
                    else if (data.CanId == 0x05)
                    {
                        try
                        {
                            var matrix0X05 = new CanCommunicationMatrix.MotorolaMatrix(0x05, 8) { MatrixData = data.CanData };

                            // DoorDrvrLatPosn
                            if (matrix0X05.GetMatrixData(46, 2) == 0)
                                DoorDrvrLatPosn = "Off";
                            else if (matrix0X05.GetMatrixData(46, 2) == 1)
                                DoorDrvrLatPosn = "On_全关";
                            else if (matrix0X05.GetMatrixData(46, 2) == 2)
                                DoorDrvrLatPosn = "On_第二位置";
                            else if (matrix0X05.GetMatrixData(46, 2) == 3)
                                DoorDrvrLatPosn = "On_全开";

                            // DoorDrvrSwtIntrLockgReq
                            if (matrix0X05.GetMatrixData(42, 2) == 0)
                                DoorDrvrSwtIntrLockgReq = "Off";
                            else if (matrix0X05.GetMatrixData(42, 2) == 1)
                                DoorDrvrSwtIntrLockgReq = "On";

                            // DoorDrvrLockSts
                            if (matrix0X05.GetMatrixData(44, 2) == 0)
                                DoorDrvrLockSts = "Off";
                            else if (matrix0X05.GetMatrixData(44, 2) == 1)
                                DoorDrvrLockSts = "On_解锁";
                            else if (matrix0X05.GetMatrixData(44, 2) == 2)
                                DoorDrvrLockSts = "On_上锁";
                            else if (matrix0X05.GetMatrixData(44, 2) == 3)
                                DoorDrvrLockSts = "On_安全锁";

                            // DoorDrvrOpenReqInsdSwt1
                            if (matrix0X05.GetMatrixData(24, 2) == 0)
                                DoorDrvrOpenReqInsdSwt1 = "Off_0";
                            else if (matrix0X05.GetMatrixData(24, 2) == 1)
                                DoorDrvrOpenReqInsdSwt1 = "On";
                            else if (matrix0X05.GetMatrixData(24, 2) == 2)
                                DoorDrvrOpenReqInsdSwt1 = "Off_2";
                            else if (matrix0X05.GetMatrixData(24, 2) == 3)
                                DoorDrvrOpenReqInsdSwt1 = "On";

                            // DoorDrvrOpenLowReqOutdSwt1
                            if (matrix0X05.GetMatrixData(22, 2) == 0)
                                DoorDrvrOpenLowReqOutdSwt1 = "Off_0";
                            else if (matrix0X05.GetMatrixData(22, 2) == 1)
                                DoorDrvrOpenLowReqOutdSwt1 = "On";
                            else if (matrix0X05.GetMatrixData(22, 2) == 2)
                                DoorDrvrOpenLowReqOutdSwt1 = "Off_2";
                            else if (matrix0X05.GetMatrixData(22, 2) == 3)
                                DoorDrvrOpenLowReqOutdSwt1 = "On";
                        }
                        catch (Exception)
                        {
                            DoorDrvrLatPosn = string.Empty;
                            DoorDrvrSwtIntrLockgReq = string.Empty;
                            DoorDrvrLockSts = string.Empty;
                            DoorDrvrOpenReqInsdSwt1 = string.Empty;
                            DoorDrvrOpenLowReqOutdSwt1 = string.Empty;
                        }
                    }
                    else if (data.CanId == 0x123)
                    {
                        try
                        {
                            var matrix0X123 = new CanCommunicationMatrix.MotorolaMatrix(0x123, 8) { MatrixData = data.CanData };

                            //if (ValueHelper.GetHextStrWithOx(data.CanData).ToLower() == "0x00 0x00 0x00 0x00 0x00 0x00 0x2B 0xC0".ToLower())
                            //{
                            //    return;
                            //}

                            if (matrix0X123.GetMatrixData(59, 1) == 0)
                                OpenDrvrSwt = "Off";
                            else if (matrix0X123.GetMatrixData(59, 1) == 1)
                                OpenDrvrSwt = "On";

                            if (matrix0X123.GetMatrixData(19, 3) == 0)
                                DoorDrvrHndlSts = "Ukwn";
                            else if (matrix0X123.GetMatrixData(19, 3) == 1)
                                DoorDrvrHndlSts = "Off";
                            else if (matrix0X123.GetMatrixData(19, 3) == 2)
                                DoorDrvrHndlSts = "On_2";
                            else if (matrix0X123.GetMatrixData(19, 3) == 3)
                                DoorDrvrHndlSts = "On_3";
                            else if (matrix0X123.GetMatrixData(19, 3) == 4)
                                DoorDrvrHndlSts = "On_4";

                            if (DoorDrvrHndlSts == "Ukwn")
                            {
                                var textToWrite = string.Format("{0}: {1}", name, ValueHelper.GetHextStrWithOx(data.CanData));
                                var logFilePath = string.Format(@"{0}\CX1E_DoorDrvrHndlSts=Ukwn_ErrorMsg_{1}.txt", Directory.GetCurrentDirectory(), Guid.NewGuid().ToString().Replace("-", ""));

                                Task.Run(() =>
                                {
                                    // 使用StreamWriter类写入文本到文件
                                    // 第二个参数为 false 表示若文件存在，则覆写；为 true 表示追加内容
                                    //using (var writer = new StreamWriter(logFilePath, false))
                                    //    writer.WriteLine(textToWrite);
                                });
                            }
                        }
                        catch (Exception)
                        {
                            OpenDrvrSwt = string.Empty;
                            DoorDrvrHndlSts = string.Empty;
                        }
                    }
                    else if (data.CanId == 0x256)
                    {
                        try
                        {
                            var matrix0X256 = new CanCommunicationMatrix.MotorolaMatrix(0x256, 8) { MatrixData = data.CanData };

                            if (matrix0X256.GetMatrixData(3, 1) == 0)
                                PawlDrvrSwt = "Off";
                            else if (matrix0X256.GetMatrixData(3, 1) == 1)
                                PawlDrvrSwt = "On";

                            //if (matrix0X256.GetMatrixData(28, 2) == 0)
                            //    DoorDrvrOpenReqInsdSwtCan2 = "Off_0";
                            //else if (matrix0X256.GetMatrixData(28, 2) == 1)
                            //    DoorDrvrOpenReqInsdSwtCan2 = "On";
                            //else if (matrix0X256.GetMatrixData(28, 2) == 2)
                            //    DoorDrvrOpenReqInsdSwtCan2 = "Off_2";
                            //else if (matrix0X256.GetMatrixData(28, 2) == 3)
                            //    DoorDrvrOpenReqInsdSwtCan2 = "Off_3";

                            if (matrix0X256.GetMatrixData(28, 2) == 0)
                                DoorDrvrOpenReqInsdSwtCan2 = "Off_0";
                            else if (matrix0X256.GetMatrixData(28, 2) == 1)
                                DoorDrvrOpenReqInsdSwtCan2 = "On";
                            else if (matrix0X256.GetMatrixData(28, 2) == 2)
                                DoorDrvrOpenReqInsdSwtCan2 = "Off_2";
                            else if (matrix0X256.GetMatrixData(28, 2) == 3)
                                DoorDrvrOpenReqInsdSwtCan2 = "On";
                        }
                        catch (Exception)
                        {
                            PawlDrvrSwt = string.Empty;
                            DoorDrvrOpenReqInsdSwtCan2 = string.Empty;
                        }
                    }
                    else if (data.CanId == 0xC0)
                    {
                        try
                        {
                            var matrix0Xc0 = new CanCommunicationMatrix.MotorolaMatrix(0xC0, 8) { MatrixData = data.CanData };

                            if (matrix0Xc0.GetMatrixData(4, 3) == 0)
                                WinSwtReqFrntLe = "Off_0";
                            else if (matrix0Xc0.GetMatrixData(4, 3) == 1)
                                WinSwtReqFrntLe = "On";
                            else if (matrix0Xc0.GetMatrixData(4, 3) == 2)
                                WinSwtReqFrntLe = "Off_2";
                        }
                        catch (Exception)
                        {
                            WinSwtReqFrntLe = string.Empty;
                        }
                    }
                    else if (data.CanId == 0x115)
                    {
                        try
                        {
                            MirrPosnStsAtDrvrMirrPosnAdjCldLeRi = data.CanData[4];
                            MirrPosnStsAtDrvrMirrPosnAdjCldUpDwn = data.CanData[5];
                        }
                        catch (Exception)
                        {
                            MirrPosnStsAtDrvrMirrPosnAdjCldLeRi = -9999;
                            MirrPosnStsAtDrvrMirrPosnAdjCldUpDwn = -9999;
                        }
                    }
                }
            }

            if (Can2 != null && Can2.Name == name && onPushCanDataType != CanBus.OnPushCanDataType.Tx)
            {
                LastCan2RecvDate = DateTime.Now;
            }
        }

        private void InitMachine()
        {
            Machine = new StateMachine<EmcState, EmcTrigger>(() => _state, s => _state = s);

            Machine.Configure(EmcState.Sleep)
                .OnEntry(StopScheduler)
                .OnExit(StartScheduler)
                .Permit(EmcTrigger.StartCan, EmcState.Awake);

            Machine.Configure(EmcState.Awake)
                .Permit(EmcTrigger.StartDiagnosis, EmcState.Diagnosis)
                .Permit(EmcTrigger.StopCan, EmcState.Sleep);

            Machine.Configure(EmcState.Diagnosis)
                .OnEntry(DiagnosisEntryAction)
                .Permit(EmcTrigger.StartIoControl, EmcState.Run);

            Machine.Configure(EmcState.Run)
                .Permit(EmcTrigger.StartDiagnosis, EmcState.Diagnosis)
                .Permit(EmcTrigger.StopCan, EmcState.Sleep);
        }

        private async void DiagnosisEntryAction()
        {
            await Task.Delay(50);

            if (AppCan != null)
            {
                lock (_lock) AppCan.SendStandardCanData(ReqCanId, new byte[] { 0x02, 0x10, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00 });

                Thread.Sleep(50);

                lock (_lock) AppCan.SendStandardCanData(ReqCanId, new byte[] { 0x02, 0x10, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00 });

                Thread.Sleep(50);

                lock (_lock) AppCan.SendStandardCanData(ReqCanId, new byte[] { 0x02, 0x27, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00 });

                Thread.Sleep(50);

                lock (_lock) AppCan.SendStandardCanData(ReqCanId, new byte[] { 0x05, 0x27, 0x06, 0x11, 0x22, 0x33, 0x00, 0x00 });
            }

            await Machine.FireAsync(EmcTrigger.StartIoControl);
        }

        ~Cx1EEmc()
        {
            Dispose();
        }

        private void InitScheduler()
        {
            _wakeup0X53F.MatrixData = new byte[] { 0x3F, 0x40, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };

            #region UP_SET

            _cemBodyFr09_0X41.UpdateData(new MatrixValDefinition(50, 1, 1));
            //_cemBodyFr09_0X41.UpdateData(new MatrixValDefinition(59, 1, 1)); // canoe no use
            _cemBodyFr09_0X41.UpdateData(new MatrixValDefinition(61, 1, 1));

            _cemBodyFr103_0X5B.UpdateData(new MatrixValDefinition(2, 1, 1));
            _cemBodyFr103_0X5B.UpdateData(new MatrixValDefinition(5, 1, 1));
            _cemBodyFr103_0X5B.UpdateData(new MatrixValDefinition(11, 1, 1));
            _cemBodyFr103_0X5B.UpdateData(new MatrixValDefinition(12, 1, 1));
            _cemBodyFr103_0X5B.UpdateData(new MatrixValDefinition(15, 1, 1));
            //_cemBodyFr103_0X5B.UpdateData(new MatrixValDefinition(41, 1, 1)); // canoe no use

            _cemBodyFr03_0X76.UpdateData(new MatrixValDefinition(6, 1, 1));
            _cemBodyFr03_0X76.UpdateData(new MatrixValDefinition(23, 1, 1));
            //_cemBodyFr03_0X76.UpdateData(new MatrixValDefinition(40, 1, 1));
            //_cemBodyFr03_0X76.UpdateData(new MatrixValDefinition(41, 1, 1));
            //_cemBodyFr03_0X76.UpdateData(new MatrixValDefinition(60, 1, 1));

            //_cemBodyFr11_0X00E0.UpdateData(new MatrixValDefinition(0, 1, 1));
            //_cemBodyFr11_0X00E0.UpdateData(new MatrixValDefinition(4, 1, 1));
            //_cemBodyFr11_0X00E0.UpdateData(new MatrixValDefinition(10, 1, 1));
            _cemBodyFr11_0X00E0.UpdateData(new MatrixValDefinition(14, 1, 1));
            //_cemBodyFr11_0X00E0.UpdateData(new MatrixValDefinition(15, 1, 1));
            //_cemBodyFr11_0X00E0.UpdateData(new MatrixValDefinition(27, 1, 1));
            //_cemBodyFr11_0X00E0.UpdateData(new MatrixValDefinition(61, 1, 1));
            _cemBodyFr11_0X00E0.UpdateData(new MatrixValDefinition(29, 1, 1));
            _cemBodyFr11_0X00E0.UpdateData(new MatrixValDefinition(44, 1, 1));
            //_cemBodyFr11_0X00E0.UpdateData(new MatrixValDefinition(45, 1, 1));
            _cemBodyFr11_0X00E0.UpdateData(new MatrixValDefinition(55, 1, 1));
            //_cemBodyFr11_0X00E0.UpdateData(new MatrixValDefinition(60, 1, 1));

            //_cemBodyFr124_0X194.UpdateData(new MatrixValDefinition(8, 1, 1));
            _cemBodyFr124_0X194.UpdateData(new MatrixValDefinition(15, 1, 1));
            //_cemBodyFr124_0X194.UpdateData(new MatrixValDefinition(16, 1, 1));
            //_cemBodyFr124_0X194.UpdateData(new MatrixValDefinition(17, 1, 1));
            //_cemBodyFr124_0X194.UpdateData(new MatrixValDefinition(29, 1, 1));
            _cemBodyFr124_0X194.UpdateData(new MatrixValDefinition(32, 1, 1));
            _cemBodyFr124_0X194.UpdateData(new MatrixValDefinition(33, 1, 1));
            //_cemBodyFr124_0X194.UpdateData(new MatrixValDefinition(39, 1, 1));
            //_cemBodyFr124_0X194.UpdateData(new MatrixValDefinition(40, 1, 1));
            _cemBodyFr124_0X194.UpdateData(new MatrixValDefinition(50, 1, 1));
            //_cemBodyFr124_0X194.UpdateData(new MatrixValDefinition(57, 1, 1));
            //_cemBodyFr124_0X194.UpdateData(new MatrixValDefinition(60, 1, 1));
            //_cemBodyFr124_0X194.UpdateData(new MatrixValDefinition(63, 1, 1));

            _cemBodyFr78_0X00B4.UpdateData(new MatrixValDefinition(2, 1, 1));
            //_cemBodyFr78_0X00B4.UpdateData(new MatrixValDefinition(3, 1, 1)); // canoe no use
            //_cemBodyFr78_0X00B4.UpdateData(new MatrixValDefinition(26, 1, 1)); // canoe no use
            //_cemBodyFr78_0X00B4.UpdateData(new MatrixValDefinition(49, 1, 1)); // canoe no use
            //_cemBodyFr78_0X00B4.UpdateData(new MatrixValDefinition(52, 1, 1)); // canoe no use
            //_cemBodyFr78_0X00B4.UpdateData(new MatrixValDefinition(55, 1, 1)); // canoe no use
            //_cemBodyFr78_0X00B4.UpdateData(new MatrixValDefinition(56, 1, 1)); // canoe no use

            //_cemBodyFr108_0X00F3.UpdateData(new MatrixValDefinition(30, 1, 1)); // canoe no use
            //_cemBodyFr108_0X00F3.UpdateData(new MatrixValDefinition(51, 1, 1)); // canoe no use
            _cemBodyFr108_0X00F3.UpdateData(new MatrixValDefinition(52, 1, 1));

            //_cemBodyFr179_0X346.UpdateData(new MatrixValDefinition(9, 1, 1)); // canoe no use
            //_cemBodyFr179_0X346.UpdateData(new MatrixValDefinition(43, 1, 1)); // canoe no use
            _cemBodyFr179_0X346.UpdateData(new MatrixValDefinition(51, 1, 1));
            //_cemBodyFr179_0X346.UpdateData(new MatrixValDefinition(56, 1, 1)); // canoe no use
            _cemBodyFr179_0X346.UpdateData(new MatrixValDefinition(59, 1, 1));

            _cemBodyFr12_0X00F0.UpdateData(new MatrixValDefinition(7, 1, 1));
            //_cemBodyFr12_0X00F0.UpdateData(new MatrixValDefinition(46, 1, 1));
            _cemBodyFr12_0X00F0.UpdateData(new MatrixValDefinition(47, 1, 1));
            _cemBodyFr12_0X00F0.UpdateData(new MatrixValDefinition(52, 1, 1));
            _cemBodyFr12_0X00F0.UpdateData(new MatrixValDefinition(53, 1, 1));
            _cemBodyFr12_0X00F0.UpdateData(new MatrixValDefinition(54, 1, 1));

            //_cemBodyFr19_0X210.UpdateData(new MatrixValDefinition(28, 1, 1));
            //_cemBodyFr19_0X210.UpdateData(new MatrixValDefinition(31, 1, 1));
            _cemBodyFr19_0X210.UpdateData(new MatrixValDefinition(56, 1, 1));
            _cemBodyFr19_0X210.UpdateData(new MatrixValDefinition(58, 1, 1));

            #endregion

            #region TIMER_SET

            _timers.Add(new MyTimer { Period = 15, Timer = new RecurringTaskScheduler(DoMatrixWork(_cemBodyFr09_0X41), 15) });
            _timers.Add(new MyTimer { Period = 15, Timer = new RecurringTaskScheduler(DoMatrixWork(_cemBodyFr103_0X5B), 15) });
            _timers.Add(new MyTimer { Period = 20, Timer = new RecurringTaskScheduler(DoMatrixWork(_cemBodyFr03_0X76), 20) });

            _timers.Add(new MyTimer { Period = 45, Timer = new RecurringTaskScheduler(DoMatrixWork(_cemBodyFr11_0X00E0), 45) });
            _timers.Add(new MyTimer { Period = 45, Timer = new RecurringTaskScheduler(DoMatrixWork(_cemBodyFr124_0X194), 45) });
            _timers.Add(new MyTimer { Period = 50, Timer = new RecurringTaskScheduler(DoMatrixWork(_cemBodyFr78_0X00B4), 50) });
            _timers.Add(new MyTimer { Period = 60, Timer = new RecurringTaskScheduler(DoMatrixWork(_cemBodyFr108_0X00F3), 60) });
            _timers.Add(new MyTimer { Period = 60, Timer = new RecurringTaskScheduler(DoMatrixWork(_cemBodyFr179_0X346), 60) });
            _timers.Add(new MyTimer { Period = 65, Timer = new RecurringTaskScheduler(DoMatrixWork(_cemBodyFr12_0X00F0), 65) });
            _timers.Add(new MyTimer { Period = 100, Timer = new RecurringTaskScheduler(DoMatrixWork(_cemBodyFr19_0X210), 100) });

            _timers.Add(new MyTimer { Period = 250, Timer = new RecurringTaskScheduler(DoMatrixWork(_wakeup0X53F), 250) });

            _timers22.Add(new MyTimer
            {
                Period = 100,
                Timer = new RecurringTaskScheduler(Do22Work(), 100)
            });

            #endregion

            #region THREAD_SET

            foreach (var t in List2FBytes)
                _timers2F.Add(new MyTimer
                {
                    Period = 100,
                    Timer = new RecurringTaskScheduler(Do2FWork(t.Key), 100)
                });

            if (_th != null)
            {
                _th.Abort();
                _th.Join();
            }
            _th = new Thread(NetWork) { IsBackground = true };
            _th.Start();

            if (_th2F != null)
            {
                _th2F.Abort();
                _th2F.Join();
            }
            _th2F = new Thread(NetWork2F) { IsBackground = true };
            _th2F.Start();

            if (_th22 != null)
            {
                _th22.Abort();
                _th22.Join();
            }

            _th22 = new Thread(NetWork22) { IsBackground = true };
            _th22.Start();

            #endregion
        }

        private void NetWork()
        {
            var count = 0;
            const int period = 5;

            while (_th.IsAlive)
            {
                if (!_th.IsAlive)
                    return;

                Thread.Sleep(period);

                if (_isStop)
                {
                    count = 0;
                    continue;
                }

                count++;

                foreach (var t in _timers.Where(t => (count * period) % t.Period == 0))
                {
                    t.Timer.TaskToRun();
                }

                if (count * period == 500000)
                    count = 0;
            }
        }

        private void NetWork2F()
        {
            while (_th2F.IsAlive)
            {
                if (!_th2F.IsAlive)
                    return;

                Thread.Sleep(10);

                if (_isStop || Machine.State != EmcState.Run)
                    continue;

                foreach (var t in _timers22)
                    t.Timer.TaskToRun();

                Thread.Sleep(100);

                foreach (var t in _timers2F)
                {
                    t.Timer.TaskToRun();
                    Thread.Sleep(5);

                    if (_isStop || Machine.State != EmcState.Run)
                        break;
                }
            }
        }

        private void NetWork22()
        {
            while (_th22.IsAlive)
            {
                if (!_th22.IsAlive)
                    return;

                Thread.Sleep(100);

                if (_isStop)
                    continue;

                //foreach (var t in _timers22)
                //    t.Timer.TaskToRun();
            }
        }

        private void StartScheduler()
        {
            _isStop = false;
        }

        private void StopScheduler()
        {
            _isStop = true;
        }

        private Action DoMatrixWork(object state)
        {
            return () =>
            {
                var matrix = state as CanCommunicationMatrix;

                if (AppCan == null || matrix == null)
                    return;

                lock (_lock)
                    AppCan.SendStandardCanData(matrix.CanId, matrix.MatrixData);
            };
        }

        private Action Do22Work()
        {
            return () =>
            {
                if (AppCan == null)
                    return;

                lock (_lock)
                {
                    byte[] echo;
                    AppCan.CanBusWithUds.TryReadData(ReqCanId, RecvCanId, CanBus.CanType.Standard,
                        CanBus.CanProtocol.Can, 0xD2, 0x14, out echo, timeoutFromMilliseconds: 100);

                    //AppCan.SendStandardCanData(ReqCanId, new byte[] { 0x03, 0x22, 0xd2, 0X14, 0x00, 0x00, 0x00, 0x00 });
                    //Thread.Sleep(10);
                    //AppCan.SendStandardCanData(ReqCanId, new byte[] { 0x30, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                }
            };
        }

        private Action Do2FWork(object state)
        {
            return () =>
            {
                if (AppCan == null)
                    return;

                lock (_lock)
                {
                    if (AppCan == null || !(state is uint) || Machine.State != EmcState.Run)
                        return;

                    var key = (uint)state;

                    var didHi = BitConverter.GetBytes(key)[1];
                    var didLo = BitConverter.GetBytes(key)[0];

                    var value = List2FBytes[key];
                    if (value.Length == 1)
                    {
                        if (didHi == 0xFE && didLo == 0x0C)
                        {
                            var bitStr0 = new List<string>();
                            for (var i = Convert.ToString(value[0], 2).PadLeft(8, '0').Length - 1; i >= 0; i--)
                                bitStr0.Add(Convert.ToString(value[0], 2).PadLeft(8, '0')[i].ToString());

                            bitStr0[1] = SwPowSupplyConPendalHall ? "1" : "0";
                            bitStr0[2] = SwPowSupplyCmdPos ? "1" : "0";
                            bitStr0[6] = SwPowSupplyConHandleHall ? "1" : "0";
                            bitStr0[5] = SwPowSupplyConDoorHall ? "1" : "0";
                            bitStr0[0] = SwPowSupplyConDdSswitch ? "1" : "0";

                            if (MotorCmd == 0)
                            {
                                bitStr0[3] = "0";
                                bitStr0[4] = "0";
                            }
                            else if (MotorCmd == 1)
                            {
                                bitStr0[3] = "1";
                                bitStr0[4] = "0";
                            }
                            else if (MotorCmd == 2)
                            {
                                bitStr0[3] = "0";
                                bitStr0[4] = "1";
                            }

                            var str0 = string.Empty;
                            for (var i = bitStr0.Count - 1; i >= 0; i--)
                                str0 += bitStr0[i];

                            value[0] = Convert.ToByte(str0, 2);
                            List2FBytes[0xFE0C][0] = value[0];
                        }

                        AppCan.SendStandardCanData(ReqCanId,
                            new byte[] { 0x05, 0x2F, didHi, didLo, 0x03, value[0], 0x00, 0x00 });
                    }
                    else if (value.Length == 2)
                    {
                        if (didHi == 0xD9 && didLo == 0x0D)
                        {
                            var bitStr0 = new List<string>();
                            for (var i = Convert.ToString(value[0], 2).PadLeft(8, '0').Length - 1; i >= 0; i--)
                                bitStr0.Add(Convert.ToString(value[0], 2).PadLeft(8, '0')[i].ToString());
                            var bitStr1 = new List<string>();
                            for (var i = Convert.ToString(value[1], 2).PadLeft(8, '0').Length - 1; i >= 0; i--)
                                bitStr1.Add(Convert.ToString(value[1], 2).PadLeft(8, '0')[i].ToString());

                            bitStr0[6] = SwBsd ? "1" : "0";
                            bitStr0[7] = SwPocketLight ? "1" : "0";
                            bitStr1[0] = SwStepLight ? "1" : "0";

                            if (_rbChdLockReLeCtrlHmiReq == 0)
                            {
                                bitStr1[2] = "0";
                                bitStr1[1] = "0";
                            }
                            else if (_rbChdLockReLeCtrlHmiReq == 1)
                            {
                                bitStr1[2] = "0";
                                bitStr1[1] = "1";
                            }
                            else if (_rbChdLockReLeCtrlHmiReq == 2)
                            {
                                bitStr1[2] = "1";
                                bitStr1[1] = "0";
                            }

                            if (FronHandleLight == 0)
                            {
                                bitStr0[1] = "0";
                                bitStr0[0] = "0";
                            }
                            else if (FronHandleLight == 1)
                            {
                                bitStr0[1] = "0";
                                bitStr0[0] = "1";
                            }
                            else if (FronHandleLight == 2)
                            {
                                bitStr0[1] = "1";
                                bitStr0[0] = "0";
                            }

                            if (WindowLight == 0)
                            {
                                bitStr0[3] = "0";
                                bitStr0[2] = "0";
                            }
                            else if (WindowLight == 1)
                            {
                                bitStr0[3] = "0";
                                bitStr0[2] = "1";
                            }
                            else if (WindowLight == 2)
                            {
                                bitStr0[3] = "1";
                                bitStr0[2] = "0";
                            }

                            if (_rbPedal == 0)
                            {
                                bitStr0[5] = "0";
                                bitStr0[4] = "0";
                            }
                            else if (_rbPedal == 1)
                            {
                                bitStr0[5] = "0";
                                bitStr0[4] = "1";
                            }
                            else if (_rbPedal == 2)
                            {
                                bitStr0[5] = "1";
                                bitStr0[4] = "0";
                            }

                            var str0 = string.Empty;
                            for (var i = bitStr0.Count - 1; i >= 0; i--)
                                str0 += bitStr0[i];

                            var str1 = string.Empty;
                            for (var i = bitStr1.Count - 1; i >= 0; i--)
                                str1 += bitStr1[i];

                            value[0] = Convert.ToByte(str0, 2);
                            value[1] = Convert.ToByte(str1, 2);

                            List2FBytes[0xD90D][0] = value[0];
                            List2FBytes[0xD90D][1] = value[1];
                        }

                        AppCan.SendStandardCanData(ReqCanId,
                            new byte[] { 0x06, 0x2F, didHi, didLo, 0x03, value[0], value[1], 0x00 });
                    }
                }

                Thread.Sleep(120);

                lock (_lock)
                    AppCan.SendStandardCanData(ReqCanId, CanBus.MyUds.KeepExtendedSessionBytes());
                Thread.Sleep(50);
            };
        }

        private class MyTimer
        {
            public int Period;
            public RecurringTaskScheduler Timer;
        }

        public class RecurringTaskScheduler
        {
            public Action TaskToRun;
            public int Interval;

            public RecurringTaskScheduler(Action taskToRun, int interval)
            {
                TaskToRun = taskToRun;
                Interval = interval;
            }
        }

        public class EmcAttribute : Attribute
        {
            public EmcType EmcType;
            public string Name;

            public EmcAttribute(EmcType emcType, string name)
            {
                EmcType = emcType;
                Name = name;
            }
        }

        #region

        /// <summary>
        ///     CAN启用
        /// </summary>
        public void StartCan()
        {
            if (Machine.CanFire(EmcTrigger.StartCan))
                Machine.Fire(EmcTrigger.StartCan);
        }

        /// <summary>
        ///     CAN关闭
        /// </summary>
        public void StopCan()
        {
            if (Machine.CanFire(EmcTrigger.StopCan))
                Machine.Fire(EmcTrigger.StopCan);
        }

        /// <summary>
        ///     开启诊断
        /// </summary>
        public void StartDiagnosis()
        {
            if (Machine.CanFire(EmcTrigger.StartDiagnosis))
                Machine.Fire(EmcTrigger.StartDiagnosis);
        }

        /// <summary>
        ///     防眩目调光
        /// </summary>
        /// <param name="perValue"></param>
        public void MirrTintgCmd(byte perValue)
        {
            if (perValue <= 100)
                _cemBodyFr11_0X00E0.UpdateData(new MatrixValDefinition(48, 7, perValue));
        }

        /// <summary>
        ///     后视镜加热
        /// </summary>
        public void MirrDefrstAtDrvrCmd(bool isOn)
        {
            _cemBodyFr19_0X210.UpdateData(isOn
                ? new MatrixValDefinition(59, 1, 1)
                : new MatrixValDefinition(59, 1, 0));

            _cemBodyFr19_0X210.UpdateData(isOn
                ? new MatrixValDefinition(57, 1, 1)
                : new MatrixValDefinition(57, 1, 0));
        }

        /// <summary>
        ///     转向灯
        /// </summary>
        public void ActvnOfIndcrIndcrOut(bool isOn)
        {
            _cemBodyFr03_0X76.UpdateData(isOn
                ? new MatrixValDefinition(4, 2, 1)
                : new MatrixValDefinition(4, 2, 0));
        }

        /// <summary>
        ///     照地灯
        /// </summary>
        public void ActvnOfPudLi(bool isOn)
        {
            _cemBodyFr108_0X00F3.UpdateData(isOn
                ? new MatrixValDefinition(53, 1, 1)
                : new MatrixValDefinition(53, 1, 0));
        }

        /// <summary>
        ///     门把手灯
        /// </summary>
        /// <param name="value"></param>
        public void ActvnOfHndlDoorLi1HndlDoorLiDrvr(byte value)
        {
            _cemBodyFr124_0X194.UpdateData(new MatrixValDefinition(6, 2, value));
            _cemBodyFr124_0X194.UpdateData(new MatrixValDefinition(2, 2, value));
        }

        /// <summary>
        ///     后视镜折叠展开
        /// </summary>
        /// <param name="value"></param>
        public void DrvrMirrCmd(byte value)
        {
            _cemBodyFr179_0X346.UpdateData(new MatrixValDefinition(52, 2, value));
            _cemBodyFr179_0X346.UpdateData(new MatrixValDefinition(60, 2, value));
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        public void DrvrAdjCmd(byte value)
        {
            _cemBodyFr179_0X346.UpdateData(new MatrixValDefinition(48, 3, value));
            _cemBodyFr179_0X346.UpdateData(new MatrixValDefinition(40, 3, value));
        }

        public void ShortDropWin(byte value)
        {
            _cemBodyFr103_0X5B.UpdateData(new MatrixValDefinition(6, 2, value));
        }

        /// <summary>
        ///     儿童锁
        /// </summary>
        public void ChdLockReLeCtrlHmiReq(byte value)
        {
            _cemBodyFr09_0X41.UpdateData(new MatrixValDefinition(62, 2, value));
            _rbChdLockReLeCtrlHmiReq = value;
        }

        public void Door(byte value)
        {
            _cemBodyFr78_0X00B4.UpdateData(new MatrixValDefinition(0, 2, value));
        }

        public void Pendal(byte value)
        {
            _rbPedal = value;
        }

        public void DoorDrvrHndlCmd(byte value)
        {
            _cemBodyFr12_0X00F0.UpdateData(new MatrixValDefinition(42, 2, value));
        }

        #endregion
    }
}
