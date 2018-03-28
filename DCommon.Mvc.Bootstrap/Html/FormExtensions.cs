using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace DCommon.Mvc.Bootstrap.Html
{
    public static class FormExtensions
    {
        public enum FormType
        {
            Default, Search, Inline, Horizontal
        }

        public static string ToCssClass(this FormType type)
        {
            switch (type)
            {
                case FormType.Search:
                    return "form-search";
                case FormType.Inline:
                    return "form-inline";
                case FormType.Horizontal:
                    return "form-horizontal";
                default:
                    return string.Empty;
            }
        }

        public static MvcForm BeginForm(this HtmlHelper html, string className)
        {
            return html.BeginForm(null, null, FormMethod.Post, new Dictionary<string, object>() { { "class", className } });
        }

        public static MvcForm BeginForm(this HtmlHelper html, FormType type)
        {
            var actionName = (string)html.ViewContext.RouteData.Values["action"];
            var controllerName = (string)html.ViewContext.RouteData.Values["controller"];
            return html.BeginForm(actionName, controllerName, FormMethod.Post, new { @class = type.ToCssClass() });
        }
    }
}
