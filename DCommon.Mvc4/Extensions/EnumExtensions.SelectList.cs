using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace DCommon.Mvc
{
    public static partial class EnumExtensions
    {
        public static IEnumerable<SelectListItem> ToSelect<TEnum>(this TEnum value)
               where TEnum : struct
        {
            var textValues = EnumHelper.GetTextValues<TEnum>(true);
            var selectItems = textValues.Select(d => new SelectListItem() { Text = d.Key, Value = d.Value.ToString(), Selected = Equals(value, d.Value) });
            return selectItems;
        }
    }
}
