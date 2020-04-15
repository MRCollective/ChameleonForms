using System.Threading.Tasks;
using ChameleonForms.Templates.ChameleonForms_DefaultTemplate;
using Microsoft.AspNetCore.Html;

namespace ChameleonForms.Templates
{
    public class DefaultTemplate
    {
        private readonly IRazorViewToStringRenderer _stringRenderer;

        public DefaultTemplate(IRazorViewToStringRenderer stringRenderer)
        {
            _stringRenderer = stringRenderer;
        }

        public Task<IHtmlContent> MessageParagraph(IHtmlContent paragraph)
        {
            return Render("MessageParagraph", new MessageParagraphParams { Paragraph = paragraph });
        }

        private async Task<IHtmlContent> Render<TModel>(string templateName, TModel model)
        {
            return new HtmlContentBuilder()
                .AppendHtml(await _stringRenderer.RenderViewToStringAsync(
                    $"{templateName}",
                    model
                ));
        }
    }
}
