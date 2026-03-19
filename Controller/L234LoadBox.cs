using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using CommonUtility;
using CommonUtility.BusLoader;

namespace Controller
{
    [Description("LIN-Product,L234-LoadBox")]
    public sealed class L234LoadBox : ControllerBase
    {
        public LinBus LinWithBaudRate10417;

        [Description("R,读0x01结果")]
        public string Read0X01Str = string.Empty;

        [Description("R,读0x02结果")]
        public string Read0X02Str = string.Empty;

        [Description("R,电源电压")]
        public string 电源电压;
        [Description("R,右侧电机霍尔频率")]
        public string 右侧电机霍尔频率;
        [Description("R,左侧电机霍尔频率")]
        public string 左侧电机霍尔频率;
        [Description("R,右侧开关电压")]
        public string 右侧开关电压;
        [Description("R,左侧开关电压")]
        public string 左侧开关电压;
        [Description("R,右侧电机电流")]
        public string 右侧电机电流;
        [Description("R,左侧电机电流")]
        public string 左侧电机电流;
        [Description("R,左侧开关状态")]
        public string 左侧开关状态;
        [Description("R,右侧开关状态")]
        public string 右侧开关状态;
        [Description("R,VCC1_5V电压")]
        public string Vcc1_5V电压;

        public L234LoadBox(string name)
            : base(name)
        {
            if (_keepNetworkThread != null)
            {
                _keepNetworkThread.Abort();
                _keepNetworkThread.Join();
            }

            _keepNetworkThread = new Thread(KeepNetwork) { IsBackground = true };
            _keepNetworkThread.Start();
        }

        ~L234LoadBox()
        {
            Dispose();
        }

        //[Description("ECU休眠")]
        //public void ModuleSleep()
        //{
        //    _isSleep = true;
        //}

        //[Description("ECU唤醒")]
        //public void ModuleAwake()
        //{
        //    _isSleep = false;
        //}

        [Description("电机100%运行")]
        public void Motor100PerRun()
        {
            if (LinWithBaudRate10417 == null)
                return;
            lock (_lockSend)
                LinWithBaudRate10417.SendMasterLin(0x30, new byte[] { 0x11, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
            _isSleep = false;
        }

        [Description("电机待机模式")]
        public void MotorStandby()
        {
            if (LinWithBaudRate10417 == null)
                return;
            lock (_lockSend)
                LinWithBaudRate10417.SendMasterLin(0x30, new byte[] { 0x99, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
            _isSleep = false;
        }

        [Description("电机休眠模式")]
        public void MotorSleep()
        {
            if (LinWithBaudRate10417 == null)
                return;
            lock (_lockSend)
                LinWithBaudRate10417.SendMasterLin(0x30, new byte[] { 0x22, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
            _isSleep = true;

            Read0X01Str = string.Empty;
            Read0X02Str = string.Empty;
            电源电压 = "未接受到数据";
            右侧电机霍尔频率 = "未接受到数据";
            左侧电机霍尔频率 = "未接受到数据";
            右侧开关电压 = "未接受到数据";
            左侧开关电压 = "未接受到数据";
            右侧电机电流 = "未接受到数据";
            左侧电机电流 = "未接受到数据";
            左侧开关状态 = "未接受到数据";
            右侧开关状态 = "未接受到数据";
            Vcc1_5V电压 = "未接受到数据";
        }

        [Description("开启自动读取状态")]
        public void StartAutoScanStatus()
        {
            _isAutoScaning = true;
        }

        [Description("停止自动读取状态")]
        public void StopAutoScanStatus()
        {
            _isAutoScaning = false;
        }

        private readonly Thread _keepNetworkThread;
        private readonly object _lockSend = new object();
        private bool _isSleep = true;
        private volatile bool _isAutoScaning;

        private void KeepNetwork()
        {
            while (_keepNetworkThread.IsAlive)
            {
                if (!_keepNetworkThread.IsAlive)
                    break;

                Thread.Sleep(50);

                if (LinWithBaudRate10417 == null)
                    continue;

                if (_isSleep)
                    continue;

                lock (_lockSend)
                {
                    byte[] echoBytes0X01;
                    Read0X01Str = LinWithBaudRate10417.SendSlaveLin(0x01, out echoBytes0X01)
                        ? ValueHelper.GetHextStr(echoBytes0X01)
                        : string.Empty;


                    byte[] echoBytes0X02;
                    Read0X02Str = LinWithBaudRate10417.SendSlaveLin(0x02, out echoBytes0X02)
                        ? ValueHelper.GetHextStr(echoBytes0X02)
                        : string.Empty;

                    if (_isAutoScaning)
                    {
                        ReadStateMessage();
                    }

                    Thread.Sleep(50);
                }
            }
        }

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
                    var strData = Read0X01Str;

                    if (!string.IsNullOrEmpty(strData))
                    {
                        var sp = strData.Split(' ');
                        recvData.AddRange(sp.Select(t => Convert.ToByte(t, 16)));
                        电源电压 = (GetIntelValue(recvData.ToArray(), 0, 8) / 10f).ToString(CultureInfo.InvariantCulture);
                        右侧电机霍尔频率 = (GetIntelValue(recvData.ToArray(), 8, 8) / 10f).ToString(CultureInfo.InvariantCulture);
                        左侧电机霍尔频率 = (GetIntelValue(recvData.ToArray(), 16, 8) / 10f).ToString(CultureInfo.InvariantCulture);
                        右侧开关电压 = (GetIntelValue(recvData.ToArray(), 24, 8) / 10f).ToString(CultureInfo.InvariantCulture);
                        左侧开关电压 = (GetIntelValue(recvData.ToArray(), 32, 8) / 10f).ToString(CultureInfo.InvariantCulture);
                        右侧电机电流 = (GetIntelValue(recvData.ToArray(), 40, 8) / 10f).ToString(CultureInfo.InvariantCulture);
                        左侧电机电流 = (GetIntelValue(recvData.ToArray(), 48, 8) / 10f).ToString(CultureInfo.InvariantCulture);
                        左侧开关状态 = GetIntelValue(recvData.ToArray(), 56, 4).ToString();
                        右侧开关状态 = GetIntelValue(recvData.ToArray(), 60, 4).ToString();
                    }
                    else
                    {
                        电源电压 = "未接受到数据";
                        右侧电机霍尔频率 = "未接受到数据";
                        左侧电机霍尔频率 = "未接受到数据";
                        右侧开关电压 = "未接受到数据";
                        左侧开关电压 = "未接受到数据";
                        右侧电机电流 = "未接受到数据";
                        左侧电机电流 = "未接受到数据";
                        左侧开关状态 = "未接受到数据";
                        右侧开关状态 = "未接受到数据";
                    }
                }

                {
                    var recvData = new List<byte>();
                    var strData = Read0X02Str;

                    if (!string.IsNullOrEmpty(strData))
                    {
                        var sp = strData.Split(' ');
                        recvData.AddRange(sp.Select(t => Convert.ToByte(t, 16)));
                        Vcc1_5V电压 = (GetIntelValue(recvData.ToArray(), 0, 8) / 10f).ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        Vcc1_5V电压 = "未接受到数据";
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
    }
}
