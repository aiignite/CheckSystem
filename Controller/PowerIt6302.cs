using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Threading;
using CommonUtility;
using Controller.HardwareController;

namespace Controller
{
    public sealed class PowerIt6302 : ControllerBase, ICcdPower
    {
        #region Fields
        public MySerialPort MySerialPort;

        [Description("R,通道1输出电压值")]
        public float VoltageRead1;

        [Description("R,通道2输出电压值")]
        public float VoltageRead2;

        [Description("R,通道3输出电压值")]
        public float VoltageRead3;

        [Description("R,通道1输出电流值")]
        public float CurreatRead1;

        [Description("R,通道1输出电流值")]
        public float CurreatRead2;

        [Description("R,通道1输出电流值")]
        public float CurreatRead3;
        #endregion

        public PowerIt6302(string name) :
            base(name) { }

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

        public void PowerOn() => OpenAllChannels();

        public void PowerOff()=> CloseAllChannels();

        /// <summary>
        /// 打开所有通道输出
        /// </summary>
        [Description("打开所有通道输出")]
        public void OpenAllChannels()=> SetOperateAllChannels(1);

        /// <summary>
        /// 关闭所有通道输出
        /// </summary>
        [Description("关闭所有通道输出")]
        public void CloseAllChannels()=> SetOperateAllChannels(0);

        /// <summary>
        /// 将电源的通道 1 和通道 2 设置为串联状态
        /// </summary>
        [Description("将电源的通道 1 和通道 2 设置为串联状态")]
        public void SetCombSerOn()
        {
            RemoteControl();
            CloseAllChannels();
            SetCombParaOrSer(false);
        }

        /// <summary>
        /// 将电源的通道 1 和通道 2 设置为并联状态
        /// </summary>
        [Description("将电源的通道 1 和通道 2 设置为并联状态")]
        public void SetCombParaOn()
        {
            RemoteControl();
            CloseAllChannels();
            SetCombParaOrSer(true);
        }

        /// <summary>
        /// 解除电源的通道 1 和通道 2 的串并联状态
        /// </summary>
        [Description("解除电源的通道 1 和通道 2 的串并联状态")]
        public void SetCombOff()
        {
            RemoteControl();

            CloseAllChannels();

            if (MySerialPort != null)
                MySerialPort.SendCommand("INSTrument:COMbine:OFF\r\n");

            Thread.Sleep(50);
        }

        public void OpenSingleChannel(string channelIndex)
        {
            var index = int.Parse(channelIndex);
            switch (index)
            {
                case 1:
                    SetOperateChannel1(1);
                    break;

                case 2:
                    SetOperateChannel2(1);
                    break;

                case 3:
                    SetOperateChannel3(1);
                    break;

                default:
                    return;
            }
        }

        public void CloseSingleChannel(string channelIndex)
        {
            var index = int.Parse(channelIndex);
            switch (index)
            {
                case 1:
                    SetOperateChannel1(0);
                    break;

                case 2:
                    SetOperateChannel2(0);
                    break;

                case 3:
                    SetOperateChannel3(0);
                    break;

                default:
                    return;
            }
        }

        /// <summary>
        /// 设置通道1-3的电压值
        /// </summary>
        /// <param name="voltage">电压值</param>
        [Description("设置通道1-3的电压值")]
        public void SetVoltageAll(float voltage)
        {
            RemoteControl();

            // ch1
            SelectChannel(1);
            SetVoltageValue(voltage);

            // ch2
            SelectChannel(2);
            SetVoltageValue(voltage);

            // ch3
            SelectChannel(3);
            SetVoltageValue(voltage);
        }

        /// <summary>
        /// 设置通道1的电压值
        /// </summary>
        /// <param name="voltage"></param>
        [Description("设置通道1的电压值")]
        public void SetVoltage1(float voltage)
        {
            RemoteControl();

            // ch1
            SelectChannel(1);
            SetVoltageValue(voltage);
        }

