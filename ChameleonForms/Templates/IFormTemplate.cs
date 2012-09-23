using System.Net.Http;
using System.Web;
using ChameleonForms.Enums;

namespace ChameleonForms.Templates
{
    public interface IFormTemplate
    {
        IHtmlString BeginForm(string action, HttpMethod method, EncType? enctype);
        IHtmlString EndForm();
        IHtmlString BeginSection(string title, IHtmlString leadingHtml);
        IHtmlString EndSection();
        IHtmlString BeginNestedSection(string title, IHtmlString leadingHtml);
        IHtmlString EndNestedSection();
        IHtmlString Field(IHtmlString labelHtml, IHtmlString elementHtml, IHtmlString validationHtml);
        IHtmlString BeginField(IHtmlString labelHtml, IHtmlString elementHtml, IHtmlString validationHtml);
        IHtmlString EndField();
    }
}