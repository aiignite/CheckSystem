using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using CommonUtility;
using CommonUtility.BusLoader;
using CommonUtility.FileOperator;
using DBUtility;

namespace Controller
{
    [Description("CAN-Product,A12前灯控制模块")]
    public sealed class A12FrontLampControlModule : ControllerBase
    {
        public CanBus Can;

        #region Fields

        /// <summary>
        /// 读取模式配置
        /// </summary>
        [Description("R,读取模式配置")]
        public string ModeStr;

        /// <summary>
        /// 读取节点配置
        /// </summary>
        [Description("R,读取节点配置")]
        public string LrdfStr;

        /// <summary>
        /// 条码
        /// </summary>
        [Description("R,条码")]
        public string BarcodeStr;

        /// <summary>
        /// 总成零件号
        /// </summary>
        [Description("R,总成零件号")]
        public string HwPn;

        /// <summary>
        /// 引导程序零件号
        /// </summary>
        [Description("R,引导程序零件号")]
        public string BootAppSwPn;

        /// <summary>
        /// 引导程序版本号
        /// </summary>
        [Description("R,引导程序版本号")]
        public string BootAppSwVer;

        /// <summary>
        /// 应用程序零件号
        /// </summary>
        [Description("R,应用程序零件号")]
        public string AppSwPn;

        /// <summary>
        /// 配置文件零件号
        /// </summary>
        [Description("R,配置文件零件号")]
        public string CfgPn;

        /// <summary>
        /// 应用程序版本号
        /// </summary>
        [Description("R,应用程序版本号")]
        public string AppSwVer;

        /// <summary>
        /// 配置文件版本号
        /// </summary>
        [Description("R,配置文件版本号")]
        public string CfgVer;

        /// <summary>
        /// 生产追溯信息
        /// </summary>
        [Description("R,生产追溯信息")]
        public string ProductData;

        /// <summary>
        /// 软件指纹
        /// </summary>
        [Description("R,软件指纹")]
        public string SwThumbPrint;

        /// <summary>
        /// 编程日期
        /// </summary>
        [Description("R,编程日期")]
        public string ProgramDate;

        [Description("R/W,生产追溯信息-生产线编号")]
        public string ProductionLineNumber = 1.ToString();

        //[Description("R/W,ProductNo")]
        //public string ProductNo = string.Empty;

        [Description("R/W,ProductNo")]
        public string ProductNo = "P00109171";

        #endregion

        public A12FrontLampControlModule(string name)
            : base(name)
        {
            foreach (var temp
                in Enum.GetValues(typeof(LightOnOffType)).Cast<LightOnOffType>())
                _lightOperaterDic.Add(temp, temp.GetCustomAttribute<MatrixValDefinition>());

            _motorolaMatrix0X25D.UpdateData(
                _lightOperaterDic[LightOnOffType.PowerModeRun]);
            _motorolaMatrix0X260.UpdateData(
                _lightOperaterDic[LightOnOffType.VehSpdVValid]);
            _motorolaMatrix0X420.UpdateData(
                _lightOperaterDic[LightOnOffType.Wakup]);

            if (_mainWorkThread != null)
            {
                _mainWorkThread.Abort();
                _mainWorkThread.Join();
            }

            _mainWorkThread = new Thread(MainWork) { IsBackground = true };
            _mainWorkThread.Start();
        }

        ~A12FrontLampControlModule()
        {
            Dispose();
        }

        private readonly Thread _mainWorkThread;
        private uint _currentReqCanId = 0x721;
        private uint _currentRecvCanId = 0x7A1;
        private bool _isLeft = true;
        private bool _isSleeping = true;
        private bool _isInExtendedSession;
        private int _periodCount;
        private byte _msgCount;
        private readonly Dictionary<LightOnOffType, MatrixValDefinition> _lightOperaterDic =
           new Dictionary<LightOnOffType, MatrixValDefinition>();

        private readonly CanCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X25D =
            new CanCommunicationMatrix.MotorolaMatrix(0x25D, 8);
        private readonly CanCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X306 =
            new CanCommunicationMatrix.MotorolaMatrix(0x306, 8);
        private readonly CanCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X260 =
            new CanCommunicationMatrix.MotorolaMatrix(0x260, 10);
        private readonly CanCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X350 =
            new CanCommunicationMatrix.MotorolaMatrix(0x350, 10);
        private readonly CanCommunicationMatrix.MotorolaMatrix _motorolaMatrix0X420 =
            new CanCommunicationMatrix.MotorolaMatrix(0x420, 4);

        #region LED 相关

