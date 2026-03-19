using CommonUtility;
using CommonUtility.BusLoader;
using CommonUtility.FileOperator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

namespace Controller
{
    [Description("CAN-Product,P12LM1前灯-融合方案")]
    public sealed class P12LM1HeadLamp : ControllerBase
    {
        public CanBus Can;

        [Description("R/W,是否为左灯")]
        public bool IsLeftLamp = false;

        /// <summary>
        /// 右远光
        /// </summary>
        [Description("R/W,右远光")]
        public string RightHbLedGray;

        /// <summary>
        /// 左远光
        /// </summary>
        [Description("R/W,左远光")]
        public string LeftHbLedGray;

        /// <summary>
        /// 右近光
        /// </summary>
        [Description("R/W,右近光")]
        public string RightLbLedGray;

        /// <summary>
        /// 左近光
        /// </summary>
        [Description("R/W,左近光")]
        public string LeftLbLedGray;

        /// <summary>
        /// 是否影响线控制器控制
        /// </summary>
        [Description("R/W,是否影响线控制器控制")]
        public bool HdLampRespsCmd = false;

        public P12LM1HeadLamp(string name)
            : base(name)
        {
            for (var i = 0; i < 50; i++)
            {
                _leftDrlPlGray.Add(0);
                _rightDrlPlGray.Add(0);
            }

            for (var i = 0; i < 44; i++)
            {
                _leftTiGray.Add(0);
                _rightTiGray.Add(0);
            }

            if (MainWorkThread != null)
            {
                MainWorkThread.Abort();
                MainWorkThread.Join();
            }

            MainWorkThread =
                new Thread(MainWork) { IsBackground = true };
            MainWorkThread.Start();
        }

        ~P12LM1HeadLamp()
        {
            Dispose();
        }

        [Description("开启CAN消息")]
        public void StartCanLinMsg()
        {
            //ResetLed();
            _isSleep = false;
        }

        [Description("关闭CAN消息")]
        public void StopCanLinMsg()
        {
            _isSleep = true;
            //ResetLed();
        }

        #region 内部方法

        private readonly List<byte> _leftDrlPlGray = new List<byte>();
        private readonly List<byte> _rightDrlPlGray = new List<byte>();

        private byte _LeftDrlGray = 0;
        private byte _RightDrlGray = 0;

        private readonly List<byte> _leftTiGray = new List<byte>();
        private readonly List<byte> _rightTiGray = new List<byte>();

        private Thread MainWorkThread { get; set; }
        private bool _isSleep = true;
        private byte _hdlmpLvlngReq;

        private readonly object _lockSend = new object();

