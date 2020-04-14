using System.IO;
using System.Text;
using System.Text.Encodings.Web;

namespace Microsoft.AspNetCore.Html
{
    public static class HtmlContentExtensions
    {
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
