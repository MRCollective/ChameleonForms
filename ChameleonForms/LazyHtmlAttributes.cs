using System;
using System.Web;

namespace ChameleonForms
{
    /// <summary>
    /// HtmlAttributes class that evaluates a given HTML generator when .ToHtmlString() is called.
    /// </summary>
    public class LazyHtmlAttributes : HtmlAttributes
    {
        private readonly Func<HtmlAttributes, IHtmlString> _htmlGenerator;

        /// <summary>
        /// Construct a LazyHtmlAttributes class.
        /// </summary>
        /// <param name="htmlGenerator">The generator to use to generate the HTML when .ToHtmlString() is called</param>
        public LazyHtmlAttributes(Func<HtmlAttributes, IHtmlString> htmlGenerator)
        {
            if (htmlGenerator == null)
                throw new ArgumentNullException("htmlGenerator");

            _htmlGenerator = htmlGenerator;
        }

        /// <summary>
        /// Invokes the given HTML generator to return HTML.
        /// </summary>
        /// <returns>The generated HTML</returns>
        public override string ToHtmlString()
        {
            return _htmlGenerator(this).ToHtmlString();
        }
    }
}