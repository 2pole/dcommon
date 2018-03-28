using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Web;

namespace DCommon
{
    public static class NameValueCollectionExtentions
    {
        public static string GetValue(this NameValueCollection nameValues, string name, string defaultValue)
        {
            string value = nameValues[name];
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;
            return value;
        }

        public static bool GetBooleanValue(this NameValueCollection nameValues, string name, bool defaultValue)
        {
            string value = nameValues[name];
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;
            return value.ToBoolean(defaultValue);
        }

        public static int GetIntValue(this NameValueCollection nameValues, string name, int defaultValue)
        {
            string value = nameValues[name];
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;
            return value.ToInt32(defaultValue);
        }

        public static string ToQueryString(this NameValueCollection nameValues)
        {
            var query = from string name in nameValues
                        select String.Concat(name, "=", nameValues[name]);

            var queryString = String.Join("&", query);
            return queryString;
        }
    }
}