        /// <summary>
        /// 设置通道2的电压值
        /// </summary>
        /// <param name="voltage"></param>
        [Description("设置通道2的电压值")]
        public void SetVoltage2(float voltage)
        {
            RemoteControl();

            // ch2
            SelectChannel(2);
            SetVoltageValue(voltage);
        }

        /// <summary>
        /// 设置通道3的电压值
        /// </summary>
        /// <param name="voltage"></param>
        [Description("设置通道3的电压值")]
        public void SetVoltage3(float voltage)
        {
            RemoteControl();

            // ch3
            SelectChannel(3);
            SetVoltageValue(voltage);
        }

        /// <summary>
        /// 设置通道1-3的电流值
        /// </summary>
        /// <param name="current">电流值</param>
        public void SetCurrentAll(float current)
        {
            RemoteControl();

            // ch1
            SelectChannel(1);
            SetCurrentValue(current);

            // ch2
            SelectChannel(2);
            SetCurrentValue(current);

            // ch3
            SelectChannel(3);
            SetCurrentValue(current);
        }

        /// <summary>
        /// 设置通道1的电流值
        /// </summary>
        /// <param name="current"></param>
        [Description("设置通道1的电流值")]
        public void SetCurrent1(float current)
        {
            RemoteControl();

            // ch1
            SelectChannel(1);
            SetCurrentValue(current);
        }

        /// <summary>
        /// 设置通道2的电流值
        /// </summary>
        /// <param name="current"></param>
        [Description("设置通道2的电流值")]
        public void SetCurrent2(float current)
        {
            RemoteControl();

            // ch2
            SelectChannel(2);
            SetCurrentValue(current);
        }

        /// <summary>
        /// 设置通道3的电流值
        /// </summary>
        /// <param name="current"></param>
        [Description("设置通道3的电流值")]
        public void SetCurrent3(float current)
        {
            RemoteControl();

            // ch3
            SelectChannel(3);
            SetCurrentValue(current);
        }

        public void ReadCurrAndVolt() => UpdateCurrentsAndVoltages();

        /// <summary>
        /// 设置通道1-3打开或者关闭
        /// </summary>
        /// <param name="flag">1=开，0=关</param>
        private void SetOperateAllChannels(int flag)
        {
            RemoteControl();

            // ch all
            SetOutput(0, flag);
        }

        /// <summary>
        /// 设置通道1打开或者关闭
        /// </summary>
        /// <param name="flag">1=开，0=关</param>
        private void SetOperateChannel1(int flag)
        {
            RemoteControl();

            // ch1
            SelectChannel(1);
            SetOutput(1, flag);
        }

        /// <summary>
        /// 设置通道2打开或者关闭
        /// </summary>
        /// <param name="flag">1=开，0=关</param>
        private void SetOperateChannel2(int flag)
        {
            RemoteControl();

            // ch2
            SelectChannel(2);
            SetOutput(2, flag);
        }

        /// <summary>
        /// 设置通道3打开或者关闭
        /// </summary>
        /// <param name="flag">1=开，0=关</param>
        private void SetOperateChannel3(int flag)
        {
            RemoteControl();

            // ch3
            SelectChannel(3);
            SetOutput(3, flag);
        }

        /// <summary>
        /// 更新通道1-3当前的输出电压值和电流值
        /// </summary>
        [Description("更新通道1-3当前的输出电压值和电流值")]
        public void UpdateCurrentsAndVoltages()
        {
            RemoteControl();

            try
            {
                var getAllVoltages = GetAllVoltages();
                VoltageRead1 = getAllVoltages[0];
                VoltageRead2 = getAllVoltages[1];
                VoltageRead3 = getAllVoltages[2];
            }
            catch (Exception)
            {
                VoltageRead1 = -9999;
                VoltageRead2 = -9999;
                VoltageRead3 = -9999;
            }

            try
            {
                var getAllCurrents = GetAllCurrents();
                CurreatRead1 = getAllCurrents[0];
                CurreatRead2 = getAllCurrents[1];
                CurreatRead3 = getAllCurrents[2];
            }
            catch (Exception)
            {
                CurreatRead1 = -9999;
                CurreatRead2 = -9999;
                CurreatRead3 = -9999;
            }
        }

