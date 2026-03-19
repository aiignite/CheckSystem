using System;
using System.Collections.Generic;
using System.ComponentModel;
using CommonUtility;
using CommonUtility.BusLoader;

namespace Controller
{
    [Description("LIN-Product,O1SL组合后灯")]
    public sealed class O1SlRearLamp : ControllerBase
    {
        public LinBus Lin;
        private byte _nad;

        public O1SlRearLamp(string name)
            : base(name)
        {

        }

        [Description("设置为Left RCLA")]
        public void SetLeftRcla()
        {
            _nad = 0x67;
        }

        [Description("设置为Right RCLA")]
        public void SetRightRcla()
        {
            _nad = 0x68;
        }

        [Description("设置为RCLB")]
        public void SetRclb()
        {
            _nad = 0x66;
        }

        [Description("R,SlaveSparePartNumber")]
        public string SlaveSparePartNumber = string.Empty;

        [Description("R,SlaveAppSwVersionNumber")]
        public string SlaveAppSwVersionNumber = string.Empty;

        [Description("读SlaveSparePartNumber")]
        public void ReadSlaveSparePartNumber()
        {
            SlaveSparePartNumber = string.Empty;
            SlaveSparePartNumber = Read3LenEcho(0xF1, 0x87);
        }

        [Description("读SlaveAppSwVersionNumber")]
        public void ReadSlaveAppSwVersionNumber()
        {
            SlaveAppSwVersionNumber = string.Empty;
            SlaveAppSwVersionNumber = Read2LenEcho(0xF1, 0x89);
        }

        private string Read3LenEcho(byte didHi, byte didLo)
        {
            if (Lin == null)
                return string.Empty;

            try
            {
                byte[] echo;
                if (Lin.SendMasterLinAndRecvSingleSlaveLin(0x3C, 0x3D, new byte[] { _nad, 0x03, 0x22, didHi, didLo, 0xFF, 0xFF, 0xFF }, out echo))
                {
                    if (echo != null && echo.Length == 8)
                    {
                        var len = echo[2] - 3;
                        var recv = new List<byte> { echo[6], echo[7] };

                        byte[] echo1;
                        if (Lin.SendSlaveLin(0x3D, out echo1))
                        {
                            if (echo1 != null && echo1.Length == 8)
                            {
                                for (var i = 2; i < 8; i++)
                                {
                                    recv.Add(echo1[i]);
                                }

                                byte[] echo2;
                                if (Lin.SendSlaveLin(0x3D, out echo2))
                                {
                                    if (echo2 != null && echo2.Length == 8)
                                    {
                                        for (var i = 2; i < 2 + (len - 2 - 6); i++)
                                        {
                                            recv.Add(echo2[i]);
                                        }

                                        return recv.ToArray().GetStringByAsciiBytes(false);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }

            return string.Empty;
        }

        private string Read2LenEcho(byte didHi, byte didLo)
        {
            if (Lin == null)
                return string.Empty;

            try
            {
                byte[] echo;
                if (Lin.SendMasterLinAndRecvSingleSlaveLin(0x3C, 0x3D, new byte[] { _nad, 0x03, 0x22, didHi, didLo, 0xFF, 0xFF, 0xFF }, out echo))
                {
                    if (echo != null && echo.Length == 8)
                    {
                        var len = echo[2] - 3;
                        var recv = new List<byte> { echo[6], echo[7] };

                        byte[] echo1;
                        if (Lin.SendSlaveLin(0x3D, out echo1))
                        {
                            if (echo1 != null && echo1.Length == 8)
                            {
                                for (var i = 2; i < 2 + (len - 2); i++)
                                {
                                    recv.Add(echo1[i]);
                                }


                                return recv.ToArray().GetStringByAsciiBytes(false);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }

            return string.Empty;
        }

        private string Read1LenEcho(byte didHi, byte didLo)
        {
            if (Lin == null)
                return string.Empty;

            try
            {
                byte[] echo;
                if (Lin.SendMasterLinAndRecvSingleSlaveLin(0x3C, 0x3D, new byte[] { _nad, 0x03, 0x22, didHi, didLo, 0xFF, 0xFF, 0xFF }, out echo))
                {
                    if (echo != null && echo.Length == 8)
                    {
                        var len = echo[1] - 3;
                        var recv = new List<byte>();
                        for (var i = 5; i < 5 + len; i++)
                        {
                            recv.Add(echo[i]);
                        }

                        return recv.ToArray().GetStringByAsciiBytes(false);
                    }
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }

            return string.Empty;
        }
    }
}
