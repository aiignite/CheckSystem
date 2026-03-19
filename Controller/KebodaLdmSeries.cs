using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Controller
{
    /// <summary>
    /// 科博达系列
    /// </summary>
    [Description("CAN-Product,科博达系列")]
    public sealed class KebodaLdmSeries : ControllerBase
    {
        public CanBus Can;

        [Description("R/W,SOA1")]
        public byte Soa1 = 0x00;

        [Description("R/W,SOA2")]
        public byte Soa2 = 0x00;

        [Description("R/W,KL15")]
        public bool Kl15;

        [Description("R/W,是否为MQB")]
        public bool IsMqb;

        [Description("R/W,是否包含MATRIX")]
        public bool IsHaveMatrix;

        #region Matrix变量

        [Description("R/W,EnvLlpMxbL1")]
        private byte EnvLlpMxbL1;//D01

        [Description("R/W,EnvLlpMxbL2")]
        private byte EnvLlpMxbL2;//D02

        [Description("R/W,EnvLlpMxbL3")]
        private byte EnvLlpMxbL3;//D03

        [Description("R/W,EnvLlpMxbL4")]
        private byte EnvLlpMxbL4;//D04

        [Description("R/W,EnvLlpMxbL5")]
        private byte EnvLlpMxbL5;//D05

        [Description("R/W,EnvLlpMxbL6")]
        private byte EnvLlpMxbL6;//D06

        [Description("R/W,EnvLlpMxbL7")]
        private byte EnvLlpMxbL7;//D07

        [Description("R/W,EnvLlpMxbL8")]
        private byte EnvLlpMxbL8;//D08

        [Description("R/W,EnvLlpMxbL9")]
        private byte EnvLlpMxbL9;//D09

        [Description("R/W,EnvLlpMxbL10")]
        private byte EnvLlpMxbL10;//D10

        [Description("R/W,EnvLlpMxbL11")]
        private byte EnvLlpMxbL11;//D11

        [Description("R/W,EnvLlpMxbL12")]
        private byte EnvLlpMxbL12;//D12

        [Description("R/W,EnvLlpMxbL22")]
        private byte EnvLlpMxbL22;//D22

        [Description("R/W,EnvLlpMxbL21")]
        private byte EnvLlpMxbL21;//D21

        [Description("R/W,EnvLlpMxbL20")]
        private byte EnvLlpMxbL20;//D20

        [Description("R/W,EnvLlpMxbL19")]
        private byte EnvLlpMxbL19;//D19

        [Description("R/W,EnvLlpMxbL18")]
        private byte EnvLlpMxbL18;//D18

        [Description("R/W,EnvLlpMxbL17")]
        private byte EnvLlpMxbL17;//D17

        [Description("R/W,EnvLlpMxbL16")]
        private byte EnvLlpMxbL16;//D16

        [Description("R/W,EnvLlpMxbL15")]
        private byte EnvLlpMxbL15;//D15

        [Description("R/W,EnvLlpMxbL14")]
        private byte EnvLlpMxbL14;//D14

        [Description("R/W,EnvLlpMxbL13")]
        private byte EnvLlpMxbL13;//D13

        [Description("R/W,EnvLlpMxbL32")]
        private byte EnvLlpMxbL32;//D32

        [Description("R/W,EnvLlpMxbL31")]
        private byte EnvLlpMxbL31;//D31

        [Description("R/W,EnvLlpMxbL30")]
        private byte EnvLlpMxbL30;//D30

        [Description("R/W,EnvLlpMxbL29")]
        private byte EnvLlpMxbL29;//D29

        [Description("R/W,EnvLlpMxbL28")]
        private byte EnvLlpMxbL28;//D28

        [Description("R/W,EnvLlpMxbL27")]
        private byte EnvLlpMxbL27;//D27

        [Description("R/W,EnvLlpMxbL26")]
        private byte EnvLlpMxbL26;//D26

        [Description("R/W,EnvLlpMxbL25")]
        private byte EnvLlpMxbL25;//D25

        [Description("R/W,EnvLlpMxbL24")]
        private byte EnvLlpMxbL24;//D24

        [Description("R/W,EnvLlpMxbL23")]
        private byte EnvLlpMxbL23;//D23

        [Description("R/W,EnvLlpMxbR1")]
        private byte EnvLlpMxbR1;//D01

        [Description("R/W,EnvLlpMxbR2")]
        private byte EnvLlpMxbR2;//D02

        [Description("R/W,EnvLlpMxbR3")]
        private byte EnvLlpMxbR3;//D03

        [Description("R/W,EnvLlpMxbR4")]
        private byte EnvLlpMxbR4;//D04

        [Description("R/W,EnvLlpMxbR5")]
        private byte EnvLlpMxbR5;//D05

        [Description("R/W,EnvLlpMxbR6")]
        private byte EnvLlpMxbR6;//D06

        [Description("R/W,EnvLlpMxbR7")]
        private byte EnvLlpMxbR7;//D07

        [Description("R/W,EnvLlpMxbR8")]
        private byte EnvLlpMxbR8;//D08

        [Description("R/W,EnvLlpMxbR9")]
        private byte EnvLlpMxbR9;//D09

        [Description("R/W,EnvLlpMxbR10")]
        private byte EnvLlpMxbR10;//D10

        [Description("R/W,EnvLlpMxbR11")]
        private byte EnvLlpMxbR11;//D11

        [Description("R/W,EnvLlpMxbR12")]
        private byte EnvLlpMxbR12;//D12

        [Description("R/W,EnvLlpMxbR22")]
        private byte EnvLlpMxbR22;//D22

        [Description("R/W,EnvLlpMxbR21")]
        private byte EnvLlpMxbR21;//D21

        [Description("R/W,EnvLlpMxbR20")]
        private byte EnvLlpMxbR20;//D20

        [Description("R/W,EnvLlpMxbR19")]
        private byte EnvLlpMxbR19;//D19

        [Description("R/W,EnvLlpMxbR18")]
        private byte EnvLlpMxbR18;//D18

        [Description("R/W,EnvLlpMxbR17")]
        private byte EnvLlpMxbR17;//D17

        [Description("R/W,EnvLlpMxbR16")]
        private byte EnvLlpMxbR16;//D16

        [Description("R/W,EnvLlpMxbR15")]
        private byte EnvLlpMxbR15;//D15

        [Description("R/W,EnvLlpMxbR14")]
        private byte EnvLlpMxbR14;//D14

        [Description("R/W,EnvLlpMxbR13")]
        private byte EnvLlpMxbR13;//D13

        [Description("R/W,EnvLlpMxbR32")]
        private byte EnvLlpMxbR32;//D32

        [Description("R/W,EnvLlpMxbR31")]
        private byte EnvLlpMxbR31;//D31

        [Description("R/W,EnvLlpMxbR30")]
        private byte EnvLlpMxbR30;//D30

        [Description("R/W,EnvLlpMxbR29")]
        private byte EnvLlpMxbR29;//D29

        [Description("R/W,EnvLlpMxbR28")]
        private byte EnvLlpMxbR28;//D28

        [Description("R/W,EnvLlpMxbR27")]
        private byte EnvLlpMxbR27;//D27

        [Description("R/W,EnvLlpMxbR26")]
        private byte EnvLlpMxbR26;//D26

        [Description("R/W,EnvLlpMxbR25")]
        private byte EnvLlpMxbR25;//D25

        [Description("R/W,EnvLlpMxbR24")]
        private byte EnvLlpMxbR24;//D24

        [Description("R/W,EnvLlpMxbR23")]
        private byte EnvLlpMxbR23;//D23
        #endregion

        private bool _isSleep = true;
        private readonly Dictionary<LampControl, MatrixValDefinition> _lampControlList =
           new Dictionary<LampControl, MatrixValDefinition>();
        private readonly object _mxbLocker = new object();
        private readonly object _canSendLock = new object();

        public KebodaLdmSeries(string name)
            : base(name)
        {
            foreach (var temp
                in Enum.GetValues(typeof(LampControl)).Cast<LampControl>())
                _lampControlList.Add(temp, temp.GetCustomAttribute<MatrixValDefinition>());

            #region init data

            _motorolaMatrix0X190.MatrixData = new byte[] { 0xFE, 0x07, 0x00, 0xFE, 0x07, 0x00, 0x00, 0x00 };
            _motorolaMatrix0X192.MatrixData = new byte[] { 0xFE, 0x07, 0x00, 0xFE, 0x07, 0x00, 0x00, 0x00 };
            _motorolaMatrix0X110.MatrixData = new byte[] { 0xFE, 0x07, 0xFE, 0x07, 0x00, 0x00, 0x00, 0x00 };
            _motorolaMatrix0X196.MatrixData = new byte[] { 0xFF, 0x0F, 0xFF, 0x02 };
            _motorolaMatrix0X197.MatrixData = new byte[] { 0xFE, 0x0F, 0xF0, 0x07, 0x00, 0x00 };
            _motorolaMatrix0X19A.MatrixData = new byte[] { 0xFE, 0x0F, 0xF0, 0x00, 0xFE, 0xFE };
            _motorolaMatrix0X54E.MatrixData = new byte[] { 0x80, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00 };

            #endregion

            CanBus.PushCanMsg += CanBus_PushCanMsg;
            MainWork();
        }

        ~KebodaLdmSeries()
        {
            Dispose();
        }

        private void CanBus_PushCanMsg(
            string name, CanBus.CanDataPackage data, CanBus.OnPushCanDataType onPushCanDataType)
        {
            if (!string.IsNullOrEmpty(name) &&
                Can != null && !string.IsNullOrEmpty(Can.Name) && Can.Name == name &&
                (onPushCanDataType == CanBus.OnPushCanDataType.FilterRx ||
                 onPushCanDataType == CanBus.OnPushCanDataType.NotFilterRx))
            {
                switch (data.CanId)
                {
                    case 0x197:
                        LeftMdfRealPos = CalMdfRealPos(data.CanData, 0, 12);
                        _isLeftLamp = true;
                        break;

                    case 0x199:
                        RightMdfRealPos = CalMdfRealPos(data.CanData, 0, 12);
                        _isLeftLamp = false;
                        break;

                    case 0x19A:
                        LeftAfsRealPos = CalAfsRealPos(data.CanData, 0, 12);
                        _isLeftLamp = true;
                        break;

                    case 0x19C:
                        RightMdfRealPos = CalAfsRealPos(data.CanData, 0, 12);
                        _isLeftLamp = false;
                        break;

                    case 0x196:
                        var leftMotorPos = CalMotorRealPos(data.CanData, 0, 12);
                        //Console.WriteLine(@"左马达位置：" + leftMotorPos);
                        break;

                    case 0x198:
                        var rightMotorPos = CalMotorRealPos(data.CanData, 0, 12);
                        //Console.WriteLine(@"右马达位置：" + rightMotorPos);
                        break;

                    case 0x19B:
                        lock (_lockErrorL)
                        {
                            DrlLState = GetIntelValue(data.CanData, 20, 1).ToString();
                            PlLState = GetIntelValue(data.CanData, 17, 1).ToString();
                            TurnLState = GetIntelValue(data.CanData, 15, 1).ToString();
                            ParkLState = GetIntelValue(data.CanData, 16, 1).ToString();

                            _lastErrorL = HighPrecisionTimer.GetTimestamp();
                        }
                        break;

                    case 0x19D:
                        lock (_lockErrorR)
                        {
                            DrlRState = GetIntelValue(data.CanData, 20, 1).ToString();
                            PlRState = GetIntelValue(data.CanData, 17, 1).ToString();
                            TurnRState = GetIntelValue(data.CanData, 15, 1).ToString();
                            ParkRState = GetIntelValue(data.CanData, 16, 1).ToString();

                            _lastErrorR = HighPrecisionTimer.GetTimestamp();
                        }
                        break;
                }
            }
        }

        private void MainWork()
        {
            // 100ms
            SetTimer(new MyTaskScheduler.TaskInfo()
            {
                Action = () =>
                {
                    lock (_lockErrorL)
                    {
                        var nowTs = HighPrecisionTimer.GetTimestamp();
                        var costMs = HighPrecisionTimer.GetTimestampIntervalMs(_lastErrorL, nowTs);
                        if (costMs > 1500)
                        {
                            DrlLState = string.Empty;
                            PlLState = string.Empty;
                            TurnLState = string.Empty;
                            ParkLState = string.Empty;
                        }
                    }

                    lock (_lockErrorR)
                    {
                        var nowTs = HighPrecisionTimer.GetTimestamp();
                        var costMs = HighPrecisionTimer.GetTimestampIntervalMs(_lastErrorR, nowTs);
                        if (costMs > 1500)
                        {
                            DrlRState = string.Empty;
                            PlRState = string.Empty;
                            TurnRState = string.Empty;
                            ParkRState = string.Empty;
                        }
                    }
                },
                Interval = 100
            });

            // 20ms
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = SendTask(() =>
                {
                    _motorolaMatrix0X192.MatrixData[6] = Soa1;
                    _motorolaMatrix0X192.MatrixData[7] = Soa2;

                    if (_isMotorCycle)
                    {
                        _motorCycleCount++;

                        if (_motorCycleIndex == -2)
                        {
                            _motorolaMatrix0X190.MatrixData = new byte[] { 0x20, 0x09, 0x00, 0xFE, 0x0A, 0x00, 0x00, 0x00 };

                            if (_motorCycleCount == 109)
                            {
                                _motorCycleCount = 0;
                                _motorCycleIndex++;
                            }
                        }
                        else if (_motorCycleIndex == -1)
                        {
                            _motorolaMatrix0X190.MatrixData = new byte[] { 0x58, 0x02, 0x00, 0x58, 0x02, 0x00, 0x00, 0x00 };

                            if (_motorCycleCount == 15)
                            {
                                _motorCycleCount = 0;
                                _motorCycleIndex++;
                            }
                        }
                        else if (_motorCycleIndex == 0)
                        {
                            _motorolaMatrix0X190.MatrixData = new byte[] { 0xD2, 0x0A, 0x00, 0xD2, 0x0A, 0x00, 0x00, 0x00 };

                            if (_motorCycleCount == 50)
                            {
                                _motorCycleCount = 0;
                                _motorCycleIndex++;
                            }
                        }
                        else if (_motorCycleIndex == 1)
                        {
                            _motorolaMatrix0X190.MatrixData = new byte[] { 0xF9, 0x09, 0x00, 0xF9, 0x09, 0x00, 0x00, 0x00 };

                            if (_motorCycleCount == 50)
                            {
                                _motorCycleCount = 0;
                                _motorCycleIndex++;
                            }
                        }
                        else if (_motorCycleIndex == 2)
                        {
                            _motorolaMatrix0X190.MatrixData = new byte[] { 0xD2, 0x0A, 0x00, 0xD2, 0x0A, 0x00, 0x00, 0x00 };

                            if (_motorCycleCount == 50)
                            {
                                _motorCycleCount = 0;
                                _motorCycleIndex++;
                            }
                        }
                        else if (_motorCycleIndex == 3)
                        {
                            _motorolaMatrix0X190.MatrixData = new byte[] { 0x6E, 0x07, 0x00, 0x6E, 0x07, 0x00, 0x00, 0x00 };

                            if (_motorCycleCount == 50)
                            {
                                _motorCycleCount = 0;
                                _motorCycleIndex++;
                            }
                        }
                        else if (_motorCycleIndex == 4)
                        {
                            _motorolaMatrix0X190.MatrixData = new byte[] { 0x20, 0x09, 0x00, 0x20, 0x09, 0x00, 0x00, 0x00 };

                            if (_motorCycleCount == 50)
                            {
                                _motorCycleCount = 0;
                                _motorCycleIndex++;
                            }
                        }
                        else if (_motorCycleIndex == 5)
                        {
                            _motorolaMatrix0X190.MatrixData = new byte[] { 0x47, 0x08, 0x00, 0x47, 0x08, 0x00, 0x00, 0x00 };

                            if (_motorCycleCount == 50)
                            {
                                _motorCycleCount = 0;
                                _motorCycleIndex++;
                            }
                        }
                        else if (_motorCycleIndex == 6)
                        {
                            _motorolaMatrix0X190.MatrixData = new byte[] { 0x20, 0x09, 0x00, 0x20, 0x09, 0x00, 0x00, 0x00 };

                            if (_motorCycleCount == 50)
                            {
                                _motorCycleCount = 0;
                                _motorCycleIndex++;
                            }
                        }
                        else if (_motorCycleIndex == 7)
                        {
                            _motorolaMatrix0X190.MatrixData = new byte[] { 0x95, 0x06, 0x00, 0x95, 0x06, 0x00, 0x00, 0x00 };

                            if (_motorCycleCount == 50)
                            {
                                _motorCycleCount = 0;
                                _motorCycleIndex++;
                            }
                        }
                        else if (_motorCycleIndex == 8)
                        {
                            _motorolaMatrix0X190.MatrixData = new byte[] { 0xF9, 0x09, 0x00, 0xF9, 0x09, 0x00, 0x00, 0x00 };

                            if (_motorCycleCount == 50)
                            {
                                _motorCycleCount = 0;
                                _motorCycleIndex++;
                            }
                        }
                        else if (_motorCycleIndex == 9)
                        {
                            _motorolaMatrix0X190.MatrixData = new byte[] { 0x6E, 0x07, 0x00, 0x6E, 0x07, 0x00, 0x00, 0x00 };

                            if (_motorCycleCount == 50)
                            {
                                _motorCycleCount = 0;
                                _motorCycleIndex++;
                            }
                        }
                        else if (_motorCycleIndex == 10)
                        {
                            _motorolaMatrix0X190.MatrixData = new byte[] { 0xF9, 0x09, 0x00, 0xF9, 0x09, 0x00, 0x00, 0x00 };

                            if (_motorCycleCount == 50)
                            {
                                _motorCycleCount = 0;
                                _motorCycleIndex++;
                            }
                        }
                        else if (_motorCycleIndex == 11)
                        {
                            _motorolaMatrix0X190.MatrixData = new byte[] { 0xE3, 0x04, 0x00, 0xE3, 0x04, 0x00, 0x00, 0x00 };

                            if (_motorCycleCount == 50)
                            {
                                _motorCycleCount = 0;
                                _motorCycleIndex++;
                            }
                        }
                        else if (_motorCycleIndex == 12)
                        {
                            _motorolaMatrix0X190.MatrixData = new byte[] { 0xBC, 0x05, 0x00, 0xBC, 0x05, 0x00, 0x00, 0x00 };

                            if (_motorCycleCount == 50)
                            {
                                _motorCycleCount = 0;
                                _motorCycleIndex++;
                            }
                        }
                        else if (_motorCycleIndex == 13)
                        {
                            _motorolaMatrix0X190.MatrixData = new byte[] { 0x58, 0x02, 0x00, 0x58, 0x02, 0x00, 0x00, 0x00 };

                            if (_motorCycleCount == 200)
                            {
                                _motorCycleCount = 0;
                                _motorCycleIndex = 0;
                            }
                        }
                    }
                    else
                    {
                        // L:
                        var valueL = (uint)(Math.Round((_motorLPos + 1023) / 0.5, 0, MidpointRounding.AwayFromZero));
                        _motorolaMatrix0X190.MatrixData = SetIntelValue(_motorolaMatrix0X190.MatrixData, 0, 12, valueL);

                        // R:
                        var valueR = (uint)(Math.Round((_motorRPos + 1023) / 0.5, 0, MidpointRounding.AwayFromZero));
                        _motorolaMatrix0X190.MatrixData = SetIntelValue(_motorolaMatrix0X190.MatrixData, 24, 12, valueR);
                    }

                    _motorolaMatrix0X190.UpdateData(_isMotorLReset ? _lampControlList[LampControl.LeftMotorResetOn] : _lampControlList[LampControl.LeftMotorResetOff]);
                    _motorolaMatrix0X190.UpdateData(_isMotorRReset ? _lampControlList[LampControl.RightMotorResetOn] : _lampControlList[LampControl.RightMotorResetOff]);

                    var lstPages = new List<CanBus.CanDataPackage>
                    {
                        new CanBus.CanDataPackage(
                            _motorolaMatrix0X110.CanId,
                            CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                            CanBus.CanFormat.Data,
                            _motorolaMatrix0X110.MatrixData), // 周期帧
                        new CanBus.CanDataPackage(
                            _motorolaMatrix0X190.CanId,
                            CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                            CanBus.CanFormat.Data,
                            _motorolaMatrix0X190.MatrixData), // LWR
                        new CanBus.CanDataPackage(
                            _motorolaMatrix0X192.CanId,
                            CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                            CanBus.CanFormat.Data,
                            _motorolaMatrix0X192.MatrixData),
                        new CanBus.CanDataPackage(
                            _motorolaMatrix0X194.CanId,
                            CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                            CanBus.CanFormat.Data,
                            _motorolaMatrix0X194.MatrixData),
                        new CanBus.CanDataPackage(
                            _motorolaMatrix0X195.CanId,
                            CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                            CanBus.CanFormat.Data,
                            _motorolaMatrix0X195.MatrixData)
                        // SOA
                    };

                    _motorolaMatrix0X0Fd.MatrixData = _list0X0Fd[_bz0X0Fd];
                    _bz0X0Fd++;
                    if (_bz0X0Fd >= 16)
                        _bz0X0Fd = 0;
                    lstPages.Add(new CanBus.CanDataPackage(
                        _motorolaMatrix0X0Fd.CanId,
                        CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data,
                        _motorolaMatrix0X0Fd.MatrixData));
                    lstPages.Add(Set15F());

                    return lstPages.ToArray();
                }),
                Interval = 20
            });

            // 40ms
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = SendTask(() =>
                {
                    var lstPages = new List<CanBus.CanDataPackage>();

                    _motorolaMatrix0X040.MatrixData = _list0X040[_bz0X040];
                    _bz0X040++;
                    if (_bz0X040 >= 16)
                        _bz0X040 = 0;
                    lstPages.Add(new CanBus.CanDataPackage(
                        _motorolaMatrix0X040.CanId,
                        CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data,
                        _motorolaMatrix0X040.MatrixData));

                    lstPages.Add(Set3C0());

                    return lstPages.ToArray();
                }),
                Interval = 40
            });

            // 100ms
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = SendTask(() =>
                {
                    var lstPages = new List<CanBus.CanDataPackage>();

                    if (_isTurnLOn)
                    {
                        if (_isBlink)
                        {
                            if (_blinkCount < 4)
                            {
                                _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftTurnOn]);
                                _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftTurnLoopOn]);
                            }
                            else if (_blinkCount == 4)
                            {
                                _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftTurnOff]);
                                _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftTurnLoopOff]);
                            }
                            else if (_blinkCount == 8)
                            {
                                _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftTurnOn]);
                                _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftTurnLoopOn]);
                            }
                        }
                        else
                        {
                            _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftTurnOn]);
                            _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftTurnLoopOn]);
                        }
                    }
                    else
                    {
                        _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftTurnOff]);
                        _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftTurnLoopOff]);
                    }

                    if (_isTurnROn)
                    {
                        if (_isBlink)
                        {
                            if (_blinkCount == 4)
                            {
                                _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightTurnOff]);
                                _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightTurnLoopOff]);
                            }
                            else if (_blinkCount == 8)
                            {
                                _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightTurnOn]);
                                _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightTurnLoopOn]);
                            }
                        }
                        else
                        {
                            _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightTurnOn]);
                            _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightTurnLoopOn]);
                        }
                    }
                    else
                    {
                        _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightTurnOff]);
                        _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightTurnLoopOff]);
                    }

                    _blinkCount++;
                    if (_blinkCount > 8)
                        _blinkCount = 0;

                    var bz193 = _motorolaMatrix0X193.MatrixData[1].GetByteLowOrder();
                    _motorolaMatrix0X193.MatrixData[0] = GetCheckSum(bz193, _motorolaMatrix0X193.MatrixData,
                        _lcmSollBzTable);

                    var temp193Bs = new byte[8];
                    Array.Copy(_motorolaMatrix0X193.MatrixData, temp193Bs, 8);
                    lstPages.Add(new CanBus.CanDataPackage(
                        _motorolaMatrix0X193.CanId,
                        CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data,
                        temp193Bs));

                    if (bz193 == 0x0F)
                        _motorolaMatrix0X193.MatrixData[1] = (byte)(_motorolaMatrix0X193.MatrixData[1] - 0x0F);
                    else
                        _motorolaMatrix0X193.MatrixData[1] = (byte)(_motorolaMatrix0X193.MatrixData[1] + 0x01);

                    lstPages.Add(new CanBus.CanDataPackage(
                        0x3BE,
                        CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data,
                        _list0X3Be[_bz0X3Be]));
                    _bz0X3Be++;
                    if (_bz0X3Be >= 16)
                        _bz0X3Be = 0;

                    _motorolaMatrix0X3D5.UpdateData(new MatrixValDefinition(15, 1,
                        _llpLLowBeam ? (byte)0x01 : (byte)0x00));

                    var bz3D5 = _motorolaMatrix0X3D5.MatrixData[1].GetByteLowOrder();
                    _motorolaMatrix0X3D5.MatrixData[0] = GetCheckSum(bz3D5, _motorolaMatrix0X3D5.MatrixData,
                        _lcmSollBzTable2);

                    var temp3D5Bs = new byte[8];
                    Array.Copy(_motorolaMatrix0X3D5.MatrixData, temp3D5Bs, 8);
                    lstPages.Add(new CanBus.CanDataPackage(
                        _motorolaMatrix0X3D5.CanId,
                        CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data,
                        temp3D5Bs));

                    if (bz3D5 == 0x0F)
                        _motorolaMatrix0X3D5.MatrixData[1] = (byte)(_motorolaMatrix0X3D5.MatrixData[1] - 0x0F);
                    else
                        _motorolaMatrix0X3D5.MatrixData[1] = (byte)(_motorolaMatrix0X3D5.MatrixData[1] + 0x01);

                    lstPages.Add(new CanBus.CanDataPackage(
                        0x1B0000A9,
                        CanBus.CanProtocol.Can,
                        CanBus.CanType.Extended,
                        CanBus.CanFormat.Data,
                        new byte[] { 0x00, 0x00, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00 })); // VW310、336必须

                    return lstPages.ToArray();

                }),
                Interval = 100
            });

            // 200ms L
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = SendTask(() =>
                {
                    var lstPages = new List<CanBus.CanDataPackage>();

                    lock (_mxbLocker)
                    {
                        if (IsHaveMatrix)
                        {
                            #region Matrix led

                            LUMI_LED();
                            //_motorolaMatrix0X160.MatrixData = new byte[] { 0xE4, 0xAF, 0xF2, 0x00, 0x00, 0x00, 0x00, 0x00 };

                            if (_isLeftLamp)
                            {
                                lstPages.Add(new CanBus.CanDataPackage(
                                    _motorolaMatrix0X160.CanId,
                                    CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                                    CanBus.CanFormat.Data,
                                    _motorolaMatrix0X160.MatrixData));
                                lstPages.Add(new CanBus.CanDataPackage(
                                    _motorolaMatrix0X161.CanId,
                                    CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                                    CanBus.CanFormat.Data,
                                    _motorolaMatrix0X161.MatrixData));
                                lstPages.Add(new CanBus.CanDataPackage(
                                    _motorolaMatrix0X162.CanId,
                                    CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                                    CanBus.CanFormat.Data,
                                    _motorolaMatrix0X162.MatrixData));
                                lstPages.Add(new CanBus.CanDataPackage(
                                    _motorolaMatrix0X163.CanId,
                                    CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                                    CanBus.CanFormat.Data,
                                    _motorolaMatrix0X163.MatrixData));
                            }
                            else
                            {
                                //lstPages.Add(new CanBus.CanDataPackage(
                                //    _motorolaMatrix0X169.CanId,
                                //    CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                                //    CanBus.CanFormat.Data,
                                //    _motorolaMatrix0X169.MatrixData));
                                //lstPages.Add(new CanBus.CanDataPackage(
                                //    _motorolaMatrix0X16A.CanId,
                                //    CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                                //    CanBus.CanFormat.Data,
                                //    _motorolaMatrix0X16A.MatrixData));
                                //lstPages.Add(new CanBus.CanDataPackage(
                                //    _motorolaMatrix0X16B.CanId,
                                //    CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                                //    CanBus.CanFormat.Data,
                                //    _motorolaMatrix0X16B.MatrixData));
                                //lstPages.Add(new CanBus.CanDataPackage(
                                //    _motorolaMatrix0X16C.CanId,
                                //    CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                                //    CanBus.CanFormat.Data,
                                //    _motorolaMatrix0X16C.MatrixData));
                            }

                            //lstPages.Add(new CanBus.CanDataPackage(
                            //    _motorolaMatrix0X164.CanId,
                            //    CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                            //    CanBus.CanFormat.Data,
                            //    _motorolaMatrix0X164.MatrixData));
                            //lstPages.Add(new CanBus.CanDataPackage(
                            //    _motorolaMatrix0X165.CanId,
                            //    CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                            //    CanBus.CanFormat.Data,
                            //    _motorolaMatrix0X165.MatrixData));
                            //lstPages.Add(new CanBus.CanDataPackage(
                            //    _motorolaMatrix0X166.CanId,
                            //    CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                            //    CanBus.CanFormat.Data,
                            //    _motorolaMatrix0X166.MatrixData));
                            //lstPages.Add(new CanBus.CanDataPackage(
                            //    _motorolaMatrix0X167.CanId,
                            //    CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                            //    CanBus.CanFormat.Data,
                            //    _motorolaMatrix0X167.MatrixData));
                            //lstPages.Add(new CanBus.CanDataPackage(
                            //    _motorolaMatrix0X168.CanId,
                            //    CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                            //    CanBus.CanFormat.Data,
                            //    _motorolaMatrix0X168.MatrixData));

                            //lstPages.Add(new CanBus.CanDataPackage(
                            //    _motorolaMatrix0X16D.CanId,
                            //    CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                            //    CanBus.CanFormat.Data,
                            //    _motorolaMatrix0X16D.MatrixData));

                            //lstPages.Add(new CanBus.CanDataPackage(
                            //    _motorolaMatrix0X16E.CanId,
                            //    CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                            //    CanBus.CanFormat.Data,
                            //    _motorolaMatrix0X16E.MatrixData));
                            //lstPages.Add(new CanBus.CanDataPackage(
                            //    _motorolaMatrix0X16F.CanId,
                            //    CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                            //    CanBus.CanFormat.Data,
                            //    _motorolaMatrix0X16F.MatrixData));
                            //lstPages.Add(new CanBus.CanDataPackage(
                            //    _motorolaMatrix0X174.CanId,
                            //    CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                            //    CanBus.CanFormat.Data,
                            //    _motorolaMatrix0X174.MatrixData));
                            //lstPages.Add(new CanBus.CanDataPackage(
                            //    _motorolaMatrix0X175.CanId,
                            //    CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                            //    CanBus.CanFormat.Data,
                            //    _motorolaMatrix0X175.MatrixData));

                            #endregion
                        }

                        return lstPages.ToArray();
                    }
                }),
                Interval = 200
            });

            // 200ms R
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = SendTask(() =>
                {
                    var lstPages = new List<CanBus.CanDataPackage>();

                    lock (_mxbLocker)
                    {
                        if (IsHaveMatrix)
                        {
                            #region Matrix led

                            LUMI_LED();
                            //_motorolaMatrix0X160.MatrixData = new byte[] { 0xE4, 0xAF, 0xF2, 0x00, 0x00, 0x00, 0x00, 0x00 };

                            if (_isLeftLamp)
                            {
                                //lstPages.Add(new CanBus.CanDataPackage(
                                //    _motorolaMatrix0X160.CanId,
                                //    CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                                //    CanBus.CanFormat.Data,
                                //    _motorolaMatrix0X160.MatrixData));
                                //lstPages.Add(new CanBus.CanDataPackage(
                                //    _motorolaMatrix0X161.CanId,
                                //    CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                                //    CanBus.CanFormat.Data,
                                //    _motorolaMatrix0X161.MatrixData));
                                //lstPages.Add(new CanBus.CanDataPackage(
                                //    _motorolaMatrix0X162.CanId,
                                //    CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                                //    CanBus.CanFormat.Data,
                                //    _motorolaMatrix0X162.MatrixData));
                                //lstPages.Add(new CanBus.CanDataPackage(
                                //    _motorolaMatrix0X163.CanId,
                                //    CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                                //    CanBus.CanFormat.Data,
                                //    _motorolaMatrix0X163.MatrixData));
                            }
                            else
                            {
                                lstPages.Add(new CanBus.CanDataPackage(
                                    _motorolaMatrix0X169.CanId,
                                    CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                                    CanBus.CanFormat.Data,
                                    _motorolaMatrix0X169.MatrixData));
                                lstPages.Add(new CanBus.CanDataPackage(
                                    _motorolaMatrix0X16A.CanId,
                                    CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                                    CanBus.CanFormat.Data,
                                    _motorolaMatrix0X16A.MatrixData));
                                lstPages.Add(new CanBus.CanDataPackage(
                                    _motorolaMatrix0X16B.CanId,
                                    CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                                    CanBus.CanFormat.Data,
                                    _motorolaMatrix0X16B.MatrixData));
                                lstPages.Add(new CanBus.CanDataPackage(
                                    _motorolaMatrix0X16C.CanId,
                                    CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                                    CanBus.CanFormat.Data,
                                    _motorolaMatrix0X16C.MatrixData));
                            }

                            //lstPages.Add(new CanBus.CanDataPackage(
                            //    _motorolaMatrix0X164.CanId,
                            //    CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                            //    CanBus.CanFormat.Data,
                            //    _motorolaMatrix0X164.MatrixData));
                            //lstPages.Add(new CanBus.CanDataPackage(
                            //    _motorolaMatrix0X165.CanId,
                            //    CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                            //    CanBus.CanFormat.Data,
                            //    _motorolaMatrix0X165.MatrixData));
                            //lstPages.Add(new CanBus.CanDataPackage(
                            //    _motorolaMatrix0X166.CanId,
                            //    CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                            //    CanBus.CanFormat.Data,
                            //    _motorolaMatrix0X166.MatrixData));
                            //lstPages.Add(new CanBus.CanDataPackage(
                            //    _motorolaMatrix0X167.CanId,
                            //    CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                            //    CanBus.CanFormat.Data,
                            //    _motorolaMatrix0X167.MatrixData));
                            //lstPages.Add(new CanBus.CanDataPackage(
                            //    _motorolaMatrix0X168.CanId,
                            //    CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                            //    CanBus.CanFormat.Data,
                            //    _motorolaMatrix0X168.MatrixData));

                            //lstPages.Add(new CanBus.CanDataPackage(
                            //    _motorolaMatrix0X16D.CanId,
                            //    CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                            //    CanBus.CanFormat.Data,
                            //    _motorolaMatrix0X16D.MatrixData));

                            //lstPages.Add(new CanBus.CanDataPackage(
                            //    _motorolaMatrix0X16E.CanId,
                            //    CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                            //    CanBus.CanFormat.Data,
                            //    _motorolaMatrix0X16E.MatrixData));
                            //lstPages.Add(new CanBus.CanDataPackage(
                            //    _motorolaMatrix0X16F.CanId,
                            //    CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                            //    CanBus.CanFormat.Data,
                            //    _motorolaMatrix0X16F.MatrixData));
                            //lstPages.Add(new CanBus.CanDataPackage(
                            //    _motorolaMatrix0X174.CanId,
                            //    CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                            //    CanBus.CanFormat.Data,
                            //    _motorolaMatrix0X174.MatrixData));
                            //lstPages.Add(new CanBus.CanDataPackage(
                            //    _motorolaMatrix0X175.CanId,
                            //    CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                            //    CanBus.CanFormat.Data,
                            //    _motorolaMatrix0X175.MatrixData));

                            #endregion
                        }

                        return lstPages.ToArray();
                    }
                }),
                Interval = 200
            });

            // 1000ms
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = SendTask(() =>
                {
                    var lstPages = new List<CanBus.CanDataPackage>();

                    if (IsMqb)
                    {
                        if (_isHelaMdfControl)
                        {
                            lstPages.Add(new CanBus.CanDataPackage(0x6B5, CanBus.CanProtocol.Can,
                                CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8]));
                        }

                        lstPages.Add(new CanBus.CanDataPackage(0x585, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                            CanBus.CanFormat.Data, new byte[] { 0x3F, 0x0C, 0xFF, 0x7F, 0x00, 0x00, 0x02, 0x00 }));
                        //lstPages.Add(new CanBus.CanDataPackage(0x65A, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00 }));
                        lstPages.Add(new CanBus.CanDataPackage(0x6B2, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                            CanBus.CanFormat.Data, new byte[8]));
                    }

                    if (_isHelaMdfControl)
                    {
                        lstPages.Add(new CanBus.CanDataPackage(0x12DD547A, CanBus.CanProtocol.Can,
                            CanBus.CanType.Extended, CanBus.CanFormat.Data, new byte[8]));
                        lstPages.Add(new CanBus.CanDataPackage(0x1B000010, CanBus.CanProtocol.Can,
                            CanBus.CanType.Extended, CanBus.CanFormat.Data, new byte[8]));
                        lstPages.Add(new CanBus.CanDataPackage(0x1B000064, CanBus.CanProtocol.Can,
                            CanBus.CanType.Extended, CanBus.CanFormat.Data, new byte[8]));
                    }

                    return lstPages.ToArray();
                }),
                Interval = 1000
            });

            SchedulerAsync();
        }

        private Action SendTask(Func<CanBus.CanDataPackage[]> msgFunc)
        {
            return () =>
            {
                if (Can == null || msgFunc == null || _isSleep)
                    return;

                lock (_canSendLock)
                    Can.SendCanDatas(msgFunc.Invoke());
            };
        }

        private byte GetCheckSum(
            byte bz, IReadOnlyList<byte> values, IReadOnlyList<byte> table)
        {
            byte checkSum = 0xff;
            for (var i = 1; i < values.Count; i++)
            {
                var lookUpCrc = _crc8LookupTable[checkSum ^ values[i]];
                var lo = (byte)(lookUpCrc & 0xFF); // 取低8位
                //var loOr = (byte)(lo ^ 0xFF); // 低8位再取反
                checkSum = lo;
            }

            checkSum ^= table[bz];
            checkSum = (byte)(_crc8LookupTable[checkSum] & 0xff);
            checkSum ^= 0xff;

            return checkSum;
        }

        private void LUMI_LED()
        {
            // L Group0
            _motorolaMatrix0X160.UpdateData(new MatrixValDefinition(0, 6, EnvLlpMxbL1));
            _motorolaMatrix0X160.UpdateData(new MatrixValDefinition(6, 6, EnvLlpMxbL2));
            _motorolaMatrix0X160.UpdateData(new MatrixValDefinition(12, 6, EnvLlpMxbL3));
            _motorolaMatrix0X160.UpdateData(new MatrixValDefinition(18, 6, EnvLlpMxbL4));
            _motorolaMatrix0X160.UpdateData(new MatrixValDefinition(24, 6, EnvLlpMxbL5));
            _motorolaMatrix0X160.UpdateData(new MatrixValDefinition(30, 6, EnvLlpMxbL6));
            _motorolaMatrix0X160.UpdateData(new MatrixValDefinition(36, 6, EnvLlpMxbL7));
            _motorolaMatrix0X160.UpdateData(new MatrixValDefinition(42, 6, EnvLlpMxbL8));
            _motorolaMatrix0X160.UpdateData(new MatrixValDefinition(48, 6, EnvLlpMxbL9));
            _motorolaMatrix0X160.UpdateData(new MatrixValDefinition(54, 6, EnvLlpMxbL10));

            // L Group1
            _motorolaMatrix0X161.UpdateData(new MatrixValDefinition(0, 6, EnvLlpMxbL11));
            _motorolaMatrix0X161.UpdateData(new MatrixValDefinition(6, 6, EnvLlpMxbL12));
            _motorolaMatrix0X161.UpdateData(new MatrixValDefinition(12, 6, EnvLlpMxbL13));
            _motorolaMatrix0X161.UpdateData(new MatrixValDefinition(18, 6, EnvLlpMxbL14));
            _motorolaMatrix0X161.UpdateData(new MatrixValDefinition(24, 6, EnvLlpMxbL15));
            _motorolaMatrix0X161.UpdateData(new MatrixValDefinition(30, 6, EnvLlpMxbL16));
            _motorolaMatrix0X161.UpdateData(new MatrixValDefinition(36, 6, EnvLlpMxbL17));
            _motorolaMatrix0X161.UpdateData(new MatrixValDefinition(42, 6, EnvLlpMxbL18));
            _motorolaMatrix0X161.UpdateData(new MatrixValDefinition(48, 6, EnvLlpMxbL19));
            _motorolaMatrix0X161.UpdateData(new MatrixValDefinition(54, 6, EnvLlpMxbL20));

            // L Group2
            _motorolaMatrix0X162.UpdateData(new MatrixValDefinition(0, 6, EnvLlpMxbL21));
            _motorolaMatrix0X162.UpdateData(new MatrixValDefinition(6, 6, EnvLlpMxbL22));
            _motorolaMatrix0X162.UpdateData(new MatrixValDefinition(12, 6, EnvLlpMxbL23));
            _motorolaMatrix0X162.UpdateData(new MatrixValDefinition(18, 6, EnvLlpMxbL24));
            _motorolaMatrix0X162.UpdateData(new MatrixValDefinition(24, 6, EnvLlpMxbL25));
            _motorolaMatrix0X162.UpdateData(new MatrixValDefinition(30, 6, EnvLlpMxbL26));
            _motorolaMatrix0X162.UpdateData(new MatrixValDefinition(36, 6, EnvLlpMxbL27));
            _motorolaMatrix0X162.UpdateData(new MatrixValDefinition(42, 6, EnvLlpMxbL28));
            _motorolaMatrix0X162.UpdateData(new MatrixValDefinition(48, 6, EnvLlpMxbL29));
            _motorolaMatrix0X162.UpdateData(new MatrixValDefinition(54, 6, EnvLlpMxbL30));

            // L Group3
            _motorolaMatrix0X163.UpdateData(new MatrixValDefinition(0, 6, EnvLlpMxbL31));
            _motorolaMatrix0X163.UpdateData(new MatrixValDefinition(6, 6, EnvLlpMxbL32));

            // R Group0
            _motorolaMatrix0X169.UpdateData(new MatrixValDefinition(0, 6, EnvLlpMxbR1));
            _motorolaMatrix0X169.UpdateData(new MatrixValDefinition(6, 6, EnvLlpMxbR2));
            _motorolaMatrix0X169.UpdateData(new MatrixValDefinition(12, 6, EnvLlpMxbR3));
            _motorolaMatrix0X169.UpdateData(new MatrixValDefinition(18, 6, EnvLlpMxbR4));
            _motorolaMatrix0X169.UpdateData(new MatrixValDefinition(24, 6, EnvLlpMxbR5));
            _motorolaMatrix0X169.UpdateData(new MatrixValDefinition(30, 6, EnvLlpMxbR6));
            _motorolaMatrix0X169.UpdateData(new MatrixValDefinition(36, 6, EnvLlpMxbR7));
            _motorolaMatrix0X169.UpdateData(new MatrixValDefinition(42, 6, EnvLlpMxbR8));
            _motorolaMatrix0X169.UpdateData(new MatrixValDefinition(48, 6, EnvLlpMxbR9));
            _motorolaMatrix0X169.UpdateData(new MatrixValDefinition(54, 6, EnvLlpMxbR10));

            // R Group1
            _motorolaMatrix0X16A.UpdateData(new MatrixValDefinition(0, 6, EnvLlpMxbR11));
            _motorolaMatrix0X16A.UpdateData(new MatrixValDefinition(6, 6, EnvLlpMxbR12));
            _motorolaMatrix0X16A.UpdateData(new MatrixValDefinition(12, 6, EnvLlpMxbR13));
            _motorolaMatrix0X16A.UpdateData(new MatrixValDefinition(18, 6, EnvLlpMxbR14));
            _motorolaMatrix0X16A.UpdateData(new MatrixValDefinition(24, 6, EnvLlpMxbR15));
            _motorolaMatrix0X16A.UpdateData(new MatrixValDefinition(30, 6, EnvLlpMxbR16));
            _motorolaMatrix0X16A.UpdateData(new MatrixValDefinition(36, 6, EnvLlpMxbR17));
            _motorolaMatrix0X16A.UpdateData(new MatrixValDefinition(42, 6, EnvLlpMxbR18));
            _motorolaMatrix0X16A.UpdateData(new MatrixValDefinition(48, 6, EnvLlpMxbR19));
            _motorolaMatrix0X16A.UpdateData(new MatrixValDefinition(54, 6, EnvLlpMxbR20));

            // R Group2
            _motorolaMatrix0X16B.UpdateData(new MatrixValDefinition(0, 6, EnvLlpMxbR21));
            _motorolaMatrix0X16B.UpdateData(new MatrixValDefinition(6, 6, EnvLlpMxbR22));
            _motorolaMatrix0X16B.UpdateData(new MatrixValDefinition(12, 6, EnvLlpMxbR23));
            _motorolaMatrix0X16B.UpdateData(new MatrixValDefinition(18, 6, EnvLlpMxbR24));
            _motorolaMatrix0X16B.UpdateData(new MatrixValDefinition(24, 6, EnvLlpMxbR25));
            _motorolaMatrix0X16B.UpdateData(new MatrixValDefinition(30, 6, EnvLlpMxbR26));
            _motorolaMatrix0X16B.UpdateData(new MatrixValDefinition(36, 6, EnvLlpMxbR27));
            _motorolaMatrix0X16B.UpdateData(new MatrixValDefinition(42, 6, EnvLlpMxbR28));
            _motorolaMatrix0X16B.UpdateData(new MatrixValDefinition(48, 6, EnvLlpMxbR29));
            _motorolaMatrix0X16B.UpdateData(new MatrixValDefinition(54, 6, EnvLlpMxbR30));

            // R Group3
            _motorolaMatrix0X16C.UpdateData(new MatrixValDefinition(0, 6, EnvLlpMxbR31));
            _motorolaMatrix0X16C.UpdateData(new MatrixValDefinition(6, 6, EnvLlpMxbR32));
        }

        private CanBus.CanDataPackage Set15F()
        {
            var ledLAbblendlAnf = new MatrixValDefinition(12, 1, _llpLLowBeam ? (byte)0x01 : (byte)0x00);
            var ledRAbblendlAnf = new MatrixValDefinition(14, 1, _llpLLowBeam ? (byte)0x01 : (byte)0x00);
            var bz = new MatrixValDefinition(8, 4, _bz0X15F);

            _motorolaMatrix0X15F.UpdateData(ledLAbblendlAnf);
            _motorolaMatrix0X15F.UpdateData(ledRAbblendlAnf);
            _motorolaMatrix0X15F.UpdateData(bz);

            var checkSum = (byte)0xFF;
            for (var i = 1; i < 8; i++)
                checkSum = (byte)(_crc8LookupTable[checkSum ^ _motorolaMatrix0X15F.MatrixData[i]] & 0xFF);

            checkSum ^= _bzTable15F[_bz0X15F];
            checkSum = (byte)(_crc8LookupTable[checkSum] & 0xff);

            checkSum ^= 0xFF;

            _motorolaMatrix0X15F.MatrixData[0] = checkSum;
            _bz0X15F++;
            if (_bz0X15F >= 16)
                _bz0X15F = 0;

            return new CanBus.CanDataPackage(_motorolaMatrix0X15F.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                CanBus.CanFormat.Data, _motorolaMatrix0X15F.MatrixData);
        }

        private CanBus.CanDataPackage Set3C0()
        {
            var bz = new MatrixValDefinition(8, 4, (byte)_bz0X3C0);
            _motorolaMatrix0X3C0.UpdateData(bz);
            _motorolaMatrix0X3C0.MatrixData[2] = Kl15 ? (byte)0xFF : (byte)0x00;

            var checkSum = (byte)0xFF;
            for (var i = 1; i < 4; i++)
                checkSum = (byte)(_crc8LookupTable[checkSum ^ _motorolaMatrix0X3C0.MatrixData[i]] & 0xFF);

            checkSum ^= _klemmenTable[_bz0X3C0];
            checkSum = (byte)(_crc8LookupTable[checkSum] & 0xff);
            //check_sum = (CRC8_Lookup_Table[check_sum ^ Klemmen_Table[bz]]) & 0xFF;
            checkSum ^= 0xFF;

            _motorolaMatrix0X3C0.MatrixData[0] = checkSum;

            //_motorolaMatrix0X3C0.MatrixData = _list0X3C0[_bz0X3C0];
            _bz0X3C0++;
            if (_bz0X3C0 >= 16)
                _bz0X3C0 = 0;
            return new CanBus.CanDataPackage(
                _motorolaMatrix0X3C0.CanId,
                CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                CanBus.CanFormat.Data,
                _motorolaMatrix0X3C0.MatrixData);
        }

        [Description("唤醒")]
        public void ModuleAwake()
        {
            _isSleep = false;
        }

        [Description("休眠")]
        public void ModuleSleep()
        {
            _isSleep = true;
        }

        #region 灯控

        private bool _llpLLowBeam;
        private bool _isLeftLamp;

        [Description("LLP_L_LowBeamOn")]
        public void LLP_L_LowBeamOn()
        {
            _llpLLowBeam = true;
        }

        [Description("LLP_L_LowBeamOff")]
        public void LLP_L_LowBeamOff()
        {
            _llpLLowBeam = false;
        }

        [Description("HB单独控制")]
        public void SingleHbControl(string index, string pwm)
        {
            lock (_mxbLocker)
            {
                int ledIndex;
                byte ledPwm;

                if (int.TryParse(index, out ledIndex) && byte.TryParse(pwm, out ledPwm))
                {
                    if (ledIndex >= 1 && ledIndex <= 32 && ledPwm <= 0x3F)
                    {
                        var fieldL = GetType().GetField(string.Format("EnvLlpMxbL{0}", ledIndex),
                            BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                        if (fieldL != null)
                            fieldL.SetValue(this, ledPwm);

                        var fieldR = GetType().GetField(string.Format("EnvLlpMxbR{0}", ledIndex),
                            BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                        if (fieldR != null)
                            fieldR.SetValue(this, ledPwm);
                    }
                }
            }
        }

        [Description("远光单数点亮")]
        public void HbOddOn(string pwm)
        {
            lock (_mxbLocker)
            {
                byte ledPwm;
                if (!byte.TryParse(pwm, out ledPwm))
                    return;
                if (ledPwm <= 0x3F)
                {
                    MxbHighBeamOff();
                    for (var i = 1; i <= 32; i++)
                    {
                        if (i % 2 != 0)
                        {
                            var fieldL = GetType().GetField(string.Format("EnvLlpMxbL{0}", i),
                                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                            if (fieldL != null)
                                fieldL.SetValue(this, ledPwm);

                            var fieldR = GetType().GetField(string.Format("EnvLlpMxbR{0}", i),
                                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                            if (fieldR != null)
                                fieldR.SetValue(this, ledPwm);
                        }
                    }
                }
            }
        }

        [Description("远光双数点亮")]
        public void HbEvenOn(string pwm)
        {
            lock (_mxbLocker)
            {
                byte ledPwm;
                if (!byte.TryParse(pwm, out ledPwm))
                    return;
                if (ledPwm <= 0x3F)
                {
                    MxbHighBeamOff();
                    for (var i = 1; i <= 32; i++)
                    {
                        if (i % 2 == 0)
                        {
                            var fieldL = GetType().GetField(string.Format("EnvLlpMxbL{0}", i),
                                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                            if (fieldL != null)
                                fieldL.SetValue(this, ledPwm);

                            var fieldR = GetType().GetField(string.Format("EnvLlpMxbR{0}", i),
                                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                            if (fieldR != null)
                                fieldR.SetValue(this, ledPwm);
                        }
                    }
                }
            }
        }

        [Description("远光全部点亮")]
        public void HbAllOn(string pwm)
        {
            lock (_mxbLocker)
            {
                byte ledPwm;
                if (!byte.TryParse(pwm, out ledPwm))
                    return;
                if (ledPwm <= 0x3F)
                {
                    MxbHighBeamOff();
                    for (var i = 1; i <= 32; i++)
                    {
                        var fieldL = GetType()
                            .GetField(string.Format("EnvLlpMxbL{0}", i),
                                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                        if (fieldL != null)
                            fieldL.SetValue(this, ledPwm);

                        var fieldR = GetType()
                            .GetField(string.Format("EnvLlpMxbR{0}", i),
                                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                        if (fieldR != null)
                            fieldR.SetValue(this, ledPwm);
                    }
                }
            }
        }

        [Description("MxbHighBeamOn")]
        public void MxbHighBeamOn()
        {
            lock (_mxbLocker)
            {
                for (var i = 1; i <= 32; i++)
                {
                    var field = GetType().GetField(string.Format("EnvLlpMxbL{0}", i),
                        BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    if (field == null)
                        continue;
                    var b = (byte)_ledCurrent[i];
                    field.SetValue(this, b);
                }

                for (var i = 33; i <= 64; i++)
                {
                    var field = GetType().GetField(string.Format("EnvLlpMxbR{0}", i - 32),
                        BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    if (field == null)
                        continue;
                    var b = (byte)_ledCurrent[i];
                    field.SetValue(this, b);
                }
            }
        }

        [Description("ADB-Line-1")]
        public void AddLine1()
        {
            lock (_mxbLocker)
            {
                MxbHighBeamOn();
                EnvLlpMxbR2 = 0x00;
                EnvLlpMxbR3 = 0x00;
                EnvLlpMxbR4 = 0x00;
                EnvLlpMxbR5 = 0x00;
                EnvLlpMxbL5 = 0x00;
                EnvLlpMxbL6 = 0x00;
                EnvLlpMxbL7 = 0x00;
                EnvLlpMxbL8 = 0x00;
            }
        }

        [Description("ADB-Line-2")]
        public void AddLine2()
        {
            lock (_mxbLocker)
            {
                MxbHighBeamOn();
                EnvLlpMxbR3 = 0x00;
                EnvLlpMxbR4 = 0x00;
                EnvLlpMxbR5 = 0x00;
                EnvLlpMxbL5 = 0x00;
                EnvLlpMxbL6 = 0x00;
                EnvLlpMxbL7 = 0x00;
            }
        }

        [Description("ADB-Line-3")]
        public void AddLine3()
        {
            lock (_mxbLocker)
            {
                MxbHighBeamOn();
                EnvLlpMxbR3 = 0x00;
                EnvLlpMxbR4 = 0x00;
                EnvLlpMxbL5 = 0x00;
                EnvLlpMxbL6 = 0x00;
            }
        }

        [Description("ADB-Line-4")]
        public void AddLine4()
        {
            lock (_mxbLocker)
            {
                MxbHighBeamOn();
                EnvLlpMxbR3 = 0x00;
                EnvLlpMxbR4 = 0x00;
                EnvLlpMxbR5 = 0x00;
                EnvLlpMxbR6 = 0x00;
                EnvLlpMxbR7 = 0x00;
                EnvLlpMxbL3 = 0x00;
                EnvLlpMxbL4 = 0x00;
                EnvLlpMxbL5 = 0x00;
                EnvLlpMxbL6 = 0x00;
                EnvLlpMxbL7 = 0x00;
            }
        }

        [Description("ADB-Line-5")]
        public void AddLine5()
        {
            lock (_mxbLocker)
            {
                MxbHighBeamOn();
                EnvLlpMxbR4 = 0x00;
                EnvLlpMxbR5 = 0x00;
                EnvLlpMxbR6 = 0x00;
                EnvLlpMxbL3 = 0x00;
                EnvLlpMxbL4 = 0x00;
                EnvLlpMxbL5 = 0x00;
                EnvLlpMxbL6 = 0x00;
            }
        }

        [Description("ADB-Line-6")]
        public void AddLine6()
        {
            lock (_mxbLocker)
            {
                MxbHighBeamOn();
                EnvLlpMxbR4 = 0x00;
                EnvLlpMxbR5 = 0x00;
                EnvLlpMxbR6 = 0x00;
                EnvLlpMxbL3 = 0x00;
                EnvLlpMxbL4 = 0x00;
                EnvLlpMxbL5 = 0x00;
                EnvLlpMxbL6 = 0x00;
            }
        }

        [Description("Class-C")]
        public void ClassC()
        {
            lock (_mxbLocker)
            {
                MxbHighBeamOff();

                EnvLlpMxbL1 = (byte)41;
                EnvLlpMxbL2 = (byte)45;
                EnvLlpMxbL3 = (byte)46.8;
                EnvLlpMxbL4 = (byte)48.3;
                EnvLlpMxbL5 = (byte)52;
                EnvLlpMxbL6 = (byte)61;

                EnvLlpMxbR13 = (byte)41;
                EnvLlpMxbR12 = (byte)45;
                EnvLlpMxbR11 = (byte)46.8;
                EnvLlpMxbR10 = (byte)48.3;
                EnvLlpMxbR9 = (byte)52;
                EnvLlpMxbR8 = (byte)61;
            }
        }

        [Description("Class-E")]
        public void ClassE()
        {
            lock (_mxbLocker)
            {
                MxbHighBeamOff();

                EnvLlpMxbL1 = (byte)41;
                EnvLlpMxbL2 = (byte)45;
                EnvLlpMxbL3 = (byte)46.8;
                EnvLlpMxbL4 = (byte)48.3;
                EnvLlpMxbL5 = (byte)52;
                EnvLlpMxbL6 = (byte)61;

                EnvLlpMxbR13 = (byte)41;
                EnvLlpMxbR12 = (byte)45;
                EnvLlpMxbR11 = (byte)46.8;
                EnvLlpMxbR10 = (byte)48.3;
                EnvLlpMxbR9 = (byte)52;
                EnvLlpMxbR8 = (byte)61;
            }
        }

        [Description("ALL-ON-HB")]
        public void AllOnHb()
        {
            lock (_mxbLocker)
            {
                MxbHighBeamOff();

                for (var i = 17; i <= 32; i++)
                {
                    var field = GetType().GetField(string.Format("EnvLlpMxbL{0}", i),
                        BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    if (field == null)
                        continue;
                    var b = (byte)_ledCurrent[i];
                    field.SetValue(this, b);
                }
            }
        }

        [Description("ALL-ON-LB")]
        public void AllOnLb()
        {
            lock (_mxbLocker)
            {
                MxbHighBeamOff();

                for (var i = 1; i <= 16; i++)
                {
                    var field = GetType().GetField(string.Format("EnvLlpMxbL{0}", i),
                        BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    if (field == null)
                        continue;
                    var b = (byte)_ledCurrent[i];
                    field.SetValue(this, b);
                }
            }
        }

        [Description("MxbHighBeamOff")]
        public void MxbHighBeamOff()
        {
            for (var i = 1; i <= 32; i++)
            {
                var field = GetType().GetField(string.Format("EnvLlpMxbL{0}", i),
                    BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                if (field == null)
                    continue;
                const byte b = 0x00;
                field.SetValue(this, b);
            }

            for (var i = 33; i <= 64; i++)
            {
                var field = GetType().GetField(string.Format("EnvLlpMxbR{0}", i - 32),
                    BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                if (field == null)
                    continue;
                const byte b = 0x00;
                field.SetValue(this, b);
            }
        }

        [Description("近光ON")]
        public void LowBeamOn()
        {
            _motorolaMatrix0X194.UpdateData(_lampControlList[LampControl.LeftLowBeamOn]);
            _motorolaMatrix0X194.UpdateData(_lampControlList[LampControl.RightLowBeamOn]);
            //_motorolaMatrix0X194.UpdateData(_lampControlList[LampControl.LeftAllWeatherOn]);
            //_motorolaMatrix0X194.UpdateData(_lampControlList[LampControl.RightAllWeatherOn]);

            _motorolaMatrix0X3D5.UpdateData(_lampControlList[LampControl.LowBeamOn]);
        }

        [Description("近光OFF")]
        public void LowBeamOff()
        {
            _motorolaMatrix0X194.UpdateData(_lampControlList[LampControl.LeftLowBeamOff]);
            _motorolaMatrix0X194.UpdateData(_lampControlList[LampControl.RightLowBeamOff]);
            //_motorolaMatrix0X194.UpdateData(_lampControlList[LampControl.LeftAllWeatherOff]);
            //_motorolaMatrix0X194.UpdateData(_lampControlList[LampControl.RightAllWeatherOff]);

            _motorolaMatrix0X3D5.UpdateData(_lampControlList[LampControl.LowBeamOff]);
        }

        [Description("角灯ON")]
        public void CornerOn()
        {
            _motorolaMatrix0X195.UpdateData(_lampControlList[LampControl.LeftCornerOn]);
            _motorolaMatrix0X195.UpdateData(_lampControlList[LampControl.RightCornerOn]);
        }

        [Description("角灯OFF")]
        public void CornerOff()
        {
            _motorolaMatrix0X195.UpdateData(_lampControlList[LampControl.LeftCornerOff]);
            _motorolaMatrix0X195.UpdateData(_lampControlList[LampControl.RightCornerOff]);
        }

        [Description("Left角灯ON")]
        public void LeftCornerOn()
        {
            _motorolaMatrix0X195.UpdateData(_lampControlList[LampControl.LeftCornerOn]);
        }

        [Description("Left角灯OFF")]
        public void LeftCornerOff()
        {
            _motorolaMatrix0X195.UpdateData(_lampControlList[LampControl.LeftCornerOff]);
        }

        [Description("Right角灯ON")]
        public void RightCornerOn()
        {
            _motorolaMatrix0X195.UpdateData(_lampControlList[LampControl.RightCornerOn]);
        }

        [Description("Right角灯OFF")]
        public void RightCornerOff()
        {
            _motorolaMatrix0X195.UpdateData(_lampControlList[LampControl.RightCornerOff]);
        }

        [Description("远光ON")]
        public void HighBeamOn()
        {
            _motorolaMatrix0X194.UpdateData(_lampControlList[LampControl.LeftHighBeamOn]);
            _motorolaMatrix0X194.UpdateData(_lampControlList[LampControl.RightHighBeamOn]);
        }

        [Description("远光OFF")]
        public void HighBeamOff()
        {
            _motorolaMatrix0X194.UpdateData(_lampControlList[LampControl.LeftHighBeamOff]);
            _motorolaMatrix0X194.UpdateData(_lampControlList[LampControl.RightHighBeamOff]);
        }

        [Description("Left远光ON")]
        public void LeftHighBeamOn()
        {
            _motorolaMatrix0X194.UpdateData(_lampControlList[LampControl.LeftHighBeamOn]);
        }

        [Description("Left远光OFF")]
        public void LeftHighBeamOff()
        {
            _motorolaMatrix0X194.UpdateData(_lampControlList[LampControl.LeftHighBeamOff]);
        }

        [Description("Right远光ON")]
        public void RightHighBeamOn()
        {
            _motorolaMatrix0X194.UpdateData(_lampControlList[LampControl.RightHighBeamOn]);
        }

        [Description("Right远光OFF")]
        public void RightHighBeamOff()
        {
            _motorolaMatrix0X194.UpdateData(_lampControlList[LampControl.RightHighBeamOff]);
        }

        [Description("日行灯ON")]
        public void DrlOn()
        {
            _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftDrlOn]);
            _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightDrlOn]);

            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftPlOff]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightPlOff]);

            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftTurnOff]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightTurnOff]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftTurnLoopOff]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightTurnLoopOff]);
        }

        [Description("日行灯OFF")]
        public void DrlOff()
        {
            _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftDrlOff]);
            _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightDrlOff]);

            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftPlOff]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightPlOff]);

            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftTurnOff]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightTurnOff]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftTurnLoopOff]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightTurnLoopOff]);
        }

        [Description("Left日行灯ON")]
        public void LeftDrlOn()
        {
            _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftDrlOn]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftPlOff]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftTurnOff]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftTurnLoopOff]);
        }

        [Description("Left日行灯OFF")]
        public void LeftDrlOff()
        {
            _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftDrlOff]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftPlOff]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftTurnOff]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftTurnLoopOff]);
        }

        [Description("Right日行灯ON")]
        public void RightDrlOn()
        {
            _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightDrlOn]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightPlOff]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightTurnOff]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightTurnLoopOff]);
        }

        [Description("Right日行灯OFF")]
        public void RightDrlOff()
        {
            _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightDrlOff]);
            _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightPlOff]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightTurnOff]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightTurnLoopOff]);
        }

        [Description("位置灯ON")]
        public void PlOn()
        {
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftDrlOff]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightDrlOff]);

            _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftPlOn]);
            _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightPlOn]);

            //_motorolaMatrix0X193.UpdateData(new MatrixValDefinition(30, 1, 1));
            //_motorolaMatrix0X193.UpdateData(new MatrixValDefinition(31, 1, 1));

            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftTurnOff]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightTurnOff]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftTurnLoopOff]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightTurnLoopOff]);
        }

        [Description("位置灯OFF")]
        public void PlOff()
        {
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftDrlOff]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightDrlOff]);

            _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftPlOff]);
            _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightPlOff]);

            //_motorolaMatrix0X193.UpdateData(new MatrixValDefinition(30, 1, 0));
            //_motorolaMatrix0X193.UpdateData(new MatrixValDefinition(31, 1, 0));

            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftTurnOff]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightTurnOff]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftTurnLoopOff]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightTurnLoopOff]);
        }

        [Description("Left位置灯ON")]
        public void LeftPlOn()
        {
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftDrlOff]);
            _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftPlOn]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftTurnOff]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftTurnLoopOff]);
        }

        [Description("Left位置灯OFF")]
        public void LeftPlOff()
        {
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftDrlOff]);
            _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftPlOff]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftTurnOff]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftTurnLoopOff]);
        }

        [Description("Right位置灯ON")]
        public void RightPlOn()
        {
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightDrlOff]);
            _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightPlOn]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightTurnOff]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightTurnLoopOff]);
        }

        [Description("Right位置灯OFF")]
        public void RightPlOff()
        {
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightDrlOff]);
            _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightPlOff]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightTurnOff]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightTurnLoopOff]);
        }

        [Description("驻车灯ON")]
        public void ParkOn()
        {
            _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftParkOn]);
            _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightParkOn]);
        }

        [Description("驻车灯OFF")]
        public void ParkOff()
        {
            _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftParkOff]);
            _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightParkOff]);
        }

        [Description("Left驻车灯ON")]
        public void LeftParkOn()
        {
            _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftParkOn]);
        }

        [Description("Left驻车灯OFF")]
        public void LeftParkOff()
        {
            _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftParkOff]);
        }

        [Description("Right驻车灯ON")]
        public void RightParkOn()
        {
            _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightParkOn]);
        }

        [Description("Right驻车灯OFF")]
        public void RightParkOff()
        {
            _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightParkOff]);
        }

        [Description("转向灯流水信号ON")]
        public void LCM_Blinkerwischen_Anf_On()
        {
            _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LCM_Blinkerwischen_Anf_On]);
        }

        [Description("转向灯流水信号OFF")]
        public void LCM_Blinkerwischen_Anf_Off()
        {
            _motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LCM_Blinkerwischen_Anf_Off]);
        }

        [Description("转向灯ON")]
        public void TurnOn()
        {
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftTurnOn]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightTurnOn]);

            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftTurnLoopOn]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightTurnLoopOn]);

            _isTurnLOn = true;
            _isTurnROn = true;
        }

        [Description("转向灯OFF")]
        public void TurnOff()
        {
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftTurnOff]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightTurnOff]);

            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftTurnLoopOff]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightTurnLoopOff]);

            _isTurnLOn = false;
            _isTurnROn = false;
        }

        [Description("Left转向灯ON")]
        public void LeftTurnOn()
        {
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftTurnOn]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftTurnLoopOn]);

            _isTurnLOn = true;
        }

        [Description("Left转向灯OFF")]
        public void LeftTurnOff()
        {
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftTurnOff]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.LeftTurnLoopOff]);

            _isTurnLOn = false;
        }

        [Description("Right转向灯ON")]
        public void RightTurnOn()
        {
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightTurnOn]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightTurnLoopOn]);

            _isTurnROn = true;
        }

        [Description("Right转向灯OFF")]
        public void RightTurnOff()
        {
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightTurnOff]);
            //_motorolaMatrix0X193.UpdateData(_lampControlList[LampControl.RightTurnLoopOff]);

            _isTurnROn = false;
        }

        [Description("Dl2ON")]
        public void Dl2On()
        {
            _motorolaMatrix0X193.UpdateData(new MatrixValDefinition(36, 1, 1));

            //_motorolaMatrix0X193.UpdateData(new MatrixValDefinition(24, 1, 1));
            //_motorolaMatrix0X193.UpdateData(new MatrixValDefinition(28, 1, 1));
            //_motorolaMatrix0X193.UpdateData(new MatrixValDefinition(30, 1, 1));
            //_motorolaMatrix0X193.UpdateData(new MatrixValDefinition(32, 1, 1));
            //_motorolaMatrix0X193.UpdateData(new MatrixValDefinition(35, 1, 1));
            //_motorolaMatrix0X193.UpdateData(new MatrixValDefinition(39, 1, 1));
        }

        [Description("Dl2OFF")]
        public void Dl2Off()
        {
            _motorolaMatrix0X193.UpdateData(new MatrixValDefinition(36, 1, 0));
        }

        [Description("LeavingHomeActive")]
        public void LeavingHomeActive()
        {
            var bcm1LhAktiv = new MatrixValDefinition(31, 1, 1);
            var bcm1ChLhAktiv = new MatrixValDefinition(40, 1, 1);

            _motorolaMatrix0X3D5.UpdateData(bcm1LhAktiv);
            _motorolaMatrix0X3D5.UpdateData(bcm1ChLhAktiv);
        }

        [Description("LeavingHomeNotActive")]
        public void LeavingHomeNotActive()
        {
            var bcm1LhAktiv = new MatrixValDefinition(31, 1, 0);
            var bcm1ChLhAktiv = new MatrixValDefinition(40, 1, 0);

            _motorolaMatrix0X3D5.UpdateData(bcm1LhAktiv);
            _motorolaMatrix0X3D5.UpdateData(bcm1ChLhAktiv);
        }

        #endregion

        #region 马达

        private bool _isMotorCycle;
        private int _motorCycleCount;
        private int _motorCycleIndex = -2;

        private bool _isMotorLReset;
        private bool _isMotorRReset;

        private double _motorLPos;
        private double _motorRPos;

        public void StartMotorCycle()
        {
            _motorCycleCount = 0;
            _isMotorCycle = true;
            _motorCycleIndex = -2;
        }

        public void EndMotorCycle()
        {
            _motorCycleCount = 0;
            _isMotorCycle = false;
            _motorCycleIndex = -2;
        }

        [Description("左马达复位On")]
        public void LeftMotorResetOn()
        {
            _isMotorLReset = true;
        }

        [Description("左马达复位On")]
        public void LeftMotorResetOff()
        {
            _isMotorLReset = false;
        }

        [Description("左马达运动到")]
        public void LeftMotorRunTo(double pos)
        {
            _motorLPos = pos;
        }

        [Description("右马达复位On")]
        public void RightMotorResetOn()
        {
            _isMotorRReset = true;
        }

        [Description("右马达复位Off")]
        public void RightMotorResetOff()
        {
            _isMotorRReset = false;
        }

        [Description("右马达运动到")]
        public void RightMotorRunTo(double pos)
        {
            _motorRPos = pos;
        }

        private static double CalMotorRealPos(byte[] data, int startBit, int bitLen)
        {
            try
            {
                var value = GetIntelValue(data, startBit, bitLen);
                return Math.Round((value * 0.5 - 1023), 2, MidpointRounding.AwayFromZero);
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        #endregion

        #region VW336售后模块版本信息读取

        private int _blinkCount;
        private bool _isBlink;
        private bool _isTurnLOn;
        private bool _isTurnROn;

        [Description("转向灯闪烁")]
        public void TurnBlinkEnable()
        {
            _blinkCount = 0;
            _isBlink = true;
        }

        [Description("转向灯常亮")]
        public void TurnBlinkDisable()
        {
            _blinkCount = 0;
            _isBlink = false;
        }

        public string LeftSwVersion = string.Empty;
        public string RightSwVersion = string.Empty;

        public void ReadLeftSwVersion()
        {
            LeftSwVersion = string.Empty;
            if (Can != null)
            {
                lock (_canSendLock)
                {
                    byte[] echo;
                    if (Can.CanBusWithUds.TryReadData(0x775, 0x675, CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xfc,
                            0xf1, out echo, timeoutFromMilliseconds: 100))
                    {
                        LeftSwVersion = echo.GetStringByAsciiBytes(false);
                    }
                }
            }
        }

        public void ReadRightSwVersion()
        {
            RightSwVersion = string.Empty;
            if (Can != null)
            {
                lock (_canSendLock)
                {
                    byte[] echo;
                    if (Can.CanBusWithUds.TryReadData(0x776, 0x676, CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xfc,
                            0xf1, out echo, timeoutFromMilliseconds: 100))
                    {
                        RightSwVersion = echo.GetStringByAsciiBytes(false);
                    }
                }
            }
        }

        private readonly object _lockErrorL = new object();
        private readonly object _lockErrorR = new object();

        private long _lastErrorL = HighPrecisionTimer.GetTimestamp();
        private long _lastErrorR = HighPrecisionTimer.GetTimestamp();

        public string DrlLState = string.Empty;
        public string DrlRState = string.Empty;

        public string PlLState = string.Empty;
        public string PlRState = string.Empty;

        public string TurnLState = string.Empty;
        public string TurnRState = string.Empty;

        public string CornerLState = string.Empty;
        public string CornerRState = string.Empty;

        public string ParkLState = string.Empty;
        public string ParkRState = string.Empty;

        public string HbLState = string.Empty;
        public string HbRState = string.Empty;

        public string LbState = string.Empty;

        #endregion

        #region DTC相关

        [Description("R,诊断-读取左灯DTC结果")]
        public string ReadDtcLResult;

        [Description("读左灯DTC")]
        public void ReadDtcL()
        {
            ReadDtcLResult = string.Empty;

            if (Can == null)
                return;

            byte[] echo;
            if (Can.CanBusWithUds.TryReadDtcInfomation(
                0x17FC0096,
                0x17FE0096,
                CanBus.CanType.Extended, 0x02, 0x09,
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

                            ReadDtcLResult += string.Format("[{0}];", dtcData.Remark);
                        }

                        if (string.IsNullOrEmpty(ReadDtcLResult))
                        {
                            ReadDtcLResult = "NoError";
                        }
                    }
                    else
                    {
                        ReadDtcLResult = "ReadDtcResLenError";
                    }
                }
            }
            else
            {
                ReadDtcLResult = "NoRead";
            }
        }

        [Description("R,清除左灯错误结果")]
        public string ClearFaultLResult;

        [Description("清除左灯错误")]
        public void ClearLFault()
        {
            ClearFaultLResult = string.Empty;

            if (Can == null)
                return;

            if (Can.CanBusWithUds.TryClearAllGroupsDiagnosticInformation(
                   0x17FC0096,
                   0x17FE0096,
                   CanBus.CanType.Extended))
            {
                ClearFaultLResult = @"OK";
            }
        }

        [Description("R,诊断-读取右灯DTC结果")]
        public string ReadDtcRResult;

        [Description("读右灯DTC")]
        public void ReadDtcR()
        {
            ReadDtcRResult = string.Empty;

            if (Can == null)
                return;

            byte[] echo;
            if (Can.CanBusWithUds.TryReadDtcInfomation(
                0x17FC0097,
                0x17FE0097,
                CanBus.CanType.Extended, 0x02, 0x09,
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

                            ReadDtcRResult += string.Format("[{0}];", dtcData.Remark);
                        }

                        if (string.IsNullOrEmpty(ReadDtcRResult))
                        {
                            ReadDtcRResult = "NoError";
                        }
                    }
                    else
                    {
                        ReadDtcRResult = "ReadDtcResLenError";
                    }
                }
            }
            else
            {
                ReadDtcRResult = "NoRead";
            }
        }

        [Description("R,清除右灯错误结果")]
        public string ClearFaultRResult;

        [Description("清除右灯错误")]
        public void ClearRFault()
        {
            ClearFaultRResult = string.Empty;

            if (Can == null)
                return;

            if (Can.CanBusWithUds.TryClearAllGroupsDiagnosticInformation(
                   0x17FC0097,
                   0x17FE0097,
                   CanBus.CanType.Extended))
            {
                ClearFaultRResult = @"OK";
            }
        }

        #endregion

        #region READ HELA DID 10 15/16 and DTC

        [Description("R,读DID 10 15")]
        public string Result1015 = string.Empty;
        [Description("R,读DID 10 16")]
        public string Result1016 = string.Empty;
        [Description("R,读DID 10 04")]
        public string Result1004 = string.Empty;

        [Description("读右灯DID 10 15/16")]
        public void ReadRDid1015AndDid1016()
        {
            Result1015 = string.Empty;
            Result1016 = string.Empty;
            Result1004 = string.Empty;

            if (Can != null)
            {
                var reqCanId = (uint)0x17FC0099;
                var recvCanId = (uint)0x17FE0099;

                byte[] echo1015;
                if (Can.CanBusWithUds.TryReadData(reqCanId, recvCanId, CanBus.CanType.Extended, CanBus.CanProtocol.Can, 0x10, 0x15, out echo1015, 0xCC))
                {
                    Result1015 = ValueHelper.GetHextStr(echo1015).Replace(" ", "");
                }

                byte[] echo1016;
                if (Can.CanBusWithUds.TryReadData(reqCanId, recvCanId, CanBus.CanType.Extended, CanBus.CanProtocol.Can, 0x10, 0x16, out echo1016, 0xCC))
                {
                    Result1016 = ValueHelper.GetHextStr(echo1016).Replace(" ", "");
                }

                byte[] echo1004;
                if (Can.CanBusWithUds.TryReadData(reqCanId, recvCanId, CanBus.CanType.Extended, CanBus.CanProtocol.Can, 0x10, 0x04, out echo1004, 0xCC))
                {
                    Result1004 = ValueHelper.GetHextStr(echo1004).Replace(" ", "");
                }
            }
        }

        [Description("读左灯DID 10 15/16")]
        public void ReadLDid1015AndDid1016()
        {
            Result1015 = string.Empty;
            Result1016 = string.Empty;
            Result1004 = string.Empty;

            if (Can != null)
            {
                var reqCanId = (uint)0x17FC0098;
                var recvCanId = (uint)0x17FE0098;

                byte[] echo1015;
                if (Can.CanBusWithUds.TryReadData(reqCanId, recvCanId, CanBus.CanType.Extended, CanBus.CanProtocol.Can, 0x10, 0x15, out echo1015, 0xCC))
                {
                    Result1015 = ValueHelper.GetHextStr(echo1015).Replace(" ", "");
                }

                byte[] echo1016;
                if (Can.CanBusWithUds.TryReadData(reqCanId, recvCanId, CanBus.CanType.Extended, CanBus.CanProtocol.Can, 0x10, 0x16, out echo1016, 0xCC))
                {
                    Result1016 = ValueHelper.GetHextStr(echo1016).Replace(" ", "");
                }

                byte[] echo1004;
                if (Can.CanBusWithUds.TryReadData(reqCanId, recvCanId, CanBus.CanType.Extended, CanBus.CanProtocol.Can, 0x10, 0x04, out echo1004, 0xCC))
                {
                    Result1004 = ValueHelper.GetHextStr(echo1004).Replace(" ", "");
                }
            }
        }

        [Description("R,诊断-读取HELA左灯DTC结果")]
        public string ReadHelaDtcLResult;

        [Description("读HELA左灯DTC")]
        public void ReadHelaDtcL()
        {
            ReadHelaDtcLResult = string.Empty;

            if (Can == null)
                return;

            byte[] echo;
            if (Can.CanBusWithUds.TryReadDtcInfomation(
                    0x17FC0098,
                    0x17FE0098,
                CanBus.CanType.Extended, 0x02, 0x09,
                out echo))
            {
                if (echo != null)
                {
                    // DTC CODE: B1DEF, (10)01110111101111
                    // CODE: 9DEF, 1001110111101111
                    // Failure Type: 0x13, dtc low byte

                    //ReadHelaDtcResult = ValueHelper.GetHextStr(echo);

                    if (echo.Length % 4 == 0)
                    {
                        for (var i = 0; i < echo.Length; i = i + 4)
                        {
                            var dtcData = new Uds14229Helper.DtcData(echo[i], echo[i + 1], echo[i + 2], echo[i + 3]);
                            //Console.WriteLine(dtcData.Remark);

                            ReadHelaDtcLResult += string.Format("[{0}];", dtcData.Remark);
                        }

                        if (string.IsNullOrEmpty(ReadHelaDtcLResult))
                        {
                            ReadHelaDtcLResult = "NoError";
                        }
                    }
                    else
                    {
                        ReadHelaDtcLResult = "ReadHelaDtcResLenError";
                    }
                }
            }
            else
            {
                ReadHelaDtcLResult = "NoRead";
            }
        }

        [Description("R,清除HELA左灯错误结果")]
        public string ClearHelaFaultLResult;

        [Description("清除HELA左灯错误")]
        public void ClearHelaLFault()
        {
            ClearHelaFaultLResult = string.Empty;

            if (Can == null)
                return;

            if (Can.CanBusWithUds.TryClearAllGroupsDiagnosticInformation(
                    0x17FC0098,
                    0x17FE0098,
                   CanBus.CanType.Extended))
            {
                ClearHelaFaultLResult = @"OK";
            }
        }

        [Description("R,诊断-读取HELA右灯DTC结果")]
        public string ReadHelaDtcRResult;

        [Description("读HELA右灯DTC")]
        public void ReadHelaDtcR()
        {
            ReadHelaDtcRResult = string.Empty;

            if (Can == null)
                return;

            byte[] echo;
            if (Can.CanBusWithUds.TryReadDtcInfomation(
                    0x17FC0099,
                    0x17FE0099,
                CanBus.CanType.Extended, 0x02, 0x09,
                out echo))
            {
                if (echo != null)
                {
                    // DTC CODE: B1DEF, (10)01110111101111
                    // CODE: 9DEF, 1001110111101111
                    // Failure Type: 0x13, dtc low byte

                    //ReadHelaDtcResult = ValueHelper.GetHextStr(echo);

                    if (echo.Length % 4 == 0)
                    {
                        for (var i = 0; i < echo.Length; i = i + 4)
                        {
                            var dtcData = new Uds14229Helper.DtcData(echo[i], echo[i + 1], echo[i + 2], echo[i + 3]);
                            //Console.WriteLine(dtcData.Remark);

                            ReadHelaDtcRResult += string.Format("[{0}];", dtcData.Remark);
                        }

                        if (string.IsNullOrEmpty(ReadHelaDtcRResult))
                        {
                            ReadHelaDtcRResult = "NoError";
                        }
                    }
                    else
                    {
                        ReadHelaDtcRResult = "ReadHelaDtcResLenError";
                    }
                }
            }
            else
            {
                ReadHelaDtcRResult = "NoRead";
            }
        }

        [Description("R,清除HELA右灯错误结果")]
        public string ClearHelaFaultRResult;

        [Description("清除HELA右灯错误")]
        public void ClearHelaRFault()
        {
            ClearHelaFaultRResult = string.Empty;

            if (Can == null)
                return;

            if (Can.CanBusWithUds.TryClearAllGroupsDiagnosticInformation(
                    0x17FC0099,
                    0x17FE0099,
                   CanBus.CanType.Extended))
            {
                ClearHelaFaultRResult = @"OK";
            }
        }

        #endregion

        #region 信号

        /// <summary>
        /// 马达LWR FE 07 00 FE 07 00 00 00
        /// </summary>
        private readonly CanCommunicationMatrix.IntelMatrix _motorolaMatrix0X190 =
            new CanCommunicationMatrix.IntelMatrix(0x190, 8);

        /// <summary>
        /// DRL/PL/TURN, bz/crc,_lcmSollBzTable
        /// </summary>
        private readonly CanCommunicationMatrix.IntelMatrix _motorolaMatrix0X193 =
            new CanCommunicationMatrix.IntelMatrix(0x193, 8);

        /// <summary>
        /// LB/HB/WEATHER
        /// </summary>
        private readonly CanCommunicationMatrix.IntelMatrix _motorolaMatrix0X194 =
            new CanCommunicationMatrix.IntelMatrix(0x194, 8);

        /// <summary>
        /// CORNER
        /// </summary> 
        private readonly CanCommunicationMatrix.IntelMatrix _motorolaMatrix0X195 =
            new CanCommunicationMatrix.IntelMatrix(0x195, 8);

        /// <summary>
        /// AFS/SOA FE 07 00 FE 07 00 E1 ??
        /// </summary>
        private readonly CanCommunicationMatrix.IntelMatrix _motorolaMatrix0X192 =
            new CanCommunicationMatrix.IntelMatrix(0x192, 8);

        /// <summary>
        /// 416 418使用此近光位 _lcmSollBzTable2
        /// </summary>
        private readonly CanCommunicationMatrix.IntelMatrix _motorolaMatrix0X3D5 =
            new CanCommunicationMatrix.IntelMatrix(0x3D5, 8);

        /// <summary>
        /// MDF周期帧 FE 07 FE 07 00 00 00 00
        /// </summary>
        private readonly CanCommunicationMatrix.IntelMatrix _motorolaMatrix0X110 =
            new CanCommunicationMatrix.IntelMatrix(0x110, 8);

        /// <summary>
        /// 周期帧 FF 0F FF 02
        /// </summary>
        private readonly CanCommunicationMatrix.IntelMatrix _motorolaMatrix0X196 =
            new CanCommunicationMatrix.IntelMatrix(0x196, 4);

        /// <summary>
        /// 周期帧 FE 0F F0 07 00 00
        /// </summary>
        private readonly CanCommunicationMatrix.IntelMatrix _motorolaMatrix0X197 =
            new CanCommunicationMatrix.IntelMatrix(0x197, 6);

        /// <summary>
        /// 周期帧 FE 0F FE 00 FE FE
        /// </summary>
        private readonly CanCommunicationMatrix.IntelMatrix _motorolaMatrix0X19A =
           new CanCommunicationMatrix.IntelMatrix(0x19A, 6);

        /// <summary>
        /// 周期帧, bz/crc _list0X0Fd
        /// </summary>
        private readonly CanCommunicationMatrix.IntelMatrix _motorolaMatrix0X0Fd =
           new CanCommunicationMatrix.IntelMatrix(0x0FD, 8);

        /// <summary>
        /// 周期帧, bz/crc, _klemmenTable
        /// </summary>
        private readonly CanCommunicationMatrix.IntelMatrix _motorolaMatrix0X3C0 =
           new CanCommunicationMatrix.IntelMatrix(0x3C0, 4);

        #region Matrix
        private readonly CanCommunicationMatrix.IntelMatrix _motorolaMatrix0X160 =
         new CanCommunicationMatrix.IntelMatrix(0x160, 8);
        private readonly CanCommunicationMatrix.IntelMatrix _motorolaMatrix0X161 =
          new CanCommunicationMatrix.IntelMatrix(0x161, 8);
        private readonly CanCommunicationMatrix.IntelMatrix _motorolaMatrix0X162 =
          new CanCommunicationMatrix.IntelMatrix(0x162, 8);
        private readonly CanCommunicationMatrix.IntelMatrix _motorolaMatrix0X163 =
          new CanCommunicationMatrix.IntelMatrix(0x163, 8);
        private readonly CanCommunicationMatrix.IntelMatrix _motorolaMatrix0X164 =
          new CanCommunicationMatrix.IntelMatrix(0x164, 8);
        private readonly CanCommunicationMatrix.IntelMatrix _motorolaMatrix0X165 =
          new CanCommunicationMatrix.IntelMatrix(0x165, 8);
        private readonly CanCommunicationMatrix.IntelMatrix _motorolaMatrix0X166 =
          new CanCommunicationMatrix.IntelMatrix(0x166, 8);
        private readonly CanCommunicationMatrix.IntelMatrix _motorolaMatrix0X167 =
          new CanCommunicationMatrix.IntelMatrix(0x167, 8);
        private readonly CanCommunicationMatrix.IntelMatrix _motorolaMatrix0X168 =
          new CanCommunicationMatrix.IntelMatrix(0x168, 8);
        private readonly CanCommunicationMatrix.IntelMatrix _motorolaMatrix0X169 =
          new CanCommunicationMatrix.IntelMatrix(0x169, 8);
        private readonly CanCommunicationMatrix.IntelMatrix _motorolaMatrix0X16A =
          new CanCommunicationMatrix.IntelMatrix(0x16A, 8);
        private readonly CanCommunicationMatrix.IntelMatrix _motorolaMatrix0X16B =
          new CanCommunicationMatrix.IntelMatrix(0x16B, 8);
        private readonly CanCommunicationMatrix.IntelMatrix _motorolaMatrix0X16C =
          new CanCommunicationMatrix.IntelMatrix(0x16C, 8);
        private readonly CanCommunicationMatrix.IntelMatrix _motorolaMatrix0X16D =
          new CanCommunicationMatrix.IntelMatrix(0x16D, 8);
        private readonly CanCommunicationMatrix.IntelMatrix _motorolaMatrix0X16E =
          new CanCommunicationMatrix.IntelMatrix(0x16E, 8);
        private readonly CanCommunicationMatrix.IntelMatrix _motorolaMatrix0X16F =
          new CanCommunicationMatrix.IntelMatrix(0x16F, 8);
        private readonly CanCommunicationMatrix.IntelMatrix _motorolaMatrix0X174 =
          new CanCommunicationMatrix.IntelMatrix(0x174, 8);
        private readonly CanCommunicationMatrix.IntelMatrix _motorolaMatrix0X175 =
          new CanCommunicationMatrix.IntelMatrix(0x175, 8);
        #endregion

        /// <summary>
        /// 周期帧, bz/crc _list0x040
        /// </summary>
        private readonly CanCommunicationMatrix.IntelMatrix _motorolaMatrix0X040 =
          new CanCommunicationMatrix.IntelMatrix(0x040, 8);

        /// <summary>
        /// 周期帧 00 00 00 00 00 00 00 00
        /// </summary>
        private readonly CanCommunicationMatrix.IntelMatrix _motorolaMatrix0X15F =
          new CanCommunicationMatrix.IntelMatrix(0x15F, 8);

        /// <summary>
        /// 周期帧 80 00 01 00 00 00 00 00
        /// </summary>
        private readonly CanCommunicationMatrix.IntelMatrix _motorolaMatrix0X54E =
          new CanCommunicationMatrix.IntelMatrix(0x54E, 8);

        /// <summary>
        /// 周期帧, bz/crc
        /// </summary>
        private readonly CanCommunicationMatrix.IntelMatrix _motorolaMatrix0X56B =
          new CanCommunicationMatrix.IntelMatrix(0x56B, 8);

        private readonly byte[] _crc8LookupTable =
        {
            0, 47, 94, 113, 188, 147, 226, 205, 87, 120, 9, 38, 235, 196, 181, 154, 174, 129, 240, 223, 18, 61, 76,
            99, 249, 214, 167, 136, 69, 106, 27, 52, 115, 92, 45, 2, 207, 224, 145, 190, 36, 11, 122, 85, 152, 183,
            198, 233, 221, 242, 131, 172, 97, 78, 63, 16, 138, 165, 212, 251, 54, 25, 104, 71, 230, 201, 184, 151,
            90, 117, 4, 43, 177, 158, 239, 192, 13, 34, 83, 124, 72, 103, 22, 57, 244, 219, 170, 133, 31, 48,
            65, 110, 163, 140, 253, 210, 149, 186, 203, 228, 41, 6, 119, 88, 194, 237, 156, 179, 126, 81, 32, 15, 59,
            20, 101, 74, 135, 168, 217, 246, 108, 67, 50, 29, 208, 255, 142, 161, 227, 204, 189, 146, 95, 112, 1, 46,
            180, 155, 234, 197, 8, 39, 86, 121, 77, 98, 19, 60, 241, 222, 175, 128, 26, 53, 68, 107, 166, 137, 248, 215,
            144, 191, 206, 225, 44, 3, 114, 93, 199, 232, 153, 182, 123, 84, 37, 10, 62, 17, 96, 79, 130, 173, 220, 243,
            105, 70, 55, 24, 213, 250, 139, 164, 5, 42, 91, 116, 185, 150, 231, 200, 82, 125, 12, 35, 238, 193, 176, 159,
            171, 132, 245, 218, 23, 56, 73, 102, 252, 211, 162, 141, 64, 111, 30, 49, 118, 89, 40, 7, 202, 229, 148, 187,
            33,
            14, 127, 80, 157, 178, 195, 236, 216, 247, 134, 169, 100, 75, 58, 21, 143, 160, 209, 254, 51, 28, 109, 66
        };

        private readonly byte[] _klemmenTable = {
                0xc3, 0xc3, 0xc3, 0xc3, 0xc3, 0xc3, 0xc3, 0xc3,
                0xc3, 0xc3, 0xc3, 0xc3, 0xc3, 0xc3, 0xc3, 0xc3
            };

        private readonly byte[] _esp21TAble = {
                0xb4, 0xef, 0xf8, 0x49, 0x1e, 0xe5, 0xc2, 0xc0,
                0x97, 0x19, 0x3c, 0xc9, 0xf1, 0x98, 0xd6, 0x61
            };

        private readonly byte[] _lcmSollBzTable = {
            0xBD, 0x44, 0x90, 0x8D, 0x74, 0xFA, 0x91, 0x89, 0x41, 0x4A, 0xBE, 0x82, 0xB3, 0x05, 0x84, 0xCF
        };
        private readonly byte[] _lcmSollBzTable2 = {
                0xc5, 0x39, 0xc7, 0xf9, 0x92, 0xdb, 0x24, 0xce,
                0xf1, 0xb5, 0x7a, 0xc4, 0xbc, 0x60, 0xe3, 0xd1
            };

        private readonly byte[] _bzTable15F =
        {

            0xd1, 0x9f, 0x19, 0x78, 0x95, 0x7c, 0x48, 0xcf,
            0xea, 0x34, 0x37, 0xe2, 0xc1, 0x52, 0xd6, 0x3d
        };

        #region BZ
        private int _bz0X0Fd;
        private readonly List<byte[]> _list0X0Fd = new List<byte[]>
        {
            new byte[] { 0xA3, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
            new byte[] { 0xA9, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
            new byte[] { 0x66, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
            new byte[] { 0x13, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
            new byte[] { 0x85, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
            new byte[] { 0x1F, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
            new byte[] { 0x0D, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
            new byte[] { 0x37, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
            new byte[] { 0xF0, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
            new byte[] { 0xC2, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
            new byte[] { 0x8E, 0x0a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
            new byte[] { 0xA1, 0x0b, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
            new byte[] { 0x38, 0x0c, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
            new byte[] { 0xB1, 0x0d, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
            new byte[] { 0x4E, 0x0e, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
            new byte[] { 0xD9, 0x0f, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
        };

        private int _bz0X040;
        private readonly List<byte[]> _list0X040 = new List<byte[]>
        {
            new byte[] {0xC7, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00},
            new byte[] {0xA3, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00},
            new byte[] {0x0F, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00},
            new byte[] {0x6B, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00},
            new byte[] {0x78, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00},
            new byte[] {0x1C, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00},
            new byte[] {0xB0, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00},
            new byte[] {0xD4, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00},
            new byte[] {0x96, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00},
            new byte[] {0xF2, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00},
            new byte[] {0x5E, 0x0a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00},
            new byte[] {0x3A, 0x0b, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00},
            new byte[] {0x29, 0x0c, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00},
            new byte[] {0x4D, 0x0d, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00},
            new byte[] {0xE1, 0x0e, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00},
            new byte[] {0x85, 0x0f, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00}
        };

        private int _bz0X3C0;
        private readonly List<byte[]> _list0X3C0 = new List<byte[]>
        {
            new byte[] { 0x0F, 0x00, 0xFF, 0x00 },
            new byte[] { 0xBA, 0x01, 0xFF, 0x00 },
            new byte[] { 0x4A, 0x02, 0xFF, 0x00 },
            new byte[] { 0xFF, 0x03, 0xFF, 0x00 },
            new byte[] { 0x85, 0x04, 0xFF, 0x00 },
            new byte[] { 0x30, 0x05, 0xFF, 0x00 },
            new byte[] { 0xC0, 0x06, 0xFF, 0x00 },
            new byte[] { 0x75, 0x07, 0xFF, 0x00 },
            new byte[] { 0x34, 0x08, 0xFF, 0x00 },
            new byte[] { 0x81, 0x09, 0xFF, 0x00 },
            new byte[] { 0x71, 0x0a, 0xFF, 0x00 },
            new byte[] { 0xC4, 0x0b, 0xFF, 0x00 },
            new byte[] { 0xBE, 0x0c, 0xFF, 0x00 },
            new byte[] { 0x0B, 0x0d, 0xFF, 0x00 },
            new byte[] { 0xFB, 0x0e, 0xFF, 0x00 },
            new byte[] { 0x4E, 0x0f, 0xFF, 0x00 }
        };

        private int _bz0X3Be;
        private readonly List<byte[]> _list0X3Be = new List<byte[]>
        {
            //new byte[] {0x21, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00},
            //new byte[] {0x45, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00},
            //new byte[] {0xE9, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00},
            //new byte[] {0x8D, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00},
            //new byte[] {0x9E, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00},
            //new byte[] {0xFA, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00},
            //new byte[] {0x56, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00},
            //new byte[] {0x32, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00},
            //new byte[] {0x70, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00},
            //new byte[] {0x14, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00},
            //new byte[] {0xB8, 0x0a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00},
            //new byte[] {0xDC, 0x0b, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00},
            //new byte[] {0xCF, 0x0c, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00},
            //new byte[] {0xAB, 0x0d, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00},
            //new byte[] {0x07, 0x0e, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00},
            //new byte[] {0x63, 0x0f, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00},
            new byte[] {0x35, 0x00, 0x04, 0x0C, 0x00, 0x00, 0x00, 0x00},
            new byte[] {0x41, 0x01, 0x04, 0x0C, 0x00, 0x00, 0x00, 0x00},
            new byte[] {0x2E, 0x02, 0x04, 0x0C, 0x00, 0x00, 0x00, 0x00},
            new byte[] {0xDD, 0x03, 0x04, 0x0C, 0x00, 0x00, 0x00, 0x00},
            new byte[] {0x2A, 0x04, 0x04, 0x0C, 0x00, 0x00, 0x00, 0x00},
            new byte[] {0x55, 0x05, 0x04, 0x0C, 0x00, 0x00, 0x00, 0x00},
            new byte[] {0x48, 0x06, 0x04, 0x0C, 0x00, 0x00, 0x00, 0x00},
            new byte[] {0xC4, 0x07, 0x04, 0x0C, 0x00, 0x00, 0x00, 0x00},
            new byte[] {0x3E, 0x08, 0x04, 0x0C, 0x00, 0x00, 0x00, 0x00},
            new byte[] {0x1D, 0x09, 0x04, 0x0C, 0x00, 0x00, 0x00, 0x00},
            new byte[] {0xD6, 0x0a, 0x04, 0x0C, 0x00, 0x00, 0x00, 0x00},
            new byte[] {0xA0, 0x0b, 0x04, 0x0C, 0x00, 0x00, 0x00, 0x00},
            new byte[] {0x25, 0x0c, 0x04, 0x0C, 0x00, 0x00, 0x00, 0x00},
            new byte[] {0x9E, 0x0d, 0x04, 0x0C, 0x00, 0x00, 0x00, 0x00},
            new byte[] {0x81, 0x0e, 0x04, 0x0C, 0x00, 0x00, 0x00, 0x00},
            new byte[] {0x8C, 0x0f, 0x04, 0x0C, 0x00, 0x00, 0x00, 0x00},
        };

        private byte _bz0X3D5;
        private List<byte[]> _list0X3D5 = new List<byte[]>
        {
             new byte[] { 0xB7, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
             new byte[] { 0xE0, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
             new byte[] { 0x21, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
             new byte[] { 0x2D, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
             new byte[] { 0x8D, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
             new byte[] { 0x06, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
             new byte[] { 0x99, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
             new byte[] { 0x82, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
             new byte[] { 0x87, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
             new byte[] { 0xB9, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
             new byte[] { 0x8A, 0x0a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
             new byte[] { 0x65, 0x0b, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
             new byte[] { 0x1A, 0x0c, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
             new byte[] { 0x3E, 0x0d, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
             new byte[] { 0x00, 0x0e, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
             new byte[] { 0xE7, 0x0f, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
        };

        private byte _bz0X15F;
        private List<byte[]> _list0X15F = new List<byte[]>
        {
             new byte[] { 0xA5, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
             new byte[] { 0x92, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
             new byte[] { 0x3F, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
             new byte[] { 0xE1, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
             new byte[] { 0x40, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
             new byte[] { 0x2A, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
             new byte[] { 0xE7, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
             new byte[] { 0xAD, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
             new byte[] { 0x0F, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
             new byte[] { 0x75, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
             new byte[] { 0xA8, 0x0a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
             new byte[] { 0xF4, 0x0b, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
             new byte[] { 0xE5, 0x0c, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
             new byte[] { 0xBD, 0x0d, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
             new byte[] { 0x4E, 0x0e, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
             new byte[] { 0x7A, 0x0f, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
        };
        #endregion

        private readonly double[] _ledCurrent =
        {
            0, 20, 20, 27, 39, 56, 63, 60.5, 52, 39, 33, //L1~L10
            25, 0, 0, 0, 0, 0, //L11~L16
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, //L17~L32
            20, 20, 27, 39, 56, 63, 60.5, 52, 39, 33, //R1~R10
            25, 0, 0, 0, 0, 0, //R11~R16
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 //R17~R18
        }; // LUMILED

        public enum LampControl
        {
            [MatrixValDefinition(0, 1, 0)]
            [Description("0x194")]
            LeftLowBeamOff,

            [MatrixValDefinition(0, 1, 1)]
            [Description("0x194")]
            LeftLowBeamOn,

            [MatrixValDefinition(2, 1, 0)]
            [Description("0x194")]
            RightLowBeamOff,

            [MatrixValDefinition(2, 1, 1)]
            [Description("0x194")]
            RightLowBeamOn,

            [MatrixValDefinition(4, 1, 0)]
            [Description("0x194")]
            LeftHighBeamOff,

            [MatrixValDefinition(4, 1, 1)]
            [Description("0x194")]
            LeftHighBeamOn,

            [MatrixValDefinition(6, 1, 0)]
            [Description("0x194")]
            RightHighBeamOff,

            [MatrixValDefinition(6, 1, 1)]
            [Description("0x194")]
            RightHighBeamOn,

            [MatrixValDefinition(8, 2, 0)]
            [Description("0x194")]
            LeftAllWeatherOff,

            [MatrixValDefinition(8, 2, 3)]
            [Description("0x194")]
            LeftAllWeatherOn,

            [MatrixValDefinition(10, 2, 0)]
            [Description("0x194")]
            RightAllWeatherOff,

            [MatrixValDefinition(10, 2, 3)]
            [Description("0x194")]
            RightAllWeatherOn,

            [MatrixValDefinition(15, 1, 0)]
            [Description("0x3D5")]
            LowBeamOff,

            [MatrixValDefinition(15, 1, 1)]
            [Description("0x3D5")]
            LowBeamOn,

            [MatrixValDefinition(12, 1, 0)]
            [Description("0x193")]
            LeftDrlOff,

            [MatrixValDefinition(12, 1, 1)]
            [Description("0x193")]
            LeftDrlOn,

            [MatrixValDefinition(13, 1, 0)]
            [Description("0x193")]
            RightDrlOff,

            [MatrixValDefinition(13, 1, 1)]
            [Description("0x193")]
            RightDrlOn,

            [MatrixValDefinition(28, 1, 0)]
            [Description("0x193")]
            LeftPlOff,

            [MatrixValDefinition(28, 1, 1)]
            [Description("0x193")]
            LeftPlOn,

            [MatrixValDefinition(29, 1, 0)]
            [Description("0x193")]
            RightPlOff,

            [MatrixValDefinition(29, 1, 1)]
            [Description("0x193")]
            RightPlOn,

            [MatrixValDefinition(30, 1, 0)]
            [Description("0x193")]
            LeftParkOff,

            [MatrixValDefinition(30, 1, 1)]
            [Description("0x193")]
            LeftParkOn,

            [MatrixValDefinition(31, 1, 0)]
            [Description("0x193")]
            RightParkOff,

            [MatrixValDefinition(31, 1, 1)]
            [Description("0x193")]
            RightParkOn,

            [MatrixValDefinition(16, 1, 0)]
            [Description("0x193")]
            LeftTurnOff,

            [MatrixValDefinition(16, 1, 1)]
            [Description("0x193")]
            LeftTurnOn,

            [MatrixValDefinition(17, 1, 0)]
            [Description("0x193")]
            RightTurnOff,

            [MatrixValDefinition(17, 1, 1)]
            [Description("0x193")]
            RightTurnOn,

            [MatrixValDefinition(18, 1, 0)]
            [Description("0x193")]
            LeftTurnLoopOff,

            [MatrixValDefinition(18, 1, 1)]
            [Description("0x193")]
            LeftTurnLoopOn,

            [MatrixValDefinition(19, 1, 0)]
            [Description("0x193")]
            RightTurnLoopOff,

            [MatrixValDefinition(19, 1, 1)]
            [Description("0x193")]
            RightTurnLoopOn,

            [MatrixValDefinition(35, 1, 1)]
            [Description("0x193")]
            LCM_Blinkerwischen_Anf_On,

            [MatrixValDefinition(35, 1, 0)]
            [Description("0x193")]
            LCM_Blinkerwischen_Anf_Off,

            [MatrixValDefinition(0, 1, 0)]
            [Description("0x195")]
            LeftCornerOff,

            [MatrixValDefinition(0, 1, 1)]
            [Description("0x195")]
            LeftCornerOn,

            [MatrixValDefinition(2, 1, 0)]
            [Description("0x195")]
            RightCornerOff,

            [MatrixValDefinition(2, 1, 1)]
            [Description("0x195")]
            RightCornerOn,

            [MatrixValDefinition(13, 1, 1)]
            [Description("0x190")]
            LeftMotorResetOn,

            [MatrixValDefinition(13, 1, 0)]
            [Description("0x190")]
            LeftMotorResetOff,

            [MatrixValDefinition(37, 1, 1)]
            [Description("0x190")]
            RightMotorResetOn,

            [MatrixValDefinition(37, 1, 0)]
            [Description("0x190")]
            RightMotorResetOff,
        }

        #endregion

        #region MDF&AFS控制

        private bool _isHelaMdfControl;
        private readonly object _lockMdfControlBytes = new object();
        private readonly object _lockAfsControlBytes = new object();

        [Description("开启海拉MDF控制")]
        public void StartHelaMdfControlSignal()
        {
            Soa1 = 225;
            Soa2 = 171;
            _motorolaMatrix0X110.MatrixData = new byte[] { 0x00, 0x00, 0x00, 0x00, 0xA0, 0x41, 0x03, 0x00 };

            _isHelaMdfControl = true;
        }

        [Description("关闭海拉MDF控制")]
        public void StopHelaMdfControlSignal()
        {
            Soa1 = 0x00;
            Soa2 = 0x00;
            _motorolaMatrix0X110.MatrixData = new byte[] { 0xFE, 0x07, 0xFE, 0x07, 0x00, 0x00, 0x00, 0x00 };

            _isHelaMdfControl = false;
        }

        [Description("MDF运动到")]
        public void MdfGoTo(double pos)
        {
            if (pos >= 0 && pos <= 200)
            {
                var value = (uint)(Math.Round(pos / 0.05, 0, MidpointRounding.AwayFromZero));

                lock (_lockMdfControlBytes)
                {
                    _motorolaMatrix0X110.MatrixData = SetIntelValue(_motorolaMatrix0X110.MatrixData, 0, 12, value);
                    _motorolaMatrix0X110.MatrixData = SetIntelValue(_motorolaMatrix0X110.MatrixData, 16, 12, value);
                }
            }
        }

        [Description("LeftAFS运动到")]
        public void LeftAfsGoTo(double pos)
        {
            if (pos >= -31.96875 && pos <= 31.953125)
            {
                var value = (uint)(Math.Round((pos + 31.96875) / 0.015625, 0, MidpointRounding.AwayFromZero));
                lock (_lockAfsControlBytes)
                    _motorolaMatrix0X192.MatrixData = SetIntelValue(_motorolaMatrix0X192.MatrixData, 0, 12, value);
            }
        }

        [Description("RightAFS运动到")]
        public void RightAfsGoTo(double pos)
        {
            if (pos >= -31.96875 && pos <= 31.953125)
            {
                var value = (uint)(Math.Round((pos + 31.96875) / 0.015625, 0, MidpointRounding.AwayFromZero));
                lock (_lockAfsControlBytes)
                    _motorolaMatrix0X192.MatrixData = SetIntelValue(_motorolaMatrix0X192.MatrixData, 24, 12, value);
            }
        }

        public static int GetIntelValue(byte[] data, int startBit, int bitLen)
        {
            try
            {
                var bitData = new BitArray(data);

                var listBitStr = new List<string>();
                for (var i = 0; i < bitLen; i++)
                {
                    listBitStr.Add(bitData[startBit + i] ? "1" : "0");
                }

                var str = string.Empty;
                for (var i = listBitStr.Count - 1; i >= 0; i--)
                    str += listBitStr[i];

                var value = Convert.ToInt32(str, 2);

                return value;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static byte[] SetIntelValue(byte[] data, int startBit, int bitLen, uint value)
        {
            try
            {
                var bits = Convert.ToString(value, 2).PadLeft(32, '0').ToCharArray().Reverse().ToArray();
                var bitsList = new List<string>();
                for (var i = 0; i < bitLen; i++)
                    bitsList.Add(bits[i].ToString());

                var bitData = new BitArray(data);
                var listBitStr = new List<string>();
                for (var i = 0; i < bitData.Count; i++)
                {
                    listBitStr.Add(bitData[i] ? "1" : "0");
                }

                for (var i = 0; i < bitLen; i++)
                {
                    listBitStr[startBit + i] = bitsList[i];
                }

                var returnByte = new List<byte>();
                for (var i = 0; i < listBitStr.Count; i = i + 8)
                {
                    var str = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", listBitStr[i + 7], listBitStr[i + 6], listBitStr[i + 5], listBitStr[i + 4], listBitStr[i + 3], listBitStr[i + 2], listBitStr[i + 1], listBitStr[i + 0]);

                    var b = Convert.ToByte(str, 2);
                    returnByte.Add(b);
                }

                return returnByte.ToArray();
            }
            catch (Exception)
            {
                return data;
            }
        }

        [Description("R,LeftMdfRealPos")]
        public double LeftMdfRealPos = 0;
        [Description("R,LeftAfsRealPos")]
        public double LeftAfsRealPos = 0;
        [Description("R,RightMdfRealPos")]
        public double RightMdfRealPos = 0;
        [Description("R,RightAfsRealPos")]
        public double RightAfsRealPos = 0;

        private double CalMdfRealPos(byte[] data, int startBit, int bitLen)
        {
            try
            {
                var value = GetIntelValue(data, startBit, bitLen);
                return Math.Round((value * 0.05), 2, MidpointRounding.AwayFromZero);
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        private double CalAfsRealPos(byte[] data, int startBit, int bitLen)
        {
            try
            {
                var value = GetIntelValue(data, startBit, bitLen);
                return Math.Round((value * 0.015625 - 31.96875), 2, MidpointRounding.AwayFromZero);
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        #endregion
    }
}
