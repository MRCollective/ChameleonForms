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

        /// <summary>
        /// Don't use a &lt;label&gt;, but still include the label text for the field.
        /// </summary>
        public bool WithoutLabelElement { get; set; }

        /// <summary>
        /// Specify one or more CSS classes to use for the field label.
        /// </summary>
        public string AddLabelClass { get; set; }

        /// <summary>
        /// Called when the tag helper is being processed.
        /// </summary>
        /// <param name="context">The context within which the tag helper is processed</param>
        /// <param name="output">The output from the tag helper</param>
        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var fc = context.GetFieldConfiguration();

            if (Label != null)
                fc.Label(Label);

            if (LabelHtml != null)
                fc.Label(LabelHtml);

            if (LabelHtmlContent != null)
                fc.Label(LabelHtmlContent);

            if (WithoutLabelElement)
                fc.WithoutLabelElement();

            if (!string.IsNullOrWhiteSpace(AddLabelClass))
                fc.AddLabelClass(AddLabelClass);

            return Task.CompletedTask;
        }
    }
}
