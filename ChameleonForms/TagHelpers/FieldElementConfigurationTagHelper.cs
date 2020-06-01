using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ChameleonForms.TagHelpers
{
    /// <summary>
    /// Tag helper that accepts field configuration attributes for a <field /> or <field-element />.
    /// </summary>
    [HtmlTargetElement("field")]
    [HtmlTargetElement("field-element")]
    public class FieldElementConfigurationTagHelper : TagHelper
    {
        /// <summary>
        /// Class(es) to add to the field element.
        /// </summary>
        public string AddClass { get; set; }

        /// <summary>
        /// Inline label text override
        /// </summary>
        public string InlineLabel { get; set; }

        /// <summary>
        /// Inline label as templated razor delegate.
        /// </summary>
        public Func<dynamic, IHtmlContent> InlineLabelHtml { get; set; }

        /// <summary>
        /// Inline label as a <see cref="IHtmlContent"/>.
        /// </summary>
        public IHtmlContent InlineLabelHtmlContent { get; set; }

        /// <summary>
        /// Placeholder text for the field.
        /// </summary>
        public string Placeholder { get; set; }

        /// <summary>
        /// Arbitrary attributes to add to the field element. Add as a dictionary (attrs="@attrsDictionary") or as individual attributes (attr-id="id" attr-data-xyz="asdf").
        /// </summary>
        [HtmlAttributeName("attrs", DictionaryAttributePrefix = "attr-")]
        public IDictionary<string, string> Attrs { get; set; } = new Dictionary<string, string>();

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var fc = context.GetFieldConfiguration();

            if (InlineLabel != null)
                fc.InlineLabel(InlineLabel);

            if (InlineLabelHtml != null)
                fc.InlineLabel(InlineLabelHtml);

            if (InlineLabelHtmlContent != null)
                fc.InlineLabel(InlineLabelHtmlContent);

            if (AddClass != null)
                fc.AddClass(AddClass);

            if (Placeholder != null)
                fc.Placeholder(Placeholder);

            fc.Attrs(Attrs);

            return Task.CompletedTask;
        }
    }
}
