using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ChameleonForms.TagHelpers
{
    /// <summary>
    /// Tag helper that accepts field configuration attributes for a <field /> or <field-label />.
    /// </summary>
    [HtmlTargetElement("field")]
    [HtmlTargetElement("field-label")]
    public class FieldLabelConfigurationTagHelper : TagHelper
    {
        /// <summary>
        /// Label text override
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Label as templated razor delegate.
        /// </summary>
        public Func<dynamic, IHtmlContent> LabelHtml { get; set; }

        /// <summary>
        /// Label as a <see cref="IHtmlContent"/>.
        /// </summary>
        public IHtmlContent LabelHtmlContent { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var fc = context.GetFieldConfiguration();

            if (Label != null)
                fc.Label(Label);

            if (LabelHtml != null)
                fc.Label(LabelHtml);

            if (LabelHtmlContent != null)
                fc.Label(LabelHtmlContent);


            return Task.CompletedTask;
        }
    }
}
