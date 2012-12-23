using System.Web;
using System.Web.Mvc;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;

namespace ChameleonForms.Templates
{
    /// <summary>
    /// The default Chameleon Forms form template renderer.
    /// </summary>
    public class DefaultFormTemplate : IFormTemplate
    {
        public IHtmlString BeginForm(string action, FormMethod method, HtmlAttributes htmlAttributes, EncType? enctype)
        {
            return HtmlCreator.BuildFormTag(action, method, htmlAttributes, enctype);
        }

        public IHtmlString EndForm()
        {
            return HtmlHelpers.EndForm();
        }

        public IHtmlString BeginSection(IHtmlString title, IHtmlString leadingHtml, HtmlAttributes htmlAttributes)
        {
            return HtmlHelpers.BeginSection(title, leadingHtml, htmlAttributes);
        }

        public IHtmlString EndSection()
        {
            return HtmlHelpers.EndSection();
        }

        public IHtmlString BeginNestedSection(IHtmlString title, IHtmlString leadingHtml, HtmlAttributes htmlAttributes)
        {
            return HtmlHelpers.BeginNestedSection(title, leadingHtml, htmlAttributes);
        }

        public IHtmlString EndNestedSection()
        {
            return HtmlHelpers.EndNestedSection();
        }

        public IHtmlString Field(IHtmlString labelHtml, IHtmlString elementHtml, IHtmlString validationHtml, ModelMetadata fieldMetadata, IFieldConfiguration fieldConfiguration)
        {
            return HtmlHelpers.Field(labelHtml, elementHtml, validationHtml, fieldMetadata, fieldConfiguration);
        }

        public IHtmlString BeginField(IHtmlString labelHtml, IHtmlString elementHtml, IHtmlString validationHtml, ModelMetadata fieldMetadata, IFieldConfiguration fieldConfiguration)
        {
            return HtmlHelpers.BeginField(labelHtml, elementHtml, validationHtml, fieldMetadata, fieldConfiguration);
        }

        public IHtmlString EndField()
        {
            return HtmlHelpers.EndField();
        }

        public IHtmlString BeginMessage(MessageType messageType, IHtmlString heading)
        {
            return HtmlHelpers.BeginMessage(messageType, heading);
        }

        public IHtmlString EndMessage()
        {
            return HtmlHelpers.EndMessage();
        }

        public IHtmlString MessageParagraph(IHtmlString paragraph)
        {
            return HtmlHelpers.MessageParagraph(paragraph);
        }

        public IHtmlString BeginNavigation()
        {
            return HtmlHelpers.BeginNavigation();
        }

        public IHtmlString EndNavigation()
        {
            return HtmlHelpers.EndNavigation();
        }
    }
}