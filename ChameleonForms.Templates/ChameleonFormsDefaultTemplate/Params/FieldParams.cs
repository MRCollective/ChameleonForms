using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ChameleonForms.Templates.ChameleonFormsDefaultTemplate.Params
{
    internal enum FieldRenderMode
    {
        Field,
        BeginField
    }

    internal class FieldParams : FieldConfigurationParams
    {
        public FieldRenderMode RenderMode { get; set; }
        public IHtmlContent LabelHtml { get; set; }
        public IHtmlContent ElementHtml { get; set; }
        public IHtmlContent ValidationHtml { get; set; }
        public ModelMetadata FieldMetadata { get; set; }
        public IHtmlContent RequiredDesignator { get; set; }
    }
}
