using CommonUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace Controller
{
    public sealed class Tps929120Ti : ControllerBase
    {
        //private readonly List<byte[]> _dataByteses = new List<byte[]>();
        private readonly Thread _ledControlTh;
        private bool _isLedControl;
        public MySerialPort MySerialPort;

        public delegate void PushLogSciMsgEventHandle(byte[] datas);
        public event PushLogSciMsgEventHandle PushLogSciMsg;

        [Description("R/W,写入时是否校验回复帧的CRC")]
        public bool IsCheckCrcWhenWtire = true;

        public Tps929120Ti(string name)
            : base(name)
        {
            CRC_Table_Init(_tps929120TiCrcInfo);
            MySerialPort.PushSciMsg += MySerialPort_PushSciMsg;

            _ledControlTh = new Thread(LedControl) { IsBackground = true };
            _ledControlTh.Start();
        }

        ~Tps929120Ti()
        {
            Dispose();
        }

        /// <summary>
        /// 是否已发送命令
        /// </summary>
        private bool _isSendCmd;

        /// <summary>
        /// 发送的命令
        /// </summary>
        private string _rxStr = string.Empty;

        /// <summary>
        /// 收到的命令
        /// </summary>
        private string _txStr = string.Empty;

        /// <summary>
        /// 事件
        /// </summary>
        private readonly EventWaitHandle _waitHandle = new AutoResetEvent(false);

        private void MySerialPort_PushSciMsg(string name, byte[] datas)
        {
            if (MySerialPort == null || MySerialPort.Name != name)
                return;

            var str =
                datas.Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
            Debug.WriteLine(str);

            if (PushLogSciMsg != null)
                PushLogSciMsg(datas);

            if (!_isSendCmd || string.IsNullOrEmpty(_rxStr))
                return;
            if (string.IsNullOrEmpty(str) || str.Length <= _rxStr.Length)
                return;
            if (str.Substring(0, _rxStr.Length) != _rxStr)
                return;
            _rxStr = string.Empty;
            _txStr = str;
            _waitHandle.Set();
        }

        //[Description("测试刷写")]
        //public void DebugProgram()
        //{
        //    if (MySerialPort == null) 
        //        return;
        //    foreach (var t in _dataByteses)
        //    {
        //        MySerialPort.SendCommand(t);
        //        Thread.Sleep(10);
        //    }
        //}

        [Description("R/W,配置参数文件路径")]
        public string DataFilePath =
            //@"C:\Projs\2022\Matrix芯片相关\TPS929120-TI\DCI1_TPS929129TI_Paras.txt";
            //@"C:\Projs\2022\芯片相关\TPS929120-TI\DEI1参数-20220601.tps";
            @"C:\Projs\2022\芯片相关\TPS929120-TI\929120默认参数.tps";
        //@"C:\Projs\2022\芯片相关\TPS929120-TI\929120A默认参数.tps";
        //string.Empty;

        [Description("R/W,REF引脚是否拉高")]
        public bool IsRefPinHigh = false;

        [Description("R,遍历刷写成功的芯片个数")]
        public int ErgodicProgramTpsCount;

        [Description("R,遍历刷写成功的芯片地址列表")]
        public string ErgodicProgramTpsDevAddrs = string.Empty;

        [Description("R,遍历刷写总耗时")]
        public string ErgodicProgramTpsCostMs;

        [Description("R/W,CONF_MISC0值")]
        public string DefautMiscellanousRegister0 = "0x00";

        [Description("R/W,CONF_MISC1值")]
        public string DefautMiscellanousRegister1 = "0x43";

        [Description("R/W,CONF_MISC2值")]
        public string DefautMiscellanousRegister2 = "0x70";

        [Description("R/W,CONF_MISC3值")]
        public string DefautMiscellanousRegister3 = "0xF1";

        [Description("R/W,CONF_MISC4值")]
        public string DefautMiscellanousRegister4 = "0x00";

        [Description("R/W,CONF_MISC5值")]
        public string DefautMiscellanousRegister5 = "0x00";

        private const byte SyncFrame = 0x55;
        private const byte BroadcastWrite1BytesDevAddrByte = 0x80; // 0xCB;
        private const byte BroadcastWrite8BytesDevAddrByte = 0xB0; // 0xFB;
        private readonly CrcHelper.CrcInfo _tps929120TiCrcInfo = new CrcHelper.CrcInfo
        {
            Width = 8,
            Poly = 0x31,
            InitReg = 0xff,
            Refin = true,
            Refout = false,
            Xorout = 0x00
        };

        [Description("从配置文件读取参数并遍历刷写")]
        public void ErgodicProgramTpsFromDataFile()
        {
            ErgodicProgramTpsCount = 0;
            ErgodicProgramTpsDevAddrs = string.Empty;
            ErgodicProgramTpsCostMs = string.Empty;

            //return;

            if (string.IsNullOrEmpty(DataFilePath))
                return;

            if (!File.Exists(DataFilePath))
                return;

            if (MySerialPort == null)
                return;

            var st = new Stopwatch();
            st.Start();

            var dataList = new List<Dictionary<byte, byte>>();
            var lastRedAddr = 0xFF;

            var toCrcData = new List<byte>();

            var lines = File.ReadAllLines(DataFilePath).ToList();

            foreach (var l in lines)
            {
                try
                {
                    if (string.IsNullOrEmpty(l))
                        continue;
                    var regAddr = Convert.ToByte(l.Split(':')[0]);
                    var regData = Convert.ToByte(l.Split(':')[1]);

                    if (lastRedAddr == 0xFF && regAddr != 0xCF)
                    {
                        lastRedAddr = regAddr;
                        dataList.Add(new Dictionary<byte, byte>());
                        dataList[dataList.Count - 1].Add(regAddr, regData);

                        if (regAddr < 0xCF)
                            toCrcData.Add(regData);
                    }
                    else if (lastRedAddr != 0xFF && regAddr != 0xCF)
                    {
                        if (lastRedAddr + 0x01 == regAddr)
                        {
                            lastRedAddr = regAddr;
                            dataList[dataList.Count - 1].Add(regAddr, regData);

                            if (regAddr < 0xCF)
                                toCrcData.Add(regData);
                        }
                        else
                        {
                            lastRedAddr = regAddr;
                            dataList.Add(new Dictionary<byte, byte>());
                            dataList[dataList.Count - 1].Add(regAddr, regData);

                            if (regAddr < 0xCF)
                                toCrcData.Add(regData);
                        }
                    }
                }
                catch (Exception)
                {
                    return;
                }
            }

            var totalCrc = CALC_CRC(_tps929120TiCrcInfo, toCrcData.ToArray());
            dataList.Add(new Dictionary<byte, byte>());
            dataList[dataList.Count - 1].Add(0xCF, (byte)totalCrc);

            ErgodicProgram(0.ToString(), 15.ToString(), dataList);

            st.Stop();
            ErgodicProgramTpsDevAddrs = ErgodicProgramTpsDevAddrs.Trim(',');
            ErgodicProgramTpsCostMs = st.ElapsedMilliseconds + "ms";
        }

        [Description("从配置文件读取参数并单独刷写")]
        public void SingleErgodicProgramTpsFromDataFile(string index)
        {
            ErgodicProgramTpsCount = 0;
            ErgodicProgramTpsDevAddrs = string.Empty;
            ErgodicProgramTpsCostMs = string.Empty;

            if (string.IsNullOrEmpty(DataFilePath))
                return;

            if (!File.Exists(DataFilePath))
                return;

            if (MySerialPort == null)
                return;

            int matrixIndex;
            if (!int.TryParse(index, out matrixIndex))
                return;

            if (matrixIndex < 0 || matrixIndex > 15)
                return;

            //#region debug
            //TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + matrixIndex), 0x61, new byte[] { 0x00 }));
            //TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + matrixIndex), 0x63, new byte[] { 0x00 }));
            //TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + matrixIndex), 0x50, new byte[] { 0x00 }));
            //TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + matrixIndex), 0x51, new byte[] { 0x00 }));
            //TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + matrixIndex), 0x56, new byte[] { 0x00 }));
            //TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + matrixIndex), 0x57, new byte[] { 0xa7 }));
            //TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + matrixIndex), 0x58, new byte[] { 0x03 }));
            //TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + matrixIndex), 0x59, new byte[] { 0x00 }));
            //TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + matrixIndex), 0x5A, new byte[] { 0x00 }));
            //TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + matrixIndex), 0x5B, new byte[] { 0x00 }));
            //return;

            //TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + matrixIndex), 0x61, new byte[] { 0x00 }));
            //TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + matrixIndex), 0x60, new byte[] { 0x33 }));
            //TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + matrixIndex), 0x60, new byte[] { 0x10 }));
            //TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + matrixIndex), 0x61, new byte[] { 0x00 }));
            //return;
            //#endregion

            var st = new Stopwatch();
            st.Start();

            var dataList = new List<Dictionary<byte, byte>>();
            var lastRedAddr = 0xFF;

            var toCrcData = new List<byte>();

            var lines = File.ReadAllLines(DataFilePath);
            foreach (var l in lines)
            {
                try
                {
                    if (string.IsNullOrEmpty(l))
                        continue;
                    var regAddr = Convert.ToByte(l.Split(':')[0]);
                    var regData = Convert.ToByte(l.Split(':')[1]);

                    if (lastRedAddr == 0xFF && regAddr != 0xCF)
                    {
                        lastRedAddr = regAddr;
                        dataList.Add(new Dictionary<byte, byte>());
                        dataList[dataList.Count - 1].Add(regAddr, regData);

                        if (regAddr < 0xCF)
                            toCrcData.Add(regData);
                    }
                    else if (lastRedAddr != 0xFF && regAddr != 0xCF)
                    {
                        if (lastRedAddr + 0x01 == regAddr)
                        {
                            lastRedAddr = regAddr;
                            dataList[dataList.Count - 1].Add(regAddr, regData);

                            if (regAddr < 0xCF)
                                toCrcData.Add(regData);
                        }
                        else
                        {
                            lastRedAddr = regAddr;
                            dataList.Add(new Dictionary<byte, byte>());
                            dataList[dataList.Count - 1].Add(regAddr, regData);

                            if (regAddr < 0xCF)
                                toCrcData.Add(regData);
                        }
                    }
                }
                catch (Exception)
                {
                    return;
                }
            }

            var totalCrc = CALC_CRC(_tps929120TiCrcInfo, toCrcData.ToArray());
            dataList.Add(new Dictionary<byte, byte>());
            dataList[dataList.Count - 1].Add(0xCF, (byte)totalCrc);
            //dataList[dataList.Count - 1].Add(0xCF, (byte)0x09);

            ErgodicProgram(matrixIndex.ToString(), matrixIndex.ToString(), dataList);

            st.Stop();
            ErgodicProgramTpsDevAddrs = ErgodicProgramTpsDevAddrs.Trim(',');
            ErgodicProgramTpsCostMs = st.ElapsedMilliseconds + "ms";
        }

        private void ErgodicProgram(
            string startIndex, string endIndex, List<Dictionary<byte, byte>> dataList)
        {
            byte v0;
            byte v1;
            byte v2;
            byte v3;
            byte v4;
            byte v5;

            try
            {
                v0 = Convert.ToByte(DefautMiscellanousRegister0, 16);
            }
            catch (Exception)
            {
                v0 = 0x00;
            }

            try
            {
                v1 = Convert.ToByte(DefautMiscellanousRegister1, 16);
            }
            catch (Exception)
            {
                v1 = 0x43;
            }

            try
            {
                v2 = Convert.ToByte(DefautMiscellanousRegister2, 16);
            }
            catch (Exception)
            {
                v2 = 0x70;
            }

            try
            {
                v3 = Convert.ToByte(DefautMiscellanousRegister3, 16);
            }
            catch (Exception)
            {
                v3 = 0xF1;
            }

            try
            {
                v4 = Convert.ToByte(DefautMiscellanousRegister4, 16);
            }
            catch (Exception)
            {
                v4 = 0x00;
            }

            try
            {
                v5 = Convert.ToByte(DefautMiscellanousRegister5, 16);
            }
            catch (Exception)
            {
                v5 = 0x00;
            }

            for (var p = int.Parse(startIndex); p < int.Parse(endIndex) + 1; p++)
            {
                /* The TPS929120-Q1 provides registers content lock feature to prevent unintended modification of registers. There
                * are 4 register lock bits for different type of registers covering all registers. The 4 lock register bits is set to 1 as
                * default, which means the master controller must the set lock bit to 0 before write operation to the corresponding
                * registers. TI recommends locking the register after register writing operations.
                * • Setting CONF_IOUTLOCK to 1 disables write operation to IOUTx registers.
                * • Setting CONF_PWMLOCK to 1 disables write operation to PWMx and PWMLx registers.
                * • Setting CONF_CONFLOCK to 1 disables write operation to CONFx registers.
                * • Setting CONF_CLRLOCK to 1 disables write operation to CLRx registers.
                */

                //TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + matrixIndex), 0x63, new byte[] { 0x00 }));
                //TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + matrixIndex), 0x60, new byte[] { 0x05 }));

                if (TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x61, new byte[] { 0x00 })))
                //TpsSendCmd(GetSendData((byte) (BroadcastWrite1BytesDevAddrByte + p), 0x61, new byte[] {0x00}));
                {
                    TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x50, new byte[] { 0x00 }));
                    TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x51, new byte[] { 0x00 }));
                    TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x63, new byte[] { 0x00 }));

                    //TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x50, new byte[] { 0x00 }));
                    //TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x51, new byte[] { 0x00 }));

                    TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x56, new[] { v0 }));
                    TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x57, new[] { v1 }));
                    TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x58, new[] { v2 }));
                    TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x59, new[] { v3 }));
                    TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x5A, new[] { v4 }));
                    TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x5B, new[] { v5 }));

                    // Write to force the device out of Fail-safe states to normal state,automatically reset to 0
                    // Write 1 to clear all fault flags, automatically reset to 0
                    // Write 1 to clear POR flag, automatically reset to 0
                    if (TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x60, new byte[] { 0x07 }))) // 改成0x17无法刷写，改回0x07，20230517
                    {
                        //continue;

                        // access the EEPROM
                        if (IsRefPinHigh)
                        {
                            // • Write 09h, 02h, 09h, 01h, 02h, 00h to 8-bit register CONF_EEPGATE one-byte by one-byte sequentially.
                            TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x65, new byte[] { 0x09 }));
                            TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x65, new byte[] { 0x02 }));
                            TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x65, new byte[] { 0x09 }));
                            TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x65, new byte[] { 0x01 }));
                            TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x65, new byte[] { 0x02 }));
                            TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x65, new byte[] { 0x00 }));
                        }
                        else
                        {
                            // • Write 00h, 02h, 01h, 09h, 02h, 09h to 8-bit register CONF_EEPGATE one-byte by one-byte sequentially.
                            TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x65, new byte[] { 0x00 }));
                            TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x65, new byte[] { 0x02 }));
                            TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x65, new byte[] { 0x01 }));
                            TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x65, new byte[] { 0x09 }));
                            TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x65, new byte[] { 0x02 }));
                            TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x65, new byte[] { 0x09 }));
                        }

                        // • Write 1 to 1-bit register CONF_EEPMODE
                        // Enable EEPMODE Programming State
                        if (TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x63, new byte[] { 0x01 })))
                        {
                            // • Write 1 to 1-bit register CONF_STAYINEEP
                            // EEPROM mode enableds
                            if (TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x62, new byte[] { 0x80 })))
                            {
                                var sendError = 0;

                                // • Wrtie data to EEPROM including CRC
                                foreach (var d in dataList)
                                {
                                    if (d.Count >= 8) // 按最大8字节分组
                                    {
                                        var groupCount = d.Count / 8;
                                        var rest = d.Count - groupCount * 8;

                                        var baseIndex = -1;

                                        for (var i = 0; i < groupCount; i++)
                                        {
                                            var sendData = new List<byte>();
                                            for (var j = 0; j < 8; j++)
                                            {
                                                baseIndex++;
                                                var key = d.Keys.ToList()[baseIndex];
                                                sendData.Add(d[key]);
                                            }

                                            if (!TpsSendCmd(GetSendData((byte)(BroadcastWrite8BytesDevAddrByte + p),
                                                d.Keys.ToList()[0 + 8 * i], sendData)))
                                                sendError++;
                                        }

                                        for (var i = 0; i < rest; i++)
                                        {
                                            var sendData = new List<byte>();
                                            baseIndex++;
                                            var key = d.Keys.ToList()[baseIndex];
                                            sendData.Add(d[key]);

                                            if (!TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), key,
                                                sendData)))
                                                sendError++;

                                            //TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), key,
                                            //    sendData));
                                        }
                                    }
                                    //else if (d.Count >= 4 && d.Count < 8) // 按最大4字节分组
                                    //{

                                    //}
                                    //else if (d.Count >= 2 && d.Count < 4) // 按最大2字节分组
                                    //{

                                    //}
                                    else if (d.Count >= 1 && d.Count < 8) // 按最大1字节分组
                                    {
                                        var baseIndex = -1;

                                        for (var i = 0; i < d.Count; i++)
                                        {
                                            var sendData = new List<byte>();
                                            baseIndex++;
                                            var key = d.Keys.ToList()[baseIndex];
                                            sendData.Add(d[key]);

                                            if (
                                                !TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p),
                                                    key,
                                                    sendData)))
                                                sendError++;
                                            //TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), key,
                                            //    sendData));
                                        }
                                    }
                                }

                                if (sendError == 0)
                                {
                                    // EEPROM burning start in EEPROM mode only, automatically returns to 0
                                    // • Write 01h to CONF_EEOOROG
                                    if (TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x64,
                                        new byte[] { 0x44 })))
                                    {
                                        // • Keep supply stable and wait for 200ms
                                        Thread.Sleep(500);

                                        // • Write 0h to CONF_STAYINEEP to Normal mode
                                        if (TpsSendCmd(GetSendData((byte)(BroadcastWrite1BytesDevAddrByte + p), 0x62,
                                            new byte[] { 0x00 })))
                                        {
                                            Thread.Sleep(15);
                                        }
                                    }
                                }

                                var isOk = true;
                                foreach (var t in dataList)
                                {
                                    foreach (var regAddr in t.Keys)
                                    {
                                        var regValue = t[regAddr];

                                        //if (regAddr != 0xCF)
                                        {
                                            ReadSingleDevReg(p.ToString(), ValueHelper.GetHextStr(regAddr));

                                            var findFieldName = string.Format("DevAddr{0}hRegAddr{1}h",
                                                ValueHelper.GetHextStr(Convert.ToByte(p.ToString())),
                                                ValueHelper.GetHextStr(regAddr));

                                            foreach (var f in GetType().GetFields())
                                            {
                                                if (f.Name.StartsWith(findFieldName))
                                                {
                                                    var fv = f.GetValue(this);
                                                    if (fv == null || string.IsNullOrEmpty(fv.ToString()))
                                                    {
                                                        isOk = false;
                                                        break;
                                                    }

                                                    if (fv.ToString() != ValueHelper.GetHextStr(regValue))
                                                    {
                                                        isOk = false;
                                                    }

                                                    break;
                                                }
                                            }
                                        }

                                        if (!isOk)
                                            break;
                                    }

                                    if (!isOk)
                                        break;
                                }

                                if (isOk)
                                {
                                    ErgodicProgramTpsDevAddrs += ValueHelper.GetHextStr((byte)p) + ",";
                                    ErgodicProgramTpsCount++;
                                }
                            }
                        }
                    }
                }
            }
        }

        [Description("读寄存器")]
        public void ReadSingleDevReg(string devAddr, string regAddr)
        {
            var devAddrByte = Convert.ToByte(byte.Parse(devAddr).ToString());
            var regAddrByte = Convert.ToByte(regAddr, 16);
            for (var q = 0; q < 8; q++)
            {
                var i1 = q;
                var addrByte = devAddrByte;
                foreach (var f in from f in GetType().GetFields()
                                  let findF = string.Format("DevAddr{0}hRegAddr{1}h",
                                  ValueHelper.GetHextStr(addrByte),
                                  ValueHelper.GetHextStr((byte)(regAddrByte + i1)))
                                  where f.Name.StartsWith(findF)
                                  select f)
                    f.SetValue(this, string.Empty);
            }

            devAddrByte = (byte)(0x30 + devAddrByte);
            TpsSendCmd(GetSendData(devAddrByte, regAddrByte, new byte[0]));
            //if (MySerialPort != null)
            //    MySerialPort.SendCommand(GetSendData(devAddrByte, regAddrByte, new byte[0])); //debug 55 06 80 E5 
        }

        [Description("读所有地址的寄存器")]
        public void ReadAllDevReg(string regAddr)
        {
            for (var i = 0; i < 16; i++)
            {
                var devAddrByte = Convert.ToByte(i.ToString());
                var regAddrByte = Convert.ToByte(regAddr, 16);

                var addrByte = devAddrByte;
                for (var q = 0; q < 8; q++)
                {
                    var i1 = q;
                    foreach (var f in from f in GetType().GetFields()
                                      let findF = string.Format("DevAddr{0}hRegAddr{1}h",
                                      ValueHelper.GetHextStr(addrByte),
                                      ValueHelper.GetHextStr((byte)(regAddrByte + i1)))
                                      where f.Name.StartsWith(findF)
                                      select f)
                        f.SetValue(this, string.Empty);
                }

                //foreach (var f in from f in GetType().GetFields()
                //                  let str = string.Format("DevAddr{0}hRegAddr{1}h", ValueHelper.GetHextStr(addrByte), ValueHelper.GetHextStr(addrByte))
                //                  where f.Name.StartsWith(str)
                //                  select f)
                //    f.SetValue(this, string.Empty);

                devAddrByte = (byte)(0x30 + devAddrByte);
                TpsSendCmd(GetSendData(devAddrByte, regAddrByte, new byte[0]));
                //if (MySerialPort != null)
                //{
                //    MySerialPort.SendCommand(GetSendData(devAddrByte, regAddrByte, new byte[0])); //debug 55 06 80 E5 
                //    Thread.Sleep(100);

                //}
            }
        }

        [Description("清除所有地址的寄存器读取结果")]
        public void ClearAllDevRegReadResult()
        {
            for (var i = 0; i < 16; i++)
            {
                var devAddrByte = Convert.ToByte(i.ToString());
                var addrByte = devAddrByte;
                foreach (var f in from f in GetType().GetFields()
                                  let findF = string.Format("DevAddr{0}hRegAddr",
                                  ValueHelper.GetHextStr(addrByte))
                                  where f.Name.StartsWith(findF)
                                  select f)
                    f.SetValue(this, string.Empty);
            }
        }

        [Description("R,CRC计算工具结果")]
        public string ClacCrcToolResult;

        [Description("CRC计算工具")]
        public void ClacCrcTool(string value)
        {
            ClacCrcToolResult = string.Empty;
            try
            {
                value = value.Replace("0x", "");
                value = value.Replace(" ", "");
                var bs = new List<byte>();
                for (var i = 0; i < value.Length; i = i + 2)
                    bs.Add(Convert.ToByte(string.Format("{0}{1}", value[i], value[i + 1]), 16));
                ClacCrcToolResult = ValueHelper.GetHextStrWithOx((byte)CALC_CRC(_tps929120TiCrcInfo, bs.ToArray()));
            }
            catch (Exception ex)
            {
                ClacCrcToolResult = ex.Message;
            }
        }

        private readonly uint[] _table = new uint[256];

        private void CRC_Table_Init(CrcHelper.CrcInfo crcInfo)
        {
            uint poly, value;
            var validBits = ((uint)2 << (crcInfo.Width - 1)) - 1;

            if (crcInfo.Refin)
            {
                poly = BitReflected(crcInfo.Poly, crcInfo.Width);
                for (uint i = 0; i < 256; i++)
                {
                    value = i;
                    for (uint j = 0; j < 8; j++)
                    {
                        if ((value & 0x0001) != 0)
                            value = (value >> 1) ^ poly;
                        else
                            value >>= 1;
                    }
                    _table[i] = value & validBits;
                }
            }
            else
            {
                poly = crcInfo.Width < 8 ? crcInfo.Poly << (8 - crcInfo.Width) : crcInfo.Poly;
                var bit = crcInfo.Width > 8 ? (uint)1 << (crcInfo.Width - 1) : 0x80;

                for (uint i = 0; i < 256; i++)
                {
                    value = crcInfo.Width > 8 ? i << (crcInfo.Width - 8) : i;

                    for (uint j = 0; j < 8; j++)
                    {
                        if ((value & bit) != 0)
                            value = (value << 1) ^ poly;
                        else
                            value <<= 1;
                    }
                    _table[i] = value & (crcInfo.Width < 8 ? 0xff : validBits);
                }
            }
        }

        private static uint BitReflected(uint input, byte bits)
        {
            uint res = 0;
            while (bits-- > 0)
            {
                res <<= 1;
                if ((input & 0x01) != 0)
                    res |= 1;
                input >>= 1;
            }
            return res;
        }

        private uint CALC_CRC(CrcHelper.CrcInfo info, IReadOnlyList<byte> memBlock)
        {
            var memBlockLen = (uint)memBlock.Count;
            uint value;

            if (info.Refin)
            {
                value = BitReflected(info.InitReg, info.Width);
                if (info.Width > 8)
                {
                    var i = 0;
                    while (memBlockLen-- != 0)
                    {
                        value = (value >> 8) ^ _table[value & 0xff ^ memBlock[i++]];
                    }
                }
                else
                {
                    var i = 0;
                    while (memBlockLen-- != 0)
                    {
                        value = _table[value ^ memBlock[i++]];
                    }
                }
            }
            else
            {
                if (info.Width > 8)
                {
                    value = info.InitReg;
                    var i = 0;
                    while (memBlockLen-- != 0)
                    {
                        var high = (byte)(value >> (info.Width - 8));
                        value = (value << 8) ^ _table[high ^ memBlock[i++]];
                    }
                }
                else
                {
                    value = info.InitReg << (8 - info.Width);
                    var i = 0;
                    while (memBlockLen-- != 0)
                    {
                        value = _table[value ^ memBlock[i++]];
                    }
                    value >>= 8 - info.Width;
                }
            }
            if (info.Refout != info.Refin)
            {
                value = BitReflected(value, info.Width);
            }
            value ^= info.Xorout;
            return value & (((uint)2 << (info.Width - 1)) - 1);
        }

        private byte[] GetSendData(
            byte devAddrByte, byte regAddeByte, IEnumerable<byte> datas)
        {
            var sendData = new List<byte>();
            sendData.AddRange(datas);

            sendData.Insert(0, regAddeByte);
            sendData.Insert(0, devAddrByte);
            var crc = CALC_CRC(_tps929120TiCrcInfo, sendData.ToArray());
            sendData.Add((byte)crc);
            sendData.Insert(0, SyncFrame);

            return sendData.ToArray();
        }

        private bool TpsSendCmd(byte[] cmdData)
        {
            if (MySerialPort == null)
                return false;

            _isSendCmd = true;
            _waitHandle.Reset();
            var str = cmdData.Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
            _rxStr = str;
            MySerialPort.SendCommand(cmdData, 1);
            //Thread.Sleep(25);
            if (_waitHandle.WaitOne(50))
            {
                if (!string.IsNullOrEmpty(_txStr) &&
                    _txStr.Length % 2 == 0 &&
                    _txStr.Length > str.Length &&
                    _txStr.Substring(0, str.Length) == str)
                {
                    var rxBytes = new List<byte>();
                    for (var i = 0; i < _txStr.Length; i = i + 2)
                    {
                        var b = string.Format("{0}{1}", _txStr[i], _txStr[i + 1]);
                        rxBytes.Add(Convert.ToByte(b, 16));
                    }

                    var bitStr = Convert.ToString(rxBytes[1], 2).PadLeft(8, '0');
                    var temp = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", bitStr[7], bitStr[6], bitStr[5], bitStr[4],
                        bitStr[3], bitStr[2], bitStr[1], bitStr[0]);
                    bitStr = temp;

                    var deviceAddr =
                        Convert.ToByte(string.Format("0000{3}{2}{1}{0}", bitStr[0], bitStr[1], bitStr[2], bitStr[3]), 2);
                    var dataLength = 1;
                    switch (string.Format("{0}{1}", bitStr[4], bitStr[5]))
                    {
                        case "00":
                            dataLength = 1;
                            break;

                        case "10":
                            dataLength = 2;
                            break;

                        case "01":
                            dataLength = 4;
                            break;

                        case "11":
                            dataLength = 8;
                            break;
                    }

                    var isRead = bitStr[7].ToString() == 0.ToString();

                    if (isRead)
                    {
                        if (rxBytes.Count == 1 + 1 + 1 + 1 + dataLength + 1)
                        {
                            var rxValues = new byte[dataLength];
                            Array.Copy(rxBytes.ToArray(), 4, rxValues, 0, rxValues.Length);

                            var rxCrc = rxBytes[4 + dataLength + 1 - 1];
                            if (CALC_CRC(_tps929120TiCrcInfo, rxValues) == rxCrc)
                            {
                                for (var i = 0; i < dataLength; i++)
                                {
                                    var i1 = i;
                                    foreach (var f in from f in GetType().GetFields()
                                                      let findF = string.Format("DevAddr{0}hRegAddr{1}h",
                                                      ValueHelper.GetHextStr(deviceAddr),
                                                      ValueHelper.GetHextStr((byte)(rxBytes[2] + i1)))
                                                      where f.Name.StartsWith(findF)
                                                      select f)
                                        f.SetValue(this, ValueHelper.GetHextStr(rxValues[i]));
                                }
                                return true;
                            }
                        }
                    }
                    else
                    {
                        if (rxBytes.Count == 1 + 1 + 1 + dataLength + 1 + 2)
                        {
                            var rxStatus = rxBytes[1 + 1 + 1 + dataLength + 1];
                            var rxCrc = rxBytes[1 + 1 + 1 + dataLength + 1 + 1];
                            if (CALC_CRC(_tps929120TiCrcInfo, new[] { rxStatus }) == rxCrc)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        #region LED控制

        private readonly List<DevLed> _devLedList = new List<DevLed>();
        private readonly object _lockDevList = new object();
        private ushort[] _broadCastModePwmOut = new ushort[] { 65295, 65295, 65295, 65295, 65295, 65295, 65295, 65295, 65295, 65295, 65295, 65295 };

        [Description("设置对应地址的对应序号的LED的占空比")]
        public void SetDevLedPwm(int devAddr, int ledIndex, int percent)
        {
            lock (_lockDevList)
            {
                if (devAddr >= 0 && devAddr <= 15 && percent >= 0 && percent <= 100 && ledIndex >= 1 && ledIndex <= 12)
                {
                    var devIndex = _devLedList.FindIndex(f => f.DevId == devAddr);
                    if (devIndex != -1)
                    {
                        _devLedList[devIndex].SetPwm(ledIndex, (ushort)percent);
                    }
                }
            }
        }

        [Description("设置广播模式下的对应序号的LED的占空比")]
        public void SetBroadcastModePwmOut(int ledIndex, int percent)
        {
            if (percent >= 0 && percent <= 100 && ledIndex >= 1 && ledIndex <= 12)
            {
                _broadCastModePwmOut[ledIndex - 1] = TransferPwm((ushort)percent);
            }
        }

        [Description("添加需单独控制LED的地址")]
        public void AddDevLed(int devAddr)
        {
            lock (_lockDevList)
            {
                if (devAddr >= 0 && devAddr <= 15)
                {
                    if (_devLedList.FindIndex(f => f.DevId == devAddr) == -1)
                    {
                        _devLedList.Add(new DevLed { DevId = devAddr });
                    }
                }
            }
        }

        [Description("移除需单独控制LED的地址")]
        public void RemoveDevLed(int devAddr)
        {
            lock (_lockDevList)
            {
                if (devAddr >= 0 && devAddr <= 15)
                {
                    var devIndex = _devLedList.FindIndex(f => f.DevId == devAddr);
                    if (devIndex != -1)
                    {
                        _devLedList.RemoveAt(devIndex);
                    }
                }
            }
        }

        [Description("控制单独地址的第X颗LED打开")]
        public void SingleDevLedOn(int devAddr, int ledIndex)
        {
            lock (_lockDevList)
            {
                var devIndex = _devLedList.FindIndex(f => f.DevId == devAddr);
                if (devIndex != -1)
                {
                    switch (ledIndex)
                    {
                        case 1:
                            _devLedList[devIndex].Led1 = true;
                            break;

                        case 2:
                            _devLedList[devIndex].Led2 = true;
                            break;

                        case 3:
                            _devLedList[devIndex].Led3 = true;
                            break;

                        case 4:
                            _devLedList[devIndex].Led4 = true;
                            break;

                        case 5:
                            _devLedList[devIndex].Led5 = true;
                            break;

                        case 6:
                            _devLedList[devIndex].Led6 = true;
                            break;

                        case 7:
                            _devLedList[devIndex].Led7 = true;
                            break;

                        case 8:
                            _devLedList[devIndex].Led8 = true;
                            break;

                        case 9:
                            _devLedList[devIndex].Led9 = true;
                            break;

                        case 10:
                            _devLedList[devIndex].Led10 = true;
                            break;

                        case 11:
                            _devLedList[devIndex].Led11 = true;
                            break;

                        case 12:
                            _devLedList[devIndex].Led12 = true;
                            break;
                    }
                }
            }
        }

        [Description("控制单独地址的第X颗LED关闭")]
        public void SingleDevLedOff(int devAddr, int ledIndex)
        {
            lock (_lockDevList)
            {
                var devIndex = _devLedList.FindIndex(f => f.DevId == devAddr);
                if (devIndex != -1)
                {
                    switch (ledIndex)
                    {
                        case 1:
                            _devLedList[devIndex].Led1 = false;
                            break;

                        case 2:
                            _devLedList[devIndex].Led2 = false;
                            break;

                        case 3:
                            _devLedList[devIndex].Led2 = false;
                            break;

                        case 4:
                            _devLedList[devIndex].Led4 = false;
                            break;

                        case 5:
                            _devLedList[devIndex].Led5 = false;
                            break;

                        case 6:
                            _devLedList[devIndex].Led6 = false;
                            break;

                        case 7:
                            _devLedList[devIndex].Led7 = false;
                            break;

                        case 8:
                            _devLedList[devIndex].Led8 = false;
                            break;

                        case 9:
                            _devLedList[devIndex].Led9 = false;
                            break;

                        case 10:
                            _devLedList[devIndex].Led10 = false;
                            break;

                        case 11:
                            _devLedList[devIndex].Led11 = false;
                            break;

                        case 12:
                            _devLedList[devIndex].Led12 = false;
                            break;
                    }
                }
            }
        }

        [Description("R/W,LED1")]
        public bool Led1;

        [Description("R/W,LED2")]
        public bool Led2;

        [Description("R/W,LED3")]
        public bool Led3;

        [Description("R/W,LED4")]
        public bool Led4;

        [Description("R/W,LED5")]
        public bool Led5;

        [Description("R/W,LED6")]
        public bool Led6;

        [Description("R/W,LED7")]
        public bool Led7;

        [Description("R/W,LED8")]
        public bool Led8;

        [Description("R/W,LED9")]
        public bool Led9;

        [Description("R/W,LED10")]
        public bool Led10;

        [Description("R/W,LED11")]
        public bool Led11;

        [Description("R/W,LED12")]
        public bool Led12;

        [Description("开启LED控制模式")]
        public void StartLedControl()
        {
            Led1 = false;
            Led2 = false;
            Led3 = false;
            Led4 = false;
            Led5 = false;
            Led6 = false;
            Led7 = false;
            Led8 = false;
            Led9 = false;
            Led10 = false;
            Led11 = false;
            Led12 = false;

            lock (_lockDevList)
            {
                foreach (var dev in _devLedList)
                {
                    dev.LedAffOff();
                }
            }

            // 寄存器解锁 
            MySerialPort.SendCommand(GetSendData(0xC0, 0x61, new byte[] { 0x00 }));
            // CLEAR FAULT
            MySerialPort.SendCommand(GetSendData(0xC0, 0x60, new byte[] { 0x03 }));
            _isLedControl = true;
        }

        [Description("关闭LED控制模式")]
        public void StopLedControl()
        {
            Led1 = false;
            Led2 = false;
            Led3 = false;
            Led4 = false;
            Led5 = false;
            Led6 = false;
            Led7 = false;
            Led8 = false;
            Led9 = false;
            Led10 = false;
            Led11 = false;
            Led12 = false;

            lock (_lockDevList)
            {
                foreach (var dev in _devLedList)
                {
                    dev.LedAffOff();
                }
            }

            _isLedControl = false;
        }

        [Description("打开所有双数LED")]
        public void LedEvenOn()
        {
            if (!_isLedControl)
                return;

            lock (_lockDevList)
            {
                foreach (var dev in _devLedList)
                {
                    dev.LedEvenOn();
                }
            }

            Led2 = true;
            Led4 = true;
            Led6 = true;
            Led8 = true;
            Led10 = true;
            Led12 = true;
        }

        [Description("打开所有单数LED")]
        public void LedOddOn()
        {
            if (!_isLedControl)
                return;

            lock (_lockDevList)
            {
                foreach (var dev in _devLedList)
                {
                    dev.LedOddOn();
                }
            }

            Led1 = true;
            Led3 = true;
            Led5 = true;
            Led7 = true;
            Led9 = true;
            Led11 = true;
        }

        [Description("打开所有LED")]
        public void LedAllOn()
        {
            if (!_isLedControl)
                return;

            lock (_lockDevList)
            {
                foreach (var dev in _devLedList)
                {
                    dev.LedAllOn();
                }
            }

            Led2 = true;
            Led4 = true;
            Led6 = true;
            Led8 = true;
            Led10 = true;
            Led12 = true;

            Led1 = true;
            Led3 = true;
            Led5 = true;
            Led7 = true;
            Led9 = true;
            Led11 = true;
        }

        [Description("关闭所有LED")]
        public void LedAffOff()
        {
            if (!_isLedControl)
                return;

            lock (_lockDevList)
            {
                foreach (var dev in _devLedList)
                {
                    dev.LedAffOff();
                }
            }

            Led2 = false;
            Led4 = false;
            Led6 = false;
            Led8 = false;
            Led10 = false;
            Led12 = false;

            Led1 = false;
            Led3 = false;
            Led5 = false;
            Led7 = false;
            Led9 = false;
            Led11 = false;
        }

        private int _sendCount;

        private void LedControl()
        {
            while (_ledControlTh.IsAlive)
            {
                if (!_ledControlTh.IsAlive)
                    break;

                if (MySerialPort == null)
                {
                    _sendCount = 0;
                    continue;
                }

                if (!_isLedControl)
                {
                    _sendCount = 0;
                    continue;
                }

                Thread.Sleep(5);
                //MySerialPort.SendCommand(new b);

                // 寄存器解锁 
                //if (_sendCount == 3 || _sendCount == 7 || _sendCount == 11 || _sendCount == 15)
                {
                    MySerialPort.SendCommand(GetSendData(0xC0, 0x61, new byte[] { 0x00 }));
                    MySerialPort.SendCommand(GetSendData(0xC0, 0x60, new byte[] { 0x03 }));
                }

                lock (_lockDevList)
                {
                    if (_devLedList.Any())
                    {
                        for (var i = 0; i < _devLedList.Count; i++)
                        {
                            var dev = _devLedList[i];

                            //if ((_sendCount >= 0 && _sendCount <= 3 && i >= 0 && i <= 3) ||
                            //    (_sendCount >= 4 && _sendCount <= 7 && i >= 4 && i <= 7) ||
                            //    (_sendCount >= 8 && _sendCount <= 11 && i >= 8 && i <= 11) ||
                            //    (_sendCount >= 12 && _sendCount <= 15 && i >= 12 && i <= 15))
                            {
                                PwmOut0To11(dev);

                                // CONF_EN0, 50h, CH0~CH7
                                var str1 = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                                    dev.Led8 ? "1" : "0", dev.Led7 ? "1" : "0", dev.Led6 ? "1" : "0", dev.Led5 ? "1" : "0", dev.Led4 ? "1" : "0",
                                    dev.Led3 ? "1" : "0", dev.Led2 ? "1" : "0", dev.Led1 ? "1" : "0");
                                // CONF_EN1, 51h, CH8~CH11
                                var str2 = string.Format("0000{0}{1}{2}{3}",
                                    dev.Led12 ? "1" : "0", dev.Led11 ? "1" : "0", dev.Led10 ? "1" : "0", dev.Led9 ? "1" : "0");
                                MySerialPort.SendCommand(GetSendData((byte)(0x90 + dev.DevId), 0x50, new[] { Convert.ToByte(str1, 2), Convert.ToByte(str2, 2), }));
                            }
                        }
                    }
                    else
                    {
                        PwmOut0To11(null, true);

                        // CONF_EN0, 50h, CH0~CH7
                        var str1 = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                            Led8 ? "1" : "0", Led7 ? "1" : "0", Led6 ? "1" : "0", Led5 ? "1" : "0", Led4 ? "1" : "0",
                            Led3 ? "1" : "0", Led2 ? "1" : "0", Led1 ? "1" : "0");
                        // CONF_EN1, 51h, CH8~CH11
                        var str2 = string.Format("0000{0}{1}{2}{3}",
                            Led12 ? "1" : "0", Led11 ? "1" : "0", Led10 ? "1" : "0", Led9 ? "1" : "0");

                        MySerialPort.SendCommand(GetSendData(0xD0, 0x50, new[] { Convert.ToByte(str1, 2), Convert.ToByte(str2, 2) }));
                    }

                    _sendCount++;
                    if (_sendCount == 16 || _sendCount == _devLedList.Count)
                        _sendCount = 0;
                }
            }
        }

        private void Iout0To11(byte devAddr, byte confOut, bool isBroadcast = false)
        {
            //var confOutData = new byte[12];
            //for (var i = 0; i < confOutData.Length; i++)
            //    confOutData[i] = confOut;

            //if (!isBroadcast)
            //{
            //    MySerialPort.SendCommand(GetSendData((byte)(0xB0 + devAddr), 0x00, new byte[] { confOutData[0], confOutData[1], confOutData[2], confOutData[3], confOutData[4], confOutData[5], confOutData[6], confOutData[7] }));
            //    MySerialPort.SendCommand(GetSendData((byte)(0xA0 + devAddr), 0x08, new byte[] { confOutData[8], confOutData[9], confOutData[10], confOutData[11] }));
            //}
            //else
            //{
            //    MySerialPort.SendCommand(GetSendData((byte)(0xF0), 0x00, new byte[] { confOutData[0], confOutData[1], confOutData[2], confOutData[3], confOutData[4], confOutData[5], confOutData[6], confOutData[7] }));
            //    MySerialPort.SendCommand(GetSendData((byte)(0xE0), 0x08, new byte[] { confOutData[8], confOutData[9], confOutData[10], confOutData[11] }));
            //}
        }

        private void PwmOut0To11(DevLed dev, bool isBroadcast = false)
        {
            if (!isBroadcast)
            {
                var devAddr = (byte)dev.DevId;

                var confPwmOut = new byte[12];
                var confPwmLowOut = new byte[12];

                var bs1 = BitConverter.GetBytes(dev.Pwm1);
                var bs2 = BitConverter.GetBytes(dev.Pwm2);
                var bs3 = BitConverter.GetBytes(dev.Pwm3);
                var bs4 = BitConverter.GetBytes(dev.Pwm4);
                var bs5 = BitConverter.GetBytes(dev.Pwm5);
                var bs6 = BitConverter.GetBytes(dev.Pwm6);
                var bs7 = BitConverter.GetBytes(dev.Pwm7);
                var bs8 = BitConverter.GetBytes(dev.Pwm8);
                var bs9 = BitConverter.GetBytes(dev.Pwm9);
                var bs10 = BitConverter.GetBytes(dev.Pwm10);
                var bs11 = BitConverter.GetBytes(dev.Pwm11);
                var bs12 = BitConverter.GetBytes(dev.Pwm12);
                confPwmOut[0] = bs1[1];
                confPwmOut[1] = bs2[1];
                confPwmOut[2] = bs3[1];
                confPwmOut[3] = bs4[1];
                confPwmOut[4] = bs5[1];
                confPwmOut[5] = bs6[1];
                confPwmOut[6] = bs7[1];
                confPwmOut[7] = bs8[1];
                confPwmOut[8] = bs9[1];
                confPwmOut[9] = bs10[1];
                confPwmOut[10] = bs11[1];
                confPwmOut[11] = bs12[1];
                confPwmLowOut[0] = bs1[0];
                confPwmLowOut[1] = bs2[0];
                confPwmLowOut[2] = bs3[0];
                confPwmLowOut[3] = bs4[0];
                confPwmLowOut[4] = bs5[0];
                confPwmLowOut[5] = bs6[0];
                confPwmLowOut[6] = bs7[0];
                confPwmLowOut[7] = bs8[0];
                confPwmLowOut[8] = bs9[0];
                confPwmLowOut[9] = bs10[0];
                confPwmLowOut[10] = bs11[0];
                confPwmLowOut[11] = bs12[0];

                MySerialPort.SendCommand(GetSendData((byte)(0xB0 + devAddr), 0x20, new[] { confPwmOut[0], confPwmOut[1], confPwmOut[2], confPwmOut[3], confPwmOut[4], confPwmOut[5], confPwmOut[6], confPwmOut[7] }));
                MySerialPort.SendCommand(GetSendData((byte)(0xA0 + devAddr), 0x28, new[] { confPwmOut[8], confPwmOut[9], confPwmOut[10], confPwmOut[11] }));
                MySerialPort.SendCommand(GetSendData((byte)(0xB0 + devAddr), 0x40, new[] { confPwmLowOut[0], confPwmLowOut[1], confPwmLowOut[2], confPwmLowOut[3], confPwmLowOut[4], confPwmLowOut[5], confPwmLowOut[6], confPwmLowOut[7] }));
                MySerialPort.SendCommand(GetSendData((byte)(0xA0 + devAddr), 0x48, new[] { confPwmLowOut[8], confPwmLowOut[9], confPwmLowOut[10], confPwmLowOut[11] }));
            }
            else
            {
                var confPwmOut = new byte[12];
                var confPwmLowOut = new byte[12];

                var bs1 = BitConverter.GetBytes(_broadCastModePwmOut[0]);
                var bs2 = BitConverter.GetBytes(_broadCastModePwmOut[1]);
                var bs3 = BitConverter.GetBytes(_broadCastModePwmOut[2]);
                var bs4 = BitConverter.GetBytes(_broadCastModePwmOut[3]);
                var bs5 = BitConverter.GetBytes(_broadCastModePwmOut[4]);
                var bs6 = BitConverter.GetBytes(_broadCastModePwmOut[5]);
                var bs7 = BitConverter.GetBytes(_broadCastModePwmOut[6]);
                var bs8 = BitConverter.GetBytes(_broadCastModePwmOut[7]);
                var bs9 = BitConverter.GetBytes(_broadCastModePwmOut[8]);
                var bs10 = BitConverter.GetBytes(_broadCastModePwmOut[9]);
                var bs11 = BitConverter.GetBytes(_broadCastModePwmOut[10]);
                var bs12 = BitConverter.GetBytes(_broadCastModePwmOut[11]);
                confPwmOut[0] = bs1[1];
                confPwmOut[1] = bs2[1];
                confPwmOut[2] = bs3[1];
                confPwmOut[3] = bs4[1];
                confPwmOut[4] = bs5[1];
                confPwmOut[5] = bs6[1];
                confPwmOut[6] = bs7[1];
                confPwmOut[7] = bs8[1];
                confPwmOut[8] = bs9[1];
                confPwmOut[9] = bs10[1];
                confPwmOut[10] = bs11[1];
                confPwmOut[11] = bs12[1];
                confPwmLowOut[0] = bs1[0];
                confPwmLowOut[1] = bs2[0];
                confPwmLowOut[2] = bs3[0];
                confPwmLowOut[3] = bs4[0];
                confPwmLowOut[4] = bs5[0];
                confPwmLowOut[6] = bs7[0];
                confPwmLowOut[7] = bs8[0];
                confPwmLowOut[8] = bs9[0];
                confPwmLowOut[9] = bs10[0];
                confPwmLowOut[10] = bs11[0];
                confPwmLowOut[11] = bs12[0];

                MySerialPort.SendCommand(GetSendData(0xF0, 0x20, new[] { confPwmOut[0], confPwmOut[1], confPwmOut[2], confPwmOut[3], confPwmOut[4], confPwmOut[5], confPwmOut[6], confPwmOut[7] }));
                MySerialPort.SendCommand(GetSendData(0xE0, 0x28, new[] { confPwmOut[8], confPwmOut[9], confPwmOut[10], confPwmOut[11] }));
                MySerialPort.SendCommand(GetSendData(0xF0, 0x40, new[] { confPwmLowOut[0], confPwmLowOut[1], confPwmLowOut[2], confPwmLowOut[3], confPwmLowOut[4], confPwmLowOut[5], confPwmLowOut[6], confPwmLowOut[7] }));
                MySerialPort.SendCommand(GetSendData(0xE0, 0x48, new[] { confPwmLowOut[8], confPwmLowOut[9], confPwmLowOut[10], confPwmLowOut[11] }));
            }
        }

        internal class DevLed
        {
            public int DevId;

            public bool Led1;
            public bool Led2;
            public bool Led3;
            public bool Led4;
            public bool Led5;
            public bool Led6;
            public bool Led7;
            public bool Led8;
            public bool Led9;
            public bool Led10;
            public bool Led11;
            public bool Led12;

            public ushort Iout1;
            public ushort Iout2;
            public ushort Iout3;
            public ushort Iout4;
            public ushort Iout5;
            public ushort Iout6;
            public ushort Iout7;
            public ushort Iout8;
            public ushort Iout9;
            public ushort Iout10;
            public ushort Iout11;
            public ushort Iout12;

            public ushort Pwm1 = 65295;
            public ushort Pwm2 = 65295;
            public ushort Pwm3 = 65295;
            public ushort Pwm4 = 65295;
            public ushort Pwm5 = 65295;
            public ushort Pwm6 = 65295;
            public ushort Pwm7 = 65295;
            public ushort Pwm8 = 65295;
            public ushort Pwm9 = 65295;
            public ushort Pwm10 = 65295;
            public ushort Pwm11 = 65295;
            public ushort Pwm12 = 65295;

            /// <summary>
            /// 打开所有双数LED
            /// </summary>
            public void LedEvenOn()
            {
                Led2 = true;
                Led4 = true;
                Led6 = true;
                Led8 = true;
                Led10 = true;
                Led12 = true;
            }

            /// <summary>
            /// 打开所有单数LED
            /// </summary>
            public void LedOddOn()
            {
                Led1 = true;
                Led3 = true;
                Led5 = true;
                Led7 = true;
                Led9 = true;
                Led11 = true;
            }

            /// <summary>
            /// 打开所有LED
            /// </summary>
            public void LedAllOn()
            {
                Led2 = true;
                Led4 = true;
                Led6 = true;
                Led8 = true;
                Led10 = true;
                Led12 = true;

                Led1 = true;
                Led3 = true;
                Led5 = true;
                Led7 = true;
                Led9 = true;
                Led11 = true;
            }

            /// <summary>
            /// 关闭所有LED
            /// </summary>
            public void LedAffOff()
            {
                Led2 = false;
                Led4 = false;
                Led6 = false;
                Led8 = false;
                Led10 = false;
                Led12 = false;

                Led1 = false;
                Led3 = false;
                Led5 = false;
                Led7 = false;
                Led9 = false;
                Led11 = false;
            }

            public void SetPwm(int index, ushort percent)
            {
                var pwmOut = TransferPwm(percent);

                var bs = BitConverter.GetBytes(pwmOut);
                if (index == 1)
                    Pwm1 = pwmOut;
                else if (index == 2)
                    Pwm2 = pwmOut;
                else if (index == 3)
                    Pwm3 = pwmOut;
                else if (index == 4)
                    Pwm4 = pwmOut;
                else if (index == 5)
                    Pwm5 = pwmOut;
                else if (index == 6)
                    Pwm6 = pwmOut;
                else if (index == 7)
                    Pwm7 = pwmOut;
                else if (index == 8)
                    Pwm8 = pwmOut;
                else if (index == 9)
                    Pwm9 = pwmOut;
                else if (index == 10)
                    Pwm10 = pwmOut;
                else if (index == 11)
                    Pwm11 = pwmOut;
                else if (index == 12)
                    Pwm12 = pwmOut;
            }

            public void SetIout()
            {

            }
        }

        private static ushort TransferPwm(ushort percent)
        {
            if (percent == 0)
                return 0;

            var pwmOut = (ushort)(((percent / 100f) * 4096) - 1);

            if (pwmOut <= 15)
                return pwmOut;

            var rest = pwmOut % 16;
            if (rest == 0)
                return (ushort)(pwmOut / 16 + 0);
            else
                return (ushort)((pwmOut / 16) * 256 + rest);
        }

        #endregion

        #region REG字段

        [Description("R,DevAddr00hRegAddr00hIout0")]
        public string DevAddr00hRegAddr00hIout0;
        [Description("R,DevAddr00hRegAddr01hIout1")]
        public string DevAddr00hRegAddr01hIout1;
        [Description("R,DevAddr00hRegAddr02hIout2")]
        public string DevAddr00hRegAddr02hIout2;
        [Description("R,DevAddr00hRegAddr03hIout3")]
        public string DevAddr00hRegAddr03hIout3;
        [Description("R,DevAddr00hRegAddr04hIout4")]
        public string DevAddr00hRegAddr04hIout4;
        [Description("R,DevAddr00hRegAddr05hIout5")]
        public string DevAddr00hRegAddr05hIout5;
        [Description("R,DevAddr00hRegAddr06hIout6")]
        public string DevAddr00hRegAddr06hIout6;
        [Description("R,DevAddr00hRegAddr07hIout7")]
        public string DevAddr00hRegAddr07hIout7;
        [Description("R,DevAddr00hRegAddr08hIout8")]
        public string DevAddr00hRegAddr08hIout8;
        [Description("R,DevAddr00hRegAddr09hIout9")]
        public string DevAddr00hRegAddr09hIout9;
        [Description("R,DevAddr00hRegAddr0AhIout10")]
        public string DevAddr00hRegAddr0AhIout10;
        [Description("R,DevAddr00hRegAddr0BhIout11")]
        public string DevAddr00hRegAddr0BhIout11;
        [Description("R,DevAddr00hRegAddr20hIoutPwm0")]
        public string DevAddr00hRegAddr20hIoutPwm0;
        [Description("R,DevAddr00hRegAddr21hIoutPwm1")]
        public string DevAddr00hRegAddr21hIoutPwm1;
        [Description("R,DevAddr00hRegAddr22hIoutPwm2")]
        public string DevAddr00hRegAddr22hIoutPwm2;
        [Description("R,DevAddr00hRegAddr23hIoutPwm3")]
        public string DevAddr00hRegAddr23hIoutPwm3;
        [Description("R,DevAddr00hRegAddr24hIoutPwm4")]
        public string DevAddr00hRegAddr24hIoutPwm4;
        [Description("R,DevAddr00hRegAddr25hIoutPwm5")]
        public string DevAddr00hRegAddr25hIoutPwm5;
        [Description("R,DevAddr00hRegAddr26hIoutPwm6")]
        public string DevAddr00hRegAddr26hIoutPwm6;
        [Description("R,DevAddr00hRegAddr27hIoutPwm7")]
        public string DevAddr00hRegAddr27hIoutPwm7;
        [Description("R,DevAddr00hRegAddr28hIoutPwm8")]
        public string DevAddr00hRegAddr28hIoutPwm8;
        [Description("R,DevAddr00hRegAddr29hIoutPwm9")]
        public string DevAddr00hRegAddr29hIoutPwm9;
        [Description("R,DevAddr00hRegAddr2AhIoutPwm10")]
        public string DevAddr00hRegAddr2AhIoutPwm10;
        [Description("R,DevAddr00hRegAddr2BhIoutPwm11")]
        public string DevAddr00hRegAddr2BhIoutPwm11;
        [Description("R,DevAddr00hRegAddr40hIoutPwml0")]
        public string DevAddr00hRegAddr40hIoutPwml0;
        [Description("R,DevAddr00hRegAddr41hIoutPwml1")]
        public string DevAddr00hRegAddr41hIoutPwml1;
        [Description("R,DevAddr00hRegAddr42hIoutPwml2")]
        public string DevAddr00hRegAddr42hIoutPwml2;
        [Description("R,DevAddr00hRegAddr43hIoutPwml3")]
        public string DevAddr00hRegAddr43hIoutPwml3;
        [Description("R,DevAddr00hRegAddr44hIoutPwml4")]
        public string DevAddr00hRegAddr44hIoutPwml4;
        [Description("R,DevAddr00hRegAddr45hIoutPwml5")]
        public string DevAddr00hRegAddr45hIoutPwml5;
        [Description("R,DevAddr00hRegAddr46hIoutPwml6")]
        public string DevAddr00hRegAddr46hIoutPwml6;
        [Description("R,DevAddr00hRegAddr47hIoutPwml7")]
        public string DevAddr00hRegAddr47hIoutPwml7;
        [Description("R,DevAddr00hRegAddr48hIoutPwml8")]
        public string DevAddr00hRegAddr48hIoutPwml8;
        [Description("R,DevAddr00hRegAddr49hIoutPwml9")]
        public string DevAddr00hRegAddr49hIoutPwml9;
        [Description("R,DevAddr00hRegAddr4AhIoutPwml10")]
        public string DevAddr00hRegAddr4AhIoutPwml10;
        [Description("R,DevAddr00hRegAddr4BhIoutPwml11")]
        public string DevAddr00hRegAddr4BhIoutPwml11;
        [Description("R,DevAddr00hRegAddr50hConfEn0")]
        public string DevAddr00hRegAddr50hConfEn0;
        [Description("R,DevAddr00hRegAddr51hConfEn1")]
        public string DevAddr00hRegAddr51hConfEn1;
        [Description("R,DevAddr00hRegAddr54hConfDiagen0")]
        public string DevAddr00hRegAddr54hConfDiagen0;
        [Description("R,DevAddr00hRegAddr55hConfDiagen1")]
        public string DevAddr00hRegAddr55hConfDiagen1;
        [Description("R,DevAddr00hRegAddr56hConfMisc0")]
        public string DevAddr00hRegAddr56hConfMisc0;
        [Description("R,DevAddr00hRegAddr57hConfMisc1")]
        public string DevAddr00hRegAddr57hConfMisc1;
        [Description("R,DevAddr00hRegAddr58hConfMisc2")]
        public string DevAddr00hRegAddr58hConfMisc2;
        [Description("R,DevAddr00hRegAddr59hConfMisc3")]
        public string DevAddr00hRegAddr59hConfMisc3;
        [Description("R,DevAddr00hRegAddr5AhConfMisc4")]
        public string DevAddr00hRegAddr5AhConfMisc4;
        [Description("R,DevAddr00hRegAddr5BhConfMisc5")]
        public string DevAddr00hRegAddr5BhConfMisc5;
        [Description("R,DevAddr00hRegAddr60hClr")]
        public string DevAddr00hRegAddr60hClr;
        [Description("R,DevAddr00hRegAddr61hConfLock")]
        public string DevAddr00hRegAddr61hConfLock;
        [Description("R,DevAddr00hRegAddr62hConfMisc6")]
        public string DevAddr00hRegAddr62hConfMisc6;
        [Description("R,DevAddr00hRegAddr63hConfMisc7")]
        public string DevAddr00hRegAddr63hConfMisc7;
        [Description("R,DevAddr00hRegAddr64hConfMisc8")]
        public string DevAddr00hRegAddr64hConfMisc8;
        [Description("R,DevAddr00hRegAddr65hConfMisc9")]
        public string DevAddr00hRegAddr65hConfMisc9;
        [Description("R,DevAddr00hRegAddr70hFlag0")]
        public string DevAddr00hRegAddr70hFlag0;
        [Description("R,DevAddr00hRegAddr71hFlag1")]
        public string DevAddr00hRegAddr71hFlag1;
        [Description("R,DevAddr00hRegAddr72hFlag2")]
        public string DevAddr00hRegAddr72hFlag2;
        [Description("R,DevAddr00hRegAddr73hFlag3")]
        public string DevAddr00hRegAddr73hFlag3;
        [Description("R,DevAddr00hRegAddr74hFlag4")]
        public string DevAddr00hRegAddr74hFlag4;
        [Description("R,DevAddr00hRegAddr75hFlag5")]
        public string DevAddr00hRegAddr75hFlag5;
        [Description("R,DevAddr00hRegAddr77hFlag7")]
        public string DevAddr00hRegAddr77hFlag7;
        [Description("R,DevAddr00hRegAddr78hFlag8")]
        public string DevAddr00hRegAddr78hFlag8;
        [Description("R,DevAddr00hRegAddr7BhFlag11")]
        public string DevAddr00hRegAddr7BhFlag11;
        [Description("R,DevAddr00hRegAddr7ChFlag12")]
        public string DevAddr00hRegAddr7ChFlag12;
        [Description("R,DevAddr00hRegAddr7DhFlag13")]
        public string DevAddr00hRegAddr7DhFlag13;
        [Description("R,DevAddr00hRegAddr7EhFlag14")]
        public string DevAddr00hRegAddr7EhFlag14;
        [Description("R,DevAddr00hRegAddr80hEepi0")]
        public string DevAddr00hRegAddr80hEepi0;
        [Description("R,DevAddr00hRegAddr81hEepi1")]
        public string DevAddr00hRegAddr81hEepi1;
        [Description("R,DevAddr00hRegAddr82hEepi2")]
        public string DevAddr00hRegAddr82hEepi2;
        [Description("R,DevAddr00hRegAddr83hEepi3")]
        public string DevAddr00hRegAddr83hEepi3;
        [Description("R,DevAddr00hRegAddr84hEepi4")]
        public string DevAddr00hRegAddr84hEepi4;
        [Description("R,DevAddr00hRegAddr85hEepi5")]
        public string DevAddr00hRegAddr85hEepi5;
        [Description("R,DevAddr00hRegAddr86hEepi6")]
        public string DevAddr00hRegAddr86hEepi6;
        [Description("R,DevAddr00hRegAddr87hEepi7")]
        public string DevAddr00hRegAddr87hEepi7;
        [Description("R,DevAddr00hRegAddr88hEepi8")]
        public string DevAddr00hRegAddr88hEepi8;
        [Description("R,DevAddr00hRegAddr89hEepi9")]
        public string DevAddr00hRegAddr89hEepi9;
        [Description("R,DevAddr00hRegAddr8AhEepi10")]
        public string DevAddr00hRegAddr8AhEepi10;
        [Description("R,DevAddr00hRegAddr8BhEepi11")]
        public string DevAddr00hRegAddr8BhEepi11;
        [Description("R,DevAddr00hRegAddrA0hEepp0")]
        public string DevAddr00hRegAddrA0hEepp0;
        [Description("R,DevAddr00hRegAddrA1hEepp1")]
        public string DevAddr00hRegAddrA1hEepp1;
        [Description("R,DevAddr00hRegAddrA2hEepp2")]
        public string DevAddr00hRegAddrA2hEepp2;
        [Description("R,DevAddr00hRegAddrA3hEepp3")]
        public string DevAddr00hRegAddrA3hEepp3;
        [Description("R,DevAddr00hRegAddrA4hEepp4")]
        public string DevAddr00hRegAddrA4hEepp4;
        [Description("R,DevAddr00hRegAddrA5hEepp5")]
        public string DevAddr00hRegAddrA5hEepp5;
        [Description("R,DevAddr00hRegAddrA6hEepp6")]
        public string DevAddr00hRegAddrA6hEepp6;
        [Description("R,DevAddr00hRegAddrA7hEepp7")]
        public string DevAddr00hRegAddrA7hEepp7;
        [Description("R,DevAddr00hRegAddrA8hEepp8")]
        public string DevAddr00hRegAddrA8hEepp8;
        [Description("R,DevAddr00hRegAddrA9hEepp9")]
        public string DevAddr00hRegAddrA9hEepp9;
        [Description("R,DevAddr00hRegAddrAAhEepp10")]
        public string DevAddr00hRegAddrAAhEepp10;
        [Description("R,DevAddr00hRegAddrABhEepp11")]
        public string DevAddr00hRegAddrABhEepp11;
        [Description("R,DevAddr00hRegAddrC0hEepm0")]
        public string DevAddr00hRegAddrC0hEepm0;
        [Description("R,DevAddr00hRegAddrC1hEepm1")]
        public string DevAddr00hRegAddrC1hEepm1;
        [Description("R,DevAddr00hRegAddrC2hEepm2")]
        public string DevAddr00hRegAddrC2hEepm2;
        [Description("R,DevAddr00hRegAddrC3hEepm3")]
        public string DevAddr00hRegAddrC3hEepm3;
        [Description("R,DevAddr00hRegAddrC4hEepm4")]
        public string DevAddr00hRegAddrC4hEepm4;
        [Description("R,DevAddr00hRegAddrC5hEepm5")]
        public string DevAddr00hRegAddrC5hEepm5;
        [Description("R,DevAddr00hRegAddrC6hEepm6")]
        public string DevAddr00hRegAddrC6hEepm6;
        [Description("R,DevAddr00hRegAddrC7hEepm7")]
        public string DevAddr00hRegAddrC7hEepm7;
        [Description("R,DevAddr00hRegAddrC8hEepm8")]
        public string DevAddr00hRegAddrC8hEepm8;
        [Description("R,DevAddr00hRegAddrC9hEepm9")]
        public string DevAddr00hRegAddrC9hEepm9;
        [Description("R,DevAddr00hRegAddrCAhEepm10")]
        public string DevAddr00hRegAddrCAhEepm10;
        [Description("R,DevAddr00hRegAddrCBhEepm11")]
        public string DevAddr00hRegAddrCBhEepm11;
        [Description("R,DevAddr00hRegAddrCChEepm12")]
        public string DevAddr00hRegAddrCChEepm12;
        [Description("R,DevAddr00hRegAddrCDhEepm13")]
        public string DevAddr00hRegAddrCDhEepm13;
        [Description("R,DevAddr00hRegAddrCEhEepm14")]
        public string DevAddr00hRegAddrCEhEepm14;
        [Description("R,DevAddr00hRegAddrCFhEepm15")]
        public string DevAddr00hRegAddrCFhEepm15;

        [Description("R,DevAddr01hRegAddr00hIout0")]
        public string DevAddr01hRegAddr00hIout0;
        [Description("R,DevAddr01hRegAddr01hIout1")]
        public string DevAddr01hRegAddr01hIout1;
        [Description("R,DevAddr01hRegAddr02hIout2")]
        public string DevAddr01hRegAddr02hIout2;
        [Description("R,DevAddr01hRegAddr03hIout3")]
        public string DevAddr01hRegAddr03hIout3;
        [Description("R,DevAddr01hRegAddr04hIout4")]
        public string DevAddr01hRegAddr04hIout4;
        [Description("R,DevAddr01hRegAddr05hIout5")]
        public string DevAddr01hRegAddr05hIout5;
        [Description("R,DevAddr01hRegAddr06hIout6")]
        public string DevAddr01hRegAddr06hIout6;
        [Description("R,DevAddr01hRegAddr07hIout7")]
        public string DevAddr01hRegAddr07hIout7;
        [Description("R,DevAddr01hRegAddr08hIout8")]
        public string DevAddr01hRegAddr08hIout8;
        [Description("R,DevAddr01hRegAddr09hIout9")]
        public string DevAddr01hRegAddr09hIout9;
        [Description("R,DevAddr01hRegAddr0AhIout10")]
        public string DevAddr01hRegAddr0AhIout10;
        [Description("R,DevAddr01hRegAddr0BhIout11")]
        public string DevAddr01hRegAddr0BhIout11;
        [Description("R,DevAddr01hRegAddr20hIoutPwm0")]
        public string DevAddr01hRegAddr20hIoutPwm0;
        [Description("R,DevAddr01hRegAddr21hIoutPwm1")]
        public string DevAddr01hRegAddr21hIoutPwm1;
        [Description("R,DevAddr01hRegAddr22hIoutPwm2")]
        public string DevAddr01hRegAddr22hIoutPwm2;
        [Description("R,DevAddr01hRegAddr23hIoutPwm3")]
        public string DevAddr01hRegAddr23hIoutPwm3;
        [Description("R,DevAddr01hRegAddr24hIoutPwm4")]
        public string DevAddr01hRegAddr24hIoutPwm4;
        [Description("R,DevAddr01hRegAddr25hIoutPwm5")]
        public string DevAddr01hRegAddr25hIoutPwm5;
        [Description("R,DevAddr01hRegAddr26hIoutPwm6")]
        public string DevAddr01hRegAddr26hIoutPwm6;
        [Description("R,DevAddr01hRegAddr27hIoutPwm7")]
        public string DevAddr01hRegAddr27hIoutPwm7;
        [Description("R,DevAddr01hRegAddr28hIoutPwm8")]
        public string DevAddr01hRegAddr28hIoutPwm8;
        [Description("R,DevAddr01hRegAddr29hIoutPwm9")]
        public string DevAddr01hRegAddr29hIoutPwm9;
        [Description("R,DevAddr01hRegAddr2AhIoutPwm10")]
        public string DevAddr01hRegAddr2AhIoutPwm10;
        [Description("R,DevAddr01hRegAddr2BhIoutPwm11")]
        public string DevAddr01hRegAddr2BhIoutPwm11;
        [Description("R,DevAddr01hRegAddr40hIoutPwml0")]
        public string DevAddr01hRegAddr40hIoutPwml0;
        [Description("R,DevAddr01hRegAddr41hIoutPwml1")]
        public string DevAddr01hRegAddr41hIoutPwml1;
        [Description("R,DevAddr01hRegAddr42hIoutPwml2")]
        public string DevAddr01hRegAddr42hIoutPwml2;
        [Description("R,DevAddr01hRegAddr43hIoutPwml3")]
        public string DevAddr01hRegAddr43hIoutPwml3;
        [Description("R,DevAddr01hRegAddr44hIoutPwml4")]
        public string DevAddr01hRegAddr44hIoutPwml4;
        [Description("R,DevAddr01hRegAddr45hIoutPwml5")]
        public string DevAddr01hRegAddr45hIoutPwml5;
        [Description("R,DevAddr01hRegAddr46hIoutPwml6")]
        public string DevAddr01hRegAddr46hIoutPwml6;
        [Description("R,DevAddr01hRegAddr47hIoutPwml7")]
        public string DevAddr01hRegAddr47hIoutPwml7;
        [Description("R,DevAddr01hRegAddr48hIoutPwml8")]
        public string DevAddr01hRegAddr48hIoutPwml8;
        [Description("R,DevAddr01hRegAddr49hIoutPwml9")]
        public string DevAddr01hRegAddr49hIoutPwml9;
        [Description("R,DevAddr01hRegAddr4AhIoutPwml10")]
        public string DevAddr01hRegAddr4AhIoutPwml10;
        [Description("R,DevAddr01hRegAddr4BhIoutPwml11")]
        public string DevAddr01hRegAddr4BhIoutPwml11;
        [Description("R,DevAddr01hRegAddr50hConfEn0")]
        public string DevAddr01hRegAddr50hConfEn0;
        [Description("R,DevAddr01hRegAddr51hConfEn1")]
        public string DevAddr01hRegAddr51hConfEn1;
        [Description("R,DevAddr01hRegAddr54hConfDiagen0")]
        public string DevAddr01hRegAddr54hConfDiagen0;
        [Description("R,DevAddr01hRegAddr55hConfDiagen1")]
        public string DevAddr01hRegAddr55hConfDiagen1;
        [Description("R,DevAddr01hRegAddr56hConfMisc0")]
        public string DevAddr01hRegAddr56hConfMisc0;
        [Description("R,DevAddr01hRegAddr57hConfMisc1")]
        public string DevAddr01hRegAddr57hConfMisc1;
        [Description("R,DevAddr01hRegAddr58hConfMisc2")]
        public string DevAddr01hRegAddr58hConfMisc2;
        [Description("R,DevAddr01hRegAddr59hConfMisc3")]
        public string DevAddr01hRegAddr59hConfMisc3;
        [Description("R,DevAddr01hRegAddr5AhConfMisc4")]
        public string DevAddr01hRegAddr5AhConfMisc4;
        [Description("R,DevAddr01hRegAddr5BhConfMisc5")]
        public string DevAddr01hRegAddr5BhConfMisc5;
        [Description("R,DevAddr01hRegAddr60hClr")]
        public string DevAddr01hRegAddr60hClr;
        [Description("R,DevAddr01hRegAddr61hConfLock")]
        public string DevAddr01hRegAddr61hConfLock;
        [Description("R,DevAddr01hRegAddr62hConfMisc6")]
        public string DevAddr01hRegAddr62hConfMisc6;
        [Description("R,DevAddr01hRegAddr63hConfMisc7")]
        public string DevAddr01hRegAddr63hConfMisc7;
        [Description("R,DevAddr01hRegAddr64hConfMisc8")]
        public string DevAddr01hRegAddr64hConfMisc8;
        [Description("R,DevAddr01hRegAddr65hConfMisc9")]
        public string DevAddr01hRegAddr65hConfMisc9;
        [Description("R,DevAddr01hRegAddr70hFlag0")]
        public string DevAddr01hRegAddr70hFlag0;
        [Description("R,DevAddr01hRegAddr71hFlag1")]
        public string DevAddr01hRegAddr71hFlag1;
        [Description("R,DevAddr01hRegAddr72hFlag2")]
        public string DevAddr01hRegAddr72hFlag2;
        [Description("R,DevAddr01hRegAddr73hFlag3")]
        public string DevAddr01hRegAddr73hFlag3;
        [Description("R,DevAddr01hRegAddr74hFlag4")]
        public string DevAddr01hRegAddr74hFlag4;
        [Description("R,DevAddr01hRegAddr75hFlag5")]
        public string DevAddr01hRegAddr75hFlag5;
        [Description("R,DevAddr01hRegAddr77hFlag7")]
        public string DevAddr01hRegAddr77hFlag7;
        [Description("R,DevAddr01hRegAddr78hFlag8")]
        public string DevAddr01hRegAddr78hFlag8;
        [Description("R,DevAddr01hRegAddr7BhFlag11")]
        public string DevAddr01hRegAddr7BhFlag11;
        [Description("R,DevAddr01hRegAddr7ChFlag12")]
        public string DevAddr01hRegAddr7ChFlag12;
        [Description("R,DevAddr01hRegAddr7DhFlag13")]
        public string DevAddr01hRegAddr7DhFlag13;
        [Description("R,DevAddr01hRegAddr7EhFlag14")]
        public string DevAddr01hRegAddr7EhFlag14;
        [Description("R,DevAddr01hRegAddr80hEepi0")]
        public string DevAddr01hRegAddr80hEepi0;
        [Description("R,DevAddr01hRegAddr81hEepi1")]
        public string DevAddr01hRegAddr81hEepi1;
        [Description("R,DevAddr01hRegAddr82hEepi2")]
        public string DevAddr01hRegAddr82hEepi2;
        [Description("R,DevAddr01hRegAddr83hEepi3")]
        public string DevAddr01hRegAddr83hEepi3;
        [Description("R,DevAddr01hRegAddr84hEepi4")]
        public string DevAddr01hRegAddr84hEepi4;
        [Description("R,DevAddr01hRegAddr85hEepi5")]
        public string DevAddr01hRegAddr85hEepi5;
        [Description("R,DevAddr01hRegAddr86hEepi6")]
        public string DevAddr01hRegAddr86hEepi6;
        [Description("R,DevAddr01hRegAddr87hEepi7")]
        public string DevAddr01hRegAddr87hEepi7;
        [Description("R,DevAddr01hRegAddr88hEepi8")]
        public string DevAddr01hRegAddr88hEepi8;
        [Description("R,DevAddr01hRegAddr89hEepi9")]
        public string DevAddr01hRegAddr89hEepi9;
        [Description("R,DevAddr01hRegAddr8AhEepi10")]
        public string DevAddr01hRegAddr8AhEepi10;
        [Description("R,DevAddr01hRegAddr8BhEepi11")]
        public string DevAddr01hRegAddr8BhEepi11;
        [Description("R,DevAddr01hRegAddrA0hEepp0")]
        public string DevAddr01hRegAddrA0hEepp0;
        [Description("R,DevAddr01hRegAddrA1hEepp1")]
        public string DevAddr01hRegAddrA1hEepp1;
        [Description("R,DevAddr01hRegAddrA2hEepp2")]
        public string DevAddr01hRegAddrA2hEepp2;
        [Description("R,DevAddr01hRegAddrA3hEepp3")]
        public string DevAddr01hRegAddrA3hEepp3;
        [Description("R,DevAddr01hRegAddrA4hEepp4")]
        public string DevAddr01hRegAddrA4hEepp4;
        [Description("R,DevAddr01hRegAddrA5hEepp5")]
        public string DevAddr01hRegAddrA5hEepp5;
        [Description("R,DevAddr01hRegAddrA6hEepp6")]
        public string DevAddr01hRegAddrA6hEepp6;
        [Description("R,DevAddr01hRegAddrA7hEepp7")]
        public string DevAddr01hRegAddrA7hEepp7;
        [Description("R,DevAddr01hRegAddrA8hEepp8")]
        public string DevAddr01hRegAddrA8hEepp8;
        [Description("R,DevAddr01hRegAddrA9hEepp9")]
        public string DevAddr01hRegAddrA9hEepp9;
        [Description("R,DevAddr01hRegAddrAAhEepp10")]
        public string DevAddr01hRegAddrAAhEepp10;
        [Description("R,DevAddr01hRegAddrABhEepp11")]
        public string DevAddr01hRegAddrABhEepp11;
        [Description("R,DevAddr01hRegAddrC0hEepm0")]
        public string DevAddr01hRegAddrC0hEepm0;
        [Description("R,DevAddr01hRegAddrC1hEepm1")]
        public string DevAddr01hRegAddrC1hEepm1;
        [Description("R,DevAddr01hRegAddrC2hEepm2")]
        public string DevAddr01hRegAddrC2hEepm2;
        [Description("R,DevAddr01hRegAddrC3hEepm3")]
        public string DevAddr01hRegAddrC3hEepm3;
        [Description("R,DevAddr01hRegAddrC4hEepm4")]
        public string DevAddr01hRegAddrC4hEepm4;
        [Description("R,DevAddr01hRegAddrC5hEepm5")]
        public string DevAddr01hRegAddrC5hEepm5;
        [Description("R,DevAddr01hRegAddrC6hEepm6")]
        public string DevAddr01hRegAddrC6hEepm6;
        [Description("R,DevAddr01hRegAddrC7hEepm7")]
        public string DevAddr01hRegAddrC7hEepm7;
        [Description("R,DevAddr01hRegAddrC8hEepm8")]
        public string DevAddr01hRegAddrC8hEepm8;
        [Description("R,DevAddr01hRegAddrC9hEepm9")]
        public string DevAddr01hRegAddrC9hEepm9;
        [Description("R,DevAddr01hRegAddrCAhEepm10")]
        public string DevAddr01hRegAddrCAhEepm10;
        [Description("R,DevAddr01hRegAddrCBhEepm11")]
        public string DevAddr01hRegAddrCBhEepm11;
        [Description("R,DevAddr01hRegAddrCChEepm12")]
        public string DevAddr01hRegAddrCChEepm12;
        [Description("R,DevAddr01hRegAddrCDhEepm13")]
        public string DevAddr01hRegAddrCDhEepm13;
        [Description("R,DevAddr01hRegAddrCEhEepm14")]
        public string DevAddr01hRegAddrCEhEepm14;
        [Description("R,DevAddr01hRegAddrCFhEepm15")]
        public string DevAddr01hRegAddrCFhEepm15;

        [Description("R,DevAddr02hRegAddr00hIout0")]
        public string DevAddr02hRegAddr00hIout0;
        [Description("R,DevAddr02hRegAddr01hIout1")]
        public string DevAddr02hRegAddr01hIout1;
        [Description("R,DevAddr02hRegAddr02hIout2")]
        public string DevAddr02hRegAddr02hIout2;
        [Description("R,DevAddr02hRegAddr03hIout3")]
        public string DevAddr02hRegAddr03hIout3;
        [Description("R,DevAddr02hRegAddr04hIout4")]
        public string DevAddr02hRegAddr04hIout4;
        [Description("R,DevAddr02hRegAddr05hIout5")]
        public string DevAddr02hRegAddr05hIout5;
        [Description("R,DevAddr02hRegAddr06hIout6")]
        public string DevAddr02hRegAddr06hIout6;
        [Description("R,DevAddr02hRegAddr07hIout7")]
        public string DevAddr02hRegAddr07hIout7;
        [Description("R,DevAddr02hRegAddr08hIout8")]
        public string DevAddr02hRegAddr08hIout8;
        [Description("R,DevAddr02hRegAddr09hIout9")]
        public string DevAddr02hRegAddr09hIout9;
        [Description("R,DevAddr02hRegAddr0AhIout10")]
        public string DevAddr02hRegAddr0AhIout10;
        [Description("R,DevAddr02hRegAddr0BhIout11")]
        public string DevAddr02hRegAddr0BhIout11;
        [Description("R,DevAddr02hRegAddr20hIoutPwm0")]
        public string DevAddr02hRegAddr20hIoutPwm0;
        [Description("R,DevAddr02hRegAddr21hIoutPwm1")]
        public string DevAddr02hRegAddr21hIoutPwm1;
        [Description("R,DevAddr02hRegAddr22hIoutPwm2")]
        public string DevAddr02hRegAddr22hIoutPwm2;
        [Description("R,DevAddr02hRegAddr23hIoutPwm3")]
        public string DevAddr02hRegAddr23hIoutPwm3;
        [Description("R,DevAddr02hRegAddr24hIoutPwm4")]
        public string DevAddr02hRegAddr24hIoutPwm4;
        [Description("R,DevAddr02hRegAddr25hIoutPwm5")]
        public string DevAddr02hRegAddr25hIoutPwm5;
        [Description("R,DevAddr02hRegAddr26hIoutPwm6")]
        public string DevAddr02hRegAddr26hIoutPwm6;
        [Description("R,DevAddr02hRegAddr27hIoutPwm7")]
        public string DevAddr02hRegAddr27hIoutPwm7;
        [Description("R,DevAddr02hRegAddr28hIoutPwm8")]
        public string DevAddr02hRegAddr28hIoutPwm8;
        [Description("R,DevAddr02hRegAddr29hIoutPwm9")]
        public string DevAddr02hRegAddr29hIoutPwm9;
        [Description("R,DevAddr02hRegAddr2AhIoutPwm10")]
        public string DevAddr02hRegAddr2AhIoutPwm10;
        [Description("R,DevAddr02hRegAddr2BhIoutPwm11")]
        public string DevAddr02hRegAddr2BhIoutPwm11;
        [Description("R,DevAddr02hRegAddr40hIoutPwml0")]
        public string DevAddr02hRegAddr40hIoutPwml0;
        [Description("R,DevAddr02hRegAddr41hIoutPwml1")]
        public string DevAddr02hRegAddr41hIoutPwml1;
        [Description("R,DevAddr02hRegAddr42hIoutPwml2")]
        public string DevAddr02hRegAddr42hIoutPwml2;
        [Description("R,DevAddr02hRegAddr43hIoutPwml3")]
        public string DevAddr02hRegAddr43hIoutPwml3;
        [Description("R,DevAddr02hRegAddr44hIoutPwml4")]
        public string DevAddr02hRegAddr44hIoutPwml4;
        [Description("R,DevAddr02hRegAddr45hIoutPwml5")]
        public string DevAddr02hRegAddr45hIoutPwml5;
        [Description("R,DevAddr02hRegAddr46hIoutPwml6")]
        public string DevAddr02hRegAddr46hIoutPwml6;
        [Description("R,DevAddr02hRegAddr47hIoutPwml7")]
        public string DevAddr02hRegAddr47hIoutPwml7;
        [Description("R,DevAddr02hRegAddr48hIoutPwml8")]
        public string DevAddr02hRegAddr48hIoutPwml8;
        [Description("R,DevAddr02hRegAddr49hIoutPwml9")]
        public string DevAddr02hRegAddr49hIoutPwml9;
        [Description("R,DevAddr02hRegAddr4AhIoutPwml10")]
        public string DevAddr02hRegAddr4AhIoutPwml10;
        [Description("R,DevAddr02hRegAddr4BhIoutPwml11")]
        public string DevAddr02hRegAddr4BhIoutPwml11;
        [Description("R,DevAddr02hRegAddr50hConfEn0")]
        public string DevAddr02hRegAddr50hConfEn0;
        [Description("R,DevAddr02hRegAddr51hConfEn1")]
        public string DevAddr02hRegAddr51hConfEn1;
        [Description("R,DevAddr02hRegAddr54hConfDiagen0")]
        public string DevAddr02hRegAddr54hConfDiagen0;
        [Description("R,DevAddr02hRegAddr55hConfDiagen1")]
        public string DevAddr02hRegAddr55hConfDiagen1;
        [Description("R,DevAddr02hRegAddr56hConfMisc0")]
        public string DevAddr02hRegAddr56hConfMisc0;
        [Description("R,DevAddr02hRegAddr57hConfMisc1")]
        public string DevAddr02hRegAddr57hConfMisc1;
        [Description("R,DevAddr02hRegAddr58hConfMisc2")]
        public string DevAddr02hRegAddr58hConfMisc2;
        [Description("R,DevAddr02hRegAddr59hConfMisc3")]
        public string DevAddr02hRegAddr59hConfMisc3;
        [Description("R,DevAddr02hRegAddr5AhConfMisc4")]
        public string DevAddr02hRegAddr5AhConfMisc4;
        [Description("R,DevAddr02hRegAddr5BhConfMisc5")]
        public string DevAddr02hRegAddr5BhConfMisc5;
        [Description("R,DevAddr02hRegAddr60hClr")]
        public string DevAddr02hRegAddr60hClr;
        [Description("R,DevAddr02hRegAddr61hConfLock")]
        public string DevAddr02hRegAddr61hConfLock;
        [Description("R,DevAddr02hRegAddr62hConfMisc6")]
        public string DevAddr02hRegAddr62hConfMisc6;
        [Description("R,DevAddr02hRegAddr63hConfMisc7")]
        public string DevAddr02hRegAddr63hConfMisc7;
        [Description("R,DevAddr02hRegAddr64hConfMisc8")]
        public string DevAddr02hRegAddr64hConfMisc8;
        [Description("R,DevAddr02hRegAddr65hConfMisc9")]
        public string DevAddr02hRegAddr65hConfMisc9;
        [Description("R,DevAddr02hRegAddr70hFlag0")]
        public string DevAddr02hRegAddr70hFlag0;
        [Description("R,DevAddr02hRegAddr71hFlag1")]
        public string DevAddr02hRegAddr71hFlag1;
        [Description("R,DevAddr02hRegAddr72hFlag2")]
        public string DevAddr02hRegAddr72hFlag2;
        [Description("R,DevAddr02hRegAddr73hFlag3")]
        public string DevAddr02hRegAddr73hFlag3;
        [Description("R,DevAddr02hRegAddr74hFlag4")]
        public string DevAddr02hRegAddr74hFlag4;
        [Description("R,DevAddr02hRegAddr75hFlag5")]
        public string DevAddr02hRegAddr75hFlag5;
        [Description("R,DevAddr02hRegAddr77hFlag7")]
        public string DevAddr02hRegAddr77hFlag7;
        [Description("R,DevAddr02hRegAddr78hFlag8")]
        public string DevAddr02hRegAddr78hFlag8;
        [Description("R,DevAddr02hRegAddr7BhFlag11")]
        public string DevAddr02hRegAddr7BhFlag11;
        [Description("R,DevAddr02hRegAddr7ChFlag12")]
        public string DevAddr02hRegAddr7ChFlag12;
        [Description("R,DevAddr02hRegAddr7DhFlag13")]
        public string DevAddr02hRegAddr7DhFlag13;
        [Description("R,DevAddr02hRegAddr7EhFlag14")]
        public string DevAddr02hRegAddr7EhFlag14;
        [Description("R,DevAddr02hRegAddr80hEepi0")]
        public string DevAddr02hRegAddr80hEepi0;
        [Description("R,DevAddr02hRegAddr81hEepi1")]
        public string DevAddr02hRegAddr81hEepi1;
        [Description("R,DevAddr02hRegAddr82hEepi2")]
        public string DevAddr02hRegAddr82hEepi2;
        [Description("R,DevAddr02hRegAddr83hEepi3")]
        public string DevAddr02hRegAddr83hEepi3;
        [Description("R,DevAddr02hRegAddr84hEepi4")]
        public string DevAddr02hRegAddr84hEepi4;
        [Description("R,DevAddr02hRegAddr85hEepi5")]
        public string DevAddr02hRegAddr85hEepi5;
        [Description("R,DevAddr02hRegAddr86hEepi6")]
        public string DevAddr02hRegAddr86hEepi6;
        [Description("R,DevAddr02hRegAddr87hEepi7")]
        public string DevAddr02hRegAddr87hEepi7;
        [Description("R,DevAddr02hRegAddr88hEepi8")]
        public string DevAddr02hRegAddr88hEepi8;
        [Description("R,DevAddr02hRegAddr89hEepi9")]
        public string DevAddr02hRegAddr89hEepi9;
        [Description("R,DevAddr02hRegAddr8AhEepi10")]
        public string DevAddr02hRegAddr8AhEepi10;
        [Description("R,DevAddr02hRegAddr8BhEepi11")]
        public string DevAddr02hRegAddr8BhEepi11;
        [Description("R,DevAddr02hRegAddrA0hEepp0")]
        public string DevAddr02hRegAddrA0hEepp0;
        [Description("R,DevAddr02hRegAddrA1hEepp1")]
        public string DevAddr02hRegAddrA1hEepp1;
        [Description("R,DevAddr02hRegAddrA2hEepp2")]
        public string DevAddr02hRegAddrA2hEepp2;
        [Description("R,DevAddr02hRegAddrA3hEepp3")]
        public string DevAddr02hRegAddrA3hEepp3;
        [Description("R,DevAddr02hRegAddrA4hEepp4")]
        public string DevAddr02hRegAddrA4hEepp4;
        [Description("R,DevAddr02hRegAddrA5hEepp5")]
        public string DevAddr02hRegAddrA5hEepp5;
        [Description("R,DevAddr02hRegAddrA6hEepp6")]
        public string DevAddr02hRegAddrA6hEepp6;
        [Description("R,DevAddr02hRegAddrA7hEepp7")]
        public string DevAddr02hRegAddrA7hEepp7;
        [Description("R,DevAddr02hRegAddrA8hEepp8")]
        public string DevAddr02hRegAddrA8hEepp8;
        [Description("R,DevAddr02hRegAddrA9hEepp9")]
        public string DevAddr02hRegAddrA9hEepp9;
        [Description("R,DevAddr02hRegAddrAAhEepp10")]
        public string DevAddr02hRegAddrAAhEepp10;
        [Description("R,DevAddr02hRegAddrABhEepp11")]
        public string DevAddr02hRegAddrABhEepp11;
        [Description("R,DevAddr02hRegAddrC0hEepm0")]
        public string DevAddr02hRegAddrC0hEepm0;
        [Description("R,DevAddr02hRegAddrC1hEepm1")]
        public string DevAddr02hRegAddrC1hEepm1;
        [Description("R,DevAddr02hRegAddrC2hEepm2")]
        public string DevAddr02hRegAddrC2hEepm2;
        [Description("R,DevAddr02hRegAddrC3hEepm3")]
        public string DevAddr02hRegAddrC3hEepm3;
        [Description("R,DevAddr02hRegAddrC4hEepm4")]
        public string DevAddr02hRegAddrC4hEepm4;
        [Description("R,DevAddr02hRegAddrC5hEepm5")]
        public string DevAddr02hRegAddrC5hEepm5;
        [Description("R,DevAddr02hRegAddrC6hEepm6")]
        public string DevAddr02hRegAddrC6hEepm6;
        [Description("R,DevAddr02hRegAddrC7hEepm7")]
        public string DevAddr02hRegAddrC7hEepm7;
        [Description("R,DevAddr02hRegAddrC8hEepm8")]
        public string DevAddr02hRegAddrC8hEepm8;
        [Description("R,DevAddr02hRegAddrC9hEepm9")]
        public string DevAddr02hRegAddrC9hEepm9;
        [Description("R,DevAddr02hRegAddrCAhEepm10")]
        public string DevAddr02hRegAddrCAhEepm10;
        [Description("R,DevAddr02hRegAddrCBhEepm11")]
        public string DevAddr02hRegAddrCBhEepm11;
        [Description("R,DevAddr02hRegAddrCChEepm12")]
        public string DevAddr02hRegAddrCChEepm12;
        [Description("R,DevAddr02hRegAddrCDhEepm13")]
        public string DevAddr02hRegAddrCDhEepm13;
        [Description("R,DevAddr02hRegAddrCEhEepm14")]
        public string DevAddr02hRegAddrCEhEepm14;
        [Description("R,DevAddr02hRegAddrCFhEepm15")]
        public string DevAddr02hRegAddrCFhEepm15;

        [Description("R,DevAddr03hRegAddr00hIout0")]
        public string DevAddr03hRegAddr00hIout0;
        [Description("R,DevAddr03hRegAddr01hIout1")]
        public string DevAddr03hRegAddr01hIout1;
        [Description("R,DevAddr03hRegAddr02hIout2")]
        public string DevAddr03hRegAddr02hIout2;
        [Description("R,DevAddr03hRegAddr03hIout3")]
        public string DevAddr03hRegAddr03hIout3;
        [Description("R,DevAddr03hRegAddr04hIout4")]
        public string DevAddr03hRegAddr04hIout4;
        [Description("R,DevAddr03hRegAddr05hIout5")]
        public string DevAddr03hRegAddr05hIout5;
        [Description("R,DevAddr03hRegAddr06hIout6")]
        public string DevAddr03hRegAddr06hIout6;
        [Description("R,DevAddr03hRegAddr07hIout7")]
        public string DevAddr03hRegAddr07hIout7;
        [Description("R,DevAddr03hRegAddr08hIout8")]
        public string DevAddr03hRegAddr08hIout8;
        [Description("R,DevAddr03hRegAddr09hIout9")]
        public string DevAddr03hRegAddr09hIout9;
        [Description("R,DevAddr03hRegAddr0AhIout10")]
        public string DevAddr03hRegAddr0AhIout10;
        [Description("R,DevAddr03hRegAddr0BhIout11")]
        public string DevAddr03hRegAddr0BhIout11;
        [Description("R,DevAddr03hRegAddr20hIoutPwm0")]
        public string DevAddr03hRegAddr20hIoutPwm0;
        [Description("R,DevAddr03hRegAddr21hIoutPwm1")]
        public string DevAddr03hRegAddr21hIoutPwm1;
        [Description("R,DevAddr03hRegAddr22hIoutPwm2")]
        public string DevAddr03hRegAddr22hIoutPwm2;
        [Description("R,DevAddr03hRegAddr23hIoutPwm3")]
        public string DevAddr03hRegAddr23hIoutPwm3;
        [Description("R,DevAddr03hRegAddr24hIoutPwm4")]
        public string DevAddr03hRegAddr24hIoutPwm4;
        [Description("R,DevAddr03hRegAddr25hIoutPwm5")]
        public string DevAddr03hRegAddr25hIoutPwm5;
        [Description("R,DevAddr03hRegAddr26hIoutPwm6")]
        public string DevAddr03hRegAddr26hIoutPwm6;
        [Description("R,DevAddr03hRegAddr27hIoutPwm7")]
        public string DevAddr03hRegAddr27hIoutPwm7;
        [Description("R,DevAddr03hRegAddr28hIoutPwm8")]
        public string DevAddr03hRegAddr28hIoutPwm8;
        [Description("R,DevAddr03hRegAddr29hIoutPwm9")]
        public string DevAddr03hRegAddr29hIoutPwm9;
        [Description("R,DevAddr03hRegAddr2AhIoutPwm10")]
        public string DevAddr03hRegAddr2AhIoutPwm10;
        [Description("R,DevAddr03hRegAddr2BhIoutPwm11")]
        public string DevAddr03hRegAddr2BhIoutPwm11;
        [Description("R,DevAddr03hRegAddr40hIoutPwml0")]
        public string DevAddr03hRegAddr40hIoutPwml0;
        [Description("R,DevAddr03hRegAddr41hIoutPwml1")]
        public string DevAddr03hRegAddr41hIoutPwml1;
        [Description("R,DevAddr03hRegAddr42hIoutPwml2")]
        public string DevAddr03hRegAddr42hIoutPwml2;
        [Description("R,DevAddr03hRegAddr43hIoutPwml3")]
        public string DevAddr03hRegAddr43hIoutPwml3;
        [Description("R,DevAddr03hRegAddr44hIoutPwml4")]
        public string DevAddr03hRegAddr44hIoutPwml4;
        [Description("R,DevAddr03hRegAddr45hIoutPwml5")]
        public string DevAddr03hRegAddr45hIoutPwml5;
        [Description("R,DevAddr03hRegAddr46hIoutPwml6")]
        public string DevAddr03hRegAddr46hIoutPwml6;
        [Description("R,DevAddr03hRegAddr47hIoutPwml7")]
        public string DevAddr03hRegAddr47hIoutPwml7;
        [Description("R,DevAddr03hRegAddr48hIoutPwml8")]
        public string DevAddr03hRegAddr48hIoutPwml8;
        [Description("R,DevAddr03hRegAddr49hIoutPwml9")]
        public string DevAddr03hRegAddr49hIoutPwml9;
        [Description("R,DevAddr03hRegAddr4AhIoutPwml10")]
        public string DevAddr03hRegAddr4AhIoutPwml10;
        [Description("R,DevAddr03hRegAddr4BhIoutPwml11")]
        public string DevAddr03hRegAddr4BhIoutPwml11;
        [Description("R,DevAddr03hRegAddr50hConfEn0")]
        public string DevAddr03hRegAddr50hConfEn0;
        [Description("R,DevAddr03hRegAddr51hConfEn1")]
        public string DevAddr03hRegAddr51hConfEn1;
        [Description("R,DevAddr03hRegAddr54hConfDiagen0")]
        public string DevAddr03hRegAddr54hConfDiagen0;
        [Description("R,DevAddr03hRegAddr55hConfDiagen1")]
        public string DevAddr03hRegAddr55hConfDiagen1;
        [Description("R,DevAddr03hRegAddr56hConfMisc0")]
        public string DevAddr03hRegAddr56hConfMisc0;
        [Description("R,DevAddr03hRegAddr57hConfMisc1")]
        public string DevAddr03hRegAddr57hConfMisc1;
        [Description("R,DevAddr03hRegAddr58hConfMisc2")]
        public string DevAddr03hRegAddr58hConfMisc2;
        [Description("R,DevAddr03hRegAddr59hConfMisc3")]
        public string DevAddr03hRegAddr59hConfMisc3;
        [Description("R,DevAddr03hRegAddr5AhConfMisc4")]
        public string DevAddr03hRegAddr5AhConfMisc4;
        [Description("R,DevAddr03hRegAddr5BhConfMisc5")]
        public string DevAddr03hRegAddr5BhConfMisc5;
        [Description("R,DevAddr03hRegAddr60hClr")]
        public string DevAddr03hRegAddr60hClr;
        [Description("R,DevAddr03hRegAddr61hConfLock")]
        public string DevAddr03hRegAddr61hConfLock;
        [Description("R,DevAddr03hRegAddr62hConfMisc6")]
        public string DevAddr03hRegAddr62hConfMisc6;
        [Description("R,DevAddr03hRegAddr63hConfMisc7")]
        public string DevAddr03hRegAddr63hConfMisc7;
        [Description("R,DevAddr03hRegAddr64hConfMisc8")]
        public string DevAddr03hRegAddr64hConfMisc8;
        [Description("R,DevAddr03hRegAddr65hConfMisc9")]
        public string DevAddr03hRegAddr65hConfMisc9;
        [Description("R,DevAddr03hRegAddr70hFlag0")]
        public string DevAddr03hRegAddr70hFlag0;
        [Description("R,DevAddr03hRegAddr71hFlag1")]
        public string DevAddr03hRegAddr71hFlag1;
        [Description("R,DevAddr03hRegAddr72hFlag2")]
        public string DevAddr03hRegAddr72hFlag2;
        [Description("R,DevAddr03hRegAddr73hFlag3")]
        public string DevAddr03hRegAddr73hFlag3;
        [Description("R,DevAddr03hRegAddr74hFlag4")]
        public string DevAddr03hRegAddr74hFlag4;
        [Description("R,DevAddr03hRegAddr75hFlag5")]
        public string DevAddr03hRegAddr75hFlag5;
        [Description("R,DevAddr03hRegAddr77hFlag7")]
        public string DevAddr03hRegAddr77hFlag7;
        [Description("R,DevAddr03hRegAddr78hFlag8")]
        public string DevAddr03hRegAddr78hFlag8;
        [Description("R,DevAddr03hRegAddr7BhFlag11")]
        public string DevAddr03hRegAddr7BhFlag11;
        [Description("R,DevAddr03hRegAddr7ChFlag12")]
        public string DevAddr03hRegAddr7ChFlag12;
        [Description("R,DevAddr03hRegAddr7DhFlag13")]
        public string DevAddr03hRegAddr7DhFlag13;
        [Description("R,DevAddr03hRegAddr7EhFlag14")]
        public string DevAddr03hRegAddr7EhFlag14;
        [Description("R,DevAddr03hRegAddr80hEepi0")]
        public string DevAddr03hRegAddr80hEepi0;
        [Description("R,DevAddr03hRegAddr81hEepi1")]
        public string DevAddr03hRegAddr81hEepi1;
        [Description("R,DevAddr03hRegAddr82hEepi2")]
        public string DevAddr03hRegAddr82hEepi2;
        [Description("R,DevAddr03hRegAddr83hEepi3")]
        public string DevAddr03hRegAddr83hEepi3;
        [Description("R,DevAddr03hRegAddr84hEepi4")]
        public string DevAddr03hRegAddr84hEepi4;
        [Description("R,DevAddr03hRegAddr85hEepi5")]
        public string DevAddr03hRegAddr85hEepi5;
        [Description("R,DevAddr03hRegAddr86hEepi6")]
        public string DevAddr03hRegAddr86hEepi6;
        [Description("R,DevAddr03hRegAddr87hEepi7")]
        public string DevAddr03hRegAddr87hEepi7;
        [Description("R,DevAddr03hRegAddr88hEepi8")]
        public string DevAddr03hRegAddr88hEepi8;
        [Description("R,DevAddr03hRegAddr89hEepi9")]
        public string DevAddr03hRegAddr89hEepi9;
        [Description("R,DevAddr03hRegAddr8AhEepi10")]
        public string DevAddr03hRegAddr8AhEepi10;
        [Description("R,DevAddr03hRegAddr8BhEepi11")]
        public string DevAddr03hRegAddr8BhEepi11;
        [Description("R,DevAddr03hRegAddrA0hEepp0")]
        public string DevAddr03hRegAddrA0hEepp0;
        [Description("R,DevAddr03hRegAddrA1hEepp1")]
        public string DevAddr03hRegAddrA1hEepp1;
        [Description("R,DevAddr03hRegAddrA2hEepp2")]
        public string DevAddr03hRegAddrA2hEepp2;
        [Description("R,DevAddr03hRegAddrA3hEepp3")]
        public string DevAddr03hRegAddrA3hEepp3;
        [Description("R,DevAddr03hRegAddrA4hEepp4")]
        public string DevAddr03hRegAddrA4hEepp4;
        [Description("R,DevAddr03hRegAddrA5hEepp5")]
        public string DevAddr03hRegAddrA5hEepp5;
        [Description("R,DevAddr03hRegAddrA6hEepp6")]
        public string DevAddr03hRegAddrA6hEepp6;
        [Description("R,DevAddr03hRegAddrA7hEepp7")]
        public string DevAddr03hRegAddrA7hEepp7;
        [Description("R,DevAddr03hRegAddrA8hEepp8")]
        public string DevAddr03hRegAddrA8hEepp8;
        [Description("R,DevAddr03hRegAddrA9hEepp9")]
        public string DevAddr03hRegAddrA9hEepp9;
        [Description("R,DevAddr03hRegAddrAAhEepp10")]
        public string DevAddr03hRegAddrAAhEepp10;
        [Description("R,DevAddr03hRegAddrABhEepp11")]
        public string DevAddr03hRegAddrABhEepp11;
        [Description("R,DevAddr03hRegAddrC0hEepm0")]
        public string DevAddr03hRegAddrC0hEepm0;
        [Description("R,DevAddr03hRegAddrC1hEepm1")]
        public string DevAddr03hRegAddrC1hEepm1;
        [Description("R,DevAddr03hRegAddrC2hEepm2")]
        public string DevAddr03hRegAddrC2hEepm2;
        [Description("R,DevAddr03hRegAddrC3hEepm3")]
        public string DevAddr03hRegAddrC3hEepm3;
        [Description("R,DevAddr03hRegAddrC4hEepm4")]
        public string DevAddr03hRegAddrC4hEepm4;
        [Description("R,DevAddr03hRegAddrC5hEepm5")]
        public string DevAddr03hRegAddrC5hEepm5;
        [Description("R,DevAddr03hRegAddrC6hEepm6")]
        public string DevAddr03hRegAddrC6hEepm6;
        [Description("R,DevAddr03hRegAddrC7hEepm7")]
        public string DevAddr03hRegAddrC7hEepm7;
        [Description("R,DevAddr03hRegAddrC8hEepm8")]
        public string DevAddr03hRegAddrC8hEepm8;
        [Description("R,DevAddr03hRegAddrC9hEepm9")]
        public string DevAddr03hRegAddrC9hEepm9;
        [Description("R,DevAddr03hRegAddrCAhEepm10")]
        public string DevAddr03hRegAddrCAhEepm10;
        [Description("R,DevAddr03hRegAddrCBhEepm11")]
        public string DevAddr03hRegAddrCBhEepm11;
        [Description("R,DevAddr03hRegAddrCChEepm12")]
        public string DevAddr03hRegAddrCChEepm12;
        [Description("R,DevAddr03hRegAddrCDhEepm13")]
        public string DevAddr03hRegAddrCDhEepm13;
        [Description("R,DevAddr03hRegAddrCEhEepm14")]
        public string DevAddr03hRegAddrCEhEepm14;
        [Description("R,DevAddr03hRegAddrCFhEepm15")]
        public string DevAddr03hRegAddrCFhEepm15;

        [Description("R,DevAddr04hRegAddr00hIout0")]
        public string DevAddr04hRegAddr00hIout0;
        [Description("R,DevAddr04hRegAddr01hIout1")]
        public string DevAddr04hRegAddr01hIout1;
        [Description("R,DevAddr04hRegAddr02hIout2")]
        public string DevAddr04hRegAddr02hIout2;
        [Description("R,DevAddr04hRegAddr03hIout3")]
        public string DevAddr04hRegAddr03hIout3;
        [Description("R,DevAddr04hRegAddr04hIout4")]
        public string DevAddr04hRegAddr04hIout4;
        [Description("R,DevAddr04hRegAddr05hIout5")]
        public string DevAddr04hRegAddr05hIout5;
        [Description("R,DevAddr04hRegAddr06hIout6")]
        public string DevAddr04hRegAddr06hIout6;
        [Description("R,DevAddr04hRegAddr07hIout7")]
        public string DevAddr04hRegAddr07hIout7;
        [Description("R,DevAddr04hRegAddr08hIout8")]
        public string DevAddr04hRegAddr08hIout8;
        [Description("R,DevAddr04hRegAddr09hIout9")]
        public string DevAddr04hRegAddr09hIout9;
        [Description("R,DevAddr04hRegAddr0AhIout10")]
        public string DevAddr04hRegAddr0AhIout10;
        [Description("R,DevAddr04hRegAddr0BhIout11")]
        public string DevAddr04hRegAddr0BhIout11;
        [Description("R,DevAddr04hRegAddr20hIoutPwm0")]
        public string DevAddr04hRegAddr20hIoutPwm0;
        [Description("R,DevAddr04hRegAddr21hIoutPwm1")]
        public string DevAddr04hRegAddr21hIoutPwm1;
        [Description("R,DevAddr04hRegAddr22hIoutPwm2")]
        public string DevAddr04hRegAddr22hIoutPwm2;
        [Description("R,DevAddr04hRegAddr23hIoutPwm3")]
        public string DevAddr04hRegAddr23hIoutPwm3;
        [Description("R,DevAddr04hRegAddr24hIoutPwm4")]
        public string DevAddr04hRegAddr24hIoutPwm4;
        [Description("R,DevAddr04hRegAddr25hIoutPwm5")]
        public string DevAddr04hRegAddr25hIoutPwm5;
        [Description("R,DevAddr04hRegAddr26hIoutPwm6")]
        public string DevAddr04hRegAddr26hIoutPwm6;
        [Description("R,DevAddr04hRegAddr27hIoutPwm7")]
        public string DevAddr04hRegAddr27hIoutPwm7;
        [Description("R,DevAddr04hRegAddr28hIoutPwm8")]
        public string DevAddr04hRegAddr28hIoutPwm8;
        [Description("R,DevAddr04hRegAddr29hIoutPwm9")]
        public string DevAddr04hRegAddr29hIoutPwm9;
        [Description("R,DevAddr04hRegAddr2AhIoutPwm10")]
        public string DevAddr04hRegAddr2AhIoutPwm10;
        [Description("R,DevAddr04hRegAddr2BhIoutPwm11")]
        public string DevAddr04hRegAddr2BhIoutPwm11;
        [Description("R,DevAddr04hRegAddr40hIoutPwml0")]
        public string DevAddr04hRegAddr40hIoutPwml0;
        [Description("R,DevAddr04hRegAddr41hIoutPwml1")]
        public string DevAddr04hRegAddr41hIoutPwml1;
        [Description("R,DevAddr04hRegAddr42hIoutPwml2")]
        public string DevAddr04hRegAddr42hIoutPwml2;
        [Description("R,DevAddr04hRegAddr43hIoutPwml3")]
        public string DevAddr04hRegAddr43hIoutPwml3;
        [Description("R,DevAddr04hRegAddr44hIoutPwml4")]
        public string DevAddr04hRegAddr44hIoutPwml4;
        [Description("R,DevAddr04hRegAddr45hIoutPwml5")]
        public string DevAddr04hRegAddr45hIoutPwml5;
        [Description("R,DevAddr04hRegAddr46hIoutPwml6")]
        public string DevAddr04hRegAddr46hIoutPwml6;
        [Description("R,DevAddr04hRegAddr47hIoutPwml7")]
        public string DevAddr04hRegAddr47hIoutPwml7;
        [Description("R,DevAddr04hRegAddr48hIoutPwml8")]
        public string DevAddr04hRegAddr48hIoutPwml8;
        [Description("R,DevAddr04hRegAddr49hIoutPwml9")]
        public string DevAddr04hRegAddr49hIoutPwml9;
        [Description("R,DevAddr04hRegAddr4AhIoutPwml10")]
        public string DevAddr04hRegAddr4AhIoutPwml10;
        [Description("R,DevAddr04hRegAddr4BhIoutPwml11")]
        public string DevAddr04hRegAddr4BhIoutPwml11;
        [Description("R,DevAddr04hRegAddr50hConfEn0")]
        public string DevAddr04hRegAddr50hConfEn0;
        [Description("R,DevAddr04hRegAddr51hConfEn1")]
        public string DevAddr04hRegAddr51hConfEn1;
        [Description("R,DevAddr04hRegAddr54hConfDiagen0")]
        public string DevAddr04hRegAddr54hConfDiagen0;
        [Description("R,DevAddr04hRegAddr55hConfDiagen1")]
        public string DevAddr04hRegAddr55hConfDiagen1;
        [Description("R,DevAddr04hRegAddr56hConfMisc0")]
        public string DevAddr04hRegAddr56hConfMisc0;
        [Description("R,DevAddr04hRegAddr57hConfMisc1")]
        public string DevAddr04hRegAddr57hConfMisc1;
        [Description("R,DevAddr04hRegAddr58hConfMisc2")]
        public string DevAddr04hRegAddr58hConfMisc2;
        [Description("R,DevAddr04hRegAddr59hConfMisc3")]
        public string DevAddr04hRegAddr59hConfMisc3;
        [Description("R,DevAddr04hRegAddr5AhConfMisc4")]
        public string DevAddr04hRegAddr5AhConfMisc4;
        [Description("R,DevAddr04hRegAddr5BhConfMisc5")]
        public string DevAddr04hRegAddr5BhConfMisc5;
        [Description("R,DevAddr04hRegAddr60hClr")]
        public string DevAddr04hRegAddr60hClr;
        [Description("R,DevAddr04hRegAddr61hConfLock")]
        public string DevAddr04hRegAddr61hConfLock;
        [Description("R,DevAddr04hRegAddr62hConfMisc6")]
        public string DevAddr04hRegAddr62hConfMisc6;
        [Description("R,DevAddr04hRegAddr63hConfMisc7")]
        public string DevAddr04hRegAddr63hConfMisc7;
        [Description("R,DevAddr04hRegAddr64hConfMisc8")]
        public string DevAddr04hRegAddr64hConfMisc8;
        [Description("R,DevAddr04hRegAddr65hConfMisc9")]
        public string DevAddr04hRegAddr65hConfMisc9;
        [Description("R,DevAddr04hRegAddr70hFlag0")]
        public string DevAddr04hRegAddr70hFlag0;
        [Description("R,DevAddr04hRegAddr71hFlag1")]
        public string DevAddr04hRegAddr71hFlag1;
        [Description("R,DevAddr04hRegAddr72hFlag2")]
        public string DevAddr04hRegAddr72hFlag2;
        [Description("R,DevAddr04hRegAddr73hFlag3")]
        public string DevAddr04hRegAddr73hFlag3;
        [Description("R,DevAddr04hRegAddr74hFlag4")]
        public string DevAddr04hRegAddr74hFlag4;
        [Description("R,DevAddr04hRegAddr75hFlag5")]
        public string DevAddr04hRegAddr75hFlag5;
        [Description("R,DevAddr04hRegAddr77hFlag7")]
        public string DevAddr04hRegAddr77hFlag7;
        [Description("R,DevAddr04hRegAddr78hFlag8")]
        public string DevAddr04hRegAddr78hFlag8;
        [Description("R,DevAddr04hRegAddr7BhFlag11")]
        public string DevAddr04hRegAddr7BhFlag11;
        [Description("R,DevAddr04hRegAddr7ChFlag12")]
        public string DevAddr04hRegAddr7ChFlag12;
        [Description("R,DevAddr04hRegAddr7DhFlag13")]
        public string DevAddr04hRegAddr7DhFlag13;
        [Description("R,DevAddr04hRegAddr7EhFlag14")]
        public string DevAddr04hRegAddr7EhFlag14;
        [Description("R,DevAddr04hRegAddr80hEepi0")]
        public string DevAddr04hRegAddr80hEepi0;
        [Description("R,DevAddr04hRegAddr81hEepi1")]
        public string DevAddr04hRegAddr81hEepi1;
        [Description("R,DevAddr04hRegAddr82hEepi2")]
        public string DevAddr04hRegAddr82hEepi2;
        [Description("R,DevAddr04hRegAddr83hEepi3")]
        public string DevAddr04hRegAddr83hEepi3;
        [Description("R,DevAddr04hRegAddr84hEepi4")]
        public string DevAddr04hRegAddr84hEepi4;
        [Description("R,DevAddr04hRegAddr85hEepi5")]
        public string DevAddr04hRegAddr85hEepi5;
        [Description("R,DevAddr04hRegAddr86hEepi6")]
        public string DevAddr04hRegAddr86hEepi6;
        [Description("R,DevAddr04hRegAddr87hEepi7")]
        public string DevAddr04hRegAddr87hEepi7;
        [Description("R,DevAddr04hRegAddr88hEepi8")]
        public string DevAddr04hRegAddr88hEepi8;
        [Description("R,DevAddr04hRegAddr89hEepi9")]
        public string DevAddr04hRegAddr89hEepi9;
        [Description("R,DevAddr04hRegAddr8AhEepi10")]
        public string DevAddr04hRegAddr8AhEepi10;
        [Description("R,DevAddr04hRegAddr8BhEepi11")]
        public string DevAddr04hRegAddr8BhEepi11;
        [Description("R,DevAddr04hRegAddrA0hEepp0")]
        public string DevAddr04hRegAddrA0hEepp0;
        [Description("R,DevAddr04hRegAddrA1hEepp1")]
        public string DevAddr04hRegAddrA1hEepp1;
        [Description("R,DevAddr04hRegAddrA2hEepp2")]
        public string DevAddr04hRegAddrA2hEepp2;
        [Description("R,DevAddr04hRegAddrA3hEepp3")]
        public string DevAddr04hRegAddrA3hEepp3;
        [Description("R,DevAddr04hRegAddrA4hEepp4")]
        public string DevAddr04hRegAddrA4hEepp4;
        [Description("R,DevAddr04hRegAddrA5hEepp5")]
        public string DevAddr04hRegAddrA5hEepp5;
        [Description("R,DevAddr04hRegAddrA6hEepp6")]
        public string DevAddr04hRegAddrA6hEepp6;
        [Description("R,DevAddr04hRegAddrA7hEepp7")]
        public string DevAddr04hRegAddrA7hEepp7;
        [Description("R,DevAddr04hRegAddrA8hEepp8")]
        public string DevAddr04hRegAddrA8hEepp8;
        [Description("R,DevAddr04hRegAddrA9hEepp9")]
        public string DevAddr04hRegAddrA9hEepp9;
        [Description("R,DevAddr04hRegAddrAAhEepp10")]
        public string DevAddr04hRegAddrAAhEepp10;
        [Description("R,DevAddr04hRegAddrABhEepp11")]
        public string DevAddr04hRegAddrABhEepp11;
        [Description("R,DevAddr04hRegAddrC0hEepm0")]
        public string DevAddr04hRegAddrC0hEepm0;
        [Description("R,DevAddr04hRegAddrC1hEepm1")]
        public string DevAddr04hRegAddrC1hEepm1;
        [Description("R,DevAddr04hRegAddrC2hEepm2")]
        public string DevAddr04hRegAddrC2hEepm2;
        [Description("R,DevAddr04hRegAddrC3hEepm3")]
        public string DevAddr04hRegAddrC3hEepm3;
        [Description("R,DevAddr04hRegAddrC4hEepm4")]
        public string DevAddr04hRegAddrC4hEepm4;
        [Description("R,DevAddr04hRegAddrC5hEepm5")]
        public string DevAddr04hRegAddrC5hEepm5;
        [Description("R,DevAddr04hRegAddrC6hEepm6")]
        public string DevAddr04hRegAddrC6hEepm6;
        [Description("R,DevAddr04hRegAddrC7hEepm7")]
        public string DevAddr04hRegAddrC7hEepm7;
        [Description("R,DevAddr04hRegAddrC8hEepm8")]
        public string DevAddr04hRegAddrC8hEepm8;
        [Description("R,DevAddr04hRegAddrC9hEepm9")]
        public string DevAddr04hRegAddrC9hEepm9;
        [Description("R,DevAddr04hRegAddrCAhEepm10")]
        public string DevAddr04hRegAddrCAhEepm10;
        [Description("R,DevAddr04hRegAddrCBhEepm11")]
        public string DevAddr04hRegAddrCBhEepm11;
        [Description("R,DevAddr04hRegAddrCChEepm12")]
        public string DevAddr04hRegAddrCChEepm12;
        [Description("R,DevAddr04hRegAddrCDhEepm13")]
        public string DevAddr04hRegAddrCDhEepm13;
        [Description("R,DevAddr04hRegAddrCEhEepm14")]
        public string DevAddr04hRegAddrCEhEepm14;
        [Description("R,DevAddr04hRegAddrCFhEepm15")]
        public string DevAddr04hRegAddrCFhEepm15;

        [Description("R,DevAddr05hRegAddr00hIout0")]
        public string DevAddr05hRegAddr00hIout0;
        [Description("R,DevAddr05hRegAddr01hIout1")]
        public string DevAddr05hRegAddr01hIout1;
        [Description("R,DevAddr05hRegAddr02hIout2")]
        public string DevAddr05hRegAddr02hIout2;
        [Description("R,DevAddr05hRegAddr03hIout3")]
        public string DevAddr05hRegAddr03hIout3;
        [Description("R,DevAddr05hRegAddr04hIout4")]
        public string DevAddr05hRegAddr04hIout4;
        [Description("R,DevAddr05hRegAddr05hIout5")]
        public string DevAddr05hRegAddr05hIout5;
        [Description("R,DevAddr05hRegAddr06hIout6")]
        public string DevAddr05hRegAddr06hIout6;
        [Description("R,DevAddr05hRegAddr07hIout7")]
        public string DevAddr05hRegAddr07hIout7;
        [Description("R,DevAddr05hRegAddr08hIout8")]
        public string DevAddr05hRegAddr08hIout8;
        [Description("R,DevAddr05hRegAddr09hIout9")]
        public string DevAddr05hRegAddr09hIout9;
        [Description("R,DevAddr05hRegAddr0AhIout10")]
        public string DevAddr05hRegAddr0AhIout10;
        [Description("R,DevAddr05hRegAddr0BhIout11")]
        public string DevAddr05hRegAddr0BhIout11;
        [Description("R,DevAddr05hRegAddr20hIoutPwm0")]
        public string DevAddr05hRegAddr20hIoutPwm0;
        [Description("R,DevAddr05hRegAddr21hIoutPwm1")]
        public string DevAddr05hRegAddr21hIoutPwm1;
        [Description("R,DevAddr05hRegAddr22hIoutPwm2")]
        public string DevAddr05hRegAddr22hIoutPwm2;
        [Description("R,DevAddr05hRegAddr23hIoutPwm3")]
        public string DevAddr05hRegAddr23hIoutPwm3;
        [Description("R,DevAddr05hRegAddr24hIoutPwm4")]
        public string DevAddr05hRegAddr24hIoutPwm4;
        [Description("R,DevAddr05hRegAddr25hIoutPwm5")]
        public string DevAddr05hRegAddr25hIoutPwm5;
        [Description("R,DevAddr05hRegAddr26hIoutPwm6")]
        public string DevAddr05hRegAddr26hIoutPwm6;
        [Description("R,DevAddr05hRegAddr27hIoutPwm7")]
        public string DevAddr05hRegAddr27hIoutPwm7;
        [Description("R,DevAddr05hRegAddr28hIoutPwm8")]
        public string DevAddr05hRegAddr28hIoutPwm8;
        [Description("R,DevAddr05hRegAddr29hIoutPwm9")]
        public string DevAddr05hRegAddr29hIoutPwm9;
        [Description("R,DevAddr05hRegAddr2AhIoutPwm10")]
        public string DevAddr05hRegAddr2AhIoutPwm10;
        [Description("R,DevAddr05hRegAddr2BhIoutPwm11")]
        public string DevAddr05hRegAddr2BhIoutPwm11;
        [Description("R,DevAddr05hRegAddr40hIoutPwml0")]
        public string DevAddr05hRegAddr40hIoutPwml0;
        [Description("R,DevAddr05hRegAddr41hIoutPwml1")]
        public string DevAddr05hRegAddr41hIoutPwml1;
        [Description("R,DevAddr05hRegAddr42hIoutPwml2")]
        public string DevAddr05hRegAddr42hIoutPwml2;
        [Description("R,DevAddr05hRegAddr43hIoutPwml3")]
        public string DevAddr05hRegAddr43hIoutPwml3;
        [Description("R,DevAddr05hRegAddr44hIoutPwml4")]
        public string DevAddr05hRegAddr44hIoutPwml4;
        [Description("R,DevAddr05hRegAddr45hIoutPwml5")]
        public string DevAddr05hRegAddr45hIoutPwml5;
        [Description("R,DevAddr05hRegAddr46hIoutPwml6")]
        public string DevAddr05hRegAddr46hIoutPwml6;
        [Description("R,DevAddr05hRegAddr47hIoutPwml7")]
        public string DevAddr05hRegAddr47hIoutPwml7;
        [Description("R,DevAddr05hRegAddr48hIoutPwml8")]
        public string DevAddr05hRegAddr48hIoutPwml8;
        [Description("R,DevAddr05hRegAddr49hIoutPwml9")]
        public string DevAddr05hRegAddr49hIoutPwml9;
        [Description("R,DevAddr05hRegAddr4AhIoutPwml10")]
        public string DevAddr05hRegAddr4AhIoutPwml10;
        [Description("R,DevAddr05hRegAddr4BhIoutPwml11")]
        public string DevAddr05hRegAddr4BhIoutPwml11;
        [Description("R,DevAddr05hRegAddr50hConfEn0")]
        public string DevAddr05hRegAddr50hConfEn0;
        [Description("R,DevAddr05hRegAddr51hConfEn1")]
        public string DevAddr05hRegAddr51hConfEn1;
        [Description("R,DevAddr05hRegAddr54hConfDiagen0")]
        public string DevAddr05hRegAddr54hConfDiagen0;
        [Description("R,DevAddr05hRegAddr55hConfDiagen1")]
        public string DevAddr05hRegAddr55hConfDiagen1;
        [Description("R,DevAddr05hRegAddr56hConfMisc0")]
        public string DevAddr05hRegAddr56hConfMisc0;
        [Description("R,DevAddr05hRegAddr57hConfMisc1")]
        public string DevAddr05hRegAddr57hConfMisc1;
        [Description("R,DevAddr05hRegAddr58hConfMisc2")]
        public string DevAddr05hRegAddr58hConfMisc2;
        [Description("R,DevAddr05hRegAddr59hConfMisc3")]
        public string DevAddr05hRegAddr59hConfMisc3;
        [Description("R,DevAddr05hRegAddr5AhConfMisc4")]
        public string DevAddr05hRegAddr5AhConfMisc4;
        [Description("R,DevAddr05hRegAddr5BhConfMisc5")]
        public string DevAddr05hRegAddr5BhConfMisc5;
        [Description("R,DevAddr05hRegAddr60hClr")]
        public string DevAddr05hRegAddr60hClr;
        [Description("R,DevAddr05hRegAddr61hConfLock")]
        public string DevAddr05hRegAddr61hConfLock;
        [Description("R,DevAddr05hRegAddr62hConfMisc6")]
        public string DevAddr05hRegAddr62hConfMisc6;
        [Description("R,DevAddr05hRegAddr63hConfMisc7")]
        public string DevAddr05hRegAddr63hConfMisc7;
        [Description("R,DevAddr05hRegAddr64hConfMisc8")]
        public string DevAddr05hRegAddr64hConfMisc8;
        [Description("R,DevAddr05hRegAddr65hConfMisc9")]
        public string DevAddr05hRegAddr65hConfMisc9;
        [Description("R,DevAddr05hRegAddr70hFlag0")]
        public string DevAddr05hRegAddr70hFlag0;
        [Description("R,DevAddr05hRegAddr71hFlag1")]
        public string DevAddr05hRegAddr71hFlag1;
        [Description("R,DevAddr05hRegAddr72hFlag2")]
        public string DevAddr05hRegAddr72hFlag2;
        [Description("R,DevAddr05hRegAddr73hFlag3")]
        public string DevAddr05hRegAddr73hFlag3;
        [Description("R,DevAddr05hRegAddr74hFlag4")]
        public string DevAddr05hRegAddr74hFlag4;
        [Description("R,DevAddr05hRegAddr75hFlag5")]
        public string DevAddr05hRegAddr75hFlag5;
        [Description("R,DevAddr05hRegAddr77hFlag7")]
        public string DevAddr05hRegAddr77hFlag7;
        [Description("R,DevAddr05hRegAddr78hFlag8")]
        public string DevAddr05hRegAddr78hFlag8;
        [Description("R,DevAddr05hRegAddr7BhFlag11")]
        public string DevAddr05hRegAddr7BhFlag11;
        [Description("R,DevAddr05hRegAddr7ChFlag12")]
        public string DevAddr05hRegAddr7ChFlag12;
        [Description("R,DevAddr05hRegAddr7DhFlag13")]
        public string DevAddr05hRegAddr7DhFlag13;
        [Description("R,DevAddr05hRegAddr7EhFlag14")]
        public string DevAddr05hRegAddr7EhFlag14;
        [Description("R,DevAddr05hRegAddr80hEepi0")]
        public string DevAddr05hRegAddr80hEepi0;
        [Description("R,DevAddr05hRegAddr81hEepi1")]
        public string DevAddr05hRegAddr81hEepi1;
        [Description("R,DevAddr05hRegAddr82hEepi2")]
        public string DevAddr05hRegAddr82hEepi2;
        [Description("R,DevAddr05hRegAddr83hEepi3")]
        public string DevAddr05hRegAddr83hEepi3;
        [Description("R,DevAddr05hRegAddr84hEepi4")]
        public string DevAddr05hRegAddr84hEepi4;
        [Description("R,DevAddr05hRegAddr85hEepi5")]
        public string DevAddr05hRegAddr85hEepi5;
        [Description("R,DevAddr05hRegAddr86hEepi6")]
        public string DevAddr05hRegAddr86hEepi6;
        [Description("R,DevAddr05hRegAddr87hEepi7")]
        public string DevAddr05hRegAddr87hEepi7;
        [Description("R,DevAddr05hRegAddr88hEepi8")]
        public string DevAddr05hRegAddr88hEepi8;
        [Description("R,DevAddr05hRegAddr89hEepi9")]
        public string DevAddr05hRegAddr89hEepi9;
        [Description("R,DevAddr05hRegAddr8AhEepi10")]
        public string DevAddr05hRegAddr8AhEepi10;
        [Description("R,DevAddr05hRegAddr8BhEepi11")]
        public string DevAddr05hRegAddr8BhEepi11;
        [Description("R,DevAddr05hRegAddrA0hEepp0")]
        public string DevAddr05hRegAddrA0hEepp0;
        [Description("R,DevAddr05hRegAddrA1hEepp1")]
        public string DevAddr05hRegAddrA1hEepp1;
        [Description("R,DevAddr05hRegAddrA2hEepp2")]
        public string DevAddr05hRegAddrA2hEepp2;
        [Description("R,DevAddr05hRegAddrA3hEepp3")]
        public string DevAddr05hRegAddrA3hEepp3;
        [Description("R,DevAddr05hRegAddrA4hEepp4")]
        public string DevAddr05hRegAddrA4hEepp4;
        [Description("R,DevAddr05hRegAddrA5hEepp5")]
        public string DevAddr05hRegAddrA5hEepp5;
        [Description("R,DevAddr05hRegAddrA6hEepp6")]
        public string DevAddr05hRegAddrA6hEepp6;
        [Description("R,DevAddr05hRegAddrA7hEepp7")]
        public string DevAddr05hRegAddrA7hEepp7;
        [Description("R,DevAddr05hRegAddrA8hEepp8")]
        public string DevAddr05hRegAddrA8hEepp8;
        [Description("R,DevAddr05hRegAddrA9hEepp9")]
        public string DevAddr05hRegAddrA9hEepp9;
        [Description("R,DevAddr05hRegAddrAAhEepp10")]
        public string DevAddr05hRegAddrAAhEepp10;
        [Description("R,DevAddr05hRegAddrABhEepp11")]
        public string DevAddr05hRegAddrABhEepp11;
        [Description("R,DevAddr05hRegAddrC0hEepm0")]
        public string DevAddr05hRegAddrC0hEepm0;
        [Description("R,DevAddr05hRegAddrC1hEepm1")]
        public string DevAddr05hRegAddrC1hEepm1;
        [Description("R,DevAddr05hRegAddrC2hEepm2")]
        public string DevAddr05hRegAddrC2hEepm2;
        [Description("R,DevAddr05hRegAddrC3hEepm3")]
        public string DevAddr05hRegAddrC3hEepm3;
        [Description("R,DevAddr05hRegAddrC4hEepm4")]
        public string DevAddr05hRegAddrC4hEepm4;
        [Description("R,DevAddr05hRegAddrC5hEepm5")]
        public string DevAddr05hRegAddrC5hEepm5;
        [Description("R,DevAddr05hRegAddrC6hEepm6")]
        public string DevAddr05hRegAddrC6hEepm6;
        [Description("R,DevAddr05hRegAddrC7hEepm7")]
        public string DevAddr05hRegAddrC7hEepm7;
        [Description("R,DevAddr05hRegAddrC8hEepm8")]
        public string DevAddr05hRegAddrC8hEepm8;
        [Description("R,DevAddr05hRegAddrC9hEepm9")]
        public string DevAddr05hRegAddrC9hEepm9;
        [Description("R,DevAddr05hRegAddrCAhEepm10")]
        public string DevAddr05hRegAddrCAhEepm10;
        [Description("R,DevAddr05hRegAddrCBhEepm11")]
        public string DevAddr05hRegAddrCBhEepm11;
        [Description("R,DevAddr05hRegAddrCChEepm12")]
        public string DevAddr05hRegAddrCChEepm12;
        [Description("R,DevAddr05hRegAddrCDhEepm13")]
        public string DevAddr05hRegAddrCDhEepm13;
        [Description("R,DevAddr05hRegAddrCEhEepm14")]
        public string DevAddr05hRegAddrCEhEepm14;
        [Description("R,DevAddr05hRegAddrCFhEepm15")]
        public string DevAddr05hRegAddrCFhEepm15;

        [Description("R,DevAddr06hRegAddr00hIout0")]
        public string DevAddr06hRegAddr00hIout0;
        [Description("R,DevAddr06hRegAddr01hIout1")]
        public string DevAddr06hRegAddr01hIout1;
        [Description("R,DevAddr06hRegAddr02hIout2")]
        public string DevAddr06hRegAddr02hIout2;
        [Description("R,DevAddr06hRegAddr03hIout3")]
        public string DevAddr06hRegAddr03hIout3;
        [Description("R,DevAddr06hRegAddr04hIout4")]
        public string DevAddr06hRegAddr04hIout4;
        [Description("R,DevAddr06hRegAddr05hIout5")]
        public string DevAddr06hRegAddr05hIout5;
        [Description("R,DevAddr06hRegAddr06hIout6")]
        public string DevAddr06hRegAddr06hIout6;
        [Description("R,DevAddr06hRegAddr07hIout7")]
        public string DevAddr06hRegAddr07hIout7;
        [Description("R,DevAddr06hRegAddr08hIout8")]
        public string DevAddr06hRegAddr08hIout8;
        [Description("R,DevAddr06hRegAddr09hIout9")]
        public string DevAddr06hRegAddr09hIout9;
        [Description("R,DevAddr06hRegAddr0AhIout10")]
        public string DevAddr06hRegAddr0AhIout10;
        [Description("R,DevAddr06hRegAddr0BhIout11")]
        public string DevAddr06hRegAddr0BhIout11;
        [Description("R,DevAddr06hRegAddr20hIoutPwm0")]
        public string DevAddr06hRegAddr20hIoutPwm0;
        [Description("R,DevAddr06hRegAddr21hIoutPwm1")]
        public string DevAddr06hRegAddr21hIoutPwm1;
        [Description("R,DevAddr06hRegAddr22hIoutPwm2")]
        public string DevAddr06hRegAddr22hIoutPwm2;
        [Description("R,DevAddr06hRegAddr23hIoutPwm3")]
        public string DevAddr06hRegAddr23hIoutPwm3;
        [Description("R,DevAddr06hRegAddr24hIoutPwm4")]
        public string DevAddr06hRegAddr24hIoutPwm4;
        [Description("R,DevAddr06hRegAddr25hIoutPwm5")]
        public string DevAddr06hRegAddr25hIoutPwm5;
        [Description("R,DevAddr06hRegAddr26hIoutPwm6")]
        public string DevAddr06hRegAddr26hIoutPwm6;
        [Description("R,DevAddr06hRegAddr27hIoutPwm7")]
        public string DevAddr06hRegAddr27hIoutPwm7;
        [Description("R,DevAddr06hRegAddr28hIoutPwm8")]
        public string DevAddr06hRegAddr28hIoutPwm8;
        [Description("R,DevAddr06hRegAddr29hIoutPwm9")]
        public string DevAddr06hRegAddr29hIoutPwm9;
        [Description("R,DevAddr06hRegAddr2AhIoutPwm10")]
        public string DevAddr06hRegAddr2AhIoutPwm10;
        [Description("R,DevAddr06hRegAddr2BhIoutPwm11")]
        public string DevAddr06hRegAddr2BhIoutPwm11;
        [Description("R,DevAddr06hRegAddr40hIoutPwml0")]
        public string DevAddr06hRegAddr40hIoutPwml0;
        [Description("R,DevAddr06hRegAddr41hIoutPwml1")]
        public string DevAddr06hRegAddr41hIoutPwml1;
        [Description("R,DevAddr06hRegAddr42hIoutPwml2")]
        public string DevAddr06hRegAddr42hIoutPwml2;
        [Description("R,DevAddr06hRegAddr43hIoutPwml3")]
        public string DevAddr06hRegAddr43hIoutPwml3;
        [Description("R,DevAddr06hRegAddr44hIoutPwml4")]
        public string DevAddr06hRegAddr44hIoutPwml4;
        [Description("R,DevAddr06hRegAddr45hIoutPwml5")]
        public string DevAddr06hRegAddr45hIoutPwml5;
        [Description("R,DevAddr06hRegAddr46hIoutPwml6")]
        public string DevAddr06hRegAddr46hIoutPwml6;
        [Description("R,DevAddr06hRegAddr47hIoutPwml7")]
        public string DevAddr06hRegAddr47hIoutPwml7;
        [Description("R,DevAddr06hRegAddr48hIoutPwml8")]
        public string DevAddr06hRegAddr48hIoutPwml8;
        [Description("R,DevAddr06hRegAddr49hIoutPwml9")]
        public string DevAddr06hRegAddr49hIoutPwml9;
        [Description("R,DevAddr06hRegAddr4AhIoutPwml10")]
        public string DevAddr06hRegAddr4AhIoutPwml10;
        [Description("R,DevAddr06hRegAddr4BhIoutPwml11")]
        public string DevAddr06hRegAddr4BhIoutPwml11;
        [Description("R,DevAddr06hRegAddr50hConfEn0")]
        public string DevAddr06hRegAddr50hConfEn0;
        [Description("R,DevAddr06hRegAddr51hConfEn1")]
        public string DevAddr06hRegAddr51hConfEn1;
        [Description("R,DevAddr06hRegAddr54hConfDiagen0")]
        public string DevAddr06hRegAddr54hConfDiagen0;
        [Description("R,DevAddr06hRegAddr55hConfDiagen1")]
        public string DevAddr06hRegAddr55hConfDiagen1;
        [Description("R,DevAddr06hRegAddr56hConfMisc0")]
        public string DevAddr06hRegAddr56hConfMisc0;
        [Description("R,DevAddr06hRegAddr57hConfMisc1")]
        public string DevAddr06hRegAddr57hConfMisc1;
        [Description("R,DevAddr06hRegAddr58hConfMisc2")]
        public string DevAddr06hRegAddr58hConfMisc2;
        [Description("R,DevAddr06hRegAddr59hConfMisc3")]
        public string DevAddr06hRegAddr59hConfMisc3;
        [Description("R,DevAddr06hRegAddr5AhConfMisc4")]
        public string DevAddr06hRegAddr5AhConfMisc4;
        [Description("R,DevAddr06hRegAddr5BhConfMisc5")]
        public string DevAddr06hRegAddr5BhConfMisc5;
        [Description("R,DevAddr06hRegAddr60hClr")]
        public string DevAddr06hRegAddr60hClr;
        [Description("R,DevAddr06hRegAddr61hConfLock")]
        public string DevAddr06hRegAddr61hConfLock;
        [Description("R,DevAddr06hRegAddr62hConfMisc6")]
        public string DevAddr06hRegAddr62hConfMisc6;
        [Description("R,DevAddr06hRegAddr63hConfMisc7")]
        public string DevAddr06hRegAddr63hConfMisc7;
        [Description("R,DevAddr06hRegAddr64hConfMisc8")]
        public string DevAddr06hRegAddr64hConfMisc8;
        [Description("R,DevAddr06hRegAddr65hConfMisc9")]
        public string DevAddr06hRegAddr65hConfMisc9;
        [Description("R,DevAddr06hRegAddr70hFlag0")]
        public string DevAddr06hRegAddr70hFlag0;
        [Description("R,DevAddr06hRegAddr71hFlag1")]
        public string DevAddr06hRegAddr71hFlag1;
        [Description("R,DevAddr06hRegAddr72hFlag2")]
        public string DevAddr06hRegAddr72hFlag2;
        [Description("R,DevAddr06hRegAddr73hFlag3")]
        public string DevAddr06hRegAddr73hFlag3;
        [Description("R,DevAddr06hRegAddr74hFlag4")]
        public string DevAddr06hRegAddr74hFlag4;
        [Description("R,DevAddr06hRegAddr75hFlag5")]
        public string DevAddr06hRegAddr75hFlag5;
        [Description("R,DevAddr06hRegAddr77hFlag7")]
        public string DevAddr06hRegAddr77hFlag7;
        [Description("R,DevAddr06hRegAddr78hFlag8")]
        public string DevAddr06hRegAddr78hFlag8;
        [Description("R,DevAddr06hRegAddr7BhFlag11")]
        public string DevAddr06hRegAddr7BhFlag11;
        [Description("R,DevAddr06hRegAddr7ChFlag12")]
        public string DevAddr06hRegAddr7ChFlag12;
        [Description("R,DevAddr06hRegAddr7DhFlag13")]
        public string DevAddr06hRegAddr7DhFlag13;
        [Description("R,DevAddr06hRegAddr7EhFlag14")]
        public string DevAddr06hRegAddr7EhFlag14;
        [Description("R,DevAddr06hRegAddr80hEepi0")]
        public string DevAddr06hRegAddr80hEepi0;
        [Description("R,DevAddr06hRegAddr81hEepi1")]
        public string DevAddr06hRegAddr81hEepi1;
        [Description("R,DevAddr06hRegAddr82hEepi2")]
        public string DevAddr06hRegAddr82hEepi2;
        [Description("R,DevAddr06hRegAddr83hEepi3")]
        public string DevAddr06hRegAddr83hEepi3;
        [Description("R,DevAddr06hRegAddr84hEepi4")]
        public string DevAddr06hRegAddr84hEepi4;
        [Description("R,DevAddr06hRegAddr85hEepi5")]
        public string DevAddr06hRegAddr85hEepi5;
        [Description("R,DevAddr06hRegAddr86hEepi6")]
        public string DevAddr06hRegAddr86hEepi6;
        [Description("R,DevAddr06hRegAddr87hEepi7")]
        public string DevAddr06hRegAddr87hEepi7;
        [Description("R,DevAddr06hRegAddr88hEepi8")]
        public string DevAddr06hRegAddr88hEepi8;
        [Description("R,DevAddr06hRegAddr89hEepi9")]
        public string DevAddr06hRegAddr89hEepi9;
        [Description("R,DevAddr06hRegAddr8AhEepi10")]
        public string DevAddr06hRegAddr8AhEepi10;
        [Description("R,DevAddr06hRegAddr8BhEepi11")]
        public string DevAddr06hRegAddr8BhEepi11;
        [Description("R,DevAddr06hRegAddrA0hEepp0")]
        public string DevAddr06hRegAddrA0hEepp0;
        [Description("R,DevAddr06hRegAddrA1hEepp1")]
        public string DevAddr06hRegAddrA1hEepp1;
        [Description("R,DevAddr06hRegAddrA2hEepp2")]
        public string DevAddr06hRegAddrA2hEepp2;
        [Description("R,DevAddr06hRegAddrA3hEepp3")]
        public string DevAddr06hRegAddrA3hEepp3;
        [Description("R,DevAddr06hRegAddrA4hEepp4")]
        public string DevAddr06hRegAddrA4hEepp4;
        [Description("R,DevAddr06hRegAddrA5hEepp5")]
        public string DevAddr06hRegAddrA5hEepp5;
        [Description("R,DevAddr06hRegAddrA6hEepp6")]
        public string DevAddr06hRegAddrA6hEepp6;
        [Description("R,DevAddr06hRegAddrA7hEepp7")]
        public string DevAddr06hRegAddrA7hEepp7;
        [Description("R,DevAddr06hRegAddrA8hEepp8")]
        public string DevAddr06hRegAddrA8hEepp8;
        [Description("R,DevAddr06hRegAddrA9hEepp9")]
        public string DevAddr06hRegAddrA9hEepp9;
        [Description("R,DevAddr06hRegAddrAAhEepp10")]
        public string DevAddr06hRegAddrAAhEepp10;
        [Description("R,DevAddr06hRegAddrABhEepp11")]
        public string DevAddr06hRegAddrABhEepp11;
        [Description("R,DevAddr06hRegAddrC0hEepm0")]
        public string DevAddr06hRegAddrC0hEepm0;
        [Description("R,DevAddr06hRegAddrC1hEepm1")]
        public string DevAddr06hRegAddrC1hEepm1;
        [Description("R,DevAddr06hRegAddrC2hEepm2")]
        public string DevAddr06hRegAddrC2hEepm2;
        [Description("R,DevAddr06hRegAddrC3hEepm3")]
        public string DevAddr06hRegAddrC3hEepm3;
        [Description("R,DevAddr06hRegAddrC4hEepm4")]
        public string DevAddr06hRegAddrC4hEepm4;
        [Description("R,DevAddr06hRegAddrC5hEepm5")]
        public string DevAddr06hRegAddrC5hEepm5;
        [Description("R,DevAddr06hRegAddrC6hEepm6")]
        public string DevAddr06hRegAddrC6hEepm6;
        [Description("R,DevAddr06hRegAddrC7hEepm7")]
        public string DevAddr06hRegAddrC7hEepm7;
        [Description("R,DevAddr06hRegAddrC8hEepm8")]
        public string DevAddr06hRegAddrC8hEepm8;
        [Description("R,DevAddr06hRegAddrC9hEepm9")]
        public string DevAddr06hRegAddrC9hEepm9;
        [Description("R,DevAddr06hRegAddrCAhEepm10")]
        public string DevAddr06hRegAddrCAhEepm10;
        [Description("R,DevAddr06hRegAddrCBhEepm11")]
        public string DevAddr06hRegAddrCBhEepm11;
        [Description("R,DevAddr06hRegAddrCChEepm12")]
        public string DevAddr06hRegAddrCChEepm12;
        [Description("R,DevAddr06hRegAddrCDhEepm13")]
        public string DevAddr06hRegAddrCDhEepm13;
        [Description("R,DevAddr06hRegAddrCEhEepm14")]
        public string DevAddr06hRegAddrCEhEepm14;
        [Description("R,DevAddr06hRegAddrCFhEepm15")]
        public string DevAddr06hRegAddrCFhEepm15;

        [Description("R,DevAddr07hRegAddr00hIout0")]
        public string DevAddr07hRegAddr00hIout0;
        [Description("R,DevAddr07hRegAddr01hIout1")]
        public string DevAddr07hRegAddr01hIout1;
        [Description("R,DevAddr07hRegAddr02hIout2")]
        public string DevAddr07hRegAddr02hIout2;
        [Description("R,DevAddr07hRegAddr03hIout3")]
        public string DevAddr07hRegAddr03hIout3;
        [Description("R,DevAddr07hRegAddr04hIout4")]
        public string DevAddr07hRegAddr04hIout4;
        [Description("R,DevAddr07hRegAddr05hIout5")]
        public string DevAddr07hRegAddr05hIout5;
        [Description("R,DevAddr07hRegAddr06hIout6")]
        public string DevAddr07hRegAddr06hIout6;
        [Description("R,DevAddr07hRegAddr07hIout7")]
        public string DevAddr07hRegAddr07hIout7;
        [Description("R,DevAddr07hRegAddr08hIout8")]
        public string DevAddr07hRegAddr08hIout8;
        [Description("R,DevAddr07hRegAddr09hIout9")]
        public string DevAddr07hRegAddr09hIout9;
        [Description("R,DevAddr07hRegAddr0AhIout10")]
        public string DevAddr07hRegAddr0AhIout10;
        [Description("R,DevAddr07hRegAddr0BhIout11")]
        public string DevAddr07hRegAddr0BhIout11;
        [Description("R,DevAddr07hRegAddr20hIoutPwm0")]
        public string DevAddr07hRegAddr20hIoutPwm0;
        [Description("R,DevAddr07hRegAddr21hIoutPwm1")]
        public string DevAddr07hRegAddr21hIoutPwm1;
        [Description("R,DevAddr07hRegAddr22hIoutPwm2")]
        public string DevAddr07hRegAddr22hIoutPwm2;
        [Description("R,DevAddr07hRegAddr23hIoutPwm3")]
        public string DevAddr07hRegAddr23hIoutPwm3;
        [Description("R,DevAddr07hRegAddr24hIoutPwm4")]
        public string DevAddr07hRegAddr24hIoutPwm4;
        [Description("R,DevAddr07hRegAddr25hIoutPwm5")]
        public string DevAddr07hRegAddr25hIoutPwm5;
        [Description("R,DevAddr07hRegAddr26hIoutPwm6")]
        public string DevAddr07hRegAddr26hIoutPwm6;
        [Description("R,DevAddr07hRegAddr27hIoutPwm7")]
        public string DevAddr07hRegAddr27hIoutPwm7;
        [Description("R,DevAddr07hRegAddr28hIoutPwm8")]
        public string DevAddr07hRegAddr28hIoutPwm8;
        [Description("R,DevAddr07hRegAddr29hIoutPwm9")]
        public string DevAddr07hRegAddr29hIoutPwm9;
        [Description("R,DevAddr07hRegAddr2AhIoutPwm10")]
        public string DevAddr07hRegAddr2AhIoutPwm10;
        [Description("R,DevAddr07hRegAddr2BhIoutPwm11")]
        public string DevAddr07hRegAddr2BhIoutPwm11;
        [Description("R,DevAddr07hRegAddr40hIoutPwml0")]
        public string DevAddr07hRegAddr40hIoutPwml0;
        [Description("R,DevAddr07hRegAddr41hIoutPwml1")]
        public string DevAddr07hRegAddr41hIoutPwml1;
        [Description("R,DevAddr07hRegAddr42hIoutPwml2")]
        public string DevAddr07hRegAddr42hIoutPwml2;
        [Description("R,DevAddr07hRegAddr43hIoutPwml3")]
        public string DevAddr07hRegAddr43hIoutPwml3;
        [Description("R,DevAddr07hRegAddr44hIoutPwml4")]
        public string DevAddr07hRegAddr44hIoutPwml4;
        [Description("R,DevAddr07hRegAddr45hIoutPwml5")]
        public string DevAddr07hRegAddr45hIoutPwml5;
        [Description("R,DevAddr07hRegAddr46hIoutPwml6")]
        public string DevAddr07hRegAddr46hIoutPwml6;
        [Description("R,DevAddr07hRegAddr47hIoutPwml7")]
        public string DevAddr07hRegAddr47hIoutPwml7;
        [Description("R,DevAddr07hRegAddr48hIoutPwml8")]
        public string DevAddr07hRegAddr48hIoutPwml8;
        [Description("R,DevAddr07hRegAddr49hIoutPwml9")]
        public string DevAddr07hRegAddr49hIoutPwml9;
        [Description("R,DevAddr07hRegAddr4AhIoutPwml10")]
        public string DevAddr07hRegAddr4AhIoutPwml10;
        [Description("R,DevAddr07hRegAddr4BhIoutPwml11")]
        public string DevAddr07hRegAddr4BhIoutPwml11;
        [Description("R,DevAddr07hRegAddr50hConfEn0")]
        public string DevAddr07hRegAddr50hConfEn0;
        [Description("R,DevAddr07hRegAddr51hConfEn1")]
        public string DevAddr07hRegAddr51hConfEn1;
        [Description("R,DevAddr07hRegAddr54hConfDiagen0")]
        public string DevAddr07hRegAddr54hConfDiagen0;
        [Description("R,DevAddr07hRegAddr55hConfDiagen1")]
        public string DevAddr07hRegAddr55hConfDiagen1;
        [Description("R,DevAddr07hRegAddr56hConfMisc0")]
        public string DevAddr07hRegAddr56hConfMisc0;
        [Description("R,DevAddr07hRegAddr57hConfMisc1")]
        public string DevAddr07hRegAddr57hConfMisc1;
        [Description("R,DevAddr07hRegAddr58hConfMisc2")]
        public string DevAddr07hRegAddr58hConfMisc2;
        [Description("R,DevAddr07hRegAddr59hConfMisc3")]
        public string DevAddr07hRegAddr59hConfMisc3;
        [Description("R,DevAddr07hRegAddr5AhConfMisc4")]
        public string DevAddr07hRegAddr5AhConfMisc4;
        [Description("R,DevAddr07hRegAddr5BhConfMisc5")]
        public string DevAddr07hRegAddr5BhConfMisc5;
        [Description("R,DevAddr07hRegAddr60hClr")]
        public string DevAddr07hRegAddr60hClr;
        [Description("R,DevAddr07hRegAddr61hConfLock")]
        public string DevAddr07hRegAddr61hConfLock;
        [Description("R,DevAddr07hRegAddr62hConfMisc6")]
        public string DevAddr07hRegAddr62hConfMisc6;
        [Description("R,DevAddr07hRegAddr63hConfMisc7")]
        public string DevAddr07hRegAddr63hConfMisc7;
        [Description("R,DevAddr07hRegAddr64hConfMisc8")]
        public string DevAddr07hRegAddr64hConfMisc8;
        [Description("R,DevAddr07hRegAddr65hConfMisc9")]
        public string DevAddr07hRegAddr65hConfMisc9;
        [Description("R,DevAddr07hRegAddr70hFlag0")]
        public string DevAddr07hRegAddr70hFlag0;
        [Description("R,DevAddr07hRegAddr71hFlag1")]
        public string DevAddr07hRegAddr71hFlag1;
        [Description("R,DevAddr07hRegAddr72hFlag2")]
        public string DevAddr07hRegAddr72hFlag2;
        [Description("R,DevAddr07hRegAddr73hFlag3")]
        public string DevAddr07hRegAddr73hFlag3;
        [Description("R,DevAddr07hRegAddr74hFlag4")]
        public string DevAddr07hRegAddr74hFlag4;
        [Description("R,DevAddr07hRegAddr75hFlag5")]
        public string DevAddr07hRegAddr75hFlag5;
        [Description("R,DevAddr07hRegAddr77hFlag7")]
        public string DevAddr07hRegAddr77hFlag7;
        [Description("R,DevAddr07hRegAddr78hFlag8")]
        public string DevAddr07hRegAddr78hFlag8;
        [Description("R,DevAddr07hRegAddr7BhFlag11")]
        public string DevAddr07hRegAddr7BhFlag11;
        [Description("R,DevAddr07hRegAddr7ChFlag12")]
        public string DevAddr07hRegAddr7ChFlag12;
        [Description("R,DevAddr07hRegAddr7DhFlag13")]
        public string DevAddr07hRegAddr7DhFlag13;
        [Description("R,DevAddr07hRegAddr7EhFlag14")]
        public string DevAddr07hRegAddr7EhFlag14;
        [Description("R,DevAddr07hRegAddr80hEepi0")]
        public string DevAddr07hRegAddr80hEepi0;
        [Description("R,DevAddr07hRegAddr81hEepi1")]
        public string DevAddr07hRegAddr81hEepi1;
        [Description("R,DevAddr07hRegAddr82hEepi2")]
        public string DevAddr07hRegAddr82hEepi2;
        [Description("R,DevAddr07hRegAddr83hEepi3")]
        public string DevAddr07hRegAddr83hEepi3;
        [Description("R,DevAddr07hRegAddr84hEepi4")]
        public string DevAddr07hRegAddr84hEepi4;
        [Description("R,DevAddr07hRegAddr85hEepi5")]
        public string DevAddr07hRegAddr85hEepi5;
        [Description("R,DevAddr07hRegAddr86hEepi6")]
        public string DevAddr07hRegAddr86hEepi6;
        [Description("R,DevAddr07hRegAddr87hEepi7")]
        public string DevAddr07hRegAddr87hEepi7;
        [Description("R,DevAddr07hRegAddr88hEepi8")]
        public string DevAddr07hRegAddr88hEepi8;
        [Description("R,DevAddr07hRegAddr89hEepi9")]
        public string DevAddr07hRegAddr89hEepi9;
        [Description("R,DevAddr07hRegAddr8AhEepi10")]
        public string DevAddr07hRegAddr8AhEepi10;
        [Description("R,DevAddr07hRegAddr8BhEepi11")]
        public string DevAddr07hRegAddr8BhEepi11;
        [Description("R,DevAddr07hRegAddrA0hEepp0")]
        public string DevAddr07hRegAddrA0hEepp0;
        [Description("R,DevAddr07hRegAddrA1hEepp1")]
        public string DevAddr07hRegAddrA1hEepp1;
        [Description("R,DevAddr07hRegAddrA2hEepp2")]
        public string DevAddr07hRegAddrA2hEepp2;
        [Description("R,DevAddr07hRegAddrA3hEepp3")]
        public string DevAddr07hRegAddrA3hEepp3;
        [Description("R,DevAddr07hRegAddrA4hEepp4")]
        public string DevAddr07hRegAddrA4hEepp4;
        [Description("R,DevAddr07hRegAddrA5hEepp5")]
        public string DevAddr07hRegAddrA5hEepp5;
        [Description("R,DevAddr07hRegAddrA6hEepp6")]
        public string DevAddr07hRegAddrA6hEepp6;
        [Description("R,DevAddr07hRegAddrA7hEepp7")]
        public string DevAddr07hRegAddrA7hEepp7;
        [Description("R,DevAddr07hRegAddrA8hEepp8")]
        public string DevAddr07hRegAddrA8hEepp8;
        [Description("R,DevAddr07hRegAddrA9hEepp9")]
        public string DevAddr07hRegAddrA9hEepp9;
        [Description("R,DevAddr07hRegAddrAAhEepp10")]
        public string DevAddr07hRegAddrAAhEepp10;
        [Description("R,DevAddr07hRegAddrABhEepp11")]
        public string DevAddr07hRegAddrABhEepp11;
        [Description("R,DevAddr07hRegAddrC0hEepm0")]
        public string DevAddr07hRegAddrC0hEepm0;
        [Description("R,DevAddr07hRegAddrC1hEepm1")]
        public string DevAddr07hRegAddrC1hEepm1;
        [Description("R,DevAddr07hRegAddrC2hEepm2")]
        public string DevAddr07hRegAddrC2hEepm2;
        [Description("R,DevAddr07hRegAddrC3hEepm3")]
        public string DevAddr07hRegAddrC3hEepm3;
        [Description("R,DevAddr07hRegAddrC4hEepm4")]
        public string DevAddr07hRegAddrC4hEepm4;
        [Description("R,DevAddr07hRegAddrC5hEepm5")]
        public string DevAddr07hRegAddrC5hEepm5;
        [Description("R,DevAddr07hRegAddrC6hEepm6")]
        public string DevAddr07hRegAddrC6hEepm6;
        [Description("R,DevAddr07hRegAddrC7hEepm7")]
        public string DevAddr07hRegAddrC7hEepm7;
        [Description("R,DevAddr07hRegAddrC8hEepm8")]
        public string DevAddr07hRegAddrC8hEepm8;
        [Description("R,DevAddr07hRegAddrC9hEepm9")]
        public string DevAddr07hRegAddrC9hEepm9;
        [Description("R,DevAddr07hRegAddrCAhEepm10")]
        public string DevAddr07hRegAddrCAhEepm10;
        [Description("R,DevAddr07hRegAddrCBhEepm11")]
        public string DevAddr07hRegAddrCBhEepm11;
        [Description("R,DevAddr07hRegAddrCChEepm12")]
        public string DevAddr07hRegAddrCChEepm12;
        [Description("R,DevAddr07hRegAddrCDhEepm13")]
        public string DevAddr07hRegAddrCDhEepm13;
        [Description("R,DevAddr07hRegAddrCEhEepm14")]
        public string DevAddr07hRegAddrCEhEepm14;
        [Description("R,DevAddr07hRegAddrCFhEepm15")]
        public string DevAddr07hRegAddrCFhEepm15;

        [Description("R,DevAddr08hRegAddr00hIout0")]
        public string DevAddr08hRegAddr00hIout0;
        [Description("R,DevAddr08hRegAddr01hIout1")]
        public string DevAddr08hRegAddr01hIout1;
        [Description("R,DevAddr08hRegAddr02hIout2")]
        public string DevAddr08hRegAddr02hIout2;
        [Description("R,DevAddr08hRegAddr03hIout3")]
        public string DevAddr08hRegAddr03hIout3;
        [Description("R,DevAddr08hRegAddr04hIout4")]
        public string DevAddr08hRegAddr04hIout4;
        [Description("R,DevAddr08hRegAddr05hIout5")]
        public string DevAddr08hRegAddr05hIout5;
        [Description("R,DevAddr08hRegAddr06hIout6")]
        public string DevAddr08hRegAddr06hIout6;
        [Description("R,DevAddr08hRegAddr07hIout7")]
        public string DevAddr08hRegAddr07hIout7;
        [Description("R,DevAddr08hRegAddr08hIout8")]
        public string DevAddr08hRegAddr08hIout8;
        [Description("R,DevAddr08hRegAddr09hIout9")]
        public string DevAddr08hRegAddr09hIout9;
        [Description("R,DevAddr08hRegAddr0AhIout10")]
        public string DevAddr08hRegAddr0AhIout10;
        [Description("R,DevAddr08hRegAddr0BhIout11")]
        public string DevAddr08hRegAddr0BhIout11;
        [Description("R,DevAddr08hRegAddr20hIoutPwm0")]
        public string DevAddr08hRegAddr20hIoutPwm0;
        [Description("R,DevAddr08hRegAddr21hIoutPwm1")]
        public string DevAddr08hRegAddr21hIoutPwm1;
        [Description("R,DevAddr08hRegAddr22hIoutPwm2")]
        public string DevAddr08hRegAddr22hIoutPwm2;
        [Description("R,DevAddr08hRegAddr23hIoutPwm3")]
        public string DevAddr08hRegAddr23hIoutPwm3;
        [Description("R,DevAddr08hRegAddr24hIoutPwm4")]
        public string DevAddr08hRegAddr24hIoutPwm4;
        [Description("R,DevAddr08hRegAddr25hIoutPwm5")]
        public string DevAddr08hRegAddr25hIoutPwm5;
        [Description("R,DevAddr08hRegAddr26hIoutPwm6")]
        public string DevAddr08hRegAddr26hIoutPwm6;
        [Description("R,DevAddr08hRegAddr27hIoutPwm7")]
        public string DevAddr08hRegAddr27hIoutPwm7;
        [Description("R,DevAddr08hRegAddr28hIoutPwm8")]
        public string DevAddr08hRegAddr28hIoutPwm8;
        [Description("R,DevAddr08hRegAddr29hIoutPwm9")]
        public string DevAddr08hRegAddr29hIoutPwm9;
        [Description("R,DevAddr08hRegAddr2AhIoutPwm10")]
        public string DevAddr08hRegAddr2AhIoutPwm10;
        [Description("R,DevAddr08hRegAddr2BhIoutPwm11")]
        public string DevAddr08hRegAddr2BhIoutPwm11;
        [Description("R,DevAddr08hRegAddr40hIoutPwml0")]
        public string DevAddr08hRegAddr40hIoutPwml0;
        [Description("R,DevAddr08hRegAddr41hIoutPwml1")]
        public string DevAddr08hRegAddr41hIoutPwml1;
        [Description("R,DevAddr08hRegAddr42hIoutPwml2")]
        public string DevAddr08hRegAddr42hIoutPwml2;
        [Description("R,DevAddr08hRegAddr43hIoutPwml3")]
        public string DevAddr08hRegAddr43hIoutPwml3;
        [Description("R,DevAddr08hRegAddr44hIoutPwml4")]
        public string DevAddr08hRegAddr44hIoutPwml4;
        [Description("R,DevAddr08hRegAddr45hIoutPwml5")]
        public string DevAddr08hRegAddr45hIoutPwml5;
        [Description("R,DevAddr08hRegAddr46hIoutPwml6")]
        public string DevAddr08hRegAddr46hIoutPwml6;
        [Description("R,DevAddr08hRegAddr47hIoutPwml7")]
        public string DevAddr08hRegAddr47hIoutPwml7;
        [Description("R,DevAddr08hRegAddr48hIoutPwml8")]
        public string DevAddr08hRegAddr48hIoutPwml8;
        [Description("R,DevAddr08hRegAddr49hIoutPwml9")]
        public string DevAddr08hRegAddr49hIoutPwml9;
        [Description("R,DevAddr08hRegAddr4AhIoutPwml10")]
        public string DevAddr08hRegAddr4AhIoutPwml10;
        [Description("R,DevAddr08hRegAddr4BhIoutPwml11")]
        public string DevAddr08hRegAddr4BhIoutPwml11;
        [Description("R,DevAddr08hRegAddr50hConfEn0")]
        public string DevAddr08hRegAddr50hConfEn0;
        [Description("R,DevAddr08hRegAddr51hConfEn1")]
        public string DevAddr08hRegAddr51hConfEn1;
        [Description("R,DevAddr08hRegAddr54hConfDiagen0")]
        public string DevAddr08hRegAddr54hConfDiagen0;
        [Description("R,DevAddr08hRegAddr55hConfDiagen1")]
        public string DevAddr08hRegAddr55hConfDiagen1;
        [Description("R,DevAddr08hRegAddr56hConfMisc0")]
        public string DevAddr08hRegAddr56hConfMisc0;
        [Description("R,DevAddr08hRegAddr57hConfMisc1")]
        public string DevAddr08hRegAddr57hConfMisc1;
        [Description("R,DevAddr08hRegAddr58hConfMisc2")]
        public string DevAddr08hRegAddr58hConfMisc2;
        [Description("R,DevAddr08hRegAddr59hConfMisc3")]
        public string DevAddr08hRegAddr59hConfMisc3;
        [Description("R,DevAddr08hRegAddr5AhConfMisc4")]
        public string DevAddr08hRegAddr5AhConfMisc4;
        [Description("R,DevAddr08hRegAddr5BhConfMisc5")]
        public string DevAddr08hRegAddr5BhConfMisc5;
        [Description("R,DevAddr08hRegAddr60hClr")]
        public string DevAddr08hRegAddr60hClr;
        [Description("R,DevAddr08hRegAddr61hConfLock")]
        public string DevAddr08hRegAddr61hConfLock;
        [Description("R,DevAddr08hRegAddr62hConfMisc6")]
        public string DevAddr08hRegAddr62hConfMisc6;
        [Description("R,DevAddr08hRegAddr63hConfMisc7")]
        public string DevAddr08hRegAddr63hConfMisc7;
        [Description("R,DevAddr08hRegAddr64hConfMisc8")]
        public string DevAddr08hRegAddr64hConfMisc8;
        [Description("R,DevAddr08hRegAddr65hConfMisc9")]
        public string DevAddr08hRegAddr65hConfMisc9;
        [Description("R,DevAddr08hRegAddr70hFlag0")]
        public string DevAddr08hRegAddr70hFlag0;
        [Description("R,DevAddr08hRegAddr71hFlag1")]
        public string DevAddr08hRegAddr71hFlag1;
        [Description("R,DevAddr08hRegAddr72hFlag2")]
        public string DevAddr08hRegAddr72hFlag2;
        [Description("R,DevAddr08hRegAddr73hFlag3")]
        public string DevAddr08hRegAddr73hFlag3;
        [Description("R,DevAddr08hRegAddr74hFlag4")]
        public string DevAddr08hRegAddr74hFlag4;
        [Description("R,DevAddr08hRegAddr75hFlag5")]
        public string DevAddr08hRegAddr75hFlag5;
        [Description("R,DevAddr08hRegAddr77hFlag7")]
        public string DevAddr08hRegAddr77hFlag7;
        [Description("R,DevAddr08hRegAddr78hFlag8")]
        public string DevAddr08hRegAddr78hFlag8;
        [Description("R,DevAddr08hRegAddr7BhFlag11")]
        public string DevAddr08hRegAddr7BhFlag11;
        [Description("R,DevAddr08hRegAddr7ChFlag12")]
        public string DevAddr08hRegAddr7ChFlag12;
        [Description("R,DevAddr08hRegAddr7DhFlag13")]
        public string DevAddr08hRegAddr7DhFlag13;
        [Description("R,DevAddr08hRegAddr7EhFlag14")]
        public string DevAddr08hRegAddr7EhFlag14;
        [Description("R,DevAddr08hRegAddr80hEepi0")]
        public string DevAddr08hRegAddr80hEepi0;
        [Description("R,DevAddr08hRegAddr81hEepi1")]
        public string DevAddr08hRegAddr81hEepi1;
        [Description("R,DevAddr08hRegAddr82hEepi2")]
        public string DevAddr08hRegAddr82hEepi2;
        [Description("R,DevAddr08hRegAddr83hEepi3")]
        public string DevAddr08hRegAddr83hEepi3;
        [Description("R,DevAddr08hRegAddr84hEepi4")]
        public string DevAddr08hRegAddr84hEepi4;
        [Description("R,DevAddr08hRegAddr85hEepi5")]
        public string DevAddr08hRegAddr85hEepi5;
        [Description("R,DevAddr08hRegAddr86hEepi6")]
        public string DevAddr08hRegAddr86hEepi6;
        [Description("R,DevAddr08hRegAddr87hEepi7")]
        public string DevAddr08hRegAddr87hEepi7;
        [Description("R,DevAddr08hRegAddr88hEepi8")]
        public string DevAddr08hRegAddr88hEepi8;
        [Description("R,DevAddr08hRegAddr89hEepi9")]
        public string DevAddr08hRegAddr89hEepi9;
        [Description("R,DevAddr08hRegAddr8AhEepi10")]
        public string DevAddr08hRegAddr8AhEepi10;
        [Description("R,DevAddr08hRegAddr8BhEepi11")]
        public string DevAddr08hRegAddr8BhEepi11;
        [Description("R,DevAddr08hRegAddrA0hEepp0")]
        public string DevAddr08hRegAddrA0hEepp0;
        [Description("R,DevAddr08hRegAddrA1hEepp1")]
        public string DevAddr08hRegAddrA1hEepp1;
        [Description("R,DevAddr08hRegAddrA2hEepp2")]
        public string DevAddr08hRegAddrA2hEepp2;
        [Description("R,DevAddr08hRegAddrA3hEepp3")]
        public string DevAddr08hRegAddrA3hEepp3;
        [Description("R,DevAddr08hRegAddrA4hEepp4")]
        public string DevAddr08hRegAddrA4hEepp4;
        [Description("R,DevAddr08hRegAddrA5hEepp5")]
        public string DevAddr08hRegAddrA5hEepp5;
        [Description("R,DevAddr08hRegAddrA6hEepp6")]
        public string DevAddr08hRegAddrA6hEepp6;
        [Description("R,DevAddr08hRegAddrA7hEepp7")]
        public string DevAddr08hRegAddrA7hEepp7;
        [Description("R,DevAddr08hRegAddrA8hEepp8")]
        public string DevAddr08hRegAddrA8hEepp8;
        [Description("R,DevAddr08hRegAddrA9hEepp9")]
        public string DevAddr08hRegAddrA9hEepp9;
        [Description("R,DevAddr08hRegAddrAAhEepp10")]
        public string DevAddr08hRegAddrAAhEepp10;
        [Description("R,DevAddr08hRegAddrABhEepp11")]
        public string DevAddr08hRegAddrABhEepp11;
        [Description("R,DevAddr08hRegAddrC0hEepm0")]
        public string DevAddr08hRegAddrC0hEepm0;
        [Description("R,DevAddr08hRegAddrC1hEepm1")]
        public string DevAddr08hRegAddrC1hEepm1;
        [Description("R,DevAddr08hRegAddrC2hEepm2")]
        public string DevAddr08hRegAddrC2hEepm2;
        [Description("R,DevAddr08hRegAddrC3hEepm3")]
        public string DevAddr08hRegAddrC3hEepm3;
        [Description("R,DevAddr08hRegAddrC4hEepm4")]
        public string DevAddr08hRegAddrC4hEepm4;
        [Description("R,DevAddr08hRegAddrC5hEepm5")]
        public string DevAddr08hRegAddrC5hEepm5;
        [Description("R,DevAddr08hRegAddrC6hEepm6")]
        public string DevAddr08hRegAddrC6hEepm6;
        [Description("R,DevAddr08hRegAddrC7hEepm7")]
        public string DevAddr08hRegAddrC7hEepm7;
        [Description("R,DevAddr08hRegAddrC8hEepm8")]
        public string DevAddr08hRegAddrC8hEepm8;
        [Description("R,DevAddr08hRegAddrC9hEepm9")]
        public string DevAddr08hRegAddrC9hEepm9;
        [Description("R,DevAddr08hRegAddrCAhEepm10")]
        public string DevAddr08hRegAddrCAhEepm10;
        [Description("R,DevAddr08hRegAddrCBhEepm11")]
        public string DevAddr08hRegAddrCBhEepm11;
        [Description("R,DevAddr08hRegAddrCChEepm12")]
        public string DevAddr08hRegAddrCChEepm12;
        [Description("R,DevAddr08hRegAddrCDhEepm13")]
        public string DevAddr08hRegAddrCDhEepm13;
        [Description("R,DevAddr08hRegAddrCEhEepm14")]
        public string DevAddr08hRegAddrCEhEepm14;
        [Description("R,DevAddr08hRegAddrCFhEepm15")]
        public string DevAddr08hRegAddrCFhEepm15;

        [Description("R,DevAddr09hRegAddr00hIout0")]
        public string DevAddr09hRegAddr00hIout0;
        [Description("R,DevAddr09hRegAddr01hIout1")]
        public string DevAddr09hRegAddr01hIout1;
        [Description("R,DevAddr09hRegAddr02hIout2")]
        public string DevAddr09hRegAddr02hIout2;
        [Description("R,DevAddr09hRegAddr03hIout3")]
        public string DevAddr09hRegAddr03hIout3;
        [Description("R,DevAddr09hRegAddr04hIout4")]
        public string DevAddr09hRegAddr04hIout4;
        [Description("R,DevAddr09hRegAddr05hIout5")]
        public string DevAddr09hRegAddr05hIout5;
        [Description("R,DevAddr09hRegAddr06hIout6")]
        public string DevAddr09hRegAddr06hIout6;
        [Description("R,DevAddr09hRegAddr07hIout7")]
        public string DevAddr09hRegAddr07hIout7;
        [Description("R,DevAddr09hRegAddr08hIout8")]
        public string DevAddr09hRegAddr08hIout8;
        [Description("R,DevAddr09hRegAddr09hIout9")]
        public string DevAddr09hRegAddr09hIout9;
        [Description("R,DevAddr09hRegAddr0AhIout10")]
        public string DevAddr09hRegAddr0AhIout10;
        [Description("R,DevAddr09hRegAddr0BhIout11")]
        public string DevAddr09hRegAddr0BhIout11;
        [Description("R,DevAddr09hRegAddr20hIoutPwm0")]
        public string DevAddr09hRegAddr20hIoutPwm0;
        [Description("R,DevAddr09hRegAddr21hIoutPwm1")]
        public string DevAddr09hRegAddr21hIoutPwm1;
        [Description("R,DevAddr09hRegAddr22hIoutPwm2")]
        public string DevAddr09hRegAddr22hIoutPwm2;
        [Description("R,DevAddr09hRegAddr23hIoutPwm3")]
        public string DevAddr09hRegAddr23hIoutPwm3;
        [Description("R,DevAddr09hRegAddr24hIoutPwm4")]
        public string DevAddr09hRegAddr24hIoutPwm4;
        [Description("R,DevAddr09hRegAddr25hIoutPwm5")]
        public string DevAddr09hRegAddr25hIoutPwm5;
        [Description("R,DevAddr09hRegAddr26hIoutPwm6")]
        public string DevAddr09hRegAddr26hIoutPwm6;
        [Description("R,DevAddr09hRegAddr27hIoutPwm7")]
        public string DevAddr09hRegAddr27hIoutPwm7;
        [Description("R,DevAddr09hRegAddr28hIoutPwm8")]
        public string DevAddr09hRegAddr28hIoutPwm8;
        [Description("R,DevAddr09hRegAddr29hIoutPwm9")]
        public string DevAddr09hRegAddr29hIoutPwm9;
        [Description("R,DevAddr09hRegAddr2AhIoutPwm10")]
        public string DevAddr09hRegAddr2AhIoutPwm10;
        [Description("R,DevAddr09hRegAddr2BhIoutPwm11")]
        public string DevAddr09hRegAddr2BhIoutPwm11;
        [Description("R,DevAddr09hRegAddr40hIoutPwml0")]
        public string DevAddr09hRegAddr40hIoutPwml0;
        [Description("R,DevAddr09hRegAddr41hIoutPwml1")]
        public string DevAddr09hRegAddr41hIoutPwml1;
        [Description("R,DevAddr09hRegAddr42hIoutPwml2")]
        public string DevAddr09hRegAddr42hIoutPwml2;
        [Description("R,DevAddr09hRegAddr43hIoutPwml3")]
        public string DevAddr09hRegAddr43hIoutPwml3;
        [Description("R,DevAddr09hRegAddr44hIoutPwml4")]
        public string DevAddr09hRegAddr44hIoutPwml4;
        [Description("R,DevAddr09hRegAddr45hIoutPwml5")]
        public string DevAddr09hRegAddr45hIoutPwml5;
        [Description("R,DevAddr09hRegAddr46hIoutPwml6")]
        public string DevAddr09hRegAddr46hIoutPwml6;
        [Description("R,DevAddr09hRegAddr47hIoutPwml7")]
        public string DevAddr09hRegAddr47hIoutPwml7;
        [Description("R,DevAddr09hRegAddr48hIoutPwml8")]
        public string DevAddr09hRegAddr48hIoutPwml8;
        [Description("R,DevAddr09hRegAddr49hIoutPwml9")]
        public string DevAddr09hRegAddr49hIoutPwml9;
        [Description("R,DevAddr09hRegAddr4AhIoutPwml10")]
        public string DevAddr09hRegAddr4AhIoutPwml10;
        [Description("R,DevAddr09hRegAddr4BhIoutPwml11")]
        public string DevAddr09hRegAddr4BhIoutPwml11;
        [Description("R,DevAddr09hRegAddr50hConfEn0")]
        public string DevAddr09hRegAddr50hConfEn0;
        [Description("R,DevAddr09hRegAddr51hConfEn1")]
        public string DevAddr09hRegAddr51hConfEn1;
        [Description("R,DevAddr09hRegAddr54hConfDiagen0")]
        public string DevAddr09hRegAddr54hConfDiagen0;
        [Description("R,DevAddr09hRegAddr55hConfDiagen1")]
        public string DevAddr09hRegAddr55hConfDiagen1;
        [Description("R,DevAddr09hRegAddr56hConfMisc0")]
        public string DevAddr09hRegAddr56hConfMisc0;
        [Description("R,DevAddr09hRegAddr57hConfMisc1")]
        public string DevAddr09hRegAddr57hConfMisc1;
        [Description("R,DevAddr09hRegAddr58hConfMisc2")]
        public string DevAddr09hRegAddr58hConfMisc2;
        [Description("R,DevAddr09hRegAddr59hConfMisc3")]
        public string DevAddr09hRegAddr59hConfMisc3;
        [Description("R,DevAddr09hRegAddr5AhConfMisc4")]
        public string DevAddr09hRegAddr5AhConfMisc4;
        [Description("R,DevAddr09hRegAddr5BhConfMisc5")]
        public string DevAddr09hRegAddr5BhConfMisc5;
        [Description("R,DevAddr09hRegAddr60hClr")]
        public string DevAddr09hRegAddr60hClr;
        [Description("R,DevAddr09hRegAddr61hConfLock")]
        public string DevAddr09hRegAddr61hConfLock;
        [Description("R,DevAddr09hRegAddr62hConfMisc6")]
        public string DevAddr09hRegAddr62hConfMisc6;
        [Description("R,DevAddr09hRegAddr63hConfMisc7")]
        public string DevAddr09hRegAddr63hConfMisc7;
        [Description("R,DevAddr09hRegAddr64hConfMisc8")]
        public string DevAddr09hRegAddr64hConfMisc8;
        [Description("R,DevAddr09hRegAddr65hConfMisc9")]
        public string DevAddr09hRegAddr65hConfMisc9;
        [Description("R,DevAddr09hRegAddr70hFlag0")]
        public string DevAddr09hRegAddr70hFlag0;
        [Description("R,DevAddr09hRegAddr71hFlag1")]
        public string DevAddr09hRegAddr71hFlag1;
        [Description("R,DevAddr09hRegAddr72hFlag2")]
        public string DevAddr09hRegAddr72hFlag2;
        [Description("R,DevAddr09hRegAddr73hFlag3")]
        public string DevAddr09hRegAddr73hFlag3;
        [Description("R,DevAddr09hRegAddr74hFlag4")]
        public string DevAddr09hRegAddr74hFlag4;
        [Description("R,DevAddr09hRegAddr75hFlag5")]
        public string DevAddr09hRegAddr75hFlag5;
        [Description("R,DevAddr09hRegAddr77hFlag7")]
        public string DevAddr09hRegAddr77hFlag7;
        [Description("R,DevAddr09hRegAddr78hFlag8")]
        public string DevAddr09hRegAddr78hFlag8;
        [Description("R,DevAddr09hRegAddr7BhFlag11")]
        public string DevAddr09hRegAddr7BhFlag11;
        [Description("R,DevAddr09hRegAddr7ChFlag12")]
        public string DevAddr09hRegAddr7ChFlag12;
        [Description("R,DevAddr09hRegAddr7DhFlag13")]
        public string DevAddr09hRegAddr7DhFlag13;
        [Description("R,DevAddr09hRegAddr7EhFlag14")]
        public string DevAddr09hRegAddr7EhFlag14;
        [Description("R,DevAddr09hRegAddr80hEepi0")]
        public string DevAddr09hRegAddr80hEepi0;
        [Description("R,DevAddr09hRegAddr81hEepi1")]
        public string DevAddr09hRegAddr81hEepi1;
        [Description("R,DevAddr09hRegAddr82hEepi2")]
        public string DevAddr09hRegAddr82hEepi2;
        [Description("R,DevAddr09hRegAddr83hEepi3")]
        public string DevAddr09hRegAddr83hEepi3;
        [Description("R,DevAddr09hRegAddr84hEepi4")]
        public string DevAddr09hRegAddr84hEepi4;
        [Description("R,DevAddr09hRegAddr85hEepi5")]
        public string DevAddr09hRegAddr85hEepi5;
        [Description("R,DevAddr09hRegAddr86hEepi6")]
        public string DevAddr09hRegAddr86hEepi6;
        [Description("R,DevAddr09hRegAddr87hEepi7")]
        public string DevAddr09hRegAddr87hEepi7;
        [Description("R,DevAddr09hRegAddr88hEepi8")]
        public string DevAddr09hRegAddr88hEepi8;
        [Description("R,DevAddr09hRegAddr89hEepi9")]
        public string DevAddr09hRegAddr89hEepi9;
        [Description("R,DevAddr09hRegAddr8AhEepi10")]
        public string DevAddr09hRegAddr8AhEepi10;
        [Description("R,DevAddr09hRegAddr8BhEepi11")]
        public string DevAddr09hRegAddr8BhEepi11;
        [Description("R,DevAddr09hRegAddrA0hEepp0")]
        public string DevAddr09hRegAddrA0hEepp0;
        [Description("R,DevAddr09hRegAddrA1hEepp1")]
        public string DevAddr09hRegAddrA1hEepp1;
        [Description("R,DevAddr09hRegAddrA2hEepp2")]
        public string DevAddr09hRegAddrA2hEepp2;
        [Description("R,DevAddr09hRegAddrA3hEepp3")]
        public string DevAddr09hRegAddrA3hEepp3;
        [Description("R,DevAddr09hRegAddrA4hEepp4")]
        public string DevAddr09hRegAddrA4hEepp4;
        [Description("R,DevAddr09hRegAddrA5hEepp5")]
        public string DevAddr09hRegAddrA5hEepp5;
        [Description("R,DevAddr09hRegAddrA6hEepp6")]
        public string DevAddr09hRegAddrA6hEepp6;
        [Description("R,DevAddr09hRegAddrA7hEepp7")]
        public string DevAddr09hRegAddrA7hEepp7;
        [Description("R,DevAddr09hRegAddrA8hEepp8")]
        public string DevAddr09hRegAddrA8hEepp8;
        [Description("R,DevAddr09hRegAddrA9hEepp9")]
        public string DevAddr09hRegAddrA9hEepp9;
        [Description("R,DevAddr09hRegAddrAAhEepp10")]
        public string DevAddr09hRegAddrAAhEepp10;
        [Description("R,DevAddr09hRegAddrABhEepp11")]
        public string DevAddr09hRegAddrABhEepp11;
        [Description("R,DevAddr09hRegAddrC0hEepm0")]
        public string DevAddr09hRegAddrC0hEepm0;
        [Description("R,DevAddr09hRegAddrC1hEepm1")]
        public string DevAddr09hRegAddrC1hEepm1;
        [Description("R,DevAddr09hRegAddrC2hEepm2")]
        public string DevAddr09hRegAddrC2hEepm2;
        [Description("R,DevAddr09hRegAddrC3hEepm3")]
        public string DevAddr09hRegAddrC3hEepm3;
        [Description("R,DevAddr09hRegAddrC4hEepm4")]
        public string DevAddr09hRegAddrC4hEepm4;
        [Description("R,DevAddr09hRegAddrC5hEepm5")]
        public string DevAddr09hRegAddrC5hEepm5;
        [Description("R,DevAddr09hRegAddrC6hEepm6")]
        public string DevAddr09hRegAddrC6hEepm6;
        [Description("R,DevAddr09hRegAddrC7hEepm7")]
        public string DevAddr09hRegAddrC7hEepm7;
        [Description("R,DevAddr09hRegAddrC8hEepm8")]
        public string DevAddr09hRegAddrC8hEepm8;
        [Description("R,DevAddr09hRegAddrC9hEepm9")]
        public string DevAddr09hRegAddrC9hEepm9;
        [Description("R,DevAddr09hRegAddrCAhEepm10")]
        public string DevAddr09hRegAddrCAhEepm10;
        [Description("R,DevAddr09hRegAddrCBhEepm11")]
        public string DevAddr09hRegAddrCBhEepm11;
        [Description("R,DevAddr09hRegAddrCChEepm12")]
        public string DevAddr09hRegAddrCChEepm12;
        [Description("R,DevAddr09hRegAddrCDhEepm13")]
        public string DevAddr09hRegAddrCDhEepm13;
        [Description("R,DevAddr09hRegAddrCEhEepm14")]
        public string DevAddr09hRegAddrCEhEepm14;
        [Description("R,DevAddr09hRegAddrCFhEepm15")]
        public string DevAddr09hRegAddrCFhEepm15;

        [Description("R,DevAddr0AhRegAddr00hIout0")]
        public string DevAddr0AhRegAddr00hIout0;
        [Description("R,DevAddr0AhRegAddr01hIout1")]
        public string DevAddr0AhRegAddr01hIout1;
        [Description("R,DevAddr0AhRegAddr02hIout2")]
        public string DevAddr0AhRegAddr02hIout2;
        [Description("R,DevAddr0AhRegAddr03hIout3")]
        public string DevAddr0AhRegAddr03hIout3;
        [Description("R,DevAddr0AhRegAddr04hIout4")]
        public string DevAddr0AhRegAddr04hIout4;
        [Description("R,DevAddr0AhRegAddr05hIout5")]
        public string DevAddr0AhRegAddr05hIout5;
        [Description("R,DevAddr0AhRegAddr06hIout6")]
        public string DevAddr0AhRegAddr06hIout6;
        [Description("R,DevAddr0AhRegAddr07hIout7")]
        public string DevAddr0AhRegAddr07hIout7;
        [Description("R,DevAddr0AhRegAddr08hIout8")]
        public string DevAddr0AhRegAddr08hIout8;
        [Description("R,DevAddr0AhRegAddr09hIout9")]
        public string DevAddr0AhRegAddr09hIout9;
        [Description("R,DevAddr0AhRegAddr0AhIout10")]
        public string DevAddr0AhRegAddr0AhIout10;
        [Description("R,DevAddr0AhRegAddr0BhIout11")]
        public string DevAddr0AhRegAddr0BhIout11;
        [Description("R,DevAddr0AhRegAddr20hIoutPwm0")]
        public string DevAddr0AhRegAddr20hIoutPwm0;
        [Description("R,DevAddr0AhRegAddr21hIoutPwm1")]
        public string DevAddr0AhRegAddr21hIoutPwm1;
        [Description("R,DevAddr0AhRegAddr22hIoutPwm2")]
        public string DevAddr0AhRegAddr22hIoutPwm2;
        [Description("R,DevAddr0AhRegAddr23hIoutPwm3")]
        public string DevAddr0AhRegAddr23hIoutPwm3;
        [Description("R,DevAddr0AhRegAddr24hIoutPwm4")]
        public string DevAddr0AhRegAddr24hIoutPwm4;
        [Description("R,DevAddr0AhRegAddr25hIoutPwm5")]
        public string DevAddr0AhRegAddr25hIoutPwm5;
        [Description("R,DevAddr0AhRegAddr26hIoutPwm6")]
        public string DevAddr0AhRegAddr26hIoutPwm6;
        [Description("R,DevAddr0AhRegAddr27hIoutPwm7")]
        public string DevAddr0AhRegAddr27hIoutPwm7;
        [Description("R,DevAddr0AhRegAddr28hIoutPwm8")]
        public string DevAddr0AhRegAddr28hIoutPwm8;
        [Description("R,DevAddr0AhRegAddr29hIoutPwm9")]
        public string DevAddr0AhRegAddr29hIoutPwm9;
        [Description("R,DevAddr0AhRegAddr2AhIoutPwm10")]
        public string DevAddr0AhRegAddr2AhIoutPwm10;
        [Description("R,DevAddr0AhRegAddr2BhIoutPwm11")]
        public string DevAddr0AhRegAddr2BhIoutPwm11;
        [Description("R,DevAddr0AhRegAddr40hIoutPwml0")]
        public string DevAddr0AhRegAddr40hIoutPwml0;
        [Description("R,DevAddr0AhRegAddr41hIoutPwml1")]
        public string DevAddr0AhRegAddr41hIoutPwml1;
        [Description("R,DevAddr0AhRegAddr42hIoutPwml2")]
        public string DevAddr0AhRegAddr42hIoutPwml2;
        [Description("R,DevAddr0AhRegAddr43hIoutPwml3")]
        public string DevAddr0AhRegAddr43hIoutPwml3;
        [Description("R,DevAddr0AhRegAddr44hIoutPwml4")]
        public string DevAddr0AhRegAddr44hIoutPwml4;
        [Description("R,DevAddr0AhRegAddr45hIoutPwml5")]
        public string DevAddr0AhRegAddr45hIoutPwml5;
        [Description("R,DevAddr0AhRegAddr46hIoutPwml6")]
        public string DevAddr0AhRegAddr46hIoutPwml6;
        [Description("R,DevAddr0AhRegAddr47hIoutPwml7")]
        public string DevAddr0AhRegAddr47hIoutPwml7;
        [Description("R,DevAddr0AhRegAddr48hIoutPwml8")]
        public string DevAddr0AhRegAddr48hIoutPwml8;
        [Description("R,DevAddr0AhRegAddr49hIoutPwml9")]
        public string DevAddr0AhRegAddr49hIoutPwml9;
        [Description("R,DevAddr0AhRegAddr4AhIoutPwml10")]
        public string DevAddr0AhRegAddr4AhIoutPwml10;
        [Description("R,DevAddr0AhRegAddr4BhIoutPwml11")]
        public string DevAddr0AhRegAddr4BhIoutPwml11;
        [Description("R,DevAddr0AhRegAddr50hConfEn0")]
        public string DevAddr0AhRegAddr50hConfEn0;
        [Description("R,DevAddr0AhRegAddr51hConfEn1")]
        public string DevAddr0AhRegAddr51hConfEn1;
        [Description("R,DevAddr0AhRegAddr54hConfDiagen0")]
        public string DevAddr0AhRegAddr54hConfDiagen0;
        [Description("R,DevAddr0AhRegAddr55hConfDiagen1")]
        public string DevAddr0AhRegAddr55hConfDiagen1;
        [Description("R,DevAddr0AhRegAddr56hConfMisc0")]
        public string DevAddr0AhRegAddr56hConfMisc0;
        [Description("R,DevAddr0AhRegAddr57hConfMisc1")]
        public string DevAddr0AhRegAddr57hConfMisc1;
        [Description("R,DevAddr0AhRegAddr58hConfMisc2")]
        public string DevAddr0AhRegAddr58hConfMisc2;
        [Description("R,DevAddr0AhRegAddr59hConfMisc3")]
        public string DevAddr0AhRegAddr59hConfMisc3;
        [Description("R,DevAddr0AhRegAddr5AhConfMisc4")]
        public string DevAddr0AhRegAddr5AhConfMisc4;
        [Description("R,DevAddr0AhRegAddr5BhConfMisc5")]
        public string DevAddr0AhRegAddr5BhConfMisc5;
        [Description("R,DevAddr0AhRegAddr60hClr")]
        public string DevAddr0AhRegAddr60hClr;
        [Description("R,DevAddr0AhRegAddr61hConfLock")]
        public string DevAddr0AhRegAddr61hConfLock;
        [Description("R,DevAddr0AhRegAddr62hConfMisc6")]
        public string DevAddr0AhRegAddr62hConfMisc6;
        [Description("R,DevAddr0AhRegAddr63hConfMisc7")]
        public string DevAddr0AhRegAddr63hConfMisc7;
        [Description("R,DevAddr0AhRegAddr64hConfMisc8")]
        public string DevAddr0AhRegAddr64hConfMisc8;
        [Description("R,DevAddr0AhRegAddr65hConfMisc9")]
        public string DevAddr0AhRegAddr65hConfMisc9;
        [Description("R,DevAddr0AhRegAddr70hFlag0")]
        public string DevAddr0AhRegAddr70hFlag0;
        [Description("R,DevAddr0AhRegAddr71hFlag1")]
        public string DevAddr0AhRegAddr71hFlag1;
        [Description("R,DevAddr0AhRegAddr72hFlag2")]
        public string DevAddr0AhRegAddr72hFlag2;
        [Description("R,DevAddr0AhRegAddr73hFlag3")]
        public string DevAddr0AhRegAddr73hFlag3;
        [Description("R,DevAddr0AhRegAddr74hFlag4")]
        public string DevAddr0AhRegAddr74hFlag4;
        [Description("R,DevAddr0AhRegAddr75hFlag5")]
        public string DevAddr0AhRegAddr75hFlag5;
        [Description("R,DevAddr0AhRegAddr77hFlag7")]
        public string DevAddr0AhRegAddr77hFlag7;
        [Description("R,DevAddr0AhRegAddr78hFlag8")]
        public string DevAddr0AhRegAddr78hFlag8;
        [Description("R,DevAddr0AhRegAddr7BhFlag11")]
        public string DevAddr0AhRegAddr7BhFlag11;
        [Description("R,DevAddr0AhRegAddr7ChFlag12")]
        public string DevAddr0AhRegAddr7ChFlag12;
        [Description("R,DevAddr0AhRegAddr7DhFlag13")]
        public string DevAddr0AhRegAddr7DhFlag13;
        [Description("R,DevAddr0AhRegAddr7EhFlag14")]
        public string DevAddr0AhRegAddr7EhFlag14;
        [Description("R,DevAddr0AhRegAddr80hEepi0")]
        public string DevAddr0AhRegAddr80hEepi0;
        [Description("R,DevAddr0AhRegAddr81hEepi1")]
        public string DevAddr0AhRegAddr81hEepi1;
        [Description("R,DevAddr0AhRegAddr82hEepi2")]
        public string DevAddr0AhRegAddr82hEepi2;
        [Description("R,DevAddr0AhRegAddr83hEepi3")]
        public string DevAddr0AhRegAddr83hEepi3;
        [Description("R,DevAddr0AhRegAddr84hEepi4")]
        public string DevAddr0AhRegAddr84hEepi4;
        [Description("R,DevAddr0AhRegAddr85hEepi5")]
        public string DevAddr0AhRegAddr85hEepi5;
        [Description("R,DevAddr0AhRegAddr86hEepi6")]
        public string DevAddr0AhRegAddr86hEepi6;
        [Description("R,DevAddr0AhRegAddr87hEepi7")]
        public string DevAddr0AhRegAddr87hEepi7;
        [Description("R,DevAddr0AhRegAddr88hEepi8")]
        public string DevAddr0AhRegAddr88hEepi8;
        [Description("R,DevAddr0AhRegAddr89hEepi9")]
        public string DevAddr0AhRegAddr89hEepi9;
        [Description("R,DevAddr0AhRegAddr8AhEepi10")]
        public string DevAddr0AhRegAddr8AhEepi10;
        [Description("R,DevAddr0AhRegAddr8BhEepi11")]
        public string DevAddr0AhRegAddr8BhEepi11;
        [Description("R,DevAddr0AhRegAddrA0hEepp0")]
        public string DevAddr0AhRegAddrA0hEepp0;
        [Description("R,DevAddr0AhRegAddrA1hEepp1")]
        public string DevAddr0AhRegAddrA1hEepp1;
        [Description("R,DevAddr0AhRegAddrA2hEepp2")]
        public string DevAddr0AhRegAddrA2hEepp2;
        [Description("R,DevAddr0AhRegAddrA3hEepp3")]
        public string DevAddr0AhRegAddrA3hEepp3;
        [Description("R,DevAddr0AhRegAddrA4hEepp4")]
        public string DevAddr0AhRegAddrA4hEepp4;
        [Description("R,DevAddr0AhRegAddrA5hEepp5")]
        public string DevAddr0AhRegAddrA5hEepp5;
        [Description("R,DevAddr0AhRegAddrA6hEepp6")]
        public string DevAddr0AhRegAddrA6hEepp6;
        [Description("R,DevAddr0AhRegAddrA7hEepp7")]
        public string DevAddr0AhRegAddrA7hEepp7;
        [Description("R,DevAddr0AhRegAddrA8hEepp8")]
        public string DevAddr0AhRegAddrA8hEepp8;
        [Description("R,DevAddr0AhRegAddrA9hEepp9")]
        public string DevAddr0AhRegAddrA9hEepp9;
        [Description("R,DevAddr0AhRegAddrAAhEepp10")]
        public string DevAddr0AhRegAddrAAhEepp10;
        [Description("R,DevAddr0AhRegAddrABhEepp11")]
        public string DevAddr0AhRegAddrABhEepp11;
        [Description("R,DevAddr0AhRegAddrC0hEepm0")]
        public string DevAddr0AhRegAddrC0hEepm0;
        [Description("R,DevAddr0AhRegAddrC1hEepm1")]
        public string DevAddr0AhRegAddrC1hEepm1;
        [Description("R,DevAddr0AhRegAddrC2hEepm2")]
        public string DevAddr0AhRegAddrC2hEepm2;
        [Description("R,DevAddr0AhRegAddrC3hEepm3")]
        public string DevAddr0AhRegAddrC3hEepm3;
        [Description("R,DevAddr0AhRegAddrC4hEepm4")]
        public string DevAddr0AhRegAddrC4hEepm4;
        [Description("R,DevAddr0AhRegAddrC5hEepm5")]
        public string DevAddr0AhRegAddrC5hEepm5;
        [Description("R,DevAddr0AhRegAddrC6hEepm6")]
        public string DevAddr0AhRegAddrC6hEepm6;
        [Description("R,DevAddr0AhRegAddrC7hEepm7")]
        public string DevAddr0AhRegAddrC7hEepm7;
        [Description("R,DevAddr0AhRegAddrC8hEepm8")]
        public string DevAddr0AhRegAddrC8hEepm8;
        [Description("R,DevAddr0AhRegAddrC9hEepm9")]
        public string DevAddr0AhRegAddrC9hEepm9;
        [Description("R,DevAddr0AhRegAddrCAhEepm10")]
        public string DevAddr0AhRegAddrCAhEepm10;
        [Description("R,DevAddr0AhRegAddrCBhEepm11")]
        public string DevAddr0AhRegAddrCBhEepm11;
        [Description("R,DevAddr0AhRegAddrCChEepm12")]
        public string DevAddr0AhRegAddrCChEepm12;
        [Description("R,DevAddr0AhRegAddrCDhEepm13")]
        public string DevAddr0AhRegAddrCDhEepm13;
        [Description("R,DevAddr0AhRegAddrCEhEepm14")]
        public string DevAddr0AhRegAddrCEhEepm14;
        [Description("R,DevAddr0AhRegAddrCFhEepm15")]
        public string DevAddr0AhRegAddrCFhEepm15;

        [Description("R,DevAddr0BhRegAddr00hIout0")]
        public string DevAddr0BhRegAddr00hIout0;
        [Description("R,DevAddr0BhRegAddr01hIout1")]
        public string DevAddr0BhRegAddr01hIout1;
        [Description("R,DevAddr0BhRegAddr02hIout2")]
        public string DevAddr0BhRegAddr02hIout2;
        [Description("R,DevAddr0BhRegAddr03hIout3")]
        public string DevAddr0BhRegAddr03hIout3;
        [Description("R,DevAddr0BhRegAddr04hIout4")]
        public string DevAddr0BhRegAddr04hIout4;
        [Description("R,DevAddr0BhRegAddr05hIout5")]
        public string DevAddr0BhRegAddr05hIout5;
        [Description("R,DevAddr0BhRegAddr06hIout6")]
        public string DevAddr0BhRegAddr06hIout6;
        [Description("R,DevAddr0BhRegAddr07hIout7")]
        public string DevAddr0BhRegAddr07hIout7;
        [Description("R,DevAddr0BhRegAddr08hIout8")]
        public string DevAddr0BhRegAddr08hIout8;
        [Description("R,DevAddr0BhRegAddr09hIout9")]
        public string DevAddr0BhRegAddr09hIout9;
        [Description("R,DevAddr0BhRegAddr0AhIout10")]
        public string DevAddr0BhRegAddr0AhIout10;
        [Description("R,DevAddr0BhRegAddr0BhIout11")]
        public string DevAddr0BhRegAddr0BhIout11;
        [Description("R,DevAddr0BhRegAddr20hIoutPwm0")]
        public string DevAddr0BhRegAddr20hIoutPwm0;
        [Description("R,DevAddr0BhRegAddr21hIoutPwm1")]
        public string DevAddr0BhRegAddr21hIoutPwm1;
        [Description("R,DevAddr0BhRegAddr22hIoutPwm2")]
        public string DevAddr0BhRegAddr22hIoutPwm2;
        [Description("R,DevAddr0BhRegAddr23hIoutPwm3")]
        public string DevAddr0BhRegAddr23hIoutPwm3;
        [Description("R,DevAddr0BhRegAddr24hIoutPwm4")]
        public string DevAddr0BhRegAddr24hIoutPwm4;
        [Description("R,DevAddr0BhRegAddr25hIoutPwm5")]
        public string DevAddr0BhRegAddr25hIoutPwm5;
        [Description("R,DevAddr0BhRegAddr26hIoutPwm6")]
        public string DevAddr0BhRegAddr26hIoutPwm6;
        [Description("R,DevAddr0BhRegAddr27hIoutPwm7")]
        public string DevAddr0BhRegAddr27hIoutPwm7;
        [Description("R,DevAddr0BhRegAddr28hIoutPwm8")]
        public string DevAddr0BhRegAddr28hIoutPwm8;
        [Description("R,DevAddr0BhRegAddr29hIoutPwm9")]
        public string DevAddr0BhRegAddr29hIoutPwm9;
        [Description("R,DevAddr0BhRegAddr2AhIoutPwm10")]
        public string DevAddr0BhRegAddr2AhIoutPwm10;
        [Description("R,DevAddr0BhRegAddr2BhIoutPwm11")]
        public string DevAddr0BhRegAddr2BhIoutPwm11;
        [Description("R,DevAddr0BhRegAddr40hIoutPwml0")]
        public string DevAddr0BhRegAddr40hIoutPwml0;
        [Description("R,DevAddr0BhRegAddr41hIoutPwml1")]
        public string DevAddr0BhRegAddr41hIoutPwml1;
        [Description("R,DevAddr0BhRegAddr42hIoutPwml2")]
        public string DevAddr0BhRegAddr42hIoutPwml2;
        [Description("R,DevAddr0BhRegAddr43hIoutPwml3")]
        public string DevAddr0BhRegAddr43hIoutPwml3;
        [Description("R,DevAddr0BhRegAddr44hIoutPwml4")]
        public string DevAddr0BhRegAddr44hIoutPwml4;
        [Description("R,DevAddr0BhRegAddr45hIoutPwml5")]
        public string DevAddr0BhRegAddr45hIoutPwml5;
        [Description("R,DevAddr0BhRegAddr46hIoutPwml6")]
        public string DevAddr0BhRegAddr46hIoutPwml6;
        [Description("R,DevAddr0BhRegAddr47hIoutPwml7")]
        public string DevAddr0BhRegAddr47hIoutPwml7;
        [Description("R,DevAddr0BhRegAddr48hIoutPwml8")]
        public string DevAddr0BhRegAddr48hIoutPwml8;
        [Description("R,DevAddr0BhRegAddr49hIoutPwml9")]
        public string DevAddr0BhRegAddr49hIoutPwml9;
        [Description("R,DevAddr0BhRegAddr4AhIoutPwml10")]
        public string DevAddr0BhRegAddr4AhIoutPwml10;
        [Description("R,DevAddr0BhRegAddr4BhIoutPwml11")]
        public string DevAddr0BhRegAddr4BhIoutPwml11;
        [Description("R,DevAddr0BhRegAddr50hConfEn0")]
        public string DevAddr0BhRegAddr50hConfEn0;
        [Description("R,DevAddr0BhRegAddr51hConfEn1")]
        public string DevAddr0BhRegAddr51hConfEn1;
        [Description("R,DevAddr0BhRegAddr54hConfDiagen0")]
        public string DevAddr0BhRegAddr54hConfDiagen0;
        [Description("R,DevAddr0BhRegAddr55hConfDiagen1")]
        public string DevAddr0BhRegAddr55hConfDiagen1;
        [Description("R,DevAddr0BhRegAddr56hConfMisc0")]
        public string DevAddr0BhRegAddr56hConfMisc0;
        [Description("R,DevAddr0BhRegAddr57hConfMisc1")]
        public string DevAddr0BhRegAddr57hConfMisc1;
        [Description("R,DevAddr0BhRegAddr58hConfMisc2")]
        public string DevAddr0BhRegAddr58hConfMisc2;
        [Description("R,DevAddr0BhRegAddr59hConfMisc3")]
        public string DevAddr0BhRegAddr59hConfMisc3;
        [Description("R,DevAddr0BhRegAddr5AhConfMisc4")]
        public string DevAddr0BhRegAddr5AhConfMisc4;
        [Description("R,DevAddr0BhRegAddr5BhConfMisc5")]
        public string DevAddr0BhRegAddr5BhConfMisc5;
        [Description("R,DevAddr0BhRegAddr60hClr")]
        public string DevAddr0BhRegAddr60hClr;
        [Description("R,DevAddr0BhRegAddr61hConfLock")]
        public string DevAddr0BhRegAddr61hConfLock;
        [Description("R,DevAddr0BhRegAddr62hConfMisc6")]
        public string DevAddr0BhRegAddr62hConfMisc6;
        [Description("R,DevAddr0BhRegAddr63hConfMisc7")]
        public string DevAddr0BhRegAddr63hConfMisc7;
        [Description("R,DevAddr0BhRegAddr64hConfMisc8")]
        public string DevAddr0BhRegAddr64hConfMisc8;
        [Description("R,DevAddr0BhRegAddr65hConfMisc9")]
        public string DevAddr0BhRegAddr65hConfMisc9;
        [Description("R,DevAddr0BhRegAddr70hFlag0")]
        public string DevAddr0BhRegAddr70hFlag0;
        [Description("R,DevAddr0BhRegAddr71hFlag1")]
        public string DevAddr0BhRegAddr71hFlag1;
        [Description("R,DevAddr0BhRegAddr72hFlag2")]
        public string DevAddr0BhRegAddr72hFlag2;
        [Description("R,DevAddr0BhRegAddr73hFlag3")]
        public string DevAddr0BhRegAddr73hFlag3;
        [Description("R,DevAddr0BhRegAddr74hFlag4")]
        public string DevAddr0BhRegAddr74hFlag4;
        [Description("R,DevAddr0BhRegAddr75hFlag5")]
        public string DevAddr0BhRegAddr75hFlag5;
        [Description("R,DevAddr0BhRegAddr77hFlag7")]
        public string DevAddr0BhRegAddr77hFlag7;
        [Description("R,DevAddr0BhRegAddr78hFlag8")]
        public string DevAddr0BhRegAddr78hFlag8;
        [Description("R,DevAddr0BhRegAddr7BhFlag11")]
        public string DevAddr0BhRegAddr7BhFlag11;
        [Description("R,DevAddr0BhRegAddr7ChFlag12")]
        public string DevAddr0BhRegAddr7ChFlag12;
        [Description("R,DevAddr0BhRegAddr7DhFlag13")]
        public string DevAddr0BhRegAddr7DhFlag13;
        [Description("R,DevAddr0BhRegAddr7EhFlag14")]
        public string DevAddr0BhRegAddr7EhFlag14;
        [Description("R,DevAddr0BhRegAddr80hEepi0")]
        public string DevAddr0BhRegAddr80hEepi0;
        [Description("R,DevAddr0BhRegAddr81hEepi1")]
        public string DevAddr0BhRegAddr81hEepi1;
        [Description("R,DevAddr0BhRegAddr82hEepi2")]
        public string DevAddr0BhRegAddr82hEepi2;
        [Description("R,DevAddr0BhRegAddr83hEepi3")]
        public string DevAddr0BhRegAddr83hEepi3;
        [Description("R,DevAddr0BhRegAddr84hEepi4")]
        public string DevAddr0BhRegAddr84hEepi4;
        [Description("R,DevAddr0BhRegAddr85hEepi5")]
        public string DevAddr0BhRegAddr85hEepi5;
        [Description("R,DevAddr0BhRegAddr86hEepi6")]
        public string DevAddr0BhRegAddr86hEepi6;
        [Description("R,DevAddr0BhRegAddr87hEepi7")]
        public string DevAddr0BhRegAddr87hEepi7;
        [Description("R,DevAddr0BhRegAddr88hEepi8")]
        public string DevAddr0BhRegAddr88hEepi8;
        [Description("R,DevAddr0BhRegAddr89hEepi9")]
        public string DevAddr0BhRegAddr89hEepi9;
        [Description("R,DevAddr0BhRegAddr8AhEepi10")]
        public string DevAddr0BhRegAddr8AhEepi10;
        [Description("R,DevAddr0BhRegAddr8BhEepi11")]
        public string DevAddr0BhRegAddr8BhEepi11;
        [Description("R,DevAddr0BhRegAddrA0hEepp0")]
        public string DevAddr0BhRegAddrA0hEepp0;
        [Description("R,DevAddr0BhRegAddrA1hEepp1")]
        public string DevAddr0BhRegAddrA1hEepp1;
        [Description("R,DevAddr0BhRegAddrA2hEepp2")]
        public string DevAddr0BhRegAddrA2hEepp2;
        [Description("R,DevAddr0BhRegAddrA3hEepp3")]
        public string DevAddr0BhRegAddrA3hEepp3;
        [Description("R,DevAddr0BhRegAddrA4hEepp4")]
        public string DevAddr0BhRegAddrA4hEepp4;
        [Description("R,DevAddr0BhRegAddrA5hEepp5")]
        public string DevAddr0BhRegAddrA5hEepp5;
        [Description("R,DevAddr0BhRegAddrA6hEepp6")]
        public string DevAddr0BhRegAddrA6hEepp6;
        [Description("R,DevAddr0BhRegAddrA7hEepp7")]
        public string DevAddr0BhRegAddrA7hEepp7;
        [Description("R,DevAddr0BhRegAddrA8hEepp8")]
        public string DevAddr0BhRegAddrA8hEepp8;
        [Description("R,DevAddr0BhRegAddrA9hEepp9")]
        public string DevAddr0BhRegAddrA9hEepp9;
        [Description("R,DevAddr0BhRegAddrAAhEepp10")]
        public string DevAddr0BhRegAddrAAhEepp10;
        [Description("R,DevAddr0BhRegAddrABhEepp11")]
        public string DevAddr0BhRegAddrABhEepp11;
        [Description("R,DevAddr0BhRegAddrC0hEepm0")]
        public string DevAddr0BhRegAddrC0hEepm0;
        [Description("R,DevAddr0BhRegAddrC1hEepm1")]
        public string DevAddr0BhRegAddrC1hEepm1;
        [Description("R,DevAddr0BhRegAddrC2hEepm2")]
        public string DevAddr0BhRegAddrC2hEepm2;
        [Description("R,DevAddr0BhRegAddrC3hEepm3")]
        public string DevAddr0BhRegAddrC3hEepm3;
        [Description("R,DevAddr0BhRegAddrC4hEepm4")]
        public string DevAddr0BhRegAddrC4hEepm4;
        [Description("R,DevAddr0BhRegAddrC5hEepm5")]
        public string DevAddr0BhRegAddrC5hEepm5;
        [Description("R,DevAddr0BhRegAddrC6hEepm6")]
        public string DevAddr0BhRegAddrC6hEepm6;
        [Description("R,DevAddr0BhRegAddrC7hEepm7")]
        public string DevAddr0BhRegAddrC7hEepm7;
        [Description("R,DevAddr0BhRegAddrC8hEepm8")]
        public string DevAddr0BhRegAddrC8hEepm8;
        [Description("R,DevAddr0BhRegAddrC9hEepm9")]
        public string DevAddr0BhRegAddrC9hEepm9;
        [Description("R,DevAddr0BhRegAddrCAhEepm10")]
        public string DevAddr0BhRegAddrCAhEepm10;
        [Description("R,DevAddr0BhRegAddrCBhEepm11")]
        public string DevAddr0BhRegAddrCBhEepm11;
        [Description("R,DevAddr0BhRegAddrCChEepm12")]
        public string DevAddr0BhRegAddrCChEepm12;
        [Description("R,DevAddr0BhRegAddrCDhEepm13")]
        public string DevAddr0BhRegAddrCDhEepm13;
        [Description("R,DevAddr0BhRegAddrCEhEepm14")]
        public string DevAddr0BhRegAddrCEhEepm14;
        [Description("R,DevAddr0BhRegAddrCFhEepm15")]
        public string DevAddr0BhRegAddrCFhEepm15;

        [Description("R,DevAddr0ChRegAddr00hIout0")]
        public string DevAddr0ChRegAddr00hIout0;
        [Description("R,DevAddr0ChRegAddr01hIout1")]
        public string DevAddr0ChRegAddr01hIout1;
        [Description("R,DevAddr0ChRegAddr02hIout2")]
        public string DevAddr0ChRegAddr02hIout2;
        [Description("R,DevAddr0ChRegAddr03hIout3")]
        public string DevAddr0ChRegAddr03hIout3;
        [Description("R,DevAddr0ChRegAddr04hIout4")]
        public string DevAddr0ChRegAddr04hIout4;
        [Description("R,DevAddr0ChRegAddr05hIout5")]
        public string DevAddr0ChRegAddr05hIout5;
        [Description("R,DevAddr0ChRegAddr06hIout6")]
        public string DevAddr0ChRegAddr06hIout6;
        [Description("R,DevAddr0ChRegAddr07hIout7")]
        public string DevAddr0ChRegAddr07hIout7;
        [Description("R,DevAddr0ChRegAddr08hIout8")]
        public string DevAddr0ChRegAddr08hIout8;
        [Description("R,DevAddr0ChRegAddr09hIout9")]
        public string DevAddr0ChRegAddr09hIout9;
        [Description("R,DevAddr0ChRegAddr0AhIout10")]
        public string DevAddr0ChRegAddr0AhIout10;
        [Description("R,DevAddr0ChRegAddr0BhIout11")]
        public string DevAddr0ChRegAddr0BhIout11;
        [Description("R,DevAddr0ChRegAddr20hIoutPwm0")]
        public string DevAddr0ChRegAddr20hIoutPwm0;
        [Description("R,DevAddr0ChRegAddr21hIoutPwm1")]
        public string DevAddr0ChRegAddr21hIoutPwm1;
        [Description("R,DevAddr0ChRegAddr22hIoutPwm2")]
        public string DevAddr0ChRegAddr22hIoutPwm2;
        [Description("R,DevAddr0ChRegAddr23hIoutPwm3")]
        public string DevAddr0ChRegAddr23hIoutPwm3;
        [Description("R,DevAddr0ChRegAddr24hIoutPwm4")]
        public string DevAddr0ChRegAddr24hIoutPwm4;
        [Description("R,DevAddr0ChRegAddr25hIoutPwm5")]
        public string DevAddr0ChRegAddr25hIoutPwm5;
        [Description("R,DevAddr0ChRegAddr26hIoutPwm6")]
        public string DevAddr0ChRegAddr26hIoutPwm6;
        [Description("R,DevAddr0ChRegAddr27hIoutPwm7")]
        public string DevAddr0ChRegAddr27hIoutPwm7;
        [Description("R,DevAddr0ChRegAddr28hIoutPwm8")]
        public string DevAddr0ChRegAddr28hIoutPwm8;
        [Description("R,DevAddr0ChRegAddr29hIoutPwm9")]
        public string DevAddr0ChRegAddr29hIoutPwm9;
        [Description("R,DevAddr0ChRegAddr2AhIoutPwm10")]
        public string DevAddr0ChRegAddr2AhIoutPwm10;
        [Description("R,DevAddr0ChRegAddr2BhIoutPwm11")]
        public string DevAddr0ChRegAddr2BhIoutPwm11;
        [Description("R,DevAddr0ChRegAddr40hIoutPwml0")]
        public string DevAddr0ChRegAddr40hIoutPwml0;
        [Description("R,DevAddr0ChRegAddr41hIoutPwml1")]
        public string DevAddr0ChRegAddr41hIoutPwml1;
        [Description("R,DevAddr0ChRegAddr42hIoutPwml2")]
        public string DevAddr0ChRegAddr42hIoutPwml2;
        [Description("R,DevAddr0ChRegAddr43hIoutPwml3")]
        public string DevAddr0ChRegAddr43hIoutPwml3;
        [Description("R,DevAddr0ChRegAddr44hIoutPwml4")]
        public string DevAddr0ChRegAddr44hIoutPwml4;
        [Description("R,DevAddr0ChRegAddr45hIoutPwml5")]
        public string DevAddr0ChRegAddr45hIoutPwml5;
        [Description("R,DevAddr0ChRegAddr46hIoutPwml6")]
        public string DevAddr0ChRegAddr46hIoutPwml6;
        [Description("R,DevAddr0ChRegAddr47hIoutPwml7")]
        public string DevAddr0ChRegAddr47hIoutPwml7;
        [Description("R,DevAddr0ChRegAddr48hIoutPwml8")]
        public string DevAddr0ChRegAddr48hIoutPwml8;
        [Description("R,DevAddr0ChRegAddr49hIoutPwml9")]
        public string DevAddr0ChRegAddr49hIoutPwml9;
        [Description("R,DevAddr0ChRegAddr4AhIoutPwml10")]
        public string DevAddr0ChRegAddr4AhIoutPwml10;
        [Description("R,DevAddr0ChRegAddr4BhIoutPwml11")]
        public string DevAddr0ChRegAddr4BhIoutPwml11;
        [Description("R,DevAddr0ChRegAddr50hConfEn0")]
        public string DevAddr0ChRegAddr50hConfEn0;
        [Description("R,DevAddr0ChRegAddr51hConfEn1")]
        public string DevAddr0ChRegAddr51hConfEn1;
        [Description("R,DevAddr0ChRegAddr54hConfDiagen0")]
        public string DevAddr0ChRegAddr54hConfDiagen0;
        [Description("R,DevAddr0ChRegAddr55hConfDiagen1")]
        public string DevAddr0ChRegAddr55hConfDiagen1;
        [Description("R,DevAddr0ChRegAddr56hConfMisc0")]
        public string DevAddr0ChRegAddr56hConfMisc0;
        [Description("R,DevAddr0ChRegAddr57hConfMisc1")]
        public string DevAddr0ChRegAddr57hConfMisc1;
        [Description("R,DevAddr0ChRegAddr58hConfMisc2")]
        public string DevAddr0ChRegAddr58hConfMisc2;
        [Description("R,DevAddr0ChRegAddr59hConfMisc3")]
        public string DevAddr0ChRegAddr59hConfMisc3;
        [Description("R,DevAddr0ChRegAddr5AhConfMisc4")]
        public string DevAddr0ChRegAddr5AhConfMisc4;
        [Description("R,DevAddr0ChRegAddr5BhConfMisc5")]
        public string DevAddr0ChRegAddr5BhConfMisc5;
        [Description("R,DevAddr0ChRegAddr60hClr")]
        public string DevAddr0ChRegAddr60hClr;
        [Description("R,DevAddr0ChRegAddr61hConfLock")]
        public string DevAddr0ChRegAddr61hConfLock;
        [Description("R,DevAddr0ChRegAddr62hConfMisc6")]
        public string DevAddr0ChRegAddr62hConfMisc6;
        [Description("R,DevAddr0ChRegAddr63hConfMisc7")]
        public string DevAddr0ChRegAddr63hConfMisc7;
        [Description("R,DevAddr0ChRegAddr64hConfMisc8")]
        public string DevAddr0ChRegAddr64hConfMisc8;
        [Description("R,DevAddr0ChRegAddr65hConfMisc9")]
        public string DevAddr0ChRegAddr65hConfMisc9;
        [Description("R,DevAddr0ChRegAddr70hFlag0")]
        public string DevAddr0ChRegAddr70hFlag0;
        [Description("R,DevAddr0ChRegAddr71hFlag1")]
        public string DevAddr0ChRegAddr71hFlag1;
        [Description("R,DevAddr0ChRegAddr72hFlag2")]
        public string DevAddr0ChRegAddr72hFlag2;
        [Description("R,DevAddr0ChRegAddr73hFlag3")]
        public string DevAddr0ChRegAddr73hFlag3;
        [Description("R,DevAddr0ChRegAddr74hFlag4")]
        public string DevAddr0ChRegAddr74hFlag4;
        [Description("R,DevAddr0ChRegAddr75hFlag5")]
        public string DevAddr0ChRegAddr75hFlag5;
        [Description("R,DevAddr0ChRegAddr77hFlag7")]
        public string DevAddr0ChRegAddr77hFlag7;
        [Description("R,DevAddr0ChRegAddr78hFlag8")]
        public string DevAddr0ChRegAddr78hFlag8;
        [Description("R,DevAddr0ChRegAddr7BhFlag11")]
        public string DevAddr0ChRegAddr7BhFlag11;
        [Description("R,DevAddr0ChRegAddr7ChFlag12")]
        public string DevAddr0ChRegAddr7ChFlag12;
        [Description("R,DevAddr0ChRegAddr7DhFlag13")]
        public string DevAddr0ChRegAddr7DhFlag13;
        [Description("R,DevAddr0ChRegAddr7EhFlag14")]
        public string DevAddr0ChRegAddr7EhFlag14;
        [Description("R,DevAddr0ChRegAddr80hEepi0")]
        public string DevAddr0ChRegAddr80hEepi0;
        [Description("R,DevAddr0ChRegAddr81hEepi1")]
        public string DevAddr0ChRegAddr81hEepi1;
        [Description("R,DevAddr0ChRegAddr82hEepi2")]
        public string DevAddr0ChRegAddr82hEepi2;
        [Description("R,DevAddr0ChRegAddr83hEepi3")]
        public string DevAddr0ChRegAddr83hEepi3;
        [Description("R,DevAddr0ChRegAddr84hEepi4")]
        public string DevAddr0ChRegAddr84hEepi4;
        [Description("R,DevAddr0ChRegAddr85hEepi5")]
        public string DevAddr0ChRegAddr85hEepi5;
        [Description("R,DevAddr0ChRegAddr86hEepi6")]
        public string DevAddr0ChRegAddr86hEepi6;
        [Description("R,DevAddr0ChRegAddr87hEepi7")]
        public string DevAddr0ChRegAddr87hEepi7;
        [Description("R,DevAddr0ChRegAddr88hEepi8")]
        public string DevAddr0ChRegAddr88hEepi8;
        [Description("R,DevAddr0ChRegAddr89hEepi9")]
        public string DevAddr0ChRegAddr89hEepi9;
        [Description("R,DevAddr0ChRegAddr8AhEepi10")]
        public string DevAddr0ChRegAddr8AhEepi10;
        [Description("R,DevAddr0ChRegAddr8BhEepi11")]
        public string DevAddr0ChRegAddr8BhEepi11;
        [Description("R,DevAddr0ChRegAddrA0hEepp0")]
        public string DevAddr0ChRegAddrA0hEepp0;
        [Description("R,DevAddr0ChRegAddrA1hEepp1")]
        public string DevAddr0ChRegAddrA1hEepp1;
        [Description("R,DevAddr0ChRegAddrA2hEepp2")]
        public string DevAddr0ChRegAddrA2hEepp2;
        [Description("R,DevAddr0ChRegAddrA3hEepp3")]
        public string DevAddr0ChRegAddrA3hEepp3;
        [Description("R,DevAddr0ChRegAddrA4hEepp4")]
        public string DevAddr0ChRegAddrA4hEepp4;
        [Description("R,DevAddr0ChRegAddrA5hEepp5")]
        public string DevAddr0ChRegAddrA5hEepp5;
        [Description("R,DevAddr0ChRegAddrA6hEepp6")]
        public string DevAddr0ChRegAddrA6hEepp6;
        [Description("R,DevAddr0ChRegAddrA7hEepp7")]
        public string DevAddr0ChRegAddrA7hEepp7;
        [Description("R,DevAddr0ChRegAddrA8hEepp8")]
        public string DevAddr0ChRegAddrA8hEepp8;
        [Description("R,DevAddr0ChRegAddrA9hEepp9")]
        public string DevAddr0ChRegAddrA9hEepp9;
        [Description("R,DevAddr0ChRegAddrAAhEepp10")]
        public string DevAddr0ChRegAddrAAhEepp10;
        [Description("R,DevAddr0ChRegAddrABhEepp11")]
        public string DevAddr0ChRegAddrABhEepp11;
        [Description("R,DevAddr0ChRegAddrC0hEepm0")]
        public string DevAddr0ChRegAddrC0hEepm0;
        [Description("R,DevAddr0ChRegAddrC1hEepm1")]
        public string DevAddr0ChRegAddrC1hEepm1;
        [Description("R,DevAddr0ChRegAddrC2hEepm2")]
        public string DevAddr0ChRegAddrC2hEepm2;
        [Description("R,DevAddr0ChRegAddrC3hEepm3")]
        public string DevAddr0ChRegAddrC3hEepm3;
        [Description("R,DevAddr0ChRegAddrC4hEepm4")]
        public string DevAddr0ChRegAddrC4hEepm4;
        [Description("R,DevAddr0ChRegAddrC5hEepm5")]
        public string DevAddr0ChRegAddrC5hEepm5;
        [Description("R,DevAddr0ChRegAddrC6hEepm6")]
        public string DevAddr0ChRegAddrC6hEepm6;
        [Description("R,DevAddr0ChRegAddrC7hEepm7")]
        public string DevAddr0ChRegAddrC7hEepm7;
        [Description("R,DevAddr0ChRegAddrC8hEepm8")]
        public string DevAddr0ChRegAddrC8hEepm8;
        [Description("R,DevAddr0ChRegAddrC9hEepm9")]
        public string DevAddr0ChRegAddrC9hEepm9;
        [Description("R,DevAddr0ChRegAddrCAhEepm10")]
        public string DevAddr0ChRegAddrCAhEepm10;
        [Description("R,DevAddr0ChRegAddrCBhEepm11")]
        public string DevAddr0ChRegAddrCBhEepm11;
        [Description("R,DevAddr0ChRegAddrCChEepm12")]
        public string DevAddr0ChRegAddrCChEepm12;
        [Description("R,DevAddr0ChRegAddrCDhEepm13")]
        public string DevAddr0ChRegAddrCDhEepm13;
        [Description("R,DevAddr0ChRegAddrCEhEepm14")]
        public string DevAddr0ChRegAddrCEhEepm14;
        [Description("R,DevAddr0ChRegAddrCFhEepm15")]
        public string DevAddr0ChRegAddrCFhEepm15;

        [Description("R,DevAddr0DhRegAddr00hIout0")]
        public string DevAddr0DhRegAddr00hIout0;
        [Description("R,DevAddr0DhRegAddr01hIout1")]
        public string DevAddr0DhRegAddr01hIout1;
        [Description("R,DevAddr0DhRegAddr02hIout2")]
        public string DevAddr0DhRegAddr02hIout2;
        [Description("R,DevAddr0DhRegAddr03hIout3")]
        public string DevAddr0DhRegAddr03hIout3;
        [Description("R,DevAddr0DhRegAddr04hIout4")]
        public string DevAddr0DhRegAddr04hIout4;
        [Description("R,DevAddr0DhRegAddr05hIout5")]
        public string DevAddr0DhRegAddr05hIout5;
        [Description("R,DevAddr0DhRegAddr06hIout6")]
        public string DevAddr0DhRegAddr06hIout6;
        [Description("R,DevAddr0DhRegAddr07hIout7")]
        public string DevAddr0DhRegAddr07hIout7;
        [Description("R,DevAddr0DhRegAddr08hIout8")]
        public string DevAddr0DhRegAddr08hIout8;
        [Description("R,DevAddr0DhRegAddr09hIout9")]
        public string DevAddr0DhRegAddr09hIout9;
        [Description("R,DevAddr0DhRegAddr0AhIout10")]
        public string DevAddr0DhRegAddr0AhIout10;
        [Description("R,DevAddr0DhRegAddr0BhIout11")]
        public string DevAddr0DhRegAddr0BhIout11;
        [Description("R,DevAddr0DhRegAddr20hIoutPwm0")]
        public string DevAddr0DhRegAddr20hIoutPwm0;
        [Description("R,DevAddr0DhRegAddr21hIoutPwm1")]
        public string DevAddr0DhRegAddr21hIoutPwm1;
        [Description("R,DevAddr0DhRegAddr22hIoutPwm2")]
        public string DevAddr0DhRegAddr22hIoutPwm2;
        [Description("R,DevAddr0DhRegAddr23hIoutPwm3")]
        public string DevAddr0DhRegAddr23hIoutPwm3;
        [Description("R,DevAddr0DhRegAddr24hIoutPwm4")]
        public string DevAddr0DhRegAddr24hIoutPwm4;
        [Description("R,DevAddr0DhRegAddr25hIoutPwm5")]
        public string DevAddr0DhRegAddr25hIoutPwm5;
        [Description("R,DevAddr0DhRegAddr26hIoutPwm6")]
        public string DevAddr0DhRegAddr26hIoutPwm6;
        [Description("R,DevAddr0DhRegAddr27hIoutPwm7")]
        public string DevAddr0DhRegAddr27hIoutPwm7;
        [Description("R,DevAddr0DhRegAddr28hIoutPwm8")]
        public string DevAddr0DhRegAddr28hIoutPwm8;
        [Description("R,DevAddr0DhRegAddr29hIoutPwm9")]
        public string DevAddr0DhRegAddr29hIoutPwm9;
        [Description("R,DevAddr0DhRegAddr2AhIoutPwm10")]
        public string DevAddr0DhRegAddr2AhIoutPwm10;
        [Description("R,DevAddr0DhRegAddr2BhIoutPwm11")]
        public string DevAddr0DhRegAddr2BhIoutPwm11;
        [Description("R,DevAddr0DhRegAddr40hIoutPwml0")]
        public string DevAddr0DhRegAddr40hIoutPwml0;
        [Description("R,DevAddr0DhRegAddr41hIoutPwml1")]
        public string DevAddr0DhRegAddr41hIoutPwml1;
        [Description("R,DevAddr0DhRegAddr42hIoutPwml2")]
        public string DevAddr0DhRegAddr42hIoutPwml2;
        [Description("R,DevAddr0DhRegAddr43hIoutPwml3")]
        public string DevAddr0DhRegAddr43hIoutPwml3;
        [Description("R,DevAddr0DhRegAddr44hIoutPwml4")]
        public string DevAddr0DhRegAddr44hIoutPwml4;
        [Description("R,DevAddr0DhRegAddr45hIoutPwml5")]
        public string DevAddr0DhRegAddr45hIoutPwml5;
        [Description("R,DevAddr0DhRegAddr46hIoutPwml6")]
        public string DevAddr0DhRegAddr46hIoutPwml6;
        [Description("R,DevAddr0DhRegAddr47hIoutPwml7")]
        public string DevAddr0DhRegAddr47hIoutPwml7;
        [Description("R,DevAddr0DhRegAddr48hIoutPwml8")]
        public string DevAddr0DhRegAddr48hIoutPwml8;
        [Description("R,DevAddr0DhRegAddr49hIoutPwml9")]
        public string DevAddr0DhRegAddr49hIoutPwml9;
        [Description("R,DevAddr0DhRegAddr4AhIoutPwml10")]
        public string DevAddr0DhRegAddr4AhIoutPwml10;
        [Description("R,DevAddr0DhRegAddr4BhIoutPwml11")]
        public string DevAddr0DhRegAddr4BhIoutPwml11;
        [Description("R,DevAddr0DhRegAddr50hConfEn0")]
        public string DevAddr0DhRegAddr50hConfEn0;
        [Description("R,DevAddr0DhRegAddr51hConfEn1")]
        public string DevAddr0DhRegAddr51hConfEn1;
        [Description("R,DevAddr0DhRegAddr54hConfDiagen0")]
        public string DevAddr0DhRegAddr54hConfDiagen0;
        [Description("R,DevAddr0DhRegAddr55hConfDiagen1")]
        public string DevAddr0DhRegAddr55hConfDiagen1;
        [Description("R,DevAddr0DhRegAddr56hConfMisc0")]
        public string DevAddr0DhRegAddr56hConfMisc0;
        [Description("R,DevAddr0DhRegAddr57hConfMisc1")]
        public string DevAddr0DhRegAddr57hConfMisc1;
        [Description("R,DevAddr0DhRegAddr58hConfMisc2")]
        public string DevAddr0DhRegAddr58hConfMisc2;
        [Description("R,DevAddr0DhRegAddr59hConfMisc3")]
        public string DevAddr0DhRegAddr59hConfMisc3;
        [Description("R,DevAddr0DhRegAddr5AhConfMisc4")]
        public string DevAddr0DhRegAddr5AhConfMisc4;
        [Description("R,DevAddr0DhRegAddr5BhConfMisc5")]
        public string DevAddr0DhRegAddr5BhConfMisc5;
        [Description("R,DevAddr0DhRegAddr60hClr")]
        public string DevAddr0DhRegAddr60hClr;
        [Description("R,DevAddr0DhRegAddr61hConfLock")]
        public string DevAddr0DhRegAddr61hConfLock;
        [Description("R,DevAddr0DhRegAddr62hConfMisc6")]
        public string DevAddr0DhRegAddr62hConfMisc6;
        [Description("R,DevAddr0DhRegAddr63hConfMisc7")]
        public string DevAddr0DhRegAddr63hConfMisc7;
        [Description("R,DevAddr0DhRegAddr64hConfMisc8")]
        public string DevAddr0DhRegAddr64hConfMisc8;
        [Description("R,DevAddr0DhRegAddr65hConfMisc9")]
        public string DevAddr0DhRegAddr65hConfMisc9;
        [Description("R,DevAddr0DhRegAddr70hFlag0")]
        public string DevAddr0DhRegAddr70hFlag0;
        [Description("R,DevAddr0DhRegAddr71hFlag1")]
        public string DevAddr0DhRegAddr71hFlag1;
        [Description("R,DevAddr0DhRegAddr72hFlag2")]
        public string DevAddr0DhRegAddr72hFlag2;
        [Description("R,DevAddr0DhRegAddr73hFlag3")]
        public string DevAddr0DhRegAddr73hFlag3;
        [Description("R,DevAddr0DhRegAddr74hFlag4")]
        public string DevAddr0DhRegAddr74hFlag4;
        [Description("R,DevAddr0DhRegAddr75hFlag5")]
        public string DevAddr0DhRegAddr75hFlag5;
        [Description("R,DevAddr0DhRegAddr77hFlag7")]
        public string DevAddr0DhRegAddr77hFlag7;
        [Description("R,DevAddr0DhRegAddr78hFlag8")]
        public string DevAddr0DhRegAddr78hFlag8;
        [Description("R,DevAddr0DhRegAddr7BhFlag11")]
        public string DevAddr0DhRegAddr7BhFlag11;
        [Description("R,DevAddr0DhRegAddr7ChFlag12")]
        public string DevAddr0DhRegAddr7ChFlag12;
        [Description("R,DevAddr0DhRegAddr7DhFlag13")]
        public string DevAddr0DhRegAddr7DhFlag13;
        [Description("R,DevAddr0DhRegAddr7EhFlag14")]
        public string DevAddr0DhRegAddr7EhFlag14;
        [Description("R,DevAddr0DhRegAddr80hEepi0")]
        public string DevAddr0DhRegAddr80hEepi0;
        [Description("R,DevAddr0DhRegAddr81hEepi1")]
        public string DevAddr0DhRegAddr81hEepi1;
        [Description("R,DevAddr0DhRegAddr82hEepi2")]
        public string DevAddr0DhRegAddr82hEepi2;
        [Description("R,DevAddr0DhRegAddr83hEepi3")]
        public string DevAddr0DhRegAddr83hEepi3;
        [Description("R,DevAddr0DhRegAddr84hEepi4")]
        public string DevAddr0DhRegAddr84hEepi4;
        [Description("R,DevAddr0DhRegAddr85hEepi5")]
        public string DevAddr0DhRegAddr85hEepi5;
        [Description("R,DevAddr0DhRegAddr86hEepi6")]
        public string DevAddr0DhRegAddr86hEepi6;
        [Description("R,DevAddr0DhRegAddr87hEepi7")]
        public string DevAddr0DhRegAddr87hEepi7;
        [Description("R,DevAddr0DhRegAddr88hEepi8")]
        public string DevAddr0DhRegAddr88hEepi8;
        [Description("R,DevAddr0DhRegAddr89hEepi9")]
        public string DevAddr0DhRegAddr89hEepi9;
        [Description("R,DevAddr0DhRegAddr8AhEepi10")]
        public string DevAddr0DhRegAddr8AhEepi10;
        [Description("R,DevAddr0DhRegAddr8BhEepi11")]
        public string DevAddr0DhRegAddr8BhEepi11;
        [Description("R,DevAddr0DhRegAddrA0hEepp0")]
        public string DevAddr0DhRegAddrA0hEepp0;
        [Description("R,DevAddr0DhRegAddrA1hEepp1")]
        public string DevAddr0DhRegAddrA1hEepp1;
        [Description("R,DevAddr0DhRegAddrA2hEepp2")]
        public string DevAddr0DhRegAddrA2hEepp2;
        [Description("R,DevAddr0DhRegAddrA3hEepp3")]
        public string DevAddr0DhRegAddrA3hEepp3;
        [Description("R,DevAddr0DhRegAddrA4hEepp4")]
        public string DevAddr0DhRegAddrA4hEepp4;
        [Description("R,DevAddr0DhRegAddrA5hEepp5")]
        public string DevAddr0DhRegAddrA5hEepp5;
        [Description("R,DevAddr0DhRegAddrA6hEepp6")]
        public string DevAddr0DhRegAddrA6hEepp6;
        [Description("R,DevAddr0DhRegAddrA7hEepp7")]
        public string DevAddr0DhRegAddrA7hEepp7;
        [Description("R,DevAddr0DhRegAddrA8hEepp8")]
        public string DevAddr0DhRegAddrA8hEepp8;
        [Description("R,DevAddr0DhRegAddrA9hEepp9")]
        public string DevAddr0DhRegAddrA9hEepp9;
        [Description("R,DevAddr0DhRegAddrAAhEepp10")]
        public string DevAddr0DhRegAddrAAhEepp10;
        [Description("R,DevAddr0DhRegAddrABhEepp11")]
        public string DevAddr0DhRegAddrABhEepp11;
        [Description("R,DevAddr0DhRegAddrC0hEepm0")]
        public string DevAddr0DhRegAddrC0hEepm0;
        [Description("R,DevAddr0DhRegAddrC1hEepm1")]
        public string DevAddr0DhRegAddrC1hEepm1;
        [Description("R,DevAddr0DhRegAddrC2hEepm2")]
        public string DevAddr0DhRegAddrC2hEepm2;
        [Description("R,DevAddr0DhRegAddrC3hEepm3")]
        public string DevAddr0DhRegAddrC3hEepm3;
        [Description("R,DevAddr0DhRegAddrC4hEepm4")]
        public string DevAddr0DhRegAddrC4hEepm4;
        [Description("R,DevAddr0DhRegAddrC5hEepm5")]
        public string DevAddr0DhRegAddrC5hEepm5;
        [Description("R,DevAddr0DhRegAddrC6hEepm6")]
        public string DevAddr0DhRegAddrC6hEepm6;
        [Description("R,DevAddr0DhRegAddrC7hEepm7")]
        public string DevAddr0DhRegAddrC7hEepm7;
        [Description("R,DevAddr0DhRegAddrC8hEepm8")]
        public string DevAddr0DhRegAddrC8hEepm8;
        [Description("R,DevAddr0DhRegAddrC9hEepm9")]
        public string DevAddr0DhRegAddrC9hEepm9;
        [Description("R,DevAddr0DhRegAddrCAhEepm10")]
        public string DevAddr0DhRegAddrCAhEepm10;
        [Description("R,DevAddr0DhRegAddrCBhEepm11")]
        public string DevAddr0DhRegAddrCBhEepm11;
        [Description("R,DevAddr0DhRegAddrCChEepm12")]
        public string DevAddr0DhRegAddrCChEepm12;
        [Description("R,DevAddr0DhRegAddrCDhEepm13")]
        public string DevAddr0DhRegAddrCDhEepm13;
        [Description("R,DevAddr0DhRegAddrCEhEepm14")]
        public string DevAddr0DhRegAddrCEhEepm14;
        [Description("R,DevAddr0DhRegAddrCFhEepm15")]
        public string DevAddr0DhRegAddrCFhEepm15;

        [Description("R,DevAddr0EhRegAddr00hIout0")]
        public string DevAddr0EhRegAddr00hIout0;
        [Description("R,DevAddr0EhRegAddr01hIout1")]
        public string DevAddr0EhRegAddr01hIout1;
        [Description("R,DevAddr0EhRegAddr02hIout2")]
        public string DevAddr0EhRegAddr02hIout2;
        [Description("R,DevAddr0EhRegAddr03hIout3")]
        public string DevAddr0EhRegAddr03hIout3;
        [Description("R,DevAddr0EhRegAddr04hIout4")]
        public string DevAddr0EhRegAddr04hIout4;
        [Description("R,DevAddr0EhRegAddr05hIout5")]
        public string DevAddr0EhRegAddr05hIout5;
        [Description("R,DevAddr0EhRegAddr06hIout6")]
        public string DevAddr0EhRegAddr06hIout6;
        [Description("R,DevAddr0EhRegAddr07hIout7")]
        public string DevAddr0EhRegAddr07hIout7;
        [Description("R,DevAddr0EhRegAddr08hIout8")]
        public string DevAddr0EhRegAddr08hIout8;
        [Description("R,DevAddr0EhRegAddr09hIout9")]
        public string DevAddr0EhRegAddr09hIout9;
        [Description("R,DevAddr0EhRegAddr0AhIout10")]
        public string DevAddr0EhRegAddr0AhIout10;
        [Description("R,DevAddr0EhRegAddr0BhIout11")]
        public string DevAddr0EhRegAddr0BhIout11;
        [Description("R,DevAddr0EhRegAddr20hIoutPwm0")]
        public string DevAddr0EhRegAddr20hIoutPwm0;
        [Description("R,DevAddr0EhRegAddr21hIoutPwm1")]
        public string DevAddr0EhRegAddr21hIoutPwm1;
        [Description("R,DevAddr0EhRegAddr22hIoutPwm2")]
        public string DevAddr0EhRegAddr22hIoutPwm2;
        [Description("R,DevAddr0EhRegAddr23hIoutPwm3")]
        public string DevAddr0EhRegAddr23hIoutPwm3;
        [Description("R,DevAddr0EhRegAddr24hIoutPwm4")]
        public string DevAddr0EhRegAddr24hIoutPwm4;
        [Description("R,DevAddr0EhRegAddr25hIoutPwm5")]
        public string DevAddr0EhRegAddr25hIoutPwm5;
        [Description("R,DevAddr0EhRegAddr26hIoutPwm6")]
        public string DevAddr0EhRegAddr26hIoutPwm6;
        [Description("R,DevAddr0EhRegAddr27hIoutPwm7")]
        public string DevAddr0EhRegAddr27hIoutPwm7;
        [Description("R,DevAddr0EhRegAddr28hIoutPwm8")]
        public string DevAddr0EhRegAddr28hIoutPwm8;
        [Description("R,DevAddr0EhRegAddr29hIoutPwm9")]
        public string DevAddr0EhRegAddr29hIoutPwm9;
        [Description("R,DevAddr0EhRegAddr2AhIoutPwm10")]
        public string DevAddr0EhRegAddr2AhIoutPwm10;
        [Description("R,DevAddr0EhRegAddr2BhIoutPwm11")]
        public string DevAddr0EhRegAddr2BhIoutPwm11;
        [Description("R,DevAddr0EhRegAddr40hIoutPwml0")]
        public string DevAddr0EhRegAddr40hIoutPwml0;
        [Description("R,DevAddr0EhRegAddr41hIoutPwml1")]
        public string DevAddr0EhRegAddr41hIoutPwml1;
        [Description("R,DevAddr0EhRegAddr42hIoutPwml2")]
        public string DevAddr0EhRegAddr42hIoutPwml2;
        [Description("R,DevAddr0EhRegAddr43hIoutPwml3")]
        public string DevAddr0EhRegAddr43hIoutPwml3;
        [Description("R,DevAddr0EhRegAddr44hIoutPwml4")]
        public string DevAddr0EhRegAddr44hIoutPwml4;
        [Description("R,DevAddr0EhRegAddr45hIoutPwml5")]
        public string DevAddr0EhRegAddr45hIoutPwml5;
        [Description("R,DevAddr0EhRegAddr46hIoutPwml6")]
        public string DevAddr0EhRegAddr46hIoutPwml6;
        [Description("R,DevAddr0EhRegAddr47hIoutPwml7")]
        public string DevAddr0EhRegAddr47hIoutPwml7;
        [Description("R,DevAddr0EhRegAddr48hIoutPwml8")]
        public string DevAddr0EhRegAddr48hIoutPwml8;
        [Description("R,DevAddr0EhRegAddr49hIoutPwml9")]
        public string DevAddr0EhRegAddr49hIoutPwml9;
        [Description("R,DevAddr0EhRegAddr4AhIoutPwml10")]
        public string DevAddr0EhRegAddr4AhIoutPwml10;
        [Description("R,DevAddr0EhRegAddr4BhIoutPwml11")]
        public string DevAddr0EhRegAddr4BhIoutPwml11;
        [Description("R,DevAddr0EhRegAddr50hConfEn0")]
        public string DevAddr0EhRegAddr50hConfEn0;
        [Description("R,DevAddr0EhRegAddr51hConfEn1")]
        public string DevAddr0EhRegAddr51hConfEn1;
        [Description("R,DevAddr0EhRegAddr54hConfDiagen0")]
        public string DevAddr0EhRegAddr54hConfDiagen0;
        [Description("R,DevAddr0EhRegAddr55hConfDiagen1")]
        public string DevAddr0EhRegAddr55hConfDiagen1;
        [Description("R,DevAddr0EhRegAddr56hConfMisc0")]
        public string DevAddr0EhRegAddr56hConfMisc0;
        [Description("R,DevAddr0EhRegAddr57hConfMisc1")]
        public string DevAddr0EhRegAddr57hConfMisc1;
        [Description("R,DevAddr0EhRegAddr58hConfMisc2")]
        public string DevAddr0EhRegAddr58hConfMisc2;
        [Description("R,DevAddr0EhRegAddr59hConfMisc3")]
        public string DevAddr0EhRegAddr59hConfMisc3;
        [Description("R,DevAddr0EhRegAddr5AhConfMisc4")]
        public string DevAddr0EhRegAddr5AhConfMisc4;
        [Description("R,DevAddr0EhRegAddr5BhConfMisc5")]
        public string DevAddr0EhRegAddr5BhConfMisc5;
        [Description("R,DevAddr0EhRegAddr60hClr")]
        public string DevAddr0EhRegAddr60hClr;
        [Description("R,DevAddr0EhRegAddr61hConfLock")]
        public string DevAddr0EhRegAddr61hConfLock;
        [Description("R,DevAddr0EhRegAddr62hConfMisc6")]
        public string DevAddr0EhRegAddr62hConfMisc6;
        [Description("R,DevAddr0EhRegAddr63hConfMisc7")]
        public string DevAddr0EhRegAddr63hConfMisc7;
        [Description("R,DevAddr0EhRegAddr64hConfMisc8")]
        public string DevAddr0EhRegAddr64hConfMisc8;
        [Description("R,DevAddr0EhRegAddr65hConfMisc9")]
        public string DevAddr0EhRegAddr65hConfMisc9;
        [Description("R,DevAddr0EhRegAddr70hFlag0")]
        public string DevAddr0EhRegAddr70hFlag0;
        [Description("R,DevAddr0EhRegAddr71hFlag1")]
        public string DevAddr0EhRegAddr71hFlag1;
        [Description("R,DevAddr0EhRegAddr72hFlag2")]
        public string DevAddr0EhRegAddr72hFlag2;
        [Description("R,DevAddr0EhRegAddr73hFlag3")]
        public string DevAddr0EhRegAddr73hFlag3;
        [Description("R,DevAddr0EhRegAddr74hFlag4")]
        public string DevAddr0EhRegAddr74hFlag4;
        [Description("R,DevAddr0EhRegAddr75hFlag5")]
        public string DevAddr0EhRegAddr75hFlag5;
        [Description("R,DevAddr0EhRegAddr77hFlag7")]
        public string DevAddr0EhRegAddr77hFlag7;
        [Description("R,DevAddr0EhRegAddr78hFlag8")]
        public string DevAddr0EhRegAddr78hFlag8;
        [Description("R,DevAddr0EhRegAddr7BhFlag11")]
        public string DevAddr0EhRegAddr7BhFlag11;
        [Description("R,DevAddr0EhRegAddr7ChFlag12")]
        public string DevAddr0EhRegAddr7ChFlag12;
        [Description("R,DevAddr0EhRegAddr7DhFlag13")]
        public string DevAddr0EhRegAddr7DhFlag13;
        [Description("R,DevAddr0EhRegAddr7EhFlag14")]
        public string DevAddr0EhRegAddr7EhFlag14;
        [Description("R,DevAddr0EhRegAddr80hEepi0")]
        public string DevAddr0EhRegAddr80hEepi0;
        [Description("R,DevAddr0EhRegAddr81hEepi1")]
        public string DevAddr0EhRegAddr81hEepi1;
        [Description("R,DevAddr0EhRegAddr82hEepi2")]
        public string DevAddr0EhRegAddr82hEepi2;
        [Description("R,DevAddr0EhRegAddr83hEepi3")]
        public string DevAddr0EhRegAddr83hEepi3;
        [Description("R,DevAddr0EhRegAddr84hEepi4")]
        public string DevAddr0EhRegAddr84hEepi4;
        [Description("R,DevAddr0EhRegAddr85hEepi5")]
        public string DevAddr0EhRegAddr85hEepi5;
        [Description("R,DevAddr0EhRegAddr86hEepi6")]
        public string DevAddr0EhRegAddr86hEepi6;
        [Description("R,DevAddr0EhRegAddr87hEepi7")]
        public string DevAddr0EhRegAddr87hEepi7;
        [Description("R,DevAddr0EhRegAddr88hEepi8")]
        public string DevAddr0EhRegAddr88hEepi8;
        [Description("R,DevAddr0EhRegAddr89hEepi9")]
        public string DevAddr0EhRegAddr89hEepi9;
        [Description("R,DevAddr0EhRegAddr8AhEepi10")]
        public string DevAddr0EhRegAddr8AhEepi10;
        [Description("R,DevAddr0EhRegAddr8BhEepi11")]
        public string DevAddr0EhRegAddr8BhEepi11;
        [Description("R,DevAddr0EhRegAddrA0hEepp0")]
        public string DevAddr0EhRegAddrA0hEepp0;
        [Description("R,DevAddr0EhRegAddrA1hEepp1")]
        public string DevAddr0EhRegAddrA1hEepp1;
        [Description("R,DevAddr0EhRegAddrA2hEepp2")]
        public string DevAddr0EhRegAddrA2hEepp2;
        [Description("R,DevAddr0EhRegAddrA3hEepp3")]
        public string DevAddr0EhRegAddrA3hEepp3;
        [Description("R,DevAddr0EhRegAddrA4hEepp4")]
        public string DevAddr0EhRegAddrA4hEepp4;
        [Description("R,DevAddr0EhRegAddrA5hEepp5")]
        public string DevAddr0EhRegAddrA5hEepp5;
        [Description("R,DevAddr0EhRegAddrA6hEepp6")]
        public string DevAddr0EhRegAddrA6hEepp6;
        [Description("R,DevAddr0EhRegAddrA7hEepp7")]
        public string DevAddr0EhRegAddrA7hEepp7;
        [Description("R,DevAddr0EhRegAddrA8hEepp8")]
        public string DevAddr0EhRegAddrA8hEepp8;
        [Description("R,DevAddr0EhRegAddrA9hEepp9")]
        public string DevAddr0EhRegAddrA9hEepp9;
        [Description("R,DevAddr0EhRegAddrAAhEepp10")]
        public string DevAddr0EhRegAddrAAhEepp10;
        [Description("R,DevAddr0EhRegAddrABhEepp11")]
        public string DevAddr0EhRegAddrABhEepp11;
        [Description("R,DevAddr0EhRegAddrC0hEepm0")]
        public string DevAddr0EhRegAddrC0hEepm0;
        [Description("R,DevAddr0EhRegAddrC1hEepm1")]
        public string DevAddr0EhRegAddrC1hEepm1;
        [Description("R,DevAddr0EhRegAddrC2hEepm2")]
        public string DevAddr0EhRegAddrC2hEepm2;
        [Description("R,DevAddr0EhRegAddrC3hEepm3")]
        public string DevAddr0EhRegAddrC3hEepm3;
        [Description("R,DevAddr0EhRegAddrC4hEepm4")]
        public string DevAddr0EhRegAddrC4hEepm4;
        [Description("R,DevAddr0EhRegAddrC5hEepm5")]
        public string DevAddr0EhRegAddrC5hEepm5;
        [Description("R,DevAddr0EhRegAddrC6hEepm6")]
        public string DevAddr0EhRegAddrC6hEepm6;
        [Description("R,DevAddr0EhRegAddrC7hEepm7")]
        public string DevAddr0EhRegAddrC7hEepm7;
        [Description("R,DevAddr0EhRegAddrC8hEepm8")]
        public string DevAddr0EhRegAddrC8hEepm8;
        [Description("R,DevAddr0EhRegAddrC9hEepm9")]
        public string DevAddr0EhRegAddrC9hEepm9;
        [Description("R,DevAddr0EhRegAddrCAhEepm10")]
        public string DevAddr0EhRegAddrCAhEepm10;
        [Description("R,DevAddr0EhRegAddrCBhEepm11")]
        public string DevAddr0EhRegAddrCBhEepm11;
        [Description("R,DevAddr0EhRegAddrCChEepm12")]
        public string DevAddr0EhRegAddrCChEepm12;
        [Description("R,DevAddr0EhRegAddrCDhEepm13")]
        public string DevAddr0EhRegAddrCDhEepm13;
        [Description("R,DevAddr0EhRegAddrCEhEepm14")]
        public string DevAddr0EhRegAddrCEhEepm14;
        [Description("R,DevAddr0EhRegAddrCFhEepm15")]
        public string DevAddr0EhRegAddrCFhEepm15;

        [Description("R,DevAddr0FhRegAddr00hIout0")]
        public string DevAddr0FhRegAddr00hIout0;
        [Description("R,DevAddr0FhRegAddr01hIout1")]
        public string DevAddr0FhRegAddr01hIout1;
        [Description("R,DevAddr0FhRegAddr02hIout2")]
        public string DevAddr0FhRegAddr02hIout2;
        [Description("R,DevAddr0FhRegAddr03hIout3")]
        public string DevAddr0FhRegAddr03hIout3;
        [Description("R,DevAddr0FhRegAddr04hIout4")]
        public string DevAddr0FhRegAddr04hIout4;
        [Description("R,DevAddr0FhRegAddr05hIout5")]
        public string DevAddr0FhRegAddr05hIout5;
        [Description("R,DevAddr0FhRegAddr06hIout6")]
        public string DevAddr0FhRegAddr06hIout6;
        [Description("R,DevAddr0FhRegAddr07hIout7")]
        public string DevAddr0FhRegAddr07hIout7;
        [Description("R,DevAddr0FhRegAddr08hIout8")]
        public string DevAddr0FhRegAddr08hIout8;
        [Description("R,DevAddr0FhRegAddr09hIout9")]
        public string DevAddr0FhRegAddr09hIout9;
        [Description("R,DevAddr0FhRegAddr0AhIout10")]
        public string DevAddr0FhRegAddr0AhIout10;
        [Description("R,DevAddr0FhRegAddr0BhIout11")]
        public string DevAddr0FhRegAddr0BhIout11;
        [Description("R,DevAddr0FhRegAddr20hIoutPwm0")]
        public string DevAddr0FhRegAddr20hIoutPwm0;
        [Description("R,DevAddr0FhRegAddr21hIoutPwm1")]
        public string DevAddr0FhRegAddr21hIoutPwm1;
        [Description("R,DevAddr0FhRegAddr22hIoutPwm2")]
        public string DevAddr0FhRegAddr22hIoutPwm2;
        [Description("R,DevAddr0FhRegAddr23hIoutPwm3")]
        public string DevAddr0FhRegAddr23hIoutPwm3;
        [Description("R,DevAddr0FhRegAddr24hIoutPwm4")]
        public string DevAddr0FhRegAddr24hIoutPwm4;
        [Description("R,DevAddr0FhRegAddr25hIoutPwm5")]
        public string DevAddr0FhRegAddr25hIoutPwm5;
        [Description("R,DevAddr0FhRegAddr26hIoutPwm6")]
        public string DevAddr0FhRegAddr26hIoutPwm6;
        [Description("R,DevAddr0FhRegAddr27hIoutPwm7")]
        public string DevAddr0FhRegAddr27hIoutPwm7;
        [Description("R,DevAddr0FhRegAddr28hIoutPwm8")]
        public string DevAddr0FhRegAddr28hIoutPwm8;
        [Description("R,DevAddr0FhRegAddr29hIoutPwm9")]
        public string DevAddr0FhRegAddr29hIoutPwm9;
        [Description("R,DevAddr0FhRegAddr2AhIoutPwm10")]
        public string DevAddr0FhRegAddr2AhIoutPwm10;
        [Description("R,DevAddr0FhRegAddr2BhIoutPwm11")]
        public string DevAddr0FhRegAddr2BhIoutPwm11;
        [Description("R,DevAddr0FhRegAddr40hIoutPwml0")]
        public string DevAddr0FhRegAddr40hIoutPwml0;
        [Description("R,DevAddr0FhRegAddr41hIoutPwml1")]
        public string DevAddr0FhRegAddr41hIoutPwml1;
        [Description("R,DevAddr0FhRegAddr42hIoutPwml2")]
        public string DevAddr0FhRegAddr42hIoutPwml2;
        [Description("R,DevAddr0FhRegAddr43hIoutPwml3")]
        public string DevAddr0FhRegAddr43hIoutPwml3;
        [Description("R,DevAddr0FhRegAddr44hIoutPwml4")]
        public string DevAddr0FhRegAddr44hIoutPwml4;
        [Description("R,DevAddr0FhRegAddr45hIoutPwml5")]
        public string DevAddr0FhRegAddr45hIoutPwml5;
        [Description("R,DevAddr0FhRegAddr46hIoutPwml6")]
        public string DevAddr0FhRegAddr46hIoutPwml6;
        [Description("R,DevAddr0FhRegAddr47hIoutPwml7")]
        public string DevAddr0FhRegAddr47hIoutPwml7;
        [Description("R,DevAddr0FhRegAddr48hIoutPwml8")]
        public string DevAddr0FhRegAddr48hIoutPwml8;
        [Description("R,DevAddr0FhRegAddr49hIoutPwml9")]
        public string DevAddr0FhRegAddr49hIoutPwml9;
        [Description("R,DevAddr0FhRegAddr4AhIoutPwml10")]
        public string DevAddr0FhRegAddr4AhIoutPwml10;
        [Description("R,DevAddr0FhRegAddr4BhIoutPwml11")]
        public string DevAddr0FhRegAddr4BhIoutPwml11;
        [Description("R,DevAddr0FhRegAddr50hConfEn0")]
        public string DevAddr0FhRegAddr50hConfEn0;
        [Description("R,DevAddr0FhRegAddr51hConfEn1")]
        public string DevAddr0FhRegAddr51hConfEn1;
        [Description("R,DevAddr0FhRegAddr54hConfDiagen0")]
        public string DevAddr0FhRegAddr54hConfDiagen0;
        [Description("R,DevAddr0FhRegAddr55hConfDiagen1")]
        public string DevAddr0FhRegAddr55hConfDiagen1;
        [Description("R,DevAddr0FhRegAddr56hConfMisc0")]
        public string DevAddr0FhRegAddr56hConfMisc0;
        [Description("R,DevAddr0FhRegAddr57hConfMisc1")]
        public string DevAddr0FhRegAddr57hConfMisc1;
        [Description("R,DevAddr0FhRegAddr58hConfMisc2")]
        public string DevAddr0FhRegAddr58hConfMisc2;
        [Description("R,DevAddr0FhRegAddr59hConfMisc3")]
        public string DevAddr0FhRegAddr59hConfMisc3;
        [Description("R,DevAddr0FhRegAddr5AhConfMisc4")]
        public string DevAddr0FhRegAddr5AhConfMisc4;
        [Description("R,DevAddr0FhRegAddr5BhConfMisc5")]
        public string DevAddr0FhRegAddr5BhConfMisc5;
        [Description("R,DevAddr0FhRegAddr60hClr")]
        public string DevAddr0FhRegAddr60hClr;
        [Description("R,DevAddr0FhRegAddr61hConfLock")]
        public string DevAddr0FhRegAddr61hConfLock;
        [Description("R,DevAddr0FhRegAddr62hConfMisc6")]
        public string DevAddr0FhRegAddr62hConfMisc6;
        [Description("R,DevAddr0FhRegAddr63hConfMisc7")]
        public string DevAddr0FhRegAddr63hConfMisc7;
        [Description("R,DevAddr0FhRegAddr64hConfMisc8")]
        public string DevAddr0FhRegAddr64hConfMisc8;
        [Description("R,DevAddr0FhRegAddr65hConfMisc9")]
        public string DevAddr0FhRegAddr65hConfMisc9;
        [Description("R,DevAddr0FhRegAddr70hFlag0")]
        public string DevAddr0FhRegAddr70hFlag0;
        [Description("R,DevAddr0FhRegAddr71hFlag1")]
        public string DevAddr0FhRegAddr71hFlag1;
        [Description("R,DevAddr0FhRegAddr72hFlag2")]
        public string DevAddr0FhRegAddr72hFlag2;
        [Description("R,DevAddr0FhRegAddr73hFlag3")]
        public string DevAddr0FhRegAddr73hFlag3;
        [Description("R,DevAddr0FhRegAddr74hFlag4")]
        public string DevAddr0FhRegAddr74hFlag4;
        [Description("R,DevAddr0FhRegAddr75hFlag5")]
        public string DevAddr0FhRegAddr75hFlag5;
        [Description("R,DevAddr0FhRegAddr77hFlag7")]
        public string DevAddr0FhRegAddr77hFlag7;
        [Description("R,DevAddr0FhRegAddr78hFlag8")]
        public string DevAddr0FhRegAddr78hFlag8;
        [Description("R,DevAddr0FhRegAddr7BhFlag11")]
        public string DevAddr0FhRegAddr7BhFlag11;
        [Description("R,DevAddr0FhRegAddr7ChFlag12")]
        public string DevAddr0FhRegAddr7ChFlag12;
        [Description("R,DevAddr0FhRegAddr7DhFlag13")]
        public string DevAddr0FhRegAddr7DhFlag13;
        [Description("R,DevAddr0FhRegAddr7EhFlag14")]
        public string DevAddr0FhRegAddr7EhFlag14;
        [Description("R,DevAddr0FhRegAddr80hEepi0")]
        public string DevAddr0FhRegAddr80hEepi0;
        [Description("R,DevAddr0FhRegAddr81hEepi1")]
        public string DevAddr0FhRegAddr81hEepi1;
        [Description("R,DevAddr0FhRegAddr82hEepi2")]
        public string DevAddr0FhRegAddr82hEepi2;
        [Description("R,DevAddr0FhRegAddr83hEepi3")]
        public string DevAddr0FhRegAddr83hEepi3;
        [Description("R,DevAddr0FhRegAddr84hEepi4")]
        public string DevAddr0FhRegAddr84hEepi4;
        [Description("R,DevAddr0FhRegAddr85hEepi5")]
        public string DevAddr0FhRegAddr85hEepi5;
        [Description("R,DevAddr0FhRegAddr86hEepi6")]
        public string DevAddr0FhRegAddr86hEepi6;
        [Description("R,DevAddr0FhRegAddr87hEepi7")]
        public string DevAddr0FhRegAddr87hEepi7;
        [Description("R,DevAddr0FhRegAddr88hEepi8")]
        public string DevAddr0FhRegAddr88hEepi8;
        [Description("R,DevAddr0FhRegAddr89hEepi9")]
        public string DevAddr0FhRegAddr89hEepi9;
        [Description("R,DevAddr0FhRegAddr8AhEepi10")]
        public string DevAddr0FhRegAddr8AhEepi10;
        [Description("R,DevAddr0FhRegAddr8BhEepi11")]
        public string DevAddr0FhRegAddr8BhEepi11;
        [Description("R,DevAddr0FhRegAddrA0hEepp0")]
        public string DevAddr0FhRegAddrA0hEepp0;
        [Description("R,DevAddr0FhRegAddrA1hEepp1")]
        public string DevAddr0FhRegAddrA1hEepp1;
        [Description("R,DevAddr0FhRegAddrA2hEepp2")]
        public string DevAddr0FhRegAddrA2hEepp2;
        [Description("R,DevAddr0FhRegAddrA3hEepp3")]
        public string DevAddr0FhRegAddrA3hEepp3;
        [Description("R,DevAddr0FhRegAddrA4hEepp4")]
        public string DevAddr0FhRegAddrA4hEepp4;
        [Description("R,DevAddr0FhRegAddrA5hEepp5")]
        public string DevAddr0FhRegAddrA5hEepp5;
        [Description("R,DevAddr0FhRegAddrA6hEepp6")]
        public string DevAddr0FhRegAddrA6hEepp6;
        [Description("R,DevAddr0FhRegAddrA7hEepp7")]
        public string DevAddr0FhRegAddrA7hEepp7;
        [Description("R,DevAddr0FhRegAddrA8hEepp8")]
        public string DevAddr0FhRegAddrA8hEepp8;
        [Description("R,DevAddr0FhRegAddrA9hEepp9")]
        public string DevAddr0FhRegAddrA9hEepp9;
        [Description("R,DevAddr0FhRegAddrAAhEepp10")]
        public string DevAddr0FhRegAddrAAhEepp10;
        [Description("R,DevAddr0FhRegAddrABhEepp11")]
        public string DevAddr0FhRegAddrABhEepp11;
        [Description("R,DevAddr0FhRegAddrC0hEepm0")]
        public string DevAddr0FhRegAddrC0hEepm0;
        [Description("R,DevAddr0FhRegAddrC1hEepm1")]
        public string DevAddr0FhRegAddrC1hEepm1;
        [Description("R,DevAddr0FhRegAddrC2hEepm2")]
        public string DevAddr0FhRegAddrC2hEepm2;
        [Description("R,DevAddr0FhRegAddrC3hEepm3")]
        public string DevAddr0FhRegAddrC3hEepm3;
        [Description("R,DevAddr0FhRegAddrC4hEepm4")]
        public string DevAddr0FhRegAddrC4hEepm4;
        [Description("R,DevAddr0FhRegAddrC5hEepm5")]
        public string DevAddr0FhRegAddrC5hEepm5;
        [Description("R,DevAddr0FhRegAddrC6hEepm6")]
        public string DevAddr0FhRegAddrC6hEepm6;
        [Description("R,DevAddr0FhRegAddrC7hEepm7")]
        public string DevAddr0FhRegAddrC7hEepm7;
        [Description("R,DevAddr0FhRegAddrC8hEepm8")]
        public string DevAddr0FhRegAddrC8hEepm8;
        [Description("R,DevAddr0FhRegAddrC9hEepm9")]
        public string DevAddr0FhRegAddrC9hEepm9;
        [Description("R,DevAddr0FhRegAddrCAhEepm10")]
        public string DevAddr0FhRegAddrCAhEepm10;
        [Description("R,DevAddr0FhRegAddrCBhEepm11")]
        public string DevAddr0FhRegAddrCBhEepm11;
        [Description("R,DevAddr0FhRegAddrCChEepm12")]
        public string DevAddr0FhRegAddrCChEepm12;
        [Description("R,DevAddr0FhRegAddrCDhEepm13")]
        public string DevAddr0FhRegAddrCDhEepm13;
        [Description("R,DevAddr0FhRegAddrCEhEepm14")]
        public string DevAddr0FhRegAddrCEhEepm14;
        [Description("R,DevAddr0FhRegAddrCFhEepm15")]
        public string DevAddr0FhRegAddrCFhEepm15;
        #endregion

        #region 恢复出厂设置

        [Description("所有地址的芯片恢复929120的出厂设置")]
        public void AllDeviceReset929120Paras()
        {
            var isCheckCrcTemp = IsCheckCrcWhenWtire;

            IsCheckCrcWhenWtire = false;

            var toCrcData = new List<byte>();

            var dataList = new List<Dictionary<byte, byte>>();
            dataList.Add(new Dictionary<byte, byte>());
            dataList.Add(new Dictionary<byte, byte>());
            dataList.Add(new Dictionary<byte, byte>());
            dataList.Add(new Dictionary<byte, byte>());

            var str80 =
                "128:63\r\n129:63\r\n130:63\r\n131:63\r\n132:63\r\n133:63\r\n134:63\r\n135:63\r\n136:63\r\n137:63\r\n138:63\r\n139:63";
            foreach (var t in str80.Split(new char[] { '\r', '\n' }))
            {
                if (!string.IsNullOrEmpty(t))
                {
                    var spT = t.Split(':');
                    var addr = byte.Parse(spT[0]);
                    var value = byte.Parse(spT[1]);
                    dataList[0].Add(addr, value);
                    toCrcData.Add(value);
                }
            }

            var stra0 = "160:255\r\n161:255\r\n162:255\r\n163:255\r\n164:255\r\n165:255\r\n166:255\r\n167:255\r\n168:255\r\n169:255\r\n170:255\r\n171:255";
            foreach (var t in stra0.Split(new char[] { '\r', '\n' }))
            {
                if (!string.IsNullOrEmpty(t))
                {
                    var spT = t.Split(':');
                    var addr = byte.Parse(spT[0]);
                    var value = byte.Parse(spT[1]);
                    dataList[1].Add(addr, value);
                    toCrcData.Add(value);
                }
            }

            var strc0 =
                "192:0\r\n193:0\r\n194:255\r\n195:15\r\n196:255\r\n197:15\r\n198:0\r\n199:167\r\n200:3\r\n201:0\r\n202:0\r\n203:0\r\n204:0\r\n205:0\r\n206:0";
            foreach (var t in strc0.Split(new char[] { '\r', '\n' }))
            {
                if (!string.IsNullOrEmpty(t))
                {
                    var spT = t.Split(':');
                    var addr = byte.Parse(spT[0]);
                    var value = byte.Parse(spT[1]);
                    dataList[2].Add(addr, value);
                    toCrcData.Add(value);
                }
            }

            var totalCrc = CALC_CRC(_tps929120TiCrcInfo, toCrcData.ToArray());
            dataList[dataList.Count - 1].Add(0xCF, (byte)totalCrc);

            ErgodicProgram(0.ToString(), 15.ToString(), dataList);
            IsCheckCrcWhenWtire = isCheckCrcTemp;
        }

        [Description("所有地址的芯片恢复929120A的出厂设置")]
        public void AllDeviceReset929120AParas()
        {
            var isCheckCrcTemp = IsCheckCrcWhenWtire;

            IsCheckCrcWhenWtire = false;

            var toCrcData = new List<byte>();

            var dataList = new List<Dictionary<byte, byte>>();
            dataList.Add(new Dictionary<byte, byte>());
            dataList.Add(new Dictionary<byte, byte>());
            dataList.Add(new Dictionary<byte, byte>());
            dataList.Add(new Dictionary<byte, byte>());

            var str80 =
                "128:63\r\n129:63\r\n130:63\r\n131:63\r\n132:63\r\n133:63\r\n134:63\r\n135:63\r\n136:63\r\n137:63\r\n138:63\r\n139:63";
            foreach (var t in str80.Split(new char[] { '\r', '\n' }))
            {
                if (!string.IsNullOrEmpty(t))
                {
                    var spT = t.Split(':');
                    var addr = byte.Parse(spT[0]);
                    var value = byte.Parse(spT[1]);
                    dataList[0].Add(addr, value);
                    toCrcData.Add(value);
                }
            }

            var stra0 = "160:255\r\n161:255\r\n162:255\r\n163:255\r\n164:255\r\n165:255\r\n166:255\r\n167:255\r\n168:255\r\n169:255\r\n170:255\r\n171:255";
            foreach (var t in stra0.Split(new char[] { '\r', '\n' }))
            {
                if (!string.IsNullOrEmpty(t))
                {
                    var spT = t.Split(':');
                    var addr = byte.Parse(spT[0]);
                    var value = byte.Parse(spT[1]);
                    dataList[1].Add(addr, value);
                    toCrcData.Add(value);
                }
            }

            var strc0 =
                "192:0\r\n193:0\r\n194:255\r\n195:15\r\n196:255\r\n197:15\r\n198:8\r\n199:167\r\n200:3\r\n201:0\r\n202:0\r\n203:0\r\n204:0\r\n205:0\r\n206:0";
            foreach (var t in strc0.Split(new char[] { '\r', '\n' }))
            {
                if (!string.IsNullOrEmpty(t))
                {
                    var spT = t.Split(':');
                    var addr = byte.Parse(spT[0]);
                    var value = byte.Parse(spT[1]);
                    dataList[2].Add(addr, value);
                    toCrcData.Add(value);
                }
            }

            var totalCrc = CALC_CRC(_tps929120TiCrcInfo, toCrcData.ToArray());
            dataList[dataList.Count - 1].Add(0xCF, (byte)totalCrc);

            ErgodicProgram(0.ToString(), 15.ToString(), dataList);
            IsCheckCrcWhenWtire = isCheckCrcTemp;
        }

        #endregion
    }
}
