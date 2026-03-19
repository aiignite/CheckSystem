using System;
using System.Collections.Generic;
using System.Linq;
using DBUtility;

namespace CheckSystem.HelperForms.Sgm458Pdm
{
    public static class LoadBoxSqlHelper
    {
        private static readonly object SqlLocker = new object();
        private const string Dbpath = @"SGM458_PDM_LoadBox\LoadBoxDb.db";
        private static readonly Sqlite Sqlite = new Sqlite(Dbpath);

        public static List<LoadBoxErrorInfoModel> GetModels(string sql)
        {
            lock (SqlLocker)
            {
                //var str = "select * from LoadBoxErrorInfo";
                var getData = Sqlite.GetRows(sql);

                return getData.Select(t => new LoadBoxErrorInfoModel
                {
                    Id = t.ItemArray[0].ToString(),
                    CheckResult = t.ItemArray[1].ToString(),
                    DutIndex = t.ItemArray[2].ToString(),
                    CreateTime = t.ItemArray[3].ToString(),
                    Mode = t.ItemArray[4].ToString(),
                    ReleaseMotorState = t.ItemArray[5].ToString(),
                    CinchMotorState = t.ItemArray[6].ToString(),
                    ActuatorMotorState = t.ItemArray[7].ToString(),
                    CanCommunicationState = t.ItemArray[8].ToString(),
                    Detail = t.ItemArray[9].ToString(),
                }).ToList();
            }
        }

        public static void SaveData(LoadBoxErrorInfoModel model)
        {
            lock (SqlLocker)
            {
                model.Id = Guid.NewGuid().ToString();

                //Sqlite.Query("PRAGMA synchronous = OFF");

                var insert =
                    string.Format(
                        "insert into LoadBoxErrorInfo values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')",
                        model.Id, model.CheckResult, model.DutIndex, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        model.Mode, model.ReleaseMotorState, model.ActuatorMotorState, model.CinchMotorState,
                        model.CanCommunicationState, model.Detail);
                Sqlite.Query(insert);
            }
        }

        public static void ClearAllData()
        {
            lock (SqlLocker)
            {
                var sql = "DELETE FROM LoadBoxErrorInfo";
                Sqlite.Query(sql);
            }
        }

        public class LoadBoxErrorInfoModel
        {
            private string _id;
            private string _checkResult;
            private string _dutIndex;
            private string _createTime;
            private string _mode;
            private string _releaseMotorState;
            private string _cinchMotorState;
            private string _actuatorMotorState;
            private string _canCommunicationState;
            private string _detail;

            public string Id
            {
                set { _id = value; }
                get { return _id; }
            }

            public string CheckResult
            {
                set { _checkResult = value; }
                get { return _checkResult; }
            }

            public string DutIndex
            {
                set { _dutIndex = value; }
                get { return _dutIndex; }
            }

            public string CreateTime
            {
                set { _createTime = value; }
                get { return _createTime; }
            }

            public string Mode
            {
                set { _mode = value; }
                get { return _mode; }
            }

            public string ReleaseMotorState
            {
                set { _releaseMotorState = value; }
                get { return _releaseMotorState; }
            }

            public string CinchMotorState
            {
                set { _cinchMotorState = value; }
                get { return _cinchMotorState; }
            }

            public string ActuatorMotorState
            {
                set { _actuatorMotorState = value; }
                get { return _actuatorMotorState; }
            }

            public string CanCommunicationState
            {
                set { _canCommunicationState = value; }
                get { return _canCommunicationState; }
            }

            public string Detail
            {
                set { _detail = value; }
                get { return _detail; }
            }
        }
    }
}
