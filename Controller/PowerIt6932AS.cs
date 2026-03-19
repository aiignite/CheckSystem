using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Threading;
using CommonUtility;
using Controller.HardwareController;

namespace Controller
{
    public sealed class PowerIt6932As : ControllerBase, ICcdPower
    {
        #region Fields
        public MySerialPort MySerialPort;

        [Description("R,输出电压值")]
        public float VoltageRead;

        [Description("R,输出电流值")]
        public float CurrenttRead;

        #endregion

        public PowerIt6932As(string name)
            : base(name) { }

        public void ConnectPower(string protocolValue)
        {
            try
            {
                if (protocolValue.StartsWith("COM"))
                {
                    var port = protocolValue.Split(':')[0];
                    var baudTate = protocolValue.Split(':')[1];
                    MySerialPort =
                        new MySerialPort(port, Convert.ToInt32(baudTate), Parity.None, 8, StopBits.One);
                    MySerialPort.MyOpen();
                }
                else
                {
                    var split = protocolValue.Split(':');
                    var ipAddressStr = split[0];
                    var port = Convert.ToInt32(split[1]);

                    MySerialPort = new MySerialPort(ipAddressStr, port);
                    MySerialPort.MyOpen();
                }

            }
            catch (Exception ex)
            {
                OnPushControllerMsg(ex.Message);
            }
        }

        /// <summary>
        /// 打开通道输出
        /// </summary>
        [Description("打开通道输出")]
        public void PowerOn()
        {
            RemoteControl();
            const string setRemoteCmd = "OUTPut 1\r\n";
            if (MySerialPort != null)
                MySerialPort.SendCommand(setRemoteCmd + "\r\n");

            Thread.Sleep(50);
        }

        /// <summary>
        /// 关闭通道输出
        /// </summary>
        [Description("关闭通道输出")]
        public void PowerOff()
        {
            RemoteControl();
            const string setRemoteCmd = "OUTPut 0\r\n";
            if (MySerialPort != null)
                MySerialPort.SendCommand(setRemoteCmd + "\r\n");

            Thread.Sleep(50);
        }

        public void SetCombSerOn()
        {

        }

        public void SetCombParaOn()
        {

        }

        public void SetCombOff()
        {

        }

        public void SetVoltage1(float voltage)
        {
            SetVoltage(voltage);
        }

        public void SetVoltage2(float voltage)
        {

        }

        public void SetVoltage3(float voltage)
        {

        }

        public void SetCurrent1(float current)
        {
            SetCurrent(current);
        }

        public void SetCurrent2(float current)
        {

        }

        public void SetCurrent3(float current)
        {
        }

        public void ReadCurrAndVolt() => ReadCurrent();

        /// <summary>
        /// 设置通道的电压值
        /// </summary>
        /// <param name="voltage"></param>
        [Description("设置通道的电压值")]
        public void SetVoltage(float voltage)
        {
            RemoteControl();

            const string setVoltageCmd = "VOLTage ";
            if (MySerialPort != null)
                MySerialPort.SendCommand(setVoltageCmd + voltage + "\r\n");
            Thread.Sleep(50);
        }

        /// <summary>
        /// 设置通道的电流值
        /// </summary>
        /// <param name="current"></param>
        [Description("设置通道的电流值")]
        public void SetCurrent(float current)
        {
            RemoteControl();

            const string setVoltageCmd = "CURRent ";
            if (MySerialPort != null)
                MySerialPort.SendCommand(setVoltageCmd + current + "\r\n");
            Thread.Sleep(50);
        }

        /// <summary>
        /// 读取通道的输出电压值
        /// </summary>
        [Description("读取通道的输出电压值")]
        public void ReadVoltage()
        {
            RemoteControl();

            VoltageRead = -9999;
            const string getCmd = "MEASure:VOLTage?\r\n";

            try
            {
                if (MySerialPort == null)
                    return;

                MySerialPort.SendCommand(getCmd);

                VoltageRead = Convert.ToSingle(MySerialPort.ReadDataStr().TrimEnd());
            }
            catch (Exception)
            {
                VoltageRead = -9999;
            }
        }

        /// <summary>
        /// 读取通道的输出电流值
        /// </summary>
        [Description("读取通道的输出电流值")]
        public void ReadCurrent()
        {
            RemoteControl();

            CurrenttRead = -9999;
            const string getCmd = "MEASure:CURRent?\r\n";

            try
            {
                if (MySerialPort == null)
                    return;

                MySerialPort.SendCommand(getCmd);

                CurrenttRead = 1000 * Convert.ToSingle(MySerialPort.ReadDataStr().TrimEnd());
            }
            catch (Exception)
            {
                CurrenttRead = -9999;
            }
        }

        /// <summary>
        /// 读取通道的输出电流和电压值
        /// </summary>
        [Description("读取通道的输出电流和电压值")]
        public void ReadCurrentAndVoltage()
        {
            ReadCurrent();
            ReadVoltage();
        }

        /// <summary>
        /// 本地连接
        /// </summary>
        [Description("本地连接")]
        public void LocalControll()
        {
            const string setRemoteCmd = "SYST:LOC\r\n";
            if (MySerialPort != null)
                MySerialPort.SendCommand(setRemoteCmd + "\r\n");

            Thread.Sleep(50);
        }

        /// <summary>
        /// 远程连接
        /// </summary>
        private void RemoteControl()
        {
            const string setRemoteCmd = "SYST:REM\r\n";
            if (MySerialPort != null)
                MySerialPort.SendCommand(setRemoteCmd + "\r\n");

            Thread.Sleep(50);
        }
    }
}
