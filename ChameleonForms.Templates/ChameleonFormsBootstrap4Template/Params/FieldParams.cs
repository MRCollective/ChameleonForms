#pragma warning disable 1591
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ChameleonForms.Templates.ChameleonFormsBootstrap4Template.Params
{
    public enum FieldRenderMode
    {
        Field,
        BeginField
    }

    public class FieldParams : FieldConfigurationParams
    {
        public FieldRenderMode RenderMode { get; set; }
        public IHtmlContent LabelHtml { get; set; }
        public IHtmlContent ElementHtml { get; set; }
        public IHtmlContent ValidationHtml { get; set; }
        public ModelMetadata FieldMetadata { get; set; }
        public IHtmlContent RequiredDesignator { get; set; }
        public bool IsValid { get; set; }
    }
}
