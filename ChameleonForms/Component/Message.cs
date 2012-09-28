using System;
using System.Web;
using ChameleonForms.Enums;
using ChameleonForms.Templates;

namespace ChameleonForms.Component
{
    public class Message<TModel, TTemplate> : FormComponent<TModel, TTemplate> where TTemplate : IFormTemplate
    {
        private readonly MessageType _messageType;
        private readonly string _heading;

        public Message(IForm<TModel, TTemplate> form, MessageType messageType, string heading) : base(form, false)
        {
            _messageType = messageType;
            _heading = heading;
            Initialise();
        }

        public override IHtmlString Begin()
        {
            return Form.Template.BeginMessage(_messageType, _heading);
        }

        public override IHtmlString End()
        {
            return Form.Template.EndMessage();
        }

        public virtual IHtmlString Paragraph(string message)
        {
            return new HtmlString(string.Format("<p>{0}</p>{1}", HttpUtility.HtmlEncode(message), Environment.NewLine));
        }
    }

    public static class MessageExtensions
    {
        public static Message<TModel, TTemplate> BeginMessage<TModel, TTemplate>(this IForm<TModel, TTemplate> form, MessageType messageType, string heading) where TTemplate : IFormTemplate
        {
            return new Message<TModel, TTemplate>(form, messageType, heading);
        }
    }
}