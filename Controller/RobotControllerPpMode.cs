using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CommonUtility;
using CommonUtility.FileOperator;

namespace Controller
{
    public sealed class RobotControllerPpMode : ControllerBase
    {
        public readonly Dictionary<int, Dictionary<string, string>> SingleAxisState =
            new Dictionary<int, Dictionary<string, string>>();
        public static object LockState = new object();
        public readonly string IsIdle = "IDLE"; // 00
        public readonly string IsHoming = "HOMING"; // 01
        public readonly string IsServoOn = "ServoOn"; // 02
        public readonly string IsStandstill = "StandStill"; // 03
        public readonly string IsMoving = "IsMoving"; // 04
        public readonly string IsServoOff = "ServoOff"; // 05
        public readonly string IsError = "IsError"; // 06
        public readonly string CurrentPos = "CurrentPos";
        public readonly string IsMoveDone = "IsMoveDone";
        public readonly string IsMoveExecute = "IsMoveExecute";
        public readonly string MoveTargetPos = "MoveTargetPos";
        public readonly string MoveTargetSpeed = "MoveTargetSpeed";
        public readonly string MoveTargetOffset = "MoveTargetOffset";
        public readonly string MoveStartPos = "MoveStartPos";

        [Description("R,RobotSate-AllAxisesIdle")]
        public bool AllAxisesIdle;
        [Description("R,RobotSate-AllAxisesServoOn")]
        public bool AllAxisesServoOn;
        [Description("R,RobotSate-AllAxisesReady")]
        public bool AllAxisesReady;
        [Description("R,RobotSate-AllAxisesRunning")]
        public bool AllAxisesRunning;
        [Description("R,RobotSate-AxisError")]
        public bool AxisError;
        [Description("R,RobotSate-EmergencyStop")]
        public bool EmergencyStop;
        [Description("R,RobotSate-ErrorAxisId")]
        public string ErrorAxisId = string.Empty;

        public RobotControllerPpMode(string name) :
            base(name)
        {
            RobotJogging = XmlHelper.Deserialize<RobotJogging>(ConfilePath);
            //RobotRunProgram("后盖上料");

            for (var i = 0; i < 16; i++)
            {
                SingleAxisState.Add(i, new Dictionary<string, string>());
                SingleAxisState[i].Add(IsIdle, false.ToString());
                SingleAxisState[i].Add(IsHoming, false.ToString());
                SingleAxisState[i].Add(IsServoOn, false.ToString());
                SingleAxisState[i].Add(IsStandstill, false.ToString());
                SingleAxisState[i].Add(IsMoving, false.ToString());
                SingleAxisState[i].Add(IsServoOff, false.ToString());
                SingleAxisState[i].Add(IsError, false.ToString());
                SingleAxisState[i].Add(
                    CurrentPos, ((float)0.0).ToString(CultureInfo.InvariantCulture));
                SingleAxisState[i].Add(IsMoveDone, false.ToString());
                SingleAxisState[i].Add(IsMoveExecute, false.ToString());
                SingleAxisState[i].Add(
                    MoveTargetPos, ((float)0.0).ToString(CultureInfo.InvariantCulture));
                SingleAxisState[i].Add(
                    MoveTargetSpeed, ((float)0.0).ToString(CultureInfo.InvariantCulture));
                SingleAxisState[i].Add(
                    MoveTargetOffset, ((float)0.0).ToString(CultureInfo.InvariantCulture));
                SingleAxisState[i].Add(
                    MoveStartPos, ((float)0.0).ToString(CultureInfo.InvariantCulture));
            }

            for (var i = 0; i < 9; i++)
            {
                PalletDictionary.Add(
                    i, new Pallet { PalletIndex = i, BlockIndex = -1 });

                if (RobotJogging.RobotPallets == null)
                    continue;
                var pallets = RobotJogging.RobotPallets.ToList();
                var pallet = pallets.Find(f => f.PattetIndex != null && f.PattetIndex == i.ToString());
                if (pallet == null)
                    continue;
                PalletDictionary[i].Name = pallet.PattetName;
                if (pallet.Blocks == null)
                    continue;
                foreach (var block in pallet.Blocks)
                    PalletDictionary[i].Blocks.Add(block);
            }
        }

        ~RobotControllerPpMode()
        {
            Dispose();
        }

        #region 通用输入输出

        [Description("R/W,通用输入SI0")]
        public bool Si0;
        [Description("R/W,通用输入SI1")]
        public bool Si1;
        [Description("R/W,通用输入SI2")]
        public bool Si2;
        [Description("R/W,通用输入SI3")]
        public bool Si3;
        [Description("R/W,通用输入SI4")]
        public bool Si4;
        [Description("R/W,通用输入SI5")]
        public bool Si5;
        [Description("R/W,通用输入SI6")]
        public bool Si6;
        [Description("R/W,通用输入SI7")]
        public bool Si7;
        [Description("R/W,通用输入SI8")]
        public bool Si8;
        [Description("R/W,通用输入SI9")]
        public bool Si9;
        [Description("R/W,通用输入SI10")]
        public bool Si10;
        [Description("R/W,通用输入SI11")]
        public bool Si11;
        [Description("R/W,通用输入SI12")]
        public bool Si12;
        [Description("R/W,通用输入SI13")]
        public bool Si13;
        [Description("R/W,通用输入SI14")]
        public bool Si14;
        [Description("R/W,通用输入SI15")]
        public bool Si15;
        [Description("R/W,通用输入SI16")]
        public bool Si16;
        [Description("R/W,通用输入SI17")]
        public bool Si17;
        [Description("R/W,通用输入SI18")]
        public bool Si18;
        [Description("R/W,通用输入SI19")]
        public bool Si19;
        [Description("R/W,通用输入SI20")]
        public bool Si20;
        [Description("R/W,通用输入SI21")]
        public bool Si21;
        [Description("R/W,通用输入SI22")]
        public bool Si22;
        [Description("R/W,通用输入SI23")]
        public bool Si23;
        [Description("R/W,通用输入SI24")]
        public bool Si24;
        [Description("R/W,通用输入SI25")]
        public bool Si25;
        [Description("R/W,通用输入SI26")]
        public bool Si26;
        [Description("R/W,通用输入SI27")]
        public bool Si27;
        [Description("R/W,通用输入SI28")]
        public bool Si28;
        [Description("R/W,通用输入SI29")]
        public bool Si29;
        [Description("R/W,通用输入SI30")]
        public bool Si30;
        [Description("R/W,通用输入SI31")]
        public bool Si31;
        [Description("R/W,通用输入SI32")]
        public bool Si32;
        [Description("R/W,通用输入SI33")]
        public bool Si33;
        [Description("R/W,通用输入SI34")]
        public bool Si34;
        [Description("R/W,通用输入SI35")]
        public bool Si35;
        [Description("R/W,通用输入SI36")]
        public bool Si36;
        [Description("R/W,通用输入SI37")]
        public bool Si37;
        [Description("R/W,通用输入SI38")]
        public bool Si38;
        [Description("R/W,通用输入SI39")]
        public bool Si39;
        [Description("R/W,通用输入SI40")]
        public bool Si40;

