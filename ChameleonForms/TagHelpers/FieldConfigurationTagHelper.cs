using System;
using System.Collections.Generic;
using ChameleonForms.Component.Config;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ChameleonForms.TagHelpers
{
    /// <summary>
    /// Tag helper that accepts field configuration attributes.
    /// </summary>
    public abstract class FieldConfigurationTagHelper : ModelPropertyTagHelper
    {
        /// <summary>
        /// Field configuration.
        /// </summary>
        public IFieldConfiguration Configuration { get; set; }

        /// <summary>
        /// Class(es) to add to the field element.
        /// </summary>
        public string AddClass { get; set; }

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

        /// <summary>
        /// Placeholder text for the field.
        /// </summary>
        public string Placeholder { get; set; }

        [HtmlAttributeName("attrs", DictionaryAttributePrefix = "attr-")]
        public IDictionary<string, string> Attrs { get; set; } = new Dictionary<string, string>();
    }

    internal static class FieldConfigurationExtensions
    {
        public static IFieldConfiguration Configure(this IFieldConfiguration fc, FieldConfigurationTagHelper th)
        {
            if (th.Label != null)
                fc.Label(th.Label);

            if (th.LabelHtml != null)
                fc.Label(th.LabelHtml);

            if (th.LabelHtmlContent != null)
                fc.Label(th.LabelHtmlContent);

            if (th.InlineLabel != null)
                fc.InlineLabel(th.InlineLabel);

            if (th.InlineLabelHtml != null)
                fc.InlineLabel(th.InlineLabelHtml);

            if (th.InlineLabelHtmlContent != null)
                fc.InlineLabel(th.InlineLabelHtmlContent);

            if (th.AddClass != null)
                fc.AddClass(th.AddClass);

            if (th.Append != null)
                fc.Append(th.Append.ToHtml());

            if (th.AppendHtml != null)
                fc.Append(th.AppendHtml);

            if (th.AppendHtmlContent != null)
                fc.Append(th.AppendHtmlContent);

            if (th.Hint != null)
                fc.WithHint(th.Hint);

            if (th.HintHtml != null)
                fc.WithHint(th.HintHtml);

            if (th.HintHtmlContent != null)
                fc.WithHint(th.HintHtmlContent);

            if (th.Placeholder != null)
                fc.Placeholder(th.Placeholder);

            fc.Attrs(th.Attrs);

            return fc;
        }
    }
}
