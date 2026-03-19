using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CommonUtility.BusLoader
{
    public class VectorDbcEmulator : IDisposable
    {
        private bool _isDisposed;
        private readonly DbcHelper _dbcHelper = new DbcHelper();
        private readonly object _outputLocker = new object();

        public readonly Dictionary<string, Dictionary<string, CanCommunicationMatrix>> Messages
            = new Dictionary<string, Dictionary<string, CanCommunicationMatrix>>();
        private readonly Dictionary<string, Dictionary<string, List<DbcHelper.Signal>>> _messageSignals =
            new Dictionary<string, Dictionary<string, List<DbcHelper.Signal>>>();

        public VectorDbcEmulator(IEnumerable<string> dbcFilePath)
        {
            foreach (var t in dbcFilePath)
                _dbcHelper.Parse(t);

            //if (_worker != null)
            //{
            //    _worker.Abort();
            //    _worker.Join();
            //}

            //_worker = new Thread(Work) { IsBackground = true };
            //_worker.Start();
        }

        ~VectorDbcEmulator()
        {
            Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposeing)
        {
            if (!isDisposeing)
                return;

            _isDisposed = true;
        }

        public void InitMessageVariable(
            string messageName, string messageVariableName, string dbName = "")
        {
            if (_dbcHelper.MyDbcFile == null)
                return;

            if (!_dbcHelper.MyDbcFile.Keys.ToArray().Any())
                return;

            if (string.IsNullOrEmpty(dbName))
                dbName = _dbcHelper.MyDbcFile.Keys.ToArray()[0];

            if (!_dbcHelper.MyDbcFile.ContainsKey(dbName))
                return;

            //if (Messages.ContainsKey(dbName))
            //    return;

            //if (_messageSignals.ContainsKey(dbName))
            //    return;

            var findMsg =
                _dbcHelper.MyDbcFile[dbName].Messages.Find(f => f.MessageName == messageName);

            if (findMsg == null)
                return;

            if (!Messages.ContainsKey(dbName))
                Messages.Add(dbName, new Dictionary<string, CanCommunicationMatrix>());

            if (findMsg.Signals.Exists(f => f.ByteOrder == 0))
                Messages[dbName].Add(messageVariableName,
                    new CanCommunicationMatrix.MotorolaMatrix(findMsg.MessgeId, (int)findMsg.MessageSize));
            else
                Messages[dbName].Add(messageVariableName,
                    new CanCommunicationMatrix.IntelMatrix(findMsg.MessgeId, (int)findMsg.MessageSize));

            if (!_messageSignals.ContainsKey(dbName))
                _messageSignals.Add(dbName, new Dictionary<string, List<DbcHelper.Signal>>());
            _messageSignals[dbName].Add(messageVariableName, findMsg.Signals);
        }

        public void SetMessageVariableSignalValue(
            string messageVariableName, string signalName, byte signalValue, string dbName = "")
        {
            if (!_dbcHelper.MyDbcFile.Keys.ToArray().Any())
                return;

            if (string.IsNullOrEmpty(dbName))
                dbName = _dbcHelper.MyDbcFile.Keys.ToArray()[0];

            if (!Messages.ContainsKey(dbName) ||
                !_messageSignals.ContainsKey(dbName))
                return;

            if (!Messages[dbName].ContainsKey(messageVariableName) ||
                !_messageSignals[dbName].ContainsKey(messageVariableName))
                return;

            var findSignal =
                _messageSignals[dbName][messageVariableName].Find(f => f.SignalName.ToLower() == signalName.ToLower());

            if (findSignal == null)
                return;

            var mvd =
                new MatrixValDefinition(
                    (int)findSignal.StartBit, (int)findSignal.SignalSize, signalValue);

            var msg = Messages[dbName][messageVariableName];
            msg.UpdateData(mvd);
        }

        //public void SetMessageVariableSignalValue(
        //    string messageVariableName, string signalName, int signalValue, string dbName = "")
        //{
        //    if (!_dbcHelper.MyDbcFile.Keys.ToArray().Any())
        //        return;

        //    if (string.IsNullOrEmpty(dbName))
        //        dbName = _dbcHelper.MyDbcFile.Keys.ToArray()[0];

        //    if (!Messages.ContainsKey(dbName) ||
        //        !_messageSignals.ContainsKey(dbName))
        //        return;

        //    if (!Messages[dbName].ContainsKey(messageVariableName) ||
        //        !_messageSignals[dbName].ContainsKey(messageVariableName))
        //        return;

        //    var findSignal =
        //        _messageSignals[dbName][messageVariableName].Find(f => f.SignalName == signalName);

        //    if (findSignal == null)
        //        return;

        //    var mvd =
        //        new MatrixValDefinition(
        //            (int)findSignal.StartBit, (int)findSignal.SignalSize, signalValue);

        //    var msg = Messages[dbName][messageVariableName];
        //    msg.UpdateData(mvd);
        //}

        public void SetMessageVariableByteValue(
            string messageVariableName, int byteIndex, byte byteValue, string dbName = "")
        {
            if (!_dbcHelper.MyDbcFile.Keys.ToArray().Any())
                return;

            if (string.IsNullOrEmpty(dbName))
                dbName = _dbcHelper.MyDbcFile.Keys.ToArray()[0];

            if (!Messages.ContainsKey(dbName))
                return;

            if (!Messages[dbName].ContainsKey(messageVariableName))
                return;

            if (Messages[dbName][messageVariableName].MatrixData.Length > byteIndex)
                Messages[dbName][messageVariableName].MatrixData[byteIndex] = byteValue;
        }

        public byte GetMessageVariableSignalValue(
            string messageVariableName, string signalName, string dbName = "")
        {
            if (!_dbcHelper.MyDbcFile.Keys.ToArray().Any())
                return 0x00;

            if (string.IsNullOrEmpty(dbName))
                dbName = _dbcHelper.MyDbcFile.Keys.ToArray()[0];

            if (!Messages.ContainsKey(dbName) ||
                 !_messageSignals.ContainsKey(dbName))
                return 0x00;

            if (!Messages[dbName].ContainsKey(messageVariableName) ||
                !_messageSignals[dbName].ContainsKey(messageVariableName))
                return 0x00;

            var msg = Messages[dbName][messageVariableName];
            var findSignal =
                _messageSignals[dbName][messageVariableName].Find(f => f.SignalName == signalName);

            if (findSignal == null)
                return 0x00;

            return
                (byte)msg.GetMatrixData((int)findSignal.StartBit, (int)findSignal.SignalSize);
        }

        public byte GetMessageVariableSignalValue(
            string messageVariableName, int byteIndex, string dbName = "")
        {
            if (!_dbcHelper.MyDbcFile.Keys.ToArray().Any())
                return 0x00;

            if (string.IsNullOrEmpty(dbName))
                dbName = _dbcHelper.MyDbcFile.Keys.ToArray()[0];

            if (!Messages.ContainsKey(dbName) ||
                 !_messageSignals.ContainsKey(dbName))
                return 0x00;

            if (!Messages[dbName].ContainsKey(messageVariableName) ||
                !_messageSignals[dbName].ContainsKey(messageVariableName))
                return 0x00;

            var msg = Messages[dbName][messageVariableName];

            return msg.MatrixData[byteIndex];
        }

        public byte GetMessageVariableSignalValue(uint canId, string signalName, string dbName = "")
        {
            if (!_dbcHelper.MyDbcFile.Keys.ToArray().Any())
                return 0x00;

            if (string.IsNullOrEmpty(dbName))
                dbName = _dbcHelper.MyDbcFile.Keys.ToArray()[0];

            if (!Messages.ContainsKey(dbName) ||
                 !_messageSignals.ContainsKey(dbName))
                return 0x00;

            return (from t in Messages[dbName].Keys
                    where Messages[dbName][t].CanId == canId
                    select GetMessageVariableSignalValue(t, signalName, dbName)).FirstOrDefault();
        }

        public void SetTimer(
            Action timerElapsed, int timerInterval)
        {
            var emulatorTimer = new EmulatorTimer(timerInterval, timerElapsed);
            timerElapsed.BeginInvoke(TimerEndInvoke, emulatorTimer);
        }

        private void TimerEndInvoke(IAsyncResult ir)
        {
            if (ir == null)
                return;

            var emulatorTimer = ir.AsyncState as EmulatorTimer;
            if (emulatorTimer == null)
                return;

            if (_isDisposed)
                return;

            var enterMs = HighPrecisionTimer.GetTimestamp();
            while (true)
            {
                var nowMs = HighPrecisionTimer.GetTimestamp();
                var ts = HighPrecisionTimer.GetTimestampIntervalMs(enterMs, nowMs);
                if (ts >= emulatorTimer.Interval)
                    break;
                Thread.Sleep(1);
            }

            //Thread.Sleep(emulatorTimer.Interval);
            //await Task.Delay(emulatorTimer.Interval);
            emulatorTimer.TimerElapsed.EndInvoke(ir);
            emulatorTimer.TimerElapsed.BeginInvoke(TimerEndInvoke, emulatorTimer);
        }

        public void OutPut(string messageVariableName, CanBus can, string dbName = "", CanBus.CanProtocol canProtocol = CanBus.CanProtocol.Can)
        {
            if (can == null)
                return;

            if (!_dbcHelper.MyDbcFile.Keys.ToArray().Any())
                return;

            if (string.IsNullOrEmpty(dbName))
                dbName = _dbcHelper.MyDbcFile.Keys.ToArray()[0];

            if (!Messages.ContainsKey(dbName))
                return;

            if (!Messages[dbName].ContainsKey(messageVariableName))
                return;

            lock (_outputLocker)
            {
                var msg = Messages[dbName][messageVariableName];

                //最高位为1的为扩展帧
                var isExternId = (msg.CanId & 0x80000000) != 0;

                if (canProtocol == CanBus.CanProtocol.CanFd)
                {
                    var dataPackage =
                        new CanBus.CanDataPackage(
                            msg.CanId,
                            CanBus.CanProtocol.CanFd,
                            isExternId ? CanBus.CanType.Extended : CanBus.CanType.Standard,
                            CanBus.CanFormat.Data,
                            msg.MatrixData);
                    can.SendCanDatas(new[] { dataPackage });
                }
                else if (canProtocol == CanBus.CanProtocol.Can)
                {
                    var dataPackage =
                        new CanBus.CanDataPackage(
                            msg.CanId,
                            CanBus.CanProtocol.Can,
                            isExternId ? CanBus.CanType.Extended : CanBus.CanType.Standard,
                            CanBus.CanFormat.Data,
                            msg.MatrixData);
                    can.SendCanDatas(new[] { dataPackage });
                }

                //if (dataPackage.CanId == 0x50)
                //{
                //    var str = dataPackage.CanData.Aggregate(string.Empty,
                //        (current, t) => current + ValueHelper.GetHextStrWithOx(t) + " ");
                //    Debug.WriteLine(str);
                //}
            }

            //var msg = Messages[dbName][messageVariableName];

            ////最高位为1的为扩展帧
            //var isExternId = (msg.CanId & 0x80000000) != 0;

            //var dataPackage =
            //    new CanBus.CanDataPackage(
            //        msg.CanId,
            //        CanBus.CanProtocol.Can,
            //        isExternId ? CanBus.CanType.Extended : CanBus.CanType.Standard,
            //        CanBus.CanFormat.Data,
            //        msg.MatrixData);

            //EqueueSendTask(can, dataPackage);
        }

        public void OutPut(string[] messageVariableNames, CanBus can, string dbName = "", CanBus.CanProtocol canProtocol = CanBus.CanProtocol.Can)
        {
            if (can == null)
                return;

            if (!_dbcHelper.MyDbcFile.Keys.ToArray().Any())
                return;

            if (string.IsNullOrEmpty(dbName))
                dbName = _dbcHelper.MyDbcFile.Keys.ToArray()[0];

            if (!Messages.ContainsKey(dbName))
                return;

            if (messageVariableNames == null)
                return;

            var listPackages = new List<CanBus.CanDataPackage>();

            foreach (var messageVariableName in messageVariableNames)
            {
                if (!Messages[dbName].ContainsKey(messageVariableName))
                    return;

                lock (_outputLocker)
                {
                    var msg = Messages[dbName][messageVariableName];

                    //最高位为1的为扩展帧
                    var isExternId = (msg.CanId & 0x80000000) != 0;

                    if (canProtocol == CanBus.CanProtocol.CanFd)
                    {
                        var dataPackage =
                            new CanBus.CanDataPackage(
                                msg.CanId,
                                CanBus.CanProtocol.CanFd,
                                isExternId ? CanBus.CanType.Extended : CanBus.CanType.Standard,
                                CanBus.CanFormat.Data,
                                msg.MatrixData);
                        listPackages.Add(dataPackage);
                        //can.SendCanDatas(new[] { dataPackage });
                    }
                    else if (canProtocol == CanBus.CanProtocol.Can)
                    {
                        var dataPackage =
                            new CanBus.CanDataPackage(
                                msg.CanId,
                                CanBus.CanProtocol.Can,
                                isExternId ? CanBus.CanType.Extended : CanBus.CanType.Standard,
                                CanBus.CanFormat.Data,
                                msg.MatrixData);
                        listPackages.Add(dataPackage);
                        //can.SendCanDatas(new[] { dataPackage });
                    }

                    //if (dataPackage.CanId == 0x50)
                    //{
                    //    var str = dataPackage.CanData.Aggregate(string.Empty,
                    //        (current, t) => current + ValueHelper.GetHextStrWithOx(t) + " ");
                    //    Debug.WriteLine(str);
                    //}
                }
            }

            can.SendCanDatas(listPackages.ToArray());
        }

        //private readonly Queue<MySendQueue> _sendTasks = new Queue<MySendQueue>();
        //private readonly object _locker = new object();
        //private readonly EventWaitHandle _wh = new AutoResetEvent(false);
        //private readonly Thread _worker;

        //private void Work()
        //{
        //    while (_worker.IsAlive)
        //    {
        //        if (!_worker.IsAlive)
        //            return;

        //        MySendQueue mySendQueueWork = null;
        //        lock (_locker)
        //        {
        //            if (_sendTasks.Count > 0)
        //            {
        //                mySendQueueWork = _sendTasks.Dequeue();

        //                if (mySendQueueWork == null)
        //                    return;
        //            }
        //        }

        //        if (mySendQueueWork != null)
        //        {
        //            if (mySendQueueWork.CanData.CanId == 0x3C0)
        //            {
        //                var str = mySendQueueWork.CanData.CanData.Aggregate(string.Empty,
        //                    (current, t) => current + ValueHelper.GetHextStrWithOx(t) + " ");
        //                Debug.WriteLine(str);
        //            }

        //            if (mySendQueueWork.Can != null)
        //            {
        //                mySendQueueWork.Can.SendCanDatas(new[] { mySendQueueWork.CanData });
        //                Thread.Sleep(5);
        //            }

        //        }
        //        //SendMsgTo(
        //        //        mySendQueueWork.EndPoint,
        //        //        mySendQueueWork.Msg);
        //        else
        //            _wh.WaitOne();
        //    }
        //}

        //public void EqueueSendTask(CanBus can, CanBus.CanDataPackage msg)
        //{
        //    lock (_locker)
        //        _sendTasks.Enqueue(
        //            new MySendQueue(
        //                can, msg));
        //    _wh.Set();
        //}

        internal class EmulatorTimer
        {
            public int Interval;
            public Action TimerElapsed;

            public EmulatorTimer(int interval, Action timerElapsed)
            {
                Interval = interval;
                TimerElapsed = timerElapsed;
            }
        }

        //internal class MySendQueue
        //{
        //    public CanBus Can { get; set; }
        //    public CanBus.CanDataPackage CanData { get; private set; }

        //    public MySendQueue(CanBus can, CanBus.CanDataPackage msg)
        //    {
        //        Can = can;
        //        CanData = msg;
        //    }
        //}
    }
}
