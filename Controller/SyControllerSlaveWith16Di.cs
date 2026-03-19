using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading;

namespace Controller
{
    public sealed class SyControllerSlaveWith16Di :
        ControllerBase,
        ControllerMasterSocketByUdp.IControllerMasterSocketSubscriber
    {
        [Description("R,CANID")]
        public uint CanId = 0x301;
        private ControllerMasterSocketByUdp Udp { get; set; }
        private readonly EventWaitHandle _readEventWaitHandle = new AutoResetEvent(false);
        private string _connectMasterName = string.Empty;

        [Description("R,DI1")]
        public bool Di1;

        [Description("R,DI2")]
        public bool Di2;

        [Description("R,DI3")]
        public bool Di3;

        [Description("R,DI4")]
        public bool Di4;

        [Description("R,DI5")]
        public bool Di5;

        [Description("R,DI6")]
        public bool Di6;

        [Description("R,DI7")]
        public bool Di7;

        [Description("R,DI8")]
        public bool Di8;

        [Description("R,DI9")]
        public bool Di9;

        [Description("R,DI10")]
        public bool Di10;

        [Description("R,DI11")]
        public bool Di11;

        [Description("R,DI12")]
        public bool Di12;

        [Description("R,DI13")]
        public bool Di13;

        [Description("R,DI14")]
        public bool Di14;

        [Description("R,DI15")]
        public bool Di15;

        [Description("R,DI16")]
        public bool Di16;

        public SyControllerSlaveWith16Di(string name)
            : base(name) { }

        ~SyControllerSlaveWith16Di()
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

            var slaveWith16DiDataPackage =
                dataPackage as ControllerMasterSocketByUdp.SlaveDataPackage;
            if (slaveWith16DiDataPackage == null)
                return;

            if (slaveWith16DiDataPackage.CanId != CanId)
                return;

            if (slaveWith16DiDataPackage.DataBytes == null ||
                slaveWith16DiDataPackage.DataBytes.Length != 2 + 3)
                return;

            var diValue = new byte[2];
            Array.Copy(slaveWith16DiDataPackage.DataBytes, 3, diValue, 0, diValue.Length);

            var listBool = new List<string>();

            foreach (var boolStr in diValue.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')))
                listBool.AddRange(boolStr.Select(t => t.ToString()));

            var diList = new string[listBool.Count];
            Array.Copy(listBool.ToArray(), diList, diList.Length);
            Array.Reverse(diList);

            for (var i = 0; i < 16; i++)
            {
                var i1 = i;
                var di = GetType().GetField(string.Format("Di{0}", i1 + 1));
                di.SetValue(this, diList[i] != "1");
            }

            _readEventWaitHandle.Set();
        }

        public bool GetSlaveDis()
        {
            if (!ControllerMasterSocketByUdp.ControllerMasterList.ContainsKey(_connectMasterName))
                return false;

            var udp = ControllerMasterSocketByUdp.ControllerMasterList[_connectMasterName];
            if (udp == null)
                return false;
            udp.SendCan1ToSlave(CanId, new byte[] { 0x22, 0xFF, 0xFF });
            return _readEventWaitHandle.WaitOne(500);
        }
    }
}
