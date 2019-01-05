using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;

namespace Microsoft.AspNetCore.Html
{
    internal static class IHtmlContentExtension
    {
        public static string ToHtmlString(this IHtmlContent htmlContent)
        {
            var sb = new StringBuilder();
            TextWriter tw = new StringWriter(sb);
            htmlContent.WriteTo(tw, HtmlEncoder.Default);
            return sb.ToString();
        }
    }
}
