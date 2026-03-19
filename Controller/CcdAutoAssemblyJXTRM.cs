using CommonUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Threading;

namespace Controller
{
    public class CcdAutoAssemblyJXTRM : ControllerBase
    {
        #region Fields
        public MySerialPort MySerialPort;

        [Description("R,输出电压值")]
        public float VoltageRead;

        [Description("R,输出电流值")]
        public float CurrenttRead;


        #endregion


        public double R1 = -999;
        public double R2 = -999;
        public double R3 = -999;
        public double R4 = -999;
        public double R5 = -999;
        public double R6 = -999;
        public double R7 = -999;
        public double R8 = -999;
        public double R9 = -999;
        public double R10 = -999;
        public double R11 = -999;
        public double R12 = -999;
        public double R13 = -999;
        public double R14 = -999;
        public double R15 = -999;
        public double R16 = -999;
        public double R17 = -999;
        public double R18 = -999;
        public double R19 = -999;
        public double R20 = -999;
        public double R21 = -999;
        public double R22 = -999;
        public double R23 = -999;
        public double R24 = -999;
        public double R25 = -999;
        public double R26 = -999;
        public double R27 = -999;
        public double R28 = -999;
        public double R29 = -999;
        public double R30 = -999;
        public double R31 = -999;
        public double R32 = -999;
        public double R33 = -999;
        public double R34 = -999;
        public double R35 = -999;
        public double R36 = -999;
        public double R37 = -999;
        public double R38 = -999;
        public double R39 = -999;
        public double R40 = -999;


        public double RR1 = -999;
        public double RR2 = -999;
        public double RR3 = -999;
        public double RR4 = -999;
        public double RR5 = -999;
        public double RR6 = -999;
        public double RR7 = -999;
        public double RR8 = -999;
        public double RR9 = -999;
        public double RR10 = -999;
        public double RR11 = -999;
        public double RR12 = -999;
        public double RR13 = -999;
        public double RR14 = -999;
        public double RR15 = -999;
        public double RR16 = -999;
        public double RR17 = -999;
        public double RR18 = -999;
        public double RR19 = -999;
        public double RR20 = -999;
        public double RR21 = -999;
        public double RR22 = -999;
        public double RR23 = -999;
        public double RR24 = -999;
        public double RR25 = -999;
        public double RR26 = -999;
        public double RR27 = -999;
        public double RR28 = -999;
        public double RR29 = -999;
        public double RR30 = -999;
        public double RR31 = -999;
        public double RR32 = -999;
        public double RR33 = -999;
        public double RR34 = -999;
        public double RR35 = -999;
        public double RR36 = -999;
        public double RR37 = -999;
        public double RR38 = -999;
        public double RR39 = -999;
        public double RR40 = -999;

        public void ConnectJX(string protocolValue)
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

        public bool enableReady;

        /// <summary>
        /// 上使能
        /// </summary>
        [Description("上使能")]
        public void Enable()
        {
            enableReady = false;
            double[] R = new double[60 / 2];
            List<byte> sendByte = new List<byte>();
            sendByte.Add(0x01);
            sendByte.Add(0x06);
            sendByte.Add(0x03);
            sendByte.Add(0xE9);
            sendByte.Add(0x00);
            sendByte.Add(0x01);
            sendByte.Add(0x99);
            sendByte.Add(0xBA);
            if (MySerialPort != null)
                MySerialPort.SendCommand(sendByte.ToArray());
            Thread.Sleep(1000);
            var OutputRRead = MySerialPort.ReadDataBytes();
            if (OutputRRead.Length > 0)
            {
                enableReady = true;
            }
            else
            {
                if (MySerialPort != null)
                    MySerialPort.SendCommand(sendByte.ToArray());
                Thread.Sleep(2000);
                OutputRRead = MySerialPort.ReadDataBytes();

                if (OutputRRead.Length > 0)
                {
                    enableReady = true;
                }
                else
                {
                    if (MySerialPort != null)
                        MySerialPort.SendCommand(sendByte.ToArray());
                    Thread.Sleep(4000);
                    OutputRRead = MySerialPort.ReadDataBytes();

                    if (OutputRRead.Length > 0)
                    {
                        enableReady = true;
                    }
                    else
                    {
                        if (MySerialPort != null)
                            MySerialPort.SendCommand(sendByte.ToArray());
                        Thread.Sleep(4000);
                        OutputRRead = MySerialPort.ReadDataBytes();

                        if (OutputRRead.Length > 0)
                        {
                            enableReady = true;
                        }
                        else
                        {
                            if (MySerialPort != null)
                                MySerialPort.SendCommand(sendByte.ToArray());
                            Thread.Sleep(4000);
                            OutputRRead = MySerialPort.ReadDataBytes();
                            if (OutputRRead.Length > 0)
                            {
                                enableReady = true;
                            }
                            else
                            {
                                for (int i = 0; i < 60 / 2; i++)
                                {
                                    R[i] = -999;
                                    var dOut = GetType().GetField(string.Format("R{0}", i + 1));
                                    dOut.SetValue(this, R[i]);
                                }
                            }
                        }
                    }
                }

            }
        }

