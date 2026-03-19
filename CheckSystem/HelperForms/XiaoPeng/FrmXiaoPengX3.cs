using CommonUtility;
using CommonUtility.BusLoader;
using Controller;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CheckSystem.HelperForms.XiaoPeng
{
    /*
           前灯：
           实验模式1：HI、Low、Turn(200ms流水；200ms ON；400ms OFF)、Pos
           实验模式2：DRL

           尾灯：
           实验模式1：位置灯点亮，其他灯熄灭
           实验模式2：转向灯400ms开关，其他灯熄灭
           实验模式3：转向灯200ms流水，200常亮，其他灯熄灭

           CDCU：
           前灯：白光/黄光
           尾灯：红光/黄光
         */

    public partial class FrmXiaoPengX3 : UIForm
    {
        private CanBus Can;

        //private readonly SyControllerWith56Pin _controllerWith56Pin = new SyControllerWith56Pin("can");
        private readonly SyRenesasMcuControllerMaster _controllerWith56Pin = new SyRenesasMcuControllerMaster("can");

        private long _lastFrontLampLTs;
        private long _lastFrontLampMTs;
        private long _lastFrontLampRTs;

        private bool _isFrontLConnect;
        private bool _isFrontMConnect;
        private bool _isFrontRConnect;

        private long _lastRearLampLTs;
        private long _lastRearLampRTs;

        private bool _isRearLConnect;
        private bool _isRearRConnect;

        public FrmXiaoPengX3()
        {
            InitializeComponent();
            cmbDrl.SelectedIndex = 0;
            cmbMode.SelectedIndex = 0;
            Load += FrmXiaoPengX3_Load;
        }

        private void FrmXiaoPengX3_Load(object sender, EventArgs e)
        {
            try
            {
                _controllerWith56Pin.InitRemoteIpAddress("192.168.1.28:8088");
                if (_controllerWith56Pin.GatewayCan1 != null)
                {
                    Can = _controllerWith56Pin.GatewayCan1;
                    //_x3FrontLamp.Can = Can;
                    //_x3RearLamp.Can = Can;
                    CanBus.PushCanMsg += CanBus_PushCanMsg;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            finally
            {
                //_x3FrontLamp.IsLConnect = true;
                //_x3FrontLamp.IsMConnect = true;
                //_x3FrontLamp.IsRConnect = true;

                //_x3RearLamp.IsLConnect = true;
                //_x3RearLamp.IsRConnect = true;

                //_x3FrontLamp.StartCanMsg();
                //_x3RearLamp.StartCanMsg();

                MainWork();

                swHb.ActiveChanged += SwHb_ActiveChanged;
                swLb.ActiveChanged += SwLb_ActiveChanged;
                swPos.ActiveChanged += SwPos_ActiveChanged;
                cmbDrl.SelectedIndexChanged += CmbDrl_SelectedIndexChanged;
                swTurnL.ActiveChanged += SwTurnL_ActiveChanged;
                swTurnR.ActiveChanged += SwTurnR_ActiveChanged;
                swTurnOnOffEnable.ActiveChanged += SwTurnOnOffEnable_ActiveChanged;
                swTurnFlashEnable.ActiveChanged += SwTurnFlashEnable_ActiveChanged;
                swCdcuControl.ActiveChanged += SwCdcuControl_ActiveChanged;

                txtFrontWhite.ValueChanged += TxtFrontWhite_ValueChanged;
                txtFrontYellow.ValueChanged += TxtFrontYellow_ValueChanged;
                txtMiddleWhite.ValueChanged += TxtMiddleWhite_ValueChanged;
                txtRearRed.ValueChanged += TxtRearRed_ValueChanged;
                txtRearYellow.ValueChanged += TxtRearYellow_ValueChanged;

                cmbMode.SelectedIndexChanged += CmbMode_SelectedIndexChanged; ;
            }
        }

        private void CanBus_PushCanMsg(
            string name, CanBus.CanDataPackage data, CanBus.OnPushCanDataType onPushCanDataType)
        {
            if (Can != null && name == Can.Name && (onPushCanDataType == CanBus.OnPushCanDataType.FilterRx || onPushCanDataType == CanBus.OnPushCanDataType.NotFilterRx))
            {
                if (data.CanId == 0x3D1 || data.CanId == 0x3D7)
                    _lastFrontLampLTs = HighPrecisionTimer.GetTimestamp();

                if (data.CanId == 0x3DB || data.CanId == 0x3DC)
                    _lastFrontLampMTs = HighPrecisionTimer.GetTimestamp();

                if (data.CanId == 0x3D2 || data.CanId == 0x3D8)
                    _lastFrontLampRTs = HighPrecisionTimer.GetTimestamp();

                if (data.CanId == 0x3D3 || data.CanId == 0x3D9)
                    _lastRearLampLTs = HighPrecisionTimer.GetTimestamp();

                if (data.CanId == 0x3D6 || data.CanId == 0x3DA)
                    _lastRearLampRTs = HighPrecisionTimer.GetTimestamp();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_lastFrontLampLTs == 0)
            {
                _isFrontLConnect = false;
            }
            else
            {
                var nowTs = HighPrecisionTimer.GetTimestamp();
                var ts = HighPrecisionTimer.GetTimestampIntervalMs(_lastFrontLampLTs, nowTs);
                _isFrontLConnect = !(ts > 5000);
            }

            if (_lastFrontLampMTs == 0)
            {
                _isFrontMConnect = false;
            }
            else
            {
                var nowTs = HighPrecisionTimer.GetTimestamp();
                var ts = HighPrecisionTimer.GetTimestampIntervalMs(_lastFrontLampMTs, nowTs);
                _isFrontMConnect = !(ts > 3500);
            }

            if (_lastFrontLampRTs == 0)
            {
                _isFrontRConnect = false;
            }
            else
            {
                var nowTs = HighPrecisionTimer.GetTimestamp();
                var ts = HighPrecisionTimer.GetTimestampIntervalMs(_lastFrontLampRTs, nowTs);
                _isFrontRConnect = !(ts > 3500);
            }

            if (_lastRearLampLTs == 0)
            {
                _isRearLConnect = false;
            }
            else
            {
                var nowTs = HighPrecisionTimer.GetTimestamp();
                var ts = HighPrecisionTimer.GetTimestampIntervalMs(_lastRearLampLTs, nowTs);
                _isRearLConnect = !(ts > 3500);
            }

            if (_lastRearLampRTs == 0)
            {
                _isRearRConnect = false;
            }
            else
            {
                var nowTs = HighPrecisionTimer.GetTimestamp();
                var ts = HighPrecisionTimer.GetTimestampIntervalMs(_lastRearLampRTs, nowTs);
                _isRearRConnect = !(ts > 3500);
            }

            BeginInvoke(new Action(() =>
            {
                ledChll.State = _isFrontLConnect ? UILightState.On : UILightState.Off;
                ledChlm.State = _isFrontMConnect ? UILightState.On : UILightState.Off;
                ledChlr.State = _isFrontRConnect ? UILightState.On : UILightState.Off;

                ledRcll.State = _isRearLConnect ? UILightState.On : UILightState.Off;
                ledRclr.State = _isRearRConnect ? UILightState.On : UILightState.Off;
            }));
        }

        #region 任务管理

        private readonly MyTaskScheduler _taskScheduler = new MyTaskScheduler();

        /// <summary>
        /// 初始化任务
        /// </summary>
        /// <param name="taskInfo"></param>
        protected void SetTimer(MyTaskScheduler.TaskInfo taskInfo)
        {
            _taskScheduler.SetTimer(taskInfo);
        }

        /// <summary>
        /// 启动任务
        /// </summary>
        protected void SchedulerAsync()
        {
            _taskScheduler.SchedulerAsync();
        }

        #endregion

        #region 协议打包

        private LampMode _lampMode = LampMode.Normal;
        private long _modeTurnTs;

        private readonly CanCommunicationMatrix.MotorolaMatrix _data0X269 = new CanCommunicationMatrix.MotorolaMatrix(0x269, 8);
        private readonly CanCommunicationMatrix.MotorolaMatrix _dataModeControl = new CanCommunicationMatrix.MotorolaMatrix(0x12B, 8);

        private readonly List<CanCommunicationMatrix.MotorolaMatrix> _dataFrontLightsL = new List<CanCommunicationMatrix.MotorolaMatrix>();
        private readonly List<CanCommunicationMatrix.MotorolaMatrix> _dataFrontLightsM = new List<CanCommunicationMatrix.MotorolaMatrix>();
        private readonly List<CanCommunicationMatrix.MotorolaMatrix> _dataFrontLightsR = new List<CanCommunicationMatrix.MotorolaMatrix>();

        private readonly List<CdcuControlLight> _frontYellowLightsL = new List<CdcuControlLight>();
        private readonly List<CdcuControlLight> _frontWhiteLightsL = new List<CdcuControlLight>();

        private readonly List<CdcuControlLight> _frontYellowLightsR = new List<CdcuControlLight>();
        private readonly List<CdcuControlLight> _frontWhiteLightsR = new List<CdcuControlLight>();

        private readonly List<CdcuControlLight> _middleWhiteLightsM = new List<CdcuControlLight>();

        private readonly List<CanCommunicationMatrix.MotorolaMatrix> _dataRearLightsL = new List<CanCommunicationMatrix.MotorolaMatrix>();
        private readonly List<CanCommunicationMatrix.MotorolaMatrix> _dataRearLightsR = new List<CanCommunicationMatrix.MotorolaMatrix>();

        private readonly List<CdcuControlLight> _rearRedLightsL = new List<CdcuControlLight>();
        private readonly List<CdcuControlLight> _rearYellowLightsL = new List<CdcuControlLight>();

        private readonly List<CdcuControlLight> _rearRedLightsR = new List<CdcuControlLight>();
        private readonly List<CdcuControlLight> _rearYellowLightsR = new List<CdcuControlLight>();

        internal class CdcuControlLight
        {
            public int Index;
            public uint BaseCanId;
            public int Value;
            public int StartBit;
        }

        internal enum LampMode
        {
            Normal,

            Mode1,

            Mode2,

            Mode3,
        }

        private void MainWork()
        {
            InitPackage();

            var sendCount = 0;

            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = () =>
                {
                    if (Can != null)
                    {
                        var sendDatas = new List<CanBus.CanDataPackage>();

                        switch (_lampMode)
                        {
                            case LampMode.Normal:
                                _data0X269.UpdateData(new MatrixValDefinition(37, 1, _lb));
                                _data0X269.UpdateData(new MatrixValDefinition(47, 1, _hb));
                                _data0X269.UpdateData(new MatrixValDefinition(61, 3, _drl));
                                _data0X269.UpdateData(new MatrixValDefinition(39, 1, _pos));
                                _data0X269.UpdateData(new MatrixValDefinition(45, 1, _turnFlashEnable));

                                if (_isTurnOnOffEnable)
                                {
                                    var nowTs = HighPrecisionTimer.GetTimestamp();
                                    var ms = HighPrecisionTimer.GetTimestampIntervalMs(_turnOnOffTs, nowTs);

                                    if (ms > 0 && ms <= 400)
                                    {
                                        if (_turnL > 0)
                                            _data0X269.UpdateData(new MatrixValDefinition(6, 2, _turnL));

                                        if (_turnR > 0)
                                            _data0X269.UpdateData(new MatrixValDefinition(4, 2, _turnR));

                                        if (_turnFlashEnable>0)
                                            _data0X269.UpdateData(new MatrixValDefinition(45, 1, _turnFlashEnable));
                                    }
                                    else if (ms > 400 && ms <= 800)
                                    {
                                        if (_turnL > 0)
                                            _data0X269.UpdateData(new MatrixValDefinition(6, 2, 0));

                                        if (_turnR > 0)
                                            _data0X269.UpdateData(new MatrixValDefinition(4, 2, 0));

                                        if (_turnFlashEnable > 0)
                                            _data0X269.UpdateData(new MatrixValDefinition(45, 1, 0));
                                    }
                                    else
                                    {
                                        _turnOnOffTs = HighPrecisionTimer.GetTimestamp();
                                    }
                                }
                                else
                                {
                                    if (_turnL > 0)
                                        _data0X269.UpdateData(new MatrixValDefinition(6, 2, _turnL));
                                    else
                                        _data0X269.UpdateData(new MatrixValDefinition(6, 2, 0));

                                    if (_turnR > 0)
                                        _data0X269.UpdateData(new MatrixValDefinition(4, 2, _turnR));
                                    else
                                        _data0X269.UpdateData(new MatrixValDefinition(4, 2, 0));
                                }

                                sendDatas.Add(new CanBus.CanDataPackage(_data0X269.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, _data0X269.MatrixData));

                                _dataModeControl.UpdateData(new MatrixValDefinition(2, 1, (byte)(_isCdcuControl ? 1 : 0)));
                                _dataModeControl.UpdateData(new MatrixValDefinition(3, 1, (byte)(_isCdcuControl ? 1 : 0)));
                                _dataModeControl.UpdateData(new MatrixValDefinition(4, 1, (byte)(_isCdcuControl ? 1 : 0)));
                                _dataModeControl.UpdateData(new MatrixValDefinition(5, 1, (byte)(_isCdcuControl ? 1 : 0)));
                                _dataModeControl.UpdateData(new MatrixValDefinition(6, 1, (byte)(_isCdcuControl ? 1 : 0)));
                                sendDatas.Add(new CanBus.CanDataPackage(_dataModeControl.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, _dataModeControl.MatrixData));

                                if (_isRearLConnect || _isRearRConnect)
                                    sendDatas.Add(new CanBus.CanDataPackage(0x2A7, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, _isCdcuControl ? (byte)0x00 : (byte)0x10, 0x00, 0x00, 0x00, 0x00, 0x00 }));

                                if (_isCdcuControl)
                                {
                                    if (sendCount == 0 && _isFrontLConnect) // 左前
                                    {
                                        // 左前白
                                        {
                                            foreach (var t in _frontWhiteLightsL)
                                                t.Value = _frontCdcuWhite;
                                            foreach (var t in _frontWhiteLightsL)
                                            {
                                                var findLMatrix = _dataFrontLightsL.Find(f => f.CanId == t.BaseCanId);
                                                if (findLMatrix == null)
                                                    continue;
                                                findLMatrix.UpdateData(new MatrixValDefinition(t.StartBit, 8, (byte)t.Value));
                                            }
                                        }

                                        // 左前黄
                                        {
                                            foreach (var t in _frontYellowLightsL)
                                                t.Value = _frontCdcuYellow;
                                            foreach (var t in _frontYellowLightsL)
                                            {
                                                var findLMatrix = _dataFrontLightsL.Find(f => f.CanId == t.BaseCanId);
                                                if (findLMatrix == null)
                                                    continue;
                                                findLMatrix.UpdateData(new MatrixValDefinition(t.StartBit, 8, (byte)t.Value));
                                            }
                                        }

                                        sendDatas.AddRange(_dataFrontLightsL.Select(t => new CanBus.CanDataPackage(t.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, t.MatrixData)));
                                    }
                                    else if (sendCount == 1 && _isFrontRConnect) // 右前
                                    {
                                        // 右前白
                                        {
                                            foreach (var t in _frontWhiteLightsR)
                                                t.Value = _frontCdcuWhite;
                                            foreach (var t in _frontWhiteLightsR)
                                            {
                                                var findLMatrix = _dataFrontLightsR.Find(f => f.CanId == t.BaseCanId);
                                                if (findLMatrix == null)
                                                    continue;
                                                findLMatrix.UpdateData(new MatrixValDefinition(t.StartBit, 8, (byte)t.Value));
                                            }
                                        }

                                        // 右前黄
                                        {
                                            foreach (var t in _frontYellowLightsR)
                                                t.Value = _frontCdcuYellow;
                                            foreach (var t in _frontYellowLightsR)
                                            {
                                                var findRMatrix = _dataFrontLightsR.Find(f => f.CanId == t.BaseCanId);
                                                if (findRMatrix == null)
                                                    continue;
                                                findRMatrix.UpdateData(new MatrixValDefinition(t.StartBit, 8, (byte)t.Value));
                                            }
                                        }

                                        sendDatas.AddRange(_dataFrontLightsR.Select(t => new CanBus.CanDataPackage(t.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, t.MatrixData)));
                                    }
                                    else if (sendCount == 2 && _isRearLConnect) // 左后
                                    {
                                        // 左后红
                                        {
                                            foreach (var t in _rearRedLightsL)
                                                t.Value = _rearCdcuRed;
                                            foreach (var t in _rearRedLightsL)
                                            {
                                                var findLMatrix = _dataRearLightsL.Find(f => f.CanId == t.BaseCanId);
                                                if (findLMatrix == null)
                                                    continue;
                                                findLMatrix.UpdateData(new MatrixValDefinition(t.StartBit, 4, (byte)t.Value));
                                            }
                                        }

                                        // 左后黄
                                        {
                                            foreach (var t in _rearYellowLightsL)
                                                t.Value = _rearCdcuYellow;
                                            foreach (var t in _rearYellowLightsL)
                                            {
                                                var findLMatrix = _dataRearLightsL.Find(f => f.CanId == t.BaseCanId);
                                                if (findLMatrix == null)
                                                    continue;
                                                findLMatrix.UpdateData(new MatrixValDefinition(t.StartBit, 4, (byte)t.Value));
                                            }
                                        }

                                        sendDatas.AddRange(_dataRearLightsL.Select(t => new CanBus.CanDataPackage(t.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, t.MatrixData)));
                                    }
                                    else if (sendCount == 3 && _isRearRConnect) // 右后
                                    {
                                        // 右后红
                                        {
                                            foreach (var t in _rearRedLightsR)
                                                t.Value = _rearCdcuRed;
                                            foreach (var t in _rearRedLightsR)
                                            {
                                                var findLMatrix = _dataRearLightsR.Find(f => f.CanId == t.BaseCanId);
                                                if (findLMatrix == null)
                                                    continue;
                                                findLMatrix.UpdateData(new MatrixValDefinition(t.StartBit, 4, (byte)t.Value));
                                            }
                                        }

                                        // 右后黄
                                        {
                                            foreach (var t in _rearYellowLightsR)
                                                t.Value = _rearCdcuYellow;
                                            foreach (var t in _rearYellowLightsR)
                                            {
                                                var findRMatrix = _dataRearLightsR.Find(f => f.CanId == t.BaseCanId);
                                                if (findRMatrix == null)
                                                    continue;
                                                findRMatrix.UpdateData(new MatrixValDefinition(t.StartBit, 4, (byte)t.Value));
                                            }
                                        }

                                        sendDatas.AddRange(_dataRearLightsR.Select(t => new CanBus.CanDataPackage(t.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, t.MatrixData)));
                                    }
                                    else if (sendCount == 4 && _isFrontMConnect) // 中灯
                                    {
                                        foreach (var t in _middleWhiteLightsM)
                                            t.Value = _middleCdcuWhite;

                                        foreach (var t in _middleWhiteLightsM)
                                        {
                                            var findRMatrix = _dataFrontLightsM.Find(f => f.CanId == t.BaseCanId);
                                            if (findRMatrix == null)
                                                continue;
                                            findRMatrix.UpdateData(new MatrixValDefinition(t.StartBit, 8, (byte)t.Value));
                                        }

                                        sendDatas.AddRange(_dataFrontLightsM.Select(t => new CanBus.CanDataPackage(t.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, t.MatrixData)));
                                    }
                                }
                                break;

                            case LampMode.Mode1:
                                {
                                    var nowTs = HighPrecisionTimer.GetTimestamp();
                                    var ms = HighPrecisionTimer.GetTimestampIntervalMs(_modeTurnTs, nowTs);
                                    if (ms > 0 && ms <= 400)
                                    {
                                        var tpMatrix = new CanCommunicationMatrix.MotorolaMatrix(0x269, 8);
                                        tpMatrix.UpdateData(new MatrixValDefinition(47, 1, 1)); // HI
                                        tpMatrix.UpdateData(new MatrixValDefinition(37, 1, 1)); // LOW
                                        tpMatrix.UpdateData(new MatrixValDefinition(39, 1, 1)); // 
                                        tpMatrix.UpdateData(new MatrixValDefinition(61, 3, 0));
                                        tpMatrix.UpdateData(new MatrixValDefinition(4, 2, 1));
                                        tpMatrix.UpdateData(new MatrixValDefinition(6, 2, 1));
                                        sendDatas.Add(new CanBus.CanDataPackage(tpMatrix.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, tpMatrix.MatrixData));
                                        sendDatas.Add(new CanBus.CanDataPackage(_dataModeControl.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8]));
                                    }
                                    else if (ms > 400 && ms <= 800)
                                    {
                                        var tpMatrix = new CanCommunicationMatrix.MotorolaMatrix(0x269, 8);
                                        tpMatrix.UpdateData(new MatrixValDefinition(47, 1, 1));
                                        tpMatrix.UpdateData(new MatrixValDefinition(37, 1, 1));
                                        tpMatrix.UpdateData(new MatrixValDefinition(39, 1, 1));
                                        tpMatrix.UpdateData(new MatrixValDefinition(61, 3, 0));
                                        tpMatrix.UpdateData(new MatrixValDefinition(4, 2, 0));
                                        tpMatrix.UpdateData(new MatrixValDefinition(6, 2, 0));

                                        sendDatas.Add(new CanBus.CanDataPackage(tpMatrix.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, tpMatrix.MatrixData));
                                        sendDatas.Add(new CanBus.CanDataPackage(_dataModeControl.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8]));
                                    }
                                    else
                                    {
                                        _modeTurnTs = HighPrecisionTimer.GetTimestamp();
                                    }
                                }
                                break;

                            case LampMode.Mode2:
                                break;

                            case LampMode.Mode3:
                                break;

                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        Can.SendCanDatas(sendDatas.ToArray());

                        sendCount++;
                        if (sendCount == 5)
                            sendCount = 0;
                    }
                },
                Interval = 5
            });

            SchedulerAsync();
        }

        private byte _lb;
        private byte _hb;
        private byte _drl;
        private byte _pos;
        private byte _turnL;
        private byte _turnR;
        private byte _turnFlashEnable;
        private bool _isTurnOnOffEnable;
        private long _turnOnOffTs;

        private bool _isCdcuControl;
        private int _frontCdcuWhite;
        private int _frontCdcuYellow;
        private int _middleCdcuWhite;
        private int _rearCdcuRed;
        private int _rearCdcuYellow;

        private void InitPackage()
        {
            // 前灯、中灯
            {
                for (var i = 1; i <= 8; i++)
                    _middleWhiteLightsM.Add(new CdcuControlLight { BaseCanId = 0x1b7, Index = i, Value = 0, StartBit = (i - 1) * 8 });
                for (var i = 9; i <= 16; i++)
                    _middleWhiteLightsM.Add(new CdcuControlLight { BaseCanId = 0x1b8, Index = i, Value = 0, StartBit = (i - 8 - 1) * 8 });
                for (var i = 17; i <= 24; i++)
                    _middleWhiteLightsM.Add(new CdcuControlLight { BaseCanId = 0x1b9, Index = i, Value = 0, StartBit = (i - 16 - 1) * 8 });
                for (var i = 25; i <= 32; i++)
                    _middleWhiteLightsM.Add(new CdcuControlLight { BaseCanId = 0x1ba, Index = i, Value = 0, StartBit = (i - 24 - 1) * 8 });
                for (var i = 33; i <= 40; i++)
                    _middleWhiteLightsM.Add(new CdcuControlLight { BaseCanId = 0x1bb, Index = i, Value = 0, StartBit = (i - 32 - 1) * 8 });
                for (var i = 41; i <= 48; i++)
                    _middleWhiteLightsM.Add(new CdcuControlLight { BaseCanId = 0x1bc, Index = i, Value = 0, StartBit = (i - 40 - 1) * 8 });
                for (var i = 49; i <= 56; i++)
                    _middleWhiteLightsM.Add(new CdcuControlLight { BaseCanId = 0x1bd, Index = i, Value = 0, StartBit = (i - 48 - 1) * 8 });
                for (var i = 57; i <= 64; i++)
                    _middleWhiteLightsM.Add(new CdcuControlLight { BaseCanId = 0x1be, Index = i, Value = 0, StartBit = (i - 56 - 1) * 8 });
                for (var i = 65; i <= 72; i++)
                    _middleWhiteLightsM.Add(new CdcuControlLight { BaseCanId = 0x1bf, Index = i, Value = 0, StartBit = (i - 64 - 1) * 8 });
                for (var i = 73; i <= 80; i++)
                    _middleWhiteLightsM.Add(new CdcuControlLight { BaseCanId = 0x1c1, Index = i, Value = 0, StartBit = (i - 72 - 1) * 8 });

                _middleWhiteLightsM.Add(new CdcuControlLight { BaseCanId = 0x1c2, Index = 81, Value = 0, StartBit = 0 * 8 });
                _middleWhiteLightsM.Add(new CdcuControlLight { BaseCanId = 0x1c2, Index = 82, Value = 0, StartBit = 1 * 8 });

                for (var i = 1; i <= 8; i++)
                {
                    _frontYellowLightsR.Add(new CdcuControlLight { BaseCanId = 0x192, Index = i, Value = 0, StartBit = (i - 1) * 8 });
                    _frontYellowLightsL.Add(new CdcuControlLight { BaseCanId = 0x18b, Index = i, Value = 0, StartBit = (i - 1) * 8 });
                }
                for (var i = 9; i <= 16; i++)
                {
                    _frontYellowLightsR.Add(new CdcuControlLight { BaseCanId = 0x193, Index = i, Value = 0, StartBit = (i - 8 - 1) * 8 });
                    _frontYellowLightsL.Add(new CdcuControlLight { BaseCanId = 0x18C, Index = i, Value = 0, StartBit = (i - 8 - 1) * 8 });
                }

                for (var i = 17; i <= 24; i++)
                {
                    _frontWhiteLightsR.Add(new CdcuControlLight { BaseCanId = 0x194, Index = i - 16, Value = 0, StartBit = (i - 16 - 1) * 8 });
                    _frontWhiteLightsL.Add(new CdcuControlLight { BaseCanId = 0x18D, Index = i - 16, Value = 0, StartBit = (i - 16 - 1) * 8 });
                }
                for (var i = 25; i <= 32; i++)
                {
                    _frontWhiteLightsR.Add(new CdcuControlLight { BaseCanId = 0x195, Index = i - 16, Value = 0, StartBit = (i - 24 - 1) * 8 });
                    _frontWhiteLightsL.Add(new CdcuControlLight { BaseCanId = 0x18E, Index = i - 16, Value = 0, StartBit = (i - 24 - 1) * 8 });
                }
                for (var i = 33; i <= 40; i++)
                {
                    _frontWhiteLightsR.Add(new CdcuControlLight { BaseCanId = 0x196, Index = i - 16, Value = 0, StartBit = (i - 32 - 1) * 8 });
                    _frontWhiteLightsL.Add(new CdcuControlLight { BaseCanId = 0x18F, Index = i - 16, Value = 0, StartBit = (i - 32 - 1) * 8 });
                }
                for (var i = 41; i <= 48; i++)
                {
                    _frontWhiteLightsR.Add(new CdcuControlLight { BaseCanId = 0x197, Index = i - 16, Value = 0, StartBit = (i - 40 - 1) * 8 });
                    _frontWhiteLightsL.Add(new CdcuControlLight { BaseCanId = 0x190, Index = i - 16, Value = 0, StartBit = (i - 40 - 1) * 8 });
                }
                for (var i = 49; i <= 56; i++)
                {
                    _frontWhiteLightsR.Add(new CdcuControlLight { BaseCanId = 0x198, Index = i - 16, Value = 0, StartBit = (i - 48 - 1) * 8 });
                    _frontWhiteLightsL.Add(new CdcuControlLight { BaseCanId = 0x191, Index = i - 16, Value = 0, StartBit = (i - 48 - 1) * 8 });
                }
                for (var i = 57; i <= 64; i++)
                {
                    _frontWhiteLightsR.Add(new CdcuControlLight { BaseCanId = 0x1AA, Index = i - 16, Value = 0, StartBit = (i - 56 - 1) * 8 });
                    _frontWhiteLightsL.Add(new CdcuControlLight { BaseCanId = 0x1AD, Index = i - 16, Value = 0, StartBit = (i - 56 - 1) * 8 });
                }
                for (var i = 65; i <= 72; i++)
                {
                    _frontWhiteLightsR.Add(new CdcuControlLight { BaseCanId = 0x1AB, Index = i - 16, Value = 0, StartBit = (i - 64 - 1) * 8 });
                    _frontWhiteLightsL.Add(new CdcuControlLight { BaseCanId = 0x1AE, Index = i - 16, Value = 0, StartBit = (i - 64 - 1) * 8 });
                }

                _dataFrontLightsL.Add(new CanCommunicationMatrix.MotorolaMatrix(0x18B, 8));
                _dataFrontLightsL.Add(new CanCommunicationMatrix.MotorolaMatrix(0x18C, 8));
                _dataFrontLightsL.Add(new CanCommunicationMatrix.MotorolaMatrix(0x18D, 8));
                _dataFrontLightsL.Add(new CanCommunicationMatrix.MotorolaMatrix(0x18E, 8));
                _dataFrontLightsL.Add(new CanCommunicationMatrix.MotorolaMatrix(0x18F, 8));
                _dataFrontLightsL.Add(new CanCommunicationMatrix.MotorolaMatrix(0x190, 8));
                _dataFrontLightsL.Add(new CanCommunicationMatrix.MotorolaMatrix(0x191, 8));
                _dataFrontLightsL.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1AA, 8));
                _dataFrontLightsL.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1AB, 8));

                _dataFrontLightsR.Add(new CanCommunicationMatrix.MotorolaMatrix(0x192, 8));
                _dataFrontLightsR.Add(new CanCommunicationMatrix.MotorolaMatrix(0x193, 8));
                _dataFrontLightsR.Add(new CanCommunicationMatrix.MotorolaMatrix(0x194, 8));
                _dataFrontLightsR.Add(new CanCommunicationMatrix.MotorolaMatrix(0x195, 8));
                _dataFrontLightsR.Add(new CanCommunicationMatrix.MotorolaMatrix(0x196, 8));
                _dataFrontLightsR.Add(new CanCommunicationMatrix.MotorolaMatrix(0x197, 8));
                _dataFrontLightsR.Add(new CanCommunicationMatrix.MotorolaMatrix(0x198, 8));
                _dataFrontLightsR.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1AD, 8));
                _dataFrontLightsR.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1AE, 8));

                _dataFrontLightsM.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1B7, 8));
                _dataFrontLightsM.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1C1, 8));
                _dataFrontLightsM.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1C2, 8));
                _dataFrontLightsM.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1B8, 8));
                _dataFrontLightsM.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1B9, 8));
                _dataFrontLightsM.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1BA, 8));
                _dataFrontLightsM.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1BB, 8));
                _dataFrontLightsM.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1BC, 8));
                _dataFrontLightsM.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1BD, 8));
                _dataFrontLightsM.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1BE, 8));
                _dataFrontLightsM.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1BF, 8));
            }

            // 尾灯
            {
                for (var i = 1; i <= 12; i++)
                {
                    _rearRedLightsR.Add(new CdcuControlLight { BaseCanId = 0x19E, Index = i, Value = 0, StartBit = (i - 1) * 4 });
                    _rearRedLightsL.Add(new CdcuControlLight { BaseCanId = 0x199, Index = i, Value = 0, StartBit = (i - 1) * 4 });
                }
                for (var i = 13; i <= 24; i++)
                {
                    _rearRedLightsR.Add(new CdcuControlLight { BaseCanId = 0x1A4, Index = i, Value = 0, StartBit = (i - 12 - 1) * 4 });
                    _rearRedLightsL.Add(new CdcuControlLight { BaseCanId = 0x19F, Index = i, Value = 0, StartBit = (i - 12 - 1) * 4 });
                }
                for (var i = 25; i <= 36; i++)
                {
                    _rearRedLightsR.Add(new CdcuControlLight { BaseCanId = 0x1A5, Index = i, Value = 0, StartBit = (i - 24 - 1) * 4 });
                    _rearRedLightsL.Add(new CdcuControlLight { BaseCanId = 0x19A, Index = i, Value = 0, StartBit = (i - 24 - 1) * 4 });
                }
                for (var i = 37; i <= 48; i++)
                {
                    _rearRedLightsR.Add(new CdcuControlLight { BaseCanId = 0x1AC, Index = i, Value = 0, StartBit = (i - 36 - 1) * 4 });
                    _rearRedLightsL.Add(new CdcuControlLight { BaseCanId = 0x19B, Index = i, Value = 0, StartBit = (i - 36 - 1) * 4 });
                }
                for (var i = 49; i <= 60; i++)
                {
                    _rearYellowLightsR.Add(new CdcuControlLight { BaseCanId = 0x1A6, Index = i, Value = 0, StartBit = (i - 48 - 1) * 4 });
                    _rearYellowLightsL.Add(new CdcuControlLight { BaseCanId = 0x19C, Index = i, Value = 0, StartBit = (i - 48 - 1) * 4 });
                }
                for (var i = 61; i <= 72; i++)
                {
                    _rearYellowLightsR.Add(new CdcuControlLight { BaseCanId = 0x1A8, Index = i, Value = 0, StartBit = (i - 60 - 1) * 4 });
                    _rearYellowLightsL.Add(new CdcuControlLight { BaseCanId = 0x19d, Index = i, Value = 0, StartBit = (i - 60 - 1) * 4 });
                }
                for (var i = 73; i <= 85; i++)
                {
                    _rearYellowLightsR.Add(new CdcuControlLight { BaseCanId = 0x1A7, Index = i, Value = 0, StartBit = (i - 72 - 1) * 4 });
                    _rearYellowLightsL.Add(new CdcuControlLight { BaseCanId = 0x189, Index = i, Value = 0, StartBit = (i - 72 - 1) * 4 });
                }

                _dataRearLightsL.Add(new CanCommunicationMatrix.MotorolaMatrix(0x199, 8));
                _dataRearLightsL.Add(new CanCommunicationMatrix.MotorolaMatrix(0x19F, 8));
                _dataRearLightsL.Add(new CanCommunicationMatrix.MotorolaMatrix(0x19A, 8));
                _dataRearLightsL.Add(new CanCommunicationMatrix.MotorolaMatrix(0x19B, 8));
                _dataRearLightsL.Add(new CanCommunicationMatrix.MotorolaMatrix(0x19C, 8));
                _dataRearLightsL.Add(new CanCommunicationMatrix.MotorolaMatrix(0x19D, 8));
                _dataRearLightsL.Add(new CanCommunicationMatrix.MotorolaMatrix(0x189, 8));

                _dataRearLightsR.Add(new CanCommunicationMatrix.MotorolaMatrix(0x19E, 8));
                _dataRearLightsR.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1A4, 8));
                _dataRearLightsR.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1A5, 8));
                _dataRearLightsR.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1AC, 8));
                _dataRearLightsR.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1A6, 8));
                _dataRearLightsR.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1A8, 8));
                _dataRearLightsR.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1A7, 8));
            }
        }

        private void Lb(bool isOn)
        {
            _lb = (byte)(isOn ? 1 : 0);
        }

        private void Hb(bool isOn)
        {
            _hb = (byte)(isOn ? 1 : 0);
        }

        private void Drl(int value)
        {
            _drl = (byte)value;
        }

        private void Pos(bool isOn)
        {
            _pos = (byte)(isOn ? 1 : 0);
        }

        private void LeftTurn(int value)
        {
            _turnL = (byte)value;
        }

        private void RightTurn(int value)
        {
            _turnR = (byte)value;
        }

        private void TurnFlash(bool isEnable)
        {
            _turnFlashEnable = (byte)(isEnable ? 1 : 0);
        }

        private void TurnOnOff(bool isEnable)
        {
            _isTurnOnOffEnable = isEnable;
        }

        #endregion

        #region 灯控按钮

        private void SwTurnFlashEnable_ActiveChanged(object sender, EventArgs e)
        {
            TurnFlash(swTurnFlashEnable.Active);
        }

        private void SwTurnOnOffEnable_ActiveChanged(object sender, EventArgs e)
        {
            TurnOnOff(swTurnOnOffEnable.Active);
        }

        private void SwTurnR_ActiveChanged(object sender, EventArgs e)
        {
            RightTurn(swTurnR.Active ? 1 : 0);
        }

        private void SwTurnL_ActiveChanged(object sender, EventArgs e)
        {
            LeftTurn(swTurnL.Active ? 1 : 0);
        }

        private void CmbDrl_SelectedIndexChanged(object sender, EventArgs e)
        {
            Drl(cmbDrl.SelectedIndex);
        }

        private void SwPos_ActiveChanged(object sender, EventArgs e)
        {
            Pos(swPos.Active);
        }

        private void SwLb_ActiveChanged(object sender, EventArgs e)
        {
            Lb(swLb.Active);
        }

        private void SwHb_ActiveChanged(object sender, EventArgs e)
        {
            Hb(swHb.Active);
        }

        #endregion

        #region 实验模式按钮

        private void CmbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMode.SelectedIndex == 0)
            {
                ResetLampControlUi(true);
                ResetExperimentModeUi(true);
                ResetCdcuControlUi(true);

                _lampMode = LampMode.Normal;
            }
            else
            {
                ResetLampControlUi(false);
                ResetCdcuControlUi(false);

                if (cmbMode.SelectedIndex == 1)
                {
                    _lampMode = LampMode.Mode1;
                }
                else if (cmbMode.SelectedIndex == 2)
                {
                    _lampMode = LampMode.Mode2;
                    _modeTurnTs = HighPrecisionTimer.GetTimestamp();
                }
                else if (cmbMode.SelectedIndex == 3)
                {
                    _lampMode = LampMode.Mode3;
                }
            }
        }

        #endregion

        #region CDCU控制按钮

        private void SwCdcuControl_ActiveChanged(object sender, EventArgs e)
        {
            _isCdcuControl = swCdcuControl.Active;
            ResetLampControlUi(!_isCdcuControl);
            ResetExperimentModeUi(!_isCdcuControl);
            ResetCdcuControlUi(_isCdcuControl);
        }

        private void TxtRearYellow_ValueChanged(object sender, int value)
        {
            _rearCdcuYellow = txtRearYellow.Value;
        }

        private void TxtRearRed_ValueChanged(object sender, int value)
        {
            _rearCdcuRed = txtRearRed.Value;
        }

        private void TxtMiddleWhite_ValueChanged(object sender, int value)
        {
            _middleCdcuWhite = txtMiddleWhite.Value;
        }

        private void TxtFrontYellow_ValueChanged(object sender, int value)
        {
            var tp = txtFrontYellow.Value + 10;
            if (tp==10)
                tp = 0;
            _frontCdcuYellow = tp;
        }

        private void TxtFrontWhite_ValueChanged(object sender, int value)
        {
            _frontCdcuWhite = txtFrontWhite.Value;
        }

        #endregion

        private void ResetLampControlUi(bool isEnable)
        {
            swLb.Active = false;
            swHb.Active = false;
            swPos.Active = false;
            swTurnR.Active = false;
            swTurnL.Active = false;
            swTurnOnOffEnable.Active = false;
            swTurnFlashEnable.Active = false;
            cmbDrl.SelectedIndex = 0;

            gpLampControl.Enabled = isEnable;
        }

        private void ResetCdcuControlUi(bool isEnable)
        {
            txtMiddleWhite.Enabled = txtFrontWhite.Enabled =
                txtFrontYellow.Enabled = txtRearRed.Enabled = txtRearYellow.Enabled = isEnable;
            txtMiddleWhite.Value = txtFrontWhite.Value =
                txtFrontYellow.Value = txtRearRed.Value = txtRearYellow.Value = 0;
            //gpCdcuControl.Enabled = isEnable;
        }

        private void ResetExperimentModeUi(bool isEnable)
        {
            cmbMode.SelectedIndex = 0;
            gpExperimentMode.Enabled = isEnable;
        }
    }
}
