using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace Controller
{
    [Description("LIN-Product,出风口执行器")]
    public sealed class AirOutletAutomotiveActuator : ControllerBase
    {
        public LinBus Lin;

        [Description("R,更改NAD结果")]
        public string ChangeNadResult = string.Empty;

        public AirOutletAutomotiveActuator(string name) : base(name)
        {
            for (var i = 0; i < 16; i++)
                _slaves.Add(new Slave { MasterLinId = (byte)i, SlaveLinId = (byte)(i + 0x10) });
            //AddSlave("0x02", "0x21");
            //AddSlave("0x01", "0x21");
            LinBus.PushLinMsg += LinBus_PushLinMsg;
            MainWork();
        }

        private void LinBus_PushLinMsg(string name, LinBus.LinDataPackage data)
        {
            if (Lin is null)
                return;

            if (string.Equals(Lin.Name, name, StringComparison.CurrentCultureIgnoreCase) && data != null)
            {
                if (data.LinDataLen != 8)
                    return;

                if (data.LinData.All(f => f == 0x00))
                    return;

                var linId = data.LinId;
                var slaveIndex = _slaves.FindIndex(f => (f.SlaveLinId == linId || LinBus.ConvertLinId(f.SlaveLinId) == linId) && f.BindingNad == data.LinData[6]);
                if (slaveIndex != -1)
                {
                    _slaves[slaveIndex].CopyRecvByte(data.LinData);
                }
            }
        }

        [Description("START_LIN")]
        public void StartLin() => _isStartLin = true;

        [Description("STOP_LIN")]
        public void StopLin() => _isStartLin = false;

        [Description("堵转使能-Stall_detection_off")]
        public void Stall_detection_off(byte nad) => SetStallDectection(nad, 0);

        [Description("堵转使能-Stall_detection_on")]
        public void Stall_detection_on(byte nad) => SetStallDectection(nad, 1);

        private void SetStallDectection(byte nad, byte value)
        {
            var findIndex = _slaves.FindIndex(f => f.BindingNad == nad);
            if (findIndex != -1)
                _slaves[findIndex].SetStallDetection(value);
        }

        [Description("清除事件标志-Do_not_reset_event_flags")]
        public void Do_not_reset_event_flags(byte nad) => ClearEvent(nad, 0);

        [Description("清除事件标志-Clear_Emergency_run_occurred")]
        public void Clear_Emergency_run_occurred(byte nad) => ClearEvent(nad, 2);

        [Description("清除事件标志-Clear_Stall_occurred")]
        public void Clear_Stall_occurred(byte nad) => ClearEvent(nad, 4);

        [Description("清除事件标志-Clear_Reset")]
        public void Clear_Reset(byte nad) => ClearEvent(nad, 8);

        private void ClearEvent(byte nad, byte value)
        {
            var findIndex = _slaves.FindIndex(f => f.BindingNad == nad);
            if (findIndex != -1)
                _slaves[findIndex].ClearEvent(value);
        }

        [Description("有效位置选择-Set_position_valid")]
        public void Set_position_valid(byte nad) => SetValidPosition(nad, 0);

        [Description("有效位置选择-Start_position_valid")]
        public void Start_position_valid(byte nad) => SetValidPosition(nad, 1);

        [Description("有效位置选择-No_position_specification_valid")]
        public void No_position_specification_valid(byte nad) => SetValidPosition(nad, 2);

        private void SetValidPosition(byte nad, byte value)
        {
            var findIndex = _slaves.FindIndex(f => f.BindingNad == nad);
            if (findIndex != -1)
                _slaves[findIndex].SetValidPosition(value);
        }

        [Description("速度设置-Speed_level_1__2_25_rpm")]
        public void Speed_level_1__2_25_rpm(byte nad) => SetSpeedLevel(nad, 1);

        [Description("速度设置-Speed_level_2_2_25_rpm_LessThan_x__3_0_rpm")]
        public void Speed_level_2_2_25_rpm_LessThan_x__3_0_rpm(byte nad) => SetSpeedLevel(nad, 2);

        [Description("速度设置-Speed_level_3_3_0_rpm_LessThan_x__4_0_rpm")]
        public void Speed_level_3_3_0_rpm_LessThan_x__4_0_rpm(byte nad) => SetSpeedLevel(nad, 3);

        [Description("速度设置-Speed_level_4_4_0_rpm_LessThan_x")]
        public void Speed_level_4_4_0_rpm_LessThan_x(byte nad) => SetSpeedLevel(nad, 4);

        [Description("速度设置-Auto_Speed")]
        public void Auto_Speed(byte nad) => SetSpeedLevel(nad, 5);

        private void SetSpeedLevel(byte nad, byte value)
        {
            var findIndex = _slaves.FindIndex(f => f.BindingNad == nad);
            if (findIndex != -1)
                _slaves[findIndex].SetSpeedLevel(value);
        }

        [Description("设置目标位置")]
        public void Set_Pos_65535_Signal(byte nad, ushort value)
        {
            var findIndex = _slaves.FindIndex(f => f.BindingNad == nad);
            if (findIndex != -1)
                _slaves[findIndex].SetTargetPosition(value);
        }

        [Description("设置起始位置")]
        public void Sta_Pos_65535_Signal(byte nad, ushort value)
        {
            var findIndex = _slaves.FindIndex(f => f.BindingNad == nad);
            if (findIndex != -1)
                _slaves[findIndex].SetStartPosition(value);
        }

        [Description("紧急使能操作-Emergency_run_close")]
        public void Emergency_run_close(byte nad) => SetSetEmergency(nad, 0);

        [Description("紧急使能操作-Emergency_run_release")]
        public void Emergency_run_release(byte nad) => SetSetEmergency(nad, 1);

        private void SetSetEmergency(byte nad, byte value)
        {
            var findIndex = _slaves.FindIndex(f => f.BindingNad == nad);
            if (findIndex != -1)
                _slaves[findIndex].SetEmergency(value);
        }

        [Description("紧急位置选择-Lower_stop")]
        public void Lower_stop(byte nad) => SetSetEmergency(nad, 0);

        [Description("紧急位置选择-Upper_stop")]
        public void Upper_stop(byte nad) => SetSetEmergencyPosition(nad, 1);

        private void SetSetEmergencyPosition(byte nad, byte value)
        {
            var findIndex = _slaves.FindIndex(f => f.BindingNad == nad);
            if (findIndex != -1)
                _slaves[findIndex].SetEmergencyPosition(value);
        }

        [Description("旋转方向选择-Rot_Dir_CW")]
        public void Rot_Dir_CW(byte nad) => SetRotationDirection(nad, 0);

        [Description("旋转方向选择-Rot_Dir_CCW")]
        public void Rot_Dir_CCW(byte nad) => SetRotationDirection(nad, 1);

        private void SetRotationDirection(byte nad, byte value)
        {
            var findIndex = _slaves.FindIndex(f => f.BindingNad == nad);
            if (findIndex != -1)
                _slaves[findIndex].SetRotationDirection(value);
        }

        [Description("启停选择-Normal_Mode")]
        public void Normal_Mode(byte nad) => SetMode(nad, 0);

        [Description("启停选择-Stop_Mode")]
        public void Stop_Mode(byte nad) => SetMode(nad, 1);

        private void SetMode(byte nad, byte value)
        {
            var findIndex = _slaves.FindIndex(f => f.BindingNad == nad);
            if (findIndex != -1)
                _slaves[findIndex].SetMode(value);
        }

        [Description("添加一个子节点")]
        public void AddSlave(string linIdHex, string nadHex)
        {
            try
            {
                var linId = Convert.ToByte(linIdHex, 16);
                var nad = Convert.ToByte(nadHex, 16);

                var findIndex = _slaves.FindIndex(f => f.MasterLinId == linId);
                if (findIndex != -1)
                {
                    _slaves[findIndex].SetNad(nad);
                    _slaves[findIndex].BindingNad = nad;
                    _slaves[findIndex].IsOnBus = true;
                }
            }
            catch (Exception)
            {

            }
        }

        [Description("移除一个子节点")]
        public void RemoveSlave(string linIdHex, string nadHex)
        {
            try
            {
                var linId = Convert.ToByte(linIdHex, 16);
                var nad = Convert.ToByte(nadHex, 16);

                var findIndex = _slaves.FindIndex(f => f.MasterLinId == linId);
                if (findIndex != -1)
                {
                    _slaves[findIndex].BindingNad = 0x00;
                    _slaves[findIndex].IsOnBus = false;
                }
            }
            catch (Exception)
            {
            }
        }

        [Description("读APP/BOOT")]
        public void ReadAppBoot(byte nad)
        {
            if (Lin is null)
                return;

            lock (_lockLinSend)
            {
                byte[] echo;
                if (Lin.SendMasterLinAndRecvSingleSlaveLin(0x3C, 0x3D, new byte[] { nad, 0x06, 0xB2, 0x03, 0x7F, 0x01, 0x01, 0x00 }, out echo))
                {
                    if (echo.Length == 8)
                    {
                        var appBytes = new byte[] { echo[3], echo[4] };
                        var bootBytes = new byte[] { echo[5], echo[6] };
                    }
                    //Console.WriteLine
                }
            }
        }

        [Description("改NAD")]
        public void ChangeNad(string srcNadHex, string tgrtNadHex)
        {
            ChangeNadResult = string.Empty;

            try
            {
                var srcNad = Convert.ToByte(srcNadHex, 16);
                var tgrtNad = Convert.ToByte(tgrtNadHex, 16);

                var bs = new byte[] { srcNad, 0x60, 0xB0, 0x7F, 0x01, 0x01, 0x00, tgrtNad };

                lock (_lockLinSend)
                {
                    //byte[] echo;
                    //if (Lin.SendMasterLinAndRecvSingleSlaveLin(0x3C, 0x3D, bs, out echo))
                    //{

                    //}
                }
            }
            catch (Exception)
            {
            }
        }

        private readonly object _lockLinSend = new object();
        private bool _isStartLin;
        public List<Slave> _slaves { get; set; } = new List<Slave>();

        private void MainWork()
        {
            for (var i = 0; i <= 15; i++)
            {
                ICC_ICC_LIN2_Pr_20ms((byte)i);
                ICC_LIN2_FrP00((byte)i);
            }
            SchedulerAsync();
        }

        private void ICC_ICC_LIN2_Pr_20ms(byte id)
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = SendMasterLinMsg(() =>
                {
                    var slaveIndex = _slaves.FindIndex(f => f.MasterLinId == id && f.IsOnBus);
                    if (slaveIndex == -1)
                        return null;
                    if (_slaves[slaveIndex].IsDisable)
                        return null;
                    return new LinBus.LinDataPackage(_slaves[slaveIndex].MasterLinId, _slaves[slaveIndex].SendBytes);
                }, false),
                Interval = 20
            });
        }

        private void ICC_LIN2_FrP00(byte id)
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = SendMasterLinMsg(() =>
                {
                    var slaveIndex = _slaves.FindIndex(f => f.MasterLinId == id && f.IsOnBus);
                    if (slaveIndex == -1)
                        return null;
                    if (_slaves[slaveIndex].IsDisable)
                        return null;
                    return new LinBus.LinDataPackage(_slaves[slaveIndex].SlaveLinId, new byte[0]);
                }, false),
                Interval = 20
            });
        }

        private Action SendMasterLinMsg(Func<LinBus.LinDataPackage> date, bool isWait = true)
        {
            return () =>
            {
                if (Lin is null)
                    return;

                if (!_isStartLin)
                    return;

                var msg = date.Invoke();
                if (msg is null)
                    return;

                lock (_lockLinSend)
                    Lin.SendMasterLin(msg.LinId, msg.LinData, isWait);
            };
        }

        [Description("自动寻址")]
        public void AutomaticAddressing(int ms)
        {
            if (Lin is null)
                return;

            var tp = _isStartLin;

            if (_isStartLin)
                StopLin();

            var delayMs = ms;

            DelayMs(7500);

            for (int i = 0; i < 5; i++)
            {
                Lin.SendMasterLin(0x3C, new byte[] { 0x7F, 0x06, 0xB5, 0xFF, 0x7F, 0x01, 0xF1, 0xFF });
                DelayMs(200);
            }

            Lin.SendMasterLin(0x3C, new byte[] { 0x7F, 0x06, 0xB5, 0xFF, 0x7F, 0x01, 0xF1, 0xFF });
            DelayMs(500);
            for (var i = 0x20; i <= 0x2F; i++)
            {
                Lin.SendMasterLin(0x3C, new byte[] { 0x7F, 0x06, 0xB5, 0xFF, 0x7F, 0x02, 0xF1, (byte)(i) });
                DelayMs(delayMs);
            }

            Lin.SendMasterLin(0x3C, new byte[] { 0x7F, 0x06, 0xB5, 0xFF, 0x7F, 0x03, 0xF1, 0xFF });
            DelayMs(500);
            Lin.SendMasterLin(0x3C, new byte[] { 0x7F, 0x06, 0xB5, 0xFF, 0x7F, 0x04, 0xF1, 0xFF });
            DelayMs(500);

            for (var i = 0x20; i <= 0x2F; i++)
            {
                Lin.SendMasterLin(0x3C, new byte[] { (byte)(i), 0x06, 0xB2, 0x01, 0xFF, 0x7F, 0xFF, 0xFF });
                DelayMs(100);

                for (int j = 0; j < 1; j++)
                {
                    byte[] echo;
                    if (Lin.SendSlaveLin(0x3D, out echo, timeOutMs: 250))
                    {
                        if (echo != null && echo.Length == 8 && echo[0] == (byte)i)
                        {
                            break;
                        }
                    }
                    Thread.Sleep(25);
                }

                //Lin.SendMasterLin(0x3D, new byte[0]);
                //DelayMs(100);
                //Lin.SendMasterLin(0x3D, new byte[0]);
                DelayMs(delayMs);
            }

            DelayMs(3500);
            _isStartLin = tp;
        }

        private void DelayMs(int ms)
        {
            var enterTime = HighPrecisionTimer.GetTimestamp();

            while (true)
            {
                Thread.Sleep(1);

                var endTime = HighPrecisionTimer.GetTimestamp();
                var ts = HighPrecisionTimer.GetTimestampIntervalMs(enterTime, endTime);
                if (ts >= ms)
                    break;
            }
        }

        private const string Ctr_Add_61_l = "Ctr_Add_61_l";
        private const string Ctr_Sav_P_D_61_l = "Ctr_Sav_P_D_61_l";
        private const string Ctr_Stall_D_R_61_l = "Ctr_Stall_D_R_61_l";
        private const string Ctr_Clr_E_F_61_l = "Ctr_Clr_E_F_61_l";
        private const string Ctr_S_Pos_S_61_l = "Ctr_S_Pos_S_61_l";
        private const string Ctr_Spe_S_61_l = "Ctr_Spe_S_61_l";
        private const string Ctr_Set_Pos_61_l = "Ctr_Set_Pos_61_l";
        private const string Ctr_Sta_Pos_61_l = "Ctr_Sta_Pos_61_l";
        private const string Ctr_E_R_R_61_l = "Ctr_E_R_R_61_l";
        private const string Ctr_E_R_P_61_l = "Ctr_E_R_P_61_l";
        private const string Ctr_Rot_Dir_61_l = "Ctr_Rot_Dir_61_l";
        private const string Ctr_Sto_M_61_l = "Ctr_Sto_M_61_l";

        public class Slave
        {
            public bool IsDisable { get; set; }

            public byte MasterLinId { get; set; }
            public byte SlaveLinId { get; set; }
            public byte BindingNad { get; set; }
            public bool IsOnBus { get; set; }
            public byte[] SendBytes { get; private set; }
            public byte[] RecvBytes { get; private set; }
            public long RecvTs { get; private set; }

            public ushort CaliStallA { get; set; }
            public ushort CaliStallB { get; set; }

            public ushort TargetPos { get; private set; }
            public ushort ActualPos { get; private set; }
            public byte Stall_D_R { get; private set; }

            public byte Stall_O { get; private set; }
            public byte Rot_Dir { get; private set; }
            public byte Spe_S { get; private set; }
            public byte Mode { get; private set; }

            public byte R_Err { get; private set; }
            public byte O_T { get; private set; }
            public byte Ele_D { get; private set; }
            public byte Sup_V { get; private set; }
            public byte Eme_R_O { get; private set; }
            public byte Res { get; private set; }

            public Slave()
            {
                RecvBytes = new byte[8];
                SendBytes = new byte[8];
                FormatBytes(16, 2, 3);
                //SetMode(1);
                //SetValidPosition(1);
                //SetStartPosition(32000);
                //SetStallDetection(1);
            }

            public void CopyRecvByte(byte[] data)
            {
                RecvTs = HighPrecisionTimer.GetTimestamp();
                Array.Copy(data, RecvBytes, 8);
                ActualPos = (ushort)BytesToBits(24, 16, data);
                Stall_O = (byte)BytesToBits(12, 2, data);
                Rot_Dir = (byte)BytesToBits(60, 2, data);
                Spe_S = (byte)BytesToBits(20, 4, data);
                Mode = (byte)BytesToBits(62, 2, data);

                R_Err = (byte)BytesToBits(0, 1, data);
                O_T = (byte)BytesToBits(2, 2, data);
                Ele_D = (byte)BytesToBits(4, 2, data);
                Sup_V = (byte)BytesToBits(6, 2, data);
                Eme_R_O = (byte)BytesToBits(8, 2, data);
                Res = (byte)BytesToBits(14, 2, data);
            }

            [Description("NAD")]
            public void SetNad(byte nad)
            {
                BindingNad = nad;
                FormatBytes(0, 8, nad);
            }

            [Description("数据存储")]
            public void SaveProgrammingData(byte mode)
            {
                if (mode >= 0 && mode <= 3)
                    FormatBytes(8, 2, mode);
            }

            [Description("堵转使能")]
            public void SetStallDetection(byte value)
            {
                if (value >= 0 && value <= 3)
                {
                    FormatBytes(10, 2, value);
                    Stall_D_R = value;
                }
            }

            [Description("清除事件标志")]
            public void ClearEvent(int value)
            {
                if (value == 0 || value == 2 || value == 4 || value == 8 || value == 15 || value == 14)
                    FormatBytes(12, 4, value);
            }

            [Description("有效位置选择")]
            public void SetValidPosition(byte value)
            {
                if (value >= 0 && value <= 3)
                    FormatBytes(18, 2, value);
            }

            [Description("速度设置")]
            public void SetSpeedLevel(byte value)
            {
                if (value >= 0 && value <= 15)
                    FormatBytes(20, 4, value);
            }

            [Description("设置目标位置")]
            public void SetTargetPosition(ushort value)
            {
                if (value >= 0 && value <= 65535)
                {
                    FormatBytes(24, 16, value);
                    TargetPos = value;
                }
            }

            [Description("设置起始位置")]
            public void SetStartPosition(ushort value)
            {
                if (value >= 0 && value <= 65535)
                    FormatBytes(40, 16, value);
            }

            [Description("紧急使能操作")]
            public void SetEmergency(byte value)
            {
                if (value >= 0 && value <= 3)
                    FormatBytes(56, 2, value);
            }

            [Description("紧急位置选择")]
            public void SetEmergencyPosition(byte value)
            {
                if (value >= 0 && value <= 3)
                    FormatBytes(58, 2, value);
            }

            [Description("旋转方向选择")]
            public void SetRotationDirection(byte value)
            {
                if (value >= 0 && value <= 3)
                    FormatBytes(60, 2, value);
            }

            [Description("启停选择")]
            public void SetMode(byte value)
            {
                if (value >= 0 && value <= 3)
                    FormatBytes(62, 2, value);
            }

            private void FormatBytes(int startBit, int len, int value)
            {
                var allBits = ValueHelper.GetBits(SendBytes);
                var bits = Convert.ToString(value, 2).PadLeft(len, '0');
                var listBits = new List<string>();
                for (var i = bits.Length - 1; i >= 0; i--)
                    listBits.Add(bits[i].ToString());
                var bIndex = 0;
                for (var i = startBit; i < startBit + len; i++)
                {
                    allBits[i] = listBits[bIndex];
                    bIndex++;
                }

                var byteIndex = 0;
                for (var i = 0; i < allBits.Length; i = i + 8)
                {
                    var tp = new string[8];
                    Array.Copy(allBits, i, tp, 0, 8);
                    var b = Convert.ToByte(string.Join("", tp.ToArray().Reverse()), 2);
                    SendBytes[byteIndex] = b;
                    byteIndex++;
                }
            }

            private int BytesToBits(int startBit, int len, byte[] value)
            {
                var allBits = ValueHelper.GetBits(value);
                var targetBits = new string[len];
                Array.Copy(allBits, startBit, targetBits, 0, len);
                return Convert.ToInt32(string.Join("", targetBits.ToArray().Reverse()), 2);
            }
        }
    }
}
