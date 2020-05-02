using System;
using Microsoft.AspNetCore.Html;

namespace ChameleonForms.Component
{
    /// <summary>
    /// HtmlAttributes class that is returned from button methods that allows for extension methods targetted at buttons.
    /// </summary>
    public class ButtonHtmlAttributes : LazyHtmlAttributes
    {
        /// <summary>
        /// Construct a ButtonHtmlAttributes class.
        /// </summary>
        /// <param name="htmlGenerator">The generator to use to generate the HTML when .ToHtmlString() is called</param>
        public ButtonHtmlAttributes(Func<HtmlAttributes, IHtmlContent> htmlGenerator) : base(htmlGenerator) { }
    }
}
