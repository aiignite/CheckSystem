using System;
using System.ComponentModel;
using System.Text;
using CommonUtility.BusLoader;

namespace Controller
{
    public sealed class ToomossUsb2XxxPwm : ControllerBase
    {
        /// <summary>
        ///  Pwm1频率
        /// </summary>
        [Description("R,Pwm1频率")]
        public float Pwm1CaptureFrequency;

        /// <summary>
        ///  Pwm1占空比
        /// </summary>
        [Description("R,Pwm1占空比")]
        public float Pwm1CaptureDuty;

        /// <summary>
        ///  Pwm2频率
        /// </summary>
        [Description("R,Pwm2频率")]
        public float Pwm2CaptureFrequency;

        /// <summary>
        ///  Pwm2占空比
        /// </summary>
        [Description("R,Pwm2占空比")]
        public float Pwm2CaptureDuty;

        private Usb2Pwm.PwmConfig _pwmConfig;
        private readonly UsbDevice.DeviceInfo _devInfo;
        private readonly int[] _devHandles = new int[20];
        private readonly int _devHandle;
        public bool State;

        public ToomossUsb2XxxPwm(string name)
            : base(name)
        {
            //扫描查找设备
            var devNum = UsbDevice.USB_ScanDevice(_devHandles);
            if (devNum <= 0)
            {
                Console.WriteLine("No device connected!");
                return;
            }
            else
            {
                Console.WriteLine("Have {0} device connected!", devNum);
            }
            _devHandle = _devHandles[0];

            //打开设备
            State = UsbDevice.USB_OpenDevice(_devHandle);
            if (!State)
            {
                Console.WriteLine("Open device error!");
                return;
            }
            else
            {
                Console.WriteLine("Open device success!");
            }
            //获取固件信息
            var funcStr = new StringBuilder(256);
            State = UsbDevice.DEV_GetDeviceInfo(_devHandle, ref _devInfo, funcStr);
            if (!State)
            {
                Console.WriteLine("Get device infomation error!");
                return;
            }

            Console.WriteLine("Firmware Info:");
            Console.WriteLine("    Name:" + Encoding.Default.GetString(_devInfo.FirmwareName));
            Console.WriteLine("    Build Date:" + Encoding.Default.GetString(_devInfo.BuildDate));
            Console.WriteLine("    Firmware Version:v{0}.{1}.{2}", (_devInfo.FirmwareVersion >> 24) & 0xFF, (_devInfo.FirmwareVersion >> 16) & 0xFF, _devInfo.FirmwareVersion & 0xFFFF);
            Console.WriteLine("    Hardware Version:v{0}.{1}.{2}", (_devInfo.HardwareVersion >> 24) & 0xFF, (_devInfo.HardwareVersion >> 16) & 0xFF, _devInfo.HardwareVersion & 0xFFFF);
            Console.WriteLine("    Functions:" + _devInfo.Functions.ToString("X8"));
            Console.WriteLine("    Functions String:" + funcStr);
        }

