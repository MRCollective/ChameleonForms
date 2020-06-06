using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ChameleonForms.TagHelpers
{
    /// <summary>
    /// Tag helper that accepts HTML attributes for a <form-section />, <form-button />, <submit-button /> and <reset-button />.
    /// </summary>
    [HtmlTargetElement("form-section")]
    [HtmlTargetElement("form-button")]
    [HtmlTargetElement("submit-button")]
    [HtmlTargetElement("reset-button")]
    public class HtmlAttributesTagHelper : TagHelper
    {
        /// <summary>
        /// Adds a HTML class.
        /// </summary>
        public string AddClass { get; set; }

        /// <summary>
        /// Sets the HTML id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Sets the element as disabled.
        /// </summary>
        public bool? Disabled { get; set; }

        /// <summary>
        /// HTML attributes to apply to the element. You can either pass them in as a dictionary (attrs="@dictionary"), or
        /// you can pass them in as individual attributes via attr-attribute-name="attributevalue" ...
        /// </summary>
        [HtmlAttributeName("attrs", DictionaryAttributePrefix = "attr-")]
        public IDictionary<string, string> Attrs { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Fluent configuration of the <see cref="HtmlAttributes"/>.
        /// </summary>
        public Func<HtmlAttributes, HtmlAttributes> FluentConfig { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var ha = context.GetHtmlAttributes();

            FluentConfig?.Invoke(ha);

            if (!string.IsNullOrWhiteSpace(AddClass))
                ha.AddClass(AddClass);

            if (!string.IsNullOrWhiteSpace(Id))
                ha.Id(Id);

            if (Disabled.HasValue)
                ha.Disabled(Disabled.Value);

            ha.Attrs(Attrs);

            return Task.CompletedTask;
        }
    }
}
