using System;
using System.Data.SqlClient;

namespace CheckSystem.SmtForms.DataUploader
{
    public class KouYoungAoi_TB_AOIResult
    {
        public string PCBGUID { get; set; }
        public string PCBResultRepair { get; set; }
        public string PCBModel { get; set; }
        public string BarCode { get; set; }
        public string ReviewStartDateTime { get; set; }
        public string ReviewEndDateTime { get; set; }

        public static bool CheckDatabaseExists(SqlConnection tmpConn, string databaseName)
        {
            bool result;

            try
            {
                tmpConn = new SqlConnection(tmpConn.ConnectionString);

                var sqlCreateDbQuery = string.Format("SELECT database_id FROM sys.databases WHERE Name= '{0}'", databaseName);

                using (tmpConn)
                {
                    using (var sqlCmd = new SqlCommand(sqlCreateDbQuery, tmpConn))
                    {
                        tmpConn.Open();

                        var resultObj = sqlCmd.ExecuteScalar();

                        var databaseId = 0;

                        if (resultObj != null)
                        {
                            int.TryParse(resultObj.ToString(), out databaseId);
                        }

                        tmpConn.Close();

                        result = databaseId > 0;
                    }
                }
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }
    }
}
