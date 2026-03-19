using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Controller
{
    [Description("CAN-Product,斯坦雷33WA")]
    public sealed class Stanleygz33Wa : ControllerBase
    {
        public CanBus Can;

        public Stanleygz33Wa(string name) : base(name)
        {
            MainWork();
        }

        ~Stanleygz33Wa()
        {
            Dispose();
        }

        private void MainWork()
        {
            // 25ms
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = SendTask(() =>
                {
                    var lstPages = new List<CanBus.CanDataPackage>
                    {
                        new CanBus.CanDataPackage(
                            _caplWakeup.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Extended,
                            CanBus.CanFormat.Data, _caplWakeup.MatrixData)
                    };
                    return lstPages.ToArray();
                }),
                Interval = 25
            });

            // 300ms
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = SendTask(() =>
                {
                    var lstPages = new List<CanBus.CanDataPackage>
                    {
                        new CanBus.CanDataPackage(
                            _caplRm.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Extended,
                            CanBus.CanFormat.Data, _caplRm.MatrixData),
                        new CanBus.CanDataPackage(
                            _caplMicu.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Extended,
                            CanBus.CanFormat.Data, _caplMicu.MatrixData),
                        new CanBus.CanDataPackage(
                            _caplHlsw.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Extended,
                            CanBus.CanFormat.Data, _caplHlsw.MatrixData),
                        new CanBus.CanDataPackage(
                            _caplAt.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Extended,
                            CanBus.CanFormat.Data, _caplAt.MatrixData),
                        new CanBus.CanDataPackage(
                            _caplYopecu.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Extended,
                            CanBus.CanFormat.Data, _caplYopecu.MatrixData),
                        new CanBus.CanDataPackage(
                            _caplHss2.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Extended,
                            CanBus.CanFormat.Data, _caplHss2.MatrixData)
                    };
                    return lstPages.ToArray();
                }),
                Interval = 300
            });

            // 100ms
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = SendTask(() =>
                {
                    var lstPages = new List<CanBus.CanDataPackage>
                    {
                        new CanBus.CanDataPackage(
                            _caplVspNe.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Extended,
                            CanBus.CanFormat.Data, _caplVspNe.MatrixData),
                        new CanBus.CanDataPackage(
                            _caplSteeringAngle.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Extended,
                            CanBus.CanFormat.Data, _caplSteeringAngle.MatrixData)
                    };
                    return lstPages.ToArray();
                }),
                Interval = 100
            });

            // 1000ms
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = SendTask(() =>
                {
                    var lstPages = new List<CanBus.CanDataPackage>
                    {
                        new CanBus.CanDataPackage(
                            _caplActive.CanId, CanBus.CanProtocol.Can, CanBus.CanType.Extended,
                            CanBus.CanFormat.Data, _caplActive.MatrixData)
                    };
                    return lstPages.ToArray();
                }),
                Interval = 1000
            });

            SchedulerAsync();
        }

        private Action SendTask(Func<CanBus.CanDataPackage[]> msgFunc)
        {
            return () =>
            {
                if (Can == null || msgFunc == null || _isSleep)
                    return;

                lock (_canSendLock)
                    Can.SendCanDatas(msgFunc.Invoke());
            };
        }

        private bool _isSleep = true;
        private readonly object _canSendLock = new object();

        private CanCommunicationMatrix.MotorolaMatrix _caplWakeup = new CanCommunicationMatrix.MotorolaMatrix(0x1E12FF00, 0);
        private CanCommunicationMatrix.MotorolaMatrix _caplRm = new CanCommunicationMatrix.MotorolaMatrix(0xAF81118, 6);
        private CanCommunicationMatrix.MotorolaMatrix _caplMicu = new CanCommunicationMatrix.MotorolaMatrix(0x12F81000, 8);
        private CanCommunicationMatrix.MotorolaMatrix _caplHlsw = new CanCommunicationMatrix.MotorolaMatrix(0xAF87018, 3);
        private CanCommunicationMatrix.MotorolaMatrix _caplAt = new CanCommunicationMatrix.MotorolaMatrix(0x12F85100, 8);
        private CanCommunicationMatrix.MotorolaMatrix _caplYopecu = new CanCommunicationMatrix.MotorolaMatrix(0xEF89400, 3);
        private CanCommunicationMatrix.MotorolaMatrix _caplHss2 = new CanCommunicationMatrix.MotorolaMatrix(0xAF87D00, 2);
        private CanCommunicationMatrix.MotorolaMatrix _caplVspNe = new CanCommunicationMatrix.MotorolaMatrix(0x12F85000, 8);
        private CanCommunicationMatrix.MotorolaMatrix _caplSteeringAngle = new CanCommunicationMatrix.MotorolaMatrix(0x12F97100, 3);
        private CanCommunicationMatrix.MotorolaMatrix _caplActive = new CanCommunicationMatrix.MotorolaMatrix(0x1610FF00, 0);

        [Description("打开CAN")]
        public void StartCan()
        {
            _isSleep = false;
        }

        [Description("关闭CAN")]
        public void StopCan()
        {
            _isSleep = true;
        }

        [Description("近光开")]
        public void LowBeamOn()
        {
            //_caplRm.MatrixData[0] = (byte)(_caplRm.MatrixData[0] | 0x02);
            _caplRm.UpdateData(new MatrixValDefinition(1, 1, 1));
        }

        [Description("近光关")]
        public void LowBeamOff()
        {
            //_caplRm.MatrixData[0] = (byte)(_caplRm.MatrixData[0] & 0xFD);
            _caplRm.UpdateData(new MatrixValDefinition(1, 1, 0));
        }

        [Description("远光开")]
        public void HighBeamOn()
        {
            //_caplRm.MatrixData[0] = (byte)(_caplRm.MatrixData[0] | 0x01);
            _caplRm.UpdateData(new MatrixValDefinition(0, 1, 1));
        }

        [Description("远光关")]
        public void HighBeamOff()
        {
            //_caplRm.MatrixData[0] = (byte)(_caplRm.MatrixData[0] & 0xFE);
            _caplRm.UpdateData(new MatrixValDefinition(0, 1, 0));
        }

        [Description("ACL开")]
        public void AclOn()
        {
            //_caplRm.MatrixData[0] = (byte)(_caplRm.MatrixData[0] | 0x02);
            //_caplMicu.MatrixData[3] = (byte)(_caplMicu.MatrixData[3] | 0x80);
            _caplRm.UpdateData(new MatrixValDefinition(3, 1, 1));
            _caplMicu.UpdateData(new MatrixValDefinition(31, 1, 1));
        }

        [Description("ACL关")]
        public void AclOff()
        {
            //_caplRm.MatrixData[0] = (byte)(_caplRm.MatrixData[0] & 0xFD);
            //_caplMicu.MatrixData[3] = (byte)(_caplMicu.MatrixData[3] & 0x7F);
            _caplRm.UpdateData(new MatrixValDefinition(3, 1, 0));
            _caplMicu.UpdateData(new MatrixValDefinition(31, 1, 0));
        }

        [Description("POS开")]
        public void PosOn()
        {
            //_caplRm.MatrixData[0] = (byte)(_caplRm.MatrixData[0] | 0x40);
            _caplRm.UpdateData(new MatrixValDefinition(6, 1, 1));
        }

        [Description("POS关")]
        public void PosOff()
        {
            //_caplRm.MatrixData[0] = (byte)(_caplRm.MatrixData[0] & 0xBF);
            _caplRm.UpdateData(new MatrixValDefinition(6, 1, 0));
        }

        [Description("DRL开")]
        public void DrlOn()
        {
            //_caplRm.MatrixData[0] = (byte)(_caplRm.MatrixData[0] | 0X08);
            _caplRm.UpdateData(new MatrixValDefinition(3, 1, 1));
        }

        [Description("DRL关")]
        public void DrlOff()
        {
            //_caplRm.MatrixData[0] = (byte)(_caplRm.MatrixData[0] & 0xF7);
            _caplRm.UpdateData(new MatrixValDefinition(3, 1, 0));
        }
    }
}
