using System.Net.Http;
using System.Web;
using ChameleonForms.Templates;

namespace ChameleonForms.Tests.Helpers
{
    public class DummyFormTemplate : IFormTemplate
    {
        public IHtmlString BeginForm(string action, HttpMethod method, string enctype)
        {
            return new HtmlString(string.Format("BeginForm|{0}|{1}|{2}", action, method.Method.ToLower(), enctype));
        }

        public IHtmlString EndForm()
        {
            return new HtmlString("EndForm");
        }

        public IHtmlString BeginSection(string title)
        {
            return new HtmlString(string.Format("BeginSection|{0}", title));
        }

        public IHtmlString EndSection()
        {
            return new HtmlString("EndSection");
        }

        public IHtmlString Field(IHtmlString elementHtml, IHtmlString labelHtml, IHtmlString validationHtml)
        {
            return new HtmlString(string.Format("Field|{0}|{1}|{2}", elementHtml, labelHtml, validationHtml));
        }
    }
}
