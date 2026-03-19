using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonUtility
{
    public static class EnumOperater
    {
        public static List<T> GetEnumValueList<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }

        public static T GetCustomAttribute<T>(this Enum em)
        {
            var type = em.GetType();
            var fd = type.GetField(em.ToString());
            if (fd == null)
            {
                var returnNull = new object();
                return (T) returnNull;
            }

            var attrs = fd.GetCustomAttributes(typeof(T), false);
            object obj = null;
            foreach (var t in attrs)
                obj = (T) t;

            return (T) obj;
        }

        public static T GetEnum<T>(string enumName)
        {
            return (T) Enum.Parse(typeof(T), enumName);
        }

        public static T GetEnumByValue<T>(object value)
        {
            return (T)Enum.Parse(typeof(T), value.ToString(), true);
        }
    }
}
