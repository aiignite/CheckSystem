using CommonUtility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Threading;

namespace Controller
{
    public sealed class Dlpc120CheetahSpiReader : ControllerBase
    {
        [Description("R,Cal Format (Piccolo)")]
        public string CalFormatPoccolo = string.Empty;
        [Description("R,Cfg Format (Piccolo)")]
        public string CfgFormatPoccolo = string.Empty;

        [Description("R,Piccolo SW Version")]
        public string PiccoloSwVersion = string.Empty;
        [Description("R,DLPC120 RTL Version")]
        public string Dlpc120RtlVersion = string.Empty;

        [Description("R,Cal Data Identifier")]
        public string CalDataIdentifier = string.Empty;
        [Description("R,Cfg Data Identifier")]
        public string CfgDataIdentifier = string.Empty;

        [Description("R,ASIC App Flash Data")]
        public string AsicAppFlashData=string.Empty;

        private int _handle;
        private int _bitRate;
        private const byte SpiMasterDummyByte = 0;
        private const byte SpiSlaveDummyByte = byte.MaxValue;
        private const int MaxPacketSize = 259;
        public static readonly byte SpiStartCharacter = 165;
        public static readonly byte SpiEscCharacter = 90;
        public static readonly byte SpiEscapeStart = 0;
        public static readonly byte SpiEscapeEsc = 90;
        private static readonly byte[] WrData = new byte[MaxPacketSize + 1];
        private static byte[] _rdData = new byte[MaxPacketSize + 1];
        private static int _wrLength;
        private static int _rdLength;
        private static int _rdIndex;
        private static bool _write;
        private static int _cmdIndex;
        private int _commandExecutionDelay;

        public Dlpc120CheetahSpiReader(string name)
            : base(name)
        {
            const int num = 16;
            var array = new ushort[16];
            var uniqueIds = new uint[16];
            var num2 = CheetahApi.ch_find_devices_ext(num, array, num, uniqueIds);

            if (num2 <= 0)
                return;
            if (num2 > num)
                num2 = num;

            for (var i = 0; i < num2; i++)
            {
                if ((array[i] & CheetahApi.ChPortNotFree) != 0)
                {
                    array[i] &= (ushort)(~CheetahApi.ChPortNotFree);
                    continue;
                }
                OpenCheetah(array[i], 100, 3);
                break;
            }
        }

        private void OpenCheetah(int port, int bitrate, int mode)
        {
            _handle = CheetahApi.ch_open(port);
            if (_handle <= 0)
                return;
            CheetahApi.ch_spi_configure(_handle, (CheetahSpiPolarity)(mode >> 1), (CheetahSpiPhase)(mode & 1),
                CheetahSpiBitorder.ChSpiBitorderMsb, 0);
            CheetahApi.ch_target_power(_handle, CheetahApi.ChTargetPowerOn);
            CheetahApi.ch_sleep_ms(100);
            _bitRate = CheetahApi.ch_spi_bitrate(_handle, bitrate);
        }

        private void TransmitReceiveBytes(IReadOnlyList<byte> packet, int packetLen, out byte[] rdData2)
        {
            rdData2 = new byte[packetLen];
            if (_handle <= 0)
                return;
            CheetahApi.ch_spi_queue_clear(_handle);
            CheetahApi.ch_spi_queue_oe(_handle, 1);
            CheetahApi.ch_spi_queue_ss(_handle, 0);
            CheetahApi.ch_spi_queue_ss(_handle, 1);
            for (var i = 0; i < packetLen; i++)
            {
                CheetahApi.ch_spi_queue_byte(_handle, 1, packet[i]);
                CheetahApi.ch_spi_queue_delay_ns(_handle, 1000000);
            }
            CheetahApi.ch_spi_queue_ss(_handle, 0);
            CheetahApi.ch_spi_queue_oe(_handle, 0);
            CheetahApi.ch_spi_batch_shift(_handle, packetLen, rdData2);
        }

