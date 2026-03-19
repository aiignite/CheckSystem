using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Controller
{
    [Description("CAN-Product,三立LCD屏")]
    public sealed class SmartGlassDisplay : ControllerBase
    {
        public CanBus CanFd;

        public byte BCM_Crc1Val;
        public byte Lamp_ExtrnlTailLmpOnReq;
        public byte Lamp_HdLmpWlcmCmd;
        public byte Lamp_HdLmpEscortShortCmd;
        public byte Lamp_AutoLtSnsrNightSta;
        public byte CF_AVN_IFSActVehSpd_Set;
        public byte CF_AVN_DWL_SelectNvalueSet;
        public byte Warn_DrvDrSwSta;
        public byte Warn_AsstDrSwSta;
        public byte Warn_RrLftDrSwSta;
        public byte Warn_RrRtDrSwSta;
        public uint WHL_SpdFLVal;
        public uint WHL_SpdFRVal;
        public uint WHL_SpdRLVal;
        public uint WHL_SpdRRVal;

        public SmartGlassDisplay(string name) : base(name)
        {
            MainWork();
            SchedulerAsync();
        }

        ~SmartGlassDisplay() => Dispose();

        private void MainWork()
        {
            Msg4C0_10ms();
            Msg41A_200ms();
            Msg41B_200ms();
            Msg41C_200ms();
            Msg4AD_10ms();
            Msg1EB_10ms();
            Msg411_200ms();
            Msg0280A002_10ms();
        }

        private void Msg4C0_10ms()
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = () =>
                {
                    try
                    {
                        var msg4C0 = GetBytes(new byte[8], BCM_Crc1Val, 0, 8);
                        SendCanMsg(new CanBus.CanDataPackage[] { new CanBus.CanDataPackage(0x4C0, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, msg4C0) });
                    }
                    catch (Exception) { }
                },
                Interval = 10
            });
        }

        private void Msg41A_200ms()
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = () =>
                {
                    try
                    {
                        var msg41A = GetBytes(new byte[8], Lamp_ExtrnlTailLmpOnReq, 38, 2);
                        SendCanMsg(new CanBus.CanDataPackage[] { new CanBus.CanDataPackage(0x41A, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, msg41A) });
                    }
                    catch (Exception) { }
                },
                Interval = 200
            });
        }

        private void Msg41B_200ms()
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = () =>
                {
                    try
                    {
                        var msg41B = new byte[8];
                        msg41B = GetBytes(msg41B, Lamp_HdLmpWlcmCmd, 34, 2);
                        msg41B = GetBytes(msg41B, Lamp_HdLmpEscortShortCmd, 50, 2);
                        SendCanMsg(new CanBus.CanDataPackage[] { new CanBus.CanDataPackage(0x41B, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, msg41B) });
                    }
                    catch (Exception) { }
                },
                Interval = 200
            });
        }

        private void Msg41C_200ms()
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = () =>
                {
                    try
                    {
                        var msg41C = GetBytes(new byte[8], Lamp_AutoLtSnsrNightSta, 19, 2);
                        SendCanMsg(new CanBus.CanDataPackage[] { new CanBus.CanDataPackage(0x41C, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, msg41C) });
                    }
                    catch (Exception) { }
                },
                Interval = 200
            });
        }

        private void Msg4AD_10ms()
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = () =>
                {
                    try
                    {
                        var msg4AD = GetBytes(new byte[8], CF_AVN_IFSActVehSpd_Set, 42, 4);
                        SendCanMsg(new CanBus.CanDataPackage[] { new CanBus.CanDataPackage(0x4AD, CanBus.CanProtocol.SpeedCanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, msg4AD) });
                    }
                    catch (Exception) { }
                },
                Interval = 10
            });
        }

        private void Msg1EB_10ms()
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = () =>
                {
                    try
                    {
                        var msg1EB = GetBytes(new byte[8], CF_AVN_DWL_SelectNvalueSet, 30, 4);
                        SendCanMsg(new CanBus.CanDataPackage[] { new CanBus.CanDataPackage(0x1EB, CanBus.CanProtocol.SpeedCanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, msg1EB) });
                    }
                    catch (Exception) { }
                },
                Interval = 10
            });
        }

        private void Msg411_200ms()
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = () =>
                {
                    try
                    {
                        var msg411 = new byte[8];
                        msg411 = GetBytes(msg411, Warn_DrvDrSwSta, 24, 2);
                        msg411 = GetBytes(msg411, Warn_AsstDrSwSta, 34, 2);
                        msg411 = GetBytes(msg411, Warn_RrLftDrSwSta, 52, 2);
                        msg411 = GetBytes(msg411, Warn_RrRtDrSwSta, 56, 2);
                        SendCanMsg(new CanBus.CanDataPackage[] { new CanBus.CanDataPackage(0x411, CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, msg411) });
                    }
                    catch (Exception) { }
                },
                Interval = 200
            });
        }

        private void Msg0280A002_10ms()
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = () =>
                {
                    try
                    {
                        var msg280A002 = new byte[8];
                        msg280A002 = GetBytes(msg280A002, WHL_SpdFLVal, 0, 14);
                        msg280A002 = GetBytes(msg280A002, WHL_SpdFRVal, 16, 14);
                        msg280A002 = GetBytes(msg280A002, WHL_SpdRLVal, 32, 14);
                        msg280A002 = GetBytes(msg280A002, WHL_SpdRRVal, 48, 14);
                        SendCanMsg(new CanBus.CanDataPackage[] { new CanBus.CanDataPackage(0x0280A002, CanBus.CanProtocol.SpeedCanFd, CanBus.CanType.Extended, CanBus.CanFormat.Data, msg280A002) });
                    }
                    catch (Exception) { }
                },
                Interval = 10
            });
        }

        private byte[] GetBytes(byte[] originBytes, uint value, int valueStartBit, int valueLen)
        {
            var listBits = new List<string>();
            for (int i = 0; i < originBytes.Length; i++)
            {
                var str = Convert.ToString(originBytes[i], 2).PadLeft(8, '0');
                for (var j = str.Length - 1; j >= 0; j--)
                    listBits.Add(str[j].ToString());
            }

            {
                var valueStr = Convert.ToString(value, 2).PadLeft(valueLen, '0');
                var valueBits = new List<string>();
                for (var j = valueStr.Length - 1; j >= 0; j--)
                    valueBits.Add(valueStr[j].ToString());

                for (var i = valueStartBit; i < valueStartBit + valueLen; i++)
                    listBits[i] = valueBits[i - valueStartBit];
            }

            {
                var bIndex = 0;
                var bBytes = new byte[originBytes.Length];
                for (int i = 0; i < listBits.Count; i = i + 8)
                {
                    var str = string.Format("{7}{6}{5}{4}{3}{2}{1}{0}", listBits[i], listBits[i + 1], listBits[i + 2], listBits[i + 3], listBits[i + 4], listBits[i + 5], listBits[i + 6], listBits[i + 7]);
                    var b = Convert.ToByte(str, 2);
                    bBytes[bIndex] = b;
                    bIndex++;
                }
                Array.Copy(bBytes, 0, originBytes, 0, bBytes.Length);
            }

            return originBytes;
        }

        private readonly object _lockSend = new object();
        private bool _isCanStop = true;

        private void SendCanMsg(CanBus.CanDataPackage[] datas)
        {
            if (CanFd == null)
                return;

            if (_isCanStop)
                return;

            lock (_lockSend)
                CanFd.SendCanDatas(datas);
        }

        [Description("打开can")]
        public void StartCan() => _isCanStop = false;

        [Description("关闭can")]
        public void StopCan() => _isCanStop = true;

        [Description("Lamp_ExtrnlTailLmpOnReq_0x0:Off/0x1:On/0x2:NotUsed/0x3:ErrorIndicator")]
        public void Set_Lamp_ExtrnlTailLmpOnReq(byte value)
        {
            if (value > 3)
                return;
            Lamp_ExtrnlTailLmpOnReq = value;
        }

        [Description("Lamp_HdLmpWlcmCmd_0x0:Off/0x1:On/0x2:NotUsed/0x3:ErrorIndicator")]
        public void Set_Lamp_HdLmpWlcmCmd(byte value)
        {
            if (value > 3)
                return;
            Lamp_HdLmpWlcmCmd = value;
        }

        [Description("Lamp_HdLmpEscortShortCmd_0x0:Off/0x1:On/0x2:NotUsed/0x3:ErrorIndicator")]
        public void Set_Lamp_HdLmpEscortShortCmd(byte value)
        {
            if (value > 3)
                return;
            Lamp_HdLmpEscortShortCmd = value;
        }

        [Description("Lamp_AutoLtSnsrNightSta_0x0:Off/0x1:Night/0x2:NotUsed/0x3:ErrorIndicator")]
        public void Set_Lamp_AutoLtSnsrNightSta(byte value)
        {
            if (value > 3)
                return;
            Lamp_AutoLtSnsrNightSta = value;
        }

        [Description("CF_AVN_IFSActVehSpd_Set")]
        public void Set_CF_AVN_IFSActVehSpd_Set(byte value)
        {
            if (value > 15)
                return;
            CF_AVN_IFSActVehSpd_Set = value;
        }

        [Description("CF_AVN_DWL_SelectNvalueSet")]
        public void Set_CF_AVN_DWL_SelectNvalueSet(byte value)
        {
            if (value > 15)
                return;
            CF_AVN_DWL_SelectNvalueSet = value;
        }

        [Description("Warn_DrvDrSwSta_0x0:Close/0x1:Open/0x2:NotUsed/0x3:ErrorIndicator")]
        public void Set_Warn_DrvDrSwSta(byte value)
        {
            if (value > 3)
                return;
            Warn_DrvDrSwSta = value;
        }

        [Description("Warn_AsstDrSwSta_0x0:Close/0x1:Open/0x2:NotUsed/0x3:ErrorIndicator")]
        public void Set_Warn_AsstDrSwSta(byte value)
        {
            if (value > 3)
                return;
            Warn_AsstDrSwSta = value;
        }

        [Description("Warn_RrLftDrSwSta_0x0:Close/0x1:Open/0x2:NotUsed/0x3:ErrorIndicator")]
        public void Set_Warn_RrLftDrSwSta(byte value)
        {
            if (value > 3)
                return;
            Warn_RrLftDrSwSta = value;
        }

        [Description("Warn_RrRtDrSwSta_0x0:Close/0x1:Open/0x2:NotUsed/0x3:ErrorIndicator")]
        public void Set_Warn_RrRtDrSwSta(byte value)
        {
            if (value > 3)
                return;
            Warn_RrRtDrSwSta = value;
        }

        private double _speedFactor = 0.03125d;

        [Description("WHL_SpdFLVal_0~512")]
        public void Set_WHL_SpdFLVal(double value)
        {
            if (value < 0 || value > 512)
                return;
            var speed = (uint)Math.Round(value / _speedFactor, 0, MidpointRounding.AwayFromZero);
            WHL_SpdFLVal = speed >= 0x3FFF ? 0x3FFF : speed;
        }

        [Description("WHL_SpdFRVal_0~512")]
        public void Set_WHL_SpdFRVal(double value)
        {
            if (value < 0 || value > 512)
                return;
            var speed = (uint)Math.Round(value / _speedFactor, 0, MidpointRounding.AwayFromZero);
            WHL_SpdFRVal = speed >= 0x3FFF ? 0x3FFF : speed;
        }

        [Description("WHL_SpdRLVal_0~512")]
        public void Set_WHL_SpdRLVal(double value)
        {
            if (value < 0 || value > 512)
                return;
            var speed = (uint)Math.Round(value / _speedFactor, 0, MidpointRounding.AwayFromZero);
            WHL_SpdRLVal = speed >= 0x3FFF ? 0x3FFF : speed;
        }

        [Description("WHL_SpdRRVal_0~512")]
        public void Set_WHL_SpdRRVal(double value)
        {
            if (value < 0 || value > 512)
                return;
            var speed = (uint)Math.Round(value / _speedFactor, 0, MidpointRounding.AwayFromZero);
            WHL_SpdRRVal = speed >= 0x3FFF ? 0x3FFF : speed;
        }
    }
}