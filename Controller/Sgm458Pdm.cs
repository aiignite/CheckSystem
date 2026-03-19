using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using CommonUtility;
using CommonUtility.BusLoader;

namespace Controller
{
    [Description("CAN-Product,SGM458PDM")]
    public sealed class Sgm458Pdm : ControllerBase
    {
        public CanBus Can;
        public LinBus Lin10417;

        [Description("R/W,FLPwrSwngDrCtrlReq")]
        public bool FlPwrSwngDrCtrlReq;

        public Sgm458Pdm(string name)
            : base(name)
        {
            if (_keepNetworkThread != null)
            {
                _keepNetworkThread.Abort();
                _keepNetworkThread.Join();
            }

            _keepNetworkThread = new Thread(KeepNetwork) { IsBackground = true };
            _keepNetworkThread.Start();

            if (_keepExtendedSessionThread != null)
            {
                _keepExtendedSessionThread.Abort();
                _keepExtendedSessionThread.Join();
            }

            _keepExtendedSessionThread = new Thread(KeepExtendedSession) { IsBackground = true };
            _keepExtendedSessionThread.Start();

            CanBus.PushCanMsg += CanBus_PushCanMsg;
        }

        ~Sgm458Pdm()
        {
            Dispose();
        }

        private readonly Thread _keepExtendedSessionThread;
        private readonly Thread _keepNetworkThread;
        private readonly object _lockSend = new object();
        private bool _isInExtendedSession;
        private bool _isSleep = true;
        private const uint CanDiagnosisRequestPhyCanId = 0x14DAA4F1;
        //private const uint CanDiagnosisRequestFunCanId = 0x7DF;
        private const uint CanDiagnosisResponseCanId = 0x14DAF1A4;
        private readonly EventWaitHandle _readCanMsgWaitHandle = new AutoResetEvent(false);
        private bool IsReadCanMsg { get; set; }

        private void KeepNetwork()
        {
            while (_keepNetworkThread.IsAlive)
            {
                if (!_keepNetworkThread.IsAlive)
                    break;

                Thread.Sleep(100);

                //if (Can == null)
                //    continue;

                //if (_isSleep)
                //    continue;
                //lock (_lockSend)
                //{
                //    Can.SendStandardCanData(
                //       0x621, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                //}

                if (Lin10417 == null)
                    continue;

                if (_isSleep)
                    continue;
                //Lin10417.SendMasterLin(0x00, new byte[8]);
                lock (_lockSend)
                {
                    Thread.Sleep(250);
                    Lin10417.SendMasterLin(0x00, new byte[6]);

                    //var msg = new LinCommunicationMatrix.IntelMatrix(0x01, 2);
                    //if (FlPwrSwngDrCtrlReq)
                    //{
                    //    msg.UpdateData(new MatrixValDefinition(0, 3, 1));
                    //}
                    //else
                    //{
                    //    msg.UpdateData(new MatrixValDefinition(0, 3, 0));
                    //}
                    //Thread.Sleep(10);
                    //Lin10417.SendMasterLin(msg.MasterLinId, msg.MatrixData);
                    //Thread.Sleep(10);
                    //Lin10417.SendMasterLin(msg.MasterLinId, msg.MatrixData);
                    //Thread.Sleep(10);
                    //Lin10417.SendMasterLin(msg.MasterLinId, msg.MatrixData);
                }
            }
        }

        private void KeepExtendedSession()
        {
            while (_keepExtendedSessionThread.IsAlive)
            {
                if (!_keepExtendedSessionThread.IsAlive)
                    break;

                Thread.Sleep(850);

                if (Can == null)
                    continue;

                if (!_isInExtendedSession)
                    continue;
                lock (_lockSend)
                {
                    Can.SendExtendedCanData(CanDiagnosisRequestPhyCanId, CanBus.MyUds.KeepExtendedSessionBytes());
                    //Console.WriteLine(Name + "Keep session");
                }
            }
        }

        private void CanBus_PushCanMsg(
            string name, CanBus.CanDataPackage data, CanBus.OnPushCanDataType onPushCanDataType)
        {
            if (Can == null ||
                string.IsNullOrEmpty(name) ||
                string.IsNullOrEmpty(Can.Name) ||
                name != Can.Name ||
                !IsReadCanMsg ||
                onPushCanDataType == CanBus.OnPushCanDataType.Tx)
                return;

            IsReadCanMsg = false;
            _readCanMsgWaitHandle.Set();
        }

        #region 产品功能

        [Description("R,MOS管全部断开结果")]
        public string MosBreakResult;

        [Description("R,MOS管满占空比打开结果")]
        public string MosFulldutyFactorOpenResult;

        [Description("R,退出电流标定工况结果")]
        public string ExitCurrentCalibrationResult;

        [Description("R,释放锁电机0A电流值")]
        public double 释放锁电机0A电流值 = -9999;
        [Description("R,释放锁电机5A电流值")]
        public double 释放锁电机5A电流值 = -9999;
        [Description("R,吸合锁电机0A电流值")]
        public double 吸合锁电机0A电流值 = -9999;
        [Description("R,吸合锁电机5A电流值")]
        public double 吸合锁电机5A电流值 = -9999;
        [Description("R,执行器电机0A电流值")]
        public double 执行器电机0A电流值 = -9999;
        [Description("R,执行器电机8A电流值")]
        public double 执行器电机8A电流值 = -9999;