        [Description("R/W,通用输出SO0")]
        public bool So0;
        [Description("R/W,通用输出SO1")]
        public bool So1;
        [Description("R/W,通用输出SO2")]
        public bool So2;
        [Description("R/W,通用输出SO3")]
        public bool So3;
        [Description("R/W,通用输出SO4")]
        public bool So4;
        [Description("R/W,通用输出SO5")]
        public bool So5;
        [Description("R/W,通用输出SO6")]
        public bool So6;
        [Description("R/W,通用输出SO7")]
        public bool So7;
        [Description("R/W,通用输出SO8")]
        public bool So8;
        [Description("R/W,通用输出SO9")]
        public bool So9;
        [Description("R/W,通用输出SO10")]
        public bool So10;
        [Description("R/W,通用输出SO11")]
        public bool So11;
        [Description("R/W,通用输出SO12")]
        public bool So12;
        [Description("R/W,通用输出SO13")]
        public bool So13;
        [Description("R/W,通用输出SO14")]
        public bool So14;
        [Description("R/W,通用输出SO15")]
        public bool So15;
        [Description("R/W,通用输出SO16")]
        public bool So16;
        [Description("R/W,通用输出SO17")]
        public bool So17;
        [Description("R/W,通用输出SO18")]
        public bool So18;
        [Description("R/W,通用输出SO19")]
        public bool So19;
        [Description("R/W,通用输出SO20")]
        public bool So20;
        [Description("R/W,通用输出SO21")]
        public bool So21;
        [Description("R/W,通用输出SO22")]
        public bool So22;
        [Description("R/W,通用输出SO23")]
        public bool So23;
        [Description("R/W,通用输出SO24")]
        public bool So24;
        [Description("R/W,通用输出SO25")]
        public bool So25;
        [Description("R/W,通用输出SO26")]
        public bool So26;
        [Description("R/W,通用输出SO27")]
        public bool So27;
        [Description("R/W,通用输出SO28")]
        public bool So28;
        [Description("R/W,通用输出SO29")]
        public bool So29;
        [Description("R/W,通用输出SO30")]
        public bool So30;
        [Description("R/W,通用输出SO31")]
        public bool So31;
        [Description("R/W,通用输出SO32")]
        public bool So32;
        [Description("R/W,通用输出SO33")]
        public bool So33;
        [Description("R/W,通用输出SO34")]
        public bool So34;
        [Description("R/W,通用输出SO35")]
        public bool So35;
        [Description("R/W,通用输出SO36")]
        public bool So36;
        [Description("R/W,通用输出SO37")]
        public bool So37;
        [Description("R/W,通用输出SO38")]
        public bool So38;
        [Description("R/W,通用输出SO39")]
        public bool So39;
        [Description("R/W,通用输出SO40")]
        public bool So40;

        #endregion

        #region DI&DO

        [Description("R,输入_DI0")]
        public string Di0;
        [Description("R,输入_DI1")]
        public string Di1;
        [Description("R,输入_DI2")]
        public string Di2;
        [Description("R,输入_DI3")]
        public string Di3;
        [Description("R,输入_DI4")]
        public string Di4;
        [Description("R,输入_DI5")]
        public string Di5;
        [Description("R,输入_DI6")]
        public string Di6;
        [Description("R,输入_DI7")]
        public string Di7;
        [Description("R,输入_DI8")]
        public string Di8;
        [Description("R,输入_DI9")]
        public string Di9;
        [Description("R,输入_DI10")]
        public string Di10;
        [Description("R,输入_DI11")]
        public string Di11;

        [Description("R/W,继电器_DO0")]
        public string Do0 = 0.ToString();
        [Description("R/W,继电器_DO1")]
        public string Do1 = 0.ToString();
        [Description("R/W,继电器_DO2")]
        public string Do2 = 0.ToString();
        [Description("R/W,继电器_DO3")]
        public string Do3 = 0.ToString();

        [Description("R/W,低边输出_DO4")]
        public string Do4 = 0.ToString();
        [Description("R/W,低边输出_DO5")]
        public string Do5 = 0.ToString();
        [Description("R/W,低边输出_DO6")]
        public string Do6 = 0.ToString();
        [Description("R/W,低边输出_DO7")]
        public string Do7 = 0.ToString();
        #endregion

        private string LocalIpAddress { get; set; }
        private string RemoteIpAddress { get; set; }
        public MyUdpClient RtexUdpClient { get; set; }
        private Thread MainWorkTh { get; set; }
        public readonly byte[] DataBytes = new byte[1024 * 1024];
        public readonly byte[] FlashBytes = new byte[1024 * 1024];
        private readonly EventWaitHandle _dataReadWait = new AutoResetEvent(false);
        private readonly EventWaitHandle _dataWriteWait = new AutoResetEvent(false);
        private readonly EventWaitHandle _flashReadWait = new AutoResetEvent(false);
        private readonly EventWaitHandle _runEventWait = new AutoResetEvent(false);
        private readonly EventWaitHandle _runJogWait = new AutoResetEvent(false);

        public void InitLocalLocalIpAddress(string ipPort)
        {
            LocalIpAddress = ipPort;
        }

        public void InitRemoteIpAddress(string ipPort)
        {
            RemoteIpAddress = ipPort;

            var local = LocalIpAddress.Split(':');
            var remote = RemoteIpAddress.Split(':');

            var localIp = local[0];
            var localPort = int.Parse(local[1]);

            var remoteIp = remote[0];
            var remotePort = int.Parse(remote[1]);

            RtexUdpClient = new MyUdpClient(localIp, localPort);
            RtexUdpClient.AddRemoteClient(remoteIp, remotePort);
            RtexUdpClient.PushMsgEvent += _rtexUdpClient_PushMsgEvent;
            RtexUdpClient.BeginReceive();

            if (MainWorkTh != null)
            {
                MainWorkTh.Abort();
                MainWorkTh.Join();
            }

            MainWorkTh = new Thread(MainWork) { IsBackground = true };
            MainWorkTh.Start();
        }

