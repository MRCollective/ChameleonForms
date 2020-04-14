using Microsoft.AspNetCore.Html;
using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Web;

namespace ChameleonForms
{
    /// <summary>
    /// HtmlAttributes class that evaluates a given HTML generator when .ToHtmlString() is called.
    /// </summary>
    public class LazyHtmlAttributes : HtmlAttributes
    {
        private readonly Func<HtmlAttributes, IHtmlContent> _htmlGenerator;

        /// <summary>
        /// Construct a LazyHtmlAttributes class.
        /// </summary>
        /// <param name="htmlGenerator">The generator to use to generate the HTML when .ToHtmlString() is called</param>
        public LazyHtmlAttributes(Func<HtmlAttributes, IHtmlContent> htmlGenerator)
        {
            if (htmlGenerator == null)
                throw new ArgumentNullException("htmlGenerator");

            _htmlGenerator = htmlGenerator;
        }

        /// <summary>
        /// Invokes the given HTML generator to return HTML.
        /// </summary>
        public override void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            _htmlGenerator(this).WriteTo(writer, encoder);
        }
    }
}