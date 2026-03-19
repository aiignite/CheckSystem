using CommonUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Threading;

namespace Controller
{
    public sealed class HopooColorOhs350XUsb : ControllerBase
    {
        [Description("R,照度/lx")] public float Lux = -9999;
        [Description("R,色温/K")] public float Color = -9999;
        [Description("R,色坐标/X")] public float CoordinateX = -9999;
        [Description("R,色坐标/Y")] public float CoordinateY = -9999;

        public HopooColorOhs350XUsb(string name)
            : base(name) { }

        private SerialPort _serialPort;

        public void ConnectSerialPort(string portName)
        {
            try
            {
                _serialPort = new SerialPort(portName, 115200);
                _serialPort.Open();
            }
            catch (Exception e)
            {
                _serialPort = null;
            }
        }

        [Description("通过自动积分时间启动测试")]
        public void StartTestByAutoIntegralTime()
        {
            Lux = -9999;
            Color = -9999;
            CoordinateX = -9999;
            CoordinateY = -9999;

            try
            {
                _serialPort.Close();
                _serialPort.Open();
            }
            catch (Exception e)
            {
                // ignored
            }

            var readAction = new Action(() =>
            {
                byte[] value;
                if (TryReadData(Func.Connect, out value))
                {
                    if (value.Length == 16)
                    {
                        Thread.Sleep(50);
                        value = null;
                        if (TryReadData(Func.SetAutoIntegralTime, out value))
                        {
                            Thread.Sleep(50);
                            value = null;
                            if (TryReadData(Func.StartTestOnce, out value))
                            {
                                if (ValueHelper.GetHextStr(value) == "25 0E")
                                {
                                    var isTestEnd = false;
                                    while (true)
                                    {
                                        Thread.Sleep(100);
                                        value = null;
                                        if (TryReadData(Func.ReadTestState, out value))
                                        {
                                            if (value.Length == 9)
                                            {
                                                var state = value[3];
                                                if (state == 0x01)
                                                {
                                                    isTestEnd = true;
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                //UpdateTextBox("读取采样状态失败");
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            //UpdateTextBox("读取采样状态失败，无响应");
                                            break;
                                        }
                                    }

                                    Thread.Sleep(50);
                                    value = null;
                                    TryReadData(Func.StopTest, out value);

                                    if (isTestEnd)
                                    {
                                        Thread.Sleep(50);
                                        value = null;
                                        if (TryReadData(Func.ReadIntegralTime, out value))
                                        {
                                            if (value.Length == 7)
                                            {
                                                var time = BitConverter.ToSingle(
                                                    new byte[] { value[5], value[4], value[3], value[2] }, 0) * 1000;
                                                var type = value[6] > 0 ? "自动" : "锁定";
                                                //UpdateTextBox(string.Format("积分时间={0}/ms,积分方式={1}", time, type));

                                                Thread.Sleep(50);
                                                value = null;
                                                if (TryReadData(Func.ReadTestResult, out value))
                                                {
                                                    if (value.Length == 3856)
                                                    {
                                                        var lxBytes = new byte[]
                                                        {
                                                            value[36 + 4], value[37 + 4], value[38 + 4], value[39 + 4]
                                                        };
                                                        var colorBytes = new byte[]
                                                        {
                                                            value[44 + 4], value[45 + 4], value[46 + 4], value[47 + 4]
                                                        };

                                                        var lx = Math.Round(BitConverter.ToSingle(lxBytes, 0), 1,
                                                            MidpointRounding.AwayFromZero);
                                                        var color = Math.Round(BitConverter.ToSingle(colorBytes, 0), 0,
                                                            MidpointRounding.AwayFromZero);

                                                        Lux = (float)lx;
                                                        Color = (float)color;

                                                        var x = new byte[]
                                                        {
                                                            value[52 + 4], value[53 + 4], value[54 + 4], value[55 + 4]
                                                        };
                                                        var y = new byte[]
                                                        {
                                                            value[56 + 4], value[57 + 4], value[58 + 4], value[59 + 4]
                                                        };

                                                        var coorX = Math.Round(BitConverter.ToSingle(x, 0), 4,
                                                            MidpointRounding.AwayFromZero);
                                                        var coorY = Math.Round(BitConverter.ToSingle(y, 0), 4,
                                                            MidpointRounding.AwayFromZero);

                                                        CoordinateX = (float)coorX;
                                                        CoordinateY = (float)coorY;

                                                        //UpdateTextBox(string.Format("照度={0}/lx,色温={1}/K", lx, color));
                                                    }
                                                    else
                                                    {
                                                        //UpdateTextBox("读取结果数据失败");
                                                    }
                                                }
                                                else
                                                {
                                                    //UpdateTextBox("读取结果数据失败，无响应");
                                                }
                                            }
                                            else
                                            {
                                                //UpdateTextBox("读取积分时间失败");
                                            }
                                        }
                                        else
                                        {
                                            //UpdateTextBox("读取积分时间失败，无响应");
                                        }
                                    }
                                }
                                else
                                {
                                    //UpdateTextBox("启动单次测量失败");
                                }
                            }
                            else
                            {
                                //UpdateTextBox("启动单次测量失败，无响应");
                            }
                        }
                        else
                        {
                            //UpdateTextBox("设置自动积分时间失败，无响应");
                        }
                    }
                    else
                    {
                        //UpdateTextBox("联机失败");
                    }
                }
                else
                {
                    //UpdateTextBox("联机失败，无响应");
                }
            });

            readAction.Invoke();

            if (Lux <= -9999 || Color <= -9999)
            {
                Thread.Sleep(50);
                readAction.Invoke();
            }
        }

        private bool TryReadData(Func func, out byte[] values, float ms = 5500)
        {
            if (_serialPort == null || !_serialPort.IsOpen)
            {
                values = new byte[0];
                return false;
            }

            _serialPort.ReadTimeout = 150;
            try
            {
                var len = _serialPort.BytesToRead;
                var buff = new byte[len];
                _serialPort.Read(buff, 0, len);
            }
            catch (Exception e)
            {
                // ignored
            }

            var sendCmd = new List<byte>();

            switch (func)
            {
                case Func.Connect:
                    sendCmd.AddRange(_connectCmd);
                    break;

                case Func.ReadTestResult:
                    sendCmd.AddRange(_readTestResultCmd);
                    break;

                case Func.ReadIntegralTime:
                    sendCmd.AddRange(_readIntegralTimeCmd);
                    break;

                case Func.ReadTestState:
                    sendCmd.AddRange(_readTestStateCmd);
                    break;

                case Func.StartTestOnce:
                    sendCmd.AddRange(_startTestOnceCmd);
                    break;

                case Func.StopTest:
                    sendCmd.AddRange(_stopTestCmd);
                    break;

                case Func.WriteIntegralTime:
                    var msBtes = BitConverter.GetBytes(ms / 1000f);
                    sendCmd.AddRange(new byte[] { 0x25, 0x01, msBtes[3], msBtes[2], msBtes[1], msBtes[0] });
                    break;

                case Func.SetAutoIntegralTime:
                    sendCmd.AddRange(new byte[] { 0x25, 0x02, 0x01 });
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(func), func, null);
            }

            _serialPort.Write(sendCmd.ToArray(), 0, sendCmd.Count);
            Thread.Sleep(200);
            var listBytes = new List<byte>();
            var temp = new List<byte>();

            while (true)
            {
                _serialPort.ReadTimeout = 150;

                try
                {
                    var len = _serialPort.BytesToRead;

                    if (len == 0)
                        break;

                    var buff = new byte[len];
                    _serialPort.Read(buff, 0, len);

                    temp.AddRange(buff);
                }
                catch (Exception exception)
                {
                    break;
                }
            }

            var tempStr = ValueHelper.GetHextStr(temp.ToArray()).Replace(" ", "");
            var tempStrIndexOf =
                tempStr.IndexOf(ValueHelper.GetHextStr(sendCmd[0]) + ValueHelper.GetHextStr(sendCmd[1]),
                    StringComparison.Ordinal);

            if (tempStrIndexOf != -1 && tempStrIndexOf % 2 == 0)
            {
                for (var i = tempStrIndexOf; i < tempStr.Length; i = i + 2)
                {
                    var b = Convert.ToByte(tempStr.Substring(i, 2), 16);
                    listBytes.Add(b);
                }
            }

            if (listBytes.Any())
            {
                values = new byte[listBytes.Count];
                Array.Copy(listBytes.ToArray(), values, listBytes.Count);

                var strOk = string.Format("接收成功：{0}", ValueHelper.GetHextStr(values));
                //UpdateTextBox(strOk);
                return true;
            }

            var strNg = string.Format("接收失败：{0}", ValueHelper.GetHextStr(temp.ToArray()));
            //UpdateTextBox(strNg);
            values = new byte[0];
            return false;
        }

        private readonly byte[] _readTestResultCmd = new byte[] { 0x25, 0x13 };
        private readonly byte[] _readIntegralTimeCmd = new byte[] { 0x25, 0x05 };
        private readonly byte[] _readTestStateCmd = new byte[] { 0x25, 0x03 };
        private readonly byte[] _connectCmd = new byte[] { 0x25, 0x00 };
        private readonly byte[] _startTestOnceCmd = new byte[] { 0x25, 0x0E, 0x01 };
        private readonly byte[] _stopTestCmd = new byte[] { 0x25, 0x25 };

        internal enum Func
        {
            /// <summary>
            /// 联机
            /// </summary>
            Connect,

            /// <summary>
            /// 读测试结果
            /// </summary>
            ReadTestResult,

            /// <summary>
            /// 读积分时间
            /// </summary>
            ReadIntegralTime,

            /// <summary>
            /// 写入积分时间
            /// </summary>
            WriteIntegralTime,

            /// <summary>
            /// 读测试状态
            /// </summary>
            ReadTestState,

            /// <summary>
            /// 开启单次测试
            /// </summary>
            StartTestOnce,

            /// <summary>
            /// 停止测试
            /// </summary>
            StopTest,

            /// <summary>
            /// 设置为自动积分时间
            /// </summary>
            SetAutoIntegralTime
        }
    }
}