using HslCommunication.ModBus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace Controller
{
    public sealed class HighSensorCaliServo : ControllerBase
    {
        [Description("R,当前角度")]
        public ushort CurrentAngle;

        [Description("R,启动信号")]
        public ushort StartSign;

        [Description("R,急停信号")]
        public ushort ForceStop;

        [Description("R,当前角度")]
        public ushort ResumeSign;

        [Description("R,运动结束标志位")]
        public ushort RunFlag;

        [Description("R/W,矩阵索引")]
        public int MatrixIndex;

        [Description("R/W,产品连接气缸")]
        public ushort PushCyclinder;
        [Description("R/W,打章气缸")]
        public ushort StampCyclinder;
        [Description("R/W,报警")]
        public ushort Alert;

        [Description("R,写入的角度值")]
        public ushort AngleWrite;

        [Description("R/W,角度值判断范围")]
        public ushort AngleRange = 10;

        private ModbusTcpNet Plc { get; set; }
        private Thread Thread { get; set; }

        private readonly object _lockPlc = new object();
        private ushort _runExecuteCmd;
        private const int StepMin = 150;
        private const int StepMax = 2950;
        private bool _runExecute;
        private float _runTarget;
        private readonly List<float> _pointsList = new List<float>();

        public HighSensorCaliServo(string name)
            : base(name) { }

        ~HighSensorCaliServo()
        {
            if (Thread == null)
                return;
            Thread.Abort();
            Thread.Join();
        }

        public void ConnectPlc(string ipPort)
        {
            try
            {
                Plc = new ModbusTcpNet(ipPort.Split(':')[0], int.Parse(ipPort.Split(':')[1]));
                if (Thread != null)
                {
                    Thread.Abort();
                    Thread.Join();
                }

                Thread = new Thread(MainWork) { IsBackground = true };
                Thread.Start();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void MainWork()
        {
            Plc.ConnectServer();

            while (Thread.IsAlive)
            {
                if (!Thread.IsAlive)
                    break;

                Thread.Sleep(25);

                lock (_lockPlc)
                {
                    var readUshorts = Plc.ReadUInt16(1.ToString(), 6);

                    if (!readUshorts.IsSuccess)
                        continue;
                    var temp = readUshorts.Content;

                    CurrentAngle = temp[0];
                    StartSign = temp[1] == 1 ? (ushort)1 : (ushort)0;
                    ForceStop = temp[1] == 2 || temp[1] == 3 ? (ushort)1 : (ushort)0;
                    ResumeSign = temp[5] == 1 ? (ushort)1 : (ushort)0;

                    var ushorts = new[] { PushCyclinder, StampCyclinder, Alert };
                    Plc.Write(3.ToString(), ushorts);

                    if (!_runExecute)
                        continue;
                    if (Math.Abs(ConvertToStep(_runTarget) - CurrentAngle) > AngleRange)
                        continue;
                    _runExecute = false;

                    RunFlag = 1;
                }
            }
        }

        [Description("重置运动条件")]
        public void ResetMove()
        {
            _runExecute = false;
            RunFlag = 0;
        }

        [Description("运动到目标点位")]
        public void MoveToSignPoint(float point)
        {
            lock (_lockPlc)
            {
                if (_runExecute)
                    return;
                RunFlag = 0;
                AngleWrite = ConvertToStep(point);

                if (Math.Abs(ConvertToStep(point) - CurrentAngle) <= AngleRange)
                {
                    RunFlag = 1;
                }
                else
                {
                    _runExecute = true;
                    _runTarget = point;

                    _runExecuteCmd = 0;

                    while (true)
                    {
                        if (Plc.Write(9.ToString(), new[] { _runExecuteCmd }).IsSuccess)
                        {
                            break;
                        }
                    }

                    while (true)
                    {
                        if (Plc.WriteOneRegister(0.ToString(), ConvertToStep(point)).IsSuccess)
                        {
                            break;
                        }
                    }

                    _runExecuteCmd = 1;

                    while (true)
                    {
                        var runUshorts = new[] { _runExecuteCmd };
                        if (Plc.Write(9.ToString(), runUshorts).IsSuccess)
                        {
                            break;
                        }
                    }
                }
            }
        }

        public void AddPointsToMatrix(float point)
        {
            _pointsList.Add(point);
        }

        public void RunMatrix()
        {
            if (MatrixIndex >= 0 && MatrixIndex < _pointsList.Count)
                MoveToSignPoint(_pointsList[MatrixIndex]);
        }

        private static ushort ConvertToStep(float a)
        {
            //var s = (a + 50) * (StepMax - StepMin) / 100 + 150;
            var s = 10000f / 360 * a;
            return Convert.ToUInt16(s);
        }
    }
}