        [Description("PWM捕获")]
        public void PwmCap(string channelIndex)
        {
            if (channelIndex == 1.ToString())
            {
                Pwm1CaptureFrequency = -9999;
                Pwm1CaptureDuty = -9999;
            }
            else if (channelIndex == 2.ToString())
            {
                Pwm2CaptureFrequency = -9999;
                Pwm2CaptureDuty = -9999;
            }

            {
                var ret = Usb2Pwm.PWM_CAP_Init(_devHandle, byte.Parse(channelIndex));
                if (ret != Usb2Pwm.PwmSuccess)
                {
                    Console.WriteLine("Init pwm sniffer error!");
                    return;
                }
                else
                {
                    Console.WriteLine("Init pwm sniffer success!");
                }
            }

            {
                var pwmCapData = new Usb2Pwm.PwmCapData();
                var ret = Usb2Pwm.PWM_CAP_GetData(
                    _devHandles[0], byte.Parse(channelIndex), ref pwmCapData);
                if (ret == Usb2Pwm.PwmSuccess)
                {
                    if ((pwmCapData.HighValue + pwmCapData.LowValue > 0) &&
                        (pwmCapData.HighValue < 0xFFFF) && (pwmCapData.LowValue < 0xFFFF))
                    {
                        if (channelIndex == 1.ToString())
                        {
                            Pwm1CaptureFrequency = 1000000 / (pwmCapData.HighValue + pwmCapData.LowValue);
                            Pwm1CaptureDuty = (100 * pwmCapData.HighValue) / (pwmCapData.HighValue + pwmCapData.LowValue);
                        }
                        else if (channelIndex == 2.ToString())
                        {
                            Pwm2CaptureFrequency = 1000000 / (pwmCapData.HighValue + pwmCapData.LowValue);
                            Pwm2CaptureDuty = (100 * pwmCapData.HighValue) / (pwmCapData.HighValue + pwmCapData.LowValue);
                        }
                        Console.WriteLine("频率 = {0}Hz", 1000000 / (pwmCapData.HighValue + pwmCapData.LowValue));
                        Console.WriteLine("占空比 = {0}%", (100 * pwmCapData.HighValue) / (pwmCapData.HighValue + pwmCapData.LowValue));
                    }
                    else
                    {
                        Console.WriteLine("未检测到PWM信号");
                    }
                }
                else
                {
                    Console.WriteLine("PWM get data error!exit thread!");
                }
            }
        }

        //public void OpemPwm(int ms, int perCent)
        //{
        //    _pwmConfig.ChannelMask = 0xFF;//初始化所有通道
        //    _pwmConfig.Polarity = new byte[8];
        //    for (var i = 0; i < 8; i++)
        //    {
        //        _pwmConfig.Polarity[i] = 1;//将所有PWM通道都设置为正极性
        //    }
        //    _pwmConfig.Precision = new ushort[8];
        //    for (var i = 0; i < 8; i++)
        //    {
        //        _pwmConfig.Precision[i] = 100;//将所有通道的占空比调节精度都设置为1%
        //    }
        //    _pwmConfig.Prescaler = new ushort[8];
        //    for (var i = 0; i < 8; i++)
        //    {
        //        _pwmConfig.Prescaler[i] = 10;//将所有通道的预分频器都设置为10，则PWM输出频率为200MHz/(PWMConfig.Precision*PWMConfig.Prescaler)
        //    }
        //    _pwmConfig.Pulse = new ushort[8];
        //    for (var i = 0; i < 8; i++)
        //    {
        //        _pwmConfig.Pulse[i] = (ushort)(_pwmConfig.Precision[i] * perCent / 100);//将所有通道的占空比都设置为30%
        //    }

        //    // 初始化PWM
        //    var ret = USB2PWM.PWM_Init(_devHandle, ref _pwmConfig);
        //    if (ret != USB2PWM.PWM_SUCCESS)
        //    {
        //        Console.WriteLine("Initialize pwm faild!\n");
        //        Console.ReadLine();
        //        return;
        //    }
        //    Console.WriteLine("Initialize pwm sunccess!\n");
        //    //启动PWM,RunTimeOfUs之后自动停止，利用该特性可以控制输出脉冲个数，脉冲个数=RunTimeOfUs*200/(PWMConfig.Precision*PWMConfig.Prescaler)
        //    var runTimeOfUs = ms * 1000;
        //    ret = USB2PWM.PWM_Start(_devHandle, _pwmConfig.ChannelMask, runTimeOfUs);
        //    if (ret != USB2PWM.PWM_SUCCESS)
        //    {
        //        Console.WriteLine("Start pwm faild!\n");
        //        Console.ReadLine();
        //    }
        //    else
        //    {
        //        Console.WriteLine("Start pwm sunccess!\n");
        //    }
        //}

        //public void ClosePwn()
        //{
        //   var  ret = USB2PWM.PWM_Stop(_devHandle, _pwmConfig.ChannelMask);
        //    if (ret != USB2PWM.PWM_SUCCESS)
        //    {
        //        Console.WriteLine("Start pwm faild!\n");
        //        Console.ReadLine();
        //    }
        //    else
        //    {
        //        Console.WriteLine("Start pwm sunccess!\n");
        //    }
        //}
    }
}
