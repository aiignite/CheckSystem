using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using CommonUtility.FileOperator;

namespace Controller
{
    [Description("CAN-Product,SGM458PLG")]
    public sealed class Sgm458Plg : ControllerBase
    {
        public CanBus Can;

        public Sgm458Plg(string name)
            : base(name)
        {
            const string macKeyStrs = "02,00000000000000000000000000000055,77D76C8151D71E1482860BDF8F5E98A2C26BF75D8680824AE5A396393EE73970,0A5DEC164090C65D2363646BF718E7F5\r\n" +
                                      "03,00000000000000000000000000000066,77D76C8151D71E1482860BDF8F5E98A2C26BF75D8680824AE5A396393EE73970,A6E84615F8155F2E62E1984B82163F8A\r\n" +
                                      "04,00000000000000000000000000000077,77D76C8151D71E1482860BDF8F5E98A2C26BF75D8680824AE5A396393EE73970,3882DC3F9D8A27C419E651744330C041\r\n" +
                                      "05,00000000000000000000000000000088,77D76C8151D71E1482860BDF8F5E98A2C26BF75D8680824AE5A396393EE73970,6D2392256D2174F42F40D44876C6A3D7\r\n" +
                                      "06,00000000000000000000000000000099,77D76C8151D71E1482860BDF8F5E98A2C26BF75D8680824AE5A396393EE73970,F6512DD4C59D160A1935251B965C2010\r\n" +
                                      "07,000000000000000000000000000000AA,77D76C8151D71E1482860BDF8F5E98A2C26BF75D8680824AE5A396393EE73970,A85F5538119362022BB9CC8708F43A3B\r\n" +
                                      "08,000000000000000000000000000000BB,77D76C8151D71E1482860BDF8F5E98A2C26BF75D8680824AE5A396393EE73970,6CAFB7F8DC6CB7695EF5698088E55D23\r\n" +
                                      "09,000000000000000000000000000000CC,77D76C8151D71E1482860BDF8F5E98A2C26BF75D8680824AE5A396393EE73970,806D3B3DFEE6301FC20D16DEAA6B8285\r\n" +
                                      "0A,000000000000000000000000000000DD,77D76C8151D71E1482860BDF8F5E98A2C26BF75D8680824AE5A396393EE73970,4F390E5EC96C2FFCE7788C2E0886F96C\r\n" +
                                      "0B,00000000000000000000000000000044,C3B088BE32431CB6FB6F35F419C51A4438B0048E61A4F81413B3DB997AE4D7B1,E74D3B7BD07287A076003C7A084516FB\r\n" +
                                      "0C,00000000000000000000000000000055,C3B088BE32431CB6FB6F35F419C51A4438B0048E61A4F81413B3DB997AE4D7B1,9112563FF71521A21792975DE78BC5C6\r\n" +
                                      "0D,00000000000000000000000000000066,C3B088BE32431CB6FB6F35F419C51A4438B0048E61A4F81413B3DB997AE4D7B1,4ECA5A57D49168D77E3CDB025F681E8A\r\n" +
                                      "0E,00000000000000000000000000000077,C3B088BE32431CB6FB6F35F419C51A4438B0048E61A4F81413B3DB997AE4D7B1,65DCD0223DED6A9A3FE34B071871A378\r\n" +
                                      "0F,00000000000000000000000000000088,C3B088BE32431CB6FB6F35F419C51A4438B0048E61A4F81413B3DB997AE4D7B1,2EED4501B83F8015EA3DF084BD729CAD\r\n" +
                                      "10,00000000000000000000000000000099,C3B088BE32431CB6FB6F35F419C51A4438B0048E61A4F81413B3DB997AE4D7B1,33BD9F647474A359456A5986117072FE\r\n" +
                                      "11,000000000000000000000000000000AA,C3B088BE32431CB6FB6F35F419C51A4438B0048E61A4F81413B3DB997AE4D7B1,49858244591E42A1C9F1B87FAFD37E38\r\n" +
                                      "12,000000000000000000000000000000BB,C3B088BE32431CB6FB6F35F419C51A4438B0048E61A4F81413B3DB997AE4D7B1,90C30BE42DA00189E18F83C2D38DE11B\r\n" +
                                      "13,000000000000000000000000000000CC,C3B088BE32431CB6FB6F35F419C51A4438B0048E61A4F81413B3DB997AE4D7B1,73611029D63C6DA424AA85A136E7D9B2\r\n" +
                                      "14,000000000000000000000000000000DD,C3B088BE32431CB6FB6F35F419C51A4438B0048E61A4F81413B3DB997AE4D7B1,75DEF3CD1029C3464B3EA6F6BE3F9B1C\r\n";

            var spMacKey = macKeyStrs.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var macKey in spMacKey)
            {
                var keySlot = Convert.ToByte(macKey.Split(',')[0], 16);
                _macKeyList.Add(keySlot, new List<string>());

                for (var i = 1; i < 4; i++)
                    _macKeyList[keySlot].Add(macKey.Split(',')[i]);
            }

            if (!EachWorkstationCurrentEcuId.ContainsKey(name))
                EachWorkstationCurrentEcuId.Add(name, "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF");

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

        ~Sgm458Plg()
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
        private readonly Dictionary<byte, List<string>> _macKeyList = new Dictionary<byte, List<string>>();
        private readonly EventWaitHandle _readCanMsgWaitHandle = new AutoResetEvent(false);
        private bool IsReadCanMsg { get; set; }

