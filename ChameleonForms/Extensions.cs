using System;
using System.Web;
using System.Web.Mvc;

namespace ChameleonForms
{
    /// <summary>
    /// Extension methods used by ChameleonForms.
    /// </summary>
    public static class Extensions
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
        /// Shortcut to tersely create HtmlAttributes object from the HTML Helper.
        /// </summary>
        /// <param name="helper">The HTML helper</param>
        /// <param name="attrs">Any attributes you want to define in attr_name => attr_value format</param>
        /// <returns>A HtmlAttributes object that can be used to chain methods to further specify attributes</returns>
        public static HtmlAttributes Attrs(this HtmlHelper helper, params Func<object, object>[] attrs)
        {
            return new HtmlAttributes(attrs);
        }
    }
}
