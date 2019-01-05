using System.Collections.Generic;
using System.Web;
using ChameleonForms.Component;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;

using ChameleonForms.FieldGenerators;
using ChameleonForms.FieldGenerators.Handlers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChameleonForms.Templates
{
    /// <summary>
    /// A Chameleon Forms form template renderer.
    /// </summary>
    public interface IFormTemplate
    {
        /// <summary>
        /// Allows the template the modify the field configuration for a particular field.
        /// </summary>
        /// <typeparam name="TModel">The type of model the form is being displayed for</typeparam>
        /// <typeparam name="T">The type of the property the field is being generated against</typeparam>
        /// <param name="fieldGenerator">The instance of the field generator that will be used to generate the field</param>
        /// <param name="fieldGeneratorHandler">The instance of the field generator handler that will be used to generate the field element</param>
        /// <param name="fieldConfiguration">The field configuration that is being used to configure the field</param>
        /// <param name="fieldParent">The parent component of the field</param>
        void PrepareFieldConfiguration<TModel, T>(IFieldGenerator<TModel, T> fieldGenerator, IFieldGeneratorHandler<TModel, T> fieldGeneratorHandler, IFieldConfiguration fieldConfiguration, FieldParent fieldParent);

        /// <summary>
        /// Creates the starting HTML for a form.
        /// </summary>
        /// <param name="action">The form action</param>
        /// <param name="method">The form method</param>
        /// <param name="htmlAttributes">Any HTML attributes the form should use; specified as an anonymous object</param>
        /// <param name="enctype">The encoding type for the form</param>
        /// <returns>The starting HTML for a form</returns>
        IHtmlContent BeginForm(string action, FormMethod method, HtmlAttributes htmlAttributes, EncType? enctype);

        /// <summary>
        /// Creates the ending HTML for a form.
        /// </summary>
        /// <returns>The ending HTML for a form</returns>
        IHtmlContent EndForm();

        /// <summary>
        /// Creates the beginning HTML for a section.
        /// </summary>
        /// <param name="heading">The heading of the section</param>
        /// <param name="leadingHtml">Any HTML to output at the start of the section</param>
        /// <param name="htmlAttributes">Any HTML attributes the section container should use; specified as an anonymous object</param>
        /// <returns>The beginning HTML for a section</returns>
        IHtmlContent BeginSection(IHtmlContent heading = null, IHtmlContent leadingHtml = null, HtmlAttributes htmlAttributes = null);

        /// <summary>
        /// Creates the ending HTML for a section.
        /// </summary>
        /// <returns>The ending HTML for a section</returns>
        IHtmlContent EndSection();

        /// <summary>
        /// Creates the beginning HTML for a section that is nested within another section.
        /// </summary>
        /// <param name="heading">The heading of the nested section</param>
        /// <param name="leadingHtml">Any HTML to output at the start of the nested section</param>
        /// <param name="htmlAttributes">Any HTML attributes the nested section container should use; specified as an anaonymous object</param>
        /// <returns>The beginning HTML for a nested section</returns>
        IHtmlContent BeginNestedSection(IHtmlContent heading = null, IHtmlContent leadingHtml = null, HtmlAttributes htmlAttributes = null);

        /// <summary>
        /// Creates the ending HTML for a section that is nested within another section.
        /// </summary>
        /// <returns>The ending HTML for a nested section</returns>
        IHtmlContent EndNestedSection();

        /// <summary>
        /// Creates the HTML for a single form field.
        /// </summary>
        /// <param name="labelHtml">The HTML that comprises the form label</param>
        /// <param name="elementHtml">The HTML that comprieses the field itself</param>
        /// <param name="validationHtml">The HTML that comprises the field's validation messages</param>
        /// <param name="fieldMetadata">The metadata for the field being created</param>
        /// <param name="fieldConfiguration">Configuration for the field</param>
        /// <param name="isValid">Whether or not the field is valid</param>
        /// <returns>The HTML for the field</returns>
        IHtmlContent Field(IHtmlContent labelHtml, IHtmlContent elementHtml, IHtmlContent validationHtml, ModelMetadata fieldMetadata, IReadonlyFieldConfiguration fieldConfiguration, bool isValid);

        /// <summary>
        /// Creates the beginning HTML for a single form field that contains other fields nested within it.
        /// </summary>
        /// <param name="labelHtml">The HTML that comprises the form label</param>
        /// <param name="elementHtml">The HTML that comprieses the field itself</param>
        /// <param name="validationHtml">The HTML that comprises the field's validation messages</param>
        /// <param name="fieldMetadata">The metadata for the field being created</param>
        /// <param name="fieldConfiguration">Configuration for the field</param>
        /// <param name="isValid">Whether or not the field is valid</param>
        /// <returns>The beginning HTML for the parent field</returns>
        IHtmlContent BeginField(IHtmlContent labelHtml, IHtmlContent elementHtml, IHtmlContent validationHtml, ModelMetadata fieldMetadata, IReadonlyFieldConfiguration fieldConfiguration, bool isValid);

        /// <summary>
        /// Creates the ending HTML for a single form field that contains other fields nested within it.
        /// </summary>
        /// <returns>The ending HTML for the parent field</returns>
        IHtmlContent EndField();

        /// <summary>
        /// Creates the beginning HTML for a navigation section.
        /// </summary>
        /// <returns>The beginning HTML for a navigation section</returns>
        IHtmlContent BeginNavigation();

        /// <summary>
        /// Creates the ending HTML for a navigation section.
        /// </summary>
        /// <returns>The ending HTML for a navigation section</returns>
        IHtmlContent EndNavigation();

        /// <summary>
        /// Creates the beginning HTML for a message.
        /// </summary>
        /// <param name="messageType">The type of message being displayed</param>
        /// <param name="heading">The heading for the message</param>
        /// <returns>The beginning HTML for the message</returns>
        IHtmlContent BeginMessage(MessageType messageType, IHtmlContent heading, bool emptyHeading);

        /// <summary>
        /// Creates the ending HTML for a message.
        /// </summary>
        /// <returns>The ending HTML for the message</returns>
        IHtmlContent EndMessage();

        /// <summary>
        /// Creates the HTML for a paragraph in a message.
        /// </summary>
        /// <param name="paragraph">The paragraph HTML</param>
        /// <returns>The HTML for the message paragraph</returns>
        IHtmlContent MessageParagraph(IHtmlContent paragraph);

        /// <summary>
        /// Creates the HTML for a button.
        /// </summary>
        /// <param name="content">The content for the user to see or null if the value should be used instead</param>
        /// <param name="type">The type of button or null if a generic button should be used</param>
        /// <param name="id">The name/id of the button or null if one shouldn't be set</param>
        /// <param name="value">The value to submit if the button is clicked or null if one shouldn't be set</param>
        /// <param name="htmlAttributes">Any HTML attributes to add to the button or null if there are none</param>
        /// <returns>The HTML for the button</returns>
        IHtmlContent Button(IHtmlContent content, string type, string id, string value, HtmlAttributes htmlAttributes);

        /// <summary>
        /// Creates the HTML for a list of radio buttons or checkboxes.
        /// </summary>
        /// <param name="list">The list of HTML items (one per radio/checkbox)</param>
        /// <param name="isCheckbox">Whether the list is for checkboxes rather than radio buttons</param>
        /// <returns>The HTML for the radio list</returns>
        IHtmlContent RadioOrCheckboxList(IEnumerable<IHtmlContent> list, bool isCheckbox);
    }
}