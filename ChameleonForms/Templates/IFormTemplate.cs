using System.Net.Http;
using System.Web;
using ChameleonForms.Enums;
using System.Web.Mvc;

namespace ChameleonForms.Templates
{
    public interface IFormTemplate
    {
        IHtmlString BeginForm(string action, FormMethod method, object htmlAttributes, EncType? encType);
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