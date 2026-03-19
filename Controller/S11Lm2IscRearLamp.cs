using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace Controller
{
    [Description("CAN-Product,S11L-M2后组合灯")]
    public sealed class S11Lm2IscRearLamp : ControllerBase
    {
        public CanBus CanFd;

        [Description("R/W,是否接RCLB")] public bool IsHaveRclb;
        [Description("R/W,是否接RCLA")] public bool IsHaveRcla;

        public S11Lm2IscRearLamp(string name) :
            base(name)
        {
            //S11L后组合灯
            MainWork();
            SchedulerAsync();
        }

        private readonly object _lockCanSend = new object();
        private bool _isSleep = true;
        private bool _isHardwareControl = false;

        private readonly byte[] _rclbRedGray = new byte[146];
        private readonly byte[] _rclbYellowGray = new byte[62];

        private readonly byte[] _rclaLeftRedGray = new byte[35];
        private readonly byte[] _rclaLeftYellowGray = new byte[33];

        private readonly byte[] _rclaRightRedGray = new byte[35];
        private readonly byte[] _rclaRightYellowGray = new byte[33];

        private const byte TurnGray = 0x4D;
        private const byte TurnHdGray = 0xFF;
        private const byte TailHdGray = 0xCF;
        private const byte TailGray = 0x1F;
        private const byte StopGray = 0xFF;

        private const byte OddEvenGray = 0xFF;

        private bool _isLeftTurnOn;
        private bool _isLeftTurnHdOn;
        private bool _isRightTurnOn;
        private bool _isRightTurnHdOn;

        private bool _isLeftStopOn;
        private bool _isRightStopOn;

        /// <summary>
        /// 0=off,1=on,2=hdOn
        /// </summary>
        private int _isLeftTailOn;

        /// <summary>
        /// 0=off,1=on,2=hdOn
        /// </summary>
        private int _isRightTailOn;

        #region 主要控制

        private void MainWork()
        {
            var msg0X200 = new byte[64];
            var msg0X22A = new byte[64];
            var msg0X225 = new byte[64];
            //var msg0X22B = new byte[64];

            var msg0X201 = new byte[64];
            var msg0X227 = new byte[64];
            var msg0X22B = new byte[64];
            var msg0X22C = new byte[64];
            var msg0X22D = new byte[64];

            // Control frame
            SetTimer(new MyTaskScheduler.TaskInfo()
            {
                Action = SendTask(new Func<CanBus.CanDataPackage[]>(() =>
                {
                    var lstPackages = new List<CanBus.CanDataPackage>
                    {
                        new CanBus.CanDataPackage(0x226, CanBus.CanProtocol.CanFd, CanBus.CanType.Standard,
                            CanBus.CanFormat.Data,
                            new byte[]
                            {
                                _isHardwareControl ? (byte)0x40 : (byte)0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
                            })
                    };

                    return lstPackages.ToArray();
                })),
                Interval = 40
            });

            // RCLB 40MS
            SetTimer(new MyTaskScheduler.TaskInfo()
            {
                Action = SendTask(new Func<CanBus.CanDataPackage[]>(() =>
                {
                    if (IsHaveRclb)
                    {
                        if (_isRedOddEvenControl > 0)
                        {
                            switch (_isRedOddEvenControl)
                            {
                                case 1:
                                    _rclbRedGray[72] = _rclbRedGray[73] = OddEvenGray;

                                    for (var i = 0; i < 72; i = i + 6)
                                    {
                                        _rclbRedGray[i] = _rclbRedGray[i + 1] = _rclbRedGray[i + 2] = OddEvenGray;
                                        _rclbRedGray[i + 3] = _rclbRedGray[i + 4] = _rclbRedGray[i + 5] = 0x00;
                                    }
                                    for (var i = 74; i < _rclbRedGray.Length; i = i + 6)
                                    {
                                        _rclbRedGray[i + 3] = _rclbRedGray[i + 4] = _rclbRedGray[i + 5] = OddEvenGray;
                                        _rclbRedGray[i] = _rclbRedGray[i + 1] = _rclbRedGray[i + 2] = 0x00;
                                    }
                                    break;

                                case 2:
                                    _rclbRedGray[72] = _rclbRedGray[73] = 0x00;

                                    for (var i = 0; i < 72; i = i + 6)
                                    {
                                        _rclbRedGray[i] = _rclbRedGray[i + 1] = _rclbRedGray[i + 2] = 0x00;
                                        _rclbRedGray[i + 3] = _rclbRedGray[i + 4] = _rclbRedGray[i + 5] = OddEvenGray;
                                    }
                                    for (var i = 74; i < _rclbRedGray.Length; i = i + 6)
                                    {
                                        _rclbRedGray[i + 3] = _rclbRedGray[i + 4] = _rclbRedGray[i + 5] = 0x00;
                                        _rclbRedGray[i] = _rclbRedGray[i + 1] = _rclbRedGray[i + 2] = OddEvenGray;
                                    }
                                    break;

                                case 3:
                                    for (var j = 0; j < _rclbRedGray.Length; j++)
                                        _rclbRedGray[j] = OddEvenGray;
                                    break;
                            }
                        }
                        else
                        {
                            switch (_isLeftTailOn)
                            {
                                case 0:
                                    for (var i = 0; i < _rclbRedGray.Length / 2; i++)
                                        _rclbRedGray[i] = 0x00;
                                    break;

                                case 1:
                                    for (var i = 0; i < _rclbRedGray.Length / 2; i++)
                                        _rclbRedGray[i] = TailGray;
                                    break;

                                case 2:
                                    for (var i = 0; i < _rclbRedGray.Length / 2; i++)
                                        _rclbRedGray[i] = TailHdGray;
                                    break;
                            }

                            if (_isLeftStopOn)
                                for (var i = 0; i < _rclbRedGray.Length / 2; i++)
                                    _rclbRedGray[i] = StopGray;

                            switch (_isRightTailOn)
                            {
                                case 0:
                                    for (var i = _rclbRedGray.Length / 2; i < _rclbRedGray.Length; i++)
                                        _rclbRedGray[i] = 0x00;
                                    break;

                                case 1:
                                    for (var i = _rclbRedGray.Length / 2; i < _rclbRedGray.Length; i++)
                                        _rclbRedGray[i] = TailGray;
                                    break;

                                case 2:
                                    for (var i = _rclbRedGray.Length / 2; i < _rclbRedGray.Length; i++)
                                        _rclbRedGray[i] = TailHdGray;
                                    break;
                            }

                            if (_isRightStopOn)
                                for (var i = _rclbRedGray.Length / 2; i < _rclbRedGray.Length; i++)
                                    _rclbRedGray[i] = StopGray;
                        }

                        if (_isYellowOddEvenControl > 0)
                        {
                            switch (_isYellowOddEvenControl)
                            {
                                case 1:
                                    // L
                                    for (var i = 0; i < 20; i = i + 4)
                                    {
                                        _rclbYellowGray[i] = _rclbYellowGray[i + 1] = OddEvenGray;
                                        _rclbYellowGray[i + 2] = _rclbYellowGray[i + 3] = 0x00;
                                    }

                                    _rclbYellowGray[20] = _rclbYellowGray[21] = OddEvenGray;
                                    _rclbYellowGray[22] = 0x00;

                                    for (var i = 23; i < 31; i = i + 4)
                                    {
                                        _rclbYellowGray[i] = _rclbYellowGray[i + 1] = OddEvenGray;
                                        _rclbYellowGray[i + 2] = _rclbYellowGray[i + 3] = 0x00;
                                    }

                                    // R
                                    for (var i = 61; i > 41; i = i - 4)
                                    {
                                        _rclbYellowGray[i] = _rclbYellowGray[i - 1] = OddEvenGray;
                                        _rclbYellowGray[i - 2] = _rclbYellowGray[i - 3] = 0x00;
                                    }

                                    _rclbYellowGray[41] = _rclbYellowGray[40] = OddEvenGray;
                                    _rclbYellowGray[39] = 0x00;

                                    _rclbYellowGray[38] = OddEvenGray;
                                    _rclbYellowGray[37] = OddEvenGray;
                                    _rclbYellowGray[34] = OddEvenGray;
                                    _rclbYellowGray[33] = OddEvenGray;

                                    _rclbYellowGray[36] = 0x00;
                                    _rclbYellowGray[35] = 0x00;
                                    break;

                                case 2:
                                    // L
                                    for (var i = 0; i < 20; i = i + 4)
                                    {
                                        _rclbYellowGray[i] = _rclbYellowGray[i + 1] = 0x00;
                                        _rclbYellowGray[i + 2] = _rclbYellowGray[i + 3] = OddEvenGray;
                                    }

                                    _rclbYellowGray[20] = _rclbYellowGray[21] = 0x00;
                                    _rclbYellowGray[22] = OddEvenGray;

                                    for (var i = 23; i < 31; i = i + 4)
                                    {
                                        _rclbYellowGray[i] = _rclbYellowGray[i + 1] = 0x00;
                                        _rclbYellowGray[i + 2] = _rclbYellowGray[i + 3] = OddEvenGray;
                                    }

                                    // R
                                    for (var i = 61; i > 41; i = i - 4)
                                    {
                                        _rclbYellowGray[i] = _rclbYellowGray[i - 1] = 0x00;
                                        _rclbYellowGray[i - 2] = _rclbYellowGray[i - 3] = OddEvenGray;
                                    }

                                    _rclbYellowGray[41] = _rclbYellowGray[40] = 0x00;
                                    _rclbYellowGray[39] = OddEvenGray;

                                    _rclbYellowGray[38] = 0x00;
                                    _rclbYellowGray[37] = 0x00;
                                    _rclbYellowGray[34] = 0x00;
                                    _rclbYellowGray[33] = 0x00;

                                    _rclbYellowGray[36] = OddEvenGray;
                                    _rclbYellowGray[35] = OddEvenGray;
                                    break;

                                case 3:
                                    for (var i = 0; i < _rclbYellowGray.Length; i++)
                                        _rclbYellowGray[i] = OddEvenGray;
                                    break;
                            }
                        }
                        else
                        {
                            if (_isLeftTurnOn)
                                for (var j = 0; j < 31; j++)
                                    _rclbYellowGray[j] = TurnGray;
                            else
                                for (var j = 0; j < 31; j++)
                                    _rclbYellowGray[j] = 0x00;

                            if (_isRightTurnOn)
                                for (var j = 31; j < 62; j++)
                                    _rclbYellowGray[j] = TurnGray;
                            else
                                for (var j = 31; j < 62; j++)
                                    _rclbYellowGray[j] = 0x00;
                        }

                        // copy red
                        Array.Copy(_rclbRedGray, 0, msg0X201, 0, 64);
                        Array.Copy(_rclbRedGray, 64, msg0X227, 0, 18);
                        Array.Copy(_rclbRedGray, 82, msg0X22B, 8, 56);
                        Array.Copy(_rclbRedGray, 138, msg0X22C, 0, 8);

                        // copy yellow
                        Array.Copy(_rclbYellowGray, 0, msg0X22C, 8, 56);
                        Array.Copy(_rclbYellowGray, 56, msg0X22D, 0, 6);

                        var lstPackages = new List<CanBus.CanDataPackage>
                        {
                            new CanBus.CanDataPackage(0x201, CanBus.CanProtocol.CanFd, CanBus.CanType.Standard,
                                CanBus.CanFormat.Data, msg0X201),
                            new CanBus.CanDataPackage(0x227, CanBus.CanProtocol.CanFd, CanBus.CanType.Standard,
                                CanBus.CanFormat.Data, msg0X227),
                            new CanBus.CanDataPackage(0x22B, CanBus.CanProtocol.CanFd, CanBus.CanType.Standard,
                                CanBus.CanFormat.Data, msg0X22B),
                            new CanBus.CanDataPackage(0x22C, CanBus.CanProtocol.CanFd, CanBus.CanType.Standard,
                                CanBus.CanFormat.Data, msg0X22C),
                            new CanBus.CanDataPackage(0x22D, CanBus.CanProtocol.CanFd, CanBus.CanType.Standard,
                                CanBus.CanFormat.Data, msg0X22D)
                        };

                        return lstPackages.ToArray();
                    }
                    else
                    {
                        return new CanBus.CanDataPackage[0];
                    }
                })),
                Interval = 40
            });

            // RCLA 40MS
            SetTimer(new MyTaskScheduler.TaskInfo()
            {
                Action = SendTask(new Func<CanBus.CanDataPackage[]>(() =>
                {
                    if (IsHaveRcla)
                    {
                        if (_isRedOddEvenControl > 0)
                        {
                            switch (_isRedOddEvenControl)
                            {
                                case 1:
                                    // L
                                    {
                                        _rclaLeftRedGray[0] = OddEvenGray;
                                        _rclaLeftRedGray[1] = OddEvenGray;

                                        var isOdd = false;
                                        for (var i = 2; i < _rclaLeftRedGray.Length; i = i + 3)
                                        {
                                            if (isOdd)
                                                _rclaLeftRedGray[i] = _rclaLeftRedGray[i + 1] =
                                                    _rclaLeftRedGray[i + 2] = OddEvenGray;
                                            else
                                                _rclaLeftRedGray[i] =
                                                    _rclaLeftRedGray[i + 1] = _rclaLeftRedGray[i + 2] = 0x00;

                                            isOdd = !isOdd;
                                        }
                                    }

                                    // R
                                    {
                                        _rclaRightRedGray[0] = OddEvenGray;
                                        _rclaRightRedGray[1] = OddEvenGray;

                                        var isOdd = false;
                                        for (var i = 2; i < _rclaRightRedGray.Length; i = i + 3)
                                        {
                                            if (isOdd)
                                                _rclaRightRedGray[i] = _rclaRightRedGray[i + 1] =
                                                    _rclaRightRedGray[i + 2] = OddEvenGray;
                                            else
                                                _rclaRightRedGray[i] =
                                                    _rclaRightRedGray[i + 1] = _rclaRightRedGray[i + 2] = 0x00;

                                            isOdd = !isOdd;
                                        }
                                    }
                                    break;

                                case 2:
                                    // L
                                    {
                                        _rclaLeftRedGray[0] = 0x00;
                                        _rclaLeftRedGray[1] = 0x00;

                                        var isEven = true;
                                        for (var i = 2; i < _rclaLeftRedGray.Length; i = i + 3)
                                        {
                                            if (isEven)
                                                _rclaLeftRedGray[i] = _rclaLeftRedGray[i + 1] = _rclaLeftRedGray[i + 2] = OddEvenGray;
                                            else
                                                _rclaLeftRedGray[i] = _rclaLeftRedGray[i + 1] = _rclaLeftRedGray[i + 2] = 0x00;
                                            isEven = !isEven;
                                        }
                                    }

                                    // R
                                    {
                                        _rclaRightRedGray[0] = 0x00;
                                        _rclaRightRedGray[1] = 0x00;

                                        var isEven = true;
                                        for (var i = 2; i < _rclaRightRedGray.Length; i = i + 3)
                                        {
                                            if (isEven)
                                                _rclaRightRedGray[i] = _rclaRightRedGray[i + 1] = _rclaRightRedGray[i + 2] = OddEvenGray;
                                            else
                                                _rclaRightRedGray[i] = _rclaRightRedGray[i + 1] = _rclaRightRedGray[i + 2] = 0x00;
                                            isEven = !isEven;
                                        }
                                    }
                                    break;

                                case 3:
                                    for (var j = 0; j < _rclaLeftRedGray.Length; j++)
                                        _rclaLeftRedGray[j] = OddEvenGray;
                                    for (var j = 0; j < _rclaRightRedGray.Length; j++)
                                        _rclaRightRedGray[j] = OddEvenGray;
                                    break;
                            }
                        }
                        else
                        {
                            switch (_isLeftTailOn)
                            {
                                case 0:
                                    for (var i = 0; i < _rclaLeftRedGray.Length; i++)
                                        _rclaLeftRedGray[i] = 0x00;
                                    break;

                                case 1:
                                    for (var i = 0; i < _rclaLeftRedGray.Length; i++)
                                        _rclaLeftRedGray[i] = TailGray;
                                    break;

                                case 2:
                                    for (var i = 0; i < _rclaLeftRedGray.Length; i++)
                                        _rclaLeftRedGray[i] = TailHdGray;
                                    break;
                            }

                            if (_isLeftStopOn)
                                for (var i = 0; i < _rclaLeftRedGray.Length; i++)
                                    _rclaLeftRedGray[i] = StopGray;

                            switch (_isRightTailOn)
                            {
                                case 0:
                                    for (var i = 0; i < _rclaRightRedGray.Length; i++)
                                        _rclaRightRedGray[i] = 0x00;
                                    break;

                                case 1:
                                    for (var i = 0; i < _rclaRightRedGray.Length; i++)
                                        _rclaRightRedGray[i] = TailGray;
                                    break;

                                case 2:
                                    for (var i = 0; i < _rclaRightRedGray.Length; i++)
                                        _rclaRightRedGray[i] = TailHdGray;
                                    break;
                            }

                            if (_isRightStopOn)
                                for (var i = 0; i < _rclaRightRedGray.Length; i++)
                                    _rclaRightRedGray[i] = StopGray;

                        }

                        if (_isYellowOddEvenControl > 0)
                        {
                            switch (_isYellowOddEvenControl)
                            {
                                case 1:
                                    // L
                                    {
                                        _rclaLeftYellowGray[0] = OddEvenGray;

                                        var isOdd = false;
                                        for (var i = 1; i < _rclaLeftYellowGray.Length; i = i + 2)
                                        {
                                            if (isOdd)
                                                _rclaLeftYellowGray[i] = _rclaLeftYellowGray[i + 1] = OddEvenGray;
                                            else
                                                _rclaLeftYellowGray[i] = _rclaLeftYellowGray[i + 1] = 0x00;

                                            isOdd = !isOdd;
                                        }
                                    }

                                    // R
                                    {
                                        _rclaRightYellowGray[0] = OddEvenGray;

                                        var isOdd = false;
                                        for (var i = 1; i < _rclaRightYellowGray.Length; i = i + 2)
                                        {
                                            if (isOdd)
                                                _rclaRightYellowGray[i] = _rclaRightYellowGray[i + 1] = OddEvenGray;
                                            else
                                                _rclaRightYellowGray[i] = _rclaRightYellowGray[i + 1] = 0x00;

                                            isOdd = !isOdd;
                                        }
                                    }
                                    break;

                                case 2:
                                    // L
                                    {
                                        _rclaLeftYellowGray[0] = 0x00;

                                        var isEven = true;
                                        for (var i = 1; i < _rclaLeftYellowGray.Length; i = i + 2)
                                        {
                                            if (isEven)
                                                _rclaLeftYellowGray[i] = _rclaLeftYellowGray[i + 1] = OddEvenGray;
                                            else
                                                _rclaLeftYellowGray[i] = _rclaLeftYellowGray[i + 1] = 0x00;
                                            isEven = !isEven;
                                        }
                                    }

                                    // R
                                    {
                                        _rclaRightYellowGray[0] = 0x00;

                                        var isEven = true;
                                        for (var i = 1; i < _rclaRightYellowGray.Length; i = i + 2)
                                        {
                                            if (isEven)
                                                _rclaRightYellowGray[i] = _rclaRightYellowGray[i + 1] = OddEvenGray;
                                            else
                                                _rclaRightYellowGray[i] = _rclaRightYellowGray[i + 1] = 0x00;
                                            isEven = !isEven;
                                        }
                                    }
                                    break;

                                case 3:
                                    for (var i = 0; i < _rclaLeftYellowGray.Length; i++)
                                        _rclaLeftYellowGray[i] = OddEvenGray;
                                    for (var i = 0; i < _rclaRightYellowGray.Length; i++)
                                        _rclaRightYellowGray[i] = OddEvenGray;
                                    break;
                            }
                        }
                        else
                        {
                            if (_isLeftTurnOn)
                                for (var j = 0; j < _rclaLeftYellowGray.Length; j++)
                                    _rclaLeftYellowGray[j] = _isLeftTurnHdOn ? TurnHdGray : TurnGray;
                            else
                                for (var j = 0; j < _rclaLeftYellowGray.Length; j++)
                                    _rclaLeftYellowGray[j] = 0x00;

                            if (_isRightTurnOn)
                                for (var j = 0; j < _rclaRightYellowGray.Length; j++)
                                    _rclaRightYellowGray[j] = _isRightTurnHdOn ? TurnHdGray : TurnGray;
                            else
                                for (var j = 0; j < _rclaRightYellowGray.Length; j++)
                                    _rclaRightYellowGray[j] = 0x00;
                        }

                        // copy red L
                        Array.Copy(_rclaLeftRedGray, 0, msg0X200, 0, 32);
                        Array.Copy(_rclaLeftRedGray, 32, msg0X22A, 0, 3);

                        // copy red R
                        Array.Copy(_rclaRightRedGray, 0, msg0X225, 0, 32);
                        Array.Copy(_rclaRightRedGray, 32, msg0X22A, 36, 3);

                        // copy yellow L
                        Array.Copy(_rclaLeftYellowGray, 0, msg0X22A, 3, 33);

                        // copy yellow R
                        Array.Copy(_rclaRightYellowGray, 0, msg0X22A, 39, 25);
                        Array.Copy(_rclaRightYellowGray, 25, msg0X22B, 0, 8);

                        var lstPackages = new List<CanBus.CanDataPackage>
                        {
                            new CanBus.CanDataPackage(0x200, CanBus.CanProtocol.CanFd, CanBus.CanType.Standard,
                                CanBus.CanFormat.Data, msg0X200),
                            new CanBus.CanDataPackage(0x22A, CanBus.CanProtocol.CanFd, CanBus.CanType.Standard,
                                CanBus.CanFormat.Data, msg0X22A),
                            new CanBus.CanDataPackage(0x225, CanBus.CanProtocol.CanFd, CanBus.CanType.Standard,
                                CanBus.CanFormat.Data, msg0X225),
                            new CanBus.CanDataPackage(0x22b, CanBus.CanProtocol.CanFd, CanBus.CanType.Standard,
                                CanBus.CanFormat.Data, msg0X22B),
                        };

                        return lstPackages.ToArray();
                    }
                    else
                    {
                        return new CanBus.CanDataPackage[0];
                    }
                })),
                Interval = 40
            });
        }

        private Action SendTask(Func<CanBus.CanDataPackage[]> msgFunc)
        {
            return () =>
            {
                if (CanFd == null || msgFunc == null || _isSleep)
                    return;

                lock (_lockCanSend)
                    CanFd.SendCanDatas(msgFunc.Invoke());
            };
        }

        [Description("开启CAN")]
        public void StartCan()
        {
            _isSleep = false;
        }

        [Description("关闭CAN")]
        public void StopCan()
        {
            _isSleep = true;
        }

        [Description("开启硬线控制")]
        public void StartHardwareControl()
        {
            _isHardwareControl = true;
        }

        [Description("关闭硬线控制")]
        public void StopHardwareControl()
        {
            _isHardwareControl = false;
        }

        #endregion

        #region 正常控制灯

        [Description("LeftTailOn")]
        public void LeftTailOn()
        {
            _isLeftTailOn = 1;
        }

        [Description("LeftTailHdOn")]
        public void LeftTailHdOn()
        {
            _isLeftTailOn = 2;
        }

        [Description("LeftTailOff")]
        public void LeftTailOff()
        {
            _isLeftTailOn = 0;
        }

        [Description("LeftStopOn")]
        public void LeftStopOn()
        {
            _isLeftStopOn = true;
        }

        [Description("LeftStopOff")]
        public void LeftStopOff()
        {
            _isLeftStopOn = false;
        }

        [Description("LeftTurnOn")]
        public void LeftTurnOn()
        {
            _isLeftTurnOn = true;
            _isLeftTurnHdOn = false;
        }

        [Description("LeftTurnHdOn")]
        public void LeftTurnHdOn()
        {
            _isLeftTurnOn = true;
            _isLeftTurnHdOn = true;
        }

        [Description("LeftTurnOff")]
        public void LeftTurnOff()
        {
            _isLeftTurnOn = false;
            _isLeftTurnHdOn = false;
        }

        [Description("RightTailOn")]
        public void RightTailOn()
        {
            _isRightTailOn = 1;
        }

        [Description("RightTailHdOn")]
        public void RightTailHdOn()
        {
            _isRightTailOn = 2;
        }

        [Description("RightTailOff")]
        public void RightTailOff()
        {
            _isRightTailOn = 0;
        }

        [Description("RightStopOn")]
        public void RightStopOn()
        {
            _isRightStopOn = true;
        }

        [Description("RightStopOff")]
        public void RightStopOff()
        {
            _isRightStopOn = false;
        }

        [Description("RightTurnOn")]
        public void RightTurnOn()
        {
            _isRightTurnOn = true;
            _isRightTurnHdOn = false;
        }

        [Description("RightTurnHdOn")]
        public void RightTurnHdOn()
        {
            _isRightTurnOn = true;
            _isRightTurnHdOn = true;
        }

        [Description("RightTurnOff")]
        public void RightTurnOff()
        {
            _isRightTurnOn = false;
            _isRightTurnHdOn = false;
        }

        #endregion

        #region 单双亮

        /// <summary>
        /// 0=off,1=odd,2=even,3=all
        /// </summary>
        private int _isRedOddEvenControl;

        /// <summary>
        /// 0=off,1=odd,2=even,3=all
        /// </summary>
        private int _isYellowOddEvenControl;

        [Description("红色奇数打开")]
        public void RedOddOn()
        {
            LeftStopOff();
            LeftTailOff();
            RightStopOff();
            RightTailOff();

            _isRedOddEvenControl = 1;
        }

        [Description("红色双数打开")]
        public void RedEvenOn()
        {
            LeftStopOff();
            LeftTailOff();
            RightStopOff();
            RightTailOff();

            _isRedOddEvenControl = 2;
        }

        [Description("红色所有打开")]
        public void RedAllOn()
        {
            LeftStopOff();
            LeftTailOff();
            RightStopOff();
            RightTailOff();

            _isRedOddEvenControl = 3;
        }

        [Description("红色所有关闭")]
        public void RedAllOff()
        {
            LeftStopOff();
            LeftTailOff();
            RightStopOff();
            RightTailOff();

            _isRedOddEvenControl = 0;
        }

        [Description("黄色奇数打开")]
        public void YellowOddOn()
        {
            LeftTurnOff();
            RightTurnOff();

            _isYellowOddEvenControl = 1;
        }

        [Description("黄色偶数打开")]
        public void YellowEvenOff()
        {
            LeftTurnOff();
            RightTurnOff();

            _isYellowOddEvenControl = 2;
        }

        [Description("黄色所有打开")]
        public void YellowAllOn()
        {
            LeftTurnOff();
            RightTurnOff();

            _isYellowOddEvenControl = 3;
        }

        [Description("黄色所有关闭")]
        public void YellowAllOff()
        {
            LeftTurnOff();
            RightTurnOff();

            _isYellowOddEvenControl = 0;
        }

        #endregion

        #region 版本信息

        [Description("R,RclbAppVer")]
        public string RclbAppVer = string.Empty;
        [Description("R,RclbSoftwareVer")]
        public string RclbSoftwareVer = string.Empty;
        [Description("ReadRclbVer")]
        public void ReadRclbVer()
        {
            RclbAppVer = string.Empty;
            RclbSoftwareVer = string.Empty;

            if (CanFd == null)
                return;

            // AppVer
            RclbAppVer = ReadbVer(0x7A2, 0x7B2, 0xF1, 0x95);

            // SoftVer
            RclbSoftwareVer = ReadbVer(0x7A2, 0x7B2, 0xF1, 0x97);
        }

        [Description("R,RclaLeftAppVer")]
        public string RclaLeftAppVer = string.Empty;
        [Description("R,RclaLeftSoftwareVer")]
        public string RclaLeftSoftwareVer = string.Empty;
        [Description("ReadRclaLeftVer")]
        public void ReadRclaLeftVer()
        {
            RclaLeftAppVer = string.Empty;
            RclaLeftSoftwareVer = string.Empty;

            if (CanFd == null)
                return;

            // AppVer
            RclaLeftAppVer = ReadbVer(0x7A0, 0x7B0, 0xF1, 0x95);

            // SoftVer
            RclaLeftSoftwareVer = ReadbVer(0x7A0, 0x7B0, 0xF1, 0x97);
        }

        [Description("R,RclaRightAppVer")]
        public string RclaRightAppVer = string.Empty;
        [Description("R,RclaRightSoftwareVer")]
        public string RclaRightSoftwareVer = string.Empty;
        [Description("ReadRclaRightVer")]
        public void ReadRclaRightVer()
        {
            RclaRightAppVer = string.Empty;
            RclaRightSoftwareVer = string.Empty;

            if (CanFd == null)
                return;

            // AppVer
            RclaRightAppVer = ReadbVer(0x7A1, 0x7B1, 0xF1, 0x95);

            // SoftVer
            RclaRightSoftwareVer = ReadbVer(0x7A1, 0x7B1, 0xF1, 0x97);
        }

        private string ReadbVer(uint reqCanId, uint recvCanId, byte didHi, byte didLo)
        {
            var str = string.Empty;

            lock (_lockCanSend)
            {
                byte[] echo;
                if (CanFd.CanBusWithUds.TryReadData(reqCanId, recvCanId, CanBus.CanType.Standard,
                        CanBus.CanProtocol.CanFd, didHi, didLo, out echo, 0x00))
                    str = echo.GetStringByAsciiBytes(false);
                else
                {
                    Thread.Sleep(150);
                    if (CanFd.CanBusWithUds.TryReadData(reqCanId, recvCanId, CanBus.CanType.Standard,
                            CanBus.CanProtocol.CanFd, didHi, didLo, out echo, 0x00))
                        str = echo.GetStringByAsciiBytes(false);
                }
            }

            return str;
        }

        #endregion
    }
}
