using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace Controller
{
    [Description("CAN-Product,VX1-ISD前后灯")]
    public sealed class Vx1IsdLamp : ControllerBase
    {
        public CanBus CanFd;

        [Description("R/W,LED点亮灰度值")]
        public string LedGray = "255";

        [Description("R,软件版本号")]
        public string SoftwareVersion = string.Empty;

        [Description("R,硬件版本号")]
        public string HardwareVersion = string.Empty;

        [Description("R,故障检测读取结果")]
        public string FaultDetectResult = string.Empty;

        [Description("R,标定电流读取")]
        public string ReadCurrent = string.Empty;

        private readonly CanBus.CanDataPackage _controlPackage =
            new CanBus.CanDataPackage(0xFF, CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data,
                new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });

        private readonly List<CanBus.CanDataPackage> _canDataPackages =
            new List<CanBus.CanDataPackage>();
        private readonly List<uint> _canFdResponseIdList = new List<uint>();
        private Thread MainWorkThread { get; set; }
        private LampType CurrentLampType { get; set; }
        private uint RequestCanId { get; set; }
        private uint ResponseCanId { get; set; }
        private int SendCount { get; set; }
        private int SendGroupIndex { get; set; }
        private bool _isSleep = true;

        public Vx1IsdLamp(string name)
            : base(name)
        {
            if (MainWorkThread != null)
            {
                MainWorkThread.Abort();
                MainWorkThread.Join();
            }

            MainWorkThread =
                new Thread(MainWork) { IsBackground = true };
            MainWorkThread.Start();
        }

        ~Vx1IsdLamp()
        {
            Dispose();
        }

        [Description("切换成ISD前灯B左")]
        public void ChangeToVx1IsdFontBLeft()
        {
            ChangeLampType(LampType.Vx1IsdFontBLeft);
        }

        [Description("切换成ISD前灯B右")]
        public void ChangeToVx1IsdFontBRight()
        {
            ChangeLampType(LampType.Vx1IsdFontBRight);
        }

        [Description("切换成ISD后灯A左")]
        public void ChangeToVx1IsdRclALeft()
        {
            ChangeLampType(LampType.Vx1IsdRclALeft);
        }

        [Description("切换成ISD后灯A右")]
        public void ChangeToVx1IsdRclARight()
        {
            ChangeLampType(LampType.Vx1IsdRclARight);
        }

        private void ChangeLampType(LampType lampType)
        {
            _canDataPackages.Clear();
            _canFdResponseIdList.Clear();
            CurrentLampType = lampType;

            switch (lampType)
            {
                case LampType.Vx1IsdFontALeft:
                    break;

                case LampType.Vx1IsdFontARight:
                    break;

                case LampType.Vx1IsdFontBLeft:
                    RequestCanId = 0x780;
                    ResponseCanId = 0x790;

                    for (var i = 0; i < 6; i++)
                        _canDataPackages.Add(
                            new CanBus.CanDataPackage((uint)(0x500 + i), CanBus.CanProtocol.CanFd,
                                CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[64]));

                    for (var i = 0; i < 2; i++)
                        _canFdResponseIdList.Add((uint)(0x650 + i));
                    break;

                case LampType.Vx1IsdFontBRight:
                    RequestCanId = 0x781;
                    ResponseCanId = 0x791;

                    for (var i = 0; i < 6; i++)
                        _canDataPackages.Add(
                            new CanBus.CanDataPackage((uint)(0x400 + i), CanBus.CanProtocol.CanFd,
                                CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[64]));

                    for (var i = 0; i < 2; i++)
                        _canFdResponseIdList.Add((uint)(0x640 + i));
                    break;

                case LampType.Vx1IsdRclALeft:
                    RequestCanId = 0x782;
                    ResponseCanId = 0x792;

                    for (var i = 0; i < 9; i++)
                        _canDataPackages.Add(
                            new CanBus.CanDataPackage((uint)(0x100 + i), CanBus.CanProtocol.CanFd,
                                CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[64]));

                    for (var i = 0; i < 2; i++)
                        _canFdResponseIdList.Add((uint)(0x710 + i));
                    break;

                case LampType.Vx1IsdRclARight:
                    RequestCanId = 0x783;
                    ResponseCanId = 0x793;

                    for (var i = 0; i < 9; i++)
                        _canDataPackages.Add(
                            new CanBus.CanDataPackage((uint)(0x300 + i), CanBus.CanProtocol.CanFd,
                                CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[64]));

                    for (var i = 0; i < 2; i++)
                        _canFdResponseIdList.Add((uint)(0x730 + i));
                    break;

                case LampType.Vx1IsdRclBLeft:
                    break;

                case LampType.Vx1IsdRclBRight:
                    break;

                default:
                    throw new ArgumentOutOfRangeException("lampType", lampType, null);
            }
        }

        private void MainWork()
        {
            while (MainWorkThread.IsAlive)
            {
                if (!MainWorkThread.IsAlive)
                    break;

                Thread.Sleep(20);

                if (SendCount > 5)
                    SendCount = 0;
                SendCount++;

                if (CanFd == null || _isSleep || !_canDataPackages.Any())
                    continue;

                var sendPackage = new List<CanBus.CanDataPackage>();

                if (SendCount == 1)
                    sendPackage.Add(_controlPackage);

                sendPackage.Add(_canDataPackages[SendGroupIndex]);
                SendGroupIndex++;
                if (SendGroupIndex == _canDataPackages.Count)
                    SendGroupIndex = 0;

                CanFd.SendCanDatas(sendPackage.ToArray());
            }
        }

        /// <summary>
        /// 读取故障信息
        /// </summary>
        [Description("读取故障信息")]
        public void FaultDetect()
        {
            FaultDetectResult = string.Empty;
            if (CanFd == null)
                return;

            foreach (var t in _canFdResponseIdList)
                CanFd.AddDoNotFilterCanId(t);

            CanFd.CanRecvDataPackages.Clear();
            Thread.Sleep(2000);

            if (CanFd.CanRecvDataPackages.Any())
            {
                try
                {
                    foreach (var item in _canFdResponseIdList)
                    {
                        var find = CanFd.CanRecvDataPackages.Find(f => f.CanId == item);
                        if (find == null) continue;
                        var datas = new List<byte>();
                        datas.AddRange(find.CanData);
                        FaultDetectResult += ValueHelper.GetHextStr(datas.ToArray());
                        FaultDetectResult += " ";
                    }

                    FaultDetectResult = FaultDetectResult.TrimEnd(' ');
                }
                catch (Exception)
                {
                    FaultDetectResult = string.Empty;
                }
            }

            foreach (var t in _canFdResponseIdList)
                CanFd.RemoveDoNotFilterCanId(t);
        }

        [Description("以红光或白光单独点亮")]
        public void LedOnRedOrWhite(string ledIndex)
        {
            var index = int.Parse(ledIndex) - 1;

            var ledGroupIndex = index / 32;

            var find = _canDataPackages[ledGroupIndex];

            var ledIndexInThisGroup = index - ledGroupIndex * 32 + 1;
            find.CanData[(ledIndexInThisGroup - 1) * 2] = 0x00;
            byte gray;
            if (byte.TryParse(LedGray,out gray))
            {
                find.CanData[(ledIndexInThisGroup - 1) * 2 + 1] = gray;
            }
            else
            {
                find.CanData[(ledIndexInThisGroup - 1) * 2 + 1] = 0xFF;
            }

            _isSleep = false;
        }

        [Description("以黄光单独点亮")]
        public void LedOnYellow(string ledIndex)
        {
            var index = int.Parse(ledIndex) - 1;

            var ledGroupIndex = index / 32;

            var find = _canDataPackages[ledGroupIndex];

            var ledIndexInThisGroup = index - ledGroupIndex * 32 + 1;
            byte gray;
            if (byte.TryParse(LedGray, out gray))
            {
                find.CanData[(ledIndexInThisGroup - 1) * 2] = gray;
            }
            else
            {
                find.CanData[(ledIndexInThisGroup - 1) * 2] = 0xFF;
            }
           
            find.CanData[(ledIndexInThisGroup - 1) * 2 + 1] = 0x00;

            _isSleep = false;
        }

        [Description("单独熄灭")]
        public void LedOff(string ledIndex)
        {
            var index = int.Parse(ledIndex) - 1;

            var ledGroupIndex = index / 32;

            var find = _canDataPackages[ledGroupIndex];

            var ledIndexInThisGroup = index - ledGroupIndex * 32 + 1;
            find.CanData[(ledIndexInThisGroup - 1) * 2] = 0x00;
            find.CanData[(ledIndexInThisGroup - 1) * 2 + 1] = 0x00;

            _isSleep = true;
        }

        [Description("以红光或白光范围点亮")]
        public void LedRangeOnRedOrWhite(string startEnd)
        {
            var sp = startEnd.Split(':');
            var start = int.Parse(sp[0]);
            var end = int.Parse(sp[1]);

            for (var i = start; i <= end; i++)
                LedOnRedOrWhite(i.ToString());

            _isSleep = false;
        }

        [Description("以红光或白光奇数范围点亮")]
        public void LedRangeOddOnRedOrWhite(string maxIndex)
        {
            for (var i = 1; i <= int.Parse(maxIndex); i++)
                if (i % 2 != 0)
                    LedOnRedOrWhite(i.ToString());
        }

        [Description("以红光或白光偶数范围点亮")]
        public void LedRangeDualOnRedOrWhite(string maxIndex)
        {
            for (var i = 1; i <= int.Parse(maxIndex); i++)
                if (i % 2 == 0)
                    LedOnRedOrWhite(i.ToString());
        }

        [Description("以黄光范围点亮")]
        public void LedRangeOnYellow(string startEnd)
        {
            var sp = startEnd.Split(':');
            var start = int.Parse(sp[0]);
            var end = int.Parse(sp[1]);

            for (var i = start; i <= end; i++)
                LedOnYellow(i.ToString());

            _isSleep = false;
        }

        [Description("以黄光奇数范围点亮")]
        public void LedRangeOddOnYellow(string maxIndex) 
        {
            for (var i = 1; i <= int.Parse(maxIndex); i++)
                if (i % 2 != 0)
                    LedOnYellow(i.ToString());
        }

        [Description("以黄光偶数范围点亮")]
        public void LedRangeDualOnYellow(string maxIndex)
        {
            for (var i = 1; i <= int.Parse(maxIndex); i++)
                if (i % 2 == 0)
                    LedOnYellow(i.ToString());
        }

        [Description("范围熄灭")]
        public void LedRangeOff(string startEnd)
        {
            var sp = startEnd.Split(':');
            var start = int.Parse(sp[0]);
            var end = int.Parse(sp[1]);

            for (var i = start; i <= end; i++)
                LedOff(i.ToString());

            _isSleep = true;
        }

        [Description("读软件版本号")]
        public void ReadSoftwareVersion()
        {
            SoftwareVersion = string.Empty;
            if (CanFd == null)
                return;

            CanFd.AddDoNotFilterCanId(ResponseCanId);
            byte[] echo;
            if (CanFd.CanBusWithUds.TryReadData(RequestCanId, ResponseCanId,
                CanBus.CanType.Standard, CanBus.CanProtocol.CanFd, 0xf1, 0x95, out echo,0x00))
                SoftwareVersion = Encoding.ASCII.GetString(echo);
            CanFd.RemoveDoNotFilterCanId(ResponseCanId);
        }

        [Description("读硬件版本号")]
        public void ReadHardwareVersion()
        {
            HardwareVersion = string.Empty;
            if (CanFd == null)
                return;
            CanFd.AddDoNotFilterCanId(ResponseCanId);
            byte[] echo;
            if (CanFd.CanBusWithUds.TryReadData(RequestCanId, ResponseCanId,
                CanBus.CanType.Standard, CanBus.CanProtocol.CanFd, 0xf1, 0x93, out echo, 0x00))
                HardwareVersion = Encoding.ASCII.GetString(echo);
            CanFd.RemoveDoNotFilterCanId(ResponseCanId);
        }

        [Description("读写电流标定")]
        public void WriteAndReadCurrent(string writeValue)
        {
            ReadCurrent = string.Empty;

            if (CanFd == null)
                return;
            CanFd.AddDoNotFilterCanId(ResponseCanId);

            CanFd.CanBusWithUds.TryEnterExtendedSession(RequestCanId, ResponseCanId, CanBus.CanType.Standard,
                CanBus.CanProtocol.CanFd,0x00);
            Thread.Sleep(59);

            byte[] seedEcho;
            if (CanFd.CanBusWithUds.TryRequestSeed(RequestCanId, ResponseCanId, CanBus.CanType.Standard, 0x01, out seedEcho,pendingByte:0x00))
            {
                Thread.Sleep(50);

                if (CanFd.CanBusWithUds.TrySendKey(RequestCanId, ResponseCanId, CanBus.CanType.Standard, 0x02,
                    new byte[] { 0xEE, 0xDD, 0xCC, 0xBB },pendingByte:0x00))
                {
                    Thread.Sleep(50);
                    var redOrWhiteBytes = Convert.ToByte(writeValue.Substring(0, 2), 16);
                    var yellowBytes = Convert.ToByte(writeValue.Substring(2, 2), 16);

                    if (CanFd.CanBusWithUds.TryWriteData(RequestCanId, ResponseCanId, CanBus.CanType.Standard,
                        CanBus.CanProtocol.CanFd, 0xAA, 0x50, new[] { redOrWhiteBytes, yellowBytes },0x00))
                    {
                        byte[] readEcho;
                        if (CanFd.CanBusWithUds.TryReadData(RequestCanId, ResponseCanId, CanBus.CanType.Standard,
                            CanBus.CanProtocol.CanFd, 0xAA, 0x50, out readEcho,0x00))
                        {
                            try
                            {
                                var str = string.Empty;
                                str += ValueHelper.GetHextStr(readEcho[0]);
                                str += ValueHelper.GetHextStr(readEcho[1]);
                                ReadCurrent = str;
                            }
                            catch (Exception)
                            {
                                ReadCurrent = string.Empty;
                            }
                        }
                    }
                }
            }
            CanFd.RemoveDoNotFilterCanId(ResponseCanId);
        }

        private enum LampType
        {
            Vx1IsdFontALeft,

            Vx1IsdFontARight,

            Vx1IsdFontBLeft,

            Vx1IsdFontBRight,

            Vx1IsdRclALeft,

            Vx1IsdRclARight,

            Vx1IsdRclBLeft,

            Vx1IsdRclBRight,
        }
    }
}
