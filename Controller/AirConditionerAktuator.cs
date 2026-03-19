using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Controller
{
    [Description("LIN-Product,空调出风口执行器")]
    public sealed class AirConditionerAktuator : ControllerBase
    {
        public LinBus Lin;

        public LinBus LinOut;


        public byte SendId = 0x17;

        public byte RecId1 = 0x18;
        public byte RecId2 = 0x10;

        private readonly Thread _keepNetworkThread;
        private readonly object _lockSend = new object();
        private bool _isSleep = true;
        //private volatile bool _isAutoScaning;


        private readonly LinCommunicationMatrix.IntelMatrix _intelMatrix =
            new LinCommunicationMatrix.IntelMatrix(0x17, 8);

        private readonly Dictionary<string, MatrixValDefinition> _matrixValDefinitions =
        new Dictionary<string, MatrixValDefinition>();

        #region Fields

        [Description("R,Lin0x18读取结果")]
        public string ReadStr1 = string.Empty;

        [Description("R,Lin0x10读取结果")]
        public string ReadStr2 = string.Empty;

        [Description("R,读取结果3")]
        public string ReadStr3 = string.Empty;

        //[Description("R,LinOut0x10读取结果")]
        //public string ReadStr4 = string.Empty;

        #region 0x18

        [Description("R,Responseerror")]
        public string _Responseerror;                                 //  从0开始  1bit
        [Description("R,Over_temperature")]
        public string _Over_temperature;                           //  从2开始  2bit
        [Description("R,Electr_defect")]
        public string _Electr_defect;                                 //  从4开始  2bit
        [Description("R,Supply_voltage")]
        public string _Supply_voltage;                              //  从6开始  2bit
        [Description("R,Emergency_operation_occurred")]
        public string _Emergency_operation_occurred;   //  从8开始  2bit
        [Description("R,Release_blockage_detection")]
        public string _Release_blockage_detection;       //  从10开始  2bit
        [Description("R,Blockage_occurred")]
        public string _Blockage_occurred;                     //  从12开始  2bit
        [Description("R,Reset")]
        public string _Reset;                                           //  从14开始  2bit
        [Description("R,Coil_hold_current_flow")]
        public string _Coil_hold_current_flow;              //  从16开始  2bit
        [Description("R,Position_specification_status")]
        public string _Position_specification_status;    //  从18开始  2bit
        [Description("R,Speed_status")]
        public string _Speed_status;                             //  从20开始  4bit
        [Description("R,Actual_position_actuator")]
        public string _Actual_position_actuator;         //  从24开始  16bit
        [Description("R,Direction_of_travel")]
        public string _Direction_of_travel;                    //  从40开始  2bit
        [Description("R,Holding_torque")]
        public string _Holding_torque;                        //  从42开始  2bit
        [Description("R,Special_functions")]
        public string _Special_functions;                    //  从44开始  2bit
        [Description("R,Address_actuator")]
        public string _Address_actuator;                    //  从48开始  8bit
        [Description("R,Emergency_operation_release")]
        public string _Emergency_operation_release; //  从56开始  2bit
        [Description("R,Emergency_operation_position")]
        public string _Emergency_operation_position; //  从58开始  2bit
        [Description("R,Direction_of_rotation")]
        public string _Direction_of_rotation;                 //  从60开始  2bit
        [Description("R,Stop_mode")]
        public string _Stop_mode;                                //  从62开始  2bit

        #endregion

        #region 0x10
        [Description("R,母线电压(mV)")]
        public string _SMP_Volt;                                //  2
        [Description("R,运行母线电流(mA)")]
        public string _SMP_IS_RUN;                                //  2
        [Description("R,堵转母线电流(mA)")]
        public string _SMP_IS_STALL;                                //  2
        [Description("R,运行方向")]
        public string _Sts_Dir;                                //  1
        [Description("R,芯片温度")]
        public string _Sts_TSD;                                //  1

        [Description("R,峰值电流")]
        public string _MaxCurr = "0";

        #endregion

        #region 0x11
        /*
 FaultNoSource       = 0,       ///< 无故障
FaultHardOVCurrent     = 1,       ///< 硬件过流
FaultSoftOVCurrent     = 2,        ///< 软件过流
FaultHardOverVoltage    = 3,       ///< 硬件过压
FaultSoftOverVoltage    = 4,       ///< 软件过压
FaultHardUnderVoltage   = 5,       ///< 硬件欠压
FaultSoftUnderVoltage   = 6,          ///< 软件欠压
FaultPhaseLost       = 7,       ///< 缺相
FaultStall         = 8,       ///< 堵转
FaultSoftOTErr       = 9,       ///< 软件过温
FaultHardOTErr       = 9,        ///< 硬件过温
FaultUartLost       = 10,       ///< 通信丢失
         */


        /*
   mcReady     = 0,    ///< 准备状态,该状态电机空闲，等待控制命令 
mcInit      = 1,    ///< 初始化,该状态进行启动前的变量初始化
mcCharge    = 2,    ///< 预充电,电机启动前给自举电容充电，一般用于高压驱动，低压驱动一般不需要
mcTailWind  = 3,    ///< 顺逆风检测,该状态下电机进行顺逆风检测 
mcPosiCheck = 4,    ///< 初始位置检测
mcAlign     = 5,    ///< 预定位
mcStart     = 6,    ///< 启动，用于配置启动代码
mcRun       = 7,    ///< 运行，
mcStop      = 8,    ///< 停止
mcFault     = 9,    ///< 故障状态
mcBrake     = 10,   ///< 刹车
         */
        [Description("R,实际转速(rpm)")]
        public string _Actual_Speed;                                //  2
        [Description("R,故障状态")]
        public string _Sts_Fault;                                //  1
        [Description("R,电机控制状态机运行状态")]
        public string _Sts_RunIndex;                                //  1


        #endregion




        [Description("R,供电电压")]
        public string SupplyVolt = "0";
        //[Description("R,休眠电流")]
        //public string SleepCurr = "0";
        [Description("R,工作电流")]
        public string WorkCurr = "0";


        #endregion

        public bool IsRead = true;
        public bool LinOutIsRead = true;

        public AirConditionerAktuator(string name)
            : base(name)
        {

            _matrixValDefinitions.Add(Address_actuator, new MatrixValDefinition(0, 8, 0x7F));
            _matrixValDefinitions.Add(Store_programmingdata, new MatrixValDefinition(8, 2, 0));
            _matrixValDefinitions.Add(Release_blockage_detection, new MatrixValDefinition(10, 2, 0));
            _matrixValDefinitions.Add(Clear_event_flags, new MatrixValDefinition(12, 4, 0x0E));

            _matrixValDefinitions.Add(Coil_hold_current_flow, new MatrixValDefinition(16, 2, 0));
            _matrixValDefinitions.Add(Select_position_specification, new MatrixValDefinition(18, 2, 0));
            _matrixValDefinitions.Add(Speed_specification, new MatrixValDefinition(20, 4, 1));

            _matrixValDefinitions.Add(Target_position_actuator1, new MatrixValDefinition(24, 8, 0));
            _matrixValDefinitions.Add(Target_position_actuator2, new MatrixValDefinition(32, 8, 0));

            _matrixValDefinitions.Add(Start_position_actuator1, new MatrixValDefinition(40, 8, 0));
            _matrixValDefinitions.Add(Start_position_actuator2, new MatrixValDefinition(48, 8, 0));

            _matrixValDefinitions.Add(Energency_operation_Release, new MatrixValDefinition(56, 2, 0));
            _matrixValDefinitions.Add(Energency_operation_position, new MatrixValDefinition(58, 2, 0));
            _matrixValDefinitions.Add(Direction_of_rotation, new MatrixValDefinition(60, 2, 1));
            _matrixValDefinitions.Add(Stop_mode, new MatrixValDefinition(62, 2, 1));

            if (_keepNetworkThread != null)
            {
                _keepNetworkThread.Abort();
                _keepNetworkThread.Join();
            }

            _keepNetworkThread = new Thread(KeepNetwork) { IsBackground = true };
            _keepNetworkThread.Start();

        }

        ~AirConditionerAktuator()
        {
            Dispose();
        }


        private int linNgcount = 0;
        private int linOutNgcount = 0;

        private int sendcount = 0;
        private void KeepNetwork()
        {
            while (_keepNetworkThread.IsAlive)
            {
                if (!_keepNetworkThread.IsAlive)
                    break;

                Thread.Sleep(10);

                if (Lin == null)
                    continue;

                if (!_isSleep)
                {
                    foreach (var key in _matrixValDefinitions.Keys)
                        _intelMatrix.UpdateData(_matrixValDefinitions[key]);

                    lock (_lockSend)
                    {
                        bool b1 = Lin.SendMasterLin(SendId, _intelMatrix.MatrixData);
                    }
                    sendcount++;

                    if (sendcount >= 10)
                    {
                        sendcount = 0;
                        //lock (_lockSend)
                        //{

                        //    byte[] echoBytes18;
                        //    ReadStr1 = Lin.SendSlaveLin(RecId1, out echoBytes18)
                        //        ? ValueHelper.GetHextStr(echoBytes18)
                        //        : string.Empty;

                        //    byte[] echoBytes0X10;
                        //    ReadStr2 = Lin.SendSlaveLin(RecId2, out echoBytes0X10)
                        //        ? ValueHelper.GetHextStr(echoBytes0X10)
                        //        : string.Empty;

                        //    //byte[] echoBytes0X11;
                        //    //ReadStr3 = Lin.SendSlaveLin(0x11, out echoBytes0X11)
                        //    //    ? ValueHelper.GetHextStr(echoBytes0X11)
                        //    //    : string.Empty;


                        //    var str = "00 00 00 00 00 00 00 00";
                        //    //LinOut.SetSlaveId(RecId2);

                        //    //  byte[] linoutread;
                        //    //  ReadStr3 = LinOut.SendSlaveLin(RecId2, out linoutread)
                        //    //? ValueHelper.GetHextStr(linoutread)
                        //    //: string.Empty;

                        //    if (LinOut.LinRecvDataPackages.Count > 1 && !LinOut.LinRecvDataPackages.All(a => a.LinData.Equals(new byte[] { 00, 00, 00, 00, 00, 00, 00, 00 })))

                        //    {
                        //        linOutNgcount = 0;
                        //        LinOutIsRead = true;
                        //        LinOut.LinRecvDataPackages.Clear();
                        //    }
                        //    else
                        //    {
                        //        linOutNgcount++;
                        //        if (linOutNgcount >= 50)
                        //        {
                        //            LinOutIsRead = false;
                        //            linOutNgcount = 0;
                        //        }
                        //    }



                        //    if (ReadStr1 == "" || ReadStr1.Equals(str) || ReadStr2 == "" || ReadStr2.Equals(str))
                        //    {
                        //        linNgcount++;
                        //        if (linNgcount >= 5)
                        //        {
                        //            IsRead = false;
                        //            linNgcount = 0;
                        //        }

                        //        continue;
                        //    }

                        //    try
                        //    {
                        //        ReadStateMessage();

                        //        if (int.Parse(_SMP_IS_RUN) > int.Parse(_MaxCurr))
                        //        {
                        //            _MaxCurr = _SMP_IS_RUN;
                        //        }

                        //        linNgcount = 0;
                        //        IsRead = true;
                        //    }
                        //    catch (Exception) { }


                        //}
                    }


                }



            }
        }

        /// <summary>
        /// 0，大众，1奥迪
        /// </summary>
        /// <param name="idx"></param>
        [Description("切换型号")]
        public void ChangeType(int idx)
        {

            if (idx == 0)
            {
                SendId = 0x17;
                RecId1 = 0x18;
                RecId2 = 0x10;

            }
            else if (idx == 1)
            {
                SendId = 0x01;
                RecId1 = 0x21;
                RecId2 = 0x10;
            }
            //else if (idx == 2)
            //{
            //    SendId = 0x01;
            //    RecId1 = 0x7F;
            //    RecId2 = 0x21;
            //}   

        }

        #region 电机控制

        [Description("ECU休眠")]
        public void ModuleSleep()
        {
            try
            {
                _isSleep = true;
                IsRead = true;
                LinOutIsRead = true;
                Thread.Sleep(200);
                lock (_lockSend)
                {
                    bool b1 = Lin.SendMasterLin(0x3C, new byte[] { 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF });
                }
            }
            catch (Exception) { }


        }

        [Description("ECU唤醒")]
        public void ModuleAwake()
        {
            _isSleep = false;
        }

        [Description("停止读写")]
        public void Stop()
        {
            _isSleep = true;
        }

        //[Description("开启自动读取状态")]
        //public void StartAutoScanStatus()
        //{
        //    _isAutoScaning = true;
        //}

        //[Description("停止自动读取状态")]
        //public void StopAutoScanStatus()
        //{
        //    _isAutoScaning = false;

        //}

        [Description("电机停止")]
        public void MotorStop()
        {
            _matrixValDefinitions[Stop_mode].Value = 0x01;
        }

        [Description("电机启动")]
        public void MotorStart()
        {
            _matrixValDefinitions[Stop_mode].Value = 0x00;
        }

        [Description("电机档位选择")]
        public void MotorSpeed(byte value)
        {
            _matrixValDefinitions[Speed_specification].Value = value;
        }

        [Description("正转")]
        public void MotorForward(byte speed)
        {
            MotorSpeed(speed);
            _matrixValDefinitions[Target_position_actuator1].Value = 0x00;
            _matrixValDefinitions[Target_position_actuator2].Value = 0x00;

        }

        [Description("反转")]
        public void MotorReverse(byte speed)
        {
            MotorSpeed(speed);
            _matrixValDefinitions[Target_position_actuator1].Value = 0xFF;
            _matrixValDefinitions[Target_position_actuator2].Value = 0xFE;
        }

        [Description("报警清除开启")]
        public void ClearEventStart()
        {
            _matrixValDefinitions[Clear_event_flags].Value = 0xE;
        }

        [Description("报警清除关闭")]
        public void ClearEventStop()
        {
            _matrixValDefinitions[Clear_event_flags].Value = 0x00;
        }


        [Description("峰值电流清空")]
        public void ClearMaxCurr()
        {
            _MaxCurr = "0";
        }

        #endregion


        /// <summary>
        /// 读取状态信息
        /// </summary>
        [Description("读取状态信息")]
        public void ReadStateMessage()
        {
            lock (_lockSend)
            {
                {
                    var recvData = new List<byte>();
                    var strData = ReadStr1;

                    if (!string.IsNullOrEmpty(strData))
                    {
                        var sp = strData.Split(' ');
                        recvData.AddRange(sp.Select(t => Convert.ToByte(t, 16)));
                        _Responseerror = (GetIntelValue(recvData.ToArray(), 0, 1)).ToString(CultureInfo.InvariantCulture);
                        _Over_temperature = (GetIntelValue(recvData.ToArray(), 2, 2)).ToString(CultureInfo.InvariantCulture);
                        _Electr_defect = (GetIntelValue(recvData.ToArray(), 4, 2)).ToString(CultureInfo.InvariantCulture);
                        _Supply_voltage = (GetIntelValue(recvData.ToArray(), 6, 2)).ToString(CultureInfo.InvariantCulture);
                        _Emergency_operation_occurred = (GetIntelValue(recvData.ToArray(), 8, 2)).ToString(CultureInfo.InvariantCulture);
                        _Release_blockage_detection = (GetIntelValue(recvData.ToArray(), 10, 2)).ToString(CultureInfo.InvariantCulture);
                        _Blockage_occurred = (GetIntelValue(recvData.ToArray(), 12, 2)).ToString(CultureInfo.InvariantCulture);

                        _Reset = (GetIntelValue(recvData.ToArray(), 14, 2)).ToString(CultureInfo.InvariantCulture);
                        _Coil_hold_current_flow = (GetIntelValue(recvData.ToArray(), 16, 2)).ToString(CultureInfo.InvariantCulture);
                        _Position_specification_status = (GetIntelValue(recvData.ToArray(), 18, 2)).ToString(CultureInfo.InvariantCulture);
                        _Speed_status = (GetIntelValue(recvData.ToArray(), 20, 4)).ToString(CultureInfo.InvariantCulture);
                        _Actual_position_actuator = (GetIntelValue(recvData.ToArray(), 24, 16)).ToString(CultureInfo.InvariantCulture);
                        _Direction_of_travel = (GetIntelValue(recvData.ToArray(), 40, 2)).ToString(CultureInfo.InvariantCulture);

                        _Holding_torque = (GetIntelValue(recvData.ToArray(), 42, 2)).ToString(CultureInfo.InvariantCulture);
                        _Special_functions = (GetIntelValue(recvData.ToArray(), 44, 2)).ToString(CultureInfo.InvariantCulture);
                        _Address_actuator = (GetIntelValue(recvData.ToArray(), 48, 8)).ToString(CultureInfo.InvariantCulture);
                        _Emergency_operation_release = (GetIntelValue(recvData.ToArray(), 56, 2)).ToString(CultureInfo.InvariantCulture);
                        _Emergency_operation_position = (GetIntelValue(recvData.ToArray(), 58, 2)).ToString(CultureInfo.InvariantCulture);
                        _Direction_of_rotation = (GetIntelValue(recvData.ToArray(), 60, 2)).ToString(CultureInfo.InvariantCulture);
                        _Stop_mode = (GetIntelValue(recvData.ToArray(), 62, 2)).ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {

                    }
                }

                {
                    var recvData = new List<byte>();
                    var strData = ReadStr2;

                    if (!string.IsNullOrEmpty(strData))
                    {
                        var sp = strData.Split(' ');
                        recvData.AddRange(sp.Select(t => Convert.ToByte(t, 16)));
                        _SMP_Volt = (GetIntelValue(recvData.ToArray(), 0, 16) / 1000.0).ToString(CultureInfo.InvariantCulture);

                        //_SMP_IS_RUN = (GetIntelValue(recvData.ToArray(), 16, 32)).ToString(CultureInfo.InvariantCulture);
                        //_SMP_IS_STALL = (GetIntelValue(recvData.ToArray(), 32, 48)).ToString(CultureInfo.InvariantCulture);


                        var data = new List<byte>();
                        data.Add(recvData[3]);
                        data.Add(recvData[2]);
                        data.Add(recvData[5]);
                        data.Add(recvData[4]);

                        //_SMP_IS_RUN = Math.Abs(BitConverter.ToInt16(data.ToArray(), 0)).ToString();
                        //_SMP_IS_STALL = Math.Abs(BitConverter.ToInt16(data.ToArray(), 2)).ToString();

                        _SMP_IS_RUN = Math.Abs(BitConverter.ToInt16(recvData.ToArray(), 2)).ToString();
                        _SMP_IS_STALL = Math.Abs(BitConverter.ToInt16(recvData.ToArray(), 4)).ToString();

                        _Sts_Dir = (GetIntelValue(recvData.ToArray(), 48, 8)).ToString(CultureInfo.InvariantCulture);

                        _Sts_TSD = (GetIntelValue(recvData.ToArray(), 56, 8) - 40).ToString();

                    }
                    else
                    {

                    }

                }

                {
                    var recvData = new List<byte>();
                    var strData = ReadStr3;

                    if (!string.IsNullOrEmpty(strData))
                    {
                        var sp = strData.Split(' ');
                        recvData.AddRange(sp.Select(t => Convert.ToByte(t, 16)));
                        _Actual_Speed = (GetIntelValue(recvData.ToArray(), 0, 16)).ToString(CultureInfo.InvariantCulture);
                        _Sts_Fault = (GetIntelValue(recvData.ToArray(), 16, 8)).ToString(CultureInfo.InvariantCulture);
                        _Sts_RunIndex = (GetIntelValue(recvData.ToArray(), 24, 8)).ToString(CultureInfo.InvariantCulture);


                    }
                    else
                    {

                    }


                }
            }
        }

        private static int GetIntelValue(byte[] data, int startBit, int bitLen)
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

        private const string Address_actuator = "Address_actuator";
        private const string Store_programmingdata = "Store_programmingdata";
        private const string Release_blockage_detection = "Release_blockage_detection";
        private const string Clear_event_flags = "Clear_event_flags";
        private const string Coil_hold_current_flow = "Coil_hold_current_flow";
        private const string Select_position_specification = "Select_position_specification";
        private const string Speed_specification = "Speed_specification";
        private const string Target_position_actuator1 = "Target_position_actuator1";
        private const string Target_position_actuator2 = "Target_position_actuator2";

        private const string Start_position_actuator1 = "Start_position_actuator1";
        private const string Start_position_actuator2 = "Start_position_actuator2";

        private const string Energency_operation_Release = "Energency_operation_Release";
        private const string Energency_operation_position = "Energency_operation_position";
        private const string Direction_of_rotation = "Direction_of_rotation";

        private const string Stop_mode = "Stop_mode";
    }
}
