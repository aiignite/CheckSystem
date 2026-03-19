using System;
using System.Collections.Generic;
using System.Linq;

namespace StateMachine
{
    public static class StateMachineHelper
    {
        /// <summary>
        /// 整理字符串,移除空格、回车换行、换行
        /// </summary>
        /// <param name="str">待整理的字符串</param>
        /// <returns>整理后的字符串,移除空格、回车换行、换行</returns>
        public static string StrTrim(this string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str))
                {
                    return string.Empty;
                }
                var strTemp = str.Replace(" ", "");  //取出空格
                strTemp = strTemp.Replace("\r\n", "");//取出回车换行
                strTemp = strTemp.Replace("\n", "");//取出换行
                strTemp = strTemp.Replace("\t", "");

                return strTemp;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 根据控制器名，在控制器列表中找到控制器
        /// </summary>
        /// <param name="controllerName">控制器名</param>
        /// <param name="controllersList">控制器列表</param>
        /// <returns>符合的控制器</returns>
        public static object GetControllerByName(
            this string controllerName,
            IEnumerable<object> controllersList)
        {
            return (from c in controllersList
                    let nameFiled = c.GetType().GetField("Name")
                    where nameFiled != null
                    where controllerName.Equals(nameFiled.GetValue(c).ToString())
                    select c).FirstOrDefault();
        }

        public static string GetStrFromLeftSingleEqualitySign(this string value)
        {
            const string singleEqualitySign = "=";
            const string singleAddEqualitySign = "+=";
            const string singleSubtractEqualitySign = "-=";
            const string singleMultiplyEqualitySign = "*=";
            const string singleDivisionEqualitySign = "/=";

            if (value.Contains(singleEqualitySign) &&
                !value.Contains(singleAddEqualitySign) &&
                !value.Contains(singleSubtractEqualitySign) &&
                !value.Contains(singleMultiplyEqualitySign) &&
                !value.Contains(singleDivisionEqualitySign))
            {

                var result =
                    value.Substring(0, value.IndexOf(singleEqualitySign, StringComparison.Ordinal));

                return result;
            }

            if (value.Contains(singleAddEqualitySign))
            {
                var result =
                    value.Substring(0, value.IndexOf(singleAddEqualitySign, StringComparison.Ordinal));

                return result;
            }

            if (value.Contains(singleSubtractEqualitySign))
            {
                var result =
                    value.Substring(0, value.IndexOf(singleSubtractEqualitySign, StringComparison.Ordinal));

                return result;
            }

            if (value.Contains(singleMultiplyEqualitySign))
            {
                var result =
                    value.Substring(0, value.IndexOf(singleMultiplyEqualitySign, StringComparison.Ordinal));

                return result;
            }

            if (value.Contains(singleDivisionEqualitySign))
            {
                var result =
                    value.Substring(0, value.IndexOf(singleDivisionEqualitySign, StringComparison.Ordinal));

                return result;
            }

            return string.Empty;
        }

        public static string GetStrFromRightSingleEqualitySign(this string value)
        {
            const string singleEqualitySign = "=";
            const string singleAddEqualitySign = "+=";
            const string singleSubtractEqualitySign = "-=";
            const string singleMultiplyEqualitySign = "*=";
            const string singleDivisionEqualitySign = "/=";

            if (value.Contains(singleEqualitySign) &&
                !value.Contains(singleAddEqualitySign) &&
                !value.Contains(singleSubtractEqualitySign) &&
                !value.Contains(singleMultiplyEqualitySign) &&
                !value.Contains(singleDivisionEqualitySign))
            {
                var result = value.Substring(value.IndexOf(singleEqualitySign, StringComparison.Ordinal) + 1,
                  value.Length - value.IndexOf(singleEqualitySign, StringComparison.Ordinal) - 1);

                return result.Trim(';');
            }

            if (value.Contains(singleAddEqualitySign))
            {
                var result = value.Substring(value.IndexOf(singleAddEqualitySign, StringComparison.Ordinal) + 2,
                  value.Length - value.IndexOf(singleAddEqualitySign, StringComparison.Ordinal) - 2);

                return result.Trim(';');
            }

            if (value.Contains(singleSubtractEqualitySign))
            {
                var result = value.Substring(value.IndexOf(singleSubtractEqualitySign, StringComparison.Ordinal) + 2,
                  value.Length - value.IndexOf(singleSubtractEqualitySign, StringComparison.Ordinal) - 2);

                return result.Trim(';');
            }

            if (value.Contains(singleMultiplyEqualitySign))
            {
                var result = value.Substring(value.IndexOf(singleMultiplyEqualitySign, StringComparison.Ordinal) + 2,
                  value.Length - value.IndexOf(singleMultiplyEqualitySign, StringComparison.Ordinal) - 2);

                return result.Trim(';');
            }

            if (value.Contains(singleDivisionEqualitySign))
            {
                var result = value.Substring(value.IndexOf(singleDivisionEqualitySign, StringComparison.Ordinal) + 2,
                  value.Length - value.IndexOf(singleDivisionEqualitySign, StringComparison.Ordinal) - 2);

                return result.Trim(';');
            }

            return string.Empty;
        }

