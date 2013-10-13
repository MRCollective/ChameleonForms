using System;
using System.Web;
using System.Web.Mvc;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;
using ChameleonForms.FieldGenerators;
using ChameleonForms.FieldGenerators.Handlers;

namespace ChameleonForms.Templates
{
    /// <summary>
    /// The default Chameleon Forms form template renderer.
    /// </summary>
    public class DefaultFormTemplate : IFormTemplate
    {
        public virtual void PrepareFieldConfiguration<TModel, T>(IFieldGenerator<TModel, T> fieldGenerator, IFieldGeneratorHandler<TModel, T> fieldGeneratorHandler, IFieldConfiguration fieldConfiguration)
        {
        }

        public virtual IHtmlString BeginForm(string action, FormMethod method, HtmlAttributes htmlAttributes, EncType? enctype)
        {
            return HtmlCreator.BuildFormTag(action, method, htmlAttributes, enctype);
        }

        public virtual IHtmlString EndForm()
        {
            return HtmlHelpers.EndForm();
        }

        public virtual IHtmlString BeginSection(IHtmlString title, IHtmlString leadingHtml, HtmlAttributes htmlAttributes)
        {
            return HtmlHelpers.BeginSection(title, leadingHtml, htmlAttributes);
        }

        public virtual IHtmlString EndSection()
        {
            return HtmlHelpers.EndSection();
        }

        public virtual IHtmlString BeginNestedSection(IHtmlString title, IHtmlString leadingHtml, HtmlAttributes htmlAttributes)
        {
            return HtmlHelpers.BeginNestedSection(title, leadingHtml, htmlAttributes);
        }

        public virtual IHtmlString EndNestedSection()
        {
            return HtmlHelpers.EndNestedSection();
        }

        public virtual IHtmlString Field(IHtmlString labelHtml, IHtmlString elementHtml, IHtmlString validationHtml, ModelMetadata fieldMetadata, IReadonlyFieldConfiguration fieldConfiguration, bool isValid)
        {
            return HtmlHelpers.Field(labelHtml, elementHtml, validationHtml, fieldMetadata, fieldConfiguration, RequiredDesignator(fieldMetadata, fieldConfiguration, isValid));
        }

        public virtual IHtmlString BeginField(IHtmlString labelHtml, IHtmlString elementHtml, IHtmlString validationHtml, ModelMetadata fieldMetadata, IReadonlyFieldConfiguration fieldConfiguration, bool isValid)
        {
            return HtmlHelpers.BeginField(labelHtml, elementHtml, validationHtml, fieldMetadata, fieldConfiguration, RequiredDesignator(fieldMetadata, fieldConfiguration, isValid));
        }

        public virtual IHtmlString RequiredDesignator(ModelMetadata fieldMetadata, IReadonlyFieldConfiguration fieldConfiguration, bool isValid)
        {
            return HtmlHelpers.RequiredDesignator();
        }

        public virtual IHtmlString EndField()
        {
            return HtmlHelpers.EndField();
        }

        public virtual IHtmlString BeginMessage(MessageType messageType, IHtmlString heading)
        {
            return HtmlHelpers.BeginMessage(messageType, heading);
        }

        public virtual IHtmlString EndMessage()
        {
            return HtmlHelpers.EndMessage();
        }

        public virtual IHtmlString MessageParagraph(IHtmlString paragraph)
        {
            return HtmlHelpers.MessageParagraph(paragraph);
        }

        public virtual IHtmlString BeginNavigation()
        {
            return HtmlHelpers.BeginNavigation();
        }

        public virtual IHtmlString EndNavigation()
        {
            return HtmlHelpers.EndNavigation();
        }

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
    }
}