        private void QueueSingleByte(byte data)
        {
            CheetahApi.ch_spi_queue_clear(_handle);
            CheetahApi.ch_spi_queue_oe(_handle, 1);
            CheetahApi.ch_spi_queue_ss(_handle, 0);
            CheetahApi.ch_spi_queue_ss(_handle, 1);
            CheetahApi.ch_spi_queue_byte(_handle, 1, data);
            CheetahApi.ch_spi_queue_delay_ns(_handle, 1000000);
            CheetahApi.ch_spi_queue_ss(_handle, 0);
            CheetahApi.ch_spi_queue_oe(_handle, 0);
        }

        private void TransmitQueuedByte(out byte data)
        {
            var array = new byte[1];
            CheetahApi.ch_spi_batch_shift(_handle, 1, array);
            data = array[0];
        }

        public void GetCalFileFormatVersion()
        {
            CalFormatPoccolo = string.Empty;

            if (StartCmdPacking(CommandId.SpiCmdCalFormatVersion, false) != (int)Common.Pass)
                return;

            if (SendCmd() != (int)Common.Pass)
                return;

            uint value;
            if (GetData(out value, 4u) != (int)Common.Pass)
                return;

            CalFormatPoccolo = string.Format("{0}{1}{2}{3}", (char)(value & 0xFFu), (char)((value >> 8) & 0xFFu),
                (char)((value >> 16) & 0xFFu), (char)((value >> 24) & 0xFFu));
            //Console.WriteLine(CalFormatPoccolo);
        }

        public void GetCfgFileFormatVersion()
        {
            CfgFormatPoccolo = string.Empty;

            if (StartCmdPacking(CommandId.SpiCmdCfgFormatVersion, false) != (int)Common.Pass)
                return;

            if (SendCmd() != (int)Common.Pass)
                return;

            uint value;
            if (GetData(out value, 4u) != (int)Common.Pass)
                return;

            CfgFormatPoccolo = string.Format("{0}{1}{2}{3}", (char)(value & 0xFFu), (char)((value >> 8) & 0xFFu),
                (char)((value >> 16) & 0xFFu), (char)((value >> 24) & 0xFFu));
            //Console.WriteLine(CfgFormatPoccolo);
        }

        public void GetDlpc120RtlVersion()
        {
            uint value;
            if (ReadAsicRegister(4, out  value) != (int)Common.Pass)
            {
                //major = (minor = 0);
                //build = 0;
                return;
            }
            var major = (byte)(value & 0xFFu);
            var minor = (byte)((value >> 8) & 0xFFu);
            var build = (ushort)((value >> 16) & 0xFFu);

            Dlpc120RtlVersion = string.Format("{0}.{1}({2})", major, minor, build);
        }

        public void GetPiccoloVersion()
        {
            PiccoloSwVersion = string.Empty;

            if (StartCmdPacking(CommandId.SpiCmdVersion, false) != (int)Common.Pass)
                return;
            if (SendCmd() != (int)Common.Pass)
                return;

            uint major;
            uint minor;
            uint build;
            if (GetData(out major, 1u) == (int)Common.Pass &&
                GetData(out minor, 1u) == (int)Common.Pass &&
                GetData(out build, 2u) == (int)Common.Pass)
                PiccoloSwVersion = string.Format("{0}.{1}({2})", (byte)major, (byte)minor, (ushort)build);
        }

        public void GetCalibrationVersion()
        {
            CalDataIdentifier = string.Empty;
            CfgDataIdentifier = string.Empty;

            if (StartCmdPacking(CommandId.SpiCmdCalVersion, false) != (int)Common.Pass)
                return;
            if (SendCmd() != (int)Common.Pass)
                return;

            uint major;
            uint minor;
            uint build;
            uint cfgMajor;
            uint cfgMinor;

            if (GetData(out major, 1u) != (int)Common.Pass || GetData(out minor, 1u) != (int)Common.Pass ||
                GetData(out build, 2u) != (int)Common.Pass || GetData(out cfgMajor, 2u) != (int)Common.Pass ||
                GetData(out cfgMinor, 2u) != (int)Common.Pass)
                return;

            CalDataIdentifier = string.Format("{0:X8}",
                (uint)(((ushort)build << 16) | ((byte)minor << 8) | (byte)major));
            CfgDataIdentifier = string.Format("{0:X8}", (uint)(((ushort)cfgMinor << 16) | (ushort)cfgMajor));
        }