        private void KeepNetwork()
        {
            while (_keepNetworkThread.IsAlive)
            {
                if (!_keepNetworkThread.IsAlive)
                    break;

                Thread.Sleep(100);

                if (Can == null)
                    continue;

                if (_isSleep)
                    continue;
                lock (_lockSend)
                {
                    Can.SendStandardCanData(
                       0x621, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
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

        [Description("R,锁电机0A")]
        public double LockMotoCurrent0A = -9999;
        [Description("R,锁电机3A")]
        public double LockMotoCurrent3A = -9999;
        [Description("R,撑杆电机0A")]
        public double CgMotoCurrent0A = -9999;
        [Description("R,撑杆电机8A")]
        public double CgMotoCurrent8A = -9999;

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
        [Description("R,SHF_FC_SW")]
        public string ShtFcSw;
        [Description("R,NEUT_SW")]
        public string NeutSw;
        [Description("R,PRIM_SW")]
        public string PrimSw;
        [Description("R,Tertairy_Switch")]
        public string TertairySwitch;
        [Description("R,CLS_HND_SW")]
        public string ClsHndSw;

        [Description("R,60%上锁电流")]
        public double Lock60 = -9999;
        [Description("R,60%解锁电流")]
        public double Unlock60 = -9999;
        [Description("R,60%撑杆上锁电流")]
        public double ChengGan60 = -9999;
        [Description("R,60%撑杆解锁电流")]
        public double UnChengGan60 = -9999;

        [Description("R,霍尔信号")]
        public double HallSingal = -9999;

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
            LockMotoCurrent0A = -9999;
            LockMotoCurrent3A = -9999;
            CgMotoCurrent0A = -9999;
            CgMotoCurrent8A = -9999;

            if (Can == null)
                return;

            var result = ReadDidViaCan(0x80, 0xBA);
            if (result == null || result.Length != 8)
                return;
            LockMotoCurrent0A = (result[1] << 8) + result[0];
            LockMotoCurrent3A = (result[3] << 8) + result[2];
            CgMotoCurrent0A = (result[5] << 8) + result[4];
            CgMotoCurrent8A = (result[7] << 8) + result[6];
        }

        [Description("读取状态")]
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
            //SHF_FC_SW
            ShtFcSw = string.Empty;
            //NEUT_SW
            ShtFcSw = string.Empty;
            //PRIM_SW
            NeutSw = string.Empty;
            //Tertairy_Switch
            TertairySwitch = string.Empty;
            //CLS_HND_SW
            ClsHndSw = string.Empty;

            if (Can == null)
                return;

            try
            {
                //读取ECU供电电压
                byte[] temp = null;
                temp = ReadDidViaCan(0x82, 0x30);
                if (temp != null)
                {
                    //ECU供电电压
                    Vbatt1 = ((temp[2] << 8) + temp[3]) * 0.01f;
                    Vbatt2 = ((temp[4] << 8) + temp[5]) * 0.01f;
                    //ODH_0电压
                    Odh0 = ((temp[7] << 8) + temp[6]) * 0.01f;
                    //VBATT1_SW电压
                    Vbatt1Sw = ((temp[9] << 8) + temp[8]) * 0.01f;
                    //PWR_0电压
                    Pwr0 = ((temp[11] << 8) + temp[10]) * 0.01f;
                    //VCC1_5V电压
                    Vcc1_5V = ((temp[13] << 8) + temp[12]) * 0.01f;
                    //VCC2_5V电压
                    Vcc2_5V = ((temp[15] << 8) + temp[14]) * 0.01f;
                    //SHF_FC_SW
                    ShtFcSw = GetBit(temp[0], 3) ? "1" : "0";
                    //NEUT_SW
                    NeutSw = GetBit(temp[0], 2) ? "1" : "0";
                    //PRIM_SW
                    PrimSw = GetBit(temp[0], 1) ? "1" : "0";
                    //Tertairy_Switch
                    TertairySwitch = GetBit(temp[0], 0) ? "1" : "0";
                    //CLS_HND_SW
                    ClsHndSw = GetBit(temp[0], 4) ? "1" : "0";
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
                //SHF_FC_SW
                ShtFcSw = string.Empty;
                //NEUT_SW
                NeutSw = string.Empty;
                //PRIM_SW
                PrimSw = string.Empty;
                //Tertairy_Switch
                TertairySwitch = string.Empty;
                //CLS_HND_SW
                ClsHndSw = string.Empty;
            }
        }

        [Description("解锁马达")]
        public void UnlockMotor(string delayMs)
        {
            Lock60 = -9999;
            Unlock60 = -9999;
            ChengGan60 = -9999;
            UnChengGan60 = -9999;

            if (Can == null)
                return;

            int delay;
            if (!int.TryParse(delayMs, out delay))
                delay = 2000;

            //EnterExtendedSession();
            //Thread.Sleep(100);
            //SecurityAccess0D0E();
            //Thread.Sleep(100);

            // ============锁电机60%占空比  上锁===================
            //if (Can.CanBusWithUds.TryInputOutputControl(
            //    CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId, CanBus.CanType.Extended, 0xFD, 0x00,
            //    Uds14229Helper.InputOutputControlParameter.ShortTermAdjustment, new byte[] { 0x01, 0x00, 0x99, 0x00, 0x13 }))
            if (IoControl(0xFD, 0x00, Uds14229Helper.InputOutputControlParameter.ShortTermAdjustment, new byte[] { 0x01, 0x00, 0x99, 0x00, 0x13 }))
            {
                Thread.Sleep(delay);
                var read1 = ReadDidViaCan(0x82, 0x32);
                if (read1 != null)
                {
                    try
                    {
                        Lock60 = ((read1[8] << 8) + read1[9]) * 0.1;
                    }
                    catch (Exception)
                    {
                        Lock60 = -9999;
                    }
                }
            }

            // ===========锁电机60%占空比  解锁==================
            //if (Can.CanBusWithUds.TryInputOutputControl(
            //    CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId, CanBus.CanType.Extended, 0xFD, 0x00,
            //    Uds14229Helper.InputOutputControlParameter.ShortTermAdjustment, new byte[] { 0x03, 0x00, 0x99, 0x00, 0x13 }))
            if (IoControl(0xFD, 0x00, Uds14229Helper.InputOutputControlParameter.ShortTermAdjustment, new byte[] { 0x03, 0x00, 0x99, 0x00, 0x13 }))
            {
                Thread.Sleep(delay);
                var read1 = ReadDidViaCan(0x82, 0x32);
                if (read1 != null)
                {
                    try
                    {
                        Unlock60 = ((read1[8] << 8) + read1[9]) * 0.1;
                    }
                    catch (Exception)
                    {
                        Unlock60 = -9999;
                    }
                }
            }

            // ===========撑杆电机60%占空比  上锁==================
            //if (Can.CanBusWithUds.TryInputOutputControl(
            //    CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId, CanBus.CanType.Extended, 0xFD, 0x00,
            //    Uds14229Helper.InputOutputControlParameter.ShortTermAdjustment, new byte[] { 0x01, 0x00, 0x00, 0x99, 0x25 }))
            if (IoControl(0xFD, 0x00, Uds14229Helper.InputOutputControlParameter.ShortTermAdjustment, new byte[] { 0x01, 0x00, 0x00, 0x99, 0x25 }))
            {
                Thread.Sleep(delay);
                var read1 = ReadDidViaCan(0x82, 0x32);
                if (read1 != null)
                {
                    try
                    {
                        ChengGan60 = ((read1[3] << 8) + read1[4]) * 0.1;
                    }
                    catch (Exception)
                    {
                        ChengGan60 = -9999;
                    }
                }
            }

            // ==============撑杆电机60%占空比  解锁===================
            //if (Can.CanBusWithUds.TryInputOutputControl(
            //    CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId, CanBus.CanType.Extended, 0xFD, 0x00,
            //    Uds14229Helper.InputOutputControlParameter.ShortTermAdjustment, new byte[] { 0x05, 0x00, 0x00, 0x99, 0x25 }))
            if (IoControl(0xFD, 0x00, Uds14229Helper.InputOutputControlParameter.ShortTermAdjustment, new byte[] { 0x05, 0x00, 0x00, 0x99, 0x25 }))
            {
                Thread.Sleep(delay);
                var read1 = ReadDidViaCan(0x82, 0x32);
                if (read1 != null)
                {
                    try
                    {
                        UnChengGan60 = ((read1[3] << 8) + read1[4]) * 0.1;
                    }
                    catch (Exception)
                    {
                        UnChengGan60 = -9999;
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

        [Description("读取霍尔传感器")]
        public void ReadHallSingal()
        {
            HallSingal = -9999;
            var responseData = ReadDidViaCan(0x82, 0x30);
            if (responseData == null)
                return;
            try
            {
                HallSingal = (responseData[17] << 8) + responseData[16];
            }
            catch (Exception)
            {
                HallSingal = -9999;
            }
        }

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

        #region 版本信息,与烧写器Flash写入相关

        [Description("R,ECU_ID-F0F3")]
        public string EcuId;

        [Description("R,DUNS-F0B3")]
        public string Duns;

        [Description("R,追溯信息MTC-F0B4")]
        public string TrackInfo;

        [Description("R,VPPS-F0AB")]
        public string Vpps;

        [Description("R,初始零件号-BaseModulePartNumber-F1CC")]
        public string BaseModulePartNumber;

        [Description("R,初始版本号-BaseModuleAlphaCode-F1DC")]
        public string BaseModuleAlphaCode;

        [Description("R,当前零件号-EndModulePartNumber-F1CB")]
        public string EndModulePartNumber;

        [Description("R,当前版本号-当前版本号-F1DB")]
        public string EndModuleAlphaCode;

        [Description("ReadF0F3和F0B4-ECU_ID和追溯信息-并追溯SQL数据库")]
        public void ReadEcuIdAndTrackInfoThenTrackSql()
        {
            EcuId = string.Empty;
            TrackInfo = string.Empty;

            var ecuId = ValueHelper.GetHextStr(ReadDidViaCan(0xF0, 0xF3)).Replace(" ", "");
            var trackInfo = ReadDidViaCan(0xF0, 0xB4).GetStringByAsciiBytes(true);

            var existSql = new StringBuilder();

            existSql.Append("select count(1) from manufactureSgm458PlgDatas");
            existSql.Append(" where EcuId=@EcuId and TackInfo=@TackInfo and IsUsage = 1");
            SqlParameter[] parameters =
            {
                new SqlParameter("@EcuId", SqlDbType.NVarChar, 100),
                new SqlParameter("@TackInfo", SqlDbType.NVarChar, 100)
            };

            parameters[0].Value = ecuId;
            parameters[1].Value = trackInfo;

            if (Exists(existSql.ToString(), parameters))
            {
                EcuId = "OK " + ecuId;
                TrackInfo = "OK " + trackInfo;
            }
            else
            {
                EcuId = "NG追溯失败 " + ecuId;
                TrackInfo = "NG追溯失败 " + trackInfo;
            }
        }

        [Description("ReadF0F3-ECU_ID")]
        public void ReadEcuId()
        {
            EcuId = string.Empty;
            var ecuId = ValueHelper.GetHextStr(ReadDidViaCan(0xF0, 0xF3)).Replace(" ", "");

            if (_ecuIdStructList.Any() && !string.IsNullOrEmpty(_ecuIdStructList[DebugEcuIdIndex].EucIdHex) && _ecuIdStructList[DebugEcuIdIndex].EucIdHex == ecuId)
                EcuId = "OK " + ecuId;
            else
                EcuId = "NG " + ecuId;
        }

        [Description("ReadF0B3-DUNS")]
        public void ReadDuns()
        {
            Duns = string.Empty;
            Duns = ReadDidViaCan(0xF0, 0xB3).GetStringByAsciiBytes(true);
        }

        [Description("ReadF0B4-追溯信息")]
        public void ReadTrackInfo()
        {
            TrackInfo = string.Empty;
            var trackInfo = ReadDidViaCan(0xF0, 0xB4).GetStringByAsciiBytes(true);

            if (_ecuIdStructList.Any() && !string.IsNullOrEmpty(_ecuIdStructList[DebugEcuIdIndex].TrackInfo) && _ecuIdStructList[DebugEcuIdIndex].TrackInfo == trackInfo)
                TrackInfo = "OK " + trackInfo;
            else
                TrackInfo = "NG " + trackInfo;
        }

        [Description("ReadF0AB-VPPS")]
        public void ReadVpps()
        {
            Vpps = string.Empty;
            Vpps = ReadDidViaCan(0xF0, 0xAB).GetStringByAsciiBytes(true);
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

        [Description("ReadF1CB-当前零件号-EndModulePartNumber")]
        public void ReadEndModulePartNumber()
        {
            EndModulePartNumber = string.Empty;
            EndModulePartNumber = ValueHelper.GetDecimal(ReadDidViaCan(0xF1, 0xCB).ToArray()).ToString();
        }

        [Description("ReadF1DB-当前版本号-EndModulePartNumber")]
        public void ReadEndModuleAlphaCode()
        {
            EndModuleAlphaCode = string.Empty;
            EndModuleAlphaCode = ReadDidViaCan(0xF1, 0xDB).GetStringByAsciiBytes(true);
        }

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
                    CanBus.CanType.Extended, CanBus.CanProtocol.Can,
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

        #endregion

        #region 版本信息,正常通过CAN写入或读取

        [Description("R,编程日期-F199")]
        public string ProgrammingDate;

        [Description("R,编程日期-Year-F199")]
        public int ProgrammingDateOfYear = -9999;

        [Description("R,编程日期-Month-F199")]
        public int ProgrammingDateOfMonth = -9999;

        [Description("R,编程日期-Day-F199")]
        public int ProgrammingDateOfDay = -9999;

        [Description("R,DDI-F091")]
        public string DiagnosticDataIdentifier;

        [Description("R,MEC-F1A0")]
        public int ManufacturersEnableCounter = -1;

        [Description("R,引导程序零件号-F1C0")]
        public string FblPartNo;

        [Description("R,应用程序零件号-F1C1")]
        public string AppPartNo;

        [Description("R,配置文件1零件号-F1C2")]
        public string Cali1PartNo;

        [Description("R,引导程序版本号-F1D0")]
        public string FblVer;

        [Description("R,应用程序版本号-F1D1")]
        public string AppVer;

        [Description("R,配置文件1版本号-F1D2")]
        public string Cali1Ver;

        [Description("R,编程状态-F0F0")]
        public string ProgrammedStateIndicator;

        [Description("R,ECU诊断地址-F1B0")]
        public string EcuDiagnosticAddress;

        [Description("R,JTAG密码")]
        public string JtagIdCode;

        [Description("R,读取JTAG密码并与需要写入烧写器中的值的比对结果")]
        public string ReadJtagIdCodeAndCompareResult;

        [Description("R,读取DID8234结果")]
        public string ReadDid8234Result = string.Empty;

        [Description("写入并读取刷新日期-F199")]
        public void WriteAndReadProgrammingDate()
        {
            ProgrammingDate = string.Empty;

            if (Can == null)
                return;

            string str;

            // 可以重复写入，无需用烧写器擦除
            // 写入前，需要先10 03，再27 01解锁

            if (_ecuIdStructList.Any() && !string.IsNullOrEmpty(_ecuIdStructList[DebugEcuIdIndex].TrackInfo))
            {
                var trackInfo = _ecuIdStructList[DebugEcuIdIndex].TrackInfo;
                try
                {
                    var year = "20" + trackInfo.Substring(2, 2);
                    var dayOfYear = trackInfo.Substring(4, 3);
                    var dateTimeStr = string.Format("{0}/01/01", year);
                    var dateTime = DateTime.Parse(dateTimeStr).AddDays(int.Parse(dayOfYear) - 1);
                    str = string.Format("{0}{1}{2}", dateTime.Year, dateTime.Month.ToString().PadLeft(2, '0'),
                       dateTime.Day.ToString().PadLeft(2, '0'));
                }
                catch (Exception)
                {
                    var dateTime = DateTime.Now;
                    str = string.Format("{0}{1}{2}", dateTime.Year, dateTime.Month.ToString().PadLeft(2, '0'),
                       dateTime.Day.ToString().PadLeft(2, '0'));
                }
            }
            else
            {
                var dateTime = DateTime.Now;

                str = string.Format("{0}{1}{2}", dateTime.Year, dateTime.Month.ToString().PadLeft(2, '0'),
                   dateTime.Day.ToString().PadLeft(2, '0'));
            }

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

            //if (!string.IsNullOrEmpty(read) && read == "00000000")
            //{
            //    lock (_lockSend)
            //    {
            //        SecurityAccess0102();
            //        Thread.Sleep(50);

            //        if (!Can.CanBusWithUds.TryWriteData(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
            //            CanBus.CanType.Extended, CanBus.CanProtocol.Can, 0xF1, 0x99, bs.ToArray()))
            //        {
            //            Thread.Sleep(50);
            //            Can.CanBusWithUds.TryWriteData(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
            //            CanBus.CanType.Extended, CanBus.CanProtocol.Can, 0xF1, 0x99, bs.ToArray());
            //        }
            //    }

            //    Thread.Sleep(100);
            //    read = ValueHelper.GetHextStr(ReadDidViaCan(0xF1, 0x99)).Replace(" ", "");
            //}

            if (read == str)
                ProgrammingDate = "OK " + read;
            else
                ProgrammingDate = "NG " + read;
        }

        [Description("写入并读取DDI-F09A")]
        public void WriteAndReadDiagnosticDataIdentifier(string ddiHex)
        {
            DiagnosticDataIdentifier = string.Empty;

            if (Can == null)
                return;

            var bs = new List<byte>();

            for (var i = 0; i < ddiHex.Length; i = i + 2)
                bs.Add(Convert.ToByte(ddiHex.Substring(i, 2)));

            lock (_lockSend)
            {
                Can.CanBusWithUds.TryWriteData(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                    CanBus.CanType.Extended, CanBus.CanProtocol.Can, 0xF0, 0x9A, bs.ToArray());
            }

            Thread.Sleep(20);
            DiagnosticDataIdentifier = ValueHelper.GetHextStr(ReadDidViaCan(0xF0, 0x9A)).Replace(" ", "");
        }

        [Description("写入并读取MEC-F1A0")]
        public void WriteAndReadManufacturersEnableCounter(string mec)
        {
            ManufacturersEnableCounter = -1;

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

        [Description("读取DDI-F09A")]
        public void ReadDiagnosticDataIdentifier()
        {
            DiagnosticDataIdentifier = string.Empty;

            if (Can == null)
                return;

            DiagnosticDataIdentifier = ValueHelper.GetHextStr(ReadDidViaCan(0xF0, 0x9A)).Replace(" ", "");
        }

        [Description("读取MEC-F1A0")]
        public void ReadManufacturersEnableCounter()
        {
            ManufacturersEnableCounter = -1;

            if (Can == null)
                return;

            ManufacturersEnableCounter = ValueHelper.GetDecimal(ReadDidViaCan(0xF1, 0xA0));
        }

        [Description("ReadF1C0-引导程序零件号")]
        public void ReadFblPartNo()
        {
            FblPartNo = string.Empty;
            FblPartNo = ValueHelper.GetDecimal(ReadDidViaCan(0xF1, 0xC0).ToArray()).ToString();
        }

        [Description("ReadF1C1-应用程序零件号")]
        public void ReadAppPartNo()
        {
            AppPartNo = string.Empty;
            AppPartNo = ValueHelper.GetDecimal(ReadDidViaCan(0xF1, 0xC1).ToArray()).ToString();
        }

        [Description("ReadF1C2-配置文件1零件号")]
        public void ReadCali1PartNo()
        {
            Cali1PartNo = string.Empty;
            Cali1PartNo = ValueHelper.GetDecimal(ReadDidViaCan(0xF1, 0xC2).ToArray()).ToString();
        }

        [Description("ReadF1D0-引导程序版本号")]
        public void ReadFblVer()
        {
            FblVer = string.Empty;
            FblVer = ReadDidViaCan(0xF1, 0xD0).GetStringByAsciiBytes(true);
        }

        [Description("ReadF1D1-应用程序版本号")]
        public void ReadAppVer()
        {
            AppVer = string.Empty;
            AppVer = ReadDidViaCan(0xF1, 0xD1).GetStringByAsciiBytes(true);
        }

        [Description("ReadF1D2-配置文件1版本号")]
        public void ReadCali1Ver()
        {
            Cali1Ver = string.Empty;
            Cali1Ver = ReadDidViaCan(0xF1, 0xD2).GetStringByAsciiBytes(true);
        }

        [Description("ReadF0F0-编程状态")]
        public void ReadProgrammedStateIndicator()
        {
            ProgrammedStateIndicator = string.Empty;
            ProgrammedStateIndicator = ValueHelper.GetHextStr(ReadDidViaCan(0xF0, 0xF0)).Replace(" ", "");
        }

        [Description("ReadF1B0-ECU诊断地")]
        public void ReadEcuDiagnosticAddress()
        {
            EcuDiagnosticAddress = string.Empty;
            EcuDiagnosticAddress = ValueHelper.GetHextStr(ReadDidViaCan(0xF1, 0xB0)).Replace(" ", "");
        }

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

        [Description("读取JTAG密码")]
        public void ReadJtagIdCode()
        {
            JtagIdCode = string.Empty;

            if (Can == null)
                return;
            lock (_lockSend)
            {
                byte[] echo;
                if (!Can.CanBusWithUds.TesterTryRequest(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                    new byte[] { 0x23, 0x14, 0xFF, 0x20, 0xF7, 0x80, 0x10 }, out echo, CanBus.CanType.Extended))
                    return;

                if (echo == null || echo.Length <= 3)
                    return;

                if (echo.Length != 17 || echo[0] != 0x63)
                    return;

                var recv = new byte[16];
                Array.Copy(echo, 1, recv, 0, 16);
                JtagIdCode = ValueHelper.GetHextStr(recv).Replace(" ", "");
            }
        }

        [Description("R,读取DFlash结果")]
        public string DFlashReadStr = string.Empty;

        [Description("读取DFlash结果")]
        public void ReadDFlash()
        {
            DFlashReadStr = string.Empty;

            if (Can == null)
                return;
            lock (_lockSend)
            {
                lock (_lockSend)
                {
                    var baseAddr = 0xFF200000;
                    var temp = string.Empty;
                    for (var i = 0; i < 1024; i++)
                    {
                        var b0 = (byte)(baseAddr >> 24);
                        var b1 = (byte)(baseAddr >> 16);
                        var b2 = (byte)(baseAddr >> 8);
                        var b3 = (byte)(baseAddr >> 0);

                        byte[] echo;
                        if (!Can.CanBusWithUds.TesterTryRequest(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                            new byte[] { 0x23, 0x14, b0, b1, b2, b3, 0x40 }, out echo, CanBus.CanType.Extended))
                            break;

                        if (echo == null || echo.Length <= 3)
                            break;

                        if (echo.Length != 65 || echo[0] != 0x63)
                            break;

                        var recv = new byte[64];
                        Array.Copy(echo, 1, recv, 0, 64);

                        temp += string.Format("{0:X}:{1}\r\n", baseAddr, ValueHelper.GetHextStr(recv));
                        baseAddr = baseAddr + 0x40;
                    }

                    DFlashReadStr = temp;
                }
            }
        }

        [Description("写DataFlash")]
        public void WriteDataFlash()
        {
            if (Can == null)
                return;

            lock (_lockSend)
            {
                byte[] echo;

                var sendTitle = new byte[] { 0x3D, 0x14 };
                var memmoryAddr = new byte[] { 0xFF, 0x20, 0x00, 0x00, };
                var datas = new byte[16];
                for (var i = 0; i < datas.Length; i++)
                {
                    datas[i] = 0x03;
                }
                var writeBytes = new List<byte>();
                writeBytes.AddRange(sendTitle);
                writeBytes.AddRange(memmoryAddr);
                writeBytes.Add((byte)datas.Length);
                writeBytes.AddRange(datas);

                if (!Can.CanBusWithUds.TesterTryRequest(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                    writeBytes.ToArray(), out echo, CanBus.CanType.Extended))
                {

                }
            }
        }

        [Description("读取JTAG密码并与需要写入烧写器中的值比对是否一致")]
        public void ReadJtagIdCodeAndCompare()
        {
            ReadJtagIdCodeAndCompareResult = string.Empty;

            if (Can == null)
                return;

            if (!_ecuIdStructList.Any())
            {
                ReadJtagIdCodeAndCompareResult = "NG 没有需要写入烧写器中的JTAG密码";
                return;
            }

            lock (_lockSend)
            {
                byte[] echo;
                if (!Can.CanBusWithUds.TesterTryRequest(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                    new byte[] { 0x23, 0x14, 0xFF, 0x20, 0xF7, 0x80, 0x10 }, out echo, CanBus.CanType.Extended))
                    return;

                if (echo == null || echo.Length <= 3)
                    return;

                if (echo.Length != 17 || echo[0] != 0x63)
                    return;

                var recv = new byte[16];
                Array.Copy(echo, 1, recv, 0, 16);
                var idCode = ValueHelper.GetHextStr(recv).Replace(" ", "");

                if (idCode == _ecuIdStructList[DebugEcuIdIndex].IdCode)
                {
                    ReadJtagIdCodeAndCompareResult = "OK " + idCode;
                }
                if (idCode == _ecuIdStructList[DebugEcuIdIndex].IdCode)
                {
                    ReadJtagIdCodeAndCompareResult = string.Format("NG 读取：{0}，需要写入：{1}", idCode,
                        _ecuIdStructList[DebugEcuIdIndex].IdCode);
                }
            }
        }

        [Description("读DID8234")]
        public void ReadDid8234()
        {
            ReadDid8234Result = string.Empty;
            ReadDid8234Result = ValueHelper.GetHextStr(ReadDidViaCan(0x82, 0x34).ToArray()).Replace(" ", "");
        }

        //[Description("R,读F081结果")]
        //public string ReadF081Str = string.Empty;

        //[Description("读F081")]
        //public void ReadF081()
        //{
        //    ReadF081Str = string.Empty;
        //    ReadF081Str = ValueHelper.GetHextStr(ReadDidViaCan(0xF0, 0x81)).Replace(" ", "");
        //}

        #endregion

        #region 烧写及随机数相关

        private static readonly Dictionary<string, string> EachWorkstationCurrentEcuId = new Dictionary<string, string>();

        //[Description("R/W,SGM-ECUIDs-SRecord文件")]
        //public string EcuidsConfigFilePath = @"C:\Projs\2022\瑞萨烧写器\20220810\测试文件\debug\SGM-ECUIDs-已解密.sx";

        [Description("R/W,SGM-ECUIDs-SRecord文件夹")]
        public string EcuidsConfigDirectory = @"C:\Projs\2022\瑞萨烧写器\PLG458资料\EUCID申请";

        //private static readonly object LockFile = new object();

        private readonly List<EcuIdStruct> _ecuIdStructList = new List<EcuIdStruct>();

        //[Description("从配置文件中读取EcuIds")]
        //public void ReadEcuIdsFromSRecord()
        //{
        //    lock (LockFile)
        //    {
        //        _ecuIdStructList.Clear();

        //        if (string.IsNullOrEmpty(EcuidsConfigFilePath))
        //            return;

        //        if (!File.Exists(EcuidsConfigFilePath))
        //            return;

        //        var lines = SRecordFileHelper.GetSRecordLineData(EcuidsConfigFilePath);
        //        var blocks = SRecordFileHelper.GetBlocks(lines);

        //        var listDatas = new List<byte>();

        //        foreach (var t in blocks.SelectMany(b => b))
        //            listDatas.AddRange(t.Data);

        //        if (listDatas.Count % 144 != 0)
        //            return;

        //        for (var i = 0; i < listDatas.Count; i = i + 144)
        //        {
        //            var ecuIdStruct = new EcuIdStruct();

        //            for (var j = 0; j < 144; j++)
        //            {
        //                if (j >= 0 && j < 16)
        //                    ecuIdStruct.EucIdHex += ValueHelper.GetHextStr(listDatas[i + j]);
        //                else if (j >= 16 && j < 32)
        //                    ecuIdStruct.MasterEcuKeyM1Hex += ValueHelper.GetHextStr(listDatas[i + j]);
        //                else if (j >= 32 && j < 64)
        //                    ecuIdStruct.MasterEcuKeyM2Hex += ValueHelper.GetHextStr(listDatas[i + j]);
        //                else if (j >= 64 && j < 80)
        //                    ecuIdStruct.MasterEcuKeyM3Hex += ValueHelper.GetHextStr(listDatas[i + j]);
        //                else if (j >= 80 && j < 96)
        //                    ecuIdStruct.UnlockEcuKeyM1Hex += ValueHelper.GetHextStr(listDatas[i + j]);
        //                else if (j >= 96 && j < 128)
        //                    ecuIdStruct.UnlockEcuKeyM2Hex += ValueHelper.GetHextStr(listDatas[i + j]);
        //                else if (j >= 128 && j < 144)
        //                    ecuIdStruct.UnlockEcuKeyM3Hex += ValueHelper.GetHextStr(listDatas[i + j]);
        //            }

        //            _ecuIdStructList.Add(ecuIdStruct);
        //        }
        //    }
        //}

        [Description("从配置文件夹读取EcuIds并上传至服务器")]
        public void ReadEcuIdsFromSRecodrAndUploadToServer()
        {
            try
            {
                if (string.IsNullOrEmpty(EcuidsConfigDirectory))
                    return;

                if (!Directory.Exists(EcuidsConfigDirectory))
                    Directory.CreateDirectory(EcuidsConfigDirectory);

                foreach (var f in Directory.GetFiles(EcuidsConfigDirectory))
                {
                    if (f.ToLower().EndsWith(".s19"))
                    {
                        var fileInfo = new FileInfo(f);

                        if (fileInfo.Name.ToLower().StartsWith("SGM-ECUIDs-".ToLower()))
                        {
                            var lines = SRecordFileHelper.GetSRecordLineData(f);
                            var blocks = SRecordFileHelper.GetBlocks(lines);

                            var listDatas = new List<byte>();

                            foreach (var t in blocks.SelectMany(b => b))
                                listDatas.AddRange(t.Data);

                            if (listDatas.Count % 144 != 0)
                                return;

                            for (var i = 0; i < listDatas.Count; i = i + 144)
                            {
                                var ecuIdStruct = new EcuIdStruct();

                                for (var j = 0; j < 144; j++)
                                {
                                    if (j >= 0 && j < 16)
                                        ecuIdStruct.EucIdHex += ValueHelper.GetHextStr(listDatas[i + j]);
                                    else if (j >= 16 && j < 32)
                                        ecuIdStruct.MasterEcuKeyM1Hex += ValueHelper.GetHextStr(listDatas[i + j]);
                                    else if (j >= 32 && j < 64)
                                        ecuIdStruct.MasterEcuKeyM2Hex += ValueHelper.GetHextStr(listDatas[i + j]);
                                    else if (j >= 64 && j < 80)
                                        ecuIdStruct.MasterEcuKeyM3Hex += ValueHelper.GetHextStr(listDatas[i + j]);
                                    else if (j >= 80 && j < 96)
                                        ecuIdStruct.UnlockEcuKeyM1Hex += ValueHelper.GetHextStr(listDatas[i + j]);
                                    else if (j >= 96 && j < 128)
                                        ecuIdStruct.UnlockEcuKeyM2Hex += ValueHelper.GetHextStr(listDatas[i + j]);
                                    else if (j >= 128 && j < 144)
                                        ecuIdStruct.UnlockEcuKeyM3Hex += ValueHelper.GetHextStr(listDatas[i + j]);
                                }

                                var existSql = new StringBuilder();

                                existSql.Append("select count(1) from manufactureSgm458PlgDatas");
                                existSql.Append(" where EcuId=@EcuId ");
                                SqlParameter[] parameters =
                                {
                                    new SqlParameter("@EcuId", SqlDbType.NVarChar, 100)
                                };

                                parameters[0].Value = ecuIdStruct.EucIdHex;

                                if (!Exists(existSql.ToString(), parameters))
                                {
                                    var insertSql = string.Format(
                                        "insert into manufactureSgm458PlgDatas " +
                                        "(EcuId,EcuIdFromFile,CreateTime,IsUsage,IsUploadToSgm," +
                                        "MASTER_ECU_KEY_KeySlot,MASTER_ECU_KEY_M1,MASTER_ECU_KEY_M2,MASTER_ECU_KEY_M3," +
                                        "UNLOCK_ECU_KEY_KeySlot,UNLOCK_ECU_KEY_M1,UNLOCK_ECU_KEY_M2,UNLOCK_ECU_KEY_M3) " +
                                        "values " +
                                        "('{0}','{1}','{2}','{3}','{4}'," +
                                        "'{5}','{6}','{7}','{8}'," +
                                        "'{9}','{10}','{11}','{12}')",
                                        ecuIdStruct.EucIdHex, fileInfo.Name, DateTime.Now, 0, 0,
                                        "FF", ecuIdStruct.MasterEcuKeyM1Hex, ecuIdStruct.MasterEcuKeyM2Hex,
                                        ecuIdStruct.MasterEcuKeyM3Hex,
                                        "01", ecuIdStruct.UnlockEcuKeyM1Hex, ecuIdStruct.UnlockEcuKeyM2Hex,
                                        ecuIdStruct.UnlockEcuKeyM3Hex);

                                    Query(insertSql);
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public string GeneratedEcuIdResult;
        public string GeneratedWriteIdResult;

        public string GeneratedWriteEcuIdHex;
        public string GeneratedWriteIdCodeHex;
        public string GeneratedWriteVersionHex;
        private const int DebugEcuIdIndex = 0;
        private static readonly object LockGenerate = new object();

        //[Description("R/W,需要写入的EUCID-HEX格式")]
        //public string ToWriteEucIdHex;

        //[Description("R/W,需要写入的IDCode-HEX格式")]
        //public string ToWriteIdCodeHex;

        [Description("R/W,需要写入的DUNS-ASCII格式")]
        public string ToWriteDunsAscii = "547656863";

        //[Description("R/W,需要写入的追溯信息-ASCII格式")]
        //public string ToWriteTrackInfoAscii = "1A22221AAA000002";

        [Description("R/W,需要写入的CVPPS-ASCII格式")]
        public string ToWriteVppsAscii = "8210700000000X";

        [Description("R/W,需要写入的初始零件号-Dec格式")]
        public string ToWriteBaseModulePartNumberDec = "26369850";

        [Description("R/W,需要写入的初始版本号-ASCII格式")]
        public string ToWriteBaseModuleAlphaCodeAscii = "AA";

        [Description("R/W,需要写入的当前零件号-Dec格式")]
        public string ToWriteEndModulePartNumberDec = "26439047";

        [Description("R/W,需要写入的当前版本号-ASCII格式")]
        public string ToWriteEndModuleAlphaCodeAscii = "AA";

        [Description("R/W,需要写入的Seed")]
        public string ToWriteSeed = string.Empty;

        [Description("生成ECU-ID-通过系统随机数-通过系统随机数")]
        public void GenerateEcuIdAndIdCode()
        {
            var randomList = new List<byte>();
            for (var i = 0; i < 32; i++)
            {
                var randomByte = (byte)new Random(Guid.NewGuid().GetHashCode()).Next(0, 256);
                randomList.Add(randomByte);
            }
            //GenerateEcuIdAndIdCodeInternal(Encoding.ASCII.GetString(randomList.ToArray()));
            GenerateEcuIdAndIdCodeInternal(ValueHelper.GetHextStr(randomList.ToArray()).Replace(" ", ""));
        }

        [Description("生成ECU-ID-通过系统随机数-通过外部随机数")]
        public void GenerateEcuIdAndIdCodeViaInputSeed()
        {
            GenerateEcuIdAndIdCodeInternal(ToWriteSeed);
            ToWriteSeed = string.Empty;
        }

        /// <summary>
        /// 生产ECU-ID
        /// </summary>
        /// <param name="seedStr"></param>
        private void GenerateEcuIdAndIdCodeInternal(string seedStr)
        {
            lock (LockGenerate)
            {
                GeneratedEcuIdResult = "NG";
                GeneratedWriteIdResult = "NG";

                GeneratedWriteEcuIdHex = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF";
                GeneratedWriteIdCodeHex = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF";
                _ecuIdStructList.Clear();

                if (string.IsNullOrEmpty(seedStr))
                    return;

                if (seedStr.Length != 64)
                    return;

                var isSleep = _isSleep;
                if (isSleep)
                {
                    Awake();
                    //Thread.Sleep(2000);
                }

                IsReadCanMsg = true;
                var isHaveCanMsg = _readCanMsgWaitHandle.WaitOne(2000);
                IsReadCanMsg = false;

                Sleep();
                Thread.Sleep(50);

                var readEcuIdFromProduct = ValueHelper.GetHextStr(ReadDidViaCan(0xF0, 0xF3)).Replace(" ", "");

                //readEcuIdFromProduct = "0000038780002E001A104974210114A6";

                var selectSql = string.Empty;

                var isReadEcuIdOk = !string.IsNullOrEmpty(readEcuIdFromProduct) &&
                    readEcuIdFromProduct.Length == "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF".Length;

                if (isHaveCanMsg && isReadEcuIdOk)
                {
                    selectSql =
                        "SELECT TOP 1 [id],[EcuId],[EcuIdFromFile],[TackInfo],[CreateTime],[LastRecordTime],[IsUsage],[IsUploadToSgm],[RandomSeed],[Sha256Total],[Sha256Sub20],[IdCodeAscii],[IdCode],[MASTER_ECU_KEY_KeySlot],[MASTER_ECU_KEY_M1],[MASTER_ECU_KEY_M2],[MASTER_ECU_KEY_M3],[MASTER_ECU_KEY_M4],[MASTER_ECU_KEY_M5],[UNLOCK_ECU_KEY_KeySlot],[UNLOCK_ECU_KEY_M1],[UNLOCK_ECU_KEY_M2],[UNLOCK_ECU_KEY_M3],[UNLOCK_ECU_KEY_M4],[UNLOCK_ECU_KEY_M5],[MacKey] FROM [IPMS].[dbo].[manufactureSgm458PlgDatas] where [IsUsage] = '2' and [EcuId] = '" +
                        readEcuIdFromProduct + "'";

                    //var ds = Query(selectSql);
                    //if (ds.Tables[0].DefaultView.Count == 0) // 根据读到的ECU-ID没有在SQL中查询到，则采用获取一个新的
                    //{
                    //    selectSql =
                    //        "SELECT TOP 1 [id],[EcuId],[EcuIdFromFile],[TackInfo],[CreateTime],[LastRecordTime],[IsUsage],[IsUploadToSgm],[RandomSeed],[Sha256Total],[Sha256Sub20],[IdCodeAscii],[IdCode],[MASTER_ECU_KEY_KeySlot],[MASTER_ECU_KEY_M1],[MASTER_ECU_KEY_M2],[MASTER_ECU_KEY_M3],[MASTER_ECU_KEY_M4],[MASTER_ECU_KEY_M5],[UNLOCK_ECU_KEY_KeySlot],[UNLOCK_ECU_KEY_M1],[UNLOCK_ECU_KEY_M2],[UNLOCK_ECU_KEY_M3],[UNLOCK_ECU_KEY_M4],[UNLOCK_ECU_KEY_M5],[MacKey] FROM [IPMS].[dbo].[manufactureSgm458PlgDatas] where [IsUsage] = '0'";
                    //}
                }
                else if (!isHaveCanMsg && !isReadEcuIdOk)
                {
                    selectSql =
                        "SELECT TOP 1 [id],[EcuId],[EcuIdFromFile],[TackInfo],[CreateTime],[LastRecordTime],[IsUsage],[IsUploadToSgm],[RandomSeed],[Sha256Total],[Sha256Sub20],[IdCodeAscii],[IdCode],[MASTER_ECU_KEY_KeySlot],[MASTER_ECU_KEY_M1],[MASTER_ECU_KEY_M2],[MASTER_ECU_KEY_M3],[MASTER_ECU_KEY_M4],[MASTER_ECU_KEY_M5],[UNLOCK_ECU_KEY_KeySlot],[UNLOCK_ECU_KEY_M1],[UNLOCK_ECU_KEY_M2],[UNLOCK_ECU_KEY_M3],[UNLOCK_ECU_KEY_M4],[UNLOCK_ECU_KEY_M5],[MacKey] FROM [IPMS].[dbo].[manufactureSgm458PlgDatas] where [IsUsage] = '0'";
                }

                //if (!string.IsNullOrEmpty(readEcuIdFromProduct) &&
                //    readEcuIdFromProduct.Length == "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF".Length &&
                //    readEcuIdFromProduct != "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF" &&
                //    readEcuIdFromProduct != "00000000000000000000000000000000")
                //{
                //    selectSql =
                //        "SELECT TOP 1 [id],[EcuId],[EcuIdFromFile],[TackInfo],[CreateTime],[LastRecordTime],[IsUsage],[IsUploadToSgm],[RandomSeed],[Sha256Total],[Sha256Sub20],[IdCodeAscii],[IdCode],[MASTER_ECU_KEY_KeySlot],[MASTER_ECU_KEY_M1],[MASTER_ECU_KEY_M2],[MASTER_ECU_KEY_M3],[MASTER_ECU_KEY_M4],[MASTER_ECU_KEY_M5],[UNLOCK_ECU_KEY_KeySlot],[UNLOCK_ECU_KEY_M1],[UNLOCK_ECU_KEY_M2],[UNLOCK_ECU_KEY_M3],[UNLOCK_ECU_KEY_M4],[UNLOCK_ECU_KEY_M5],[MacKey] FROM [IPMS].[dbo].[manufactureSgm458PlgDatas] where [IsUsage] = '2' and [EcuId] = '" +
                //        readEcuIdFromProduct + "'";
                //}
                //else
                //{
                //    selectSql =
                //        "SELECT TOP 1 [id],[EcuId],[EcuIdFromFile],[TackInfo],[CreateTime],[LastRecordTime],[IsUsage],[IsUploadToSgm],[RandomSeed],[Sha256Total],[Sha256Sub20],[IdCodeAscii],[IdCode],[MASTER_ECU_KEY_KeySlot],[MASTER_ECU_KEY_M1],[MASTER_ECU_KEY_M2],[MASTER_ECU_KEY_M3],[MASTER_ECU_KEY_M4],[MASTER_ECU_KEY_M5],[UNLOCK_ECU_KEY_KeySlot],[UNLOCK_ECU_KEY_M1],[UNLOCK_ECU_KEY_M2],[UNLOCK_ECU_KEY_M3],[UNLOCK_ECU_KEY_M4],[UNLOCK_ECU_KEY_M5],[MacKey] FROM [IPMS].[dbo].[manufactureSgm458PlgDatas] where [IsUsage] = '0'";
                //}

                if (!string.IsNullOrEmpty(selectSql))
                {
                    var ds = Query(selectSql);
                    if (ds.Tables[0].DefaultView.Count == 1)
                    {
                        var ecuIdStruct = new EcuIdStruct
                        {
                            EucIdHex = ds.Tables[0].DefaultView[0]["EcuId"].ToString(),
                            MasterEcuKeySlot = ds.Tables[0].DefaultView[0]["MASTER_ECU_KEY_KeySlot"].ToString(),
                            MasterEcuKeyM1Hex = ds.Tables[0].DefaultView[0]["MASTER_ECU_KEY_M1"].ToString(),
                            MasterEcuKeyM2Hex = ds.Tables[0].DefaultView[0]["MASTER_ECU_KEY_M2"].ToString(),
                            MasterEcuKeyM3Hex = ds.Tables[0].DefaultView[0]["MASTER_ECU_KEY_M3"].ToString(),
                            MasterEcuKeyM4Hex = ds.Tables[0].DefaultView[0]["MASTER_ECU_KEY_M4"] == null ? string.Empty : ds.Tables[0].DefaultView[0]["MASTER_ECU_KEY_M4"].ToString(),
                            MasterEcuKeyM5Hex = ds.Tables[0].DefaultView[0]["MASTER_ECU_KEY_M5"] == null ? string.Empty : ds.Tables[0].DefaultView[0]["MASTER_ECU_KEY_M5"].ToString(),
                            UnlockEcuKeySlot = ds.Tables[0].DefaultView[0]["UNLOCK_ECU_KEY_KeySlot"].ToString(),
                            UnlockEcuKeyM1Hex = ds.Tables[0].DefaultView[0]["UNLOCK_ECU_KEY_M1"].ToString(),
                            UnlockEcuKeyM2Hex = ds.Tables[0].DefaultView[0]["UNLOCK_ECU_KEY_M2"].ToString(),
                            UnlockEcuKeyM3Hex = ds.Tables[0].DefaultView[0]["UNLOCK_ECU_KEY_M3"].ToString(),
                            UnlockEcuKeyM4Hex = ds.Tables[0].DefaultView[0]["UNLOCK_ECU_KEY_M4"] == null ? string.Empty : ds.Tables[0].DefaultView[0]["UNLOCK_ECU_KEY_M4"].ToString(),
                            UnlockEcuKeyM5Hex = ds.Tables[0].DefaultView[0]["UNLOCK_ECU_KEY_M5"] == null ? string.Empty : ds.Tables[0].DefaultView[0]["UNLOCK_ECU_KEY_M5"].ToString(),
                            MacKeyInfo = ds.Tables[0].DefaultView[0]["MacKey"] == null ? string.Empty : ds.Tables[0].DefaultView[0]["MacKey"].ToString(),
                            RandomSeed = ds.Tables[0].DefaultView[0]["RandomSeed"] == null ? string.Empty : ds.Tables[0].DefaultView[0]["RandomSeed"].ToString(),
                            Sha256Total = ds.Tables[0].DefaultView[0]["Sha256Total"] == null ? string.Empty : ds.Tables[0].DefaultView[0]["Sha256Total"].ToString(),
                            Sha256Sub20 = ds.Tables[0].DefaultView[0]["Sha256Sub20"] == null ? string.Empty : ds.Tables[0].DefaultView[0]["Sha256Sub20"].ToString(),
                            IdCodeAscii = ds.Tables[0].DefaultView[0]["IdCodeAscii"] == null ? string.Empty : ds.Tables[0].DefaultView[0]["IdCodeAscii"].ToString(),
                            IdCode = ds.Tables[0].DefaultView[0]["IdCode"] == null ? string.Empty : ds.Tables[0].DefaultView[0]["IdCode"].ToString(),
                            TrackInfo = ds.Tables[0].DefaultView[0]["IdCode"] == null ? string.Empty : ds.Tables[0].DefaultView[0]["TackInfo"].ToString(),
                        };

                        var path = Directory.GetCurrentDirectory() + @"\DllImport\Pbkdf2_sha256.dll";
                        //var strSeed = seedStr; // Encoding.ASCII.GetString(randomList.ToArray()); //以下strSeed即周晓杨提供的32byte随机数输入

                        //string strMaster32, strMaster20, strPBKDF2;
                        //byte[] bytePBKDF2;

                        //注意：由于字符显示的问题，用网上的sha256小软件验证下面GetSHA256方法结果是否正确，网上小软件的输入要从strSeed里面的窗口拷贝，不要拷贝下面Console.WriteLine输出的窗口strSeed内容
                        var rt = EcuPasswordDll.GetSha256(seedStr, path);
                        var strMaster32 = BitConverter.ToString(rt).Replace("-", "");//经过SHA256算法得到的(32Byte)
                        var strMaster20 = BitConverter.ToString(rt).Replace("-", "").Substring(0, 40);//经过SHA256算法得到的(前20Byte)
                        //Console.WriteLine("sha256的32byte： " + strMaster32+"/r/n"
                        //            + "  前20字节：" + strMaster20);
                        //strMaster32 = Encoding.ASCII.GetString(ECUPasswordDll.GetSHA256(strSeed, path));//经过SHA256算法得到的(32Byte)
                        //strMaster20 = Encoding.ASCII.GetString(ECUPasswordDll.GetSHA256(strSeed, path).Take(20).ToArray());//经过SHA256算法得到的(前20Byte)
                        var strPbkdf2 = EcuPasswordDll.GetEcuPassword(strMaster20, ecuIdStruct.EucIdHex, 16, path); //经过PBKDF2算法得到的(字符串)
                        if (string.IsNullOrEmpty(strPbkdf2) || strPbkdf2.Length != 16)
                        {
                            var temp = new List<byte>();
                            for (var i = 0; i < seedStr.Length; i = i + 2)
                            {
                                temp.Add(Convert.ToByte(seedStr.Substring(i, 2), 16));
                            }
                            seedStr = ValueHelper.GetHextStr(temp.ToArray().Reverse().ToArray()).Replace(" ", "");

                            rt = EcuPasswordDll.GetSha256(seedStr, path);
                            strMaster32 = BitConverter.ToString(rt).Replace("-", "");//经过SHA256算法得到的(32Byte)
                            strMaster20 = BitConverter.ToString(rt).Replace("-", "").Substring(0, 40);//经过SHA256算法得到的(前20Byte)

                            strPbkdf2 = EcuPasswordDll.GetEcuPassword(strMaster20, ecuIdStruct.EucIdHex, 16, path);
                        }
                        var bytePbkdf2 = Encoding.ASCII.GetBytes(strPbkdf2);//经过PBKDF2算法得到的(byte)
                        var outBytePbkdf2 = BitConverter.ToString(bytePbkdf2).Replace("-", "");
                        var strEcuid = ecuIdStruct.EucIdHex;

                        ecuIdStruct.IdCode = outBytePbkdf2;
                        ecuIdStruct.IdCodeAscii = strPbkdf2;

                        var randomList = new List<byte>();
                        for (var i = 0; i < seedStr.Length; i = i + 2)
                        {
                            randomList.Add(Convert.ToByte(seedStr.Substring(i, 2), 16));
                        }
                        ecuIdStruct.RandomSeed = BitConverter.ToString(randomList.ToArray());

                        ecuIdStruct.Sha256Total = strMaster32;
                        ecuIdStruct.Sha256Sub20 = strMaster20;

                        if (strEcuid != "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF" &&
                            outBytePbkdf2 != "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF" &&
                            !string.IsNullOrEmpty(strEcuid) &&
                            !string.IsNullOrEmpty(outBytePbkdf2) &&
                            strEcuid.Length == 32 &&
                            outBytePbkdf2.Length == 32 &&
                            ValueHelper.GetHextStr(rt).Replace(" ", "") != ValueHelper.GetHextStr(new byte[32]).Replace(" ", ""))
                        {
                            GeneratedWriteEcuIdHex = strEcuid;
                            GeneratedWriteIdCodeHex = outBytePbkdf2;

                            GeneratedEcuIdResult = "OK " + strEcuid;
                            GeneratedWriteIdResult = "OK " + outBytePbkdf2;

                            _ecuIdStructList.Add(ecuIdStruct);

                            LockEcuId(ecuIdStruct, 2.ToString());
                            //LockUnlockEcuId(strEcuid, 2.ToString());
                        }
                    }
                }

                _isSleep = isSleep;
            }
        }

        [Description("生成版本信息")]
        public void GenerateVersion(string productNo)
        {
            GeneratedWriteVersionHex = string.Empty;
            for (var i = 0; i < 51; i++)
                GeneratedWriteVersionHex += "FF";

            var sqlConnectiong = string.Format(@"server={0};database={1};uid={2};pwd={3}", ServerIp, ServerDataBase,
               ServerUid, ServerPwd);

            //sqlConnectiong=@"server=192.168.1.150;database=IPMS;uid=sa;pwd=123456";

            using (var conn = new SqlConnection(sqlConnectiong))
            {
                try
                {
                    var cmd = new SqlCommand("GetCheckNo", conn) { CommandType = CommandType.StoredProcedure };
                    cmd.Parameters.AddWithValue("@Productno", productNo);  //给输入参数赋值
                    var parOutputDate = cmd.Parameters.Add("@date", SqlDbType.DateTime);  //定义输出参数 
                    parOutputDate.Direction = ParameterDirection.Output;  //参数类型为Output 
                    var returnSerialNo = cmd.Parameters.Add("@return_value", SqlDbType.Int);
                    returnSerialNo.Direction = ParameterDirection.ReturnValue;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    var serialNo = returnSerialNo.Value.ToString();
                    var date = parOutputDate.Value.ToString();

                    var toWriteTrackInfoAscii =
                        string.Format("1A{0}{1}AAA00{2}", DateTime.Parse(date).Year.ToString().Substring(2, 2), DateTime.Parse(date).DayOfYear.ToString().PadLeft(3, '0'), serialNo.PadLeft(4, '0')); // 1A22221AAA00000
                    if (_ecuIdStructList.Any())
                    {
                        _ecuIdStructList[DebugEcuIdIndex].TrackInfo = toWriteTrackInfoAscii;
                        UpdateEcuIdStruct(_ecuIdStructList[DebugEcuIdIndex]);
                    }

                    var tempStr = string.Empty;
                    tempStr += ValueHelper.GetHextStr(Encoding.ASCII.GetBytes(ToWriteDunsAscii));
                    tempStr += ValueHelper.GetHextStr(Encoding.ASCII.GetBytes(toWriteTrackInfoAscii));
                    tempStr += ValueHelper.GetHextStr(Encoding.ASCII.GetBytes(ToWriteVppsAscii));

                    tempStr +=
                        ValueHelper.GetHextStr(
                            BitConverter.GetBytes(uint.Parse(ToWriteBaseModulePartNumberDec)).Reverse().ToArray());
                    tempStr += ValueHelper.GetHextStr(Encoding.ASCII.GetBytes(ToWriteBaseModuleAlphaCodeAscii));

                    tempStr +=
                        ValueHelper.GetHextStr(
                            BitConverter.GetBytes(uint.Parse(ToWriteEndModulePartNumberDec)).Reverse().ToArray());
                    tempStr += ValueHelper.GetHextStr(Encoding.ASCII.GetBytes(ToWriteEndModuleAlphaCodeAscii));

                    GeneratedWriteVersionHex = tempStr.Replace(" ", "");
                }
                catch (Exception)
                {
                    GeneratedWriteVersionHex = string.Empty;
                    for (var i = 0; i < 51; i++)
                        GeneratedWriteVersionHex += "FF";
                }
            }
        }

        /// <summary>
        /// 保存数据到txt
        /// </summary>
        private static void SaveData(
            string radomListStr,
            string strMaster32, string strMaster20, string strEcuid, string strPbkdf2, string outBytePbkdf2,
            string masterEcuKeyKeyM1Hex, string masterEcuKeyKeyM2Hex, string masterEcuKeyKeyM3Hex,
            string unlockEcuKeyKeyM1Hex, string unlockEcuKeyKeyM2Hex, string unlockEcuKeyKeyM3Hex)
        {
            var newPath = Path.Combine(Environment.CurrentDirectory, "DataRecord\\");
            if (!Directory.Exists(newPath))
                Directory.CreateDirectory(newPath);
            //string newChildPath = System.IO.Path.Combine(@newPath, type);
            //System.IO.Directory.CreateDirectory(newChildPath);
            //指定日志文件的目录
            var fname = newPath + "SGM458PLG-" + DateTime.Now.ToString("yyyyMMddHH") + ".txt";
            //定义文件信息对象
            var finfo = new FileInfo(fname);
            //创建只写文件流
            using (var fs = finfo.OpenWrite())
            {
                //根据上面创建的文件流创建写数据流
                var w = new StreamWriter(fs);

                //设置写数据流的起始位置为文件流的末尾
                w.BaseStream.Seek(0, SeekOrigin.End);

                //w.Write("\nLog Entry : ");

                var strBuild = new StringBuilder();
                strBuild.Append("=====================\r\n");
                strBuild.Append(string.Format("{0} {1} \r\n", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString()));
                strBuild.Append(string.Format("最后完整输出，即32位随机数种子: {0}\r\n", radomListStr));

                strBuild.Append(string.Format("SHA256(32byte): {0}\r\n", strMaster32));
                strBuild.Append(string.Format("SHA256(前20byte): {0}\r\n", strMaster20));

                strBuild.Append(string.Format("ECUID: {0}\r\n", strEcuid));

                strBuild.Append(string.Format("PBKDF2(字符串,即IDCode的ASCII格式): {0}\r\n", strPbkdf2));
                strBuild.Append(string.Format("PBKDF2(byte,即IDCode的HEX格式): {0}\r\n", outBytePbkdf2));

                strBuild.Append(string.Format("MASTER_ECU_KEY_M1: {0}\r\n", masterEcuKeyKeyM1Hex));
                strBuild.Append(string.Format("MASTER_ECU_KEY_M2: {0}\r\n", masterEcuKeyKeyM2Hex));
                strBuild.Append(string.Format("MASTER_ECU_KEY_M3: {0}\r\n", masterEcuKeyKeyM3Hex));

                strBuild.Append(string.Format("UNLOCK_ECU_KEY_M1: {0}\r\n", unlockEcuKeyKeyM1Hex));
                strBuild.Append(string.Format("UNLOCK_ECU_KEY_M2: {0}\r\n", unlockEcuKeyKeyM2Hex));
                strBuild.Append(string.Format("UNLOCK_ECU_KEY_M3: {0}\r\n", unlockEcuKeyKeyM3Hex));

                strBuild.Append("=====================\r\n");
                strBuild.Append("\r\n");

                w.Write(strBuild.ToString());

                //写入当前系统时间并换行
                //w.Write("//{0} {1} \r\n", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());

                //w.Write("最后完整输出: " + radomListStr + "\r\n" +
                //        "SHA256(32byte): " + strMaster32 + "\r\n"
                //        + "SHA256(前20byte): " + strMaster20 + "\r\n"
                //        + "ECUID: " + strEcuid + "\r\n"
                //        + "PBKDF2(字符串): " + strPbkdf2 + "\r\n"
                //        + "PBKDF2(byte): " + outBytePbkdf2);

                //写入------------------------------------“并换行
                //w.Write("\r\n////\r\n");

                //清空缓冲区内容，并把缓冲区内容写入基础流
                w.Flush();

                //关闭写数据流
                w.Close();
            }
        }

        public static class EcuPasswordDll
        {
            #region 调用申明需要

            public static IntPtr DllHandle = IntPtr.Zero;

            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern IntPtr LoadLibrary(string name);

            [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
            private static extern IntPtr GetProcAddress(IntPtr hModule, string name);
            #endregion

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate IntPtr ReturnEcuPassword(StringBuilder masterPassword, StringBuilder serialNumber, int len);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate IntPtr ReturnSha256(StringBuilder seed);

            public static byte[] GetSha256(string seed, string path)
            {
                byte[] str;
                try
                {
                    if (!File.Exists(path))
                        return null;

                    DllHandle = LoadLibrary(path);
                    if (DllHandle == IntPtr.Zero)
                        throw new Win32Exception();
                    var addr = GetProcAddress(DllHandle, "ReturnSHA256");
                    if (addr == IntPtr.Zero) throw new Win32Exception();
                    var func = (ReturnSha256)Marshal.GetDelegateForFunctionPointer(addr, typeof(ReturnSha256));
                    var dataPtr = func(new StringBuilder(seed));
                    str = new byte[32];
                    Marshal.Copy(dataPtr, str, 0, 32);
                    //Marshal.FreeHGlobal(dataPtr);
                }
                catch (Exception err)
                {
                    Console.WriteLine(err.Message);
                    str = new byte[32];
                }
                return str;
            }

            public static string GetEcuPassword(string masterPassword, string serialNumber, int len, string path)
            {
                string ecuPassword;
                try
                {
                    if (!File.Exists(path))
                    {
                        return string.Empty;
                    }

                    DllHandle = LoadLibrary(path);
                    if (DllHandle == IntPtr.Zero)
                        throw new Win32Exception();
                    var addr = GetProcAddress(DllHandle, "ReturnECUPassword");
                    if (addr == IntPtr.Zero) throw new Win32Exception();
                    var func = (ReturnEcuPassword)Marshal.GetDelegateForFunctionPointer(addr, typeof(ReturnEcuPassword));
                    var dataPtr = func(new StringBuilder(masterPassword),
                        new StringBuilder(serialNumber)
                        , len);
                    ecuPassword = Marshal.PtrToStringAnsi(dataPtr);
                    if (ecuPassword != null) ecuPassword = ecuPassword.Substring(0, len);
                    //Marshal.FreeHGlobal(dataPtr);
                }
                catch (Exception err)
                {
                    Console.WriteLine(err.Message);
                    ecuPassword = string.Empty;
                }
                return ecuPassword;
            }
        }

        internal class EcuIdStruct
        {
            public string EucIdHex;

            public string MasterEcuKeySlot;

            public string MasterEcuKeyM1Hex;
            public string MasterEcuKeyM2Hex;
            public string MasterEcuKeyM3Hex;
            public string MasterEcuKeyM4Hex;
            public string MasterEcuKeyM5Hex;

            public string UnlockEcuKeySlot;

            public string UnlockEcuKeyM1Hex;
            public string UnlockEcuKeyM2Hex;
            public string UnlockEcuKeyM3Hex;
            public string UnlockEcuKeyM4Hex;
            public string UnlockEcuKeyM5Hex;

            public string MacKeyInfo;

            public string RandomSeed;

            /// <summary>
            /// 经过SHA256算法得到的(32Byte)
            /// </summary>
            public string Sha256Total;

            /// <summary>
            /// 经过SHA256算法得到的(前20Byte)
            /// </summary>
            public string Sha256Sub20;

            public string IdCodeAscii;
            public string IdCode;

            public string TrackInfo;
        }

        //[Description("通过2701获取seed")]
        //public void RequestSeed01()
        //{
        //    if (Can == null)
        //        return;

        //    lock (_lockSend)
        //    {
        //        byte[] seedBytes;
        //        if (!Can.CanBusWithUds.TryRequestSeed(
        //            CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
        //            CanBus.CanType.Extended, 0x01, out seedBytes))
        //            return;

        //        if (seedBytes == null)
        //            return;

        //        Console.WriteLine("输出1：" + BitConverter.ToString(seedBytes));

        //        var list = new List<byte>();
        //        list.AddRange(seedBytes);
        //        list.RemoveRange(0, 14);
        //        list.RemoveRange(2, 1);
        //        list.RemoveRange(9, 1);
        //        //byte[] interSeedBytes = new byte[seedBytes.Length];
        //        //Array.Copy(seedBytes, interSeedBytes, seedBytes.Length);
        //        //Array.Clear(interSeedBytes, 0, 14);
        //        //Array.Clear(interSeedBytes, 2, 1);
        //        //Array.Clear(interSeedBytes, 9, 1);
        //        //Array.Clear(interSeedBytes, 15, 1);
        //        //randomList.AddRange(list);
        //        //var keyBytes = BodyCanCalcKey(seedBytes.ToArray(), 1).ToArray();
        //        Console.WriteLine("输出2：" + BitConverter.ToString(list.ToArray()));
        //        //Console.WriteLine("输出："+ BitConverter.ToString(seedBytes));
        //        //BodyCan.CanBusWithUds.TrySendKey(
        //        //    BodyCanDiagnosisRequestPhyCanId, BodyCanDiagnosisResponseCanId,
        //        //    CanBus.CanType.Standard, 0x02, keyBytes);
        //    }
        //}

        #endregion

        #region 安全信息

        //[Description("R,SeedKey注入结果")]
        //public string GeneralKey = string.Empty;

        [Description("R,安全访问0D0E结果")]
        public string SecurityAccessResult = string.Empty;

        [Description("R,Step1-MasterKey注入结果")]
        public string MasterKey = string.Empty;

        [Description("R,Step2-UnlockKey注入结果")]
        public string UnlockKey = string.Empty;

        [Description("R,Step3-MacKey注入结果")]
        public string MacKey = string.Empty;

        //[Description("检查SeedKey注入")]
        //public void CheckGeneralKey()
        //{
        //    GeneralKey = string.Empty;

        //    //读取F080
        //    byte[] responseData;
        //    Can.CanBusWithUds.TryReadData(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId, CanBus.CanType.Extended, CanBus.CanProtocol.Can, 0xF0, 0x80, out responseData);
        //    if (responseData == null || responseData[0] != 0x00)
        //    {
        //        GeneralKey = "NG";
        //        return;
        //    }

        //    //读取F081
        //    Can.CanBusWithUds.TryReadData(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId, CanBus.CanType.Extended, CanBus.CanProtocol.Can, 0xF0, 0x81, out responseData);
        //    var temp = ValueHelper.GetHextStr(responseData.Skip(3).ToArray()).Replace(" ", "");
        //    if (temp != "FFFFE1")
        //    {
        //        GeneralKey = "NG";
        //        return;
        //    }

        //    GeneralKey = "OK";
        //}

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

        [Description("Step1-MasterKey")]
        public void Step1MasterKey()
        {
            MasterKey = string.Empty;

            if (!_ecuIdStructList.Any())
            {
                MasterKey = "NG";
                return;
            }

            //_ecuIdStructList[DebugEcuIdIndex].MasterEcuKeyM4Hex = "005700600A2031352D38375539334E11CEFDF029C2C9BB4E7275F3D526E54345";
            //_ecuIdStructList[DebugEcuIdIndex].MasterEcuKeyM5Hex = "2EC6A334E9FC8B07948A5FAC15CD59BC";
            //UpdateEcuIdStruct(_ecuIdStructList[DebugEcuIdIndex]);
            //return;

            const byte keySlot = 0xFF;

            var m1 = new List<byte>();
            var m2 = new List<byte>();
            var m3 = new List<byte>();

            for (var i = 0; i < _ecuIdStructList[DebugEcuIdIndex].MasterEcuKeyM1Hex.Length; i = i + 2)
                m1.Add(
                        Convert.ToByte(string.Format("{0}{1}", _ecuIdStructList[DebugEcuIdIndex].MasterEcuKeyM1Hex[i],
                            _ecuIdStructList[DebugEcuIdIndex].MasterEcuKeyM1Hex[i + 1]), 16));

            for (var i = 0; i < _ecuIdStructList[DebugEcuIdIndex].MasterEcuKeyM2Hex.Length; i = i + 2)
                m2.Add(
                         Convert.ToByte(string.Format("{0}{1}", _ecuIdStructList[DebugEcuIdIndex].MasterEcuKeyM2Hex[i],
                             _ecuIdStructList[DebugEcuIdIndex].MasterEcuKeyM2Hex[i + 1]), 16));

            for (var i = 0; i < _ecuIdStructList[DebugEcuIdIndex].MasterEcuKeyM3Hex.Length; i = i + 2)
                m3.Add(
                        Convert.ToByte(string.Format("{0}{1}", _ecuIdStructList[DebugEcuIdIndex].MasterEcuKeyM3Hex[i],
                            _ecuIdStructList[DebugEcuIdIndex].MasterEcuKeyM3Hex[i + 1]), 16));

            byte[] m4;
            byte[] m5;
            byte writeHistory;
            if (UnlockSeedKey(keySlot, m1, m2, m3, out writeHistory, out m4, out m5))
            {
                if (writeHistory == 0x00 || writeHistory == 0x31)
                {
                    MasterKey = "OK";

                    Console.WriteLine("MasterKeySlot-0x{0} M4：{1}", ValueHelper.GetHextStr(keySlot), ValueHelper.GetHextStr(m4));
                    Console.WriteLine("MasterKeySlot-0x{0} M5：{1}", ValueHelper.GetHextStr(keySlot), ValueHelper.GetHextStr(m5));

                    if (writeHistory == 0x00)
                    {
                        _ecuIdStructList[DebugEcuIdIndex].MasterEcuKeyM4Hex = ValueHelper.GetHextStr(m4).Replace(" ", "");
                        _ecuIdStructList[DebugEcuIdIndex].MasterEcuKeyM5Hex = ValueHelper.GetHextStr(m5).Replace(" ", "");
                        UpdateEcuIdStruct(_ecuIdStructList[DebugEcuIdIndex]);
                    }
                    else if (writeHistory == 0x31)
                    {
                        if (string.IsNullOrEmpty(_ecuIdStructList[DebugEcuIdIndex].MasterEcuKeyM4Hex) || string.IsNullOrEmpty(_ecuIdStructList[DebugEcuIdIndex].MasterEcuKeyM5Hex))
                        {
                            MasterKey = "NG MasterKey已注入，但未从数据库中查询到数据";
                        }
                    }
                }
                else
                {
                    Console.WriteLine("MasterKeySlot-0x{0} M4,M5注入失败,代码-{1}", ValueHelper.GetHextStr(keySlot), ValueHelper.GetHextStrWithOx(writeHistory));
                    MasterKey = "NG " + ValueHelper.GetHextStrWithOx(writeHistory);
                }
            }
            else
            {
                MasterKey = "NG";
            }
        }

        [Description("Step2-UnlockKey")]
        public void Step2UnlockKey()
        {
            UnlockKey = string.Empty;

            if (!_ecuIdStructList.Any())
            {
                UnlockKey = "NG";
                return;
            }

            //_ecuIdStructList[DebugEcuIdIndex].UnlockEcuKeyM4Hex = "005700600A2031352D38375539334E444889C93FE55129C5951083EE0EC10DC7";
            //_ecuIdStructList[DebugEcuIdIndex].UnlockEcuKeyM5Hex = "7D856A94C2B8118F93F1FB003F4AF09B";
            //UpdateEcuIdStruct(_ecuIdStructList[DebugEcuIdIndex]);
            //return;

            //读取数据库获取M值
            const byte keySlot = 0x01;

            var m1 = new List<byte>();
            var m2 = new List<byte>();
            var m3 = new List<byte>();

            for (var i = 0; i < _ecuIdStructList[DebugEcuIdIndex].UnlockEcuKeyM1Hex.Length; i = i + 2)
                m1.Add(
                        Convert.ToByte(string.Format("{0}{1}", _ecuIdStructList[DebugEcuIdIndex].UnlockEcuKeyM1Hex[i],
                            _ecuIdStructList[DebugEcuIdIndex].UnlockEcuKeyM1Hex[i + 1]), 16));

            for (var i = 0; i < _ecuIdStructList[DebugEcuIdIndex].UnlockEcuKeyM2Hex.Length; i = i + 2)
                m2.Add(
                         Convert.ToByte(string.Format("{0}{1}", _ecuIdStructList[DebugEcuIdIndex].UnlockEcuKeyM2Hex[i],
                             _ecuIdStructList[DebugEcuIdIndex].UnlockEcuKeyM2Hex[i + 1]), 16));

            for (var i = 0; i < _ecuIdStructList[DebugEcuIdIndex].UnlockEcuKeyM3Hex.Length; i = i + 2)
                m3.Add(
                        Convert.ToByte(string.Format("{0}{1}", _ecuIdStructList[DebugEcuIdIndex].UnlockEcuKeyM3Hex[i],
                            _ecuIdStructList[DebugEcuIdIndex].UnlockEcuKeyM3Hex[i + 1]), 16));

            byte[] m4;
            byte[] m5;
            byte writeHistory;
            if (UnlockSeedKey(keySlot, m1, m2, m3, out writeHistory, out m4, out m5))
            {
                if (writeHistory == 0x00 || writeHistory == 0x31)
                {
                    UnlockKey = "OK";

                    Console.WriteLine("UnlockKeySlot-0x{0} M4：{1}", ValueHelper.GetHextStr(keySlot), ValueHelper.GetHextStr(m4));
                    Console.WriteLine("UnlockKeySlot-0x{0} M5：{1}", ValueHelper.GetHextStr(keySlot), ValueHelper.GetHextStr(m5));

                    if (writeHistory == 0x00)
                    {
                        _ecuIdStructList[DebugEcuIdIndex].UnlockEcuKeyM4Hex = ValueHelper.GetHextStr(m4).Replace(" ", "");
                        _ecuIdStructList[DebugEcuIdIndex].UnlockEcuKeyM5Hex = ValueHelper.GetHextStr(m5).Replace(" ", "");
                        UpdateEcuIdStruct(_ecuIdStructList[DebugEcuIdIndex]);
                    }
                    else if (writeHistory == 0x31)
                    {
                        if (string.IsNullOrEmpty(_ecuIdStructList[DebugEcuIdIndex].UnlockEcuKeyM4Hex) ||
                            string.IsNullOrEmpty(_ecuIdStructList[DebugEcuIdIndex].UnlockEcuKeyM5Hex))
                        {
                            UnlockKey = "NG UnlockKey已注入，但未从数据库中查询到数据";
                        }
                    }
                }
                else
                {
                    Console.WriteLine("UnlockKeySlot-0x{0} M4,M5注入失败,代码-{1}", ValueHelper.GetHextStr(keySlot), ValueHelper.GetHextStr(writeHistory));
                    UnlockKey = "NG " + ValueHelper.GetHextStrWithOx(writeHistory);
                }
            }
            else
            {
                UnlockKey = "NG";
            }
        }

        [Description("Step3-MacKey")]
        public void Step3MacKey()
        {
            MacKey = string.Empty;

            if (!_ecuIdStructList.Any())
            {
                MacKey = "NG";
                return;
            }

            //var temp = new EcuIdStruct();
            //temp.EucIdHex = "_ecuIdStructList";
            //temp.MacKeyInfo =
            //    "[MacKey,Slot=02,M4=001A006B012031352D3532594D344E558C165B76E2E445D3CC1E4F1121FF2746,M5=958B5A05A7C51B63BAF921B2629D99B9];[MacKey,Slot=03,M4=001A006B012031352D3532594D344E668C165B76E2E445D3CC1E4F1121FF2746,M5=415747EA2619A6FB4B421DAB6897DD94];[MacKey,Slot=04,M4=001A006B012031352D3532594D344E778C165B76E2E445D3CC1E4F1121FF2746,M5=B6FABA8357D93497779ABC3357459976];[MacKey,Slot=05,M4=001A006B012031352D3532594D344E888C165B76E2E445D3CC1E4F1121FF2746,M5=695A5E7195D6DF3260740B18DAF6FE37];[MacKey,Slot=06,M4=001A006B012031352D3532594D344E998C165B76E2E445D3CC1E4F1121FF2746,M5=11D519BD91A12742A07FA700549647E8];[MacKey,Slot=07,M4=001A006B012031352D3532594D344EAA8C165B76E2E445D3CC1E4F1121FF2746,M5=7306694F5FB09CFE35F64DAFA1AB73CD];[MacKey,Slot=08,M4=001A006B012031352D3532594D344EBB8C165B76E2E445D3CC1E4F1121FF2746,M5=F924691BE99FAF201B54C7898786A10A];[MacKey,Slot=09,M4=001A006B012031352D3532594D344ECC8C165B76E2E445D3CC1E4F1121FF2746,M5=4173EC1B0DBF147AAE836B08A8F2086D];[MacKey,Slot=0A,M4=001A006B012031352D3532594D344EDD8C165B76E2E445D3CC1E4F1121FF2746,M5=BDE59B9E511A134FBA7F8814892900E0];[MacKey,Slot=0B,M4=001A006B012031352D3532594D344E440E45E9DE165D84C5A80DF96C211430E5,M5=F2AFBBF0406E0CD222B61AAB19343C24];[MacKey,Slot=0C,M4=001A006B012031352D3532594D344E550E45E9DE165D84C5A80DF96C211430E5,M5=37488988073A901E493DEF3C34EE4502];[MacKey,Slot=0D,M4=001A006B012031352D3532594D344E660E45E9DE165D84C5A80DF96C211430E5,M5=0488B17918B6C05CDF991F3C2A1FD469];[MacKey,Slot=0E,M4=001A006B012031352D3532594D344E770E45E9DE165D84C5A80DF96C211430E5,M5=412224888E96FF8BACFB7402C0FB5F76];[MacKey,Slot=0F,M4=001A006B012031352D3532594D344E880E45E9DE165D84C5A80DF96C211430E5,M5=990A48F885930A240D551E78E88C6B69];[MacKey,Slot=10,M4=001A006B012031352D3532594D344E990E45E9DE165D84C5A80DF96C211430E5,M5=B502E181216EC1D2767213A38065542C];[MacKey,Slot=11,M4=001A006B012031352D3532594D344EAA0E45E9DE165D84C5A80DF96C211430E5,M5=5F19D6F16B1D15F74EDAC62FB6776B35];[MacKey,Slot=12,M4=001A006B012031352D3532594D344EBB0E45E9DE165D84C5A80DF96C211430E5,M5=1B9027C6F232F8D678F491EB34B6443B];[MacKey,Slot=13,M4=001A006B012031352D3532594D344ECC0E45E9DE165D84C5A80DF96C211430E5,M5=640C8BEBAFEA81448E9DE6256E60549F];[MacKey,Slot=14,M4=001A006B012031352D3532594D344EDD0E45E9DE165D84C5A80DF96C211430E5,M5=4F527D887C54BFD76522D26E92306536];";
            //_ecuIdStructList.Add(temp);

            //_ecuIdStructList[DebugEcuIdIndex].MacKeyInfo = "[MacKey,Slot=02,M4=005700600A2031352D38375539334E558C165B76E2E445D3CC1E4F1121FF2746,M5=5CA7EDE78BEE52A5A563E15E517D8674];[MacKey,Slot=03,M4=005700600A2031352D38375539334E668C165B76E2E445D3CC1E4F1121FF2746,M5=528D551A4CF910146511850C790D33E6];[MacKey,Slot=04,M4=005700600A2031352D38375539334E778C165B76E2E445D3CC1E4F1121FF2746,M5=BE69FAC317B91A638B7EB32EDC3AC6F3];[MacKey,Slot=05,M4=005700600A2031352D38375539334E888C165B76E2E445D3CC1E4F1121FF2746,M5=CDA5D183952A6CDC13640E3A8D0ECB2F];[MacKey,Slot=06,M4=005700600A2031352D38375539334E998C165B76E2E445D3CC1E4F1121FF2746,M5=457482DB296FAE5D088A4C355D5CB78A];[MacKey,Slot=07,M4=005700600A2031352D38375539334EAA8C165B76E2E445D3CC1E4F1121FF2746,M5=5117F2422D6F8304A79B5D9FCFC320F1];[MacKey,Slot=08,M4=005700600A2031352D38375539334EBB8C165B76E2E445D3CC1E4F1121FF2746,M5=5A22467A43E2853C7383289687A4C0EE];[MacKey,Slot=09,M4=005700600A2031352D38375539334ECC8C165B76E2E445D3CC1E4F1121FF2746,M5=D763EA47590BB093E33709836308D693];[MacKey,Slot=0A,M4=005700600A2031352D38375539334EDD8C165B76E2E445D3CC1E4F1121FF2746,M5=90962DA27F6CC8A57020141C61695B73];[MacKey,Slot=0B,M4=005700600A2031352D38375539334E440E45E9DE165D84C5A80DF96C211430E5,M5=73885BE25487A25A1FB922B333DF56E1];[MacKey,Slot=0C,M4=005700600A2031352D38375539334E550E45E9DE165D84C5A80DF96C211430E5,M5=4502DE522F3188148C47426FC46C80EB];[MacKey,Slot=0D,M4=005700600A2031352D38375539334E660E45E9DE165D84C5A80DF96C211430E5,M5=F03DED4FC6FFC9713A7331A10C586875];[MacKey,Slot=0E,M4=005700600A2031352D38375539334E770E45E9DE165D84C5A80DF96C211430E5,M5=15F68981EBAF5D1E55E72B876FA048DA];[MacKey,Slot=0F,M4=005700600A2031352D38375539334E880E45E9DE165D84C5A80DF96C211430E5,M5=AC4A127E9B51DBCE495C4B62272E56D9];[MacKey,Slot=10,M4=005700600A2031352D38375539334E990E45E9DE165D84C5A80DF96C211430E5,M5=37D1D101CB3ECF4E7F0EA25FC2DCE2E5];[MacKey,Slot=11,M4=005700600A2031352D38375539334EAA0E45E9DE165D84C5A80DF96C211430E5,M5=99D41B39A7AF2ED43FB03F655FBCF28F];[MacKey,Slot=12,M4=005700600A2031352D38375539334EBB0E45E9DE165D84C5A80DF96C211430E5,M5=F46AE079BD182C793896C9B49626AE7B];[MacKey,Slot=13,M4=005700600A2031352D38375539334ECC0E45E9DE165D84C5A80DF96C211430E5,M5=BD7E771576A2617CFEAD6E472BC36FA8];[MacKey,Slot=14,M4=005700600A2031352D38375539334EDD0E45E9DE165D84C5A80DF96C211430E5,M5=326167E0E59D0A4E0B2C51881E123375]";
            //UpdateEcuIdStruct(_ecuIdStructList[DebugEcuIdIndex]);
            //return;

            var result = true;

            var macKeyInfoList = new List<string>();
            if (!string.IsNullOrEmpty(_ecuIdStructList[DebugEcuIdIndex].MacKeyInfo))
            {
                var sp = _ecuIdStructList[DebugEcuIdIndex].MacKeyInfo.Split(';');

                foreach (var t in sp)
                {
                    var isFind = false;
                    if (!string.IsNullOrEmpty(t))
                    {
                        foreach (var key in _macKeyList.Keys)
                        {
                            var macKeyInfoTitle = string.Format("[MacKey,Slot={0}", ValueHelper.GetHextStr(key));

                            if (t.StartsWith(macKeyInfoTitle))
                            {
                                macKeyInfoList.Add(t);
                                isFind = true;
                                break;
                            }
                        }

                        if (!isFind)
                        {
                            MacKey = "NG 数据库中记录异常";
                            return;
                        }
                    }
                }
            }

            var ngError = string.Empty;

            foreach (var key in _macKeyList.Keys)
            {
                var keySlot = key;
                var m1 = new List<byte>();
                var m2 = new List<byte>();
                var m3 = new List<byte>();

                for (var i = 0; i < _macKeyList[key][0].Length; i = i + 2)
                    m1.Add(
                            Convert.ToByte(
                            string.Format("{0}{1}", _macKeyList[key][0][i], _macKeyList[key][0][i + 1]), 16));

                for (var i = 0; i < _macKeyList[key][1].Length; i = i + 2)
                    m2.Add(
                            Convert.ToByte(
                            string.Format("{0}{1}", _macKeyList[key][1][i], _macKeyList[key][1][i + 1]), 16));


                for (var i = 0; i < _macKeyList[key][2].Length; i = i + 2)
                    m3.Add(
                            Convert.ToByte(
                            string.Format("{0}{1}", _macKeyList[key][2][i], _macKeyList[key][2][i + 1]), 16));

                byte[] m4;
                byte[] m5;
                byte writeHistory;
                if (UnlockSeedKey(keySlot, m1.ToArray(), m2.ToArray(), m3.ToArray(), out writeHistory, out m4, out m5))
                {
                    if (writeHistory == 0x00 || writeHistory == 0x31)
                    {
                        Console.WriteLine("MacKeySlot-0x{0} M4：{1}", ValueHelper.GetHextStr(keySlot), ValueHelper.GetHextStr(m4));
                        Console.WriteLine("MacKeySlot-0x{0} M5：{1}", ValueHelper.GetHextStr(keySlot), ValueHelper.GetHextStr(m5));

                        var macKeyInfoTitle = string.Format("[MacKey,Slot={0}", ValueHelper.GetHextStr(keySlot));
                        var findTitleIndex = macKeyInfoList.FindIndex(f => f.StartsWith(macKeyInfoTitle));

                        var macKeyInfo = string.Format("[MacKey,Slot={0},M4={1},M5={2}]",
                            ValueHelper.GetHextStr(keySlot),
                            ValueHelper.GetHextStr(m4).Replace(" ", ""),
                            ValueHelper.GetHextStr(m5).Replace(" ", ""));

                        if (writeHistory == 0x00)
                        {
                            if (findTitleIndex == -1)
                            {
                                macKeyInfoList.Add(macKeyInfo);
                            }
                            else
                            {
                                macKeyInfoList[findTitleIndex] = macKeyInfo;
                            }
                        }
                        else if (writeHistory == 0x31)
                        {
                            if (findTitleIndex == -1)
                            {
                                ngError += string.Format("MacKeySlot-0x{0} 已注入，但未从数据库中查询到记录;", ValueHelper.GetHextStr(keySlot));
                                result = false;
                                break;
                            }

                            //macKeyInfoList[findTitleIndex] = macKeyInfo;
                        }

                        var str = macKeyInfoList.Aggregate(string.Empty, (current, t) => current + t + ";").TrimEnd(',');
                        _ecuIdStructList[DebugEcuIdIndex].MacKeyInfo = str;
                        UpdateEcuIdStruct(_ecuIdStructList[DebugEcuIdIndex]);
                    }
                    else
                    {
                        ngError += string.Format("MacKeySlot-0x{0} M4,M5注入失败,代码-{1};", ValueHelper.GetHextStr(keySlot),
                            ValueHelper.GetHextStr(writeHistory));
                        Console.WriteLine(ngError);

                        result = false;
                        break;
                    }
                    continue;
                }

                result = false;
                break;
            }

            MacKey = result ? "OK" : "NG " + ngError;

            //if (!result)
            //    return;
            //var str = macKeyInfoList.Aggregate(string.Empty, (current, t) => current + t + ";").TrimEnd(',');
            //_ecuIdStructList[DebugEcuIdIndex].MacKeyInfo = str;
        }

        private bool UnlockSeedKey(
            byte keySlot,
            IEnumerable<byte> m1, IEnumerable<byte> m2, IEnumerable<byte> m3,
            out byte wrtieHistory, out byte[] m4, out byte[] m5)
        {
            if (Can == null)
            {
                m4 = new byte[] { };
                m5 = new byte[] { };
                wrtieHistory = 0xFF;
                return false;
            }

            lock (_lockSend)
            {
                EnterExtendedSession();
                Thread.Sleep(50);

                DisableRxAndTxCommunication();
                Thread.Sleep(50);

                SecurityAccess0D0E();
                Thread.Sleep(50);

                if (SecurityAccessResult == "OK")
                {
                    var routineControlOptBytes = new List<byte> { keySlot };
                    routineControlOptBytes.AddRange(m1);
                    routineControlOptBytes.AddRange(m2);
                    routineControlOptBytes.AddRange(m3);

                    byte[] echoBytes;
                    if (Can.CanBusWithUds.TryStartRoutineControl(
                        CanDiagnosisRequestPhyCanId,
                        CanDiagnosisResponseCanId,
                        CanBus.CanType.Extended,
                        0x02, 0x72, out echoBytes, routineControlOptBytes.ToArray()))
                    {
                        if (echoBytes.Length == 49)
                        {
                            wrtieHistory = echoBytes[0];

                            m4 = new byte[32];
                            Array.Copy(echoBytes, 1, m4, 0, 32);

                            m5 = new byte[16];
                            Array.Copy(echoBytes, 33, m5, 0, 16);

                            //if (wrtieHistory == 0x31 || wrtieHistory == 0x00)
                            //    return true;

                            Console.WriteLine(Name + " keySlot: " + ValueHelper.GetHextStrWithOx(keySlot) + ", WriteHistory: " + ValueHelper.GetHextStrWithOx(wrtieHistory));

                            if (wrtieHistory == 0x00 || wrtieHistory == 0x31)
                            {
                                EnterDefaultSession();
                                return true;
                            }
                        }
                    }
                }

                EnterDefaultSession();
                m4 = new byte[] { };
                m5 = new byte[] { };
                wrtieHistory = 0xFF;
                return false;
            }
        }

        //[Description("调试解锁Key")]
        //public void PrivateTestKey(string ecuId, string keySlot)
        //{
        //    var selectSql =
        //                "SELECT TOP 1 [id],[EcuId],[EcuIdFromFile],[TackInfo],[CreateTime],[LastRecordTime],[IsUsage],[IsUploadToSgm],[RandomSeed],[Sha256Total],[Sha256Sub20],[IdCodeAscii],[IdCode],[MASTER_ECU_KEY_KeySlot],[MASTER_ECU_KEY_M1],[MASTER_ECU_KEY_M2],[MASTER_ECU_KEY_M3],[MASTER_ECU_KEY_M4],[MASTER_ECU_KEY_M5],[UNLOCK_ECU_KEY_KeySlot],[UNLOCK_ECU_KEY_M1],[UNLOCK_ECU_KEY_M2],[UNLOCK_ECU_KEY_M3],[UNLOCK_ECU_KEY_M4],[UNLOCK_ECU_KEY_M5],[MacKey] FROM [IPMS].[dbo].[manufactureSgm458PlgDatas] where [IsUsage] = '2' and [EcuId] = '" +
        //                ecuId + "'";

        //    var ds = Query(selectSql);
        //    if (ds.Tables[0].DefaultView.Count == 1)
        //    {
        //        var ecuIdStruct = new EcuIdStruct
        //        {
        //            EucIdHex = ds.Tables[0].DefaultView[0]["EcuId"].ToString(),
        //            MasterEcuKeySlot = ds.Tables[0].DefaultView[0]["MASTER_ECU_KEY_KeySlot"].ToString(),
        //            MasterEcuKeyM1Hex = ds.Tables[0].DefaultView[0]["MASTER_ECU_KEY_M1"].ToString(),
        //            MasterEcuKeyM2Hex = ds.Tables[0].DefaultView[0]["MASTER_ECU_KEY_M2"].ToString(),
        //            MasterEcuKeyM3Hex = ds.Tables[0].DefaultView[0]["MASTER_ECU_KEY_M3"].ToString(),
        //            MasterEcuKeyM4Hex =
        //                ds.Tables[0].DefaultView[0]["MASTER_ECU_KEY_M4"] == null
        //                    ? string.Empty
        //                    : ds.Tables[0].DefaultView[0]["MASTER_ECU_KEY_M4"].ToString(),
        //            MasterEcuKeyM5Hex =
        //                ds.Tables[0].DefaultView[0]["MASTER_ECU_KEY_M5"] == null
        //                    ? string.Empty
        //                    : ds.Tables[0].DefaultView[0]["MASTER_ECU_KEY_M5"].ToString(),
        //            UnlockEcuKeySlot = ds.Tables[0].DefaultView[0]["UNLOCK_ECU_KEY_KeySlot"].ToString(),
        //            UnlockEcuKeyM1Hex = ds.Tables[0].DefaultView[0]["UNLOCK_ECU_KEY_M1"].ToString(),
        //            UnlockEcuKeyM2Hex = ds.Tables[0].DefaultView[0]["UNLOCK_ECU_KEY_M2"].ToString(),
        //            UnlockEcuKeyM3Hex = ds.Tables[0].DefaultView[0]["UNLOCK_ECU_KEY_M3"].ToString(),
        //            UnlockEcuKeyM4Hex =
        //                ds.Tables[0].DefaultView[0]["UNLOCK_ECU_KEY_M4"] == null
        //                    ? string.Empty
        //                    : ds.Tables[0].DefaultView[0]["UNLOCK_ECU_KEY_M4"].ToString(),
        //            UnlockEcuKeyM5Hex =
        //                ds.Tables[0].DefaultView[0]["UNLOCK_ECU_KEY_M5"] == null
        //                    ? string.Empty
        //                    : ds.Tables[0].DefaultView[0]["UNLOCK_ECU_KEY_M5"].ToString(),
        //            MacKeyInfo =
        //                ds.Tables[0].DefaultView[0]["MacKey"] == null
        //                    ? string.Empty
        //                    : ds.Tables[0].DefaultView[0]["MacKey"].ToString(),
        //            RandomSeed =
        //                ds.Tables[0].DefaultView[0]["RandomSeed"] == null
        //                    ? string.Empty
        //                    : ds.Tables[0].DefaultView[0]["RandomSeed"].ToString(),
        //            Sha256Total =
        //                ds.Tables[0].DefaultView[0]["Sha256Total"] == null
        //                    ? string.Empty
        //                    : ds.Tables[0].DefaultView[0]["Sha256Total"].ToString(),
        //            Sha256Sub20 =
        //                ds.Tables[0].DefaultView[0]["Sha256Sub20"] == null
        //                    ? string.Empty
        //                    : ds.Tables[0].DefaultView[0]["Sha256Sub20"].ToString(),
        //            IdCodeAscii =
        //                ds.Tables[0].DefaultView[0]["IdCodeAscii"] == null
        //                    ? string.Empty
        //                    : ds.Tables[0].DefaultView[0]["IdCodeAscii"].ToString(),
        //            IdCode =
        //                ds.Tables[0].DefaultView[0]["IdCode"] == null
        //                    ? string.Empty
        //                    : ds.Tables[0].DefaultView[0]["IdCode"].ToString(),
        //            TrackInfo =
        //                ds.Tables[0].DefaultView[0]["IdCode"] == null
        //                    ? string.Empty
        //                    : ds.Tables[0].DefaultView[0]["TackInfo"].ToString(),
        //        };

        //        if (keySlot == "FF")
        //        {
        //            var m1 = new List<byte>();
        //            var m2 = new List<byte>();
        //            var m3 = new List<byte>();

        //            for (var i = 0; i < ecuIdStruct.MasterEcuKeyM1Hex.Length; i = i + 2)
        //                m1.Add(
        //                        Convert.ToByte(string.Format("{0}{1}", ecuIdStruct.MasterEcuKeyM1Hex[i],
        //                            ecuIdStruct.MasterEcuKeyM1Hex[i + 1]), 16));

        //            for (var i = 0; i < ecuIdStruct.MasterEcuKeyM2Hex.Length; i = i + 2)
        //                m2.Add(
        //                         Convert.ToByte(string.Format("{0}{1}", ecuIdStruct.MasterEcuKeyM2Hex[i],
        //                             ecuIdStruct.MasterEcuKeyM2Hex[i + 1]), 16));

        //            for (var i = 0; i < ecuIdStruct.MasterEcuKeyM3Hex.Length; i = i + 2)
        //                m3.Add(
        //                        Convert.ToByte(string.Format("{0}{1}", ecuIdStruct.MasterEcuKeyM3Hex[i],
        //                            ecuIdStruct.MasterEcuKeyM3Hex[i + 1]), 16));

        //            byte[] m4;
        //            byte[] m5;
        //            byte writeHistory;
        //            if (UnlockSeedKey(0xFF, m1, m2, m3, out writeHistory, out m4, out m5))
        //            {
        //                if (writeHistory == 0x00 || writeHistory == 0x31)
        //                {
        //                    Console.WriteLine("MasterKeySlot-0x{0} M4：{1}", ValueHelper.GetHextStr(0xFF), ValueHelper.GetHextStr(m4));
        //                    Console.WriteLine("MasterKeySlot-0x{0} M5：{1}", ValueHelper.GetHextStr(0xFF), ValueHelper.GetHextStr(m5));

        //                    if (writeHistory == 0x00)
        //                    {
        //                        ecuIdStruct.MasterEcuKeyM4Hex = ValueHelper.GetHextStr(m4).Replace(" ", "");
        //                        ecuIdStruct.MasterEcuKeyM5Hex = ValueHelper.GetHextStr(m5).Replace(" ", "");
        //                    }
        //                    else if (writeHistory == 0x31)
        //                    {
        //                        if (string.IsNullOrEmpty(ecuIdStruct.MasterEcuKeyM4Hex) || string.IsNullOrEmpty(ecuIdStruct.MasterEcuKeyM5Hex))
        //                        {
        //                            Console.WriteLine("NG MasterKey已注入，但未从数据库中查询到数据");
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    Console.WriteLine("MasterKeySlot-0x{0} M4,M5注入失败,代码-{1}", ValueHelper.GetHextStr(0xFF), ValueHelper.GetHextStrWithOx(writeHistory));
        //                    Console.WriteLine("NG " + ValueHelper.GetHextStrWithOx(writeHistory));
        //                }
        //            }
        //            else
        //            {
        //                Console.WriteLine(" MasterKey = NG");
        //            }
        //        }
        //        else if (keySlot == "01")
        //        {
        //            var m1 = new List<byte>();
        //            var m2 = new List<byte>();
        //            var m3 = new List<byte>();

        //            for (var i = 0; i < ecuIdStruct.UnlockEcuKeyM1Hex.Length; i = i + 2)
        //                m1.Add(
        //                        Convert.ToByte(string.Format("{0}{1}", ecuIdStruct.UnlockEcuKeyM1Hex[i],
        //                            ecuIdStruct.UnlockEcuKeyM1Hex[i + 1]), 16));

        //            for (var i = 0; i < ecuIdStruct.UnlockEcuKeyM2Hex.Length; i = i + 2)
        //                m2.Add(
        //                         Convert.ToByte(string.Format("{0}{1}", ecuIdStruct.UnlockEcuKeyM2Hex[i],
        //                             ecuIdStruct.UnlockEcuKeyM2Hex[i + 1]), 16));

        //            for (var i = 0; i < ecuIdStruct.UnlockEcuKeyM3Hex.Length; i = i + 2)
        //                m3.Add(
        //                        Convert.ToByte(string.Format("{0}{1}", ecuIdStruct.UnlockEcuKeyM3Hex[i],
        //                            ecuIdStruct.UnlockEcuKeyM3Hex[i + 1]), 16));

        //            byte[] m4;
        //            byte[] m5;
        //            byte writeHistory;
        //            if (UnlockSeedKey(0x01, m1, m2, m3, out writeHistory, out m4, out m5))
        //            {
        //                if (writeHistory == 0x00 || writeHistory == 0x31)
        //                {
        //                    Console.WriteLine("UnlockKeySlot-0x{0} M4：{1}", ValueHelper.GetHextStr(0x01), ValueHelper.GetHextStr(m4));
        //                    Console.WriteLine("UnlockKeySlot-0x{0} M5：{1}", ValueHelper.GetHextStr(0x01), ValueHelper.GetHextStr(m5));

        //                    if (writeHistory == 0x00)
        //                    {
        //                        ecuIdStruct.UnlockEcuKeyM4Hex = ValueHelper.GetHextStr(m4).Replace(" ", "");
        //                        ecuIdStruct.UnlockEcuKeyM5Hex = ValueHelper.GetHextStr(m5).Replace(" ", "");
        //                    }
        //                    else if (writeHistory == 0x31)
        //                    {
        //                        if (string.IsNullOrEmpty(ecuIdStruct.UnlockEcuKeyM4Hex) ||
        //                            string.IsNullOrEmpty(ecuIdStruct.UnlockEcuKeyM5Hex))
        //                        {
        //                            Console.WriteLine("NG UnlockKey已注入，但未从数据库中查询到数据");
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    Console.WriteLine("UnlockKeySlot-0x{0} M4,M5注入失败,代码-{1}", ValueHelper.GetHextStr(0x01), ValueHelper.GetHextStr(writeHistory));
        //                    Console.WriteLine("NG " + ValueHelper.GetHextStrWithOx(writeHistory));
        //                }
        //            }
        //            else
        //            {
        //                Console.WriteLine("UnlockKey = NG");
        //            }
        //        }
        //    }
        //}

        #endregion

        #region SQL相关

        [Description("R,存储数据结果")]
        public string SaveDataResult;

        [Description("R/W,服务器IP地址")]
        public string ServerIp = "192.168.0.138";

        [Description("R/W,服务器数据库名称")]
        public string ServerDataBase = "IPMS";

        [Description("R/W,服务器用户名")]
        public string ServerUid = "ipms";

        [Description("R/W,服务器用密码")]
        public string ServerPwd = "Scae2020#";

        public void SaveData()
        {
            SaveDataResult = string.Empty;

            if (!_ecuIdStructList.Any())
            {
                SaveDataResult = "NG";
                return;
            }

            var selectSql =
                      "SELECT TOP 1 [id],[EcuId],[EcuIdFromFile],[TackInfo],[CreateTime],[LastRecordTime],[IsUsage],[IsUploadToSgm],[RandomSeed],[Sha256Total],[Sha256Sub20],[IdCodeAscii],[IdCode],[MASTER_ECU_KEY_KeySlot],[MASTER_ECU_KEY_M1],[MASTER_ECU_KEY_M2],[MASTER_ECU_KEY_M3],[MASTER_ECU_KEY_M4],[MASTER_ECU_KEY_M5],[UNLOCK_ECU_KEY_KeySlot],[UNLOCK_ECU_KEY_M1],[UNLOCK_ECU_KEY_M2],[UNLOCK_ECU_KEY_M3],[UNLOCK_ECU_KEY_M4],[UNLOCK_ECU_KEY_M5],[MacKey] FROM [IPMS].[dbo].[manufactureSgm458PlgDatas] where [EcuId] = '" + _ecuIdStructList[DebugEcuIdIndex].EucIdHex + "'";

            var ds = Query(selectSql);
            if (ds.Tables[0].DefaultView.Count == 1)
            {
                var readEcuIdStructFromSql = new EcuIdStruct
                {
                    EucIdHex = ds.Tables[0].DefaultView[0]["EcuId"].ToString(),
                    MasterEcuKeySlot = ds.Tables[0].DefaultView[0]["MASTER_ECU_KEY_KeySlot"].ToString(),
                    MasterEcuKeyM1Hex = ds.Tables[0].DefaultView[0]["MASTER_ECU_KEY_M1"].ToString(),
                    MasterEcuKeyM2Hex = ds.Tables[0].DefaultView[0]["MASTER_ECU_KEY_M2"].ToString(),
                    MasterEcuKeyM3Hex = ds.Tables[0].DefaultView[0]["MASTER_ECU_KEY_M3"].ToString(),
                    MasterEcuKeyM4Hex =
                        ds.Tables[0].DefaultView[0]["MASTER_ECU_KEY_M4"] == null
                            ? string.Empty
                            : ds.Tables[0].DefaultView[0]["MASTER_ECU_KEY_M4"].ToString(),
                    MasterEcuKeyM5Hex =
                        ds.Tables[0].DefaultView[0]["MASTER_ECU_KEY_M5"] == null
                            ? string.Empty
                            : ds.Tables[0].DefaultView[0]["MASTER_ECU_KEY_M5"].ToString(),
                    UnlockEcuKeySlot = ds.Tables[0].DefaultView[0]["UNLOCK_ECU_KEY_KeySlot"].ToString(),
                    UnlockEcuKeyM1Hex = ds.Tables[0].DefaultView[0]["UNLOCK_ECU_KEY_M1"].ToString(),
                    UnlockEcuKeyM2Hex = ds.Tables[0].DefaultView[0]["UNLOCK_ECU_KEY_M2"].ToString(),
                    UnlockEcuKeyM3Hex = ds.Tables[0].DefaultView[0]["UNLOCK_ECU_KEY_M3"].ToString(),
                    UnlockEcuKeyM4Hex =
                        ds.Tables[0].DefaultView[0]["UNLOCK_ECU_KEY_M4"] == null
                            ? string.Empty
                            : ds.Tables[0].DefaultView[0]["UNLOCK_ECU_KEY_M4"].ToString(),
                    UnlockEcuKeyM5Hex =
                        ds.Tables[0].DefaultView[0]["UNLOCK_ECU_KEY_M5"] == null
                            ? string.Empty
                            : ds.Tables[0].DefaultView[0]["UNLOCK_ECU_KEY_M5"].ToString(),
                    MacKeyInfo =
                        ds.Tables[0].DefaultView[0]["MacKey"] == null
                            ? string.Empty
                            : ds.Tables[0].DefaultView[0]["MacKey"].ToString(),
                    RandomSeed =
                        ds.Tables[0].DefaultView[0]["RandomSeed"] == null
                            ? string.Empty
                            : ds.Tables[0].DefaultView[0]["RandomSeed"].ToString(),
                    Sha256Total =
                        ds.Tables[0].DefaultView[0]["Sha256Total"] == null
                            ? string.Empty
                            : ds.Tables[0].DefaultView[0]["Sha256Total"].ToString(),
                    Sha256Sub20 =
                        ds.Tables[0].DefaultView[0]["Sha256Sub20"] == null
                            ? string.Empty
                            : ds.Tables[0].DefaultView[0]["Sha256Sub20"].ToString(),
                    IdCodeAscii =
                        ds.Tables[0].DefaultView[0]["IdCodeAscii"] == null
                            ? string.Empty
                            : ds.Tables[0].DefaultView[0]["IdCodeAscii"].ToString(),
                    IdCode =
                        ds.Tables[0].DefaultView[0]["IdCode"] == null
                            ? string.Empty
                            : ds.Tables[0].DefaultView[0]["IdCode"].ToString(),
                    TrackInfo =
                        ds.Tables[0].DefaultView[0]["IdCode"] == null
                            ? string.Empty
                            : ds.Tables[0].DefaultView[0]["TackInfo"].ToString(),
                };

                var ecuIdToStoreSql = _ecuIdStructList[DebugEcuIdIndex];

                if (readEcuIdStructFromSql.EucIdHex != ecuIdToStoreSql.EucIdHex)
                {
                    SaveDataResult = "NG" + "ECUID" + "与存储不符";
                    return;
                }

                if (readEcuIdStructFromSql.MasterEcuKeyM1Hex != ecuIdToStoreSql.MasterEcuKeyM1Hex)
                {
                    SaveDataResult = "NG" + "MasterEcuKeyM1" + "与存储不符";
                    return;
                }

                if (readEcuIdStructFromSql.MasterEcuKeyM2Hex != ecuIdToStoreSql.MasterEcuKeyM2Hex)
                {
                    SaveDataResult = "NG" + "MasterEcuKeyM2" + "与存储不符";
                    return;
                }

                if (readEcuIdStructFromSql.MasterEcuKeyM3Hex != ecuIdToStoreSql.MasterEcuKeyM3Hex)
                {
                    SaveDataResult = "NG" + "MasterEcuKeyM3" + "与存储不符";
                    return;
                }

                if (readEcuIdStructFromSql.MasterEcuKeyM4Hex != ecuIdToStoreSql.MasterEcuKeyM4Hex)
                {
                    SaveDataResult = "NG" + "MasterEcuKeyM4" + "与存储不符";
                    return;
                }

                if (readEcuIdStructFromSql.MasterEcuKeyM5Hex != ecuIdToStoreSql.MasterEcuKeyM5Hex)
                {
                    SaveDataResult = "NG" + "MasterEcuKeyM5" + "与存储不符";
                    return;
                }

                if (readEcuIdStructFromSql.UnlockEcuKeyM1Hex != ecuIdToStoreSql.UnlockEcuKeyM1Hex)
                {
                    SaveDataResult = "NG" + "UnlockEcuKeyM1" + "与存储不符";
                    return;
                }

                if (readEcuIdStructFromSql.UnlockEcuKeyM2Hex != ecuIdToStoreSql.UnlockEcuKeyM2Hex)
                {
                    SaveDataResult = "NG" + "UnlockEcuKeyM2" + "与存储不符";
                    return;
                }

                if (readEcuIdStructFromSql.UnlockEcuKeyM3Hex != ecuIdToStoreSql.UnlockEcuKeyM3Hex)
                {
                    SaveDataResult = "NG" + "UnlockEcuKeyM3" + "与存储不符";
                    return;
                }

                if (readEcuIdStructFromSql.UnlockEcuKeyM4Hex != ecuIdToStoreSql.UnlockEcuKeyM4Hex)
                {
                    SaveDataResult = "NG" + "UnlockEcuKeyM4" + "与存储不符";
                    return;
                }

                if (readEcuIdStructFromSql.UnlockEcuKeyM5Hex != ecuIdToStoreSql.UnlockEcuKeyM5Hex)
                {
                    SaveDataResult = "NG" + "UnlockEcuKeyM5" + "与存储不符";
                    return;
                }

                if (readEcuIdStructFromSql.MacKeyInfo != ecuIdToStoreSql.MacKeyInfo)
                {
                    SaveDataResult = "NG" + "MacKey" + "与存储不符";
                    return;
                }

                if (readEcuIdStructFromSql.RandomSeed != ecuIdToStoreSql.RandomSeed)
                {
                    SaveDataResult = "NG" + "RandomSeed" + "与存储不符";
                    return;
                }

                if (readEcuIdStructFromSql.Sha256Total != ecuIdToStoreSql.Sha256Total)
                {
                    SaveDataResult = "NG" + "经过SHA256算法得到的(32Byte)" + "与存储不符";
                    return;
                }

                if (readEcuIdStructFromSql.Sha256Sub20 != ecuIdToStoreSql.Sha256Sub20)
                {
                    SaveDataResult = "NG" + "经过SHA256算法得到的(前20Byte)" + "与存储不符";
                    return;
                }

                if (readEcuIdStructFromSql.IdCode != ecuIdToStoreSql.IdCode)
                {
                    SaveDataResult = "NG" + "ID-CODE-16进制格式" + "与存储不符";
                    return;
                }

                if (readEcuIdStructFromSql.IdCodeAscii != ecuIdToStoreSql.IdCodeAscii)
                {
                    SaveDataResult = "NG" + "ID-CODE-ASCII格式" + "与存储不符";
                    return;
                }

                if (readEcuIdStructFromSql.TrackInfo != ecuIdToStoreSql.TrackInfo)
                {
                    SaveDataResult = "NG" + "追溯信息" + "与存储不符";
                    return;
                }

                LockEcuId(ecuIdToStoreSql, 1.ToString());
                SaveDataResult = "OK";

            }
            //if (!_ecuIdStructList.Any())
            //{
            //    SaveDataResult = "NG";
            //    return;
            //}

            //var toSave = _ecuIdStructList[DebugEcuIdIndex];

            //if (string.IsNullOrEmpty(toSave.EucIdHex))
            //{
            //    SaveDataResult = "NG " + "ECUID" + "缺失";
            //    return;
            //}

            //if (string.IsNullOrEmpty(toSave.MasterEcuKeySlot))
            //{
            //    SaveDataResult = "NG " + "MasterEcuKeySlot" + "缺失";
            //    return;
            //}

            //if (string.IsNullOrEmpty(toSave.MasterEcuKeyM1Hex))
            //{
            //    SaveDataResult = "NG " + "MasterEcuKeyM1" + "缺失";
            //    return;
            //}

            //if (string.IsNullOrEmpty(toSave.MasterEcuKeyM2Hex))
            //{
            //    SaveDataResult = "NG " + "MasterEcuKeyM2" + "缺失";
            //    return;
            //}

            //if (string.IsNullOrEmpty(toSave.MasterEcuKeyM3Hex))
            //{
            //    SaveDataResult = "NG " + "MasterEcuKeyM3" + "缺失";
            //    return;
            //}

            //if (string.IsNullOrEmpty(toSave.MasterEcuKeyM4Hex))
            //{
            //    SaveDataResult = "NG " + "MasterEcuKeyM4" + "缺失";
            //    return;
            //}

            //if (string.IsNullOrEmpty(toSave.MasterEcuKeyM5Hex))
            //{
            //    SaveDataResult = "NG " + "MasterEcuKeyM5" + "缺失";
            //    return;
            //}

            //if (string.IsNullOrEmpty(toSave.UnlockEcuKeySlot))
            //{
            //    SaveDataResult = "NG " + "UnlockEcuKeySlot" + "缺失";
            //    return;
            //}

            //if (string.IsNullOrEmpty(toSave.UnlockEcuKeyM1Hex))
            //{
            //    SaveDataResult = "NG " + "UnlockEcuKeyM1" + "缺失";
            //    return;
            //}

            //if (string.IsNullOrEmpty(toSave.UnlockEcuKeyM2Hex))
            //{
            //    SaveDataResult = "NG " + "UnlockEcuKeyM2" + "缺失";
            //    return;
            //}

            //if (string.IsNullOrEmpty(toSave.UnlockEcuKeyM3Hex))
            //{
            //    SaveDataResult = "NG " + "UnlockEcuKeyM3" + "缺失";
            //    return;
            //}

            //if (string.IsNullOrEmpty(toSave.UnlockEcuKeyM4Hex))
            //{
            //    SaveDataResult = "NG " + "UnlockEcuKeyM4" + "缺失";
            //    return;
            //}

            //if (string.IsNullOrEmpty(toSave.UnlockEcuKeyM5Hex))
            //{
            //    SaveDataResult = "NG " + "UnlockEcuKeyM5" + "缺失";
            //    return;
            //}

            //if (string.IsNullOrEmpty(toSave.MacKeyInfo))
            //{
            //    SaveDataResult = "NG " + "MacKey" + "缺失";
            //    return;
            //}

            //if (string.IsNullOrEmpty(toSave.RandomSeed))
            //{
            //    SaveDataResult = "NG " + "RandomSeed" + "缺失";
            //    return;
            //}

            //if (string.IsNullOrEmpty(toSave.Sha256Total))
            //{
            //    SaveDataResult = "NG " + "经过SHA256算法得到的(32Byte)" + "缺失";
            //    return;
            //}

            //if (string.IsNullOrEmpty(toSave.Sha256Sub20))
            //{
            //    SaveDataResult = "NG " + "经过SHA256算法得到的(前20Byte)" + "缺失";
            //    return;
            //}

            //if (string.IsNullOrEmpty(toSave.IdCodeAscii))
            //{
            //    SaveDataResult = "NG " + "IdCodeAscii" + "缺失";
            //    return;
            //}

            //if (string.IsNullOrEmpty(toSave.IdCode))
            //{
            //    SaveDataResult = "NG " + "IdCode" + "缺失";
            //    return;
            //}

            //if (string.IsNullOrEmpty(toSave.TrackInfo))
            //{
            //    SaveDataResult = "NG " + "追溯信息" + "缺失";
            //    return;
            //}

            //try
            //{

            //    var updateSql =
            //        string.Format(
            //            "update manufactureSgm458PlgDatas set " +
            //            "IsUsage = '{0}', " +
            //            "RandomSeed = '{1}', Sha256Total = '{2}', Sha256Sub20 = '{3}', " +
            //            "IdCodeAscii = '{4}', IdCode = '{5}', TackInfo = '{6}', " +
            //            "MacKey = '{7}', " +
            //            "MASTER_ECU_KEY_M4 = '{8}', MASTER_ECU_KEY_M5 = '{9}', " +
            //            "UNLOCK_ECU_KEY_M4 = '{10}', UNLOCK_ECU_KEY_M5 = '{11}' " +
            //            "where EcuId = '{12}'",
            //            1,
            //            toSave.RandomSeed, toSave.Sha256Total, toSave.Sha256Sub20,
            //            toSave.IdCodeAscii, toSave.IdCode, toSave.TrackInfo,
            //            toSave.MacKeyInfo,
            //            toSave.MasterEcuKeyM4Hex, toSave.MasterEcuKeyM5Hex,
            //            toSave.UnlockEcuKeyM4Hex, toSave.UnlockEcuKeyM5Hex,
            //            toSave.EucIdHex);

            //    var sqlConnectiong = string.Format(@"server={0};database={1};uid={2};pwd={3}", ServerIp, ServerDataBase,
            //        ServerUid, ServerPwd);


            //    using (var connection = new SqlConnection(sqlConnectiong))
            //    {
            //        using (var cmd = new SqlCommand())
            //        {
            //            PrepareCommand(cmd, connection, null, updateSql, null);
            //            var rows = cmd.ExecuteNonQuery();
            //            cmd.Parameters.Clear();
            //            SaveDataResult = rows == 1 ? "OK" : "NG";
            //        }
            //    }
            //}
            //catch (Exception)
            //{
            //    SaveDataResult = "NG";
            //}
        }

        [Description("释放当前使用的ECU-ID")]
        public void UnlockEcuId()
        {
            try
            {
                var ecuId = _ecuIdStructList[DebugEcuIdIndex].EucIdHex;

                var selectSql =
                    "SELECT TOP 1 [id],[EcuId],[EcuIdFromFile],[TackInfo],[CreateTime],[LastRecordTime],[IsUsage],[IsUploadToSgm],[RandomSeed],[Sha256Total],[Sha256Sub20],[IdCodeAscii],[IdCode],[MASTER_ECU_KEY_KeySlot],[MASTER_ECU_KEY_M1],[MASTER_ECU_KEY_M2],[MASTER_ECU_KEY_M3],[MASTER_ECU_KEY_M4],[MASTER_ECU_KEY_M5],[UNLOCK_ECU_KEY_KeySlot],[UNLOCK_ECU_KEY_M1],[UNLOCK_ECU_KEY_M2],[UNLOCK_ECU_KEY_M3],[UNLOCK_ECU_KEY_M4],[UNLOCK_ECU_KEY_M5] FROM [IPMS].[dbo].[manufactureSgm458PlgDatas] where [IsUsage] = '2' and [EcuId] = '" +
                    ecuId + "'";
                var ds = Query(selectSql);
                if (ds.Tables[0].DefaultView.Count == 1)
                {
                    LockUnlockEcuId(ecuId, 0.ToString());
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void LockUnlockEcuId(string ecuId, string lockStr)
        {
            try
            {
                var updateSql =
                    string.Format(
                        "update manufactureSgm458PlgDatas set " +
                        "IsUsage = '{0}' " +
                        "where EcuId = '{1}'",
                        lockStr,
                        ecuId);

                var sqlConnectiong = string.Format(@"server={0};database={1};uid={2};pwd={3}", ServerIp, ServerDataBase,
                    ServerUid, ServerPwd);


                using (var connection = new SqlConnection(sqlConnectiong))
                {
                    using (var cmd = new SqlCommand())
                    {
                        PrepareCommand(cmd, connection, null, updateSql, null);
                        var rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void LockEcuId(EcuIdStruct ecuIdStruct, string lockId)
        {
            var updateSql =
                   string.Format(
                       "update manufactureSgm458PlgDatas set IsUsage = '{0}', RandomSeed = '{1}', Sha256Total = '{2}', Sha256Sub20 = '{3}', IdCodeAscii = '{4}', IdCode = '{5}', TackInfo = '{6}', MacKey = '{7}', MASTER_ECU_KEY_M4 = '{8}', MASTER_ECU_KEY_M5 = '{9}', UNLOCK_ECU_KEY_M4 = '{10}', UNLOCK_ECU_KEY_M5 = '{11}' , [LastRecordTime] = '{12}' where EcuId = '{13}'",
                       lockId,
                       ecuIdStruct.RandomSeed, ecuIdStruct.Sha256Total, ecuIdStruct.Sha256Sub20,
                       ecuIdStruct.IdCodeAscii, ecuIdStruct.IdCode, ecuIdStruct.TrackInfo,
                       ecuIdStruct.MacKeyInfo,
                       ecuIdStruct.MasterEcuKeyM4Hex, ecuIdStruct.MasterEcuKeyM5Hex,
                       ecuIdStruct.UnlockEcuKeyM4Hex, ecuIdStruct.UnlockEcuKeyM5Hex,
                       DateTime.Now,
                       ecuIdStruct.EucIdHex);

            try
            {
                var sqlConnectiong = string.Format(@"server={0};database={1};uid={2};pwd={3}", ServerIp, ServerDataBase,
                    ServerUid, ServerPwd);

                using (var connection = new SqlConnection(sqlConnectiong))
                {
                    using (var cmd = new SqlCommand())
                    {
                        PrepareCommand(cmd, connection, null, updateSql, null);
                        var rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void UpdateEcuIdStruct(EcuIdStruct ecuIdStruct)
        {
            var updateSql =
                   string.Format(
                       "update manufactureSgm458PlgDatas set RandomSeed = '{0}', Sha256Total = '{1}', Sha256Sub20 = '{2}', IdCodeAscii = '{3}', IdCode = '{4}', TackInfo = '{5}', MacKey = '{6}', MASTER_ECU_KEY_M4 = '{7}', MASTER_ECU_KEY_M5 = '{8}', UNLOCK_ECU_KEY_M4 = '{9}', UNLOCK_ECU_KEY_M5 = '{10}' , [LastRecordTime] = '{11}' where EcuId = '{12}'",
                       ecuIdStruct.RandomSeed, ecuIdStruct.Sha256Total, ecuIdStruct.Sha256Sub20,
                       ecuIdStruct.IdCodeAscii, ecuIdStruct.IdCode, ecuIdStruct.TrackInfo,
                       ecuIdStruct.MacKeyInfo,
                       ecuIdStruct.MasterEcuKeyM4Hex, ecuIdStruct.MasterEcuKeyM5Hex,
                       ecuIdStruct.UnlockEcuKeyM4Hex, ecuIdStruct.UnlockEcuKeyM5Hex,
                       DateTime.Now,
                       ecuIdStruct.EucIdHex);

            try
            {
                var sqlConnectiong = string.Format(@"server={0};database={1};uid={2};pwd={3}", ServerIp, ServerDataBase,
                    ServerUid, ServerPwd);

                using (var connection = new SqlConnection(sqlConnectiong))
                {
                    using (var cmd = new SqlCommand())
                    {
                        PrepareCommand(cmd, connection, null, updateSql, null);
                        var rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private bool Exists(string strSql, params SqlParameter[] cmdParms)
        {
            var obj = GetSingle(strSql, cmdParms);
            int cmdresult;
            if (Equals(obj, null) || Equals(obj, DBNull.Value))
                cmdresult = 0;
            else
                cmdresult = int.Parse(obj.ToString());
            return cmdresult != 0;
        }

        public object GetSingle(string sqlString, params SqlParameter[] cmdParms)
        {
            var sqlConnectiong = string.Format(@"server={0};database={1};uid={2};pwd={3}", ServerIp, ServerDataBase,
                ServerUid, ServerPwd);

            using (var connection = new SqlConnection(sqlConnectiong))
            {
                using (var cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                        var obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if (Equals(obj, null) || Equals(obj, DBNull.Value))
                            return null;
                        return obj;
                    }
                    catch (SqlException e)
                    {
                        throw e;
                    }
                }
            }
        }

        private static void PrepareCommand(
           SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms == null)
                return;
            foreach (var parameter in cmdParms)
            {
                if ((parameter.Direction == ParameterDirection.InputOutput ||
                     parameter.Direction == ParameterDirection.Input) &&
                    (parameter.Value == null))
                    parameter.Value = DBNull.Value;
                cmd.Parameters.Add(parameter);
            }
        }

        private DataSet Query(string sqlString)
        {
            var sqlConnectiong = string.Format(@"server={0};database={1};uid={2};pwd={3}", ServerIp, ServerDataBase,
                ServerUid, ServerPwd);

            using (var connection = new SqlConnection(sqlConnectiong))
            {
                var ds = new DataSet();
                try
                {
                    connection.Open();
                    var command = new SqlDataAdapter(sqlString, connection);
                    command.Fill(ds, "ds");
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }

        #endregion

        #region 返工相关

        [Description("返工相关-ReadF0F3-ECU_ID")]
        public void OverAgainReadEcuId()
        {
            EcuId = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF";
            EcuId = ValueHelper.GetHextStr(ReadDidViaCan(0xF0, 0xF3)).Replace(" ", "");

            //if (_ecuIdStructList.Any() && !string.IsNullOrEmpty(_ecuIdStructList[DebugEcuIdIndex].EucIdHex) && _ecuIdStructList[DebugEcuIdIndex].EucIdHex == ecuId)
            //    EcuId = "OK " + ecuId;
            //else
            //    EcuId = "NG " + ecuId;
        }

        public void OverAgainGenerateEcuIdAndIdCode()
        {
            GeneratedEcuIdResult = "NG";
            GeneratedWriteIdResult = "NG";

            if (EcuId == "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF" || string.IsNullOrEmpty(EcuId))
            {
                GeneratedWriteEcuIdHex = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF";
                GeneratedWriteIdCodeHex = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF";
                return;
            }

            GeneratedWriteEcuIdHex = EcuId;
            GeneratedWriteIdCodeHex = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF";
            _ecuIdStructList.Clear();

            //if (ds.Tables[0].DefaultView.Count == 1)
            {
                var ecuIdStruct = new EcuIdStruct
                {
                    EucIdHex = GeneratedWriteEcuIdHex,
                    MasterEcuKeySlot = "FF",
                    MasterEcuKeyM1Hex = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF",
                    MasterEcuKeyM2Hex = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF",
                    MasterEcuKeyM3Hex = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF",
                    MasterEcuKeyM4Hex = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF",
                    MasterEcuKeyM5Hex = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF",
                    UnlockEcuKeySlot = "01",
                    UnlockEcuKeyM1Hex = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF",
                    UnlockEcuKeyM2Hex = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF",
                    UnlockEcuKeyM3Hex = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF",
                    UnlockEcuKeyM4Hex = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF",
                    UnlockEcuKeyM5Hex = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF",

                    MacKeyInfo = "NULL"

                    //MasterEcuKeySlot = ds.Tables[0].DefaultView[0]["MASTER_ECU_KEY_KeySlot"].ToString(),
                    //MasterEcuKeyM1Hex = ds.Tables[0].DefaultView[0]["MASTER_ECU_KEY_M1"].ToString(),
                    //MasterEcuKeyM2Hex = ds.Tables[0].DefaultView[0]["MASTER_ECU_KEY_M2"].ToString(),
                    //MasterEcuKeyM3Hex = ds.Tables[0].DefaultView[0]["MASTER_ECU_KEY_M3"].ToString(),
                    //UnlockEcuKeySlot = ds.Tables[0].DefaultView[0]["UNLOCK_ECU_KEY_KeySlot"].ToString(),
                    //UnlockEcuKeyM1Hex = ds.Tables[0].DefaultView[0]["UNLOCK_ECU_KEY_M1"].ToString(),
                    //UnlockEcuKeyM2Hex = ds.Tables[0].DefaultView[0]["UNLOCK_ECU_KEY_M2"].ToString(),
                    //UnlockEcuKeyM3Hex = ds.Tables[0].DefaultView[0]["UNLOCK_ECU_KEY_M3"].ToString(),
                };

                var randomList = new List<byte>();
                for (var i = 0; i < 32; i++)
                {
                    var randomByte = (byte)new Random(Guid.NewGuid().GetHashCode()).Next(0, 256);
                    randomList.Add(randomByte);
                }

                var path = Directory.GetCurrentDirectory() + @"\DllImport\Pbkdf2_sha256.dll";
                var strSeed = Encoding.ASCII.GetString(randomList.ToArray()); //以下strSeed即周晓杨提供的32byte随机数输入

                //注意：由于字符显示的问题，用网上的sha256小软件验证下面GetSHA256方法结果是否正确，网上小软件的输入要从strSeed里面的窗口拷贝，不要拷贝下面Console.WriteLine输出的窗口strSeed内容
                var rt = EcuPasswordDll.GetSha256(strSeed, path);
                var strMaster32 = BitConverter.ToString(rt).Replace("-", "");//经过SHA256算法得到的(32Byte)
                var strMaster20 = BitConverter.ToString(rt).Replace("-", "").Substring(0, 40);//经过SHA256算法得到的(前20Byte)
                var strPbkdf2 = EcuPasswordDll.GetEcuPassword(strMaster20, ecuIdStruct.EucIdHex, 16, path); //经过PBKDF2算法得到的(字符串)
                var bytePbkdf2 = Encoding.ASCII.GetBytes(strPbkdf2);//经过PBKDF2算法得到的(byte)
                var outBytePbkdf2 = BitConverter.ToString(bytePbkdf2).Replace("-", "");
                var strEcuid = ecuIdStruct.EucIdHex;

                ecuIdStruct.IdCode = outBytePbkdf2;
                ecuIdStruct.IdCodeAscii = strPbkdf2;
                ecuIdStruct.RandomSeed = BitConverter.ToString(randomList.ToArray());
                ecuIdStruct.Sha256Total = strMaster32;
                ecuIdStruct.Sha256Sub20 = strMaster20;

                if (strEcuid != "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF" && outBytePbkdf2 != "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF")
                {
                    GeneratedWriteEcuIdHex = strEcuid;
                    GeneratedWriteIdCodeHex = outBytePbkdf2;

                    GeneratedEcuIdResult = "OK " + strEcuid;
                    GeneratedWriteIdResult = "OK " + outBytePbkdf2;

                    _ecuIdStructList.Add(ecuIdStruct);
                }
            }
        }

        public void OverAgainSaveData()
        {
            SaveDataResult = string.Empty;

            if (!_ecuIdStructList.Any())
            {
                SaveDataResult = "NG";
                return;
            }

            var toSave = _ecuIdStructList[DebugEcuIdIndex];

            if (string.IsNullOrEmpty(toSave.EucIdHex))
            {
                SaveDataResult = "NG " + "ECUID" + "缺失";
                return;
            }

            if (string.IsNullOrEmpty(toSave.MasterEcuKeySlot))
            {
                SaveDataResult = "NG " + "MasterEcuKeySlot" + "缺失";
                return;
            }

            if (string.IsNullOrEmpty(toSave.MasterEcuKeyM1Hex))
            {
                SaveDataResult = "NG " + "MasterEcuKeyM1" + "缺失";
                return;
            }

            if (string.IsNullOrEmpty(toSave.MasterEcuKeyM2Hex))
            {
                SaveDataResult = "NG " + "MasterEcuKeyM2" + "缺失";
                return;
            }

            if (string.IsNullOrEmpty(toSave.MasterEcuKeyM3Hex))
            {
                SaveDataResult = "NG " + "MasterEcuKeyM3" + "缺失";
                return;
            }

            if (string.IsNullOrEmpty(toSave.MasterEcuKeyM4Hex))
            {
                SaveDataResult = "NG " + "MasterEcuKeyM4" + "缺失";
                return;
            }

            if (string.IsNullOrEmpty(toSave.MasterEcuKeyM5Hex))
            {
                SaveDataResult = "NG " + "MasterEcuKeyM5" + "缺失";
                return;
            }

            if (string.IsNullOrEmpty(toSave.UnlockEcuKeySlot))
            {
                SaveDataResult = "NG " + "UnlockEcuKeySlot" + "缺失";
                return;
            }

            if (string.IsNullOrEmpty(toSave.UnlockEcuKeyM1Hex))
            {
                SaveDataResult = "NG " + "UnlockEcuKeyM1" + "缺失";
                return;
            }

            if (string.IsNullOrEmpty(toSave.UnlockEcuKeyM2Hex))
            {
                SaveDataResult = "NG " + "UnlockEcuKeyM2" + "缺失";
                return;
            }

            if (string.IsNullOrEmpty(toSave.UnlockEcuKeyM3Hex))
            {
                SaveDataResult = "NG " + "UnlockEcuKeyM3" + "缺失";
                return;
            }

            if (string.IsNullOrEmpty(toSave.UnlockEcuKeyM4Hex))
            {
                SaveDataResult = "NG " + "UnlockEcuKeyM4" + "缺失";
                return;
            }

            if (string.IsNullOrEmpty(toSave.UnlockEcuKeyM5Hex))
            {
                SaveDataResult = "NG " + "UnlockEcuKeyM5" + "缺失";
                return;
            }

            if (string.IsNullOrEmpty(toSave.MacKeyInfo))
            {
                SaveDataResult = "NG " + "MacKey" + "缺失";
                return;
            }

            if (string.IsNullOrEmpty(toSave.RandomSeed))
            {
                SaveDataResult = "NG " + "RandomSeed" + "缺失";
                return;
            }

            if (string.IsNullOrEmpty(toSave.Sha256Total))
            {
                SaveDataResult = "NG " + "经过SHA256算法得到的(32Byte)" + "缺失";
                return;
            }

            if (string.IsNullOrEmpty(toSave.Sha256Sub20))
            {
                SaveDataResult = "NG " + "经过SHA256算法得到的(前20Byte)" + "缺失";
                return;
            }

            if (string.IsNullOrEmpty(toSave.IdCodeAscii))
            {
                SaveDataResult = "NG " + "IdCodeAscii" + "缺失";
                return;
            }

            if (string.IsNullOrEmpty(toSave.IdCode))
            {
                SaveDataResult = "NG " + "IdCode" + "缺失";
                return;
            }

            if (string.IsNullOrEmpty(toSave.TrackInfo))
            {
                SaveDataResult = "NG " + "追溯信息" + "缺失";
                return;
            }

            try
            {
                var existSql = new StringBuilder();

                existSql.Append("select count(1) from manufactureSgm458PlgDatas");
                existSql.Append(" where EcuId=@EcuId ");
                SqlParameter[] parameters =
                                {
                                    new SqlParameter("@EcuId", SqlDbType.NVarChar, 100)
                                };

                parameters[0].Value = toSave.EucIdHex;

                if (!Exists(existSql.ToString(), parameters))
                {
                    var insertSql = string.Format(
                        "insert into manufactureSgm458PlgDatas " +
                        "(EcuId,EcuIdFromFile,CreateTime,IsUsage,IsUploadToSgm," +
                        "MASTER_ECU_KEY_KeySlot,MASTER_ECU_KEY_M1,MASTER_ECU_KEY_M2,MASTER_ECU_KEY_M3," +
                        "UNLOCK_ECU_KEY_KeySlot,UNLOCK_ECU_KEY_M1,UNLOCK_ECU_KEY_M2,UNLOCK_ECU_KEY_M3) " +
                        "values " +
                        "('{0}','{1}','{2}','{3}','{4}'," +
                        "'{5}','{6}','{7}','{8}'," +
                        "'{9}','{10}','{11}','{12}')",
                        toSave.EucIdHex, "返工记录", DateTime.Now, 0, 0,
                        "FF", toSave.MasterEcuKeyM1Hex, toSave.MasterEcuKeyM2Hex,
                        toSave.MasterEcuKeyM3Hex,
                        "01", toSave.UnlockEcuKeyM1Hex, toSave.UnlockEcuKeyM2Hex,
                        toSave.UnlockEcuKeyM3Hex);

                    Query(insertSql);
                }

                var updateSql =
                    string.Format(
                        "update manufactureSgm458PlgDatas set " +
                        "IsUsage = '{0}', " +
                        "RandomSeed = '{1}', Sha256Total = '{2}', Sha256Sub20 = '{3}', " +
                        "IdCodeAscii = '{4}', IdCode = '{5}', TackInfo = '{6}', " +
                        "MacKey = '{7}', " +
                        "MASTER_ECU_KEY_M4 = '{8}', MASTER_ECU_KEY_M5 = '{9}', " +
                        "UNLOCK_ECU_KEY_M4 = '{10}', UNLOCK_ECU_KEY_M5 = '{11}' " +
                        "where EcuId = '{12}'",
                        1,
                        toSave.RandomSeed, toSave.Sha256Total, toSave.Sha256Sub20,
                        toSave.IdCodeAscii, toSave.IdCode, toSave.TrackInfo,
                        toSave.MacKeyInfo,
                        toSave.MasterEcuKeyM4Hex, toSave.MasterEcuKeyM5Hex,
                        toSave.UnlockEcuKeyM4Hex, toSave.MasterEcuKeyM5Hex,
                        toSave.EucIdHex);

                var sqlConnectiong = string.Format(@"server={0};database={1};uid={2};pwd={3}", ServerIp, ServerDataBase,
                    ServerUid, ServerPwd);


                using (var connection = new SqlConnection(sqlConnectiong))
                {
                    using (var cmd = new SqlCommand())
                    {
                        PrepareCommand(cmd, connection, null, updateSql, null);
                        var rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        SaveDataResult = rows == 1 ? "OK" : "NG";
                    }
                }
            }
            catch (Exception)
            {
                SaveDataResult = "NG";
            }
        }

        #endregion

        #region 生成二维码

        [Description("R,生成二维码内容")]
        public string GeneratedBarcode;

        [Description("生成二维码内容")]
        public void GenerateBarcode(string generalPartNo, string generalVpps, string seeyaoDuns)
        {
            GeneratedBarcode = string.Empty;

            //generalPartNo = "26467265";
            //generalVpps = "8210700000000X";
            //seeyaoDuns = "547656863";
            //TrackInfo = "OK 1A22238AAA000005";

            if (string.IsNullOrEmpty(generalPartNo) || generalPartNo.Length != 8)
                return;

            if (string.IsNullOrEmpty(generalVpps) || generalVpps.Length != 14)
                return;

            if (string.IsNullOrEmpty(seeyaoDuns) || seeyaoDuns.Length != 9)
                return;

            if (string.IsNullOrEmpty(TrackInfo) || !TrackInfo.StartsWith("OK "))
                return;

            var tracInfo = TrackInfo.Replace("OK ", "");

            var matrixBytes = new List<byte>();
            matrixBytes.AddRange(new byte[] { 0x5B, 0x29, 0x3E, 0x1E, 0x30, 0x36, 0x1D, 0x59 }); // header:[)><RS>06<GS>Y
            matrixBytes.AddRange(Encoding.ASCII.GetBytes(generalVpps)); // 通用VPPS号
            matrixBytes.Add(0x1D); //<GS>
            matrixBytes.AddRange(Encoding.ASCII.GetBytes("P" + generalPartNo)); //  P+通用零件号
            matrixBytes.Add(0x1D); //<GS>
            matrixBytes.AddRange(Encoding.ASCII.GetBytes("12V" + seeyaoDuns)); //  12V+DUNS邓氏码
            matrixBytes.Add(0x1D); //<GS>
            matrixBytes.AddRange(Encoding.ASCII.GetBytes("T" + tracInfo)); //  追溯码
            matrixBytes.AddRange(new byte[] { 0x1E, 0x04 }); // trailer <RS>

            GeneratedBarcode = matrixBytes.ToArray().GetStringByAsciiBytes(false);

            //var generatedBarcode = matrixBytes.ToArray().GetStringByAsciiBytes(true);
            //Console.WriteLine(generatedBarcode);

            //var getTrackInfo = GeneratedBarcode.Substring(GeneratedBarcode.Length - 16 - 2, 16);
        }

        #endregion

        #region 导出XML相关

        public bool PrintToXml(string printXmlFilePath, string sql, out int count)
        {
            try
            {
                // var selectSql =
                //"SELECT [id],[EcuId],[EcuIdFromFile],[TackInfo],[CreateTime],[LastRecordTime],[IsUsage],[IsUploadToSgm],[RandomSeed],[Sha256Total],[Sha256Sub20],[IdCodeAscii],[IdCode],[MASTER_ECU_KEY_KeySlot],[MASTER_ECU_KEY_M1],[MASTER_ECU_KEY_M2],[MASTER_ECU_KEY_M3],[MASTER_ECU_KEY_M4],[MASTER_ECU_KEY_M5],[UNLOCK_ECU_KEY_KeySlot],[UNLOCK_ECU_KEY_M1],[UNLOCK_ECU_KEY_M2],[UNLOCK_ECU_KEY_M3],[UNLOCK_ECU_KEY_M4],[UNLOCK_ECU_KEY_M5],[MacKey] FROM [IPMS].[dbo].[manufactureSgm458PlgDatas] where [IsUsage] = '1' and [LastRecordTime] > '2022/11/15'";

                var ds = Query(sql);
                var plgKey = new ecuRecordList { ecuRecord = new ecuRecordListEcuRecord[ds.Tables[0].DefaultView.Count] };

                for (var i = 0; i < ds.Tables[0].DefaultView.Count; i++)
                {
                    var ecuIdStruct = new EcuIdStruct
                    {
                        EucIdHex = ds.Tables[0].DefaultView[i]["EcuId"].ToString(),
                        MasterEcuKeySlot = ds.Tables[0].DefaultView[i]["MASTER_ECU_KEY_KeySlot"].ToString(),
                        MasterEcuKeyM1Hex = ds.Tables[0].DefaultView[i]["MASTER_ECU_KEY_M1"].ToString(),
                        MasterEcuKeyM2Hex = ds.Tables[0].DefaultView[i]["MASTER_ECU_KEY_M2"].ToString(),
                        MasterEcuKeyM3Hex = ds.Tables[0].DefaultView[i]["MASTER_ECU_KEY_M3"].ToString(),
                        MasterEcuKeyM4Hex = ds.Tables[0].DefaultView[i]["MASTER_ECU_KEY_M4"] == null ? string.Empty : ds.Tables[0].DefaultView[i]["MASTER_ECU_KEY_M4"].ToString(),
                        MasterEcuKeyM5Hex = ds.Tables[0].DefaultView[i]["MASTER_ECU_KEY_M5"] == null ? string.Empty : ds.Tables[0].DefaultView[i]["MASTER_ECU_KEY_M5"].ToString(),
                        UnlockEcuKeySlot = ds.Tables[0].DefaultView[i]["UNLOCK_ECU_KEY_KeySlot"].ToString(),
                        UnlockEcuKeyM1Hex = ds.Tables[0].DefaultView[i]["UNLOCK_ECU_KEY_M1"].ToString(),
                        UnlockEcuKeyM2Hex = ds.Tables[0].DefaultView[i]["UNLOCK_ECU_KEY_M2"].ToString(),
                        UnlockEcuKeyM3Hex = ds.Tables[0].DefaultView[i]["UNLOCK_ECU_KEY_M3"].ToString(),
                        UnlockEcuKeyM4Hex = ds.Tables[0].DefaultView[i]["UNLOCK_ECU_KEY_M4"] == null ? string.Empty : ds.Tables[0].DefaultView[i]["UNLOCK_ECU_KEY_M4"].ToString(),
                        UnlockEcuKeyM5Hex = ds.Tables[0].DefaultView[i]["UNLOCK_ECU_KEY_M5"] == null ? string.Empty : ds.Tables[0].DefaultView[i]["UNLOCK_ECU_KEY_M5"].ToString(),
                        MacKeyInfo = ds.Tables[0].DefaultView[i]["MacKey"] == null ? string.Empty : ds.Tables[0].DefaultView[i]["MacKey"].ToString(),
                        RandomSeed = ds.Tables[0].DefaultView[i]["RandomSeed"] == null ? string.Empty : ds.Tables[0].DefaultView[i]["RandomSeed"].ToString(),
                        Sha256Total = ds.Tables[0].DefaultView[i]["Sha256Total"] == null ? string.Empty : ds.Tables[0].DefaultView[i]["Sha256Total"].ToString(),
                        Sha256Sub20 = ds.Tables[0].DefaultView[i]["Sha256Sub20"] == null ? string.Empty : ds.Tables[0].DefaultView[i]["Sha256Sub20"].ToString(),
                        IdCodeAscii = ds.Tables[0].DefaultView[i]["IdCodeAscii"] == null ? string.Empty : ds.Tables[0].DefaultView[i]["IdCodeAscii"].ToString(),
                        IdCode = ds.Tables[0].DefaultView[i]["IdCode"] == null ? string.Empty : ds.Tables[0].DefaultView[i]["IdCode"].ToString(),
                        TrackInfo = ds.Tables[0].DefaultView[i]["IdCode"] == null ? string.Empty : ds.Tables[0].DefaultView[i]["TackInfo"].ToString(),
                    };

                    plgKey.ecuRecord[i] = new ecuRecordListEcuRecord
                    {
                        ecuid = Convert.ToBase64String(GetByteArray(ecuIdStruct.EucIdHex, false)),
                        serialNo =
                            ValueHelper.GetHextStr(Encoding.ASCII.GetBytes(ecuIdStruct.TrackInfo)).Replace(" ", ""),
                        mkm4 = Convert.ToBase64String(GetByteArray(ecuIdStruct.MasterEcuKeyM4Hex, false)),
                        mkm5 = Convert.ToBase64String(GetByteArray(ecuIdStruct.MasterEcuKeyM5Hex, false)),
                        ukm4 = Convert.ToBase64String(GetByteArray(ecuIdStruct.UnlockEcuKeyM4Hex, false)),
                        ukm5 = Convert.ToBase64String(GetByteArray(ecuIdStruct.UnlockEcuKeyM5Hex, false))
                    };
                }

                XmlHelper.SerializeToFile(plgKey, printXmlFilePath, Encoding.UTF8);
                count = plgKey.ecuRecord.Length;
                return true;
            }
            catch (Exception)
            {
                count = 0;
                return false;
            }
        }

        private static byte[] GetByteArray(string hexArray, bool withSpace = true)
        {
            string[] array;
            if (withSpace)
                array = hexArray.Split(' ');
            else
            {
                var temp = new List<string>();
                for (var i = 0; i < hexArray.Length; i += 2)
                {
                    temp.Add(new string(hexArray.Skip(i).Take(2).ToArray()));
                }
                array = temp.ToArray();
            }
            return array.Select(hex => Convert.ToByte(hex, 16)).ToArray();
        }

        #endregion
    }
}
