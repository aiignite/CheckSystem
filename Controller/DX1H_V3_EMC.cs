using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace Controller
{
    [Description("Can-Product,DX1H_V3")]
    public sealed class DX1H_V3_EMC : ControllerBase
    {
        public CanBus Can;

        #region Fields

        [Description("R,KL30_1Voltage")]
        public float KL30_1Voltage;
        [Description("R,KL30_2Voltage")]
        public float KL30_2Voltage;
        [Description("R,KL30_3Voltage")]
        public float KL30_3Voltage;
        [Description("R,NTC1温度值")]
        public float NTC1Temperature;
        [Description("R,NTC2温度值")]
        public float NTC2Temperature;
        [Description("R,坐垫加热电流值")]
        public float SecRIHotCurrent;
        [Description("R,靠背加热电流值")]
        public float BackrestSeatHotCurrent;
        [Description("R,电机2电流值")]
        public float Mot2Current;
        [Description("R,电机4电流值")]
        public float Mot4Current;
        [Description("R,电机6电流值")]
        public float Mot6Current;
        [Description("R,电机8电流值")]
        public float Mot8Current;
        [Description("R,靠背加热PWM")]
        public float BackrestSeatHotPWM;
        [Description("R,坐垫加热PWM")]
        public float SecRIHotPWM;
        [Description("R,电机2霍尔值")]
        public float Mot2Hall;
        [Description("R,电机4霍尔值")]
        public float Mot4Hall;
        [Description("R,电机6霍尔值")]
        public float Mot6Hall;
        [Description("R,电机8霍尔值")]
        public float Mot8Hall;
        [Description("R,DPCState")]
        public float DPCState;

        [Description("R/W,开关")]
        public bool KL30TestState = false;

        [Description("R,0x171报文")]
        public string Msg171 = string.Empty;
        [Description("R,0x149报文")]
        public string Msg149 = string.Empty;
        [Description("R,0x120报文")]
        public string Msg120 = string.Empty;

        [Description("R,CAN丢失")]
        public bool IsCanLoss = true;

        #endregion

        public DX1H_V3_EMC(string name) : base(name)
        {
            CanBus.PushCanMsg += Can_PushCanMsg;
            _data0X100.MatrixData = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x20, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x64, 0x00, 0x00, 0x64, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, };
            _data0x401.MatrixData = new byte[] { 0xFA, 0x3E, 0x8F, 0xA1, 0x50, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            _data0XFE.MatrixData = new byte[] { 0x00, 0x00, 0x00, 0x32, 0x79, 0x52, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, };
            foreach (var temp in Enum.GetValues(typeof(DX1HOnOrOffType)).Cast<DX1HOnOrOffType>())
                _lampOperaterDic.Add(temp, temp.GetCustomAttribute<MatrixValDefinition>());

            // init data 230
            {
                _data0X230.MatrixData = new byte[32];
                var listBits = new List<string>();
                for (int i = 0; i < _data0X230.MatrixData.Length; i++)
                {
                    var str = Convert.ToString(_data0X230.MatrixData[i], 2).PadLeft(8, '0');
                    for (var j = str.Length - 1; j >= 0; j--)
                        listBits.Add(str[j].ToString());
                }

                listBits[47] = listBits[46] = listBits[45] = listBits[44] = listBits[71] = listBits[137] = listBits[136] = listBits[151] = "1";

                var bIndex = 0;
                var bBytes = new byte[32];
                for (int i = 0; i < listBits.Count; i = i + 8)
                {
                    var str = string.Format("{7}{6}{5}{4}{3}{2}{1}{0}", listBits[i], listBits[i + 1], listBits[i + 2], listBits[i + 3], listBits[i + 4], listBits[i + 5], listBits[i + 6], listBits[i + 7]);
                    var b = Convert.ToByte(str, 2);
                    bBytes[bIndex] = b;
                    bIndex++;
                }
                Array.Copy(bBytes, 0, _data0X230.MatrixData, 0, bBytes.Length);
            }

            MainWork();
            SchedulerAsync();
        }

        private long _lastRevCanMsg = 0;
        private readonly object _lockTs = new object();

        private void Can_PushCanMsg(string name, CanBus.CanDataPackage data, CanBus.OnPushCanDataType onPushCanDataType)
        {
            try
            {
                if (Can == null || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(Can.Name) || onPushCanDataType == CanBus.OnPushCanDataType.Tx)
                    return;

                if (Can.Name == name)
                {
                    //Console.WriteLine("recv can msg from " + name);
                    AnalysisData(data);
                }
            }
            catch (Exception)
            {
            }
        }

        public double Hsd1OverCurr = 8.4;
        public double Hsd2OverCurr = 8.4;
        public double Motor2OverCurr = 10.5;
        public double Motor4OverCurr = 10.5;
        public double Motor6OverCurr = 10.5;
        public double Motor8OverCurr = 10.5;

        private bool _isHsd1OverCurr;
        private bool _isHsd2OverCurr;
        private bool _isMotor2OverCurr;
        private bool _isMotor4OverCurr;
        private bool _isMotor6OverCurr;
        private bool _isMotor8OverCurr;

        private void AnalysisData(CanBus.CanDataPackage d)
        {
            try
            {
                if (d.CanId == 0x00000171 && d.CanData.Length > 0)
                {
                    _lastRevCanMsg = HighPrecisionTimer.GetTimestamp();
                    Msg171 = ValueHelper.GetHextStr(d.CanData);

                    KL30_1Voltage = (float)Math.Round((GetMatrixData(d.CanData, 40, 8)) * 0.1f, 2, MidpointRounding.AwayFromZero);
                    KL30_2Voltage = (float)Math.Round((GetMatrixData(d.CanData, 70, 8)) * 0.1f, 2, MidpointRounding.AwayFromZero);
                    KL30_3Voltage = (float)Math.Round((GetMatrixData(d.CanData, 82, 8)) * 0.1f, 2, MidpointRounding.AwayFromZero);

                    //NTC1Temperature = GetMatrixData(d.CanData, 0, 11);
                    //NTC2Temperature = GetMatrixData(d.CanData, 12, 11);

                    var ntc1 = (float)Math.Round(((GetMatrixData(d.CanData, 0, 11) / 10f) - 70f), 2, MidpointRounding.AwayFromZero);
                    var ntc2 = (float)Math.Round(((GetMatrixData(d.CanData, 12, 11) / 10f) - 70f), 2, MidpointRounding.AwayFromZero); ;
                    if (ntc1 >= 30)
                        ntc1 = 26.9f;
                    if (ntc2 >= 30)
                        ntc2 = 26.9f;

                    NTC1Temperature = ntc1;
                    NTC2Temperature = ntc2;

                    SecRIHotCurrent = (float)Math.Round((GetMatrixData(d.CanData, 90, 8)) * 0.1f * (_hsd1Duty * 0.01d), 2, MidpointRounding.AwayFromZero);
                    if (!_isHsd1OverCurr && SecRIHotCurrent > Hsd1OverCurr)
                    {
                        _isHsd1OverCurr = true;
                        SeatHeatgCmdBackSecRi(0);
                        SeatHeatgCmdCushSecRi(0);
                    }

                    BackrestSeatHotCurrent = (float)Math.Round((((GetMatrixData(d.CanData, 98, 4) * 16f) + GetMatrixData(d.CanData, 105, 4)) * 0.1f * (_hsd2Duty * 0.01)), 2, MidpointRounding.AwayFromZero);
                    if (!_isHsd2OverCurr && BackrestSeatHotCurrent > Hsd2OverCurr)
                    {
                        _isHsd2OverCurr = true;
                        SeatHeatgCmdBackSecRi(0);
                        SeatHeatgCmdCushSecRi(0);
                    }
                }
                else if (d.CanId == 0x00000149 && d.CanData.Length > 0)
                {
                    Msg149 = ValueHelper.GetHextStr(d.CanData);

                    Mot2Current = (float)Math.Round(GetMatrixData(d.CanData, 24, 10) * 0.1f, 2, MidpointRounding.AwayFromZero);
                    if (!_isMotor2OverCurr && Mot2Current > Motor2OverCurr)
                    {
                        _isMotor2OverCurr = true;
                        SecRiBackrestMotCmd(0, 0);
                    }

                    Mot4Current = (float)Math.Round(GetMatrixData(d.CanData, 72, 10) * 0.1f, 2, MidpointRounding.AwayFromZero);
                    if (!_isMotor4OverCurr && Mot4Current > Motor4OverCurr)
                    {
                        _isMotor4OverCurr = true;
                        SecRiLenMotCmd(0, 0);
                    }

                    Mot6Current = (float)Math.Round(GetMatrixData(d.CanData, 48, 10) * 0.1f, 2, MidpointRounding.AwayFromZero);
                    if (!_isMotor6OverCurr && Mot6Current > Motor6OverCurr)
                    {
                        _isMotor6OverCurr = true;
                        SecRiLegRstHozlMotCmd(0, 0);
                    }

                    Mot8Current = (float)Math.Round(GetMatrixData(d.CanData, 60, 10) * 0.1f, 2, MidpointRounding.AwayFromZero);
                    if (!_isMotor8OverCurr && Mot8Current > Motor8OverCurr)
                    {
                        _isMotor8OverCurr = true;
                        SecRiLegRstVertMotCmd(0, 0);
                    }

                    Mot2Hall = 50;//(float)Math.Round(GetMatrixData(d.CanData, 36, 10) * 1.0f, 2, MidpointRounding.AwayFromZero);

                    DPCState = (float)Math.Round(GetMatrixData(d.CanData, 96, 2) * 1.0f, 2, MidpointRounding.AwayFromZero);
                }
                else if (d.CanId == 0x00000112 && d.CanData.Length > 0)
                {

                }
                else if (d.CanId == 0x00000120 && d.CanData.Length > 0)
                {
                    Msg120 = ValueHelper.GetHextStr(d.CanData);

                    Mot4Hall = 50;// GetMatrixData(d.CanData, 16, 8);
                    Mot6Hall = 50;//GetMatrixData(d.CanData, 24, 8);
                    Mot8Hall = 50;//GetMatrixData(d.CanData, 32, 8);
                    BackrestSeatHotPWM = ((GetMatrixData(d.CanData, 8, 8))) * 0.1f * 10;
                    SecRIHotPWM = (GetMatrixData(d.CanData, 0, 8)) * 0.1f * 10;
                }
            }
            catch (Exception)
            {

            }
        }

        public static ulong GetMatrixData(byte[] matrixData, int startBit, int bitLength)
        {
            var bsList = new List<string>();

            foreach (var bs in matrixData.Select(t => Convert.ToString(t, 2).PadLeft(8, '0')))
                for (var i = 0; i <= bs.Length - 1; i++)
                    bsList.Add(bs[i].ToString());

            var tempBs = new List<string>();
            for (var i = 0; i < bitLength; i++)
                tempBs.Add(bsList[startBit + i]);

            var str = string.Empty;
            for (var i = 0; i <= tempBs.Count - 1; i++)
                str += tempBs[i];
            return Convert.ToUInt64(str, 2);
        }

        ~DX1H_V3_EMC()
        {
            Dispose();
        }

        public void MainWork()
        {
            SendMsg();
            MonitorMsg();
        }

        public static object _sendLock = new object();
        public bool _isSleep = true;
        private readonly CanCommunicationMatrix.MotorolaMatrix _data0XFE = new CanCommunicationMatrix.MotorolaMatrix(0xFE, 64);
        public readonly CanCommunicationMatrix.MotorolaMatrix _data0X230 = new CanCommunicationMatrix.MotorolaMatrix(0x230, 32);
        private readonly CanCommunicationMatrix.MotorolaMatrix _data0X100 = new CanCommunicationMatrix.MotorolaMatrix(0x100, 48);
        private readonly CanCommunicationMatrix.MotorolaMatrix _data0x401 = new CanCommunicationMatrix.MotorolaMatrix(0x401, 64);
        private readonly Dictionary<DX1HOnOrOffType, MatrixValDefinition> _lampOperaterDic =
          new Dictionary<DX1HOnOrOffType, MatrixValDefinition>();

        private void SendMsg()
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = () =>
                {
                    try
                    {
                        var sendDatas = new List<CanBus.CanDataPackage>();
                        sendDatas.Add(new CanBus.CanDataPackage(_data0XFE.CanId, CanBus.CanProtocol.SpeedCanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, _data0XFE.MatrixData));

                        // 根据全局变量修改数据
                        {
                            var bits230 = Get230Bits();

                            // hsd1 start=14 len=10
                            {
                                var bits = GetBits(_hsd1Duty * 10, 10);

                                bits230[14] = bits[0];
                                bits230[15] = bits[1];
                                bits230[0] = bits[2];
                                bits230[1] = bits[3];
                                bits230[2] = bits[4];
                                bits230[3] = bits[5];
                                bits230[4] = bits[6];
                                bits230[5] = bits[7];
                                bits230[6] = bits[8];
                                bits230[7] = bits[9];
                            }

                            // hsd2 start=20 len=10
                            {
                                var bits = GetBits(_hsd2Duty * 10, 10);

                                bits230[20] = bits[0];
                                bits230[21] = bits[1];
                                bits230[22] = bits[2];
                                bits230[23] = bits[3];
                                bits230[8] = bits[4];
                                bits230[9] = bits[5];
                                bits230[10] = bits[6];
                                bits230[11] = bits[7];
                                bits230[12] = bits[8];
                                bits230[13] = bits[9];
                            }

                            // fan1 start=26 len=10
                            {
                                var bits = GetBits(_fan1Duty * 10, 10);

                                bits230[26] = bits[0];
                                bits230[27] = bits[1];
                                bits230[28] = bits[2];
                                bits230[29] = bits[3];
                                bits230[30] = bits[4];
                                bits230[31] = bits[5];
                                bits230[16] = bits[6];
                                bits230[17] = bits[7];
                                bits230[18] = bits[8];
                                bits230[19] = bits[9];
                            }

                            // fan2 start=32 len=10
                            {
                                var bits = GetBits(_fan2Duty * 10, 10);

                                bits230[32] = bits[0];
                                bits230[33] = bits[1];
                                bits230[34] = bits[2];
                                bits230[35] = bits[3];
                                bits230[36] = bits[4];
                                bits230[37] = bits[5];
                                bits230[38] = bits[6];
                                bits230[39] = bits[7];
                                bits230[24] = bits[8];
                                bits230[25] = bits[9];
                            }

                            // motor2 speed start=52 len=8
                            // motor2 sts start=50 len=2
                            {
                                var speedBits = GetBits(_motor2Speed, 8);
                                var stsBits = GetBits(_motor2Sts, 2);

                                bits230[52] = speedBits[0];
                                bits230[53] = speedBits[1];
                                bits230[54] = speedBits[2];
                                bits230[55] = speedBits[3];
                                bits230[40] = speedBits[4];
                                bits230[41] = speedBits[5];
                                bits230[42] = speedBits[6];
                                bits230[43] = speedBits[7];

                                bits230[50] = stsBits[0];
                                bits230[51] = stsBits[1];
                            }

                            // motor6 speed start=102 len=8
                            // motor6 sts start=100 len=2
                            {
                                var speedBits = GetBits(_motor6Speed, 8);
                                var stsBits = GetBits(_motor6Sts, 2);

                                bits230[102] = speedBits[0];
                                bits230[103] = speedBits[1];
                                bits230[88] = speedBits[2];
                                bits230[89] = speedBits[3];
                                bits230[90] = speedBits[4];
                                bits230[91] = speedBits[5];
                                bits230[92] = speedBits[6];
                                bits230[93] = speedBits[7];

                                bits230[100] = stsBits[0];
                                bits230[101] = stsBits[1];
                            }

                            // motor8 speed start=108 len=8
                            // motor8 sts start=106 len=2
                            {
                                var speedBits = GetBits(_motor8Speed, 8);
                                var stsBits = GetBits(_motor8Sts, 2);

                                bits230[108] = speedBits[0];
                                bits230[109] = speedBits[1];
                                bits230[110] = speedBits[2];
                                bits230[111] = speedBits[3];
                                bits230[96] = speedBits[4];
                                bits230[97] = speedBits[5];
                                bits230[98] = speedBits[6];
                                bits230[99] = speedBits[7];

                                bits230[106] = stsBits[0];
                                bits230[107] = stsBits[1];
                            }

                            // motor4 speed start=114 len=8
                            // motor4 sts start=112 len=2
                            {
                                var speedBits = GetBits(_motor4Speed, 8);
                                var stsBits = GetBits(_motor4Sts, 2);

                                bits230[114] = speedBits[0];
                                bits230[115] = speedBits[1];
                                bits230[116] = speedBits[2];
                                bits230[117] = speedBits[3];
                                bits230[118] = speedBits[4];
                                bits230[119] = speedBits[5];
                                bits230[104] = speedBits[6];
                                bits230[105] = speedBits[7];

                                bits230[112] = stsBits[0];
                                bits230[113] = stsBits[1];
                            }

                            BitsTo230Bytes(bits230);
                        }
                        sendDatas.Add(new CanBus.CanDataPackage(_data0X230.CanId, CanBus.CanProtocol.SpeedCanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, _data0X230.MatrixData));
                        SendCanMsg(sendDatas.ToArray());
                    }
                    catch (Exception)
                    {

                    }
                },
                Interval = 50
            });
        }

        private void MonitorMsg()
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = () =>
                {
                    try
                    {
                        lock (_lockTs)
                        {
                            var nowTs = HighPrecisionTimer.GetTimestamp();
                            var ts = HighPrecisionTimer.GetTimestampIntervalMs(_lastRevCanMsg, nowTs);
                            IsCanLoss = ts > 5000;
                            if (IsCanLoss)
                            {
                                Msg171 = string.Empty;
                                Msg149 = string.Empty;
                                Msg120 = string.Empty;
                            }
                            else
                            {
                                //Console.WriteLine("LIN正常");
                            }
                        }
                    }
                    catch (Exception)
                    { }
                },
                Interval = 50
            });
        }

        private void SendCanMsg(CanBus.CanDataPackage[] d)
        {
            if (!_isSleep && Can != null)
            {
                lock (_sendLock)
                {
                    Can.SendCanDatas(d);
                    Thread.Sleep(25);
                }
            }
        }

        [Description("打开Can发送")]
        public void StartScheduler()
        {
            _isSleep = false;
        }

        [Description("关闭Can发送")]
        public void StopScheduler()
        {
            _isSleep = true;
        }

        #region 负载动作

        //[Description("电机组1电机2启动")]
        //public void Group1Motro2Start_BackrestMotOn()
        //{
        //    _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiBackrestMotCmd_UB_On]);
        //    _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiBackrestMotCmdSwtHozlSts1_On]);
        //    _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiBackrestMotCmdMotorSpeed_On]);
        //}

        //[Description("电机组1电机2停")]
        //public void Group1Motro2_BackrestMotOff()
        //{
        //    //_data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiBackrestMotCmd_UB_Off]);
        //    _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiBackrestMotCmdSwtHozlSts1_Off]);
        //    _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiBackrestMotCmdMotorSpeed_Off]);
        //}
        //[Description("电机组2电机4动")]
        //public void Group2Motro4Start_LenMotOn()
        //{
        //    _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLenMotCmd_UB_On]);
        //    _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLenMotCmdSwtHozlSts1_On]);
        //    _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLenMotCmdMotorSpeed_On]);
        //}

        //[Description("电机组2电机4停")]
        //public void Group2Motro4_LenMotOff()
        //{
        //    //_data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLenMotCmd_UB_Off]);
        //    _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLenMotCmdSwtHozlSts1_Off]);
        //    _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLenMotCmdMotorSpeed_Off]);
        //}

        //[Description("电机组3电机6启动")]
        //public void Group3Motro6Start_BackrestMotOn()
        //{
        //    _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLegRstHozlMotCmd_UB_On]);
        //    _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLegRstHozlMotCmdSwtHozlSts1_On]);
        //    _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLegRstHozlMotCmdMotorSpeed_On]);
        //}

        //[Description("电机组3电机6停")]
        //public void Group3Motro6_BackrestMotOff()
        //{
        //    //_data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLegRstHozlMotCmd_UB_Off]);
        //    _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLegRstHozlMotCmdSwtHozlSts1_Off]);
        //    _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLegRstHozlMotCmdMotorSpeed_Off]);

        //}
        //[Description("电机组4电机8动")]
        //public void Group4Motro8Start_LenMotOn()
        //{
        //    _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLegRstVertMotCmd_UB_On]);
        //    _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLegRstVertMotCmdMotorSpeed_On]);
        //    _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLegRstVertMotCmdSwtHozlSts1_On]);
        //}

        //[Description("电机组4电机8停")]
        //public void Group4Motro8_LenMotOff()
        //{
        //    //_data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLegRstVertMotCmd_UB_Off]);
        //    _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLegRstVertMotCmdSwtHozlSts1_Off]);
        //    _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLegRstVertMotCmdMotorSpeed_Off]);
        //}

        //[Description("座椅靠背加热开始")]
        //public void SeatHeatgCmdBackSecRightOn(string dutyCycle)
        //{
        //    var DutyCycle = Convert.ToByte(dutyCycle, 10);
        //    _data0X230.UpdateData(new MatrixValDefinition(14, 10, DutyCycle));
        //    _data0X230.UpdateData(new MatrixValDefinition(47, 1, 1));
        //}
        //[Description("座椅靠背加热结束")]
        //public void SeatHeatgCmdBackSecRightOff()
        //{
        //    _data0X230.UpdateData(new MatrixValDefinition(14, 10, 0));
        //    _data0X230.UpdateData(new MatrixValDefinition(47, 1, 1));
        //}


        //[Description("座椅加热开始")]
        //public void SeatHeatgCmdCushSecRightOn(string dutyCycle)
        //{
        //    var DutyCycle = Convert.ToByte(dutyCycle, 10);
        //    _data0X230.UpdateData(new MatrixValDefinition(20, 10, DutyCycle));
        //    _data0X230.UpdateData(new MatrixValDefinition(46, 1, 1));
        //}
        //[Description("座椅加热结束")]
        //public void SeatHeatgCmdCushSecRightOff()
        //{
        //    _data0X230.UpdateData(new MatrixValDefinition(20, 10, 0));
        //    _data0X230.UpdateData(new MatrixValDefinition(46, 1, 1));
        //}

        //[Description("靠背通风开始")]
        //public void SeatVentnCmdBackSecRightOn(string dutyCycle)
        //{
        //    try
        //    {
        //        var DutyCycle = Convert.ToByte(dutyCycle, 10);
        //        //_data0X230.UpdateData(new MatrixValDefinition(26, 8, DutyCycle));
        //        _data0X230.UpdateData(new MatrixValDefinition(26, 10, DutyCycle));
        //        _data0X230.UpdateData(new MatrixValDefinition(45, 1, 1));
        //    }
        //    catch
        //    {

        //    }
        //}

        //[Description("靠背通风结束")]
        //public void SeatVentnCmdBackSecRightOff()
        //{
        //    _data0X230.UpdateData(new MatrixValDefinition(26, 10, 0));
        //    _data0X230.UpdateData(new MatrixValDefinition(45, 1, 1));
        //}

        //[Description("坐垫通风开始")]
        //public void SeatVentnCmdCushSecRightOn(string dutyCycle)
        //{
        //    var DutyCycle = Convert.ToByte(dutyCycle, 10);
        //    _data0X230.UpdateData(new MatrixValDefinition(32, 10, DutyCycle));
        //    _data0X230.UpdateData(new MatrixValDefinition(44, 1, 1));
        //}

        //[Description("坐垫通风结束")]
        //public void SeatVentnCmdCushSecRightOff()
        //{
        //    _data0X230.UpdateData(new MatrixValDefinition(32, 10, 0));
        //    _data0X230.UpdateData(new MatrixValDefinition(44, 1, 1));
        //}

        //private double _nowDuty = 0d;

        public void Heat100OnMotorOn()
        {
            try
            {
                //_nowDuty = 1.0d;
                //_data0X230.MatrixData = new byte[32] { 0xFA, 0x3E, 0x8F, 0xA3, 0xE8, 0xF6, 0x44, 0x00, 0x80, 0x00, 0x00, 0x19, 0x16, 0x45, 0x91, 0x00, 0x00, 0x03, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

                SeatHeatgCmdBackSecRi(100);
                SeatHeatgCmdCushSecRi(100);
                SeatVentnCmdBackSecRi(100);
                SeatVentnCmdCushSecRi(100);

                SecRiBackrestMotCmd(100, 1);
                SecRiLegRstHozlMotCmd(100, 1);
                SecRiLegRstVertMotCmd(100, 1);
                SecRiLenMotCmd(100, 1);
            }
            catch (Exception)
            {

            }
        }

        public void Heat100OnMotorOff()
        {
            try
            {
                //_nowDuty = 1.0d;
                //_data0X230.MatrixData = new byte[32] { 0xFA, 0x3E, 0x8F, 0xA3, 0xE8, 0xF0, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

                SeatHeatgCmdBackSecRi(100);
                SeatHeatgCmdCushSecRi(100);
                SeatVentnCmdBackSecRi(100);
                SeatVentnCmdCushSecRi(100);

                SecRiBackrestMotCmd(0, 0);
                SecRiLegRstHozlMotCmd(0, 0);
                SecRiLegRstVertMotCmd(0, 0);
                SecRiLenMotCmd(0, 0);
            }
            catch (Exception)
            {

            }
        }

        public void Heat50OnMotorOn()
        {
            try
            {
                //_nowDuty = 0.5d;
                //_data0X230.MatrixData = new byte[32] { 0x7D, 0x1F, 0x47, 0xD1, 0xF4, 0xF6, 0x44, 0x00, 0x80, 0x00, 0x00, 0x19, 0x16, 0x45, 0x91, 0x00, 0x00, 0x03, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

                SeatHeatgCmdBackSecRi(50);
                SeatHeatgCmdCushSecRi(50);
                SeatVentnCmdBackSecRi(50);
                SeatVentnCmdCushSecRi(50);

                SecRiBackrestMotCmd(100, 1);
                SecRiLegRstHozlMotCmd(100, 1);
                SecRiLegRstVertMotCmd(100, 1);
                SecRiLenMotCmd(100, 1);
            }
            catch (Exception)
            {

            }
        }

        public void Heat50OnMotorOff()
        {
            try
            {
                //_nowDuty = 0.5d;
                //_data0X230.MatrixData = new byte[32] { 0x7D, 0x1F, 0x47, 0xD1, 0xF4, 0xF0, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

                SeatHeatgCmdBackSecRi(50);
                SeatHeatgCmdCushSecRi(50);
                SeatVentnCmdBackSecRi(50);
                SeatVentnCmdCushSecRi(50);

                SecRiBackrestMotCmd(0, 0);
                SecRiLegRstHozlMotCmd(0, 0);
                SecRiLegRstVertMotCmd(0, 0);
                SecRiLenMotCmd(0, 0);
            }
            catch (Exception)
            {

            }
        }

        public void AllOff()
        {
            try
            {
                //_nowDuty = 0d;
                //_data0X230.MatrixData = new byte[32] { 0x00, 0x00, 0x00, 0x00, 0x00, 0xF0, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

                SeatHeatgCmdBackSecRi(0);
                SeatHeatgCmdCushSecRi(0);
                SeatVentnCmdBackSecRi(0);
                SeatVentnCmdCushSecRi(0);

                SecRiBackrestMotCmd(0, 0);
                SecRiLegRstHozlMotCmd(0, 0);
                SecRiLegRstVertMotCmd(0, 0);
                SecRiLenMotCmd(0, 0);
            }
            catch (Exception)
            {

            }
        }

        private void SeatHeatgCmdBackSecRi(int duty)
        {
            if (duty > 0 && _isHsd1OverCurr)
            {
                _hsd1Duty = 0;
                return;
            }
            _hsd1Duty = duty;
            //UpdateData230(duty * 10, 14, 10);           
        }

        private void SeatHeatgCmdCushSecRi(int duty)
        {
            if (duty > 0 && _isHsd2OverCurr)
            {
                _hsd2Duty = 0;
                return;
            }
            _hsd2Duty = duty;
            //UpdateData230(duty * 10, 20, 10);
        }

        private void SeatVentnCmdBackSecRi(int duty)
        {
            _fan1Duty = duty;
            //UpdateData230(duty * 10, 26, 10);
        }

        private void SeatVentnCmdCushSecRi(int duty)
        {
            _fan2Duty = duty;
            //UpdateData230(duty * 10, 32, 10);
        }

        /// <summary>
        /// MOTOR2
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="sts"></param>
        private void SecRiBackrestMotCmd(int speed, int sts)
        {
            if (sts > 0 && _isMotor2OverCurr)
            {
                _motor2Speed = 0;
                _motor2Sts = 0;
                return;
            }
            _motor2Speed = speed;
            _motor2Sts = sts;
            //UpdateData230(speed, 52, 8);
            //UpdateData230(sts, 50, 2);
        }

        /// <summary>
        /// MOTOR6
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="sts"></param>
        private void SecRiLegRstHozlMotCmd(int speed, int sts)
        {
            if (sts > 0 && _isMotor6OverCurr)
            {
                _motor6Speed = 0;
                _motor6Sts = 0;
                return;
            }
            _motor6Speed = speed;
            _motor6Sts = sts;
            //UpdateData230(speed, 102, 8);
            //UpdateData230(sts, 100, 2);
        }

        /// <summary>
        /// MOTOR8
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="sts"></param>
        private void SecRiLegRstVertMotCmd(int speed, int sts)
        {
            if (sts > 0 && _isMotor8OverCurr)
            {
                _motor8Speed = 0;
                _motor8Sts = 0;
                return;
            }
            _motor8Speed = speed;
            _motor8Sts = sts;
            //UpdateData230(speed, 108, 8);
            //UpdateData230(sts, 106, 2);
        }

        /// <summary>
        /// MOTOR4
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="sts"></param>
        private void SecRiLenMotCmd(int speed, int sts)
        {
            if (sts > 0 && _isMotor4OverCurr)
            {
                _motor4Speed = 0;
                _motor4Sts = 0;
                return;
            }
            _motor4Speed = speed;
            _motor4Sts = sts;
            //UpdateData230(speed, 114, 8);
            //UpdateData230(sts, 112, 2);
        }

        private int _hsd1Duty;
        private int _hsd2Duty;
        private int _fan1Duty;
        private int _fan2Duty;

        private int _motor2Speed;
        private int _motor2Sts;

        private int _motor4Speed;
        private int _motor4Sts;

        private int _motor6Speed;
        private int _motor6Sts;

        private int _motor8Speed;
        private int _motor8Sts;

        private List<string> GetBits(int value, int len)
        {
            var valBits = Convert.ToString(value, 2).PadLeft(64, '0');
            var toWriteBits = new List<string>();
            for (var i = valBits.Length - 1; i >= 0; i--)
                toWriteBits.Add(valBits[i].ToString());

            var bbIndex = 0;
            var returnBits = new List<string>();
            for (var i = 0; i < len; i++)
            {
                returnBits.Add(toWriteBits[bbIndex]);
                bbIndex++;
            }

            return returnBits;
        }

        private List<string> Get230Bits()
        {
            var listBits = new List<string>();
            for (int i = 0; i < _data0X230.MatrixData.Length; i++)
            {
                var str = Convert.ToString(_data0X230.MatrixData[i], 2).PadLeft(8, '0');
                for (var j = str.Length - 1; j >= 0; j--)
                    listBits.Add(str[j].ToString());
            }

            return listBits;
        }

        private void BitsTo230Bytes(List<string> listBits)
        {
            var bIndex = 0;
            var bBytes = new byte[32];
            for (int i = 0; i < listBits.Count; i = i + 8)
            {
                var str = string.Format("{7}{6}{5}{4}{3}{2}{1}{0}", listBits[i], listBits[i + 1], listBits[i + 2], listBits[i + 3], listBits[i + 4], listBits[i + 5], listBits[i + 6], listBits[i + 7]);
                var b = Convert.ToByte(str, 2);
                bBytes[bIndex] = b;
                bIndex++;
            }
            Array.Copy(bBytes, 0, _data0X230.MatrixData, 0, bBytes.Length);
        }
    }

    //private void UpdateData230(int value, int startBit, int len)
    //{
    //    var listBits = new List<string>();
    //    for (int i = 0; i < _data0X230.MatrixData.Length; i++)
    //    {
    //        var str = Convert.ToString(_data0X230.MatrixData[i], 2).PadLeft(8, '0');
    //        for (var j = str.Length - 1; j >= 0; j--)
    //            listBits.Add(str[j].ToString());
    //    }

    //    listBits[47] = listBits[46] = listBits[45] = listBits[44] = listBits[71] = listBits[137] = listBits[136] = listBits[151] = "1";

    //    var valBits = Convert.ToString(value, 2).PadLeft(64, '0');
    //    var toWriteBits = new List<string>();
    //    for (var i = valBits.Length - 1; i >= 0; i--)
    //        toWriteBits.Add(valBits[i].ToString());

    //    var bbIndex = 0;
    //    for (var i = startBit; i < startBit + len; i++)
    //    {
    //        listBits[i] = toWriteBits[bbIndex];
    //        bbIndex++;
    //    }

    //    var bIndex = 0;
    //    var bBytes = new byte[32];
    //    for (int i = 0; i < listBits.Count; i = i + 8)
    //    {
    //        var str = string.Format("{7}{6}{5}{4}{3}{2}{1}{0}", listBits[i], listBits[i + 1], listBits[i + 2], listBits[i + 3], listBits[i + 4], listBits[i + 5], listBits[i + 6], listBits[i + 7]);
    //        var b = Convert.ToByte(str, 2);
    //        bBytes[bIndex] = b;
    //        bIndex++;
    //    }
    //    Array.Copy(bBytes, 0, _data0X230.MatrixData, 0, bBytes.Length);
    //}

    #endregion

    internal enum DX1HOnOrOffType
    {
        /// <summary>
        /// 间隔10ms 电机组1电机2 电机动
        /// </summary>
        [MatrixValDefinition(71, 1, 1)]
        [Description("0x230")]
        SecRiBackrestMotCmd_UB_On,
        /// <summary>
        /// 间隔10ms 电机组1电机2 电机停
        /// </summary>
        [MatrixValDefinition(71, 1, 0)]
        [Description("0x230")]
        SecRiBackrestMotCmd_UB_Off,
        /// <summary>
        /// 间隔10ms 电机组1电机2 电机动
        /// </summary> 
        [MatrixValDefinition(50, 2, 1)]
        [Description("0x230")]
        SecRiBackrestMotCmdSwtHozlSts1_On,
        /// <summary>
        /// 间隔10ms 电机组1电机2 电机动
        /// </summary> 
        [MatrixValDefinition(50, 2, 0)]
        [Description("0x230")]
        SecRiBackrestMotCmdSwtHozlSts1_Off,
        /// <summary>
        /// 间隔10ms 电机组1电机2 电机动
        /// </summary> 
        [MatrixValDefinition(52, 8, 100)]
        [Description("0x230")]
        SecRiBackrestMotCmdMotorSpeed_On,
        /// <summary>
        /// 间隔10ms 电机组1电机2 电机动
        /// </summary> 
        [MatrixValDefinition(52, 8, 0)]
        [Description("0x230")]
        SecRiBackrestMotCmdMotorSpeed_Off,

        /// <summary>
        /// 间隔10ms 电机组3电机6 电机动
        /// </summary>
        [MatrixValDefinition(137, 1, 1)]
        [Description("0x230")]
        SecRiLegRstHozlMotCmd_UB_On,
        /// <summary>
        /// 间隔10ms 电机组3电机6 电机停
        /// </summary>
        [MatrixValDefinition(137, 1, 0)]
        [Description("0x230")]
        SecRiLegRstHozlMotCmd_UB_Off,
        /// <summary>
        /// 间隔10ms 电机组3电机6 电机动
        /// </summary> 
        [MatrixValDefinition(100, 2, 1)]
        [Description("0x230")]
        SecRiLegRstHozlMotCmdSwtHozlSts1_On,
        /// <summary>
        /// 间隔10ms 电机组3电机6 电机动
        /// </summary> 
        [MatrixValDefinition(100, 2, 0)]
        [Description("0x230")]
        SecRiLegRstHozlMotCmdSwtHozlSts1_Off,
        /// <summary>
        /// 间隔10ms 电机组3电机6 电机动
        /// </summary> 
        [MatrixValDefinition(102, 8, 100)]
        [Description("0x230")]
        SecRiLegRstHozlMotCmdMotorSpeed_On,
        /// <summary>
        /// 间隔10ms 电机组3电机6 电机动
        /// </summary> 
        [MatrixValDefinition(102, 8, 0)]
        [Description("0x230")]
        SecRiLegRstHozlMotCmdMotorSpeed_Off,

        /// <summary>
        /// 间隔10ms 电机组4电机8 电机动
        /// </summary>
        [MatrixValDefinition(136, 1, 1)]
        [Description("0x230")]
        SecRiLegRstVertMotCmd_UB_On,
        /// <summary>
        /// 间隔10ms 电机组4电机8 电机停
        /// </summary>
        [MatrixValDefinition(136, 1, 0)]
        [Description("0x230")]
        SecRiLegRstVertMotCmd_UB_Off,
        /// <summary>
        /// 间隔10ms 电机组4电机8 电机动
        /// </summary> 
        [MatrixValDefinition(106, 2, 1)]
        [Description("0x230")]
        SecRiLegRstVertMotCmdSwtHozlSts1_On,
        /// <summary>
        /// 间隔10ms 电机组4电机8 电机动
        /// </summary> 
        [MatrixValDefinition(106, 2, 0)]
        [Description("0x230")]
        SecRiLegRstVertMotCmdSwtHozlSts1_Off,
        /// <summary>
        /// 间隔10ms 电机组4电机8 电机动
        /// </summary> 
        [MatrixValDefinition(108, 8, 100)]
        [Description("0x230")]
        SecRiLegRstVertMotCmdMotorSpeed_On,
        /// <summary>
        /// 间隔10ms 电机组4电机8 电机动
        /// </summary> 
        [MatrixValDefinition(108, 8, 0)]
        [Description("0x230")]
        SecRiLegRstVertMotCmdMotorSpeed_Off,


        /// <summary>
        /// 间隔10ms 电机组2电机4 电机动
        /// </summary>
        [MatrixValDefinition(151, 1, 1)]
        [Description("0x230")]
        SecRiLenMotCmd_UB_On,
        /// <summary>
        /// 间隔10ms 电机组2电机4 电机停
        /// </summary>
        [MatrixValDefinition(151, 1, 0)]
        [Description("0x230")]
        SecRiLenMotCmd_UB_Off,
        /// <summary>
        /// 间隔10ms 电机组2电机4 电机动
        /// </summary>
        [MatrixValDefinition(114, 8, 100)]
        [Description("0x230")]
        SecRiLenMotCmdMotorSpeed_On,
        /// <summary>
        /// 间隔10ms 电机组2电机4 电机停
        /// </summary>
        [MatrixValDefinition(114, 8, 0)]
        [Description("0x230")]
        SecRiLenMotCmdMotorSpeed_Off,
        /// <summary>
        /// 间隔10ms 
        /// </summary> 电机组2电机4 电机动
        [MatrixValDefinition(112, 2, 1)]
        [Description("0x230")]
        SecRiLenMotCmdSwtHozlSts1_On,
        /// <summary>
        /// 间隔10ms 
        /// </summary> 电机组2电机4 电机动
        [MatrixValDefinition(112, 2, 0)]
        [Description("0x230")]
        SecRiLenMotCmdSwtHozlSts1_Off
    }
}
