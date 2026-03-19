using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

namespace Controller
{
    public sealed class SyControllerSlaveWith20Do :
        ControllerBase,
        ControllerMasterSocketByUdp.IControllerMasterSocketSubscriber
    {
        public uint CanId = 0x401;
        private ControllerMasterSocketByUdp Udp { get; set; }
        private readonly EventWaitHandle _readEventWaitHandle = new AutoResetEvent(false);
        private readonly EventWaitHandle _writeEventHandle = new AutoResetEvent(false);
        private string _connectMasterName = string.Empty;

        public bool Do1;
        public bool Do2;
        public bool Do3;
        public bool Do4;
        public bool Do5;
        public bool Do6;
        public bool Do7;
        public bool Do8;
        public bool Do9;
        public bool Do10;
        public bool Do11;
        public bool Do12;
        public bool Do13;
        public bool Do14;
        public bool Do15;
        public bool Do16;
        public bool Do17;
        public bool Do18;
        public bool Do19;
        public bool Do20;

        public SyControllerSlaveWith20Do(string name)
            : base(name) { }

        ~SyControllerSlaveWith20Do()
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

            var slaveWith20DoDataPackage =
                dataPackage as ControllerMasterSocketByUdp.SlaveDataPackage;
            if (slaveWith20DoDataPackage == null)
                return;

            if (slaveWith20DoDataPackage.CanId != CanId)
                return;

            if (slaveWith20DoDataPackage.DataBytes == null)
                return;

            if (slaveWith20DoDataPackage.DataBytes.Length == 3 &&
                slaveWith20DoDataPackage.DataBytes[0] == 0x2E + 0x40 &&
                slaveWith20DoDataPackage.DataBytes[1] == 0xFF &&
                slaveWith20DoDataPackage.DataBytes[2] == 0xFF)
            {
                _writeEventHandle.Set();
            }
            else if (slaveWith20DoDataPackage.DataBytes.Length == 3 + 3 &&
                slaveWith20DoDataPackage.DataBytes[0] == 0x22 + 0x40)
            {
                var doValue = new byte[3];
                Array.Copy(slaveWith20DoDataPackage.DataBytes, 3, doValue, 0, doValue.Length);

                var listBool = new List<string>();

                foreach (var boolStr in doValue.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')))
                    listBool.AddRange(boolStr.Select(t => t.ToString()));

                var diList = new string[listBool.Count];
                Array.Copy(listBool.ToArray(), diList, diList.Length);
                Array.Reverse(diList);

                for (var i = 0; i < 20; i++)
                {
                    var i1 = i;
                    var dOut = GetType().GetField(string.Format("Do{0}", i1 + 1));
                    dOut.SetValue(this, diList[i] == "1");
                }

                _readEventWaitHandle.Set();
            }
        }

        public bool SetSlaveDos()
        {
            if (!ControllerMasterSocketByUdp.ControllerMasterList.ContainsKey(_connectMasterName))
                return false;

            var udp = ControllerMasterSocketByUdp.ControllerMasterList[_connectMasterName];
            if (udp == null)
                return false;

            var relayCharList = new List<string>();
            for (var i = 0; i < 32; i++)
                relayCharList.Add(0.ToString());

            for (var i = 0; i < 20; i++)
            {
                var relay =
                    (bool)GetType().GetField(string.Format("Do{0}", i + 1)).GetValue(this);
                relayCharList[i] = relay ? 1.ToString() : 0.ToString();
            }

            var doChars = new string[32];
            Array.Copy(relayCharList.ToArray(), doChars, relayCharList.Count);
            Array.Reverse(doChars);

            var str = doChars.Aggregate(string.Empty, (current, s) => current + s);
            var b1 = Convert.ToUInt32(str, 2);
            var b2 = BitConverter.GetBytes(b1);
            Array.Reverse(b2);

            udp.SendCan1ToSlave(CanId, new byte[] { 0x2E, 0xff, 0xff, b2[1], b2[2], b2[3] });

            return _writeEventHandle.WaitOne(500);
        }

        public bool GetSlaveDos()
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