        public bool enableReady2;
        /// <summary>
        /// 上使能
        /// </summary>
        [Description("上使能")]
        public void Enable2()
        {
            enableReady2 = false;
            double[] R = new double[60 / 2];
            List<byte> sendByte = new List<byte>();
            sendByte.Add(0x02);
            sendByte.Add(0x06);
            sendByte.Add(0x03);
            sendByte.Add(0xE9);
            sendByte.Add(0x00);
            sendByte.Add(0x01);
            sendByte.Add(0x99);
            sendByte.Add(0x89);

            if (MySerialPort != null)
                MySerialPort.SendCommand(sendByte.ToArray());
            Thread.Sleep(1000);
            var OutputRRead = MySerialPort.ReadDataBytes();

            if (OutputRRead.Length > 0)
            {
                enableReady2 = true;
            }
            else
            {
                if (MySerialPort != null)
                    MySerialPort.SendCommand(sendByte.ToArray());
                Thread.Sleep(2000);
                OutputRRead = MySerialPort.ReadDataBytes();

                if (OutputRRead.Length > 0)
                {
                    enableReady2 = true;
                }
                else
                {

                    if (MySerialPort != null)
                        MySerialPort.SendCommand(sendByte.ToArray());
                    Thread.Sleep(4000);
                    OutputRRead = MySerialPort.ReadDataBytes();

                    if (OutputRRead.Length > 0)
                    {
                        enableReady2 = true;
                    }
                    else
                    {

                        if (MySerialPort != null)
                            MySerialPort.SendCommand(sendByte.ToArray());
                        Thread.Sleep(4000);
                        OutputRRead = MySerialPort.ReadDataBytes();

                        if (OutputRRead.Length > 0)
                        {
                            enableReady2 = true;
                        }
                        else
                        {
                            if (MySerialPort != null)
                                MySerialPort.SendCommand(sendByte.ToArray());
                            Thread.Sleep(4000);
                            OutputRRead = MySerialPort.ReadDataBytes();

                            if (OutputRRead.Length > 0)
                            {
                                enableReady2 = true;
                            }
                            else
                            {
                                for (int i = 0; i < 60 / 2; i++)
                                {
                                    R[i] = -999;
                                    var dOut = GetType().GetField(string.Format("R{0}", i + 1));
                                    dOut.SetValue(this, R[i]);
                                }
                            }
                        }

                    }
                }
            }
        }

