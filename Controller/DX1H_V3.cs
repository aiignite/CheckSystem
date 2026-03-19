using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;

namespace Controller
{
    [Description("Can-Product,DX1H座椅模块")]
    public sealed class DX1H_V3 : ControllerBase
    {
        public struct CandateMode
        {
            public uint CanID { get; set; }

            public byte[] Candata { get; set; }
        }

        #region Files


        public static object CreateTimeStampLock = new object();
        [Description("R,风扇供电")]
        public float FanPower;
        [Description("R,FanCurrent1")]
        public float FanCurrent1;
        [Description("R,FanCurrent2")]
        public float FanCurrent2;

        [Description("R,AI_BackRest_CushionTilt_LegRestVert_Switch1")]
        public float AI_BackRest_CushionTilt_LegRestVert_Switch1 = 9999;
        [Description("R,AI_Slide_LegRestHori_Switch2")]
        public float AI_Slide_LegRestHori_Switch2 = 9999;
        [Description("R,AI_Lumbar_4W_Switch3")]
        public float AI_Lumbar_4W_Switch3 = 9999;
        [Description("R,AI_SWITCH4_TBD")]
        public float AI_SWITCH4_TBD = 9999;
        [Description("R,AI_SWITCH5_TBD")]
        public float AI_SWITCH5_TBD = 9999;
        [Description("R,AI_SWITCH6_TBD")]
        public float AI_SWITCH6_TBD = 9999;
        [Description("R,AI_SWITCH8_TBD")]
        public float AI_SWITCH8_TBD = 9999;
        [Description("R,AI_SWITCH9_TBD")]
        public float AI_SWITCH9_TBD = 9999;
        [Description("R,AI_Micro_SWITCH11")]
        public float AI_Micro_SWITCH11 = 9999;
        [Description("R,AI_ODS_SWITCH12")]
        public float AI_ODS_SWITCH12 = 9999;
        [Description("R,AI_EasyEntry_SWITCH13")]
        public float AI_EasyEntry_SWITCH13 = 9999;
        [Description("R,AI_M1_M2_SWITCH14")]
        public float AI_M1_M2_SWITCH14 = 9999;
        [Description("R,AI_ZeroGravity_RESET_SWITCH15")]
        public float AI_ZeroGravity_RESET_SWITCH15 = 9999;
        [Description("R,AI_Pedal_SWITCH16")]
        public float AI_Pedal_SWITCH16 = 9999;

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
        [Description("R,电机2频率")]
        public float Mot2P;
        [Description("R,电机4频率")]
        public float Mot4P;
        [Description("R,电机6频率")]
        public float Mot6P;
        [Description("R,电机8频率")]
        public float Mot8P;
        [Description("R,DPCState")]
        public float DPCState;
        [Description("R,obs开关状态")]
        public string OBSSwitchState;

        [Description("R/W,通讯板子ID")]
        public string CanControlleName;
        public CanBus Can;
        public static object _sendLock = new object();
        public readonly Thread _mainWorkThread;
        private bool _isSleep = true;
        public string IsReciveCanMeaage = "NG";
        private readonly List<CandateMode> _ftpBuffer = new List<CandateMode>();
        private bool _isInFtp;
        private bool _isInRead;
        private uint _responseCanId;
        private readonly CanCommunicationMatrix.MotorolaMatrix _data0XFE = new CanCommunicationMatrix.MotorolaMatrix(0xFE, 64);
        private readonly CanCommunicationMatrix.MotorolaMatrix _data0X230 = new CanCommunicationMatrix.MotorolaMatrix(0x230, 32);
        private readonly CanCommunicationMatrix.MotorolaMatrix _data0X120 = new CanCommunicationMatrix.MotorolaMatrix(0x120, 48);
        private readonly CanCommunicationMatrix.MotorolaMatrix _data0X201 = new CanCommunicationMatrix.MotorolaMatrix(0x201, 50);
        private readonly CanCommunicationMatrix.MotorolaMatrix _data0X340 = new CanCommunicationMatrix.MotorolaMatrix(0x340, 24);
        private readonly CanCommunicationMatrix.MotorolaMatrix _data0x401 = new CanCommunicationMatrix.MotorolaMatrix(0x401, 64);
        private readonly Dictionary<DX1HOnOrOffType, MatrixValDefinition> _lampOperaterDic =
          new Dictionary<DX1HOnOrOffType, MatrixValDefinition>();
        /// <summary>
        /// 超时信号量
        /// </summary>
        private readonly EventWaitHandle _recvWaitHandle = new AutoResetEvent(false);
        [Description("R/W,KL30读取")]
        public bool KL30TestState = false;
        [Description("R/W,Lin通讯")]
        public bool LinTestState = false;
        [Description("R/W,开关电阻")]
        public bool SwitchTestState = false;
        [Description("R/W,座椅靠背开关")]
        public bool SeatNTCTestState = false;
        [Description("R/W,风扇开关")]
        public bool FanTestState = false;
        [Description("R/W,软件版本")]
        public bool APPVarState = false;
        [Description("R/W,马达开关")]
        public bool MotorTestState = false;
        [Description("R/W,腰托开关")]
        public bool LumbarSupportState = false;
        #endregion

