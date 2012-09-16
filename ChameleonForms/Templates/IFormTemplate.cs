using System.Net.Http;
using System.Web;

namespace ChameleonForms.Templates
{
    public interface IFormTemplate
    {
        IHtmlString BeginForm(string action, HttpMethod method, string enctype);
        IHtmlString EndForm();
        IHtmlString BeginSection(string title);
        IHtmlString EndSection();
        IHtmlString BeginNestedSection(string title);
        IHtmlString EndNestedSection();
        IHtmlString Field(IHtmlString labelHtml, IHtmlString elementHtml, IHtmlString validationHtml);
        IHtmlString BeginField(IHtmlString labelHtml, IHtmlString elementHtml, IHtmlString validationHtml);
        IHtmlString EndField();
    }
}