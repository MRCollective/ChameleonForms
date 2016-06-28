using System.Web;

namespace ChameleonForms.Component
{
    public static class MessageExtensions
    {
        public static IHtml Paragraph<T>(this Message<T> message, IHtmlString paragraph)
        {
            return message.Paragraph(paragraph.ToIHtml());
        }
    }
}
