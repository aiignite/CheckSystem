using System.Configuration;

namespace DBUtility
{
    public class PubConstant
    {
        public static string ConnectionStringKey = @"LocalConnectionString";

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        public static string ConnectionString
        {           
            get
            {
                var connectionString = ConfigurationManager.AppSettings[ConnectionStringKey];
                var conStringEncrypt = ConfigurationManager.AppSettings["ConStringEncrypt"];
                if (conStringEncrypt == "true")
                    connectionString = DESEncrypt.Decrypt(connectionString);
               
                return connectionString; 
            }
        }

        /// <summary>
        /// 得到web.config里配置项的数据库连接字符串。
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static string GetConnectionString(string configName)
        {
            var connectionString = ConfigurationManager.AppSettings[configName];
            var conStringEncrypt = ConfigurationManager.AppSettings["ConStringEncrypt"];
            if (conStringEncrypt == "true")
                connectionString = DESEncrypt.Decrypt(connectionString);
            return connectionString;
        }
    }
}