        public static string GetStrFromLeftSign(this string value)
        {
            const string doubleEqualitySign = "==";
            const string unequalSign = "!=";
            const string greatthanOrequaltoSign = ">=";
            const string lessthanOrequaltoSign = "<=";
            const string greatthanSign = ">";
            const string lessthanSign = "<";

            var func = new Func<string, string>(sign =>
            {
                var result = value.Substring(0, value.IndexOf(sign, StringComparison.Ordinal));
                return result;
            });

            if (!value.Contains(doubleEqualitySign) && !value.Contains(unequalSign)
                && !value.Contains(greatthanOrequaltoSign) && !value.Contains(lessthanOrequaltoSign)
                && !value.Contains(greatthanSign) && !value.Contains(lessthanSign))
                return string.Empty;

            if (value.Contains(doubleEqualitySign))
                return func.Invoke(doubleEqualitySign);

            if (value.Contains(unequalSign))
                return func.Invoke(unequalSign);

            if (value.Contains(greatthanOrequaltoSign))
                return func.Invoke(greatthanOrequaltoSign);

            if (value.Contains(lessthanOrequaltoSign))
                return func.Invoke(lessthanOrequaltoSign);

            if (value.Contains(greatthanSign))
                return func.Invoke(greatthanSign);

            if (value.Contains(lessthanSign))
                return func.Invoke(lessthanSign);

            return string.Empty;
        }

        public static string GetStrFromRightSign(this string value)
        {
            const string equalitySign = "==";
            const string unequalSign = "!=";
            const string greatthanOrequaltoSign = ">=";
            const string lessthanOrequaltoSign = "<=";
            const string greatthanSign = ">";
            const string lessthanSign = "<";

            var func = new Func<string, string>(sign =>
            {
                var result = value.Substring(value.IndexOf(sign, StringComparison.Ordinal) + sign.Length,
                     value.Length - value.IndexOf(sign, StringComparison.Ordinal) - sign.Length);
                return result.Trim(';').Trim('|').Trim('&');
            });

            if (!value.Contains(equalitySign) && !value.Contains(unequalSign) && !value.Contains(greatthanOrequaltoSign) &&
                !value.Contains(lessthanOrequaltoSign)
                && !value.Contains(greatthanSign) && !value.Contains(lessthanSign))
                return string.Empty;
            if (value.Contains(equalitySign))
                return func.Invoke(equalitySign);

            if (value.Contains(unequalSign))
                return func.Invoke(unequalSign);

            if (value.Contains(greatthanOrequaltoSign))
                return func.Invoke(greatthanOrequaltoSign);

            if (value.Contains(lessthanOrequaltoSign))
                return func.Invoke(lessthanOrequaltoSign);

            if (value.Contains(greatthanSign))
                return func.Invoke(greatthanSign);

            if (value.Contains(lessthanSign))
                return func.Invoke(lessthanSign);

            return string.Empty;
        }

        public static List<string> GetCodeLine(this string value)
        {
            return new List<string>(value.TrimEnd(';').Split(';'));
        }

        public static string[] GetStrsSplitByValue(
            this string value, string splitValue, bool isRemoveEmptyEntries = false)
        {
            var splitByDot = value.Split(new[] { splitValue },
                isRemoveEmptyEntries ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);

            return splitByDot;
        }

        public static string[] GetStrsSplitByValues(
            this string value, string[] splitValue, bool isRemoveEmptyEntries = false)
        {
            var splitByDot = value.Split(splitValue,
                isRemoveEmptyEntries ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);

            return splitByDot;
        }

        public enum EDefaultStateUnits
        {
            /// <summary>
            /// 初始
            /// </summary>
            Idle,

            /// <summary>
            /// 准备开始
            /// </summary>
            PreStart,

            /// <summary>
            /// 检测结束
            /// </summary>
            CheckEnd,

            ///// <summary>
            ///// 异常
            ///// </summary>
            //Error,

            ///// <summary>
            ///// 停止
            ///// </summary>
            //Stop,

            ///// <summary>
            ///// 暂停
            ///// </summary>
            //Halt,

            ///// <summary>
            ///// 超时
            ///// </summary>
            //OutTime,

            /// <summary>
            /// 检测失败
            /// </summary>
            CheckFail
        }

        //public static string[] GetStrsSplitByDot(this string value)
        //{
        //    var splitByDot = value.Split('.');
        //    return splitByDot.Length != 3 ? null : splitByDot;
        //}
    }
}
