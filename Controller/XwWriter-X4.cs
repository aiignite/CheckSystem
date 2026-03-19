using CommonUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;

namespace Controller
{

    [Description("稀微1拖4烧写器")]
    public class XwWriter_X4 : ControllerBase
    {
        /// <summary>
        /// 烧写文件名称
        /// </summary>
        public string SelectProjectName = string.Empty;
        /// <summary>
        /// 当前接收到的信息
        /// </summary>
        public string CurrentRcvMsg = string.Empty;
        /// <summary>
        /// 当前烧录的信息
        /// </summary>
        public string CurrentBurnMsg = string.Empty;

        /// <summary>
        /// 通道1烧写结果
        /// </summary>
        /// 
        [Description("R,通道1烧写结果")]
        public string BurnResult1 = string.Empty;

        /// <summary>
        /// 通道2烧写结果
        /// </summary>
        /// 
        [Description("R,通道2烧写结果")]
        public string BurnResult2 = string.Empty;

        /// <summary>
        /// 通道3烧写结果
        /// </summary>
        /// 
        [Description("R,通道3烧写结果")]
        public string BurnResult3 = string.Empty;

        /// <summary>
        /// 通道4烧写结果
        /// </summary>
        /// 
        [Description("R,通道4烧写结果")]
        public string BurnResult4 = string.Empty;


        private SerialPort _mySerialPort = null;
        private Thread OperateDataThread;
        private object rcvLock = new object();
        private List<byte> rcvData = new List<byte>();
        private EventWaitHandle WaitHandle_Burn = new EventWaitHandle(false, EventResetMode.AutoReset);
        private EventWaitHandle WaitHandle_SelectProject = new EventWaitHandle(false, EventResetMode.AutoReset);
        private EventWaitHandle WaitHandle_ReadCurrentProject = new EventWaitHandle(false, EventResetMode.AutoReset);
        public XwWriter_X4(string name) : base(name)
        {
            _mySerialPort = new SerialPort();
        }