        [Description("R,ECU供电电压1")]
        public double Vbatt1 = -9999;
        [Description("R,ECU供电电压2")]
        public double Vbatt2 = -9999;
        [Description("R,ODH_0电压")]
        public double Odh0 = -9999;
        [Description("R,VBATT1_SW电压")]
        public double Vbatt1Sw = -9999;
        [Description("R,PWR_0电压")]
        public double Pwr0 = -9999;
        [Description("R,VCC1_5V电压")]
        public double Vcc1_5V = -9999;
        [Description("R,VCC2_5V电压")]
        public double Vcc2_5V = -9999;

        [Description("R,Home_SW开关信号-DIDB010")]
        public string HomeSw = string.Empty;
        [Description("R,Full_SW开关信号-DIDB010")]
        public string FullSw = string.Empty;
        [Description("R,Half Switch开关信号-DIDB010")]
        public string HalfSwitch = string.Empty;
        [Description("R,Hall A-DIDB010")]
        public string HallA = string.Empty;
        [Description("R,Hall B-DIDB010")]
        public string HallB = string.Empty;
        [Description("R,霍尔信号-DIDB010")]
        public double HallSingal = -9999;

        [Description("R,吸合锁电机百分之60占空比上锁电流")]
        public double 吸合锁电机百分之60占空比上锁电流 = -9999;
        [Description("R,吸合锁电机百分之60占空比复位电流")]
        public double 吸合锁电机百分之60占空比复位电流 = -9999;
        [Description("R,解电机百分之60占空比解锁电流")]
        public double 解电机百分之60占空比解锁电流 = -9999;
        [Description("R,解电机百分之60占空比解锁复位电流")]
        public double 解电机百分之60占空比解锁复位电流 = -9999;
        [Description("R,执行器电机百分之60占空比打开电流")]
        public double 执行器电机百分之60占空比打开电流 = -9999;
        [Description("R,执行器电机百分之60占空比关闭电流")]
        public double 执行器电机百分之60占空比关闭电流 = -9999;

        [Description("R,清除错误结果")]
        public string ClearFaultResult;

        [Description("R,读取故障结果")]
        public string ReadFaultResult;

        [Description("MOS管全部断开")]
        public void MosBreak()
        {
            MosBreakResult = string.Empty;

            if (Can == null)
            {
                MosBreakResult = "NG CAN未初始化";
                return;
            }

            lock (_lockSend)
            {
                MosBreakResult = IoControl(0xFD, 0x01, Uds14229Helper.InputOutputControlParameter.ShortTermAdjustment,
                    new byte[] { 0x01, 0x01 })
                    ? "OK"
                    : "NG";

                //MosBreakResult = Can.CanBusWithUds.TryInputOutputControl(
                //    CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId, CanBus.CanType.Extended, 0xFD, 0x01,
                //    Uds14229Helper.InputOutputControlParameter.ShortTermAdjustment, new byte[] { 0x01, 0x01 })
                //    ? "OK"
                //    : "NG";
            }
        }

        [Description("MOS管满占空比打开")]
        public void MosFulldutyFactorOpen()
        {
            MosFulldutyFactorOpenResult = string.Empty;

            if (Can == null)
            {
                MosFulldutyFactorOpenResult = "NG CAN未初始化";
                return;
            }
            lock (_lockSend)
            {
                MosFulldutyFactorOpenResult = IoControl(0xFD, 0x01, Uds14229Helper.InputOutputControlParameter.ShortTermAdjustment,
                    new byte[] { 0x02, 0x01 })
                    ? "OK"
                    : "NG";

                //MosFulldutyFactorOpenResult = Can.CanBusWithUds.TryInputOutputControl(
                //    CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                //    CanBus.CanType.Extended, 0xFD, 0x01,
                //    Uds14229Helper.InputOutputControlParameter.ShortTermAdjustment, new byte[] { 0x02, 0x01 })
                //    ? "OK"
                //    : "NG";
            }
        }

        [Description("退出电流标定工况")]
        public void ExitCurrentCalibration()
        {
            ExitCurrentCalibrationResult = string.Empty;

            if (Can == null)
            {
                ExitCurrentCalibrationResult = "NG CAN未初始化";
                return;
            }
            lock (_lockSend)
            {
                ExitCurrentCalibrationResult = IoControl(0xFD, 0x01, Uds14229Helper.InputOutputControlParameter.ReturnControlToEcu,
                    new byte[] { })
                    ? "OK"
                    : "NG";

                //ExitCurrentCalibrationResult = Can.CanBusWithUds.TryInputOutputControl(
                //    CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                //    CanBus.CanType.Extended, 0xFD, 0x01,
                //    Uds14229Helper.InputOutputControlParameter.ReturnControlToEcu, new byte[] { })
                //    ? "OK"
                //    : "NG";
            }
        }

        [Description("读取电流")]
        public void ReadCurrent()
        {
            释放锁电机0A电流值 = -9999;
            释放锁电机5A电流值 = -9999;
            吸合锁电机0A电流值 = -9999;
            吸合锁电机5A电流值 = -9999;
            执行器电机0A电流值 = -9999;
            执行器电机8A电流值 = -9999;

            if (Can == null)
                return;

            var result = ReadDidViaCan(0xB0, 0x00);
            if (result == null || result.Length != 12)
                return;

            释放锁电机0A电流值 = result[0] * 255 + result[1];//(result[1] << 8) + result[0];
            释放锁电机5A电流值 = result[6] * 255 + result[7];//(result[3] << 8) + result[2];
            吸合锁电机0A电流值 = result[2] * 255 + result[3];//(result[5] << 8) + result[4];
            吸合锁电机5A电流值 = result[8] * 255 + result[9];//(result[7] << 8) + result[6];
            执行器电机0A电流值 = result[4] * 255 + result[5];//(result[9] << 8) + result[8];
            执行器电机8A电流值 = result[10] * 255 + result[11];//(result[11] << 8) + result[10];
        }

