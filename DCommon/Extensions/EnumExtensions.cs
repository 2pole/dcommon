using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using DCommon.Caching;
using DCommon.Utility;

namespace DCommon
{
    public static partial class EnumExtensions
    {
		public static string GetDescription(this Enum value)
		{
			Type enumType = value.GetType();
            EnumCache cache = EnumHelper.GetEnumCache(enumType.TypeHandle);
			ulong valueUL = EnumHelper.ToUInt64(value);
			int idx = Array.BinarySearch(cache.Values, valueUL);
			if (idx >= 0)
			{
				return cache.Descriptions[idx];
			}
			if (!cache.HasFlagsAttribute)
			{
				return GetStringValue(enumType, valueUL);
			}
			List<string> list = new List<string>();
			for (int i = cache.Values.Length - 1; i >= 0 && valueUL != 0UL; i--)
			{
				ulong enumValue = cache.Values[i];
				if (enumValue == 0UL)
				{
					continue;
				}
				if ((valueUL & enumValue) == enumValue)
				{
					valueUL -= enumValue;
					list.Add(cache.Descriptions[i]);
				}
			}
			list.Reverse();
			if (list.Count == 0 || valueUL != 0UL)
			{
				list.Add(GetStringValue(enumType, valueUL));
			}
			return string.Join(", ", list);
		}
        
		private static string GetStringValue(Type enumType, ulong value)
		{
			if ((value & 0x8000000000000000UL) > 0)
			{
				switch (Type.GetTypeCode(enumType))
				{
					case TypeCode.SByte:
					case TypeCode.Int16:
					case TypeCode.Int32:
					case TypeCode.Int64:
						long longValue = unchecked((long)value);
						return longValue.ToString(CultureInfo.CurrentCulture);
				}
			}
			return value.ToString(CultureInfo.CurrentCulture);
		}

        public static TEnum ToEnum<TEnum>(this string value)
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value);
        }

        public static TEnum ToEnum<TEnum>(this string value, bool ignoreCase)
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);
        }
		
		public static object ToEnumEx(this string value, Type enumType)
		{
            return ToEnumEx(value, enumType, false);
		}

        public static object ToEnumEx(string value, Type enumType, bool ignoreCase)
		{
            Guard.Against<ArgumentNullException>(enumType == null, "enumType");
            Guard.Against<ArgumentNullException>(string.IsNullOrWhiteSpace(value), "value");

			if (!enumType.IsEnum)
			{
			    throw new ArgumentException("The enumType must be enum.");
			}
			value = value.Trim();
			ulong tmpValue;
			if (ParseString(value, out tmpValue))
			{
				return Enum.ToObject(enumType, tmpValue);
			}
			EnumCache cache = EnumHelper.GetEnumCache(enumType.TypeHandle);
			StringComparison comparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
			ulong valueUL = 0;
			int start = 0;
			do
			{
				while (char.IsWhiteSpace(value, start)) { start++; }
				int idx = value.IndexOf(',', start);
				if (idx < 0) { idx = value.Length; }
				int nIdx = idx - 1;
				while (char.IsWhiteSpace(value, nIdx)) { nIdx--; }
				if (nIdx >= start)
				{
					string str = value.Substring(start, nIdx - start + 1);
					int j = 0;
					for (; j < cache.Names.Length; j++)
					{
						if (string.Equals(str, cache.Names[j], comparison))
						{
							valueUL |= cache.Values[j];
							break;
						}
					}
					if (j == cache.Names.Length && cache.HasDescription)
					{
						for (j = 0; j < cache.Descriptions.Length; j++)
						{
							if (string.Equals(str, cache.Descriptions[j], comparison))
							{
								valueUL |= cache.Values[j];
								break;
							}
						}
					}
					if (j == cache.Descriptions.Length)
					{
						if (ParseString(str, out tmpValue))
						{
							valueUL |= tmpValue;
						}
						else
						{
                            throw new ArgumentOutOfRangeException("");
						}
					}
				}
				start = idx + 1;
			} while (start < value.Length);
			return Enum.ToObject(enumType, valueUL);
		}

        public static TEnum ToEnumEx<TEnum>(this string value)
		{
            return (TEnum)ToEnumEx(value, typeof(TEnum), false);
		}

        public static TEnum ToEnumEx<TEnum>(this string value, bool ignoreCase)
		{
            return (TEnum)ToEnumEx(value, typeof(TEnum), ignoreCase);
		}

		private static bool ParseString(string str, out ulong value)
		{
			char firstChar = str[0];
			if (char.IsDigit(firstChar) || firstChar == '+')
			{
				return ulong.TryParse(str, out value);
			}
			else if (firstChar == '-')
			{
				long valueL;
				if (long.TryParse(str, out valueL))
				{
					value = unchecked((ulong)valueL);
					return true;
				}
			}
			value = 0;
			return false;
		}

		public static bool AnyFlag(this Enum baseEnum, Enum value)
		{
			if (!Type.GetTypeHandle(baseEnum).Equals(Type.GetTypeHandle(value)))
                throw new ArgumentNullException(string.Format("The enum type {0} does not math the value {1}", baseEnum.GetType(), value));

            return ((EnumHelper.ToUInt64(baseEnum) & EnumHelper.ToUInt64(value)) != 0);
		}
    }
}
