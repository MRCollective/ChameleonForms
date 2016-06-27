using System.Web;

namespace ChameleonForms.Utils
{
    /// <summary>
    /// Extension methods for dealing with HTML.
    /// </summary>
    public static class HtmlExtensions
    {
        /// <summary>
        /// Converts a string to an encoded <see cref="IHtml"/>.
        /// </summary>
        /// <param name="content">The content to encode and turn into an IHtmlString</param>
        /// <returns>The IHtmlString</returns>
        public static IHtml ToHtml(this string content)
        {
            return new Html(HttpUtility.HtmlEncode(content ?? ""));
        }
    }
}