        private void MainWork()
        {
            while (MainWorkTh.IsAlive)
            {
                if (!MainWorkTh.IsAlive)
                    break;

                if (RtexUdpClient == null)
                    continue;

                Thread.Sleep(10);

                try
                {
                    //lock (RtexUdpClient)
                    {
                        ReadData();
                        Thread.Sleep(5);
                        SetDo();
                        Thread.Sleep(10);

                        Action mySendQueueWork = null;
                        lock (_jogLocker)
                        {
                            if (_sendTasks.Count > 0)
                            {
                                mySendQueueWork = _sendTasks.Dequeue();

                                if (mySendQueueWork == null)
                                    continue;
                            }
                        }

                        if (mySendQueueWork != null)
                            mySendQueueWork.Invoke();
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private void ReadData()
        {
            var readBytes =
                new byte[]
                {
                    0x00, 0x01, 0x00, 0x00, 0x00, 0x06, 0x01, 0x03, 0x10, 0x00, 0x00, 0x90
                };

            SendMsg(readBytes);

            if (_dataReadWait.WaitOne(100))
            {
                #region controller_status
                switch (DataBytes[1])
                {
                    case 0:
                        AllAxisesIdle = true;
                        AllAxisesServoOn = false;
                        AllAxisesReady = false;
                        AllAxisesRunning = false;
                        AxisError = false;
                        break;

                    case 1:
                        AllAxisesIdle = false;
                        AllAxisesServoOn = true;
                        AllAxisesReady = false;
                        AllAxisesRunning = false;
                        AxisError = false;
                        break;

                    case 2:
                        AllAxisesIdle = false;
                        AllAxisesServoOn = false;
                        AllAxisesReady = true;
                        AllAxisesRunning = false;
                        AxisError = false;
                        break;

                    case 3:
                        AllAxisesIdle = false;
                        AllAxisesServoOn = false;
                        AllAxisesReady = false;
                        AllAxisesRunning = true;
                        AxisError = false;
                        break;

                    case 4:
                        AllAxisesIdle = false;
                        AllAxisesServoOn = false;
                        AllAxisesReady = false;
                        AllAxisesRunning = false;
                        AxisError = true;
                        break;
                }
                #endregion

                EmergencyStop = DataBytes[5] == 0x00;
                ErrorAxisId = ValueHelper.GetHextStrWithOx(DataBytes[7]);

                Di0 = DataBytes[8].ToString();
                Di1 = DataBytes[9].ToString();
                Di2 = DataBytes[10].ToString();
                Di3 = DataBytes[11].ToString();
                Di4 = DataBytes[12].ToString();
                Di5 = DataBytes[13].ToString();
                Di6 = DataBytes[14].ToString();
                Di7 = DataBytes[15].ToString();
                Di8 = DataBytes[16].ToString();
                Di9 = DataBytes[17].ToString();
                Di10 = DataBytes[18].ToString();
                Di11 = DataBytes[19].ToString();

                lock (LockState)
                {
                    var baseIndex = 32;
                    for (var i = 0; i < 16; i++)
                    {
                        #region 单轴状态
                        switch (DataBytes[baseIndex + 3])
                        {
                            case 0:
                                SingleAxisState[i][IsIdle] = true.ToString();
                                SingleAxisState[i][IsHoming] = false.ToString();
                                SingleAxisState[i][IsServoOn] = false.ToString();
                                SingleAxisState[i][IsStandstill] = false.ToString();
                                SingleAxisState[i][IsMoving] = false.ToString();
                                SingleAxisState[i][IsServoOff] = false.ToString();
                                SingleAxisState[i][IsError] = false.ToString();
                                break;

                            case 1:
                                SingleAxisState[i][IsIdle] = false.ToString();
                                SingleAxisState[i][IsHoming] = false.ToString();
                                SingleAxisState[i][IsServoOn] = true.ToString();
                                SingleAxisState[i][IsStandstill] = false.ToString();
                                SingleAxisState[i][IsMoving] = false.ToString();
                                SingleAxisState[i][IsServoOff] = false.ToString();
                                SingleAxisState[i][IsError] = false.ToString();
                                break;

                            case 2:
                                SingleAxisState[i][IsIdle] = false.ToString();
                                SingleAxisState[i][IsHoming] = true.ToString();
                                SingleAxisState[i][IsServoOn] = false.ToString();
                                SingleAxisState[i][IsStandstill] = false.ToString();
                                SingleAxisState[i][IsMoving] = false.ToString();
                                SingleAxisState[i][IsServoOff] = false.ToString();
                                SingleAxisState[i][IsError] = false.ToString();
                                break;

                            case 3:
                                SingleAxisState[i][IsIdle] = false.ToString();
                                SingleAxisState[i][IsHoming] = false.ToString();
                                SingleAxisState[i][IsServoOn] = false.ToString();
                                SingleAxisState[i][IsStandstill] = true.ToString();
                                SingleAxisState[i][IsMoving] = false.ToString();
                                SingleAxisState[i][IsServoOff] = false.ToString();
                                SingleAxisState[i][IsError] = false.ToString();
                                break;

                            case 4:
                                SingleAxisState[i][IsIdle] = false.ToString();
                                SingleAxisState[i][IsHoming] = false.ToString();
                                SingleAxisState[i][IsServoOn] = false.ToString();
                                SingleAxisState[i][IsStandstill] = false.ToString();
                                SingleAxisState[i][IsMoving] = true.ToString();
                                SingleAxisState[i][IsServoOff] = false.ToString();
                                SingleAxisState[i][IsError] = false.ToString();
                                break;

                            case 5:
                                SingleAxisState[i][IsIdle] = false.ToString();
                                SingleAxisState[i][IsHoming] = false.ToString();
                                SingleAxisState[i][IsServoOn] = false.ToString();
                                SingleAxisState[i][IsStandstill] = false.ToString();
                                SingleAxisState[i][IsMoving] = false.ToString();
                                SingleAxisState[i][IsServoOff] = false.ToString();
                                SingleAxisState[i][IsError] = true.ToString();
                                break;

                            case 6:
                                SingleAxisState[i][IsIdle] = false.ToString();
                                SingleAxisState[i][IsHoming] = false.ToString();
                                SingleAxisState[i][IsServoOn] = false.ToString();
                                SingleAxisState[i][IsStandstill] = false.ToString();
                                SingleAxisState[i][IsMoving] = false.ToString();
                                SingleAxisState[i][IsServoOff] = false.ToString();
                                SingleAxisState[i][IsError] = false.ToString();
                                break;
                        }
                        #endregion

                        var fb1 = DataBytes[baseIndex + 8];
                        var fb2 = DataBytes[baseIndex + 9];
                        var fb3 = DataBytes[baseIndex + 10];
                        var fb4 = DataBytes[baseIndex + 11];
                        //SingleAxisState[i][CurrentPos] =
                        //    BitConverter.ToSingle(new[] { fb4, fb3, fb2, fb1 }, 0)
                        //        .ToString(CultureInfo.InvariantCulture);
                        SingleAxisState[i][CurrentPos] =
                            Math.Round(BitConverter.ToSingle(new[] { fb4, fb3, fb2, fb1 }, 0), 2,
                                MidpointRounding.AwayFromZero).ToString(CultureInfo.InvariantCulture);

                        baseIndex = baseIndex + 16;
                    }

                    foreach (var key in SingleAxisState.Keys)
                    {
                        if (SingleAxisState[key][IsMoveExecute] != true.ToString())
                            continue;

                        var targetPos = float.Parse(SingleAxisState[key][MoveTargetPos]);
                        var currentPos = float.Parse(SingleAxisState[key][CurrentPos]);
                        var startPos = float.Parse(SingleAxisState[key][MoveStartPos]);

                        //if (SingleAxisState[key][IsStandstill] != true.ToString() ||
                        //    SingleAxisState[key][IsHoming] == true.ToString())
                        if (SingleAxisState[key][IsHoming] == true.ToString() ||
                            !EmergencyStop ||
                            SingleAxisState[key][IsError] == true.ToString())
                        {
                            continue;
                        }

                        var offset = float.Parse(SingleAxisState[key][MoveTargetOffset]);
                        if (offset >= -0.01 && offset <= 0.01)
                        {
                            if (!(Math.Abs(targetPos - currentPos) < 0.5))
                                continue;

                            SingleAxisState[key][IsMoveExecute] = false.ToString();
                            SingleAxisState[key][IsMoveDone] = true.ToString();
                        }
                        else
                        {
                            if (targetPos > startPos)
                            {
                                if (currentPos + Math.Abs(offset) >= targetPos)
                                {
                                    SingleAxisState[key][IsMoveExecute] = false.ToString();
                                    SingleAxisState[key][IsMoveDone] = true.ToString();
                                }
                            }
                            else if (targetPos < startPos)
                            {
                                if (currentPos - Math.Abs(offset) <= targetPos)
                                {
                                    SingleAxisState[key][IsMoveExecute] = false.ToString();
                                    SingleAxisState[key][IsMoveDone] = true.ToString();
                                }
                            }
                        }
                    }
                }
            }
        }

        public bool ReadFlash()
        {
            var readBytes = new byte[]
            {
                0x00, 0x01,
                0x00, 0x00,
                0x00, 0x0B,
                0x01,
                0x6F,
                0x01,
                0x10, 0x00,
                0x01, 0xE0
            };

            //return false;

            SendMsg(readBytes);
            return _flashReadWait.WaitOne(150);
        }

        public void WriteFlash(byte[] sendValues)
        {
            var sendBs = new List<byte>();
            sendBs.AddRange(new byte[] { 0x00, 0x01, 0x00, 0x00 });

            var len = (ushort)(sendValues.Length + 7);
            sendBs.AddRange(BitConverter.GetBytes(len).Reverse());

            sendBs.Add(0x01);
            sendBs.Add(0x6E);

            sendBs.Add(0x00);
            sendBs.Add(0x00);

            var regCount = (ushort)(sendValues.Length / 2);
            sendBs.AddRange(BitConverter.GetBytes(regCount).Reverse());

            var byteCount = (ushort)sendValues.Length;
            sendBs.Add(BitConverter.GetBytes(byteCount).Reverse().ToArray()[1]);

            sendBs.AddRange(sendValues);

            lock (_jogLocker)
                _sendTasks.Enqueue(() =>
                {
                    SendMsg(sendBs);
                    _runJogWait.WaitOne(100);
                });
        }

        private void SetDo()
        {
            var bytes = new byte[8];
            bytes[0] = Do0 == 1.ToString() ? (byte)0x01 : (byte)0x00;
            bytes[1] = Do1 == 1.ToString() ? (byte)0x01 : (byte)0x00;
            bytes[2] = Do2 == 1.ToString() ? (byte)0x01 : (byte)0x00;
            bytes[3] = Do3 == 1.ToString() ? (byte)0x01 : (byte)0x00;
            bytes[4] = Do4 == 1.ToString() ? (byte)0x01 : (byte)0x00;
            bytes[5] = Do5 == 1.ToString() ? (byte)0x01 : (byte)0x00;
            bytes[6] = Do6 == 1.ToString() ? (byte)0x01 : (byte)0x00;
            bytes[7] = Do7 == 1.ToString() ? (byte)0x01 : (byte)0x00;

            // func=0x10
            var setDoBytes = new List<byte>();
            setDoBytes.AddRange(
                new byte[] { 0x00, 0x01, 0x00, 0x00, 0x00, 0x0F, 0x01, 0x10, 0x10, 0x0A, 0x00, 0x04, 0x08 });
            setDoBytes.AddRange(bytes);
            SendMsg(setDoBytes);
            _dataWriteWait.WaitOne(25);
        }

        private void SendMsg(IEnumerable<byte> bytes)
        {
            var sendBytes = new List<byte>();
            sendBytes.AddRange(bytes);

            RtexUdpClient.SendMsgTo(
                new IPEndPoint(IPAddress.Parse(RemoteIpAddress.Split(':')[0]), int.Parse(RemoteIpAddress.Split(':')[1])),
                sendBytes.ToArray(), 5);
        }

        private void _rtexUdpClient_PushMsgEvent(
            EndPoint ipEndPoint, byte[] bytes)
        {
            if (bytes.Length < 7)
                return;

            var buffLst = bytes.ToList();

            do
            {
                #region 拆包
                if (buffLst.Count < 7)
                {
                    buffLst.Clear();
                    continue;
                }

                var len = buffLst[4] * 256 + buffLst[5];
                if (buffLst.Count < len + 6)
                {
                    buffLst.Clear();
                    continue;
                }

                var tempLen = len + 6;
                var temp = new byte[tempLen];
                Array.Copy(buffLst.ToArray(), temp, tempLen);
                buffLst.RemoveRange(0, tempLen);

                var tempContent = new byte[tempLen];
                Array.Copy(temp, tempContent, tempLen);
                #endregion

                #region CRC16-MODBUS校验
                var totoalLen = temp[4] * 256 + temp[5];
                if (temp.Length - 6 != totoalLen)
                    continue; // 数组长度不正确
                #endregion

                lock (RtexUdpClient)
                {
                    #region 解析数据并推送

                    var registerCount = (totoalLen - 3) / 2;

                    var function = temp[7];
                    var contentBytes = new byte[registerCount * 2];
                    Array.Copy(bytes.ToArray(), 9, contentBytes, 0, registerCount * 2);

                    switch (function)
                    {
                        case 0x03:
                            Array.Copy(contentBytes, 0, DataBytes, 0, contentBytes.Length);
                            _dataReadWait.Set();
                            break;

                        case 0x6E:
                            //_flashWriteWait.Set();
                            break;

                        case 0x6F:
                            Array.Copy(contentBytes, 0, FlashBytes, 0, contentBytes.Length);
                            _flashReadWait.Set();
                            break;

                        case 0x6D:
                            _runEventWait.Set();
                            break;

                        case 0x6C:
                            _runJogWait.Set();
                            break;

                        case 0x6B:
                            _runJogWait.Set();
                            break;

                        case 0x10:
                            _dataWriteWait.Set();
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    #endregion
                }
            } while (buffLst.Count != 0);
        }

        private readonly object _jogLocker = new object();
        private readonly Queue<Action> _sendTasks = new Queue<Action>();

        public readonly string Speed = "Speed";
        public readonly string Position = "Position";
        public readonly string Offset = "Offset";

        /// <summary>
        /// 多轴run multi axises move
        /// 功能码0x6B
        /// </summary>
        /// <param name="paras"></param>
        public void RobotRun(Dictionary<int, Dictionary<string, string>> paras)
        {
            var d = new Dictionary<int, Dictionary<string, string>>();
            //d = paras;

            foreach (var t in paras.Keys)
            {
                while (true)
                {
                    lock (LockState)
                    {
                        if (SingleAxisState[t][IsStandstill] == true.ToString())
                        {
                            break;
                        }

                        if (IsBreak || !EmergencyStop || SingleAxisState[t][IsError] == true.ToString())
                        {
                            return;
                        }
                    }
                }

                lock (LockState)
                {
                    if (Math.Abs(float.Parse(paras[t][Position]) - float.Parse(SingleAxisState[t][CurrentPos])) < 1 &&
                    SingleAxisState[t][IsMoving] != true.ToString() &&
                    SingleAxisState[t][IsStandstill] == true.ToString() &&
                    SingleAxisState[t][IsHoming] != true.ToString())
                    {
                        SingleAxisState[t][IsMoveDone] = true.ToString();
                    }
                    else
                    {
                        d.Add(t, new Dictionary<string, string>());
                        d[t].Add(Speed, paras[t][Speed]);
                        d[t].Add(Position, paras[t][Position]);
                        if (paras[t].ContainsKey(Offset))
                        {
                            d[t].Add(Offset, paras[t][Offset]);
                        }
                        else
                        {
                            d[t].Add(Offset, 0.ToString());
                        }

                        //RtexController.RunJog(AxisIndex, float.Parse(pos), moveSpeed);
                        //IsMoveEventSet = true;

                        SingleAxisState[t][IsMoveDone] = false.ToString();
                        SingleAxisState[t][MoveTargetPos] = paras[t][Position];
                        SingleAxisState[t][IsMoveExecute] = true.ToString();
                        SingleAxisState[t][MoveTargetSpeed] = paras[t][Speed];
                        if (paras[t].ContainsKey(Offset))
                        {
                            SingleAxisState[t][MoveTargetOffset] = paras[t][Offset];
                        }
                        else
                        {
                            SingleAxisState[t][MoveTargetOffset] = 0.ToString();
                        }

                        SingleAxisState[t][MoveStartPos] = SingleAxisState[t][CurrentPos];
                    }
                }
            }

            if (!d.Any())
                return;
            {
                if (d.Count > 1)
                {
                    var listTime = new Dictionary<int, float>();

                    var baseTime = -1.0;

                    lock (LockState)
                    {
                        foreach (var t in d.Keys)
                        {
                            var speed = float.Parse(d[t][Speed]);
                            var targetPos = float.Parse(d[t][Position]);

                            var fromPos = float.Parse(SingleAxisState[t][CurrentPos]);

                            var time = Math.Abs(targetPos - fromPos) / speed;

                            if (baseTime == -1.0)
                                baseTime = time;
                            else
                            {
                                if (baseTime < time)
                                    baseTime = time;
                            }

                            listTime.Add(t, time);
                        }
                    }

                    foreach (var t in listTime.Keys)
                    {
                        var orginT = listTime[t];
                        var k = baseTime / orginT;
                        var orginV = float.Parse(d[t][Speed]);
                        var newV = orginV / k;
                        if (newV < 1)
                            newV = 1;
                        d[t][Speed] = newV.ToString(CultureInfo.InvariantCulture);
                    }
                }

                var content = new List<byte> { 0x00, (byte)d.Count };

                foreach (var t in d.Keys)
                {
                    var speedBs = BitConverter.GetBytes(float.Parse(d[t][Speed]));
                    Array.Reverse(speedBs);

                    var posBs = BitConverter.GetBytes(float.Parse(d[t][Position]));
                    Array.Reverse(posBs);

                    content.Add(0x00);
                    content.Add(byte.Parse(t.ToString())); // 轴号

                    content.Add(0x00);
                    content.Add(0x0A); // 运动类型

                    content.AddRange(posBs); // 目标位置
                    content.AddRange(speedBs); // 运行速度
                }

                var sendBs = new List<byte>();
                sendBs.AddRange(new byte[] { 0x00, 0x01, 0x00, 0x00 });

                var len = (ushort)(content.Count + 7);
                sendBs.AddRange(BitConverter.GetBytes(len).Reverse());

                sendBs.Add(0x01);
                sendBs.Add(0x6B);

                sendBs.Add(0x00);
                sendBs.Add(0x20);

                var regCount = (ushort)(content.Count / 2);
                sendBs.AddRange(BitConverter.GetBytes(regCount).Reverse());

                var byteCount = (ushort)content.Count;
                sendBs.Add(BitConverter.GetBytes(byteCount).Reverse().ToArray()[1]);

                sendBs.AddRange(content);

                lock (_jogLocker)
                {
                    SendMsg(sendBs);
                    if (!_runJogWait.WaitOne(150))
                    {
                        SendMsg(sendBs);
                        Thread.Sleep(20);
                    }
                    //_sendTasks.Enqueue(() =>
                    //{
                    //    SendMsg(sendBs);
                    //    _runJogWait.WaitOne(25);
                    //});
                }
            }
        }

        #region Event

        public void RunEventNull(string index)
        {
            RunEvent(index, 0x00);
        }

        public void RunEventServoOn(string index)
        {
            RunEvent(index, 0x01);
        }

        public void RunEventServoOff(string index)
        {
            RunEvent(index, 0x02);
        }

        public void RunEventEmergencyStop(string index)
        {
            RunEvent(index, 0x03);
        }

        public void RunEventReset(string index)
        {
            RunEvent(index, 0x04);
        }

        public void RunEventHome(string index)
        {
            RunEvent(index, 0x05);
        }

        public void RunEventSetHomeOffset(string index)
        {
            // 00 01 00 00 00 0F 01 68 00 00 00 01 02 00 03
            //RunEvent(index, 0x06);
            SendMsg(new byte[]
            {
                0x00, 0x01,
                0x00, 0x00,
                0x00,0x0F,
                0x01,
                0x68,
                0x00,0x00,
                0x00,0x01,
                0x02,
                0x00,byte.Parse(index)
            });
            Thread.Sleep(100);
        }

        public void RunEventStop(string index)
        {
            RunEvent(index, 0x07);
        }

        public void RunEventHalt(string index)
        {
            RunEvent(index, 0x08);
        }

        public void RunEventContinue(string index)
        {
            RunEvent(index, 0x09);
        }

        private void RunEvent(string index, byte eventVaue)
        {
            var sendBs = new byte[]
            {
                0x00, 0x01, 0x00, 0x00,
                0x00, 0x15,
                0x01,
                0x6B,
                0x00, 0x20,
                0x00, 0x07,
                0x0E,
                0x00, 0x01,
                0x00, byte.Parse(index), 0x00, eventVaue, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            };

            lock (_jogLocker)
                _sendTasks.Enqueue(() =>
                {
                    SendMsg(sendBs);
                    _runEventWait.WaitOne(100);
                });
        }

        public void RunEventNullAll()
        {
            RunEventAll(0x00);
        }

        public void RunEventServoOnAll()
        {
            RunEventAll(0x01);
        }

        public void RunEventServoOffAll()
        {
            RunEventAll(0x02);
        }

        public void RunEventEmergencyStopAll()
        {
            RunEventAll(0x03);
        }

        public void RunEventResetAll()
        {
            RunEventAll(0x04);
        }

        public void RunEventHomeAll()
        {
            RunEventAll(0x05);
        }

        public void RunEventStopAll()
        {
            RunEventAll(0x07);
        }

        public void RunEventHaltAll()
        {
            RunEventAll(0x08);
        }

        public void RunEventContinueAll()
        {
            RunEventAll(0x09);
        }

        private void RunEventAll(byte eventVaue)
        {
            var d = new Dictionary<int, Dictionary<string, string>>();

            for (var i = 0; i < 16; i++)
            {
                d.Add(i, new Dictionary<string, string>());

                d[i].Add(Speed, 0.0.ToString(CultureInfo.InvariantCulture));
                d[i].Add(Position, 0.0.ToString(CultureInfo.InvariantCulture));
            }

            var content = new List<byte> { 0x00, (byte)d.Count };

            foreach (var t in d.Keys)
            {
                var speedBs = BitConverter.GetBytes(float.Parse(d[t][Speed]));
                Array.Reverse(speedBs);

                var posBs = BitConverter.GetBytes(float.Parse(d[t][Position]));
                Array.Reverse(posBs);

                content.Add(0x00);
                content.Add(byte.Parse(t.ToString())); // 轴号

                content.Add(0x00);
                content.Add(eventVaue); // 运动类型

                content.AddRange(posBs); // 目标位置
                content.AddRange(speedBs); // 运行速度
            }

            var sendBs = new List<byte>();
            sendBs.AddRange(new byte[] { 0x00, 0x01, 0x00, 0x00 });

            var len = (ushort)(content.Count + 7);
            sendBs.AddRange(BitConverter.GetBytes(len).Reverse());

            sendBs.Add(0x01);
            sendBs.Add(0x6B);

            sendBs.Add(0x00);
            sendBs.Add(0x20);

            var regCount = (ushort)(content.Count / 2);
            sendBs.AddRange(BitConverter.GetBytes(regCount).Reverse());

            var byteCount = (ushort)content.Count;
            sendBs.Add(BitConverter.GetBytes(byteCount).Reverse().ToArray()[1]);

            sendBs.AddRange(content);

            lock (_jogLocker)
                _sendTasks.Enqueue(() =>
                {
                    SendMsg(sendBs);
                    _runJogWait.WaitOne(100);
                });
        }

        public void RunEventNullMultiple(List<int> axitList)
        {
            RunEventMultiple(0x00, axitList);
        }

        public void RunEventServoOnMultiple(List<int> axitList)
        {
            RunEventMultiple(0x01, axitList);
        }

        public void RunEventServoOffMultiple(List<int> axitList)
        {
            RunEventMultiple(0x02, axitList);
        }

        public void RunEventEmergencyStopMultiple(List<int> axitList)
        {
            RunEventMultiple(0x03, axitList);
        }

        public void RunEventResetMultiple(List<int> axitList)
        {
            RunEventMultiple(0x04, axitList);
        }

        public void RunEventHomeMultiple(List<int> axitList)
        {
            RunEventMultiple(0x05, axitList);
        }

        public void RunEventStopMultiple(List<int> axitList)
        {
            RunEventMultiple(0x07, axitList);
        }

        public void RunEventHaltMultiple(List<int> axitList)
        {
            RunEventMultiple(0x08, axitList);
        }

        public void RunEventContinueMultiple(List<int> axitList)
        {
            RunEventMultiple(0x09, axitList);
        }

        private void RunEventMultiple(byte eventVaue, IReadOnlyList<int> axitList)
        {
            var d = new Dictionary<int, Dictionary<string, string>>();

            foreach (var t in axitList)
            {
                d.Add(t, new Dictionary<string, string>());

                d[t].Add(Speed, 0.0.ToString(CultureInfo.InvariantCulture));
                d[t].Add(Position, 0.0.ToString(CultureInfo.InvariantCulture));
            }

            var content = new List<byte> { 0x00, (byte)d.Count };

            foreach (var t in d.Keys)
            {
                var speedBs = BitConverter.GetBytes(float.Parse(d[t][Speed]));
                Array.Reverse(speedBs);

                var posBs = BitConverter.GetBytes(float.Parse(d[t][Position]));
                Array.Reverse(posBs);

                content.Add(0x00);
                content.Add(byte.Parse(t.ToString())); // 轴号

                content.Add(0x00);
                content.Add(eventVaue); // 运动类型

                content.AddRange(posBs); // 目标位置
                content.AddRange(speedBs); // 运行速度
            }

            var sendBs = new List<byte>();
            sendBs.AddRange(new byte[] { 0x00, 0x01, 0x00, 0x00 });

            var len = (ushort)(content.Count + 7);
            sendBs.AddRange(BitConverter.GetBytes(len).Reverse());

            sendBs.Add(0x01);
            sendBs.Add(0x6B);

            sendBs.Add(0x00);
            sendBs.Add(0x20);

            var regCount = (ushort)(content.Count / 2);
            sendBs.AddRange(BitConverter.GetBytes(regCount).Reverse());

            var byteCount = (ushort)content.Count;
            sendBs.Add(BitConverter.GetBytes(byteCount).Reverse().ToArray()[1]);

            sendBs.AddRange(content);

            lock (_jogLocker)
                _sendTasks.Enqueue(() =>
                {
                    SendMsg(sendBs);
                    _runJogWait.WaitOne(100);
                });
        }

        #endregion

        #region Jog Logic

        public RobotJogging RobotJogging { get; set; }
        public bool IsBreak { get; set; }
        public static string ConfilePath =
            Directory.GetCurrentDirectory() + @"\ControllerConfig\RobotJogging.xml";

        public Dictionary<string, int> CurrentRunProgeam = new Dictionary<string, int>();

        public async void StartProgram(string name)
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    RobotRunProgram(name, 0);
                    while (true)
                    {
                        var i =
                            CurrentRunProgeam[name];
                        if (i == -1)
                            break;
                        Thread.Sleep(5);
                    }
                    Thread.Sleep(5);
                }
            });
        }

        internal class RobotProgram
        {

        }

        public void RobotRunProgram(string name, int blockIndex, int endIndex = -1)
        {
            if (RobotJogging != null && RobotJogging.RobotPrograms != null && RobotJogging.RobotPrograms.Any())
            {
                var findPm = RobotJogging.RobotPrograms.ToList().Find(f => f.Name == name);
                if (findPm != null && findPm.Blocks != null && findPm.Blocks.Any())
                {
                    if (blockIndex > findPm.Blocks.Length - 1)
                    {
                        if (!CurrentRunProgeam.ContainsKey(name))
                            CurrentRunProgeam.Add(name, -1);
                        else
                            CurrentRunProgeam[name] = -1;
                        return;
                    }

                    if (!CurrentRunProgeam.ContainsKey(name))
                        CurrentRunProgeam.Add(name, blockIndex);
                    else
                        CurrentRunProgeam[name] = blockIndex;

                    var block = findPm.Blocks[blockIndex];
                    IsBreak = false;

                    if (block.StartsWith("Move:"))
                    {
                        var point = block.Replace("Move:", string.Empty)
                            .Split(new[] { "[", "]" }, StringSplitOptions.RemoveEmptyEntries);

                        var d = new Dictionary<int, Dictionary<string, string>>();

                        foreach (var p in point)
                        {
                            if (!p.StartsWith("Axis="))
                                continue;

                            var index = 0;
                            var pos = 0.0;
                            var speed = 0.0;
                            var offset = 0.0;

                            foreach (var a in p.Split(';'))
                            {
                                if (a.StartsWith("Axis="))
                                    index = int.Parse(a.Replace("Axis=", string.Empty));
                                else if (a.StartsWith("Pos="))
                                    pos = float.Parse(a.Replace("Pos=", string.Empty));
                                else if (a.StartsWith("Speed="))
                                    speed = float.Parse(a.Replace("Speed=", string.Empty));
                                else if (a.StartsWith("Offset="))
                                    offset = float.Parse(a.Replace("Offset=", string.Empty));
                            }

                            d.Add(index, new Dictionary<string, string>());
                            d[index].Add(Position, pos.ToString(CultureInfo.InvariantCulture));
                            d[index].Add(Speed, speed.ToString(CultureInfo.InvariantCulture));
                            d[index].Add(Offset, offset.ToString(CultureInfo.InvariantCulture));
                        }

                        var programRunAction = new ProgramRunAction
                        {
                            Index = blockIndex,
                            ProgramName = name,
                            EndIndex = endIndex
                        };

                        programRunAction.Action = () =>
                        {
                            RobotRun(d);
                            var doneList = d.Keys.ToDictionary(t => t, t => false.ToString());

                            while (true)
                            {
                                var doneCount = doneList.Values.Count(t => t == true.ToString());

                                if (doneCount == d.Count)
                                {
                                    break;
                                }

                                lock (LockState)
                                {
                                    var isBreak =
                                    d.Keys.Any(
                                        t =>
                                            SingleAxisState[t][IsStandstill] == false.ToString() &&
                                            SingleAxisState[t][IsMoving] == false.ToString());

                                    if (isBreak)
                                    {
                                        programRunAction.Index = findPm.Blocks.Length - 1;
                                        break;
                                    }

                                    foreach (var t in d.Keys.Where(t => SingleAxisState[t][IsMoveDone] == true.ToString()))
                                        doneList[t] = true.ToString();
                                }

                                Thread.Sleep(5);
                            }
                        };

                        programRunAction.Action.BeginInvoke(StatusRunEnd, programRunAction);
                    }
                    else if (block.StartsWith("Pallet:"))
                    {
                        var pallet = block.Replace("Pallet:", string.Empty)
                            .Split(new[] { "[", "]" }, StringSplitOptions.RemoveEmptyEntries);

                        var palletIndex = -1;
                        foreach (var s in from t in pallet
                                          where !t.StartsWith("//")
                                          select t.Split(';')
                                              into sp
                                              from s in sp.Where(s => s.StartsWith("PalletIndex="))
                                              select s)
                            palletIndex = int.Parse(s.Replace("PalletIndex=", string.Empty));

                        if (palletIndex != -1 && PalletDictionary.ContainsKey(palletIndex))
                        {
                            PalletDictionary[palletIndex].BlockIndex++;
                            if (PalletDictionary[palletIndex].BlockIndex == PalletDictionary[palletIndex].Blocks.Count)
                                PalletDictionary[palletIndex].BlockIndex = 0;
                            var palletBlock =
                                PalletDictionary[palletIndex].Blocks[PalletDictionary[palletIndex].BlockIndex];

                            var point = palletBlock.Replace("Move:", string.Empty)
                                .Split(new[] { "[", "]" }, StringSplitOptions.RemoveEmptyEntries);

                            var d = new Dictionary<int, Dictionary<string, string>>();

                            foreach (var p in point)
                            {
                                if (!p.StartsWith("Axis="))
                                    continue;

                                var index = 0;
                                var pos = 0.0;
                                var speed = 0.0;
                                var offset = 0.0;

                                foreach (var a in p.Split(';'))
                                {
                                    if (a.StartsWith("Axis="))
                                        index = int.Parse(a.Replace("Axis=", string.Empty));
                                    else if (a.StartsWith("Pos="))
                                        pos = float.Parse(a.Replace("Pos=", string.Empty));
                                    else if (a.StartsWith("Speed="))
                                        speed = float.Parse(a.Replace("Speed=", string.Empty));
                                    else if (a.StartsWith("Offset="))
                                        offset = float.Parse(a.Replace("Offset=", string.Empty));
                                }

                                d.Add(index, new Dictionary<string, string>());
                                d[index].Add(Position, pos.ToString(CultureInfo.InvariantCulture));
                                d[index].Add(Speed, speed.ToString(CultureInfo.InvariantCulture));
                                d[index].Add(Offset, offset.ToString(CultureInfo.InvariantCulture));
                            }

                            var programRunAction = new ProgramRunAction
                            {
                                Index = blockIndex,
                                ProgramName = name,
                                EndIndex = endIndex
                            };

                            programRunAction.Action = () =>
                            {
                                RobotRun(d);
                                var doneList = d.Keys.ToDictionary(t => t, t => false.ToString());
                                while (true)
                                {
                                    var doneCount = doneList.Values.Count(t => t == true.ToString());

                                    if (doneCount == d.Count)
                                        break;

                                    lock (LockState)
                                    {
                                        var isBreak =
                                        d.Keys.Any(
                                            t =>
                                                SingleAxisState[t][IsStandstill] == false.ToString() &&
                                                SingleAxisState[t][IsMoving] == false.ToString());

                                        if (isBreak)
                                        {
                                            programRunAction.Index = findPm.Blocks.Length - 1;
                                            break;
                                        }

                                        foreach (var t in d.Keys.Where(t => SingleAxisState[t][IsMoveDone] == true.ToString()))
                                            doneList[t] = true.ToString();
                                    }

                                    Thread.Sleep(5);
                                }
                            };

                            programRunAction.Action.BeginInvoke(StatusRunEnd, programRunAction);
                        }
                    }
                    else if (block.StartsWith("Set:"))
                    {
                        var set = block.Replace("Set:", string.Empty)
                            .Split(new[] { "[", "]" }, StringSplitOptions.RemoveEmptyEntries);

                        var programRunAction = new ProgramRunAction
                        {
                            Index = blockIndex,
                            ProgramName = name,
                            EndIndex = endIndex,
                            Action = () =>
                            {
                                foreach (var p in set)
                                {
                                    for (var i = 0; i < 100; i++)
                                    {
                                        var str = string.Format("Do{0}", i);

                                        if (!p.StartsWith(str + "="))
                                            continue;

                                        var targetValue = int.Parse(p.Replace(str + "=", string.Empty));

                                        var fi = GetType().GetField(str);
                                        if (fi == null) continue;
                                        fi.SetValue(this, targetValue.ToString());
                                        break;
                                    }

                                    for (var i = 0; i < 100; i++)
                                    {
                                        var str = string.Format("So{0}", i);

                                        if (!p.StartsWith(str + "="))
                                            continue;

                                        var targetValue = int.Parse(p.Replace(str + "=", string.Empty));

                                        var fi = GetType().GetField(str);
                                        if (fi == null) continue;
                                        if (targetValue == 1)
                                            fi.SetValue(this, true);
                                        else if (targetValue == 0)
                                            fi.SetValue(this, false);
                                        break;
                                    }
                                }
                            }
                        };

                        programRunAction.Action.BeginInvoke(StatusRunEnd, programRunAction);
                    }
                    else if (block.StartsWith("Wait:"))
                    {
                        var wait = block.Replace("Wait:", string.Empty)
                           .Split(new[] { "[", "]" }, StringSplitOptions.RemoveEmptyEntries);

                        var programRunAction = new ProgramRunAction
                        {
                            Index = blockIndex,
                            ProgramName = name,
                            EndIndex = endIndex,
                            Action = () =>
                            {
                                foreach (var p in wait)
                                {
                                    for (var i = 0; i < 100; i++)
                                    {
                                        var str = string.Format("Di{0}", i);

                                        if (!p.StartsWith(str + "="))
                                            continue;

                                        var targetValue = int.Parse(p.Replace(str + "=", string.Empty));

                                        var fi = GetType().GetField(str);
                                        if (fi == null)
                                            continue;

                                        while (true)
                                        {
                                            Thread.Sleep(5);

                                            if (IsBreak)
                                                break;

                                            var actualValue = fi.GetValue(this);
                                            if (actualValue != null && actualValue.ToString() == targetValue.ToString())
                                                break;
                                        }
                                    }

                                    for (var i = 0; i < 100; i++)
                                    {
                                        var str = string.Format("Si{0}", i);

                                        if (!p.StartsWith(str + "="))
                                            continue;

                                        var targetValue = int.Parse(p.Replace(str + "=", string.Empty));

                                        var fi = GetType().GetField(str);
                                        if (fi == null)
                                            continue;

                                        while (true)
                                        {
                                            Thread.Sleep(5);

                                            if (IsBreak)
                                                break;

                                            var actualValue = fi.GetValue(this);
                                            if (actualValue == null)
                                                continue;
                                            if (targetValue == 1)
                                            {
                                                if ((bool)actualValue)
                                                    break;
                                            }
                                            else if (targetValue == 0)
                                            {
                                                if ((bool)actualValue == false)
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }
                        };

                        programRunAction.Action.BeginInvoke(StatusRunEnd, programRunAction);
                    }
                    else if (block.StartsWith("Delay:"))
                    {
                        var delay = block.Replace("Delay:", string.Empty)
                            .Split(new[] { "[", "]" }, StringSplitOptions.RemoveEmptyEntries);

                        var programRunAction = new ProgramRunAction
                        {
                            Index = blockIndex,
                            ProgramName = name,
                            EndIndex = endIndex,
                            Action = () =>
                            {
                                foreach (var p in delay)
                                {
                                    if (!p.StartsWith("WaitTime="))
                                        continue;

                                    var targetMs = int.Parse(p.Replace("WaitTime=", string.Empty));
                                    const int baseMs = 5;
                                    var startTime = DateTime.Now;

                                    while (true)
                                    {
                                        if (IsBreak)
                                            break;

                                        Thread.Sleep(baseMs);
                                        var endTime = DateTime.Now;
                                        if (ValueHelper.GetTimeSpanMs(startTime, endTime) >= targetMs)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        };

                        programRunAction.Action.BeginInvoke(StatusRunEnd, programRunAction);
                    }
                }
            }
        }

        private void StatusRunEnd(IAsyncResult ir)
        {
            try
            {
                if (ir == null)
                    return;

                var temp = ir.AsyncState as ProgramRunAction;
                if (temp == null)
                    return;

                temp.Action.EndInvoke(ir);

                if (temp.EndIndex != -1 && temp.EndIndex == temp.Index)
                {
                    if (RobotJogging.RobotPrograms != null)
                    {
                        var find = RobotJogging.RobotPrograms.ToList().Find(f => f.Name == temp.ProgramName);
                        if (find != null && find.Blocks != null)
                        {
                            RobotRunProgram(temp.ProgramName, find.Blocks.Length, temp.EndIndex);
                            return;
                        }
                    }
                }

                RobotRunProgram(temp.ProgramName, temp.Index + 1, temp.EndIndex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        internal class ProgramRunAction
        {
            public int Index;
            public Action Action;
            public string ProgramName;
            public int EndIndex;
        }

        #endregion

        #region Pallet

        public readonly Dictionary<int, Pallet> PalletDictionary =
            new Dictionary<int, Pallet>();

        [Description("R,Pallet0当前点位索引")]
        public int Pattet0RunIndex;

        [Description("R,Pallet1当前点位索引")]
        public int Pattet1RunIndex;

        [Description("R,Pallet2当前点位索引")]
        public int Pattet2RunIndex;

        [Description("R,Pallet3当前点位索引")]
        public int Pattet3RunIndex;

        [Description("R,Pallet4当前点位索引")]
        public int Pattet4RunIndex;

        [Description("R,Pallet5当前点位索引")]
        public int Pattet5RunIndex;

        [Description("R,Pallet6当前点位索引")]
        public int Pattet6RunIndex;

        [Description("R,Pallet7当前点位索引")]
        public int Pattet7RunIndex;

        [Description("R,Pallet8当前点位索引")]
        public int Pattet8RunIndex;

        [Description("R,Pallet9当前点位索引")]
        public int Pattet9RunIndex;

        public void PattetReset(string palletIndex)
        {
            if (string.IsNullOrEmpty(palletIndex))
                return;

            int index;
            if (int.TryParse(palletIndex, out index))
            {
                if (PalletDictionary.ContainsKey(index) && !PalletDictionary[index].IsRunning)
                {
                    var field = GetType().GetField(string.Format("Pattet{0}RunIndex", index));
                    if (field != null)
                        field.SetValue(this, 0);

                    //if (index == 0)
                    //    Pattet0RunIndex = 0;
                    //else if (index == 1)
                    //    Pattet1RunIndex = 0;
                    //else if (index == 2)
                    //    Pattet2RunIndex = 0;
                    //else if (index == 3)
                    //    Pattet3RunIndex = 0;
                    //else if (index == 4)
                    //    Pattet4RunIndex = 0;
                    //else if (index == 5)
                    //    Pattet5RunIndex = 0;
                    //else if (index == 6)
                    //    Pattet6RunIndex = 0;
                    //else if (index == 7)
                    //    Pattet7RunIndex = 0;
                    //else if (index == 8)
                    //    Pattet8RunIndex = 0;
                    //else if (index == 9)
                    //    Pattet9RunIndex = 0;
                }
            }
        }

        public void PattetBlockSkip(string palletIndex, string blockSkipTo)
        {
            if (string.IsNullOrEmpty(palletIndex) || string.IsNullOrEmpty(blockSkipTo))
                return;

            int whichPallet;
            int skipBlockIndex;
            if (!int.TryParse(palletIndex, out whichPallet) || !int.TryParse(blockSkipTo, out skipBlockIndex))
                return;
            if (!PalletDictionary.ContainsKey(whichPallet))
                return;
            if (PalletDictionary[whichPallet].IsRunning)
                return;
            if (PalletDictionary[whichPallet].Blocks.Count - 1 >= skipBlockIndex)
                PalletDictionary[whichPallet].BlockIndex = skipBlockIndex;
        }

        public void ReLoadPallet()
        {
            PalletDictionary.Clear();

            for (var i = 0; i < 9; i++)
            {
                PalletDictionary.Add(
                    i, new Pallet { PalletIndex = i, BlockIndex = -1 });

                if (RobotJogging.RobotPallets == null)
                    continue;
                var pallets = RobotJogging.RobotPallets.ToList();
                var pallet = pallets.Find(f => f.PattetIndex != null && f.PattetIndex == i.ToString());
                if (pallet == null)
                    continue;
                PalletDictionary[i].Name = pallet.PattetName;
                if (pallet.Blocks == null)
                    continue;
                foreach (var block in pallet.Blocks)
                    PalletDictionary[i].Blocks.Add(block);
            }
        }

        public class Pallet
        {
            public int PalletIndex;
            public string Name;
            public List<string> Blocks = new List<string>();
            public int BlockIndex = -1;
            public bool IsRunning;
        }

        #endregion
    }
}
