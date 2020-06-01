using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ChameleonForms.TagHelpers
{
    /// <summary>
    /// Tag helper that accepts field configuration attributes for a <field />.
    /// </summary>
    [HtmlTargetElement("field")]
    public class FieldConfigurationTagHelper : TagHelper
    {
        /// <summary>
        /// Appended HTML as a <see cref="String"/>.
        /// </summary>
        public string Append { get; set; }

        /// <summary>
        /// Appended HTML as templated razor delegate.
        /// </summary>
        public Func<dynamic, IHtmlContent> AppendHtml { get; set; }

        /// <summary>
        /// Appended HTML as a <see cref="IHtmlContent"/>.
        /// </summary>
        public IHtmlContent AppendHtmlContent { get; set; }

        /// <summary>
        /// Hint as a <see cref="String"/>.
        /// </summary>
        public string Hint { get; set; }

        /// <summary>
        /// Hint HTML as templated razor delegate.
        /// </summary>
        public Func<dynamic, IHtmlContent> HintHtml { get; set; }

        /// <summary>
        /// Hint HTML as a <see cref="IHtmlContent"/>.
        /// </summary>
        public IHtmlContent HintHtmlContent { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var fc = context.GetFieldConfiguration();

            if (Append != null)
                fc.Append(Append.ToHtml());

            if (AppendHtml != null)
                fc.Append(AppendHtml);

            if (AppendHtmlContent != null)
                fc.Append(AppendHtmlContent);

            if (Hint != null)
                fc.WithHint(Hint);

            if (HintHtml != null)
                fc.WithHint(HintHtml);

            if (HintHtmlContent != null)
                fc.WithHint(HintHtmlContent);

            return Task.CompletedTask;
        }
    }
}