        [Description("读取状态-PIDB010")]
        public void ReadStatus()
        {
            //ECU供电电压
            Vbatt1 = -9999;
            Vbatt2 = -9999;
            //ODH_0电压
            Odh0 = -9999;
            //VBATT1_SW电压
            Vbatt1Sw = -9999;
            //PWR_0电压
            Pwr0 = -9999;
            //VCC1_5V电压
            Vcc1_5V = -9999;
            //VCC2_5V电压
            Vcc2_5V = -9999;

            // Home_SW开关信号
            HomeSw = string.Empty;
            // Full_SW开关信号
            FullSw = string.Empty;
            // Half Switch开关信号
            HalfSwitch = string.Empty;
            // Hall A
            HallA = string.Empty;
            // Hall B
            HallB = string.Empty;
            // 执行器电机霍尔传感器信号
            HallSingal = -9999;

            if (Can == null)
                return;

            try
            {
                //读取ECU供电电压
                var temp = ReadDidViaCan(0xB0, 0x10);
                if (temp != null)
                {
                    //ECU供电电压
                    Vbatt1 = (temp[2] * 256 + temp[3]) * 0.01;
                    Vbatt2 = (temp[4] * 256 + temp[5]) * 0.01;
                    //ODH_0电压
                    Odh0 = (temp[6] * 256 + temp[7]) * 0.01;
                    //VBATT1_SW电压
                    Vbatt1Sw = (temp[8] * 256 + temp[9]) * 0.01;
                    //PWR_0电压
                    Pwr0 = (temp[10] * 256 + temp[11]) * 0.01;
                    //VCC1_5V电压
                    Vcc1_5V = (temp[12] * 256 + temp[13]) * 0.01;
                    //VCC2_5V电压
                    Vcc2_5V = (temp[14] * 256 + temp[15]) * 0.01;

                    // Home_SW开关信号
                    HomeSw = GetBit(temp[0], 2) ? "1" : "0";
                    // Full_SW开关信号
                    FullSw = GetBit(temp[0], 0) ? "1" : "0";
                    // Half Switch开关信号
                    HalfSwitch = GetBit(temp[0], 5) ? "1" : "0";
                    // Hall A
                    HallA = GetBit(temp[19], 0) ? "1" : "0";
                    // Hall B
                    HallB = GetBit(temp[19], 1) ? "1" : "0";
                    // 执行器电机霍尔传感器信号
                    HallSingal = (temp[16] << 8) + temp[15];
                }
            }
            catch (Exception)
            {
                //ECU供电电压
                Vbatt1 = -9999;
                Vbatt2 = -9999;
                //ODH_0电压
                Odh0 = -9999;
                //VBATT1_SW电压
                Vbatt1Sw = -9999;
                //PWR_0电压
                Pwr0 = -9999;
                //VCC1_5V电压
                Vcc1_5V = -9999;
                //VCC2_5V电压
                Vcc2_5V = -9999;

                // Home_SW开关信号
                HomeSw = string.Empty;
                // Full_SW开关信号
                FullSw = string.Empty;
                // Half Switch开关信号
                HalfSwitch = string.Empty;
                // Hall A
                HallA = string.Empty;
                // Hall B
                HallB = string.Empty;
                // 执行器电机霍尔传感器信号
                HallSingal = -9999;
            }
        }

