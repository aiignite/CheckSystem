using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonUtility;
using CommonUtility.BusLoader;

namespace Controller
{
    public sealed class ToomossUsb2XxxSlaveLin : ControllerBase
    {
        public ToomossUsbSlaveLinChannel SlaveLin1;
        public ToomossUsbSlaveLinChannel SlaveLin2;

        private readonly UsbDevice.DeviceInfo _devInfo;
        private readonly int[] _devHandles = new int[20];
        public readonly int _devHandle;
        public readonly bool State;

        public ToomossUsb2XxxSlaveLin(string name)
            : base(name)
        {
            // 扫描查找设备
            var devNum = UsbDevice.USB_ScanDevice(_devHandles);
            if (devNum <= 0)
            {
                Console.WriteLine("No device connected!");
                return;
            }

            Console.WriteLine("Have {0} device connected!", devNum);
            _devHandle = _devHandles[0];

            //打开设备
            State = UsbDevice.USB_OpenDevice(_devHandle);
            if (!State)
            {
                Console.WriteLine("Open device error!");
                return;
            }
            Console.WriteLine("Open device success!");

            // 获取固件信息
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

            // 初始化配置LIN1
            var ret1 = Usb2LinEx.LIN_EX_Init(_devHandle, 0, 10417, 0); // 初始化为从机
            if (ret1 == Usb2LinEx.LinExSuccess)
            {
                //SlaveLin1 = this;
                SlaveLin1 = new ToomossUsbSlaveLinChannel(this, name + "_slvaeLin", 0);
            }

            // 初始化配置LIN2
            var ret2 = Usb2LinEx.LIN_EX_Init(_devHandle, 1, 10417, 0); // 初始化为从机
            if (ret2 == Usb2LinEx.LinExSuccess)
            {
                //SlaveLin2 = this;
                SlaveLin2 = new ToomossUsbSlaveLinChannel(this, name + "_slvaeLin", 1);
            }
        }

        public void SetBaudRate(string linIndex, string baudRate)
        {
            if (baudRate != "9600" && baudRate != "10417" && baudRate != "10400" &&
                baudRate != "14400" && baudRate != "56000" &&
                baudRate != "19200")
                return;

            if (linIndex != "1" && linIndex != "2")
                return;

            if (linIndex == "1")
            {
                // 初始化配置LIN1
                var lin1Ret = Usb2LinEx.LIN_EX_Init(_devHandle, 0, int.Parse(baudRate), 0); //初始化为主机
                if (lin1Ret == Usb2LinEx.LinExSuccess)
                {
                    SlaveLin1 = new ToomossUsbSlaveLinChannel(this, Name + "_slvaeLin", 0);
                }
            }
            else if (linIndex == "2")
            {
                // 初始化配置LIN2
                var linwRet = Usb2LinEx.LIN_EX_Init(_devHandle, 1, int.Parse(baudRate), 0); //初始化为主机
                if (linwRet == Usb2LinEx.LinExSuccess)
                {
                    SlaveLin2 = new ToomossUsbSlaveLinChannel(this, Name + "_slvaeLin", 1);
                }
            }
        }

        private Usb2LinEx.LinExMsg[] _linSlave1Msg;
        private Usb2LinEx.LinExMsg[] _linSlave2Msg;

