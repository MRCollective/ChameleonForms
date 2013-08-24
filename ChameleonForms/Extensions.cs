using System.Web;

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
    }
}