        [Description("解锁马达")]
        public void UnlockMotor(string delayMs)
        {
            吸合锁电机百分之60占空比上锁电流 = -9999;
            吸合锁电机百分之60占空比复位电流 = -9999;
            解电机百分之60占空比解锁电流 = -9999;
            解电机百分之60占空比解锁复位电流 = -9999;
            执行器电机百分之60占空比打开电流 = -9999;
            执行器电机百分之60占空比关闭电流 = -9999;

            if (Can == null)
                return;

            int delay;
            if (!int.TryParse(delayMs, out delay))
                delay = 2000;

            // ============吸合锁电机百分之60占空比上锁===================
            if (IoControl(0xFD, 0x00, Uds14229Helper.InputOutputControlParameter.ShortTermAdjustment, new byte[] { 0x09, 0x00, 0x3C, 0x00, 0x05 }))
            {
                Thread.Sleep(delay);
                var read1 = ReadDidViaCan(0xB0, 0x12);
                if (read1 != null)
                {
                    try
                    {
                        吸合锁电机百分之60占空比上锁电流 = ((read1[8] << 8) + read1[9]) * 0.01;
                        //吸合锁电机百分之60占空比上锁电流 = ((read1[9] << 8) + read1[8]) / 2048F;
                    }
                    catch (Exception)
                    {
                        吸合锁电机百分之60占空比上锁电流 = -9999;
                    }
                }
            }

            // ===========吸合锁电机60%占空比复位==================
            if (IoControl(0xFD, 0x00, Uds14229Helper.InputOutputControlParameter.ShortTermAdjustment, new byte[] { 0x11, 0x00, 0x3C, 0x00, 0x05 }))
            {
                Thread.Sleep(delay);
                var read1 = ReadDidViaCan(0xB0, 0x12);
                if (read1 != null)
                {
                    try
                    {
                        吸合锁电机百分之60占空比复位电流 = ((read1[8] << 8) + read1[9]) * 0.01;
                        //吸合锁电机百分之60占空比复位电流 = ((read1[9] << 8) + read1[8]) / 2048F;
                    }
                    catch (Exception)
                    {
                        吸合锁电机百分之60占空比复位电流 = -9999;
                    }
                }
            }

            // ===========解锁电机60%占空比解锁==================
            if (IoControl(0xFD, 0x00, Uds14229Helper.InputOutputControlParameter.ShortTermAdjustment, new byte[] { 0x03, 0x3C, 0x00, 0x00, 0x03 }))
            {
                Thread.Sleep(delay);
                var read1 = ReadDidViaCan(0xB0, 0x12);
                if (read1 != null)
                {
                    try
                    {
                        解电机百分之60占空比解锁电流 = ((read1[4] << 8) + read1[5]) * 0.01;
                        //解电机百分之60占空比解锁电流 = ((read1[5] << 8) + read1[4]) / 2048F;
                    }
                    catch (Exception)
                    {
                        解电机百分之60占空比解锁电流 = -9999;
                    }
                }
            }

            // ==============解锁电机60%占空比解锁复位===================
            if (IoControl(0xFD, 0x00, Uds14229Helper.InputOutputControlParameter.ShortTermAdjustment, new byte[] { 0x05, 0x00, 0x99, 0x00, 0x13 }))
            {
                Thread.Sleep(delay);
                var read1 = ReadDidViaCan(0xB0, 0x12);
                if (read1 != null)
                {
                    try
                    {
                        解电机百分之60占空比解锁复位电流 = ((read1[4] << 8) + read1[5]) * 0.01;
                        //解电机百分之60占空比解锁复位电流 = ((read1[5] << 8) + read1[4]) / 2048F;
                    }
                    catch (Exception)
                    {
                        解电机百分之60占空比解锁复位电流 = -9999;
                    }
                }
            }

            // ==============执行器电机60%占空比打开===================
            if (IoControl(0xFD, 0x00, Uds14229Helper.InputOutputControlParameter.ShortTermAdjustment, new byte[] { 0x21, 0x00, 0x00, 0x3C, 0x09 }))
            {
                Thread.Sleep(delay);
                var read1 = ReadDidViaCan(0xB0, 0x12);
                if (read1 != null)
                {
                    try
                    {
                        执行器电机百分之60占空比打开电流 = ((read1[12] << 8) + read1[13]) * 0.01;
                        //执行器电机百分之60占空比打开电流 = ((read1[13] << 8) + read1[12]) / 2048F;
                    }
                    catch (Exception)
                    {
                        执行器电机百分之60占空比打开电流 = -9999;
                    }
                }
            }

            // ==============执行器电机60%占空比关闭===================
            if (IoControl(0xFD, 0x00, Uds14229Helper.InputOutputControlParameter.ShortTermAdjustment, new byte[] { 0x41, 0x00, 0x00, 0x99, 0x25 }))
            {
                Thread.Sleep(delay);
                var read1 = ReadDidViaCan(0xB0, 0x12);
                if (read1 != null)
                {
                    try
                    {
                        执行器电机百分之60占空比关闭电流 = ((read1[12] << 8) + read1[13]) * 0.01;
                        //执行器电机百分之60占空比关闭电流 = ((read1[13] << 8) + read1[12]) / 2048F;
                    }
                    catch (Exception)
                    {
                        执行器电机百分之60占空比关闭电流 = -9999;
                    }
                }
            }

            // ===============锁电机和撑杆电机退出指令===================
            IoControl(0xFD, 0x00, Uds14229Helper.InputOutputControlParameter.ReturnControlToEcu, new byte[] { });
            //Can.CanBusWithUds.TryInputOutputControl(
            //    CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId, CanBus.CanType.Extended, 0xFD, 0x00,
            //    Uds14229Helper.InputOutputControlParameter.ReturnControlToEcu, new byte[] { });
            Thread.Sleep(delay);
        }

        //[Description("读取霍尔传感器-DIDB010")]
        //public void ReadHallSingal()
        //{
        //    HallSingal = -9999;
        //    var responseData = ReadDidViaCan(0xB0, 0x10);
        //    if (responseData == null)
        //        return;
        //    try
        //    {
        //        HallSingal = (responseData[17] << 8) + responseData[16];
        //    }
        //    catch (Exception)
        //    {
        //        HallSingal = -9999;
        //    }
        //}

        [Description("清除错误")]
        public void ClearFault()
        {
            ClearFaultResult = string.Empty;
            ReadFaultResult = string.Empty;

            if (Can == null)
                return;

            lock (_lockSend)
            {
                if (Can.CanBusWithUds.TryClearAllGroupsDiagnosticInformation(
                   CanDiagnosisRequestPhyCanId,
                   CanDiagnosisResponseCanId,
                   CanBus.CanType.Extended))
                {
                    ClearFaultResult = @"OK";
                }

                byte[] echo;
                if (Can.CanBusWithUds.TryReadDtcInfomation(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId,
                    CanBus.CanType.Extended, 0x02, 0x09,
                    out echo))
                {
                    if (echo == null)
                        return;
                    //ReadFaultResult = "OK ";
                    foreach (var t in echo)
                        ReadFaultResult += ValueHelper.GetHextStr(t);

                    if (string.IsNullOrEmpty(ReadFaultResult))
                    {
                        ReadFaultResult = "OK";
                    }
                }
            }
        }

        public bool GetBit(byte value, int bitnum)
        {
            return (value & (1 << bitnum)) > 0;
        }