        public void GetAsicAppFlashIdentifier()
        {
            AsicAppFlashData = string.Empty;
            uint value;
            if (ReadAsicRegister(5, out value) != (int)Common.Pass)
            {
                return;
            }

            AsicAppFlashData = string.Format("{0:X8}", value);
        }

        private int ReadAsicRegister(byte register, out uint value)
        {
            value = 0;
            //return 0;
            if (StartCmdPacking(CommandId.SpiCmdAsicRegister, false) == (int)Common.Pass)
            {
                if (PutData(register, 1u) == (int)Common.Pass)
                {
                    if (SendCmd() != (int)Common.Pass)
                    {
                        value = 0u;
                        return (int)Common.Fail;
                    }

                    if (GetData(out value, 4u) == (int)Common.Pass)
                    {
                        return (int)Common.Pass;
                    }
                }
            }

            return (int)Common.Fail;
            //throw new Exception($"Cannot Read from Unknown Register [0x{Register:X2}]");
        }

        private static int StartCmdPacking(CommandId id, bool wr)
        {
            _wrLength = 0;
            if (GetCommandIndex(id, out _cmdIndex) != (int)Common.Pass)
            {
                return (int)Common.Fail;
            }
            WrData[0] = (byte)((uint)((int)id << 1) | 1u);
            WrData[1] = (byte)CmdList[_cmdIndex].WrLenRd;
            _rdLength = 3 + CmdList[_cmdIndex].RdLen;
            _write = wr;
            _wrLength = 2;
            return (int)Common.Pass;
        }

        private static int GetCommandIndex(CommandId cmd, out int index)
        {
            const int fAil = (int)Common.Fail;
            var num = 0;
            var num2 = CmdList.Count;
            var flag = false;
            while (!flag)
            {
                var num3 = num + num2 >> 1;
                if (CmdList[num3].CmdId == cmd)
                {
                    index = num3;
                    //flag = true;
                    return (int)Common.Pass;
                }
                if (num3 == num)
                {
                    flag = true;
                }
                else if (CmdList[num3].CmdId > cmd)
                {
                    num2 = num3;
                }
                else
                {
                    num = num3;
                }
            }
            index = 0;
            return fAil;
        }

        private int SendCmd()
        {
            byte response;
            return SendCmd(out response);
        }

        private int SendCmd(out byte response)
        {
            return SendCmd(out response, true);
        }

        private int SendCmd(out byte response, bool timeout)
        {
            var num = 1;
            byte b = 0;
            if (_wrLength == 0)
            {
                response = 0;
                //ErrStr = "SendCmd called before cmd packing!";
                //throw new Exception(ErrStr);
                return (int)Common.Fail;
            }
            var array = new byte[_wrLength * 2 + 1];
            array[0] = SpiStartCharacter;
            for (var i = 0; i < _wrLength; i++)
            {
                b = (byte)(b + WrData[i]);
                if (WrData[i] == SpiStartCharacter)
                {
                    array[num] = SpiEscCharacter;
                    array[num + 1] = SpiEscapeStart;
                    num += 2;
                }
                else if (WrData[i] == SpiEscCharacter)
                {
                    array[num] = SpiEscCharacter;
                    array[num + 1] = SpiEscapeEsc;
                    num += 2;
                }
                else
                {
                    array[num] = WrData[i];
                    num++;
                }
            }
            if (b == SpiStartCharacter)
            {
                array[num] = SpiEscCharacter;
                array[num + 1] = SpiEscapeStart;
                num += 2;
            }
            else if (b == SpiEscCharacter)
            {
                array[num] = SpiEscCharacter;
                array[num + 1] = SpiEscapeEsc;
                num += 2;
            }
            else
            {
                array[num] = b;
                num++;
            }
            response = 0;
            _rdIndex = 2;
            var arg = !_write ? "Read" : "Write";
            while (true)
            {
                try
                {
                    SendCommand(array, num, ref _rdData, _rdLength, timeout);
                }
                catch (Exception)
                {
                    _wrLength = 0;
                    //ErrStr = string.Format("Command : {1} ({2})\r\n{0}", ex.Message, CmdList[CmdIndex].Name, arg);
                    //throw new Exception(ErrStr)
                    //{
                    //    Data = { 
                    //    {
                    //        (object)CommandExDataKeys.CmdID,
                    //        (object)CmdList[CmdIndex].CmdId
                    //    } }
                    //};
                }
                response = _rdData[0];
                switch (response)
                {
                    case 1:
                        _wrLength = 0;
                        return (int)Common.Pass;
                }
                break;
            }

            _wrLength = 0;
            return (int)Common.Fail;
        }

