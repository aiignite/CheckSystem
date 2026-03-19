// ***********************************************************************
// Assembly         : HZH_Controls
// Created          : 08-08-2019
//
// ***********************************************************************
// <copyright file="Ext.cs">
//     Copyright by Huang Zhenghui(黄正辉) All, QQ group:568015492 QQ:623128629 Email:623128629@qq.com
// </copyright>
//
// Blog: https://www.cnblogs.com/bfyx
// GitHub：https://github.com/kwwwvagaa/NetWinformControl
// gitee：https://gitee.com/kwwwvagaa/net_winform_custom_control.git
//
// If you use this code, please keep this note.
// ***********************************************************************

using System;
using System.Linq;
using System.Text;

namespace HZH_Controls.Helpers
{
    /// <summary>
    /// Class Ext.
    /// </summary>
    public static partial class Ext
    {
        /// <summary>
        /// Clones the model.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="classObject">The class object.</param>
        /// <returns>T.</returns>
        public static T CloneModel<T>(this T classObject) where T : class
        {
            T result;
            if (classObject == null)
                result = default(T);
            else
            {
                var obj = Activator.CreateInstance(typeof(T));
                var properties = typeof(T).GetProperties();
                var array = properties;
                foreach (var propertyInfo in array.Where(propertyInfo => propertyInfo.CanWrite))
                    propertyInfo.SetValue(obj, propertyInfo.GetValue(classObject, null), null);
                result = obj as T;
            }
            return result;
        }

        /// <summary>
        /// ASCII编码的数组转换为英文字符串
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>结果</returns>
        public static string ToEnString(this byte[] s)
        {
            return ToEncodeString(s, Encoding.ASCII).Trim('\0').Trim();
        }

        /// <summary>
        /// 数组按指定编码转换为字符串
        /// </summary>
        /// <param name="dealBytes">数组</param>
        /// <param name="encode">编码</param>
        /// <returns>结果</returns>
        public static string ToEncodeString(this byte[] dealBytes, Encoding encode)
        {
            return encode.GetString(dealBytes);
        }

        #region 转换为base64字符串
        /// <summary>
        /// 功能描述:转换为base64字符串
        /// 作　　者:HZH
        /// 创建日期:2019-03-29 10:12:38
        /// 任务编号:POS
        /// </summary>
        /// <param name="data">data</param>
        /// <returns>返回值</returns>
        public static string ToBase64Str(this string data)
        {
            if (data.IsEmpty())
                return string.Empty;
            var buffer = Encoding.Default.GetBytes(data);
            return Convert.ToBase64String(buffer);
        }

        #endregion
        /// <summary>
        /// 转换为坐标
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>System.Drawing.Point.</returns>
        public static System.Drawing.Point ToPoint(this string data)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(data, @"^\s*\d+(\.\d+)?\s*\,\s*\d+(\.\d+)?\s*$"))
                return System.Drawing.Point.Empty;

