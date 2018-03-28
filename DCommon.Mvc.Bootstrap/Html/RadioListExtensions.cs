using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DCommon.Mvc.Bootstrap.Html
{
    public static class RadioListExtensions
    {
        /// <summary>
        /// Returns a single-selection select element using the specified HTML helper and the name of the form field.
        /// </summary>
        /// 
        /// <returns>
        /// An HTML select element.
        /// </returns>
        /// <param name="html">The HTML helper instance that this method extends.</param><param name="name">The name of the form field to return.</param><exception cref="T:System.ArgumentException">The <paramref name="name"/> parameter is null or empty.</exception>
        public static MvcHtmlString RadioList(this HtmlHelper html, string name)
        {
            return html.RadioList(name, (IEnumerable<SelectListItem>)null, (string)null, (IDictionary<string, object>)null);
        }

        /// <summary>
        /// Returns a single-selection select element using the specified HTML helper, the name of the form field, and an option label.
        /// </summary>
        /// 
        /// <returns>
        /// An HTML select element with an option subelement for each item in the list.
        /// </returns>
        /// <param name="html">The HTML helper instance that this method extends.</param><param name="name">The name of the form field to return.</param><param name="optionLabel">The text for a default empty item. This parameter can be null.</param><exception cref="T:System.ArgumentException">The <paramref name="name"/> parameter is null or empty.</exception>
        public static MvcHtmlString RadioList(this HtmlHelper html, string name, string optionLabel)
        {
            return html.RadioList(name, (IEnumerable<SelectListItem>)null, optionLabel, (IDictionary<string, object>)null);
        }

        /// <summary>
        /// Returns a single-selection select element using the specified HTML helper, the name of the form field, and the specified list items.
        /// </summary>
        /// 
        /// <returns>
        /// An HTML select element with an option subelement for each item in the list.
        /// </returns>
        /// <param name="html">The HTML helper instance that this method extends.</param><param name="name">The name of the form field to return.</param><param name="selectList">A collection of <see cref="T:System.Web.Mvc.SelectListItem"/> objects that are used to populate the drop-down list.</param><exception cref="T:System.ArgumentException">The <paramref name="name"/> parameter is null or empty.</exception>
        public static MvcHtmlString RadioList(this HtmlHelper html, string name, IEnumerable<SelectListItem> selectList)
        {
            return html.RadioList(name, selectList, (string)null, (IDictionary<string, object>)null);
        }

        /// <summary>
        /// Returns a single-selection select element using the specified HTML helper, the name of the form field, the specified list items, and the specified HTML attributes.
        /// </summary>
        /// 
        /// <returns>
        /// An HTML select element with an option subelement for each item in the list.
        /// </returns>
        /// <param name="html">The HTML helper instance that this method extends.</param><param name="name">The name of the form field to return.</param><param name="selectList">A collection of <see cref="T:System.Web.Mvc.SelectListItem"/> objects that are used to populate the drop-down list.</param><param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param><exception cref="T:System.ArgumentException">The <paramref name="name"/> parameter is null or empty.</exception>
        public static MvcHtmlString RadioList(this HtmlHelper html, string name, IEnumerable<SelectListItem> selectList, object htmlAttributes)
        {
            return html.RadioList(name, selectList, (string)null, (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        /// <summary>
        /// Returns a single-selection select element using the specified HTML helper, the name of the form field, the specified list items, and the specified HTML attributes.
        /// </summary>
        /// 
        /// <returns>
        /// An HTML select element with an option subelement for each item in the list.
        /// </returns>
        /// <param name="html">The HTML helper instance that this method extends.</param><param name="name">The name of the form field to return.</param><param name="selectList">A collection of <see cref="T:System.Web.Mvc.SelectListItem"/> objects that are used to populate the drop-down list.</param><param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param><exception cref="T:System.ArgumentException">The <paramref name="name"/> parameter is null or empty.</exception>
        public static MvcHtmlString RadioList(this HtmlHelper html, string name, IEnumerable<SelectListItem> selectList, IDictionary<string, object> htmlAttributes)
        {
            return html.RadioList(name, selectList, (string)null, htmlAttributes);
        }

        /// <summary>
        /// Returns a single-selection select element using the specified HTML helper, the name of the form field, the specified list items, and an option label.
        /// </summary>
        /// 
        /// <returns>
        /// An HTML select element with an option subelement for each item in the list.
        /// </returns>
        /// <param name="html">The HTML helper instance that this method extends.</param><param name="name">The name of the form field to return.</param><param name="selectList">A collection of <see cref="T:System.Web.Mvc.SelectListItem"/> objects that are used to populate the drop-down list.</param><param name="optionLabel">The text for a default empty item. This parameter can be null.</param><exception cref="T:System.ArgumentException">The <paramref name="name"/> parameter is null or empty.</exception>
        public static MvcHtmlString RadioList(this HtmlHelper html, string name, IEnumerable<SelectListItem> selectList, string optionLabel)
        {
            return html.RadioList(name, selectList, optionLabel, (IDictionary<string, object>)null);
        }

        /// <summary>
        /// Returns a single-selection select element using the specified HTML helper, the name of the form field, the specified list items, an option label, and the specified HTML attributes.
        /// </summary>
        /// 
        /// <returns>
        /// An HTML select element with an option subelement for each item in the list.
        /// </returns>
        /// <param name="html">The HTML helper instance that this method extends.</param><param name="name">The name of the form field to return.</param><param name="selectList">A collection of <see cref="T:System.Web.Mvc.SelectListItem"/> objects that are used to populate the drop-down list.</param><param name="optionLabel">The text for a default empty item. This parameter can be null.</param><param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param><exception cref="T:System.ArgumentException">The <paramref name="name"/> parameter is null or empty.</exception>
        public static MvcHtmlString RadioList(this HtmlHelper html, string name, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes)
        {
            return html.RadioList(name, selectList, optionLabel, (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        /// <summary>
        /// Returns a single-selection select element using the specified HTML helper, the name of the form field, the specified list items, an option label, and the specified HTML attributes.
        /// </summary>
        /// 
        /// <returns>
        /// An HTML select element with an option subelement for each item in the list.
        /// </returns>
        /// <param name="html">The HTML helper instance that this method extends.</param><param name="name">The name of the form field to return.</param><param name="selectList">A collection of <see cref="T:System.Web.Mvc.SelectListItem"/> objects that are used to populate the drop-down list.</param><param name="optionLabel">The text for a default empty item. This parameter can be null.</param><param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param><exception cref="T:System.ArgumentException">The <paramref name="name"/> parameter is null or empty.</exception>
        public static MvcHtmlString RadioList(this HtmlHelper html, string name, IEnumerable<SelectListItem> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
        {
            ModelMetadata metadata = (ModelMetadata)null;
            string expression = name;
            IEnumerable<SelectListItem> selectList1 = selectList;
            string optionLabel1 = optionLabel;
            IDictionary<string, object> htmlAttributes1 = htmlAttributes;
            return RadioListHelper(html, metadata, expression, selectList1, optionLabel1, htmlAttributes1);
        }

        /// <summary>
        /// Returns an HTML select element for each property in the object that is represented by the specified expression using the specified list items.
        /// </summary>
        /// 
        /// <returns>
        /// An HTML select element for each property in the object that is represented by the expression.
        /// </returns>
        /// <param name="html">The HTML helper instance that this method extends.</param><param name="expression">An expression that identifies the object that contains the properties to display.</param><param name="selectList">A collection of <see cref="T:System.Web.Mvc.SelectListItem"/> objects that are used to populate the drop-down list.</param><typeparam name="TModel">The type of the model.</typeparam><typeparam name="TProperty">The type of the value.</typeparam><exception cref="T:System.ArgumentNullException">The <paramref name="expression"/> parameter is null.</exception>
        public static MvcHtmlString RadioListFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList)
        {
            return html.RadioListFor<TModel, TProperty>(expression, selectList, (string)null, (IDictionary<string, object>)null);
        }

        /// <summary>
        /// Returns an HTML select element for each property in the object that is represented by the specified expression using the specified list items and HTML attributes.
        /// </summary>
        /// 
        /// <returns>
        /// An HTML select element for each property in the object that is represented by the expression.
        /// </returns>
        /// <param name="html">The HTML helper instance that this method extends.</param><param name="expression">An expression that identifies the object that contains the properties to display.</param><param name="selectList">A collection of <see cref="T:System.Web.Mvc.SelectListItem"/> objects that are used to populate the drop-down list.</param><param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param><typeparam name="TModel">The type of the model.</typeparam><typeparam name="TProperty">The type of the value.</typeparam><exception cref="T:System.ArgumentNullException">The <paramref name="expression"/> parameter is null.</exception>
        public static MvcHtmlString RadioListFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, object htmlAttributes)
        {
            return html.RadioListFor<TModel, TProperty>(expression, selectList, (string)null, (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        /// <summary>
        /// Returns an HTML select element for each property in the object that is represented by the specified expression using the specified list items and HTML attributes.
        /// </summary>
        /// 
        /// <returns>
        /// An HTML select element for each property in the object that is represented by the expression.
        /// </returns>
        /// <param name="html">The HTML helper instance that this method extends.</param><param name="expression">An expression that identifies the object that contains the properties to display.</param><param name="selectList">A collection of <see cref="T:System.Web.Mvc.SelectListItem"/> objects that are used to populate the drop-down list.</param><param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param><typeparam name="TModel">The type of the model.</typeparam><typeparam name="TProperty">The type of the value.</typeparam><exception cref="T:System.ArgumentNullException">The <paramref name="expression"/> parameter is null.</exception>
        public static MvcHtmlString RadioListFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, IDictionary<string, object> htmlAttributes)
        {
            return html.RadioListFor<TModel, TProperty>( expression, selectList, (string)null, htmlAttributes);
        }

        /// <summary>
        /// Returns an HTML select element for each property in the object that is represented by the specified expression using the specified list items and option label.
        /// </summary>
        /// 
        /// <returns>
        /// An HTML select element for each property in the object that is represented by the expression.
        /// </returns>
        /// <param name="html">The HTML helper instance that this method extends.</param><param name="expression">An expression that identifies the object that contains the properties to display.</param><param name="selectList">A collection of <see cref="T:System.Web.Mvc.SelectListItem"/> objects that are used to populate the drop-down list.</param><param name="optionLabel">The text for a default empty item. This parameter can be null.</param><typeparam name="TModel">The type of the model.</typeparam><typeparam name="TProperty">The type of the value.</typeparam><exception cref="T:System.ArgumentNullException">The <paramref name="expression"/> parameter is null.</exception>
        public static MvcHtmlString RadioListFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel)
        {
            return html.RadioListFor<TModel, TProperty>( expression, selectList, optionLabel, (IDictionary<string, object>)null);
        }

        /// <summary>
        /// Returns an HTML select element for each property in the object that is represented by the specified expression using the specified list items, option label, and HTML attributes.
        /// </summary>
        /// 
        /// <returns>
        /// An HTML select element for each property in the object that is represented by the expression.
        /// </returns>
        /// <param name="html">The HTML helper instance that this method extends.</param><param name="expression">An expression that identifies the object that contains the properties to display.</param><param name="selectList">A collection of <see cref="T:System.Web.Mvc.SelectListItem"/> objects that are used to populate the drop-down list.</param><param name="optionLabel">The text for a default empty item. This parameter can be null.</param><param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param><typeparam name="TModel">The type of the model.</typeparam><typeparam name="TProperty">The type of the value.</typeparam><exception cref="T:System.ArgumentNullException">The <paramref name="expression"/> parameter is null.</exception>
        public static MvcHtmlString RadioListFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes)
        {
            return html.RadioListFor<TModel, TProperty>( expression, selectList, optionLabel, (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        /// <summary>
        /// Returns an HTML select element for each property in the object that is represented by the specified expression using the specified list items, option label, and HTML attributes.
        /// </summary>
        /// 
        /// <returns>
        /// An HTML select element for each property in the object that is represented by the expression.
        /// </returns>
        /// <param name="html">The HTML helper instance that this method extends.</param><param name="expression">An expression that identifies the object that contains the properties to display.</param><param name="selectList">A collection of <see cref="T:System.Web.Mvc.SelectListItem"/> objects that are used to populate the drop-down list.</param><param name="optionLabel">The text for a default empty item. This parameter can be null.</param><param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param><typeparam name="TModel">The type of the model.</typeparam><typeparam name="TProperty">The type of the value.</typeparam><exception cref="T:System.ArgumentNullException">The <paramref name="expression"/> parameter is null.</exception>
        public static MvcHtmlString RadioListFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
        {
            if (expression == null)
                if (expression == null)
                    throw new ArgumentNullException("expression");
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, html.ViewData);
            return RadioListHelper((HtmlHelper)html, metadata, ExpressionHelper.GetExpressionText((LambdaExpression)expression), selectList, optionLabel, htmlAttributes);
        }

        private static MvcHtmlString RadioListHelper(HtmlHelper html, ModelMetadata metadata, string expression, IEnumerable<SelectListItem> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
        {
            bool allowMultiple = false;
            IDictionary<string, object> htmlAttributes1 = htmlAttributes;
            return SelectInternal(html, metadata, optionLabel, expression, selectList, allowMultiple, htmlAttributes1);
        }

        internal static string ListItemToOption(string name, IDictionary<string, object> attributes, SelectListItem item)
        {
            TagBuilder tagBuilder = new TagBuilder("input")
            {
                InnerHtml = HttpUtility.HtmlEncode(item.Text)
            };

            tagBuilder.Attributes["type"] = "radio";
            if (name != null)
                tagBuilder.Attributes["name"] = name;
            if (attributes != null)
                tagBuilder.MergeAttributes(attributes);
            if (item.Value != null)
                tagBuilder.Attributes["value"] = item.Value;
            if (item.Selected)
                tagBuilder.Attributes["checked"] = "checked";

            var html = string.Format(
                "<label class=\"radio inline\">{0}</label>",
                tagBuilder.ToString(TagRenderMode.Normal));

            return html;
        }

        private static MvcHtmlString SelectInternal(this HtmlHelper html, ModelMetadata metadata, string optionLabel, string name, IEnumerable<SelectListItem> selectList, bool allowMultiple, IDictionary<string, object> htmlAttributes)
        {
            string fullHtmlFieldName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            if (string.IsNullOrEmpty(fullHtmlFieldName))
                throw new ArgumentException("name");
            bool flag = false;
            if (selectList == null)
            {
                selectList = html.GetSelectData(name);
                flag = true;
            }
            object defaultValue = html.GetModelStateValue(fullHtmlFieldName, typeof(string[]));
            if (!flag && defaultValue == null && !string.IsNullOrEmpty(name))
                defaultValue = html.ViewData.Eval(name);
            if (defaultValue != null)
                selectList = selectList.GetSelectListWithDefaultValue(defaultValue, allowMultiple);

            StringBuilder stringBuilder = new StringBuilder();
            if (optionLabel != null)
                stringBuilder.AppendLine(ListItemToOption(name, htmlAttributes, new SelectListItem()
                {
                    Text = optionLabel,
                    Value = string.Empty,
                    Selected = false
                }));
            foreach (SelectListItem selectListItem in selectList)
                stringBuilder.AppendLine(ListItemToOption(name, htmlAttributes, selectListItem));

            return new MvcHtmlString(stringBuilder.ToString());
        }
    }
}