        private void MainWork()
        {
            while (MainWorkThread.IsAlive)
            {
                if (!MainWorkThread.IsAlive)
                    break;

                Thread.Sleep(10);

                if (Can == null)
                    continue;

                if (_isSleep)
                    continue;

                lock (_lockSend)
                {
                    var sendCanDats = new List<CanBus.CanDataPackage>();

                    #region 远近光
                    byte leftLbLedGray;
                    byte rightLbLedGray;
                    byte leftHbLedGray;
                    byte rightHbLedGray;

                    if (!byte.TryParse(LeftLbLedGray, out leftLbLedGray))
                    {
                        leftLbLedGray = 0x00;
                    }
                    else
                    {
                        if (leftLbLedGray > 0x64)
                        {
                            leftLbLedGray = 0x64;
                        }
                    }

                    if (!byte.TryParse(RightLbLedGray, out rightLbLedGray))
                    {
                        rightLbLedGray = 0x00;
                    }
                    else
                    {
                        if (rightLbLedGray > 0x64)
                        {
                            rightLbLedGray = 0x64;
                        }
                    }

                    if (!byte.TryParse(LeftHbLedGray, out leftHbLedGray))
                    {
                        leftHbLedGray = 0x00;
                    }
                    else
                    {
                        if (leftHbLedGray > 0x64)
                        {
                            leftHbLedGray = 0x64;
                        }
                    }

                    if (!byte.TryParse(RightHbLedGray, out rightHbLedGray))
                    {
                        rightHbLedGray = 0x00;
                    }
                    else
                    {
                        if (rightHbLedGray > 0x64)
                        {
                            rightHbLedGray = 0x64;
                        }
                    }
                    sendCanDats.Add(new CanBus.CanDataPackage(0x13C, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                    CanBus.CanFormat.Data,
                    new byte[] { leftLbLedGray, rightLbLedGray, leftHbLedGray, rightHbLedGray, 0x00, 0x00, 0x00, 0x00 }));
                    Console.WriteLine("远近光：" + leftLbLedGray + rightLbLedGray + leftHbLedGray + rightHbLedGray + _LeftDrlGray);
                    #endregion

                    if (IsLeftLamp)
                    {
                        #region DRL/PL/TI 左
                        sendCanDats.Add(new CanBus.CanDataPackage(0x214, CanBus.CanProtocol.Can,
                         CanBus.CanType.Standard, CanBus.CanFormat.Data,
                         new byte[] { _LeftDrlGray, 0x00, _LeftDrlGray, 0x00, _LeftDrlGray, 0x00, 0x00, _LeftDrlGray }));
                        sendCanDats.Add(new CanBus.CanDataPackage(0x21F, CanBus.CanProtocol.Can,
                        CanBus.CanType.Standard, CanBus.CanFormat.Data,
                        new byte[] { 0x00, 0x00, 0x00, _LeftDrlGray, 0x00, 0x00, 0x00, 0x00 }));

                        for (var i = 0x215; i <= 0x21E; i++)
                        {
                            //sendCanDats.Add(new CanBus.CanDataPackage((uint)i, CanBus.CanProtocol.Can,
                            //         CanBus.CanType.Standard, CanBus.CanFormat.Data,
                            //         new byte[] { 0x00, 0x00, 0x00, _LeftDrlGray, 0x00, 0x00, _LeftDrlGray, 0x00 }));

                            sendCanDats.Add(new CanBus.CanDataPackage((uint)i, CanBus.CanProtocol.Can,
                                 CanBus.CanType.Standard, CanBus.CanFormat.Data,
                                 new byte[] { 0x00, 0x00, 0x00, _LeftDrlGray, 0x00, 0x00, 0x00, _LeftDrlGray }));
                        }
                        #endregion
                    }
                    else
                    {
                        #region DRL/PL/TI 右
                        sendCanDats.Add(new CanBus.CanDataPackage(0x200, CanBus.CanProtocol.Can,
                            CanBus.CanType.Standard, CanBus.CanFormat.Data,
                            new byte[] { _RightDrlGray, 0x00, _RightDrlGray, 0x00, _RightDrlGray, 0x00, 0x00, _RightDrlGray }));
                        sendCanDats.Add(new CanBus.CanDataPackage(0x20B, CanBus.CanProtocol.Can,
                        CanBus.CanType.Standard, CanBus.CanFormat.Data,
                        new byte[] { 0x00, 0x00, 0x00, _RightDrlGray, 0x00, 0x00, 0x00, 0x00 }));
                        for (var i = 0x201; i <= 0x20A; i++)
                        {
                            //sendCanDats.Add(new CanBus.CanDataPackage((uint)i, CanBus.CanProtocol.Can,
                            //            CanBus.CanType.Standard, CanBus.CanFormat.Data,
                            //            new byte[] { 0x00, 0x00, 0x00, _RightDrlGray, 0x00, 0x00, _RightDrlGray, 0x00 }));

                            sendCanDats.Add(new CanBus.CanDataPackage((uint)i, CanBus.CanProtocol.Can,
                                       CanBus.CanType.Standard, CanBus.CanFormat.Data,
                                       new byte[] { 0x00, 0x00, 0x00, _RightDrlGray, 0x00, 0x00, 0x00, _RightDrlGray }));
                        }

                        #endregion
                    }
                    #region 
                    for (var i = 0x200; i <= 0x227; i++)
                    {
                        if (i >= 0x214 && i <= 0x21F)
                            continue;
                        if (i >= 0x200 && i <= 0x20B)
                            continue;
                        sendCanDats.Add(new CanBus.CanDataPackage((uint)i, CanBus.CanProtocol.Can,
                                    CanBus.CanType.Standard, CanBus.CanFormat.Data,
                                    new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }));
                    }

                    sendCanDats.Add(new CanBus.CanDataPackage(0x287, CanBus.CanProtocol.Can,
                                CanBus.CanType.Standard, CanBus.CanFormat.Data,
                                new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }));
                    sendCanDats.Add(new CanBus.CanDataPackage(0x35C, CanBus.CanProtocol.Can,
                            CanBus.CanType.Standard, CanBus.CanFormat.Data,
                            new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }));

                    #endregion