        public void SendCommand(byte[] packet, int packetLen, ref byte[] rdData, int rdLen, bool timeout)
        {
            var num = 0u;
            var num2 = 0;
            var readPacketState = ReadPacketState.Response;
            var flag2 = false;
            //string text = "";
            //if (!ConnectionWrapper.AttemptConnection())
            //{
            //    throw new Exception("Cannot Transmit Data when SPI Adapter is not connected!");
            //}

            var a = packet[1];
            var flag = (a & 1) == 1;
            byte[] rdData2;
            TransmitReceiveBytes(packet, packetLen, out rdData2);
            //for (uint num3 = 0u; num3 < packetLen; num3++)
            //{
            //    // text = $"{text} {Packet[num3]:X2}{RdData2[num3]:X2}";
            //}
            QueueSingleByte(SpiMasterDummyByte);
            if (_commandExecutionDelay != 0)
            {
                Thread.Sleep(_commandExecutionDelay);
                _commandExecutionDelay = 0;
            }
            while (!flag2)
            {
                num++;
                if (timeout && num > 1000)
                {
                    //MainForm.SpiCmdLogPage.EventLog(text, SpiCmdLog.EventType.SPI_CMD_LOG_FAIL);
                    throw new Exception("ERROR: Timed out waiting for response");
                }
                byte data;
                TransmitQueuedByte(out data);
                //text = $"{text} {SPI_MASTER_DUMMY_BYTE:X2}{Data:X2}";
                switch (readPacketState)
                {
                    case ReadPacketState.Response:
                        if (data == SpiSlaveDummyByte)
                        {
                            break;
                        }
                        rdData[0] = data;
                        if (data == 1)
                        {
                            if (flag)
                            {
                                readPacketState = ReadPacketState.Length;
                            }
                            else
                            {
                                flag2 = true;
                            }
                        }
                        else
                        {
                            flag2 = true;
                        }
                        break;

                    case ReadPacketState.Length:
                        {
                            byte b;
                            rdData[1] = b = data;
                            num2 = b;
                            if (rdLen != -1)
                            {
                                if (rdData[1] != rdLen - 3)
                                {
                                    //throw new Exception($"Mismatch between Expected Length {RdLen - 3} and Length Value Returned {RdData[1]}");
                                    throw new Exception(string.Format("Mismatch between Expected Length {0} and Length Value Returned {1}", rdLen - 3, rdData[1]));
                                }
                            }
                            else
                            {
                                rdLen = num2 + 3;
                            }
                            readPacketState = ReadPacketState.Data;
                            break;
                        }

                    case ReadPacketState.Data:
                        rdData[2 + rdLen - 3 - num2] = data;
                        num2--;
                        if (num2 == 0)
                        {
                            readPacketState = ReadPacketState.Readchksm;
                        }
                        break;

                    case ReadPacketState.Readchksm:
                        rdData[rdLen - 1] = data;
                        flag2 = true;
                        break;
                }
            }
        }

