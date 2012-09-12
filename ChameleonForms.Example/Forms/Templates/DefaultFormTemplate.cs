using System.Net.Http;
using System.Web;

namespace ChameleonForms.Example.Forms.Templates
{
    public class DefaultFormTemplate : IFormTemplate
    {
        public IHtmlString BeginForm(string action, HttpMethod method, string enctype)
        {
            return HtmlHelpers.BeginForm(action, method, enctype);
        }

        public IHtmlString EndForm()
        {
            return HtmlHelpers.EndForm();
        }

        public IHtmlString BeginSection(string title)
        {
            return HtmlHelpers.BeginSection(title);
        }

        public IHtmlString EndSection()
        {
            return HtmlHelpers.EndSection();
        }

        public IHtmlString Field(IHtmlString elementHtml, IHtmlString labelHtml)
        {
            return HtmlHelpers.Field(elementHtml, labelHtml);
        }
    }
}