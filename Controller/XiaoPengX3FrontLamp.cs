using CommonUtility;
using CommonUtility.BusLoader;
using System.Collections.Generic;
using System.ComponentModel;

namespace Controller
{
    [Description("CAN-Product,小鹏X3前灯")]
    public sealed class XiaoPengX3FrontLamp : ControllerBase
    {
        public CanBus Can;

        [Description("R/W,是否连接L")]
        public bool IsLConnect;

        [Description("R/W,是否连接R")]
        public bool IsRConnect;

        [Description("R/W,是否连接M")]
        public bool IsMConnect;

        public XiaoPengX3FrontLamp
            (string name) : base(name)
        {
            for (var i = 1; i <= 8; i++)
                _drlLightsM.Add(new CdcuControlLight { BaseCanId = 0x1b7, Index = i, Value = 0, StartBit = (i - 1) * 8 });
            for (var i = 9; i <= 16; i++)
                _drlLightsM.Add(new CdcuControlLight { BaseCanId = 0x1b8, Index = i, Value = 0, StartBit = (i - 8 - 1) * 8 });
            for (var i = 17; i <= 24; i++)
                _drlLightsM.Add(new CdcuControlLight { BaseCanId = 0x1b9, Index = i, Value = 0, StartBit = (i - 16 - 1) * 8 });
            for (var i = 25; i <= 32; i++)
                _drlLightsM.Add(new CdcuControlLight { BaseCanId = 0x1ba, Index = i, Value = 0, StartBit = (i - 24 - 1) * 8 });
            for (var i = 33; i <= 40; i++)
                _drlLightsM.Add(new CdcuControlLight { BaseCanId = 0x1bb, Index = i, Value = 0, StartBit = (i - 32 - 1) * 8 });
            for (var i = 41; i <= 48; i++)
                _drlLightsM.Add(new CdcuControlLight { BaseCanId = 0x1bc, Index = i, Value = 0, StartBit = (i - 40 - 1) * 8 });
            for (var i = 49; i <= 56; i++)
                _drlLightsM.Add(new CdcuControlLight { BaseCanId = 0x1bd, Index = i, Value = 0, StartBit = (i - 48 - 1) * 8 });
            for (var i = 57; i <= 64; i++)
                _drlLightsM.Add(new CdcuControlLight { BaseCanId = 0x1be, Index = i, Value = 0, StartBit = (i - 56 - 1) * 8 });
            for (var i = 65; i <= 72; i++)
                _drlLightsM.Add(new CdcuControlLight { BaseCanId = 0x1bf, Index = i, Value = 0, StartBit = (i - 64 - 1) * 8 });
            for (var i = 73; i <= 80; i++)
                _drlLightsM.Add(new CdcuControlLight { BaseCanId = 0x1c1, Index = i, Value = 0, StartBit = (i - 72 - 1) * 8 });

            _drlLightsM.Add(new CdcuControlLight { BaseCanId = 0x1c2, Index = 81, Value = 0, StartBit = 0 * 8 });
            _drlLightsM.Add(new CdcuControlLight { BaseCanId = 0x1c2, Index = 82, Value = 0, StartBit = 1 * 8 });

            for (var i = 1; i <= 8; i++)
            {
                _turnLightsR.Add(new CdcuControlLight { BaseCanId = 0x192, Index = i, Value = 0, StartBit = (i - 1) * 8 });
                _turnLightsL.Add(new CdcuControlLight { BaseCanId = 0x18b, Index = i, Value = 0, StartBit = (i - 1) * 8 });
            }
            for (var i = 9; i <= 16; i++)
            {
                _turnLightsR.Add(new CdcuControlLight { BaseCanId = 0x193, Index = i, Value = 0, StartBit = (i - 8 - 1) * 8 });
                _turnLightsL.Add(new CdcuControlLight { BaseCanId = 0x18C, Index = i, Value = 0, StartBit = (i - 8 - 1) * 8 });
            }

            for (var i = 17; i <= 24; i++)
            {
                _drlLightsR.Add(new CdcuControlLight { BaseCanId = 0x194, Index = i - 16, Value = 0, StartBit = (i - 16 - 1) * 8 });
                _drlLightsL.Add(new CdcuControlLight { BaseCanId = 0x18D, Index = i - 16, Value = 0, StartBit = (i - 16 - 1) * 8 });
            }
            for (var i = 25; i <= 32; i++)
            {
                _drlLightsR.Add(new CdcuControlLight { BaseCanId = 0x195, Index = i - 16, Value = 0, StartBit = (i - 24 - 1) * 8 });
                _drlLightsL.Add(new CdcuControlLight { BaseCanId = 0x18E, Index = i - 16, Value = 0, StartBit = (i - 24 - 1) * 8 });
            }
            for (var i = 33; i <= 40; i++)
            {
                _drlLightsR.Add(new CdcuControlLight { BaseCanId = 0x196, Index = i - 16, Value = 0, StartBit = (i - 32 - 1) * 8 });
                _drlLightsL.Add(new CdcuControlLight { BaseCanId = 0x18F, Index = i - 16, Value = 0, StartBit = (i - 32 - 1) * 8 });
            }
            for (var i = 41; i <= 48; i++)
            {
                _drlLightsR.Add(new CdcuControlLight { BaseCanId = 0x197, Index = i - 16, Value = 0, StartBit = (i - 40 - 1) * 8 });
                _drlLightsL.Add(new CdcuControlLight { BaseCanId = 0x190, Index = i - 16, Value = 0, StartBit = (i - 40 - 1) * 8 });
            }
            for (var i = 49; i <= 56; i++)
            {
                _drlLightsR.Add(new CdcuControlLight { BaseCanId = 0x198, Index = i - 16, Value = 0, StartBit = (i - 48 - 1) * 8 });
                _drlLightsL.Add(new CdcuControlLight { BaseCanId = 0x191, Index = i - 16, Value = 0, StartBit = (i - 48 - 1) * 8 });
            }
            for (var i = 57; i <= 64; i++)
            {
                _drlLightsR.Add(new CdcuControlLight { BaseCanId = 0x1AD, Index = i - 16, Value = 0, StartBit = (i - 56 - 1) * 8 });
                _drlLightsL.Add(new CdcuControlLight { BaseCanId = 0x1AA, Index = i - 16, Value = 0, StartBit = (i - 56 - 1) * 8 });
            }
            for (var i = 65; i <= 72; i++)
            {
                _drlLightsR.Add(new CdcuControlLight { BaseCanId = 0x1AE, Index = i - 16, Value = 0, StartBit = (i - 64 - 1) * 8 });
                _drlLightsL.Add(new CdcuControlLight { BaseCanId = 0x1AB, Index = i - 16, Value = 0, StartBit = (i - 64 - 1) * 8 });
            }

            _dataLightsL.Add(new CanCommunicationMatrix.MotorolaMatrix(0x18B, 8));
            _dataLightsL.Add(new CanCommunicationMatrix.MotorolaMatrix(0x18C, 8));
            _dataLightsL.Add(new CanCommunicationMatrix.MotorolaMatrix(0x18D, 8));
            _dataLightsL.Add(new CanCommunicationMatrix.MotorolaMatrix(0x18E, 8));
            _dataLightsL.Add(new CanCommunicationMatrix.MotorolaMatrix(0x18F, 8));
            _dataLightsL.Add(new CanCommunicationMatrix.MotorolaMatrix(0x190, 8));
            _dataLightsL.Add(new CanCommunicationMatrix.MotorolaMatrix(0x191, 8));
            _dataLightsL.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1AA, 8));
            _dataLightsL.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1AB, 8));

            _dataLightsR.Add(new CanCommunicationMatrix.MotorolaMatrix(0x192, 8));
            _dataLightsR.Add(new CanCommunicationMatrix.MotorolaMatrix(0x193, 8));
            _dataLightsR.Add(new CanCommunicationMatrix.MotorolaMatrix(0x194, 8));
            _dataLightsR.Add(new CanCommunicationMatrix.MotorolaMatrix(0x195, 8));
            _dataLightsR.Add(new CanCommunicationMatrix.MotorolaMatrix(0x196, 8));
            _dataLightsR.Add(new CanCommunicationMatrix.MotorolaMatrix(0x197, 8));
            _dataLightsR.Add(new CanCommunicationMatrix.MotorolaMatrix(0x198, 8));
            _dataLightsR.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1AD, 8));
            _dataLightsR.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1AE, 8));

            _dataLightsM.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1B7, 8));
            _dataLightsM.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1C1, 8));
            _dataLightsM.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1C2, 8));
            _dataLightsM.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1B8, 8));
            _dataLightsM.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1B9, 8));
            _dataLightsM.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1BA, 8));
            _dataLightsM.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1BB, 8));
            _dataLightsM.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1BC, 8));
            _dataLightsM.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1BD, 8));
            _dataLightsM.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1BE, 8));
            _dataLightsM.Add(new CanCommunicationMatrix.MotorolaMatrix(0x1BF, 8));

            MainWork();
        }

        ~XiaoPengX3FrontLamp() => Dispose();

        private readonly CanCommunicationMatrix.MotorolaMatrix _data0X269 = new CanCommunicationMatrix.MotorolaMatrix(0x269, 8);
        private readonly CanCommunicationMatrix.MotorolaMatrix _dataModeControl = new CanCommunicationMatrix.MotorolaMatrix(0x12B, 8);
        private readonly List<CanCommunicationMatrix.MotorolaMatrix> _dataLightsL = new List<CanCommunicationMatrix.MotorolaMatrix>();
        private readonly List<CanCommunicationMatrix.MotorolaMatrix> _dataLightsR = new List<CanCommunicationMatrix.MotorolaMatrix>();
        private readonly List<CanCommunicationMatrix.MotorolaMatrix> _dataLightsM = new List<CanCommunicationMatrix.MotorolaMatrix>();
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

                    switch (_frontLampMode)
                    {
                        case FrontLampMode.Normal:
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

                                if (sendCount == 2 && IsMConnect)
                                {
                                    if (!_isDrlOddOn && !_isDrlEvenOn)
                                    {
                                        foreach (var t in _drlLightsM)
                                            t.Value = 0;
                                    }
                                    else
                                    {
                                        for (var i = 0; i < _drlLightsM.Count; i++)
                                        {
                                            if (i % 2 == 0)
                                                _drlLightsM[i].Value = _isDrlEvenOn ? _drlValue : 0;
                                            else
                                                _drlLightsM[i].Value = _isDrlOddOn ? _drlValue : 0;
                                        }
                                    }

                                    foreach (var t in _drlLightsM)
                                    {
                                        var findRMatrix = _dataLightsM.Find(f => f.CanId == t.BaseCanId);
                                        if (findRMatrix == null)
                                            continue;
                                        findRMatrix.UpdateData(new MatrixValDefinition(t.StartBit, 8, (byte)t.Value));
                                    }

                                    foreach (var t in _dataLightsM)
                                    {
                                        //Can.SendStandardCanData(t.CanId, t.MatrixData);
                                        sendDatas.Add(new CanBus.CanDataPackage(t.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, t.MatrixData));
                                    }
                                }
                            }

                            if (sendCount == 1 || sendCount == 3)
                            {
                                _dataModeControl.UpdateData(new MatrixValDefinition(2, 1, (byte)(_isCdcuControl ? 1 : 0)));
                                _dataModeControl.UpdateData(new MatrixValDefinition(3, 1, (byte)(_isCdcuControl ? 1 : 0)));
                                _dataModeControl.UpdateData(new MatrixValDefinition(4, 1, (byte)(_isCdcuControl ? 1 : 0)));
                                //Can.SendStandardCanData(_dataModeControl.CanId, _dataModeControl.MatrixData);
                                sendDatas.Add(new CanBus.CanDataPackage(_dataModeControl.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, _dataModeControl.MatrixData));

                                if (IsLConnect && sendCount == 1)
                                {
                                    if (!_isTurnOddOn && !_isTurnEvenOn)
                                    {
                                        foreach (var t in _turnLightsL)
                                            t.Value = 0;
                                    }
                                    else
                                    {
                                        for (var i = 0; i < _turnLightsL.Count; i++)
                                        {
                                            if (i % 2 == 0)
                                                _turnLightsL[i].Value = _isTurnEvenOn ? _turnValue : 0;
                                            else
                                                _turnLightsL[i].Value = _isTurnOddOn ? _turnValue : 0;
                                        }
                                    }

                                    if (!_isDrlOddOn && !_isDrlEvenOn)
                                    {
                                        foreach (var t in _drlLightsL)
                                            t.Value = 0;
                                    }
                                    else
                                    {
                                        for (var i = 0; i < _drlLightsL.Count; i++)
                                        {
                                            if (i % 2 == 0)
                                                _drlLightsL[i].Value = _isDrlEvenOn ? _drlValue : 0;
                                            else
                                                _drlLightsL[i].Value = _isDrlOddOn ? _drlValue : 0;
                                        }
                                    }

                                    foreach (var t in _turnLightsL)
                                    {
                                        var findLMatrix = _dataLightsL.Find(f => f.CanId == t.BaseCanId);
                                        if (findLMatrix == null)
                                            continue;
                                        findLMatrix.UpdateData(new MatrixValDefinition(t.StartBit, 8, (byte)t.Value));
                                    }
                                    foreach (var t in _drlLightsL)
                                    {
                                        var findLMatrix = _dataLightsL.Find(f => f.CanId == t.BaseCanId);
                                        if (findLMatrix == null)
                                            continue;
                                        findLMatrix.UpdateData(new MatrixValDefinition(t.StartBit, 8, (byte)t.Value));
                                    }

                                    foreach (var t in _dataLightsL)
                                    {
                                        //Can.SendStandardCanData(t.CanId, t.MatrixData);
                                        sendDatas.Add(new CanBus.CanDataPackage(t.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, t.MatrixData));
                                    }
                                }

                                if (IsRConnect && sendCount == 3)
                                {
                                    if (!_isTurnOddOn && !_isTurnEvenOn)
                                    {
                                        foreach (var t in _turnLightsR)
                                            t.Value = 0;
                                    }
                                    else
                                    {
                                        for (var i = 0; i < _turnLightsR.Count; i++)
                                        {
                                            if (i % 2 == 0)
                                                _turnLightsR[i].Value = _isTurnEvenOn ? _turnValue : 0;
                                            else
                                                _turnLightsR[i].Value = _isTurnOddOn ? _turnValue : 0;
                                        }
                                    }

                                    if (!_isDrlOddOn && !_isDrlEvenOn)
                                    {
                                        foreach (var t in _drlLightsR)
                                            t.Value = 0;
                                    }
                                    else
                                    {
                                        for (var i = 0; i < _drlLightsR.Count; i++)
                                        {
                                            if (i % 2 == 0)
                                                _drlLightsR[i].Value = _isDrlEvenOn ? _drlValue : 0;
                                            else
                                                _drlLightsR[i].Value = _isDrlOddOn ? _drlValue : 0;
                                        }
                                    }

                                    foreach (var t in _turnLightsR)
                                    {
                                        var findLMatrix = _dataLightsR.Find(f => f.CanId == t.BaseCanId);
                                        if (findLMatrix == null)
                                            continue;
                                        findLMatrix.UpdateData(new MatrixValDefinition(t.StartBit, 8, (byte)t.Value));
                                    }
                                    foreach (var t in _drlLightsR)
                                    {
                                        var findRMatrix = _dataLightsR.Find(f => f.CanId == t.BaseCanId);
                                        if (findRMatrix == null)
                                            continue;
                                        findRMatrix.UpdateData(new MatrixValDefinition(t.StartBit, 8, (byte)t.Value));
                                    }

                                    foreach (var t in _dataLightsR)
                                    {
                                        //Can.SendStandardCanData(t.CanId, t.MatrixData);
                                        sendDatas.Add(new CanBus.CanDataPackage(t.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, t.MatrixData));
                                    }
                                }
                            }
                            break;

                        case FrontLampMode.Mode1:
                            {
                                var nowTs = HighPrecisionTimer.GetTimestamp();
                                var ms = HighPrecisionTimer.GetTimestampIntervalMs(_modeTurnTs, nowTs);
                                if (ms > 0 && ms <= 400)
                                {
                                    var tpMatrix = new CanCommunicationMatrix.MotorolaMatrix(0x269, 8);
                                    tpMatrix.UpdateData(new MatrixValDefinition(47, 1, 1));
                                    tpMatrix.UpdateData(new MatrixValDefinition(37, 1, 1));
                                    tpMatrix.UpdateData(new MatrixValDefinition(39, 1, 1));
                                    tpMatrix.UpdateData(new MatrixValDefinition(61, 3, 0));
                                    tpMatrix.UpdateData(new MatrixValDefinition(4, 2, 1));
                                    tpMatrix.UpdateData(new MatrixValDefinition(6, 2, 1));
                                    //Can.SendStandardCanData(tpMatrix.CanId, tpMatrix.MatrixData);
                                    //Can.SendStandardCanData(_dataModeControl.CanId, new byte[8]);
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
                                    //Can.SendStandardCanData(tpMatrix.CanId, tpMatrix.MatrixData);
                                    //Can.SendStandardCanData(_dataModeControl.CanId, new byte[8]);

                                    sendDatas.Add(new CanBus.CanDataPackage(tpMatrix.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, tpMatrix.MatrixData));
                                    sendDatas.Add(new CanBus.CanDataPackage(_dataModeControl.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8]));
                                }
                                else
                                {
                                    _modeTurnTs = HighPrecisionTimer.GetTimestamp();
                                }
                            }
                            break;

                        case FrontLampMode.Mode2:
                            {
                                var nowTs = HighPrecisionTimer.GetTimestamp();
                                var ms = HighPrecisionTimer.GetTimestampIntervalMs(_modeTurnTs, nowTs);
                                if (ms > 0 && ms <= 400)
                                {
                                    var tpMatrix = new CanCommunicationMatrix.MotorolaMatrix(0x269, 8);
                                    tpMatrix.UpdateData(new MatrixValDefinition(47, 1, 0));
                                    tpMatrix.UpdateData(new MatrixValDefinition(37, 1, 0));
                                    tpMatrix.UpdateData(new MatrixValDefinition(39, 1, 0));
                                    tpMatrix.UpdateData(new MatrixValDefinition(61, 3, 1));
                                    tpMatrix.UpdateData(new MatrixValDefinition(4, 2, 0));
                                    tpMatrix.UpdateData(new MatrixValDefinition(6, 2, 0));
                                    //Can.SendStandardCanData(tpMatrix.CanId, tpMatrix.MatrixData);
                                    //Can.SendStandardCanData(_dataModeControl.CanId, new byte[8]);

                                    sendDatas.Add(new CanBus.CanDataPackage(tpMatrix.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, tpMatrix.MatrixData));
                                    sendDatas.Add(new CanBus.CanDataPackage(_dataModeControl.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8]));
                                }
                                else if (ms > 400 && ms <= 800)
                                {
                                    var tpMatrix = new CanCommunicationMatrix.MotorolaMatrix(0x269, 8);
                                    tpMatrix.UpdateData(new MatrixValDefinition(47, 1, 0));
                                    tpMatrix.UpdateData(new MatrixValDefinition(37, 1, 0));
                                    tpMatrix.UpdateData(new MatrixValDefinition(39, 1, 0));
                                    tpMatrix.UpdateData(new MatrixValDefinition(61, 3, 1));
                                    tpMatrix.UpdateData(new MatrixValDefinition(4, 2, 0));
                                    tpMatrix.UpdateData(new MatrixValDefinition(6, 2, 0));
                                    //Can.SendStandardCanData(tpMatrix.CanId, tpMatrix.MatrixData);
                                    //Can.SendStandardCanData(_dataModeControl.CanId, new byte[8]);
                                    sendDatas.Add(new CanBus.CanDataPackage(tpMatrix.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, tpMatrix.MatrixData));
                                    sendDatas.Add(new CanBus.CanDataPackage(_dataModeControl.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8]));
                                }
                                else
                                {
                                    _modeTurnTs = HighPrecisionTimer.GetTimestamp();
                                }
                            }
                            break;

                        default:
                            break;
                    }

                    Can.SendCanDatas(sendDatas.ToArray());

                    sendCount++;
                    if (sendCount == 4)
                        sendCount = 0;
                },
                Interval = 5
            });

            SchedulerAsync();
        }

        [Description("打开CAN消息")]
        public void StartCanMsg() => _isSleep = false;

        [Description("关闭CAN消息")]
        public void StopCanMsg() => _isSleep = true;

        #region 模块控制

        private bool _isTurnOnOffEnable;
        private long _turnOnOffTs;
        private int _turnLValue;
        private int _turnRValue;

        [Description("近光灯亮")]
        public void LbOn()
        {
            FrontLampModeOff();
            _data0X269.UpdateData(new MatrixValDefinition(37, 1, 1));
        }

        [Description("近光灯灭")]
        public void LbOff()
        {
            FrontLampModeOff();
            _data0X269.UpdateData(new MatrixValDefinition(37, 1, 0));
        }

        [Description("远光灯亮")]
        public void HbOn()
        {
            FrontLampModeOff();
            _data0X269.UpdateData(new MatrixValDefinition(47, 1, 1));
        }

        [Description("远光灯灭")]
        public void HbOff()
        {
            FrontLampModeOff();
            _data0X269.UpdateData(new MatrixValDefinition(47, 1, 0));
        }

        [Description("日行灯亮")]
        public void DrlOn(int value)
        {
            if (value != 1 && value != 2 && value != 3 && value != 4 && value != 5 && value != 6 && value != 7)
                return;

            FrontLampModeOff();
            _data0X269.UpdateData(new MatrixValDefinition(61, 3, (byte)value));
        }

        [Description("日行灯灭")]
        public void DrlOff()
        {
            FrontLampModeOff();
            _data0X269.UpdateData(new MatrixValDefinition(61, 3, 0));
        }

        [Description("位置灯亮")]
        public void PosOn()
        {
            FrontLampModeOff();
            _data0X269.UpdateData(new MatrixValDefinition(39, 1, 1));
        }

        [Description("位置灯灭")]
        public void PosOff()
        {
            FrontLampModeOff();
            _data0X269.UpdateData(new MatrixValDefinition(39, 1, 0));
        }

        [Description("左转向灯亮")]
        public void TurnLOn(int value)
        {
            if (value != 1 && value != 2 && value != 3)
                return;

            FrontLampModeOff();
            _data0X269.UpdateData(new MatrixValDefinition(6, 2, (byte)value));
            _turnLValue = value;
        }

        [Description("左转向灯灭")]
        public void TurnLOff()
        {
            FrontLampModeOff();
            _data0X269.UpdateData(new MatrixValDefinition(6, 2, 0));
            _turnLValue = 0;
        }

        [Description("右转向灯亮")]
        public void TurnROn(int value)
        {
            if (value != 1 && value != 2 && value != 3)
                return;

            FrontLampModeOff();
            _data0X269.UpdateData(new MatrixValDefinition(4, 2, (byte)value));
            _turnRValue = value;
        }

        [Description("右转向灯灭")]
        public void TurnROff()
        {
            FrontLampModeOff();
            _data0X269.UpdateData(new MatrixValDefinition(4, 2, 0));
            _turnRValue = 0;
        }

        [Description("转向灯闪烁使能信号开")]
        public void TurnFlashEnable()
        {
            FrontLampModeOff();
            _data0X269.UpdateData(new MatrixValDefinition(45, 1, 1));
        }

        [Description("转向灯闪烁使能信号关")]
        public void TurnFlashDisable()
        {
            FrontLampModeOff();
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
        private int _turnValue;
        private bool _isTurnOddOn;
        private bool _isTurnEvenOn;
        private int _drlValue;
        private bool _isDrlOddOn;
        private bool _isDrlEvenOn;

        private readonly List<CdcuControlLight> _turnLightsL = new List<CdcuControlLight>();
        private readonly List<CdcuControlLight> _drlLightsL = new List<CdcuControlLight>();

        private readonly List<CdcuControlLight> _turnLightsR = new List<CdcuControlLight>();
        private readonly List<CdcuControlLight> _drlLightsR = new List<CdcuControlLight>();
        private readonly List<CdcuControlLight> _drlLightsM = new List<CdcuControlLight>();

        [Description("打开CDCN控制")]
        public void StartCdcuControl() => _isCdcuControl = true;

        [Description("关闭CDCN控制")]
        public void StopCdcuControl() => _isCdcuControl = false;

        [Description("TURN全亮")]
        public void TurnAllOn(int value)
        {
            if (value >= 1 && value <= 20)
            {
                _turnValue = value;
                _isTurnOddOn = true;
                _isTurnEvenOn = true;
            }
        }

        [Description("TURN双数全亮")]
        public void TurnEvenOn(int value)
        {
            if (value >= 1 && value <= 20)
            {
                _turnValue = value;
                _isTurnOddOn = false;
                _isTurnEvenOn = true;
            }
        }

        [Description("TURN奇数全亮")]
        public void TurnOddOn(int value)
        {
            if (value >= 1 && value <= 20)
            {
                _turnValue = value;
                _isTurnOddOn = true;
                _isTurnEvenOn = false;
            }
        }

        [Description("TURN全关")]
        public void TurnAllOff() => _turnValue = 0;

        [Description("DRL全亮")]
        public void DrlAllOn(int value)
        {
            if (value >= 1 && value <= 20)
            {
                _drlValue = value;
                _isDrlEvenOn = true;
                _isDrlOddOn = true;
            }
        }

        [Description("DRL双数全亮")]
        public void DrlEvenOn(int value)
        {
            if (value >= 1 && value <= 20)
            {
                _drlValue = value;
                _isDrlEvenOn = true;
                _isDrlOddOn = false;
            }
        }

        [Description("DRL奇数全亮")]
        public void DrlOddOn(int value)
        {
            if (value >= 1 && value <= 20)
            {
                _drlValue = value;
                _isDrlEvenOn = false;
                _isDrlOddOn = true;
            }
        }

        [Description("DRL全关")]
        public void DrlAllOff() => _drlValue = 0;

        internal class CdcuControlLight
        {
            public int Index;
            public uint BaseCanId;
            public int Value;
            public int StartBit;
        }

        #endregion

        #region 后灯实验模式

        private FrontLampMode _frontLampMode = FrontLampMode.Normal;
        private long _modeTurnTs;

        [Description("R,当前模式")]
        public string CurrenModeName = "正常点灯模式";

        [Description("实验模式OFF")]
        public void FrontLampModeOff()
        {
            _frontLampMode = FrontLampMode.Normal;
            CurrenModeName = "正常点灯模式";
        }

        [Description("实验模式1")]
        public void FrontLampMode1()
        {
            _frontLampMode = FrontLampMode.Mode1;
            CurrenModeName = "模式1";
        }

        [Description("实验模式2")]
        public void FrontLampMode2()
        {
            _frontLampMode = FrontLampMode.Mode2;
            CurrenModeName = "模式2";
            _modeTurnTs = HighPrecisionTimer.GetTimestamp();
        }

        private enum FrontLampMode
        {
            Normal,

            Mode1,

            Mode2,
        }

        #endregion
    }
}