        private void PutData(IEnumerable<byte> values)
        {
            foreach (var value in values)
            {
                WrData[_wrLength++] = value;
            }
        }

        private static int PutData(uint value, uint sizeBytes)
        {
            if (_wrLength > MaxPacketSize)
            {
                //ErrStr = $"TI SPI Protocol Packet Length overflow detected in control program - {WrLength}";
                //throw new Exception(ErrStr);
                return (int)Common.Fail;
            }
            switch (sizeBytes)
            {
                case 1u:
                    WrData[_wrLength] = (byte)(value & 0xFFu);
                    _wrLength++;
                    break;
                case 2u:
                    WrData[_wrLength] = (byte)(value & 0xFFu);
                    _wrLength++;
                    WrData[_wrLength] = (byte)((value >> 8) & 0xFFu);
                    _wrLength++;
                    break;
                case 4u:
                    WrData[_wrLength] = (byte)(value & 0xFFu);
                    _wrLength++;
                    WrData[_wrLength] = (byte)((value >> 8) & 0xFFu);
                    _wrLength++;
                    WrData[_wrLength] = (byte)((value >> 16) & 0xFFu);
                    _wrLength++;
                    WrData[_wrLength] = (byte)((value >> 24) & 0xFFu);
                    _wrLength++;
                    break;
                default:
                    //ErrStr = $"Invalid SizeBytes Parameter {SizeBytes}\r\nValue should be 1, 2 or 4";
                    //throw new Exception(ErrStr);
                    return (int)Common.Fail;
            }
            return (int)Common.Pass;
        }

        private void GetData(out List<byte> values)
        {
            if (_write)
            {
                values = null;
                return;
            }
            values = new List<byte>();
            for (var i = 0; i < _rdData[1]; i++)
            {
                values.Add(_rdData[2 + i]);
            }
        }

        private static int GetData(out uint value, uint sizeBytes)
        {
            value = 0;
            if (_rdIndex + sizeBytes > _rdLength)
            {
                //ErrStr = $"Cannot read more data for command {CmdList[CmdIndex].Name}";
                //throw new Exception(ErrStr);
                return (int)Common.Fail;
            }
            switch (sizeBytes)
            {
                case 1u:
                    value = _rdData[_rdIndex];
                    _rdIndex++;
                    break;

                case 2u:
                    value = (uint)(_rdData[_rdIndex] | (_rdData[_rdIndex + 1] << 8));
                    _rdIndex += 2;
                    break;

                case 4u:
                    value = (uint)(_rdData[_rdIndex] | (_rdData[_rdIndex + 1] << 8) | (_rdData[_rdIndex + 2] << 16) | (_rdData[_rdIndex + 3] << 24));
                    _rdIndex += 4;
                    break;

                default:
                    return (int)Common.Fail;
                    //ErrStr = $"Invalid SizeBytes Parameter {SizeBytes}\r\nValue should be 1, 2 or 4";
                    //throw new Exception(ErrStr);
            }

            return (int)Common.Pass;
        }

