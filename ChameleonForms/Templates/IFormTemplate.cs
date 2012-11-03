using System.Web;
using ChameleonForms.Enums;
using System.Web.Mvc;

namespace ChameleonForms.Templates
{
    /// <summary>
    /// A Chameleon Forms form template renderer.
    /// </summary>
    public interface IFormTemplate
    {
        /// <summary>
        /// Creates the starting HTML for a form.
        /// </summary>
        /// <param name="action">The form action</param>
        /// <param name="method">The form method</param>
        /// <param name="htmlAttributes">Any HTML attributes the form should use; specified as an anonymous object</param>
        /// <param name="encType">The encoding type for the form</param>
        /// <returns>The starting HTML for a form</returns>
        IHtmlString BeginForm(string action, FormMethod method, object htmlAttributes, EncType? encType);

        /// <summary>
        /// Creates the ending HTML for a form.
        /// </summary>
        /// <returns>The ending HTML for a form</returns>
        IHtmlString EndForm();

        /// <summary>
        /// Creates the beginning HTML for a section.
        /// </summary>
        /// <param name="title">The title of the section</param>
        /// <param name="leadingHtml">Any HTML to output at the start of the section</param>
        /// <param name="htmlAttributes">Any HTML attributes the section container should use; specified as an anonymous object</param>
        /// <returns>The beginning HTML for a section</returns>
        IHtmlString BeginSection(string title, IHtmlString leadingHtml, object htmlAttributes);

        /// <summary>
        /// Creates the ending HTML for a section.
        /// </summary>
        /// <returns>The ending HTML for a section</returns>
        IHtmlString EndSection();

        /// <summary>
        /// Creates the beginning HTML for a section that is nested within another section.
        /// </summary>
        /// <param name="title">The title of the nested section</param>
        /// <param name="leadingHtml">Any HTML to output at the start of the nested section</param>
        /// <param name="htmlAttributes">Any HTML attributes the nested section container should use; specified as an anaonymous object</param>
        /// <returns>The beginning HTML for a nested section</returns>
        IHtmlString BeginNestedSection(string title, IHtmlString leadingHtml, object htmlAttributes);

        /// <summary>
        /// Creates the ending HTML for a section that is nested within another section.
        /// </summary>
        /// <returns>The ending HTML for a nested section</returns>
        IHtmlString EndNestedSection();

        /// <summary>
        /// Creates the HTML for a single form field.
        /// </summary>
        /// <param name="labelHtml">The HTML that comprises the form label</param>
        /// <param name="elementHtml">The HTML that comprieses the field itself</param>
        /// <param name="validationHtml">The HTML that comprises the field's validation messages</param>
        /// <param name="fieldMetadata">The metadata for the field being created</param>
        /// <returns>The HTML for the field</returns>
        IHtmlString Field(IHtmlString labelHtml, IHtmlString elementHtml, IHtmlString validationHtml, ModelMetadata fieldMetadata);

        /// <summary>
        /// Creates the beginning HTML for a single form field that contains other fields nested within it.
        /// </summary>
        /// <param name="labelHtml">The HTML that comprises the form label</param>
        /// <param name="elementHtml">The HTML that comprieses the field itself</param>
        /// <param name="validationHtml">The HTML that comprises the field's validation messages</param>
        /// <param name="fieldMetadata">The metadata for the field being created</param>
        /// <returns>The beginning HTML for the parent field</returns>
        IHtmlString BeginField(IHtmlString labelHtml, IHtmlString elementHtml, IHtmlString validationHtml, ModelMetadata fieldMetadata);

        /// <summary>
        /// Creates the ending HTML for a single form field that contains other fields nested within it.
        /// </summary>
        /// <returns>The ending HTML for the parent field</returns>
        IHtmlString EndField();

        /// <summary>
        /// Creates the beginning HTML for a navigation section.
        /// </summary>
        /// <returns>The beginning HTML for a navigation section</returns>
        IHtmlString BeginNavigation();

        /// <summary>
        /// Creates the ending HTML for a navigation section.
        /// </summary>
        /// <returns>The ending HTML for a navigation section</returns>
        IHtmlString EndNavigation();

        /// <summary>
        /// Creates the beginning HTML for a message.
        /// </summary>
        /// <param name="messageType">The type of message being displayed</param>
        /// <param name="heading">The heading for the message</param>
        /// <returns>The beginning HTML for the message</returns>
        IHtmlString BeginMessage(MessageType messageType, string heading);

        /// <summary>
        /// Creates the ending HTML for a message.
        /// </summary>
        /// <returns>The ending HTML for the message</returns>
        IHtmlString EndMessage();
    }
}