        public DX1H_V3(string name) : base(name)
        {
            CanBus.PushCanMsg += Can_PushCanMsg;
            //_data0X100.MatrixData = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x20, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x64, 0x00, 0x00, 0x64, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, };
            _data0x401.MatrixData = new byte[] { 0xFA, 0x3E, 0x8F, 0xA1, 0x50, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            //_data0XFE.MatrixData = new byte[] {0x00, 0x00, 0x00, 0x32, 0x79, 0x52, 0x00, 0x00,  0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, };
            _data0XFE.MatrixData = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0xC0, 0xCE, 0x80, 0x00, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, };
            foreach (var temp in Enum.GetValues(typeof(DX1HOnOrOffType)).Cast<DX1HOnOrOffType>())
                _lampOperaterDic.Add(temp, temp.GetCustomAttribute<MatrixValDefinition>());
            _mainWorkThread = new Thread(CyNormalCyclicTimer);
            _mainWorkThread.IsBackground = true;
            _mainWorkThread.Start();
        }

        private void Can_PushCanMsg(string name, CanBus.CanDataPackage data, CanBus.OnPushCanDataType onPushCanDataType)
        {
            if (Can != null && Can.Name == name && name.Contains(this.CanControlleName) &&
              (onPushCanDataType == CanBus.OnPushCanDataType.FilterRx || onPushCanDataType == CanBus.OnPushCanDataType.NotFilterRx) &&
              data.CanId != 0x00 && (data.CanId == CanDiagnosisResponseCanId))
            {
                if (_isInFtp)
                {
                    _ftpBuffer.Add(new CandateMode() { CanID = data.CanId, Candata = data.CanData });
                    _recvWaitHandle.Set();
                    _isInFtp = false;
                }

                Console.WriteLine(@"{0}: {1}", data.CanId, ValueHelper.GetHextStrWithOx(data.CanData));
            }

            if (Can != null && Can.Name == name && name.Contains(this.CanControlleName) &&
             (onPushCanDataType == CanBus.OnPushCanDataType.FilterRx || onPushCanDataType == CanBus.OnPushCanDataType.NotFilterRx) &&
             _bReadCanId != 0x00 && (data.CanId == _bReadCanId))
            {
                if (_bReadState)
                {
                    Array.Copy(data.CanData, _bReadBuff, data.CanData.Length);
                }
            }
        }

        #region 读取状态

        private bool _bReadState;
        private byte[] _bReadBuff;
        private uint _bReadCanId;

        public void ReadState(bool isLeft)
        {
            _bReadCanId = isLeft ? (uint)0x197 : (uint)0x171;
            _bReadState = true;
            _bReadBuff = null;
            Thread.Sleep(2000); // 可以改小点
            _bReadState = false;
            _bReadCanId = 0x00;

            if (_bReadBuff != null)
            {
                // 解析
            }
        }

        #endregion


        ~DX1H_V3()
        {
            Dispose();
        }

        #region 二维码

        public string BarcodeContent;

        public static object _lockbarcode = new object();
        public void CreatBarcode(string partno, string supply)
        {
            lock (_lockbarcode)
            {
                BarcodeContent = "";
                string num = "0000001";
                string date = DateTime.Now.ToString("yy-MM-dd");
                date = date.Replace("-", "");

                string filepath = Path.Combine(Directory.GetCurrentDirectory(), "DX1H序列号");
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }
                string filename = Path.Combine(filepath, $"{DateTime.Now.ToString("yyyy-MM-dd")}.txt");
                if (!File.Exists(filename))
                {
                    File.WriteAllText(filename, "0");
                }
                int a = 1;
                try
                {
                    a = int.Parse(File.ReadAllText(filename)) + 1;
                    File.WriteAllText(filename, a.ToString());
                }
                catch (Exception ex)
                {
                    File.WriteAllText(filename, "0");
                }

                num = a.ToString().PadLeft(7, '0');
                BarcodeContent = partno + date + num + supply;
            }

        }

