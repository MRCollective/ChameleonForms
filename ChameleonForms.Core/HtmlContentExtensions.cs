using System.IO;
using System.Text;
using System.Text.Encodings.Web;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Html
{
    /// <summary>
    /// Extensions to <see cref="IHtmlContent"/>.
    /// </summary>
    public static class HtmlContentExtensions
    {
        /// <summary>
        /// Returns the encoded HTML string for the <see cref="IHtmlContent"/>.
        /// </summary>
        /// <param name="htmlContent">The <see cref="IHtmlContent"/> to extract the string for</param>
        /// <returns>The encoded HTML in string form</returns>
        public static string ToHtmlString(this IHtmlContent htmlContent)
        {
            if (htmlContent == null)
                return null;

            var sb = new StringBuilder();
            TextWriter tw = new StringWriter(sb);
            htmlContent.WriteTo(tw, HtmlEncoder.Default);
            return sb.ToString();
        }
    }
}
