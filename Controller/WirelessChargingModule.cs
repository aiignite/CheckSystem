using CommonUtility;
using CommonUtility.BusLoader;
using CommonUtility.FileOperator;
using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Controller
{
    [Description("CAN-Product,50W无线充电模块")]
    public sealed class WirelessChargingModule : ControllerBase
    {
        public CanBus CanFD;

        public WirelessChargingModule(string name) : base(name)
        {
            //// debug
            //{
            //    var toPriintData = new List<ECUData>();

            //    toPriintData.Add(new ECUData { EcuId = "0000008D8000B6001A164105610114AC", MASTER_ECU_KEY_M4 = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF111B5031465FD7859DE75C337BE5DC5A80", MASTER_ECU_KEY_M5 = "08B4438D85CDDC9ABD2B3147F2198A6A", UNLOCK_ECU_KEY_M4 = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF447F8A1D012F264CB231837A47D9A7815D", UNLOCK_ECU_KEY_M5 = "9251D246D1FF52CB740F1CEA4AB964DA" });
            //    toPriintData.Add(new ECUData { EcuId = "0000008E8000B6001A164105610114AC", MASTER_ECU_KEY_M4 = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF11AF8D494ECA1D01529B1238E2B955F30D", MASTER_ECU_KEY_M5 = "C05FDA4A0CC063B2226993E9113094B0", UNLOCK_ECU_KEY_M4 = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF44F1421AEBB73B5B982FBD85EAC3ED375A", UNLOCK_ECU_KEY_M5 = "00946D90D63B7D2CBB9BECEC83664EE4" });
            //    toPriintData.Add(new ECUData { EcuId = "0000008F8000B6001A164105610114AC", MASTER_ECU_KEY_M4 = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF113B72993525E170067BC374AE3912C1A2", MASTER_ECU_KEY_M5 = "67EB2E1F0CBF0C28C32B76D6D075FA54", UNLOCK_ECU_KEY_M4 = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF445FBCB90971E41E67FBC541B7D5767304", UNLOCK_ECU_KEY_M5 = "88A3DBF7A14ACF9D42C57EF6942F8F20" });
            //    toPriintData.Add(new ECUData { EcuId = "000000908000B6001A164105610114AC", MASTER_ECU_KEY_M4 = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF116AE49090EBBD739E88B366854E1E3367", MASTER_ECU_KEY_M5 = "191AA98C0F459CB952F6E2F589759BCD", UNLOCK_ECU_KEY_M4 = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF44828E9B7454778421B21B30AA6422FA0F", UNLOCK_ECU_KEY_M5 = "F1B7BDF18A2CAECB31F28E88E6F7724D" });
            //    toPriintData.Add(new ECUData { EcuId = "000000918000B6001A164105610114AC", MASTER_ECU_KEY_M4 = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF114AC05C7244919F7ED8C30983504A8DBF", MASTER_ECU_KEY_M5 = "B205D8C25EDA5C199FCA0C8600F5B46B", UNLOCK_ECU_KEY_M4 = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF4409F9AA491063762AA290D5E65DF11472", UNLOCK_ECU_KEY_M5 = "655E0044C2FC08ABF0DF78789165B682" });
            //    toPriintData.Add(new ECUData { EcuId = "000000928000B6001A164105610114AC", MASTER_ECU_KEY_M4 = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF11E11AE96D7967A99247E4CCD26503AF72", MASTER_ECU_KEY_M5 = "4E19D72E177EBDC1503A2F787DEF9602", UNLOCK_ECU_KEY_M4 = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF449600D34A29E4A68912B80F0D4523C3F6", UNLOCK_ECU_KEY_M5 = "985C42AAD09CD58F68C3D992CFD07FFA" });
            //    toPriintData.Add(new ECUData { EcuId = "000000938000B6001A164105610114AC", MASTER_ECU_KEY_M4 = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF1130D432D98F4DECC52C5E9B7ADC42FA85", MASTER_ECU_KEY_M5 = "8CA0CB65C0535F89AD8D894191BE1C96", UNLOCK_ECU_KEY_M4 = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF4452AF549B86D0DCA63252236548216F0F", UNLOCK_ECU_KEY_M5 = "D3B2F152682F3C0D383F5F100E4E3971" });
            //    toPriintData.Add(new ECUData { EcuId = "000000948000B6001A164105610114AC", MASTER_ECU_KEY_M4 = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF1171D65E3BE85A31657A3C481600806146", MASTER_ECU_KEY_M5 = "E889D08B1E88544C78C5C0DB35C9AEAD", UNLOCK_ECU_KEY_M4 = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF444974C32FC48C9E334B7FF976A45F8365", UNLOCK_ECU_KEY_M5 = "B643C670690174E46B4352D0D257F4B6" });
            //    toPriintData.Add(new ECUData { EcuId = "000000958000B6001A164105610114AC", MASTER_ECU_KEY_M4 = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF112F7CF941B1DDB8C3B8E8A1E5F34F6775", MASTER_ECU_KEY_M5 = "5D48C24433D7682A860BDA7721888049", UNLOCK_ECU_KEY_M4 = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF44BE76C7B5A68E779977D2429F4875E8F0", UNLOCK_ECU_KEY_M5 = "4226EFDDF5813C63BE3E052E4F42D7A1" });

            //    foreach (var item in toPriintData)
            //    {
            //        item.TrackInfo = "SM25333A00AD0021";
            //    }

            //    PrintToXml(@"E:\Projects\50W无线充\ECUID下载回传\回传\20251117\手工9个\WCM_50W_20251117_9PCS.xml", toPriintData, out _);
            //}

            lock (_lockLocalDb)
            {
                if (!_isLocalDbInit)
                {
                    _filePath = GetSql();
                    if (!string.IsNullOrEmpty(_filePath))
                    {
                        if (CreateBase() && CreateTable())
                        {
                            _localDb.Open();
                        }
                        else
                            _localDb = null;
                    }
                    else
                        _localDb = null;

                    _isLocalDbInit = true;
                }
            }

            CanBus.PushCanMsg += CanBus_PushCanMsg;
            MainWork();
            SchedulerAsync();
        }

        private void CanBus_PushCanMsg(string name, CanBus.CanDataPackage data, CanBus.OnPushCanDataType onPushCanDataType)
        {
            if (CanFD is null)
                return;

            if (CanFD.Name != name)
                return;

            if (data is null || data.CanData.Length < 8)
                return;

            if (data.CanId == 0x2F0 && _bPreCheckExecuteQValueCalibration)
            {
                if (data.CanData.Length > 2 && data.CanData[0] == 0x36 && data.CanData[1] == 0xC0)
                {
                    _waitHandle.Set();
                }
            }

            if (data.CanId == REQUEST_CAN_ID)
            {
                //Console.WriteLine("GET REQUEST_CAN_ID 0x10DBFEF1 = " + ValueHelper.GetHextStrWithOx(data.CanData));
            }

            if (data.CanId == RESPONSE_CAN_ID)
            {
                //Console.WriteLine("GET RESPONSE_CAN_ID 0x14DAF1BE = " + ValueHelper.GetHextStrWithOx(data.CanData));

                if (_bExecuteQValueCalibration)
                {
                    var expected = "1003432099000000";
                    var recv = ValueHelper.GetHextStr(data.CanData).Replace(" ", "");
                    if (string.Equals(recv, expected, StringComparison.CurrentCultureIgnoreCase))
                        _waitHandle.Set();
                }

                if (_bReadQValueCalibrationResult)
                {
                    if (_bReadQValueCalibrationResultBuffer.Count == 0)
                    {
                        if (data.CanData[0] == 0x10 && data.CanData[1] == 0x15 && data.CanData[2] == 0x43 && data.CanData[3] == 0x30)
                        {
                            _bReadQValueCalibrationResultBuffer.AddRange(data.CanData);
                        }
                    }
                    else if (_bReadQValueCalibrationResultBuffer.Count == 8 * 1)
                    {
                        if (data.CanData[0] == 0x21)
                        {
                            _bReadQValueCalibrationResultBuffer.AddRange(data.CanData);
                        }
                    }
                    else if (_bReadQValueCalibrationResultBuffer.Count == 8 * 2)
                    {
                        if (data.CanData[0] == 0x22)
                        {
                            _bReadQValueCalibrationResultBuffer.AddRange(data.CanData);
                        }
                    }
                    else if (_bReadQValueCalibrationResultBuffer.Count == 8 * 3)
                    {
                        if (data.CanData[0] == 0x23)
                        {
                            _bReadQValueCalibrationResultBuffer.AddRange(data.CanData);
                            _waitHandle.Set();
                        }
                    }
                }
            }

            //if (data.CanId == DiagnosticReqCanId)
            //{
            //    Console.WriteLine("tx 0x14DABEF1: " + ValueHelper.GetHextStr(data.CanData));
            //}

            if (data.CanId == DiagnosticRecvCanId)
            {
                //Console.WriteLine("rx 0x14DAF1BE: " + ValueHelper.GetHextStr(data.CanData));

                if (_bReadDid)
                {
                    _uds22Buffer.AddRange(data.CanData);
                    _bReadDidOk = true;
                    //_bReadDidWaitHandle.Set();
                }

                if (_bSeedKeySubFunc)
                {
                    _uds27Buffer.AddRange(data.CanData);
                    _bSeedKeySubFuncOk = true;
                    //_bseedKeyWaitHandle.Set();
                }

                if (_bRid272SecodnFrame)
                {
                    if (data.CanData.Length >= 8 && data.CanData[0] == 0x30)
                    {
                        _bRid272SecondFrameOk = true;
                        //_bRid272WaitHandle.Set();
                    }
                }

                if (_bRid272)
                {
                    if (data.CanData.Length == 64)
                    {
                        _udsRid272Buffer.AddRange(data.CanData);
                        _bRid272Ok = true;
                        //_bRid272WaitHandle.Set();
                    }
                }
            }

            if (_isLogInjectKeyBuffer && (data.CanId == DiagnosticReqCanId || data.CanId == DiagnosticRecvCanId))
            {
                _injectKeyBuffer.Add(new KeyLogData
                {
                    MessageKey = data.MessageKey,
                    Direction = onPushCanDataType.ToString(),
                    DateTime = data.DateTime,
                    CanId = string.Format("0x:{0:X2}", data.CanId),
                    CanProtocol = data.CanProtocol,
                    CanType = data.CanType,
                    CanFormat = data.CanFormat,
                    CanDataLen = data.CanDataLen,
                    CanData = ValueHelper.GetHextStrWithOx(data.CanData),
                    Note = _logNote
                });
            }
        }

        private string SysDir = Directory.GetCurrentDirectory();
        private CsvConfiguration _logConfig;
        private string _logCsv = string.Empty;
        private string _logNote = string.Empty;

        private static bool _isLocalDbInit = false;
        private static readonly object _lockLocalDb = new object();
        private static SqlSugarClient _localDb;
        private static string _filePath = string.Empty;
        private const string _dbName = "WirelessChargingModuleLocalKeys";
        private const string _localDbFolder = "WirelessChargingModuleDbs";

        private string GetSql()
        {
            var folder = string.Format(@"{0}/LocalDB/{1}/", Path.GetPathRoot(SysDir), _localDbFolder);
            if (!Directory.Exists(folder))
            {
                try
                {
                    Directory.CreateDirectory(folder);
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }

            try
            {
                var f = string.Format(@"{0}/LocalDB/{1}/{2}.db", Path.GetPathRoot(SysDir), _localDbFolder, _dbName);
                _localDb = new SqlSugarClient(new ConnectionConfig()
                {
                    ConnectionString = $"Data Source={f}", // SQLite 连接字符串
                    DbType = DbType.Sqlite, // 指定数据库类型为 SQLite
                    IsAutoCloseConnection = true, // 自动关闭连接
                    InitKeyType = InitKeyType.Attribute // 从实体特性中读取主键自增列信息
                });

                return f;
            }
            catch (Exception)
            {
                _localDb = null;
                return string.Empty;
            }
        }

        /// <summary>
        /// 创建数据库
        /// </summary>
        private bool CreateBase()
        {
            try
            {
                return _localDb.DbMaintenance.CreateDatabase();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CreateTable()
        {
            try
            {
                _localDb.CodeFirst.InitTables<ECUData>();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #region SQL追溯相关

        [Description("R,存储数据结果")]
        public string SaveDataResult;

        [Description("R/W,服务器IP地址")]
        public string ServerIp = "192.168.0.138";

        [Description("R/W,服务器数据库名称")]
        public string ServerDataBase = "IPMS";

        [Description("R/W,服务器用户名")]
        public string ServerUid = "ipms";

        [Description("R/W,服务器用密码")]
        public string ServerPwd = "Scae2020#";

        //[Description("R/W,服务器IP地址")]
        //public string ServerIp = "192.168.1.150";

        //[Description("R/W,服务器数据库名称")]
        //public string ServerDataBase = "IPMS";

        //[Description("R/W,服务器用户名")]
        //public string ServerUid = "sa";

        //[Description("R/W,服务器用密码")]
        //public string ServerPwd = "123456";

        [Description("R/W,追溯条码")]
        public string TrackBarcode = string.Empty;

        [Description("R,追溯结果")]
        public string TrackResult = string.Empty;

        private ECUData _trackEcuData;
        private SqlSugarClient _serverDb;

        [Description("读取追溯")]
        public void LoadTrack(int mtcIndex, int mtcLen)
        {
            _trackEcuData = null;
            TrackResult = string.Empty;
            _logConfig = null;
            _logCsv = string.Empty;
            _isLogInjectKeyBuffer = false;
            _injectKeyBuffer.Clear();
            _logNote = string.Empty;

            if (_localDb is null)
            {
                TrackResult = "NG 本地数据库加载失败，数据可能会丢失，请重启程序";
                return;
            }

            if (!string.IsNullOrEmpty(TrackBarcode) && TrackBarcode.StartsWith("[)>"))
            {
                var mtc = string.Empty;
                try
                {
                    mtc = TrackBarcode.Substring(mtcIndex, mtcLen);
                }
                catch (Exception)
                {
                    TrackResult = "NG 条码格式不正确";
                    return;
                }

                lock (_lockLocalDb)
                {
                    _serverDb = new SqlSugarClient(
                        new ConnectionConfig
                        {
                            ConnectionString = string.Format(@"server={0};database={1};uid={2};pwd={3}", ServerIp, ServerDataBase, ServerUid, ServerPwd),
                            DbType = DbType.SqlServer,
                            IsAutoCloseConnection = true,
                            InitKeyType = InitKeyType.SystemTable
                        });

                    var dt = _serverDb.GetDate();

                    // 先在服务器中查询
                    // 先找IsUsage=1的数据，再找IsUsage=2的数据，再找IsUsage=0的数据
                    var isUsagedData = _serverDb.Queryable<ECUData>().Where(it => it.IsUsage == 1 && it.Barcode == TrackBarcode && it.TrackInfo == mtc).ToList();

                    if (isUsagedData.Any())
                    {
                        if (IsCheckExistOk)
                        {
                            TrackResult = "NG 重复二维码，该条码已有注入成功记录";
                            return;
                        }
                        else
                        {
                            TrackResult = "OK 该条码已有注入成功记录";

                            _trackEcuData = new ECUData
                            {
                                id = isUsagedData[0].id,
                                IsUsage = isUsagedData[0].IsUsage,
                                EcuId = isUsagedData[0].EcuId,
                                EcuIdFromFile = isUsagedData[0].EcuIdFromFile,
                                Barcode = isUsagedData[0].Barcode,
                                TrackInfo = isUsagedData[0].TrackInfo,
                                CreateTime = isUsagedData[0].CreateTime,
                                LastRecordTime = isUsagedData[0].LastRecordTime,
                                IsUploadToSgm = isUsagedData[0].IsUploadToSgm,
                                IsConstMasterKeyOk = isUsagedData[0].IsConstMasterKeyOk,
                                IsConstUnlockKeyOk = isUsagedData[0].IsConstUnlockKeyOk,
                                MASTER_ECU_KEY_KeySlot = isUsagedData[0].MASTER_ECU_KEY_KeySlot,
                                MASTER_ECU_KEY_M1 = isUsagedData[0].MASTER_ECU_KEY_M1,
                                MASTER_ECU_KEY_M2 = isUsagedData[0].MASTER_ECU_KEY_M2,
                                MASTER_ECU_KEY_M3 = isUsagedData[0].MASTER_ECU_KEY_M3,
                                MASTER_ECU_KEY_M4 = isUsagedData[0].MASTER_ECU_KEY_M4,
                                MASTER_ECU_KEY_M5 = isUsagedData[0].MASTER_ECU_KEY_M5,
                                UNLOCK_ECU_KEY_KeySlot = isUsagedData[0].UNLOCK_ECU_KEY_KeySlot,
                                UNLOCK_ECU_KEY_M1 = isUsagedData[0].UNLOCK_ECU_KEY_M1,
                                UNLOCK_ECU_KEY_M2 = isUsagedData[0].UNLOCK_ECU_KEY_M2,
                                UNLOCK_ECU_KEY_M3 = isUsagedData[0].UNLOCK_ECU_KEY_M3,
                                UNLOCK_ECU_KEY_M4 = isUsagedData[0].UNLOCK_ECU_KEY_M4,
                                UNLOCK_ECU_KEY_M5 = isUsagedData[0].UNLOCK_ECU_KEY_M5,
                            };

                            return;
                        }
                    }
                    else
                    {
                        var isUsingData = _serverDb.Queryable<ECUData>().Where(it => it.IsUsage == 2 && it.Barcode == TrackBarcode && it.TrackInfo == mtc).ToList();

                        if (isUsingData.Any())
                        {
                            // 如果服务器显示正在使用中，就先从本地加载
                            var usingData = isUsingData[0];
                            var localTempData = _localDb.Queryable<ECUData>().Where(it => it.EcuId == usingData.EcuId && it.Barcode == usingData.Barcode && it.TrackInfo == usingData.TrackInfo).ToList().First();

                            if (localTempData is null)
                            {
                                TrackResult = "NG 服务器显示正在使用中，但本地数据库没有数据";
                                return;
                            }
                            else
                            {
                                if (localTempData.IsUsage == 1 || localTempData.IsUsage == 2) // 本地显示1，之前已注入但上传失败，需重新上传; 本地显示2，正在使用中
                                {
                                    _trackEcuData = new ECUData
                                    {
                                        id = usingData.id,
                                        IsUsage = localTempData.IsUsage,
                                        EcuId = localTempData.EcuId,
                                        EcuIdFromFile = localTempData.EcuIdFromFile,
                                        Barcode = localTempData.Barcode,
                                        TrackInfo = localTempData.TrackInfo,
                                        CreateTime = localTempData.CreateTime,
                                        LastRecordTime = localTempData.LastRecordTime,
                                        IsUploadToSgm = localTempData.IsUploadToSgm,
                                        IsConstMasterKeyOk = localTempData.IsConstMasterKeyOk,
                                        IsConstUnlockKeyOk = localTempData.IsConstUnlockKeyOk,
                                        MASTER_ECU_KEY_KeySlot = localTempData.MASTER_ECU_KEY_KeySlot,
                                        MASTER_ECU_KEY_M1 = localTempData.MASTER_ECU_KEY_M1,
                                        MASTER_ECU_KEY_M2 = localTempData.MASTER_ECU_KEY_M2,
                                        MASTER_ECU_KEY_M3 = localTempData.MASTER_ECU_KEY_M3,
                                        MASTER_ECU_KEY_M4 = localTempData.MASTER_ECU_KEY_M4,
                                        MASTER_ECU_KEY_M5 = localTempData.MASTER_ECU_KEY_M5,
                                        UNLOCK_ECU_KEY_KeySlot = localTempData.UNLOCK_ECU_KEY_KeySlot,
                                        UNLOCK_ECU_KEY_M1 = localTempData.UNLOCK_ECU_KEY_M1,
                                        UNLOCK_ECU_KEY_M2 = localTempData.UNLOCK_ECU_KEY_M2,
                                        UNLOCK_ECU_KEY_M3 = localTempData.UNLOCK_ECU_KEY_M3,
                                        UNLOCK_ECU_KEY_M4 = localTempData.UNLOCK_ECU_KEY_M4,
                                        UNLOCK_ECU_KEY_M5 = localTempData.UNLOCK_ECU_KEY_M5,
                                    };

                                    TrackResult = "OK";
                                    return;
                                }
                                else
                                {
                                    TrackResult = "NG 本地数据异常，状态为：" + localTempData.IsUsage;
                                    return;
                                }
                            }
                        }
                        else
                        {
                            var freeData = _serverDb.Queryable<ECUData>().Where(it => it.IsUsage == 0).ToList().First();

                            if (freeData is null)
                            {
                                TrackResult = "NG 无法从服务器获取数据，请检查网络连接或ECU ID数量";
                                return;
                            }
                            else
                            {
                                freeData.Barcode = TrackBarcode;
                                freeData.TrackInfo = mtc;
                                freeData.IsUsage = 2;
                                freeData.LastRecordTime = dt;
                                var result = _serverDb.Updateable(freeData).ExecuteCommand();
                                if (result != 1)
                                {
                                    TrackResult = "NG 已从服务器获取数据，但无法更新服务器数据，请检查网络连接";
                                    return;
                                }

                                _trackEcuData = new ECUData
                                {
                                    id = freeData.id,
                                    EcuId = freeData.EcuId,
                                    EcuIdFromFile = freeData.EcuIdFromFile,
                                    Barcode = freeData.Barcode,
                                    TrackInfo = freeData.TrackInfo,
                                    CreateTime = freeData.CreateTime,
                                    LastRecordTime = freeData.LastRecordTime,
                                    IsUsage = freeData.IsUsage,
                                    IsUploadToSgm = freeData.IsUploadToSgm,
                                    IsConstMasterKeyOk = freeData.IsConstMasterKeyOk,
                                    IsConstUnlockKeyOk = freeData.IsConstUnlockKeyOk,
                                    MASTER_ECU_KEY_KeySlot = freeData.MASTER_ECU_KEY_KeySlot,
                                    MASTER_ECU_KEY_M1 = freeData.MASTER_ECU_KEY_M1,
                                    MASTER_ECU_KEY_M2 = freeData.MASTER_ECU_KEY_M2,
                                    MASTER_ECU_KEY_M3 = freeData.MASTER_ECU_KEY_M3,
                                    MASTER_ECU_KEY_M4 = freeData.MASTER_ECU_KEY_M4,
                                    MASTER_ECU_KEY_M5 = freeData.MASTER_ECU_KEY_M5,
                                    UNLOCK_ECU_KEY_KeySlot = freeData.UNLOCK_ECU_KEY_KeySlot,
                                    UNLOCK_ECU_KEY_M1 = freeData.UNLOCK_ECU_KEY_M1,
                                    UNLOCK_ECU_KEY_M2 = freeData.UNLOCK_ECU_KEY_M2,
                                    UNLOCK_ECU_KEY_M3 = freeData.UNLOCK_ECU_KEY_M3,
                                    UNLOCK_ECU_KEY_M4 = freeData.UNLOCK_ECU_KEY_M4,
                                    UNLOCK_ECU_KEY_M5 = freeData.UNLOCK_ECU_KEY_M5,
                                };

                                if (SaveLocalTrack())
                                {
                                    TrackResult = "OK";
                                }
                                else
                                {
                                    TrackResult = "NG 已从服务器获取数据，但无法保存到本地";
                                    return;
                                }
                            }
                        }
                    }

                    return;

                    var allServerData = _serverDb.Queryable<ECUData>().Where(it => it.Barcode == TrackBarcode && it.TrackInfo == mtc).ToList();

                    {
                        if (!allServerData.Any())
                            allServerData = _serverDb.Queryable<ECUData>().Where(it => it.IsUsage == 0 || it.IsUsage == 2).ToList();
                        else
                        {
                            if (IsCheckExistOk && allServerData.Any(it => it.IsUsage == 1))
                            {
                                TrackResult = "NG 获取不到服务器数据";
                                return;
                            }
                        }

                        if (allServerData is null || allServerData.Count == 0)
                        {
                            if (IsCheckExistOk)
                            {
                                TrackResult = "NG 获取不到服务器数据";
                                return;
                            }
                            else
                            {
                                allServerData = _serverDb.Queryable<ECUData>().Where(it => it.IsUsage == 1 && it.Barcode == TrackBarcode && it.TrackInfo == mtc).ToList();

                                if (allServerData is null || allServerData.Count == 0)
                                {
                                    TrackResult = "NG 获取不到服务器数据";
                                    return;
                                }
                            }
                        }

                        var serverData = allServerData.Find(it => it.Barcode == TrackBarcode && it.TrackInfo == mtc);
                        if (serverData is null)
                        {
                            serverData = _serverDb.Queryable<ECUData>().Where(it => it.IsUsage == 0).OrderBy(f => f.CreateTime).First();
                            if (serverData is null)
                            {
                                TrackResult = "NG 获取不到ECU ID";
                                return;
                            }
                            serverData.Barcode = TrackBarcode;
                            serverData.TrackInfo = mtc;
                        }

                        _trackEcuData = new ECUData
                        {
                            id = serverData.id,
                            EcuId = serverData.EcuId,
                            EcuIdFromFile = serverData.EcuIdFromFile,
                            Barcode = serverData.Barcode,
                            TrackInfo = serverData.TrackInfo,
                            CreateTime = serverData.CreateTime,
                            LastRecordTime = serverData.LastRecordTime,
                            IsUploadToSgm = serverData.IsUploadToSgm,
                            IsConstMasterKeyOk = serverData.IsConstMasterKeyOk,
                            IsConstUnlockKeyOk = serverData.IsConstUnlockKeyOk,
                            MASTER_ECU_KEY_KeySlot = serverData.MASTER_ECU_KEY_KeySlot,
                            MASTER_ECU_KEY_M1 = serverData.MASTER_ECU_KEY_M1,
                            MASTER_ECU_KEY_M2 = serverData.MASTER_ECU_KEY_M2,
                            MASTER_ECU_KEY_M3 = serverData.MASTER_ECU_KEY_M3,
                            MASTER_ECU_KEY_M4 = serverData.MASTER_ECU_KEY_M4,
                            MASTER_ECU_KEY_M5 = serverData.MASTER_ECU_KEY_M5,
                            UNLOCK_ECU_KEY_KeySlot = serverData.UNLOCK_ECU_KEY_KeySlot,
                            UNLOCK_ECU_KEY_M1 = serverData.UNLOCK_ECU_KEY_M1,
                            UNLOCK_ECU_KEY_M2 = serverData.UNLOCK_ECU_KEY_M2,
                            UNLOCK_ECU_KEY_M3 = serverData.UNLOCK_ECU_KEY_M3,
                            UNLOCK_ECU_KEY_M4 = serverData.UNLOCK_ECU_KEY_M4,
                            UNLOCK_ECU_KEY_M5 = serverData.UNLOCK_ECU_KEY_M5,
                        };

                        if (serverData.IsUsage == 0)
                        {
                            _trackEcuData.LastRecordTime = dt;
                            _trackEcuData.IsUsage = 2;
                        }
                        else if (serverData.IsUsage == 1)
                        {
                            if (IsCheckExistOk)
                            {
                                _trackEcuData = null;
                                TrackResult = "NG 根据当前条码获取到已经使用过的ECU ID";
                                return;
                            }

                            serverData.IsUsage = 1;
                        }
                        else if (serverData.IsUsage == 2)
                        {
                            //var getLocalFirst = _localDb.Queryable<ECUData>().Where(
                            //    it =>
                            //    it.Barcode == TrackBarcode && it.TrackInfo == mtc &&
                            //    it.EcuId == serverData.EcuId &&
                            //    it.MASTER_ECU_KEY_M1 == serverData.MASTER_ECU_KEY_M1 &&
                            //    it.MASTER_ECU_KEY_M2 == serverData.MASTER_ECU_KEY_M2 &&
                            //    it.MASTER_ECU_KEY_M3 == serverData.MASTER_ECU_KEY_M3 &&
                            //    it.UNLOCK_ECU_KEY_M1 == serverData.UNLOCK_ECU_KEY_M1 &&
                            //    it.UNLOCK_ECU_KEY_M2 == serverData.UNLOCK_ECU_KEY_M2 &&
                            //    it.UNLOCK_ECU_KEY_M3 == serverData.UNLOCK_ECU_KEY_M3 &&
                            //    it.IsUsage == 2).First();
                            //if (getLocalFirst is null)
                            //{
                            //    _trackEcuData = null;
                            //    TrackResult = "NG 根据当前条码获取到服务器显示已使用，但本地缓存未找到数据，标记异常";
                            //    return;
                            //}

                            //_trackEcuData.MASTER_ECU_KEY_M4 = getLocalFirst.MASTER_ECU_KEY_M4;
                            //_trackEcuData.MASTER_ECU_KEY_M5 = getLocalFirst.MASTER_ECU_KEY_M5;
                            //_trackEcuData.UNLOCK_ECU_KEY_M4 = getLocalFirst.UNLOCK_ECU_KEY_M4;
                            //_trackEcuData.UNLOCK_ECU_KEY_M5 = getLocalFirst.UNLOCK_ECU_KEY_M5;
                            //_trackEcuData.IsConstMasterKeyOk = getLocalFirst.IsConstMasterKeyOk;
                            //_trackEcuData.IsConstUnlockKeyOk = getLocalFirst.IsConstUnlockKeyOk;

                            _trackEcuData.LastRecordTime = dt;
                        }

                        if (serverData.IsUsage == 1)
                        {
                            TrackResult = "OK";
                        }
                        else
                        {
                            var result = _serverDb.Updateable(_trackEcuData).ExecuteCommand();

                            if (result == 1)
                            {
                                SaveLocalTrack();
                                TrackResult = "OK";

                                var folder = string.Format(@"{0}/LocalDB", Path.GetPathRoot(SysDir));
                                if (Directory.Exists(folder))
                                {
                                    _logCsv = string.Format("{0}/ECUID{1}_MTC{2}_{3}.csv", folder, _trackEcuData.EcuId, _trackEcuData.TrackInfo, DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                                    _logConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
                                    {
                                        Delimiter = ",",
                                        HasHeaderRecord = !File.Exists(_logCsv), // 仅首次写入时添加表头
                                        BufferSize = 65536,
                                        Mode = CsvMode.RFC4180 // 严格兼容RFC4180标准
                                    };
                                }
                            }
                            else
                            {
                                TrackResult = "NG 数据库更新失败";
                            }
                        }
                    }
                }
            }
            else
            {
                TrackResult = "NG 条码格式不正确";
            }
        }

        public void ExecuteWriteTrackInfo(int bootPartNo, string bootVer, string ddi, int mec)
        {
            var bIsBootPartNoSame = false;
            var bIsBootPartVerSame = false;
            var bIsDDISame = false;
            var bIsMECSame = false;

            if (CanFD is null)
                return;

            var tpIsStartCan = _isCanStart;
            var tpIsPowerModeOn = _isPowerModeOn;

            PowerModeOff();
            StartCan();
            Thread.Sleep(3000);
            StopCan();
            Thread.Sleep(500);

            // 判断boot
            {
                ReadBoot();
                if (!string.IsNullOrEmpty(BootNo) && !string.IsNullOrEmpty(BootVersion))
                {
                    var partNo = BitConverter.GetBytes(bootPartNo).Reverse().ToArray();
                    var ver = Encoding.ASCII.GetBytes(bootVer);

                    bIsBootPartNoSame = BootNo == ValueHelper.GetDecimal(new byte[] { partNo[0], partNo[1], partNo[2], partNo[3] }).ToString();
                    bIsBootPartVerSame = BootVersion == bootVer;
                }
            }

            // 判断DDI
            {
                ReadDDI();
                if (!string.IsNullOrEmpty(DDI))
                    bIsDDISame = DDI == ddi;
            }

            // 判断MEC
            {
                ReadManufacturersEnableCounter();
                if (ManufacturersEnableCounter != int.MinValue)
                    bIsMECSame = ManufacturersEnableCounter == mec;
            }

            //// 判断编程日期
            //{
            //    ReadProgrammingDate();
            //}

            EnterExtendMode();
            Thread.Sleep(15);
            SecurityAccess0102();
            if (SecurityAccess0102Result is "OK")
            {
                if (!bIsBootPartNoSame || !bIsBootPartVerSame)
                {
                    WriteBoot(bootPartNo, bootVer);
                    ReadBoot();
                }
                if (!bIsDDISame)
                    WriteAndReadDDI(ddi);
                if (!bIsMECSame)
                {
                    WriteManufacturersEnableCounter(mec);
                    ReadManufacturersEnableCounter();
                }

                WriteAndReadProgrammingDate();
                WriteAndReadMtc();
                WriteAndReadECUID();

                SecurityAccess0D0E();
                if (SecurityAccess0D0EResult is "OK")
                {
                    DisableRxAndTxCommunication();
                    Thread.Sleep(15);
                    InjectConstKey();

                    if (InjectConstKeyResult is "OK")
                    {
                        Thread.Sleep(15);
                        InjectIECSKey();

                        if (MasterKey is "OK" && UnlockKey is "OK")
                        {
                            if (_trackEcuData != null)
                            {
                                SaveLocalTrack();
                                UploadTrackToServer();
                            }
                            else
                            {
                                _trackEcuData = null;
                                SaveDataResult = string.Empty;
                            }
                        }
                        else
                        {
                            _trackEcuData = null;
                            SaveDataResult = string.Empty;
                        }
                    }
                    else
                    {
                        _trackEcuData = null;
                        SaveDataResult = string.Empty;
                        MasterKey = string.Empty;
                        UnlockKey = string.Empty;
                    }
                }
                else
                {
                    _trackEcuData = null;
                    SaveDataResult = string.Empty;
                    InjectConstKeyResult = string.Empty;
                    MasterKey = string.Empty;
                    UnlockKey = string.Empty;
                }
            }
            else
            {
                _trackEcuData = null;
                ProgrammingDate = string.Empty;
                ProgrammingDateStr = string.Empty;
                MTC = string.Empty;
                ECUID = string.Empty;

                SaveDataResult = string.Empty;
                InjectConstKeyResult = string.Empty;
                MasterKey = string.Empty;
                UnlockKey = string.Empty;
            }

            EnterNormalMode();
            ClearTrack();

            if (tpIsPowerModeOn)
                PowerModeOn();
            else
                PowerModeOff();

            if (tpIsStartCan)
                StartCan();
            else
                StopCan();
        }

        public void ClearTrack()
        {
            TrackBarcode = string.Empty;
            if (_trackEcuData != null)
                _trackEcuData = null;
        }

        public void UploadTrackToServer()
        {
            SaveDataResult = string.Empty;

            if (_trackEcuData is null)
                return;

            if (_trackEcuData.IsUsage == 1)
            {
                if (IsCheckExistOk)
                {
                    SaveDataResult = "NG 该追溯条码已被使用,不能重复保存";
                    return;
                }
                else
                {
                    SaveDataResult = "OK 该追溯条码已被记录,无需重复保存";
                    return;
                }
            }

            if (_serverDb is null)
            {
                _serverDb = new SqlSugarClient(
                   new ConnectionConfig
                   {
                       ConnectionString = string.Format(@"server={0};database={1};uid={2};pwd={3}", ServerIp, ServerDataBase, ServerUid, ServerPwd),
                       DbType = DbType.SqlServer,
                       IsAutoCloseConnection = true,
                       InitKeyType = InitKeyType.SystemTable
                   });
            }

            if (string.IsNullOrEmpty(_trackEcuData.EcuId) || _trackEcuData.EcuId.Length != 32)
            {
                SaveDataResult = "NG ECU ID 缺失";
                return;
            }

            if (string.IsNullOrEmpty(_trackEcuData.Barcode) || string.IsNullOrEmpty(_trackEcuData.TrackInfo))
            {
                SaveDataResult = "NG 追溯信息缺失";
                return;
            }

            if (_trackEcuData.IsConstUnlockKeyOk != 1 || _trackEcuData.IsConstMasterKeyOk != 1)
            {
                SaveDataResult = "NG 固定值KEY未注入成功无法保存";
                return;
            }

            if (_trackEcuData.MASTER_ECU_KEY_KeySlot.Length != 2 ||
                _trackEcuData.MASTER_ECU_KEY_M1.Length != 32 ||
                _trackEcuData.MASTER_ECU_KEY_M2.Length != 64 ||
                _trackEcuData.MASTER_ECU_KEY_M3.Length != 32)
            {
                SaveDataResult = "NG MasterKeyM1M2M3数据异常无法保存";
                return;
            }

            if (string.IsNullOrEmpty(_trackEcuData.MASTER_ECU_KEY_M4) || string.IsNullOrEmpty(_trackEcuData.MASTER_ECU_KEY_M5) || _trackEcuData.MASTER_ECU_KEY_M4.Length != 64 || _trackEcuData.MASTER_ECU_KEY_M5.Length != 32)
            {
                SaveDataResult = "NG MasterKeyM4M5数据异常无法保存";
                return;
            }

            if (_trackEcuData.UNLOCK_ECU_KEY_KeySlot.Length != 2 || _trackEcuData.UNLOCK_ECU_KEY_M1.Length != 32 || _trackEcuData.UNLOCK_ECU_KEY_M2.Length != 64 || _trackEcuData.UNLOCK_ECU_KEY_M3.Length != 32)
            {
                SaveDataResult = "NG UnlockKeyM1M2M3数据异常无法保存";
                return;
            }

            if (string.IsNullOrEmpty(_trackEcuData.UNLOCK_ECU_KEY_M4) || string.IsNullOrEmpty(_trackEcuData.UNLOCK_ECU_KEY_M5) || _trackEcuData.UNLOCK_ECU_KEY_M4.Length != 64 || _trackEcuData.UNLOCK_ECU_KEY_M5.Length != 32)
            {
                SaveDataResult = "NG UnlockKeyM4M5数据异常无法保存";
                return;
            }

            _trackEcuData.IsUsage = 1;
            SaveLocalTrack(true);
            var result = _serverDb.Updateable(_trackEcuData).ExecuteCommand();
            SaveDataResult = result == 1 ? "OK" : "NG";

            if (result == 1 && _localDb != null)
            {
                lock (_lockLocalDb)
                {
                    var getData = _localDb.Queryable<ECUData>().Where(it => it.EcuId == _trackEcuData.EcuId && it.Barcode == _trackEcuData.Barcode && it.TrackInfo == _trackEcuData.TrackInfo).First();
                    if (getData != null)
                    {
                        var dr = _localDb.Deleteable(getData).Where(it => it.EcuId == _trackEcuData.EcuId && it.Barcode == _trackEcuData.Barcode && it.TrackInfo == _trackEcuData.TrackInfo).ExecuteCommand();
                    }
                }
            }
        }

        private bool SaveLocalTrack(bool bFLag = false)
        {
            if (_trackEcuData is null)
                return false;

            if (_localDb is null)
                return false;

            if (_trackEcuData.IsUsage == 1 && !bFLag)
            {
                return true;
            }

            var tempData = new ECUData
            {
                IsConstMasterKeyOk = _trackEcuData.IsConstMasterKeyOk,
                IsConstUnlockKeyOk = _trackEcuData.IsConstUnlockKeyOk,
                IsUsage = _trackEcuData.IsUsage,
                EcuId = _trackEcuData.EcuId,
                Barcode = _trackEcuData.Barcode,
                TrackInfo = _trackEcuData.TrackInfo,
                MASTER_ECU_KEY_KeySlot = _trackEcuData.MASTER_ECU_KEY_KeySlot,
                MASTER_ECU_KEY_M1 = _trackEcuData.MASTER_ECU_KEY_M1,
                MASTER_ECU_KEY_M2 = _trackEcuData.MASTER_ECU_KEY_M2,
                MASTER_ECU_KEY_M3 = _trackEcuData.MASTER_ECU_KEY_M3,
                MASTER_ECU_KEY_M4 = _trackEcuData.MASTER_ECU_KEY_M4,
                MASTER_ECU_KEY_M5 = _trackEcuData.MASTER_ECU_KEY_M5,
                UNLOCK_ECU_KEY_KeySlot = _trackEcuData.UNLOCK_ECU_KEY_KeySlot,
                UNLOCK_ECU_KEY_M1 = _trackEcuData.UNLOCK_ECU_KEY_M1,
                UNLOCK_ECU_KEY_M2 = _trackEcuData.UNLOCK_ECU_KEY_M2,
                UNLOCK_ECU_KEY_M3 = _trackEcuData.UNLOCK_ECU_KEY_M3,
                UNLOCK_ECU_KEY_M4 = _trackEcuData.UNLOCK_ECU_KEY_M4,
                UNLOCK_ECU_KEY_M5 = _trackEcuData.UNLOCK_ECU_KEY_M5,
                LastRecordTime = _trackEcuData.LastRecordTime,
                CreateTime = _trackEcuData.CreateTime,
                EcuIdFromFile = _trackEcuData.EcuIdFromFile,
                IsUploadToSgm = _trackEcuData.IsUploadToSgm,
            };

            lock (_lockLocalDb)
            {
                var getData = _localDb.Queryable<ECUData>().Where(it => it.EcuId == tempData.EcuId).First();
                if (getData is null)
                {
                    var insertable = _localDb.Insertable(tempData);
                    insertable.IgnoreColumns("id"); // 忽略自增长列
                    var r = insertable.ExecuteReturnIdentity();
                    return r > 0;
                }
                else
                {
                    var r = _localDb.Updateable(tempData).Where(f => f.EcuId == tempData.EcuId).ExecuteCommand();
                    return r > 0;
                }
            }

            //if (_db is null)
            //{
            //    _db = new SqlSugarClient(
            //       new ConnectionConfig
            //       {
            //           ConnectionString = string.Format(@"server={0};database={1};uid={2};pwd={3}", ServerIp, ServerDataBase, ServerUid, ServerPwd),
            //           DbType = DbType.SqlServer,
            //           IsAutoCloseConnection = true,
            //           InitKeyType = InitKeyType.SystemTable
            //       });
            //}

            //_trackEcuData.LastRecordTime = DateTime.Now;
            //_db?.Updateable(_trackEcuData).ExecuteCommand();
        }

        #endregion

        #region 周期报文

        private readonly object _lockCanSend = new object();
        private bool _isCanStart;
        private bool _isPowerModeOn = true;
        private bool _isPEPSOn = true;

        [Description("打开CAN")]
        public void StartCan() => _isCanStart = true;

        [Description("关闭CAN")]
        public void StopCan() => _isCanStart = false;

        [Description("打开PowerMode")]
        public void PowerModeOn() => _isPowerModeOn = true;

        [Description("关闭PowerMode")]
        public void PowerModeOff() => _isPowerModeOn = false;

        public void PEPSOn() => _isPEPSOn = true;

        public void PEPSOff() => _isPEPSOn = false;

        private void MainWork()
        {
            //SetTimer100Ms();
            SetTimer50Ms();
            KeepExtendedSessionTimer();
        }

        public void SetTimer100Ms()
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = SendMsg(() =>
                {
                    return new CanBus.CanDataPackage[]
                    {
                         new CanBus.CanDataPackage(0x600, CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8])
                    };
                }),
                Interval = 100
            });
        }

        public void SetTimer50Ms()
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                //Action = SendMsg(new CanBus.CanDataPackage[]d
                //{
                //    new CanBus.CanDataPackage(0x638, CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8]),
                //    new CanBus.CanDataPackage(0x34A, CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8]{ 0x00,0x00,0x00,0x00,0x00,0x00,0x02,0x00}),
                //    new CanBus.CanDataPackage(0x111, CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8]{ 0x00,0x00,0x00,0x00,0x00,0x00,0x02,0x00})
                //}),
                //Interval = 50

                Action = SendMsg(() =>
                {
                    return new CanBus.CanDataPackage[]
                    {
                        new CanBus.CanDataPackage(0x638, CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8]),
                        new CanBus.CanDataPackage(0x34A, CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, (byte)(_isPEPSOn ?0x02:0x00), 0x00 }),
                        new CanBus.CanDataPackage(0x111, CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data, new byte[8] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, _isPowerModeOn ? (byte)0x02 : (byte)0x00, 0x00 })
                    };
                }),

                Interval = 50
            });
        }

        public void KeepExtendedSessionTimer()
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = new Action(() =>
                {
                    if (_bInExtendedSession && CanFD != null)
                    {
                        //if (!_bInSeedKey && !_bInWriteDID && !_bInJnjectKey && !_bInReadDID)
                        //    CanFD.SendExtendedCanFdData(DiagnosticReqCanId, CanBus.MyUds.KeepExtendedSessionBytes());

                        // 关于NDLB无线充 功能寻址会话维持的ID及报文：
                        // ID：0x10DBFEF1    02 3E 80 00 00 00 00 00    该报文不需要回复
                        // ID：0x10DBFEF1    02 3E 00 00 00 00 00 00    该报文WCM回复 ID：14DAF1BE    02 7E 00 00 00 00 00 00
                        if (!_bInSeedKey && !_bInWriteDID && !_bInJnjectKey && !_bReadDid)
                        {
                            lock (_lockCanSend)
                                CanFD.SendExtendedCanFdData(0x10DBFEF1, CanBus.MyUds.KeepExtendedSessionBytes());
                        }
                    }
                }),
                Interval = 1000
            });
        }

        private Action SendMsg(Func<CanBus.CanDataPackage[]> msgFunc)
        {
            return () =>
            {
                if (CanFD is null)
                    return;

                if (_isCanStart && msgFunc != null)
                {
                    var data = msgFunc.Invoke();
                    if (data is null)
                        return;

                    lock (_lockCanSend)
                        CanFD.SendCanDatas(data);
                }
            };
        }

        #endregion

        #region Q值校准功能

        /// <summary>
        /// 线圈校准参数
        /// </summary>
        public class CoilCalibrationData
        {
            /// <summary>
            /// 线圈索引
            /// </summary>
            public int QIndex { get; set; }

            /// <summary>
            /// Q值
            /// </summary>
            public ushort QValue { get; set; }

            /// <summary>
            /// 频率
            /// </summary>
            public ushort Frequency { get; set; }

            /// <summary>
            /// 阻抗
            /// </summary>
            public ushort Impedance { get; set; }
        }

        // Q值校准 CAN ID定义
        private const uint REQUEST_CAN_ID = 0x10DBFEF1;
        private const uint RESPONSE_CAN_ID = 0x14DAF1BE;

        // Q值校准工厂模式算法常量
        private const ushort TMP_CODE = 0x7755;
        private const ushort INITIAL_REMAINDER = 0x4351;

        [Description("R/W,校准等待时间MS")]
        public int CALIBRATION_WAIT_TIME = 7000;

        [Description("R,线圈1-Q值")]
        public float Q1Value = float.MinValue;
        [Description("R,线圈1-频率")]
        public float Q1Frequency = float.MinValue;
        [Description("R,线圈1-阻抗")]
        public float Q1Impedance = float.MinValue;

        [Description("R,线圈2-Q值")]
        public float Q2Value = float.MinValue;
        [Description("R,线圈2-频率")]
        public float Q2Frequency = float.MinValue;
        [Description("R,线圈2-阻抗")]
        public float Q2Impedance = float.MinValue;

        [Description("R,线圈3-Q值")]
        public float Q3Value = float.MinValue;
        [Description("R,线圈3-频率")]
        public float Q3Frequency = float.MinValue;
        [Description("R,线圈3-阻抗")]
        public float Q3Impedance = float.MinValue;

        private readonly ManualResetEventSlim _waitHandle = new ManualResetEventSlim(false);
        private bool _bPreCheckExecuteQValueCalibration;
        private bool _bEnterFactoryMode;
        private bool _bExecuteQValueCalibration;
        private bool _bReadQValueCalibrationResult;
        private readonly List<byte> _bReadQValueCalibrationResultBuffer = new List<byte>();

        /// <summary>
        /// 工厂模式算法函数
        /// </summary>
        /// <param name="seed">工厂模式种子</param>
        /// <returns>工厂模式密钥</returns>
        private ushort CalcKey(ushort seed)
        {
            byte[] bSeed = new byte[2];
            ushort remainder;
            byte n;
            byte i;

            bSeed[0] = (byte)(seed >> 8); /* MSB */
            bSeed[1] = (byte)seed; /* LSB */
            remainder = INITIAL_REMAINDER;

            for (n = 0; n < 2; n++)
            {
                remainder ^= (ushort)(bSeed[n] << 8);
                for (i = 0; i < 8; i++)
                {
                    remainder += TMP_CODE;
                    remainder ^= TMP_CODE;
                    remainder -= TMP_CODE;
                }
            }
            return remainder;
        }

        /// <summary>
        /// 计算校验和
        /// </summary>
        /// <param name="seedBytes"></param>
        /// <param name="keyBytes"></param>
        /// <returns>校验和</returns>
        private byte CalculateChecksum(byte[] seedBytes, byte[] keyBytes)
        {
            var sum = 0x0B + 0x45 + 0xAA + 0x59 + 0x47 + 0x4B + 0x4A + seedBytes[0] + seedBytes[1] + keyBytes[0] + keyBytes[1];
            return (byte)~sum;
        }

        /// <summary>
        /// 进入工厂模式
        /// </summary>
        /// <param name="seed">工厂模式种子</param>
        /// <returns>是否成功进入工厂模式</returns>
        public bool EnterFactoryMode(byte[] seedBytes)
        {
            try
            {
                if (seedBytes is null || seedBytes.Length != 2)
                    return false;

                // 计算工厂模式密钥
                var key = CalcKey((ushort)(seedBytes[0] * 256 + seedBytes[1]));
                var keyBytes = BitConverter.GetBytes(key).Reverse().ToArray();
                //var keyBytes = BitConverter.GetBytes(key).ToArray();
                var crcByte = CalculateChecksum(seedBytes, keyBytes);

                _bEnterFactoryMode = true;

                var taskSend = Task.Factory.StartNew(() =>
                {
                    lock (_lockCanSend)
                    {
                        // 构建进入工厂模式命令
                        byte[] requestData = new byte[8];
                        requestData[0] = 0x10;
                        requestData[1] = 0x0B;
                        requestData[2] = 0x45;
                        requestData[3] = 0xAA;
                        requestData[4] = 0x59;
                        requestData[5] = 0x47;
                        requestData[6] = 0x4B;
                        requestData[7] = 0x4A;
                        CanFD.SendExtendedCanFdData(REQUEST_CAN_ID, requestData);
                        Thread.Sleep(5);

                        requestData[0] = 0x21;
                        requestData[1] = seedBytes[0];
                        requestData[2] = seedBytes[1];
                        requestData[3] = keyBytes[0];
                        requestData[4] = keyBytes[1];
                        requestData[5] = crcByte;
                        requestData[6] = 0x00;
                        requestData[7] = 0x00;
                        CanFD.SendExtendedCanFdData(REQUEST_CAN_ID, requestData);
                    }
                });

                var taskWait = Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(50);
                    //return _waitHandle.WaitOne(250);
                    return true;
                });

                Task.WaitAll(taskSend, taskWait);
                _bEnterFactoryMode = false;

                if (taskWait.Result)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                // 记录错误日志
                Console.WriteLine($"进入工厂模式失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 启动Q值校准
        /// </summary>
        /// <returns>是否成功启动校准</returns>
        private bool StartQValueCalibration()
        {
            try
            {
                _waitHandle.Reset();
                _bExecuteQValueCalibration = true;

                var taskSend = Task.Factory.StartNew(() =>
                {
                    lock (_lockCanSend)
                    {
                        // 构建启动Q值校准命令
                        byte[] requestData = new byte[8];
                        requestData[0] = 0x10;
                        requestData[1] = 0x03;
                        requestData[2] = 0x43;
                        requestData[3] = 0x20;
                        requestData[4] = 0x99;
                        requestData[5] = 0x00;
                        requestData[6] = 0x00;
                        requestData[7] = 0x00;
                        CanFD.SendExtendedCanFdData(REQUEST_CAN_ID, requestData);
                    }
                });

                var taskWait = Task.Factory.StartNew(() =>
                {
                    return _waitHandle.Wait(500);
                });

                Task.WaitAll(taskSend, taskWait);
                _bExecuteQValueCalibration = false;
                _waitHandle.Reset();

                if (taskWait.Result)
                {
                    Console.WriteLine($"Q值校准启动成功");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Q值校准启动失败，响应异常");
                    return false;
                }
            }
            catch (Exception ex)
            {
                // 记录错误日志
                Console.WriteLine($"启动Q值校准失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 获取Q值校准参数
        /// </summary>
        /// <returns></returns>
        private CoilCalibrationData[] GetQValueCalibrationData()
        {
            try
            {
                _waitHandle.Reset();
                _bReadQValueCalibrationResultBuffer.Clear();
                _bReadQValueCalibrationResult = true;

                var taskSend = Task.Factory.StartNew(() =>
                {
                    lock (_lockCanSend)
                    {
                        // 构建启动Q值校准命令
                        byte[] requestData = new byte[8];
                        requestData[0] = 0x10;
                        requestData[1] = 0x03;
                        requestData[2] = 0x43;
                        requestData[3] = 0x30;
                        requestData[4] = 0x89;
                        requestData[5] = 0x00;
                        requestData[6] = 0x00;
                        requestData[7] = 0x00;
                        CanFD.SendExtendedCanFdData(REQUEST_CAN_ID, requestData);
                    }
                });

                var taskWait = Task.Factory.StartNew(() =>
                {
                    return _waitHandle.Wait(1500);
                });

                Task.WaitAll(taskSend, taskWait);
                _bReadQValueCalibrationResult = false;
                _waitHandle.Reset();

                if (taskWait.Result)
                {
                    Console.WriteLine($"获取Q值校准参数成功");
                    // 解析响应数据
                    var calibrationData = new CoilCalibrationData[3];
                    var bIndex = 0;

                    var tpDt = new List<byte>();
                    for (var i = 4; i < 8; i++)
                        tpDt.Add(_bReadQValueCalibrationResultBuffer[i]);
                    for (var i = 9; i < 8 + 8; i++)
                        tpDt.Add(_bReadQValueCalibrationResultBuffer[i]);
                    for (var i = 17; i < 16 + 8; i++)
                        tpDt.Add(_bReadQValueCalibrationResultBuffer[i]);

                    for (var i = 0; i < tpDt.Count; i = i + 6)
                    {
                        var thisQValueBytes = tpDt.Skip(i).Take(6).ToArray();
                        calibrationData[bIndex] = new CoilCalibrationData();
                        calibrationData[bIndex].QIndex = bIndex;
                        calibrationData[bIndex].QValue = (ushort)(thisQValueBytes[0] * 256 + thisQValueBytes[0 + 1]);
                        calibrationData[bIndex].Frequency = (ushort)(thisQValueBytes[0 + 2] * 256 + thisQValueBytes[0 + 3]);
                        calibrationData[bIndex].Impedance = (ushort)(thisQValueBytes[0 + 4] * 256 + thisQValueBytes[0 + 5]);
                        bIndex++;
                        if (bIndex > 2)
                            break;
                    }

                    return calibrationData;
                }
                else
                {
                    Console.WriteLine($"获取Q值校准参数失败，响应异常");
                    return null;
                }
            }
            catch (Exception ex)
            {
                // 记录错误日志
                Console.WriteLine($"获取Q值校准参数失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 执行完整的Q值校准流程
        /// </summary>
        /// <param name="seed">工厂模式种子</param>
        /// <param name="coilIndex">线圈索引（0-3）</param>
        /// <returns>校准结果</returns>
        [Description("执行完整的Q值校准流程")]
        public void PerformQValueCalibration()
        {
            Q1Value = float.MinValue;
            Q1Frequency = float.MinValue;
            Q1Impedance = float.MinValue;

            Q2Value = float.MinValue;
            Q2Frequency = float.MinValue;
            Q2Impedance = float.MinValue;

            Q3Value = float.MinValue;
            Q3Frequency = float.MinValue;
            Q3Impedance = float.MinValue;

            if (CanFD is null)
                return;

            _bPreCheckExecuteQValueCalibration = true;
            var isPreCheckOk = _waitHandle.Wait(5000);
            _bPreCheckExecuteQValueCalibration = false;
            if (!isPreCheckOk)
                return;

            var rd = new Random();
            var seedBytes = new byte[2];
            rd.NextBytes(seedBytes);

            // 步骤1: 进入工厂模式
            if (!EnterFactoryMode(seedBytes))
                return;

            // 步骤2: 启动Q值校准
            if (!StartQValueCalibration())
                return;

            // 步骤3: 等待校准完成
            Thread.Sleep(CALIBRATION_WAIT_TIME); // 需要等待7秒钟的自校准过程

            // 步骤4: 获取校准数据
            var calibrationData = GetQValueCalibrationData();
            if (calibrationData == null)
                return;

            Q1Value = calibrationData[0].QValue;
            Q1Frequency = calibrationData[0].Frequency;
            Q1Impedance = calibrationData[0].Impedance;

            Q2Value = calibrationData[1].QValue;
            Q2Frequency = calibrationData[1].Frequency;
            Q2Impedance = calibrationData[1].Impedance;

            Q3Value = calibrationData[2].QValue;
            Q3Frequency = calibrationData[2].Frequency;
            Q3Impedance = calibrationData[2].Impedance;
        }

        #endregion

        #region 诊断

        private uint DiagnosticReqCanId = 0x14DABEF1;
        private uint DiagnosticRecvCanId = 0x14DAF1BE;
        private bool _bInExtendedSession;

        [Description("一键清空读取结果")]
        public void ClearResult()
        {
            SecurityAccess0102Result = string.Empty;
            SecurityAccess0D0EResult = string.Empty;
            BootNo = string.Empty;
            BootVersion = string.Empty;
            AppNo = string.Empty;
            AppVersion = string.Empty;
            CalNo = string.Empty;
            CalVersion = string.Empty;
            BaseModulePartNumber = string.Empty;
            BaseModuleAlphaCode = string.Empty;
            EndModulePartNumber = string.Empty;
            EndModuleAlphaCode = string.Empty;
            VPPS = string.Empty;
            DUNS = string.Empty;
            DDI = string.Empty;
            MTC = string.Empty;
            ECUID = string.Empty;
            ProgrammingDate = string.Empty;
            ManufacturersEnableCounter = int.MinValue;
            ClearDtcResult = string.Empty;
            ReadDtcResult = string.Empty;
        }

        [Description("进入拓展模式")]
        public void EnterExtendMode()
        {
            if (CanFD is null)
                return;

            lock (_lockCanSend)
                CanFD.CanBusWithUds.TryEnterExtendedSession(DiagnosticReqCanId, DiagnosticRecvCanId, CanBus.CanType.Extended, CanBus.CanProtocol.CanFd, pendingByte: 0x00);
            _bInExtendedSession = true;
        }

        [Description("进入正常模式")]
        public void EnterNormalMode()
        {
            if (CanFD is null)
                return;

            _bInExtendedSession = false;
            lock (_lockCanSend)
                CanFD.CanBusWithUds.TryEnterDefaultSession(DiagnosticReqCanId, DiagnosticRecvCanId, CanBus.CanType.Extended, CanBus.CanProtocol.CanFd, pendingByte: 0x00);
        }

        [Description("R,安全访问解锁0102结果")]
        public string SecurityAccess0102Result = string.Empty;
        [Description("安全访问解锁0102")]
        public void SecurityAccess0102() => SecurityAccess0102Result = SecurityAccess(0x01, 0x02);

        [Description("R,安全访问解锁0D0E结果")]
        public string SecurityAccess0D0EResult = string.Empty;
        [Description("安全访问解锁0D0E")]
        public void SecurityAccess0D0E() => SecurityAccess0D0EResult = SecurityAccess(0x0D, 0x0E);

        private List<byte> _uds27Buffer = new List<byte>();
        private bool _bSeedKeySubFunc;
        private bool _bSeedKeySubFuncOk;
        //private ManualResetEventSlim _bseedKeyWaitHandle = new ManualResetEventSlim(false);
        private bool _bInSeedKey;

        private string SecurityAccess(byte requesetSeedSubFunc, byte sendKeySubunc)
        {
            if (CanFD is null)
                return string.Empty;

            lock (_lockCanSend)
            {
                var keyBytes = new byte[12];
                for (var i = 0; i < keyBytes.Length; i++)
                    keyBytes[i] = (byte)(i + 1);

                //_bseedKeyWaitHandle.Reset();
                _bSeedKeySubFuncOk = false;
                _uds27Buffer.Clear();
                _bSeedKeySubFunc = true;
                _bInSeedKey = true;

                var firstSend = new byte[] { 0x02, 0x27, requesetSeedSubFunc, 0x00, 0x00, 0x00, 0x00, 0x00 };
                var t1 = HighPrecisionTimer.GetTimestamp();

                //使用 ThreadPool 异步发送，减少 Task 创建开销
                ThreadPool.QueueUserWorkItem(_ => CanFD.SendExtendedCanFdData(DiagnosticReqCanId, firstSend));

                var taskFirstWaitResult = false;
                while (true)
                {
                    Thread.Sleep(1);

                    if (_bSeedKeySubFuncOk)
                    {
                        taskFirstWaitResult = true;
                        break;
                    }

                    var ns = HighPrecisionTimer.GetTimestamp();
                    var tts = HighPrecisionTimer.GetTimestampIntervalMs(t1, ns);
                    if (tts > 500)
                        break;
                }

                //var taskFirstWait = Task.Factory.StartNew(() =>
                //{
                //    return _bseedKeyWaitHandle.Wait(1500);
                //});
                //var taskFirstSend = Task.Factory.StartNew(() =>
                //{
                //    CanFD.SendExtendedCanFdData(DiagnosticReqCanId, firstSend);
                //});
                //Task.WaitAll(taskFirstSend, taskFirstWait);


                _bSeedKeySubFunc = false;
                _bSeedKeySubFuncOk = false;
                //_bseedKeyWaitHandle.Reset();
                _bInSeedKey = false;

                var isFirstRecvOk = taskFirstWaitResult;
                if (isFirstRecvOk)
                {
                    if (_uds27Buffer.Count >= 4)
                    {
                        var b1 = _uds27Buffer[0];
                        var b2 = _uds27Buffer[1];
                        var b3 = _uds27Buffer[2];
                        var b4 = _uds27Buffer[3];

                        if (b1.GetByteHighOrder() == 0x00 && b3 == 0x67 && b4 == requesetSeedSubFunc)
                        {
                            _bSeedKeySubFuncOk = false;
                            //_bseedKeyWaitHandle.Reset();
                            _uds27Buffer.Clear();
                            _bSeedKeySubFunc = true;
                            firstSend = new byte[16] { 0x00, 0x0E, 0x27, sendKeySubunc, keyBytes[0], keyBytes[1], keyBytes[2], keyBytes[3], keyBytes[4], keyBytes[5], keyBytes[6], keyBytes[7], keyBytes[8], keyBytes[9], keyBytes[10], keyBytes[11] };

                            //使用 ThreadPool 异步发送，减少 Task 创建开销
                            ThreadPool.QueueUserWorkItem(_ => CanFD.SendExtendedCanFdData(DiagnosticReqCanId, firstSend));

                            var t2 = HighPrecisionTimer.GetTimestamp();
                            taskFirstWaitResult = false;
                            while (true)
                            {
                                Thread.Sleep(1);

                                if (_bSeedKeySubFuncOk)
                                {
                                    taskFirstWaitResult = true;
                                    break;
                                }

                                var ns = HighPrecisionTimer.GetTimestamp();
                                var tts = HighPrecisionTimer.GetTimestampIntervalMs(t1, ns);
                                if (tts > 500)
                                    break;
                            }

                            //taskFirstWait = Task.Factory.StartNew(() =>
                            //{
                            //    return _bseedKeyWaitHandle.Wait(1500);
                            //});
                            //taskFirstSend = Task.Factory.StartNew(() =>
                            //{
                            //    CanFD.SendExtendedCanFdData(DiagnosticReqCanId, firstSend);
                            //});
                            //Task.WaitAll(taskFirstSend, taskFirstWait);
                            _bSeedKeySubFunc = false;

                            var isSecondRecvOk = taskFirstWaitResult;
                            if (_uds27Buffer.Count >= 4)
                            {
                                b1 = _uds27Buffer[0];
                                b2 = _uds27Buffer[1];
                                b3 = _uds27Buffer[2];
                                b4 = _uds27Buffer[3];

                                if (b1.GetByteHighOrder() == 0x00 && b1.GetByteLowOrder() == 0x02 && b2 == 0x67 && b3 == sendKeySubunc)
                                {
                                    return "OK";
                                }
                                else
                                {
                                    return "NG 解锁KEY失败";
                                }
                            }
                            else
                            {
                                return "NG 解锁KEY失败";
                            }
                        }
                        else
                        {
                            return "NG 获取SEED失败";
                        }
                    }
                    else
                    {
                        return "NG 获取SEED失败";
                    }
                }
                else
                {
                    return "NG 获取SEED失败";
                }
            }

            //return CanFD.CanBusWithUds.TryRequestSeed(DiagnosticReqCanId, DiagnosticRecvCanId, CanBus.CanType.Extended, requesetSeedSubFunc, out _, pendingByte: 0x00, canProtocol: CanProtocol.CanFd) ?
            //    (CanFD.CanBusWithUds.TrySendKey(DiagnosticReqCanId, DiagnosticRecvCanId, CanBus.CanType.Extended, sendKeySubunc, keyBytes, pendingByte: 0x00, canProtocol: CanProtocol.CanFd) ? "OK" : "NG 解锁KEY失败") :
            //    "NG 获取SEED失败";
        }

        [Description("R,手机密钥读取")]
        public string PhoneKey = string.Empty;

        [Description("手机密钥读取")]
        public void ReadPhoneKey()
        {
            PhoneKey = string.Empty;
            var result = ReadDid(0xFD, 0x08);
            PhoneKey = ValueHelper.GetHextStr(result);
        }

        [Description("R,引导程序零件号")]
        public string BootNo = string.Empty;
        [Description("R,引导程序版本号")]
        public string BootVersion = string.Empty;
        [Description("读引导程序")]
        public void ReadBoot()
        {
            BootNo = string.Empty;
            BootVersion = string.Empty;

            var result = ReadDid(0xF1, 0x80, true);
            if (result.Length == 8)
            {
                BootNo = ValueHelper.GetDecimal(new byte[] { result[2], result[3], result[4], result[5] }).ToString();
                BootVersion = Encoding.ASCII.GetString(new byte[] { result[6], result[7] });
            }

            //BootNo = ValueHelper.GetDecimal(ReadDid(0xF1, 0x80)).ToString();
            //BootVersion = Encoding.ASCII.GetString(ReadDid(0xF1, 0x80));
        }

        [Description("写引导程序号")]
        public void WriteBoot(int bootPartNo, string bootVer)
        {
            var partNo = BitConverter.GetBytes(bootPartNo).Reverse().ToArray();
            var ver = Encoding.ASCII.GetBytes(bootVer);

            var writeBs = new List<byte> { 0x00, 0x00 };
            writeBs.AddRange(partNo);
            writeBs.AddRange(ver);

            WriteDid(0xF1, 0x80, writeBs.ToArray());
        }

        [Description("R,应用程序零件号")]
        public string AppNo = string.Empty;
        [Description("R,应用程序版本号")]
        public string AppVersion = string.Empty;
        [Description("读应用程序")]
        public void ReadApp()
        {
            AppNo = ValueHelper.GetDecimal(ReadDid(0xF1, 0xC1, true)).ToString();
            AppVersion = Encoding.ASCII.GetString(ReadDid(0xF1, 0xD1, true));
        }

        [Description("R,配置文件1零件号")]
        public string CalNo = string.Empty;
        [Description("R,配置文件1版本号")]
        public string CalVersion = string.Empty;
        [Description("读配置文件1")]
        public void ReadCal()
        {
            CalNo = ValueHelper.GetDecimal(ReadDid(0xF1, 0xC2, true)).ToString();
            CalVersion = Encoding.ASCII.GetString(ReadDid(0xF1, 0xD2, true));
        }

        [Description("R,初始零件号-BaseModulePartNumber")]
        public string BaseModulePartNumber = string.Empty;
        [Description("R,初始版本号-BaseModuleAlphaCode")]
        public string BaseModuleAlphaCode = string.Empty;
        [Description("读初始信息")]
        public void ReadBaseModule()
        {
            BaseModulePartNumber = ValueHelper.GetDecimal(ReadDid(0xF1, 0xCC, true)).ToString();
            BaseModuleAlphaCode = Encoding.ASCII.GetString(ReadDid(0xF1, 0xDC, true));
        }

        [Description("R,当前零件号-EndModulePartNumber")]
        public string EndModulePartNumber = string.Empty;
        [Description("R,当前版本号-EndModuleAlphaCode")]
        public string EndModuleAlphaCode = string.Empty;
        [Description("读当前信息")]
        public void ReadEndModule()
        {
            EndModulePartNumber = ValueHelper.GetDecimal(ReadDid(0xF1, 0xCB, true)).ToString();
            EndModuleAlphaCode = Encoding.ASCII.GetString(ReadDid(0xF1, 0xDB, true));
        }

        [Description("R,VPPS")]
        public string VPPS = string.Empty;
        [Description("读VPPS")]
        public void ReadVPPS() => VPPS = Encoding.ASCII.GetString(ReadDid(0xF0, 0xAB, true));

        [Description("R,DUNS")]
        public string DUNS = string.Empty;
        [Description("读DUNS")]
        public void ReadDUNS() => DUNS = Encoding.ASCII.GetString(ReadDid(0xF0, 0xB3, true));

        [Description("R,DDI")]
        public string DDI = string.Empty;

        [Description("读取DDI")]
        public void ReadDDI()
        {
            var didHi = (byte)0xF0;
            var didLo = (byte)0x9A;
            DDI = string.Empty;
            DDI = ValueHelper.GetHextStr(ReadDid(didHi, didLo, true)).Replace(" ", "");
        }

        [Description("写入并读取DDI")]
        public void WriteAndReadDDI(string ddiHex)
        {
            var bs = new List<byte>();
            var didHi = (byte)0xF0;
            var didLo = (byte)0x9A;
            try
            {
                for (var i = 0; i < ddiHex.Length; i = i + 2)
                    bs.Add(Convert.ToByte(ddiHex.Substring(i, 2)));
            }
            catch (Exception)
            {
                DDI = ValueHelper.GetHextStr(ReadDid(didHi, didLo, true)).Replace(" ", "");
                return;
            }

            WriteDid(didHi, didLo, bs.ToArray());
            DDI = ValueHelper.GetHextStr(ReadDid(didHi, didLo, true)).Replace(" ", "");
        }

        [Description("R,MTC值")]
        public string MTC = string.Empty;
        [Description("写入并读取MTC")]
        public void WriteAndReadMtc()
        {
            var tp = Encoding.ASCII.GetString(ReadDid(0xF0, 0xB4, true));
            if (_trackEcuData is null)
            {
                MTC = "NG 无可用的追溯信息 仅读取模块内容:" + tp + "，无法校验";
                return;
            }

            if (tp == _trackEcuData.TrackInfo)
            {
                MTC = "OK " + tp;
                return;
            }

            Thread.Sleep(15);
            var writeBs = Encoding.ASCII.GetBytes(_trackEcuData.TrackInfo);
            if (_trackEcuData.IsUsage != 1)
                WriteDid(0xF0, 0xB4, writeBs);
            Thread.Sleep(150);
            var tpr = Encoding.ASCII.GetString(ReadDid(0xF0, 0xB4, true));
            if (string.Equals(_trackEcuData.TrackInfo, tpr, StringComparison.CurrentCultureIgnoreCase))
            {
                MTC = "OK " + tpr;
                if (_trackEcuData.IsUsage != 1)
                    SaveLocalTrack();
            }
            else
            {
                MTC = string.Format("NG 需写入:{0}，实际写入:{1}，校验错误", _trackEcuData.TrackInfo, tpr);
                _trackEcuData = null;
            }
        }

        [Description("debug用只读取MTC")]
        public void DebugReadMtc()
        {
            //var writeBs = Encoding.ASCII.GetBytes("SM25360A00TT1119");
            //WriteDid(0xF0, 0xB4, writeBs);
            //Thread.Sleep(155);
            MTC = string.Empty;
            MTC = ReadDid(0xF0, 0xB4).GetStringByAsciiBytes(true); /*Encoding.ASCII.GetString(ReadDid(0xF0, 0xB4));*/
        }

        //[Description("debug一键写入并读取")]
        //public void DebugWriteMtc()
        //{
        //    PowerModeOff();


        //    var writeBs = Encoding.ASCII.GetBytes("SM25360A00TT1119");
        //    WriteDid(0xF0, 0xB4, writeBs);
        //    Thread.Sleep(155);
        //    MTC = Encoding.ASCII.GetString(ReadDid(0xF0, 0xB4));
        //}

        [Description("R,ECU-ID值")]
        public string ECUID = string.Empty;
        [Description("写入并读取ECUID")]
        public void WriteAndReadECUID()
        {
            var tp = ValueHelper.GetHextStr(ReadDid(0xF0, 0xF3, true)).Replace(" ", "");

            if (_trackEcuData is null)
            {
                ECUID = "NG 无可用的追溯信息 仅读取模块内容:" + tp + "，无法校验";
                return;
            }

            var writeBs = new List<byte>();
            try
            {
                for (var i = 0; i < _trackEcuData.EcuId.Length; i = i + 2)
                {
                    writeBs.Add(Convert.ToByte(_trackEcuData.EcuId.Substring(i, 2), 16));
                }
            }
            catch (Exception)
            {
                ECUID = "NG ECU ID转换错误";
                return;
            }

            if (string.Equals(_trackEcuData.EcuId, tp, StringComparison.CurrentCultureIgnoreCase))
            {
                ECUID = "OK " + tp;
                return;
            }

            Thread.Sleep(15);
            WriteDid(0xF0, 0xF3, writeBs.ToArray());
            Thread.Sleep(15);
            var tpr = ValueHelper.GetHextStr(ReadDid(0xF0, 0xF3, true)).Replace(" ", "");
            if (string.Equals(_trackEcuData.EcuId, tpr, StringComparison.CurrentCultureIgnoreCase))
            {
                ECUID = "OK " + tpr;
            }
            else
            {
                ECUID = string.Format("NG 需写入:{0}，实际写入:{1}，校验错误", _trackEcuData.EcuId, tpr);
            }
        }

        [Description("debug用只读取ECUID")]
        public void DebugReadEUCID()
        {
            //var tp = new byte[16];
            //tp[15] = 0x02;
            //WriteDid(0xF0, 0xF3, tp);
            //Thread.Sleep(150);
            ECUID = string.Empty;
            ECUID = ValueHelper.GetHextStr(ReadDid(0xF0, 0xF3, true)).Replace(" ", "");
        }

        [Description("R,写入读取编程日期-F199的结果")]
        public string ProgrammingDate = string.Empty;
        [Description("R,编程日期-F199的实际读取值")]
        public string ProgrammingDateStr = string.Empty;

        [Description("读取刷新日期-F199")]
        public void ReadProgrammingDate()
        {
            ProgrammingDateStr = string.Empty;
            ProgrammingDateStr = ValueHelper.GetHextStr(ReadDid(0xF1, 0x99, true)).Replace(" ", "");
        }

        [Description("写入并读取刷新日期-F199")]
        public void WriteAndReadProgrammingDate()
        {
            ProgrammingDate = string.Empty;
            ProgrammingDateStr = string.Empty;

            string str;

            var dateTime = DateTime.Now;
            str = string.Format("{0}{1}{2}", dateTime.Year, dateTime.Month.ToString().PadLeft(2, '0'),
               dateTime.Day.ToString().PadLeft(2, '0'));
            var bs = new List<byte>();
            for (var i = 0; i < str.Length; i = i + 2)
                bs.Add(Convert.ToByte(str.Substring(i, 2), 16));

            WriteDid(0xF1, 0x99, bs.ToArray());

            Thread.Sleep(100);
            var read = ValueHelper.GetHextStr(ReadDid(0xF1, 0x99, true)).Replace(" ", "");
            ProgrammingDateStr = read;
            if (read == str)
                ProgrammingDate = "OK " + read;
            else
                ProgrammingDate = "NG " + read;
        }

        public void DebugWriteAndReadProgrammingDate()
        {
            ProgrammingDate = string.Empty;

            string str;

            var dateTime = DateTime.Now;

            var rd = new Random();
            var rb = rd.Next(0, 50);
            dateTime = dateTime.AddDays(0 - rb);

            str = string.Format("{0}{1}{2}", dateTime.Year, dateTime.Month.ToString().PadLeft(2, '0'),
               dateTime.Day.ToString().PadLeft(2, '0'));
            var bs = new List<byte>();
            for (var i = 0; i < str.Length; i = i + 2)
                bs.Add(Convert.ToByte(str.Substring(i, 2), 16));

            WriteDid(0xF1, 0x99, bs.ToArray());

            Thread.Sleep(100);
            var read = ValueHelper.GetHextStr(ReadDid(0xF1, 0x99)).Replace(" ", "");

            if (read == str)
                ProgrammingDate = "OK " + read;
            else
                ProgrammingDate = "NG " + read;
        }

        [Description("R,MEC值")]
        public int ManufacturersEnableCounter = int.MinValue;

        [Description("读MEC值")]
        public void ReadManufacturersEnableCounter()
        {
            ManufacturersEnableCounter = int.MinValue;
            var result = ReadDid(0xF1, 0xA0, true);
            ManufacturersEnableCounter = ValueHelper.GetDecimal(result);
        }

        [Description("写MEC值")]
        public void WriteManufacturersEnableCounter(int value)
        {
            if (value >= 0 && value <= 254)
            {
                WriteDid(0xF1, 0xA0, new byte[] { (byte)value });
            }
        }

        [Description("R,充电状态")]
        public string ChargingState = string.Empty;
        [Description("R,错误码")]
        public string ErrorCode = string.Empty;

        [Description("读取充电状态")]
        public void ReadChargingState()
        {
            ChargingState = string.Empty;
            var r = ReadDid(0xFD, 0x0C);
            if (r.Length >= 1)
            {
                if (r[0] == 0x00)
                {
                    ChargingState = "00:没有充电";
                }
                else if (r[0] == 0x01)
                {
                    ChargingState = "01:正在充电";
                }
                else
                {
                    ChargingState = ValueHelper.GetHextStr(r[0]) + ":其他";
                }
            }
        }

        [Description("读取错误")]
        public void ReadErrorCode()
        {
            ErrorCode = string.Empty;

            var r = ReadDid(0xFD, 0x10);
            if (r.Length >= 1)
            {
                if (r[0] == 0x00)
                {
                    ErrorCode = "00:无错误";
                }
                else if (r[0] == 0x01)
                {
                    ErrorCode = "01:无线充关";
                }
                else if (r[0] == 0x02)
                {
                    ErrorCode = "02:电源错误";
                }
                else if (r[0] == 0x03)
                {
                    ErrorCode = "03:QFOD";
                }
                else if (r[0] == 0x04)
                {
                    ErrorCode = "04:PFOD";
                }
                else if (r[0] == 0x05)
                {
                    ErrorCode = "05:过压";
                }
                else if (r[0] == 0x06)
                {
                    ErrorCode = "06:欠压";
                }
                else if (r[0] == 0x07)
                {
                    ErrorCode = "07:过温";
                }
                else if (r[0] == 0x08)
                {
                    ErrorCode = "08:PEPS";
                }
                else if (r[0] == 0x09)
                {
                    ErrorCode = "09:卡保护";
                }
                else if (r[0] == 0x0A)
                {
                    ErrorCode = "0A:风扇不转";
                }
                else
                {
                    ErrorCode = ValueHelper.GetHextStr(r[0]) + ":其他";
                }
            }
        }

        private List<byte> _uds22Buffer = new List<byte>();
        private bool _bReadDid;
        private bool _bReadDidOk;
        private bool _bReadSecodnFrame;
        //private ManualResetEventSlim _bReadDidWaitHandle = new ManualResetEventSlim(false);

        private byte[] ReadDid(byte didHi, byte didLo, bool isCheckSpeicalWord = false)
        {
            if (CanFD is null)
                return new byte[0];

            byte[] echo;
            if (CanFdUds22(didHi, didLo, out echo))
            {
                if (isCheckSpeicalWord)
                {
                    if (CheckSpeicalWord(echo))
                    {
                        return echo;
                    }
                    else
                    {
                        Thread.Sleep(100);
                        if (CanFdUds22(didHi, didLo, out echo))
                        {
                            Thread.Sleep(50);
                            return echo;
                        }
                        else
                        {
                            Thread.Sleep(50);
                            return new byte[0];
                        }
                    }
                }

                Thread.Sleep(50);
                return echo;
            }

            Thread.Sleep(100);

            if (CanFdUds22(didHi, didLo, out echo))
            {
                if (isCheckSpeicalWord)
                {
                    if (CheckSpeicalWord(echo))
                    {
                        return echo;
                    }
                    else
                    {
                        Thread.Sleep(100);
                        if (CanFdUds22(didHi, didLo, out echo))
                        {
                            Thread.Sleep(50);
                            return echo;
                        }
                        else
                        {
                            Thread.Sleep(50);
                            return new byte[0];
                        }
                    }
                }

                Thread.Sleep(50);
                return echo;
            }

            Thread.Sleep(50);
            return new byte[0];
        }

        private bool CanFdUds22(byte didHi, byte didLo, out byte[] echo)
        {
            lock (_lockCanSend)
            {
                _uds22Buffer.Clear();

                if (CanFD is null)
                {
                    echo = new byte[0];
                    return false;
                }

                //_bReadDidWaitHandle.Reset();
                _bReadDidOk = false;
                _bReadDid = true;

                var firstSend = new byte[] { 0x03, 0x22, didHi, didLo, 0x00, 0x00, 0x00, 0x00 };
                var t1 = HighPrecisionTimer.GetTimestamp();

                //使用 ThreadPool 异步发送，减少 Task 创建开销
                ThreadPool.QueueUserWorkItem(_ => CanFD.SendExtendedCanFdData(DiagnosticReqCanId, firstSend));

                //var sendAct = new Action(() => { CanFD.SendExtendedCanFdData(DiagnosticReqCanId, firstSend); });
                //sendAct.BeginInvoke(null, null);

                // 同步等待响应
                //var isFirstRecvOk = _bReadDidWaitHandle.Wait(500);
                var isFirstRecvOk = false;
                while (true)
                {
                    Thread.Sleep(1);

                    if (_bReadDidOk)
                    {
                        isFirstRecvOk = true;
                        break;
                    }

                    var ns = HighPrecisionTimer.GetTimestamp();
                    var tts = HighPrecisionTimer.GetTimestampIntervalMs(t1, ns);
                    if (tts > 500)
                        break;
                }

                _bReadDidOk = false;
                _bReadDid = false;
                //_bReadDidWaitHandle.Reset();

                var t2 = HighPrecisionTimer.GetTimestamp();
                Console.WriteLine("read did cost: {0}/ms", HighPrecisionTimer.GetTimestampIntervalMs(t1, t2));

                if (isFirstRecvOk)
                {
                    var bufferArray = _uds22Buffer.ToArray();
                    var count = bufferArray.Length;

                    if (count >= 4)
                    {
                        var b1 = bufferArray[0];
                        var b2 = bufferArray[1];
                        var b3 = bufferArray[2];
                        var b4 = bufferArray[3];

                        if (count == 8 && b1.GetByteHighOrder() == 0x00 && b2 == 0x62 && b3 == didHi && b4 == didLo)
                        {
                            var datalen = b1.GetByteLowOrder();
                            if (datalen >= 3 && datalen <= 7)
                            {
                                echo = new byte[datalen - 3];
                                Array.Copy(bufferArray, 4, echo, 0, datalen - 3);
                                return true;
                            }
                        }
                        else if (count > 8 && b1.GetByteHighOrder() == 0x00)
                        {
                            var datalen = b2;
                            if (datalen >= 3 && datalen <= 62 && count >= datalen + 2)
                            {
                                echo = new byte[datalen - 3];
                                Array.Copy(bufferArray, 5, echo, 0, datalen - 3);
                                return true;
                            }
                        }
                    }
                }

                echo = new byte[0];
                return false;
            }
        }

        private bool CheckSpeicalWord(byte[] value)
        {
            if (value is null || value.Length == 0)
            {
                return true;
            }

            var isOk = true;
            for (var i = 0; i < value.Length; i++)
            {
                if ((value[i] >= 0x30 && value[i] <= 0x39) || (value[i] >= 0x41 && value[i] <= 0x5A) || (value[i] >= 0x61 && value[i] <= 0x7A))
                {
                    continue;
                }
                else
                {
                    isOk = false;
                    break;
                }
            }

            return isOk;
        }

        private bool _bInWriteDID;

        private bool WriteDid(byte didHi, byte didLo, byte[] data)
        {
            if (CanFD is null)
                return false;

            if (data is null || data.Length is 0 || data.Length >= 32)
                return false;

            //return CanFD.CanBusWithUds.TryWriteData(DiagnosticReqCanId, DiagnosticRecvCanId, CanBus.CanType.Extended, CanBus.CanProtocol.CanFd, didHi, didLo, data, pendingByte: 0x00);

            lock (_lockCanSend)
            {
                _bInWriteDID = true;

                if (data.Length > 0 && data.Length <= 4)
                {
                    var dLen = (byte)(1 + 2 + data.Length);
                    var sendBytes = new byte[8] { dLen, 0x2E, didHi, didLo, 0x00, 0x00, 0x00, 0x00 };
                    Array.Copy(data, 0, sendBytes, 4, data.Length);
                    CanFD.SendExtendedCanFdData(DiagnosticReqCanId, sendBytes);
                }
                else
                {
                    var count = (data.Length + 2 + 1 + 2) / 8;
                    var restCount = (data.Length + 2 + 1 + 2) % 8;
                    if (restCount != 0)
                    {
                        count++;
                    }

                    var sendBytes = new byte[count * 8];

                    var dLen = (ushort)(1 + 2 + data.Length);
                    var dLenBytes = BitConverter.GetBytes(dLen).Reverse().ToArray();
                    sendBytes[0] = dLenBytes[0];
                    sendBytes[1] = dLenBytes[1];
                    sendBytes[2] = 0x2E;
                    sendBytes[3] = didHi;
                    sendBytes[4] = didLo;
                    Array.Copy(data, 0, sendBytes, 5, data.Length);
                    for (var i = 0; i < (8 - restCount); i++)
                        sendBytes[sendBytes.Length - 1 - i] = 0xCC;
                    CanFD.SendExtendedCanFdData(DiagnosticReqCanId, sendBytes);
                }

                Thread.Sleep(200);
                _bInWriteDID = false;

                return true;
            }
        }

        [Description("关闭正常通信")]
        public void DisableRxAndTxCommunication()
        {
            if (CanFD is null)
                return;

            lock (_lockCanSend)
            {
                CanFD.SendExtendedCanFdData(DiagnosticReqCanId, new byte[8] { 0x03, 0x28, 0x03, 0x03, 0xCC, 0xCC, 0xCC, 0xCC });
                Thread.Sleep(200);
            }

            //CanFD.CanBusWithUds.TryCommunicationControl(
            //    DiagnosticReqCanId,
            //    DiagnosticRecvCanId,
            //    CanBus.CanType.Extended,
            //    Uds14229Helper.CommunicationControl.DisableRxAndTx);
        }

        [Description("R,清除DTC结果")]
        public string ClearDtcResult = string.Empty;
        [Description("R,读取DTC结果")]
        public string ReadDtcResult = string.Empty;

        [Description("清除DTC")]
        public void ClearDtc()
        {
            ClearDtcResult = string.Empty;

            if (CanFD is null)
                return;

            lock (_lockCanSend)
                ClearDtcResult = CanFD.CanBusWithUds.TryClearAllGroupsDiagnosticInformation(DiagnosticReqCanId, DiagnosticRecvCanId, CanBus.CanType.Extended, pendingByte: 0x00, canProtocol: CanBus.CanProtocol.CanFd) ? "OK" : "NG";
        }

        [Description("读取DTC")]
        public void ReadDtc()
        {
            ReadDtcResult = string.Empty;

            if (CanFD is null)
                return;

            lock (_lockCanSend)
            {
                byte[] echo;
                if (CanFD.CanBusWithUds.TryReadDtcInfomation(DiagnosticReqCanId, DiagnosticRecvCanId, CanBus.CanType.Extended, 0x02, 0x2F, out echo, canProtocol: CanBus.CanProtocol.CanFd, pendingByte: 0x00))
                {
                    if (echo != null)
                    {
                        if (echo.Length == 0)
                        {
                            ReadDtcResult = "NoError";
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

                                foreach (var t in readCodes)
                                    ReadDtcResult += string.Format("[{0}];", t.Remark);
                            }
                            else
                            {
                                ReadDtcResult = "ReadDtcResLenError";
                            }
                        }
                    }
                }
                else
                {
                    ReadDtcResult = "ReadFailed";
                }
            }
        }

        #endregion

        #region 安全信息

        private const byte _constMasterKeySlot = 0xFF;
        private readonly string _constMasterM1 = "00000000000000000000000000000011";
        private readonly string _constMasterM2 = "B6E8D3ADC981A512CF8E5BB5537249D6A4556CB812D50C916AA3670F884038BC";
        private readonly string _constMasterM3 = "18BC889D2F2BD58E4DC30409F571E82E";

        private const byte _constUnlockKeySlot = 0x01;
        private readonly string _constUnlockM1 = "00000000000000000000000000000044";
        private readonly string _constUnlockM2 = "B6E8D3ADC981A512CF8E5BB5537249D6A4556CB812D50C916AA3670F884038BC";
        private readonly string _constUnlockM3 = "CB02D04EF3D782DFB2B3EFBD2FD20B1F";

        private readonly string _correctMasterM4 = "11EE52F069492A9D3D1283557EA645746C";
        private readonly string _correctUnlockM4 = "44EE52F069492A9D3D1283557EA645746C";

        [Description("R,注入固定KEY结果")]
        public string InjectConstKeyResult = string.Empty;

        [Description("注入固定KEY")]
        public void InjectConstKey()
        {
            InjectConstKeyResult = string.Empty;

            if (_trackEcuData is null)
                return;

            var masterKeysM1 = new List<byte>();
            var masterKeysM2 = new List<byte>();
            var masterKeysM3 = new List<byte>();
            for (var i = 0; i < _constMasterM1.Length; i = i + 2)
                masterKeysM1.Add(Convert.ToByte(_constMasterM1.Substring(i, 2), 16));
            for (var i = 0; i < _constMasterM2.Length; i = i + 2)
                masterKeysM2.Add(Convert.ToByte(_constMasterM2.Substring(i, 2), 16));
            for (var i = 0; i < _constMasterM3.Length; i = i + 2)
                masterKeysM3.Add(Convert.ToByte(_constMasterM3.Substring(i, 2), 16));

            var unlockKeysM1 = new List<byte>();
            var unlockKeysM2 = new List<byte>();
            var unlockKeysM3 = new List<byte>();
            for (var i = 0; i < _constUnlockM1.Length; i = i + 2)
                unlockKeysM1.Add(Convert.ToByte(_constUnlockM1.Substring(i, 2), 16));
            for (var i = 0; i < _constUnlockM2.Length; i = i + 2)
                unlockKeysM2.Add(Convert.ToByte(_constUnlockM2.Substring(i, 2), 16));
            for (var i = 0; i < _constUnlockM3.Length; i = i + 2)
                unlockKeysM3.Add(Convert.ToByte(_constUnlockM3.Substring(i, 2), 16));

            var isCheckMasterKeyOk = false;
            var isCheckUnlockKeyOk = false;

            var errorInfo = string.Empty;

            if (_trackEcuData.IsConstMasterKeyOk == 0)
            {
                StartKeyLog("MasterKey固定值");
                byte masterKeyWriteHistory;
                byte[] masterKeyM4;
                byte[] masterKeyM5;
                if (UnlockSeedKey(_constMasterKeySlot, masterKeysM1.ToArray(), masterKeysM2.ToArray(), masterKeysM3.ToArray(), out masterKeyWriteHistory, out masterKeyM4, out masterKeyM5))
                {
                    StopKeyLog();
                    var toCheckBytes = new byte[17];
                    Array.Copy(masterKeyM4, 15, toCheckBytes, 0, 17);
                    var convertStr = ValueHelper.GetHextStr(toCheckBytes).Replace(" ", "");
                    isCheckMasterKeyOk = masterKeyWriteHistory == 0x00 && string.Equals(convertStr, _correctMasterM4, StringComparison.CurrentCultureIgnoreCase);
                    _trackEcuData.IsConstMasterKeyOk = isCheckMasterKeyOk ? 1 : 0;
                    SaveLocalTrack();

                    if (_trackEcuData.IsConstMasterKeyOk != 1)
                    {
                        errorInfo += string.Format(" masterKey status: {0} ", ValueHelper.GetHextStrWithOx(masterKeyWriteHistory));
                        SaveKeyLog();
                    }
                }
                else
                {
                    errorInfo += " masterKey uds failed ";
                    StopKeyLog(25);
                    SaveKeyLog();
                }
            }
            else
            {
                isCheckMasterKeyOk = true;
            }

            if (isCheckMasterKeyOk)
            {
                if (_trackEcuData.IsConstUnlockKeyOk == 0)
                {
                    StartKeyLog("UnlockKey固定值");
                    byte unlockKeyWriteHistory;
                    byte[] unlockKeyM4;
                    byte[] unlockKeyM5;
                    if (UnlockSeedKey(_constUnlockKeySlot, unlockKeysM1.ToArray(), unlockKeysM2.ToArray(), unlockKeysM3.ToArray(), out unlockKeyWriteHistory, out unlockKeyM4, out unlockKeyM5))
                    {
                        StopKeyLog();
                        var toCheckBytes = new byte[17];
                        Array.Copy(unlockKeyM4, 15, toCheckBytes, 0, 17);
                        var convertStr = ValueHelper.GetHextStr(toCheckBytes).Replace(" ", "");
                        isCheckUnlockKeyOk = unlockKeyWriteHistory == 0x00 && string.Equals(convertStr, _correctUnlockM4, StringComparison.CurrentCultureIgnoreCase);
                        _trackEcuData.IsConstUnlockKeyOk = isCheckUnlockKeyOk ? 1 : 0;
                        SaveLocalTrack();

                        if (_trackEcuData.IsConstUnlockKeyOk != 1)
                        {
                            errorInfo += string.Format(" unlockKey status: {0} ", ValueHelper.GetHextStrWithOx(unlockKeyWriteHistory));
                            SaveKeyLog();
                        }
                    }
                    else
                    {
                        errorInfo += " unlockKey uds failed ";
                        StopKeyLog(25);
                        SaveKeyLog();
                    }
                }
                else
                {
                    isCheckUnlockKeyOk = true;
                }
            }

            InjectConstKeyResult = isCheckMasterKeyOk && isCheckUnlockKeyOk ? "OK" : "NG" + errorInfo;
        }

        public void InjectIECSKey()
        {
            MasterKey = string.Empty;
            UnlockKey = string.Empty;

            if (_trackEcuData is null)
                return;

            if (_trackEcuData.IsConstUnlockKeyOk == 0 || _trackEcuData.IsConstUnlockKeyOk == 0)
            {
                MasterKey = "NG 未先注入固定值的KEY";
                UnlockKey = "NG 未先注入固定值的KEY";
                return;
            }

            var masterKeysM1 = new List<byte>();
            var masterKeysM2 = new List<byte>();
            var masterKeysM3 = new List<byte>();
            var masterKeySlot = (byte)0x00;

            if (_trackEcuData.MASTER_ECU_KEY_KeySlot.Length == 2 &&
                _trackEcuData.MASTER_ECU_KEY_M1.Length == 32 &&
                _trackEcuData.MASTER_ECU_KEY_M2.Length == 64 &&
                _trackEcuData.MASTER_ECU_KEY_M3.Length == 32)
            {
                try
                {
                    masterKeySlot = Convert.ToByte(_trackEcuData.MASTER_ECU_KEY_KeySlot, 16);

                    for (var i = 0; i < _trackEcuData.MASTER_ECU_KEY_M1.Length; i = i + 2)
                        masterKeysM1.Add(Convert.ToByte(_trackEcuData.MASTER_ECU_KEY_M1.Substring(i, 2), 16));
                    for (var i = 0; i < _trackEcuData.MASTER_ECU_KEY_M2.Length; i = i + 2)
                        masterKeysM2.Add(Convert.ToByte(_trackEcuData.MASTER_ECU_KEY_M2.Substring(i, 2), 16));
                    for (var i = 0; i < _trackEcuData.MASTER_ECU_KEY_M3.Length; i = i + 2)
                        masterKeysM3.Add(Convert.ToByte(_trackEcuData.MASTER_ECU_KEY_M3.Substring(i, 2), 16));
                }
                catch (Exception)
                {
                    MasterKey = "NG 获取到的ICES KEY异常";
                    UnlockKey = "NG 获取到的ICES KEY异常";
                    return;
                }
            }

            var unlockKeysM1 = new List<byte>();
            var unlockKeysM2 = new List<byte>();
            var unlockKeysM3 = new List<byte>();
            var unlockKeySlot = (byte)0x00;

            if (_trackEcuData.UNLOCK_ECU_KEY_KeySlot.Length == 2 &&
                _trackEcuData.UNLOCK_ECU_KEY_M1.Length == 32 &&
                _trackEcuData.UNLOCK_ECU_KEY_M2.Length == 64 &&
                _trackEcuData.UNLOCK_ECU_KEY_M3.Length == 32)
            {
                try
                {
                    unlockKeySlot = Convert.ToByte(_trackEcuData.UNLOCK_ECU_KEY_KeySlot, 16);

                    for (var i = 0; i < _trackEcuData.UNLOCK_ECU_KEY_M1.Length; i = i + 2)
                        unlockKeysM1.Add(Convert.ToByte(_trackEcuData.UNLOCK_ECU_KEY_M1.Substring(i, 2), 16));
                    for (var i = 0; i < _trackEcuData.UNLOCK_ECU_KEY_M2.Length; i = i + 2)
                        unlockKeysM2.Add(Convert.ToByte(_trackEcuData.UNLOCK_ECU_KEY_M2.Substring(i, 2), 16));
                    for (var i = 0; i < _trackEcuData.UNLOCK_ECU_KEY_M3.Length; i = i + 2)
                        unlockKeysM3.Add(Convert.ToByte(_trackEcuData.UNLOCK_ECU_KEY_M3.Substring(i, 2), 16));
                }
                catch (Exception)
                {
                    MasterKey = "NG 获取到的ICES KEY异常";
                    UnlockKey = "NG 获取到的ICES KEY异常";
                    return;
                }
            }

            var isCheckMasterKeyOk = false;
            var isCheckUnlockKeyOk = false;

            var mErrorInfo = string.Empty;
            var uErrorInfo = string.Empty;

            if (string.IsNullOrEmpty(_trackEcuData.MASTER_ECU_KEY_M4) ||
                string.IsNullOrEmpty(_trackEcuData.MASTER_ECU_KEY_M5) ||
                _trackEcuData.MASTER_ECU_KEY_M4.Length != 64 || _trackEcuData.MASTER_ECU_KEY_M5.Length != 32)
            {
                StartKeyLog("MasterKey_IECS值");
                byte masterKeyWriteHistory;
                byte[] masterKeyM4;
                byte[] masterKeyM5;
                if (UnlockSeedKey(masterKeySlot, masterKeysM1.ToArray(), masterKeysM2.ToArray(), masterKeysM3.ToArray(), out masterKeyWriteHistory, out masterKeyM4, out masterKeyM5))
                {
                    StopKeyLog();
                    var toCheckBytes = new byte[17];
                    Array.Copy(masterKeyM4, 15, toCheckBytes, 0, 17);
                    var convertStr = ValueHelper.GetHextStr(toCheckBytes).Replace(" ", "");
                    isCheckMasterKeyOk = masterKeyWriteHistory == 0x00 && masterKeyM4 != null && masterKeyM4.Length == 32 && masterKeyM5 != null && masterKeyM5.Length == 16;
                    if (isCheckMasterKeyOk)
                    {
                        _trackEcuData.MASTER_ECU_KEY_M4 = ValueHelper.GetHextStr(masterKeyM4).Replace(" ", "");
                        _trackEcuData.MASTER_ECU_KEY_M5 = ValueHelper.GetHextStr(masterKeyM5).Replace(" ", "");
                        SaveLocalTrack();
                    }
                    else
                    {
                        mErrorInfo += string.Format(" masterKey status: {0} ", ValueHelper.GetHextStrWithOx(masterKeyWriteHistory));
                        SaveKeyLog();
                    }
                }
                else
                {
                    mErrorInfo += " masterKey uds failed ";
                    StopKeyLog(25);
                    SaveKeyLog();
                }
            }
            else if (!string.IsNullOrEmpty(_trackEcuData.MASTER_ECU_KEY_M4) && _trackEcuData.MASTER_ECU_KEY_M4.Length == 64 && _trackEcuData.MASTER_ECU_KEY_M5.Length == 32)
            {
                isCheckMasterKeyOk = true;
            }

            if (isCheckMasterKeyOk)
            {
                if (string.IsNullOrEmpty(_trackEcuData.UNLOCK_ECU_KEY_M4) ||
                    string.IsNullOrEmpty(_trackEcuData.UNLOCK_ECU_KEY_M5) ||
                    _trackEcuData.UNLOCK_ECU_KEY_M4.Length != 64 ||
                    _trackEcuData.UNLOCK_ECU_KEY_M5.Length != 32)
                {
                    StartKeyLog("UnlockKey_IECS值");
                    byte unlockKeyWriteHistory;
                    byte[] unlockKeyM4;
                    byte[] unlockKeyM5;
                    if (UnlockSeedKey(unlockKeySlot, unlockKeysM1.ToArray(), unlockKeysM2.ToArray(), unlockKeysM3.ToArray(), out unlockKeyWriteHistory, out unlockKeyM4, out unlockKeyM5))
                    {
                        StopKeyLog();
                        var toCheckBytes = new byte[17];
                        Array.Copy(unlockKeyM4, 15, toCheckBytes, 0, 17);
                        var convertStr = ValueHelper.GetHextStr(toCheckBytes).Replace(" ", "");
                        isCheckUnlockKeyOk = unlockKeyWriteHistory == 0x00 && unlockKeyM4 != null && unlockKeyM4.Length == 32 && unlockKeyM5 != null && unlockKeyM5.Length == 16;
                        if (isCheckUnlockKeyOk)
                        {
                            _trackEcuData.UNLOCK_ECU_KEY_M4 = ValueHelper.GetHextStr(unlockKeyM4).Replace(" ", "");
                            _trackEcuData.UNLOCK_ECU_KEY_M5 = ValueHelper.GetHextStr(unlockKeyM5).Replace(" ", "");
                            SaveLocalTrack();
                        }
                        else
                        {
                            uErrorInfo += string.Format(" unlockKey status: {0} ", ValueHelper.GetHextStrWithOx(unlockKeyWriteHistory));
                            SaveKeyLog();
                        }
                    }
                    else
                    {
                        uErrorInfo += " unlockKey uds failed ";
                        StopKeyLog(25);
                        SaveKeyLog();
                    }
                }
                else if (!string.IsNullOrEmpty(_trackEcuData.UNLOCK_ECU_KEY_M4) && _trackEcuData.UNLOCK_ECU_KEY_M4.Length == 64 && _trackEcuData.UNLOCK_ECU_KEY_M5.Length == 32)
                {
                    isCheckUnlockKeyOk = true;
                }
            }

            MasterKey = isCheckMasterKeyOk ? "OK" : "NG" + mErrorInfo;
            UnlockKey = isCheckUnlockKeyOk ? "OK" : "NG" + uErrorInfo;
        }

        [Description("R,Step1-MasterKey注入结果")]
        public string MasterKey = string.Empty;

        [Description("R,Step2-UnlockKey注入结果")]
        public string UnlockKey = string.Empty;

        private bool _bInJnjectKey;
        private bool UnlockSeedKey(
            byte keySlot,
            IEnumerable<byte> m1, IEnumerable<byte> m2, IEnumerable<byte> m3,
            out byte wrtieHistory, out byte[] m4, out byte[] m5)
        {
            if (CanFD is null)
            {
                m4 = new byte[] { };
                m5 = new byte[] { };
                wrtieHistory = 0xFF;
                return false;
            }

            lock (_lockCanSend)
            {
                {
                    var routineControlOptBytes = new List<byte> { keySlot };
                    routineControlOptBytes.AddRange(m1);
                    routineControlOptBytes.AddRange(m2);
                    routineControlOptBytes.AddRange(m3);

                    byte[] echoBytes;
                    if (CanFdUdsRid272(routineControlOptBytes.ToArray(), out echoBytes))
                    {
                        if (echoBytes.Length == 49)
                        {
                            wrtieHistory = echoBytes[0];

                            m4 = new byte[32];
                            Array.Copy(echoBytes, 1, m4, 0, 32);

                            m5 = new byte[16];
                            Array.Copy(echoBytes, 33, m5, 0, 16);

                            return true;
                        }
                    }
                }

                m4 = new byte[] { };
                m5 = new byte[] { };
                wrtieHistory = 0xFF;
                return false;
            }
        }

        private List<byte> _udsRid272Buffer = new List<byte>();
        private bool _bRid272;
        private bool _bRid272SecodnFrame;

        private bool _bRid272Ok;
        private bool _bRid272SecondFrameOk;

        //private ManualResetEventSlim _bRid272WaitHandle = new ManualResetEventSlim(false);

        private bool CanFdUdsRid272(byte[] routineControlOptBytes, out byte[] echoBytes)
        {
            if (CanFD is null)
            {
                echoBytes = new byte[0];
                return false;
            }

            if (routineControlOptBytes is null || routineControlOptBytes.Length != 65)
            {
                echoBytes = new byte[0];
                return false;
            }

            var firstSendBytes = new byte[64];
            firstSendBytes[0] = 0x10;
            firstSendBytes[1] = 0x45;
            firstSendBytes[2] = 0x31;
            firstSendBytes[3] = 0x01;
            firstSendBytes[4] = 0x02;
            firstSendBytes[5] = 0x72;
            Array.Copy(routineControlOptBytes, 0, firstSendBytes, 6, 64 - 6);

            var secondSendBytes = new byte[8];
            secondSendBytes[0] = 0x21;
            Array.Copy(routineControlOptBytes, 65 - 7, secondSendBytes, 1, 7);

            //_bRid272WaitHandle.Reset();
            _bRid272Ok = false;
            _bRid272SecondFrameOk = false;
            _bRid272SecodnFrame = true;
            _bInJnjectKey = true;

            var t1 = HighPrecisionTimer.GetTimestamp();

            // 使用 ThreadPool 异步发送，减少 Task 创建开销
            ThreadPool.QueueUserWorkItem(_ => CanFD.SendExtendedCanFdData(DiagnosticReqCanId, firstSendBytes));
            var isFirstRecvOk = false;//_bRid272WaitHandle.Wait(500);
            while (true)
            {
                Thread.Sleep(1);

                if (_bRid272SecondFrameOk)
                {
                    isFirstRecvOk = true;
                    break;
                }

                var ns = HighPrecisionTimer.GetTimestamp();
                var tts = HighPrecisionTimer.GetTimestampIntervalMs(t1, ns);
                if (tts > 500)
                    break;
            }

            _bRid272SecodnFrame = false;
            _bRid272SecondFrameOk = false;
            _bRid272SecodnFrame = true;
            //_bRid272WaitHandle.Reset();

            if (isFirstRecvOk)
            {
                //_bRid272WaitHandle.Reset();
                _bRid272SecodnFrame = false;
                _bRid272SecondFrameOk = false;
                _udsRid272Buffer.Clear();
                _bRid272 = true;

                // 使用 ThreadPool 异步发送，减少 Task 创建开销
                var t2 = HighPrecisionTimer.GetTimestamp();
                ThreadPool.QueueUserWorkItem(_ => CanFD.SendExtendedCanFdData(DiagnosticReqCanId, secondSendBytes));
                var isSencondRecvOk = false;//_bRid272WaitHandle.Wait(500);

                while (true)
                {
                    Thread.Sleep(1);

                    if (_bRid272Ok)
                    {
                        isSencondRecvOk = true;
                        break;
                    }

                    var ns = HighPrecisionTimer.GetTimestamp();
                    var tts = HighPrecisionTimer.GetTimestampIntervalMs(t2, ns);
                    if (tts > 500)
                        break;
                }

                _bRid272 = false;
                _bRid272SecodnFrame = false;
                _bRid272SecondFrameOk = false;
                //_bRid272WaitHandle.Reset();
                _bInJnjectKey = false;
                if (isSencondRecvOk && _udsRid272Buffer.Count == 64)
                {
                    var b0 = _udsRid272Buffer[0];
                    var b1 = _udsRid272Buffer[1];
                    var b2 = _udsRid272Buffer[2];
                    var b3 = _udsRid272Buffer[3];
                    var b4 = _udsRid272Buffer[4];
                    var b5 = _udsRid272Buffer[5];

                    if (b0 == 0x00 && b1 == 0x35 && b2 == 0x71 && b3 == 0x01 && b4 == 0x02 && b5 == 0x72)
                    {
                        echoBytes = new byte[49];
                        Array.Copy(_udsRid272Buffer.ToArray(), 6, echoBytes, 0, 49);
                        return true;
                    }
                }
            }

            _bInJnjectKey = false;
            echoBytes = new byte[0];
            return false;
        }

        private void StartKeyLog(string note)
        {
            _logNote = note;
            _isLogInjectKeyBuffer = true;
        }

        private void StopKeyLog(int delayMs = 0)
        {
            _isLogInjectKeyBuffer = false;
            if (delayMs > 0 && delayMs <= 1000)
                Thread.Sleep(delayMs);
        }

        private void SaveKeyLog()
        {
            if (_injectKeyBuffer.Any() && _logConfig != null)
            {
                using (var _currentWriter = new StreamWriter(_logCsv, true, Encoding.UTF8, 65536))
                {
                    using (var _currentCsv = new CsvWriter(_currentWriter, _logConfig))
                    {
                        if (_logConfig.HasHeaderRecord)
                        {
                            _currentCsv.WriteHeader<KeyLogData>();
                            _currentCsv.NextRecord();
                            _currentWriter.Flush();
                        }

                        foreach (var item in _injectKeyBuffer)
                        {
                            _currentCsv.WriteRecord(item);
                            _currentCsv.NextRecord();
                            _currentWriter.Flush();
                        }
                    }
                }

                _injectKeyBuffer.Clear();
            }
        }

        #endregion

        #region M4M5回传相关

        public List<ECUData> ReadEcuIdsFromServer(DateTime startTime, DateTime endTime, int isUsage)
        {
            var serverIp = ServerIp;

            using (var db = new SqlSugarClient(
                new ConnectionConfig
                {
                    ConnectionString = string.Format(@"server={0};database={1};uid={2};pwd={3}", serverIp, ServerDataBase, ServerUid, ServerPwd),
                    DbType = DbType.SqlServer,
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.SystemTable
                }))
            {
                return db.Queryable<ECUData>().Where(it => it.LastRecordTime >= startTime && it.LastRecordTime <= endTime && it.IsUsage == isUsage).ToList();
            }
        }

        [Description("从配置文件夹读取EcuIds并上传至服务器")]
        public void ReadEcuIdsFromSRecordAndUploadToServer(string ecuidsConfigDirectory)
        {
            try
            {
                if (string.IsNullOrEmpty(ecuidsConfigDirectory))
                    return;

                if (!Directory.Exists(ecuidsConfigDirectory))
                    Directory.CreateDirectory(ecuidsConfigDirectory);

                foreach (var f in Directory.GetFiles(ecuidsConfigDirectory))
                {
                    if (f.ToLower().EndsWith(".s19"))
                    {
                        var fileInfo = new FileInfo(f);

                        if (fileInfo.Name.ToLower().StartsWith("SGM-ECUIDs-".ToLower()))
                        {
                            var lines = SRecordFileHelper.GetSRecordLineData(f);
                            var blocks = SRecordFileHelper.GetBlocks(lines);

                            var listDatas = new List<byte>();

                            foreach (var t in blocks.SelectMany(b => b))
                                listDatas.AddRange(t.Data);

                            if (listDatas.Count % 144 != 0)
                                return;

                            var ecuDatas = new List<ECUData>();

                            for (var i = 0; i < listDatas.Count; i = i + 144)
                            {
                                var ecuIdStruct = new ECUData
                                {
                                    EcuIdFromFile = fileInfo.Name,
                                    CreateTime = DateTime.Now,
                                    LastRecordTime = DateTime.Now,
                                    MASTER_ECU_KEY_KeySlot = "FF",
                                    UNLOCK_ECU_KEY_KeySlot = "01",
                                    Barcode = string.Empty,
                                    TrackInfo = string.Empty,
                                    IsUploadToSgm = 0,
                                    IsUsage = 0,
                                    IsConstMasterKeyOk = 0,
                                    IsConstUnlockKeyOk = 0
                                };

                                for (var j = 0; j < 144; j++)
                                {
                                    if (j >= 0 && j < 16)
                                        ecuIdStruct.EcuId += ValueHelper.GetHextStr(listDatas[i + j]);
                                    else if (j >= 16 && j < 32)
                                        ecuIdStruct.MASTER_ECU_KEY_M1 += ValueHelper.GetHextStr(listDatas[i + j]);
                                    else if (j >= 32 && j < 64)
                                        ecuIdStruct.MASTER_ECU_KEY_M2 += ValueHelper.GetHextStr(listDatas[i + j]);
                                    else if (j >= 64 && j < 80)
                                        ecuIdStruct.MASTER_ECU_KEY_M3 += ValueHelper.GetHextStr(listDatas[i + j]);
                                    else if (j >= 80 && j < 96)
                                        ecuIdStruct.UNLOCK_ECU_KEY_M1 += ValueHelper.GetHextStr(listDatas[i + j]);
                                    else if (j >= 96 && j < 128)
                                        ecuIdStruct.UNLOCK_ECU_KEY_M2 += ValueHelper.GetHextStr(listDatas[i + j]);
                                    else if (j >= 128 && j < 144)
                                        ecuIdStruct.UNLOCK_ECU_KEY_M3 += ValueHelper.GetHextStr(listDatas[i + j]);
                                }

                                ecuDatas.Add(ecuIdStruct);

                                //var existSql = new StringBuilder();

                                //existSql.Append("select count(1) from manufactureSgm458PlgDatas");
                                //existSql.Append(" where EcuId=@EcuId ");
                                //SqlParameter[] parameters =
                                //{
                                //    new SqlParameter("@EcuId", SqlDbType.NVarChar, 100)
                                //};

                                //parameters[0].Value = ecuIdStruct.EucIdHex;

                                //if (!Exists(existSql.ToString(), parameters))
                                //{
                                //    var insertSql = string.Format(
                                //        "insert into manufactureSgm458PlgDatas " +
                                //        "(EcuId,EcuIdFromFile,CreateTime,IsUsage,IsUploadToSgm," +
                                //        "MASTER_ECU_KEY_KeySlot,MASTER_ECU_KEY_M1,MASTER_ECU_KEY_M2,MASTER_ECU_KEY_M3," +
                                //        "UNLOCK_ECU_KEY_KeySlot,UNLOCK_ECU_KEY_M1,UNLOCK_ECU_KEY_M2,UNLOCK_ECU_KEY_M3) " +
                                //        "values " +
                                //        "('{0}','{1}','{2}','{3}','{4}'," +
                                //        "'{5}','{6}','{7}','{8}'," +
                                //        "'{9}','{10}','{11}','{12}')",
                                //        ecuIdStruct.EucIdHex, fileInfo.Name, DateTime.Now, 0, 0,
                                //        "FF", ecuIdStruct.MasterEcuKeyM1Hex, ecuIdStruct.MasterEcuKeyM2Hex,
                                //        ecuIdStruct.MasterEcuKeyM3Hex,
                                //        "01", ecuIdStruct.UnlockEcuKeyM1Hex, ecuIdStruct.UnlockEcuKeyM2Hex,
                                //        ecuIdStruct.UnlockEcuKeyM3Hex);

                                //    Query(insertSql);
                                //}
                            }

                            // 这里要加查重

                            var serverIp = ServerIp;

                            using (var db = new SqlSugarClient(
                                new ConnectionConfig
                                {
                                    ConnectionString = string.Format(@"server={0};database={1};uid={2};pwd={3}", serverIp, ServerDataBase, ServerUid, ServerPwd),
                                    DbType = DbType.SqlServer,
                                    IsAutoCloseConnection = true,
                                    InitKeyType = InitKeyType.SystemTable
                                }))
                            {
                                var insertable = db.Insertable(ecuDatas);
                                insertable.IgnoreColumns("id"); // 忽略自增长列
                                var result = insertable.ExecuteReturnIdentity(); // 执行插入并返回插入数据的ID
                            }
                        }
                    }

                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public bool ReadEcuIdsFromSRecordAndUploadToServerNew(string binFilePath, int toVerifuCount, out string errorMsg)
        {
            errorMsg = string.Empty;

            var fileInfo = new FileInfo(binFilePath);

            var dir = System.Windows.Forms.Application.StartupPath;

            var cmd = new Process();
            var startInfo = new ProcessStartInfo
            {
                FileName = dir + @"\DllImport\OpenPgpDecrypt\OpenPgpDecrypt\OpenPgpDecrypt\bin\Debug\OpenPgpDecrypt.exe",
                Arguments = string.Empty,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            cmd.StartInfo = startInfo;

            var keyStr = string.Empty;
            if (cmd.Start())
            {
                var input = binFilePath;
                var privateKeyPath = dir + @"\DllImport\OpenPgpDecrypt\Seeyao_Private_Key.asc";
                var password = "Scae-1234";

                cmd.StandardInput.WriteLine(input);
                Thread.Sleep(10);
                cmd.StandardInput.WriteLine(privateKeyPath);
                Thread.Sleep(10);
                cmd.StandardInput.WriteLine(password);
                Thread.Sleep(10);

                var enterTs = HighPrecisionTimer.GetTimestamp();

                while (true)
                {
                    var log = cmd.StandardOutput.ReadLine();
                    if (log == null)
                        break;
                    if (!log.StartsWith(@"{""") || !log.EndsWith(@"""}"))
                        continue;

                    var nowTs = HighPrecisionTimer.GetTimestamp();
                    if (HighPrecisionTimer.GetTimestampIntervalMs(enterTs, nowTs) > 2000)
                        break;

                    keyStr = log;
                    break;
                }
                cmd.StandardInput.WriteLine("\n");
            }

            if (string.IsNullOrEmpty(keyStr))
            {
                errorMsg = "解密失败";
                return false;
            }
            else
            {
                DecryptResult decryptResult;
                try
                {
                    decryptResult = JsonConvert.DeserializeObject<DecryptResult>(keyStr);
                }
                catch (Exception)
                {
                    decryptResult = null;
                }

                if (decryptResult is null)
                {
                    errorMsg = "解密失败";
                    return false;
                }
                else
                {
                    if (!decryptResult.IsDecryptOk)
                    {
                        errorMsg = "解密失败: " + decryptResult.ErrorMsg;
                        return false;
                    }
                    else
                    {
                        var filePath = decryptResult.DecryptFilePath;
                        var resultFileInfo = new FileInfo(filePath);
                        if (!resultFileInfo.Exists || resultFileInfo.Extension.ToLower() != ".bin")
                        {
                            errorMsg = "解密失败：解密后的文件非.bin文件";
                            return false;
                        }
                        else
                        {
                            var record = BinToS19Converter.ConvertBinToS19Lines(filePath).Where(f => f.Type == CommonUtility.FileOperator.SRecordFileHelper.SRecordType.S3).ToList();
                            //if (record.Any(f => (f.Data is null || f.Data.Length != 32 || f.DataLen != 32)))
                            //{
                            //    UIMessageBox.ShowError("解密后转换ECU ID失败：出现不规则的block数据");
                            //}
                            //else
                            {
                                var blocks = SRecordFileHelper.GetBlocks(record);
                                var listDatas = new List<byte>();

                                foreach (var t in blocks.SelectMany(b => b))
                                    listDatas.AddRange(t.Data);

                                if (listDatas.Count % 144 != 0)
                                {
                                    errorMsg = "解密后转换ECU ID失败：数据不完整";
                                    return false;
                                }
                                else
                                {
                                    using (var db = new SqlSugarClient(
                                        new ConnectionConfig
                                        {
                                            ConnectionString = string.Format(@"server={0};database={1};uid={2};pwd={3}", ServerIp, ServerDataBase, ServerUid, ServerPwd),
                                            DbType = DbType.SqlServer,
                                            IsAutoCloseConnection = true,
                                            InitKeyType = InitKeyType.SystemTable
                                        }))
                                    {

                                        var ecuDatas = new List<ECUData>();

                                        for (var i = 0; i < listDatas.Count; i = i + 144)
                                        {
                                            var ecuIdStruct = new ECUData
                                            {
                                                EcuIdFromFile = fileInfo.Name,
                                                CreateTime = DateTime.Now,
                                                LastRecordTime = DateTime.Now,
                                                MASTER_ECU_KEY_KeySlot = "FF",
                                                UNLOCK_ECU_KEY_KeySlot = "01",
                                                Barcode = string.Empty,
                                                TrackInfo = string.Empty,
                                                IsUploadToSgm = 0,
                                                IsUsage = 0,
                                                IsConstMasterKeyOk = 0,
                                                IsConstUnlockKeyOk = 0
                                            };

                                            for (var j = 0; j < 144; j++)
                                            {
                                                if (j >= 0 && j < 16)
                                                    ecuIdStruct.EcuId += ValueHelper.GetHextStr(listDatas[i + j]);
                                                else if (j >= 16 && j < 32)
                                                    ecuIdStruct.MASTER_ECU_KEY_M1 += ValueHelper.GetHextStr(listDatas[i + j]);
                                                else if (j >= 32 && j < 64)
                                                    ecuIdStruct.MASTER_ECU_KEY_M2 += ValueHelper.GetHextStr(listDatas[i + j]);
                                                else if (j >= 64 && j < 80)
                                                    ecuIdStruct.MASTER_ECU_KEY_M3 += ValueHelper.GetHextStr(listDatas[i + j]);
                                                else if (j >= 80 && j < 96)
                                                    ecuIdStruct.UNLOCK_ECU_KEY_M1 += ValueHelper.GetHextStr(listDatas[i + j]);
                                                else if (j >= 96 && j < 128)
                                                    ecuIdStruct.UNLOCK_ECU_KEY_M2 += ValueHelper.GetHextStr(listDatas[i + j]);
                                                else if (j >= 128 && j < 144)
                                                    ecuIdStruct.UNLOCK_ECU_KEY_M3 += ValueHelper.GetHextStr(listDatas[i + j]);
                                            }

                                            if (!ecuIdStruct.EcuId.StartsWith("000"))
                                            {
                                                errorMsg = "解密后转换ECU ID失败：出现异常的ECU ID：" + ecuIdStruct.EcuId;
                                                return false;
                                            }

                                            if (!ecuIdStruct.MASTER_ECU_KEY_M1.StartsWith("00000000000000000000000000000011"))
                                            {
                                                errorMsg = string.Format("解密后转换MASTER_ECU_KEY_M1失败：ECU_ID={0}, 解密后转换MASTER_ECU_KEY_M1失败={1}", ecuIdStruct.EcuId, ecuIdStruct.MASTER_ECU_KEY_M1);
                                                return false;
                                            }
                                            if (!ecuIdStruct.MASTER_ECU_KEY_M2.StartsWith("A4BA2FEECF1"))
                                            {
                                                errorMsg = string.Format("解密后转换MASTER_ECU_KEY_M2失败：ECU_ID={0}, 解密后转换MASTER_ECU_KEY_M2失败={1}", ecuIdStruct.EcuId, ecuIdStruct.MASTER_ECU_KEY_M2);
                                                return false;
                                            }
                                            if (!ecuIdStruct.UNLOCK_ECU_KEY_M1.StartsWith("00000000000000000000000000000044"))
                                            {
                                                errorMsg = string.Format("解密后转换UNLOCK_ECU_KEY_M1失败：ECU_ID={0}, 解密后转换UNLOCK_ECU_KEY_M1失败={1}", ecuIdStruct.EcuId, ecuIdStruct.UNLOCK_ECU_KEY_M1);
                                                return false;
                                            }
                                            if (!ecuIdStruct.UNLOCK_ECU_KEY_M2.StartsWith("1BAFA3B95A64DF9B40B8FD17A47433E1"))
                                            {
                                                errorMsg = string.Format("解密后转换UNLOCK_ECU_KEY_M2失败：ECU_ID={0}, 解密后转换UNLOCK_ECU_KEY_M2失败={1}", ecuIdStruct.EcuId, ecuIdStruct.UNLOCK_ECU_KEY_M2);
                                                return false;
                                            }

                                            ecuDatas.Add(ecuIdStruct);
                                        }

                                        if (toVerifuCount != ecuDatas.Count)
                                        {
                                            errorMsg = string.Format("待解密数量={0}, 本次解密数量={1}，数量不符，无法上传至服务器", toVerifuCount, ecuDatas.Count);
                                            return false;
                                        }
                                        else
                                        {
                                            //var getall = db.Queryable<ECUData>().Select(it => new { CsName = it.EcuId }).ToList();
                                            foreach (var item in ecuDatas)
                                            {
                                                var isRepeat = db.Queryable<ECUData>().Where(f => f.EcuId.ToLower() == item.EcuId.ToLower()).ToList().Count > 0;
                                                if (isRepeat)
                                                {
                                                    errorMsg = string.Format("解密后转换的ECU ID: {0}, 在数据库中出现重复的，当前文件标记异常，所有ECU ID都无法导入", item.EcuId);
                                                    return false;
                                                }
                                            }

                                            var insertable = db.Insertable(ecuDatas);
                                            insertable.IgnoreColumns("id"); // 忽略自增长列
                                            var result = insertable.ExecuteReturnIdentity(); // 执行插入并返回插入数据的ID

                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private class DecryptResult
        {
            public bool IsDecryptOk { get; set; } = false;
            public string DecryptFilePath { get; set; } = string.Empty;
            public string ErrorMsg { get; set; } = string.Empty;
        }

        [SugarTable("manufactureSgmWirelessChargingDatas")]
        public class ECUData
        {
            [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "id")]
            public int id { get; set; }

            [SugarColumn(ColumnName = "EcuId")]
            public string EcuId { get; set; } = string.Empty;

            [SugarColumn(ColumnName = "EcuIdFromFile")]
            public string EcuIdFromFile { get; set; } = string.Empty;

            [SugarColumn(ColumnName = "Barcode")]
            public string Barcode { get; set; } = string.Empty;

            [SugarColumn(ColumnName = "TrackInfo")]
            public string TrackInfo { get; set; } = string.Empty;

            [SugarColumn(ColumnName = "CreateTime")]
            public DateTime CreateTime { get; set; }

            [SugarColumn(ColumnName = "LastRecordTime")]
            public DateTime LastRecordTime { get; set; }

            /// <summary>
            /// 0=未使用；1=已使用，待上传；2=正在使用
            /// </summary>
            [SugarColumn(ColumnName = "IsUsage", ColumnDescription = " 0=未使用；1=已使用，待上传；2=正在使用")]
            public int IsUsage { get; set; } = 0;

            [SugarColumn(ColumnName = "IsUploadToSgm")]
            public int IsUploadToSgm { get; set; } = 0;

            [SugarColumn(ColumnName = "IsConstMasterKeyOk")]
            public int IsConstMasterKeyOk { get; set; } = 0;

            [SugarColumn(ColumnName = "IsConstUnlockKeyOk")]
            public int IsConstUnlockKeyOk { get; set; } = 0;

            [SugarColumn(ColumnName = "MASTER_ECU_KEY_KeySlot")]
            public string MASTER_ECU_KEY_KeySlot { get; set; } = string.Empty;

            [SugarColumn(ColumnName = "MASTER_ECU_KEY_M1")]
            public string MASTER_ECU_KEY_M1 { get; set; } = string.Empty;

            [SugarColumn(ColumnName = "MASTER_ECU_KEY_M2")]
            public string MASTER_ECU_KEY_M2 { get; set; } = string.Empty;

            [SugarColumn(ColumnName = "MASTER_ECU_KEY_M3")]
            public string MASTER_ECU_KEY_M3 { get; set; } = string.Empty;

            [SugarColumn(ColumnName = "MASTER_ECU_KEY_M4")]
            public string MASTER_ECU_KEY_M4 { get; set; } = string.Empty;

            [SugarColumn(ColumnName = "MASTER_ECU_KEY_M5")]
            public string MASTER_ECU_KEY_M5 { get; set; } = string.Empty;

            [SugarColumn(ColumnName = "UNLOCK_ECU_KEY_KeySlot")]
            public string UNLOCK_ECU_KEY_KeySlot { get; set; } = string.Empty;

            [SugarColumn(ColumnName = "UNLOCK_ECU_KEY_M1")]
            public string UNLOCK_ECU_KEY_M1 { get; set; } = string.Empty;

            [SugarColumn(ColumnName = "UNLOCK_ECU_KEY_M2")]
            public string UNLOCK_ECU_KEY_M2 { get; set; } = string.Empty;

            [SugarColumn(ColumnName = "UNLOCK_ECU_KEY_M3")]
            public string UNLOCK_ECU_KEY_M3 { get; set; } = string.Empty;

            [SugarColumn(ColumnName = "UNLOCK_ECU_KEY_M4")]
            public string UNLOCK_ECU_KEY_M4 { get; set; } = string.Empty;

            [SugarColumn(ColumnName = "UNLOCK_ECU_KEY_M5")]
            public string UNLOCK_ECU_KEY_M5 { get; set; } = string.Empty;
        }

        public bool PrintToXml(string printXmlFilePath, List<ECUData> toPriintData, out int count)
        {
            try
            {
                var wirelessKey = new ecuRecordList { ecuRecord = new ecuRecordListEcuRecord[toPriintData.Count] };

                for (var i = 0; i < toPriintData.Count; i++)
                {
                    wirelessKey.ecuRecord[i] = new ecuRecordListEcuRecord
                    {
                        ecuid = Convert.ToBase64String(GetByteArray(toPriintData[i].EcuId, false)),
                        serialNo =
                            ValueHelper.GetHextStr(Encoding.ASCII.GetBytes(toPriintData[i].TrackInfo)).Replace(" ", ""),
                        mkm4 = Convert.ToBase64String(GetByteArray(toPriintData[i].MASTER_ECU_KEY_M4, false)),
                        mkm5 = Convert.ToBase64String(GetByteArray(toPriintData[i].MASTER_ECU_KEY_M5, false)),
                        ukm4 = Convert.ToBase64String(GetByteArray(toPriintData[i].UNLOCK_ECU_KEY_M4, false)),
                        ukm5 = Convert.ToBase64String(GetByteArray(toPriintData[i].UNLOCK_ECU_KEY_M5, false))
                    };
                }

                XmlHelper.SerializeToFile(wirelessKey, printXmlFilePath, Encoding.UTF8);
                count = wirelessKey.ecuRecord.Length;
                return true;
            }
            catch (Exception)
            {
                count = 0;
                return false;
            }
        }

        private static byte[] GetByteArray(string hexArray, bool withSpace = true)
        {
            string[] array;
            if (withSpace)
                array = hexArray.Split(' ');
            else
            {
                var temp = new List<string>();
                for (var i = 0; i < hexArray.Length; i += 2)
                {
                    temp.Add(new string(hexArray.Skip(i).Take(2).ToArray()));
                }
                array = temp.ToArray();
            }
            return array.Select(hex => Convert.ToByte(hex, 16)).ToArray();
        }

        #endregion

        #region debug用

        private List<KeyLogData> _injectKeyBuffer = new List<KeyLogData>();
        private bool _isLogInjectKeyBuffer;

        private class KeyLogData
        {
            /// <summary>
            /// 消息唯一标识
            /// </summary>
            public string MessageKey { get; set; }

            public string Direction { get; set; }

            /// <summary>
            /// CAN消息的时间
            /// </summary>
            public DateTime DateTime { get; set; }

            /// <summary>
            /// CAN ID
            /// </summary>
            public string CanId { get; set; }

            /// <summary>
            /// CAN协议类型
            /// </summary>
            public CanBus.CanProtocol CanProtocol { get; set; }

            /// <summary>
            /// 帧类型
            /// </summary>
            public CanBus.CanType CanType { get; set; }

            /// <summary>
            /// 帧格式
            /// </summary>
            public CanBus.CanFormat CanFormat { get; set; }

            /// <summary>
            /// CAN消息长度
            /// </summary>
            public int CanDataLen { get; set; }

            /// <summary>
            /// CAN消息内容
            /// </summary>
            public string CanData { get; set; }

            public string Note { get; set; }
        }

        [Description("debug注入默认值key")]
        public void DebugKey()
        {
            if (CanFD is null)
            {
                return;
            }

            var folder = string.Format(@"{0}/LocalDB", Path.GetPathRoot(SysDir));
            if (Directory.Exists(folder))
            {
                _logCsv = string.Format("{0}/ECUID{1}_MTC{2}_{3}.csv", folder, "debug", "debug", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                _logConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ",",
                    HasHeaderRecord = !File.Exists(_logCsv), // 仅首次写入时添加表头
                    BufferSize = 65536,
                    Mode = CsvMode.RFC4180 // 严格兼容RFC4180标准
                };
            }

            PowerModeOff();
            StartCan();
            Thread.Sleep(3000);
            StopCan();
            EnterExtendMode();
            SecurityAccess0D0E();
            DisableRxAndTxCommunication();

            var masterKeysM1 = new List<byte>();
            var masterKeysM2 = new List<byte>();
            var masterKeysM3 = new List<byte>();
            for (var i = 0; i < _constMasterM1.Length; i = i + 2)
                masterKeysM1.Add(Convert.ToByte(_constMasterM1.Substring(i, 2), 16));
            for (var i = 0; i < _constMasterM2.Length; i = i + 2)
                masterKeysM2.Add(Convert.ToByte(_constMasterM2.Substring(i, 2), 16));
            for (var i = 0; i < _constMasterM3.Length; i = i + 2)
                masterKeysM3.Add(Convert.ToByte(_constMasterM3.Substring(i, 2), 16));

            var unlockKeysM1 = new List<byte>();
            var unlockKeysM2 = new List<byte>();
            var unlockKeysM3 = new List<byte>();
            for (var i = 0; i < _constUnlockM1.Length; i = i + 2)
                unlockKeysM1.Add(Convert.ToByte(_constUnlockM1.Substring(i, 2), 16));
            for (var i = 0; i < _constUnlockM2.Length; i = i + 2)
                unlockKeysM2.Add(Convert.ToByte(_constUnlockM2.Substring(i, 2), 16));
            for (var i = 0; i < _constUnlockM3.Length; i = i + 2)
                unlockKeysM3.Add(Convert.ToByte(_constUnlockM3.Substring(i, 2), 16));

            var isCheckMasterKeyOk = false;
            var isCheckUnlockKeyOk = false;

            var errorInfo = string.Empty;

            {
                StartKeyLog("MasterKey固定值");
                byte masterKeyWriteHistory;
                byte[] masterKeyM4;
                byte[] masterKeyM5;
                if (UnlockSeedKey(_constMasterKeySlot, masterKeysM1.ToArray(), masterKeysM2.ToArray(), masterKeysM3.ToArray(), out masterKeyWriteHistory, out masterKeyM4, out masterKeyM5))
                {
                    StopKeyLog();
                    var toCheckBytes = new byte[17];
                    Array.Copy(masterKeyM4, 15, toCheckBytes, 0, 17);
                    var convertStr = ValueHelper.GetHextStr(toCheckBytes).Replace(" ", "");
                    isCheckMasterKeyOk = masterKeyWriteHistory == 0x00 && string.Equals(convertStr, _correctMasterM4, StringComparison.CurrentCultureIgnoreCase);
                    var _trackEcuDataIsConstMasterKeyOk = isCheckMasterKeyOk ? 1 : 0;

                    if (_trackEcuDataIsConstMasterKeyOk != 1)
                    {
                        errorInfo += string.Format(" masterKey status: {0} ", ValueHelper.GetHextStrWithOx(masterKeyWriteHistory));
                        SaveKeyLog();
                    }
                }
                else
                {
                    errorInfo += " masterKey uds failed ";
                    StopKeyLog(25);
                    SaveKeyLog();
                }
            }

            //if (isCheckMasterKeyOk)
            {
                {
                    StartKeyLog("UnlockKey固定值");
                    byte unlockKeyWriteHistory;
                    byte[] unlockKeyM4;
                    byte[] unlockKeyM5;
                    if (UnlockSeedKey(_constUnlockKeySlot, unlockKeysM1.ToArray(), unlockKeysM2.ToArray(), unlockKeysM3.ToArray(), out unlockKeyWriteHistory, out unlockKeyM4, out unlockKeyM5))
                    {
                        StopKeyLog();
                        var toCheckBytes = new byte[17];
                        Array.Copy(unlockKeyM4, 15, toCheckBytes, 0, 17);
                        var convertStr = ValueHelper.GetHextStr(toCheckBytes).Replace(" ", "");
                        isCheckUnlockKeyOk = unlockKeyWriteHistory == 0x00 && string.Equals(convertStr, _correctUnlockM4, StringComparison.CurrentCultureIgnoreCase);
                        var _trackEcuDataIsConstUnlockKeyOk = isCheckUnlockKeyOk ? 1 : 0;

                        if (_trackEcuDataIsConstUnlockKeyOk != 1)
                        {
                            errorInfo += string.Format(" unlockKey status: {0} ", ValueHelper.GetHextStrWithOx(unlockKeyWriteHistory));
                            SaveKeyLog();
                        }
                    }
                    else
                    {
                        errorInfo += " unlockKey uds failed ";
                        StopKeyLog(25);
                        SaveKeyLog();
                    }
                }
            }

            InjectConstKeyResult = isCheckMasterKeyOk && isCheckUnlockKeyOk ? "OK" : "NG" + errorInfo;
        }

        [Description("debug注入iecskey")]
        public void DebugIECSKey()
        {
            if (CanFD is null)
                return;

            return;

            {
                var folder = string.Format(@"{0}/LocalDB", Path.GetPathRoot(SysDir));
                if (Directory.Exists(folder))
                {
                    _logCsv = string.Format("{0}/ECUID{1}_MTC{2}_{3}.csv", folder, "debug", "debug", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                    _logConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        Delimiter = ",",
                        HasHeaderRecord = !File.Exists(_logCsv), // 仅首次写入时添加表头
                        BufferSize = 65536,
                        Mode = CsvMode.RFC4180 // 严格兼容RFC4180标准
                    };
                }

                PowerModeOff();
                StartCan();
                Thread.Sleep(3000);
                StopCan();
                EnterExtendMode();
                SecurityAccess0D0E();
                DisableRxAndTxCommunication();
            }

            {
                MasterKey = string.Empty;
                UnlockKey = string.Empty;

                var masterKeysM1 = new List<byte>();
                var masterKeysM2 = new List<byte>();
                var masterKeysM3 = new List<byte>();
                var masterKeySlot = (byte)0x00;

                masterKeySlot = Convert.ToByte("FF", 16);

                for (var i = 0; i < "00000000000000000000000000000011".Length; i = i + 2)
                    masterKeysM1.Add(Convert.ToByte("00000000000000000000000000000011".Substring(i, 2), 16));
                for (var i = 0; i < "A4BA2FEECF1A77BB8297C389E4DDCD6B9ED8CC805B70AECD9A3B5216B5511927".Length; i = i + 2)
                    masterKeysM2.Add(Convert.ToByte("A4BA2FEECF1A77BB8297C389E4DDCD6B9ED8CC805B70AECD9A3B5216B5511927".Substring(i, 2), 16));
                for (var i = 0; i < "8BAA3E6FC184EFE6C6A9B7D6FD81104A".Length; i = i + 2)
                    masterKeysM3.Add(Convert.ToByte("8BAA3E6FC184EFE6C6A9B7D6FD81104A".Substring(i, 2), 16));

                var unlockKeysM1 = new List<byte>();
                var unlockKeysM2 = new List<byte>();
                var unlockKeysM3 = new List<byte>();
                var unlockKeySlot = (byte)0x00;

                unlockKeySlot = Convert.ToByte("01", 16);

                for (var i = 0; i < "00000000000000000000000000000044".Length; i = i + 2)
                    unlockKeysM1.Add(Convert.ToByte("00000000000000000000000000000044".Substring(i, 2), 16));
                for (var i = 0; i < "1BAFA3B95A64DF9B40B8FD17A47433E13DC263E28F8FC7140B05C4B97AED3DD2".Length; i = i + 2)
                    unlockKeysM2.Add(Convert.ToByte("1BAFA3B95A64DF9B40B8FD17A47433E13DC263E28F8FC7140B05C4B97AED3DD2".Substring(i, 2), 16));
                for (var i = 0; i < "4794E33EBF6055699164A70042B4973A".Length; i = i + 2)
                    unlockKeysM3.Add(Convert.ToByte("4794E33EBF6055699164A70042B4973A".Substring(i, 2), 16));

                var isCheckMasterKeyOk = false;
                var isCheckUnlockKeyOk = false;

                var mErrorInfo = string.Empty;
                var uErrorInfo = string.Empty;

                if (true)
                {
                    StartKeyLog("MasterKey_IECS值");
                    byte masterKeyWriteHistory;
                    byte[] masterKeyM4;
                    byte[] masterKeyM5;
                    if (UnlockSeedKey(masterKeySlot, masterKeysM1.ToArray(), masterKeysM2.ToArray(), masterKeysM3.ToArray(), out masterKeyWriteHistory, out masterKeyM4, out masterKeyM5))
                    {
                        StopKeyLog();
                        var toCheckBytes = new byte[17];
                        Array.Copy(masterKeyM4, 15, toCheckBytes, 0, 17);
                        var convertStr = ValueHelper.GetHextStr(toCheckBytes).Replace(" ", "");
                        isCheckMasterKeyOk = masterKeyWriteHistory == 0x00 && masterKeyM4 != null && masterKeyM4.Length == 32 && masterKeyM5 != null && masterKeyM5.Length == 16;
                        if (isCheckMasterKeyOk)
                        {

                        }
                        else
                        {
                            mErrorInfo += string.Format(" masterKey status: {0} ", ValueHelper.GetHextStrWithOx(masterKeyWriteHistory));
                            SaveKeyLog();
                        }
                    }
                    else
                    {
                        mErrorInfo += " masterKey uds failed ";
                        StopKeyLog(25);
                        SaveKeyLog();
                    }
                }

                //if (isCheckMasterKeyOk)
                {
                    if (true)
                    {
                        StartKeyLog("UnlockKey_IECS值");
                        byte unlockKeyWriteHistory;
                        byte[] unlockKeyM4;
                        byte[] unlockKeyM5;
                        if (UnlockSeedKey(unlockKeySlot, unlockKeysM1.ToArray(), unlockKeysM2.ToArray(), unlockKeysM3.ToArray(), out unlockKeyWriteHistory, out unlockKeyM4, out unlockKeyM5))
                        {
                            StopKeyLog();
                            var toCheckBytes = new byte[17];
                            Array.Copy(unlockKeyM4, 15, toCheckBytes, 0, 17);
                            var convertStr = ValueHelper.GetHextStr(toCheckBytes).Replace(" ", "");
                            isCheckUnlockKeyOk = unlockKeyWriteHistory == 0x00 && unlockKeyM4 != null && unlockKeyM4.Length == 32 && unlockKeyM5 != null && unlockKeyM5.Length == 16;
                            if (isCheckUnlockKeyOk)
                            {

                            }
                            else
                            {
                                uErrorInfo += string.Format(" unlockKey status: {0} ", ValueHelper.GetHextStrWithOx(unlockKeyWriteHistory));
                                SaveKeyLog();
                            }
                        }
                        else
                        {
                            uErrorInfo += " unlockKey uds failed ";
                            StopKeyLog(25);
                            SaveKeyLog();
                        }
                    }
                }

                MasterKey = isCheckMasterKeyOk ? "OK" : "NG" + mErrorInfo;
                UnlockKey = isCheckUnlockKeyOk ? "OK" : "NG" + uErrorInfo;
            }
        }

        #endregion

        #region 自动线

        public bool IsCheckExistOk;

        //[Description("AutoOneKeyTest")]
        //public void AutoOneKeyTest() 
        //{
        //    VersionRead(47, 16, 26955982, "AB", "0101", 254);
        //}

        public void VersionRead(int mtcIndex, int mtcLen, int bootPartNo, string bootVer, string ddiHex, int mecValue)
        {
            PowerModeOff();
            Thread.Sleep(1000);
            LoadTrack(mtcIndex, mtcLen);
            ReadApp();
            Thread.Sleep(100);
            ReadCal();
            Thread.Sleep(100);
            ReadBaseModule();
            Thread.Sleep(100);
            ReadEndModule();
            Thread.Sleep(100);
            ReadVPPS();
            Thread.Sleep(100);
            ReadDUNS();
            Thread.Sleep(100);
            EnterExtendMode();
            Thread.Sleep(100);
            SecurityAccess0102();
            Thread.Sleep(100);
            WriteBoot(bootPartNo, bootVer);
            Thread.Sleep(100);
            WriteAndReadDDI(ddiHex);
            Thread.Sleep(100);
            WriteManufacturersEnableCounter(mecValue);
            Thread.Sleep(100);
            WriteAndReadMtc();
            Thread.Sleep(100);
            WriteAndReadECUID();
            Thread.Sleep(100);
            WriteAndReadProgrammingDate();
            Thread.Sleep(100);
            ReadManufacturersEnableCounter();
            Thread.Sleep(100);
            ReadBoot();
            Thread.Sleep(100);
            SecurityAccess0D0E();
            Thread.Sleep(100);
            DisableRxAndTxCommunication();
            Thread.Sleep(100);
            InjectConstKey();
            Thread.Sleep(100);
            InjectIECSKey();
            UploadTrackToServer();
            EnterNormalMode();
            ClearTrack();
            PowerModeOn();
        }

        #endregion

        #region +读取MTC并与二维码比对和追溯，用于GP12

        public string GP12TrackBarcode = string.Empty;
        public string GP12TrackResult = string.Empty;
        private static readonly object _lockTrackDb = new object();

        public void GP12Track(int mtcIndex, int mtcLen)
        {
            var toTrackCode = GP12TrackBarcode;
            GP12TrackBarcode = string.Empty;
            GP12TrackResult = string.Empty;

            var mtc = string.Empty;
            var ecuId = string.Empty;
            try
            {
                mtc = toTrackCode.Substring(mtcIndex, mtcLen);
            }
            catch (Exception e)
            {
                GP12TrackResult = "NG 二维码解析失败";
                return;
            }

            if (string.IsNullOrEmpty(mtc))
            {
                GP12TrackResult = "NG 二维码解析失败";
                return;
            }

            lock (_lockTrackDb)
            {
                try
                {
                    if (_serverDb is null)
                    {
                        _serverDb = new SqlSugarClient(
                            new ConnectionConfig
                            {
                                ConnectionString = string.Format(@"server={0};database={1};uid={2};pwd={3}", ServerIp, ServerDataBase, ServerUid, ServerPwd),
                                DbType = DbType.SqlServer,
                                IsAutoCloseConnection = true,
                                InitKeyType = InitKeyType.SystemTable
                            });
                    }

                    var trackData = _serverDb.Queryable<ECUData>().Where(it => it.IsUsage == 1 && it.Barcode == toTrackCode && it.TrackInfo == mtc).ToList();

                    if (trackData.Count != 1)
                    {
                        GP12TrackResult = "NG 查询数据库失败";
                        return;
                    }

                    ecuId = trackData[0].EcuId;

                    if (string.IsNullOrEmpty(ecuId))
                    {
                        GP12TrackResult = "NG 查询ECUID数据失败";
                        return;
                    }
                }
                catch (Exception e)
                {
                    GP12TrackResult = "NG 查询数据库失败";
                    return;
                }
            }

            DebugReadEUCID();
            DebugReadMtc();
            var isEcuIdOk = ECUID == ecuId;
            var isMtcOk = mtc == MTC;
            if (isMtcOk && isEcuIdOk)
            {
                GP12TrackResult = "OK";
                ECUID = "OK " + ECUID;
                MTC = "OK " + MTC;
            }
            else
            {
                GP12TrackResult = "NG";
                if (isMtcOk)
                {
                    MTC = "OK " + MTC;
                }
                else
                {
                    MTC = "NG " + string.Format("查询记录：{0}，实际读取：{1}", mtc, MTC);
                    GP12TrackResult += " MTC校验错误";
                }

                if (isEcuIdOk)
                {
                    ECUID = "OK " + ECUID;
                }
                else
                {
                    ECUID = "NG " + string.Format("查询记录：{0}，实际读取：{1}", ecuId, ECUID);
                    GP12TrackResult += " ECUID校验错误";
                }
            }
        }

        #endregion
    }
}