        public void ConnectXwWriter(string portBaudRate)
        {
            try
            {
                if (portBaudRate.StartsWith("COM"))
                {
                    var port = portBaudRate.Split(':')[0];
                    var baudTate = portBaudRate.Split(':')[1];

                    _mySerialPort.BaudRate = Convert.ToInt32(baudTate);
                    _mySerialPort.PortName = port;
                    _mySerialPort.DataBits = 8;
                    _mySerialPort.Parity = Parity.None;
                    _mySerialPort.StopBits = StopBits.One;
                    _mySerialPort.Open();
                    _mySerialPort.DataReceived += new SerialDataReceivedEventHandler(_mySerialPortDataReceived);

                    OperateDataThread = new Thread(OperiteRcvData);
                    OperateDataThread.IsBackground = true;
                    OperateDataThread.Start();
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                OnPushControllerMsg(ex.Message);
            }
        }

        /// <summary>
        /// 选择烧写器内的程序的名称，不带后缀
        /// </summary>
        /// <param name="projectName"></param>
        /// 
        [Description("选择配置文件名称")]
        public void SelectProject(string projectName)
        {
            this.SelectProjectName = string.Empty;
            if (!this._mySerialPort.IsOpen) return;
            //ReadCurrentProject();
            //if (this.ReadProjectName == projectName + ".hxs")
            //{
            //    this.SelectProjectName = this.ReadProjectName;
            //    return;
            //}
            List<byte> conmend = new List<byte>() { 0xC0, 0x06, 0x00, 0x00 };
            byte[] ffff = Encoding.GetEncoding("GB2312").GetBytes(projectName.Trim() + ".ware");
            conmend.AddRange(ffff);
            conmend[1] = Convert.ToByte(conmend.Count + 2);
            conmend.AddRange(ValueHelper.GetCrc16(conmend, conmend.Count));
            SendCommand(conmend);
            //WaitHandle_SelectProject.WaitOne(3000);
            //Thread.Sleep(1500);//选择程序后，烧写器会重启，这里等待烧写器重启
            //ReadCurrentProject();
            //if (this.ReadProjectName == projectName + ".hxs")
            //{
            //    this.SelectProjectName = this.ReadProjectName;
            //    return;
            //}

        }

        /// <summary>
        /// 读取的当前烧写名称
        /// </summary>
        /// 
        [Description("R/W,当前配置文件名称")]
        public string ReadProjectName = string.Empty;
        [Description("读取当前配置文件名称")]
        public void ReadCurrentProject()
        {
            ReadProjectName = string.Empty;
            List<byte> conmend = new List<byte>() { 0xA9, 0x06, 0x00, 0x00 };
            conmend[1] = Convert.ToByte(conmend.Count + 2);
            conmend.AddRange(ValueHelper.GetCrc16(conmend, conmend.Count));
            SendCommand(conmend);
            //WaitHandle_ReadCurrentProject.WaitOne(3000);
            //Stopwatch sw = new Stopwatch();
            //sw.Restart();
            //do
            //{
            //    if (sw.Elapsed.TotalSeconds > 3) break;
            //    Thread.Sleep(50);
            //}
            //while (ReadProjectName == string.Empty);
        }

        public void ReadCurrentProjectWaitResult(int timeOutSecond = 3)
        {
            ReadProjectName = string.Empty;
            List<byte> conmend = new List<byte>() { 0xA9, 0x06, 0x00, 0x00 };
            conmend[1] = Convert.ToByte(conmend.Count + 2);
            conmend.AddRange(ValueHelper.GetCrc16(conmend, conmend.Count));
            SendCommand(conmend);
            //WaitHandle_ReadCurrentProject.WaitOne(3000);
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            do
            {
                if (sw.Elapsed.TotalSeconds > timeOutSecond) break;
                Thread.Sleep(50);
            }
            while (ReadProjectName == string.Empty);
        }

        /// <summary>
        /// 开始烧写
        /// </summary>
        /// <param name="portNums">端口编号(从1开始)</param>
        [Description("开始烧写   1:2:3:4")]
        public void StartBurn(string portNums)
        {
            BurnResult1 = string.Empty;
            BurnResult2 = string.Empty;
            BurnResult3 = string.Empty;
            BurnResult4 = string.Empty;


            byte value = GetUsePortByteValue(portNums);
            //烧写
            List<byte> conmend = new List<byte>() { 0xA8, 0x06, value, 0x00 };
            conmend.AddRange(ValueHelper.GetCrc16(conmend, 4));
            SendCommand(conmend);


            //WaitHandle_Burn.WaitOne(50000);

            //Stopwatch sw = new Stopwatch();
            //sw.Restart();

            //while (BurnResult.Contains("NG"))
            //{
            //    if (sw.Elapsed.TotalSeconds > 15) break;
            //}
            //BurnResult += ":" + this.CurrentBurnMsg;
        }

        private static byte GetUsePortByteValue(string portNums)
        {
            var port = portNums.Split(new char[] { ':', '：' });
            List<int> ports = new List<int>();
            for (int i = 0; i < port.Length; i++)
            {
                int a = 0;
                if (int.TryParse(port[i], out a))
                {
                    ports.Add(a);
                }
            }

            List<bool> usePort = new List<bool>() { false, false, false, false };

            for (int i = 0; i < ports.Count; i++)
            {
                if (ports[i] <= 4)
                {
                    usePort[ports[i] - 1] = true;
                }
            }

            usePort.Reverse();
            int value = 0;
            for (int i = 0; i < usePort.Count; i++)
            {
                value = (value << 1) + (usePort[i] ? 1 : 0);
            }

            return (byte)value;
        }


        string[] _burnResult = new string[4];
        bool[] _isBurnFinish = new bool[4];
        [Description("开始烧写")]
        public void StartBurnWaitResult(string portNums, int timeOutSecond = 15)
        {
            BurnResult1 = string.Empty;
            BurnResult2 = string.Empty;
            BurnResult3 = string.Empty;
            BurnResult4 = string.Empty;
            _isBurnFinish = new bool[4];
            _burnResult = new string[] { "", "", "", "" };
            byte value = GetUsePortByteValue(portNums);
            //烧写
            List<byte> conmend = new List<byte>() { 0xA8, 0x06, value, 0x00 };
            conmend.AddRange(ValueHelper.GetCrc16(conmend, 4));
            SendCommand(conmend);
            //WaitHandle_Burn.WaitOne(50000);

            Stopwatch sw = new Stopwatch();
            sw.Restart();

            while (true)
            {
                if (sw.Elapsed.TotalSeconds > timeOutSecond) break;
                Thread.Sleep(10);
                int finishCount = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (!Convert.ToBoolean(value & Convert.ToInt16(Math.Pow(2, i)))) _isBurnFinish[i] = true;
                    if (_isBurnFinish[i]) finishCount++;
                }

                if (finishCount == 4) break;
            }

            BurnResult1 = _burnResult[0];
            BurnResult2 = _burnResult[1];
            BurnResult3 = _burnResult[2];
            BurnResult4 = _burnResult[3];
        }