        /// <summary>
        /// 仅更新通道1当前的输出电压值和电流值
        /// </summary>
        [Description("仅更新通道1当前的输出电压值和电流值")]
        public void UpdateCh1CurrentAndVoltage()
        {
            RemoteControl();

            const string getAllCurrentsCmd = "MEAS:CURR?\r\n";

            var r = string.Empty;
            if (MySerialPort != null)
            {
                MySerialPort.SendCommand(getAllCurrentsCmd);
                r = MySerialPort.ReadDataStr();
            }

            if (string.IsNullOrEmpty(r))
                CurreatRead1 = -9999;
            else
            {
                try
                {
                    if (Math.Abs(Convert.ToSingle(r)) < 0.1)
                    {
                        CurreatRead1 = (float)0.1;
                    }
                    else
                    {
                        var floatValue = Convert.ToSingle(r);
                        CurreatRead1 = floatValue * 1000;
                    }
                }
                catch (Exception)
                {
                    CurreatRead1 = -9999;
                }
            }
        }

        /// <summary>
        /// 更新通道1-3当前的输出电压值
        /// </summary>
        public void UpdateVoltages()
        {
            RemoteControl();

            var fl = GetAllVoltages();
            VoltageRead1 = fl[0];
            VoltageRead2 = fl[1];
            VoltageRead3 = fl[2];
        }

        /// <summary>
        /// 更新通道1-3当前的输出电流值
        /// </summary>
        public void UpdateCurrents()
        {
            RemoteControl();

            var fl = GetAllCurrents();
            CurreatRead1 = fl[0];
            CurreatRead2 = fl[1];
            CurreatRead3 = fl[2];
        }

        /// <summary>
        /// 远程连接
        /// </summary>
        private void RemoteControl()
        {
            var n = Name;
            const string setRemoteCmd = "SYST:REM\r\n";
            if (MySerialPort != null)
                MySerialPort.SendCommand(setRemoteCmd + "\r\n");

            Thread.Sleep(50);
        }

        /// <summary>
        /// 选择通道
        /// </summary>
        /// <param name="chId">通道号，1-3</param>
        private void SelectChannel(int chId)
        {
            const string selectChCmd = "INSTrument CH";
            if (MySerialPort != null)
                MySerialPort.SendCommand(selectChCmd + chId + "\r\n");

            Thread.Sleep(50);
        }

        /// <summary>
        /// 设置电压值
        /// </summary>
        /// <param name="voltage">电压值</param>
        private void SetVoltageValue(float voltage)
        {
            const string setVoltageCmd = "VOLT ";
            if (MySerialPort != null)
                MySerialPort.SendCommand(setVoltageCmd + voltage + "\r\n");
            Thread.Sleep(50);
        }

        /// <summary>
        /// 设置电流值
        /// </summary>
        /// <param name="current">电流值</param>
        private void SetCurrentValue(float current)
        {
            const string setCurrentCmd = "CURR ";
            if (MySerialPort != null)
                MySerialPort.SendCommand(setCurrentCmd + current + "\r\n");
            Thread.Sleep(50);
        }

        /// <summary>
        /// 设置并联或者串联
        /// </summary>
        /// <param name="isPara">true为并联状态，false为串联状态</param>
        private void SetCombParaOrSer(bool isPara)
        {
            if (MySerialPort != null)
                MySerialPort.SendCommand(
                    isPara ? "INSTrument:COMbine:PARAllel\r\n" : "INSTrument:COMbine:SERies\r\n");
            Thread.Sleep(50);
        }

        /// <summary>
        /// 获取当前选择通道的电压值
        /// </summary>
        /// <returns></returns>
        private float GetVoltage()
        {
            const string getVoltageCmd = "MEAS:VOLT?\r\n";
            if (MySerialPort != null)
                MySerialPort.SendCommand(getVoltageCmd);

            var r = string.Empty;
            if (MySerialPort != null)
                MySerialPort.ReadDataStr();
            if (string.IsNullOrEmpty(r))
            {
                return -9999;
            }

            try
            {
                return Convert.ToSingle(r);
            }
            catch (Exception)
            {
                return -9999;
            }
        }

