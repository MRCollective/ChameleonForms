using ChameleonForms.Enums;
using Microsoft.AspNetCore.Html;

namespace ChameleonForms.Templates.ChameleonFormsDefaultTemplate.Params
{
    internal class MessageParams
    {
        public MessageType MessageType { get; set; }
        public IHtmlContent Heading { get; set; }
    }
}