        private void _mySerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort serialPort = sender as SerialPort;
            int byteCount = serialPort.BytesToRead;
            byte[] rcv = new byte[byteCount];
            serialPort.Read(rcv, 0, byteCount);
            lock (rcvLock)
            {
                rcvData.AddRange(rcv);
            }
        }

        /// <summary>
        /// 处理接收到的数据
        /// </summary>
        private void OperiteRcvData()
        {
            while (OperateDataThread != null)
            {
                if (rcvData != null && rcvData.Count > 0)
                {
                    List<byte> command = null;
                    if (RcvCommandFilter(rcvData, out command))
                    {
                        XwRcvHeader header = (XwRcvHeader)command[0];
                        switch (header)
                        {
                            case XwRcvHeader.Connect_MCU_TO_PC:
                                CurrentRcvMsg = header.GetCustomAttribute<DescriptionAttribute>().Description;
                                break;
                            case XwRcvHeader.PackInfo_MCU_TO_PC:
                                CurrentRcvMsg = header.GetCustomAttribute<DescriptionAttribute>().Description;
                                break;
                            case XwRcvHeader.Firmware_MCU_TO_PC:
                                CurrentRcvMsg = header.GetCustomAttribute<DescriptionAttribute>().Description;
                                break;
                            case XwRcvHeader.RecvDone_MCU_TO_PC:
                                CurrentRcvMsg = header.GetCustomAttribute<DescriptionAttribute>().Description;
                                break;
                            case XwRcvHeader.ProgramDown_MCU_TO_PC:
                                CurrentRcvMsg = header.GetCustomAttribute<DescriptionAttribute>().Description;
                                break;
                            case XwRcvHeader.Error_MCU_TO_PC:
                                CurrentRcvMsg = string.Format("{0}", header.GetCustomAttribute<DescriptionAttribute>().Description);
                                break;
                            case XwRcvHeader.Get_DeviceID_MCU_TO_PC:
                                CurrentRcvMsg = string.Format("{0}:{1}", header.GetCustomAttribute<DescriptionAttribute>().Description, Encoding.GetEncoding("GB2312").GetString(command.GetRange(4, command[1] - 6).ToArray()));
                                break;
                            case XwRcvHeader.ReadAllRecord_MCU_TO_PC:
                                CurrentRcvMsg = header.GetCustomAttribute<DescriptionAttribute>().Description;
                                int count = BitConverter.ToInt16(command.GetRange(2, 2).ToArray(), 0);
                                CurrentRcvMsg += " " + count;
                                break;
                            case XwRcvHeader.ReadSingLereCord_MCU_TO_PC:
                                List<byte> fileInfo = command.GetRange(4, command[1] - 6);
                                BurnFileInfo burnFileInfo = new BurnFileInfo();
                                //固件信息为 18bytes 的芯片型号,32bytes 的文件名,4bytes 的文件 CRC,4bytes 的程序起始地址
                                if (fileInfo.Count >= 18) burnFileInfo.ChipType = Encoding.GetEncoding("GB2312").GetString(fileInfo.GetRange(0, 18).ToArray()).Replace("\0", "");
                                if (fileInfo.Count >= 40) burnFileInfo.FileName = Encoding.GetEncoding("GB2312").GetString(fileInfo.GetRange(18, 32).ToArray()).Replace("\0", "");
                                if (fileInfo.Count >= 54)
                                {
                                    var crcByteList = fileInfo.GetRange(50, 4);
                                    crcByteList.Reverse();
                                    burnFileInfo.CrcNum = crcByteList.Aggregate(string.Empty, (currten, t) => currten + string.Format("0x{0} ", t.ToString("X2"))).Trim();
                                }
                                if (fileInfo.Count >= 58) burnFileInfo.StartAddress = "0x" + BitConverter.ToInt32(fileInfo.GetRange(54, 4).ToArray(), 0).ToString("X10");
                                CurrentRcvMsg = string.Format("{0}:{1}", header.GetCustomAttribute<DescriptionAttribute>().Description, burnFileInfo.ToString());
                                break;
                            case XwRcvHeader.DeleteSingLereCord_MCU_TO_PC:
                                CurrentRcvMsg = header.GetCustomAttribute<DescriptionAttribute>().Description;
                                break;
                            case XwRcvHeader.DeleteAllRecord_MCU_TO_PC:
                                CurrentRcvMsg = header.GetCustomAttribute<DescriptionAttribute>().Description;
                                break;
                            case XwRcvHeader.Get_DeviceTypeVersion_MCU_TO_PC:
                                CurrentRcvMsg = header.GetCustomAttribute<DescriptionAttribute>().Description;
                                break;
                            case XwRcvHeader.BeginDown_MCU_TO_PC:
                                CurrentRcvMsg = header.GetCustomAttribute<DescriptionAttribute>().Description;
                                break;
                            case XwRcvHeader.GetCurrentFile_MCU_TO_PC:
                                string aaa = Encoding.GetEncoding("GB2312").GetString(command.GetRange(4, command[1] - 6).ToArray());
                                this.ReadProjectName = aaa;
                                WaitHandle_ReadCurrentProject.Set();
                                CurrentRcvMsg = string.Format("{0}:{1}", header.GetCustomAttribute<DescriptionAttribute>().Description, aaa);
                                break;
                            case XwRcvHeader.SelectFile_MCU_TO_PC:
                                CurrentRcvMsg = string.Format("{0}:{1}", header.GetCustomAttribute<DescriptionAttribute>().Description, Encoding.GetEncoding("GB2312").GetString(command.GetRange(4, command[1] - 6).ToArray()));
                                this.SelectProjectName = Encoding.GetEncoding("GB2312").GetString(command.GetRange(4, command[1] - 6).ToArray());
                                WaitHandle_SelectProject.Set();
                                break;
                            case XwRcvHeader.SelectFileIndex_MCU_TO_PC:

                                CurrentRcvMsg = string.Format("{0}:{1}", header.GetCustomAttribute<DescriptionAttribute>().Description, Encoding.GetEncoding("GB2312").GetString(command.GetRange(4, command[1] - 6).ToArray()));
                                break;
                            case XwRcvHeader.GetDownload_MCU_TO_PC:
                                CurrentRcvMsg = header.GetCustomAttribute<DescriptionAttribute>().Description;
                                int portIndex = command[2];
                                if (Enum.IsDefined(typeof(XwBurnState), command[3]) && portIndex < 4)
                                {
                                    CurrentBurnMsg = "通道" + portIndex + ":" + ((XwBurnState)command[3]).GetCustomAttribute<DescriptionAttribute>().Description + command[3];
                                    XwBurnState xwBurnState = (XwBurnState)command[3];
                                    if (xwBurnState == XwBurnState.ProgramOK || xwBurnState == XwBurnState.VerifyOK)
                                    {
                                        _isBurnFinish[portIndex] = true;
                                        this._burnResult[portIndex] = "OK:" + "通道" + (portIndex + 1) + " " + CurrentBurnMsg;
                                    }
                                    else if (xwBurnState == XwBurnState.ConnectFail || xwBurnState == XwBurnState.EraseFail || xwBurnState == XwBurnState.InitFail || xwBurnState == XwBurnState.NotHaveBurnCount || xwBurnState == XwBurnState.ProgramFail || xwBurnState == XwBurnState.VerifyFail || xwBurnState == XwBurnState.VolError)
                                    {
                                        _isBurnFinish[portIndex] = true;
                                        this._burnResult[portIndex] = "NG:" + "通道" + (portIndex + 1) + " " + CurrentBurnMsg;
                                    }
                                    else
                                    {
                                        //this._burnResult[portIndex] = "通道" + (portIndex + 1) + " " + CurrentBurnMsg;
                                    }
                                }
                                else
                                {
                                    CurrentBurnMsg = "未知状态：" + "通道" + (portIndex + 1) + " " + command[3];
                                    this._burnResult[portIndex] = CurrentBurnMsg;
                                }
                                BurnResult1 = this._burnResult[0];
                                BurnResult2 = this._burnResult[1];
                                BurnResult3 = this._burnResult[2];
                                BurnResult4 = this._burnResult[3];
                                break;
                            case XwRcvHeader.SendEraser_MCU_TO_PC:
                                CurrentRcvMsg = header.GetCustomAttribute<DescriptionAttribute>().Description;
                                break;
                            case XwRcvHeader.SendSpeed_MCU_TO_PC:
                                CurrentRcvMsg = header.GetCustomAttribute<DescriptionAttribute>().Description;
                                break;
                            case XwRcvHeader.SendSerialID_MCU_TO_PC:
                                CurrentRcvMsg = header.GetCustomAttribute<DescriptionAttribute>().Description;
                                break;
                            case XwRcvHeader.GetCheckCode_MCU_TO_PC:
                                var crc = command.GetRange(4, 4);
                                crc.Reverse();
                                string crcStr = crc.Aggregate(string.Empty, (currten, t) => currten + string.Format("0x{0} ", t.ToString("X2"))).Trim();
                                CurrentRcvMsg = string.Format("{0}:{1}", header.GetCustomAttribute<DescriptionAttribute>().Description, crcStr);
                                break;
                            default:
                                break;
                        }
                    }
                }
                Thread.Sleep(20);
            }
        }

