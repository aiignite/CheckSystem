using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CommonUtility.BusLoader
{
    public abstract class LinBus
    {
        public delegate void PushLinMsgEventHandle(string name, LinDataPackage data);
        public static event PushLinMsgEventHandle PushLinMsg;

        protected LinBus(string name)
        {
            Name = name;
        }

        protected void RegisterSendMasterLinFunc(
            Func<LinDataPackage, bool, bool> sendMasterLinFunc)
        {
            _sendMasterLinFunc = sendMasterLinFunc;
        }

        protected void RegisterSendSlaveLinACtion(
            Action<byte> sendSlaveLinAction)
        {
            _sendSlaveLinAction = sendSlaveLinAction;
        }

        public readonly string Name;

        public readonly List<LinDataPackage> LinRecvDataPackages = new List<LinDataPackage>();

        /// <summary>
        /// 发送主节点命令帧消息函数
        /// </summary>
        private Func<LinDataPackage, bool, bool> _sendMasterLinFunc;

        /// <summary>
        /// 发送从节点相应帧命令函数
        /// </summary>
        private Action<byte> _sendSlaveLinAction;

        private byte _currentSendSlaveLinId;
        private readonly EventWaitHandle _slaveLinEventWaitHandle = new AutoResetEvent(false);

        /// <summary>
        /// 发送主节点命令帧加数据
        /// </summary>
        /// <param name="masterLinId"></param>
        /// <param name="value"></param>
        /// <param name="isWaitSlaveLin"></param>
        /// <returns></returns>
        public bool SendMasterLin(
            byte masterLinId, IEnumerable<byte> value, bool isWaitSlaveLin = false)
        {
            if (_sendMasterLinFunc == null)
                return false;

            var masterLinDataPackage = new LinDataPackage(masterLinId, value.ToArray());
            return _sendMasterLinFunc(masterLinDataPackage, isWaitSlaveLin);
        }

        /// <summary>
        /// 发送从节点命令帧帧头并接收数据
        /// </summary>
        /// <param name="slaveLinId"></param>
        /// <param name="recvData"></param>
        /// <param name="timeOutMs"></param>
        /// <returns></returns>
        public bool SendSlaveLin(byte slaveLinId, out byte[] recvData, int timeOutMs = 500)
        {
            _slaveLinEventWaitHandle.Reset();
            recvData = new byte[0];
            _currentSendSlaveLinId = ConvertLinId(slaveLinId);

            if (_sendSlaveLinAction == null)
                return false;

            lock (LinRecvDataPackages)
                LinRecvDataPackages.Clear();

            _sendSlaveLinAction(slaveLinId);
            if (_slaveLinEventWaitHandle.WaitOne(timeOutMs))
            {
                lock (LinRecvDataPackages)
                {
                    if (LinRecvDataPackages.Any())
                    {
                        var recvPackage = LinRecvDataPackages[0];
                        recvData = new byte[recvPackage.LinDataLen];
                        Array.Copy(recvPackage.LinData, recvData, recvData.Length);

                        _currentSendSlaveLinId = 0xFF;
                        return true;
                    }

                    _currentSendSlaveLinId = 0xFF;
                    return false;
                }
            }

            _currentSendSlaveLinId = 0xFF;
            return false;
        }

        /// <summary>
        /// 发送主节点命令帧加数据
        /// 再发送从节点命令帧帧头并接收数据
        /// </summary>
        /// <param name="masterLinId"></param>
        /// <param name="slaveLinId"></param>
        /// <param name="sendData"></param>
        /// <param name="recvData"></param>
        /// <param name="delayMs"></param>
        /// <returns></returns>
        public bool SendMasterLinAndRecvSingleSlaveLin(
             byte masterLinId, byte slaveLinId, byte[] sendData, out byte[] recvData, int delayMs = 50)
        {
            recvData = new byte[0];
            var isSendMasterLinSuccess = SendMasterLin(masterLinId, sendData, true);
            if (!isSendMasterLinSuccess)
                return false;

            Thread.Sleep(delayMs);

            var isSendSlaveLinSuccess = SendSlaveLin(slaveLinId, out recvData);
            return isSendSlaveLinSuccess;
        }

        public void RecviveLinDatas(LinDataPackage[] linDataPackages)
        {
            if (linDataPackages == null)
                return;

            foreach (var t in linDataPackages)
            {
                OnPushLinMsg(Name, t);

                if ((_currentSendSlaveLinId != t.LinId && _currentSendSlaveLinId != ConvertLinId(t.LinId)) ||
                    _currentSendSlaveLinId == 0xFF)
                    continue;

                lock (LinRecvDataPackages)
                {
                    LinRecvDataPackages.Add(t);
                    _slaveLinEventWaitHandle.Set();
                    break;
                }
            }
        }

        /// <summary>
        /// 将LinId转换为PID
        /// </summary>
        /// <param name="linId"></param>
        /// <returns></returns>
        public static byte ConvertLinId(byte linId)
        {
            var temp = (linId & ~0x40 | 0x80) ^
                       (((linId << 2) ^
                         ((linId << 3) & 0x80) ^
                         (linId << 4) ^
                         ((linId << 5) & 0x40) ^
                         (linId << 6)) & 0xc0);

            return (byte)temp;
        }

        /// <summary>
        /// 将PID转换为LinId
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static byte ConvertPid(byte pid)
        {
            // 提取原始LIN ID（忽略最高的两个校验位）。
            return (byte)(pid & 0x3F);
        }

        private static async void OnPushLinMsg(
            string name, LinDataPackage data)
        {
            await Task.Run(() =>
            {
                if (PushLinMsg != null)
                    PushLinMsg.Invoke(name, data);
            });
        }

        /// <summary>
        /// LIN消息接收包
        /// </summary>
        public class LinDataPackage
        {
            /// <summary>
            /// 消息唯一标识
            /// </summary>
            public string MessageKey { get; private set; }

            /// <summary>
            /// 收到LIN消息的时间
            /// </summary>
            public DateTime DateTime { get; private set; }

            /// <summary>
            /// 收到的LIN ID
            /// </summary>
            public byte LinId { get; private set; }

            /// <summary>
            /// 收到的LIN消息长度
            /// </summary>
            public int LinDataLen { get; private set; }

            /// <summary>
            /// 收到的LIN消息内容
            /// </summary>
            public byte[] LinData { get; private set; }

            /// <summary>
            /// LIN消息包
            /// </summary>
            /// <param name="linId">LIN ID</param>
            /// <param name="data">LIN DATA</param>
            public LinDataPackage(byte linId, byte[] data)
            {
                MessageKey = Guid.NewGuid().ToString();
                DateTime = DateTime.Now;
                LinId = linId;
                if (data == null)
                {
                    LinData = new byte[0];
                    LinDataLen = 0;
                }
                else
                {
                    LinData = data;
                    LinDataLen = data.Length;
                }
            }
        }
    }
}
