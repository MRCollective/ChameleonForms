using System;
using System.Collections.Generic;
using ChameleonForms.Component;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;
using ChameleonForms.FieldGenerators;
using ChameleonForms.FieldGenerators.Handlers;
using ChameleonForms.Templates.ChameleonFormsDefaultTemplate;
using ChameleonForms.Templates.ChameleonFormsDefaultTemplate.Params;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorRenderer;

namespace ChameleonForms.Templates.Default
{
    // todo: Make the generated classes internal
    /// <summary>
    /// The default Chameleon Forms form template renderer.
    /// </summary>
    public class DefaultFormTemplate : IFormTemplate
    {
        /// <inheritdoc />
        public virtual void PrepareFieldConfiguration<TModel, T>(IFieldGenerator<TModel, T> fieldGenerator, IFieldGeneratorHandler<TModel, T> fieldGeneratorHandler,
            IFieldConfiguration fieldConfiguration, FieldParent fieldParent)
        {}

        /// <inheritdoc />
        public virtual IHtmlContent BeginForm(string action, FormMethod method, HtmlAttributes htmlAttributes, EncType? enctype)
        {
            return HtmlCreator.BuildFormTag(action, method, htmlAttributes, enctype);
        }

        /// <inheritdoc />
        public virtual IHtmlContent EndForm()
        {
            return new EndForm().Render((object)null);
        }

        /// <inheritdoc />
        public virtual IHtmlContent BeginSection(IHtmlContent heading = null, IHtmlContent leadingHtml = null,
            HtmlAttributes htmlAttributes = null)
        {
            return new BeginSection().Render(new BeginSectionParams{Heading = heading, HtmlAttributes = htmlAttributes, LeadingHtml = leadingHtml});
        }

        /// <inheritdoc />
        public virtual IHtmlContent EndSection()
        {
            return new EndSection().Render((object)null);
        }

        /// <inheritdoc />
        public virtual IHtmlContent BeginNestedSection(IHtmlContent heading = null, IHtmlContent leadingHtml = null,
            HtmlAttributes htmlAttributes = null)
        {
            return new BeginNestedSection().Render(new BeginSectionParams { Heading = heading, HtmlAttributes = htmlAttributes, LeadingHtml = leadingHtml });
        }

        /// <inheritdoc />
        public virtual IHtmlContent EndNestedSection()
        {
            return new EndNestedSection().Render((object)null);
        }

        /// <inheritdoc />
        public virtual IHtmlContent Field(IHtmlContent labelHtml, IHtmlContent elementHtml, IHtmlContent validationHtml,
            ModelMetadata fieldMetadata, IReadonlyFieldConfiguration fieldConfiguration, bool isValid)
        {
            var requiredDesignator = RequiredDesignator(fieldMetadata, fieldConfiguration, isValid);
            return new ChameleonFormsDefaultTemplate.Field().Render(new FieldParams()
            {
                RenderMode = FieldRenderMode.Field,
                FieldConfiguration = fieldConfiguration,
                ElementHtml = elementHtml,
                FieldMetadata = fieldMetadata,
                LabelHtml = labelHtml,
                RequiredDesignator = requiredDesignator,
                ValidationHtml = validationHtml
            });
        }

        /// <inheritdoc />
        public virtual IHtmlContent BeginField(IHtmlContent labelHtml, IHtmlContent elementHtml, IHtmlContent validationHtml,
            ModelMetadata fieldMetadata, IReadonlyFieldConfiguration fieldConfiguration, bool isValid)
        {
            var requiredDesignator = RequiredDesignator(fieldMetadata, fieldConfiguration, isValid);
            return new ChameleonFormsDefaultTemplate.Field().Render(new FieldParams()
            {
                RenderMode = FieldRenderMode.BeginField,
                FieldConfiguration = fieldConfiguration,
                ElementHtml = elementHtml,
                FieldMetadata = fieldMetadata,
                LabelHtml = labelHtml,
                RequiredDesignator = requiredDesignator,
                ValidationHtml = validationHtml
            });
        }

        /// <inheritdoc />
        public virtual IHtmlContent EndField()
        {
            return new EndField().Render((object)null);
        }

        /// <inheritdoc />
        public virtual IHtmlContent BeginNavigation()
        {
            return new BeginNavigation().Render((object)null);
        }

        /// <inheritdoc />
        public virtual IHtmlContent EndNavigation()
        {
            return new EndNavigation().Render((object)null);
        }

        /// <inheritdoc />
        public virtual IHtmlContent BeginMessage(MessageType messageType, IHtmlContent heading)
        {
            return new BeginMessage().Render(new MessageParams {Heading = heading, MessageType = messageType});
        }

        /// <inheritdoc />
        public virtual IHtmlContent EndMessage()
        {
            return new EndMessage().Render((object)null);
        }

        /// <inheritdoc />
        public virtual IHtmlContent MessageParagraph(IHtmlContent paragraph)
        {
            return new MessageParagraph().Render(new MessageParagraphParams {Paragraph = paragraph});
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
            return new RadioOrCheckboxList().Render(new ListParams {Items = list});
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
            return new RequiredDesignator().Render((object) null);
        }
    }
}
