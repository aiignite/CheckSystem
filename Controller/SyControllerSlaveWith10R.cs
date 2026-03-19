using Controller.HardwareController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading;

namespace Controller
{
    public sealed class SyControllerSlaveWith10R :
        ControllerBase,
        ControllerMasterSocketByUdp.IControllerMasterSocketSubscriber, ICcdIoController
    {
        public bool SetOutputs()
        {
            SetSlaveRelays();
            return true;
        }

        public void GetInputs()
        {
            //throw new NotImplementedException();
        }

        public void ResetOutPuts()
        {
            AllRelaysOff();
        }

        [Description("R,CANID")]
        public uint CanId = 0x201;
        private ControllerMasterSocketByUdp Udp { get; set; }
        private readonly EventWaitHandle _readEventWaitHandle = new AutoResetEvent(false);
        private readonly EventWaitHandle _writeEventHandle = new AutoResetEvent(false);
        private readonly Thread _daemonThread;
        private string _connectMasterName = string.Empty;

        [Description("R/W,继电器1")]
        public bool Relay1;

        [Description("R/W,继电器2")]
        public bool Relay2;

        [Description("R/W,继电器3")]
        public bool Relay3;

        [Description("R/W,继电器4")]
        public bool Relay4;

        [Description("R/W,继电器5")]
        public bool Relay5;

        [Description("R/W,继电器6")]
        public bool Relay6;

        [Description("R/W,继电器7")]
        public bool Relay7;

        [Description("R/W,继电器8")]
        public bool Relay8;

        [Description("R/W,继电器9")]
        public bool Relay9;

        [Description("R/W,继电器10")]
        public bool Relay10;

        public SyControllerSlaveWith10R(string name)
            : base(name)
        {
            if (_daemonThread != null)
            {
                _daemonThread.Abort();
                _daemonThread.Join();
            }

            //_daemonThread = new Thread(DaemonWork)
            //{
            //    IsBackground = true
            //};
            //_daemonThread.Start();
        }

        ~SyControllerSlaveWith10R()
        {
            Dispose();
        }

        protected override void Dispose(bool isDisposeing)
        {
            if (!isDisposeing)
                return;
            if (_daemonThread == null)
                return;
            _daemonThread.Abort();
            _daemonThread.Join();
        }

        private string _lastRelsyState = string.Empty;

        private void DaemonWork()
        {
            while (_daemonThread.IsAlive)
            {
                if (!_daemonThread.IsAlive)
                    break;

                if (!ControllerMasterSocketByUdp.ControllerMasterList.ContainsKey(_connectMasterName))
                    continue;

                var udp = ControllerMasterSocketByUdp.ControllerMasterList[_connectMasterName];

                if (udp == null)
                    continue;

                Thread.Sleep(5);

                lock (_lastRelsyState)
                {
                    var currentRelayState = string.Empty;
                    for (var i = 0; i < 10; i++)
                    {
                        var relay =
                            (bool)GetType().GetField(string.Format("Relay{0}", i + 1)).GetValue(this);
                        currentRelayState += relay ? 1.ToString() : 0.ToString();
                    }

                    if (currentRelayState != _lastRelsyState)
                        SetSlaveRelays();

                    _lastRelsyState = currentRelayState;
                }
            }
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

            var slaveWith10RDataPackage =
                dataPackage as ControllerMasterSocketByUdp.SlaveDataPackage;
            if (slaveWith10RDataPackage == null)
                return;

            if (slaveWith10RDataPackage.CanId != CanId)
                return;

            if (slaveWith10RDataPackage.DataBytes == null)
                return;

            if (slaveWith10RDataPackage.DataBytes.Length == 3 &&
                slaveWith10RDataPackage.DataBytes[0] == 0x2E + 0x40 &&
                slaveWith10RDataPackage.DataBytes[1] == 0xFF &&
                slaveWith10RDataPackage.DataBytes[2] == 0xFF)
            {
                _writeEventHandle.Set();
            }
            else if (slaveWith10RDataPackage.DataBytes.Length == 2 + 3)
            {
                var diValue = new byte[2];
                Array.Copy(slaveWith10RDataPackage.DataBytes, 3, diValue, 0, diValue.Length);

                var listBool = new List<string>();

                foreach (var boolStr in diValue.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')))
                    listBool.AddRange(boolStr.Select(t => t.ToString()));

                var diList = new string[listBool.Count];
                Array.Copy(listBool.ToArray(), diList, diList.Length);
                Array.Reverse(diList);

                for (var i = 0; i < 10; i++)
                {
                    var i1 = i;
                    var di = GetType().GetField(string.Format("Relay{0}", i1 + 1));
                    di.SetValue(this, diList[i] == "1");
                }

                _readEventWaitHandle.Set();
            }
        }

        [Description("操作所有继电器")]
        public void SetSlaveRelays()
        {
            if (!ControllerMasterSocketByUdp.ControllerMasterList.ContainsKey(_connectMasterName))
                return;

            var udp = ControllerMasterSocketByUdp.ControllerMasterList[_connectMasterName];

            if (udp == null)
                return;

            var relayCharList = new List<string>();
            for (var i = 0; i < 16; i++)
                relayCharList.Add(0.ToString());

            for (var i = 0; i < 10; i++)
            {
                var relay =
                    (bool)GetType().GetField(string.Format("Relay{0}", i + 1)).GetValue(this);
                relayCharList[i] = relay ? 1.ToString() : 0.ToString();
            }

            var relayChars = new string[16];
            Array.Copy(relayCharList.ToArray(), relayChars, relayCharList.Count);
            Array.Reverse(relayChars);

            var str = relayChars.Aggregate(string.Empty, (current, s) => current + s);
            var b1 = Convert.ToUInt16(str, 2);
            var b2 = BitConverter.GetBytes(b1);
            Array.Reverse(b2);

            udp.SendCan1ToSlave(CanId, new byte[] { 0x2E, 0xff, 0xff, b2[0], b2[1] });
            Thread.Sleep(50);
        }

        [Description("读取继电器状态")]
        public bool GetSlaveRelays()
        {
            lock (_lastRelsyState)
            {
                if (!ControllerMasterSocketByUdp.ControllerMasterList.ContainsKey(_connectMasterName))
                    return false;

                var udp = ControllerMasterSocketByUdp.ControllerMasterList[_connectMasterName];

                if (udp == null)
                    return false;
                udp.SendCan1ToSlave(CanId, new byte[] { 0x22, 0xFF, 0xFF });

                if (!_readEventWaitHandle.WaitOne(150))
                    return false;

                var currentRelayState = string.Empty;
                for (var i = 0; i < 10; i++)
                {
                    var relay =
                        (bool)GetType().GetField(string.Format("Relay{0}", i + 1)).GetValue(this);
                    currentRelayState += relay ? 1.ToString() : 0.ToString();
                }

                _lastRelsyState = currentRelayState;
                return true;
            }
        }

        [Description("关闭所有继电器")]
        public void AllRelaysOff()
        {
            Relay1 = false;
            Relay2 = false;
            Relay3 = false;
            Relay4 = false;
            Relay5 = false;
            Relay6 = false;
            Relay7 = false;
            Relay8 = false;
            Relay9 = false;
            Relay10 = false;
            SetSlaveRelays();
        }
    }
}