                    Can.SendCanDatas(sendCanDats.ToArray());
                    continue;
                }
            }
        }

        #endregion

        #region Motor Control

        [Description("HdlmpLvlngReq")]
        public void HdlmpLvlngReq(string value)
        {
            byte hdlmpLvlngReq;
            if (!byte.TryParse(value, out hdlmpLvlngReq))
                return;
            if (hdlmpLvlngReq <= 0x07)
                _hdlmpLvlngReq = hdlmpLvlngReq;
        }

        public string canresult;
        [Description("BodyCan消息检测")]
        public void CanIsHaveMsgCheck(int idx)
        {
            canresult = "无数据";

            if (Can == null)
                return;

            //远光
            if (idx == 1)
            {
                var findCnaIdList = new List<uint> { 0x304 };
                var canlist = new byte[] { 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00 };
                if (!IsLeftLamp)
                {
                    findCnaIdList = new List<uint> { 0x305 };
                    canlist = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
                }


                foreach (var canId in findCnaIdList)
                    lock (_lockSend)
                        Can.AddDoNotFilterCanId(canId);

                Can.CanRecvDataPackages.Clear();
                Thread.Sleep(2550);

                foreach (var canId in findCnaIdList)
                    lock (_lockSend)
                        Can.RemoveDoNotFilterCanId(canId);

                List<byte[]> canDatas = new List<byte[]>();
                foreach (var item in Can.CanRecvDataPackages)
                {
                    if (findCnaIdList[0] == item.CanId)
                        canDatas.Add(item.CanData);

                }
                if (canDatas.Count > 0)
                {
                    var sdf = canDatas.Where(t => { for (int i = 0; i < t.Length; i++) { if (t[i] != canlist[i]) return false; } return true; }).Any();
                    if (sdf)
                    {
                        canresult = "OK";
                    }
                    else
                    {
                        canresult = "NG";
                    }
                }

                //   var isOk = findCnaIdList.All(canId => Can.CanRecvDataPackages.FindAll(f => f.CanId == canId).Any());
                ////  var isOk1 = canlist.All(candata => Can.CanRecvDataPackages.FindAll(f => f.CanData == candata).Any());
                //if (isOk)
                //{

                //}



            }

            //近光
            else if (idx == 2)
            {
                var findCnaIdList = new List<uint> { 0x304 };
                var canlist = new byte[] { 0x00, 0x00, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00 };
                if (!IsLeftLamp)
                {
                    findCnaIdList = new List<uint> { 0x305 };
                    canlist = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40 };
                }

                foreach (var canId in findCnaIdList)
                    lock (_lockSend)
                        Can.AddDoNotFilterCanId(canId);

                Can.CanRecvDataPackages.Clear();
                Thread.Sleep(2550);

                foreach (var canId in findCnaIdList)
                    lock (_lockSend)
                        Can.RemoveDoNotFilterCanId(canId);

                List<byte[]> canDatas = new List<byte[]>();
                foreach (var item in Can.CanRecvDataPackages)
                {
                    if (findCnaIdList[0] == item.CanId)
                        canDatas.Add(item.CanData);

                }
                if (canDatas.Count > 0)
                {
                    //if (canDatas.Any(data => data.Equals(canlist)))
                    var sdf = canDatas.Where(t => { for (int i = 0; i < t.Length; i++) { if (t[i] != canlist[i]) return false; } return true; }).Any();
                    if (sdf)
                    {
                        canresult = "OK";
                    }
                    else
                    {
                        canresult = "NG";
                    }
                }


            }
            else if (idx == 3)
            {
                var findCnaIdList = new List<uint> { 0x300 };
                var canlist = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04 };
                if (!IsLeftLamp)
                {
                    findCnaIdList = new List<uint> { 0x302 };
                    canlist = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40 };
                }

                foreach (var canId in findCnaIdList)
                    lock (_lockSend)
                        Can.AddDoNotFilterCanId(canId);

                Can.CanRecvDataPackages.Clear();
                Thread.Sleep(2550);

                foreach (var canId in findCnaIdList)
                    lock (_lockSend)
                        Can.RemoveDoNotFilterCanId(canId);

                List<byte[]> canDatas = new List<byte[]>();
                foreach (var item in Can.CanRecvDataPackages)
                {
                    if (findCnaIdList[0] == item.CanId)
                        canDatas.Add(item.CanData);

                }
                if (canDatas.Count > 0)
                {
                    //if (canDatas.Any(data => data.Equals(canlist)))          
                    if (canDatas.Where(t => { for (int i = 0; i < t.Length; i++) { if (t[i] != canlist[i]) return false; } return true; }).Any())
                    {
                        canresult = "OK";
                    }
                    else
                    {
                        canresult = "NG";
                    }
                }



            }


        }

        #endregion

        #region LED Control

        [Description("DRL/PL打开")]
        public void DrlPlOn(string grayValue)
        {
            if (IsLeftLamp)
            {
                _LeftDrlGray = byte.Parse(grayValue);
                for (var i = 0; i < _leftDrlPlGray.Count; i++)
                    WLedSingleControl(i.ToString(), grayValue);
            }
            else
            {
                _RightDrlGray = byte.Parse(grayValue);

                for (var i = 0; i < _rightDrlPlGray.Count; i++)
                    WLedSingleControl(i.ToString(), grayValue);
            }
        }

        [Description("DRL/PL关闭")]
        public void DrlPlOff()
        {
            _LeftDrlGray = 0x00;
            _RightDrlGray = 0x00;
            if (IsLeftLamp)
            {
                _LeftDrlGray = 0x00;
                for (var i = 0; i < _leftDrlPlGray.Count; i++)
                    WLedSingleControl(i.ToString(), "0");
            }
            else
            {
                _RightDrlGray = 0x00;
                for (var i = 0; i < _rightDrlPlGray.Count; i++)
                    WLedSingleControl(i.ToString(), "0");
            }
        }

        private void WLedSingleControl(string ledIndex, string grayValue)
        {
            if (string.IsNullOrEmpty(grayValue))
                return;

            byte gray;
            if (!byte.TryParse(grayValue, out gray))
                return;

            int index;
            if (!int.TryParse(ledIndex, out index))
                return;

            if (IsLeftLamp)
            {
                if (index < 0 || index > _leftDrlPlGray.Count)
                    return;

                //index--;
                _leftDrlPlGray[index] = gray;
            }
            else
            {
                if (index < 0 || index > _rightDrlPlGray.Count)
                    return;

                //index--;
                _rightDrlPlGray[index] = gray;
            }
        }

        [Description("TI打开")]
        public void TiOn(string grayValue)
        {
            if (IsLeftLamp)
            {
                for (var i = 0; i < _leftTiGray.Count; i++)
                {
                    YLedSingleControl(i.ToString(), grayValue);
                }
            }
            else
            {
                for (var i = 0; i < _rightTiGray.Count; i++)
                {
                    YLedSingleControl(i.ToString(), grayValue);
                }
            }
        }

        [Description("TI关闭")]
        public void TiOff()
        {
            if (IsLeftLamp)
            {
                for (var i = 0; i < _leftTiGray.Count; i++)
                {
                    YLedSingleControl(i.ToString(), "0");
                }
            }
            else
            {
                for (var i = 0; i < _rightTiGray.Count; i++)
                {
                    YLedSingleControl(i.ToString(), "0");
                }
            }
        }

        private void YLedSingleControl(string ledIndex, string grayValue)
        {
            if (string.IsNullOrEmpty(grayValue))
                return;

            byte gray;
            if (!byte.TryParse(grayValue, out gray))
                return;

            int index;
            if (!int.TryParse(ledIndex, out index))
                return;

            if (IsLeftLamp)
            {
                if (index < 0 || index > _leftTiGray.Count)
                    return;

                //index--;
                _leftTiGray[index] = gray;
            }
            else
            {
                if (index < 0 || index > _rightTiGray.Count)
                    return;

                //index--;
                _rightTiGray[index] = gray;
            }
        }

        //[Description("WLed单独打开")]
        //public void WLedSingleOn(string ledIndex, string grayValue)
        //{
        //    WLedSingleControl(ledIndex, grayValue);
        //}

        //[Description("WLed单独关闭")]
        //public void WLedSingleOff(string ledIndex)
        //{
        //    WLedSingleControl(ledIndex, "0");
        //}

        [Description("YLed单独打开")]
        public void YLedSingleOn(string ledIndex, string grayValue)
        {
            YLedSingleControl(ledIndex, grayValue);
        }

        [Description("YLed单独关闭")]
        public void YLedSingleOff(string ledIndex)
        {
            YLedSingleControl(ledIndex, "0");
        }

        [Description("Led全部关闭")]
        public void LedAllOff()
        {


            LeftHbLedGray = "0";
            RightHbLedGray = "0";
            RightLbLedGray = "0";
            LeftLbLedGray = "0";
            _LeftDrlGray = 0;
            _RightDrlGray = 0;

            //for (var i = 0; i < _leftDrlPlGray.Count; i++)
            //    _leftDrlPlGray[i] = 0x00;
            //for (var i = 0; i < _rightDrlPlGray.Count; i++)
            //    _rightDrlPlGray[i] = 0x00;


            for (var i = 0; i < _leftTiGray.Count; i++)
                _leftTiGray[i] = 0x00;

            for (var i = 0; i < _rightTiGray.Count; i++)
                _rightTiGray[i] = 0x00;
        }

        [Description("打开侧转")]
        public void SideTurnOn()
        {
            //for (var i = 0; i < _turnGray.Length; i++)
            //    _turnGray[i] = 0xFF;
        }

        [Description("关闭侧转")]
        public void SideTurnOff()
        {
            //for (var i = 0; i < _turnGray.Length; i++)
            //    _turnGray[i] = 0x00;
        }

        #endregion

        #region 版本信息

        private uint CanDiagnosisRequestPhyCanId = 0x776;
        private uint CanDiagnosisResponseCanId = 0x77E;

        [Description("R,软件版本号1")]
        public string SwVer1 = string.Empty;

        [Description("R,软件版本号2")]
        public string SwVer2 = string.Empty;

        [Description("R,软件版本号3")]
        public string SwVer3 = string.Empty;

        [Description("Read软件版本号")]
        public void ReadSwVer()
        {
            SwVer1 = string.Empty;
            SwVer2 = string.Empty;
            SwVer3 = string.Empty;

            SwVer1 = CommonUtility.ValueHelper.GetStringByAsciiBytes(ReadDidViaCan(0xf1, 0x94), false);
            SwVer2 = CommonUtility.ValueHelper.GetStringByAsciiBytes(ReadDidViaCan(0xfc, 0xf1), false);
            SwVer3 = CommonUtility.ValueHelper.GetStringByAsciiBytes(ReadDidViaCan(0xfc, 0xf2), false);
        }

        private byte[] ReadDidViaCan(byte didHi, byte didLo)
        {
            if (Can == null)
                return new List<byte>().ToArray();

            lock (Can)
            {
                for (int i = 0; i < 3; i++)
                {
                    byte[] echoBytes;
                    if (Can.CanBusWithUds.TryReadData(
                        CanDiagnosisRequestPhyCanId,
                        CanDiagnosisResponseCanId,
                        CanBus.CanType.Standard, CanBus.CanProtocol.Can, didHi, didLo, out echoBytes, timeoutFromMilliseconds: 1000))
                    {
                        if (echoBytes != null)
                            return echoBytes;
                    }

                    Thread.Sleep(500);
                }

                return new List<byte>().ToArray();

                //    byte[] echoBytes;
                //    if (Can.CanBusWithUds.TryReadData(
                //        CanDiagnosisRequestPhyCanId,
                //        CanDiagnosisResponseCanId,
                //        CanBus.CanType.Standard, CanBus.CanProtocol.Can, didHi, didLo, out echoBytes))
                //    {
                //        if (echoBytes != null)
                //            return echoBytes;


                //    Thread.Sleep(250);
                //    if (!Can.CanBusWithUds.TryReadData(
                //        CanDiagnosisRequestPhyCanId,
                //        CanDiagnosisResponseCanId,
                //         CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                //        didHi, didLo, out echoBytes))
                //        {

                //        }

                //        return new List<byte>().ToArray();

                //    if (echoBytes != null)
                //        return echoBytes;

                //    Thread.Sleep(250);
                //    if (!Can.CanBusWithUds.TryReadData(
                //        CanDiagnosisRequestPhyCanId,
                //        CanDiagnosisResponseCanId,
                //         CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                //        didHi, didLo, out echoBytes))
                //        return new List<byte>().ToArray();

                //    if (echoBytes != null)
                //        return echoBytes;

                //    return new List<byte>().ToArray();

                //    Thread.Sleep(250);
                //    if (!Can.CanBusWithUds.TryReadData(
                //        CanDiagnosisRequestPhyCanId,
                //        CanDiagnosisResponseCanId,
                //         CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                //        didHi, didLo, out echoBytes))
                //        return new List<byte>().ToArray();
                //    return echoBytes ?? new List<byte>().ToArray();
            }
        }

        #endregion

        #region APP刷新

        [Description("R,ECU复位结果")]
        public string EcuResetResult = string.Empty;

        [Description("R,下载结果")]
        public string DownloadResult = string.Empty;

        [Description("R,下载耗时")]
        public string DownloadCostTime = string.Empty;

        [Description("R/W,APP文件路径")]
        public string AppFilePath = @"E:\Projects\P12L_M1\刷新文件\20251203\P12L_M1_APP_V0.0.03_20251127_Test.s19";

        private static readonly object LockFile = new object();
        private uint _level1SecurityAccessMask = 0x544D738Au;
        private uint _level2SecurityAccessMask = 0xDA8E29B3u;

        [Description("ECU复位")]
        public string EcuReset()
        {
            EcuResetResult = string.Empty;

            byte[] ecuResetEcho;
            if (!Can.CanBusWithUds
                    .TesterTryRequest(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId, new byte[] { 0x11, 0x01 }, out ecuResetEcho, CanBus.CanType.Standard))
            {
                Thread.Sleep(200);
                Can.CanBusWithUds
                    .TesterTryRequest(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId, new byte[] { 0x11, 0x01 }, out ecuResetEcho, CanBus.CanType.Standard);
            }

            EcuResetResult = ValueHelper.GetHextStr(ecuResetEcho).Replace(" ", "");
            return EcuResetResult; //ValueHelper.GetHextStr(ecuResetEcho).Replace(" ", "");
        }

        [Description("下载")]
        public void DownLoadFile()
        {
            DownloadResult = string.Empty;
            DownloadCostTime = string.Empty;

            if (Can == null)
            {
                DownloadResult = "NG CAN未初始化";
                return;
            }

            var fileListApp = new List<string>();

            lock (LockFile)
            {
                if (!string.IsNullOrEmpty(AppFilePath))
                {
                    if (!File.Exists(AppFilePath))
                    {
                        DownloadResult = "NG APP文件不存在";
                        return;
                    }
                    else
                        fileListApp.Add(AppFilePath);
                }
                else
                {
                    DownloadResult = "NG APP文件不存在";
                    return;
                }
            }

            if (!fileListApp.Any())
            {
                DownloadResult = "NG 未指定APP下载文件";
                return;
            }

            var downloadAction = new Action(() =>
            {
                #region APP

                if (!PreProgramming(CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId, ref DownloadResult))
                    return;

                //if (!Can.CanBusWithUds.TryStartRoutineControl(
                //        CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId, CanBus.CanType.Standard,
                //        Uds14229Helper.RoutineControl.EraseMemory))
                //{
                //    DownloadResult = "NG APP下载擦除内存3101FF00失败失败 " + DownloadResult;
                //    return;
                //}

                if (
                    fileListApp.Select(SRecordFileHelper.GetSRecordLineData)
                    .Select(SRecordFileHelper.GetBlocks)
                    .Any(blocks => !Can.CanBusWithUds.TransferData(
                        CanDiagnosisRequestPhyCanId,
                        CanDiagnosisResponseCanId,
                        CanBus.CanType.Standard,
                        blocks, false, ref DownloadResult, isCrcCheck: false, baseAddr: 0x08000000, is37RequireCrc: 0x00, deley36ms: 5)))
                {
                    DownloadResult = "NG APP下载Block失败";
                    return;
                }

                if (!Can.CanBusWithUds.TryStartRoutineControl(
                       CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                       CanBus.CanType.Standard, Uds14229Helper.RoutineControl.CheckProgrammingIntegrity))
                {
                    DownloadResult = "NG APP下载检查刷新完整性3101DFFF失败";
                    return;
                }

                if (!Can.CanBusWithUds.TryStartRoutineControl(
                        CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                        CanBus.CanType.Standard, Uds14229Helper.RoutineControl.CheckProgrammingDependencies))
                {
                    DownloadResult = "NG APP下载检测程序一致完整性3101FF01失败";
                    return;
                }

                if (!Can.CanBusWithUds.TryStartRoutineControl(
                      CanDiagnosisRequestPhyCanId, CanDiagnosisResponseCanId,
                      CanBus.CanType.Standard, Uds14229Helper.RoutineControl.CalculateSoftwareVerificationNumber, operationBytes: new byte[] { 0x01 }))
                {
                    DownloadResult = "NG APP下载计算软件校验码3101DFFE01失败";
                    return;
                }

                EcuReset();

                #endregion

                DownloadResult = "OK";
            });

            var st = new Stopwatch();
            st.Start();
            downloadAction.Invoke();

            if (DownloadResult != "OK")
            {
                EcuReset();
                Thread.Sleep(1000);
                StartCanLinMsg();
                Thread.Sleep(500);
                StopCanLinMsg();
                Thread.Sleep(50);
                downloadAction.Invoke();
            }

            st.Stop();
            DownloadCostTime = Math.Round(st.ElapsedMilliseconds / 1000f, 2, MidpointRounding.AwayFromZero).ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 预编程
        /// </summary>
        /// <param name="recvCanId"></param>
        /// <param name="result"></param>
        /// <param name="reqCanId"></param>
        /// <returns></returns>
        private bool PreProgramming(uint reqCanId, uint recvCanId, ref string result)
        {
            if (!Can.CanBusWithUds.TryEnterProgrammingSession(
                    reqCanId, recvCanId, CanBus.CanType.Standard, timeOut: 1000))
            {
                result = "NG 进入编程模式1002失败";
                return false;
            }

            byte[] seedBytes;
            if (!Can.CanBusWithUds.TryRequestSeed(
                    reqCanId, recvCanId, CanBus.CanType.Standard, 0x05, out seedBytes, pendingByte: 0x55))
            {
                result = "NG 请求seed2705失败";
                return false;
            }

            var keyBytes = CalcKey(seedBytes, _level2SecurityAccessMask);

            if (!Can.CanBusWithUds.TrySendKey(
                    reqCanId, recvCanId, CanBus.CanType.Standard, 0x06, keyBytes))
            {
                result = "NG 写入key2706失败";
                return false;
            }

            Thread.Sleep(25);

            return true;
        }

        private static IEnumerable<byte> CalcKey(uint wSeed, uint mask)
        {
            uint wTemp;
            uint wTop31Bits;
            uint jj;
            var wLastSeed = wSeed;
            var temp = (byte)((byte)((mask & 0x00000800) >> 10) | ((mask & 0x00200000) >> 21));

            if (temp == 0)
            {
                wTemp = (uint)((wSeed & 0xff000000) >> 24);
            }
            else if (temp == 1)
            {
                wTemp = (uint)((wSeed & 0x00ff0000) >> 16);
            }
            else if (temp == 2)
            {
                wTemp = (uint)((wSeed & 0x0000ff00) >> 8);
            }
            else
            {
                wTemp = (uint)(wSeed & 0x000000ff);
            }
            var sb1 = (uint)((mask & 0x000003FC) >> 2);
            var sb2 = (uint)(((mask & 0x7F800000) >> 23) ^ 0xA5);
            var sb3 = (uint)(((mask & 0x001FE000) >> 13) ^ 0x5A);

            var iterations = (uint)(((wTemp ^ sb1) & sb2) + sb3);

            for (jj = 0; jj < iterations; jj++)
            {
                wTemp = ((wLastSeed & 0x40000000) / 0x40000000) ^ ((wLastSeed & 0x01000000) / 0x01000000) ^ ((wLastSeed & 0x1000) / 0x1000) ^ ((wLastSeed & 0x04) / 0x04);
                var wLsBit = (wTemp & 0x00000001);
                wLastSeed = (uint)(wLastSeed << 1); /* Left Shift the bits */
                wTop31Bits = (uint)(wLastSeed & 0xFFFFFFFE);
                wLastSeed = (uint)(wTop31Bits | wLsBit);
            }

            if ((mask & 0x00000001) != 0)
            {
                wTop31Bits = ((wLastSeed & 0x00FF0000) >> 16) |
                             ((wLastSeed & 0xFF000000) >> 8) |
                             ((wLastSeed & 0x000000FF) << 8) |
                             ((wLastSeed & 0x0000FF00) << 16);
            }
            else
            {
                wTop31Bits = wLastSeed;
            }

            wTop31Bits = wTop31Bits ^ mask;

            var returnBs = BitConverter.GetBytes(wTop31Bits).Reverse();

            return returnBs;
        }

        private static IEnumerable<byte> CalcKey(byte[] wSeeds, uint mask)
        {
            Array.Reverse(wSeeds);
            var wSeed = BitConverter.ToUInt32(wSeeds, 0);
            return CalcKey(wSeed, mask);
        }

        #endregion
    }
}
