using CommonUtility;
using CommonUtility.BusLoader;
using System.Collections.Generic;
using System.ComponentModel;

namespace Controller
{
    [Description("CAN-Product,小鹏X3尾灯")]
    public sealed class XiaoPengX3RearLamp : ControllerBase
    {
        public CanBus Can;

        [Description("R/W,是否连接L")]
        public bool IsLConnect;

        [Description("R/W,是否连接R")]
        public bool IsRConnect;

        public XiaoPengX3RearLamp
            (string name) : base(name)
        {
            for (var i = 1; i <= 12; i++)
            {
                _redLightsR.Add(new CdcuControlLight { BaseCanId = 0x19E, Index = i, Value = 0, StartBit = (i - 1) * 4 });
                _redLightsL.Add(new CdcuControlLight { BaseCanId = 0x199, Index = i, Value = 0, StartBit = (i - 1) * 4 });
            }
            for (var i = 13; i <= 24; i++)
            {
                _redLightsR.Add(new CdcuControlLight { BaseCanId = 0x1A4, Index = i, Value = 0, StartBit = (i - 12 - 1) * 4 });
                _redLightsL.Add(new CdcuControlLight { BaseCanId = 0x19F, Index = i, Value = 0, StartBit = (i - 12 - 1) * 4 });
            }
            for (var i = 25; i <= 36; i++)
            {
                _redLightsR.Add(new CdcuControlLight { BaseCanId = 0x1A5, Index = i, Value = 0, StartBit = (i - 24 - 1) * 4 });
                _redLightsL.Add(new CdcuControlLight { BaseCanId = 0x19A, Index = i, Value = 0, StartBit = (i - 24 - 1) * 4 });
            }
            for (var i = 37; i <= 48; i++)
            {
                _redLightsR.Add(new CdcuControlLight { BaseCanId = 0x1AC, Index = i, Value = 0, StartBit = (i - 36 - 1) * 4 });
                _redLightsL.Add(new CdcuControlLight { BaseCanId = 0x19B, Index = i, Value = 0, StartBit = (i - 36 - 1) * 4 });
            }
            for (var i = 49; i <= 60; i++)
            {
                _yellowLightsR.Add(new CdcuControlLight { BaseCanId = 0x1A6, Index = i, Value = 0, StartBit = (i - 48 - 1) * 4 });
                _yellowLightsL.Add(new CdcuControlLight { BaseCanId = 0x19C, Index = i, Value = 0, StartBit = (i - 48 - 1) * 4 });
            }
            for (var i = 61; i <= 72; i++)
            {
                _yellowLightsR.Add(new CdcuControlLight { BaseCanId = 0x1A8, Index = i, Value = 0, StartBit = (i - 60 - 1) * 4 });
                _yellowLightsL.Add(new CdcuControlLight { BaseCanId = 0x19d, Index = i, Value = 0, StartBit = (i - 60 - 1) * 4 });
            }
            for (var i = 73; i <= 85; i++)
            {
                _yellowLightsR.Add(new CdcuControlLight { BaseCanId = 0x1A7, Index = i, Value = 0, StartBit = (i - 72 - 1) * 4 });
                _yellowLightsL.Add(new CdcuControlLight { BaseCanId = 0x189, Index = i, Value = 0, StartBit = (i - 72 - 1) * 4 });
            }

            _dataLightsL.Add(new CanCommunicationMatrix.MotorolaMatrix(0x199, 8));
            _dataLightsL.Add(new CanCommunicationMatrix.MotorolaMatrix(0x19F, 8));
            _dataLightsL.Add(new CanCommunicationMatrix.MotorolaMatrix(0x19A, 8));
            _dataLightsL.Add(new CanCommunicationMatrix.MotorolaMatrix(0x19B, 8));
            _dataLightsL.Add(new CanCommunicationMatrix.MotorolaMatrix(0x19C, 8));
            _dataLightsL.Add(new CanCommunicationMatrix.MotorolaMatrix(0x19D, 8));
            _dataLightsL.Add(new CanCommunicationMatrix.MotorolaMatrix(0x189, 8));

            _dataLightsR.Add(new CanCommunicationMatrix.MotorolaMatrix(0x19E, 8));
            _dataLightsR.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1A4, 8));
            _dataLightsR.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1A5, 8));
            _dataLightsR.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1AC, 8));
            _dataLightsR.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1A6, 8));
            _dataLightsR.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1A8, 8));
            _dataLightsR.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1A7, 8));

            MainWork();
        }

        ~XiaoPengX3RearLamp() => Dispose();

        private readonly CanCommunicationMatrix.MotorolaMatrix _data0X269 = new CanCommunicationMatrix.MotorolaMatrix(0x269, 8);
        private readonly CanCommunicationMatrix.MotorolaMatrix _dataModeControl = new CanCommunicationMatrix.MotorolaMatrix(0x12B, 8);
        private readonly List<CanCommunicationMatrix.MotorolaMatrix> _dataLightsL = new List<CanCommunicationMatrix.MotorolaMatrix>();
        private readonly List<CanCommunicationMatrix.MotorolaMatrix> _dataLightsR = new List<CanCommunicationMatrix.MotorolaMatrix>();
        private bool _isSleep = true;

        private void MainWork()
        {
            var sendCount = 0;

            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = () =>
                {
                    if (Can == null)
                        return;

                    if (_isSleep)
                    {
                        sendCount = 0;
                        return;
                    }

                    var sendDatas = new List<CanBus.CanDataPackage>();

                    switch (_rearLampMode)
                    {
                        case RearLampMode.Normal:
                            if (sendCount == 0 || sendCount == 2)
                            {
                                if (_isTurnOnOffEnable)
                                {
                                    var nowTs = HighPrecisionTimer.GetTimestamp();
                                    var ms = HighPrecisionTimer.GetTimestampIntervalMs(_turnOnOffTs, nowTs);

                                    if (ms > 0 && ms <= 400)
                                    {
                                        if (_turnLValue > 0)
                                            _data0X269.UpdateData(new MatrixValDefinition(6, 2, (byte)_turnLValue));

                                        if (_turnRValue > 0)
                                            _data0X269.UpdateData(new MatrixValDefinition(4, 2, (byte)_turnRValue));
                                    }
                                    else if (ms > 400 && ms <= 800)
                                    {
                                        if (_turnLValue > 0)
                                            _data0X269.UpdateData(new MatrixValDefinition(6, 2, 0));

                                        if (_turnRValue > 0)
                                            _data0X269.UpdateData(new MatrixValDefinition(4, 2, 0));
                                    }
                                    else
                                    {
                                        _turnOnOffTs = HighPrecisionTimer.GetTimestamp();
                                    }
                                }
                                else
                                {
                                    if (_turnLValue > 0)
                                        _data0X269.UpdateData(new MatrixValDefinition(6, 2, (byte)_turnLValue));
                                    else
                                        _data0X269.UpdateData(new MatrixValDefinition(6, 2, 0));

                                    if (_turnRValue > 0)
                                        _data0X269.UpdateData(new MatrixValDefinition(4, 2, (byte)_turnRValue));
                                    else
                                        _data0X269.UpdateData(new MatrixValDefinition(4, 2, 0));
                                }

                                //Can.SendStandardCanData(_data0X269.CanId, _data0X269.MatrixData);
                                sendDatas.Add(new CanBus.CanDataPackage(_data0X269.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, _data0X269.MatrixData));
                                sendDatas.Add(new CanBus.CanDataPackage(0x2A7, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, _isCdcuControl ? (byte)0x00 : (byte)0x10, 0x00, 0x00, 0x00, 0x00, 0x00 }));
                            }

                            if (sendCount == 1 || sendCount == 3)
                            {
                                _dataModeControl.UpdateData(new MatrixValDefinition(5, 1, (byte)(_isCdcuControl ? 1 : 0)));
                                _dataModeControl.UpdateData(new MatrixValDefinition(6, 1, (byte)(_isCdcuControl ? 1 : 0)));
                                //Can.SendStandardCanData(_dataModeControl.CanId, _dataModeControl.MatrixData);
                                sendDatas.Add(new CanBus.CanDataPackage(_dataModeControl.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, _dataModeControl.MatrixData));

                                if (IsLConnect && sendCount == 1)
                                {
                                    if (!_isRedOddOn && !_isRedEvenOn)
                                    {
                                        foreach (var t in _redLightsL)
                                            t.Value = 0;
                                    }
                                    else
                                    {
                                        for (var i = 0; i < _redLightsL.Count; i++)
                                        {
                                            if (i % 2 == 0)
                                                _redLightsL[i].Value = _isRedEvenOn ? _redValue : 0;
                                            else
                                                _redLightsL[i].Value = _isRedOddOn ? _redValue : 0;
                                        }
                                    }

                                    if (!_isYellowOddOn && !_isYellowEvenOn)
                                    {
                                        foreach (var t in _yellowLightsL)
                                            t.Value = 0;
                                    }
                                    else
                                    {
                                        for (var i = 0; i < _yellowLightsL.Count; i++)
                                        {
                                            if (i % 2 == 0)
                                                _yellowLightsL[i].Value = _isYellowEvenOn ? _yellowValue : 0;
                                            else
                                                _yellowLightsL[i].Value = _isYellowOddOn ? _yellowValue : 0;
                                        }
                                    }

                                    foreach (var t in _redLightsL)
                                    {
                                        var findLMatrix = _dataLightsL.Find(f => f.CanId == t.BaseCanId);
                                        if (findLMatrix == null)
                                            continue;
                                        findLMatrix.UpdateData(new MatrixValDefinition(t.StartBit, 4, (byte)t.Value));
                                    }
                                    foreach (var t in _yellowLightsL)
                                    {
                                        var findLMatrix = _dataLightsL.Find(f => f.CanId == t.BaseCanId);
                                        if (findLMatrix == null)
                                            continue;
                                        findLMatrix.UpdateData(new MatrixValDefinition(t.StartBit, 4, (byte)t.Value));
                                    }

                                    foreach (var t in _dataLightsL)
                                    {
                                        //Can.SendStandardCanData(t.CanId, t.MatrixData);
                                        sendDatas.Add(new CanBus.CanDataPackage(t.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, t.MatrixData));
                                    }
                                }

                                if (IsRConnect && sendCount == 3)
                                {
                                    if (!_isRedOddOn && !_isRedEvenOn)
                                    {
                                        foreach (var t in _redLightsR)
                                            t.Value = 0;
                                    }
                                    else
                                    {
                                        for (var i = 0; i < _redLightsR.Count; i++)
                                        {
                                            if (i % 2 == 0)
                                                _redLightsR[i].Value = _isRedEvenOn ? _redValue : 0;
                                            else
                                                _redLightsR[i].Value = _isRedOddOn ? _redValue : 0;
                                        }
                                    }

                                    if (!_isYellowOddOn && !_isYellowEvenOn)
                                    {
                                        foreach (var t in _yellowLightsR)
                                            t.Value = 0;
                                    }
                                    else
                                    {
                                        for (var i = 0; i < _yellowLightsR.Count; i++)
                                        {
                                            if (i % 2 == 0)
                                                _yellowLightsR[i].Value = _isYellowEvenOn ? _yellowValue : 0;
                                            else
                                                _yellowLightsR[i].Value = _isYellowOddOn ? _yellowValue : 0;
                                        }
                                    }

                                    foreach (var t in _redLightsR)
                                    {
                                        var findLMatrix = _dataLightsR.Find(f => f.CanId == t.BaseCanId);
                                        if (findLMatrix == null)
                                            continue;
                                        findLMatrix.UpdateData(new MatrixValDefinition(t.StartBit, 4, (byte)t.Value));
                                    }
                                    foreach (var t in _yellowLightsR)
                                    {
                                        var findRMatrix = _dataLightsR.Find(f => f.CanId == t.BaseCanId);
                                        if (findRMatrix == null)
                                            continue;
                                        findRMatrix.UpdateData(new MatrixValDefinition(t.StartBit, 4, (byte)t.Value));
                                    }

                                    foreach (var t in _dataLightsR)
                                    {
                                        //Can.SendStandardCanData(t.CanId, t.MatrixData);
                                        sendDatas.Add(new CanBus.CanDataPackage(t.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, t.MatrixData));
                                    }
                                }
                            }
                            break;

                        case RearLampMode.Mode1:
                            {
                                var tpMatrix = new CanCommunicationMatrix.MotorolaMatrix(0x269, 8);
                                tpMatrix.UpdateData(new MatrixValDefinition(39, 1, 1));
                                //Can.SendStandardCanData(tpMatrix.CanId, tpMatrix.MatrixData);
                                //Can.SendStandardCanData(_dataModeControl.CanId, new byte[8]);

                                sendDatas.Add(new CanBus.CanDataPackage(tpMatrix.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, tpMatrix.MatrixData));
                                sendDatas.Add(new CanBus.CanDataPackage(0x2A7, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, _isCdcuControl ? (byte)0x00 : (byte)0x10, 0x00, 0x00, 0x00, 0x00, 0x00 }));
                                sendDatas.Add(new CanBus.CanDataPackage(_dataModeControl.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8]));
                            }
                            break;

                        case RearLampMode.Mode2:
                            {
                                var nowTs = HighPrecisionTimer.GetTimestamp();
                                var ms = HighPrecisionTimer.GetTimestampIntervalMs(_modeTurnTs, nowTs);
                                //Can.SendStandardCanData(0X269, new byte[8]);
                                sendDatas.Add(new CanBus.CanDataPackage(0x269, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8]));
                                sendDatas.Add(new CanBus.CanDataPackage(0x2A7, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, _isCdcuControl ? (byte)0x00 : (byte)0x10, 0x00, 0x00, 0x00, 0x00, 0x00 }));
                                var tpMatrix = new CanCommunicationMatrix.MotorolaMatrix(0x12B, 8);
                                tpMatrix.UpdateData(new MatrixValDefinition(5, 1, 1));
                                tpMatrix.UpdateData(new MatrixValDefinition(6, 1, 1));
                                //Can.SendStandardCanData(tpMatrix.CanId, tpMatrix.MatrixData);
                                sendDatas.Add(new CanBus.CanDataPackage(tpMatrix.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, tpMatrix.MatrixData));
                                if (ms > 0 && ms <= 400)
                                {
                                    if (sendCount == 1)
                                    {
                                        {
                                            var tpyL = new List<CdcuControlLight>();
                                            tpyL.AddRange(_yellowLightsL);
                                            var tmyL = new List<CanCommunicationMatrix.MotorolaMatrix>();
                                            tmyL.AddRange(_dataLightsL);

                                            foreach (var t in tpyL)
                                                t.Value = _mode2Value;

                                            foreach (var t in tpyL)
                                            {
                                                var findLMatrix = tmyL.Find(f => f.CanId == t.BaseCanId);
                                                if (findLMatrix == null)
                                                    continue;
                                                findLMatrix.UpdateData(new MatrixValDefinition(t.StartBit, 4, (byte)t.Value));
                                            }

                                            foreach (var t in tmyL)
                                            {
                                                //Can.SendStandardCanData(t.CanId, t.MatrixData);
                                                sendDatas.Add(new CanBus.CanDataPackage(t.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, t.MatrixData));
                                            }
                                        }
                                    }

                                    if (sendCount == 3)
                                    {
                                        {
                                            var tpyL = new List<CdcuControlLight>();
                                            tpyL.AddRange(_yellowLightsR);
                                            var tmyL = new List<CanCommunicationMatrix.MotorolaMatrix>();
                                            tmyL.AddRange(_dataLightsR);

                                            foreach (var t in tpyL)
                                                t.Value = _mode2Value;

                                            foreach (var t in tpyL)
                                            {
                                                var findLMatrix = tmyL.Find(f => f.CanId == t.BaseCanId);
                                                if (findLMatrix == null)
                                                    continue;
                                                findLMatrix.UpdateData(new MatrixValDefinition(t.StartBit, 4, (byte)t.Value));
                                            }

                                            foreach (var t in tmyL)
                                            {
                                                //Can.SendStandardCanData(t.CanId, t.MatrixData);
                                                sendDatas.Add(new CanBus.CanDataPackage(t.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, t.MatrixData));
                                            }
                                        }
                                    }
                                }
                                else if (ms > 400 && ms <= 800)
                                {
                                    if (sendCount == 1)
                                    {
                                        {
                                            var tpyL = new List<CdcuControlLight>();
                                            tpyL.AddRange(_yellowLightsL);
                                            var tmyL = new List<CanCommunicationMatrix.MotorolaMatrix>();
                                            tmyL.AddRange(_dataLightsL);

                                            foreach (var t in tpyL)
                                                t.Value = 0;

                                            foreach (var t in tpyL)
                                            {
                                                var findLMatrix = tmyL.Find(f => f.CanId == t.BaseCanId);
                                                if (findLMatrix == null)
                                                    continue;
                                                findLMatrix.UpdateData(new MatrixValDefinition(t.StartBit, 4, (byte)t.Value));
                                            }

                                            foreach (var t in tmyL)
                                            {
                                                //Can.SendStandardCanData(t.CanId, t.MatrixData);
                                                sendDatas.Add(new CanBus.CanDataPackage(t.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, t.MatrixData));
                                            }
                                        }
                                    }

                                    if (sendCount == 3)
                                    {
                                        {
                                            var tpyL = new List<CdcuControlLight>();
                                            tpyL.AddRange(_yellowLightsR);
                                            var tmyL = new List<CanCommunicationMatrix.MotorolaMatrix>();
                                            tmyL.AddRange(_dataLightsR);

                                            foreach (var t in tpyL)
                                                t.Value = 0;

                                            foreach (var t in tpyL)
                                            {
                                                var findLMatrix = tmyL.Find(f => f.CanId == t.BaseCanId);
                                                if (findLMatrix == null)
                                                    continue;
                                                findLMatrix.UpdateData(new MatrixValDefinition(t.StartBit, 4, (byte)t.Value));
                                            }

                                            foreach (var t in tmyL)
                                            {
                                                //Can.SendStandardCanData(t.CanId, t.MatrixData);
                                                sendDatas.Add(new CanBus.CanDataPackage(t.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, t.MatrixData));
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    _modeTurnTs = HighPrecisionTimer.GetTimestamp();
                                }
                            }
                            break;

                        case RearLampMode.Mode3:
                            {
                                var nowTs = HighPrecisionTimer.GetTimestamp();
                                var ms = HighPrecisionTimer.GetTimestampIntervalMs(_modeTurnTs, nowTs);
                                if (ms > 0 && ms <= 400)
                                {
                                    var tpMatrix = new CanCommunicationMatrix.MotorolaMatrix(0x269, 8);
                                    tpMatrix.UpdateData(new MatrixValDefinition(4, 2, 1));
                                    tpMatrix.UpdateData(new MatrixValDefinition(6, 2, 1));
                                    //Can.SendStandardCanData(tpMatrix.CanId, tpMatrix.MatrixData);
                                    //Can.SendStandardCanData(_dataModeControl.CanId, new byte[8]);
                                    sendDatas.Add(new CanBus.CanDataPackage(tpMatrix.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, tpMatrix.MatrixData));
                                    sendDatas.Add(new CanBus.CanDataPackage(0x2A7, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, _isCdcuControl ? (byte)0x00 : (byte)0x10, 0x00, 0x00, 0x00, 0x00, 0x00 }));
                                    sendDatas.Add(new CanBus.CanDataPackage(_dataModeControl.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, _dataModeControl.MatrixData));
                                }
                                else if (ms > 400 && ms <= 800)
                                {
                                    var tpMatrix = new CanCommunicationMatrix.MotorolaMatrix(0x269, 8);
                                    tpMatrix.UpdateData(new MatrixValDefinition(4, 2, 0));
                                    tpMatrix.UpdateData(new MatrixValDefinition(6, 2, 0));
                                    //Can.SendStandardCanData(tpMatrix.CanId, tpMatrix.MatrixData);
                                    //Can.SendStandardCanData(_dataModeControl.CanId, new byte[8]);
                                    sendDatas.Add(new CanBus.CanDataPackage(tpMatrix.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, tpMatrix.MatrixData));
                                    sendDatas.Add(new CanBus.CanDataPackage(0x2A7, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x00, 0x00, _isCdcuControl ? (byte)0x00 : (byte)0x10, 0x00, 0x00, 0x00, 0x00, 0x00 }));
                                    sendDatas.Add(new CanBus.CanDataPackage(_dataModeControl.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8]));
                                }
                                else
                                {
                                    _modeTurnTs = HighPrecisionTimer.GetTimestamp();
                                }
                            }
                            break;
                    }

                    Can.SendCanDatas(sendDatas.ToArray());

                    sendCount++;
                    if (sendCount == 4)
                        sendCount = 0;
                },
                Interval = 20
            });

            SchedulerAsync();
        }

        [Description("打开CAN消息")]
        public void StartCanMsg() => _isSleep = false;

        [Description("关闭CAN消息")]
        public void StopCanMsg()=> _isSleep = true;

        #region 模块控制

        private bool _isTurnOnOffEnable;
        private long _turnOnOffTs;
        private int _turnLValue;
        private int _turnRValue;

        [Description("位置灯亮")]
        public void PosOn()
        {
            RearLampModeOff();
            _data0X269.UpdateData(new MatrixValDefinition(39, 1, 1));
        }

        [Description("位置灯灭")]
        public void PosOff()
        {
            RearLampModeOff();
            _data0X269.UpdateData(new MatrixValDefinition(39, 1, 0));
        }

        [Description("左转向灯亮")]
        public void TurnLOn(int value)
        {
            if (value != 1 && value != 2 && value != 3)
                return;

            RearLampModeOff();
            _data0X269.UpdateData(new MatrixValDefinition(6, 2, (byte)value));
            _turnLValue = value;
        }

        [Description("左转向灯灭")]
        public void TurnLOff()
        {
            RearLampModeOff();
            _data0X269.UpdateData(new MatrixValDefinition(6, 2, 0));
            _turnLValue = 0;
        }

        [Description("右转向灯亮")]
        public void TurnROn(int value)
        {
            if (value != 1 && value != 2 && value != 3)
                return;

            RearLampModeOff();
            _data0X269.UpdateData(new MatrixValDefinition(4, 2, (byte)value));
            _turnRValue = value;
        }

        [Description("右转向灯灭")]
        public void TurnROff()
        {
            RearLampModeOff();
            _data0X269.UpdateData(new MatrixValDefinition(4, 2, 0));
            _turnRValue = 0;
        }

        [Description("转向灯闪烁使能信号开")]
        public void TurnFlashEnable()
        {
            RearLampModeOff();
            _data0X269.UpdateData(new MatrixValDefinition(45, 1, 1));
        }

        [Description("转向灯闪烁使能信号关")]
        public void TurnFlashDisable()
        {
            RearLampModeOff();
            _data0X269.UpdateData(new MatrixValDefinition(45, 1, 0));
        }

        [Description("转向灯400ms亮灭打开")]
        public void TurnOnOffEnable()
        {
            _isTurnOnOffEnable = true;
            _turnOnOffTs = HighPrecisionTimer.GetTimestamp();
        }

        [Description("转向灯400ms亮灭关闭")]
        public void TurnOnOffDisable()
        {
            _isTurnOnOffEnable = false;
            _turnOnOffTs = HighPrecisionTimer.GetTimestamp();
        }

        #endregion

        #region MyRegion

        private bool _isCdcuControl;
        private int _redValue;
        private bool _isRedOddOn;
        private bool _isRedEvenOn;
        private int _yellowValue;
        private bool _isYellowOddOn;
        private bool _isYellowEvenOn;

        private readonly List<CdcuControlLight> _redLightsL = new List<CdcuControlLight>();
        private readonly List<CdcuControlLight> _yellowLightsL = new List<CdcuControlLight>();

        private readonly List<CdcuControlLight> _redLightsR = new List<CdcuControlLight>();
        private readonly List<CdcuControlLight> _yellowLightsR = new List<CdcuControlLight>();

        [Description("打开CDCN控制")]
        public void StartCdcuControl() => _isCdcuControl = true;

        [Description("关闭CDCN控制")]
        public void StopCdcuControl() => _isCdcuControl = false;

        [Description("红灯全亮")]
        public void RedAllOn(int value)
        {
            if (value >= 1 && value <= 10)
            {
                _redValue = value;
                _isRedOddOn = true;
                _isRedEvenOn = true;
            }
        }

        [Description("红灯双数全亮")]
        public void RedEvenOn(int value)
        {
            if (value >= 1 && value <= 10)
            {
                _redValue = value;
                _isRedOddOn = false;
                _isRedEvenOn = true;
            }
        }

        [Description("红灯奇数全亮")]
        public void RedOddOn(int value)
        {
            if (value >= 1 && value <= 10)
            {
                _redValue = value;
                _isRedOddOn = true;
                _isRedEvenOn = false;
            }
        }

        [Description("红灯全关")]
        public void RedAllOff() => _redValue = 0;

        [Description("黄灯全亮")]
        public void YellowAllOn(int value)
        {
            if (value >= 1 && value <= 10)
            {
                _yellowValue = value;
                _isYellowEvenOn = true;
                _isYellowOddOn = true;
            }
        }

        [Description("黄灯双数全亮")]
        public void YellowEvenOn(int value)
        {
            if (value >= 1 && value <= 10)
            {
                _yellowValue = value;
                _isYellowEvenOn = true;
                _isYellowOddOn = false;
            }
        }

        [Description("黄灯奇数全亮")]
        public void YellowOddOn(int value)
        {
            if (value >= 1 && value <= 10)
            {
                _yellowValue = value;
                _isYellowEvenOn = false;
                _isYellowOddOn = true;
            }
        }

        [Description("黄灯全关")]
        public void YellowAllOff()=> _yellowValue = 0;

        internal class CdcuControlLight
        {
            public int Index;
            public uint BaseCanId;
            public int Value;
            public int StartBit;
        }

        #endregion

        #region 后灯实验模式

        private RearLampMode _rearLampMode = RearLampMode.Normal;
        private long _modeTurnTs;

        [Description("R,当前模式")]
        public string CurrenModeName = "正常点灯模式";

        [Description("实验模式OFF")]
        public void RearLampModeOff()
        {
            _rearLampMode = RearLampMode.Normal;
            CurrenModeName = "正常点灯模式";
        }

        [Description("实验模式1")]
        public void RearLampMode1()
        {
            _rearLampMode = RearLampMode.Mode1;
            CurrenModeName = "模式1";
        }

        private int _mode2Value;

        [Description("实验模式2")]
        public void RearLampMode2(int value)
        {
            if (value > 0 && value <= 10)
                _mode2Value = value;
            else
                _mode2Value = 10;

            _rearLampMode = RearLampMode.Mode2;
            CurrenModeName = "模式2";
            _modeTurnTs = HighPrecisionTimer.GetTimestamp();
        }

        [Description("实验模式3")]
        public void RearLampMode3()
        {
            _rearLampMode = RearLampMode.Mode3;
            CurrenModeName = "模式3";
            _modeTurnTs = HighPrecisionTimer.GetTimestamp();
        }

        private enum RearLampMode
        {
            Normal,

            Mode1,

            Mode2,

            Mode3
        }

        #endregion
    }
}
