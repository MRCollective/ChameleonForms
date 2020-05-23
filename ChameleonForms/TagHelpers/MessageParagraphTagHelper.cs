using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ChameleonForms.Utils;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ChameleonForms.TagHelpers
{
    /// <summary>
    /// Creates a ChameleonForms form message paragraph, use within a ChameleonForm form message context.
    /// </summary>
    public class MessageParagraphTagHelper : ModelAwareTagHelper
    {
        /// <inheritdoc />
        public override async Task ProcessWhileAwareOfModelTypeAsync<TModel>(TagHelperContext context, TagHelperOutput output)
        {
            var helper = ViewContext.GetHtmlHelper<TModel>();
            var m = helper.GetChameleonFormsMessage();

            var childContent = await output.GetChildContentAsync();
            output.Content.SetHtmlContent(m.Paragraph(new HtmlString(childContent.GetContent(HtmlEncoder.Default))));
            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = null;
        }
    }
}
