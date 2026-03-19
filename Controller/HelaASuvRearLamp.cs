using CommonUtility;
using CommonUtility.BusLoader;
using CommonUtility.FileOperator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Controller
{
    [Description("LIN-Product,海拉后灯-lin波特率19200")]
    public sealed class HelaASuvRearLamp : ControllerBase
    {
        public LinBus Lin19200;
        public string EolWrite;
        public HelaASuvRearLamp(string name)
            : base(name)
        {
            foreach (var temp
                 in Enum.GetValues(typeof(LampOnOffType)).Cast<LampOnOffType>())
                _lampOperaterDic.Add(temp, temp.GetCustomAttribute<MatrixValDefinition>());

            Kl15On();
            RearLampOff();

            if (_th != null)
            {
                _th.Abort();
                _th.Join();
            }

            _th = new Thread(MainWork) { IsBackground = true };
            _th.Start();
        }

        ~HelaASuvRearLamp()
        {
            Dispose();
        }

        #region 点灯相关

        private void Count()
        {
            _lampOperaterDic[LampOnOffType.BcmSbbr01Bz].Value++;
            if (_lampOperaterDic[LampOnOffType.BcmSbbr01Bz].Value > 15)
                _lampOperaterDic[LampOnOffType.BcmSbbr01Bz].Value = 0;
            MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.BcmSbbr01Bz]);
        }

        [Description("唤醒")]
        public void LampAwake()
        {
            IsSleep = false;
        }

        [Description("休眠")]
        public void LampSleep()
        {
            IsSleep = true;
        }

        [Description("KL15开关ON")]
        public void Kl15On()
        {
            MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.ZasKl15On]);
            MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.ZasKl15InvOff]);
        }

        [Description("KL15开关OFF")]
        public void Kl15Off()
        {
            MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.ZasKl15Off]);
            MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.ZasKl15InvOn]);
        }

        [Description("TAIL 3D亮")]
        public void Tail3DOn()
        {
            SwitchTail3D(true);
        }

        [Description("TAIL 3D灭")]
        public void Tail3DOff()
        {
            SwitchTail3D(false);
        }

        private void SwitchTail3D(bool isOn)
        {
            if (isOn)
            {
                MotorolaMatrix0X0A.UpdateData(
                    _lampOperaterDic[LampOnOffType.SbbrParklichtLiAnfOn]);
                MotorolaMatrix0X0A.UpdateData(
                    _lampOperaterDic[LampOnOffType.SbbrParklichtReAnfOn]);
            }
            else
            {
                MotorolaMatrix0X0A.UpdateData(
                    _lampOperaterDic[LampOnOffType.SbbrParklichtLiAnfOff]);
                MotorolaMatrix0X0A.UpdateData(
                    _lampOperaterDic[LampOnOffType.SbbrParklichtReAnfOff]);
            }
        }

        /// <summary>
        /// 制动灯亮
        /// </summary>
        [Description("制动灯亮")]
        public void StopOn()
        {
            MotorolaMatrix0X0A.UpdateData(
                _lampOperaterDic[LampOnOffType.SbbrBrlAnfOn]);
        }

        /// <summary>
        /// 制动灯灭
        /// </summary>
        [Description("制动灯灭")]
        public void StopOff()
        {
            MotorolaMatrix0X0A.UpdateData(
                _lampOperaterDic[LampOnOffType.SbbrBrlAnfOff]);
        }

        /// <summary>
        /// 位置灯亮
        /// </summary>
        [Description("位置灯低亮")]
        public void RearLampOn()
        {
            SwitchRearLamp(true);
            SwitchRearLampHd(false);
        }

        [Description("位置灯高亮开")]
        public void RearLampHdOn()
        {
            SwitchRearLamp(true);
            SwitchRearLampHd(true);
        }

        /// <summary>
        /// 位置灯灭
        /// </summary>
        [Description("位置灯灭")]
        public void RearLampOff()
        {
            SwitchRearLamp(false);
            SwitchRearLampHd(false);
        }

        private void SwitchRearLamp(bool isOn)
        {
            if (isOn)
            {
                MotorolaMatrix0X0A.UpdateData(
                    _lampOperaterDic[LampOnOffType.SbbrSchlusslichtAnfOn]);
                MotorolaMatrix0X0A.UpdateData(
                    _lampOperaterDic[LampOnOffType.SbbrSchlusslichtAnfInvOff]);
            }
            else
            {
                MotorolaMatrix0X0A.UpdateData(
                    _lampOperaterDic[LampOnOffType.SbbrSchlusslichtAnfOff]);
                MotorolaMatrix0X0A.UpdateData(
                    _lampOperaterDic[LampOnOffType.SbbrSchlusslichtAnfInvOn]);
            }
        }

        private void SwitchRearLampHd(bool isOn)
        {
            MotorolaMatrix0X0A.UpdateData(isOn
                ? _lampOperaterDic[LampOnOffType.ZvHdOffenOn]
                : _lampOperaterDic[LampOnOffType.ZvHdOffenOff]);
        }

        /// <summary>
        /// 倒车灯亮
        /// </summary>
        [Description("倒车灯亮")]
        public void BulLampOn()
        {
            SwitchBulLamp(true);
        }

        /// <summary>
        /// 倒车灯灭
        /// </summary>
        [Description("倒车灯灭")]
        public void BulLampOff()
        {
            SwitchBulLamp(false);
        }

        /// <summary>
        /// 倒车灯
        /// </summary>
        /// <param name="isOn"></param>
        private void SwitchBulLamp(bool isOn)
        {
            MotorolaMatrix0X0A.UpdateData(isOn
               ? _lampOperaterDic[LampOnOffType.SbbrRueckfahrlichtAnfOn]
               : _lampOperaterDic[LampOnOffType.SbbrRueckfahrlichtAnfOff]);
        }

        [Description("雾灯亮")]
        public void FogLampOn()
        {
            SwitchFogLamp(true);
        }

        [Description("雾灯灭")]
        public void FogLampOff()
        {
            SwitchFogLamp(false);
        }

        private void SwitchFogLamp(bool isOn)
        {
            MotorolaMatrix0X0A.UpdateData(isOn
              ? _lampOperaterDic[LampOnOffType.FogLampOn]
              : _lampOperaterDic[LampOnOffType.FogLampOff]);
        }

        /// <summary>
        /// 左转向灯顺序亮
        /// </summary>
        [Description("左转向灯顺序亮")]
        public void LeftRearTurnRunningOn()
        {
            SwitchLeftTurnFlicker(false);
            SwitchLeftRearTurnRunningLamp(true);
        }

        private void SwitchLeftRearTurnRunningLamp(bool isOn)
        {
            if (isOn)
            {
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiLinksOn]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiRechtsOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraLinksOn]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraRechtsOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiAnimationOn]);
            }
            else
            {
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiLinksOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiRechtsOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraLinksOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraRechtsOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiAnimationOff]);
            }
        }

        /// <summary>
        /// 左转向灯闪烁亮
        /// </summary>
        [Description("左转向灯闪烁亮")]
        public void LeftRearTurnFlickerOn()
        {
            SwitchLeftRearTurnRunningLamp(false);
            SwitchLeftTurnFlicker(true);
        }


        [Description("左转向灯灭")]
        public void LeftRearTurnOff()
        {
            SwitchRightTurnFlicker(false);
            SwitchLeftRearTurnRunningLamp(false);
        }

        private void SwitchLeftTurnFlicker(bool isOn)
        {
            if (isOn)
            {
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiLinksOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiRechtsOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraLinksOn]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraRechtsOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiAnimationOff]);
            }
            else
            {
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiLinksOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiRechtsOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraLinksOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraRechtsOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiAnimationOff]);
            }
        }

        [Description("右转向灯顺序亮")]
        public void RightRearTurnRunningOn()
        {
            SwitchRightTurnFlicker(false);
            SwitchRightRearTurnRunningLamp(true);
        }

        private void SwitchRightRearTurnRunningLamp(bool isOn)
        {
            if (isOn)
            {
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiLinksOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiRechtsOn]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraLinksOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraRechtsOn]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiAnimationOn]);
            }
            else
            {
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiLinksOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiRechtsOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraLinksOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraRechtsOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiAnimationOff]);
            }
        }

        [Description("右转向灯闪烁亮")]
        public void RightRearTurnFlickerOn()
        {
            SwitchRightRearTurnRunningLamp(false);
            SwitchRightTurnFlicker(true);
        }

        [Description("右转向灯灭")]
        public void RightRearTurnOff()
        {
            SwitchRightTurnFlicker(false);
            SwitchRightRearTurnRunningLamp(false);
        }

        private void SwitchRightTurnFlicker(bool isOn)
        {
            if (isOn)
            {
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiLinksOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiRechtsOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraLinksOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraRechtsOn]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiAnimationOff]);
            }
            else
            {
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiLinksOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiRechtsOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraLinksOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraRechtsOff]);
                MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[LampOnOffType.SbbrFraWiBiAnimationOff]);
            }
        }

        private void SwitchSignal(LampOnOffType type)
        {
            MotorolaMatrix0X0A.UpdateData(_lampOperaterDic[type]);
        }

        [Description("离家动画")]
        public void LevingHome()
        {
            AllLedOff();
            //Thread.Sleep(10);
            SwitchSignal(LampOnOffType.Bcm1LhAktivOn);
        }

        [Description("回家动画")]
        public void ComingHome()
        {
            AllLedOff();
            SwitchSignal(LampOnOffType.Bcm1ChAktivOn);
        }

        [Description("关闭所有灯光并打断动画")]
        public void AllLedOff()
        {
            StopOff();
            RearLampOff();
            FogLampOff();
            BulLampOff();
            LeftRearTurnOff();
            RightRearTurnOff();
            Kl15On();
            ResetAnimation();
        }
        private void ResetAnimation()
        {
            //AllLedOff();
            SwitchSignal(LampOnOffType.SbbrChLhAnimationOff);
            SwitchSignal(LampOnOffType.Bcm1ChAktivOff);
            SwitchSignal(LampOnOffType.Bcm1LhAktivOff);
        }

        #endregion

        #region 版本信息

        private string _lampType = "RIGHT RCLA";

        [Description("设置为RIGHT RCLA")]
        public void SetRightRcla()
        {
            _lampType = "RIGHT RCLA";
        }

        [Description("设置为LEFT RCLA")]
        public void SetLeftRcla()
        {
            _lampType = "LEFT RCLA";
        }

        [Description("R,FazitId")]
        public string FazitId = string.Empty;
        [Description("R,SerialNumber")]
        public string SerialNumber = string.Empty;

        [Description("R,SparePartNumber")]
        public string SparePartNumber = string.Empty;
        [Description("R,EcuHartwareNumber")]
        public string EcuHartwareNumber = string.Empty;
        [Description("R,SystemNameOrEngineType")]
        public string SystemNameOrEngineType = string.Empty;
        [Description("R,AppVer")]
        public string AppVer = string.Empty;
        [Description("R,EcuHardwareVer")]
        public string EcuHardwareVer = string.Empty;
        [Description("R,HflInternapAppSwVer")]
        public string HflInternapAppSwVer = string.Empty;
        [Description("R,HflInternapBootSwVer")]
        public string HflInternapBootSwVer = string.Empty;

        [Description("ReadVer")]
        public void ReadVer()
        {
            //FazitId = string.Empty;
            //SerialNumber = string.Empty;
            SparePartNumber = string.Empty;
            EcuHartwareNumber = string.Empty;
            SystemNameOrEngineType = string.Empty;
            AppVer = string.Empty;
            EcuHardwareVer = string.Empty;
            HflInternapAppSwVer = string.Empty;
            HflInternapBootSwVer = string.Empty;

            byte nad;
            switch (_lampType)
            {
                case "RIGHT RCLA":
                    nad = 0x68;
                    break;

                case "LEFT RCLA":
                    nad = 0x67;
                    break;

                default:
                    return;
            }

            //FazitId = ReadDid(23, 0xF1, 0x7C, nad, "ASCII");
            //SerialNumber = ReadDid(20, 0xF1, 0x8C, nad, "ASCII");

            SparePartNumber = ReadDid(11, 0xF1, 0x87, nad, "ASCII");
            EcuHartwareNumber = ReadDid(11, 0xF1, 0x91, nad, "ASCII");
            SystemNameOrEngineType = ReadDid(20, 0xF1, 0x97, nad, "ASCII");
            AppVer = ReadDid(4, 0xF1, 0x89, nad, "ASCII");
            EcuHardwareVer = ReadDid(3, 0xF1, 0xA3, nad, "ASCII");
            HflInternapAppSwVer = ReadDid(3, 0xFA, 0xAA, nad, "HEX");
            HflInternapBootSwVer = ReadDid(3, 0xFB, 0xBB, nad, "HEX");
            SparePartNumber = ReadDid(11, 0xF1, 0x87, nad, "ASCII");
        }

        [Description("读FazitId")]
        public void ReadFazitId()
        {
            FazitId = string.Empty;
            byte nad;
            switch (_lampType)
            {
                case "RIGHT RCLA":
                    nad = 0x68;
                    break;

                case "LEFT RCLA":
                    nad = 0x67;
                    break;

                default:
                    return;
            }
            for (int i = 0; i < 5; i++)
            {
                if (!string.IsNullOrEmpty(FazitId))
                    break;
                FazitId = ReadDid(23, 0xF1, 0x7C, nad, "ASCII");
                Thread.Sleep(100);
            }

        }

        [Description("读SerialNumber")]
        public void ReadSerialNumber()
        {
            SerialNumber = string.Empty;
            byte nad;
            switch (_lampType)
            {
                case "RIGHT RCLA":
                    nad = 0x68;
                    break;

                case "LEFT RCLA":
                    nad = 0x67;
                    break;

                default:
                    return;
            }

            SerialNumber = ReadDid(20, 0xF1, 0x8C, nad, "ASCII");
        }

        public string ToWriteBarcode = string.Empty;

        //[Description("R,根据二维码写入FazitId结果")]
        //public string WriteFazitId = string.Empty;
        //[Description("R,根据二维码写入SeruamNumber结果")]
        //public string WriteSeruamNumber = string.Empty;

        //[Description("根据二维码写入FazitId和SerialNumber")]
        public void WriteFazitIdAndSerialNumberByBarcode()
        {
            //ToWriteBarcode = "27289901110000070004010026";

            EolWrite = "NG";
            FazitId = string.Empty;
            SerialNumber = string.Empty;
            //ReadFazitId();
            //ReadSerialNumber();

            if (Lin19200 == null)
                return;

            if (string.IsNullOrEmpty(AppVer))
                return;

            if (string.IsNullOrEmpty(EcuHardwareVer))
                return;

            if (string.IsNullOrEmpty(ToWriteBarcode))
            {
                ReadFazitId();
                ReadSerialNumber();

                FazitId = "NG 条码为空，产品读取结果为：" + FazitId;
                SerialNumber = "NG 条码为空，产品读取结果为：" + SerialNumber;
                EolWrite += " 条码为空，产品读取FazitId结果为：" + FazitId + "产品读取SerialNumber结果为：" + SerialNumber;
                return;
            }

            if (ToWriteBarcode.Length != 26)
            {
                ReadFazitId();
                ReadSerialNumber();

                FazitId = "NG 条码长度不对，产品读取结果为：" + FazitId;
                SerialNumber = "NG 条码长度不对，产品读取结果为：" + SerialNumber;
                EolWrite += " 条码长度不对，产品读取FazitId结果为：" + FazitId + "产品读取SerialNumber结果为：" + SerialNumber;
                return;
            }

            if (!ToWriteBarcode.StartsWith("27289901") && !ToWriteBarcode.StartsWith("27289902"))
            {
                ReadFazitId();
                ReadSerialNumber();

                FazitId = "NG 条码格式不对（非27289901或27289902开头），产品读取结果为：" + FazitId;
                SerialNumber = "NG 条码格式不对（非27289901或27289902开头），产品读取结果为：" + SerialNumber;
                EolWrite += " 条码格式不对，产品读取FazitId结果为：" + FazitId + "产品读取SerialNumber结果为：" + SerialNumber;
                return;
            }

            int deviceId;
            byte nad;
            switch (_lampType)
            {
                case "RIGHT RCLA":
                    deviceId = 1;
                    nad = 0x68;
                    break;

                case "LEFT RCLA":
                    deviceId = 2;
                    nad = 0x67;
                    break;

                default:
                    return;
            }

            var count = GetCount();
            if (int.Parse(count) > 9999 || int.Parse(count) < 1)
            {
                ReadFazitId();
                ReadSerialNumber();

                FazitId = "NG 写入序列号不在1~9999之间(count=" + count + ")，产品读取结果为：" + FazitId;
                SerialNumber = "NG 写入序列号不在1~9999之间(count=" + count + ")，产品读取结果为：" + SerialNumber;
                EolWrite += " 写入序列号不在1~9999之间(count=" + count + ")，产品读取FazitId结果为：" + FazitId + "产品读取SerialNumber结果为：" + SerialNumber;
                return;
            }

            var days = DateTime.Now.ToString("dd.MM.yy");
            var week = GetTheWeekNum2();
            var serialNo = string.Format("{0}{1}.{2}{3}", ToWriteBarcode.Substring(0, 8), week.PadLeft(2, '0'), DateTime.Now.ToString("yy"), count.ToString().PadLeft(7, '0'));
            var fazit = string.Format("E6X-HFL{0}{1}{2}", days, deviceId.ToString().PadLeft(4, '0'), count.ToString().PadLeft(3, '0'));

            var fazitBytes = Encoding.ASCII.GetBytes(fazit);
            var serialNoBytes = Encoding.ASCII.GetBytes(serialNo);

            var isRepeat = CheckIsRepeat(fazit, serialNo);
            if (isRepeat)
            {
                ReadFazitId();
                ReadSerialNumber();

                FazitId = "NG 写入序列号有重复禁止写入(FazitId=" + fazit + ")，产品读取结果为：" + FazitId;
                SerialNumber = "NG 写入序列号有重复禁止写入(SerialNumber=" + serialNo + ")，产品读取结果为：" + SerialNumber;
                EolWrite += " 写入序列号有重复禁止写入，产品读取FazitId结果为：" + FazitId + "产品读取SerialNumber结果为：" + SerialNumber;
                return;
            }

            var tpSleep = IsSleep;
            if (tpSleep == true)
            {
                LampAwake();
                Thread.Sleep(2000);
                LampSleep();
                Thread.Sleep(200);
            }
            else
            {
                Thread.Sleep(2000);
                LampSleep();
                Thread.Sleep(200);
            }

            lock (_lockSend)
            {
                {
                    Lin19200.SendMasterLin(0x3C,
                        new byte[] { nad, 0x10, 0x1A, 0x2E, 0xF1, 0x7C, fazitBytes[0], fazitBytes[1] });
                    Thread.Sleep(150);
                    Lin19200.SendMasterLin(0x3C,
                       new byte[] { nad, 0x21, fazitBytes[2], fazitBytes[3], fazitBytes[4], fazitBytes[5], fazitBytes[6], fazitBytes[7] });
                    Lin19200.SendMasterLin(0x3C,
                       new byte[] { nad, 0x22, fazitBytes[8], fazitBytes[9], fazitBytes[10], fazitBytes[11], fazitBytes[12], fazitBytes[13] });
                    Lin19200.SendMasterLin(0x3C,
                       new byte[] { nad, 0x23, fazitBytes[14], fazitBytes[15], fazitBytes[16], fazitBytes[17], fazitBytes[18], fazitBytes[19] });
                    Lin19200.SendMasterLin(0x3C,
                       new byte[] { nad, 0x24, fazitBytes[20], fazitBytes[21], fazitBytes[22], 0xFF, 0xFF, 0xFF });
                    Thread.Sleep(250);
                    byte[] echo1;
                    Lin19200.SendSlaveLin(0x3D, out echo1, 100);
                    Console.WriteLine(echo1);
                }
                Thread.Sleep(100);
                {
                    Lin19200.SendMasterLin(0x3C,
                        new byte[] { nad, 0x10, 0x17, 0x2E, 0xF1, 0x8C, serialNoBytes[0], serialNoBytes[1] });
                    Thread.Sleep(150);
                    Lin19200.SendMasterLin(0x3C,
                       new byte[] { nad, 0x21, serialNoBytes[2], serialNoBytes[3], serialNoBytes[4], serialNoBytes[5], serialNoBytes[6], serialNoBytes[7] });
                    Lin19200.SendMasterLin(0x3C,
                       new byte[] { nad, 0x22, serialNoBytes[8], serialNoBytes[9], serialNoBytes[10], serialNoBytes[11], serialNoBytes[12], serialNoBytes[13] });
                    Lin19200.SendMasterLin(0x3C,
                       new byte[] { nad, 0x23, serialNoBytes[14], serialNoBytes[15], serialNoBytes[16], serialNoBytes[17], serialNoBytes[18], serialNoBytes[19] });
                    Thread.Sleep(250);
                    byte[] echo2;
                    Lin19200.SendSlaveLin(0x3D, out echo2, 100);
                    Console.WriteLine(echo2);
                }
            }

            if (tpSleep == true)
            {
                LampAwake();
                Thread.Sleep(2500);
                LampSleep();
                Thread.Sleep(200);
            }
            else
            {
                Thread.Sleep(2500);
                LampSleep();
                Thread.Sleep(200);
            }
            var fazitIdRead = ReadDid(23, 0xF1, 0x7C, nad, "ASCII");
            var serialNumberRead = ReadDid(20, 0xF1, 0x8C, nad, "ASCII");
            //fazitIdRead = ReadDid(23, 0xF1, 0x7C, nad, "ASCII");
            if (fazitIdRead == fazit)
                FazitId = "OK " + fazitIdRead;
            else
                FazitId = string.Format("NG 实际读取结果为：{0}，期望读取结果（需写入）为：{1}", fazitIdRead, fazit);
            //FazitId = "NG " + fazitIdRead;

            if (serialNumberRead == serialNo)
                SerialNumber = "OK " + serialNumberRead;
            else
                SerialNumber = string.Format("NG 实际读取结果为：{0}，期望读取结果（需写入）为：{1}", serialNumberRead, serialNo);

            if (FazitId.StartsWith("OK") && SerialNumber.StartsWith("OK"))
            {
                EolWrite = "OK";
            }
            else
            {
                EolWrite += " 写入与读取不符：" + string.Format(" 实际读取结果为：{0}，期望读取结果（需写入）为：{1}", fazitIdRead, fazit) + string.Format(" 实际读取结果为：{0}，期望读取结果（需写入）为：{1}", serialNumberRead, serialNo);
            }

            ToWriteBarcode = string.Empty;

            if (tpSleep == true)
            {
                LampSleep();
            }
        }

        [Description("R,存储数据结果")]
        public string SaveDataResult;

        [Description("R/W,服务器IP地址")]
        public string ServerIp = "192.168.0.138";

        [Description("R/W,服务器数据库名称")]
        public string ServerDataBase = "PLMS";

        [Description("R/W,服务器用户名")]
        public string ServerUid = "ipms";

        [Description("R/W,服务器用密码")]
        public string ServerPwd = "Scae2020#";


        public bool CheckIsRepeat(string fazit, string serialNo)
        {
            var st = new Stopwatch();
            st.Start();
            var existSql = new StringBuilder();

            existSql.Append("select count(1) from SyProductionCheckData");
            existSql.Append(" where (Result = '0001' or Result = '9001') and  MaterialName like 'ASUV尾灯%' and (CheckData like '%" + fazit + "%' or CheckData like '%" + serialNo + "%')");

            var isRepeat = Exists(existSql.ToString());

            st.Stop();
            Console.WriteLine("check repeat cost time:" + st.ElapsedMilliseconds + "ms");

            return isRepeat;
        }

        private bool Exists(string strSql, params SqlParameter[] cmdParms)
        {
            var obj = GetSingle(strSql, cmdParms);
            int cmdresult;
            if (Equals(obj, null) || Equals(obj, DBNull.Value))
                cmdresult = 0;
            else
                cmdresult = int.Parse(obj.ToString());
            return cmdresult != 0;
        }

        public object GetSingle(string sqlString, params SqlParameter[] cmdParms)
        {
            var sqlConnectiong = string.Format(@"server={0};database={1};uid={2};pwd={3}", ServerIp, ServerDataBase,
                ServerUid, ServerPwd);

            using (var connection = new SqlConnection(sqlConnectiong))
            {
                using (var cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                        var obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if (Equals(obj, null) || Equals(obj, DBNull.Value))
                            return null;
                        return obj;
                    }
                    catch (SqlException e)
                    {
                        throw e;
                    }
                }
            }
        }

        private static void PrepareCommand(
   SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms == null)
                return;
            foreach (var parameter in cmdParms)
            {
                if ((parameter.Direction == ParameterDirection.InputOutput ||
                     parameter.Direction == ParameterDirection.Input) &&
                    (parameter.Value == null))
                    parameter.Value = DBNull.Value;
                cmd.Parameters.Add(parameter);
            }
        }

        public string GetTheWeekNum2()
        {
            //创建公历日历对象
            GregorianCalendar gregorianCalendar = new GregorianCalendar();

            //获取指定日期是周数 CalendarWeekRule指定 第一周开始于该年的第一天，DayOfWeek指定每周第一天是星期几
            int weekOfYear = gregorianCalendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);

            return weekOfYear.ToString();

        }

        public IniFileHelper SetupL = new IniFileHelper(string.Format(@"{0}\{1}", Application.StartupPath, "HelaASuvNumberCountLeft.ini"));
        public IniFileHelper SetupR = new IniFileHelper(string.Format(@"{0}\{1}", Application.StartupPath, "HelaASuvNumberCountRight.ini"));

        [Description("获取编号")]
        private string GetCount()
        {
            bool isLeft = false;
            switch (_lampType)
            {
                case "RIGHT RCLA":
                    isLeft = false;
                    break;

                case "LEFT RCLA":
                    isLeft = true;
                    break;

                default:
                    return "-1";
            }
            var week = GetTheWeekNum2();
            var countIni = isLeft ? SetupL.IniReadValue("System", "Num") : SetupR.IniReadValue("System", "Num");
            var weekIni = isLeft ? SetupL.IniReadValue("System", "week") : SetupR.IniReadValue("System", "week");
            if (weekIni != week)
            {
                UpdateCount(week, "1");
                return 1.ToString();
            }
            else
            {
                UpdateCount(week, (int.Parse(countIni) + 1).ToString());
                return (int.Parse(countIni) + 1).ToString();
            }
        }

        private void UpdateCount(string week, string conut)
        {
            switch (_lampType)
            {
                case "RIGHT RCLA":
                    SetupR.IniWriteValue("System", "Week", week);
                    SetupR.IniWriteValue("System", "Num", conut);
                    break;

                case "LEFT RCLA":
                    SetupL.IniWriteValue("System", "Week", week);
                    SetupL.IniWriteValue("System", "Num", conut);
                    break;

                default:
                    return;
            }

        }

        private string ReadDid(int dataLen, byte didHi, byte didLo, byte nad, string dataType)
        {
            if (Lin19200 == null)
            {
                return string.Empty;
            }

            var readValueDataLen = dataLen;
            var data0 = nad;

            //Console.WriteLine("config: " + infoName);
            //Console.WriteLine("config: " + standard);

            lock (_lockSend)
            {
                try
                {
                    var sendBytes = new byte[] { data0, 0x03, 0x22, didHi, didLo, 0xFF, 0xFF, 0xFF };
                    Lin19200.SendMasterLin(0x3C, sendBytes);

                    var resultBs = new List<byte>();

                    Thread.Sleep(100);
                    byte[] recv;
                    var isReadFirstFrameSucceed = Lin19200.SendSlaveLin(0x3D, out recv);
                    if (isReadFirstFrameSucceed && recv != null && recv.Length == 8)
                    {
                        if (recv[0] == data0)
                        {
                            if (recv[1] >= 0x10) // 多帧
                            {
                                if (recv[3] == 0x62 && recv[4] == didHi && recv[5] == didLo)
                                {
                                    resultBs.Add(recv[6]);
                                    resultBs.Add(recv[7]);
                                    var len = (recv[1] - 0x10) * 256 + recv[2];

                                    int count;
                                    if ((len - 5) % 6 == 0)
                                        count = (len - 5) / 6;
                                    else
                                        count = (len - 5) / 6 + 1;

                                    for (var i = 0; i < count; i++)
                                    {
                                        byte[] recvBytesRest;
                                        var isSucceed = Lin19200.SendSlaveLin(0x3D, out recvBytesRest);
                                        if (isSucceed && recvBytesRest != null && recvBytesRest.Length == 8)
                                        {
                                            for (var j = 2; j < 8; j++)
                                                resultBs.Add(recvBytesRest[j]);
                                        }
                                    }
                                }
                            }
                            else // 单帧
                            {
                                if (recv[2] == 0x62 && recv[3] == didHi && recv[4] == didLo)
                                {
                                    for (var i = 5; i < 5 + dataLen; i++)
                                    {
                                        resultBs.Add(recv[i]);
                                    }
                                }
                            }
                        }

                        if (resultBs.Any() && resultBs.Count >= readValueDataLen)
                        {
                            var temp3333 = new byte[readValueDataLen];
                            Array.Copy(resultBs.ToArray(), 0, temp3333, 0, readValueDataLen);
                            resultBs.Clear();
                            resultBs.AddRange(temp3333);
                        }

                        var getStr = string.Empty;
                        if (string.Equals(dataType, "ASCII", StringComparison.CurrentCultureIgnoreCase))
                            getStr = Encoding.ASCII.GetString(resultBs.ToArray());
                        else if (string.Equals(dataType, "Hex", StringComparison.CurrentCultureIgnoreCase))
                            getStr = resultBs.Aggregate(getStr, (current, t) => current + ValueHelper.GetHextStr(t));
                        return getStr;
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }

                return string.Empty;
            }
        }

        #endregion

        #region 内部函数逻辑
        public bool IsSleep = true;
        private readonly Thread _th;

        private readonly Dictionary<LampOnOffType, MatrixValDefinition> _lampOperaterDic =
            new Dictionary<LampOnOffType, MatrixValDefinition>();

        public readonly LinCommunicationMatrix.IntelMatrix MotorolaMatrix0X0A =
            new LinCommunicationMatrix.IntelMatrix(0x0A, 8);

        private readonly object _lockSend = new object();

        private void MainWork()
        {
            while (_th.IsAlive)
            {
                if (!_th.IsAlive)
                    break;

                Thread.Sleep(25);

                if (Lin19200 == null)
                    continue;

                Count();

                if (IsSleep)
                    continue;

                lock (_lockSend)
                {
                    Lin19200.SendMasterLin(
                        MotorolaMatrix0X0A.MasterLinId, MotorolaMatrix0X0A.MatrixData);
                }


                //Lin19200.SendMasterLin(0x0A, new byte[] { 0x00, 0x80, 0x00, 0x00, 0xE0, 0xB8, 0x0D, 0x03 });
            }
        }

        internal enum LampOnOffType
        {
            ///// <summary>
            ///// 左转向灯时序点亮启动
            ///// </summary>
            //[MatrixValDefinition(16, 1, 1)]
            //[Description("0x0A")]
            //LeftRearTurnRunningOn,

            ///// <summary>
            ///// 左转向灯时序点亮结束
            ///// </summary>
            //[MatrixValDefinition(16, 1, 0)]
            //[Description("0x0A")]
            //LeftRearTurnRunningOff,

            ///// <summary>
            ///// 右转向灯时序点亮启动
            ///// </summary>
            //[MatrixValDefinition(17, 1, 1)]
            //[Description("0x0A")]
            //RightRearTurnRunningOn,

            ///// <summary>
            ///// 右转向灯时序点亮结束
            ///// </summary>
            //[MatrixValDefinition(17, 1, 0)]
            //[Description("0x0A")]
            //RightRearTurnRunningOff,

            ///// <summary>
            ///// 左转向灯闪烁点亮启动
            ///// </summary>
            //[MatrixValDefinition(18, 1, 1)]
            //[Description("0x0A")]
            //LeftRearTurnFlickerOn,

            ///// <summary>
            ///// 左转向灯闪烁点亮结束
            ///// </summary>
            //[MatrixValDefinition(18, 1, 0)]
            //[Description("0x0A")]
            //LeftRearTurnFlickerOff,

            ///// <summary>
            ///// 右转向灯闪烁点亮启动
            ///// </summary>
            //[MatrixValDefinition(21, 1, 1)]
            //[Description("0x0A")]
            //RigthRearTurnFlickerOn,

            ///// <summary>
            ///// 右转向灯闪烁点亮结束
            ///// </summary>
            //[MatrixValDefinition(21, 1, 0)]
            //[Description("0x0A")]
            //RigthRearTurnFlickerOff,

            /// <summary>
            /// 位置灯的开关信号
            /// </summary>
            [MatrixValDefinition(48, 1, 1)]
            [Description("0x0A")]
            SbbrSchlusslichtAnfInvOn,

            /// <summary>
            /// 位置灯的开关信号
            /// </summary>
            [MatrixValDefinition(48, 1, 0)]
            [Description("0x0A")]
            SbbrSchlusslichtAnfInvOff,

            /// <summary>
            /// 位置灯的开关信号
            /// </summary>
            [MatrixValDefinition(49, 1, 1)]
            [Description("0x0A")]
            SbbrSchlusslichtAnfOn,

            /// <summary>
            /// 位置灯的开关信号
            /// </summary>
            [MatrixValDefinition(49, 1, 0)]
            [Description("0x0A")]
            SbbrSchlusslichtAnfOff,

            /// <summary>
            /// KL15的开关信号
            /// </summary>
            [MatrixValDefinition(12, 1, 1)]
            [Description("0x0A")]
            ZasKl15On,

            /// <summary>
            /// KL15的开关信号
            /// </summary>
            [MatrixValDefinition(12, 1, 0)]
            [Description("0x0A")]
            ZasKl15Off,

            /// <summary>
            /// KL15的开关信号
            /// </summary>
            [MatrixValDefinition(15, 1, 1)]
            [Description("0x0A")]
            ZasKl15InvOn,

            /// <summary>
            /// KL15的开关信号
            /// </summary>
            [MatrixValDefinition(15, 1, 0)]
            [Description("0x0A")]
            ZasKl15InvOff,

            /// <summary>
            /// 位置灯静态开关
            /// </summary>
            [MatrixValDefinition(43, 1, 1)]
            [Description("0x0A")]
            SbbrShlStatischStatusOn,

            /// <summary>
            /// 位置灯静态开关
            /// </summary>
            [MatrixValDefinition(43, 1, 0)]
            [Description("0x0A")]
            SbbrShlStatischStatusOff,

            /// <summary>
            /// 动画模式
            /// </summary>
            [MatrixValDefinition(30, 2, 1)]
            [Description("0x0A")]
            SbbrChLhAnimationOn,

            /// <summary>
            /// 动画模式
            /// </summary>
            [MatrixValDefinition(30, 2, 0)]
            [Description("0x0A")]
            SbbrChLhAnimationOff,

            /// <summary>
            /// 尾灯高亮
            /// </summary>
            [MatrixValDefinition(14, 1, 0)]
            [Description("0x0A")]
            ZvHdOffenOff,

            /// <summary>
            /// 尾灯高亮
            /// </summary>
            [MatrixValDefinition(14, 1, 1)]
            [Description("0x0A")]
            ZvHdOffenOn,

            /// <summary>
            /// 倒车灯
            /// </summary>
            [MatrixValDefinition(52, 1, 1)]
            [Description("0x0A")]
            SbbrRueckfahrlichtAnfOn,

            /// <summary>
            /// 倒车灯
            /// </summary>
            [MatrixValDefinition(52, 1, 0)]
            [Description("0x0A")]
            SbbrRueckfahrlichtAnfOff,

            /// <summary>
            /// 雾灯亮
            /// </summary>
            [MatrixValDefinition(53, 1, 1)]
            [Description("0x0A")]
            FogLampOn,

            /// <summary>
            /// 雾灯灭
            /// </summary>
            [MatrixValDefinition(53, 1, 0)]
            [Description("0x0A")]
            FogLampOff,

            /// <summary>
            /// 制动灯灭
            /// </summary>
            [MatrixValDefinition(32, 1, 0)]
            [Description("0x0A")]
            SbbrBrlAnfOff,

            /// <summary>
            /// 制动灯亮
            /// </summary>
            [MatrixValDefinition(32, 1, 1)]
            [Description("0x0A")]
            SbbrBrlAnfOn,

            /// <summary>
            /// 计数器
            /// </summary>
            [MatrixValDefinition(8, 4, 0)]
            [Description("0x0A")]
            BcmSbbr01Bz,

            /// <summary>
            /// 左转向
            /// </summary>
            [MatrixValDefinition(16, 1, 0)]
            [Description("0x0A")]
            SbbrFraWiBiLinksOff,

            /// <summary>
            /// 左转向
            /// </summary>
            [MatrixValDefinition(16, 1, 1)]
            [Description("0x0A")]
            SbbrFraWiBiLinksOn,

            /// <summary>
            /// 左转向
            /// </summary>
            [MatrixValDefinition(17, 1, 0)]
            [Description("0x0A")]
            SbbrFraWiBiRechtsOff,

            /// <summary>
            /// 左转向
            /// </summary>
            [MatrixValDefinition(17, 1, 1)]
            [Description("0x0A")]
            SbbrFraWiBiRechtsOn,

            /// <summary>
            /// 左转向
            /// </summary>
            [MatrixValDefinition(20, 1, 0)]
            [Description("0x0A")]
            SbbrFraLinksOff,

            /// <summary>
            /// 左转向
            /// </summary>
            [MatrixValDefinition(20, 1, 1)]
            [Description("0x0A")]
            SbbrFraLinksOn,

            /// <summary>
            /// 左转向
            /// </summary>
            [MatrixValDefinition(21, 1, 0)]
            [Description("0x0A")]
            SbbrFraRechtsOff,

            /// <summary>
            /// 左转向
            /// </summary>
            [MatrixValDefinition(21, 1, 1)]
            [Description("0x0A")]
            SbbrFraRechtsOn,

            /// <summary>
            /// 左转向
            /// </summary>
            [MatrixValDefinition(22, 2, 0)]
            [Description("0x0A")]
            SbbrFraWiBiAnimationOff,

            /// <summary>
            /// 左转向
            /// </summary>
            [MatrixValDefinition(22, 1, 1)]
            [Description("0x0A")]
            SbbrFraWiBiAnimationOn,

            [MatrixValDefinition(24, 1, 0)]
            [Description("0x0A")]
            Bcm1ChAktivOff,

            [MatrixValDefinition(24, 1, 1)]
            [Description("0x0A")]
            Bcm1ChAktivOn,

            [MatrixValDefinition(25, 1, 0)]
            [Description("0x0A")]
            Bcm1LhAktivOff,

            [MatrixValDefinition(25, 1, 1)]
            [Description("0x0A")]
            Bcm1LhAktivOn,

            [MatrixValDefinition(26, 4, 0)]
            [Description("0x0A")]
            SbbrVarianteOff,

            [MatrixValDefinition(26, 4, 0)]
            [Description("0x0A")]
            SbbrVarianteOn,

            [MatrixValDefinition(50, 1, 1)]
            [Description("0x0A")]
            SbbrParklichtLiAnfOn,

            [MatrixValDefinition(51, 1, 1)]
            [Description("0x0A")]
            SbbrParklichtReAnfOn,

            [MatrixValDefinition(50, 1, 0)]
            [Description("0x0A")]
            SbbrParklichtLiAnfOff,

            [MatrixValDefinition(51, 1, 0)]
            [Description("0x0A")]
            SbbrParklichtReAnfOff,
        }
        #endregion
    }
}