        /// <summary>
        /// 获取通道1-3的电压值
        /// </summary>
        /// <returns></returns>
        private float[] GetAllVoltages()
        {
            const string getAllVoltagesCmd = "MEAS:VOLT? ALL\r\n";
            if (MySerialPort != null)
            {
                MySerialPort.SendCommand(getAllVoltagesCmd);

                var floats = ConvertStr(MySerialPort.ReadDataStr()).ToArray();
                return floats;
            }

            return null;
        }

        /// <summary>
        /// 获取当前选择通道的电流值
        /// </summary>
        /// <returns></returns>
        private float GetCurrent()
        {
            const string getCurrentCmd = "MEAS:CURR?\r\n";
            if (MySerialPort != null)
                MySerialPort.SendCommand(getCurrentCmd);

            var r = string.Empty;

            if (MySerialPort != null)
                MySerialPort.ReadDataStr();

            if (string.IsNullOrEmpty(r))
                return -9999;

            try
            {
                return Convert.ToSingle(r) * 1000;
            }
            catch (Exception)
            {

                return -9999;
            }
        }

        /// <summary>
        /// 获取通道1-3的电流值
        /// </summary>
        /// <returns></returns>
        private float[] GetAllCurrents()
        {
            const string getAllCurrentsCmd = "MEAS:CURR? ALL\r\n";

            if (MySerialPort != null)
                MySerialPort.SendCommand(getAllCurrentsCmd);

            float[] floats;
            if (MySerialPort != null)
            {
                floats = ConvertStr(MySerialPort.ReadDataStr()).ToArray();
                for (var i = 0; i < floats.Length; i++)
                    floats[i] = Math.Abs(floats[i] - -9999) < 0.01 ? -9999 : floats[i] * 1000;
                return floats;
            }

            return null;
        }

        /// <summary>
        /// 设置通道输出
        /// </summary>
        /// <param name="chNum">通道号，0表示全部</param>
        /// <param name="flag">开启1、关闭0</param>
        private void SetOutput(int chNum, int flag)
        {
            if (chNum == 0)
            {
                if (MySerialPort != null)
                    MySerialPort.SendCommand(string.Format("OUTPUT {0}\r\n", flag));
            }
            else
            {
                const string setOperateSpeciicChannelCmd = "CHANNEL:OUTPUT ";
                if (MySerialPort != null)
                    MySerialPort.SendCommand(setOperateSpeciicChannelCmd + flag + "\r\n");
            }

            Thread.Sleep(50);
        }

        private static List<float> ConvertStr(string str)
        {
            //if (splits.Length != 3)
            //    throw new Exception("");

            var returnFloats = new List<float> { -9999, -9999, -9999 };

            if (string.IsNullOrEmpty(str))
                return returnFloats;

            try
            {
                var splits = str.Trim(',').Split(',');

                var len = splits.Length;
                switch (len)
                {
                    case 1:
                        returnFloats[0] = Convert.ToSingle(splits[0].Trim(' '));
                        break;

                    case 2:
                        returnFloats[0] = Convert.ToSingle(splits[0].Trim(' '));
                        returnFloats[1] = Convert.ToSingle(splits[1].Trim(' '));
                        break;

                    case 3:
                        returnFloats[0] = Convert.ToSingle(splits[0].Trim(' '));
                        returnFloats[1] = Convert.ToSingle(splits[1].Trim(' '));
                        returnFloats[2] = Convert.ToSingle(splits[2].Trim(' '));
                        break;
                }

                for (var i = 0; i < returnFloats.Count; i++)
                    returnFloats[i] = (float)Math.Round(returnFloats[i], 4, MidpointRounding.AwayFromZero);
            }
            catch (Exception)
            {
                return returnFloats;
            }

            //var returnFloats = splits.Select(t => t.Trim(' ')).Select(Convert.ToSingle).ToList();

            return returnFloats;
        }
    }
}