        private void MainWork()
        {
            while (_mainWorkThread.IsAlive)
            {
                if (!_mainWorkThread.IsAlive)
                    break;

                Thread.Sleep(4);

                if (Can == null || _isSleeping)
                {
                    _isInExtendedSession = false;
                    _periodCount = 0;
                    continue;
                }

                try
                {
                    if (_periodCount % 2 == 0) // Period = 8ms
                    {
                        Can.SendStandardCanData(
                            _motorolaMatrix0X260.CanId, _motorolaMatrix0X260.MatrixData);
                        Can.SendStandardCanData(
                            _motorolaMatrix0X350.CanId, _motorolaMatrix0X350.MatrixData);
                        Can.SendStandardCanData(
                            _motorolaMatrix0X420.CanId, _motorolaMatrix0X420.MatrixData);
                    }

                    if (_periodCount % 5 == 0) // Period = 20ms
                    {
                        Can.SendStandardCanData(
                            _motorolaMatrix0X25D.CanId, _motorolaMatrix0X25D.MatrixData);
                    }

                    if (_periodCount % 10 == 0) // Period = 40ms
                    {
                        UpdateMsgCount();
                        UpdateCheckSum();
                        Can.SendStandardCanData(
                            _motorolaMatrix0X306.CanId, _motorolaMatrix0X306.MatrixData);
                    }

                    if (_periodCount % 100 == 0 && _isInExtendedSession) // Period = 1000ms
                    {
                        //Can.SendStandardCanData(
                        //        _currentReqCanId, new byte[] { 0x02, 0x3E, 0x00, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA });

                        Can.SendStandardCanData(
                                _currentReqCanId, CanBus.MyUds.KeepExtendedSessionBytes(0x00, 0xAA));
                    }

                    _periodCount++;
                    if (_periodCount > 10000)
                        _periodCount = 0;
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private void UpdateMsgCount()
        {
            _msgCount++;
            if (_msgCount > 15)
                _msgCount = 0;
            _lightOperaterDic[LightOnOffType.MsgConunt].Value = _msgCount;
            _motorolaMatrix0X306.UpdateData(_lightOperaterDic[LightOnOffType.MsgConunt]);
        }

        private void UpdateCheckSum()
        {
            var num = 0x000;
            for (var i = 0; i < 7; i++)
                num = num + _motorolaMatrix0X306.MatrixData[i];
            var lo = (byte)(num & 0xFF); // 取低8位
            var loOr = (byte)(lo ^ 0xFF); // 低8位再取反
            _lightOperaterDic[LightOnOffType.MsgcheckSum].Value = loOr;
            _motorolaMatrix0X306.UpdateData(_lightOperaterDic[LightOnOffType.MsgcheckSum]);
        }

        [Description("切换成右节点")]
        public void SwitchRight()
        {
            _currentReqCanId = 0x732;
            _currentRecvCanId = 0x7B2;
        }

        [Description("切换成左节点")]
        public void SwitchLeft()
        {
            _currentReqCanId = 0x721;
            _currentRecvCanId = 0x7A1;
        }

        [Description("模块休眠")]
        public void EnterSleepMode()
        {
            _msgCount = 0;
            _isSleeping = true;
        }

        [Description("模块唤醒")]
        public void ExitSleepMode()
        {
            _msgCount = 0;
            _isSleeping = false;
        }

        [Description("配置为普通模式")]
        public void WriteNormalMode()
        {
            if (Can == null)
                return;

            if (Can.CanBusWithUds.TryWriteData(
                _currentReqCanId, _currentRecvCanId,
                CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0x40, 0x0F,
                new byte[] { 0x01 }))
                return;
            Thread.Sleep(500);
            Can.CanBusWithUds.TryWriteData(
                _currentReqCanId, _currentRecvCanId,
                CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0x40, 0x0F,
                new byte[] { 0x01 });
        }

        [Description("配置为测试模式")]
        public void WriteTestMode()
        {
            if (Can == null)
                return;

            if (Can.CanBusWithUds.TryWriteData(
                _currentReqCanId, _currentRecvCanId,
                CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0x40, 0x0F,
                new byte[] { 0x00 }))
                return;
            Thread.Sleep(500);
            Can.CanBusWithUds.TryWriteData(
                _currentReqCanId, _currentRecvCanId,
                CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0x40, 0x0F,
                new byte[] { 0x00 });
        }

        [Description("进入拓展")]
        public void EnterExtendedSession()
        {
            if (Can == null)
                return;

            if (
                !Can.CanBusWithUds.TryEnterExtendedSession(_currentReqCanId, _currentRecvCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can))
            {
                Thread.Sleep(500);
                if (Can.CanBusWithUds.TryEnterExtendedSession(_currentReqCanId, _currentRecvCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can))
                    _isInExtendedSession = true;
            }
            else
                _isInExtendedSession = true;
        }

        [Description("退出拓展")]
        public void ExitExtendedSession()
        {
            if (Can == null)
                return;

            _isInExtendedSession = false;

            if (Can.CanBusWithUds.TryEnterDefaultSession(_currentReqCanId, _currentRecvCanId,
                CanBus.CanType.Standard, CanBus.CanProtocol.Can))
                return;

            Thread.Sleep(500);
            Can.CanBusWithUds.TryEnterDefaultSession(_currentReqCanId, _currentRecvCanId,
                CanBus.CanType.Standard, CanBus.CanProtocol.Can);
        }

        [Description("配置成工厂状态")]
        public void WriteFactoryState()
        {
            if (Can == null)
                return;

            if (Can.CanBusWithUds.TryWriteData(
                _currentReqCanId, _currentRecvCanId,
                CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0x01, 0x40,
                new byte[] { 0x00 }))
                return;
            Thread.Sleep(500);
            Can.CanBusWithUds.TryWriteData(
                _currentReqCanId, _currentRecvCanId,
                CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0x01, 0x40,
                new byte[] { 0x00 });
        }

        [Description("测试模式下打开所有LED")]
        public void AllControlOn()
        {
            if (Can == null)
                return;

            Can.SendStandardCanData(_currentReqCanId, new byte[] { 0x06, 0x2F, 0x50, 0x94, 0x03, 0x3F, 0x07, 0xAA });
        }

        [Description("读取模式配置")]
        public string ReadCurrMode()
        {
            ModeStr = string.Empty;

            if (Can == null)
                return string.Empty;

            try
            {
                var readResult = ReadDid(_currentReqCanId, _currentRecvCanId, 0x40, 0x0F);
                if (readResult.Any() && (readResult[0] == 0x0F || readResult[0] == 0x00))
                    ModeStr = "测试模式 " + ValueHelper.GetHextStrWithOx(readResult[0]);
                else
                    ModeStr = "正常模式 " + ValueHelper.GetHextStrWithOx(readResult[0]);
            }
            catch (Exception)
            {
                // ignored
            }

            return ModeStr;
        }

        [Description("读取节点配置")]
        public string ReadLrdf()
        {
            LrdfStr = string.Empty;

            if (Can == null)
                return string.Empty;

            var readResult = ReadDid(_currentReqCanId, _currentRecvCanId, 0x40, 0x05);
            if (!readResult.Any())
                return LrdfStr;

            if (readResult[0] == 0x00)
            {
                LrdfStr = "左节点 " + ValueHelper.GetHextStrWithOx(readResult[0]);
                _isLeft = true;
            }
            else if (readResult[0] == 0x01)
            {
                LrdfStr = "右节点 " + ValueHelper.GetHextStrWithOx(readResult[0]);
                _isLeft = false;
            }
            else
                LrdfStr = "未配置节点 " + ValueHelper.GetHextStrWithOx(readResult[0]);

            return LrdfStr;
        }

        [Description("打开远光HB")]
        public void HighBmOn()
        {
            SwthchHighBm(true);
        }

        [Description("关闭远光HB")]
        public void HighBmOff()
        {
            SwthchHighBm(false);
        }

        private void SwthchHighBm(bool isOn)
        {
            _motorolaMatrix0X306.UpdateData(isOn
                ? _lightOperaterDic[LightOnOffType.HighBmOn]
                : _lightOperaterDic[LightOnOffType.HighBmOff]);
        }

        [Description("打开PL")]
        public void PlOn()
        {
            SwitchPl(true);
        }

        [Description("关闭PL")]
        public void PlOff()
        {
            SwitchPl(false);
        }

        private void SwitchPl(bool isOn)
        {
            _motorolaMatrix0X306.UpdateData(isOn
               ? _lightOperaterDic[LightOnOffType.PlOn]
               : _lightOperaterDic[LightOnOffType.PlOff]);
        }

        [Description("打开DRL")]
        public void DrlOn()
        {
            SwitchDrl(true);
        }

        [Description("关闭DRL")]
        public void DrlOff()
        {
            SwitchDrl(false);
        }

        private void SwitchDrl(bool isOn)
        {
            _motorolaMatrix0X306.UpdateData(isOn
               ? _lightOperaterDic[LightOnOffType.DrlOn]
               : _lightOperaterDic[LightOnOffType.DrlOff]);
        }

        [Description("打开左近光L_LB")]
        public void LftLowBmOn()
        {
            SwitchLowBm(true);
        }

        [Description("关闭左近光L_LB")]
        public void LftLowBmOff()
        {
            SwitchLowBm(false);
        }

        private void SwitchLowBm(bool isOn)
        {
            _motorolaMatrix0X306.UpdateData(isOn
               ? _lightOperaterDic[LightOnOffType.LowBmOn]
               : _lightOperaterDic[LightOnOffType.LowBmOff]);
        }

        [Description("打开左转向L_TURN")]
        public void LftTrnOn()
        {
            SwitchLftTrn(true);
        }

        [Description("关闭左转向L_TURN")]
        public void LftTrnOff()
        {
            SwitchLftTrn(false);
        }

        private void SwitchLftTrn(bool isOn)
        {
            _motorolaMatrix0X306.UpdateData(isOn
               ? _lightOperaterDic[LightOnOffType.LeftTrnOn]
               : _lightOperaterDic[LightOnOffType.LeftTrnOff]);
        }

        [Description("打开右转向R_TURN")]
        public void RightTrnOn()
        {
            SwitchRightTrn(true);
        }

        [Description("关闭右转向R_TURN")]
        public void RightTrnOff()
        {
            SwitchRightTrn(false);
        }

        private void SwitchRightTrn(bool isOn)
        {
            _motorolaMatrix0X306.UpdateData(isOn
              ? _lightOperaterDic[LightOnOffType.RightTrnOn]
              : _lightOperaterDic[LightOnOffType.RightTrnOff]);
        }

        #endregion

        #region 确认追溯（内部）

        /// <summary>
        /// 读引导程序零件号
        /// </summary>
        [Description("读引导程序零件号")]
        public void ReadBootAppSwPn()
        {
            BootAppSwPn = string.Empty;
            var result = ReadDid(_currentReqCanId, _currentRecvCanId, 0x01, 0xC0);
            if (result.Length != 5) return;
            var str1 = Encoding.ASCII.GetString(new[] { result[0], result[1] });
            var str2 = ValueHelper.GetDecimal(new[] { result[2], result[3], result[4] }).ToString().PadLeft(7, '0');
            BootAppSwPn = str1 + str2;
        }

        /// <summary>
        /// 读引导程序版本号
        /// </summary>
        [Description("读引导程序版本号")]
        public void ReadBootAppSwVer()
        {
            BootAppSwVer = string.Empty;
            var result = ReadDid(_currentReqCanId, _currentRecvCanId, 0x01, 0xD0);
            BootAppSwVer = result.GetStringByAsciiBytes(false); // B003
        }

        /// <summary>
        /// 读总成零件号
        /// </summary>
        [Description("读总成零件号")]
        public void ReadHwPn()
        {
            HwPn = string.Empty;
            var result = ReadDid(_currentReqCanId, _currentRecvCanId, 0x40, 0x0C);
            HwPn = result.GetStringByAsciiBytes(false); // P00109171
        }

        /// <summary>
        /// 读应用程序零件号
        /// </summary>
        [Description("读应用程序零件号")]
        public void ReadAppSwPn()
        {
            AppSwPn = string.Empty;
            var result = ReadDid(_currentReqCanId, _currentRecvCanId, 0x01, 0xC1);
            AppSwPn = "SW" + ValueHelper.GetDecimal(result).ToString().PadLeft(7, '0'); // SW0000354
        }

        /// <summary>
        /// 读配置文件零件号
        /// </summary>
        [Description("读配置文件零件号")]
        public void ReadCfgPn()
        {
            CfgPn = string.Empty;
            var result = ReadDid(_currentReqCanId, _currentRecvCanId, 0x01, 0xC2);
            CfgPn = "SW" + ValueHelper.GetDecimal(result).ToString().PadLeft(7, '0'); // SW0000356
        }

        /// <summary>
        /// 读应用程序版本号
        /// </summary>
        [Description("读应用程序版本号")]
        public void ReadAppSwVer()
        {
            AppSwVer = string.Empty;
            var result = ReadDid(_currentReqCanId, _currentRecvCanId, 0x01, 0xD1);
            AppSwVer = result.GetStringByAsciiBytes(false); // A013
        }

        /// <summary>
        /// 配置文件版本号
        /// </summary>
        [Description("配置文件版本号")]
        public void ReadCfgVer()
        {
            CfgVer = string.Empty;
            var result = ReadDid(_currentReqCanId, _currentRecvCanId, 0x01, 0xD2);
            CfgVer = result.GetStringByAsciiBytes(false); // C013
        }

        /// <summary>
        /// 读生产追溯信息
        /// </summary>
        [Description("读生产追溯信息")]
        public void ReadProductData()
        {
            ProductData = string.Empty;
            ProductData = "NG";

            //if (string.IsNullOrEmpty(BarcodeStr))
            //    return;
            var result = ReadDid(_currentReqCanId, _currentRecvCanId, 0xF1, 0x8C);
            if (result.Any())
            {
                ProductData = "OK ";
                foreach (var item in result)
                {
                    ProductData += ValueHelper.GetHextStr(item);
                }
            }
        }

        /// <summary>
        /// 读编程日期
        /// </summary>
        [Description("读编程日期")]
        public void ReadProgramDate()
        {
            ProgramDate = string.Empty;
            ProgramDate = "NG";
            var result = ReadDid(_currentReqCanId, _currentRecvCanId, 0xF1, 0x99);

            if (!result.Any())
                return;
            ProgramDate = "OK ";

            foreach (var t in result)
                ProgramDate += ValueHelper.GetHextStr(t);
        }

        /// <summary>
        /// 读软件指纹
        /// </summary>
        [Description("读软件指纹")]
        public void ReadSwThumbPrint()
        {
            SwThumbPrint = string.Empty;
            SwThumbPrint = "NG";
            var result = ReadDid(_currentReqCanId, _currentRecvCanId, 0xF1, 0x84);

            if (!result.Any())
                return;
            SwThumbPrint = "OK ";

            foreach (var t in result)
                SwThumbPrint += ValueHelper.GetHextStr(t);
        }

        #endregion

        #region 确认追溯（广汽GAC）

        /// <summary>
        /// 广汽引导程序版本号
        /// </summary>
        [Description("R,广汽引导程序版本号")]
        public string GacBootAppSwVer;

        /// <summary>
        /// 应用程序版本号
        /// </summary>
        [Description("R,应用程序版本号")]
        public string GacAppSwVer;

        /// <summary>
        /// 配置文件版本号
        /// </summary>
        [Description("R,配置文件版本号")]
        public string GacCfgVer;

        /// <summary>
        /// 读广汽版本引导程序版本号
        /// </summary>
        [Description("读广汽版本引导程序版本号")]
        public void ReadGacBootAppSwVer()
        {
            GacBootAppSwVer = string.Empty;
            var result = ReadDid(_currentReqCanId, _currentRecvCanId, 0xF1, 0x80);
            GacBootAppSwVer = result.GetStringByAsciiBytes(false); // 8050011ACN00B.B00
        }

        /// <summary>
        /// 读广汽版本应用程序版本号
        /// </summary>
        [Description("读广汽版本应用程序版本号")]
        public void ReadGacAppSwVer()
        {
            GacAppSwVer = string.Empty;
            var result = ReadDid(_currentReqCanId, _currentRecvCanId, 0xF1, 0x81);
            GacAppSwVer = result.GetStringByAsciiBytes(false); // 8050011ACN00A.001
        }

        /// <summary>
        /// 读广汽版本配置文件版本号
        /// </summary>
        [Description("读广汽版本配置文件版本号")]
        public void ReadGacCfgVer()
        {
            GacCfgVer = string.Empty;
            var result = ReadDid(_currentReqCanId, _currentRecvCanId, 0xF1, 0xB2);
            GacCfgVer = result.GetStringByAsciiBytes(false); // 8050011ACN00C.001
        }

        #endregion

        #region 读采样电压

        [Description("R,RBIN_1采样电压")]
        public double RBin1Volt;

        [Description("R,RBIN_2采样电压")]
        public double RBin2Volt;

        [Description("R,RBIN_3采样电压")]
        public double RBin3Volt;

        [Description("R,RBIN_4采样电压")]
        public double RBin4Volt;

        [Description("R,RBIN_5采样电压")]
        public double RBin5Volt;

        [Description("R,RBIN_6采样电压")]
        public double RBin6Volt;

        [Description("读RBIN采样电压")]
        public void ReadRBinVolt(int index)
        {
            if (Can == null)
                return;

            var did = new byte[2];
            switch (index)
            {
                case 1:
                    RBin1Volt = 9999;
                    did = new byte[] { 0x11, 0x00 };
                    break;

                case 2:
                    RBin2Volt = -9999;
                    did = new byte[] { 0x11, 0x01 };
                    break;

                case 3:
                    RBin3Volt = -9999;
                    did = new byte[] { 0x11, 0x02 };
                    break;

                case 4:
                    RBin4Volt = -9999;
                    did = new byte[] { 0x11, 0x03 };
                    break;

                case 5:
                    RBin5Volt = -9999;
                    did = new byte[] { 0x11, 0x04 };
                    break;

                case 6:
                    RBin6Volt = -9999;
                    did = new byte[] { 0x11, 0x05 };
                    break;
            }

            var result = ReadDid(_currentReqCanId, _currentRecvCanId, did[0], did[1]);
            if (result.Length == 2)
            {
                switch (index)
                {
                    case 1:
                        RBin1Volt = (result[0] * 256.0 + result[1]) / 1024.0 * 5.0;
                        break;

                    case 2:
                        RBin2Volt = (result[0] * 256.0 + result[1]) / 1024.0 * 5.0;
                        break;

                    case 3:
                        RBin3Volt = (result[0] * 256.0 + result[1]) / 1024.0 * 5.0;
                        break;

                    case 4:
                        RBin4Volt = (result[0] * 256.0 + result[1]) / 1024.0 * 5.0;
                        break;

                    case 5:
                        RBin5Volt = (result[0] * 256.0 + result[1]) / 1024.0 * 5.0;
                        break;

                    case 6:
                        RBin6Volt = (result[0] * 256.0 + result[1]) / 1024.0 * 5.0;
                        break;
                }
            }
        }

        [Description("R,NTC_1采样电压")]
        public double Ntc1Volt;

        [Description("R,NTC_3采样电压")]
        public double Ntc3Volt;

        [Description("R,NTC_4采样电压")]
        public double Ntc4Volt;

        [Description("R,NTC_5采样电压")]
        public double Ntc5Volt;

        [Description("读NTC采样电压")]
        public void ReadNtcVolt(int index)
        {
            if (Can == null)
                return;

            var did = new byte[2];
            switch (index)
            {
                case 1:
                    Ntc1Volt = -9999;
                    did = new byte[] { 0x11, 0x20 };
                    break;

                case 2:
                    Ntc3Volt = -9999;
                    did = new byte[] { 0x11, 0x21 };
                    break;

                case 3:
                    Ntc4Volt = -9999;
                    did = new byte[] { 0x11, 0x22 };
                    break;

                case 4:
                    Ntc5Volt = -9999;
                    did = new byte[] { 0x11, 0x23 };
                    break;
            }

            var result = ReadDid(_currentReqCanId, _currentRecvCanId, did[0], did[1]);
            if (result.Length == 2)
            {
                switch (index)
                {
                    case 1:
                        Ntc1Volt = (result[0] * 256.0 + result[1]) / 1024.0 * 5.0;
                        break;

                    case 2:
                        Ntc3Volt = (result[0] * 256.0 + result[1]) / 1024.0 * 5.0;
                        break;

                    case 3:
                        Ntc4Volt = (result[0] * 256.0 + result[1]) / 1024.0 * 5.0;
                        break;

                    case 4:
                        Ntc5Volt = (result[0] * 256.0 + result[1]) / 1024.0 * 5.0;
                        break;
                }
            }
        }

        #endregion

        #region 读写电感

        public double Led1Current;
        public double Led2Current;
        public double Led3Current;
        public double Led4Current;
        public double Led5Current;
        public double Led6Current;

        public double Led1K;
        public double Led1B;
        public double Led2K;
        public double Led2B;
        public double Led3K;
        public double Led3B;
        public double Led4K;
        public double Led4B;
        public double Led5K;
        public double Led5B;
        public double Led6K;
        public double Led6B;

        [Description("R,电感1读取值")]
        public double Inductance1;
        [Description("R,电感2读取值")]
        public double Inductance2;
        [Description("R,电感3读取值")]
        public double Inductance3;
        [Description("R,电感4读取值")]
        public double Inductance4;
        [Description("R,电感5读取值")]
        public double Inductance5;
        [Description("R,电感6读取值")]
        public double Inductance6;

        public void SetInductance()
        {
            if (Can == null)
                return;

            Led1K = 0.0001 * Led1K;
            Led1B = 0.0001 * Led1B;
            Led2K = 0.0001 * Led2K;
            Led2B = 0.0001 * Led2B;
            Led3K = 0.0001 * Led3K;
            Led3B = 0.0001 * Led3B;

            Led4K = 0.0001 * Led4K;
            Led4B = 0.0001 * Led4B;
            Led5K = 0.0001 * Led5K;
            Led5B = 0.0001 * Led5B;
            Led6K = 0.0001 * Led6K;
            Led6B = 0.0001 * Led6B;

            var led1Curr = Convert.ToUInt16(Led1Current * Led1K + Led1B);
            var led2Curr = Convert.ToUInt16(Led2Current * Led2K + Led2B);

            var led3Curr = Convert.ToUInt16(Led3Current * Led3K + Led3B);
            var led4Curr = Convert.ToUInt16(Led4Current * Led4K + Led4B);

            var led5Curr = Convert.ToUInt16(Led5Current * Led5K + Led5B);
            var led6Curr = Convert.ToUInt16(Led6Current * Led6K + Led6B);

            var led1Bytes = BitConverter.GetBytes(led1Curr);
            var led2Bytes = BitConverter.GetBytes(led2Curr);
            var led3Bytes = BitConverter.GetBytes(led3Curr);
            var led4Bytes = BitConverter.GetBytes(led4Curr);
            var led5Bytes = BitConverter.GetBytes(led5Curr);
            var led6Bytes = BitConverter.GetBytes(led6Curr);

            Array.Reverse(led1Bytes);
            Array.Reverse(led2Bytes);
            Array.Reverse(led3Bytes);
            Array.Reverse(led4Bytes);
            Array.Reverse(led5Bytes);
            Array.Reverse(led6Bytes);

            ReadInductance();

            if (Inductance1 == -9999 && Inductance2 == -9999)
            {
                Thread.Sleep(25);

                if (
                !Can.CanBusWithUds.TryStartRoutineControl(_currentReqCanId, _currentRecvCanId,
                    CanBus.CanType.Standard, Uds14229Helper.RoutineControl.Unknown1,
                    new[] { led1Bytes[0], led1Bytes[1], led2Bytes[0], led2Bytes[01] }))
                {
                    //Thread.Sleep(500);
                    //Can.CanBusWithUds.TryStartRoutineControl(_currentReqCanId, _currentRecvCanId,
                    //    CanBus.CanType.Standard, Uds14229Helper.RoutineControl.Unknown1,
                    //    new[] { led1Bytes[0], led1Bytes[1], led2Bytes[0], led2Bytes[01] });
                }
            }

            if (Inductance3 == -9999 && Inductance4 == -9999)
            {
                Thread.Sleep(25);

                if (
                    !Can.CanBusWithUds.TryStartRoutineControl(_currentReqCanId, _currentRecvCanId,
                        CanBus.CanType.Standard, Uds14229Helper.RoutineControl.Unknown2,
                        new[] { led3Bytes[0], led3Bytes[1], led4Bytes[0], led4Bytes[01] }))
                {
                    //Thread.Sleep(500);
                    //Can.CanBusWithUds.TryStartRoutineControl(_currentReqCanId, _currentRecvCanId,
                    //    CanBus.CanType.Standard, Uds14229Helper.RoutineControl.Unknown2,
                    //    new[] { led3Bytes[0], led3Bytes[1], led4Bytes[0], led4Bytes[01] });
                }
            }

            if (Inductance5 == -9999 && Inductance6 == -9999)
            {
                Thread.Sleep(25);

                if (
                    !Can.CanBusWithUds.TryStartRoutineControl(_currentReqCanId, _currentRecvCanId,
                        CanBus.CanType.Standard, Uds14229Helper.RoutineControl.Unknown3,
                        new[] { led5Bytes[0], led5Bytes[1], led6Bytes[0], led6Bytes[01] }))
                {
                    //Thread.Sleep(500);
                    //Can.CanBusWithUds.TryStartRoutineControl(_currentReqCanId, _currentRecvCanId,
                    //    CanBus.CanType.Standard, Uds14229Helper.RoutineControl.Unknown3,
                    //    new[] { led5Bytes[0], led5Bytes[1], led6Bytes[0], led6Bytes[01] });
                }
            }
        }

        [Description("读取电感")]
        public void ReadInductance()
        {
            Inductance1 = -9999;
            Inductance2 = -9999;
            Inductance3 = -9999;
            Inductance4 = -9999;
            Inductance5 = -9999;
            Inductance6 = -9999;

            if (Can == null)
                return;

            var read12Func = new Func<byte[]>(() =>
            {
                byte[] readBytes;
                Can.CanBusWithUds.TryReadData(
                    _currentReqCanId, _currentRecvCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    0x14, 0x06,
                    out readBytes);
                return readBytes;
            });

            var read12 = read12Func.Invoke() ?? read12Func.Invoke();
            if (read12 != null && read12.Length >= 5)
                if (read12[0] == 1)
                {
                    Inductance1 = read12[1] * 256 + read12[2];
                    Inductance2 = read12[3] * 256 + read12[4];
                }

            var read34Func = new Func<byte[]>(() =>
            {
                byte[] readBytes;
                Can.CanBusWithUds.TryReadData(
                    _currentReqCanId, _currentRecvCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    0x14, 0x07,
                    out readBytes);
                return readBytes;
            });

            var read34 = read34Func.Invoke() ?? read34Func.Invoke();
            if (read34 != null && read34.Length >= 5)
                if (read34[0] == 1)
                {
                    Inductance3 = read34[1] * 256 + read34[2];
                    Inductance4 = read34[3] * 256 + read34[4];
                }

            var read56Func = new Func<byte[]>(() =>
            {
                byte[] readBytes;
                Can.CanBusWithUds.TryReadData(
                    _currentReqCanId, _currentRecvCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can,
                    0x14, 0x08,
                    out readBytes);
                return readBytes;
            });

            var read56 = read56Func.Invoke() ?? read56Func.Invoke();
            if (read56 != null && read56.Length >= 5)
                if (read56[0] == 1)
                {
                    Inductance5 = read56[1] * 256 + read56[2];
                    Inductance6 = read56[3] * 256 + read56[4];
                }
        }

        #endregion

        #region 安全解锁

        /// <summary>
        /// 左节点安全解锁算法
        /// </summary>
        /// <param name="seedBytes"></param>
        /// <param name="securetyLevel"></param>
        /// <returns></returns>
        public IEnumerable<byte> CalcKey(byte[] seedBytes, int securetyLevel)
        {
            var keyBytes = new byte[4];
            int keyLen;

            if (_isLeft)
                FllcuCalcKey.GenerateKeyEx(seedBytes, 4, securetyLevel, null, keyBytes, 4, out keyLen);
            else
                FrlcuCalcKey.GenerateKeyEx(seedBytes, 4, securetyLevel, null, keyBytes, 4, out keyLen);

            return keyBytes;
        }

        /// <summary>
        /// 左节点安全解锁算法
        /// </summary>
        internal static class FllcuCalcKey
        {
            [DllImport(@"\DllImport\GAC_A12_FLLCU_HASCOVISION_SA.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr GenerateKeyEx(
                byte[] iSeedArray, /* Array for the seed [in] */
                short iSeedArraySize, /* Length of the array for the seed [in] */
                int iSecurityLevel, /* Security level [in] */
                byte[] iVariant, /* Name of the active variant [in] */
                byte[] ioKeyArray, /* Array for the key [in, out] */
                int iKeyArraySize, /* Maximum length of the array for the key [in] */
                out int oSize /* Length of the key [out] */);
        }

        /// <summary>
        /// 右节点安全解锁算法
        /// </summary>
        internal static class FrlcuCalcKey
        {
            [DllImport(@"\DllImport\GAC_A12_FRLCU_HASCOVISION_SA.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr GenerateKeyEx(
                byte[] iSeedArray, /* Array for the seed [in] */
                short iSeedArraySize, /* Length of the array for the seed [in] */
                int iSecurityLevel, /* Security level [in] */
                byte[] iVariant, /* Name of the active variant [in] */
                byte[] ioKeyArray, /* Array for the key [in, out] */
                int iKeyArraySize, /* Maximum length of the array for the key [in] */
                out int oSize /* Length of the key [out] */);
        }
        #endregion

        #region 程序下载

        //public string DrvFlashFilePath = string.Empty;
        //public string AppFilePath = string.Empty;
        //public string CfgFilePath = string.Empty;

        //[Description("R,APP下载结果")]
        //public string DownLoadAppResult = string.Empty;

        //[Description("R,CFG下载结果")]
        //public string DownLoadCfgResult = string.Empty;

        //[Description("R,APP和配置文件下载结果")]
        //public string DownLoadAppCfgResult = string.Empty;

        [Description("R,下载结果")]
        public string DownloadResult = string.Empty;

        public string DrvFlashFilePath = @"D:\Projects\A12&A13前灯模块检测\A13程序\DrvFlash_Xc2000.sx";

        //public string AppFilePath = @"D:\Projects\A12&A13前灯模块检测\A13程序\S001\SW0000948_X002_APP.sx";
        //public string CfgFilePath = @"D:\Projects\A12&A13前灯模块检测\A13程序\S001\SW0000949_Z003_CFG.sx";

        //public readonly string AppFilePath = @"D:\Projects\A12&A13前灯模块检测\A13程序\S002\SW0000948_A002_APP.sx";
        //public readonly string CfgFilePath = @"D:\Projects\A12&A13前灯模块检测\A13程序\S002\SW0000949_C002_CFG.sx";

        public readonly string AppFilePath = @"D:\Projects\A12&A13前灯模块检测\A12程序\调试1\SW0000354.A013.sx";
        public readonly string CfgFilePath = @"D:\Projects\A12&A13前灯模块检测\A12程序\调试1\SW0000356.C013.sx";

        private static readonly object LockFile = new object();

        [Description("下载")]
        public void DownLoadFile()
        {
            DownloadResult = string.Empty;

            if (Can == null)
                DownloadResult = "NG CAN未初始化";

            var fileList = new List<string>();

            lock (LockFile)
            {
                if (!string.IsNullOrEmpty(AppFilePath))
                    if (!File.Exists(AppFilePath))
                        DownloadResult = "NG APP文件不存在";
                    else
                        fileList.Add(AppFilePath);

                if (!string.IsNullOrEmpty(CfgFilePath))
                    if (!File.Exists(CfgFilePath))
                        DownloadResult = "NG CFG文件不存在";
                    else
                        fileList.Add(CfgFilePath);
            }

            if (!fileList.Any())
                DownloadResult = "NG 未指定下载文件";

            var downloadAction = new Action(() =>
            {
                if (!PreProgramming(ref DownloadResult))
                    return;

                if (!DownloadDrvFlash(ref DownloadResult))
                {
                    DownloadResult = "NG DrvFlash下载失败 " + DownloadResult;
                    return;
                }

                if (
                    fileList.Select(SRecordFileHelper.GetSRecordLineData)
                        .Select(SRecordFileHelper.GetBlocks)
                        .Any(blocks => !Can.CanBusWithUds.TransferData(
                            _currentReqCanId,
                            _currentRecvCanId,
                            CanBus.CanType.Standard,
                            blocks, true, ref DownloadResult)))
                {
                    DownloadResult = "NG 下载Block失败";
                    return;
                }

                if (!Can.CanBusWithUds.TryStartRoutineControl(
                        _currentReqCanId, _currentRecvCanId,
                        CanBus.CanType.Standard, Uds14229Helper.RoutineControl.CheckProgrammingDependencies))
                {
                    DownloadResult = "NG 检测程序一致完整性3101FF01失败";
                    return;
                }

                // 写入编程日期
                var year = new[]
                {
                    Convert.ToByte(DateTime.Now.Year.ToString().Substring(0, 2), 16),
                    Convert.ToByte(DateTime.Now.Year.ToString().Substring(2, 2), 16)
                };
                var month = Convert.ToByte(DateTime.Now.Month.ToString().PadLeft(4, '0').Substring(2, 2), 16);
                var day = Convert.ToByte(DateTime.Now.Day.ToString().PadLeft(4, '0').Substring(2, 2), 16);
                if (!Can.CanBusWithUds.TryWriteData(
                    _currentReqCanId, _currentRecvCanId, CanBus.CanType.Standard,
                    CanBus.CanProtocol.Can, 0xF1, 0x99, new[] { year[0], year[1], month, day }))
                {
                    DownloadResult = "NG 写入编程日期失败";
                    return;
                }

                // 写F18C生产追溯信息
                var serialNo = string.Empty;
                var date = string.Empty;

                using (var conn = new SqlConnection(ConfigurationManager.AppSettings[PubConstant.ConnectionStringKey]))
                {
                    try
                    {
                        var cmd = new SqlCommand("GetCheckNo", conn) { CommandType = CommandType.StoredProcedure };
                        cmd.Parameters.AddWithValue("@Productno", ProductNo);  //给输入参数赋值
                        //cmd.Parameters.AddWithValue("@checkStaff", "admin");  //给输入参数赋值
                        //cmd.Parameters.AddWithValue("@creater", "admin");  //给输入参数赋值
                        //var parOutputSerialNo = cmd.Parameters.Add("@serialNumber", SqlDbType.Int);  //定义输出参数 
                        //parOutputSerialNo.Direction = ParameterDirection.Output;  //参数类型为Output  
                        var parOutputDate = cmd.Parameters.Add("@date", SqlDbType.DateTime);  //定义输出参数 
                        parOutputDate.Direction = ParameterDirection.Output;  //参数类型为Output 
                        var returnSerialNo = cmd.Parameters.Add("@return_value", SqlDbType.Int);
                        returnSerialNo.Direction = ParameterDirection.ReturnValue;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        serialNo = returnSerialNo.Value.ToString();
                        date = parOutputDate.Value.ToString();
                        //serialNp = parOutputSerialNo.Value.ToString();
                        //date = parOutputDate.Value.ToString();
                        //MessageBox.Show(parOutputSerialNo.Value.ToString());   //显示输出参数的值  
                        //MessageBox.Show(parOutputDate.Value.ToString());  //显示返回值  
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }

                if (string.IsNullOrEmpty(serialNo) || string.IsNullOrEmpty(date))
                {
                    DownloadResult = "NG 写入追溯信息失败";
                    return;
                }

                ushort productSerialNumer;
                if (!ushort.TryParse(serialNo, out productSerialNumer))
                {
                    DownloadResult = "NG 写入追溯信息失败";
                    return;
                }

                var yearStr = (DateTime.Now.Year - 2000).ToString();
                if (!Can.CanBusWithUds.TryWriteData(
                    _currentReqCanId,
                    _currentRecvCanId,
                    CanBus.CanType.Standard,
                    CanBus.CanProtocol.Can,
                    0xF1, 0x8C,
                    new[]
                    {
                        BitConverter.GetBytes(productSerialNumer)[1], BitConverter.GetBytes(productSerialNumer)[0],
                        Encoding.ASCII.GetBytes(ProductionLineNumber.PadLeft(2,'0'))[0],Encoding.ASCII.GetBytes(ProductionLineNumber.PadLeft(2,'0'))[1],
                        Convert.ToByte(yearStr),month,day,
                        (byte)0x00,(byte)0x00,(byte)0x00,(byte)0x00,(byte)0x00,(byte)0x00,(byte)0x00,(byte)0x00,(byte)0x00
                    }))
                {
                    //DownloadResult = "NG 写入追溯信息失败";
                    //return;
                }

                byte[] echo;
                Can.CanBusWithUds.TesterTryRequest(_currentReqCanId, _currentRecvCanId, new byte[] { 0x11, 0x01 }, out echo,
                    CanBus.CanType.Standard);

                DownloadResult = "OK";
            });

            var st = new Stopwatch();
            st.Start();
            downloadAction.Invoke();
            st.Stop();
            DownloadResult += " " + st.ElapsedMilliseconds + "ms";
        }

        /// <summary>
        /// 下载DrvFlash
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool DownloadDrvFlash(ref string result)
        {
            if (!string.IsNullOrEmpty(DrvFlashFilePath))
            {
                if (!File.Exists(DrvFlashFilePath))
                {
                    result = "NG DrvFlash文件不存在";
                    return false;
                }
            }
            else
            {
                result = "NG 未指定DrvFlash文件";
                return false;
            }

            var sRecord = SRecordFileHelper.GetSRecordLineData(DrvFlashFilePath);
            var drvFlashBlocks = SRecordFileHelper.GetBlocks(sRecord); // Block集合

            return Can.CanBusWithUds.TransferData(_currentReqCanId, _currentRecvCanId, CanBus.CanType.Standard,
                drvFlashBlocks, false, ref result);
        }

        /// <summary>
        /// 预编程
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool PreProgramming(ref string result)
        {
            if (!Can.CanBusWithUds.TryEnterExtendedSession(
                _currentReqCanId, _currentRecvCanId, CanBus.CanType.Standard))
            {
                result = "NG 进入拓展模式1003失败";
                return false;
            }

            if (!Can.CanBusWithUds.TryStartRoutineControl(
                _currentReqCanId,
                _currentRecvCanId,
                CanBus.CanType.Standard,
                Uds14229Helper.RoutineControl.PreProgrammingCheck))
            {
                result = "NG 检查编程预条件31010203失败";
                return false;
            }

            if (!Can.CanBusWithUds.TryControlDtcSetting(
                _currentReqCanId,
                _currentRecvCanId,
                CanBus.CanType.Standard,
                Uds14229Helper.ControlDtcSetting.Disable))
            {
                result = "NG 禁止网络中所有ECU进行DTC设置8502失败";
                return false;
            }

            if (!Can.CanBusWithUds.TryCommunicationControl(
                _currentReqCanId,
                _currentRecvCanId,
                CanBus.CanType.Standard,
                Uds14229Helper.CommunicationControl.DisableRxAndTx))
            {
                result = "NG 禁止网络中所有ECU进行常规报文通信280301失败";
                return false;
            }

            if (!Can.CanBusWithUds.TryEnterProgrammingSession(
                _currentReqCanId,
                _currentRecvCanId,
                CanBus.CanType.Standard))
            {
                result = "NG 进入编程模式1002失败";
                return false;
            }

            byte[] seedBytes;
            if (!Can.CanBusWithUds.TryRequestSeed(
                _currentReqCanId,
                _currentRecvCanId,
                CanBus.CanType.Standard, 0x11, out seedBytes))
            {
                result = "NG 请求seed2711失败";
                return false;
            }

            var keyBytes = CalcKey(seedBytes, 0x11);
            if (!Can.CanBusWithUds.TrySendKey(
                _currentReqCanId,
                _currentRecvCanId,
                CanBus.CanType.Standard, 0x12, keyBytes))
            {
                result = "NG 写入key2712失败";
                return false;
            }

            // 写入软件指纹
            var year = new[]
            {
                Convert.ToByte(DateTime.Now.Year.ToString().Substring(0, 2), 16),
                Convert.ToByte(DateTime.Now.Year.ToString().Substring(2, 2), 16)
            };
            var month = Convert.ToByte(DateTime.Now.Month.ToString().PadLeft(4, '0').Substring(2, 2), 16);
            var day = Convert.ToByte(DateTime.Now.Day.ToString().PadLeft(4, '0').Substring(2, 2), 16);
            if (Can.CanBusWithUds.TryWriteData(
                _currentReqCanId,
                _currentRecvCanId,
                CanBus.CanType.Standard,
                CanBus.CanProtocol.Can,
                0xf1, 0x84, new byte[] { 0x91, year[1], month, day, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }))
                return true;

            result = "NG 写入F184失败";
            return false;
        }

        #endregion

        private byte[] ReadDid(uint reqCanId, uint recvCanId, byte didHi, byte didLo)
        {
            if (Can == null)
                return new byte[0];

            byte[] echo;
            if (Can.CanBusWithUds.TryReadData(reqCanId, recvCanId, CanBus.CanType.Standard,
                CanBus.CanProtocol.Can, didHi, didLo, out echo))
                return echo;
            Thread.Sleep(500);
            return Can.CanBusWithUds.TryReadData(reqCanId, recvCanId, CanBus.CanType.Standard,
                CanBus.CanProtocol.Can, didHi, didLo, out echo) ? echo : new byte[0];
        }

        /// <summary>
        /// 点灯控制类型
        /// </summary>
        public enum LightOnOffType
        {
            [MatrixValDefinition(8, 2, 0)]
            [Description("0x25D")]
            PowerModeOff,

            [MatrixValDefinition(8, 2, 1)]
            [Description("0x25D")]
            PowerModeAcc,

            [MatrixValDefinition(8, 2, 2)]
            [Description("0x25D")]
            PowerModeRun,

            [MatrixValDefinition(8, 2, 3)]
            [Description("0x25D")]
            PowerModeAccCrank,

            [MatrixValDefinition(34, 1, 0)]
            [Description("0x25D")]
            LftTrnFaultSignal,

            [MatrixValDefinition(34, 1, 1)]
            [Description("0x25D")]
            LftTrnNormalSignal,

            [MatrixValDefinition(36, 1, 1)]
            [Description("0x25D")]
            RghtTrnFaultSignal,

            [MatrixValDefinition(36, 1, 0)]
            [Description("0x25D")]
            RghtTrnNormalSignal,

            [MatrixValDefinition(0, 1, 1)]
            [Description("0x306")]
            HighBmOn,

            [MatrixValDefinition(0, 1, 0)]
            [Description("0x306")]
            HighBmOff,

            [MatrixValDefinition(1, 1, 1)]
            [Description("0x306")]
            LowBmOn,

            [MatrixValDefinition(1, 1, 0)]
            [Description("0x306")]
            LowBmOff,

            [MatrixValDefinition(3, 1, 1)]
            [Description("0x306")]
            LeftTrnOn,

            [MatrixValDefinition(3, 1, 0)]
            [Description("0x306")]
            LeftTrnOff,

            [MatrixValDefinition(4, 1, 1)]
            [Description("0x306")]
            RightTrnOn,

            [MatrixValDefinition(4, 1, 0)]
            [Description("0x306")]
            RightTrnOff,

            [MatrixValDefinition(2, 1, 1)]
            [Description("0x306")]
            PlOn,

            [MatrixValDefinition(2, 1, 0)]
            [Description("0x306")]
            PlOff,

            [MatrixValDefinition(5, 1, 1)]
            [Description("0x306")]
            DrlOn,

            [MatrixValDefinition(5, 1, 0)]
            [Description("0x306")]
            DrlOff,

            [MatrixValDefinition(48, 4, 0)]
            [Description("0x306")]
            MsgConunt,

            [MatrixValDefinition(56, 8, 0)]
            [Description("0x306")]
            MsgcheckSum,

            [MatrixValDefinition(37, 1, 0)]
            [Description("0x260")]
            VehSpdVInvalid,

            [MatrixValDefinition(37, 1, 1)]
            [Description("0x260")]
            VehSpdVValid,

            [MatrixValDefinition(22, 1, 1)]
            [Description("0x350")]
            EngStRun,

            [MatrixValDefinition(22, 1, 0)]
            [Description("0x350")]
            EngStOff,

            [MatrixValDefinition(26, 1, 0)]
            [Description("0x420")]
            NoRequestWakup,

            [MatrixValDefinition(26, 1, 1)]
            [Description("0x420")]
            Wakup,

            [MatrixValDefinition(0, 1, 1)]
            [Description("0x420")]
            Led1ControlOn,

            [MatrixValDefinition(0, 1, 0)]
            [Description("0x721")]
            Led1ControlOff,

            [MatrixValDefinition(1, 1, 1)]
            [Description("0x721")]
            Led2ControlOn,

            [MatrixValDefinition(1, 1, 0)]
            [Description("0x721")]
            Led2ControlOff,

            [MatrixValDefinition(2, 1, 1)]
            [Description("0x721")]
            Led3ControlOn,

            [MatrixValDefinition(2, 1, 0)]
            [Description("0x721")]
            Led3ControlOff,

            [MatrixValDefinition(3, 1, 1)]
            [Description("0x721")]
            Led4ControlOn,

            [MatrixValDefinition(3, 1, 0)]
            [Description("0x721")]
            Led4ControlOff,

            [MatrixValDefinition(4, 1, 1)]
            [Description("0x721")]
            Led5ControlOn,

            [MatrixValDefinition(4, 1, 0)]
            [Description("0x721")]
            Led5ControlOff,

            [MatrixValDefinition(5, 1, 0)]
            [Description("0x721")]
            Led6ControlOn,

            [MatrixValDefinition(5, 1, 1)]
            [Description("0x721")]
            Led6ControlOff,

            [MatrixValDefinition(8, 1, 0)]
            [Description("0x721")]
            HsdControlOn,

            [MatrixValDefinition(8, 1, 1)]
            [Description("0x721")]
            HsdControlOff,

            [MatrixValDefinition(9, 1, 1)]
            [Description("0x721")]
            Slave1ControlOn,

            [MatrixValDefinition(9, 1, 0)]
            [Description("0x721")]
            Slave1ControlOff,

            [MatrixValDefinition(10, 1, 1)]
            [Description("0x721")]
            Slave2ControlOn,

            [MatrixValDefinition(10, 1, 0)]
            [Description("0x721")]
            Slave2ControlOff
        }
    }
}
