using Microsoft.AspNetCore.Html;

namespace ChameleonForms.Templates.ChameleonFormsDefaultTemplate.Params
{
    internal class BeginSectionParams
    {
        public IHtmlContent Heading { get; set; }
        public IHtmlContent LeadingHtml { get; set; }
        public HtmlAttributes HtmlAttributes { get; set; }
    }
}
