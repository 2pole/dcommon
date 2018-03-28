using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace DCommon.Mvc.Bootstrap.Html
{
    public static class HtmlHelperExtensions
    {
        internal static object GetModelStateValue(this HtmlHelper html, string key, Type destinationType)
        {
            ModelState modelState;
            if (html.ViewData.ModelState.TryGetValue(key, out modelState) && modelState.Value != null)
                return modelState.Value.ConvertTo(destinationType, (CultureInfo)null);
            else
                return (object)null;
        }

        internal static IEnumerable<SelectListItem> GetSelectListWithDefaultValue(this IEnumerable<SelectListItem> selectList, object defaultValue, bool allowMultiple)
        {
            IEnumerable source;
            if (allowMultiple)
            {
                source = defaultValue as IEnumerable;
                if (source == null || source is string)
                    throw new InvalidOperationException();
            }
            else
                source = (IEnumerable)new object[] { defaultValue };

            var items = source.Cast<object>().Select(value => Convert.ToString(value, CultureInfo.CurrentCulture));
            HashSet<string> hashSet = new HashSet<string>(items, StringComparer.OrdinalIgnoreCase);
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (SelectListItem selectListItem in selectList)
            {
                selectListItem.Selected = selectListItem.Value != null ? hashSet.Contains(selectListItem.Value) : hashSet.Contains(selectListItem.Text);
                list.Add(selectListItem);
            }
            return (IEnumerable<SelectListItem>)list;
        }

        internal static IEnumerable<SelectListItem> GetSelectData(this HtmlHelper html, string name)
        {
            object obj = (object)null;
            if (html.ViewData != null)
                obj = html.ViewData.Eval(name);

            if (obj == null)
            {
                throw new InvalidOperationException();
            }
            else
            {
                IEnumerable<SelectListItem> enumerable = obj as IEnumerable<SelectListItem>;
                if (enumerable != null)
                    return enumerable;
                throw new InvalidOperationException();
            }
        }
    }
}
