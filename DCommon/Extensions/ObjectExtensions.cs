using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using System.IO;
using System.Xml.Serialization;
using Formatting = Newtonsoft.Json.Formatting;

namespace DCommon
{
    public static class ObjectExtentions
    {
        private static readonly JsonSerializerSettings JsonSettings;
        private static readonly XmlSerializerNamespaces EmptyNamespaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName(string.Empty, string.Empty) });

        static ObjectExtentions()
        {
            JsonSettings = new JsonSerializerSettings();
            //JsonSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore;
            //JsonSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            JsonSettings.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore;
            JsonSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            JsonSettings.Converters.Add(new Newtonsoft.Json.Converters.IsoDateTimeConverter());
            //JsonSettings.Converters.Add(new DataRowConverter());
        }

        #region ToJson - Json.NET
        public static string ToJson(this Object entity)
        {
            if (entity == null)
                return null;
            return JsonConvert.SerializeObject(entity, Formatting.None, JsonSettings);
        }

        public static string ToJson(this Object entity, Formatting formatting)
        {
            return JsonConvert.SerializeObject(entity, formatting, JsonSettings);
        }

        public static string ToJson(this Object entity, params JsonConverter[] converters)
        {
            return JsonConvert.SerializeObject(entity, converters);
        }

        public static string ToJson(this Object entity, Formatting formatting, params JsonConverter[] converters)
        {
            return JsonConvert.SerializeObject(entity, formatting, converters);
        }

        public static string ToJson(this Object entity, Formatting formatting, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(entity, formatting, settings);
        }

        public static string ToJson(this XmlNode xml,
          Formatting formatting = Formatting.None,
          bool omitRootObject = false)
        {
            var json = JsonConvert.SerializeXmlNode(xml, formatting, omitRootObject);
            return json;
        }

        #endregion

        #region FromJson - Json.NET

        public static T FromJson<T>(this string json)
        {
            try
            {
                if (!string.IsNullOrEmpty(json))
                    return JsonConvert.DeserializeObject<T>(json, JsonSettings);
            }
            catch
            { }
            return default(T);
        }
        #endregion

        #region ToXml - XmlSerializer

        public static string ToXml(this object value, XmlSerializerNamespaces namespaces = null)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                value.ToXml(ms, namespaces);
                ms.Position = 0;
                using (StreamReader reader = new StreamReader(ms))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static void ToXml(this object value, Stream outputStream, XmlSerializerNamespaces namespaces = null)
        {
            XmlSerializer serializer = new XmlSerializer(value.GetType());
            serializer.Serialize(outputStream, value, namespaces);
        }

        public static string ToXmlWithoutNamespaces(this object value)
        {
            var xml = value.ToXml(EmptyNamespaces);
            return xml;
        }

        #endregion

        #region FromXml - XmlSerializer
        public static T FromXml<T>(this string xmlDoc)
        {
            using (MemoryStream ms = new MemoryStream())
            using (StreamWriter writer = new StreamWriter(ms))
            {
                writer.Write(xmlDoc);
                writer.Flush();
                ms.Position = 0;
                return ms.FromXml<T>();
            }
        }

        public static T FromXml<T>(this Stream inputStream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            var obj = serializer.Deserialize(inputStream);
            return (T)obj;
        }
        #endregion

        #region FromXml - Json.NET

        public static T FromXml<T>(this XmlNode xml,
                                   Formatting formatting = Formatting.None,
                                   bool omitRootObject = false)
        {
            var json = JsonConvert.SerializeXmlNode(xml, formatting, omitRootObject);
            return json.FromJson<T>();
        }

        #endregion

        #region JsonToXml - Json.NET

        public static XmlDocument JsonToXml(this string json, string deserializeRootElementName = null, bool writeArrayAttribute = false)
        {
            var document = JsonConvert.DeserializeXmlNode(json, deserializeRootElementName, writeArrayAttribute);
            return document;
        }
        #endregion
    }
}
