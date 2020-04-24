#pragma warning disable 1591
using Microsoft.AspNetCore.Html;

namespace ChameleonForms.Templates.ChameleonFormsTwitterBootstrap3Template.Params
{
    public class LabelParams
    {
        public IHtmlContent Label { get; set; }
        public bool HasLabel { get; set; }
        public bool IsCheckboxControl { get; set; }
        public bool DisplayDesignator { get; set; }
        public bool IsRequired { get; set; }
        public IHtmlContent RequiredDesignator { get; set; }
    }
}
