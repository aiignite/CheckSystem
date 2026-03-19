using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using CommonUtility;
using CommonUtility.BusLoader;

namespace Controller
{
    [Description("LIN-Product,L246LinLoadBox")]
    public sealed class L246LinLoadBox : ControllerBase
    {
        public LinBus MasterLin1;
        public LinBus MasterLin2;
        public LinBus MasterLin3;
        public LinBus SlaveLin;

        private bool _isReadMasterLin1Msg;
        private bool _isReadMasterLin2Msg;
        private bool _isReadMasterLin3Msg;
        private readonly EventWaitHandle _waitMasetLin1Handle = new AutoResetEvent(false);
        private readonly EventWaitHandle _waitMasetLin2Handle = new AutoResetEvent(false);
        private readonly EventWaitHandle _waitMasetLin3Handle = new AutoResetEvent(false);

        [Description("R,Master LIN1读取消息内容")]
        public string MasterLin1Msg;

        [Description("R,Master LIN1读取消息结果")]
        public string ReadMasterLin1MsgResult;

        [Description("R,Master LIN2读取消息内容")]
        public string MasterLin2Msg;

        [Description("R,Master LIN2读取消息结果")]
        public string ReadMasterLin2MsgResult;

        [Description("R,Master LIN3读取消息内容")]
        public string MasterLin3Msg;

        [Description("R,Master LIN3读取消息结果")]
        public string ReadMasterLin3MsgResult;

        [Description("R,Slave LIN1读取消息结果")]
        public string SlaveLinMsg;

        [Description("R,Slave LIN1读取消息内容")]
        public string ReadSlaveLinMsgResult;

        public L246LinLoadBox(string name)
            : base(name)
        {
            LinBus.PushLinMsg += LinBus_PushLinMsg;
        }

        ~L246LinLoadBox()
        {
            LinBus.PushLinMsg -= LinBus_PushLinMsg;
            Dispose();
        }
        
        private void LinBus_PushLinMsg(string name, LinBus.LinDataPackage data)
        {
            if (_isReadMasterLin1Msg && name == MasterLin1.Name)
            {
                MasterLin1Msg = ValueHelper.GetHextStrWithOx(data.LinId) + " ";
                MasterLin1Msg +=
                    data.LinData.Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
                _waitMasetLin1Handle.Set();
            }

            if (_isReadMasterLin2Msg && name == MasterLin2.Name)
            {
                MasterLin2Msg = ValueHelper.GetHextStrWithOx(data.LinId) + " ";
                MasterLin2Msg +=
                    data.LinData.Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
                _waitMasetLin2Handle.Set();
            }

            if (_isReadMasterLin3Msg && name == MasterLin3.Name)
            {
                MasterLin3Msg = ValueHelper.GetHextStrWithOx(data.LinId) + " ";
                MasterLin3Msg +=
                    data.LinData.Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
                _waitMasetLin3Handle.Set();
            }
        }

        [Description("Read Master LIN1 Message")]
        public void ReadMasterLin1Message(string delayMs)
        {
            try
            {
                MasterLin1Msg = string.Empty;
                ReadMasterLin1MsgResult = "NG";

                if (MasterLin1 == null)
                    return;

                _isReadMasterLin1Msg = true;

                if (_waitMasetLin1Handle.WaitOne(int.Parse(delayMs)))
                    ReadMasterLin1MsgResult = "OK";

                _isReadMasterLin1Msg = false;
            }
            catch (Exception)
            {
                MasterLin1Msg = string.Empty;
                ReadMasterLin1MsgResult = "NG";
                _isReadMasterLin1Msg = false;
            }
        }

        [Description("Read Master LIN2 Message")]
        public void ReadMasterLin2Message(string delayMs)
        {
            try
            {
                MasterLin2Msg = string.Empty;
                ReadMasterLin2MsgResult = "NG";

                if (MasterLin2 == null)
                    return;

                _isReadMasterLin2Msg = true;

                if (_waitMasetLin2Handle.WaitOne(int.Parse(delayMs)))
                    ReadMasterLin2MsgResult = "OK";

                _isReadMasterLin2Msg = false;

            }
            catch (Exception)
            {
                MasterLin2Msg = string.Empty;
                ReadMasterLin2MsgResult = "NG";
                _isReadMasterLin2Msg = false;
            }
        }

        [Description("Read Master LIN3 Message")]
        public void ReadMasterLin3Message(string delayMs)
        {
            try
            {
                MasterLin3Msg = string.Empty;
                ReadMasterLin3MsgResult = "NG";

                if (MasterLin3 == null)
                    return;

                _isReadMasterLin3Msg = true;

                if (_waitMasetLin3Handle.WaitOne(int.Parse(delayMs)))
                    ReadMasterLin3MsgResult = "OK";

                _isReadMasterLin3Msg = false;
            }
            catch (Exception)
            {
                MasterLin3Msg = string.Empty;
                ReadMasterLin3MsgResult = "NG";
                _isReadMasterLin3Msg = false;
            }
        }

        [Description("Read Slave LIN Message")]
        public void ReadSlaveMessage(string delayMs)
        {
            try
            {
                SlaveLinMsg = string.Empty;
                ReadSlaveLinMsgResult = "NG";

                if (SlaveLin == null)
                    return;

                const byte masterLinId = (byte)0x30; // ??????
                const byte slaveLinId = (byte)0x31; // ??????
                var sendData = new byte[8]; // ??????

                byte[] echoBytes;
                if (!SlaveLin.SendMasterLinAndRecvSingleSlaveLin(masterLinId, slaveLinId, sendData, out echoBytes))
                    return;

                SlaveLinMsg = ValueHelper.GetHextStrWithOx(slaveLinId) + " ";
                SlaveLinMsg +=
                    echoBytes.Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
                ReadSlaveLinMsgResult = "OK";
            }
            catch (Exception)
            {
                SlaveLinMsg = string.Empty;
                ReadSlaveLinMsgResult = "NG";
            }
        }
    }
}
