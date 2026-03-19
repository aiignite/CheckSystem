using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;

namespace Controller
{
    [Description("CAN-Product,T9M照地灯")]
    public sealed class T9M : ControllerBase
    {
        public CanBus CanFd;

        public static object _sendLock = new object();
        public bool _isSleep = true;
        private uint _requestCanId = 0x639;
        private uint _responseCanId = 0x6B9;

        public T9M(string name) : base(name)
        {
            MainWork();
            SchedulerAsync();
        }

        private void MainWork()
        {
            SendMsg10Ms();
            SendMsg20Ms();
            SendMsg50Ms();
            SendMsg100Ms();
            SendMsg500Ms();
            SendMsg1000Ms();
        }

        private void SendMsg10Ms()
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = () =>
                {
                    try
                    {
                        var sendDatas = new List<CanBus.CanDataPackage>
                        {
                            new CanBus.CanDataPackage(0x260,  CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8]),
                        };
                        SendCanMsg(sendDatas.ToArray());
                    }
                    catch (Exception)
                    {

                    }
                },
                Interval = 10
            });
        }

        private void SendMsg20Ms()
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = () =>
                {
                    try
                    {
                        var sendDatas = new List<CanBus.CanDataPackage>
                        {
                            new CanBus.CanDataPackage(0x25D,  CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[]{ 0X00,0X00,0X00,0X00,0X00,0X02,0X00,0X00}),
                            new CanBus.CanDataPackage(0x260,  CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8]),
                        };
                        SendCanMsg(sendDatas.ToArray());
                    }
                    catch (Exception)
                    {

                    }
                },
                Interval = 10
            });
        }

        private void SendMsg50Ms()
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = () =>
                {
                    try
                    {
                        var sendDatas = new List<CanBus.CanDataPackage>
                        {
                            new CanBus.CanDataPackage(0x580,  CanBus.CanProtocol.Can, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }),
                            new CanBus.CanDataPackage(0x311,  CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8]),
                            new CanBus.CanDataPackage(0x260,  CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8]),
                        };
                        SendCanMsg(sendDatas.ToArray());
                    }
                    catch (Exception)
                    {

                    }
                },
                Interval = 35
            });
        }

        private void SendMsg100Ms()
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = () =>
                {
                    try
                    {
                        var sendDatas = new List<CanBus.CanDataPackage>
                        {
                            new CanBus.CanDataPackage(0x32B,  CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8]),
                            new CanBus.CanDataPackage(0x350,  CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8]),
                            new CanBus.CanDataPackage(0x318,  CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[] { 0x01, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00 }),
                            new CanBus.CanDataPackage(0x3E7,  CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8]),
                            new CanBus.CanDataPackage(0x306,  CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8]),
                            new CanBus.CanDataPackage(0x2C9,  CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data,  new byte[]{ 0X00,0X00,0X00,0X08,0X00,0X00,0X00,0X00})
                        };
                        SendCanMsg(sendDatas.ToArray());
                    }
                    catch (Exception)
                    {

                    }
                },
                Interval = 80
            });
        }

        private void SendMsg500Ms()
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = () =>
                {
                    try
                    {
                        var sendDatas = new List<CanBus.CanDataPackage>
                        {
                            new CanBus.CanDataPackage(0x39E,  CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8] { 0x19, 0x49, 0x44, 0xCA, 0xC0, 0x00, 0x00, 0x00 }),
                            new CanBus.CanDataPackage(0x322,  CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8])
                        };
                        SendCanMsg(sendDatas.ToArray());
                    }
                    catch (Exception)
                    {

                    }
                },
                Interval = 350
            });
        }

        private void SendMsg1000Ms()
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = () =>
                {
                    try
                    {
                        var sendDatas = new List<CanBus.CanDataPackage>
                        {
                            new CanBus.CanDataPackage(0x326,  CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8])
                        };
                        SendCanMsg(sendDatas.ToArray());
                    }
                    catch (Exception)
                    {

                    }
                },
                Interval = 800
            });
        }

        private void SendCanMsg(CanBus.CanDataPackage[] d)
        {
            if (!_isSleep && CanFd != null)
            {
                lock (_sendLock)
                {
                    CanFd.SendCanDatas(d);
                    Thread.Sleep(1);
                }
            }
        }

        [Description("R,读取Boot版本号")]
        public string BootVer = string.Empty;
        [Description("R,读取零件号")]
        public string PartNo = string.Empty;
        [Description("R,读取硬件版本号")]
        public string HwVer = string.Empty;
        [Description("R,读取软件版本号")]
        public string SwVer = string.Empty;
        [Description("R,读取App版本号")]
        public string AppVer = string.Empty;
        [Description("R,读取素材版本号")]
        public string MaterialVersionNumber = string.Empty;
        [Description("R,读取工厂模式")]
        public string FactoryMode = string.Empty;
        [Description("R,读取软件子版本号")]
        public string SwChildVer = string.Empty;

        [Description("打开CAN")]
        public void StartCan()
        {
            _isSleep = false;
        }

        [Description("关闭CAN")]
        public void StopCan()
        {
            _isSleep = true;
        }

        [Description("清除所有读取信息结果")]
        public void ClearReadResult()
        {
            BootVer = string.Empty;
            PartNo = string.Empty;
            HwVer = string.Empty;
            SwVer = string.Empty;
            AppVer = string.Empty;
            MaterialVersionNumber = string.Empty;
            FactoryMode = string.Empty;
            SwChildVer = string.Empty;

            ClearDTCResult = string.Empty;
            ReadDTCResult = string.Empty;
        }

        [Description("读取Boot版本号")]
        public void ReadBootVer() => BootVer = CanFd is null ? string.Empty : Encoding.ASCII.GetString(ReadDID(0xf1, 0x80));

        [Description("读取App版本号")]
        public void ReadAppVer() => AppVer = CanFd is null ? string.Empty : Encoding.ASCII.GetString(ReadDID(0xf1, 0x81));

        [Description("读取硬件版本号")]
        public void ReadHwVer() => HwVer = CanFd is null ? string.Empty : Encoding.ASCII.GetString(ReadDID(0xf1, 0x7F));

        [Description("读取软件版本号")]
        public void ReadSwVer() => SwVer = CanFd is null ? string.Empty : Encoding.ASCII.GetString(ReadDID(0xf1, 0x89));

        [Description("读取零件号")]
        public void ReadPartNo() => PartNo = CanFd is null ? string.Empty : Encoding.ASCII.GetString(ReadDID(0xf1, 0x87));

        [Description("读取素材版本号")]
        public void ReadMaterialVersionNumber() => MaterialVersionNumber = CanFd is null ? string.Empty : Encoding.ASCII.GetString(ReadDID(0x10, 0x4c));

        [Description("读取工厂模式")]
        public void ReadFactoryMode() => FactoryMode = CanFd is null ? string.Empty : ValueHelper.GetHextStr(ReadDID(0x01, 0x10));

        [Description("读取软件子版本号")]
        public void ReadSwChildVer() => SwChildVer = CanFd is null ? string.Empty : Encoding.ASCII.GetString(ReadDID(0xF1, 0x95));

        private byte[] ReadDID(byte didHi, byte didLo)
        {
            if (CanFd is null)
                return new byte[0];

            //lock (_sendLock)
            {
                byte[] echo;
                if (CanFd.CanBusWithUds.TryReadData(_requestCanId, _responseCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.CanFd, didHi, didLo, out echo, 0x00))
                    return echo;
            }

            Thread.Sleep(125);
            //lock (_sendLock)
            {
                byte[] echo;
                if (CanFd.CanBusWithUds.TryReadData(_requestCanId, _responseCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.CanFd, didHi, didLo, out echo, 0x00))
                    return echo;
            }

            return new byte[0];
        }

        [Description("R,读取0x269报文测试结果")]
        public string ReadMsg269Result = string.Empty;
        [Description("R,高压故障")]
        public string HighVoltError = string.Empty;
        [Description("R,低压故障")]
        public string LowVoltError = string.Empty;
        [Description("R,正常电压无故障")]
        public string NormalVoltNoError = string.Empty;

        [Description("读取0x269报文及高低压故障信息")]
        public void ReadMsg269(int delayMs)
        {
            ReadMsg269Result = string.Empty;
            HighVoltError = string.Empty;
            LowVoltError = string.Empty;
            NormalVoltNoError = string.Empty;

            if (CanFd is null)
                return;
            var ms = delayMs > 0 && delayMs <= 10000 ? delayMs : 3000;

            CanFd.CanRecvDataPackages.Clear();
            CanFd.AddDoNotFilterCanId(0x269);
            Thread.Sleep(ms);
            CanFd.RemoveDoNotFilterCanId(0x269);
            ReadMsg269Result = CanFd.CanRecvDataPackages.Any(f => f.CanId == 0x269 && f.CanDataLen == 8) ? "OK" : "NG";

            if (ReadMsg269Result is "OK")
            {
                var last269msg = CanFd.CanRecvDataPackages.Last(f => f.CanId == 0x269 && f.CanDataLen == 8);
                if (last269msg != null)
                {
                    var bits = ValueHelper.GetBits(last269msg.CanData);
                    HighVoltError = bits[8];
                    LowVoltError = bits[9];
                    NormalVoltNoError = LowVoltError + HighVoltError;
                }
            }
        }

        [Description("R,清除DTC结果")]
        public string ClearDTCResult = string.Empty;
        [Description("R,读取DTC结果")]
        public string ReadDTCResult = string.Empty;

        [Description("清除DTC")]
        public void ClearDTC()
        {
            lock (_sendLock)
                ClearDTCResult = CanFd is null ? string.Empty : CanFd.CanBusWithUds.TryClearAllGroupsDiagnosticInformation(_requestCanId, _responseCanId, CanBus.CanType.Standard, CanBus.CanProtocol.CanFd, 0x00) ? "OK" : "NG";
        }

        [Description("读取DTC")]
        public void ReadDtc()
        {
            ReadDTCResult = string.Empty;

            if (CanFd is null)
                return;

            var readDtcAct = new Action(() => 
            {
                lock (_sendLock)
                {
                    byte[] echo;
                    if (CanFd.CanBusWithUds.TryReadDtcInfomation(_requestCanId, _responseCanId, CanBus.CanType.Standard, 0x02, 0x2F, out echo, canProtocol: CanBus.CanProtocol.CanFd, pendingByte: 0x00))
                    {
                        if (echo != null)
                        {
                            if (echo.Length == 0)
                            {
                                ReadDTCResult = "NoError";
                            }
                            else
                            {
                                if (echo.Length % 4 == 0)
                                {
                                    var readCodes = new List<Uds14229Helper.DtcData>();

                                    for (var i = 0; i < echo.Length; i = i + 4)
                                    {
                                        var dtcData = new Uds14229Helper.DtcData(echo[i], echo[i + 1], echo[i + 2], echo[i + 3]);
                                        readCodes.Add(dtcData);
                                    }

                                    var codeNotInWhiteList = FilterCodeNotInWhiteList(readCodes);

                                    if (codeNotInWhiteList.Any())
                                    {
                                        foreach (var t in codeNotInWhiteList)
                                            ReadDTCResult += string.Format("[{0}];", t.Remark);
                                    }
                                    else
                                    {
                                        ReadDTCResult = "NoError";
                                    }
                                }
                                else
                                {
                                    ReadDTCResult = "ReadDtcResLenError";
                                }
                            }
                        }
                    }
                    else
                    {
                        ReadDTCResult = "ReadFailed";
                    }
                }
            });

            readDtcAct.Invoke();
            if (ReadDTCResult == "ReadFailed")
            {
                Thread.Sleep(250);
                readDtcAct.Invoke();
            }
        }

        private readonly List<string> _whiteList = new List<string>();

        [Description("将DTC添加进白名单")]
        public void AddDtcCodeIntoWhiteList(string code)
        {
            var index = _whiteList.FindIndex(f => string.Equals(f, code, StringComparison.CurrentCultureIgnoreCase));
            if (index == -1)
                _whiteList.Add(code.Trim().ToUpper());
        }

        private List<Uds14229Helper.DtcData> FilterCodeNotInWhiteList(
            IEnumerable<Uds14229Helper.DtcData> toFilterDtcCodes)
        {
            return (from t in toFilterDtcCodes
                    let findIndex =
                        _whiteList.FindIndex(f => string.Equals(f, t.Code, StringComparison.CurrentCultureIgnoreCase))
                    where findIndex == -1
                    select t).ToList();
        }

        [Description("R,以太网接口检测")]
        public string EthernetTestResult = string.Empty;

        [Description("以太网测试")]
        public void EthernetTest(string ip)
        {
            EthernetTestResult = string.Empty;

            using (var p1 = new Ping())
            {
                try
                {
                    var reply = p1.Send(ip); //发送主机名或Ip地址

                    EthernetTestResult = reply.Status == IPStatus.Success ? "OK" : "NG";
                }
                catch (Exception ex)
                {
                    EthernetTestResult = "NG " + ex.Message;
                }
                finally
                {
                    p1.Dispose();
                }
            }
        }
    }
}