        internal static readonly IList<CommandList> CmdList = new ReadOnlyCollection<CommandList>(new[]
        {
            new CommandList(CommandId.SpiCmdDimming, 2, 0, 2, true, true, "Backlight"),
            new CommandList(CommandId.SpiCmdMasterOnOff, 1, 0, 1, true, true, "Master On/Off"),
            new CommandList(CommandId.SpiCmdLedname, 0, 0, 30, false, true, "LED Name"),
            new CommandList(CommandId.SpiCmdSplashctrl, 1, 0, 1, true, true, "Splash Control"),
            new CommandList(CommandId.SpiCmdDmdDriveStrength, 1, 0, 1, true, true, "DMD Drive Strength"),
            new CommandList(CommandId.SpiCmdHeaterPwmParameters, 3, 0, 3, true, true, "Heater PWM Parameters"),
            new CommandList(CommandId.SpiCmdBezelOffset, 6, 0, 6, true, true, "Bezel Adjustment"),
            new CommandList(CommandId.SpiCmdPrepareAsicFlash, 1, 0, 4, true, true, "Prepare ASIC Flash"),
            new CommandList(CommandId.SpiCmdBistResults, 0, 0, 13, false, true, "BIST Results"),
            new CommandList(CommandId.SpiCmdAsicInitBranch, 0, 0, 1, false, true, "ASIC Initialization Type"),
            new CommandList(CommandId.SpiCmdVersion, 0, 0, 4, false, true, "Piccolo SW Version"),
            new CommandList(CommandId.SpiCmdStatus, 0, 0, 4, false, true, "Software Status"),
            new CommandList(CommandId.SpiCmdAsicRegister, 5, 1, 4, true, true, "ASIC Register"),
            new CommandList(CommandId.SpiCmdVacControl, 3, 0, 3, true, true, "VAC Control"),
            new CommandList(CommandId.SpiCmdOperatingMode, 0, 0, 1, false, true, "Operating Mode"),
            new CommandList(CommandId.SpiCmdPwmSensitivity, 0, 0, 2, false, true, "PWM Sensitivity"),
            new CommandList(CommandId.SpiCmdSecondaryStatus, 0, 0, 4, false, true, "Software Secondary Status"),
            new CommandList(CommandId.SpiCmdKeyInfo, 0, 4, 4, false, true, "Key Info"),
            new CommandList(CommandId.SpiCmdKeyValue, 0, 4, 4, false, true, "Key Values"),
            new CommandList(CommandId.SpiCmdReadFeatureEnable, 0, 0, 1, false, true, "Feature Enable"),
            new CommandList(CommandId.SpiCmdSeqtableIndex, 2, 0, 4, true, true, "Sequence Table Index"),
            new CommandList(CommandId.SpiCmdSeqtableInfo, 0, 1, 35, false, true, "Sequence Table Info"),
            new CommandList(CommandId.SpiCmdGammaInfo, 0, 2, 32, false, true, "Gamma Info"),
            new CommandList(CommandId.SpiCmdCmdlistAddr, 0, 2, 4, false, true, "Command List Address"),
            new CommandList(CommandId.SpiCmdCmdlistGenericType, 0, 1, 31, false, true, "Generic Command List Type"),
            new CommandList(CommandId.SpiCmdCmdlistNum, 0, 1, 2, true, true, "Command List Numbers"),
            new CommandList(CommandId.SpiCmdCmdlist, 2, 2, 31, true, true, "Command List"),
            new CommandList(CommandId.SpiCmdFevbistpixels, 8, 0, 8, true, true,
                "Front End Video BIST Start and End Pixels"),
            new CommandList(CommandId.SpiCmdFevbist, 0, 0, 5, true, true, "Front End Video BIST execution and results"),
            new CommandList(CommandId.SpiCmdEvdbist, 8, 0, 17, true, true, "External Video BIST execution and results"),
            new CommandList(CommandId.SpiCmdTemperatureLpfConsts, 8, 0, 8, true, true, "Temperature LPF Constants"),
            new CommandList(CommandId.SpiCmdTemperatureCmp, 3, 0, 4, true, true, "Temperature Compensation"),
            new CommandList(CommandId.SpiCmdLedvandi, 0, 0, 8, true, true, "LED Voltage and Current"),
            new CommandList(CommandId.SpiCmdTmp411Temperature, 0, 0, 2, false, true, "TMP411 Temperature"),
            new CommandList(CommandId.SpiCmdCalibrationMode, 1, 0, 1, true, true, "Calibration Mode"),
            new CommandList(CommandId.SpiCmdRedLedPwm, 2, 0, 2, true, true, "Red LED PWM"),
            new CommandList(CommandId.SpiCmdGrnLedPwm, 2, 0, 2, true, true, "Green LED PWM"),
            new CommandList(CommandId.SpiCmdBluLedPwm, 2, 0, 2, true, true, "Blue LED PWM"),
            new CommandList(CommandId.SpiCmdCurrLimitPwm, 2, 0, 2, true, true, "Current Limit PWM"),
            new CommandList(CommandId.SpiCmdSensorGain, 1, 0, 1, true, true, "Sensor Gain"),
            new CommandList(CommandId.SpiCmdCmdTblIndex, 1, 0, 3, true, true, "LDC Index"),
            new CommandList(CommandId.SpiCmdSensorgainmap, 0, 0, 4, false, true, "Sensor Gain Mapping Info"),
            new CommandList(CommandId.SpiCmdAdapterVoltages, 0, 0, 12, false, true, "Adapter ADC Voltages Info"),
            new CommandList(CommandId.SpiCmdCfgFormatVersion, 0, 0, 4, false, true, "Config File Format Version"),
            new CommandList(CommandId.SpiCmdCalFormatVersion, 0, 0, 4, false, true,
                "Calibration File Format Version"),
            new CommandList(CommandId.SpiCmdCalVersion, 0, 0, 8, false, true, "Calibration Data Version"),
            new CommandList(CommandId.SpiCmdCalProgramToFlash, 255, 0, 255, true, true,
                "Flash Program Calibration Data"),
            new CommandList(CommandId.SpiCmdReadPiccoloFlash, 4, 1, 255, true, true, "Piccolo Flash Data Read"),
            new CommandList(CommandId.SpiCmdPwmPeriod, 2, 0, 8, true, true, "PWM Period"),
            new CommandList(CommandId.SpiCmdPwmScalefactor, 0, 0, 6, false, true, "PWM Scalefactor"),
            new CommandList(CommandId.SpiCmdAsicFlashReadwrite, 255, 1, 255, true, true, "ASIC Flash Read/Write"),
            new CommandList(CommandId.SpiCmdAsicFlashReadInfo, 8, 0, 12, true, true, "ASIC Flash Read Info"),
            new CommandList(CommandId.SpiCmdAsicFlashWriteInfo, 8, 0, 12, true, true, "ASIC Flash Write Info"),
            new CommandList(CommandId.SpiCmdAsicFlashSectorErase, 4, 0, 0, true, false, "ASIC Flash Sector Erase"),
            new CommandList(CommandId.SpiCmdPowerRailVoltages, 0, 0, 17, false, true, "Power rail voltage read"),
            new CommandList(CommandId.SpiCmdEnableVoltageMon, 1, 0, 1, true, true, "Enable/Disable Voltage Monitor"),
            new CommandList(CommandId.SpiCmdToggleMode, 0, 5, 4, true, false, "Toggle Mode"),
            new CommandList(CommandId.SpiCmdAppProgramPiccoloSw, 255, 1, 1, true, true,
                "Flash Program Application Data"),
            new CommandList(CommandId.SpiCmdI2CClkRate, 1, 0, 1, true, true, "I2C Clock Speed"),
            new CommandList(CommandId.SpiCmdDbgprint, 0, 0, 10, true, true, "Debug Data"),
            new CommandList(CommandId.SpiCmdIsInBootloader, 0, 0, 1, false, true, "Operating Mode"),
            new CommandList(CommandId.SpiCmdUnknown, 0, 0, 0, true, true, "Unknown Command")
        });

