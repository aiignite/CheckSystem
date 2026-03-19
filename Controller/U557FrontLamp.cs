using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Controller
{
    [Description("CAN-Product,U557高配前灯")]
    public sealed class U557FrontLamp : ControllerBase
    {
        public CanBus Can;

        public U557FrontLamp(string name) : base(name)
        {
            InitSignals();
            MainWork();
        }

        ~U557FrontLamp()
        {
            Dispose();
        }

        private bool _isLeft = true;
        private bool _isAwake;
        private readonly object _canSendLock = new object();

        private void InitSignals()
        {
            #region init signals

            _matrixValDefinitions.Add(LftDrlCmd, new MatrixValDefinition(5, 3, 0));
            _matrixValDefinitions.Add(RtDrlCmd, new MatrixValDefinition(11, 3, 0));
            _matrixValDefinitions.Add(LftDrlDimValue, new MatrixValDefinition(14, 7, 0));
            _matrixValDefinitions.Add(LftPrkLmpCmd, new MatrixValDefinition(17, 3, 0));
            _matrixValDefinitions.Add(RtDrlDimValue, new MatrixValDefinition(20, 7, 0));
            _matrixValDefinitions.Add(LftPrkLmpDimValue, new MatrixValDefinition(26, 7, 0));
            _matrixValDefinitions.Add(RtPrkLmpDimValue, new MatrixValDefinition(32, 7, 0));
            _matrixValDefinitions.Add(RtPrkLmpCmd, new MatrixValDefinition(39, 3, 0));
            _matrixValDefinitions.Add(RtFrtTrnLmpCmd, new MatrixValDefinition(42, 3, 0));
            _matrixValDefinitions.Add(LftFrtTrnLmpCmd, new MatrixValDefinition(45, 3, 0));

            _matrixValDefinitions.Add(LftLowBmCmdPv, new MatrixValDefinition(1, 2, 0));
            _matrixValDefinitions.Add(LftLowBmCmdArc, new MatrixValDefinition(3, 2, 0));
            _matrixValDefinitions.Add(LftLowBmCmd, new MatrixValDefinition(5, 3, 0));
            _matrixValDefinitions.Add(LftLowBmDimValue, new MatrixValDefinition(10, 7, 0));
            _matrixValDefinitions.Add(RtLowBmCmdPv, new MatrixValDefinition(19, 2, 0));
            _matrixValDefinitions.Add(RtLowBmCmdArc, new MatrixValDefinition(21, 2, 0));
            _matrixValDefinitions.Add(RtLowBmCmd, new MatrixValDefinition(23, 3, 0));
            _matrixValDefinitions.Add(LftHiBmCmd, new MatrixValDefinition(25, 3, 0));
            _matrixValDefinitions.Add(RtLowBmDimValue, new MatrixValDefinition(28, 7, 0));
            _matrixValDefinitions.Add(LftHiBmCmdPv, new MatrixValDefinition(37, 2, 0));
            _matrixValDefinitions.Add(LftHiBmCmdArc, new MatrixValDefinition(39, 2, 0));
            _matrixValDefinitions.Add(RtHiBmCmdArc, new MatrixValDefinition(41, 2, 0));
            _matrixValDefinitions.Add(RtHiBmCmd, new MatrixValDefinition(43, 3, 0));
            _matrixValDefinitions.Add(LftHiBmDimValue, new MatrixValDefinition(46, 7, 0));
            _matrixValDefinitions.Add(RtHiBmDimValue, new MatrixValDefinition(48, 7, 0));
            _matrixValDefinitions.Add(RtHiBmCmdPv, new MatrixValDefinition(55, 2, 0));

            _matrixValDefinitions.Add(LftBndDim, new MatrixValDefinition(0, 8, 0));
            _matrixValDefinitions.Add(RtBndDim, new MatrixValDefinition(8, 8, 0));

            _matrixValDefinitions.Add(HbMdSelt, new MatrixValDefinition(0, 3, 0));

            _matrixValDefinitions.Add(FrtLtShwMode, new MatrixValDefinition(3, 5, 0));
            _matrixValDefinitions.Add(FrtLtShwType, new MatrixValDefinition(0, 3, 0));

            _matrixValDefinitions.Add(VedMssgCntr, new MatrixValDefinition(8, 4, 0));
            _matrixValDefinitions.Add(HlsMssgCntr, new MatrixValDefinition(8, 4, 0));

            #endregion

            LbOff();
            //_motorolaMatrix0X164.MatrixData = new byte[8] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
        }

        private readonly Dictionary<string, MatrixValDefinition> _matrixValDefinitions =
            new Dictionary<string, MatrixValDefinition>();
        private readonly CanCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X186 =
            new CanCommunicationMatrix.MotorolaMatrix(0x186, 6);
        private readonly CanCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X187 =
            new CanCommunicationMatrix.MotorolaMatrix(0x187, 7);
        private readonly CanCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X203 =
            new CanCommunicationMatrix.MotorolaMatrix(0x203, 3);
        private readonly CanCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X204 =
            new CanCommunicationMatrix.MotorolaMatrix(0x204, 2);
        private readonly CanCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X189 =
            new CanCommunicationMatrix.MotorolaMatrix(0x189, 2);
        private readonly CanCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X18A =
            new CanCommunicationMatrix.MotorolaMatrix(0x18A, 4);
        private readonly CanCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X2F9 =
            new CanCommunicationMatrix.MotorolaMatrix(0x2F9, 1);
        private readonly CanCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X201 =
            new CanCommunicationMatrix.MotorolaMatrix(0x201, 5);
        private readonly CanCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X164 =
            new CanCommunicationMatrix.MotorolaMatrix(0x164, 8);
        private readonly CanCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X20D =
            new CanCommunicationMatrix.MotorolaMatrix(0x20D, 6);
        private readonly CanCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X1Ff =
            new CanCommunicationMatrix.MotorolaMatrix(0x1FF, 8);

        private readonly CanCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X638 =
            new CanCommunicationMatrix.MotorolaMatrix(0x638, 8);

        private int _vedBzCount;
        private int _hlsBzCount;

        private void MainWork()
        {
            _matrixValDefinitions[LftDrlCmd].Value = 0x02;
            _matrixValDefinitions[RtDrlCmd].Value = 0x02;

            // 100ms
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = SendTask(new Func<CanBus.CanDataPackage[]>(() =>
                {
                    var sendList = new List<CanBus.CanDataPackage>();

                    _motorolaMatrix0X186.UpdateData(_matrixValDefinitions[LftDrlCmd]);
                    _motorolaMatrix0X186.UpdateData(_matrixValDefinitions[RtDrlCmd]);
                    _motorolaMatrix0X186.UpdateData(_matrixValDefinitions[LftDrlDimValue]);
                    _motorolaMatrix0X186.UpdateData(_matrixValDefinitions[LftPrkLmpCmd]);
                    _motorolaMatrix0X186.UpdateData(_matrixValDefinitions[RtDrlDimValue]);
                    _motorolaMatrix0X186.UpdateData(_matrixValDefinitions[LftPrkLmpDimValue]);
                    _motorolaMatrix0X186.UpdateData(_matrixValDefinitions[RtPrkLmpDimValue]);
                    _motorolaMatrix0X186.UpdateData(_matrixValDefinitions[RtPrkLmpCmd]);
                    _motorolaMatrix0X186.UpdateData(_matrixValDefinitions[RtFrtTrnLmpCmd]);
                    _motorolaMatrix0X186.UpdateData(_matrixValDefinitions[LftFrtTrnLmpCmd]);

                    _motorolaMatrix0X187.UpdateData(_matrixValDefinitions[LftLowBmCmdPv]);
                    _motorolaMatrix0X187.UpdateData(_matrixValDefinitions[LftLowBmCmdArc]);
                    _motorolaMatrix0X187.UpdateData(_matrixValDefinitions[LftLowBmCmd]);
                    _motorolaMatrix0X187.UpdateData(_matrixValDefinitions[LftLowBmDimValue]);
                    _motorolaMatrix0X187.UpdateData(_matrixValDefinitions[RtLowBmCmdPv]);
                    _motorolaMatrix0X187.UpdateData(_matrixValDefinitions[RtLowBmCmdArc]);
                    _motorolaMatrix0X187.UpdateData(_matrixValDefinitions[RtLowBmCmd]);
                    _motorolaMatrix0X187.UpdateData(_matrixValDefinitions[LftHiBmCmd]);
                    _motorolaMatrix0X187.UpdateData(_matrixValDefinitions[RtLowBmDimValue]);
                    _motorolaMatrix0X187.UpdateData(_matrixValDefinitions[LftHiBmCmdPv]);
                    _motorolaMatrix0X187.UpdateData(_matrixValDefinitions[LftHiBmCmdArc]);
                    _motorolaMatrix0X187.UpdateData(_matrixValDefinitions[RtHiBmCmdArc]);
                    _motorolaMatrix0X187.UpdateData(_matrixValDefinitions[RtHiBmCmd]);
                    _motorolaMatrix0X187.UpdateData(_matrixValDefinitions[LftHiBmDimValue]);
                    _motorolaMatrix0X187.UpdateData(_matrixValDefinitions[RtHiBmDimValue]);
                    _motorolaMatrix0X187.UpdateData(_matrixValDefinitions[RtHiBmCmdPv]);

                    _matrixValDefinitions[FrtLtShwMode].Value = FrtLtShwModeValue;
                    _matrixValDefinitions[FrtLtShwType].Value = FrtLtShwTypeValue;
                    _motorolaMatrix0X189.UpdateData(_matrixValDefinitions[FrtLtShwMode]);
                    _motorolaMatrix0X189.UpdateData(_matrixValDefinitions[FrtLtShwType]);

                    _matrixValDefinitions[VedMssgCntr].Value = (byte)_vedBzCount;
                    _vedBzCount++;
                    if (_vedBzCount > 15)
                        _vedBzCount = 0;
                    _motorolaMatrix0X20D.UpdateData(_matrixValDefinitions[VedMssgCntr]);

                    // LowBeam
                    {
                        var lbPv = _lbIndex;
                        var lbArc = 3 - _lbIndex;

                        //_matrixValDefinitions[LftLowBmCmdPv].Value = (byte)lbPv;
                        //_matrixValDefinitions[RtLowBmCmdPv].Value = (byte)lbPv;

                        //_matrixValDefinitions[LftLowBmCmdArc].Value = (byte)lbArc;
                        //_matrixValDefinitions[RtLowBmCmdArc].Value = (byte)lbArc;

                        _matrixValDefinitions[LftLowBmCmdPv].Value = (byte)(3 - _lbIndex);
                        _matrixValDefinitions[RtLowBmCmdPv].Value = (byte)(3 - _lbIndex);

                        _matrixValDefinitions[LftLowBmCmdArc].Value = (byte)_lbIndex;
                        _matrixValDefinitions[RtLowBmCmdArc].Value = (byte)_lbIndex;

                        _lbIndex++;
                        if (_lbIndex > 3)
                            _lbIndex = 0;
                    }

                    // HighBeam
                    {
                        var hbPv = _hbIndex;
                        var hbArc = 3 - _hbIndex;

                        //_matrixValDefinitions[LftHiBmCmdPv].Value = (byte)hbPv;
                        //_matrixValDefinitions[RtHiBmCmdPv].Value = (byte)hbPv;

                        //_matrixValDefinitions[LftHiBmCmdArc].Value = (byte)hbArc;
                        //_matrixValDefinitions[RtHiBmCmdArc].Value = (byte)hbArc;

                        _matrixValDefinitions[LftHiBmCmdPv].Value = (byte)(3 - _hbIndex);
                        _matrixValDefinitions[RtHiBmCmdPv].Value = (byte)(3 - _hbIndex);

                        _matrixValDefinitions[LftHiBmCmdArc].Value = (byte)_hbIndex;
                        _matrixValDefinitions[RtHiBmCmdArc].Value = (byte)_hbIndex;

                        _hbIndex++;
                        if (_hbIndex > 3)
                            _hbIndex = 0;
                    }

                    sendList.AddRange(
                        new[]
                        {
                            new CanBus.CanDataPackage(_motorolaMatrix0X186.CanId, CanBus.CanProtocol.Can,
                                CanBus.CanType.Standard, CanBus.CanFormat.Data, _motorolaMatrix0X186.MatrixData),
                            new CanBus.CanDataPackage(_motorolaMatrix0X187.CanId, CanBus.CanProtocol.Can,
                                CanBus.CanType.Standard, CanBus.CanFormat.Data, _motorolaMatrix0X187.MatrixData),
                            new CanBus.CanDataPackage(_motorolaMatrix0X189.CanId, CanBus.CanProtocol.Can,
                                CanBus.CanType.Standard, CanBus.CanFormat.Data, _motorolaMatrix0X189.MatrixData),
                            new CanBus.CanDataPackage(_motorolaMatrix0X18A.CanId, CanBus.CanProtocol.Can,
                                CanBus.CanType.Standard, CanBus.CanFormat.Data, _motorolaMatrix0X18A.MatrixData),
                            new CanBus.CanDataPackage(_motorolaMatrix0X164.CanId, CanBus.CanProtocol.Can,
                                CanBus.CanType.Standard, CanBus.CanFormat.Data, _motorolaMatrix0X164.MatrixData),
                            new CanBus.CanDataPackage(_motorolaMatrix0X20D.CanId, CanBus.CanProtocol.Can,
                                CanBus.CanType.Standard, CanBus.CanFormat.Data, _motorolaMatrix0X20D.MatrixData),
                            new CanBus.CanDataPackage(_motorolaMatrix0X1Ff.CanId, CanBus.CanProtocol.Can,
                                CanBus.CanType.Standard, CanBus.CanFormat.Data, _motorolaMatrix0X1Ff.MatrixData)
                        });

                    return sendList.ToArray();
                })),
                Interval = 100
            });

            // 20ms
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = SendTask(new Func<CanBus.CanDataPackage[]>(() =>
                {
                    var sendList = new List<CanBus.CanDataPackage>();

                    _motorolaMatrix0X203.UpdateData(_matrixValDefinitions[HbMdSelt]);

                    _motorolaMatrix0X204.UpdateData(_matrixValDefinitions[RtBndDim]);
                    _motorolaMatrix0X204.UpdateData(_matrixValDefinitions[LftBndDim]);

                    _matrixValDefinitions[HlsMssgCntr].Value = (byte)_hlsBzCount;
                    _hlsBzCount++;
                    if (_hlsBzCount > 15)
                        _hlsBzCount = 0;
                    _motorolaMatrix0X201.UpdateData(_matrixValDefinitions[HlsMssgCntr]);

                    sendList.AddRange(
                        new[]
                        {
                            new CanBus.CanDataPackage(_motorolaMatrix0X203.CanId, CanBus.CanProtocol.Can,
                                CanBus.CanType.Standard, CanBus.CanFormat.Data, _motorolaMatrix0X203.MatrixData),
                            new CanBus.CanDataPackage(_motorolaMatrix0X204.CanId, CanBus.CanProtocol.Can,
                                CanBus.CanType.Standard, CanBus.CanFormat.Data, _motorolaMatrix0X204.MatrixData),
                            new CanBus.CanDataPackage(_motorolaMatrix0X201.CanId, CanBus.CanProtocol.Can,
                                CanBus.CanType.Standard, CanBus.CanFormat.Data, _motorolaMatrix0X201.MatrixData),
                        });
                    return sendList.ToArray();
                })),
                Interval = 20
            });

            // 250ms
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = SendTask(new Func<CanBus.CanDataPackage[]>(() =>
                {
                    var sendList = new List<CanBus.CanDataPackage>();

                    sendList.AddRange(
                        new[]
                        {
                            new CanBus.CanDataPackage(_motorolaMatrix0X638.CanId, CanBus.CanProtocol.Can,
                                CanBus.CanType.Standard, CanBus.CanFormat.Data, _motorolaMatrix0X638.MatrixData)
                        });
                    return sendList.ToArray();
                })),
                Interval = 250
            });

            // 1000ms
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = SendTask(new Func<CanBus.CanDataPackage[]>(() =>
                {
                    var sendList = new List<CanBus.CanDataPackage>();

                    sendList.AddRange(
                        new[]
                        {
                            new CanBus.CanDataPackage(_motorolaMatrix0X2F9.CanId, CanBus.CanProtocol.Can,
                                CanBus.CanType.Standard, CanBus.CanFormat.Data, _motorolaMatrix0X2F9.MatrixData)
                        });

                    return sendList.ToArray();
                })),
                Interval = 1000
            });

            //// 35ms
            //SetTimer(new TaskInfo
            //{
            //    Action = SendTask(new Func<CanBus.CanDataPackage[]>(() =>
            //    {
            //        var sendList = new List<CanBus.CanDataPackage>();

            //        if (_isLeft)
            //        {
            //            sendList.AddRange(
            //                new[]
            //                {
            //                    new CanBus.CanDataPackage(0x102, CanBus.CanProtocol.Can,
            //                        CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8]),
            //                    new CanBus.CanDataPackage(0x104, CanBus.CanProtocol.Can,
            //                        CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8]),
            //                    new CanBus.CanDataPackage(0x106, CanBus.CanProtocol.Can,
            //                        CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8]),
            //                    new CanBus.CanDataPackage(0x108, CanBus.CanProtocol.Can,
            //                        CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8]),
            //                    new CanBus.CanDataPackage(0x10A, CanBus.CanProtocol.Can,
            //                        CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8]),
            //                    new CanBus.CanDataPackage(0x10C, CanBus.CanProtocol.Can,
            //                        CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8]),
            //                    new CanBus.CanDataPackage(0x10E, CanBus.CanProtocol.Can,
            //                        CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8]),
            //                    new CanBus.CanDataPackage(0x110, CanBus.CanProtocol.Can,
            //                        CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8]),
            //                });
            //        }
            //        return sendList.ToArray();
            //    })),
            //    Interval = 35
            //});

            SchedulerAsync();
        }

        private Action SendTask(Func<CanBus.CanDataPackage[]> msgFunc)
        {
            return () =>
            {
                if (Can == null || msgFunc == null || !_isAwake)
                {
                    _vedBzCount = 0;
                    _hlsBzCount = 0;
                    return;
                }

                lock (_canSendLock)
                    Can.SendCanDatas(msgFunc.Invoke());
            };
        }

        #region LED相关公共函数

        [Description("R/W,FrtLtShwType")]
        public byte FrtLtShwTypeValue;

        [Description("R/W,FrtLtShwModeValue")]
        public byte FrtLtShwModeValue;

        [Description("唤醒")]
        public void LampAwake()
        {
            _isAwake = true;
        }

        [Description("休眠")]
        public void LampSleep()
        {
            _isAwake = false;
        }

        [Description("转向流水点亮")]
        public void SwipeTurnOn()
        {
            _matrixValDefinitions[LftFrtTrnLmpCmd].Value = 0x01;
            _matrixValDefinitions[RtFrtTrnLmpCmd].Value = 0x01;
        }

        [Description("转向整体点亮")]
        public void TurnHoldOnLight()
        {
            _matrixValDefinitions[LftFrtTrnLmpCmd].Value = 0x03;
            _matrixValDefinitions[RtFrtTrnLmpCmd].Value = 0x03;
        }

        [Description("转向熄灭")]
        public void TurnOff()
        {
            _matrixValDefinitions[LftFrtTrnLmpCmd].Value = 0x00;
            _matrixValDefinitions[RtFrtTrnLmpCmd].Value = 0x00;
        }

        [Description("日行灯点亮")]
        public void DrlOn()
        {
            _matrixValDefinitions[LftDrlCmd].Value = 0x01;
            _matrixValDefinitions[RtDrlCmd].Value = 0x01;
        }

        [Description("DRL日行灯熄灭")]
        public void DrlOff()
        {
            _matrixValDefinitions[LftDrlCmd].Value = 0x02;
            _matrixValDefinitions[RtDrlCmd].Value = 0x02;
        }

        [Description("位置灯点亮")]
        public void PlOn()
        {
            _matrixValDefinitions[LftPrkLmpCmd].Value = 0x01;
            _matrixValDefinitions[RtPrkLmpCmd].Value = 0x01;
        }

        [Description("位置灯熄灭")]
        public void PlOff()
        {
            _matrixValDefinitions[LftPrkLmpCmd].Value = 0x02;
            _matrixValDefinitions[LftPrkLmpCmd].Value = 0x02;
        }

        private int _lbIndex;

        [Description("近光灯点亮")]
        public void LbOn()
        {
            _matrixValDefinitions[LftLowBmCmd].Value = 0x01;
            _matrixValDefinitions[RtLowBmCmd].Value = 0x01;
        }

        [Description("近光灯熄灭")]
        public void LbOff()
        {
            _matrixValDefinitions[LftLowBmCmd].Value = 0x02;
            _matrixValDefinitions[RtLowBmCmd].Value = 0x02;
        }

        private int _hbIndex;

        [Description("远光灯点亮")]
        public void HbOn()
        {
            _matrixValDefinitions[HbMdSelt].Value = 0x01;
            _matrixValDefinitions[LftHiBmCmd].Value = 0x01;
            _matrixValDefinitions[RtHiBmCmd].Value = 0x01;
        }

        [Description("远光灯熄灭")]
        public void HbOff()
        {
            _matrixValDefinitions[HbMdSelt].Value = 0x00;
            _matrixValDefinitions[LftHiBmCmd].Value = 0x00;
            _matrixValDefinitions[RtHiBmCmd].Value = 0x00;
        }

        [Description("角灯点亮")]
        public void CorneringOn()
        {
            _matrixValDefinitions[LftBndDim].Value = 0xC8;
            _matrixValDefinitions[RtBndDim].Value = 0xC8;
        }

        [Description("角灯熄灭")]
        public void CorneringOff()
        {
            _matrixValDefinitions[LftBndDim].Value = 0x00;
            _matrixValDefinitions[RtBndDim].Value = 0x00;
        }

        private const string LftDrlCmd = "LftDrlCmd";
        private const string RtDrlCmd = "RtDRLCmd";
        private const string LftDrlDimValue = "LftDRLDimValue";
        private const string LftPrkLmpCmd = "LftPrkLmpCmd";
        private const string RtDrlDimValue = "RtDRLDimValue";
        private const string LftPrkLmpDimValue = "LftPrkLmpDimValue";
        private const string RtPrkLmpDimValue = "RtPrkLmpDimValue";
        private const string RtPrkLmpCmd = "RtPrkLmpCmd";
        private const string RtFrtTrnLmpCmd = "RtFrtTrnLmpCmd";
        private const string LftFrtTrnLmpCmd = "LftFrtTrnLmpCmd";

        private const string LftLowBmCmdPv = "LftLowBmCmdPV";
        private const string LftLowBmCmdArc = "LftLowBmCmdARC";
        private const string LftLowBmCmd = "LftLowBmCmd";
        private const string LftLowBmDimValue = "LftLowBmDimValue";
        private const string RtLowBmCmdPv = "RtLowBmCmdPV";
        private const string RtLowBmCmdArc = "RtLowBmCmdARC";
        private const string RtLowBmCmd = "RtLowBmCmd";
        private const string LftHiBmCmd = "LftHiBmCmd";
        private const string RtLowBmDimValue = "RtLowBmDimValue";
        private const string LftHiBmCmdPv = "LftHiBmCmdPV";
        private const string LftHiBmCmdArc = "LftHiBmCmdARC";
        private const string RtHiBmCmdArc = "RtHiBmCmdARC";
        private const string RtHiBmCmd = "RtHiBmCmd";
        private const string LftHiBmDimValue = "LftHiBmDimValue";
        private const string RtHiBmDimValue = "RtHiBmDimValue";
        private const string RtHiBmCmdPv = "RtHiBmCmdPV";

        private const string LftBndDim = "LftBndDim";
        private const string RtBndDim = "RtBndDim";

        private const string HbMdSelt = "HbMdSelt";

        private const string FrtLtShwMode = "FrtLtShwMode";
        private const string FrtLtShwType = "FrtLtShwType";

        private const string VedMssgCntr = "VED_MssgCntr";
        private const string HlsMssgCntr = "HLS_MssgCntr";

        #endregion
    }
}