            var strs = data.Split(',');
            return new System.Drawing.Point(strs[0].ToInt(), strs[1].ToInt());
        }

        #region 数值转换
        /// <summary>
        /// 转换为整型
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>System.Int32.</returns>
        public static int ToInt(this object data)
        {
            if (data == null)
                return 0;
            if (data is bool)
                return (bool)data ? 1 : 0;
            int result;
            var success = int.TryParse(data.ToString(), out result);
            if (success)
                return result;
            try
            {
                return Convert.ToInt32(ToDouble(data, 0));
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// 转换为可空整型
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>System.Nullable&lt;System.Int32&gt;.</returns>
        public static int? ToIntOrNull(this object data)
        {
            if (data == null)
                return null;
            int result;
            var isValid = int.TryParse(data.ToString(), out result);
            if (isValid)
                return result;
            return null;
        }

        /// <summary>
        /// 转换为双精度浮点数
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>System.Double.</returns>
        public static double ToDouble(this object data)
        {
            if (data == null)
                return 0;
            double result;
            return double.TryParse(data.ToString(), out result) ? result : 0;
        }

        /// <summary>
        /// 转换为双精度浮点数,并按指定的小数位4舍5入
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="digits">小数位数</param>
        /// <returns>System.Double.</returns>
        public static double ToDouble(this object data, int digits)
        {
            return Math.Round(ToDouble(data), digits, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 转换为可空双精度浮点数
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>System.Nullable&lt;System.Double&gt;.</returns>
        public static double? ToDoubleOrNull(this object data)
        {
            if (data == null)
                return null;
            double result;
            var isValid = double.TryParse(data.ToString(), out result);
            if (isValid)
                return result;
            return null;
        }

        /// <summary>
        /// 转换为高精度浮点数
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>System.Decimal.</returns>
        public static decimal ToDecimal(this object data)
        {
            if (data == null)
                return 0;
            decimal result;
            return decimal.TryParse(data.ToString(), out result) ? result : 0;
        }

        /// <summary>
        /// 转换为高精度浮点数,并按指定的小数位4舍5入
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="digits">小数位数</param>
        /// <returns>System.Decimal.</returns>
        public static decimal ToDecimal(this  object data, int digits)
        {
            return Math.Round(ToDecimal(data), digits, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 转换为可空高精度浮点数
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>System.Nullable&lt;System.Decimal&gt;.</returns>
        public static decimal? ToDecimalOrNull(this  object data)
        {
            if (data == null)
                return null;
            decimal result;
            var isValid = decimal.TryParse(data.ToString(), out result);
            if (isValid)
                return result;
            return null;
        }

        /// <summary>
        /// 转换为可空高精度浮点数,并按指定的小数位4舍5入
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="digits">小数位数</param>
        /// <returns>System.Nullable&lt;System.Decimal&gt;.</returns>
        public static decimal? ToDecimalOrNull(this object data, int digits)
        {
            var result = ToDecimalOrNull(data);
            if (result == null)
                return null;
            return Math.Round(result.Value, digits, MidpointRounding.AwayFromZero);
        }

        #endregion

        #region 日期转换
        /// <summary>
        /// 转换为日期
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>DateTime.</returns>
        public static DateTime ToDate(this object data)
        {
            try
            {
                if (data == null)
                    return DateTime.MinValue;
                if (System.Text.RegularExpressions.Regex.IsMatch(data.ToStringExt(), @"^\d{8}$"))
                {
                    var strValue = data.ToStringExt();
                    return new DateTime(strValue.Substring(0, 4).ToInt(), strValue.Substring(4, 2).ToInt(), strValue.Substring(6, 2).ToInt());
                }
                DateTime result;
                return DateTime.TryParse(data.ToString(), out result) ? result : DateTime.MinValue;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// 转换为可空日期
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>System.Nullable&lt;DateTime&gt;.</returns>
        public static DateTime? ToDateOrNull(this object data)
        {
            try
            {
                if (data == null)
                    return null;
                if (System.Text.RegularExpressions.Regex.IsMatch(data.ToStringExt(), @"^\d{8}$"))
                {
                    var strValue = data.ToStringExt();
                    return new DateTime(strValue.Substring(0, 4).ToInt(), strValue.Substring(4, 2).ToInt(), strValue.Substring(6, 2).ToInt());
                }
                DateTime result;
                var isValid = DateTime.TryParse(data.ToString(), out result);
                if (isValid)
                    return result;
                return null;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region 布尔转换
        /// <summary>
        /// 转换为布尔值
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool ToBool(this object data)
        {
            if (data == null)
                return false;
            var value = GetBool(data);
            if (value != null)
                return value.Value;
            bool result;
            return bool.TryParse(data.ToString(), out result) && result;
        }

        /// <summary>
        /// 获取布尔值
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private static bool? GetBool(this object data)
        {
            switch (data.ToString().Trim().ToLower())
            {
                case "0":
                    return false;
                case "1":
                    return true;
                case "是":
                    return true;
                case "否":
                    return false;
                case "yes":
                    return true;
                case "no":
                    return false;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 转换为可空布尔值
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool? ToBoolOrNull(this object data)
        {
            if (data == null)
                return null;
            var value = GetBool(data);
            if (value != null)
                return value.Value;
            bool result;
            var isValid = bool.TryParse(data.ToString(), out result);
            if (isValid)
                return result;
            return null;
        }

        #endregion

        #region 字符串转换
        /// <summary>
        /// 字符串转换为byte[]
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] ToBytes(this string data)
        {
            return Encoding.GetEncoding("GBK").GetBytes(data);
        }

        /// <summary>
        /// Converts to bytesdefault.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] ToBytesDefault(this string data)
        {
            return Encoding.Default.GetBytes(data);
        }

        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>System.String.</returns>
        public static string ToStringExt(this object data)
        {
            return data == null ? string.Empty : data.ToString();
        }
        #endregion

        /// <summary>
        /// 安全返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">可空值</param>
        /// <returns>T.</returns>
        public static T SafeValue<T>(this T? value) where T : struct
        {
            return value ?? default(T);
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="value">值</param>
        /// <returns><c>true</c> if the specified value is empty; otherwise, <c>false</c>.</returns>
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="value">值</param>
        /// <returns><c>true</c> if the specified value is empty; otherwise, <c>false</c>.</returns>
        public static bool IsEmpty(this Guid? value)
        {
            return value == null || IsEmpty(value.Value);
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="value">值</param>
        /// <returns><c>true</c> if the specified value is empty; otherwise, <c>false</c>.</returns>
        public static bool IsEmpty(this Guid value)
        {
            return value == Guid.Empty;
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="value">值</param>
        /// <returns><c>true</c> if the specified value is empty; otherwise, <c>false</c>.</returns>
        public static bool IsEmpty(this object value)
        {
            return value == null || string.IsNullOrEmpty(value.ToString());
        }

        #region 是否数字
        /// <summary>
        /// 功能描述:是否数字
        /// 作　　者:HZH
        /// 创建日期:2019-03-06 09:03:05
        /// 任务编号:POS
        /// </summary>
        /// <param name="value">value</param>
        /// <returns>返回值</returns>
        public static bool IsNum(this string value)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(value, @"^\d+(\.\d*)?$");
        }
        #endregion
    }
}