        private bool IoControl(
            byte didHi, byte didLo, Uds14229Helper.InputOutputControlParameter inputOutputControlParameter, byte[] optBytes)
        {
            if (Can == null)
                return false;

            if (Can.CanBusWithUds.TryInputOutputControl(
                CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId, CanBus.CanType.Extended, didHi, didLo,
                inputOutputControlParameter,
                optBytes))
                return true;

            Thread.Sleep(200);
            return Can.CanBusWithUds.TryInputOutputControl(
                CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId, CanBus.CanType.Extended, didHi, didLo,
                inputOutputControlParameter,
                optBytes);
        }

        //private void IoControl(byte didHi, byte didLo, IEnumerable<byte> optionBytes)
        //{
        //    if (Can == null)
        //        return;

        //    lock (_lockSend)
        //    {
        //        Can.CanBusWithUds.TryInputOutputControl(
        //            CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId, CanBus.CanType.Standard, didHi, didLo,
        //            Uds14229Helper.InputOutputControlParameter.ShortTermAdjustment, optionBytes);
        //    }
        //}

        #endregion

        #region 唤醒休眠，session选择
        [Description("唤醒")]
        public void Awake()
        {
            _isSleep = false;
        }

        [Description("休眠")]
        public void Sleep()
        {
            _isSleep = true;
        }

        [Description("进入正常模式")]
        public void EnterDefaultSession()
        {
            if (Can == null)
                return;

            lock (_lockSend)
            {
                _isInExtendedSession = false;

                if (Can.CanBusWithUds.TryEnterDefaultSession(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId, CanBus.CanType.Extended, CanBus.CanProtocol.Can))
                    return;

                Thread.Sleep(500);

                Can.CanBusWithUds.TryEnterDefaultSession(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId, CanBus.CanType.Extended, CanBus.CanProtocol.Can);
            }
        }

        [Description("进入拓展模式")]
        public void EnterExtendedSession()
        {
            if (Can == null)
                return;

            lock (_lockSend)
            {
                if (Can.CanBusWithUds.TryEnterExtendedSession(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId, CanBus.CanType.Extended))
                {
                    _isInExtendedSession = true;
                    return;
                }

                Thread.Sleep(500);
                if (Can.CanBusWithUds.TryEnterExtendedSession(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId, CanBus.CanType.Extended))
                {
                    _isInExtendedSession = true;
                }
            }
        }

        [Description("进入编程模式")]
        public void EnterProgramSession()
        {
            if (Can == null)
                return;

            _isInExtendedSession = false;

            lock (_lockSend)
            {
                if (Can.CanBusWithUds.TryEnterProgrammingSession(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId, CanBus.CanType.Extended))
                {
                    return;
                }

                Thread.Sleep(500);
                Can.CanBusWithUds.TryEnterProgrammingSession(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId, CanBus.CanType.Extended);
            }
        }

        [Description("关闭正常通信")]
        public void DisableRxAndTxCommunication()
        {
            Can.CanBusWithUds.TryCommunicationControl(
                CanDiagnosisRequestPhyCanId,
                CanDiagnosisResponseCanId,
                CanBus.CanType.Extended,
                Uds14229Helper.CommunicationControl.DisableRxAndTx);
        }

        #endregion

        #region 版本信息,写入与读取
        [Description("R,初始零件号-BaseModulePartNumber-F1CC")]
        public string BaseModulePartNumber;
        [Description("R,初始版本号-BaseModuleAlphaCode-F1DC")]
        public string BaseModuleAlphaCode;
        [Description("R,读左右门配置-A800")]
        public string PdmConfig;
        [Description("R,当前零件号-EndModulePartNumber-F1CB")]
        public string EndModulePartNumber;
        [Description("R,当前版本号-当前版本号-F1DB")]
        public string EndModuleAlphaCode;
        [Description("R,编程日期-F199")]
        public string ProgrammingDate;
        [Description("R,编程日期-Year-F199")]
        public int ProgrammingDateOfYear = -9999;
        [Description("R,编程日期-Month-F199")]
        public int ProgrammingDateOfMonth = -9999;
        [Description("R,编程日期-Day-F199")]
        public int ProgrammingDateOfDay = -9999;
        [Description("R,VPPS-F0AB")]
        public string Vpps;
        [Description("R,DUNS-F0B3")]
        public string Duns;
        [Description("R,MEC-F1A0")]
        public int ManufacturersEnableCounter = -9999;
        [Description("R,PDMVersion-B023")]
        public string PdmVersion = string.Empty;

        [Description("R,ECU_ID-F0F3")]
        public string EcuId;
        [Description("R,追溯信息MTC-F0B4")]
        public string TrackInfo;

