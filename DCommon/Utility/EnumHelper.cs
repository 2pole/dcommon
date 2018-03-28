using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace DCommon
{
    public static class EnumHelper
    {
        private static readonly IDictionary<RuntimeTypeHandle, EnumCache> EnumCaches = new Dictionary<RuntimeTypeHandle, EnumCache>();
        
        public static TEnum[] GetValues<TEnum>()
        {
            return Enum.GetValues(typeof(TEnum)) as TEnum[];
        }

        public static ICollection<KeyValuePair<string, object>> GetTextValues(Type enumType)
        {
            return GetTextValues(enumType, true);
        }

        public static ICollection<KeyValuePair<string, object>> GetTextValues(Type enumType, bool useDescription)
        {
            var enumList =  new Collection<KeyValuePair<string, object>>();
            EnumCache cache = GetEnumCache(enumType.TypeHandle);
            ulong[] values = cache.Values;
            string[] names = useDescription ? cache.Descriptions : cache.Names;
            for (int i = 0; i < values.Length; i++)
            {
                var pair = new KeyValuePair<string, object>(names[i], Enum.ToObject(enumType, values[i]));
                enumList.Add(pair);
            }
            return enumList;
        }

        public static ICollection<KeyValuePair<string, TEnum>> GetTextValues<TEnum>()
        {
            return GetTextValues<TEnum>(true);
        }

        public static ICollection<KeyValuePair<string, TEnum>> GetTextValues<TEnum>(bool useDescription)
        {
            Type enumType = typeof(TEnum);
            var enumList = new Collection<KeyValuePair<string, TEnum>>();
            EnumCache cache = GetEnumCache(enumType.TypeHandle);
            ulong[] values = cache.Values;
            string[] names = useDescription ? cache.Descriptions : cache.Names;
            for (int i = 0; i < values.Length; i++)
            {
                var pair = new KeyValuePair<string, TEnum>(names[i], (TEnum)Enum.ToObject(enumType, values[i]));
                enumList.Add(pair);
            }
            return enumList;
        }

        internal static EnumCache GetEnumCache(RuntimeTypeHandle typeHandle)
        {
            return EnumCaches.GetOrAdd(typeHandle, type =>
            {
                Type enumType = Type.GetTypeFromHandle(type);
                FieldInfo[] fields = enumType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
                int len = fields.Length;
                ulong[] values = new ulong[len];
                for (int i = 0; i < len; i++)
                {
                    values[i] = ToUInt64(fields[i].GetRawConstantValue());
                }
                Array.Sort(values, fields);
                string[] names = new string[len];
                string[] descs = new string[len];
                bool hasDesc = false;
                for (int i = 0; i < len; i++)
                {
                    FieldInfo fieldInfo = fields[i];
                    names[i] = descs[i] = fieldInfo.Name;
                    DescriptionAttribute attr = Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute), false) as DescriptionAttribute;
                    if (attr != null)
                    {
                        descs[i] = attr.Description;
                        hasDesc = true;
                    }
                }
                if (!hasDesc)
                {
                    descs = names;
                }
                return new EnumCache(enumType.IsDefined(typeof(FlagsAttribute), false), hasDesc, values, names, descs);
            });
        }

        internal static ulong ToUInt64(object value)
        {
            switch (Convert.GetTypeCode(value))
            {
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                    return unchecked((ulong)Convert.ToInt64(value, CultureInfo.InvariantCulture));
                case TypeCode.Byte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return Convert.ToUInt64(value, CultureInfo.InvariantCulture);
            }
            Debug.Fail("Invalid enum type.");
            return 0;
        }
    }

    [StructLayout(LayoutKind.Auto)]
    internal struct EnumCache
    {
        public ulong[] Values;

        public string[] Descriptions;

        public string[] Names;

        public bool HasFlagsAttribute;

        public bool HasDescription;

        public EnumCache(bool hasFlags, bool hasDesc, ulong[] values, string[] names, string[] descs)
        {
            this.HasFlagsAttribute = hasFlags;
            this.HasDescription = hasDesc;
            this.Values = values;
            this.Names = names;
            this.Descriptions = descs;
        }
    }
}
