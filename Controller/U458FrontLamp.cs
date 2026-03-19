using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Controller
{
    [Description("CAN-Product,U458高配前灯")]
    public sealed class U458FrontLamp : ControllerBase
    {
        public CanBus Can;
        
        public U458FrontLamp(string name)
            : base(name)
        {
            InitSignals();
            MainWork();
        }

        ~U458FrontLamp()
        {
            Dispose();
        }

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

            _matrixValDefinitions.Add(HbMdSelt, new MatrixValDefinition(5, 3, 0));

            _matrixValDefinitions.Add(FrtLtShwMode, new MatrixValDefinition(3, 5, 0));
            _matrixValDefinitions.Add(FrtLtShwType, new MatrixValDefinition(0, 3, 0));

            #endregion
        }

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

                    sendList.AddRange(
                        new[]
                        {
                            new CanBus.CanDataPackage(_motorolaMatrix0X186.CanId, CanBus.CanProtocol.Can,
                                CanBus.CanType.Standard, CanBus.CanFormat.Data, _motorolaMatrix0X186.MatrixData),
                            new CanBus.CanDataPackage(_motorolaMatrix0X187.CanId, CanBus.CanProtocol.Can,
                                CanBus.CanType.Standard, CanBus.CanFormat.Data, _motorolaMatrix0X187.MatrixData),
                            new CanBus.CanDataPackage(_motorolaMatrix0X189.CanId, CanBus.CanProtocol.Can,
                                CanBus.CanType.Standard, CanBus.CanFormat.Data, _motorolaMatrix0X189.MatrixData)
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

                    sendList.AddRange(
                        new[]
                        {
                            new CanBus.CanDataPackage(_motorolaMatrix0X203.CanId, CanBus.CanProtocol.Can,
                                CanBus.CanType.Standard, CanBus.CanFormat.Data, _motorolaMatrix0X203.MatrixData),
                            new CanBus.CanDataPackage(_motorolaMatrix0X204.CanId, CanBus.CanProtocol.Can,
                                CanBus.CanType.Standard, CanBus.CanFormat.Data, _motorolaMatrix0X204.MatrixData)
                        });
                    return sendList.ToArray();
                })),
                Interval = 20
            });

            SchedulerAsync();
        }

        private Action SendTask(Func<CanBus.CanDataPackage[]> msgFunc)
        {
            return () =>
            {
                if (Can == null || msgFunc == null || !_isAwake)
                    return;

                lock (_canSendLock)
                    Can.SendCanDatas(msgFunc.Invoke());
            };
        }

        #region LED相关公共函数

        private readonly Dictionary<string, MatrixValDefinition> _matrixValDefinitions =
            new Dictionary<string, MatrixValDefinition>();
        private readonly CanCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X186 =
            new CanCommunicationMatrix.MotorolaMatrix(0x186, 7);
        private readonly CanCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X187 =
            new CanCommunicationMatrix.MotorolaMatrix(0x187, 7);
        private readonly CanCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X203 =
            new CanCommunicationMatrix.MotorolaMatrix(0x203, 3);
        private readonly CanCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X204 =
            new CanCommunicationMatrix.MotorolaMatrix(0x204, 6);
        private readonly CanCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X189 =
            new CanCommunicationMatrix.MotorolaMatrix(0x189, 6);

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

        [Description("近光灯点亮")]
        public void LbOn()
        {
            _matrixValDefinitions[LftLowBmCmd].Value = 0x01;
            _matrixValDefinitions[RtLowBmCmd].Value = 0x01;
        }

        [Description("近光灯熄灭")]
        public void LbOff()
        {
            _matrixValDefinitions[LftLowBmCmd].Value = 0x00;
            _matrixValDefinitions[RtLowBmCmd].Value = 0x00;
        }

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

        #endregion
    }
}
