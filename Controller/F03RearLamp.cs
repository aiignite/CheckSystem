using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Controller
{
    [Description("CAN-Product,F03尾灯")]
    public sealed class F03RearLamp : ControllerBase
    {
        public CanBus CanFD;

        public F03RearLamp(string name) : base(name)
        {
            {
                //// 0x379: b7 b1 73 01 00 00 01 40 00 00 00 00 00 00 00 00 00 00 00 00 57 2e 5c a0 00 00 00 ca 80 00 00 00
                //// 0x379: 29 42 74 01 00 00 01 40 00 00 00 00 00 00 00 00 00 00 00 00 57 2e 5c a0 00 00 00 ca 80 00 00 00
                ////var toCheckCrc = new byte[] { 0xB1, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0A, 0x72, 0x72, 0x07, 0x01, 0x02, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x03, 0x79,0x43 };
                //var toCheckCrc = new byte[] { 0x73, 0x01, 0x00, 0x00, 0x01, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x57, 0x2e, 0x5c, 0xa0, 0x00, 0x00, 0x00, 0xca, 0x80, 0x00, 0x00, 0x00, 0x79, 0x43 };
                //var crcBytes = BitConverter.GetBytes(CALC_CRC(_crcInfo, toCheckCrc));
                //var byte0 = crcBytes[1];
                //var byte1 = crcBytes[0];
            }

            //WriteF18C("zhjtest001", "0000", "VG350", 1);

            #region 用dll计算SeedKey
            //{
            //    // 1057: [67 01 E4 11 50 9D CC 6E 60 B4 0E 60 47 5A 7A 89 B8 F5]
            //    // 1989: [27 02 10 F1 BB 86 A6 EF DF 59 45 70 71 B7 A2 4D 2C 0C]
            //    var seedBytes = new byte[16] { 0xe4, 0x11, 0x50, 0x9d, 0xcc, 0x6e, 0x60, 0xb4, 0x0e, 0x60, 0x47, 0x5a, 0x7a, 0x89, 0xb8, 0xf5 };
            //    var keyBytes = new byte[16];
            //    int keyLen;
            //    CalcKey.GenerateKeyEx(seedBytes, 16, 0x01, null, keyBytes, 16, out keyLen);
            //}
            #endregion

            #region AES-CMAC计算CMAC
            //var keyStr = "2b7e151628aed2a6abf7158809cf4f3c";
            //var dataStr = "6bc1bee22e409f96e93d7e117393172a";

            //var key = new List<byte>();
            //var data = new List<byte>();
            //for (var i = 0; i < keyStr.Length; i = i + 2)
            //    key.Add(Convert.ToByte(keyStr.Substring(i, 2), 16));
            //for (var i = 0; i < dataStr.Length; i = i + 2)
            //    data.Add(Convert.ToByte(dataStr.Substring(i, 2), 16));

            //byte[] cmacResult = ComputeCMAC(key.ToArray(), data.ToArray());
            //Console.WriteLine(BitConverter.ToString(cmacResult).Replace("-", ""));
            #endregion

            VIU_Ctrl2_0x379(); // E2E Data ID:0x4379
            VIU_Info_0x1D2(); // E2E Data ID: 0x41D2
            VIU_LDM_Ctrl_0x15C(); // E2E Data ID:0x415C
            VIU_Route_LCU_0x3BA();
            VIU1_0x3AE();
            NM_Autosar_VIU2_LCU_0x454();
            SchedulerAsync();
            CanBus.PushCanMsg += CanBus_PushCanMsg;
        }

        private void CanBus_PushCanMsg(string name, CanBus.CanDataPackage data, CanBus.OnPushCanDataType onPushCanDataType)
        {
            if (CanFD is null)
                return;

            if (CanFD.Name != name)
                return;

            if (data is null || data.CanData.Length < 8)
                return;

            if (data.CanId == DiagnosticRecvCanId)
            {
                //Console.WriteLine("rx 0x14DAF1BE: " + ValueHelper.GetHextStr(data.CanData));

                if (_bReadDid)
                {
                    _uds22Buffer.AddRange(data.CanData);
                    _bReadDidWaitHandle.Set();
                }

                if (_bSeedKeySubFunc)
                {
                    _uds27Buffer.AddRange(data.CanData);
                    _bseedKeyWaitHandle.Set();
                }
            }
        }

        ~F03RearLamp() => Dispose();

        private bool _isCANStart;

        [Description("打开CAN")]
        public void StartCAN() => _isCANStart = true;

        [Description("关闭CAN")]
        public void StopCAN() => _isCANStart = false;

        #region 点灯周期报文

        private readonly object _lockSend = new object();
        private byte[] _viu_ctrl2_0x379 = new byte[32];
        private byte[] _viu_info_0x1D2 = new byte[8];
        private byte[] _viu_ldm_ctrl_0x15C = new byte[32];
        private byte[] _viu_route_lcu_0x3BA = new byte[32];
        private byte[] _viu1_0X3AE = new byte[8];
        private byte[] _nm_autosar_viu2_lcu_0x454 = new byte[8];

        private void VIU_Ctrl2_0x379()
        {
            // E2E Data ID:0x4379
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = SendNetWorkCanFD(() =>
                {
                    // stop
                    FormatData(ref _viu_ctrl2_0x379, 50, 2, _leftStopCmd);
                    FormatData(ref _viu_ctrl2_0x379, 48, 2, _rightStopCmd);
                    FormatData(ref _viu_ctrl2_0x379, 62, 2, _middleLeftStopCmd);
                    FormatData(ref _viu_ctrl2_0x379, 60, 2, _middleRightStopCmd);
                    // bul
                    FormatData(ref _viu_ctrl2_0x379, 58, 2, _middleLeftBulCmd);
                    FormatData(ref _viu_ctrl2_0x379, 56, 2, _middleRightBulCmd);
                    // ads
                    FormatData(ref _viu_ctrl2_0x379, 231, 2, _rearADSCmd);
                    FormatData(ref _viu_ctrl2_0x379, 217, 7, _rearADSBritSet);
                    FormatData(ref _viu_ctrl2_0x379, 240, 4, _rearADSEffectCtrlCmd);
                    FormatData(ref _viu_ctrl2_0x379, 244, 4, _rearADSEffectSelect);
                    FormatData(ref _viu_ctrl2_0x379, 248, 2, _rearADSEffectModeSet);
                    // 贯穿灯
                    FormatData(ref _viu_ctrl2_0x379, 204, 2, _rearRibbonEffectModeSet);
                    FormatData(ref _viu_ctrl2_0x379, 144, 4, _rearRibbonEffectCtrlCmd);
                    FormatData(ref _viu_ctrl2_0x379, 156, 4, _rearRibbonEffectSelect);
                    // 尾灯
                    FormatData(ref _viu_ctrl2_0x379, 171, 7, _leftRearPosBritSet);
                    FormatData(ref _viu_ctrl2_0x379, 180, 7, _rightRearPosBritSet);
                    FormatData(ref _viu_ctrl2_0x379, 186, 3, _leftRearPosFlowCmd);
                    FormatData(ref _viu_ctrl2_0x379, 199, 3, _rightRearPosFlowCmd);
                    FormatData(ref _viu_ctrl2_0x379, 166, 2, _leftRearPosCmd);
                    FormatData(ref _viu_ctrl2_0x379, 164, 2, _rightRearPosCmd);
                    FormatData(ref _viu_ctrl2_0x379, 196, 3, _middleLeftRearPosFlowCmd);
                    FormatData(ref _viu_ctrl2_0x379, 201, 3, _mdiileRightRearPosFlowCmd);
                    FormatData(ref _viu_ctrl2_0x379, 189, 7, _middleLeftRearPosBritSet);
                    FormatData(ref _viu_ctrl2_0x379, 208, 7, _middleRightRearPosBritSet);
                    FormatData(ref _viu_ctrl2_0x379, 162, 2, _middleLeftRearPosCmd);
                    FormatData(ref _viu_ctrl2_0x379, 152, 2, _middleRightRearPosCmd);
                    // LOGO
                    FormatData(ref _viu_ctrl2_0x379, 24, 2, _logoCmd);

                    _viu_ctrl2_0x379[2] = (byte)_viu_ctrl2_0x379_RollingCounter;

                    {
                        var toCheckCrc = new List<byte>();
                        for (var i = 2; i < _viu_ctrl2_0x379.Length; i++)
                            toCheckCrc.Add(_viu_ctrl2_0x379[i]);
                        toCheckCrc.Add(0x79);
                        toCheckCrc.Add(0x43);
                        var crcBytes = BitConverter.GetBytes(CALC_CRC(_crcInfo, toCheckCrc));
                        _viu_ctrl2_0x379[0] = crcBytes[1];
                        _viu_ctrl2_0x379[1] = crcBytes[0];
                    }

                    _viu_ctrl2_0x379_RollingCounter++;
                    if (_viu_ctrl2_0x379_RollingCounter > byte.MaxValue)
                        _viu_ctrl2_0x379_RollingCounter = 0;

                    return new CanBus.CanDataPackage[] { new CanBus.CanDataPackage(0x379, CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, _viu_ctrl2_0x379) };
                }),
                Interval = 100
            });
        }

        private void VIU_Info_0x1D2()
        {
            // E2E Data ID: 0x41D2
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = SendNetWorkCanFD(() =>
                {
                    FormatData(ref _viu_info_0x1D2, 28, 4, _viu_UsageMode);
                    _viu_info_0x1D2[2] = (byte)_viu_info_Counter;

                    {
                        var toCheckCrc = new List<byte>();
                        for (var i = 2; i < _viu_info_0x1D2.Length; i++)
                            toCheckCrc.Add(_viu_info_0x1D2[i]);
                        toCheckCrc.Add(0xD2);
                        toCheckCrc.Add(0x41);
                        var crcBytes = BitConverter.GetBytes(CALC_CRC(_crcInfo, toCheckCrc));
                        _viu_info_0x1D2[0] = crcBytes[1];
                        _viu_info_0x1D2[1] = crcBytes[0];
                    }

                    _viu_info_Counter++;
                    if (_viu_info_Counter > byte.MaxValue)
                        _viu_info_Counter = 0;

                    return new CanBus.CanDataPackage[] { new CanBus.CanDataPackage(0x1D2, CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, _viu_info_0x1D2) };
                }),
                Interval = 20
            });
        }

        private void VIU_LDM_Ctrl_0x15C()
        {
            // E2E Data ID:0x415C
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = SendNetWorkCanFD(() =>
                {
                    // TI
                    FormatData(ref _viu_ldm_ctrl_0x15C, 80, 2, _leftTICmd);
                    FormatData(ref _viu_ldm_ctrl_0x15C, 103, 2, _rightTICmd);
                    FormatData(ref _viu_ldm_ctrl_0x15C, 82, 3, _leftTIFlowCmd);
                    FormatData(ref _viu_ldm_ctrl_0x15C, 85, 3, _rightTIFlowCmd);
                    FormatData(ref _viu_ldm_ctrl_0x15C, 89, 7, _leftTIBritSet);
                    FormatData(ref _viu_ldm_ctrl_0x15C, 96, 7, _rightTIBritSet);

                    _viu_ldm_ctrl_0x15C[2] = (byte)_viu_ldm_ctrl_RollingCounter;

                    {
                        var toCheckCrc = new List<byte>();
                        for (var i = 2; i < _viu_ldm_ctrl_0x15C.Length; i++)
                            toCheckCrc.Add(_viu_ldm_ctrl_0x15C[i]);
                        toCheckCrc.Add(0x5C);
                        toCheckCrc.Add(0x41);
                        var crcBytes = BitConverter.GetBytes(CALC_CRC(_crcInfo, toCheckCrc));
                        _viu_ldm_ctrl_0x15C[0] = crcBytes[1];
                        _viu_ldm_ctrl_0x15C[1] = crcBytes[0];
                    }

                    _viu_ldm_ctrl_RollingCounter++;
                    if (_viu_ldm_ctrl_RollingCounter > byte.MaxValue)
                        _viu_ldm_ctrl_RollingCounter = 0;

                    return new CanBus.CanDataPackage[] { new CanBus.CanDataPackage(0x15C, CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, _viu_ldm_ctrl_0x15C) };
                }),
                Interval = 100
            });
        }

        private void VIU_Route_LCU_0x3BA()
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = SendNetWorkCanFD(() =>
                {
                    // ADS_L3
                    FormatData(ref _viu_route_lcu_0x3BA, 0, 2, _ads_l3);
                    return new CanBus.CanDataPackage[] { new CanBus.CanDataPackage(0x3BA, CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, _viu_route_lcu_0x3BA) };
                }),
                Interval = 100
            });
        }

        private void VIU1_0x3AE()
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = SendNetWorkCanFD(() =>
                {
                    // 尾门
                    FormatData(ref _viu1_0X3AE, 48, 4, _tailgate);
                    return new CanBus.CanDataPackage[] { new CanBus.CanDataPackage(0x3AE, CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, _viu1_0X3AE) };
                }),
                Interval = 100
            });
        }

        private void NM_Autosar_VIU2_LCU_0x454()
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = SendNetWorkCanFD(() =>
                {
                    FormatData(ref _nm_autosar_viu2_lcu_0x454, 12, 1, 1);
                    return new CanBus.CanDataPackage[] { new CanBus.CanDataPackage(0x454, CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, _nm_autosar_viu2_lcu_0x454) };
                }),
                Interval = 200
            });
        }

        private Action SendNetWorkCanFD(Func<CanBus.CanDataPackage[]> data)
        {
            return () =>
            {
                if (CanFD is null)
                    return;

                if (data is null)
                    return;

                if (!_isCANStart)
                {
                    _viu_ctrl2_0x379_RollingCounter = 0;
                    _viu_info_Counter = 0;
                    _viu_ldm_ctrl_RollingCounter = 0;
                    return;
                }

                lock (_lockSend)
                    CanFD.SendCanDatas(data.Invoke());
            };
        }

        private void FormatData(ref byte[] srcData, int startBit, int len, int value)
        {
            var motorala = new CanCommunicationMatrix.MotorolaMatrix(0xFF, srcData.Length);
            Array.Copy(srcData, 0, motorala.MatrixData, 0, srcData.Length);
            motorala.UpdateData(new MatrixValDefinition(startBit, len, (byte)value));
            Array.Copy(motorala.MatrixData, 0, srcData, 0, srcData.Length);
        }

        private int _leftTICmd;
        private int _rightTICmd;
        private int _leftTIFlowCmd;
        private int _rightTIFlowCmd;
        private int _leftTIBritSet;
        private int _rightTIBritSet;

        private int _leftStopCmd;
        private int _rightStopCmd;
        private int _middleLeftStopCmd;
        private int _middleRightStopCmd;
        private int _middleLeftBulCmd;
        private int _middleRightBulCmd;

        private int _rearADSCmd;
        private int _rearADSBritSet;
        private int _rearADSEffectCtrlCmd;
        private int _rearADSEffectSelect;
        private int _rearADSEffectModeSet;
        private int _rearRibbonEffectModeSet;
        private int _rearRibbonEffectCtrlCmd;
        private int _rearRibbonEffectSelect;

        private int _leftRearPosBritSet;
        private int _rightRearPosBritSet;
        private int _leftRearPosFlowCmd;
        private int _rightRearPosFlowCmd;
        private int _leftRearPosCmd;
        private int _rightRearPosCmd;
        private int _middleLeftRearPosFlowCmd;
        private int _mdiileRightRearPosFlowCmd;
        private int _middleLeftRearPosBritSet;
        private int _middleRightRearPosBritSet;
        private int _middleLeftRearPosCmd;
        private int _middleRightRearPosCmd;

        private int _tailgate;
        private int _ads_l3;
        private int _logoCmd;

        private int _viu_ctrl2_0x379_RollingCounter;
        private int _viu_info_Counter;
        private int _viu_ldm_ctrl_RollingCounter;
        private int _viu_UsageMode;

        [Description("左后转向灯点亮关闭命令")]
        public void LeftTiOnOff(int value) => _leftTICmd = value >= 0 && value <= 3 ? value : _leftTICmd;

        [Description("右后转向灯点亮关闭命令")]
        public void RightTiOnOff(int value) => _rightTICmd = value >= 0 && value <= 3 ? value : _rightTICmd;

        [Description("左后转向灯流水使能")]
        public void LeftTiFlow(int value) => _leftTIFlowCmd = value >= 0 && value <= 7 ? value : _leftTIFlowCmd;

        [Description("右后转向灯流水使能")]
        public void RightTiFlow(int value) => _rightTIFlowCmd = value >= 0 && value <= 7 ? value : _rightTIFlowCmd;

        [Description("左后转向灯亮度设置")]
        public void LeftTiBrightSet(int value) => _leftTIBritSet = value >= 0 && value <= 127 ? value : _leftTIBritSet;

        [Description("右后转向灯亮度设置")]
        public void RightTiBrightSet(int value) => _rightTIBritSet = value >= 0 && value <= 127 ? value : _rightTIBritSet;

        //[Description("转向灯400ms亮灭使能ON")]
        //public void Ti400msOnOffEnable(int value) { }

        //[Description("转向灯400ms亮灭使能OFF")]
        //public void Ti400msOnOffDisable(int value) { }

        [Description("左制动灯点亮关闭命令")]
        public void LeftStopOnOff(int value) => _leftStopCmd = value >= 0 && value <= 3 ? value : _leftStopCmd;

        [Description("右制动灯点亮关闭命令")]
        public void RightStopOnOff(int value) => _rightStopCmd = value >= 0 && value <= 3 ? value : _rightStopCmd;

        [Description("中左制动灯点亮关闭命令")]
        public void MiddleLeftStopOnOff(int value) => _middleLeftStopCmd = value >= 0 && value <= 3 ? value : _middleLeftStopCmd;

        [Description("中右制动灯点亮关闭命令")]
        public void MiddleRightStopOnOff(int value) => _middleRightStopCmd = value >= 0 && value <= 3 ? value : _middleRightStopCmd;

        [Description("中左倒车灯点亮关闭命令")]
        public void MiddleLeftBulOnOff(int value) => _middleLeftBulCmd = value >= 0 && value <= 3 ? value : _middleLeftBulCmd;

        [Description("中右倒车灯点亮关闭命令")]
        public void MiddleRightBulOnOff(int value) => _middleRightBulCmd = value >= 0 && value <= 3 ? value : _middleRightBulCmd;

        [Description("后ADS灯点亮关闭命令")]
        public void RearADSOnOff(int value) => _rearADSCmd = value >= 0 && value <= 3 ? value : _rearADSCmd;

        [Description("后ADS灯亮度设置")]
        public void RearADSBritSet(int value) => _rearADSBritSet = value >= 0 && value <= 127 ? value : _rearADSBritSet;

        [Description("后ADS灯灯语控制命令")]
        public void RearADSEffectCtrl(int value) => _rearADSEffectCtrlCmd = value >= 0 && value <= 15 ? value : _rearADSEffectCtrlCmd;

        [Description("后ADS灯灯语效果选择")]
        public void RearADSEffectSelect(int value) => _rearADSEffectSelect = value >= 0 && value <= 15 ? value : _rearADSEffectSelect;

        [Description("后ADS灯灯语效果模式设置")]
        public void RearADSEffectModeSet(int value) => _rearADSEffectModeSet = value >= 0 && value <= 3 ? value : _rearADSEffectModeSet;

        [Description("后贯穿灯灯语效果模式设置")]
        public void RearRibbonEffectModeSet(int value) => _rearRibbonEffectModeSet = value >= 0 && value <= 3 ? value : _rearRibbonEffectModeSet;

        [Description("后贯穿灯灯语效果控制命令")]
        public void RearRibbonEffectCtrl(int value) => _rearRibbonEffectCtrlCmd = value >= 0 && value <= 15 ? value : _rearRibbonEffectCtrlCmd;

        [Description("后贯穿灯灯语效果选择")]
        public void RearRibbonEffectSelect(int value) => _rearRibbonEffectSelect = value >= 0 && value <= 15 ? value : _rearRibbonEffectSelect;

        [Description("左后灯亮度设置")]
        public void LeftRearPosBritSet(int value) => _leftRearPosBritSet = value >= 0 && value <= 127 ? value : _leftRearPosBritSet;

        [Description("右后灯亮度设置")]
        public void RightRearPosBritSet(int value) => _rightRearPosBritSet = value >= 0 && value <= 127 ? value : _rightRearPosBritSet;

        [Description("左后灯流水使能")]
        public void LeftRearPosFlow(int value) => _leftRearPosFlowCmd = value >= 0 && value <= 7 ? value : _leftRearPosFlowCmd;

        [Description("右后灯流水使能")]
        public void RightRearPosFlow(int value) => _rightRearPosFlowCmd = value >= 0 && value <= 7 ? value : _rightRearPosFlowCmd;

        [Description("左后灯点亮关闭命令")]
        public void LeftRearPosCtrl(int value) => _leftRearPosCmd = value >= 0 && value <= 3 ? value : _leftRearPosCmd;

        [Description("右后灯点亮关闭命令")]
        public void RightRearPosCtrl(int value) => _rightRearPosCmd = value >= 0 && value <= 3 ? value : _rightRearPosCmd;

        [Description("中左后灯流水使能")]
        public void MidLeftRearPosFlow(int value) => _middleLeftRearPosFlowCmd = value >= 0 && value <= 7 ? value : _middleLeftRearPosFlowCmd;

        [Description("中右后灯流水使能")]
        public void MidRightRearPosFlow(int value) => _mdiileRightRearPosFlowCmd = value >= 0 && value <= 7 ? value : _mdiileRightRearPosFlowCmd;

        [Description("中左后灯亮度设置")]
        public void MidLeftRearPosBritSet(int value) => _middleLeftRearPosBritSet = value >= 0 && value <= 127 ? value : _middleLeftRearPosBritSet;

        [Description("中右后灯亮度设置")]
        public void MidRightRearPosBritSet(int value) => _middleRightRearPosBritSet = value >= 0 && value <= 127 ? value : _middleRightRearPosBritSet;

        [Description("中左后灯点亮关闭命令")]
        public void MidLeftRearPosCtrl(int value) => _middleLeftRearPosCmd = value >= 0 && value <= 3 ? value : _middleLeftRearPosCmd;

        [Description("中右后灯点亮关闭命令")]
        public void MidRightRearPosCtrl(int value) => _middleRightRearPosCmd = value >= 0 && value <= 3 ? value : _middleRightRearPosCmd;

        [Description("尾门信号")]
        public void TailGateCtrl(int value) => _tailgate = value >= 0 && value <= 15 ? value : _tailgate;

        [Description("ADS_L3_使能")]
        public void ADS_L3(int value) => _ads_l3 = value >= 0 && value <= 3 ? value : _ads_l3;

        [Description("LOGO使能")]
        public void RearLogoCtrl(int value) => _logoCmd = value >= 0 && value <= 3 ? value : _logoCmd;

        [Description("VIU_UsageMode")]
        public void VIU_UsageMode(int value) => _viu_UsageMode = value >= 0 && value <= 3 ? value : _viu_UsageMode;

        [Description("测试动画begin")]
        public void DebugAnimationBegin(int value)
        {
            if (value == 0x01 || value == 0x08 || value == 0x07 || value == 0x14)
            {
                VIU_UsageMode(1); // ID0X1D2 的整车用户模式修改为0X1或者0x2
                RearRibbonEffectCtrl(3); // 贯穿灯语控制为On
                RearRibbonEffectModeSet(2); // 贯穿灯语模式Hold
                RearRibbonEffectSelect(value); // 贯穿灯语效果0x1或者0x7或者0x8或者0x14
            }
        }

        [Description("测试动画end")]
        public void DebugAnimationEnd()
        {
            VIU_UsageMode(0);
            RearRibbonEffectCtrl(1); // 贯穿灯语控制为On
            RearRibbonEffectModeSet(0); // 贯穿灯语模式Hold
            RearRibbonEffectSelect(0); // 贯穿灯语效果0x1或者0x7或者0x8或者0x14
        }

        #endregion

        #region 诊断

        [Description("进入拓展模式")]
        public void EnterExtendMode()
        {
            if (CanFD is null)
                return;

            lock (_lockSend)
                CanFD.CanBusWithUds.TryEnterExtendedSession(DiagnosticReqCanId, DiagnosticRecvCanId, CanBus.CanType.Standard, CanBus.CanProtocol.CanFd, pendingByte: 0x00);
        }

        [Description("进入正常模式")]
        public void EnterNormalMode()
        {
            if (CanFD is null)
                return;

            lock (_lockSend)
                CanFD.CanBusWithUds.TryEnterDefaultSession(DiagnosticReqCanId, DiagnosticRecvCanId, CanBus.CanType.Standard, CanBus.CanProtocol.CanFd, pendingByte: 0x00);
        }

        [Description("R,安全访问解锁0102结果")]
        public string SecurityAccess0102Result = string.Empty;
        [Description("安全访问解锁0102")]
        public void SecurityAccess0102() => SecurityAccess0102Result = SecurityAccess(0x01, 0x02);

        [Description("R,ECU名称读取[F197]")]
        public string ECUName = string.Empty;
        [Description("R,信耀定义ECU硬件版本号[F193]")]
        public string SeeYaoECUHwVer = string.Empty;
        [Description("R,信耀定义ECU软件版本号[F195]")]
        public string SeeYaoECUSwVer = string.Empty;
        [Description("R,BootLoader软件版本号[F180]")]
        public string BootLoaderVer = string.Empty;
        [Description("R,整车厂ECU软件版本号[F189]")]
        public string FactoryECUSwVer = string.Empty;
        [Description("R,备份软件版本号[F090]")]
        public string BackupSwVer = string.Empty;
        [Description("R,整车厂ECU硬件版本号[F089]")]
        public string FactoryECUHwVer = string.Empty;
        [Description("R,整车厂定义零件号[F187]")]
        public string FactoryPartNo = string.Empty;
        [Description("R,供应商代码[F18A]")]
        public string SupplierCode = string.Empty;
        [Description("R,读取指纹信息[F184]")]
        public string Thumbprint = string.Empty;
        [Description("R,当前运行分区信息[F0F0]")]
        public string RunningAreaInfo = string.Empty;
        [Description("R,刷写尝试计数器[F0F1]")]
        public string UpdateTriedCounter = string.Empty;
        [Description("R,编程依赖检查成功计数器[F0F3]")]
        public string DependecyCheckSuccessCounter = string.Empty;

        [Description("R,写精确追溯码结果")]
        public string TraceCodeRes = string.Empty;
        [Description("R,整车厂定义软件零件号[F013]")]
        public string FactorySwPartNo = string.Empty;
        [Description("R,写ECU生产日期结果")]
        public string ECUProduceDataRes = string.Empty;

        [Description("ECU名称读取[F197]")]
        public void ReadEcuName()
        {
            ECUName = string.Empty;
            ECUName = Encoding.ASCII.GetString(ReadDid(0xF1, 0x97));
        }

        [Description("信耀定义ECU硬件版本号[F193]")]
        public void ReadSeeYaoECUHwVer()
        {
            SeeYaoECUHwVer = string.Empty;
            SeeYaoECUHwVer = Encoding.ASCII.GetString(ReadDid(0xF1, 0x93));
        }

        [Description("信耀定义ECU软件版本号[F195]")]
        public void ReadSeeYaoECUSwVer()
        {
            SeeYaoECUSwVer = string.Empty;
            SeeYaoECUSwVer = Encoding.ASCII.GetString(ReadDid(0xF1, 0x95));
        }

        [Description("BootLoader软件版本号[F180]")]
        public void ReadBootLoaderVer()
        {
            BootLoaderVer = string.Empty;
            BootLoaderVer = Encoding.ASCII.GetString(ReadDid(0xF1, 0x80));
        }

        [Description("整车厂ECU软件版本号[F189]")]
        public void ReadFactoryECUSwVer()
        {
            FactoryECUSwVer = string.Empty;
            FactoryECUSwVer = Encoding.ASCII.GetString(ReadDid(0xF1, 0x89));
        }

        [Description("整车厂定义软件零件号[F013]]")]
        public void ReadFactorySwPartNo()
        {
            FactorySwPartNo = string.Empty;
            FactorySwPartNo = Encoding.ASCII.GetString(ReadDid(0xF0, 0x13));
        }

        [Description("备份软件版本号[F090]")]
        public void ReadBackupSwVer()
        {
            BackupSwVer = string.Empty;
            BackupSwVer = Encoding.ASCII.GetString(ReadDid(0xF0, 0x90));
        }

        [Description("整车厂ECU硬件版本号[F089]")]
        public void ReadFactoryECUHwVer()
        {
            FactoryECUHwVer = string.Empty;
            FactoryECUHwVer = Encoding.ASCII.GetString(ReadDid(0xF0, 0x89));
        }

        [Description("整车厂定义零件号[F187]")]
        public void ReadFactoryPartNo()
        {
            FactoryPartNo = string.Empty;
            FactoryPartNo = Encoding.ASCII.GetString(ReadDid(0xF1, 0x87));
        }

        [Description("供应商代码[F18A]")]
        public void ReadSupplierCode()
        {
            SupplierCode = string.Empty;
            SupplierCode = Encoding.ASCII.GetString(ReadDid(0xF1, 0x8A));//ValueHelper.GetHextStr(ReadDid(0xF1, 0x8A)).Replace(" ", "");
        }

        [Description("读取指纹信息[F184]")]
        public void ReadThumbprint()
        {
            Thumbprint = string.Empty;
            Thumbprint = ValueHelper.GetHextStr(ReadDid(0xF1, 0x84)).Replace(" ", "");
        }

        [Description("当前运行分区信息[F0F0]")]
        public void ReadRunningAreaInfo()
        {
            RunningAreaInfo = string.Empty;
            RunningAreaInfo = ValueHelper.GetHextStrWithOx(ReadDid(0xF0, 0xF0));
        }

        [Description("刷写尝试计数器[F0F1]")]
        public void ReadUpdateTriedCounter()
        {
            UpdateTriedCounter = string.Empty;
            UpdateTriedCounter = ValueHelper.GetHextStr(ReadDid(0xF0, 0xF1)).Replace(" ", "");
        }

        [Description("刷写尝试计数器[F0F3]")]
        public void ReadDependecyCheckSuccessCounter()
        {
            DependecyCheckSuccessCounter = string.Empty;
            DependecyCheckSuccessCounter = ValueHelper.GetHextStr(ReadDid(0xF0, 0xF3)).Replace(" ", "");
        }

        [Description("写追溯信息")]
        public void WriteCode(string prNo, string partNo, string supplierCode, int lineNo, string factorySwPn)
        {
            if (factorySwPn.Length != 10)
            {
                TraceCodeRes = "NG 待写入整车厂定义软件零件号[F013]长度异常";
                ECUProduceDataRes = "NG 待写入整车厂定义软件零件号[F013]长度异常";
                return;
            }

            if (!SyProductionSaveCheckData.TryGetSySequenceGlobalData(prNo, out string outDate, out int outSerialNo))
            {
                TraceCodeRes = "NG 获取服务器数据失败";
                ECUProduceDataRes = "NG 获取服务器数据失败";
                return;
            }

            var str = string.Format("{0}/{1}/{2}", outDate.Substring(0, 4), outDate.Substring(4, 2), outDate.Substring(6, 2)) + " 00:00:00";
            var date = DateTime.Parse(str);
            var year = FormatYearAscii(date.Year);
            var month = FormatMonthAscii(date.Month);
            if (string.IsNullOrEmpty(year) || string.IsNullOrEmpty(month))
            {
                TraceCodeRes = "NG 获取服务器数据格式异常";
                ECUProduceDataRes = "NG 获取服务器数据格式异常";
                return;
            }
            var day = date.Day.ToString().PadLeft(2, '0');
            var writeTraceCodeAscii = "+" + partNo + supplierCode + lineNo.ToString() + year + month + day + _10To34_(outSerialNo) + Encoding.ASCII.GetString(new byte[] { 0x30, 0x30 });
            var writeDateHex = string.Format("{0}{1}{2}", date.Year, date.Month.ToString().PadLeft(2, '0'), date.Day.ToString().PadLeft(2, '0'));
            if (writeTraceCodeAscii.Length != 21)
            {
                TraceCodeRes = "NG 写入精确追溯码长度异常";
                ECUProduceDataRes = "NG 写入精确追溯码长度异常";
                return;
            }

            var writeTraceCodeBytes = Encoding.ASCII.GetBytes(writeTraceCodeAscii);
            var writeDateBytes = new List<byte>();
            for (int i = 0; i < writeDateHex.Length; i += 2)
                writeDateBytes.Add(Convert.ToByte(writeDateHex.Substring(i, 2), 16));

            EnterExtendMode();
            Thread.Sleep(100);
            SecurityAccess0102();
            if (SecurityAccess0102Result != "OK")
            {
                TraceCodeRes = "NG 安全访问0102失败";
                ECUProduceDataRes = "NG 安全访问0102失败";
                return;
            }

            Thread.Sleep(100);
            WriteDid(0xF1, 0x8C, writeTraceCodeBytes);
            WriteDid(0xF1, 0x8B, writeDateBytes.ToArray());
            WriteDid(0xF0, 0x13, Encoding.ASCII.GetBytes(factorySwPn));
            EnterNormalMode();

            var resF18C = Encoding.ASCII.GetString(ReadDid(0xF1, 0x8C));
            var resF18B = ValueHelper.GetHextStr(ReadDid(0xF1, 0x8B)).Replace(" ", "");

            if (resF18C == writeTraceCodeAscii)
                TraceCodeRes = "OK " + resF18C;
            else
                TraceCodeRes = string.Format("NG 待写入:{0}, 实际读取:{1}", writeTraceCodeAscii, resF18C);

            if (resF18B == writeDateHex)
                ECUProduceDataRes = "OK " + resF18B;
            else
                ECUProduceDataRes = string.Format("NG 待写入:{0}, 实际读取:{1}", writeDateHex, resF18B);
        }

        public string ToWriteBarcode = string.Empty;

        [Description("通过二维码写追溯信息")]
        public void WriteCodeByBarcodeCode(string partNo, string supplyerCode, int codeLen, string gse, string factorySwPn, string simplePartNo)
        {
            var tpBarcode = ToWriteBarcode;
            ToWriteBarcode = string.Empty;

            if (!tpBarcode.StartsWith("+"))
            {
                TraceCodeRes = "NG 待写入二维码开头不是+";
                ECUProduceDataRes = "NG 待写入二维码开头不是+";
                return;
            }

            if (tpBarcode.Length != codeLen)
            {
                TraceCodeRes = "NG 待写入二维码长度与判定标准不一致";
                ECUProduceDataRes = "NG 待写入二维码长度与判定标准不一致";
                return;
            }

            if (string.IsNullOrEmpty(simplePartNo) || simplePartNo.Length != 4)
            {
                TraceCodeRes = "NG 待写入零件简号长度不为4";
                ECUProduceDataRes = "NG 待写入零件简号长度不为4";
                return;
            }

            var toWritePartNoStr = string.Empty;
            var toWriteSupplyerCode = string.Empty;
            var outDate = string.Empty;
            var lineNo = string.Empty;
            var outSerialNo = string.Empty;

            try
            {
                toWritePartNoStr = tpBarcode.Substring(1, 14);
                toWriteSupplyerCode = tpBarcode.Substring(15, 5);
                lineNo = int.Parse(tpBarcode.Substring(20, 1)).ToString();
                outDate = "20" + tpBarcode.Substring(21, 6);
                outSerialNo = _34To10_(tpBarcode.Substring(27, 4)).ToString();

                if (toWritePartNoStr != partNo)
                {
                    TraceCodeRes = "NG 待写入二维码中客户零件图号与判定标准不一致";
                    ECUProduceDataRes = "NG 待写入二维码中客户零件图号与判定标准不一致";
                    return;
                }

                if (toWriteSupplyerCode != supplyerCode)
                {
                    TraceCodeRes = "NG 待写入二维码中供应商代码与判定标准不一致";
                    ECUProduceDataRes = "NG 待写入二维码中供应商代码与判定标准不一致";
                    return;
                }

                if (!tpBarcode.EndsWith(gse))
                {
                    TraceCodeRes = "NG 待写入二维码中GSE零件名称+GSE零件号与判定标准不一致";
                    ECUProduceDataRes = "NG 待写入二维码中GSE零件名称+GSE零件号与判定标准不一致";
                    return;
                }

            }
            catch (Exception ex)
            {
                TraceCodeRes = "NG 待写入二维码解析失败：" + ex.Message;
                ECUProduceDataRes = "NG 待写入二维码解析失败：" + ex.Message;
                return;
            }

            var str = string.Format("{0}/{1}/{2}", outDate.Substring(0, 4), outDate.Substring(4, 2), outDate.Substring(6, 2)) + " 00:00:00";
            var date = DateTime.Parse(str);
            var year = FormatYearAscii(date.Year);
            var month = FormatMonthAscii(date.Month);
            if (string.IsNullOrEmpty(year) || string.IsNullOrEmpty(month))
            {
                TraceCodeRes = "NG 二维码解析日期异常";
                ECUProduceDataRes = "NG 二维码解析日期异常";
                return;
            }
            var day = date.Day.ToString().PadLeft(2, '0');
            var writeTraceCodeAscii = "+" + simplePartNo + toWriteSupplyerCode + lineNo.ToString() + year + month + day + _10To34_(int.Parse(outSerialNo)) + Encoding.ASCII.GetString(new byte[] { 0x30, 0x30 });
            var writeDateHex = string.Format("{0}{1}{2}", date.Year, date.Month.ToString().PadLeft(2, '0'), date.Day.ToString().PadLeft(2, '0'));
            if (writeTraceCodeAscii.Length != 21)
            {
                TraceCodeRes = "NG 写入精确追溯码长度异常";
                ECUProduceDataRes = "NG 写入精确追溯码长度异常";
                return;
            }

            var writeTraceCodeBytes = Encoding.ASCII.GetBytes(writeTraceCodeAscii);
            var writeDateBytes = new List<byte>();
            for (int i = 0; i < writeDateHex.Length; i += 2)
                writeDateBytes.Add(Convert.ToByte(writeDateHex.Substring(i, 2), 16));

            EnterExtendMode();
            Thread.Sleep(100);
            SecurityAccess0102();
            if (SecurityAccess0102Result != "OK")
            {
                TraceCodeRes = "NG 安全访问0102失败";
                ECUProduceDataRes = "NG 安全访问0102失败";
                return;
            }

            Thread.Sleep(100);
            WriteDid(0xF1, 0x8C, writeTraceCodeBytes);
            WriteDid(0xF1, 0x8B, writeDateBytes.ToArray());
            WriteDid(0xF0, 0x13, Encoding.ASCII.GetBytes(factorySwPn));
            EnterNormalMode();

            var resF18C = Encoding.ASCII.GetString(ReadDid(0xF1, 0x8C));
            var resF18B = ValueHelper.GetHextStr(ReadDid(0xF1, 0x8B)).Replace(" ", "");

            if (resF18C == writeTraceCodeAscii)
                TraceCodeRes = "OK " + resF18C;
            else
                TraceCodeRes = string.Format("NG 待写入:{0}, 实际读取:{1}", writeTraceCodeAscii, resF18C);

            if (resF18B == writeDateHex)
                ECUProduceDataRes = "OK " + resF18B;
            else
                ECUProduceDataRes = string.Format("NG 待写入:{0}, 实际读取:{1}", writeDateHex, resF18B);
        }

        [Description("信息验证")]
        public void VersionCheck(string prNo, string partNo, string supplierCode, int lineNo, string factorySwPn)
        {
            ReadEcuName();
            ReadSeeYaoECUHwVer();
            ReadSeeYaoECUSwVer();
            ReadBootLoaderVer();
            ReadFactoryECUSwVer();
            ReadBackupSwVer();
            ReadFactoryECUHwVer();
            ReadFactoryPartNo();
            ReadSupplierCode();
            ReadThumbprint();
            ReadRunningAreaInfo();
            ReadUpdateTriedCounter();
            ReadDependecyCheckSuccessCounter();

            // "PR4101001062", "0000", "VG350", 1, "8744AFK010"
            WriteCode(prNo, partNo, supplierCode, lineNo, factorySwPn);

            ReadFactorySwPartNo();
        }

        [Description("R,清除DTC结果")]
        public string ClearDtcResult = string.Empty;
        [Description("R,读取DTC结果")]
        public string ReadDtcResult = string.Empty;

        [Description("清除DTC")]
        public void ClearDtc()
        {
            ClearDtcResult = string.Empty;

            if (CanFD is null)
                return;

            lock (_lockSend)
                ClearDtcResult = CanFD.CanBusWithUds.TryClearAllGroupsDiagnosticInformation(DiagnosticReqCanId, DiagnosticRecvCanId, CanBus.CanType.Standard, pendingByte: 0x00, canProtocol: CanBus.CanProtocol.CanFd) ? "OK" : "NG";
        }

        [Description("读取DTC")]
        public void ReadDtc()
        {
            ReadDtcResult = string.Empty;

            if (CanFD is null)
                return;

            lock (_lockSend)
            {
                byte[] echo;
                if (CanFD.CanBusWithUds.TryReadDtcInfomation(DiagnosticReqCanId, DiagnosticRecvCanId, CanBus.CanType.Standard, 0x02, 0x2F, out echo, canProtocol: CanBus.CanProtocol.CanFd, pendingByte: 0x00))
                {
                    if (echo != null)
                    {
                        if (echo.Length == 0)
                        {
                            ReadDtcResult = "NoError";
                        }
                        else
                        {
                            if (echo.Length % 4 == 0)
                            {
                                var readCodes = new List<Uds14229Helper.DtcData>();

                                for (var i = 0; i < echo.Length; i = i + 4)
                                {
                                    var dtcData = new Uds14229Helper.DtcData(echo[i], echo[i + 1], echo[i + 2], echo[i + 3]);
                                    readCodes.Add(dtcData);
                                }

                                foreach (var t in readCodes)
                                    ReadDtcResult += string.Format("[{0}];", t.Remark);
                            }
                            else
                            {
                                ReadDtcResult = "ReadDtcResLenError";
                            }
                        }
                    }
                }
                else
                {
                    ReadDtcResult = "ReadFailed";
                }
            }
        }

        private List<byte> _uds22Buffer = new List<byte>();
        private bool _bReadDid;
        private bool _bReadSecodnFrame;
        private ManualResetEventSlim _bReadDidWaitHandle = new ManualResetEventSlim(false);
        private uint DiagnosticReqCanId = 0x75C;
        private uint DiagnosticRecvCanId = 0x7DC;

        private List<byte> _uds27Buffer = new List<byte>();
        private bool _bSeedKeySubFunc;
        private ManualResetEventSlim _bseedKeyWaitHandle = new ManualResetEventSlim(false);
        private bool _bInSeedKey;

        private static string FormatYearAscii(int year)
        {
            switch (year)
            {
                case 2025: return "S";
                case 2026: return "T";
                case 2027: return "V";
                case 2028: return "W";
                case 2029: return "X";
                case 2030: return "Y";
                case 2031: return "1";
                case 2032: return "2";
                case 2033: return "3";
                case 2034: return "4";
                case 2035: return "5";
                case 2036: return "6";
                case 2037: return "7";
                case 2038: return "8";
                case 2039: return "9";
                case 2040: return "A";
            }

            return string.Empty;
        }

        private static string FormatMonthAscii(int month)
        {
            switch (month)
            {
                case 1: return "1";
                case 2: return "2";
                case 3: return "3";
                case 4: return "4";
                case 5: return "5";
                case 6: return "6";
                case 7: return "7";
                case 8: return "8";
                case 9: return "9";
                case 10: return "A";
                case 11: return "B";
                case 12: return "C";
            }

            return string.Empty;
        }

        private const string repString = "0123456789ABCDEFGHJKLMNPQRSTUVWXYZ"; // 权展开求和：将34进制数从右至左的每一位分别乘以34的相应次方，然后求和。例如，34进制数“1A2B”转换为10进制数的计算为： (34^3×1) + (34^2×10) + (34^1×2) + (34^0×11) = 39313。 
        public static string _10To34_(int Num) => repString.Substring(Num / (34 * 34 * 34), 1) + repString.Substring(Num / (34 * 34) % 34, 1) + repString.Substring(Num / 34 % 34, 1) + repString.Substring(Num % 34, 1);
        public static int _34To10_(string Numstring) => repString.IndexOf(Numstring.Substring(3, 1)) + 34 * (repString.IndexOf(Numstring.Substring(2, 1))) + 34 * 34 * (repString.IndexOf(Numstring.Substring(1, 1))) + 34 * 34 * 34 * (repString.IndexOf(Numstring.Substring(0, 1)));

        public static byte[] ComputeCMAC(byte[] key, byte[] data)
        {
            // Generate SubKeys
            byte[] L = AESEncrypt(key, new byte[16], new byte[16]);
            byte[] K1 = Rol(L);
            if ((L[0] & 0x80) == 0x80) K1[15] ^= 0x87;

            byte[] K2 = Rol(K1);
            if ((K1[0] & 0x80) == 0x80) K2[15] ^= 0x87;

            // Process Data
            if (data.Length % 16 == 0 && data.Length != 0)
            {
                for (int i = 0; i < K1.Length; i++)
                    data[data.Length - 16 + i] ^= K1[i];
            }
            else
            {
                byte[] padding = new byte[16 - data.Length % 16];
                padding[0] = 0x80;
                data = data.Concat(padding).ToArray();

                for (int i = 0; i < K2.Length; i++)
                    data[data.Length - 16 + i] ^= K2[i];
            }

            // Final Encryption
            return AESEncrypt(key, new byte[16], data).Take(16).ToArray();
        }

        private static byte[] AESEncrypt(byte[] key, byte[] iv, byte[] data)
        {
            using (var aes = new AesCryptoServiceProvider { Mode = CipherMode.CBC, Padding = PaddingMode.None })
            using (var ms = new MemoryStream())
            using (var cs = new CryptoStream(ms, aes.CreateEncryptor(key, iv), CryptoStreamMode.Write))
            {
                cs.Write(data, 0, data.Length);
                cs.FlushFinalBlock();
                return ms.ToArray();
            }
        }

        private static byte[] Rol(byte[] input)
        {
            byte[] result = new byte[input.Length];
            byte carry = 0;

            for (int i = input.Length - 1; i >= 0; i--)
            {
                ushort temp = (ushort)(input[i] << 1);
                result[i] = (byte)((temp & 0xFF) | carry);
                carry = (byte)((temp & 0xFF00) >> 8);
            }

            return result;
        }

        internal static class CalcKey
        {
            [DllImport(@"\DllImport\F03斯坦雷_LDMR_SA.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr GenerateKeyEx(
                byte[] iSeedArray, /* Array for the seed [in] */
                short iSeedArraySize, /* Length of the array for the seed [in] */
                int iSecurityLevel, /* Security level [in] */
                byte[] iVariant, /* Name of the active variant [in] */
                byte[] ioKeyArray, /* Array for the key [in, out] */
                int iKeyArraySize, /* Maximum length of the array for the key [in] */
                out int oSize /* Length of the key [out] */);
        }

        private byte[] ReadDid(byte didHi, byte didLo)
        {
            if (CanFD is null)
                return new byte[0];
            lock (_lockSend)
            {
                byte[] echo;
                if (CanFdUds22(didHi, didLo, out echo))
                {
                    Thread.Sleep(1);
                    return echo;
                }
            }

            Thread.Sleep(100);

            lock (_lockSend)
            {
                byte[] echo;
                if (CanFdUds22(didHi, didLo, out echo))
                {
                    Thread.Sleep(1);
                    return echo;
                }
            }


            return new byte[0];
        }

        private bool CanFdUds22(byte didHi, byte didLo, out byte[] echo)
        {
            _uds22Buffer.Clear();

            if (CanFD is null)
            {
                echo = new byte[0];
                return false;
            }

            _bReadDidWaitHandle.Reset();
            _bReadDid = true;

            var firstSend = new byte[] { 0x03, 0x22, didHi, didLo, 0x00, 0x00, 0x00, 0x00 };

            var t1 = HighPrecisionTimer.GetTimestamp();

            //使用 ThreadPool 异步发送，减少 Task 创建开销
            ThreadPool.QueueUserWorkItem(_ => CanFD.SendStandardCanFdData(DiagnosticReqCanId, firstSend));

            // 同步等待响应
            var isFirstRecvOk = _bReadDidWaitHandle.Wait(250);

            _bReadDid = false;
            _bReadDidWaitHandle.Reset();

            var t2 = HighPrecisionTimer.GetTimestamp();
            Console.WriteLine("read did cost: {0}/ms", HighPrecisionTimer.GetTimestampIntervalMs(t1, t2));

            if (isFirstRecvOk)
            {
                var bufferArray = _uds22Buffer.ToArray();
                var count = bufferArray.Length;

                if (count >= 4)
                {
                    var b1 = bufferArray[0];
                    var b2 = bufferArray[1];
                    var b3 = bufferArray[2];
                    var b4 = bufferArray[3];

                    if (count == 8 && b1.GetByteHighOrder() == 0x00 && b2 == 0x62 && b3 == didHi && b4 == didLo)
                    {
                        var datalen = b1.GetByteLowOrder();
                        if (datalen >= 3 && datalen <= 7)
                        {
                            echo = new byte[datalen - 3];
                            Array.Copy(bufferArray, 4, echo, 0, datalen - 3);
                            return true;
                        }
                    }
                    else if (count > 8 && b1.GetByteHighOrder() == 0x00)
                    {
                        var datalen = b2;
                        if (datalen >= 3 && datalen <= 62 && count >= datalen + 2)
                        {
                            echo = new byte[datalen - 3];
                            Array.Copy(bufferArray, 5, echo, 0, datalen - 3);
                            return true;
                        }
                    }
                }
            }

            echo = new byte[0];
            return false;
        }

        private bool _bInWriteDID;

        private bool WriteDid(byte didHi, byte didLo, byte[] data)
        {
            if (CanFD is null)
                return false;

            if (data is null || data.Length is 0 || data.Length >= 32)
                return false;

            //return CanFD.CanBusWithUds.TryWriteData(DiagnosticReqCanId, DiagnosticRecvCanId, CanBus.CanType.Extended, CanBus.CanProtocol.CanFd, didHi, didLo, data, pendingByte: 0x00);

            lock (_lockSend)
            {
                _bInWriteDID = true;

                if (data.Length > 0 && data.Length <= 4)
                {
                    var dLen = (byte)(1 + 2 + data.Length);
                    var sendBytes = new byte[8] { dLen, 0x2E, didHi, didLo, 0x00, 0x00, 0x00, 0x00 };
                    Array.Copy(data, 0, sendBytes, 4, data.Length);
                    CanFD.SendStandardCanFdData(DiagnosticReqCanId, sendBytes);
                }
                else
                {
                    var count = (data.Length + 2 + 1 + 2) / 8;
                    var restCount = (data.Length + 2 + 1 + 2) % 8;
                    if (restCount != 0)
                    {
                        count++;
                    }

                    var sendBytes = new byte[count * 8];

                    var dLen = (ushort)(1 + 2 + data.Length);
                    var dLenBytes = BitConverter.GetBytes(dLen).Reverse().ToArray();
                    sendBytes[0] = dLenBytes[0];
                    sendBytes[1] = dLenBytes[1];
                    sendBytes[2] = 0x2E;
                    sendBytes[3] = didHi;
                    sendBytes[4] = didLo;
                    Array.Copy(data, 0, sendBytes, 5, data.Length);
                    for (var i = 0; i < (8 - restCount); i++)
                        sendBytes[sendBytes.Length - 1 - i] = 0xCC;
                    CanFD.SendStandardCanFdData(DiagnosticReqCanId, sendBytes);
                }

                Thread.Sleep(200);
                _bInWriteDID = false;

                return true;
            }
        }

        private string SecurityAccess(byte requesetSeedSubFunc, byte sendKeySubunc)
        {
            if (CanFD is null)
                return string.Empty;

            //var keyBytes = new byte[12];
            //for (var i = 0; i < keyBytes.Length; i++)
            //    keyBytes[i] = (byte)(i + 1);

            _bseedKeyWaitHandle.Reset();
            _uds27Buffer.Clear();
            _bSeedKeySubFunc = true;
            _bInSeedKey = true;
            var firstSend = new byte[] { 0x02, 0x27, requesetSeedSubFunc, 0x00, 0x00, 0x00, 0x00, 0x00 };
            //var taskFirstWait = Task.Factory.StartNew(() =>
            //{
            //    return _bseedKeyWaitHandle.Wait(1500);
            //});
            var taskFirstWait = Task<bool>.Factory.StartNew(() => _bseedKeyWaitHandle.Wait(1500));
            var taskFirstSend = Task.Factory.StartNew(() =>
            {
                CanFD.SendStandardCanFdData(DiagnosticReqCanId, firstSend);
            });
            Task.WaitAll(taskFirstSend, taskFirstWait);
            _bSeedKeySubFunc = false;
            _bseedKeyWaitHandle.Reset();
            _bInSeedKey = false;

            var isFirstRecvOk = taskFirstWait.Result;
            if (isFirstRecvOk)
            {
                if (_uds27Buffer.Count >= 4)
                {
                    var b1 = _uds27Buffer[0];
                    var b2 = _uds27Buffer[1];
                    var b3 = _uds27Buffer[2];
                    var b4 = _uds27Buffer[3];

                    if (b1.GetByteHighOrder() == 0x00 && b3 == 0x67 && b4 == requesetSeedSubFunc)
                    {
                        var seedBytes = new byte[16];
                        Array.Copy(_uds27Buffer.ToArray(), 4, seedBytes, 0, 16);

                        var keyBytes = new byte[16];
                        int keyLen;
                        CalcKey.GenerateKeyEx(seedBytes, 16, 0x01, null, keyBytes, 16, out keyLen);

                        _bseedKeyWaitHandle.Reset();
                        _uds27Buffer.Clear();
                        _bSeedKeySubFunc = true;
                        //firstSend = new byte[16] { 0x00, 0x12, 0x27, sendKeySubunc, keyBytes[0], keyBytes[1], keyBytes[2], keyBytes[3], keyBytes[4], keyBytes[5], keyBytes[6], keyBytes[7], keyBytes[8], keyBytes[9], keyBytes[10], keyBytes[11] };
                        firstSend = new byte[20];
                        firstSend[0] = 0x00;
                        firstSend[1] = 16 + 2;
                        firstSend[2] = 0x27;
                        firstSend[3] = sendKeySubunc;
                        Array.Copy(keyBytes, 0, firstSend, 4, 16);
                        //var firstSend2 = new byte[8];
                        //firstSend2[0] = keyBytes[12];
                        //firstSend2[1] = keyBytes[13];
                        //firstSend2[2] = keyBytes[14];
                        //firstSend2[3] = keyBytes[15];
                        taskFirstWait = Task.Factory.StartNew(() =>
                        {
                            return _bseedKeyWaitHandle.Wait(1500);
                        });
                        taskFirstSend = Task.Factory.StartNew(() =>
                        {
                            CanFD.SendStandardCanFdData(DiagnosticReqCanId, firstSend);
                            //Thread.Sleep(15);
                            //Can.SendStandardCanFdData(DiagnosticReqCanId, firstSend2);
                        });
                        Task.WaitAll(taskFirstSend, taskFirstWait);
                        _bSeedKeySubFunc = false;

                        var isSecondRecvOk = taskFirstWait.Result;
                        if (_uds27Buffer.Count >= 4)
                        {
                            b1 = _uds27Buffer[0];
                            b2 = _uds27Buffer[1];
                            b3 = _uds27Buffer[2];
                            b4 = _uds27Buffer[3];

                            if (b1.GetByteHighOrder() == 0x00 && b1.GetByteLowOrder() == 0x02 && b2 == 0x67 && b3 == sendKeySubunc)
                            {
                                return "OK";
                            }
                            else
                            {
                                return "NG 解锁KEY失败";
                            }
                        }
                        else
                        {
                            return "NG 解锁KEY失败";
                        }
                    }
                    else
                    {
                        return "NG 获取SEED失败";
                    }
                }
                else
                {
                    return "NG 获取SEED失败";
                }
            }
            else
            {
                return "NG 获取SEED失败";
            }

            //return CanFD.CanBusWithUds.TryRequestSeed(DiagnosticReqCanId, DiagnosticRecvCanId, CanBus.CanType.Extended, requesetSeedSubFunc, out _, pendingByte: 0x00, canProtocol: CanProtocol.CanFd) ?
            //    (CanFD.CanBusWithUds.TrySendKey(DiagnosticReqCanId, DiagnosticRecvCanId, CanBus.CanType.Extended, sendKeySubunc, keyBytes, pendingByte: 0x00, canProtocol: CanProtocol.CanFd) ? "OK" : "NG 解锁KEY失败") :
            //    "NG 获取SEED失败";
        }

        #endregion

        #region E2E

        private uint CALC_CRC(CrcHelper.CrcInfo info, IReadOnlyList<byte> memBlock)
        {
            var memBlockLen = (uint)memBlock.Count;
            uint value;

            if (info.Refin)
            {
                value = BitReflected(info.InitReg, info.Width);
                if (info.Width > 8)
                {
                    var i = 0;
                    while (memBlockLen-- != 0)
                    {
                        value = (value >> 8) ^ _table[value & 0xff ^ memBlock[i++]];
                    }
                }
                else
                {
                    var i = 0;
                    while (memBlockLen-- != 0)
                    {
                        value = _table[value ^ memBlock[i++]];
                    }
                }
            }
            else
            {
                if (info.Width > 8)
                {
                    value = info.InitReg;
                    var i = 0;
                    while (memBlockLen-- != 0)
                    {
                        var high = (byte)(value >> (info.Width - 8));
                        value = (value << 8) ^ _table[high ^ memBlock[i++]];
                    }
                }
                else
                {
                    value = info.InitReg << (8 - info.Width);
                    var i = 0;
                    while (memBlockLen-- != 0)
                    {
                        value = _table[value ^ memBlock[i++]];
                    }
                    value >>= 8 - info.Width;
                }
            }
            if (info.Refout != info.Refin)
            {
                value = BitReflected(value, info.Width);
            }
            value ^= info.Xorout;
            return value & (((uint)2 << (info.Width - 1)) - 1);
        }

        private static uint BitReflected(uint input, byte bits)
        {
            uint res = 0;
            while (bits-- > 0)
            {
                res <<= 1;
                if ((input & 0x01) != 0)
                    res |= 1;
                input >>= 1;
            }
            return res;
        }

        private readonly CrcHelper.CrcInfo _crcInfo = new CrcHelper.CrcInfo
        {
            Width = 16,
            Poly = 0x1021,
            InitReg = 0xFFFF,
            Refin = false,
            Refout = false,
            Xorout = 0x00,
        };

        private readonly uint[] _table = new uint[256]
        {
            0x0000, 0x1021, 0x2042, 0x3063, 0x4084, 0x50A5, 0x60C6, 0x70E7, 0x8108, 0x9129, 0xA14A, 0xB16B, 0xC18C,
            0xD1AD, 0xE1CE, 0xF1EF, 0x1231, 0x0210, 0x3273, 0x2252, 0x52B5, 0x4294, 0x72F7, 0x62D6, 0x9339, 0x8318,
            0xB37B, 0xA35A, 0xD3BD, 0xC39C, 0xF3FF, 0xE3DE, 0x2462, 0x3443, 0x0420, 0x1401, 0x64E6, 0x74C7, 0x44A4,
            0x5485, 0xA56A, 0xB54B, 0x8528, 0x9509, 0xE5EE, 0xF5CF, 0xC5AC, 0xD58D, 0x3653, 0x2672, 0x1611, 0x0630,
            0x76D7, 0x66F6, 0x5695, 0x46B4, 0xB75B, 0xA77A, 0x9719, 0x8738, 0xF7DF, 0xE7FE, 0xD79D, 0xC7BC, 0x48C4,
            0x58E5, 0x6886, 0x78A7, 0x0840, 0x1861, 0x2802, 0x3823, 0xC9CC, 0xD9ED, 0xE98E, 0xF9AF, 0x8948, 0x9969,
            0xA90A, 0xB92B, 0x5AF5, 0x4AD4, 0x7AB7, 0x6A96, 0x1A71, 0x0A50, 0x3A33, 0x2A12, 0xDBFD, 0xCBDC, 0xFBBF,
            0xEB9E, 0x9B79, 0x8B58, 0xBB3B, 0xAB1A, 0x6CA6, 0x7C87, 0x4CE4, 0x5CC5, 0x2C22, 0x3C03, 0x0C60, 0x1C41,
            0xEDAE, 0xFD8F, 0xCDEC, 0xDDCD, 0xAD2A, 0xBD0B, 0x8D68, 0x9D49, 0x7E97, 0x6EB6, 0x5ED5, 0x4EF4, 0x3E13,
            0x2E32, 0x1E51, 0x0E70, 0xFF9F, 0xEFBE, 0xDFDD, 0xCFFC, 0xBF1B, 0xAF3A, 0x9F59, 0x8F78, 0x9188, 0x81A9,
            0xB1CA, 0xA1EB, 0xD10C, 0xC12D, 0xF14E, 0xE16F, 0x1080, 0x00A1, 0x30C2, 0x20E3, 0x5004, 0x4025, 0x7046,
            0x6067, 0x83B9, 0x9398, 0xA3FB, 0xB3DA, 0xC33D, 0xD31C, 0xE37F, 0xF35E, 0x02B1, 0x1290, 0x22F3, 0x32D2,
            0x4235, 0x5214, 0x6277, 0x7256, 0xB5EA, 0xA5CB, 0x95A8, 0x8589, 0xF56E, 0xE54F, 0xD52C, 0xC50D, 0x34E2,
            0x24C3, 0x14A0, 0x0481, 0x7466, 0x6447, 0x5424, 0x4405, 0xA7DB, 0xB7FA, 0x8799, 0x97B8, 0xE75F, 0xF77E,
            0xC71D, 0xD73C, 0x26D3, 0x36F2, 0x0691, 0x16B0, 0x6657, 0x7676, 0x4615, 0x5634, 0xD94C, 0xC96D, 0xF90E,
            0xE92F, 0x99C8, 0x89E9, 0xB98A, 0xA9AB, 0x5844, 0x4865, 0x7806, 0x6827, 0x18C0, 0x08E1, 0x3882, 0x28A3,
            0xCB7D, 0xDB5C, 0xEB3F, 0xFB1E, 0x8BF9, 0x9BD8, 0xABBB, 0xBB9A, 0x4A75, 0x5A54, 0x6A37, 0x7A16, 0x0AF1,
            0x1AD0, 0x2AB3, 0x3A92, 0xFD2E, 0xED0F, 0xDD6C, 0xCD4D, 0xBDAA, 0xAD8B, 0x9DE8, 0x8DC9, 0x7C26, 0x6C07,
            0x5C64, 0x4C45, 0x3CA2, 0x2C83, 0x1CE0, 0x0CC1, 0xEF1F, 0xFF3E, 0xCF5D, 0xDF7C, 0xAF9B, 0xBFBA, 0x8FD9,
            0x9FF8, 0x6E17, 0x7E36, 0x4E55, 0x5E74, 0x2E93, 0x3EB2, 0x0ED1, 0x1EF0
        };

        #endregion
    }
}