        /// <summary>
        /// 采集
        /// </summary>
        /// <returns></returns>
        [Description("采集")]
        public void CaiJi()
        {
            List<byte> sendByte = new List<byte>();
            sendByte.Add(0x01);
            sendByte.Add(0x03);
            sendByte.Add(0x03);
            sendByte.Add(0xE8);
            sendByte.Add(0x00);
            sendByte.Add(0x01);
            sendByte.Add(0x04);
            sendByte.Add(0x7A);
            if (MySerialPort != null)
                MySerialPort.SendCommand(sendByte.ToArray());
            Thread.Sleep(3000);

            var OutputRRead = MySerialPort.ReadDataBytes();

            if (OutputRRead.Length > 0)
            {
                enableReady = true;
            }
            else
            {
                if (MySerialPort != null)
                    MySerialPort.SendCommand(sendByte.ToArray());
                Thread.Sleep(2000);
            }
        }

        /// <summary>
        /// 读电阻
        /// </summary>
        /// <returns></returns>
        [Description("读电阻")]
        public void GetReciveRValue(int count)
        {
            try
            {
                string[] rcvMsg = new string[count / 2];
                double[] R = new double[count / 2];
                byte[] pack = new byte[2];
                byte[] byte_i = BitConverter.GetBytes(count);//此函数将十进制转为四个字节的十六进制
                pack[0] = byte_i[1];
                pack[1] = byte_i[0];
                List<byte> sendByte = new List<byte>();
                sendByte.Add(0x01);
                sendByte.Add(0x03);
                sendByte.Add(0x17);
                sendByte.Add(0x71);
                sendByte.Add(pack[0]);
                sendByte.Add(pack[1]);
                sendByte.AddRange(ValueHelper.Crc16(sendByte));
                if (MySerialPort != null)
                    MySerialPort.SendCommand(sendByte.ToArray());
                var OutputRRead = MySerialPort.ReadDataBytes();
                if (OutputRRead.Length > 0)
                {
                    for (int i = 0; i < count / 2; i++)
                    {
                        double value;
                        int a = OutputRRead[5 + 4 * i] * 256 + OutputRRead[6 + 4 * i];
                        value = a;
                        R[i] = value;
                        var dOut = GetType().GetField(string.Format("R{0}", i + 1));
                        dOut.SetValue(this, R[i]);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 读电阻
        /// </summary>
        /// <returns></returns>
        [Description("读电阻")]
        public void GetReciveRValue2(int count)
        {

            try
            {
                string[] rcvMsg = new string[count / 2];
                double[] R = new double[count / 2];

                byte[] pack = new byte[2];
                byte[] byte_i = BitConverter.GetBytes(count);//此函数将十进制转为四个字节的十六进制
                pack[0] = byte_i[1];
                pack[1] = byte_i[0];
                List<byte> sendByte = new List<byte>();
                sendByte.Add(0x02);
                sendByte.Add(0x03);
                sendByte.Add(0x17);
                //sendByte.Add(0x70);
                sendByte.Add(0x71);
                sendByte.Add(pack[0]);
                sendByte.Add(pack[1]);
                sendByte.AddRange(ValueHelper.Crc16(sendByte));
                if (MySerialPort != null)
                    MySerialPort.SendCommand(sendByte.ToArray());
                var OutputRRead = MySerialPort.ReadDataBytes();

                if (OutputRRead.Length > 0)
                {
                    for (int i = 0; i < count / 2; i++)
                    {
                        double value;
                        int a = OutputRRead[5 + 4 * i] * 256 + OutputRRead[6 + 4 * i];
                        value = a;
                        R[i] = value;
                        var dOut = GetType().GetField(string.Format("RR{0}", i + 1));
                        dOut.SetValue(this, R[i]);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 根据字符串指令创建byte集合
        /// </summary>
        /// <param name="sendStr"></param>
        /// <returns></returns>
        private static List<byte> CreatByteList(string sendStr)
        {
            List<char> charList = new List<char>(sendStr.ToCharArray());
            List<byte> sendByte = charList.Select(t => (byte)t).ToList();
            sendByte.AddRange(new byte[] { 0x0D, 0x0A });
            return sendByte;
        }

        public CcdAutoAssemblyJXTRM(string name)
            : base(name)
        {

        }

        ~CcdAutoAssemblyJXTRM()
        {
            MySerialPort?.Close();
            Dispose();
        }
    }
}
