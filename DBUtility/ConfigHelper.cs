using System;
using System.Configuration;

namespace DBUtility
{
    /// <summary>
    /// web.config操作类
    /// Copyright (C) Maticsoft
    /// </summary>
    public sealed class ConfigHelper
    {
        /// <summary>
        /// 得到AppSettings中的配置字符串信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfigString(string key)
        {
            var cacheKey = "AppSettings-" + key;
            var objModel = DataCache.GetCache(cacheKey);
            if (objModel != null) return objModel.ToString();
            try
            {
                objModel = ConfigurationManager.AppSettings[key];
                if (objModel != null)
                {
                    DataCache.SetCache(cacheKey, objModel, DateTime.Now.AddMinutes(180), TimeSpan.Zero);
                }
            }
            catch
            {
                // ignored
            }
            return objModel.ToString();
        }

        /// <summary>
        /// 得到AppSettings中的配置Bool信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetConfigBool(string key)
        {
            var result = false;
            var cfgVal = GetConfigString(key);
            if (string.IsNullOrEmpty(cfgVal)) 
                return false;
            try
            {
                result = bool.Parse(cfgVal);
            }
            catch (FormatException)
            {
                // Ignore format exceptions.
            }
            return result;
        }

        /// <summary>
        /// 得到AppSettings中的配置Decimal信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static decimal GetConfigDecimal(string key)
        {
            decimal result = 0;
            var cfgVal = GetConfigString(key);
            if (string.IsNullOrEmpty(cfgVal)) 
                return result;
            try
            {
                result = decimal.Parse(cfgVal);
            }
            catch (FormatException)
            {
                // Ignore format exceptions.
            }

            return result;
        }

        /// <summary>
        /// 得到AppSettings中的配置int信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetConfigInt(string key)
        {
            var result = 0;
            var cfgVal = GetConfigString(key);
            if (string.IsNullOrEmpty(cfgVal)) 
                return result;
            try
            {
                result = int.Parse(cfgVal);
            }
            catch (FormatException)
            {
                // Ignore format exceptions.
            }

            return result;
        }
    }
}
