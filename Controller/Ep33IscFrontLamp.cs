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
    [Description("CAN-Product,EP33-ISC-前灯")]
    public sealed class Ep33IscFrontLamp : ControllerBase
    {
        #region public fields
        public CanBus CanFd;

        [Description("R,软件版本号读取")]
        public string SoftwareVersion = string.Empty;

        [Description("R,硬件版本号读取")]
        public string HardwareVersion = string.Empty;

        [Description("R,故障信息读取")]
        public string FaultRead = string.Empty;
        #endregion

        #region 构造与释放
        public Ep33IscFrontLamp(string name)
            : base(name)
        {
            FrontATurnLedList.Add(0x140,
                "0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80");
            FrontATurnLedList.Add(0x141,
                "0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80");
            FrontATurnLedList.Add(0x142,
                "0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00");
            FrontATurnLedList.Add(0x143,
                "0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00");
            FrontATurnLedList.Add(0x144,
                "0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00");
            FrontATurnLedList.Add(0x145,
                "0x80 0x00 0x80 0x00 0x80 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00");

            FrontATurnLedList.Add(0x240,
                "0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80");
            FrontATurnLedList.Add(0x241,
                "0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80");
            FrontATurnLedList.Add(0x242,
                "0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00");
            FrontATurnLedList.Add(0x243,
                "0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00");
            FrontATurnLedList.Add(0x244,
                "0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00 0x80 0x00");
            FrontATurnLedList.Add(0x245,
                "0x80 0x00 0x80 0x00 0x80 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00");

            FrontADrlLedList.Add(0x140,
                "00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00");
            FrontADrlLedList.Add(0x141,
                "80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00");
            FrontADrlLedList.Add(0x142,
                "80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80");
            FrontADrlLedList.Add(0x143,
                "00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80");
            FrontADrlLedList.Add(0x144,
                "00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80");
            FrontADrlLedList.Add(0x145,
                "00 80 00 80 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
            FrontADrlLedList.Add(0x240,
                "00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00");
            FrontADrlLedList.Add(0x241,
                "80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00");
            FrontADrlLedList.Add(0x242,
                "80 00 80 00 80 00 80 00 80 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80");
            FrontADrlLedList.Add(0x243,
                "00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80");
            FrontADrlLedList.Add(0x244,
                "00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80 00 80");
            FrontADrlLedList.Add(0x245,
                "00 80 00 80 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");

            if (MainWorkThread != null)
            {
                MainWorkThread.Abort();
                MainWorkThread.Join();
            }

            MainWorkThread =
                new Thread(MainWork) { IsBackground = true };
            MainWorkThread.Start();
        }

        ~Ep33IscFrontLamp()
        {
            Dispose();
        }
        #endregion

        #region 内部方法
        private Thread MainWorkThread { get; set; }
        private int SendCount { get; set; }
        private int SendGroupIndex { get; set; }
        private bool _isSleep = true;
        private readonly CanBus.CanDataPackage _controlDataPackage =
           new CanBus.CanDataPackage(0xFF, CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data,
               new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
        private readonly List<CanBus.CanDataPackage> _ledDataPackages =
            new List<CanBus.CanDataPackage>();
        private readonly List<int> _whiteLedList = new List<int>();
        private readonly List<int> _yellowLedList = new List<int>();
        private readonly List<uint> _canFdResponseIdList = new List<uint>();
        private uint _requestCanId;
        private uint _responseCanId;
        private LampType _lampType;

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

                try
                {
                    lock (_ledDataPackages)
                    {
                        if (CanFd == null || _isSleep || !_ledDataPackages.Any())
                            continue;

                        var sendPackage = new List<CanBus.CanDataPackage>();

                        if (SendCount == 1)
                            sendPackage.Add(_controlDataPackage);

                        sendPackage.Add(_ledDataPackages[SendGroupIndex]);
                        SendGroupIndex++;
                        if (SendGroupIndex == _ledDataPackages.Count)
                            SendGroupIndex = 0;

                        CanFd.SendCanDatas(sendPackage.ToArray());
                    }
                }
                catch (Exception)
                {
                    // ignoWhite
                }
            }
        }
        #endregion

        #region 切换灯种
        [Description("切换成L_Front_B")]
        public void ChangeToFontBLeft()
        {
            lock (_ledDataPackages)
            {
                _lampType = LampType.FrontB;

                _canFdResponseIdList.Clear();
                for (uint i = 0x680; i <= 0x682; i++)
                    _canFdResponseIdList.Add(i);

                _isSleep = true;
                _ledDataPackages.Clear();
                _whiteLedList.Clear();
                _yellowLedList.Clear();

                _requestCanId = 0x782;
                _responseCanId = 0x792;

                for (uint i = 0x2C0; i <= 0x2CD; i++)
                    _ledDataPackages.Add(
                             new CanBus.CanDataPackage(
                                 i,
                                 CanBus.CanProtocol.CanFd,
                                 CanBus.CanType.Standard,
                                 CanBus.CanFormat.Data,
                                 new byte[64]));
            }
        }

        [Description("切换成R_Front_B")]
        public void ChangeToFontBRight()
        {
            lock (_ledDataPackages)
            {
                _lampType = LampType.FrontB;

                _canFdResponseIdList.Clear();
                for (uint i = 0x640; i <= 0x642; i++)
                    _canFdResponseIdList.Add(i);

                _isSleep = true;
                _ledDataPackages.Clear();
                _whiteLedList.Clear();
                _yellowLedList.Clear();

                _requestCanId = 0x783;
                _responseCanId = 0x793;

                for (uint i = 0x1C0; i <= 0x1CD; i++)
                    _ledDataPackages.Add(
                             new CanBus.CanDataPackage(
                                 i,
                                 CanBus.CanProtocol.CanFd,
                                 CanBus.CanType.Standard,
                                 CanBus.CanFormat.Data,
                                 new byte[64]));
            }
        }

        [Description("切换成L_Front_A")]
        public void ChangeToFrontALeft()
        {
            lock (_ledDataPackages)
            {
                _lampType = LampType.FrontA;

                _canFdResponseIdList.Clear();
                for (uint i = 0x670; i <= 0x671; i++)
                    _canFdResponseIdList.Add(i);

                _isSleep = true;
                _ledDataPackages.Clear();
                _whiteLedList.Clear();
                _yellowLedList.Clear();

                _requestCanId = 0x780;
                _responseCanId = 0x790;

                for (uint i = 0x240; i <= 0x245; i++)
                    _ledDataPackages.Add(
                             new CanBus.CanDataPackage(
                                 i,
                                 CanBus.CanProtocol.CanFd,
                                 CanBus.CanType.Standard,
                                 CanBus.CanFormat.Data,
                                 new byte[64]));
            }
        }

        [Description("切换成R_Front_A")]
        public void ChangeToFrontARight()
        {
            lock (_ledDataPackages)
            {
                _lampType = LampType.FrontA;

                _canFdResponseIdList.Clear();
                for (uint i = 0x630; i <= 0x631; i++)
                    _canFdResponseIdList.Add(i);

                _isSleep = true;
                _ledDataPackages.Clear();
                _whiteLedList.Clear();
                _yellowLedList.Clear();

                _requestCanId = 0x781;
                _responseCanId = 0x791;

                for (uint i = 0x140; i <= 0x145; i++)
                    _ledDataPackages.Add(
                             new CanBus.CanDataPackage(
                                 i,
                                 CanBus.CanProtocol.CanFd,
                                 CanBus.CanType.Standard,
                                 CanBus.CanFormat.Data,
                                 new byte[64]));
            }
        }

        public Dictionary<uint, string> FrontATurnLedList = new Dictionary<uint, string>();
        public Dictionary<uint, string> FrontADrlLedList = new Dictionary<uint, string>();

        #endregion

        #region LED相关
        [Description("点亮所有黄光LED")]
        public void YellowLedOn(string grayValue)
        {
            if (string.IsNullOrEmpty(grayValue))
                return;

            byte gray;
            if (!byte.TryParse(grayValue, out gray))
                return;

            lock (_ledDataPackages)
            {
                foreach (var t in _ledDataPackages)
                    for (var j = 0; j < t.CanDataLen; j++)
                        t.CanData[j] = 0x00;

                if (_lampType == LampType.FrontB)
                {
                    foreach (var t in _ledDataPackages)
                        for (var j = 0; j < t.CanData.Length; j = j + 2)
                            t.CanData[j] = gray;
                }
                else
                {
                    foreach (var t in _ledDataPackages)
                    {
                        if (!FrontATurnLedList.ContainsKey(t.CanId))
                            continue;
                        var str = FrontATurnLedList[t.CanId].Split(' ');

                        if (str.Length != t.CanData.Length)
                            continue;
                        for (var i = 0; i < t.CanData.Length; i++)
                        {
                            if (str[i].EndsWith("00"))
                                t.CanData[i] = 0x00;
                            else
                                t.CanData[i] = gray;
                        }
                    }
                }
            }

            _isSleep = false;
        }

        [Description("点亮所有白光LED")]
        public void WhiteLedOn(string grayValue)
        {
            if (string.IsNullOrEmpty(grayValue))
                return;

            byte gray;
            if (!byte.TryParse(grayValue, out gray))
                return;

            lock (_ledDataPackages)
            {
                foreach (var t in _ledDataPackages)
                    for (var j = 0; j < t.CanDataLen; j++)
                        t.CanData[j] = 0x00;

                if (_lampType == LampType.FrontB)
                {
                    foreach (var t in _ledDataPackages)
                        for (var j = 0; j < t.CanData.Length; j = j + 2)
                            t.CanData[j + 1] = gray;
                }
                else
                {
                    foreach (var t in _ledDataPackages)
                    {
                        if (!FrontADrlLedList.ContainsKey(t.CanId))
                            continue;
                        var str = FrontADrlLedList[t.CanId].Split(' ');

                        if (str.Length != t.CanData.Length)
                            continue;
                        for (var i = 0; i < t.CanData.Length; i++)
                        {
                            if (str[i].EndsWith("00"))
                                t.CanData[i] = 0x00;
                            else
                                t.CanData[i] = gray;
                        }
                    }
                }
            }


            _isSleep = false;
        }

        [Description("点亮所有单数白光LED")]
        public void WhiteLedOddOn(string grayValue)
        {
            if (string.IsNullOrEmpty(grayValue))
                return;

            byte gray;
            if (!byte.TryParse(grayValue, out gray))
                return;

            lock (_ledDataPackages)
            {
                foreach (var t in _ledDataPackages)
                    for (var j = 0; j < t.CanDataLen; j++)
                        t.CanData[j] = 0x00;

                if (_lampType == LampType.FrontB)
                {
                    foreach (var t in _ledDataPackages)
                        for (var j = 0; j < t.CanData.Length; j = j + 4)
                            t.CanData[j + 1] = gray;
                }
                else
                {
                    var isReverse = false;

                    foreach (var t in _ledDataPackages)
                    {
                        if (!FrontADrlLedList.ContainsKey(t.CanId))
                            continue;
                        var str = FrontADrlLedList[t.CanId].Split(' ');

                        if (str.Length != t.CanData.Length)
                            continue;
                        for (var i = 0; i < t.CanData.Length; i++)
                        {
                            if (str[i].EndsWith("00"))
                                t.CanData[i] = 0x00;
                            else
                            {
                                if (isReverse)
                                    t.CanData[i] = gray;
                                else
                                    t.CanData[i] = 0x00;

                                isReverse = !isReverse;
                            }
                        }
                    }
                }
            }

            _isSleep = false;
        }

        [Description("点亮所有双数白光LED")]
        public void WhiteLedEvenOn(string grayValue)
        {
            if (string.IsNullOrEmpty(grayValue))
                return;

            byte gray;
            if (!byte.TryParse(grayValue, out gray))
                return;

            lock (_ledDataPackages)
            {
                foreach (var t in _ledDataPackages)
                    for (var j = 0; j < t.CanDataLen; j++)
                        t.CanData[j] = 0x00;

                if (_lampType == LampType.FrontB)
                {
                    foreach (var t in _ledDataPackages)
                        for (var j = 0; j < t.CanData.Length; j = j + 4)
                            t.CanData[j + 3] = gray;
                }
                else
                {
                    var isReverse = true;

                    foreach (var t in _ledDataPackages)
                    {
                        if (!FrontADrlLedList.ContainsKey(t.CanId))
                            continue;
                        var str = FrontADrlLedList[t.CanId].Split(' ');

                        if (str.Length != t.CanData.Length)
                            continue;
                        for (var i = 0; i < t.CanData.Length; i++)
                        {
                            if (str[i].EndsWith("00"))
                                t.CanData[i] = 0x00;
                            else
                            {
                                if (isReverse)
                                    t.CanData[i] = gray;
                                else
                                    t.CanData[i] = 0x00;

                                isReverse = !isReverse;
                            }
                        }
                    }
                }
            }

            _isSleep = false;
        }

        [Description("点亮所有单数黄光LED")]
        public void YellowLedOddOn(string grayValue)
        {
            if (string.IsNullOrEmpty(grayValue))
                return;

            byte gray;
            if (!byte.TryParse(grayValue, out gray))
                return;

            lock (_ledDataPackages)
            {
                foreach (var t in _ledDataPackages)
                    for (var j = 0; j < t.CanDataLen; j++)
                        t.CanData[j] = 0x00;

                if (_lampType == LampType.FrontB)
                {
                    foreach (var t in _ledDataPackages)
                        for (var j = 0; j < t.CanData.Length; j = j + 4)
                            t.CanData[j] = gray;
                }
                else
                {
                    var isReverse = false;

                    foreach (var t in _ledDataPackages)
                    {
                        if (!FrontATurnLedList.ContainsKey(t.CanId))
                            continue;
                        var str = FrontATurnLedList[t.CanId].Split(' ');

                        if (str.Length != t.CanData.Length)
                            continue;
                        for (var i = 0; i < t.CanData.Length; i++)
                        {
                            if (str[i].EndsWith("00"))
                                t.CanData[i] = 0x00;
                            else
                            {
                                if (isReverse)
                                    t.CanData[i] = gray;
                                else
                                    t.CanData[i] = 0x00;

                                isReverse = !isReverse;
                            }
                        }
                    }
                }
            }

            _isSleep = false;
        }

        [Description("点亮所有双数黄光LED")]
        public void YellowLedEvenOn(string grayValue)
        {
            if (string.IsNullOrEmpty(grayValue))
                return;

            byte gray;
            if (!byte.TryParse(grayValue, out gray))
                return;

            lock (_ledDataPackages)
            {
                foreach (var t in _ledDataPackages)
                    for (var j = 0; j < t.CanDataLen; j++)
                        t.CanData[j] = 0x00;

                if (_lampType == LampType.FrontB)
                {
                    foreach (var t in _ledDataPackages)
                        for (var j = 0; j < t.CanData.Length; j = j + 4)
                            t.CanData[j + 2] = gray;
                }
                else
                {
                    var isReverse = true;

                    foreach (var t in _ledDataPackages)
                    {
                        if (!FrontATurnLedList.ContainsKey(t.CanId))
                            continue;
                        var str = FrontATurnLedList[t.CanId].Split(' ');

                        if (str.Length != t.CanData.Length)
                            continue;
                        for (var i = 0; i < t.CanData.Length; i++)
                        {
                            if (str[i].EndsWith("00"))
                                t.CanData[i] = 0x00;
                            else
                            {
                                if (isReverse)
                                    t.CanData[i] = gray;
                                else
                                    t.CanData[i] = 0x00;

                                isReverse = !isReverse;
                            }
                        }
                    }
                }
            }

            _isSleep = false;
        }

        [Description("点亮所有LED")]
        public void AllLedOn(string grayValue)
        {
            if (string.IsNullOrEmpty(grayValue))
                return;

            byte gray;
            if (!byte.TryParse(grayValue, out gray))
                return;

            lock (_ledDataPackages)
            {
                foreach (var t in _ledDataPackages)
                    for (var j = 0; j < t.CanDataLen; j++)
                        t.CanData[j] = 0x00;

                if (_lampType == LampType.FrontB)
                {
                    foreach (var t in _ledDataPackages)
                    {
                        for (var j = 0; j < t.CanData.Length; j = j + 2)
                        {
                            t.CanData[j] = gray;
                            t.CanData[j + 1] = gray;
                        }
                    }
                }
                else
                {
                    foreach (var t in _ledDataPackages)
                    {
                        if (FrontATurnLedList.ContainsKey(t.CanId))
                        {
                            var str = FrontATurnLedList[t.CanId].Split(' ');

                            if (str.Length == t.CanData.Length)
                            {
                                for (var i = 0; i < t.CanData.Length; i++)
                                {
                                    if (str[i].EndsWith("00"))
                                    {
                                        t.CanData[i] = 0x00;
                                    }
                                    else
                                    {
                                        t.CanData[i] = gray;
                                    }
                                }
                            }
                        }

                        if (FrontADrlLedList.ContainsKey(t.CanId))
                        {
                            var str = FrontADrlLedList[t.CanId].Split(' ');

                            if (str.Length == t.CanData.Length)
                            {
                                for (var i = 0; i < t.CanData.Length; i++)
                                {
                                    if (str[i].EndsWith("00"))
                                    {
                                        t.CanData[i] = 0x00;
                                    }
                                    else
                                    {
                                        t.CanData[i] = gray;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            _isSleep = false;
        }

        [Description("关闭所有LED")]
        public void AllLedOff()
        {
            _isSleep = true;

            lock (_ledDataPackages)
            {
                foreach (var p in _ledDataPackages)
                {
                    for (var i = 0; i < p.CanDataLen; i++)
                        p.CanData[i] = 0x00;
                }
            }
        }

        #endregion

        #region 信息读取相关
        [Description("读软件版本号")]
        public void ReadSoftwareVersion()
        {
            SoftwareVersion = string.Empty;
            if (CanFd == null)
                return;

            byte[] echo;
            if (CanFd.CanBusWithUds.TryReadData(_requestCanId, _responseCanId,
                CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xf1, 0x95, out echo, 0x00))
                SoftwareVersion = Encoding.ASCII.GetString(echo);
            else
            {
                Thread.Sleep(500);
                if (CanFd.CanBusWithUds.TryReadData(_requestCanId, _responseCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xf1, 0x95, out echo, 0x00))
                    SoftwareVersion = Encoding.ASCII.GetString(echo);
            }
        }

        [Description("读硬件版本号")]
        public void ReadHardwareVersion()
        {
            HardwareVersion = string.Empty;
            if (CanFd == null)
                return;

            byte[] echo;
            if (CanFd.CanBusWithUds.TryReadData(_requestCanId, _responseCanId,
                CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xf1, 0x93, out echo, 0x00))
                HardwareVersion = Encoding.ASCII.GetString(echo);
            else
            {
                Thread.Sleep(500);
                if (CanFd.CanBusWithUds.TryReadData(_requestCanId, _responseCanId,
                CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xf1, 0x93, out echo, 0x00))
                    HardwareVersion = Encoding.ASCII.GetString(echo);
            }
        }

        /// <summary>
        /// 读取故障信息
        /// </summary>
        [Description("读取故障信息")]
        public void FaultDetect()
        {
            FaultRead = string.Empty;
            if (CanFd == null)
                return;

            foreach (var t in _canFdResponseIdList)
                CanFd.AddDoNotFilterCanId(t);

            CanFd.CanRecvDataPackages.Clear();
            Thread.Sleep(2000);

            if (CanFd.CanRecvDataPackages.Any())
            {
                var temp = CanFd.CanRecvDataPackages.ToArray();

                try
                {
                    foreach (var find in _canFdResponseIdList.Select(item => temp.ToList().Find(f => f.CanId == item)))
                    {
                        if (find == null)
                        {
                            FaultRead = string.Empty;
                            break;
                        }
                        var datas = new List<byte>();
                        datas.AddRange(find.CanData);
                        FaultRead += ValueHelper.GetHextStr(datas.ToArray());
                        FaultRead += " ";
                    }

                    FaultRead = FaultRead.TrimEnd(' ');
                }
                catch (Exception)
                {
                    FaultRead = string.Empty;
                }
            }

            foreach (var t in _canFdResponseIdList)
                CanFd.RemoveDoNotFilterCanId(t);
        }
        #endregion

        internal enum LampType
        {
            FrontB,

            FrontA
        }
    }
}