        [Description("写入左右门配置")]
        public void WritePdmConfig(string valueHex)
        {
            if (Can == null)
                return;

            try
            {
                lock (_lockSend)
                {
                    Can.CanBusWithUds.TryWriteData(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                        CanBus.CanType.Extended, CanBus.CanProtocol.Can, 0xA8, 0x00,
                        new[] { Convert.ToByte(valueHex, 16) });
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        [Description("读左右门配置")]
        public void ReadPdmConfig()
        {
            PdmConfig = string.Empty;
            PdmConfig = ValueHelper.GetHextStr(ReadDidViaCan(0xA8, 0x00)).Replace(" ", "");
        }

        [Description("写入当前零件号-EndModulePartNumber-F1CB")]
        public void WriteEndModulePartNumber(string value)
        {
            if (Can == null)
                return;

            try
            {
                lock (_lockSend)
                {
                    Can.CanBusWithUds.TryWriteData(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                        CanBus.CanType.Extended, CanBus.CanProtocol.Can, 0xF1, 0xCB,
                        BitConverter.GetBytes(uint.Parse(value)).Reverse().ToArray());
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        [Description("ReadF1CB-当前零件号-EndModulePartNumber")]
        public void ReadEndModulePartNumber()
        {
            EndModulePartNumber = string.Empty;
            EndModulePartNumber = ValueHelper.GetDecimal(ReadDidViaCan(0xF1, 0xCB).ToArray()).ToString();
        }

        [Description("写入当前版本号-EndModuleAlphaCode-F1DB")]
        public void WriteEndModuleAlphaCode(string value)
        {
            if (Can == null)
                return;

            try
            {
                lock (_lockSend)
                {
                    Can.CanBusWithUds.TryWriteData(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                        CanBus.CanType.Extended, CanBus.CanProtocol.Can, 0xF1, 0xDB,
                        Encoding.ASCII.GetBytes(value));
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        [Description("ReadF1DB-当前版本号-EndModuleAlphaCode")]
        public void ReadEndModuleAlphaCode()
        {
            EndModuleAlphaCode = string.Empty;
            EndModuleAlphaCode = ReadDidViaCan(0xF1, 0xDB).GetStringByAsciiBytes(true);
        }

        [Description("写入并读取刷新日期-F199")]
        public void WriteAndReadProgrammingDate()
        {
            ProgrammingDate = string.Empty;
            ProgrammingDateOfYear = -9999;
            ProgrammingDateOfMonth = -9999;
            ProgrammingDateOfDay = -9999;

            if (Can == null)
                return;

            string str;

            // 可以重复写入，无需用烧写器擦除
            // 写入前，需要先10 03，再27 01解锁
            var dateTime = DateTime.Now;

            str = string.Format("{0}{1}{2}", dateTime.Year, dateTime.Month.ToString().PadLeft(2, '0'),
               dateTime.Day.ToString().PadLeft(2, '0'));

            var bs = new List<byte>();

            for (var i = 0; i < str.Length; i = i + 2)
                bs.Add(Convert.ToByte(str.Substring(i, 2), 16));

            lock (_lockSend)
            {
                if (!Can.CanBusWithUds.TryWriteData(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                    CanBus.CanType.Extended, CanBus.CanProtocol.Can, 0xF1, 0x99, bs.ToArray()))
                {
                    //Thread.Sleep(50);
                    //Can.CanBusWithUds.TryWriteData(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                    //CanBus.CanType.Extended, CanBus.CanProtocol.Can, 0xF1, 0x99, bs.ToArray());
                }
            }

            Thread.Sleep(100);
            var read = ValueHelper.GetHextStr(ReadDidViaCan(0xF1, 0x99)).Replace(" ", "");

            if (!string.IsNullOrEmpty(read) && read.Length == 8)
            {
                ProgrammingDateOfYear = int.Parse(read.Substring(0, 4));
                ProgrammingDateOfMonth = int.Parse(read.Substring(4, 2));
                ProgrammingDateOfDay = int.Parse(read.Substring(6, 2));

                if (read == str)
                    ProgrammingDate = "OK " + read;
                else
                    ProgrammingDate = "NG " + read;
            }
            else
            {
                ProgrammingDate = "NG " + read;
            }

            //if (read == str)
            //    ProgrammingDate = "OK " + read;
            //else
            //    ProgrammingDate = "NG " + read;
        }

        [Description("读取刷新日期-F199")]
        public void ReadProgrammingDate()
        {
            ProgrammingDateOfYear = -9999;
            ProgrammingDateOfMonth = -9999;
            ProgrammingDateOfDay = -9999;

            if (Can == null)
                return;

            var read = ValueHelper.GetHextStr(ReadDidViaCan(0xF1, 0x99)).Replace(" ", "");
            if (!string.IsNullOrEmpty(read) && read.Length == 8)
            {
                try
                {
                    ProgrammingDateOfYear = int.Parse(read.Substring(0, 4));
                    ProgrammingDateOfMonth = int.Parse(read.Substring(4, 2));
                    ProgrammingDateOfDay = int.Parse(read.Substring(6, 2));
                }
                catch (Exception)
                {
                    ProgrammingDateOfYear = -9999;
                    ProgrammingDateOfMonth = -9999;
                    ProgrammingDateOfDay = -9999;
                }
            }
        }

        [Description("写入VPPS-F0AB")]
        public void WriteVpps(string value)
        {
            if (Can == null)
                return;

            try
            {
                lock (_lockSend)
                {
                    Can.CanBusWithUds.TryWriteData(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                        CanBus.CanType.Extended, CanBus.CanProtocol.Can, 0xF0, 0xAB,
                        Encoding.ASCII.GetBytes(value));
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        [Description("ReadF0AB-VPPS")]
        public void ReadVpps()
        {
            Vpps = string.Empty;
            Vpps = ReadDidViaCan(0xF0, 0xAB).GetStringByAsciiBytes(true);
        }

        [Description("写入DUNS-F0B3")]
        public void WriteDuns(string value)
        {
            if (Can == null)
                return;

            try
            {
                lock (_lockSend)
                {
                    Can.CanBusWithUds.TryWriteData(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                        CanBus.CanType.Extended, CanBus.CanProtocol.Can, 0xF0, 0xB3,
                        Encoding.ASCII.GetBytes(value));
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        [Description("ReadF0B3-DUNS")]
        public void ReadDuns()
        {
            Duns = string.Empty;
            Duns = ReadDidViaCan(0xF0, 0xB3).GetStringByAsciiBytes(true);
        }

        [Description("ReadF1CC-初始零件号-BaseModulePartNumber")]
        public void ReadBaseModulePartNumber()
        {
            BaseModulePartNumber = string.Empty;
            BaseModulePartNumber = ValueHelper.GetDecimal(ReadDidViaCan(0xF1, 0xCC).ToArray()).ToString();
        }

        [Description("ReadF1DC-初始版本号-BaseModuleAlphaCode")]
        public void ReadBaseModuleAlphaCode()
        {
            BaseModuleAlphaCode = string.Empty;
            BaseModuleAlphaCode = ReadDidViaCan(0xF1, 0xDC).GetStringByAsciiBytes(true);
        }

        [Description("写入并读取MEC-F1A0")]
        public void WriteAndReadManufacturersEnableCounter(string mec)
        {
            ManufacturersEnableCounter = -9999;

            if (Can == null)
                return;

            byte mecValue;
            if (byte.TryParse(mec, out mecValue))
            {
                var bs = new List<byte> { mecValue };

                lock (_lockSend)
                {
                    Can.CanBusWithUds.TryWriteData(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                        CanBus.CanType.Extended, CanBus.CanProtocol.Can, 0xF1, 0xA0, bs.ToArray());
                }
            }

            Thread.Sleep(20);
            ManufacturersEnableCounter = ValueHelper.GetDecimal(ReadDidViaCan(0xF1, 0xA0));
        }

        [Description("读取MEC-F1A0")]
        public void ReadManufacturersEnableCounter()
        {
            ManufacturersEnableCounter = -9999;

            if (Can == null)
                return;

            ManufacturersEnableCounter = ValueHelper.GetDecimal(ReadDidViaCan(0xF1, 0xA0));
        }

        [Description("读取PDMVersion-B023")]
        public void ReadPdmVersion()
        {
            PdmVersion = string.Empty;
            PdmVersion = ReadDidViaCan(0xB0, 0x23).GetStringByAsciiBytes(true);
        }

        public string ToWriteBarcode = string.Empty;

        [Description("根据二维码写入MTC和ECUID-F0B4和F0F3")]
        public void WriteMtcAndEcuIdByBarcode()
        {
            EcuId = string.Empty;
            TrackInfo = string.Empty;

            if (Can == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(ToWriteBarcode))
            {
                EcuId = "NG 条码为空";
                TrackInfo = "NG 条码为空";
                return;
            }

            if (ToWriteBarcode.Length != 63)
            {
                EcuId = "NG 条码长度不对";
                TrackInfo = "NG 条码长度不对";
                return;
            }

            var barcodeHex = ValueHelper.GetHextStr(Encoding.ASCII.GetBytes(ToWriteBarcode)).Replace(" ", "");
            if (!barcodeHex.StartsWith("5B293E1E"))
            {
                EcuId = "NG 条码格式不对";
                TrackInfo = "NG 条码格式不对";
                return;
            }

            if (!barcodeHex.EndsWith("1E04"))
            {
                EcuId = "NG 条码格式不对";
                TrackInfo = "NG 条码格式不对";
                return;
            }

            var mtc = ToWriteBarcode.Substring(ToWriteBarcode.Length - 16 - 2, 16);
            var mtcBytes = Encoding.ASCII.GetBytes(mtc);
            var ecuIdBytes = mtcBytes.Reverse().ToArray();
            var ecuIdHex = ValueHelper.GetHextStr(ecuIdBytes).Replace(" ", "");

            lock (_lockSend)
            {
                Can.CanBusWithUds.TryWriteData(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                    CanBus.CanType.Extended, CanBus.CanProtocol.Can, 0xF0, 0xB4, mtcBytes);
            }

            lock (_lockSend)
            {
                Can.CanBusWithUds.TryWriteData(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                    CanBus.CanType.Extended, CanBus.CanProtocol.Can, 0xF0, 0xF3, ecuIdBytes);
            }

            var ecuId = ValueHelper.GetHextStr(ReadDidViaCan(0xF0, 0xF3)).Replace(" ", "");
            var trackInfo = ReadDidViaCan(0xF0, 0xB4).GetStringByAsciiBytes(true);

            if (trackInfo == mtc)
                TrackInfo = "OK " + trackInfo;
            else
                TrackInfo = "NG " + trackInfo;

            if (ecuIdHex == ecuId)
                EcuId = "OK " + ecuId;
            else
                EcuId = "NG " + ecuId;
        }

        [Description("读EcuId-F0F3")]
        public void ReadEcuId()
        {
            EcuId = string.Empty;
            EcuId = ValueHelper.GetHextStr(ReadDidViaCan(0xF0, 0xF3)).Replace(" ", "");
        }

        [Description("读MTC-F0B4")]
        public void ReadMtc()
        {
            TrackInfo = string.Empty;
            TrackInfo = ReadDidViaCan(0xF0, 0xB4).GetStringByAsciiBytes(true);
        }

        #endregion

        #region 安全信息

        [Description("安全访问0102")]
        public void SecurityAccess0102()
        {
            byte[] seedBytes;
            if (Can.CanBusWithUds.TryRequestSeed(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                CanBus.CanType.Extended, 0x01, out seedBytes))
            {
                var keyBytes = new byte[] { 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0x00, 0x11, 0x22 };

                if (Can.CanBusWithUds.TrySendKey(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                    CanBus.CanType.Extended, 0x02, keyBytes))
                {
                }
            }
        }

        [Description("R,安全访问0D0E结果")]
        public string SecurityAccessResult = string.Empty;

        [Description("安全访问0D0E")]
        public void SecurityAccess0D0E()
        {
            {
                SecurityAccessResult = string.Empty;

                byte[] seedBytes;
                if (Can.CanBusWithUds.TryRequestSeed(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                    CanBus.CanType.Extended, 0x0D, out seedBytes))
                {
                    var keyBytes = new byte[] { 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0x00, 0x11, 0x22 };

                    if (Can.CanBusWithUds.TrySendKey(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                        CanBus.CanType.Extended, 0x0E, keyBytes))
                    {
                        SecurityAccessResult = "OK";
                        return;
                    }
                }

                SecurityAccessResult = "NG";
            }

            if (SecurityAccessResult == "OK")
            {
                return;
            }
            else
            {
                SecurityAccessResult = string.Empty;

                byte[] seedBytes;
                if (Can.CanBusWithUds.TryRequestSeed(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                    CanBus.CanType.Extended, 0x0D, out seedBytes))
                {
                    var keyBytes = new byte[] { 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0x00, 0x11, 0x22 };

                    if (Can.CanBusWithUds.TrySendKey(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                        CanBus.CanType.Extended, 0x0E, keyBytes))
                    {
                        SecurityAccessResult = "OK";
                        return;
                    }
                }

                SecurityAccessResult = "NG";
            }
        }

        #endregion

        #region 生成二维码

        //[Description("R,生成二维码内容")]
        //public string GeneratedBarcode;

        //[Description("生成二维码内容")]
        //public void GenerateBarcode(string generalPartNo, string generalVpps, string seeyaoDuns)
        //{
        //    GeneratedBarcode = string.Empty;

        //    //generalPartNo = "26467265";
        //    //generalVpps = "8210700000000X";
        //    //seeyaoDuns = "547656863";
        //    //TrackInfo = "OK 1A22238AAA000005";

        //    if (string.IsNullOrEmpty(generalPartNo) || generalPartNo.Length != 8)
        //        return;

        //    if (string.IsNullOrEmpty(generalVpps) || generalVpps.Length != 14)
        //        return;

        //    if (string.IsNullOrEmpty(seeyaoDuns) || seeyaoDuns.Length != 9)
        //        return;

        //    if (string.IsNullOrEmpty(TrackInfo) || !TrackInfo.StartsWith("OK "))
        //        return;

        //    var tracInfo = TrackInfo.Replace("OK ", "");

        //    var matrixBytes = new List<byte>();
        //    matrixBytes.AddRange(new byte[] { 0x5B, 0x29, 0x3E, 0x1E, 0x30, 0x36, 0x1D, 0x59 }); // header:[)><RS>06<GS>Y
        //    matrixBytes.AddRange(Encoding.ASCII.GetBytes(generalVpps)); // 通用VPPS号
        //    matrixBytes.Add(0x1D); //<GS>
        //    matrixBytes.AddRange(Encoding.ASCII.GetBytes("P" + generalPartNo)); //  P+通用零件号
        //    matrixBytes.Add(0x1D); //<GS>
        //    matrixBytes.AddRange(Encoding.ASCII.GetBytes("12V" + seeyaoDuns)); //  12V+DUNS邓氏码
        //    matrixBytes.Add(0x1D); //<GS>
        //    matrixBytes.AddRange(Encoding.ASCII.GetBytes("T" + tracInfo)); //  追溯码
        //    matrixBytes.AddRange(new byte[] { 0x1E, 0x04 }); // trailer <RS>

        //    GeneratedBarcode = matrixBytes.ToArray().GetStringByAsciiBytes(false);

        //    //var generatedBarcode = matrixBytes.ToArray().GetStringByAsciiBytes(true);
        //    //Console.WriteLine(generatedBarcode);

        //    //var getTrackInfo = GeneratedBarcode.Substring(GeneratedBarcode.Length - 16 - 2, 16);
        //}

        #endregion

        private byte[] ReadDidViaCan(byte didHi, byte didLo)
        {
            if (Can == null)
                return new List<byte>().ToArray();

            lock (Can)
            {
                byte[] echoBytes;
                if (Can.CanBusWithUds.TryReadData(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId,
                    CanBus.CanType.Extended, CanBus.CanProtocol.Can, didHi, didLo, out echoBytes))
                {
                    if (echoBytes != null)
                        return echoBytes;

                    return new List<byte>().ToArray();

                    //Thread.Sleep(250);
                    //if (!Can.CanBusWithUds.TryReadData(
                    //    CanDiagnosisRequestPhyCanId,
                    //    CanDiagnosisResponseCanId,
                    //    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    //    didHi, didLo, out echoBytes))
                    //    return new List<byte>().ToArray();
                    //return echoBytes ?? new List<byte>().ToArray();
                }

                //return new List<byte>().ToArray();

                Thread.Sleep(250);
                if (!Can.CanBusWithUds.TryReadData(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId,
                    CanBus.CanType.Extended, CanBus.CanProtocol.Can,
                    didHi, didLo, out echoBytes))
                    return new List<byte>().ToArray();

                if (echoBytes != null)
                    return echoBytes;

                Thread.Sleep(250);
                if (!Can.CanBusWithUds.TryReadData(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    didHi, didLo, out echoBytes))
                    return new List<byte>().ToArray();

                if (echoBytes != null)
                    return echoBytes;

                return new List<byte>().ToArray();

                Thread.Sleep(250);
                if (!Can.CanBusWithUds.TryReadData(
                    CanDiagnosisRequestPhyCanId,
                    CanDiagnosisResponseCanId,
                    CanBus.CanType.Extended, CanBus.CanProtocol.Can,
                    didHi, didLo, out echoBytes))
                    return new List<byte>().ToArray();
                return echoBytes ?? new List<byte>().ToArray();
            }
        }
    }
}