        internal struct CommandList
        {
            public CommandId CmdId;

            public ushort WrLen;

            public ushort WrLenRd;

            public ushort RdLen;

            public bool ReadAllowed;

            public bool WriteAllowed;

            public string Name;

            public CommandList(CommandId id, ushort writeLen, ushort writeLenRead, ushort readLen, bool write, bool read, string cmdName)
            {
                CmdId = id;
                WrLen = writeLen;
                WrLenRd = writeLenRead;
                RdLen = readLen;
                ReadAllowed = read;
                WriteAllowed = write;
                Name = cmdName;
            }
        }

        internal class Register
        {
            public class Field
            {
                public string Name;

                public string Desc;

                public string Bits;

                public byte Lsb;

                public byte Msb;

                public uint Reset;

                public uint Value;

                public string Type;

                public Field(string name, string desc, string bits, string lsb, string msb, string reset, string type)
                {
                    Name = name;
                    Desc = desc;
                    Bits = bits;
                    Lsb = byte.Parse(lsb);
                    Msb = byte.Parse(msb);
                    Reset = uint.Parse(reset.Substring(2), NumberStyles.HexNumber);
                    Type = type;
                }

                public void SetValue(uint value)
                {
                    Value = value;
                }
            }

            public byte Addr;

            public string Name;

