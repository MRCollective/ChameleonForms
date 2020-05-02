#pragma warning disable 1591
using Microsoft.AspNetCore.Html;

namespace ChameleonForms.Templates.ChameleonFormsDefaultTemplate.Params
{
    public class BeginSectionParams
    {
        public IHtmlContent Heading { get; set; }
        public IHtmlContent LeadingHtml { get; set; }
        public HtmlAttributes HtmlAttributes { get; set; }
    }
}
