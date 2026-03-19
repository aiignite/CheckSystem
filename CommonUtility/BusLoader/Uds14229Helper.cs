using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CommonUtility.BusLoader
{
    public static class Uds14229Helper
    {
        public enum ServiceType : byte
        {
            [Description("诊断会话控制")]
            DiagnosticSessionControl = 0x10,

            [Description("EUC重置")]
            EcuRequest = 0x11,

            [Description("安全访问")]
            SecurityAccess = 0x27,

            [Description("控制DTC设置")]
            ControlDtcSetting = 0x85,

            [Description("按标识符读取数据")]
            ReadDataByIdentifier = 0x22,

            ReadScalingDataByIdentifier = 0x24,

            ReadDataByPeriodByIdentifier = 0x2A,

            DynamicallyDefineDataIdentifier = 0x2C,

            [Description("按标识符写数据")]
            WriteDataByIdentifier = 0x2E,

            [Description("读取DTC信息")]
            ReadDtcInfomation = 0x19,

            InputOutputControlByIdentifier = 0x2F,

            RequestDownload = 0x34,

            RequestUpload = 0x35,

            TransferData = 0x36,

            RequestTransferExit = 0x37,

            [Description("清除诊断信息")]
            ClearDiagnosticInformation = 0x14,

            ReadMemoryByAddress = 0x23,

            WriteMemoryByAddress = 0x3D,

            RoutineControl = 0x31,

            [Description("通讯控制")]
            CommunicationControl = 0x28,

            TesterPresent = 0x3E,

            ResponseOnEvent = 0x86,

            LinkControl = 0x87,
        }

        public enum RoutineControl : ushort
        {
            /// <summary>
            /// 擦除内存
            /// </summary>
            EraseMemory = 0xFF00,

            /// <summary>
            /// 检查刷新相关性
            /// </summary>
            CheckProgrammingDependencies = 0xFF01,

            /// <summary>
            /// 重编程条件检查
            /// </summary>
            CheckProgrammingPreconditions = 0xFF02,

            /// <summary>
            /// 检查编程预条件
            /// </summary>
            PreProgrammingCheck = 0x0203,

            PreCheck = 0x0202,

            /// <summary>
            /// 标定1&2电感
            /// </summary>
            Unknown1 = 0x5902,

            /// <summary>
            /// 标定3&4电感
            /// </summary>
            Unknown2 = 0x5903,

            /// <summary>
            /// 标定5&6电感
            /// </summary>
            Unknown3 = 0x5904,

            /// <summary>
            /// 触发MPU测试
            /// </summary>
            MpuMode = 0xA1A1,

            /// <summary>
            /// 检查刷新完整性 
            /// </summary>
            CheckProgrammingIntegrity = 0xDFFF,

            /// <summary>
            /// 计算软件校验码 
            /// </summary>
            CalculateSoftwareVerificationNumber = 0xDFFE,
        }

        /// <summary>
        /// 诊断模式
        /// </summary>
        public enum DiagnosticSession : byte
        {
            /// <summary>
            /// 默认模式
            /// </summary>
            Default = 0x01,

            /// <summary>
            /// 编程模式
            /// </summary>
            Programming = 0x02,

            /// <summary>
            /// 扩展模式
            /// </summary>
            Extedned = 0x03,
        }

        public enum CommunicationControl : byte
        {
            /// <summary>
            /// （激活接收和发送）
            /// </summary>
            EnableRxAndTx = 0x00,

            /// <summary>
            /// （激活接收和关闭发送）
            /// </summary>
            EnableRxAndDisableTx = 0x01,

            /// <summary>
            /// （激活发送和关闭接收）
            /// </summary>
            DisableRxAndEnableTx = 0x02,

            /// <summary>
            /// （关闭接收和发送）
            /// </summary>
            DisableRxAndTx = 0x03,

            /// <summary>
            /// （激活接收和关闭发送，针对特定的地址）
            /// </summary>
            EnableRxAndDisableTxWithEnhancedAddressInformation = 0x04,

            /// <summary>
            /// （激活接收和发送，针对特定的地址）
            /// </summary>
            EnableRxAndTxWithEnhancedAddressInformation = 0x05
        }

        public enum ControlDtcSetting : byte
        {
            /// <summary>
            /// ON
            /// </summary>
            Enablle = 0x01,

            /// <summary>
            /// OFF
            /// </summary>
            Disable = 0x02,
        }

        public enum InputOutputControlParameter : byte
        {
            /// <summary>
            /// 将控制权返还给ECU
            /// 0x00
            /// </summary>
            ReturnControlToEcu = 0x00,

            /// <summary>
            /// 重置为默认状态
            /// 0x01
            /// </summary>
            ResetToDefault = 0x01,

            /// <summary>
            /// 冻结当前状态
            /// 0x02
            /// </summary>
            FreezeCurrentState = 0x02,

            /// <summary>
            /// 短期调整
            /// 0x03
            /// </summary>
            ShortTermAdjustment = 0x03
        }

        public class DtcData
        {
            public string DtcCode;
            public string Code;
            public string FailureType;
            public string DtcStatusMask;
            public string Remark;

            public DtcData(byte dtcHigtByte, byte dtcMiddleByte, byte dtcLowByte, byte dtcStatusMask)
            {
                var errorSystem = string.Empty;
                var bitStrDtcHi = Convert.ToString(dtcHigtByte, 2).PadLeft(8, '0');

                switch (bitStrDtcHi.Substring(0, 2))
                {
                    case "00":
                        errorSystem = "P";
                        break;

                    case "01":
                        errorSystem = "C";
                        break;

                    case "10":
                        errorSystem = "B";
                        break;

                    case "11":
                        errorSystem = "U";
                        break;
                }

                var dtcCode0 = ValueHelper.GetHextStr(Convert.ToByte(bitStrDtcHi.Substring(2, 6), 2));
                var dtcCode1 = ValueHelper.GetHextStr(dtcMiddleByte);

                var dtcCode = errorSystem + (dtcCode0 + dtcCode1).PadLeft(4, '0');
                var dtcFailureType = ValueHelper.GetHextStr(dtcLowByte);
                var dtcStatus = ValueHelper.GetHextStr(dtcStatusMask);

                Code = string.Format("{0}{1}", dtcCode, dtcFailureType);
                DtcCode = dtcCode;
                FailureType = dtcFailureType;
                DtcStatusMask = dtcStatus;
                Remark = string.Format("0x{0},{1},{2},{3}", ValueHelper.GetHextStr(dtcHigtByte) + ValueHelper.GetHextStr(dtcMiddleByte), dtcCode, dtcFailureType, dtcStatus);
            }

            public DtcData(string code, string failureType)
            {
                Code = code;
                FailureType = failureType;
            }
        }

        public static IEnumerable<byte> CommonGenerateKey64Bits(byte[] seed)
        {
            Array.Reverse(seed);
            var securitySeed = BitConverter.ToUInt32(seed, 0);
            var securityKey = securitySeed >> 2;
            securityKey ^= securitySeed;
            securityKey <<= 4;
            securityKey ^= securitySeed;

            //securityKey = securityKey ^ securitySeed;
            //securityKey = securityKey << 4;
            //securityKey = securityKey ^ securitySeed;

            return BitConverter.GetBytes(securityKey).Reverse().ToArray();
        }

        public static IEnumerable<byte> CommonGenerateKey32Bits(byte[] seed)
        {
            Array.Reverse(seed);
            var securitySeed = BitConverter.ToUInt16(seed, 0);
            var securityKey = (ushort)(securitySeed >> 2);
            securityKey ^= securitySeed;
            securityKey <<= 4;
            securityKey ^= securitySeed;

            //securityKey = securityKey ^ securitySeed;
            //securityKey = securityKey << 4;
            //securityKey = securityKey ^ securitySeed;

            return BitConverter.GetBytes(securityKey).Reverse().ToArray();
        }
    }
}
