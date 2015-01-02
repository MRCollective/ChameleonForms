﻿using System;
using System.Web;
using ChameleonForms.Enums;
using ChameleonForms.Templates;

namespace ChameleonForms.Component
{
    /// <summary>
    /// Wraps the output of a message to display to a user.
    /// </summary>
    /// <typeparam name="TModel">The view model type for the current view</typeparam>
    public class Message<TModel> : FormComponent<TModel>
    {
        private readonly MessageType _messageType;
        private readonly IHtmlString _heading;

        /// <summary>
        /// Creates a message.
        /// </summary>
        /// <param name="form">The form the message is being created in</param>
        /// <param name="messageType">The type of message to display</param>
        /// <param name="heading">The heading for the message</param>
        public Message(IForm<TModel> form, MessageType messageType, IHtmlString heading) : base(form, false)
        {
            _messageType = messageType;
            _heading = heading;
            Initialise();
        }

        /// <inheritdoc />
        public override IHtmlString Begin()
        {
            return Form.Template.BeginMessage(_messageType, _heading);
        }

        /// <inheritdoc />
        public override IHtmlString End()
        {
            return Form.Template.EndMessage();
        }

        /// <summary>
        /// Creates the HTML for a paragraph in the message.
        /// </summary>
        /// <param name="paragraph">The paragraph to output</param>
        /// <returns>The HTML for the paragraph</returns>
        public virtual IHtmlString Paragraph(string paragraph)
        {
            return Form.Template.MessageParagraph(paragraph.ToHtml());
        }

        /// <summary>
        /// Creates the HTML for a paragraph in the message.
        /// </summary>
        /// <param name="paragraph">The paragraph to output</param>
        /// <returns>The HTML for the paragraph</returns>
        public virtual IHtmlString Paragraph(IHtmlString paragraph)
        {
            return Form.Template.MessageParagraph(paragraph);
        }
    }

    /// <summary>
    /// Extension methods for the creation of messages.
    /// </summary>
    public static class MessageExtensions
    {
        /// <summary>
        /// Creates a message.
        /// </summary>
        /// <example>
        /// @using (var m = f.BeginMessage(MessageType.Success, "Your submission was successful")) {
        ///     @m.Paragraph(string.Format("Your item was successfully created with id {0}", Model.Id))
        /// }
        /// </example>
        /// <typeparam name="TModel">The view model type for the current view</typeparam>
        /// <param name="form">The form the message is being created in</param>
        /// <param name="messageType">The type of message to display</param>
        /// <param name="heading">The heading for the message</param>
        /// <returns>The message</returns>
        public static Message<TModel> BeginMessage<TModel>(this IForm<TModel> form, MessageType messageType, string heading = null)
        {
            return new Message<TModel>(form, messageType, heading.ToHtml());
        }
    }
}