        private void SendCommand(List<byte> conmend)
        {
            this.CurrentBurnMsg = "";
            this.CurrentRcvMsg = "";
            if (this._mySerialPort == null) return;

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    if (!this._mySerialPort.IsOpen)
                    {
                        Thread.Sleep(100);
                        this._mySerialPort.Open();
                    }
                    else
                    {
                        break;
                    }
                }
                catch (Exception)
                {

                    Thread.Sleep(1000);
                }
            }
            try
            {
                this._mySerialPort.Write(conmend.ToArray(), 0, conmend.Count);
            }
            catch (Exception)
            {
            }

        }

        /// <summary>
        /// 当前收到的可用数据的索引
        /// </summary>
        private int _usefulIndex = 0;
        /// <summary>
        /// 等待数据的次数（获取的数据个数很大，等待几次，如果等不到则认为这个数据个数是错的，继续增加索引判断）
        /// </summary>
        private int _waitDataCount = 0;
        /// <summary>
        /// 从收到的数据中过滤出 完整的指令
        /// </summary>
        /// <param name="rcvData"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        private bool RcvCommandFilter(List<byte> rcvData, out List<byte> command)
        {
            if (rcvData != null && rcvData.Count > _usefulIndex + 1)
            {
                if (Enum.IsDefined(typeof(XwRcvHeader), rcvData[_usefulIndex]))
                {
                    byte rcvCommandLength = rcvData[_usefulIndex + 1];
                    if (rcvCommandLength > 2)
                    {
                        if (rcvData.Count >= _usefulIndex + rcvCommandLength)
                        {
                            List<byte> tempList = rcvData.GetRange(_usefulIndex, rcvCommandLength);
                            byte[] crc = ValueHelper.GetCrc16(tempList.GetRange(0, rcvCommandLength - 2), rcvCommandLength - 2);
                            if (crc[0] == tempList[rcvCommandLength - 2] && crc[1] == tempList[rcvCommandLength - 1])
                            {
                                command = tempList;
                                lock (rcvLock)
                                {
                                    rcvData.RemoveRange(0, _usefulIndex + rcvCommandLength);
                                }
                                _usefulIndex = 0;
                                return true;
                            }
                            else
                            {
                                _usefulIndex++;
                            }
                        }
                        else
                        {
                            _waitDataCount++;
                            if (_waitDataCount >= 5)
                            {
                                _waitDataCount = 0;
                                _usefulIndex++;
                            }
                        }
                    }
                    else
                    {
                        _usefulIndex++;
                    }
                }
                else
                {
                    _usefulIndex++;
                }
            }
            command = new List<byte>() { };
            return false;
        }

        /// <summary>
        /// 稀微烧写器回复的帧头
        /// </summary>
        public enum XwRcvHeader : byte
        {
            /// <summary>
            /// 下位机握手回复
            /// </summary>
            [Description("下位机握手回复")]
            Connect_MCU_TO_PC = 0XBB,
            /// <summary>
            /// 下位机回复文件信息
            /// </summary>
            [Description("下位机回复文件信息")]
            PackInfo_MCU_TO_PC = 0XDD,
            /// <summary>
            /// 下位机传固件回复包至上位机
            /// </summary>
            [Description("下位机传固件回复包至上位机")]
            Firmware_MCU_TO_PC = 0XFF,
            /// <summary>
            /// 下位机收到完整的固件报告至上位机
            /// </summary>
            [Description("下位机收到完整的固件报告至上位机")]
            RecvDone_MCU_TO_PC = 0X11,
            /// <summary>
            /// 下位机报告烧写完成的信息至上位机
            /// </summary>
            [Description("下位机报告烧写完成的信息至上位机")]
            ProgramDown_MCU_TO_PC = 0X22,
            /// <summary>
            /// 下位机向上位机报告当前无错误
            /// </summary>
            [Description("下位机向上位机报告当前无错误")]
            Error_MCU_TO_PC = 0X33,
            /// <summary>
            /// 下位机回复 ID
            /// </summary>
            [Description("下位机回复ID")]
            Get_DeviceID_MCU_TO_PC = 0X55,
            /// <summary>
            /// 下位机回复全部文件记录
            /// </summary>
            [Description("下位机回复全部文件记录个数")]
            ReadAllRecord_MCU_TO_PC = 0XB1,
            /// <summary>
            /// 下位机传读单文件记录
            /// </summary>
            [Description("下位机传读单文件记录")]
            ReadSingLereCord_MCU_TO_PC = 0XB2,
            /// <summary>
            /// 下位机回复删除单文件记录
            /// </summary>
            [Description("下位机回复删除单文件记录")]
            DeleteSingLereCord_MCU_TO_PC = 0XB3,
            /// <summary>
            /// 下位机回复删除全部文件记录
            /// </summary>
            [Description("下位机回复删除全部文件记录")]
            DeleteAllRecord_MCU_TO_PC = 0XB4,
            /// <summary>
            /// 下位机回复类型版本信息
            /// </summary>
            [Description("下位机回复类型版本信息")]
            Get_DeviceTypeVersion_MCU_TO_PC = 0XB5,
            /// <summary>
            /// 下位机向上位机发送确认开始烧录信息
            /// </summary>
            [Description("下位机向上位机发送确认开始烧录信息")]
            BeginDown_MCU_TO_PC = 0XB8,
            /// <summary>
            /// 下位机向上位机发送当前烧录文件名
            /// </summary>
            [Description("下位机向上位机发送当前烧录文件名")]
            GetCurrentFile_MCU_TO_PC = 0XB9,
            /// <summary>
            /// 下位机向上位机发送选择文件名
            /// </summary>
            [Description("下位机向上位机发送选择文件名")]
            SelectFile_MCU_TO_PC = 0XD0,
            /// <summary>
            /// 下位机向上位机发送选择文件名序号
            /// </summary>
            [Description("下位机向上位机发送选择文件名序号")]
            SelectFileIndex_MCU_TO_PC = 0XD1,
            /// <summary>
            /// 下位机向上位机发送请求烧录进度信息
            /// </summary>
            [Description("下位机向上位机发送请求烧录进度信息")]
            GetDownload_MCU_TO_PC = 0XD2,
            /// <summary>
            /// 下位机向上位机发送擦除命令确认
            /// </summary>
            [Description("下位机向上位机发送擦除命令确认")]
            SendEraser_MCU_TO_PC = 0XD3,
            /// <summary>
            /// 下位机向上位机发送修改速度命令确认
            /// </summary>
            [Description("下位机向上位机发送修改速度命令确认")]
            SendSpeed_MCU_TO_PC = 0XD4,
            /// <summary>
            /// 下位机向上位机发送更新序列号内容
            /// </summary>
            [Description("下位机向上位机发送更新序列号内容")]
            SendSerialID_MCU_TO_PC = 0XD5,
            /// <summary>
            /// 下位机向上位机发送读取校验
            /// </summary>
            [Description("下位机向上位机发送读取校验")]
            GetCheckCode_MCU_TO_PC = 0XD6,
        }

        /// <summary>
        /// 烧写文件信息
        /// </summary>
        public class BurnFileInfo
        {
            /// <summary>
            /// 芯片类型
            /// </summary>
            public string ChipType = string.Empty;
            /// <summary>
            /// 文件名称
            /// </summary>
            public string FileName = string.Empty;
            /// <summary>
            /// CRC校验
            /// </summary>
            public string CrcNum = string.Empty;
            /// <summary>
            /// 程序起始地址
            /// </summary>
            public string StartAddress = string.Empty;


            public override string ToString()
            {
                return string.Format("芯片类型:{0},文件名称{1},CRC校验{2},程序起始地址{3}", ChipType, FileName, CrcNum, StartAddress);
            }

        }

        /// <summary>
        /// 稀微烧写器烧录状态
        /// </summary>
        public enum XwBurnState : byte
        {
            /// <summary>
            /// 烧录器读出数据,准备烧录
            /// </summary>
            [Description("烧录器读出数据,准备烧录")]
            ReadyBurn = 0x11,
            /// <summary>
            /// 烧录器准备OK,可以烧录
            /// </summary>
            [Description("烧录器准备OK,可以烧录")]
            ReadyOK = 0x22,
            /// <summary>
            /// 初始化失败
            /// </summary>
            [Description("初始化失败")]
            InitFail = 0x33,
            /// <summary>
            /// 正在擦除
            /// </summary>
            [Description("正在擦除")]
            Erasing = 0x44,
            /// <summary>
            /// 擦除成功
            /// </summary>
            [Description("擦除成功")]
            ErasingOK = 0x55,
            /// <summary>
            /// 正在编程
            /// </summary>
            [Description("正在编程")]
            Programing = 0x66,
            /// <summary>
            /// 编程成功
            /// </summary>
            [Description("编程成功")]
            ProgramOK = 0x77,
            /// <summary>
            /// 正在校验
            /// </summary>
            [Description("正在校验")]
            Verifing = 0x88,
            /// <summary>
            /// 校验成功
            /// </summary>
            [Description("校验成功")]
            VerifyOK = 0x99,
            /// <summary>
            /// 链接失败
            /// </summary>
            [Description("链接失败")]
            ConnectFail = 0xAA,
            /// <summary>
            /// 擦除失败
            /// </summary>
            [Description("擦除失败")]
            EraseFail = 0xBB,
            /// <summary>
            /// 编程失败
            /// </summary>
            [Description("编程失败")]
            ProgramFail = 0xCC,
            /// <summary>
            /// 校验失败
            /// </summary>
            [Description("校验失败")]
            VerifyFail = 0xDD,
            /// <summary>
            /// 剩余烧录次数为0
            /// </summary>
            [Description("剩余烧录次数为0")]
            NotHaveBurnCount = 0xEE,
            /// <summary>
            /// 电压异常
            /// </summary>
            [Description("电压异常")]
            VolError = 0xFF
        }
    }
}
