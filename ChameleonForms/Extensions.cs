using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Web;

namespace ChameleonForms
{
    /// <summary>
    /// Extension methods used by ChameleonForms.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Converts a string to an encoded <see cref="IHtmlContent"/>.
        /// </summary>
        /// <param name="content">The content to encode and turn into an IHtmlContent</param>
        /// <returns>The IHtmlContent</returns>
        public static IHtmlContent ToHtml(this string content)
        {
            return new HtmlString(HttpUtility.HtmlEncode(content ?? ""));
        }

        /// <summary>
        /// Shortcut to tersely create HtmlAttributes object from the HTML Helper.
        /// </summary>
        /// <param name="helper">The HTML helper</param>
        /// <param name="attrs">Any attributes you want to define in attr_name => attr_value format</param>
        /// <returns>A HtmlAttributes object that can be used to chain methods to further specify attributes</returns>
        public static HtmlAttributes Attrs(this IHtmlHelper helper, params Func<object, object>[] attrs)
        {
            return new HtmlAttributes(attrs);
        }
    }
}