        [Description("设置左右")]
        public void SetLeft(bool isLeft)
        {
            if (isLeft)
            {
                CanDiagnosisRequestPhyCanId = 0x736;
                CanDiagnosisResponseCanId = 0x636;
                _data0XFE.MatrixData = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0xC0, 0xCE, 0x80, 0x00, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, };
            }
            else
            {
                CanDiagnosisRequestPhyCanId = 0x781;
                CanDiagnosisResponseCanId = 0x681;
                _data0XFE.MatrixData = new byte[] { 0x00, 0x00, 0x00, 0x07, 0x81, 0x98, 0x00, 0x00, 0x84, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x5A, 0x40, 0xCA, 0x19, 0x40, 0xCA, 0xCA, 0x00, 0x00, 0x00, 0x00, 0x00, };
            }
        }

        #endregion

        [Description("刷新信息")]
        public void GetCurrentMessage()
        {
            IsReciveCanMeaage = "NG";
            _ftpBuffer.Clear();
            _isInFtp = true;
            Thread.Sleep(600);
            _isInFtp = false;
            KL30_1Voltage = 0;
            KL30_2Voltage = 0;
            KL30_3Voltage = 0;

            NTC1Temperature = 0;
            NTC2Temperature = 0;
            SecRIHotCurrent = 0;
            BackrestSeatHotCurrent = 0;
            IsReciveCanMeaage = _ftpBuffer.Count > 0 ? "OK" : "NG";
            try
            {
                var getLast171Message = _ftpBuffer.Last(t => t.CanID == 0x00000171);

                var message = ValueHelper.GetHextStr(getLast171Message.Candata);
                var data = new CanCommunicationMatrix.IntelMatrix(0x117, getLast171Message.Candata.Length);
                Array.Copy(getLast171Message.Candata, data.MatrixData, getLast171Message.Candata.Length);

                KL30_1Voltage = data.GetMatrixData(40, 8) * 0.1f;
                KL30_2Voltage = ((data.GetMatrixData(78, 3)) * 16 + data.GetMatrixData(74, 4)) * 0.1f;
                KL30_3Voltage = ((data.GetMatrixData(94, 4)) * 16 + data.GetMatrixData(82, 4)) * 0.1f;

                NTC1Temperature = data.GetMatrixData(17, 11);
                NTC2Temperature = data.GetMatrixData(13, 11);

                SecRIHotCurrent = ((data.GetMatrixData(90, 4)) * 16 + data.GetMatrixData(102, 4)) * 0.1f;
                BackrestSeatHotCurrent = ((data.GetMatrixData(98, 4)) * 16 + data.GetMatrixData(105, 4)) * 0.1f;

                var getLast149Message = _ftpBuffer.Last(t => t.CanID == 0x00000149);
                var data_149 = new CanCommunicationMatrix.IntelMatrix(0x149, getLast149Message.Candata.Length);
                Array.Copy(getLast171Message.Candata, data_149.MatrixData, getLast149Message.Candata.Length);
                Mot2Current = data_149.GetMatrixData(38, 10);
                Mot4Current = data_149.GetMatrixData(86, 10);
                Mot6Current = data_149.GetMatrixData(62, 10);
                Mot8Current = data_149.GetMatrixData(66, 10);
                Mot2Hall = data_149.GetMatrixData(42, 10);
                Mot4Hall = data_149.GetMatrixData(138, 12);
                BackrestSeatHotPWM = ((data_149.GetMatrixData(6, 2)) * 16 + (data_149.GetMatrixData(4, 2)) * 8 + (data_149.GetMatrixData(2, 2)) * 2 + data_149.GetMatrixData(0, 2)) * 0.1f;
                DPCState = data_149.GetMatrixData(102, 2);

                var getLast112Message = _ftpBuffer.Last(t => t.CanID == 0x00000112);
                var data_112 = new CanCommunicationMatrix.IntelMatrix(0x112, getLast112Message.Candata.Length);
                Array.Copy(getLast112Message.Candata, data_112.MatrixData, getLast112Message.Candata.Length);
                Mot6Hall = data_112.GetMatrixData(12, 12);
                Mot8Hall = data_112.GetMatrixData(28, 12);
                SecRIHotPWM = ((data_112.GetMatrixData(10, 2)) * 16 + (data_112.GetMatrixData(85, 3)) * 2 + (data_112.GetMatrixData(26, 2))) * 0.1f;
            }
            catch
            {

            }
        }
        public void CyNormalCyclicTimer()
        {
            while (_mainWorkThread.IsAlive)
            {
                if (!_mainWorkThread.IsAlive)
                    break;

                Thread.Sleep(25);

                lock (_sendLock)
                {
                    try
                    {
                        if (Can == null)
                            continue;

                        if (!_isSleep)
                        {
                            var sendDatas = new List<CanBus.CanDataPackage>();
                            sendDatas.Add(new CanBus.CanDataPackage(_data0XFE.CanId, CanBus.CanProtocol.SpeedCanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, _data0XFE.MatrixData));
                            sendDatas.Add(new CanBus.CanDataPackage(_data0X230.CanId, CanBus.CanProtocol.SpeedCanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, _data0X230.MatrixData));
                            sendDatas.Add(new CanBus.CanDataPackage(_data0X201.CanId, CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, _data0X201.MatrixData));
                            //sendDatas.Add(new CanBus.CanDataPackage(_data0X340.CanId, CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, _data0X340.MatrixData));
                            //sendDatas.Add(new CanBus.CanDataPackage(_data0X100.CanId, CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, _data0X230.MatrixData));
                            sendDatas.Add(new CanBus.CanDataPackage(_data0X120.CanId, CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, _data0X120.MatrixData));
                            Can.SendCanDatas(sendDatas.ToArray());
                            Thread.Sleep(10);
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
        }

        [Description("打开Can发送")]
        public void LinStartScheduler()
        {
            _isSleep = false;
        }

        [Description("关闭Can发送")]

        #region 负载动作

        public void LinStopScheduler()
        {
            _isSleep = true;
        }

        [Description("左电机组1电机2启动")]
        public void Group1Motro2Start_BackrestMotOnL()
        {
            _data0X120.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecLeBackrestMotCmd_UB_On]);
            _data0X120.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecLeBackrestMotCmdSwtHozlSts1_On]);
            _data0X120.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecLeBackrestMotCmdMotorSpeed_On]);
        }

        [Description("左电机组1电机2停")]
        public void Group1Motro2_BackrestMotOffL()
        {
            //_data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiBackrestMotCmd_UB_Off]);
            _data0X120.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecLeBackrestMotCmdSwtHozlSts1_Off]);
            _data0X120.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecLeBackrestMotCmdMotorSpeed_Off]);
        }

        [Description("左电机组2电机4动")]
        public void Group2Motro4Start_LenMotOnL()
        {
            _data0X120.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecLeLenMotCmd_UB_On]);
            _data0X120.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecLeLenMotCmdSwtHozlSts1_On]);
            _data0X120.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecLeLenMotCmdMotorSpeed_On]);
        }

        [Description("左电机组2电机4停")]
        public void Group2Motro4_LenMotOffL()
        {
            //_data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLenMotCmd_UB_Off]);
            _data0X120.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecLeLenMotCmdSwtHozlSts1_Off]);
            _data0X120.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecLeLenMotCmdMotorSpeed_Off]);
        }

        [Description("左电机组3电机6启动")]
        public void Group3Motro6Start_BackrestMotOnL()
        {
            _data0X120.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecLeLegRstHozlMotCmd_UB_On]);
            _data0X120.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecLeLegRstHozlMotCmdSwtHozlSts1_On]);
            _data0X120.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecLeLegRstHozlMotCmdMotorSpeed_On]);
        }

        [Description("左电机组3电机6停")]
        public void Group3Motro6_BackrestMotOffL()
        {
            //_data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLegRstHozlMotCmd_UB_Off]);
            _data0X120.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecLeLegRstHozlMotCmdSwtHozlSts1_Off]);
            _data0X120.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecLeLegRstHozlMotCmdMotorSpeed_Off]);

        }

        [Description("左电机组4电机8动")]
        public void Group4Motro8Start_LenMotOnL()
        {
            _data0X120.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecLeLegRstVertMotCmd_UB_On]);
            _data0X120.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecLeLegRstVertMotCmdMotorSpeed_On]);
            _data0X120.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecLeLegRstVertMotCmdSwtVertSts1_On]);
        }

        [Description("左电机组4电机8停")]
        public void Group4Motro8_LenMotOffL()
        {
            //_data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLegRstVertMotCmd_UB_Off]);
            _data0X120.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecLeLegRstVertMotCmdSwtVertSts1_Off]);
            _data0X120.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecLeLegRstVertMotCmdMotorSpeed_Off]);
        }

        [Description("右电机组1电机2启动")]
        public void Group1Motro2Start_BackrestMotOnR()
        {
            _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiBackrestMotCmd_UB_On]);
            _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiBackrestMotCmdSwtHozlSts1_On]);
            _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiBackrestMotCmdMotorSpeed_On]);
        }

        [Description("右电机组1电机2停")]
        public void Group1Motro2_BackrestMotOffR()
        {
            //_data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiBackrestMotCmd_UB_Off]);
            _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiBackrestMotCmdSwtHozlSts1_Off]);
            _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiBackrestMotCmdMotorSpeed_Off]);
        }

        [Description("右电机组2电机4动")]
        public void Group2Motro4Start_LenMotOnR()
        {
            _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLenMotCmd_UB_On]);
            _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLenMotCmdSwtHozlSts1_On]);
            _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLenMotCmdMotorSpeed_On]);
        }

        [Description("右电机组2电机4停")]
        public void Group2Motro4_LenMotOffR()
        {
            //_data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLenMotCmd_UB_Off]);
            _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLenMotCmdSwtHozlSts1_Off]);
            _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLenMotCmdMotorSpeed_Off]);
        }

        [Description("右电机组3电机6启动")]
        public void Group3Motro6Start_BackrestMotOnR()
        {
            _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLegRstHozlMotCmd_UB_On]);
            _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLegRstHozlMotCmdSwtHozlSts1_On]);
            _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLegRstHozlMotCmdMotorSpeed_On]);
        }

        [Description("右电机组3电机6停")]
        public void Group3Motro6_BackrestMotOffR()
        {
            //_data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLegRstHozlMotCmd_UB_Off]);
            _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLegRstHozlMotCmdSwtHozlSts1_Off]);
            _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLegRstHozlMotCmdMotorSpeed_Off]);

        }

        [Description("右电机组4电机8动")]
        public void Group4Motro8Start_LenMotOnR()
        {
            _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLegRstVertMotCmd_UB_On]);
            _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLegRstVertMotCmdMotorSpeed_On]);
            _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLegRstVertMotCmdSwtHozlSts1_On]);
        }

        [Description("右电机组4电机8停")]
        public void Group4Motro8_LenMotOffR()
        {
            //_data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLegRstVertMotCmd_UB_Off]);
            _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLegRstVertMotCmdSwtHozlSts1_Off]);
            _data0X230.UpdateData(_lampOperaterDic[DX1HOnOrOffType.SecRiLegRstVertMotCmdMotorSpeed_Off]);
        }

        [Description("右座椅加热开始")]
        public void SeatHeatgCmdCushSecRightOn(string dutyCycle)
        {
            var DutyCycle = Convert.ToByte(dutyCycle, 10);
            _data0X230.UpdateData(new MatrixValDefinition(20, 10, DutyCycle));
            _data0X230.UpdateData(new MatrixValDefinition(46, 1, 1));
        }

        [Description("右座椅加热结束")]
        public void SeatHeatgCmdCushSecRightOff()
        {
            _data0X230.UpdateData(new MatrixValDefinition(20, 10, 0));
            _data0X230.UpdateData(new MatrixValDefinition(46, 1, 1));
        }

        [Description("右座椅靠背加热开始")]
        public void SeatHeatgCmdBackSecRightOn(string dutyCycle)
        {
            var DutyCycle = Convert.ToByte(dutyCycle, 10);
            _data0X230.UpdateData(new MatrixValDefinition(14, 10, DutyCycle));
            _data0X230.UpdateData(new MatrixValDefinition(47, 1, 1));
        }

        [Description("右座椅靠背加热结束")]
        public void SeatHeatgCmdBackSecRightOff()
        {
            _data0X230.UpdateData(new MatrixValDefinition(14, 10, 0));
            _data0X230.UpdateData(new MatrixValDefinition(47, 1, 1));
        }

        [Description("左座椅加热开始")]
        public void SeatHeatgCmdCushSecLeftOn(string dutyCycle)
        {
            var DutyCycle = Convert.ToByte(dutyCycle, 10);
            _data0X201.UpdateData(new MatrixValDefinition(186, 10, DutyCycle));
            _data0X201.UpdateData(new MatrixValDefinition(207, 1, 1));
        }

        [Description("左座椅加热结束")]
        public void SeatHeatgCmdCushSecLeftOff()
        {
            _data0X201.UpdateData(new MatrixValDefinition(186, 10, 0));
            _data0X201.UpdateData(new MatrixValDefinition(207, 1, 1));
        }

        [Description("左座椅靠背加热开始")]
        public void SeatHeatgCmdBackSecLeftOn(string dutyCycle)
        {
            var DutyCycle = Convert.ToByte(dutyCycle, 10);
            _data0X201.UpdateData(new MatrixValDefinition(174, 10, DutyCycle));
            _data0X201.UpdateData(new MatrixValDefinition(1, 1, 1));
        }

        [Description("左座椅靠背加热结束")]
        public void SeatHeatgCmdBackSecLeftOff()
        {
            _data0X201.UpdateData(new MatrixValDefinition(174, 10, 0));
            _data0X201.UpdateData(new MatrixValDefinition(1, 1, 1));
        }

        [Description("右靠背通风开始")]
        public void SeatVentnCmdBackSecRightOn(string dutyCycle)
        {
            try
            {
                var DutyCycle = Convert.ToByte(dutyCycle, 10);
                //_data0X230.UpdateData(new MatrixValDefinition(26, 8, DutyCycle));
                _data0X230.UpdateData(new MatrixValDefinition(26, 10, DutyCycle));
                _data0X230.UpdateData(new MatrixValDefinition(45, 1, 1));
            }
            catch
            {

            }

        }

        [Description("右靠背通风结束")]
        public void SeatVentnCmdBackSecRightOff()
        {
            _data0X230.UpdateData(new MatrixValDefinition(26, 10, 0));
            _data0X230.UpdateData(new MatrixValDefinition(45, 1, 1));
        }

        [Description("右坐垫通风开始")]
        public void SeatVentnCmdCushSecRightOn(string dutyCycle)
        {
            var DutyCycle = Convert.ToByte(dutyCycle, 10);
            _data0X230.UpdateData(new MatrixValDefinition(32, 10, DutyCycle));
            _data0X230.UpdateData(new MatrixValDefinition(44, 1, 1));
        }

        [Description("右坐垫通风结束")]
        public void SeatVentnCmdCushSecRightOff()
        {
            _data0X230.UpdateData(new MatrixValDefinition(32, 10, 0));
            _data0X230.UpdateData(new MatrixValDefinition(44, 1, 1));
        }

        [Description("左靠背通风开始")]
        public void SeatVentnCmdBackSecLeftOn(string dutyCycle)
        {
            try
            {
                var DutyCycle = Convert.ToByte(dutyCycle, 10);
                //_data0X230.UpdateData(new MatrixValDefinition(26, 8, DutyCycle));
                _data0X201.UpdateData(new MatrixValDefinition(212, 10, DutyCycle));
                _data0X201.UpdateData(new MatrixValDefinition(245, 1, 1));
            }
            catch
            {

            }

        }

        [Description("左靠背通风结束")]
        public void SeatVentnCmdBackSecLeftOff()
        {
            _data0X201.UpdateData(new MatrixValDefinition(212, 10, 0));
            _data0X201.UpdateData(new MatrixValDefinition(245, 1, 1));
        }

        [Description("左坐垫通风开始")]
        public void SeatVentnCmdCushSecLeftOn(string dutyCycle)
        {
            var DutyCycle = Convert.ToByte(dutyCycle, 10);
            _data0X201.UpdateData(new MatrixValDefinition(224, 10, DutyCycle));
            _data0X201.UpdateData(new MatrixValDefinition(243, 1, 1));
        }

        [Description("左坐垫通风结束")]
        public void SeatVentnCmdCushSecLeftOff()
        {
            _data0X201.UpdateData(new MatrixValDefinition(224, 10, 0));
            _data0X201.UpdateData(new MatrixValDefinition(243, 1, 1));
        }
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
            SecRiLenMotCmdSwtHozlSts1_Off,

            /// <summary>
            /// 间隔10ms 电机组2电机4 电机动左
            /// </summary>
            [MatrixValDefinition(70, 1, 1)]
            [Description("0x120")]
            SecLeLenMotCmd_UB_On,

            /// <summary>
            /// 间隔10ms 电机组2电机4 电机停左
            /// </summary>
            [MatrixValDefinition(70, 1, 0)]
            [Description("0x120")]
            SecLeLenMotCmd_UB_Off,

            /// <summary>
            /// 间隔10ms 电机组2电机4 电机动左
            /// </summary>
            [MatrixValDefinition(57, 8, 0)]
            [Description("0x120")]
            SecLeLenMotCmdMotorSpeed_On,

            /// <summary>
            /// 间隔10ms 电机组2电机4 电机停左
            /// </summary>
            [MatrixValDefinition(57, 8, 0)]
            [Description("0x120")]
            SecLeLenMotCmdMotorSpeed_Off,

            /// <summary>
            /// 间隔10ms 
            /// </summary> 电机组2电机4 电机动左
            [MatrixValDefinition(71, 2, 1)]
            [Description("0x120")]
            SecLeLenMotCmdSwtHozlSts1_On,

            /// <summary>
            /// 间隔10ms 
            /// </summary> 电机组2电机4 电机动左
            [MatrixValDefinition(71, 2, 0)]
            [Description("0x120")]
            SecLeLenMotCmdSwtHozlSts1_Off,

            /// <summary>
            /// 间隔10ms 电机组1电机2 电机动
            /// </summary>
            [MatrixValDefinition(13, 1, 1)]
            [Description("0x120")]
            SecLeBackrestMotCmd_UB_On,

            /// <summary>
            /// 间隔10ms 电机组1电机2 电机停
            /// </summary>
            [MatrixValDefinition(13, 1, 0)]
            [Description("0x120")]
            SecLeBackrestMotCmd_UB_Off,

            /// <summary>
            /// 间隔10ms 电机组1电机2 电机动
            /// </summary> 
            [MatrixValDefinition(14, 2, 1)]
            [Description("0x120")]
            SecLeBackrestMotCmdSwtHozlSts1_On,

            /// <summary>
            /// 间隔10ms 电机组1电机2 电机动
            /// </summary> 
            [MatrixValDefinition(14, 2, 0)]
            [Description("0x120")]
            SecLeBackrestMotCmdSwtHozlSts1_Off,

            /// <summary>
            /// 间隔10ms 电机组1电机2 电机动
            /// </summary> 
            [MatrixValDefinition(0, 8, 0)]
            [Description("0x120")]
            SecLeBackrestMotCmdMotorSpeed_On,

            /// <summary>
            /// 间隔10ms 电机组1电机2 电机动
            /// </summary> 
            [MatrixValDefinition(0, 8, 0)]
            [Description("0x120")]
            SecLeBackrestMotCmdMotorSpeed_Off,

            /// <summary>
            /// 间隔10ms 电机组3电机6 电机动
            /// </summary>
            [MatrixValDefinition(44, 1, 1)]
            [Description("0x120")]
            SecLeLegRstHozlMotCmd_UB_On,

            /// <summary>
            /// 间隔10ms 电机组3电机6 电机停
            /// </summary>
            [MatrixValDefinition(44, 1, 0)]
            [Description("0x120")]
            SecLeLegRstHozlMotCmd_UB_Off,

            /// <summary>
            /// 间隔10ms 电机组3电机6 电机动
            /// </summary> 
            [MatrixValDefinition(45, 2, 1)]
            [Description("0x120")]
            SecLeLegRstHozlMotCmdSwtHozlSts1_On,

            /// <summary>
            /// 间隔10ms 电机组3电机6 电机动
            /// </summary> 
            [MatrixValDefinition(45, 2, 0)]
            [Description("0x120")]
            SecLeLegRstHozlMotCmdSwtHozlSts1_Off,

            /// <summary>
            /// 间隔10ms 电机组3电机6 电机动
            /// </summary> 
            [MatrixValDefinition(47, 8, 100)]
            [Description("0x120")]
            SecLeLegRstHozlMotCmdMotorSpeed_On,

            /// <summary>
            /// 间隔10ms 电机组3电机6 电机动
            /// </summary> 
            [MatrixValDefinition(47, 8, 0)]
            [Description("0x120")]
            SecLeLegRstHozlMotCmdMotorSpeed_Off,

            /// <summary>
            /// 间隔10ms 电机组4电机8 电机动
            /// </summary>
            [MatrixValDefinition(49, 1, 1)]
            [Description("0x120")]
            SecLeLegRstVertMotCmd_UB_On,

            /// <summary>
            /// 间隔10ms 电机组4电机8 电机停
            /// </summary>
            [MatrixValDefinition(49, 1, 0)]
            [Description("0x120")]
            SecLeLegRstVertMotCmd_UB_Off,

            /// <summary>
            /// 间隔10ms 电机组4电机8 电机动
            /// </summary> 
            [MatrixValDefinition(50, 2, 1)]
            [Description("0x120")]
            SecLeLegRstVertMotCmdSwtVertSts1_On,

            /// <summary>
            /// 间隔10ms 电机组4电机8 电机动
            /// </summary> 
            [MatrixValDefinition(50, 2, 0)]
            [Description("0x120")]
            SecLeLegRstVertMotCmdSwtVertSts1_Off,

            /// <summary>
            /// 间隔10ms 电机组4电机8 电机动
            /// </summary> 
            [MatrixValDefinition(52, 8, 0)]
            [Description("0x120")]
            SecLeLegRstVertMotCmdMotorSpeed_On,

            /// <summary>
            /// 间隔10ms 电机组4电机8 电机动
            /// </summary> 
            [MatrixValDefinition(52, 8, 0)]
            [Description("0x120")]
            SecLeLegRstVertMotCmdMotorSpeed_Off,
        }

        #region 读取版本号  
        [Description("R,ECU序列号读取")]
        public string ECU序列号读取;
        [Description("R,读取DU零件号")]
        public string 读取DU零件号;
        [Description("R,读取HWSD零件号")]
        public string 读取HWSD零件号;
        [Description("R,读取SXBL零件号")]
        public string 读取SXBL零件号;
        [Description("R,读取SXDISXBL零件号")]
        public string 读取SXDISXBL零件号;
        [Description("R,读取SWLM和SWP1零件号")]
        public string 读取SWLM和SWP1零件号;
        [Description("R,读取SXDISWLM零件号")]
        public string 读取SXDISWLM零件号;
        [Description("R,读取信耀内部版本号")]
        public string 读取信耀内部版本号;
        public uint CanDiagnosisRequestPhyCanId = 0x781;
        public uint CanDiagnosisResponseCanId = 0x681;

        [Description("ECU写入")]
        public void ECUWrite()
        {
            lock (CreateTimeStampLock)
            {
                TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                var sd = Convert.ToInt32(ts.TotalSeconds);
                var bytes = new byte[4];
                bytes[0] = (byte)(sd & 0xFF);
                bytes[1] = (byte)((sd >> 4) & 0xFF);
                bytes[2] = (byte)((sd >> 8) & 0xFF);
                bytes[3] = (byte)((sd >> 16) & 0xFF);
                var sendByte = new List<byte>() { 0x2E, 0XF1, 0X8C };
                sendByte.AddRange(bytes.ToList());
                var re = SendCan(new uint[] { CanDiagnosisRequestPhyCanId }, CanDiagnosisResponseCanId, sendByte).GetStringByAsciiBytes(false).ToString();
                Thread.Sleep(1500);
            }
        }

        [Description("版本读取")]
        public void ReadApp()
        {
            ECU序列号读取 = "";
            读取DU零件号 = "";
            读取HWSD零件号 = "";
            读取SXBL零件号 = "";
            读取SXDISXBL零件号 = "";
            读取SWLM和SWP1零件号 = "";
            读取SXDISWLM零件号 = "";

            ECU序列号读取 = SendCan(new uint[] { CanDiagnosisRequestPhyCanId }, CanDiagnosisResponseCanId, new byte[] { 0X03, 0x22, 0xF1, 0x8C }.ToList()).GetStringByAsciiBytes(false).ToString();
            读取DU零件号 = SendCan(new uint[] { CanDiagnosisRequestPhyCanId }, CanDiagnosisResponseCanId, new byte[] { 0X03, 0x22, 0xF1, 0xAB }.ToList()).GetStringByAsciiBytes(false).ToString();
            读取HWSD零件号 = SendCan(new uint[] { CanDiagnosisRequestPhyCanId }, CanDiagnosisResponseCanId, new byte[] { 0X03, 0x22, 0xF1, 0xAA }.ToList()).GetStringByAsciiBytes(false).ToString();
            读取SXBL零件号 = SendCan(new uint[] { CanDiagnosisRequestPhyCanId }, CanDiagnosisResponseCanId, new byte[] { 0X03, 0x22, 0xF1, 0xA5 }.ToList()).GetStringByAsciiBytes(false).ToString();
            读取SXDISXBL零件号 = SendCan(new uint[] { CanDiagnosisRequestPhyCanId }, CanDiagnosisResponseCanId, new byte[] { 0X03, 0x22, 0xF1, 0xA1 }.ToList()).GetStringByAsciiBytes(false).ToString();
            读取SWLM和SWP1零件号 = SendCan(new uint[] { CanDiagnosisRequestPhyCanId }, CanDiagnosisResponseCanId, new byte[] { 0X03, 0x22, 0xF1, 0xAE }.ToList()).GetStringByAsciiBytes(false).ToString();
            读取SXDISWLM零件号 = SendCan(new uint[] { CanDiagnosisRequestPhyCanId }, CanDiagnosisResponseCanId, new byte[] { 0X03, 0x22, 0xF1, 0xA0 }.ToList()).GetStringByAsciiBytes(false).ToString();

            var internalSoftVar = SendCan(new uint[] { CanDiagnosisRequestPhyCanId }, CanDiagnosisResponseCanId, new byte[] { 0X03, 0x22, 0xF1, 0xFE }.ToList());
            if (internalSoftVar.Length > 10)
                读取信耀内部版本号 = ValueHelper.GetStringByAsciiBytes(new byte[] { internalSoftVar[5], internalSoftVar[6], internalSoftVar[7], internalSoftVar[8], internalSoftVar[9], internalSoftVar[10] }, false);
        }

        [Description("读取NTC")]
        public void ReadNTC()
        {
            NTC1Temperature = 0;
            NTC2Temperature = 0;
            var bytes = SendCan(new uint[] { CanDiagnosisRequestPhyCanId }, CanDiagnosisResponseCanId, new byte[] { 0X03, 0x22, 0xF1, 0xF2 }.ToList());
            if (bytes.Length < 8)
                return;
            var temp = new byte[bytes.Length - 4];
            Array.Copy(bytes, 4, temp, 0, bytes.Length - 4);
            NTC1Temperature = GetValueByBytes(new byte[] { temp[0], temp[1] });
            NTC2Temperature = GetValueByBytes(new byte[] { temp[2], temp[3] });
        }

        [Description("R,身份识别功能")]
        public string LorR;

        public void ReadLorR()
        {
            LorR = string.Empty;
            LorR = ValueHelper.GetHextStr(SendCan(new uint[] { CanDiagnosisRequestPhyCanId }, CanDiagnosisResponseCanId, new byte[] { 0X03, 0x22, 0x40, 0xB8 }.ToList()));
        }

        public void Test()
        {
            var bs = SendCan(new uint[] { CanDiagnosisRequestPhyCanId }, CanDiagnosisResponseCanId, new byte[] { 0X03, 0x22, 0xF1, 0x8C }.ToList());
        }

        private int GetValueByBytes(byte[] bytes)
        {
            var str = string.Empty;
            str = bytes.Aggregate(string.Empty, (values, t) => values + ValueHelper.GetHextStr(t));
            return Convert.ToInt32(str, 16);
        }

        [Description("读取电机HAll")]
        public void ReadMotsHall()
        {
            Mot2Hall = 0;
            Mot4Hall = 0;
            Mot6Hall = 0;
            Mot8Hall = 0;
            Mot2P = 0;
            Mot4P = 0;
            Mot6P = 0;
            Mot8P = 0;
            var bytes = SendCan(new uint[] { CanDiagnosisRequestPhyCanId }, CanDiagnosisResponseCanId, new byte[] { 0X03, 0x22, 0x40, 0xA3 }.ToList());
            if (bytes.Length < 40)
                return;
            var temp = new byte[bytes.Length - 5];
            Array.Copy(bytes, 5, temp, 0, bytes.Length - 5);
            Mot2Hall = GetValueByBytes(new byte[] { temp[4], temp[5] });
            Mot4Hall = GetValueByBytes(new byte[] { temp[16], temp[17] });
            Mot6Hall = GetValueByBytes(new byte[] { temp[24], temp[25] });
            Mot8Hall = GetValueByBytes(new byte[] { temp[40], temp[41] });
            Mot2P = GetValueByBytes(new byte[] { temp[6], temp[7] });
            Mot4P = GetValueByBytes(new byte[] { temp[18], temp[19] });
            Mot6P = GetValueByBytes(new byte[] { temp[26], temp[27] });
            Mot8P = GetValueByBytes(new byte[] { temp[42], temp[43] });
        }

        [Description("读取电机电机电流值")]
        public void ReadMotsCurrent()
        {
            for (int i = 0; i < 3; i++)
            {
                Mot2Current = 0;
                Mot4Current = 0;
                Mot6Current = 0;
                Mot8Current = 0;
                var bytes = SendCan(new uint[] { CanDiagnosisRequestPhyCanId }, CanDiagnosisResponseCanId, new byte[] { 0X03, 0x22, 0xD3, 0x33 }.ToList());
                if (bytes.Length < 24)
                    return;
                var temp = new byte[bytes.Length - 5];
                Array.Copy(bytes, 5, temp, 0, bytes.Length - 5);
                Mot2Current = GetValueByBytes(new byte[] { temp[0], temp[1] });
                Mot4Current = GetValueByBytes(new byte[] { temp[2], temp[3] });
                Mot6Current = GetValueByBytes(new byte[] { temp[4], temp[5] });
                Mot8Current = GetValueByBytes(new byte[] { temp[6], temp[7] });
                FanCurrent1 = GetValueByBytes(new byte[] { temp[8], temp[9] });
                FanCurrent2 = GetValueByBytes(new byte[] { temp[14], temp[15] });
                SecRIHotCurrent = GetValueByBytes(new byte[] { temp[12], temp[13] });
                BackrestSeatHotCurrent = GetValueByBytes(new byte[] { temp[10], temp[11] });
                if (!(Mot2Current < 200 || Mot4Current < 200 || Mot8Current < 200))
                { break; }
            }
        }

        [Description("读取电机开关状态")]
        public void ReadObs()
        {
            OBSSwitchState = "NG";
            var bytes = SendCan(new uint[] { CanDiagnosisRequestPhyCanId }, CanDiagnosisResponseCanId, new byte[] { 0X03, 0x22, 0x40, 0xB8 }.ToList());
            if (bytes.Length < 8)
                return;
            var temp = new byte[bytes.Length - 4];
            Array.Copy(bytes, 4, temp, 0, bytes.Length - 4);
            switch (temp[0])
            {
                case 0x40:
                    OBSSwitchState = "NotPrsnt";
                    break;
                case 0x80:
                    OBSSwitchState = "Prsnt";
                    break;
                case 0xC0:
                    OBSSwitchState = "error";
                    break;
            }
        }

        [Description("读取KL30")]
        public void ReadKL30()
        {
            KL30_1Voltage = 0;
            KL30_2Voltage = 0;
            KL30_3Voltage = 0;

            var bytes = SendCan(new uint[] { CanDiagnosisRequestPhyCanId }, CanDiagnosisResponseCanId, new byte[] { 0X03, 0x22, 0xD1, 0x10 }.ToList());
            if (bytes.Length < 8)
                return;

            KL30_1Voltage = Convert.ToInt32(bytes[4]);
            KL30_2Voltage = Convert.ToInt32(bytes[5]);
            KL30_3Voltage = Convert.ToInt32(bytes[6]);
        }

        private byte[] SendCan(uint[] requestCanId, uint responseCanId, List<byte> sendbyte, int timeout = 500)
        {
            lock (_sendLock)
            {
                if (Can == null)
                    return new byte[0];
                try
                {
                    for (int ii = 0; ii < 3; ii++)
                    {
                        _ftpBuffer.Clear();
                        _responseCanId = CanDiagnosisResponseCanId;

                        var tempcount = sendbyte.Count;
                        for (var i = 0; i < 8 - tempcount; i++)
                            sendbyte.Add(0x00);
                        List<CanBus.CanDataPackage> canDataPackages = new List<CanBus.CanDataPackage>();
                        foreach (var item in requestCanId)
                        {
                            canDataPackages.Add(new CanBus.CanDataPackage(item, CanBus.CanProtocol.SpeedCanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, sendbyte.ToArray()));
                        }

                        _isInFtp = true;
                        Can.SendCanDatas(canDataPackages.ToArray());
                        _recvWaitHandle.WaitOne(timeout);
                        _isInFtp = false;
                        if (_ftpBuffer.Where(t => t.CanID == responseCanId).Any())
                            return _ftpBuffer.Where(t => t.CanID == responseCanId).Last().Candata;
                    }
                    return _ftpBuffer.Where(t => t.CanID == responseCanId).Last().Candata;
                }
                catch
                {
                    return new byte[0];
                }
            }

        }
        #endregion
    }
}
