using System;
using System.Collections.Generic;
using System.Web;

using ChameleonForms.Component;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;
using ChameleonForms.FieldGenerators;
using ChameleonForms.FieldGenerators.Handlers;
using ChameleonForms.Generated;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChameleonForms.Templates.Default
{
    /// <summary>
    /// The default Chameleon Forms form template renderer.
    /// </summary>
    public class DefaultFormTemplate : IFormTemplate
    {
        /// <inheritdoc />
        public virtual void PrepareFieldConfiguration<TModel, T>(IFieldGenerator<TModel, T> fieldGenerator, IFieldGeneratorHandler<TModel, T> fieldGeneratorHandler, IFieldConfiguration fieldConfiguration, FieldParent fieldParent)
        {
        }

        /// <inheritdoc />
        public virtual IHtmlContent BeginForm(string action, FormMethod method, HtmlAttributes htmlAttributes, EncType? enctype)
        {
            return HtmlCreator.BuildFormTag(action, method, htmlAttributes, enctype);
        }

        /// <inheritdoc />
        public virtual IHtmlContent EndForm()
        {
            return DefaultHtmlHelpers.EndForm();
        }

        /// <inheritdoc />
        public virtual IHtmlContent BeginSection(IHtmlContent heading = null, IHtmlContent leadingHtml = null, HtmlAttributes htmlAttributes = null)
        {
            return DefaultHtmlHelpers.BeginSection(heading, leadingHtml, htmlAttributes ?? new HtmlAttributes());
        }

        /// <inheritdoc />
        public virtual IHtmlContent EndSection()
        {
            return DefaultHtmlHelpers.EndSection();
        }

        /// <inheritdoc />
        public virtual IHtmlContent BeginNestedSection(IHtmlContent heading = null, IHtmlContent leadingHtml = null, HtmlAttributes htmlAttributes = null)
        {
            return DefaultHtmlHelpers.BeginNestedSection(heading, leadingHtml, htmlAttributes ?? new HtmlAttributes());
        }

        /// <inheritdoc />
        public virtual IHtmlContent EndNestedSection()
        {
            return DefaultHtmlHelpers.EndNestedSection();
        }

        /// <inheritdoc />
        public virtual IHtmlContent Field(IHtmlContent labelHtml, IHtmlContent elementHtml, IHtmlContent validationHtml, ModelMetadata fieldMetadata, IReadonlyFieldConfiguration fieldConfiguration, bool isValid)
        {
            return DefaultHtmlHelpers.Field(labelHtml, elementHtml, validationHtml, fieldMetadata, fieldConfiguration, RequiredDesignator(fieldMetadata, fieldConfiguration, isValid));
        }

        /// <inheritdoc />
        public virtual IHtmlContent BeginField(IHtmlContent labelHtml, IHtmlContent elementHtml, IHtmlContent validationHtml, ModelMetadata fieldMetadata, IReadonlyFieldConfiguration fieldConfiguration, bool isValid)
        {
            return DefaultHtmlHelpers.BeginField(labelHtml, elementHtml, validationHtml, fieldMetadata, fieldConfiguration, RequiredDesignator(fieldMetadata, fieldConfiguration, isValid));
        }

        /// <summary>
        /// Creates the HTML for a required designator for a single form field (will only be output if the field is required).
        /// </summary>
        /// <param name="fieldMetadata">The metadata for the field being created</param>
        /// <param name="fieldConfiguration">Configuration for the field</param>
        /// <param name="isValid">Whether or not the field is valid</param>
        /// <returns>The HTML for the required designator of field with the given information</returns>
        protected virtual IHtmlContent RequiredDesignator(ModelMetadata fieldMetadata, IReadonlyFieldConfiguration fieldConfiguration, bool isValid)
        {
            return DefaultHtmlHelpers.RequiredDesignator();
        }

        /// <inheritdoc />
        public virtual IHtmlContent EndField()
        {
            return DefaultHtmlHelpers.EndField();
        }

        /// <inheritdoc />
        public virtual IHtmlContent BeginMessage(MessageType messageType, IHtmlContent heading, bool emptyHeading)
        {
            return DefaultHtmlHelpers.BeginMessage(messageType, heading, emptyHeading);
        }

        /// <inheritdoc />
        public virtual IHtmlContent EndMessage()
        {
            return DefaultHtmlHelpers.EndMessage();
        }

        /// <inheritdoc />
        public virtual IHtmlContent MessageParagraph(IHtmlContent paragraph)
        {
            return DefaultHtmlHelpers.MessageParagraph(paragraph);
        }

        /// <inheritdoc />
        public virtual IHtmlContent BeginNavigation()
        {
            return DefaultHtmlHelpers.BeginNavigation();
        }

        /// <inheritdoc />
        public virtual IHtmlContent EndNavigation()
        {
            return DefaultHtmlHelpers.EndNavigation();
        }

        /// <inheritdoc />
        /// <remarks>
        /// Uses an &lt;input&gt; by default so the submitted value works in IE7.
        /// See http://rommelsantor.com/clog/2012/03/12/fixing-the-ie7-submit-value/
        /// </remarks>
        public virtual IHtmlContent Button(IHtmlContent content, string type, string id, string value, HtmlAttributes htmlAttributes)
        {
            if (content == null && value == null)
                throw new ArgumentNullException("content", "Expected one of content or value to be specified");

            if (content == null)
                return HtmlCreator.BuildInput(id, value, type ?? "button", htmlAttributes);

            return HtmlCreator.BuildButton(content, type, id, value, htmlAttributes);
        }

        /// <inheritdoc />
        public virtual IHtmlContent RadioOrCheckboxList(IEnumerable<IHtmlContent> list, bool isCheckbox)
        {
            return DefaultHtmlHelpers.RadioOrCheckboxList(list);
        }
    }
}
