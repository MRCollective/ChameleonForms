using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ChameleonForms.Component;
using ChameleonForms.Utils;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ChameleonForms.TagHelpers
{
    /// <summary>
    /// Creates a ChameleonForms form section context, use within a ChameleonForm form context.
    /// </summary>
    public class FormSectionTagHelper : ModelAwareTagHelper
    {
        /// <summary>
        /// Section heading as a <see cref="String"/>.
        /// </summary>
        public string Heading { get; set; }

        /// <summary>
        /// Section heading as templated razor delegate.
        /// </summary>
        public Func<dynamic, IHtmlContent> HeadingHtml { get; set; }

        /// <summary>
        /// Section heading as a <see cref="IHtmlContent"/>.
        /// </summary>
        public IHtmlContent HeadingHtmlContent { get; set; }

        /// <summary>
        /// Leading HTML for the section, provided via a templated razor delegate.
        /// </summary>
        public Func<dynamic, IHtmlContent> LeadingHtml { get; set; }

        /// <summary>
        /// Leading HTML for the section, provided as an <see cref="IHtmlContent"/>.
        /// </summary>
        public IHtmlContent LeadingHtmlContent { get; set; }

        /// <inheritdoc />
        public override async Task ProcessWhileAwareOfModelTypeAsync<TModel>(TagHelperContext context, TagHelperOutput output)
        {
            var helper = ViewContext.GetHtmlHelper<TModel>();

            var leadingHtml = LeadingHtml?.Invoke(null) ?? LeadingHtmlContent;
            var heading = Heading != null ? Heading.ToHtml() : (HeadingHtml?.Invoke(null) ?? HeadingHtmlContent);

            if (helper.IsInChameleonFormsSection())
            {
                var s = helper.GetChameleonFormsSection();
                using (s.BeginSection(heading: heading, leadingHtml: leadingHtml, context.GetHtmlAttributes()))
                {
                    var childContent = await output.GetChildContentAsync();
                    childContent.WriteTo(helper.ViewContext.Writer, HtmlEncoder.Default);
                }
            }
            else
            {
                var f = helper.GetChameleonForm();
                using (f.BeginSection(heading: heading, leadingHtml: leadingHtml, context.GetHtmlAttributes()))
                {
                    var childContent = await output.GetChildContentAsync();
                    childContent.WriteTo(helper.ViewContext.Writer, HtmlEncoder.Default);
                }
            }

            output.SuppressOutput();
        }
    }
}
