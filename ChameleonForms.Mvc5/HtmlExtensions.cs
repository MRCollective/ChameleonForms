using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChameleonForms.Enums;

namespace ChameleonForms
{
    /// <summary>
    /// Extension methods used by ChameleonForms.
    /// </summary>
    public static class HtmlExtensions
    {
        /// <summary>
        /// Converts a string to an encoded <see cref="IHtmlString"/>.
        /// </summary>
        /// <param name="content">The content to encode and turn into an IHtmlString</param>
        /// <returns>The IHtmlString</returns>
        public static IHtmlString ToHtml(this string content)
        {
            return new HtmlString(HttpUtility.HtmlEncode(content ?? ""));
        }

        /// <summary>
        /// Converts a string to an encoded <see cref="IHtml"/>.
        /// </summary>
        /// <param name="content">The content to encode and turn into an IHtml</param>
        /// <returns>The IHtmlString</returns>
        public static IHtml ToIHtml(this string content)
        {
            return content.ToHtml().ToIHtml();
        }

        /// <summary>
        /// Converts a <see cref="IHtmlString"/> to a <see cref="IHtml"/>.
        /// </summary>
        /// <param name="content">The content to turn into an IHtml</param>
        /// <returns>The IHtml</returns>
        public static IHtml ToIHtml(this IHtmlString content)
        {
            if (content == null)
                return null;
            return new Html(content.ToHtmlString());
        }

        /// <summary>
        /// Converts a <see cref="IHtml"/> to a <see cref="IHtmlString"/>.
        /// </summary>
        /// <param name="content">The content to turn into an IHtmlString</param>
        /// <returns>The IHtmlString</returns>
        public static IHtmlString ToIHtmlString(this IHtml content)
        {
            if (content == null)
                return null;
            return new HtmlString(content.ToHtmlString());
        }
        
        /// <summary>
        /// Shortcut to tersely create HtmlAttributes object from the HTML Helper.
        /// </summary>
        /// <param name="helper">The HTML helper</param>
        /// <param name="attrs">Any attributes you want to define in attr_name => attr_value format</param>
        /// <returns>A HtmlAttributes object that can be used to chain methods to further specify attributes</returns>
        public static HtmlAttributes Attrs(this HtmlHelper helper, params Func<object, object>[] attrs)
        {
            return new HtmlAttributes(attrs);
        }

        public static IEnumerable<SelectListItem> ToMvcSelectList(this IEnumerable<FieldGenerators.SelectListItem> selectList)
        {
            return selectList.Select(i =>
                new SelectListItem
                {
                    Value = i.Value,
                    Disabled = i.Disabled,
                    Selected = i.Selected,
                    Text = i.Text
                });
        }

        public static FormSubmitMethod ToFormSubmitMethod(this FormMethod method)
        {
            switch (method)
            {
                case FormMethod.Get:
                    return FormSubmitMethod.Get;
                default:
                    return FormSubmitMethod.Post;
            }
        }
    }
}
