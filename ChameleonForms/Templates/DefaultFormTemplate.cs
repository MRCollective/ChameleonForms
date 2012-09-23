using System.Net.Http;
using System.Web;
using ChameleonForms.Enums;

namespace ChameleonForms.Templates
{
    public class DefaultFormTemplate : IFormTemplate
    {
        public IHtmlString BeginForm(string action, HttpMethod method, EncType? enctype)
        {
            return HtmlHelpers.BeginForm(action, method, enctype);
        }

        public IHtmlString EndForm()
        {
            return HtmlHelpers.EndForm();
        }

        public IHtmlString BeginSection(string title, IHtmlString leadingHtml)
        {
            return HtmlHelpers.BeginSection(title, leadingHtml);
        }

        public IHtmlString EndSection()
        {
            return HtmlHelpers.EndSection();
        }

        public IHtmlString BeginNestedSection(string title, IHtmlString leadingHtml)
        {
            return HtmlHelpers.BeginNestedSection(title, leadingHtml);
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
    }
}