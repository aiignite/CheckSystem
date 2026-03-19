using CommonUtility;
using Controller.HardwareController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Threading;

namespace Controller
{
    public sealed class SyControllerSlaveWith14Ad :
        ControllerBase,
        ControllerMasterSocketByUdp.IControllerMasterSocketSubscriber, ICcdIoController
    {
        public bool SetOutputs()
        {
            //throw new NotImplementedException();
            return true;
        }

        public void GetInputs()
        {
            GetSlaveAds();
        }

        public void ResetOutPuts()
        {
            //throw new NotImplementedException();
        }

        [Description("R,CANID")]
        public uint CanId = 0x101;
        private ControllerMasterSocketByUdp Udp { get; set; }
        private readonly EventWaitHandle _readEventWaitHandle = new AutoResetEvent(false);
        private bool _isGettingSlaveAd;
        private string _connectMasterName = string.Empty;

        [Description("R,电流1")]
        public float AdCurrent1;

        [Description("R,电流2")]
        public float AdCurrent2;

        [Description("R,电流3")]
        public float AdCurrent3;

        [Description("R,电流4")]
        public float AdCurrent4;

        [Description("R,电流5")]
        public float AdCurrent5;

        [Description("R,电流6")]
        public float AdCurrent6;

        [Description("R,电压1")]
        public float AdVoltage1;

        [Description("R,电压2")]
        public float AdVoltage2;

        [Description("R,电压3")]
        public float AdVoltage3;

        [Description("R,电压4")]
        public float AdVoltage4;

        [Description("R,电压5")]
        public float AdVoltage5;

        [Description("R,电压6")]
        public float AdVoltage6;

        [Description("R,电压7")]
        public float AdVoltage7;

        [Description("R,电压8")]
        public float AdVoltage8;

        public SyControllerSlaveWith14Ad(string name)
            : base(name) { }

        ~SyControllerSlaveWith14Ad()
        {
            Dispose();
        }

        public void ConnectToMaster(string masterName)
        {
            _connectMasterName = masterName;
        }

        public void ChangeCanId(string canId)
        {
            CanId = Convert.ToUInt32(canId, 16);

            if (!ControllerMasterSocketByUdp.ControllerMasterList.ContainsKey(_connectMasterName))
                return;

            Udp = ControllerMasterSocketByUdp.ControllerMasterList[_connectMasterName];
            if (Udp == null)
                return;

            Udp.AddOvserver(ReceiveMsg);
        }

        public void ReceiveMsg(
           EndPoint ipEndPointFrom, object dataPackage)
        {
            if (dataPackage.GetType() !=
                typeof(ControllerMasterSocketByUdp.SlaveDataPackage))
                return;

            var slaveWith14AdDataPackage =
                dataPackage as ControllerMasterSocketByUdp.SlaveDataPackage;
            if (slaveWith14AdDataPackage == null)
                return;

            if (slaveWith14AdDataPackage.CanId != CanId)
                return;

            if (slaveWith14AdDataPackage.DataBytes == null ||
                slaveWith14AdDataPackage.DataBytes.Length != 14 * 4 + 3)
                return;

            if (!_isGettingSlaveAd)
                return;

            var adValue = new byte[14 * 4];
            Array.Copy(slaveWith14AdDataPackage.DataBytes, 3, adValue, 0, adValue.Length);

            var listFloat = new List<float>();
            for (var i = 0; i < adValue.Length; i = i + 4)
            {
                var floatBytes = new[] { adValue[i], adValue[i + 1], adValue[i + 2], adValue[i + 3] };
                Array.Reverse(floatBytes);
                listFloat.Add((float)Math.Round(BitConverter.ToSingle(floatBytes, 0), 4, MidpointRounding.AwayFromZero));
            }

            AdCurrent1 = listFloat[0];
            AdCurrent2 = listFloat[1];
            AdCurrent3 = listFloat[2];
            AdCurrent4 = listFloat[3];
            AdCurrent5 = listFloat[4];
            AdCurrent6 = listFloat[5];

            AdVoltage1 = listFloat[6];
            AdVoltage2 = listFloat[7];
            AdVoltage3 = listFloat[8];
            AdVoltage4 = listFloat[9];
            AdVoltage5 = listFloat[10];
            AdVoltage6 = listFloat[11];
            AdVoltage7 = listFloat[12];
            AdVoltage8 = listFloat[13];

            _readEventWaitHandle.Set();
        }

        [Description("获取所有通道的电流电压值")]
        public bool GetSlaveAds()
        {
            if (!ControllerMasterSocketByUdp.ControllerMasterList.ContainsKey(_connectMasterName))
            {
                AdCurrent1 = -9999;
                AdCurrent2 = -9999;
                AdCurrent3 = -9999;
                AdCurrent4 = -9999;
                AdCurrent5 = -9999;
                AdCurrent6 = -9999;

                AdVoltage1 = -9999;
                AdVoltage2 = -9999;
                AdVoltage3 = -9999;
                AdVoltage4 = -9999;
                AdVoltage5 = -9999;
                AdVoltage6 = -9999;
                AdVoltage7 = -9999;
                AdVoltage8 = -9999;
                return false;
            }

            var udp = ControllerMasterSocketByUdp.ControllerMasterList[_connectMasterName];

            if (udp == null)
            {
                AdCurrent1 = -9999;
                AdCurrent2 = -9999;
                AdCurrent3 = -9999;
                AdCurrent4 = -9999;
                AdCurrent5 = -9999;
                AdCurrent6 = -9999;

                AdVoltage1 = -9999;
                AdVoltage2 = -9999;
                AdVoltage3 = -9999;
                AdVoltage4 = -9999;
                AdVoltage5 = -9999;
                AdVoltage6 = -9999;
                AdVoltage7 = -9999;
                AdVoltage8 = -9999;
                return false;
            }

            var maxCount = 5;

            var getAdc1Sum = new List<float>();
            var getAdc2Sum = new List<float>();
            var getAdc3Sum = new List<float>();
            var getAdc4Sum = new List<float>();
            var getAdc5Sum = new List<float>();
            var getAdc6Sum = new List<float>();

            var getAdv1Sum = new List<float>();
            var getAdv2Sum = new List<float>();
            var getAdv3Sum = new List<float>();
            var getAdv4Sum = new List<float>();
            var getAdv5Sum = new List<float>();
            var getAdv6Sum = new List<float>();
            var getAdv7Sum = new List<float>();
            var getAdv8Sum = new List<float>();

            var tempFailedCount = 0;

            for (var i = 0; i < maxCount; i++)
            {
                _isGettingSlaveAd = true;
                udp.SendCan1ToSlave(CanId, new byte[] { 0x22, 0x3F, 0xFF });

                if (_readEventWaitHandle.WaitOne(2500))
                {
                    getAdc1Sum.Add(AdCurrent1);
                    getAdc2Sum.Add(AdCurrent2);
                    getAdc3Sum.Add(AdCurrent3);
                    getAdc4Sum.Add(AdCurrent4);
                    getAdc5Sum.Add(AdCurrent5);
                    getAdc6Sum.Add(AdCurrent6);

                    getAdv1Sum.Add(AdVoltage1);
                    getAdv2Sum.Add(AdVoltage2);
                    getAdv3Sum.Add(AdVoltage3);
                    getAdv4Sum.Add(AdVoltage4);
                    getAdv5Sum.Add(AdVoltage5);
                    getAdv6Sum.Add(AdVoltage6);
                    getAdv7Sum.Add(AdVoltage7);
                    getAdv8Sum.Add(AdVoltage8);
                }
                else
                    tempFailedCount++;

                _isGettingSlaveAd = false;
            }

            if (tempFailedCount == maxCount)
            {
                AdCurrent1 = -9999;
                AdCurrent2 = -9999;
                AdCurrent3 = -9999;
                AdCurrent4 = -9999;
                AdCurrent5 = -9999;
                AdCurrent6 = -9999;

                AdVoltage1 = -9999;
                AdVoltage2 = -9999;
                AdVoltage3 = -9999;
                AdVoltage4 = -9999;
                AdVoltage5 = -9999;
                AdVoltage6 = -9999;
                AdVoltage7 = -9999;
                AdVoltage8 = -9999;
            }
            else
            {
                //var tempAdc1 = getAdc1Sum.ToArray();
                //Array.Sort(tempAdc1);
                //Array.Reverse(tempAdc1);
                //AdCurrent1 = tempAdc1[0];

                //var tempAdc2 = getAdc2Sum.ToArray();
                //Array.Sort(tempAdc2);
                //Array.Reverse(tempAdc2);
                //AdCurrent2 = tempAdc2[0];

                //var tempAdc3 = getAdc3Sum.ToArray();
                //Array.Sort(tempAdc3);
                //Array.Reverse(tempAdc3);
                //AdCurrent3 = tempAdc3[0];

                //var tempAdc4 = getAdc4Sum.ToArray();
                //Array.Sort(tempAdc4);
                //Array.Reverse(tempAdc4);
                //AdCurrent4 = tempAdc4[0];

                //var tempAdc5 = getAdc5Sum.ToArray();
                //Array.Sort(tempAdc5);
                //Array.Reverse(tempAdc5);
                //AdCurrent5 = tempAdc5[0];

                //var tempAdc6 = getAdc6Sum.ToArray();
                //Array.Sort(tempAdc6);
                //Array.Reverse(tempAdc6);
                //AdCurrent6 = tempAdc6[0];

                //var tempAdv1 = getAdv1Sum.ToArray();
                //Array.Sort(tempAdv1);
                //Array.Reverse(tempAdv1);
                //AdVoltage1 = tempAdv1[0];

                //var tempAdv2 = getAdv2Sum.ToArray();
                //Array.Sort(tempAdv2);
                //Array.Reverse(tempAdv2);
                //AdVoltage2 = tempAdv2[0];

                //var tempAdv3 = getAdv3Sum.ToArray();
                //Array.Sort(tempAdv3);
                //Array.Reverse(tempAdv3);
                //AdVoltage3 = tempAdv3[0];

                //var tempAdv4 = getAdv4Sum.ToArray();
                //Array.Sort(tempAdv4);
                //Array.Reverse(tempAdv4);
                //AdVoltage4 = tempAdv4[0];

                //var tempAdv5 = getAdv5Sum.ToArray();
                //Array.Sort(tempAdv5);
                //Array.Reverse(tempAdv5);
                //AdVoltage5 = tempAdv5[0];

                //var tempAdv6 = getAdv6Sum.ToArray();
                //Array.Sort(tempAdv6);
                //Array.Reverse(tempAdv6);
                //AdVoltage6 = tempAdv6[0];

                //var tempAdv7 = getAdv7Sum.ToArray();
                //Array.Sort(tempAdv7);
                //Array.Reverse(tempAdv7);
                //AdVoltage7 = tempAdv7[0];

                //var tempAdv8 = getAdv8Sum.ToArray();
                //Array.Sort(tempAdv8);
                //Array.Reverse(tempAdv8);
                //AdVoltage8 = tempAdv8[0];

                AdCurrent1 = WaveFilter.MidianAverageFileter(getAdc1Sum.ToArray());
                AdCurrent2 = WaveFilter.MidianAverageFileter(getAdc2Sum.ToArray());
                AdCurrent3 = WaveFilter.MidianAverageFileter(getAdc3Sum.ToArray());
                AdCurrent4 = WaveFilter.MidianAverageFileter(getAdc4Sum.ToArray());
                AdCurrent5 = WaveFilter.MidianAverageFileter(getAdc5Sum.ToArray());
                AdCurrent6 = WaveFilter.MidianAverageFileter(getAdc6Sum.ToArray());

                AdVoltage1 = WaveFilter.MidianAverageFileter(getAdv1Sum.ToArray());
                AdVoltage2 = WaveFilter.MidianAverageFileter(getAdv2Sum.ToArray());
                AdVoltage3 = WaveFilter.MidianAverageFileter(getAdv3Sum.ToArray());
                AdVoltage4 = WaveFilter.MidianAverageFileter(getAdv4Sum.ToArray());
                AdVoltage5 = WaveFilter.MidianAverageFileter(getAdv5Sum.ToArray());
                AdVoltage6 = WaveFilter.MidianAverageFileter(getAdv6Sum.ToArray());
                AdVoltage7 = WaveFilter.MidianAverageFileter(getAdv7Sum.ToArray());
                AdVoltage8 = WaveFilter.MidianAverageFileter(getAdv8Sum.ToArray());
            }

            return false;
        }
    }
}
