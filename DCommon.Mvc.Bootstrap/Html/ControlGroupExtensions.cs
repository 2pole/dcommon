using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace DCommon.Mvc.Bootstrap.Html
{
    public static class ControlGroupExtensions
    {
        public static MvcContainer BeginControlGroup(this HtmlHelper html)
        {
            return html.ContainerHelper(new { @class = BootstrapperCss.ControlGroup });
        }

        public static MvcContainer BeginControlGroupFor<T>(this HtmlHelper<T> html, Expression<Func<T, object>> modelProperty)
        {
            var partialFieldName = ExpressionHelper.GetExpressionText(modelProperty);
            var fullHtmlFieldName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(partialFieldName);
            var className = html.ViewData.ModelState.IsValidField(fullHtmlFieldName)
                                ? BootstrapperCss.ControlGroup
                                : BootstrapperCss.ControlGroupError;
            var container = html.ContainerHelper(new { @class = className });
            return container;
        }

        public static MvcContainer BeginControlGroupFor<T>(this HtmlHelper<T> html, string propertyName)
        {
            var partialFieldName = propertyName;
            var fullHtmlFieldName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(partialFieldName);
            var className = html.ViewData.ModelState.IsValidField(fullHtmlFieldName)
                                ? BootstrapperCss.ControlGroup
                                : BootstrapperCss.ControlGroupError;
            var container = html.ContainerHelper(new { @class = className });
            return container;
        }

        public static MvcHtmlString ControlLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return html.LabelFor(expression, new Dictionary<string, object> { { BootstrapperCss.CssClass, BootstrapperCss.ControlLabel } });
        }
    }
}