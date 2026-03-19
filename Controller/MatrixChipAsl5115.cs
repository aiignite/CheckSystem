using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Controller
{
    [Description("CAN-Product,MAXTRIX芯片ASL5115")]
    public sealed class MatrixChipAsl5115 : ControllerBase
    {
        public CanBus Can;

        public MatrixChipAsl5115(string name)
            : base(name)
        {
            CanBus.PushCanMsg += CanBus_PushCanMsg;
            _backgroundWorkTh = new Thread(BackgroundWork) { IsBackground = true };
            _backgroundWorkTh.Start();
        }

        private void CanBus_PushCanMsg(
            string name, CanBus.CanDataPackage data, CanBus.OnPushCanDataType onPushCanDataType)
        {
            if (Can != null && Can.Name == name)
            {
                if (_isReadingMtp && string.IsNullOrEmpty(_readMtpBuff))
                {
                    if (data != null && data.CanDataLen == 2 && data.CanId == RecvCanId + _moduleAddress)
                    {
                        Array.Reverse(data.CanData);
                        //_readMtpBuff = data.CanData.Aggregate(_readMtpBuff, (current, t) => current + ValueHelper.GetHextStr(t));
                        _readMtpBuff = ValueHelper.GetHextStr(data.CanData).Replace(" ", "");

                        _waitReadHandle.Set();
                    }
                }
            }
        }

        ~MatrixChipAsl5115()
        {
            Dispose(true);
        }

        protected override void Dispose(bool isDisposeing)
        {
            if (!isDisposeing)
                return;
            if (_backgroundWorkTh == null)
                return;
            _backgroundWorkTh.Abort();
            _backgroundWorkTh.Join();
        }

        private void BackgroundWork()
        {
            while (_backgroundWorkTh.IsAlive)
            {
                if (!_backgroundWorkTh.IsAlive)
                    break;

                if (Can == null)
                    continue;

                Thread.Sleep(25);

                if (!_isKeepNormalMode)
                    continue;

                if (_isMtpConfiguration)
                    continue;

                var pwmPercent1 = new List<byte>();
                for (var i = 1; i <= 4; i++)
                {
                    var index = i;
                    // 设置占空比
                    var fieldValue =
                        bool.Parse(GetType().GetField("Led" + index).GetValue(this).ToString());
                    pwmPercent1.AddRange(fieldValue ? new byte[] { 0xff, 0x0f } : new byte[] { 0x00, 0x00 });
                }
                Can.SendExtendedCanData(0x15540440 + _moduleAddress, _bytesNull);
                Thread.Sleep(5);
                Can.SendExtendedCanData(0x155404A0 + _moduleAddress, new byte[] { 0x7E });
                Thread.Sleep(5);
                SendCanMsg(OpenPwm, new byte[] { 0xFF, 0x0F });
                Thread.Sleep(5);
                SendCanMsg(SetPwm1, pwmPercent1);
                Thread.Sleep(5);

                var pwmPercent2 = new List<byte>();
                for (var i = 5; i <= 8; i++)
                {
                    var index = i;
                    // 设置占空比
                    var fieldValue =
                        bool.Parse(GetType().GetField("Led" + index).GetValue(this).ToString());
                    pwmPercent2.AddRange(fieldValue ? new byte[] { 0xff, 0x0f } : new byte[] { 0x00, 0x00 });
                }
                Can.SendExtendedCanData(0x15540440 + _moduleAddress, _bytesNull);
                Thread.Sleep(5);
                Can.SendExtendedCanData(0x155404A0 + _moduleAddress, new byte[] { 0x7E });
                Thread.Sleep(5);
                SendCanMsg(OpenPwm, new byte[] { 0xFF, 0x0F });
                Thread.Sleep(5);
                SendCanMsg(SetPwm2, pwmPercent2);
                Thread.Sleep(5);

                var pwmPercent3 = new List<byte>();
                for (var i = 9; i <= 12; i++)
                {
                    var index = i;
                    // 设置占空比
                    var fieldValue =
                        bool.Parse(GetType().GetField("Led" + index).GetValue(this).ToString());
                    pwmPercent3.AddRange(fieldValue ? new byte[] { 0xff, 0x0f } : new byte[] { 0x00, 0x00 });
                }
                Can.SendExtendedCanData(0x15540440 + _moduleAddress, _bytesNull);
                Thread.Sleep(5);
                Can.SendExtendedCanData(0x155404A0 + _moduleAddress, new byte[] { 0x7E });
                Thread.Sleep(5);
                SendCanMsg(OpenPwm, new byte[] { 0xFF, 0x0F });
                Thread.Sleep(5);
                SendCanMsg(SetPwm3, pwmPercent3);
                Thread.Sleep(5);
            }
        }

        //public string HzRegisterValue;
        //public string Ch1To3RegisterValue;
        //public string Ch4To6RegisterValue;
        //public string Ch7To9RegisterValue;
        //public string Ch10To12RegisterValue;

        public string Reg0X34;
        public string Reg0X35;
        public string Reg0X36;
        public string Reg0X37;

        [Description("R,寄存器0x38读取值")]
        public string Reg0X38;

        [Description("R,寄存器0x39读取值")]
        public string Reg0X39;

        [Description("R,寄存器0x3A读取值")]
        public string Reg0X3A;

        [Description("R,寄存器0x3B读取值")]
        public string Reg0X3B;

        [Description("R,寄存器0x3C读取值")]
        public string Reg0X3C;

        [Description("R,寄存器0x3D读取值")]
        public string Reg0X3D;

        [Description("R,寄存器0x3E读取值")]
        public string Reg0X3E;

        [Description("R,寄存器0x3F读取值")]
        public string Reg0X3F;

        [Description("R,寄存器0x40读取值")]
        public string Reg0X40;

        [Description("R,寄存器0x41读取值")]
        public string Reg0X41;

        [Description("R,寄存器0x42读取值")]
        public string Reg0X42;

        [Description("R,寄存器0x43读取值")]
        public string Reg0X43;

        [Description("R,寄存器0x44读取值")]
        public string Reg0X44;

        [Description("R,寄存器0x45读取值")]
        public string Reg0X45;

        [Description("R,寄存器0x46读取值")]
        public string Reg0X46;

        [Description("R,寄存器0x47读取值")]
        public string Reg0X47;

        [Description("R,寄存器0x48读取值")]
        public string Reg0X48;

        [Description("R,寄存器0x49读取值")]
        public string Reg0X49;

        [Description("R,寄存器0x4A读取值")]
        public string Reg0X4A;

        [Description("R,寄存器0x4B读取值")]
        public string Reg0X4B;

        [Description("R,寄存器0x4C读取值")]
        public string Reg0X4C;

        [Description("R,寄存器0x4D读取值")]
        public string Reg0X4D;

        [Description("R,寄存器0x4E读取值")]
        public string Reg0X4E;

        [Description("R,寄存器0x4F读取值")]
        public string Reg0X4F;

        [Description("R,寄存器0x50读取值")]
        public string Reg0X50;

        [Description("R,寄存器0x51读取值")]
        public string Reg0X51;

        [Description("R,寄存器0x52读取值")]
        public string Reg0X52;

        [Description("R,寄存器0x53读取值")]
        public string Reg0X53;

        [Description("R,寄存器0x54读取值")]
        public string Reg0X54;

        [Description("R,寄存器0x55读取值")]
        public string Reg0X55;

        [Description("R,寄存器0x56读取值")]
        public string Reg0X56;

        [Description("R,寄存器0x57读取值")]
        public string Reg0X57;

        [Description("R,寄存器0x58读取值")]
        public string Reg0X58;

        [Description("R,寄存器0x59读取值")]
        public string Reg0X59;

        [Description("R,寄存器0x5A读取值")]
        public string Reg0X5A;

        [Description("R,寄存器0x5B读取值")]
        public string Reg0X5B;

        [Description("R,寄存器0x5C读取值")]
        public string Reg0X5C;

        [Description("R,寄存器0x5D读取值")]
        public string Reg0X5D;

        [Description("R,寄存器0x5E读取值")]
        public string Reg0X5E;

        [Description("R,寄存器0x5F读取值")]
        public string Reg0X5F;

        [Description("R,寄存器0x60读取值")]
        public string Reg0X60;

        /// <summary>
        /// 模块地址
        /// </summary>
        private uint _moduleAddress;

        /// <summary>
        /// CAN ID
        /// 开PWM通道扩展帧
        /// </summary>
        private const uint OpenPwm = 0x15540080;

        /// <summary>
        /// CAN ID
        /// 关PWM通道拓展帧
        /// </summary>
        private const uint ClosePwm = 0x155400A0;

        /// <summary>
        /// CAN ID
        /// 配置PWM 通道1 2 3 4的占空比
        /// </summary>
        private const uint SetPwm1 = 0x15540100;

        /// <summary>
        /// CAN ID
        /// 配置PWM 通道5 6 7 8的占空比
        /// </summary>
        private const uint SetPwm2 = 0x15540120;

        /// <summary>
        /// CAN ID
        /// 配置PWM 通道9 10 11 12的占空比
        /// </summary>
        private const uint SetPwm3 = 0x15540140;

        /// <summary>
        /// Selective Register Write
        /// 02x
        /// </summary>
        private const uint SelectiveRegisterWrite = 0x15540020;

        /// <summary>
        /// MTP Write
        /// 1Cx
        /// </summary>
        private const uint MtpWrite = 0x155401C0;

        /// <summary>
        /// Selective Register Read
        /// 22x
        /// </summary>
        private const uint SelectiveRegisterRead = 0x15540220;

        /// <summary>
        /// MTP Read
        /// 3Cx
        /// </summary>
        private const uint MtpRead = 0x155403C0;

        private const uint RecvCanId = 0x7E0;

        /// <summary>
        /// 选择所有通道的数据
        /// </summary>
        private readonly byte[] _allChSelectBytes = { 0xff, 0x0f };

        /// <summary>
        /// 空字节
        /// </summary>
        private readonly byte[] _bytesNull = { };

        /// <summary>
        /// 单个通道默认占空比
        /// 最亮是FF 0F 
        /// 即百分百
        /// </summary>
        private readonly byte[] _pwmPercent = { 0xff, 0x0f };

        [Description("R/W,LED1开关")]
        public bool Led1;

        [Description("R/W,LED2开关")]
        public bool Led2;

        [Description("R/W,LED3开关")]
        public bool Led3;

        [Description("R/W,LED4开关")]
        public bool Led4;

        [Description("R/W,LED5开关")]
        public bool Led5;

        [Description("R/W,LED6开关")]
        public bool Led6;

        [Description("R/W,LED7开关")]
        public bool Led7;

        [Description("R/W,LED8开关")]
        public bool Led8;

        [Description("R/W,LED9开关")]
        public bool Led9;

        [Description("R/W,LED10开关")]
        public bool Led10;

        [Description("R/W,LED11开关")]
        public bool Led11;

        [Description("R/W,LED12开关")]
        public bool Led12;

        private readonly string[] _start1 = { "0", "0", "0", "0", "0", "0", "0", "0" };
        private readonly string[] _start2 = { "0", "0", "0", "0", "0", "0", "0", "0" };

        private readonly string[] _immpoff1 = { "1", "1", "1", "1", "1", "1", "1", "1" };
        private readonly string[] _immpoff2 = { "1", "1", "1", "1", "1", "1", "1", "1" };

        private readonly byte[] _startBytes = { 0, 0 };
        private readonly byte[] _immpoffBytes = { 0xff, 0x0f };

        private readonly Thread _backgroundWorkTh;
        private bool _isKeepNormalMode;
        private bool _isMtpConfiguration;
        //private static object _lockRead = new object();
        private bool _isReadingMtp;
        private string _readMtpBuff = string.Empty;
        private readonly EventWaitHandle _waitReadHandle = new AutoResetEvent(false);

        /// <summary>
        /// 初始化模块地址
        /// </summary>
        /// <param name="ligthType"></param>
        [Description("初始化模块地址")]
        public void InitLightType(string ligthType)
        {
            //switch (ligthType)
            //{
            //    case "00000":
            //        _moduleAddress = 0;
            //        break;

            //    case "00100":
            //        _moduleAddress = 4;
            //        break;

            //    case "00011":
            //        _moduleAddress = 3;
            //        break;

            //    case "00001":
            //        _moduleAddress = 1;
            //        break;

            //    case "00010":
            //        _moduleAddress = 2;
            //        break;

            //    default:
            //        throw new ArgumentOutOfRangeException("ligthType", ligthType, null);
            //}

            _moduleAddress = Convert.ToByte(ligthType.PadLeft(8, '0'), 2);
            if (Can != null)
                Can.AddDoNotFilterCanId(RecvCanId + _moduleAddress);
        }

        /// <summary>
        /// 切换LED开关
        /// </summary>
        /// <param name="index">LED序号</param>
        [Description("切换LED开关")]
        public void SwitchLed(int index)
        {
            if (index < 1 || index > 12)
                return;

            var fieldValue =
                bool.Parse(GetType().GetField("Led" + index).GetValue(this).ToString());
            GetType().GetField("Led" + index).SetValue(this, !fieldValue);
            //Thread.Sleep(20);
            //if (index >= 1 && index <= 8)
            //{
            //    if (!fieldValue)
            //    {
            //        var tempOn = _start1;
            //        Array.Reverse(tempOn);
            //        tempOn[index - 1] = "1";
            //        Array.Reverse(tempOn);
            //        Array.Copy(tempOn, _start1, 8);

            //        var tempOff = _immpoff1;
            //        Array.Reverse(tempOff);
            //        tempOff[index - 1] = "0";
            //        Array.Reverse(tempOff);
            //        Array.Copy(tempOff, _immpoff1, 8);
            //    }
            //    else
            //    {
            //        var tempOn = _start1;
            //        Array.Reverse(tempOn);
            //        tempOn[index - 1] = "0";
            //        Array.Reverse(tempOn);
            //        Array.Copy(tempOn, _start1, 8);

            //        var tempOff = _immpoff1;
            //        Array.Reverse(tempOff);
            //        tempOff[index - 1] = "1";
            //        Array.Reverse(tempOff);
            //        Array.Copy(tempOff, _immpoff1, 8);
            //    }
            //}
            //else
            //{
            //    if (!fieldValue)
            //    {
            //        var temp = _start2;
            //        Array.Reverse(temp);
            //        temp[index - 8 - 1] = "1";
            //        Array.Reverse(temp);
            //        Array.Copy(temp, _start2, 8);

            //        var tempOff = _immpoff2;
            //        Array.Reverse(tempOff);
            //        tempOff[index - 8 - 1] = "0";
            //        Array.Reverse(tempOff);
            //        Array.Copy(tempOff, _immpoff2, 8);
            //    }
            //    else
            //    {
            //        var temp = _start2;
            //        Array.Reverse(temp);
            //        temp[index - 8 - 1] = "0";
            //        Array.Reverse(temp);
            //        Array.Copy(temp, _start2, 8);

            //        var tempOff = _immpoff2;
            //        Array.Reverse(tempOff);
            //        tempOff[index - 8 - 1] = "1";
            //        Array.Reverse(tempOff);
            //        Array.Copy(tempOff, _immpoff2, 8);
            //    }
            //}

            //_startBytes[0] =
            //    Convert.ToByte(_start1.Aggregate(string.Empty, (current, t) => current + t), 2);
            //_startBytes[1] =
            //    Convert.ToByte(_start2.Aggregate(string.Empty, (current, t) => current + t), 2);
            //_immpoffBytes[0] =
            //    Convert.ToByte(_immpoff1.Aggregate(string.Empty, (current, t) => current + t), 2);
            //_immpoffBytes[1] =
            //    Convert.ToByte(_immpoff2.Aggregate(string.Empty, (current, t) => current + t), 2);

            //if (!fieldValue)
            //    SendCanMsg(OpenPwm, _startBytes);
            //else
            //    SendCanMsg(ClosePwm, _immpoffBytes);
            //Thread.Sleep(200);
        }

        /// <summary>
        /// 退出LMP模式 进入正常模式
        /// </summary>
        [Description("退出LMP模式 进入正常模式")]
        public void EnterMormalMode()
        {
            if (Led1)
                SwitchLed(1);
            if (Led2)
                SwitchLed(2);
            if (Led3)
                SwitchLed(3);
            if (Led4)
                SwitchLed(4);
            if (Led5)
                SwitchLed(5);
            if (Led6)
                SwitchLed(6);
            if (Led7)
                SwitchLed(7);
            if (Led8)
                SwitchLed(8);
            if (Led9)
                SwitchLed(9);
            if (Led10)
                SwitchLed(10);
            if (Led11)
                SwitchLed(11);
            if (Led12)
                SwitchLed(12);

            for (var i = 0; i < 6; i++)
                SendCanMsg(0x15540440, _bytesNull);
            SendCanMsg(0x15540060, new byte[] { 0x1E, 0x2E, 0x4E });

            Thread.Sleep(100);
            // 读当前通道寄存器
            // 通过CAN ID区分
            // 7E0 == 00000
            // 7E1 == 00001
            // 7E3 == 00011
            // 7E4 == 00100
            SendCanMsg(0x15540460, _bytesNull);

            // 把所有LED关闭
            SendCanMsg(ClosePwm, _allChSelectBytes);

            // 设置占空比
            var pwmPercent = new List<byte>();
            for (var i = 0; i < 4; i++)
                pwmPercent.AddRange(new byte[] { 0x00, 0x00 });
            SendCanMsg(SetPwm1, pwmPercent);
            SendCanMsg(SetPwm2, pwmPercent);
            SendCanMsg(SetPwm3, pwmPercent);

            Thread.Sleep(200);

            _isKeepNormalMode = true;
        }

        /// <summary>
        /// 退出正常模式 进入LMP模式
        /// </summary>
        [Description("退出正常模式 进入LMP模式")]
        public void ExitMormalMode()
        {
            _isKeepNormalMode = false;
            Thread.Sleep(100);

            if (Led1)
                SwitchLed(1);
            if (Led2)
                SwitchLed(2);
            if (Led3)
                SwitchLed(3);
            if (Led4)
                SwitchLed(4);
            if (Led5)
                SwitchLed(5);
            if (Led6)
                SwitchLed(6);
            if (Led7)
                SwitchLed(7);
            if (Led8)
                SwitchLed(8);
            if (Led9)
                SwitchLed(9);
            if (Led10)
                SwitchLed(10);
            if (Led11)
                SwitchLed(11);
            if (Led12)
                SwitchLed(12);
        }

        [Description("写寄存器0x38的值")]
        public void WriteMtpReg0X38(string value)
        {
            WriteMtp(0x38, value);
        }

        [Description("写寄存器0x39的值")]
        public void WriteMtpReg0X39(string value)
        {
            WriteMtp(0x39, value);
        }

        [Description("写寄存器0x3A的值")]
        public void WriteMtpReg0X3A(string value)
        {
            WriteMtp(0x3A, value);
        }

        [Description("写寄存器0x3B的值")]
        public void WriteMtpReg0X3B(string value)
        {
            WriteMtp(0x3B, value);
        }

        [Description("写寄存器0x3C的值")]
        public void WriteMtpReg0X3C(string value)
        {
            WriteMtp(0x3C, value);
        }

        [Description("写寄存器0x3D的值")]
        public void WriteMtpReg0X3D(string value)
        {
            WriteMtp(0x3D, value);
        }

        [Description("写寄存器0x3E的值")]
        public void WriteMtpReg0X3E(string value)
        {
            WriteMtp(0x3E, value);
        }

        [Description("写寄存器0x3F的值")]
        public void WriteMtpReg0X3F(string value)
        {
            WriteMtp(0x3F, value);
        }

        [Description("写寄存器0x40的值")]
        public void WriteMtpReg0X40(string value)
        {
            WriteMtp(0x40, value);
        }

        [Description("写寄存器0x41的值")]
        public void WriteMtpReg0X41(string value)
        {
            WriteMtp(0x41, value);
        }

        [Description("写寄存器0x42的值")]
        public void WriteMtpReg0X42(string value)
        {
            WriteMtp(0x42, value);
        }

        [Description("写寄存器0x43的值")]
        public void WriteMtpReg0X43(string value)
        {
            WriteMtp(0x43, value);
        }

        [Description("写寄存器0x44的值")]
        public void WriteMtpReg0X44(string value)
        {
            WriteMtp(0x44, value);
        }

        [Description("写寄存器0x45的值")]
        public void WriteMtpReg0X45(string value)
        {
            WriteMtp(0x45, value);
        }

        [Description("写寄存器0x46的值")]
        public void WriteMtpReg0X46(string value)
        {
            WriteMtp(0x46, value);
        }

        [Description("写寄存器0x47的值")]
        public void WriteMtpReg0X47(string value)
        {
            WriteMtp(0x47, value);
        }

        [Description("写寄存器0x48的值")]
        public void WriteMtpReg0X48(string value)
        {
            WriteMtp(0x48, value);
        }

        [Description("写寄存器0x49的值")]
        public void WriteMtpReg0X49(string value)
        {
            WriteMtp(0x49, value);
        }

        [Description("写寄存器0x4A的值")]
        public void WriteMtpReg0X4A(string value)
        {
            WriteMtp(0x4A, value);
        }

        [Description("写寄存器0x4B的值")]
        public void WriteMtpReg0X4B(string value)
        {
            WriteMtp(0x4B, value);
        }

        [Description("写寄存器0x4C的值")]
        public void WriteMtpReg0X4C(string value)
        {
            WriteMtp(0x4C, value);
        }

        [Description("写寄存器0x4D的值")]
        public void WriteMtpReg0X4D(string value)
        {
            WriteMtp(0x4D, value);
        }

        [Description("写寄存器0x4E的值")]
        public void WriteMtpReg0X4E(string value)
        {
            WriteMtp(0x4E, value);
        }

        [Description("写寄存器0x4F的值")]
        public void WriteMtpReg0X4F(string value)
        {
            WriteMtp(0x4F, value);
        }

        [Description("写寄存器0x50的值")]
        public void WriteMtpReg0X50(string value)
        {
            WriteMtp(0x50, value);
        }

        [Description("写寄存器0x51的值")]
        public void WriteMtpReg0X51(string value)
        {
            WriteMtp(0x51, value);
        }

        [Description("写寄存器0x52的值")]
        public void WriteMtpReg0X52(string value)
        {
            WriteMtp(0x52, value);
        }

        [Description("写寄存器0x53的值")]
        public void WriteMtpReg0X53(string value)
        {
            WriteMtp(0x53, value);
        }

        [Description("写寄存器0x54的值")]
        public void WriteMtpReg0X54(string value)
        {
            WriteMtp(0x54, value);
        }

        [Description("写寄存器0x55的值")]
        public void WriteMtpReg0X55(string value)
        {
            WriteMtp(0x55, value);
        }

        [Description("写寄存器0x56的值")]
        public void WriteMtpReg0X56(string value)
        {
            WriteMtp(0x56, value);
        }

        [Description("写寄存器0x57的值")]
        public void WriteMtpReg0X57(string value)
        {
            WriteMtp(0x57, value);
        }

        [Description("写寄存器0x58的值")]
        public void WriteMtpReg0X58(string value)
        {
            WriteMtp(0x58, value);
        }

        [Description("写寄存器0x59的值")]
        public void WriteMtpReg0X59(string value)
        {
            WriteMtp(0x59, value);
        }

        [Description("写寄存器0x5A的值")]
        public void WriteMtpReg0X5A(string value)
        {
            WriteMtp(0x5A, value);
        }

        [Description("写寄存器0x5B的值")]
        public void WriteMtpReg0X5B(string value)
        {
            WriteMtp(0x5B, value);
        }

        [Description("写寄存器0x5C的值")]
        public void WriteMtpReg0X5C(string value)
        {
            WriteMtp(0x5C, value);
        }

        [Description("写寄存器0x5D的值")]
        public void WriteMtpReg0X5D(string value)
        {
            WriteMtp(0x5D, value);
        }

        [Description("写寄存器0x5E的值")]
        public void WriteMtpReg0X5E(string value)
        {
            WriteMtp(0x5E, value);
        }

        [Description("写寄存器0x5F的值")]
        public void WriteMtpReg0X5F(string value)
        {
            WriteMtp(0x5F, value);
        }

        [Description("写寄存器0x60的值")]
        public void WriteMtpReg0X60(string value)
        {
            WriteMtp(0x60, value);
        }

        [Description("读寄存器0x34的值")]
        public void ReadMtpReg0X34()
        {
            Reg0X34 = string.Empty;
            Reg0X34 = ReadMtp(0x34);
        }

        [Description("读寄存器0x35的值")]
        public void ReadMtpReg0X35()
        {
            Reg0X35 = string.Empty;
            Reg0X35 = ReadMtp(0x35);
        }

        [Description("读寄存器0x36的值")]
        public void ReadMtpReg0X36()
        {
            Reg0X36 = string.Empty;
            Reg0X36 = ReadMtp(0x36);
        }

        [Description("读寄存器0x37的值")]
        public void ReadMtpReg0X37()
        {
            Reg0X37 = string.Empty;
            Reg0X37 = ReadMtp(0x37);
        }

        [Description("读寄存器0x38的值")]
        public void ReadMtpReg0X38()
        {
            Reg0X38 = string.Empty;
            Reg0X38 = ReadMtp(0x38);
        }

        [Description("读寄存器0x39的值")]
        public void ReadMtpReg0X39()
        {
            Reg0X39 = string.Empty;
            Reg0X39 = ReadMtp(0x39);
        }

        [Description("读寄存器0x3A的值")]
        public void ReadMtpReg0X3A()
        {
            Reg0X3A = string.Empty;
            Reg0X3A = ReadMtp(0x3A);
        }

        [Description("读寄存器0x3B的值")]
        public void ReadMtpReg0X3B()
        {
            Reg0X3B = string.Empty;
            Reg0X3B = ReadMtp(0x3B);
        }

        [Description("读寄存器0x3C的值")]
        public void ReadMtpReg0X3C()
        {
            Reg0X3C = string.Empty;
            Reg0X3C = ReadMtp(0x3C);
        }

        [Description("读寄存器0x3D的值")]
        public void ReadMtpReg0X3D()
        {
            Reg0X3D = string.Empty;
            Reg0X3D = ReadMtp(0x3D);
        }

        [Description("读寄存器0x3E的值")]
        public void ReadMtpReg0X3E()
        {
            Reg0X3E = string.Empty;
            Reg0X3E = ReadMtp(0x3E);
        }

        [Description("读寄存器0x3F的值")]
        public void ReadMtpReg0X3F()
        {
            Reg0X3F = string.Empty;
            Reg0X3F = ReadMtp(0x3F);
        }

        [Description("读寄存器0x40的值")]
        public void ReadMtpReg0X40()
        {
            Reg0X40 = string.Empty;
            Reg0X40 = ReadMtp(0x40);
        }

        [Description("读寄存器0x41的值")]
        public void ReadMtpReg0X41()
        {
            Reg0X41 = string.Empty;
            Reg0X41 = ReadMtp(0x41);
        }

        [Description("读寄存器0x42的值")]
        public void ReadMtpReg0X42()
        {
            Reg0X42 = string.Empty;
            Reg0X42 = ReadMtp(0x42);
        }

        [Description("读寄存器0x43的值")]
        public void ReadMtpReg0X43()
        {
            Reg0X43 = string.Empty;
            Reg0X43 = ReadMtp(0x43);
        }

        [Description("读寄存器0x44的值")]
        public void ReadMtpReg0X44()
        {
            Reg0X44 = string.Empty;
            Reg0X44 = ReadMtp(0x44);
        }

        [Description("读寄存器0x45的值")]
        public void ReadMtpReg0X45()
        {
            Reg0X45 = string.Empty;
            Reg0X45 = ReadMtp(0x45);
        }

        [Description("读寄存器0x46的值")]
        public void ReadMtpReg0X46()
        {
            Reg0X46 = string.Empty;
            Reg0X46 = ReadMtp(0x46);
        }

        [Description("读寄存器0x47的值")]
        public void ReadMtpReg0X47()
        {
            Reg0X47 = string.Empty;
            Reg0X47 = ReadMtp(0x47);
        }

        [Description("读寄存器0x48的值")]
        public void ReadMtpReg0X48()
        {
            Reg0X48 = string.Empty;
            Reg0X48 = ReadMtp(0x48);
        }

        [Description("读寄存器0x49的值")]
        public void ReadMtpReg0X49()
        {
            Reg0X49 = string.Empty;
            Reg0X49 = ReadMtp(0x49);
        }

        [Description("读寄存器0x4A的值")]
        public void ReadMtpReg0X4A()
        {
            Reg0X4A = string.Empty;
            Reg0X4A = ReadMtp(0x4A);
        }

        [Description("读寄存器0x4B的值")]
        public void ReadMtpReg0X4B()
        {
            Reg0X4B = string.Empty;
            Reg0X4B = ReadMtp(0x4B);
        }

        [Description("读寄存器0x4C的值")]
        public void ReadMtpReg0X4C()
        {
            Reg0X4C = string.Empty;
            Reg0X4C = ReadMtp(0x4C);
        }

        [Description("读寄存器0x4D的值")]
        public void ReadMtpReg0X4D()
        {
            Reg0X4D = string.Empty;
            Reg0X4D = ReadMtp(0x4D);
        }

        [Description("读寄存器0x4E的值")]
        public void ReadMtpReg0X4E()
        {
            Reg0X4E = string.Empty;
            Reg0X4E = ReadMtp(0x4E);
        }

        [Description("读寄存器0x4F的值")]
        public void ReadMtpReg0X4F()
        {
            Reg0X4F = string.Empty;
            Reg0X4F = ReadMtp(0x4F);
        }

        [Description("读寄存器0x50的值")]
        public void ReadMtpReg0X50()
        {
            Reg0X50 = string.Empty;
            Reg0X50 = ReadMtp(0x50);
        }

        [Description("读寄存器0x51的值")]
        public void ReadMtpReg0X51()
        {
            Reg0X51 = string.Empty;
            Reg0X51 = ReadMtp(0x51);
        }

        [Description("读寄存器0x52的值")]
        public void ReadMtpReg0X52()
        {
            Reg0X52 = string.Empty;
            Reg0X52 = ReadMtp(0x52);
        }

        [Description("读寄存器0x53的值")]
        public void ReadMtpReg0X53()
        {
            Reg0X53 = string.Empty;
            Reg0X53 = ReadMtp(0x53);
        }

        [Description("读寄存器0x54的值")]
        public void ReadMtpReg0X54()
        {
            Reg0X54 = string.Empty;
            Reg0X54 = ReadMtp(0x54);
        }

        [Description("读寄存器0x55的值")]
        public void ReadMtpReg0X55()
        {
            Reg0X55 = string.Empty;
            Reg0X55 = ReadMtp(0x55);
        }

        [Description("读寄存器0x56的值")]
        public void ReadMtpReg0X56()
        {
            Reg0X56 = string.Empty;
            Reg0X56 = ReadMtp(0x56);
        }

        [Description("读寄存器0x57的值")]
        public void ReadMtpReg0X57()
        {
            Reg0X57 = string.Empty;
            Reg0X57 = ReadMtp(0x57);
        }

        [Description("读寄存器0x58的值")]
        public void ReadMtpReg0X58()
        {
            Reg0X58 = string.Empty;
            Reg0X58 = ReadMtp(0x58);
        }

        [Description("读寄存器0x59的值")]
        public void ReadMtpReg0X59()
        {
            Reg0X59 = string.Empty;
            Reg0X59 = ReadMtp(0x59);
        }

        [Description("读寄存器0x5A的值")]
        public void ReadMtpReg0X5A()
        {
            Reg0X5A = string.Empty;
            Reg0X5A = ReadMtp(0x5A);
        }

        [Description("读寄存器0x5B的值")]
        public void ReadMtpReg0X5B()
        {
            Reg0X5B = string.Empty;
            Reg0X5B = ReadMtp(0x5B);
        }

        [Description("读寄存器0x5C的值")]
        public void ReadMtpReg0X5C()
        {
            Reg0X5C = string.Empty;
            Reg0X5C = ReadMtp(0x5C);
        }

        [Description("读寄存器0x5D的值")]
        public void ReadMtpReg0X5D()
        {
            Reg0X5D = string.Empty;
            Reg0X5D = ReadMtp(0x5D);
        }

        [Description("读寄存器0x5E的值")]
        public void ReadMtpReg0X5E()
        {
            Reg0X5E = string.Empty;
            Reg0X5E = ReadMtp(0x5E);
        }

        [Description("读寄存器0x5F的值")]
        public void ReadMtpReg0X5F()
        {
            Reg0X5F = string.Empty;
            Reg0X5F = ReadMtp(0x5F);
        }

        [Description("读寄存器0x60的值")]
        public void ReadMtpReg0X60()
        {
            Reg0X60 = string.Empty;
            Reg0X60 = ReadMtp(0x60);
        }

        /// <summary>
        /// 发送CAN消息帧
        /// </summary>
        /// <param name="canId">命令ID</param>
        /// <param name="vals">内容</param>
        private void SendCanMsg(uint canId, IEnumerable<byte> vals)
        {
            if (Can == null)
                return;

            Can.SendExtendedCanData(canId + _moduleAddress, vals);
        }

        private void EnterMtpConfigurationMode(bool isWrite)
        {
            if (Can == null)
                return;

            _isMtpConfiguration = true;

            if (isWrite)
            {
                SendCanMsg(SelectiveRegisterWrite, new byte[] { 0x34, 0x8C, 0x3C, 0x7F }); // Clear the flags in reg. 0x34h and configure MTP_CFG bit = 1
                Thread.Sleep(10); // wait
            }
            else
            {
                SendCanMsg(SelectiveRegisterWrite, new byte[] { 0x3C, 0x7F });
                Thread.Sleep(10); // wait
                SendCanMsg(SelectiveRegisterWrite, new byte[] { 0x34, 0xDF });
                Thread.Sleep(10); // wait
            }
        }

        private void ExitMtpConfigurationMode()
        {
            SendCanMsg(SelectiveRegisterWrite, new byte[] { 0x3C, 0x7E }); // Exit MTP configuration.
            Thread.Sleep(25); // wait

            _isMtpConfiguration = false;
        }

        private void WriteMtp(byte address, string value)
        {
            if (Can == null)
                return;

            EnterMtpConfigurationMode(true);

            var val1 = Convert.ToByte(value.Substring(0, 2), 16);
            var val2 = Convert.ToByte(value.Substring(2, 2), 16);
            SendCanMsg(MtpWrite, new byte[] { address, val2, val1, 0x00 }); // Write value [val1,val2] in register [address] of MTP. Last byte is the MTP Key
            Thread.Sleep(50); // wait
            SendCanMsg(SelectiveRegisterRead, new byte[] { 0x34 }); // check Illegal_Access_Status and MTP_Access_Status in reg 0x34h
            Thread.Sleep(50); // wait
            SendCanMsg(SelectiveRegisterWrite, new byte[] { 0x34, 0x8C }); // Clear the flags for next process
            Thread.Sleep(50);

            ExitMtpConfigurationMode();
        }

        private string ReadMtp(byte address)
        {
            if (Can == null)
                return string.Empty;

            EnterMtpConfigurationMode(false);

            var addr = Convert.ToByte(address);
            var temp = string.Empty;

            _readMtpBuff = string.Empty;

            var taskSendCmd = new Task(() =>
            {
                SendCanMsg(MtpRead, new byte[] { addr, 0x00 }); // Read register addr in MTP with Lock Key = 0
                Thread.Sleep(50);
                SendCanMsg(SelectiveRegisterRead, new byte[] { 0x34 }); // Check the MTP access and process flags
                Thread.Sleep(50);

                _isReadingMtp = true;
                SendCanMsg(SelectiveRegisterRead, new byte[] { 0x40, 0x41 }); // Read registers 40h and 41h. Answer will show MSB LSB of Register addr.
            });

            var taskWait = new Task(() =>
            {
                if (_waitReadHandle.WaitOne(1000))
                {
                    temp = _readMtpBuff;
                }
            });

            taskSendCmd.Start();
            taskWait.Start();
            Task.WaitAll(taskSendCmd, taskWait);

            _isReadingMtp = false;
            _readMtpBuff = string.Empty;

            ExitMtpConfigurationMode();

            return temp;
        }

        [Description("读所有MTP")]
        public void ReadMtps()
        {
            for (var i = 0x38; i <= 0x60; i++)
            {
                var fieldName = string.Format("Reg{0}", ValueHelper.GetHextStrWithOx((byte)i).ToUpper());

                var field = GetType().GetField(fieldName);
                if (field != null)
                    field.SetValue(this, string.Empty);
            }

            if (Can == null)
                return;

            EnterMtpConfigurationMode(false);

            var noReadCount = 0;

            for (var i = 0x38; i <= 0x60; i++)
            {
                var addr = Convert.ToByte(i);
                _readMtpBuff = string.Empty;
                _waitReadHandle.Reset();

                var sendCmdTask = new Task(() =>
                {
                    SendCanMsg(MtpRead, new byte[] { addr, 0x00 }); // Read register addr in MTP with Lock Key = 0
                    Thread.Sleep(50);
                    SendCanMsg(SelectiveRegisterRead, new byte[] { 0x34 }); // Check the MTP access and process flags
                    Thread.Sleep(50);

                    _isReadingMtp = true;
                    SendCanMsg(SelectiveRegisterRead, new byte[] { 0x40, 0x41 }); // Read registers 40h and 41h. Answer will show MSB LSB of Register addr.
                });
                var i1 = i;

                var isReadOk = false;
                var watiTask = new Task(() =>
                {
                    if (_waitReadHandle.WaitOne(1000))
                    {
                        isReadOk = true;
                        var fieldName = string.Format("Reg{0}", ValueHelper.GetHextStrWithOx((byte)i1).ToUpper());

                        var field = GetType().GetField(fieldName);
                        if (field != null)
                            field.SetValue(this, _readMtpBuff);
                    }
                });

                sendCmdTask.Start();
                watiTask.Start();
                Task.WaitAll(sendCmdTask, watiTask);

                _isReadingMtp = false;
                _readMtpBuff = string.Empty;
                _waitReadHandle.Reset();

                if (!isReadOk)
                {
                    sendCmdTask = new Task(() =>
                    {
                        SendCanMsg(MtpRead, new byte[] { addr, 0x03 }); // Read register addr in MTP with Lock Key = 0
                        Thread.Sleep(50);
                        SendCanMsg(SelectiveRegisterRead, new byte[] { 0x34 }); // Check the MTP access and process flags
                        Thread.Sleep(50);

                        _isReadingMtp = true;
                        SendCanMsg(SelectiveRegisterRead, new byte[] { 0x40, 0x41 }); // Read registers 40h and 41h. Answer will show MSB LSB of Register addr.
                    });

                    watiTask = new Task(() =>
                    {
                        if (_waitReadHandle.WaitOne(1000))
                        {
                            isReadOk = true;
                            var fieldName = string.Format("Reg{0}", ValueHelper.GetHextStrWithOx((byte)i1).ToUpper());

                            var field = GetType().GetField(fieldName);
                            if (field != null)
                                field.SetValue(this, _readMtpBuff);

                            if (string.IsNullOrEmpty(_readMtpBuff))
                                noReadCount++;
                        }
                        else
                        {
                            noReadCount++;
                        }
                    });

                    sendCmdTask.Start();
                    watiTask.Start();
                    Task.WaitAll(sendCmdTask, watiTask);

                    _isReadingMtp = false;
                    _readMtpBuff = string.Empty;
                }

                if (noReadCount > 5)
                    break;
            }

            ExitMtpConfigurationMode();
        }
    }
}