            public string Desc;

            public List<Field> Fields;

            public Register()
            {
            }

            public Register(string addr, string name, string desc)
            {
                if (!byte.TryParse(addr.Substring(2), NumberStyles.HexNumber, CultureInfo.CurrentCulture, out Addr))
                    return;
                Name = name;
                Desc = desc;
                Fields = null;
            }
        }

        internal enum CommandId
        {
            SpiCmdDimming = 0,
            SpiCmdMasterOnOff = 1,
            SpiCmdLedname = 3,
            SpiCmdSplashctrl = 37,
            SpiCmdDmdDriveStrength = 38,
            SpiCmdHeaterPwmParameters = 39,
            SpiCmdBezelOffset = 40,
            SpiCmdPrepareAsicFlash = 47,
            SpiCmdBistResults = 48,
            SpiCmdAsicInitBranch = 49,
            SpiCmdVersion = 50,
            SpiCmdStatus = 51,
            SpiCmdAsicRegister = 52,
            SpiCmdVacControl = 53,
            SpiCmdOperatingMode = 54,
            SpiCmdPwmSensitivity = 55,
            SpiCmdSecondaryStatus = 56,
            SpiCmdKeyInfo = 57,
            SpiCmdKeyValue = 58,
            SpiCmdReadFeatureEnable = 59,
            SpiCmdSeqtableIndex = 64,
            SpiCmdSeqtableInfo = 65,
            SpiCmdGammaInfo = 67,
            SpiCmdCmdlistAddr = 78,
            SpiCmdCmdlistGenericType = 79,
            SpiCmdCmdlistNum = 80,
            SpiCmdCmdlist = 81,
            SpiCmdFevbistpixels = 83,
            SpiCmdFevbist = 84,
            SpiCmdEvdbist = 85,
            SpiCmdTemperatureLpfConsts = 96,
            SpiCmdTemperatureCmp = 97,
            SpiCmdLedvandi = 98,
            SpiCmdTmp411Temperature = 99,
            SpiCmdCalibrationMode = 100,
            SpiCmdRedLedPwm = 101,
            SpiCmdGrnLedPwm = 102,
            SpiCmdBluLedPwm = 103,
            SpiCmdCurrLimitPwm = 104,
            SpiCmdSensorGain = 105,
            SpiCmdCmdTblIndex = 106,
            SpiCmdSensorgainmap = 107,
            SpiCmdAdapterVoltages = 108,
            SpiCmdCfgFormatVersion = 109,
            SpiCmdCalFormatVersion = 110,
            SpiCmdCalVersion = 111,
            SpiCmdCalProgramToFlash = 112,
            SpiCmdReadPiccoloFlash = 113,
            SpiCmdPwmPeriod = 114,
            SpiCmdPwmScalefactor = 115,
            SpiCmdAsicFlashReadwrite = 116,
            SpiCmdAsicFlashReadInfo = 117,
            SpiCmdAsicFlashWriteInfo = 118,
            SpiCmdAsicFlashSectorErase = 119,
            SpiCmdPowerRailVoltages = 120,
            SpiCmdEnableVoltageMon = 121,
            SpiCmdToggleMode = 122,
            SpiCmdAppProgramPiccoloSw = 123,
            SpiCmdI2CClkRate = 124,
            SpiCmdDbgprint = 125,
            SpiCmdIsInBootloader = 126,
            SpiCmdUnknown = 255
        }

        internal enum Common
        {
            Pass,

            Fail
        }

        private enum ReadPacketState
        {
            Response = 1,
            Length,
            Data,
            Readchksm
        }
    }
}
