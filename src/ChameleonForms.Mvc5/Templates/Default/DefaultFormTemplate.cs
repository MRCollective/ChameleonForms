using System;
using System.Collections.Generic;
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
        public virtual IHtml BeginForm(string action, FormSubmitMethod method, HtmlAttributes htmlAttributes, EncType? enctype)
        {
            return HtmlCreator.BuildFormTag(action, method, htmlAttributes, enctype);
        }

        /// <inheritdoc />
        public virtual IHtml EndForm()
        {
            return DefaultHtmlHelpers.EndForm().ToIHtml();
        }

        /// <inheritdoc />
        public virtual IHtml BeginSection(IHtml heading = null, IHtml leadingHtml = null, HtmlAttributes htmlAttributes = null)
        {
            return DefaultHtmlHelpers.BeginSection(heading.ToIHtmlString(), leadingHtml.ToIHtmlString(), htmlAttributes ?? new HtmlAttributes()).ToIHtml();
        }

        /// <inheritdoc />
        public virtual IHtml EndSection()
        {
            return DefaultHtmlHelpers.EndSection().ToIHtml();
        }

        /// <inheritdoc />
        public virtual IHtml BeginNestedSection(IHtml heading = null, IHtml leadingHtml = null, HtmlAttributes htmlAttributes = null)
        {
            return DefaultHtmlHelpers.BeginNestedSection(heading.ToIHtmlString(), leadingHtml.ToIHtmlString(), htmlAttributes ?? new HtmlAttributes()).ToIHtml();
        }

        /// <inheritdoc />
        public virtual IHtml EndNestedSection()
        {
            return DefaultHtmlHelpers.EndNestedSection().ToIHtml();
        }

        /// <inheritdoc />
        public virtual IHtml Field(IHtml labelHtml, IHtml elementHtml, IHtml validationHtml, IFieldMetadata fieldMetadata, IReadonlyFieldConfiguration fieldConfiguration, bool isValid)
        {
            return DefaultHtmlHelpers.Field(labelHtml.ToIHtmlString(), elementHtml.ToIHtmlString(), validationHtml.ToIHtmlString(), fieldMetadata, fieldConfiguration, RequiredDesignator(fieldMetadata, fieldConfiguration, isValid).ToIHtmlString()).ToIHtml();
        }

        /// <inheritdoc />
        public virtual IHtml BeginField(IHtml labelHtml, IHtml elementHtml, IHtml validationHtml, IFieldMetadata fieldMetadata, IReadonlyFieldConfiguration fieldConfiguration, bool isValid)
        {
            return DefaultHtmlHelpers.BeginField(labelHtml.ToIHtmlString(), elementHtml.ToIHtmlString(), validationHtml.ToIHtmlString(), fieldMetadata, fieldConfiguration, RequiredDesignator(fieldMetadata, fieldConfiguration, isValid).ToIHtmlString()).ToIHtml();
        }

        /// <summary>
        /// Creates the HTML for a required designator for a single form field (will only be output if the field is required).
        /// </summary>
        /// <param name="fieldMetadata">The metadata for the field being created</param>
        /// <param name="fieldConfiguration">Configuration for the field</param>
        /// <param name="isValid">Whether or not the field is valid</param>
        /// <returns>The HTML for the required designator of field with the given information</returns>
        protected virtual IHtml RequiredDesignator(IFieldMetadata fieldMetadata, IReadonlyFieldConfiguration fieldConfiguration, bool isValid)
        {
            return DefaultHtmlHelpers.RequiredDesignator().ToIHtml();
        }

        /// <inheritdoc />
        public virtual IHtml EndField()
        {
            return DefaultHtmlHelpers.EndField().ToIHtml();
        }

        /// <inheritdoc />
        public virtual IHtml BeginMessage(MessageType messageType, IHtml heading)
        {
            return DefaultHtmlHelpers.BeginMessage(messageType, heading.ToIHtmlString()).ToIHtml();
        }

        /// <inheritdoc />
        public virtual IHtml EndMessage()
        {
            return DefaultHtmlHelpers.EndMessage().ToIHtml();
        }

        /// <inheritdoc />
        public virtual IHtml MessageParagraph(IHtml paragraph)
        {
            return DefaultHtmlHelpers.MessageParagraph(paragraph.ToIHtmlString()).ToIHtml();
        }

        /// <inheritdoc />
        public virtual IHtml BeginNavigation()
        {
            return DefaultHtmlHelpers.BeginNavigation().ToIHtml();
        }

        /// <inheritdoc />
        public virtual IHtml EndNavigation()
        {
            return DefaultHtmlHelpers.EndNavigation().ToIHtml();
        }

        /// <inheritdoc />
        /// <remarks>
        /// Uses an &lt;input&gt; by default so the submitted value works in IE7.
        /// See http://rommelsantor.com/clog/2012/03/12/fixing-the-ie7-submit-value/
        /// </remarks>
        public virtual IHtml Button(IHtml content, string type, string id, string value, HtmlAttributes htmlAttributes)
        {
            if (content == null && value == null)
                throw new ArgumentNullException("content", "Expected one of content or value to be specified");

            if (content == null)
                return HtmlCreator.BuildInput(id, value, type ?? "button", htmlAttributes);

            return HtmlCreator.BuildButton(content, type, id, value, htmlAttributes);
        }

        /// <inheritdoc />
        public virtual IHtml RadioOrCheckboxList(IEnumerable<IHtml> list, bool isCheckbox)
        {
            return DefaultHtmlHelpers.RadioOrCheckboxList(list).ToIHtml();
        }
    }
}
