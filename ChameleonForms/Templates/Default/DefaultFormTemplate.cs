using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using ChameleonForms.Component;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;
using ChameleonForms.FieldGenerators;
using ChameleonForms.FieldGenerators.Handlers;

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
        public virtual IHtmlString BeginForm(string action, FormMethod method, HtmlAttributes htmlAttributes, EncType? enctype)
        {
            return HtmlCreator.BuildFormTag(action, method, htmlAttributes, enctype);
        }

        /// <inheritdoc />
        public virtual IHtmlString EndForm()
        {
            return DefaultHtmlHelpers.EndForm();
        }

        /// <inheritdoc />
        public virtual IHtmlString BeginSection(IHtmlString heading = null, IHtmlString leadingHtml = null, HtmlAttributes htmlAttributes = null)
        {
            return DefaultHtmlHelpers.BeginSection(heading, leadingHtml, htmlAttributes ?? new HtmlAttributes());
        }

        /// <inheritdoc />
        public virtual IHtmlString EndSection()
        {
            return DefaultHtmlHelpers.EndSection();
        }

        /// <inheritdoc />
        public virtual IHtmlString BeginNestedSection(IHtmlString heading = null, IHtmlString leadingHtml = null, HtmlAttributes htmlAttributes = null)
        {
            return DefaultHtmlHelpers.BeginNestedSection(heading, leadingHtml, htmlAttributes ?? new HtmlAttributes());
        }

        /// <inheritdoc />
        public virtual IHtmlString EndNestedSection()
        {
            return DefaultHtmlHelpers.EndNestedSection();
        }

        /// <inheritdoc />
        public virtual IHtmlString Field(IHtmlString labelHtml, IHtmlString elementHtml, IHtmlString validationHtml, ModelMetadata fieldMetadata, IReadonlyFieldConfiguration fieldConfiguration, bool isValid)
        {
            return DefaultHtmlHelpers.Field(labelHtml, elementHtml, validationHtml, fieldMetadata, fieldConfiguration, RequiredDesignator(fieldMetadata, fieldConfiguration, isValid));
        }

        /// <inheritdoc />
        public virtual IHtmlString BeginField(IHtmlString labelHtml, IHtmlString elementHtml, IHtmlString validationHtml, ModelMetadata fieldMetadata, IReadonlyFieldConfiguration fieldConfiguration, bool isValid)
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
        protected virtual IHtmlString RequiredDesignator(ModelMetadata fieldMetadata, IReadonlyFieldConfiguration fieldConfiguration, bool isValid)
        {
            return DefaultHtmlHelpers.RequiredDesignator();
        }

        /// <inheritdoc />
        public virtual IHtmlString EndField()
        {
            return DefaultHtmlHelpers.EndField();
        }

        /// <inheritdoc />
        public virtual IHtmlString BeginMessage(MessageType messageType, IHtmlString heading)
        {
            return DefaultHtmlHelpers.BeginMessage(messageType, heading);
        }

        /// <inheritdoc />
        public virtual IHtmlString EndMessage()
        {
            return DefaultHtmlHelpers.EndMessage();
        }

        /// <inheritdoc />
        public virtual IHtmlString MessageParagraph(IHtmlString paragraph)
        {
            return DefaultHtmlHelpers.MessageParagraph(paragraph);
        }

        /// <inheritdoc />
        public virtual IHtmlString BeginNavigation()
        {
            return DefaultHtmlHelpers.BeginNavigation();
        }

        /// <inheritdoc />
        public virtual IHtmlString EndNavigation()
        {
            return DefaultHtmlHelpers.EndNavigation();
        }

        /// <inheritdoc />
        /// <remarks>
        /// Uses an &lt;input&gt; by default so the submitted value works in IE7.
        /// See http://rommelsantor.com/clog/2012/03/12/fixing-the-ie7-submit-value/
        /// </remarks>
        public virtual IHtmlString Button(IHtmlString content, string type, string id, string value, HtmlAttributes htmlAttributes)
        {
            if (content == null && value == null)
                throw new ArgumentNullException("content", "Expected one of content or value to be specified");

            if (content == null)
                return HtmlCreator.BuildInput(id, value, type ?? "button", htmlAttributes);

            return HtmlCreator.BuildButton(content, type, id, value, htmlAttributes);
        }

        /// <inheritdoc />
        public virtual IHtmlString RadioOrCheckboxList(IEnumerable<IHtmlString> list, bool isCheckbox)
        {
            return DefaultHtmlHelpers.RadioOrCheckboxList(list);
        }
    }
}
