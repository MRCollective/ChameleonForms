using System.Web;
using System.Web.Mvc;
using ChameleonForms.Enums;

namespace ChameleonForms.Templates
{
    /// <summary>
    /// The default Chameleon Forms form template renderer.
    /// </summary>
    public class DefaultFormTemplate : IFormTemplate
    {
        public IHtmlString BeginForm(string action, FormMethod method, object htmlAttributes, EncType? encType)
        {
            return HtmlHelpers.BeginForm(action, method, htmlAttributes, encType);
        }

        public IHtmlString EndForm()
        {
            return HtmlHelpers.EndForm();
        }

        public IHtmlString BeginSection(string title, IHtmlString leadingHtml, object htmlAttributes)
        {
            return HtmlHelpers.BeginSection(title, leadingHtml, htmlAttributes);
        }

        public IHtmlString EndSection()
        {
            return HtmlHelpers.EndSection();
        }

        public IHtmlString BeginNestedSection(string title, IHtmlString leadingHtml, object htmlAttributes)
        {
            return HtmlHelpers.BeginNestedSection(title, leadingHtml, htmlAttributes);
        }

        public IHtmlString EndNestedSection()
        {
            return HtmlHelpers.EndNestedSection();
        }

        public IHtmlString Field(IHtmlString labelHtml, IHtmlString elementHtml, IHtmlString validationHtml)
        {
            return HtmlHelpers.Field(labelHtml, elementHtml, validationHtml);
        }

        public IHtmlString BeginField(IHtmlString labelHtml, IHtmlString elementHtml, IHtmlString validationHtml)
        {
            return HtmlHelpers.BeginField(labelHtml, elementHtml, validationHtml);
        }

        public IHtmlString EndField()
        {
            return HtmlHelpers.EndField();
        }

        public IHtmlString BeginMessage(MessageType messageType, string heading)
        {
            return HtmlHelpers.BeginMessage(messageType, heading);
        }

        public IHtmlString EndMessage()
        {
            return HtmlHelpers.EndMessage();
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