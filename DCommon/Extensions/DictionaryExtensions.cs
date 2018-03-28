using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;

namespace DCommon
{
    public static class DictionaryExtensions
    {
        public static bool TryGetValue<T>(this IDictionary<string, object> collection, string key, out T value)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            object obj;
            if (collection.TryGetValue(key, out obj) && obj is T)
            {
                value = (T)obj;
                return true;
            }
            else
            {
                value = default(T);
                return false;
            }
        }

        public static T GetValue<T>(this IDictionary<string, object> collection, string key, T defaultValue)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            object obj;
            if (collection.TryGetValue(key, out obj) && obj is T)
            {
                return (T)obj;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>Extension Method to initialize an <see cref="Dictionary{TKey,TValue}"/></summary>
        /// <param name="dict"></param>
        /// <param name="hash">The key / value pairs to add to the dictionary.</param>
        /// <returns>The the dictionary.</returns>
        public static IDictionary<string, T> Add<T>(this IDictionary<string, T> dict, params Func<object, T>[] hash)
        {
            if (dict == null || hash == null)
            {
                return dict;
            }

            foreach (var func in hash)
            {
                dict.Add(func.Method.GetParameters()[0].Name, func(null));
            }
            return dict;
        }

        /// <summary>Extension Method to initialize an <see cref="IDictionary"/></summary>
        /// <param name="dict"></param>
        /// <param name="hash">The key / value pairs to add to the dictionary.</param>
        /// <returns>The the dictionary.</returns>
        public static IDictionary Add(this IDictionary dict, params Func<object, object>[] hash)
        {
            if (dict == null || hash == null)
            {
                return dict;
            }

            foreach (var func in hash)
            {
                dict.Add(func.Method.GetParameters()[0].Name, func(null));
            }

            return dict;
        }

        /// <summary>
        /// Takes an anonymous object and converts it to a <see cref="Dictionary{String,Object}"/>
        /// </summary>
        /// <param name="objectToConvert">The object to convert</param>
        /// <returns>A generic dictionary</returns>
        public static Dictionary<string, object> AnonymousObjectToCaseSensitiveDictionary(object objectToConvert)
        {
            var dictionary = new Dictionary<string, object>(StringComparer.Ordinal);

            if (objectToConvert != null)
            {
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(objectToConvert))
                {
                    dictionary[property.Name] = property.GetValue(objectToConvert);
                }
            }

            return dictionary;
        }

        /// <summary>
        /// Converts a dictionary into a string of HTML attributes
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public static string ToHtmlAttributes(this IDictionary<string, object> attributes)
        {
            if (attributes == null || attributes.Count == 0)
            {
                return string.Empty;
            }

            const string attributeFormat = "{0}=\"{1}\"";

            var attributesEncoded = from pair in attributes
                                    let value = pair.Value == null ? null : HttpUtility.HtmlAttributeEncode(pair.Value.ToString())
                                    select string.Format(attributeFormat, pair.Key, value);

            return string.Join(" ", attributesEncoded.ToArray());
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> collection, TKey key,
                                                  Func<TKey, TValue> creator)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            TValue cachedValue;
            if (!collection.TryGetValue(key, out cachedValue))
            {
                cachedValue = creator(key);
                collection.Add(key, cachedValue);
            }

            return cachedValue;
        }
    }
}
