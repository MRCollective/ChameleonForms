using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;

namespace ChameleonForms.Templates
{
    internal static class WebViewPage
    {
        internal static void WriteLiteralTo(TextWriter textWriter, string value)
        {
            textWriter.Write(value);
        }

        internal static void WriteTo(TextWriter textWriter, IHtmlContent htmlContent)
        {
            htmlContent.WriteTo(textWriter, HtmlEncoder.Default);
        }

        internal static void WriteTo(TextWriter textWriter, string value)
        {
            textWriter.Write(HtmlEncoder.Default.Encode(value));
        }
    }
}
