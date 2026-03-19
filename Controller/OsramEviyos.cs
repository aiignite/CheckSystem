using CommonUtility;
using CommonUtility.FileOperator;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Controller
{
    public sealed class OsramEviyos : ControllerBase
    {
        public MySerialPort MySerialPort;

        private const byte FirstByte = 0x80;
        private const byte SecondByte = 0xA5;
        private bool _isRead;
        private readonly List<string> _readBuffer = new List<string>();

        public OsramEviyos(string name)
            : base(name)
        {
            var bs = FormatData(true, true, false, 16, 0);

            MySerialPort.PushSciMsg += MySerialPort_PushSciMsg;

            MainThread = new Thread(MainWork) { IsBackground = true };
            MainThread.Start();
        }

        private void MySerialPort_PushSciMsg(string name, byte[] datas)
        {
            if (MySerialPort == null || MySerialPort.Name != name)
                return;

            var str =
                datas.Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));
            //Debug.WriteLine(str);

            var sp = str.Split(new string[] { ValueHelper.GetHextStr(new byte[] { FirstByte, SecondByte }).Replace(" ", "") }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var t in sp)
            {
                if (!string.IsNullOrEmpty(t) && t.Length % 2 == 0)
                {
                    var bs = new List<byte> { 0x80, 0xA5 };
                    for (var i = 0; i < t.Length; i = i + 2)
                        bs.Add(Convert.ToByte(t.Substring(i, 2), 16));
                    ProcessMsg(bs.ToArray());
                }
            }
        }

        private void ProcessMsg(byte[] datas)
        {
            var str =
                datas.Aggregate(string.Empty, (current, t) => current + ValueHelper.GetHextStr(t));

            var bitsStr = Convert.ToString(datas[2], 2).PadLeft(8, '0').ToCharArray().Reverse().ToList();
            if (bitsStr[7].ToString() == 0.ToString())
            {
                var toPrintStr = ValueHelper.GetHextStr(datas).Replace(" ", "");

                if (toPrintStr.StartsWith("80A5202C"))
                {
                    //Console.WriteLine(toPrintStr);
                }

                if (toPrintStr != "80A52006300075EF")
                {
                    //Console.WriteLine(toPrintStr);
                }
                //Console.WriteLine(toPrintStr);
            }

            if (bitsStr[7].ToString() == 1.ToString())
            {
                if ((str.StartsWith("80A5BE70") || str.StartsWith("80A5BE80") || str.StartsWith("80A5BE90") || str.StartsWith("80A5BEA0")) && str.Length == 76)
                {
                    //Console.WriteLine("ELEDERP: " + str);
                }
                else if ((str.StartsWith("80A5BEB0") || str.StartsWith("80A5BEC0") || str.StartsWith("80A5BED0") ||
                          str.StartsWith("80A5BEE0")) && str.Length == 76)
                {
                    //Console.WriteLine("ELEDERS: " + str);
                }
                else if ((str.StartsWith("80A5BE1C") || str.StartsWith("80A5BE1E") || str.StartsWith("80A5BE1F")))
                {
                    //Console.WriteLine(str);
                }
                else
                {
                    //Console.WriteLine(str);
                }
            }
            if ((str.StartsWith("80A5BE70") || str.StartsWith("80A5BE80") || str.StartsWith("80A5BE90") ||
                 str.StartsWith("80A5BEA0")) && str.Length == 76)
            {
                //Console.WriteLine("ELEDERP: " + str);
            }
            else if ((str.StartsWith("80A5BEB0") || str.StartsWith("80A5BEC0") || str.StartsWith("80A5BED0") || str.StartsWith("80A5BEE0")) && str.Length == 76)
            {
                //Console.WriteLine("ELEDERS: " + str);
            }
            if (_isRead)
            {
                if ((str.StartsWith("80A5BE70") || str.StartsWith("80A5BE80") || str.StartsWith("80A5BE90") || str.StartsWith("80A5BEA0")) && str.Length == 76)
                {
                    var strValue = str.Substring(8, 64);
                    var points = new List<Point>();
                    for (var i = 0; i < strValue.Length; i = i + 4)
                    {
                        var bY = Convert.ToByte(strValue.Substring(i, 2), 16);
                        var bX = Convert.ToByte(strValue.Substring(i + 2, 2), 16);
                        points.Add(new Point(bX, bY));
                    }
                    var isToSet = false;

                    if (!_readBuffer.Contains("80A5BE70"))
                    {
                        isToSet = true;
                        RegRead70To7F = str.Substring(8, 64);
                        _readBuffer.Add("80A5BE70");
                    }
                    else if (!_readBuffer.Contains("80A5BE80"))
                    {
                        isToSet = true;
                        RegRead80To8F = str.Substring(8, 64);
                        _readBuffer.Add("80A5BE80");
                    }
                    else if (!_readBuffer.Contains("80A5BE90"))
                    {
                        isToSet = true;
                        RegRead90To9F = str.Substring(8, 64);
                        _readBuffer.Add("80A5BE90");
                    }
                    else if (!_readBuffer.Contains("80A5BEA0"))
                    {
                        isToSet = true;
                        RegReadA0ToAf = str.Substring(8, 64);
                        _readBuffer.Add("80A5BEA0");
                    }

                    if (isToSet)
                    {
                        //Console.WriteLine(str);
                    }

                    for (var i = 0; i < points.Count; i++)
                    {
                        var baseLedIndex = 0;

                        if (str.StartsWith("80A5BE70"))
                        {
                            baseLedIndex = 1;
                        }
                        else if (str.StartsWith("80A5BE80"))
                        {
                            baseLedIndex = 17;
                        }
                        else if (str.StartsWith("80A5BE90"))
                        {
                            baseLedIndex = 33;
                        }
                        else if (str.StartsWith("80A5BEA0"))
                        {
                            baseLedIndex = 49;
                        }

                        if (isToSet)
                        {
                            var fieldName = string.Format("Led{0}Position", baseLedIndex + i);
                            var field = GetType().GetField(fieldName);
                            if (field != null)
                                field.SetValue(this, string.Format("{0},{1}", points[i].X, points[i].Y));
                        }
                    }

                    //Console.WriteLine("ELEDERP: " + str);
                }
                else if ((str.StartsWith("80A5BEB0") || str.StartsWith("80A5BEC0") || str.StartsWith("80A5BED0") || str.StartsWith("80A5BEE0")) && str.Length == 76)
                {
                    var strValue = str.Substring(8, 64);
                    var state = new List<byte[]>();
                    for (var i = 0; i < strValue.Length; i = i + 4)
                    {
                        var pxstate = Convert.ToByte(strValue.Substring(i, 2), 16);
                        var pxdiag = Convert.ToByte(strValue.Substring(i + 2, 2), 16);
                        state.Add(new[] { pxstate, pxdiag });
                    }
                    var isToSet = false;

                    if (!_readBuffer.Contains("80A5BEB0"))
                    {
                        isToSet = true;
                        RegReadB0ToBf = str.Substring(8, 64);
                        _readBuffer.Add("80A5BEB0");
                    }
                    else if (!_readBuffer.Contains("80A5BEC0"))
                    {
                        isToSet = true;
                        RegReadC0ToCf = str.Substring(8, 64);
                        _readBuffer.Add("80A5BEC0");
                    }
                    else if (!_readBuffer.Contains("80A5BED0"))
                    {
                        isToSet = true;
                        RegReadD0ToDf = str.Substring(8, 64);
                        _readBuffer.Add("80A5BED0");
                    }
                    else if (!_readBuffer.Contains("80A5BEE0"))
                    {
                        isToSet = true;
                        RegReadE0ToEf = str.Substring(8, 64);
                        _readBuffer.Add("80A5BEE0");
                    }

                    if (isToSet)
                    {
                        Console.WriteLine(str);
                    }

                    for (var i = 0; i < state.Count; i++)
                    {
                        var baseLedIndex = 0;

                        if (str.StartsWith("80A5BEB0"))
                        {
                            baseLedIndex = 1;
                        }
                        else if (str.StartsWith("80A5BEC0"))
                        {
                            baseLedIndex = 17;
                        }
                        else if (str.StartsWith("80A5BED0"))
                        {
                            baseLedIndex = 33;
                        }
                        else if (str.StartsWith("80A5BEE0"))
                        {
                            baseLedIndex = 49;
                        }

                        if (isToSet)
                        {
                            var fieldName = string.Format("Led{0}ErrorType", baseLedIndex + i);
                            var field = GetType().GetField(fieldName);
                            if (field != null)
                            {
                                var pxstate = state[i][0] == 0x00 ? "LED OFF" : "LED ON";
                                var pxdiag = "no defect";
                                if (state[i][1] == 0x00)
                                    pxdiag = "no defect";
                                else if (state[i][1] == 0x01)
                                    pxdiag = "S2G";
                                else if (state[i][1] == 0x02)
                                    pxdiag = "OPEN";
                                else if (state[i][1] == 0x03)
                                    pxdiag = " not used";

                                field.SetValue(this, string.Format("{0},{1}", pxstate, pxdiag));
                            }
                        }
                    }

                    //Console.WriteLine("ELEDERS: " + str);
                }
            }

            if (_isReadConfigRegisters)
            {
                if ((str.StartsWith("80A5BE00") || str.StartsWith("80A5BE10") || str.StartsWith("80A5BE20") || str.StartsWith("80A5BEF0")) && str.Length == 76)
                {
                    var findIndex = _readConfigRegistersBuff.FindIndex(f => f.StartsWith(str.Substring(0, 8)));
                    if (findIndex == -1)
                        _readConfigRegistersBuff.Add(str);
                    else
                        _readConfigRegistersBuff[findIndex] = str;
                }
            }
        }

        ~OsramEviyos()
        {
            Dispose();
            if (HikBarcodeScaner == null)
                return;
            HikBarcodeScaner.Stop();
            HikBarcodeScaner.OnPushSocketsToTcpClient -=
                tcpClient_OnPushSocketsToTcpClient;
        }

        #region CAN相关
        [Description("开启CAN")]
        public void StartCan()
        {
            if (MySerialPort != null)
            {
                //MySerialPort.SendCommand(StringToBytes("80A520063510982B"));
                //MySerialPort.SendCommand(StringToBytes("80A52006300075EF"));
                //MySerialPort.SendCommand(StringToBytes("80A52006300075EF"));
                //MySerialPort.SendCommand(StringToBytes("80A52006300075EF"));
                //MySerialPort.SendCommand(StringToBytes("80A5201C75000374"));
                //MySerialPort.SendCommand(StringToBytes("80A5201F01C267B7"));
                //MySerialPort.SendCommand(StringToBytes("80A5202ED2013C79"));
                //MySerialPort.SendCommand(StringToBytes("80A5202CF600881A"));
                //MySerialPort.SendCommand(StringToBytes("80A52006300075EF"));
                //MySerialPort.SendCommand(StringToBytes("80A52003FFFF8685"));
                //MySerialPort.SendCommand(StringToBytes("80A5200010F13E97"));
                //MySerialPort.SendCommand(StringToBytes("80A520150064466B"));
                //MySerialPort.SendCommand(StringToBytes("80A52016008C631D"));
                //MySerialPort.SendCommand(StringToBytes("80A52017003FC395"));
                //MySerialPort.SendCommand(StringToBytes("80A52018008C781C"));
                //MySerialPort.SendCommand(StringToBytes("80A5201A00FF5888"));
                //MySerialPort.SendCommand(StringToBytes("80A5201C3003C12E"));
                //MySerialPort.SendCommand(StringToBytes("80A5201EFFFF87B7"));
                //MySerialPort.SendCommand(StringToBytes("80A5201F01C267B7"));
                //MySerialPort.SendCommand(StringToBytes("80A5202C0000317D"));
                //MySerialPort.SendCommand(StringToBytes("80A520FCFACCB023"));

                //MySerialPort.SendCommand(StringToBytes("80A52006300075EF"));
                //MySerialPort.SendCommand(StringToBytes("80A52006300075EF"));
                //MySerialPort.SendCommand(StringToBytes("80A520010003C589"));
            }

            IsStart = true;
        }

        [Description("关闭CAN")]
        public void StopCan()
        {
            IsStart = false;
        }

        private Thread MainThread { get; set; }
        private bool IsStart { get; set; }

        private void MainWork()
        {
            while (MainThread.IsAlive)
            {
                Thread.Sleep(15);

                if (!MainThread.IsAlive)
                    break;

                if (!IsStart)
                    continue;

                try
                {
                    if (MySerialPort == null)
                        continue;

                    MySerialPort.SendCommand(StringToBytes("80A52006300075EF"));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        [Description("读LED ERROR")]
        public void ReadLedError()
        {
            RegRead70To7F = string.Empty;
            RegRead80To8F = string.Empty;
            RegRead90To9F = string.Empty;
            RegReadA0ToAf = string.Empty;
            RegReadB0ToBf = string.Empty;
            RegReadC0ToCf = string.Empty;
            RegReadD0ToDf = string.Empty;
            RegReadE0ToEf = string.Empty;

            for (var i = 1; i < 65; i++)
            {
                {
                    var fieldName = string.Format("Led{0}Position", i);
                    var field = GetType().GetField(fieldName);
                    if (field != null)
                        field.SetValue(this, string.Empty);
                }

                {
                    var fieldName = string.Format("Led{0}ErrorType", i);
                    var field = GetType().GetField(fieldName);
                    if (field != null)
                        field.SetValue(this, string.Empty);
                }
            }

            _readBuffer.Clear();

            //for (var i = 0; i < 5; i++)
            {
                //MySerialPort.SendCommand(new byte[] { 0x80, 0xA5, 0x20, 0x2C, 0xFE, 0x00, 0x01, 0xB3 });
                //Thread.Sleep(25);
            }

            _isRead = true;
            Thread.Sleep(2000);
            _isRead = false;

            if (IsHaveError(RegRead70To7F))
                RegRead70To7F = "NG " + RegRead70To7F;
            else
                RegRead70To7F = "OK " + RegRead70To7F;

            if (IsHaveError(RegRead80To8F))
                RegRead80To8F = "NG " + RegRead80To8F;
            else
                RegRead80To8F = "OK " + RegRead80To8F;

            if (IsHaveError(RegRead90To9F))
                RegRead90To9F = "NG " + RegRead90To9F;
            else
                RegRead90To9F = "OK " + RegRead90To9F;

            if (IsHaveError(RegReadA0ToAf))
                RegReadA0ToAf = "NG " + RegReadA0ToAf;
            else
                RegReadA0ToAf = "OK " + RegReadA0ToAf;

            if (IsHaveError(RegReadB0ToBf))
                RegReadB0ToBf = "NG " + RegReadB0ToBf;
            else
                RegReadB0ToBf = "OK " + RegReadB0ToBf;

            if (IsHaveError(RegReadC0ToCf))
                RegReadC0ToCf = "NG " + RegReadC0ToCf;
            else
                RegReadC0ToCf = "OK " + RegReadC0ToCf;

            if (IsHaveError(RegReadD0ToDf))
                RegReadD0ToDf = "NG " + RegReadD0ToDf;
            else
                RegReadD0ToDf = "OK " + RegReadD0ToDf;

            if (IsHaveError(RegReadE0ToEf))
                RegReadE0ToEf = "NG " + RegReadE0ToEf;
            else
                RegReadE0ToEf = "OK " + RegReadE0ToEf;

            //var points = new List<Point>();
            //for (var i = 1; i < 65; i++)
            //{
            //    var fieldName = string.Format("Led{0}Position", i);
            //    var field = GetType().GetField(fieldName);
            //    if (field != null)
            //    {
            //        try
            //        {
            //            var fieldValue = field.GetValue(this).ToString();
            //            var sp = fieldValue.Split(',');
            //            points.Add(new Point(int.Parse(sp[0]), int.Parse(sp[1])));
            //        }
            //        catch (Exception)
            //        {
            //            // ignored
            //        }
            //    }
            //}

            //ReadData(true, false, 0x070, 16);
            //ReadData(true, false, 0x080, 16);
            //ReadData(true, false, 0x090, 16);
            //ReadData(true, false, 0x0A0, 16);

            //ReadData(true, false, 0x0B0, 16);
            //ReadData(true, false, 0x0C0, 16);
            //ReadData(true, false, 0x0D0, 16);
            //ReadData(true, false, 0x0E0, 16);
        }

        private bool IsHaveError(string value)
        {
            if (string.IsNullOrEmpty(value))
                return true;

            for (var i = 0; i < value.Length; i++)
            {
                if (value[i].ToString() != "0")
                {
                    return true;
                }
            }

            return false;
        }

        //[Description("读寄存器")]
        private void ReadData(bool id0, bool id1, int startReg, int wordLen)
        {
            if (MySerialPort != null)
            {
                //MySerialPort.SendCommand(new byte[] { 0x80, 0xA5, 0xA0, 0xFB });
                MySerialPort.SendCommand(FormatData(true, id0, id1, wordLen, startReg));
            }
        }

        private void WrtieData(bool id0, bool id1, int startReg, byte[] values)
        {
            if (MySerialPort != null)
            {
                MySerialPort.SendCommand(FormatData(false, id0, id1, values.Length / 2, startReg, values));
            }
        }

        private static byte[] FormatData(bool isRead, bool id0, bool id1, int wordLen, int startReg, byte[] values = null)
        {
            var hctrlStr = string.Empty;
            hctrlStr += isRead ? "1" : "0";
            hctrlStr += id1 ? "1" : "0";
            hctrlStr += id0 ? "1" : "0";
            hctrlStr += Convert.ToString((byte)(wordLen - 1), 2).PadLeft(4, '0');

            var startRegStr = Convert.ToString(startReg, 2).PadLeft(9, '0');
            hctrlStr += startRegStr[0];

            var hadrStr = startRegStr.Substring(1, 8);

            var header = new List<byte> { Convert.ToByte(hctrlStr, 2), Convert.ToByte(hadrStr, 2) };

            if (!isRead && values != null)
            {
                header.AddRange(values);
                var crc = CalcCrc16(header.ToArray());
                header.AddRange(crc);
            }

            //if (values != null)
            //    header.AddRange(values);
            //var crc = CalcCrc16(header.ToArray());
            //header.AddRange(crc);

            header.Insert(0, SecondByte);
            header.Insert(0, FirstByte);

            return header.ToArray();
        }

        //[Description("解析")]
        //public void GetWriteData()
        //{
        //    var temp = new List<string>();
        //    using (var sr = new StreamReader(@"E:\Projects-2022\点灯&芯片相关\OSRAM EVIYOS\20230726.txt", Encoding.Default))
        //    {
        //        string line;
        //        while ((line = sr.ReadLine()) != null)
        //        {
        //            try
        //            {
        //                var str = line.Trim(' ');
        //                if (!string.IsNullOrEmpty(str))
        //                    temp.Add(str);
        //            }
        //            catch (Exception)
        //            {
        //                // ignored
        //            }
        //        }
        //    }

        //    var datas = new List<byte[]>();

        //    foreach (var t in temp)
        //    {
        //        var str = t.Replace("sci2 data: 80A5", "");

        //        var tb = new List<byte>();
        //        for (var i = 0; i < str.Length; i = i + 2)
        //        {
        //            var b = Convert.ToByte(str.Substring(i, 2), 16);
        //            tb.Add(b);
        //        }

        //        datas.Add(tb.ToArray());
        //    }

        //    foreach (var d in datas)
        //    {
        //        var bitsStr = Convert.ToString(d[0], 2).PadLeft(8, '0').ToCharArray().Reverse().ToList();
        //        if (bitsStr[7].ToString() == 0.ToString())
        //        {
        //            var toPrintStr = d.Aggregate(string.Empty, (current, dd) => current + ValueHelper.GetHextStr(dd) + " ");

        //            Console.WriteLine(toPrintStr);
        //        }
        //    }
        //}

        private byte[] GetCrc16(string toCrcStr)
        {
            var toCrc = new List<byte>();

            for (var i = 0; i < toCrcStr.Length; i = i + 2)
            {
                var b = Convert.ToByte(toCrcStr.Substring(i, 2), 16);
                toCrc.Add(b);
            }

            return CalcCrc16(toCrc.ToArray());
        }

        private static byte[] CalcCrc16(byte[] value)
        {
            var crc16Info = new CrcHelper();
            var crc = crc16Info.Crc16CcittFalse;
            crc.InitReg = 0xDEAD;
            crc16Info.CRC_Table_Init(crc);
            var keys = BitConverter.GetBytes(crc16Info.CALC_CRC(crc, value));

            return new[] { keys[1], keys[0] };
        }

        public static byte[] CalcCrc32(byte[] value)
        {
            var crc32Info = new CrcHelper();
            var crc = crc32Info.Crc32;
            crc.InitReg = 0xDEADAFFE;
            crc32Info.CRC_Table_Init(crc);
            var keys = BitConverter.GetBytes(crc32Info.CALC_CRC(crc, value));

            return new[] { keys[3], keys[2], keys[1], keys[0] };
        }

        private byte[] StringToBytes(string str)
        {
            var bs = new List<byte>();
            for (var i = 0; i < str.Length; i = i + 2)
            {
                var b = Convert.ToByte(str.Substring(i, 2), 16);
                bs.Add(b);
            }

            return bs.ToArray();
        }

        #region led state

        [Description("R,LED1错误类型")]
        public string Led1ErrorType = string.Empty;
        [Description("R,LED2错误类型")]
        public string Led2ErrorType = string.Empty;
        [Description("R,LED3错误类型")]
        public string Led3ErrorType = string.Empty;
        [Description("R,LED4错误类型")]
        public string Led4ErrorType = string.Empty;
        [Description("R,LED5错误类型")]
        public string Led5ErrorType = string.Empty;
        [Description("R,LED6错误类型")]
        public string Led6ErrorType = string.Empty;
        [Description("R,LED7错误类型")]
        public string Led7ErrorType = string.Empty;
        [Description("R,LED8错误类型")]
        public string Led8ErrorType = string.Empty;
        [Description("R,LED9错误类型")]
        public string Led9ErrorType = string.Empty;
        [Description("R,LED10错误类型")]
        public string Led10ErrorType = string.Empty;
        [Description("R,LED11错误类型")]
        public string Led11ErrorType = string.Empty;
        [Description("R,LED12错误类型")]
        public string Led12ErrorType = string.Empty;
        [Description("R,LED13错误类型")]
        public string Led13ErrorType = string.Empty;
        [Description("R,LED14错误类型")]
        public string Led14ErrorType = string.Empty;
        [Description("R,LED15错误类型")]
        public string Led15ErrorType = string.Empty;
        [Description("R,LED16错误类型")]
        public string Led16ErrorType = string.Empty;
        [Description("R,LED17错误类型")]
        public string Led17ErrorType = string.Empty;
        [Description("R,LED18错误类型")]
        public string Led18ErrorType = string.Empty;
        [Description("R,LED19错误类型")]
        public string Led19ErrorType = string.Empty;
        [Description("R,LED20错误类型")]
        public string Led20ErrorType = string.Empty;
        [Description("R,LED21错误类型")]
        public string Led21ErrorType = string.Empty;
        [Description("R,LED22错误类型")]
        public string Led22ErrorType = string.Empty;
        [Description("R,LED23错误类型")]
        public string Led23ErrorType = string.Empty;
        [Description("R,LED24错误类型")]
        public string Led24ErrorType = string.Empty;
        [Description("R,LED25错误类型")]
        public string Led25ErrorType = string.Empty;
        [Description("R,LED26错误类型")]
        public string Led26ErrorType = string.Empty;
        [Description("R,LED27错误类型")]
        public string Led27ErrorType = string.Empty;
        [Description("R,LED28错误类型")]
        public string Led28ErrorType = string.Empty;
        [Description("R,LED29错误类型")]
        public string Led29ErrorType = string.Empty;
        [Description("R,LED30错误类型")]
        public string Led30ErrorType = string.Empty;
        [Description("R,LED31错误类型")]
        public string Led31ErrorType = string.Empty;
        [Description("R,LED32错误类型")]
        public string Led32ErrorType = string.Empty;
        [Description("R,LED33错误类型")]
        public string Led33ErrorType = string.Empty;
        [Description("R,LED34错误类型")]
        public string Led34ErrorType = string.Empty;
        [Description("R,LED35错误类型")]
        public string Led35ErrorType = string.Empty;
        [Description("R,LED36错误类型")]
        public string Led36ErrorType = string.Empty;
        [Description("R,LED37错误类型")]
        public string Led37ErrorType = string.Empty;
        [Description("R,LED38错误类型")]
        public string Led38ErrorType = string.Empty;
        [Description("R,LED39错误类型")]
        public string Led39ErrorType = string.Empty;
        [Description("R,LED40错误类型")]
        public string Led40ErrorType = string.Empty;
        [Description("R,LED41错误类型")]
        public string Led41ErrorType = string.Empty;
        [Description("R,LED42错误类型")]
        public string Led42ErrorType = string.Empty;
        [Description("R,LED43错误类型")]
        public string Led43ErrorType = string.Empty;
        [Description("R,LED44错误类型")]
        public string Led44ErrorType = string.Empty;
        [Description("R,LED45错误类型")]
        public string Led45ErrorType = string.Empty;
        [Description("R,LED46错误类型")]
        public string Led46ErrorType = string.Empty;
        [Description("R,LED47错误类型")]
        public string Led47ErrorType = string.Empty;
        [Description("R,LED48错误类型")]
        public string Led48ErrorType = string.Empty;
        [Description("R,LED49错误类型")]
        public string Led49ErrorType = string.Empty;
        [Description("R,LED50错误类型")]
        public string Led50ErrorType = string.Empty;
        [Description("R,LED51错误类型")]
        public string Led51ErrorType = string.Empty;
        [Description("R,LED52错误类型")]
        public string Led52ErrorType = string.Empty;
        [Description("R,LED53错误类型")]
        public string Led53ErrorType = string.Empty;
        [Description("R,LED54错误类型")]
        public string Led54ErrorType = string.Empty;
        [Description("R,LED55错误类型")]
        public string Led55ErrorType = string.Empty;
        [Description("R,LED56错误类型")]
        public string Led56ErrorType = string.Empty;
        [Description("R,LED57错误类型")]
        public string Led57ErrorType = string.Empty;
        [Description("R,LED58错误类型")]
        public string Led58ErrorType = string.Empty;
        [Description("R,LED59错误类型")]
        public string Led59ErrorType = string.Empty;
        [Description("R,LED60错误类型")]
        public string Led60ErrorType = string.Empty;
        [Description("R,LED61错误类型")]
        public string Led61ErrorType = string.Empty;
        [Description("R,LED62错误类型")]
        public string Led62ErrorType = string.Empty;
        [Description("R,LED63错误类型")]
        public string Led63ErrorType = string.Empty;
        [Description("R,LED64错误类型")]
        public string Led64ErrorType = string.Empty;

        [Description("R,LED1位置")]
        public string Led1Position = string.Empty;
        [Description("R,LED2位置")]
        public string Led2Position = string.Empty;
        [Description("R,LED3位置")]
        public string Led3Position = string.Empty;
        [Description("R,LED4位置")]
        public string Led4Position = string.Empty;
        [Description("R,LED5位置")]
        public string Led5Position = string.Empty;
        [Description("R,LED6位置")]
        public string Led6Position = string.Empty;
        [Description("R,LED7位置")]
        public string Led7Position = string.Empty;
        [Description("R,LED8位置")]
        public string Led8Position = string.Empty;
        [Description("R,LED9位置")]
        public string Led9Position = string.Empty;
        [Description("R,LED10位置")]
        public string Led10Position = string.Empty;
        [Description("R,LED11位置")]
        public string Led11Position = string.Empty;
        [Description("R,LED12位置")]
        public string Led12Position = string.Empty;
        [Description("R,LED13位置")]
        public string Led13Position = string.Empty;
        [Description("R,LED14位置")]
        public string Led14Position = string.Empty;
        [Description("R,LED15位置")]
        public string Led15Position = string.Empty;
        [Description("R,LED16位置")]
        public string Led16Position = string.Empty;
        [Description("R,LED17位置")]
        public string Led17Position = string.Empty;
        [Description("R,LED18位置")]
        public string Led18Position = string.Empty;
        [Description("R,LED19位置")]
        public string Led19Position = string.Empty;
        [Description("R,LED20位置")]
        public string Led20Position = string.Empty;
        [Description("R,LED21位置")]
        public string Led21Position = string.Empty;
        [Description("R,LED22位置")]
        public string Led22Position = string.Empty;
        [Description("R,LED23位置")]
        public string Led23Position = string.Empty;
        [Description("R,LED24位置")]
        public string Led24Position = string.Empty;
        [Description("R,LED25位置")]
        public string Led25Position = string.Empty;
        [Description("R,LED26位置")]
        public string Led26Position = string.Empty;
        [Description("R,LED27位置")]
        public string Led27Position = string.Empty;
        [Description("R,LED28位置")]
        public string Led28Position = string.Empty;
        [Description("R,LED29位置")]
        public string Led29Position = string.Empty;
        [Description("R,LED30位置")]
        public string Led30Position = string.Empty;
        [Description("R,LED31位置")]
        public string Led31Position = string.Empty;
        [Description("R,LED32位置")]
        public string Led32Position = string.Empty;
        [Description("R,LED33位置")]
        public string Led33Position = string.Empty;
        [Description("R,LED34位置")]
        public string Led34Position = string.Empty;
        [Description("R,LED35位置")]
        public string Led35Position = string.Empty;
        [Description("R,LED36位置")]
        public string Led36Position = string.Empty;
        [Description("R,LED37位置")]
        public string Led37Position = string.Empty;
        [Description("R,LED38位置")]
        public string Led38Position = string.Empty;
        [Description("R,LED39位置")]
        public string Led39Position = string.Empty;
        [Description("R,LED40位置")]
        public string Led40Position = string.Empty;
        [Description("R,LED41位置")]
        public string Led41Position = string.Empty;
        [Description("R,LED42位置")]
        public string Led42Position = string.Empty;
        [Description("R,LED43位置")]
        public string Led43Position = string.Empty;
        [Description("R,LED44位置")]
        public string Led44Position = string.Empty;
        [Description("R,LED45位置")]
        public string Led45Position = string.Empty;
        [Description("R,LED46位置")]
        public string Led46Position = string.Empty;
        [Description("R,LED47位置")]
        public string Led47Position = string.Empty;
        [Description("R,LED48位置")]
        public string Led48Position = string.Empty;
        [Description("R,LED49位置")]
        public string Led49Position = string.Empty;
        [Description("R,LED50位置")]
        public string Led50Position = string.Empty;
        [Description("R,LED51位置")]
        public string Led51Position = string.Empty;
        [Description("R,LED52位置")]
        public string Led52Position = string.Empty;
        [Description("R,LED53位置")]
        public string Led53Position = string.Empty;
        [Description("R,LED54位置")]
        public string Led54Position = string.Empty;
        [Description("R,LED55位置")]
        public string Led55Position = string.Empty;
        [Description("R,LED56位置")]
        public string Led56Position = string.Empty;
        [Description("R,LED57位置")]
        public string Led57Position = string.Empty;
        [Description("R,LED58位置")]
        public string Led58Position = string.Empty;
        [Description("R,LED59位置")]
        public string Led59Position = string.Empty;
        [Description("R,LED60位置")]
        public string Led60Position = string.Empty;
        [Description("R,LED61位置")]
        public string Led61Position = string.Empty;
        [Description("R,LED62位置")]
        public string Led62Position = string.Empty;
        [Description("R,LED63位置")]
        public string Led63Position = string.Empty;
        [Description("R,LED64位置")]
        public string Led64Position = string.Empty;
        #endregion

        public string RegRead70To7F;
        public string RegRead80To8F;
        public string RegRead90To9F;
        public string RegReadA0ToAf;
        public string RegReadB0ToBf;
        public string RegReadC0ToCf;
        public string RegReadD0ToDf;
        public string RegReadE0ToEf;
        #endregion

        #region 烧写相关

        private MyAsyncSocketClient HikBarcodeScaner { get; set; }
        private readonly EventWaitHandle _recvWaitHandle = new AutoResetEvent(false);
        private bool _isReadingBarcode;
        public bool isGetBar = false;
        public string IsRepeat = string.Empty;
        public void ClearBarcode()
        {
            isGetBar = false;
            OsramBarcode = String.Empty;
        }

        [Description("连接海康扫码枪")]
        public void ConnectHikBarcodeScaner(string ipPort)
        {
            try
            {
                HikBarcodeScaner = new MyAsyncSocketClient();
                HikBarcodeScaner.InitSocket(ipPort.Split(':')[0], int.Parse(ipPort.Split(':')[1]));
                HikBarcodeScaner.OnPushSocketsToTcpClient += tcpClient_OnPushSocketsToTcpClient;
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void tcpClient_OnPushSocketsToTcpClient(BaseSocket sockets)
        {
            if (sockets == null || sockets.RecBuffer == null)
                return;

            if (isGetBar)
                return;

            if (sockets.Offset == 0)
                return;

            if (sockets.Offset != 10)
                return;

            var buffer = new byte[sockets.Offset];
            Array.Copy(sockets.RecBuffer, 0, buffer, 0, sockets.Offset);

            OsramBarcode = Encoding.ASCII.GetString(buffer);
            isGetBar = true;
            _recvWaitHandle.Set();
        }

        [Description("向海康扫码枪发送读码指令")]
        public void ReadBarcode(string cmd)
        {
            OsramBarcode = string.Empty;

            if (HikBarcodeScaner != null)
            {
                _isReadingBarcode = true;
                var bs = Encoding.ASCII.GetBytes("T");
                HikBarcodeScaner.SendData(bs);
                _recvWaitHandle.WaitOne(1000);
                _isReadingBarcode = false;
            }
        }

        public void CheckRepeatByLocaDataBase()
        {
            IsRepeat = "NG";
            IsRepeat =  LocalDbHelper.IsRepeat(OsramBarcode) ? "OK" : "NG";
        }
        
        [Description("R/W,欧司朗二维码内容")]
        public string OsramBarcode = string.Empty;//"0P7N2C0404";

        public string OsramBinFilePath = string.Empty;

        [Description("W,工位1产品二维码")]
        public string ProductBarcode1 = string.Empty;
        [Description("R,获取工位2产品二维码")]
        public string ProductBarcode2 = String.Empty;

        [Description("R,BIN读取全部内容")]
        public string BinStrFromOsramFile = string.Empty;
        [Description("R,BIN读取DMC")]
        public string DmcFromOsramBinFile = string.Empty;
        [Description("R,BIN读取DeviceId")]
        public string DeviceIdFromOsramBinFile = string.Empty;
        [Description("R,根据这个二维码找对应的BIN文件")]
        public string OsramBarcodes = string.Empty;
        [Description("R,找到的BIN文件")]
        public string OsramFile = string.Empty;

        [Description("根据产品1二维码获取产品2二维码")]
        public void GetProductBarcode2ByPlms()
        {
            var table = @"SyProductionCheckDataHistory";
            ProductBarcode2 = string.Empty;
            if (ProductBarcode1.Length < 10)
                return;
            var sqlString = $"SELECT [SubMaterialsInfo] FROM [PLMS].[dbo].[{table}] where SubMaterialsInfo like '%{ProductBarcode1.Trim()}%' AND isdelete = 0 and ProcessName = 'AOI检查'";
            if (!GetBar(sqlString))
            {
                table = @"SyProductionCheckData";
                sqlString = $"SELECT [SubMaterialsInfo] FROM [PLMS].[dbo].[{table}] where SubMaterialsInfo like '%{ProductBarcode1.Trim()}%' AND isdelete = 0 and ProcessName = 'AOI检查'";
                GetBar(sqlString);
            }
        }

        private bool GetBar(string strSQl)
        {
            var connectionString = @"Server=192.168.0.138;DataBase=PLMS;uid=ipms;pwd=Scae2020#";
            var ds = new DataSet();
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var command = new SqlDataAdapter(strSQl, connection);
                    command.Fill(ds, "ds");
                }
                catch (SqlException)
                {
                    return false;
                }
            }

            if (ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0) return false;
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                if (!item[0].ToString().Contains("|"))
                    continue;
                var tempBarcode = item[0].ToString().Split('|');
                ProductBarcode2 = ProductBarcode1.Trim().Equals(tempBarcode[0]) ? tempBarcode[1] : tempBarcode[0];
                return true;
            }

            return false;
        }

        [Description("根据二维码读取对应BIN文件")]
        public void ReadBinByBarcode()
        {
            OsramBinFilePath = string.Empty;
            OsramFile = string.Empty;
            BinStrFromOsramFile = string.Empty;
            DmcFromOsramBinFile = string.Empty;
            DeviceIdFromOsramBinFile = string.Empty;

            if (!string.IsNullOrEmpty(OsramBarcodes))
            {
                var dir = string.Format(@"{0}\{1}", Directory.GetCurrentDirectory(), "OsramConvertBinFiles");
                var getFiles = Directory.GetFiles(dir).ToList();

                var file = getFiles.Find(f => f.EndsWith(OsramBarcodes + @".bin"));
                OsramFile = Path.GetFileName(file);
                if (!string.IsNullOrEmpty(file))
                {
                    OsramBinFilePath = file;
                    var binData = BinFileHelper.GetBinData(file);

                    var data0006000 = binData.Find(f => f.Address == 0x00060000);
                    var data0006010 = binData.Find(f => f.Address == 0x00060010);
                    var data0006020 = binData.Find(f => f.Address == 0x00060020);

                    if (data0006000 != null && data0006010 != null && data0006020 != null &&
                        data0006000.Data.Count == 16 && data0006010.Data.Count == 16 && data0006020.Data.Count == 16)
                    {
                        var deviceIdBytes = new[] { data0006000.Data[10], data0006000.Data[11], data0006000.Data[12], data0006000.Data[13] };
                        Array.Reverse(deviceIdBytes);
                        DeviceIdFromOsramBinFile = "OK " + ValueHelper.GetHextStr(deviceIdBytes).Replace(" ", "");

                        var dmcBytes = new[]
                        {
                            data0006000.Data[14], data0006000.Data[15], data0006010.Data[0], data0006010.Data[1],
                            data0006010.Data[2], data0006010.Data[3], data0006010.Data[4], data0006010.Data[5],
                            data0006010.Data[6], data0006010.Data[7]
                        };
                        Array.Reverse(dmcBytes);
                        DmcFromOsramBinFile = "OK " + Encoding.ASCII.GetString(dmcBytes);
                    }

                    //foreach (var str in from binLine in binData
                    //                    let data = ValueHelper.GetHextStr(binLine.Data.ToArray()).Replace(" ", "")
                    //                    select string.Format("{0}:{1}\r\n", binLine.Address.ToString("x8"), data))
                    //    BinStr += str;
                }
                else
                {
                    DeviceIdFromOsramBinFile = "NG 文件不存在";
                    DmcFromOsramBinFile = "NG 文件不存在";
                }
            }

            OsramBarcodes = string.Empty;
        }

        [Description("R,BIN读取全部内容")]
        public string BinStrFromProgramRead = string.Empty;
        [Description("R,BIN读取DMC")]
        public string DmcFromProgramRead = string.Empty;
        [Description("R,BIN读取DeviceId")]
        public string DeviceIdFromProgramRead = string.Empty;

        [Description("R,反读flash与源文件-比对device和dmc")]
        public string CompareDeviceIdAndDmcResult = string.Empty;

        public string ReadBinFilePath = string.Empty;

        [Description("从反读的BIN文件读取内容")]
        public void ReadBinFromProgramReadBinFile()
        {
            BinStrFromProgramRead = string.Empty;
            DmcFromProgramRead = string.Empty;
            DeviceIdFromProgramRead = string.Empty;

            #region 20250109 反读烧录信息

            CR = string.Empty;
            DTR = string.Empty;
            TSTIM = string.Empty;
            RATO = string.Empty;
            MB = string.Empty;
            DLSET = string.Empty;
            NVMCR = string.Empty;
            CURR = string.Empty;
            HWSET = string.Empty;
            FWCT = string.Empty;
            FRT = string.Empty;
            EGSET = string.Empty;
            TSTDR = string.Empty;
            PWMR = string.Empty;
            EVDDPT = string.Empty;
            TSPDR = string.Empty;
            FSTXR = string.Empty;
            ETEMPT = string.Empty;
            MBDR = string.Empty;
            FSTYR = string.Empty;
            NTB = string.Empty;

            #endregion

            CompareDeviceIdAndDmcResult = string.Empty;

            if (File.Exists(ReadBinFilePath))
            {
                var binData = BinFileHelper.GetBinData(ReadBinFilePath);

                var data0006000 = binData.Find(f => f.Address == 0x00060000);
                var data0006010 = binData.Find(f => f.Address == 0x00060010);
                var data0006020 = binData.Find(f => f.Address == 0x00060020);

                if (data0006000 != null && data0006010 != null && data0006020 != null &&
                    data0006000.Data.Count == 16 && data0006010.Data.Count == 16 && data0006020.Data.Count == 16)
                {
                    var deviceIdBytes = new[] { data0006000.Data[10], data0006000.Data[11], data0006000.Data[12], data0006000.Data[13] };
                    Array.Reverse(deviceIdBytes);
                    DeviceIdFromProgramRead = "OK " + ValueHelper.GetHextStr(deviceIdBytes).Replace(" ", "");

                    var dmcBytes = new[]
                        {
                            data0006000.Data[14], data0006000.Data[15], data0006010.Data[0], data0006010.Data[1],
                            data0006010.Data[2], data0006010.Data[3], data0006010.Data[4], data0006010.Data[5],
                            data0006010.Data[6], data0006010.Data[7]
                        };
                    Array.Reverse(dmcBytes);
                    DmcFromProgramRead = "OK " + Encoding.ASCII.GetString(dmcBytes);

                    if (DmcFromOsramBinFile.StartsWith("OK") && DeviceIdFromOsramBinFile.StartsWith("OK"))
                    {
                        var readFlashDmc = DmcFromProgramRead.Replace("OK ", "");
                        var readFlashDevice = DeviceIdFromProgramRead.Replace("OK ", "");

                        var readOsramBinDmc = DmcFromOsramBinFile.Replace("OK ", "");
                        var readOsramBinFlashDevice = DeviceIdFromOsramBinFile.Replace("OK ", "");

                        if (readFlashDmc == readOsramBinDmc && readFlashDevice == readOsramBinFlashDevice)
                        {
                            CompareDeviceIdAndDmcResult = "OK";
                        }
                        else
                        {
                            CompareDeviceIdAndDmcResult = "NG";
                        }
                    }
                    else
                    {
                        CompareDeviceIdAndDmcResult = "NG";
                    }
                }

                #region 20250109 反读烧录信息

                var data00000000 = binData.Find(f => f.Address == 0x00000000);
                var data00000010 = binData.Find(f => f.Address == 0x00000010);
                var data00000020 = binData.Find(f => f.Address == 0x00000020);
                var data00000030 = binData.Find(f => f.Address == 0x00000030);
                var data00000040 = binData.Find(f => f.Address == 0x00000040);
                var data00000050 = binData.Find(f => f.Address == 0x00000050);
                var data000001F0 = binData.Find(f => f.Address == 0x000001F0);

                if (data00000000 != null && data00000010 != null && data00000020 != null && data00000030 != null && data00000040 != null && data00000050 != null && data000001F0 != null &&
                    data00000000.Data.Count == 16 && data00000010.Data.Count == 16 && data00000020.Data.Count == 16 && data00000030.Data.Count == 16 && data00000040.Data.Count == 16 && data00000050.Data.Count == 16 && data000001F0.Data.Count == 16)
                {
                    var listValue = new List<byte>();
                    listValue.AddRange(data00000000.Data);
                    listValue.AddRange(data00000010.Data);
                    listValue.AddRange(data00000020.Data);
                    listValue.AddRange(data00000030.Data);
                    listValue.AddRange(data00000040.Data);
                    listValue.AddRange(data00000050.Data);

                    CR = ValueHelper.GetHextStr(listValue[2 * 0 + 1]) + ValueHelper.GetHextStr(listValue[2 * 0]);
                    RATO = ValueHelper.GetHextStr(listValue[2 * 4 + 1]) + ValueHelper.GetHextStr(listValue[2 * 4]);
                    NVMCR = ValueHelper.GetHextStr(listValue[2 * 0x0C + 1]) + ValueHelper.GetHextStr(listValue[2 * 0x0C]);
                    FWCT = ValueHelper.GetHextStr(listValue[2 * 0x0F + 1]) + ValueHelper.GetHextStr(listValue[2 * 0x0F]);
                    TSTDR = ValueHelper.GetHextStr(listValue[2 * 0x15 + 1]) + ValueHelper.GetHextStr(listValue[2 * 0x15]);
                    TSPDR = ValueHelper.GetHextStr(listValue[2 * 0x16 + 1]) + ValueHelper.GetHextStr(listValue[2 * 0x16]);
                    MBDR = ValueHelper.GetHextStr(listValue[2 * 0x17 + 1]) + ValueHelper.GetHextStr(listValue[2 * 0x17]);
                    DTR = ValueHelper.GetHextStr(listValue[2 * 0x18 + 1]) + ValueHelper.GetHextStr(listValue[2 * 0x18]);
                    MB = ValueHelper.GetHextStr(listValue[2 * 0x1A + 1]) + ValueHelper.GetHextStr(listValue[2 * 0x1A]);
                    CURR = ValueHelper.GetHextStr(listValue[2 * 0x1C + 1]) + ValueHelper.GetHextStr(listValue[2 * 0x1C]);
                    FRT = ValueHelper.GetHextStr(listValue[2 * 0x1E + 1]) + ValueHelper.GetHextStr(listValue[2 * 0x1E]);
                    PWMR = ValueHelper.GetHextStr(listValue[2 * 0x1F + 1]) + ValueHelper.GetHextStr(listValue[2 * 0x1F]);
                    FSTXR = ValueHelper.GetHextStr(listValue[2 * 0x20 + 1]) + ValueHelper.GetHextStr(listValue[2 * 0x20]);
                    FSTYR = ValueHelper.GetHextStr(listValue[2 * 0x21 + 1]) + ValueHelper.GetHextStr(listValue[2 * 0x21]);
                    TSTIM = ValueHelper.GetHextStr(listValue[2 * 0x22 + 1]) + ValueHelper.GetHextStr(listValue[2 * 0x22]);
                    DLSET = ValueHelper.GetHextStr(listValue[2 * 0x23 + 1]) + ValueHelper.GetHextStr(listValue[2 * 0x23]);

                    HWSET = ValueHelper.GetHextStr(listValue[2 * 0x26 + 1]) + ValueHelper.GetHextStr(listValue[2 * 0x26]);
                    EGSET = ValueHelper.GetHextStr(listValue[2 * 0x2C + 1]) + ValueHelper.GetHextStr(listValue[2 * 0x2C]);
                    EVDDPT = ValueHelper.GetHextStr(listValue[2 * 0x2E + 1]) + ValueHelper.GetHextStr(listValue[2 * 0x2E]);
                    ETEMPT = ValueHelper.GetHextStr(listValue[2 * 0x2F + 1]) + ValueHelper.GetHextStr(listValue[2 * 0x2F]);

                    NTB = ValueHelper.GetHextStr(data000001F0.Data[15]) + ValueHelper.GetHextStr(data000001F0.Data[14]);
                }

                #endregion

                //foreach (var str in from binLine in binData
                //                    let data = ValueHelper.GetHextStr(binLine.Data.ToArray()).Replace(" ", "")
                //                    select string.Format("{0}:{1}\r\n", binLine.Address.ToString("x8"), data))
                //    BinStrFromProgramRead += str;
            }
        }

        [Description("从反读的BIN文件读取内容-偏移地址从0000开始")]
        public void ReadBinFromProgramReadBinFileChange0103()
        {
            BinStrFromProgramRead = string.Empty;
            DmcFromProgramRead = string.Empty;
            DeviceIdFromProgramRead = string.Empty;

            CompareDeviceIdAndDmcResult = string.Empty;

            if (File.Exists(ReadBinFilePath))
            {
                var binData = BinFileHelper.GetBinData(ReadBinFilePath);

                var data0006000 = binData.Find(f => f.Address == 0x00000000);
                var data0006010 = binData.Find(f => f.Address == 0x00000010);
                var data0006020 = binData.Find(f => f.Address == 0x00000020);

                if (data0006000 != null && data0006010 != null && data0006020 != null &&
                    data0006000.Data.Count == 16 && data0006010.Data.Count == 16)
                {
                    var deviceIdBytes = new[] { data0006000.Data[10], data0006000.Data[11], data0006000.Data[12], data0006000.Data[13] };
                    Array.Reverse(deviceIdBytes);
                    DeviceIdFromProgramRead = "OK " + ValueHelper.GetHextStr(deviceIdBytes).Replace(" ", "");

                    var dmcBytes = new[]
                        {
                            data0006000.Data[14], data0006000.Data[15], data0006010.Data[0], data0006010.Data[1],
                            data0006010.Data[2], data0006010.Data[3], data0006010.Data[4], data0006010.Data[5],
                            data0006010.Data[6], data0006010.Data[7]
                        };
                    Array.Reverse(dmcBytes);
                    DmcFromProgramRead = "OK " + Encoding.ASCII.GetString(dmcBytes);

                    if (DmcFromOsramBinFile.StartsWith("OK") && DeviceIdFromOsramBinFile.StartsWith("OK"))
                    {
                        var readFlashDmc = DmcFromProgramRead.Replace("OK ", "");
                        var readFlashDevice = DeviceIdFromProgramRead.Replace("OK ", "");

                        var readOsramBinDmc = DmcFromOsramBinFile.Replace("OK ", "");
                        var readOsramBinFlashDevice = DeviceIdFromOsramBinFile.Replace("OK ", "");

                        if (readFlashDmc == readOsramBinDmc && readFlashDevice == readOsramBinFlashDevice)
                        {
                            CompareDeviceIdAndDmcResult = "OK";
                        }
                        else
                        {
                            CompareDeviceIdAndDmcResult = "NG";
                        }
                    }
                    else
                    {
                        CompareDeviceIdAndDmcResult = "NG";
                    }
                }

                //foreach (var str in from binLine in binData
                //                    let data = ValueHelper.GetHextStr(binLine.Data.ToArray()).Replace(" ", "")
                //                    select string.Format("{0}:{1}\r\n", binLine.Address.ToString("x8"), data))
                //    BinStrFromProgramRead += str;
            }
        }

        #endregion

        #region 20250107 远程目录访问获取bin

        [Description("R/W,bin文件的远程目录")]
        public string RemoteBinFilePath = string.Empty;
        [Description("R/W,bin文件的远程目录用户名")]
        public string RemoteBinFileUserName = string.Empty;
        [Description("R/W,bin文件的远程目录密码")]
        public string RemoteBinFilePassword = string.Empty;

        [Description("根据二维码读取对应BIN文件")]
        public void ReadRemoteBinByBarcode()
        {
            OsramBinFilePath = string.Empty;
            OsramFile = string.Empty;
            BinStrFromOsramFile = string.Empty;
            DmcFromOsramBinFile = string.Empty;
            DeviceIdFromOsramBinFile = string.Empty;

            if (!string.IsNullOrEmpty(OsramBarcodes) && !string.IsNullOrEmpty(RemoteBinFilePath))
            {
                var getFiles = RemoteFileHelper.GetRemoteFileList(RemoteBinFilePath,
                    RemoteBinFileUserName, RemoteBinFilePassword);

                var file = getFiles.Find(f => f.Name.EndsWith(OsramBarcodes + @".bin"));
                if (file != null && !string.IsNullOrEmpty(file.Name))
                {
                    OsramFile = Path.GetFileName(file.Name);
                    OsramBinFilePath = file.FullName;
                    var binData = BinFileHelper.GetBinData(file.FullName);

                    var data0006000 = binData.Find(f => f.Address == 0x00060000);
                    var data0006010 = binData.Find(f => f.Address == 0x00060010);
                    var data0006020 = binData.Find(f => f.Address == 0x00060020);

                    if (data0006000 != null && data0006010 != null && data0006020 != null &&
                        data0006000.Data.Count == 16 && data0006010.Data.Count == 16 && data0006020.Data.Count == 16)
                    {
                        var deviceIdBytes = new[] { data0006000.Data[10], data0006000.Data[11], data0006000.Data[12], data0006000.Data[13] };
                        Array.Reverse(deviceIdBytes);
                        DeviceIdFromOsramBinFile = "OK " + ValueHelper.GetHextStr(deviceIdBytes).Replace(" ", "");

                        var dmcBytes = new[]
                        {
                            data0006000.Data[14], data0006000.Data[15], data0006010.Data[0], data0006010.Data[1],
                            data0006010.Data[2], data0006010.Data[3], data0006010.Data[4], data0006010.Data[5],
                            data0006010.Data[6], data0006010.Data[7]
                        };
                        Array.Reverse(dmcBytes);
                        DmcFromOsramBinFile = "OK " + Encoding.ASCII.GetString(dmcBytes);
                    }
                }
                else
                {
                    DeviceIdFromOsramBinFile = "NG 文件不存在";
                    DmcFromOsramBinFile = "NG 文件不存在";
                }
            }

            OsramBarcodes = string.Empty;
        }

        #endregion"

        #region 读配置寄存器相关

        [Description("R,CR")] public string CR = string.Empty;
        [Description("R,DTR")] public string DTR = string.Empty;
        [Description("R,TSTIM")] public string TSTIM = string.Empty;
        [Description("R,RATO")] public string RATO = string.Empty;
        [Description("R,MB")] public string MB = string.Empty;
        [Description("R,DLSET")] public string DLSET = string.Empty;
        [Description("R,NVMCR")] public string NVMCR = string.Empty;
        [Description("R,CURR")] public string CURR = string.Empty;
        [Description("R,HWSET")] public string HWSET = string.Empty;
        [Description("R,FWCT")] public string FWCT = string.Empty;
        [Description("R,FRT")] public string FRT = string.Empty;
        [Description("R,EGSET")] public string EGSET = string.Empty;
        [Description("R,TSTDR")] public string TSTDR = string.Empty;
        [Description("R,PWMR")] public string PWMR = string.Empty;
        [Description("R,EVDDPT")] public string EVDDPT = string.Empty;
        [Description("R,TSPDR")] public string TSPDR = string.Empty;
        [Description("R,FSTXR")] public string FSTXR = string.Empty;
        [Description("R,ETEMPT")] public string ETEMPT = string.Empty;
        [Description("R,MBDR")] public string MBDR = string.Empty;
        [Description("R,FSTYR")] public string FSTYR = string.Empty;
        [Description("R,NTB")] public string NTB = string.Empty;

        private bool _isReadConfigRegisters;
        private readonly List<string> _readConfigRegistersBuff = new List<string>();
        //private readonly object _lockReadConfigRegisters = new object();

        [Description("读配置寄存器")]
        public void ReadConfigRegisters()
        {
            CR = string.Empty;
            DTR = string.Empty;
            TSTIM = string.Empty;
            RATO = string.Empty;
            MB = string.Empty;
            DLSET = string.Empty;
            NVMCR = string.Empty;
            CURR = string.Empty;
            HWSET = string.Empty;
            FWCT = string.Empty;
            FRT = string.Empty;
            EGSET = string.Empty;
            TSTDR = string.Empty;
            PWMR = string.Empty;
            EVDDPT = string.Empty;
            TSPDR = string.Empty;
            FSTXR = string.Empty;
            ETEMPT = string.Empty;
            MBDR = string.Empty;
            FSTYR = string.Empty;
            NTB = string.Empty;

            _readConfigRegistersBuff.Clear();
            _isReadConfigRegisters = true;
            Thread.Sleep(2000);
            _isReadConfigRegisters = false;
            Thread.Sleep(250);

            // debug
            //_readConfigRegistersBuff.Add("80A5BE0070FDFFFF00020000FFFFFFFF3000002000000000000000A50001FFFF0000003C2013");
            //_readConfigRegistersBuff.Add("80A5BE10000000000000000000000064008C003F008C010000FFFFFF3003FFFFFFFF01C295C3");
            //_readConfigRegistersBuff.Add("80A5BE2000000000000B000EFFFFFFFF300DFFFFFFFFFFFFBEC4CB3300000000D2010064450A");
            //_readConfigRegistersBuff.Add("80A5BEF0FFFFFFFFFFFFFFFFFFFFFFFFFFFF810081000000000080A3FACCFFFF2402ABCD7B4A");

            var find00Index = _readConfigRegistersBuff.FindIndex(f => f.StartsWith("80A5BE00"));
            if (find00Index != -1)
            {
                var result = GetHexValue(_readConfigRegistersBuff[find00Index]);
                if (result.Count == 16)
                {
                    CR = result[0];
                    RATO = result[4];
                    NVMCR = result[12];
                    FWCT = result[15];
                }
            }

            var find10Index = _readConfigRegistersBuff.FindIndex(f => f.StartsWith("80A5BE10"));
            if (find10Index != -1)
            {
                var result = GetHexValue(_readConfigRegistersBuff[find10Index]);
                if (result.Count == 16)
                {
                    TSTDR = result[5];
                    TSPDR = result[6];
                    MBDR = result[7];
                    DTR = result[8];
                    MB = result[10];
                    CURR = result[12];
                    FRT = result[14];
                    PWMR = result[15];
                }
            }

            var find20Index = _readConfigRegistersBuff.FindIndex(f => f.StartsWith("80A5BE20"));
            if (find20Index != -1)
            {
                var result = GetHexValue(_readConfigRegistersBuff[find20Index]);
                if (result.Count == 16)
                {
                    FSTXR = result[0];
                    FSTYR = result[1];
                    TSTIM = result[2];
                    DLSET = result[3];
                    HWSET = result[6];
                    EGSET = result[12];
                    EVDDPT = result[14];
                    ETEMPT = result[15];
                }
            }

            var findF0Index = _readConfigRegistersBuff.FindIndex(f => f.StartsWith("80A5BEF0"));
            if (findF0Index != -1)
            {
                var result = GetHexValue(_readConfigRegistersBuff[findF0Index]);
                if (result.Count == 16)
                {
                    NTB = result[15];
                }
            }
        }

        private static List<string> GetHexValue(string str)
        {
            var result = new List<string>();

            var strValue = str.Substring(8, 64);

            for (var i = 0; i < strValue.Length; i = i + 4)
                result.Add(strValue.Substring(i, 4));

            return result;
        }

        #endregion

        #region 2024/12/27 修改读新增坏点策略

        public void ClearReadErrorHistory()
        {

        }

        public string ErrorPositionAnalysisResult = string.Empty;

        public void ReadErrorAndAnalysis(int maxCount)
        {
            ErrorPositionAnalysisResult = string.Empty;

            if (maxCount < 5)
            {
                ErrorPositionAnalysisResult = "NG 判断参数不正确";
                return;
            }

            RegRead70To7F = string.Empty;
            RegRead80To8F = string.Empty;
            RegRead90To9F = string.Empty;
            RegReadA0ToAf = string.Empty;
            RegReadB0ToBf = string.Empty;
            RegReadC0ToCf = string.Empty;
            RegReadD0ToDf = string.Empty;
            RegReadE0ToEf = string.Empty;

            for (var i = 1; i < 65; i++)
            {
                {
                    var fieldName = string.Format("Led{0}Position", i);
                    var field = GetType().GetField(fieldName);
                    if (field != null)
                        field.SetValue(this, string.Empty);
                }

                {
                    var fieldName = string.Format("Led{0}ErrorType", i);
                    var field = GetType().GetField(fieldName);
                    if (field != null)
                        field.SetValue(this, string.Empty);
                }
            }

            _readBuffer.Clear();

            _isRead = true;
            Thread.Sleep(2000);
            _isRead = false;

            var readErrorMsg = string.Empty;

            if (string.IsNullOrEmpty(RegRead70To7F))
                readErrorMsg += "ELEDERP0~ELEDERP15 No Read;";
            else if (RegRead70To7F.Length != 64)
                readErrorMsg += "ELEDERP0~ELEDERP15 Length Error;";

            if (string.IsNullOrEmpty(RegRead80To8F))
                readErrorMsg += "ELEDERP16~ELEDERP31 No Read;";
            else if (RegRead80To8F.Length != 64)
                readErrorMsg += "ELEDERP16~ELEDERP31 Length Error;";

            if (string.IsNullOrEmpty(RegRead90To9F))
                readErrorMsg += "ELEDERP32~ELEDERP47 No Read;";
            else if (RegRead90To9F.Length != 64)
                readErrorMsg += "ELEDERP32~ELEDERP47 Length Error;";

            if (string.IsNullOrEmpty(RegReadA0ToAf))
                readErrorMsg += "ELEDERP48~ELEDERP63 No Read;";
            else if (RegReadA0ToAf.Length != 64)
                readErrorMsg += "ELEDERP48~ELEDERP63 Length Error;";

            if (string.IsNullOrEmpty(RegReadB0ToBf))
                readErrorMsg += "ELEDERS0~ELEDERS15 No Read;";
            else if (RegReadB0ToBf.Length != 64)
                readErrorMsg += "ELEDERS0~ELEDERS15 Length Error;";

            if (string.IsNullOrEmpty(RegReadC0ToCf))
                readErrorMsg += "ELEDERS16~ELEDERS31 No Read;";
            else if (RegReadC0ToCf.Length != 64)
                readErrorMsg += "ELEDERS16~ELEDERS31 Length Error;";

            if (string.IsNullOrEmpty(RegReadD0ToDf))
                readErrorMsg += "ELEDERS32~ELEDERS47 No Read;";
            else if (RegReadD0ToDf.Length != 64)
                readErrorMsg += "ELEDERS32~ELEDERS47 Length Error;";

            if (string.IsNullOrEmpty(RegReadE0ToEf))
                readErrorMsg += "ELEDERS48~ELEDERS63 No Read;";
            else if (RegReadE0ToEf.Length != 64)
                readErrorMsg += "ELEDERS48~ELEDERS63 Length Error;";

            if (!string.IsNullOrEmpty(readErrorMsg))
            {
                ErrorPositionAnalysisResult = "NG " + readErrorMsg;
                return;
            }

            // RegRead70To7F~RegReadB0ToBf
            // RegRead80To8F~RegReadC0ToCf
            // RegRead90To9F~RegReadD0ToDf
            // RegReadA0ToAf~RegReadE0ToEf

            // 1. 如果过新增点位数量>5个，则判定不良
            // 2. 如果新增点位数量≤5个，但是存在横或纵向的点位能连续形成长度>=3个像素的直线，则判定不良。

            var newStrPos = RegRead70To7F + RegRead80To8F + RegRead90To9F + RegReadA0ToAf;
            var newStrError = RegReadB0ToBf + RegReadC0ToCf + RegReadD0ToDf + RegReadE0ToEf;
            var listError = new List<LedErrorPosition>();
            for (var i = 0; i < 64; i++)
            {
                var ledErrorPos = new LedErrorPosition { ErrorIndex = i };

                var posStr = newStrPos.Substring(i * 4, 4);
                var errorStr = newStrError.Substring(i * 4, 4);
                ledErrorPos.DataFormat(posStr, errorStr);

                listError.Add(ledErrorPos);
            }

            var findError = listError.FindAll(f => f.LedError != "NoDefect");
            var showStr = JsonConvert.SerializeObject(findError);

            if (findError.Count == 0)
            {
                ErrorPositionAnalysisResult = "DetectionPass";
                return;
            }

            if (findError.Count > 0 && findError.Count < 3)
            {
                ErrorPositionAnalysisResult = "DetectionPass " + showStr;
                return;
            }

            if (findError.Count > maxCount)
            {
                ErrorPositionAnalysisResult = "NG " + showStr;
                return;
            }

            string CheckConsecutiveIncreaseMsg;
            var toCheckPoint = findError.Select(t => t.LedPosition).ToList();
            if (CheckConsecutiveIncrease(toCheckPoint, out CheckConsecutiveIncreaseMsg))
            {
                ErrorPositionAnalysisResult = "NG " + showStr + " " + CheckConsecutiveIncreaseMsg;
                return;
            }

            ErrorPositionAnalysisResult = "DetectionPass " + showStr;
        }

        private static bool CheckConsecutiveIncrease(List<Point> points, out string msg)
        {
            msg = string.Empty;

            if (points.Count < 3)
            {
                return false;
            }

            // 检查横坐标连续增长
            for (int i = 0; i < points.Count - 2; i++)
            {
                if (points[i].X + 1 == points[i + 1].X && points[i + 1].X + 1 == points[i + 2].X)
                {
                    Console.WriteLine($"连续增长点：{points[i]}, {points[i + 1]}, {points[i + 2]}");
                    msg = $"连续增长点：{points[i]}, {points[i + 1]}, {points[i + 2]}";
                    return true;
                }
            }

            // 检查纵坐标连续增长
            for (int i = 0; i < points.Count - 2; i++)
            {
                if (points[i].Y + 1 == points[i + 1].Y && points[i + 1].Y + 1 == points[i + 2].Y)
                {
                    Console.WriteLine($"连续增长点：{points[i]}, {points[i + 1]}, {points[i + 2]}");
                    msg = $"连续增长点：{points[i]}, {points[i + 1]}, {points[i + 2]}";
                    return true;
                }
            }

            return false;
        }
    }

    internal class LedErrorPosition
    {
        public int ErrorIndex;
        public Point LedPosition;
        public string PixelState;
        public string LedError;

        public void DataFormat(string posData, string errorData)
        {
            var xy = Convert.ToUInt16(posData, 16);
            var y = (byte)(xy >> 8);
            var x = (byte)(xy & 0xFF);
            LedPosition = new Point(x, y);

            var error = Convert.ToString(Convert.ToUInt16(errorData, 16), 2).PadLeft(16, '0');
            var state = error.Substring(13, 1);
            var pxdiag = error.Substring(14, 2);

            PixelState = state == "0" ? "LedOff" : "LedOn";

            if (pxdiag == "00")
                LedError = "NoDefect";
            else if (pxdiag == "01")
                LedError = "S2G";
            else if (pxdiag == "10")
                LedError = "OPEN";
            else if (pxdiag == "11")
                LedError = "NotUsed";
        }
    }

    #endregion
}