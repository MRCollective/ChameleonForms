using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ChameleonForms.Component;
using ChameleonForms.Enums;
using ChameleonForms.Utils;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ChameleonForms.TagHelpers
{
    /// <summary>
    /// Creates a ChameleonForms form message context, use within a ChameleonForm form context.
    /// </summary>
    public class FormMessageTagHelper : ModelAwareTagHelper
    {
        /// <summary>
        /// Message type.
        /// </summary>
        public MessageType Type { get; set; }

        /// <summary>
        /// Message heading as a <see cref="String"/>.
        /// </summary>
        public string Heading { get; set; }

        /// <summary>
        /// Message heading as templated razor delegate.
        /// </summary>
        public Func<dynamic, IHtmlContent> HeadingHtml { get; set; }

        /// <summary>
        /// Message heading as a <see cref="IHtmlContent"/>.
        /// </summary>
        public IHtmlContent HeadingHtmlContent { get; set; }

        /// <inheritdoc />
        public override async Task ProcessWhileAwareOfModelTypeAsync<TModel>(TagHelperContext context, TagHelperOutput output)
        {
            var helper = ViewContext.GetHtmlHelper<TModel>();

            var heading = Heading != null ? Heading.ToHtml() : (HeadingHtml?.Invoke(null) ?? HeadingHtmlContent);

            var f = helper.GetChameleonForm();
            using (f.BeginMessage(heading: heading, messageType: Type))
            {
                var childContent = await output.GetChildContentAsync();
                childContent.WriteTo(helper.ViewContext.Writer, HtmlEncoder.Default);
            }

            output.SuppressOutput();
        }
    }
}