        private void ConfigLinMsgCount(int linIndex, int count)
        {
            var slaveLinMsg = new Usb2LinEx.LinExMsg[count];
            for (var j = 0; j < slaveLinMsg.Length; j++)
            {
                //配置第一帧数据
                slaveLinMsg[j] = new Usb2LinEx.LinExMsg
                {
                    MsgType = Usb2LinEx.LinExMsgTypeSw, // 从机发送数据模式
                    CheckType = Usb2LinEx.LinExCheckExt, // 配置为增强校验
                    PID = 0xFF, // 可以只传入ID，校验位底层会自动计算
                    Data = new byte[8], // 必须分配8字节空间
                    DataLen = 8
                };

                // 实际要发送的数据字节数
                var dataBuffer = new byte[8]; // 从机返回的数据，根据自己实际情况修改数据

                for (var i = 0; i < slaveLinMsg[j].DataLen; i++) // 循环填充8字节数据
                    slaveLinMsg[j].Data[i] = dataBuffer[i];
            }

            if (linIndex == 0)
            {
                _linSlave1Msg = new Usb2LinEx.LinExMsg[slaveLinMsg.Length];
                Array.Copy(slaveLinMsg, _linSlave1Msg, slaveLinMsg.Length);
                if (Usb2LinEx.LIN_EX_SlaveSetIDMode(_devHandle, 0, _linSlave1Msg, _linSlave1Msg.Length) != Usb2LinEx.LinExSuccess)
                {
                }
            }
            else if (linIndex == 1)
            {
                _linSlave2Msg = new Usb2LinEx.LinExMsg[slaveLinMsg.Length];
                Array.Copy(slaveLinMsg, _linSlave2Msg, slaveLinMsg.Length);
                if (Usb2LinEx.LIN_EX_SlaveSetIDMode(_devHandle, 1, _linSlave2Msg, _linSlave2Msg.Length) != Usb2LinEx.LinExSuccess)
                {
                }
            }
        }

        private void UpdateLinMsg(int linIndex, string linId, string value)
        {
            try
            {
                if (linIndex == 0)
                {
                    UpdateLinExMsg(_linSlave1Msg, linId, value);
                    //Usb2LinEx.LIN_EX_Init(_devHandle, 0, 19200, 0);
                    if (Usb2LinEx.LIN_EX_SlaveSetIDMode(_devHandle, 0, _linSlave1Msg, _linSlave1Msg.Length) != Usb2LinEx.LinExSuccess)
                    {

                    }
                }
                else if (linIndex == 1)
                {
                    UpdateLinExMsg(_linSlave2Msg, linId, value);
                    //Usb2LinEx.LIN_EX_Init(_devHandle, 1, 19200, 0);
                    if (Usb2LinEx.LIN_EX_SlaveSetIDMode(_devHandle, 1, _linSlave2Msg, _linSlave2Msg.Length) != Usb2LinEx.LinExSuccess)
                    {

                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private static void UpdateLinExMsg(
            Usb2LinEx.LinExMsg[] linSlaveMsg, string linId, string value)
        {
            try
            {
                var valueBytes = new List<byte>();
                for (var i = 0; i < value.Length; i = i + 2)
                    valueBytes.Add(Convert.ToByte(value.Substring(i, 2), 16));

                var index =
                    linSlaveMsg.ToList()
                        .FindIndex(f => ValueHelper.GetHextStr(f.PID) == linId.Replace("0x", string.Empty));

                if (index == -1)
                {
                    index = linSlaveMsg.ToList()
                       .FindIndex(f => ValueHelper.GetHextStr(f.PID) == "0xFF".Replace("0x", string.Empty));

                    if (index == -1)
                        index = 0;
                }

                linSlaveMsg[index].PID = Convert.ToByte(linId, 16);
                linSlaveMsg[index].DataLen = (byte)valueBytes.Count;

                for (var i = 0; i < valueBytes.Count; i++)
                    linSlaveMsg[index].Data[i] = valueBytes[i];
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public class ToomossUsbSlaveLinChannel
        {
            private readonly ToomossUsb2XxxSlaveLin _toomossUsb2XxxCanLin;
            private readonly int _linIndex;
            private string _name;

            public ToomossUsbSlaveLinChannel(
                ToomossUsb2XxxSlaveLin toomossUsb2XxxSlaveLin, string name, int linIndex)
            {
                _linIndex = linIndex;
                _name = name;
                _toomossUsb2XxxCanLin = toomossUsb2XxxSlaveLin;
            }

            public void ConfigLinMsgCount(int count)
            {
                _toomossUsb2XxxCanLin.ConfigLinMsgCount(_linIndex, count);
            }

            public void UpdateLinExMsg(string linId, string value)
            {
                _toomossUsb2XxxCanLin.UpdateLinMsg(_linIndex, linId, value);
            }
        }
    }
}
