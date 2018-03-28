using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCommon
{
    public static class DateTimeExtentions
    {
        public static int CompareMonthAndDay(this DateTime source, DateTime target)
        {
            int monthCompare = source.Month.CompareTo(target.Month);
            if (monthCompare == 0)
                return source.Day.CompareTo(target.Day);
            else
                return monthCompare;
        }

        public static DateTime ToFirstDayOfMonth(this DateTime source)
        {
            return new DateTime(source.Year, source.Month, 1);
        }
    }
}
