using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using CommonUtility.BusLoader;

namespace Controller
{
    [Description("LIN-Product,GEELY_仪表盘氛围灯")]
    public sealed class GeeleyAutoRgbLamp : ControllerBase
    {
        public LinBus Lin;

        public GeeleyAutoRgbLamp(string name) :
            base(name)
        {
            if (MainWorkThread != null)
            {
                MainWorkThread.Abort();
                MainWorkThread.Join();
            }

            MainWorkThread = new Thread(MainWork);
            MainWorkThread.Start();
        }

        ~GeeleyAutoRgbLamp()
        {
            Dispose();
        }

        private Thread MainWorkThread { get; set; }
        private bool IsSleep { get; set; }
        private int _periodTimeCount;
        //private LampType _lampType;
        private byte _breathCount;

        private byte _redByte;
        private byte _greenByte;
        private byte _blueByte;
        private bool _isBreathMode;
        private byte _brightness = 0x3F;

        private void MainWork()
        {
            while (MainWorkThread.IsAlive)
            {
                if (!MainWorkThread.IsAlive)
                    break;

                Thread.Sleep(25);

                if (Lin == null)
                    continue;

                if (IsSleep)
                    continue;

                try
                {
                    if (Lin == null)
                        continue;

                    _periodTimeCount++;

                    if (_periodTimeCount == 1)
                    {
                        if (_isBreathMode)
                        {
                            Lin.SendMasterLin(0x1A, new byte[] { 0x00, 0x40, 0x09, 0x00, 0x1F, _breathCount, 0x00, 0x00 });
                            _breathCount++;

                            if (_breathCount == 0x3F + 0x01)
                                _breathCount = 0;
                        }
                        else
                        {
                            Lin.SendMasterLin(0x1A, new byte[] { 0x00, 0x40, 0x09, 0x00, 0x00, _brightness, 0x00, 0x00 });
                        }
                    }
                    else if (_periodTimeCount == 2)
                    {
                        _periodTimeCount = 0;

                        if (_isBreathMode)
                        {
                            Lin.SendMasterLin(0x1A, new byte[] { 0x00, 0x40, 0x09, 0x00, 0x1F, _breathCount, 0x00, 0x00 });
                            _breathCount++;

                            if (_breathCount == 0x3F + 0x01)
                                _breathCount = 0;
                        }
                        else
                        {
                            Lin.SendMasterLin(0x1A, new byte[] { 0x00, 0x40, 0x09, 0x00, 0x00, _brightness, 0x00, 0x00 });
                        }

                        Lin.SendMasterLin(0x1F, new byte[] { 0x00, _redByte, _greenByte, _blueByte, _isBreathMode ? (byte)0xFF : (byte)0x00, 0x00, 0x00, 0x00 });

                        //Lin.SendMasterLin(0x1F, new byte[] { 0x00, _redByte, _greenByte, _blueByte, 0x00, 0x00, 0x00, 0x00 });
                    }

                    //if (_periodTimeCount == 1)
                    //{
                    //    // send 0x1A 60ms
                    //    if (_lampType == LampType.BreathWithRed || _lampType == LampType.BreathWithGreen || _lampType == LampType.BreathWithBlue || _lampType == LampType.BreathWithWhite)
                    //    {
                    //        Lin.SendMasterLin(0x1A, new byte[] { 0x00, 0x40, 0x09, 0x00, 0x1F, _breathCount, 0x00, 0x00 });
                    //        _breathCount++;

                    //        if (_breathCount == 0x3F + 0x01)
                    //            _breathCount = 0;
                    //    }
                    //    else
                    //        Lin.SendMasterLin(0x1A, new byte[] { 0x00, 0x40, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00 });
                    //}
                    //else if (_periodTimeCount == 2)
                    //{
                    //    _periodTimeCount = 0;

                    //    // send 0x1A 60ms
                    //    if (_lampType == LampType.BreathWithRed || _lampType == LampType.BreathWithGreen || _lampType == LampType.BreathWithBlue || _lampType == LampType.BreathWithWhite)
                    //    {
                    //        Lin.SendMasterLin(0x1A, new byte[] { 0x00, 0x40, 0x09, 0x00, 0x1F, _breathCount, 0x00, 0x00 });
                    //        _breathCount++;

                    //        if (_breathCount == 0x3F + 0x01)
                    //            _breathCount = 0;
                    //    }
                    //    else
                    //        Lin.SendMasterLin(0x1A, new byte[] { 0x00, 0x40, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00 });

                    //    // send 0x1F 120ms
                    //    switch (_lampType)
                    //    {
                    //        case LampType.Default:
                    //            Lin.SendMasterLin(0x1F, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                    //            break;

                    //        case LampType.White:
                    //            Lin.SendMasterLin(0x1F, new byte[] { 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00 });
                    //            break;

                    //        case LampType.Red:
                    //            Lin.SendMasterLin(0x1F, new byte[] { 0x00, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                    //            break;

                    //        case LampType.Green:
                    //            Lin.SendMasterLin(0x1F, new byte[] { 0x00, 0x00, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00 });
                    //            break;

                    //        case LampType.Blue:
                    //            Lin.SendMasterLin(0x1F, new byte[] { 0x00, 0x00, 0x00, 0xFF, 0x00, 0x00, 0x00, 0x00 });
                    //            break;

                    //        case LampType.BreathWithRed:
                    //            Lin.SendMasterLin(0x1F, new byte[] { 0x00, 0xFF, 0x00, 0x00, 0xFF, 0x00, 0x00, 0x00 });
                    //            break;

                    //        case LampType.BreathWithGreen:
                    //            Lin.SendMasterLin(0x1F, new byte[] { 0x00, 0x00, 0xFF, 0x00, 0xFF, 0x00, 0x00, 0x00 });
                    //            break;

                    //        case LampType.BreathWithBlue:
                    //            Lin.SendMasterLin(0x1F, new byte[] { 0x00, 0x00, 0x00, 0xFF, 0xFF, 0x00, 0x00, 0x00 });
                    //            break;

                    //        case LampType.BreathWithWhite:
                    //            Lin.SendMasterLin(0x1F, new byte[] { 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00 });
                    //            break;

                    //        default:
                    //            throw new ArgumentOutOfRangeException();
                    //    }
                    //}
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        [Description("控制板唤醒")]
        public void LampAwake()
        {
            if (IsSleep)
            {
                _isBreathMode = false;
                _redByte = 0x00;
                _greenByte = 0x00;
                _blueByte = 0x0;
                _brightness = 0x3F;
                IsSleep = false;
            }
        }

        [Description("控制板休眠")]
        public void LampSleep()
        {
            IsSleep = true;
            _isBreathMode = false;
            _redByte = 0x00;
            _greenByte = 0x00;
            _blueByte = 0x0;
            _brightness = 0x3F;
        }

        //[Description("设置亮度")]
        //public void SetBrightness(string brightness)
        //{
        //    if (IsSleep)
        //        return;

        //    if (_isBreathMode)
        //        return;

        //    byte gray;
        //    if (!byte.TryParse(brightness, out gray))
        //        return;
        //    if (gray <= 0x3F)
        //        _brightness = gray;
        //}

        [Description("切换RBG")]
        public void SwitchRgd(string red, string green, string blue)
        {
            if (IsSleep)
                return;

            byte rGray;
            byte gGray;
            byte bGray;

            if (byte.TryParse(red, out rGray) && byte.TryParse(green, out gGray) && byte.TryParse(blue, out bGray))
            {
                _redByte = rGray;
                _greenByte = gGray;
                _blueByte = bGray;
            }
        }

        [Description("呼吸模式打开")]
        public void BreathModeOn()
        {
            if (IsSleep)
                return;
            _isBreathMode = true;
        }

        [Description("呼吸模式关闭")]
        public void BreathModeOff()
        {
            if (IsSleep)
                return;
            _isBreathMode = false;

            //byte rGray = _redByte;
            //byte gGray = _greenByte;
            //byte bGray = _blueByte;

            //_redByte = 0x00;
            //_greenByte = 0x00;
            //_blueByte = 0x0;

            //Thread.Sleep(250);

            //_redByte = rGray;
            //_greenByte = gGray;
            //_blueByte = bGray;
        }

        [Description("Red")]
        public void SwitchRed()
        {
            SwitchRgd("255", "0", "0");
        }

        [Description("Green")]
        public void SwitchGreen()
        {
            SwitchRgd("0", "255", "0");
        }

        [Description("Blue")]
        public void SwitchBlue()
        {
            SwitchRgd("0", "0", "255");
        }

        [Description("AutoColorOn")]
        public void AutoColorOn()
        {
            if (IsSleep)
                return;

            var colors = typeof (Color).GetProperties();
            foreach (var color in colors)
            {
                var x=color.GetValue(typeof(Color));
                var rGray = ((Color) x).R;
                var gGray = ((Color)x).G;
                var bGray = ((Color)x).B;

                Console.WriteLine(((Color) x).Name);
                SwitchRgd(rGray.ToString(), gGray.ToString(), bGray.ToString());
                Thread.Sleep(850);
                //var rGray = ((Color) color).A;
            }
        }

        //[Description("BreathWithRed")]
        //public void BreathEnableWithRed()
        //{
        //    SwitchLampType(LampType.BreathWithRed);
        //}

        //[Description("BreathWithGreen")]
        //public void BreathEnableWithGreen()
        //{
        //    SwitchLampType(LampType.BreathWithGreen);
        //}

        //[Description("BreathWithBlue")]
        //public void BreathEnableWithBlue()
        //{
        //    SwitchLampType(LampType.BreathWithBlue);
        //}

        //[Description("BreathWithWhite")]
        //public void BreathEnableWithWhite()
        //{
        //    SwitchLampType(LampType.BreathWithWhite);
        //}

        //[Description("Default")]
        //public void BreathDisable()
        //{
        //    SwitchLampType(LampType.Default);
        //}

        //[Description("LampOn")]
        //public void LampOn()
        //{
        //    SwitchLampType(LampType.Red);
        //}

        //[Description("LampOff")]
        //public void LampOff()
        //{
        //    SwitchLampType(LampType.Default);
        //}

        //private void SwitchLampType(LampType lampType)
        //{
        //    _lampType = lampType;
        //}

        //private enum LampType
        //{
        //    Default,

        //    White,

        //    Red,

        //    Green,

        //    Blue,

        //    BreathWithRed,

        //    BreathWithGreen,

        //    BreathWithBlue,

        //    BreathWithWhite,
        //}
    }
}
