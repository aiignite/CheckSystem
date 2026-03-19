using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Controller
{
    public sealed class AxisController : ControllerBase
    {
        public RtexControllerBase RtexController;
        public float AxisCurrentPos;
        public bool IsAxisServoOn;
        public bool IsHoming;
        public bool IsAxisReady;
        public bool IsAxisBusy;
        public bool IsMoveDone;
        public float SpeedPercent = 15;
        private float _defaultSpeed = 100;

        private Thread MainWorkTh { get; set; }
        private int AxisIndex { get; set; }
        private float AxisSpeed { get; set; }
        private bool IsMoveEventSet { get; set; }
        private float MoveEventTargetPos { get; set; }

        private readonly List<AddrDefinition> _addrDefinitionList = new List<AddrDefinition>();

        public AxisController(string name)
            : base(name) { }

        ~AxisController()
        {
            Dispose();
        }

        public void InitAxisIndex(string axisNo)
        {
            _addrDefinitionList.Clear();

            AxisIndex = int.Parse(axisNo);

            var baseAddr = 0x0210;
            var axisIndex = AxisIndex; // 当前轴号
            baseAddr = baseAddr + axisIndex * 16;

            _addrDefinitionList.Add(
                new AddrDefinition(string.Format("0x{0}", string.Format("{0:X}", baseAddr).PadLeft(4, '0')), "轴号",
                    "ushort"));
            baseAddr++;

            _addrDefinitionList.Add(
                new AddrDefinition(string.Format("0x{0}", string.Format("{0:X}", baseAddr).PadLeft(4, '0')), "轴运行状态",
                    "ushort"));
            baseAddr++;

            _addrDefinitionList.Add(
                new AddrDefinition(string.Format("0x{0}", string.Format("{0:X}", baseAddr).PadLeft(4, '0')), "错误码",
                    "ushort"));
            baseAddr++;

            _addrDefinitionList.Add(
                new AddrDefinition(string.Format("0x{0}", string.Format("{0:X}", baseAddr).PadLeft(4, '0')), "编码器类型",
                    "ushort"));
            baseAddr++;

            _addrDefinitionList.Add(
                new AddrDefinition(string.Format("0x{0}", string.Format("{0:X}", baseAddr).PadLeft(4, '0')), "当前位置",
                    "float"));
            baseAddr = baseAddr + 2;

            _addrDefinitionList.Add(
                new AddrDefinition(string.Format("0x{0}", string.Format("{0:X}", baseAddr).PadLeft(4, '0')), "电机单位",
                    "float"));
            baseAddr = baseAddr + 2;

            _addrDefinitionList.Add(
                new AddrDefinition(string.Format("0x{0}", string.Format("{0:X}", baseAddr).PadLeft(4, '0')), "轴单位",
                    "float"));
            baseAddr = baseAddr + 2;

            _addrDefinitionList.Add(
                new AddrDefinition(string.Format("0x{0}", string.Format("{0:X}", baseAddr).PadLeft(4, '0')), "原点偏移",
                    "float"));

            StartBackgroundThread();
        }

        private void StartBackgroundThread()
        {
            if (MainWorkTh != null)
            {
                MainWorkTh.Abort();
                MainWorkTh.Join();
            }

            MainWorkTh = new Thread(MainWork) { IsBackground = true };
            MainWorkTh.Start();
        }

        private void MainWork()
        {
            while (MainWorkTh.IsAlive)
            {
                if (!MainWorkTh.IsAlive)
                    break;

                if (RtexController == null)
                    continue;

                Thread.Sleep(50);

                try
                {
                    foreach (var item in _addrDefinitionList)
                    {
                        const uint baseAddr = (uint)0x0200;
                        var addr = Convert.ToUInt32(item.Addr, 16) - baseAddr;
                        var dataType = item.DataType.ToLower();

                        switch (dataType)
                        {
                            case "ushort":
                                item.SetValue((RtexController.DataBytes[addr * 2] * 256 + RtexController.DataBytes[addr * 2 + 1]).ToString());
                                break;

                            case "float":
                                var fb1 = RtexController.DataBytes[addr * 2];
                                var fb2 = RtexController.DataBytes[addr * 2 + 1];
                                var fb3 = RtexController.DataBytes[addr * 2 + 2];
                                var fb4 = RtexController.DataBytes[addr * 2 + 3];
                                item.SetValue(BitConverter.ToSingle(new[] { fb4, fb3, fb2, fb1 }, 0).ToString(CultureInfo.InvariantCulture));
                                break;
                        }
                    }

                    #region 赋值当前状态
                    AxisCurrentPos = float.Parse(_addrDefinitionList.Find(f => f.Name.Equals("当前位置")).Value);
                    var currentState = int.Parse(_addrDefinitionList.Find(f => f.Name.Equals("轴运行状态")).Value);

                    switch (currentState)
                    {
                        case 0:
                            IsAxisServoOn = false;
                            IsAxisReady = false;
                            IsAxisBusy = false;
                            IsHoming = false;
                            break;

                        case 1:
                            IsAxisServoOn = true;
                            IsAxisReady = false;
                            IsAxisBusy = true;
                            IsHoming = false;
                            break;

                        case 2:
                            IsAxisServoOn = true;
                            IsAxisReady = false;
                            IsAxisBusy = true;
                            IsHoming = true;
                            break;

                        case 3:
                            IsAxisServoOn = true;
                            IsAxisReady = true;
                            IsAxisBusy = false;
                            IsHoming = false;
                            break;

                        case 4:
                            IsAxisServoOn = true;
                            IsAxisReady = false;
                            IsAxisBusy = true;
                            IsHoming = false;
                            break;
                    }
                    #endregion

                    if (IsMoveEventSet)
                    {
                        var targetPos = MoveEventTargetPos;
                        var currentPos = AxisCurrentPos;

                        if (Math.Abs(targetPos - currentPos) < 0.05 && IsAxisReady && !IsAxisBusy && !IsHoming)
                        {
                            IsMoveEventSet = false;
                            IsMoveDone = true;
                        }
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        [Description("设置当前速度")]
        public void SetSpeed(string speed)
        {
            AxisSpeed = float.Parse(speed);
        }

        [Description("运动到目标位置")]
        public void AxisMoveTo(string pos)
        {
            IsMoveDone = false;
            MoveEventTargetPos = float.Parse(pos);

            var moveSpeed = Math.Abs(AxisSpeed) < 0.00001 ? _defaultSpeed : AxisSpeed;
            moveSpeed = moveSpeed * SpeedPercent / 100;
            AxisSpeed = 0;

            var targetPos = MoveEventTargetPos;
            var currentPos = AxisCurrentPos;

            if (Math.Abs(targetPos - currentPos) < 0.05 && IsAxisReady && !IsAxisBusy)
            {
                IsMoveDone = true;
            }
            else
            {
                RtexController.RunJog(AxisIndex, float.Parse(pos), moveSpeed);
                IsMoveEventSet = true;
            }
        }

        [Description("根据当前位置运动相对距离")]
        public void AxisMoveRelativeTo(string pos)
        {
            IsMoveDone = false;
            MoveEventTargetPos = AxisCurrentPos + float.Parse(pos);

            var moveSpeed = Math.Abs(AxisSpeed) < 0.00001 ? _defaultSpeed : AxisSpeed;
            moveSpeed = moveSpeed * SpeedPercent / 100;
            AxisSpeed = 0;

            var targetPos = MoveEventTargetPos;
            var currentPos = AxisCurrentPos;

            if (Math.Abs(targetPos - currentPos) < 0.05 && IsAxisReady && !IsAxisBusy)
            {
                IsMoveDone = true;
            }
            else
            {
                RtexController.RunJog(AxisIndex, targetPos, moveSpeed);
                IsMoveEventSet = true;
            }
        }

        [Description("使能打开")]
        public void ServoOn()
        {
            RtexController.RunEvent(
                AxisIndex, RtexControllerBase.EventType.ServoEnable);
        }

        [Description("使能关闭")]
        public void ServoOff()
        {
            RtexController.RunEvent(
                AxisIndex, RtexControllerBase.EventType.ServoDisalbe);
        }

        [Description("回到原点")]
        public void AxisHome()
        {
            IsMoveDone = false;
            MoveEventTargetPos = float.Parse(0.ToString());

            RtexController.RunEvent(
                AxisIndex, RtexControllerBase.EventType.Home);

            IsMoveEventSet = true;
        }

        #region 预设点位

        private readonly Dictionary<string, PrePostion> _prePostionsList = new Dictionary<string, PrePostion>();

        [Description("添加一个预存点位")]
        public void AddPrePos(string value) // Name:Speed=102.32/Pos=213.5
        {
            var sp = value.Split(':');
            if (sp.Length != 2)
                return;

            var name = sp[0];

            if (string.IsNullOrEmpty(name))
                return;

            var sp2 = sp[1].Split('/');
            if (sp2.Length != 2)
                return;

            var speed = string.Empty;
            var pos = string.Empty;

            foreach (var sp3 in sp2.Select(t => t.Split('=')))
            {
                if (sp3.Length != 2)
                    return;

                if (string.IsNullOrEmpty(sp3[0]) || string.IsNullOrEmpty(sp3[1]))
                    return;

                if (sp3[0].ToLower() == "speed")
                {
                    float tempSpeed;
                    if (!float.TryParse(sp3[1], out tempSpeed))
                        return;
                    speed = tempSpeed.ToString(CultureInfo.InvariantCulture);
                }
                else if (sp3[0].ToLower() == "pos")
                {
                    float tempPos;
                    if (!float.TryParse(sp3[1], out tempPos))
                        return;
                    pos = tempPos.ToString(CultureInfo.InvariantCulture);
                }
            }

            if (string.IsNullOrEmpty(speed) || string.IsNullOrEmpty(pos))
                return;

            if (_prePostionsList.ContainsKey(name))
                _prePostionsList[name] = new PrePostion(pos, speed);
            else
                _prePostionsList.Add(name, new PrePostion(pos, speed));
        }

        [Description("移除一个预存点位")]
        public void RemovePrePos(string name)
        {
            if (!string.IsNullOrEmpty(name) && _prePostionsList.ContainsKey(name))
                _prePostionsList.Remove(name);
        }

        [Description("清除所有预存点位")]
        public void ClearPrePos()
        {
            _prePostionsList.Clear();
        }

        [Description("运动到预存点位")]
        public void RunToPrePos(string name)
        {
            if (string.IsNullOrEmpty(name) || !_prePostionsList.ContainsKey(name))
                return;
            SetSpeed(_prePostionsList[name].Speed);
            AxisMoveTo(_prePostionsList[name].Pos);
        }

        #endregion

        internal class AddrDefinition
        {
            public string Addr;
            public string Name;
            public string DataType;
            public string Value;

            public AddrDefinition(string addr, string name, string dataType)
            {
                Addr = addr;
                Name = name;
                DataType = dataType;
            }

            public void SetValue(string value)
            {
                Value = value;
            }
        }

        internal class PrePostion
        {
            public string Speed;
            public string Pos;

            public PrePostion(string pos, string speed)
            {
                Pos = pos;
                Speed = speed;
            }
        }
    }
}
